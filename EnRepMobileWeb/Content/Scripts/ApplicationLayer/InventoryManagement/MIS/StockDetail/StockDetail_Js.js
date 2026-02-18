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
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
    //    $("#datatable-buttons >tbody >tr > #tditem").css("background-color", "#ffffff");
    //    $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    //    clickedrow.find("#tditem").css("background-color", "rgba(38, 185, 154, .16)");
    //});
    //var ddlStockBy = $("#ddlStockBy").val();//Commented by Suraj Maurya on 06-03-2025
    //if (ddlStockBy == "LotBatchwisestock") {
    //    $("#ddlwarehouse").css("display", "");
    //}
    //else {
    //    $("#ddlwarehouse").css("display", "none");
    //}
    Cmn_ItemAliasListBind("ddlItemAlias");
   // $('#ddlHsnCode').select2();
   // $('#ddlWarehouse').select2();
    
    $("#ddlItemName").select2({
        ajax: {
            url: $("#ItemListName").val(),
            data: function (params) {
                var queryParameters = {
                    ItemName: params.term ,// search term like "a" then "an"
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
                            var select = { ID: "0", Name: " ---Select---"};
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

    //$("#ddlItemGroupName").select2({
    //    ajax: {
    //        url: $("#ItemGrpName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                GroupName: params.term // search term like "a" then "an"
    //                //Group: params.page
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
    //$("#ItemPortfolio").select2({
    //    ajax: {
    //        url: $("#ItemPortfName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                PortfolioName: params.term // search term like "a" then "an"
    //                //Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            //params.page = params.page || 1;
    //            return {
    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
    //$('#ddlStockBy').on('change',function () {
    //    showLoader();
    //});
    Cmn_initializeMultiselect(['#ddlBranch', '#ddlWarehouse', '#ddlHsnCode', '#ddlStkGlAccId', '#ddlSuppName']);
});

function BtnSearch() {
    debugger;
    //if ($("#IncludeZeroStock").is(":checked")) {
    //    FilterStockDetailList("Y");
    //}
    //else {
    //    FilterStockDetailList("N");
    //}
    StockDetailsDtList();//Above Commented And Added by Suraj on 19-12-2023
}
function FilterStockDetailList(IncludeZeroStock,flag,e) {
    debugger;
    
    try {
        //StockDetailsDtList();//Added by Suraj on 18-12-2023

        var StockBy = $("#ddlStockBy").val();
        var ItemId = $("#ddlItemName option:selected").val();
        var GroupId = $("#ddlItemGroupName option:selected").val();
        var BrachID = $("#ddlBranch").val();
        var WarehouseID = $("#ddlWarehouse").val();
        var hsnCode = $("#ddlHsnCode").val();
        var UpToDate = $("#calUpTodate").val();
        var PortfolioId = $("#ItemPortfolio option:selected").val();
        var StkGlAccId = $("#ddlStkGlAccId option:selected").val();
        var ItemAlias = $("#ddlItemAlias option:selected").val();
        var ExpiredItems = "N";
        if ($("#ExpiredItems").is(":checked")) {
            ExpiredItems = "Y";
        }
        else {
            ExpiredItems = "N";
        }
        var StockoutItems = "N";
        if ($("#StockoutItems").is(":checked")) {
            StockoutItems = "Y";
        }
        else {
            StockoutItems = "N";
        }
        var NearExpiryItm = "N";

        if (flag == "ItemGroupWise") { // Added by Suraj Maurya on 06-10-2025
            let row = $(e.target).closest('tr');
            GroupId = row.find("#hfItemID").val();
            BrachID = row.find("#hfBranchID").val();

            $("#hdnGroupId_ItemGrpWise").val(GroupId);
            $("#hdnStkGlAccId_StkGlAccWise").val(StkGlAccId);
            $("#hdnBranchId_ItemGrpAndStkGlAccWise").val(BrachID);
            $("#hdnItemAlias_StkAliasWise").val(ItemAlias);
            StockBy = StockBy + "_Popup";
        }
        else if (flag == "StockGlAccountWise") { // Added by Suraj Maurya on 06-10-2025
            let row = $(e.target).closest('tr');
            StkGlAccId = row.find("#hfItemID").val();
            BrachID = row.find("#hfBranchID").val();

            $("#hdnGroupId_ItemGrpWise").val(GroupId);
            $("#hdnStkGlAccId_StkGlAccWise").val(StkGlAccId);
            $("#hdnBranchId_ItemGrpAndStkGlAccWise").val(BrachID);
            $("#hdnItemAlias_StkAliasWise").val(ItemAlias);
            
            StockBy = StockBy + "_Popup";
        }
        else if (flag == "ItemAliasWiseStock") { // Added by Suraj Maurya on 06-10-2025
            let row = $(e.target).closest('tr');
            ItemAlias = row.find("#ItmNameSpan").text();
            BrachID = row.find("#hfBranchID").val();

            $("#hdnItemAlias_StkAliasWise").val(ItemAlias);
            $("#hdnGroupId_ItemGrpWise").val(GroupId);
            $("#hdnStkGlAccId_StkGlAccWise").val(StkGlAccId);
            $("#hdnBranchId_ItemGrpAndStkGlAccWise").val(BrachID);
            
            StockBy = StockBy + "_Popup";
        }
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/StockDetail/SearchStockDetail",
            data: {
                IncludeZeroStockFlag: IncludeZeroStock,
                StockBy: StockBy,
                ItemId: ItemId,
                GroupId: GroupId,
                BrachID: BrachID,
                WarehouseID: WarehouseID,
                UpToDate: UpToDate,
                PortfolioId: PortfolioId,
                hsnCode: hsnCode,
                ExpiredItems: ExpiredItems,
                StockoutItems: StockoutItems,
                NearExpiryItm: NearExpiryItm,
                StkGlAccId: StkGlAccId
            },
            success: function (data) {
                debugger;
                if (["ItemAliasWiseStock", "StockGlAccountWise", "ItemGroupWise"].includes(flag)) {
                    $('#PopupItemGroupDetail').html(data);
                }
                //else if (flag == "StockGlAccountWise") {
                //    $('#PopupStockGLAccountDetail').html(data);
                //}
                else {
                    $('#divlotandbatchtable').html(data);
                }
                
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
    
    if (["ItemGroupWise", "Branchwisestock", "StockGlAccountWise", "ItemAliasWiseStock"].includes(StockBy)) {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "none");
        $("#divBranchwiseRept_Issue").css("display", "block");

        if (type == "A" || type == "S") {
            $("#BW_th_billNumber").css("display", "");
            $("#BW_th_bilDate").css("display", "");
        }
        else {
            $('#BW_th_billNumber').css("display", "none");
            $('#BW_th_bilDate').css("display", "none");
        }
    }
    if (StockBy == "Warehousewisestock") {
        $("#divBatchLotewiseRept_Issue").css("display", "none");
        $("#divWarehousewiseRept_Issue").css("display", "block");
        $("#divBranchwiseRept_Issue").css("display", "none");
        WareHouse = clickedrow.find("#hfWarehouseID").val();
        ItmWarehouse = clickedrow.find("#ItmwarehouseSpan").text();

        if (type == "A" || type == "S") {
            $("#WW_th_billNumber").css("display", "");
            $("#WW_th_bilDate").css("display", "");
        }
        else {
            $('#WW_th_billNumber').css("display", "none");
            $('#WW_th_bilDate').css("display", "none");
        }
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

        if (type == "A" || type == "S") {
            $("#BLW_th_billNumber").css("display", "");
            $("#BLW_th_bilDate").css("display", "");
        }
        else {
            $('#BLW_th_billNumber').css("display", "none");
            $('#BLW_th_bilDate').css("display", "none");
        }
    }

    ItmName = clickedrow.find("#ItmNameSpan").text();
    ItmUOM = clickedrow.find("#ItmuomSpan").text();
    ItmBranch = clickedrow.find("#ItmBranchSpan").text(); 

    if (ItmCode != "" && ItmCode != null && Branch != "" && Branch != null ) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/StockDetail/GetItemReceivedDetailList",
                    data: { StockBy:StockBy,ItemID: ItmCode, Branch: Branch, TransType: type, WareHouse: WareHouse, LotNo: Lot, BatchNo: Batch, SerialNo: Serial, UpToDate: UpToDate },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        var tbl_id = "";
                        if (["ItemGroupWise", "Branchwisestock", "StockGlAccountWise", "ItemAliasWiseStock"].includes(StockBy)) {
                            tbl_id = "#BranchwiseDocDetailTbl";
                        }
                        if (StockBy == "ItemGroupWise") {
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
                                if (["ItemGroupWise", "Branchwisestock", "StockGlAccountWise", "ItemAliasWiseStock"].includes(StockBy)) {
                                    $("#txtbranchitemName").text(ItmName);
                                    $("#txtbranchuom").text(ItmUOM);
                                    $("#txtbranchbranch").text(ItmBranch);
                                    var rowIdx = 0;
                                    var TReceQty = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#BranchwiseDocDetailTbl tbody tr').remove();                                  
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        var BillNumber = "";
                                        var BillDate = "";
                                        if (type == "A" || type == "S") {

                                            BillNumber = `<td>${arr.Table[k].bill_no}</td>`
                                            BillDate = `<td>${arr.Table[k].bill_date}</td>`

                                            $("#BW_th_billNumber").css("display", "");
                                            $("#BW_th_bilDate").css("display", "");
                                        }
                                        else {
                                            BillNumber = `<td hidden="hidden"></td>`
                                            BillDate = `<td hidden="hidden"></td>`

                                            $('#BW_th_billNumber').css("display", "none");
                                            $('#BW_th_bilDate').css("display", "none");
                                        }
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#BranchwiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        `+ BillNumber+`
                                        `+ BillDate+`
                                        <td>${arr.Table[k].wh_name}</td>
                                        <td>${arr.Table[k].lot_id}</td>
                                        <td>${arr.Table[k].batch_no}</td>
                                        <td>${arr.Table[k].serial_no}</td>
                                        <td>${arr.Table[k].mfg_name}</td>
                                        <td>${IsNull(arr.Table[k].mfg_mrp,'')}</td>
                                        <td>${Cmn_FormatDate_ddmmmyyyy(arr.Table[k].mfg_date)}</td>
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
                                        if (type == "A" || type == "S") {

                                            BillNumber = `<td>${arr.Table[k].bill_no}</td>`
                                            BillDate = `<td>${arr.Table[k].bill_date}</td>`

                                            $("#WW_th_billNumber").css("display", "");
                                            $("#WW_th_bilDate").css("display", "");
                                        }
                                        else {
                                            BillNumber = `<td hidden="hidden"></td>`
                                            BillDate = `<td hidden="hidden"></td>`

                                            $('#WW_th_billNumber').css("display", "none");
                                            $('#WW_th_bilDate').css("display", "none");
                                        }
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#WarehousewiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        `+ BillNumber + `
                                        `+ BillDate +`
                                        <td>${arr.Table[k].lot_id}</td>
                                        <td>${arr.Table[k].batch_no}</td>
                                        <td>${arr.Table[k].serial_no}</td>
                                        <td>${arr.Table[k].mfg_name}</td>
                                        <td>${IsNull(arr.Table[k].mfg_mrp, '')}</td>
                                        <td>${Cmn_FormatDate_ddmmmyyyy(arr.Table[k].mfg_date)}</td>
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
                                        if (type == "A" || type == "S") {

                                            BillNumber = `<td>${arr.Table[k].bill_no}</td>`
                                            BillDate = `<td>${arr.Table[k].bill_date}</td>`

                                            $("#BLW_th_billNumber").css("display", "");
                                            $("#BLW_th_bilDate").css("display", "");
                                        }
                                        else {
                                            BillNumber = `<td hidden="hidden"></td>`
                                            BillDate = `<td hidden="hidden"></td>`

                                            $('#BLW_th_billNumber').css("display", "none");
                                            $('#BLW_th_bilDate').css("display", "none");
                                        }
                                        TReceQty = parseFloat(TReceQty) + parseFloat(arr.Table[k].qty_bs);
                                        $('#BatchLotwiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td id="tditem">${arr.Table[k].src_doc_id}</td>
                                        <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].src_doc_no}</td>
                                        <td>${arr.Table[k].src_doc_dt}</td>
                                        `+ BillNumber + `
                                        `+ BillDate +`
                                        <td class="num_right">${parseFloat(arr.Table[k].qty_bs).toFixed(QtyDecDigit)}</td>
                                        <td>${arr.Table[k].remarks}</td>
                                        <td>${arr.Table[k].EntryDate}</td>
                            </tr>`);
                                    }
                                    $("#BatchLotwiseTotalQty").text(parseFloat(TReceQty).toFixed(QtyDecDigit));
                                }
                            }
                            else {
                                if (["ItemGroupWise", "Branchwisestock", "StockGlAccountWise", "ItemAliasWiseStock"].includes(StockBy)) {
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
                            if (["ItemGroupWise", "Branchwisestock", "StockGlAccountWise", "ItemAliasWiseStock"].includes(StockBy)) {
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
    $("#ExpiredItems").prop("checked", false);
    $("#StockoutItems").prop("checked", false);
    $("#NearExpiryItems").prop("checked", false);

    $("#ddlWarehouse").val(0).trigger('change')
    $("#ddlItemGroupName").val(0).trigger('change')
    $("#ddlItemName").val(0).trigger('change')
    $("#ItemPortfolio").val(0).trigger('change')
    $("#ddlItemAlias").val("").trigger('change')

    $("#div_Stockout").addClass("pl-0");
    $("#div_Stockout").removeClass("clear");

    if (StockBy == "Branchwisestock") {
        $("#div_itemName").css("display", "");
        $("#ddlwarehouse").css("display", "none");
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemGroupDdl").css("display", "");
        $("#div_itemPortFolio").css("display", "");
        $("#div_hsnCode").css("display", "");
        $("#div_itemAliasDdl").css("display", "none");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "none");
    }
    if (StockBy == "Warehousewisestock") {
        $("#div_itemName").css("display", "");
        $("#ddlwarehouse").css("display", "block");
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemGroupDdl").css("display", "");
        $("#div_itemPortFolio").css("display", "");
        $("#div_hsnCode").css("display", "");
        $("#div_itemAliasDdl").css("display", "none");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "none");
    }
    if (StockBy == "LotBatchwisestock") {
        $("#div_itemName").css("display", "");
        $("#ddlwarehouse").css("display", "block");
        $("#ExpiredItemsTgl").css("display", "");
        $("#NearExpiryItemsTgl").css("display", "");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemGroupDdl").css("display", "");
        $("#div_itemPortFolio").css("display", "");
        $("#div_hsnCode").css("display", "");
        $("#div_itemAliasDdl").css("display", "none");
        $("#div_SupplierName").css("display", "");
        $("#NearExpiryItemsTgl").removeClass("clear");
        $("#IDClearDiv").css("display", "");
    }
    if (StockBy == "SubitemWise") {
        $("#div_itemName").css("display", "");
        $("#ddlwarehouse").css("display", "block");
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemAliasDdl").css("display", "none");
        $("#div_itemGroupDdl").css("display", "");
        $("#div_itemPortFolio").css("display", "");
        $("#div_hsnCode").css("display", "");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "none");
    }
    if (StockBy == "ItemGroupWise") {//Added by Suraj Maurya on 06-10-2025
        $("#div_itemName").css("display", "none");
        $("#ddlwarehouse").css("display", "none");
        
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemGroupDdl").css("display", "");
        $("#div_itemPortFolio").css("display", "");
        $("#div_hsnCode").css("display", "none");
        $("#div_itemAliasDdl").css("display", "none");
        $("#ddlWarehouse").val(0).trigger('change')
        $("#ddlItemName").val(0).trigger('change')
        $("#ddlStkGlAccId").val(0).trigger('change')
        //$("#div_Stockout").addClass("clear");
        //$("#div_Stockout").removeClass("pl-0");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "");
        $("#div_Stockout").removeClass("clear");
    }
    if (StockBy == "StockGlAccountWise") {//Added by Suraj Maurya on 06-10-2025
        $("#div_itemName").css("display", "none");
        $("#ddlwarehouse").css("display", "none");
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "");
        $("#div_itemGroupDdl").css("display", "none");
        $("#div_itemPortFolio").css("display", "none");
        $("#div_hsnCode").css("display", "none");
        $("#div_itemAliasDdl").css("display", "none");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "none");
    }
    if (StockBy == "ItemAliasWiseStock") {//Added by Suraj Maurya on 06-10-2025
        $("#div_itemName").css("display", "none");
        $("#ddlwarehouse").css("display", "none");
        $("#ExpiredItemsTgl").css("display", "none");
        $("#NearExpiryItemsTgl").css("display", "none");
        $("#div_stockGlAccount").css("display", "none");
        $("#div_itemGroupDdl").css("display", "none");
        $("#div_itemPortFolio").css("display", "none");
        $("#div_hsnCode").css("display", "none");
        $("#div_itemAliasDdl").css("display", "");
        $("#div_SupplierName").css("display", "none");
        $("#IDClearDiv").css("display", "none");
    }
    FilterStockDetailList("N");
}
function CheckedIncludeZeroStock() {
    $('#NearExpiryItems').prop("checked", false);
    $('#ExpiredItems').prop("checked", false);
    StockDetailsDtList();
}
function CheckedExpiredItems() {
    $('#IncludeZeroStock').prop("checked", false);
    $('#StockoutItems').prop("checked", false);
    $('#NearExpiryItems').prop("checked", false);
    StockDetailsDtList();
}
function CheckedNearExpiryItems() {
    $('#IncludeZeroStock').prop("checked", false);
    $('#StockoutItems').prop("checked", false);
    $('#ExpiredItems').prop("checked", false);
    StockDetailsDtList();
}
function CheckedStockoutItems() {
    $('#NearExpiryItems').prop("checked", false);
    $('#ExpiredItems').prop("checked", false);
    StockDetailsDtList();
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
        WareHouse = clickedrow.find("#hfWarehouseID").val();
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
                    url: "/ApplicationLayer/StockDetail/GetSubItemStkDetailList",
                    data: { StockBy: StockBy, ItemID: ItmCode, Branch: Branch, WareHouse: WareHouse, UpToDate: UpToDate, IncludeZero: includeZero },
                    //dataType: "json",
                    success: function (data) {
                        debugger;
                        //var tbl_id = "#Wh_wiseDocDetailTbl";

                        //cmn_delete_datatable(tbl_id);

                        if (data == 'ErrorPage') {
                            return false;
                        }
                        $("#PopupSubItemStockDetail").html(data);
                        $("#txtwh_itemName").text(ItmName);
                        $("#txtwh_uom").text(ItmUOM);
                        $("#txtwh_branch").text(ItmBranch);
                        $("#txtwh_warehouse").text(ItmWarehouse);

                        //if (data !== null && data !== "") {
                        //    var arr = [];
                        //    arr = JSON.parse(data);
                        //    if (arr.Table.length > 0) {
                        //           var rowIdx = 0;
                        //        $('#Wh_wiseDocDetailTbl tbody tr').remove();
                        //            for (var k = 0; k < arr.Table.length; k++) {
                        //                $('#Wh_wiseDocDetailTbl tbody').append(`<tr id="R${++rowIdx}">
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
                        //            }
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

function Export() {

    $('#export').click(function () {
        
        var StockBy = $("#ddlStockBy").val();
        var ItemId = $("#ddlItemName option:selected").val();
        var GroupId = $("#ddlItemGroupName option:selected").val();
        var BrachID = $("#ddlBranch").val();
        var WarehouseID = $("#ddlWarehouse").val();
        var hsnCode = $("#ddlHsnCode").val();
        var UpToDate = $("#calUpTodate").val();
        var PortfolioId = $("#ItemPortfolio option:selected").val();
        var IncludeZeroStock = "";
        if ($("#IncludeZeroStock").is(":checked")) {
            IncludeZeroStock = "Y";
        }
        else {
            IncludeZeroStock = "N";
        }
        var ExpiredItems = "";
        if ($("#ExpiredItems").is(":checked")) {
            ExpiredItems = "Y";
        }
        else {
            ExpiredItems = "N";
        }
        var StockoutItems = "";
        if ($("#StockoutItems").is(":checked")) {
            StockoutItems = "Y";
        }
        else {
            StockoutItems = "N";
        }
        var NearExpiryItm = "N";
        if ($("#NearExpiryItems").is(":checked")) {
            NearExpiryItm = "Y";
        }
        else {
            NearExpiryItm = "N";
        }

        var filters = IncludeZeroStock + "," + StockBy + "," + ItemId + "," + GroupId + "," + BrachID + "," + WarehouseID + "," + UpToDate + "," + PortfolioId + "," + hsnCode + "," + ExpiredItems + "," + StockoutItems + "," + NearExpiryItm;

        var searchValue = $("#stockDetailDtList_filter input[type=search]").val();
        window.location.href = "/ApplicationLayer/StockDetail/ExportStkDtlData?searchValue=" + searchValue + "&filters=" + filters;
       
    });
}

function OnClickItemsInfoBtn(e,flag) {
    if ($("#IncludeZeroStock").is(":checked")) {
        FilterStockDetailList("Y", flag,e);
    }
    else {
        FilterStockDetailList("N", flag,e);
    }
}

