$(document).ready(function () {
    $("#CurrencyName").select2();
})
function CheckValidationCurreny() {
    debugger;
    var validate = true;
    if ($("#CurrencyName").val() == "" || $("#CurrencyName").val() == "0") {
        $('[aria-labelledby="select2-CurrencyName-container"]').css("border-color", "red");
        $('#SpanCurrname').text($("#valueReq").text());
        $("#SpanCurrname").css("display", "block");
        validate = false;
    }
    if ($("#Rate").val() == "" || $("#Rate").val() == "0") {
        $("#Rate").css("border-color", "red");
        $("#vmprice").text($("#valueReq").text());
        $("#vmprice").css("display", "block");
        validate = false;
    }
    if (validate == true) {
        debugger
        if ($("#btn_save").val() != "Update") {
            var validateflag = "Y";
            var effDate = $("#txtEffdate").val();
            var currID = $("#CurrencyName").val();
            $("#datatable-buttons1 >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                var effecdate = currentRow.find("#convdate").text();
                var curr_id = currentRow.find("#curr_id").text();
                if (currID == curr_id) {
                    if (effDate == effecdate)  {
                        $("#txtEffdate").css("border-color", "red");
                        document.getElementById("vmeffdate").innerHTML = $("#valueduplicate").text();
                        validateflag = "N";
                    }
                }
            });
            if (validateflag == "N") {
                return false
            }
            else {
                return true;
            }
        }
        else {
            debugger;
            $('#txtEffdate').removeAttr('min')
            $("#CurrencyName").prop("disabled", false);
            $("#txtEffdate").prop("disabled", false);
            $("#Rate").attr("disabled", false)
            return true;
        }
    }
    else {
        return false;
    } 
}
function RqtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        $("#Rate").css("border-color", "#ced4da");
        $("#vmprice").text("");
        return true;
    }
}
function functionConfirm(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var currency = clickedrow.find("#curr_id").text();
    var conv_date = clickedrow.find("#convdate").text();
    $("#currid").val(currency);
    $("#hdnConvdate").val(conv_date);
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
function onchangeeffdate() {
    $("#txtEffdate").css("border-color", "#ced4da");
    $("#vmeffdate").text("");
    debugger;
    var validateflag = "N";
    var effDate = $("#txtEffdate").val();
    var currID = $("#CurrencyName").val();
    $("#datatable-buttons1 >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var effecdate = currentRow.find("#convdate").text();
        var curr_id = currentRow.find("#curr_id").text();
        if (currID == curr_id) {
            if (effDate < effecdate) {
                var now = new Date();
                var month = (now.getMonth() + 1);
                var day = now.getDate();
                if (month < 10)
                    month = "0" + month;
                if (day < 10)
                    day = "0" + day;
                var today = now.getFullYear() + '-' + month + '-' + day;
                $("#txtEffdate").val(today);
            }
            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            if (effDate > today) {
                var now = new Date();
                var month = (now.getMonth() + 1);
                var day = now.getDate();
                if (month < 10)
                    month = "0" + month;
                if (day < 10)
                    day = "0" + day;
                var today = now.getFullYear() + '-' + month + '-' + day;
                $("#txtEffdate").val(today);
            }
           
        }
    });
    /*var CurrentDate = moment().format('YYYY-MM-DD');*/
    if ($("#txtEffdate").val() == "0001-01-01") {
        var now = new Date();
        var month = (now.getMonth() + 1);
        var day = now.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = now.getFullYear() + '-' + month + '-' + day;
        $("#txtEffdate").val(today);
        var now = new Date();
        var month = (now.getMonth() + 1);
        var day = now.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = now.getFullYear() + '-' + month + '-' + day;
        $("#txtEffdate").val(today);
    }
}
function onChangerate() {
    $("#Rate").css("border-color", "#ced4da");
   $("#vmprice").text("");
}
function onChangecurrency() {
    debugger;
    var validateflag = "N";
    var currID = $("#CurrencyName").val();
    if (currID != null) {
        $("#datatable-buttons1 >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var curr_id = currentRow.find("#curr_id").text();
            var effecdate = currentRow.find("#convdate").text();
            if (currID == curr_id) {
                $("#txtEffdate").attr('min', effecdate);
                validateflag = "Y";
            }
        });
        if (validateflag == "N") {
            $('#txtEffdate').removeAttr('min')
        }
        var now = new Date();
        var month = (now.getMonth() + 1);
        var day = now.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = now.getFullYear() + '-' + month + '-' + day;
        $("#txtEffdate").val(today);
        debugger;
        $('[aria-labelledby="select2-CurrencyName-container"]').css("border-color", "#ced4da");
        $("#SpanCurrname").css("display", "none");
        $("#txtEffdate").css("border-color", "#ced4da");
        $("#vmeffdate").text("");
    }
}
function Editcurrdetail(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var cc_id = CurrentRow.find("#curr_id").text();
    var effecdate = CurrentRow.find("#convdate").text();
    var convrate = CurrentRow.find("#Convrate").text();
    $("#CurrencyName").val(cc_id).trigger('change');
    $("#CurrencyName").prop("disabled", true);
    $("#txtEffdate").val(effecdate).attr("disabled", true);
    $("#Rate").val(convrate);
    $("#btn_save").val("Update");
    $("#Rate").css("border-color", "#ced4da");
    $("#vmprice").css("display", "none");
    $("#txtEffdate").css("border-color", "#ced4da");
    $("#vmeffdate").text("");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#Rate").attr("disabled", false);
    }
}