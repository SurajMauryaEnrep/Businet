$(document).ready(function () {
    $("#ddlDocList").select2();
    $("#ddlDocList1").select2();
    $("#ddlDocEventList").select2();
    $("#ddlDocEventList1").select2();
    DisableAllProperties();
    var selectedEvent = $('#ddlDocEventList1').val();
    if (selectedEvent != undefined) {
        if (selectedEvent != "0") {
            BindAlertFields();
        }
    }
    EnableDisableMsgTextArea();
    $('#PAlertSetupList #datatable-buttons tbody').bind("dblclick", function (e) {
        debugger
        try {
            var clickedrow = $(e.target).closest("tr");
            var docId = clickedrow.children("#docId").text();
            var eventId = clickedrow.children("#events").text();
            var alertType = clickedrow.children("#hdAlertType").text();
            var asData = "";
            if (docId != null && eventId != "") {
                window.location.href = "/BusinessLayer/MessageSetupExternal/EditAlertSetup/?docId=" + docId + "&events=" + eventId + "&alertType=" + alertType + "&asData=" + asData;
            }
        }
        catch (exc) {
            throw exc;
        }
    });
})
function BindDocumentEventList() {
    debugger;
    var selectedDocumentId1 = $('#ddlDocList1').val();
    if (selectedDocumentId1 == "105104135115")  {
        $("#ddlDocEventList1").prop("disabled", true);
        $('#txtarea').attr('readonly', false);
        $('#txtmailSubject').attr('readonly', false);
        $('#txtHeader').attr('readonly', false);
        $('#txtBody').attr('readonly', false);
        $('#txtFooter').attr('readonly', false);
        $('#eventReqMsg').css('display', 'none');
        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'black');
    }
   else {
        var alertType = "";
        var type = "All";
        var compId = $('#CompID').text();
        var brId = $('#BrId').text();
        //change action type to remove data from ddl
        var txnType = $("#hdn_TransType").val();
        var command = $("#hdCommand").val();
        //if ((txnType == "Save" && command == "New") || (txnType == "Update" && command == "Save") || (txnType == "" && command == "") || (txnType == "Save" && command == "Refresh")) {
        //    type = "INSERT";
        //}
        if (txnType == undefined && command == undefined) {
            type = "All";
        }
        else {
            type = "Insert";
            if ($('#ddlAlertType').val() == "mail") {
                alertType = "mail";
            }
        }
        /*alert(txnType + ' ' + command);*/
        var selectedDocumentId = $('#ddlDocList').val();
       
        var value = "0";
        if (selectedDocumentId != undefined) {
            value = selectedDocumentId;
        }
        else {
            value = selectedDocumentId1;
        }

        $.ajax({
            type: "POST",
            url: "/BusinessLayer/MessageSetupExternal/BindDocumentEvents",
            data: { documentId: value, ddlType: type, alertType: alertType, compId: compId, brId: brId },
            success: function (data) {
                debugger
                var arr = []
                arr = JSON.parse(data);
                var option = "";
                if (type == "All") {
                    option += `<option value="0">All</option>`;
                }
                else {
                    option += `<option value="0">--Select--</option>`;
                }
                for (var i = 0; i < arr.length; i++) {

                    option += `<option value="${arr[i]["status_code"].trim()}">${arr[i]["status_name"]}</option>`;
                }
                if (selectedDocumentId != undefined) {
                    $("#ddlDocEventList").html(option);
                }
                else {
                    $("#ddlDocEventList1").html(option);
                }
            }
        });
        /*ValidateForm();*/
    }
   
}
function FilterListCust() {
    var selectedDocumentId = $('#ddlDocList').val();
    var selectedEvent = $('#ddlDocEventList').val();
    var alertType = $('#ddlAlertType1').val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/MessageSetupExternal/SearchMessageSetupDetail",
        data: {
            docId: selectedDocumentId,
            events: selectedEvent,
            alertType: alertType,
        },
        success: function (data) {
            $('#PAlertSetupList').empty();
            $('#PAlertSetupList').html(data);
        }
    });
}
function validateDdlBeforeSearch() {
    //debugger;
    //var selectedDocumentId = $('#ddlDocList').val();
    ///*var selectedEvent = $('#ddlDocEventList').val();*/
    //if (selectedDocumentId == '0') {
    //    $('#docIdvalidate').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
    //}
    //else {
    //    $('#docIdvalidate').css('display', 'none');
    //    /*FilterListCust();*/
    //}
}
function attachmentCheckedChange() {
    if ($("#chkno").attr("checked", "checked")) {
        $('#hdnAttach').val("No");
    }
    else {
        $('#hdnAttach').val("Yes");
    }
}
function checkBoxYesClick() {
    $("#chkno").attr("checked", false);
    $("#chkyes").attr("checked", true);
}
function checkBoxNoClick() {
    $("#chkno").attr("checked", true);
    $("#chkyes").attr("checked", false);
}
function BindAlertFields() {
    debugger;
    var selectedDocumentId1 = $('#ddlDocList1').val();
    var alertType = $('#ddlAlertType').val();
    var list = $('#ulDocFields');
    list.empty();
    if (selectedDocumentId1 == "0" || alertType == "0") {
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/MessageSetupExternal/GetAlertFields",
        data: { documentId: selectedDocumentId1, brId: '', alertType: alertType },
        //dataType: "json",
        success: function (data) {
            debugger
            list.empty();
            var arr = []
            arr = JSON.parse(data);
            var html = "";
            for (var i = 0; i < arr.length; i++) {
                /*html += "<option value=" + arr[i]["status_code"] + ">" + arr[i]["status_name"] + "</option>"*/
                list.append(` <li class="draggable nav-item" draggable="true" ondragstart="drag(event)">${arr[i]["docFieldName"]}</li>`);
            }
        }
    });
    /*ValidateForm();*/
}
function ValidateSubject() {
    debugger;
    var mailSubject = $('#txtmailSubject').val();
    if (mailSubject != "") {
        if ($.trim(mailSubject) == "" || mailSubject == undefined) {
            $('#subjectReqMsg').html($('#valueReq').text()).css("color", "red");
            $('#subjectReqMsg').css('display', 'block');
            $('#txtmailSubject').css('border-color', 'red');
        }
        else {
            $('#subjectReqMsg').css('display', 'none');
            $('#txtmailSubject').css('border-color', 'black');

        }
    }
}
function ValidateHeader() {
    var mailHeader = $('#txtHeader').val();
    //var submitCount = $('#hfSubmitCount').val();
    if (mailHeader != "") {
        if (mailHeader == "" || mailHeader == undefined) {
            $('#headerReqMsg').html($('#valueReq').text()).css("color", "red");
            $('#headerReqMsg').css('display', 'block');
            $('#txtHeader').css('border-color', 'red');

        }
        else {
            $('#headerReqMsg').css('display', 'none');
            $('#txtHeader').css('border-color', 'black');
        }
    }
}
function ValidateBody() {
    var mailBody = $('#txtBody').val();
    //var submitCount = $('#hfSubmitCount').val();
    if (mailBody != "") {
        if (mailBody == "" || mailBody == undefined) {
            $('#bodyReqMsg').html($('#valueReq').text()).css("color", "red");
            $('#bodyReqMsg').css('display', 'block');
            $('#txtBody').css('border-color', 'red');

        }
        else {
            $('#bodyReqMsg').css('display', 'none');
            $('#txtBody').css('border-color', 'black');
        }
    }
}
function ValidateFooter() {
    var mailFooter = $('#txtFooter').val();
    //var submitCount = $('#hfSubmitCount').val();
    if (mailFooter != "") {
        if (mailFooter == "" || mailFooter == undefined) {
            $('#footerReqMsg').html($('#valueReq').text()).css("color", "red");
            $('#footerReqMsg').css('display', 'block');
            $('#txtFooter').css('border-color', 'red');

        }
        else {
            $('#footerReqMsg').css('display', 'none');
            $('#txtFooter').css('border-color', 'black');
        }
    }
}
function EnableDisableMsgTextArea() {
    debugger;
    var events = $('#ddlDocEventList1').val();
    var docList = $('#ddlDocList1').val();
    var alertType = $('#ddlAlertType').val();
    if (docList == '105104135115') {
        $('#txtarea').attr('readonly', false);
        $('#txtmailSubject').attr('readonly', false);
        $('#txtHeader').attr('readonly', false);
        $('#txtBody').attr('readonly', false);
        $('#txtFooter').attr('readonly', false);
    }
    else {
        if (events != '0' && docList != '0' && alertType != '0') {
            if ($.trim(events) != "0") {
                $('#txtarea').attr('readonly', false);
                $('#txtmailSubject').attr('readonly', false);
                $('#txtHeader').attr('readonly', false);
                $('#txtBody').attr('readonly', false);
                $('#txtFooter').attr('readonly', false);
            }
            else {
                $('#txtarea').attr('readonly', true).val("");
                $('#txtmailSubject').attr('readonly', true).val("");
                $('#txtHeader').attr('readonly', true).val("");
                $('#txtBody').attr('readonly', true).val("");
                $('#txtFooter').attr('readonly', true).val("");
            }
        }
        else {
            $('#txtarea').attr('readonly', true).val("");
            $('#txtmailSubject').attr('readonly', true).val("");
            $('#txtHeader').attr('readonly', true).val("");
            $('#txtBody').attr('readonly', true).val("");
            $('#txtFooter').attr('readonly', true).val("");
        }
    }
    var txnType = $('#hdn_TransType').val();
    if (txnType == "Update") {
        $('#txtarea').attr('readonly', false);
        $('#txtmailSubject').attr('readonly', false);
        $('#txtHeader').attr('readonly', false);
        $('#txtBody').attr('readonly', false);
        $('#txtFooter').attr('readonly', false);
    }
}
function ValidateForm() {
    debugger;
    var btn = $("#hdnSavebtn").val(); 
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); 
    }
    var count = 0;
    var alertType = $('#ddlAlertType').val();
        count = ValidateEmailForm();
   
    if (count > 0) {
        return false;
    }
    else {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function ValidateEmailForm() {
    var count = 0;
    debugger;
    var docName = $('#ddlDocList1').val();
    var eventName = $('#ddlDocEventList1').val();
    var alertType = $('#ddlAlertType').val();
    var mailSubject = $('#txtmailSubject').val();
    var mailHeader = $('#txtHeader').val();
    var mailBody = $('#txtBody').val();
    var mailFooter = $('#txtFooter').val();

    if ($.trim(docName) == "0" || docName == undefined) {
        $('#docIdReqMsg').html($('#valueReq').text()).css("color", "red");
        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'red');
        $('#docIdReqMsg').css('display', 'block');
        count = count + 1;
    }
    else {
        $('#docIdReqMsg').css('display', 'none');
        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'black');
    }
    if (($.trim(eventName) == "0" && docName != '105104135115') || eventName == undefined) {
        $('#eventReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#eventReqMsg').css('display', 'block');
        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'red');
        $('#txtarea').attr('readonly', true).val("");
        $('#txtmailSubject').attr('readonly', true).val("");
        $('#txtHeader').attr('readonly', true).val("");
        $('#txtBody').attr('readonly', true).val("");
        $('#txtFooter').attr('readonly', true).val("");

        count = count + 1;
    }
    else {
        $('#txtarea').attr('readonly', false);
        $('#txtmailSubject').attr('readonly', false);
        $('#txtHeader').attr('readonly', false);
        $('#txtBody').attr('readonly', false);
        $('#txtFooter').attr('readonly', false);
        $('#eventReqMsg').css('display', 'none');
        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'black');

    }
    if ($.trim(alertType) == "0" || alertType == undefined) {
        $('#alertTypeReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#ddlAlertType').css('border-color', 'red');
        $('#alertTypeReqMsg').css('display', 'block');
        count = count + 1;
    }
    else {
        $('#alertTypeReqMsg').css('display', 'none');
        $('#ddlAlertType').css('border-color', 'black');
    }
    if ($.trim(mailSubject) == "" || mailSubject == undefined) {
        $('#subjectReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#subjectReqMsg').css('display', 'block');
        $('#txtmailSubject').css('border-color', 'red');
        /*$('#txtmailSubject').attr('readonly', true).val("");*/
        count = count + 1;
    }
    else {
        /*$('#txtmailSubject').attr('readonly', false);*/
        $('#subjectReqMsg').css('display', 'none');
        $('#txtmailSubject').css('border-color', 'black');

    }
    if (mailHeader == "" || mailHeader == undefined) {
        $('#headerReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#headerReqMsg').css('display', 'block');
        $('#txtHeader').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#headerReqMsg').css('display', 'none');
        $('#txtHeader').css('border-color', 'black');
    }
    if (mailBody == "" || mailBody == undefined) {
        $('#bodyReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#bodyReqMsg').css('display', 'block');
        $('#txtBody').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#bodyReqMsg').css('display', 'none');
        $('#txtBody').css('border-color', 'black');
    }
    if (mailFooter == "" || mailFooter == undefined) {
        $('#footerReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#footerReqMsg').css('display', 'block');
        $('#txtFooter').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#footerReqMsg').css('display', 'none');
        $('#txtFooter').css('border-color', 'black');
    }
    return count;
}
//function ValidateWappSmsDbForm() {
//    debugger;
//    var docName = $('#ddlDocList1').val();
//    var eventName = $('#ddlDocEventList1').val();
//    var alertType = $('#ddlAlertType').val();
//    var textMsg = $('#txtarea').val();
//    var count = 0;

//    if ($.trim(docName) == "0" || docName == undefined) {
//        $('#docIdReqMsg').html($('#valueReq').text()).css("color", "red");
//        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'red');
//        $('#docIdReqMsg').css('display', 'block');
//        count = count + 1;
//    }
//    else {
//        $('#docIdReqMsg').css('display', 'none');
//        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'black');
//    }
//    if ($.trim(eventName) == "0" || eventName == undefined) {
//        $('#eventReqMsg').html($('#valueReq').text()).css("color", "red");
//        $('#eventReqMsg').css('display', 'block');
//        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'red');
//        $('#txtarea').attr('readonly', true).val("");
//        $('#txtmailSubject').attr('readonly', true).val("");
//        $('#txtHeader').attr('readonly', true).val("");
//        $('#txtBody').attr('readonly', true).val("");
//        $('#txtFooter').attr('readonly', true).val("");

//        count = count + 1;
//    }
//    else {
//        $('#txtarea').attr('readonly', false);
//        $('#txtmailSubject').attr('readonly', false);
//        $('#txtHeader').attr('readonly', false);
//        $('#txtBody').attr('readonly', false);
//        $('#txtFooter').attr('readonly', false);
//        $('#eventReqMsg').css('display', 'none');
//        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'black');

//    }
//    if (textMsg == "" || textMsg == undefined) {
//        $('#textAreaReqMsg').html($('#valueReq').text()).css("color", "red");
//        $('#textAreaReqMsg').css('display', 'block');
//        $('#txtarea').css('border-color', 'red');
//        count = count + 1;
//    }
//    else {
//        $('#textAreaReqMsg').css('display', 'none');
//        $('#txtarea').css('border-color', 'black');
//    }
//    return count;
//}
function validatedocument() {
    debugger;
    var docName = $('#ddlDocList1').val();

    var count = 0;
    if ($.trim(docName) == "0" || docName == undefined) {
        $('#docIdReqMsg').html($('#valueReq').text()).css("color", "red");
        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'red');
        $('#docIdReqMsg').css('display', 'block');
        count = count + 1;
    }
    else {
        $('#docIdReqMsg').css('display', 'none');
        $('[aria-labelledby="select2-ddlDocList1-container"]').css('border-color', 'black');
    }

    if (count > 0) {
        return false;
    }
}
function validateevent() {
    debugger;
    var eventName = $('#ddlDocEventList1').val();
    var textMsg = $('#txtarea').val();
    var count = 0;
    if ($.trim(eventName) == "0" || eventName == undefined) {
        $('#eventReqMsg').html($('#valueReq').text()).css("color", "red");
        $('#eventReqMsg').css('display', 'block');
        $('#txtarea').attr('readonly', true).val("");
        $('#txtmailSubject').attr('readonly', true).val("");
        $('#txtHeader').attr('readonly', true).val("");
        $('#txtBody').attr('readonly', true).val("");
        $('#txtFooter').attr('readonly', true).val("");
        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'red');
        $('#txtarea').attr('readonly', true).val("");
        count = count + 1;
    }
    else {
        $('#txtarea').attr('readonly', false);
        $('#txtmailSubject').attr('readonly', false);
        $('#txtHeader').attr('readonly', false);
        $('#txtBody').attr('readonly', false);
        $('#txtFooter').attr('readonly', false);
        $('#eventReqMsg').css('display', 'none');
        $('[aria-labelledby="select2-ddlDocEventList1-container"]').css('border-color', 'black');
    }
    if (count > 0) {
        return false;
    }
}
function ValidateReqArea() {
    debugger;
    var submitCount = $('#hfSubmitCount').val();
    if (submitCount != "0") {
        var docName = $('#ddlDocList1').val();
        var eventName = $('#ddlDocEventList1').val();

        var textMsg = $('#txtarea').val();
        var count = 0;
        if (textMsg == "" || textMsg == undefined) {
            $('#textAreaReqMsg').html($('#valueReq').text()).css("color", "red");
            $('#textAreaReqMsg').css('display', 'block');
            $('#txtarea').css('border-color', 'red');
            count = count + 1;
        }
        else {
            $('#textAreaReqMsg').css('display', 'none');
            $('#txtarea').css('border-color', 'black');
        }
        if (count > 0) {
            return false;
        }
    }
}
function DisableAllProperties() {
    var txnType = $("#hdn_TransType").val();
    var command = $("#hdCommand").val();
    /*alert(txnType + '  ' + command);*/
    if (txnType == "Update" && command == "Add") {
        $('#ddlDocList1').attr('disabled', true);
        $('#ddlDocEventList1').attr('disabled', true);
        $('#ddlAlertType').attr('disabled', true);
        $('#txtmailSubject').attr('disabled', true);
        $('#txtHeader').attr('disabled', true);
        $('#txtBody').attr('disabled', true);
        $('#txtFooter').attr('disabled', true);
        $('#txtarea').attr('disabled', true);
        $('#ulDocFields').attr('disabled', true);
    }
    else if (txnType == "Update" && command == "Save") {
        $('#ddlDocList1').attr('disabled', true);
        $('#ddlDocEventList1').attr('disabled', true);
        $('#txtarea').attr('disabled', true);
        $('#ulDocFields').attr('disabled', true);
        $('#ddlAlertType').attr('disabled', true);
        $('#txtmailSubject').attr('disabled', true);
        $('#txtHeader').attr('disabled', true);
        $('#txtBody').attr('disabled', true);
        $('#txtFooter').attr('disabled', true);
    }
    else if (txnType == "Save" && command == "Refresh") {
        $('#ddlDocList1').attr('disabled', true);
        $('#ddlDocEventList1').attr('disabled', true);
        $('#txtarea').attr('disabled', true);
        $('#ulDocFields').attr('disabled', true);
        $('#ddlAlertType').attr('disabled', true);
        $('#txtmailSubject').attr('disabled', true);
        $('#txtHeader').attr('disabled', true);
        $('#txtBody').attr('disabled', true);
        $('#txtFooter').attr('disabled', true);
    }
    else if (txnType == "Refresh" && command == "Delete") {
        $('#ddlDocList1').attr('disabled', true);
        $('#ddlDocEventList1').attr('disabled', true);
        $('#txtarea').attr('disabled', true);
        $('#ulDocFields').attr('disabled', true);
        $('#ddlAlertType').attr('disabled', true);
        $('#txtmailSubject').attr('disabled', true);
        $('#txtHeader').attr('disabled', true);
        $('#txtBody').attr('disabled', true);
        $('#txtFooter').attr('disabled', true);
    }
    else if (txnType == "Update" && command == "Edit") {
        $('#ddlDocList1').attr('disabled', true);
        $('#ddlDocEventList1').attr('disabled', true);
        $('#txtarea').attr('disabled', false);
        $('#ulDocFields').attr('disabled', false);
        $('#ddlAlertType').attr('disabled', true);
        $('#txtmailSubject').attr('disabled', false);
        $('#txtHeader').attr('disabled', false);
        $('#txtBody').attr('disabled', false);
        $('#txtFooter').attr('disabled', false);
    }
    else {
        $('#ddlDocList1').attr('disabled', false);
        $('#ddlDocEventList1').attr('disabled', false);
        $('#txtarea').attr('disabled', false);
        $('#ulDocFields').attr('disabled', false);
        $('#ddlAlertType').attr('disabled', false);
        $('#txtmailSubject').attr('disabled', false);
        $('#txtHeader').attr('disabled', false);
        $('#txtBody').attr('disabled', false);
        $('#txtFooter').attr('disabled', false);
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
            $("#hfDeletecommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
$('#btn_save').click(function () {
    $('#hfSubmitCount').val("1");
    /*alert($('#hfSubmitCount').val());*/
});