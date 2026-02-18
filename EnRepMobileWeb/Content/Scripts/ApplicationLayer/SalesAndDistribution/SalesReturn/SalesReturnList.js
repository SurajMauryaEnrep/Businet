$(document).ready(function () {
    debugger;
    $("#SRTListTBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var SRTNo = clickedrow.children("#SRTNumber").text();
            var Srt_Date = clickedrow.children("#hdSRTDate").text();
            if (SRTNo != null && SRTNo != "") {
                window.location.href = "/ApplicationLayer/SalesReturn/EditSalesReturn/?SRTNo=" + SRTNo + "&Srt_Date=" + Srt_Date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }

        }
        catch (err) {
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var SRTNo = clickedrow.children("#SRTNumber").text();
        var Srt_Date = clickedrow.children("#hdSRTDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        //$("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        //$(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SRTNo, Srt_Date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(SRTNo);
    });
   //ListRowHighLight();

    //var now = new Date();
    //var month = (now.getMonth() + 1);
    //var day = now.getDate();
    //if (month < 10)
    //    month = "0" + month;
    //if (day < 10)
    //    day = "0" + day;
    //var today = now.getFullYear() + '-' + month + '-' + day;
    //$("#txtTodate").val(today);
    $("#ddlCustomerName").select2({});
    //BindDnSupplierList();

});

function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var SRT_No = clickedrow.children("#SRTNumber").text();
    var SRT_Date = clickedrow.children("#hdSRTDate").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(SRT_No);
    GetWorkFlowDetails(SRT_No, SRT_Date, Doc_id);
    var a = 1;
}
function FilterSalesReturnList() {
    debugger;
    try {
        var CustId = $("#ddlCustomerName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SalesReturn/SearchSalesReturnDetail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#SRTListTBody').html(data);
                $('#ListFilterData').val(CustId + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
//function ResetWF_Level() {
//    var li_count = $("#wizard ul li").length;
//    if (li_count > 0) {
//        for (var y = 0; y < li_count; y++) {
//            var id = parseInt(y) + 1;
//            $("#a_" + id).removeClass("done");
//            $("#a_" + id).removeClass("selected");
//            $("#a_" + id).addClass("disabled");
//        }
//    }
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = clickedrow.children("td:eq(1)").text();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
}