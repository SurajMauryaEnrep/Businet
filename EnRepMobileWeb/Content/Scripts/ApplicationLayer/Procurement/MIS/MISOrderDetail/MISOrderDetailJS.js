$(document).ready(function () {
    //$('#ddlSuppliersList').select2();
    //$('#ddlItemsList').select2();
    DynamicSerchableItemDDL("", "#ddlItemsList", "", "", "", "MISOrderDeatil")
    //$('#ddlStatus').select2();
    //$('#ddlCurrencylist').select2();
    //$(document).ready(function () {
    //    $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
    //    $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderDetailWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});

    Cmn_initializeMultiselect([
        '#ddlSuppliersList',
        '#ddlCurrencylist',
        '#ddlSrcType',
        '#ddlOrderType',
        '#SupplierCategory',
        '#SupplierPortfolio',
        '#ddlStatus',
        '#ddl_branch',
    ]);
});
function OnchangeItemName() {
    debugger;
    var ItemID = "";
    $("#SpanSerchItemID").css("display", "none");
    $("#itemNameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-itemNameList-container']").css("border-color", "#ced4da");
    ItemID = $("#ddlItemsList").val();

    $("#hf_ItemID").val(ItemID);



}
// async function BindMISDetail() {
//    var ShowAs="S";
//    if ($("#ProcurementMISOrderSummary").is(":checked")) {
//        $("#divitemlist").css("display", "none");
//        ShowAs = "S";
//    }
//    if ($("#ProcurementMISOrderDetail").is(":checked")) {

//        $("#divitemlist").css("display","block");
//        ShowAs = "D";
//    }
//    var fromDate = $("#txtFromdate").val();
//    var toDate = $("#txtTodate").val();
//    var suppId = $('#ddlSuppliersList').val();
//    var itemId = $("#ddlItemsList").val();
//    var currId = $('#ddlCurrencylist').val();
//    var srctype = $('#ddlSrcType').val();
//    var orderType = $('#ddlOrderType').val();
//    var Status = $('#ddlStatus').val();
//    $("#datatable-buttons1")[0].id = "dttbl1";
//     await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderSummary",
//        data: {
//            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, currId: currId, srctype: srctype,
//            orderType: orderType, Status: Status, ShowAs: ShowAs
//        },
//        success: function (data) {
//            debugger;
//            $("#MISOrderSummary").html(data);
//            var element = $("#dttbl1")[0];
//            element.id = "datatable-buttons1";
//          /*  $("#dttbl1")[0].id = "datatable-buttons1";*/
//            $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
//            $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderDetailWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
//            //$("#dttbl2")[0].id = "datatable-buttons2";
//        }
//    });
//}
async function BindMISDetail() {
    var ShowAs = "S";

    // Cache the selectors
    var $divItemList = $("#divitemlist");
    var $procurementMISOrderSummary = $("#ProcurementMISOrderSummary");
    var $procurementMISOrderDetail = $("#ProcurementMISOrderDetail");
    var $datatableButtons = $("#datatable-buttons1");

    // Update the display based on the checked status
    if ($procurementMISOrderSummary.is(":checked")) {
        $divItemList.hide();  // Better than using css("display", "none")
        ShowAs = "S";
    }
    if ($procurementMISOrderDetail.is(":checked")) {
        $divItemList.show();  // Better than using css("display", "block")
        ShowAs = "D";
    }

    // Get input values
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    //var suppId = $('#ddlSuppliersList').val();
    //var itemId = $("#ddlItemsList").val();
    //var currId = $('#ddlCurrencylist').val();
    //var srctype = $('#ddlSrcType').val();
    //var orderType = $('#ddlOrderType').val();
    //var Status = $('#ddlStatus').val();
    var suppId = $("#ddlSuppliersList option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var itemId = $("#ddlItemsList option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var currId = $("#ddlCurrencylist option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var srctype = $("#ddlSrcType option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var orderType = $("#ddlOrderType option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var Status = $("#ddlStatus option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var custCat = cmn_getddldataasstring("#SupplierCategory");
    var custPort = cmn_getddldataasstring("#SupplierPortfolio");
    var brid_list = cmn_getddldataasstring("#ddl_branch");


    // Update the ID only once, at the appropriate point.
    $datatableButtons[0].id = "dttbl1";

    // Perform AJAX request
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderSummary",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, currId: currId, srctype: srctype,
            orderType: orderType, Status: Status, ShowAs: ShowAs, custCat: custCat, custPort: custPort, brid_list: brid_list
        },
        success: function (data) {
            $("#MISOrderSummary").html(data);

            // Update the ID once inside success, if needed
            var $datatable = $("#dttbl1");
            $datatable[0].id = "datatable-buttons1";  // Reset the ID after data is loaded

            // Append the CSV button
           /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
            //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderDetailWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>');
        }
    });
}
function GetOrderDetailWiseCSV() {
    var ShowAs = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ShowAs = "D";
    }
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var suppId = $('#ddlSuppliersList').val();
    var itemId = $("#ddlItemsList").val();
    var currId = $('#ddlCurrencylist').val();
    var srctype = $('#ddlSrcType').val();
    var orderType = $('#ddlOrderType').val();
    var Status = $('#ddlStatus').val();
    window.location.href = "/ApplicationLayer/MISOrderDetail/OrderDetailExporttoExcelDt?ShowAs=" + ShowAs + "&fromDate=" + fromDate + "&toDate=" + toDate + "&suppId=" + suppId + "&itemId=" + itemId + "&currId=" + currId + "&srctype=" + srctype + "&orderType=" + orderType + "&Status=" + Status;
}
//function BindMisOrderWiseSummary() {
//    debugger;
//    var fromDate = $("#txtFromdate").val();
//    var toDate = $("#txtTodate").val();
//    var suppId = $('#ddlSuppliersList').val();
//    var itemId = $("#ddlItemsList").val();
//    var currId = $('#ddlCurrencylist').val();
//    var srctype = $('#ddlSrcType').val();
//    var orderType = $('#ddlOrderType').val();
//    var Status = $('#ddlStatus').val();
//    $("#datatable-buttons1")[0].id = "dttbl1";
//    $("#datatable-buttons2")[0].id = "dttbl2";
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderSummary",
//        data: {
//            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, currId: currId, srctype: srctype,
//            orderType: orderType, Status: Status
//        },
//        success: function (data) {
//            debugger;
//            $("#MISOrderSummary").html(data);
//            $("#dttbl1")[0].id = "datatable-buttons1";
//            $("#dttbl2")[0].id = "datatable-buttons2";
//        }
//    });
//}
function OnclickDeliverySchedule(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");

    var poNo = $("#OrderNumber").val();
    var poDate = $("#hdnPoDt").val();
    var item_id = currentrow.find("#hdnitemid").val();
    var item = currentrow.find("#hdnitemid").text();
    var OrderType = $("#OrderType").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderdeliveryShudule",
        data: {
            orderNo: poNo, item_id: item, OrderType: OrderType
        },
        success: function (data) {
            debugger;
            $("#ddldeliverysch").html(data);
        }
    });
}

function BindMisOrderSummary(e) {
    var currentrow = $(e.target).closest("tr");
    var poNo = currentrow.find("#hdnPoNo").text();
    var poDate = currentrow.find("#hdnPoDt").text();

    var OrderDate = currentrow.find("#hdnOrderDate").text();
    var OrderType = currentrow.find("#hdnOrderType").text();
    var Currency = currentrow.find("#hdnCurrLogo").text();
    var SupplierName = currentrow.find("#hdnSuppName").text();

    var custCat = cmn_getddldataasstring("#SupplierCategory");
    var custPort = cmn_getddldataasstring("#SupplierPortfolio"); 
    var brid_list = currentrow.find("#hdn_brid").text();
    $("#datatable-buttons")[0].id = "dttbl";
    //$("#datatable-buttons2")[0].id = "dttbl2";

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderSummaryDetails",
        data: {
            poNo: poNo, poDate: poDate, custCat: custCat, custPort: custPort, brid_list: brid_list
        },
        success: function (data) {
            debugger;
            $("#MISOrderItemSummary").html(data);
            $('#OrderNumber').val(poNo);
            $('#OrderDate').val(OrderDate);
            $('#OrderType').val(OrderType);
            $('#Currency').val(Currency);
            $('#SupplierName').val(SupplierName);
            $('#OrderDateOrderWise').val(poDate);//add by shubham maurya on 14-01-2025

            $("#dttbl")[0].id = "datatable-buttons";

            //$("#datatable-buttons1_wrapper a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
            //$("#datatable-buttons1_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvPrintSO" onclick="GetOrderWiseCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            //$("#dttbl2")[0].id = "datatable-buttons2";
        }
    });
}
function GetOrderWiseCSV() {
    var ShowAs = "SD";
    var fromDate = "";
    var toDate = "";
    var suppId = "";
    var itemId = "";
    var currId = "";
    var srctype = "";
    var orderType = "";
    var Status = "";
    var poNo = $('#OrderNumber').val();
    var poDate = $('#OrderDateOrderWise').val();
    window.location.href = "/ApplicationLayer/MISOrderDetail/OrderDetailExporttoExcelDt?ShowAs=" + ShowAs + "&fromDate=" + fromDate + "&toDate=" + toDate + "&suppId=" + suppId + "&itemId=" + itemId + "&currId=" + currId + "&srctype=" + srctype + "&orderType=" + orderType + "&Status=" + Status + "&poNo=" + poNo + "&poDate=" + poDate;
}
//function BindMisOrderDetails() {
//    var fromDate = $("#txtFromdate").val();
//    var toDate = $("#txtTodate").val();
//    var suppId = $('#ddlSuppliersList').val();
//    var itemId = $("#ddlItemsList").val();
//    var currId = $('#ddlCurrencylist').val();
//    var srctype = $('#ddlSrcType').val();
//    var orderType = $('#ddlOrderType').val();
//    var Status = $('#ddlStatus').val();
//    $("#datatable-buttons")[0].id = "dttbl";
//    $("#datatable-buttons1")[0].id = "dttbl1";
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MISOrderDetail/GetMisOrderDetails",
//        data: {
//            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId, currId: currId, srctype: srctype,
//            orderType: orderType, Status: Status
//        },
//        success: function (data) {
//            debugger;
//            $("#MISOrderDetail").html(data);
//            $("#dttbl")[0].id = "datatable-buttons";
//            $("#dttbl1")[0].id = "datatable-buttons1";
//        }
//    });
//}
function BindOrderMisReport() {
    //const ShowAs = $("#ShowAs").val();
    //debugger
    //var ShowAs = "";//$("#ShowAs").val();
    //if ($("#ProcurementMISOrderSummary").is(":checked")) {
    //    ShowAs = "S";
    //}
    //if ($("#ProcurementMISOrderDetail").is(":checked")) {
    //    ShowAs = "D";
    //}
    //if (ShowAs == "S") {
    //    BindMisOrderWiseSummary();
    //}
    //else {
    //    BindMisOrderDetails();
    //}
    BindMISDetail();
}
function onchangeShowAs() {
    debugger;
    BindMISDetail();
    //var ShowAs = "";//$("#ShowAs").val();
    //if ($("#ProcurementMISOrderSummary").is(":checked")) {
    //    ShowAs = "S";
    //}
    //if ($("#ProcurementMISOrderDetail").is(":checked")) {
    //    ShowAs = "D";
    //}
    //debugger
    //BindOrderMisReport();
    //if (ShowAs == "S") {
    //    $("#MISOrderSummary").css("display", "block");
    //    $("#MISOrderDetail").css("display", "none");
    //    $('#divitemlist').css('display', 'none');
    //}
    //else {
    //    $("#MISOrderSummary").css("display", "none");
    //    $("#MISOrderDetail").css("display", "block");
    //    $('#divitemlist').css('display', 'block');
    //}
}
// $(document).ready(function () {
//        onchangeShowAs();
//    });
//    const onchangeShowAs = () => {
//        const ShowAs = "";//$("#ShowAs").val();
//        if ($("#ProcurementMISOrderSummary").is(":checked")) {
//            ShowAs = "S";
//        }
//        if ($("#ProcurementMISOrderDetail").is(":checked")) {
//            ShowAs = "D";
//        }
//        debugger
//        BindOrderMisReport();
//        if (ShowAs == "S") {
//            $("#MISOrderSummary").css("display", "block");
//            $("#MISOrderDetail").css("display", "none");
//            $('#divitemlist').css('display', 'none');
//        }
//        else {
//            $("#MISOrderSummary").css("display", "none");
//            $("#MISOrderDetail").css("display", "block");
//            $('#divitemlist').css('display', 'block');
//        }
//}
