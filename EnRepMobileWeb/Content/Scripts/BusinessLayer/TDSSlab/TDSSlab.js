

function TDSSaveBtnClick() {
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    let Errflag = "N";
    if (TDSSlabCheckVallidation("value_from", "vm_value_from") == false) {
        Errflag = "Y";
    }
    if (TDSSlabCheckVallidation("value_to", "vm_value_to") == false) {
        Errflag = "Y";
    }
    if (TDSSlabCheckVallidation("tds_perc", "vm_tds_perc") == false) {
        Errflag = "Y";
    }
    if (TDSSlabCheckVallidation("ddlTdsAcc", "vm_ddlTdsAcc","N") == false) {
        Errflag = "Y";
    }
    if (TDSSlabCheckVallidation("ddlTcsAcc", "vm_ddlTcsAcc", "N") == false) {
        Errflag = "Y";
    }
    if (Errflag == "Y") {
        return false;
    } else {
        const value_from = $("#value_from").val();
        const value_to = $("#value_to").val();
        if (parseFloat(CheckNullNumber(value_to)) <= parseFloat(CheckNullNumber(value_from))) {
            $("#value_to").css("border-color", "red");
            $("#vm_value_to").text($("#InvalidRange").text());
            $("#vm_value_to").css("display", "block");
            return false;
        } else {
            $("#value_to").css("border-color", "#ced4da");
            $("#vm_value_to").text("");
            $("#vm_value_to").css("display", "none");
            $("#value_from").css("border-color", "#ced4da");
            $("#vm_value_from").text("");
            $("#vm_value_from").css("display", "none");

            $("#btn_save").css("filter", "grayscale(100%)");
            $("#hdnSavebtn").val("AllreadyClick");
            return true;
        }
    }
}
function Validate_value_from() {
    TDSSlabCheckVallidation("value_from", "vm_value_from");
    const value_from = $("#value_from").val();
    const value_to = $("#value_to").val();
    debugger;
    if (value_to != null && value_to != "") {
        if (parseFloat(CheckNullNumber(value_to)) <= parseFloat(CheckNullNumber(value_from))) {
            $("#value_from").css("border-color", "red");
            $("#vm_value_from").text($("#InvalidRange").text());
            $("#vm_value_from").css("display", "block");
        } else {
            $("#value_from").css("border-color", "#ced4da");
            $("#vm_value_from").text("");
            $("#vm_value_from").css("display", "none");
            $("#value_to").css("border-color", "#ced4da");
            $("#vm_value_to").text("");
            $("#vm_value_to").css("display", "none");
        }
    }
    
}

function Validate_value_to() {
    TDSSlabCheckVallidation("value_to", "vm_value_to");
    const value_from = $("#value_from").val();
    const value_to = $("#value_to").val();
    debugger;
    if (parseFloat(CheckNullNumber(value_to)) <= parseFloat(CheckNullNumber(value_from))) {
        $("#value_to").css("border-color", "red");
        $("#vm_value_to").text($("#InvalidRange").text());
        $("#vm_value_to").css("display", "block");
    } else {
        $("#value_to").css("border-color", "#ced4da");
        $("#vm_value_to").text("");
        $("#vm_value_to").css("display", "none");
        $("#value_from").css("border-color", "#ced4da");
        $("#vm_value_from").text("");
        $("#vm_value_from").css("display", "none");
    }
}
function Validate_tds_perc() {
    TDSSlabCheckVallidation("tds_perc", "vm_tds_perc");
}
function Validate_tds_acc_id() {
    TDSSlabCheckVallidation("ddlTdsAcc", "vm_ddlTdsAcc","N");
}
function Validate_tcs_acc_id() {
    TDSSlabCheckVallidation("ddlTcsAcc", "vm_ddlTcsAcc", "N");
}
function ResetFeilds() {
    TDSSlabCheckVallidation("value_from", "vm_value_from");
    TDSSlabCheckVallidation("value_to", "vm_value_to");
    TDSSlabCheckVallidation("tds_perc", "vm_tds_perc");
    TDSSlabCheckVallidation("ddlTdsAcc", "vm_ddlTdsAcc","N");
    TDSSlabCheckVallidation("ddlTcsAcc", "vm_ddlTcsAcc","N");

}
function ConfirmDeleteTDS(e) {
    const Crow = $(e.target).closest('tr');
    try {
        swal({
            title: $("#deltital").text() + "?",
            text: $("#deltext").text() + "!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        },
            function () {
                const slab_id = Crow.find("#td_slab_id").text();
                $("#slab_id").val(slab_id);
                $("#HdnCommand").val("Delete");
                $("form").submit();
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function OnclickEditTDS(e) {
    const Crow = $(e.target).closest('tr');
    
    $("#btn_save").val("Update");
    const value_from = Crow.find("#td_value_from").text();
    const value_to = Crow.find("#td_value_to").text();
    const tds_perc = Crow.find("#td_tds_perc").text();
    const slab_id = Crow.find("#td_slab_id").text();
    const tds_acc_id = Crow.find("#td_tds_acc_id").text();
    const tcs_acc_id = Crow.find("#td_tcs_acc_id").text();
    $("#value_from").val(value_from);
    $("#value_to").val(value_to);
    $("#tds_perc").val(tds_perc);
    $("#slab_id").val(slab_id);
    $("#ddlTdsAcc").val(tds_acc_id);
    $("#ddlTcsAcc").val(tcs_acc_id);
    ResetFeilds();
    $("#hdnSavebtn").val("");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#value_from").attr("disabled", false);
        $("#value_to").attr("disabled", false);
        $("#tds_perc").attr("disabled", false);
        $("#ddlTdsAcc").attr("disabled", false);
    }
}
function AllowFloatValueOnly(el, evt) {
    if (TdsSlab_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
}
function TDSSlabCheckVallidation(main_ID, Message_ID,AllowZero) {
    /*Use this function for validation message in input and simple dropdownfields 
     *  as if(CheckVallidation(main_ID, Message_ID)==(true/false))*/
    var Field_Data = $("#" + main_ID).val();
    if (AllowZero == "N") {
        if (Field_Data == "0") {
            Field_Data = "";
        }
    }
    var ErrorFlag = "N";
    if (Field_Data == null || Field_Data == "") {
        $("#" + main_ID).css("border-color", "red");
        $('[aria-labelledby="select2-' + main_ID + '-container"]').css("border-color", "red");
        $("#" + Message_ID).text($("#valueReq").text());
        $("#" + Message_ID).css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#" + main_ID).css("border-color", "#ced4da");
        $('[aria-labelledby="select2-' + main_ID + '-container"]').css("border-color", "#ced4da");
        $("#" + Message_ID).text("");
        $("#" + Message_ID).css("display", "none");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function TdsSlab_FloatValueonly(el, evt, Digit) {
    //debugger;
    var BooleanTF = false;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (charCode == 46 && el.value.indexOf(".") !== -1) {
        return false;
    }
    var ValDecDigit;
    if (Digit == "0") {
        ValDecDigit = "";
    } else {
        ValDecDigit = $(Digit).text();
    }

    if (Cmn_CheckSelectedTextInTextField(evt) == true) {
        return true;
    };
    var key = evt.key;
    var number = el.value.split('.');
    if (key == "0" && number[0] == "0" && number.length == 1) {
        return false;
    }
    if (number.length == 1) {
        //var valPer = number[0] + '' + key;
        var selectedval = Cmn_SelectedTextInTextField(evt);
        var KeyLocation = evt.currentTarget.selectionStart;
        var valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        //if (parseFloat(valPer) >= 1000000000) {
        //    return false;
        //}
    }
    else {
        //var valPer = number[0] + '.' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        var decDgtLength = number[1].length;
        if (KeyLocation < (el.value.length - (decDgtLength))) {
            if (KeyLocation < (el.value.length - (decDgtLength))) {
                //valPer = number[0].splice(KeyLocation, 0, key);
                BooleanTF = true;
            }
        } else {
            //valPer = el.value.splice(KeyLocation, 0, key);
        }
        var selectedval = Cmn_SelectedTextInTextField(evt);
        var KeyLocation = evt.currentTarget.selectionStart;
        var valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        //if (parseFloat(valPer) >= 1000000000) {
        //    return false;
        //}
    }

    if (ValDecDigit != "") {
        var number = el.value.split('.');
        if (number.length == 2 && number[1].length > (parseInt(ValDecDigit) - 1)) {
            if (BooleanTF) {
                return true;
            }
            return false;
        }

    }
    return true;
}

