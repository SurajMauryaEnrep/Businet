$(document).ready(function () {
    //make all dropdowns as searchable
    $('#ddldoclistemail,#ddleventlistemail,#ddlmailuserstype,#ddlmailusers,#ddldoclistsms,#ddleventlistsms,#ddlsmsuserstype,#ddlsmsusers,#ddldoclistwhatsapp,#ddleventlistwhatsapp,#ddlwhatsappuserstype,#ddlwhatsappusers,#ddldoclistdashboard,#ddleventlistdashboard,#ddldashboarduserstype,#ddldashboardusers').select2();
    EnableDisableTab();
});
function setTabId(val) {
    $('#hfactivetab').val(val);
}
function EnableDisableTab() {
    debugger;
    var activeTab = $('#hfactivetab').val();
    if (activeTab == "2" || activeTab == "3" || activeTab == "4") {
        $('#collapseOne').removeClass('show');
    }
    if (activeTab == "1") {
        $('#collapseOne').addClass('show');
    }
    else if (activeTab == "2") {
        $('#collapseTwo').addClass('show');
    }
    else if (activeTab == "3") {
        $('#collapseThree').addClass('show');
    }
    else if (activeTab == "4") {
        $('#collapseFour').addClass('show');
    }
    else {
        $('#collapseOne').addClass('show');
    }

}
function BindDocEventList(dropdown) {
    debugger;
    var compId = $('#CompID').text();
    var brId = $('#BrId').text();
    /*var type = "All";*/
    /*var compId = $('#CompID').text();*/
    var documentid = "0";
    if (dropdown == "mail") {
        documentid = $('#ddldoclistemail').val();
    }
    else if (dropdown == "sms") {
        documentid = $('#ddldoclistsms').val();
    }
    else if (dropdown == "whatsapp") {
        documentid = $('#ddldoclistwhatsapp').val();
    }
    else {
        documentid = $('#ddldoclistdashboard').val();
    }


    $.ajax({
        type: "POST",
        url: "/BusinessLayer/DocumentSetup/BindDocumentEvents",
        data: { documentId: documentid, ddlType: dropdown },
        success: function (data) {
            debugger
            var arr = []
            arr = JSON.parse(data);
            /*arr[0]["status_name"] = arr[0]["status_name"] = "--Select--";*/
            var option =  `<option value="0">--Select--</option>`;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]["status_name"] != "All") {
                    option += `<option value="${arr[i]["status_code"]}">${arr[i]["status_name"]}</option>`;
                }
            }
            if (dropdown == "mail") {
                    $('#ddleventlistemail').html(option).trigger("change");
            }
            else if (dropdown == "sms") {
                    $('#ddleventlistsms').html(option).trigger("change");
            }
            else if (dropdown == "whatsapp") {
                    $('#ddleventlistwhatsapp').html(option).trigger("change");
            }
            else {
                    $('#ddleventlistdashboard').html(option).trigger("change");
            }
        }
    });
    /*ValidateForm();*/
}
function BindBindRcptTypeList(dropdown) {
    debugger;
    var documentid = "0";
    var events = "0";
    /*var documentid = "0";*/
    if (dropdown == "mail") {
        documentid = $('#ddldoclistemail').val();
        events = $('#ddleventlistemail').val();
        events = $('#ddleventlistemail').val();
    }
    else if (dropdown == "sms") {
        documentid = $('#ddldoclistsms').val();
        events = $('#ddleventlistsms').val();
    }
    else if (dropdown == "whatsapp") {
        documentid = $('#ddldoclistwhatsapp').val();
        events = $('#ddleventlistwhatsapp').val();
    }
    else {
        documentid = $('#ddldoclistdashboard').val();
        events = $('#ddleventlistdashboard').val();
    }


    $.ajax({
        type: "POST",
        url: "/BusinessLayer/DocumentSetup/BindRcptType",
        data: { document: documentid, events: events, alertType: dropdown },
        success: function (data) {
            debugger
            var arr = []
            arr = JSON.parse(data);
            /*arr[0]["status_name"] = arr[0]["status_name"] = "--Select--";*/
            var option = "";
            for (var i = 0; i < arr.length; i++) {
                option += `<option value="${arr[i]["value"]}">${arr[i]["text"]}</option>`;
            }
            if (dropdown == "mail") {
                $('#ddlmailuserstype').html(option).trigger("change");
            }
            else if (dropdown == "sms") {
                $('#ddlsmsuserstype').html(option).trigger("change");
            }
            else if (dropdown == "whatsapp") {
                $('#ddlwhatsappuserstype').html(option).trigger("change");
            }
            else {
                $('#ddldashboarduserstype').html(option).trigger("change");
            }
        }
    });
    /*ValidateForm();*/
}
function BindUserList(dropdown) {
    debugger;
    /*var compId = $('#CompID').text();*/
    var usersType = "0";
    var documentid = "0";
    var events = "0";
    if (dropdown == "mail") {
        documentid = $('#ddldoclistemail').val();
        usersType = $('#ddlmailuserstype').val();
        events = $('#ddleventlistemail').val();
    }
    else if (dropdown == "sms") {
        documentid = $('#ddldoclistsms').val();
        usersType = $('#ddlsmsuserstype').val();
        events = $('#ddleventlistsms').val();
    }
    else if (dropdown == "whatsapp") {
        documentid = $('#ddldoclistwhatsapp').val();
        usersType = $('#ddlwhatsappuserstype').val();
        events = $('#ddleventlistwhatsapp').val();
    }
    else {
        documentid = $('#ddldoclistdashboard').val();
        usersType = $('#ddldashboarduserstype').val();
        events = $('#ddleventlistdashboard').val();
    }

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/DocumentSetup/BindUserList",
        data: { docId: documentid, userType: usersType, alertType: dropdown, events: events },
        success: function (data) {
            debugger
            var arr = []
            arr = JSON.parse(data);
            var option = "<option value='0' >--Select--</option>";
            for (var i = 0; i < arr.length; i++) {
                option += `<option value="${arr[i]["user"]}">${arr[i]["user_nm"]}</option>`;
            }
            if (dropdown == "mail") {
                $('#ddlmailusers').html(option);
            }
            else if (dropdown == "sms") {
                $('#ddlsmsusers').html(option);
            }
            else if (dropdown == "whatsapp") {
                $('#ddlwhatsappusers').html(option);
            }
            else {
                $('#ddldashboardusers').html(option);
            }
        }
    });
    /*ValidateForm();*/
}
/*Mail Data Validations*/
function EnableDisableMailReqMsg() {
    var count = 0;
    debugger;
    $('#hfSubmitCount').val("1");
    /*var docId = $('#ddldoclistemail').val();*/
    if (docId == undefined || docId == "0") {
        $('#vmMailDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldoclistemail').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailDocId').html('').css('display', 'none');
        $('#ddldoclistemail').css('border-color', 'gray');
    }
    var docId = $('#ddleventlistemail').val();
    if (docId == undefined || docId == "0") {
        $('#vmMailEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddleventlistemail').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailEventId').html('').css('display', 'none');
        $('#ddleventlistemail').css('border-color', 'gray');
    }
    var docId = $('#ddlmailuserstype').val();
    if (docId == undefined || docId == "0") {
        $('#vmMailUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlmailuserstype').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailUserType').html('').css('display', 'none');
        $('#ddlmailuserstype').css('border-color', 'gray');
    }
    var docId = $('#ddlmailusers').val();
    var userType = $('#ddlmailuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        $('#vmMailUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlmailusers').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailUser').html('').css('display', 'none');
        $('#ddlmailusers').css('border-color', 'gray');
    }
}
function ValidateMailData() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btnSaveMailData").css("filter", "grayscale(100%)");
        $("#btnSaveMailData").prop("disabled", true); /*End*/
    }
    var count = 0;
    debugger;
    /*$('#hfSubmitCount').val("1");*/
    var docId = $('#ddldoclistemail').val();
    if (docId == undefined || docId == "0") {
        $('#vmMailDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('[aria-labelledby="select2-ddldoclistemail-container"]').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailDocId').html('').css('display', 'none');
        $('#ddldoclistemail').css('border-color', 'gray');
    }
    var docId = $('#ddleventlistemail').val();
    if (docId == undefined || docId == "0") {
        $('#vmMailEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('[aria-labelledby="select2-ddleventlistemail-container"]').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailEventId').html('').css('display', 'none');
        $('#ddleventlistemail').css('border-color', 'gray');
    }
    var docId = $('#ddlmailuserstype').val();
    if (docId == undefined || docId == "0") {
        $('#vmMailUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('[aria-labelledby="select2-ddlmailuserstype-container"]').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailUserType').html('').css('display', 'none');
        $('#ddlmailuserstype').css('border-color', 'gray');
    }
    var docId = $('#ddlmailusers').val();
    var userType = $('#ddlmailuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        $('#vmMailUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('[aria-labelledby="select2-ddlmailusers-container"]').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmMailUser').html('').css('display', 'none');
        $('#ddlmailusers').css('border-color', 'gray');
    }
    if (count > 0) {
        return false;
    }
    else {
        $("#btnSaveMailData").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function validateMailDocId() {
    debugger;
    var docId = $('#ddldoclistemail').val();
    if (docId == undefined || docId == "0") {
        //
    }
    else {
        $('#vmMailDocId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldoclistemail-container"]').css('border-color', 'gray');
    }
}
function validateMailEvent() {
    var docId = $('#ddleventlistemail').val();
    if (docId == undefined || docId == "0") {
        //
    }
    else {
        $('#vmMailEventId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddleventlistemail-container"]').css('border-color', 'gray');
    }
}
function validateMailUsertype() {
    var docId = $('#ddlmailuserstype').val();
    if (docId == undefined || docId == "0") {
        //
    }
    else {
        $('#vmMailUserType').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddlmailuserstype-container"]').css('border-color', 'gray');
    }
}
function validateMailUserId() {
    var docId = $('#ddlmailusers').val();
    var userType = $('#ddlmailuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        //
    }
    else {
        $('#vmMailUser').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddlmailusers-container"]').css('border-color', 'gray');
    }
}

/*Sms Data Validations*/
function ValidateSmsData() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btnSaveSmsData").css("filter", "grayscale(100%)");
        $("#btnSaveSmsData").prop("disabled", true); /*End*/
    }
    var count = 0;
    debugger;
    var docId = $('#ddldoclistsms').val();
    /*$('#hfSubmitCount').val("1");*/
    if (docId == undefined || docId == "0") {
        $('#vmSmsDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldoclistsms').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmSmsDocId').html('').css('display', 'none');
        $('#ddldoclistsms').css('border-color', 'gray');
    }
    var docId = $('#ddleventlistsms').val();
    if (docId == undefined || docId == "0") {
        $('#vmSmsEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddleventlistsms').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmSmsEventId').html('').css('display', 'none');
        $('#ddleventlistsms').css('border-color', 'gray');
    }
    var docId = $('#ddlsmsuserstype').val();
    if (docId == undefined || docId == "0") {
        $('#vmSmsUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlsmsuserstype').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmSmsUserType').html('').css('display', 'none');
        $('#ddlsmsuserstype').css('border-color', 'gray');
    }
    var docId = $('#ddlsmsusers').val();
    var userType = $('#ddlsmsuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        $('#vmSmsUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlsmsusers').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmSmsUser').html('').css('display', 'none');
        $('#ddlsmsusers').css('border-color', 'gray');
    }
    if (count > 0) {
        return false;
    }
    else {
        $("#btnSaveSmsData").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function validateSmsDocId() {
    debugger;
    var docId = $('#ddldoclistsms').val();
    if (docId == undefined || docId == "0") {
        //$('#vmSmsDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddldoclistsms').css('border-color', 'red');
    }
    else {
        $('#vmSmsDocId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldoclistsms-container"]').css('border-color', 'gray');
    }
}
function validateSmsEvent() {
    var docId = $('#ddleventlistsms').val();
    if (docId == undefined || docId == "0") {
        //$('#vmSmsEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddleventlistsms').css('border-color', 'red');
    }
    else {
        $('#vmSmsEventId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddleventlistsms-container"]').css('border-color', 'gray');
    }
}
function validateSmsUsertype() {
    var docId = $('#ddlsmsuserstype').val();
    if (docId == undefined || docId == "0") {
        //$('#vmSmsUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddlsmsuserstype').css('border-color', 'red');
    }
    else {
        $('#vmSmsUserType').html('').css('display', 'none');
        $('#ddlsmsuserstype').css('border-color', 'gray');
        $('[aria-labelledby="select2-ddleventlistsms-container"]').css('border-color', 'gray');
    }
}
function validateSmsUserId() {
    var docId = $('#ddlsmsusers').val();
    var userType = $('#ddlsmsuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        //$('#vmSmsUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddlsmsusers').css('border-color', 'red');
    }
    else {
        $('#vmSmsUser').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddlsmsusers-container"]').css('border-color', 'gray');
    }
}

/*Whatsapp Data Validations*/
function ValidateWhatsappData() {
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btnSaveWhatsappData").css("filter", "grayscale(100%)");
        $("#btnSaveWhatsappData").prop("disabled", true); /*End*/
    }
    var count = 0;
    debugger;
    var docId = $('#ddldoclistwhatsapp').val();
    /*$('#hfSubmitCount').val("1");*/
    if (docId == undefined || docId == "0") {
        $('#vmWhatsappDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldoclistwhatsapp').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmWhatsappDocId').html('').css('display', 'none');
        $('#ddldoclistwhatsapp').css('border-color', 'gray');
    }
    var docId = $('#ddleventlistwhatsapp').val();
    if (docId == undefined || docId == "0") {
        $('#vmWhatsappEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddleventlistwhatsapp').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmWhatsappEventId').html('').css('display', 'none');
        $('#ddleventlistwhatsapp').css('border-color', 'gray');
    }
    var docId = $('#ddlwhatsappuserstype').val();
    if (docId == undefined || docId == "0") {
        $('#vmWhatsappUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlwhatsappuserstype').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmWhatsappUserType').html('').css('display', 'none');
        $('#ddlwhatsappuserstype').css('border-color', 'gray');
    }
    var docId = $('#ddlwhatsappusers').val();
    var userType = $('#ddlwhatsappuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        $('#vmWhatsappUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddlwhatsappusers').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmWhatsappUser').html('').css('display', 'none');
        $('#ddlwhatsappusers').css('border-color', 'gray');
    }
    if (count > 0)
        return false;
    else {
        $("#btnSaveWhatsappData").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function validateWhatsappDocId() {
    debugger;
    var docId = $('#ddldoclistwhatsapp').val();
    if (docId == undefined || docId == "0") {
        //$('#vmWhatsappDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddldoclistwhatsapp').css('border-color', 'red');
    }
    else {
        $('#vmWhatsappDocId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldoclistwhatsapp-container"]').css('border-color', 'gray');
    }
}
function validateWhatsappEvent() {
    var docId = $('#ddleventlistwhatsapp').val();
    if (docId == undefined || docId == "0") {
        //$('#vmWhatsappEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddleventlistwhatsapp').css('border-color', 'red');
    }
    else {
        $('#vmWhatsappEventId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddleventlistwhatsapp-container"]').css('border-color', 'gray');
    }
}
function validateWhatsappUsertype() {
    var docId = $('#ddlwhatsappuserstype').val();
    if (docId == undefined || docId == "0") {
        //$('#vmWhatsappUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddlwhatsappuserstype').css('border-color', 'red');
    }
    else {
        $('#vmWhatsappUserType').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddlwhatsappuserstype-container"]').css('border-color', 'gray');
    }
}
function validateWhatsappUserId() {
    var docId = $('#ddlwhatsappusers').val();
    var userType = $('#ddlwhatsappuserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        //$('#vmWhatsappUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddlwhatsappusers').css('border-color', 'red');
    }
    else {
        $('#vmWhatsappUser').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddlwhatsappusers-container"]').css('border-color', 'gray');
    }
}

/*Dashboard Data Validations*/
function ValidateDashboardData() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btnSaveDashboardData").css("filter", "grayscale(100%)");
        $("#btnSaveDashboardData").prop("disabled", true); /*End*/
    }
    var count = 0;
    debugger;
    var docId = $('#ddldoclistdashboard').val();
    /*$('#hfSubmitCount').val("1");*/
    if (docId == undefined || docId == "0") {
        $('#vmDashboardDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldoclistdashboard').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmDashboardDocId').html('').css('display', 'none');
        $('#ddldoclistdashboard').css('border-color', 'gray');
    }
    var docId = $('#ddleventlistdashboard').val();
    if (docId == undefined || docId == "0") {
        $('#vmDashboardEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddleventlistdashboard').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmDashboardEventId').html('').css('display', 'none');
        $('#ddleventlistdashboard').css('border-color', 'gray');
    }

    var docId = $('#ddldashboarduserstype').val();
    if (docId == undefined || docId == "0") {
        $('#vmDashboardUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldashboarduserstype').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmDashboardUserType').html('').css('display', 'none');
        $('#ddldashboarduserstype').css('border-color', 'gray');
    }
    var docId = $('#ddldashboardusers').val();
    var userType = $('#ddldashboarduserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        $('#vmDashboardUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        $('#ddldashboardusers').css('border-color', 'red');
        count = count + 1;
    }
    else {
        $('#vmDashboardUser').html('').css('display', 'none');
        $('#ddldashboardusers').css('border-color', 'gray');
    }
    if (count > 0)
        return false;
    else {
        $("#btnSaveDashboardData").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
       
}
function validateDashboardDocId() {
    debugger;
    var docId = $('#ddldoclistdashboard').val();
    if (docId == undefined || docId == "0") {
        //$('#vmDashboardDocId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddldoclistdashboard').css('border-color', 'red');
    }
    else {
        $('#vmDashboardDocId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldoclistdashboard-container"]').css('border-color', 'gray');
    }
}
function validateDashboardEvent() {
    var docId = $('#ddleventlistdashboard').val();
    if (docId == undefined || docId == "0") {
        //$('#vmDashboardEventId').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        //$('#ddleventlistdashboard').css('border-color', 'red');
    }
    else {
        $('#vmDashboardEventId').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddleventlistdashboard-container"]').css('border-color', 'gray');
    }
}
function validateDashboardUsertype() {
    var docId = $('#ddldashboarduserstype').val();
    if (docId == undefined || docId == "0") {
        // $('#vmDashboardUserType').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        // $('#ddldashboarduserstype').css('border-color', 'red');
    }
    else {
        $('#vmDashboardUserType').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldashboarduserstype-container"]').css('border-color', 'gray');
    }
}
function validateDashboardUserId() {
    var docId = $('#ddldashboardusers').val();
    var userType = $('#ddldashboarduserstype').val();
    if ((docId == undefined || docId == "0") && userType == "2") {
        // $('#vmDashboardUser').html($('#valueReq').text()).css('color', 'red').css('display', 'block');
        // $('#ddldashboardusers').css('border-color', 'red');
    }
    else {
        $('#vmDashboardUser').html('').css('display', 'none');
        $('[aria-labelledby="select2-ddldashboardusers-container"]').css('border-color', 'gray');
    }
}

function functionConfirmDelete(event, row_id) {
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
            $("#hfrowId").val(row_id);
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}

function EnableDisableMailUsersDiv() {
    debugger;
    var userType = $('#ddlmailuserstype').val();
    if (userType == "1" || userType == "0") {
        $('#divmailusers').css('display', 'none');
    }
    else {
        $('#divmailusers').css('display', 'block');
    }
}
function EnableDisableSmsUsersDiv() {
    var userType = $('#ddlsmsuserstype').val();
    if (userType == "1" || userType == "0") {
        $('#divsmsusers').css('display', 'none');
    }
    else {
        $('#divsmsusers').css('display', 'block');
    }
}
function EnableDisableWhatsappUsersDiv() {
    var userType = $('#ddlwhatsappuserstype').val();
    if (userType == "1" || userType == "0") {
        $('#divwhatsappusers').css('display', 'none');
    }
    else {
        $('#divwhatsappusers').css('display', 'block');
    }
}
function EnableDisableDashboardUsersDiv() {
    var userType = $('#ddldashboarduserstype').val();
    if (userType == "1" || userType == "0") {
        $('#divdashboardusers').css('display', 'none');
    }
    else {
        $('#divdashboardusers').css('display', 'block');
    }
}