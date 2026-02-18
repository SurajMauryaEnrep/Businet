/************************************************
Javascript Name:MTI List
Created By:Mukesh Sharma
Created Date: 29-07-2021
Description: This Javascript use for the Material Transfer List Page

Modified By:
Modified Date:
Description:

*************************************************/
$(document).ready(function () {
    debugger;
    $("#toWh").select2();
    $("#MTIList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var MTINO = clickedrow.children("#MTINo").text();
            var MTI_Type = clickedrow.children("#trfType").text();
            var MTI_Date = clickedrow.children("#issue_date").text();
            window.location.href = "/ApplicationLayer/MaterialTransferIssue/EditMTI/?MTINO=" + MTINO + "&MTI_Date=" + MTI_Date + "&MTI_Type=" + MTI_Type + "&ListFilterData=" + ListFilterData;

        }
        catch (err) {
            debugger
            //alert(err.message);
        }
    });
});
function BtnSearch() {
    debugger;
    FilterMTIList();
}

function FilterMTIList() {
    debugger;
    try {
        var TransferType = $("#TransferType").val();
        var tobranch = $("#tobranch").val();
        var toWh = $("#toWh").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (tobranch == "" || tobranch == null) {
            tobranch = "0";
        }
        if (toWh == "" || toWh == null) {
            toWh = "0";
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialTransferIssue/SearchMTIDetail",
            data: {
                toWh: toWh,
                tobranch: tobranch,
                Fromdate: Fromdate,
                Todate: Todate,
                TransferType: TransferType,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#MTIList').html(data);
                $('#ListFilterData').val(toWh + ',' + tobranch + ',' + Fromdate + ',' + Todate + ',' + TransferType + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MTO Error : " + err.message);

    }
}

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
