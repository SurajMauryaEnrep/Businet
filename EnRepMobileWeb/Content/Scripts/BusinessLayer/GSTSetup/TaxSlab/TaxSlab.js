$(document).ready(function () {

    $("#listTaxPer").select2();
    $("#HSNNumber").select2();

})
function OnchangeHsnNumbar() {
    $('[aria-labelledby="select2-HSNNumber-container"]').css("border-color", "#ced4da");
    $("#vmHSNNumber").text("");
}
function OnchangeTaxPerHsn() {
    $('[aria-labelledby="select2-listTaxPer-container"]').css("border-color", "#ced4da");
    $("#vmlistTaxPer").text("");
}
function OnChangeTaxPer() {
    document.getElementById("vmTaxPer").innerHTML = "";
    $("#TaxPer").css("border-color", "#ced4da");
}
function CheckValidation() {

    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var validate = true;
    if ($("#TaxPer").val() == "" || parseFloat($("#TaxPer").val()) == 0) {
        document.getElementById("vmTaxPer").innerHTML = $("#valueReq").text();
        $("#TaxPer").css("border-color", "red");
        validate = false;
    }
    if (validate == true) {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }
}
function CheckValidationTaxHSN() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_saveHSN").css("filter", "grayscale(100%)");
        $("#btn_saveHSN").prop("disabled", true); /*End*/
    }

    var validate = true;
    if ($("#listTaxPer").val() == "" || $("#listTaxPer").val() == "0") {
        //document.getElementById("vmlistTaxPer").innerHTML = $("#valueReq").text();
        $('[aria-labelledby="select2-listTaxPer-container"]').css("border-color", "red");
        $("#vmlistTaxPer").text($("#valueReq").text());
        validate = false;
    }
    if ($("#HSNNumber").val() == "" || $("#HSNNumber").val() == "0") {
        //document.getElementById("vmHSNNumber").innerHTML = $("#valueReq").text();
        $('[aria-labelledby="select2-HSNNumber-container"]').css("border-color", "red");
        $("#vmHSNNumber").text($("#valueReq").text());
        validate = false;
    }
    if (validate == true) {
        $("#btn_saveHSN").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }
}
function functionConfirm(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var Tax_id = clickedrow.find("#taxperc").text();
    $("#hdnigsttaxperc").val(Tax_id);

    debugger;
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
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function DeleteHSNNumber(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var hsn_no = clickedrow.find("#hsn_no").text();
    $("#hdnlistTaxHsn").val(hsn_no);
    debugger;
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
            $("#hdnAction").val("DeleteHSN");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function FloatValuePerOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
}