$(document).ready(function () {
    DisabledField();
    var Doc_No = $("#txtMaterialReceiptNo").val();
    
    $("#hdDoc_No").val(Doc_No);
    $('#itemtableExternalReceipt').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id);
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        var Itemid = $(this).closest('tr').find("#hfItemID").val();
        updateItemSerialNumber();
        Cmn_DeleteSubItemQtyDetail(Itemid);
        DeleteItemBatchSerialOrderQtyDetails(Itemid);
        var src_type = $("#ddlSourceType").val();
        if (src_type == "A") {
            deletelotbatchserialdetail(Itemid);
        }
        //var status = $("#hdn_Status").val();
        //if (status == "") {
        //    DisablesFieldAdditem();
        //}
    });
    RemoveSessionNew();
    var src_type = $("#ddlSourceType").val();
    if (src_type == "D") {
        BindDLLItemList();
    }
    else {
        //BindSrcDocNumberOnBehalfSrcType();
    }
   // BindDLLItemList();
    WareHouseSearchAble();
    CancelledRemarks("#CancelFlag", "Disabled");
    if (Doc_No !== "" && Doc_No !== null && Doc_No !== "0") {
        var srcDocno = $("#Hdn_src_doc_number").val();
        $("#src_doc_number").val(srcDocno).trigger('change');  // Set the value and trigger the change event
    }
})
function WareHouseSearchAble() {
    $('#itemtableExternalReceipt tbody tr').each(function () {
        var Currentrow = $(this);
        var sno = Currentrow.find("#SNohf").val();
        Currentrow.find("#wh_id" + sno).select2();
    })
}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#itemtableExternalReceipt >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#ItemID").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#SerialItemID").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}
function deletelotbatchserialdetail(Itemid) {
    $("#SaveBatchSerialLotDeatil TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#rcptItemID").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}
function BindDLLItemList() {
    debugger;

    BindItemList("#Itemname_", "1", "#itemtableExternalReceipt", "#SNohf", "", "ExternalReceipt");

}
function OnChangeEntityNameList() {
    debugger;
    var Entity_Name = $("#ddl_EntityName option:selected").val();
    var Entity_Type = $("#ddl_EntityType").val();
    var ddlSourceType = $("#ddlSourceType").val();

    if (Entity_Name == "0" || Entity_Name == "" || Entity_Name == null) {

        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {

            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "Red");
        }
        $('#vmEntityName').text($("#valueReq").text());
        $("#vmEntityName").css("display", "block");
        // ErrorFlag = "Y";

    }
    else {
        if (Entity_Type == "0" || Entity_Type == "" || Entity_Type == null) {
            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "#ced4da");
        }
        $("#vmEntityName").css("display", "none");
        //$("#ddl_EntityName").css("border-color", "#ced4da");
        $("#hd_entity_id").val(Entity_Name);
       
    }
    if (ddlSourceType == "A") {

        BindSrcDocNumberOnBehalfSrcType();
    }
    else {
        $("#DivItemAddItem").css("display", "block");
    }

}
function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();
    $("#ddl_EntityName").val(0).trigger('change');


    if (EntityType == "0" || EntityType == "" || EntityType == null) {
        $("#ddl_EntityType").css("border-color", "Red");
        $('#vmEntityType').text($("#valueReq").text());
        $("#vmEntityType").css("display", "block");
        // ErrorFlag = "Y";
    }
    else {
        debugger;
        $("#vmEntityType").css("display", "none");
        $("#ddl_EntityType").css("border-color", "#ced4da");
        $("#vmEntityName").css("display", "none");
        $("#ddl_EntityName").css("border-color", "#ced4da");
        $("#hd_EntityTypeID").val(EntityType);
        BindSR_SuppCustList_List(EntityType);
    }
}
function BindSR_SuppCustList_List(EntityType) {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();

    $("#ddl_EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlistBind").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType,
                    // source_type: sou_type
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
                    Error_Page();
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
function QtyFloatValueonlyRecQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function QtyFloatValueonlycost(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    return true;
}
function OnKeyupRecQty(e) {

    var currentrow = $(e.target).closest('tr');
    var RecQty = currentrow.find("#ReceivedQuantity").val();
    if (RecQty != "") {
        currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        currentrow.find("#ReceivedQtyError").text("");
        currentrow.find("#ReceivedQtyError").css("display", "none");
    }
}
function OnChangeReceivedQuantity(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var Quantity = currentrow.find("#ReceivedQuantity").val();
    var Qtydigit = $("#QtyDigit").text();
    if (Quantity == "0" || Quantity == "" || Quantity == null) {
        currentrow.find("#ReceivedQuantity").css("border-color", "Red");
        currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
        currentrow.find("#ReceivedQtyError").css("display", "block");
        currentrow.find("#ReceivedQuantity").val("");     
    }
    else {
        currentrow.find("#ReceivedQtyError").css("display", "none");
        currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        var Rec_Quantity = parseFloat(Quantity).toFixed(Qtydigit);
        currentrow.find("#ReceivedQuantity").val(Rec_Quantity);
    }
}
function OnChangeCostPrice(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var Cost_Price = currentrow.find("#CostPrice").val();
    var Ratedigit = $("#RateDigit").text();
    //if (Cost_Price == "0" || Cost_Price == "" || Cost_Price == null) {
    //    currentrow.find("#CostPrice").css("border-color", "Red");
    //    currentrow.find('#CostPriceError').text($("#valueReq").text());
    //    currentrow.find("#CostPriceError").css("display", "block");
    //    currentrow.find("#CostPrice").val("");
    //}
    //else {
        currentrow.find("#CostPriceError").css("display", "none");
        currentrow.find("#CostPrice").css("border-color", "#ced4da");
        var CostPrice = parseFloat(Cost_Price).toFixed(Ratedigit);
        currentrow.find("#CostPrice").val(CostPrice)
    //}
}
function OnChangeItemName(evt, e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    SetnullValueItemonchange(currentrow);
    var sno = currentrow.find("#SNohf").val();
    var item_id = currentrow.find("#Itemname_" + sno).val();
   var Old_itemID= currentrow.find("#hfItemID").val();
    if (Old_itemID == item_id) {
        currentrow.find("#hfItemID").val(item_id);
    }
    else {
        Cmn_DeleteSubItemQtyDetail(Old_itemID);
        DeleteItemBatchSerialOrderQtyDetails(Old_itemID);
        currentrow.find("#hfItemID").val(item_id);
    }
    if (item_id == "0" || item_id == "" || item_id == null) {
        currentrow.find("[aria-labelledby='select2-Itemname_" + sno + "-container']").css("border-color", "Red");
        currentrow.find('#ItemNameError').text($("#valueReq").text());
        currentrow.find("#ItemNameError").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        currentrow.find("#ItemNameError").css("display", "none");
        currentrow.find("[aria-labelledby='select2-Itemname_" + sno + "-container']").css("border-color", "#ced4da");
    }
    Cmn_BindUOM(currentrow, item_id, "", "", "ExternalReceipt")
}
function SetnullValueItemonchange(curr)
{
    var sno = curr.find("#SNohf").val();
    curr.find("#ReceivedQuantity").val("");
    curr.find("#CostPrice").val("");
    curr.find("#wh_id" + sno).val(0).trigger('change');
    curr.find("#wh_Error" + sno).css("display", "none");
    curr.find("#wh_id" + sno).css("border-color", "#ced4da");
    curr.find("#ItemRemarks").text("");
    curr.find("#ItemRemarks").val("");
}
function HideAndShow_BatchSerialButton(row) {
    debugger;
    var b_flag = row.find("#hdi_cbatch").val();
    var s_flag = row.find("#hdi_cserial").val();
    if (b_flag == "Y") {
        row.find("#btncbatchdeatil").prop("disabled", false);
        row.find("#btncbatchdeatil").removeClass("subItmImg");
    }
    else {
        row.find("#btncbatchdeatil").prop("disabled", true);
        row.find("#btncbatchdeatil").addClass("subItmImg");
    }
    if (s_flag == "Y") {
        row.find("#btncserialdeatil").prop("disabled", false);
        row.find("#btncserialdeatil").removeClass("subItmImg");
    }
    else {
        row.find("#btncserialdeatil").prop("disabled", true);
        row.find("#btncserialdeatil").addClass("subItmImg");

    }
}

function BindWarehouseList(id) {
    try {
        debugger;
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ExternalReceipt/GetWarehouseList1",
                data: {},
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        GRN_ErrorPage();
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
                            if (id == null) {
                                $('#itemtableExternalReceipt tbody tr').each(function () {
                                    var row = $(this);
                                    let srNo = row.find("#RowNo").val();
                                    $("#wh_id" + srNo).html(s);
                                });
                            } else {
                                $("#wh_id" + id).html(s);
                            }

                            //$("#wh_id" + id).html(s);

                            //var FItmDetails = JSON.parse(sessionStorage.getItem("GRNItemDetailSession"));
                            //if (FItmDetails != null) {
                            //    if (FItmDetails.length > 0) {
                            //        for (i = 0; i < FItmDetails.length; i++) {
                            //            var Wh_ID = FItmDetails[i].wh_id;
                            //            var Item_ID = FItmDetails[i].item_id;

                            //            $("#itemtableExternalReceipt >tbody >tr").each(function () {
                            //                var currentRow = $(this);
                            //                var SNo = currentRow.find("#RowNo").text();

                            //                var ItmID = $("#hfItemID" + SNo).val();
                            //                if (ItmID == Item_ID) {
                            //                    currentRow.find("#wh_id" + SNo).val(Wh_ID).prop('selected', true);
                            //                }
                            //            });
                            //        }
                            //    }
                            //}
                        }
                    }
                },
            });
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function BindItmList(ID) {
    debugger;

    BindItemList("#Itemname_", ID, "#itemtableExternalReceipt", "#SNohf", "", "ExternalReceipt");

}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function SubItemDetailsPopUp(e,flag) {
    debugger;
    var ddlSourceType = $("#ddlSourceType").val();
    if (ddlSourceType == "D") {
        var NewArr = [];
        var QtyDigit = $("#QtyDigit").text()
        var currentrow = $(e.target).closest('tr');
        var IsDisabled = $("#DisableSubItem").val();

        var recpt_no = $("#txtMaterialReceiptNo").val();
        var recpt_dt = $("#ReceiptDate").val();//.format("yyyy-MM-dd");
        var hd_Status = $("#hdn_Status").val().trim();
        var sno = currentrow.find("#SNohf").val();
        var item_id = currentrow.find("#hfItemID").val();
        var ReceivedQuantity = currentrow.find("#ReceivedQuantity").val();
        var uomName = currentrow.find("#UOM").val();
        var Item_name = currentrow.find("#Itemname_" + sno + " option:selected").text();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + item_id + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            var Qty = row.find('#subItemQty').val();
            if (Qty == "0") {
                Qty = "";
                List.qty = Qty;
            }
            else {
                List.qty = row.find('#subItemQty').val();
            }
            NewArr.push(List);
        });
        if (item_id != null || item_id != "" || item_id != "0") {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ExternalReceipt/GetSubItemDetails",
                data: {
                    item_id: item_id,
                    SubItemListwithPageData: JSON.stringify(NewArr),
                    IsDisabled: IsDisabled,
                    Status: hd_Status,
                    recpt_no: recpt_no,
                    recpt_dt: recpt_dt
                },
                success: function (data) {
                    debugger;
                    $("#SubItemPopUp").html(data);
                    $("#Sub_ProductlName").val(Item_name);
                    $("#Sub_ProductlId").val(item_id);
                    $("#Sub_serialUOM").val(uomName);
                    $("#Sub_Quantity").val(ReceivedQuantity);
                }

            })
        }
    }
    else {
        var NewArr = [];
        var QtyDigit = $("#QtyDigit").text()
        var currentrow = $(e.target).closest('tr');
        var IsDisabled = $("#DisableSubItem").val();

        var recpt_no = $("#txtMaterialReceiptNo").val();
        var srDocno = $("#src_doc_number").val();
        var srDocdt = $("#src_doc_date").val();
        var recpt_dt = $("#ReceiptDate").val();//.format("yyyy-MM-dd");
        var hd_Status = $("#hdn_Status").val().trim();
        var sno = currentrow.find("#SNohf").val();
        var item_id = currentrow.find("#hfItemID").val();
        var ReceivedQuantity = currentrow.find("#ReceivedQuantity").val();
        var IssuedQuantity = currentrow.find("#IssuedQuantity").val();
        var PendingQty = currentrow.find("#PendingQty").val();
        var uomName = currentrow.find("#UOM").val();
        var Item_name = currentrow.find("#Itemname_" + sno).val();
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + item_id + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.IssueQty = row.find('#subItemIssueQty').val();
            List.PendingQty = row.find('#subItemPendQty').val();
            var Qty = row.find('#subItemQty').val();
            if (Qty == "0") {
                Qty = "";
                List.qty = Qty;
            }
            else {
                List.qty = row.find('#subItemQty').val();
            }
            NewArr.push(List);
        });
        if (item_id != null || item_id != "" || item_id != "0") {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ExternalReceipt/AgainstissueGetSubItemDetails",
                data: {
                    item_id: item_id,
                    SubItemListwithPageData: JSON.stringify(NewArr),
                    IsDisabled: IsDisabled,
                    Status: hd_Status,
                    recpt_no: recpt_no,
                    recpt_dt: recpt_dt,
                    srDocno: srDocno,
                    srDocdt: srDocdt,
                    flag: flag
                },
                success: function (data) {
                    debugger;
                    $("#SubItemPopUp").html(data);
                    $("#Sub_ProductlName").val(Item_name);
                    $("#Sub_ProductlId").val(item_id);
                    $("#Sub_serialUOM").val(uomName);
                    if (flag == "Exterissued_qty") {
                        $("#Sub_Quantity").val(IssuedQuantity);
                    }
                    else if (flag == "ExterPendingQty") {
                        $("#Sub_Quantity").val(PendingQty);
                    }
                    else {
                        $("#Sub_Quantity").val(ReceivedQuantity);
                    }
                }

            })
        }
    }
   
}
function onclickcancilflag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#CancelFlag").val(true)
        $("#btn_save").prop('disabled', false);
        $("#btn_save").attr("onclick", "return CheckFormValidation();");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#CancelFlag").val(false);
        $("#btn_save").prop('disabled', true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
};
function ItemStockBatchWise(el, evt)
{
    debugger;
    var QtyDigit = $("#QtyDigit").text()
    var RateDecDigit = $("#RateDigit").text();
    var currentrow = $(evt.target).closest('tr');
    var SNohf = currentrow.find("#SNohf").val();
    var item_id = currentrow.find("#hfItemID").val();
    var ExpireAble_item = currentrow.find("#hfexpiralble").val();
    if (ExpireAble_item == "Y") {
        $("#spanexpiryrequire").show();
    }
    else {
        $("#spanexpiryrequire").hide();
    }
    var uom_name = currentrow.find("#UOM").val();
    var ReceivedQuantity = currentrow.find("#ReceivedQuantity").val();
    var Item_name = currentrow.find("#Itemname_" + SNohf + " option:selected").text();
    var hd_Status = $("#hdn_Status").val();
    var hdn_BatchCommand = $("#hdn_BatchCommand").val();
    var GreyScale = "";

    ResetBatchDetailValExternal();

    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");

    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");

    if (hdn_BatchCommand == "N") {

        $("#BatchResetBtn").css("display", "block")
        $("#BatchSaveAndExitBtn").css("display", "block")
        $("#BatchDiscardAndExitBtn").css("display", "block")
        $("#BatchClosebtn").css("display", "none")


        $("#DivAddNewBatch").css("display", "block")
        $("#txtBatchNumber").attr("disabled", false);
        $("#BatchExpiryDate").attr("disabled", false);
        $("#BatchQuantity").attr("disabled", false);
        $("#BtMfgName").attr("disabled", false);
        $("#BtMfgMrp").attr("disabled", false);
        $("#BtMfgDate").attr("disabled", false);
    }
    else {
        $("#BatchResetBtn").css("display", "none")
        $("#BatchSaveAndExitBtn").css("display", "none")
        $("#BatchDiscardAndExitBtn").css("display", "none")
        $("#BatchClosebtn").css("display", "block")

        GreyScale = "style='filter: grayscale(100%)'";
        $("#DivAddNewBatch").css("display", "none");
        $("#txtBatchNumber").attr("disabled", true);
        $("#BatchExpiryDate").attr("disabled", true);
        $("#BatchQuantity").attr("disabled", true);
        $("#BtMfgName").attr("disabled", true);
        $("#BtMfgMrp").attr("disabled", true);
        $("#BtMfgDate").attr("disabled", true);
    }
    if (item_id != null && item_id != "" && item_id != "0") {
        $("#BatchItemName").val(Item_name);
        $("#BatchItemID").val(item_id);
        $("#hfItemSNo").val(SNohf);
        $("#batchUOM").val(uom_name);
        $("#hfexpiryflag").val(ExpireAble_item);
        if (ReceivedQuantity != "" && parseFloat(ReceivedQuantity) != 0 && ReceivedQuantity != null) {
            $("#BatchReceivedQuantity").val(ReceivedQuantity);
        }
        else {
            $("#BatchReceivedQuantity").val("");
        }
        if (ReceivedQuantity == "NaN" || ReceivedQuantity == "" || ReceivedQuantity == "0") {
            $("#BtnBatchDetail").attr("data-target", "");
            return false;
        }
        else {
            $("#BtnBatchDetail").attr("data-target", "#BatchNumber");
        }
        var batchitemID = $("#BatchItemID").val();

        var rowIdx = 0;      
        var Count =  $("#SaveItemBatchTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                $("#BatchDetailTbl >tbody >tr").remove();           
               
                $("#SaveItemBatchTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    var SUserID = SaveBatchRow.find("#BatchUserID").val();
                    var SRowID = SaveBatchRow.find("#RowSNo").val();
                    var SItemID = SaveBatchRow.find("#ItemID").val();
                    var BatchExDate = SaveBatchRow.find("#BatchExDate").val();
                    var BatchQty = SaveBatchRow.find("#BatchQty").val();
                    var BatchNo = SaveBatchRow.find("#BatchNo").val();
                    let MfgName = SaveBatchRow.find("#bt_mfg_name").text();
                    let MfgMrp = parseFloat(CheckNullNumber(SaveBatchRow.find("#bt_mfg_mrp").val())).toFixed(RateDecDigit);
                    let MfgDate = SaveBatchRow.find("#bt_mfg_date").val();
                   
                    if (SNohf != null && SNohf != "") {
                        if ( SItemID == item_id) {
                            var date = "";
                            if (BatchExDate != "" && BatchExDate != null) {
                                if (BatchExDate == "1900-01-01") {
                                    date = "";
                                }
                                else {
                                    date = moment(BatchExDate).format('DD-MM-YYYY');
                                }
                            }
                            $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                             <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            <td id="BatchNo" >${BatchNo}</td>
                            <td id="bt_MfgName" >${MfgName}</td>
                            <td id="bt_MfgMrp" class="num_right">${MfgMrp}</td>
                            <td id="bt_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                            <td id="bt_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
                            <td id="BatchExDate" hidden="hidden">${BatchExDate}</td>
                            <td>${date}</td>
                            <td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(QtyDigit)}</td>
                            </tr>`);
                        }
                    }
                    else {
                        debugger
                        if (SItemID == item_id) {
                            var date = "";
                            if (BatchExDate != "" && BatchExDate != null) {
                                date = moment(BatchExDate).format('DD-MM-YYYY');
                            }
                            $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                             <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            <td id="BatchNo" >${BatchNo}</td>
                            <td id="bt_MfgName" >${MfgName}</td>
                            <td id="bt_MfgMrp" class="num_right">${MfgMrp}</td>
                            <td id="bt_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                            <td id="bt_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
                            <td id="BatchExDate" hidden="hidden">${BatchExDate}</td>
                            <td>${date}</td>
                            <td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(QtyDigit)}</td>
                            </tr>`);
                        }
                    }
                })              
                if (hdn_BatchCommand == "N") {
                    OnClickDeleteIconExternalReceipt();
                }

                CalculateBatchQtyTblExternalReceipt();
               
                if (hdn_BatchCommand == "Y") {
                    $("#BatchDetailTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        currentRow.find("#BatchDeleteIcon").css("display", "none");
                    });
                }
            }
        }
    }


}
function OnKeyPressBatchNoExter_rec(e) {
    debugger
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

}
function OnKeyDownBatchNoExter_rec(e) {
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeBatchNumber(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    $("#txtBatchNumber").val(data)
}
function OnChangeBatchExpiryDate_External() {
    if ($('#BatchExpiryDate').val() != "") {
        $("#SpanBatchExDate").css("display", "none");
        $("#BatchExpiryDate").css("border-color", "#ced4da");
    }
    else {
        if ($("#hfexpiryflag").val() == 'Y') {
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#valueReq").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
}
function OnChangeBatchQtyExternal() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var BatchReceivedQuantity = $("#BatchQuantity").val();
    if (BatchReceivedQuantity == "" || BatchReceivedQuantity == "0" || BatchReceivedQuantity == null || parseFloat(BatchReceivedQuantity) == parseFloat(0)) {
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
        return false;
    }
    else {      
        $("#SpanBatchQty").css("display", "none");
        $("#BatchQuantity").css("border-color", "#ced4da");

        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {         
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
        }
    }

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }   
    return true;
}
function OnClickAddNewBatchDetailExternalReceipt() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var BatchNumber = $("#txtBatchNumber").val();
    var BatchExpiryDate = $("#BatchExpiryDate").val();
    var BatchReceivedQuantity = $("#BatchReceivedQuantity").val();
    var MfgName = $("#BtMfgName").val();
    let MfgMrp = parseFloat(CheckNullNumber($("#BtMfgMrp").val())).toFixed(RateDecDigit);
    var MfgDate = $("#BtMfgDate").val();
    var rowIdx = 0;
    var ValidInfo = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    if ($('#txtBatchNumber').val() == "") {
        ValidInfo = "Y";
        $("#txtBatchNumber").css("border-color", "Red");
        $('#SpanBatchNo').text($("#valueReq").text());
        $("#SpanBatchNo").css("display", "block");
    }
    else {
        var BatchNo = $('#txtBatchNumber').val().toUpperCase();
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblBtachNo = currentRow.find("#BatchNo").text().toUpperCase();
            if (tblBtachNo == BatchNo) {
                $('#SpanBatchNo').text($("#valueduplicate").text());
                $("#SpanBatchNo").css("display", "block");
                $("#txtBatchNumber").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
    }
    if ($('#BatchExpiryDate').val() == "") {
        if ($("#hfexpiryflag").val() == 'Y') {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#valueReq").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
    var ExDate = $('#BatchExpiryDate').val().split('-');
    if ($('#BatchExpiryDate').val() != "") {
        if (ExDate[0].length > 4) {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatchExDate").css("display", "block");
        }
        var currentdate = moment().format('YYYY-MM-DD');
        if ($('#BatchExpiryDate').val() < currentdate) {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
    var BatchReceived_qty = $("#BatchQuantity").val();
    if (BatchReceived_qty == "" || BatchReceived_qty == null || parseFloat(BatchReceived_qty) == parseFloat(0))
    {
        ValidInfo = "Y";
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
    }
    else {
        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
        }

    }
    if (ValidInfo == "Y") {
        return false;
    }
  
    var date = $("#BatchExpiryDate").val();
    if (date != null && date != "") {
        date = moment(date).format('DD-MM-YYYY');
    }
    else {
        date = "";
    }

    //if (MfgDate != null && MfgDate != "") {
    //    MfgDate = moment(MfgDate).format('DD-MM-YYYY');
    //}
    //else {
    //    MfgDate = "";
    //}
    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
<td id="bt_MfgName" >${MfgName}</td>
<td id="bt_MfgMrp" class="num_right">${MfgMrp}</td>
<td id="bt_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
<td id="bt_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
<td id="BatchExDate" hidden="hidden">${$("#BatchExpiryDate").val()}</td>
<td>${date}</td>
<td id="BatchQty" class="num_right">${parseFloat(CheckNullNumber(BatchReceived_qty)).toFixed(QtyDecDigit)}</td>
</tr>`);
    ResetBatchDetailValExternal("Mfg_DoNotReset");
    CalculateBatchQtyTblExternalReceipt();
    OnClickDeleteIconExternalReceipt();
}
function CalculateBatchQtyTblExternalReceipt() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);   
    $("#BatchDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);

        if (tblQty == null || tblQty == "") {
            tblQty = 0;
        }     
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);      
    });

    $('#BatchQtyTotal').text(parseFloat(TotalReceiQty).toFixed(QtyDecDigit));
}
function OnClickDeleteIconExternalReceipt() {
    $('#BatchDetailTbl tbody').on('click', '.deleteIcon', function () {
        //debugger;
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
        debugger;
        //var SNo = $(this).closest('tr')[0].cells[2].children[0].value;
        //var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        CalculateBatchQtyTblExternalReceipt();

    });
}
function ResetBatchDetailValExternal(flag) {
    $('#BatchQuantity').val("");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
    if (flag != "Mfg_DoNotReset") {
        $("#BtMfgName").val("");
        $("#BtMfgMrp").val("");
        $("#BtMfgDate").val("");
    }
}
function OnClickSaveAndCloseGRN() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ItemID = $('#BatchItemID').val();
    var RowSNo = $("#hfItemSNo").val();
    var UserID = $("#UserID").text();
    var ReceiBQty = parseFloat($("#BatchReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQtyTotal").text()).toFixed(QtyDecDigit);
    if (parseFloat(ReceiBQty) == parseFloat(TotalBQty)) {
        //$("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        //ValidateEyeColor($("#hfItemID").closest("tr"), "btncbatchdeatil", "N");

        


        let NewArr = [];     
        var BatchDetailList = [];
        debugger;
        var SelectedItem = $("#BatchItemID").val();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#ItemID").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });    
            $("#BatchDetailTbl >tbody >tr").each(function () {
                let currentRow = $(this);
                let BatchQty = currentRow.find("#BatchQty").text();
                let BatchNo = currentRow.find("#BatchNo").text();
                let BatchExDate = currentRow.find("#BatchExDate").text();
                let Mfg_Name = currentRow.find("#bt_MfgName").text();
                let Mfg_Mrp = currentRow.find("#bt_MfgMrp").text();
                let Mfg_Date = currentRow.find("#bt_hdn_MfgDate").text();
              
                //$('#SaveItemBatchTbl tbody').append(
                //    `<tr>
                //    //<td><input type="text" id="BatchUserID" value="${UserID}" /></td>
                //    //<td><input type="text" id="RowSNo" value="${RowSNo}" /></td>
                //    <td><input type="text" id="ItemID" value="${ItemID}" /></td>
                //    <td><input type="text" id="BatchNo" value="${BatchNo}" /></td>
                //    <td><input type="text" id="BatchExDate" value="${BatchExDate}" /></td>
                //    <td><input type="text" id="BatchQty" value="${BatchQty}" /></td>
                    
                //</tr>`
                $('#SaveItemBatchTbl tbody').append(
                    `<tr>                
                    <td><input type="text" id="ItemID" value="${ItemID}" /></td>
                    <td><input type="text" id="BatchNo" value="${BatchNo}" /></td>
                    <td><input type="text" id="BatchExDate" value="${BatchExDate}" /></td>
                    <td><input type="text" id="BatchQty" value="${BatchQty}" /></td>
                    <td id="bt_mfg_name">${Mfg_Name}</td>
                    <td><input type="text" id="bt_mfg_mrp" value="${Mfg_Mrp}" /></td>
                    <td><input type="text" id="bt_mfg_date" value="${Mfg_Date}" /></td>                    
                </tr>`
                );
             
            });
         $("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        $("#itemtableExternalReceipt tbody tr #hfItemID[value='" + SelectedItem + "']").closest('tr') // Get the closest row containing the item
            .each(function () {
                var row = $(this);
                ValidateEyeColor(row, "btncbatchdeatil", "N");
            })
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function RemoveSessionNew() {
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
}
function OnClickBatchResetBtnGRN() {
    var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValExternal();
    $('#BatchDetailTbl tbody tr').remove();
    $('#BatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));

    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
}
function OnClickDiscardAndExitGRN() {
    OnClickBatchResetBtnGRN();
}
function HeaderValidation(flag) {
    var ErrorFlag = "N";
    //  var ReceivedFrom= $("#ReceivedFrom").val();
    var CheckedBy = $("#CheckedBy").val();
    var EntityType = $("#ddl_EntityType").val();
    var EntityName = $("#ddl_EntityName").val();
    var SourceType = $("#ddlSourceType").val();


    if (SourceType == "" || SourceType == null || SourceType == "0") {
        $("#ddlSourceType").css("border-color", "Red");
        $('#vmSource_Type').text($("#valueReq").text());
        $("#vmSource_Type").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmSource_Type").css("display", "none");
        $("#ddlSourceType").css("border-color", "#ced4da");
    }
    if (EntityType == "0" || EntityType == "" || EntityType == null) {
        $("#ddl_EntityType").css("border-color", "Red");
        $('#vmEntityType').text($("#valueReq").text());
        $("#vmEntityType").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmEntityType").css("display", "none");
        $("#ddl_EntityType").css("border-color", "#ced4da");
        $("#hd_EntityTypeID").val(EntityType);


    }
    if (EntityName == "0" || EntityName == "" || EntityName == null) {

        if (EntityType == "0" || EntityType == "" || EntityType == null) {
            $("#ddl_EntityName").css("border-color", "Red");
        }
        else {
            $("[aria-labelledby='select2-ddl_EntityName-container']").css("border-color", "Red");
        }
        $('#vmEntityName').text($("#valueReq").text());
        $("#vmEntityName").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmEntityName").css("display", "none");
        $("#ddl_EntityName").css("border-color", "#ced4da");
    }

    if (CheckedBy == "" || CheckedBy == null) {
        $("#CheckedBy").css("border-color", "Red");
        $('#Valid_CheckedBy').text($("#valueReq").text());
        $("#Valid_CheckedBy").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#Valid_CheckedBy").css("display", "none");
        $("#CheckedBy").css("border-color", "#ced4da");
    }
    if (SourceType == "A" && flag == "AddItem") {
        if (src_doc_number == "" || src_doc_number == "0" || src_doc_number == "---Select---") {
            $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
            $("#src_doc_number").css("border-color", "red");
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
            $("#SpanSourceDocNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
            $("#src_doc_number").css("border-color", "#ced4da");
            $("#SpanSourceDocNoErrorMsg").css("display", "none");

        }

    }
    if ($("#CancelFlag").is(":checked")) {
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
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckFormValidation() {
    debugger;
    var ErrorFlag = "N";
    var ReceivedFrom= $("#ReceivedFrom").val();
    var CheckedBy = $("#CheckedBy").val();
    var EntityType = $("#ddl_EntityType").val();
    var EntityName = $("#ddl_EntityName").val();

    var Headervalid = HeaderValidation('SaveDeatil');
    if (Headervalid == false) {
        ErrorFlag = "Y";
        return false;
    }


    if (ErrorFlag == "N") {
        var rowCount = $('#itemtableExternalReceipt >tbody >tr').length
        if (rowCount == 0) {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
        var ItemValidation = ItemDeatilValidation();
        if (ItemValidation == false)
            return false;
        var subitemValidation = Check_subitemValidation();
        if (subitemValidation == false)
            return false;

        if (CheckItemBatchValidation() == false) {
            swal("", $("#PleaseenterBatchDetails").text(), "warning");
            return false;
        }
        if (CheckItemSerialValidation() == false) {
            swal("", $("#PleaseenterSerialDetails").text(), "warning");
            return false;
        }
        var ItemList = new Array;
        var SourceType = $("#ddlSourceType").val();
        $('#itemtableExternalReceipt tbody tr').each(function () {
            var CurrentRow = $(this);
            var arr = {};

            var sno = CurrentRow.find("#SNohf").val();

            arr.itemid = CurrentRow.find("#hfItemID").val();
            arr.UOMID = CurrentRow.find("#UOMID").val();
            arr.ReceivedQuantity = CurrentRow.find("#ReceivedQuantity").val();
            arr.CostPrice = CurrentRow.find("#CostPrice").val();
            arr.wh_id = CurrentRow.find("#wh_id" + sno).val();
            arr.ItemRemarks = CurrentRow.find("#ItemRemarks").val();
            if (SourceType == "A") {
                arr.issued_qty = CurrentRow.find("#IssuedQuantity").val();
                arr.pending_qty = CurrentRow.find("#PendingQty").val();
            }
            else {
                arr.issued_qty = "0";
                arr.pending_qty = "0";
            }
            ItemList.push(arr);
        })
        $("#hdnItemTableDataForSave").val(JSON.stringify(ItemList));

        var SubItemsListArr = Cmn_SubItemList();
        var str3 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str3);
        if (SourceType == "A")
        {
            var LotSerialBatch = new Array;
            $('#SaveBatchSerialLotDeatil tbody tr').each(function () {
                var CurrentRow = $(this);
                var arr = {};

              //  var sno = CurrentRow.find("#SNohf").val();

                var recQty = CurrentRow.find("#rcptRec_qty").val();
                if (recQty != "" && recQty != "0" && parseFloat(recQty) != parseFloat(0)) {
                    arr.itemid = CurrentRow.find("#rcptItemID").val();
                    arr.Lot_id = CurrentRow.find("#rcptLotID").val();
                    arr.BatchSerial = CurrentRow.find("#rcptBatchSerial").val();
                    arr.BatchExDate = CurrentRow.find("#hdnrcptExp_date").val();
                    arr.issued_qty = CurrentRow.find("#rcptIssueQty").val();
                    arr.pending_qty = CurrentRow.find("#rcptPendingQty").val();
                    arr.rec_qty = CurrentRow.find("#rcptRec_qty").val();
                    arr.mfg_name = CurrentRow.find("#bt_mfg_name").text();
                    arr.mfg_mrp = CurrentRow.find("#bt_mfg_mrp").val();
                    arr.mfg_date = CurrentRow.find("#bt_mfg_date").val();

                    LotSerialBatch.push(arr);
                }
              
            })
            $("#hdnSaveBatchSerialLotDeatil").val(JSON.stringify(LotSerialBatch));
        }
        else {
            SaveBatchItemDeatil();
            SaveItemSerialItemDeatil();
        }
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SaveBatchItemDeatil() {
    var Batcharr = new Array;
    $('#SaveItemBatchTbl tbody tr').each(function () {
        var Batch_arr = {};
        var CurrentRow = $(this);
        Batch_arr.itemid = CurrentRow.find("#ItemID").val();
        Batch_arr.BatchNo = CurrentRow.find("#BatchNo").val();
        Batch_arr.BatchExDate = CurrentRow.find("#BatchExDate").val();
        Batch_arr.BatchQty = CurrentRow.find("#BatchQty").val();
        Batch_arr.MfgName = IsNull(CurrentRow.find("#bt_mfg_name").text(), "");
        Batch_arr.MfgMrp = IsNull(CurrentRow.find("#bt_mfg_mrp").val(), "") == "" ? "0" : CurrentRow.find("#bt_mfg_mrp").val();
        Batch_arr.MfgDate = IsNull(CurrentRow.find("#bt_mfg_date").val(), "");
        Batcharr.push(Batch_arr);
    })
    $("#hdn_BatchItemDeatilData").val(JSON.stringify(Batcharr));
}
function SaveItemSerialItemDeatil() {
    var Serialarr = new Array;
    $('#SaveItemSerialTbl tbody tr').each(function () {
        var Serial_arr = {};
        var CurrentRow = $(this);
        Serial_arr.itemid = CurrentRow.find("#SerialItemID").val();
        Serial_arr.SerialNo = CurrentRow.find("#Serial_SerialNo").val();
        Serial_arr.MfgName = IsNull(CurrentRow.find("#sr_mfg_name").text(), "");
        Serial_arr.MfgMrp = IsNull(CurrentRow.find("#sr_mfg_mrp").val(), "") == "" ? "0" : CurrentRow.find("#sr_mfg_mrp").val();
        Serial_arr.MfgDate = IsNull(CurrentRow.find("#sr_mfg_date").val(), "");
        Serialarr.push(Serial_arr);
    })
    $("#hdn_SerialItemDeatilData").val(JSON.stringify(Serialarr));
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    $("#itemtableExternalReceipt >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohf").val();
        var Batchable = currentRow.find("#hdi_cbatch").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ReceivedQuantity").val();
        if (Batchable == "Y") {
           // var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
            var Count = $("#SaveItemBatchTbl tbody tr").length;
            var totalbatchQty = "0";
            if (Count != null) {
             //   var filteredValue = FBatchDetails.filter(function (item) {
                if (Count > 0) {
                    $("#SaveItemBatchTbl tbody tr").each(function () {
                        var Curr = $(this);
                        var batchitemit = Curr.find("#ItemID").val();
                        if (batchitemit == ItemID) {
                            var BatchNo = Curr.find("#BatchNo").val();
                            var BatchQty = Curr.find("#BatchQty").val();
                            totalbatchQty = parseFloat(totalbatchQty) + parseFloat(BatchQty)
                        }

                    })
                    if (totalbatchQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "btncbatchdeatil", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "btncbatchdeatil", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "btncbatchdeatil", "Y");
                    ErrorFlag = "Y";
                }
                  
              //  });
            }
            else {
              
                ValidateEyeColor(currentRow, "btncbatchdeatil", "Y");
                ErrorFlag = "Y";
                //return false;
            }        
        }

    });
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
    var totalserial = "0";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#itemtableExternalReceipt >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohf").text();
        var Serialable = currentRow.find("#hdi_cserial").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ReceivedQuantity").val();
        if (Serialable == "Y") {
            var TotalSQty = parseFloat($("#SaveItemSerialTbl tbody tr #SerialItemID[value='" + ItemID + "']").closest('tr').length).toFixed(QtyDecDigit);
            if (TotalSQty != null) {
                if (parseFloat(TotalSQty) > parseFloat(0)) {
                    if (TotalSQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "btncserialdeatil", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "btncserialdeatil", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "btncserialdeatil", "Y");
                    ErrorFlag = "Y";
                }
              
            }
            else {
                ErrorFlag = "Y";
              
                ValidateEyeColor(currentRow, "btncserialdeatil", "Y");

            }
          
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("itemtableExternalReceipt", "", "hfItemID", "ReceivedQuantity", "SubItemReceivedQty", "N");
}

function Check_subitemValidation() {
    return Cmn_CheckValidations_forSubItems("itemtableExternalReceipt", "", "hfItemID", "ReceivedQuantity", "SubItemReceivedQty", "Y");
}
function ItemDeatilValidation() {
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    var SourceType = $("#ddlSourceType").val();
    $("#itemtableExternalReceipt tbody tr").each(function () {
        var Currentrow = $(this);
        var item_id = Currentrow.find("#hfItemID").val();
        var Sno = Currentrow.find("#SNohf").val();
        var Quantity = Currentrow.find("#ReceivedQuantity").val();
        var CostPrice = Currentrow.find("#CostPrice").val();
        var Warehouse = Currentrow.find("#wh_id" + Sno).val();

        if (item_id == "0" || item_id == "" || item_id == null) {
            Currentrow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border-color", "Red");
            Currentrow.find('#ItemNameError').text($("#valueReq").text());
            Currentrow.find("#ItemNameError").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            Currentrow.find("#ItemNameError").css("display", "none");
            Currentrow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border-color", "#ced4da");
        }
        if (Quantity == "0" || Quantity == "" || Quantity == null || parseFloat(Quantity) == parseFloat(0)) {
            Currentrow.find("#ReceivedQuantity").css("border-color", "Red");
            Currentrow.find('#ReceivedQtyError').text($("#valueReq").text());
            Currentrow.find("#ReceivedQtyError").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            if (SourceType == "A") {
                if ($("#CancelFlag").is(":checked")) {

                }
                else {
                    var PendingQty = Currentrow.find("#PendingQty").val();
                    if (parseFloat(Quantity) > parseFloat(PendingQty)) {
                        var errMessage = $("#ExceedingQty").text();
                        Currentrow.find("#ReceivedQuantity").css("border-color", "red");
                        Currentrow.find("#ReceivedQtyError").text(errMessage);
                        Currentrow.find("#ReceivedQtyError").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        Currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
                        Currentrow.find("#ReceivedQtyError").text("");
                        Currentrow.find("#ReceivedQtyError").css("display", "none");
                    }
                }
               
            }
            else {
                Currentrow.find("#ReceivedQtyError").css("display", "none");
                Currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            }
          
           
        }
        //if (CostPrice == "0" || CostPrice == "" || CostPrice == null || parseFloat(CostPrice) == parseFloat(0)) {
        //    Currentrow.find("#CostPrice").css("border-color", "Red");
        //    Currentrow.find('#CostPriceError').text($("#valueReq").text());
        //    Currentrow.find("#CostPriceError").css("display", "block");
        //    ErrorFlag = "Y";
        //}
        //else {
        //    Currentrow.find("#CostPriceError").css("display", "none");
        //    Currentrow.find("#CostPrice").css("border-color", "#ced4da");
        //}
        if (Warehouse == "0" || Warehouse == "" || Warehouse == null) {
            Currentrow.find("#wh_id" + Sno).css("border-color", "Red");
            Currentrow.find('#wh_Error' + Sno).text($("#valueReq").text());
            Currentrow.find("#wh_Error" + Sno).css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            Currentrow.find("#wh_Error" + Sno).css("display", "none");
            Currentrow.find("#wh_id" + Sno).css("border-color", "#ced4da");
        }
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChnageWarehouse(e) {
    var Currentrow = $(e.target).closest('tr');
    var Sno = Currentrow.find("#SNohf").val();
    var Warehouse = Currentrow.find("#wh_id" + Sno).val();
    if (Warehouse == "0" || Warehouse == "" || Warehouse == null) {
        Currentrow.find("#wh_id" + Sno).css("border-color", "Red");
        Currentrow.find('#wh_Error' + Sno).text($("#valueReq").text());
        Currentrow.find("#wh_Error" + Sno).css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        Currentrow.find("#wh_Error" + Sno).css("display", "none");
        Currentrow.find("#wh_id" + Sno).css("border-color", "#ced4da");
    }
}
function OnchangeReceivedFrom() {
    var ReceivedFrom = $("#ReceivedFrom").val();
    if (ReceivedFrom == "" || ReceivedFrom == null) {
        $("#ReceivedFrom").css("border-color", "Red");
        $('#Valid_ReceivedFrom').text($("#valueReq").text());
        $("#Valid_ReceivedFrom").css("display", "block");
        //ErrorFlag = "Y";
    }
    else {
        $("#Valid_ReceivedFrom").css("display", "none");
        $("#ReceivedFrom").css("border-color", "#ced4da");
    }
}
function OnchangeCheckedBy() {
    var CheckedBy = $("#CheckedBy").val();
    if (CheckedBy == "" || CheckedBy == null) {
        $("#CheckedBy").css("border-color", "Red");
        $('#Valid_CheckedBy').text($("#valueReq").text());
        $("#Valid_CheckedBy").css("display", "block");
      //  ErrorFlag = "Y";
    }
    else {
        $("#Valid_CheckedBy").css("display", "none");
        $("#CheckedBy").css("border-color", "#ced4da");
    }
}
function ItemStockSerialWise(evt, e) {
    var DisableSubItem = $("#DisableSubItem").val();
    if (DisableSubItem == "Y") {
        $("#DivDownloadImportExcelSerial").hide();
    }
    else {
        $("#DivDownloadImportExcelSerial").show();
    }
    $("#SerialNo").val("");
    $("#SrMfgName").val("");
    $("#SrMfgMrp").val("");
    $("#SrMfgDate").val("");
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var currentrow = $(e.target).closest("tr");
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ReceiveQty = 0;
    var GreyScale = "";
    var hdn_BatchCommand = $("#hdn_BatchCommand").val();
    if (hdn_BatchCommand == "N") {
        $("#SerialResetBtn").css("display", "block")
        $("#SerialSaveAndExitBtn").css("display", "block")
        $("#SerialDiscardAndExitBtn").css("display", "block")
        $("#Closebtn").css("display", "none")
        $("#DivAddNewSerial").css("display", "block")
        $("#SerialNo").attr("disabled", false);
        $("#SrMfgName").attr("disabled", false);
        $("#SrMfgMrp").attr("disabled", false);
        $("#SrMfgDate").attr("disabled", false);

    }
    else {
        GreyScale = "style='filter: grayscale(100%)'";
        $("#DivAddNewSerial").css("display", "none");
        $("#SerialNo").attr("disabled", true);
        $("#SrMfgName").attr("disabled", true);
        $("#SrMfgMrp").attr("disabled", true);
        $("#SrMfgDate").attr("disabled", true);

        $("#SerialResetBtn").css("display", "none")
        $("#SerialSaveAndExitBtn").css("display", "none")
        $("#SerialDiscardAndExitBtn").css("display", "none")
        $("#Closebtn").css("display", "block")
    }
    var SNohf = currentrow.find("#SNohf").val();
     ItemID = currentrow.find("#hfItemID").val();
    var ExpireAble_item = currentrow.find("#hfexpiralble").val();
     ItemUOM = currentrow.find("#UOM").val();
     ReceiveQty = currentrow.find("#ReceivedQuantity").val();
     ItemName = currentrow.find("#Itemname_" + SNohf + " option:selected").text();
    var hd_Status = $("#hdn_Status").val();
    var ReceQty = parseFloat(ReceiveQty).toFixed(QtyDecDigit);

    if (ReceQty == "NaN" || ReceQty == "" || ReceQty == "0") {
        $("#BtnSerialDetail").attr("data-target", "");
        return false;
    }
    else {
        $("#BtnSerialDetail").attr("data-target", "#SerialDetail");
    }
    $("#SerialItemName").val(ItemName);
    $("#serialUOM").val(ItemUOM);
    $("#SerialReceivedQuantity").val(ReceQty);
    $("#hfSItemSNo").val(SNohf);
    $("#hfSItemID").val(ItemID);
    var rowIdx = 0;

   // var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    var Count = $("#SaveItemSerialTbl >tbody >tr").length;

    if (Count != null) {
        if (Count > 0)
        {
            $("#SerialDetailTbl >tbody >tr").remove();

                   
           
            $("#SaveItemSerialTbl >tbody >tr").each(function () {
                var SerialCurrentRow = $(this);
                var SUserID = SerialCurrentRow.find("#SerialUserID").val();
                var SRowID = SerialCurrentRow.find("#SerialRowSNo").val();
                var SItemID = SerialCurrentRow.find("#SerialItemID").val();
                var SerialNo = SerialCurrentRow.find("#Serial_SerialNo").val();
                let MfgName = SerialCurrentRow.find("#sr_mfg_name").text();
                let MfgMrp = parseFloat(CheckNullNumber(SerialCurrentRow.find("#sr_mfg_mrp").val())).toFixed(RateDecDigit);
                let MfgDate = SerialCurrentRow.find("#sr_mfg_date").val();
                //let mfg_date = "";
                //if (MfgDate != "" && MfgDate != null) {
                //    if (MfgDate == "1900-01-01") {
                //        mfg_date = "";
                //    }
                //    else {
                //        mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                //    }
                //}
                if (SNohf != null && SNohf != "") {
                    if ( SItemID == ItemID) {
                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">

 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                           <td id="SerialID" >${rowIdx}</td>
                            <td id="SerialNo" >${SerialNo}</td>
                            <td id="sr_MfgName" >${MfgName}</td>
                            <td id="sr_MfgMrp" class="num_right">${MfgMrp}</td>
                            <td id="sr_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                            <td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
                             </tr>`);
                    }
                    else {
                        if (SItemID == ItemID) {
                            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
<td class="red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                              <td id="SerialID" >${rowIdx}</td>
                               <td id="SerialNo" >${SerialNo}</td>
                            <td id="sr_MfgName" >${MfgName}</td>
                            <td id="sr_MfgMrp" class="num_right">${MfgMrp}</td>
                            <td id="sr_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                            <td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
                               </tr>`);
                        }
                    }
                }
            })
          
            if (hdn_BatchCommand == "N") {
                OnClickSerialDeleteIcon_ExternalReceipt();
            }
            if (hdn_BatchCommand == "Y") {
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#SerialDeleteIcon").css("display", "none");
                });
            }
        }
        else {
            $("#SerialDetailTbl >tbody >tr").remove();
        }
        
    }
    else {
        $("#SerialDetailTbl >tbody >tr").remove();
    }
}
function OnClickAddNewSerialDetail_ExternalReceipt()
{
    debugger;
  

    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var AcceptLength = parseInt(0);
    $("#SerialDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);            
            AcceptLength = parseInt(AcceptLength) + 1;           
    });
    if ($('#SerialNo').val() == "") {
        ValidInfo = "Y";
        $("#SerialNo").css("border-color", "Red");
        $('#SpanSerialNo').text($("#valueReq").text());
        $("#SpanSerialNo").css("display", "block");
    }
    else {
        $("#SpanSerialNo").css("display", "none");
        $("#SerialNo").css("border-color", "#ced4da");

        var SerialNo = $('#SerialNo').val().toUpperCase();
        $("#SerialDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblSerialNo = currentRow.find("#SerialNo").text().toUpperCase();
            if (tblSerialNo == SerialNo) {
                $('#SpanSerialNo').text($("#valueduplicate").text());
                $("#SpanSerialNo").css("display", "block");
                $("#SerialNo").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
       
    }
    if (ValidInfo == "Y") {
        return false;
    }
    else {
        if (parseInt(AcceptLength) != parseInt(ReceiSQty)) {
            let MfgName = $("#SrMfgName").val();
            let MfgMrp = parseFloat(CheckNullNumber($("#SrMfgMrp").val())).toFixed(RateDecDigit);
            let MfgDate = $("#SrMfgDate").val();
            //let mfg_date = "";
            //if (MfgDate != null && MfgDate != "") {
            //    mfg_date = moment(MfgDate).format('DD-MM-YYYY');
            //}
            //else {
            //    mfg_date = "";
            //}
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
                <td id="SerialID" >${TblLen + 1}</td>
                <td id="SerialNo" >${$("#SerialNo").val()}</td>
                <td id="sr_MfgName" >${MfgName}</td>
                <td id="sr_MfgMrp" class="num_right">${MfgMrp}</td>
                <td id="sr_MfgDate" >${Cmn_FormatDate_ddmmyyyy(MfgDate)}</td>
                <td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);

            $('#SerialNo').val("");
            OnClickSerialDeleteIcon_ExternalReceipt();
        }
    }
}
function OnClickSerialDeleteIcon_ExternalReceipt() {
    $('#SerialDetailTbl tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        ResetSerialNoAfterDelete_ER();
    });
}
function ResetSerialNoAfterDelete_ER() {
    debugger;
    var rowIdx = 0;
    $("#SerialDetailTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        currentRow.find("#SerialID").text(++rowIdx);
    });
}
function OnClickSerialSaveAndClose_ExternalReceipt() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();

    var ItemID = $('#hfSItemID').val();
    var RowSNo = $("#hfSItemSNo").val();
    var UserID = $("#UserID").text();

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalRecQty = parseFloat(ReceiSQty);
    var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);
    if (parseFloat(TotalRecQty) == parseFloat(TotalSQty)) {

       
        let NewArr = [];      
        var Count = $("#SaveItemSerialTbl >tbody >tr").length;
        var SelectedItem = $("#hfSItemID").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#SerialItemID").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
          //  $("#SaveItemSerialTbl  >tbody >tr ").remove();
            var SerialDetailList = [];
            $("#SerialDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var SerialNo = currentRow.find("#SerialNo").text();
                var QtyType = currentRow.find("#QtyType").text();
                let MfgName = currentRow.find("#sr_MfgName").text();
                let MfgMrp = currentRow.find("#sr_MfgMrp").text();
                let MfgDate = currentRow.find("#sr_hdn_MfgDate").text();
                $('#SaveItemSerialTbl tbody').append(`<tr>                  
                   <td><input type="text" id="SerialItemID" value="${ItemID}" /></td>
                   <td><input type="text" id="Serial_SerialNo" value="${SerialNo}" /></td>
                   <td id="sr_mfg_name">${MfgName}</td>
                   <td><input type="text" id="sr_mfg_mrp" value="${MfgMrp}" /></td>
                   <td><input type="text" id="sr_mfg_date" value="${MfgDate}" /></td>
                   </tr>`
                );
              
            });

        $("#SerialSaveAndExitBtn").attr("data-dismiss", "modal");
      
        $("#itemtableExternalReceipt tbody tr #hfItemID[value='" + SelectedItem + "']").closest('tr') // Get the closest row containing the item
            .each(function () {
                var row = $(this);
                ValidateEyeColor(row, "btncserialdeatil", "N");
            })
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }

}

function OnClickSerialResetBtn_ExternalReceipt() {
    $('#SerialNo').val("");
    $('#SerialDetailTbl tbody tr').remove();
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDiscardAndExit_ExternalReceipt() {
    OnClickSerialResetBtnGRN();
}
function OnKeyPressSerialNo_ExternalReceipt() {
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#txtMaterialReceiptNo").val();
    DocDate = $("#ReceiptDate").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (DocNo + ',' + DocDate + ',' + WF_Status1);
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    //var pdfAlertEmailFilePath = 'BOM_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath);

    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.href = "/ApplicationLayer/ExternalReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ExternalReceipt/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
           // Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ExternalReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            //await Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ExternalReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var ExtRcpDt = $("#ReceiptDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: ExtRcpDt
        },
        success: function (data) {
            /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var OrderStatus = "";
                OrderStatus = $('#hdn_Status').val().trim();
                if (OrderStatus === "D" || OrderStatus === "F") {

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
                /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txtMaterialReceiptNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
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
        debugger;
        if (isConfirm) {
            debugger;
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            RemoveSessionNew();
            return true;
        } else {
            return false;
        }
    });
    return false;
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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "itemtableExternalReceipt", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}
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

function OnChangeSourceType_List() {
    debugger;
    var ddlSourceType = $("#ddlSourceType").val();
    $("#hd_SourceTypeID").val(ddlSourceType);
    if (ddlSourceType == "0" || ddlSourceType == "" || ddlSourceType == null) {
        $("#ddlSourceType").css("border-color", "Red");
        $('#vmSource_Type').text($("#valueReq").text());
        $("#vmSource_Type").css("display", "block");
    }
    else {
        $("#vmSource_Type").css("display", "none");
        $("#ddlSourceType").css("border-color", "#ced4da");

    }
    if (ddlSourceType == "A") {
        $("#itemtableExternalReceipt tbody tr").remove();
        $("#DivItemAddItem").css("display", "none");
        $("#Div_CheckedBy").addClass("pl-0");
        $("#res_issueQty").attr("hidden", false);
        $("#res_pendingQty").attr("hidden", false);
        $("#res_pendingQty").attr("hidden", false);
        $("#res_Lot").attr("hidden", true);
        $("#res_Batch").attr("hidden", true);
        $("#res_Serial").attr("hidden", true);
    }
    else {
        if (ddlSourceType == "0" || ddlSourceType == "" || ddlSourceType == null) {
            $("#DivItemAddItem").css("display", "none");
            $("#itemtableExternalReceipt tbody tr").remove();
        }
        else {
            $("#Div_CheckedBy").removeClass("pl-0");
             //$("#DivItemAddItem").css("display", "block");
            $("#res_issueQty").attr("hidden", true);
            $("#res_pendingQty").attr("hidden", true);
            $("#res_Lot").attr("hidden", false);
            $("#res_Batch").attr("hidden", false);
            $("#res_Serial").attr("hidden", false);
        }
        
    }
    $("#ddl_EntityType").val(0).trigger('change');
    $("#vmEntityType").css("display", "none");
    $("#ddl_EntityType").css("border-color", "#ced4da");
    $("#vmEntityName").css("display", "none");
    $("#ddl_EntityName").css("border-color", "#ced4da");
    DisabledField();

}
function DisabledField() {
    var ddlSourceType = $("#ddlSourceType").val();
    if (ddlSourceType == "A") {
        $("#DivSrcDocNo").css("display", "block");
        $("#DivDocumentDate").css("display", "block");
    }
    else {
        $("#DivSrcDocNo").css("display", "none");
        $("#DivDocumentDate").css("display", "none");
    }
}
function BindSrcDocNumberOnBehalfSrcType() {
    debugger;
    var SuppID = $("#ddl_EntityName option:selected").val();

    var entity_type = $('#ddl_EntityType option:selected').val();
    if (SuppID != null && SuppID != "" && SuppID != "0" && entity_type != null && entity_type != "" && entity_type != "0") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/ExternalReceipt/GetSourceDocList',
            data: { SuppID: SuppID, entity_type: entity_type },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        $("#src_doc_number option").remove();
                        $("#src_doc_number optgroup").remove();
                        $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl1" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl1').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#src_doc_number').select2({
                            templateResult: function (data) {
                                debugger;
                                var list = [];
                                //$("#SaveSrcDocDeatil >tbody >tr").each(function (i, row) {
                                //    debugger;
                                //    var currentRow = $(this);
                                //    var Suppid = "";
                                //    // var rowin = currentRow.find('#SpanRowId').text();
                                //    src_docno = currentRow.find('#rcptDocNo').val();
                                //    list.push(src_docno);
                                //});
                                if (list.includes(data.id) == false) {
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
                                }


                                firstEmptySelect = false;
                            }
                        });

                        $("#src_doc_date").val("");

                    }
                }
            }

        })
    }
}
function OnchangeSrcDocNumber() {
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    
    debugger
   
    if (doc_no != 0) {
        var doc_Date = $("#src_doc_number option:selected")[0].dataset.date
        var newdate = doc_Date.split("-").reverse().join("-");

        $("#src_doc_date").val(newdate);
        $("#src_doc_number").val(doc_no);
        $("#Hdn_src_doc_number").val(doc_no);


    }
    if (doc_no == "" || doc_no == "0" || doc_no == "---Select---") {
        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
        $("#src_doc_number").css("border-color", "red");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css("display", "block");
        // ErrorFlag = "Y";
        $("#DivAddHeaderPlusIcon").css("display", "none");
        $("#ddlSourceType").attr("disabled", false);
        $("#ddl_EntityType").attr("disabled", false);
        $("#ddl_EntityName").attr("disabled", false);
    }
    else {
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
        $("#src_doc_number").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
        $("#DivAddHeaderPlusIcon").css("display", "block");
        $("#ddlSourceType").attr("disabled", true);
        $("#ddl_EntityType").attr("disabled", true);
        $("#ddl_EntityName").attr("disabled", true);

    }
}
function AddAttribute() {
    debugger;
    var Header = HeaderValidation("AddItem");
    if (Header == true) {
        AddNewRow();
    }

}
function AddNewRow() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var SourceType = $("#ddlSourceType").val();
    var rowIdx = 0;
    var rowCount = $("#itemtableExternalReceipt > tbody > tr").length + 1;
    var RowNo = 0;

    if (RowNo == "0") {
        RowNo = 1;
    }
    $("#itemtableExternalReceipt >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (SourceType == "D") {
        $("#itemtableExternalReceipt tbody").append(`<tr id="R${++rowIdx}">
                                                 <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
                                                            <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class="col-sm-11 lpo_form no-padding">
                                                                    <select class="form-control" id="Itemname_${RowNo}" name="Itemname" onchange="OnChangeItemName(this,event)">
                                                                    </select>
                                                                    <input type="hidden" id="hfItemID" value="" />
                                                                    <input type="hidden" id="hfexpiralble" value=""/>
                                                                    <span id="ItemNameError" class="error-message is-visible"></span>
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" class="calculator subItmImg" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" onclick="OnClickIconBtn(event);" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly">
                                                                <input type="hidden" id="UOMID" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                    <input id="ReceivedQuantity" class="form-control num_right" autocomplete="off" name="Quantity" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeReceivedQuantity(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'">
                                                                    <span id="ReceivedQtyError" class="error-message is-visible"></span>
                                                                </div>
                                                                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReceivedQty">
                                                                    <button type="button" id="SubItemReceivedQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp(event,'Direct')" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                    <input type="hidden" id="sub_item" value="N" style="display: none;">
                                                                </div>
                                                            </td>
                                                            <td class="lpo_form">

                                                                <input id="CostPrice" class="form-control num_right"  autocomplete="off" name="CostPrice" placeholder="0000.00" onkeypress="return QtyFloatValueonlycost(this,event);" onpaste="return CopyPasteData(event);" type="text" onchange="OnChangeCostPrice(event);">
                                                                <span id="CostPriceError" class="error-message is-visible"></span>
                                                            </td>
                                                              <td width="7%"><div class="lpo_form">
                                                              <select class="form-control" id="wh_id${RowNo}" onchange ="OnChnageWarehouse(event)"><option value="0">---Select---</option></select>
                                                              <input type="hidden" id="Hdn_GRN_WhName" value='' style="display: none;" />
                                                              <span id="wh_Error${RowNo}" class="error-message is-visible"></span></div></td>
                                                            <td>
                                                                <input id="Lot" value="" class="form-control"  maxlength="25" autocomplete="off" type="text" name="Lot" placeholder="${$("#span_Lot_has").text()}" disabled="disabled">
                                                            </td>
                                                            <td class="center">
                                                                <button type="button" id="btncbatchdeatil"  onclick="ItemStockBatchWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#ExternalReceiptBatchNumber" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>

                                                                <input type="hidden" id="hdi_cbatch" value="N" style="display: none;">
                                                            </td>
                                                            <td class="center">
                                                                <button type="button" id="btncserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>

                                                                <input type="hidden" id="hdi_cserial" value="N" style="display: none;">
                                                            </td>
                                                            <td>
                                                                <textarea id="ItemRemarks" class="form-control remarksmessage" onmouseover="OnMouseOver(this.value)" autocomplete="off" name="ItemRemarks" maxlength="300" placeholder="${$("#span_remarks").text()}"></textarea>
                                                            </td>
                                                            <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
      </tr>`)
        BindItmList(RowNo);
        BindWarehouseList(RowNo);
        WareHouseSearchAble();
    }
    else {
        var entity_type = $("#ddl_EntityType").val();
        var entity_Name = $("#ddl_EntityName").val();
        var Doc_no = $("#src_doc_number").val();
        var Doc_dt = $("#src_doc_date").val();
        var hdn_Status = $("#hdn_Status").val();
        var rcpt_no = $("#txtMaterialReceiptNo").val();
        var rcpt_dt = $("#ReceiptDate").val();

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ExternalReceipt/BindItemTable",
            data: {
                entity_type: entity_type,
                entity_Name: entity_Name,
                Doc_no: Doc_no,
                Doc_dt: Doc_dt,
                hdn_Status: hdn_Status,
                rcpt_no: rcpt_no,
                rcpt_dt: rcpt_dt
            },
            success: function (data) {
                debugger;
                if (data && data !== "") {
                    $("#DivAddHeaderPlusIcon").hide();

                    const arr = JSON.parse(data);
                    const QtyDecDigit = parseInt($("#QtyDigit").text()) || 2;
                    const RateDigit = parseInt($("#RateDigit").text()) || 2;

                    const $itemTableBody = $("#itemtableExternalReceipt tbody");
                    const $lotTableBody = $("#SaveBatchSerialLotDeatil tbody");
                    const $SubItemTableBody = $("#hdn_Sub_ItemDetailTbl tbody");

                    $itemTableBody.empty();
                    $lotTableBody.empty();
                    $SubItemTableBody.empty();

                    // ===================== TABLE 0 (ITEM DETAILS) =====================
                    if (arr.Table && arr.Table.length > 0) {
                       // const rows = arr.Table[0]; // ✅ your actual data rows array

                        for (let i = 0; i < arr.Table.length; i++) {
                            const row = arr.Table[i];
                            const rowCount = $itemTableBody.find("tr").length + 1; // serial no
                            const RowNo = rowCount; // you can also use i+1

                            const ItemID = row.item_id || '';
                            const ItemName = row.item_name || '';
                            const UOM_ID = row.uom_id || '';
                            const UOM_Alias = row.uom_alias || '';
                            const IssuedQty = parseFloat(row.Issue_qty || 0).toFixed(QtyDecDigit);
                            const PendingQty = parseFloat(row.Pending_qty || 0).toFixed(QtyDecDigit);
                            const cost_pr = parseFloat(row.cost_pr || 0).toFixed(RateDigit);
                            const wh_name = row.wh_name;
                            const sub_item = row.sub_item;
                            const wh_id = row.wh_id;
                            const LotID = row.Lot_id || '';

                            var DisabledSubitm = "";
                            if (sub_item == "N") {
                                DisabledSubitm = "disabled";
                            }

                            // ✅ Append Row
                            $itemTableBody.append(`
            <tr id="R${RowNo}">
                <td class="red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="Delete"></i></td>
                <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>

                <td class="ItmNameBreak itmStick tditemfrz">
                    <div class="col-sm-11 lpo_form" style="padding:0px;" id="multiWrapper">
                        <input id="Itemname_${RowNo}" value="${ItemName}" class="form-control" type="text" disabled title="${ItemName}">
                        <input type="hidden" id="hfItemID" value="${ItemID}" />
                       <input type="hidden" id="sub_item" value="${sub_item}" style="display: none;">
                    </div>
                    <div class="col-sm-1 i_Icon">
                        <button type="button" class="calculator item_pop" data-toggle="modal"
                            onclick="OnClickIconBtn(event);" data-target="#ItemInfo">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
                        </button>
                    </div>
                </td>

                <td>
                    <input id="UOM" class="form-control" value="${UOM_Alias}" type="text" readonly>
                    <input type="hidden" id="UOMID" value="${UOM_ID}">
                </td>

                <td>
                    <div class="col-sm-8 no-padding">
                        <input id="IssuedQuantity" value="${IssuedQty}" class="form-control num_right" readonly>
                    </div>
                    <div class="col-sm-2 i_Icon">
                        <button type="button" class="calculator ItmInfoBtnIcon"
                            onclick="OnClickItemOrderQtyIconBtn(event,'issued_qty');"
                            data-toggle="modal" data-target="#ExternalDetailRe_qtyPOPUp">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_issuedQuantity").text()}">
                        </button>
                    </div>
                        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemissuedQty">
                    <button type="button" id="SubItemissuedQty" ${DisabledSubitm} class="calculator subItmImg"
                     onclick="return SubItemDetailsPopUp(event,'Exterissued_qty')" data-toggle="modal" data-target="#SubItem" data-backdrop="static"
                    data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                </td>

                <td>
                    <div class="col-sm-8 no-padding">
                        <input id="PendingQty" class="form-control num_right" value="${PendingQty}" readonly>
                    </div>
                    <div class="col-sm-2 i_Icon">
                        <button type="button" class="calculator ItmInfoBtnIcon"
                            onclick="OnClickItemOrderQtyIconBtn(event,'PendingQty');"
                            data-toggle="modal" data-target="#ExternalDetailRe_qtyPOPUp">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_pendingquantity").text()}">
                        </button>
                    </div>
                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemPendingQty">
                    <button type="button" id="SubItemPendingQty" ${DisabledSubitm} class="calculator subItmImg"
                     onclick="return SubItemDetailsPopUp(event,'ExterPendingQty')" data-toggle="modal" data-target="#SubItem" data-backdrop="static"
                    data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>

                </td>

                <td>
                    <div class="col-sm-8 lpo_form no-padding">
                        <input id="ReceivedQuantity" readonly class="form-control num_right" placeholder="0000.00">
                    </div>
                              <div class="col-sm-1 i_Icon">
                        <button type="button" class="calculator ItmInfoBtnIcon"
                            onclick="OnClickItemOrderQtyIconBtn(event,'recqty');"
                            data-toggle="modal" data-target="#ExternalDetailRe_qtyPOPUp">
                            <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_pendingquantity").text()}">
                        </button>
                    </div>
                         <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReceivedQty">
                    <button type="button" id="SubItemReceivedQty" ${DisabledSubitm} class="calculator subItmImg" 
                     onclick="return SubItemDetailsPopUp(event,'ExterRec_qty')" data-toggle="modal" data-target="#SubItem" data-backdrop="static"
                    data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                </td>

                <td class="lpo_form">
                    <input id="CostPrice" value="${cost_pr}" readonly class="form-control num_right" placeholder="0000.00" type="text">
                </td>

                <td width="7%">
                    <div class="lpo_form">

                        <select class="form-control" id="wh_id${RowNo}" disabled="disabled" title="${wh_name}">
                            <option value="${wh_id}">${wh_name}</option>
                        </select>
                        <input type="hidden" id="Hdn_GRN_WhName_${RowNo}"  value="${wh_id}">
                    </div>
                </td>

                <td>
                    <textarea id="ItemRemarks" class="form-control remarksmessage"
                        maxlength="300" placeholder="${$("#span_remarks").text()}"></textarea>
                </td>

                <td style="display:none;">
                    <input type="hidden" id="SNohf" value="${RowNo}">
                </td>
            </tr>
        `);

                            // ✅ Bind warehouse dropdowns after adding row
                           // BindWarehouseList(RowNo);
                           // WareHouseSearchAble();
                        }
                    }


                    // ===================== TABLE 1 (BATCH / SERIAL DETAILS) =====================
                    if (arr.Table1 && arr.Table1.length > 0) {
                        debugger;
                        for (let j = 0; j < arr.Table1.length; j++) {
                            const row = arr.Table1[j];
                            const LotID = row.Lot_id || '';
                            const BatchSerial = row.batch_no || '';
                            const Exp_date = row.exp_date || '';
                            const Expdate = row.expirydate || '';
                            const ItemID = row.item_id || '';
                            const UOM_ID = row.uom_id || '';
                            const MfgDate = (row.mfg_date == "1900-01-01" ? "" : row.mfg_date) || '';
                            const IssuedQty = parseFloat(row.Issue_qty || 0).toFixed(QtyDecDigit);
                            const PendingQty = parseFloat(row.PendingQty || 0).toFixed(QtyDecDigit);

                            $lotTableBody.append(`
                            <tr>
                                <td hidden="hidden"><input type="text" id="rcptItemID"  value="${ItemID}"></td>                               
                                <td hidden="hidden"><input type="text" id="rcptUOMID"  value="${UOM_ID}"></td>
                                <td><input type="text" id="rcptLotID" value="${LotID}"></td>
                                <td><input type="text" id="rcptBatchSerial" value="${BatchSerial}"></td>
                                <td><input type="text" id="rcptExp_date" value="${Exp_date}"></td>
                                <td hidden="hidden"><input type="text" id="hdnrcptExp_date" value="${Expdate}"></td>
                                <td><input type="text" id="rcptIssueQty" value="${IssuedQty}"></td>
                                <td><input type="text" id="rcptPendingQty" value="${PendingQty}"></td>
                                <td><input type="text" id="rcptRec_qty" value="${parseFloat(0).toFixed(QtyDecDigit)}"></td>
                                <td id="bt_mfg_name">${row.mfg_name}</td>
                                <td><input type="text" id="bt_mfg_mrp" value="${parseFloat(row.mfg_mrp).toFixed(RateDigit)}" /></td>
                                <td><input type="text" id="bt_mfg_date" value="${MfgDate}" /></td>
                            </tr>
                        `);
                        }
                    }



                    // ===================== TABLE 1 (BATCH / SERIAL DETAILS) =====================
                    if (arr.Table2 && arr.Table2.length > 0) {
                        debugger;
                        for (let j = 0; j < arr.Table2.length; j++) {
                            const row = arr.Table2[j];
                         
                            const ItemID = row.item_id || '';
                            const sub_item_id = row.sub_item_id || '';
                            const UOM_ID = row.uom_id || '';
                            const IssuedQty = parseFloat(row.Issue_qty || 0).toFixed(QtyDecDigit);
                            const PendingQty = parseFloat(row.pending_qty || 0).toFixed(QtyDecDigit);

                          


                            $SubItemTableBody.append(`
                            <tr>
                               <td><input type="text" id="ItemId" value="${ItemID}"></td>
                                <td><input type="text" id="subItemId" value="${sub_item_id}"></td>
                                <td><input type="text" id="subItemIssueQty" value="${IssuedQty}"></td>
                                <td><input type="text" id="subItemPendQty" value="${PendingQty}"></td>
                                <td><input type="text" id="subItemQty" value="${parseFloat(0).toFixed(QtyDecDigit)}"></td>

                            </tr>
                        `);
                        }
                    }

                    // Reset validation UI
                    $("#src_doc_number, [aria-labelledby='select2-src_doc_number-container']").attr("disabled", true);
                    $("#src_doc_number, [aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
                    $("#SpanSourceDocNoErrorMsg").hide();
                }
            }
        });
    }

   
}
function OnClickItemOrderQtyIconBtn(e, flag) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").val();

    // ✅ Get returned array from function
    var FDocNoWiseItemQtyDetails = BindOrderQtyDetails(ItmCode);
    var SelectedItemdetail = JSON.stringify(FDocNoWiseItemQtyDetails);
    var SNohf = clickedrow.find("#SNohf").val();
    var ItmName = clickedrow.find("#Itemname_" + SNohf).val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMid = clickedrow.find("#UOMID").val();
    var OrderQty = clickedrow.find("#IssuedQuantity").val();
    var PendingQty = clickedrow.find("#PendingQty").val();
    var docid = $("#DocumentMenuId").val();
    var Command = $("#Qty_pari_Command").val();
    var TransType = $("#hdn_TransType").val();
    var PL_Status = $("#hdn_Status").val().trim();

    if (ItmCode != "" && ItmCode != null) {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ExternalReceipt/getItemOrderQuantityDetail",
            data: {
                ItemID: ItmCode,
                Status: PL_Status,
                SelectedItemdetail: SelectedItemdetail,
                TransType: TransType,
                Command: Command,
                docid: docid,
                flag: flag
            },
            success: function (data) {
                debugger;
                $('#ItemIssuedQuantityDetail').html(data);
                $("#OrderQtyItemName").val(ItmName);
                $("#hd_OQtyItemId").val(ItmCode);
                $("#OrderQtyUOM").val(UOM);
                $("#hd_OQtyUOMId").val(UOMid);
                $("#ItemIssuedQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
                $("#ItemPendingQuantity").val(parseFloat(PendingQty).toFixed(QtyDecDigit));
                TotalQtyCalulation();
            },
        });
    }
}

function BindOrderQtyDetails(ItmCode) {
    var FDocNoWiseItemQtyDetails = [];
    $("#SaveBatchSerialLotDeatil tbody tr #rcptItemID[value='" + ItmCode + "']").closest('tr')
   // $("#SaveBatchSerialLotDeatil tbody tr")
        .each(function () {
        var row = $(this);
        var ItemID = row.find("#rcptItemID").val() || "";

        // ✅ Match by input value
        if (ItemID === ItmCode) {
            FDocNoWiseItemQtyDetails.push({
                ItemID: ItemID,
                UomID: row.find("#rcptUOMID").val() || "",
                LotID: row.find("#rcptLotID").val() || "",
                BatchSerial: row.find("#rcptBatchSerial").val() || "",
                Exp_date: row.find("#rcptExp_date").val() || "",               
                Expdate: row.find("#hdnrcptExp_date").val() || "",
                IssueQty: row.find("#rcptIssueQty").val() || "0",
                PendingQty: row.find("#rcptPendingQty").val() || "0",
                RecQty: row.find("#rcptRec_qty").val() || "0",
                MfgName: row.find("#bt_mfg_name").text() || "",
                MfgMrp: parseFloat((row.find("#bt_mfg_mrp").val() || "0")).toFixed(cmn_RateDecDigit),
                MfgDate: Cmn_FormatDate_ddmmmyyyy(row.find("#bt_mfg_date").val()) || "",
                MfgDateHdn: (row.find("#bt_mfg_date").val() == "1900-01-01" ? "" : row.find("#bt_mfg_date").val()) || ""
            });
        }
    });

    return FDocNoWiseItemQtyDetails;
}



function TotalQtyCalulation() {

   var QtyDecDigit = $("#QtyDigit").text();
    var TotalPendingQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalRecQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#TblExternalbatchserialLot tbody tr").each(function () {
        var CurrentRow = $(this);
        var PendingQty = CurrentRow.find("#OrderQty_Pending").val();
        var RecQty = CurrentRow.find("#OrderQty_RecQty").val();
        if (PendingQty != null && PendingQty != "") {
            TotalPendingQty = parseFloat(TotalPendingQty) + parseFloat(PendingQty);
        }
        if (RecQty != null && RecQty != "") {
            TotalRecQty = parseFloat(TotalRecQty) + parseFloat(RecQty);
        }
    });
    $("#LblTotalPending").text(parseFloat(TotalPendingQty).toFixed(QtyDecDigit));
    $("#LblTotalOrderQty").text(parseFloat(TotalRecQty).toFixed(QtyDecDigit));
}
function onclickbtnOrderQtyReset() {
    debugger;
    $("#TblExternalbatchserialLot tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        CurrentRow.find("#OrderQty_RecQty").val("");

    });

    $("#LblTotalOrderQty").text("");
}
function OnKeyPressOrderRecQty(el, evt) {
    var clickedrow = $(evt.target).closest("tr");
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    clickedrow.find("#OrderQty_RecQty_Error").css("display", "none");
    clickedrow.find("#OrderQty_RecQty ").css("border-color", "#ced4da");
    return true;
}
function OnChangeOrderRecQty(evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(evt.target).closest("tr");
    var RecQty = clickedrow.find("#OrderQty_RecQty").val();

    if ((RecQty == "") || (RecQty == null)) {
        RecQty = "";
        clickedrow.find("#OrderQty_RecQty").val(RecQty);
    }
    var PendingQty = clickedrow.find("#OrderQty_Pending").val();
    var QtyDigit = $("#QtyDigit").text();;
    if (parseFloat(RecQty) > parseFloat(PendingQty)) {
        debugger;
        clickedrow.find("#OrderQty_RecQty_Error").text($("#ExceedingQty").text());
        clickedrow.find("#OrderQty_RecQty_Error").css("display", "block");
        clickedrow.find("#OrderQty_RecQty").css("border-color", "red");
    }
    else {
        clickedrow.find("#OrderQty_RecQty_Error").css("display", "none");
        clickedrow.find("#OrderQty_RecQty").css("border-color", "#ced4da");
        if (parseFloat(RecQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
            clickedrow.find("#OrderQty_RecQty").val("");
        }
        else {
            var test = parseFloat(RecQty).toFixed(QtyDigit);
            clickedrow.find("#OrderQty_RecQty").val(test);
        }
    }
    TotalQtyCalulation();
}
function onclickbtnOrderQtySaveAndExit(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    if (CheckOrderRecQtyValidation() === true) {
        TotalQtyCalulation();

        var clickedrow = $(e.target).closest("tr");
        var ItemPendingQty = parseFloat($("#ItemPendingQuantity").val() || 0);
        var ProductID = $("#hd_OQtyItemId").val();
        var ItemTotalRecQuantity = parseFloat($("#LblTotalOrderQty").text() || 0);
        var flagSrcMatch = "N";

        if (ItemTotalRecQuantity > ItemPendingQty) {
            swal("", $("#PackedQtycannotexceedtoPendingQty").text(), "warning");
            flagSrcMatch = "Y";
            return false;
        }

        var SelectedItem = $("#hd_OQtyItemId").val();
        var ItemUomId = $("#hd_OQtyUOMId").val();
        const $lotTableBody = $("#SaveBatchSerialLotDeatil tbody");

        $("#TblExternalbatchserialLot tbody tr").each(function () {
            var CurrentRow = $(this);
            var Rec_qty = CurrentRow.find("#OrderQty_RecQty").val();

            if (parseFloat(Rec_qty) != parseFloat(0) && Rec_qty !== "") {
                var ItemID = CurrentRow.find("#OrderQty_ItemID").val();
                var LotID = CurrentRow.find("#OrderQty_Lot").val() || "";
                var BatchSerial = CurrentRow.find("#OrderQty_Batch").val() || "";
                var Exp_date = CurrentRow.find("#OrderQty_ExpDate").val() || "";
                var Expdate = CurrentRow.find("#hdnExpDate").val() || "";
                var UomID = CurrentRow.find("#OrderQty_UomID").val() || "";
                var IssueQty = CurrentRow.find("#OrderQty_Issued").val() || "0";
                var PendingQty = CurrentRow.find("#OrderQty_Pending").val() || "0";
                var RecQty = CurrentRow.find("#OrderQty_RecQty").val() || "0";
          
                $("#SaveBatchSerialLotDeatil tbody tr #rcptItemID[value='" + ItemID + "']").closest('tr') // Get the closest row containing the item
                    .each(function () {
                        var row = $(this);

                        // Retrieve the values from the current row
                        var rcpt_Item_ID = row.find("#rcptItemID").val();
                        var rcpt_LotID = row.find("#rcptLotID").val();
                        var rcpt_BatchSerial = row.find("#rcptBatchSerial").val();

                        // Match by input values
                        if (ItemID === rcpt_Item_ID && LotID === rcpt_LotID && BatchSerial === rcpt_BatchSerial) {
                            // If the conditions match, update the RecQty with the correct decimal places
                            var RecQty1 = parseFloat(RecQty).toFixed(QtyDecDigit); // Ensure RecQty is rounded correctly
                            row.find("#rcptRec_qty").val(RecQty1); // Set the value in the row
                        }
                    });

            }
        });

        // Update main table rows
        $("#itemtableExternalReceipt >tbody >tr").each(function () {
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hfItemID").val();

            if (ItemId == SelectedItem) {
                clickedrow.find("#ReceivedQuantity").val(
                    parseFloat($("#LblTotalOrderQty").text()).toFixed(QtyDecDigit)
                );
                clickedrow.find("#ReceivedQtyError").hide();
                clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            }
            clickedrow.find("#spanrecQtyInfoBtnIcon").hide();
            clickedrow.find("#recQtyInfoBtnIcon").css("border-color", "#ced4da");
        });

        $("#ExternalDetailRe_qtyPOPUp").modal('hide');
    } else {
        return false;
    }
}

function CheckOrderRecQtyValidation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var status = "N";
    var totalRecQty = 0;

    // ✅ Step 1: Check if any RecQty > 0
    var hasPositiveQty = false;
    $("#TblExternalbatchserialLot tbody tr").each(function () {
        var CurrentRow = $(this);
        var rec_qty = parseFloat(CurrentRow.find("#OrderQty_RecQty").val() || 0);
        if (parseFloat(rec_qty).toFixed(QtyDecDigit) > parseFloat(0).toFixed(QtyDecDigit)  ) {
            hasPositiveQty = true;
            return false; // stop loop early
        }
    });

    // ✅ Step 2: If any rec_qty > 0 → skip validation
    if (hasPositiveQty) {

        // ✅ Step 3: Normal validation if no rec_qty > 0
        $("#TblExternalbatchserialLot tbody tr").each(function () {
            var CurrentRow = $(this);
            var rec_qty = CurrentRow.find("#OrderQty_RecQty").val();
            var PendingQty = CurrentRow.find("#OrderQty_Pending").val();

            if (rec_qty != "" && rec_qty != null && parseFloat(rec_qty).toFixed(QtyDecDigit) != parseFloat(0).toFixed(QtyDecDigit)) {
                if (parseFloat(rec_qty) > parseFloat(PendingQty)) {
                    CurrentRow.find("#OrderQty_RecQty_Error").text($("#ExceedingQty").text());
                    CurrentRow.find("#OrderQty_RecQty_Error").css("display", "block");
                    CurrentRow.find("#OrderQty_RecQty").css("border-color", "red");
                    status = "Y";
                } else {
                    CurrentRow.find("#OrderQty_RecQty_Error").css("display", "none");
                    CurrentRow.find("#OrderQty_RecQty").css("border-color", "#ced4da");
                    var test = parseFloat(rec_qty).toFixed(QtyDecDigit);
                    CurrentRow.find("#OrderQty_RecQty").val(test);
                    totalRecQty += parseFloat(test);
                }
            }
            
        });


    }
    else {
        $("#TblExternalbatchserialLot tbody tr").each(function () {
            var CurrentRow = $(this);
            CurrentRow.find("#OrderQty_RecQty_Error").text($("#valueReq").text());
            CurrentRow.find("#OrderQty_RecQty_Error").css("display", "block");
            CurrentRow.find("#OrderQty_RecQty").css("border-color", "red");
            status = "Y";
        })
    }

  

    return status === "Y" ? false : true;
}
