$(document).ready(function () {
    $("#ddlShopfloorID").select2();
    $("#ddloperationname").select2();
    var src_type = $("#dllSource_type").val();
    if (src_type == "D") {
        BindFGRProductItemOutPut(1);
        BindFGRProductItemInPut(1);
    }
    else {
        BindProductNameDDL();
    }

    var Doc_No = $("#ReceiptNumber").val();
    $("#hdDoc_No").val(Doc_No);

    $('#ConsumptionItmDetailsTbl').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            //var dig = parseInt(id.substring(1));
            var dig = parseInt(id);
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        var Itemid = $(this).closest('tr').find("#hf_ItemID").val();

        Cmn_DeleteSubItemQtyDetail(Itemid);
        DeleteItemBatchSerialOrderQtyDetails(Itemid);


        updateItemSerialNumber()
    });
    $('#tblIDFinishGoodReceipt').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            // var dig = parseInt(id.substring(1));
            var dig = parseInt(id);
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        debugger;
        var Itemid = $(this).closest('tr').find("#hf_ItemID").val();
        Cmn_DeleteSubItemQtyDetail(Itemid);

        updateOutItemSerialNumber()
    });
    OnchageSource_type();
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#ConsumptionItmDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hf_ItemID').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
})

function OnchangeProductName() {
    debugger;
    var itemID = $("#ddlItemName option:selected").val();
    $("#hdnProductID").val(itemID);
    if (itemID == "0" || itemID == "" || itemID == null) {
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "Red");
        $('#vmProductName').text($("#valueReq").text());
        $("#vmProductName").css("display", "block");
        return false;
    }
    else {
        $("#vmProductName").css("display", "none");
        $("#ddlItemName").css("border-color", "#ced4da");
    }
    $.ajax({
        type: "post",
        url: "/ApplicationLayer/FinishedGoodsReceipt/GetOperationData",
        data: {
            itemID: itemID
        },
        success: function (data) {
            debugger;
            data = JSON.parse(data).Table;
            $("#ddloperationname").val(data[0].op_id).trigger('change');
            $("#Hdnoperation_id").val(data[0].op_id);
            $("#ddlUOM").val(data[0].uom_alias);
            $("#HdnUom_id").val(data[0].uom_id);
            $("#ddloperationname").attr("disabled", "disabled")
        }
    })
}
function BindProductNameDDL() {
    debugger;
    $("#ddlItemName").append("<option value='0'>---Select---</option>");
    $("#ddlItemName").select2({

        ajax: {
            url: "/ApplicationLayer/FinishedGoodsReceipt/GetItemListinDeatil",
            data: function (params) {
                var queryParameters = {
                    ProductName: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize
                data = JSON.parse(data).Table;
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
</strong></li></ul>`)
                }

                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.Item_id, text: val.Item_name, UOM: val.uom_name };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger

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
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#Sno").text(SerialNo);

    });
};
function updateOutItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#tblIDFinishGoodReceipt >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#Sno").text(SerialNo);
    });
};
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    debugger;
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#Batch_ItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#Serial_ItemId").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}
function BindFGRProductItemOutPut(ID) {
    debugger;
    BindItemList("#ItemName_", ID, "#tblIDFinishGoodReceipt", "#hfSNo", "", "FGROutput");
}
function BindFGRProductItemInPut(ID) {
    debugger;
    BindItemList("#Consum_ItemName_", ID, "#ConsumptionItmDetailsTbl", "#hfSNo", "", "FGRInput");
}
function OnchageBindFGR_ItemList(evt) {
    debugger;
    var currentrow = $(evt.target).closest("tr");
    var sno = currentrow.find("#hfSNo").val();
    var itemid = currentrow.find("#ItemName_" + sno).val();
    currentrow.find("#hf_ItemID").val(itemid);
    NullInput_OutPutTableData(currentrow, 'Output', itemid);
    Cmn_BindUOM(currentrow, itemid, sno, "N", "Output");
    DisableHeaderField(itemid);
   
}
function DisableHeaderField(itemid) {
    var Source_type = $("#dllSource_type").val();
    if (itemid == "0" || itemid == "") {

        if (Source_type == "B") {
            $("#ddlItemName").attr("Disabled", false);
        }
        $("#dllSource_type").attr("Disabled", false);
        $("#ddlShopfloorID").attr("Disabled", false);
        $("#ddloperationname").attr("Disabled", false);
        $("#SupervisorName").attr("Disabled", false);
    }
    else {
        if (Source_type == "B") {
            $("#ddlItemName").attr("Disabled", true);
            $("#SupervisorName").attr("Disabled", false);
        }
        else {
            $("#SupervisorName").attr("Disabled", true);
        }
        $("#dllSource_type").attr("Disabled", true);
        $("#ddlShopfloorID").attr("Disabled", true);
        $("#ddloperationname").attr("Disabled", true);

    }
}
function NullInput_OutPutTableData(currentrow, flag, itemid) {
    if (flag == "Output") {
     var sno=  currentrow.find("#hfSNo").val();
        currentrow.find("#OutputQuantity").val("");
        currentrow.find("#OutputQuantityinSpec").val("");
        currentrow.find("#CostPrice").val("");
        currentrow.find("#BatchNumber").val("");
        currentrow.find("#ExpiryDate").val("");
        currentrow.find("#Cremarks").val("");
        currentrow.find("#Cremarks").val("");
        currentrow.find("#QC_" + sno).prop("chacked", false);
        currentrow.find("#hdnQC").val("N");

        Cmn_DeleteSubItemQtyDetail(itemid);
    }
    else if (flag == "Input") {
        currentrow.find("#CConsumedQuantity").val("");
        currentrow.find("#Cremarks").val("");
        Cmn_DeleteSubItemQtyDetail(itemid);
        DeleteItemBatchSerialOrderQtyDetails(itemid);
    }

}
function OnchangeBindFGRConsum_ItemList(evt) {
    //debugger;
    var currentrow = $(evt.target).closest("tr");
    var sno = currentrow.find("#hfSNo").val();
    var itemid = currentrow.find("#Consum_ItemName_" + sno).val();
    currentrow.find("#hf_ItemID").val(itemid);
    Cmn_BindUOM(currentrow, itemid, sno, "Y", "Input");
    AvlStockShopfloor(itemid, currentrow, sno);
    DisableHeaderField(itemid);
    NullInput_OutPutTableData(currentrow, 'Input', itemid);
}


function HideAndShow_BatchSerialButton(row) {
    // $("#ConsumptionItmDetailsTbl tbody tr").each(function () {
    debugger;
    // var row = $(e.target).closest('tr');
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
    //  });
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function Allow_FloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    return true;
}

function AllowFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemButton");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemProduceQty");
}
function AddNewItem(flag) {
    if (flag == "Output") {
        var rowIdx = 0;
        debugger;
        var rowCount = $('#tblIDFinishGoodReceipt >tbody >tr').length + 1;
        var RowNo = 0;

        $("#tblIDFinishGoodReceipt >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
        });
        if (RowNo == "0") {
            RowNo = 1;
        }
        $('#tblIDFinishGoodReceipt tbody').append(`<tr id="R${++rowIdx}">
 <td class="sr_padding" id="Sno">${rowCount}</td>
                                                                    <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz">
                                                                        <div class="col-sm-11 lpo_form"  style="padding:0px;">

                                                                            <select class="form-control" id="ItemName_${RowNo}" name="ProductName" onchange="OnchageBindFGR_ItemList(event)">
                                                                            </select>
                                                                            <input type="hidden" id="hf_ItemID" />
                                                                           <input type="hidden" id="Item_type" value="" />
                                                                            <span id="ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon" style="padding:0px;">
                                                                            <button type="button" id="OP_ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information">  </button>
                                                                        </div>

                                                                    </td>
                                                                    <td>
                                                                        <input id="UOM" class="form-control" autocomplete="off" value="" type="text" name="UOM" disabled="" placeholder="${$("#ItemUOM").text()}">
                                                                        <input type="hidden" id="UOMID" style="display: none;">
                                                                        <input type="hidden" id="UOMName_in_Spec" style="display: none;">
                                                                            <input type="hidden" id="Conv_rate"  style="display: none;">
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form" style="padding:0px;">
                                                                            <div class="lpo_form">

                                                                                <input id="OutputQuantity" class="form-control num_right" value="" autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="OnChangeOutPutQty(event);" placeholder="0000.00" type="text" name="OutputQuantity">
                                                                                <span id="OutputQuantityError" class="error-message is-visible"></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-2 i_Icon" id="Div_SubItemButton1">
                                                                             <input type="hidden" id="sub_item" value="N">
                                                                            <button type="button" class="calculator subItmImg" id="SubItemButton" onclick="return SubItemDetailsPopUp('OutPut',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false" Disabled><img src="/Content/Images/subItem.png" alt="" title="Sub-Item Detail" > </button>
                                                                            <input type="hidden" id="hdn_Subitmflag" value="OutPut" style="display: none;" />
                                                                         </div>


                                                                    </td>
                                                                          <td>
                                                                            <input id="OutputQuantityinSpec" class="form-control num_right" disabled="disabled" value="" autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="" placeholder="0000.00" type="text" name="OutputQuantityinSpec">
                                                                        </td>
                                                                    <td class="lpo_form"><input id="CostPrice" onkeypress="return Allow_FloatQtyonly(this,event)" class="form-control num_right" autocomplete="off" type="text" value="" name="CostPrice" placeholder="000.00" onchange="onchangeCostPrice(event)">
                                                                      <span id="CostPriceError" class="error-message is-visible"></span></td>
                                                                      <td>
                                                                        <div class="lpo_form">
                                                                            <input id="BatchNumber" maxlength="25" class="form-control"  onchange="OnchangeBatchNumber(event)" autocomplete="off" type="text" value="" name="BatchNumber" placeholder="${$("#BatchSpanDetail").text()}">
                                                                            <span id="BatchNumberError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>
                                                                  
                                                                    <td class="lpo_form">
                                                                        <input id="ExpiryDate" class="form-control" autocomplete="off" type="date" value="" name="ExpiryDate" onchange="OnchngeExpiryDate(event)">
                                                                        <input type="hidden" id="hdnExperDate" value="N">
                                                                     <span id="ExpiryDateError" class="error-message is-visible"></span>

                                                                    </td>
                                                                       <td>

                                                                       <div class="custom-control custom-switch sample_issue">
                                                                       <input type="checkbox" class="custom-control-input margin-switch" id="QC_${RowNo}" onchange="OnchangeQC(event)">
                                                                         <label class="custom-control-label" for="QC_${RowNo}"></label>
                                                                       </div>
                                                                          <input type="hidden" id="hdnQC" value="">
                                                                       </td>
 <td>
                                                                                <div class="col-sm-10 lpo_form no-padding">

                                                                                    <input id="AcceptQty" value="" class="form-control num_right" autocomplete="off" type="text" name="AcceptQty" placeholder="0000.00" disabled>



                                                                                </div>
                                                                                <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemAccQty">
                                                                                    <button type="button" id="SubItemAccQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCAccQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div class=" col-sm-8 lpo_form no-padding">


                                                                                    <input id="RejectQty" value="" class="form-control num_right" autocomplete="off" type="text" name="RejectQty" placeholder="0000.00" disabled>

                                                                                    <span id="RejectQty_Error" class="error-message is-visible"></span>
                                                                                </div>

                                                                                <div class=" col-sm-2 i_Icon" id="div_SubItemRejQty" style="padding:0px;">
                                                                                    <button type="button" id="SubItemRejQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCRejQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                <div class=" col-sm-8 lpo_form" style="padding:0px;">


                                                                                    <input id="ReworkableQty" value="" class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>

                                                                                </div>

                                                                                <div class=" col-sm-2 i_Icon" id="div_SubItemRewQty" style="padding:0px;">
                                                                                    <button type="button" id="SubItemRewQty" class="calculator subItmImg" disabled  onclick="return SubItemDetailsPopUp('QCRewQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>
                                                                                
                                                                            </td>
                                                                    <td>
                                                                        <textarea id="Cremarks" maxlength="100" class="form-control" name="remarks" value="" style="height:26px;" placeholder="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                    <td style="display: none;"><input type="hidden" id="hfSNo" value="${RowNo}" /></td>
</tr>`);
        BindFGRProductItemOutPut(RowNo);
    }
    else if (flag == "Input") {
        var rowIdx = 0;
        debugger;
        var rowCount = $('#ConsumptionItmDetailsTbl >tbody >tr').length + 1;
        var RowNo = 0;
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
        });
        if (RowNo == "0") {
            RowNo = 1;
        }
        $('#ConsumptionItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
          <td class="sr_padding" id="Sno">${rowCount}</td>
                                                                    <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz">
                                                                        <div class="col-sm-11 lpo_form" style="padding:0px;">
                                                                            <select class="form-control" id="Consum_ItemName_${RowNo}" name="ProductName" onchange="OnchangeBindFGRConsum_ItemList(event)">
                                                                            </select>
                                                                            <input type="hidden" id="hf_ItemID" />
                                                                            <input type="hidden" id="CItem_type" />
                                                                            <span id="con_ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon" style="padding:0px;">
                                                                            <button type="button" id="CItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information"> </button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="CUOM" class="form-control" autocomplete="off" value="" type="text" name="UOM" disabled="disabled" placeholder="${$("#ItemUOM").text()}">
                                                                        <input type="hidden" id="hdn_cuom" value="" style="display: none;">
                                                                    </td>
                                                                    <td><input id="ShopFloorstock" class="form-control num_right" autocomplete="off" type="text" value="" name="AvailableStockInShopfloor" disabled="disabled" placeholder="${$("#span_AvailableStock").text()}"></td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form" style="padding:0px;">
                                                                            <div class="lpo_form">
                                                                                <input type="hidden" id="hdnitem_reqqty" value="50" style="display: none;">
                                                                                <input id="CConsumedQuantity" class="form-control num_right" value="" autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="OnChangeConsumedQty(event);" placeholder="0000.00" type="text" name="ConsumedQuantity">
                                                                                <span id="consumedqty_Error" class="error-message is-visible"></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-2 i_Icon" id="div_SubItemConsumeQuantity" style="padding:0px; ">
                                                                            <input type="hidden" id="Consumesub_item" value="N">
                                                                            <button type="button" id="SubItemConsumeQuantity" disabled="" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Input',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="Sub-Item Detail"> </button>
                                                                            <input type="hidden" id="hdn_Subitmflag" value="Input" style="display: none;" />
                                                                          </div>

                                                                  
                                                                    
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" id="btncbatchdeatil" onclick="ItemStockBatchWise(this,event)" disabled="" class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Batch Detail" ></i></button>

                                                                        <input type="hidden" id="hdi_cbatch" value="N" style="display: none;">
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" id="btncserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Serial Detail"></i></button>

                                                                        <input type="hidden" id="hdi_cserial" value="N" style="display: none;">
                                                                    </td>
                                                                    <td>
                                                                        <textarea id="Cremarks" maxlength="100" class="form-control" name="remarks" value="" style="height:26px;" placeholder="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                    <td style="display: none;"><input type="hidden" id="hfSNo" value=${RowNo} /></td>
                                                               </tr>`);
        BindFGRProductItemInPut(RowNo);
    }
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfSNo").val();
    var hd_Status = $("#hdn_Status").val();
    var hdnshopfloorid = $("#ddlShopfloorID").val();
    var ProductNm = "";
    var ProductId = "";
    var Sub_Quantity = "";
    var UOM = "";
    var UOMID = "";
    var NewArr = [];

    var source_type = $("#dllSource_type").val();



    if (flag == "OutPut" || flag == "QCAccQty" || flag == "QCRejQty" || flag == "QCRewQty") {
        if (source_type == "D") {
            ProductNm = clickdRow.find("#ItemName_" + hfsno + " option:selected").text();
            ProductId = clickdRow.find("#ItemName_" + hfsno).val();
            UOM = clickdRow.find("#UOM").val();
            UOMID = clickdRow.find("#UOMID").val();

            Sub_Quantity = clickdRow.find("#OutputQuantity").val();


        }
        else {

            ProductNm = clickdRow.find("#ItemName_" + hfsno).val();
            ProductId = clickdRow.find("#hf_ItemID").val();
            UOM = clickdRow.find("#UOM").val();
            UOMID = clickdRow.find("#UOMID").val();
            Sub_Quantity = clickdRow.find("#OutputQuantity").val();
        }


        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
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
    }
    if (flag == "Input") {
        if (source_type == "D") {
            ProductNm = clickdRow.find("#Consum_ItemName_" + hfsno + " option:selected").text();
            ProductId = clickdRow.find("#Consum_ItemName_" + hfsno).val();
            UOM = clickdRow.find("#CUOM").val();
            UOMID = clickdRow.find("#hdn_cuom").val();
            Sub_Quantity = clickdRow.find("#CConsumedQuantity").val();
        }
        else {

            ProductNm = clickdRow.find("#Consum_ItemName_" + hfsno).val();
            ProductId = clickdRow.find("#hf_ItemID").val();


            UOM = clickdRow.find("#CUOM").val();
            UOMID = clickdRow.find("#hdn_cuom").val();
            Sub_Quantity = clickdRow.find("#CConsumedQuantity").val();
        }
        $("#hdn_ConsumeSub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
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
    }
    var rcpt_no = $("#ReceiptNumber").val();
    var rcpt_dt = $("#ddlreceiptdate").val();
    var IsDisabled = $("#DisableSubItem").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/FinishedGoodsReceipt/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            Uom_id: UOMID,
            Shfl_id: hdnshopfloorid,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            rcpt_no: rcpt_no,
            rcpt_dt: rcpt_dt
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
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hf_ItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function ItemStockBatchWise(el, evt) {

    try {
        debugger;
        var ItemName = "", ItemId = "", UOMName = "", ConsumedQuantity = "", hfsno = "";
        var clickedrow = $(evt.target).closest("tr");
        var Status = $("#hdn_Status").val();
        var source_type = $("#dllSource_type").val();
        if (source_type == "D") {
            debugger;
            ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
            hfsno = clickedrow.find("#hfSNo").val();
            ItemName = clickedrow.find("#Consum_ItemName_" + hfsno + " option:selected").text().trim();
            UOMName = clickedrow.find("#CUOM").val();
            ItemId = clickedrow.find("#Consum_ItemName_" + hfsno).val();
        }
        else {
            hfsno = clickedrow.find("#hfSNo").val();
            ItemName = clickedrow.find("#Consum_ItemName_" + hfsno).val();
            ItemId = clickedrow.find("#hf_ItemID").val();
            ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();

            UOMName = clickedrow.find("#CUOM").val();
        }
        var UOMId = clickedrow.find("#hdn_cuom").val();
        if (AvoidDot(ConsumedQuantity) == false) {
            ConsumedQuantity = "";
        }
        if ($("#ddlShopfloorID").val() == "0" || $("#ddlShopfloorID").val() == "") {
            $("#BatchNumber").css("display", "block");
            $("#vmShopfloor").text($("#valueReq").text());
            $("#vmShopfloor").css("display", "block");
            $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "Red");
            return false;
        }

        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "" || ConsumedQuantity == "") {
            if (source_type == "D") {
                $("#BatchNumber").css("display", "block");
                clickedrow.find("#consumedqty_Error").text($("#FillQuantity").text());
                clickedrow.find("#consumedqty_Error").css("display", "block");
                clickedrow.find("#CConsumedQuantity").css("border-color", "red");
                return false;
            }
            else {
                $("#ItemNameBatchWise").val(ItemName);
                $("#UOMBatchWise").val(UOMName);
                $("#QuantityBatchWise").val("0.000");
                $("#HDItemNameBatchWise").val(ItemId);
                $("#HDUOMBatchWise").val(UOMId);
                $("#BatchwiseTotalIssuedQuantity").text("0.000");
                $("#SaveItemBatchTbl TBODY TR").remove();
                $("#BatchWiseItemStockTbl TBODY TR").remove();
                $("#BatchNumber").css("display", "block");
                return false;
            }
        }
        else {

            var Transtype = $("#hdn_TransType").val();
            var cmd = $("#batch_Command").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemBatchDetail();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                var Shfl_Id = $("#ddlShopfloorID").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/FinishedGoodsReceipt/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            ShflId: Shfl_Id,
                            DocStatus: Status,
                            SelectedItemdetail: SelectedItemdetail,
                            Transtype: Transtype,
                            cmd: cmd,
                            uom_id: UOMId
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(ConsumedQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");

                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", ConsumedQuantity, "AvailableQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }
            else {
                var FGR_NO = $("#ReceiptNumber").val();
                var FGR_dt = $("#ddlreceiptdate").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/FinishedGoodsReceipt/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            FGR_NO: FGR_NO,
                            FGR_dt: FGR_dt,
                            DocStatus: Status,
                            ItemID: ItemId,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(ConsumedQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");

                        },
                    });
            }
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function CheckFormValidation() {
    debugger;
    var header = HeaderValidation();
    if (header == false) {
        return false;
    }
    var Outputitemvalid = CheckItemOutPutValidation();

    if (Outputitemvalid == false) {
        return false;
    }

    var inputitemvalidation = CheckItemInputPutValidation();
    if (inputitemvalidation == false) {
        return false;
    }
    debugger;
    var OutPutsubitemvalid = CheckOutPutValidations_forSubItems()
    if (OutPutsubitemvalid == false) {
        return false;
    }
    var subitemvalid = CheckInputPutValidations_forSubItems()
    if (subitemvalid == false) {
        return false;
    }

    var batchvalid = CheckItemBatch_Validation();

    if (batchvalid == false) {
        return false;
    }

    var serialValid = CheckItemSerial_Validation();
    if (serialValid == false) {
        return false;
    }
    if (Outputitemvalid == true && inputitemvalidation == true && batchvalid == true && serialValid == true && OutPutsubitemvalid == true) {
        var FRGInputItemDetailList = new Array();
        var FRGOutputItemDetailList = new Array();

        $("#ConsumptionItmDetailsTbl TBODY TR").each(function () {
            var row = $(this);
            var InputItemList = {};
            InputItemList.ItemId = row.find("#hf_ItemID").val();
            InputItemList.UOMId = row.find('#hdn_cuom').val();
            InputItemList.ConsumedQuantity = row.find('#CConsumedQuantity').val();
            InputItemList.remarks = row.find('#Cremarks').val();
            InputItemList.uom_alias = row.find('#CUOM').val();
            InputItemList.avl_stock_shfl = row.find('#ShopFloorstock').val();
            InputItemList.hdi_cbatch = row.find('#hdi_cbatch').val();
            InputItemList.hdi_cserial = row.find('#hdi_cserial').val();
            InputItemList.item_type = row.find('#CItem_type').val();
            FRGInputItemDetailList.push(InputItemList);
            debugger;
        });
        $("#tblIDFinishGoodReceipt TBODY TR").each(function () {
            var row = $(this);
            var OutputItemList = {};
            debugger;

            var sno = row.find("#hfSNo").val();
            OutputItemList.Item_id = row.find("#hf_ItemID").val();
            OutputItemList.item_name = row.find("#ItemName_" + sno + " option:selected").text();
            OutputItemList.UOMId = row.find('#UOMID').val();
            OutputItemList.OutputQuantity = row.find('#OutputQuantity').val();
            OutputItemList.CostPrice = row.find('#CostPrice').val();
            OutputItemList.lot = "";
            //OutputItemList.lot = row.find('#LotNumber').val();
            OutputItemList.BatchNo = row.find('#BatchNumber').val();
            OutputItemList.ExDate = row.find('#ExpiryDate').val();
            OutputItemList.ItemInfoBtnClick = row.find('#OP_ItmInfoBtnIcon').val();
            OutputItemList.sub_item = row.find('#sub_item').val();
            OutputItemList.i_exp = row.find('#hdnExperDate').val();
            OutputItemList.uom_alias = row.find('#UOM').val();
            OutputItemList.remarks = row.find('#Cremarks').val();
            OutputItemList.item_type = row.find('#Item_type').val();
            OutputItemList.QC_req = row.find('#hdnQC').val();
            FRGOutputItemDetailList.push(OutputItemList);
            debugger;
        });

        var InputItmStr = JSON.stringify(FRGInputItemDetailList);
        var OutputItmStr = JSON.stringify(FRGOutputItemDetailList);
        $('#hd_InputFinishGoodReciptItemDetail').val(InputItmStr);
        $('#hd_OutputItemDetail').val(OutputItmStr);

        Comn_BindItemBatchDetail();
        Comn_BindItemSerialDetail();
        /*-----------Sub-item-------------*/
        debugger;
        var SubItemsListArr = Cmn_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#OutPutSubItemDetailsDt').val(str2);

        var InputSubItemsListArr = BindInputSubitem();
        var InputSubitemData = JSON.stringify(InputSubItemsListArr);
        $('#InputSubItemDetailsDt').val(InputSubitemData);
        /*-----------Sub-item end-------------*/
        return true;
    }
    else {
        return false;
    }
}
function BindInputSubitem() {
    var NewArr = new Array();
    $("#hdn_ConsumeSub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {
            debugger;
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        }
    });
    return NewArr;
}
function CheckInputPutValidations_forSubItems() {
    debugger;
    var ErrFlg = "";
    $("#hdnPopupFlag").val("Input");
    if (Cmn_CheckValidations_forSubItems("ConsumptionItmDetailsTbl", "", "hf_ItemID", "CConsumedQuantity", 'SubItemConsumeQuantity', "Y") == false) { /*"SubItemConsumeQuantity"*/
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }


}
function HeaderValidation() {
    var ErrorFlag = "N";
    var dllSource_type = $("#dllSource_type").val();
    var shop_id = $("#ddlShopfloorID").val();
    var Oper_id = $("#ddloperationname").val();
    var SuppName = $("#SupervisorName").val();

    if (dllSource_type == "" || dllSource_type == null) {
        $("#dllSource_type").css("border-color", "Red");
        $('#vmSource_type').text($("#valueReq").text());
        $("#vmSource_type").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmSource_type").css("display", "none");
        $("#dllSource_type").css("border-color", "#ced4da");
    }

    if (dllSource_type == "B") {
        var item_id = $("#ddlItemName option:selected").val();
        if (item_id == "0" || item_id == "" || item_id == null) {
            $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "Red");
            $('#vmProductName').text($("#valueReq").text());
            $("#vmProductName").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#vmProductName").css("display", "none");
            $("#ddlItemName").css("border-color", "#ced4da");
        }
    }


    if (shop_id == "0" || shop_id == "" || shop_id == null) {
        $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "Red");
        $('#vmShopfloor').text($("#valueReq").text());
        $("#vmShopfloor").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmShopfloor").css("display", "none");
        $("#ddlShopfloorID").css("border-color", "#ced4da");
    }
    if (Oper_id == "0" || Oper_id == "" || Oper_id == null) {
        $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "Red");
        $('#vmOperation').text($("#valueReq").text());
        $("#vmOperation").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmOperation").css("display", "none");
        $("#ddloperationname").css("border-color", "#ced4da");
    }
    if (SuppName == "" || SuppName == null) {
        $("#SupervisorName").css("border-color", "Red");
        $('#vmSuppervisorName').text($("#valueReq").text());
        $("#vmSuppervisorName").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#vmSuppervisorName").css("display", "none");
        $("#SupervisorName").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatch_Validation() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var ItemId = clickedrow.find("#hf_ItemID").val();
        var UOMId = clickedrow.find("#hdn_cuom").val();
        var Batchable = clickedrow.find("#hdi_cbatch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                if (parseFloat(ConsumedQuantity) == parseFloat("0")) {

                }
                else {
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "Y");
                }
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#Batch_Qty').val();
                    var bchitemid = currentRow.find('#Batch_ItemId').val();
                    var bchuomid = currentRow.find('#Batch_UOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "N");
                }
                else {
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithconsumedqty").text(), "warning");
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
function CheckItemSerial_Validation() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var ItemId = clickedrow.find("#hf_ItemID").val();
        var UOMId = clickedrow.find("#hdn_cuom").val();
        var Serialable = clickedrow.find("#hdi_cserial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btncserialdeatil").css("border-color", "red");
                ValidateEyeColor(clickedrow, "btncserialdeatil", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#Serial_Qty').val();
                    var srialitemid = currentRow.find('#Serial_ItemId').val();
                    var srialuomid = currentRow.find('#Serial_UOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {

                    ValidateEyeColor(clickedrow, "btncserialdeatil", "N");
                }
                else {

                    ValidateEyeColor(clickedrow, "btncserialdeatil", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serializedqtydoesnotmatchwithconsumedqty").text(), "warning");
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
function CheckOutPutValidations_forSubItems() {
    debugger;
    var ErrFlg = "";
    if (Cmn_CheckValidations_forSubItems("tblIDFinishGoodReceipt", "", "hf_ItemID", "OutputQuantity", "SubItemButton", "Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
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

                clickedrow.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
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
function ResetWorningBorderColor() {
    var Tbllen = $("#ConsumptionItmDetailsTbl >tbody>tr").length;

    if (Tbllen > 0) {
        return Cmn_CheckValidations_forSubItems("ConsumptionItmDetailsTbl", "", "hf_ItemID", "CConsumedQuantity", "SubItemConsumeQuantity", "N");

    }
    return Cmn_CheckValidations_forSubItems("tblIDFinishGoodReceipt", "", "hf_ItemID", "OutputQuantity", "SubItemButton", "N");


}
function CheckItemOutPutValidation() {
    var ErrorFlag = 'N';
    if ($("#tblIDFinishGoodReceipt >tbody >tr").length == 0) {
        swal("", $("#OutputItemNotSelected").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    else {
        $("#tblIDFinishGoodReceipt >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            var OutputQuantity = currentRow.find("#OutputQuantity").val();
            var CostPrice = currentRow.find("#CostPrice").val();
            var hdnExperDate = currentRow.find("#hdnExperDate").val();
            var ExpiryDate = currentRow.find("#ExpiryDate").val();
            var BatchNumber = currentRow.find("#BatchNumber").val();
            if (currentRow.find("#ItemName_" + Sno).val() == "" || currentRow.find("#ItemName_" + Sno).val() == "0") {
                swal("", $("#OutputItemNotSelected").text(), "warning");
                ErrorFlag = "Y";
                return false;
            }
            if (OutputQuantity == "0" || OutputQuantity == "" || OutputQuantity == null) {
                currentRow.find("#OutputQuantity").css("border-color", "Red");
                currentRow.find('#OutputQuantityError').text($("#valueReq").text());
                currentRow.find("#OutputQuantityError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#OutputQuantityError").css("display", "none");
                currentRow.find("#OutputQuantity").css("border-color", "#ced4da");
            }
            if (CostPrice == "0" || CostPrice == "" || CostPrice == null) {
                currentRow.find("#CostPrice").css("border-color", "Red");
                currentRow.find('#CostPriceError').text($("#valueReq").text());
                currentRow.find("#CostPriceError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#CostPriceError").css("display", "none");
                currentRow.find("#CostPrice").css("border-color", "#ced4da");
            }
            if (BatchNumber == "0" || BatchNumber == "" || BatchNumber == null) {
                currentRow.find("#BatchNumber").css("border-color", "Red");
                currentRow.find('#BatchNumberError').text($("#valueReq").text());
                currentRow.find("#BatchNumberError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#BatchNumberError").css("display", "none");
                currentRow.find("#BatchNumber").css("border-color", "#ced4da");
            }
            if (hdnExperDate == "Y") {
                if (ExpiryDate == "0" || ExpiryDate == "" || ExpiryDate == null) {
                    currentRow.find("#ExpiryDate").css("border-color", "Red");
                    currentRow.find('#ExpiryDateError').text($("#valueReq").text());
                    currentRow.find("#ExpiryDateError").css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#ExpiryDateError").css("display", "none");
                    currentRow.find("#ExpiryDate").css("border-color", "#ced4da");
                }
            }
            else {
                currentRow.find("#ExpiryDateError").css("display", "none");
                currentRow.find("#ExpiryDate").css("border-color", "#ced4da");
            }
        })
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemInputPutValidation() {
    var ErrorFlag = "N";
    if ($("#ConsumptionItmDetailsTbl >tbody >tr").length == 0) {
        ErrorFlag = "Y";
        swal("", $("#InputItemNotSelected").text(), "warning");
        return false;
    } else {
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            var CConsumedQuantity = currentRow.find("#CConsumedQuantity").val();
            var errMessage = $("#ExceedingQty").text();
            var avlstck = currentRow.find("#ShopFloorstock").val();
            var source_type = $("#dllSource_type").val();
            if (currentRow.find("#Consum_ItemName_" + Sno).val() == "" || currentRow.find("#Consum_ItemName_" + Sno).val() == "0") {

                ErrorFlag = "Y";
                swal("", $("#InputItemNotSelected").text(), "warning");
                //return false;
            }
            if (ErrorFlag == "N") {
                if (source_type == "D") {


                    if (CConsumedQuantity == "0" || CConsumedQuantity == "" || CConsumedQuantity == null) {



                        currentRow.find("#CConsumedQuantity").css("border-color", "Red");
                        currentRow.find('#consumedqty_Error').text($("#valueReq").text());
                        currentRow.find("#consumedqty_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        if (parseFloat(CConsumedQuantity) > parseFloat(avlstck)) {
                            currentRow.find("#CConsumedQuantity").css("border-color", "Red");
                            currentRow.find('#consumedqty_Error').text(errMessage);
                            currentRow.find("#consumedqty_Error").css("display", "block");
                            ErrorFlag = "Y";
                        }
                        else {
                            currentRow.find("#consumedqty_Error").css("display", "none");
                            currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
                        }
                    }
                }
                else {
                    if (parseFloat(CConsumedQuantity) > parseFloat(avlstck)) {
                        currentRow.find("#CConsumedQuantity").css("border-color", "Red");
                        currentRow.find('#consumedqty_Error').text(errMessage);
                        currentRow.find("#consumedqty_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#consumedqty_Error").css("display", "none");
                        currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
                    }
                }
            }

        })
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnchageSource_type() {
    debugger;
    var source_type = $("#dllSource_type").val().trim();
    $("#hdnSource_typeID").val(source_type);
    var status_id = $("#hdn_Status").val();
    if (source_type == "D") {
        $("#ProductNameDiv").css("display", "none");

        $("#DivUomName").css("display", "none");
       var batch_Command = $("#batch_Command").val();
        if (status_id == "" && batch_Command != "Refresh") {
            $("#ddloperationname").attr("disabled", false);
        }
        if (status_id == "") {
            if ($("#tblIDFinishGoodReceipt tbody tr").length > 0) {
                $("#tblIDFinishGoodReceipt tbody tr").remove();
            }
            if ($("#ConsumptionItmDetailsTbl tbody tr").length > 0) {
                $("#ConsumptionItmDetailsTbl tbody tr").remove();
            }
            if ($("#hdn_Sub_ItemDetailTbl tbody tr").length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr").remove();
            }
            if ($("#SaveItemBatchTbl tbody tr").length > 0) {
                $("#SaveItemBatchTbl TBODY TR").remove();
            }
            if ($("#SaveItemSerialTbl tbody tr").length > 0) {
                $("#SaveItemSerialTbl TBODY TR").remove();
            }
        }
    }
    else {
        $("#ProductNameDiv").css("display", "block");
        $("#DivUomName").css("display", "block");

        BindProductNameDDL();
    }
    if (status_id == "") {
        $("#ddlItemName").val("0").trigger("change");
        $("#ddloperationname").val("0").trigger("change");
        $("#ddlShopfloorID").val("0").trigger("change");
        $("#ddlUOM").val("").trigger("change");
    }
    $("#vmProductName").css("display", "none");
    // $("#ddlItemName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");

    $("#vmOperation").css("display", "none");
    $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "#ced4da");

    $("#vmShopfloor").css("display", "none");
    $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "#ced4da");
}
function Onchngeshopflore() {
    var shop_id = $("#ddlShopfloorID").val();
    if (shop_id == "0" || shop_id == "" || shop_id == null) {
        $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "Red");
        $('#vmShopfloor').text($("#valueReq").text());
        $("#vmShopfloor").css("display", "block");
        EnableAndDisableInputItemDetail("Disable");
        EnableAndDisableOutPutItemDetail("Disable");
    }
    else {

        var SuppName = $("#SupervisorName").val();
        $("#Hdnshopfloor_id").val(shop_id);
        var Oper_id = $("#ddloperationname").val();
        if ((Oper_id == "0" || Oper_id == "") || (SuppName == "0" || SuppName == "")) {
            EnableAndDisableInputItemDetail("Disable");
            EnableAndDisableOutPutItemDetail("Disable");
        }
        else {
            EnableAndDisableInputItemDetail("Enable");
            EnableAndDisableOutPutItemDetail("Enable")
        }
        var dllSource_type = $("#dllSource_type").val();
        if (dllSource_type == "B") {
            var ErrorFlag = "N";
            var itemID = $("#ddlItemName option:selected").val();
            if (Oper_id == "0" || Oper_id == "") {
                $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "Red");
                $('#vmOperation').text($("#valueReq").text());
                $("#vmOperation").css("display", "block");
                $("#ddlShopfloorID").val("0").trigger("change");
                $("#vmShopfloor").css("display", "none");
                $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "#ced4da");
                ErrorFlag = "Y";
            }
            else {
                $("#vmOperation").css("display", "none");
                $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "#ced4da");
            }
            if (itemID == "0" || itemID == "" || itemID == null) {
                $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "Red");
                $('#vmProductName').text($("#valueReq").text());
                $("#vmProductName").css("display", "block");
                $("#ddlShopfloorID").val("0").trigger("change");
                $("#vmShopfloor").css("display", "none");
                $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "#ced4da");
                ErrorFlag = "Y";
            }
            else {
                $("#vmProductName").css("display", "none");
                $("#ddlItemName").css("border-color", "#ced4da");
            }

            if (ErrorFlag == "N") {
                GetDataOutPut('Output');
            }
            else {
                return false;
            }
        }
        $("#vmShopfloor").css("display", "none");
        $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "#ced4da");
    }
}

function OnchngeOperation() {
    var Oper_id = $("#ddloperationname").val();
    if (Oper_id == "0" || Oper_id == "" || Oper_id == null) {
        $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "Red");
        $('#vmOperation').text($("#valueReq").text());
        $("#vmOperation").css("display", "block");
        EnableAndDisableInputItemDetail("Disable");
        EnableAndDisableOutPutItemDetail("Disable");
    }
    else {
        $("#vmOperation").css("display", "none");
        $("[aria-labelledby='select2-ddloperationname-container']").css("border-color", "#ced4da");
        $("#Hdnoperation_id").val(Oper_id);
        var SuppName = $("#SupervisorName").val();
        var shop_id = $("#ddlShopfloorID").val();
        if ((shop_id == "0" || shop_id == "") || (SuppName == "0" || SuppName == "")) {
            EnableAndDisableInputItemDetail("Disable");
            EnableAndDisableOutPutItemDetail("Disable");
        }
        else {
            EnableAndDisableInputItemDetail("Enable");
            EnableAndDisableOutPutItemDetail("Enable")
        }

    }
}
function onchngesuppervisorName() {
    debugger;
    var SuppName = $("#SupervisorName").val();

    if (SuppName == "" || SuppName == null) {
        $("#SupervisorName").css("border-color", "Red");
        $('#vmSuppervisorName').text($("#valueReq").text());
        $("#vmSuppervisorName").css("display", "block");

        EnableAndDisableInputItemDetail("Disable");
        EnableAndDisableOutPutItemDetail("Disable");

    }
    else {
        $("#HdSuppervisorName").val(SuppName);
        $("#vmSuppervisorName").css("display", "none");
        $("#SupervisorName").css("border-color", "#ced4da");
        var Oper_id = $("#ddloperationname").val();
        var shop_id = $("#ddlShopfloorID").val();
        if ((shop_id == "0" || shop_id == "") || (Oper_id == "0" || Oper_id == "")) {
            EnableAndDisableInputItemDetail("Disable");
            EnableAndDisableOutPutItemDetail("Disable");
        }
        else {
            EnableAndDisableInputItemDetail("Enable");
            EnableAndDisableOutPutItemDetail("Enable");



        }
    }
}
function GetDataOutPut(flag) {
    debugger;
    var itemID = $("#ddlItemName option:selected").val();
    var operation_ID = $("#Hdnoperation_id").val();
    var shop_floor = $("#Hdnshopfloor_id").val();
    $.ajax({

        type: "post",
        url: "/ApplicationLayer/FinishedGoodsReceipt/GetBomitemData",
        data: {
            itemID: itemID,
            operation_ID: operation_ID,
            shop_floor: shop_floor,
            flag: flag

        },
        success: function (data) {
            debugger;
            if (data != "" && data != null && data != "[]") {
                data = JSON.parse(data).Table;
                if (flag == "Output") {
                    $("#tblIDFinishGoodReceipt tbody tr").remove();

                }
                else {
                    $("#ConsumptionItmDetailsTbl tbody tr").remove();
                }
                AddNewItemBomData(flag, data);

            }

        }


    })
}
function AddNewItemBomData(flag, data) {
    var QtyDecDigit = $("#QtyDigit").text();
    if (flag == "Output") {

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {

                var rowIdx = 0;
                debugger;
                var rowCount = $('#tblIDFinishGoodReceipt >tbody >tr').length + 1;
                var RowNo = 0;

                $("#tblIDFinishGoodReceipt >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRow = $(this);
                    RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
                });
                if (RowNo == "0") {
                    RowNo = 1;
                }
                var OutPutQty_out = "";
                if (data[i].Item_type != "OF") {
                    OutPutQty_out = (parseFloat(data[i].qty)).toFixed(QtyDecDigit);
                }
                var SubItemDisabled = "";
                var checked = "";
                var checkedDisabled = "";
                if (data[i].sub_item == "N") {
                    SubItemDisabled = "Disabled";
                }
                if (data[i].i_qc == "N") {
                    checked = "unchecked";
                    checkedDisabled = "Disabled";
                }
                else {
                    checked = "checked";
                    checkedDisabled = "";
                }
                $('#tblIDFinishGoodReceipt tbody').append(`<tr id="R${++rowIdx}">
                                                                   <td class="sr_padding" id="Sno">${rowCount}</td>
                                                                    <td >  </td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz ItemNametbl ">
                                                                        <div class="col-sm-11 lpo_form"  style="padding:0px;">
                                                                       <input id="ItemName_${RowNo}" class="form-control" autocomplete="off"
                                                                       value='${data[i].item_name}'
                                                                       type="text" name="ProductName" disabled=""
                                                                           onchange="OnchageBindFGR_ItemList(event)"
                                                                           placeholder="${$("#ItemUOM").text()}">
                                                                           
                                                                            <input type="hidden" id="hf_ItemID" value=${data[i].item_id} />
                                                                            <input type="hidden" id="Item_type" value=${data[i].Item_type} />
                                                                            <span id="ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon" style="padding:0px;">
                                                                            <button type="button" id="OP_ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information">  </button>
                                                                        </div>

                                                                    </td>
                                                                    <td>
                                                                        <input id="UOM" class="form-control" autocomplete="off" value=${data[i].uom_alias} type="text" name="UOM" disabled="" placeholder="${$("#ItemUOM").text()}">
                                                                        <input type="hidden" id="UOMID" style="display: none;" value=${data[i].uom_id}>
                                                                        <input type="hidden" id="UOMName_in_Spec" style="display: none;" value="${data[i].UOM_in_Sp}">
                                                                            <input type="hidden" id="Conv_rate"  style="display: none;" value=${data[i].conv_rate}>
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form" style="padding:0px;">
                                                                            <div class="lpo_form">

                                                                                <input id="OutputQuantity" class="form-control num_right" value="${OutPutQty_out}" autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="OnChangeOutPutQty(event);" placeholder="0000.00" type="text" name="OutputQuantity">
                                                                                <span id="OutputQuantityError" class="error-message is-visible"></span>
                                                                               <input type="hidden" id="Span_outQty" style="display: none;" value="${OutPutQty_out}">
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-2 i_Icon" id="Div_SubItemButton1">
                                                                             <input type="hidden" id="sub_item" value="${data[i].sub_item}">
                                                                            <button type="button" class="calculator subItmImg" id="SubItemButton" onclick="return SubItemDetailsPopUp('OutPut',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false" ${SubItemDisabled}><img src="/Content/Images/subItem.png" alt="" title="Sub-Item Detail" > </button>
                                                                            <input type="hidden" id="hdn_Subitmflag" value="OutPut" style="display: none;" />
                                                                         </div>


                                                                    </td>
                                                                          <td>
                                                                            <input id="OutputQuantityinSpec" class="form-control num_right" disabled="disabled" value="" autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="" placeholder="0000.00" type="text" name="OutputQuantityinSpec">
                                                                        </td>
                                                                    <td class="lpo_form"><input id="CostPrice" onkeypress="return Allow_FloatQtyonly(this,event)" class="form-control num_right" autocomplete="off" type="text" value="" name="CostPrice" placeholder="000.00" onchange="onchangeCostPrice(event)">
                                                                      <span id="CostPriceError" class="error-message is-visible"></span></td>
                                                                      <td>
                                                                        <div class="lpo_form">
                                                                            <input id="BatchNumber" maxlength="25" class="form-control"  onchange="OnchangeBatchNumber(event)" autocomplete="off" type="text" value="" name="BatchNumber" placeholder="${$("#BatchSpanDetail").text()}">
                                                                            <span id="BatchNumberError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>
                                                                
                                                                    <td class="lpo_form">
                                                                        <input id="ExpiryDate" class="form-control" autocomplete="off" type="date" value="" name="ExpiryDate" onchange="OnchngeExpiryDate(event)">
                                                                        <input type="hidden" id="hdnExperDate" value="${data[i].i_exp}">
                                                                     <span id="ExpiryDateError" class="error-message is-visible"></span>

                                                                    </td>
                                                                        <td>

                                                                       <div class="custom-control custom-switch sample_issue">
                                                                       <input type="checkbox" class="custom-control-input margin-switch" id="QC_${RowNo}" ${checked} ${checkedDisabled} onchange="OnchangeQC(event)">
                                                                         <label class="custom-control-label" for="QC_"></label>
                                                                       </div>
                                                                          <input type="hidden" id="hdnQC" value="${data[i].i_qc}">
                                                                       </td>
<td>
                                                                                <div class="col-sm-10 lpo_form no-padding">

                                                                                    <input id="AcceptQty" value="" class="form-control num_right" autocomplete="off" type="text" name="AcceptQty" placeholder="0000.00" disabled>



                                                                                </div>
                                                                                <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemAccQty">
                                                                                    <button type="button" id="SubItemAccQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCAccQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div class=" col-sm-8 lpo_form no-padding">


                                                                                    <input id="RejectQty" value="" class="form-control num_right" autocomplete="off" type="text" name="RejectQty" placeholder="0000.00" disabled>

                                                                                    <span id="RejectQty_Error" class="error-message is-visible"></span>
                                                                                </div>

                                                                                <div class=" col-sm-2 i_Icon" id="div_SubItemRejQty" style="padding:0px;">
                                                                                    <button type="button" id="SubItemRejQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('QCRejQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>

                                                                            </td>
                                                                            <td>
                                                                                <div class=" col-sm-8 lpo_form" style="padding:0px;">


                                                                                    <input id="ReworkableQty" value="" class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>

                                                                                </div>

                                                                                <div class=" col-sm-2 i_Icon" id="div_SubItemRewQty" style="padding:0px;">
                                                                                    <button type="button" id="SubItemRewQty" class="calculator subItmImg" disabled  onclick="return SubItemDetailsPopUp('QCRewQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                                </div>

                                                                            </td>
                                                                    <td>
                                                                        <textarea id="Cremarks" maxlength="100" class="form-control" name="remarks" value="" style="height:26px;" placeholder="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                    <td style="display: none;"><input type="hidden" id="hfSNo" value="${RowNo}" /></td>
</tr>`);

                if (data[i].i_exp == "Y") {
                    $("#ExpRequiredStar").css("display", "");
                } else {
                    $("#ExpRequiredStar").css("display", "none");
                }

               
                   
            }
            
            DisableHeaderField(data[0].item_id);
           
        }


    }
    else {
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                debugger;
                var rowIdx = 0;
                debugger;
                var rowCount = $('#ConsumptionItmDetailsTbl >tbody >tr').length + 1;
                var RowNo = 0;
                $("#ConsumptionItmDetailsTbl >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRow = $(this);
                    RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
                });
                var OutputQuantity = 0;
                var Consqty = 0;
                debugger;
                $("#tblIDFinishGoodReceipt >tbody >tr").each(function (i, row) {
                    debugger;
                    var outputCurr = $(this);
                    var item_type = outputCurr.find("#Item_type").val();
                    if (item_type == "OF") {
                        OutputQuantity = parseFloat(outputCurr.find("#OutputQuantity").val());
                    }
                });
                Consqty = (parseFloat(data[i].qty) * parseFloat(OutputQuantity)).toFixed(QtyDecDigit)

                var Disabled_batch = "";
                if (data[i].i_batch == "Y") {
                    Disabled_batch = "";
                }
                else {
                    Disabled_batch = "Disabled";
                }
                var Disabled_serial = "";
                if (data[i].i_serial == "N") {
                    Disabled_serial = "Disabled";
                }

                var SubItemDisabledInput = "";
                if (data[i].sub_item == "N") {
                    SubItemDisabledInput = "Disabled";
                }
                if (RowNo == "0") {
                    RowNo = 1;
                }

                $('#ConsumptionItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
          <td class="sr_padding" id="Sno">${rowCount}</td>
                                                                    <td </td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz">
                                                                        <div class="col-sm-11 lpo_form" style="padding:0px;">
                                                            <input id="Consum_ItemName_${RowNo}" class="form-control" autocomplete="off"
                                                                   value='${data[i].item_name}' type="text" name="ProductName" disabled="disabled"
                                                                   onchange="OnchangeBindFGRConsum_ItemList(event)" placeholder="${$("#ItemUOM").text()}">
                                                                           
                                                                            <input type="hidden" id="hf_ItemID" value="${data[i].item_id}"/>
                                                                            <input type="hidden" id="CItem_type" value="${data[i].Item_type}"/>
                                                                            <span id="con_ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon" style="padding:0px;">
                                                                            <button type="button" id="CItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information"> </button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="CUOM" class="form-control" autocomplete="off" value="${data[i].uom_alias}" type="text" name="UOM" disabled="disabled" placeholder="${$("#ItemUOM").text()}">
                                                                        <input type="hidden" id="hdn_cuom" value="${data[i].uom_id}" style="display: none;">
                                                                    </td>
                                                                    <td><input id="ShopFloorstock" class="form-control num_right" autocomplete="off" type="text" value='${data[i].avl_stk}' name="AvailableStockInShopfloor" disabled="disabled" placeholder="${$("#span_AvailableStock").text()}"></td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form" style="padding:0px;">
                                                                            <div class="lpo_form">
                                                                                <input type="hidden" id="hdnitem_reqqty" value="50" style="display: none;">
                                                                                <input id="CConsumedQuantity" class="form-control num_right" value=${Consqty} autocomplete="off" onkeypress="return AllowFloatQtyonly(this,event)" onchange="OnChangeConsumedQty(event);" placeholder="0000.00" type="text" name="ConsumedQuantity">
                                                                                <span id="consumedqty_Error" class="error-message is-visible"></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-2 i_Icon" id="div_SubItemConsumeQuantity" style="padding:0px; ">
                                                                            <input type="hidden" id="Consumesub_item" value="${data[i].sub_item}">
                                                                            <button type="button" id="SubItemConsumeQuantity" ${SubItemDisabledInput} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Input',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="Sub-Item Detail"> </button>
                                                                            <input type="hidden" id="hdn_Subitmflag" value="Input" style="display: none;" />
                                                                          </div>

                                                                  
                                                                    
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" id="btncbatchdeatil" onclick="ItemStockBatchWise(this,event)" ${Disabled_batch} class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Batch Detail" ></i></button>

                                                                        <input type="hidden" id="hdi_cbatch" value="${data[i].i_batch}" style="display: none;">
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" id="btncserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" ${Disabled_serial}><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Serial Detail"></i></button>

                                                                        <input type="hidden" id="hdi_cserial" value="${data[i].i_serial}" style="display: none;">
                                                                    </td>
                                                                    <td>
                                                                        <textarea id="Cremarks" maxlength="100" class="form-control" name="remarks" value="" style="height:26px;" placeholder="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                    <td style="display: none;"><input type="hidden" id="hfSNo" value=${RowNo} /></td>
                                                               </tr>`);
            }
        }


    }
}
function OnchangeQC(e) {
    debugger;
    var target = $(e.target).closest('tr');
    var Sno = target.find("#hfSNo").val();
    var qc_req = target.find("#QC_" + Sno);
    if (qc_req.is(':checked')) {
        target.find('#hdnQC').val('Y');
    } else {
        target.find('#hdnQC').val('N');
    }
}
function OnChangeOutPutQty(e) {
    var QtyDecDigit = $("#QtyDigit").text();
    var currentRow = $(e.target).closest('tr');
    // var Sno = currentRow.find("#hfSNo").val();
    var OutputQuantity = currentRow.find("#OutputQuantity").val();
    if (OutputQuantity == "0" || OutputQuantity == "" || OutputQuantity == null) {
        currentRow.find("#OutputQuantity").css("border-color", "Red");
        currentRow.find('#OutputQuantityError').text($("#valueReq").text());
        currentRow.find("#OutputQuantityError").css("display", "block");
        // ErrorFlag = "Y";
    }
    else {
        currentRow.find("#OutputQuantityError").css("display", "none");
        currentRow.find("#OutputQuantity").css("border-color", "#ced4da");
        var OutputQuantity1 = parseFloat(parseFloat(OutputQuantity)).toFixed(parseFloat(QtyDecDigit));
        currentRow.find("#OutputQuantity").val(OutputQuantity1);

        calculateOutputQty(currentRow);
        var item_type = currentRow.find("#Item_type").val();
        if (item_type == "OF") {
            GetDataOutPut("Input");
            calculateOutPutQty_itemType(currentRow);
        }

    }

}
function calculateOutPutQty_itemType(currentRow) {
    var QtyDecDigit = $("#QtyDigit").text();

    var OutputQuantity = currentRow.find("#OutputQuantity").val();
    $('#tblIDFinishGoodReceipt tbody tr').each(function () {

        var target = $(this);
        var Item_type = target.find("#Item_type").val();
        if (Item_type != "OF") {
            var outQty1 = target.find("#Span_outQty").val();
            var final_outPutQty = (parseFloat(OutputQuantity) * parseFloat(outQty1)).toFixed(QtyDecDigit);
            target.find("#OutputQuantity").val(final_outPutQty);
            calculateOutputQty(target);
        }


    })
}
function calculateOutputQty(currentRow) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var item_id = currentRow.find("#hf_ItemID").val();
    var OutputQuantity = currentRow.find("#OutputQuantity").val();
    var conv_rate = currentRow.find("#Conv_rate").val();
    var UOMName_in_Spec = currentRow.find("#UOMName_in_Spec").val().trim();
    var outputqtyinspec = parseFloat(OutputQuantity) * parseFloat(conv_rate);
    var qtyinspec = "";
    if (UOMName_in_Spec != "" && UOMName_in_Spec != null) {
        qtyinspec = parseFloat(outputqtyinspec).toFixed(parseFloat(QtyDecDigit)) + '(' + UOMName_in_Spec + ')';
    }
    else {
        qtyinspec = parseFloat(outputqtyinspec).toFixed(parseFloat(QtyDecDigit));
    }
    currentRow.find("#OutputQuantityinSpec").val(qtyinspec);
}
function onchangeCostPrice(e) {
    var RateDigit = $("#RateDigit").text();
    var currentRow = $(e.target).closest('tr');
    // var Sno = currentRow.find("#hfSNo").val();
    var CostPrice = currentRow.find("#CostPrice").val();
    if (CostPrice == "0" || CostPrice == "" || CostPrice == null) {
        currentRow.find("#CostPrice").css("border-color", "Red");
        currentRow.find('#CostPriceError').text($("#valueReq").text());
        currentRow.find("#CostPriceError").css("display", "block");
        //ErrorFlag = "Y";
    }
    else {
        currentRow.find("#CostPriceError").css("display", "none");
        currentRow.find("#CostPrice").css("border-color", "#ced4da");
        var CostPrice1 = parseFloat(parseFloat(CostPrice)).toFixed(parseFloat(RateDigit));
        currentRow.find("#CostPrice").val(CostPrice1);
    }
}
function OnchangeBatchNumber(e) {
    var RateDigit = $("#RateDigit").text();
    var currentRow = $(e.target).closest('tr');
    // var Sno = currentRow.find("#hfSNo").val();
    var BatchNumber = currentRow.find("#BatchNumber").val();
    if (BatchNumber == "0" || BatchNumber == "" || BatchNumber == null) {
        currentRow.find("#BatchNumber").css("border-color", "Red");
        currentRow.find('#BatchNumberError').text($("#valueReq").text());
        currentRow.find("#BatchNumberError").css("display", "block");
        //ErrorFlag = "Y";
    }
    else {
        currentRow.find("#BatchNumberError").css("display", "none");
        currentRow.find("#BatchNumber").css("border-color", "#ced4da");
        //var BatchNumber1 = parseFloat(parseFloat(BatchNumber)).toFixed(parseFloat(RateDigit));
        //currentRow.find("#BatchNumber").val(BatchNumber1);
    }
}
function OnchngeExpiryDate(e) {
    var currentRow = $(e.target).closest('tr');
    //var Sno = currentRow.find("#hfSNo").val();
    var hdnExperDate = currentRow.find("#hdnExperDate").val();
    var ExpiryDate = currentRow.find("#ExpiryDate").val();
    if (hdnExperDate == "Y") {
        if (ExpiryDate == "0" || ExpiryDate == "" || ExpiryDate == null) {
            currentRow.find("#ExpiryDate").css("border-color", "Red");
            currentRow.find('#ExpiryDateError').text($("#valueReq").text());
            currentRow.find("#ExpiryDateError").css("display", "block");
            // ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ExpiryDateError").css("display", "none");
            currentRow.find("#ExpiryDate").css("border-color", "#ced4da");
        }
    }
    else {
        currentRow.find("#ExpiryDateError").css("display", "none");
        currentRow.find("#ExpiryDate").css("border-color", "#ced4da");
    }
}
function OnChangeConsumedQty(e) {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    var currentRow = $(e.target).closest('tr');
    var errMessage = $("#ExceedingQty").text();
    var CConsumedQuantity = currentRow.find("#CConsumedQuantity").val();

    var source_type = $("#dllSource_type").val();



    if (CConsumedQuantity == "0" || CConsumedQuantity == "" || CConsumedQuantity == null) {
        if (source_type == "D") {
            currentRow.find("#CConsumedQuantity").css("border-color", "Red");
            currentRow.find('#consumedqty_Error').text($("#valueReq").text());
            currentRow.find("#consumedqty_Error").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#consumedqty_Error").css("display", "none");
            currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
            var CConsumedQuantity1 = parseFloat(parseFloat(CConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#CConsumedQuantity").val(CConsumedQuantity1);
        }
    }
    else {
        var CConsumedQuantity1 = parseFloat(parseFloat(CConsumedQuantity)).toFixed(parseFloat(QtyDecDigit));
        currentRow.find("#CConsumedQuantity").val(CConsumedQuantity1);

        var avlstck = currentRow.find("#ShopFloorstock").val();
        if (parseFloat(CConsumedQuantity) > parseFloat(avlstck)) {
            currentRow.find("#CConsumedQuantity").css("border-color", "Red");
            currentRow.find('#consumedqty_Error').text(errMessage);
            currentRow.find("#consumedqty_Error").css("display", "block");
        }
        else {
            currentRow.find("#consumedqty_Error").css("display", "none");
            currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
        }
    }
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    if (ItemMI_Qty == "") {
        ItemMI_Qty = "0";
    }
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "btncbatchdeatil", "Y");
            IsuueFlag = false;
        }
    });

    if (IsuueFlag == true) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                debugger;
                var rowitem = row.find("#Batch_ItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="Batch_LotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="Batch_ItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="Batch_UOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="Batch_BatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="Batch_Qty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="Batch_ExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="Batch_AvlBatchQty" value="${AvailableQty}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hf_ItemID").val();
            if (ItemId == SelectedItem) {
                ValidateEyeColor(clickedrow, "btncbatchdeatil", "N");
            }
        });
    }
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
        var QtyDecDigit = $("#QtyDigit").text();
        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var hfsno = clickedrow.find("#hfSNo").val();
        var ItemName = clickedrow.find("#Consum_ItemName_" + hfsno + " option:selected").text().trim();
        var UOMName = clickedrow.find("#CUOM").val();
        var ItemId = clickedrow.find("#Consum_ItemName_" + hfsno).val();
        var UOMID = clickedrow.find("#hdn_cuom").val();

        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "") {
            clickedrow.find("#consumedqty_Error").text($("#FillQuantity").text());
            clickedrow.find("#consumedqty_Error").css("display", "block");
            clickedrow.find("#CConsumedQuantity").css("border-color", "red");
        }
        else {

            var Status = $("#hdn_Status").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemSerialDetail();

                var SelectedItemSerial = $("#HDSelectedSerialwise").val();
                var Shfl_Id = $("#ddlShopfloorID").val();
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/FinishedGoodsReceipt/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            ShflId: Shfl_Id,
                            DocStatus: Status,
                            SelectedItemSerial: SelectedItemSerial,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);

                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(ConsumedQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                        },
                    });
            }
            else {
                var rcpt_no = $("#ReceiptNumber").val();
                var rcpt_dt = $("#ddlreceiptdate").val();
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/FinishedGoodsReceipt/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            rcpt_no: rcpt_no,
                            rcpt_dt: rcpt_dt,
                            DocStatus: Status,
                            ItemID: ItemId,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(ConsumedQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#Serial_ItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#Serial_Qty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }
        }
    } catch (err) {
        console.log("Production Confirmation Error : " + err.message);
    }
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#Serial_ItemId").val();
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
            <td><input type="text" id="Serial_ItemId" value="${ItemId}" /></td>
            <td><input type="text" id="Serial_UOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="Serial_LOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="Serial_Qty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="Serial_SerialNo" value="${ItemSerialNO}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var hfsno = clickedrow.find("#hfSNo").val();
            var ItemId = clickedrow.find("#Consum_ItemName_" + hfsno).val();
            if (ItemId == SelectedItem) {
                //clickedrow.find("#btncserialdeatil").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "btncserialdeatil", "N");
            }
        });
    }
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
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
function AvlStockShopfloor(itemid, currentrow, sno) {
    try {
        var QtyDecDigit = $("#QtyDigit").text();
        var shopflore = $("#ddlShopfloorID").val();
        if ($("#ddlShopfloorID").val() == "0" || $("#ddlShopfloorID").val() == "") {
            $("#BatchNumber").css("display", "block");
            $("#vmShopfloor").text($("#valueReq").text());
            $("#vmShopfloor").css("display", "block");
            $("[aria-labelledby='select2-ddlShopfloorID-container']").css("border-color", "Red");
            return false;
        }
        else {

            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/FinishedGoodsReceipt/GetAvlStock",
                    data: {
                        itemid: itemid,
                        shopflore: shopflore
                    },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            LSO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (currentrow != null) {
                                if (arr.Table.length > 0) {
                                    currentrow.find("#ShopFloorstock").val(parseFloat(arr.Table[0].avl_stk).toFixed(QtyDecDigit));
                                }
                            }
                        }
                    },
                });
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function EnableAndDisableInputItemDetail(flag) {
    if (flag == "Disable") {
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            var sourcetype = $("#dllSource_type").val();

            $(".plus_icon1").css("display", "none");


            currentRow.find("#CConsumedQuantity").attr("disabled", true);
            currentRow.find("#Cremarks").attr("disabled", true);
            currentRow.find("#Consum_ItemName_" + Sno).attr("disabled", true);
        })
    }
    else if (flag == "Enable") {
        var sourcetype = $("#dllSource_type").val();
        if (sourcetype == "D") {
            $(".plus_icon1").css("display", "");
        }
        else {
            $(".plus_icon1").css("display", "none");
        }
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            var sourcetype = $("#dllSource_type").val();
            if (sourcetype == "D") {

                currentRow.find("#Consum_ItemName_" + Sno).attr("disabled", false);
            }
            else {


                currentRow.find("#Consum_ItemName_" + Sno).attr("disabled", true);
            }

            currentRow.find("#CConsumedQuantity").attr("disabled", false);
            currentRow.find("#Cremarks").attr("disabled", false);

        })
    }

}
function EnableAndDisableOutPutItemDetail(flag) {
    if (flag == "Disable") {
        $(".plus_icon1").css("display", "none");
        $("#tblIDFinishGoodReceipt >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            var sourcetype = $("#dllSource_type").val();

            currentRow.find("#ItemName_" + Sno).attr("disabled", true);
            currentRow.find("#ItemName_" + Sno).attr("disabled", true);
            currentRow.find("#OutputQuantity").attr("disabled", true);
            currentRow.find("#Cremarks").attr("disabled", true);
            currentRow.find("#CostPrice").attr("disabled", true);
            currentRow.find("#BatchNumber").attr("disabled", true);
            currentRow.find("#ExpiryDate").attr("disabled", true);

        })
    }
    else if (flag == "Enable") {
        var sourcetype = $("#dllSource_type").val();
        if (sourcetype == "D") {
            $(".plus_icon1").css("display", "");
        }
        else {
            $(".plus_icon1").css("display", "none");
        }
        $("#tblIDFinishGoodReceipt >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();

            if (sourcetype == "D") {

                currentRow.find("#ItemName_" + Sno).attr("disabled", false);
            }
            else {

                currentRow.find("#ItemName_" + Sno).attr("disabled", true);
            }



            currentRow.find("#OutputQuantity").attr("disabled", false);
            currentRow.find("#CostPrice").attr("disabled", false);
            currentRow.find("#BatchNumber").attr("disabled", false);
            currentRow.find("#ExpiryDate").attr("disabled", false);
            currentRow.find("#Cremarks").attr("disabled", false);

        })
    }

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
    DocNo = $("#ReceiptNumber").val();
    DocDate = $("#ddlreceiptdate").val();
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
    //var pdfAlertEmailFilePath = 'FGR_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath);

    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks /*pdfAlertEmailFilePath*/);
            window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            await Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
// added by Nidhi on 25-06-2025
//async function GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, fileName) {
//    debugger;
//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/FinishedGoodsReceipt/SavePdfDocToSendOnEmailAlert",
//        data: { DocNo: DocNo, DocDate: DocDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ReceiptNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)

        ///  var Doc_Status = clickedrow.children("#Doc_Status").text();
        // Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
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
function onclickcancilflag() {

    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").prop('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").prop('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
};
function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var FGRDt = $("#ddlreceiptdate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: FGRDt
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
                /*   swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "tblIDFinishGoodReceipt", [{ "FieldId": "ItemName_", "FieldType": "input", "SrNo": "hfSNo"  }]);
}