/************************************************
Javascript Name:Material Dispatch SC Detail
Created By:Hina Sharma
Created Date: 30-01-2023
Description: This Javascript use for the Material Dispatch SC many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    BindDDlContractorList();
    var Supp_IdNm = $("#SupplierName").val();
    var MDissueNum = $("#IssueNumber").val();
    var Message = $("#hdn_massage").val();
    if (Message == "DocModify") {

    }
    else {
        if (MDissueNum == null || MDissueNum == "") {
            debugger;
            BindJobOrderNumber(Supp_IdNm);
        }
    }
    $('#MaterialDispatchItemDetailsTbl tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        SerialNoAfterDelete();
        if ($("#MaterialDispatchItemDetailsTbl >tbody >tr").length == 0) {
            debugger;
            var suppnameID = $("#SupplierName").val();
            $("#Job_Ord_number").prop("disabled", false);
            $("#DispatchQuantity").prop("readonly", true);
            $("#SupplierName").prop("disabled", false);
            $(".plus_icon1").css("display", "none");
            BindJobOrderNumber(suppnameID);
            $("#OperationName").val("");
            $("#ProductName").val("");
            $("#UOMName").val("");
            $("#OrderQuantity").val("");
            $("#PendingQuantity").val("");
            $("#DispatchQuantity").val("");
            $('#SaveItemBatchTbl tbody tr').empty();
            $('#SaveItemSerialTbl tbody tr').empty();
            $('#SaveItemBatchTbl tbody tr').remove();
            $('#SaveItemSerialTbl tbody tr').remove();
            debugger;
            $("#HDSelectedBatchwise").val("");
            $("#HDSelectedSerialwise").val("");
            //BindItemBatchDetail();
            //BindItemSerialDetail();
        }
        else {
            debugger;
            var MDItemId = $(this).closest('tr').find("#hdItemId").val();
            $("#SaveItemBatchTbl >tbody >tr").each(function () {
                debugger;
                var row = $(this);
                var BatchItemID = row.find("#mi_lineBatchItemId").val();
                if (MDItemId == BatchItemID) {
                    $(this).closest('tr').remove();
                    $("#HDSelectedBatchwise").val("");
                }
            });
            $("#SaveItemSerialTbl >tbody >tr").each(function () {
                debugger;
                var row = $(this);
                var SerialItemID = row.find("#mi_lineSerialItemId").val();
                if (MDItemId == SerialItemID) {
                    $(this).closest('tr').remove();
                    $("#HDSelectedSerialwise").val("");
                }
            });
        }
    });
    var SuppName = $("#SupplierName").val();
    $("#Hdn_SupplierName").val(SuppName);
    var JobOrdNo = $("#Job_Ord_number").val();
    $("#Hdn_JobNum").val(JobOrdNo);

    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        ////debugger;

        var clickedrow = $(e.target).closest("tr");
        if (clickedrow.find('#OrderTypeLocal').prop("disabled") == false) {
            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

            clickedrow.find('#OrderTypeLocal').prop("checked", true);
            var Address_id = clickedrow.find("#addhd").text();
            var Address = clickedrow.find("#cust_add").text();
            var pin = clickedrow.find("#cust_pin").text();
            var city = clickedrow.find("#city_name").text();
            var dist = clickedrow.find("#district_name").text();
            var state = clickedrow.find("#state_name").text();
            var country = clickedrow.find("#country_name").text();

            $('#hd_TxtBillingAddr').val(Address + ',' + ' ' + city + '-' + ' ' + pin + ',' + ' ' + dist + ',' + ' ' + state + ',' + ' ' + country);
            $("#hd_bill_add_id").val(Address_id);
        }
    });
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#MaterialDispatchItemDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hdItemId').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
    md_no = $("#IssueNumber").val();
    $("#hdDoc_No").val(md_no);
});

function BindDDlContractorList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });

}
function OnChangeContractorName(SuppID) {
    debugger;
    var Supp_id = SuppID.value;
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")

    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_MDSupplierName").val(SuppName)

        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")

    }
    BindJobOrderNumber(Supp_id);
    $("#DispatchQuantity").val("");
    $("#OperationName").val("");
    $("#ProductName").val("");
    $("#UOMName").val("");
    $("#OrderQuantity").val("");
    $("#PendingQuantity").val("");
    $("#DispatchQuantity").val("");
    $("#DispatchQuantity").prop("readonly", true);
    $("#SpanDispatchQuantityErrorMsg").css("display", "none");
    $("#DispatchQuantity").css("border-color", "#ced4da");
    $(".plus_icon1").css("display", "none");
    //var Supp_id = SuppID.value;
    //if (Supp_id == "0") {
    //    $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
    //    $("#SpanSuppNameErrorMsg").css("display", "block");
    //    $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")

    //    $("#Address").val("");

    //    $('#Job_Ord_number').attr("disabled",true)
    //   $("#DispatchQuantity").val("");
    //   $("#DispatchQuantity").prop("readonly", true);
    //   $("#OperationName").val("");
    //   $("#ProductName").val("");
    //    $("#UOMName").val("");
    //   $("#OrderQuantity").val("");
    //  $("#PendingQuantity").val("");
    //    $(".plus_icon1").css("display", "none");
    //    $("#SpanDispatchQuantityErrorMsg").css("display", "none");
    //    $("#DispatchQuantity").css("border-color", "#ced4da");
    //}
    //else {
    //    $("#SpanSuppNameErrorMsg").css("display", "none");
    //    $("#SupplierName").css("border-color", "#ced4da");
    //    $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
    //    $("#Job_Ord_number").attr('disabled', false);
    //}


    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/MaterialDispatchSC/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    //debugger;
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Rqrdaddr").css("display", "");
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            //CheckJOHeaderValidations();
                            if ($("#Address").val() === "") {
                                $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
                                $("#Address").css("border-color", "Red");
                                $("#SpanSuppAddrErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppAddrErrorMsg").css("display", "none");
                                $("#Address").css("border-color", "#ced4da");
                            }
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function BindJobOrderNumber(Supp_IdNm) {
    debugger;
    //SuppID = $("#SupplierName").val();

    $.ajax({
        type: 'POST',
        url: '/ApplicationLayer/MaterialDispatchSC/GetJobORDDocList',
        data: { Supp_IdNm: Supp_IdNm },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            }
            if (data !== null && data !== "" && data !== "{}") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0 || arr.Table1.length > 0) {
                    debugger; var status = $("#hfStatus").val();

                    $("#Job_Ord_number option").remove();
                    $("#Job_Ord_number optgroup").remove();

                    $('#Job_Ord_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-fitem='${$("#span_FinishedProduct").text()}' data-fuom='${$("#ItemUOM").text()}' data-opname='${$("#span_OpName").text()}' ></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no + ',' + arr.Table[i].fitem_id + ',' + arr.Table[i].uom_id + ',' + arr.Table[i].op_id}" data-fitem = "${arr.Table[i].fitem_name}" data-fuom = "${arr.Table[i].fuom}" data-opname = "${arr.Table[i].op_name}">${arr.Table[i].doc_no}</option>`);
                    }
                    var firstEmptySelect = true;

                    $('#Job_Ord_number').select2({
                        templateResult: function (data) {
                            debugger;
                            var DocDate = $(data.element).data('date');
                            var fitem = $(data.element).data('fitem');
                            var fuom = $(data.element).data('fuom');
                            var opname = $(data.element).data('opname');
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-3 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                '<div class="col-md-2 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                '<div class="col-md-3 col-xs-12' + classAttr + '">' + fitem + '</div>' +
                                '<div class="col-md-1 col-xs-12' + classAttr + '">' + fuom + '</div>' +
                                '<div class="col-md-3 col-xs-12' + classAttr + '">' + opname + '</div>' +
                                '</div>'
                            );
                            return $result;
                            firstEmptySelect = false;
                        }
                    });

                    $("#Job_ord_date").val("");
                }

            }
        }

    })

}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var JODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        JODTransType = "Update";
    }

    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, JODTransType);
}
//function OnchangeJobORDDocNumber1() {
//    debugger;
//    var suppname = $("#SupplierName").val();

//    var doc_no = $("#Job_Ord_number").val();
//    var docno = doc_no.split(',');
//    var prdordNo = docno[0];
//    var OPid = docno[1];
//    var Productid = docno[2];
//    var ProductName = docno[3];
//    var Uomid = docno[4];
//    var UomName = docno[5];
//    var OrderQty = docno[6];
//    var PendQty = docno[7];


//    var DocMenuId = $("#DocumentMenuId").val();
//    var QtyDecDigit = $("#QtyDigit").text();///Quantity
//    /*var PendingQty = parseFloat(0).toFixed(QtyDecDigit);*/

//    debugger

//    if ($("#SupplierName").val() === "0") {
//        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
//        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
//        $("#SpanSuppNameErrorMsg").css("display", "block");
//        $('#Job_Ord_number').empty().append('<option value="---Select----0" selected="selected">---Select---</option>');
//        BindJobOrderNumber(suppname);
//        ErrorFlag = "Y";
//    }
//    else {
//        $("#SpanSuppNameErrorMsg").css("display", "none");
//        $("#SupplierName").css("border-color", "#ced4da");
//        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

//        if (prdordNo != 0) {
//            debugger;
//            $("#SpanJobOrdNoErrorMsg").css("display", "none");
//            $("#Job_Ord_number").css("border-color", "#ced4da");
//            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "#ced4da");
//            var doc_Date = $("#Job_Ord_number option:selected")[0].dataset.date
//            var newdate = doc_Date.split("-").reverse().join("-");
//            var OpName = $("#Job_Ord_number option:selected")[0].dataset.opname


//            $("#Job_ord_date").val(newdate);
//            $("#Job_Ord_number").val(prdordNo);

//            $("#Hdn_JobNum").val(prdordNo);
//            $("#OperationName").val(OpName)
//            $("#Hdn_OPid").val(OPid)
//            $("#ProductName").val(ProductName)
//            $("#Hdn_Prodctid").val(Productid)
//            $("#UOMName").val(UomName)
//            $("#Hdn_Uomid").val(Uomid)
//            $("#OrderQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit))

//            //PendingQty = parseFloat($("#PendingQuantity").val()).toFixed(QtyDecDigit);
//            //if (AvoidDot(PendingQty) == false) {
//            //    $("#DispatchQuantity").val("");
//            //    PendingQty = 0;
//            //}
//            //if (PendingQty === "" || PendingQty === "0.000") {
//            //    $("#PendingQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit))
//            //}
//            if (PendQty == "null") {
//                $("#PendingQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit))
//            }
//            else {
//                $("#PendingQuantity").val(parseFloat(PendQty).toFixed(QtyDecDigit))
//            }

//            $("#DispatchQuantity").val("");
//            $("#DispatchQuantity").prop("readonly", false);
//            $(".plus_icon1").css("display", "block");
//        }
//        else {
//            $("#SpanJobOrdNoErrorMsg").text($("#valueReq").text());
//            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "red")
//            $("#SpanJobOrdNoErrorMsg").css("display", "block");
//        }
//    }
//}
function OnchangeJobORDDocNumber() {
    debugger;
    var doc_no = $("#Job_Ord_number").val();
    if (doc_no == "---Select---,0,0,0") {
        $("#SpanJobOrdNoErrorMsg").text($("#valueReq").text());
        $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "red")
        $("#SpanJobOrdNoErrorMsg").css("display", "block");
        $("#Job_ord_date").val("");
        $("#DispatchQuantity").val("");
        $("#OperationName").val("");
        $("#ProductName").val("");
        $("#UOMName").val("");
        $("#OrderQuantity").val("");
        $("#PendingQuantity").val("");
        $("#DispatchQuantity").prop("readonly", true);
        $("#SpanDispatchQuantityErrorMsg").css("display", "none");
        $("#DispatchQuantity").css("border-color", "#ced4da");
        $(".plus_icon1").css("display", "none");
    }
    else {
        var docno = doc_no.split(',');
        var prdordNo = docno[0];
        if (prdordNo != 0) {
            debugger;
            $("#SpanJobOrdNoErrorMsg").css("display", "none");
            $("#Job_Ord_number").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "#ced4da");
            var QtyDecDigit = $("#QtyDigit").text();
            var Disp_No = $("#IssueNumber").val();
            var doc_no = $("#Job_Ord_number").val();
            var docno = doc_no.split(',');
            var JobordNo = docno[0];
            $("#Hdn_JobNum").val(JobordNo);
            var OPid = docno[1];
            var doc_Date = $("#Job_Ord_number option:selected")[0].dataset.date
            var JobOrddate = doc_Date.split("-").reverse().join("-");

            try {
                $.ajax(
                    {
                        type: "POST",
                        url: "/ApplicationLayer/MaterialDispatchSC/GetDetailofJobOrdNoList",
                        data: {
                            JobordNo: JobordNo,
                            JobOrddate: JobOrddate,
                            Disp_No: Disp_No
                        },
                        success: function (data) {
                            //debugger;
                            if (data == 'ErrorPage') {
                                PO_ErrorPage();
                                return false;
                            }
                            if (data !== null && data !== "") {
                                var arr = [];
                                arr = JSON.parse(data);
                                if (arr.Table.length > 0) {
                                    debugger;
                                    var JobOrd_Typ = "";
                                    var JobOrdTyp = arr.Table[0].src_type;
                                    if (JobOrdTyp == "D") {
                                        JobOrd_Typ = "Direct"
                                    }
                                    else {
                                        JobOrd_Typ = "PrdOrd"
                                    }
                                    $("#hdnJobOrdTyp").val(JobOrd_Typ);
                                    var JoTyp = $("#hdnJobOrdTyp").val();
                                    if (JoTyp == "Direct") {
                                        $("#DivMDOpName").css("display", "none");
                                        $("#DivMDOpOutPrdct").css("display", "none");
                                        $("#DivMDOpOutPrdctUOM").css("display", "none");
                                        
                                    }
                                    $("#Job_Ord_number").val(arr.Table[0].doc_no);
                                    $("#Hdn_JobNum").val(arr.Table[0].doc_no);
                                    // $("#Job_ord_date").val(arr.Table[0].doc_dt);
                                    $("#Job_ord_date").val(JobOrddate);
                                    $("#FinishProduct").val(arr.Table[0].Fitem)
                                    $("#Hdn_FinishProductid").val(arr.Table[0].fg_product_id)
                                    $("#FinishUom").val(arr.Table[0].Fuom)
                                    $("#Hdn_FinishUomId").val(arr.Table[0].fg_uom_id)
                                    $("#OperationName").val(arr.Table[0].op_name);
                                    $("#Hdn_OPid").val(arr.Table[0].op_id);
                                    $("#Hdn_Prodctid").val(arr.Table[0].item_id);
                                    $("#ProductName").val(arr.Table[0].item_name);
                                    $("#Hdn_Uomid").val(arr.Table[0].uom_id);
                                    $("#UOMName").val(arr.Table[0].UomName);
                                    HideShowPageWise(arr.Table[0].sub_item, "NoRow");
                                    $("#OrderQuantity").val((arr.Table[0].OrderQty).toFixed(QtyDecDigit));
                                    $("#PendingQuantity").val((arr.Table[0].PendQty).toFixed(QtyDecDigit));

                                }
                                else {
                                    $("#ProductName").val("");
                                    //$("#conv_rate").val("0");
                                }
                            }
                        },
                    });
            } catch (err) {
                console.log("GetMenuData Error : " + err.message);
            }
            //if (PendQty == "null") {
            //    $("#PendingQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit))
            //}
            //else {
            //    $("#PendingQuantity").val(parseFloat(PendQty).toFixed(QtyDecDigit))
            //}

            $("#DispatchQuantity").val("");
            $("#DispatchQuantity").prop("readonly", false);
            $(".plus_icon1").css("display", "block");
        }
        else {
            $("#SpanJobOrdNoErrorMsg").text($("#valueReq").text());
            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "red")
            $("#SpanJobOrdNoErrorMsg").css("display", "block");
        }
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#hdItemId").val();
    ItemInfoBtnClick(ItmCode)

}
function OnchangeDispatchQty() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    DisptQty = $("#DispatchQuantity").val();
    var PendingQty = parseFloat(0).toFixed(QtyDecDigit);
    var DispatchQty = parseFloat(0).toFixed(QtyDecDigit);

    PendingQty = parseFloat($("#PendingQuantity").val()).toFixed(QtyDecDigit);
    DispatchQty = parseFloat($("#DispatchQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(DispatchQty) == false) {
        $("#DispatchQuantity").val("");
        DispatchQty = "";
    }
    if (DisptQty == "0" || DisptQty == parseFloat(0)) {
        $("#DispatchQuantity").val("");
    }
    /* if (DispatchQty != "" || DispatchQty != "0.000" || DispatchQty != 'NaN') {*/
    if (DisptQty != "" || DisptQty != "0" || DisptQty != parseFloat(0) || DispatchQty != "") {

        if (parseFloat(CheckNullNumber(DispatchQty)) < parseFloat(CheckNullNumber(PendingQty)) || parseFloat(CheckNullNumber(DispatchQty)) == parseFloat(CheckNullNumber(PendingQty))) {
            parseFloat($("#DispatchQuantity").val(DispatchQty)).toFixed(QtyDecDigit);
            $("#SpanDispatchQuantityErrorMsg").css("display", "none");
            $("#DispatchQuantity").css("border-color", "#ced4da");
        }
        else {
            $("#SpanDispatchQuantityErrorMsg").text($("#ExceedingPendingQuantity").text());
            $("#SpanDispatchQuantityErrorMsg").css("display", "block");
            $("#DispatchQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }

    }
    else {
        $("#SpanDispatchQuantityErrorMsg").text($("#valueReq").text());
        $("#SpanDispatchQuantityErrorMsg").css("display", "block");
        $("#DispatchQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}

function AmountFloatRate(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
}
function AmountFloatVal(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#QuotedPrice").val();
    item_rate = CheckNullNumber(item_rate);

    var selectedval = Cmn_SelectedTextInTextField(evt);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }

    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;

        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    //clickedrow.find("#item_disc_amtError").css("display", "none");
    //clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
    return true;
}
function AmountFloatQty(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
}

/*------------Start Section Item DEtail--------------------------------*/
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount

    if (MDHeaderValidations() == false) {
        return false;
    }
    if (CheckValidations_forHeaderSubItems() == false) {
        return false;
    }

    var JONo = $("#Hdn_JobNum").val();
    var JODate = $('#Job_ord_date').val();
    var OrdQty = parseFloat($("#OrderQuantity").val()).toFixed(QtyDecDigit);
    var DispatchQty = parseFloat($("#DispatchQuantity").val()).toFixed(QtyDecDigit);
    var JobOrdTyp = $("#hdnJobOrdTyp").val();

    $.ajax(
        {
            type: "Post",
            url: "/ApplicationLayer/MaterialDispatchSC/getMaterialInputItemDetailByJONumber",
            data: {

                JONo: JONo,
                JODate: JODate,
                OrdQty: OrdQty,
                DispatchQty: DispatchQty
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        var rowIdx = 0;
                        var WHAndAvlStk = "";
                        var Iserial = "";
                        var Ibatch = "";
                        var ItemType = "";
                        var IssuedQuantity = "";

                        for (var i = 0; i < arr.Table.length; i++) {
                            debugger;
                            var subitmDisable = "";
                            if (arr.Table[i].sub_item != "Y") {
                                subitmDisable = "disabled";
                            }

                            WHAndAvlStk = `  <td width="7%">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <div class="lpo_form">
                                                                    <select class="form-control" id="wh_id${rowIdx + 1}" onchange="OnChangeWarehouse(this,event)"><option value="0">---Select---</option></select>
                                                                    <span id="wh_Error${rowIdx + 1}" class="error-message is-visible"></span></div> 
                                                                    </div> 
                                                                     <div class="col-sm-1 i_Icon">
                                                                    <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input id="AvailableStock" value="${parseFloat(arr.Table[i].AvailableQuantity).toFixed(QtyDecDigit)}" readonly class="form-control num_right" autocomplete="off" type="text" name="AvailableStock" placeholder="0000.00"  >
                                                             </td>`;
                            if (arr.Table[i].i_serial == 'Y') {
                                Iserial = ` <td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="Y" style="display: none;" /></td>`;
                            }
                            else {
                                Iserial = ` <td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" disabled class="calculator subItmImg " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="N" style="display: none;" /></td>`;
                            }

                            if (arr.Table[i].i_batch == 'Y') {
                                Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetails" onchange="OnChangeIssueQty" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                              <input type="hidden" id="hdi_batch" value="Y" style="display: none;" /></td>`;
                            }
                            else {
                                Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetails" onchange="OnChangeIssueQty" disabled class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                            }

                            if (arr.Table[i].i_batch == 'N' && arr.Table[i].i_serial == 'N') {
                                ItemType = `<td style="display:none"><input id="ItemType" type="text" value="YES" /></td>`;
                            }
                            else {
                                ItemType = `<td style="display:none"><input id="ItemType" type="text" value="NO" /></td>`;
                            }
                            if (JobOrdTyp == "Direct") {
                                IssuedQuantity=    `<td>
                                                        <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="IssuedQuantity" value="${parseFloat(arr.Table[i].Req_Qty).toFixed(QtyDecDigit)}" disabled onpaste="return CopyPasteData(event);" onchange="OnChangeIssueQuantity(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                            <span id="IssuedQuantity_Error" class="error-message is-visible"></span>
                                                        </div>
                                                        <div class="col-sm-2 i_Icon no-padding" id="div_MDSubItemIssueQty" >
                                                            <button type="button" id="MDSubItemIssueQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('MDIssueQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
                                                        </div>
                                                    </td>`
                            }
                            else {
                                IssuedQuantity=   `<td>
                                                        <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="IssuedQuantity" value="${parseFloat(arr.Table[i].Req_Qty).toFixed(QtyDecDigit)}" onpaste="return CopyPasteData(event);" onchange="OnChangeIssueQuantity(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                            <span id="IssuedQuantity_Error" class="error-message is-visible"></span>
                                                        </div>
                                                        <div class="col-sm-2 i_Icon no-padding" id="div_MDSubItemIssueQty" >
                                                            <button type="button" id="MDSubItemIssueQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('MDIssueQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
                                                        </div>
                                                    </td>`
                            }

                            //<td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$(" #Span_Delete_Title").text()}"></i></td >


                            $('#MaterialDispatchItemDetailsTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                             <td id="SRNO" class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value="${arr.Table[i].item_name}" class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='Order Number'" disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                    <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                    </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value="${arr.Table[i].UOMName}" id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table[i].uom_id}" style="display: none;" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="RequiredQuantity"  value="${parseFloat(arr.Table[i].Req_Qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequiredQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <div class="col-sm-2 i_Icon no-padding" id="div_MDSubItemReqQty" >
                                                                
                                                                <button type="button" id="MDSubItemReqQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('MDReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            
                                                            `+ WHAndAvlStk + `
                                                            `+ IssuedQuantity + `
                                                            <td>
                                                                <input id="MDValue" onpaste="return CopyPasteData(event);" onchange="OnChangeValue(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="value" placeholder="${$("#span_Value").text()}">
                                                                <input type="hidden" id="hdMDValue" style="display: none;" />
                                                            </td>
                                                            `+ Ibatch + `
                                                            `+ Iserial + `
                                                            `+ ItemType + `
                                                            <td>
                                                                <textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="message" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"></textarea>
                                                            </td>
                                                        </tr>
                                `);

                            BindWarehouseList(rowIdx);
                            /*MdItmIssuesub_item*/
                            $("#Job_Ord_number").prop("disabled", true);
                            $("#DispatchQuantity").prop("readonly", true);
                            $("#SupplierName").prop("disabled", true);
                            debugger;
                            $(".plus_icon1").css("display", "none");

                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
                //   alert("some error");
            }
        });
}

function BindWarehouseList(id) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialDispatchSC/GetWarehouseList",
            data: {},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        $("#wh_id" + id).html(s);
                    }
                }
            },
        });
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        BindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdItemId").val();;
        var CompId = $("#HdCompId").val();
        var BranchId = $("#HdBranchId").val();
        var ItemId = clickedrow.find("#hdItemId").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();

        $("#WareHouseWiseItemName").val(ItemName);
        $("#WareHouseWiseUOM").val(UOMName);
        $.ajax(
            {

                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId,
                    //CompId: CompId,
                    //BranchId: BranchId
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnChangeWarehouse(el, evt) {
    debugger;
    
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SNohiddenfiled").val();
    var ddlId = "#wh_id" + Index;

    var whERRID = "#wh_Error" + Index;
    if (clickedrow.find(ddlId).val() == "0") {
        debugger;
        clickedrow.find(whERRID).text($("#valueReq").text());
        clickedrow.find(whERRID).css("display", "block");
        clickedrow.find(ddlId).css("border-color", "red");
    }
    else {

        var WHName = $("#wh_id" + Index + " option:selected").text();
        clickedrow.find("#hdWHName").val(WHName)
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
    }

    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var ItemId = clickedrow.find("#hdItemId").val();
    var WarehouseId = clickedrow.find(ddlId).val();
    var sub_item = clickedrow.find("#sub_item").val();
    var OrdQty = parseFloat($("#OrderQuantity").val()).toFixed(QtyDecDigit);
    var DispatchQty = parseFloat($("#DispatchQuantity").val()).toFixed(QtyDecDigit);
    var JONo = $("#Hdn_JobNum").val();
    var JODate = $('#Job_ord_date').val();
    var JobOrdTyp = $("#hdnJobOrdTyp").val();
    //var CompId = $("#HdCompId").val();
    //var BranchId = $("#HdBranchId").val();
    debugger;
    if (WarehouseId != "0") {
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineBatchItemId").val();
            if (rowitem == ItemId) {
                debugger;
                $(this).remove();
            }
        });

        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/MaterialDispatchSC/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
                JONo: JONo,
                JODate: JODate,
                OrdQty: OrdQty,
                DispatchQty: DispatchQty
            },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        var QtyDecDigit = $("#QtyDigit").text();///Quantity
                        debugger;
                        var avaiableStock = arr.Table[0].wh_avl_stk_bs;
                        var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                        clickedrow.find("#AvailableStock").val(parseavaiableStock);
                    }
                    if (JobOrdTyp == "Direct") {
                        debugger;
                        if (sub_item == "Y") {
                            if (arr.Table1.length > 0) {
                                var rowIdx = 0;
                                for (var y = 0; y < arr.Table1.length; y++) {
                                    var ItmId = arr.Table1[y].item_id;
                                    var SubItmId = arr.Table1[y].sub_item_id;
                                    var SubItmName = arr.Table1[y].sub_item_name;
                                    var SubItmQty = arr.Table1[y].Qty;
                                    var AvailblQty = arr.Table1[y].avl_stk;
                                    var OrdQty = arr.Table1[y].OrdQty;
                                    var PendQty = arr.Table1[y].PendQty;
                                    var SubItemTyp = "SubItm";

                                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemQty" value='${SubItmQty}'></td>
                                    <td><input type="text" id="subItemAvlQty" value='${AvailblQty}'></td>
                                    <td><input type="text" id="subItemType" value='${SubItemTyp}'></td>
                                    <td><input type="text" id="subItemOrdQty" value='${OrdQty}'></td>
                                    <td><input type="text" id="subItemPendQty" value='${PendQty}'></td>
                                    
                                    </tr>`);
                                    
                                }
                                /*<td><input type="text" id="subItemName" value='${SubItmName}'></td>*/
                            }
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    }
}

function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();

        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "";
        }
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#Command").val();
        var TransType = $("#TransType").val();
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialDispatchSC/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockBatchWise').html(data);

                    },
                });
        }
        else {
            debugger;
            var MD_Status = $("#hfStatus").val();
            if (MD_Status == "" || MD_Status == null || MD_Status == "D") {
                BindItemBatchDetail();
                debugger;
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                var ddlId = "#wh_id" + Index;

                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var HdnMessage = $("#Hdn_Message").val();


                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialDispatchSC/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemdetail: SelectedItemdetail,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            MD_Status: MD_Status,
                            HdnMessage: HdnMessage,
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            var Index = clickedrow.find("#SNohiddenfiled").val();
                            var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                            var ddlId = "#wh_id" + Index;
                            //var ItemId = clickedrow.find("#hdItemId").val();;
                            var WarehouseId = clickedrow.find(ddlId).val();
                            var CompId = $("#HdCompId").val();
                            var BranchId = $("#HdBranchId").val();

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            // $("#MI_TotalIssuedQuantity").val("");
                            $("#BatchwiseTotalIssuedQuantity").val("");

                            //Added by Hina on 10-05-2024
                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", MI_pedQty, "AvailableQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }

            else {
                debugger;
                var Mrd_No = $("#IssueNumber").val();
                var Mrd_Date = $("#IssueDate").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialDispatchSC/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            //IssueType: Mrs_IssueType,
                            IssueNo: Mrd_No,
                            IssueDate: Mrd_Date,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");
                        },
                    });
            }
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var QtyDecDigit = $("#QtyDigit").text();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#Command").val();
        var TransType = $("#TransType").val();
        var Mrd_Status = $("#hfStatus").val();

        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialDispatchSC/getItemstockSerialWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemSerial: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockSerialWise').html(data);
                    },
                });

        }
        else {

            var Mrd_Status = $("#hfStatus").val();
            if (Mrd_Status == "" || Mrd_Status == null || Mrd_Status == "D") {
                BindItemSerialDetail();
                //BindMtDispatchItemSerialDetail();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var ddlId = "#wh_id" + Index;

                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var SelectedItemSerial = $("#HDSelectedSerialwise").val();



                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialDispatchSC/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemSerial: SelectedItemSerial,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            Mrd_Status: Mrd_Status
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            //$("#TotalIssuedSerial").text("");
                        },
                    });
            }
            if (Mrd_Status == "C") {
                /* else {*/

                var Mrd_No = $("#IssueNumber").val();
                var Mrd_Date = $("#IssueDate").val();


                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialDispatchSC/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            //IssueType: Mrs_IssueType,
                            IssueNo: Mrd_No,
                            IssueDate: Mrd_Date,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#mi_lineSerialItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#mi_lineSerialIssueQty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }
            //else {

            //    var Mrd_No = $("#IssueNumber").val();
            //    var Mrd_Date = $("#IssueDate").val();


            //    $.ajax(
            //        {
            //            type: "Post",
            //            url: "/ApplicationLayer/MaterialDispatchSC/getItemstockSerialWiseAfterStockUpadte",
            //            data: {
            //                //IssueType: Mrs_IssueType,
            //                IssueNo: Mrd_No,
            //                IssueDate: Mrd_Date,
            //                ItemID: ItemId,
            //                DMenuId: DMenuId,
            //                Command: _mdlCommand,
            //                TransType: TransType
            //            },
            //            success: function (data) {
            //                debugger;
            //                $('#ItemStockSerialWise').html(data);
            //                $("#ItemNameSerialWise").val(ItemName);
            //                $("#UOMSerialWise").val(UOMName);
            //                $("#QuantitySerialWise").val(MI_pedQty);
            //                $("#HDItemIDSerialWise").val(ItemId);
            //                $("#HDUOMIDSerialWise").val(UOMID);
            //                var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

            //                if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
            //                    $("#SaveItemSerialTbl TBODY TR").each(function () {
            //                        var row = $(this)
            //                        var HdnItemId = row.find("#mi_lineSerialItemId").val();
            //                        if (ItemId === HdnItemId) {
            //                            TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#mi_lineSerialIssueQty").val());
            //                        }
            //                    });
            //                }
            //                $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
            //            },
            //        });
            //}

        }

    } catch (err) {
        console.log("Material Issue Error : " + err.message);
    }
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "0";
        }
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyisGreaterthanAvailableQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }

        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });

        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}

function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableStock").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "BtnBatchDetails", "Y");
            IsuueFlag = false;
        }
    });

    if (IsuueFlag) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#mi_lineBatchItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    //var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="mi_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="mi_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="mi_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="mi_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="mi_lineBatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="mi_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="mi_lineBatchavl_batch_qty" value="${AvailableQty}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
            }
        });
    }
}
function onchangeChkItemSerialWise() {
    debugger;
    var TotalIssueLot = 0;
    var QtyDigit = $("#QtyDigit").text();
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDigit));
    // localStorage.setItem('BatchResetFlag', 'True');
}
function onclickbtnItemSerialReset() {
    debugger;
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineSerialItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="mi_lineSerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="mi_lineSerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="mi_lineSerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="mi_lineSerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="mi_lineSerialSerialNO" value="${ItemSerialNO}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
            }
        });
    }
}
function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}

function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();
        debugger;
        var RequiredQuantity = clickedrow.find("#RequiredQuantity").val();
        if (parseFloat(IssueQuantity) < parseFloat(RequiredQuantity)) {
            debugger;
            clickedrow.find("#IssuedQuantity_Error").text($("#CanNotBeLessThanRequiredQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");

            ErrorFlag = "Y";
        }
        else {
            clickedrow.find("#IssuedQuantity_Error").css("display", "none");
            clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
            var mi_issueqty = parseFloat(parseFloat(IssueQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#IssuedQuantity").val(mi_issueqty);
        }
        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#mi_lineBatchIssueQty').val();
                    var bchitemid = currentRow.find('#mi_lineBatchItemId').val();
                    var bchuomid = currentRow.find('#mi_lineBatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
                }
                else {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#mi_lineSerialIssueQty').val();
                    var srialitemid = currentRow.find('#mi_lineSerialItemId').val();
                    var srialuomid = currentRow.find('#mi_lineSerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
                }
                else {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeIssueQuantity(el, evt) {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();

    if ((IssuedQuantity == "") || IssuedQuantity == "0" || IssuedQuantity == parseFloat(0) || (IssuedQuantity == null)) {
        clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val("");
        ErrorFlag = "Y";
        //HideErrorMsg();

        //return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }
    //var PendingQuantity = clickedrow.find("#PendingQuantity").val();
    var AvailableQuantity = clickedrow.find("#AvailableStock").val();

    if (IssuedQuantity > 0) {
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            debugger;
            clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyisGreaterthanAvailableQty").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            //HideErrorMsg();
            //return false;
        }
        else {
            clickedrow.find("#IssuedQuantity_Error").css("display", "none");
            clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
            var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#IssuedQuantity").val(mi_issueqty);
        }

    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}
function OnChangeValue(el, evt) {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    var MDValue = clickedrow.find("#MDValue").val();
    
    if ((MDValue == "") || MDValue == "0" || MDValue == parseFloat(0) || (MDValue == null)) {
        //clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
        //clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        //clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#MDValue").val("0.000");
        //ErrorFlag = "Y";
        
    }
    else {
        //clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
    var md_value = parseFloat(parseFloat(MDValue)).toFixed(parseFloat(QtyDecDigit));
    clickedrow.find("#MDValue").val(md_value);
    }
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}

}
function SerialNoAfterDelete() {
    debugger
    var SerialNo = 0;
    $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);


    });
};
function BindMtDispatchItemSerialDetail() {
    var serialrowcount = $('#SaveItemSerialTbl tbody tr').length;
    if (serialrowcount > 0) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.ItemId = row.find("#shipmentlineSerialItemId").val();
            SerialList.UOMId = row.find("#shipmentlineSerialUOMId").val();
            SerialList.LOTId = row.find("#shipmentlineSerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#shipmentlineSerialIssueQty").val();
            SerialList.SerialNO = row.find("#shipmentlineBatchSerialNO").val();
            ItemSerialList.push(SerialList);
            debugger;
        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);
    }
}
/*-------------Insert Section Start-------------------------*/

function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    var rowcount = $('#MaterialDispatchItemDetailsTbl tr').length;
    var ValidationFlag = true;

    var flag = MDHeaderValidations();
    if (flag == false) {
        ValidationFlag = false;
        return false;

    }

    //HideErrorMsg();

    if (ValidationFlag == true) {
        debugger;
        if (rowcount > 1) {

            var flag = MDItemValidations();
            if (flag == false) {
                return false;
            }
            /*---Start Sub Item Validation */
            var JOTyp = $("#hdnJobOrdTyp").val();
            if (JOTyp == "Direct") {
                var Subflag = MDSubItemValidation()
                if (Subflag == false) {
                    swal("", $("#SubItemQuantityMismatchWithAvailableStock").text(), "warning");
                    return false;
                }
            }
            else {
                var Subflag = CheckValidations_forSubItems()
                if (Subflag == false) {
                    return false;
                }
            }
            
            /*---Start Sub Item Validation */

            var Batchflag = true, SerialFlag = true;
            Batchflag = CheckItemBatchValidation();
            if (Batchflag == false) {
                return false;
            }
            SerialFlag = CheckItemSerialValidation();
            if (SerialFlag == false) {
                return false;
            }



            if (flag == true && Subflag == true && Batchflag == true && SerialFlag == true) {

                /* if (navigator.onLine === true)*//*Checing For Internet is open or not*//* {*/
                debugger;
                var MDNum = $("#IssueNumber").val();
                if (MDNum == null || MDNum == "") {
                    $("#hdtranstype").val("Save");
                }
                else {
                    $("#hdtranstype").val("Update");
                }
                var FinalMDItemDetail = [];
                FinalMDItemDetail = InsertMDItemDetails();

                /*-----------Sub-item-------------*/
                debugger;
                var JOTyp = $("#hdnJobOrdTyp").val();
                if (JOTyp == "Direct") {
                    var SubItemsListArr = InsertMDSubItemList_ForDirectJO();
                    var str2 = JSON.stringify(SubItemsListArr);
                    $('#SubItemDetailsDt').val(str2);
                      }
                else {
                    var SubItemsListArr = Cmn_SubItemList();
                    var str2 = JSON.stringify(SubItemsListArr);
                    $('#SubItemDetailsDt').val(str2);
                }

                /*-----------Sub-item end-------------*/

                $("#hdItemDetailList").val(JSON.stringify(FinalMDItemDetail));
                debugger;
                BindItemBatchDetail();
                BindItemSerialDetail();
                $("#Hdn_SupplierName").val($("#SupplierName").val());
                var MDNo = $("#JobOrd_Num").val();
                $("#JobOrd_Num").val(MDNo);
                $("#Hdn_Prodctid").val($("#Hdn_Prodctid").val());

                var SuppName = $("#SupplierName option:selected").text();
                $("#Hdn_MDSupplierName").val(SuppName);

                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                return true;
            }
            else {
                return false;
            }
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
function MDHeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    var suppname = $("#SupplierName").val();

    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        if ($("#Address").val() === "" || $("#Address").val() === null) {
            $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
            $("#Address").css("border-color", "Red");
            $("#SpanSuppAddrErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {

            $("#SpanSuppAddrErrorMsg").css("display", "none");
            $("#Address").css("border-color", "#ced4da");
        }
    }
    debugger;
    if (suppname != "0") {
        var jobOrdNo = $("#Job_Ord_number").val();
        var jobOrd_No = $("#Job_Ord_number option:selected").text();
        //$("#Job_Ord_number").val() === "---Select----0---0--0-0"
        if ($("#Job_Ord_number").val() === "0" || $("#Job_Ord_number").text() === "---Select---"
            || $("#Job_Ord_number").val() === "---Select---,0" || $("#Job_Ord_number").val() === "---Select---,0,0,0") {
            $("#SpanJobOrdNoErrorMsg").text($("#valueReq").text());
            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "red")
            $("#SpanJobOrdNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanJobOrdNoErrorMsg").css("display", "none");
            $("#Job_Ord_number").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-Job_Ord_number-container']").css("border-color", "#ced4da");
        }
    }
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var PendingQty = parseFloat(0).toFixed(QtyDecDigit);
    var DispatchQty = parseFloat(0).toFixed(QtyDecDigit);

    PendingQty = parseFloat($("#PendingQuantity").val()).toFixed(QtyDecDigit);
    DispatchQty = parseFloat($("#DispatchQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(DispatchQty) == false) {
        $("#DispatchQuantity").val("");
        DispatchQty = 0;
    }
    debugger;
    var jobord = $("#Job_Ord_number").val();
    if (suppname != "0") {
        //if (jobord !== "---Select----0---0--0-0")
        if (jobord !== "---Select---,0,0,0") {
            //var DisptchQty = $("#DispatchQuantity").val();
            if (DispatchQty == '' || DispatchQty == 'NaN') {
                $("#SpanDispatchQuantityErrorMsg").text($("#valueReq").text());
                $("#SpanDispatchQuantityErrorMsg").css("display", "block");
                $("#DispatchQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                if (DispatchQty != '' && (parseFloat(CheckNullNumber(DispatchQty)) < parseFloat(CheckNullNumber(PendingQty)) || parseFloat(CheckNullNumber(DispatchQty)) == parseFloat(CheckNullNumber(PendingQty)))) {
                    $("#SpanDispatchQuantityErrorMsg").css("display", "none");
                    $("#DispatchQuantity").css("border-color", "#ced4da");
                }
                else {
                    $("#SpanDispatchQuantityErrorMsg").text($("#ExceedingPendingQuantity").text());
                    $("#SpanDispatchQuantityErrorMsg").css("display", "block");
                    $("#DispatchQuantity").css("border-color", "red");
                    ErrorFlag = "Y";
                }
            }
        }
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function MDItemValidations() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    var RowIndex = 0;
    if ($("#Cancelled").is(":checked")) {
        ErrorFlag = "N";
    }
    else {

        $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            RowIndex++;
            var ddlId = "#wh_id" + RowIndex;
            var whERRID = "#wh_Error" + RowIndex;

            var IssueQuantity = currentRow.find("#IssuedQuantity").val();

            var AvaialbleQuantity = currentRow.find("#AvailableStock").val();
            var RequiredQuantity = currentRow.find("#RequiredQuantity").val();

            if (currentRow.find(ddlId).val() == "" || parseFloat(currentRow.find(ddlId).val()) == parseFloat("0")) {
                currentRow.find(whERRID).text($("#valueReq").text());
                currentRow.find(whERRID).css("display", "block");
                currentRow.find(ddlId).css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find(whERRID).css("display", "none");
                currentRow.find(ddlId).css("border-color", "#ced4da");
            }
            if (parseFloat(IssueQuantity) != parseFloat("0") || parseFloat(IssueQuantity) != "") {
                if (parseFloat(IssueQuantity) > parseFloat(AvaialbleQuantity)) {
                    currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
                    currentRow.find("#IssuedQuantity_Error").css("display", "block");
                    currentRow.find("#IssuedQuantity").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    //if (parseFloat(IssueQuantity) < parseFloat(RequiredQuantity)) {
                    //    debugger;
                    //    currentRow.find("#IssuedQuantity_Error").text($("#CanNotBeLessThanRequiredQuantity").text());
                    //    currentRow.find("#IssuedQuantity_Error").css("display", "block");
                    //    currentRow.find("#IssuedQuantity").css("border-color", "red");

                    //    ErrorFlag = "Y";
                    //}
                    //else {
                    currentRow.find("#IssuedQuantity_Error").css("display", "none");
                    currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    var mi_issueqty = parseFloat(parseFloat(IssueQuantity)).toFixed(parseFloat(QtyDecDigit));
                    currentRow.find("#IssuedQuantity").val(mi_issueqty);
                    /*}*/

                }

            }
            else {
                currentRow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
                currentRow.find("#IssuedQuantity_Error").css("display", "block");
                currentRow.find("#IssuedQuantity").css("border-color", "red");
                ErrorFlag = "Y";

            }

        })
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertMDItemDetails() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var MDItemList = [];
    $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;


        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UOMName = "";
        var RequiredQty = "";
        var WhID = "";
        var AvlStock = "";
        var IssuedQty = "";
        var AvlStock = parseFloat(0).toFixed(QtyDecDigit);
        var IssuedQty = parseFloat(0).toFixed(QtyDecDigit);
        var Value = "";
        var i_batch = "";
        var i_serial = "";
        var Remarks = "";

        var currentRow = $(this);
        var Index = currentRow.find("#SNohiddenfiled").val();
        var whERRID = "#wh_id" + Index + "  option:selected";

        ItemID = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ItemName").val();
        subitem = currentRow.find("#sub_item").val();
        UOMID = currentRow.find("#hdUOMId").val();
        UOMName = currentRow.find("#UOM").val();
        RequiredQty = currentRow.find("#RequiredQuantity").val();
        WhID = CheckNullNumber(currentRow.find(whERRID).val());
        debugger;
        AvlStock = CheckNullNumber(currentRow.find('#AvailableStock').val());
        IssuedQty = currentRow.find('#IssuedQuantity').val();
        Value = currentRow.find('#MDValue').val();
        i_batch = currentRow.find("#hdi_batch").val();
        i_serial = currentRow.find("#hdi_serial").val();
        Remarks = currentRow.find("#remarks").val();
        //WhID = currentRow.find("#wh_id").val();
        //AvlStock = currentRow.find("#AvailableStock").val();
        //IssuedQty = currentRow.find("#IssuedQuantity").val();

        MDItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, RequiredQty: RequiredQty, WhID: WhID, AvlStock: AvlStock, IssuedQty: IssuedQty, Value: Value, i_batch: i_batch, i_serial: i_serial, Remarks: Remarks });
    });

    return MDItemList;
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

/*-------------Insert Section End-------------------------*/
/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var MDStatus = "";
    //MDStatus = $('#hfStatus').val().trim();
    //if (MDStatus === "D" || MDStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
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

    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var dispDt = $("#IssueDate").val();
        $.ajax({
            type: "POST",
            /*   url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: dispDt
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var MDStatus = "";
                    MDStatus = $('#hfStatus').val().trim();
                    if (MDStatus === "D" || MDStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
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
                    /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
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
    var MDNo = "";
    var MDDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";

    WF_Status = $("#WF_Status1").val();
    docid = $("#DocumentMenuId").val();
    MDNo = $("#IssueNumber").val();
    MDDate = $("#IssueDate").val();
    $("#hdDoc_No").val(MDNo);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    //if ($("#OrderTypeD").is(":checked")) {
    //    InvType = "D";
    //}
    //if ($("#OrderTypeE").is(":checked")) {
    //    InvType = "E";
    //}
    //SaleVouMsg = $("#SaleVoucherPassAgainstInv").text()


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && MDNo != "" && MDDate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(MDNo, MDDate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/MaterialDispatchSC/ToRefreshByJS";
            var list = [{ MDNo: MDNo, MDDate: MDDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/MaterialDispatchSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ MDNo: MDNo, MDDate: MDDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/MaterialDispatchSC/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/MaterialDispatchSC/SIListApprove?SI_No=" + MDNo + "&SI_Date=" + MDDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && MDNo != "" && MDDate != "") {
             Cmn_InsertDocument_ForwardedDetail(MDNo, MDDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ MDNo: MDNo, MDDate: MDDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/MaterialDispatchSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && MDNo != "" && MDDate != "") {
             Cmn_InsertDocument_ForwardedDetail(MDNo, MDDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/MaterialDispatchSC/ToRefreshByJS";
            var list = [{ MDNo: MDNo, MDDate: MDDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/MaterialDispatchSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#IssueNumber").val();
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

/*-----------End Workflow--------------*/
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "MDSubItemOrdQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "MDSubItemPendQty",);
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "MDSubItemDispQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var SNo = clickdRow.find("#SNohiddenfiled").val();
    var JOTyp = $("#hdnJobOrdTyp").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var hd_command = $("#Command").val();
    $("#HdnFlag").val(flag);
    var HdnFlag = $("#HdnFlag").val();
    var Wh_id = "";
    if (flag == "MDReqQty" || flag == "MDIssueQty") {
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        var ProductNm = clickdRow.find("#ItemName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();
        var OrdQty = parseFloat($("#OrderQuantity").val()).toFixed(QtyDecDigit);
        var DispatchQty = parseFloat($("#DispatchQuantity").val()).toFixed(QtyDecDigit);
        var SubitmtypFlg = "SubItm";
        //if (flag == "MDIssueQty") {

        //    var FlagIssue = "MDIssueQty";
        //}

    }
    else {
        var ProductNm = $("#ProductName").val();
        var ProductId = $("#Hdn_Prodctid").val();
        var UOM = $("#UOMName").val();
        var SubitmtypFlg = "SubHedr";
    }


    var Sub_Quantity = 0;
    var IsDisabled = "";
    var NewArr = new Array();
    if (flag == "MDDispQty" || flag == "MDIssueQty") {
        if (flag == "MDDispQty") {
            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr').find("#subItemType[value='" + SubitmtypFlg + "']").closest('tr');
            rows.each(function () {
                //$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
                var row = $(this);
                var List = {};
                List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.qty = row.find('#subItemQty').val();
                List.SubItmType = row.find('#subItemType').val();
                if (flag == "MDIssueQty") {
                    //List.WhId = row.find('#subItemWhId').val();
                    List.avl_stk = row.find('#subItemAvlQty').val();
                    List.OrderQty = "";
                    List.PendQty = "";
                }
                else {
                    List.avl_stk = "";
                    List.OrderQty = row.find('#subItemOrdQty').val();
                    List.PendQty = row.find('#subItemPendQty').val();
                }


                //List.WhId = row.find('#subItemWhId').val();


                NewArr.push(List);
            });
        }
        if (flag == "MDIssueQty") {
            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr').find("#subItemType[value='" + SubitmtypFlg + "']").closest('tr');
            rows.each(function () {
                //$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
                var row = $(this);
                var List = {};
                List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.qty = row.find('#subItemQty').val();
                List.SubItmType = row.find('#subItemType').val();
                if (flag == "MDIssueQty") {
                    //List.WhId = row.find('#subItemWhId').val();
                    List.avl_stk = row.find('#subItemAvlQty').val();
                    List.OrderQty = "";
                    List.PendQty = "";
                }
                else {
                    List.avl_stk = "";
                    List.OrderQty = row.find('#subItemOrdQty').val();
                    List.PendQty = row.find('#subItemPendQty').val();
                }


                //List.WhId = row.find('#subItemWhId').val();


                NewArr.push(List);
            });
        }

        var len = $('#MaterialDispatchItemDetailsTbl tbody tr').length;
        if (flag == "MDDispQty") {
            Sub_Quantity = $("#DispatchQuantity").val();
            if (len > 0) {
                var IsDisabled = "Y";
            }
            else {
                var IsDisabled = $("#DisableSubItem").val();
            }
        }
        else {
            Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
            Wh_id = clickdRow.find("#wh_id" + SNo).val();
            if (hd_Status == "" || (hd_Status == "D" && hd_command == "Edit")) {
                var HdnMessage = $("#Hdn_Message").val();
                if (HdnMessage == "DocModify") {
                    IsDisabled = "Y";
                }
                else {
                    if (JOTyp == "Direct") {
                        IsDisabled = "Y";
                    }
                    else {
                        IsDisabled = "N";
                    }
                }

            }
            else {
                IsDisabled = "Y";
            }

        }


    }
    else if (flag == "MDOrdQty") {
        Sub_Quantity = $("#OrderQuantity").val();
        IsDisabled = "Y";
    }
    else if (flag == "MDPendQty") {
        Sub_Quantity = $("#PendingQuantity").val();
        IsDisabled = "Y";
    }
    else if (flag == "MDReqQty") {
        Sub_Quantity = clickdRow.find("#RequiredQuantity").val();
        IsDisabled = "Y";
    }
    else {
        Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
        Wh_id = clickdRow.find("#wh_id" + SNo).val();
        //IsDisabled = "N";
        var HdnMessage = $("#Hdn_Message").val();
        if (HdnMessage == "DocModify") {
            IsDisabled = "Y";
        }
        else {
            if (JOTyp == "Direct") {
                IsDisabled = "Y";
            }
            else {
                IsDisabled = "N";
            }
        }
    }


    if (hd_Status != "") {
        var JobOrdNo = $("#Job_Ord_number option:selected").text();
    }
    else {
        var JobOrdNo = $("#select2-Job_Ord_number-container").text();
        var JobOrdNo = $("#Hdn_JobNum").val()
    }
    var DocNo = $("#IssueNumber").val();
    var DocDt = $("#IssueDate").val();

    //var JobOrdNo = $("#JobOrderNumber").val(); 
    var JobOrdDt = $("#Job_ord_date").val();
    //var JobOrdDt = JobOrddate.split("-").reverse().join("-");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialDispatchSC/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            JobOrdNo: JobOrdNo,
            JobOrdDt: JobOrdDt,
            OrdQty: OrdQty,
            DispatchQty: DispatchQty,
            Wh_id: Wh_id,
            DocNo: DocNo,
            DocDt: DocDt,
            JOTyp: JOTyp

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

}
function CheckValidations_forHeaderSubItems()
{
    debugger
    var JOTyp = $("#hdnJobOrdTyp").val();
    if (JOTyp == "Direct")
    {
        return MDHeaderSubItemValidation("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "Y", "SubHedr");
    }
    else
    {
        return Cmn_CheckValidations_forSubItemsNonTable("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "Y");
    }
}
function CheckValidations_forSubItems() {
    debugger
    var JOTyp = $("#hdnJobOrdTyp").val();
    if (JOTyp == "Direct") {
        return MDMtrlSubItemValidation("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "Y", "SubItm");
     }
    else {
        return Cmn_CheckValidations_forSubItems("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "Y");

    }
    //var ErrFlg = "";
    //if (Cmn_CheckValidations_forSubItemsNonTable("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "Y") == false) {
    //    ErrFlg = "Y"
    //}
    //if (Cmn_CheckValidations_forSubItems("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "MDIssueQty", "Y") == false) {
    //    ErrFlg = "Y"
    //}
    //if (ErrFlg == "Y") {
    //    return false
    //}
    //else {
    //    return true
    //}

}
function ResetWorningBorderColor() {
    debugger; 
    var SubItmFlag = $("#HdnFlag").val(); 
    var JOTyp = $("#hdnJobOrdTyp").val();
    //return Cmn_CheckValidations_forSubItemsNonTable("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "N");
    var ErrFlg = "";
    if ((SubItmFlag == "MDDispQty" && JOTyp == "Direct") || (SubItmFlag == "MDIssueQty" && JOTyp == "Direct")) {
        if (SubItmFlag == "MDDispQty" && JOTyp == "Direct") {
            if (MDHeaderSubItemValidation("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "N", "SubHedr") == false);
            ErrFlg = "Y"
        }
        else {
            if (MDMtrlSubItemValidation("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "N", "SubItm") == false);
            ErrFlg = "Y"
        }
    }
    else {
        if (Cmn_CheckValidations_forSubItemsNonTable("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "N") == false) {
            ErrFlg = "Y"
        }
        if (Cmn_CheckValidations_forSubItems("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "N") == false) {
            ErrFlg = "Y"
        }
    }
    
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}

//----------------Sub Item Detail Only for Direct Case----------------------------
function fn_CustmMDSubItemDataAgainstDirectJO(itemId, SubItmType) {
    debugger;

    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        debugger;
        var Crow = $(this).closest("tr");
        //var ItemId = Crow.find("#ItemId").val();
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        //var subItemTyp = Crow.find("#subItemType").val(SubItmType);
        
        if (SubItmType == "SubItm") {
            
            var subItemAvlQty = Crow.find("#subItemAvlStk").val();
            var subItemOrdQty = "";
            var subItemPendQty = "";
        }
        else {
            var subItemAvlQty = "";
            var subItemPendQty = Crow.find("#subItemPendQty").val();
            var subItemOrdQty = Crow.find("#subItemOrdQty").val();
        }
        
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr').length;
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').find("#subItemType[value='" + SubItmType + "']").closest('tr');

        /*if (rows.len > 0) {*/
        if (rows.length > 0) {
            rows.each(function () {
                var InnerCrow = $(this).closest("tr");
                debugger;
                InnerCrow.find("#subItemQty").val(subItemQty);
            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemType" value='${SubItmType}'></td>
                            <td><input type="text" id="subItemOrdQty" value='${subItemOrdQty}'></td>
                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
                        </tr>`);

        }

    });

}
function CheckValidations_forSubItems2nd(Flag) {
    if (Flag == "MDDispQty") {
        $("#HdnFlag").val(Flag);
        return MDHeaderSubItemValidation("Hdn_Prodctid", "DispatchQuantity", "MDSubItemDispQty", "Y", "SubHedr");
    }
    else {
        return MDMtrlSubItemValidation("MaterialDispatchItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "MDSubItemIssueQty", "Y", "SubItm");
    }

}
function MDHeaderSubItemValidation(Item_field_id, Item_Qty_field_id, Button_id, ShowMessage, SubHedr) {
    debugger;
    var flag = "N";
    var item_id = $("#" + Item_field_id).val();
    var item_PrdQty = $("#" + Item_Qty_field_id).val();
    if (item_PrdQty == null || item_PrdQty == "") {
        item_PrdQty = $("#" + Item_Qty_field_id).text();
    }
    var sub_item = $("#sub_item").val();
    var Sub_Quantity = 0;
    var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubHedr + "']").closest('tr');
    rows.each(function () {
        /* $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {*/
        var Crow = $(this).closest("tr");
        //var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
    });
    if (sub_item == "Y") {
        if (item_PrdQty != Sub_Quantity) {
            flag = "Y";
            $("#" + Button_id).css("border", "1px solid red");
        } else {
            $("#" + Button_id).css("border", "");
        }
    }
    if (flag == "Y") {
        if (ShowMessage == "Y") {
            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }
        return false;

    }

    else {

        return true;

    }
}
function MDMtrlSubItemValidation(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage, SubItm) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    /*var docSrcTypid = $("#hdSrcTyp").val();*/

    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        }
        else {
            item_id = PPItemRow.find("#" + Item_field_id).val();
        }

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var sub_item = PPItemRow.find("#sub_item").val();
        //var sub_item = PPItemRow.find("#JoInsub_item").val();
        var Sub_Quantity = 0;
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').find("#subItemType[value='" + SubItm + "']").closest('tr');


        rows.each(function () {
            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });


        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
        }
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}
function InsertMDSubItemList_ForDirectJO() {
    var NewArr = new Array();
    debugger;
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stk = row.find('#subItemAvlQty').val();
            List.SubItmType = row.find('#subItemType').val();
            List.OrderQty = row.find('#subItemOrdQty').val();
            List.PendQty = row.find('#subItemPendQty').val();
           
            NewArr.push(List);
        }
    });
    return NewArr;
}

function MDSubItemValidation() {
    debugger;
    var ErrorFlag = "N";
   var subitmtyp = "SubItm"
     var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemType[value=" + subitmtyp + "]").closest('tr');
      rows.each(function () {
                    //$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
           var Crow = $(this);
           var subItem_ItemId = Crow.find("#ItemId").val();
           var subItemQty = Crow.find("#subItemQty").val();

           subItemAvlQty = Crow.find("#subItemAvlQty").val();
           if (subItemQty != null && subItemQty != "") {
                        if (subItemAvlQty != null && subItemAvlQty != "") {
                            if (parseFloat(CheckNullNumber(subItemAvlQty)) < parseFloat(CheckNullNumber(subItemQty))) {
                                $("#MaterialDispatchItemDetailsTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    var ItemID = currentRow.find("#hdItemId").val();
                                    if (ItemID == subItem_ItemId) {
                                        currentRow.find("#MDSubItemIssueQty").css("border", "1px solid red");
                                        ErrorFlag = "Y";
                                        return false;
                                    }
                                });
                            } 
                        }
                    }
      });
            
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
   




/***--------------------------------Sub Item Section End-----------------------------------------***/

/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() { /**Added this Condition by Nitesh 15-01-2024 for Disable Approve btn after one Click**/
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

/***---------------------Print Popup Start-------------------------------***/
function OnCheckedChangeProdDesc() {
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#MD_ShowCustSpecProdDesc').val('N');
        $("#MD_ShowProdDesc").val("Y");
    }
    else {
        $("#MD_ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    debugger;
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$("#MD_ShowProdDesc").val("N");
        $('#MD_ShowCustSpecProdDesc').val('Y');
    }
    else {
        $('#MD_ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#MD_ShowProdTechDesc').val('Y');
    }
    else {
        $('#MD_ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#MD_ShowSubItem').val('Y');
    }
    else {
        $('#MD_ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeI").is(":checked")) {
        $('#MD_PrintFormat').val('F2');
    }
    else {
        $('#MD_PrintFormat').val('F1');
    }
}
/***---------------------Print Popup End-------------------------------***/
/***---------------------Transporter Detail Section -------------------------------***/

function onchangeFreightAmount(el, e) {
    debugger;
    //if (AvoidChar(el, "RateDigit") == false) {
    //    return false;
    //}
    debugger;
    var QtyDecDigit = $("#RateDigit").text();
    FreightAmount = $("#txtMD_FreightAmount").val();
    $("#txtMD_FreightAmount").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));

}
function onchangVehicleLoadInTonnage(el, e) {
    debugger;
    //if (AvoidChar(el, "RateDigit") == false) {
    //    return false;
    //}
    debugger;
    var QtyDecDigit = $("#RateDigit").text();
    VehicleLoadInTonnage = $("#txtMD_VehicleLoadInTonnage").val();
    $("#txtMD_VehicleLoadInTonnage").val(parseFloat(CheckNullNumber(VehicleLoadInTonnage)).toFixed(QtyDecDigit));

}