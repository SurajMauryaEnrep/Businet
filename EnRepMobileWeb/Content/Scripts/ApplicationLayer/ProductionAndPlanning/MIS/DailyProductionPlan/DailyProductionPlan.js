$(document).ready(function () {
    debugger;
    
    cmn_apply_datatable("#datatable-buttons-new", DailyProductionPlanCSV);/*add by Hina Sharma on 24-09-2025*/
    $("#ddlOpName").select2({
    });
    //BindItemList("#ProductNameList", "", "", "", "", "PPL");
    DynamicSerchableItemDDL("", "#DDLProductNameList", "", "", "", "DPP");
});
function OnClickDPP_PlannedQty(e, planDt) {
    debugger;
    var ClickedRow = $(e.target).closest('tr');
    var ProductId = ClickedRow.find("#tdProductId").val();
    var ProductName = ClickedRow.find("#tdProductName").val();
    var ProductUOM = ClickedRow.find("#UOM").text();
    var planDt = $("#" + planDt).text();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DailyProductionPlan/GetDPP_PlannedQtyDetail",
        data: { ProductId: ProductId, PlanDt: planDt},
        success: function (data) {
            $("#PopUpDPlannedDetail").html(data);

            $("#PQ_ProductName").val(ProductName);
            $("#PQ_UOM").val(ProductUOM);
            $("#PQ_Date").val(planDt);

        }
    })
}
function OnClickDPP_ProducedQty(e, producedDt) {
    debugger
    var ClickedRow = $(e.target).closest('tr');
    var ProductId = ClickedRow.find("#tdProductId").val();
    var ProductName = ClickedRow.find("#tdProductName").val();
    var ProductUOM = ClickedRow.find("#UOM").text();
    var producedDt = $("#" + producedDt).text();
    

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DailyProductionPlan/GetDPP_ProducedQtyDetail",
        data: { ProductId: ProductId, ProducedDt: producedDt },
        success: function (data) {
            $("#PopUpDProductionDetail").html(data);

            $("#PrdQ_ProductName").val(ProductName);
            $("#PrdQ_UOM").val(ProductUOM);
            $("#PrdQ_Date").val(producedDt);
        }
    })
}
function FilterDailyProductionPlan() {
    debugger
    var ProductId = $("#DDLProductNameList").val();
    var DR_fromDt = $("#DR_fromDt").val();
    var DR_ToDt = $("#DR_ToDt").val();
    var ddlOpId = $("#ddlOpName").val();
    var startDate = new Date(DR_fromDt);
    var endDate = new Date(DR_ToDt);

    var DateRange = (endDate - startDate) / (1000 * 60 * 60 * 24); // difference in days/*add by Hina Sharma on 24-09-2025*/
    var filters = ProductId + "," + DR_fromDt + "," + DR_ToDt + "," + ddlOpId + "," + DateRange;
    $("#hdnFiltersData").val(filters);/*add by Hina Sharma on 24-09-2025*/
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DailyProductionPlan/GetFilteredDailyProductionPlan",
        data: { ProductId: ProductId, DR_fromDt: DR_fromDt, DR_ToDt: DR_ToDt, ddlOpId: ddlOpId },
        success: function (data) {
            
            $("#DailyProductionPlanData").html(data);
            $("a.btn.btn-default.buttons-print.btn-sm").remove();/*add by Hina Sharma on 24-09-2025*/
            $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();/*add by Hina Sharma on 24-09-2025*/
            //$(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm" onclick=""  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>')
            //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="DailyProductionPlanCSV()" tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        }
    })
}
function DailyProductionPlanCSV() {/*add by Hina Sharma on 24-09-2025*/
    debugger;
    
    var ProductId = $("#DDLProductNameList").val();
    var DR_fromDt = $("#DR_fromDt").val();
    var DR_ToDt = $("#DR_ToDt").val();
    var ddlOpId = $("#ddlOpName").val();
    var startDate = new Date(DR_fromDt);
    var endDate = new Date(DR_ToDt);

    var DateRange = (endDate - startDate) / (1000 * 60 * 60 * 24); // difference in days
    var filters = ProductId + "," + DR_fromDt + "," + DR_ToDt + "," + ddlOpId + "," + DateRange;
    $("#hdnFiltersData").val(filters);
    $("#hdnDPPCSVPrint").val("CSV");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#DPPsearchValue").val(searchValue);
    $('form').submit();
    
}


