var QtyDecDigit = $("#QtyDigit").text();///Quantity
var RateDecDigit = $("#RateDigit").text();///Rate 
var ValDecDigit = $("#ValDigit").text();///Amount 

$(document).ready(function () {
    debugger;
    //RemoveSessionNew();
    $("#ddlItemGroupName").select2({
        ajax: {
            url: $("#ItemGrpName").val(),
            data: function (params) {
                var queryParameters = {
                    GroupName: params.term // search term like "a" then "an"
                    //Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                //params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    $("#ddlWarehouse").select2({
        ajax: {
            url: $("#WHName").val(),
            data: function (params) {
                var queryParameters = {
                    WarehouseName: params.term // search term like "a" then "an"
                    //Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                //params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    
    //debugger;
    //var rowlen = $('#PopUp_STKAddTable >tbody >tr').length;
    //if (rowlen == 1) {
    //    BindWarehouseListForPopup(1);
    //}
    //else {
    //    BindWarehouseListForPopup();
    //}
    //var Wh_Id = $("#PopUpwhid_1").val();
    //if (Wh_Id == "0") {
    //    $('#PopUpDDLItemname_1').empty().append('<option value="0" selected="selected">---Select---</option>');
    //}
    BindDLLItemList();
    
    //
    GetViewDetails();
    //GetAndViewDetailsOfPopUpItemBatchandSerialDtlOnDblClik();
    $('#StockTakeTbl').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        var itemId = $(this).closest('tr').find("#hdItemId").val();
        $(this).closest('tr').remove();

        var stk_no = $("#StockTakeNumber").val();
        Cmn_DeleteSubItemQtyDetail(itemId);
        if (stk_no == null || stk_no == "") {
            if ($('#StockTakeTbl tr').length <= 1) {
                debugger;
                $("#ddlWarehouse").prop("disabled", false);
                $("#ddlItemGroupName").prop("disabled", false);
                $("#Itemname").prop("disabled", false);
                //$("#BtnAttribute").css('display', 'block');
                $(".plus_icon1").css('display', 'block');
            }
        }

        var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
        var NewArr = [];
        if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
            if (FStockTakeItemDetails.length > 0) {
                for (j = 0; j < FStockTakeItemDetails.length; j++) {
                    debugger;
                    var ItemID = FStockTakeItemDetails[j].ItmCode;

                    if (ItemID == itemId) {
                    }
                    else {
                        NewArr.push(FStockTakeItemDetails[j]);
                    }

                }
                sessionStorage.setItem("ItemQtyDetails", JSON.stringify(NewArr));
            }
        }
        updateItemSerialNumber();

    });
    $('#PopUp_STKAddTable').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        var itemId = $(this).closest('tr').find("#PopUphfItemID").val(); 
        var Batchable = $(this).closest('tr').find("#hfbatchable").val();
        var Seialable = $(this).closest('tr').find("#hfserialable").val();
        $(this).closest('tr').remove();

        //var stk_no = $("#StockTakeNumber").val();
        DeleteSubItemQtyDetailForStkPopup(itemId);
        DeleteItemBatchSerialQtyDetails(itemId, Batchable, Seialable)
        updateItemSerialNumberforPopup();

    });
    //sessionStorage.removeItem("ItemQtyDetails");    
    STKNo = $("#StockTakeNumber").val();
    $("#hdDoc_No").val(STKNo);

});

function GetViewDetails() {
    //
    debugger;
    if ($("#hdItemBatchSerialDetailList").val() != null && $("#hdItemBatchSerialDetailList").val() != "") {
        debugger
        var arr2 = $("#hdItemBatchSerialDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdItemBatchSerialDetailList").val("");
        sessionStorage.removeItem("ItemQtyDetails");
        sessionStorage.setItem("ItemQtyDetails", JSON.stringify(arr));
    } else {
        sessionStorage.removeItem("ItemQtyDetails");
    }

    //if ($("#WFBarStatus").val() != null && $("#WFBarStatus").val() != "") {
    //    var arr2 = $("#WFBarStatus").val();
    //    var arr = JSON.parse(arr2);
    //    $("#WFBarStatus").val();
    //    if (arr.length > 0) {
    //        for (var yw = 0; yw < arr.length; yw++) {
    //            var Level = parseInt(arr[yw].level);
    //            var nextLevel = parseInt(Level) + 1;
    //            $("#a_" + Level).removeClass("disabled");
    //            $("#a_" + Level).addClass("done");

    //            $("#a_" + nextLevel).removeClass("disabled");
    //            $("#a_" + nextLevel).addClass("selected");
    //        }
    //    }
    //}
    //if ($("#WFStatus").val() != null && $("#WFStatus").val() != "") {
    //    debugger
    //    var arr2 = $("#WFStatus").val();
    //    var arr = JSON.parse(arr2);
    //    $("#WFStatus").val();
    //    if (arr.length > 0) {
    //        var Createrid = $("#Create_ID").val();
    //        $("#hd_nextlevel").val(arr[0].nextlevel);
    //        $("#hd_currlevel").val(arr[0].userlevel);
    //        $("#hd_createrlevel").val(arr[0].createrlevel);
    //        var createlvl = arr[0].createrlevel;
    //        var userlvl = arr[0].userlevel;
    //        if (AvoidDot(createlvl) == false) {
    //            createlvl = 0;
    //        }
    //        if (AvoidDot(userlvl) == false) {
    //            userlvl = 0;
    //        }
    //        if (parseFloat(createlvl) < (parseFloat(userlvl) - parseFloat(createlvl))) {
    //            $("#radio_revert").prop("disabled", false);
    //        }
    //        else {
    //            $("#radio_revert").prop("disabled", true);
    //        }
    //        if (arr[0].nextlevel === "0") {
    //            $("#span_forward").css("display", "none");
    //            $("#span_approve").removeAttr("style");
    //            $("#radio_forward").val("Approve");
    //        }
    //        else {

    //        }
    //    }
    //}
    //var UserID = $("#UserID").text();
    //var create_Id = $("#Create_ID").val();
    //if (UserID === create_Id) {
    //    $("#radio_reject").prop("disabled", true);
    //    $("#radio_revert").prop("disabled", true);
    //}



}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#StockTakeTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function updateItemSerialNumberforPopup() {
    debugger;
    var SerialNo = 0;
    $("#PopUp_STKAddTable >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#PopUpSpanRowId").text(SerialNo);
        currentRow.find("#PopupSNohf").text(SerialNo);

    });
};
function CheckFormValidation() {

    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var rowcount = $('#StockTakeTbl tr').length;
    var return_date = $('#txtstkDate').val();

    var ValidationFlag = true;
    var tDnDate = $('#txtsrcdocdate').val();
    var wh_id = $('#ddlWarehouse').val();
    $('#hdwh').val(wh_id)
    var ST_Status = $('#hdSTStatus').val();
    if (ST_Status == "A") {
        if (CheckCancelledStatus() == false) {
            return false;
        }
    }
    if (ValidationFlag == true) {
        if (rowcount > 1) {
            var flag = CheckItemLevelValidations();
            debugger;
            var Batchflag = CheckItemBatchValidation();
            if (Batchflag == false) {
                return false;
            }
            var SubItemFlag = CheckValidations_forSubItems();
            if (SubItemFlag == false) {
                return false;
            }

            if (flag == true) {
                var StockTakeItemDetail = new Array();
                $("#StockTakeTbl TBODY TR").each(function () {
                    debugger;
                    var row = $(this);
                    var ItemList = {};
                    ItemList.FlagForStk = row.find("#hdFlagForStk").val();
                    ItemList.ItemId = row.find('#hdItemId').val();
                    ItemList.UOMId = row.find('#hdUOMId').val();
                    ItemList.WHId = row.find("#hdWHId").val();
                    ItemList.AvailableStock = row.find("#AvailableStock").val();
                    ItemList.PhysicalStock = row.find("#PhysicalStock").val();
                    if (ItemList.FlagForStk == "I" || ItemList.FlagForStk == "N") {
                        ItemList.ItemCost = row.find("#hdStkItmPrice").val();
                    }
                    else {
                        ItemList.ItemCost = parseFloat(0).toFixed(RateDecDigit);
                    }
                    ItemList.ShortQuantity = row.find("#ShortQuantity").val();
                    //ItemList.ShortQuantity = row.find("#ShortQuantity").val();
                    ItemList.SurplusQuantity = row.find("#SurplusQuantity").val();
                    ItemList.AdjustmentValue = row.find("#AdjustmentValue").val();
                    ItemList.ItemRemarks = row.find('#remarks').val();
                    StockTakeItemDetail.push(ItemList);
                    debugger;
                });

                var str = JSON.stringify(StockTakeItemDetail);
                $('#hdStkTakeItemDetailList').val(str);
                BindItemBatchSerialDetail();

                /*-----------Sub-item-------------*/

                var SubItemsListArr = Cmn_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
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
function BindItemBatchSerialDetail() {
    debugger;
    var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
    if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
        if (FStockTakeItemDetails.length > 0) {
            var str = JSON.stringify(FStockTakeItemDetails);
            $('#hdItemBatchSerialDetailList').val(str);
        }

    }
}
function OnchangeGroup() {
    debugger;
    BindDLLItemList();
}
function BindDLLItemList() {
    debugger;
    var GrpID = $("#ddlItemGroupName").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/StockTake/getListOfItems",
            data: { GroupID: GrpID },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        $('#Itemname optgroup option').remove();
                        $('#Itemname optgroup').remove();

                        $('#Itemname').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value='${arr.Table[i].Item_id}'>${arr.Table[i].Item_name}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#Itemname').select2({
                            templateResult: function (data) {
                                var UOM = $(data.element).data('uom');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-8 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-4 col-xs-12' + classAttr + '">' + UOM + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });


                    }
                }
            },
        });
}


function AddItemStockDetail() {
    debugger;

    if ($('#ddlWarehouse').val() != "0" && $('#ddlWarehouse').val() != "") {
        $("#BtnSearch").attr("disabled", true);
        //$(".plus_icon1").css('display', 'none');
        debugger;
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "#ced4da");
        $("#SpanddlwarehouseErrorMsg").css("display", "none");
        $("#SpanddlwarehouseErrorMsg").text("");

        var WHID = $("#ddlWarehouse").val();
        var GRPID = $("#ddlItemGroupName").val();
        var ItemID = $("#Itemname").val();
        var ListItems = "";

        $("#StockTakeTbl tbody tr").each(function () {
            var row = $(this);
            var item = row.find("#hdItemId").val();
            if (ListItems == "") {
                ListItems = item
            } else {
                ListItems += "," + item
            }
        });
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/StockTake/GetStockTakeItemDetail",
                data: {
                    WHID: WHID,
                    GRPID: GRPID,
                    ItemID: ItemID,
                    ListItems: ListItems
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        $("#StockTakeTbl tbody").append(data);
                        $("#BtnSearch").attr("disabled", false);

                        if ($("#StockTakeTbl tbody tr").length > 0) {
                            $("#ddlWarehouse").prop("disabled", true);
                            $("#ddlItemGroupName").prop("disabled", true);
                            $("#Itemname").prop("disabled", true);

                        }

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "red");
        $("#SpanddlwarehouseErrorMsg").css("display", "block");
        $("#SpanddlwarehouseErrorMsg").text($("#valueReq").text());
    }
}
function OnKeyPressPhyQty(el, evt) {
    //debugger;
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    
    var checkSR = $('#' + el.id).closest('tr').find('#Serialable').val();
    if (checkSR == "Y") {

        var key = evt.key;
        var number = el.value.split('.');

        if (number.length == 1) {
            var valPer = number[0] + '' + key;
            if (parseFloat(valPer) > 1) {
                return false;
            }
        }
        else {
            var valPer = number[0] + '.' + key;
            if (parseFloat(valPer) > 1) {
                return false;
            }
        }
    }
    return true;
}
function OnChangePhyQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    var clickedrow = $(evt.target).closest("tr");
    var Rqtydt = clickedrow.find("#PhysicalStock").val();
    if (AvoidDot(Rqtydt) == false) {
        Rqtydt = 0;
    }
    var hdItemId = clickedrow.find("#hdItemId").val();

    var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
    var NewArr = [];
    if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
        if (FStockTakeItemDetails.length > 0) {
            for (j = 0; j < FStockTakeItemDetails.length; j++) {
                debugger;
                var ItemID = FStockTakeItemDetails[j].ItmCode;

                if (ItemID == hdItemId) {
                }
                else {
                    NewArr.push(FStockTakeItemDetails[j]);
                }

            }
            sessionStorage.setItem("ItemQtyDetails", JSON.stringify(NewArr));
        }
    }

    var AvailableQty = parseFloat(clickedrow.find("#AvailableStock").val()).toFixed(parseFloat(QtyDecDigit));
    var PhysicalStock = parseFloat(Rqtydt).toFixed(parseFloat(QtyDecDigit));
    if ((PhysicalStock == "") || (PhysicalStock == null)) {
        var test = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#PhysicalStock").val(test);
        clickedrow.find("#PhysicalStockDetail").attr("disabled", true);
        clickedrow.find("#PhysicalStock_Error").text($("#valueReq").text());
        clickedrow.find("#PhysicalStock_Error").css("display", "block");
        clickedrow.find("#PhysicalStock").css("border-color", "red");
    }
    else {
        var test = parseFloat(parseFloat(PhysicalStock)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#PhysicalStock").val(test);
        clickedrow.find("#PhysicalStock_Error").css("display", "none");
        clickedrow.find("#PhysicalStock").css("border-color", "#ced4da");

        var Zero = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
        if (parseFloat(PhysicalStock) < parseFloat(AvailableQty)) {
            var DiffrenceQty = parseFloat(parseFloat(AvailableQty) - parseFloat(PhysicalStock)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#ShortQuantity").val(DiffrenceQty);
            clickedrow.find("#ShortQuantityDetail").attr("disabled", false);

            clickedrow.find("#SurplusQuantity").val(Zero);
            clickedrow.find("#SurplusQuantityDetail").attr("disabled", true);
            if (clickedrow.find("#sub_item").val() == "Y") {
                clickedrow.find("#SubItemShortQty").attr("disabled", false);
                clickedrow.find("#SubItemSurplusQty").attr("disabled", true);
            }

        }
        else if (parseFloat(PhysicalStock) > parseFloat(AvailableQty)) {
            var DiffrenceQty = parseFloat(parseFloat(PhysicalStock) - parseFloat(AvailableQty)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#SurplusQuantity").val(DiffrenceQty);
            clickedrow.find("#SurplusQuantityDetail").attr("disabled", false);

            clickedrow.find("#ShortQuantity").val(Zero);
            clickedrow.find("#ShortQuantityDetail").attr("disabled", true);
            if (clickedrow.find("#sub_item").val() == "Y") {
                clickedrow.find("#SubItemShortQty").attr("disabled", true);
                clickedrow.find("#SubItemSurplusQty").attr("disabled", false);
            }
        } else {
            clickedrow.find("#SurplusQuantity").val(Zero);
            clickedrow.find("#SurplusQuantityDetail").attr("disabled", true);
            clickedrow.find("#ShortQuantity").val(Zero);
            clickedrow.find("#ShortQuantityDetail").attr("disabled", true);
            if (clickedrow.find("#sub_item").val() == "Y") {
                clickedrow.find("#SubItemShortQty").attr("disabled", true);
                clickedrow.find("#SubItemSurplusQty").attr("disabled", true);
            }
        }
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    ItemInfoBtnClick(ItmCode);
}
function OnClickShortQtyIconBtn(e, flagAddNewStock) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    //var TotalQty = parseFloat(0).toFixed(QtyDecDigit);
    var flag = "";
    if (flagAddNewStock == "AddNewStock") {
        var clickedrow = e;
    }
    else {
        var clickedrow = $(e.target).closest("tr");
    }
    var hdFlagForAddNewStk = clickedrow.find("#hdFlagForStk").val();
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMID = clickedrow.find("#hdUOMId").val();
    var WHID = clickedrow.find("#hdWHId").val(); 
    var WarehouseName = clickedrow.find("#Warehouse").val();
    var ShortQuantity = clickedrow.find("#ShortQuantity").val();
    var SurplusQuantity = clickedrow.find("#SurplusQuantity").val();
    var StkBtchFlg = clickedrow.find("#hdStkBatchabl").val();
    var StkSrlFlg = clickedrow.find("#hdStkSeriable").val();
    var StkItmPrice = clickedrow.find("#hdStkItmPrice").val();


    $("#IconItemName").val(ItemName);
    $("#IconUOM").val(UOM);
    $("#IconShortQuantity").val(ShortQuantity);
    $("#IconSurplusQuantity").val(SurplusQuantity);
    $("#RQhdItemId").val(ItmCode);
    $("#RQhdUomId").val(UOMID);
    if (parseFloat(ShortQuantity) > parseFloat(0)) {
        flag = "S";
    }
    if (parseFloat(SurplusQuantity) > parseFloat(0)) {
        flag = "P";
        $("#DivSrtQty").css("display", "none")
        $("#DivPlusQty").css("display", "block")
        $("#DivSrtQty").removeAttr("style");
    }
    
    if (hdFlagForAddNewStk == "I") {
        debugger;
        var StkStatus = $("#hdSTStatus").val();
      
            var QtyDtlSurplusQty = $("#IconSurplusQuantity").val();
            //var qtytbllen = $("#QtyDetailsTbl >tbody >tr td #Item_id[value=" + ItmCode + "]").closest('tr').length;
            
            
                if (StkBtchFlg == "Y") {
                    var batchLen = $("#hdn_BatchDetailTbl >tbody >tr").length;
                    if (batchLen > 0) {
                        $("#QtyDetailsTbl tbody tr").remove();
                        var TotalAdjustValue = 0;
                        var TotalQty = 0;
                        //var TotalQty = parseFloat(0).toFixed(QtyDecDigit);
                        $("#hdn_BatchDetailTbl >tbody >tr").each(function (i, row) {
                            debugger;
                            var currentRow = $(this);
                            Btch_ItemID = currentRow.find("#hdnBtch_ItemID").val();
                            Btch_BatchQty = currentRow.find("#hdnBtch_BatchQty").val();
                            Btch_BatchNo = currentRow.find("#hdnBtch_BatchNo").val();
                            Btch_MfgName = currentRow.find("#hdnBtch_mfg_name").text();
                            Btch_MfgMrp = currentRow.find("#hdnBtch_mfg_mrp").val();
                            Btch_MfgDate = currentRow.find("#hdnBtch_mfg_date").val();
                            Btch_ExpDate = currentRow.find("#hdnBtch_BatchExDate").val();
                            if (Btch_MfgDate != "" && Btch_MfgDate != null) {
                                if (Btch_MfgDate == "1900-01-01") {
                                    Btch_MfgDate = "";
                                }
                            }
                            /*Btch_BatchExDate = currentRow.find("#hdnBtch_BatchExDate").val();*/
                            var Whid = $("#hf_PopupWhID").val();
                            var WhName = $("#hf_PopupWhName").val();
                            var BtchItmName = $("#BatchItemName").val();
                            var btchUOM = $("#batchUOM").val();
                            var BtchUOMId = $("#batchUOMId").val();
                            //var BItmRate = $("#hfBCostPrice").val();
                            var Bachable = $("#hfBachable").val();
                            var LotNo = 0;
                            var AdjustValue = Btch_BatchQty * StkItmPrice
                            var FlagType = "I";

                            //var hdFlagForAddNewStk = "Insert";

                            var rowCount = $('#QtyDetailsTbl >tbody >tr').length + 1
                            var RowNo = 0;

                            $("#QtyDetailsTbl >tbody >tr").each(function (i, row) {
                                //debugger;
                                var currentRow = $(this);
                                RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;

                            });

                            if (RowNo == "0") {
                                RowNo = 1;
                            }

                            var Short = "";
                            var Plus = "";
                            var returnqty = "";
                            var addqty = 0;
                            var PlusQty = "";
                            var ShortQty = "";

                            if ((ShortQty == "") || (ShortQty == null) || (ShortQty == 0) || (ShortQty == "NaN")) {
                                PlusQty = Btch_BatchQty;
                                addqty = PlusQty;
                            }
                            else {
                                returnqty = ShortQty;
                            }
                            if (AvoidDot(returnqty) == false) {
                                returnqty = 0;
                            }
                            if (AvoidDot(addqty) == false) {
                                addqty = 0;
                            }


                            if (flag == 'S') {
                                Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                                Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                                $("#hdsrtqty").css("display", true)
                                $("#hdplusqty").css("display", false)
                            }
                            else {
                                Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                                Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                                $("#hdsrtqty").css("display", false)
                                $("#hdplusqty").css("display", true)
                            }


                            if (Btch_ItemID == ItmCode) {
                                $('#QtyDetailsTbl tbody').append(` <tr id="${RowNo}">
                     <td hidden>
                     <input id="Warehouse${RowNo}" value="${WhName}"  class="form-control" autocomplete="off" type="text" name="Warehouse${RowNo}"  placeholder="${$("#span_Warehouse").text()}"  onblur="this.placeholder='${$("#span_Warehouse").text()}'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowCount}" />
                     <input type="hidden" id="Item_id" value='${Btch_ItemID}' />
                     <input type="hidden" id="hduom_id" value='${BtchUOMId}' />
                     <input type="hidden" id="wh_id${RowNo}" value='${Whid}' />
                     <input type="hidden" id="Batchable" value='${StkBtchFlg}' />
                     <input type="hidden" id="Serialable" value='${StkSrlFlg}' />
                     <input type="hidden" id="hdFlagForStk" value='${FlagType}' />
                                </td>

                                <td style="display: none;"><input type="hidden" id="hdFlagForStk" value='${FlagType}' /> </td>

                                 <td>
                                <input id="Lot${RowNo}" value="${LotNo}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${RowNo}" value="${Btch_BatchNo}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${RowNo}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                <td>
                                <input id="MfgName${RowNo}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_ManufacturerName").text()}"  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgMrp${RowNo}"  value="${Btch_MfgMrp}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00" disabled>
                                  </td>
                                <td>
                                <input id="MfgDate${RowNo}"  value="${Btch_MfgDate}" class="form-control" autocomplete="off" type="date" disabled>
                                  </td>
                                <td>
                                <input id="ExpDate${RowNo}"  value="${Btch_ExpDate}" class="form-control" autocomplete="off" type="date" disabled>
                                  </td>
                                <td>
                                <input id="CostPrice${RowNo}"  value="${parseFloat(StkItmPrice).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>

                                <td>
                                <input id="AvailableQuantity${RowNo}" value="" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                </td>
                                 <td id="tdshort">
                                `+ Short + `
                                 </td>
                                    <td id="tdplus">
                                    `+ Plus + `
                                 </td>
                                 <td>
                                <input id="AdjustmentValue${RowNo}" value="${parseFloat(AdjustValue).toFixed(ValDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                </tr>`);
                                $("#QtyDetailsTbl > tbody #MfgName" + RowNo).val(Btch_MfgName);
                                TotalQty = parseFloat(TotalQty) + parseFloat(addqty);
                                TotalAdjustValue = TotalAdjustValue + AdjustValue;
                            }
                        });
                    }
                    

                }
                if (StkSrlFlg == "Y") {
                    var serialLen = $("#hdn_SerialDetailTbl >tbody >tr").length;
                    if (serialLen > 0) {
                        $("#QtyDetailsTbl tbody tr").remove();
                        var TotalAdjustValue = 0;
                        var TotalQty = 0;

                        $("#hdn_SerialDetailTbl >tbody >tr").each(function (i, row) {
                            debugger;
                            var currentRow = $(this);
                            Srl_ItemID = currentRow.find("#hdnSrl_ItemID").val();
                            Srl_SerialNo = currentRow.find("#hdnSrl_SerialNo").val();
                            let Srl_MfgName = currentRow.find("#hdnSrl_MfgName").val();
                            let Srl_MfgMrp = currentRow.find("#hdnSrl_MfgMrp").val();
                            let Srl_MfgDate = currentRow.find("#hdnSrl_MfgDate").val();
                            if (Srl_MfgDate != "" && Srl_MfgDate != null) {
                                if (Srl_MfgDate == "1900-01-01") {
                                    Srl_MfgDate = "";
                                }
                            }
                            var Whid = $("#hf_PopupWhID").val();
                            var WhName = $("#hf_PopupWhName").val();
                            var Serialable = $("#hfSerialable").val();
                            var SrlUOMId = $("#SerialUOMId").val();
                            //var SrlItmRate = $("#hfSCostPrice").val();

                            var SerialQty = $("#SerialReceivedQuantity").val();
                            var SurPlusQty = 1;
                            var AdjustValue = SurPlusQty * StkItmPrice;
                            var LotNo = 0;
                            var FlagType = "I";
                            //var hdFlagForAddNewStk = "Insert";

                            var rowCount = $('#QtyDetailsTbl >tbody >tr').length + 1
                            var RowNo = 0;

                            $("#QtyDetailsTbl >tbody >tr").each(function (i, row) {
                                debugger;
                                var currentRow = $(this);
                                RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;

                            });

                            if (RowNo == "0") {
                                RowNo = 1;
                            }
                            var Short = "";
                            var Plus = "";
                            var returnqty = "";
                            var addqty = "";
                            var PlusQty = "";
                            var ShortQty = "";

                            if ((ShortQty == "") || (ShortQty == null) || (ShortQty == 0) || (ShortQty == "NaN")) {
                                PlusQty = SurPlusQty;
                                addqty = PlusQty;
                            }
                            else {
                                returnqty = ShortQty;
                            }
                            if (AvoidDot(returnqty) == false) {
                                returnqty = 0;
                            }
                            if (AvoidDot(addqty) == false) {
                                addqty = 0;
                            }

                            if (flag == 'S') {
                                Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                                Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                                $("#hdsrtqty").css("display", true)
                                $("#hdplusqty").css("display", false)
                            }
                            else {
                                Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                                Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" >
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                                $("#hdsrtqty").css("display", false)
                                $("#hdplusqty").css("display", true)
                            }

                            if (Srl_ItemID == ItmCode) {

                                //$("#QtyDetailsTbl tbody tr td #Item_id[value=" + ItmCode + "]").closest("tr").remove()
                                $('#QtyDetailsTbl tbody').append(` <tr id="${RowNo}">
                    <td hidden>
                     <input id="Warehouse${RowNo}" value="${WhName}"  class="form-control" autocomplete="off" type="text" name="Warehouse${RowNo}"  placeholder="${$("#span_Warehouse").text()}"  onblur="this.placeholder='${$("#span_Warehouse").text()}'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowCount}" />
                     <input type="hidden" id="Item_id" value='${Srl_ItemID}' />
                     <input type="hidden" id="hduom_id" value='${SrlUOMId}' />
                     <input type="hidden" id="wh_id${RowNo}" value='${Whid}' />
                     <input type="hidden" id="Batchable" value='${StkBtchFlg}' />
                     <input type="hidden" id="Serialable" value='${StkSrlFlg}' />
                     <input type="hidden" id="hdFlagForStk" value='${FlagType}' />
                                </td>
                                <td style="display: none;"><input type="hidden" id="hdFlagForStk" value='${FlagType}' /> </td>

                                 <td>
                                <input id="Lot${RowNo}" value="${LotNo}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${RowNo}" value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${RowNo}"  value="${Srl_SerialNo}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                <td>
                                <input id="MfgName${RowNo}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_ManufacturerName").text()}"  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgMrp${RowNo}"  value="${Srl_MfgMrp}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00" disabled>
                                  </td>
                                <td>
                                <input id="MfgDate${RowNo}"  value="${Srl_MfgDate}" class="form-control" autocomplete="off" type="date" disabled>
                                  </td>
                                <td>
                                <input id="CostPrice${RowNo}"  value="${parseFloat(StkItmPrice).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>

                                <td>
                                <input id="AvailableQuantity${RowNo}" value="" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                </td>
                                 <td id="tdshort">
                                `+ Short + `
                                 </td>
                                    <td id="tdplus">
                                    `+ Plus + `
                                 </td>
                                 <td>
                                <input id="AdjustmentValue${RowNo}" value="${parseFloat(AdjustValue).toFixed(ValDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00"  onblur="this.placeholder = ''" disabled>
                                  </td>
                </tr>`);
                                $("#QtyDetailsTbl > tbody #MfgName" + RowNo).val(Srl_MfgName);
                                TotalQty = TotalQty + addqty;
                                TotalAdjustValue = TotalAdjustValue + AdjustValue;
                            }
                        });

                    }
                    
                }
                if (StkBtchFlg == "N" && StkSrlFlg == "N") {
                    $("#QtyDetailsTbl tbody tr").remove();
                    var TotalAdjustValue = 0;
                    var TotalQty = 0;
                    var AdjustValue = SurplusQuantity * StkItmPrice;
                    var LotNo = 0;
                    var FlagType = "I";
                    //var hdFlagForAddNewStk = "Insert";

                    var rowCount = $('#QtyDetailsTbl >tbody >tr').length + 1
                    var RowNo = 0;

                    $("#QtyDetailsTbl >tbody >tr").each(function (i, row) {
                        debugger;
                        var currentRow = $(this);
                        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;

                    });

                    if (RowNo == "0") {
                        RowNo = 1;
                    }
                    var Short = "";
                    var Plus = "";
                    var returnqty = "";
                    var addqty = "";
                    var PlusQty = "";
                    var ShortQty = "";

                    if ((ShortQty == "") || (ShortQty == null) || (ShortQty == 0) || (ShortQty == "NaN")) {
                        PlusQty = SurplusQuantity;
                        addqty = PlusQty;
                    }
                    else {
                        returnqty = ShortQty;
                    }
                    if (AvoidDot(returnqty) == false) {
                        returnqty = 0;
                    }
                    if (AvoidDot(addqty) == false) {
                        addqty = 0;
                    }
                    let command = $("#hdnCommand").val();
                    if ((command == "Edit" && StkStatus == "D") || command == "New" || command == "Add") {
                        if (flag == 'S') {
                            Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                            Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                            $("#hdsrtqty").css("display", true)
                            $("#hdplusqty").css("display", false)
                        }
                        else {

                            Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                            Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                            $("#hdsrtqty").css("display", false)
                            $("#hdplusqty").css("display", true)
                        }
                    }
                    else {
                        if (flag == 'S') {
                            Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                            Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                            $("#hdsrtqty").css("display", true)
                            $("#hdplusqty").css("display", false)
                        }
                        else {

                            Short = `<div class="lpo_form">
                                <input id="IconShortQtyinput${RowNo}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanShortQty${RowNo}" class="error-message is-visible"></span></div>`;

                            Plus = `<div class="lpo_form">
                                <input id="IconSurPlusQtyinput${RowNo}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                    <span id="SpanSurPlusQty${RowNo}" class="error-message is-visible"></span></div>`;
                            $("#hdsrtqty").css("display", false)
                            $("#hdplusqty").css("display", true)
                        }
                    }
                    $('#QtyDetailsTbl tbody').append(` <tr id="${RowNo}">
                    <td hidden>
                     <input id="Warehouse${RowNo}" value="${WarehouseName}"  class="form-control" autocomplete="off" type="text" name="Warehouse${RowNo}"  placeholder="${$("#span_Warehouse").text()}"  onblur="this.placeholder='${$("#span_Warehouse").text()}'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowCount}" />
                     <input type="hidden" id="Item_id" value='${ItmCode}' />
                     <input type="hidden" id="hduom_id" value='${UOMID}' />
                     <input type="hidden" id="wh_id${RowNo}" value='${WHID}' />
                     <input type="hidden" id="Batchable" value='${StkBtchFlg}' />
                     <input type="hidden" id="Serialable" value='${StkSrlFlg}' />
                     <input type="hidden" id="hdFlagForStk" value='${FlagType}' />
                                </td>
                                <td style="display: none;"><input type="hidden" id="hdFlagForStk" value='${FlagType}' /> </td>

                                 <td>
                                <input id="Lot${RowNo}" value="${LotNo}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${RowNo}" value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${RowNo}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                <td>

                                <input id="MfgName${RowNo}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgMrp${RowNo}"  value="" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgDate${RowNo}"  value="" class="form-control num_right" autocomplete="off" type="date" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="CostPrice${RowNo}"  value="${parseFloat(StkItmPrice).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>

                                <td>
                                <input id="AvailableQuantity${RowNo}" value="" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                </td>
                                 <td id="tdshort">
                                `+ Short + `
                                 </td>
                                    <td id="tdplus">
                                    `+ Plus + `
                                 </td>
                                 <td>
                                <input id="AdjustmentValue${RowNo}" value="${parseFloat(AdjustValue).toFixed(ValDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="" placeholder="0000.00"  onblur="this.placeholder = ''" disabled>
                                  </td>
                </tr>`);
                    TotalQty = TotalQty + addqty;
                    TotalAdjustValue = TotalAdjustValue + AdjustValue;
                   


        }
                
            
            $('#QtyDetailsTbl tbody tr').each(function () {
                var row = $(this);
                if (flag == "S") {
                    row.find("#tdshort").css("display", "block")
                    $("#hdplusqty").css("display", "none")
                    $("#hdsrtqty").removeAttr("style");
                    row.find("#tdplus").removeAttr("style");
                    row.find("#tdplus").css("display", "none")
                }
                if (flag == "P") {
                    row.find("#tdplus").css("display", "block")
                    $("#hdplusqty").removeAttr("style");
                    $("#hdsrtqty").css("display", "none")
                    row.find("#tdshort").removeAttr("style");
                    row.find("#tdshort").css("display", "none")
                }
            });
            if (flag == "S") {
                $("#DivSrtQty").css("display", "block")
                $("#DivPlusQty").css("display", "none")
            }
            if (flag == "P") {
                $("#DivPlusQty").css("display", "block")
                $("#DivSrtQty").css("display", "none")
            }
            $('#TotalQty').text(parseFloat(TotalQty).toFixed(QtyDecDigit));
            $('#TotalValue').text(parseFloat(TotalAdjustValue).toFixed(ValDecDigit));
        
     }
    else {
        AddItemShortQtyDetail(clickedrow, ItmCode, WHID, flag);
    }
    

}
function AddItemShortQtyDetail(clickedrow, ItmID, WHID, flag) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ItemID = ItmID;
    var DocumentNumber = $('#StockTakeNumber').val();
    var hdFlagForAddNewStk = clickedrow.find("#hdFlagForStk").val();

    var ST_Status = $('#hdSTStatus').val();
    if (ItemID != "" && ItemID != null && WHID != "" && WHID != null) {
        debugger;
        $.ajax(
            {
                type: "Post",              
                url: "/ApplicationLayer/StockTake/GetStockItemLotBatchSerialDetail",
                data: {
                    ItemID: ItemID, WHID: WHID, SrcDocNumber: DocumentNumber, RT_Status: ST_Status, flag: flag,
                    hdFlagForAddNewStk: hdFlagForAddNewStk
                },
                async: false,
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        
                        var flagupdt = hdFlagForAddNewStk;
                            $('#QtyDetailsTbl tbody tr').remove();
                        //$("#QtyDetailsTbl >tbody >tr td #hdFlagForStk[value=" + flagupdt + "]").closest('tr').remove();

                        
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var Short = "";
                            var Plus = "";

                            for (var i = 0; i < arr.Table.length; i++) {
                                var item_id = arr.Table[i].item_id;
                                var whid = arr.Table[i].wh_id;
                                var lot_id = arr.Table[i].lot_id;
                                var batch_no = arr.Table[i].batch_no;
                                var serial_no = arr.Table[i].serial_no;
                                var serial = arr.Table[i].i_serial;
                                var mfg_date = arr.Table[i].mfg_date;
                                var exp_date = arr.Table[i].exp_date;
                                if (mfg_date != "" && mfg_date != null) {
                                    if (mfg_date == "1900-01-01") {
                                        mfg_date = "";
                                    }
                                }
                                if (exp_date != "" && exp_date != null) {
                                    if (exp_date == "1900-01-01") {
                                        exp_date = "";
                                    }
                                }
                                debugger;
                                if (batch_no === null || batch_no === "null") {
                                    batch_no = "";
                                }
                                if (serial_no === null || serial_no === "null") {
                                    serial_no = "";
                                }
                                debugger;
                                var TotalQty;
                                var TotalValue;
                                var returnqty = "";
                                var addqty = "";
                                var ItemCost = "";
                                var TotalAdjustQty = "";
                                var TotalAdjustValue = "";
                                var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
                                if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
                                    if (FStockTakeItemDetails.length > 0) {
                                        for (j = 0; j < FStockTakeItemDetails.length; j++) {
                                            debugger;
                                            var ItemID = FStockTakeItemDetails[j].ItmCode;
                                            var ItmUomId = FStockTakeItemDetails[j].ItmUomId;
                                            var wh_id = FStockTakeItemDetails[j].wh_id;
                                            var Lot = FStockTakeItemDetails[j].Lot;
                                            var Batch = FStockTakeItemDetails[j].Batch;
                                            var Serial = FStockTakeItemDetails[j].Serial;
                                            var ShortQty = FStockTakeItemDetails[j].ShortQty;
                                            var PlusQty = FStockTakeItemDetails[j].PlusQty;
                                            var ItemValue = FStockTakeItemDetails[j].ItemValue;
                                            var Batchable = FStockTakeItemDetails[j].Batchable;
                                            var Serialble = FStockTakeItemDetails[j].Serialable;
                                            if (Batch === null || Batch === "null") {
                                                Batch = "";
                                            }
                                            if (Serial === null || Serial === "null") {
                                                Serial = "";
                                            }
                                            if (ItemID == item_id && wh_id == whid && Lot == lot_id && Batch == batch_no && Serial == serial_no) {
                                                if ((ShortQty == "") || (ShortQty == null) || (ShortQty == 0) || (ShortQty == "NaN")) {
                                                    addqty = PlusQty;
                                                }
                                                else {
                                                    returnqty = ShortQty;
                                                }

                                                TotalQty = FStockTakeItemDetails[j].TotalQty;
                                                TotalValue = FStockTakeItemDetails[j].TotalValue;

                                                ItemCost = ItemValue;
                                            }

                                        }
                                    }
                                }
                                if (TotalQty == "" || TotalQty == null) {
                                    TotalQty = "0";
                                }
                                if (TotalValue == "" || TotalValue == null) {
                                    TotalValue = "0";
                                }
                                TotalAdjustQty = TotalQty;
                                TotalAdjustValue = TotalValue;

                                debugger;

                                if (AvoidDot(returnqty) == false) {
                                    returnqty = 0;
                                }
                                if (AvoidDot(addqty) == false) {
                                    addqty = 0;
                                }
                                if (AvoidDot(ItemCost) == false) {
                                    ItemCost = 0;
                                }

                                ++rowIdx;
                                if (flag == 'S') {
                                    Short = `<div class="lpo_form">
                                    <input id="IconShortQtyinput${rowIdx}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="1"  onblur="this.placeholder = '1'">
                                    <span id="SpanShortQty${rowIdx}" class="error-message is-visible"></span></div>`;

                                    Plus = `<div class="lpo_form">
                                    <input id="IconSurPlusQtyinput${rowIdx}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="1"  onblur="this.placeholder = '1'">
                                    <span id="SpanSurPlusQty${rowIdx}" class="error-message is-visible"></span></div>`;
                                    $("#hdsrtqty").css("display", true)
                                    $("#hdplusqty").css("display", false)
                                }
                                else {
                                   
                                     Short = `<div class="lpo_form">
                                     <input id="IconShortQtyinput${rowIdx}" value="${parseFloat(returnqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="1"  onblur="this.placeholder = '1'">
                                     <span id="SpanShortQty${rowIdx}" class="error-message is-visible"></span></div>`;

                                     Plus = `<div class="lpo_form">
                                     <input id="IconSurPlusQtyinput${rowIdx}" value="${parseFloat(addqty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePopupPhyQty(this,event)"  type="text" name="" placeholder="1"  onblur="this.placeholder = '1'">
                                     <span id="SpanSurPlusQty${rowIdx}" class="error-message is-visible"></span></div>`;
                                        $("#hdsrtqty").css("display", false)
                                        $("#hdplusqty").css("display", true)
                                    
                                }


                                $('#QtyDetailsTbl tbody').append(` <tr id="${rowIdx}">
                    <td hidden>
                     <input id="Warehouse${rowIdx}" value="${arr.Table[i].wh_name}"  class="form-control" autocomplete="off" type="text" name="Warehouse${rowIdx}"  placeholder="${$("#span_Warehouse").text()}"  onblur="this.placeholder='${$("#span_Warehouse").text()}'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowIdx}" />                    
                     <input type="hidden" id="Item_id" value='${arr.Table[i].item_id}' />
                     <input type="hidden" id="hduom_id" value='${arr.Table[i].uom_id}' />
                     <input type="hidden" id="wh_id${rowIdx}" value='${arr.Table[i].wh_id}' />  
                     <input type="hidden" id="Batchable" value='${arr.Table[i].i_batch}' />
                     <input type="hidden" id="Serialable" value='${arr.Table[i].i_serial}' />
                     <input type="hidden" id="hdFlagForStk" value='${arr.Table[i].flag}' />
                                </td>
                                <td style="display: none;"><input type="hidden" id="hdFlagForStk" value='${arr.Table[i].flag}' /> </td>
                                 <td>
                                <input id="Lot${rowIdx}" value="${arr.Table[i].lot_id}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${rowIdx}" value="${arr.Table[i].batch_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${rowIdx}"  value="${arr.Table[i].serial_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                <td>
                                <input id="MfgName${rowIdx}"  value="" class="form-control" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgMrp${rowIdx}"  value="${parseFloat(CheckNullNumber(arr.Table[i].mfg_mrp)).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="MfgDate${rowIdx}"  value="${mfg_date}" class="form-control num_right" autocomplete="off" type="date" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="ExpDate${rowIdx}"  value="${exp_date}" class="form-control num_right" autocomplete="off" type="date" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                                <td>
                                <input id="CostPrice${rowIdx}"  value="${parseFloat(arr.Table[i].landed_cost).toFixed(RateDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>

                                <td>
                                <input id="AvailableQuantity${rowIdx}" value="${parseFloat(arr.Table[i].batch_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="1"  onblur="this.placeholder = '1'" disabled>
                                </td>
                                 <td id="tdshort">
                                `+ Short + `
                                 </td>
                                    <td id="tdplus">
                                    `+ Plus + `
                                 </td>
                                 <td>
                                <input id="AdjustmentValue${rowIdx}" value="${parseFloat(ItemCost).toFixed(ValDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="" placeholder=""  onblur="this.placeholder = ''" disabled>
                                  </td>
                </tr>`);
                                $("#QtyDetailsTbl > tbody #MfgName" + rowIdx).val(arr.Table[i].mfg_name);
                                DisableDetail(ST_Status);
                               /* DisableDetail(ST_Status,serial);*/


                                $('#QtyDetailsTbl tbody tr').each(function () {
                                    var row = $(this);
                                    if (flag == "S") {
                                        row.find("#tdshort").css("display", "block")
                                        $("#hdplusqty").css("display", "none")
                                        $("#hdsrtqty").removeAttr("style");
                                        row.find("#tdplus").removeAttr("style");
                                        row.find("#tdplus").css("display", "none")
                                    }
                                    if (flag == "P") {
                                        row.find("#tdplus").css("display", "block")
                                        $("#hdplusqty").removeAttr("style");
                                        $("#hdsrtqty").css("display", "none")
                                        row.find("#tdshort").removeAttr("style");
                                        row.find("#tdshort").css("display", "none")
                                    }
                                });

                                if (flag == "S") {
                                    $("#DivSrtQty").css("display", "block")
                                    $("#DivPlusQty").css("display", "none")
                                }
                                if (flag == "P") {
                                    $("#DivPlusQty").css("display", "block")
                                    $("#DivSrtQty").css("display", "none")
                                }
                            }
                            if (TotalAdjustQty == "") {
                                $('#TotalQty').text(parseFloat(0).toFixed(ValDecDigit));
                                $('#TotalValue').text(parseFloat(0).toFixed(ValDecDigit));
                            }
                            else {
                                $("#TotalQty").text(parseFloat(TotalAdjustQty).toFixed(QtyDecDigit));
                                $('#TotalValue').text(parseFloat(TotalAdjustValue).toFixed(ValDecDigit));
                            }
                        }
                        else {
                            $("#QtyDetailsTbl >tbody >tr").remove();
                            //$("#QtyDetailsTbl >tbody >tr td #hdFlagForStk[value=" + flagupdt + "]").closest('tr').remove();

                            $('#TotalQty').text(parseFloat(0).toFixed(ValDecDigit));
                            $('#TotalValue').text(parseFloat(0).toFixed(ValDecDigit));
                        }

                    }
                    else {
                        $("#QtyDetailsTbl >tbody >tr").remove();
                        //$("#QtyDetailsTbl >tbody >tr td #hdFlagForStk[value=" + flagupdt + "]").closest('tr').remove();

                        $('#TotalQty').text(parseFloat(0).toFixed(ValDecDigit));
                        $('#TotalValue').text(parseFloat(0).toFixed(ValDecDigit));
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }

}

function OnChangePopupPhyQty(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var checkSR = clickedrow.find('#Serialable').val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var ValDigit = $("#ValDigit").text();
    var ShortQty = clickedrow.find("#IconShortQtyinput" + Sno).val();
    var PlusQty = clickedrow.find("#IconSurPlusQtyinput" + Sno).val();
    var ItemCost = clickedrow.find("#CostPrice" + Sno).val();
    if (AvoidDot(ShortQty) == false) {
        ShortQty = 0;
    }
    if (AvoidDot(PlusQty) == false) {
        PlusQty = 0;
    }
    var Available = parseFloat(clickedrow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
    if ((ShortQty == "") || (ShortQty == null) || (ShortQty == 0) || (ShortQty == "NaN")) {
        ShortQty = 0;
    }
    if ((PlusQty == "") || (PlusQty == null) || (PlusQty == 0) || (PlusQty == "NaN")) {
        PlusQty = 0;
    }

    if (PlusQty == 0) {
        if (checkSR == "Y") {
            if (parseFloat(ShortQty) > 0) {
                ShortQty = 1;
            }
        }
        if (parseFloat(ShortQty) <= parseFloat(Available)) {

            var Cost = parseFloat(ItemCost) * parseFloat(ShortQty)
            var FinalCost = parseFloat(parseFloat(Cost)).toFixed(parseFloat(ValDigit));
            clickedrow.find("#AdjustmentValue" + Sno).val(FinalCost);
            var test = parseFloat(parseFloat(ShortQty)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#IconShortQtyinput" + Sno).val(test);
            clickedrow.find("#SpanShortQty" + Sno).css("display", "none");
            clickedrow.find("#IconShortQtyinput" + Sno).css("border-color", "#ced4da");

        }
        else {
            clickedrow.find("#SpanShortQty" + Sno).text($("#ExceedingQty").text());
            clickedrow.find("#SpanShortQty" + Sno).css("display", "block");
            clickedrow.find("#IconShortQtyinput" + Sno).css("border-color", "red");
            var test = parseFloat(parseFloat(ShortQty)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#IconShortQtyinput" + Sno).val(test);

        }
    }
    else {
        if (checkSR == "Y") {
            if (parseFloat(PlusQty) > 0) {
                PlusQty = 1;
            }

        }
        var Cost = parseFloat(ItemCost) * parseFloat(PlusQty)
        //var Cost = parseFloat(ItemCost) * parseFloat(1)
        var FinalCost = parseFloat(parseFloat(Cost)).toFixed(parseFloat(ValDigit));
        clickedrow.find("#AdjustmentValue" + Sno).val(FinalCost);
        var test = parseFloat(parseFloat(PlusQty)).toFixed(parseFloat(QtyDecDigit));
        //var test = parseFloat(parseFloat(1)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconSurPlusQtyinput" + Sno).val(test);
        clickedrow.find("#SpanSurPlusQty" + Sno).css("display", "none");
        clickedrow.find("#IconSurPlustQtyinput" + Sno).css("border-color", "#ced4da");
    }
    CalTotalQty(el, evt);
    CalTotalValue(el, evt);
}
function CalTotalQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var TotalQty = parseFloat(0).toFixed(QtyDecDigit);

    $("#QtyDetailsTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        var Sno = CRow.find("#SNohiddenfiled").val();
        var ShortQty = CRow.find("#IconShortQtyinput" + Sno).val();
        if (ShortQty != "" && ShortQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(ShortQty));
        }
        var PlusQty = CRow.find("#IconSurPlusQtyinput" + Sno).val();
        if (PlusQty != "" && PlusQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(PlusQty));
        }
    });

    $("#TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));

};
function CalTotalValue(el, evt) {
    debugger;

    var ValDecDigit = $("#ValDigit").text();
    var TotalValue = parseFloat(0).toFixed(ValDecDigit);

    $("#QtyDetailsTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        var Sno = CRow.find("#SNohiddenfiled").val();
        var Value = CRow.find("#AdjustmentValue" + Sno).val();
        if (Value != "" && Value != null) {
            TotalValue = (parseFloat(TotalValue) + parseFloat(Value));
        }
    });

    $("#TotalValue").text(parseFloat(TotalValue).toFixed(ValDecDigit));

};

function OnClickSaveAndExitQty_Btn() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var IconShortQuantity = $("#IconShortQuantity").val();
    if (IconShortQuantity == "") {
        IconShortQuantity = parseFloat(0).toFixed(QtyDigit);
    }
    var IconSurplusQuantity = $("#IconSurplusQuantity").val();
    var ItmCode = $("#Item_id").val();
    var ItmUomId = $("#hduom_id").val();
    var Batchable = $("#Batchable").val();
    var Serialable = $("#Serialable").val();
    var TotalQty = $("#TotalQty").text();
    var TotalValue = $("#TotalValue").text();

    var error = "N";
    $("#QtyDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        debugger;
        var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
        if (ShortQty == "" || ShortQty == null || ShortQty == 0 || ShortQty == "NaN") {
            ShortQty = 0;
        }
        var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
        if (PlusQty == "" || PlusQty == null || PlusQty == 0 || PlusQty == "NaN") {
            PlusQty = 0;
        }
            var Available = currentRow.find("#AvailableQuantity" + Sno).val()
            if (Available == "" || Available == null || Available == 0 || Available == "NaN") {
                Available = 0;
            }
            else {
                var Available = parseFloat(currentRow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
            }
        

        if (PlusQty == 0) {
            if (parseFloat(ShortQty) <= parseFloat(Available)) {
                var test = parseFloat(parseFloat(ShortQty)).toFixed(parseFloat(QtyDecDigit));
                currentRow.find("#IconShortQtyinput" + Sno).val(test);
            }
            else {
                currentRow.find("#SpanShortQty" + Sno).text($("#ExceedingQty").text());
                currentRow.find("#SpanShortQty" + Sno).css("display", "block");
                currentRow.find("#IconShortQtyinput" + Sno).css("border-color", "red");
                var test = parseFloat(parseFloat(ShortQty)).toFixed(parseFloat(QtyDecDigit));
                currentRow.find("#IconShortQtyinput" + Sno).val(test);
                error = "Y";
                $("#SaveAndExitBtn").attr("data-dismiss", "");
            }
        }
        else {
            var test = parseFloat(parseFloat(PlusQty)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#IconSurPlusQtyinput" + Sno).val(test);
            $("#SaveAndExitBtn").attr("data-dismiss", "modal");

        }

        debugger;

    });
    if (error == "Y") {
        $("#SaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        debugger;
        if (parseFloat(IconShortQuantity) + parseFloat(IconSurplusQuantity) == parseFloat(TotalQty)) {
            $("#SaveAndExitBtn").attr("data-dismiss", "modal");
        }
        else {
            error = "Y";
            $("#SaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#TotalSurplusQtyDoesNotMatchWithSelectedQty").text(), "warning");
            return false
        }
    }
    let NewArr = [];
    var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
    if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
        if (FStockTakeItemDetails.length > 0) {
            for (i = 0; i < FStockTakeItemDetails.length; i++) {
                var ItemID = FStockTakeItemDetails[i].ItmCode;
                if (ItemID == ItmCode) {
                }
                else {
                    NewArr.push(FStockTakeItemDetails[i]);
                }
            }
            $("#QtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();

                debugger;
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
                var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
                var CostPrice = currentRow.find("#CostPrice" + Sno).val();
                var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
                var MfgName = currentRow.find("#MfgName" + Sno).val();
                var MfgMrp = currentRow.find("#MfgMrp" + Sno).val();
                var MfgDate = currentRow.find("#MfgDate" + Sno).val();
                var ExpDate = currentRow.find("#ExpDate" + Sno).val();
                var TotalQty = $("#TotalQty").text();
                var TotalValue = $("#TotalValue").text();

                NewArr.push({
                    TotalQty: TotalQty, TotalValue: TotalValue, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice
                    , MfgName: IsNull(MfgName, ""), MfgMrp: IsNull(MfgMrp, ""), MfgDate: IsNull(MfgDate, ""), ExpDate: IsNull(ExpDate,"")
                })
            });

            sessionStorage.removeItem("ItemQtyDetails");
            sessionStorage.setItem("ItemQtyDetails", JSON.stringify(NewArr));
        }
        else {
            var ItemList = [];
            $("#QtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                debugger;
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
                var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
                var CostPrice = currentRow.find("#CostPrice" + Sno).val();
                var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
                var MfgName = currentRow.find("#MfgName" + Sno).val();
                var MfgMrp = currentRow.find("#MfgMrp" + Sno).val();
                var MfgDate = currentRow.find("#MfgDate" + Sno).val();
                var ExpDate = currentRow.find("#ExpDate" + Sno).val();
                var TotalQty = $("#TotalQty").text();
                var TotalValue = $("#TotalValue").text();
                ItemList.push({
                    TotalQty: TotalQty, TotalValue: TotalValue, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice
                    , MfgName: IsNull(MfgName, ""), MfgMrp: IsNull(MfgMrp, ""), MfgDate: IsNull(MfgDate, ""), ExpDate: IsNull(ExpDate, "")
                })
            });

            sessionStorage.removeItem("ItemQtyDetails");
            sessionStorage.setItem("ItemQtyDetails", JSON.stringify(ItemList));
        }
    }
    else {
        var ItemList = [];
        $("#QtyDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            debugger;
            var wh_id = currentRow.find("#wh_id" + Sno).val();
            var Lot = currentRow.find("#Lot" + Sno).val();
            var Batch = currentRow.find("#Batch1" + Sno).val();
            var Serial = currentRow.find("#Serial1" + Sno).val();
            var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
            var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
            var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
            var CostPrice = currentRow.find("#CostPrice" + Sno).val();
            var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
            var MfgName = currentRow.find("#MfgName" + Sno).val();
            var MfgMrp = currentRow.find("#MfgMrp" + Sno).val();
            var MfgDate = currentRow.find("#MfgDate" + Sno).val();
            var ExpDate = currentRow.find("#ExpDate" + Sno).val();
            var TotalQty = $("#TotalQty").text();
            var TotalValue = $("#TotalValue").text();
            ItemList.push({
                TotalQty: TotalQty, TotalValue: TotalValue, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice
                , MfgName: IsNull(MfgName, ""), MfgMrp: IsNull(MfgMrp, ""), MfgDate: IsNull(MfgDate, ""), ExpDate: IsNull(ExpDate, "")
            })
        });
        sessionStorage.removeItem("ItemQtyDetails");
        sessionStorage.setItem("ItemQtyDetails", JSON.stringify(ItemList));
    }
    //$("#StockTakeTbl >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    //hdItemId
    //    var itemid = currentRow.find("#hdItemId").val();
    //    if (itemid == ItmCode) {
    //        currentRow.find("#AdjustmentValue").val($("#TotalValue").text());
    //    }
    // });
    $("#StockTakeTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var TotalItemBatchQty = parseFloat("0");
        var SurplusQuantity = currentRow.find("#SurplusQuantity").val();
        var ShortQuantity = currentRow.find("#ShortQuantity").val();
        var PhysicalStock = parseFloat(ShortQuantity) + parseFloat(SurplusQuantity);
        //hdItemId
        var itemid = currentRow.find("#hdItemId").val();
        if (itemid == ItmCode) {
            currentRow.find("#AdjustmentValue").val($("#TotalValue").text());
        }
        var FStockItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
        if (FStockItemDetails != null && FStockItemDetails != "") {
            if (FStockItemDetails.length > 0) {
                for (i = 0; i < FStockItemDetails.length; i++) {
                    var ItemID = FStockItemDetails[i].ItmCode;

                    if (ItemID == itemid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(FStockItemDetails[i].ShortQty) + parseFloat(FStockItemDetails[i].PlusQty);
                    }
                }
            }
        }
        if (parseFloat(PhysicalStock).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            //clickedrow.find("#PhysicalStock").css("border-color", "#ced4da");
            currentRow.find("#SurplusQuantityDetail").css("border", "#ced4da");

        }

    });
}

function CheckItemLevelValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#StockTakeTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);

        if (currentRow.find("#PhysicalStock").val() == "") {
            currentRow.find("#PhysicalStock_Error").text($("#valueReq").text());
            currentRow.find("#PhysicalStock_Error").css("display", "block");
            currentRow.find("#PhysicalStock").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#PhysicalStock_Error").css("display", "none");
            currentRow.find("#PhysicalStock").css("border-color", "#ced4da");
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    //var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var FlgFocus = "N";
    $("#StockTakeTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ShortQuantity = clickedrow.find("#ShortQuantity").val();
        var SurplusQuantity = clickedrow.find("#SurplusQuantity").val();
        var AvailableStock = clickedrow.find("#AvailableStock").val();
        var PhyStock = clickedrow.find("#PhysicalStock").val();
        var PhysicalStock = parseFloat(CheckNullNumber(ShortQuantity)) + parseFloat(CheckNullNumber(SurplusQuantity));
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var TotalItemBatchQty = parseFloat("0");

        let NewArr = [];
        var FStockItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
        if (FStockItemDetails != null && FStockItemDetails != "") {
            if (FStockItemDetails.length > 0) {
                for (i = 0; i < FStockItemDetails.length; i++) {
                    var ItemID = FStockItemDetails[i].ItmCode;
                    
                    if (ItemID == ItemId) {
                        TotalItemBatchQty = parseFloat(CheckNullNumber(TotalItemBatchQty)) + parseFloat(CheckNullNumber(FStockItemDetails[i].ShortQty)) + parseFloat(CheckNullNumber(FStockItemDetails[i].PlusQty));
                    }
                }
            }
        }

        if (parseFloat(PhysicalStock).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            //clickedrow.find("#PhysicalStock").css("border-color", "#ced4da");
            clickedrow.find("#SurplusQuantityDetail").css("border", "#ced4da");
            clickedrow.find("#ShortQuantityDetail").css("border", "#ced4da");
        }
        else {
            if (parseFloat(CheckNullNumber(PhyStock)) > parseFloat(CheckNullNumber(AvailableStock))) {
                clickedrow.find("#SurplusQuantityDetail").css("border", "2px solid red");
                if (FlgFocus == "N") {
                    clickedrow.find("#SurplusQuantityDetail").focus();
                    FlgFocus = "Y";
                    //clickedrow.find("#SurplusQuantityDetail").css("border", "#ced4da");
                }
            } else {
                clickedrow.find("#ShortQuantityDetail").css("border", "2px solid red");
                if (FlgFocus == "N") {
                    clickedrow.find("#ShortQuantityDetail").focus();
                    FlgFocus = "Y";
                }
            }
            /*clickedrow.find("#PhysicalStock").css("border-color", "red");   */
            EmptyFlag = "Y";
        }

    });
    if (EmptyFlag == "Y") {
        swal("", $("#span_BatchSerialQuantityDoesNotMatchWithShortSurplusQty").text(), "warning");
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
function DisableDetail(ST_Status) {
    debugger;
    var sessionval = sessionStorage.getItem("EditBtnClickST");
    if (ST_Status == "") {
        sessionval = "Y";
    }

    if (ST_Status == "D" || ST_Status == "") {
        $('#QtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            if (sessionval == "Y") {
                currentrow.find("#IconShortQtyinput" + SNo).attr("disabled", false);
            }
            else {
                currentrow.find("#IconShortQtyinput" + SNo).attr("disabled", true);
            }
            if (sessionval == "Y") {
                //if (serial == "Y") {/*Add By Hina on 25-07-2024 for serial item always disabled */
                //    currentrow.find("#IconSurPlusQtyinput" + SNo).attr("disabled", true);
                //}
                //else {
                    currentrow.find("#IconSurPlusQtyinput" + SNo).attr("disabled", false);
                //}
                
            }
            else {
                currentrow.find("#IconSurPlusQtyinput" + SNo).attr("disabled", true);
            }
        });

        if (sessionval == "Y") {


            //$("#SaveAndExitBtn").attr("disabled", false); // Commented by Suraj on 14-09-2023
            //$("#DiscardAndExit").attr("disabled", false);
            $("#SaveAndExitBtn").css("display", "block");
            $("#DiscardAndExit").css("display", "block");
            $("#BtnClose").css("display", "none");
            //$("#InvoicePopupReturnValue").attr("disabled", false);
            //$("#InvoiceSaveAndExitBtn").attr("disabled", false);
            //$("#InvoiceDiscardAndExit").attr("disabled", false);
        }
        else {
            //$("#SaveAndExitBtn").attr("disabled", true); // Commented by Suraj on 14-09-2023
            //$("#DiscardAndExit").attr("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
            $("#DiscardAndExit").css("display", "none");
            $("#BtnClose").css("display", "block");
            //$("#InvoicePopupReturnValue").attr("disabled", true);
            //$("#InvoiceSaveAndExitBtn").attr("disabled", true);
            //$("#InvoiceDiscardAndExit").attr("disabled", true);
        }
    }
    else {
        $('#QtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            currentrow.find("#IconShortQtyinput" + SNo).attr("disabled", true);
            currentrow.find("#IconSurPlusQtyinput" + SNo).attr("disabled", true);
        });
        $("#SaveAndExitBtn").attr("disabled", true);
        $("#DiscardAndExit").attr("disabled", true);
        //$("#InvoicePopupReturnValue").attr("disabled", true);
        //$("#InvoiceSaveAndExitBtn").attr("disabled", true);
        //$("#InvoiceDiscardAndExit").attr("disabled", true);
    }
}
function EnableEditkBtntst() {
    debugger;
    sessionStorage.setItem("EditBtnClickST", "Y");
    sessionStorage.removeItem("ItemQtyDetails");
    return true;
}
function functionConfirm(event) {
    debugger;
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

function ForwardBtnClick() {
    debugger;
    //var STKStatus = "";
    //STKStatus = $('#hdSTStatus').val().trim();
    //if (STKStatus === "D" || STKStatus === "F") {

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
        var StkDt = $("#txtstkDate").val();
        $.ajax({
            type: "POST",
            /*   url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: StkDt
            },
            success: function (data) {
                /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var STKStatus = "";
                    STKStatus = $('#hdSTStatus').val().trim();
                    if (STKStatus === "D" || STKStatus === "F") {

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
    var STKNo = "";
    var STKDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    Remarks = $("#fw_remarks").val();
    STKNo = $("#StockTakeNumber").val();
    $("#hdDoc_No").val(STKNo);
    STKDate = $("#txtstkDate").val();
    WF_Status1 = $("#WF_status1").val();
    docid = $("#DocumentMenuId").val();
    dashbordData = (docid + ',' + STKNo + ',' + STKDate + ',' + WF_Status1);
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && STKNo != "" && STKDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(STKNo, STKDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/StockTake/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData;
        }
    }
    debugger;

    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/StockTake/StockTakeApprove?STKNo=" + STKNo + "&STKDate=" + STKDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&docid=" + docid;


    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && STKNo != "" && STKDate != "") {
             Cmn_InsertDocument_ForwardedDetail(STKNo, STKDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/StockTake/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData;

        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && STKNo != "" && STKDate != "") {
             Cmn_InsertDocument_ForwardedDetail(STKNo, STKDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/StockTake/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&dashbordData=" + dashbordData;
        }
    }
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
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#StockTakeNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
}


/***--------------------------------SubItem and New Popup Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');

    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = clickdRow.find("#UOM").val();
    var phyqty = clickdRow.find("#PhysicalStock").val();
    /*var hdn_branch = $("#hdn_branch").val();*/
    var wh_id = clickdRow.find("#hdWHId").val();
    var DocNumber = $("#StockTakeNumber").val();
    var DocumentDate = $("#txtstkDate").val();
    var Sub_Quantity = 0;
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdSTStatus").val();

    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.avl_stk = row.find('#subItemAvlQty').val();
        List.short_qty = row.find('#subItemShortQty').val();
        List.surplus_qty = row.find('#subItemSurplusQty').val();
        NewArr.push(List);
    });
    if (flag == "Quantity") {
        Sub_Quantity = clickdRow.find("#PhysicalStock").val();
    } else if (flag == "ShortQuantity") {
        IsDisabled = "Y";
        Sub_Quantity = clickdRow.find("#ShortQuantity").val();
    }
    else if (flag == "SurplusQuantity") {
        IsDisabled = "Y";
        Sub_Quantity = clickdRow.find("#SurplusQuantity").val();
    }
    
    if (flag == "AvailableStock" && (hd_Status == "D" || hd_Status == "F" || hd_Status == "")) {
        Sub_Quantity = clickdRow.find("#AvailableStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, wh_id, Sub_Quantity, "wh");
    }
    else if (flag == "ReservedStock" && (hd_Status == "D" || hd_Status == "F" || hd_Status == "")) {
        Sub_Quantity = clickdRow.find("#ReservedStock").val();
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, wh_id, Sub_Quantity, "whres");
    }
    else {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/StockTake/GetSubItemDetails",
            data: {
                Item_id: ProductId,
                SubItemListwithPageData: JSON.stringify(NewArr),
                Flag: flag,
                doc_no: DocNumber,
                doc_dt: DocumentDate,
                wh_id: wh_id,
                IsDisabled: IsDisabled,
                hd_Status: hd_Status,
                stqty: phyqty
            },
            success: function (data) {
                debugger;
                if (flag == "AvailableStock") {
                    $("#SubItemStockPopUp").html(data);
                    $("#Stk_Sub_ProductlName").val(ProductNm);
                    $("#Stk_Sub_ProductlId").val(ProductId);
                    $("#Stk_Sub_serialUOM").val(UOM);
                    $("#Stk_Sub_Quantity").val(Sub_Quantity);
                } else {
                    $("#SubItemPopUp").html(data);
                    $("#Sub_ProductlName").val(ProductNm);
                    $("#Sub_ProductlId").val(ProductId);
                    $("#Sub_serialUOM").val(UOM);
                    $("#Sub_Quantity").val(Sub_Quantity);
                }

            }
        })
    }

}
function CheckValidations_forSubItems() {
    debugger;
    return Cmn_CheckValidations_forSubItems("StockTakeTbl", "", "hdItemId", "PhysicalStock", "SubItemPhyQty", "Y");
}

function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("StockTakeTbl", "", "hdItemId", "PhysicalStock", "SubItemPhyQty", "N");
}

function OnchangesubitemPhyQty(e) {
    var QtyDigit = $("#QtyDigit").text();
    var clickdRow = $(e.target).closest('tr');
    var subItemAvlQty = clickdRow.find("#subItemAvlQty").val();
    var subItemQty = clickdRow.find("#subItemQty").val();
    var ChangeInQty = parseFloat(CheckNullNumber(subItemAvlQty)) - parseFloat(CheckNullNumber(subItemQty));
    var shortqty = 0;
    var surplusqty = 0;
    if (parseFloat(ChangeInQty) > 0) {
        shortqty = parseFloat(ChangeInQty).toFixed(QtyDigit);
        surplusqty = parseFloat(0).toFixed(QtyDigit);

    } else if (parseFloat(ChangeInQty) < 0) {
        shortqty = parseFloat(0).toFixed(QtyDigit);
        surplusqty = parseFloat(-ChangeInQty).toFixed(QtyDigit);
    } else {

    }
    clickdRow.find("#subItemQty").val(parseFloat(subItemQty).toFixed(QtyDigit));
    clickdRow.find("#subItemShortQty").val(shortqty);
    clickdRow.find("#subItemSurplusQty").val(surplusqty);

    CalculateSubItemShortSurplusTotal();

}
function CalculateSubItemShortSurplusTotal() {
    var short_qty = 0;
    var surplus_qty = 0;
    $("#Sub_ItemDetailTbl tbody tr").each(function () {
        var Crow = $(this).closest("tr");

        var subItemQty = Crow.find("#subItemQty").val();
        var subItemshortQty = Crow.find("#subItemShortQty").val();
        var subItemsurplusQty = Crow.find("#subItemSurplusQty").val();
        Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        short_qty = parseFloat(short_qty) + parseFloat(CheckNullNumber(subItemshortQty));
        surplus_qty = parseFloat(surplus_qty) + parseFloat(CheckNullNumber(subItemsurplusQty));
    });
    $("#SubItemTotalShortQty").text(short_qty);
    $("#SubItemTotalSurplusQty").text(surplus_qty);



}
/***--------------------------------New Popup Sub Item Section End-----------------------------------------***/
function ShortingTableStockTakeItem(TableId) {
    var table, rows, switching, i, x, y, shouldSwitch;

    table = document.getElementById(TableId);
    switching = true;
    while (switching) {
        switching = false;
        rows = table.tBodies[0].rows;
        for (i = 0; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            //x = rows[i].getElementsByTagName("TD")[Col1].innerText;
            //y = rows[i + 1].getElementsByTagName("TD")[Col1].innerText;
            x = rows[i].children[2].children[0].children[0].value;
            y = rows[i + 1].children[2].children[0].children[0].value;
            if (x > y) {
                shouldSwitch = true;
                break;
            }

        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
function OnChangeWareHouse() {
    var WHID = $("#ddlWarehouse").val();
    if (WHID == null || WHID == "" || WHID == "0") {
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "red");
        $("#SpanddlwarehouseErrorMsg").css("display", "block");
        $("#SpanddlwarehouseErrorMsg").text($("#valueReq").text());
    } else {
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "#ced4da");
        $("#SpanddlwarehouseErrorMsg").css("display", "none");
        $("#SpanddlwarehouseErrorMsg").text("");
        
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

/*--------------------Work For Add New Stock Popup----------------------------*/
//function BindWarehouseListForPopup(id) {
//    debugger;
//    $.ajax(
//        {
//            type: "POST",
//            url: "/ApplicationLayer/StockTake/GetWarehouseListForPopUpStk",
//            data: {},
//            dataType: "json",
//            success: function (data) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    GRN_ErrorPage();
//                    return false;
//                }
//                if (data !== null && data !== "") {
//                    var arr = [];
//                    arr = JSON.parse(data);
//                    if (arr.Table.length > 0) {
//                        var s = '<option value="0">---Select---</option>';
//                        for (var i = 0; i < arr.Table.length; i++) {
//                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
//                        }
//                        $("#PopUpwhid_" + id).html(s);
//                        $("#PopUpwhid_" + id).select2()
                        
//                    }
//                }
//            },
//        });
//}
//function OnChangeWhForPopup(el, e) {
//    debugger;
//    var RowNo;
//    var clickedrow = $(e.target).closest("tr");
//    RowNo = clickedrow.find("#PopupSNohf").val();
//    var WHID = clickedrow.find("#PopUpwhid_" + RowNo ).val();
//    var WHID1 = clickedrow.find("#Hdn_PopUpWhId").val(WHID);
//    if (WHID == null || WHID == "" || WHID == "0") {
//        clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid red");
//        clickedrow.find("#PopUpwh_Error").css("display", "block");
//        clickedrow.find("#PopUpwh_Error").text($("#valueReq").text());
//    } else {
//        clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid #aaa");
//        clickedrow.find("#PopUpwh_Error").css("display", "none");
//        clickedrow.find("#PopUpwh_Error").text("");
//        clickedrow.find("#PopUpItemNameError").css("display", "none");
//        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + RowNo + "-container']").css("border", "1px solid #aaa");

//        BindDDLItemListForPopup(e);
//    }
//}
function OnClkAddNewStk() {
    debugger;
    //var errorFlag ="N"
    var WHID = $("#ddlWarehouse").val();
    var WHName = $("#ddlWarehouse option:selected").text();
    if (WHID == null || WHID == "" || WHID == "0") {
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "red");
        $("#SpanddlwarehouseErrorMsg").css("display", "block");
        $("#SpanddlwarehouseErrorMsg").text($("#valueReq").text());
        //errorFlag = "Y"
        $("#STK_AddNewStock").attr("data-target", "");
        return false

    } else {
        $('[aria-labelledby="select2-ddlWarehouse-container"]').css("border-color", "#ced4da");
        $("#SpanddlwarehouseErrorMsg").css("display", "none");
        $("#SpanddlwarehouseErrorMsg").text("");
        $("#hf_PopupWhID").val(WHID);
        $("#hf_PopupWhName").val(WHName)
        $("#PopupWh_Id").val(WHID);
        $("#PopupWh_Name").val(WHName); 
        
        $("#STK_AddNewStock").attr("data-target", "#AddNewStock");
        BindDDLItemListForPopup(WHID,1);
    }
}
function BindDDLItemListForPopup(WHID, RowNo) {
    debugger;
    var ItmDDLName = "#PopUpDDLItemname_";
    var TableID = "#PopUp_STKAddTable";
    var SnoHiddenField = "#PopupSNohf";
    BindItemListForPopup(ItmDDLName, RowNo, TableID, SnoHiddenField, "", "StockTake", WHID);
   /* $('#PopUpDDLItemname_1').empty();*/
    //var Wh_Id = $("#ddlWarehouse").val();
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/StockTake/getListOfItemsForAddNewPopup",
    //        data: { Wh_Id: WHID },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    //var PopupLen = $("#PopUp_STKAddTable >tbody >tr").length
    //                    //if (PopupLen > 0) {
    //                        //$("#PopUp_STKAddTable >tbody >tr").each(function () {
    //                        //    var currentRow = $(this);
    //                        //    var rowid = currentRow.find("#PopupSNohf").val();
    //                            //rowid = parseFloat(rowid) + 1;
    //                    $('#PopUpDDLItemname_' + RowNo + 'optgroup option').remove();
    //                    $('#PopUpDDLItemname_' + RowNo + ' optgroup').remove();

    //                    $('#PopUpDDLItemname_' + RowNo).append(`<optgroup class='def-cursor' id="Textddl${RowNo}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                $('#Textddl'+RowNo).append(`<option data-uom="${arr.Table[i].uom_name}" value='${arr.Table[i].Item_id}'>${arr.Table[i].Item_name}</option>`);
    //                            }
    //                            var firstEmptySelect = true;
    //                    $('#PopUpDDLItemname_' + RowNo).select2({
    //                                templateResult: function (data) {
    //                                    var UOM = $(data.element).data('uom');
    //                                    var classAttr = $(data.element).attr('class');
    //                                    var hasClass = typeof classAttr != 'undefined';
    //                                    classAttr = hasClass ? ' ' + classAttr : '';
    //                                    var $result = $(
    //                                        '<div class="row">' +
    //                                        '<div class="col-md-8 col-xs-12' + classAttr + '">' + data.text + '</div>' +
    //                                        '<div class="col-md-4 col-xs-12' + classAttr + '">' + UOM + '</div>' +
    //                                        '</div>'
    //                                    );
    //                                    return $result;
    //                                    firstEmptySelect = false;
    //                                }
    //                            });
    //                       /* });*/


    //                    //}
    //                }
    //            }
    //        },
    //    });
}
function BindItemListForPopup(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName, WHID) {
    debugger;
   if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
           $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
            $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName, WHID)

        });

    }
    else {
        DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName, WHID)
    }


}
function DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName, WHID) {
    debugger;
   var firstEmptySelect = true;
 $(ItmDDLName + RowID).select2({

            ajax: {
            url: '/ApplicationLayer/StockTake/getListOfItemsForAddNewPopup',
                data: function (params) {
                    var queryParameters = {
                        SearchName: params.term,
                        PageName: PageName,
                        WHID: WHID,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    //debugger
                    var pageSize,
                        pageSize = 20;//50000; // or whatever pagesize
                    //results = [];
                    debugger;
                    ConvertIntoRtnArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                    var ItemListArrey = [];
                    if (sessionStorage.getItem("selecteditemlist").length != null) {
                        ItemListArrey = sessionStorage.getItem("selecteditemlist");
                    }
                    var StkDtltblItmId = ConvertIntoRtnArreyListStkItmDtl();
                    //sessionStorage.removeItem("selecteditemlist");
                    debugger;
                    let selected = [];
                    selected.push({ id: $(ItmDDLName + RowID).val() });
                    selected = JSON.stringify(selected);

                    var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id));
                    NewArrey.push({ id: StkDtltblItmId })/*For Filter StkItmdetailTbl's Item in this Item List for filter in Add NewStk Popup */
                    data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));
                   
                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                    }
                    var page = params.page || 1;
                    // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                    debugger;
                    Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                    if (page == 1) {
                        debugger;
                        if (Fdata[0] != null) {
                            if (Fdata[0].Name.trim() != "---Select---") {
                                var select = { ID: "0_0", Name: "---Select---" };
                                Fdata.unshift(select);
                                //Fdata.unshift("");
                            }
                        }
                    }

                    return {
                        results: $.map(Fdata, function (val, Item) {
                            return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                        }),
                        //results: data.slice((page - 1) * pageSize, page * pageSize),
                        //more: data.length >= page * pageSize,
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
function ConvertIntoRtnArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField,) {
    debugger;
    let array = [];
    $(TableID + " tbody tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var rowno = currentRow.find(SnoHiddenField).val();
        var itemId = "";
        if (OtherFx == "listToHide") {
            itemId = currentRow.find(SnoHiddenField).val();
        }
        else {
            itemId = currentRow.find(ItmDDLName + rowno).val();
        }
        if (itemId != "0" /*&& itemId != $(ItmDDLName + RowID).val()*/) {
            array.push({ id: itemId });
        }
    });

    sessionStorage.removeItem("selecteditemlist");
    sessionStorage.setItem("selecteditemlist", JSON.stringify(array));
    //return array;
}
function ConvertIntoRtnArreyListStkItmDtl() {
    debugger;
    let Stktblarray = [];
    var InputItmLen = $("#StockTakeTbl tbody tr").length;
    if (InputItmLen > 0) {
        $("#StockTakeTbl tbody tr").each(function () {
            var row = $(this);
            var StktblItmId = row.find("#hdItemId").val();
            if (StktblItmId != "0") {
                Stktblarray.push({ id: StktblItmId });
            }
        });
    }
    return Stktblarray;
}

function OnChangeItemNamePopUp(RowID, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#PopupSNohf").val();

    Itm_ID = clickedrow.find("#PopUpDDLItemname_" + SNo).val();
    Itm_Name = clickedrow.find("#PopUpDDLItemname_" + SNo + " option:selected").text();
    //var pre_Item_id = clickedrow.find("#PopUphfItemID").val();
    //Cmn_DeleteSubItemQtyDetail(pre_Item_id);
    clickedrow.find("#PopUphfItemID").val(Itm_ID);
    clickedrow.find("#PopUphfItemName").val(Itm_Name);



    if (Itm_ID == "0") {
        clickedrow.find("#PopUpItemNameError").text($("#valueReq").text());
        clickedrow.find("#PopUpItemNameError").css("display", "block");
        //clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + SNo + "-container']").css("border", "1px solid #red");
        return false
    }
    else {
        clickedrow.find("#PopUpItemNameError").css("display", "none");
        //clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + SNo + "-container']").css("border", "1px solid #aaa");
        clickedrow.find("#PopUpItemNameError").text("");
    }
    debugger;
    ResetItemBatchSerialDetailsOnChngQtyAndOnChngItmName(Itm_ID);
    Cmn_BindUOM(clickedrow, Itm_ID, SNo, "", "stockTake")
    
    
    clickedrow.find("#PopupStockQuantity").val("");
    clickedrow.find("#PopUpItmPrice").val("");
    
    
}
function StkqtyFloatValueonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnclickStkQty(el, e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var errorFlag = "N";
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#PopupSNohf").val();
    var PopupQty;
    var WhId;
    var ItemId;
   
    WhId = clickedrow.find("#PopupWh_Id").val();
    ItemId = clickedrow.find("#PopUpDDLItemname_" + Sno).val();
    PopupQty = clickedrow.find("#PopupStockQuantity").val();
    var Batchable = clickedrow.find("#hfbatchable").val();
    var Seialable = clickedrow.find("#hfserialable").val();

    if (ItemName == "0") {
        clickedrow.find("#PopUpItemNameError").text($("#valueReq").text());
        clickedrow.find("#PopUpItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#PopUpDDLItemname_" + Sno).css("border", "1px solid red");
        clickedrow.find("#PopupStockQuantity").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#PopUpItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    
    if (errorFlag == "Y") {
        return false;
    }
    if (AvoidDot(PopupQty) == false) {
        clickedrow.find("#PopupStockQuantity").val("");
        PopupQty = 0;
    }

    if (PopupQty != "" && PopupQty != ".") {
        PopupQty = parseFloat(PopupQty);
    }
    if (PopupQty == ".") {
        PopupQty = 0;
    }
    if (PopupQty == "" || PopupQty == 0 || ItemId == "0") {
        clickedrow.find("#PopupStockQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#PopupStockQuantity_Error").css("display", "block");
        clickedrow.find("#PopupStockQuantity").css("border-color", "red");
        clickedrow.find("#PopupStockQuantity").val("");
        //clickedrow.find("#PopupStockQuantity").focus();

    }
    else {
        clickedrow.find("#PopupStockQuantity_Error").text("");
        clickedrow.find("#PopupStockQuantity_Error").css("display", "none");
        clickedrow.find("#PopupStockQuantity").css("border-color", "#ced4da");
        clickedrow.find("#PopupStockQuantity").val(parseFloat(PopupQty).toFixed(QtyDecDigit));

        ResetItemBatchSerialDetailsOnChngQtyAndOnChngItmName(ItemId, Batchable, Seialable);
    }
}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#PopupSNohf").val();
    clickedrow.find("#PopupStockQuantity_Error" + RowNo).css("display", "none");
    clickedrow.find("#PopupStockQuantity" + RowNo).css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#PopupSNohf").val();
    clickedrow.find("#PopUpItmPrice_Error" + RowNo).css("display", "none");
    clickedrow.find("#PopUpItmPrice" + RowNo).css("border-color", "#ced4da");
    return true;
}
function OnClickIconBtnPopUpStk(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#PopupSNohf").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#PopUpDDLItemname_" + Sno).val();
    ItemInfoBtnClick(ItmCode)

}
function OnChangePopUpItemPrice(RowID, e) {
    debugger;
    //let trgtrow = $(e.target).closest("tr");
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#PopupSNohf").val();
    var WhId;
    var ItemId;
    var ItmRate;
    WhId = clickedrow.find("#PopupWh_Id").val();
    //WhId = clickedrow.find("#PopUpwhid_" + Sno).val();
    ItemId = clickedrow.find("#PopUpDDLItemname_" + Sno).val();
    ItmRate = clickedrow.find("#PopUpItmPrice").val();
    
    if (ItemId == "0") {
        clickedrow.find("#PopUpItemNameError").text($("#valueReq").text());
        clickedrow.find("#PopUpItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#PopUpDDLItemname_" + Sno).css("border", "1px solid red");
        clickedrow.find("#PopUpItmPrice").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#PopUpItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    
    if (errorFlag == "Y") {
        return false;
    }
    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#PopUpItmPrice").val("");
        ItmRate = 0;
    }

    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 || ItemId == "0" || ItemId == "0.000") {
        clickedrow.find("#PopUpItmPrice_Error").text($("#valueReq").text());
        clickedrow.find("#PopUpItmPrice_Error").css("display", "block");
        clickedrow.find("#PopUpItmPrice").css("border-color", "red");
        currentRow.find("#PopUpItmPrice").val("");
        return false;
    }
    else {
        clickedrow.find("#PopUpItmPrice_Error").css("display", "none");
        clickedrow.find("#PopUpItmPrice").css("border-color", "#ced4da");
        clickedrow.find("#PopUpItmPrice").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    }
}
function AddNewRowForPopUp() {
    var rowIdx = 0;
    debugger;
    var Whid = $("#hf_PopupWhID").val();
    var WhName = $("#hf_PopupWhName").val();
    
    var rowCount = $('#PopUp_STKAddTable >tbody >tr').length + 1
    var RowNo = 0;
    
    $("#PopUp_STKAddTable >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#PopupSNohf").val()) + 1;
        
    });
    
    if (RowNo == "0") {
        RowNo = 1;
    }

    //var row_id = 0;
    //var SrNo = $("#PopUp_STKAddTable tbody tr").length;
    //SrNo = parseInt(SrNo) + 1;

    //$("#PopUp_STKAddTable tbody tr").each(function () {
    //    debugger
    //    row_id = $(this).find("#PopupSNohf").val();
    //})
    //row_id = parseInt(row_id) + 1;

    $('#PopUp_STKAddTable tbody').append(`<tr id="${RowNo}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td style="display: none;"><input type="hidden" id="PopupSNohf" value="${RowNo}" /></td>
<td class="sr_padding"><span id="PopUpSpanRowId">${rowCount}</span></td>

<td>
<input id="PopupWh_Name" value="${WhName}" class="form-control" autocomplete="off" type="text" name="Warehouse" placeholder="${$("#span_Warehouse").text()}" disabled>
<input id="PopupWh_Id" value="${Whid}"  type="hidden" value="" />

</td>

<td class="ItmNameBreak itmStick tditemfrz">
<div class="col-sm-11 lpo_form" style="padding:0px;">
<select class="form-control" id="PopUpDDLItemname_${RowNo}"" name="DDLItemname" onchange="OnChangeItemNamePopUp(${RowNo},event)"></select>
<input  type="hidden" id="PopUphfItemID" value="" />
<input type="hidden" id="PopUphfItemName" value="" />
<span id="PopUpItemNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtnPopUpStk(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
</button>
</div>
</td>
<td>
<input id="UOMPopup" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
<input id="UOMIDPopup" type="hidden" /></td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="PopupStockQuantity" class="form-control date num_right" autocomplete="off" type="text" onchange="OnclickStkQty(this,event)" onkeypress="return AmountFloatQty(this,event);" name="StockQuantity" onkeypress="return StkqtyFloatValueonly(this, event)" placeholder="0000.00" onblur="this.placeholder='0000.00'">
<span id="PopupStockQuantity_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemPopUpQty">
<input hidden type="text" id="PopUp_subitem" value="" />
<button type="button" id="SubItemPopUpQty" disabled class="calculator subItmImg" onclick="return StKPopUp_SubItemDetailsPopUp('PopUpStk_Qty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="lpo_form">
<input id="PopUpItmPrice" class="form-control num_right" maxlength="10" autocomplete="off" onchange="OnChangePopUpItemPrice(1,event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate" placeholder="0000.00" onblur="this.placeholder='0000.00'">
<span id="PopUpItmPrice_Error" class="error-message is-visible"></span>
</div>
</td>
<td>
<input id="LotNumber" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}"   disabled>
</td>
<td class="center">
<button type="button" id="BtnBatchDetail" disabled class="calculator subItmImg" onclick="OnClickBatchDetailBtnPopUp(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle=""  title="${$("#span_BatchDetail").text()}"></i>
</button>
</td>
<td class="center">
<button type="button" id="BtnSerialDetail" disabled class="calculator subItmImg"  onclick="OnClickSerialDetailBtnPopUp(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i>
</button>
</td>
<td style="display: none;"><input type="hidden" id="hfbatchable" value="" /></td>
<td style="display: none;"><input type="hidden" id="hfserialable" value="" /></td>
<td style="display: none;"><input type="hidden" id="hfexpiralble" value="" /></td>


</tr>`);
    //<td>
    //    <div class="lpo_form">
    //        <select class="form-control" id="PopUpwhid_${RowNo}" onchange="OnChangeWhForPopup(${RowNo},event)"><option value="0">---Select---</option>
    //        </select>
    //        <input type="hidden" id="Hdn_PopUpWhName" value="@Model.PopUpWh_Name" style="display: none;" />
    //        <span id="PopUpwh_Error" class="error-message is-visible"></span>
    //    </div>
    //</td>
    BindDDLItemListForPopup(Whid, RowNo);
    
}

function AddNewPopUpSaveandExitAndInsertStkItemDetails() {
    debugger;

    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text(); 
    var ValDecDigit = $("#ValDigit").text();
    var ItmValidate = CheckStkPopUpItemValidations();
    if (ItmValidate == false) {
        //swal("", $("#noitemselectedmsg").text(), "warning");
        $("#StkPopUpSaveAndExit").attr("data-dismiss", "");
        return false;
    }
    else {
        if (ItmValidate == true) {
            var SubItemFlag = CheckValidations_forNewPopupSubItems();
            if (SubItemFlag == false) {
                $("#StkPopUpSaveAndExit").attr("data-dismiss", "");
                return false;
            }
            else {
                if (SubItemFlag == true) {
                    var BatchValidate = CheckStkPopUpItemBatchValidation();
                    if ((BatchValidate) == false) {
                        swal("", $("#PleaseenterBatchDetails").text(), "warning");
                        $("#StkPopUpSaveAndExit").attr("data-dismiss", "");
                        return false;
                    }
                    else {
                        if ((BatchValidate) == true) {
                            var SerialValidate = CheckStkPopUpItemSerialValidation();
                            if ((SerialValidate) == false) {
                                swal("", $("#PleaseenterSerialDetails").text(), "warning");
                                $("#StkPopUpSaveAndExit").attr("data-dismiss", "");
                                return false;
                            }
                        }
                    }
                }
            }
        }
    }
    debugger;
    if (ItmValidate == true && SubItemFlag == true && BatchValidate == true && SerialValidate == true) {
        $("#StkPopUpSaveAndExit").attr("data-dismiss", "modal");

        $("#PopUp_STKAddTable >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            //RowNo = currentRow.find("#PopupSNohf").val();
            ItemId = currentRow.find("#PopUphfItemID").val();
            ItemName = currentRow.find("#PopUphfItemName").val();
            UomID = currentRow.find("#UOMIDPopup").val();
            UomName = currentRow.find("#UOMPopup").val();
            Wh_ID = currentRow.find("#PopupWh_Id").val();
            Wh_Name = currentRow.find("#PopupWh_Name").val();
            PopupStkQty = currentRow.find("#PopupStockQuantity").val();
            sub_item = currentRow.find("#PopUp_subitem").val();
            Popupbatchable = currentRow.find("#hfbatchable").val();
            Popupserialable = currentRow.find("#hfserialable").val();
            PopUpItmPrice = currentRow.find("#PopUpItmPrice").val(); 
            TotalValue = PopupStkQty * PopUpItmPrice;
            ShortQty = parseFloat(0).toFixed(QtyDecDigit);
            
            Resrv_Stk = parseFloat(0).toFixed(ValDecDigit);
            Avl_Stk = parseFloat(0).toFixed(ValDecDigit);
            var subitmDisable = "";
            var hdFlagForAddNewStk = "I";
            if (sub_item != "Y") {
                subitmDisable = "disabled";
            }
            var stltbllen = $('#StockTakeTbl tbody tr').length;
            if (stltbllen == 0) {
                var RowNo = currentRow.find("#PopupSNohf").val();
            }
            else {
                var RowNo = stltbllen + 1;
            }
            $('#StockTakeTbl tbody').append(`<tr id="${RowNo}">
<td class="center"><input type="checkbox" class="tableflat" id="PackingCheck" onclick="OnClickPackingCheck()" disabled></td>
    <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
    <td class="ItmNameBreak itmStick tditemfrz">
        <div class=" col-sm-11 pl-0">
            <input id="ItemName" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder="" disabled value='${ItemName}'>
            <input type="hidden" id="hdItemId" value='${ItemId}' style="display: none;" />
            <input type="hidden" id="hdFlagForStk" value='${hdFlagForAddNewStk}' style="display: none;" />
            <input type="hidden" id="hdStkBatchabl" value='${Popupbatchable}' style="display: none;" />
            <input type="hidden" id="hdStkSeriable" value='${Popupserialable}' style="display: none;" />
            <input type="hidden" id="hdStkItmPrice" value='${PopUpItmPrice}' style="display: none;" />

        </div>
        <div class="col-sm-1 i_Icon">
            <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
        </div>
    </td>
    <td>
        <input id="UOM" class="form-control"  autocomplete="off" type="text" name="UOM" placeholder='${$("#ItemUOM").text()}' disabled value='${UomName}'>
        <input type="hidden" id="hdUOMId" value='${UomID}' style="display: none;" />
    </td>
    <td hidden>
        <input id="Warehouse" class="form-control" name="Warehouse" placeholder="" disabled value='${Wh_Name}'>
        <input type="hidden" id="hdWHId" value='${Wh_ID}' style="display: none;" />
    </td>
    <td>
        <div class="col-sm-9 lpo_form no-padding">
            <input id="ReservedStock" value='${Resrv_Stk}' class="form-control num_right" autocomplete="" type="text" name="ReservedStock" placeholder="0000.00" disabled>
        </div>
        <input hidden type="text" id="sub_item" value="" />
        <div class="col-sm-3 i_Icon no-padding" id="div_SubItemAvlQty">
            <button type="button" id="SubItemAvlQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('ReservedStock',event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
    </td>
    <td>
        <div class="col-sm-9 lpo_form no-padding">
            <input id="AvailableStock" value='${Avl_Stk}' class="form-control num_right" autocomplete="" type="text" name="AvailableStock" placeholder="0000.00" disabled>
        </div>
        <input hidden type="text" id="sub_item" value="" />
        <div class="col-sm-3 i_Icon no-padding" id="div_SubItemAvlQty">
            <button type="button" id="SubItemAvlQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('AvailableStock',event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
    </td>
    <td>
        <div class="col-sm-9 lpo_form no-padding">
            <input id="PhysicalStock" class="form-control num_right" autocomplete="off" value='${PopupStkQty}' onkeypress="return OnKeyPressPhyQty(this,event);" onchange="OnChangePhyQty(this,event)" type="text" name="PhysicalStock" placeholder="0000.00">
            <span id="PhysicalStock_Error" class="error-message is-visible"></span>
        </div>
        <div class="col-sm-3 i_Icon no-padding" id="div_SubItemPhyQty">
            <button type="button" id="SubItemPhyQty"  class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
    </td>
    <td>
        <div class="col-sm-8" style="padding:0px;">
            <input id="ShortQuantity" class="form-control num_right" value='${ShortQty}' autocomplete="off" type="text" name="ShortQuantity" placeholder="0000.00" disabled>
        </div>
        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShortQty">
            <button type="button" id="SubItemShortQty"  class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('ShortQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
        <div class="col-sm-2 i_Icon">
            <button type="button" id="ShortQuantityDetail"  class="calculator greyImg3 subItmImg" onclick="OnClickShortQtyIconBtn(event);" data-toggle="modal" data-target="#StockTakeQuantityDetail" data-backdrop="static" data-keyboard="false" disabled><img src="/Content/Images/iIcon1.png" alt="" title="QuantityDetail"> </button>
        </div>
    </td>
    <td>
        <div class="col-sm-8" style="padding:0px;">
            <input id="SurplusQuantity" class="form-control num_right" autocomplete="off" value='${PopupStkQty}' type="text" name="SurplusQuantity" placeholder="0000.00" onblur="this.placeholder=" 0000.00"" disabled>
        </div>
        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemSurplusQty">
            <button type="button" id="SubItemSurplusQty"  class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('SurplusQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
        </div>
        <div class="col-sm-2 i_Icon">
            <button type="button" id="SurplusQuantityDetail" class="calculator greyImg3 subItmImg" onclick="OnClickShortQtyIconBtn(event);" data-toggle="modal" data-target="#StockTakeQuantityDetail" data-backdrop="static" data-keyboard="false" ><img src="/Content/Images/iIcon1.png" alt="" title="QuantityDetail"> </button>
        </div>
    </td>
    <td>
        <input id="AdjustmentValue" class="form-control num_right" value='${parseFloat(TotalValue).toFixed(ValDecDigit)}' autocomplete="off" type="text" name="AdjustmentValue" placeholder="0000.00" disabled>
    </td>
    <td>
        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}"></textarea>
    </td>
</tr>`);
        });
        $("#hdn_PopUpStkSub_ItemDetailTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            RowNo = currentRow.find("#PopupSNohf").val();
            ItmId = currentRow.find("#ItemId").val();
            SubItmId = currentRow.find("#subItemId").val();
            SubItmQty = currentRow.find("#subItemQty").val();
            avl_stk = 0;
            short_qty = 0;
            surplus_qty = currentRow.find("#subItemQty").val();
            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemQty" value='${SubItmQty}'></td>
                                    <td><input type="text" id="subItemAvlQty" value='${avl_stk}'></td>
                                    <td><input type="text" id="subItemShortQty" value='${short_qty}'></td>
                                    <td><input type="text" id="subItemSurplusQty" value='${surplus_qty}'></td>
                                    </tr>`);
        });
        debugger;
        
        $("#PopUp_STKAddTable >tbody >tr").remove();
        $("#hdn_PopUpStkSub_ItemDetailTbl >tbody >tr").remove();
        
        
        //let NewArr = [];
        //var QtyDecDigit = $("#QtyDigit").text();///Quantity  
        //var ItmCode = $("#Item_id").val();
        //var ItmUomId = $("#hduom_id").val();
        //var Batchable = $("#Batchable").val();
        //var Serialable = $("#Serialable").val();
        //var TotalQty = $("#TotalQty").text();
        //var TotalValue = $("#TotalValue").text();
        //var FStockTakeItemDetails = JSON.parse(sessionStorage.getItem("ItemQtyDetails"));
        //if (FStockTakeItemDetails != null && FStockTakeItemDetails != "") {
        //    if (FStockTakeItemDetails.length > 0) {
        //        for (i = 0; i < FStockTakeItemDetails.length; i++) {
        //            var ItemID = FStockTakeItemDetails[i].ItmCode;
        //            if (ItemID == ItmCode) {
        //            }
        //            else {
        //                NewArr.push(FStockTakeItemDetails[i]);
        //            }
        //        }
        //        $("#QtyDetailsTbl >tbody >tr").each(function () {
        //            var currentRow = $(this);
        //            var Sno = currentRow.find("#SNohiddenfiled").val();

        //            debugger;
        //            var wh_id = currentRow.find("#wh_id" + Sno).val();
        //            var WhName = currentRow.find("#Warehouse" + Sno).val();
        //            var Lot = currentRow.find("#Lot" + Sno).val();
        //            var Batch = currentRow.find("#Batch1" + Sno).val();
        //            var Serial = currentRow.find("#Serial1" + Sno).val();
        //            var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
        //            var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
        //            var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
        //            var CostPrice = currentRow.find("#CostPrice" + Sno).val();
        //            var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
        //            var TotalQty = $("#TotalQty").text();
        //            var TotalValue = $("#TotalValue").text();

        //            NewArr.push({ Sno: Sno, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, WhName: WhName, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice,TotalQty: TotalQty, TotalValue: TotalValue })
        //        });

        //        sessionStorage.removeItem("ItemQtyDetails");
        //        sessionStorage.setItem("ItemQtyDetails", JSON.stringify(NewArr));
        //    }
        //    else {
        //        var ItemList = [];
        //        $("#QtyDetailsTbl >tbody >tr").each(function () {
        //            var currentRow = $(this);
        //            var Sno = currentRow.find("#SNohiddenfiled").val();
        //            debugger;
        //            var wh_id = currentRow.find("#wh_id" + Sno).val();
        //            var WhName = currentRow.find("#Warehouse" + Sno).val();
        //            var Lot = currentRow.find("#Lot" + Sno).val();
        //            var Batch = currentRow.find("#Batch1" + Sno).val();
        //            var Serial = currentRow.find("#Serial1" + Sno).val();
        //            var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
        //            var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
        //            var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
        //            var CostPrice = currentRow.find("#CostPrice" + Sno).val();
        //            var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
        //            var TotalQty = $("#TotalQty").text();
        //            var TotalValue = $("#TotalValue").text();
        //            //ItemList.push({ Sno:Sno,TotalQty: TotalQty, TotalValue: TotalValue, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice })
        //            ItemList.push({ Sno: Sno, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, WhName: WhName, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice, TotalQty: TotalQty, TotalValue: TotalValue })

        //        });

        //        sessionStorage.removeItem("ItemQtyDetails");
        //        sessionStorage.setItem("ItemQtyDetails", JSON.stringify(ItemList));
        //    }
        //}
        //else {
        //    var ItemList = [];
        //    $("#QtyDetailsTbl >tbody >tr").each(function () {
        //        var currentRow = $(this);
        //        var Sno = currentRow.find("#SNohiddenfiled").val();
        //        debugger;
        //        var ItemidQtydtl = currentRow.find("#Item_id").val();
        //        var wh_id = currentRow.find("#wh_id" + Sno).val();
        //        var WhName = currentRow.find("#Warehouse" + Sno).val();
        //        var Lot = currentRow.find("#Lot" + Sno).val();
        //        var Batch = currentRow.find("#Batch1" + Sno).val();
        //        var Serial = currentRow.find("#Serial1" + Sno).val();
        //        var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
        //        var ShortQty = currentRow.find("#IconShortQtyinput" + Sno).val();
        //        var PlusQty = currentRow.find("#IconSurPlusQtyinput" + Sno).val();
        //        var CostPrice = currentRow.find("#CostPrice" + Sno).val();
        //        var ItemValue = currentRow.find("#AdjustmentValue" + Sno).val();
        //        var TotalQty = $("#TotalQty").text();
        //        var TotalValue = $("#TotalValue").text(); 
        //        var FlagType = currentRow.find("#hdFlagForStk").val();
        //        ItemList.push({ Sno: Sno, ItemidQtydtl: ItemidQtydtl, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, WhName: WhName, Lot: Lot, Batch: Batch, Serial: Serial, ShortQty: ShortQty, ItemValue: ItemValue, PlusQty: PlusQty, AvaQty: AvaQty, CostPrice: CostPrice, FlagType: FlagType, TotalQty: TotalQty, TotalValue: TotalValue })
        //    });
        //    sessionStorage.removeItem("ItemQtyDetails");
        //    sessionStorage.setItem("ItemQtyDetails", JSON.stringify(ItemList));
        //}



        $("#StockTakeTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            OnClickShortQtyIconBtn(currentRow, "AddNewStock");
            OnClickSaveAndExitQty_Btn();
        });
    }
}
function CheckStkPopUpItemValidations() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#PopUp_STKAddTable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#PopupSNohf").val();
        var ItemName = currentRow.find("#PopUpDDLItemname_" + Sno).val();
        var PopupQty = currentRow.find("#PopupStockQuantity").val();
        var ItmRate = currentRow.find("#PopUpItmPrice").val();
        //if (ItemName == "0" || ItemName == "" || ItemName == null) {
        //    swal("", $("#noitemselectedmsg").text(), "warning");
        //    ErrorFlag = "Y";
        //    return false;
        //}
        if (ItemName == "0" || ItemName == "" || ItemName == null) {
            currentRow.find("#PopUpItemNameError").text($("#valueReq").text());
            currentRow.find("#PopUpItemNameError").css("display", "block");
            currentRow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid red");
            currentRow.find("#PopUpDDLItemname_" + Sno).css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#PopUpItemNameError").css("display", "none");
            currentRow.find("[aria-labelledby='select2-PopUpDDLItemname_" + Sno + "-container']").css("border", "1px solid #aaa");
        }
        if (PopupQty == "" || PopupQty == "0" || PopupQty == "0.000") {
            currentRow.find("#PopupStockQuantity_Error").text($("#valueReq").text());
            currentRow.find("#PopupStockQuantity_Error").css("display", "block");
            currentRow.find("#PopupStockQuantity").css("border-color", "red");
            currentRow.find("#PopupStockQuantity").val("");
            ErrorFlag = "Y";
            //clickedrow.find("#PopupStockQuantity").focus();

        }
        else {
            currentRow.find("#PopupStockQuantity_Error").text("");
            currentRow.find("#PopupStockQuantity_Error").css("display", "none");
            currentRow.find("#PopupStockQuantity").css("border-color", "#ced4da");
            currentRow.find("#PopupStockQuantity").val(parseFloat(PopupQty).toFixed(QtyDecDigit));
        }
        if (ItmRate == "" || ItmRate == 0 || ItmRate == "0" || ItmRate=="0.000") {
            currentRow.find("#PopUpItmPrice_Error").text($("#valueReq").text());
            currentRow.find("#PopUpItmPrice_Error").css("display", "block");
            currentRow.find("#PopUpItmPrice").css("border-color", "red");
            currentRow.find("#PopUpItmPrice").val("");
            ErrorFlag = "Y";
            
        }
        else {
            currentRow.find("#PopUpItmPrice_Error").css("display", "none");
            currentRow.find("#PopUpItmPrice").css("border-color", "#ced4da");
            currentRow.find("#PopUpItmPrice").val(parseFloat(ItmRate).toFixed(RateDecDigit));
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckStkPopUpItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#PopUp_STKAddTable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#PopupSNohf").val();
        /*var Batchable = currentRow.find("#hfbatchable" + Sno).val();*/
        var Batchable = currentRow.find("#hfbatchable").val();
        var ItemID = currentRow.find("#PopUphfItemID").val();
        var PopupBatchQuantity = currentRow.find("#PopupStockQuantity").val();
        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#hdn_BatchDetailTbl >tbody >tr").length == 0) {
                //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
                
            }
            else {
                $("#hdn_BatchDetailTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchBatchQty = currentRow.find('#hdnBtch_BatchQty').val();
                    var bchitemid = currentRow.find('#hdnBtch_ItemID').val();
                    if (ItemID == bchitemid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchBatchQty);
                    }
                });

                if (parseFloat(PopupBatchQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "N");
                }
                else {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                  
                }
            }
        }
        if (Batchable == "") {
            ErrorFlag == "Y"
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}
function CheckStkPopUpItemSerialValidation() {
    debugger;
    var serialErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#PopUp_STKAddTable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#PopupSNohf").val();
        var Serialable = currentRow.find("#hfserialable").val();
        var ItemID = currentRow.find("#PopUphfItemID").val();
        var PopupSerialQuantity = currentRow.find("#PopupStockQuantity").val();
        if (Serialable == "Y") {
           var TotalItemSerialQty = parseFloat("0");
           if (Serialable == "Y" && $("#hdn_SerialDetailTbl >tbody >tr").length == 0) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
               ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
               serialErrorFlag = "Y";
                    
                }
           else {
               
               var rowcount = $("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + ItemID + "]").closest('tr').length;
               var rows = $("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + ItemID + "]").closest('tr');

               rows.each(function () {
                        debugger;
                        var currentRow = $(this);
                        
                        var srialitemid = currentRow.find('#hdnSrl_ItemID').val();
                        
                        if (ItemID == srialitemid) {
                            /*TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);*/
                            /*TotalItemSerialQty = parseFloat(TotalItemSerialQty);*/
                            TotalItemSerialQty = rowcount ;
                        }
                    });

                    if (parseFloat(PopupSerialQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                        //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "N");
                    }
                    else {
                        //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
                        serialErrorFlag = "Y";
                       
                    }
                }
            
        }
        if (Serialable == "") {
            serialErrorFlag == "Y";
        }
    });
    if (serialErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ResetItemBatchSerialDetailsOnChngQtyAndOnChngItmName(itemId, Batchable, Seialable) {
    debugger
    if (Batchable == "Y") {
        var hdnbtchLen = $("#hdn_BatchDetailTbl TBODY TR").length;
        if (hdnbtchLen > 0) {
            //var rows = $("#hdn_BatchDetailTbl >tbody >tr td #ItemId[value=" + ItemID + "]").closest('tr');
            debugger;

            var Batch_DetailList = [];
            var hdn_BatchDetail = [];
            $("#hdn_BatchDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Btch_ItemID = currentRow.find("#hdnBtch_ItemID").val();
                var Btch_BatchQty = currentRow.find("#hdnBtch_BatchQty").val();
                var Btch_BatchNo = currentRow.find("#hdnBtch_BatchNo").val();
                var Btch_BatchExDate = currentRow.find("#hdnBtch_BatchExDate").val();

                if (Btch_ItemID == itemId) {
                    $("#hdn_BatchDetailTbl tbody tr td #hdnBtch_ItemID[value=" + itemId + "]").closest("tr").remove();
                    $("#BatchDetailTbl tbody tr td #BatchNo[value=" + Btch_BatchNo + "]").closest("tr").remove();
                }
            });
        }
    }
    
    if (Seialable == "Y") {
        var hdnSrlLen = $("#hdn_SerialDetailTbl TBODY TR").length;
        if (hdnSrlLen > 0) {
            //var rows = $("#hdn_BatchDetailTbl >tbody >tr td #ItemId[value=" + ItemID + "]").closest('tr');
            debugger;
            $("#hdn_SerialDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Srl_ItemID = currentRow.find("#hdnSrl_ItemID").val();
                var Srl_SerialNo = currentRow.find("#hdnSrl_SerialNo").val();


                if (Srl_ItemID == itemId) {
                    $("#hdn_SerialDetailTbl tbody tr td #hdnSrl_ItemID[value=" + itemId + "]").closest("tr").remove();
                    //$("#SerialDetailTbl tbody tr td #BatchNo[value=" + Btch_BatchNo + "]").closest("tr").remove();
                }
            });
        }
    }
    
}
function DeleteItemBatchSerialQtyDetails(itemId, Bachable, Seriable) {
    debugger;
    if (Bachable == "Y") {
        //$("#hdn_BatchDetailTbl >tbody >tr td #hdnBtch_ItemID[value=" + itemId + "]").closest('tr').remove();
        $("#hdn_BatchDetailTbl TBODY TR").each(function ()
        {
        var row = $(this);
        var rowitem = row.find("#hdnBtch_ItemID").val();
        //var rowitem = $("#HDItemNameBatchWise").val();
            if (rowitem == itemId) {
                debugger;
                $(this).remove();
            }
        });
    }
    if (Seriable == "Y") {
        //$("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + itemId + "]").closest('tr').remove();
        $("#hdn_SerialDetailTbl TBODY TR").each(function ()
        {
        var row = $(this)
        var HdnItemId = row.find("#hdnSrl_ItemID").val();
            if (HdnItemId == itemId) {
                debugger;
                $(this).remove();
            }
        })
    }

    //$("#BatchDetailTbl >tbody >tr td #hdnBtch_ItemID[value=" + itemId + "]").closest('tr').remove();
   
    
    //$("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + itemId + "]").closest('tr').remove();
    //$("#hdn_SerialDetailTbl TBODY TR").each(function () {
    //    var row = $(this)
    //    var HdnItemId = row.find("#hdnSrl_ItemID").val();
    //    if (HdnItemId == itemId) {
    //        debugger;
    //        $(this).remove();
    //    }
    //})
}
/*----------------------For PopUp Batch Detail----------------------*/

function OnClickBatchDetailBtnPopUp(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();

    var errorFlag = "N";
    var ItemName;
    var PopupQty;
    var ItmRate;
    var WhId;
    
    var RowNo;
    var clickedrow = $(e.target).closest("tr");
    //RowNo = clickedrow.find("#PopUpSpanRowId").text();
    RowNo = clickedrow.find("#PopupSNohf").val();
    //WhId = clickedrow.find("#PopUpwhid_" + RowNo).val();
    WhId = clickedrow.find("#PopupWh_Id").val();
    Item_ID = clickedrow.find("#PopUpDDLItemname_" + RowNo).val();
    PopupQty = clickedrow.find("#PopupStockQuantity").val();
    ItmRate = clickedrow.find("#PopUpItmPrice").val();
    //if (WhId == null || WhId == "" || WhId == "0") {
    //    clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid red");
    //    clickedrow.find("#PopUpwh_Error").css("display", "block");
    //    clickedrow.find("#PopUpwh_Error").text($("#valueReq").text());
    //    errorFlag = "Y";
    //} else {
    //    clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid #aaa");
    //    clickedrow.find("#PopUpwh_Error").css("display", "none");
    //    clickedrow.find("#PopUpwh_Error").text("");
    //}
    if (Item_ID == "0") {
        clickedrow.find("#PopUpItemNameError").text($("#valueReq").text());
        clickedrow.find("#PopUpItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + RowNo + "-container']").css("border", "1px solid red");
        clickedrow.find("#PopUpDDLItemname_" + RowNo).css("border", "1px solid red");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#PopUpItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + RowNo + "-container']").css("border", "1px solid #aaa");
    }
    if (PopupQty == "" || PopupQty == 0 || Item_ID == "0") {
        clickedrow.find("#PopupStockQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#PopupStockQuantity_Error").css("display", "block");
        clickedrow.find("#PopupStockQuantity").css("border-color", "red");
        clickedrow.find("#PopupStockQuantity").val("");
        errorFlag = "Y";
        //clickedrow.find("#PopupStockQuantity").focus();

    }
    else {
        clickedrow.find("#PopupStockQuantity_Error").text("");
        clickedrow.find("#PopupStockQuantity_Error").css("display", "none");
        clickedrow.find("#PopupStockQuantity").css("border-color", "#ced4da");
        clickedrow.find("#PopupStockQuantity").val(parseFloat(PopupQty).toFixed(QtyDecDigit));
    }
    if (ItmRate == "" || ItmRate == 0 || Item_ID == "0") {
        clickedrow.find("#PopUpItmPrice_Error").text($("#valueReq").text());
        clickedrow.find("#PopUpItmPrice_Error").css("display", "block");
        clickedrow.find("#PopUpItmPrice").css("border-color", "red");
        errorFlag = "Y";

    }
    else {
        clickedrow.find("#PopUpItmPrice_Error").css("display", "none");
        clickedrow.find("#PopUpItmPrice").css("border-color", "#ced4da");
        clickedrow.find("#PopUpItmPrice").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    }
    if (errorFlag == "Y") {
        $("#BtnBatchDetail").attr("data-target", "");
        return false;
    }
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");

    ResetBatchDetailValSTK();
    var Sno = clickedrow.find("#PopUpSpanRowId").text();
    //if (Sno == "") {
    //    Sno = clickedrow.find("#PopupSNohf").val();
    //}
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ItemUOMID = "";
    var ExpiryFlag = "";
    var ReceiveQty = 0;
   

    //ItemName = clickedrow.find("#PopUpDDLItemname_" + Sno).val();
    //ItemName = clickedrow.find("#PopUpDDLItemname_" + Sno + " option:selected").text();
    ItemName = clickedrow.find("#PopUphfItemName").val();
    ItemID = clickedrow.find("#PopUphfItemID").val();
    ItemUOM = clickedrow.find("#UOMPopup").val();
    ItemUOMID = clickedrow.find("#UOMIDPopup").val();
    ExpiryFlag = clickedrow.find("#hfexpiralble").val();
    ReceiveQty = clickedrow.find("#PopupStockQuantity").val();
    Batchable = clickedrow.find("#hfbatchable").val();
   
    
    
    var ReceQty = parseFloat(ReceiveQty).toFixed(QtyDecDigit);

    if (ReceQty == "NaN" || ReceQty == "" || ReceQty == "0") {
        $("#BtnBatchDetail").attr("data-target", "");
        return false;
    }
    else {
        $("#BtnBatchDetail").attr("data-target", "#BatchNumber");
    }

    $("#BatchItemName").val(ItemName);
    $("#batchUOM").val(ItemUOM);
    $("#batchUOMId").val(ItemUOMID);
    $("#BatchReceivedQuantity").val(ReceQty); 
    //$("#hfBCostPrice").val(ItmRate);
    
    $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    $("#hfItemSNo").val(Sno);
    $("#hfItemID").val(ItemID);
    $("#hfexpiryflag").val(ExpiryFlag);
    $("#hfBachable").val(Batchable);
    
    
    if (ExpiryFlag != "Y") {
        $("#spanexpiryrequire").css("display", "none");
    }
    else {
        $("#spanexpiryrequire").css("display", "");
    }

    var rowIdx = 0;


    
    //var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    //if (FBatchDetails != null) {
    var btchrowcount = $("#hdn_BatchDetailTbl TBODY TR").length;
    //if (FBatchDetails.length > 0)
    if (btchrowcount > 0) {
        $("#BatchDetailTbl >tbody >tr").remove();
        var EnableBatchdetail = $("#EnableBatch_detail").val();
        var GreyScale = "";
        if (EnableBatchdetail == "Disable") {
            GreyScale = "style='filter: grayscale(100%)'";
        }
        /*for (i = 0; i < FBatchDetails.length; i++)*/
        //for (i = 0; i < btchrowcount; i++){
        $("#hdn_BatchDetailTbl TBODY TR").each(function () {
            var row = $(this);
            var SUserID = row.find("#hdnBtch_UserID").val();
            var SRowID = row.find("#hdnBtch_RowSNo").val();
            var SItemID = row.find("#hdnBtch_ItemID").val();
            var SBatchQty = row.find("#hdnBtch_BatchQty").val();
            var SBatchNo = row.find("#hdnBtch_BatchNo").val();
            var SBatchExDate = row.find("#hdnBtch_BatchExDate").val();
            var SBatchMfgName = row.find("#hdnBtch_mfg_name").text();
            var SBatchMfgMrp = row.find("#hdnBtch_mfg_mrp").val();
            var SBatchMfgDate = row.find("#hdnBtch_mfg_date").val();
            //var SBatchItmRate = row.find("#hdnBtch_ItmRate").val();
            //var SUserID = FBatchDetails[i].UserID;
            //var SRowID = FBatchDetails[i].RowSNo;
            //var SItemID = FBatchDetails[i].ItemID;
            //var SUserID = btchrowcount[i].UserID;
            //var SRowID = btchrowcount[i].RowSNo;
            //var SItemID = btchrowcount[i].ItemID;
            var mfg_date = "";
            /*if (FBatchDetails[i].BatchExDate != "" && FBatchDetails[i].BatchExDate != null) {*/
            if (SBatchMfgDate != "" && SBatchMfgDate != null) {
                if (SBatchMfgDate == "1900-01-01") {
                    mfg_date = "";
                }
                else {
                    mfg_date = moment(SBatchMfgDate).format('DD-MM-YYYY');
                }
            }

            debugger;
            if (Sno != null && Sno != "") {
                if (/*SRowID == Sno &&*/ SItemID == ItemID) {
                    var date = "";
                    /*if (FBatchDetails[i].BatchExDate != "" && FBatchDetails[i].BatchExDate != null) {*/
                    if (SBatchExDate != "" && SBatchExDate != null) {
                        if (SBatchExDate == "1900-01-01") {
                            date = "";
                        }
                        else {
                            date = moment(SBatchExDate).format('DD-MM-YYYY');
                        }
                    }

                    debugger;
                    RenderHtmlForBatch(++rowIdx, GreyScale, SBatchQty, SBatchNo, date, SBatchExDate
                        , SBatchMfgName, SBatchMfgMrp, mfg_date, SBatchMfgDate)
//                    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
//<td id="BatchQty" class="num_right">${parseFloat(SBatchQty).toFixed(QtyDecDigit)}</td>
//<td id="BatchNo" >${SBatchNo}</td>
//<td id="bt_mfg_name" >${SBatchMfgName}</td>
//<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(SBatchMfgMrp)).toFixed(QtyDecDigit)}</td>
//<td id="bt_mfg_date" >${mfg_date}</td>
//<td id="bt_mfg_hdn_date" hidden="hidden">${SBatchMfgDate}</td>
//<td id="BatchExDate" hidden="hidden">${SBatchExDate}</td>
//<td>${date}</td>
//</tr>`);
                }
            }
            else {
                debugger
                if (SItemID == ItemID) {
                    var date = "";
                    /*if (FBatchDetails[i].BatchExDate != "" && FBatchDetails[i].BatchExDate != null) {*/
                    if (SBatchExDate != "" && SBatchExDate != null) {
                        date = moment(SBatchExDate).format('DD-MM-YYYY');
                    }
                    RenderHtmlForBatch(++rowIdx, GreyScale, SBatchQty, SBatchNo, date, SBatchExDate
                        , SBatchMfgName, SBatchMfgMrp, mfg_date, SBatchMfgDate)

//                    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
//<td id="BatchQty" class="num_right">${parseFloat(SBatchQty).toFixed(QtyDecDigit)}</td>
//<td id="BatchNo" >${SBatchNo}</td>
//<td id="bt_mfg_name" >${SBatchMfgName}</td>
//<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(SBatchMfgMrp)).toFixed(QtyDecDigit)}</td>
//<td id="bt_mfg_date" >${mfg_date}</td>
//<td id="bt_mfg_hdn_date" hidden="hidden">${SBatchMfgDate}</td>
//<td id="BatchExDate" hidden="hidden">${SBatchExDate}</td>
//<td>${date}</td>
//</tr>`);
                }
            }

            //}
        });
        debugger;
        if (EnableBatchdetail == "Enable") {
            OnClickDeleteIconSTK();
        }
        debugger;
        CalculateBatchQtyTblSTK();

        var EDStatus = sessionStorage.getItem("PopUpEnableDisable");
        if (EDStatus == "Disabled") {
            $("#BatchDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                currentRow.find("#BatchDeleteIcon").css("display", "none");
            });
        }
    }
    else {
        $("#BatchDetailTbl >tbody >tr").remove();
        $("#BatchQtyTotal").text("");
    }
    //}
}
function RenderHtmlForBatch(rowIdx, GreyScale, SBatchQty, BatchNo, ExDate,ExDateHdn, MfgName, MfgMrp, MfgDate, MfgDateHdn) {
    $('#BatchDetailTbl tbody').append(`<tr id="R${rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(SBatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${BatchNo}</td>
<td id="bt_mfg_name" >${MfgName}</td>
<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(MfgMrp)) == "0" ? "" : parseFloat(CheckNullNumber(MfgMrp)).toFixed(QtyDecDigit)}</td>
<td id="bt_mfg_date" >${MfgDate}</td>
<td id="bt_mfg_hdn_date" hidden="hidden">${MfgDateHdn}</td>
<td id="BatchExDate" hidden="hidden">${ExDateHdn}</td>
<td>${ExDate}</td>
</tr>`);
}
function RenderHtmlForSerial(rowIdx, GreyScale, tblSrNo, SerialNo, MfgName, MfgMrp, MfgDate, MfgDateHdn) {
    $('#SerialDetailTbl tbody').append(`<tr id="R${rowIdx}">
                    <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${tblSrNo}</td>
<td id="SerialNo" >${SerialNo}</td>
<td id="sr_mfg_name" >${MfgName}</td>
<td id="sr_mfg_mrp" class="num_right" >${parseFloat(CheckNullNumber(MfgMrp)) == "0" ? "" : parseFloat(CheckNullNumber(MfgMrp)).toFixed(QtyDecDigit)}</td>
<td id="sr_mfg_date" >${MfgDate}</td>
<td id="sr_mfg_hdn_date" hidden="hidden">${MfgDateHdn}</td>
                </tr>`);
}
function OnClickAddNewBatchDetailSTK() {
    var rowIdx = 0;
    var ValidInfo = "N";
    //var QtyDecDigit = $("#QtyDigit").text();

    if ($('#BatchQuantity').val() == "0" || $('#BatchQuantity').val() == "") {
        ValidInfo = "Y";
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
    }
    else {
        $("#SpanBatchQty").css("display", "none");
        $("#BatchQuantity").css("border-color", "#ced4da");
        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(cmn_QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(cmn_QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(cmn_QtyDecDigit);

        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);

            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(cmn_QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(cmn_QtyDecDigit);
        });

        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(cmn_QtyDecDigit);
        //Math.fround(ItmRate) > Math.fround(DisAmt)
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            //$('#BatchQuantity').val("");
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(cmn_QtyDecDigit));
        }
    }
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

    if (ValidInfo == "Y") {
        return false;
    }
    
    var mfg_date = Cmn_FormatDate_ddmmyyyy($("#txtBtMfgDate").val());
    var exp_date = Cmn_FormatDate_ddmmyyyy($("#BatchExpiryDate").val());

    RenderHtmlForBatch(++rowIdx, "", $("#BatchQuantity").val(), $("#txtBatchNumber").val(), exp_date, $("#BatchExpiryDate").val()
        , $("#txtBtMfgName").val(), $("#txtBtMfgMRP").val(), mfg_date, $("#txtBtMfgDate").val())
//    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
//<td id="BatchQty" class="num_right">${parseFloat($("#BatchQuantity").val()).toFixed(cmn_QtyDecDigit)}</td>
//<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
//<td id="bt_mfg_name" >${$("#txtBtMfgName").val()}</td>
//<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber($("#txtBtMfgMRP").val())).toFixed(QtyDecDigit)}</td>
//<td id="bt_mfg_date" >${mfg_date}</td>
//<td id="bt_mfg_hdn_date" hidden="hidden">${$("#txtBtMfgDate").val()}</td>
//<td id="BatchExDate" >${$("#BatchExpiryDate").val()}</td>
//</tr>`);

    ResetBatchDetailValSTK("Mfg_DoNotReset");
    CalculateBatchQtyTblSTK();
    OnClickDeleteIconSTK();
}
function OnClickSaveAndClose() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ItemID = $('#hfItemID').val();
    var RowSNo = $("#hfItemSNo").val();
    var UserID = $("#UserID").text();

    var ReceiBQty = parseFloat($("#BatchReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQtyTotal").text()).toFixed(QtyDecDigit);
    var TotalReceQty = parseFloat(ReceiBQty);
    var SumofAllTotal = parseFloat(TotalBQty);

    if (parseFloat(TotalReceQty) == parseFloat(SumofAllTotal)) {

        $("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        //$("#PopUphfItemID" + RowSNo).closest("tr").find("#BtnBatchDetail").css("border-color", "");
        //ValidateEyeColor($("#PopUphfItemID" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
        $("#PopUphfItemID").closest("tr").find("#BtnBatchDetail").css("border-color", "");
        var filter = $("#PopUp_STKAddTable >tbody >tr td #PopUphfItemID[value='" + ItemID + "']").closest('tr');
        ValidateEyeColor(filter, "BtnBatchDetail", "N");
        debugger;
       var hdnbtchLen= $("#hdn_BatchDetailTbl TBODY TR").length;
        if (hdnbtchLen> 0) {
            $("#hdn_BatchDetailTbl >tbody >tr td #hdnBtch_ItemID[value=" + ItemID + "]").closest('tr').remove();
            debugger;
            
                var Batch_DetailList = [];
                var hdn_BatchDetail = [];
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var BatchQty = currentRow.find("#BatchQty").text();
                    var BatchNo = currentRow.find("#BatchNo").text();
                    var BatchExDate = currentRow.find("#BatchExDate").text();
                    var mfg_name = currentRow.find("#bt_mfg_name").text();
                    var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
                    var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
                    //var BatchItmRate = currentRow.find("#BatchItmRate").text();

                    Batch_DetailList.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, /*RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty,*/ BatchNo: BatchNo, BatchExDate: BatchExDate
                        , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                    })
                });
                hdn_BatchDetail = Batch_DetailList;
                for (var y = 0; y < hdn_BatchDetail.length; y++) {

                    var UserID = hdn_BatchDetail[y].UserID;
                    var RowSNo = hdn_BatchDetail[y].RowSNo;
                    var ItemID = hdn_BatchDetail[y].ItemID;
                    var BatchQty = hdn_BatchDetail[y].BatchQty;
                    var BatchNo = hdn_BatchDetail[y].BatchNo;
                    var BatchExDate = hdn_BatchDetail[y].BatchExDate;
                    //var BatchItmRate = hdn_BatchDetail[y].BatchItmRate;
                  var row=  $("#hdn_BatchDetailTbl >tbody >tr td #hdnBtch_ItemID[value=" + ItemID + "]").closest('tr');
                    debugger;
                    if (row > 0)
                    {
                    }
                    else
                    {
                        $("#hdn_BatchDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="hdnBtch_UserID" value='${UserID}'></td>
                                    <td><input type="text" id="hdnBtch_RowSNo" value='${RowSNo}'></td>
                                    <td><input type="text" id="hdnBtch_ItemID" value='${ItemID}'></td>
                                    <td><input type="text" id="hdnBtch_BatchQty" value='${BatchQty}'></td>
                                    <td><input type="text" id="hdnBtch_BatchNo" value='${BatchNo}'></td>
                                    <td><input type="text" id="hdnBtch_BatchExDate" value='${BatchExDate}'></td>
                                    <td id="hdnBtch_mfg_name" >${hdn_BatchDetail[y].mfg_name}</td>
                                    <td><input type="text" id="hdnBtch_mfg_mrp" value='${hdn_BatchDetail[y].mfg_mrp}'></td>
                                    <td><input type="text" id="hdnBtch_mfg_date" value='${hdn_BatchDetail[y].mfg_date}'></td>
                                    
                        </tr>`);

                    }
                }
        }
            else {
            var BatchDetailList = [];
            var hdnBatchDetail = [];
            $("#BatchDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var BatchQty = currentRow.find("#BatchQty").text();
                var BatchNo = currentRow.find("#BatchNo").text();
                var BatchExDate = currentRow.find("#BatchExDate").text();
                var mfg_name = currentRow.find("#bt_mfg_name").text();
                var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
                var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
                //var BatchItmRate = currentRow.find("#BatchItmRate").text();

                BatchDetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });
            hdnBatchDetail = BatchDetailList;
            for (var y = 0; y < hdnBatchDetail.length; y++) {
                
                var UserID = hdnBatchDetail[y].UserID;
                var RowSNo = hdnBatchDetail[y].RowSNo;
                var ItemID = hdnBatchDetail[y].ItemID;
                var BatchQty = hdnBatchDetail[y].BatchQty;
                var BatchNo = hdnBatchDetail[y].BatchNo; 
                var BatchExDate = hdnBatchDetail[y].BatchExDate;
                
                
                $("#hdn_BatchDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="hdnBtch_UserID" value='${UserID}'></td>
                                    <td><input type="text" id="hdnBtch_RowSNo" value='${RowSNo}'></td>
                                    <td><input type="text" id="hdnBtch_ItemID" value='${ItemID}'></td>
                                    <td><input type="text" id="hdnBtch_BatchQty" value='${BatchQty}'></td>
                                    <td><input type="text" id="hdnBtch_BatchNo" value='${BatchNo}'></td>
                                    <td><input type="text" id="hdnBtch_BatchExDate" value='${BatchExDate}'></td>
                                    <td id="hdnBtch_mfg_name" >${hdnBatchDetail[y].mfg_name}</td>
                                    <td><input type="text" id="hdnBtch_mfg_mrp" value='${hdnBatchDetail[y].mfg_mrp}'></td>
                                    <td><input type="text" id="hdnBtch_mfg_date" value='${hdnBatchDetail[y].mfg_date}'></td>                </tr>`);
            }

        }

        $("#PopUp_STKAddTable >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            //var Sno = clickedrow.find("#PopupSNohf").text().trim();
            //var scrap_ItemID = clickedrow.find("#PopUphfItemID" + Sno).val();
            var Sno = clickedrow.find("#PopupSNohf").val();
            var scrap_ItemID = clickedrow.find("#PopUphfItemID").val();

            if (scrap_ItemID == ItemID) {
                /*clickedrow.find("#BtnBatchDetail" + Sno).css("border-color", "#007bff");*/
                clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                //ValidateEyeColor(clickedrow, "BtnBatchDetail" + Sno, "Y");
                //ValidateEyeColor(clickedrow, "BtnBatchDetail", "Y");
                //ValidateEyeColor($("#PopUphfItemID" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
            }
        });
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    }

}
//function OnChangeBatchQty() {
//    debugger;
    
//    var QtyDecDigit = $("#QtyDigit").text();
//    $("#BatchQuantity").css("border-color", "#ced4da");
//    if ($('#BatchQuantity').val() != "0" && $('#BatchQuantity').val() != "" && $('#BatchQuantity').val() != "0") {
//        $("#SpanBatchQty").css("display", "none");
//        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit);
//        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(QtyDecDigit);

//        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

//        $("#BatchDetailTbl >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
//            if (tblQty == null || tblQty == "") {
//                tblQty = 0;
//            }
//            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
//        });

//        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
//        //Math.fround(ItmRate) > Math.fround(DisAmt)
//        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
//            //$('#BatchQuantity').val("");
//            $("#BatchQuantity").css("border-color", "Red");
//            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
//            $("#SpanBatchQty").css("display", "block");
//            return false;
//        }
//        else {
//            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
//        }
//    }
//    else {
//        $('#BatchQuantity').val("0");
//        $("#BatchQuantity").css("border-color", "Red");
//        $('#SpanBatchQty').text($("#valueReq").text());
//        $("#SpanBatchQty").css("display", "block");
//    }
//}
function OnKeyPressBatchNo() {
   
    if (AvoidChar(el, "QtyDigit") == false) {
        return false;
    }
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
}
function OnChangeBatchExpiryDate() {
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
function ResetBatchDetailValSTK(flag) {
    //debugger;
    $('#BatchQuantity').val("");
    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    //$('#RejBatchQuantity').val("0");
    //$('#ReworkBatchQuantity').val("0");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
    if (flag != "Mfg_DoNotReset") {
        $('#txtBtMfgName').val("");
        $('#txtBtMfgMRP').val("");
        $('#txtBtMfgDate').val("");
    }
}
function CalculateBatchQtyTblSTK() {
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
function OnClickBatchResetBtn() {
    var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValSTK();
    $('#BatchDetailTbl tbody tr').remove();
    $('#BatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));
    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
}
function OnClickDeleteIconSTK() {
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
        var ItemID = $("#hfItemID").val();
        //var SNo = $(this).closest('tr')[0].cells[2].children[0].value;
        //var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        $("#hdn_BatchDetailTbl tbody tr td #hdnBtch_ItemID[value=" + ItemID + "]").closest("tr").remove();
        //$("#BatchDetailTbl tbody tr td #BatchNo[value=" + Btch_BatchNo + "]").closest("tr").remove();

        CalculateBatchQtyTblSTK();

    });
}
function OnClickDiscardAndExit() {
    OnClickBatchResetBtn();
}
function QtyFloatValueonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnKeyPressBatchNoGRN(e) {
    debugger
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

}
function OnKeyDownBatchNoGRN(e) {
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeGRNBatchNo(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    $("#txtBatchNumber").val(data)
}
/*----------------------For PopUp Serial Detail----------------------*/


function OnClickSerialDetailBtnPopUp(e) {
    debugger;
    var DisableSubItem = $("#DisableSubItem").val();
    if (DisableSubItem == "Y") {
        $("#DivDownloadImportExcelSerial").hide();
    }
    else {
        $("#DivDownloadImportExcelSerial").show();
    }
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();
     
    var errorFlag = "N";
    var ItemName;
    var PopupQty;
    var ItmRate;
    var WhId;

    var RowNo;
    var clickedrow = $(e.target).closest("tr"); 
    //RowNo = clickedrow.find("#PopUpSpanRowId").text();
    RowNo = clickedrow.find("#PopupSNohf").val();
    //WhId = clickedrow.find("#PopUpwhid_" + RowNo).val();
    WhId = clickedrow.find("#PopupWh_Id").val();
    Item_ID = clickedrow.find("#PopUpDDLItemname_" + RowNo).val();
    ItemUOMID = clickedrow.find("#UOMIDPopup").val();
    PopupQty = clickedrow.find("#PopupStockQuantity").val();
    ItmRate = clickedrow.find("#PopUpItmPrice").val();
    Serialable = clickedrow.find("#hfserialable").val();
    
    //if (WhId == null || WhId == "" || WhId == "0") {
    //    clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid red");
    //    clickedrow.find("#PopUpwh_Error").css("display", "block");
    //    clickedrow.find("#PopUpwh_Error").text($("#valueReq").text());
    //    errorFlag = "Y";
    //} else {
    //    clickedrow.find("[aria-labelledby='select2-PopUpwhid_" + RowNo + "-container']").css("border", "1px solid #aaa");
    //    clickedrow.find("#PopUpwh_Error").css("display", "none");
    //    clickedrow.find("#PopUpwh_Error").text("");
    //}
    if (Item_ID == "0") {
        clickedrow.find("#PopUpItemNameError").text($("#valueReq").text());
        clickedrow.find("#PopUpItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + RowNo + "-container']").css("border", "1px solid red");
        clickedrow.find("#PopUpDDLItemname_" + RowNo).css("border", "1px solid red");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#PopUpItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-PopUpDDLItemname_" + RowNo + "-container']").css("border", "1px solid #aaa");
    }
    if (PopupQty == "" || PopupQty == 0 || Item_ID == "0") {
        clickedrow.find("#PopupStockQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#PopupStockQuantity_Error").css("display", "block");
        clickedrow.find("#PopupStockQuantity").css("border-color", "red");
        clickedrow.find("#PopupStockQuantity").val("");
        errorFlag = "Y";
        //clickedrow.find("#PopupStockQuantity").focus();

    }
    else {
        clickedrow.find("#PopupStockQuantity_Error").text("");
        clickedrow.find("#PopupStockQuantity_Error").css("display", "none");
        clickedrow.find("#PopupStockQuantity").css("border-color", "#ced4da");
        clickedrow.find("#PopupStockQuantity").val(parseFloat(PopupQty).toFixed(QtyDecDigit));
    }
    if (ItmRate == "" || ItmRate == 0 || Item_ID == "0") {
        clickedrow.find("#PopUpItmPrice_Error").text($("#valueReq").text());
        clickedrow.find("#PopUpItmPrice_Error").css("display", "block");
        clickedrow.find("#PopUpItmPrice").css("border-color", "red");
        errorFlag = "Y";

    }
    else {
        clickedrow.find("#PopUpItmPrice_Error").css("display", "none");
        clickedrow.find("#PopUpItmPrice").css("border-color", "#ced4da");
        clickedrow.find("#PopUpItmPrice").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    }
    if (errorFlag == "Y") {
        $("#BtnSerialDetail").attr("data-target", "");
        return false;
    }

    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");

    var Sno = clickedrow.find("#PopUpSpanRowId").text();
    //if (Sno == "") {
    //    Sno = clickedrow.find("#PopupSNohf").val();
    //}
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ExpiryFlag = "";
    var ReceiveQty = 0;

    /*ItemName = clickedrow.find("#PopUpDDLItemname_" + Sno + " option:selected").text();*/
    ItemName = clickedrow.find("#PopUphfItemName").val();
    ItemID = clickedrow.find("#PopUphfItemID").val();
    ItemUOM = clickedrow.find("#UOMPopup").val();
    //ExpiryFlag = clickedrow.find("#hfexpiralble").val();
    ReceiveQty = clickedrow.find("#PopupStockQuantity").val();
   
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
    $("#SerialUOMId").val(ItemUOMID);
    $("#hfSCostPrice").val(ItmRate);
    $("#hfSeriable").val(Serialable);

    $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    $("#hfSItemSNo").val(Sno);
    $("#hfSItemID").val(ItemID);



    var rowIdx = 0;
    //var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    //if (FSerialDetails != null) {
    var serlrowcount = $('#hdn_SerialDetailTbl tr').length;
    /*if (FSerialDetails.length > 0) {*/
         if (serlrowcount> 0) {
            $("#SerialDetailTbl >tbody >tr").remove();
            var EnableSerialdetail = $("#EnableSerial_detail").val();
            var GreyScale = "";
            if (EnableSerialdetail == "Disable") {
                GreyScale = "style='filter: grayscale(100%)'";
            }
         
           $("#hdn_SerialDetailTbl TBODY TR").each(function () {
               var row = $(this);
               var SUserID = row.find("#hdnSrl_UserID").val();
               var SRowID = row.find("#hdnSrl_RowSNo").val();
               var SItemID = row.find("#hdnSrl_ItemID").val();
               var MfgName = row.find("#hdnSrl_MfgName").val();
               var MfgMrp = row.find("#hdnSrl_MfgMrp").val();
               var MfgDate = row.find("#hdnSrl_MfgDate").val();
               //var SSerialQty = row.find("#hdnSrl_SerialQty").val();
               var SSerialNo = row.find("#hdnSrl_SerialNo").val();
               debugger;
               if (Sno != null && Sno != "") {
                   if (/*SRowID == Sno && */SItemID == ItemID)
                   {
                       var mfg_date = "";
                       if (MfgDate != null && MfgDate != "" && MfgDate!="1900-01-01") {
                           mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                       }
                       else {
                           mfg_date = "";
                       }
                       RenderHtmlForSerial(++rowIdx, GreyScale, rowIdx, SSerialNo, MfgName, MfgMrp, mfg_date, MfgDate);
                       //$('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                       // <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="Delete"></i></td>
                       // <td id="SerialID" >${rowIdx}</td>
                       // <td id="SerialNo" >${SSerialNo}</td>
                       // <td id="sr_mfg_name" >${MfgName}</td>
                       // <td id="sr_mfg_mrp" class="num_right" >${MfgMrp}</td>
                       // <td id="sr_mfg_date" >${mfg_date}</td>
                       // <td id="sr_mfg_hdn_date" hidden >${MfgDate}</td>
                       // </tr>`);
                   }
               }
               else {
                   debugger
                   if (SItemID == ItemID) {
                       var mfg_date = "";
                       if (MfgDate != null && MfgDate != "" && MfgDate != "1900-01-01") {
                           mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                       }
                       else {
                           mfg_date = "";
                       }
                       RenderHtmlForSerial(++rowIdx, GreyScale, rowIdx, SSerialNo, MfgName, MfgMrp, mfg_date, MfgDate);
                       //$('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                       //  <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="Delete"></i></td>
                       //  <td id="SerialID" >${rowIdx}</td>
                       //  <td id="SerialNo" >${SSerialNo}</td>
                       // <td id="sr_mfg_name" >${MfgName}</td>
                       // <td id="sr_mfg_mrp" class="num_right">${MfgMrp}</td>
                       // <td id="sr_mfg_date" >${mfg_date}</td>
                       // <td id="sr_mfg_hdn_date" hidden>${MfgDate}</td>
                       //  </tr>`);
                   }
               }

               //}
           });
           debugger;

//            for (i = 0; i < FSerialDetails.length; i++) {
//                var SUserID = FSerialDetails[i].UserID;
//                var SRowID = FSerialDetails[i].RowSNo;
//                var SItemID = FSerialDetails[i].ItemID;
//                debugger;
//                if (Sno != null && Sno != "") {
//                    if (SRowID == Sno && SItemID == ItemID) {
//                        debugger;
//                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="Delete"></i></td>
//<td id="SerialID" >${rowIdx}</td>
//<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
//</tr>`);
//                    }
//                }
//                else {
//                    debugger
//                    if (SItemID == ItemID) {
//                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="Delete"></i></td>
//<td id="SerialID" >${rowIdx}</td>
//<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
//</tr>`);
//                    }
//                }
//            }
            if (EnableSerialdetail == "Enable") {
                OnClickSerialDeleteIconSTK();
            }


            var EDStatus = sessionStorage.getItem("PopUpEnableDisable");
            if (EDStatus == "Disabled") {
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#SerialDeleteIcon").css("display", "none");
                });
            }
        }
         else {
             $("#SerialDetailTbl >tbody >tr").remove();
             $("#SerialNo").val("");
         }
    //}
    //else {
    //    $("#SerialDetailTbl >tbody >tr").remove();
    //}
}
function OnClickAddNewSerialDetailSTK() {
    var QtyDecDigit = $("#QtyDigit").text();
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var AcceptLength = parseInt(0);
    var ItemID = $("#hfSItemID").val()
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
                //$('#SerialNo').val("");
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
            var mfg_date = $("#txtSrMfgDate").val();
            if (mfg_date != null && mfg_date != "") {
                mfg_date = moment(mfg_date).format('DD-MM-YYYY');
            }
            else {
                mfg_date = "";
            }
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            RenderHtmlForSerial(++rowIdx, "", TblLen + 1, $("#SerialNo").val(), $("#txtSrMfgName").val(), $("#txtSrMfgMRP").val(), mfg_date, $("#txtSrMfgDate").val());
//                $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
//                    <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
//<td id="SerialID" >${TblLen + 1}</td>
//<td id="SerialNo" >${$("#SerialNo").val()}</td>
//<td id="sr_mfg_name" >${$("#txtSrMfgName").val()}</td>
//<td id="sr_mfg_mrp" class="num_right" >${parseFloat(CheckNullNumber($("#txtSrMfgMRP").val())).toFixed(QtyDecDigit)}</td>
//<td id="sr_mfg_date" >${mfg_date}</td>
//<td id="sr_mfg_hdn_date" hidden="hidden">${$("#txtSrMfgDate").val()}</td>
//                </tr>`);

                $('#SerialNo').val("");
                OnClickSerialDeleteIconSTK();
            }
       
    }
    

}
function OnClickSerialSaveAndCloseSTK() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();

    var ItemID = $('#hfSItemID').val();
    var RowSNo = $("#hfSItemSNo").val();
    var UserID = $("#UserID").text();

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);

    var TotalRecQty = parseFloat(ReceiSQty)
    var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);
    if (parseFloat(TotalRecQty) == parseFloat(TotalSQty)) {

        $("#SerialSaveAndExitBtn").attr("data-dismiss", "modal");
        var filter = $("#PopUp_STKAddTable >tbody >tr td #PopUphfItemID[value='" + ItemID + "']").closest('tr');
        ValidateEyeColor(filter, "BtnSerialDetail", "N");
        //ValidateEyeColor($("#PopUphfItemID").closest("tr"), "BtnSerialDetail", "N");
        //let NewArr = [];
        //var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
        //if (FSerialDetails != null) {
        debugger;
        var hdnSrlLen = $("#hdn_SerialDetailTbl TBODY TR").length;
        if (hdnSrlLen > 0) {
            $("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + ItemID + "]").closest('tr').remove();
            debugger;

            var Serial_DetailList = [];
            var hdn_SerialDetail = [];
            $("#SerialDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                //var SerialQty = currentRow.find("#SerialQty").text();
                var SerialNo = currentRow.find("#SerialNo").text();
                var MfgName = currentRow.find("#sr_mfg_name").text();
                var MfgMrp = currentRow.find("#sr_mfg_mrp").text();
                var MfgDate = currentRow.find("#sr_mfg_hdn_date").text();
                
                Serial_DetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, /*SerialQty: SerialQty,*/ SerialNo: SerialNo
                    , MfgName: MfgName, MfgMrp: IsNull(MfgMrp, "") == "" ? "0" : MfgMrp, MfgDate: MfgDate
                })
            });
            hdn_SerialDetail = Serial_DetailList;
            for (var y = 0; y < hdn_SerialDetail.length; y++) {

                var UserID = hdn_SerialDetail[y].UserID;
                var RowSNo = hdn_SerialDetail[y].RowSNo;
                var ItemID = hdn_SerialDetail[y].ItemID;
                //var SerialQty = hdn_SerialDetail[y].SerialQty;
                var SerialNo = hdn_SerialDetail[y].SerialNo;
                var row = $("#hdn_SerialDetailTbl >tbody >tr td #hdnSrl_ItemID[value=" + ItemID + "]").closest('tr');
                debugger;
                if (row > 0) {
                }
                else {
                    $("#hdn_SerialDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="hdnSrl_UserID" value='${UserID}'></td>
                                    <td><input type="text" id="hdnSrl_RowSNo" value='${RowSNo}'></td>
                                    <td><input type="text" id="hdnSrl_ItemID" value='${ItemID}'></td>
                                     <td><input type="text" id="hdnSrl_SerialNo" value='${SerialNo}'></td>
                                     <td><input type="text" id="hdnSrl_MfgName" value='${hdn_SerialDetail[y].MfgName}'></td>
                                     <td><input type="text" id="hdnSrl_MfgMrp" value='${hdn_SerialDetail[y].MfgMrp}'></td>
                                     <td><input type="text" id="hdnSrl_MfgDate" value='${hdn_SerialDetail[y].MfgDate}'></td>
                                     </tr>`);

                }
            } /*<td><input type="text" id="hdnSrl_SerialQty" value='${SerialQty}'></td>*/



        }
        else {
            var SerialDetailList = [];
            var hdnSerialDetail = [];
            $("#SerialDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                //var SerialQty = currentRow.find("#SerialQty").text();
                var SerialNo = currentRow.find("#SerialNo").text();
                var MfgName = currentRow.find("#sr_mfg_name").text();
                var MfgMrp = currentRow.find("#sr_mfg_mrp").text();
                var MfgDate = currentRow.find("#sr_mfg_hdn_date").text();
                SerialDetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, /*SerialQty: SerialQty,*/ SerialNo: SerialNo
                    , MfgName: MfgName, MfgMrp: IsNull(MfgMrp, "") == "" ? "0" : MfgMrp, MfgDate: MfgDate
                })
            });
            hdnSerialDetail = SerialDetailList;
            for (var y = 0; y < hdnSerialDetail.length; y++) {
                var UserID = hdnSerialDetail[y].UserID;
                var RowSNo = hdnSerialDetail[y].RowSNo;
                var ItemID = hdnSerialDetail[y].ItemID;
                //var SerialQty = hdnSerialDetail[y].SerialQty;
                var SerialNo = hdnSerialDetail[y].SerialNo;
                $("#hdn_SerialDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="hdnSrl_UserID" value='${UserID}'></td>
                                    <td><input type="text" id="hdnSrl_RowSNo" value='${RowSNo}'></td>
                                    <td><input type="text" id="hdnSrl_ItemID" value='${ItemID}'></td>
                                    <td><input type="text" id="hdnSrl_SerialNo" value='${SerialNo}'></td>
                                     <td><input type="text" id="hdnSrl_MfgName" value='${hdnSerialDetail[y].MfgName}'></td>
                                     <td><input type="text" id="hdnSrl_MfgMrp" value='${hdnSerialDetail[y].MfgMrp}'></td>
                                     <td><input type="text" id="hdnSrl_MfgDate" value='${hdnSerialDetail[y].MfgDate}'></td>
                                    </tr>`);
            }

            //sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
            //var FFBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));

        }

            //if (FSerialDetails.length > 0) {
            //    for (i = 0; i < FSerialDetails.length; i++) {
            //        var SUserID = FSerialDetails[i].UserID;
            //        var SRowID = FSerialDetails[i].RowSNo;
            //        var SItemID = FSerialDetails[i].ItemID;
            //        if (SRowID == RowSNo && SItemID == ItemID) {
            //        }
            //        else {
            //            NewArr.push(FSerialDetails[i]);
            //        }
            //    }
            //    $("#SerialDetailTbl >tbody >tr").each(function () {
            //        var currentRow = $(this);
            //        var SerialNo = currentRow.find("#SerialNo").text();
            //        NewArr.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo })
            //    });
            //    sessionStorage.removeItem("SerialDetailSession");
            //    sessionStorage.setItem("SerialDetailSession", JSON.stringify(NewArr));
            //}
            //else {
            //    var SerialDetailList = [];
            //    $("#SerialDetailTbl >tbody >tr").each(function () {
            //        var currentRow = $(this);
            //        var SerialNo = currentRow.find("#SerialNo").text();

            //        SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo })
            //    });
            //    sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
            //}
        //}
        //else {
        //    var SerialDetailList = [];
        //    $("#SerialDetailTbl >tbody >tr").each(function () {
        //        var currentRow = $(this);
        //        var SerialNo = currentRow.find("#SerialNo").text();

        //        SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo })
        //    });
        //    sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
        //}
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function OnKeyPressSerialNoSTK() {
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialResetBtnSTK() {
    $('#SerialNo').val("");
    $('#txtSrMfgName').val("");
    $('#txtSrMfgMRP').val("");
    $('#txtSrMfgDate').val("");
    $('#SerialDetailTbl tbody tr').remove();
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDiscardAndExitSTK() {
    OnClickSerialResetBtnSTK();
}
function OnClickSerialDeleteIconSTK() {
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
        var ItemID = $("#hfItemID").val();
        var RowSNo = $("#hfSItemSNo").val();
        //$("#hdn_SerialDetailTbl tbody tr td #hdnSrl_SerialNo[value=" + RowSNo + "]").closest("tr").remove();
        //$("#SerialDetailTbl tbody tr td #SerialNo[value=" + RowSNo + "]").closest("tr").remove();
        ResetSerialNoAfterDeleteSTK();
    });
}
function ResetSerialNoAfterDeleteSTK() {
    var rowIdx = 0;
    $("#SerialDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#SerialID").text(++rowIdx);
    });
}
/***--------------------------------For PopUp Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "PopUp_subitem", "SubItemPopUpQty");
}
function StKPopUp_SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var SRNO = clickdRow.find("#PopupSNohf").val();
    var ProductNm = clickdRow.find("#PopUpDDLItemname_" + SRNO + " option:selected").text();
    var ProductId = clickdRow.find("#PopUphfItemID").val();
    var UOM = clickdRow.find("#UOMPopup").val();
    clickdRow.find("#hdnPopupFlag").val(flag);
    
    var Sub_Quantity = 0;

    var NewArr = new Array();
    if (flag == "PopUpStk_Qty") {
        $("#hdn_PopUpStkSub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var ListStk = {};
            ListStk.item_id = row.find("#ItemId").val();
            ListStk.sub_item_id = row.find('#subItemId').val();
            ListStk.qty = row.find('#subItemQty').val();
            NewArr.push(ListStk);
        });
        Sub_Quantity = clickdRow.find("#PopupStockQuantity").val();
    }
    
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#StatusCode").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/StockTake/GetSubItemDetails_StkPopUp",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status
            
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });
}
function DeleteSubItemQtyDetailForStkPopup(item_id) {
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_PopUpStkSub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_PopUpStkSub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").length > 0) {
                $("#hdn_PopUpStkSub_ItemDetailTbl >tbody >tr td #ItemId[value=" + item_id + "]").closest('tr').remove();
            }
        }
    }

}
function CheckValidations_forSubItemsforNewPopup(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    
        QtyDecDigit = $("#QtyDigit").text();
    
   
    //var hdnPopUpFlagStk = $("#hdnPopupFlag").val();
    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        } 
            //else {
        //    if (docid == "105103130" || docid == "105103145115") {
        //        item_id = $("#" + Item_field_id).val();
        //    }
        //    else {
        //        item_id = PPItemRow.find("#" + Item_field_id).val();
        //    }

        //}

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        
        
        
       /* else if (docid == "105102155" && hdnPopUpFlagStk == "PopUpStk_Qty") {*/
            var sub_item = PPItemRow.find("#PopUp_subitem").val();
        //}
        var Sub_Quantity = 0;

        var SumitemTableName = "";
        
       /* else if (docid == "105102155" && hdnPopUpFlagStk == "PopUpStk_Qty") {*/
            $("#hdn_PopUpStkSub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {
                debugger;
                var Crow = $(this).closest("tr");
                var subItemQty = Crow.find("#subItemQty").val();
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            });
        /*}*/
       
        

        if (sub_item == "Y") {
            if (parseFloat(item_PrdQty) != parseFloat(Sub_Quantity)) {
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
function CheckValidations_forNewPopupSubItems() {
    debugger;
    //var hdnPopUpFlagStk = $("#hdnPopupFlag").val();
    //if (hdnPopUpFlagStk == "PopUpStk_Qty") {
    return CheckValidations_forSubItemsforNewPopup("PopUp_STKAddTable", "PopupSNohf", "PopUpDDLItemname_", "PopupStockQuantity", "SubItemPopUpQty", "Y");
    //}
    //else {
    //return Cmn_CheckValidations_forSubItems("StockTakeTbl", "", "hdItemId", "PhysicalStock", "SubItemPhyQty", "Y");
    /*}*/
}

function StokTakeResetWorningBorderColor() {
    debugger;
    return CheckValidations_forSubItemsforNewPopup("PopUp_STKAddTable", "PopupSNohf", "PopUpDDLItemname_", "PopupStockQuantity", "SubItemPopUpQty", "N");
}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "StockTakeTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}


/***--------------------------------Sub Item Section End-----------------------------------------***/

//function RemoveSessionNew() {
//    sessionStorage.removeItem("BatchDetailSession");
//    sessionStorage.removeItem("SerialDetailSession");
//}