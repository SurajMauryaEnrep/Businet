$(document).ready(function () {
    debugger;
    $("#hdDoc_No").val($("#SwapNumber").val())

    $("#ddlItemNameList").select2();
    $("#ddlItemNameDestList").select2();
    $("#ddlWarehouse").select2();
    var CurrentDate = moment().format('DD-MM-YYYY');
    if ($("#SwapDate").val() == "" || $("#SwapDate").val() == null) {
        $("#SwapDate").val(CurrentDate);
    }  
    $("#ddlWarehouseDest").select2();
    SwapStkListDoubleClick();
    debugger;
    DynamicSerchableItemDDL("", "#ddlItemName", "", "", "#ddlItemNameDest", "STKSWPSS")
  //  DynamicSerchableItemDDL("", "#ddlItemNameDest", "", "", "#ddlItemName", "STKSWP")

    if ($("#ItemOrderType").is(":checked")) {
        $("#requiredProductName1").removeAttr("style");
        $("#requiredWarehouse1").removeAttr("style");
        $("#requiredSwapQuantity1").removeAttr("style");
    }
    else {
        $("#requiredProductName1").attr("style", "display: none;");
        $("#requiredWarehouse1").attr("style", "display: none;");
        $("#requiredSwapQuantity1").attr("style", "display: none;");
    }
    BatchAndSerail();
});
function FilterLotBatchPartial(e) {


    var searchValue = e.currentTarget.value.trim().toUpperCase();

    $("#BatchWiseItemStockTbl > tbody > tr").each(function () {
        var row = $(this);

        // Get values from input fields (Lot and BatchNumber)
        var lotVal = row.find('input[name="Lot"]').val()?.trim().toUpperCase() || "";
        var batchVal = row.find('input[name="BatchNumber"]').val()?.trim().toUpperCase() || "";
        var ExpiryDt = row.find('input[name="ExpiryDate"]').val()?.trim().toUpperCase() || "";

        // Check if searchValue is contained in Lot or BatchNumber
        if (searchValue === "" || lotVal.includes(searchValue) || batchVal.includes(searchValue) || ExpiryDt.includes(searchValue)) {
            row.show(); // Show matching or all if empty
        } else {
            row.hide(); // Hide non-matching
        }
    });
}
function FilterSerialLotBatchPartial(e) {


    var searchValue = e.currentTarget.value.trim().toUpperCase();

    $("#ItemSerialwiseTbl > tbody > tr").each(function () {
        var row = $(this);

        // Get values from input fields (Lot and BatchNumber)
        var lotVal = row.find('input[name="Lot"]').val()?.trim().toUpperCase() || "";
        var batchVal = row.find('input[name="SerialNumberDetail"]').val()?.trim().toUpperCase() || "";
       

        // Check if searchValue is contained in Lot or BatchNumber
        if (searchValue === "" || lotVal.includes(searchValue) || batchVal.includes(searchValue)) {
            row.show(); // Show matching or all if empty
        } else {
            row.hide(); // Hide non-matching
        }
    });
}
function OnClickOrderType() {
    debugger;
    if ($("#ItemOrderType").is(":checked")) {
        OnClickItemOrderType();
        $("#ddlItemNameDest").val(0).trigger('change');
        $("#ddlWarehouseDest").val(0).trigger('change');
        $("#AvailableQuantityDest").val(null);
        $("#DestSwapQuantity").val(null);
        DynamicSerchableItemDDL("", "#ddlItemName", "", "", "#ddlItemNameDest", "STKSWPSS")
        //DynamicSerchableItemDDL("", "#ddlItemNameDest", "", "", "#ddlItemName", "STKSWP")
    }
    else if ($("#UOMItemOrderType").is(":checked")) {
        OnClickUOMOrderType();
        $("#ddlItemNameDest").val(0).trigger('change');
        $("#ddlWarehouseDest").val(0).trigger('change');
        $("#AvailableQuantityDest").val(null);
        $("#DestSwapQuantity").val(null);
        DynamicSerchableItemDDL("", "#ddlItemName", "", "", "#ddlItemNameDest", "STKSWP")
      //  DynamicSerchableItemDDL("", "#ddlItemNameDest", "", "", "#ddlItemName", "STKSWP");
       // DynamicSerchableItemDDL("", "#ddlItemNameDest", "", "", "#ddlItemName", "STKSWP")
    }
    else {
        OnClickSubItemOrderType();
        DynamicSerchableItemDDL("", "#ddlItemName", "", "", "#ddlItemNameDest", "STKSWPSUBITEM")
    }
}
function OnClickUOMOrderType() {
    OnClickItemOrderType();
    $("#DestSwapQuantity").attr("disabled", false);
}
function OnClickItemOrderType() {
    $("#hdi_batch").val("N")
    $("#hdi_serial").val("N")
    $("#SubItemAvlStkDest").attr("disabled", true)
    $("#DestSubItemReqQty").attr("disabled", true)
    $("#SubItemAvlStk").attr("disabled", true)
    $("#SubItemReqQty").attr("disabled", true)
    $("#DestSwapQuantity").attr("disabled", true);
    removeValidaction();
    $("#ddlItemName").val(0).trigger('change');
    $("#ddlWarehouse").val(0).trigger('change');
    $("#UOMSourc").val(null)//.trigger('change');
    $("#UOMIDSourc").val(null)//.trigger('change');
    $("#ddlItemNameDest").attr("disabled", false);
    //$("#AvailableQuantityDest").attr("disabled", false);
    $("#ddlWarehouseDest").attr("disabled", false);
    $("#SwapQuantity").attr("disabled", false);

    $("#requiredProductName1").removeAttr("style");
    $("#requiredWarehouse1").removeAttr("style");
    $("#requiredSwapQuantity1").removeAttr("style");
}
function OnClickSubItemOrderType() {
    $("#hdi_batch").val("N")
    $("#hdi_serial").val("N")
    $("#SubItemAvlStkDest").attr("disabled", true)
    $("#DestSubItemReqQty").attr("disabled", true)
    $("#SubItemAvlStk").attr("disabled", true)
    $("#SubItemReqQty").attr("disabled", true)
    removeValidaction();
    $("#ddlItemName").val(0).trigger('change');
    $("#ddlItemNameDest").val(0).trigger('change');
    $("#ddlWarehouseDest").val(0).trigger('change');
    $("#ddlWarehouse").val(0).trigger('change');
    $("#UOMDest").val(null);
    $("#UOMIDDest").val(null);
    $("#UOMSourc").val(null);
    $("#UOMIDSourc").val(null);
    $("#SwapQuantity").val(null);
    $("#AvailableQuantityDest").val(null);
    $("#DestSwapQuantity").val(null);
    $("#ddlItemNameDest").attr("disabled", true);
    $("#AvailableQuantityDest").attr("disabled", true);
    $("#ddlWarehouseDest").attr("disabled", true);
    $("#SwapQuantity").attr("disabled", true);
    $("#btnbatchdeatil").attr("disabled", true)
    $("#btnserialdeatil").attr("disabled", true)

    $("#requiredProductName1").attr("style", "display: none;");
    $("#requiredWarehouse1").attr("style", "display: none;");
    $("#requiredSwapQuantity1").attr("style", "display: none;");
}
function SwapStockListSearch() {
    debugger;
    const Src_prod_id = $("#ddlItemNameList").val();
    const Dest_prod_id = $("#ddlItemNameDestList").val();
    const txtFromdate = $("#txtFromdate").val();
    const txtTodate = $("#txtTodate").val();
    const ddlStatus = $("#ddlStatus").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockSwap/SwapStockListSearch",
        data: { Src_prod_id: Src_prod_id, Dest_prod_id: Dest_prod_id, txtFromdate: txtFromdate, txtTodate: txtTodate, status: ddlStatus },
        success: function (data) {
            debugger
            $("#SwapStockBody").html(data);
        }
    })
}
function SwapStkListDoubleClick() {
    debugger;
    $("#SwapStockTbody tr").on("dblclick", function () {
        const CrRow = $(this);
        const swp_no = CrRow.find("#swp_no").text();
        const swp_dt = CrRow.find("#swp_date").text();
        const ListFilterData1 = $("#ListFilterData1").val();
        window.location.href = "/ApplicationLayer/StockSwap/AddStockSwapDetail?swp_no=" + swp_no + "&swp_dt=" + swp_dt + "&ListFilterData1=" + ListFilterData1;
    });
}

function OnchangeProductName(evt,Value) {
    debugger;
    //$("#btnbatchdeatil").css("border-color", "#ced4da");
    //$("#btnserialdeatil").css("border-color", "#ced4da");
    
    if (Value == "Dest") {
        /*------------ Code Added by Suraj on 11-10-2024 --------------*/
        var old_DestProdName = $("#hdn_ddlItemNameDest").val();
        var DestProdName = $("#ddlItemNameDest").val();
        StockSwap_DeleteSubItemQtyDetail(old_DestProdName,"dest");
        $("#hdn_ddlItemNameDest").val(DestProdName);
        /*------------ Code Added by Suraj on 11-10-2024 End--------------*/

        $("#spanProductName1").css("display", "none");
        $("[aria-labelledby='select2-ddlItemNameDest-container']").css("border-color", "#ced4da");
        var ddlItemName = $('#ddlItemNameDest').val();
        if (ddlItemName != "0") {
            Cmn_BindUOM(null, ddlItemName, "Dest", "", "Pur")
            $("#AvailableQuantityDest").val(null);
            $("#ddlWarehouseDest").val(0).trigger("change");
        }
        
    }
    else {
        /*------------ Code Added by Suraj on 11-10-2024 --------------*/
        var old_SrcProdName = $("#hdn_ddlItemName").val();
        var SrcProdName = $("#ddlItemName").val();
        StockSwap_DeleteSubItemQtyDetail(old_SrcProdName,"src");
        $("#hdn_ddlItemName").val(SrcProdName);
        /*------------ Code Added by Suraj on 11-10-2024 End--------------*/

        $("#spanProductName").css("display", "none");
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");
        var ddlItemName = $('#ddlItemName').val();
        if (ddlItemName != "0") {
            Cmn_BindUOM(null, ddlItemName, "Sourc", "", "Pur")
            $("#AvailableStockQuantity").val(null);
            $("#SwapQuantity").val(null);
            $("#ddlWarehouse").val(0).trigger("change");
            debugger;
            $('#ddlItemNameDest').val("0");
            $('#UOMDest').val("");
            $('#UOMIDDest').val("");
            BindDestinationItemList(ddlItemName);
        }
    }
    //$("#SwapQuantity").val(null);
}
function StockSwap_DeleteSubItemQtyDetail(item_id,flag) {//this function is created by Suraj on 11-10-2024 to refresh subitem details
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if (flag == "dest") {
                if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #dest_Item_id[value=" + item_id + "]").length > 0) {
                    $("#hdn_Sub_ItemDetailTbl >tbody >tr td #dest_Item_id[value=" + item_id + "]").closest('tr').each(function () {
                        var row = $(this);
                        var rowItem_id = row.find("#subItemId").val();
                        if (rowItem_id != null) {
                            if (rowItem_id.split('_')[0] == item_id) {
                                row.remove();
                            }
                        }
                    });
                }
            } else {
                if ($("#hdn_Sub_ItemDetailTbl >tbody >tr td #src_Item_id[value=" + item_id + "]").length > 0) {
                    $("#hdn_Sub_ItemDetailTbl >tbody >tr td #src_Item_id[value=" + item_id + "]").closest('tr').each(function () {
                        var row = $(this);
                        var rowItem_id = row.find("#subItemId").val();
                        if (rowItem_id != null) {
                            if (rowItem_id.split('_')[0] == item_id) {
                                row.remove();
                            }
                        }
                    });
                }
            }
            
        }
    }
}
function BatchAndSerail() {
    debugger;
    var batch = $("#hdi_batch").val()
    var serial = $("#hdi_serial").val()
    if (batch == 'Y') {
       $("#btnbatchdeatil").prop("disabled", false);
        $("#btach").addClass("StockSwap");
    }
    else {
       $("#btnbatchdeatil").prop("disabled", true);
       $("#btach").removeClass("StockSwap");
    }
    if (serial == 'Y') {
        $("#btnserialdeatil").prop("disabled", false);
        $("#serial").addClass("StockSwap");
    }
    else {
        $("#btnserialdeatil").prop("disabled", true);
        $("#serial").removeClass("StockSwap");
    }
}
function OnchangeddlWarehouse(Value) {
    debugger;
    if (Value == "Dest") {
        var ddlWarehouse = $('#ddlWarehouseDest').val();
        var ddlItemName = $('#ddlItemNameDest').val();
        $("#DestSubItemReqQty").css("border", "");
        $("#SubItemReqQty").css("border", "");
        if (ddlWarehouse != "0") {
            $("#spanWarehouse1").css("display", "none");
            $("[aria-labelledby='select2-ddlWarehouseDest-container']").css("border-color", "#ced4da");
        }
        else {
            $("#AvailableQuantityDest").val(null)
        }
    }
    else {
        var ddlWarehouse = $('#ddlWarehouse').val();
        var ddlItemName = $('#ddlItemName').val();
        if (ddlWarehouse != "0") {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr td #src_Item_id[value=" + ddlItemName + "]").closest('tr').each(function () {
                debugger
                var Crow = $(this).closest("tr");
                Crow.find("#subItemQty").val("");
            });
            $("#SaveItemBatchTbl tbody tr").remove();
            $("#SwapQuantity").val(null);
            $("#spanWarehouse").css("display", "none");
            $("[aria-labelledby='select2-ddlWarehouse-container']").css("border-color", "#ced4da");
        }
        else {
            $("#AvailableStockQuantity").val(null)
        }
    }
    var Doc_id = $("#DocumentMenuId").val();
    if (ddlWarehouse != "0" && ddlItemName != "0") {
        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ddlItemName,
                WarehouseId: ddlWarehouse,
                UomId: null,
                br_id: null,
                DocumentMenuId: Doc_id
            },
            success: function (data) {
                var QtyDecDigit = $("#QtyDigit").text();///Quantity

                var avaiableStock = JSON.parse(data);
                var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                if (Value == "Dest") {
                    $("#AvailableQuantityDest").val(parseavaiableStock);
                }
                else {
                    $("#AvailableStockQuantity").val(parseavaiableStock);
                }
                if (Value != "Dest") {
                    MTI_BindDestiWHList();
                }           
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    }
}
function MTI_BindDestiWHList() {
    debugger;
    var wh_id = $("#ddlWarehouse").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferOrder/GetSourceAndDestinationList",
        data: { wh_id: wh_id },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            var b = '<option value="0">---Select---</option>';
            for (var i = 0; i < arr.Table.length; i++) {
                b += '<option value=' + arr.Table[i].wh_id + '>' + arr.Table[i].wh_name + '</option>'
            }
            $("#ddlWarehouseDest").html(b);
        },
    });
}
function GetSubItemAvlStock(Value) {
    debugger;
    var status = $("#hdnStatus").val();
    var swp_no = $("#SwapNumber").val();
    var swp_dt = $("#SwapDate").val();
    if (Value == "Dest") {
        var UOM = $('#UOMDest').val();
        var AvailableQuantity = $('#AvailableQuantityDest').val();
        var ddlItemName = $('#ddlItemNameDest option:selected').text();
        var ddlItemId = $('#ddlItemNameDest').val();
        var ddlWarehouse = $('#ddlWarehouseDest').val();
    }
    else {
        var UOM = $('#UOMSourc').val();
        var AvailableQuantity = $('#AvailableStockQuantity').val();
        var ddlItemName = $('#ddlItemName option:selected').text();
        var ddlItemId = $('#ddlItemName').val();
        var ddlWarehouse = $('#ddlWarehouse').val();
    }
    if (status != "A") {
        Cmn_SubItemWareHouseAvlStock(ddlItemName, ddlItemId, UOM, ddlWarehouse, AvailableQuantity, "wh");
    }
    else {
        SubItemWareHouseAvlStock(ddlItemName, ddlItemId, UOM, AvailableQuantity, Value, swp_no, swp_dt);
    }
}
function SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, ItemQty, flag, swp_no, swp_dt) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockSwap/GetSubItemWhAvlstockDetail",
        data: {
            Item_id: ProductId,
            flag: flag,
            swp_no: swp_no,
            swp_dt: swp_dt
        },
        success: function (data) {
            debugger;
            $("#SubItemStockPopUp").html(data);
            $("#Stk_Sub_ProductlName").val(ProductNm);
            $("#Stk_Sub_ProductlId").val(ProductId);
            $("#Stk_Sub_serialUOM").val(UOM);
            $("#Stk_Sub_Quantity").val(ItemQty);
        }

    });
}

function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function ItemStockWareHouseWise(Value) {
    try {
        debugger;
        BindItemBatchDetail();
        if (Value == "Dest") {
            var ItemId = $("#ddlItemNameDest").val();
            var ItemName = $("#ddlItemNameDest option:selected").text();
            var UOMName = $("#UOMDest").val();
        }
        else {
            var ItemId = $("#ddlItemName").val();
            var ItemName = $("#ddlItemName option:selected").text();
            var UOMName = $("#UOMSourc").val();
        }
        $("#WareHouseWiseItemName").val(ItemName);
        $("#WareHouseWiseUOM").val(UOMName);
        var Doc_id = $("#DocumentMenuId").val();
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId,
                    UomId: null,
                    DocumentMenuId: Doc_id
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function QtyFloatValueonly(el, evt) {

    //if ($("#UOMItemOrderType").is(":checked")) {
    //    $("#DestSwapQuantityError1").css("display", "none");
    //    $("#DestSwapQuantity").css("border-color", "#ced4da");
    //}
    //else {
    //    $("#SwapQuantityError1").css("display", "none");
    //    $("#SwapQuantity").css("border-color", "#ced4da");
    //}

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnChangeSwapQuantity(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var SwapQuantity = $("#SwapQuantity").val();
    var AvailableQuantity = $("#AvailableStockQuantity").val();

    if (parseFloat(SwapQuantity) > parseFloat(AvailableQuantity)) {
            debugger;
        $("#SwapQuantityError1").text($("#SwapQuantityIsExceedingTheAvailableQuantity").text());
        $("#SwapQuantityError1").css("display", "block");
        $("#SwapQuantity").css("border-color", "red");
        $("#SwapQuantity").val(parseFloat(0).toFixed(QtyDecDigit));

        HideErrorMsg();
        return false;
        }
        else {
        $("#SwapQuantityError1").css("display", "none");
        $("#SwapQuantity").css("border-color", "#ced4da");

        $("#DestSwapQuantityError1").css("display", "none");
        $("#DestSwapQuantity").css("border-color", "#ced4da");

        var mi_issueqty = parseFloat(parseFloat(SwapQuantity)).toFixed(parseFloat(QtyDecDigit));
        $("#SwapQuantity").val(mi_issueqty);
        $("#DestSwapQuantity").val(mi_issueqty);
    }
}
function OnChangeDestSwapQuantity() {
    $("#DestSwapQuantityError1").css("display", "none");
    $("#DestSwapQuantity").css("border-color", "#ced4da");

    var QtyDecDigit = $("#QtyDigit").text();
    var SwapQuantity = $("#DestSwapQuantity").val();
    var DestSwapQuantity = parseFloat(parseFloat(SwapQuantity)).toFixed(parseFloat(QtyDecDigit));
    $("#DestSwapQuantity").val(DestSwapQuantity);
}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var IssueQuantity = $("#SwapQuantity").val();
        var ItemId = $("#ddlItemName").val();
        var UOMName = $("#UOMSourc").val();
        var SwapQuantity = $("#SwapQuantity").val();
        var WarehouseId = $("#ddlWarehouse").val();
        var CompId = $("#HdCompId").val();
        var BranchId = $("#HdBranchId").val();
        var ItemName = $("#ddlItemName option:selected").text();
        var UOMId = $("#UOMIDSourc").val();
        var SelectedItemdetail = $("#HDSelectedBatchwise").val();
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#command").val();
        var TransType = $("#TransType").val();
        var status = $("#hdnStatus").val();
        $("#btnbatchdeatil").css("border-color", "#ced4da");

        if (IssueQuantity == "0" || IssueQuantity == "") {
            $("#BatchNumber").css("display", "block");
            $("#SwapQuantityError1").text($("#FillQuantity").text());
            $("#SwapQuantityError1").css("display", "block");
            $("#SwapQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/StockSwap/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType,
                        status: status,
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockBatchWise').html(data);
                    },
                });
        }
        else {
            var hdnStatus = $("#hdnStatus").val();
            if (hdnStatus == "" || hdnStatus == null || hdnStatus != "A") {
                BindItemBatchDetail();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/StockSwap/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemdetail: SelectedItemdetail,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            status: status,
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(SwapQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");
                        },
                    });
            }
            else {
                var SwapNumber = $("#SwapNumber").val();
                var SwapDate = $("#SwapDate").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/StockSwap/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            SwapNumber: SwapNumber,
                            SwapDate: SwapDate,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            status: status,
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(SwapQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");
                        },
                    });
            }         
        }
    }
    catch (err) {
    console.log("UserValidate Error : " + err.message);
    }
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        var IssueQuantity = $("#SwapQuantity").val();
        var ItemId = $("#ddlItemName").val();
        var UOMName = $("#UOMSourc").val();
        var SwapQuantity = $("#SwapQuantity").val();
        var WarehouseId = $("#ddlWarehouse").val();
        var CompId = $("#HdCompId").val();
        var BranchId = $("#HdBranchId").val();
        var ItemName = $("#ddlItemName option:selected").text();
        var UOMId = $("#UOMIDSourc").val();
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#command").val();
        var TransType = $("#TransType").val();
        var status = $("#hdnStatus").val();
        $("#btnserialdeatil").css("border-color", "#ced4da");

        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/StockSwap/getItemstockSerialWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemSerial: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType,
                        status: status,
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockSerialWise').html(data);
                    },
                });

        }
        else {
            var hdnStatus = $("#hdnStatus").val();
            if (hdnStatus == "" || hdnStatus == null || hdnStatus != "A") {
                BindItemSerialDetail();
                var SelectedItemSerial = $("#HDSelectedSerialwise").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/StockSwap/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemSerial: SelectedItemSerial,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            status: status,
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(SwapQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMId);
                        },
                    });
            }
            else {
                var SwapNumber = $("#SwapNumber").val();
                var SwapDate = $("#SwapDate").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/StockSwap/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            SwapNumber: SwapNumber,
                            SwapDate: SwapDate,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            status: status,
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(SwapQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMId);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#mi_lineSerialItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#mi_lineSerialIssueQty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }             
        }      
    }
    catch (err) {
        console.log("Material Issue Error : " + err.message);
    }
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(CheckNullNumber(IssuedQuantity)) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            IsuueFlag = false;
        }
    });
    if (IsuueFlag) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#BatchQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#mi_lineBatchItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });
            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (CheckNullNumber(ItemIssueQuantity) > 0) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="mi_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="mi_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="mi_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="mi_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="mi_lineBatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="mi_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="mi_lineBatchavl_batch_qty" value="${AvailableQty}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
            ValidateEyeColor("", "btnbatchdeatil", "N");
        }
        /*Commented by Hina on 05-02-2024 to  this table is not used in this page according to subham*/
        //$("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        //    debugger;
        //    var clickedrow = $(this);
        //    var ItemId = clickedrow.find("#hdItemId").val();
        //    if (ItemId == SelectedItem) {
        //        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        //    }
        //});
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    var QtyDigit = $("#QtyDigit").text();
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDigit));
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerialQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineSerialItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="mi_lineSerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="mi_lineSerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="mi_lineSerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="mi_lineSerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="mi_lineSerialSerialNO" value="${ItemSerialNO}" /></td>
            </tr>`
                );
            }
        });
        $("#SerialDetail").modal('hide');
        ValidateEyeColor("", "btnserialdeatil", "N");
        /*Commented by Hina on 05-02-2024 to  this table is not used in this page according to subham*/
        //$("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        //    debugger;
        //    var clickedrow = $(this);
        //    var ItemId = clickedrow.find("#hdItemId").val();
        //    if (ItemId == SelectedItem) {
        //        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        //    }
        //});
    }
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "0";
        }
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#ExceedingQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }
        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });
        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function functionConfirm(event) {
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function InsertSwapStockDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var flag = true;
    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");
    if ($("#ItemOrderType").is(":checked") || $("#UOMItemOrderType").is(":checked")) {
        if (CheckSwapStockItemValidation() == false) {
            return false;
        }
    }
    else {
        if (CheckSwapStockSubItemValidation() == false) {
            return false;
        }
    }
    var Batchflag = true, SerialFlag = true;
    Batchflag = CheckItemBatchValidation();
    if (Batchflag == false) {
        return false;
    }
    SerialFlag = CheckItemSerialValidation();
    if (SerialFlag == false) {
        return false;
    }
    if (flag == true && Batchflag == true && SerialFlag == true) {
        BindItemBatchDetail();
        BindItemSerialDetail();
        /*-----------Sub-item-------------*/

        var SubItemsListArr = SwapStock_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);

        /*-----------Sub-item end-------------*/
        $("#ddlItemName").attr("disabled", false);
        $("#ddlItemNameDest").attr("disabled", false);
        $("#AvailableStockQuantity").attr("disabled", false);
        $("#AvailableQuantityDest").attr("disabled", false);
        $("#DestSwapQuantity").attr("disabled", false);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    else {
        return false;
    }
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var IssueQuantity = $("#SwapQuantity").val();
    var ItemId = $("#ddlItemName").val();
    var UOMId = $("#UOMIDSourc").val();
    var Batchable = $("#hdi_batch").val();

    if (Batchable == "Y") {
        var TotalItemBatchQty = parseFloat("0");
        if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
            /*commented byHina on 05-02-2024 to validate by Eye color*/
            /*$("#btnbatchdeatil").css("border-color", "red");*/
            ValidateEyeColor("", "btnbatchdeatil", "Y");
            BatchableFlag = "Y";
            EmptyFlag = "Y";
        }
        else {
            $("#SaveItemBatchTbl >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                var bchIssueQty = currentRow.find('#mi_lineBatchIssueQty').val();
                var bchitemid = currentRow.find('#mi_lineBatchItemId').val();
                var bchuomid = currentRow.find('#mi_lineBatchUOMId').val();
                if (ItemId == bchitemid) {
                    TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                }
            });
            if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //$("#btnbatchdeatil").css("border-color", "#ced4da");
                ValidateEyeColor("", "btnbatchdeatil", "N");
            }
            else {
                //$("#btnbatchdeatil").css("border-color", "red");
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                ValidateEyeColor("", "btnbatchdeatil", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "N";
            }
        }
    }
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#BatchQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#BatchQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var IssueQuantity = $("#SwapQuantity").val();
    var ItemId = $("#ddlItemName").val();
    var UOMId = $("#UOMIDSourc").val();
    var Serialable = $("#hdi_serial").val();

    if (Serialable == "Y") {
        var TotalItemSerialQty = parseFloat("0");
        if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
            /*commented by Hina on 05-02-2024 to validate by Eye color*/
            //$("#btnserialdeatil").css("border-color", "red");
            ValidateEyeColor("", "btnserialdeatil", "Y");
            SerialableFlag = "Y";
            EmptyFlag = "Y";
        }
        else {
            $("#SaveItemSerialTbl >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                var srialIssueQty = currentRow.find('#mi_lineSerialIssueQty').val();
                var srialitemid = currentRow.find('#mi_lineSerialItemId').val();
                var srialuomid = currentRow.find('#mi_lineSerialUOMId').val();
                if (ItemId == srialitemid && UOMId == srialuomid) {
                    TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                }
            });

            if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                /*commented by Hina on 05-02-2024 to validate by Eye color*/
                //$("#btnserialdeatil").css("border-color", "#ced4da");
                ValidateEyeColor("", "btnserialdeatil", "N");
            }
            else {
                /*commented by Hina on 05-02-2024 to validate by Eye color*/
                //$("#btnserialdeatil").css("border-color", "red");
                ValidateEyeColor("", "btnserialdeatil", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "N";
            }
        }
    }
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#SerialQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#SerialQuantityDoesNotMatchWithSwapQuantity").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SwapStock_SubItemList() {
    var NewArr = new Array();
    if ($("#ItemOrderType").is(":checked")) {
        $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
            var row = $(this);
            debugger;
            //var Qty = row.find('#subItemQty').val();
            //if (parseFloat(CheckNullNumber(Qty)) > 0) {
                var List = {};
                List.SwapType = row.find("#SwapType").val();
                List.prod_id = row.find("#src_Item_id").val();
                List.prod_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.qty = row.find('#subItemQty').val();
                if (row.find('#subItemAvlQty').val() == null && row.find('#subItemAvlQty').val() == "") {
                    List.avl_qty = "";
                }
                else {
                    List.avl_qty = row.find('#subItemAvlQty').val();
                }
                NewArr.push(List);
            //}
        });
    }
    else {
        $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
            debugger;
            var row = $(this);
            var List = {};
            List.SwapType = row.find("#SwapType").val();
            List.prod_id = row.find("#src_Item_id").val();
            List.prod_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            if (row.find('#subItemQty').val() == "0") {
                var qty = row.find('#subItemQty').val();
                List.qty = parseFloat(qty).toFixed($("#QtyDigit").text());
            }
            else {
                List.qty = row.find('#subItemQty').val();
            }
            if (row.find('#subItemAvlQty').val() == null && row.find('#subItemAvlQty').val() == "") {
                List.avl_qty = "";
            }
            else {
                List.avl_qty = row.find('#subItemAvlQty').val();
            }
            NewArr.push(List);
        });
    }
    return NewArr; 
}
function CheckSwapStockSubItemValidation() {
    debugger;
    var Flag = 'N';
    var ddlItemName = $('#ddlItemName').val();
    if (ddlItemName == "" || ddlItemName == "0") {
        $('#spanProductName').text($("#valueReq").text());
        $("#spanProductName").css("display", "block");
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var ddlWarehouse = $('#ddlWarehouse').val();
    if (ddlWarehouse == "" || ddlWarehouse == "0") {
        $('#spanWarehouse').text($("#valueReq").text());
        $("#spanWarehouse").css("display", "block");
        $("[aria-labelledby='select2-ddlWarehouse-container']").css("border-color", "red");
        Flag = 'Y';
    }
    if (Flag == "Y") {
        return false;
    }
    var SrcSubItm = $("#SrcSubItem").val();
    if (SrcSubItm == "Y") {
        var len1 = $("#hdn_Sub_ItemDetailTbl tbody tr td").length;
        if (len1 == 0) {
            $("#SubItemReqQty").css("border", "1px solid red");
            swal("", $("#SubItemDetaiNoFound").text(), "warning");
            Flag == "Y"
            return false;
        }
    }
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckSwapStockItemValidation() {
    debugger;
    var Flag = 'N';
    var ddlItemName = $('#ddlItemName').val();
    if (ddlItemName == "" || ddlItemName == "0") {
        $('#spanProductName').text($("#valueReq").text());
        $("#spanProductName").css("display", "block");
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var ddlItemNameDest = $('#ddlItemNameDest').val();
    if (ddlItemNameDest == "" || ddlItemNameDest == "0") {
        $('#spanProductName1').text($("#valueReq").text());
        $("#spanProductName1").css("display", "block");
        $("[aria-labelledby='select2-ddlItemNameDest-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var ddlWarehouse = $('#ddlWarehouse').val();
    if (ddlWarehouse == "" || ddlWarehouse == "0") {
        $('#spanWarehouse').text($("#valueReq").text());
        $("#spanWarehouse").css("display", "block");
        $("[aria-labelledby='select2-ddlWarehouse-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var ddlWarehouseDest = $('#ddlWarehouseDest').val();
    if (ddlWarehouseDest == "" || ddlWarehouseDest == "0") {
        $('#spanWarehouse1').text($("#valueReq").text());
        $("#spanWarehouse1").css("display", "block");
        $("[aria-labelledby='select2-ddlWarehouseDest-container']").css("border-color", "red");
        Flag = 'Y';
    }
    var SwapQuantity = $('#SwapQuantity').val();
    if (SwapQuantity == "" || SwapQuantity == "0" || SwapQuantity == "0.000" || SwapQuantity == "0.00") {
        $('#SwapQuantityError1').text($("#valueReq").text());
        $("#SwapQuantityError1").css("display", "block");
        $("#SwapQuantity").css("border-color", "red");
        Flag = 'Y';
    }
    if ($("#UOMItemOrderType").is(":checked")) {
        //var Flag = 'Y';
        var DestSwapQuantity = $('#DestSwapQuantity').val();
        if (DestSwapQuantity == "" || DestSwapQuantity == "0" || DestSwapQuantity == "0.000" || DestSwapQuantity == "0.00") {
            $('#DestSwapQuantityError1').text($("#valueReq").text());
            $("#DestSwapQuantityError1").css("display", "block");
            $("#DestSwapQuantity").css("border-color", "red");
            Flag = 'Y';
        }
    }
    if (Flag == "Y") {
        return false;
    }
    var SrcSubItm = $("#SrcSubItem").val();
    var subItmeQty = $("#SwapQuantity").val();
    var DestSwapQuantity = $("#DestSwapQuantity").val();
    var ChecksubItemQty = 0;
    var ChecksubItemQty1 = 0;
    var ChecksubItemQty2 = 0;
    var DestSubItm = $("#DestSubItem").val();
    if (SrcSubItm == "Y") {
        var len1 = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='SrcSwapQty']").closest("tr").find("#src_Item_id[value='" + ddlItemName + "']").length;
        if (len1 == 0) {
            $("#SubItemReqQty").css("border", "1px solid red");
            swal("", $("#SubItemDetaiNoFound").text(), "warning");
            Flag == "Y"
            return false;
        }
        var TblReserveSubItem = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='SrcSwapQty']").closest("tr").find("#src_Item_id[value='" + ddlItemName + "']").closest('tr');
        TblReserveSubItem.each(function () {
            var InnerCrow = $(this);
            var subItemQty = InnerCrow.find("#subItemQty").val();
            ChecksubItemQty = parseFloat(ChecksubItemQty) + parseFloat(CheckNullNumber(subItemQty));
        });
        if (parseFloat(subItmeQty) != parseFloat(ChecksubItemQty)) {
            $("#SubItemReqQty").css("border", "1px solid red");
            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
            return false;
        }
        if (DestSubItm == "Y") {
            var len = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='DestSwapQty']").closest("tr").find("#dest_Item_id[value='" + ddlItemNameDest + "']").length;
            if (len == "0") {
                $("#DestSubItemReqQty").css("border", "1px solid red");
                swal("", $("#SubItemDetaiNoFound").text(), "warning");
                Flag == "Y"
                return false;
            }
            var TblReserveSubItem = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='DestSwapQty']").closest("tr").find("#dest_Item_id[value='" + ddlItemNameDest + "']").closest('tr');
            TblReserveSubItem.each(function () {
                var InnerCrow = $(this);
                var subItemQty = InnerCrow.find("#subItemQty").val();
                ChecksubItemQty1 = parseFloat(ChecksubItemQty1) + parseFloat(CheckNullNumber(subItemQty));
            });
            if (parseFloat(DestSwapQuantity) != parseFloat(ChecksubItemQty1)) {
                $("#DestSubItemReqQty").css("border", "1px solid red");
                swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                return false;
            }
        }
    }
    if (DestSubItm == "Y") {
        var len = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='DestSwapQty']").closest("tr").find("#dest_Item_id[value='" + ddlItemNameDest + "']").length;
        if (len == "0") {
            $("#DestSubItemReqQty").css("border", "1px solid red");
            swal("", $("#SubItemDetaiNoFound").text(), "warning");
            Flag == "Y"
            return false;
        }
        var TblReserveSubItem = $("#hdn_Sub_ItemDetailTbl tbody tr td #SwapType[value='DestSwapQty']").closest("tr").find("#dest_Item_id[value='" + ddlItemNameDest + "']").closest('tr');
        TblReserveSubItem.each(function () {
            var InnerCrow = $(this);
            var subItemQty = InnerCrow.find("#subItemQty").val();
            ChecksubItemQty2 = parseFloat(ChecksubItemQty2) + parseFloat(CheckNullNumber(subItemQty));
        });
        if (parseFloat(DestSwapQuantity) != parseFloat(ChecksubItemQty2)) {
            $("#DestSubItemReqQty").css("border", "1px solid red");
            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
            return false;
        }
    }
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function removeValidaction() {
    $("#btnbatchdeatil").css("border-color", "#ced4da");
    $("#btnserialdeatil").css("border-color", "#ced4da");

    $("#spanProductName1").css("display", "none");
    $("[aria-labelledby='select2-ddlItemNameDest-container']").css("border-color", "#ced4da");
   
    $("#spanProductName").css("display", "none");
    $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");

    $("#spanWarehouse1").css("display", "none");
    $("[aria-labelledby='select2-ddlWarehouseDest-container']").css("border-color", "#ced4da");

    $("#spanWarehouse").css("display", "none");
    $("[aria-labelledby='select2-ddlWarehouse-container']").css("border-color", "#ced4da");

    $("#SwapQuantityError1").css("display", "none");
    $("#SwapQuantity").css("border-color", "#ced4da");
}
function ResetWorningBorderColor() {
    //return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "N");
}
function SubItemDetailsPopUp(flag, e,data) {
    debugger
    $("#DestSubItemReqQty").css("border", "");
    $("#SubItemReqQty").css("border", "");
    if (data == "DestSwapQty") {
        var ProductId = $("#ddlItemNameDest").val();
        var ProductNm = $("#ddlItemNameDest option:selected").text();
        var wh_id = $("#ddlWarehouseDest").val();
        var UOM = $("#UOMDest").val();
    }
    else {
        var ProductId = $("#ddlItemName").val();
        var ProductNm = $("#ddlItemName option:selected").text();
        var wh_id = $("#ddlWarehouse").val();
        var UOM = $("#UOMSourc").val();
    }
    if (data == "SwapQty") {
        data = "SrcSwapQty";
    }
    var doc_no = $("#SwapNumber").val();
    var doc_dt = $("#SwapDate").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (data == "SrcSwapQty") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #src_Item_id[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.src_Item_id = row.find('#src_Item_id').val();
            List.dest_Item_id = row.find('#dest_Item_id').val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlQty').val();
            NewArr.push(List);
        });
    }
    else {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #dest_Item_id[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.src_Item_id = row.find('#src_Item_id').val();
            List.dest_Item_id = row.find('#dest_Item_id').val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlQty').val();
            NewArr.push(List);
        });
    }
    if (flag == "SwapQty") {
        Sub_Quantity = $("#SwapQuantity").val();
    }
    if (data == "DestSwapQty") {
        Sub_Quantity = $("#DestSwapQuantity").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdnStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockSwap/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt, 
            wh_id: wh_id,
            Type: data,
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
function ForwardBtnClick() {
    debugger;
    //var PRStatus = "";
    //PRStatus = $('#hdnStatus').val().trim();
    //if (PRStatus === "D" || PRStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var StkDt = $("#SwapDate").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: StkDt
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var PRStatus = "";
                    PRStatus = $('#hdnStatus').val().trim();
                    if (PRStatus === "D" || PRStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
                        var Doc_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");

                        Cmn_GetForwarderList(Doc_ID);

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    /*swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SwpNo = "";
    var SwpDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SourceType = "";

    docid = $("#DocumentMenuId").val();
    SwpNo = $("#SwapNumber").val();
    SwpDate = $("#SwapDate").val();
    $("#hdDoc_No").val(SwpNo);
    Remarks = $("#fw_remarks").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = "";
    if ($("#ItemOrderType").is(":checked")) {
        var SwapType = "I";
    }
    else if ($("#UOMItemOrderType").is(":checked")) {
        var SwapType = "U";
    }
    else {
        var SwapType = "S";
    }
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SwpNo != "" && SwpDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(SwpNo, SwpDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/StockSwap/ApproveSwapStockDetails?swp_no=" + SwpNo + "&swp_dt=" + SwpDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&SwapType=" + SwapType;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SwpNo != "" && SwpDate != "") {
            Cmn_InsertDocument_ForwardedDetail(SwpNo, SwpDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SwpNo != "" && SwpDate != "") {
            Cmn_InsertDocument_ForwardedDetail(SwpNo, SwpDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType;
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#SwapNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {

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
function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}
function BindDestinationItemList(Itemid) {
    debugger;
    $("#ddlItemNameDest").select2({
        ajax: {
            url: "/ApplicationLayer/StockSwap/GetDestProductListBind",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,             
                    page: params.page || 1,
                    Itemid: Itemid.trim()
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize
                var ItemListArrey = [];

               
                let selected = [];
                selected.push({ id: $("#ddlItemNameDest").val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));


                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;         
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }

                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                    }),                  
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
               
            },
            cache: true

        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
      
    });

}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl tbody tr").each(function () {
        var Crow = $(this);
        Crow.find("#ChkItemSerialWise").prop("checked", false);
    });
}