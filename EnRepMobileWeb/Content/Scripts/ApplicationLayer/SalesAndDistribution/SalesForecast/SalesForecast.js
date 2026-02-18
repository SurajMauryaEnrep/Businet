$(document).ready(function () {
    debugger;
    
    $("#txtTargetSaleQuantity").attr("autocomplete", "off");
    $("#divUpdate").hide();    
    BindItemNameDDL();
    $("#datatable-buttons1 tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var sf_id = clickedrow.children("#hdn_sf_id").text();
            //var f_freq = clickedrow.children("#hdn_f_freq").text();
            //var f_fy = clickedrow.children("#hdn_f_fy").text();
            //var f_period = clickedrow.children("#hdn_f_period1").text();
            //var f_status = clickedrow.children("#hdn_f_status1").text();
            //window.location = "/ApplicationLayer/SalesForecast/dbClickEdit/?f_freq=" + f_freq + "&f_fy=" + f_fy + "&f_period=" + f_period + "&f_status=" + f_status;
            window.location = "/ApplicationLayer/SalesForecast/dbClickEdit/?sf_id=" + sf_id + "&WF_Status=" + WF_Status;// + "&f_fy=" + f_fy + "&f_period=" + f_period + "&f_status=" + f_status;
        }
        catch (err) {
            debugger;
        }
    });
    $("#datatable-buttons1 >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        //var f_fy = clickedrow.children("#hdn_f_fy").text();
        //var f_period = clickedrow.children("#hdn_f_period1").text();
        //SFCNo = f_fy + f_period;
        //SFCNo = SFCNo.replaceAll(',', '').replaceAll('-', '')
        var SFCNo = clickedrow.children("#hdn_sf_id").text();
        var SfCDate = clickedrow.children("#hdn_createdate").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        //$("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        //$(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SFCNo, SfCDate, Doc_id, Doc_Status);
        $("#hdDoc_No").val(SFCNo);
    });

    
    //var sfrNo1 = $("#ddl_financial_year").val();
    //var sfrNo2 = $("#ddl_period").val();
    var sf_id = $("#hdn_sf_id").val();
    //Doc_no = (sfrNo1 + ',' + sfrNo2);
    //Doc_no = Doc_no.replaceAll(',', '').replaceAll('-', '')

    $("#hdDoc_No").val(sf_id);
    //ListRowHighLight();
    txtSalePriceEnblDsbl();
});
function CmnGetWorkFlowDetails(e) {

}
function ddl_f_frequency_onchange(e) {
    $('#ddl_financial_year').val('0');
}
function BindItemNameDDL() {
    debugger;
    $("#ddl_ItemName").append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl').append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL("#datatable-buttons", "#ddl_ItemName", "", "#hdn_item_id", "listToHide", "SF")
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/SalesForecast/GetItemNameInDDL",
    //        data: function (params) {
    //            var queryParameters = {
    //                SO_ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                SFC_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    sessionStorage.removeItem("PLitemList");
    //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
    //                    $('#ddl_ItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                        //HideItemType_AfterAdd(arr.Table[i].Item_id);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ddl_ItemName').select2({
    //                        templateResult: function (data) {
    //                            var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    HideItemType_AfterAdd();
    //                }
    //            }
    //        },
    //    });
}
function HideItemType_AfterAdd() {
    debugger;
    $("#datatable-buttons >tbody >tr").each(function (j, rows) {
        var currentRowChild = $(this);
        var hdn_item_id = currentRowChild.find("#hdn_item_id").val();
        $("#ddl_ItemName option[value=" + hdn_item_id + "]").select2().hide();
        $('#ddl_ItemName').val("0").change();
    });
    $("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
    //if (item_id != '') {

    //    if (ddlItemType_ID == "O") {
    //        $("#ddl_ItemName option[value=" + ddl_Item_id + "]").select2().hide();
    //        $('#ddl_ItemName').val("0").change();
    //        //$("#ddl_ItemName option[value=" + item_id + "]").hide();
    //        //$('#ddl_ItemName').val("0").change();
    //    }
    //    else {
    //        $('#ddl_ItemName').val("0");
    //        $('#ddl_ItemName').val("0").change();
    //    }

    //}
}
function ddl_ItemName_onchange() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    $("#vmddl_item_id").css("display", "none");
    //$("#vm_txtTargetSaleQuantity").css("display", "none");
    disableFyPeriod();
    resetitemdetail();
    var Itm_ID = $("#ddl_ItemName").val();
    if (Itm_ID != "0") {
        $("#vmddl_item_id").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");      
    }
   
    $("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
    
    var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
    if (txtIncreaseByInPercentage == "")
    {
        //txtIncreaseByInPercentage = parseFloat(0).toFixed(RateDigit);
        txtIncreaseByInPercentage = "";
        if (isNaN(txtIncreaseByInPercentage)) {
            //txtIncreaseByInPercentage = parseFloat(0).toFixed(RateDigit);
            txtIncreaseByInPercentage = "";
            $("#txtIncreaseByInPercentage").val(txtIncreaseByInPercentage);
        }
        else {
            $("#txtIncreaseByInPercentage").val(txtIncreaseByInPercentage);
        }
    }
    var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
    if (txtReducedByInPercentage == "") {
        //txtReducedByInPercentage = parseFloat(0).toFixed(RateDigit);
        txtReducedByInPercentage = "";
        if (isNaN(txtReducedByInPercentage)) {
            //txtReducedByInPercentage = parseFloat(0).toFixed(RateDigit);
            txtReducedByInPercentage = "";
            $("#txtReducedByInPercentage").val(txtReducedByInPercentage);
        }
        else {
            $("#txtReducedByInPercentage").val(txtReducedByInPercentage);
        }
    }

    var txtTargetSalesValue = $("#txtTargetSalesValue").val();
    if (txtTargetSalesValue == "") {
        //txtTargetSalesValue = parseFloat(0).toFixed(ValDigit);
        txtTargetSalesValue = "";
        if (isNaN(txtTargetSalesValue)) {
            //txtTargetSalesValue = parseFloat(0).toFixed(ValDigit);
            txtTargetSalesValue = "";
            $("#txtTargetSalesValue").val(txtTargetSalesValue);
        }
        else {
            $("#txtTargetSalesValue").val(txtTargetSalesValue);
        }
    }
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    if (Itm_ID != null) {
        if (Itm_ID != null) {
            $("#hdn_item_id").val(Itm_ID);
        }
        else if (Itm_ID == null) {
            Itm_ID = $("#hdn_item_id").val();
        }

        var fromdate = $("#txtFromDate").val();
        var todate = $("#txtToDate").val();

        if (Itm_ID != "") {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/SalesForecast/GetSOItemUOM",
                    data: {
                        Itm_ID: Itm_ID,
                        fromdate: fromdate,
                        todate: todate,
                    },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            SFC_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var arr = [];
                            arr = JSON.parse(data);
                            debugger;
                            if (arr.Table.length > 0) {
                                $("#UOM").val(arr.Table[0].uom_alias);
                                $("#UOMID").val(arr.Table[0].uom_id);
                                debugger
                                try {
                                    HideShowPageWise(arr.Table[0].sub_item);
                                }
                                catch (ex) {
                                    //console.log(ex);
                                }
                            }
                            else {
                                $("#UOM").val("");
                                $("#UOMID").val("");
                                //clickedrow.find("#UOM").val("");
                            }
                            if (arr.Table1.length > 0) {
                                var txtPreviousYearSalesInQuantity = (parseFloat(arr.Table1[0].ship_qty).toFixed(QtyDecDigit));
                                $("#txtPreviousYearSalesInQuantity").val(txtPreviousYearSalesInQuantity);//.val(arr.Table1[0].ship_qty).toFixed(QtyDecDigit);
                                var txtPreviousYearSalesInAmount = (parseFloat(arr.Table1[0].gr_val).toFixed(ValDigit));
                                $("#txtPreviousYearSalesInAmount").val(txtPreviousYearSalesInAmount);
                                //$("#txtSalesPrice").val(arr.Table1[0].sale_price);
                                $("#txtIncreaseByInPercentage").prop("disabled", false);
                                $("#txtReducedByInPercentage").prop("disabled", false);
                            }
                            else {
                                var txtPreviousYearSalesInQuantity = (parseFloat(0).toFixed(QtyDecDigit));
                                $("#txtPreviousYearSalesInQuantity").val(txtPreviousYearSalesInQuantity);
                                var txtPreviousYearSalesInAmount = (parseFloat(0).toFixed(ValDigit));
                                $("#txtPreviousYearSalesInAmount").val(txtPreviousYearSalesInAmount);

                                $("#txtIncreaseByInPercentage").prop("disabled", true);
                                $("#txtReducedByInPercentage").prop("disabled", true);
                                //$("#txtSalesPrice").val('');
                            }
                            if (arr.Table2.length > 0) {

                                var hdn_actual_sale_qty = (parseFloat(arr.Table2[0].ship_qty).toFixed(QtyDecDigit));
                                var hdn_actual_sale_value = (parseFloat(arr.Table2[0].gr_val).toFixed(ValDigit));
                                if (isNaN(hdn_actual_sale_qty) && isNaN(hdn_actual_sale_value)) {
                                    var hdn_actual_sale_qty = (parseFloat(0).toFixed(QtyDecDigit));
                                    $("#hdn_actual_sale_qty").val(hdn_actual_sale_qty);
                                    var hdn_actual_sale_value = (parseFloat(0).toFixed(ValDigit));
                                    $("#hdn_actual_sale_value").val(hdn_actual_sale_value);
                                }
                                else {
                                    $("#hdn_actual_sale_qty").val(hdn_actual_sale_qty);
                                    $("#hdn_actual_sale_value").val(hdn_actual_sale_value);
                                }
                            }
                            else {
                                var hdn_actual_sale_qty = (parseFloat(0).toFixed(QtyDecDigit));
                                $("#hdn_actual_sale_qty").val(hdn_actual_sale_qty);
                                var hdn_actual_sale_value = (parseFloat(0).toFixed(ValDigit));
                                $("#hdn_actual_sale_value").val(hdn_actual_sale_value);
                            }

                            if (arr.Table3.length > 0) {
                                var salep = arr.Table3[0].sale_price;
                                if (salep == "0") {
                                    var txtSalesPrice = (parseFloat(0).toFixed(RateDigit));
                                    $("#txtSalesPrice").val(txtSalesPrice);
                                    //$("#txtSalesPrice").attr("disabled", false);

                                    //var txtSalesPrice = $("#txtSalesPrice").val();
                                    //var txtSalesPrice1 = parseFloat(txtSalesPrice);
                                    //if (txtSalesPrice1 > 0) {
                                    //    $("#vm_txtSalesPrice").css("display", "none");
                                    //}
                                    //else {
                                    //    $('#vm_txtSalesPrice').text($("#valueReq").text());
                                    //    $("#vm_txtSalesPrice").css("display", "block");

                                    //}
                                }
                                else {
                                    var txtSalesPrice = (parseFloat(arr.Table3[0].sale_price).toFixed(RateDigit));
                                    $("#txtSalesPrice").val(txtSalesPrice);
                                    //$("#txtSalesPrice").attr("disabled", false);

                                    //var txtSalesPrice = $("#txtSalesPrice").val();
                                    //var txtSalesPrice1 = parseFloat(txtSalesPrice);
                                    //if (txtSalesPrice1 > 0) {
                                    //    $("#vm_txtSalesPrice").css("display", "none");
                                    //}
                                    //else {
                                    //    $('#vm_txtSalesPrice').text($("#valueReq").text());
                                    //    $("#vm_txtSalesPrice").css("display", "block");
                                    //    flag = 'Y';
                                    //}
                                }
                            }
                            else {
                                $("#txtSalesPrice").val('');
                            }
                            var Itm_ID1 = $("#ddl_ItemName").val();
                            if (Itm_ID1 != "0") {

                                var txtSalesPrice = $("#txtSalesPrice").val();
                                var txtSalesPrice1 = parseFloat(txtSalesPrice);
                                if (txtSalesPrice1 > 0) {
                                    $("#vm_txtSalesPrice").css("display", "none");
                                    $("#txtSalesPrice").css("border-color", "#ced4da");
                                }
                                else {//Price can not be 0
                                    $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
                                    $("#vm_txtSalesPrice").css("display", "block");
                                    $("#txtSalesPrice").css("border-color", "red");
                                }
                            }
                            else {
                                $("#txtSalesPrice").val(parseFloat(0).toFixed(RateDigit));
                                }
                           
                        }
                        else {
                            $("#UOM").val("");
                            $("#UOMID").val('');
                            $("#txtPreviousYearSalesInQuantity").val('');
                            $("#txtPreviousYearSalesInAmount").val('');
                            $("#txtSalesPrice").val('');
                            var hdn_actual_sale_qty = (parseFloat(0).toFixed(QtyDecDigit));
                            $("#hdn_actual_sale_qty").val(hdn_actual_sale_qty);
                            var hdn_actual_sale_value = (parseFloat(0).toFixed(ValDigit));
                            $("#hdn_actual_sale_value").val(hdn_actual_sale_value);
                        }
                    },
                });
        }
    }
}
    
function OnchangetxtSalesPrice() {
    var QtyDecDigit = $("#RateDigit").text();
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice);
    if (txtSalesPrice1 > 0) {
        $("#vm_txtSalesPrice").css("display", "none");
        $("#txtSalesPrice").css("border-color", "#ced4da");
        $("#txtSalesPrice").val(parseFloat(txtSalesPrice1).toFixed(QtyDecDigit));

    }
    else {//Price can not be 0
        $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
        $("#vm_txtSalesPrice").css("display", "block");
        $("#txtSalesPrice").css("border-color", "red");
    }
    OnChange_txtTargetSaleQuantity();
}
function txtSalePriceEnblDsbl() {
    $("#ddl_ItemName").change(function () {
        debugger
        if ($("#ddl_ItemName").val() != "0")
            $("#txtSalesPrice").attr("disabled", false);
        else 
            $("#txtSalesPrice").attr("disabled", true);
    });
    //$("#ParaNewAddBtn").click(function () {
    //    $("#txtSalesPrice").attr("disabled", true);
    //});
    //$("#ParaItemUpdateBtn").click(function () {
    //    $("#txtSalesPrice").attr("disabled", true);
    //});
}
function resetitemdetail() {
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    //var valzero = parseFloat(0).toFixed();
    $("#UOM").val('');
   
    $("#txtPreviousYearSalesInQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
    $("#txtPreviousYearSalesInAmount").val(parseFloat(0).toFixed(ValDigit));
    $("#txtIncreaseByInPercentage").val("");
    $("#txtReducedByInPercentage").val("");
    $("#txtTargetSaleQuantity").val("");
    $("#txtSalesPrice").val("");
    $("#txtTargetSalesValue").val("");
};
function ddl_financial_year_onchange(e) {
    debugger;
   
    var ddl_f_frequency = $("#ddl_financial_year").val();
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");

    }
    var ddl_f_frequency = $("#ddl_f_frequency").val();
    $("#hdn_ddl_f_frequency").val(ddl_f_frequency);
    var f_frequency = $("#hdn_ddl_f_frequency").val();

    var ddl_financial_year = $("#ddl_financial_year").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();
    $("#txtFromDate").val('');
    $("#txtToDate").val('');
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/SalesForecast/BindPeriod",
            data: {
                f_frequency: f_frequency,
                financial_year: financial_year,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#ddl_period').empty();
                    arr = JSON.parse(data);
                    if (arr.Table1.length > 0) {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                        for (var i = 0; i < arr.Table1.length; i++) {
                            $('#ddl_period').append(`<option value="${arr.Table1[i].id}">${arr.Table1[i].name}</option>`);
                        }
                    }
                    else {
                        $('#ddl_period').append(`<option value=0>---Select---</option>`);
                    }
                }
            },
        });
};
function ddl_period_onchange(e) {
    debugger;
    var ddl_f_frequency = $("#ddl_period").val();
    
    if (ddl_f_frequency != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
    }

    var ddl_f_frequency = $("#ddl_f_frequency").val();
    $("#hdn_ddl_f_frequency").val(ddl_f_frequency);
    var f_frequency = $("#hdn_ddl_f_frequency").val();

    var ddl_financial_year = $("#ddl_financial_year").val();
    $("#hdn_ddl_financial_year").val(ddl_financial_year);
    var financial_year = $("#hdn_ddl_financial_year").val();

    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#ddl_ItemName").attr('disabled', false);
    }
    else {
        $("#ddl_ItemName").attr('disabled', true);
    }
    $("#hdn_ddl_period").val(ddl_period);
    var period = $("#hdn_ddl_period").val();
    resetitemdetail();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/SalesForecast/BindDateRange",
            data: {
                f_frequency: f_frequency,
                financial_year: financial_year,
                period: period,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    $('#txtFromDate').val('');
                    $('#txtToDate').val('');
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#txtFromDate').val(arr.Table[i].StartDT);
                            $('#txtToDate').val(arr.Table[i].EndDT);
                        }
                    }
                    //if (arr.Table1.length > 0) {
                    //    sessionStorage.removeItem("PLitemList");
                    //    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table1));
                    //    $('#ddl_ItemName').empty();
                    //    $('#ddl_ItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                    //    for (var i = 0; i < arr.Table1.length; i++) {
                    //        $('#Textddl').append(`<option data-uom="${arr.Table1[i].uom_name}" value="${arr.Table1[i].Item_id}">${arr.Table1[i].Item_name}</option>`);
                    //    }
                    //    var firstEmptySelect = true;
                    //    $('#ddl_ItemName').select2({
                    //        templateResult: function (data) {
                    //            var UOM = $(data.element).data('uom');
                    //            var classAttr = $(data.element).attr('class');
                    //            var hasClass = typeof classAttr != 'undefined';
                    //            classAttr = hasClass ? ' ' + classAttr : '';
                    //            var $result = $(
                    //                '<div class="row">' +
                    //                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    //                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                    //                '</div>'
                    //            );
                    //            return $result;
                    //            firstEmptySelect = false;
                    //        }
                    //    });
                    //    $("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
                    //    //$('#ddl_ItemName').val("0").trigger('change');
                    //}
                }
                else {
                    $('#txtFromDate').val('');
                    $('#txtToDate').val('');
                }
            },
        });
};
function onInput_txtIncreaseByInPercentage() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtReducedByInPercentage = $('#txtReducedByInPercentage').val().trim();
    var txtIncreaseByInPercentage = $('#txtIncreaseByInPercentage').val().trim();
    if (txtIncreaseByInPercentage == "") {
        $("#txtReducedByInPercentage").prop("disabled", false);
    }
    else {
        if (parseFloat(txtIncreaseByInPercentage) ==0) {
            $("#txtReducedByInPercentage").prop("disabled", false);
        }
    }
    if (txtIncreaseByInPercentage != "") {
        if (parseFloat(txtIncreaseByInPercentage) == 0) {
            $("#txtReducedByInPercentage").prop("disabled", false);
        }
        else {
            $("#txtReducedByInPercentage").prop("disabled", true);
        }
    }
   
    var PreviousYearSalesInQuantity = $('#txtPreviousYearSalesInQuantity').val().trim();
    // var txtIncreaseByInPercentage = $('#txtIncreaseByInPercentage').val().trim();

    var per = (parseFloat(PreviousYearSalesInQuantity) / 100) * parseFloat(txtIncreaseByInPercentage);//((txtIncreaseByInPercentage / 100) * PreviousYearSalesInQuantity)
    var totalTSale = (parseFloat(PreviousYearSalesInQuantity) + parseFloat(per)).toFixed(QtyDecDigit);
    if (isNaN(totalTSale)) {
        totalTSale = "";
    }
    $('#txtTargetSaleQuantity').val(totalTSale);

    var TargetSaleQuantity = $('#txtTargetSaleQuantity').val();
    var txtSalesPrice = $('#txtSalesPrice').val();
    //if (txtSalesPrice = "0") {
    //    txtSalesPrice = 0;
    //}
    var TargetSalesValue = (parseFloat(TargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
    if (TargetSalesValue == "" || isNaN(TargetSalesValue)) {
        TargetSalesValue = "";
    }
    $('#txtTargetSalesValue').val(TargetSalesValue);

}
function onInput_txtReducedByInPercentage() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var PreviousYearSalesInQuantity = $('#txtPreviousYearSalesInQuantity').val().trim();
    //var txtReducedByInPercentage = $('#txtReducedByInPercentage').val().trim();
    var txtReducedByInPercentage = $('#txtReducedByInPercentage').val().trim();
    //var txtIncreaseByInPercentage = $('#txtIncreaseByInPercentage').val().trim();
    if (txtReducedByInPercentage == "") {
        $("#txtIncreaseByInPercentage").prop("disabled", false);
    }
    else {
        if (parseFloat(txtReducedByInPercentage) == 0) {
            $("#txtIncreaseByInPercentage").prop("disabled", false);
        }
    }
    if (txtReducedByInPercentage != "") {
        if (parseFloat(txtReducedByInPercentage) == 0) {
            $("#txtIncreaseByInPercentage").prop("disabled", false);
        }
        else {
            $("#txtIncreaseByInPercentage").prop("disabled", true);
        }
    }
    var totalTSale;
    if (parseFloat(txtReducedByInPercentage) <= 100) {
        var per = (parseFloat(PreviousYearSalesInQuantity) / 100) * parseFloat(txtReducedByInPercentage);//((txtIncreaseByInPercentage / 100) * PreviousYearSalesInQuantity)
         totalTSale = (parseFloat(PreviousYearSalesInQuantity) - parseFloat(per)).toFixed(QtyDecDigit);
    }
    if (isNaN(totalTSale)) {
        totalTSale = "";
    }

    $('#txtTargetSaleQuantity').val(totalTSale);
    var TargetSaleQuantity = $('#txtTargetSaleQuantity').val();
    var txtSalesPrice = $('#txtSalesPrice').val();
    //if (txtSalesPrice = "0") {
    //    txtSalesPrice = 0;
    //}
    var TargetSalesValue = (parseFloat(TargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
    if (TargetSalesValue == "" || isNaN(TargetSalesValue)) {
        TargetSalesValue = "";
    }
    $('#txtTargetSalesValue').val(TargetSalesValue);

}
function ValidateDetailToAdd() {
    var flag = 'N';
    var ddl_f_frequency = $("#ddl_f_frequency").val();
    if (ddl_f_frequency != "0") {
        $("#span_vm_ddl_f_frequency").css("display", "none");
        $("#ddl_f_frequency").css("border-color", "#ced4da");
    }
    else {
        $('#span_vm_ddl_f_frequency').text($("#valueReq").text());
        $("#span_vm_ddl_f_frequency").css("display", "block");
        $("#ddl_f_frequency").css("border-color", "red");
        flag = 'Y';
    }
    var ddl_financial_year = $("#ddl_financial_year").val();
    if (ddl_financial_year != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
        flag = 'Y';
    }
    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
        flag = 'Y';
    }

    var ddl_Item_id = $("#ddl_ItemName").val();
    if (ddl_Item_id != "0") {
        $("#vmddl_item_id").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_item_id').text($("#valueReq").text());
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        $("#vmddl_item_id").css("display", "block");
        flag = 'Y';
    }
    var UOM = $("#UOM").val();

    var txtPreviousYearSalesInQuantity = $("#txtPreviousYearSalesInQuantity").val();

    var txtPreviousYearSalesInVal = $("#txtPreviousYearSalesInAmount").val();

    var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
    if (txtIncreaseByInPercentage == "") {
        txtIncreaseByInPercentage = parseFloat(0).toFixed(RateDigit);
    }

    var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
    if (txtReducedByInPercentage == "") {
        txtReducedByInPercentage = parseFloat(0).toFixed(RateDigit);
    }

    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = parseFloat(txtTargetSaleQuantity);
    if (txtTargetSaleQuantity1 > 0) {
        $("#vm_txtTargetSaleQuantity").css("display", "none");
        $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
        $("#vm_txtTargetSaleQuantity").css("display", "block");
        $("#txtTargetSaleQuantity").css("border-color", "red");
        flag = 'Y';
    }
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice);
    if (txtSalesPrice1 > 0) {
        $("#vm_txtSalesPrice").css("display", "none");
        $("#txtSalesPrice").css("border-color", "#ced4da");
    }
    else {//Price can not be 0
        $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
        $("#vm_txtSalesPrice").css("display", "block");
        $("#txtSalesPrice").css("border-color", "red");
        flag = 'Y';
    }
    //var txtTargetSalesValue = $("#txtTargetSalesValue").val();

    if (flag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}
function OnClickAddParaNewAddBtn() {
    debugger;
   
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var flag = 'N';
    if (ValidateDetailToAdd() == false) {
        return false;
    }
    var ddl_f_frequency = $("#ddl_f_frequency").val();
    //if (ddl_f_frequency != "0") {
    //    $("#span_vm_ddl_f_frequency").css("display", "none");
    //    $("#ddl_f_frequency").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#span_vm_ddl_f_frequency').text($("#valueReq").text());
    //    $("#span_vm_ddl_f_frequency").css("display", "block");
    //    $("#ddl_f_frequency").css("border-color", "red");
    //    flag = 'Y';
    //}
    var ddl_financial_year = $("#ddl_financial_year").val();
    //if (ddl_financial_year != "0") {
    //    $("#vm_ddl_financial_year").css("display", "none");
    //    $("#ddl_financial_year").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#vm_ddl_financial_year').text($("#valueReq").text());
    //    $("#vm_ddl_financial_year").css("display", "block");
    //    $("#ddl_financial_year").css("border-color", "red");
    //    flag = 'Y';
    //}
    var ddl_period = $("#ddl_period").val();
    //if (ddl_period != "0") {
    //    $("#vm_ddl_period").css("display", "none");
    //    $("#ddl_period").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#vm_ddl_period').text($("#valueReq").text());
    //    $("#vm_ddl_period").css("display", "block");
    //    $("#ddl_period").css("border-color", "red");
    //    flag = 'Y';
    //}
    
    var ddl_Item_id = $("#ddl_ItemName").val();
    //if (ddl_Item_id != "0") {
    //    $("#vmddl_item_id").css("display", "none");
    //    $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#vmddl_item_id').text($("#valueReq").text());
    //    $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
    //    $("#vmddl_item_id").css("display", "block");
    //    flag = 'Y';
    //}
    var UOM = $("#UOM").val();
    
    var txtPreviousYearSalesInQuantity = $("#txtPreviousYearSalesInQuantity").val();
    
    var txtPreviousYearSalesInVal = $("#txtPreviousYearSalesInAmount").val();
    
    var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
    if (txtIncreaseByInPercentage == "") {
        txtIncreaseByInPercentage = parseFloat(0).toFixed(RateDigit);
    }
   
    var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
    if (txtReducedByInPercentage == "") {
        txtReducedByInPercentage = parseFloat(0).toFixed(RateDigit);
    }
   
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = parseFloat(txtTargetSaleQuantity).toFixed(QtyDecDigit);
    //if (txtTargetSaleQuantity1 >0) {
    //    $("#vm_txtTargetSaleQuantity").css("display", "none");
    //    $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
    //    $("#vm_txtTargetSaleQuantity").css("display", "block");
    //    $("#txtTargetSaleQuantity").css("border-color", "red");
    //    flag = 'Y';
    //}
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice);
    //if (txtSalesPrice1 > 0) {
    //    $("#vm_txtSalesPrice").css("display", "none");
    //    $("#txtSalesPrice").css("border-color", "#ced4da");
    //}
    //else {//Price can not be 0
    //    $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
    //    $("#vm_txtSalesPrice").css("display", "block");
    //    $("#txtSalesPrice").css("border-color", "red");
    //    flag = 'Y';
    //}
    var txtTargetSalesValue = $("#txtTargetSalesValue").val();
    
    //if (flag == 'Y') {
    //    return false;
    //}
    var ddl_ItemName = $("#ddl_ItemName option:selected").text();
    var hdn_UOMID = $("#UOMID").val();
    var hdn_actual_sale_qty = $("#hdn_actual_sale_qty").val();
    var hdn_actual_sale_value = $("#hdn_actual_sale_value").val();
   
    var targetSaleQty = $("#txtTargetSaleQuantity").val();
    var sub_item = $("#sub_item").val();
    var subitmFlag = CheckValidations_forSingleItem_SubItems(ddl_Item_id, targetSaleQty, sub_item, "SubItemTargtSalQty");
    if (subitmFlag == false) {
        return false;
    }

    var t = $('#datatable-buttons').DataTable();
    var RowId = 0;
    debugger;
    var sub_item = $("#sub_item").val();
   /* var sub_item = $("#datatable-buttons > tr >td #sub_item").val();*/
    var subitmDisable = "";
    if (sub_item != "Y") {
        subitmDisable = "disabled";
    }
    //$(".dataTables_empty").remove();
    //var S_NO = $('#datatable-buttons tbody tr').length;
    //var S_NO1 = SerialNoAfterDelete();// parseInt(S_NO) + parseInt(1);
    t.row.add([
        `<td class="red s_f_d center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event,'${ddl_Item_id}')" title="${$("#Span_Delete_Title").text()}"></i></td>`,
    `<i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" title="${$("#Edit").text()}"  onclick="editRow(this, event,'${ddl_Item_id}')"></i>`,
        `<td class="sr_padding"><span id="srno_">${++RowId}</span></td>`,
        `<td class="ItmNameBreak itmStick tditemfrz">
            <div class="col-sm-11 lpo_form" style="padding:0px;" id='td_ItemName'> ${ddl_ItemName}</div>

            <div class="col-sm-1 i_Icon">
                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${ddl_Item_id}','${ddl_ItemName}','edit');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" ><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button>
            </div>
        </td>`,


    `<input type='hidden' id='hdn_item_id' value="${ddl_Item_id}" style="display:none;"/>`,
        `<span id="FC_Item_Details_List">${UOM}
    <input type="hidden" id="tblhdn_Uom_name" value="${UOM}" style="display:none;"></span>`,
        
        `<input type='hidden' id='hdn_uom_id' value="${hdn_UOMID}"  style="display:none;" />`,
        `<div class='num_right'><div class="col-sm-10">${txtPreviousYearSalesInQuantity} </div>
            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemPYearSalQty">
            
            <button type="button" id="SubItemPYearSalQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ListPYearSalQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
            </div>
        </div>`,
    `<input type='hidden' id='hdn_pys_qty' class='num_right' value="${txtPreviousYearSalesInQuantity}" />`,
        `<div class='num_right'>${txtPreviousYearSalesInVal}</div>`,
    `<input type='hidden' id='hdn_pys_value' class='num_right' value="${txtPreviousYearSalesInVal}" />`,
        `<div id='FC_Item_Details_List1' class='num_right'>${txtIncreaseByInPercentage}</div>`,
    `<input type='hidden' id='hdn_increase_by' class='num_right' value="${txtIncreaseByInPercentage}" />`,
        `<div id='FC_Item_Details_List2' class='num_right'>${txtReducedByInPercentage}</div>`,
    `<input type='hidden' id='hdn_reduced_by' class='num_right' value="${txtReducedByInPercentage}" />`,
        `<div id='FC_Item_Details_List3' class='num_right'><div class="col-sm-10">${txtTargetSaleQuantity}</div>
        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemTargtSalQty">
        <input hidden type="text" id="sub_item" value="${sub_item}" />
        <button type="button" id="SubItemTargtSalQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('TargtSalSubitmQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
    </div>`,
    `<input type='hidden' id='hdn_target_sale_qty' class='num_right' value="${txtTargetSaleQuantity}" />`,
        `<div id='FC_Item_Details_List4' class='num_right'>${txtSalesPrice}</div>`,
    `<input type='hidden' id='hdn_sale_price' class='num_right' value="${txtSalesPrice}" />`,
        `<div id='FC_Item_Details_List5' class='num_right'>${txtTargetSalesValue}</div>`,
    `<input type='hidden' id='hdn_target_sale_value' class='num_right' value="${txtTargetSalesValue}" />`,
        `<div class='num_right'>${hdn_actual_sale_qty}</div>`,
    `<input type='hidden' id='hdn_actual_sale_qty' class='num_right' value="${hdn_actual_sale_qty}" />`,
        `<div class='num_right'>${hdn_actual_sale_value}</div>`,
    `<input type='hidden' id='hdn_actual_sale_value' class='num_right' value="${hdn_actual_sale_value}" />`,
    ]).draw();
    var sno = 0;
    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        sno = parseInt(sno) + 1
        $(this).closest('tr').find("#srno_").text(sno);

    });
    //SerialNoAfterDelete();
    if (flag == 'N') {
        var footer_PreviousYearSalesInValue = $("#footer_PreviousYearSalesInValue").val();
        if (footer_PreviousYearSalesInValue == "") {
            footer_PreviousYearSalesInValue = 0;
        }
        var txtPreviousYearSalesInAmount = $("#txtPreviousYearSalesInAmount").val();
        var txtPreviousYearSalesInAmount_Total = (parseFloat(txtPreviousYearSalesInAmount) + parseFloat(footer_PreviousYearSalesInValue)).toFixed(ValDigit);
        $("#footer_PreviousYearSalesInValue").val(txtPreviousYearSalesInAmount_Total);

        var footer_TargetSalesInValue = $("#footer_TargetSalesInValue").val();
        if (footer_TargetSalesInValue == "" || isNaN(footer_TargetSalesInValue)) {
            footer_TargetSalesInValue = 0;
        }

        var txtTargetSalesValue = $("#txtTargetSalesValue").val();
        if (txtTargetSalesValue == "" || isNaN(txtTargetSalesValue)) {
            txtTargetSalesValue = 0;
        }
        var txtTargetSalesValue_total = (parseFloat(txtTargetSalesValue) + parseFloat(footer_TargetSalesInValue)).toFixed(ValDigit);
        $("#footer_TargetSalesInValue").val(txtTargetSalesValue_total);

        var footer_ActualSaleInValue = $("#footer_ActualSaleInValue").val();
        if (footer_ActualSaleInValue == "" || isNaN(footer_ActualSaleInValue)) {
            footer_ActualSaleInValue = 0;
        }

        var hdn_actual_sale_value = $("#hdn_actual_sale_value").val();
        if (hdn_actual_sale_value == "" || isNaN(hdn_actual_sale_value)) {
            hdn_actual_sale_value = 0;
        }
        var hdn_actual_sale_value_total = (parseFloat(hdn_actual_sale_value) + parseFloat(footer_ActualSaleInValue)).toFixed(ValDigit);
        $("#footer_ActualSaleInValue").val(hdn_actual_sale_value_total);


        $("#txtIncreaseByInPercentage").val('');
        $("#txtReducedByInPercentage").val('');
        $("#txtTargetSaleQuantity").val('');
        $("#txtSalesPrice").val('');
        $("#txtTargetSalesValue").val('');
        $("#divAddNew").show();
        $("#divUpdate").hide();
        $("#SubItemPYearSalQty").prop("disabled", true);
        $("#SubItemTargtSalQty").prop("disabled", true);
        AddRemoveItemName(ddl_Item_id)
        //disableFyPeriod();
        
    }
};
function HeaderValidation()
{
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    
    var flag = 'N';
    var ddl_f_frequency = $("#ddl_f_frequency").val();
    if (ddl_f_frequency != "0") {
        $("#span_vm_ddl_f_frequency").css("display", "none");
        $("#ddl_f_frequency").css("border-color", "#ced4da");
    }
    else {
        $('#span_vm_ddl_f_frequency').text($("#valueReq").text());
        $("#span_vm_ddl_f_frequency").css("display", "block");
        $("#ddl_f_frequency").css("border-color", "red");
        flag = 'Y';
    }
    var ddl_financial_year = $("#ddl_financial_year").val();
    if (ddl_financial_year != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
        flag = 'Y';
    }
    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
        flag = 'Y';
    }

    var ddl_Item_id = $("#ddl_ItemName").val();
    if (ddl_Item_id != "0") {
        $("#vmddl_item_id").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_item_id').text($("#valueReq").text());
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        $("#vmddl_item_id").css("display", "block");
        flag = 'Y';
    }
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = parseFloat(txtTargetSaleQuantity).toFixed(QtyDecDigit);
    if (txtTargetSaleQuantity1 > 0) {
        $("#vm_txtTargetSaleQuantity").css("display", "none");
        $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
        $("#vm_txtTargetSaleQuantity").css("display", "block");
        $("#txtTargetSaleQuantity").css("border-color", "red");
        flag = 'Y';
    }
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice);
    if (txtSalesPrice1 > 0) {
        $("#vm_txtSalesPrice").css("display", "none");
        $("#txtSalesPrice").css("border-color", "#ced4da");
    }
    else {//Price can not be 0
        $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
        $("#vm_txtSalesPrice").css("display", "block");
        $("#txtSalesPrice").css("border-color", "red");
        flag = 'Y';
    }
    
    if (flag == 'Y') {
        return false;
    }
    else {
        return true;
    }

}
function ItemValidation() {
    debugger
    var ErrorFlag = "N";
    if ($("#datatable-buttons tbody tr").length > 0) {
        $("#datatable-buttons tbody tr").each(function () {
            debugger
            var currentRow = $(this);
            var number = currentRow.find("#srno_").text();
            if (number == "" || number == null || number == "0") {
                swal("", $("#noitemselectedmsg").text(), "warning");
                ErrorFlag = "Y";
            }
        });
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickAddParaNewAddBtn1() {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var flag = 'N';
    var ddl_financial_year = $("#ddl_financial_year").val();
    if (ddl_financial_year != "0") {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_financial_year').text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
        flag = 'Y';
    }
    var ddl_period = $("#ddl_period").val();
    if (ddl_period != "0") {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    else {
        $('#vm_ddl_period').text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
        flag = 'Y';
    }

    var ddl_Item_id = $("#ddl_ItemName").val();
    if (ddl_Item_id != "0") {
        $("#vmddl_item_id").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_item_id').text($("#valueReq").text());
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        $("#vmddl_item_id").css("display", "block");
        flag = 'Y';
    }
    var UOM = $("#UOM").val();

    var txtPreviousYearSalesInQuantity = $("#txtPreviousYearSalesInQuantity").val();

    var txtPreviousYearSalesInVal = $("#txtPreviousYearSalesInAmount").val();

    var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
    if (txtIncreaseByInPercentage == "") {
        txtIncreaseByInPercentage = parseFloat(0).toFixed(RateDigit);
    }

    var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
    if (txtReducedByInPercentage == "") {
        txtReducedByInPercentage = parseFloat(0).toFixed(RateDigit);
    }

    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    if (txtTargetSaleQuantity != "") {
        $("#vm_txtTargetSaleQuantity").css("display", "none");
        $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
        $("#vm_txtTargetSaleQuantity").css("display", "block");
        $("#txtTargetSaleQuantity").css("border-color", "red");
        flag = 'Y';
    }
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice);
    if (txtSalesPrice1 > 0) {
        $("#vm_txtSalesPrice").css("display", "none");
        $("#txtSalesPrice").css("border-color", "#ced4da");
    }
    else {//Price can not be 0
        $('#vm_txtSalesPrice').text($("#PriceNotZero").text());
        $("#vm_txtSalesPrice").css("display", "block");
        $("#txtSalesPrice").css("border-color", "red");
        flag = 'Y';
    }
    var txtTargetSalesValue = $("#txtTargetSalesValue").val();

    if (flag == 'Y') {
        return false;
    }
    var ddl_ItemName = $("#ddl_ItemName option:selected").text();
    var hdn_UOMID = $("#UOMID").val();
    var hdn_actual_sale_qty = $("#hdn_actual_sale_qty").val();
    var hdn_actual_sale_value = $("#hdn_actual_sale_value").val();


    $(".dataTables_empty").remove();
    var S_NO = $('#datatable-buttons tbody tr').length;
    //var S_NO1 = SerialNoAfterDelete();// parseInt(S_NO) + parseInt(1);
    $('#datatable-buttons tbody').append(`<tr id="">
   <td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}" onclick="deleteRow(this, event,'${ddl_Item_id}')"></i></td>
    <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" title="${$("#Edit").text()}"  onclick="editRow(this, event,'${ddl_Item_id}')"></i></td>
    <td id="srno" ></td>
    <td>${ddl_ItemName}</td>
    <td hidden='hidden'><input type='hidden' id='hdn_item_id' value="${ddl_Item_id}" /></td>
    <td><span id="FC_Item_Details_List">${UOM}</span></td>
    <td hidden='hidden'><input type='hidden' id='hdn_uom_id' value="${hdn_UOMID}" /></td>
    <td class='num_right'>${txtPreviousYearSalesInQuantity}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_pys_qty' value="${txtPreviousYearSalesInQuantity}" /></td>
    <td class='num_right'>${txtPreviousYearSalesInVal}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_pys_value' value="${txtPreviousYearSalesInVal}" /></td>
    <td id="FC_Item_Details_List1" class='num_right'>${txtIncreaseByInPercentage}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_increase_by' value="${txtIncreaseByInPercentage}" /></td>
    <td id="FC_Item_Details_List2" class='num_right'>${txtReducedByInPercentage}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_reduced_by' value="${txtReducedByInPercentage}" /></td>
    <td id="FC_Item_Details_List3" class='num_right'>${txtTargetSaleQuantity}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_target_sale_qty' value="${txtTargetSaleQuantity}" /></td>
    <td id="FC_Item_Details_List4" class='num_right'>${txtSalesPrice}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_sale_price' value="${txtSalesPrice}" /></td>
    <td id="FC_Item_Details_List5" class='num_right'>${txtTargetSalesValue}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_target_sale_value' value="${txtTargetSalesValue}" /></td>
    <td class='num_right'>${hdn_actual_sale_qty}</td>
    <td hidden='hidden' class='num_right'><input type='hidden' id='hdn_actual_sale_qty' value="${hdn_actual_sale_qty}" /></td>
    <td class='num_right'>${hdn_actual_sale_value}</td>
    <td hidden='hidden' ><input type='hidden' id='hdn_actual_sale_value' value="${hdn_actual_sale_value}" /></td>
    </tr>`);

    SerialNoAfterDelete();
    if (flag == 'N') {
        var footer_PreviousYearSalesInValue = $("#footer_PreviousYearSalesInValue").val();
        if (footer_PreviousYearSalesInValue == "") {
            footer_PreviousYearSalesInValue = 0;
        }
        var txtPreviousYearSalesInAmount = $("#txtPreviousYearSalesInAmount").val();
        var txtPreviousYearSalesInAmount_Total = (parseFloat(txtPreviousYearSalesInAmount) + parseFloat(footer_PreviousYearSalesInValue)).toFixed(ValDigit);
        $("#footer_PreviousYearSalesInValue").val(txtPreviousYearSalesInAmount_Total);

        var footer_TargetSalesInValue = $("#footer_TargetSalesInValue").val();
        if (footer_TargetSalesInValue == "" || isNaN(footer_TargetSalesInValue)) {
            footer_TargetSalesInValue = 0;
        }

        var txtTargetSalesValue = $("#txtTargetSalesValue").val();
        if (txtTargetSalesValue == "" || isNaN(txtTargetSalesValue)) {
            txtTargetSalesValue = 0;
        }
        var txtTargetSalesValue_total = (parseFloat(txtTargetSalesValue) + parseFloat(footer_TargetSalesInValue)).toFixed(ValDigit);
        $("#footer_TargetSalesInValue").val(txtTargetSalesValue_total);

        var footer_ActualSaleInValue = $("#footer_ActualSaleInValue").val();
        if (footer_ActualSaleInValue == "" || isNaN(footer_ActualSaleInValue)) {
            footer_ActualSaleInValue = 0;
        }

        var hdn_actual_sale_value = $("#hdn_actual_sale_value").val();
        if (hdn_actual_sale_value == "" || isNaN(hdn_actual_sale_value)) {
            hdn_actual_sale_value = 0;
        }
        var hdn_actual_sale_value_total = (parseFloat(hdn_actual_sale_value) + parseFloat(footer_ActualSaleInValue)).toFixed(ValDigit);
        $("#footer_ActualSaleInValue").val(hdn_actual_sale_value_total);


        $("#txtIncreaseByInPercentage").val('');
        $("#txtReducedByInPercentage").val('');
        $("#txtTargetSaleQuantity").val('');
        $("#txtSalesPrice").val('');
        $("#txtTargetSalesValue").val('');
        $("#divAddNew").show();
        $("#divUpdate").hide();
        AddRemoveItemName(ddl_Item_id)
        //disableFyPeriod();

    }
};

function AddRemoveItemName(ddl_op_ID) {
    debugger;
    var flag = 'N';
    //$("#ddl_ItemName option").removeClass("select2-hidden-accessible");
    //$('#ddl_ItemName').val("0").change();

    $("#datatable-buttons >tbody >tr").each(function (j, rows) {
        var currentRowChild = $(this);
        //Hide item name, if exist in table
        debugger;
        var item_id = currentRowChild.find("#hdn_item_id").val();//recheck                

        $("#ddl_ItemName option[value=" + item_id + "]").select2().hide();
        $('#ddl_ItemName').val("0").change();
    });

}
function disableFyPeriod() {

    $("#ddl_financial_year").prop("disabled", true);
    $("#ddl_period").prop("disabled", true);
    //$("#txt_Quantity").prop("readonly", true);

}
function deleteRow(el, e, option) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var i = el.parentNode.parentNode.rowIndex-1;
    //alert(i);
    var clickedrow = $(e.target).closest("tr");
    var hdn_Itm_ID = clickedrow.find("#hdn_item_id").val();

    var footer_PreviousYearSalesInValue = $("#footer_PreviousYearSalesInValue").val();
    if (footer_PreviousYearSalesInValue == "" || isNaN(footer_PreviousYearSalesInValue)) {
        footer_PreviousYearSalesInValue = parseFloat(0).toFixed(ValDigit);
    }
    var footer_TargetSalesInValue = $("#footer_TargetSalesInValue").val();
    if (footer_TargetSalesInValue == "" || isNaN(footer_TargetSalesInValue)) {
        footer_TargetSalesInValue = parseFloat(0).toFixed(ValDigit);
    }
    var footer_ActualSaleInValue = $("#footer_ActualSaleInValue").val();
    if (footer_ActualSaleInValue == "" || isNaN(footer_ActualSaleInValue)) {
        footer_ActualSaleInValue = parseFloat(0).toFixed(ValDigit);
    }
    var hdn_pys_value = clickedrow.find("#hdn_pys_value").val();
    //var hdn_pys_value = $("#hdn_pys_value").val();
    if (hdn_pys_value == "" || isNaN(hdn_pys_value)) {
        hdn_pys_value = parseFloat(0).toFixed(ValDigit);
    }
    var hdn_target_sale_value = clickedrow.find("#hdn_target_sale_value").val();
    //var hdn_target_sale_value = $("#hdn_target_sale_value").val();
    if (hdn_target_sale_value == "" || isNaN(hdn_target_sale_value)) {
        hdn_target_sale_value = parseFloat(0).toFixed(ValDigit);
    }
    var hdn_actual_sale_value = clickedrow.find("#hdn_actual_sale_value").val();
    //var hdn_actual_sale_value = $("#hdn_actual_sale_value").val();
    if (hdn_actual_sale_value == "" || isNaN(hdn_actual_sale_value)) {
        hdn_actual_sale_value = parseFloat(0).toFixed(ValDigit);
    }

    var PreviousYearSalesInValue_total = (parseFloat(footer_PreviousYearSalesInValue) - parseFloat(hdn_pys_value)).toFixed(ValDigit);
    $("#footer_PreviousYearSalesInValue").val(PreviousYearSalesInValue_total);

    var TargetSalesInValue_total = (parseFloat(footer_TargetSalesInValue) - parseFloat(hdn_target_sale_value)).toFixed(ValDigit);
    $("#footer_TargetSalesInValue").val(TargetSalesInValue_total);

    var ActualSaleInValue_total = (parseFloat(footer_ActualSaleInValue) - parseFloat(hdn_actual_sale_value)).toFixed(ValDigit);
    $("#footer_ActualSaleInValue").val(ActualSaleInValue_total);


    var table = $('#datatable-buttons').DataTable();
    table.row(i).remove().draw(false);
//document.getElementById("datatable-buttons").deleteRow(i);
    $("#ddl_ItemName option[value=" + hdn_Itm_ID + "]").removeClass("select2-hidden-accessible");
    $('#ddl_ItemName').val("0").change();
    var lenght = $('#datatable-buttons tbody tr').length;

    if ($("#spanval").text() == "") {
        if (lenght - 1 == 0) {
            $("#ddl_financial_year").prop("disabled", false);
            $("#ddl_period").prop("disabled", false);
        }
    }
    else {
        if (lenght == 0) {
            $("#ddl_financial_year").prop("disabled", false);
            $("#ddl_period").prop("disabled", false);
        }
    }
    $("#ddl_ItemName").prop("disabled", false);
    $("#txtSalesPrice").val(parseFloat(0).toFixed(RateDigit));
    $("#UOM").val('');
    $("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
    $("#divAddNew").show();
    $("#divUpdate").hide();
    Cmn_DeleteSubItemQtyDetail(hdn_Itm_ID);
    $("#SubItemPYearSalQty").prop("disabled", true);
    $("#SubItemTargtSalQty").prop("disabled", true);
    resetitemdetail();
    SerialNoAfterDelete();
};

function editRow(el, e, option) {
    debugger;
    $("#ddl_ItemName").removeAttr('onchange');
    $("#ddl_ItemName").prop("disabled", true);
    
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var rowJavascript = el.parentNode.parentNode;
    var clickedrow = $(e.target).closest("tr");
    var subitem = clickedrow.find("#sub_item").val();
        $("#sub_item").val(subitem);
        var sub_item = $("#sub_item").val();
        if (sub_item == "Y") {
            $("#SubItemPYearSalQty").prop("disabled", false);
            $("#SubItemTargtSalQty").prop("disabled", false);
        }
        else {
            $("#SubItemPYearSalQty").prop("disabled", true);
            $("#SubItemTargtSalQty").prop("disabled", true);
        }
   

    $('#hdnUpdateInTable').val(rowJavascript.rowIndex);
    var hdn_item_id = clickedrow.find("#hdn_item_id").val();

    $("#vmddl_item_id").css("display", "none");
    $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");   

    $("#vm_txtSalesPrice").css("display", "none");
    $("#txtSalesPrice").css("border-color", "#ced4da");

    $("#vm_txtTargetSaleQuantity").css("display", "none");
    $("#txtTargetSaleQuantity").css("border-color", "#ced4da");

    var hdn_increase_by1 = clickedrow.find("#hdn_increase_by").val();
    var hdn_increase_by = parseFloat(hdn_increase_by1).toFixed(RateDigit);
    if (hdn_increase_by > 0) {
        $("#txtIncreaseByInPercentage").prop("disabled", false);
        $("#txtReducedByInPercentage").val(parseFloat(0).toFixed(RateDigit));
    }
    else {
        $("#txtIncreaseByInPercentage").prop("disabled", true);
    }
    $("#txtIncreaseByInPercentage").val(hdn_increase_by);

    var hdn_reduced_by1 = clickedrow.find("#hdn_reduced_by").val();
    var hdn_reduced_by = parseFloat(hdn_reduced_by1).toFixed(RateDigit);
    $("#txtReducedByInPercentage").val(hdn_reduced_by);
    if (hdn_reduced_by > 0) {
        $("#txtReducedByInPercentage").prop("disabled", false);
        $("#txtIncreaseByInPercentage").val(parseFloat(0).toFixed(RateDigit));
    }
    else {
        $("#txtReducedByInPercentage").prop("disabled", true);
    }

    var hdn_target_sale_qty1 = clickedrow.find("#hdn_target_sale_qty").val();
    var hdn_target_sale_qty = parseFloat(hdn_target_sale_qty1).toFixed(QtyDecDigit);
    $("#txtTargetSaleQuantity").val(hdn_target_sale_qty);

    var hdn_sale_price1 = clickedrow.find("#hdn_sale_price").val();
    var hdn_sale_price = parseFloat(hdn_sale_price1).toFixed(RateDigit);
    $("#txtSalesPrice").val(hdn_sale_price);

    var hdn_target_sale_value1 = clickedrow.find("#hdn_target_sale_value").val();
    var hdn_target_sale_value = parseFloat(hdn_target_sale_value1).toFixed(ValDigit);
    $("#txtTargetSalesValue").val(hdn_target_sale_value);
   
    var UOM = clickedrow.find("#FC_Item_Details_List").text();
    $("#UOM").val(UOM);

    var hdn_pys_qty = clickedrow.find("#hdn_pys_qty").val();
    $("#txtPreviousYearSalesInQuantity").val(parseFloat(hdn_pys_qty).toFixed(QtyDecDigit));

    var hdn_pys_value = clickedrow.find("#hdn_pys_value").val();
    $("#txtPreviousYearSalesInAmount").val(parseFloat(hdn_pys_value).toFixed(ValDigit));

    $("#hdn_item_id").val(hdn_item_id);

    var hdn_uom_id = clickedrow.find("#hdn_uom_id").val();
    $("#UOMID").val(hdn_uom_id);
    var td_ItemName = clickedrow.find("#td_ItemName").text();
    if (td_ItemName != null) {
        td_ItemName = td_ItemName.trim();
    }
    $('#ddl_ItemName').append('<option value=' + hdn_item_id+' selected>' + td_ItemName + '</option>').change();
    //$('#ddl_ItemName').val(hdn_item_id).change();
    //$("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
    $("#divAddNew").hide();
    $("#divUpdate").show();

};
function SerialNoAfterDelete() {
    var SerialNo = 0;
    var Status = $("#spanval").text();
   
    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;

        //if (Status == "") {
        //        currentRow.find("td:eq(2)").text(SerialNo - 1);
        //}
        //else {
        currentRow.find("#srno_").text(SerialNo);
        //}
        

    });
};
function OnClickParaItemUpdateBtn() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    //var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    if (ValidateDetailToAdd() == false) {
        return false;
    }
    debugger;
    var ddl_Item_id = $("#ddl_ItemName").val();
    var targetSaleQty = $("#txtTargetSaleQuantity").val();
    var status = $("#hdStatus_Code").val();
    //if (status == "" || status == null) {
    //    var subitem = "";
    //     subitem = $("#sub_item").val();
    //}
    //else
    //{
    //    var clickedrow = $(e.target).closest("tr");
    //    subitem = clickedrow.find("#sub_item").val();
    //}
    var sub_item = $("#sub_item").val();
    var subitmFlag = CheckValidations_forSingleItem_SubItems(ddl_Item_id, targetSaleQty, sub_item, "SubItemTargtSalQty");
    if (subitmFlag == false) {
        return false;
    }
    var subitmDisable = "";
    if (sub_item != "Y") {
        subitmDisable = "disabled";
    }
    flag = 'N';
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = parseFloat(txtTargetSaleQuantity).toFixed(QtyDecDigit);
    //if (txtTargetSaleQuantity1 > 0) {
    //    $("#vm_txtTargetSaleQuantity").css("display", "none");
    //    $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    //}
    //else {
    //    $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
    //    $("#vm_txtTargetSaleQuantity").css("display", "block");
    //    $("#txtTargetSaleQuantity").css("border-color", "red");
    //    flag = 'Y';
    //}
    if (flag == 'N') {
        var tableRow = $('#hdnUpdateInTable').val();
        var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_increase_by").val(txtIncreaseByInPercentage);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List1").html(`<div class='num_right'>${txtIncreaseByInPercentage}</div>`);

        var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_reduced_by").val(txtReducedByInPercentage);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List2").html(`<div class='num_right'>${txtReducedByInPercentage}</div>`);

        var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_target_sale_qty").val(txtTargetSaleQuantity);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List3").html(`
<div class=' col-sm-10 num_right'>${txtTargetSaleQuantity}</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemTargtSalQty">
        <input hidden type="text" id="sub_item" value="${sub_item}" />
        <button type="button" id="SubItemTargtSalQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('TargtSalSubitmQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>`);

        var txtSalesPrice = $("#txtSalesPrice").val();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_sale_price").val(txtSalesPrice);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List4").html(`<div class='num_right'>${txtSalesPrice}</div>`);

        var txtTargetSalesValue = $("#txtTargetSalesValue").val();
        var footer_TargetSalesInValue = $("#footer_TargetSalesInValue").val();
        var hdn_target_sale_value = $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_target_sale_value").val();
        var target_sale_value1 = parseFloat(footer_TargetSalesInValue) - parseFloat(hdn_target_sale_value);
        var target_sale_value2 = (parseFloat(txtTargetSalesValue) + parseFloat(target_sale_value1)).toFixed(ValDigit);
        $("#footer_TargetSalesInValue").val(target_sale_value2);

        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_target_sale_value").val(txtTargetSalesValue);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List5").html(`<div class='num_right'>${txtTargetSalesValue}</div>`);
        var item_id = $("#hdn_item_id").val();
        var ddl_ItemName = $("#ddl_ItemName option:selected").text();
        var abc = `<div class="col-sm-11 lpo_form" style="padding:0px;" id=''> ${ddl_ItemName}</div>

            <div class="col-sm-1 i_Icon">
                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${item_id}','${ddl_ItemName}','edit');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
            </div >`;

    
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_item_id").val(item_id);
       // $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("td:eq(3)").remove();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#td_ItemName").append();

        var UOMID = $("#UOMID").val();
        var UOM_name = $("#UOM").val();
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#hdn_uom_id").val(UOMID);
        $('#datatable-buttons').find("tr:eq(" + tableRow + ")").find("#FC_Item_Details_List").text(UOM_name);

        //var table = $('#datatable-buttons').DataTable();
        //table.row().draw();

        $("#ddl_ItemName").attr('disabled', false);
        $("#ddl_ItemName").attr('onchange', 'ddl_ItemName_onchange()');
        $("#divAddNew").show();
        $("#divUpdate").hide();
        $("#SubItemPYearSalQty").prop("disabled", true);
        $("#SubItemTargtSalQty").prop("disabled", true);
        resetitemdetail();
        AddRemoveItemName(item_id);
    }
};
function onInput_txtTargetSaleQuantity() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    if (txtTargetSaleQuantity != "") {
        $("#vm_txtTargetSaleQuantity").css("display", "none");
        $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
    }
    else {
        $('#vm_txtTargetSaleQuantity').text($("#valueReq").text());
        $("#vm_txtTargetSaleQuantity").css("display", "block");
    }
    var PreviousYearSalesInQuantity = $('#txtPreviousYearSalesInQuantity').val().trim();
    var txtTargetSaleQuantity1 = $("#txtTargetSaleQuantity").val().trim();
    var txtTargetSaleQuantity = (parseFloat(txtTargetSaleQuantity1).toFixed(QtyDecDigit));
    var txtSalesPrice = $("#txtSalesPrice").val().trim();
    if (txtSalesPrice == "") {
        txtSalesPrice = (parseFloat(0).toFixed(RateDigit));
    }
    txtSalesPrice = (parseFloat(txtSalesPrice).toFixed(RateDigit));
    var forminus = "100";
    if (PreviousYearSalesInQuantity == "") {
        var txtReducedByInPercentage = (parseFloat(0).toFixed(RateDigit));
        $("#txtReducedByInPercentage").val(txtReducedByInPercentage);
        var txtIncreaseByInPercentage = (parseFloat(0).toFixed(RateDigit));
        $("#txtIncreaseByInPercentage").val(txtIncreaseByInPercentage);
        $("#txtIncreaseByInPercentage").prop("disabled", true);
        $("#txtReducedByInPercentage").prop("disabled", true);
        var TargetSaleVal = (parseFloat(txtTargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
        if (isNaN(TargetSaleVal)) {
            TargetSaleVal = '';
        }
        $("#txtTargetSalesValue").val(TargetSaleVal);
    }
    else {
        var targetPercentage = parseFloat((parseFloat(txtTargetSaleQuantity) / parseFloat(PreviousYearSalesInQuantity)) * 100).toFixed(QtyDecDigit);
        if (targetPercentage != "Infinity") {
            if (targetPercentage > 100) {
                $("#txtReducedByInPercentage").prop("disabled", true);
                $("#txtIncreaseByInPercentage").prop("disabled", false);
                var txtReducedByInPercentage = (parseFloat(0).toFixed(RateDigit));
                $("#txtReducedByInPercentage").val(txtReducedByInPercentage);
                var increaseby = parseFloat(targetPercentage) - parseFloat(forminus);
                var increaseby1 = (increaseby).toFixed(RateDigit);
                $('#txtIncreaseByInPercentage').val(increaseby1);

                var TargetSaleVal = (parseFloat(txtTargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
                if (isNaN(TargetSaleVal)) {
                    TargetSaleVal = '';
                }
                $("#txtTargetSalesValue").val(TargetSaleVal);
            }
            if (targetPercentage < 100) {
                $("#txtIncreaseByInPercentage").prop("disabled", true);
                $("#txtReducedByInPercentage").prop("disabled", false);
                var txtIncreaseByInPercentage = (parseFloat(0).toFixed(RateDigit));
                $("#txtIncreaseByInPercentage").val(txtIncreaseByInPercentage);
                var ReducedBy = parseFloat(forminus) - parseFloat(targetPercentage);
                var ReducedBy1 = (ReducedBy).toFixed(RateDigit);
                $('#txtReducedByInPercentage').val(ReducedBy1);

                var TargetSaleVal = (parseFloat(txtTargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
                if (isNaN(TargetSaleVal)) {
                    TargetSaleVal = '';
                }
                $("#txtTargetSalesValue").val(TargetSaleVal);
            }
            if (targetPercentage == 100) {
                $("#txtReducedByInPercentage").prop("disabled", true);
                $("#txtIncreaseByInPercentage").prop("disabled", false);
                var txtReducedByInPercentage = (parseFloat(0).toFixed(RateDigit));
                $("#txtReducedByInPercentage").val(txtReducedByInPercentage);
                var increaseby = parseFloat(targetPercentage) - parseFloat(forminus);
                var increaseby1 = (increaseby).toFixed(RateDigit);
                $('#txtIncreaseByInPercentage').val(increaseby1);

                var TargetSaleVal = (parseFloat(txtTargetSaleQuantity) * parseFloat(txtSalesPrice)).toFixed(ValDigit);
                if (isNaN(TargetSaleVal)) {
                    TargetSaleVal = '';
                }
                $("#txtTargetSalesValue").val(TargetSaleVal);
            }
        }
        else {
            var txtTargetSalesValue = (parseFloat(0).toFixed(ValDigit));
            $("#txtTargetSalesValue").val(txtTargetSalesValue);
        }
    }
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = (parseFloat(txtTargetSaleQuantity).toFixed(QtyDecDigit));
    var txtSalesPrice = $("#txtSalesPrice").val();
    var txtSalesPrice1 = parseFloat(txtSalesPrice).toFixed(RateDigit);
    var totalTarget_val = parseFloat(txtTargetSaleQuantity1 * txtSalesPrice1).toFixed(ValDigit);
    if (isNaN(totalTarget_val)) {
        totalTarget_val = parseFloat(0).toFixed(ValDigit);
    }
    $("#txtTargetSalesValue").val(totalTarget_val);
};

function validateSFCDetailInsert() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var ValDigit = $("#ValDigit").text();
    debugger;
    var length = $("#datatable-buttons >tbody >tr>td").length;
        if(length==1)
        {
            if (HeaderValidation() == false)
            {
                 return false;
            }
        }
            
    if (ItemValidation() == false) {
        return false;
    }
    var subitmFlag = CheckValidations_forSubItems();
    if (subitmFlag == false) {
        return false;
    }
    var flag = true;
    var footer_TargetSalesInValue = $("#footer_TargetSalesInValue").val();
    var footer_TargetSalesInValue1 = parseFloat(footer_TargetSalesInValue).toFixed(ValDigit);
    if (( isNaN( footer_TargetSalesInValue1)) || (footer_TargetSalesInValue1 > 0)) {
        
    }
    else {
        
        flag = false;
    }
    if (flag == false) {
        return false;
    }
    //var lenght = $('#datatable-buttons tbody tr').length;
    //if (flag == true) {
    //    $("#datatable-buttons >tbody >tr").each(function (i, row) {
    //        debugger;
    //        var currentRow = $(this);
    //        var number = currentRow.find("td:eq(2)").text();
    //        if (number > 0) {
    //            flag = true;
    //        }
    //        else {
    //            flag = false;
    //         }
    //      });
    //}
    //if (flag == false) {
    //    return false;
    //}
    
   if (flag == true) {
       validateSFCDetailInsert1();
       /*-----------Sub-item-------------*/

       var SubItemsListArr = Cmn_SubItemList();
       var str2 = JSON.stringify(SubItemsListArr);
       $('#SubItemDetailsDt').val(str2);

       /*-----------Sub-item end-------------*/
       $("#hdnsavebtn").val("AllreadyclickSaveBtn");
           return true;
        }

        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }

    
    
};

function validateSFCDetailInsert1() {
    debugger;
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        FinalItemDetail = InsertItemAttributeDetails();

        debugger;
        var ItemAttrDt = JSON.stringify(FinalItemDetail);
        $('#hd_SFCItemdetails').val(ItemAttrDt);

        return true;
    }
    else {
        //alert("Check network");
        return false;
    }
};
function InsertItemAttributeDetails() {
    debugger;
    var itemDTransType = sessionStorage.getItem("ItmDTransType");
    var itmcode = sessionStorage.getItem("EditItemCode");
    var TransType = '';
    if (itemDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var AttrList = [];
    debugger;
    $("#datatable-buttons >tbody >tr").each(function (j, rows) {
        var currentRowChild = $(this);
        debugger;
        var item_id = currentRowChild.find("#hdn_item_id").val();
        var uom_id = currentRowChild.find("#hdn_uom_id").val();
        var inc_by = currentRowChild.find("#hdn_increase_by").val();
        var red_by = currentRowChild.find("#hdn_reduced_by").val();
        var tgt_qty = currentRowChild.find("#hdn_target_sale_qty").val();
        var sale_price = currentRowChild.find("#hdn_sale_price").val();
        var tgt_val = currentRowChild.find("#hdn_target_sale_value").val();
        AttrList.push({ item_id: item_id, uom_id: uom_id, inc_by: inc_by, red_by: red_by, tgt_qty: tgt_qty, sale_price: sale_price, tgt_val: tgt_val, })

    });
    // });

    return AttrList;
};
function functionConfirm(event) {
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function onclickcancilflag() {

    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").prop('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").prop('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
};
//function AvoidDot(a) {
//    debugger;
//    if (a.length == 1 && a == ".") {
//        return false;
//    }
//    else if (parseFloat(a) == 0) {
//        return false;
//    } else if (a == "") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function OnChange_txtIncreaseByInPercentage() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtIncreaseByInPercentage = $("#txtIncreaseByInPercentage").val();
    var txtIncreaseByInPercentage1 = (parseFloat(txtIncreaseByInPercentage).toFixed(RateDigit));
    if (isNaN(txtIncreaseByInPercentage1)) {
        txtIncreaseByInPercentage1 = parseFloat(0).toFixed(RateDigit);
        
    }
    $("#txtIncreaseByInPercentage").val(txtIncreaseByInPercentage1);
    $("#vm_txtTargetSaleQuantity").css("display", "none");
    $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
};
function OnChange_txtReducedByInPercentage() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtReducedByInPercentage = $("#txtReducedByInPercentage").val();
    var txtReducedByInPercentage1 = (parseFloat(txtReducedByInPercentage).toFixed(RateDigit));
    if (isNaN(txtReducedByInPercentage1)) {
        txtReducedByInPercentage1 = parseFloat(0).toFixed(RateDigit);
    }
    $("#txtReducedByInPercentage").val(txtReducedByInPercentage1);
    $("#vm_txtTargetSaleQuantity").css("display", "none");
    $("#txtTargetSaleQuantity").css("border-color", "#ced4da");
};
function OnChange_txtTargetSaleQuantity() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtTargetSaleQuantity = $("#txtTargetSaleQuantity").val();
    var txtTargetSaleQuantity1 = (parseFloat(txtTargetSaleQuantity).toFixed(QtyDecDigit));
    //var txtSalesPrice = $("#txtSalesPrice").val();
    //var txtSalesPrice1 = parseFloat(txtSalesPrice).toFixed(RateDigit);
    //var totalTarget_val = txtTargetSaleQuantity1 * txtSalesPrice1;
    if (isNaN(txtTargetSaleQuantity1)) {
        txtTargetSaleQuantity1 = parseFloat(0).toFixed(QtyDecDigit);
    }
    $("#txtTargetSaleQuantity").val(txtTargetSaleQuantity1);
};
function emtpy_text() {
    debugger;
    $("#txtIncreaseByInPercentage").val('');
    $("#txtReducedByInPercentage").val('');
    $("#txtTargetSaleQuantity").val('');
    $("#txtTargetSalesValue").val('');

};

function AmtFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
        $("#txtTargetSaleQuantity").attr("autocomplete", "off");
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
} 
function AmtRateDigitonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
} 

function OnClickIconBtn(e, item_id, item_mame, edit) {
    debugger;

    var ItmCode = "";
  //  var ItmName = "";

    ItmCode = $("#ddl_ItemName").val();
    //ItmName = $("#ddl_ItemName option:selected").text();
    if (edit == 'edit') {
        ItmCode = item_id;
       // ItmName = item_mame;
    }
    ItemInfoBtnClick(ItmCode)
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DPO/GetPOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        SFC_ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            $("#SpanItemDescription").text(ItmName);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                        else {
    //                            $("#Txt_ItmOEMNo").val("");
    //                            $("#Txt_ItmSampCode").val("");
    //                            $("#Txt_ItmRefNo").val("");
    //                            $("#Txt_ItmHSNCode").val("");
    //                            $("#SpanItemDescription").text("");
    //                            $("#myCarousel").html("");
    //                        }
    //                    }
    //                    else {
    //                        $("#Txt_ItmOEMNo").val("");
    //                        $("#Txt_ItmSampCode").val("");
    //                        $("#Txt_ItmRefNo").val("");
    //                        $("#Txt_ItmHSNCode").val("");
    //                        $("#SpanItemDescription").text("");
    //                        $("#myCarousel").html("");
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}

/*------------- For Workflow,Forward,Approve------------------*/
function ForwardBtnClick() {
    debugger;
    //var MRSStatus = "";
    //MRSStatus = $('#hdStatus_Code').val().trim();
    //if (MRSStatus === "D" || MRSStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");

    //    }
    //    //$("#radio_reject").prop("disabled", true);
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId
            },
            success: function (data) {
                if (data == "Exist") { /*End to chk Financial year exist or not*/
                    var MRSStatus = "";
                    MRSStatus = $('#hdStatus_Code').val().trim();
                    if (MRSStatus === "D" || MRSStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");

                        }
                        //$("#radio_reject").prop("disabled", true);
                        var Doc_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");

                        Cmn_GetForwarderList(Doc_ID);

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SFRCNo = "";
    var SFRCDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var Status = "";
    var Period = "";
    var FrFyear = "";
    //var ReqstionType = "";
    debugger;
    docid = $("#DocumentMenuId").val();
    var sf_id = $("#hdn_sf_id").val();
    var sfrNo1 = $("#ddl_financial_year").val();
    var sfrNo2 = $("#ddl_period").val();
    var WF_Status1 = $("#WF_Status1").val();
    SFRCNo = (sfrNo1 + ',' + sfrNo2);
    //SFRCNo = SFRCNo.replaceAll(',', '').replaceAll('-', '')
    SFRCNo = sf_id;
    SFRCDate = $("#hdCreatedOn").val();
    //$("#hdDoc_No").val(SFRCNo); //Commented by Suraj
    $("#hdDoc_No").val(sf_id);
    Remarks = $("#fw_remarks").val();
    var TrancType = (sf_id + ',' + WF_Status1)
    Status = "A";
    Period = $("#ddl_period").val();
    FrFyear = $("#ddl_financial_year").val();
    
    //ReqstionType = $("#ddlRequisitionTypeList").val();


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && SFRCNo != "" && SFRCDate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(SFRCNo, SFRCDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SalesForecast/ToRefreshByJS?TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {
        var list1 = [{
            sf_id: sf_id, /*sfc_no: sf_id,*/ sfc_dt: SFRCDate, status: Status, period: Period, fyear: FrFyear, A_Status: "Approve",
            A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks
        }];
        var AppDtList1 = JSON.stringify(list1);
        window.location.href = "/ApplicationLayer/SalesForecast/ApproveDocByWorkFlow?AppDtList1=" + AppDtList1 + "" + "&WF_Status1=" + WF_Status1;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/SalesForecast/SaveSalesForecast?frc_no=" + SFRCNo + "&frc_date=" + SFRCDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SFRCNo != "" && SFRCDate != "") {
             Cmn_InsertDocument_ForwardedDetail(SFRCNo, SFRCDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SalesForecast/ToRefreshByJS?TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SFRCNo != "" && SFRCDate != "") {
             Cmn_InsertDocument_ForwardedDetail(SFRCNo, SFRCDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SalesForecast/ToRefreshByJS?TrancType=" + TrancType;
        }
    }

}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    //var sfrNo1 = $("#ddl_financial_year").val();
    //var sfrNo2 = $("#ddl_period").val();
    var sf_id = $("#hdn_sf_id").val();
    //var Doc_No = (sfrNo1 + ',' + sfrNo2);
   // Doc_No = Doc_No.replaceAll(',', '').replaceAll('-', '')
    var Doc_No = sf_id;
    //var Doc_No = $('#ddl_financial_year' + ',' + 'ddl_period').val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {

}
function ResetWF_Level() {
    var li_count = $("#wizard ul li").length;
    if (li_count > 0) {
        for (var y = 0; y < li_count; y++) {
            var id = parseInt(y) + 1;
            $("#a_" + id).removeClass("done");
            $("#a_" + id).removeClass("selected");
            $("#a_" + id).addClass("disabled");
        }
    }
}
function OnChangeCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return validateSFCDetailInsert()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item) {
    debugger;
    Cmn_SubItemHideShow(sub_item, "NoRow", "sub_item", "SubItemTargtSalQty");
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPYearSalQty");
   
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var SFRCNo = "";
    var SFRCDate = "";
    if (flag == "Quantity" || flag == "PYearSalQty") {
            var ProductNm = $("#ddl_ItemName option:selected").text()
            var ProductId = $("#ddl_ItemName option:selected").val();
            var UOM = $("#UOM").val();
        }
        else {
            var ProductNm = clickdRow.find("#td_ItemName").text();
            var ProductId = clickdRow.find("#hdn_item_id").val();
            var UOM = clickdRow.find("#tblhdn_Uom_name").val();
        }
    
    if (flag == "ListPYearSalQty") {
        var ProductNm = clickdRow.find("#td_ItemName").text();
        var ProductId = clickdRow.find("#hdn_item_id").val();
        var UOM = clickdRow.find("#tblhdn_Uom_name").val();
    }
    var fromdate = $("#txtFromDate").val();
    var todate = $("#txtToDate").val();
    var sf_id = $("#hdn_sf_id").val();
    var sfrNo1 = $("#ddl_financial_year").val();
    var sfrNo2 = $("#ddl_period").val();
    SFRCNo = (sfrNo1 + ',' + sfrNo2);
    SFRCNo = sf_id;
    SFRCDate = $("#hdCreatedOn").val();
    var Doc_no = SFRCNo;
    var Doc_dt = SFRCDate;
    var QtyDecDigit = $("#QtyDigit").text();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        var IsDisabled = $("#DisableSubItem").val();
        Sub_Quantity = $("#txtTargetSaleQuantity").val();
    }
    else if (flag == "PYearSalQty") {
        var IsDisabled = $("#DisableSubItem").val();
        Sub_Quantity = $("#txtPreviousYearSalesInQuantity").val();
    } else if (flag == "TargtSalSubitmQty") {
        ProductNm = clickdRow.find("#td_ItemName").text();
        ProductId = clickdRow.find("#hdn_item_id").val();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        UOM = clickdRow.find("#tblhdn_Uom_name").val();
        Sub_Quantity = clickdRow.find("#hdn_target_sale_qty").val();
        IsDisabled = "Y";
        flag = "Quantity";
    }
    else if (flag == "ListPYearSalQty") {
        ProductNm = clickdRow.find("#td_ItemName").text();
        ProductId = clickdRow.find("#hdn_item_id").val();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        UOM = clickdRow.find("#tblhdn_Uom_name").val();
        Sub_Quantity = clickdRow.find("#hdn_pys_qty").val();
        IsDisabled = "Y";
        flag = "PYearSalQty";
    }
   
    var hd_Status = $("#hdStatus_Code").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesForecast/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status, 
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            fromdate: fromdate,
            todate: todate
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    return Cmn_CheckValidations_forSubItems("datatable-buttons", "", "hdn_item_id", "hdn_target_sale_qty", "SubItemTargtSalQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("SReturnItmDetailsTbl", "", "hdItemId", "ReturnQuantity", "SubItemReturnQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}