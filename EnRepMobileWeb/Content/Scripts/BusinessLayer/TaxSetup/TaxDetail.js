/************************************************
Javascript Name:TAx Account Setup
Created By:Mukesh
Created Date: 120-01-2020
Description: This Javascript use for the Tax setup many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    $("#GLAccGrp").select2();
    $("#tax_acc_id").select2();
   
    if ($("#TAX").is(":checked")) { /*Add by Hina on 07-08-2025*/
        $("#Div_SectionCode").css("display", "none");
    }
    if ($("#TDS").is(":checked")) { /*Add by Hina on 07-08-2025*/
        $("#Div_SectionCode").css("display", "");
    }
    ReadBranchList();
    var PageName = sessionStorage.getItem("MenuName");
    $('#TaxDetailsPageName').text(PageName);

   

    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
});

function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var Branch = {};
        Branch.Id = row.find("#hdCustomerBranchId").val();
        var checkBoxId = "#cust_act_stat_" + Branch.Id;
        if (row.find(checkBoxId).is(":checked")) {
            Branch.BranchFlag = "Y";
        }
        else {
            Branch.BranchFlag = "N";
        }
        Branches.push(Branch);
    });
    debugger;
    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
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
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(2) - 1)) {
    //    return false;
    //}
    var TaxPer = $('#TaxPer').val().trim();
    if (TaxPer != "") {
        document.getElementById("vmTaxPer").innerHTML = "";
        $("#TaxPer").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmTaxPer").innerHTML = $("#valueReq").text();
        $("#TaxPer").css("border-color", "red");
    }
    return true;
}

function validateTaxInsertform() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var Taxname = $("#TaxName").val();
    var ValidationFlag = true;
    if (Taxname == '') {
        document.getElementById("vmTaxName").innerHTML = $("#valueReq").text();
        $("#TaxName").css("border-color", "red");
        ValidationFlag = false; 
    }
    if ($("#TaxPer").val() == '') {
        document.getElementById("vmTaxPer").innerHTML = $("#valueReq").text();
        $("#TaxPer").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccGrp").val() == '0') {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#tax_auth_acc_id").val() == '' || $("#tax_auth_acc_id").val()=="0") {
        document.getElementById("vmtaxAuthAccid").innerHTML = $("#valueReq").text();
        $("#tax_auth_acc_id").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#tax_doc_dt").val() == '') {
        document.getElementById("vmAppDate").innerHTML = $("#valueReq").text();
        $("#tax_doc_dt").css("border-color", "red");
        ValidationFlag = false;
    }
    debugger;
    if (ValidationFlag == true) {
        debugger;
        $("#act_status").prop("disabled", false);
    }
    if (ValidationFlag == true) {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");

        $("#TAX").attr("disabled", false)
        $("#TDS").attr("disabled", false)
        return true;
    }
    else {
        return false;
    }
}
function OnChangeAccGrp() {
    debugger;
    var GrpName = $('#GLAccGrp').val().trim();
    if (GrpName != "") {
        $("#SpanAccGrp").css("display", "none");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "#ced4da");
    }
    else {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
    }
}
function OnChangeTaxName(e) {
    debugger;
    var TaxName = $('#TaxName').val().trim();
    if (TaxName != "") {
        document.getElementById("vmTaxName").innerHTML = "";
        $("#TaxName").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmTaxName").innerHTML = $("#valueReq").text();
        $("#TaxName").css("border-color", "red");
    }
    if (e.key === "_") {
        e.preventDefault();
    }
}
function OnChangeTaxPer() {
    debugger;
    var TaxPer = $('#TaxPer').val().trim();
    if (TaxPer != "") {
        document.getElementById("vmTaxPer").innerHTML = "";
        $("#TaxPer").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmTaxPer").innerHTML = $("#valueReq").text();
        $("#TaxPer").css("border-color", "red");
    }
}
function OnChangeTaxCoa() {
    debugger;
    var TaxCoa = $('#tax_acc_id').val().trim();
    if (TaxCoa != "") {
        $("#spanacctype").css("display", "none");
        $("[aria-labelledby='select2-tax_acc_id-container']").css("border-color", "#ced4da");
        //document.getElementById("vmAccid").innerHTML = "";
        //$("#tax_acc_id").css("border-color", "#ced4da");
    }
    else {
        $('#spanacctype').text($("#valueReq").text());
        $("#spanacctype").css("display", "block");
        $("[aria-labelledby='select2-tax_acc_id-container']").css("border-color", "red");
        //document.getElementById("vmAccid").innerHTML = $("#valueReq").text();
        //$("#tax_acc_id").css("border-color", "red");
    }
}
function OnChangeTaxAuthCoa() {
    debugger;
    var TaxAuthCoa = $('#tax_auth_acc_id').val().trim();
    if (TaxAuthCoa != "") {
        document.getElementById("vmtaxAuthAccid").innerHTML = "";
        $("#tax_auth_acc_id").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmtaxAuthAccid").innerHTML = $("#valueReq").text();
        $("#tax_auth_acc_id").css("border-color", "red");
    }
}
function OnChangeTaxAppDate() {
    debugger;
    var TaxDate = $('#tax_doc_dt').val().trim();
    if (TaxDate != "") {
        document.getElementById("vmAppDate").innerHTML = "";
        $("#tax_doc_dt").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmAppDate").innerHTML = $("#valueReq").text();
        $("#tax_doc_dt").css("border-color", "red");
    }
}
function MandatoryDisable() {
    $("#TaxName").attr("style", "border-color: #ced4da;");
    $("#Spantaxname").attr("style", "display: none;");
    ;
    $("#SpanGLPer").attr("style", "display: none;");
    $("#TaxPer").attr("style", "border-color: #ced4da;");
    ;
    $("#spanacctype").attr("style", "display: none;");
    $("#tax_acc_id").attr("style", "border-color: #ced4da;");


}
function onclickEditButton() {
    debugger;
    debugger;
    $("#recoverable").prop("disabled", false);
    $("#man_calc").prop("disabled", false);
    $("#act_status").prop("disabled", false);
}
$("#TaxPer").keypress(function (e) {
    if (e.which == 46) {
        if ($(this).val().indexOf('.') != -1) {
            return false;
        }
    }
    if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function OnClkTaxType() {/*Add by Hina on 07-08-2025*/
    debugger;
    if ($("#TAX").is(":checked")) { 
        $("#Div_SectionCode").css("display", "none");
    }
    if ($("#TDS").is(":checked")) { 
        $("#Div_SectionCode").css("display", "block");
    }
}

