function CheckEmpty(ControlName, ControlSpanName, MessageName) {
    if ($("#" + ControlName + "").val().trim() == "") {
        $("#" + ControlSpanName + "").text("" + MessageName + "");
        $(".field-validation-valid").addClass('field-validation-error');
        $("#" + ControlName + "").focus();
        return false;
    }
    return true;
}

function ReturnMsg(ControlName, ControlSpanName, MessageName) {   
        $("#" + ControlSpanName + "").text("" + MessageName + "");
        $(".field-validation-valid").addClass('field-validation-error');
        $("#" + ControlName + "").focus();
        return false;    
}

function CheckEmptyDropdown(ControlName, ControlSpanName, defaultValue, MessageName) {
    if ($('#' + ControlName).find(":selected").val() == defaultValue) {
        $("#" + ControlSpanName + "").text(MessageName);
        $(".field-validation-valid").addClass('field-validation-error');
        $("#" + ControlName + "").focus();
        return false;
    }
}