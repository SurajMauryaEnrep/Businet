/************************************************
Javascript Name:Service Verification Detail
Created By:Mukesh
Created Date: 01-03-2023
Description: This Javascript use for the Service Verification many function

Modified By:
Modified Date: 
Description:

*************************************************/

$(document).ready(function () {
    BindSuppList();

    BindPOItmList(1);
    $("#TxtGrossValue").on('change', function () {
        
        var GstCat = $("#Hd_GstCat").val();
        var ToTdsAmt = 0;
        //if (GstCat == "UR") {
        //var ToTdsAmt_IT = parseFloat(CheckNullNumber($("#TxtGrossValue").val())) + parseFloat(CheckNullNumber($("#TxtDocSuppOtherCharges").val()))
        //    + parseFloat(CheckNullNumber($("#TxtTaxAmount").val()));
        var ToTdsAmt_IT = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));

        //} else {
        //    ToTdsAmt = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        //}
        ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
        ResetTDS_CalOnchangeDocDetail(ToTdsAmt, "#TxtTDS_Amount", ToTdsAmt_IT);
    });
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    $('#ServicePIItemTbl tbody').on('click', '.deleteIcon', function () {
        ////
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
        //
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";

        ItemCode = $(this).closest('tr').find("#ServiceName" + SNo).val();

        ////var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        // ShowItemListItm(ItemCode);
        CalculateAmount();
        //var TOCAmount = parseFloat($("#TxtOtherCharges").val());//Commented by Suraj Maurya on 06-01-2025
        var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetPI_ItemTaxDetail();
        BindOtherChargeDeatils();
        GetAllGLID();
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });

    SrVer_No = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(SrVer_No);
    BindDDLAccountList();
    //GetAllGLID();
    CancelledRemarks("#Cancelled", "Disabled");
});

function SerialNoAfterDelete() {
    
    var SerialNo = 0;
    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
        //
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};

function ShowSavedAttatchMentFiles(_LPO_No, _LPO_Date) {

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetPOAttatchDetailEdit",
        data: { PO_No: _LPO_No, PO_Date: _LPO_Date },
        success: function (data) {
            
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            } else {
                $("#PartialImageBind").html(data);
            }

        }
    });

}
function ApproveBtnClick() {
    InsertVerApproveDetails("", "", "");
}
function DeleteBtnClick() {
    //RemoveSession();
    Delete_PoDetails();
    ResetWF_Level();

}
function ForwardBtnClick() {
    
    //var OrderStatus = "";
    //OrderStatus = $('#hfStatus').val();
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

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var SPIDate = $("#Sinv_dt").val();
        $.ajax({
            type: "POST",
            /*     url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: SPIDate
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var OrderStatus = "";
                    OrderStatus = $('#hfStatus').val();
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
                    /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
// function OnClickForwardOK_Btn() {
    
//    var fwchkval = $("input[name='forward_action']:checked").val();
//    var DocNo = "";
//    var DocDate = "";
//    var docid = "";
//    var level = "";
//    var Remarks = "";
//    var forwardedto = "";
//    var VoucherNarr = $("#PurchaseVoucherRaisedAgainstInv").text();

//    Remarks = $("#fw_remarks").val();
//    DocNo = $("#InvoiceNumber").val();
//    DocDate = $("#Sinv_dt").val();
//    docid = $("#DocumentMenuId").val();
//    var ListFilterData1 = $('#ListFilterData1').val();
//    var WF_status1 = $("#WF_status1").val();
//    var TrancType = (DocNo + ',' + DocDate + ',' + "Update" + ',' + WF_status1)

//    $("#forwardDetailsTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        //
//        var Userid = currentRow.find("#user").text();
//        if ($("#r_" + Userid).is(":checked")) {
//            forwardedto = currentRow.find("#user").text();
//            level = currentRow.find("#level").text();
//        }
//    });

//    if (fwchkval === "Forward") {
//        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            
//            //var Action_name = "DPODetail,DPO";
//            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + DocNo + "&docdate=" + DocDate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
//            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks);
//            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
//        }
//    }
//    if (fwchkval === "Approve") {

//        var list = [{
//            DocNo: DocNo, DocDate: DocDate, VoucherNarr: VoucherNarr, A_Status: fwchkval, A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks
//            , Nurration: $("#hdn_Nurration").val(), BP_Nurration: $("#hdn_BP_Nurration").val()
//            , DN_Nurration: $("#hdn_DN_Nurration").val()
//        }];
//        var AppDtList = JSON.stringify(list);
//        window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 ;
//    }
//    if (fwchkval === "Reject") {
//        if (fwchkval != "" && DocNo != "" && DocDate != "") {
//             Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
//            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
//        }
//    }
//    if (fwchkval === "Revert") {
//        if (fwchkval != "" && DocNo != "" && DocDate != "") {
//            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
//            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
//        }
//    }
//}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";
    var VoucherNarr = $("#PurchaseVoucherRaisedAgainstInv").text();

    Remarks = $("#fw_remarks").val();
    DocNo = $("#InvoiceNumber").val();
    DocDate = $("#Sinv_dt").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $('#ListFilterData1').val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (DocNo + ',' + DocDate + ',' + "Update" + ',' + WF_status1)

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ServicePurchaseInvoice_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ServicePurchaseInvoice/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            //var Action_name = "DPODetail,DPO";
            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + DocNo + "&docdate=" + DocDate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {

        var list = [{
            DocNo: DocNo, DocDate: DocDate, VoucherNarr: VoucherNarr, A_Status: fwchkval, A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks
            , Nurration: $("#hdn_Nurration").val(), BP_Nurration: $("#hdn_BP_Nurration").val()
            , DN_Nurration: $("#hdn_DN_Nurration").val()
        }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
function BindSuppList() {
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
function OnChangeSuppName(SuppID) {
    
    var docid = $("#DocumentMenuId").val();
    var Supp_id = SuppID.value;
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#ServicePIItemTbl .plus_icon1").css("display", "none");
        $("#Address").val("");
        $("#ddlCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_SRPISuppName").val(Suppname);
        
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")

        //$("#ServicePIItemTbl >tbody >tr").each(function () {
        //    var currentRow = $(this);
        //    var Sno = currentRow.find("#SNohiddenfiled").val();
        //    currentRow.find("#ServiceName" + Sno).attr("disabled", false);
        //    currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
        //    currentRow.find("#BtnTxtCalculation").attr("disabled", true);
        //    currentRow.find("#ord_qty").attr("disabled", false);
        //    currentRow.find("#TxtRate").attr("disabled", false);
        //    currentRow.find("#remarks").attr("disabled", false);
        //    currentRow.find("#ServiceNameError").css("display", "none");
        //    currentRow.find("#ServiceName" + Sno).css("border-color", "#ced4da");//aria-labelledby="select2-ServiceName1-container"
        //    currentRow.find("select2-selection select2-selection--single").css("border", "1px solid #aaa");
        //    currentRow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border-color", "#ced4da");
        //    currentRow.find("#ord_qtyError").css("display", "none");
        //    currentRow.find("#ord_qty").css("border-color", "#ced4da");
        //    currentRow.find("#TxtRateError").css("display", "none");
        //    currentRow.find("#TxtRate").css("border-color", "#ced4da");
        //})

        var sourcetype ;
        if ($("#SourceTypeD").is(":checked")) {
            sourcetype = "D";
        }
        if ($("#SourceTypeV").is(":checked")) {
            sourcetype = "V";
        }
        $("#hdSrc_type").val(sourcetype);
        if (sourcetype == "D") {
            $("#DivSrcDocNo").css("display", "none");
            $("#DivSrcDocDt").css("display", "none");
            $("#DivPlusAddBtn").css("display", "none");
            $("#ServicePIItemTbl .plus_icon1").css("display", "block");
            $("#ServicePIItemTbl >tbody >tr").remove();
        }
        else {
            $("#DivSrcDocNo").css("display", "block");
            $("#DivSrcDocDt").css("display", "block");
            $("#DivPlusAddBtn").css("display", "block");
            $("#ServicePIItemTbl .plus_icon1").css("display", "none");
            $("#ServicePIItemTbl >tbody >tr").remove();
        }

        //$("#ServicePIItemTbl #delBtnIcon1").css("display", "block");
       // $("#ServicePIItemTbl .plus_icon1").css("display", "block");

    } //
    $("#Hdn_SupplierName").val(Supp_id);
    GetSuppAddress(Supp_id);
    $("#txtsrcdocdate").val("");
    BindDocumentNumberList(Supp_id);
    OnChangeBillNo();
}
function BindDocumentNumberList(Supp_id) {

    try {
        

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseInvoice/GetServiceVerifcationList",
            data: { Supp_id: Supp_id },
            success: function (data) {
                
                if (data == 'ErrorPage') {
                    PI_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        
                        $("#ddlSrcDocNo option").remove();
                        $("#ddlSrcDocNo optgroup").remove();
                        $('#ddlSrcDocNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].sr_ver_dt}" value="${arr.Table[i].sr_ver_no}">${arr.Table[i].sr_ver_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlSrcDocNo').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-7 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-5 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });

                        $("#ddlSrcDocDt").val("");
                    }
                }
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function GetSuppAddress(Supp_id) {
    
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseOrder/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    
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
                            $("#ddlCurrency").val(arr.Table[0].curr_name);
                            $("#Hdn_ddlCurrency").val(arr.Table[0].curr_id);
                            $("#supp_acc_id").val(arr.Table[0].supp_acc_id);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            $("#conv_rate").prop("readonly", true);
                            if (arr.Table[0].gst_cat == "UR") {
                                $('#RCMApplicable').prop("checked", true);
                                $("#RCMApplicable").prop("disabled", true);
                            }
                            else {
                                $('#RCMApplicable').prop("checked", false);
                                $("#RCMApplicable").prop("disabled", false);
                            }
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            if ($("#conv_rate").val() == "") {
                                $('#SpanSuppExRateErrorMsg').text($("#valueReq").text());
                                $("#conv_rate").css("border-color", "Red");
                                $("#SpanSuppExRateErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppExRateErrorMsg").css("display", "none");
                                $("#conv_rate").css("border-color", "#ced4da");
                            }
                            //CheckPOHraderValidations();
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
function CheckRCMApplicable() {
    
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    GetAllGLID();
}
function InsertSPIDetails() {
    
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }

        var DecDigit = $("#ValDigit").text();
        var INSDTransType = sessionStorage.getItem("INSTransType");
        if ($("#duplicateBillNo").val() == "Y") {
            $("#Bill_No").css("border-color", "Red");
            $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
            $("#SpanBillNoErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanBillNoErrorMsg").css("display", "none");
            $("#Bill_No").css("border-color", "#ced4da");
        }
        if (CheckSPI_Validations() == false) {
            return false;
        }
        if (CheckSPI_ItemValidations() == false) {
            return false;
        }
        if ($("#Cancelled").is(":checked")) {
            if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
                $('#SpanCancelledRemarks').text($("#valueReq").text());
                $("#SpanCancelledRemarks").css("display", "block");
                $("#Cancelledremarks").css("border-color", "Red");
                return false;
            }
            else {
                $('#SpanCancelledRemarks').text("");
                $("#SpanCancelledRemarks").css("display", "none");
                $("#Cancelledremarks").css("border-color", "#ced4da");
            }

        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("ServicePIItemTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "ServiceName") == false) {
                return false;
            }
        }
        if (CheckSPI_VoucherValidations() == false) {
            return false;
        }
        if ($("#Cancelled").is(":checked")) {
            Cancelled = "Y";
        }
        else {
            Cancelled = "N";
        }
        if ($("#SourceTypeD").is(":checked")) {
            sourcetype = "D";
        }
        if ($("#SourceTypeV").is(":checked")) {
            sourcetype = "V";
        }
        $("#hdSrc_type").val(sourcetype);
        
        var TransType = "";
        if (INSDTransType === 'Update') {
            TransType = 'Update';
        }
        else {
            TransType = 'Save';
        }
        
        $("#SupplierName").attr("disabled", false);

        var Narration = $("#DebitNoteRaisedAgainstInv").text()
        $('#hdNarration').val(Narration);

        var FinalItemDetail = [];
        FinalItemDetail = InsertSPIItemDetails();
        var str = JSON.stringify(FinalItemDetail);
        $('#hdItemDetailList').val(str);

        var TaxDetail = [];
        TaxDetail = InsertTaxDetails();
        var str_TaxDetail = JSON.stringify(TaxDetail);
        $('#hdItemTaxDetail').val(str_TaxDetail);

        var TdsDetail = [];
        TdsDetail = InsertTdsDetails();
        var str_TdsDetail = JSON.stringify(TdsDetail);
        $('#hdn_tds_details').val(str_TdsDetail);

        var Final_OC_TdsDetails = [];
        Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
        var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
        $('#hdn_oc_tds_details').val(Oc_Tds_Details);

        var OC_TaxDetail = [];
        OC_TaxDetail = OC_InsertTaxDetails();
        var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
        $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

        var OCDetail = [];
        OCDetail = GetSPI_OtherChargeDetails();
        var str_OCDetail = JSON.stringify(OCDetail);
        $('#hdItemOCDetail').val(str_OCDetail);

        var vou_Detail = [];
        vou_Detail = GetSPI_VoucherDetails();
        var str_vou_Detail = JSON.stringify(vou_Detail);
        $('#hdItemvouDetail').val(str_vou_Detail);

        var FinalCostCntrDetails = [];
        FinalCostCntrDetails = Cmn_InsertCCDetails();
        var CCDetails = JSON.stringify(FinalCostCntrDetails);
        $('#hdn_CC_DetailList').val(CCDetails);

        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfStatus") == false) {
            return false;
        }
        $("#RCMApplicable").prop("disabled", false);
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_SRPISuppName").val(Suppname);
        // $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");

        return true;
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    
}
function InsertSPIItemDetails() {
    var srctyp = $("#hdSrc_type").val();
    var PI_ItemsDetail = [];
    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
        
       
        var item_id = "";
        var item_name = "";
        var Hsn_no = "";
        var inv_qty = "";
        var item_rate = "";
        var item_gr_val = "";
        var item_tax_amt = "";
        var item_oc_amt = "";       
        var item_net_val_bs = "";
        var TaxExempted = "";
        var ManualGST = "";
        var ClaimITC = "";
        var item_acc_id = "";
        var ItmRemarks = "";

        var currentRow = $(this); 
        SNO = currentRow.find("#SNohiddenfiled").val();
        item_id = currentRow.find("#hfItemID").val();
        if (srctyp == "D") {
            item_name = currentRow.find("#ServiceName" + SNO + " option:selected").text();
        }
        else {
            item_name = currentRow.find("#ServiceName").val();
        }
       
        Hsn_no = currentRow.find("#HsnNo").val();
        inv_qty = currentRow.find("#TxtInvoiceQuantity").val();
        item_rate = currentRow.find("#TxtRate").val();
        item_gr_val = currentRow.find("#TxtItemGrossValue").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        ItmRemarks = currentRow.find("#SPIItemRemarks").val();/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }      
        item_net_val_bs = currentRow.find("#NetValueinBase").val();
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y"
            }
            else {
                ManualGST = "N"
            }
            if (currentRow.find("#ClaimITC").is(":checked")) {
                ClaimITC = "Y"
            }
            else {
                ClaimITC = "N"
            }
        }
        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, Hsn_no: Hsn_no, inv_qty: inv_qty, item_rate: item_rate
            , item_gr_val: item_gr_val, item_tax_amt: item_tax_amt, item_oc_amt: item_oc_amt, item_net_val_bs: item_net_val_bs
            , TaxExempted: TaxExempted, ManualGST: ManualGST, ClaimITC: ClaimITC, item_acc_id: item_acc_id, ItmRemarks: ItmRemarks
        });
    });
    return PI_ItemsDetail;
}
function InsertTaxDetails() {
    
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#ServicePIItemTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            if ($("#SourceTypeV").is(":checked")) {
                var ItmCode = currentRow.find("#hfItemID").val();
            }
            else {
                var ItmCode = currentRow.find("#ServiceName" + RowSNo).val();
            }
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var item_id = Crow.find("#TaxItmCode").text();
                        var tax_id = Crow.find("#TaxNameID").text();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var tax_rate = Crow.find("#TaxPercentage").text();
                        var tax_level = Crow.find("#TaxLevel").text();
                        var tax_val = Crow.find("#TaxAmount").text();
                        var tax_apply_on = Crow.find("#TaxApplyOnID").text();
                        var tax_recov = Crow.find("#TaxRecov").text();
                        var tax_acc_id = Crow.find("#TaxAccId").text();
                        var tax_apply_onName = Crow.find("#TaxApplyOn").text();
                        var totaltax_amt = Crow.find("#TotalTaxAmount").text();
                        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_recov: tax_recov, tax_acc_id: tax_acc_id, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
                    });
                }
            }
        }
    });
    return TaxDetails;
    //$("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
    //    
       
    //    var item_id = "";
    //    var tax_id = "";
    //    var TaxName = "";
    //    var tax_rate = "";
    //    var tax_level = "";
    //    var tax_val = "";
    //    var tax_apply_on = "";
    //    var tax_apply_onName = "";
    //    var totaltax_amt = "";
    //    var currentRow = $(this);
      
    //    item_id = currentRow.find("#TaxItmCode").text();
    //    tax_id = currentRow.find("#TaxNameID").text();
    //    TaxName = currentRow.find("#TaxName").text().trim();
    //    tax_rate = currentRow.find("#TaxPercentage").text();
    //    tax_level = currentRow.find("#TaxLevel").text();
    //    tax_val = currentRow.find("#TaxAmount").text();
    //    tax_apply_on = currentRow.find("#TaxApplyOnID").text();
    //    tax_apply_onName = currentRow.find("#TaxApplyOn").text();
    //    totaltax_amt = currentRow.find("#TotalTaxAmount").text();
       
    //    TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    //});
}
function InsertTdsDetails() {
    
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        
        var Tds_id = "";
        var Tds_rate = "";
        var Tds_level = "";
        var Tds_val = "";
        var Tds_apply_on = "";
        var Tds_name = "";
        var Tds_applyOnName = "";
        var Tds_totalAmnt = "";
        

        var currentRow = $(this);

        Tds_id = currentRow.find("#td_TDS_NameID").text();
        Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        Tds_level = currentRow.find("#td_TDS_Level").text();
        Tds_val = currentRow.find("#td_TDS_Amount").text();
        Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        Tds_name = currentRow.find("#td_TDS_Name").text();
        Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();

        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
}
function OC_InsertTaxDetails() {
    
    var TaxDetails = [];
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        
       
        var item_id = "";
        var tax_id = "";
        var TaxName = "";
        var tax_rate = "";
        var tax_level = "";
        var tax_val = "";
        var tax_apply_on = "";
        var tax_recov = "";
        var tax_acc_id = "";
        var tax_apply_onName = "";
        var totaltax_amt = "";
        var currentRow = $(this);
       
        item_id = currentRow.find("#TaxItmCode").text();
        tax_id = currentRow.find("#TaxNameID").text();
        TaxName = currentRow.find("#TaxName").text().trim();
        tax_rate = currentRow.find("#TaxPercentage").text();
        tax_level = currentRow.find("#TaxLevel").text();
        tax_val = currentRow.find("#TaxAmount").text();
        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        tax_recov = currentRow.find("#TaxRecov").text();
        tax_acc_id = currentRow.find("#TaxAccId").text();
        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
        totaltax_amt = currentRow.find("#TotalTaxAmount").text();
        TaxDetails.push({
            item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level
            , tax_val: tax_val, tax_apply_on: tax_apply_on, tax_recov: tax_recov, tax_acc_id: tax_acc_id
            , tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt
        });
    });
    return TaxDetails;
}
function GetSPI_OtherChargeDetails() {
    
    var PI_OCList = [];
    //if ($("#ht_Tbl_OC_Deatils >tbody >tr").length > 0) {
    //    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function () {  
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = "";
            var oc_val = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            oc_id = currentRow.find("#OCValue").text();
            oc_val = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            let curr_id = currentRow.find("#HdnOCCurrId").text();
            let supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text(); // Added by Suraj on 20-05-2024
            let bill_no = currentRow.find("#OCBillNo").text(); // Added by Suraj on 20-05-2024
            let bill_date = currentRow.find("#OCBillDt").text(); // Added by Suraj on 20-05-2024
            let tds_amt = currentRow.find("#OC_TDSAmt").text(); // Added by Suraj on 19-06-2024
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            PI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt
                , OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs
                , supp_type: supp_type, supp_id: supp_id, curr_id: curr_id, bill_no: bill_no, bill_date: bill_date
                , tds_amt: tds_amt, round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    return PI_OCList;
};
function GetSPI_VoucherDetails() {
    
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
   
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var Gltype = "";
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#txthfAccID").val();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            Gltype = currentRow.find("#type").val();
            var dr_amt_bs = currentRow.find("#dramtInBase").text();
            var cr_amt_bs = currentRow.find("#cramtInBase").text();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            PI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });

        });
    }
    return PI_VouList;
};
function CheckSPI_Validations() {
    
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#SupplierName").css("border-color", "#ced4da");
    }
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }

    if (ErrorFlag == "Y") {
        return false;
    } else {
        
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }




}
function CheckSPI_ItemValidations() {
    
    var RateDecDigit = $("#RateDigit").text();
    var ErrorFlag = "N";
    if ($("#ServicePIItemTbl >tbody >tr").length > 0) {
        $("#ServicePIItemTbl >tbody >tr").each(function () {
            
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#ServiceName" + Sno).val() == "0") {
                currentRow.find("#ServiceNameError").text($("#valueReq").text());
                currentRow.find("#ServiceNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ServiceNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#TxtInvoiceQuantity").val() == "" || parseFloat(currentRow.find("#TxtInvoiceQuantity").val()) == 0) {
                currentRow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
                currentRow.find("#TxtInvoiceQuantityError").css("display", "block");
                currentRow.find("#TxtInvoiceQuantity").css("border-color", "red");
                currentRow.find("#TxtInvoiceQuantity");//.focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtInvoiceQuantityError").css("display", "none");
                currentRow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
            }
            if (currentRow.find("#TxtRate").val() == "") {
                currentRow.find("#TxtRateError").text($("#valueReq").text());
                currentRow.find("#TxtRateError").css("display", "block");
                currentRow.find("#TxtRate").css("border-color", "red");
                if (currentRow.find("#TxtInvoiceQuantity").val() != "" && currentRow.find("#TxtInvoiceQuantity").val() != null) {
                    currentRow.find("#TxtRate");//.focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtRateError").css("display", "none");
                currentRow.find("#TxtRate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#TxtRate").val() != "") {
                if (parseFloat(currentRow.find("#TxtRate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#TxtRateError").text($("#valueReq").text());
                    currentRow.find("#TxtRateError").css("display", "block");
                    currentRow.find("#TxtRate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#TxtRateError").css("display", "none");
                    currentRow.find("#TxtRate").css("border-color", "#ced4da");
                }
            }
        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckSPI_ItemValidationsForGL() {
    
    var RateDecDigit = $("#RateDigit").text();
    var ErrorFlag = "N";
    if ($("#ServicePIItemTbl >tbody >tr").length > 0) {
        $("#ServicePIItemTbl >tbody >tr").each(function () {
            
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#ServiceName" + Sno).val() == "0") {
                currentRow.find("#ServiceNameError").text($("#valueReq").text());
                currentRow.find("#ServiceNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ServiceNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#TxtInvoiceQuantity").val() == "") {
               // currentRow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
               // currentRow.find("#TxtInvoiceQuantityError").css("display", "block");
                //currentRow.find("#TxtInvoiceQuantity").css("border-color", "red");
                currentRow.find("#TxtInvoiceQuantity");//.focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtInvoiceQuantityError").css("display", "none");
                currentRow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
            }
            if (currentRow.find("#TxtRate").val() == "") {
                //currentRow.find("#TxtRateError").text($("#valueReq").text());
                //currentRow.find("#TxtRateError").css("display", "block");
                //currentRow.find("#TxtRate").css("border-color", "red");
                if (currentRow.find("#TxtInvoiceQuantity").val() != "" && currentRow.find("#TxtInvoiceQuantity").val() != null) {
                    currentRow.find("#TxtRate");//.focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtRateError").css("display", "none");
                currentRow.find("#TxtRate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#TxtRate").val() != "") {
                if (parseFloat(currentRow.find("#TxtRate").val()).toFixed(RateDecDigit) == 0) {
                    //currentRow.find("#TxtRateError").text($("#valueReq").text());
                    //currentRow.find("#TxtRateError").css("display", "block");
                    //currentRow.find("#TxtRate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#TxtRateError").css("display", "none");
                    currentRow.find("#TxtRate").css("border-color", "#ced4da");
                }
            }
        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckSPI_VoucherValidations() {
    

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 06-08-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 06-08-2024 to add new common gl validations*/

    //var ErrorFlag = "N";
    //var ErrorFg = "N";
    //var ValDigit = $("#ValDigit").text();
    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {
    //    
    //    var currentRow = $(this);
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //    }
    //    else {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //        return false;
    //    }       
    //    if (currentRow.find("#hdnAccType").val() == "DDLValue") {
    //        if (AccID == null) {
    //            currentRow.find("#SpanAcc_name_"+ rowid).text($("#valueReq").text());
    //            currentRow.find("#SpanAcc_name_"+ rowid).css("display", "block");
    //            currentRow.find("[aria-labelledby='select2-Acc_name_" + rowid + "-container']").css("border-color", "red");
    //            $("#collapseThree").addClass("show");
    //            ErrorFg = "Y";
    //        }
    //    }       
    //});
    //if (ErrorFg == "Y") {
    //    return false;
    //}
    //
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}

    //
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}


    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
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
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}

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

function OnChangeSrcDocNo(Ver_No) {
    
    var VerNo = Ver_No.value;
    var VerDate = $('#ddlSrcDocNo').select2("data")[0].element.attributes[0].value;
    var VerDP = VerDate.split("-");
    $("#HDddlSrcDocNo").val(VerNo);
    var FVerDate = (VerDP[2] + "-" + VerDP[1] + "-" + VerDP[0]);
    if (VerNo == "---Select---") {
        $("#ddlSrcDocDt").val("");
        $('#SpanVerNoErrorMsg').text($("#valueReq").text());
        $("#SpanVerNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlSrcDocNo-container']").css("border-color", "red");
    }
    else {
        $("#ddlSrcDocDt").val(FVerDate);
        $("#SpanVerNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlSrcDocNo-container']").css("border-color", "#ced4da");
    }
}
function OnClickAddButton() {
    
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    $("#conv_rate").prop("readonly", true);
    var VerNo = $('#ddlSrcDocNo').val();
    var VerDate = $('#ddlSrcDocDt').val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (VerNo == "---Select---" || VerNo == "0") {
        $('#SpanVerNoErrorMsg').text($("#valueReq").text());
        $("#SpanVerNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlSrcDocNo-container']").css("border-color", "red");
        $("#ddlSrcDocNo").css("border-color", "red");
    }
    else {
        $("#SpanVerNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlSrcDocNo-container']").css("border-color", "#ced4da");
        $("#ddlSrcDocNo").css("border-color", "#ced4da");
        $("#SupplierName").attr("disabled", "disabled");
        $("#ddlSrcDocNo").attr("disabled", "disabled");
        $("#SourceTypeD").attr("disabled", "disabled");
        $("#SourceTypeV").attr("disabled", "disabled");
        //$(".plus_icon1").css("display", "none");
        $("#DivPlusAddBtn").css("display", "none");
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseInvoice/GetServiceVerifcationDetails",
                data: { VerNo: VerNo, VerDate: VerDate },
                success: function (data) {
                    
                    if (data == 'ErrorPage') {
                        PI_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var k = 0; k < arr.Table.length; k++) {
                                
                                var TotalGrossVal = ((parseFloat(arr.Table[k].grossval)) + (parseFloat(arr.Table[k].item_tax)) + (parseFloat(arr.Table[k].item_oc)));
                                var BaseVal;
                                BaseVal = (parseFloat(ConvRate).toFixed(ValDecDigit) * parseFloat(TotalGrossVal).toFixed(ValDecDigit));
                                var ManualGst = "";
                                var TaxExempted = "";
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (GstApplicable == "Y") {
                                    ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                    ManualGst += `<td class="qt_to">
                                                     <div class="custom-control custom-switch sample_issue">
                                                             <input type="checkbox" checked class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                                                         <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                                                     </div>
                                                 </td>`;
                                }
                                TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'


                                var S_NO = $('#ServicePIItemTbl tbody tr').length + 1;
                                $('#ServicePIItemTbl tbody').append(`<tr id="R${++rowIdx}">
                                                                        <td class=" red center"> <i class="" aria-hidden="true"></i></td>
                                                                     <td class="sr_padding" id="srno">${S_NO}</td>
                                                                    <td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${S_NO}" /></td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz">
                                                                        <div class="lpo_form">
                                                                            <input id="ServiceName" class="form-control" autocomplete="off"  type="text" name="ServiceName" value='${arr.Table[k].item_name}' placeholder='${$("#ItemName").text()}' disabled>
                                                                            <span id="ServiceNameError" class="error-message is-visible"></span>
                                                                            <input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                                                            <input class="" type="hidden" id="hdn_item_gl_acc" value="${arr.Table[k].loc_pur_coa}" />
                                                                        </div>
                                                                    </td>
                                                                        <td>
                                                                        <div class="lpo_form">
                                                                            <input id="HsnNo" class="form-control" autocomplete="off" type="text" name="HsnNo" value="${arr.Table[k].hsn_no}" placeholder="0000.00" disabled>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="TxtInvoiceQuantity" class="form-control num_right" autocomplete="off"   type="text" name="TxtInvoiceQuantity" value="${parseFloat(arr.Table[k].conf_qty).toFixed(ValDecDigit)}" placeholder="0000.00" disabled> <span id="TxtInvoiceQuantityError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>
                                                                        <td>
                                                                     <div class="lpo_form">
                                                                <input id="TxtRate" class="form-control num_right" autocomplete="" type="text" onchange ="OnChangePOItemRate(1,event)" name="Rate" placeholder="0000.00" value="${parseFloat(arr.Table[k].item_rate).toFixed(ValDecDigit)}" disabled><span id="TxtRateError" class="error-message is-visible"></span>
                                                                   </div>
                                                            </td>
                                                            <td>
                                                                <input id="TxtItemGrossValue" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="${parseFloat(arr.Table[k].grossval).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                            </td>
                                                              `+ TaxExempted+`
                                                              `+ ManualGst+`
                                                            <td>
                                                                <div class=" col-sm-10 num_right" style="padding:0px;">
                                                                    <input id="Txtitem_tax_amt" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(arr.Table[k].item_tax).toFixed(ValDecDigit)}" name="item_tax_amt" placeholder="0000.00" disabled>
                                                                </div>
                                                                <div class=" col-sm-2 num_right" style="padding:0px;">
                                                                    <button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#TaxInfo").text()}"></i></button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input id="TxtOtherCharge" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(arr.Table[k].item_oc).toFixed(ValDecDigit)}" name="OtherCharge" placeholder="0000.00" disabled>
                                                            </td>
                                                            <td>
                                                                <input id="NetValueinBase" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(BaseVal).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                               
                                                            </td>
                                                              <td>
                                                                 <textarea id="SPIItemRemarks" value="${arr.Table[k].it_remarks}" class="form-control remarksmessage" onmouseover="OnMouseOver(this)" autocomplete="off" name="ItemRemarks"  maxlength="300" placeholder="${$("#span_remarks").text()}">${arr.Table[k].it_remarks}</textarea>
                                                               </td>

                            </tr>`);
                                
                                var Itm_ID = arr.Table[k].item_id;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var docid = $("#DocumentMenuId").val();
                                if (docid == '105101150') {
                                    if (GstApplicable == "Y") {
                                        $("#HdnTaxOn").val("Item");
                                        Cmn_ApplyGSTToAtable("ServicePIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue", arr.Table1, "Y");
                                    }
                                    else {
                                        $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                        if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                            $("#HdnTaxOn").val("Item");
                                            $("#TaxCalcItemCode").val(Itm_ID);
                                            //$("#HiddenRowSNo").val(k);
                                            $("#Tax_AssessableValue").val(arr.Table[k].grossval);
                                            $("#TaxCalcGRNNo").val(VerNo);
                                            $("#TaxCalcGRNDate").val(VerDate);
                                            var TaxArr = arr.Table1;
                                            let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                            selected = JSON.stringify(selected);
                                            TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                            selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                            selected = JSON.stringify(selected);
                                            TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                            if (TaxArr.length > 0) {
                                                AddTaxByHSNCalculation(TaxArr);
                                                OnClickSaveAndExit("Y");
                                                var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                                Reset_ReOpen_LevelVal(lastLevel);
                                            }
                                        }

                                    }
                                }
                            }
                            var TxtOC = $("#TxtOtherCharges").val();
                            //var _OCTotalAmt = $("#_OtherChargeTotalAmt").text();//Commented by Suraj Maurya on 06-01-2025
                            var _OCTotalAmt = $("#TxtDocSuppOtherCharges").text();
                            if (TxtOC == _OCTotalAmt) {
                                Calculate_OC_AmountItemWise(_OCTotalAmt)
                            }
                            CalculateAmount();                           
                            CalculateVoucherTotalAmount();
                            GetAllGLID();
                            
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GoodReceiptInvoice Error : " + err.message);
        }
    }
}
function OnChangeVerificationQty(RowID, e) {
    
    var docid = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var VerifyQty;
    var ItemName;
    VerifyQty = clickedrow.find("#Verific_qty").val();
    PendQty = clickedrow.find("#pend_qty").val();
    if (VerifyQty != "" && VerifyQty != ".") {
        VerifyQty = parseFloat(VerifyQty);
    }
    if (VerifyQty == ".") {
        VerifyQty = 0;
    }
    if (VerifyQty == "" || VerifyQty == 0) {
        clickedrow.find("#Verific_qtyError").text($("#valueReq").text());
        clickedrow.find("#Verific_qtyError").css("display", "block");
        clickedrow.find("#Verific_qty").css("border-color", "red");
        clickedrow.find("#Verific_qty").val("");
        clickedrow.find("#Verific_qty");//.focus();
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#Verific_qtyError").text("");
        clickedrow.find("#Verific_qtyError").css("display", "none");
        clickedrow.find("#Verific_qty").css("border-color", "#ced4da");
    }

    var VerifyQty = clickedrow.find("#Verific_qty").val();

    if (AvoidDot(VerifyQty) == false) {
        clickedrow.find("#Verific_qty").val("");
        VerifyQty = 0;
    }

    if (parseFloat(VerifyQty) > parseFloat(PendQty)) {
        clickedrow.find("#Verific_qtyError").text($("#ExceedingQty").text());
        clickedrow.find("#Verific_qtyError").css("display", "block");
        clickedrow.find("#Verific_qty").css("border-color", "red");
        clickedrow.find("#Verific_qty").val("");
        VerifyQty = 0;
    }
    else {
        if (VerifyQty == "" || VerifyQty == 0) {
            clickedrow.find("#Verific_qtyError").text($("#valueReq").text());
            clickedrow.find("#Verific_qtyError").css("display", "block");
            clickedrow.find("#Verific_qty").css("border-color", "red");
            clickedrow.find("#Verific_qty").val("");
            clickedrow.find("#Verific_qty");//.focus();
            errorFlag = "Y";
        }
        else {
            clickedrow.find("#Verific_qtyError").text("");
            clickedrow.find("#Verific_qtyError").css("display", "none");
            clickedrow.find("#Verific_qty").css("border-color", "#ced4da");
        }
    }

    clickedrow.find("#Verific_qty").val(parseFloat(VerifyQty).toFixed(QtyDecDigit));



}

function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");


    return true;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    } 
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#TxtRateError").css("display", "none");
    clickedrow.find("#TxtRate").css("border-color", "#ced4da");


    return true;
}

function CheckedCancelled() {
    //
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}

function OnchangeSourceType() {
    
    var sourcetype = "D";
    if ($("#SourceTypeD").is(":checked")) {
        sourcetype = "D";       
    }
    if ($("#SourceTypeV").is(":checked")) {
        sourcetype = "V";
    }
    $("#hdSrc_type").val(sourcetype);
    if (sourcetype == "D") {
        $("#DivSrcDocNo").css("display", "none");
        $("#DivSrcDocDt").css("display", "none");
        $("#DivPlusAddBtn").css("display", "none");
        $("#ServicePIItemTbl .plus_icon1").css("display", "block");
        $("#ServicePIItemTbl >tbody >tr").remove();

    }
    else {
        $("#DivSrcDocNo").css("display", "block");
        $("#DivSrcDocDt").css("display", "block");
        $("#DivPlusAddBtn").css("display", "block");
        $("#ServicePIItemTbl .plus_icon1").css("display", "none");
        $("#ServicePIItemTbl >tbody >tr").remove();
    }
}

function OnClickbillingAddressIconBtn(e) {
    
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var PODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        PODTransType = "Update";
    }

    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, PODTransType);
}
async function OnChangeBillNo() {
    let supp_acc_id = $("#supp_acc_id").val();
    const Bill_No = $("#Bill_No").val();
    const bill_dt = $("#Bill_Date").val();
    const DocumentMenuId = $("#DocumentMenuId").val();
    const supp_id = $("#SupplierName").val();
    const hdn_Bill_No = $("#hdn_Bill_No").val();
    const hdn_Bill_Dt = $("#hdn_Bill_Dt").val();

    const showError = (id, msgId) => {
        $(id).css("border-color", "Red");
        $(msgId).text($("#valueduplicate").text()).css("display", "block");
    };

    const clearError = (id, msgId) => {
        $(id).css("border-color", "#ced4da");
        $(msgId).css("display", "none");
    };
    if (!Bill_No) {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text()).css("display", "block");
        return;
    } else {
        clearError("#Bill_No", "#SpanBillNoErrorMsg");
    }
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
        data: { supp_id, Bill_No, doc_id: DocumentMenuId, bill_dt: bill_dt },
        dataType: "json",
        success: function (data) {
            if (data === "ErrorPage") {
                LSO_ErrorPage();
                return;
            }

            if (!data) return;

            const arr = JSON.parse(data);
            const result = arr?.Table[0]?.Result;

            if (result === "Duplicate") {
                if (hdn_Bill_No !== Bill_No) {
                    showError("#Bill_No", "#SpanBillNoErrorMsg");
                    showError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else if (hdn_Bill_Dt !== bill_dt) {
                    showError("#Bill_No", "#SpanBillNoErrorMsg");
                    showError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else {
                    clearError("#Bill_No", "#SpanBillNoErrorMsg");
                    clearError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("N");
                }
            } else {
                clearError("#Bill_No", "#SpanBillNoErrorMsg");
                clearError("#Bill_Date", "#SpanBillDateErrorMsg");
                $("#duplicateBillNo").val("N");
            }
        }
    });
    Cmn_RefreshBillNoBillDateInGLDetail(supp_acc_id, Bill_No, bill_dt);
}
async function OnChangeBillNo1() {
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }

    /* ------ To Update bill number and date to GL Section ------ */
    let supp_acc_id = $("#supp_acc_id").val();
    let bill_no = $("#Bill_No").val();
    let bill_dt = $("#Bill_Date").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var Bill_No = $("#Bill_No").val();
    var supp_id = $("#SupplierName").val();
    var hdn_Bill_No = $("#hdn_Bill_No").val();
    await $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
            data: { supp_id:supp_id, Bill_No: Bill_No, doc_id: DocumentMenuId },
            dataType: "json",
            success: function (data) {
                debugger
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table[0].Result == "Duplicate") {
                        if (hdn_Bill_No != Bill_No) {
                            $("#Bill_No").css("border-color", "Red");
                            $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
                            $("#SpanBillNoErrorMsg").css("display", "block");
                            $("#duplicateBillNo").val("Y");
                        }
                        else {
                            $("#SpanBillNoErrorMsg").css("display", "none");
                            $("#Bill_No").css("border-color", "#ced4da");
                            $("#duplicateBillNo").val("N");
                        }
                    }
                    else {
                        $("#SpanBillNoErrorMsg").css("display", "none");
                        $("#Bill_No").css("border-color", "#ced4da");
                        $("#duplicateBillNo").val("N");
                    }
                }
            },
        });
    Cmn_RefreshBillNoBillDateInGLDetail(supp_acc_id, bill_no, bill_dt);
}
function OnChangeBillDate() {
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }
    OnChangeBillNo();
    /* ------ To Update bill number and date to GL Section ------ */
    //let supp_acc_id = $("#supp_acc_id").val();
    //let bill_no = $("#Bill_No").val();
    //let bill_dt = $("#Bill_Date").val();

    //Cmn_RefreshBillNoBillDateInGLDetail(supp_acc_id, bill_no, bill_dt);
    /* ------ To Update bill number and date to GL Section End------ */

    //GetAllGLID();//Commented by Suraj Maurya on 06-12-2024
}
//------------------------OC-------------------------//
async function OnClickSaveAndExit_OC_Btn() {
    debugger;
    await CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueInBase", "#NetOrderValueInBase");
    click_chkplusminus();
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    if (parseFloat(CheckNullNumber(TotalOCAmt)) < 0) {
        TotalOCAmt = 0;
    }
    $("#ServicePIItemTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
       /* var NetOrderValueInBase = currentRow.find("#TxtNetValue").val();*/
        var NetOrderValueBase = currentRow.find("#NetValueinBase").val();

        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
            }
        } else {
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#ServicePIItemTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);
        var OCValue = currentRow.find("#TxtOtherCharge").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        var Sno = 0;
        $("#ServicePIItemTbl >tbody >tr").each(function () {
            
            var currentRow = $(this);
            Sno = Sno + 1;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        var Sno = 0;
        $("#ServicePIItemTbl >tbody >tr").each(function () {
            
            Sno = Sno + 1;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#ServicePIItemTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);

        var POItm_GrossValue = currentRow.find("#TxtItemGrossValue").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        }
        var POItm_NetOrderValueInBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        /*currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueInBase)).toFixed(DecDigit));*/
        FinalPOItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(DecDigit);
        currentRow.find("#NetValueinBase").val((parseFloat(FinalPOItm_NetOrderValueBase)).toFixed(DecDigit));
    });
    CalculateAmount();
};
function CalculateAmount(flag) {
    //

    var DecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    //var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#ServicePIItemTbl >tbody >tr").each(function () {
        //
        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#NetValueinBase").val() == "" || currentRow.find("#NetValueinBase").val() == null || currentRow.find("#NetValueinBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#NetValueinBase").val())).toFixed(DecDigit);
        }

    });
    
    $("#TxtTaxAmount").val(TaxValue);
    
    var oc_amount = $("#TxtDocSuppOtherCharges").val();
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amount));
    $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(DecDigit));
    $("#TxtGrossValue").val(GrossValue).trigger('change');

    if (flag == "CallByGetAllGL") {
        ApplyRoundOff("CallByGetAllGL");
    }
}
function SetOtherChargeVal() {
    $("#ServicePIItemTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed($("#ValDigit").text()));
    })
}
function BindOtherChargeDeatils(val) {
    var DecDigit = $("#ValDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    //$("#PI_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //  
            var td = "";
            if (DocumentMenuId == "105101150") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td >${currentRow.find("#td_OCSuppName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
            if (DocumentMenuId == "105101150") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
            }

        });
        
    }
    //$("#SI_OtherChargeTotal").text(TotalAMount);
    $("#_OtherChargeTotal").text(TotalAMount);
    if (DocumentMenuId == "105101150") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
    //var GstCat = $("#Hd_GstCat").val();
    //var ToTdsAmt = 0;
    //if (GstCat == "UR") {
    //    ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val())) + parseFloat(CheckNullNumber(TotalAMount));
        
    //} else {
    //    ToTdsAmt = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
    //}
    //ResetTDS_CalOnchangeDocDetail(ToTdsAmt,"#TxtTDS_Amount");
    if (val == "") {
        GetAllGLID();
    }

}

//--------------------------------------------//

//-------------GL Detail---------------------------------//
function BindDDLAccountList() {
    
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105101150");
}
function BindData() {
    
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        
                        var selected = $("#Acc_name_" + (rowid-1)).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });
                rowid = parseFloat(rowid) + 1;
            });
        }
    }
    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        if (AccID != '0' && AccID != "") {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
function OnChangeAccountName(RowID, e) {
    
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var Acc_Name = clickedrow.find("#Acc_name_" + SNo+" option:selected").text();

    clickedrow.find("#SpanAcc_name_" + SNo).css("display", "none");
    clickedrow.find("[aria-labelledby='select2-Acc_name_" + SNo + "-container']").css("border-color", "#ced4da");

    if (Acc_ID != null) {
        var hdn_acc_id = clickedrow.find("#hfAccID").val();
        var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
        if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
            var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
            if (Len > 0) {
                $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').each(function () {
                    var row = $(this);
                    var vouSrNo = row.find("#hdntbl_vou_sr_no").text();
                    if (vouSrNo == vou_sr_no) {
                        row.remove();
                    }
                });
            }
            //Added by Suraj on 09-08-2024 to stop reset of GL Account if user changes the GL Acc.
            $("#ServicePIItemTbl >tbody >tr #hdn_item_gl_acc[value='"+hdn_acc_id+"']").closest('tr').each(function () {
                var row = $(this);
                row.find("#hdn_item_gl_acc").val(Acc_ID);
            });
        }
        if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
            clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
        }
        else {
            clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
        }
    }
    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
}
function GetAllGLID(flag) {
    
    //var GstType = $("#Hd_GstType").val();
    //var GstCat = $("#Hd_GstCat").val();
    //var NetInvValue = $("#NetOrderValueInBase").val();
    //var NetTaxValue = $("#TxtTaxAmount").val();
    //var ValueWithoutTax = (parseFloat(NetInvValue) - parseFloat(NetTaxValue));
    //var ValDecDigit = $("#ValDigit").text();///Amount
    //var supp_id = $("#SupplierName").val();
    //var SuppVal = 0;
    //var Compid = $("#CompID").text();
    //var InvType = "D";
    //var TransType = 'Pur';
    //var GLDetail = [];
    //var TxaExantedItemList = [];
    //GLDetail.push({
    //    comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal
    //    , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp"
    //});
    //$("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
    //    //
    //    var currentRow = $(this);
    //    var item_id = currentRow.find("#hfItemID").val();
    //    var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();

    //    var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
    //    var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
    //    var TxaExanted = "N";
    //    if (currentRow.find("#TaxExempted").is(":checked")) {
    //        TxaExanted = "Y";
    //        TxaExantedItemList.push({ item_id: item_id });
    //    }
    //    var GstApplicable = $("#Hdn_GstApplicable").text();
    //    if (GstApplicable == "Y") {
    //        if (ItemTaxAmt == TaxAmt) {
    //            if (currentRow.find("#ManualGST").is(":checked")) {
    //                TxaExanted = "Y";
    //                TxaExantedItemList.push({ item_id: item_id });
    //            }
    //        }
    //    }
    //    GLDetail.push({ comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm" });
    //});

    //$("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
    //    //
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#TaxNameID").text();
    //    var tax_amt = currentRow.find("#TaxAmount").text();
    //    GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
    //});
    //$("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
    //    // 
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#taxID").text();
    //    var tax_amt = currentRow.find("#TotalTaxAmount").text();
    //    var TaxPerc = currentRow.find("#TaxPerc").text();
    //    GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
    //});
    //$("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
    //    // 
    //    var currentRow = $(this);
    //    var tds_id = currentRow.find("#td_TDS_NameID").text();
    //    var tds_amt = currentRow.find("#td_TDS_Amount").text();
    //    GLDetail.push({ comp_id: Compid, id: tds_id, type: "Tds", doctype: InvType, Value: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds" });
    //});
    //$("#Tbl_OtherChargeList >tbody >tr").each(function (i, row) {
    //    //
    //    var currentRow = $(this);
    //    var oc_id = currentRow.find("#OCID").text();
    //    var oc_amt = currentRow.find("#OCAmtSp1").text();
    //    GLDetail.push({ comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC" });
    //});
    // 
    //if (GstCat == "UR") {
    //    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
    //         
    //        var currentRow = $(this);
    //        var TaxID = currentRow.find("#TaxNameID").text();
    //        TaxID = "R" + TaxID;
    //        var TaxPerc = currentRow.find("#TaxPercentage").text();
    //        var TaxPerc_id = TaxPerc.replace("%", "");
    //        var TaxVal = currentRow.find("#TaxAmount").text();
    //        var TaxItmCode = currentRow.find("#TaxItmCode").text();

    //        if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
    //            if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
    //                objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
    //                GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
    //            } else {
    //                GLDetail.push({ comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType });
    //            }
    //        }           
    //    });
    //}
    //
    //$.ajax({
    //    type: "POST",
    //    url: "/ApplicationLayer/ServicePurchaseInvoice/GetGLDetails",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    data: JSON.stringify({ GLDetail: GLDetail }),
    //    success: function (data) {
    //        
    //        if (data == 'ErrorPage') {
    //            //SI_ErrorPage();
    //            return false;
    //        }
    //        if (data !== null && data !== "") {
    //            var arr = [];
    //            arr = JSON.parse(data);
    //            var Voudet = 'Y';
    //            $('#VoucherDetail tbody tr').remove();
    //            if (arr.Table1.length > 0) {
                    
    //                var errors = [];
    //                var step = [];
    //                for (var i = 0; i < arr.Table1.length; i++) {
    //                    // 
    //                    if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
    //                        errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
    //                        step.push(parseInt(i));
    //                        Voudet = 'N';
    //                    }
    //                }
    //                var arrayOfErrorsToDisplay = [];
    //                $.each(errors, function (i, error) {
    //                    arrayOfErrorsToDisplay.push({ text: error });
    //                });
    //                Swal.mixin({
    //                    confirmButtonText: 'Ok',
    //                    type: "warning",
    //                }).queue(arrayOfErrorsToDisplay)
    //                    .then((result) => {
    //                        if (result.value) {

    //                        }
    //                    });
    //            }
    //            if (Voudet == 'Y') {
    //                if (arr.Table.length > 0) {
    //                    $('#VoucherDetail tbody tr').remove();
    //                    var rowIdx = $('#VoucherDetail tbody tr').length;
    //                    for (var j = 0; j < arr.Table.length; j++) {
    //                        ++rowIdx;
    //                        //var Acc_Name_previos = arr.Table1.acc_id;
    //                        var Acc_Id = arr.Table[j].acc_id;
    //                        var acc_Id = Acc_Id.substring(0, 1);
    //                        var Disable;
    //                        if (acc_Id == "3" || acc_Id == "4") {
    //                            Disable = "";
    //                        }
    //                        else {
    //                            Disable = "disabled";
    //                        }
    //                        var FieldType = "";
    //                        if (arr.Table[j].type == 'Itm') {
    //                            FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;">
    //                            <select class="form-control" id="Acc_name_${rowIdx}" onchange="OnChangeAccountName(${rowIdx},event)">
    //                                <option value="${arr.Table[j].acc_id}">${arr.Table[j].acc_name}</option>
    //                             </select>
    //                            <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" />
    //                            <input type="hidden" value='DDLValue' id="hdnAccType">
    //                            <span id="SpanAcc_name_${rowIdx}" class="error_message"></span></div> `;
    //                        }
    //                        else {
    //                            FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /><input type="hidden" value='TxtValue' id="hdnAccType">`;
    //                        }
    //                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
    //                            if (arr.Table[j].type == "RCM" || arr.Table[j].type == "Tds") {
    //                                $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
    //                                <td class="sr_padding">${rowIdx}</td>
    //                                <td>`+ FieldType + `</td>
    //                                <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
    //                                <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
    //                                <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
    //                                <td class="center">
    //                                    <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
    //                                 </td>
    //                                 <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
    //                                 <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
    //                                <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
    //                                <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>          
    //                        </tr>`);
    //                            }
    //                            else {
    //                                $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
    //                                <td class="sr_padding">${rowIdx}</td>
    //                                <td>`+ FieldType + `</td>
    //                                <td id="dramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
    //                                <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
    //                                <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
    //                                <td class="center">
    //                                    <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
    //                                 </td>
    //                                 <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
    //                                 <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
    //                                <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
    //                                <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>
                                   
    //                        </tr>`);
    //                            }
    //                        }
    //                        Cmn_BindAccountList1("#Acc_name_", rowIdx, "#VoucherDetail", "#SNohf", "BindData", "105101150");
    //                        
    //                        if ($("#ForDisableOCDlt").val() == "Disable") {
    //                            
    //                            $("#Acc_name_" + rowIdx).attr("disabled", true);
    //                        }
    //                    }
    //                    var tds_amt = $("#TxtTDS_Amount").val();
    //                    if (GstCat == "UR") {
    //                        var CrAmt = parseFloat(parseFloat(ValueWithoutTax) - parseFloat(CheckNullNumber(tds_amt))).toFixed(ValDecDigit);
    //                        $("#VoucherDetail >tbody >tr:first").find("#cramt").text(CrAmt);

    //                    }
    //                    else {
    //                        var CrAmt = parseFloat(parseFloat($("#NetOrderValueInBase").val()) - parseFloat(CheckNullNumber(tds_amt))).toFixed(ValDecDigit);
    //                        $("#VoucherDetail >tbody >tr:first").find("#cramt").text(CrAmt);
    //                    }
    //                }

    //            }
    //            CalculateVoucherTotalAmount();
    //        }
    //    }
    //});
    GetAllGL_WithMultiSupplier(flag);
}
async function GetAllGL_WithMultiSupplier(flag) {
    console.log("GetAllGL_WithMultiSupplier")

    if ($("#ServicePIItemTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    if (CheckSPI_ItemValidationsForGL() == false) {
        return false;
    }
    CalculateAmount("CallByGetAllGL");/*Added by Suraj on 29-03-2024*/
    //if (flag != "RO") {
        
    //    ApplyRoundOff("CallByGetAllGL");
    //}
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    var DocStatus = $('#hfStatus').val().trim();
    /////////////////12-07-2023 Comment by Shubham Maurya //////////
    //var NetInvValue = $("#NetOrderValueSpe").val();
    var NetInvValue = $("#NetOrderValueInBase").val();
    //var NetInvValue = $("#TxtGrossValue").val();
    var NetTaxValue = $("#TxtTaxAmount").val();
    var conv_rate = $("#conv_rate").val();
    var ValueWithoutTax = parseFloat(NetInvValue);
    var ValDecDigit = $("#ValDigit").text();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    //SuppValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    var curr_id = $("#Hdn_ddlCurrency").val();
    var bill_no = IsNull($("#Bill_No").val(),'');
    var bill_dt = IsNull($("#Bill_Date").val(),'');
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];
    
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt,acc_id:""
    });
   
    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {

        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        if (item_id != "" && item_id != null) {
            var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
            var ItmGrossValInBase = currentRow.find("#TxtItemGrossValue").val();
            var TxtGrnNo = currentRow.find("#TxtGrnNo").val();
            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
            var ItemAccId = currentRow.find("#hdn_item_gl_acc").val()
            var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
            var TxaExanted = "N";
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
            }
            if (ItemTaxAmt == TaxAmt) {
                if (currentRow.find("#ManualGST").is(":checked")) {
                    TxaExanted = "Y";
                    TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
                }
            }

            GLDetail.push({
                comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
                , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
                , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt
                , acc_id: IsNull(ItemAccId, '0')
            });
        }
        
    });

    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var oc_id = currentRow.find("#OC_ID").text();
        var oc_acc_id = currentRow.find("#OCAccId").text();
        var oc_amt = currentRow.find("#OCAmtSp").text();
        var oc_amt_bs = currentRow.find("#OCAmtBs").text();
        var oc_supp_id = currentRow.find("#td_OCSuppID").text();
        var oc_supp_acc_id = currentRow.find("#OCSuppAccId").text();
        var oc_supp_type = currentRow.find("#td_OCSuppType").text();
        var oc_amt_bs_wt = currentRow.find("#OCTotalTaxAmt").text();
        var oc_conv_rate = currentRow.find("#OC_Conv").text();
        var oc_curr_id = currentRow.find("#HdnOC_CurrId").text();
        var oc_supp_bill_no = IsNull(currentRow.find("#OCBillNo").text(),'');
        var oc_supp_bill_dt = IsNull(currentRow.find("#OCBillDt").text(),'');
        var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

        var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? supp_acc_id : oc_supp_acc_id;

        GLDetail.push({
            comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt
            , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate
            , bill_no: oc_supp_bill_no == "" ? bill_no : oc_supp_bill_no
            , bill_date: oc_supp_bill_dt == "" ? bill_dt : oc_supp_bill_dt, acc_id: ""
        });

        if (oc_supp_id != "" && oc_supp_id != "0") {
            let gl_type = "Supp";
            if (oc_supp_type == "2")
                gl_type = "Supp";
            if (oc_supp_type == "7")
                gl_type = "Bank";

            GLDetail.push({
                comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: InvType, Value: oc_amt_wt
                , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: oc_supp_bill_no
                , bill_date: oc_supp_bill_dt, acc_id: ""
            });
        } else {
            //if (GLDetail.findIndex((obj => obj.id == supp_id)) > -1) {
            //    objIndex = GLDetail.findIndex((obj => obj.id == supp_id));
            //    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(oc_amt_wt);
            //    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(oc_amt_bs_wt);
            //}
        }
    });
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var oc_id = currentRow.find("#TaxItmCode").text();
        var TaxPerc = currentRow.find("#TaxPercentage").text();
        var TaxPerc_id = TaxPerc.replace("%", "");
        var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
        GLDetail.push({
            comp_id: Compid, id: tax_id, type: "OcTax", doctype: InvType, Value: tax_amt
            , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: ArrOcGl[0].id
            , Entity_id: tax_acc_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate,
            bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date,''), acc_id: ""
        });
        if (GstCat == "UR") { /* Added by Suraj Maurya on 04-08-2025 to Add in RCM all tax on OC for Self Supplier */
            tax_id = "R" + tax_id;
            $("#ht_Tbl_OC_Deatils >tbody >tr").find("#OC_ID:contains(" + oc_id + ")").closest('tr').each(function () {
                if ($(this).find("#td_OCSuppID").text() == "0" && $(this).find("#OC_ID").text() == oc_id) {
                    GLDetail.push({
                        comp_id: Compid, id: tax_id, type: "RCM", doctype: InvType, Value: tax_amt
                        , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                    });
                }
            });
        }
    });

    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();
        var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
        var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
        if (TaxExempted == false) {
            if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
                var GstApplicable = $("#Hdn_GstApplicable").text();

                var ClaimItc = true;
                if (GstApplicable == "Y") {
                    ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
                }


                if (TaxRecov == "N" || !ClaimItc) {
                    if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
                        var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
                        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
                        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
                    }
                } else {
                    GLDetail.push({
                        comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                    });
                }
            }
        }
        
        
    });

    //$("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#taxID").text();
    //    var tax_acc_id = currentRow.find("#taxAccID").text();
    //    var tax_amt = currentRow.find("#TotalTaxAmount").text();
    //    var TaxPerc = currentRow.find("#TaxPerc").text();
    //    GLDetail.push({
    //        comp_id: Compid, id: tax_acc_id, type: "Tax", doctype: InvType, Value: tax_amt
    //        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
    //        , Entity_id: tax_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt
    //    });
    //});
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // 
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        //tds_amt = parseFloat(tds_amt).toFixed(0);
        Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
        GLDetail.push({
            comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
            , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: supp_acc_id
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    });
    if (Cal_Tds_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "TSupp", doctype: InvType
            , Value: Cal_Tds_Amt, ValueInBase: Cal_Tds_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }

    var Oc_Tds = [];
    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // 
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        var tds_supp_id = currentRow.find("#td_TDS_Supp_Id").text();
        
        var ArrOcGl = GLDetail.filter(v => (v.id == tds_supp_id && v.type == "Supp"));
        var tdsIndex = Oc_Tds.findIndex(v => v.supp_id == tds_supp_id);
        if (tdsIndex > -1) {
            Oc_Tds[tdsIndex].tds_amt = parseFloat(Oc_Tds[tdsIndex].tds_amt) + parseFloat(tds_amt);
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        } else {
            Oc_Tds.push({
                supp_id: tds_supp_id, supp_acc_id: ArrOcGl[0].id, tds_amt: tds_amt
                , bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date,'')
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        }
        

        
    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item,idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: IsNull(item.bill_no, ''), bill_date: IsNull(item.bill_date,'')
                , acc_id: ""
            });
        });
        
    }
    

    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            var DocNo = currentRow.find("#DocNo").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();
            var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
            if (ClaimITC) {

                if (TaxRecov == "N") {
                }
                else {
                    if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
                        if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
                            objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
                            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                        } else {
                            GLDetail.push({
                                comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal
                                , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                                , Entity_id: TaxAccID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt
                                , acc_id: ""

                            });
                        }

                    }
                }
            }


            //if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
            //    if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
            //        objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
            //        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
            //        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
            //    } else {
            //        GLDetail.push({
            //            comp_id: Compid, id: TaxAccID, type: "RCM", doctype: InvType, Value: TaxVal
            //            , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
            //            , Entity_id: TaxID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt

            //        });
            //    }

            //}

        });
    }

    await Cmn_GLTableBind(supp_acc_id, GLDetail,"Purchase");
    //var glData = JSON.stringify(GLDetail);
    //await $.ajax({
    //    type: "POST",
    //    //url: "/Common/Common/GetGLDetail1",
    //    url: "/Common/Common/GetGLDetails_StringData",
    //    //contentType: "application/json; charset=utf-8",
    //    //dataType: "json",
    //    //data: JSON.stringify({ GLDetail: GLDetail }),
    //    data: { GLDetail: glData },
    //    success: function (data) {
            
    //        if (data == 'ErrorPage') {
    //            Comn_ErrorPage();
    //            return false;
    //        }

    //        if (data !== null && data !== "") {
    //            var arr = [];
    //            arr = JSON.parse(data);
    //            var Voudet = 'Y';
    //            $('#VoucherDetail tbody tr').remove();
    //            if (arr.Table1.length > 0) {
    //                var errors = [];
    //                var step = [];
    //                for (var i = 0; i < arr.Table1.length; i++) {

    //                    if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
    //                        errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
    //                        step.push(parseInt(i));
    //                        Voudet = 'N';
    //                    }
    //                }
    //                var arrayOfErrorsToDisplay = [];
    //                $.each(errors, function (i, error) {
    //                    arrayOfErrorsToDisplay.push({ text: error });
    //                });
    //                Swal.mixin({
    //                    confirmButtonText: 'Ok',
    //                    type: "warning",
    //                }).queue(arrayOfErrorsToDisplay)
    //                    .then((result) => {
    //                        if (result.value) {

    //                        }
    //                    });
    //            }
    //            if (Voudet == 'Y') {
    //                if (arr.Table.length > 0) {
    //                    $('#VoucherDetail tbody tr').remove();
    //                    //let supp_acc_id = $("#supp_acc_id").val();
    //                    var arrSupp = arr.Table.filter(v => (v.type == "TSupp" || v.type == "Supp" || v.type == "Bank"));
    //                    var mainSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "Supp");
    //                    var TdsSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "TSupp");
    //                    var NewArrSupp = arrSupp.filter(v => v.acc_id != supp_acc_id).sort((a,b)=>a.acc_id-b.acc_id);
    //                    if (TdsSuppGl.length > 0) {
    //                        NewArrSupp.unshift(TdsSuppGl[0]);
    //                    }
    //                    NewArrSupp.unshift(mainSuppGl[0]);
    //                    arrSupp = NewArrSupp;
    //                    for (var j = 0; j < arrSupp.length; j++) {
    //                        let supp_id = arrSupp[j].id;
    //                        let supp_type = arrSupp[j].type;
    //                        let supp_bill_no = arrSupp[j].bill_no;
    //                        let supp_bill_dt = arrSupp[j].bill_dt;
    //                        let arrDetail;// = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank")));
    //                        //let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && (v.type == "OC" || v.type == "Itm" || v.type == "Tax" || v.type == "RCM")));
    //                        let arrDetailDr;
    //                        if (supp_type == "TSupp") {
    //                            arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "TSupp")));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type == "Tds"));
    //                        } else {
    //                            arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank") && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "OcTax" && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                        }
                            

    //                        let RcmValue = 0;
    //                        let RcmValueInBase = 0;
    //                        if (supp_type != "TSupp") {
    //                            arr.Table.filter(v => (v.parent == supp_id && v.type == "RCM"))
    //                                .map((res) => {
    //                                    RcmValue = RcmValue + res.Value;
    //                                    RcmValueInBase = RcmValueInBase + res.ValueInBase;
    //                                });
    //                        }
                           
    //                        arrDetail[0].Value = arrDetail[0].Value - RcmValue;
    //                        arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

    //                        /*--------------------- For Round of After RCM Applied //Under Proccess And Commented by Suraj Maurya on 05-12-2024--------------------*/
    //                        //if (parseFloat(RcmValueInBase) > 0 || parseFloat(RcmValue) > 0) {
    //                        //    let round_type = "";
    //                        //    if (supp_acc_id == supp_id) {
    //                        //        if ($("#chk_roundoff").is(":checked")) {
    //                        //            if ($("#p_round").is(":checked")) {
    //                        //                round_type = "P";
    //                        //            }
    //                        //            if ($("#m_round").is(":checked")) {
    //                        //                round_type = "M";
    //                        //            }
    //                        //            arrDetail[0].ValueInBase = To_RoundOff(arrDetail[0].ValueInBase, round_type)
    //                        //            //arrDetail[0].ValueInBase = Math.round(arrDetail[0].ValueInBase);
    //                        //            arrDetail[0].Value = arrDetail[0].ValueInBase / conv_rate;
    //                        //        } 
    //                        //    }
    //                        //}
    //                        /*--------------------- For Round of After RCM Applied End--------------------*/

    //                        let rowSpan = 1;//arrDetailDr.length + 1;
    //                        let GlRowNo = 1;
    //                        // First Row Generated here for all GL Voucher 
    //                        let vouType = "";
    //                        if (arrDetail[0].type == "TSupp")
    //                            vouType = "DN"
    //                        if (arrDetail[0].type == "Supp")
    //                            vouType = "PV"
    //                        if (arrDetail[0].type == "Bank")
    //                            vouType = "BP"
    //                        if (vouType == "DN") {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        } else {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        }
                           
    //                        let ArrTax = [];
    //                        let ArrGlDetailsDr = [];
    //                        for (var k = 0; k < arrDetailDr.length; k++) {
    //                            //GlRowNo++;
    //                            // Row Generated here for Other charge and Tax on item
    //                            let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                            if (getAccIndex > -1) {
    //                                ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                            } else {
    //                                //rowSpan++;
    //                                if (arrDetailDr[k].type == "Tax") {
    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                                    } else {
    //                                        //rowSpan++;
    //                                        ArrTax.push(arrDetailDr[k]);
    //                                    }
    //                                    // ArrTax.push(arrDetailDr[k]);
    //                                }
    //                                else if (arrDetailDr[k].type == "OcTax") {
    //                                }
    //                                else {
    //                                    ArrGlDetailsDr.push(arrDetailDr[k]);
    //                                }
    //                            }



    //                            if (arrDetailDr[k].type == "OC") {

    //                                let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].oc_id && v.type == "OcTax"));
    //                                // Grouping the similer tax account
    //                                for (var l = 0; l < arrDetailOCDr.length; l++) {

    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
    //                                    } else {
    //                                        //rowSpan++;
    //                                        ArrTax.push(arrDetailOCDr[l]);
    //                                    }
    //                                }
    //                                //Updating rowspan as Tax on OC added

    //                            }
    //                        }
    //                        var ArrGLDetailRCM = []
    //                        for (var i = 0; i < ArrGlDetailsDr.length; i++) {

    //                            if (ArrGlDetailsDr[i].type == "RCM") {
    //                                ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

    //                            } else {
    //                                GlRowNo++;
    //                                rowSpan++;
    //                                if (ArrGlDetailsDr[i].type == "Tds") {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                } else {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                }
                                    
    //                            }

    //                        }
    //                        for (var i = 0; i < ArrTax.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            // Row Generated here for Tax on Other Charge
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
    //                                , ArrTax[i].Value, ArrTax[i].ValueInBase, 0, 0, vouType,
    //                                ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
    //                            )

    //                        }
    //                        for (var i = 0; i < ArrGLDetailRCM.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
    //                                , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
    //                                , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
    //                                , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
    //                            )
    //                        }
    //                        $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
    //                    }
    //                }
    //            }
    //            BindDDLAccountList();
    //            CalculateVoucherTotalAmount();
    //        }
    //    }
    //});

}

function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {
    var ValDecDigit = $("#ValDigit").text();///Amount
    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }
    if (parseFloat(DrValue) < 0) {
        CrValue = Math.abs(DrValue);
        DrValue = 0;
    }
    if (parseFloat(DrValueInBase) < 0) {
        CrValueInBase = Math.abs(DrValueInBase);
        DrValueInBase = 0;
    }
    if (parseFloat(CrValue) < 0) {
        DrValue = Math.abs(CrValue);
        CrValue = 0;
    }
    if (parseFloat(CrValueInBase) < 0) {
        DrValueInBase = Math.abs(CrValueInBase);
        CrValueInBase = 0;
    }

    let hfSrNo = $('#VoucherDetail tbody tr').length + 1;
    var FieldType = "";
    if (type == 'Itm') {
        FieldType = `<div class="col-sm-11 lpo_form no-padding">
                            <select class="form-control" id="Acc_name_${hfSrNo}" onchange ="OnChangeAccountName(${hfSrNo},event)">
                              </select>
                        </div>`;
        $("#hdnAccID").val(acc_id);
    }
    else {
        FieldType = `${acc_name}`;
    }
    let Table_tds = `<td id="td_GlSrNo">${GlSrNo}</td>
                                    <td id="td_vou_sr_no" hidden>${SrNo}</td>
                                    <td id="td_GlAccName" class="no-padding">
                                        `+ FieldType + `
                                    </td>
                                    <td id="tdhdn_GlAccId" hidden>${acc_id}</td>
                                    <td class="num_right" id="dramt">${parseFloat(DrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="dramtInBase">${parseFloat(DrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramt">${parseFloat(CrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramtInBase">${parseFloat(CrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="center" id="td_CC">
                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
                                    </td>
                                <td hidden>
                                    <input type="hidden" id="SNohf" value="${hfSrNo}" />
                                    <input type="hidden" id="type" value="${type}"/>
                                    <input  type="hidden" id="txthfAccID" value="${acc_name}"/>
                                    <input  type="hidden" id="hfAccID"  value="${acc_id}" />
                                    <input  type="hidden" id="glVouType"  value="${VouType}" />
                                    <input  type="hidden" id="gl_curr_id"  value="${curr_id}" />
                                    <input  type="hidden" id="gl_conv_rate"  value="${conv_rate}" />
                                    <input  type="hidden" id="gl_bill_no"  value="${bill_bo}" />
                                    <input  type="hidden" id="gl_bill_date"  value="${bill_date}" />
                                    <input  type="hidden" id="dramt1" value="${parseFloat(DrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="dramtInBase1" value="${parseFloat(DrValueInBase).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramt1" value="${parseFloat(CrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramtInBase1" value="${parseFloat(CrValueInBase).toFixed(ValDecDigit)}"/>
                                </td>`
    if (type == "TSupp" || type == "Supp" || type == "Bank") {
        $('#VoucherDetail tbody').append(`<tr id="tr_GlRow${SrNo}">
                                <td rowspan="${rowSpan}" id="td_SrNo">${SrNo}</td>
                                `+ Table_tds + `
                                <td rowspan="${rowSpan}" id="td_VouType">${VouType}</td>
                                <td rowspan="${rowSpan}" id="td_VouNo"></td>
                                <td rowspan="${rowSpan}" id="td_VouDate"></td>
                                
                            </tr>`)
    } else {
        $('#VoucherDetail tbody').append(`<tr>
                                     `+ Table_tds + `
                                </tr>`)

    }
}

async function CalculateVoucherTotalAmount() {
    console.log("CalculateVoucherTotalAmount")
    var ValDecDigit = $("#ValDigit").text();
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    //$("#VoucherDetail >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
    //    }
    //});
    var VouDrCrList = [];
    $("#VoucherDetail >tbody >tr").each(function (index) {
        //
        var currentRow = $(this);
        var vou_sr_no = currentRow.find("#td_vou_sr_no").text();
        var dr_amt = currentRow.find("#dramt").text();
        var cr_amt = currentRow.find("#cramt").text()
        var dr_amt_bs = currentRow.find("#dramtInBase").text()
        var cr_amt_bs = currentRow.find("#cramtInBase").text()

        var getVouIndex = VouDrCrList.findIndex(m => m.vou_sr_no == vou_sr_no);
        if (getVouIndex > -1) {
            VouDrCrList[getVouIndex].dr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_sp) + parseFloat(dr_amt)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].dr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_base) + parseFloat(dr_amt_bs)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].cr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_sp) + parseFloat(cr_amt)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].cr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_base) + parseFloat(cr_amt_bs)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].vou_entries = VouDrCrList[getVouIndex].vou_entries + 1;
        } else {
            VouDrCrList.push({
                vou_sr_no: vou_sr_no
                , dr_in_sp: dr_amt, dr_in_base: dr_amt_bs
                , cr_in_sp: cr_amt, cr_in_base: cr_amt_bs, vou_entries: 1, all_previous_entries: index
            });
        }
    });

    var flag_check_diff = "N";
    VouDrCrList.map((item, index) => {
        DrTotAmt = parseFloat(DrTotAmt) + parseFloat(item.dr_in_sp);
        DrTotAmtInBase = parseFloat(DrTotAmtInBase) + parseFloat(item.dr_in_base);
        CrTotAmt = parseFloat(CrTotAmt) + parseFloat(item.cr_in_sp);
        CrTotAmtInBase = parseFloat(CrTotAmtInBase) + parseFloat(item.cr_in_base);

        if (parseFloat(item.dr_in_base) != parseFloat(item.cr_in_base)) {

            if (Math.abs(parseFloat(item.dr_in_base) - parseFloat(item.cr_in_base)) < 1) {
                flag_check_diff = "Y";
            }

        }
    });

    $("#DrTotal").text(parseFloat(DrTotAmt).toFixed(ValDecDigit));
    $("#DrTotalInBase").text(parseFloat(DrTotAmtInBase).toFixed(ValDecDigit));
    $("#CrTotal").text(parseFloat(CrTotAmt).toFixed(ValDecDigit));
    $("#CrTotalInBase").text(parseFloat(CrTotAmtInBase).toFixed(ValDecDigit));

    //if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        
    //    if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
    //        await AddRoundOffGL();
    //    }
        
    //}

    if (flag_check_diff == "Y") {
        await AddRoundOffGL();
    }

}
async function AddRoundOffGL() {
    
    var ValDecDigit = $("#ValDigit").text();///Amount
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            
            if (data == 'ErrorPage') {  
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                
                if (arr.Table.length > 0) {
                    /* Commented by Suraj Maurya on 05-12-2024  */
                    //var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    //var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    //var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    //var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);

                    //$("#VoucherDetail >tbody >tr").each(function () {
                    //    //
                    //    var currentRow = $(this);
                    //    if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
                    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                    //    }
                    //    else {
                    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
                    //    }
                    //    if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
                    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                    //    }
                    //    else {
                    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
                    //    }
                    //    if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
                    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                    //    }
                    //    else {
                    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
                    //    }
                    //    if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
                    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                    //    }
                    //    else {
                    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
                    //    }
                    //});
                    /* Commented by Suraj Maurya on 05-12-2024 end */
                     //-------------
    
                        var VouDrCrList = [];
                        $("#VoucherDetail >tbody >tr").each(function (index) {
                            //
                            var currentRow = $(this);
                            var vou_sr_no = currentRow.find("#td_vou_sr_no").text();
                            var dr_amt = currentRow.find("#dramt").text();
                            var cr_amt = currentRow.find("#cramt").text()
                            var dr_amt_bs = currentRow.find("#dramtInBase").text()
                            var cr_amt_bs = currentRow.find("#cramtInBase").text()

                            var getVouIndex = VouDrCrList.findIndex(m => m.vou_sr_no == vou_sr_no);
                            if (getVouIndex > -1) {
                                VouDrCrList[getVouIndex].dr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_sp) + parseFloat(dr_amt)).toFixed(cmn_ValDecDigit);
                                VouDrCrList[getVouIndex].dr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_base) + parseFloat(dr_amt_bs)).toFixed(cmn_ValDecDigit);
                                VouDrCrList[getVouIndex].cr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_sp) + parseFloat(cr_amt)).toFixed(cmn_ValDecDigit);
                                VouDrCrList[getVouIndex].cr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_base) + parseFloat(cr_amt_bs)).toFixed(cmn_ValDecDigit);
                                VouDrCrList[getVouIndex].vou_entries = VouDrCrList[getVouIndex].vou_entries + 1;
                            } else {
                                VouDrCrList.push({
                                    vou_sr_no: vou_sr_no
                                    , dr_in_sp: dr_amt, dr_in_base: dr_amt_bs
                                    , cr_in_sp: cr_amt, cr_in_base: cr_amt_bs, vou_entries: 1, all_previous_entries: index
                                });
                            }
                        });
    
                    console.log("AddRoundOffGL");
                    debugger
                    var CountEntries = 0;
                    VouDrCrList.map((item, index) => {

                        if (Math.abs(parseFloat(item.cr_in_base) - parseFloat(item.dr_in_base)) < 1) {
                            if (parseFloat(item.dr_in_base) != parseFloat(item.cr_in_base)) {
                                if (parseFloat(item.dr_in_base) < parseFloat(item.cr_in_base)) {
                                    var Diff = parseFloat(item.cr_in_sp) - parseFloat(item.dr_in_sp);
                                    var DiffInBase = parseFloat(item.cr_in_base) - parseFloat(item.dr_in_base);

                                    var rowIdx = $('#VoucherDetail tbody tr').length;
                                    for (var j = 0; j < arr.Table.length; j++) {
                                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                            var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                            GlSrNo = SrRows[index].textContent;
                                            let spanRowCount = 1;
                                            $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                                var row = $(this);
                                                if (row.text() == item.vou_sr_no) {
                                                    spanRowCount++;
                                                }
                                            });

                                            GlTableRenderHtml(item.vou_sr_no, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                                , Diff, DiffInBase, 0, 0, "PV", $("#Hdn_ddlCurrency").val()
                                                , $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())

                                            var vouRow = $('#VoucherDetail > tbody > #tr_GlRow' + item.vou_sr_no);
                                            vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                            var table = document.getElementById("VoucherDetail");
                                            rows = table.tBodies[0].rows;
                                            let ib = item.all_previous_entries + item.vou_entries + CountEntries;
                                            CountEntries = CountEntries + 1;
                                            let LastRow = $('#VoucherDetail tbody tr').length;
                                            rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                        }
                                    }
                                }
                                else {
                                    var Diff = parseFloat(item.dr_in_sp) - parseFloat(item.cr_in_sp);
                                    var DiffInBase = parseFloat(item.dr_in_base) - parseFloat(item.cr_in_base);
                                    var rowIdx = $('#VoucherDetail tbody tr').length;
                                    for (var j = 0; j < arr.Table.length; j++) {
                                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                            var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                            GlSrNo = SrRows[index].textContent;
                                            let spanRowCount = 1;
                                            $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                                var row = $(this);
                                                if (row.text() == item.vou_sr_no) {
                                                    spanRowCount++;
                                                }
                                            });
                                            GlTableRenderHtml(item.vou_sr_no, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                                , 0, 0, Diff, DiffInBase, "PV"
                                                , $("#Hdn_ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val()
                                                , $("#Bill_Date").val())
                                            var vouRow = $('#VoucherDetail tbody #tr_GlRow' + item.vou_sr_no);
                                            vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                            var table = document.getElementById("VoucherDetail");
                                            rows = table.tBodies[0].rows;
                                            let ib = item.all_previous_entries + item.vou_entries + CountEntries;
                                            CountEntries = CountEntries + 1;
                                            let LastRow = $('#VoucherDetail tbody tr').length;
                                            rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                        }
                                    }
                                }
                            }
                        }

                    });
    //-------------
                    //
                    //if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                    //    if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                    //        var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                    //        var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                    //        var rowIdx = $('#VoucherDetail tbody tr').length;
                    //        for (var j = 0; j < arr.Table.length; j++) {
                    //            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                    //                var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                    //                GlSrNo = SrRows[SrRows.length - 1].textContent;
                    //                let spanRowCount = 1;
                    //                $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                    //                    var row = $(this);
                    //                    if (row.text() == 1) {
                    //                        spanRowCount++;
                    //                    }
                    //                });

                    //                GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                    //                    , Diff, DiffInBase, 0, 0, "PV", $("#Hdn_ddlCurrency").val()
                    //                    , $("#conv_rate").val()
                    //                    , $("#Bill_No").val(), $("#Bill_Date").val())

                    //                var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                    //                vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                    //                var table = document.getElementById("VoucherDetail");
                    //                rows = table.tBodies[0].rows;
                    //                let ib = spanRowCount - 1;
                    //                let LastRow = $('#VoucherDetail tbody tr').length;
                    //                rows[ib].parentNode.insertBefore(rows[LastRow-1], rows[ib]);
                    //            }
                    //        }
                    //    }
                    //    else {
                    //        var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                    //        var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
                    //        var rowIdx = $('#VoucherDetail tbody tr').length;
                    //        for (var j = 0; j < arr.Table.length; j++) {
                    //            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                    //                var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                    //                GlSrNo = SrRows[SrRows.length - 1].textContent;
                    //                let spanRowCount = 1;
                    //                $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                    //                    var row = $(this);
                    //                    if (row.text() == 1) {
                    //                        spanRowCount++;
                    //                    }
                    //                });
                    //                GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                    //                    , 0, 0, Diff, DiffInBase, "PV"
                    //                    , $("#Hdn_ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val()
                    //                    , $("#Bill_Date").val())
                    //                var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                    //                vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                    //                vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                    //                var table = document.getElementById("VoucherDetail");
                    //                rows = table.tBodies[0].rows;
                    //                let ib = spanRowCount - 1;
                    //                let LastRow = $('#VoucherDetail tbody tr').length;
                    //                rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                    //            }
                    //        }
                    //    }
                        
                    //}
                    
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Nurration").val() : VouType == "BP" ? $("#hdn_BP_Nurration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
//--------------------------------------//

//---------------Tax---------------------//
//function OnClickTaxCalculationBtn(e) {
//    
//    var ItemListName = "#ServiceNameServiceName";
//    var SNohiddenfiled = "#SNohiddenfiled";
//    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, ItemListName);
//}
function OnClickTaxCalBtn(e) {
    
    var SOServiceName = "#ServiceName";
    var SNohiddenfiled = "PI";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOServiceName)
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").css("display", "Block");
            }
            else {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").css("display", "none");
            }
        }
    }
}
function OnClickSaveAndExit() {
    
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //
    let NewArr = [];
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    if (("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            } else {
                if (TaxItemID == TaxItmCode && DocNo == GRNNo && DocDate == GRNDate) {
                    $(this).remove();
                }
                else {
                    var TaxName = currentRow.find("#TaxName").text();
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxAmount = currentRow.find("#TaxAmount").text();
                    var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                    var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                    var TaxRecov = currentRow.find("#TaxRecov").text();
                    var TaxAccId = currentRow.find("#TaxAccId").text();
                    //Added by Suraj on 08-08-2024
                    var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted == false) {
                        NewArr.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
                    }

                }
            }

        });

        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            var TaxAccId = currentRow.find("#AccID").text();
            
            $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccId}</td>
                </tr>`);
            //Added by Suraj on 08-08-2024
            var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
            }
        });
    }
    else {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            var TaxAccId = currentRow.find("#AccID").text();

            
            $("#" + HdnTaxCalculateTable + " tbody").append(`<tr id="R${++rowIdx}">
           
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccId}</td>
                </tr>`);
            //Added by Suraj on 08-08-2024
            var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
            }
        });
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
 
    if (TaxOn == "OC") {
        debugger;
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // 
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit);
                Cmn_click_Oc_chkroundoff(null, currentRow);
                //currentRow.find("#OCTotalTaxAmt").text(Total);
                var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
            }
        });
        Calculate_OCAmount();
    } else {
        $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
             
            var currentRow = $(this);

            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (ItmCode == TaxItmCode ) {
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
            }
        });
        CalculateAmount();
        
        
    }
    if ($("#taxTemplate").text() == "GST Slab") {
        GetAllGLID();
    }
    //GetAllGLID();
}
//function OnClickSaveAndExit() {
//    var ConvRate = $("#conv_rate").val();
//    var DecDigit = $("#ValDigit").text();
//    var TotalTaxAmount = $('#TotalTaxAmount').text();
//    var RowSNo = $("#HiddenRowSNo").val();
//    var TaxItmCode = $("#TaxCalcItemCode").val();
//    var TaxAmt = parseFloat(0).toFixed(DecDigit);
//    var AssessableValue = parseFloat(0).toFixed(DecDigit);
//    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
//    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
//    var UserID = $("#UserID").text();

//    
//    let NewArr = [];
//    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
//    if (FTaxDetails > 0) {
//        //for (i = 0; i < FTaxDetails.length; i++) {
//        //    var TaxUserID = FTaxDetails[i].UserID;
//        //    var TaxRowID = FTaxDetails[i].RowSNo;
//        //    var TaxItemID = FTaxDetails[i].TaxItmCode;
//        //    if (TaxUserID == UserID && TaxRowID == RowSNo && TaxItemID == TaxItmCode) {
//        //    }
//        //    else {
//        //        NewArr.push(FTaxDetails[i]);
//        //    }
//        //}
//        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
//            var currentRow = $(this);
//            //var TaxUserID = currentRow.find("#UserID").text();
//            //var TaxRowID = currentRow.find("#RowSNo").text();
//            var TaxItemID = currentRow.find("#TaxItmCode").text();
//            var TaxName = currentRow.find("#TaxName").text();
//            var TaxNameID = currentRow.find("#TaxNameID").text();
//            var TaxPercentage = currentRow.find("#TaxPercentage").text();
//            var TaxLevel = currentRow.find("#TaxLevel").text();
//            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
//            var TaxAmount = currentRow.find("#TaxAmount").text();
//            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
//            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == TaxItmCode) {
//                currentRow.remove();
//            }
//            else {

//                NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
//            }
//        });
//        $("#TaxCalculatorTbl >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var TaxName = currentRow.find("#taxname").text();
//            var TaxNameID = currentRow.find("#taxid").text();
//            var TaxPercentage = currentRow.find("#taxrate").text();
//            var TaxLevel = currentRow.find("#taxlevel").text();
//            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
//            var TaxAmount = currentRow.find("#taxval").text();
//            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
//            $("#Hdn_TaxCalculatorTbl > tbody").append(`
//                                <tr>
                                    
//                                    <td id="TaxItmCode">${TaxItmCode}</td>
//                                    <td id="TaxName">${TaxName}</td>
//                                    <td id="TaxNameID">${TaxNameID}</td>
//                                    <td id="TaxPercentage">${TaxPercentage}</td>
//                                    <td id="TaxLevel">${TaxLevel}</td>
//                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
//                                    <td id="TaxAmount">${TaxAmount}</td>
//                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
//                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
//                                </tr>`)
//            //TotalTaxAmount = parseFloat(TotalTaxAmount)+parseFloat(TaxAmount);
//            NewArr.push({ /*UserID: UserID, RowSNo: RowSNo, */TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
//        });
//        //sessionStorage.removeItem("TaxCalcDetails");
//        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(NewArr));
//        BindTaxAmountDeatils(NewArr);
//    }
//    else {
//        var TaxCalculationList = [];
//        $("#TaxCalculatorTbl >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var TaxName = currentRow.find("#taxname").text();
//            var TaxNameID = currentRow.find("#taxid").text();
//            var TaxPercentage = currentRow.find("#taxrate").text();
//            var TaxLevel = currentRow.find("#taxlevel").text();
//            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
//            var TaxAmount = currentRow.find("#taxval").text();
//            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
//            $("#Hdn_TaxCalculatorTbl > tbody").append(`
// <tr>
                                    
//                                    <td id="TaxItmCode">${TaxItmCode}</td>
//                                    <td id="TaxName">${TaxName}</td>
//                                    <td id="TaxNameID">${TaxNameID}</td>
//                                    <td id="TaxPercentage">${TaxPercentage}</td>
//                                    <td id="TaxLevel">${TaxLevel}</td>
//                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
//                                    <td id="TaxAmount">${TaxAmount}</td>
//                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
//                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
//                                </tr>
//`)
//            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
//        });
//        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
//        BindTaxAmountDeatils(TaxCalculationList);
//    }

//    //$("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
//    //         
//    //        var currentRow = $(this);
//    //        //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
//    //        //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
//    //        var ItmCode = currentRow.find("#hfItemID").val();
//    //        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
//    //        if (TaxAmt == "" || TaxAmt == "NaN") {
//    //            TaxAmt = (parseFloat(0)).toFixed(DecDigit);
//    //        }
//    //        OC_Amt = (parseFloat(0)).toFixed(DecDigit);
//    //        if (ItmCode == TaxItmCode /*&& GRNNoTbl == GRNNo && GRNDateTbl == GRNDate*/) {
//    //            currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
//    //            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
//    //                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
//    //            }
//    //            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
//    //            NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
//    //            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
//    //           /* currentRow.find("#TxtNetValue").val(NetOrderValueInBase);*/
//    //            FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
//    //            currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
//    //        }
//    //    });

//    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
//        //
//        var currentRow = $(this);
//        if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {

//            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
//            if (TaxAmt == "" || TaxAmt == "NaN") {
//                TaxAmt = parseFloat(0).toFixed(DecDigit);
//            }
//            currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
//            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
//            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
//                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
//            }
//            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
//            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
//            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
//          /*  currentRow.find("#item_net_val_sp").val(NetOrderValueSpec);*/
//            FinalNetOrderValueBase = ConvRate * NetOrderValueBase
//            currentRow.find("#NetValueinBase").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

//        }
//    });
//    CalculateAmount();
//}
function OnClickReplicateOnAllItems() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        var TaxRecov = currentRow.find("#tax_recov").text();
        var TaxAccId = currentRow.find("#AccID").text();
        TaxCalculationList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov,TaxAccId:TaxAccId })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "ServicePIItemTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = currentRow.find("#TxtGrnNo").val();
                GRNDateTbl = currentRow.find("#hfGRNDate").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                } else {
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#TxtItemGrossValue").val();
                }


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                for (i = 0; i < TaxCalculationList.length; i++) {
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationList[i].TaxName;
                    var TaxNameID = TaxCalculationList[i].TaxNameID;
                    var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                    TaxPercentage = TaxCalculationList[i].TaxPercentage;
                    var TaxLevel = TaxCalculationList[i].TaxLevel;
                    var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                    var TaxRecov = TaxCalculationList[i].TaxRecov;
                    var TaxAccId = TaxCalculationList[i].TaxAccId;
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    NewArray.push({ /*UserID: UserID,*/ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            var DocNo = NewArray[k].DocNo;
                            var DocDate = NewArray[k].DocDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            var TaxRecov = NewArray[k].TaxRecov;
                            var TaxAccId = NewArray[k].TaxAccId;
                            if (CitmTaxItmCode != TaxItmCode && GRNNo == DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov,TaxAccId:TaxAccId })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov, TaxAccId: TaxAccId })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov,TaxAccId:TaxAccId })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov,TaxAccId:TaxAccId })
                            }
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
    //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
    
    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        
        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            
            <td id="DocNo">${TaxCalculationListFinalList[i].DocNo}</td>
            <td id="DocDate">${TaxCalculationListFinalList[i].DocDate}</td>
            <td id="TaxItmCode">${TaxCalculationListFinalList[i].TaxItmCode}</td>
            <td id="TaxName">${TaxCalculationListFinalList[i].TaxName}</td>
            <td id="TaxNameID">${TaxCalculationListFinalList[i].TaxNameID}</td>
            <td id="TaxPercentage">${TaxCalculationListFinalList[i].TaxPercentage}</td>
            <td id="TaxLevel">${TaxCalculationListFinalList[i].TaxLevel}</td>
            <td id="TaxApplyOn">${TaxCalculationListFinalList[i].TaxApplyOn}</td>
            <td id="TaxAmount">${TaxCalculationListFinalList[i].TaxAmount}</td>
            <td id="TotalTaxAmount">${TaxCalculationListFinalList[i].TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxCalculationListFinalList[i].TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxCalculationListFinalList[i].TaxRecov}</td>
            <td id="TaxAccId">${TaxCalculationListFinalList[i].TaxAccId}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    //
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //
            var OCValue = currentRow.find("#OCValue").text();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
                        if (OCValue == TaxItmCode) {
                            currentRow.find("#OCTaxAmt").text(TotalTaxAmtF);
                            var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(DecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(DecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    } else {
        $("#ServicePIItemTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            // 
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID && GRNNoTbl == AGRNNo && GRNDateTbl == AGRNDate) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#Txtitem_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                            NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                           /* currentRow.find("#TxtNetValue").val(NetOrderValueInBase);*/
                            FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                            currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                    currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                    /*currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                    FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                    currentRow.find("#NetValueinBase").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
               /* currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                currentRow.find("#NetValueinBase").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
        GetAllGLID();
    }

}
function CheckedCancelled() {
    
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertSPIDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    

    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/

    //CMNBindTaxAmountDeatils(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);//commented by suraj on 21-05-2024 for using New common function */
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}
function AfterDeleteResetPI_ItemTaxDetail() {
    //
    var ServicePIItemTbl = "#ServicePIItemTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ServiceName = "#ServiceName";
    CMNAfterDeleteReset_ItemTaxDetailModel(ServicePIItemTbl, SNohiddenfiled, ServiceName);
}

function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//---------------------------------------------//

function AddNewRow() {
    var rowIdx = 0;
    //
    /*var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#ServicePIItemTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
        //
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }

    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    AddNewRowForEditPOItem();

    BindPOItmList(RowNo);

};
function AddNewRowForEditPOItem() {
    var rowIdx = 0;
    
    var docid = $("#DocumentMenuId").val();

    var rowCount = $('#ServicePIItemTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#ServicePIItemTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    var CountRows = RowNo;
    RowNo = "";
    var deletetext = $("#Span_Delete_Title").text();

    var ManualGst = "";
    var TaxExempted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
        ManualGst +=`<td class="qt_to">
                         <div class="custom-control custom-switch sample_issue">
                             <input type="checkbox" checked class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                             <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                         </div>
                     </td>`
    }
    TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'


    $('#ServicePIItemTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
<td id="srno" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" />
<input class="" type="hidden" id="hfItemID" />
<input class="" type="hidden" id="hdn_item_gl_acc" />
<input id="ItemtaxTmplt" type="hidden" /></td>
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form no-padding"><select class="form-control" id="ServiceName${CountRows}" name="ServiceName${CountRows}" onchange="OnChangePOItemName(${CountRows},event)" ></select><span id="ServiceNameError" class="error-message is-visible"></span></div></td>
<td><input id="HsnNo${RowNo}" class="form-control date" autocomplete="off" type="text" name="Hsn"  placeholder="${$("#Hsn").text()}" disabled></td>
<td><div class="lpo_form"><input id="TxtInvoiceQuantity${RowNo}" class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="TxtInvoiceQuantity"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="TxtInvoiceQuantityError${RowNo}" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="TxtRate${RowNo}" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(1,event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="TxtRate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="TxtRateError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="TxtItemGrossValue${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="TxtItemGrossValue"  placeholder="0000.00"  disabled></td>
 `+ TaxExempted + `
 `+ ManualGst + `
<td><div class="col-sm-10 no-padding"><input id="Txtitem_tax_amt${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div><div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation${RowNo}" disabled="disabled" onclick="OnClickTaxCalBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${$("#TaxInfo").text()}" data-original-title="${$("#TaxInfo").text()}"></i></button></div></td>
<td><input id="TxtOtherCharge${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="TxtOtherCharge"  placeholder="0000.00"  disabled></td>
<td><input id="NetValueinBase${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="NetValueinBase"  placeholder="0000.00"  disabled></td>
<td>
  <textarea id="SPIItemRemarks${RowNo}" class="form-control remarksmessage" onmouseover="OnMouseOver(this)" autocomplete="off" name="ItemRemarks"  maxlength="300" placeholder="${$("#span_remarks").text()}"></textarea>
 </td>
</tr>`);
    $("#ServiceName" + CountRows);//.focus()
};
function BindPOItmList(ID) {
    //
    BindItemList("#ServiceName", ID, "#ServicePIItemTbl", "#SNohiddenfiled", "", "SPO");
}
function OnChangePOItemName(RowID, e) {
    //
    BindPOItemList(e);
}
function BindPOItemList(e) {
    
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    Itm_ID = clickedrow.find("#ServiceName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {
        clickedrow.find("#ServiceNameError").text($("#valueReq").text());
        clickedrow.find("#ServiceNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#ServiceNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField();
    //GetAllGLID();
    try {
        $("#HdnTaxOn").val("Tax");
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");

    } catch (err) {
    }
   
}
function OnClickTaxExemptedCheckBox(e) {
    
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    if ($("#SourceTypeV").is(":checked")) {
        var ItmCode = currentrow.find("#hfItemID").val();
    }
    else {
        var ItmCode = currentrow.find("#ServiceName" + RowSNo).val();
    }
    var AssAmount = currentrow.find("#TxtItemGrossValue").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "ServicePIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    
    var currentrow = $(e.target).closest('tr');
    //var RowSNo = currentrow.find("#SNohiddenfiled").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        let zero = parseFloat(0).toFixed($("#ValDigit").text());
        currentrow.find("#Txtitem_tax_amt").val(zero);
        var itemId = currentrow.find("#hfItemID").val();
        currentrow.find("#TxtRate").trigger('change');
       
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "ServicePIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue")
        CalculationBaseRate(e)
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
function ClearRowDetails(e, ItemID) {
    
    //var docid = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    //var SNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#HsnNo").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#TxtInvoiceQuantity").val("");
    clickedrow.find("#TxtRate").val("");
    clickedrow.find("#TxtItemGrossValue").val("");
    clickedrow.find("#Txtitem_tax_amt").val("");
    clickedrow.find("#TxtOtherCharge").val("");
    clickedrow.find("#NetValueinBase").val("");

    CalculateAmount();
    //var TOCAmount = parseFloat($("#TxtOtherCharges").val());//Commented by Suraj Maurya on 06-01-2025
    var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetPI_ItemTaxDetail();
   
}
function DisableHeaderField() {
    //
    $("#src_type").attr('disabled', true);
    $("#SupplierName").attr('disabled', true);
    //$("#ddlCurrency").attr('disabled', true);
    //$("#conv_rate").prop("readonly", true);
}
function OnChangePOItemQty(RowID, e) {
    //
    CalculationBaseQty(e);
    GetAllGLID();
}
function OnChangePOItemRate(RowID, e) {
    //
    CalculationBaseRate(e);
    GetAllGLID();
}

function CalculationBaseQty(e) {
    
    var ConvRate = $("#conv_rate").val();
    var docid = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;

    OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    ItemName = clickedrow.find("#ServiceName" + Sno).val();
    ItmRate = clickedrow.find("#TxtRate").val();


    if (ItemName == "0") {
        clickedrow.find("#ServiceNameError").text($("#valueReq").text());
        clickedrow.find("#ServiceNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#TxtInvoiceQuantity").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ServiceNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (OrderQty != "" && OrderQty != ".") {
        OrderQty = parseFloat(OrderQty);
    }
    if (OrderQty == ".") {
        OrderQty = 0;
    }
    if (OrderQty == "" || OrderQty == 0 || ItemName == "0") {
    //    clickedrow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
    //    clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
    //    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
        clickedrow.find("#TxtInvoiceQuantity").val("");
    //    clickedrow.find("#TxtInvoiceQuantity");//.focus();
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtInvoiceQuantityError").text("");
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    }

    var OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    var ItmRate = clickedrow.find("#TxtRate").val();

    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#TxtInvoiceQuantity").val("");
        OrderQty = 0;
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#TxtItemGrossValue").val(FinVal);   
        FinalFinVal = ConvRate * FinVal
        clickedrow.find("#NetValueinBase").val(parseFloat(FinalFinVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    clickedrow.find("#TxtInvoiceQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
        // CalculateDisPercent(e);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {

    }
    OnChangeGrossAmt();

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    var assVal = clickedrow.find("#TxtItemGrossValue").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
           clickedrow.find("#BtnTxtCalculation").prop("disabled", false);        
        clickedrow.find("#TxtItemGrossValueError").css("display", "none");
        clickedrow.find("#TxtItemGrossValue").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    
}
function CalculationBaseRate(e) {
    
    var ConvRate = $("#conv_rate").val();
    var docid = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;   
    OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    ItemName = clickedrow.find("#hfItemID").val();
    ItmRate = clickedrow.find("#TxtRate").val();

    if (ItemName == "0") {
        clickedrow.find("#ServiceNameError").text($("#valueReq").text());
        clickedrow.find("#ServiceNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#TxtRate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ServiceNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
    //    clickedrow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
    //    clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
    //    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
        clickedrow.find("#TxtInvoiceQuantity").val("");
        clickedrow.find("#TxtInvoiceQuantity");//.focus();
       clickedrow.find("#TxtRate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {
        clickedrow.find("#TxtRateError").text($("#valueReq").text());
        clickedrow.find("#TxtRateError").css("display", "block");
        clickedrow.find("#TxtRate").css("border-color", "red");
        clickedrow.find("#TxtRate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#TxtRate");//.focus();
        }
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtRateError").css("display", "none");
        clickedrow.find("#TxtRate").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#TxtRate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinVal);       
        FinalVal = FinVal * ConvRate
        clickedrow.find("#NetValueinBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
        // CalculateDisPercent(e);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {

    }
    clickedrow.find("#TxtRate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            //clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
            //clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        }
    }
    //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    var assVal = clickedrow.find("#TxtItemGrossValue").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);        
        clickedrow.find("#TxtItemGrossValueError").css("display", "none");
        clickedrow.find("#TxtItemGrossValue").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }

}
function OnChangeGrossAmt() {
    //
    /*var TotalOCAmt = $('#Total_OC_Amount').text();*/
    //var TotalOCAmt = $('#_OtherChargeTotalAmt').text();// Commented by Suraj Maurya on 03-01-2024
    var TotalOCAmt = $('#TxtDocSuppOtherCharges').val();// Added by Suraj Maurya on 03-01-2024
    var Total_PO_OCAmt = $('#TxtOtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
        //if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) { // Commented by Suraj Maurya on 03-01-2024
            Calculate_OC_AmountItemWise(TotalOCAmt);
        //}
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                
                var currentRow = $(this);                
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                var ItemRow = $("#ServicePIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                
                if (TaxItemID == ItmCode) {
                    if (TaxExempted) {
                        AssessVal = 0;
                    }
                    var GstApplicable = $("#Hdn_GstApplicable").text();
                    if (GstApplicable == "Y") {
                       
                        var ManualGST = ItemRow.find("#ManualGST").is(":checked");
                        var item_tax_amt = ItemRow.find("#Txtitem_tax_amt").val();
                        if (ManualGST && parseFloat(CheckNullNumber(item_tax_amt))==0) {
                            AssessVal = 0;
                        }
                    }
                    
                   
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxPercentage = "";
                    TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    currentRow.find("#TaxAmount ").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount ").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                var TaxRecov = currentRow.find("#TaxRecov").text();
                var TaxAccId = currentRow.find("#TaxAccId").text();
                if (TaxExempted == false) {
                    NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId });
                }
            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr').each(function () {
                var currentRow = $(this);               
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if ( TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });

            $("#ServicePIItemTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").closest('tr').each(function () {
                
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#hfItemID").val();

                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr');
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                                NewArray = NewArray.filter(v => v.TaxItmCode != ItemNo);
                            }
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit);
                                    NewArray = NewArray.filter(v => v.TaxItmCode != ItemNo);
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {                             
                                currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(DecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);                              
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#NetValueinBase").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

                            }
                        });                       
                    }
                    else {
                        
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit); 
                        FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        currentRow.find("#NetValueinBase").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
            CalculateAmount();        
            BindTaxAmountDeatils(NewArray);
        }
    }
}

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var GLAcc_Name = clickedrow.find("#txthfAccID").val();
    var GLAcc_id = clickedrow.find("#hfAccID").val();
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ServicePurchaseInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}

//-------------------Cost Center Section End-------------------//

/*----------------------------TDS Section-----------------------------*/
function OnClickTDSCalculationBtn() {
    //modified by Suraj Maurya on 06-12-2024 to pass ToTdsAmtWithTxAndOc
    const GrVal = $("#TxtGrossValue").val();
    //const OCVal = $("#TxtDocSuppOtherCharges").val();
    //const TaxVal = $("#TxtTaxAmount").val();
    const ToTdsAmtWithTxAndOc = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));// + parseFloat(CheckNullNumber(OCVal)) + parseFloat(CheckNullNumber(TaxVal));
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("D");
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, null, null, ToTdsAmtWithTxAndOc,"TDS");

}
function OnClickTDS_SaveAndExit() {
    debugger;

    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {

        var DecDigit = $("#ValDigit").text();
        //var TotalTDS_Amount = $('#TotalTDS_Amount').text();
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        //let NewArr = [];
        var rowIdx = 0;
        //var GstCat = $("#Hd_GstCat").val();
        //var ToTdsAmt = 0;
        //if (GstCat == "UR") {
        //    ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val())) + parseFloat(CheckNullNumber($("#TxtOtherCharges").val()));

        //} else {
        //    ToTdsAmt = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        //}
        //ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));//commented by Suraj maurya on 06-12-2024

        //Added by Suraj Maurya on 06-12-2024
        var ToTdsAmt = 0;
        let TdsAssVal_applyOn = "ET";
        //ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
            //ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
        }
        else if ($("#TdsAssVal_Custom").is(":checked")) {/* Added by Suraj Maurya on 19-07-2025 */
            TdsAssVal_applyOn = "CA"
        }
        //Added by Suraj Maurya on 06-12-2024 End

        if ($("#Hdn_TDS_CalculatorTbl >tbody >tr").length > 0) {

            $("#Hdn_TDS_CalculatorTbl >tbody >tr").remove();
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);

                //NewArr.push({ /*UserID: UserID, */DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
            });
        }
        else {
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $("#Hdn_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);

                //NewArr.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
            });

        }
        var TotalAMount = parseFloat(0).toFixed(DecDigit);

        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
        });

        $("#TxtTDS_Amount").val(TotalAMount);
        /*Added by Surbhi on 11/09/2025 for Custome TDS Clear*/
        if (($("#TDS_CalculatorTbl > tbody > tr").length > 0) && (TdsAssVal_applyOn == "CA") && $("#TxtTDS_Amount").val() == 0) {
            debugger;
            if ($("#TDS_AssessableValueCustom").val() === "0" || $("#TDS_AssessableValueCustom").val() === "0.00" || $("#TDS_AssessableValueCustom").val() == "") {
                $('#SpanTDS_AssessableValueCustomErrorMsg').text($("#valueReq").text());
                $("#TDS_AssessableValueCustom").css("border-color", "Red");
                $("#SpanTDS_AssessableValueCustomErrorMsg").css("display", "block");
                return;
            }
            else {
                $("#SpanTDS_AssessableValueCustomErrorMsg").css("display", "none");
                $("TDS_AssessableValueCustom").css("border-color", "#ced4da");
            }

        }
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        //$("#PopUp_TDSCalculate").html("");
        GetAllGLID();
    }

}

/*----------------------------TDS Section End-----------------------------*/


function approveonclick() { /**Added this Condition by Nitesh 15-01-2024 for Disable Approve btn after one Click**/
    
    var btn = $("#hdnsavebtn").val();
    var VoucherNarr = $("#PurchaseVoucherRaisedAgainstInv").text();
    $('#hdNarration').val(VoucherNarr);
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}

/***-------------------Roundoff------------------------------------***/
function checkMultiSupplier() {
    //let countSupp = 0;
    //$("#VoucherDetail tbody tr").each(function () {
    //    let row = $(this);
    //    if (row.find("#type").val() == "Supp") {
    //        countSupp++;
    //    }
    //});
    //if (countSupp > 1) {
    //    $("#chk_roundoff").attr("disabled", true);
    //    return false;
    //} else {
    //    return true;
    //}
    return true;
}
function click_chkroundoff() {
    
    if (checkMultiSupplier() == true) {
        if ($("#chk_roundoff").is(":checked")) {
            $("#div_pchkbox").show();
            $("#div_mchkbox").show();
            $("#pm_flagval").val("");
            $("#p_round").prop('checked', false);
            $("#m_round").prop('checked', false);
        }
        else {
            $("#div_pchkbox").hide();
            $("#div_mchkbox").hide();
            $("#pm_flagval").val("");
            $("#p_round").prop('checked', false);
            $("#m_round").prop('checked', false);

            //$("#ServicePIItemTbl > tbody > tr").each(function () {
            //    
            //    var currentrow = $(this);
            //    CalculateTaxExemptedAmt(currentrow, "N")
            //});
            CalculateAmount();
            GetAllGLID();
        }
    } else {
        //for multi supplier
        if ($("#chk_roundoff").is(":checked")) {
            swal("", $("#ManualRoundOffIsNotApplicableForGLHavingMultipleSuppliers").text(), "warning")
            $("#chk_roundoff").attr("checked", false);
        }
    }

}
function click_chkplusminus() {
   
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    var ValDecDigit = $("#ValDigit").text();
    if ($("#chk_roundoff").is(":checked")) {
        try {
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                $.ajax(
                    {
                        type: "POST",
                        url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                        data: {},
                        success: function (data) {
                            
                            if (data == 'ErrorPage') {
                                PO_ErrorPage();
                                return false;
                            }
                            if (data !== null && data !== "") {
                                var arr = [];
                                arr = JSON.parse(data);
                                if (arr.length > 0) {
                                    if (parseInt(arr[0]["r_acc"]) > 0) {

                                        ApplyRoundOff();
                                        //var grossval = $("#TxtGrossValue").val();
                                        //var taxval = $("#TxtTaxAmount").val();
                                        ////var ocval = CheckNullNumber($("#TxtOtherCharges").val());
                                        //var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

                                        //var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

                                        //var netval = finalnetval;//$("#NetOrderValueInBase").val();
                                        //var fnetval = 0;

                                        //if (parseFloat(netval) > 0) {
                                        //    var finnetval = netval.split('.');
                                        //    if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                                        //        if ($("#p_round").is(":checked")) {
                                        //            var decval = '0.' + finnetval[1];
                                        //            var faddval = 1 - parseFloat(decval);
                                        //            fnetval = parseFloat(netval) + parseFloat(faddval);
                                        //            $("#pm_flagval").val($("#p_round").val());

                                        //            //let valInBase = $("#TxtGrossValue").val();
                                        //            //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                                        //        }
                                        //        if ($("#m_round").is(":checked")) {
                                        //            //var finnetval = netval.split('.');
                                        //            var decval = '0.' + finnetval[1];
                                        //            fnetval = parseFloat(netval) - parseFloat(decval);
                                        //            $("#pm_flagval").val($("#m_round").val());

                                        //            //let valInBase = $("#TxtGrossValue").val();
                                        //            //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                                        //        }
                                        //        if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                                        //            var roundoff_netval = Math.round(fnetval);
                                        //            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                        //            $("#NetOrderValueInBase").val(f_netval);
                                        //            //$("#NetOrderValueSpe").val(f_netval);
                                        //            GetAllGLID("RO");
                                        //        }

                                        //    }
                                        //}
                                    }
                                    else {
                                        swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                        $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                        return false;
                                    }
                                }
                                else {
                                    swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                    $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                    return false;
                                }
                            }
                            else {
                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                return false;
                            }
                        },
                    });
            }
          
        } catch (err) {
            console.log("Purchase invoice round off Error : " + err.message);
        }

    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);

        //$("#ServicePIItemTbl > tbody > tr").each(function () {
        //    
        //    var currentrow = $(this);
        //    CalculateTaxExemptedAmt(currentrow, "N")
        //});
        CalculateAmount();
        GetAllGLID();
    }
}
//Created by Suraj on 08-08-2024 to Reuse Roundoff functionality
function ApplyRoundOff(flag) {
    var ValDecDigit = $("#ValDigit").text();
    var grossval = $("#TxtGrossValue").val();
    var taxval = $("#TxtTaxAmount").val();
    //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

    var netval = finalnetval;//$("#NetOrderValueInBase").val();
    var fnetval = 0;

    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if ($("#p_round").is(":checked")) {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
                $("#pm_flagval").val($("#p_round").val());

                //let valInBase = $("#TxtGrossValue").val();
                //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
            }
            if ($("#m_round").is(":checked")) {
                //var finnetval = netval.split('.');
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
                $("#pm_flagval").val($("#m_round").val());

                //let valInBase = $("#TxtGrossValue").val();
                //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
            }
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                $("#NetOrderValueInBase").val(f_netval);
                //$("#NetOrderValueSpe").val(f_netval);
                if (flag == "CallByGetAllGL") {
                    //do not call  GetAllGLID("RO");
                } else {
                    GetAllGLID("RO");
                }
                
            }

        }
    }

}

function To_RoundOff(Amount,type) {

    var netval = Amount.toString();
    var fnetval = 0;

    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if (type=="P") {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
            }
            if (type == "M") {
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
            }
            if (type == "P" || type=="M") {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(cmn_ValDecDigit);
                return f_netval;
            }   
        }
    }
    return Amount;
}

/***----------------------end-------------------------------------***/
/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/

/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row; 
function OnClickTP_TDSCalculationBtn(e) {
    const row = $(e.target).closest("tr");
    CC_Clicked_Row = row;
    const OcVal = row.find("#OCAmount").text();
    const OcValWithTax = row.find("#OCTotalTaxAmt").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(OcVal));
    $("#hdn_tds_on").val("OC");
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, CheckNullNumber(OcValWithTax),"OcTds");

}
function OnClickTP_TDS_SaveAndExit() {
    //New Column <td id="td_TDS_AssValApplyOn"> is Added by Suraj Maurya on 05-12-2024
    var TotalTDS_Amount = $('#TotalTDS_Amount').text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //Added by Suraj Maurya on 05-12-2024
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
    }
    //Added by Suraj Maurya on 05-12-2024 End

    //var TDS_OcId = $("#TDS_oc_id").text();
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    //var TDS_SuppId = $("#TDS_supp_id").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var GstCat = $("#Hd_GstCat").val();
    
    if ($("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").length > 0) {

        $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function () {
            let row = $(this);
            if (row.find("#td_TDS_OC_Id").text() == TDS_OcId) {
                $(this).remove();
            }
        });
        
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();
         
            $('#Hdn_OC_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });
    }
    else {
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();
           
            $("#Hdn_OC_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });
    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
    //GetAllGLID();
}
function SetTds_Amt_To_OC() {
    debugger
    var DecDigit = $("#ValDigit").text();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
    });

    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}

/***------------------------------TDS On Third Party End------------------------------***/

function OC_GlVoucherRoundOff() {
    //Created by Suraj on 19-07-2024 to validate GL Voucher Details.
    try {
       
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }


}

function Fn_AfterTaxChangeUpdateNetValueItemWise(currentRow, total_tax_val) {
    /* Created By : Suraj Maurya, On : 30-11-2024, Purpose : To Update Row Data after tax change */
    if (!currentRow.find("#TaxExempted").is(":checked")) {
        currentRow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(total_tax_val)).toFixed(cmn_ValDecDigit));
        let ItemGrVal = parseFloat(CheckNullNumber(currentRow.find("#TxtItemGrossValue").val()));
        let ItemOcVal = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()));
        let ItemNetVal = ItemGrVal + ItemOcVal + parseFloat(CheckNullNumber(total_tax_val));
        currentRow.find("#NetValueinBase").val(ItemNetVal.toFixed(cmn_ValDecDigit));
    }
    
}
function FilterItemDetail(e) {//added by Prakash Kumar on 26-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ServicePIItemTbl", [{ "FieldId": "ServiceName", "FieldType": "select", "SrNo": "SNohiddenfiled"  }]);
}
//function GetPdfFilePathToSendonEmailAlert(INV_NO, INV_Date, fileName) {
//    debugger;
//    var Inv_no = INV_NO;
//    var InvDate = INV_Date;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/ServicePurchaseInvoice/SavePdfDocToSendOnEmailAlert",
//        data: { Inv_no: Inv_no, InvDate: InvDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
//Added by on 01-09-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var supp_id = $('#SupplierName option:selected').val();
    Cmn_SendEmail(docid, supp_id, 'Supp');
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/ServicePurchaseInvoice/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var pdfAlertEmailFilePath = 'SPI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseInvoice/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath},
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
    }
}
function EmailAlertLogDetails() {
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//End on  01-09-2025
function onchangeCancelledRemarks() {
    debugger;
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
