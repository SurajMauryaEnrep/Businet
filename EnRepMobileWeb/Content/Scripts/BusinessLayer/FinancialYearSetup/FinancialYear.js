$(document).ready(function () {
    debugger;
   
    $("#datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var pfysdt = clickedrow.find("#pfy_sdt").val();
            var pfyedt = clickedrow.find("#pfy_edt").val();
            var nfysdt = clickedrow.find("#nfy_sdt").val();
            var nfyedt = clickedrow.find("#nfy_edt").val();
            var bkclose = clickedrow.find("#bk_close").val();

            if (nfysdt != "" && nfysdt != null) {
                window.location.href = "/BusinessLayer/FinancialYearSetup/EditFY_Closing/?pfy_sdt=" + pfysdt + "&pfy_edt=" + pfyedt + "&nfy_sdt=" + nfysdt + "&nfy_edt=" + nfyedt + "&bk_close=" + bkclose;
            }
        }
        catch (err) {}
    });
});
function OnChange_fybr_ddl() {
    var br_id = $("#ddl_branch").val();

    if (br_id == "0" || br_id == "") {
        $("#ddl_branch").attr("style", "border-color: #ff0000;");
        $("#span_br").text($("#valueReq").text());
        $("#span_br").css("display", "block");
    }
    else {
        $("#span_br").text("");
        $("#ddl_branch").css("border-color", "#ced4da");
    }

}
function functionConfirm(event) {
    debugger;
    var br_id = $("#ddl_branch").val();

    if (br_id == "0" || br_id == "") {
        $("#ddl_branch").attr("style", "border-color: #ff0000;");
        $("#span_br").text($("#valueReq").text());
        $("#span_br").css("display", "block");
    }
    else {
        swal({
            title: $("#deltital").text(),
            /*text: $("#deltext").text() + "!",*/
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-primary",
            confirmButtonText: "Yes, Continue!",
            closeOnConfirm: false
        }, function (isConfirm) {
            debugger;
            if (isConfirm) {
                debugger;
                $("#hdn_save_comand").val("Save");
                $("#hdn_br_id").val(br_id);
                $('form').submit();

                return true;
            } else {
                return false;
            }
        });
    }
    return false;
}