/*modifyed by shubham maurya on 11-10-2022 18:00 convert js to model*/
/************************************************
Javascript Name:Good Receipt Note List
Created By:Prem
Created Date: 15-04-2021
Description: This Javascript use for the Local Sale Order List many function

Modified By:
Modified Date:
Description:

*************************************************/
$(document).ready(function () {
    debugger;
    $('#SupplierName').select2();
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var _GRN_No = clickedrow.children("#GRNNo").text();
        var _GRN_Date = clickedrow.children("#MrDate").text();
        if (_GRN_No != null && _GRN_Date != "") {
            window.location.href = "/ApplicationLayer/GRNDetail/EditGRN/?grn_no=" + _GRN_No + "&grn_dt=" + _GRN_Date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            //sessionStorage.removeItem("EditGRNNo");
            //sessionStorage.setItem("EditGRNNo", _GRN_No);
            //sessionStorage.removeItem("EditGRNDate");
            //sessionStorage.setItem("EditGRNDate", _GRN_Date);
            //GRNDetail();
        }

    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var GRNNo = clickedrow.children("#GRNNo").text();
        var GRN_Date = clickedrow.children("#MrDate").text();
        var Doc_id = $("#DocumentMenuId").val();

        $("#hdDoc_No").val(GRNNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(GRNNo, GRN_Date, Doc_id, Doc_Status);
    });

    ListRowHighLight();
    //BindSupplierList();
});
function ResetWF_Level() {
    var li_count = $("#wizard ul li").length;
    if (li_count > 0) {
        for (var y = 0; y < li_count; y++) {
            var id = parseInt(y) + 1;
            $("#a_" + id).removeClass("done");
            $("#a_" + id).removeClass("selected");
            $("#a_" + id).addClass("disabled");
        }
    }
}
function BtnSearch() {
    debugger;
    FilterGRNList();
    ResetWF_Level();
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
function GRNDetail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/GRNDetail/GRNDetail";
    } catch (err) {
        console.log("GRN Error : " + err.message);
    }
}
function FilterGRNList() {
    debugger;
    try {
        var SuppId = $("#SupplierName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GRNDetail/SearchGRNDetail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyGRNList').html(data);
                $('#ListFilterData').val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}