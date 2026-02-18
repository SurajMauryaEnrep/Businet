/************************************************
Javascript Name:Service Purchase Order Detail
Created By:Mukesh
Created Date: 20-02-2023
Description: This Javascript use for the SPO many function

Modified By:
Modified Date: 
Description:

*************************************************/

$(document).ready(function () {
    BindSuppList();
    BindPOItmList(1);
    // jQuery button click event to remove a row. 
    $('#SPOItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        ////debugger;
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
        //debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#ItemListName" + SNo).val();
        ////var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        // ShowItemListItm(ItemCode);
        CalculateAmount();
        var TOCAmount = parseFloat($("#PO_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetPO_ItemTaxDetail();
        DelDeliSchAfterDelItem(ItemCode);
        BindOtherChargeDeatils();
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });
    Cmn_DeleteDeliverySch();
    DeleteTermCondition();
    SPO_No = $("#SPO_No").val();
    $("#hdDoc_No").val(SPO_No);
    CancelledRemarks("#Cancelled", "Disabled");
});

function SaveBtnClick() {
    ////debugger;
    return InsertSPODetails();
}
function ShowSavedAttatchMentFiles(_LPO_No, _LPO_Date) {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetPOAttatchDetailEdit",
        data: { PO_No: _LPO_No, PO_Date: _LPO_Date },
        success: function (data) {
            debugger;
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
    InsertSPOApproveDetails("", "", "");
}
function DeleteBtnClick() {
    //RemoveSession();
    Delete_PoDetails();
    ResetWF_Level();
}
function OnClickPrintBtn() {
    debugger;
    $("#hd_order_no").val($("#SPO_No").val());
    $("#hd_order_dt").val($("#SPO_Date").val());
    return true;
}
function ForwardBtnClick() {
    debugger;
    try {
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
        return false;

        /**Commented by Hina sharma on 30-05-2025 to add as per discuss with vishal sir */
    ///*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
    //var compId = $("#CompID").text();
    //var brId = $("#BrId").text();
    //var SPODate = $("#SPO_Date").val();
    //$.ajax({
    //    type: "POST",
    //    /* url: "/Common/Common/CheckFinancialYear",*/
    //    url: "/Common/Common/CheckFinancialYearAndPreviousYear",
    //    data: {
    //        compId: compId,
    //        brId: brId,
    //        DocDate: SPODate
    //    },
    //    success: function (data) {
    //        /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
    //        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
    //        if (data == "TransAllow") {
    //            var OrderStatus = "";
    //            OrderStatus = $('#hfStatus').val();
    //            if (OrderStatus === "D" || OrderStatus === "F") {

    //                if ($("#hd_nextlevel").val() === "0") {
    //                    $("#Btn_Forward").attr("data-target", "");
    //                }
    //                else {
    //                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //                    $("#Btn_Approve").attr("data-target", "");
    //                }
    //                var Doc_ID = $("#DocumentMenuId").val();
    //                $("#OKBtn_FW").attr("data-dismiss", "modal");

    //                Cmn_GetForwarderList(Doc_ID);

    //            }
    //            else {
    //                $("#Btn_Forward").attr("data-target", "");
    //                $("#Btn_Forward").attr('onclick', '');
    //                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //            }
    //        }
    //        else {/* to chk Financial year exist or not*/
    //            /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
    //            /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
    //            swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
    //            $("#Btn_Forward").attr("data-target", "");
    //            $("#Btn_Approve").attr("data-target", "");
    //            $("#Forward_Pop").attr("data-target", "");

    //        }
    //    }
    //});
    ///*End to chk Financial year exist or not*/
    //return false;
    }
    catch (ex) {
        console.log(ex);
    }
    


}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#SPO_No").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var PONo = "";
    var PODate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    PONo = $("#SPO_No").val();
    PODate = $("#SPO_Date").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $('#ListFilterData1').val();

    var WF_status1 = $("#WF_status1").val();
    var TrancType = (PONo + ',' + PODate + ',' + "Update" + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = PONo.replace(/\//g, "") +"_"+ GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PONo, PODate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ServicePurchaseOrder/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PONo != "" && PODate != "" && level != "") {
            debugger;

            //var Action_name = "DPODetail,DPO";
            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + PONo + "&docdate=" + PODate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ PONo: PONo, PODate: PODate, A_Status: fwchkval, A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ServicePurchaseOrder/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + " &ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PONo != "" && PODate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && PONo != "" && PODate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ServicePurchaseOrder/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
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
function BindPOItmList(ID) {
    //debugger;
    BindItemList("#ItemListName", ID, "#SPOItmDetailsTbl", "#SNohiddenfiled", "", "SPO");
}
function OnChangeSuppName(SuppID) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Supp_id = SuppID.value;
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SPOItmDetailsTbl .plus_icon1").css("display", "none");
        $("#Address").val("");
        $("#ddlCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
        var src_type = $("#src_type").val();
        if (src_type == "P") {
            $("#SPOItmDetailsTbl >tbody >tr").remove();
            $("#SPOItmDetailsTbl #delBtnIcon1").css("display", "none");
            $("#SPOItmDetailsTbl .plus_icon1").css("display", "none");
            BindSrcDocNumber();
        }
        else {
            $("#SPOItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                currentRow.find("#ItemListName" + Sno).attr("disabled", false);
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
                currentRow.find("#BtnTxtCalculation").attr("disabled", true);
                currentRow.find("#ord_qty").attr("disabled", false);
                currentRow.find("#item_rate").attr("disabled", false);
                currentRow.find("#remarks").attr("disabled", false);
                currentRow.find("#TaxExempted").attr("disabled", false);
                var GstApplicable = $("#Hdn_GstApplicable").text();
                if (GstApplicable == "Y") {
                    currentRow.find("#ManualGST").attr("disabled", false);
                }
                currentRow.find("#ItemListNameError").css("display", "none");
                currentRow.find("#ItemListName" + Sno).css("border-color", "#ced4da");//aria-labelledby="select2-ItemListName1-container"
                currentRow.find("select2-selection select2-selection--single").css("border", "1px solid #aaa");
                currentRow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border-color", "#ced4da");
                currentRow.find("#ord_qtyError").css("display", "none");
                currentRow.find("#ord_qty").css("border-color", "#ced4da");
                currentRow.find("#item_rateError").css("display", "none");
                currentRow.find("#item_rate").css("border-color", "#ced4da");
                $("#SPOItmDetailsTbl #delBtnIcon1").css("display", "block");
                $("#SPOItmDetailsTbl .plus_icon1").css("display", "block");
            })

        }



    } //debugger;
    GetSuppAddress(Supp_id);

}
function GetSuppAddress(Supp_id) {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseOrder/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    debugger;
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
                            $("#Pymnt_terms").val(arr.Table[0].paym_term);/*Add by Hina Sharma on 22-04-2025*/
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            $("#conv_rate").prop("readonly", true);
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

function OnChangePOItemName(RowID, e) {
    //debugger;
    BindPOItemList(e);
}
function OnChangePOItemQty(RowID, e) {
    //debugger;
    CalculationBaseQty(e);
}
function OnChangePOItemRate(RowID, e) {
    //debugger;
    CalculationBaseRate(e);
}

function OnChangeGrossAmt() {
    //debugger;
    var TotalOCAmt = $('#Total_OC_Amount').text();
    var Total_PO_OCAmt = $('#PO_OtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
        if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {
            Calculate_OC_AmountItemWise(TotalOCAmt);
        }
    }
}
function OnChangeAssessAmt(e) {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var errorFlag = "N";
    var DecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var AssessAmt = parseFloat(0).toFixed(DecDigit);
    ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    AssessAmt = clickedrow.find("#item_gr_val").val();
    if (AvoidDot(AssessAmt) == false) {
        clickedrow.find("#item_gr_valError").text($("#valueReq").text());
        clickedrow.find("#item_gr_valError").css("display", "block");
        clickedrow.find("#item_gr_val").css("border-color", "red");
        errorFlag = "Y";
    }
    if (CheckItemRowValidation(e) == false) {
        errorFlag = "Y";
    }
    if (errorFlag == "Y") {
        clickedrow.find("#item_gr_val").val("");
        AssessAmt = 0;
        //return false;
    }
    if (parseFloat(AssessAmt) > 0) {
        if (docid != '105101140101') {
            $("#BtnTxtCalculation").prop("disabled", false);
        }
    }
    else {
        $("#BtnTxtCalculation").prop("disabled", true);
    }
    if (parseFloat(AssessAmt).toFixed(DecDigit) > 0 && ItmCode != "" && Sno != null && Sno != "") {
        //debugger;        
        clickedrow.find("#item_gr_val").val(parseFloat(AssessAmt).toFixed(DecDigit));
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(DecDigit));
    }
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
    // window.location.reload();
}
function OnChangeValidUpToDate(ValidUpToDate) {
    //debugger;
    var ValidUpTo = ValidUpToDate.value;
    var PODate = $("#po_date").val();
    try {
        if (PODate > ValidUpTo) {
            $("#ValidUptoDate").val("")
            $('#SpanValidUpToErrorMsg').text($("#VDateCNBGTPODate").text());
            $("#SpanValidUpToErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanValidUpToErrorMsg").css("display", "none");
            $("#ValidUptoDate").css("border-color", "#ced4da");
        }

    } catch (err) {

    }
}

function OnClickHistoryIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    ItmName = clickedrow.find("#ItemListName" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
    $("#tbl_trackingDetail").DataTable().destroy();
}
function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
    $("#tbl_trackingDetail").DataTable().destroy();
}
function DisableHeaderField() {
    //debugger;
    $("#src_type").attr('disabled', true);
    $("#SupplierName").attr('disabled', true);
    $("#ddlCurrency").attr('disabled', true);
    $("#conv_rate").prop("readonly", true);
}
function BindPOItemList(e) {
    //debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    Itm_ID = clickedrow.find("#ItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {
        clickedrow.find("#ItemListNameError").text($("#valueReq").text());
        clickedrow.find("#ItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#ItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField();
    try {
        //BindHsn(clickedrow, Itm_ID, "");
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");

    } catch (err) {
    }
    BindDelivertSchItemList();
}
function ClearRowDetails(e, ItemID) {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#ItemHsnCode").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#ord_qty").val("");
    clickedrow.find("#item_rate").val("");
    clickedrow.find("#item_gr_val").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#item_oc_amt").val("");
    clickedrow.find("#item_net_val_sp").val("");
    clickedrow.find("#item_net_val_bs").val("");
    clickedrow.find("#remarks").val("");

    CalculateAmount();
    var TOCAmount = parseFloat($("#PO_OtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetPO_ItemTaxDetail();
    if (ItemID != "" && ItemID != null) {
        DelDeliSchAfterDelItem(ItemID);
    }
}
function CalculationBaseQty(e) {
    debugger;
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
    var DisAmt;
    var DisPer;

    OrderQty = clickedrow.find("#ord_qty").val();
    ItemName = clickedrow.find("#ItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();


    if (ItemName == "0") {
        clickedrow.find("#ItemListNameError").text($("#valueReq").text());
        clickedrow.find("#ItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#ord_qty").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
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
        clickedrow.find("#ord_qtyError").text($("#valueReq").text());
        clickedrow.find("#ord_qtyError").css("display", "block");
        clickedrow.find("#ord_qty").css("border-color", "red");
        clickedrow.find("#ord_qty").val("");
        clickedrow.find("#ord_qty").focus();
        clickedrow.find("#item_oc_amt").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qtyError").text("");
        clickedrow.find("#ord_qtyError").css("display", "none");
        clickedrow.find("#ord_qty").css("border-color", "#ced4da");
    }

    var OrderQty = clickedrow.find("#ord_qty").val();
    var ItmRate = clickedrow.find("#item_rate").val();

    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#ord_qty").val("");
        OrderQty = 0;
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_net_val_sp").val(FinVal);
        FinalFinVal = ConvRate * FinVal
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalFinVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    clickedrow.find("#ord_qty").val(parseFloat(OrderQty).toFixed(QtyDecDigit));

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        // CalculateDisPercent(e);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {

    }
    OnChangeGrossAmt();

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
    var assVal = clickedrow.find("#item_gr_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#item_gr_valError").css("display", "none");
        clickedrow.find("#item_gr_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    BindDelivertSchItemList();
}
function CalculationBaseRate(e) {
    debugger;
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
    var DisAmt;
    var DisPer;
    OrderQty = clickedrow.find("#ord_qty").val();
    ItemName = clickedrow.find("#ItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();


    if (ItemName == "0") {
        clickedrow.find("#ItemListNameError").text($("#valueReq").text());
        clickedrow.find("#ItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        clickedrow.find("#ord_qtyError").text($("#valueReq").text());
        clickedrow.find("#ord_qtyError").css("display", "block");
        clickedrow.find("#ord_qty").css("border-color", "red");
        clickedrow.find("#ord_qty").val("");
        clickedrow.find("#ord_qty").focus();
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qtyError").css("display", "none");
        clickedrow.find("#ord_qty").css("border-color", "#ced4da");
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
        clickedrow.find("#item_rateError").text($("#valueReq").text());
        clickedrow.find("#item_rateError").css("display", "block");
        clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#item_rate").focus();
        }
        clickedrow.find("#item_oc_amt").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_net_val_sp").val(FinVal);
        FinalVal = FinVal * ConvRate
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        // CalculateDisPercent(e);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {

    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();

    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        }
    }
    var assVal = clickedrow.find("#item_gr_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#item_gr_valError").css("display", "none");
        clickedrow.find("#item_gr_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }

}

//------------------Tax Amount Calculation------------------//
function OnClickTaxCalculationBtn(e) {
    debugger;
    var ItemListName = "#ItemListName";
    var SNohiddenfiled = "#SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, ItemListName);
    debugger;
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
    var ConvRate = $("#conv_rate").val();
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();

    var tbllenght = $("#TaxCalculatorTbl tbody tr").length
    if (tbllenght == 0) {
        $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
        $("#SpanTax_Template").text($("#valueReq").text());
        $("#SpanTax_Template").css("display", "block");
        $("#SaveAndExitBtn").attr("data-dismiss", "");
        return false;
    }
    else {
        $("#SaveAndExitBtn").attr("data-dismiss", "modal");
    }
    debugger;
    let NewArr = [];
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    if (FTaxDetails > 0) {
        //for (i = 0; i < FTaxDetails.length; i++) {
        //    var TaxUserID = FTaxDetails[i].UserID;
        //    var TaxRowID = FTaxDetails[i].RowSNo;
        //    var TaxItemID = FTaxDetails[i].TaxItmCode;
        //    if (TaxUserID == UserID && TaxRowID == RowSNo && TaxItemID == TaxItmCode) {
        //    }
        //    else {
        //        NewArr.push(FTaxDetails[i]);
        //    }
        //}
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
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
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                </tr>`)
            //TotalTaxAmount = parseFloat(TotalTaxAmount)+parseFloat(TaxAmount);
            NewArr.push({ /*UserID: UserID, RowSNo: RowSNo, */TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //sessionStorage.removeItem("TaxCalcDetails");
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(NewArr));
        BindTaxAmountDeatils(NewArr);
    }
    else {
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
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
 <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                </tr>
`)
            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
        BindTaxAmountDeatils(TaxCalculationList);
    }
    //}
    //else {
    //    var TaxCalculationList = [];
    //    $("#TaxCalculatorTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        var TaxName = currentRow.find("#taxname").text();
    //        var TaxNameID = currentRow.find("#taxid").text();
    //        var TaxPercentage = currentRow.find("#taxrate").text();
    //        var TaxLevel = currentRow.find("#taxlevel").text();
    //        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
    //        var TaxAmount = currentRow.find("#taxval").text();
    //        var TaxApplyOnID = currentRow.find("#taxapplyon").text();

    //        TaxCalculationList.push({ UserID: UserID, RowSNo: RowSNo, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    //    });
    //    //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
    //    BindTaxAmountDeatils(TaxCalculationList);
    //}
    0
    $("#SPOItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {

            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
            }
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
            }
            else {
                currentRow.find("#item_tax_amt").val(TaxAmt);
            }
            // currentRow.find("#item_tax_amt").val(TaxAmt);
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
            }
            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
            //***Modifyed By shubham maurya on 12-12-2023 for remove NaN Start***//
            if (NetOrderValueBase == "NaN") {
                NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
            }
            if (NetOrderValueSpec == "NaN") {
                NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
            }
            //***Modifyed By shubham maurya on 12-12-2023 for remove NaN End***//
            currentRow.find("#item_net_val_sp").val(NetOrderValueSpec);
            FinalNetOrderValueBase = ConvRate * NetOrderValueBase;

            currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

        }
    });
    CalculateAmount();
}
function OnClickReplicateOnAllItems() {
    var ConvRate = $("#conv_rate").val();
    var errorflag = "N";
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmRowSNo = RowSNo;
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#Hdn_TaxCalculatorTbl > tbody > tr").remove();
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();

        TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            $("#SPOItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemCode;
                var AssessVal;
                //debugger;
                ItemCode = currentRow.find("#ItemListName" + Sno).val();
                AssessVal = currentRow.find("#item_gr_val").val();
                //if (CheckPOItemValidations() == false) {
                //    errorflag = "Y"
                //}
                var AssessValcheck = parseFloat(AssessVal);
                if (AssessVal != "" && AssessValcheck > 0) {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);

                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        var RowSNo = TaxCalculationList[i].RowSNo;
                        TaxPercentage = TaxCalculationList[i].TaxPercentage;
                        var TaxLevel = TaxCalculationList[i].TaxLevel;
                        var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                        var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
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
                        NewArray.push({ /*UserID: UserID, RowSNo: Sno,*/ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                    }
                    if (NewArray != null) {
                        if (NewArray.length > 0) {
                            for (k = 0; k < NewArray.length; k++) {
                                var TaxName = NewArray[k].TaxName;
                                var TaxNameID = NewArray[k].TaxNameID;
                                var TaxItmCode = NewArray[k].TaxItmCode;
                                var RowSNo = NewArray[k].RowSNo;
                                var TaxPercentage = NewArray[k].TaxPercentage;
                                var TaxLevel = NewArray[k].TaxLevel;
                                var TaxApplyOn = NewArray[k].TaxApplyOn;
                                var TaxAmount = NewArray[k].TaxAmount;
                                var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                                if (CitmTaxItmCode != TaxItmCode && CitmRowSNo != RowSNo) {
                                    //    $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                    // <tr>
                                    //    <td id="UserID">${UserID}</td>
                                    //    <td id="RowSNo">${RowSNo}</td>
                                    //    <td id="TaxItmCode">${TaxItmCode}</td>
                                    //    <td id="TaxName">${TaxName}</td>
                                    //    <td id="TaxNameID">${TaxNameID}</td>
                                    //    <td id="TaxPercentage">${TaxPercentage}</td>
                                    //    <td id="TaxLevel">${TaxLevel}</td>
                                    //    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    //    <td id="TaxAmount">${TaxAmount}</td>
                                    //    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    //    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    //</tr>`)
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }
                }

            });
        }
    }
    if (TaxCalculationListFinalList.length > 0) {
        // sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        /* Modifyed By Shubham Maurya on 07 - 10 - 2023 : 1423 for service Purchase Order Tax add <td id="DocNo"></td><td id="DocDate"></td> */
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
                                 <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxCalculationListFinalList[j].TaxItmCode}</td>
                                    <td id="TaxName">${TaxCalculationListFinalList[j].TaxName}</td>
                                    <td id="TaxNameID">${TaxCalculationListFinalList[j].TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxCalculationListFinalList[j].TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxCalculationListFinalList[j].TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxCalculationListFinalList[j].TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxCalculationListFinalList[j].TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TaxCalculationListFinalList[j].TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxCalculationListFinalList[j].TaxApplyOnID}</td>
                                </tr>`)
        }

        BindTaxAmountDeatils(TaxCalculationListFinalList);
    } else {
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }

    //debugger;
    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        //var Sno = currentRow.find("#SNohiddenfiled").val();
        var hfItemID = currentRow.find("#hfItemID").val();
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                    //var RowSNoF = TaxCalculationListFinalList[i].RowSNo;
                    var txtTaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;

                    if (hfItemID == txtTaxItmCode) {

                        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                        var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        currentRow.find("#item_net_val_sp").val(NetOrderValueSpec);
                        FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                        currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));
                    }
                }
            }
            else {
                var GrossAmtOR = parseFloat(0).toFixed(DecDigit);
                if (currentRow.find("#item_gr_val").val() != "") {
                    GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                }
                currentRow.find("#item_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                    OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                }
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                currentRow.find("#item_gr_val").val(GrossAmtOR);
                currentRow.find("#item_net_val_sp").val(FGrossAmtOR);
                FinalGrossAmtOR = ConvRate * FGrossAmtOR
                currentRow.find("#item_net_val_bs").val(parseFloat(FinalGrossAmtOR).toFixed(DecDigit));

            }
        }
    });
    CalculateAmount();
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {

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
                    //debugger;
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

                NewArray.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            //for (i = 0; i < FTaxDetailsItemWise.length; i++) {
            //    var TaxUserID = FTaxDetailsItemWise[i].UserID;
            //    var TaxRowID = FTaxDetailsItemWise[i].RowSNo;
            //    var TaxItemID = FTaxDetailsItemWise[i].TaxItmCode;
            //    var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
            //    if (TaxUserID == UserID && TaxRowID == RowSNo && TaxItemID == ItmCode) {

            //        var TaxNameID = FTaxDetailsItemWise[i].TaxNameID;
            //        var TaxLevel = FTaxDetailsItemWise[i].TaxLevel;
            //        var TaxApplyOn = FTaxDetailsItemWise[i].TaxApplyOn;
            //        var TaxPercentage = "";
            //        TaxPercentage = FTaxDetailsItemWise[i].TaxPercentage;
            //        var TaxAmount;
            //        var TaxPec;
            //        TaxPec = TaxPercentage.replace('%', '');

            //        if (TaxApplyOn == "Immediate Level") {
            //            if (TaxLevel == "1") {
            //                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
            //                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
            //            }
            //            else {
            //                var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
            //                var TaxLevelTbl = parseInt(TaxLevel) - 1;
            //                for (j = 0; j < NewArray.length; j++) {
            //                    var Level = NewArray[j].TaxLevel;
            //                    var TaxAmtLW = NewArray[j].TaxAmount;
            //                    if (TaxLevelTbl == Level) {
            //                        TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
            //                    }
            //                }
            //                var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
            //                TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
            //                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
            //            }
            //        }
            //        if (TaxApplyOn == "Cummulative") {
            //            var Level = TaxLevel;
            //            if (TaxLevel == "1") {
            //                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
            //                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
            //            }
            //            else {
            //                var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

            //                for (j = 0; j < NewArray.length; j++) {
            //                    var Level = NewArray[j].TaxLevel;
            //                    var TaxAmtLW = NewArray[j].TaxAmount;
            //                    if (TaxLevel != Level) {
            //                        TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
            //                    }
            //                }
            //                var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
            //                TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
            //                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
            //            }
            //        }
            //        //debugger;
            //        NewArray.push({ UserID: UserID, RowSNo: RowSNo, TaxItmCode: ItmCode, TaxNameID: TaxNameID, TaxLevel: TaxLevel, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt })
            //        FTaxDetailsItemWise[i].TaxAmount = TaxAmount;
            //        FTaxDetailsItemWise[i].TotalTaxAmount = TotalTaxAmt;
            //    }
            //}
            //for (j = 0; j < FTaxDetailsItemWise.length; j++) {
            //    var TaxUserID = FTaxDetailsItemWise[j].UserID;
            //    var TaxRowID = FTaxDetailsItemWise[j].RowSNo;
            //    var TaxItemID = FTaxDetailsItemWise[j].TaxItmCode;
            //    if (TaxUserID == UserID && TaxRowID == RowSNo && TaxItemID == ItmCode) {
            //        FTaxDetailsItemWise[j].TotalTaxAmount = TotalTaxAmt;
            //    }
            //}
            //$("#SPOItmDetailsTbl >tbody >tr #ItemListName" + RowSNo + ":contains(" + ItmCode + ")").parent().parent().each(function () {
            $("#SPOItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#ItemListName" + Sno).val();

                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            //var RowSNoF = CRow.find("#RowSNo").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            if (/*RowSNoF == RowSNo && */TaxItmCode == ItmCode) {
                                //if (Sno == RowSNoF) {

                                //TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                                currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#item_net_val_sp").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

                                //}
                            }
                        });
                        //for (i = 0; i < FTaxDetailsItemWise.length; i++) {
                        //    var TotalTaxAmtF = FTaxDetailsItemWise[i].TotalTaxAmount;
                        //    var RowSNoF = FTaxDetailsItemWise[i].RowSNo;
                        //    var TaxItmCode = FTaxDetailsItemWise[i].TaxItmCode;
                        //    if (RowSNoF == RowSNo && TaxItmCode == ItmCode) {
                        //        if (Sno == RowSNoF) {

                        //            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                        //            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                        //            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                        //            if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        //                OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        //            }
                        //            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                        //            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        //            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        //            currentRow.find("#item_net_val_sp").val(NetOrderValueSpec);
                        //            currentRow.find("#item_net_val_bs").val(NetOrderValueBase);

                        //        }
                        //    }
                        //}
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        //currentRow.find("#item_ass_val").val(GrossAmtOR);
                        currentRow.find("#item_net_val_sp").val(FGrossAmtOR);
                        FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        currentRow.find("#item_net_val_bs").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
            CalculateAmount();
            //sessionStorage.removeItem("TaxCalcDetails");
            //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(FTaxDetailsItemWise));
            debugger;
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    debugger;

    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
}
function AfterDeleteResetPO_ItemTaxDetail() {
    //debugger;
    var SPOItmDetailsTbl = "#SPOItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ItemListName = "#ItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(SPOItmDetailsTbl, SNohiddenfiled, ItemListName);
}
function OnClickSaveAndExit_OC_Btn() {
    //debugger;
    var NetOrderValueSpe = "#NetOrderValueSpe";
    var NetOrderValueInBase = "#NetOrderValueInBase";
    var PO_otherChargeId = '#Tbl_OtherChargeList';

    CMNOnClickSaveAndExit_OC_Btn(PO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    //debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#item_gr_val").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#item_oc_amt").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var OCValue = currentRow.find("#item_oc_amt").val();
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
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var POItm_GrossValue = currentRow.find("#item_gr_val").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#item_oc_amt").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(DecDigit);
        }
        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#item_net_val_sp").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(DecDigit));
        FinalSPOItm_NetOrderValueBase = ConvRate * POItm_NetOrderValueBase
        currentRow.find("#item_net_val_bs").val((parseFloat(FinalSPOItm_NetOrderValueBase)).toFixed(DecDigit));
    });
    CalculateAmount();
};
function BindOtherChargeDeatils() {
    debugger;
    var DecDigit = $("#ValDigit").text();//Tbl_OC_Deatils
    if ($("#SPOItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
        $("#PO_OtherCharges").val(parseFloat(0).toFixed(DecDigit));
    }

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#PO_OtherChargeTotal").text(parseFloat(0).toFixed($("#ValDigit").text()));
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
<td align="right">${currentRow.find("#OCAmount").text()}</td>
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
        });

    }
    $("#_OtherChargeTotal").text(TotalAMount);
    $("#PO_OtherCharges").val(TotalAMount);
}
function SetOtherChargeVal() {

}
//------------------End------------------//

//------------------Delivert Section------------------//
function BindDelivertSchItemList() {
    Cmn_BindDelivertSchItemList("SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty")
};
function OnClickReplicateDeliveryShdul() {
    Cmn_OnClickReplicateDeliveryShdul("SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty");
}
function OnClickAddDeliveryDetail() {
    debugger;
    Cmn_OnClickAddDeliveryDetail("SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty");
}
function DeleteDeliverySch() {
    Cmn_DeleteDeliverySch();
}
function OnChangeDelSchItm() {
    Cmn_OnChangeDelSchItm("SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty");
}
function OnChangeDeliveryDate(DeliveryDate) {
    Cmn_OnChangeDeliveryDate(DeliveryDate);
}
function OnChangeDeliveryQty() {
    Cmn_OnChangeDeliveryQty("SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty");
}
function DelDeliSchAfterDelItem(ItemID) {
    Cmn_DelDeliSchAfterDelItem(ItemID, "SPOItmDetailsTbl", "N", "SNohiddenfiled", "ItemListName", "ord_qty");
}
//------------------End------------------//

function appendTermsAndCondition(rowIdx, TandC) {
    var deletetext = $("#Span_Delete_Title").text();
    $('#TblTerms_Condition tbody').append(`<tr id="R${rowIdx}" class="even pointer">
<td class="red"><i class="deleteIcon fa fa-trash" id="TC_DelIcon" title="${deletetext}"></i> </td>
<td>${TandC}</td>
</tr>`);
}
function EnableDeleteAction() {
    DeleteTermCondition();
    DeleteDeliverySch();
    Cmn_EnableDeleteAttatchFiles();
    //$("#PartialImageBind .file-preview .file-preview-thumbnails").each(function () {
    //    var Current = $(this);
    //    var filename = Current.find(".file-caption-info")[0].innerText;
    //    Current.find("#RemoveAttatchbtn").attr("onclick", "RemoveAttatchFile(event,'" + filename + "')");
    //});
    //$("#RemoveAttatchbtn").attr("onclick", "RemoveAttatchFile(event,'')")
}
function Ass_valFloatValueonly(el, evt) {
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
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_gr_valError" + RowNo).css("display", "none");
    clickedrow.find("#item_gr_val" + RowNo).css("border-color", "#ced4da");

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function RateFloatValueonly(el, evt) {
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
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#ord_qtyError").css("display", "none");
    clickedrow.find("#ord_qty").css("border-color", "#ced4da");

    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function AmtFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#item_rate").val();
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
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    return true;
}
function FloatValuePerOnly(el, evt) {
    // var charCode = (evt.which) ? evt.which : event.keyCode;
    $("#SpanTaxPercent").css("display", "none");
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    ////debugger;
    //var key = evt.key;
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > 1) {
    //    return false;
    //}
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }

}
function AddNewRow() {
    var rowIdx = 0;
    //debugger;
    /*var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#SPOItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#SPOItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
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
    AddNewRowForEditItem();

    BindPOItmList(RowNo);

};

function CheckPOValidations() {
    debugger;
    var ErrorFlag = "N";
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


    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanSuppCurrErrorMsg').text($("#valueReq").text());
        $("#SpanSuppCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
    }
    if ($("#conv_rate").val() === "0" || $("#conv_rate").val() == "") {
        $('#SpanSuppExRateErrorMsg').text($("#valueReq").text());
        $("#SpanSuppExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
    if ($("#ValidUptoDate").val() == "") {
        $('#SpanValidUpToErrorMsg').text($("#valueReq").text());
        $("#SpanValidUpToErrorMsg").css("display", "block");
        $("#ValidUptoDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanValidUpToErrorMsg").css("display", "none");
        $("#ValidUptoDate").css("border-color", "#ced4da");
    }
    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckPOHraderValidations() {
    //debugger;
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
    }
    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanSuppCurrErrorMsg').text($("#valueReq").text());
        $("#SpanSuppCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
    }
    if ($("#conv_rate").val() === "0" || $("#conv_rate").val() == "") {
        $('#SpanSuppExRateErrorMsg').text($("#valueReq").text());
        $("#SpanSuppExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
}
function CheckPOItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#SPOItmDetailsTbl >tbody >tr").length > 0) {
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#ItemListName" + Sno).val() == "0") {
                currentRow.find("#ItemListNameError").text($("#valueReq").text());
                currentRow.find("#ItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#ord_qty").val() == "") {
                currentRow.find("#ord_qtyError").text($("#valueReq").text());
                currentRow.find("#ord_qtyError").css("display", "block");
                currentRow.find("#ord_qty").css("border-color", "red");
                currentRow.find("#ord_qty").focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qtyError").css("display", "none");
                currentRow.find("#ord_qtyError").css("border-color", "#ced4da");
            }
            if (currentRow.find("#ord_qty").val() != "") {
                if (parseFloat(currentRow.find("#ord_qty").val()).toFixed(QtyDecDigit) == 0) {
                    currentRow.find("#ord_qtyError").text($("#valueReq").text());
                    currentRow.find("#ord_qtyError").css("display", "block");
                    currentRow.find("#ord_qty").css("border-color", "red");
                    currentRow.find("#ord_qty").focus();
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#ord_qtyError").css("display", "none");
                    currentRow.find("#ord_qtyError").css("border-color", "#ced4da");
                }
            }
            if (currentRow.find("#item_rate").val() == "") {
                currentRow.find("#item_rateError").text($("#valueReq").text());
                currentRow.find("#item_rateError").css("display", "block");
                currentRow.find("#item_rate").css("border-color", "red");
                if (currentRow.find("#ord_qty").val() != "" && currentRow.find("#ord_qty").val() != null) {
                    currentRow.find("#item_rate").focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rateError").css("display", "none");
                currentRow.find("#item_rate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_rate").val() != "") {
                if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_rateError").text($("#valueReq").text());
                    currentRow.find("#item_rateError").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_rateError").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
                }
            }
            if (currentRow.find("#item_gr_val").val() == "") {
                currentRow.find("#item_gr_valError").text($("#valueReq").text());
                currentRow.find("#item_gr_valError").css("display", "block");
                currentRow.find("#item_gr_val").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_gr_valError").css("display", "none");
                currentRow.find("#item_gr_val").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_gr_val").val() != "") {
                if (parseFloat(currentRow.find("#item_gr_val").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_gr_valError").text($("#valueReq").text());
                    currentRow.find("#item_gr_valError").css("display", "block");
                    currentRow.find("#item_gr_val").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_gr_valError").css("display", "none");
                    currentRow.find("#item_gr_val").css("border-color", "#ced4da");
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
function CheckItemRowValidation(e) {
    //debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();

    if (currentRow.find("#ItemListName" + Sno).val() == "0") {
        currentRow.find("#ItemListNameError").text($("#valueReq").text());
        currentRow.find("#ItemListNameError").css("display", "block");
        currentRow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#ItemListNameError").css("display", "none");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
    }
    if (currentRow.find("#ord_qty").val() == "") {
        currentRow.find("#ord_qtyError").text($("#valueReq").text());
        currentRow.find("#ord_qtyError").css("display", "block");
        currentRow.find("#ord_qty").css("border-color", "red");
        currentRow.find("#ord_qty").focus();
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#ord_qtyError").css("display", "none");
        currentRow.find("#ord_qtyError").css("border-color", "#ced4da");
    }
    if (currentRow.find("#ord_qty").val() != "") {
        if (parseFloat(currentRow.find("#ord_qty").val()).toFixed(QtyDecDigit) == 0) {
            currentRow.find("#ord_qtyError").text($("#valueReq").text());
            currentRow.find("#ord_qtyError").css("display", "block");
            currentRow.find("#ord_qty").css("border-color", "red");
            currentRow.find("#ord_qty").focus();
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ord_qtyError").css("display", "none");
            currentRow.find("#ord_qtyError").css("border-color", "#ced4da");
        }
    }
    if (currentRow.find("#item_rate").val() == "") {
        currentRow.find("#item_rateError").text($("#valueReq").text());
        currentRow.find("#item_rateError").css("display", "block");
        currentRow.find("#item_rate").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
    }
    if (currentRow.find("#item_rate").val() != "") {
        if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
            currentRow.find("#item_rateError").text($("#valueReq").text());
            currentRow.find("#item_rateError").css("display", "block");
            currentRow.find("#item_rate").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
        }
    }
    if (currentRow.find("#item_gr_val").val() == "") {
        currentRow.find("#item_gr_valError").text($("#valueReq").text());
        currentRow.find("#item_gr_valError").css("display", "block");
        currentRow.find("#item_gr_val").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#item_gr_valError").css("display", "none");
        currentRow.find("#item_gr_val").css("border-color", "#ced4da");
    }
    if (currentRow.find("#item_gr_val").val() != "") {
        if (parseFloat(currentRow.find("#item_gr_val").val()).toFixed(RateDecDigit) == 0) {
            currentRow.find("#item_gr_valError").text($("#valueReq").text());
            currentRow.find("#item_gr_valError").css("display", "block");
            currentRow.find("#item_gr_val").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_gr_valError").css("display", "none");
            currentRow.find("#item_gr_val").css("border-color", "#ced4da");
        }
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    ItemInfoBtnClick(ItmCode);
}
function InsertSPODetails() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var PODTransType = sessionStorage.getItem("POTransType");
    var POStatus = "";

    if (CheckPOValidations() == false) {
        return false;
    }
    if (CheckPOItemValidations() == false) {
        return false;
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (Cmn_taxVallidation("SPOItmDetailsTbl", "item_tax_amt", "ItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
            return false;
        }
    }
    if (Cmn_CheckDeliverySchdlValidations("SPOItmDetailsTbl", "hfItemID", "ItemListName", "ord_qty", "SNohiddenfiled") == false) {
        return false;
    }

    if (navigator.onLine === true)/*Checking For Internet is open or not*/ {

        var FinalSPOItemDetail = [];
        var FinalSPODeliveryDetail = [];
        var FinalSPOTaxDetail = [];
        var FinalSPOOCDetail = [];
        var FinalSPOTermDetail = [];

        FinalSPOItemDetail = InsertSPOItemDetails();
        FinalSPOTaxDetail = InsertSPOTaxDetails();
        FinalSPOOCDetail = InsertSPOOtherChargeDetails();
        FinalSPODeliveryDetail = InsertSPOItem_DeliverySchDetails();
        FinalSPOTermDetail = InsertSPOTermConditionDetails();

        $("#hdItemDetailList").val(JSON.stringify(FinalSPOItemDetail));
        $("#hdTaxDetailList").val(JSON.stringify(FinalSPOTaxDetail));
        $("#hdOCDetailList").val(JSON.stringify(FinalSPOOCDetail));
        $("#hdDelSchDetailList").val(JSON.stringify(FinalSPODeliveryDetail));
        $("#hdTermsDetailList").val(JSON.stringify(FinalSPOTermDetail));

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);

        $("#hdn_Attatchment_details").val(ItemAttchmentDt);
        /*----- Attatchment End--------*/


        $("#Hdn_SupplierName").val($("#SupplierName").val());
        //$("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
        $("#Hdn_conv_rate").val($("#conv_rate").val());
        var uptodate = $("#ValidUptoDate").val();
        var date = moment().format('YYYY-MM-DD');
        if (uptodate < date) {
            $("#btn_save").attr("disabled", false);
            $("#hdnsavebtn").val("");
        }
        else {
            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        }
        return true;

    }
};
function InsertSPOItemDetails() {
    //debugger;
    var POItemList = [];
    $("#SPOItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var ItemID = "";
        var Hsn_no = "";
        var OrderQty = "";
        var InvQty = "";
        var ItmRate = "";
        var GrossVal = "";
        var TaxAmt = "";
        var OCAmt = "";
        var NetValSpec = "";
        var NetValBase = "";
        var FClosed = "";
        var Remarks = "";
        var TaxExempted = "";
        var ManualGST = "";
        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();
        var sr_on = currentRow.find("#srno").text();
        ItemID = currentRow.find("#ItemListName" + SNo).val();
        Hsn_no = currentRow.find("#ItemHsnCode").val();
        OrderQty = currentRow.find("#ord_qty").val();
        if (currentRow.find("#inv_qty").val() == null || currentRow.find("#inv_qty").val() == "") {
            InvQty = "0";
        }
        else {
            InvQty = currentRow.find("#inv_qty").val();
        }
        ItmRate = currentRow.find("#item_rate").val();

        GrossVal = currentRow.find("#item_gr_val").val();
        //AssVal = currentRow.find("#item_ass_val").val();
        if (currentRow.find("#item_tax_amt").val() === "" || currentRow.find("#item_tax_amt").val() === null) {
            TaxAmt = "0";
        }
        else {
            TaxAmt = currentRow.find("#item_tax_amt").val();
        }
        if (currentRow.find("#item_oc_amt").val() === "" || currentRow.find("#item_oc_amt").val() === null) {
            OCAmt = "0";
        }
        else {
            OCAmt = currentRow.find("#item_oc_amt").val();
        }

        NetValSpec = currentRow.find("#item_net_val_sp").val();
        NetValBase = currentRow.find("#item_net_val_bs").val();

        if (currentRow.find("#ItmFClosed").is(":checked")) {
            FClosed = "Y";
        }
        else {
            FClosed = "N";
        }
        Remarks = currentRow.find("#remarks").val();
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
        }
        POItemList.push({
            ItemID: ItemID, Hsn_no: Hsn_no, OrderQty: OrderQty, InvQty: InvQty, ItmRate: ItmRate,
            GrossVal: GrossVal, TaxAmt: TaxAmt, OCAmt: OCAmt, NetValSpec: NetValSpec, NetValBase: NetValBase,
            FClosed: FClosed, Remarks: Remarks, TaxExempted: TaxExempted, ManualGST: ManualGST, sr_on: sr_on
        });
    });
    return POItemList;
};
function InsertSPOTaxDetails() {

    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var POTaxList = [];

    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#ItemListName" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                        var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
                        POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });
                    });
                }
            }
        }
    });
    return POTaxList;
    //if (FTaxDetails != null) {
    //    if (FTaxDetails > 0) {
    //        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
    //            var Crow = $(this);
    //            var ItemID = Crow.find("#TaxItmCode").text().trim();
    //            var TaxID = Crow.find("#TaxNameID").text().trim();
    //            var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
    //            var TaxLevel = Crow.find("#TaxLevel").text().trim();
    //            var TaxValue = Crow.find("#TaxAmount").text().trim();
    //            var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
    //            POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn });
    //        });
    //    }
    //}
    //return POTaxList;
};
function InsertSPOOtherChargeDetails() {
    var PO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            PO_OCList.push({ OC_ID: OC_ID, OCValue: OCValue });
        });
    }
    return PO_OCList;
};
function InsertSPOItem_DeliverySchDetails() {
    var PODelieryList = [];
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemID = "";
            var SchDate = "";
            var DeliveryQty = "";
            var RecQty = "";
            var PendingQty = "";
            ItemID = currentRow.find("#Hd_ItemIdFrDS").text();
            DeliveryQty = currentRow.find("#delv_qty").text();
            SchDate = currentRow.find("#sch_date").text();
            RecQty = currentRow.find("#SRQty").text();
            PendingQty = currentRow.find("#PenQty").text();

            PODelieryList.push({ ItemID: ItemID, SchDate: SchDate, DeliveryQty: DeliveryQty });
        });
    }
    return PODelieryList;
};
function InsertSPOTermConditionDetails() {
    var POTermsList = [];
    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var TermsDesc = "";
            TermsDesc = currentRow.find("#term_desc").text();
            POTermsList.push({ TermsDesc: TermsDesc });
        });
    }
    return POTermsList;
};
function InsertSPOAttachmentDetails() {
    var formData = new FormData();
    var fp = $('#file-1');
    var lg = fp[0].files.length;
    if (lg > 0) {
        for (var i = 0; i < lg; i++) {
            formData.append('file', $('#file-1')[0].files[i]);
        }
    }
    return formData;
};
function CalculateAmount() {
    //debugger;
    var DecDigit = $("#ValDigit").text();
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        }
        //if (currentRow.find("#item_ass_val").val() == "" || currentRow.find("#item_ass_val").val() == "NaN") {
        //    AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(DecDigit);
        //}
        //else {
        //    AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_ass_val").val())).toFixed(DecDigit);
        //}
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_net_val_sp").val() == "" || currentRow.find("#item_net_val_sp").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val_sp").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_net_val_bs").val() == "" || currentRow.find("#item_net_val_bs").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#item_net_val_bs").val())).toFixed(DecDigit);
        }
    });
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
    $("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
};
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#SPOItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
function DisableAfterSave() {
    DisableHeaderField();
    $("#ValidUptoDate").prop("readonly", true);
    $("#remarks").prop("readonly", true);
    $("#src_doc_number").prop("disabled", true);
    $("#SPOItmDetailsTbl .plus_icon1").css("display", "none");
    DisableItemDetailsPO();
    $("#ForceClosed").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);
    $("#Tax_Template").attr('disabled', true);
    $("#Tax_Type").attr('disabled', true);
    $("#Tax_Percentage").prop("readonly", true);
    $("#Tax_Level").attr('disabled', true);
    $("#Tax_ApplyOn").attr('disabled', true);
    $("#TaxResetBtn").css("display", "none");
    $("#TaxSaveTemplate").css("display", "none");
    $("#ReplicateOnAllItemsBtn").css("display", "none");
    $("#SaveAndExitBtn").css("display", "none");
    $("#TaxExitAndDiscard").css("display", "none");
    $("#OtherCharge_DDL").attr('disabled', true);
    $("#TxtOCAmt").prop("readonly", true);
    $("#SaveAndExitBtn_OC").css("display", "none");
    $("#DiscardAndExit_OC").css("display", "none");

    $("#ForDisableOCDlt").val("Disable");
    //$("#Tbl_OC_Deatils >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    currentRow.find("#OCDelIcon").css("display", "none");
    //});
    $("#DeliverySchItemDDL").attr('disabled', true);
    $("#DeliveryDate").prop("readonly", true);
    $("#DeliverySchQty").prop("readonly", true);
    $(".plus_icon").css("display", "none");
    $("#DeliverySchTble >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#DeliveryDelIconBtn").css("display", "none");
    });
    $("#TxtTerms_Condition").prop("readonly", true);
    $(".plus_icon1").css("display", "none");
    $("#TblTerms_Condition >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TC_DelIcon").css("display", "none");
    });
    $("#TemplateTermsddl").attr('disabled', true);
    //$("#file-1").attr('disabled', true);

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
            $("#hdnAction").val("Delete");
            //location.href = "/ApplicationLayer/PurchaseQuotation/PQDetailsActionCommands";
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function EnableForEdit() {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    $("#ValidUptoDate").prop("readonly", false);
    $("#remarks").prop("readonly", false);
    if ($("#hdn_attatchment_list tbody tr").length < 5) {
        $("#file-1").attr('disabled', false);
        $("#FilesUpload").css("display", "block");
    }



    if ($("#SupplierName").val() != "" && $("#SupplierName").val() != null && $("#SupplierName").val() != "0") {
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            debugger;
            currentRow.find("#ItemListName" + Sno).attr("disabled", false);
            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
            currentRow.find("#ord_qty").attr("disabled", false);
            currentRow.find("#item_rate").attr("disabled", false);
            currentRow.find("#item_gr_val").attr("disabled", false);
            currentRow.find("#remarks").attr("disabled", false);
        });
    }

    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#delBtnIcon").css("display", "block");
    });

    $("#Tax_Template").attr('disabled', false);
    $("#Tax_Type").attr('disabled', false);
    $("#Tax_Percentage").prop("readonly", false);
    $("#Tax_Level").attr('disabled', false);
    $("#Tax_ApplyOn").attr('disabled', false);
    $("#TaxResetBtn").css("display", "block");
    $("#TaxSaveTemplate").css("display", "block");
    $("#ReplicateOnAllItemsBtn").css("display", "block");
    $("#SaveAndExitBtn").css("display", "block");
    $("#TaxExitAndDiscard").css("display", "block");

    $("#OtherCharge_DDL").attr('disabled', false);
    $("#TxtOCAmt").prop("readonly", false);
    $("#SaveAndExitBtn_OC").css("display", "block");
    $("#DiscardAndExit_OC").css("display", "block");

    $("#ForDisableOCDlt").val("Enable");

    $("#DeliverySchItemDDL").attr('disabled', false);
    $("#DeliveryDate").prop("readonly", false);
    $("#DeliverySchQty").prop("readonly", false);
    $(".plus_icon").css("display", "block");
    $("#DeliverySchTble >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#DeliveryDelIconBtn").css("display", "block");
    });

    $("#TxtTerms_Condition").prop("readonly", false);
    $(".plus_icon1").css("display", "block");

    if ($("#SupplierName").val() == "" || $("#SupplierName").val() == null || $("#SupplierName").val() == "0") {
        $("#SPOItmDetailsTbl .plus_icon1").css("display", "none");
    }

    $("#TblTerms_Condition >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TC_DelIcon").css("display", "block");
    });
    $("#TemplateTermsddl").attr('disabled', false);




} 

function EnableForEditAfterApprove(Status) {
    if (Status == "A") {
        $("#Cancelled").attr('disabled', false);
    }
    if (Status === "PDL" || Status === "PR" || Status === "PN") {
        $("#ForceClosed").attr('disabled', false);
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("checked") == true) {
                currentRow.find("#ItmFClosed").attr("disabled", true);
            } else {
                currentRow.find("#ItmFClosed").attr("disabled", false);
            }
        });
    }
}
function DisableForSaveAfterApprove() {
    $("#ForceClosed").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);

    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#ItmFClosed").attr("disabled", true);
    });
}
function CheckedFClose() {
    //debugger;
    if ($("#ForceClosed").is(":checked")) {
        $("#Cancelled").attr("disabled", true);
        $("#Cancelled").prop("checked", false);
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {

            } else {
                currentRow.find("#ItmFClosed").prop("checked", true);
                //currentRow.find("#ItmFClosed").attr("disabled", true);
            }

        });
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {

            } else {
                currentRow.find("#ItmFClosed").prop("checked", false);
                //currentRow.find("#ItmFClosed").attr("disabled", false);
            }

        });
        CheckedItemWiseFClose();
    }
}
function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {

        $("#ForceClosed").attr("disabled", true);
        $("#ForceClosed").prop("checked", false);
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
function CheckedItemWiseFClose() {
    //debugger;
    var FClose = "N";
    var FinalFClose = "Y";
    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //debugger;
        if (currentRow.find("#ItmFClosed").is(":checked")) {
            FClose = "Y";
        }
        else {
            FinalFClose = "N";

        }
    });
    if (FClose == "Y") {
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    if (FClose == "Y" && FinalFClose == "Y") {
        $("#ForceClosed").prop("checked", true);
    }
    else {
        $("#ForceClosed").prop("checked", false);
    }
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
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
function OnClickSupplierInfoIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";

    ItmCode = clickedrow.find("#ItemListName" + Sno).val();
    var Supp_id = $('#SupplierName').val();

    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
function GetTrackingDetails() {
    //debugger;
    $("#tbl_trackingDetail").DataTable().destroy();
    $("#tbl_trackingDetail").DataTable();
}

function approveonclick() {
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

function OnCheckedChangePrintQuantityTotal() {
    debugger;
    if ($('#chkshowtotalqty').prop('checked')) {
        $('#hdn_ShowTotalQty').val('Y');
    }
    else {
        $('#hdn_ShowTotalQty').val('N');
    }
}
function OnCheckedChangeShowRemarksBlwItm() {
    debugger;
    if ($('#chkshowremarksblwitm').prop('checked')) {
        $('#hdn_ShowRemarksBlwItm').val('Y');
    }
    else {
        $('#hdn_ShowRemarksBlwItm').val('N');
    }
}
function OnCheckedChangeProdDesc() {
    debugger;
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#ShowCustSpecProdDesc').val('N');
        $("#ShowProdDesc").val("Y");
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    debugger;
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$("#ShowProdDesc").val("N");
        $('#ShowCustSpecProdDesc').val('Y');
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    debugger;
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#ShowProdTechDesc').val('Y');
    }
    else {
        $('#ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#ShowSubItem').val('Y');
    }
    else {
        $('#ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeI").is(":checked")) {
        $('#PrintFormat').val('F2');
    }
    else {
        $('#PrintFormat').val('F1');
    }
}
function OnCheckedChangeSuppAliasName() {/*add by Hina on 19-05-2025*/
    debugger;
    if ($('#chksuppaliasname').prop('checked')) {
        $('#ShowSuppAliasName').val('Y');
    }
    else {
        $('#ShowSuppAliasName').val('N');
    }
}



function OnClickTaxExemptedCheckBox(e) {
    debugger;

    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#ItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_gr_val").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#item_rate").trigger('change');
        debugger;
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        //if ($("#taxTemplate").text() == "GST Slab") {
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "SPOItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val")
            //}
            //CalculationBaseRate(e)
            //CalculateAmount();
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#ItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_gr_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#item_rate").trigger('change');
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        //var gst_number = $("#ship_add_gstNo").val()
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "SPOItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val");
        var trgtrow = $(e.target).closest("tr");
        CalculationBaseRate(trgtrow)
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}

function OnCheckedChangeDeliverySchedule() {/*add by Hina Sharma on 26-12-2024*/
    if ($('#chkshowdeliveryschedule').prop('checked')) {
        $("#ShowDeliverySchedule").val("Y");
    }
    else {
        $("#ShowDeliverySchedule").val("N");
    }
}
function OnCheckedChangeHsnNumber() {/*add by Hina Sharma on 26-12-2024*/
    if ($('#chkhsnnumber').prop('checked')) {
        $("#ShowHSNNumber").val("Y");
    }
    else {
        $("#ShowHSNNumber").val("N");
    }
}
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SPOItmDetailsTbl", [{ "FieldId": "ItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
function onchangesrctype() {
    debugger;
    var src_type = $("#src_type").val();
    if (src_type == "P") {
        $("#SrcDocNoDiv").css("display", "");
        $("#SrcDocdtNoDiv").css("display", "");
    }
    else {
        $("#SrcDocNoDiv").css("display", "none");
        $("#SrcDocdtNoDiv").css("display", "none");
    }
    $("#Hdn_src_type").val(src_type);
}
function BindSrcDocNumber() {
    debugger;
    SuppID = $("#SupplierName").val();
    if (SuppID != null && SuppID != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/ServicePurchaseOrder/GetSourceDocList',
            data: {},
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        $("#src_doc_number option").remove();
                        $("#src_doc_number optgroup").remove();
                        $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#src_doc_number').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        //var Pending_Flag = $("#Pending_SourceDocumentLink").val();
                        //debugger
                        //if (Pending_Flag == "CreateDocument_Pending") {
                        //    var src_doc_no = $("#Pending_Src_DocNo").val()
                        //    $("#src_doc_number").val(src_doc_no).trigger('change');
                        //}
                        //else {
                        //    $("#src_doc_date").val("");
                        //}


                    }
                }
            }

        })
    }
}
function AddNewRowForEditItem() {
    var rowIdx = 0;
    debugger;
    var docid = $("#DocumentMenuId").val();

    var rowCount = $('#SPOItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#SPOItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }

    if (RowNo == "0") {
        RowNo = 1;
    }
    else {
        RowNo = RowNo + 1;
    }

    var CountRows = RowNo;

    var deletetext = $("#Span_Delete_Title").text();

    var ManualGst = "";
    var TaxExempted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();

    if (GstApplicable == "Y") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
    }
    TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
    var disableDelete = "";
    if (CountRows == 1) {
        disableDelete = "Disabled"
    }
   
    

    $('#SPOItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" ${disableDelete} aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
<td id="srno" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" /></td>
<td style="display: none;"><input class="" type="hidden" id="hfItemID" /></td>
<td class="itmStick"><div class="col-sm-12 lpo_form no-padding"><select class="form-control" id="ItemListName${CountRows}" name="ItemListName${CountRows}" onchange="OnChangePOItemName(${CountRows},event)" ></select><span id="ItemListNameError" class="error-message is-visible"></span></div></td>
<td><input id="ItemHsnCode" class="form-control date" autocomplete="off" type="text" name="Hsn"  placeholder="${$("#Hsn").text()}" disabled></td>

<td><div class="lpo_form"><input id="ord_qty" class="form-control date num_right" autocomplete="off"  onchange ="OnChangePOItemQty(${CountRows},event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="ord_qtyError${RowNo}" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="item_rate" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(${CountRows},event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="item_rateError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="item_gr_val" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"  disabled></td>
`+ TaxExempted + `
`+ ManualGst + `
<td><div class="col-sm-10 no-padding"><input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div>
<div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation" disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="Tax Calculator" data-original-title="Tax Calculator"></i></button></div></td>
<td><input id="item_oc_amt" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"  disabled></td>

<td><input id="item_net_val_sp" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_sp"  placeholder="0000.00"  disabled></td>

<td><input id="item_net_val_bs" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"  disabled></td>
<td><textarea id="remarks"  class="form-control remarksmessage" onmouseover="OnMouseOver(this)" name="remarks" maxlength = "250",  placeholder="${$("#span_remarks").text()}"   data-parsley-validation-threshold="10" ></textarea></td>
</tr>`);
    //$("#ItemListName" + CountRows).focus()
};
function OnchangeSrcDocNumber() {
    var ErrorFlag = "N";
    var SupplierName = $("#SupplierName").val();
    if (SupplierName != "" && SupplierName != null && SupplierName != "0") {
        var docid = $("#DocumentMenuId").val();
        var doc_no = $("#src_doc_number").val();
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        debugger
        if (doc_no != 0) {
            var doc_Date = $("#src_doc_number option:selected")[0].dataset.date
            var newdate = doc_Date.split("-").reverse().join("-");

            $("#src_doc_date").val(newdate);
            $("#src_doc_number").val(doc_no);
            $("#Hdn_src_doc_number").val(doc_no);

            $.ajax({
                type: 'POST',
                url: '/ApplicationLayer/ServicePurchaseOrder/GetDetailsAgainstPR',
                data: {
                    Doc_no: doc_no,
                    Doc_date: newdate,
                },
                success: function (data) {
                    debugger;
                    var arr = JSON.parse(data);


                    $("#src_type").attr("disabled", true);
                    $("#SPOItmDetailsTbl >tbody >tr").remove();

                    if (arr.Table.length > 0) {

                        for (var a = 0; a < arr.Table.length; a++) {
                            // var subitem = arr.Table[0].sub_item;
                            AddNewRowForEditItem();
                        }
                    }

                    if ($("#SPOItmDetailsTbl >tbody >tr").length == arr.Table.length) {
                        var PRCons = 'N';//Shubham 23-10-2024
                        $("#SPOItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            var SNo = currentRow.find("#SNohiddenfiled").val();
                            var DataSno;
                            DataSno = parseInt(SNo) - 1;
                            debugger;
                            currentRow.find("#ItemListName" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table[DataSno].item_id).trigger('change');
                            currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);

                            currentRow.find("#ItemHsnCode").val(arr.Table[DataSno].hsn_code);
                            currentRow.find("#ItemtaxTmplt").val(arr.Table[DataSno].tmplt_id);
                            currentRow.find("#ord_qty").val(parseFloat(arr.Table[DataSno].pr_qty).toFixed(QtyDecDigit));
                            currentRow.find("#BtnTxtCalculation" + SNo).attr("disabled", false);
                            var k = DataSno;
                            currentRow.find("#remarks" + SNo).val(arr.Table[DataSno].it_remarks);
                            var Itm_ID = arr.Table[k].item_id;
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            var docid = $("#DocumentMenuId").val();

                            if (GstApplicable == "Y") {
                                $("#HdnTaxOn").val("Item");
                                Cmn_ApplyGSTToAtable("SPOItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", arr.Table1);
                            }
                            else {
                                $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                    $("#HdnTaxOn").val("Item");
                                    $("#TaxCalcItemCode").val(Itm_ID);
                                    $("#HiddenRowSNo").val(SNo);
                                    $("#Tax_AssessableValue").val(arr.Table[k].item_gross_val);
                                    $("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                                    $("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                                    var TaxArr = arr.Table1;
                                    let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                    selected = JSON.stringify(selected);
                                    TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                    selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                    selected = JSON.stringify(selected);
                                    TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                    if (TaxArr.length > 0) {
                                        AddTaxByHSNCalculation(TaxArr);
                                        OnClickSaveAndExit();
                                        var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                        Reset_ReOpen_LevelVal(lastLevel);
                                    }
                                }

                            }


                        });

                        CalculateAmount();
                        OnChangeGrossAmt();
                    }
                    debugger
                    
                    $("#SupplierName").prop("disabled", true);
                    $("#ddlCurrency").prop("disabled", false);
                    $("#conv_rate").prop("disabled", false);
                    $("#src_doc_number").prop("disabled", true);
                    // DisableHeaderField();
                    $("#SPOItmDetailsTbl .plus_icon1").css("display", "none");
                    $("#SPOItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var Sno = currentRow.find("#SNohiddenfiled").val();
                        //currentRow.find("#delBtnIcon").css("display", "none");
                        currentRow.find("#ItemListName" + Sno).prop("disabled", true);
                    });

                     BindDelivertSchItemList();
                }
            });
        }
       

    }
    else {
        if (SupplierName == "0" || SupplierName == "---Select---") {
            $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
            $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
            $("#SpanSuppNameErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanSuppNameErrorMsg").css("display", "none");
            $("#SupplierName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
        }
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
        $("#src_doc_date").val("");
        $("#src_doc_number").val("");
    }
}
//Added by on 28-08-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var supp_id = $("#SupplierName").val();
    Cmn_SendEmail(docid, supp_id, 'Supp');
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#SPO_No").val();
    var Doc_dt = $("#SPO_Date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/ServicePurchaseOrder/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#SPO_No").val();
    var Doc_dt = $("#SPO_Date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'SPO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseOrder/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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
    var Doc_no = $("#SPO_No").val();
    var Doc_dt = $("#SPO_Date").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
function PrintFormate() {
    debugger;
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#SPO_No").val();
    var Doc_dt = $("#SPO_Date").val();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'SPO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseOrder/SavePdfDocToSendOnEmailAlert_Ext",
            data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray)},
            /*dataType: "json",*/
            success: function (data) {
                var filepath = data;
                $('#hdfilepathpdf').val(filepath)
                $("#btn_mail_print").css("display", "none");
                $("#btn_print").css("display", "");
            }
        });
    }
}
function GetPrintFormatArray() {
    return [{
        PrintFormat: $('#PrintFormat').val(),
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        ShowDeliverySchedule: $("#ShowDeliverySchedule").val(),
        ShowHsnNumber: $("#ShowHSNNumber").val(),
        ShowRemarksBlwItm: $('#hdn_ShowRemarksBlwItm').val(),
        ShowTotalQty: $("#hdn_ShowTotalQty").val(),
        ShowSupplierAliasName: $("#ShowSuppAliasName").val(),
    }];
}
//End on  28-08-2025

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