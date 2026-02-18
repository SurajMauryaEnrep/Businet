/*Modified by Suraj on 15-03-2024 to remove third party Name*/
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
    $("#HSN_cd").select2();
    ReadBranchList();
    var PageName = sessionStorage.getItem("MenuName");
    $('#OCDetailsPageName').text(PageName);
    $("#TaxTemplate").select2();
    //$("#tp_name").select2();
    $("#oc_acc").select2();
    //var Tp_name = $("#tp_name").val();
    //if (Tp_name != 0) {
    //    $("#ThirdPartyName").css("display", "inherit");
    //}
    StarValidationShowAndHide();
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
});
function StarValidationShowAndHide() {
    var Template_name = $("#TaxTemplate").val();
    var HSN_cd = $("#HSN_cd").val();
    var GSTApplicable = $("#GstApplicable").val();
    
    if ($("#TaxApplicable").is(":checked")) {
        TaxApplicable = "Y";
    }
    else {
        TaxApplicable = "N";
    }
    if (TaxApplicable == "Y" || GSTApplicable == "Y") {
        $("#Templaterequired").css("display", "inherit");
        $("#hsnrequired").css("display", "inherit");
    }
    else {
        $("#Templaterequired").css("display", "none");
        $("#hsnrequired").css("display", "none");
    }
}
function onchangeOCType() {
    debugger;
    if ($("#TP").is(":checked")) {
        TP = "Y";
        $("#ThirdPartyName").css("display", "inherit");
    }
    else {
        TP = "N";
        $("#ThirdPartyName").attr("style", "display: none;");
    }
    //    if (TP === 'N') {
    //        $("#tp_name").attr('disabled', true);
    //        $('#tp_name').val(0).trigger('change')
    //    }
    //    else {
    //        $("#tp_name").attr('disabled', false);
    //        $("#tp_name").val(0);
    //    }
    //    document.getElementById("vmTPid").innerHTML = "";
    //    $("#tp_name").attr("style", "border-color: #ced4da;");
    //$('#tp_name').val(0).trigger('change')
}

function MandatoryDisable() {
    $("#OtherChargeName").attr("style", "border-color: #ced4da;");
    $("#SpanOCname").attr("style", "display: none;");

    //$("#spanTPName").attr("style", "display: none;");
    //$("#tp_name").attr("style", "border-color: #ced4da;");

    $("#spanocacc").attr("style", "display: none;");
    $("#oc_acc").attr("style", "border-color: #ced4da;");

    $("#Spandate").attr("style", "display: none;");
    $("#oc_doc_dt").attr("style", "border-color: #ced4da;");

}


function CheckFormValidation() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    var hdnSavebtn = $("#EnableforhdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick" && hdnSavebtn != "Duplicate") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    else {
        $("#hdnSavebtn").val("");
    }
    var OCName = $('#OtherChargeName').val();
    var OcAcc = $('#oc_acc').val();
    var Ocdt = $('#oc_doc_dt').val();
    //var tpname = $('#tp_name').val();
    var TaxTemplate = $("#TaxTemplate").val();
    var HSN_no = $("#HSN_cd").val();
    if ($("#TP").is(":checked")) {
        TP = "Y";
    }
    else {
        TP = "N";
    }
    debugger;
    if ($("#TaxApplicable").is(":checked")) {
        TaxApplicable = "Y";
    }
    else {
        TaxApplicable = "N";
    }
    var ValidationFlag = true;
    if (TaxApplicable == "Y" && TaxTemplate == "0") {
        $("[aria-labelledby='select2-TaxTemplate-container']").css("border-color", "red");
        $("#SpanTaxTemplate").text($("#valueReq").text());
        $("#SpanTaxTemplate").css("display", "block");
        ValidationFlag = false;
    }
    if (TaxApplicable == "Y" && (HSN_no == "0" || HSN_no == "" || HSN_no == null)) {
        $("[aria-labelledby='select2-HSN_cd-container']").css("border-color", "red");
        $("#HSN_cd").css("border-color", "red");
        $("#SpanHSNErrorMsg").text($("#valueReq").text());
        $("#SpanHSNErrorMsg").css("display", "block");
        ValidationFlag = false;
    }
    if (Ocdt == "") {
        document.getElementById("vmAppDate").innerHTML = $("#valueReq").text();
        $("#oc_doc_dt").css("border-color", "red");
        $("#oc_doc_dt").css("display", "block");
        ValidationFlag = false;
    }

    if (OCName == "") {
        document.getElementById("vmOCName").innerHTML = $("#valueReq").text();
        $("#OtherChargeName").css("border-color", "red");
        ValidationFlag = false;
    }
    if (OcAcc == "" || OcAcc == "0") {
        $("[aria-labelledby='select2-oc_acc-container']").css("border-color", "red");
        $("#Span_oc_name").text($("#valueReq").text());
        $("#Span_oc_name").css("display", "block");
        //document.getElementById("vmAccid").innerHTML = $("#valueReq").text();
        //$("#oc_acc").css("border-color", "red");
        ValidationFlag = false;
    }
    var GSTApplicable = $("#GstApplicable").val();
    if (GSTApplicable == "Y" && (HSN_no == "0" || HSN_no == "" || HSN_no == null)) {
        $("[aria-labelledby='select2-HSN_cd-container']").css("border-color", "red");
        $("#HSN_cd").css("border-color", "red");
        $("#SpanHSNErrorMsg").text($("#valueReq").text());
        $("#SpanHSNErrorMsg").css("display", "block");
        ValidationFlag = false;
    }
    /*Commented by Suraj on 15-03-2024 to remove Third Party Name*/
    //if (TP == "Y" && tpname == "0") {
    //    $("[aria-labelledby='select2-tp_name-container']").css("border-color", "red");
    //    $("#Span_tp_name").text($("#valueReq").text());
    //    $("#Span_tp_name").css("display", "block");
    //        //document.getElementById("vmTPid").innerHTML = $("#valueReq").text();
    //        //$("#tp_name").css("border-color", "red");
    //        ValidationFlag = false;
    //}

    if (ValidationFlag == true) {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }

    else {
        return false;
    }
}
function onclickEditButton() {
    debugger;
    $("#TaxApplicable").prop("disabled", false);
    $("#act_status").prop("disabled", false);
}
function OnChangeOCName() {
    debugger;
    var OCName = $('#OtherChargeName').val().trim();
    if (OCName != "") {
        document.getElementById("vmOCName").innerHTML = "";
        $("#OtherChargeName").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmOCName").innerHTML = $("#valueReq").text();
        $("#OtherChargeName").css("border-color", "red");
    }
}
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
function OnChangedate() {
    debugger;
    var OCdate = $('#oc_doc_dt').val().trim();
    if (OCdate != "") {
        document.getElementById("vmAppDate").innerHTML = "";
        $("#oc_doc_dt").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmAppDate").innerHTML = $("#valueReq").text();
        $("#oc_doc_dt").css("border-color", "red");
    }
}
function OnChangeGL() {
    debugger;
    var GL = $('#oc_acc').val().trim();
    /* if (GL != "0") {*/
    if (GL != "") {
        $("#Span_oc_name").css("display", "none");
        $("[aria-labelledby='select2-oc_acc-container']").css("border-color", "#ced4da");
        //document.getElementById("vmAccid").innerHTML = "";
        //$("#oc_acc").css("border-color", "#ced4da");
    }
    else {
        $("[aria-labelledby='select2-oc_acc-container']").css("border-color", "red");
        $("#Span_oc_name").text($("#valueReq").text());
        $("#Span_oc_name").css("display", "block");
        //document.getElementById("vmAccid").innerHTML = $("#valueReq").text();
        //$("#oc_acc").css("border-color", "red");
    }
}
function onchangeTaxTemplate() {
    debugger;
    var TaxTemplate = $("#TaxTemplate").val().trim();
    if (TaxTemplate != "") {
        $("#SpanTaxTemplate").css("display", "none");
        $("[aria-labelledby='select2-TaxTemplate-container']").css("border-color", "#ced4da");
    }
    else {
        $("[aria-labelledby='select2-TaxTemplate-container']").css("border-color", "red");
        $("#SpanTaxTemplate").text($("#valueReq").text());
        $("#SpanTaxTemplate").css("display", "block");
    }
}
function onchangeHSn_no() {
    debugger;
    var hsn_id = $("#HSN_cd").val();
    if (hsn_id != null && hsn_id != "0" && hsn_id != "") {
        $("#hdHsnID").val(hsn_id);
    }
    else {
        $("#hdHsnID").val("");
    }
    $("#SpanHSNErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-HSN_cd-container']").css("border-color", "#ced4da");

}
//function onchangeTP() {
//    debugger;
//    var TP = $('#oc_acc').val().trim();
//    /* if (TP != "0") {*/
//    if (TP != "") {
//        $("#Span_tp_name").css("display", "none");
//        $("[aria-labelledby='select2-tp_name-container']").css("border-color", "#ced4da");
//        //document.getElementById("vmTPid").innerHTML = "";
//        //$("#tp_name").css("border-color", "#ced4da");
//    }
//    else {
//        $("[aria-labelledby='select2-tp_name-container']").css("border-color", "red");
//        $("#Span_tp_name").text($("#valueReq").text());
//        $("#Span_tp_name").css("display", "block");
//        //document.getElementById("vmTPid").innerHTML = $("#valueReq").text();
//        //$("#tp_name").css("border-color", "red");
//    }
//}
function OnClickTaxApplicable() {
    debugger;
    var GSTApplicable = $("#GstApplicable").val();
    if ($("#TaxApplicable").is(":checked")) {
        $("#TaxTemplate").attr("disabled", false);
        $("#Templaterequired").css("display", "inherit");
        $("#hsnrequired").css("display", "inherit");
    } else {
        $("#TaxTemplate").attr("disabled", true);
        $("#TaxTemplate").val(0).trigger("change");
        if (GSTApplicable == "Y") {
            $("#hsnrequired").css("display", "inherit");
        }
        else {
            $("#hsnrequired").attr("style", "display: none;");
        }
        $("#Templaterequired").attr("style", "display: none;");
        onchangeTaxTemplate();
    }
}


