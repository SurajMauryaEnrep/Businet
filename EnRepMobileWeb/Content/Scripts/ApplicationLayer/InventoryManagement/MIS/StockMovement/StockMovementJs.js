$(document).ready(function () {
    debugger;
    //$("#Item_name").select2();
    DynamicSerchableItemDDL("", "#Item_name", "", "", "", "AllItems");
    $("#Batch_no").select2();
    $("#Serial_no").select2();
});

function BtnSearch() {
    debugger;
    FilterStockDetailList();
}
function FilterStockDetailList() {
    debugger;
    try {
        var MovementBy = $("#ddl_movementby").val();
        var ItemId = $("#Item_name option:selected").val();
        var Batch = $("#Batch_no option:selected").val();
        var Serial_no = $("#Serial_no option:selected").val();
        var Fromdt = $("#from_dt").val();
        var Todt = $("#To_dt").val();
        /*if (MovementBy == "SW") {*/
            if (ItemId == "" || ItemId == null || ItemId == "0") {
                document.getElementById("vmitem_id").innerHTML = $("#valueReq").text();
                $("[aria-labelledby='select2-Item_name-container']").css("border-color", "red");
                return;
            }
        //}
        //if (MovementBy == "BW") {
        //    if (ItemId == "" || ItemId == null || ItemId == "0") {
        //        document.getElementById("vmitem_id").innerHTML = $("#valueReq").text();
        //        $("[aria-labelledby='select2-Item_name-container']").css("border-color", "red");
        //        return;
        //    }
        //    if (Batch == "" || Batch == null || Batch == "0") {
        //        document.getElementById("vmbatch_no").innerHTML = $("#valueReq").text();
        //        $("[aria-labelledby='select2-Batch_no-container']").css("border-color", "red");
        //        return;
        //    }
        //}
      
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/StockMovement/SearchMovementDetail",
            data: {
                MovementBy: MovementBy,
                ItemId: ItemId,
                BatchNo: Batch,
                Serial_no: Serial_no,
                Fromdt: Fromdt,
                Todt: Todt
           },
            success: function (data) {
                debugger;
                $('#div_movementlist').html(data);
                hideLoader();
            },
            error: function OnError(xhr, errorType, exception) {
                hideLoader();
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("stock movement  Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickReceiptIconBtn(e, type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var Branch = "";
    var WareHouse = "";
    var Lot = "";
    var Batch = "";
    var Serial = "";
    var ItmName = "";
    var ItmUOM = "";
    var ItmBranch = "";
    var ItmWarehouse = "";

    var StockBy = $("#ddlStockBy").val();
    var UpToDate = $("#calUpTodate").val();
    var QtyDecDigit = $("#QtyDigit").text();
    ItmCode = clickedrow.find("#hfItemID").val();
    Branch = clickedrow.find("#hfBranchID").val();
    if (type == "A") {
        $("#SDPopupHeading").text($("#span_ReceiptsDetail").text());
    }
    if (type == "S") {
        $("#SDPopupHeading").text($("#span_IssuedDetail").text());
    }
    if (type == "R") {
        $("#SDPopupHeading").text($("#span_ReservedDetail").text());
    }
    if (type == "RJ") {
        $("#SDPopupHeading").text($("#span_RejectedStockDetail").text());
    }
    if (type == "RW") {
        $("#SDPopupHeading").text($("#span_ReworkableStockDetail").text());
    }

    if (StockBy == "Branchwisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "none");
        $("#divBranchwiseRept_Issue").css("display", "block");
    }
    if (StockBy == "Warehousewisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "block");
        $("#divBranchwiseRept_Issue").css("display", "none");
        WareHouse = clickedrow.find("#hfWarehouseID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();
    }
    if (StockBy == "LotBatchwisestock") {

        $("#divBatchLotewiseRept_Issue").css("display", "block");
        $("#divWarehousewiseRept_Issue").css("display", "none");
        $("#divBranchwiseRept_Issue").css("display", "none");
        WareHouse = clickedrow.find("#hfWarehouseID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();
        Lot = clickedrow.find("#SpanLotNo").text();
        Batch = clickedrow.find("#SpanBatchNo").text();
        Serial = clickedrow.find("#SpanSerialNo").text();
    }

    ItmName = clickedrow.find("#ItmNameSpan").text();
    ItmUOM = clickedrow.find("#ItmuomSpan").text();
    ItmBranch = clickedrow.find("#ItmBranchSpan").text();

    if (ItmCode != "" && ItmCode != null && Branch != "" && Branch != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/StockDetail/GetItemReceivedDetailList",
                    data: { StockBy: StockBy, ItemID: ItmCode, Branch: Branch, TransType: type, WareHouse: WareHouse, LotNo: Lot, BatchNo: Batch, SerialNo: Serial, UpToDate: UpToDate },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        var tbl_id = "";
                        if (StockBy == "Branchwisestock") {
                            tbl_id = "#BranchwiseDocDetailTbl";
                        }
                        if (StockBy == "Warehousewisestock") {
                            tbl_id = "#WarehousewiseDocDetailTbl";
                        }
                        if (StockBy == "LotBatchwisestock") {
                            tbl_id = "#BatchLotwiseDocDetailTbl";
                        }

                        cmn_delete_datatable(tbl_id);

                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                if (StockBy == "Branchwisestock") {
                                    $("#txtbranchitemName").text(ItmName);
                                    $("#txtbranchuom").text(ItmUOM);
                                    $("#txtbranchbranch").text(ItmBranch);
                                    var rowIdx = 0;
                                    var TReceQty = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#BranchwiseDocDetailTbl tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#BranchwiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td >${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        <td>${arr.Table[k].wh_name}</td>
                                        <td>${arr.Table[k].lot_id}</td>
                                        <td>${arr.Table[k].batch_no}</td>
                                        <td>${arr.Table[k].serial_no}</td>
                                        <td class="num_right">${parseFloat(arr.Table[k].qty_bs).toFixed(QtyDecDigit)}</td>
                                        <td>${arr.Table[k].remarks}</td>
                                        <td>${arr.Table[k].EntryDate}</td>
                            </tr>`);
                                    }
                                    $("#BranchwiseTotalQty").text(parseFloat(TReceQty).toFixed(QtyDecDigit));
                                }
                                if (StockBy == "Warehousewisestock") {
                                    $("#txtwarehouseitemName").text(ItmName);
                                    $("#txtwarehouseuom").text(ItmUOM);
                                    $("#txtwarehousebranch").text(ItmBranch);
                                    $("#txtwarehousewarehouse").text(ItmWarehouse);
                                    var rowIdx = 0;
                                    var TReceQty = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#WarehousewiseDocDetailTbl tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#WarehousewiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td>${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        <td>${arr.Table[k].lot_id}</td>
                                        <td>${arr.Table[k].batch_no}</td>
                                        <td>${arr.Table[k].serial_no}</td>
                                        <td class="num_right">${parseFloat(arr.Table[k].qty_bs).toFixed(QtyDecDigit)}</td>
                                        <td>${arr.Table[k].remarks}</td>
                                        <td>${arr.Table[k].EntryDate}</td>
                            </tr>`);
                                    }
                                    $("#WarehousewiseTotalQty").text(parseFloat(TReceQty).toFixed(QtyDecDigit));
                                }
                                if (StockBy == "LotBatchwisestock") {
                                    $("#txtbatchlotitemName").text(ItmName);
                                    $("#txtbatchlotuom").text(ItmUOM);
                                    $("#txtbatchlotbranch").text(ItmBranch);
                                    $("#txtbatchlotwarehouse").text(ItmWarehouse);
                                    $("#txtbatchlotlot").text(Lot);
                                    $("#txtbatchlotbatch").text(Batch);
                                    $("#txtbatchlotserial").text(Serial);
                                    var rowIdx = 0;
                                    var TReceQty = parseFloat(0).toFixed(QtyDecDigit);

                                    $('#BatchLotwiseDocDetailTbl tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#BatchLotwiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td>${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        <td class="num_right">${parseFloat(arr.Table[k].qty_bs).toFixed(QtyDecDigit)}</td>
                                        <td>${arr.Table[k].remarks}</td>
                                        <td>${arr.Table[k].EntryDate}</td>
                            </tr>`);
                                    }
                                    $("#BatchLotwiseTotalQty").text(parseFloat(TReceQty).toFixed(QtyDecDigit));
                                }
                            }
                            else {
                                if (StockBy == "Branchwisestock") {
                                    $('#BranchwiseDocDetailTbl tbody tr').remove();
                                    $("#BranchwiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                                }
                                if (StockBy == "Warehousewisestock") {
                                    $('#WarehousewiseDocDetailTbl tbody tr').remove();
                                    $("#WarehousewiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                                }
                                if (StockBy == "LotBatchwisestock") {
                                    $('#BatchLotwiseDocDetailTbl tbody tr').remove();
                                    $("#BatchLotwiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                                }

                            }
                        }
                        else {
                            if (StockBy == "Branchwisestock") {
                                $('#BranchwiseDocDetailTbl tbody tr').remove();
                                $("#BranchwiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                            }
                            if (StockBy == "Warehousewisestock") {
                                $('#WarehousewiseDocDetailTbl tbody tr').remove();
                                $("#WarehousewiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                            }
                            if (StockBy == "LotBatchwisestock") {
                                $('#BatchLotwiseDocDetailTbl tbody tr').remove();
                                $("#BatchLotwiseTotalQty").text(parseFloat(0).toFixed(QtyDecDigit));
                            }
                        }

                        cmn_apply_datatable(tbl_id);
                    },
                });
        } catch (err) {
            //console.log(" Error : " + err.message);
        }
    }
}


function OnChangeItemName() {
    var itemid = "";
    itemid = $("#Item_name").val();

    if (itemid == "" || itemid == null || itemid == "0") {
        document.getElementById("vmitem_id").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Item_name-container']").css("border-color", "red");
        return;
    }
    else {
        document.getElementById("vmitem_id").innerHTML = "";
        $("[aria-labelledby='select2-Item_name-container']").css("border-color", "#ced4da");
    }

    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/StockMovement/GetBatchDetails",
        data: {
            ItemId: itemid
        },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            }
            $("#txtuom").val("");
            if (data !== null && data !== "" && data !== "{}") {
                var arr = [];
                arr = JSON.parse(data);

                $("#txtuom").val(arr.Table1[0].uom_alias);
                if (arr.Table1[0].i_batch == "Y") {
                    $("#Batch_no").attr("disabled", false);
                    $("#Serial_no option").remove();
                    $('#Serial_no').append(`<option value="0">---Select---</option>`);
                    $("#Serial_no").attr("disabled", true);

                    if (arr.Table.length > 0) {
                        $("#Batch_no option").remove();

                        $('#Batch_no').append(`<option value="0">---Select---</option>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Batch_no').append(`<option value="${arr.Table[i].batch_no}">${arr.Table[i].batch_no}</option>`);
                        }
                    }
                }
                else if (arr.Table1[0].i_serial == "Y") {
                    $("#Serial_no").attr("disabled", false);
                    $("#Batch_no option").remove();
                    $('#Batch_no').append(`<option value="0">---Select---</option>`);
                    $("#Batch_no").attr("disabled", true);

                    if (arr.Table2.length > 0) {
                        $("#Serial_no option").remove();

                        $('#Serial_no').append(`<option value="0">---Select---</option>`);
                        for (var i = 0; i < arr.Table2.length; i++) {
                            $('#Serial_no').append(`<option value="${arr.Table2[i].serial_no}">${arr.Table2[i].serial_no}</option>`);
                        }
                    }
                }
                else {
                    $("#Serial_no").val("0").trigger('change');
                    $("#Batch_no").val("0").trigger('change');
                    $("#Batch_no").attr("disabled", true);
                    $("#Serial_no").attr("disabled", true);
}
                
              
            }
        },
        error: function OnError(xhr, errorType, exception) {
            debugger;
        }
    });
}
