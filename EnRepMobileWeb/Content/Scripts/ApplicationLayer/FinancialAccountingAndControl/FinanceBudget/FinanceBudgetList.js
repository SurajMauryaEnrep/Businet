$(document).ready(function () {
    debugger;
    //$('#FinanceBudgetList').DataTable().destroy();
    $("#datatable-buttons1 > tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var FinYr = clickedrow.find("#FinyearLs").text();
        var RevNo = clickedrow.find("#RevnoLs").text();
        var FB_No = FinYr + '_' + RevNo;
        var CreateDate = clickedrow.find("#CreateDateLs").text();
        let dt = moment(CreateDate, "DD-MMM-YYYY HH:mm");
        let formatted = dt.format("YYYY-MM-DD");

        //var dt = CreateDate.split("-");
        //var newdt = dt[2] + "-" + dt[1] + "-" + dt[0];
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(FB_No);

        var Doc_Status = clickedrow.find("#StatusLs").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "A");
        GetWorkFlowDetails(FB_No, formatted, Doc_id, Doc_Status);
        //GetWorkFlowDetails(FB_No, newdt, Doc_id, Doc_Status);
    });

    $("#PartialFinBudList #datatable-buttons1 tbody").bind("dblclick", function (e) {

        try {
            debugger;
            var WF_Status = $("#WF_Status").val();
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var FB_Yr = clickedrow.children("#FinyearLs").text();
            var FB_Dt1 = clickedrow.children("#CreateDateLs").text();
            let dt = moment(FB_Dt1, "DD-MMM-YYYY HH:mm");
            let FB_Dt = dt.format("YYYY-MM-DD");
            //var FB_Dt = clickedrow.children("#Create_dt_Ls").text();
            var Rev_No = clickedrow.children("#RevnoLs").text();
            var FB_Per = clickedrow.children("#PeriodLs").text();
            if (FB_Yr != null && FB_Yr != "") {
                window.location.href = "/ApplicationLayer/FinanceBudget/EditVou/?FB_Yr=" + FB_Yr + "&FB_Dt=" + FB_Dt + "&Rev_No=" + Rev_No + "&FB_Per=" + FB_Per + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    //cmn_apply_datatable("#FinanceBudgetList");
})

function onclickFinBudSearch() {
    debugger;
    //$('#FinanceBudgetList').DataTable().destroy();
    var FinyearVal = $("#FinBudddlFinyrList option:selected").val();
    var Finyear = $("#FinBudddlFinyrList option:selected").text();
    var Status = $("#FinBudddlStatus option:selected").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/FinanceBudget/FinBudSearchedList",
        data: {
            Finyear: Finyear,
            Status: Status,
        },
        success: function (data) {
            $("#PartialFinBudList").html(data);
            $("#ListFilterData").val(FinyearVal + ',' + Status + ',' + Finyear);
           
        /*    cmn_apply_datatable("#FinanceBudgetList");*/
        }
    })
}



