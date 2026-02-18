$(document).ready(function () {
    $("#ddlPayeeGlListpage").select2();
    $("#ExpVouList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            /*var WF_Status = $("#WF_Status").val();*/
            var clickedrow = $(e.target).closest("tr");
            var VouId = clickedrow.children("#Vou_No").text();
            var VouDate = clickedrow.children("#hdnvou_date").text();
            $("#hdDoc_No").val(VouId);
            if (VouId != null && VouDate != "") {
                window.location.href = "/ApplicationLayer/ExpenseVoucher/DblClickExVouList/?Vou_No=" + VouId + "&Vou_Dt=" + VouDate + "&ListFilterData=" + ListFilterData;
                 /*   + "&WF_Status=" + WF_Status;*/
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var EV_No = clickedrow.children("#Vou_No").text();
        var EV_Dt = clickedrow.children("#hdnvou_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(EV_No, EV_Dt, Doc_id, Doc_Status);
        $("#hdDoc_No").val(EV_No);
    });
    //$(document).ready(function () {
    //    debugger;
    //    /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
    //    $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="ExpenseVoucherCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
})
function ExpenseVoucherCSV() {
    var Acc_Id = $("#ddlPayeeGlListpage option:selected").val();
    var FromDate = $("#From_Date").val();
    var Todate = $("#To_Date").val();
    var Status = $("#ddlStatusList option:selected").val();
    window.location.href = "/ApplicationLayer/ExpenseVoucher/ExpenseVoucherExporttoExcelDt?Acc_Id=" + Acc_Id + "&FromDate=" + FromDate + "&Todate=" + Todate + "&Status=" + Status;
}
function OnclickSearchBtn() {
    debugger;

    var Acc_Id = $("#ddlPayeeGlListpage option:selected").val();
    var FromDate = $("#From_Date").val();
    var Todate = $("#To_Date").val();
    var Status = $("#ddlStatusList option:selected").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ExpenseVoucher/ExpVouSearchedList",
        data: {
            Acc_Id: Acc_Id, Fromdate: FromDate, Todate: Todate, Status: Status
        },
        success: function (data) {
            debugger;
            $('#ExpVouList').html(data);
           /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
            //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="ExpenseVoucherCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            $('#ListFilterData').val(FromDate + ',' + Todate + ',' + Status + ',' + Acc_Id);
        }
    })
}

