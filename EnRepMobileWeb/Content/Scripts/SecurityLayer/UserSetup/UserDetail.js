
$(document).ready(function () {
    $("#IDReportingTo").select2();
    $('#UserRoleList tbody').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {            
            var id = $(this).attr('id');            
            var idx = $(this).children('.row-index').children('p');            
            var dig = parseInt(id.substring(1));            
            idx.html(`Row ${dig - 1}`);            
            $(this).attr('id', `R${dig - 1}`);
        });        
        $(this).closest('tr').remove();
        debugger;
        var Roll_ID = $(this).closest('tr')[0].cells[3].childNodes[0].data;  
        var HO_ID = $(this).closest('tr')[0].cells[1].childNodes[0].data;  
        $("#RoleName option[value=" + Roll_ID + "]").removeClass("select2-hidden-accessible");
        $("#RoleHoName option[value=" + HO_ID + "]").show();
        BindBrHO();
        resetBrHO(HO_ID);
    });
    $('#UserBranchList tbody').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        var ItemCode = $(this).closest('tr')[0].cells[3].childNodes[0].data;
        $("#BranchName option[value=" + ItemCode + "]").removeClass("select2-hidden-accessible");

    });
    debugger;
  
    var hdnuser_id = $("#hdnUser_id").val();
    if (hdnuser_id == 0 || hdnuser_id == null || hdnuser_id=="") {
        $("#act_stats").prop("checked", true);
        }
    

    $("#RoleName").prop("disabled", true);//BranchName
    $("#BranchName").prop("disabled", true);
    BindBrHO();
    hideHeadOffice();
    $(this).on('click', '.simpleTree-label', function (e) {
        debugger;

        var User_id = this.nextSibling.innerText;

        var Parent = this.innerText;
        GetDataUserDetail(User_id)
    });

   
});
function OnChangeUserNm() {
    var user_nm = $("#user_nm").val();

    if (user_nm === "" || user_nm === null) {
        $("#Spanusername").html($("#valueReq").text()).show();
        $("#user_nm").css("border-color", "red");
        errorFlag = "Y";
    } else {
        $("#Spanusername").hide();
        $("#user_nm").css("border-color", "#ced4da");

        // Sanitize input and set value
        var filteredVal = allowLoginChars1(user_nm);
        $("#ddlLogInID").val(filteredVal);
        OnChangeLogInID();
    }
}
function OnChangeLogInID() {
    var LogInID = $("#ddlLogInID").val();

    if (LogInID === "" || LogInID === null) {
        $("#SpanLogInID").html($("#valueReq").text()).show();
        $("#ddlLogInID").css("border-color", "red");
        errorFlag = "Y";
    } else {
        $("#SpanLogInID").hide();
        $("#ddlLogInID").css("border-color", "#ced4da");

       
    }
}
function allowLoginChars1(input) {
    // Allow only letters, numbers, and @ # $ & .
    return input.replace(/[^a-zA-Z0-9@#$&. ]/g, '');
}
function allowLoginChars(input) {
    // Allow: letters, numbers, @ # $ & .
    input.value = input.value.replace(/[^a-zA-Z0-9@#$&. ]/g, '');
}
function FilterSearchTreeTree(searchValue, $container)
{
    debugger;
    if (!searchValue) {
        // If search is empty, show all nodes
        $container.find('.simpleTree-nodeContainer').show();
        $container.find('.simpleTree-childrenContainer').show();
        return;
    }

    searchValue = searchValue.trim().toUpperCase();

    $container.children('.simpleTree-nodeContainer').each(function () {
        var $node = $(this);
        var nodeValue = $node.children('.simpleTree-value').text().trim().toUpperCase();
        var nodeLabel = $node.children('.simpleTree-label').text().trim().toUpperCase();

        var $childContainer = $node.children('.simpleTree-childrenContainer');

        // Recursively filter children firstsssss
        var childMatches = false;
        if ($childContainer.length) {
            childMatches = FilterSearchTreeTree(searchValue, $childContainer);
        }

        // Check if current node matches
        var currentMatch = nodeValue.includes(searchValue) || nodeLabel.includes(searchValue);

        if (currentMatch || childMatches) {
            $node.show();
            if ($childContainer.length) $childContainer.show();
        } else {
            $node.hide();
            if ($childContainer.length) $childContainer.hide();
        }
    });

    // Return true if any node in this container matches
    return $container.children('.simpleTree-nodeContainer:visible').length > 0;
}



function GetDataUserDetail(User_id) {
    debugger;
    window.location.href = "/SecurityLayer/UserDetail/UserDetail/?UserId=" + User_id;
}
function resetBrHO(HO_ID) {
    $("#UserBranchList tbody tr").each(function () {
        debugger;
        var curruntRow = $(this);
        var TblHO_ID = curruntRow.find("td:eq(1)").text();
        if (TblHO_ID == HO_ID) {
            curruntRow.remove();
        }
        
    });
}
function AddNewRoleAcc() {
    var RoleHoNametxt = $("#RoleHoName option:selected").text();
    var RoleNametxt = $("#RoleName option:selected").text();
    var RoleHoNameval = $("#RoleHoName option:selected").val();
    var RoleNameval = $("#RoleName option:selected").val();
 
    debugger;
    if (RoleHoNameval == "0") {
        document.getElementById("vmRoleHoName").innerHTML = $("#valueReq").text();
        $("#vmRoleHoName").css("display", "block");
        $("#RoleHoName").css("border-color", "red");
        return false;
    }
    else {
        $("#vmRoleHoName").css("display", "none");
        $("#RoleHoName").css("border-color", "#ced4da");
    }
    if (RoleNameval == "0") {
        document.getElementById("vmRoleName").innerHTML = $("#valueReq").text();
        $("#vmRoleName").css("display", "block");
        $("[aria-labelledby='select2-RoleName-container']").css("border-color", "red");
        return false;
    }
    else {
        $("#vmRoleName").css("display", "none");
        $("[aria-labelledby='select2-RoleName-container']").css("border-color", "#ced4da");
    }
   
    var RowNo =1;
    $("#UserRoleList tbody tr").each(function () {
        debugger;
        RowNo = parseFloat($(this).find("#HiddenRowID").text()) + 1;
    });

    $("#UserRoleList tbody").append(` <tr id="R${RowNo}">
<td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="@Resource.Delete"></i></td>
<td style="display:none">${RoleHoNameval}</td>
<td>${RoleHoNametxt}</td>
<td style="display:none">${RoleNameval}</td>
<td>${RoleNametxt}</td>
<td><span id="HiddenRowID" style="display:none;">${RowNo}</span></td>
</tr>`)
    $("#RoleHoName").val(0);
    $("#RoleName").val(0).trigger('change');
    $("#RoleName").prop("disabled", true);
    $("#RoleName option[value=" + RoleNameval + "]").select2().hide();
    //$("#RoleHoName option[value=" + RoleHoNameval + "]").hide();

    hideHeadOffice();

    BindBrHO();
    OnChangeBR_HO();
}
function hideHeadOffice() {
    $("#UserRoleList tbody tr").each(function () {
        debugger;
       
        var HO_ID = $(this).find("td:eq(1)").text();
        $("#RoleHoName option[value=" + HO_ID + "]").hide();

    });
}
function BindBrHO() {
    $('#BR_HoName').empty();
    $('#BR_HoName').append(`<option value="0">---Select---</option>`);
    $("#UserRoleList tbody tr").each(function () {
        debugger;
        var currentRow = $(this);
        var HO_name = currentRow.find("td:eq(2)").text();
        var HO_ID = currentRow.find("td:eq(1)").text();
        $('#BR_HoName').append(`<option value="${HO_ID}">${HO_name}</option>`);

    });
}
function AddNewBranchAcc() {
    var BRHoNametxt = $("#BR_HoName option:selected").text();
    var BRNametxt = $("#BranchName option:selected").text();
    var BRHoNameval = $("#BR_HoName option:selected").val();
    var BRNameval = $("#BranchName option:selected").val();
   
    debugger;
    if (BRHoNameval == "0") {
        document.getElementById("vmBR_HoName").innerHTML = $("#valueReq").text();
        $("#vmBR_HoName").css("display", "block");
        $("#BR_HoName").css("border-color", "red");
        return false;
    }
    else {
        $("#vmBR_HoName").css("display", "none");
        $("#BR_HoName").css("border-color", "#ced4da");
    }
    if (BRNameval == "0") {
        document.getElementById("vmBranchName").innerHTML = $("#valueReq").text();
        $("#vmBranchName").css("display", "block");
        $("[aria-labelledby='select2-BranchName-container']").css("border-color", "red");
        return false;
    }
    else {
        $("#vmBranchName").css("display", "none");
        $("[aria-labelledby='select2-BranchName-container']").css("border-color", "#ced4da");
    }
    var RowNo=1;
    $("#UserBranchList tbody tr").each(function () {
        debugger;
        RowNo = parseFloat($(this).find("#HiddenRowID").text()) + 1;
    });
    
    var SNo = 0;
    $("#UserBranchList tbody tr").each(function () {
        debugger;
        var CHO_ID = $(this).find("td:eq(1)").text();
        SNo = $(this).find("#HiddenRowID").text();
        if (CHO_ID == BRHoNameval) {
            $("#act_stats_Branch_" + SNo).attr("checked", false);
            $(this).find("#spanHiddenBr_" + SNo).text("N");
        }        
        SNo++;
    });

    $("#UserBranchList tbody").append(` <tr id="R${RowNo}">
<td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="@Resource.Delete"></i></td>
<td style="display:none">${BRHoNameval}</td>
<td>${BRHoNametxt}</td>
<td style="display:none">${BRNameval}</td>
<td>${BRNametxt}</td>
<td><div class="custom-control custom-switch margin-left25" style="margin-bottom:0px;">
<input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch" onclick=defBrchk(event) id="act_stats_Branch_${RowNo}" checked>
<label class="custom-control-label col-md-9 col-sm-12" for="act_stats_Branch_${RowNo}" style="padding: 3px 0px;"></label><span id="spanHiddenBr_${RowNo}" style="display:none">Y</span></div></td>
<td><span id="HiddenRowID" style="display:none;">${RowNo}</span></td>
</tr>`)
 
    $("#BranchName").val(0).trigger('change');
    $("#BranchName option[value=" + BRNameval + "]").select2().hide();

}
function defBrchk(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var RowID = clickedrow.find("#HiddenRowID").text();
    var HO_ID = clickedrow.find("td:eq(1)").text();
    var rowIdx = 0;
    if (clickedrow.find("#act_stats_Branch_" + RowID).is(":checked")) {
        
        $("#UserBranchList TBODY TR").each(function () {
            debugger;
            rowIdx = $(this).find("#HiddenRowID").text();
            var CHO_ID = $(this).find("td:eq(1)").text();
            if (CHO_ID == HO_ID) {
                $(this).find("#act_stats_Branch_" + rowIdx).prop("checked", false);
                $(this).find("#spanHiddenBr_" + rowIdx).text("N");
            }
        
        });
        clickedrow.find("#act_stats_Branch_" + RowID).prop("checked", true);
        clickedrow.find("#spanHiddenBr_" + RowID).text("Y");       
    }
    else {
        clickedrow.find("#spanHiddenBr_" + RowID).text("N");        
    }

}
function OnClickHideSelectedRole() {
    debugger;
    if ($("#RoleHoName option:selected").val()=="0") {
        $('#RoleName').empty();
        $('#RoleName').append(`<option value="0">---Select---</option>`);
    }

    $("#UserRoleList tbody tr").each(function () {
        debugger;
        var currentrow = $(this);
        var role_ID = currentrow.find("td:eq(3)").text();
        //$("#RoleName").val(role_ID).hide();
        $("#RoleName option[value=" + role_ID + "]").select2().hide();

    });
}
function OnchangeHOGetRole() {
    debugger;
    var HO_ID = $("#RoleHoName").val();
    $.ajax({
        type: "POST",
        url: "/SecurityLayer/UserDetail/GetRoleName",
        data: { HO_ID: HO_ID },
        success: function (data) {
            debugger;
           
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                $("#RoleName").prop("disabled", false);
                if (arr.Table.length > 0) {
                    
                    $('#RoleName').empty();
                    $('#RoleName').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#RoleName').append(`<option value="${arr.Table[i].role_id}">${arr.Table[i].role_name}</option>`);
                    }
                    $("#UserRoleList tbody tr").each(function () {
                        debugger;
                        var currentrow = $(this);
                        var role_ID = currentrow.find("td:eq(3)").text();
                        var hoid = currentrow.find("td:eq()").text();
                        if (hoid == HO_ID) {
                            $("#RoleName option[value=" + role_ID + "]").select2().hide();
                        }
                    });
                    var firstEmptySelect = true;
                    $('#RoleName').select2({
                        templateResult: function (data) {                            
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row w-100">' +
                                '<div class="col-md-12 col-xs-12 ml-1' + classAttr + '">' + data.text + '</div>' +
                                '</div>'
                            );
                            return $result;
                            firstEmptySelect = false;
                        }
                    });


                }
                else {
                    $('#RoleName').empty();
                    $('#RoleName').append(`<option value="0">---Select---</option>`);
                 
                }
            }
        }

    })

}

function OnChangeBR_HO() {

    debugger;
    var HO_ID = $("#BR_HoName option:selected").val();
    if (HO_ID != "0") {
        $("#BranchName").prop("disabled", false);
    }
    else {
        $("#BranchName").prop("disabled", true);
        $('#BranchName').empty();
        $('#BranchName').append(`<option value="0">---Select---</option>`);
    }
    $.ajax({
        type: "POST",
        url: "/SecurityLayer/UserDetail/GetBranchName",
        data: { HO_ID: HO_ID },
        success: function (data) {
            debugger;

            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                
                if (arr.Table.length > 0) {
                    $('#BranchName').empty();
                    $('#BranchName').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#BranchName').append(`<option value="${arr.Table[i].Comp_Id}">${arr.Table[i].comp_nm}</option>`);
                    }
                    $("#UserBranchList tbody tr").each(function () {
                        debugger;
                        var currentrow = $(this);
                        var role_ID = currentrow.find("td:eq(3)").text();
                        $("#BranchName option[value=" + role_ID + "]").select2().hide();
                    });
                    var firstEmptySelect = true;
                    $('#BranchName').select2({
                        templateResult: function (data) {
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row w-100">' +
                                '<div class="col-md-12 col-xs-12 ml-1' + classAttr + '">' + data.text + '</div>' +
                                '</div>'
                            );
                            return $result;
                            firstEmptySelect = false;
                        }
                    });


                }
                else {
                    $('#BranchName').empty();
                    $('#BranchName').append(`<option value="0">---Select---</option>`);
                   
                }
            }
        }

    })

}
function HideShowPWD() {
    debugger;
    if ($("#user_pwd").prop("type") == "password") {
        $("#user_pwd").prop("type", "text");
    }
    else {
        $("#user_pwd").prop("type", "password");
    }
}
function HideShowEmailPWD() {
    debugger;
    if ($("#Password").prop("type") == "password") {
        $("#Password").prop("type", "text");
    }
    else {
        $("#Password").prop("type", "password");
    }
}
function OnChangeGender() {
    var gender = $("#gender").val();
    if (gender == "0") {
        document.getElementById("Spangender").innerHTML = $("#valueReq").text();
        $("#Spangender").css("display", "block");
        $("#gender").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spangender").css("display", "none");
        $("#gender").css("border-color", "#ced4da");
    }
}

function OnChangePassWd() {
    var user_pwd = $("#user_pwd").val();
    if (user_nm == "" || user_pwd == null) {
        document.getElementById("Spanuserpwd").innerHTML = $("#valueReq").text();
        $("#Spanuserpwd").css("display", "block");
        $("#user_pwd").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanuserpwd").css("display", "none");
        $("#user_pwd").css("border-color", "#ced4da");
    }
}
function OnChangeSenderMail() {
    var sender_email = $("#SenderEmail").val();
    if (sender_email == "" || sender_email == null) {
        document.getElementById("Spansender").innerHTML = $("#valueReq").text();
        $("#Spansender").css("display", "block");
        $("#SenderEmail").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spansender").css("display", "none");
        $("#SenderEmail").css("border-color", "#ced4da");
    }
}
function OnChangeMailPassWd() {
    var mail_pwd = $("#Password").val();
    if (mail_pwd == "" || mail_pwd == null) {
        document.getElementById("Spanmailpwd").innerHTML = $("#valueReq").text();
        $("#Spanmailpwd").css("display", "block");
        $("#Password").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanmailpwd").css("display", "none");
        $("#Password").css("border-color", "#ced4da");
    }
}
function OnChangeHostServer() {
    var host_server = $("#HostServer").val();
    if (host_server == "" || host_server == null) {
        document.getElementById("Spanhostserver").innerHTML = $("#valueReq").text();
        $("#Spanhostserver").css("display", "block");
        $("#HostServer").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanhostserver").css("display", "none");
        $("#HostServer").css("border-color", "#ced4da");
    }
}
function OnChangePort() {
    debugger;
    var port = $("#Port").val();
    if (port == 0 || port == null) {
        document.getElementById("Spanport").innerHTML = $("#valueReq").text();
        $("#Spanport").css("display", "block");
        $("#Port").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanport").css("display", "none");
        $("#Port").css("border-color", "#ced4da");
    }
}
function InsertUserSetupDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var RoleHoNameval = $("#RoleHoName option:selected").val();
    var RoleNameval = $("#RoleName option:selected").val();
    var user_nm = $("#user_nm").val();
    var LogInID = $("#ddlLogInID").val();
    var user_pwd = $("#user_pwd").val();
    var gender = $("#gender").val();
    var errorFlag = "N";
    var Flag = "N";

    var sender_email = $("#SenderEmail").val();
    var mail_pwd = $("#Password").val();
    var host_server = $("#HostServer").val();
    var port = $("#Port").val();
    var ssl_flag = $("#SSLFlag").val();
    var user_deflt_cred = $("#DefaultCredentials").val();

    if (sender_email != "" || mail_pwd != "" || host_server != "" || port != "0") {
        Flag = "Y";
    }

    if (gender == "0") {
        document.getElementById("Spangender").innerHTML = $("#valueReq").text();
        $("#Spangender").css("display", "block");
        $("#gender").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spangender").css("display", "none");
        $("#gender").css("border-color", "#ced4da");
    }
    if (user_nm == "" || user_nm == null) {
        document.getElementById("Spanusername").innerHTML = $("#valueReq").text();
        $("#Spanusername").css("display", "block");
        $("#user_nm").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanusername").css("display", "none");
        $("#user_nm").css("border-color", "#ced4da");
    }
    if (LogInID == "" || LogInID == null || LogInID == "0") {
        document.getElementById("SpanLogInID").innerHTML = $("#valueReq").text();
        $("#SpanLogInID").css("display", "block");
        $("#ddlLogInID").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#SpanLogInID").css("display", "none");
        $("#ddlLogInID").css("border-color", "#ced4da");
    }
    if (user_pwd == "" || user_pwd == null) {
        document.getElementById("Spanuserpwd").innerHTML = $("#valueReq").text();
        $("#Spanuserpwd").css("display", "block");
        $("#user_pwd").css("border-color", "red");
        errorFlag = "Y";
    }
    else {
        $("#Spanuserpwd").css("display", "none");
        $("#user_pwd").css("border-color", "#ced4da");
    }
    if (Flag == "Y") {
        if (sender_email == "" || sender_email == null) {
            document.getElementById("Spansender").innerHTML = $("#valueReq").text();
            $("#Spansender").css("display", "block");
            $("#SenderEmail").css("border-color", "red");
            errorFlag = "Y";
        }
        else {
            $("#Spansender").css("display", "none");
            $("#SenderEmail").css("border-color", "#ced4da");
        }
        if (mail_pwd == "" || mail_pwd == null) {
            document.getElementById("Spanmailpwd").innerHTML = $("#valueReq").text();
            $("#Spanmailpwd").css("display", "block");
            $("#Password").css("border-color", "red");
            errorFlag = "Y";
        }
        else {
            $("#Spanmailpwd").css("display", "none");
            $("#Password").css("border-color", "#ced4da");
        }
        if (host_server == "" || host_server == null) {
            document.getElementById("Spanhostserver").innerHTML = $("#valueReq").text();
            $("#Spanhostserver").css("display", "block");
            $("#HostServer").css("border-color", "red");
            errorFlag = "Y";
        }
        else {
            $("#Spanhostserver").css("display", "none");
            $("#HostServer").css("border-color", "#ced4da");
        }
        if (port == 0 || port == null) {
            document.getElementById("Spanport").innerHTML = $("#valueReq").text();
            $("#Spanport").css("display", "block");
            $("#Port").css("border-color", "red");
            errorFlag = "Y";
        }
        else {
            $("#Spanport").css("display", "none");
            $("#Port").css("border-color", "#ced4da");
        }
    }
    if (errorFlag == "Y") {
        return false;
    }
    if ($("#UserRoleList tbody tr").length == 0) {
        swal("", $("#AtLeastOneUserRoleIsReq").text(), "warning");
        return false;
    }
    if ($("#UserBranchList tbody tr").length == 0) {
        swal("", $("#AtLeastOneBranchAccessIsReq").text(), "warning");
        return false;
      
    }
    if (sender_email != "" || sender_email != null) {
        var emailPattern = /^[a-zA-Z0-9_.+\-]+[\x40][a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$/;
        if ((sender_email !== "") && (!emailPattern.test(sender_email))) {
            $("#vmEmail").text("Invalid email format.");
            $("#vmEmail").addClass('right');
            $("#SenderEmail").css("border-color", "red");
            return false;
        }
        else {
            $("#vmEmail").text("");
            $("#SenderEmail").css("border-color", "#ced4da");
        }
    }
    
    var FinalRoleDetail = [];
    var FinalBranchDetail = [];
    debugger;
    FinalRoleDetail = InsertRoleDetail();
    FinalBranchDetail = InsertBranchDetail();
    var RoleDt = JSON.stringify(FinalRoleDetail);
    var BranchDt = JSON.stringify(FinalBranchDetail);

    
    $("#RoleDetail").val(RoleDt);
    $("#BranchDetail").val(BranchDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

}
function InsertRoleDetail() {
    var RoleDetailList = new Array();

    $("#UserRoleList tbody tr").each(function () {
        var currentRow = $(this);
        debugger;
        var rowNo = currentRow.find("#HiddenRowID").text()
        var RoleDt = {};
        RoleDt.HO_id = currentRow.find("td:eq(1)").text();
        RoleDt.HO_nm = currentRow.find("td:eq(2)").text();
        RoleDt.Role_id = currentRow.find("td:eq(3)").text();
        RoleDt.Role_nm = currentRow.find("td:eq(4)").text();
        RoleDt.user_id = "";//currentRow.find("#spanHiddenRoleAct" + rowNo).text();
        RoleDetailList.push(RoleDt);

    });

    return RoleDetailList;

}
function InsertBranchDetail() {
    var BranchDetailList = new Array();
    $("#UserBranchList tbody tr").each(function () {
        debugger;
        var currentRow = $(this);
        var RowNo = currentRow.find("#HiddenRowID").text();
        var BranchList = {};
        BranchList.HO_id = currentRow.find("td:eq(1)").text();
        BranchList.HO_nm = currentRow.find("td:eq(2)").text();
        BranchList.BR_id = currentRow.find("td:eq(3)").text();
        BranchList.BR_nm = currentRow.find("td:eq(4)").text();
        BranchList.Status = currentRow.find("#spanHiddenBr_" + RowNo).text();
        BranchDetailList.push(BranchList);
        debugger;
    })
    return BranchDetailList;

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
                debugger;
            $("#hdnAction").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function showPreview(event) {
    if (event.target.files.length > 0) {
        let src = URL.createObjectURL(event.target.files[0]);
        let preview = document.getElementById("file-ip-2-preview");
        preview.src = src;
        preview.style.display = "block";
    }
}
//function showPreview_User(e) {
//    debugger;
//    if (e.target.files.length > 0) {
//        let src1 = URL.createObjectURL(e.target.files[0]);
//        let preview1 = document.getElementById("file-ip-1-preview");
//        preview1.src1 = src1;
//        preview1.style.display = "block";
//    }
//}
function myImgRemove() {
    var dfltimg = $("#defltimg")[0].src;
    document.getElementById("file-ip-2-preview").src = dfltimg;
    document.getElementById("file-ip-2").value = null;
    $("#hdn_Attatchment_details").val(null);
    $("#hdnAttachment").val(null);
}
function myImgRemove1() {
    var dfltimg = $("#defltimg1")[0].src;
    document.getElementById("file-ip-1-preview").src = dfltimg;
    document.getElementById("file-ip-1").value = null;
    $("#UserImagehdn_Attatchment_details").val(null);
    $("#hdnAttachment_UserImg").val(null);
}
function showPreview_User(event) {
    var reader = new FileReader();
    reader.onload = function () {
        // Set the preview image to the selected file
        var output = document.getElementById('file-ip-1-preview');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);

    // Optionally, update the hidden field with the selected image path (you might want to handle this server-side)
    //document.getElementById('UserImagehdn_Attatchment_details').value = event.target.files[0].name;
}
//Added by Nidhi on 13-06-2025
function validateEmail() {
    debugger
    var email = $("#SenderEmail").val().trim();
    var emailPattern = /^[a-zA-Z0-9_.+\-]+[\x40][a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$/;
    if ((email !== "") && (!emailPattern.test(email))) {
        $("#vmEmail").text("Invalid email format.");
        $("#vmEmail").addClass('right');
        $("#SenderEmail").css("border-color", "red");
        return false;
    }
    else {
        $("#vmEmail").text("");
        $("#SenderEmail").css("border-color", "#ced4da");
        return true;
    }
}
function isNumberKey(evt) {
    debugger;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    // Allow only numbers
    if (charCode < 48 || charCode > 57)
        return false;
    return true;
}