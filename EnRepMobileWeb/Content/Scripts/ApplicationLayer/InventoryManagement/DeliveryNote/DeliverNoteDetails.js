$(document).ready(function () {
    debugger;
    var Doc_no = $("#DeliveryNoteNumber").val();
    $("#hdDoc_No").val(Doc_no);

    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#DeliveryNoteNumber").val() == "" || $("#DeliveryNoteNumber").val() == null) {
        $("#txtDnDate").val(CurrentDate);
    }

    BindDnSupplierList();
    $('#dnItemDetails').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var ItemID = $(this).closest('tr').find("#hdItemId").val();
        var Dn_no = $("#DeliveryNoteNumber").val();
        if (Dn_no == null || Dn_no == "") {
            if ($('#DnItmDetailsTbl tr').length <= 1) {
                debugger;
                $("#ddlSupplierNameList").prop("disabled", false);
                $("#ddlDocumentNumber").prop("disabled", false);
                $("#hdSelectedSourceDocument").val(null);
                //$("#BtnAttribute").css('display', 'block');
                $(".plus_icon1").css('display', 'block');
            }
        }
        Cmn_DeleteSubItemQtyDetail(ItemID);
        updateItemSerialNumber();

    });
    OnChangeSupp();
    CancelledRemarks("#Cancelled", "Disabled");

    var type = "";
    if ($("#rbOrder").is(":checked")) {
        type = "DN";
    }

    if ($("#rbGatePass").is(":checked")) {
        type = "DNCPO";
    }
    DynamicSerchableItemDDL("", "#itemNameList", "", "", "", type);
});


function OnchangeItemName() {
   
    var item_id = $("#itemNameList").val();
    if (item_id != "0" && item_id != "" &&  item_id != null) {
        $("#header_ItemID").val(item_id);
    }
    else {
        $("#header_ItemID").val("0");
    }
    BindDocumentNumberList();
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
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#DnItmDetailsTbl >tbody >tr").each(function (i, row) {
    debugger;
    var currentRow = $(this);
    SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

});
};
function CheckFormValidation() {
        
    debugger;

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


        var rowcount = $('#DnItmDetailsTbl tr').length;
    var bill_date = $('#txtbilldate').val();
    
        var BillNumber = $('#BillNumber').val();
        var ValidationFlag = true;
        var SupplierName = $('#ddlSupplierNameList').val();
    var DocumentNumber = $('#ddlDocumentNumber').val();


        var tDnDate = $('#txtsrcdocdate').val();
        if (bill_date == "") {
            document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
            $("#txtbilldate").css("border-color", "red");
            ValidationFlag = false;
        }
        
        if (SupplierName == "" || SupplierName == "0") {
            document.getElementById("vmsupp_id").innerHTML = $("#valueReq").text();
            $(".select2-container--default .select2-selection--single").css("border-color", "red");
          
            ValidationFlag = false;
    }
    document.getElementById("vmsrc_doc_no").innerHTML = "";
    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
        //if (DocumentNumber == "" || DocumentNumber == "0") {
        //    //document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        //    //$("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
        //    //$("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
        //    document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        //    $("#ddlDocumentNumber").css("border-color", "red");
        //    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
        //    ValidationFlag = false;
        //}

    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ValidationFlag = false;
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }
    if (BillNumber == "") {
        $("#vmbill_no").text($("#valueReq").text());
        $("#vmbill_no").css("display", "block");
        $("#BillNumber").css("border-color", "red");
        ValidationFlag = false;
    }
    else {
        if ($("#duplicateBillNo").val() == "Y") {
            $("#BillNumber").css("border-color", "Red");
            $('#vmbill_no').text($("#valueduplicate").text());
            $("#vmbill_no").css("display", "block");
            return false;
        }
        else {
            $("#vmbill_no").css("display", "none");
            $("#BillNumber").css("border-color", "#ced4da");
        }
    }
   
    var Billdate = $("#txtbilldate").val();
    var Podate = $("#txtsrcdocdate").val();
    //var newPOdate =Podate.split("-").reverse().join("-");
    debugger;
    if ((Billdate != null || Billdate == "") && (Podate != null || Podate == "")) {
        //if (Billdate < Podate) {
        //    swal("", $("#BillDateExceedsSourceDate").text(), "warning");
        //            $("#txtbilldate").val("");
        //            ValidationFlag = false;
        //        }
            }   
        if (ValidationFlag == true) {
            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $('#hdn_Attatchment_details').val(ItemAttchmentDt);
            /*----- Attatchment End--------*/
            
            if (rowcount > 1) {
                var flag = CheckSOItemValidations();
                var subitmFlag = CheckValidations_forSubItems();
                if (subitmFlag == false) {
                    return false;
                }
                if (flag == true && subitmFlag== true) {
                   
                    var QtyDecDigit = $("#QtyDigit").text();
                    var DeliveryNoteItemDetailList = new Array();
                    $("#DnItmDetailsTbl TBODY TR").each(function () {
                        debugger;
                        var row = $(this);
                        var rownumber = row.find("#hdSpanRowID").val();
                        //var sampleId = "Sample" + rownumber;
                        //var QCId = "QC" + rownumber;

                        var ItemList = {};
                        var QcFlag, SampleFlag;
                        if (row.find("#QC_" + rownumber).is(":checked"))
                            QcFlag = 'Y';
                        else
                            QcFlag = 'N';

                        if (row.find("#Sample" + rownumber).is(":checked"))
                            SampleFlag = 'Y';
                        else
                            SampleFlag = 'N';

                        debugger;
                        ItemList.SourceDocumentNo = row.find("#po_no").val();
                        //ItemList.SourceDocumentDate = row.find("#SourceDate").val();
                        ItemList.SourceDocumentDate = row.find("#hfsourceDate").val();
                        ItemList.ItemId = row.find('#hdItemId').val();
                        ItemList.ItemName = row.find("#ItemName").val();
                        ItemList.subitem = row.find("#sub_item").val();
                        ItemList.UOMId = row.find('#hdUOMId').val();
                        ItemList.uom_name = row.find('#UOM').val();
                        ItemList.OrderQty = row.find("#OrderQuantity").val();
                        ItemList.BillledQty = row.find("#BilledQuantity").val();
                        ItemList.RecievedQty = row.find("#ReceivedQuantity").val();
                        ItemList.QCRequired = QcFlag;
                        ItemList.AcceptedQty = row.find("#AcceptedQuantity").val();
                        ItemList.RejectedQty = row.find("#RejectedQuantity").val();                        
                        ItemList.ReworkableQty = row.find("#ReworkableQuantity").val();
                        ItemList.SampleRecieved = SampleFlag;
                        ItemList.ItemRemarks = row.find('#txtItemRemarks').val();
                        ItemList.PendingQuantity = row.find('#PendingQuantity').val();
                        ItemList.DifferenceQuantity = row.find('#DifferenceQuantity').val();
                        DeliveryNoteItemDetailList.push(ItemList);
                        debugger;
                    });

                    var str = JSON.stringify(DeliveryNoteItemDetailList);
                    $('#hdDeliveryNoteItemDetailList').val(str);

                    /*-----------Sub-item-------------*/

                    var SubItemsListArr = DN_SubItemList();
                    var str2 = JSON.stringify(SubItemsListArr);
                    $('#SubItemDetailsDt').val(str2);

                    /*-----------Sub-item end-------------*/

                    var Suppname = $('#ddlSupplierNameList option:selected').text();
                    $("#Hdn_DNSuppName").val(Suppname);


                    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
               // swal("", "Delivery Note cannot be without Item Details", "warning")
                swal("", $("#noitemselectedmsg").text(), "warning");
                return false;
            }
        }
        else {
            return false;
        }
  
}
function onchangeFreightAmount(el, e) {
    debugger;
    //if (AvoidChar(el, "RateDigit") == false) {
    //    return false;
    //}
    debugger;
    var QtyDecDigit = $("#RateDigit").text();
    FreightAmount = $("#FreightAmount").val();
    $("#FreightAmount").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));

}
function onchangVehicleLoadInTonnage(el, e) {
    debugger;
    //if (AvoidChar(el, "RateDigit") == false) {
    //    return false;
    //}
    debugger;
    var QtyDecDigit = $("#RateDigit").text();
    VehicleLoadInTonnage = $("#VehicleLoadInTonnage").val();
    $("#VehicleLoadInTonnage").val(parseFloat(CheckNullNumber(VehicleLoadInTonnage)).toFixed(QtyDecDigit));

}
function checkItemWithDifferentUom(item_id, uom_id) {
    let flag = true;
    $('#DnItmDetailsTbl tbody tr #hdItemId[value="' + item_id + '"]').closest("tr").each(function () {
        let row = $(this);
        let dnUomId = row.find("#hdUOMId").val();
        if (dnUomId != uom_id) {
            flag = false;
        }
    });
    return flag;
}
function AddAttribute() {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var docno = $('#ddlDocumentNumber option:selected').text();
    $("#hd_doc_no").val(docno);
    //$("#hdsrc_doc_date").val($('#ddlDocumentNumber').val());

    if ($('#ddlDocumentNumber').val() != "0" && $('#ddlDocumentNumber').val() != "") {
        var text = $('#ddlDocumentNumber').val();
       // $("#BtnAttribute").prop("disabled", true);
        //$("#BtnAttribute").css('display', 'none');
        //$(".plus_icon1").css('display', 'none');
        debugger;
        $("#ddlSupplierNameList").prop("disabled", true);
        //$("#ddlDocumentNumber").prop("disabled", true);
        

        var hdSelectedSourceDocument = null;
        //var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();
        //$("#txtsrcdocdate").val(SourceDocumentDate);


        var SourDocumentNo = $('#ddlDocumentNumber option:selected').text();

        var SourDocumentDate = $('#txtsrcdocdate').val();
        var Item_id = $('#header_ItemID').val();
        hdSelectedSourceDocument = SourDocumentNo;
        $("#hdSelectedSourceDocument").val(hdSelectedSourceDocument);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/DeliveryNoteDetail/getDetailBySourceDocumentNo",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument, SourDocumentDate: SourDocumentDate,
                    Item_id: Item_id
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            let RowNumber = $('#DnItmDetailsTbl tbody tr').length;
                            if (arr.Table[0].order_type == "I") {
                                $("#ddlDocumentNumber").attr("disabled", true);
                                $(".plus_icon1").attr("hidden", true);
                            }
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                if (checkItemWithDifferentUom(arr.Table[i].item_id, arr.Table[i].base_uom_id)) {
                                    RowNumber++;
                                    var subitmDisable = "";
                                    if (arr.Table[i].sub_item != "Y") {
                                        subitmDisable = "disabled";
                                    }
                                    var qc_val = "";
                                    if (arr.Table[i].i_qc == 'Y') {
                                        qc_val = `<div class="custom-control custom-switch sample_issue" >
                                                                    <input type="checkbox" class="custom-control-input  margin-switch" id="QC_${RowNumber}" checked disabled>
                                                                    <label class="custom-control-label  for="QC_${RowNumber}" ></label>
                                                             </div>`
                                    }
                                    else {
                                        qc_val = `<div class="custom-control custom-switch sample_issue" >
                                        <input type="checkbox" class="custom-control-input margin-switch" id="QC_${RowNumber}" disabled>
                                            <label class="custom-control-label for="QC_${RowNumber}" ></label>
                                                             </div>`
                                    }
                                    var Placeholder = parseFloat(0).toFixed(QtyDecDigit)
                                    var BilledQty = "";
                                    var ReceivedQuantity = "";
                                    var DifferenceQuantity = "";
                                    if (parseFloat(arr.Table[i].BilledQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                        BilledQty = "";
                                    }
                                    else {
                                        BilledQty = parseFloat(arr.Table[i].BilledQty).toFixed(QtyDecDigit);
                                    }
                                    if (parseFloat(arr.Table[i].RecievedQty).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                        ReceivedQuantity = "";
                                    }
                                    else {
                                        ReceivedQuantity = parseFloat(arr.Table[i].BilledQty).toFixed(QtyDecDigit);
                                    }
                                    if (parseFloat(arr.Table[i].DifferenceQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                        DifferenceQuantity = "";
                                    }
                                    if (arr.Table[i].ItemType == "Stockable" || arr.Table[i].ItemType == "Consumable") {
                                        $('#DnItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">                                                       
                                                       <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                        <td class="sr_padding"><span id="SpanRowId">${RowNumber}</span></td>
                                                         <td style="display:none;"><input type="hidden" id="hdSpanRowID" value="${RowNumber}" style="display: none;" /></td>
                                                        <td>
                                                            <input id="po_no" class="form-control" autocomplete="off" type="text" name="PODate" disabled value="${arr.Table[i].app_po_no}">
                                                        </td>
                                                        <td>
                                                            <input id="SourceDate" class="form-control " autocomplete="off" type="text" name="PONumber"  disabled value="${arr.Table[i].po_dt}">
                                                            <input type="hidden" id="hfsourceDate" value="${arr.Table[i].po_date}" style="display: none;" />
                                                             </td>
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class=" col-sm-10 no-padding">
                                                                <input id="ItemName" class="form-control" autocomplete="off" type="text" name="ItemName" disabled value='${arr.Table[i].item_name}'>

                                                                <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                            </div>
                                                           <div class="col-sm-1 i_Icon">
                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                            </div>
                                                           <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                        </td>
                                                        <td>
                                                            <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled value="${arr.Table[i].uom_name}">
                                                            <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                                            <input type="hidden" id="hdnItemType" value="${arr.Table[i].ItemType}" style="display: none;" />
                                                        </td>
                                                                 <td>
                                                            <input id="ItemType" class="form-control" autocomplete="off" type="text" name="ItemType" placeholder="${$("#ItemType").text()}" disabled value="${arr.Table[i].ItemType}">
                                                           
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="OrderQuantity" value="${parseFloat(arr.Table[i].ord_qty_base).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled value="@dr.OrderQty">
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderQty" >
                                                            <button type="button" id="SubItemOrderQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNOrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                            
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="PendingQuantity" value="${parseFloat(arr.Table[i].pending_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="OrderQuantity" placeholder="0000.00"  disabled>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemPendingQty" >
                                                            <button type="button" id="SubItemPendingQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNPendQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                            </td>
                                                         <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="BilledQuantity" value="${BilledQty}" onkeypress="return OnKeyPressBLQty(this,event);" onchange="OnChangeBLQty(this,event)" class="form-control num_right" autocomplete="off" type="text" name="BilledQuantity" placeholder="0000.00"  >
                                                            <span id="BilledQuantity_Error" class="error-message is-visible"></span>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemBilledQty" >
                                                            <button type="button" id="SubItemBilledQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNBillQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                           <div class="col-sm-10 lpo_form no-padding">
                                                           <input id="ReceivedQuantity" value="${ReceivedQuantity}"onkeypress="return OnKeyPressRecQty(this,event);" onchange="OnChangeRecQty(this,event)" autocomplete="off"  class="form-control num_right"  type="text" name="ReceivedQuantity" placeholder="0000.00"  >
                                                           <span id="ReceivedQuantity_Error" class="error-message is-visible"></span>
                                                           </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRecivedQty" >
                                                            <button type="button" id="SubItemRecivedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNRecQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td class="num_right">
                                                             <input id="DifferenceQuantity" value="${DifferenceQuantity}" onchange="OnChangeDifferenceQuantity(this,event)" autocomplete="off"  class="form-control num_right"  type="text" name="DifferenceQuantity" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                       `+ qc_val + `
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="AcceptedQuantity" value="${parseFloat(arr.Table[i].accept_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="AcceptedQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAcceptQty" >
                                                            <button type="button" id="SubItemAcceptQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNAcceptQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                         </td>
                                                        <td>
                                                            <div class="col-sm-8 lpo_form no-padding">
                                                                <input id="RejectedQuantity" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="RejectedQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRejectQty" >
                                                            <button type="button" id="SubItemRejectQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNRejctQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon">
                                                                <button type="button" class="calculator" id="BtnReasonForReject" onclick="return onClickReasonRemarks(event,'reject')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForRejection").text()}">  </button>
                                                                <input hidden id="ReasonForReject"  />
                                                            </div>
                                                            </td>
                                                        <td>
                                                            <div class="col-sm-8 lpo_form no-padding">
                                                            <input id="ReworkableQuantity" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}"     disabled class="form-control num_right" autocomplete="" type="text" name="ReworkableQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReworkQty" >
                                                            <button type="button" id="SubItemReworkQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNRewrkQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon">
                                                                <button type="button" class="calculator" id="BtnReasonForRework" onclick="return onClickReasonRemarks(event,'rework')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_Reworkremarks").text()}">  </button>
                                                                <input hidden id="ReasonForRework"  />
                                                            </div>
                                                            </td>
                                                                        <td>
                                                                            <div class=" col-sm-10 lpo_form no-padding" >
                                                                                <input id="ShortQty" class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>
                                                                            </div>
                                                                            <div class=" col-sm-2 i_Icon" id="div_SubItemShortQty no-padding">
                                                                                <button type="button" id="SubItemShortQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCShortQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div class=" col-sm-10 lpo_form no-padding">
                                                                                <input id="SampleQty" class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>
                                                                            </div>

                                                                            <div class=" col-sm-2 i_Icon" id="div_SubItemSampleQty no-padding">
                                                                                <button type="button" id="SubItemSampleQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCSampleQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                            </div>
                                                                        </td>
                                                        <td>
                                                            <div class="custom-control custom-switch sample_issue" >
                                                                    <input type="checkbox" class="custom-control-input margin-switch" id="Sample${rowIdx}">
                                                                    <label class="custom-control-label  for="Sample${rowIdx}"> </label>
                                                             </div>
                                                         </td>
                                         
                                                        <td>
                                                                <textarea id="txtItemRemarks"  value="${arr.Table[i].it_remarks}"  class="form-control remarksmessage" name="remarks"  maxlength="100" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"></textarea>
                                                           
                                                        </td>
                                                    </tr>`);
                                    }                                  
                                }                                
                            }
                        }


                        $('#ddlDocumentNumber').attr("onchange", "");
                        $('#txtsrcdocdate').val("");
                        $('#ddlDocumentNumber').val("0").trigger('change');
                        $('#ddlDocumentNumber').attr("onchange", "ddlDocumentNumberSelect()");
                        
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                 //   alert("some error");
                }
            });

    }
    else {

        var bill_date = $('#txtbilldate').val();

        var BillNumber = $('#BillNumber').val();
        var ValidationFlag = true;
        var SupplierName = $('#ddlSupplierNameList').val();
        


       
        if (bill_date == "") {
            document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
            $("#txtbilldate").css("border-color", "red");
            ValidationFlag = false;
        }
        if (BillNumber == "") {
            document.getElementById("vmbill_no").innerHTML = $("#valueReq").text();
            $("#BillNumber").css("border-color", "red");
            ValidationFlag = false;
        }
        if (SupplierName == "" || SupplierName == "0") {
            document.getElementById("vmsupp_id").innerHTML = $("#valueReq").text();
            $(".select2-container--default .select2-selection--single").css("border-color", "red");

            ValidationFlag = false;
        }
        var Str = $("#valueReq").text();
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }
}
function OnddlDocumentNumberChange() {

    debugger;
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
async function OnTextChangeBillNumber(flag) {
    const Bill_No = $("#BillNumber").val();
    const Bill_Date = $("#txtbilldate").val();
    const DocumentMenuId = $("#DocumentMenuId").val();
    const supp_id = $("#ddlSupplierNameList").val();
    const hdn_Bill_No = $("#hdbill_no").val();
    const hdn_Bill_Dt = $("#hdbill_date").val();

    const showError = (id, msgId) => {
        $(id).css("border-color", "Red");
        $(msgId).text($("#valueduplicate").text()).css("display", "block");
    };

    const clearError = (id, msgId) => {
        $(id).css("border-color", "#ced4da");
        $(msgId).css("display", "none");
    };
    if (flag == "Onchangesupp") {
      
            clearError("#BillNumber", "#vmbill_no");
        
    }
    else {
        if (!Bill_No) {
            $("#BillNumber").css("border-color", "Red");
            $('#vmbill_no').text($("#valueReq").text()).css("display", "block");
            return;
        } else {
            clearError("#BillNumber", "#vmbill_no");
        }
    }
    if (!Bill_No) {
        await $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
            data: { supp_id, Bill_No, doc_id: DocumentMenuId, bill_dt: Bill_Date },
            dataType: "json",
            success: function (data) {
                if (data === "ErrorPage") {
                    LSO_ErrorPage();
                    return;
                }
                if (!data) return;
                const arr = JSON.parse(data);
                const result = arr?.Table[0]?.Result;
                var disable = $("#DisableData").val();
                if (disable != "Y") {
                    if (result === "Duplicate") {
                        if (hdn_Bill_No !== Bill_No) {
                            showError("#BillNumber", "#vmbill_no");
                            showError("#txtbilldate", "#vmbill_date");
                            $("#duplicateBillNo").val("Y");
                        } else if (hdn_Bill_Dt !== Bill_Date) {
                            showError("#BillNumber", "#vmbill_no");
                            showError("#txtbilldate", "#vmbill_date");
                            $("#duplicateBillNo").val("Y");
                        } else {
                            clearError("#BillNumber", "#vmbill_no");
                            clearError("#txtbilldate", "#vmbill_date");
                            $("#duplicateBillNo").val("N");
                        }
                    } else {
                        clearError("#BillNumber", "#vmbill_no");
                        clearError("#txtbilldate", "#vmbill_date");
                        $("#duplicateBillNo").val("N");
                    }
                }

            }
        });
    }
  

   // RefreshBillNoBillDateInGLDetail();
}
function OnTextChangeBillNumber1() {

    debugger;
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

    debugger;
    var BillDate = $('#txtbilldate').val().trim();


    if (BillDate != "0") {
        document.getElementById("vmbill_date").innerHTML = "";
        $("#txtbilldate").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
        $("#txtbilldate").css("border-color", "red");
    }
    OnTextChangeBillNumber("onchangeBilldate");
}
function BindDnSupplierList() {
    debugger;
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
    var Suppname = $('#ddlSupplierNameList option:selected').text();
    $("#Hdn_DNSuppName").val(Suppname);
    $("#hdsupp_id").val($("#ddlSupplierNameList").val());
    if (SuppID != "0") {

        if ($("#rbOrder").is(":checked")) {
            $(".srctypedn_typerbGatePass").attr("disabled", true);
            $("#rbOrder").val("O");
        }

        if ($("#rbGatePass").is(":checked")) {
            $(".srctypedn_typerbOrder").attr("disabled", true);
            $("#rbOrder").val("C");
        }
        document.getElementById("vmsupp_id").innerHTML = "";

        $(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
        OnTextChangeBillNumber("Onchangesupp");
    }
    else {
        $(".srctypedn_type").attr("disabled", false);
    }
    //else {
    //    document.getElementById("vmsupp_id").innerHTML = $("#valueReq").text();

    //    $(".select2-container--default .select2-selection--single").css("border-color", "red");
    //}
    debugger;
    $("#txtsrcdocdate").val("");
    BindDocumentNumberList();
    $('#ddlDocumentNumber').attr("onchange", "");
    $('#ddlDocumentNumber').val("0").trigger('change');
    $('#ddlDocumentNumber').attr("onchange", "ddlDocumentNumberSelect()");
    var type = "";
    if ($("#rbOrder").is(":checked")) {
        type = "DN";
    }

    if ($("#rbGatePass").is(":checked")) {
        type = "DNCPO";
    }
    DynamicSerchableItemDDL("", "#itemNameList", "", "", "", type);
        //$("#ddlDocumentNumber").val('0');
         
}
function BindDocumentNumberList() {
    debugger;
   var SuppID = $('#ddlSupplierNameList').val();
    var Item_id = $('#header_ItemID').val();
    var Dn_type = "";

    if ($("#rbOrder").is(":checked")) {
        Dn_type = "P";
    }

    if ($("#rbGatePass").is(":checked")) {
        Dn_type = "C";
    }

    $("#ddlDocumentNumber").select2({
        ajax: {
            url: $("#hdsrc_doc_no").val(),
            data: function (params) {
                var queryParameters = {
                    DocumentNo: params.term, // search term like "a" then "an"
                    SupplierID: SuppID,
                    Item_id: Item_id,
                    Dn_type: Dn_type
                };
                return queryParameters;
            },
            //dataType: "json",
            cache: true,
            multiple: true,
            //delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                //var pageSize,
                //    pageSize = 200;
                //var page = params.page || 1;
                //return {
                    
                //    //results:data.results,
                //    results: $.map(data, function (val, item) {
                //        debugger;
                //        return { id: val.ID, text: val.Name };
                //    })
                //};
                $("#ddlDocumentNumber").empty();
                $("#ddlDocumentNumber").append('<option value="0">---Select---</option>') // Add new option
                    .val('0') 
                    var DocList = ConvertIntoArreyDocNoList("#DnItmDetailsTbl", "#po_no");             
                data = data.filter(j => !JSON.stringify(DocList).includes(j.Name));
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
                        <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#DocNo").text()}</div>
                        <div class="col-md-3 col-xs-6 def-cursor">${$("#DocDate").text()}</div></div>
                        </strong></li></ul>`)
                }

                return {
                    results: $.map(data, function (val, Item) {
                        return {
                            id: val.Name, text: val.Name, date: val.ID };
                    }),
                    //pagination: {
                    //    more: (page * pageSize) < data.length
                    //}
                };
                //arr = data;//JSON.parse(data);
                //if (arr.length > 0) {
                //    $("#ddlDocumentNumber option").remove();
                //    $("#ddlDocumentNumber optgroup").remove();
                //    $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                //    for (var i = 0; i < arr.length; i++) {
                //        $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                //    }
                //    var firstEmptySelect = true;
                //    $('#ddlDocumentNumber').select2({
                //        templateResult: function (data) {
                //            var DocDate = $(data.element).data('date');
                //            var classAttr = $(data.element).attr('class');
                //            var hasClass = typeof classAttr != 'undefined';
                //            classAttr = hasClass ? ' ' + classAttr : '';
                //            var $result = $(
                //                '<div class="row">' +
                //                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                //                '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                //                '</div>'
                //            );
                //            return $result;
                //            firstEmptySelect = false;
                //        }
                //    });
                //}
                //document.getElementById("vmsrc_doc_no").innerHTML = null;
            },
            

        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.date + '</div>' +
               
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function ConvertIntoArreyDocNoList(TableID, DDLName) {
    let array = [];
    var Item_id = $('#header_ItemID').val();
    if (Item_id == "0" || Item_id == "" || Item_id == null) {
        $(TableID + " tbody tr").each(function () {
            let currentRow = $(this);
            let DocNo = currentRow.find(DDLName).val();
            if (DocNo != "0" && DocNo != "") {
                array.push({ id: DocNo });
            }
        });
    }
    else {
        $(TableID + " tbody tr").each(function () {
            let currentRow = $(this);
            let DocNo = currentRow.find(DDLName).val();
            let Itemid = currentRow.find("#hdItemId").val();
            if (DocNo != "0" && DocNo != "" && Itemid == Item_id) {                
                array.push({ id: DocNo });
            }
        });
    }
    return array;
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
   // var Sno = clickedrow.find("#SpanRowId").val();
    var ItmCode = clickedrow.find("#hdItemId").val();
    //var ItmCode = "";
    //var ItmName = "";
    //if (Sno == "1") {
    //    ItmCode = clickedrow.find("#SOItemListName").val();
    //    ItmName = clickedrow.find("#SOItemListName option:selected").text()
    //}
    //else {
    //    ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    //    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text()
    //}
    ItemInfoBtnClick(ItmCode);
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function CheckSOItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#DnItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        ///   var Sno = currentRow.find("#SNohiddenfiled").val();

        if (currentRow.find("#BilledQuantity").val() == "" || parseFloat(currentRow.find("#BilledQuantity").val()) == parseFloat("0") ) {
            currentRow.find("#BilledQuantity_Error").text($("#valueReq").text());
            currentRow.find("#BilledQuantity_Error").css("display", "block");
            currentRow.find("#BilledQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#BilledQuantity_Error").css("display", "none");
            currentRow.find("#BilledQuantity").css("border-color", "#ced4da");
        }
        if (currentRow.find("#ReceivedQuantity").val() == "" || parseFloat(currentRow.find("#ReceivedQuantity").val()) == parseFloat("0")) {
            currentRow.find("#ReceivedQuantity_Error").text($("#valueReq").text());
            currentRow.find("#ReceivedQuantity_Error").css("display", "block");
            currentRow.find("#ReceivedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ReceivedQuantity_Error").css("display", "none");
            currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ddlDocumentNumberSelect() {
    debugger;
    //var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();
    var SourceDocumentDate = $('#ddlDocumentNumber option:selected').data().data.date.trim();

    var date = SourceDocumentDate.split("-");

    var FDate = date[2] + '-' + date[1] + '-' + date[0];

    $("#txtsrcdocdate").val(FDate);
    var DocumentNumber = $('#ddlDocumentNumber').val();
    if (DocumentNumber != "0") {
        document.getElementById("vmsrc_doc_no").innerHTML = null;
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
         }
    else {
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
   
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function OnKeyPressBLQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
   
    clickedrow.find("#BilledQuantity_Error").css("display", "none");
    clickedrow.find("#BilledQuantity").css("border-color", "#ced4da");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}

function OnChangeBLQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var BilledQuantity = clickedrow.find("#BilledQuantity").val();
    if ((BilledQuantity == "") || (BilledQuantity == null) || (parseFloat(BilledQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit))) {  
         clickedrow.find("#BilledQuantity").val("");
    }
    else {
        var Quantity = parseFloat(parseFloat(BilledQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#BilledQuantity").val(Quantity);
        clickedrow.find("#ReceivedQuantity").val(Quantity);
        clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");

        var BilledQuantity = clickedrow.find("#BilledQuantity").val();
        DifferenceQuantity = parseFloat(BilledQuantity - BilledQuantity).toFixed(QtyDecDigit);
        clickedrow.find("#DifferenceQuantity").val(DifferenceQuantity);
    }
}
function OnKeyPressRecQty(el, evt) {
    //debugger;
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
    clickedrow.find("#ReceivedQuantity").css("border-color","#ced4da");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function OnChangeRecQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var ReceivedQuantity = clickedrow.find("#ReceivedQuantity").val();
    if (((ReceivedQuantity == "") || (ReceivedQuantity == null)) || (parseFloat(ReceivedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit))) {
        clickedrow.find("#ReceivedQuantity").val("");
    }
    else {
        var ReceivedQnty = parseFloat(parseFloat(ReceivedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#ReceivedQuantity").val(ReceivedQnty);

        var BilledQuantity = clickedrow.find("#BilledQuantity").val();
        DifferenceQuantity = parseFloat(BilledQuantity - ReceivedQuantity).toFixed(QtyDecDigit);
        clickedrow.find("#DifferenceQuantity").val(DifferenceQuantity);
    } 
}
function OnChangeDifferenceQuantity(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var DifferenceQuantity = clickedrow.find("#DifferenceQuantity").val();
    if (((DifferenceQuantity == "") || (DifferenceQuantity == null)) || (parseFloat(DifferenceQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit))) {
        clickedrow.find("#DifferenceQuantity").val("");
    }
    else {
        var ShortRec = parseFloat(parseFloat(DifferenceQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#DifferenceQuantity").val(ShortRec);
    }
}
function ReceiveQtyChange(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var str = clickedrow.find("#ReceivedQuantity").val();
    var clickedrow = $(evt.target).closest("tr");
    if (clickedrow.find("#ReceivedQuantity").val() == "" || clickedrow.find("#ReceivedQuantity").val() == "0") {
        clickedrow.find("#ReceivedQuantity_Error").css("display", "block");
        clickedrow.find("#ReceivedQuantity").css("border-color", "red");
    }
    else {
        clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
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
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    //$("#ItemCode").val("");
    //$("#ItemDescription").val("");
    //$("#PackingDetail").val("");
    //$("#Weight").val("");
    //$("#detailremarks").val("");



    ItmCode = clickedrow.find("#hdItemId").val();
    var Supp_id = $('#ddlSupplierNameList').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id)
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DPO/GetItemSupplierInfo",
    //                data: { ItemID: ItmCode, SuppID: Supp_id },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#ItemCode").val(arr.Table[0].Item_code);
    //                            $("#ItemDescription").val(arr.Table[0].item_des);
    //                            $("#PackingDetail").val(arr.Table[0].pack_dt);
    //                            $("#Weight").val(arr.Table[0].wt);
    //                            $("#detailremarks").val(arr.Table[0].remark);

    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}

/*------------- For Workflow,Forward,Approve------------------*/
function ForwardBtnClick() {
    debugger;
    //var DNStatus = "";
    //DNStatus = $('#DNStatusCode').val().trim();
    //if (DNStatus === "D" || DNStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
            
    //    }
    //    //$("#radio_reject").prop("disabled", true);
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
    debugger;
    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DnDt = $("#txtDnDate").val();
    $.ajax({
        type: "POST",
        /*  url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DnDt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var DNStatus = "";
                DNStatus = $('#DNStatusCode').val().trim();
                if (DNStatus === "D" || DNStatus === "F") {

                    if ($("#hd_nextlevel").val() === "0") {
                        $("#Btn_Forward").attr("data-target", "");
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                        $("#Btn_Approve").attr("data-target", "");

                    }
                    //$("#radio_reject").prop("disabled", true);
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
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DNNo = "";
    var DNDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SourceType = "";
    var mailerror = "";
    //var SourcDocNo = "";
    //var SourcDocDt = "";

    docid = $("#DocumentMenuId").val();
    DNNo = $("#DeliveryNoteNumber").val();
    DNDate = $("#txtDnDate").val();
    $("#hdDoc_No").val(DNNo);
    Remarks = $("#fw_remarks").val();
    SourceType = $("#ddlSourceType").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (DNNo  + ',' + "Update" + ',' + WF_status1)
    //SourcDocNo = $("#ddlPRNumberList").val();
    //SourcDocDt = $("#txtPRDate").val();


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    try {
        if (fwchkval != "Approve") {
            var pdfAlertEmailFilePath = "GateEntry_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DNNo, DNDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/DeliveryNoteDetail/SavePdfDocToSendOnEmailAlert");
            if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
                pdfAlertEmailFilePath = "";
            }
        }
    }
    catch {
        
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DNNo != "" && DNDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DNNo, DNDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DeliveryNoteDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/DeliveryNoteDetail/DeliveryNoteApprove?DN_no=" + DNNo + "&Dn_date=" + DNDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DNNo != "" && DNDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DNNo, DNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DeliveryNoteDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DNNo != "" && DNDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DNNo, DNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DeliveryNoteDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }

}
//async function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
//    debugger;
//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/DeliveryNoteDetail/SavePdfDocToSendOnEmailAlert",
//        data: { spoNo: poNo, spoDate: poDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#DeliveryNoteNumber").val();
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
function OnChangeCancelFlag() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return CheckFormValidation()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}

/***--------------------------------Sub Item Section-----------------------------------------***/
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = $("#UOM").val();
    $("#hdn_subitmFlag").val(flag);
    
    var Srcdoc_no = clickdRow.find("#po_no").val();
    var Srcdoc_dt = clickdRow.find("#hfsourceDate").val();
    var doc_no = $("#DeliveryNoteNumber").val();
    var doc_dt = $("#txtDnDate").val();

    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value='" + ProductId + "']").closest("tr")
        .find("#subItemSrcDocNo[value='" + Srcdoc_no + "']").closest('tr').each(function () {
        var row = $(this);
        var List = {};
        List.Item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.OrderQty = row.find('#subItemOrdQty').val();
        List.PendingQuantity = row.find('#subItemPendQty').val();
        List.BilledQty = row.find('#subItemBillQty').val();
        List.ReceivedQuantity = row.find('#subItemReceiveQty').val();
        List.AcceptedQty = row.find('#subItemAccptQty').val();
        List.RejectedQty = row.find('#subItemRejctQty').val();
        List.ReworkableQty = row.find('#subItemRewrkQty').val();
        List.SrcDocNo = row.find('#subItemSrcDocNo').val();
        List.SrcDocDate = row.find('#subItemSrcDocDate').val();
        NewArr.push(List);
    });
    if (flag == "DNBillQty") {
        Sub_Quantity = clickdRow.find("#BilledQuantity").val();
    }
   else if (flag == "DNRecQty") {
        Sub_Quantity = clickdRow.find("#ReceivedQuantity").val();
    }
    else if (flag == "DNPendQty") {
        Sub_Quantity = clickdRow.find("#PendingQuantity").val();
    } else if (flag == "DNOrdQty") {
        Sub_Quantity = clickdRow.find("#OrderQuantity").val().trim();
    }
    else if (flag == "DNAcceptQty") {
        Sub_Quantity = clickdRow.find("#AcceptedQuantity").val().trim();
    }
    else if (flag == "DNRejctQty") {
        Sub_Quantity = clickdRow.find("#RejectedQuantity").val().trim();
    }
    else if (flag == "DNRewrkQty") {
        Sub_Quantity = clickdRow.find("#ReworkableQuantity").val().trim();
     }
   

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $('#DNStatusCode').val();
    hd_Status = IsNull(hd_Status, "").trim();
   
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DeliveryNoteDetail/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            Srcdoc_no: Srcdoc_no,
            Srcdoc_dt: Srcdoc_dt
            
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
function fn_DNCustomReSetSubItemData(itemId) {

    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        debugger
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
       /* var subItemQty = Crow.find("#subItemQty").val();*/
        var subItemOrdQty = Crow.find("#subItemOrdQty").val();
        var subItemPendQty = Crow.find("#subItemPendQty").val();
        var subItemBillQty = Crow.find("#subItemBillQty").val();
        var subItemRecQty = Crow.find("#subItemReceiveQty").val();
        var AcceptedQty = Crow.find('#subItemAccptQty').val();
        var RejectedQty = Crow.find('#subItemRejctQty').val();
        var ReworkableQty = Crow.find('#subItemRewrkQty').val();
        var src_doc_no = $('#hdSub_SrcDocNo').val();
        var src_doc_date = $('#hdSub_SrcDocDate').val();
       
        
       
        var Rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemSrcDocNo[value='" + src_doc_no + "']").closest('tr')
            .find("#subItemId[value='" + subItemId + "']").closest('tr');
        if (Rows.length > 0) {
            Rows.each(function () {
                debugger
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemOrdQty").val(subItemOrdQty);
                InnerCrow.find("#subItemPendQty").val(subItemPendQty);
                InnerCrow.find("#subItemBillQty").val(subItemBillQty);
                InnerCrow.find("#subItemReceiveQty").val(subItemRecQty);
                InnerCrow.find('#subItemAccptQty').val(AcceptedQty);
                InnerCrow.find('#subItemRejctQty').val(RejectedQty);
                InnerCrow.find('#subItemRewrkQty').val(ReworkableQty);

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemOrdQty" value='${subItemOrdQty}'></td>
                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
                            <td><input type="text" id="subItemBillQty" value='${subItemBillQty}'></td>
                            <td><input type="text" id="subItemReceiveQty" value='${subItemRecQty}'></td>
                            <td><input type="text" id="subItemAccptQty" value='${AcceptedQty}'></td>
                            <td><input type="text" id="subItemRejctQty" value='${RejectedQty}'></td>
                            <td><input type="text" id="subItemRewrkQty" value='${ReworkableQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${src_doc_no}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${src_doc_date}'></td>
                        </tr>`);

        }

    });

}
function DN_CheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            item_id = PPItemRow.find("#" + Item_field_id + Sno).val();
        } else {
            item_id = PPItemRow.find("#" + Item_field_id).val();
        }
        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var item_PrdBillQty = PPItemRow.find("#" + "BilledQuantity").val();
        var PrdRecevQty = PPItemRow.find("#" + "ReceivedQuantity").val();
        var po_no = PPItemRow.find("#po_no").val();
       /* var item_PrdRewQty = PPItemRow.find("#" + "ReworkableQty").val();*/

        var sub_item = PPItemRow.find("#sub_item").val();
        var Sub_Quantity = 0;
        var Sub_BillQuantity = 0;
        var Sub_RecevQuantity = 0;
       
        $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr')
            .find("#subItemSrcDocNo[value='"+po_no+"']").closest('tr').each(function () {
            var Crow = $(this).closest("tr");
           //var subItemQty = Crow.find("#subItemQty").val();
            //Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            var subItemAccQty = Crow.find("#subItemBillQty").val();
            Sub_BillQuantity = parseFloat(Sub_BillQuantity) + parseFloat(CheckNullNumber(subItemAccQty));
            var subItemRejQty = Crow.find("#subItemReceiveQty").val();
            Sub_RecevQuantity = parseFloat(Sub_RecevQuantity) + parseFloat(CheckNullNumber(subItemRejQty));
           
            
        });
        if (sub_item == "Y") {
           //if (item_PrdQty != Sub_Quantity) {
           //         flag = "Y";
           //         PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
           //}
           //else
           //{
           //         PPItemRow.find("#" + SubItemButton).css("border", "");
           //}
            if (item_PrdBillQty != Sub_BillQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemBilledQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemBilledQty").css("border", "");
            }
            if (PrdRecevQty != Sub_RecevQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "SubItemRecivedQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "SubItemRecivedQty").css("border", "");
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

function CheckValidations_forSubItems() {
    var ErrFlg = "";
    if (DN_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdItemId", "BilledQuantity", "SubItemBilledQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (DN_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdItemId", "ReceivedQuantity", "SubItemRecivedQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}
function ResetWorningBorderColor() {
    var ErrFlg = "";
    if (DN_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdItemId", "BilledQuantity", "SubItemBilledQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (DN_CheckValidations_forSubItems("DnItmDetailsTbl", "", "hdItemId", "ReceivedQuantity", "SubItemRecivedQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
    
}
function DN_SubItemList() {
    var NewArr = new Array();
  var SubitmFlag=  $("#hdn_subitmFlag").val();
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        debugger;
        var BillQty = 0;
        var RecQty = 0;
        BillQty = row.find('#subItemBillQty').val();
        RecQty = row.find('#subItemReceiveQty').val();
        if ((parseFloat(CheckNullNumber(BillQty)) + parseFloat(CheckNullNumber(RecQty))) > 0) {
                var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.OrderQty = row.find('#subItemOrdQty').val();
            List.PendingQuantity = row.find('#subItemPendQty').val();
            List.BilledQty = row.find('#subItemBillQty').val();
            List.ReceivedQuantity = row.find('#subItemReceiveQty').val();
            List.AcceptedQty = row.find('#subItemAccptQty').val();
            List.RejectedQty = row.find('#subItemRejctQty').val();
            List.ReworkableQty = row.find('#subItemRewrkQty').val();
            List.src_doc_no = row.find('#subItemSrcDocNo').val();
            List.src_doc_date = row.find('#subItemSrcDocDate').val();
                NewArr.push(List);
            }
       

        
    });
    return NewArr;
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
/****======------------Reason For Reject And Rework section Start-------------======****/
//var RR_Clicked_Row;
function onClickReasonRemarks(e, flag) {
    let Row = $(e.target).closest('tr');
    //RR_Clicked_Row = Row;
    let ItemName = Row.find("#ItemName").val();
    let ItemId = Row.find("#hdItemId").val();
    let UOM = Row.find("#UOM").val();
    let ReasonRemarks = "";

    $("#RR_Remarks").attr("readonly", true);
    $("#RR_btnClose").attr("hidden", false);
    $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").val();
        let rejQty = Row.find("#RejectedQuantity").val();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);
        } else {
            Row.find("#ReasonForReject").val("");
            ReasonRemarks = "";
        }
    }
    else if (flag == "rework") {
        ReasonRemarks = Row.find("#ReasonForRework").val();

        let rwkQty = Row.find("#ReworkableQuantity").val();
        if (parseFloat(CheckNullNumber(rwkQty)) > 0) {
            $("#RR_Quantity").val(rwkQty);
        } else {
            Row.find("#ReasonForRework").val("");
            ReasonRemarks = "";
        }
    }
    $("#RR_ItemName").val(ItemName);
    $("#RR_ItemId").val(ItemId);
    $("#RR_Uom").val(UOM);
    $("#RR_Flag").val(flag);

    $("#RR_Remarks").val(ReasonRemarks);

}

/****======------------Reason For Reject And Rework section End-------------======****/

function approveonclick() {
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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "DnItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}
