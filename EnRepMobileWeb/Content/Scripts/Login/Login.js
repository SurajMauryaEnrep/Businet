var timerInterval;
function RemoveValidation() {
    var spans = $('.field-validation-error');
    spans.text('');

}
$(document).ready(function () {
    debugger;
    //$("#txtUserName").val("");
    $("#HdnPw").val("");
    if ($('#hdnAutoLog').val() == "1") {
        if ($('#txtPass').val() != "") {
            UserPassValidate();
            $('#hdnAutoLog').val("0");
        }
    }
    $('#hrefchangepass').click(function () {
        $('#txtusername').val($('#txtUserName').val());
    });
    var showPass = 0;
    debugger;
    $('.btn-show-pass').on('click', function () {
        if (showPass == 0) {
            debugger;
            //$(this).next('input').attr('type', 'text');
            $("#txtPass").attr('type', 'text');
            $(this).find('i').removeClass('zmdi-eye');
            $(this).find('i').addClass('zmdi-eye-off');
            showPass = 1;
        }
        else {
            debugger;
            //$(this).next('input').attr('type', 'password');
            $("#txtPass").attr('type', 'password');
            $(this).find('i').addClass('zmdi-eye');
            $(this).find('i').removeClass('zmdi-eye-off');
            showPass = 0;
        }

    });
    $(document).keydown(function (e) {
        // Check if Ctrl and Shift keys are pressed and the key code is 'R' (82)
        if (e.ctrlKey && e.shiftKey && e.keyCode === 82) {
            var redirectURL = window.location.href.replace('popup1', '');
            window.location.href = redirectURL;
        }
    });
});

$("#selectBranch").on("change", function () {
    $("#spanBranch").text("");
    $("#spanBranch").removeClass('field-validation-error');
    $("#selectBranch").css("border", "#ced4da");

});
document.onkeypress = function (e) {
    if (event.keyCode == 13) {
        debugger;
        if (ValidateloginDetail() == false) {
            return false;
        }
    }
}
//$("#txtUserName").focus(function () {
//    $('#txtUserName').attr("readonly", false);
//})
function onclickPassWord() {

}
function emptypass2() {
    //$('#txtPass').attr("type", "text");
    $('#txtPass').attr("readonly", true);
    setTimeout(function () {
        $('#txtPass').attr("readonly", false);
    }, 1);
}
function OnkeypressUser() {
    $("#spanUser").text("");
    $("#spanUser").removeClass('field-validation-error');
    $("#txtUserName").css("border", "#ced4da");

}
function OnkeypressPassword() {
    debugger;
    //if (e.keyCode == 8) {
    //    var txtPass = $('#txtPass').val().trim();
    //if (txtPass == "") {
    //    $('#txtPass').attr("readonly", true);
    //    setTimeout(function () {
    //        $('#txtPass').attr("readonly", false);
    //    }, 1);
    //}
    //}
    $('#txtPass').attr("type", "password");
    $("#spanInvalidPass").text("");
    $("#spanInvalidPass").removeClass('field-validation-error');
    $("#txtPass").css("border", "#ced4da");
    sessionStorage.removeItem("changeUserName");
}
$("#txtUserName").change(function () {
    debugger;
    try {
        UserValidate();
        sessionStorage.setItem("changeUserName", "Yes");
        Cleardetail();

    } catch (err) {
        console.log("OnChangetxtUserName Error : " + err.message);
    }
});
function Cleardetail() {
    $("#txtPass").val("");
    var s1 = '<option value="-1"> Choose Company</option>';
    var s2 = '<option value="-1"> Choose Branch</option>';
    var s3 = '<option value="-1"> Choose Language</option>';
    $("#selectCompany").html(s1);
    $('#selectBranch').html(s2);
    $("#selectLanguage").html(s3);

}
$("#txtPass").change(function () {
    debugger;
    try {
        var changeUserName = sessionStorage.getItem("changeUserName");
        if (changeUserName == "Yes") {
            //sessionStorage.removeItem("changeUserName");
            Cleardetail();
        }
        UserPassValidate();
        $("#txtPass").css("border", "#ced4da");


    } catch (err) {
        console.log("OnChangetxtPass Error : " + err.message);
    }
});
$("#selectCompany").change(function () {
    debugger;
    try {
        var SelectedCompany = $("#selectCompany").find(":selected").val();
        var userName = $('#txtUserName').val().trim();
        var passward = $('#txtPass').val().trim();
        if (SelectedCompany != -1) {
            //$("#selectLanguage").prop('disabled', false);
            $.ajax(
                {

                    type: "Post",
                    url: "/Home/GetBranchList",
                    data: {
                        userName: userName,
                        passward: passward,
                        Comp_id: SelectedCompany
                    },
                    success: function (data) {
                        debugger;
                        arr = JSON.parse(data);
                        var s = '';
                        if (arr.Table4[0].br_id == "-1") {
                            s += '<option value="-1"> Choose Branch</option>';
                        }
                        for (var i = 0; i < arr.Table3.length; i++) {
                            s += '<option value=' + arr.Table3[i].Comp_Id + '>' + arr.Table3[i].comp_nm + '</option>';
                        }
                        $('#selectBranch').html(s);
                        $('#selectBranch').val(arr.Table4[0].br_id);
                    },
                });
        } else {
            $("#selectLanguage").prop('disabled', true);
        }
    } catch (err) {
        console.log("OnChangeselectCompany Error : " + err.message);
    }
});
function UserValidate() {
    try {
        debugger;
        var userName = $('#txtUserName').val().trim();
        $.ajax(
            {
                type: "Post",
                url: "/Home/ValidUser",
                data: {
                    userName: userName,
                },

                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        $("#spanUser").text("");
                        $("#spanUser").removeClass('field-validation-error');
                       // $("#txtUserName").css("border", "#ced4da");/*Commented Nitesh With Prakash Sir 13052025*/
                        $("#txtUserName").css("border", "");
                        $('#hrefchangepass').prop('disabled', false);
                    } else {

                        $("#spanUser").text("Invalid Login ID");
                        $("#spanUser").addClass('field-validation-error');
                        $("#txtUserName").css("border", "1px solid red");
                        $("#txtUserName").focus();
                        $('#hrefchangepass').prop('disabled', true);
                        return false
                    }
                },
            });
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function UserPassValidate() {
    try {

        var userName = $('#txtUserName').val().trim();
        var passward = $('#txtPass').val().trim();

        $.ajax(
            {

                type: "Post",
                url: "/Home/UserPassValidate",
                data: {
                    userName: userName,
                    passward: passward
                },
                success: function (data) {
                    debugger;

                    if (data != "" || data != null) {
                        var uservali = sessionStorage.getItem("UserVali");
                        var CompId = $("#selectCompany").val();
                        var s1 = '<option value=' + CompId + '> ' + $("#selectCompany option:selected").text() + '</option>';
                        var BrId = $("#selectBranch").val();
                        var s2 = '<option value=' + BrId + '> ' + $("#selectBranch option:selected").text() + '</option>';
                        var Lang = $("#selectLanguage").val();
                        var s3 = '<option value=' + Lang + '> ' + $("#selectLanguage option:selected").text() + '</option>';

                        $('#Languae').html(data);

                        if (CompId != -1 && uservali == "Yes") {
                            $("#selectCompany").html(s1);
                        }
                        if (BrId != -1 && uservali == "Yes") {
                            $("#selectBranch").html(s2);
                        }
                        if (Lang != -1 && uservali == "Yes") {
                            $("#selectLanguage").html(s3);
                        }

                        if ($("#selectCompany").val() == -1) {
                            sessionStorage.removeItem("UserVali");
                            $("#spanInvalidPass").text("Invalid Password");
                            $("#spanInvalidPass").addClass('field-validation-error');
                            $("#txtPass").css("border", "1px solid red");
                            $("#txtPass").focus();
                            var changeUserName = sessionStorage.getItem("changeUserName");
                            if (changeUserName == "Yes") {
                                sessionStorage.removeItem("changeUserName");
                                $("#spanInvalidPass").text("");
                            }
                            return false;
                        }
                        else {
                            var uservali = sessionStorage.getItem("UserVali");
                            if (uservali == "Yes") {
                                var selectBranch = $("#selectBranch option:selected").text();
                                $("#HdnBranchName").val(selectBranch);
                                sessionStorage.removeItem("UserVali");
                                $('form').submit();
                            }
                            $("#spanInvalidPass").text("");
                            $("#spanInvalidPass").removeClass('field-validation-error');
                           // $("#txtPass").css("border", "#ced4da");/*Commenetd By  Nitesh with prakash 13052025*/
                            $("#txtPass").css("border", "");
                        }

                    }

                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function RemoveValidationUser() {
    $("#spanUser").text("");
    $("#spanUser").removeClass('field-validation-error');
    $("#txtUserName").css("border", "#ced4da");


}
function RemoveValidationPass() {
    $("#spanInvalidPass").text("");
    $("#spanInvalidPass").removeClass('field-validation-error');
    $("#txtPass").css("border", "#ced4da");
}
$("#LoginButton").click(function () {
    if (ValidateloginDetail() == false) {
        return false;
    }
    else {
        if ($("#selectCompany").val() == -1) {
            return false;
        }
    }


});
function ValidateloginDetail() {
    debugger;
    var txtUserName = $("#txtUserName").val();
    var txtPass = $("#txtPass").val();
    var Flag = "N";
    if (txtUserName == "") {
        $("#spanUser").text("Value required.");
        $("#spanUser").addClass('field-validation-error');
        $("#txtUserName").css("border", "1px solid red");
        $("#txtUserName").focus();
        Flag = "Y";
        return false;
    }
    else {
        $("#spanUser").text("");
        $("#spanUser").removeClass('field-validation-error');
        $("#txtUserName").css("border", "#ced4da");

    }
    if (txtPass == "") {
        $("#spanInvalidPass").text("Value required.");
        $("#spanInvalidPass").addClass('field-validation-error');
        $("#txtPass").css("border", "1px solid red");
        $("#txtPass").focus();
        Flag = "Y";
        return false;
    }
    else {
        $("#spanInvalidPass").text("");
        $("#spanInvalidPass").removeClass('field-validation-error');
        $("#txtPass").css("border", "#ced4da");

    }
    if ($("#selectBranch").val() == null || $("#selectBranch").val() == "" || $("#selectBranch").val() == "-1") {
        $("#spanBranch").text("Value required.");
        $("#spanBranch").addClass('field-validation-error');
        $("#selectBranch").css("border", "1px solid red");
        $("#selectBranch").focus();
        Flag = "Y";
        return false;
    }
    else {
        $("#spanBranch").text("");
        $("#spanBranch").removeClass('field-validation-error');
        $("#selectBranch").css("border", "#ced4da");

    }
    if (Flag == "Y") {
        return false;
    }
    else {
        debugger;
        if (sessionStorage.getItem("UserVali") != "Yes") {
            UserPassValidate();
        }

        if ($("#selectCompany").val() == -1) {
            sessionStorage.setItem("UserVali", "Yes");
        } else {
            sessionStorage.setItem("UserVali", "Yes");
            var selectBranch = $("#selectBranch option:selected").text();
            $("#HdnBranchName").val(selectBranch);

            $('form').submit();
        }


    }
}
function ChangePasswordClick() {
    debugger;
    $('#txtusername').val($('#txtUserName').val());
    $('#hfAction').val('changepass');
    $('#divoldpass').css('display', 'block');
    $("#heading_change_pass").css('display', '');
    $("#heading_forgot_pass").css('display', 'none');
}
function PasswordCompareValidator() {
    debugger;
    var NewPass = $('#txtnewpass').val();
    var ComfirmPass = $('#txtconfpass').val();
    if (NewPass != ComfirmPass && ComfirmPass != "") {
        /*$('#ChangePassword').attr('disabled', true);*/
        $('#comparevalidator').css('display', 'block');
        $('#comparevalidator').text('Password not matched');

    }
    else {
        /*$('#ChangePassword').attr('disabled', false);*/
        $('#comparevalidator').css('display', 'none');
    }
}
function ValidateOldPassword() {
    debugger;
    var userName = $('#txtusername').val();
    var oldPassword = $('#txtoldpass').val();
    $.ajax(
        {
            type: "Post",
            url: "/Home/ValidatePasswordToChange",
            data: {
                userName: userName,
                password: oldPassword
            },
            success: function (data) {
                debugger;
                if (data == "Valid") {
                    /*$('ChangePassword').prop('disabled', false);*/
                    $('#validateOldPass').css('display', 'none');
                    $('#hfPassValidate').val('valid');
                    $('#txtnewpass').prop('disabled', false);
                    $('#txtconfpass').prop('disabled', false);
                }
                else {
                    /*$('ChangePassword').prop('disabled', true);*/
                    $('#validateOldPass').text('Invalid Old Password');
                    $('#validateOldPass').css('display', 'block');
                    $('#hfPassValidate').val('invalid');
                    $('#txtnewpass').val('');
                    $('#txtconfpass').val('');
                    $('#txtnewpass').prop('disabled', true);
                    $('#txtconfpass').prop('disabled', true);
                }
            },
        });
}
function SendOTP() {
    debugger;
    let userName = $('#txtusername').val();
    let oldPassword = $('#txtoldpass').val();
    if ($('#txtnewpass').val() != $('#txtconfpass').val()) {
        event.preventDefault();
        return false;
    }
    if ($('#hfPassValidate').val() != 'valid' && $('#hfAction').val() == 'changepass') {
        event.preventDefault();
        return false;
    }
    var act = $('#hfAction').val();
    $.ajax(
        {
            type: "Post",
            url: "/Home/SendOtpTochangePassword",
            data: {
                act: act,
                userName: userName,
                password: oldPassword
            },
            success: function (data) {
                debugger;
                if (data == "Success") {
                    swal({
                        title: "OTP Verification",
                        text: "OTP send on registered email. please verify otp to change password",
                        type: "success",
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    });
                    $('#txtOtp').prop('disabled', false);
                    OtpExpireTimer();
                }
                else {
                    swal({
                        title: "OTP Verification",
                        text: "Something went wrong",
                        type: "warning",
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    });
                    $('#txtOtp').prop('disabled', true);
                }
            },
        });
}
function OtpExpireTimer() {
    
    if (timerInterval) {
        clearInterval(timerInterval);
    }
    // Set the timer for 5 seconds (5000 milliseconds)
    var endTime = new Date().getTime() + 120000; // 5 seconds from now

    // Update the timer display every second
     timerInterval = setInterval(function () {
        var currentTime = new Date().getTime();
        var timeRemaining = endTime - currentTime;

        // Calculate minutes and seconds
        var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);
         if (seconds < 10) {
             seconds = "0" + seconds;
         }
        // Display the time remaining
        $("#timerDisplay").html("expire in-0"+minutes + ":" + seconds );

        // If the timer has expired, show a message and clear the interval
        if (timeRemaining <= 0) {
            $("#timerDisplay").html("OTP expired!");
            clearInterval(timerInterval);
        }
    }, 1000); // Update every second
}
function PreventIfUsernameEmpty() {
    if ($('#txtUserName').val() == "") {
        event.preventDefault();
    }
}
function ChangePassword() {
    debugger;
    let userName = $('#txtusername').val();
    let oldPassword = $('#txtoldpass').val();
    let newPassword = $('#txtnewpass').val();
    let act = $('#hfAction').val();
    let otp = $('#txtOtp').val();
    var otpExp = $('#timerDisplay').html();
    if (otpExp == "OTP expired!") {
        swal({
            title: "Change Password",
            text: "OTP Expired",
            type: "warning",
            confirmButtonText: "OK",
            closeOnConfirm: true
        });
        return false;
    }
    $.ajax(
        {
            type: "Post",
            url: "/Home/ChangePassword",
            data: {
                act: act,
                userName: userName,
                oldPassword: oldPassword,
                newPassword: newPassword,
                otp: otp
            },
            success: function (data) {
                debugger;
                if (data == "Password changed successfully") {

                    var redirectURL = window.location.href.replace('popup1', '');
                    $('#txtoldpass').val('');
                    $('#txtnewpass').val('');
                    $('#txtconfpass').val('');
                    $('#txtOtp').val('');
                    $('#txtoldpass').prop('disabled', true);
                    $('#txtnewpass').prop('disabled', true);
                    $('#txtconfpass').prop('disabled', true);
                    $('#txtOtp').prop('disabled', true);
                    $('#ChangePassword').prop('disabled', true);
                    swal({
                        title: "Change Password",
                        text: data,
                        type: "success",


                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    });
                    $('#timerDisplay').html('');
                    clearInterval(timerInterval);
                    window.location.href = redirectURL;
                    /*location.reload(true);*/
                }
                else {
                    swal({
                        title: "Change Password",
                        text: data,
                        type: "warning",


                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    });
                }
                $('#timerDisplay').html('');
                clearInterval(timerInterval);
            },
        });
}
function onclickForgetPass() {
    $('#txtusername').val($('#txtUserName').val());
    $('#hfAction').val('forgetpass');
    $('#divoldpass').css('display', 'none');
    $('#txtnewpass').prop('disabled', false);
    $('#txtconfpass').prop('disabled', false);

    $("#heading_change_pass").css('display', 'none');
    $("#heading_forgot_pass").css('display', '');
}
function OnChangeOTP() {
    $('#ChangePassword').prop('disabled', false);
}
function DiscardClick() {
    $('#txtoldpass').val('');
    $('#txtnewpass').val('');
    $('#txtconfpass').val('');
    $('#txtOtp').val('');
    $('#ChangePassword').prop('disabled', true);
    $('#validateOldPass').css('display','none');
    $('#comparevalidator').css('display', 'none');
    $("#timerDisplay").html("");
    var redirectURL = window.location.href.replace('popup1', '');
    window.location.href = redirectURL;
}
