$(document).ready(function () {
    var src_type = $('#src_type').val();
    if (src_type == "H") {
        $('#AdSReturnItmDetailsTbl tbody tr').each(function () {
            var row = $(this);
            var Sno = row.find("#SNohiddenfiled").val();
            row.find("#wh_id" + Sno).select2();
        });
    }
    BindSupplierList();
    $('#AdSReturnItmDetailsTbl').on('click', '#deleteAdhocItem', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        var ItemCode = $(this).closest('tr').find("#hdItemId").val();
        var ReturnValue = $(this).closest('tr').find("#ReturnValue").val();
        $(this).closest('tr').remove();
        var prt_no = $("#ReturnNumber").val();
        if (prt_no == null || prt_no == "") {
            if ($('#AdSReturnItmDetailsTbl tbody tr').length <= 1) {
                debugger;
                $("#ddlSupplierNameList").prop("disabled", false);
            }
        }
        updateItemSerialNumber(src_type);
        AfterDeleteResetSI_ItemTaxDetail();
        DeleteItemBatchSerialOrderQtyDetails(ItemCode);
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        DeleteVoucherDetail(ItemCode, "", "", ReturnValue)
    });
    $('#PReturnItmDetailsTbl').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var ItemCode = $(this).closest('tr').find("#hdItemId").val();
        var GrnNo = $(this).closest('tr').find("#GRNNumber").val();
        var GRNDate = $(this).closest('tr').find("#GRNDate").val();
        var GrnDt = GRNDate.split("-").reverse().join("-");

        var InvoiceValue = $(this).closest('tr').find("#InvoiceValue").val();
        var ReturnValue = $(this).closest('tr').find("#ReturnValue").val();
        var prt_no = $("#ReturnNumber").val();
        if (prt_no == null || prt_no == "") {
            if ($('#PReturnItmDetailsTbl tr').length <= 1) {
                
                $("#ddlSupplierNameList").prop("disabled", false);
                $("#ddlDocumentNumber").prop("disabled", false);
                $("#hdSelectedSourceDocument").val(null);
                //$("#BtnAttribute").css('display', 'block');
                $(".plus_icon1").css('display', 'block');
            }
        }

        
        var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
        if (FReturnItemDetails != null && FReturnItemDetails != "") {
            if (FReturnItemDetails.length > 0) {
                
                var ArrFiltData = FReturnItemDetails;
                var ArrFilt_Data = ArrFiltData.filter(v => v.ItmCode == ItemCode && v.IconGRNNumber == GrnNo && v.IconGRNDate == GrnDt)
                if (ArrFilt_Data.length > 0) {
                    DeleteSubItemQtyDetail(ItemCode, GrnNo, GrnDt);
                }
            }
        }
        //OnClickInvoiceValueIconBtn(this, "change");
        updateItemSerialNumber();
        DeleteVoucherDetail(ItemCode, GrnNo, InvoiceValue, ReturnValue)
        DeleteItemBatchSerialDetail(ItemCode, GrnNo);

    });
    sessionStorage.removeItem("ItemRetQtyDetails");
    GetViewDetails();
    PRTNo = $("#ReturnNumber").val();
    $("#hdDoc_No").val(PRTNo);
    //var DnNarr = $('#DebitNoteRaisedAgainstPurRet').text();
    //$("#DnNarr").val(DnNarr);
    BindDDLAccountList();
    if (src_type == "H") {
        $("#PReturnItmDetailsTbl").css("display", "none")
        $("#AdSReturnItmDetailsTbl").css("display", "")
        $("#TaxDetailAccordian").css("display", "")

        $("#DivInvoiceNumber").css("display", "none");
        $("#DivInvoiceDate").css("display", "none");
        $("#InvWiseAddItem").css("display", "none");
        $("#th_inv_value").css("display", "none");
        $("#th_oc_value").css("display", "none");

        //$("#DivBillno").css("display", "");
        //$("#DivBillDate").css("display", "");
        $("#AdHocBill_no").attr("disabled", false);
        $("#AdHocBill_dt").attr("disabled", false);
        var cust_id = $("#ddlSupplierNameList").val()
        var PageDisable = $("#PageDisable").val()
        if (cust_id != "0" && cust_id != "" && cust_id != null) {
            if (PageDisable == "Y") {
                $("#addbtn").css("display", "none");
            }
            else {
                $("#addbtn").css("display", "");
            }

        }
        else {
            $("#addbtn").css("display", "none");
        }
        BindScrpItmList("1");
        //BindWarehouseList("1");
    }
    else {
        $("#TaxDetailAccordian").css("display", "none")
        $("#DivInvoiceNumber").css("display", "");
        $("#DivInvoiceDate").css("display", "");
        $("#InvWiseAddItem").css("display", "");
        $("#th_oc_value").css("display", "");

        //$("#DivBillno").css("display", "none");
        //$("#DivBillDate").css("display", "none");
    }
    CancelledRemarks("#Cancelled", "Disabled");
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        if (src_type == "H") {
            $('#AdSReturnItmDetailsTbl > tbody > tr').each(function () {

                var cellText = $(this).find('#hdItemId').val().trim();
                if (msg.includes(cellText)) {
                    $(this).css('border', '3px solid red');
                }
            });
        }
        else {
            $('#PReturnItmDetailsTbl > tbody > tr').each(function () {

                var cellText = $(this).find('#hdItemId').val().trim();
                if (msg.includes(cellText)) {
                    $(this).css('border', '3px solid red');
                }
            });
        }
        $("#hdnStockItemWiseMessage").val("");
    }
});
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#PD_BatchItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#PD_SerialItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    //$("#SaveItemOrderQtyDetails TBODY TR").each(function () {
    //    var row = $(this);
    //    var rowitem = row.find("#PD_OrderQtyItemID").val();
    //    if (rowitem == Itemid) {
    //        var Docno = row.find("#PD_OrderQtyDocNo").val();

    //        $("#ddlOrderNumber option[value='" + Docno + "']").removeClass("select2-hidden-accessible");

    //        debugger;
    //        $(this).remove();
    //    }
    //});
}
function OnChangeSourType() {
    debugger;
    var src_type = $('#src_type').val();
    if (src_type == "H") {
        $("#PReturnItmDetailsTbl").css("display", "none")
        $("#AdSReturnItmDetailsTbl").css("display", "")
        $("#TaxDetailAccordian").css("display", "")
        $("#DivInvoiceNumber").css("display", "none");
        $("#DivInvoiceDate").css("display", "none");
        $("#AdHocBill_no").attr("disabled", false);
        $("#AdHocBill_dt").attr("disabled", false);
        $("#InvWiseAddItem").css("display", "none");
        $("#th_inv_value").css("display", "none");
        $("#th_oc_value").css("display", "none");
    }
    else {
        $("#TaxDetailAccordian").css("display", "none")
        $("#DivInvoiceNumber").css("display", "");
        $("#DivInvoiceDate").css("display", "");
        $("#InvWiseAddItem").css("display", "");
        $("#th_inv_value").css("display", "");
        $("#th_oc_value").css("display", "");
        $("#AdHocBill_no").attr("disabled", true);
        $("#AdHocBill_dt").attr("disabled", true);
    }
}
function AddAdHocItemNewRow() {
    var rowIdx = 0;
    var rowCount = $('#AdSReturnItmDetailsTbl >tbody >tr').length + 1;
    $('#AdSReturnItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                                                    <td class="red center">
                                                                        <i class="fa fa-trash" id="deleteAdhocItem" aria-hidden="true" title="Delete"></i>
                                                                    </td>
                                                                    <td class="sr_padding"><span id="SpanRowId">${rowCount}</span>
                                                                        <input class="" type="hidden" id="SNohiddenfiled" value="${rowCount}" /></td>
                                                                    <td>
                                                                        <div class="col-sm-11 lpo_form no-padding" id="multiWrapper">
                                                                           <select class="form-control" id="ItemName${rowCount}" name="ItemName${rowCount}" onchange ="OnChangePRItemName(event)"></select>
                                                                           <span id="ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon">
                                                                            <button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information"></button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled="disabled" value="">
                                                                        <input type="hidden" id="hdUOMId" value="" />
                                                                        <input type="hidden" id="HsnNo" value="" style="display: none;">
                                                                        <input type="hidden" id="hfbatchable" value="" />
                                                                        <input type="hidden" id="hfserialable" value="" />
                                                                        <input type="hidden" id="hfexpiralble" value="" />
                                                                        <input class="" type="hidden" id="hdn_item_gl_acc" />
                                                                        <input type="hidden" id="sub_item" value="" />
                                                                        <input class="" type="hidden" id="hdItemId" />
                                                                        <input class="" type="hidden" id="ItemType" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-11 no-padding">
                                                                            <div class="lpo_form">
                                                                                  <select class="form-control" id="wh_id${rowCount}" onchange="OnChangeWarehouse(this,event)"></select>
                                                                                  <span id="wh_Error${rowCount}" class="error-message is-visible"></span>
                                                                            </div>
                                                                        </div>
 <div class=" col-sm-1 i_Icon">
                                  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(event)" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                  </div>
                                                                    </td>
  <td>
                                    <div class="col-sm-9 lpo_form no-padding">
                                        <input id="AvailableStock" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="AvailableStock" required="required" placeholder="0000.00"  disabled="">
                                     </div>
                                    <div class="col-sm-3 i_Icon" id="div_SubItemAvlStk">
                                        <button type="button" id="SubItemAvlStk" disabled="disabled" class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                  </td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form no-padding lpo_form">
                                                                            <input id="ReturnQuantity" value="" class="form-control num_right" onpaste = "return CopyPasteAvoidFloat(event)" onchange ="OnChangePRItemQty(event)" autocomplete="off" type="text" name="ReturnQuantity" placeholder="0000.00">
                                                                            <span id="ReturnQuantity_Error" class="error-message is-visible"></span>
                                                                        </div>                                                                     
                                                                        <div class="col-sm-2 i_Icon pl-0" id="div_SubItemReturnQty">
                                                                            <button type="button" id="SubItemReturnQty" disabled="disabled" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('AdPRTReturnQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_subitemdetail").text()}"=""> </button>
                                                                        </div>
                                                                    </td>
                                                                                                     <td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator " data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                  <input type="hidden" id="hfbatchable" value="" style="display: none;">
                                  <input type="hidden" id="hfserialable" value="" style="display: none;">
                                  </td>
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="Price" disabled="disabled" value="" onchange="OnChangePRTItemRate(event)" onkeypress="return AmountFloatQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="Price" placeholder="0000.00">
                                                                            <span id="Price_Error" class="error-message is-visible"></span>
                                                                            <input id="item_ass_val" type="hidden" class="form-control date num_right" autocomplete="off" name="item_ass_val" placeholder="0000.00" onblur="this.placeholder='0000.00'">
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-10 no-padding">
                                                                            <input id="item_tax_amt" disabled="" class="form-control num_right" value="" autocomplete="off" type="text" name="item_tax_amt" placeholder="0000.00">
                                                                        </div>
                                                                        <div class="col-sm-2 no-padding">
                                                                            <button type="button" class="calculator" id="BtnTxtCalculation" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}" data-original-title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="ReturnValue" value="" class="form-control num_right" autocomplete="off" type="text" name="ReturnValue" placeholder="0000.00" disabled="">
                                                                    </td>
                                                                    <td>
                                                                        <textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks" value="" placeholder="${$("#span_remarks").text()}" onmouseover="OnMouseOver(this)" title="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                </tr>`)
    BindScrpItmList(rowCount);
    BindWarehouseList(rowCount);
}
function OnChangePRItemQty(el, evt) {

}

function GetSubItemAvlStock(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#ItemName" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#ItemName" + rowNo).val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStock").val();
    var hdwh_Id = Crow.find("#wh_id" + rowNo).val();
    var hd_Status = $("#hdPRTStatus").val();
    var src_type = $('#src_type').val();
    if (hd_Status == "A") {
        if (src_type == "H") {
            SubItemDetailsPopUp("AdPRTReturnQty", e,"avlstock")
        }
        else {
            SubItemDetailsPopUp("PRTReturnQty", e)
        }
    }
    else {
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwh_Id, AvlStk, "wh");
    }


}
function BindScrpItmList(ID) {
    BindItemList("#ItemName", ID, "#AdSReturnItmDetailsTbl", "#SNohiddenfiled", "", "AdHocPrt");
}
function BindWarehouseList(id) {
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/GetWarehouseList1",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        var src_type = $('#src_type').val();
                        if (src_type == "H") {
                            var PreWhVal = $("#AdSReturnItmDetailsTbl #wh_id" + id).val();
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).html(s);
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).val(IsNull(PreWhVal, '0'));
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).select2();
                        }
                        else {
                            var PreWhVal = $("#SReturnItmDetailsTbl #wh_id" + id).val();
                            $("#SReturnItmDetailsTbl #wh_id" + id).html(s);
                            $("#SReturnItmDetailsTbl #wh_id" + id).val(IsNull(PreWhVal, '0'));
                            $("#SReturnItmDetailsTbl #wh_id" + id).select2();
                        }
                    }
                }
            },
        });
}
function OnChangePRItemName(e) {
    var clickedrow = $(e.target).closest("tr");
    var NewItm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    NewItm_ID = clickedrow.find("#ItemName" + SNo).val();
    var OldItemID = clickedrow.find("#hdItemId").val();
    clickedrow.find("#hdItemId").val(NewItm_ID);
    var ItemID = clickedrow.find("#hdItemId").val();
    if (NewItm_ID == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    Cmn_DeleteSubItemQtyDetail(OldItemID);
    //DeleteItemBatchOrderQtyDetails(OldItemID);
    ClearRowDetails(e);
    try {
        $("#HdnTaxOn").val("Tax");
        Cmn_BindUOM(clickedrow, NewItm_ID, "", "Y", "sale");

    } catch (err) {
        console.log(err.message)
    }
}
function ClearRowDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#HsnNo").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#ReturnQuantity").val("");
    clickedrow.find("#Price").val("");
    clickedrow.find("#ReturnValue").val("");
    clickedrow.find("#remarks").val("");
    clickedrow.find("#wh_id" + SNo).val(0).trigger('change.select2');
    clickedrow.find("#wh_Error" + SNo).css("display", "none");
    clickedrow.find("[aria-labelledby='select2-wh_id" + SNo + "-container']").css("border-color", "#ced4da");
}
function OnChangeWarehouse(el, evt) {
    debugger;
    var src_type = $('#src_type').val();
    var QtyDecDigit = $('#QtyDigit').text();
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#RowId").val();
    var hdItemId = clickedrow.find("#hdItemId").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (src_type == "H") {
        var Itm_ID;
        var SNo = clickedrow.find("#SNohiddenfiled").val();
        Itm_ID = clickedrow.find("#ItemName" + SNo).val();
        var Index = clickedrow.find("#SNohiddenfiled").val();
    }
    var Warehouse = clickedrow.find("#wh_id" + Index).val();
    if (Warehouse == "0") {
        clickedrow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "red");
        if (src_type == "H") {
            clickedrow.find("#wh_Error" + Index).text($("#valueReq").text());
            clickedrow.find("#wh_Error" + Index).css("display", "block");
        }
        else {
            clickedrow.find("#sp_Warehouse").text($("#valueReq").text());
            clickedrow.find("#sp_Warehouse").css("display", "block");
        }
    }
    else {
        clickedrow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "#ced4da");
        if (src_type == "H") {
            clickedrow.find("#wh_Error" + Index).text("");
            clickedrow.find("#wh_Error" + Index).css("display", "none");
        }
        else {
            clickedrow.find("#sp_Warehouse").text("");
            clickedrow.find("#sp_Warehouse").css("display", "none");
        }
    }
    var row = $("#ReturnedQtyDetailsTbl >tbody >tr #Item_id[value='" + hdItemId + "']").closest('tr');
    var Sno = row.find("#SNohiddenfiled").val();
    row.find("#wh_id" + Sno).val(Warehouse);
    if (Warehouse != "0" && Warehouse != null) {
        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: Itm_ID,
                WarehouseId: Warehouse,
                UomId: null,
                br_id: null,
                DocumentMenuId: DocumentMenuId,
            },
            success: function (data) {
                var avaiableStock = JSON.parse(data);
                var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                clickedrow.find("#AvailableStock").val(parseavaiableStock);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    }

    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            for (j = 0; j < FReturnItemDetails.length; j++) {
                debugger;
                var ItmCode = FReturnItemDetails[j].ItmCode;
                var ItmUomId = FReturnItemDetails[j].ItmUomId;
                var wh_id = Warehouse;
                var Lot = FReturnItemDetails[j].Lot;
                var Batch = FReturnItemDetails[j].Batch;
                var Serial = FReturnItemDetails[j].Serial;
                var RetQty = FReturnItemDetails[j].RetQty;
                var IconShipNumber = FReturnItemDetails[j].IconShipNumber;
                var IconShipDate = FReturnItemDetails[j].IconShipDate;
                var TotalQty = FReturnItemDetails[j].TotalQty;
                var Batchable = FReturnItemDetails[j].Batchable;
                var Serialable = FReturnItemDetails[j].Serialable;
                if (Batch === null || Batch === "null") {
                    Batch = "";
                }
                if (Serial === null || Serial === "null") {
                    Serial = "";
                }
                if (ItmCode == hdItemId) {
                    NewArr.push({ TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty })
                }
                else {
                    NewArr.push({ TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: FReturnItemDetails[j].wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty })
                }

            }
            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));
        }
    }
    var hdItemId = clickedrow.find("#hdItemId").val();
    var sub_item = clickedrow.find("#sub_item").val();
    if (sub_item == "Y") {
        var row = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value='" + hdItemId + "']").closest('tr');
        row.each(function () {
            var row = $(this).closest("tr");
            row.find("#subItemWhId").val(Warehouse);

        });
    }
}
//function ItemStockWareHouseWise(el, evt) {
//    try {
//        debugger;
//        var clickedrow = $(evt.target).closest("tr");
//        var Sno = clickedrow.find("#SNohiddenfiled").val();
//        var ItemId = clickedrow.find("#hdItemId").val();;
//        var ItemName = clickedrow.find("#ItemName" + Sno+ "option:selected").val();
//        var UOMName = clickedrow.find("#UOM").val();
//        var DocumentMenuId = $("#DocumentMenuId").val();
//        $.ajax(
//            {
//                type: "Post",
//                url: "/Common/Common/getItemstockWareHouselWise",
//                data: {
//                    ItemId: ItemId, UomId: null, DocumentMenuId: DocumentMenuId
//                },
//                success: function (data) {
//                    debugger;
//                    $('#ItemStockWareHouseWise').html(data);
//                    $("#WareHouseWiseItemName").val(ItemName);
//                    $("#WareHouseWiseUOM").val(UOMName);
//                },
//            });

//    } catch (err) {
//        console.log("UserValidate Error : " + err.message);
//    }
//}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        PLBindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var Index = clickedrow.find("#SNohiddenfiled").val();
        var subitem = clickedrow.find("#sub_item").val();
        var ddlId = "#wh_id" + Index;
        var ItemId = clickedrow.find("#hdItemId").val();
        var WarehouseId = clickedrow.find(ddlId).val();

        var ItemName = clickedrow.find("#ItemName" + Index + " option:selected").text();
        var UOMName = clickedrow.find("#UOM").val();
        var PackedQty = clickedrow.find("#ReturnQuantity").val();
        var SelectedItemdetail = $("#HDSelectedBatchwise").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var docid = $("#DocumentMenuId").val();
        var TransType = $("#qty_TransType").val();/*This is use only Partial View */
        var Command = $("#PageDisable").val();/*This is use only Partial View */
        var PL_Status = $("#hdPRTStatus").val();
            QtyDecDigit = $("#QtyDigit").text();

        if (PL_Status == "" || PL_Status == null || PL_Status == "D") {
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PurchaseReturn/getItemStockBatchWise",
                data: {
                    ItemId: ItemId,
                    WarehouseId: WarehouseId,
                    Status: PL_Status,
                    SelectedItemdetail: SelectedItemdetail,
                    docid: docid,
                    TransType: TransType,
                    Command: Command
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockBatchWisePaking').html(data);
                    $("#HDBatchSubItem").val(subitem);
                    if (subitem == "N") {
                        $("#SubItemBatchPopupQty").attr("disabled", true);
                    } else {
                        $("#SubItemBatchPopupQty").attr("disabled", false);
                    }

                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(PackedQty);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDUOMBatchWise").val(UOMId);


                    var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                        $("#SaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var BtItemId = row.find("#PD_BatchItemId").val();
                            if (BtItemId === ItemId) {
                                TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                            }
                        });
                    }

                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var issueQty = row.find("#IssuedQuantity").val();
                        //var bt_sale = row.find("#bt_sale").text();
                        //if (bt_sale == "N") {
                        //    row.find("#IssuedQuantity").attr("disabled", true)
                        //}
                        if (issueQty != null && issueQty != "") {
                            row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                        }
                    });
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));

                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var ResQty = row.find("#ResQty").val();
                        if (ResQty != null && ResQty != "") {
                            if (parseFloat(ResQty) > parseFloat(0)) {
                                row.find("#IssuedQuantity").prop('readonly', true);
                                row.find("#Icon_Order_resqty").removeAttr("style")
                            }
                            else {
                                row.find("#Icon_Order_resqty").css("display", "none")
                            }
                        }
                        else {
                            row.find("#Icon_Order_resqty").css("display", "none")
                        }
                    });
                    //Added by Suraj on 20-02-2024
                    try {
                        //For Auto fill Quantity on FIFO basis in the Batch Table.
                        //this will work only first time after save old value will come in the table
                        //Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", PackedQty, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                        Cmn_PackingAutoFillBatchQty("BatchWiseItemStockTbl", PackedQty, "ToatlAvlQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                    } catch (err) {
                        console.log('Error : ' + err.message)
                    }

                },
            });
        }
        else {
            var prt_no = $("#ReturnNumber").val();
            var PL_Date = $("#txtReturnDate").val();
            var docid = $("#DocumentMenuId").val();
            $.ajax({
                type: "Post",
                url: "/ApplicationLayer/PurchaseReturn/getItemStockBatchWiseAfterInsert",
                data: {
                    prt_no: prt_no,
                    prt_dt: PL_Date,
                    Status: PL_Status,
                    ItemId: ItemId,
                    TransType: TransType,
                    Command: Command

                },
                success: function (data) {
                    debugger;
                    $('#ItemStockBatchWisePaking').html(data);
                    if (subitem == "N") {
                        $("#SubItemBatchPopupQty").attr("disabled", true);
                    } else {
                        $("#SubItemBatchPopupQty").attr("disabled", false);
                    }
                    $("#ItemNameBatchWise").val(ItemName);
                    $("#UOMBatchWise").val(UOMName);
                    $("#QuantityBatchWise").val(PackedQty);
                    $("#HDWhIDBatchWise").val(WarehouseId);
                    $("#HDItemNameBatchWise").val(ItemId);
                    $("#HDUOMBatchWise").val(UOMId);

                    var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                    if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                        $("#SaveItemBatchTbl TBODY TR").each(function () {
                            var row = $(this)
                            var btItemId = row.find("#PD_BatchItemId").val();
                            if (btItemId === ItemId) {
                                TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#PD_BatchIssueQty").val());
                            }
                        });
                    }
                    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));
                    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                        var row = $(this)
                        var ResQty = row.find("#ResQty").val();
                        if (ResQty != null && ResQty != "") {
                            if (parseFloat(ResQty) > parseFloat(0)) {
                                row.find("#IssuedQuantity").prop('readonly', true);
                                row.find("#Icon_Order_resqty").removeAttr("style")
                            }
                            else {
                                row.find("#Icon_Order_resqty").css("display", "none")
                            }
                        }
                        else {
                            row.find("#Icon_Order_resqty").css("display", "none")
                        }
                    });
                },
            });
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    debugger;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var HdItemId = $("#HDItemNameBatchWise").val();
    var sub_item = $("#HDBatchSubItem").val();
    var ItemPackedQty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    var FlagMsg = "N";
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var TotalAvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(TotalAvailableQuantity)) {
            FlagMsg = "Y";
            row.find("#IssuedQuantity_Error").text($("#ExceedingQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "btnbatchdeatil", "Y");
            return false;
        }
    });
    debugger;
    if (FlagMsg === "N") {
        if (parseFloat(ItemPackedQty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#IssuedqtyshouldbeequaltoPackedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_BatchItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });
            var value = 0;
            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemBatchAvlstock = row.find("#AvailableQuantity").val();
                    var ItemBatchResstock = row.find("#ResQty").val();
                    var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var MfgName = row.find("#BtMfgName").val();
                    var MfgMrp = row.find("#BtMfgMrp").val();
                    var MfgDate = row.find("#BtMfgDate").val();
                    var LotNo = row.find("#Lot").val();
                    var landedCost = row.find("#LandedCost").val();
                    value = value+ parseFloat(ItemIssueQuantity) * parseFloat(landedCost);

                    $('#SaveItemBatchTbl tbody').append(`<tr>
                    <td><input type="text" id="PD_BatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="PD_BatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="PD_BatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="PD_BatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="PD_BatchResAvlStk" value="${ItemBatchResstock}" /></td>
                    <td><input type="text" id="PD_BatchBatchAvlStk" value="${ItemBatchAvlstock}" /></td>
                    <td><input type="text" id="PD_BatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="PD_BatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="MfgName" value="${MfgName}" /></td>
                    <td><input type="text" id="MfgMrp" value="${MfgMrp}" /></td>
                    <td><input type="text" id="MfgDate" value="${MfgDate}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
            $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#hdItemId").val();
                if (ItemId == SelectedItem) {
                    ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
                }
            });
            var GLRow = $("#AdSReturnItmDetailsTbl > tbody >tr #hdItemId[value='" + HdItemId + "']").closest('tr');
            var price = value / ItemPackedQty;
            GLRow.find("#Price").val(parseFloat(price).toFixed($("#RateDigit").text())).trigger('change');
        }
    }
}
function OnChangeIssueQty(el, evt) {
    try {
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var TotalAvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (IssuedQuantity != "" && IssuedQuantity != null && TotalAvailableQuantity != "" && TotalAvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(TotalAvailableQuantity)) {
                clickedrow.find("#IssuedQuantity_Error").text($("#ExceedingQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }
        TotalBatchIssueQty();
    }
    catch (err) {
        console.log("Error : " + err.message);
    }
}
function TotalBatchIssueQty() {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var Issueqty = row.find("#IssuedQuantity").val();
        if (Issueqty != "" && Issueqty != null) {
            TotalIssueQty = TotalIssueQty + parseFloat(Issueqty);
        }
    });
    $("#BatchwiseTotalIssuedQuantity").text(parseFloat(parseFloat(TotalIssueQty)).toFixed(parseFloat(QtyDecDigit)));
}
function PLBindItemBatchDetail() {
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#PD_BatchLotNo').val();
            batchList.ItemId = row.find('#PD_BatchItemId').val();
            batchList.UOMId = row.find('#PD_BatchUOMId').val();
            batchList.BatchNo = row.find('#PD_BatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#PD_BatchBatchAvlStk').val();
            batchList.IssueQty = row.find('#PD_BatchIssueQty').val();

            var ExDate = row.find('#PD_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            batchList.bt_sale = row.find('#PD_bt_sale').val();
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }

}
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105102150");
}
function BindData() {
    
    debugger
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                //rowid = parseFloat(rowid) + 1;
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty()
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="TextddlAcc${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#TextddlAcc' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + rowid).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            /* var UOM = $(data.element).data('uom');*/
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
                                '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });
            });
        }
    }

    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        if (AccID != '0' && AccID != "" ) {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
var ValDecDigit = $("#ValDigit").text();
function GetViewDetails() {
    debugger;
    
    if ($("#hdItemBatchSerialDetailList").val() != null && $("#hdItemBatchSerialDetailList").val() != "") {
        debugger
        var arr2 = $("#hdItemBatchSerialDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdItemBatchSerialDetailList").val("");

        sessionStorage.removeItem("ItemRetQtyDetails");
        sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(arr));
    }

}
function updateItemSerialNumber(src_type) {
    
    var SerialNo = 0;
    if (src_type == "H") {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            SerialNo = SerialNo + 1;
            currentRow.find("#SpanRowId").text(SerialNo);
        });
    }
    else {
        $("#PReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            SerialNo = SerialNo + 1;
            currentRow.find("#SpanRowId").text(SerialNo);
        });
    }
};
function CheckFormValidation() {
    
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var rowcount = 0;
    var src_type = $('#src_type').val();
    if (src_type == "H") {
        rowcount = $('#AdSReturnItmDetailsTbl tr').length;

    }
    else {
        rowcount = $('#PReturnItmDetailsTbl tr').length;
    }
    var return_date = $('#txtReturnDate').val();

    var ValidationFlag = true;
    var SupplierName = $('#ddlSupplierNameList').val();
    var DocumentNumber = $('#ddlDocumentNumber').val();

    var tDnDate = $('#txtsrcdocdate').val();
    if (SupplierName == "" || SupplierName == "0") {
        document.getElementById("vmsupp_id").innerHTML = $("#valueReq").text();
        $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }
    if (src_type == "H") {
        var AdHocBill_no = $("#AdHocBill_no").val();
        var AdHocBill_dt = $("#AdHocBill_dt").val();
        if (AdHocBill_no == "" || AdHocBill_no == "0" || AdHocBill_no == null) {
            document.getElementById("vmAdHocBill_no").innerHTML = $("#valueReq").text();
            $('#AdHocBill_no').css("border-color", "red");
            //$("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
            ValidationFlag = false;
        }
        if (AdHocBill_dt == "" || AdHocBill_dt == "0" || AdHocBill_dt == null) {
            document.getElementById("vmAdHocBill_dt").innerHTML = $("#valueReq").text();
            $('#AdHocBill_dt').css("border-color", "red");
            //$("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    else {
        if (DocumentNumber == "" || DocumentNumber == "0" || DocumentNumber == null) {
            document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
            $('#ddlDocumentNumber').css("border-color", "red");
            $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    var RT_Status = $('#hdPRTStatus').val();
    if (RT_Status == "RT") {
        if (CheckCancelledStatus() == false) {
            return false;
        }
    }
    if ($("#Cancelled").is(":checked")) {

        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            return false;
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }
        
    }
   
    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hdPRTStatus") == false) {
        return false;
    }
    if (ValidationFlag == true) {
        if (rowcount > 1) {
            var flag = CheckItemLevelValidations();
            if (flag == false) {
                return false;
            }
            if (src_type == "H") {
                var Batchflag = CheckItemBatch_Validation();
                if (Batchflag == false) {
                    return false;
                }
                if (CheckValidations_forAdHocSubItems() == false) {
                    return false;
                }
                var GstApplicable = $("#Hdn_GstApplicable").text();
                if (GstApplicable == "Y") {
                    if (Cmn_taxVallidation("AdSReturnItmDetailsTbl", "item_tax_amt", "hdItemId", "Hdn_TaxCalculatorTbl", "ItemName") == false) {
                        return false;
                    }
                }
            }
            else {
                var Batchflag = CheckItemBatchValidation();
                if (Batchflag == false) {
                    return false;
                }
                if (ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", "", "", src_type) == false) {
                    swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                    return false
                }
                else {
                    $("#SaveAndExitBtn").attr("data-dismiss", "modal");

                }
            } 
           
            var VoucherValidationFlag = Check_VoucherValidations();
            if (VoucherValidationFlag == false) {
                return false;
            }
            if (Math.abs(parseFloat(CheckNullNumber($("#DrTotalInBase").text())) - parseFloat(CheckNullNumber($("#TotalReturnValue").val()))) > 1) {
                //Modified by Suraj Maurya on 31-01-2025 to show message if difference is greater then 1. as discussed with vishal sir (T.N. : 1302)
                swal("", $("#ReturnValueMismatchedWithTotalGlValue").text(), "warning");
                return false;
            }
            if (flag == true) {
                var PurchaseReturnItemDetailList = new Array();
                if (src_type == "H") {
                    $("#AdSReturnItmDetailsTbl TBODY TR").each(function () {

                        var row = $(this);
                        var ItemList = {};

                        var Srno = row.find("#SNohiddenfiled").val();
                        var ItemType = row.find("#ItemType").val();
                        ItemList.SourceDocumentNo = "";
                        ItemList.SourceDocumentDate = "";
                        ItemList.ItemId = row.find('#hdItemId').val();
                        ItemList.ItemName = row.find('#ItemName' + Srno).val();
                        ItemList.subitem = row.find('#sub_item').val();
                        //ItemList.UOMId = row.find('#hdUOMId').val();
                        if (row.find('#hdUOMId').val() == "" || row.find('#hdUOMId').val() == null) {
                            ItemList.UOMId = 0;
                        }
                        else {
                            ItemList.UOMId = row.find('#hdUOMId').val();
                        }
                        ItemList.uom_name = row.find('#UOM').val();
                        ItemList.GRNNo = "";
                        ItemList.GRNDate = "";
                        ItemList.GRNQuantity = "";
                        ItemList.ReturnQuantity = row.find("#ReturnQuantity").val();
                        ItemList.InvoiceValue = 0;
                        ItemList.ReturnValue = row.find("#ReturnValue").val();
                        ItemList.ReasonForReturn = "";
                        ItemList.ItemRemarks = row.find('#remarks').val();
                        ItemList.item_acc_id = IsNull(row.find('#hdn_item_gl_acc').val(), "");
                        ItemList.prt_avl = IsNull(row.find('#AvailableStock').val(), 0);
                        ItemList.item_rate = IsNull(row.find('#Price').val(), 0);
                        ItemList.item_tax_amt = IsNull(row.find('#item_tax_amt').val(), 0);
                        if (ItemType == "Service") {
                            ItemList.wh_id = 0;
                        }
                        else {
                            ItemList.wh_id = row.find('#wh_id' + Srno).val();
                        }
                        PurchaseReturnItemDetailList.push(ItemList);
                    });
                }
                else {
                    $("#PReturnItmDetailsTbl TBODY TR").each(function () {

                        var row = $(this);
                        var ItemList = {};

                        ItemList.SourceDocumentNo = $("#vmsrc_doc_no option:selected").text();
                        ItemList.SourceDocumentDate = $("#txtsrcdocdate").val();
                        ItemList.ItemId = row.find('#hdItemId').val();
                        ItemList.ItemName = row.find('#ItemName').val();
                        ItemList.subitem = row.find('#sub_item').val();
                        ItemList.UOMId = row.find('#hdUOMId').val();
                        ItemList.uom_name = row.find('#UOM').val();
                        ItemList.GRNNo = row.find("#GRNNumber").val();
                        var GRNDate = row.find("#GRNDate").val().trim();
                        var date = GRNDate.split("-");
                        var FDate = date[2] + '-' + date[1] + '-' + date[0];
                        ItemList.GRNDate = FDate;
                        ItemList.GRNQuantity = row.find("#GRNQuantity").val();
                        ItemList.ReturnQuantity = row.find("#ReturnQuantity").val();
                        ItemList.InvoiceValue = row.find("#InvoiceValue").val();
                        ItemList.ReturnValue = row.find("#ReturnValue").val();
                        ItemList.ReasonForReturn = row.find("#ReasonForReturn").val();
                        ItemList.ItemRemarks = row.find('#remarks').val();
                        ItemList.item_acc_id = IsNull(row.find('#hdn_item_gl_acc').val(), "");
                        ItemList.prt_avl = IsNull(row.find('#PrtAvlQuantity').val(), 0);
                        ItemList.item_rate = IsNull(row.find('#Item_cost').val(), 0);
                        ItemList.item_tax_amt = 0;
                        ItemList.wh_id = 0;
                        PurchaseReturnItemDetailList.push(ItemList);

                    });
                }
                var str = JSON.stringify(PurchaseReturnItemDetailList);
                $('#hdPurchaseReturnItemDetailList').val(str);
                if (src_type == "H") {
                    BindItemBatch_Detail();
                    BindItemSerial_Detail();
                }
                else {
                    BindItemBatchSerialDetail();
                }
                var TaxDetail = [];
                if (src_type == "H") {
                    TaxDetail = InsertTaxDetails();
                    var str_TaxDetail = JSON.stringify(TaxDetail);
                    $('#hdItemTaxDetail').val(str_TaxDetail);
                }

                BindGLVoucherDetail();
                /*-----------Sub-item-------------*/

                var SubItemsListArr = PRT_SubItemList(src_type);
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/
                var Suppname = $('#ddlSupplierNameList option:selected').text();
                $("#Hdn_PrtSuppName").val(Suppname);
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");

                var FinalCostCntrDetails = [];
                FinalCostCntrDetails = Cmn_InsertCCDetails();
                var CCDetails = JSON.stringify(FinalCostCntrDetails);
                $('#hdn_CC_DetailList').val(CCDetails);

                var FinalPOOCDetail = [];
                var FinalPOOCTaxDetail = [];

                FinalPOOCDetail = InsertPOOtherChargeDetails();
                FinalPOOCTaxDetail = InsertPO_OCTaxDetails();

                $("#hdOCDetailList").val(JSON.stringify(FinalPOOCDetail));
                $("#hdOCTaxDetailList").val(JSON.stringify(FinalPOOCTaxDetail));

                $('#src_type').attr("disabled", false);
                $('#AdHocBill_no').attr("disabled", false);
                $('#AdHocBill_dt').attr("disabled", false);

               // return false;
                return true;
            }
            else {
                return false;
            }
        }
        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function InsertPOOtherChargeDetails() {
    debugger;
    var PO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OC_TaxAmt = IsNull(currentRow.find("#OCTaxAmt").text(), 0);
            OC_TotlAmt = IsNull(currentRow.find("#OCTotalTaxAmt").text(), 0);
            PO_OCList.push({ OC_ID: OC_ID, OCValue: OCValue, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, OC_TaxAmt: OC_TaxAmt, OC_TotlAmt: OC_TotlAmt });
        });
    }
    return PO_OCList;
};
function InsertPO_OCTaxDetails() {/*Add by Hina Sharma on 09-07-2025*/
    debugger;


    var FTaxDetails = $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").length;
    var PO_OCTaxList = [];

    if (FTaxDetails != null) {
        if (FTaxDetails > 0) {

            $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").each(function () {
                var Crow = $(this);

                debugger;
                var ItemID = Crow.find("#TaxItmCode").text().trim();
                var TaxID = Crow.find("#TaxNameID").text().trim();
                var TaxName = Crow.find("#TaxName").text().trim();
                var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                var TaxLevel = Crow.find("#TaxLevel").text().trim();
                var TaxValue = Crow.find("#TaxAmount").text().trim();
                var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                PO_OCTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });


            });
        }
    }
    return PO_OCTaxList;
};
function onchangeAdHocBill_no() {
    document.getElementById("vmAdHocBill_no").innerHTML = "";
    $("#AdHocBill_no").css("border-color", "#ced4da");
}
function onchangeAdHocBill_dt() {
    document.getElementById("vmAdHocBill_dt").innerHTML = "";
    $("#AdHocBill_dt").css("border-color", "#ced4da");
}
function BindItemBatch_Detail() {
    debugger;
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#PD_BatchLotNo').val();
            batchList.ItemId = row.find('#PD_BatchItemId').val();
            var ItemId = row.find('#PD_BatchItemId').val();
            batchList.UOMId = row.find('#PD_BatchUOMId').val();
            batchList.BatchNo = row.find('#PD_BatchBatchNo').val();
           /* batchList.BatchResStock = row.find('#PD_BatchResAvlStk').val();*/
            batchList.BatchAvlStock = row.find('#PD_BatchBatchAvlStk').val();
            batchList.IssueQty = row.find('#PD_BatchIssueQty').val();
            var GLRow = $("#AdSReturnItmDetailsTbl > tbody >tr #hdItemId[value='" + ItemId + "']").closest('tr');
            var srNo = GLRow.find("#SNohiddenfiled").val();
            batchList.batchable = GLRow.find("#hfbatchable").val();
            batchList.Seriable = GLRow.find("#hfserialable").val();
            batchList.wh_id = GLRow.find("#wh_id" + srNo).val();

            var ExDate = row.find('#PD_BatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "") {
                FDate = "";
            }
            else {
                var date = ExDate.split("-");
                FDate = date[2] + '-' + date[1] + '-' + date[0];
            }
            batchList.ExpiryDate = FDate;
            batchList.mfg_name = row.find("#MfgName").val() || '';
            batchList.mfg_mrp = row.find("#MfgMrp").val() || '';
            batchList.mfg_date = row.find("#MfgDate").val() || '';
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }
}
function BindItemSerial_Detail() {
    debugger;
    var serialrowcount = $('#SaveItemSerialTbl tr').length;
    if (serialrowcount > 1) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            debugger;
            var ItemId = row.find("#PD_SerialItemId").val();
            SerialList.ItemId = row.find("#PD_SerialItemId").val();
            SerialList.UOMId = row.find("#PD_SerialUOMId").val();
            SerialList.LOTId = row.find("#PD_SerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#PD_SerialIssueQty").val();
            SerialList.SerialNO = row.find("#PD_BatchSerialNO").val().trim();
            var GLRow = $("#AdSReturnItmDetailsTbl > tbody >tr #hdItemId[value='" + ItemId + "']").closest('tr');
            var srNo = GLRow.find("#SNohiddenfiled").val();
            SerialList.batchable = GLRow.find("#hfbatchable").val();
            SerialList.Seriable = GLRow.find("#hfserialable").val();
            SerialList.wh_id = GLRow.find("#wh_id" + srNo).val();
            SerialList.mfg_name = row.find("#Serial_MfgName").val() || '';
            SerialList.mfg_mrp = row.find("#Serial_MfgMrp").val() || '';
            SerialList.mfg_date = row.find("#Serial_MfgDate").val() || '';
            ItemSerialList.push(SerialList);
        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);
    }
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ItmCode = currentRow.find("#hdItemId").val();
        if (FTaxDetails != null) {
            if (FTaxDetails > 0) {
                $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                    var currentRow = $(this);
                    var item_id = currentRow.find("#TaxItmCode").text();
                    var tax_id = currentRow.find("#TaxNameID").text();
                    var TaxName = currentRow.find("#TaxName").text().trim();
                    var tax_rate = currentRow.find("#TaxPercentage").text();
                    var tax_level = currentRow.find("#TaxLevel").text();
                    var tax_val = currentRow.find("#TaxAmount").text();
                    var tax_apply_on = currentRow.find("#TaxApplyOnID").text();
                    var tax_apply_onName = currentRow.find("#TaxApplyOn").text();
                    var totaltax_amt = currentRow.find("#TotalTaxAmount").text();
                    var tax_recov = currentRow.find("#TaxRecov").text();
                    TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt, totaltax_amt, tax_recov: tax_recov });
                });
            }

        }
    });
    return TaxDetails;
}
function CheckValidations_forAdHocSubItems() {
    return Cmn_CheckValidations_forSubItems("AdSReturnItmDetailsTbl", "", "hdItemId", "ReturnQuantity", "SubItemReturnQty", "Y");
}
function CheckItemBatch_Validation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {

        var clickedrow = $(this);
        var PackedQuantity = clickedrow.find("#ReturnQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var ItemType = clickedrow.find("#ItemType").val();
        //var Batchable = clickedrow.find("#hdi_batch").val();

        var TotalItemBatchQty = parseFloat("0");
        if (ItemType != "Service") {
            $("#SaveItemBatchTbl >tbody >tr").each(function () {

                var currentRow = $(this);
                var bchIssueQty = currentRow.find('#PD_BatchIssueQty').val();
                var bchitemid = currentRow.find('#PD_BatchItemId').val();
                var bchuomid = currentRow.find('#PD_BatchUOMId').val();
                //if (ItemId == bchitemid && UOMId == bchuomid) {
                if (ItemId == bchitemid) {
                    TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                }
            });
        }
        else {
            PackedQuantity = 0;
        }
        if (parseFloat(PackedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
        }
        else {
            ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
            BatchableFlag = "Y";
            EmptyFlag = "N";
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithpackedqty").text(), "warning");
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
//function OnclickTaxAmtDetailIcon(e) {//addby shubham maurya on 05-04-2025
//    debugger;
//    var CurrentRow = $(e.target).closest("tr");
//    var InvoiceNo = $("#hd_doc_no").val();
//    var ItmCode = $("#InvoiceItem_id").val();
//    var ShipNumber = $("#InvoiceIconGRNNumber").val();
//    var ReturnQuantity = $("#InvoiceIconReturnQuantity").val();
//    var TaxAmt = $("#TaxAmount").val();
//    var src_type = $('#src_type').val();
//    if (TaxAmt != null && TaxAmt != "") {
//        try {
//            $.ajax({
//                async: true,
//                type: "POST",
//                url: "/ApplicationLayer/PurchaseReturn/GetTaxAmountDetail",
//                data: {
//                    ItmCode: ItmCode,
//                    InvoiceNo: InvoiceNo,
//                    ShipNumber: ShipNumber,
//                    ReturnQuantity: ReturnQuantity,
//                    src_type: src_type
//                },
//                success: function (data) {
//                    debugger;
//                    $('#TaxAmountDetailsPopup').html(data);
//                },
//                error: function OnError(xhr, errorType, exception) {
//                    debugger;
//                }
//            });
//        }
//        catch (err) {
//            debugger;
//            console.log("Trial Balance Error : " + err.message);
//        }
//    }
//}
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var docno = $('#ddlDocumentNumber option:selected').text();
    var src_type = $('#src_type').val();
    $("#hd_doc_no").val(docno);

    var SuppID = $('#ddlSupplierNameList option:selected').val();
    if (SuppID != "0") {
        document.getElementById("vmsupp_id").innerHTML = "";

        $(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
        document.getElementById("vmsupp_id").innerHTML = null;
        $("[aria-labelledby='select2-ddlSupplierNameList-container']").css("border-color", "#ced4da");
        $('#src_type').attr("disabled", true);
    }
    else {
        document.getElementById("vmsupp_id").innerHTML = $("#valueReq").text();
        $('#ddlSupplierNameList').css("border-color", "red");
        $("[aria-labelledby='select2-ddlSupplierNameList-container']").css("border-color", "red");
    }

    if ($('#ddlDocumentNumber').val() != "0" && $('#ddlDocumentNumber').val() != "") {
        var text = $('#ddlDocumentNumber').val();
        $(".plus_icon1").css('display', 'none');
        
        $("#ddlSupplierNameList").prop("disabled", true);
        $("#ddlDocumentNumber").prop("disabled", true);
        var hdSelectedSourceDocument = null;
        var SourDocumentNo = $('#ddlDocumentNumber option:selected').text();
        hdSelectedSourceDocument = SourDocumentNo;
        $("#hdSelectedSourceDocument").val(hdSelectedSourceDocument);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/PurchaseReturn/GetPIItemDetail",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument,
                    src_type: src_type
                },
                success: function (data) {
                    debugger
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                if (i == 0) {
                                    $("#hdn_curr_id").val(arr.Table[i].curr_id);
                                    $("#Hdn_conv_rate").val(arr.Table[i].conv_rate);
                                    $("#hdn_bill_no").val(arr.Table[i].bill_no);
                                    $("#hdn_bill_dt").val(arr.Table[i].bill_dt);
                                }
                                var ReturnQty = "";
                                if (parseFloat(arr.Table[i].returnedqty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                    ReturnQty = "";
                                }
                                else {
                                    ReturnQty = parseFloat(arr.Table[i].returnedqty).toFixed(QtyDecDigit);
                                }
                                var Con_Disable = "";
                                var Item_Type = arr.Table[i].item_type;
                                if (Item_Type == "Consumable" || Item_Type == "Service") {
                                    Con_Disable = `<div class="col-sm-2 i_Icon">
                                                                          <button type="button" disabled style="filter: grayscale(100%)" id="ReturnQuantityDetail" class="calculator" onclick="OnClickReturnedQtyIconBtn(event);" data-toggle="modal" data-target="#ReturnQuantityDetail" disabled><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Title_ReturnQuantityDetail").text()}">  </button>
                                                                      </div>`
                                }
                                else {
                                    Con_Disable = `<div class="col-sm-2 i_Icon">
                                                                          <button type="button" id="ReturnQuantityDetail" class="calculator" onclick="OnClickReturnedQtyIconBtn(event);" data-toggle="modal" data-target="#ReturnQuantityDetail"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Title_ReturnQuantityDetail").text()}">  </button>
                                                                      </div>`
                                }
                                $('#PReturnItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">                                                       
                                                        <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                        <td class="sr_padding"><span id="SpanRowId">${i + 1}</span></td>                                                        
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class=" col-sm-11" style="padding:0px;">
                                                                <input id="ItemName" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder=""  disabled value='${arr.Table[i].item_name}'>
                                                                <input type="hidden" id="hdItemId" value='${arr.Table[i].item_id}' style="display: none;" />
                                                                <input type="hidden" id="hdn_item_gl_acc" value='${arr.Table[i].pur_ret_coa}' />
                                                                <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                <input hidden type="text" id="Item_Type" value="${Item_Type}" />
                                                                </div>
                                                            <div class="col-sm-1 i_Icon">
                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                       <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled value='${arr.Table[i].uom_name}'>
                                                            <input type="hidden" id="hdUOMId" value='${arr.Table[i].uom_id}' style="display: none;" />
                                                        </td>
   <td>
                                                                        <div class="col-sm-10 no-padding"><input id="AvailableStockInBase" class="form-control num_right" type="text" value='${arr.Table[i].br_avl_stk_bs}' name="AvailableStock" placeholder="0000.00" disabled=""></div>
                                                                        <div class="col-sm-2 i_Icon"><button type="button" id="StockDetail" class="calculator" onclick="ItemStockWareHouseWise(event,'P')" data-toggle="modal" data-target="#StockDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_StockDetail").text()}">  </button></div>                                                                       
                                                                    </td>
                                                        <td>
                                                            <input id="GRNNumber" value='${arr.Table[i].mr_no}'  class="form-control" autocomplete="" type="text" name="GRNNumber" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                        <input id="GRNDate" value='${arr.Table[i].mr_date}' class="form-control" autocomplete="" type="text" name="GRNDate" placeholder="0000.00"  disabled>
                                                    </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="GRNQuantity" value='${parseFloat(arr.Table[i].mr_qty).toFixed(QtyDecDigit)}' class="form-control num_right" autocomplete="" type="text" name="GRNQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemGRNQty" >
                                                            <button type="button" id="SubItemGRNQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                            </td>
<td>
                                                                        <input id="PrtAvlQuantity" value='${parseFloat(arr.Table[i].Returnable_qty).toFixed(QtyDecDigit)}' class="form-control num_right" autocomplete="" type="text" name="GRNQuantity" placeholder="0000.00" disabled>
                                                                    </td>
                                                        <td>
                                                                <input hidden type="text" id="sub_item" value='${arr.Table[i].sub_item}' />
                                                                <div class=" col-sm-10" style="padding:0px;"> <div class="lpo_form" style="padding:0px;">
                                                                    <input id="ReturnQuantity" value='${ReturnQty}' onkeypress="return OnKeyPressRetQty(this,event);" onchange="OnChangeRetQty(this,event)" class="form-control num_right" autocomplete="off" type="text" name="ReturnQuantity" placeholder="0000.00"  >
                                                                  <span id="ReturnQuantity_Error" class="error-message is-visible"></span>
                                                                    </div>
                                                                    </div>
                                                                    `+ Con_Disable+`
                                                                      </td>   
                                                                    <td >
                                                                                <input hidden="hidden" id="Item_cost" value="${arr.Table[i].item_cost}" />
                                                                                <input hidden="hidden" id="Item_Tax" value="${arr.Table[i].item_tax_amt}" />
                                                                            <div class="col-sm-10 lpo_form" style="padding:0px;" id=''>
                                                                                <input id="InvoiceValue" class="form-control num_right" autocomplete="off" type="text" name="InvoiceValue" onchange="OnChangeInvoiceValue();"  placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                                                            </div>
                                                                            <div class="col-sm-2 i_Icon">
                                                                                <button type="button" id="InvoiceValueCalculation" class="calculator" onclick="OnClickInvoiceValueIconBtn(event,${null});" data-toggle="modal" data-target="#InvoiceValueCalculation" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InvoiceValueCalculation").text()}"> </button>
                                                                            </div>
                                                                        </td>
                                                        <td>
                                                                <input id="ReturnValue" disabled class="form-control num_right" autocomplete="" type="text" onchange="OnChangeReturnValue();" name="ReturnValue" placeholder="0000.00"  >
                                                        </td>
                                                        <td>
                                                                 <input id="ReasonForReturn" maxlength="100" class="form-control" autocomplete="off" type="text" name="ReasonForReturn" placeholder="${$("#span_ReasonForReturn").text()}"  onblur="this.placeholder = '${$("#span_ReasonForReturn").text()}'">
                                                        </td>                                                        
                                                        <td>
                                                                <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "100" placeholder="${$("#span_remarks").text()}"    ></textarea>
                                                           
                                                        </td>
                                </tr>`);
                            }
                        }
                        if (arr.Table1.length > 0) {
                            var rowIdx = 0;
                            $('#hd_GL_DeatilTbl tbody tr').remove();
                            for (var i = 0; i < arr.Table1.length; i++) {

                                $('#hd_GL_DeatilTbl tbody').append(`<tr>
                                    <td id="compid">${arr.Table1[i].comp_id}</td>
                                    <td id="brid">${arr.Table1[i].br_id}</td>
                                    <td id="invno">${arr.Table1[i].inv_no}</td>
                                    <td id="invdt">${arr.Table1[i].inv_dt}</td>
                                    <td id="mrno">${arr.Table1[i].mr_no}</td>
                                    <td id="mrdt">${arr.Table1[i].mr_date}</td>
                                    <td id="itemid">${arr.Table1[i].item_id}</td>
                                    <td id="mrqty">${arr.Table1[i].mr_qty}</td>
                                    <td id="item_rate">${arr.Table1[i].item_rate}</td>
                                    <td id="id">${arr.Table1[i].id}</td>
                                    <td id="amt">${arr.Table1[i].amt}</td>
                                    <td id="totval">${arr.Table1[i].TotalValue}</td>
                                    <td id="type">${arr.Table1[i].Type}</td>
                                    <td id="Entity_id">${arr.Table1[i].Entity_id}</td>
                                    <td id="tax_rate">${arr.Table1[i].tax_rate}</td>
                                    <td id="GL_tax_level">${arr.Table1[i].tax_level}</td>
                                    </tr>`);
                            }
                        }
                    }
                    GetAllGLID();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                    //   alert("some error");
                }
            });
    }
    else {
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $('#ddlDocumentNumber').css("border-color", "red");
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }
}
function ItemStockWareHouseWise(evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        //var sno = "";
        var ItemId = "";
        var ItemName = "";
        var UOMName = "";
        var DocumentMenuId = $("#DocumentMenuId").val();

        var src_type = $('#src_type').val();
        if (src_type == "H") {
            var sno = clickedrow.find("#SNohiddenfiled").val();
            ItemId = clickedrow.find("#ItemName" + sno).val();
            ItemName = clickedrow.find("#ItemName" + sno + " option:selected").text();
        }
        else {
            ItemId = clickedrow.find("#hdItemId").val();
            ItemName = clickedrow.find("#ItemName").val();
        }
        UOMName = clickedrow.find("#UOM").val();
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: { ItemId: ItemId, UomId: null, DocumentMenuId: DocumentMenuId },
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
function OnddlDocumentNumberChange() {

    
    var DocumentNumber = $('#ddlDocumentNumber').val().trim();
    if (DocumentNumber != "0") {
        document.getElementById("vmDocument_id").innerHTML = "";
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");

        $("#txtsrcdocdate").val($('#ddlDocumentNumber').val());
    }
    else {
        $("#txtsrcdocdate").val(null);
        $("#hdSupplierId").val(SupplierId);
        document.getElementById("vmDocument_id").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }

}
function OnTextChangeBillNumber() {

    
    var BillNUmber = $('#BillNumber').val().trim();

    if (BillNUmber != "0") {
        document.getElementById("vmbill_no").innerHTML = "";
        $("#BillNumber").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_no").innerHTML = $("#valueReq").text();
        $("#BillNumber").css("border-color", "red");
    }

}
function OnTextChangeBillDate() {

    
    var BillDate = $('#txtbilldate').val().trim();


    if (BillDate != "0") {
        document.getElementById("vmbill_date").innerHTML = "";
        $("#txtbilldate").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
        $("#txtbilldate").css("border-color", "red");
    }

}
function BindSupplierList() {
    
    $("#ddlSupplierNameList").select2({
        ajax: {
            url: $("#hdSupplierList").val(),
            data: function (params) {
                var queryParameters = {
                    SupplierName: params.term // search term like "a" then "an"
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
function OnChangeSupp() {
    debugger;
    var SuppID = $('#ddlSupplierNameList option:selected').val();
    var src_type = $('#src_type').val();
    $("#hdsupp_id").val($("#ddlSupplierNameList").val());
    if (SuppID != "0") {
        document.getElementById("vmsupp_id").innerHTML = "";

        $(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
        $('#src_type').attr("disabled", true);
        if (src_type == "H") {
            $("#addbtn").css("display", "");
        }
    }
    else {
        $('#src_type').attr("disabled", false);
        if (src_type == "H") {
            $("#addbtn").css("display", "none");
        }
    }
    var Suppname = $('#ddlSupplierNameList option:selected').text();
    $("#Hdn_PrtSuppName").val(Suppname);
    
    $("#txtsrcdocdate").val("");
    BindDocumentNumberList();
    $('#ddlDocumentNumber').val("0").trigger('change.select2');
}
function BindDocumentNumberList() {
    var src_type = $('#src_type').val();
    var SuppID = $('#ddlSupplierNameList').val();
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/PurchaseReturn/GetPurchaseReturnSourceDocumentNoList",
        data: {
            DocumentNo: "",
            SupplierID: SuppID,
            Src_Type: src_type
        },
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.Table.length > 0) {
                $("#ddlDocumentNumber option").remove();
                $("#ddlDocumentNumber optgroup").remove();
                $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                for (var i = 0; i < arr.Table.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr.Table[i].inv_dt}" value='${arr.Table[i].inv_dt}' data-billNumber="${arr.Table[i].bill_no}"data-billdt="${arr.Table[i].bill_dt}">${arr.Table[i].inv_no}</option>`);
                }
                var firstEmptySelect = true;
                $('#ddlDocumentNumber').select2({
                    templateResult: function (data) {
                        var DocDate = $(data.element).data('date');
                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div class="row">' +
                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                            '</div>'
                        );
                        return $result;
                        firstEmptySelect = false;
                    }
                });
                if (arr.Table1.length > 0) {
                    $("#supp_acc_id").val(arr.Table1[0].supp_acc_id);
                }
                if (arr.Table2.length > 0) {
                    $("#hdn_curr_id").val(arr.Table2[0].curr_id);
                    $("#Hdn_conv_rate").val(arr.Table2[0].conv_rate);
                    $("#ship_add_gstNo").val(arr.Table2[0].supp_gst_no);
                    $("#Ship_StateCode").val(arr.Table2[0].bill_state_code);
                }
            }

        },
    })
    //$("#ddlDocumentNumber").select2({
    //    ajax: {
    //        url: $("#hdsrc_doc_no").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                DocumentNo: params.term, // search term like "a" then "an"
    //                SupplierID: SuppID
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        //type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //                          
    //            arr = data;//JSON.parse(data);
    //            if (arr.length > 0) {
    //                $("#ddlDocumentNumber option").remove();
    //                $("#ddlDocumentNumber optgroup").remove();
    //                $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
    //                for (var i = 0; i < arr.length; i++) {
    //                    $('#Textddl').append(`<option data-date="${arr[i].ID}" value='${arr[i].ID}'>${arr[i].Name}</option>`);
    //                }
    //                var firstEmptySelect = true;
    //                $('#ddlDocumentNumber').select2({
    //                    templateResult: function (data) {
    //                        var DocDate = $(data.element).data('date');
    //                        var classAttr = $(data.element).attr('class');
    //                        var hasClass = typeof classAttr != 'undefined';
    //                        classAttr = hasClass ? ' ' + classAttr : '';
    //                        var $result = $(
    //                            '<div class="row">' +
    //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
    //                            '</div>'
    //                        );
    //                        return $result;
    //                        firstEmptySelect = false;
    //                    }
    //                });
    //            }

    //        }
    //    },

    //});
}
function OnClickIconBtn(e) {
    
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    ItemInfoBtnClick(ItmCode);
}
function OnClickReturnedQtyIconBtn(e) {
    
    var ValDecDigit = $("#ValDigit").text();

    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var SubItem = clickedrow.find("#sub_item").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMID = clickedrow.find("#hdUOMId").val();
    var GRNNumber = clickedrow.find("#GRNNumber").val();
    var GRNDate = clickedrow.find("#GRNDate").val();
    var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
    var sub_item = clickedrow.find("#sub_item").val();

    $("#IconItemName").val(ItemName);
    $("#Iconsub_item").val(SubItem);
    $("#IconUOM").val(UOM);
    $("#IconGRNNumber").val(GRNNumber);
    $("#IconGRNDate").val(GRNDate);
    $("#IconReturnQuantity").val(ReturnQuantity);
    $("#RQhdItemId").val(ItmCode);
    $("#RQhdUomId").val(UOMID);

    AddItemReturnedQtyDetail(ItmCode, GRNNumber, sub_item);

}
function AddItemReturnedQtyDetail(ItmID, GRNNumber, sub_item) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ItemID = ItmID;
    var DocumentNumber = $('#ReturnNumber').val();
    //var UomID = ItemuomID;
    var RT_Status = $('#hdPRTStatus').val();
    var src_type = $('#src_type').val();
    if (ItemID != "" && ItemID != null && GRNNumber != "" && GRNNumber != null) {
        
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/PurchaseReturn/GetGRNItemDetail",
                data: {
                    ItemID: ItemID, GRNNumber: GRNNumber, SrcDocNumber: DocumentNumber, RT_Status: RT_Status, src_type: src_type
                },
                success: function (data) {
                    
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#ReturnedQtyDetailsTbl tbody tr').remove();
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                //var subitmDisable = "";
                                //if (arr.Table[i].sub_item != "Y") {
                                //    subitmDisable = "disabled";
                                //}
                                var item_id = arr.Table[i].item_id;
                                var whid = arr.Table[i].wh_id;
                                var whtype = arr.Table[i].WhType;
                                var lot_id = arr.Table[i].lot_id;
                                var batch_no = arr.Table[i].batch_no;
                                var serial_no = arr.Table[i].serial_no;
                                var GRNNumber = arr.Table[i].mr_no;
                                var mfg_name = arr.Table[i].mfg_name;
                                var mfg_mrp = arr.Table[i].mfg_mrp;
                                var mfg_date = arr.Table[i].mfg_date;
                                
                                if (batch_no === null || batch_no === "null") {
                                    batch_no = "";
                                }
                                if (serial_no === null || serial_no === "null") {
                                    serial_no = "";
                                }
                                var TotalQty;
                                var returnqty = "";
                                var TotalRetQty = "";
                                var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
                                if (FReturnItemDetails != null && FReturnItemDetails != "") {
                                    if (FReturnItemDetails.length > 0) {
                                        for (j = 0; j < FReturnItemDetails.length; j++) {
                                            
                                            var ItemID = FReturnItemDetails[j].ItmCode;
                                            var ItmUomId = FReturnItemDetails[j].ItmUomId;
                                            var wh_id = FReturnItemDetails[j].wh_id;
                                            var Lot = FReturnItemDetails[j].Lot;
                                            var Batch = FReturnItemDetails[j].Batch;
                                            var Serial = FReturnItemDetails[j].Serial;
                                            var RetQty = FReturnItemDetails[j].RetQty;
                                            var IconGRNNumber = FReturnItemDetails[j].IconGRNNumber;

                                            var Batchable = FReturnItemDetails[j].Batchable;
                                            var Serialble = FReturnItemDetails[j].Serialable;
                                            if (Batch === null || Batch === "null") {
                                                Batch = "";
                                            }
                                            if (Serial === null || Serial === "null") {
                                                Serial = "";
                                            }
                                            if (IconGRNNumber == GRNNumber, ItemID == item_id && wh_id == whid && Lot == lot_id && Batch == batch_no && Serial == serial_no) {
                                                returnqty = RetQty;
                                                //TotalRetQty = TotalQty;

                                                var TotalQty = FReturnItemDetails[j].TotalQty;
                                            }
                                        }
                                    }
                                }
                                if (TotalQty == "" || TotalQty == null) {
                                    TotalQty = "0";
                                }

                                TotalRetQty = TotalQty;

                                ++rowIdx;
                                
                                if (AvoidDot(returnqty) == false) {
                                    returnqty = 0;
                                }
                                var iConReturnQty = "";
                                if (parseFloat(returnqty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || returnqty == "" || returnqty == null) {
                                    iConReturnQty = "";
                                }
                                else {
                                    iConReturnQty = parseFloat(returnqty).toFixed(QtyDecDigit);
                                }
                                $('#ReturnedQtyDetailsTbl tbody').append(` <tr id="${rowIdx}">
                    <td>
                     <input id="Warehouse${rowIdx}" value='${arr.Table[i].wh_name}'  class="form-control" autocomplete="off" type="text" name="Warehouse${rowIdx}"  placeholder="${$("#span_Warehouse").text()}"  onblur="this.placeholder='${$("#span_Warehouse").text()}'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value='${rowIdx}' />                    
                     <input type="hidden" id="Item_id" value='${arr.Table[i].item_id}' />
                     <input type="hidden" id="hduom_id" value='${arr.Table[i].uom_id}' />
                     <input type="hidden" id="wh_id${rowIdx}" value='${arr.Table[i].wh_id}' />
                     <input type="hidden" id="wh_type${rowIdx}" value='${arr.Table[i].WhType}' />
                     <input type="hidden" id="Batchable" value='${arr.Table[i].i_batch}' />
                     <input type="hidden" id="Serialable" value='${arr.Table[i].i_serial}' />
                                </td>
                                 <td>
                                <input id="Lot${rowIdx}" value="${arr.Table[i].lot_id}" class="form-control" autocomplete="off" type="text" disabled>
                                </td>

                                 <td><input id="Batch1${rowIdx}" value="${arr.Table[i].batch_no}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${rowIdx}"  value="${arr.Table[i].serial_no}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                  </td>
                                    <td>
                                <input id="MfgName${rowIdx}"  value="${IsNull(arr.Table[i].mfg_name, '')}" class="form-control" autocomplete="off" type="text" name="" disabled>
                                  </td>
                                    <td>
                                <input id="MfgMrp${rowIdx}"  value="${IsNull(arr.Table[i].mfg_mrp, '')}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                  </td>
                                    <td>
                                <input id=""  value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                <input id="MfgDate${rowIdx}"  value="${IsNull(arr.Table[i].mfg_date, '')}" hidden type="text">
                                  </td>
                                <td>
                                <input id="AvailableQuantity${rowIdx}" value="${parseFloat(arr.Table[i].batch_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="1"  onblur="this.placeholder = '1'" disabled>
                                </td>
                                 <td>
                                    <div class="lpo_form">
                                    <input id="IconReturnQtyinput${rowIdx}" value="${iConReturnQty}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressRetQty(this,event);" onchange="OnChangePopupReturnQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanRetQty${rowIdx}" class="error-message is-visible"></span></div>
                                    </td>
                </tr>`);

                                DisableDetail(RT_Status);


                            }
                            if (TotalRetQty == "") {
                                $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                            }
                            else {
                                $("#TotalReturnQty").text(parseFloat(TotalRetQty).toFixed(QtyDecDigit));
                            }
                            debugger;
                            debugger;
                            if (sub_item == "N") {
                                $("#SubItemReturnQuantity_Detail").attr("disabled", true);
                                $("#SubItemReturnQuantity_Detail").css("filter", "grayscale(100%)");
                            }
                            else {
                                $("#SubItemReturnQuantity_Detail").attr("disabled", false);
                                //$("#SubItemReturnQuantity_Detail").css('border', '1px solid #ced4da');
                                //$("#SubItemReturnQuantity_Detail").css("filter", "#ced4da");
                                $("#SubItemReturnQuantity_Detail").css("filter", "");
                            }
                        }
                        else {
                            $("#ReturnedQtyDetailsTbl >tbody >tr").remove();
                            $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                        }

                    }
                    else {
                        $("#ReturnedQtyDetailsTbl >tbody >tr").remove();
                        $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                    //   alert("some error");
                }
            });
    } else {

    }

}
function DisableDetail(RT_Status) {
    
    var PageDisable = $("#PageDisable").val();
    //var sessionval = sessionStorage.getItem("EditBtnClickPR");
    //if (RT_Status == "") {
    //    sessionval = "Y";
    //}

    if (RT_Status == "D" || RT_Status == "") {
        $('#ReturnedQtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            if (PageDisable == "N") {
                currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", false);
            }
            else {
                currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", true);
            }
        });

        if (PageDisable == "N") {

            $("#btnRqSaveDiscardOnly").css("display", "");
            $("#btnRqCloseOnly").css("display", "none");

            $("#btnIvSaveDiscardOnly").css("display", "");
            $("#btnIvCloseOnly").css("display", "none");

            $("#SaveAndExitBtn").attr("disabled", false);
            $("#DiscardAndExit").attr("disabled", false);
            //$("#InvoicePopupReturnValue").attr("disabled", false);
            $("#InvoiceSaveAndExitBtn").attr("disabled", false);
            $("#InvoiceDiscardAndExit").attr("disabled", false);
        }
        else {
            $("#btnRqSaveDiscardOnly").css("display", "none");
            $("#btnRqCloseOnly").css("display", "");

            $("#btnIvSaveDiscardOnly").css("display", "none");
            $("#btnIvCloseOnly").css("display", "");

            $("#SaveAndExitBtn").attr("disabled", true);
            $("#DiscardAndExit").attr("disabled", true);
            $("#InvoicePopupReturnValue").attr("disabled", true);
            $("#InvoiceSaveAndExitBtn").attr("disabled", true);
            $("#InvoiceDiscardAndExit").attr("disabled", true);
        }
    }
    else {
        $('#ReturnedQtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", true);
        });

        $("#btnRqSaveDiscardOnly").css("display", "none");
        $("#btnRqCloseOnly").css("display", "");

        $("#btnIvSaveDiscardOnly").css("display", "none");
        $("#btnIvCloseOnly").css("display", "");

        $("#btnSaveDiscardOnly").css("display", "none");
        $("#btnCloseOnly").css("display", "");
        $("#SaveAndExitBtn").attr("disabled", true);
        $("#DiscardAndExit").attr("disabled", true);
        $("#InvoicePopupReturnValue").attr("disabled", true);
        $("#InvoiceSaveAndExitBtn").attr("disabled", true);
        $("#InvoiceDiscardAndExit").attr("disabled", true);
    }
}
function OnChangePopupReturnQty(el, evt) {
    
    var clickedrow = $(evt.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var RetQty = clickedrow.find("#IconReturnQtyinput" + Sno).val();
    var Available = parseFloat(clickedrow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
    if ((RetQty == "") || (RetQty == null) || (RetQty == 0) || (RetQty == "NaN")) {
        RetQty = 0;
    }
    if (parseFloat(RetQty) <= parseFloat(Available)) {

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);
        clickedrow.find("#SpanRetQty" + Sno).css("display", "none");
        clickedrow.find("#IconReturnQtyinput" + Sno).css("border-color", "#ced4da");

    }
    else {
        clickedrow.find("#SpanRetQty" + Sno).text($("#ExceedingQty").text());
        clickedrow.find("#SpanRetQty" + Sno).css("display", "block");
        clickedrow.find("#IconReturnQtyinput" + Sno).css("border-color", "red");
        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);

    }

    CalTotalReturnQty(el, evt);
}
function CalTotalReturnQty(el, evt) {
    

    var QtyDecDigit = $("#QtyDigit").text();
    var TotalQty = parseFloat(0).toFixed(QtyDecDigit);

    $("#ReturnedQtyDetailsTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        var Sno = CRow.find("#SNohiddenfiled").val();
        var RtnQty = CRow.find("#IconReturnQtyinput" + Sno).val();
        if (RtnQty != "" && RtnQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(RtnQty));
        }
    });

    $("#TotalReturnQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));

};
function OnClickInvoiceValueIconBtn(evt, vChange) {
    
    var ValDecDigit = $("#ValDigit").text();
    var RT_Status = $('#hdPRTStatus').val();
    var clickedrow = $(evt.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var GRNNumber = clickedrow.find("#GRNNumber").val();
    var GRNDate = clickedrow.find("#GRNDate").val();
    var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
    var InvoiceNo = $('#ddlDocumentNumber option:selected').text();

    $("#SpanReturnValuePopup").css("display", "none");
    $("#InvoicePopupReturnValue").css("border-color", "#ced4da");

    $("#hd_doc_no").val(InvoiceNo);
    $("#InvoiceIconItemName").val(ItemName);
    $("#InvoiceItem_id").val(ItmCode)
    $("#InvoiceIconUOM").val(UOM);
    $("#InvoiceIconGRNNumber").val(GRNNumber);
    $("#InvoiceIconGRNDate").val(GRNDate);
    $("#InvoiceIconReturnQuantity").val(ReturnQuantity);

    GetInvoiceValueCalculationDetail(evt, ItmCode, InvoiceNo, GRNNumber, ReturnQuantity, vChange);
    //DisableDetail(RT_Status);
}
function GetInvoiceValueCalculationDetail(evt, ItmCode, InvoiceNo, GRNNumber, ReturnQuantity, vChange) {
    debugger;
    debugger;
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount   
    var clickedrow = $(evt.target).closest("tr");
    var ItemID = ItmCode;
    var src_type = $('#src_type').val();
    //var ReturnQuantity = clickedrow.find('#ReturnQuantity').val();
    if (ItemID != "" && ItemID != null && GRNNumber != "" && GRNNumber != null) {
        
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/PurchaseReturn/GetInvoiceItemDetail",
                data: {
                    ItemID: ItemID, InvoiceNo: InvoiceNo, GRNNumber: GRNNumber, src_type: src_type
                },
                success: function (data) {
                    
                    var RT_Status = $('#hdPRTStatus').val();
                    DisableDetail(RT_Status);
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                ++rowIdx;
                                
                                //var landedOC;
                                var landedtax=0;
                                var mrqty = arr.Table[i].mr_qty;
                                var itemrate = arr.Table[i].item_rate;

                                if (itemrate == null) {
                                    $("#ItemCost").val(parseFloat(0).toFixed(RateDecDigit));
                                    itemrate = 0;
                                }
                                else {
                                   

                                    var rate = parseFloat(itemrate).toFixed(RateDecDigit);
                                    $("#ItemCost").val(rate);

                                }
                                var landedRate = parseFloat(ReturnQuantity) * parseFloat(rate);
                                /*****-------------------------------------------------------------------*****/
                                //for (var p = 0; p < arr.Table1.length; p++) {
                                //    if (arr.Table1[p].tax_rate == null) {
                                //        $("#TaxAmount").val(parseFloat(0).toFixed(ValDecDigit));
                                //        landedtax = 0;
                                //    }
                                //    else {
                                //        landedtax = (parseFloat(landedtax) + (parseFloat(landedRate) * parseFloat(arr.Table1[p].tax_rate) / 100));
                                //        $("#TaxAmount").val(parseFloat(landedtax / ReturnQuantity).toFixed(ValDecDigit));
                                //    }
                                //}
                                /*****-------------------------------------------------------------------*****/
                                $("#OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                                var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
                                var NewArray = [];
                                arr.Table1.sort(function (a, b) {
                                    return a.tax_level - b.tax_level;
                                });
                                for (var p = 0; p < arr.Table1.length; p++) {
                                    var TaxPec = arr.Table1[p].tax_rate;
                                    if (arr.Table1[p].tax_apply_on == "I") {
                                        if (arr.Table1[p].tax_level == "1") {
                                            TaxAmount = ((parseFloat(landedRate) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                                        }
                                        else {
                                            var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                                            var TaxLevelTbl = parseInt(arr.Table1[p].tax_level) - 1;
                                            for (j = 0; j < NewArray.length; j++) {
                                                var Level = NewArray[j].TaxLevel;
                                                var TaxAmtLW = NewArray[j].TaxAmount;
                                                if (TaxLevelTbl == Level) {
                                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                                }
                                            }
                                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                                        }
                                        NewArray.push({
                                            TaxItmCode: ItemID, TaxNameID: arr.Table1[p].tax_id,
                                            TaxPercentage: arr.Table1[p].tax_rate, TaxLevel: arr.Table1[p].tax_level, TaxApplyOn: arr.Table1[p].tax_apply_on,
                                            TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt
                                        });
                                    }
                                    if (arr.Table1[p].tax_apply_on == "C") {
                                        var Level = arr.Table1[p].tax_level;
                                        if (arr.Table1[p].tax_level == "1") {
                                            TaxAmount = ((parseFloat(landedRate) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                                        }
                                        else {
                                            var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                                            for (j = 0; j < NewArray.length; j++) {
                                                var Level = NewArray[j].TaxLevel;
                                                var TaxAmtLW = NewArray[j].TaxAmount;
                                                if (arr.Table1[p].tax_level != Level) {
                                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                                }
                                            }
                                            var FinalAssAmt = (parseFloat(landedRate) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                                        }
                                        NewArray.push({
                                            TaxItmCode: ItemID, TaxNameID: arr.Table1[p].tax_id,
                                            TaxPercentage: arr.Table1[p].tax_rate, TaxLevel: arr.Table1[p].tax_level, TaxApplyOn: arr.Table1[p].tax_apply_on,
                                            TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt
                                        });
                                    }
                                   
                                    landedtax = parseFloat(landedtax) + parseFloat(TaxAmount);
                                    $("#TaxAmount").val(parseFloat(parseFloat(landedtax) / parseFloat(ReturnQuantity)).toFixed(ValDecDigit));
                                }
                                

                                /*****-------------------------------------------------------------------*****/
                                //if (arr.Table[i].item_tax_amt == null) {
                                //    $("#TaxAmount").val(parseFloat(0).toFixed(ValDecDigit));
                                //    landedtax = 0;
                                //}
                                //else {
                                //    var totaltaxamt = parseFloat(arr.Table[i].item_tax_amt).toFixed(ValDecDigit);

                                //    landedtax = parseFloat((totaltaxamt / mrqty)).toFixed(ValDecDigit);
                                //    $("#TaxAmount").val(parseFloat(landedtax).toFixed(ValDecDigit));
                                //}
                                /*****-------------------------------------------------------------------*****/

                                //if (arr.Table[i].item_oc_amt == null) {
                                //    $("#OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                                //    landedOC = 0;
                                //}
                                //else {
                                //    var totalOc = parseFloat(arr.Table[i].item_oc_amt).toFixed(ValDecDigit);
                                //    landedOC = 0;//(totalOc / mrqty)
                                //    $("#OtherCharges").val(parseFloat(landedOC).toFixed(ValDecDigit));
                                //}

                                //var landedrate = parseFloat(landedRate);// + landedOC;
                                //var landedrate = itemrate + parseFloat(landedtax);// + landedOC;
                                //var invoiceAmt = parseFloat((ReturnQuantity * landedrate)).toFixed(ValDecDigit);
                                var invoiceAmt = parseFloat(parseFloat(landedtax) + landedRate).toFixed(ValDecDigit);
                                //$("#NetRate").val(parseFloat(landedrate).toFixed(ValDecDigit));
                                var lanNetRate = (parseFloat(parseFloat(landedtax) / ReturnQuantity)).toFixed(ValDecDigit);
                                $("#NetRate").val(parseFloat(parseFloat(rate) + parseFloat(lanNetRate)).toFixed(ValDecDigit));
                                $("#InvoicePopupInvoiceAmount").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                if (clickedrow.find("#ReturnValue").val() != null && clickedrow.find("#ReturnValue").val() != "") {
                                    $("#InvoicePopupReturnValue").val(parseFloat(clickedrow.find("#ReturnValue").val()).toFixed(ValDecDigit));
                                }
                                else {
                                    $("#InvoicePopupReturnValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                }
                                if (vChange === "change") {
                                    clickedrow.find("#ReturnValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                    clickedrow.find("#InvoiceValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                    OnChangeInvoiceValue();
                                    OnChangeReturnValue();
                                    var ItemCode = clickedrow.find("#hdItemId").val();
                                    var GrnNo = clickedrow.find("#GRNNumber").val();
                                    UpdateGLValue(ItemCode, GrnNo, ReturnQuantity, rate, arr.Table1)
                                }

                                
                            }

                        }

                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                    //   alert("some error");
                }
            });
    } else {

    }
}
function CheckItemLevelValidations(Src) {
    
    var ErrorFlag = "N";
    var src_type = $('#src_type').val();
    if (src_type == "H") {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItemName" + Sno).val() == "0") {
                if (Src != "FromGL") {
                    currentRow.find("#ItemNameError").text($("#valueReq").text());
                    currentRow.find("#ItemNameError").css("display", "block");
                    currentRow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ItemNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            var ItemType = currentRow.find("#ItemType").val();
            if (ItemType != "Service") {
                if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
                    if (Src != "FromGL") {
                        currentRow.find("[aria-labelledby='select2-wh_id" + Sno + "-container']").css("border-color", "red");
                        currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
                        currentRow.find("#wh_Error" + Sno).css("display", "block");
                    }
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("[aria-labelledby='select2-wh_id" + Sno + "-container']").css("border-color", "#ced4da");
                    currentRow.find("#wh_Error" + Sno).text("");
                    currentRow.find("#wh_Error" + Sno).css("display", "none");
                }
            }  
            if (currentRow.find("#ReturnQuantity").val() == "" || parseFloat(currentRow.find("#ReturnQuantity").val()) == parseFloat("0")) {
                if (Src != "FromGL") {
                    currentRow.find("#ReturnQuantity_Error").text($("#valueReq").text());
                    currentRow.find("#ReturnQuantity_Error").css("display", "block");
                    currentRow.find("#ReturnQuantity").css("border-color", "red");
                }
                //$("#ReturnQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReturnQuantity_Error").css("display", "none");
                currentRow.find("#ReturnQuantity").css("border-color", "#ced4da");
            }
            //if (currentRow.find("#Price").val() == "" || parseFloat(currentRow.find("#Price").val()) == parseFloat("0")) {
            //    if (Src != "FromGL") {
            //        currentRow.find("#Price_Error").text($("#valueReq").text());
            //        currentRow.find("#Price_Error").css("display", "block");
            //        currentRow.find("#Price").css("border-color", "red");        
            //    }
            //    ErrorFlag = "Y";
            //}
            //else {
            //    currentRow.find("#Price_Error").css("display", "none");
            //    currentRow.find("#Price").css("border-color", "#ced4da");
            //}
        });
    }
    else {
        $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            if (currentRow.find("#ReturnQuantity").val() == "" || parseFloat(currentRow.find("#ReturnQuantity").val()) == parseFloat("0")) {
                if (Src != "FromGL") {
                    currentRow.find("#ReturnQuantity_Error").text($("#valueReq").text());
                    currentRow.find("#ReturnQuantity_Error").css("display", "block");
                    currentRow.find("#ReturnQuantity").css("border-color", "red");
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReturnQuantity_Error").css("display", "none");
                currentRow.find("#ReturnQuantity").css("border-color", "#ced4da");
            }
        });
    }  
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatchValidation() {
    
    var ErrorFlag = "N";
    //var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
        
        var clickedrow = $(this);
        var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var GRNNumberCheck = clickedrow.find("#GRNNumber").val();
        var TotalItemBatchQty = parseFloat("0");
        var Item_Type = clickedrow.find("#Item_Type").val()
        if (Item_Type == "Stockable") {
            let NewArr = [];
            var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
            if (FReturnItemDetails != null && FReturnItemDetails != "") {
                if (FReturnItemDetails.length > 0) {
                    for (i = 0; i < FReturnItemDetails.length; i++) {

                        var ItemID = FReturnItemDetails[i].ItmCode;
                        var GRNNumber = FReturnItemDetails[i].IconGRNNumber;

                        if (ItemID == ItemId && GRNNumber == GRNNumberCheck) {
                            TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(FReturnItemDetails[i].RetQty);
                        }
                    }
                }
            }
            else {
                $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
                    var clickedrow1 = $(this);
                    clickedrow1.find('#ReturnQuantityDetail').css('border', '1px solid red');
                });
            }
            if (parseFloat(ReturnQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            }
            else {
                EmptyFlag = "Y";
                clickedrow.find('#ReturnQuantityDetail').css('border', '1px solid red');
            }
        }
    });
    if (EmptyFlag == "Y") {
        swal("", $("#BatchSerialqtydoesnotmatchwithreturnqty").text(), "warning");
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
function Check_VoucherValidations() {

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 22-07-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 22-07-2024 to add new common gl validations*/
    //var ErrorFlag = "N";
    //var ValDigit = $("#ValDigit").text();
    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {
        
    //    var currentRow = $(this);
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#Acc_name_' + rowid;
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //    }
    //    else {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //        return false;
    //    }

    //});
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
}
function ddlDocumentNumberSelect() {
    debugger;
    var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();
    var date = SourceDocumentDate.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];

    $("#txtsrcdocdate").val(FDate);

    $('#ddlDocumentNumber').on('change', function () {
        var selectedOption = $(this).find('option:selected');
        var billNumber = selectedOption.data('billnumber'); // gets data-billnumber
        var billDate = selectedOption.data('billdt');       // gets data-billdt

        $('#AdHocBill_no').val(billNumber);
        $('#AdHocBill_dt').val(billDate);
    });

    var DocumentNumber = $('#ddlDocumentNumber').val();
    if (DocumentNumber != "0") {
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
    }
    //else {
    //    document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
    //    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    //}

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}

function OnKeyPressRetQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#ReturnQuantity_Error").css("display", "none");
    clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");


    return true;
}
function OnChangeRetQty(el, evt, Flag) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    if (Flag == "forPageLoad") {
        var clickedrow = el;
    }
    else {
        var clickedrow = $(evt.target).closest("tr");
    }

    var Rqtydt = clickedrow.find("#ReturnQuantity").val();
    if (AvoidDot(Rqtydt) == false) {
        Rqtydt = 0;
    }
    var ReturnQuantity = parseFloat(Rqtydt).toFixed(parseFloat(QtyDecDigit));
    var GRNQuantity = parseFloat(clickedrow.find("#GRNQuantity").val()).toFixed(parseFloat(QtyDecDigit));
    var item_type = clickedrow.find("#Item_Type").val();
    if ($("#chk_roundoff").is(":checked")) {
        $("#chk_roundoff").parent().find(".switchery").trigger("click");
    }

    if (parseFloat(ReturnQuantity) <= parseFloat(GRNQuantity)) {

        if ((ReturnQuantity == "") || (ReturnQuantity == null) || (ReturnQuantity == 0)) {
            var test = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#ReturnQuantity").val("");
            clickedrow.find("#ReturnQuantityDetail").attr("disabled", true);
            clickedrow.find("#ReturnQuantity_Error").text($("#valueReq").text());
            clickedrow.find("#ReturnQuantity_Error").css("display", "block");
            clickedrow.find("#ReturnQuantity").css("border-color", "red");
            OnClickInvoiceValueIconBtn(evt, "change");
        }
        else {
            var test = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#ReturnQuantity").val(test);
            if (item_type == "Stockable") {
                clickedrow.find("#ReturnQuantityDetail").attr("disabled", false);
            }
            //clickedrow.find("#ReturnQuantityDetail").attr("disabled", false);
            clickedrow.find("#ReturnQuantity_Error").css("display", "none");
            clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");
            OnClickInvoiceValueIconBtn(evt, "change");
            //OnChangeInvoiceValue();
            //var ItemCode= clickedrow.find("#hdItemId").val();
            //var GrnNo= clickedrow.find("#GRNNumber").val();
            //UpdateGLValue(ItemCode, GrnNo, ReturnQuantity)
        }
    }
    else {
        clickedrow.find("#ReturnQuantity_Error").text($("#ExceedingQty").text());
        clickedrow.find("#ReturnQuantity_Error").css("display", "block");
        clickedrow.find("#ReturnQuantity").css("border-color", "red");

        var test = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#ReturnQuantity").val(test);
        clickedrow.find("#ReturnQuantityDetail").attr("disabled", true);
    }
    //click_chkplusminus();
}
function OnChangeRetValue(el, evt) {
    
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(evt.target).closest("tr");
    var ReturnValue = parseFloat($("#InvoicePopupReturnValue").val()).toFixed(parseFloat(ValDecDigit));

    if ((ReturnValue == "NaN") || (ReturnValue == null) || (ReturnValue == 0)) {
        var test = parseFloat(parseFloat("0")).toFixed(parseFloat(ValDecDigit));
        $("#InvoicePopupReturnValue").val(test);
        $("#SpanReturnValuePopup").text($("#valueReq").text());
        $("#SpanReturnValuePopup").css("display", "block");
        $("#InvoicePopupReturnValue").css("border-color", "red");
    }
    else {
        var test = parseFloat(parseFloat(ReturnValue)).toFixed(parseFloat(ValDecDigit));
        $("#InvoicePopupReturnValue").val(test);
        $("#SpanReturnValuePopup").css("display", "none");
        $("#InvoicePopupReturnValue").css("border-color", "#ced4da");
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
function OnClickSupplierInfoIconBtn(e) {
    
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    ItmCode = clickedrow.find("#hdItemId").val();
    var Supp_id = $('#ddlSupplierNameList').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id)

}

function OnClickSaveAndExitRetQty_Btn() {
    
    var QtyDecDigit = $("#QtyDigit").text();///Quantity     
    var IconGRNNumber = $("#IconGRNNumber").val();

    var IGRNDate = $("#IconGRNDate").val().trim();
    var date = IGRNDate.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    var IconGRNDate = FDate;
    var IconReturnQuantity = $("#IconReturnQuantity").val();
    var ItmCode = $("#RQhdItemId").val();
    var ItmUomId = $("#RQhdUomId").val();
    var Batchable = $("#Batchable").val();
    var Serialable = $("#Serialable").val();
    var TotalQty = $("#TotalReturnQty").text();

    var error = "N";
    $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
        if (RetQty == "" || RetQty == null || RetQty == 0 || RetQty == "NaN") {
            RetQty = 0;
        }
        var Available = parseFloat(currentRow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
        if (parseFloat(RetQty) <= parseFloat(Available)) {
            var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#IconReturnQtyinput" + Sno).val(test);

            $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
                var clickedrow1 = $(this);
                var itm = clickedrow1.find('#Item_id').val();
                if (itm == null) {//add by shubham maurya on 29-04-2025 for show undifiend item 
                    itm = clickedrow1.find('#hdItemId').val();
                }   
                if (ItmCode == itm) {
                    clickedrow1.find('#ReturnQuantityDetail').css('border', '1px solid #ced4da');
                }
            });
        }
        else {
            currentRow.find("#SpanRetQty" + Sno).text($("#ExceedingQty").text());
            currentRow.find("#SpanRetQty" + Sno).css("display", "block");
            currentRow.find("#IconReturnQtyinput" + Sno).css("border-color", "red");
            var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#IconReturnQtyinput" + Sno).val(test);
            error = "Y";
            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
        }


    });
    if (error == "Y") {
        $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        if (parseFloat(IconReturnQuantity) == parseFloat(TotalQty)) {

            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "modal");
            //ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", ItmCode);
            if (ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", ItmCode, IconGRNNumber) == false) {
                error = "Y";
                $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
                swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                //return false
            }
            else {
                $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "modal");
                //return true;
            }
        }
        else {
            error = "Y";
            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#ReturnQtyDoesNotMatchSelectedQty").text(), "warning");
            return false
        }
    }
    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            for (i = 0; i < FReturnItemDetails.length; i++) {
                var ItemID = FReturnItemDetails[i].ItmCode;
                var GRNNumber = FReturnItemDetails[i].IconGRNNumber;
                if (ItemID == ItmCode && GRNNumber == IconGRNNumber) {
                }
                else {
                    NewArr.push(FReturnItemDetails[i]);
                }
            }
            $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();

                
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
                var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
                var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
                var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
                var TotalQty = $("#TotalReturnQty").text();

                NewArr.push({
                    TotalQty: TotalQty, IconGRNNumber: IconGRNNumber, IconGRNDate: IconGRNDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });

            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));
        }
        else {
            var ReturnItemList = [];
            $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();

                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
                var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
                var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
                var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
                var TotalQty = $("#TotalReturnQty").text();
                ReturnItemList.push({
                    TotalQty: TotalQty, IconGRNNumber: IconGRNNumber, IconGRNDate: IconGRNDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });

            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(ReturnItemList));
        }
    }
    else {
        var ReturnItemList = [];
        $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            
            var wh_id = currentRow.find("#wh_id" + Sno).val();
            var Lot = currentRow.find("#Lot" + Sno).val();
            var Batch = currentRow.find("#Batch1" + Sno).val();
            var Serial = currentRow.find("#Serial1" + Sno).val();
            var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
            var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
            var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
            var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
            var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
            var TotalQty = $("#TotalReturnQty").text();
            ReturnItemList.push({
                TotalQty: TotalQty, IconGRNNumber: IconGRNNumber, IconGRNDate: IconGRNDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
            })
        });
        sessionStorage.removeItem("ItemRetQtyDetails");
        sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(ReturnItemList));
    }
    $("#ReturnQuantity").css("border-color", "#ced4da");
}
function OnClickInvoiceSaveAndExitBtn() {
    

    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var ValDecDigit = $("#ValDigit").text();
    var ItmName = $("#IconItemName").val();
    var ItemUOM = $("#IconUOM").val();
    var InvoiceIconGRNNumber = $("#InvoiceIconGRNNumber").val();
    var InvoiceIconGRNDate = $("#InvoiceIconGRNDate").val();
    var InvoiceIconReturnQuantity = $("#InvoiceIconReturnQuantity").val();
    var InvoicePopupReturnValue = $("#InvoicePopupReturnValue").val();
    var InvoiceItem_id = $("#InvoiceItem_id").val();
    
    if (InvoicePopupReturnValue == "" || InvoicePopupReturnValue == 0) {

        $("#SpanReturnValuePopup").text($("#valueReq").text());
        $("#SpanReturnValuePopup").css("display", "block");
        $("#InvoicePopupReturnValue").css("border-color", "red");
        $("#InvoiceSaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        $("#SpanReturnValuePopup").css("display", "none");
        $("#InvoicePopupReturnValue").css("border-color", "#ced4da");

        $("#InvoiceSaveAndExitBtn").attr("data-dismiss", "modal");
    }
    $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);
        var ItemID = currentRow.find("#hdItemId").val()
        var GRNNo = currentRow.find("#GRNNumber").val()

        if (ItemID == InvoiceItem_id && GRNNo == InvoiceIconGRNNumber) {
            currentRow.find("#ReturnValue").val(parseFloat(InvoicePopupReturnValue).toFixed(ValDecDigit));

        }
    })
    OnChangeReturnValue();
}
function BindItemBatchSerialDetail() {
    
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            var str = JSON.stringify(FReturnItemDetails);
            $('#hdItemBatchSerialDetailList').val(str);
        }

    }
}
function BindGLVoucherDetail() {
    
    var VouList = [];
    //var Val = 0;
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var DocType = "D";
    var TransType = "PurRet";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var dr_amt_bs = "";
            var cr_amt_bs = "";
            var Gltype = "";
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#txthfAccID").val();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            dr_amt_bs = currentRow.find("#dramtInBase").text();
            cr_amt_bs = currentRow.find("#cramtInBase").text();
            Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: DocType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });

        });

    //    $("#VoucherDetail >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        var acc_id = "";
    //        var acc_name = "";
    //        var dr_amt = "";
    //        var cr_amt = "";
    //        acc_id = currentRow.find("#hfAccID").val();
    //        acc_name = currentRow.find("#txthfAccID").val();
    //        dr_amt = currentRow.find("#dramt").text();
    //        cr_amt = currentRow.find("#cramt").text();
    //        gl_type = currentRow.find("#type").val();
    //        VouList.push({ comp_id: Compid, accid: acc_id, acc_name: acc_name, type: "I", doctype: DocType, Value: Val, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, gl_type: gl_type });

    //    });
    }
    var str = JSON.stringify(VouList);
    $('#hdPurchaseReturnVouDetailList').val(str);
}
function OnChangeInvoiceValue() {
    var ValDecDigit = $("#ValDigit").text();
    var TotalValue = parseFloat(0);
    $("#PReturnItmDetailsTbl TBODY TR").each(function () {
        
        var row = $(this);
        var InvoiceValue = "";
        InvoiceValue = row.find("#InvoiceValue").val();
        if (InvoiceValue != null && InvoiceValue != "") {
            if (parseFloat(InvoiceValue) > parseFloat(0)) {
                TotalValue = parseFloat(TotalValue) + parseFloat(InvoiceValue);
            }
        }
        $("#TotalInvoiceValue").val(parseFloat(TotalValue).toFixed(ValDecDigit));
    });
}
function OnChangeReturnValue() { 
    var ValDecDigit = $("#ValDigit").text();
    var TotalValue = parseFloat(0);
    $("#PReturnItmDetailsTbl TBODY TR").each(function () {   
        var row = $(this);
        var ReturnValue = "";
        ReturnValue = row.find("#ReturnValue").val();
        if (ReturnValue != null && ReturnValue != "") {
            if (parseFloat(ReturnValue) > parseFloat(0)) {
                TotalValue = parseFloat(TotalValue) + parseFloat(ReturnValue);
            }
        }
        $("#TotalReturnValue").val(parseFloat(TotalValue).toFixed(ValDecDigit));
    });
    var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
    var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
    $("#TotalReturnValue").val(parseFloat(Oc+Tr).toFixed(ValDecDigit));
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $("#btn_save").prop("disabled", false);
        $("#btn_save").attr("onclick", "return CheckFormValidation()");
        CancelledRemarks("#Cancelled", "Enable");
        return true;
    }
    else {
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_save").prop("disabled", true);
        $("#btn_save").attr("onclick", "");
        CancelledRemarks("#Cancelled", "Enable");
        return false;
    }
    
}
function CheckCancelledStatus() {
    
    if ($("#Cancelled").is(":checked")) {
        return true;
    }
    else {
        return false;
    }
}
function EnableEditkBtntst() {
    
    //sessionStorage.setItem("EditBtnClickPR", "Y");
    return true;
}

function ForwardBtnClick() {
    
    //var PRTStatus = "";
    //PRTStatus = $('#hdPRTStatus').val().trim();
    //if (PRTStatus === "D" || PRTStatus === "F") {

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

    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var PrtDt = $("#txtReturnDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: PrtDt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var PRTStatus = "";
                PRTStatus = $('#hdPRTStatus').val().trim();
                if (PRTStatus === "D" || PRTStatus === "F") {

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
                /*  swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
async function OnClickForwardOK_Btn() {
    
    var fwchkval = $("input[name='forward_action']:checked").val();
    var PRTNo = "";
    var PRTDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_status1 = "";

    Remarks = $("#fw_remarks").val();
    PRTNo = $("#ReturnNumber").val();
    $("#hdDoc_No").val(PRTNo);
    PRTDate = $("#txtReturnDate").val();
    WF_status1 = $("#WF_status1").val();
    docid = $("#DocumentMenuId").val();
    var DashBord = (docid + ',' + PRTNo + ',' + PRTDate + ',' + WF_status1);
    //var DnNarr = $('#DebitNoteRaisedAgainstPurRet').text();
    var DnNarr = $('#DnNarr').val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var src_type = $('#src_type').val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });
    var pdfAlertEmailFilePath = 'PR_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    GetPdfFilePathToSendonEmailAlert(PRTNo, PRTDate, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PRTNo != "" && PRTDate != "" && level != "") {
            
            await Cmn_InsertDocument_ForwardedDetail1(PRTNo, PRTDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/PurchaseReturn/PurchaseReturnApprove?PRTNo=" + PRTNo + "&PRTDate=" + PRTDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&DnNarr=" + DnNarr + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid + "&src_type=" + src_type;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PRTNo != "" && PRTDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRTNo, PRTDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && PRTNo != "" && PRTDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRTNo, PRTDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&DashBord=" + DashBord;
        }
    }
}
function GetPdfFilePathToSendonEmailAlert(PRTNo, PRTDate, fileName) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseReturn/SavePdfDocToSendOnEmailAlert",
        data: { PRTNo: PRTNo, PRTDate: PRTDate, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function OtherFunctions(StatusC, StatusName) {

}

function ForwardHistoryBtnClick() {
    
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ReturnNumber").val();
    
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

//async function GetAllGLID() {

//    var Compid = $("#CompID").text();
//    var DocType = "D";
//    var TransType = 'PurRet';
//    var GLDetail = [];
//    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {

//        var currentRow = $(this);
//        var id = currentRow.find("#id").text();
//        var Amt = currentRow.find("#totval").text();
//        var type = currentRow.find("#type").text();

//        GLDetail.push({ comp_id: Compid, id: id, type: type, doctype: DocType, Value: Amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type });
//    });
//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/PurchaseReturn/GetGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        //async: true,
//        data: JSON.stringify({ GLDetail: GLDetail }),
//        success: function (data) {
//            
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                
//                
//                var arr = [];
//                arr = JSON.parse(data);
//                if (arr.Table.length > 0) {
//                    $('#VoucherDetail tbody tr').remove();
//                    var rowIdx = $('#VoucherDetail tbody tr').length;
//                    for (var j = 0; j < arr.Table.length; j++) {
//                        ++rowIdx;
//                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                            //if (arr.Table[j].acc_id == "100042" && parseFloat(arr.Table[j].Value) > 1) {
//                            //    GetAllGLID();
//                            //    //return;
//                            //}
//                            var Acc_Id = arr.Table[j].acc_id;
//                            var acc_id_start_no = Acc_Id.toString().substring(0, 1);
//                            var Disable;
//                            if (acc_id_start_no == "3" || acc_id_start_no == "4") {
//                                Disable = "";
//                            }
//                            else {
//                                Disable = "disabled";
//                            }
//                            var FieldType = "";
                          
//                            var drAmt = parseFloat(arr.Table[j].type == "Supp" ? arr.Table[j].Value : 0).toFixed(ValDecDigit);
//                            var crAmt = parseFloat(arr.Table[j].type == "Supp" ? 0 : arr.Table[j].Value).toFixed(ValDecDigit);
//                            if (arr.Table[j].type == 'Itm') {
//                                FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;">
//                                        <select class="form-control" id="Acc_name_${rowIdx}" onchange ="OnChangeAccountName(${rowIdx},event)">
//                                        </select>
//                                    <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" /></div> `;
//                                $("#hdnAccID").val(arr.Table[j].acc_id);
//                            }
//                            else {
//                                FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" />`;
//                            }
//                            $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                     <td>`+ FieldType + `</td>
//                                    <td id="dramt" class="num_right">${drAmt}</td>
//                                    <td id="cramt" class="num_right">${crAmt}</td>
//                                    <td style="display: none;">
//                                        <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                                    </td>
//                                           <td class="center">
//                                               <button type="button" ${Disable} id="CostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                           </td>
//                            </tr>`);
//                            //$('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                            //        <td class="sr_padding">${rowIdx}</td>
//                            //        <td id="GLAccName">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                            //        <td id="dramt" class="num_right">${drAmt}</td>
//                            //        <td id="cramt" class="num_right">${crAmt}</td>
//                            //        <td style="display: none;">
//                            //            <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                            //            <input type="hidden" id="SNohf" value="${rowIdx}" />
//                            //            <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                            //        </td>
//                            //               <td class="center">
//                            //                   <button type="button" ${disabled} id="CostCenterDetail" onclick="" class="calculator " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                            //               </td>
//                            //</tr>`);
//                        }
//                    }
//                    var TotInvVal = $("#TotalInvoiceValue").val();
//                    //if (TotInvVal == 0 || TotInvVal == null || TotInvVal == '' || TotInvVal == "NaN") {
//                    if (parseFloat(CheckNullNumber(TotInvVal)) > 0) {
//                        //TotInvVal = 0;
//                        $("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat(TotInvVal).toFixed(ValDecDigit));

//                    }
//                    else {
//                        TotInvVal = 0;
//                        $("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat(TotInvVal).toFixed(ValDecDigit));

//                    }
//                    CalculateVoucherTotalAmount();
//                    BindDDLAccountList();
//                    var errors = [];
//                    var step = [];

//                    for (var i = 0; i < arr.Table1.length; i++) {
//                        
//                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
//                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
//                            step.push(parseInt(i));
//                        }
//                    }

//                    var arrayOfErrorsToDisplay = [];

//                    $.each(errors, function (i, error) {
//                        arrayOfErrorsToDisplay.push({ text: error });
//                    });

//                    Swal.mixin({
//                        confirmButtonText: 'Ok',
//                        type: "warning",
//                    }).queue(arrayOfErrorsToDisplay)
//                        .then((result) => {
//                            if (result.value) {

//                            }
//                        });
//                }

//            }

//        }
//    });
//}
function OnChangeAccountName(RowID, e) {
    
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').each(function () {
                var row = $(this);
                var vouSrNo = row.find("#hdntbl_vou_sr_no").text();
                if (vouSrNo == vou_sr_no) {
                    row.remove();
                }
            });
        }
        //Added by Suraj on 09-08-2024 to stop reset of GL Account if user changes the GL Acc.
        $("#PReturnItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
            var row = $(this);
            row.find("#hdn_item_gl_acc").val(Acc_ID);
        });
    }
    if (Acc_ID.toString().substring(0, 1) == "3" || Acc_ID.toString().substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }

    clickedrow.find("#hfAccID").val(Acc_ID);
    $("#hdnAccID").val(Acc_ID);
}
//function CalculateVoucherTotalAmount() {
    
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
        
//        var currentRow = $(this);
//        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//        }
//    });
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {

//        AddRoundOffGL(DrTotAmt, CrTotAmt);

//    }
//}

function DeleteVoucherDetail(ItemCode, GrnNo, InvoiceValue, ReturnValue) {
    
    if (InvoiceValue == '' || InvoiceValue == null) {
        InvoiceValue = 0;
    }
    if (ReturnValue == '' || ReturnValue == null) {
        ReturnValue = 0;
    }
    var TotalInvval = $("#TotalInvoiceValue").val();
    if (TotalInvval == '' || TotalInvval == null) {
        TotalInvval = 0;
    }
    var FTotalInvval = parseFloat(TotalInvval) - parseFloat(InvoiceValue)

    var TotalRetval = $("#TotalReturnValue").val();
    if (TotalRetval == '' || TotalRetval == null) {
        TotalRetval = 0;
    }
    var FTotalRetval = parseFloat(TotalRetval) - parseFloat(ReturnValue)
    $("#TotalReturnValue").val(parseFloat(FTotalRetval).toFixed(ValDecDigit));
    $("#TotalInvoiceValue").val(parseFloat(FTotalInvval).toFixed(ValDecDigit));
    $("#hd_GL_DeatilTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        var ItemId = currentRow.find("#itemid").text();
        var MrNo = currentRow.find("#mrno").text();
        if (ItemCode == ItemId && GrnNo == MrNo) {
            currentRow.remove();
        }
    });
    GetAllGLID();


}

function UpdateGLValue(ItemCode, GrnNo, ReturnQuantity,Itemrate,taxtable) {

    var NewArray = [];
    $("#hd_GL_DeatilTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);
        var ItemId = currentRow.find("#itemid").text();
        var MrNo = currentRow.find("#mrno").text();
        var type = currentRow.find("#type").text();
        var GL_tax_level = currentRow.find("#GL_tax_level").text();
        if (ItemCode == ItemId && GrnNo == MrNo) {
            if (type == "Tax") {
                //if (GL_tax_level)
                var landedRate = parseFloat(Itemrate) * parseFloat(ReturnQuantity)
                var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
               
                taxtable.sort(function (a, b) {
                    return a.tax_level - b.tax_level;
                });
                var filtered = taxtable.filter(x => x.tax_level == GL_tax_level);
                for (var p = 0; p < filtered.length; p++) {
                    var TaxPec = filtered[p].tax_rate;
                    if (filtered[p].tax_apply_on == "I") {
                        if (filtered[p].tax_level == "1") {
                            TaxAmount = ((parseFloat(landedRate) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                            var TaxLevelTbl = parseInt(filtered[p].tax_level) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        NewArray.push({
                            TaxItmCode: ItemId, TaxNameID: filtered[p].tax_id,
                            TaxPercentage: filtered[p].tax_rate, TaxLevel: filtered[p].tax_level, TaxApplyOn: filtered[p].tax_apply_on,
                            TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt
                        });
                    }
                    if (filtered[p].tax_apply_on == "C") {
                        var Level = filtered[p].tax_level;
                        if (filtered[p].tax_level == "1") {
                            TaxAmount = ((parseFloat(landedRate) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (filtered[p].tax_level != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(landedRate) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        NewArray.push({
                            TaxItmCode: ItemId, TaxNameID: filtered[p].tax_id,
                            TaxPercentage: filtered[p].tax_rate, TaxLevel: filtered[p].tax_level, TaxApplyOn: filtered[p].tax_apply_on,
                            TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt
                        });
                    }

                    //landedtax = parseFloat(landedtax) + parseFloat(TaxAmount);
                    //$("#TaxAmount").val(parseFloat(parseFloat(landedtax) / parseFloat(ReturnQuantity)).toFixed(ValDecDigit));
                    currentRow.find("#totval").text(TaxAmount);
                }

                //var landedrate = parseFloat(Itemrate) * parseFloat(ReturnQuantity)
                //var total = (parseFloat(landedrate) * parseFloat(tax_rate) / 100);
                //currentRow.find("#totval").text(total);
            }
            else {
                var Amt = parseFloat(currentRow.find("#amt").text()).toFixed(ValDecDigit);
                //var Qty = currentRow.find("#mrqty").text();
                var Value = parseFloat(Amt) * parseFloat(ReturnQuantity)
                currentRow.find("#totval").text(Value);
            }
        }
    });
    GetAllGLID();


}

function DeleteItemBatchSerialDetail(ItemCode, GrnNo) {
    
    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            
            for (i = 0; i < FReturnItemDetails.length; i++) {
                var ItemID = FReturnItemDetails[i].ItmCode;
                var GRNNumber = FReturnItemDetails[i].IconGRNNumber;

                if (ItemID == ItemCode && GRNNumber == GrnNo) {


                }
                else {
                    NewArr.push(FReturnItemDetails[i])
                }

            }

            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));

        }
    }
}

//function AddRoundOffGL(DrTotAmt, CrTotAmt) {
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
            
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                if (arr.Table.length > 0) {
//                    // $('#VoucherDetail tbody tr').remove();
//                    if (DrTotAmt < CrTotAmt) {
//                        var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                var Acc_Id = arr.Table[j].acc_id;
//                                var acc_id_start_no = Acc_Id.toString().substring(0, 1);
//                                var Disable;
//                                if (acc_id_start_no == "3" || acc_id_start_no == "4") {
//                                    Disable = "";
//                                }
//                                else {
//                                    Disable = "disabled";
//                                }
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;">
//                                        <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                                    </td>
//                                           <td class="center">
//                                               <button type="button" ${Disable} id="CostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                           </td>
                                   
//                            </tr>`);
//                            }
//                        }
//                    }
//                    else {
//                        var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);

//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                var Acc_Id = arr.Table[j].acc_id;
//                                var acc_id_start_no = Acc_Id.toString().substring(0, 1);
//                                var Disable;
//                                if (acc_id_start_no == "3" || acc_id_start_no == "4") {
//                                    Disable = "";
//                                }
//                                else {
//                                    Disable = "disabled";
//                                }
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;">
//                                       <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                                    </td>
//                                           <td class="center">
//                                               <button type="button" ${Disable} id="CostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                           </td>
                                   
//                            </tr>`);
//                            }
//                        }
//                    }

//                    CalculateVoucherTotalAmount();

//                }

//            }

//        }
//    });
//}

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    
    var clickedrow = $(e.target).closest("tr");
    //var CstDbAmt = clickedrow.find("#dramt").text(); commented by Suraj on 30-03-2024
    //var CstCrtAmt = clickedrow.find("#cramt").text();
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
    if (GLAcc_Name == null || GLAcc_Name == "") {
        GLAcc_Name = clickedrow.find("#txthfAccID").val();
        GLAcc_id = clickedrow.find("#hfAccID").val();
    }
    var disableflag = ($("#DisableSubItem").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            List.vou_sr_no = row.find('#hdntbl_vou_sr_no').text();
            List.gl_sr_no = row.find('#hdntbl_gl_sr_no').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}

//-------------------Cost Center Section End-------------------//

/**---------------WareHouse Sub Item Return Quantity PopUP Work Start----------------------**/
function OnclickSubItemRtnQtyDetail() {
    
    var QtyDecDigit = $("#QtyDigit").text();
    var Arr = [];
    var ProductNm = $("#IconItemName").val();
    var ProductId = $("#RQhdItemId").val();
    var Subitem = $("#Iconsub_item").val();
    var UOM = $("#IconUOM").val();
    var GRNNo = $("#IconGRNNumber").val();
    var GRNDt = $("#IconGRNDate").val();

    var subitm_RtrnQty = $("#IconReturnQuantity").val();
    /*var doc_Date = $("#Prod_Ord_number option:selected")[0].dataset.date*/

    $("#RtWh_Sub_ProductlName").val(ProductNm);
    $("#RtWh_Sub_ProductlId").val(ProductId);
    $("#RtWh_Subitem").val(Subitem);
    $("#RtWh_Sub_serialUOM").val(UOM);
    $("#RtWh_Sub_GRNNo").val(GRNNo);
    $("#RtWh_Sub_GRNDate").val(GRNDt);
    $("#RtWh_Sub_RetrnQuantity").val(subitm_RtrnQty);
    var subitmDisable = "";
    var RtWh_Subitem = $("#RtWh_Subitem").val();
    if (RtWh_Subitem != "Y") {
        subitmDisable = "disabled";
    }
    $("#ReturnedQtyDetailsTbl tbody tr").each(function () {
        var Row = $(this);
        var RowNo = Row.find("#SNohiddenfiled").val();
        var List = {};

        var wh_id = Row.find("#wh_id" + RowNo).val();
        var wh_type = Row.find("#wh_type" + RowNo).val();
        var wh_name = Row.find("#Warehouse" + RowNo).val();
        var avl_qty = CheckNullNumber(Row.find("#AvailableQuantity" + RowNo).val());
        var return_qty = CheckNullNumber(Row.find("#IconReturnQtyinput" + RowNo).val());
        var CheckWh = Arr.filter(v => v.wh_id == wh_id);
        if (CheckWh.length > 0) {
            let Index = Arr.findIndex(p => p.wh_id == wh_id);
            Arr[Index].avl_qty = parseFloat(Arr[Index].avl_qty) + parseFloat(avl_qty);
            Arr[Index].return_qty = parseFloat(Arr[Index].return_qty) + parseFloat(return_qty);
        } else {
            Arr.push({ wh_id: wh_id, wh_name: wh_name, wh_type: wh_type, avl_qty: avl_qty, return_qty: return_qty });
        }

    });
    $("#RtWh_Sub_ItemDetailTbl tbody >tr").remove();
    var PRTWhReturnQty = 0;
    var Disable = $("#DisableSubItem").val();

    Arr.map(function (Item, Index) {
        debugger
        var disabled = "";
        if (Disable == "Y") {
            disabled = "disabled";
            //$("#RtWh_SubItemSaveAndExitBtn").attr("disabled", true);
            //$("#RtWh_SubItemDiscardAndExitBtn").attr("disabled", true);
            $("#RtWh_SubItemSaveAndExitBtn").css("display", "none");
            $("#RtWh_SubItemDiscardAndExitBtn").css("display", "none");
            $("#Close").css("display", "block");


        }
        else {
            disabled = "";
            //$("#RtWh_SubItemSaveAndExitBtn").attr("disabled", false);
            //$("#RtWh_SubItemDiscardAndExitBtn").attr("disabled", false);
            $("#RtWh_SubItemSaveAndExitBtn").css("display", "block");
            $("#RtWh_SubItemDiscardAndExitBtn").css("display", "block");
            $("#Close").css("display", "none");
        }
        var itemiconreturnqty = "";
        if (parseFloat(Item.return_qty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit) || Item.return_qty == "" || Item.return_qty == null) {
            itemiconreturnqty = "";
        }
        else {
            itemiconreturnqty = parseFloat(Item.return_qty).toFixed(QtyDecDigit);
        }
        $("#RtWh_Sub_ItemDetailTbl tbody").append(`<tr>
                            <td><input class="form-control" id="RtWhSubItm_SrNo" disabled value="${Index + 1}" /></td>
                            <td><input class="form-control" id="RtWhSubItm_WhName" disabled value="${Item.wh_name}" /></td>
                            <td style="display: none;">
                            <input  type="hidden" id="RtWhSubItm_WhId" value="${Item.wh_id}"/></td>
                            <input  type="hidden" id="RtWhSubItm_WhType" value="${Item.wh_type}"/></td>
                            <td><input class="form-control num_right" id="RtWhSubItm_AvlQty" disabled value="${parseFloat(Item.avl_qty).toFixed(QtyDecDigit)}" autocomplete="off" type="text" placeholder="0000.00" /></td>
                            <td>
                              <div class="col-sm-10 lpo_form no-padding">
                                <input class="form-control num_right" id="RtWhSubItm_ReturnQty" ${disabled} value="${itemiconreturnqty}" onchange="OnChangePopupRTWhReturnQty(this,event)" onkeypress="return OnKeyPressRetQty(this,event);" autocomplete="off" type="text" placeholder="0000.00" />
                                <span id="SpanRtWhSubItm_ReturnQty" class="error-message is-visible"></span></div>
                              </div>
                              <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReturnQty" >
                              <button type="button" id="SubItemReturnQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PRTReturnQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                              </div>
                            </td>
                        </tr>`);

        if (Item.return_qty == "" || Item.return_qty == null) {
            Item.return_qty = 0;
        }

        PRTWhReturnQty = parseFloat(PRTWhReturnQty) + parseFloat(Item.return_qty);

    })
    $("#RtWh_TotalQty").text(parseFloat(PRTWhReturnQty).toFixed(QtyDecDigit));
}
function OnChangePopupRTWhReturnQty(el, evt) {
    
    var clickedrow = $(evt.target).closest("tr");
    var Sno = clickedrow.find("#RtWhSubItm_SrNo").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var RetQty = clickedrow.find("#RtWhSubItm_ReturnQty").val();
    var Available = parseFloat(clickedrow.find("#RtWhSubItm_AvlQty").val()).toFixed(parseFloat(QtyDecDigit));
    if ((RetQty == "") || (RetQty == null) || (RetQty == 0) || (RetQty == "NaN")) {
        RetQty = 0;
    }
    if (parseFloat(RetQty) <= parseFloat(Available)) {

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").css("display", "none");
        clickedrow.find("#RtWhSubItm_ReturnQty").css("border-color", "#ced4da");

    }
    else {
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").text($("#ExceedingQty").text());
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").css("display", "block");
        clickedrow.find("#RtWhSubItm_ReturnQty").css("border-color", "red");
        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);

    }

    RtWhSubItmCalTotalReturnQty(el, evt);
}
function RtWhSubItmCalTotalReturnQty(el, evt) {
    

    var QtyDecDigit = $("#QtyDigit").text();
    var TotalQty = parseFloat(0).toFixed(QtyDecDigit);

    $("#RtWh_Sub_ItemDetailTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        //var Sno = CRow.find("#SNohiddenfiled").val();
        var RtnQty = CRow.find("#RtWhSubItm_ReturnQty").val();
        if (RtnQty != "" && RtnQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(RtnQty));
        }
    });

    $("#RtWh_TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));

};
function PRtWh_SaveAndExitSubItem() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity     
    var IconGRNNumber = $("#RtWh_Sub_GRNNo").val();

    var IGRNDate = $("#RtWh_Sub_GRNDate").val().trim();
    var date = IGRNDate.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    var IconGRNDate = FDate;
    var IconReturnQuantity = $("#RtWh_Sub_RetrnQuantity").val();
    var ItmCode = $("#RtWh_Sub_ProductlId").val();
    var ItmUomId = $("#RtWh_Sub_serialUOM").val();

    var TotalQty = $("#RtWh_TotalQty").text();

    var error = "N";
    $("#RtWh_Sub_ItemDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#RtWhSubItm_SrNo").val();
        
        var RetQty = currentRow.find("#RtWhSubItm_ReturnQty").val();
        var RTWhId = currentRow.find("#RtWhSubItm_WhId").val();
        RetQty = parseFloat(CheckNullNumber(RetQty));

        var Available = parseFloat(currentRow.find("#RtWhSubItm_AvlQty").val()).toFixed(parseFloat(QtyDecDigit));
        if (parseFloat(RetQty) <= parseFloat(Available)) {

            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value='" + ItmCode + "']").closest('tr')
                .find("#subItemSrcDocNo[value='" + IconGRNNumber + "']").closest('tr')
                .find("#subItemSrcDocDate[value='" + IconGRNDate + "']").closest('tr')
                .find("#subItemWhId[value='" + RTWhId + "']").closest('tr');

            var SubTotalQty = 0;

            rows.each(function () {
                var clickedrow1 = $(this);
                let subItemQty = clickedrow1.find('#subItemQty').val();
                SubTotalQty = parseFloat(SubTotalQty) + parseFloat(CheckNullNumber(subItemQty));
            });
            var SubItemButton = "SubItemReturnQty";
            var sub_item = $("#RtWh_Subitem").val();
            if (sub_item == "Y") {
                if (parseFloat(RetQty) != parseFloat(SubTotalQty)) {
                    error = "Y";
                    currentRow.find("#" + SubItemButton).css("border", "1px solid red");
                } else {
                    currentRow.find("#" + SubItemButton).css("border", "");
                }
            }
            currentRow.find("#SpanRtWhSubItm_ReturnQty").text("");
            currentRow.find("#SpanRtWhSubItm_ReturnQty").css("display", "none");
            currentRow.find("#RtWhSubItm_ReturnQty").css("border-color", "#ced4da");
            $("#SubItemReturnQuantity_Detail").css('border', '1px solid #ced4da');
        }
        else {
            currentRow.find("#SpanRtWhSubItm_ReturnQty").text($("#ExceedingQty").text());
            currentRow.find("#SpanRtWhSubItm_ReturnQty").css("display", "block");
            currentRow.find("#RtWhSubItm_ReturnQty").css("border-color", "red");
            error = "Y";
            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
        }
        

    });
    if (error == "Y") {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        if (parseFloat(IconReturnQuantity) == parseFloat(TotalQty)) {

            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "modal");
            //var Subitemvalidation = ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y");
            if (ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", ItmCode) == false) {
                error = "Y";
                ResetWorningBorderColor();

                $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
                //swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
                //var SubItemButton = "SubItemReturnQty";
                return false
            }
            else {
                return true;
            }
        }
        else {
            error = "Y";
            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#ReturnQtyDoesNotMatchSelectedQty").text(), "warning");
            return false
        }
    }
}

/**--------------- Sub Item Return Quantity PopUP Work End----------------------**/

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    
    Cmn_SubItemHideShow(sub_item, clickedrow, "RtWh_Subitem", "SubItemReturnQty");

}
function SubItemDetailsPopUp(flag, e, avlstock) {

    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();

    var wh_id = clickdRow.find("#RtWhSubItm_WhId").val();
    var WhName = clickdRow.find("#RtWhSubItm_WhName").val();
    var WhType = clickdRow.find("#RtWhSubItm_WhType").val();

    var ProductNm = $("#IconItemName").val();
    var ProductId = $("#RQhdItemId").val();
    var UOM = $("#IconUOM").val();
    var UOMID = 0;
    var GRNNo = $("#IconGRNNumber").val();
    var GRNDate = $("#IconGRNDate").val();
    var GRNDt = GRNDate.split("-").reverse().join("-");
    var Hdnmessage = $("#Hdn_Message").val();
    if (Hdnmessage == "DocModify") {
        GRNDt = $("#IconGRNDate").val();
    }

    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    //var src_doc_no = $("#hd_doc_no").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "PRTReturnQty") {
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + ProductId + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + GRNNo + "']").closest('tr')
            .find("#subItemWhId[value='" + wh_id + "']").closest('tr');
        /*$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {*/
        rows.each(function () {
            var row = $(this);
            var List = {};
            
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlStk').val();
            List.GRNNo = row.find('#subItemSrcDocNo').val();
            List.GRNDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#RtWhSubItm_ReturnQty").val();
        var IsDisabled = $("#DisableSubItem").val();
    }
    else if (flag == "AdPRTReturnQty") {
        UOM = clickdRow.find("#UOM").val();
        UOMID = clickdRow.find("#hdUOMId").val();
        wh_id = clickdRow.find("#wh_id" + hfsno).val();
        WhName = clickdRow.find("#wh_id" + hfsno + " option:selected").text();
        ProductId = clickdRow.find("#hdItemId").val();
        ProductNm = clickdRow.find("#ItemName" + hfsno + " option:selected").text();
        Sub_Quantity = clickdRow.find("#ReturnQuantity").val();
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + ProductId + "]").closest('tr');
        rows.each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlStk').val();
            List.GRNNo = row.find('#subItemSrcDocNo').val();
            List.GRNDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        });        
        var IsDisabled = $("#DisableSubItem").val();
    }
    else {
        var ProductNm = clickdRow.find("#ItemName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();
        var PinvNo = $("#ddlDocumentNumber option:selected").text();
        var GRNNo = clickdRow.find("#GRNNumber").val();
        var GRNDate = clickdRow.find("#GRNDate").val();
        var GRNDt = GRNDate.split("-").reverse().join("-");
        var Hdnmessage = $("#Hdn_Message").val();
        if (Hdnmessage == "DocModify") {
            GRNDt = clickdRow.find("#GRNDate").val();
        }
        Sub_Quantity = clickdRow.find("#GRNQuantity").val();
        var IsDisabled = "Y";
    }

    var src_type = $('#src_type').val();
    var hd_Status = $("#hdPRTStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseReturn/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            GRNNo: GRNNo,
            GRNDt: GRNDt,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            wh_id: wh_id,
            WhType: WhType,
            PinvNo: PinvNo,
            src_type: src_type,
            UOMID: UOMID,
            avlstock: avlstock

        },
        success: function (data) {

            if (hd_Status == "A" && avlstock == "avlstock") {
                var ItemQty = $("#AvailableStock").val();
                $("#SubItemStockPopUp").html(data);
                $("#Stk_Sub_ProductlName").val(ProductNm);
                $("#Stk_Sub_ProductlId").val(ProductId);
                $("#Stk_Sub_serialUOM").val(UOM);
                $("#Stk_Sub_Quantity").val(ItemQty);
                //$("#Stk_Sub_Quantity").val(ItemQty);
            }
            else {
                $("#SubItemPopUp").html(data);
                $("#Sub_ProductlName").val(ProductNm);
                $("#Sub_ProductlId").val(ProductId);
                $("#Sub_serialUOM").val(UOM);

                if (flag == "PRTReturnQty") {
                    $("#subWHName").css('display', 'block');
                }
                if (src_type == "H") {
                    $("#subSrcDocNo").css('display', 'none');
                    $("#subSrcDocdate").css('display', 'none');
                }
                else {
                    $("#subSrcDocNo").css('display', 'block');
                    $("#subSrcDocdate").css('display', 'block');
                }
                $("#Sub_SrcDocNo").val(GRNNo);
                $("#Sub_SrcDocDate").val(GRNDt);
                $("#hdSub_SrcDocDate").val(GRNDt);
                $("#Sub_Quantity").val(Sub_Quantity);
                $("#hdSub_WHId").val(wh_id);
                $("#sub_WHName").val(WhName);
            }
           
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    return Cmn_CheckValidations_forSubItems("ReturnedQtyDetailsTbl", "", "RQhdItemId", "ReturnQuantity", "SubItemReturnQty", "Y");
}
function ResetWorningBorderColor() {
    
    return PRCheckValidations_forSubItems("RtWh_Sub_ItemDetailTbl", "", "RtWh_Sub_ProductlId", "RtWhSubItm_ReturnQty", "SubItemReturnQty", "N");
}
function PRCheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    
    var docid = $("#DocumentMenuId").val();
    var SrcDocNo = $("#Sub_SrcDocNo").val();
    var SrcDocDt = $("#Sub_SrcDocDate").val();
    //var SrcDocWhId = $("#hdSub_WHId").val();
    var Sub_ProductlId = $('#Sub_ProductlId').val();
    var src_type = $('#src_type').val();
    let clickedrow = ""
    if (src_type == "H") {
        clickedrow = $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + Sub_ProductlId + "']").closest("tr");
    }
    else {
        clickedrow = $("#PReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + Sub_ProductlId + "']").closest("tr");
    }
    clickedrow.find("#SubItemReturnQty").css("border", "");

    var flag = "N";
    $("#" + Main_table + " tbody tr ").each(function () {
        var PPItemRow = $(this);
        var item_id;
        item_id = $("#" + Item_field_id).val();

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var SrcDocWhId = PPItemRow.find("#RtWhSubItm_WhId").val();
        var sub_item = $("#RtWh_Subitem").val();
        var Sub_Quantity = 0;

        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr').find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr').find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr');

        /*$("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {*/
        rows.each(function () {

            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            var subItemSrcDocNo = Crow.find("#subItemSrcDocNo").val();
            var subItemWhId = Crow.find("#subItemWhId").val();

            if (subItemSrcDocNo == SrcDocNo && subItemWhId == SrcDocWhId) {
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            }
            else {
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            }
            //Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });

        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
        }
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}

function fn_PRTCustomReSetSubItemData1(itemId) {
    debugger
    var Quantity = $("#Sub_Quantity").val();
    var SrcDocNo = $("#Sub_SrcDocNo").val();
    var SrcDocDt = $("#Sub_SrcDocDate").val();
    var SrcDocWhId = $("#hdSub_WHId").val();
    var src_type = $('#src_type').val();
    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        var subItemAvlQty = Crow.find("#subItemAvlStk").val();
        //var subItemReqQty = Crow.find("#subItemReqQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();
        if (src_type == "H") {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr').each(function () {
                    var InnerCrow = $(this).closest("tr");
                    var wh_id = InnerCrow.find("#subItemWhId").val();
                    var checkWh = $("#RtWh_Sub_ItemDetailTbl > tbody > tr #RtWhSubItm_WhId[value='" + wh_id + "']").val();
                    if (checkWh != "" && checkWh != null) {

                    } else {
                        InnerCrow.remove();
                    }
                });
        }
        else {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr').each(function () {
                    var InnerCrow = $(this).closest("tr");
                    var wh_id = InnerCrow.find("#subItemWhId").val();
                    var checkWh = $("#RtWh_Sub_ItemDetailTbl > tbody > tr #RtWhSubItm_WhId[value='" + wh_id + "']").val();
                    if (checkWh != "" && checkWh != null) {

                    } else {
                        InnerCrow.remove();
                    }
                });
        }
       
        /* var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;*/
        if (src_type == "H") {
            var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr').length;
        }
        else {
            var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr')
                .find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr').length;
        }

        if (len > 0) {
            if (src_type == "H") {
                var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr');
            }
            else {
                var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
                    .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr')
                    .find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr');
            }


            /*$("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {*/
            rows.each(function () {
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemQty").val(subItemQty);
                InnerCrow.find("#subItemAvlStk").val(subItemAvlQty);
                InnerCrow.find("#subItemSrcDocNo").val(SrcDocNo);
                InnerCrow.find("#subItemSrcDocDate").val(SrcDocDt);
                InnerCrow.find("#subItemWhId").val(SrcDocWhId);

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlStk" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${SrcDocNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${SrcDocDt}'></td>
                            <td><input type="text" id="subItemWhId" value='${SrcDocWhId}'></td>
                            
                        </tr>`);

        }

    });

}
function PRT_SubItemList(src_type) {
    var NewArr = new Array();
    debugger;
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        
        var Qty = row.find('#subItemQty').val();
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlStk').val();
            List.GRNNo = row.find('#subItemSrcDocNo').val();
            List.GRNDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        }
    });
    return NewArr;
}
function DeleteSubItemQtyDetail(item_id, GrnNo, GrnDt) {
    

    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + GrnNo + "']").closest('tr').length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                    .find("#subItemSrcDocNo[value='" + GrnNo + "']").closest('tr').remove();
            }
        }
    }

}
/***--------------------------------Sub Item Section End-----------------------------------------***/
/*------------------------All PopupValidation For SubItem ----------------*/
function ItemtbltoRetrnQtyTblToRtwhtosubitmValidation(ShowMessage, ItmCode, IconGRNNumber, src_type) {
    
    var flag = "N";
    var EffectedRows;
    if (src_type == "H") {
        if (ItmCode != null && ItmCode != "") {
            EffectedRows = $("#AdSReturnItmDetailsTbl tbody tr #hdItemId[value='" + ItmCode + "']").closest("tr");
        } else {
            EffectedRows = $("#AdSReturnItmDetailsTbl tbody tr");
        }
    }
    else {
        if (ItmCode != null && ItmCode != "") {
            EffectedRows = $("#PReturnItmDetailsTbl tbody tr #hdItemId[value='" + ItmCode + "']").closest("tr")
                .find("#GRNNumber[value='" + IconGRNNumber + "']").closest("tr");
        } else {
            EffectedRows = $("#PReturnItmDetailsTbl tbody tr");
        }
    }

    if (src_type == "H") {
        EffectedRows.each(function () {
            var Row = $(this);
            var ArrFiltData = [];
            var Item_Id = Row.find("#hdItemId").val();
            //var GrnNo = Row.find("#GRNNumber").val();
            //var GRNDate = Row.find("#GRNDate").val();
            //var GrnDt = GRNDate.split("-").reverse().join("-");
            var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
            if (ItmCode != null && ItmCode != "") {
                var NewArr = [];
                $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var Sno = currentRow.find("#SNohiddenfiled").val();


                    var wh_id = currentRow.find("#wh_id" + Sno).val();
                    var Lot = currentRow.find("#Lot" + Sno).val();
                    var Batch = currentRow.find("#Batch1" + Sno).val();
                    var Serial = currentRow.find("#Serial1" + Sno).val();
                    var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();

                    NewArr.push({
                        IconGRNNumber: "", IconGRNDate: "", ItmCode: Item_Id, wh_id: wh_id
                        , Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                    })
                });
                FReturnItemDetails = NewArr;
            }

            if (FReturnItemDetails != null && FReturnItemDetails != "") {
                if (FReturnItemDetails.length > 0) {

                    var ArrFiltData = FReturnItemDetails;
                    var ArrFilt_Data = ArrFiltData.filter(v => v.ItmCode == Item_Id && v.RetQty > 0)
                    if (ArrFilt_Data.length > 0) {
                        var Array = [];
                        for (j = 0; j < ArrFilt_Data.length; j++) {

                            var WhId = ArrFilt_Data[j].wh_id;
                            var return_qty = ArrFilt_Data[j].RetQty;

                            var CheckWh = Array.filter(v => v.wh_id == WhId);
                            if (CheckWh.length > 0) {
                                let Index = Array.findIndex(p => p.wh_id == WhId);
                                //Arr[Index].avl_qty = parseFloat(Arr[Index].avl_qty) + parseFloat(avl_qty);
                                Array[Index].return_qty = parseFloat(Array[Index].return_qty) + parseFloat(return_qty);
                            } else {
                                Array.push({ Item_Id: Item_Id, GrnNo: "", GrnDt: "", wh_id: WhId, return_qty: return_qty });
                            }
                        }
                        for (k = 0; k < Array.length; k++) {
                            let itmId = Array[k].Item_Id;
                            let whid = Array[k].wh_id;
                            let rtrnQty = Array[k].return_qty;
                            var sub_item = Row.find("#sub_item").val();
                            var Sub_Quantity = 0;
                            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + itmId + "]").closest('tr')
                                .find("#subItemWhId[value='" + whid + "']").closest('tr');
                            rows.each(function () {
                                var Crow = $(this).closest("tr");
                                var subItemQty = Crow.find("#subItemQty").val();
                                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
                            });
                            var SubItemButton = "SubItemReturnQuantity_Detail";
                            if (ItmCode != null && ItmCode != "") {
                                if (sub_item == "Y") {
                                    if (rtrnQty != Sub_Quantity) {
                                        flag = "Y";
                                        $("#" + SubItemButton).css("border", "1px solid red");

                                    } else {
                                        $("#" + SubItemButton).css("border", "");
                                    }
                                }
                            }
                            else {
                                SubItemButton = "ReturnQuantityDetail";
                                if (sub_item == "Y") {
                                    if (rtrnQty != Sub_Quantity) {
                                        flag = "Y";
                                        Row.find("#" + SubItemButton).css("border", "1px solid red");

                                    } else {
                                        Row.find("#" + SubItemButton).css("border", "");
                                    }
                                }
                            }

                        }
                    }
                }
            }

            else {
                flag = "Y"
            }
        });
    }
    else {
        EffectedRows.each(function () {
            var Row = $(this);
            var ArrFiltData = [];
            var Item_Id = Row.find("#hdItemId").val();
            var GrnNo = Row.find("#GRNNumber").val();
            var GRNDate = Row.find("#GRNDate").val();
            var GrnDt = GRNDate.split("-").reverse().join("-");
            var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
            if (ItmCode != null && ItmCode != "") {
                var NewArr = [];
                $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var Sno = currentRow.find("#SNohiddenfiled").val();


                    var wh_id = currentRow.find("#wh_id" + Sno).val();
                    var Lot = currentRow.find("#Lot" + Sno).val();
                    var Batch = currentRow.find("#Batch1" + Sno).val();
                    var Serial = currentRow.find("#Serial1" + Sno).val();
                    var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();

                    NewArr.push({
                        IconGRNNumber: GrnNo, IconGRNDate: GrnDt, ItmCode: Item_Id, wh_id: wh_id
                        , Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                    })
                });
                FReturnItemDetails = NewArr;
            }

            if (FReturnItemDetails != null && FReturnItemDetails != "") {
                if (FReturnItemDetails.length > 0) {

                    var ArrFiltData = FReturnItemDetails;
                    var ArrFilt_Data = ArrFiltData.filter(v => v.ItmCode == Item_Id && v.IconGRNNumber == GrnNo && v.IconGRNDate == GrnDt && v.RetQty > 0)
                    if (ArrFilt_Data.length > 0) {
                        var Array = [];
                        for (j = 0; j < ArrFilt_Data.length; j++) {

                            var WhId = ArrFilt_Data[j].wh_id;
                            var return_qty = ArrFilt_Data[j].RetQty;

                            var CheckWh = Array.filter(v => v.wh_id == WhId);
                            if (CheckWh.length > 0) {
                                let Index = Array.findIndex(p => p.wh_id == WhId);
                                //Arr[Index].avl_qty = parseFloat(Arr[Index].avl_qty) + parseFloat(avl_qty);
                                Array[Index].return_qty = parseFloat(Array[Index].return_qty) + parseFloat(return_qty);
                            } else {
                                Array.push({ Item_Id: Item_Id, GrnNo: GrnNo, GrnDt: GrnDt, wh_id: WhId, return_qty: return_qty });
                            }
                        }
                        for (k = 0; k < Array.length; k++) {
                            let itmId = Array[k].Item_Id;
                            let grnNo = Array[k].GrnNo;
                            let grndt = Array[k].GrnDt;
                            let whid = Array[k].wh_id;
                            let rtrnQty = Array[k].return_qty;
                            var sub_item = Row.find("#sub_item").val();
                            var Sub_Quantity = 0;
                            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + itmId + "]").closest('tr')
                                .find("#subItemSrcDocNo[value='" + grnNo + "']").closest('tr')
                                .find("#subItemSrcDocDate[value='" + grndt + "']").closest('tr')
                                .find("#subItemWhId[value='" + whid + "']").closest('tr');
                            rows.each(function () {
                                var Crow = $(this).closest("tr");
                                var subItemQty = Crow.find("#subItemQty").val();
                                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
                            });
                            var SubItemButton = "SubItemReturnQuantity_Detail";
                            if (ItmCode != null && ItmCode != "") {
                                if (sub_item == "Y") {
                                    if (rtrnQty != Sub_Quantity) {
                                        flag = "Y";
                                        $("#" + SubItemButton).css("border", "1px solid red");

                                    } else {
                                        $("#" + SubItemButton).css("border", "");
                                    }
                                }
                            }
                            else {
                                SubItemButton = "ReturnQuantityDetail";
                                if (sub_item == "Y") {
                                    if (rtrnQty != Sub_Quantity) {
                                        flag = "Y";
                                        Row.find("#" + SubItemButton).css("border", "1px solid red");

                                    } else {
                                        Row.find("#" + SubItemButton).css("border", "");
                                    }
                                }
                            }

                        }
                    }
                }
            }

            else {
                flag == "Y"
            }
        });
    }
    
    if (flag == "Y") {
        if (ShowMessage == "Y") {
            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}
function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
    
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

/***---------------------------GL Voucher Entry-----------------------------***/
function GetAllGLID() {
    debugger;
    GetAllGL_WithMultiSupplier();
}
/*GetAllGLIDForImport() function is use for both domestinc and import*/
async function GetAllGL_WithMultiSupplier() {
    debugger;
    debugger;
    var src_type = $("#src_type").val();
    //var GstType = "IGST"//$("#Hd_GstType").val();
    if (src_type == "H") {
        if ($("#AdSReturnItmDetailsTbl > tbody > tr").length == 0) {
            return false;
        }
        if (CheckItemLevelValidations("FromGL") == false) {
            return false;
        }
    }
    else {
        if ($("#PReturnItmDetailsTbl > tbody > tr").length == 0) {
            return false;
        }
        if (CheckItemLevelValidations("FromGL") == false) {
            return false;
        }
    }
    var curr_id = $("#hdn_curr_id").val();
    var conv_rate = $("#Hdn_conv_rate").val();
    var supp_acc_id = $("#supp_acc_id").val();

    var bill_no = $("#hdn_bill_no").val();
    var bill_dt = $("#hdn_bill_dt").val();
  
    var Compid = $("#CompID").text();
    var DocType = "D";
    var TransType = 'PurRet';
    var supp_id = $("#ddlSupplierNameList").val();
    var CustVal = 0;
    var CustValInBase = 0;
    var GLDetail = [];
    //var suppVal = $("#TotalReturnValue").val();
    var suppVal = $("#TotalReturnValue").val();
    CustValInBase = parseFloat(suppVal).toFixed(ValDecDigit);
    CustVal = (parseFloat(CustValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)
    if (src_type == "H") {
        debugger;
        let AdHocBill_no = $("#AdHocBill_no").val();
        let AdHocBill_dt = $("#AdHocBill_dt").val();
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "Supp", doctype: DocType, Value: CustVal, ValueInBase: CustValInBase
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: AdHocBill_no, bill_date: AdHocBill_dt, acc_id: ""
        });
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var item_id = currentRow.find("#hdItemId").val();
            var ItmGrossVal = currentRow.find("#item_ass_val").val();
            var ItemAccId = currentRow.find("#hdn_item_gl_acc").val()
            var ItmGrossValInBase = currentRow.find("#item_ass_val").val();
            GLDetail.push({
                comp_id: Compid, id: item_id, type: "Itm", doctype: DocType, Value: ItmGrossVal
                , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
                , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: AdHocBill_no, bill_date: AdHocBill_dt, acc_id: ItemAccId
            });
        });
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger
            var currentRow = $(this);
            var tax_id = currentRow.find("#TaxNameID").text();
            var tax_acc_id = currentRow.find("#TaxAccId").text();
            var tax_amt = currentRow.find("#TaxAmount").text();
            if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
                GLDetail.push({
                    comp_id: Compid, id: tax_id, type: "Tax", doctype: DocType, Value: tax_amt
                    , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                    , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: AdHocBill_no, bill_date: AdHocBill_dt, acc_id: ""
                });
            }
        });
        $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var oc_id = currentRow.find("#OC_ID").text();
            var oc_acc_id = currentRow.find("#OCAccId").text();
            var oc_amt = currentRow.find("#OCAmtSp").text();
            var oc_amt_bs = currentRow.find("#OCAmtBs").text();
            var oc_supp_id = currentRow.find("#td_OCSuppID").text();
            var oc_supp_acc_id = currentRow.find("#OCSuppAccId").text();
            var oc_supp_type = currentRow.find("#td_OCSuppType").text();
            var oc_amt_bs_wt = currentRow.find("#OCTotalTaxAmt").text();
            var oc_conv_rate = currentRow.find("#OC_Conv").text();
            var oc_curr_id = currentRow.find("#HdnOC_CurrId").text();
            var oc_supp_bill_no = currentRow.find("#OCBillNo").text();
            var oc_supp_bill_dt = currentRow.find("#OCBillDt").text();
            var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

            var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? supp_acc_id : oc_supp_acc_id;
            //var oc_parent = supp_acc_id; //: oc_supp_acc_id;

            GLDetail.push({
                comp_id: Compid, id: oc_id, type: "OC", doctype: "D", Value: oc_amt
                , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
                , Entity_id: oc_acc_id, curr_id: curr_id
                , conv_rate: oc_conv_rate
                , bill_no: AdHocBill_no
                , bill_date: AdHocBill_dt
            });

            if (oc_supp_id != "" && oc_supp_id != "0") {
                let gl_type = "Supp";
                if (oc_supp_type == "2")
                    gl_type = "Supp";
                if (oc_supp_type == "7")
                    gl_type = "Bank";

                GLDetail.push({
                    comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: "D", Value: oc_amt_wt
                    , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                    , Entity_id: oc_supp_acc_id, curr_id: curr_id
                    , conv_rate: oc_conv_rate, bill_no: AdHocBill_no
                    , bill_date: AdHocBill_dt, acc_id: ""
                });
            }
            else {

            }
        });
        $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var tax_id = currentRow.find("#TaxNameID").text();
            var tax_acc_id = currentRow.find("#TaxAccId").text();
            var tax_amt = currentRow.find("#TaxAmount").text();
            var oc_id = currentRow.find("#TaxItmCode").text();
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "OcTax", doctype: "D", Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: "OcTax", parent: ArrOcGl[0].id
                , Entity_id: tax_acc_id, curr_id: curr_id//ArrOcGl[0].curr_id
                , conv_rate: ArrOcGl[0].conv_rate,
                bill_no: AdHocBill_no, bill_date: AdHocBill_dt, acc_id: ""
            });
        });
    }
    else {
        $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var id = currentRow.find("#id").text();
            var Amt = currentRow.find("#totval").text();

            var type = currentRow.find("#type").text();
            var Entity_id = currentRow.find("#Entity_id").text();
            var acc_id = "";
            if (type == "Itm") {
                acc_id = $("#PReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + id + "']").closest('tr').find("#hdn_item_gl_acc").val();
            }
            GLDetail.push({
                comp_id: Compid, id: id, type: type, doctype: DocType, Value: type == 'Supp' ? suppVal : Amt
                , ValueInBase: type == 'Supp' ? suppVal : Amt, DrAmt: 0, CrAmt: 0
                , TransType: TransType, gl_type: type, parent: type == 'Supp' ? '0' : supp_acc_id, Entity_id: Entity_id
                , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: acc_id
            });
        });
        $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var oc_id = currentRow.find("#OC_ID").text();
            var oc_acc_id = currentRow.find("#OCAccId").text();
            var oc_amt = currentRow.find("#OCAmtSp").text();
            var oc_amt_bs = currentRow.find("#OCAmtBs").text();
            var oc_supp_id = currentRow.find("#td_OCSuppID").text();
            var oc_supp_acc_id = currentRow.find("#OCSuppAccId").text();
            var oc_supp_type = currentRow.find("#td_OCSuppType").text();
            var oc_amt_bs_wt = currentRow.find("#OCTotalTaxAmt").text();
            var oc_conv_rate = currentRow.find("#OC_Conv").text();
            var oc_curr_id = currentRow.find("#HdnOC_CurrId").text();
            var oc_supp_bill_no = currentRow.find("#OCBillNo").text();
            var oc_supp_bill_dt = currentRow.find("#OCBillDt").text();
            var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

            var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? supp_acc_id : oc_supp_acc_id;
            //var oc_parent = supp_acc_id; //: oc_supp_acc_id;

            GLDetail.push({
                comp_id: Compid, id: oc_id, type: "OC", doctype: "D", Value: oc_amt
                , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
                , Entity_id: oc_acc_id, curr_id: curr_id //oc_curr_id
                , conv_rate: oc_conv_rate
                , bill_no: oc_supp_bill_no == "" ? bill_no : oc_supp_bill_no
                , bill_date: oc_supp_bill_dt == "" ? bill_dt : oc_supp_bill_dt, acc_id: ""
            });

            if (oc_supp_id != "" && oc_supp_id != "0") {
                let gl_type = "Supp";
                if (oc_supp_type == "2")
                    gl_type = "Supp";
                if (oc_supp_type == "7")
                    gl_type = "Bank";

                GLDetail.push({
                    comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: "D", Value: oc_amt_wt
                    , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                    , Entity_id: oc_supp_acc_id, curr_id: curr_id //oc_curr_id
                    , conv_rate: oc_conv_rate, bill_no: oc_supp_bill_no
                    , bill_date: oc_supp_bill_dt, acc_id: ""
                });
            }
            else {

            }
        });
        $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var tax_id = currentRow.find("#TaxNameID").text();
            var tax_acc_id = currentRow.find("#TaxAccId").text();
            var tax_amt = currentRow.find("#TaxAmount").text();
            var oc_id = currentRow.find("#TaxItmCode").text();
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "OcTax", doctype: "D", Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: "OcTax", parent: ArrOcGl[0].id
                , Entity_id: tax_acc_id, curr_id: curr_id //ArrOcGl[0].curr_id
                , conv_rate: ArrOcGl[0].conv_rate,
                bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        });
    }
    await $.ajax({
        type: "POST",
        url: "/Common/Common/GetGLDetails1",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ GLDetail: GLDetail }),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                Comn_ErrorPage();
                return false;
            }

            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                var Voudet = 'Y';
                $('#VoucherDetail tbody tr').remove();
                if (arr.Table1.length > 0) {
                    var errors = [];
                    var step = [];
                    for (var i = 0; i < arr.Table1.length; i++) {

                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
                            step.push(parseInt(i));
                            Voudet = 'N';
                        }
                    }
                    var arrayOfErrorsToDisplay = [];
                    $.each(errors, function (i, error) {
                        arrayOfErrorsToDisplay.push({ text: error });
                    });
                    Swal.mixin({
                        confirmButtonText: 'Ok',
                        type: "warning",
                    }).queue(arrayOfErrorsToDisplay)
                        .then((result) => {
                            if (result.value) {

                            }
                        });
                }
                if (Voudet == 'Y') {
                    if (arr.Table.length > 0) {
                        $('#VoucherDetail tbody tr').remove();
                        //let supp_acc_id = $("#supp_acc_id").val();

                        arr.Table = arr.Table.filter(v => (v.type != "RCM"))


                        var arrSupp = arr.Table.filter(v => (v.type == "TSupp" || v.type == "Supp" || v.type == "Bank"));
                        var mainSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "Supp");
                        var TdsSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "TSupp");
                        var NewArrSupp = arrSupp.filter(v => v.acc_id != supp_acc_id);
                        if (TdsSuppGl.length > 0) {
                            NewArrSupp.unshift(TdsSuppGl[0]);
                        }
                        NewArrSupp.unshift(mainSuppGl[0]);
                        arrSupp = NewArrSupp;
                        for (var j = 0; j < arrSupp.length; j++) {
                            let supp_id = arrSupp[j].id;
                            let supp_type = arrSupp[j].type;
                            let supp_bill_no = arrSupp[j].bill_no;
                            let supp_bill_dt = arrSupp[j].bill_dt;
                            let arrDetail;// = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank")));
                            //let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && (v.type == "OC" || v.type == "Itm" || v.type == "Tax" || v.type == "RCM")));
                            let arrDetailDr;
                            if (supp_type == "TSupp") {
                                arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "TSupp")));
                                arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type == "Tds"));
                            } else {
                                arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank") && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
                                //arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
                                arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "OcTax" && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
                            }


                            let RcmValue = 0;
                            let RcmValueInBase = 0;
                            if (supp_type != "TSupp") {
                                arr.Table.filter(v => (v.parent == supp_id && v.type == "RCM"))
                                    .map((res) => {
                                        RcmValue = RcmValue + res.Value;
                                        RcmValueInBase = RcmValueInBase + res.ValueInBase;
                                    });
                            }

                            arrDetail[0].Value = arrDetail[0].Value - RcmValue;//commented by shubham maurya on 08-09-2025
                            arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

                            let rowSpan = 1;//arrDetailDr.length + 1;
                            let GlRowNo = 1;
                            // First Row Generated here for all GL Voucher 
                            let vouType = "";
                            if (arrDetail[0].type == "TSupp")
                                vouType = "DN"
                            if (arrDetail[0].type == "Supp")
                                vouType = "DN"
                           
                            if (arrDetail[0].type == "Bank")
                                vouType = "BP"
                            if (vouType == "DN") {
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
                                    , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
                            } else {
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
                                    , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
                            }

                            let ArrTax = [];
                            let ArrGlDetailsDr = [];
                            for (var k = 0; k < arrDetailDr.length; k++) {
                                //GlRowNo++;
                                // Row Generated here for Other charge and Tax on item
                                let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
                                if (getAccIndex > -1) {
                                    ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
                                    ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
                                } else {
                                    //rowSpan++;
                                    if (arrDetailDr[k].type == "Tax") {
                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
                                        if (getAccIndex > -1) {
                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
                                        } else {
                                            //rowSpan++;
                                            ArrTax.push(arrDetailDr[k]);
                                        }
                                        // ArrTax.push(arrDetailDr[k]);
                                    }
                                    else if (arrDetailDr[k].type == "OcTax") {
                                    }
                                    else {
                                        ArrGlDetailsDr.push(arrDetailDr[k]);
                                    }
                                }



                                if (arrDetailDr[k].type == "OC") {

                                    //let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].id && v.type == "OcTax"));
                                    let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].oc_id && v.type == "OcTax"));
                                    // Grouping the similer tax account
                                    for (var l = 0; l < arrDetailOCDr.length; l++) {

                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
                                        if (getAccIndex > -1) {
                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
                                        } else {
                                            //rowSpan++;
                                            ArrTax.push(arrDetailOCDr[l]);
                                        }
                                    }
                                    //Updating rowspan as Tax on OC added

                                }
                            }
                            var ArrGLDetailRCM = []
                            for (var i = 0; i < ArrGlDetailsDr.length; i++) {

                                if (ArrGlDetailsDr[i].type == "RCM") {
                                    ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

                                } else {
                                    GlRowNo++;
                                    rowSpan++;
                                    if (ArrGlDetailsDr[i].type == "Tds") {
                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
                                            , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
                                        )
                                    } else {
                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
                                            , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
                                        )
                                    }

                                }

                            }
                            for (var i = 0; i < ArrTax.length; i++) {
                                GlRowNo++;
                                rowSpan++;
                                // Row Generated here for Tax on Other Charge
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
                                    , 0, 0, ArrTax[i].Value, ArrTax[i].ValueInBase,  vouType,
                                    ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
                                )

                            }
                            for (var i = 0; i < ArrGLDetailRCM.length; i++) {
                                GlRowNo++;
                                rowSpan++;
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
                                    , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
                                    , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
                                    , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
                                )
                            }
                            $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
                        }
                    }
                }
                BindDDLAccountList();
                CalculateVoucherTotalAmount();
            }
        }
    });

}
function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {

    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }

    let hfSrNo = $('#VoucherDetail tbody tr').length + 1;
    var FieldType = "";
    if (type == 'Itm') {
        FieldType = `<div class="col-sm-11 lpo_form no-padding">
                            <select class="form-control" id="Acc_name_${hfSrNo}" onchange ="OnChangeAccountName(${hfSrNo},event)">
                              </select>
                        </div>`;
        $("#hdnAccID").val(acc_id);
    }
    else {
        FieldType = `${acc_name}`;
    }
    /*--------------------Added by Suraj Maurya on 08-08-2025----------------------*/

    let gl_narr = "";

    try {
        gl_narr = Get_Gl_Narration(VouType, bill_bo, bill_date, type);/*Getting GL Narration from Main Document*/
    }
    catch (ex) {
        console.log(ex);
    }


    /*--------------------Added by Suraj Maurya on 08-08-2025 End----------------------*/
    let Table_tds = `<td id="td_GlSrNo">${GlSrNo}</td>
                                    <td id="td_vou_sr_no" hidden>${SrNo}</td>
                                    <td id="td_GlAccName" class="no-padding">
                                        `+ FieldType + `
                                    </td>
                                    <td id="tdhdn_GlAccId" hidden>${acc_id}</td>
                                    <td class="num_right" id="dramt">${parseFloat(DrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="dramtInBase">${parseFloat(DrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramt">${parseFloat(CrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramtInBase">${parseFloat(CrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="truncate-text" id="gl_narr" contenteditable="true" style="width:350px;padding-right:30px;"><span>${gl_narr}</span></td>
                                    <td class="center" id="td_CC">
                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
                                    </td>
                                <td hidden>
                                    <input type="hidden" id="SNohf" value="${hfSrNo}" />
                                    <input type="hidden" id="type" value="${type}"/>
                                    <input  type="hidden" id="txthfAccID" value="${acc_name}"/>
                                    <input  type="hidden" id="hfAccID"  value="${acc_id}" />
                                    <input  type="hidden" id="glVouType"  value="${VouType}" />
                                    <input  type="hidden" id="gl_curr_id"  value="${curr_id}" />
                                    <input  type="hidden" id="gl_conv_rate"  value="${conv_rate}" />
                                    <input  type="hidden" id="gl_bill_no"  value="${bill_bo}" />
                                    <input  type="hidden" id="gl_bill_date"  value="${bill_date}" />
                                    <input  type="hidden" id="dramt1" value="${parseFloat(DrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="dramtInBase1" value="${parseFloat(DrValueInBase).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramt1" value="${parseFloat(CrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramtInBase1" value="${parseFloat(CrValueInBase).toFixed(ValDecDigit)}"/>
                                </td>`
    if (type == "Supp" || type == "Bank") {
        $('#VoucherDetail tbody').append(`<tr id="tr_GlRow${SrNo}">
                                <td rowspan="${rowSpan}" id="td_SrNo">${SrNo}</td>
                                `+ Table_tds + `
                                <td rowspan="${rowSpan}" id="td_VouType">${VouType}</td>
                                <td rowspan="${rowSpan}" id="td_VouNo"></td>
                                <td rowspan="${rowSpan}" id="td_VouDate"></td>
                                
                            </tr>`)
    } else {
        $('#VoucherDetail tbody').append(`<tr>
                                     `+ Table_tds + `
                                </tr>`)

    }
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    let Narration = "";
    switch (VouType) {
        case "DN":
            Narration = $("#DnNarr").val();
            break;
        default:
            Narration = $("#DnNarr").val();
            break;
    }
    return (Narration).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
async function AddRoundOffGL() {
    var curr_id = $("#hdn_curr_id").val();
    var conv_rate = $("#Hdn_conv_rate").val();
   
    var bill_no = $("#hdn_bill_no").val();
    var bill_dt = $("#hdn_bill_dt").val();
    var ValDecDigit = $("#ValDigit").text();///Amount
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    $("#VoucherDetail >tbody >tr").each(function () {
                        //
                        var currentRow = $(this);
                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
                        }
                    });
                    
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    //        $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    //        <td class="sr_padding">${rowIdx}</td>
                                    //        <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    //        <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
                                    //        <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
                                    //        <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                    //         <input type="hidden" id="type" value="RO"/></td>
                                    //</tr>`);
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });

                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , Diff, DiffInBase, 0, 0, "DN", curr_id
                                        , conv_rate, bill_no, bill_dt)

                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }
                        else {
                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                            var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    //        $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    //        <td class="sr_padding">${rowIdx}</td>
                                    //        <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    //        <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
                                    //        <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
                                    //        <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                    //         <input type="hidden" id="type" value="RO"/></td>
                                    //</tr>`);
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });
                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , 0, 0, Diff, DiffInBase, "DN"
                                        , curr_id, conv_rate, bill_no, bill_dt)
                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }

                    }
                    
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
async function CalculateVoucherTotalAmount() {

    var ValDecDigit = $("#ValDigit").text();
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
        }
    });

    $("#DrTotal").text(DrTotAmt);
    $("#DrTotalInBase").text(DrTotAmtInBase);
    $("#CrTotal").text(CrTotAmt);
    $("#CrTotalInBase").text(CrTotAmtInBase);

    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }
    }

}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "PReturnItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}

function AfterDeleteResetSI_ItemTaxDetail() {
    var ScrapSIItemTbl = "#AdSReturnItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ItemName = "#ItemName";
    CMNAfterDeleteReset_ItemTaxDetailModel(ScrapSIItemTbl, SNohiddenfiled, ItemName);
}
/***---------------------------GL Voucher Entry End-----------------------------***/
function OnChangePRItemQty(e) {
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow);
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#RateDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}
function OnChangePRTItemRate(e) {
    if ($("#chk_roundoff").is(":checked")) {
        $("#chk_roundoff").parent().find(".switchery").trigger("click");

    }
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow,"","RateChange");
}
function CalculationBaseRate(clickedrow, flag,rate) {
    debugger;
    if (rate != "RateChange") {
        if ($("#chk_roundoff").is(":checked")) {
            $("#chk_roundoff").parent().find(".switchery").trigger("click");

        }
    }
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    OrderQty = clickedrow.find("#ReturnQuantity").val();
    ItemName = clickedrow.find("#ItemName" + Sno).val();
    ItmRate = clickedrow.find("#Price").val();

    if (ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate 
            clickedrow.find("#ItemNameError" + Sno).text($("#valueReq").text());
            clickedrow.find("#ItemNameError" + Sno).css("display", "block");
            clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
            clickedrow.find("#Price").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemNameError" + Sno).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate
            clickedrow.find("#ReturnQuantity_Error").text($("#valueReq").text());
            clickedrow.find("#ReturnQuantity_Error").css("display", "block");
            clickedrow.find("#ReturnQuantity").css("border-color", "red");
            clickedrow.find("#ReturnQuantity").val("");
            clickedrow.find("#ReturnQuantity").focus();
            clickedrow.find("#Price").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ReturnQuantity_Error").css("display", "none");
        clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");
        clickedrow.find("#ReturnQuantity").val(parseFloat(OrderQty).toFixed($("#QtyDigit").text()));
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {
        //clickedrow.find("#Price_Error").text($("#valueReq").text());
        //clickedrow.find("#Price_Error").css("display", "block");
        //clickedrow.find("#Price").css("border-color", "red");
        clickedrow.find("#Price").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#Price").focus();
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#Price_Error").css("display", "none");
        clickedrow.find("#Price").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#Price").val("");
        ItmRate = 0;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#ReturnValue").val(FinVal);
        CalculateAmount();
    }
    if (ItmRate == 0) {
        clickedrow.find("#Price").val("");
    }
    else {
        clickedrow.find("#Price").val(parseFloat(ItmRate).toFixed($("#RateDigit").text()));
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    if (OrderQty != 0 && OrderQty != null && OrderQty != "") {
        //click_chkplusminus();
        GetAllGLID();
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    let TaxNonRecovAmt = 0;
    let TaxRecovAmt = 0;
    var src_type = $('#src_type').val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if (TaxItemID == ItmCode) {
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxPercentage = "";
                    TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');
                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    currentRow.find("#TaxAmount ").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount ").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                var TaxRecov = currentRow.find("#TaxRecov").text();
                if (TaxItemID == ItmCode) {
                    if (TaxRecov == "Y") {
                        TaxRecovAmt = parseFloat(CheckNullNumber(TaxRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                    } else {
                        TaxNonRecovAmt = parseFloat(CheckNullNumber(TaxNonRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                    }
                }
                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov });
            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr').each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#ItemName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr');
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                                TaxNonRecovAmt = 0;
                                TaxRecovAmt = 0;
                            }
                            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                            //var GstApplicable = $("#Hdn_GstApplicable").text();
                            //if (GstApplicable == "Y") {
                            //    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                            //        TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit);
                            //        TaxNonRecovAmt = 0;
                            //        TaxRecovAmt = 0;
                            //    }
                            //}
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                //if (currentRow.find("#TaxExempted").is(":checked")) {
                                //    if (parseFloat(ItemTaxAmt) == 0) {
                                //        CRow.remove();
                                //    }
                                //}
                                //else if (GstApplicable == "Y") {
                                //    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                //        if (parseFloat(ItemTaxAmt) == 0) {
                                //            CRow.remove();
                                //        }
                                //    }
                                //}
                                currentRow.find("#item_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                                //var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {//05-02-2025
                                //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                                //}
                                //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                                //}
                                AssessableValue = (parseFloat(currentRow.find("#item_ass_val").val())).toFixed(ValDecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF));
                                //currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                                //currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));
                                currentRow.find("#ReturnValue").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));

                                //let DPIQty = currentRow.find("#ord_qty_spec").val();
                                //debugger
                                //debugger
                                //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                                //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                                //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                                //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ValDecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        //var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                        //    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                        //}
                        //var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        currentRow.find("#ReturnValue").val(GrossAmtOR);
                        //let DPIQty = currentRow.find("#ord_qty_spec").val();
                        //debugger;
                        //debugger;
                        //let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                        //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                        //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                        //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                    }
                }
            });
            CalculateAmount();
            if (src_type == "H") {
                var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
                var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
                $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
            }
            BindTaxAmountDeatils(NewArray);
        }
        else {
            $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#ItemName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    var GrossAmtOR = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ValDecDigit);
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    //var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                    //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                    //    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                    //}
                    //var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                    currentRow.find("#ReturnValue").val(GrossAmtOR);
                    //let DPIQty = currentRow.find("#ord_qty_spec").val();
                    //debugger;
                    //debugger;
                    //let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                    //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                    //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                    //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                }
            });
        }
    }
}
function CalculateAmount() {
    var ReturnValue = parseFloat(0).toFixed(ValDecDigit);
    var src_type = $('#src_type').val();
    if (src_type == "H") {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            if (currentRow.find("#ReturnValue").val() == "" || currentRow.find("#ReturnValue").val() == "NaN") {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(currentRow.find("#ReturnValue").val())).toFixed(ValDecDigit);
            }
        });
        $("#TotalReturnValue").val(ReturnValue);
    }
    else {
        $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            if (currentRow.find("#ReturnValue").val() == "" || currentRow.find("#ReturnValue").val() == "NaN") {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(currentRow.find("#ReturnValue").val())).toFixed(ValDecDigit);
            }
        });
        $("#TotalReturnValue").val(ReturnValue);
    }
}
function OnClickTaxCalculationBtn(e) {
    debugger;
    var SOItemListName = "#ItemName";
    var SNohiddenfiled = "#SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentrow = $(e.target).closest('tr');
        var Itm_ID = currentrow.find("#hdItemId").val();
        var item_gross_val = currentrow.find("#item_ass_val").val();
        //var mr_no = currentrow.find("#QuotationNumber").val();
        //var mr_date = currentrow.find("#QuotationDate").val();
        var RowSNo = currentrow.find("#SNohiddenfiled").val();
        $("#HdnTaxOn").val("Item");
        $("#TaxCalcItemCode").val(Itm_ID);
        $("#Tax_AssessableValue").val(item_gross_val);
        //$("#TaxCalcGRNNo").val(mr_no);
        //$("#TaxCalcGRNDate").val(mr_date);
        $("#HiddenRowSNo").val(RowSNo)

        //if (currentrow.find("#ManualGST").is(":checked")) {
        //    $("#taxTemplate").text("GST Slab");
        //}
        //else {
        //    $("#taxTemplate").text("Template")
        //}
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    //if (GstApplicable == "Y") {
    //    if ($("#Disable").val() == "Y") {
    //        $("#Tax_Template").attr("disabled", true);
    //        $("#SaveAndExitBtn").prop("disabled", true);
    //    }
    //    else {
    //        if (currentrow.find("#ManualGST").is(":checked")) {
    //            $("#Tax_Template").attr("disabled", false);
    //            $("#SaveAndExitBtn").prop("disabled", false);
    //        }
    //        else {
    //            $("#Tax_Template").attr("disabled", true);
    //            $("#SaveAndExitBtn").prop("disabled", true);
    //        }
    //    }
    //}
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}
function OnClickReplicateOnAllItems() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var GRNNo = "";
    var GRNDate = "";
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        //TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "AdSReturnItmDetailsTbl";
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = "";
                GRNDateTbl = "";
                ItemCode = currentRow.find("#hdItemId").val();
                AssessVal = currentRow.find("#item_ass_val").val();


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                for (i = 0; i < TaxCalculationList.length; i++) {
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationList[i].TaxName;
                    var TaxNameID = TaxCalculationList[i].TaxNameID;
                    var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                    TaxPercentage = TaxCalculationList[i].TaxPercentage;
                    var TaxLevel = TaxCalculationList[i].TaxLevel;
                    var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    NewArray.push({ /*UserID: UserID,*/ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            var DocNo = NewArray[k].DocNo;
                            var DocDate = NewArray[k].DocDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            if (CitmTaxItmCode != TaxItmCode) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            //if (CitmTaxItmCode != TaxItmCode) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                            //if (CitmTaxItmCode == TaxItmCode) {
                            //    if (TaxOn != "OC") {
                            //        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //    }
                            //}
                            //if (CitmTaxItmCode != TaxItmCode) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${TaxCalculationListFinalList[i].DocNo}</td>
            <td id="DocDate">${TaxCalculationListFinalList[i].DocDate}</td>
            <td id="TaxItmCode">${TaxCalculationListFinalList[i].TaxItmCode}</td>
            <td id="TaxName">${TaxCalculationListFinalList[i].TaxName}</td>
            <td id="TaxNameID">${TaxCalculationListFinalList[i].TaxNameID}</td>
            <td id="TaxPercentage">${TaxCalculationListFinalList[i].TaxPercentage}</td>
            <td id="TaxLevel">${TaxCalculationListFinalList[i].TaxLevel}</td>
            <td id="TaxApplyOn">${TaxCalculationListFinalList[i].TaxApplyOn}</td>
            <td id="TaxAmount">${TaxCalculationListFinalList[i].TaxAmount}</td>
            <td id="TotalTaxAmount">${TaxCalculationListFinalList[i].TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxCalculationListFinalList[i].TaxApplyOnID}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ItemID = currentRow.find("#hdItemId").val();
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                    var AItemID = TaxCalculationListFinalList[i].TaxItmCode;
                    if (ItemID == AItemID) {
                        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                        AssessableValue = (parseFloat(currentRow.find("#item_ass_val").val())).toFixed(DecDigit);
                        NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit);
                        NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit);
                        FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                        currentRow.find("#ReturnValue").val(FinalNetOrderValueBase);
                    }
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(DecDigit);
                currentRow.find("#item_tax_amt").val(TaxAmt);
                var FGrossAmt = (parseFloat(GrossAmt)).toFixed(DecDigit);
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                currentRow.find("#ReturnValue").val(FinalGrossAmt);
            }
        }
        else {
            var GrossAmt = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(DecDigit);
            currentRow.find("#item_tax_amt").val(TaxAmt);
            var FGrossAmt = (parseFloat(GrossAmt)).toFixed(DecDigit);
            FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
            currentRow.find("#ReturnValue").val(FinalGrossAmt);
        }
    });
    CalculateAmount();
    GetAllGLID();
}
function OnClickSaveAndExit(MGST) {
    debugger;
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);

    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;
    //var TaxOn = $("#HdnTaxOn").val();
    //var TaxOn = "Item";
    var TaxOn = $("#HdnTaxOn").val();
    var tbllenght = $("#TaxCalculatorTbl tbody tr").length
    if (TaxOn == "Item") {
        if (tbllenght == 0) {
            $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
            $("#SpanTax_Template").text($("#valueReq").text());
            $("#SpanTax_Template").css("display", "block");
            $("#ItemCalculate #SaveAndExitBtn").attr("data-dismiss", "");
            return false;
        }
        else {
            $("#ItemCalculate #SaveAndExitBtn").attr("data-dismiss", "modal");
        }
    }
    if (TaxOn == "OC") {
        $("#ItemCalculate #SaveAndExitBtn").attr("data-dismiss", "modal");
    }
    debugger;
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    let NewArr = [];
    var FTaxDetails = $("#" + HdnTaxCalculateTable + " >tbody >tr").length;
    if (FTaxDetails > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();
            var TaxAccId = currentRow.find("#TaxAccId").text();
            if (TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId });
            }
        });
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $('#' + HdnTaxCalculateTable + ' tbody').append(`
                                <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    <td id="TaxRecov">${tax_recov}</td>
                                    <td id="TaxAccId">${TaxAccID}</td>
                                </tr>`)
            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: tax_recov })
        });
    }
    else {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $("#" + HdnTaxCalculateTable + " tbody").append(`
 <tr>
                                      <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    <td id="TaxRecov">${tax_recov}</td>
                                    <td id="TaxAccId">${TaxAccID}</td>
                                </tr>`)
            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: tax_recov })
        });
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {

        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(CheckNullNumber(TotalTaxAmount))).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    }
    else {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var ItmCode = currentRow.find("#hdItemId").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode) {
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                currentRow.find("#item_tax_amt").val(TaxAmt);
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_ass_val").val()))).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt)).toFixed(ValDecDigit);
                currentRow.find("#ReturnValue").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
    }
    
    CalculateAmount();
    if(TaxOn != "OC"){
        GetAllGLID();
    }
    $('#BtnTxtCalculation').css('border', '"#ced4da"');
}
function checkMultiSupplier() {
    return true;
}
function click_chkroundoff() {
    debugger
    if (checkMultiSupplier() == true) {
        if ($("#chk_roundoff").is(":checked")) {
            $("#div_pchkbox").show();
            $("#div_mchkbox").show();
            $("#pm_flagval").val("");
            $("#p_round").prop('checked', false);
            $("#m_round").prop('checked', false);
        }
        else {
            $("#div_pchkbox").hide();
            $("#div_mchkbox").hide();
            $("#pm_flagval").val("");
            $("#p_round").prop('checked', false);
            $("#m_round").prop('checked', false);
            CalculateAmount();
            var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
            var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
            $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
            GetAllGLID();
        }
    } else {
        //for multi supplier
        if ($("#chk_roundoff").is(":checked")) {
            swal("", $("#ManualRoundOffIsNotApplicableForGLHavingMultipleSuppliers").text(), "warning")
            $("#chk_roundoff").attr("checked", false);
        }
    }
}
function click_chkplusminus() {
    debugger;
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    var ValDecDigit = $("#ValDigit").text();
    if ($("#chk_roundoff").is(":checked")) {
        try {
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                $.ajax(
                    {
                        type: "POST",
                        url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                        data: {},
                        success: function (data) {

                            if (data == 'ErrorPage') {
                                PO_ErrorPage();
                                return false;
                            }
                            if (data !== null && data !== "") {
                                var arr = [];
                                arr = JSON.parse(data);
                                if (arr.length > 0) {
                                    if (parseInt(arr[0]["r_acc"]) > 0) {
                                        var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
                                        var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
                                        $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
                                        ApplyRoundOff();
                                    }
                                    else {
                                        swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                        $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                        return false;
                                    }
                                }
                                else {
                                    swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                    $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                    return false;
                                }
                            }
                            else {
                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                return false;
                            }
                        },
                    });
            }
        } catch (err) {
            console.log("Purchase invoice round off Error : " + err.message);
        }
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
        CalculateAmount();
        var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
        var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
        $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
        GetAllGLID();
    }
}
async function addRoundOffToNetValue(flag) {
    var ValDecDigit = $("#ValDigit").text();
    try {
        await $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                data: {},
                success: function (data) {

                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.length > 0) {
                            if (parseInt(arr[0]["r_acc"]) > 0) {
                                if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                                    var TotalReturnValue = $("#TotalReturnValue").val();
                                    //var taxval = $("#TxtTaxAmount").val();
                                    //var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());
                                    var finalnetval = (TotalReturnValue).toFixed(ValDecDigit);
                                    var netval = finalnetval;//$("#NetOrderValueInBase").val();
                                    var fnetval = 0;

                                    if (parseFloat(netval) > 0) {
                                        var finnetval = netval.split('.');
                                        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                                            if ($("#p_round").is(":checked")) {
                                                var decval = '0.' + finnetval[1];
                                                var faddval = 1 - parseFloat(decval);
                                                fnetval = parseFloat(netval) + parseFloat(faddval);
                                                $("#pm_flagval").val($("#p_round").val());
                                            }
                                            if ($("#m_round").is(":checked")) {
                                                var decval = '0.' + finnetval[1];
                                                fnetval = parseFloat(netval) - parseFloat(decval);
                                                $("#pm_flagval").val($("#m_round").val());
                                            }
                                            var roundoff_netval = Math.round(fnetval);
                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                            $("#TotalReturnValue").val(f_netval);
                                            if (flag == "CallByGetAllGL") {
                                                //do not call  GetAllGLID("RO");
                                            } else {
                                                GetAllGLID("RO");
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                return false;
                            }
                        }
                        else {
                            swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                            $("#chk_roundoff").parent().find(".switchery").trigger("click");
                            return false;
                        }
                    }
                    else {
                        swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                        $("#chk_roundoff").parent().find(".switchery").trigger("click");
                        return false;
                    }
                },
            });
    } catch (err) {
        console.log("Purchase invoice round off Error : " + err.message);
    }
}
function ApplyRoundOff(flag) {
    debugger;
    debugger;
    var finalnetval = parseFloat(CheckNullNumber($("#TotalReturnValue").val())).toFixed($("#ValDigit").text());
    var netval = finalnetval;
    var fnetval = 0;
    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if ($("#p_round").is(":checked")) {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
                $("#pm_flagval").val($("#p_round").val());
            }
            if ($("#m_round").is(":checked")) {
                //var finnetval = netval.split('.');
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
                $("#pm_flagval").val($("#m_round").val());
            }
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                $("#TotalReturnValue").val(f_netval);
                if (flag == "CallByGetAllGL") {
                    //do not call  GetAllGLID("RO");
                } else {
                    GetAllGLID("RO");
                }
            }
        }
    }
}
function To_RoundOff(Amount, type) {
    var netval = Amount.toString();
    var fnetval = 0;
    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if (type == "P") {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
            }
            if (type == "M") {
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
            }
            if (type == "P" || type == "M") {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(cmn_ValDecDigit);
                return f_netval;
            }
        }
    }
    return Amount;
}
function OnclickTaxAmtDetailIcon(e) {/*Add by hina sharma on 10-12-2024*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InvoiceNo = $("#hd_doc_no").val();
    var ItmCode = $("#InvoiceItem_id").val();
    var ShipNumber = $("#InvoiceIconGRNNumber").val();
    var ReturnQuantity = $("#InvoiceIconReturnQuantity").val();
    var TaxAmt = $("#TaxAmount").val();
    var src_type = $('#src_type').val();

    var TaxDetail = [];
    TaxDetail = GlTaxDetailForInsight();
    var str_TaxDetail = JSON.stringify(TaxDetail);

    if (TaxAmt != null && TaxAmt != "") {
        try {
            $.ajax({
                async: true,
                type: "POST",
                url: "/ApplicationLayer/PurchaseReturn/GetTaxAmountDetail",
                data: {
                    ItmCode: ItmCode,
                    InvoiceNo: InvoiceNo,
                    ShipNumber: ShipNumber,
                    ReturnQuantity: ReturnQuantity,
                    src_type: src_type,
                    HdGlTaxDetailInsight: str_TaxDetail
                },
                success: function (data) {
                    debugger;
                    $('#TaxAmountDetailsPopup').html(data);
                    //cmn_apply_datatable("#tbl_PaidAmtDetails");
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }
            });
        }
        catch (err) {
            debugger;
            console.log("Trial Balance Error : " + err.message);
        }
    }
}
function GlTaxDetailForInsight() {
    var TaxDetails = [];
    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var tax_rate = currentRow.find("#tax_rate").text();
        var itemid = currentRow.find("#itemid").text();
        var type = currentRow.find("#type").text();
        var Entity_id = currentRow.find("#Entity_id").text();
        var amt = currentRow.find("#amt").text();
        if (type == "Tax") {
            TaxDetails.push({ itemid: itemid, type: type, Entity_id: Entity_id, tax_rate: tax_rate, amt: amt });
        }
    });
    return TaxDetails;
}
//async function Cmn_FetchGLDetails(GLDetail, doc_supp_acc_id,doc_name) {
//    let supp_acc_id = doc_supp_acc_id;
//    await $.ajax({
//        type: "POST",
//        url: "/Common/Common/GetGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({ GLDetail: GLDetail }),
//        success: function (data) {
//            debugger;
//            if (data == 'ErrorPage') {
//                Comn_ErrorPage();
//                return false;
//            }

//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                var Voudet = 'Y';
//                $('#VoucherDetail tbody tr').remove();
//                if (arr.Table1.length > 0) {
//                    var errors = [];
//                    var step = [];
//                    for (var i = 0; i < arr.Table1.length; i++) {

//                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
//                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
//                            step.push(parseInt(i));
//                            Voudet = 'N';
//                        }
//                    }
//                    var arrayOfErrorsToDisplay = [];
//                    $.each(errors, function (i, error) {
//                        arrayOfErrorsToDisplay.push({ text: error });
//                    });
//                    Swal.mixin({
//                        confirmButtonText: 'Ok',
//                        type: "warning",
//                    }).queue(arrayOfErrorsToDisplay)
//                        .then((result) => {
//                            if (result.value) {

//                            }
//                        });
//                }
//                if (Voudet == 'Y') {
//                    if (arr.Table.length > 0) {
//                        $('#VoucherDetail tbody tr').remove();
//                        //let supp_acc_id = $("#supp_acc_id").val();
//                        var arrSupp = arr.Table.filter(v => (v.type == "TSupp" || v.type == "Supp" || v.type == "Bank"));
//                        var mainSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "Supp");
//                        var TdsSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "TSupp");
//                        var NewArrSupp = arrSupp.filter(v => v.acc_id != supp_acc_id);
//                        if (TdsSuppGl.length > 0) {
//                            NewArrSupp.unshift(TdsSuppGl[0]);
//                        }
//                        NewArrSupp.unshift(mainSuppGl[0]);
//                        arrSupp = NewArrSupp;
//                        for (var j = 0; j < arrSupp.length; j++) {
//                            let supp_id = arrSupp[j].id;
//                            let supp_type = arrSupp[j].type;
//                            let supp_bill_no = arrSupp[j].bill_no;
//                            let supp_bill_dt = arrSupp[j].bill_dt;
//                            let arrDetail;// = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank")));
//                            //let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && (v.type == "OC" || v.type == "Itm" || v.type == "Tax" || v.type == "RCM")));
//                            let arrDetailDr;
//                            if (supp_type == "TSupp") {
//                                arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "TSupp")));
//                                arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type == "Tds"));
//                            } else {
//                                arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank") && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
//                                arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "OcTax" && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
//                            }


//                            let RcmValue = 0;
//                            let RcmValueInBase = 0;
//                            if (supp_type != "TSupp") {
//                                arr.Table.filter(v => (v.parent == supp_id && v.type == "RCM"))
//                                    .map((res) => {
//                                        RcmValue = RcmValue + res.Value;
//                                        RcmValueInBase = RcmValueInBase + res.ValueInBase;
//                                    });
//                            }

//                            arrDetail[0].Value = arrDetail[0].Value - RcmValue;
//                            arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

//                            let rowSpan = 1;//arrDetailDr.length + 1;
//                            let GlRowNo = 1;
//                            // First Row Generated here for all GL Voucher 
//                            let vouType = "";
//                            if (arrDetail[0].type == "TSupp")
//                                vouType = "DN"
//                            if (arrDetail[0].type == "Supp")
//                                vouType = "PV"
//                            //if (arrDetail[0].type == "Supp" && $("#DocumentMenuId").val() == "105102150")
//                            //    vouType = "DN"
//                            if (arrDetail[0].type == "Bank")
//                                vouType = "BP"
//                            if (vouType == "DN") {
//                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
//                                    , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
//                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
//                            } else {
//                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
//                                    , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
//                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
//                            }

//                            let ArrTax = [];
//                            let ArrGlDetailsDr = [];
//                            for (var k = 0; k < arrDetailDr.length; k++) {
//                                //GlRowNo++;
//                                // Row Generated here for Other charge and Tax on item
//                                let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
//                                if (getAccIndex > -1) {
//                                    ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
//                                    ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
//                                } else {
//                                    //rowSpan++;
//                                    if (arrDetailDr[k].type == "Tax") {
//                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
//                                        if (getAccIndex > -1) {
//                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
//                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
//                                        } else {
//                                            //rowSpan++;
//                                            ArrTax.push(arrDetailDr[k]);
//                                        }
//                                        // ArrTax.push(arrDetailDr[k]);
//                                    }
//                                    else if (arrDetailDr[k].type == "OcTax") {
//                                    }
//                                    else {
//                                        ArrGlDetailsDr.push(arrDetailDr[k]);
//                                    }
//                                }



//                                if (arrDetailDr[k].type == "OC") {

//                                    let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].id && v.type == "OcTax"));
//                                    // Grouping the similer tax account
//                                    for (var l = 0; l < arrDetailOCDr.length; l++) {

//                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
//                                        if (getAccIndex > -1) {
//                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
//                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
//                                        } else {
//                                            //rowSpan++;
//                                            ArrTax.push(arrDetailOCDr[l]);
//                                        }
//                                    }
//                                    //Updating rowspan as Tax on OC added

//                                }
//                            }
//                            var ArrGLDetailRCM = []
//                            for (var i = 0; i < ArrGlDetailsDr.length; i++) {

//                                if (ArrGlDetailsDr[i].type == "RCM") {
//                                    ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

//                                } else {
//                                    GlRowNo++;
//                                    rowSpan++;
//                                    if (ArrGlDetailsDr[i].type == "Tds") {
//                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
//                                            , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
//                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
//                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
//                                        )
//                                    } else {
//                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
//                                            , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
//                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
//                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
//                                        )
//                                    }

//                                }

//                            }
//                            for (var i = 0; i < ArrTax.length; i++) {
//                                GlRowNo++;
//                                rowSpan++;
//                                // Row Generated here for Tax on Other Charge
//                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
//                                    , ArrTax[i].Value, ArrTax[i].ValueInBase, 0, 0, vouType,
//                                    ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
//                                )

//                            }
//                            for (var i = 0; i < ArrGLDetailRCM.length; i++) {
//                                GlRowNo++;
//                                rowSpan++;
//                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
//                                    , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
//                                    , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
//                                    , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
//                                )
//                            }
//                            $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
//                            $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
//                            $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
//                            $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
//                        }

//                    }
//                }
//                BindDDLAccountList();
//                CalculateVoucherTotalAmount();
//            }
//        }
//    });
//}
function onchangeCancelledRemarks() {
    debugger;
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
function OnClickSaveAndExit_OC_Btn() {
    debugger;
   
    //var status = $("#hdPRTStatus").val();
    //if (status == "D") {
    //    debugger;
    //    $("#PReturnItmDetailsTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        OnChangeRetQty(currentRow, "", "forPageLoad");
    //    });
    //}


    CalculateAmount();
    var NetOrderValueSpe = "#TotalReturnValue";
    var NetOrderValueInBase = "#TotalReturnValue";
    /*var PO_otherChargeId = '#Tbl_OtherChargeList';*//*commented and modify by Hina on 09-07-2025*/
    var PO_otherChargeId = '#PO_OtherCharges';
    CMNOnClickSaveAndExit_OC_Btn(PO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
    click_chkplusminus();
    //GetAllGLID();
}
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
function SetOtherChargeVal() {
    debugger;
}
function Calculate_OC_AmountItemWise() {
    debugger;
}