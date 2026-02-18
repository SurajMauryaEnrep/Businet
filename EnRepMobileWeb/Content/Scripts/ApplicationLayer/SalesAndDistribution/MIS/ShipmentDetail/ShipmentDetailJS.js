$(document).ready(function () {
   // $('#ddlCustId').select2();
  $('#ddlItemId').select2();
    Cmn_initializeMultiselect([
        '#ddlCustId',
         '#ddlShipmentType',
    ]);
});
function BindItemWiseSummary() {
    debugger;
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var shipmentType = $("#ddlShipmentType").val();
    var customerId = $("#ddlCustId").val();
    var ItemId = $("#ddlItemId").val();
   //var ItemId = cmn_getddldataasstring("#ddlItemId");
    $("#datatable-buttons1")[0].id = "dttbl1";
    $("#datatable-buttons2")[0].id = "dttbl2";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISShipmentDetail/GetShipmentItemWiseSummary",
        data: {
            fromDate:fromDate, toDate:toDate, shipmentType:shipmentType, customerId:customerId, ItemId:ItemId
        },
        success: function (data) {
            debugger;
            $("#ShipmentSummary").html(data);
            $("#dttbl1")[0].id = "datatable-buttons1";
            $("#dttbl2")[0].id = "datatable-buttons2";
        }
    })
}
function BindShipmentDetails() {
    debugger;
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    //var shipmentType = $("#ddlShipmentType").val();
    //var customerId = $("#ddlCustId").val();
     var ItemId = $("#ddlItemId").val();
    var shipmentType = cmn_getddldataasstring("#ddlShipmentType");
    var customerId = cmn_getddldataasstring("#ddlCustId");
    //var ItemId = cmn_getddldataasstring("#ddlItemId");
    $("#datatable-buttons1")[0].id = "dttbl1";
    $("#datatable-buttons3")[0].id = "dttbl3";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISShipmentDetail/GetShipmentDetails",
        data: {
            fromDate: fromDate, toDate: toDate, shipmentType: shipmentType, customerId: customerId, ItemId: ItemId
        },
        success: function (data) {
            debugger;
            $("#ShipmentDetail").html(data);
            $("#dttbl1")[0].id = "datatable-buttons1";
            $("#dttbl3")[0].id = "datatable-buttons3";
        }
    })
}
function BindShipmentMisDetails() {
    //var showAs = $('#ShowAs').val();
    let showAs = "";//$("#ShowAs").val();
    if ($("#SalesMISShipmentSummary").is(":checked")) {
        showAs = "S";
    }
    else if ($("#SalesMISShipmentDetail").is(":checked")) {
        showAs = "D";
    }

    if (showAs == "S") {
        BindItemWiseSummary();
    }
    else {
        BindShipmentDetails();
    }
}
function BindShipmentSummary(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var shipmentType = $("#ddlShipmentType").val();
    var customerId = $("#ddlCustId").val();
    var ItemId = currentrow.find("#ItemId").text();
    var itemName = currentrow.find("#ItemName").text();
    var uomName = currentrow.find("#UomName").text();
    var itemQty = currentrow.find("#TotalQty").text();
    var valDigit = $('#hdnValDigit').val();
    $("#datatable-buttons2")[0].id = "dttbl2";
    $("#datatable-buttons3")[0].id = "dttbl3";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISShipmentDetail/GetShipmentSummary",
        data: {
            fromDate: fromDate, toDate: toDate, shipmentType: shipmentType, customerId: customerId, ItemId: ItemId
        },
        success: function (data) {
            debugger;
            $("#MISShipmentSummary").html(data);
            //txtItemName,txtUomId,txtShippedQty
            $('#txtItemName').val(itemName);
            $('#txtUomId').val(uomName);
            $('#txtShippedQty').val(parseFloat(itemQty).toFixed(valDigit));
            $("#dttbl2")[0].id = "datatable-buttons2";
            $("#dttbl3")[0].id = "datatable-buttons3";
        }
    })
}
function onchangeShowAs() {
    let ShowAs = "";//$("#ShowAs").val();
    if ($("#SalesMISShipmentSummary").is(":checked")) {
        ShowAs = "S";
    }
    if ($("#SalesMISShipmentDetail").is(":checked")) {
        ShowAs = "D";
    }
    ShowHideFilters(ShowAs)
    if (ShowAs == "S") {
        BindShipmentMisDetails();
        $("#ShipmentSummary").css("display", "block");
        $("#ShipmentDetail").css("display", "none");
    }
    else {
        BindShipmentMisDetails();
        $("#ShipmentSummary").css("display", "none");
        $("#ShipmentDetail").css("display", "block");
    }
}

function ShowHideFilters(ShowAs) {
    //var ShowAs = $("#ShowAs").val();
    if (ShowAs == "S") {
        $("#divCustomers").css('display', 'none');
        $("#divShipmentType").css('display', 'none');
    }
    else {
        $("#divCustomers").css('display', 'block');
        $("#divShipmentType").css('display', 'block');
    }
}