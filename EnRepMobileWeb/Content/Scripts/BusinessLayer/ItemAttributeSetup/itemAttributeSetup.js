
$(document).ready(function () {
    $("#L_attrName").select2();
    EditAttrName();
    EditAttrValName();
})

function ChakeValidation() {
    var validate = true;
    if ($("#AttributeName").val() == "") {
        document.getElementById("vmAttributeName").innerHTML = $("#valueReq").text();
        $("#AttributeName").css("border-color", "red");
        validate = false;
    }
    if (validate == true) {
        $("#AttributeName").attr("disabled", false)
        return true;
    }
    else {
        return false;
    }
}
function ChakeValidationAttrVal() {
    var validate = true;
    if ($("#L_attrName").val() == 0) {
        $("[aria-labelledby='select2-L_attrName-container']").css("border-color", "red");
        $("#VMattrName").text($("#valueReq").text());
        $("#VMattrName").css("display", "block");
        //document.getElementById("vmL_attrName").innerHTML = $("#valueReq").text();
        //$("#L_attrName").css("border-color", "red");
        validate = false;
    }
    if ($("#AttrVal").val() == "") {
        document.getElementById("vmAttrVal").innerHTML = $("#valueReq").text();
        $("#AttrVal").css("border-color", "red");
        validate = false;
    }
    if (validate == true) {
        $("#L_attrName").attr("disabled", false)
        $("#AttrVal").attr("disabled", false)
        return true;
    }
    else {
        return false;
    }
}
function onchangeAttrName() {

    if ($("#AttributeName").val() != "") {
        document.getElementById("vmAttributeName").innerHTML = "";
        $("#AttributeName").css("border-color", "#ced4da");
       // validationFlag = false;
    }
}
function onchangeAttrVal_Id() {
    if ($("#L_attrName").val() != 0) {
        $("#VMattrName").css("display", "none");
        $("[aria-labelledby='select2-L_attrName-container']").css("border-color", "#ced4da");
        //document.getElementById("vmL_attrName").innerHTML = "";
        //$("#L_attrName").css("border-color", "#ced4da");
        // validationFlag = false;
    }
}
function onchangeAttrVal_Name() {
    if ($("#AttrVal").val() != 0) {
        document.getElementById("vmAttrVal").innerHTML = "";
        $("#AttrVal").css("border-color", "#ced4da");
        // validationFlag = false;
    }
}
function functionConfirm(event) {
    $("#AttrNameList").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            /*var attr_Id = clickedrow.children("td:eq(2)").text();   */
           var attr_Id = clickedrow.children("#hdnAttrId").text();
            $("#hdnAttributeID").val(attr_Id);
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
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
            $("#hdnAction").val("DeleteAttrName");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function functionConfirmAV(event) {
    $("#AttrValList").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            /*var attr_val_Id = clickedrow.children("td:eq(2)").text();*/
           var attr_val_Id = clickedrow.children("#hdnAttrvalId").text();
            $("#hdnAttrValId").val(attr_val_Id);
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
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
            $("#hdnAction").val("DeleteAttrVal");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function EditAttrName() {
    debugger;
    //var flag = 'N';
    $(".editAttrName").bind("click", function (e) {
        debugger;
        try {
            document.getElementById("vmAttributeName").innerHTML = "";
            $("#AttributeName").css("border-color", "#ced4da");
            if ($("#AttributeName").val() == "") {
                document.getElementById("vmAttributeName").innerHTML = "";
                document.getElementById("vmAttrVal").innerHTML = "";
                $("#AttributeName").css("border-color", "#ced4da");
            }
            var clickedrow = $(e.target).closest("tr");
            //var attr_id = clickedrow.children("td:eq(2)").text();
            //var attr_name = clickedrow.children("td:eq(3)").text();
            var attr_id = clickedrow.children("#hdnAttrId").text();
            var attr_name = clickedrow.children("#hdnAttrName").text();

            $("#hdnAction").val("Update_attr");
            $("#AttributeName").val(attr_name);
            $("#hdnAttributeID").val(attr_id);

            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                $("#SaveBtnEnable").css("display", "block");
                $("#AttributeName").attr("disabled", false)
            }
            //flag = 'Y';
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    //if (flag == "Y") {
        
    //}
}
function EditAttrValName() {
    debugger;
    //var flag = 'N';
    $(".editAttrValName").bind("click", function (e) {
        try {
            document.getElementById("vmAttrVal").innerHTML = "";
            $("#AttrVal").css("border-color", "#ced4da");
            debugger;
            var clickedrow = $(e.target).closest("tr");
            //var attrval_id = clickedrow.children("td:eq(2)").text();
            //var attr_id = clickedrow.children("td:eq(3)").text();
            //var attr_valName = clickedrow.children("td:eq(5)").text();
            var attrval_id = clickedrow.children("#hdnAttrvalId").text();
            var attr_id = clickedrow.children("#hdnAttrid").text();
            var attr_valName = clickedrow.children("#hdnAttrValname").text();

            $("#hdnActionattrval").val("Update_attr_val");
            $("#hdnAttrValId").val(attrval_id);
            $("#L_attrName").val(attr_id).trigger("change");
            $("#AttrVal").val(attr_valName);

            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                $("#AttrValueSaveBtnEnable").css("display", "block");
                $("#L_attrName").attr("disabled", false)
                $("#AttrVal").attr("disabled", false)
            }
            //flag = "Y";
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    //if (flag == "Y") {
       
    //}
}