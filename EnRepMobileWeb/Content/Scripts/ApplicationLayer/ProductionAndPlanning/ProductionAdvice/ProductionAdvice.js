$(document).ready(function () {
    debugger;
    BindProductItmList();
    $("#ProductName").select2();
    ddl_src_type_onchange();
    BindTableDataOnDoubleClick();
    var DocNo = $("#txt_AdviceNumber").val();
    $("#hdDoc_No").val(DocNo);
    $("#PAdviceListTble #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var PAdviceNo = clickedrow.children("#adv_no").text();
            var PAdviceDate = clickedrow.children("#advdt").text();
            if (PAdviceNo != null && PAdviceNo != "" && PAdviceDate != null && PAdviceDate != "") {
                window.location.href = "/ApplicationLayer/ProductionAdvice/EditDomesticAdviceDetails/?PAdviceNo=" + PAdviceNo + "&PAdviceDate=" + PAdviceDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $(".RowHighLightMultiTable >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        debugger;
        $(e.target).closest("tr").parent().find("tr").css("background-color", "#ffffff");
        var hfopid = $(e.target).closest("tr").find("#hfopid").val();
        $(clickedrow).parent().find("tr").each(function () {
            var tdlen = $(this).find("td").length;
            if (tdlen == 6) {
                $(this).find("td:eq(2),td:eq(3),td:eq(4),td:eq(5)").css("background-color", "");
            }
        });
        $(clickedrow).parent().find("tr #hfopid[value=" + hfopid + "]").closest("tr").css("background-color", "rgba(38, 185, 154, .16)");

        if ($(clickedrow).find("td").length == 6) {
            if ($(e.target).closest("td").prop("rowspan") == 1) {
                $(clickedrow).find("td:eq(2),td:eq(3),td:eq(4),td:eq(5)").css("background-color", "rgba(38, 185, 154, .115)");
            }
          
        }
        else {
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .24)");
        }
        $(clickedrow).find("#ItmInfoBtnIcon").focus();
        //clickdownAndUp();
    });
    
});


var QtyDecDigit = $("#QtyDigit").text();///Quantity 
//--------------------------------WorkFlow ----------------------------------------//
function CmnGetWorkFlowDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var Doc_No = clickedrow.children("#adv_no").text();
    var Doc_Date = clickedrow.children("#advdt").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Doc_No);
   /* debugger;*/
    Doc_Date = Doc_Date.split(' ')[0];
    if (Doc_Date.split('-')[2].length == 4) {
        Doc_Date = Doc_Date.split('-').reverse().join('-');
    }
    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetAdviceSidebar_Details(Doc_No, Doc_Date);
    GetWorkFlowDetails(Doc_No, Doc_Date, Doc_id, Doc_Status);
}
function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#hd_Status').val().trim();
    //if (OrderStatus === "D" || OrderStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");
    //    var DocNo = $("#txt_AdviceNumber").val().trim();
    //    Cmn_GetForwarderList(Doc_ID, DocNo);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
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
                var OrderStatus = "";
                OrderStatus = $('#hd_Status').val().trim();
                if (OrderStatus === "D" || OrderStatus === "F") {

                    if ($("#hd_nextlevel").val() === "0") {
                        $("#Btn_Forward").attr("data-target", "");
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                        $("#Btn_Approve").attr("data-target", "");
                    }
                    var Doc_ID = $("#DocumentMenuId").val();
                    $("#OKBtn_FW").attr("data-dismiss", "modal");
                    var DocNo = $("#txt_AdviceNumber").val().trim();
                    Cmn_GetForwarderList(Doc_ID, DocNo);

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
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#txt_AdviceNumber").val().trim();
    DocDate = $("#txt_AdviceDate").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (DocNo + ',' + DocDate + ',' + WF_Status1);
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductionAdvice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ProductionAdvice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductionAdvice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductionAdvice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txt_AdviceNumber").val().trim();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}


//-------------------------------------------------WorkFlow----------------------------------------------//

function GetAdviceSidebar_Details(AdvNo, AdvDate) {
    if (AdvNo != null && AdvNo != "" && AdvDate != null && AdvDate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionAdvice/GetProductionAdvice_Detail",
            data: { Adv_No: AdvNo, Adv_Date: AdvDate },
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    //debugger;
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $("#AdviceQuantity").val(parseFloat(arr.Table[0].adv_qty).toFixed(QtyDecDigit));
                        $("#ProducedQuantity").val(parseFloat(arr.Table[0].produceqty).toFixed(QtyDecDigit));
                        $("#RemainingQuantity").val(parseFloat(arr.Table[0].remainingqty).toFixed(QtyDecDigit));
                        $("#AcceptedQuantity").val(parseFloat(arr.Table[0].accept_qty).toFixed(QtyDecDigit));
                        $("#RejectedQuantity").val(parseFloat(arr.Table[0].reject_qty).toFixed(QtyDecDigit));
                        $("#ReworkableQuantity").val(parseFloat(arr.Table[0].rework_qty).toFixed(QtyDecDigit));
                    }
                    else {
                        $("#AdviceQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        $("#ProducedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        $("#RemainingQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        $("#AcceptedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        $("#RejectedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                        $("#ReworkableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    }
                }
                else {
                    $("#AdviceQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    $("#ProducedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    $("#RemainingQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    $("#AcceptedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    $("#RejectedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    $("#ReworkableQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                }
            }
        });
    }
}


function ddl_src_type_onchange() {
    debugger;
    //HideShowAddProPlanBtn();
    var srcval = $("#ddl_src_type").val();
    if (srcval === "P") {
        $("#fysection").removeAttr("style");
    }
    else {
        $("#fysection").css("display", "none");
        $('#ddl_financial_year').val('0').prop('selected', true);
        $('#ddl_period').val('0').prop('selected', true);

        $("#txtFromDate").val("");
        $("#hdn_FromDate").val("");
        $("#txtToDate").val("");
        $("#hdn_ToDate").val("");

        BindProductItmList();
    }
};
function HideShowAddProPlanBtn() {
    var srcval = $("#ddl_src_type").val();
    if (srcval === "P") {
        $("#AddBtnIconMRP").removeAttr("style");

        //ShowForecastfields();
    }
    else {
        $("#AddBtnIconMRP").css("display", "none");
        //HideForecastfields();
    }

    $("#hdn_ddl_src_type").val(srcval);
}
function BindProductItmList() {
    debugger;

    var srctype = $("#ddl_src_type").val();
    var f_fy = $("#ddl_financial_year").val();
    var period = $("#ddl_period").val();
    var firstEmptySelect = true;

    //$('#ddl_ProductName').append(`<option value="0">---Select---</option>`);
    $("#ddl_ProductName").select2({

        ajax: {
            url: "/ApplicationLayer/ProductionAdvice/BindProductList",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    SrcType: srctype, fy: f_fy, period: period,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
<div class="row"><div class="col-md-8 col-xs-6 def-cursor">Item Name</div>
<div class="col-md-4 col-xs-6 def-cursor">UOM</div></div>
</strong></li></ul>`)
                }
                page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                    }),
                    
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            
                $result = $(
                    '<div class="row">' +
                    '<div class="col-md-8 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
           
            return $result;
            firstEmptySelect = false;
        },

    });


        //$.ajax(
        //    {
        //        type: "POST",
        //        url: "/ApplicationLayer/ProductionAdvice/BindProductList",
        //        data: { SrcType: srctype, fy: f_fy, period: period},
        //        dataType: "json",
        //        success: function (data) {
        //            debugger;
        //            if (data !== null && data !== "") {
        //                var arr = [];
        //                arr = JSON.parse(data);
        //                if (arr.Table.length > 0) {
        //                    $("#ddl_ProductName optgroup option").remove();
        //                    $("#ddl_ProductName optgroup").remove();

        //                    $("#ddl_ProductName").append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        //                    for (var i = 0; i < arr.Table.length; i++) {
        //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].product_id}">${arr.Table[i].item_name}</option>`);
        //                    }
        //                    var firstEmptySelect = true;
        //                    $("#ddl_ProductName").select2({
        //                        templateResult: function (data) {
        //                            var selected = $("#ddl_ProductName").val();
        //                            //if (check(data, selected, "#ProductItemDetailsTbl", "#hfsno", '#ddl_ProductName') == true) {
        //                                var UOM = $(data.element).data('uom');
        //                                var classAttr = $(data.element).attr('class');
        //                                var hasClass = typeof classAttr != 'undefined';
        //                                classAttr = hasClass ? ' ' + classAttr : '';
        //                                var $result = $(
        //                                    '<div class="row">' +
        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
        //                                    '</div>'
        //                                );
        //                                return $result;
        //                            //}
        //                            firstEmptySelect = false;
        //                        }
        //                    });

        //                    debugger;
        //                    var productid = $("#hdn_product_id").val();
        //                    if (productid != null && productid != "") {
        //                        $('#ddl_ProductName').val(productid).trigger('change');
        //                    }
        //                }
        //            }
        //            WorkflowdetailLoadFX();
        //        },
        //    });
    }
function OnChange_Fy() {
    var f_fy = $("#ddl_financial_year").val();

    if (f_fy == "0" || f_fy == null) {
        $("#vm_ddl_financial_year").text($("#valueReq").text());
        $("#vm_ddl_financial_year").css("display", "block");
        $("#ddl_financial_year").css("border-color", "red");
    }
    else {
        $("#vm_ddl_financial_year").css("display", "none");
        $("#ddl_financial_year").css("border-color", "#ced4da");
    }
    if (f_fy != "0" && f_fy != null) {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductionAdvice/BindPeriodList",
                data: { fy: f_fy },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#ddl_period option").remove();
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#ddl_period').append(`<option value="${arr.Table[i].f_period}">${arr.Table[i].f_pname}</option>`);
                            }
                        }
                    }
                },
            });
    }
}
function OnChange_Period() {
    var f_fy = $("#ddl_financial_year").val();
    var period = $("#ddl_period").val();
    if (period == "0" || period == null) {
        $("#vm_ddl_period").text($("#valueReq").text());
        $("#vm_ddl_period").css("display", "block");
        $("#ddl_period").css("border-color", "red");
    }
    else {
        $("#vm_ddl_period").css("display", "none");
        $("#ddl_period").css("border-color", "#ced4da");
    }
    if (period != "0" && period != null && f_fy != "0" && f_fy != null) {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductionAdvice/BindPeriodRAndProductList",
                data: { fy: f_fy, period: period },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {

                            if (arr.Table.length > 1) {
                                $("#txtFromDate").val(arr.Table[1].from_date);
                                $("#txtToDate").val(arr.Table[1].to_date);

                                $("#hdn_FromDate").val(arr.Table[1].fromdate);
                                $("#hdn_ToDate").val(arr.Table[1].todate);
                            }
                            else {
                                $("#txtFromDate").val("");
                                $("#txtToDate").val("");
                            }


                            $("#ddl_ProductName optgroup option").remove();
                            $("#ddl_ProductName optgroup").remove();

                            $("#ddl_ProductName").append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].item_id}">${arr.Table[i].item_name}</option>`);
                            }
                            var firstEmptySelect = true;
                            $("#ddl_ProductName").select2({
                                templateResult: function (data) {
                                    var selected = $("#ddl_ProductName").val();
                                    if (check(data, selected, "#ProductItemDetailsTbl", "#hfsno", '#ddl_ProductName') == true) {
                                        var UOM = $(data.element).data('uom');
                                        var classAttr = $(data.element).attr('class');
                                        var hasClass = typeof classAttr != 'undefined';
                                        classAttr = hasClass ? ' ' + classAttr : '';
                                        var $result = $(
                                            '<div class="row">' +
                                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                                            '</div>'
                                        );
                                        return $result;
                                    }
                                    firstEmptySelect = false;
                                }
                            });
                        }
                    }
                },
            });
    }
}
function OnChange_ProductName() {
    debugger;
    var productid = $("#ddl_ProductName").val();
    var hdn_product_id = $("#hdn_product_id").val();
    Cmn_DeleteSubItemQtyDetail(hdn_product_id);
    $("#hdn_product_id").val(productid);
    if (productid == "0" || productid == null) {
        $("#vmddl_ProductName").text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
    }
    else {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }

    if (productid != "0" && productid != null) {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductionAdvice/BindRevisionNnoList",
                data: { productid: productid },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {

                            $("#ddl_RevisionNumber option").remove();

                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#ddl_RevisionNumber').append(`<option value="${arr.Table[i].rev_no}">${arr.Table[i].rev_no}</option>`);
                            }

                            $('#ddl_RevisionNumber').val(arr.Table1[0].rev_no);
                            $("#vm_ddl_RevisionNumber").css("display", "none");
                            $("#ddl_RevisionNumber").css("border-color", "#ced4da");
                            Cmn_BindUOM(null, productid, "", "N", "");
                        }
                        else {
                            $("#ddl_RevisionNumber option").remove();
                            $("#UOM").val("");
                            $("#UOMID").val("");
                        }
                    }
                    else {
                        $("#ddl_RevisionNumber option").remove();
                        $("#UOM").val("");
                        $("#UOMID").val("");
                    }
                },
            });
    }
    else {
        $('#ddl_RevisionNumber').html('<option value="---Select---">---Select---</option>');
    }
}
function OnChange_RevisionNo() {
    debugger;
    var revno = $("#ddl_RevisionNumber").text().trim();

    if (revno == "---Select---" || revno == null) {
        $("#vm_ddl_RevisionNumber").text($("#valueReq").text());
        $("#vm_ddl_RevisionNumber").css("display", "block");
        $("#ddl_RevisionNumber").css("border-color", "red");
    }
    else {
        $("#vm_ddl_RevisionNumber").css("display", "none");
        $("#ddl_RevisionNumber").css("border-color", "#ced4da");
    }

    $("#PA_MaterialDetailsTbl tbody tr").remove();
    $("#AddBtnIcon").css("display", "block");
}
function OnChange_AdviceQty() {
    debugger;
    var advqty = $("#txt_AdviceQuantity").val();
    /*Code add by Hina on 02-03-2024 to show balnk instead of 0 in insert fields*/
    if (advqty == "0" || advqty == parseFloat(0)) {
        $("#txt_AdviceQuantity").val("");
    }
    /*Code End*/
    if (advqty != "" && advqty != null) {
        if (parseInt(advqty) > 0) {
            $("#vm_AdviceQty").css("display", "none");
            $("#txt_AdviceQuantity").css("border-color", "#ced4da");
        }
        else {
            $("#vm_AdviceQty").text($("#valueReq").text());
            $("#vm_AdviceQty").css("display", "block");
            $("#txt_AdviceQuantity").css("border-color", "red");
        }
    }
    else {
        $("#vm_AdviceQty").text($("#valueReq").text());
        $("#vm_AdviceQty").css("display", "block");
        $("#txt_AdviceQuantity").css("border-color", "red");
    }

    $("#PA_MaterialDetailsTbl tbody tr").remove();
    $("#AddBtnIcon").css("display", "block");
}
function OnChange_BatchNo() {
    debugger;
    var batchno = $("#txt_BatchNumber").val();

    if (batchno == "" || batchno == null) {
        $("#vm_BatchNumber").text($("#valueReq").text());
        $("#vm_BatchNumber").css("display", "block");
        $("#txt_BatchNumber").css("border-color", "red");
    }
    else {
        $("#vm_BatchNumber").css("display", "none");
        $("#txt_BatchNumber").css("border-color", "#ced4da");
    }   
}
function OnChange_Completiondt() {
    debugger;
    var completiondt = $("#txtCompletionDate").val();
    if (completiondt == "" || completiondt == null) {
        $("#vm_completiondt").text($("#valueReq").text());
        $("#vm_completiondt").css("display", "block");
        $("#txtCompletionDate").css("border-color", "red");
    }
    else {
        $("#vm_completiondt").css("display", "none");
        $("#txtCompletionDate").css("border-color", "#ced4da");
    }
}
function AddMaterialDetails() {
    debugger;
    var productid = $("#ddl_ProductName").val();
    var RevNo = $("#ddl_RevisionNumber").val();

    var ValidationFlag = CheckHeaderValidation();

    if (ValidationFlag == true) {
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionAdvice/GetMtaterialDetail",
            data: { productid: productid, revno: RevNo },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0 && arr.Table1.length > 0)
                    {
                        var adviceqty = $("#txt_AdviceQuantity").val();
                        if (adviceqty == "" || adviceqty == null) {
                            adviceqty = 0;
                        }
                        var iteminfotitle = $("#ItmInfo").text();
                        $("#PA_MaterialDetailsTbl tbody tr").remove();
                        for (var i = 0; i < arr.Table.length; i++) {
                            var opid = arr.Table[i].op_id;
                            var opidcount = arr.Table[i].op_count;

                            var op_item = arr.Table1.filter(v => v.op_id === opid);
                            op_item.sort(arr.Table1.item_type_seq_no, arr.Table1.item_name);

                            for (var j = 0; j < op_item.length; j++) {
                                if (j === 0) {
                                    $('#PA_MaterialDetailsTbl tbody').append(`<tr>
                                       <td rowspan="${opidcount}">${i + 1}</td>
                                       <td rowspan="${opidcount}" class="center" style="vertical-align:middle;">${op_item[j].op_name}</td>
                                       <td>${op_item[j].Item_type}<input type="hidden" id="hfitmtypeid" value="${op_item[j].item_type_id}" /><input type="hidden" id="hfopid" value="${op_item[j].op_id}" /></td>
                                       <td><div class="col-sm-11" style="padding:0px;">${op_item[j].item_name}<input type="hidden" id="hfitmid" value="${op_item[j].item_id}" /></div> <div class="col-sm-1 i_Icon">
                                       <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${op_item[j].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${iteminfotitle}"> </button>
                                       </div></td>
                                       <td>${op_item[j].uom_alias}<input type="hidden" id="hfuomid" value="${op_item[j].uom_id}" /></td>
                                       <td class="num_right">${(parseFloat(op_item[j].qty) * parseFloat(adviceqty)).toFixed(QtyDecDigit)}<input type="hidden" id="hfqty" value="${(parseFloat(op_item[j].qty) * parseFloat(adviceqty)).toFixed(QtyDecDigit)}" /></td>
                                       </tr>`);
                                }
                                else {
                                    $('#PA_MaterialDetailsTbl tbody').append(`<tr>
                                       <td>${op_item[j].Item_type}<input type="hidden" id="hfitmtypeid" value="${op_item[j].item_type_id}" /><input type="hidden" id="hfopid" value="${op_item[j].op_id}" /></td>
                                       <td><div class="col-sm-11" style="padding:0px;">${op_item[j].item_name}<input type="hidden" id="hfitmid" value="${op_item[j].item_id}" /></div><div class="col-sm-1 i_Icon">
                                       <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${op_item[j].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${iteminfotitle}"> </button>
                                       </div></td>
                                       <td>${op_item[j].uom_alias}<input type="hidden" id="hfuomid" value="${op_item[j].uom_id}" /></td>
                                       <td class="num_right">${(parseFloat(op_item[j].qty) * parseFloat(adviceqty)).toFixed(QtyDecDigit)}<input type="hidden" id="hfqty" value="${(parseFloat(op_item[j].qty) * parseFloat(adviceqty)).toFixed(QtyDecDigit)}" /></td>
                                       </tr>`);
                                }
                            }
                        }
                    }
                    else {
                        $("#PA_MaterialDetailsTbl tbody tr").remove();
                    }

                    $("#AddBtnIcon").css("display", "none");
                }
                else {
                    $("#PA_MaterialDetailsTbl tbody tr").remove();
                }
            },
        });
}
}
function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 11-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var rowcount = $('#PA_MaterialDetailsTbl tbody tr').length;
    var ValidationFlag = CheckHeaderValidation();
    
    if (ValidationFlag == true) {
        var flagsubItemVald = CheckValidations_forSubItems();
        if (flagsubItemVald == false) {
            return false;
        }
        if (rowcount > 0) {

            var srctype = $("#ddl_src_type").val();
            var fy = $("#ddl_financial_year").val();
            var period = $("#ddl_period").val();
            var productid = $("#ddl_ProductName").val();
            var revno = $("#ddl_RevisionNumber").val();
            $("#hdn_ddl_src_type").val(srctype);
            $("#hdn_ddl_financial_year").val(fy);
            $("#hdn_ddl_period").val(period);
            $("#hdn_product_id").val(productid);
            $("#hdn_RevisionNumber").val(revno);

            var MaterialItemDetailList = new Array();

            $("#PA_MaterialDetailsTbl TBODY TR").each(function () {
                var row = $(this);
                var ItemList = {};
                ItemList.OpId = row.find("#hfopid").val();
                ItemList.InputTypeId = row.find('#hfitmtypeid').val();
                ItemList.ItemId = row.find('#hfitmid').val();
                ItemList.UomId = row.find('#hfuomid').val();
                ItemList.Qty = row.find('#hfqty').val();

                MaterialItemDetailList.push(ItemList);
            });

            var str = JSON.stringify(MaterialItemDetailList);
            $('#hdn_materialdetail').val(str);

            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
            /*----- Attatchment End--------*/
            /*----- Attatchment start--------*/
            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $('#hdn_Attatchment_details').val(ItemAttchmentDt);
      /*----- Attatchment End--------*/
        //InsertItemDetail();

            var SubItemsListArr = Cmn_SubItemList();
            var strSubItemList = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(strSubItemList);
            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;
        }
        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function CheckHeaderValidation() {
    var chkvali = "N";
    var src = $("#ddl_src_type").val();
    var f_fy = $("#ddl_financial_year").val();
    var period = $("#ddl_period").val();
    var productid = $("#ddl_ProductName").val();
    var revno = $("#ddl_RevisionNumber").val();
    //var revnot = $("#ddl_RevisionNumber").text();
    var advqty = $("#txt_AdviceQuantity").val();
    var batchno = $("#txt_BatchNumber").val();
    var completiondt = $("#txtCompletionDate").val();
    var Cancel = $("#cancelflag").is(":checked");
    if (src === "P" && Cancel == false) {
        if (f_fy == "0" || f_fy == null) {
            $("#vm_ddl_financial_year").text($("#valueReq").text());
            $("#vm_ddl_financial_year").css("display", "block");
            $("#ddl_financial_year").css("border-color", "red");
            chkvali = "Y";
        }
        else {
            $("#vm_ddl_financial_year").css("display", "none");
            $("#ddl_financial_year").css("border-color", "#ced4da");
        }
        if (period == "0" || period == null) {
            $("#vm_ddl_period").text($("#valueReq").text());
            $("#vm_ddl_period").css("display", "block");
            $("#ddl_period").css("border-color", "red");
            chkvali = "Y";
        }
        else {
            $("#vm_ddl_period").css("display", "none");
            $("#ddl_period").css("border-color", "#ced4da");
        }
    }

    if (productid == "0" || productid == null) {
        $("#vmddl_ProductName").text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        //$("#ddl_ProductName").css("border-color", "red");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        chkvali = "Y";
    }
    else {
        $("#vmddl_ProductName").css("display", "none");
        //$("#ddl_ProductName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    if (revno == "---Select---" || revno == null) {
        $("#vm_ddl_RevisionNumber").text($("#valueReq").text());
        $("#vm_ddl_RevisionNumber").css("display", "block");
        $("#ddl_RevisionNumber").css("border-color", "red");
        chkvali = "Y";
    }
    else {
        $("#vm_ddl_RevisionNumber").css("display", "none");
        $("#ddl_RevisionNumber").css("border-color", "#ced4da");
    }
    if (advqty == "" || advqty == null) {
        $("#vm_AdviceQty").text($("#valueReq").text());
        $("#vm_AdviceQty").css("display", "block");
        $("#txt_AdviceQuantity").css("border-color", "red");
        chkvali = "Y";
    }
    else {
        if (parseInt(advqty) > 0) {
            $("#vm_AdviceQty").css("display", "none");
            $("#txt_AdviceQuantity").css("border-color", "#ced4da");
        }
        else {
            $("#vm_AdviceQty").text($("#valueReq").text());
            $("#vm_AdviceQty").css("display", "block");
            $("#txt_AdviceQuantity").css("border-color", "red");
            chkvali = "Y";
        }
    }
    if (batchno == "" || batchno == null) {
        $("#vm_BatchNumber").text($("#valueReq").text());
        $("#vm_BatchNumber").css("display", "block");
        $("#txt_BatchNumber").css("border-color", "red");
        chkvali = "Y";
    }
    else {
        $("#vm_BatchNumber").css("display", "none");
        $("#txt_BatchNumber").css("border-color", "#ced4da");
    }
    if (completiondt == "" || completiondt == null) {
        $("#vm_completiondt").text($("#valueReq").text());
        $("#vm_completiondt").css("display", "block");
        $("#txtCompletionDate").css("border-color", "red");
        chkvali = "Y";
    }
    else {
        $("#vm_completiondt").css("display", "none");
        $("#txtCompletionDate").css("border-color", "#ced4da");
    }

    if (chkvali == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function BtnSearch() {
    debugger;
    FilterPAdviceListData();
    ResetWF_Level();
}
function FilterPAdviceListData() {
    debugger;
    try {
        var Source = $("#ddl_src_type").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var productid = $("#ProductName").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionAdvice/ProductionAdviceListSearch",
            data: {
                Productid: productid,
                Source: Source,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#PAdviceListTble').html(data);
                $('#ListFilterData').val(productid + ',' + Source + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Advice Error : " + err.message);

    }
}
function BindTableDataOnDoubleClick() {
    debugger;
    var opdetails = $("#hdn_opdetail").val();
    var opitemdetails = $("#hdn_opitemdetail").val();

    if (opdetails !== null && opdetails !== "" && opdetails !== undefined && opitemdetails !== null && opitemdetails !== "" && opitemdetails !== undefined) {

        var oparr = [];
        var opitmarr = [];
        oparr = JSON.parse(opdetails);
        opitmarr = JSON.parse(opitemdetails);

        if (oparr.length > 0 && opitmarr.length > 0) {
            var iteminfotitle = $("#ItmInfo").text();
            $("#PA_MaterialDetailsTbl tbody tr").remove();
            for (var i = 0; i < oparr.length; i++) {
                var opid = oparr[i].op_id;
                var opidcount = oparr[i].op_count;

                var op_item = opitmarr.filter(v => v.op_id === opid);
                op_item.sort(opitmarr.item_type_seq_no, opitmarr.item_name);

                for (var j = 0; j < op_item.length; j++) {
                    if (j === 0) {
                        $('#PA_MaterialDetailsTbl tbody').append(`<tr>
                                       <td rowspan="${opidcount}">${i + 1}</td>
                                       <td rowspan="${opidcount}" class="center" style="vertical-align:middle;">${op_item[j].op_name}</td>
                                       <td>${op_item[j].Item_type}<input type="hidden" id="hfitmtypeid" value="${op_item[j].Item_typeid}" /><input type="hidden" id="hfopid" value="${op_item[j].op_id}" /></td>
                                       <td><div class="col-sm-11" style="padding:0px;">${op_item[j].item_name}<input type="hidden" id="hfitmid" value="${op_item[j].item_id}" /></div> <div class="col-sm-1 i_Icon">
                                       <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${op_item[j].item_id}','${op_item[j].item_name}','edit');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${iteminfotitle}"> </button>
                                       </div></td>
                                       <td>${op_item[j].uom_alias}<input type="hidden" id="hfuomid" value="${op_item[j].uom_id}" /></td>
                                       <td class="num_right">${parseFloat(op_item[j].qty).toFixed(QtyDecDigit)}<input type="hidden" id="hfqty" value="${parseFloat(op_item[j].qty).toFixed(QtyDecDigit)}" /></td>
                                       </tr>`);
                    }
                    else {
                        $('#PA_MaterialDetailsTbl tbody').append(`<tr>
                                       <td>${op_item[j].Item_type}<input type="hidden" id="hfitmtypeid" value="${op_item[j].Item_typeid}" /><input type="hidden" id="hfopid" value="${op_item[j].op_id}" /></td>
                                       <td><div class="col-sm-11" style="padding:0px;">${op_item[j].item_name}<input type="hidden" id="hfitmid" value="${op_item[j].item_id}" /></div><div class="col-sm-1 i_Icon">
                                       <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${op_item[j].item_id}','${op_item[j].item_name}','edit');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${iteminfotitle}"></button>
                                       </div></td>
                                       <td>${op_item[j].uom_alias}<input type="hidden" id="hfuomid" value="${op_item[j].uom_id}" /></td>
                                       <td class="num_right">${parseFloat(op_item[j].qty).toFixed(QtyDecDigit)}<input type="hidden" id="hfqty" value="${parseFloat(op_item[j].qty).toFixed(QtyDecDigit)}" /></td>
                                       </tr>`);
                    }
                }
            }
        }
    }
}
function OnClickIconBtn(e, item_id) {
    debugger;
    var ItmCode = "";
    if (item_id == "" || item_id == null) {
        ItmCode = $("#ddl_ProductName").val();
    }
    else {
        ItmCode = item_id;
    }
    if (ItmCode != "" && ItmCode != null) {
        ItemInfoBtnClick(ItmCode);
    }
}
function functionConfirm(event) {
    debugger;
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
function OnClickCancelFlag() {
    if ($("#cancelflag").is(":checked")) {
        $("#btn_save").attr('onclick', "return CheckFormValidation();");
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");


        }
    }

}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, "NoRow", "sub_item", "SubItemAdvQty",);
}
function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    //var hfsno = clickdRow.find("#hfsno").val();
    var ProductNm = $("#ddl_ProductName option:selected").text();
    var ProductId = $("#ddl_ProductName").val();
    var UOM = $("#UOM").val();
    //var UOMID = clickdRow.find("#UOMID").val();
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
        Sub_Quantity = $("#txt_AdviceQuantity").val();

    }
    else {
        Sub_Quantity = $("#Protxt_AdviceQuantitydQuantity").val();
    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hd_Status").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var doc_no = $("#txt_AdviceNumber").val();
    var doc_dt = $("#txt_AdviceDate").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionAdvice/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            doc_no: doc_no,
            doc_dt: doc_dt
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);


        }

    })
    if (flag == 'enable') {

    }
    else if (flag = 'readonly') {

    }
}
function CheckValidations_forSubItems() {
    return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_AdviceQuantity", "SubItemAdvQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItemsNonTable("ddl_ProductName", "txt_AdviceQuantity", "SubItemAdvQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() { /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
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