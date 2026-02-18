$(document).ready(function () {
    debugger;
    $("#ddlRequiredAreaList").select2();
    debugger;
    $("#tbodyMRPList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var MRPNumber = clickedrow.children("#mrp_no").text(); 
            var MRPDate = clickedrow.children("#mrp_date").text();
            if (MRPNumber != null && MRPNumber != "") {
                window.location.href = "/ApplicationLayer/MaterialRequirementPlan/EditDomesticMRPDetails/?MRPNumber=" + MRPNumber + "&MRPDate=" + MRPDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var MRPNumber = clickedrow.children("#mrp_no").text();
        var MRPDate = clickedrow.children("#mrp_date").text();
        var date = MRPDate.split("-");
        var FDate = date[2] + '-' + date[1] + '-' + date[0];
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(MRPNumber, FDate, Doc_id, Doc_Status);
        $("#hdDoc_No").val(MRPNumber);
    });
    function CmnGetWorkFlowDetails(e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var MRP_No = clickedrow.children("#mrp_no").text();
        var MRP_Date = clickedrow.children("#mrp_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(MRP_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        if (MRP_Date.split('-')[2].length == 4) {
            MRP_Date = MRP_Date.split('-').reverse().join('-');
        }
        GetWorkFlowDetails(MRP_No, MRP_Date, Doc_id, Doc_Status);
        //var a = 1;
    }
});
function BtnSearch() {
    debugger;
    FilterMRPListData();
    ResetWF_Level();
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

function FilterMRPListData() {
    debugger;
    try {
        var Source = $("#ddl_src_type").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var Req_area = $("#ddlRequiredAreaList").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequirementPlan/MRPListSearch",
            data: {
                Source: Source,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Req_area: Req_area
            },
            success: function (data) {
                debugger;
                $('#tbodyMRPList').html(data);
                $('#ListFilterData').val(Source + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + Req_area);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MRP Error : " + err.message);

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