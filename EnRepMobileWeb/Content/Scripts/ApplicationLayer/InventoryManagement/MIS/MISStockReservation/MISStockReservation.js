$(document).ready(function () {
    $('#ddlItemsList').select2();
   /// $('#ddlItemsGroupList').select2();
   // $('#ddlWarehouseList').select2();
    Cmn_initializeMultiselect(['#ddlItemsGroupList','#ddlWarehouseList']);
});
var QtyDecDigit = $("#QtyDigit").text();
function GetStockReservationReport() {
    debugger;
    var itemId =      $('#ddlItemsList').val();
    //var itemGroupId = $('#ddlItemsGroupList').val();
    //var warehouseId = $('#ddlWarehouseList').val();
    var itemGroupId = cmn_getddldataasstring('#ddlItemsGroupList');
    var warehouseId = cmn_getddldataasstring('#ddlWarehouseList');
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISStockReservation/SearchStockReservation",
        data: {
            itemId: itemId,
            itemGroupId: itemGroupId,
            warehouseId: warehouseId
        },
        success: function (data) {
            /*debugger;*/
            $('#tbodySIList').html(data);
        }
    });
}
function BindItemInfoBtnClick(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdItemId").val();
    ItemInfoBtnClick(itemId);
}
function BindItemInfoBtnClick1(e) {
    debugger;
    var itemId = $('#ddlItemsList').val();
    ItemInfoBtnClick(itemId);
}
function OnClickReservedQtyIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#lblitemname").text();
    var UOMName = clickedrow.find("#lbluom").text();
    var UOMid = clickedrow.find("#hduomid").text();
    var Branch = clickedrow.find("#lblcompname").text();
    var Warehouse = clickedrow.find("#lblwhname").text();
    var ResQty = clickedrow.find("#lblresqty").text();
    var TotStk = clickedrow.find("#lblwhtotstk").text();
    var AvalStk = clickedrow.find("#lblwhavltstk").text();
    var Wh_id = clickedrow.find("#ReshdWhId").val();
    var sub_item = clickedrow.find("#hdmListSubItem").val();
    $("#sub_item").val(sub_item);
    if (sub_item == null) {
        $("#div_SubItemPopupPendResQty").css("display", "");
    } else {
        $("#div_SubItemPopupPendResQty").css("display", "none");
    }
    var poptitle = $("#hdnresval").val();
    $("#SubItemPopupTotalUnResQty").attr("onclick", "return SubItemDetailsPopUp('ListUnResQuantity',event)");
    AddReservedItemDetail(ItmCode, Wh_id, "tblrev", ItemName, UOMName, UOMid, Warehouse, Branch, ResQty, TotStk, AvalStk, poptitle);
}
function AddReservedItemDetail(ItmID, Wh_id, Flag, ItemName, uomName, uomid, WhName, BranchName, RevQty, TotalStk, AvlTotalStk, poptitle) {
    debugger;
    var EntityName = $("#ddlCustomerName option:selected").text();
    var EntityId = $("#ddlCustomerName").val();
    var DocType = $("#Document_Type option:selected").text();
    var DocNo = $("#ddlOrderNumber").val();
    var DocDate = $("#txtdoc_dt").val();
    var DocQty = $("#DocQty").val();
    var DocPendingQty = $("#UnResQty").val();
    if (DocNo == "---Select---") {
        DocNo = "0";
    }

    var BranchID = $("#TopNavBranchList").val();
    var ItemLotBatchdetail = $("#hdres_unresdetails").val();

    var ItemID = ItmID;
    if (ItemID != "" && ItemID != null && Wh_id != "" && Wh_id != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/MISStockReservation/GetReservedItemDetail",
                data: { ItemID: ItmID, wh_id: Wh_id, flag: Flag, entity_id: EntityId, DocNo: DocNo, SelectedItemLotBatchdetail: ItemLotBatchdetail },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#Un_ReservedQtyDetail tbody tr').remove();
                        $('#ReservedQtyDetail tbody tr').remove();
                        var span_SubItemDetail = $("#span_SubItemDetail").val();
                        if (arr.Table.length > 0) {
                            ++rowIdx;
                            var sub_item = $("#sub_item").val();
                            var disablesubitm = "";
                            if (sub_item != "Y") {
                                disablesubitm = "disabled";
                            }
                            HideShowPopupSubItem(sub_item, "");
                            if (Flag == "rev") {
                                $("#Span_ResStkPopup").text(poptitle);
                                $("#ResItemName").val(ItemName.trim());
                                $("#hdn_itemid").val(ItmID.trim());
                                $("#ResUOM").val(uomName.trim());
                                $("#hdn_uom").val(uomid.trim());
                                $("#ResWarehouse").val(WhName.trim());
                                $("#hdn_whid").val(Wh_id.trim());
                                $("#ResBranch").val(BranchName.trim());
                                $("#hdn_branch").val(BranchID.trim());
                                $("#ResEntityName").val(EntityName.trim());
                                $("#ResDocumentType").val(DocType.trim());
                                $("#ResDocumentNumber").val(DocNo.trim());
                                $("#ResDocumentDate").val(DocDate.trim());
                                $("#ResDocumentQuantity").val(DocQty.trim());
                                $("#ResPendingQuantity").val(parseFloat(DocPendingQty.trim()).toFixed(QtyDecDigit));
                                $("#popupReservedQuantity").val(parseFloat(RevQty.trim()).toFixed(QtyDecDigit));
                                $("#popupTotalQuantity").val(parseFloat(TotalStk.trim()).toFixed(QtyDecDigit));
                                $("#popupAvailableQuantity").val(parseFloat(AvlTotalStk.trim()).toFixed(QtyDecDigit));

                                var totalrevqty = 0;
                                for (var i = 0; i < arr.Table.length; i++) {
                                    var resqty = arr.Table[i].res_qty;
                                    if (resqty != "" && resqty != null) {
                                        if (parseFloat(resqty) != parseFloat(0)) {
                                            totalrevqty = parseFloat(totalrevqty) + parseFloat(resqty);
                                        }
                                    }
                                    var rowIdx = 0;
                                    $('#ReservedQtyDetail tbody').append(`<tr id="${rowIdx}">
                                
                                <td><input id="ResLot" value="${arr.Table[i].lotno}" class="form-control" type="text" name="Lot" disabled></td>
                                <td><input id="ResBatch" value="${arr.Table[i].batchno}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="BtMfgName" value="${IsNull(arr.Table[i].mfg_name, '')}" class="form-control remarksmessage" type="text" name="" disabled></td>
                                <td><input id="BtMfgMrp" value="${parseFloat(CheckNullNumber(arr.Table[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}" class="form-control" type="text" name="" disabled></td>
                                <td><input id="BtMfgDate" value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" type="text" name="" disabled></td>
                                <td><input id="ResExDate" value="${arr.Table[i].expirydate}" class="form-control" type="text" name="Batch1" disabled><input id="hdn_ResExDate" value="${arr.Table[i].ex_date}" class="form-control" type="hidden" disabled></td>
                                <td><input id="ResSerial" value="${arr.Table[i].serialno}" class="form-control" type="text" name="Serial1" disabled></td>
                                <td>
                                <div class="col-sm-12 lpo_form no-padding">
                                <input id="ResAvlQty" value="${parseFloat(arr.Table[i].Qty).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="AvlQuantity"  disabled>
                                </div>
                                
                                </td>
                                <td>
                                <div class="col-sm-12 lpo_form no-padding">
                                <input id="ResReservedQty" value="${parseFloat(arr.Table[i].res_qty).toFixed(QtyDecDigit)}" class="form-control num_right" onchange="OnchangeUnres_ResQty(event);" onkeypress="return OnKeyPressUnres_ResQty(this,event);" type="text" name="ReservedQuantity">
                                <span id="resqty_Error" class="error-message is-visible"></span>
                                </div>
                               
                                </td>
                                </tr>`);
                                }
                                $("#TotalResQuantity").text(parseFloat(totalrevqty).toFixed(QtyDecDigit));
                            }
                            else {
                                $("#Span_UnResStkPopup").text(poptitle);
                                $("#UnresItemName").val(ItemName.trim());
                                $("#hdn_Unresitemid").val(ItmID.trim());
                                $("#UnresUOM").val(uomName.trim());
                                $("#hdn_Unresuom").val(uomid.trim());
                                $("#UnresWarehouse").val(WhName.trim());
                                $("#hdn_Unreswhid").val(Wh_id.trim());
                                $("#UnresBranch").val(BranchName.trim());
                                $("#hdn_Unresbranch").val(BranchID.trim());
                                $("#UnrespopupReservedQuantity").val(parseFloat(RevQty.trim()).toFixed(QtyDecDigit));
                                $("#UnrespopupTotalQuantity").val(parseFloat(TotalStk.trim()).toFixed(QtyDecDigit));
                                $("#UnrespopupAvailableQuantity").val(parseFloat(AvlTotalStk.trim()).toFixed(QtyDecDigit));
                                var totalunrevqty = 0;
                                var Doc_type = "";
                                for (var i = 0; i < arr.Table.length; i++) {
                                    var rowIdx = 0;
                                    var unresqty;
                                    if (Flag == "tblrevstk") {
                                        unresqty = arr.Table[i].res_qty;
                                    }
                                    else {
                                        unresqty = arr.Table[i].unres_qty;
                                    }
                                    if (Flag == "tblrev" || Flag == "unrev") {
                                        Doc_type = DocType;
                                    }
                                    else {
                                        Doc_type = arr.Table[i].srcname;
                                    }

                                    if (unresqty != "" && unresqty != null) {
                                        if (parseFloat(unresqty) != parseFloat(0)) {
                                            totalunrevqty = parseFloat(totalunrevqty) + parseFloat(unresqty);
                                        }
                                    }
                                    $('#Un_ReservedQtyDetail tbody').append(`<tr id="${rowIdx}">
                                <td><input id="Unrestransdt" value="${arr.Table[i].trans_date}" class="form-control" type="text" disabled></td>
                                <td class=" ItmNameBreak itmStick tditemfrz"><input id="EntityName" value="${arr.Table[i].CustName}" class="form-control" type="text" name="EntityName" disabled></td>
                                <td><input id="DocumentType" value="${Doc_type}" class="form-control " type="text" name="DocumentType" disabled></td>
                                <td><input id="DocumentNumber" value="${arr.Table[i].srcno}" class="form-control" type="text" name="DocumentNumber" disabled></td>
                                <td><input id="DocumentDate" value="${arr.Table[i].srcdate}" class="form-control" type="text" name="DocumentDate" disabled><input type="hidden" id="hd_docdt" value="${arr.Table[i].src_date}" style="display: none;" /></td>
                                <td><input id="DocumentQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].SrcQty)).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="DocumentQuantity" disabled></td>
                                <td><input id="Lot" value="${arr.Table[i].lotno}" class="form-control" type="text" name="Lot" disabled></td>
                                <td><input id="Batch" value="${arr.Table[i].batchno}" class="form-control" type="text" name="Batch1" disabled></td>
                                <td><input id="BtMfgName" value="${IsNull(arr.Table[i].mfg_name, '')}" class="form-control remarksmessage" type="text" name="" disabled></td>
                                <td><input id="BtMfgMrp" value="${parseFloat(CheckNullNumber(arr.Table[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}" class="form-control" type="text" name="" disabled></td>
                                <td><input id="BtMfgDate" value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" type="text" name="" disabled></td>
                                <td><input id="ExDate" value="${arr.Table[i].expirydate}" class="form-control" type="text" name="Batch1" disabled><input id="hdn_ExDate" value="${arr.Table[i].ex_date}" class="form-control" type="hidden" disabled></td>
                                <td><input id="Serial" value="${arr.Table[i].serialno}" class="form-control" type="text" name="Serial1" disabled></td>
                                <td id="res_qty"><input id="ReservedQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].res_qty)).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="ReservedQuantity"></td>
                                <td id="unres_qty"><div class="lpo_form">
                                <input id="UnreserveQuantity" value="${parseFloat(CheckNullNumber(arr.Table[i].unres_qty)).toFixed(QtyDecDigit)}" class="form-control num_right" onchange="OnchangeUnres_ResQty(event);" onkeypress="return OnKeyPressUnres_ResQty(this,event);" type="text" name="UnreserveQuantity">
                                <span id="unresqty_Error" class="error-message is-visible"></span>
                                 </div></td>
                                </tr>`);
                                }
                                $("#TotalUnResQuantity").text(parseFloat(totalunrevqty).toFixed(QtyDecDigit));
                                if (Flag == "unrev") {
                                    $("#div_Unresfooter").removeAttr("style");
                                }
                                else {
                                    $("#div_Unresfooter").css("display", "none");
                                }

                            }
                        } else {

                            $("#U_SubItemPopupResStock").attr("disabled", true);
                            $("#U_SubItemPopupTotalStock").attr("disabled", true);
                            $("#U_SubItemPopupAvlStock").attr("disabled", true);

                            $("#UnrespopupReservedQuantity").val(null);
                            $("#UnrespopupTotalQuantity").val(null);
                            $("#UnrespopupAvailableQuantity").val(null);
                            $("#UnresItemName").val(null);
                            $("#UnresUOM").val(null);
                            $("#UnresBranch").val(null);
                            $("#UnresWarehouse").val(null);
                        }
                    }
                    $("#Un_ReservedQtyDetail TBODY TR").each(function () {
                        var row = $(this);

                        if (Flag == "tblrev") {
                            $("#thunrevqty").css("display", "none");
                            $("#tdftotal").css("display", "none");
                            row.find("#unres_qty").css("display", "none");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                            var TotResStk = 0;
                            $("#Un_ReservedQtyDetail TBODY TR").each(function () {
                                var row = $(this);
                                var resqty = row.find("#ReservedQuantity").val();
                                if (resqty != "" && resqty != null) {
                                    if (parseFloat(resqty) != parseFloat(0)) {
                                        TotResStk = parseFloat(TotResStk) + parseFloat(resqty);
                                    }
                                }
                            });
                            if (TotResStk != "" && TotResStk != null) {
                                if (parseFloat(TotResStk) != parseFloat(0)) {
                                    $("#TotalUnResQuantity").text(parseFloat(TotResStk).toFixed(QtyDecDigit));
                                }
                                else {
                                    $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
                                }
                            }
                            else {
                                $("#TotalUnResQuantity").text(parseFloat(0).toFixed(QtyDecDigit));
                            }

                        }
                        if (Flag == "tblrevstk") {
                            $("#thunrevqty").css("display", "none");
                            $("#tdftotal").css("display", "none");
                            row.find("#unres_qty").css("display", "none");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                        }

                        if (Flag == "unrev") {
                            $("#thunrevqty").removeAttr("style");
                            $("#tdftotal").removeAttr("style");
                            row.find("#unres_qty").removeAttr("style");

                            $("#threvqty").removeAttr("style");
                            row.find("#res_qty").removeAttr("style");
                            row.find("#ReservedQuantity").attr("readonly", "readonly");

                        }
                    });
                    $(".remarksmessage").attr('onmouseover', 'OnMouseOver(this)');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                }
            });
    } else {
    }
}
function HideShowPopupSubItem(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, "NoRow", "Popup_sub_item", "SubItemPopupDocQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupAvlStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupPendResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalResQty",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupResStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupTotalStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "U_SubItemPopupAvlStock",);
    Cmn_SubItemHideShow(sub_item, "NoRow", "", "SubItemPopupTotalUnResQty",);
}
function SubItemStockDetailsPopUp(flag, e) {
    debugger;
    var ProductNm = $("#ddl_ProductName option:selected").text();
    var ProductId = $("#ddl_ProductName").val();
    var UOM = $("#UOM").val();
    var toWh = $("#toWh").val();
    var Quantity = 0;
    if (flag == "U_AvlStk" || flag == "U_TotalResStk" || flag == "U_TotalStk") {
        ProductNm = $("#UnresItemName").val();
        ProductId = $("#hdn_Unresitemid").val();
        UOM = $("#UnresUOM").val();
        toWh = $("#hdn_Unreswhid").val();
    }
    if (flag == "AvlStk") {

        Quantity = $("#AvailableStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh");
    } else if (flag == "U_AvlStk") {

        Quantity = $("#UnrespopupAvailableQuantity").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh");
    } else if (flag == "TotalResStk") {

        Quantity = $("#ReservedStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whres");
    } else if (flag == "U_TotalResStk") {

        Quantity = $("#UnrespopupReservedQuantity").val();
        //Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whres");
        MISStkRswSubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "whres");
    } else if (flag == "TotalStk") {

        Quantity = $("#TotalStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh_tstk");
    } else if (flag == "U_TotalStk") {

        Quantity = $("#UnrespopupTotalQuantity").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, toWh, Quantity, "wh_tstk");
    }

}

function MISStkRswSubItemWareHouseAvlStock(ProductNm, ProductId, UOM, Wh_id, ItemQty, flag, UomId) {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MISStockReservation/MISStk_GetSubItemWhAvlstockDetails",
        data: {
            Wh_id: Wh_id,
            Item_id: ProductId,
            flag: flag,
            UomId: UomId /*Added by Suraj on 12-01-2024*/
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