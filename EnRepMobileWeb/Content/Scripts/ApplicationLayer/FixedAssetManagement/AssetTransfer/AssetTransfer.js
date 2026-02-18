const ValDecDigit = $("#ValDigit").text();///Amount
$(document).ready(function () {
    $("#ddlAssetDescription").select2();
    $("#ddlAssetDescriptionList").select2();
    $("#ddlAssetSerialNo").select2();
    GetAssignedRequirementArea();
    GetDestinationAssignedRequirementArea();
    $("#ddlRequirementArea").select2();
    $("#ddlAssignedRequirementArea").select2();
   
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#Doc_no").val() == "" || $("#Doc_no").val() == null) {
        $("#Doc_date").val(CurrentDate);
    }
    if ($("#hfPIStatus").val() == "" || $("#hfPIStatus").val() == null) {
        $("#TransferDate").val(CurrentDate);
    }
    $("#Tbl_list_FA_AT #datatable-buttons tbody").bind("dblclick", function (e) {
         var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        if (InvoiceNo != "" && InvoiceNo != null) {
            window.location.href = "/ApplicationLayer/AssetTransfer/AddAssetTransferDetail?DocNo=" + InvoiceNo + "&DocDt=" + InvDate + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#Tbl_list_FA_AT #datatable-buttons tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(InvoiceNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(InvoiceNo, InvDate, Doc_id, Doc_Status);
    });
    var InvoiceNo = $("#Doc_no").val();
    $("#hdDoc_No").val(InvoiceNo);
});
function BtnSearch() {
    try {
        var AssetId = $("#ddlAssetDescriptionList").val();
        var RequirementArea = $("#ddlAssignedRequirementArea").val();
        var Status = $("#ddlStatus").val();
        var FromDate = $("#hdFromdate").val();
        var ToDate = $("#txtTodate").val();
        $("#Hdn_AssignedRequirementAreaId").val(RequirementArea);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetTransfer/ATListSearch",
            data: {
                AssetId: AssetId, RequirementArea: RequirementArea, Status: Status, FromDate: FromDate, ToDate: ToDate
            },
            success: function (data) {
                $('#Tbl_list_FA_AT').html(data);
                $('#ListFilterData').val(AssetId + ',' + RequirementArea + ',' + Status + ',' + FromDate + ',' + ToDate);
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    }
    catch (err) {
        console.log("PQA Error : " + err.message);
    }
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
/***-------------------For Workflow Start----------------***/
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
function ForwardBtnClick() {
    debugger;
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DrPinvDate = $("#Doc_date").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: DrPinvDate
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var INVStatus = "";
                    INVStatus = $('#hfPIStatus').val().trim();
                    if (INVStatus === "D" || INVStatus === "F") {

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
                    /*   swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
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
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var INV_NO = "";
    var INVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#Doc_no").val();
    INVDate = $("#Doc_date").val();
    WF_Status1 = $("#WF_Status1").val();
    //var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    var TrancType = (INV_NO + ',' + INVDate + ',' + "Update" + ',' + WF_Status1 + ',' + docid)
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();
    var ListFilterData1 = $("#ListFilterData1").val();

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
        var pdfAlertEmailFilePath = INV_NO.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/AssetTransfer/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/AssetTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
            //window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/AssetTransfer/ApproveATDetails?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + '';
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/AssetTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/AssetTransfer/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
            //window.location.reload();
        }
    }
}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#Doc_no").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        var Doc_Status = $("#hfPIStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "POItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
/***-------------------For Workflow End----------------***/
function OnChangeddlAssetDescription(ACID) {
    if ($('#ddlAssetDescription').val() != '') {
        $('[aria-labelledby="select2-ddlAssetDescription-container"]').css("border-color", "#ced4da");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("#Hdn_AssetItemsId").val($('#ddlAssetDescription').val());
    }
    GetAssetSerialNo();
    $('#AssetLabel').val("");
    //GetAssignedRequirementArea();
    //GetDestinationAssignedRequirementArea();
    $('#ddlAssignedRequirementArea').val(0).trigger("change");
    $('#ddlDestinationAssignedRequirementArea').val(0).trigger("change");
   // $('#Remarks').val("");
}
function GetAssetSerialNo() {
    try {
        var AssetDescriptionId = $('#Hdn_AssetItemsId').val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetTransfer/GetSerialNoJS",
            data: {
                AssetDescriptionId: AssetDescriptionId,
            },
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlAssetSerialNo").empty();
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#ddlAssetSerialNo').append(`<option value="${data.Table[i].serial_noid}">${data.Table[i].serial_no}</option>`);
                        }
                    }
                    else {
                        $('#ddlAssetSerialNo').append(`<option value=0>---Select---</option>`);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssetSerialNo Error: " + err.message); // Handle any JavaScript errors
    }
}
function OnChangeddlAssetSerialNo(ACID) {
    if ($('#ddlAssetSerialNo').val().trim() != '') {
        $('[aria-labelledby="select2-ddlAssetSerialNo-container"]').css("border-color", "#ced4da");
        $("#SpanSerialNumberErrorMsg").css("display", "none");
        $("#Hdn_SerialNumber").val($('#ddlAssetSerialNo').val().trim());
    }
    try {
        var SerialNo = $('#ddlAssetSerialNo').val().trim();
        var AssetDescriptionId = $('#Hdn_AssetItemsId').val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetTransfer/GetLabelJS",
            data: {
                AssetDescriptionId: AssetDescriptionId,
                SerialNo: SerialNo,
            },
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#AssetLabel').val(data.Table[i].asset_label);
                            $('#ddlAssignedRequirementArea').val(data.Table[i].assign_req_area).trigger("change");
                            $("#Hdn_AssignedRequirementAreaType").val(data.Table[i].assign_req_area_type);
                           
                        }
                    }
                    else {
                        $('#AssetLabel').val("");
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssetLabel Error: " + err.message); // Handle any JavaScript errors
    }
}
function OnChangeAssetLabel(ACID) {
    if ($('#AssetLabel').val().trim() != '') {
        $('#AssetLabel').css("border-color", "#ced4da");
        $("#SpanAssetLabelErrorMsg").css("display", "none");
    }
}
function OnChangeddlAssignedRequirementArea(ACID) {
     if ($('#ddlAssignedRequirementArea').val() != '') {
        $('[aria-labelledby="select2-ddlAssignedRequirementArea-container"]').css("border-color", "#ced4da");
        $("#spanddlassignedrequirementareaerrormsg").css("display", "none");
        $("#Hdn_AssignedRequirementAreaId").val($('#ddlAssignedRequirementArea').val());
        //var ratype = $("#ddlassignedrequirementarea option:selected")[0].dataset.ratype
        //$("#hdn_assignedrequirementareatype").val(ratype);
    }
}
function GetAssignedRequirementArea() {
    try {
       $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetTransfer/GetAssignedRequirementArea",
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlAssignedRequirementArea").empty();
                        var uniqueOptGroupId = "Textddl_" + new Date().getTime();
                        $('#ddlAssignedRequirementArea').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#span_RequirementArea").text()}' data-ratype='${$("#span_Type").text()}'></optgroup>`);
                        // Append options to the dropdown
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#' + uniqueOptGroupId).append(`<option data-invdate="${data.Table[i].acc_id}" data-ratype="${data.Table[i].RAType}" value="${data.Table[i].acc_id}">${data.Table[i].acc_name}</option>`);
                        }
                        $('#ddlAssignedRequirementArea').select2({
                            templateResult: function (data) {
                                var PInvDate = $(data.element).data('ratype');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                            }
                        });
                        var rq_id = $("#Hdn_AssignedRequirementAreaId").val();
                        if (rq_id != 'undefined' && rq_id != '0' && rq_id != '') {
                        $('#ddlAssignedRequirementArea').val(rq_id).trigger("change");
                        }
                    }
                    else {

                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssignedRequirementArea Error: " + err.message); // Handle any JavaScript errors
    }
}
function GetDestinationAssignedRequirementArea() {
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssetTransfer/GetDestinationAssignedRequirementArea",
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlDestinationAssignedRequirementArea").empty();
                        var uniqueOptGroupId = "Textddl_" + new Date().getTime();
                        $('#ddlDestinationAssignedRequirementArea').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#span_RequirementArea").text()}' data-ratype='${$("#span_Type").text()}'></optgroup>`);
                        // Append options to the dropdown
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#' + uniqueOptGroupId).append(`<option data-invdate="${data.Table[i].acc_id}" data-ratype="${data.Table[i].RAType}" value="${data.Table[i].acc_id}">${data.Table[i].acc_name}</option>`);
                        }
                        $('#ddlDestinationAssignedRequirementArea').select2({
                            templateResult: function (data) {
                                let idd = $("#ddlAssignedRequirementArea").val();
                                console.log('idd ' + idd);
                                if (idd != 'undefined' && idd != '0' && idd != '' && idd != data.id) {
                                var PInvDate = $(data.element).data('ratype');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                }
                            }
                        });
                        var rq_id = $("#Hdn_DestinationAssignedRequirementAreaId").val();
                        if (rq_id != 'undefined' && rq_id != '0' && rq_id != '') {
                            $('#ddlDestinationAssignedRequirementArea').val(rq_id).trigger("change");
                        }
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetAssignedRequirementArea Error: " + err.message); // Handle any JavaScript errors
    }
}
function OnChangeddlDestinationAssignedRequirementArea(ACID) {
    if ($('#ddlDestinationAssignedRequirementArea').val() != '') {
        $('[aria-labelledby="select2-ddlDestinationAssignedRequirementArea-container"]').css("border-color", "#ced4da");
        $("#SpanddlDestinationAssignedRequirementAreaErrorMsg").css("display", "none");
        $("#Hdn_DestinationAssignedRequirementAreaId").val($('#ddlDestinationAssignedRequirementArea').val());
        //var ratype = $("#ddlDestinationAssignedRequirementArea option:selected")[0].dataset.ratype
        //$("#Hdn_DestinationAssignedRequirementAreaType").val(ratype);
    }
}
function CheckAT_Validations() {
    var ErrorFlag = "N";
    if ($("#ddlAssetDescription").val() === "0") {
        $('#SpanddlAssetDescriptionErrorMsg').text($("#valueReq").text());
        $("#ddlAssetDescription").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "red");
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlAssetDescriptionErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetDescription-container']").css("border-color", "#ced4da");
        $("#ddlAssetDescription").css("border-color", "#ced4da");
    }
    if ($("#ddlAssetSerialNo").val() == "0") {
        $('#SpanSerialNumberErrorMsg').text($("#valueReq").text());
        $("#SpanSerialNumberErrorMsg").css("display", "block");
        $("#ddlAssetSerialNo").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlAssetSerialNo-container']").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSerialNumberErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlAssetSerialNo-container']").css("border-color", "#ced4da");
        $("#ddlAssetSerialNo").css("border-color", "#ced4da");
    }
    if ($("#ddlDestinationAssignedRequirementArea").val() === "0") {
        $('#SpanddlDestinationAssignedRequirementAreaErrorMsg').text($("#valueReq").text());
        $("#ddlDestinationAssignedRequirementArea").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlDestinationAssignedRequirementArea-container']").css("border-color", "red");
        $("#SpanddlDestinationAssignedRequirementAreaErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlDestinationAssignedRequirementAreaErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlDestinationAssignedRequirementArea-container']").css("border-color", "#ced4da");
        $("#ddlDestinationAssignedRequirementArea").css("border-color", "#ced4da");
    }
    debugger;
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}
function SaveBtnClick() {
    var HeaderValidation = CheckAT_Validations();
    if (HeaderValidation == false) {
        return false;
    }
}