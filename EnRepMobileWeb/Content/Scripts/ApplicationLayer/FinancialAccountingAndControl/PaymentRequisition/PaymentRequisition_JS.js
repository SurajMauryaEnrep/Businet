$(document).ready(function () {
    $("#CurrencyName, #ddlRequiredArea, #ddlStatus").select2();
    var Doc_no = $("#ddlRequisitionNumber").val();
    $("#hdDoc_No").val(Doc_no);
    dbclickAllMethods();
    debugger;
    var statuscode = $("#StatusCode").val();
    if (statuscode == "C") {

        $("#CancelFlag").prop("checked", true);
    }
    CancelledRemarks("#CancelFlag", "Disabled");
  
});
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
function dbclickAllMethods() {
    $("#datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var PRData = $("#hdnFilterDataList").val();
            var WF_status = $("#DashbordPendingDocumentStatus").val();
            var clickedrow = $(e.target).closest("tr");
            var PRId = clickedrow.children("#pr_no").text();
            var PRDate = clickedrow.children("#prdateID").text();
            if (PRId != null && PRId != "") {
                window.location.href = "/ApplicationLayer/PaymentRequisition/DblClick/?PRId=" + PRId + "&PRDate=" + PRDate + "&PRData=" + PRData + "&WF_status=" + WF_status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var PR_No = clickedrow.children("#pr_no").text();
        var PR_Date = clickedrow.children("#prdateID").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(PR_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(PR_No, PR_Date, Doc_id, Doc_Status);
    });

  
        
   
}
function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text(); txtPRDate
        var brId = $("#BrId").text();
        var PRDate = $("#txtPRDate").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: PRDate
            },
            success: function (data) {
                /* if (data == "Exist") { */
                if (data == "TransAllow") { /*End to chk Financial year exist or not*/
                    var PRStatus = "";
                    PRStatus = $('#StatusCode').val().trim();
                    if (PRStatus === "D" || PRStatus === "F") {

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


function InsertPaymentReqDetail() {
    debugger;
    
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    if (CheckPRFormValidation() == false) {
        return false;
    }
}
function CheckPRFormValidation() {

    debugger;
 
    var ValidationFlag = true;
    var Flag = 'N';

    var RequirementArea = $('#ddlRequiredArea').val();
    if (RequirementArea == "" || RequirementArea == "0") {
        $('#vmRequiredArea').text($("#valueReq").text());
        $("#vmRequiredArea").css("display", "block");
        $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        $("#vmRequiredArea").css("display", "none");
        $('#vmRequiredArea').text("");
        $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
    }
    var Currency = $("#CurrencyName option:selected").val();
  
    if (Currency == "0" || Currency == "" || Currency == null) {
        $('#vmCurrname').text($("#valueReq").text());
        $("#vmCurrname").css("display", "block");
        $("#CurrencyName").css("border-color", "Red");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "Red");
        Flag = 'Y';
    }
    else {
        $("#vmCurrname").css("display", "none");
        $("#CurrencyName").css("border-color", "#ced4da");
        $('#vmCurrname').text("");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");
      
    }
    var RequestedAmount = $("#ddlRequestedAmount").val();

    if (RequestedAmount == "0" || RequestedAmount == "" || RequestedAmount == null || parseFloat(RequestedAmount) == parseFloat(0)) {
        $('#vmRequestedAmount').text($("#valueReq").text());
        $("#vmRequestedAmount").css("display", "block");
        $("#ddlRequestedAmount").css("border-color", "Red");
        Flag = 'Y';
    }
    else {
        $("#vmRequestedAmount").css("display", "none");
        $('#vmRequestedAmount').text("");
        $("#ddlRequestedAmount").css("border-color", "#ced4da");
      

    }
    var Purpose = $("#ddlPurpose").val();

    if (Purpose == "0" || Purpose == "" || Purpose == null) {
        $('#vmPurpose').text($("#valueReq").text());
        $("#vmPurpose").css("display", "block");
        $("#ddlPurpose").css("border-color", "Red");
        Flag = 'Y';
    }
    else {
        $("#vmPurpose").css("display", "none");
        $('#vmPurpose').text("");
        $("#ddlPurpose").css("border-color", "#ced4da");
    }
    var RequestedBy = $("#RequestedBy").val();

    if (RequestedBy == "0" || RequestedBy == "" || RequestedBy == null) {
        $('#vmRequestedBy').text($("#valueReq").text());
        $("#vmRequestedBy").css("display", "block");
        $("#RequestedBy").css("border-color", "Red");
        Flag = 'Y';
    }
    else {
        $("#vmRequestedBy").css("display", "none");
        $('#vmRequestedBy').text("");
        $("#RequestedBy").css("border-color", "#ced4da");
    }
    if ($("#CancelFlag").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            Flag = 'Y';
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }

    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }

    if (ValidationFlag == true) {

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
    }
    else {
        return false;
    }

}
function onChangeRequiredArea() {
    debugger;
    var RequisitionArea = $('#ddlRequiredArea').val();
    $("#vmRequiredArea").css("display", "none");
    $('#vmRequiredArea').text("");
    $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
    if (RequisitionArea != "0") {
        $("#hdRequiredArea").val(RequisitionArea);
    }

}
function onChangecurrency() {
    var Currency = $("#CurrencyName option:selected").val();
    var Currency_name = $("#CurrencyName option:selected").text().trim();
    if (Currency == "0" || Currency == "" || Currency == null) {
        $('#vmCurrname').text($("#valueReq").text());
        $("#vmCurrname").css("display", "block");
        $("#CurrencyName").css("border-color", "Red");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "Red");
      
    }
    else {
        $("#vmCurrname").css("display", "none");
        $("#CurrencyName").css("border-color", "#ced4da");
        $('#vmCurrname').text("");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");
        $("#curr_id").val(Currency);
        $("#currency_name").val(Currency_name);
    }
}
function onChangeRequestedAmount() {
    var RequestedAmount = $("#ddlRequestedAmount").val();

    if (RequestedAmount == "0" || RequestedAmount == "" || RequestedAmount == null || parseFloat(RequestedAmount) == parseFloat(0)) {
        $('#vmRequestedAmount').text($("#valueReq").text());
        $("#vmRequestedAmount").css("display", "block");
        $("#ddlRequestedAmount").css("border-color", "Red");      
       
    }
    else {
        $("#vmRequestedAmount").css("display", "none");
        $('#vmRequestedAmount').text("");
        $("#ddlRequestedAmount").css("border-color", "#ced4da");
        var amount = parseFloat(RequestedAmount) || 0;
        $("#ddlRequestedAmount").val(amount.toFixed(2));
      
    }
}
function onChangePurpose() {
    var Purpose = $("#ddlPurpose").val();

    if (Purpose == "0" || Purpose == "" || Purpose == null) {
        $('#vmPurpose').text($("#valueReq").text());
        $("#vmPurpose").css("display", "block");
        $("#ddlPurpose").css("border-color", "Red");
      
    }
    else {
        $("#vmPurpose").css("display", "none");
        $('#vmPurpose').text("");
        $("#ddlPurpose").css("border-color", "#ced4da");
    }
}
function onChangeRequestedBy() {
    var RequestedBy = $("#RequestedBy").val();

    if (RequestedBy == "0" || RequestedBy == "" || RequestedBy == null) {
        $('#vmRequestedBy').text($("#valueReq").text());
        $("#vmRequestedBy").css("display", "block");
        $("#RequestedBy").css("border-color", "Red");
      
    }
    else {
        $("#vmRequestedBy").css("display", "none");
        $('#vmRequestedBy').text("");
        $("#RequestedBy").css("border-color", "#ced4da");
    }
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var PRNo = "";
    var PRDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
 
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    PRNo = $("#ddlRequisitionNumber").val();
    PRDate = $("#txtPRDate").val();
    $("#hdDoc_No").val(PRNo);
    Remarks = $("#fw_remarks").val();
   
    var FilterDataList = $("#hdnFilterDataList").val();
    var DashbordPendingStatus = $("#DashbordPendingDocumentStatus").val();
    var TrancType = (PRNo + ',' + PRDate + ',' + "Update" + ',' + DashbordPendingStatus)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = PRNo.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PRNo, PRDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/PaymentRequisition/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PRNo != "" && PRDate != "" && level != "")
        {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PaymentRequisition/ToRefreshByJS?PRData=" + FilterDataList + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/PaymentRequisition/DocumentApprove?RequisitionNumber=" + PRNo + "&Req_date=" + PRDate +
            "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks +
            "&FilterDataList=" + FilterDataList + "&DashbordPendingStatus=" + DashbordPendingStatus;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PRNo != "" && PRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PaymentRequisition/ToRefreshByJS?PRData=" + FilterDataList + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && PRNo != "" && PRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PaymentRequisition/ToRefreshByJS?PRData=" + FilterDataList + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
function OtherFunctions(StatusC, StatusName) {

}
function AmountFloatVal(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
}
function BtnSearch() {
    debugger;
    FilterPRList();
    ResetWF_Level();
}
function FilterPRList() {
    debugger;
    try {
        var ReqArea = $("#ddlRequiredArea").val();

        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();       
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PaymentRequisition/SearchDetail",
            data: {
                req_area: ReqArea,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#ListTbl').html(data);
                $('#hdnFilterDataList').val(ReqArea + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("MRS Error : " + err.message);

    }
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate1").val();
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
            $("#txtTodate1").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ddlRequisitionNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OnClickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertPaymentReqDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
}
