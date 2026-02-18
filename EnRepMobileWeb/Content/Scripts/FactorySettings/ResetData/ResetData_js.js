
$(document).ready(function () {
    GetHo_list();

    $("#dho_ho").select2();
    $("#dbr_ho").select2();
    $("#dtd_ho").select2();
    $("#dmtd_ho").select2();
});

function GetHo_list() {
    try {
        $.ajax(
            {
                type: "POST",
                url: "/FactorySettings/ResetData/GetHo_list",
                data: {},
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        $('#dho_ho').empty();
                        $('#dbr_ho').empty();
                        $('#dtd_ho').empty();
                        $('#dmtd_ho').empty();

                        arr = JSON.parse(data);

                        if (arr.length > 0) {
                            $('#dho_ho').append(`<option value=0>---Select---</option>`);
                            $('#dbr_ho').append(`<option value=0>---Select---</option>`);
                            $('#dtd_ho').append(`<option value=0>---Select---</option>`);
                            $('#dmtd_ho').append(`<option value=0>---Select---</option>`);

                            for (var i = 0; i < arr.length; i++) {
                                if (arr[i].Comp_Id != "1") {
                                    $('#dho_ho').append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                }
                                $('#dbr_ho').append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                $('#dtd_ho').append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                $('#dmtd_ho').append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                            }
                        }
                        else {
                            $('#dho_ho').append(`<option value=0>---Select---</option>`);
                            $('#dbr_ho').append(`<option value=0>---Select---</option>`);
                            $('#dtd_ho').append(`<option value=0>---Select---</option>`);
                            $('#dmtd_ho').append(`<option value=0>---Select---</option>`);
                        }
                    }
                },
            });
    }
    catch (ex) { };
}
function OnChangeHo_data(flag) {
    try {
        debugger
        var comp_id = $("#" + flag).val();
        var ddl_br_id = flag.replace("_ho", "_br");
        if (comp_id != "0" && comp_id != "" && comp_id != null) {
            $.ajax(
                {
                    type: "POST",
                    url: "/FactorySettings/ResetData/GetBr_list",
                    data: { compid: comp_id },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "") {
                            var arr = [];
                            $('#' + ddl_br_id).empty();

                            arr = JSON.parse(data);

                            if (arr.length > 0) {
                                $('#' + ddl_br_id).append(`<option value=0>---Select---</option>`);

                                for (var i = 0; i < arr.length; i++) {
                                    if (arr[i].Comp_Id != "2" && ddl_br_id !="dbr_br") {
                                        $('#' + ddl_br_id).append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                    }
                                    if (arr[i].Comp_Id != "2" && ddl_br_id == "dbr_br") {
                                        $('#' + ddl_br_id).append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                    }
                                    if (arr[i].Comp_Id == "2" && ddl_br_id != "dbr_br") {
                                        $('#' + ddl_br_id).append(`<option value="${arr[i].Comp_Id}">${arr[i].comp_nm}</option>`);
                                    }
                                }
                                $("#" + ddl_br_id).select2();
                            }
                            else {
                                $('#' + ddl_br_id).append(`<option value=0>---Select---</option>`);
                            }
                        }
                    },
                });
        }
        else {
            $('#' + ddl_br_id).empty();
            $('#' + ddl_br_id).append(`<option value=0>---Select---</option>`);
            $("[aria-labelledby='select2-"+ddl_br_id+"-container']").css("border-color", "#ced4da")
        }

        hideerrmsg(flag);
    }
    catch (ex) { };
}
function onchangebr_ddl(flag) {
    hideerrmsg(flag);
}
function OnChangeRadioBtn() {
    debugger
    var rdo_val = $("input[type='radio']:checked").val();
    ddldisableenable(rdo_val);
    hideerrmsg("RDO");
    GetHo_list();
    clearbrlist();
}
function ddldisableenable(flag) {
    if (flag == "DHO") {
        $("#dho_ho").removeAttr("disabled");
        $("#dbr_ho").attr('disabled', 'disabled');
        $("#dbr_br").attr('disabled', 'disabled');
        $("#dtd_ho").attr('disabled', 'disabled');
        $("#dtd_br").attr('disabled', 'disabled');
        $("#dmtd_ho").attr('disabled', 'disabled');
        $("#dmtd_br").attr('disabled', 'disabled');

        $("#para_freset").css("display", "none");
        $("#para_DHO").removeAttr("style");
        $("#para_DBR").css("display", "none");
        $("#para_DALD").css("display", "none");
        $("#para_BLAPD").css("display", "none");
    }
    if (flag == "FRT") {
        $("#dho_ho").attr("disabled", 'disabled');
        $("#dbr_ho").attr('disabled', 'disabled');
        $("#dbr_br").attr('disabled', 'disabled');
        $("#dtd_ho").attr('disabled', 'disabled');
        $("#dtd_br").attr('disabled', 'disabled');
        $("#dmtd_ho").attr('disabled', 'disabled');
        $("#dmtd_br").attr('disabled', 'disabled');

        $("#para_freset").removeAttr("style");
        $("#para_DHO").css("display", "none");
        $("#para_DBR").css("display", "none");
        $("#para_DALD").css("display", "none");
        $("#para_BLAPD").css("display", "none");
    }
    if (flag == "DBR") {
        $("#dho_ho").attr("disabled", 'disabled');
        $("#dbr_ho").removeAttr('disabled');
        $("#dbr_br").removeAttr('disabled');
        $("#dtd_ho").attr('disabled', 'disabled');
        $("#dtd_br").attr('disabled', 'disabled');
        $("#dmtd_ho").attr('disabled', 'disabled');
        $("#dmtd_br").attr('disabled', 'disabled');

        $("#para_freset").css("display", "none");
        $("#para_DHO").css("display", "none");
        $("#para_DBR").removeAttr("style");
        $("#para_DALD").css("display", "none");
        $("#para_BLAPD").css("display", "none");
    }
    if (flag == "DTD") {
        $("#dho_ho").attr("disabled", 'disabled');
        $("#dbr_ho").attr('disabled', 'disabled');
        $("#dbr_br").attr('disabled', 'disabled');
        $("#dtd_ho").removeAttr('disabled');
        $("#dtd_br").removeAttr('disabled');
        $("#dmtd_ho").attr('disabled', 'disabled');
        $("#dmtd_br").attr('disabled', 'disabled');

        $("#para_freset").css("display", "none");
        $("#para_DHO").css("display", "none");
        $("#para_DBR").css("display", "none");
        $("#para_DALD").removeAttr("style");
        $("#para_BLAPD").css("display", "none");
    }
    if (flag == "DMTD") {
        $("#dho_ho").attr("disabled", 'disabled');
        $("#dbr_ho").attr('disabled', 'disabled');
        $("#dbr_br").attr('disabled', 'disabled');
        $("#dtd_ho").attr('disabled', 'disabled');
        $("#dtd_br").attr('disabled', 'disabled');
        $("#dmtd_ho").removeAttr('disabled');
        $("#dmtd_br").removeAttr('disabled');

        $("#para_freset").css("display", "none");
        $("#para_DHO").css("display", "none");
        $("#para_DBR").css("display", "none");
        $("#para_DALD").css("display", "none");
        $("#para_BLAPD").removeAttr("style");
    }
    
}
function clearbrlist() {
    $('#dbr_br').empty();
    $('#dtd_br').empty();
    $('#dmtd_br').empty();
    $('#dbr_br').append(`<option value=0>---Select---</option>`);
    $('#dtd_br').append(`<option value=0>---Select---</option>`);
    $('#dmtd_br').append(`<option value=0>---Select---</option>`);
}
function OnClickClearBtn() {
    debugger
    $("#rdo_freset").prop("checked", true);
    ddldisableenable("FRT");
    GetHo_list();
    clearbrlist();
    hideerrmsg("RDO");
}
function hideerrmsg(flag) {
    if (flag == "RDO") {
        $("#dbr_br").css("border-color", "#ced4da")
        $("#dtd_br").css("border-color", "#ced4da")

        $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "#ced4da")
        $("#span_dhoho").css("display", "none");
        $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "#ced4da")
        $("#span_dbrho").css("display", "none");
        $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "#ced4da")
        $("#span_dbrbr").css("display", "none");
        $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "#ced4da")
        $("#span_dtdho").css("display", "none");
        $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "#ced4da")
        $("#span_dtdbr").css("display", "none");
        $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "#ced4da")
        $("#span_dmtdho").css("display", "none");

        $("#divotpcofm").css("display", "none");
        $("#txt_otpnumber").val("");
    }
    
    if (flag == "dho_ho") {
        $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "#ced4da")
        $("#span_dhoho").css("display", "none");
        $("#span_dhobr").css("display", "none");
    }
    if (flag == "dbr_ho") {
        $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "#ced4da")
        $("#span_dbrho").css("display", "none");
        $("#span_dbrbr").css("display", "none");
    }
    if (flag == "dtd_ho") {
        $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "#ced4da")
        $("#span_dtdho").css("display", "none");
        $("#span_dtdbr").css("display", "none");
    }
    if (flag == "dmtd_ho") {
        $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "#ced4da")
        $("#span_dmtdho").css("display", "none");
    }
    if (flag == "dbr_br") {
        $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "#ced4da")
        $("#span_dbrbr").css("display", "none");
    }
    if (flag == "dtd_br") {
        $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "#ced4da")
        $("#span_dtdbr").css("display", "none");
    }
}
function onclick_resetdatabtn() {
    debugger
    try {
        showLoader();

        var result_ = validateresetdata();
        if (result_ == "N") {
            $("#divotpcofm").removeAttr("style");

            $.ajax(
                {
                    type: "POST",
                    url: "/FactorySettings/ResetData/SendOtpTochangeResetData",
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);

                        }
                        hideLoader();
                    },
                });
        }
    }
    catch (ex) { hideLoader(); };
}
function onclick_varify_otpbtn() {
    debugger
    try {
        showLoader();
        var rsetflag = $("input[type='radio']:checked").val();
        var comp_id = "0";
        var br_id = "0";

        var err_flag = "N";
        var otp = $("#txt_otpnumber").val();
        if (rsetflag == "DHO") {
            var ddl_dho = $("#dho_ho").val();
            comp_id = ddl_dho;
            if (ddl_dho == "0") {
                $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "red")
                $("#span_dhoho").text($("#valueReq").text());
                $("#span_dhoho").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "#ced4da")
                $("#span_dhoho").css("display", "none");
            }
        }
        if (rsetflag == "DBR") {
            var ddl_dbrho = $("#dbr_ho").val();
            var ddl_ddbrbr = $("#dbr_br").val();
            comp_id = ddl_dbrho;
            br_id = ddl_ddbrbr;
            if (ddl_dbrho == "0") {
                $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "red")
                $("#span_dbrho").text($("#valueReq").text());
                $("#span_dbrho").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "#ced4da")
                $("#span_dbrho").css("display", "none");
            }
            if (ddl_ddbrbr == "0") {
                $("#dbr_br").css("border-color", "red")
                $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "red")
                $("#span_dbrbr").text($("#valueReq").text());
                $("#span_dbrbr").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("#dbr_br").css("border-color", "#ced4da")
                $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "#ced4da")
                $("#span_dbrbr").css("display", "none");
            }
        }
        if (rsetflag == "DTD") {
            var ddl_dtdho = $("#dtd_ho").val();
            var ddl_dtdbr = $("#dtd_br").val();
            comp_id = ddl_dtdho;
            br_id = ddl_dtdbr;
            if (ddl_dtdho == "0") {
                $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "red")
                $("#span_dtdho").text($("#valueReq").text());
                $("#span_dtdho").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "#ced4da")
                $("#span_dtdho").css("display", "none");
            }
            if (ddl_dtdbr == "0") {
                $("#dtd_br").css("border-color", "red")
                $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "red")
                $("#span_dtdbr").text($("#valueReq").text());
                $("#span_dtdbr").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("#dtd_br").css("border-color", "#ced4da")
                $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "#ced4da")
                $("#span_dtdbr").css("display", "none");
            }
        }
        if (rsetflag == "DMTD") {
            var ddl_dmtdho = $("#dmtd_ho").val();
            comp_id = ddl_dmtdho;
            br_id = $("#dmtd_br").val();
            if (ddl_dmtdho == "0") {
                $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "red")
                $("#span_dmtdho").text($("#valueReq").text());
                $("#span_dmtdho").css("display", "block");
                err_flag = "Y";
            }
            else {
                $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "#ced4da")
                $("#span_dmtdho").css("display", "none");
            }
        }
        if (otp == "0" || otp == "" || otp == null) {
            $("#txt_otpnumber").css("border-color", "red")
            $("#span_otpverify").text($("#valueReq").text());
            $("#span_otpverify").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("#txt_otpnumber").css("border-color", "#ced4da")
            $("#span_otpverify").css("display", "none");
        }
        if (err_flag == "N") {

          $.ajax(
                {
                    type: "POST",
                    url: "/FactorySettings/ResetData/FactoryReset_data",
                    data: {
                        compid: comp_id,
                        brid: br_id,
                        flag: rsetflag,
                        verfy_otp: otp
                    },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                             
                            if (arr == "Invalid OTP" || arr == "OTP expired") {
                                swal("", arr, "warning");
                            }
                            if (arr == "Success") {
                                swal("", $("#dataDelSucess").text(), "success");

                                OnClickClearBtn();
                            }
                        }
                        hideLoader();
                    },
                });
        }
    }
    catch (ex) { hideLoader(); };
}
function validateresetdata() {
    var rsetflag = $("input[type='radio']:checked").val();
    var err_flag = "N";
    if (rsetflag == "DHO") {
        var ddl_dho = $("#dho_ho").val();
        if (ddl_dho == "0") {
            $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "red")
            $("#span_dhoho").text($("#valueReq").text());
            $("#span_dhoho").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("[aria-labelledby='select2-dho_ho-container']").css("border-color", "#ced4da")
            $("#span_dhoho").css("display", "none");
        }
    }
    if (rsetflag == "DBR") {
        var ddl_dbrho = $("#dbr_ho").val();
        var ddl_ddbrbr = $("#dbr_br").val();
        if (ddl_dbrho == "0") {
            $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "red")
            $("#span_dbrho").text($("#valueReq").text());
            $("#span_dbrho").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("[aria-labelledby='select2-dbr_ho-container']").css("border-color", "#ced4da")
            $("#span_dbrho").css("display", "none");
        }
        if (ddl_ddbrbr == "0") {
            $("#dbr_br").css("border-color", "red")
            $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "red")
            $("#span_dbrbr").text($("#valueReq").text());
            $("#span_dbrbr").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("#dbr_br").css("border-color", "#ced4da")
            $("[aria-labelledby='select2-dbr_br-container']").css("border-color", "#ced4da")
            $("#span_dbrbr").css("display", "none");
        }
    }
    if (rsetflag == "DTD") {
        var ddl_dtdho = $("#dtd_ho").val();
        var ddl_dtdbr = $("#dtd_br").val();
        if (ddl_dtdho == "0") {
            $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "red")
            $("#span_dtdho").text($("#valueReq").text());
            $("#span_dtdho").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("[aria-labelledby='select2-dtd_ho-container']").css("border-color", "#ced4da")
            $("#span_dtdho").css("display", "none");
        }
        if (ddl_dtdbr == "0") {
            $("#dtd_br").css("border-color", "red")
            $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "red")
            $("#span_dtdbr").text($("#valueReq").text());
            $("#span_dtdbr").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("#dtd_br").css("border-color", "#ced4da")
            $("[aria-labelledby='select2-dtd_br-container']").css("border-color", "#ced4da")
            $("#span_dtdbr").css("display", "none");
        }
    }
    if (rsetflag == "DMTD") {
        var ddl_dmtdho = $("#dmtd_ho").val();
        br_id = $("#dmtd_br").val();
        if (ddl_dmtdho == "0") {
            $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "red")
            $("#span_dmtdho").text($("#valueReq").text());
            $("#span_dmtdho").css("display", "block");
            err_flag = "Y";
        }
        else {
            $("[aria-labelledby='select2-dmtd_ho-container']").css("border-color", "#ced4da")
            $("#span_dmtdho").css("display", "none");
        }
    }
    return err_flag;
}
function onotpkeypress() {
    $("#txt_otpnumber").css("border-color", "#ced4da")
    $("#span_otpverify").css("display", "none");
}