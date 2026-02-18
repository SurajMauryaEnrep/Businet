$(document).ready(function () {
    debugger;
    $("#ddlHDWarehouse").select2();
   // BindProductNameDDL();
    BindAssemblyKitItemBindDeatil();
    BindAssemblyKitCompList(1);
    WareHouseSearchAble();
    var ass_no = $("#hdnPRNo").val();
    if (ass_no != null && ass_no != "" && ass_no != undefined) {
        $("#hdDoc_No").val(ass_no);
    }
  
    $('#AsembliKit_tbl').on('click', '.deleteIcon', function () {
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
        var Itemid = $(this).closest('tr').find("#hfItemID").val();

        Cmn_DeleteSubItemQtyDetail(Itemid);
        DeleteItemBatchSerialOrderQtyDetails(Itemid);


        updateItemSerialNumber()
    });

    $("#datatable-buttons tbody tr").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var Doc_no = clickedrow.children("#doc_no").text();
            var Doc_dt = clickedrow.children("#doc_dt").text();
            if (Doc_no != null && Doc_no != "" && Doc_dt != null && Doc_dt != "") {
                window.location.href = "/ApplicationLayer/AssemblyKit/DblClick/?Doc_no=" + Doc_no + "&Doc_dt=" + Doc_dt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons tbody").bind("click", function (e) {

        debugger;
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.children("#doc_no").text();
        var doc_dt = clickedrow.children("#doc_dt").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(doc_no, doc_dt, Doc_id, Doc_Status);
        $("#hdDoc_No").val(doc_no);
    });
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#AsembliKit_tbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hfItemID').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});
function OnchangeItemName() {
    var itemID = $("#ddlAssemblyProduct option:selected").val();
}
function BtnSearch() {
    debugger;
    try {    
        var Item_id = $("#ddlAssemblyProduct option:selected").val().trim();     
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AssemblyKit/List_Search",
            data: {            
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,              
                Item_id: Item_id
            },
            success: function (data) {
                debugger;
                $('#AssKitdiv').html(data);
                $('#ListFilterData').val(Fromdate + ',' + Todate + ',' + Status + ',' + Item_id);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
    ResetWF_Level();
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#AsembliKit_tbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var rowCount = $('#AsembliKit_tbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#AsembliKit_tbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#AsembliKit_tbl tbody').append(`<tr id="R${++rowIdx}">
        <td class=" red center">  <i class="deleteIcon fa fa-trash" aria-hidden="true" title=Delete></i></td>
        <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
        <td>
            <div class="col-sm-11 no-padding">
                <select class="form-control" id="ComponentName_${RowNo}" name="ComponentName" onchange="OnChangeComponentName(event)">
                </select>
                <input type="hidden" id="hfItemID" value="" />
                <span id="ItemNameError" class="error-message is-visible"></span>
            </div>
            <div class="col-sm-1 i_Icon">
                <button type="button" class="calculator subItmImg" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static"
                onclick="OnClickIconBtn(event);" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information">  </button>
            </div>
        </td>
        <td>
            <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
            <input id="UOMID" type="hidden" />
        </td>
         <td> <div class=" col-sm-10 no-padding">
                                                                                            <div class="lpo_form">
                                                              <select class="form-control" id="wh_id${RowNo}" onchange ="OnChangeWarehouse(this,event)"><option value="0">---Select---</option></select>
                                  <span id="wh_Error" class="error-message is-visible"></span>
                                  </div>
 </div></div>
                                  <div class=" col-sm-2 i_Icon">
                                  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                  </div>
                                <input id="WhID" type="hidden" />
                              </td>
         <td><input id="AvailableStock" class="form-control num_right" name="AvailableStock" placeholder="0000.00" readonly="readonly"></td>
          <td>
                                                                            <div class="col-sm-9 lpo_form no-padding">
                                                                                <input id="InputQty" value="" onkeypress="return AllowFloatQtyonly(this,event)" onchange="return onchangeInputQty(this,event)" class="form-control num_right" autocomplete="off" type="text" name="ord_qty_spec" placeholder="0000.00">
                                                                                <span id="InputQty_specError" class="error-message is-visible"></span>
                                                                            </div>
                                                                            <div class="col-sm-3 i_Icon" id="div_SubItemInputQty">
                                                                                <button type="button" id="SubItemInputQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('AssemblyKitINput',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="@Resource.SubItem @Resource.Detail"> </button>
                                                                            </div>
                                                                            <input hidden type="text" id="hdnsub_item" value="N" />
                                                                        </td>
       <td class="center">

           <button type="button" id="btncbatchdeatil" onclick="ItemStockBatchWise(this,event)" disabled="" class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Batch Detail"></i></button>


                <input type="hidden" id="hdn_batch" value="" style="display: none;">
            </td>
            <td class="center">


             <button type="button" id="btncserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="Serial Detail"></i></button>
             <input type="hidden" id="hdn_serial" value="" style="display: none;">
         </td>
         <td>
             <textarea id="ItemRemarks" class="form-control remarksmessage" onmouseover="OnMouseOver(this.value)" autocomplete="off" name="ItemRemarks" maxlength="200" placeholder="${$("#span_remarks").text()}"></textarea>

         </td>
         <td style="display: none;"><input type="hidden" id="SNohf" value=${RowNo} /></td>                                                       
            </tr>`);

    BindAssemblyKitCompList(RowNo);
    BindWarehouseList(RowNo);
    WareHouseSearchAble();
}
function AllowFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnChangeWarehouse(el, evt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();

        QtyDecDigit = $("#QtyDigit").text();

    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SNohf").val();
    var ddlId = "#wh_id" + Index;
    var whERRID = "#wh_Error";
    if (clickedrow.find(ddlId).val() == "0") {
        debugger;
        clickedrow.find(whERRID).text($("#valueReq").text());
        clickedrow.find(whERRID).css("display", "block");
        clickedrow.find(ddlId).css("border-color", "red");
        clickedrow.find('[aria-labelledby="select2-wh_id' + Index + '-container"]').css("border", "1px solid red");
    }
    else {
        var WHName = clickedrow.find("#wh_id" + Index + " option:selected").text();
        $("#hdwh_name").val(WHName)
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
        clickedrow.find('[aria-labelledby="select2-wh_id' + Index + '-container"]').css("border", "1px solid #ced4da");
    }

    var ItemId = clickedrow.find("#hfItemID").val();
    var WarehouseId = clickedrow.find(ddlId).val();
    clickedrow.find("#WhID").val(WarehouseId);

    //var CompId = $("#HdCompId").val();
    //var BranchId = $("#HdBranchId").val();
    debugger;
    if (WarehouseId != "0" && WarehouseId != null) {
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#PD_BatchItemId").val();
            if (rowitem == ItemId) {
                debugger;
                $(this).remove();
            }
        });


        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
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
}
function HideAndShow_BatchSerialButton(row) {
    // $("#ConsumptionItmDetailsTbl tbody tr").each(function () {
    debugger;
    // var row = $(e.target).closest('tr');
    var b_flag = row.find("#hdn_batch").val();
    var s_flag = row.find("#hdn_serial").val();
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

function WareHouseSearchAble() {
    $('#AsembliKit_tbl tbody tr').each(function () {
        var Currentrow = $(this);
        var sno = Currentrow.find("#SNohf").val();
        Currentrow.find("#wh_id" + sno).select2();
    })
}
function BindWarehouseList(id) {
    try {
        debugger;
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/AssemblyKit/GetWaherHouseList",
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
                        if (arr.length > 0 > 0) {
                            var PreWhVal = $("#wh_id" + id).val();
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.length; i++) {
                                s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
                            }
                            if (id == null) {
                                $('#AsembliKit_tbl tbody tr').each(function () {
                                    var row = $(this);
                                    let srNo = row.find("#SNohf").val();
                                    $("#wh_id" + srNo).html(s);
                                    $("#wh_id" + id).val(IsNull(PreWhVal, '0'));
                                });
                            } else {
                                $("#wh_id" + id).html(s);
                                $("#wh_id" + id).val(IsNull(PreWhVal, '0'));
                            }
                        }
                    }
                },
            });
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}

function OnChangeComponentName(evt) {
    debugger;
    var currentrow = $(evt.target).closest("tr");
    var sno = currentrow.find("#SNohf").val();
    var itemid = currentrow.find("#ComponentName_" + sno).val();
    currentrow.find("#hfItemID").val(itemid);
    NullInput_OutPutTableData(currentrow, itemid);
    Cmn_BindUOM(currentrow, itemid, sno, "N", "AssemblyKit");

}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var sno = clickedrow.find("#SNohf").val();;
        var ItemId = clickedrow.find("#hfItemID").val();;
        var ItemName = clickedrow.find("#ComponentName_" + sno +" option:selected").text();
        var UOMName = clickedrow.find("#UOM").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId, UomId: null, DocumentMenuId: DocumentMenuId
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
function NullInput_OutPutTableData(currentrow, itemid) {
        var sno = currentrow.find("#hfSNo").val();
    //var Sno_Text = currentrow.find("#SpanRowId").text();
    var Sno_Text = currentrow.find("#SNohf").val();
        currentrow.find("#InputQty").val("");
        currentrow.find("#ItemRemarks").val("");
        currentrow.find("#ItemRemarks").val("");
    var ddlId = "#wh_id" + Sno_Text;
    var whERRID = "#wh_Error";
    currentrow.find(ddlId).val("0").trigger("change");
    currentrow.find(whERRID).css("display", "none");
    currentrow.find(ddlId).css("border-color", "#ced4da");
    currentrow.find('[aria-labelledby="select2-wh_id' + Sno_Text + '-container"]').css("border", "1px solid #ced4da");

    currentrow.find("#AvailableStock").val("");
        Cmn_DeleteSubItemQtyDetail(itemid);
        DeleteItemBatchSerialOrderQtyDetails(itemid);

}
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
function BindAssemblyKitCompList(ID) {
    debugger;
    BindItemList("#ComponentName_", ID, "#AsembliKit_tbl", "#SNohf", "", "AssemblyKit");
}
//function BindProductNameDDL() {
//    debugger;
//    $("#ddlAssemblyProduct").append("<option value='0'>---Select---</option>");
//    $("#ddlAssemblyProduct").select2({

//        ajax: {
//            url: "/ApplicationLayer/AssemblyKit/GetItemList",
//            data: function (params) {
//                var queryParameters = {
//                    AssemblyProduct: params.term,
//                    page: params.page || 1
//                };
//                return queryParameters;
//            },
//            multiple: true,
//            cache: true,
//            processResults: function (data, params) {
//                debugger
//                var pageSize,
//                    pageSize = 2000; // or whatever pagesize
//                data = JSON.parse(data);

             

//                if ($(".select2-search__field").parent().find("ul").length == 0) {
//                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
//                      <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
//                       <div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
//                       </strong></li></ul>`)
//                }

//                var page = 1;
//                data = data.slice((page - 1) * pageSize, page * pageSize)
//                return {
//                    results: $.map(data, function (val, Item) {
//                        return { id: val.Item_id, text: val.Item_name, UOM: val.uom_name };
//                    }),
//                };
//            },
//            cache: true
//        },
//        templateResult: function (data) {
//            debugger

//            var classAttr = $(data.element).attr('class');
//            var hasClass = typeof classAttr != 'undefined';
//            classAttr = hasClass ? ' ' + classAttr : '';
//            var $result;

//            $result = $(
//                '<div class="row">' +
//                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
//                '</div>'
//            );

//            return $result;

//            firstEmptySelect = false;
//        },

//    });


//}

function BindAssemblyKitItemBindDeatil() {
    debugger;
    var Ass_proID = $("#hdnAssemblyProductID").val();
    if (Ass_proID != null && Ass_proID != "") {
        $("#ddlAssemblyProduct").val(Ass_proID).trigger('onchange');
            
    }
    else {
    
        $("#ddlAssemblyProduct")
            .empty()
            .trigger("change");
       // BindItemList("#ddlAssemblyProduct", null, null, null, null, "Ass")
            $("#ddlAssemblyProduct").append("<option value='0'>---Select---</option>");
        DynamicSerchableItemDDL("", "#ddlAssemblyProduct", "", "", "", "ItmAssemblyKitHeader")
        $("#SpanAssemblyProduct").css("display", "none");
        $("#ddlAssemblyProduct").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlAssemblyProduct-container']").css("border-color", "#ced4da");
    }

    //$("#ddlAssemblyProduct").select2({
    //    ajax: {
    //        url: "/ApplicationLayer/AssemblyKit/GetItemList_Deatil",
    //        data: function (params) {
    //            return {
    //                AssemblyProduct: params.term,
    //                page: params.page || 1
    //            };
    //        },
    //        cache: true,
    //        processResults: function (data) {

    //            data = JSON.parse(data);
    //            ConvertIntoArreyList("#AsembliKit_tbl", "#ComponentName_", "", "#SNohf");

    //            var selectedId = $("#ddlAssemblyProduct").val();

    //            if (selectedId && selectedId !== "0") {
    //                data = data.filter(j => j.Item_id != selectedId);
    //            }

    //            if ($(".select2-results__options .custom-header").length === 0) {
    //                $(".select2-results__options").prepend(`
    //                    <li class="select2-results__option custom-header">
    //                        <strong>
    //                            <div class="row">
    //                                <div class="col-md-9 col-xs-6">${$("#ItemName").text()}</div>
    //                                <div class="col-md-3 col-xs-6">UOM</div>
    //                            </div>
    //                        </strong>
    //                    </li>
    //                `);
    //            }

    //            return {
    //                results: $.map(data, function (val) {
    //                    return {
    //                        id: val.Item_id,
    //                        text: val.Item_name,
    //                        UOM: val.uom_name
    //                    };
    //                })
    //            };
    //        }
    //    },
    //    templateResult: function (data) {
    //        if (!data.id) return data.text;

    //        return $(`
    //            <div class="row">
    //                <div class="col-md-9 col-xs-6">${data.text}</div>
    //                <div class="col-md-3 col-xs-6">${data.UOM || ""}</div>
    //            </div>
    //        `);
    //    }
    //});
}

function ItemStockBatchWise(el, evt) {

    try {
        debugger;
        var ItemName = "", ItemId = "", UOMName = "", ConsumedQuantity = "", hfsno = "";
        var clickedrow = $(evt.target).closest("tr");
        var Status = $("#hdn_Status").val();
      //  var Index = clickedrow.find("#SpanRowId").text();
        var Index = clickedrow.find("#SpanRowId").text();
        ConsumedQuantity = clickedrow.find("#InputQty").val();
                 hfsno = clickedrow.find("#SNohf").val();
        ItemName = clickedrow.find("#ComponentName_" + hfsno + " option:selected").text().trim();
            UOMName = clickedrow.find("#UOM").val();
        ItemId = clickedrow.find("#ComponentName_" + hfsno).val();
       
       
        var UOMId = clickedrow.find("#UOMID").val();
        if (AvoidDot(ConsumedQuantity) == false) {
            ConsumedQuantity = "";
        }
        var Wh_ID = clickedrow.find("#WhID").val();
        if (Wh_ID == "0" || Wh_ID == "") {
            clickedrow.find("#BatchNumber").css("display", "block");
            clickedrow.find("#wh_Error").text($("#valueReq").text());
            clickedrow.find("#wh_Error").css("display", "block");

            clickedrow.find('[aria-labelledby="select2-wh_id' + hfsno + '-container"]').css("border-color", "Red");

            //$("[aria-labelledby='select2-wh_id${Index}-container']").css("border-color", "Red");
            return false;
        }

        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "" || ConsumedQuantity == "") {          
                $("#BatchNumber").css("display", "block");
                clickedrow.find("#InputQty_specError").text($("#FillQuantity").text());
                clickedrow.find("#InputQty_specError").css("display", "block");
                clickedrow.find("#InputQty").css("border-color", "red");
                return false;
        }
        else {

            var Transtype = $("#hdn_TransType").val();
            var cmd = $("#batch_Command").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemBatchDetail();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
               
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/AssemblyKit/GetItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            Wh_ID: Wh_ID,
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
                var Doc_no = $("#hdnPRNo").val();
                var Doc_dt = $("#ddlDoc_Date").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/AssemblyKit/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            Doc_no: Doc_no,
                            Doc_dt: Doc_dt,
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
        $("#AsembliKit_tbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hfItemID").val();
            if (ItemId == SelectedItem) {
                ValidateEyeColor(clickedrow, "btncbatchdeatil", "N");
            }
        });
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
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var Sno = clickdRow.find("#SpanRowId").text();
    var hfsno = clickdRow.find("#SNohf").val();
    var hd_Status = $("#hdn_Status").val();

    var ddlId = "#wh_id" + hfsno;
    var WhID = clickdRow.find(ddlId).val();
    var ProductNm = "";
    var ProductId = "";
    var Sub_Quantity = "";
    var UOM = "";
    var UOMID = "";
    var NewArr = [];
    ProductNm = clickdRow.find("#ComponentName_" + hfsno + " option:selected").text().trim();
    ProductId = clickdRow.find("#ComponentName_" + hfsno).val();
    UOM = clickdRow.find("#UOM").val();
    UOMID = clickdRow.find("#UOMID").val();

    Sub_Quantity = clickdRow.find("#InputQty").val();

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
    var Doc_no = $("#hdnPRNo").val();
    var Doc_dt = $("#ddlDoc_Date").val();
    var IsDisabled = $("#DisableSubItem").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/AssemblyKit/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            Uom_id: UOMID,
            WhID: WhID,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
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
function ResetWorningBorderColor() {
    var Tbllen = $("#AsembliKit_tbl >tbody>tr").length;

    if (Tbllen > 0) {
        return Cmn_CheckValidations_forSubItems("AsembliKit_tbl", "", "hfItemID", "InputQty", "SubItemInputQty", "N");

    }
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
        var QtyDecDigit = $("#QtyDigit").text();
        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#InputQty").val();
        var hfsno = clickedrow.find("#SNohf").val();
        var ItemName = clickedrow.find("#ComponentName_" + hfsno + " option:selected").text().trim();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#ComponentName_" + hfsno).val();
        var UOMID = clickedrow.find("#UOMID").val();
        var WhID = clickedrow.find("#WhID").val();
        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "") {
            clickedrow.find("#InputQty_specError").text($("#FillQuantity").text());
            clickedrow.find("#InputQty_specError").css("display", "block");
            clickedrow.find("#InputQty").css("border-color", "red");
        }
        else {

            var Status = $("#hdn_Status").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemSerialDetail();

                var SelectedItemSerial = $("#HDSelectedSerialwise").val();
               
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/AssemblyKit/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WhID: WhID,
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
                var Doc_no = $("#hdnPRNo").val();
                var Doc_dt = $("#ddlDoc_Date").val();
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/AssemblyKit/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            Doc_no: Doc_no,
                            Doc_dt: Doc_dt,
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

        $("#AsembliKit_tbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var hfsno = clickedrow.find("#SNohf").val();
            var ItemId = clickedrow.find("#ComponentName_" + hfsno).val();
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
function onchangeInputQty(e ,evt) {
    var clickedrow = $(evt.target).closest("tr");
    var InputQty = clickedrow.find("#InputQty").val();
    var AvailableStock = clickedrow.find("#AvailableStock").val();
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();

    if (parseFloat(InputQty).toFixed(parseFloat(QtyDecDigit)) == parseFloat(0).toFixed(parseFloat(QtyDecDigit))
        || InputQty == "" || InputQty == null || InputQty == "0") {

        clickedrow.find("#InputQty").css("border-color", "Red");
        clickedrow.find('#InputQty_specError').text($("#valueReq").text());
        clickedrow.find("#InputQty_specError").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        clickedrow.find("#InputQty_specError").css("display", "none");
        clickedrow.find("#InputQty").css("border-color", "#ced4da");
        var InputQty1 = parseFloat(parseFloat(InputQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#InputQty").val(InputQty1);
    }
   

    if (parseFloat(InputQty) > parseFloat(AvailableStock)) {
        var errMessage = $("#ExceedingQty").text();
        clickedrow.find("#InputQty").css("border-color", "Red");
        clickedrow.find('#InputQty_specError').text(errMessage);
        clickedrow.find("#InputQty_specError").css("display", "block");
    }
    else {
        clickedrow.find("#InputQty_specError").css("display", "none");
        clickedrow.find("#InputQty").css("border-color", "#ced4da");
    }
}
function OnchangeAssemblyProduct() {
    var AssemblyProduct = $("#ddlAssemblyProduct option:selected").val();

    if (AssemblyProduct == "0" || AssemblyProduct == "" || AssemblyProduct == null) {
        $("[aria-labelledby='select2-ddlAssemblyProduct-container']").css("border-color", "Red");
        $('#SpanAssemblyProduct').text($("#valueReq").text());
        $("#SpanAssemblyProduct").css("display", "block");
    }
    else {
        $("#SpanAssemblyProduct").css("display", "none");
        $("#ddlAssemblyProduct").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlAssemblyProduct-container']").css("border-color", "#ced4da");
        $("#hdnAssemblyProductID").val(AssemblyProduct);
        Cmn_BindUOM(null, AssemblyProduct, "", "N", "AssemblyKit_Header");
    }
}
function OnchangeHDWarehouse() {
    var ddlHDWarehouse = $("#ddlHDWarehouse option:selected").val();
    if (ddlHDWarehouse == "0" || ddlHDWarehouse == "" || ddlHDWarehouse == null) {
        $("[aria-labelledby='select2-ddlHDWarehouse-container']").css("border-color", "Red");
        $('#SpanHeaderWareHouse').text($("#valueReq").text());
        $("#SpanHeaderWareHouse").css("display", "block");
    }
    else {
        $("#SpanHeaderWareHouse").css("display", "none");
        $("#ddlHDWarehouse").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlHDWarehouse-container']").css("border-color", "#ced4da");
        $("#WHIDHeader").val(ddlHDWarehouse);
    }
}
function OnchangeHeaderAssemblyQty() {
    var QtyDecDigit = $("#QtyDigit").text();
    var AssemblyQty = $("#AssemblyQty").val();
    if (AssemblyQty == "0" || AssemblyQty == "" || AssemblyQty == null ||
        parseFloat(AssemblyQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        $("#AssemblyQty").css("border-color", "Red");
        $('#SapnAssemblyQuantity').text($("#valueReq").text());
        $("#SapnAssemblyQuantity").css("display", "block");
    }
    else {
        $("#SapnAssemblyQuantity").css("display", "none");
        $("#AssemblyQty").css("border-color", "#ced4da");
        var AssemblyQty1 = parseFloat(parseFloat(AssemblyQty)).toFixed(parseFloat(QtyDecDigit));
        $("#AssemblyQty").val(AssemblyQty1);
    }
}
function CheckFormValidation() {
    debugger;
    var header = HeaderValidation();
    if (header == false) {
        return false;
    }
    var CheckItemValid = CheckItemValidation();
    if (CheckItemValid == false) {
        return false;
    }
    var subitemvalid = CheckValidations_forSubItems()
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
    if (CheckItemValid == true && batchvalid == true && serialValid == true) {
        var ItemDetailList = new Array();
        $("#AsembliKit_tbl TBODY TR").each(function () {
            var row = $(this);
            var InputItemList = {};

            var Sno_Text = row.find("#SNohf").val();

            var ddlId = "#wh_id" + Sno_Text;
            InputItemList.ItemId = row.find("#hfItemID").val();
            InputItemList.UOMId = row.find('#UOMID').val();
            InputItemList.WareHouse = row.find(ddlId).val();
            InputItemList.InputQty = row.find('#InputQty').val();
            InputItemList.remarks = row.find('#ItemRemarks').val();
            InputItemList.uom_alias = row.find('#UOM').val();
            InputItemList.avl_stock = row.find('#AvailableStock').val();
            InputItemList.hdi_cbatch = row.find('#hdn_batch').val();
            InputItemList.hdi_cserial = row.find('#hdn_serial').val();
            ItemDetailList.push(InputItemList);
            debugger;
        });
        var InputItmStr = JSON.stringify(ItemDetailList);
        $("#hdnItemDetail").val(InputItmStr);

        var SubItemsListArr = BindInputSubitem();
        var SubitemData = JSON.stringify(SubItemsListArr);
        $('#InputSubItemDetailsDt').val(SubitemData);

        Comn_BindItemBatchDetail();
        Comn_BindItemSerialDetail();

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    }
   
}
function HeaderValidation() {
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    var WareHouse = $("#ddlHDWarehouse").val();
    var AssemblyProduct = $("#ddlAssemblyProduct option:selected").val();
    var AssemblyQty = $("#AssemblyQty").val();
    if (AssemblyProduct == "0" || AssemblyProduct == "" || AssemblyProduct == null) {
        $("[aria-labelledby='select2-ddlAssemblyProduct-container']").css("border-color", "Red");
        $('#SpanAssemblyProduct').text($("#valueReq").text());
        $("#SpanAssemblyProduct").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanAssemblyProduct").css("display", "none");
        $("#ddlAssemblyProduct").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlAssemblyProduct-container']").css("border-color", "#ced4da");
    }
    if (WareHouse == "0" || WareHouse == "" || WareHouse == null) {
        $("[aria-labelledby='select2-ddlHDWarehouse-container']").css("border-color", "Red");
        $('#SpanHeaderWareHouse').text($("#valueReq").text());
        $("#SpanHeaderWareHouse").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanHeaderWareHouse").css("display", "none");
        $("#ddlHDWarehouse").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlHDWarehouse-container']").css("border-color", "#ced4da");
    }
    if (AssemblyQty == "0" || AssemblyQty == "" || AssemblyQty == null ||
        parseFloat(AssemblyQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        $("#AssemblyQty").css("border-color", "Red");
        $('#SapnAssemblyQuantity').text($("#valueReq").text());
        $("#SapnAssemblyQuantity").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SapnAssemblyQuantity").css("display", "none");
        $("#AssemblyQty").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidation() {
    var ErrorFlag = "N";
    if ($("#AsembliKit_tbl >tbody >tr").length == 0) {
        ErrorFlag = "Y";
        swal("", $("#InputItemNotSelected").text(), "warning");
        return false;
    }
    else {
        $("#AsembliKit_tbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohf").val();
            var Sno_Text = currentRow.find("#SpanRowId").text();
            var SNohf = currentRow.find("#SNohf").val();
            var InputQty = currentRow.find("#InputQty").val();
            var errMessage = $("#ExceedingQty").text();
            var avlstck = currentRow.find("#AvailableStock").val();
            
            if (currentRow.find("#ComponentName_" + Sno).val() == "" || currentRow.find("#ComponentName_" + Sno).val() == "0") {

                ErrorFlag = "Y";
                swal("", $("#InputItemNotSelected").text(), "warning");
                //return false;
            }
            if (ErrorFlag == "N")
            {
                    if (InputQty == "0" || InputQty == "" || InputQty == null) {
                        currentRow.find("#InputQty").css("border-color", "Red");
                        currentRow.find('#InputQty_specError').text($("#valueReq").text());
                        currentRow.find("#InputQty_specError").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        if (parseFloat(InputQty) > parseFloat(avlstck)) {
                            currentRow.find("#InputQty").css("border-color", "Red");
                            currentRow.find('#InputQty_specError').text(errMessage);
                            currentRow.find("#InputQty_specError").css("display", "block");
                            ErrorFlag = "Y";
                        }
                        else {
                            currentRow.find("#InputQty_specError").css("display", "none");
                            currentRow.find("#InputQty").css("border-color", "#ced4da");
                        }
                }
                var ddlId = "#wh_id" + SNohf;
                var whERRID = "#wh_Error";
                if (currentRow.find(ddlId).val() == "0" || currentRow.find(ddlId).val()=="") {
                    debugger;
                    currentRow.find(whERRID).text($("#valueReq").text());
                    currentRow.find(whERRID).css("display", "block");
                    currentRow.find(ddlId).css("border-color", "red");
                    currentRow.find('[aria-labelledby="select2-wh_id' + SNohf + '-container"]').css("border", "1px solid red");
                }
                else {
                    var WHName = $("#wh_id" + SNohf + " option:selected").text();
                    $("#hdwh_name").val(WHName)
                    currentRow.find(whERRID).css("display", "none");
                    currentRow.find(ddlId).css("border-color", "#ced4da");
                    currentRow.find('[aria-labelledby="select2-wh_id' + SNohf + '-container"]').css("border", "1px solid #ced4da");
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
function CheckValidations_forSubItems() {
    debugger;
    var ErrFlg = "";
    $("#hdnPopupFlag").val("Input");
    if (Cmn_CheckValidations_forSubItems("AsembliKit_tbl", "", "hfItemID", "InputQty", 'SubItemInputQty', "Y") == false) { /*"SubItemConsumeQuantity"*/
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }


}
function CheckItemBatch_Validation() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    $("#AsembliKit_tbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#InputQty").val();
        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Batchable = clickedrow.find("#hdn_batch").val();

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
    $("#AsembliKit_tbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#InputQty").val();
        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Serialable = clickedrow.find("#hdn_serial").val();

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
function BindInputSubitem() {
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
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

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#hdnPRNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)

        ///  var Doc_Status = clickedrow.children("#Doc_Status").text();
        // Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function ForwardBarHistoryClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#hdnPRNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)

        ///  var Doc_Status = clickedrow.children("#Doc_Status").text();
        // Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
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
    DocNo = $("#hdnPRNo").val();
    DocDate = $("#ddlDoc_Date").val();
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
            window.location.href = "/ApplicationLayer/AssemblyKit/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/AssemblyKit/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/AssemblyKit/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            // await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            await Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/AssemblyKit/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData;
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
        var kitDt = $("#ddlDoc_Date").val();
        $.ajax({
            type: "POST",
            /* url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: kitDt
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
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

function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "AsembliKit_tbl", [{ "FieldId": "ComponentName_", "FieldType": "select", "SrNo": "SNohf" }]);
}