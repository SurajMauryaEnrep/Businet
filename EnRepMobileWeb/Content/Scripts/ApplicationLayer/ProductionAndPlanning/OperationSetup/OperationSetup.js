
$(document).ready(function () {

    $("#ddl_ShopfloorName").select2();

    $("#OperationSetupListBody").bind("dblclick", function (e) {
        debugger;
        try { 
            var clickedrow = $(e.target).closest("tr");
            var QCId = clickedrow.children("#count").text();
            var op_id = clickedrow.children("#op_id").text();
            var op_name = clickedrow.children("#op_name").text();
            var op_type = clickedrow.children("#op_type").text();
            var remarks = clickedrow.children('#op_remarks').text();
            var op_type_id = clickedrow.children("#op_type_id").text();
            var shfl_id = clickedrow.find("#tdshfl_id").text();
            var Ws_id = clickedrow.find("#tdws_id").text();
            var supervisor = clickedrow.find("#tdsupervisor").text();

            /**Commented By Nitesh 05022025 For Remarks Is not Show Complite**/ 
            //window.location = "/ApplicationLayer/OperationSetup/dbClickEdit/?op_id1=" + op_id + "&op_name=" + op_name + "&op_type_id="
            //    + op_type_id + '&op_remarks=' + remarks + "&shfl_id=" + shfl_id + "&wrkstn_id=" + Ws_id + "&supervisor=" + supervisor;
            window.location = "/ApplicationLayer/OperationSetup/dbClickEdit/?op_id1=" + encodeURIComponent(op_id) +
                "&op_name=" + encodeURIComponent(op_name) +
                "&op_type_id=" + encodeURIComponent(op_type_id) +
                "&op_remarks=" + encodeURIComponent(remarks) +
                "&shfl_id=" + encodeURIComponent(shfl_id) +
                "&wrkstn_id=" + encodeURIComponent(Ws_id) +
                "&supervisor=" + encodeURIComponent(supervisor);
            
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
});

function OnChangeShopfloore() {
    var Shfl_id = $("#ddl_ShopfloorName").val();
    
    if (Shfl_id != null && Shfl_id != "" && Shfl_id != "0") {
        $("#hdn_ShopfloorName").val(Shfl_id);
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/OperationSetup/BindWorkStationList",
                data: {
                    shfl_id: Shfl_id,
                },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        $('#ddl_WorkstationList').empty();
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $('#ddl_WorkstationList').append(`<option value=0>---Select---</option>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#ddl_WorkstationList').append(`<option value="${arr.Table[i].ws_id}">${arr.Table[i].ws_name}</option>`);
                            }
                        }
                        else {
                            $('#ddl_WorkstationList').append(`<option value=0>---Select---</option>`);
                        }
                    }
                },
            });
    }
}
function OnChangeWorkstation() {
    var wrk_id = $("#ddl_WorkstationList").val();
    if (wrk_id != null && wrk_id != null && wrk_id != "0") {
        $("#hdn_WorkstationList").val(wrk_id);
    }
}

function validateOPSetupInsertform() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var op_name = $("#op_name").val();
    //alert('test validation');
    var ValidationFlag = true;
    if (op_name == '') {
        document.getElementById("vmop_name").innerHTML = $("#valueReq").text();
        $("#op_name").css("border-color", "red");
        ValidationFlag = false;
    }
    //var ddlOpTypeList = $("#ddlOpTypeList").val();
    //if (ddlOpTypeList == '0') {
    //    document.getElementById("vmop_name").innerHTML = $("#valueReq").text();
    //    $("#op_name").css("border-color", "red");
    //    ValidationFlag = false;
    //}

    if ($("#ddlOpTypeList").val() == '0') {
        document.getElementById("vmddlOpTypeList").innerHTML = $("#valueReq").text();
        $("#ddlOpTypeList").css("border-color", "red");
        ValidationFlag = false;
    }


    if (ValidationFlag == true) {
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }

    else {
        return false;
    }
}
function OnChangeopname() {
    debugger;    
    var op_name = $('#op_name').val().trim();
    if (op_name != "") {
        document.getElementById("vmop_name").innerHTML = "";
        $("#op_name").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmop_name").innerHTML = $("#valueReq").text();
        $("#op_name").css("border-color", "red");
    }
}

function OnChangeddlOpTypeList() {
    debugger;
    var ddlOpTypeList = $('#ddlOpTypeList').val().trim();
    if (ddlOpTypeList != "0") {
        document.getElementById("vmddlOpTypeList").innerHTML = "";
        $("#ddlOpTypeList").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmddlOpTypeList").innerHTML = $("#valueReq").text();
        $("#ddlOpTypeList").css("border-color", "red");
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
