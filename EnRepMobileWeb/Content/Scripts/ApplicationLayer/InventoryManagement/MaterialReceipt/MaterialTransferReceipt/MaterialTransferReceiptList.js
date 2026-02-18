/************************************************
Javascript Name:TMR List
Created By:Mukesh Sharma
Created Date: 18-08-2021
Description: This Javascript use for the Material Transfer Receipt List Page

Modified By:
Modified Date:
Description:

*************************************************/
$(document).ready(function () {
    debugger;
    $("#fromWh").select2();
    var Doc_no = $("#txtMaterialReceiptNo").val();
    $("#hdDoc_No").val(Doc_no);

            $("#datatable-buttons >tbody").bind("click", function (e) {
            debugger;
            var clickedrow = $(e.target).closest("tr");
            var TMRNO = clickedrow.children("#MRNo").text();
            var TMR_Date = clickedrow.children("#MR_Dt").text();
            var Doc_id = $("#DocumentMenuId").val();
                $("#hdDoc_No").val(TMRNO);
                var Doc_Status = clickedrow.children("#Doc_Status").text();
                Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
                GetWorkFlowDetails(TMRNO, TMR_Date, Doc_id, Doc_Status);

            });
            $("#TMRList #datatable-buttons tbody").bind("dblclick", function (e) {
            debugger;
                try {
                    var ListFilterData = $("#ListFilterData").val();
                    var WF_status = $("#WF_status").val();
                    var clickedrow = $(e.target).closest("tr");      
                var TMRNO = clickedrow.children("#MRNo").text();
                var TMR_Type = clickedrow.children("#Trf_Type").text();
                var TMR_Date = clickedrow.children("#MR_Dt").text();
                if (TMRNO != null && TMRNO != "") {
                    window.location.href = "/ApplicationLayer/MaterialTransferReceipt/EditTMR/?TMRNO=" + TMRNO + "&TMR_Date=" + TMR_Date + "&TMR_Type=" + TMR_Type + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
                }                
            }
            catch (err) {
            }
        });
    //$("#DeliveryNoteListTbBody #datatable-buttons tbody").bind("dblclick", function (e) {
    //    debugger;
    //    try {
    //        var clickedrow = $(e.target).closest("tr");
    //        var DeliveryNoteNo = clickedrow.children("td:eq(1)").text();
    //        if (DeliveryNoteNo != null && DeliveryNoteNo != "") {
    //            window.location.href = "/ApplicationLayer/DeliveryNoteList/EditDeliveryNote/?DeliveryNoteNo=" + DeliveryNoteNo;
    //        }

    //    }
    //    catch (err) {
    //    }
    //});
    ListRowHighLight();
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    var DeliveryNoteNo = clickedrow.children("td:eq(1)").text();
    //    var DeliveryNoteDate = clickedrow.children("td:eq(3)").text();
    //    var Doc_id = $("#DocumentMenuId").val();
    //    $("#hdDoc_No").val(DeliveryNoteNo);
    //    GetWorkFlowDetails(DeliveryNoteNo, DeliveryNoteDate, Doc_id);
    //});
    //var now = new Date();
    //var month = (now.getMonth() + 1);
    //var day = now.getDate();
    //if (month < 10)
    //    month = "0" + month;
    //if (day < 10)
    //    day = "0" + day;
    //var today = now.getFullYear() + '-' + month + '-' + day;
    //$("#txtTodate").val(today);
});


function BtnSearch() {
    debugger;
    FilterTMRList();
    ResetWF_Level();
}

function FilterTMRList() {
    debugger;
    try {
        var from_br = $("#Fr_branch").val();
        var from_wh = $("#fromWh").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (from_br == "" || from_br == null) {
            from_br = "0";
        }
        if (from_wh == "" || from_wh == null) {
            from_wh = "0";
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialTransferReceipt/SearchTMRDetail",
            data: {
         
                from_br: from_br,
                from_wh: from_wh,
                Fromdate: Fromdate,
                Todate: Todate,               
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#TMRList').html(data);
                $('#ListFilterData').val(from_br + ',' + from_wh + ',' + Fromdate + ',' + Todate + ',' + Status);
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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}

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
