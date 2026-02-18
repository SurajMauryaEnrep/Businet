/************************************************
Javascript Name:GL Account Setup
Created By:Mukesh
Created Date: 12-12-2020
Description: This Javascript use for the Supplier setup many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    
  /*  $("#TAp").css("disabled", true)*/
    
    var AccType = $('#acc_type').val().trim();
    if (AccType != "7") {
        if (AccType != "") {
            document.getElementById("TAp").checked = false;
            $("#TAp").attr("disabled", true)
            $("#ODLimit").css("display", "none");
        }
        else {
            if ($("#Disable").val() == "Disable") {
                $("#TAp").attr("disabled", true)
            }
            else {
                $("#TAp").attr("disabled", false)
            }
            //$("#TAp").attr("disabled", false)
        }
    }
    else {
       
        if ($("#Disable").val() == "Disable") {
            $("#TAp").attr("disabled", true)
        }
        else {
            $("#TAp").attr("disabled", false)
        }
        //$("#TAp").attr("disabled", false)
    }
    if (AccType == "7") {
        $("#bankdetailModule").css("display", 'block')
    }
    else {
        $("#bankdetailModule").css("display", 'none')
    }
    $("#ODLimit").css("display", "none");
    if ($("#TAp").is(":checked")) {
        tgl = "Y";
    }
    else {
        tgl = "N";
    }
    if (tgl == "Y") {
        $("#ODLimit").css("display", "block");
    }
    else {
        $("#ODLimit").css("display", "none");
    }
    //if ($("#Disable").val() == "Disable") {
    //    $("#TAp").attr("disabled", true)
    //}
    //else {
    //    $("#TAp").attr("disabled", false)
    //}
    ReadBranchList();
    $("#GLAccGrp").select2();
    $("#AccountGroupName").select2({
        ajax: {
            url: $("#GLAccGrp").val(),

            data: function (params) {
                var queryParameters = {              
                    ddlGroup: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },
    });
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
});
function onclickEditbtn() {
    debugger;
    //$("#acc_type").attr("disabled", false);
    //$("#GLAccGrp").attr("disabled", false);
    //$("#act_status_tr").prop("disabled", false);
    //$("#TPA").prop("disabled", false);
    //$("#roa").prop("disabled", false);
    //$("#iwt").prop("disabled", false);
    //$("#plr").prop("disabled", false);
    //$("#egl").prop("disabled", false);
    //$("#ibt").prop("disabled", false);
    //$("#sta").prop("disabled", false);
    //$("#tr").prop("disabled", false);
    //$("#tp").prop("disabled", false);
    var AccType = $('#acc_type').val().trim();
    if (AccType != "7") {
        if (AccType != "") {
            document.getElementById("TAp").checked = false;
            $("#TAp").attr("disabled", true)
            $("#ODLimit").css("display", "none");
        }
        else {
            $("#TAp").attr("disabled", false)
        }
    }
    else {
        $("#TAp").attr("disabled", false)
    }
    var Overdraft = $("#Overdraft_Limit").val();
    var QtyDecDigit = $("#QtyDigit").text();
    $("#Overdraft_Limit").val(parseFloat(Overdraft).toFixed(QtyDecDigit));
}
function ActStatusDisable() {
    debugger;
    ($("#act_status_tr").is(":checked"))
    $("#act_status_tr").prop("disabled", true);
}
function onoverdraft() {
    debugger;

   /* var toggle = $("#TAp").val();*/
    if ($("#TAp").is(":checked")) {
        tgl = "Y";
    }
    else {
        tgl = "N";
    }
    if (tgl == "Y") {
        $("#ODLimit").css("display", "block");
        //$("#ODLimit").css("col-md-2 ");
        //$("#ODLimit").addClass("col-md-2 ");
       
    }
    else {
        $("#ODLimit").css("display", "none");
        $("#Overdraft_Limit").val("");
        //$("#ODLimit").css("col-md-2 ");
        //$("#ODLimit").removeClass("col-md-2 "); 
    }
   
}

function validateGLInsertform() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
        return false;
    }
    var GLname = $("#acc_name").val();
    var reason = $("#inact_reason").val();
    var AccName = $('#Overdraft_Limit').val();
    var curr = $('#curr').val();
    $('#hdn_currid').val(curr);
    if ($("#act_status_tr").is(":checked")) {
        active = "Y";
    }
    else {
        active = "N";
    }
    if ($("#TAp").is(":checked")) {
        tgl = "Y";
    }
    else {
        tgl = "N";
    }
    var ValidationFlag = true;
    debugger;
    if (GLname == '') {
        document.getElementById("vmAcc_name").innerHTML = $("#valueReq").text();
        $("#acc_name").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccGrp").val() == '0') {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        //document.getElementById("vmAccountGroup").innerHTML = $("#valueReq").text();
        //$("#GLAccGrp").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#acc_type").val() == '') {
        document.getElementById("vmAccounttype").innerHTML = $("#valueReq").text();
        $("#acc_type").css("border-color", "red");
        ValidationFlag = false;
    }
    if (active === 'Y') {
        $("#inact_reason").attr("style", "border-color: #ced4da;");
        $("#vminactreason").attr("style", "display: none;");
        $("#reasonrequired").attr("style", "display: none;");
    }
    if (active === 'N' && reason === '') {
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vminactreason").innerHTML = $("#valueReq").text();
        $("#vminactreason").css("display", "block");
        $("#reasonrequired").css("display", "inherit");
        ValidationFlag = false;
    }
    debugger;
    
    if (tgl == 'Y') {  
        if( AccName == "" || AccName == 0) {
            $("#Overdraft_Limit").attr("style", "border-color:#ff0000;", "required");
            document.getElementById("vmOver").innerHTML = $("#valueReq").text();
            $("#Overdraft_Limit").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    if (tgl == 'N') {
        $("#Overdraft_Limit").attr("style", "border-color: #ced4da;");
        $("#vmOver").attr("style", "display: none;");

    }
    if (ValidationFlag == true) {
        if ($("#acc_type").val() == "7" || $("#acc_type").val() == 7) {
            if (bankDetail() == false) {
                ValidationFlag = false;
            }
        }
    }
    if (ValidationFlag == true) {
        $("#act_status_tr").attr("disabled", false);
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }
}
function bankDetail() {
    var bank_add =  $("#BankAddress").val();
    var bank_acc_no=  $("#AccountNumber").val();
    var SWIFT_Code = $("#SWIFT_Code").val();
    var ifsc_code = $("#IFSC_Code").val();
    var valFalg = true;
    if (bank_add == "") {
        document.getElementById("vmbank_add").innerHTML = $("#valueReq").text();
        $("#BankAddress").css('border-color', 'Red');
        $("#vmbank_add").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("vmbank_add").innerHTML = "";
        $("#BankAddress").css('border-color', '#ced4da');
        $("#vmbank_add").attr("style", "display: none;");

    }

    if (bank_acc_no == "") {
        document.getElementById("vmbank_acc_no").innerHTML = $("#valueReq").text();
        $("#AccountNumber").css('border-color', 'Red');
        $("#vmbank_acc_no").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("vmbank_acc_no").innerHTML = "";
        $("#AccountNumber").css('border-color', '#ced4da');
        $("#vmbank_acc_no").attr("style", "display: none;");

    }
    if (SWIFT_Code == "") {
        document.getElementById("vmswift_code").innerHTML = $("#valueReq").text();
        $("#SWIFT_Code").css('border-color', 'Red');
        $("#vmswift_code").attr("style", "display: block;");
        valFalg = false;
    }
    else {


        document.getElementById("vmswift_code").innerHTML = "";
        $("#SWIFT_Code").css('border-color', '#ced4da');
        $("#vmswift_code").attr("style", "display: none;");
    }

    if (ifsc_code == "") {
        document.getElementById("vmifsc_code").innerHTML = $("#valueReq").text();
        $("#IFSC_Code").css('border-color', 'Red');
        $("#vmifsc_code").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("vmifsc_code").innerHTML = "";
        $("#IFSC_Code").css('border-color', '#ced4da');
        $("#vmifsc_code").attr("style", "display: none;");
    }
    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
}
function onkeypressAddress() {
    $("#vmbank_add").css("display", "none");
    $("#BankAddress").css("border-color", "#ced4da");
    $("#vmbank_add").css("display", "none");

}
function onkeypressAcco() {
    $("#vmbank_acc_no").css("display", "none");
    $("#AccountNumber").css("border-color", "#ced4da");
    $("#vmbank_acc_no").css("display", "none");

}
function onkeypressshiftcode() {
    $("#vmswift_code").css("display", "none");
    $("#SWIFT_Code").css("border-color", "#ced4da");
    $("#vmswift_code").css("display", "none");

}
function onkeypressifsccode() {
    $("#vmifsc_code").css("display", "none");
    $("#IFSC_Code").css("border-color", "#ced4da");
    $("#vmifsc_code").css("display", "none");

}
function OnChangeAccName() {
    debugger;
    var AccName = $('#acc_name').val().trim();
    if (AccName != "") {
        document.getElementById("vmAcc_name").innerHTML = "";
        $("#acc_name").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmAcc_name").innerHTML = $("#valueReq").text();
        $("#acc_name").css("border-color", "red");
    }
}
function functionConfirm(event) {
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
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
function OnChangeAccGrp() {
    debugger;
    var GrpName = $('#GLAccGrp').val().trim();
    if (GrpName != "") {
        //document.getElementById("vmAccountGroup").innerHTML = "";
        //$("#GLAccGrp").css("border-color", "#ced4da");
        $("#SpanAccGrp").css("display", "none");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "#ced4da");
    }
    else {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        //document.getElementById("vmAccountGroup").innerHTML = $("#valueReq").text();
        //$("#GLAccGrp").css("border-color", "red");
    }
}
function OnChangeAccType() {
    debugger;
    var AccType = $('#acc_type').val().trim();
    var AccTypey = $('#acc_type').val();
    if (AccType != "") {
        document.getElementById("vmAccounttype").innerHTML = "";
        $("#acc_type").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmAccounttype").innerHTML = $("#valueReq").text();
        $("#acc_type").css("border-color", "red");     
            
    }
    if (AccType == "1" || AccType == "2" || AccType == "3" || AccType == "7" || AccType == "8") {
        $("#CurrDiv").css("display", 'block')
        if (AccType == "7") {
            $("#bankdetailModule").css("display", 'block')
        }
        else {
        $("#bankdetailModule").css("display", 'none')
        }
    }
    else {
        $("#CurrDiv").css("display", 'none')
        $("#bankdetailModule").css("display", 'none')
    }
    if (AccType != "7") {
      /*  document.getElementById("TAp").checked = true; // Check the checkbox*/
        document.getElementById("TAp").checked = false;
        // Uncheck the checkbox
        $("#TAp").attr("disabled", true)
        $("#ODLimit").css("display", "none");
        return false;
    }
    else {
        $("#TAp").attr("disabled", false)
    }
   
}
function OnChangeReason() {
    debugger;
    var Reason = $('#inact_reason').val().trim();
    if (Reason != "") {
        document.getElementById("vminactreason").innerHTML = "";
        $("#inact_reason").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vminactreason").innerHTML = $("#valueReq").text();
        $("#inact_reason").css("border-color", "red");
    }
}
function OnActiveStatusChange() {
    debugger;
    if ($("#act_status_tr").is(":checked")) {
        $("#inact_reason").prop("disabled", true);
        document.getElementById("vminactreason").innerHTML = "";
        $("#inact_reason").css("border-color", "#ced4da");
        $("#reasonrequired").attr("style", "display: none;");
    }
    else {
        $("#inact_reason").prop("disabled", false);
        document.getElementById("vminactreason").innerHTML = $("#valueReq").text();
        $("#inact_reason").css("border-color", "red");
        $("#reasonrequired").css("display", "inherit");
    }
}
function mandatorydisable() {
    $("#acc_name").attr("style", "border-color: #ced4da;");
    $("#spanglname").attr("style", "display: none;");
    ;
    $("#spanacctype").attr("style", "display: none;");
    $("#acc_type").attr("style", "border-color: #ced4da;");
    ;
    $("#spanaccgrp").attr("style", "display: none;");
    $("#accountgroupname").attr("style", "border-color: #ced4da;");

}

function OnclickReqQty() {
    debugger;
    //if (AvoidChar(el, "QtyDigit") == false) {
    //    return false;
    //}
    var QtyDecDigit = $("#QtyDigit").text();

  var  Overdraft = $("#Overdraft_Limit").val();
    var AccName = $('#Overdraft_Limit').val();
    if (AccName != "") {
        document.getElementById("vmOver").innerHTML = "";
        $("#Overdraft_Limit").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmOver").innerHTML = $("#valueReq").text();
        $("#Overdraft_Limit").css("border-color", "red");
    }
    $("#Overdraft_Limit").val(parseFloat(Overdraft).toFixed(QtyDecDigit));
}
function RqtyFloatValueonly(el, evt) {
  
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        document.getElementById("vmOver").innerHTML = "";
        $("#Overdraft_Limit").css("border-color", "#ced4da");
        return true;
    }
   
      
    }
/*---------------Add by Hina on 03-09-2024 to all 4 toggle----------------------*/
function ChcekAllToggle() {
    debugger;
    var tgl = "";
    if ($("#plr").is(":checked") || $("#ibt").is(":checked") || $("#iwt").is(":checked") || $("#bra").is(":checked")) {
        tgl = "Y";
        ($("#act_status_tr").is(":checked"))
        $("#act_status_tr").prop("disabled", true);
    }
    else {
        tgl = "N";
        ($("#act_status_tr").is(":checked"))
        $("#act_status_tr").prop("disabled", false);
    }
    if (tgl == "Y") {
        var Branches = new Array();
        $("#CustBrDetail TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var Branch = {};
            Branch.Id = row.find("#hdCustomerBranchId").val();
            var checkBoxId = "#cust_act_stat_" + Branch.Id;
            if (row.find(checkBoxId).is(":checked")) {
                row.find(checkBoxId).attr("disabled", true);
                Branch.BranchFlag = "Y";
            }
            else {
                row.find(checkBoxId).prop("checked", true);
                row.find(checkBoxId).attr("disabled", true);
                Branch.BranchFlag = "Y";
            }
            Branches.push(Branch);
        });
        debugger;
        var str = JSON.stringify(Branches);
        $('#hdCustomerBranchList').val(str);
    }
    else {
        var Branches = new Array();
        $("#CustBrDetail TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var Branch = {};
            Branch.Id = row.find("#hdCustomerBranchId").val();
            var checkBoxId = "#cust_act_stat_" + Branch.Id;
            if (row.find(checkBoxId).is(":checked")) {
                row.find(checkBoxId).attr("disabled", false);
                Branch.BranchFlag = "Y";
            }
            else {
                //row.find(checkBoxId).prop("checked", true);
                row.find(checkBoxId).attr("disabled", false);
                Branch.BranchFlag = "N";
            }
            Branches.push(Branch);
        });
        debugger;
        var str = JSON.stringify(Branches);
        $('#hdCustomerBranchList').val(str);
    }
}

