$(document).ready(function () {
   // $('#ddlSuppliersList').select2();
    $('#ddlItemsList').select2();
    Cmn_initializeMultiselect(['#ddlSuppliersList']);
});

function BindItemWiseSummary() {
    debugger;
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    //var suppId = $("#ddlSuppliersList").val();
    var suppId = cmn_getddldataasstring("#ddlSuppliersList");
    var itemId = $("#ddlItemsList").val();
    try {
        $("#datatable-buttons2")[0].id = "dttbl2";
        $("#datatable-buttons3")[0].id = "dttbl3";
    }
    catch { }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISDeliveryNoteDetail/GetDNMisItemWiseSummary",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId
        },
        success: function (data) {
            debugger;

            $("#DeliveryNoteSummaryItemWise").html(data);
            try {
                $("#dttbl2")[0].id = "datatable-buttons2";
                $("#dttbl3")[0].id = "datatable-buttons3";
            }
            catch { }

        }
    })
}
function BindDNMISDataByDetails() {
    debugger;
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var suppId = $("#ddlSuppliersList").val();
    var itemId = $("#ddlItemsList").val();
    try {
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons2")[0].id = "dttbl2";
    }
    catch { }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISDeliveryNoteDetail/GetDNMISByDetails",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId
        },
        success: function (data) {
            debugger;
            $("#MIS_DeliveryNoteDetail").html(data);
            try {
                $("#dttbl1")[0].id = "datatable-buttons1";
                $("#dttbl2")[0].id = "datatable-buttons2";
            }
            catch { }
        }
    })
}
function BindMISDNDetailReport() {
    //var showAs = $('#ShowAs').val();
    //if (showAs == "S") {
        BindItemWiseSummary();
    //}
    //else {
    //    BindDNMISDataByDetails();
    //}
}
function BindMRSSummary(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var suppId = $("#ddlSuppliersList").val();
    var itemId = currentrow.find("#hdnItemId").text();
    var itemName = currentrow.find("#hdnItemName").text();
    var uomName = currentrow.find("#hdnUomName").text();
    var itemQty = currentrow.find("#hdnItemQty").text();
    try {
        $("#datatable-buttons1")[0].id = "dttbl1";
        $("#datatable-buttons3")[0].id = "dttbl3";
    }
    catch { }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISDeliveryNoteDetail/GetGRNMisSummary",
        data: {
            fromDate: fromDate, toDate: toDate, suppId: suppId, itemId: itemId
        },
        success: function (data) {
            debugger;
            $("#GoodsReceiptSummary").html(data);
            //txtItemName,txtUomId,txtShippedQty
            $('#itemName').val(itemName);
            $('#uomName').val(uomName);
            $('#itemQty').val(itemQty);
            try {
                $("#dttbl1")[0].id = "datatable-buttons1";
                $("#dttbl3")[0].id = "datatable-buttons3";
            }
            catch { }
        }
    })
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var ProductId = clickdRow.find("#td_ItemId").text();
    var ProductNm = clickdRow.find("#td_ItemName").text(); 
    var UOM = clickdRow.find("#td_UomName").text();
    var DnNo = clickdRow.find("#td_DnNo").text();
    var DnDate = clickdRow.find("#td_DnDt").text().split("-").reverse().join("-");
   /*var DnDate = clickdRow.find("#td_DnDate").text();*/
    var ItemQty = 0;
    /* -----WAREHOUSE DATA-----------*/
    if (flag == "MIS_DNRecvQty") {
        ItemQty = clickdRow.find("#MISDNRecvQty").text();
    }
    else if (flag == "MIS_DNBillQty") {
        ItemQty = clickdRow.find("#MISDNBillQty").text();
    }
    else if (flag == "MIS_DNOrdQty") {
        ItemQty = clickdRow.find("#MISDNOrdQty").text();
    }
    else if (flag == "MIS_DNPendQty") {
        ItemQty = clickdRow.find("#MISDNPendQty").text();
    }
    ItemQty = ItemQty.trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISDeliveryNoteDetail/GetSubItemDetails",
        data: {
            DnNo: DnNo,
            DnDate: DnDate,
            Item_id: ProductId
          },
        success: function (data) {
            debugger;
            $("#MISDN_SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_UOM").val(UOM);
            $("#Sub_Quantity").val(ItemQty);
        }
    });

}
//function ShowAsChange() {
//    const ShowAs = $("#ShowAs").val();
//    if (ShowAs == "S") {
//        BindItemWiseSummary();
//    }
//    else {
//        BindDNMISDataByDetails();
//    }
//}