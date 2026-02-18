/************************************************
Javascript Name:Stock Details List
Created By:Prem
Created Date: 26-04-2021
Description: This Javascript use for the Stock Details List many function

Modified By:
Modified Date:
Description:
*************************************************/

$(document).ready(function () {
    debugger;
    $("#ddlShopfloor").css("display", "none");
   // $('#ddlHsnCode').select2();
    $("#ddlItemName").select2({
        ajax: {
            url: $("#ItemListName").val(),
            data: function (params) {
                var queryParameters = {
                    ItemName: params.term, // search term like "a" then "an"
                    page: params.page || 1
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                var pageSize = 20;
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            }
        },
    });
    //$("#ddlItemGroupName").select2({
    //    ajax: {
    //        url: $("#ItemGrpName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                GroupName: params.term, // search term like "a" then "an"
    //                page: params.page || 1
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            var pageSize = 20;
    //            var page = params.page || 1;
    //            Fdata = data.slice((page - 1) * pageSize, page * pageSize);
    //            if (page == 1) {
    //                if (Fdata[0] != null) {
    //                    if (Fdata[0].Name.trim() != "---Select---") {
    //                        var select = { ID: "0", Name: " ---Select---" };
    //                        Fdata.unshift(select);
    //                    }
    //                }
    //            }
    //            return {
    //                results: $.map(Fdata, function (val, Item) {
    //                    return { id: val.ID, text: val.Name };
    //                }),
    //                pagination: {
    //                    more: (page * pageSize) < data.length
    //                }
    //            };
    //        }
    //    },
    //});
    $.ajax({
        url: $("#ItemGrpName").val(),
        dataType: "json",
        success: function (data) {
            var $ddl = $("#ddlItemGroupName");
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.ID !== "0") { // Skip option with value '0'
                    $ddl.append($('<option>', {
                        value: val.ID,
                        text: val.Name
                    }));
                }
            });
            Cmn_initializeMultiselect(['#ddlItemGroupName']);
        }
    });
    //$("#ItemPortfolio").select2({
    //    ajax: {
    //        url: $("#ItemPortfName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                PortfolioName: params.term, // search term like "a" then "an"
    //                page: params.page || 1
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            var pageSize = 20;
    //            var page = params.page || 1;
    //            Fdata = data.slice((page - 1) * pageSize, page * pageSize);
    //            if (page == 1) {
    //                if (Fdata[0] != null) {
    //                    if (Fdata[0].Name.trim() != "---Select---") {
    //                        var select = { ID: "0", Name: " ---Select---" };
    //                        Fdata.unshift(select);
    //                    }
    //                }
    //            }
    //            return {
    //                results: $.map(Fdata, function (val, Item) {
    //                    return { id: val.ID, text: val.Name };
    //                }),
    //                pagination: {
    //                    more: (page * pageSize) < data.length
    //                }
    //            };
    //        }
    //    },
    //});
    $.ajax({
        url: $("#ItemPortfName").val(),
        dataType: "json",
        success: function (data) {
            var $ddl = $("#ItemPortfolio");
            $ddl.empty();
            $.each(data, function (i, val) {
                if (val.ID !== "0") { // Skip option with value '0'
                    $ddl.append($('<option>', {
                        value: val.ID,
                        text: val.Name
                    }));
                }
            });
            Cmn_initializeMultiselect(['#ItemPortfolio']);
        }
    });
    $('#ddlStockBy').on('change', function () {
        showLoader();
    });
    Cmn_initializeMultiselect(['#ddlBranch', '#ddlHsnCode','#ddlShopfloorID']);
});

function BtnSearch() {
    debugger;
    if ($("#IncludeZeroStock").is(":checked")) {
        FilterStockDetailList("Y");
    }
    else {
        FilterStockDetailList("N");
    }
}
function FilterStockDetailList(IncludeZeroStock) {
    debugger;
    try {
        var StockBy = $("#ddlStockBy").val();
        var ItemId = $("#ddlItemName option:selected").val();
        var GroupId = $("#ddlItemGroupName option:selected").val();
        var BrachID = $("#ddlBranch").val();
       //var BrachID = cmn_getddldataasstring("#ddlBranch").val();
        var ShopfloorID = $("#ddlShopfloorID").val();
        //var ShopfloorID = cmn_getddldataasstring("#ddlShopfloorID").val();
        var hsnCode = $("#ddlHsnCode").val();
       // var hsnCode = cmn_getddldataasstring("#ddlHsnCode").val();
        var UpToDate = $("#calUpTodate").val();
        var PortfolioId = $("#ItemPortfolio option:selected").val();
       // var PortfolioId = cmn_getddldataasstring("#ItemPortfolio").val();
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/StockDetailShopfloor/SearchStockDetailShopfloor",
            data: {
                IncludeZeroStockFlag: IncludeZeroStock,
                StockBy: StockBy,
                ItemId: ItemId,
                GroupId: GroupId,
                BrachID: BrachID,
                ShflID: ShopfloorID,
                UpToDate: UpToDate,
                PortfolioId: PortfolioId,
                hsnCode: hsnCode
            },
            success: function (data) {
                debugger;
                $('#divlotandbatchtable').html(data);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickReceiptIconBtn(e,type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var Branch = "";
    var ShopfloorID = "";
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

    if (StockBy == "Branchwisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "none");
        $("#divBranchwiseRept_Issue").css("display", "block");
    }
    if (StockBy == "Warehousewisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "block");
        $("#divBranchwiseRept_Issue").css("display", "none");
        ShopfloorID = clickedrow.find("#hfShopfloorID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();
    }
    if (StockBy == "LotBatchwisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "block");
        $("#divWarehousewiseRept_Issue").css("display", "none");
        $("#divBranchwiseRept_Issue").css("display", "none");
        ShopfloorID = clickedrow.find("#hfShopfloorID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();
        Lot = clickedrow.find("#SpanLotNo").text();
        Batch = clickedrow.find("#SpanBatchNo").text();
        Serial = clickedrow.find("#SpanSerialNo").text();
    }

    ItmName = clickedrow.find("#ItmNameSpan").text();
    ItmUOM = clickedrow.find("#ItmuomSpan").text();
    ItmBranch = clickedrow.find("#ItmBranchSpan").text();

    if (ItmCode != "" && ItmCode != null && Branch != "" && Branch != null ) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/StockDetailShopfloor/GetItemReceivedDetailList",
                    data: { StockBy: StockBy, ItemID: ItmCode, Branch: Branch, TransType: type, ShflID: ShopfloorID, LotNo: Lot, BatchNo: Batch, SerialNo: Serial, UpToDate: UpToDate },
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
                                        <td>${arr.Table[k].src_doc_id}</td>
                                        <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        <td>${arr.Table[k].shfl_name}</td>
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
                                        <td>${arr.Table[k].src_doc_id}</td>
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
                                        <td>${arr.Table[k].src_doc_id}</td>
                                        <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].src_doc_no}</td>
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
function OnChangeStockByddl() {
    var StockBy = $("#ddlStockBy").val();
    $("#IncludeZeroStock").prop("checked", false);
    if (StockBy == "Branchwisestock") {
        $("#ddlShopfloor").val("0");
        $("#ddlShopfloorID").val("0");
        $("#ddlShopfloor").css("display", "none");
    }
    if (StockBy == "Warehousewisestock") {
        $("#ddlShopfloor").css("display", "block");
    }
    if (StockBy == "LotBatchwisestock") {
        $("#ddlShopfloor").css("display", "block");
    }
    if (StockBy == "SubitemWise") {
        $("#ddlShopfloor").css("display", "block");
    }

    FilterStockDetailList("N");
}
function CheckedIncludeZeroStock() {
    debugger;
    if ($("#IncludeZeroStock").is(":checked")) {
        FilterStockDetailList("Y");
    }
    else {
        FilterStockDetailList("N");
    }
}
function OnClickSubItemStockIconBtn(e, type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var Branch = "";
    var WareHouse = "";
    var ItmName = "";
    var ItmUOM = "";
    var ItmBranch = "";
    var ItmWarehouse = "";

    var StockBy = $("#ddlStockBy").val();
    var UpToDate = $("#calUpTodate").val();
    //var QtyDecDigit = $("#QtyDigit").text();
    ItmCode = clickedrow.find("#hfItemID").val();
    Branch = clickedrow.find("#hfBranchID").val();

    if (StockBy == "Branchwisestock") {
        $("#div_subitempop_Wh").css("display", "none");
    }
    if (StockBy == "Warehousewisestock") {
        $("#div_subitempop_Wh").css("display", "block");
        WareHouse = clickedrow.find("#hfShopfloorID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();
    }

    ItmName = clickedrow.find("#ItmNameSpan").text();
    ItmUOM = clickedrow.find("#ItmuomSpan").text();
    ItmBranch = clickedrow.find("#ItmBranchSpan").text();

    var includeZero = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        includeZero = "Y";
    }

    if (ItmCode != "" && ItmCode != null && Branch != "" && Branch != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/StockDetailShopfloor/GetSubItemStkDetailList",
                    data: { StockBy: StockBy, ItemID: ItmCode, Branch: Branch, WareHouse: WareHouse, UpToDate: UpToDate, IncludeZero: includeZero },
                    //dataType: "json",
                    success: function (data) {
                        debugger;

                        
                        //var tbl_id = "#Wh_wiseDocDetailTbl";

                        //cmn_delete_datatable(tbl_id);

                        if (data == 'ErrorPasge') {
                            return false;
                        }
                        $("#PopUpSubItemStockDetail").html(data);

                        $("#txtwh_itemName").text(ItmName);
                        $("#txtwh_uom").text(ItmUOM);
                        $("#txtwh_branch").text(ItmBranch);
                        $("#txtwh_warehouse").text(ItmWarehouse);

                        //if (data !== null && data !== "") {
                        //    var arr = [];
                        //    arr = JSON.parse(data);
                        //    if (arr.Table.length > 0) {
                        //        var rowIdx = 0;
                        //        $('#Wh_wiseDocDetailTbl tbody tr').remove();
                        //        for (var k = 0; k < arr.Table.length; k++) {
                        //            $('#Wh_wiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                        //                <td width="5%">${k + 1}</td>
                        //                <td>${arr.Table[k].subitemname}</td>
                        //                <td class="num_right">${arr.Table[k].opening}</td>
                        //                <td class="num_right">${arr.Table[k].receipts}</td>
                        //                <td class="num_right">${arr.Table[k].issued}</td>
                        //                <td class="num_right">${arr.Table[k].reserved}</td>
                        //                <td class="num_right">${arr.Table[k].rejected}</td>
                        //                <td class="num_right">${arr.Table[k].reworkabled}</td>
                        //                <td class="num_right">${arr.Table[k].totalstk}</td>
                        //                <td class="num_right">${arr.Table[k].avlstk}</td>
                        //    </tr>`);
                        //        }
                        //    }
                        //    else {
                        //        $('#Wh_wiseDocDetailTbl tbody tr').remove();
                        //    }
                        //}
                        //else {
                        //    $('#Wh_wiseDocDetailTbl tbody tr').remove();
                        //}

                        //cmn_apply_datatable(tbl_id);
                    },
                });
        } catch (err) {
        }
    }
}
