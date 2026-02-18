$(document).ready(function () {
    $('#ddlItemName').select2();
    $('#ddlItemGroupName').select2();
    $('#ddlShopfloor').select2();
    $('#ddlStatus').select2();
});

function SearchShflTrfDetail() {

    debugger;
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    var TransactionType = $("#TransactionType").val();
    var MaterialType = $('#MaterialType').val();
    var itemId = $('#ddlItemName').val();
    var itemGrpId = $('#ddlItemGroupName').val();
    var shflId = $('#ddlShopfloor').val();
    var status = $('#ddlStatus').val();
    $("#datatable-buttons")[0].id = "dttbl1";
    $("#datatable-buttons10")[0].id = "dttbl10";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorTransferDetail/SearchShfltrfDetails",
        data: {
            trfType: TransactionType, materialType: MaterialType, itemId: itemId, fromDate: txtFromdate, toDate: txtTodate,
            itemGrpId: itemGrpId, shflId: shflId, status: status
        },
        success: function (data) {
            $("#DivShopfloorTrfDetail").html(data);
            try {
                $("#dttbl1")[0].id = "datatable-buttons";
                $("#dttbl10")[0].id = "datatable-buttons10";
            }
            catch { }
            /*cmn_apply_datatable("#tblqcDetails");*/
        }
    })

}
function SubItemDetailsPopUpShopfloorTD(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var ProductNm = clickdRow.find("#item_name").text();
    var ProductId = clickdRow.find("#hdnItemId").text();
    var UOM = clickdRow.find("#uom_alias").text();
    var Doc_no = clickdRow.find("#hdnTrfNo").text();
    var Doc_dt = clickdRow.find("#hdnTrfDt").text();
    var Sub_Quantity = clickdRow.find("#trf_qty").text();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorTransferDetail/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            Flag: flag,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    })
}
function BindItemInfoBtnClick(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdnItemId").text();
    ItemInfoBtnClick(itemId);
}
function BindBatchOrSerialDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdnItemId").text();
    var trfNo = clickedrow.find("#hdnTrfNo").text();
    var trfDt = clickedrow.find("#hdnTrfDt").text();
    

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ShopfloorTransferDetail/GetTrfBatchOrSerialDetails",
        data: {
            trfNo: trfNo, trfDate: trfDt, itemId: itemId
        },
        success: function (data) {
            $("#MISBatchDetail1").html(data);

            /*cmn_apply_datatable("#tblqcDetails");*/
        }
    })
}