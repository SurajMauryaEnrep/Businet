$(document).ready(function () {
    $("#CostCenterlist").select2();
    $("#cc_id_inbranch").select2();
    $("#ModuleCC_id").select2();
});
function CCSetupSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_saveCCname").css("filter", "grayscale(100%)");
        $("#btn_saveCCname").prop("disabled", true); /*End*/
    }
    if (CheckVallidation("cc_name", "vmcc_name") == false) {
        return false;
    } else {
        $("#btn_saveCCname").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function functionConfirm(e) {
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
            debugger
            var currentRow = $(e.target).closest('tr');
            var cc_id = currentRow.find("#HdnCC_Id").val();
        
            window.location.href = "/BusinessLayer/CostCenterSetup/deleteCCSetup/?cc_id=" + cc_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function EditCostCenter(e) {
    debugger;
    document.getElementById("vmcc_name").innerHTML = "";
    $("#cc_name").css("border-color", "#ced4da");

    var CurrentRow = $(e.target).closest('tr');
    var cc_id = CurrentRow.find("#HdnCC_Id").val();
    var cc_name = CurrentRow.find("#cc_name1").text();

    $("#cc_name").val(cc_name).trigger('change');
    $("#cc_id").val(cc_id);
    $("#btn_saveCCname").val("UpdateCC_name");
    $("#hdnSavebtn").val("");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#cc_name").attr("disabled", false);
    }
}
function OnChangeCostCenter() {
    CheckVallidation("cc_name", "vmcc_name");
}
//------------------------------------------------------------------------------//
function CCValueSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickccvalue") {
        $("#btn_saveCCValue").css("filter", "grayscale(100%)");
        $("#btn_saveCCValue").prop("disabled", true); /*End*/
    }
    if (CheckVallidation("cc_val_name", "vmcc_val_name") == false || CheckVallidation("CostCenterlist", "VmCostCenterlist") == false) {
        if ($("#CostCenterlist").val() == "0") {
            $("[aria-labelledby='select2-CostCenterlist-container']").css("border-color", "red");
        }
        else {
            $("[aria-labelledby='select2-CostCenterlist-container']").css("border-color", "#ced4da");
        }
        CheckVallidation("CostCenterlist", "VmCostCenterlist")
        return false;
    }
    else {
        $("#btn_saveCCValue").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickccvalue");
        return true;
    }
}
function EditCostCenterValue(e) {
    debugger;
    document.getElementById("vmcc_val_name").innerHTML = "";
    $("#cc_val_name").css("border-color", "#ced4da");

    var CurrentRow = $(e.target).closest('tr');
    var cc_id = CurrentRow.find("#hdn_cc_idval").text();
    var cc_val_name = CurrentRow.find("#cc_name3").text();
    var cc_val_id = CurrentRow.find("#HdnCC_val_Id").val();

    $("#CostCenterlist").val(cc_id).trigger('change');
    $("#cc_val_name").val(cc_val_name).trigger('change');
    $("#cc_val_id").val(cc_val_id);
    $("#btn_saveCCValue").val("UpdateCC_val_name");
    $("#hdnSavebtn").val("");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableValue").css("display", "block");
        $("#CostCenterlist").attr("disabled", false);
        $("#cc_val_name").attr("disabled", false);
    }
}
function OnChangeCostCenterValue() {
    CheckVallidation("cc_val_name", "vmcc_val_name");
}
function OnChangeCostCenterDropdownValue() {
    CheckVallidation("CostCenterlist", "VmCostCenterlist");
    if ($('#CostCenterlist').val() == "" || $('#CostCenterlist').val() == "0") {

        $("[aria-labelledby='select2-CostCenterlist-container']").css("border-color", "red");
    }
    else {
        $("[aria-labelledby='select2-CostCenterlist-container']").css("border-color", "#ced4da");
    }
}
function functionValueConfirm(e) {
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
            debugger
            var currentRow = $(e.target).closest('tr');
            var cc_val_id = currentRow.find("#HdnCC_val_Id").val();
            window.location.href = "/BusinessLayer/CostCenterSetup/deleteCCSetup_val/?cc_val_id=" + cc_val_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}
//--------------------------------------------------------------------------------------------------//
function CCValue_branchSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickbranch") {
        $("#btn_saveBrnachValue").css("filter", "grayscale(100%)");
        $("#btn_saveBrnachValue").prop("disabled", true); /*End*/
    }
    if (CheckVallidation("cc_id_inbranch", "vmcc_id_inbranch") == false || CheckVallidation("BranchName", "vmbranchname") == false) {
        if ($("#cc_id_inbranch").val() == "0") {
            $("[aria-labelledby='select2-cc_id_inbranch-container']").css("border-color", "red");
        }
        else {
            $('[aria-labelledby="select2-cc_id_inbranch-container"]').css("border-color", "#ced4da");
        }
        CheckVallidation("BranchName", "vmbranchname");
        return false;
    }
    else {
        $("#btn_saveBrnachValue").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickbranch");
        return true;
    }
}
function OnChangebranchCC_idName() {
    CheckVallidation("cc_id_inbranch", "vmcc_id_inbranch");
}
function OnChangeCostCenterDDL_inBr() {

    CheckVallidation("cc_id_inbranch", "vmcc_id_inbranch");
    if ($('#cc_id_inbranch').val() == "" || $('#cc_id_inbranch').val() == "0") {

        $("[aria-labelledby='select2-cc_id_inbranch-container']").css("border-color", "red");
    }
    else {
        $("[aria-labelledby='select2-cc_id_inbranch-container']").css("border-color", "#ced4da");
    }

    var ddlcc_id = $("#cc_id_inbranch").val();
    $.ajax({
        type:"POST",
        url: "/BusinessLayer/CostCenterSetup/GetBranchOnchangeCC",
        data: { ddlcc_id: ddlcc_id },
        success: function (data) {
            debugger
            var arr = JSON.parse(data);
            var option = "";
            for (var i = 0; i < arr.Table.length; i++) {
                option += "<option value=" + arr.Table[i].Comp_Id + ">" + arr.Table[i].comp_nm +"</option>"
            }
            $("#BranchName").html(option);
        }
    });
}
function OnChangeCostCenterDDLin_module() {

    CheckVallidation("ModuleCC_id", "VmModuleCC_id");
    if ($('#ModuleCC_id').val() == "" || $('#ModuleCC_id').val() == "0") {

        $("[aria-labelledby='select2-ModuleCC_id-container']").css("border-color", "red");
    }
    else {
        $("[aria-labelledby='select2-ModuleCC_id-container']").css("border-color", "#ced4da");
    }
    var DDLModulecc_id = $("#ModuleCC_id").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/CostCenterSetup/GetModuleOnchangeCC",
        data: { DDLModulecc_id: DDLModulecc_id },
        success: function (data) {
            debugger
            var arr = JSON.parse(data);
            var option = "<option value= '0'>---Select---</option>";
            for (var i = 0; i < arr.Table.length; i++) {
                option += "<option value=" + arr.Table[i].doc_id + ">" + arr.Table[i].doc_name_eng + "</option>"
            }
            $("#Module").html(option);
        }
    });
}
function functionCC_BrancConfirm(e) {
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
            debugger
            var currentRow = $(e.target).closest('tr');
            var cc_id = currentRow.find("#hdn_cc_Branch_idval").text();
            var br_id = currentRow.find("#HdnCC_Branch_val_Id").text();
            window.location.href = "/BusinessLayer/CostCenterSetup/deleteCC_branc/?cc_id=" + cc_id + "&br_id=" + br_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}
//--------------------------------------------------------------------------------------------------//
function CC_ModuleSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickModule") {
        $("#btn_savemoduleValue").css("filter", "grayscale(100%)");
        $("#btn_savemoduleValue").prop("disabled", true); /*End*/
        
    }
    if (CheckVallidation("ModuleCC_id", "VmModuleCC_id") == false || CheckVallidation("Module", "vmModule") == false) {
        if ($('#ModuleCC_id').val() == "" || $('#ModuleCC_id').val() == "0") {

            $("[aria-labelledby='select2-ModuleCC_id-container']").css("border-color", "red");
        }
        else {
            $("[aria-labelledby='select2-ModuleCC_id-container']").css("border-color", "#ced4da");
        }
        CheckVallidation("Module", "vmModule")
            return false;
    } else {
        $("#btn_savemoduleValue").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickModule");
            return true;
        }
}
function OnChangeCCDDL() {
    CheckVallidation("ModuleCC_id", "VmModuleCC_id");
}
function functionCC_ModuleConfirm(e) {
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
            debugger
            var currentRow = $(e.target).closest('tr');
            var cc_id = currentRow.find("#hdn_ccModule").text();
            var module_id = currentRow.find("#HdnCC_ModuleId").text();
            $("#hdnSavebtn").val("");
            window.location.href = "/BusinessLayer/CostCenterSetup/deleteCC_Module/?cc_id=" + cc_id + "&module_id=" + module_id;
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnChangeCCModule() {
    CheckVallidation("Module", "vmModule");
}