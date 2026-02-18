
$(document).ready(function () {
    debugger;
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    });

    $("#TbodyUserRoleSetup").bind("dblclick", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var RoleID = clickedrow.children("td:eq(2)").text();
            var HOID = clickedrow.children("td:eq(4)").text();
            if (RoleID != "" && RoleID != null) {
                window.location.href = "/SecurityLayer/UserRoleSetup/EditUserRoleSetupDetails/?RoleID=" + RoleID + "&HOID=" + HOID;
            }
        }
        catch (err) {
        }
    });
});

function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var RoleName = $('#TxtRoleName').val();
    var ValidationFlag = true;

    if (RoleName == "") {
        document.getElementById("vmrole_name").innerHTML = $("#valueReq").text();
        $("#TxtRoleName").css("border-color", "red");
        ValidationFlag = false;
    }

    if (ValidationFlag == true) {
            if (CheckUserRoleValidations() == true) {
                var UserRole_DetailList = new Array();

                $('input[type=checkbox]:checked').each(function () {
                    var ID = (this.checked ? $(this).attr("id") : "");
                    var featureid = $("#hf_FID" + ID).val();
                    var Docid = $("#hf_DocID" + ID).val();
                    var UserRoleList = {};

                    UserRoleList.feature_id = featureid;
                    UserRoleList.doc_id = Docid;
                    UserRoleList.grant_access = "Y";

                    UserRole_DetailList.push(UserRoleList);
                });

                var str = JSON.stringify(UserRole_DetailList);
                $('#hdUserRoleDetailListDetailList').val(str);
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                return true;
            }
            else {
                swal("", $("#noitemselectedmsg").text(), "warning");
                return false;
            }
        
    }
    else {
        return false;
    }
}
function CheckUserRoleValidations() {
    debugger;
    var ErrorFlag = "Y";
    $('input[type=checkbox]:checked').each(function () {
        ErrorFlag = "N";
    });

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnTextRoleName() {
    debugger;
    var RoleName = $('#TxtRoleName').val();
    if (RoleName != "" && RoleName != null) {
        document.getElementById("vmrole_name").innerHTML = "";
        $("#TxtRoleName").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmrole_name").innerHTML = $("#valueReq").text();
        $("#TxtRoleName").css("border-color", "red");
    }
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}