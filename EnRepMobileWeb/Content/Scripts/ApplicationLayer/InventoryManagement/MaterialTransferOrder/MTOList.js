/************************************************
Javascript Name:MRS List
Created By:Mukesh Sharma
Created Date: 12-06-2021
Description: This Javascript use for the MRS List Page

Modified By:
Modified Date:
Description:

*************************************************/
$(document).ready(function () {
    $("#toWh").select2();
    var Doc_no = $("#TRFNumber").val();
    $("#hdDoc_No").val(Doc_no);
    debugger;
    $("#MTOList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_status = $('#WF_status').val();
            var clickedrow = $(e.target).closest("tr");
            var TRFNo = clickedrow.children("#TRFNo").text();
            var TRFDt = clickedrow.children("#trf_date").text();
            if (TRFNo != null && TRFNo != "") {
                window.location.href = "/ApplicationLayer/MaterialTransferOrder/EditMTO/?TRFNo=" + TRFNo + "&TRFDt=" + TRFDt + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }

        }
        catch (err) {
            debugger
            //alert(err.message);
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var TRFNo = clickedrow.children("#TRFNo").text();
        var TRFDate = clickedrow.children("#trf_date").text();
        var TRFDT = TRFDate;
        var Date = TRFDT.split("-");
        var TRFDATE = Date[2] + '-' + Date[1] + '-' + Date[0];
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(TRFNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(TRFNo, TRFDate, Doc_id, Doc_Status);
    });
});
   
function BtnSearch() {
    debugger;
    FilterMTOList();
}
function FilterMTOList() {
    debugger;
    try {
        var TransferType = $("#TransferType").val();
        $('#WF_status').val(null);
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
            url: "/ApplicationLayer/MaterialTransferOrder/SearchMTODetail",
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
                $('#MTOList').html(data);
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
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var TRFNo = clickedrow.children("#TRFNo").text();
    var TRFDate = clickedrow.children("#trf_date").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(TRFNo);
    GetWorkFlowDetails(TRFNo, TRFDate, Doc_id);
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
