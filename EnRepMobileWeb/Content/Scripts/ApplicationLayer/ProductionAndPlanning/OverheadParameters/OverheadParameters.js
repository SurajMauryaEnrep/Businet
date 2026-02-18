
$(document).ready(function () {
    $("#OverheadParameterListBody").bind("dblclick", function (e) {
        debugger;
        try {
              var clickedrow = $(e.target).closest("tr");
            var ohd_exp_id = clickedrow.children("#ohd_exp_id").text();
              window.location.href = "/ApplicationLayer/OverheadParameters/EditOhdParam/?ohd_exp_id1=" + ohd_exp_id;
         }
        catch (err) {
            debugger
        }
    }); 
});

function OnChangeohdname() {
    debugger;
    var ohd_exp_name = $('#ohd_exp_name').val().trim();
    if (ohd_exp_name != "") {
        document.getElementById("vmohd_name").innerHTML = "";
        $("#ohd_exp_name").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmohd_name").innerHTML = $("#valueReq").text();
        $("#ohd_exp_name").css("border-color", "red");
    }
}
function OnChangeddlUom() {
    debugger;
    var Uom = $('#ddlUOM').val().trim();
    if (Uom != "0") {
        document.getElementById("vmUom").innerHTML = "";
        $("#ddlUOM").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmUom").innerHTML = $("#valueReq").text();
        $("#ddlUOM").css("border-color", "red");
    }
}

function validateInsertform() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var ohd_exp_name = $("#ohd_exp_name").val().trim();
    var ValidationFlag = true;
     if (ohd_exp_name == '') {
        document.getElementById("vmohd_name").innerHTML = $("#valueReq").text();
        $("#ohd_exp_name").css("border-color", "red");
         ValidationFlag = false;
     }
   
    if ($("#ddlUOM").val() == '0')
    {
        document.getElementById("vmUom").innerHTML = $("#valueReq").text();
        $("#ddlUOM").css("border-color", "red");
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

