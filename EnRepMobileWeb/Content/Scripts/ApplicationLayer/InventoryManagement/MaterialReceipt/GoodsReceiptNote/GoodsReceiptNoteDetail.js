/* Modifyed by Suraj Maurya on 21-10-2024 for decimal places : var converted to const*/ 
/* modifyed by shubham maurya on 11-10-2022 convert js to model */
/************************************************
Javascript Name:Goods Receipt Note Detail
Created By:Prem 
Created Date: 01-04-2021
Description: This Javascript use for the Goods Receipt Note many function
    
Modified By:
Modified Date:
Description:
*************************************************/
/*-----------------Modified by Suraj Maurya on 22-10-2024-------------------*/
const ExchDecDigit = $("#ExchDigit").text();///Amount
let hdn_ord_type = $("#hdnOrderType").val();///Amount
let QtyDecDigit = hdn_ord_type == "I" ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();///Quantity
let RateDecDigit = hdn_ord_type == "I" ? $("#ExpImpRateDigit").text() : $("#RateDigit").text();///Rate And Percentage
let ValDecDigit = hdn_ord_type == "I" ? $("#ExpImpValDigit").text() : $("#ValDigit").text();///Amount

function ResetDecimals(ord_type) {
    hdn_ord_type = ord_type;
    QtyDecDigit = hdn_ord_type == "I" ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();///Quantity
    RateDecDigit = hdn_ord_type == "I" ? $("#ExpImpRateDigit").text() : $("#RateDigit").text();///Rate And Percentage
    ValDecDigit = hdn_ord_type == "I" ? $("#ExpImpValDigit").text() : $("#ValDigit").text();///Amount
}
/*-----------------Modified by Suraj Maurya on 22-10-2024 End-------------------*/
$(document).ready(function () {
    debugger;
    RemoveSessionNew();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#GRNNumber").val() == "" || $("#GRNNumber").val() == null) {
        $("#grn_date").val(CurrentDate);
    }
    BindDDlList();
    GetAndViwe();
    jQuery(document).trigger('jquery.loaded');
    debugger;
    GRNNo = $("#GRNNumber").val();
    $("#hdDoc_No").val(GRNNo);
    if ($("#hdnOrderType").val() == "I") {
        $("#Hdn_GstApplicable").text("N");
    }
    CancelledRemarks("#CancelFlag", "Disabled");

    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#GRN_ItmDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hiddenfiledItemID' + $(this).find('#SNo').text().trim()).val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
    if ($("#StatusCode").val() != "PC") {
        $("#PurCostingRateDigit").text(RateDecDigit);
    }
    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        var currnt = $(this);
        currnt.find("#hsn_no").select2();
    });
});
function DisableTbleDetail() {
    try {
        $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            currentRow.find("#IDItemName").attr("disabled", true);
            currentRow.find("#idUOM").attr("disabled", true);
            currentRow.find("#ReceivedQuantity").attr("disabled", true);
            currentRow.find("#wh_id").attr("disabled", true);
            currentRow.find("#LotNumber").attr("disabled", true);
            currentRow.find("#BtnBatchDetail").attr("disabled", true);
            currentRow.find("#BtnSerialDetail").attr("disabled", true);
            currentRow.find("#SampleReceived").attr("disabled", true);
            currentRow.find("#remarks").attr("disabled", true);
        });

    }
    catch (ex) {
        ToAddJsErrorLogs(err)
    }
}
function BindDDlList() {
    try {
        $("#SupplierName").select2({
            ajax: {
                url: $("#SuppNameList").val(),
                data: function (params) {
                    var queryParameters = {
                        SuppName: params.term // search term like "a" then "an"
                    };
                    return queryParameters;
                },
                dataType: "json",
                cache: true,
                delay: 250,
                contentType: "application/json; charset=utf-8",
                processResults: function (data, params) {
                    if (data == 'ErrorPage') {
                        GRN_ErrorPage();
                        return false;
                    }
                    params.page = params.page || 1;
                    return {
                        results: $.map(data, function (val, item) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            },
        });

    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }
}
function ForwardHistoryBtnClick() {
    try {
        var Doc_ID = $("#DocumentMenuId").val();
        var Doc_No = $("#GRNNumber").val();
        debugger;
        if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
            Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
        return false;
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
        return false;
    }

}
function GetAndViwe() {
    try {

        debugger;
        if ($("#HdnBatchDetail").val() != null && $("#HdnBatchDetail").val() != "") {
            var arr2 = $("#HdnBatchDetail").val();
            var arr = JSON.parse(arr2);
            $("#HdnBatchDetail").val("");
            debugger;
            if (arr.length > 0) {
                var rowIdx = 0;
                var BatchDetailList = [];

                for (var j = 0; j < arr.length; j++) {
                    var RowSNo;
                    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNo").text();
                        var ItmCode = "";
                        ItmCode = currentRow.find('#hiddenfiledItemID' + SNo).val();
                        if (ItmCode == arr[j].item_id) {
                            RowSNo = SNo;
                        }
                    });

                    var BItmCode = arr[j].item_id;
                    var BBatchQty = arr[j].batch_qty;
                    var BRejBatchQty = arr[j].reject_qty;
                    var BReworkBatchQty = arr[j].rework_qty;
                    var BBatchNo = arr[j].batch_no;
                    var BExDate = arr[j].exp_dt;
                    if (BExDate == null) {
                        BExDate = "";
                    }
                    var saleable = arr[j].bt_sale;
                    var MfgName = arr[j].mfg_name;
                    var Mrp = arr[j].mfg_mrp;
                    var MfgDate = arr[j].mfg_date;
                    BatchDetailList.push({
                        UserID: ""/*UserID //Commented by Suraj on 03-08-2024*/, RowSNo: RowSNo, ItemID: BItmCode, BatchQty: BBatchQty, RejBatchQty: BRejBatchQty, ReworkBatchQty: BReworkBatchQty, BatchNo: BBatchNo, BatchExDate: BExDate, saleable: saleable, MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                    })

                }
                debugger;
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
            }
        }
        if ($("#HdnSerialDetail").val() != null && $("#HdnSerialDetail").val() != "") {
            debugger;
            var arr2 = $("#HdnSerialDetail").val();
            console.log(arr2);
            var arr = JSON.parse(arr2);
            
            $("#HdnSerialDetail").val("");
            debugger;
            if (arr.length > 0) {
                var SerialDetailList = [];

                for (var k = 0; k < arr.length; k++) {
                    var RowSNo;
                    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNo").text();
                        var ItmCode = "";
                        ItmCode = currentRow.find('#hiddenfiledItemID' + SNo).val();
                        if (ItmCode == arr[k].item_id) {
                            RowSNo = SNo;
                        }
                    });

                    var SItmCode = arr[k].item_id;
                    var SSerialNo = arr[k].serial_no;
                    var QtyType = arr[k].QtyType;
                    var MfgName = arr[k].mfg_name;
                    var Mrp = arr[k].mfg_mrp;
                    var MfgDate = arr[k].mfg_date;

                    SerialDetailList.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: SItmCode, SerialNo: SSerialNo, QtyType: QtyType
                        , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                    })
                }
                debugger;
                sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
            }
        }
        debugger;
        //EnableForEdit();
        if ($("#StatusCode").val() == "PC") {
            OnChangeExchangeRate();
        }

    }
    catch (ex) {
        //console.log(ex);
        ToAddJsErrorLogs(ex);
    }
}

//var QtyDecDigit = $("#QtyDigit").text();///Quantity
//var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
//var ValDecDigit = $("#ValDigit").text();///Amount

var LddCostDecDigit = 5;///For Landed Cost 
function InsertGRNDetail() {
    try {
        debugger;
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        //var DecDigit = $("#ValDigit").text();
        var StatusCode = $("#StatusCode").val();
        if (StatusCode == "PC") {
               //return SaveCostingDetail();
            var CostingDetail = SaveCostingDetail();
            if (CostingDetail == false) {
                return false;
            }
         
        }
        else {
            var INSDTransType = sessionStorage.getItem("INSTransType");

            if (CheckValidations() == false) {
                return false;
            }
            if (CheckItemValidations() == false) {
                return false;
            }
            
            if (CheckItemBatchValidation() == false) {
                swal("", $("#PleaseenterBatchDetails").text(), "warning");
                return false;
            }
            if (CheckItemSerialValidation() == false) {
                swal("", $("#PleaseenterSerialDetails").text(), "warning");
                return false;
            }
            //if (CheckValidations_forSubItems() == false) {
            //    return false;
            //}
            if ($("#duplicateBillNo").val() == "Y") {
                $("#BillNumber").css("border-color", "Red");
                $('#billNoErrorMsg').text($("#valueduplicate").text());
                $("#billNoErrorMsg").css("display", "block");
                return false;
            }
            else {
                $("#billNoErrorMsg").css("display", "none");
                $("#BillNumber").css("border-color", "#ced4da");
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


                Cancelled = "Y";

            }
            else {
                Cancelled = "N";
            }
            //if ($('#hfStatus').val() === "" || $('#hfStatus').val() === null) {
            //    OrdStatus = "D";
            //    GRNStatus = "D";
            //}
            //else {
            //    OrdStatus = $('#hfStatus').val();
            //    GRNStatus = $('#hfStatus').val();
            //}
            //EditItemDetails();
            debugger;

            debugger;
            var TransType = "";
            if (INSDTransType === 'Update') {
                TransType = 'Update';
            }
            else {
                TransType = 'Save';
            }
            debugger;
            var FinalItemDetail = [];

            FinalItemDetail = InsertGRNItemDetails();

            debugger;
            var str = JSON.stringify(FinalItemDetail);
            $('#hdGRNItemDetailList').val(str);

            $("#SupplierName").attr("disabled", false);
            $("#ddldeliverynoteno").attr("disabled", false);
            debugger;
            var batchdetails = [];
            batchdetails = GetGRN_ItemBatchDetails();
            debugger;
            var str1 = JSON.stringify(batchdetails);
            $('#HdnBatchDetail').val(str1);
            debugger;
            var serialdetails = [];
            serialdetails = GetGRN_ItemSerialDetails();
            debugger;
            var str2 = JSON.stringify(serialdetails);
            $('#HdnSerialDetail').val(str2);

            ///*-----------Sub-item-------------*/

            var SubItemsListArr = Cmn_SubItemList();
            var str3 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str3);

            ///*-----------Sub-item end-------------*/

            /*  BindGLVoucherDetail();*/
            var Suppname = $('#SupplierName option:selected').text();
            $("#Hdn_GRNSuppName").val(Suppname);

            $("#hdnsavebtn").val("AllreadyclickSaveBtn");

            return true;
        }

    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
        return false;
    }

};
function onchangeFreightAmount(el, e) {
    try {
        debugger;
        //if (AvoidChar(el, "RateDigit") == false) {
        //    return false;
        //}
        debugger;
        //var QtyDecDigit = $("#RateDigit").text();
        FreightAmount = $("#FreightAmount").val();
        $("#FreightAmount").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));

    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function onchangVehicleLoadInTonnage(el, e) {
    try {
        debugger;
        //if (AvoidChar(el, "RateDigit") == false) {
        //    return false;
        //}
        debugger;
        //var QtyDecDigit = $("#RateDigit").text();
        VehicleLoadInTonnage = $("#VehicleLoadInTonnage").val();
        $("#VehicleLoadInTonnage").val(parseFloat(CheckNullNumber(VehicleLoadInTonnage)).toFixed(QtyDecDigit));

    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function InsertGRNItemDetails() {
    try {
        debugger;
        var StatusCode = $("#StatusCode").val();

        var GRNItemsDetail = [];
        $("#GRN_ItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;

            var item_id = "";
            var item_name = "";
            var subitem = "";
            var uom_id = "";
            var uom_alias = "";
            var rec_qty = "";
            var reject_wh_id = "";
            var reject_qty = "";
            var rework_wh_id = "";
            var rework_qty = "";
            var wh_id = "";
            var item_rate = "";
            var lot_id = "";
            var item_gross_val = "";
            var item_tax_amt_recov = "";
            var item_tax_amt_nrecov = "";
            var item_oc_amt = "";
            var item_net_val = "";
            var item_landed_rate = "";
            var item_landed_val = "";
            var it_remarks = "";
            var short_qty = "";
            var sample_qty = "";
            var reason_rej = "";
            var reason_rwk = "";
            debugger;
            var currentRow = $(this);
            var SNo = currentRow.find("#SNo").text();
            item_id = currentRow.find("#hiddenfiledItemID" + SNo).val();
            item_name = currentRow.find("#IDItemName" + SNo).val();
            subitem = currentRow.find("#sub_item").val();
            uom_id = currentRow.find("#hfItemUOMID" + SNo).val();
            uom_alias = currentRow.find("#idUOM" + SNo).val();
            rec_qty = currentRow.find("#ReceivedQuantity" + SNo).val();
            reject_qty = currentRow.find("#RejectedQuantity" + SNo).val();
            rework_qty = currentRow.find("#ReworkQuantity" + SNo).val();
            reject_wh_id = currentRow.find("#hfRejWHID" + SNo).val();
            rej_qty = currentRow.find("#hfRejQty" + SNo).val();
            rework_wh_id = currentRow.find("#hfRewWHID" + SNo).val();
            rew_qty = currentRow.find("#hfRewQty" + SNo).val();
            wh_id = currentRow.find("#wh_id" + SNo).val();
            item_rate = currentRow.find("#hfItemRate" + SNo).val();
            if (StatusCode == "A" || StatusCode == "D") {
                lot_id = currentRow.find("#LotNumber").val();//created by hina
            }
            else if (StatusCode == "") {
                lot_id = "0";
            }
            else {
                lot_id = currentRow.find("#LotNumber" + SNo).val();
            }

            item_gross_val = currentRow.find("#hfItemGrVal" + SNo).val();
            item_tax_amt_recov = currentRow.find("#hfItemTaxAmtRecov" + SNo).val();
            item_tax_amt_nrecov = currentRow.find("#hfItemTaxAmtNoRecov" + SNo).val();
            item_oc_amt = currentRow.find("#hfItemOCAmt" + SNo).val();
            item_net_val = currentRow.find("#hfItemNetVal" + SNo).val();
            item_landed_rate = currentRow.find("#hfItemLandedRate" + SNo).val();
            item_landed_val = currentRow.find("#hfItemLandedVal" + SNo).val();
            it_remarks = currentRow.find("#remarks" + SNo).val();
            short_qty = currentRow.find("#ShortQty").val();
            sample_qty = currentRow.find("#SampleQty").val();
            reason_rej = currentRow.find("#ReasonForReject").text();
            reason_rwk = currentRow.find("#ReasonForRework").text();

            if (StatusCode == "A" || StatusCode == "D") {
                i_batch = currentRow.find("#BtnBatchDetail").val();
            }
            else {
                i_batch = currentRow.find("#hfbatchable" + SNo).val();
            }
            if (StatusCode == "A" || StatusCode == "D") {
                i_serial = currentRow.find("#BtnSerialDetail").val();
            }
            else {
                i_serial = currentRow.find("#hfserialable" + SNo).val();
            }

            GRNItemsDetail.push({
                item_id: item_id, item_name: item_name, subitem: subitem, uom_id: uom_id, uom_alias: uom_alias, rec_qty: rec_qty
                , reject_wh_id: reject_wh_id, reject_qty: reject_qty, rework_wh_id: rework_wh_id, rework_qty: rework_qty
                , wh_id: wh_id, item_rate: item_rate, lot_id: lot_id, item_gross_val: item_gross_val, item_tax_amt_recov: item_tax_amt_recov
                , item_tax_amt_nrecov: item_tax_amt_nrecov, item_oc_amt: item_oc_amt, item_net_val: item_net_val
                , item_landed_rate: item_landed_rate, item_landed_val: item_landed_val, it_remarks: it_remarks
                , short_qty: short_qty, sample_qty: sample_qty, reason_rej: reason_rej, reason_rwk: reason_rwk
                , i_batch: i_batch, i_serial: i_serial
            });
        });
        return GRNItemsDetail;
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function GetGRN_ItemBatchDetails() {
    try {
        debugger;
        var GRNItemsBatchDetail = [];
        var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
        if (FBatchDetails != null) {
            if (FBatchDetails.length > 0) {
                for (i = 0; i < FBatchDetails.length; i++) {
                    var ItemID = FBatchDetails[i].ItemID;
                    var BatchNo = FBatchDetails[i].BatchNo;
                    var BatchQty = FBatchDetails[i].BatchQty;
                    var RejBatchQty = FBatchDetails[i].RejBatchQty;
                    var ReworkBatchQty = FBatchDetails[i].ReworkBatchQty;
                    var BatchExDate = FBatchDetails[i].BatchExDate;
                    var saleable = FBatchDetails[i].saleable;
                    var MfgName = FBatchDetails[i].MfgName;
                    var Mrp = FBatchDetails[i].Mrp;
                    var MfgDate = FBatchDetails[i].MfgDate;
                    if (saleable == 'Yes' || saleable == 'Y') {
                        saleable = 'Y'
                    }
                    else {
                        saleable = 'N'
                    }
                    GRNItemsBatchDetail.push({
                        item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, reject_batch_qty: RejBatchQty, rework_batch_qty: ReworkBatchQty, exp_dt: BatchExDate, saleable: saleable
                        , MfgName: MfgName, Mrp: (IsNull(Mrp, "") == "" ? "0" : Mrp), MfgDate: MfgDate
                    });
                }
            }
        }

        return GRNItemsBatchDetail;
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function GetGRN_ItemSerialDetails() {
    try {
        debugger;
        var GRNItemsSerialDetail = [];

        var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
        if (FSerialDetails != null) {
            if (FSerialDetails.length > 0) {
                for (i = 0; i < FSerialDetails.length; i++) {
                    var ItemID = FSerialDetails[i].ItemID;
                    var SerialNo = FSerialDetails[i].SerialNo;
                    var QtyType = FSerialDetails[i].QtyType;
                    var MfgName = FSerialDetails[i].MfgName;
                    var Mrp = FSerialDetails[i].Mrp;
                    var MfgDate = FSerialDetails[i].MfgDate;
                    GRNItemsSerialDetail.push({
                        item_id: ItemID, serial_no: SerialNo, QtyType: QtyType
                        , MfgName: MfgName, Mrp: (IsNull(Mrp, "") == "" ? "0" : Mrp), MfgDate: MfgDate
                    });
                }
            }
        }

        return GRNItemsSerialDetail;
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function OnChangeDeliveryNoteNo(DeliveryNoteNo) {
    try {
        debugger;
        var DNNo = DeliveryNoteNo.value;
        if (DNNo == "---Select---") {
            $("#DeliveryNoteDate").val("");
            $("#BillNumber").val("");
            $("#BillDate").val("");

            $('#SpanDNNoErrorMsg').text($("#valueReq").text());
            $("#SpanDNNoErrorMsg").css("display", "block");
            $("[aria-labelledby='select2-ddldeliverynoteno-container']").css("border-color", "Red");
        }
        else {
            $("#SpanDNNoErrorMsg").css("display", "none");
            $("[aria-labelledby='select2-ddldeliverynoteno-container']").css("border-color", "#ced4da");
        }
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/GRNDetail/GetDeliveryNoteDetail",
                    data: {
                        DnNo: DNNo
                    },
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
                                $("#DeliveryNoteDate").val(arr.Table[0].dn_dt);
                                $("#BillNumber").val(arr.Table[0].bill_no);
                                $("#BillDate").val(arr.Table[0].bill_date);
                            }
                        }
                    },
                });
        } catch (err) {
            console.log("OnChangeDeliveryNoteNo Error : " + err.message);
            ToAddJsErrorLogs(err);
        }
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }


}
function AddItemDetail() {
    try {
        debugger;
        if (CheckValidations() == false) {
            return false;
        }
        var GNNo = $("#ddldeliverynoteno").val();
        var GNDate = $("#DeliveryNoteDate").val();
        //var QtyDecDigit = $("#QtyDigit").text();
        //var ValDecDigit = $("#ValDigit").text();
        //var RateDecDigit = $("#RateDigit").text();
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/GRNDetail/GetGRNDetails",
                data: { GNNo: GNNo, GNDate: GNDate },
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

                            var rowIdx = 0;

                            ResetDecimals(arr.Table[0].order_type);

                            var TotalGrossAmt = parseFloat(0).toFixed(ValDecDigit);
                            var TotalNonRecoverableAmt = parseFloat(0).toFixed(ValDecDigit);
                            var TotalRecoverableAmt = parseFloat(0).toFixed(ValDecDigit);
                            var TotalOCAmt = parseFloat(0).toFixed(ValDecDigit);
                            var TotalNetMRAmt = parseFloat(0).toFixed(ValDecDigit);
                            var TotalNetLandedAmt = parseFloat(0).toFixed(ValDecDigit);

                            $('#GRN_ItmDetailsTbl tbody tr').remove();
                            for (var i = 0; i < arr.Table.length; i++) {
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                /*//Commented by Suraj on 09-02-2024 for posibilty 0/0*/
                                //var ItmReceiveQty = parseFloat(arr.Table[i].recd_qty).toFixed(QtyDecDigit);
                                //var ItmLandedAmt = parseFloat(arr.Table[i].NetLandedAmt).toFixed(ValDecDigit);
                                //var ItmLandedRate = (parseFloat(ItmLandedAmt) / parseFloat(ItmReceiveQty)).toFixed(RateDecDigit);
                                var ItmLandedRate = 0;
                                $('#GRN_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                <td class="sr_padding"><span id="SNo">${rowIdx}</span> </td>
                                <td style="display: none;"><input  type="hidden" id="hiddenfiledItemID${rowIdx}" value='${arr.Table[i].item_id}' />
                                <input hidden type="text" id="sub_item" value='${arr.Table[i].sub_item}' />
                                <input  type="hidden" id="HdnSrNo" value='${rowIdx}' /></td>
                                 <td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-10 no-padding"><input id="IDItemName${rowIdx}" class="form-control" value='${arr.Table[i].item_name}' autocomplete="off" type="text" name="IDItemName" placeholder='${$("#ItemName").text()}'disabled>
                                </div><div class="col-sm-1 i_Icon"><button type="button" class="calculator" id="ItmInfoBtnIcon${rowIdx}" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button></div> <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div></td>
                                <td><input id="idUOM${rowIdx}" class="form-control" value='${arr.Table[i].uom_alias}' autocomplete="off" type="text" name="idUOM" placeholder='${$("#ItemUOM").text()}'  onblur="this.placeholder='${$("#ItemUOM").text()}'" readonly="readonly"></td>
                                <td style="display: none;"><input  type="hidden" id="hfItemUOMID${rowIdx}" value="${arr.Table[i].uom_id}" /></td></td>
                                <td>
                                <div class="col-sm-10 lpo_form no-padding">
                                <input id="ReceivedQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].recd_qty).toFixed(QtyDecDigit)}" name="ReceivedQuantity" placeholder="0000.00"  readonly="readonly">
                                </div>
                                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAcceptQty" >
                                <button type="button" id="SubItemAcceptQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('AccQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                </div>
                                </td>
                                <td>
                                    <div class="col-sm-8 lpo_form no-padding">
                                    <input id="RejectedQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" name="RejectedQuantity" placeholder="0000.00"  readonly="readonly">
                                    </div>
                                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRejectQty" >
                                    <button type="button" id="SubItemRejectQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RejQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                    <div class="col-sm-2 i_Icon">
                                        <button type="button" class="calculator" id="BtnReasonForReject" onclick="return onClickReasonRemarks(event,'reject')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_ReasonForRejection").text()}">  </button>
                                        <span hidden id="ReasonForReject">${arr.Table[i].reason_rej}</span>
                                    </div>
                                </td>
                                <td>
                                    <div class="col-sm-8 lpo_form no-padding">
                                    <input id="ReworkQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}" name="ReworkQuantity" placeholder="0000.00"  readonly="readonly">
                                    </div>
                                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReworkQty" >
                                    <button type="button" id="SubItemReworkQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RewQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                     <div class="col-sm-2 i_Icon">
                                        <button type="button" class="calculator" id="BtnReasonForRework" onclick="return onClickReasonRemarks(event,'rework')" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_Reworkremarks").text()}">  </button>
                                        <span hidden id="ReasonForRework">${arr.Table[i].reason_rwk}</span>
                                    </div>
                                </td>
                                <td>
                                    <div class=" col-sm-10 lpo_form no-padding">
                                        <input id="ShortQty" value='${parseFloat(arr.Table[i].short_qty).toFixed(QtyDecDigit)}' class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>
                                    </div>
                                    <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemShortQty">
                                        <button type="button" id="SubItemShortQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCShortQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title=${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                </td>
                                <td>
                                    <div class=" col-sm-10 lpo_form no-padding">
                                        <input id="SampleQty" value='${parseFloat(arr.Table[i].sample_qty).toFixed(QtyDecDigit)}' class="form-control num_right" autocomplete="off" type="text" name="ReworkableQty" placeholder="0000.00" disabled>
                                    </div>
                                    <div class=" col-sm-2 i_Icon no-padding" id="div_SubItemSampleQty">
                                        <button type="button" id="SubItemSampleQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCSampleQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                </td>
                                <td style="display: none;"><input  type="hidden" id="hfRejWHID${rowIdx}" value="${arr.Table[i].rej_wh_id}" /></td></td>
                                <td style="display: none;"><input  type="hidden" id="hfRejQty${rowIdx}" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" /></td></td>
                                <td style="display: none;"><input  type="hidden" id="hfRewWHID${rowIdx}" value="${arr.Table[i].rework_wh_id}" /></td></td>
                                <td style="display: none;"><input  type="hidden" id="hfRewQty${rowIdx}" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}" /></td></td>
                                <td width="7%"><div class="lpo_form">
                                <select class="form-control" id="wh_id${rowIdx}" onchange ="OnChnageWarehouse(event)"><option value="0">---Select---</option></select>
                                <input type="hidden" id="Hdn_GRN_WhName" value='' style="display: none;" />
                                <span id="wh_Error${rowIdx}" class="error-message is-visible"></span></div></td>
                                <td><input id="LotNumber${rowIdx}" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder='${$("#span_LotNumber").text()}'" disabled></td>
                                <td class="center"><button type="button" id="BtnBatchDetail" class="calculator subItmImg" onclick="OnClickBatchDetailBtnGRN(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button></td>
                                <td class="center"><button type="button" id="BtnSerialDetail" class="calculator subItmImg"  onclick="OnClickSerialDetailBtnGRN(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button></td>
                                <td hidden><div class="custom-control custom-switch" style="padding-left:40px !important;"><input type="checkbox" class="custom-control-input  margin-switch" id="SampleReceived${rowIdx}" disabled><label class="custom-control-label " for="SampleReceived${rowIdx}"></label></div></td>
                                <td><textarea id="remarks${rowIdx}"  class="form-control remarksmessage" name="remarks"  maxlength="100" onmouseover="OnMouseOver(this)"  placeholder="${$("#span_remarks").text()}"></textarea></td>
                                <td style="display: none;"><input type="hidden" id="hfbatchable${rowIdx}" value="${arr.Table[i].i_batch}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfserialable${rowIdx}" value="${arr.Table[i].i_serial}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfexpiralble${rowIdx}" value="${arr.Table[i].i_exp}" /></td>

                                <td style="display: none;"><input type="hidden" id="hfItemRate${rowIdx}" value="${parseFloat(arr.Table[i].ItemRate).toFixed(5)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemGrVal${rowIdx}" value="${parseFloat(arr.Table[i].GrossAmt).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemTaxAmtRecov${rowIdx}" value="${parseFloat(arr.Table[i].TaxRecoverAmt).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemTaxAmtNoRecov${rowIdx}" value="${parseFloat(arr.Table[i].TaxNoRecoverAmt).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemOCAmt${rowIdx}" value="${parseFloat(arr.Table[i].OcAmt).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemNetVal${rowIdx}" value="${parseFloat(arr.Table[i].NetMRAmt).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemLandedRate${rowIdx}" value="${parseFloat(ItmLandedRate).toFixed(RateDecDigit)}" /></td>
                                <td style="display: none;"><input type="hidden" id="hfItemLandedVal${rowIdx}" value="${parseFloat(arr.Table[i].NetLandedAmt).toFixed(RateDecDigit)}" /></td>

                                </tr>`);

                                TotalGrossAmt = (parseFloat(TotalGrossAmt) + parseFloat(arr.Table[i].GrossAmt)).toFixed(ValDecDigit);
                                TotalNonRecoverableAmt = (parseFloat(TotalNonRecoverableAmt) + parseFloat(arr.Table[i].TaxNoRecoverAmt)).toFixed(ValDecDigit);
                                TotalRecoverableAmt = (parseFloat(TotalRecoverableAmt) + parseFloat(arr.Table[i].TaxRecoverAmt)).toFixed(ValDecDigit);
                                TotalOCAmt = (parseFloat(TotalOCAmt) + parseFloat(arr.Table[i].OcAmt)).toFixed(ValDecDigit);
                                TotalNetMRAmt = (parseFloat(TotalNetMRAmt) + parseFloat(arr.Table[i].NetMRAmt)).toFixed(ValDecDigit);
                                TotalNetLandedAmt = (parseFloat(TotalNetLandedAmt) + parseFloat(arr.Table[i].NetLandedAmt)).toFixed(ValDecDigit);
                                //BindWarehouseList(rowIdx);
                            }
                            BindWarehouseList(null);

                            //$("#grnGrossValue").val(parseFloat(TotalGrossAmt).toFixed(ValDecDigit));
                            //$("#grnTaxAmtNonRecoverable").val(parseFloat(TotalNonRecoverableAmt).toFixed(ValDecDigit));
                            //$("#grnTaxAmtRecoverable").val(parseFloat(TotalRecoverableAmt).toFixed(ValDecDigit));
                            //$("#grnOtherCharges").val(parseFloat(TotalOCAmt).toFixed(ValDecDigit));
                            //$("#grnNetMRValue").val(parseFloat(TotalNetMRAmt).toFixed(ValDecDigit));
                            //$("#grnNetLandedValue").val(parseFloat(TotalNetLandedAmt).toFixed(ValDecDigit));
                            DisableHeaderDetail();
                            //DisableItemDetail();
                            EnableForEdit();
                        }
                        debugger;
                        if (arr.Table2.length > 0) {
                            $("#GRNumber").val(arr.Table2[0].gr_no);
                            $("#GRDate").val(arr.Table2[0].gr_date);
                            if (arr.Table2[0].freight_amt == null) {
                                $("#FreightAmount").val(parseFloat(0).toFixed(RateDecDigit));
                            }
                            else {
                                $("#FreightAmount").val(parseFloat(arr.Table2[0].freight_amt).toFixed(RateDecDigit));
                            }
                            $("#TransporterName").val(arr.Table2[0].trans_name);
                            $("#VehicleNumber").val(arr.Table2[0].veh_no);
                            $("#VehicleLoadInTonnage").val(parseFloat(arr.Table2[0].veh_load).toFixed(RateDecDigit));
                        }
                        debugger;
                        if (arr.Table3.length > 0) {
                            var rowIdx = 0;
                            for (var y = 0; y < arr.Table3.length; y++) {
                                var ItmId = arr.Table3[y].item_id;
                                var SubItmId = arr.Table3[y].sub_item_id;
                                var SubItmName = arr.Table3[y].sub_item_name;
                                var AccQty = arr.Table3[y].AccptQty;
                                var RejQty = arr.Table3[y].RejQty;
                                var RewQty = arr.Table3[y].RewrkQty;
                                var ShortQty = arr.Table3[y].short_qty;
                                var SampleQty = arr.Table3[y].sample_qty;

                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemAccQty" value='${AccQty}'></td>
                                    <td><input type="text" id="subItemRejQty" value='${RejQty}'></td>
                                    <td><input type="text" id="subItemRewQty" value='${RewQty}'></td>
                                    <td><input type="text" id="subItemShortQty" value='${ShortQty}'></td>
                                    <td><input type="text" id="subItemSampleQty" value='${SampleQty}'></td>
                                    </tr>`);
                            }

                        }
                        //                    if (arr.Table1.length > 0) {
                        //                        var rowIdx = 0;
                        //                        for (var i = 0; i < arr.Table1.length; i++) {
                        //                            $('#hd_GL_DeatilTbl tbody').append(`<tr>
                        //<td id="compid">${arr.Table1[i].comp_id}</td>
                        //<td id="brid">${arr.Table1[i].br_id}</td>
                        //<td id="dnno">${arr.Table1[i].dn_no}</td>
                        //<td id="dndt">${arr.Table1[i].dn_dt}</td>
                        //<td id="mrno">${arr.Table1[i].mr_no}</td>
                        //<td id="mrdt">${arr.Table1[i].mr_dt}</td>
                        //<td id="itemid">${arr.Table1[i].item_id}</td>
                        //<td id="mrqty">${arr.Table1[i].qty}</td>
                        //<td id="item_rate">${arr.Table1[i].item_rate}</td>
                        //<td id="id">${arr.Table1[i].id}</td>
                        //<td id="amt">${arr.Table1[i].item_rate}</td>
                        //<td id="totval">${arr.Table1[i].TotalValue}</td>
                        //<td id="type">${arr.Table1[i].Type}</td>
                        //</tr>`);

                        //                        }
                        //                    }
                        //GetAllGLID();
                    }
                },
            });
        } catch (err) {
            ToAddJsErrorLogs(err);
        }
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

};
function OnChnageWarehouse(e) {
    try {
        debugger;
        var currentRow = $(e.target).closest("tr");
        var Sno = currentRow.find("#SNo").text();
        var whid = currentRow.find("#wh_id" + Sno).val();
        var whname = currentRow.find("#wh_id" + Sno + " option:selected").text();
        //var whname = currentRow.find("#wh_id" + Sno + "option:selected").text();
        $("#Hdn_GRN_WhName").val(whname);
        if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
            currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
            currentRow.find("#wh_Error" + Sno).css("display", "block");
            currentRow.find("#wh_id" + Sno).css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error" + Sno).css("display", "none");
            currentRow.find("#wh_id" + Sno).css("border", "1px solid #aaa");
        }
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function OnClickIconBtn(e) {
    try {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var SNo = clickedrow.find("#SNo").text();
        var ItmCode = "";
        var ItmName = "";
        ItmCode = clickedrow.find("#hiddenfiledItemID" + SNo).val();
        ItmName = clickedrow.find("#IDItemName" + SNo).val();
        if (ItmCode == null || ItmCode == "") {
            ItmCode = clickedrow.find("#TxtItemId").val();
        }
        ItemInfoBtnClick(ItmCode);
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function DisableHeaderDetail() {
    try {
        $("#SupplierName").attr('disabled', true);
        $("#ddldeliverynoteno").attr('disabled', true);
        $("#GRN_AddNewItem").css("display", "none");
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function EnableForEdit() {
    try {
        debugger;
        $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNo").text();

            var Batch = currentRow.find("#hfbatchable" + Sno).val();
            var Serial = currentRow.find("#hfserialable" + Sno).val();

            currentRow.find("#wh_id" + Sno).attr("disabled", false);
            currentRow.find("#BtnBatchDetail").attr("disabled", false);
            currentRow.find("#BtnSerialDetail").attr("disabled", false);
            $("#remarks" + Sno).prop("readonly", false);

            if (Batch == "Y") {
                currentRow.find("#BtnBatchDetail").attr("disabled", false);
            }
            else {
                currentRow.find("#BtnBatchDetail").attr("disabled", true);
            }
            if (Serial == "Y") {
                currentRow.find("#BtnSerialDetail").attr("disabled", false);
            }
            else {
                currentRow.find("#BtnSerialDetail").attr("disabled", true);
            }
        });

        $("#file-1").attr('disabled', false);
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function BindWarehouseList(id) {
    try {
        debugger;
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/GRNDetail/GetWarehouseList1",
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
                        if (arr.Table.length > 0)
                        {
                            if (id == "Replicate") {
                                var PreWhVal = $("#Replicat_wh_id").val();
                                var s = '<option value="0">---Select---</option>';
                                for (var i = 0; i < arr.Table.length; i++) {
                                    s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                                }
                                $("#Replicat_wh_id").html(s);
                                $("#Replicat_wh_id").val(IsNull(PreWhVal, '0'));
                                $("#Replicat_wh_id").select2();
                            }
                            else {
                                var s = '<option value="0">---Select---</option>';
                                for (var i = 0; i < arr.Table.length; i++) {
                                    s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                                }
                                if (id == null) {
                                    $('#GRN_ItmDetailsTbl tbody tr').each(function () {
                                        var row = $(this);
                                        let srNo = row.find("#HdnSrNo").val();
                                        $("#wh_id" + srNo).html(s);
                                    });
                                } else {
                                    $("#wh_id" + id).html(s);
                                }

                                //$("#wh_id" + id).html(s);

                                var FItmDetails = JSON.parse(sessionStorage.getItem("GRNItemDetailSession"));
                                if (FItmDetails != null) {
                                    if (FItmDetails.length > 0) {
                                        for (i = 0; i < FItmDetails.length; i++) {
                                            var Wh_ID = FItmDetails[i].wh_id;
                                            var Item_ID = FItmDetails[i].item_id;

                                            $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
                                                var currentRow = $(this);
                                                var SNo = currentRow.find("#SNo").text();

                                                var ItmID = $("#hiddenfiledItemID" + SNo).val();
                                                if (ItmID == Item_ID) {
                                                    currentRow.find("#wh_id" + SNo).val(Wh_ID).prop('selected', true);
                                                }
                                            });
                                        }
                                    }
                                }
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
function CheckValidations() {
    try {
        debugger;
        if ($("#Cancelled").is(":checked")) {
            Cancelled = "Y";
        }
        else {
            Cancelled = "N";
        }
        var ErrorFlag = "N";
        if ($("#SupplierName").val() === "0") {
            $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "Red");
            $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
            $("#SpanSuppNameErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanSuppNameErrorMsg").css("display", "none");
            $("#SupplierName").css("border-color", "#ced4da");
        }

        if ($("#ddldeliverynoteno").val() == null || $("#ddldeliverynoteno").val() == "" || $("#ddldeliverynoteno").val() == "0" || $("#ddldeliverynoteno").val() == "---Select---") {
            $("[aria-labelledby='select2-ddldeliverynoteno-container']").css("border-color", "Red");
            $("#ddldeliverynoteno").css("border-color", "Red");
            $('#SpanDNNoErrorMsg').text($("#valueReq").text());
            $("#SpanDNNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanDNNoErrorMsg").css("display", "none");
            $("#ddldeliverynoteno").css("border-color", "#ced4da");
        }
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }

}
function CheckItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNo").text();

        if (currentRow.find("#IDItemName").val() == "" || currentRow.find("#IDItemName").val() == "0") {
            swal("", $("#noitemselectedmsg").text(), "warning");
            ErrorFlag = "Y";
            return false;
        }

        if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
            currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
            currentRow.find("#wh_Error" + Sno).css("display", "block");
            currentRow.find("#wh_id" + Sno).css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error" + Sno).css("display", "none");
            currentRow.find("#wh_id" + Sno).css("border", "1px solid #aaa");
        }
        if (currentRow.find("#ReceivedQuantity" + Sno).val() == "" || currentRow.find("#ReceivedQuantity" + Sno).val() == "0") {
            currentRow.find("#ReceivedQuantity" + Sno).css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ReceivedQuantity" + Sno).css("border-color", "#ced4da");
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        debugger;
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNo").text();
        var Batchable = currentRow.find("#hfbatchable" + Sno).val();
        var ItemID = currentRow.find("#hiddenfiledItemID" + Sno).val();
        if (Batchable == "Y") {
            var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));

            if (FBatchDetails != null) {
                var filteredValue = FBatchDetails.filter(function (item) {
                    return item.ItemID == ItemID;
                });
            }
            else {
                /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                /*currentRow.find("#BtnBatchDetail").css("border-color", "red");*/
                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
                //return false;
            }
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {
                    /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                    /*currentRow.find("#BtnBatchDetail").css("border-color", "red");*/
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                    //return false;
                }
            }
            else {
                /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                //currentRow.find("#BtnBatchDetail").css("border-color", "red");
                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
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
    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNo").text();
        var Serialable = currentRow.find("#hfserialable" + Sno).val();
        var ItemID = currentRow.find("#hiddenfiledItemID" + Sno).val();
        if (Serialable == "Y") {
            var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
            if (FSerialDetails != null) {
                var filteredValue = FSerialDetails.filter(function (item) {
                    return item.ItemID == ItemID;
                });
            }
            else {
                ErrorFlag = "Y";
                /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                //currentRow.find("#BtnSerialDetail").css("border-color", "red");
                ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");

            }
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {
                    ErrorFlag = "Y";
                    /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                    /*currentRow.find("#BtnSerialDetail").css("border-color", "red");*/
                    ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");

                }
            }
            else {
                ErrorFlag = "Y";
                /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
                //currentRow.find("#BtnSerialDetail").css("border-color", "red");
                ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
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
function RemoveSessionNew() {
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
}
function OnChangeSuppName(SuppID) {
    debugger;
    var Supp_id = SuppID.value;
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");

        //$("#Address").val("");
        //$("#Currency").html("");
        //$("#conv_rate").val("");
        //$("#conv_rate").prop("readonly", true);
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_GRNSuppName").val(Suppname);
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }

    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GRNDetail/GetDeliveryNoteList",
            data: { Supp_id: Supp_id },
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
                        $("#ddldeliverynoteno option").remove();
                        $("#ddldeliverynoteno optgroup").remove();
                        /*Commented and modify by Hina Sharma on 04-03-2025 to add Billno and BillDt in Dropdown*/
                        //$('#ddldeliverynoteno').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        //for (var i = 0; i < arr.Table.length; i++) {
                        //    $('#Textddl').append(`<option data-date="${arr.Table[i].dn_dt}" value="${arr.Table[i].dn_no}">${arr.Table[i].dn_no}</option>`);
                        //}
                        $('#ddldeliverynoteno').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-billno='${$("#span_BillNumber").text()}' data-billdt='${$("#span_BillDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].dn_dt}" value="${arr.Table[i].dn_no}" data-billno = "${arr.Table[i].bill_no}" data-billdt = "${arr.Table[i].bill_date}">${arr.Table[i].dn_no}</option>`);

                        }

                        var firstEmptySelect = true;
                        /*Commented and modify by Hina Sharma on 04-03-2025 to add Billno and BillDt in Dropdown*/
                        //$('#ddldeliverynoteno').select2({
                        //    templateResult: function (data) {
                        //        var DocDate = $(data.element).data('date');
                        //        var classAttr = $(data.element).attr('class');
                        //        var hasClass = typeof classAttr != 'undefined';
                        //        classAttr = hasClass ? ' ' + classAttr : '';
                        //        var $result = $(
                        //            '<div class="row">' +
                        //            '<div class="col-md-8 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                        //            '<div class="col-md-4 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                        //            '</div>'
                        //        );
                        //        return $result;
                        //        firstEmptySelect = false;
                        //    }
                        //});
                        $('#ddldeliverynoteno').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var billno = $(data.element).data('billno');
                                var billdt = $(data.element).data('billdt');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row jo_dll">' +
                                    '<div class="col-md-3 col-xs-4' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-4' + classAttr + '">' + DocDate + '</div>' +
                                    '<div class="col-md-3 col-xs-4' + classAttr + '">' + billno + '</div>' +
                                    '<div class="col-md-3 col-xs-4' + classAttr + '">' + billdt + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        $("#DeliveryNoteDate").val("");
                        $("#BillNumber").val("");
                        $("#BillDate").val("");
                    }
                }
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnClickBatchDetailBtnGRN(e) {
    debugger;

    //var QtyDecDigit = $("#QtyDigit").text();
    //var ValDecDigit = $("#ValDigit").text();
    //var RateDecDigit = $("#RateDigit").text();

    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");

    $("#SpanRejBatchQty").css("display", "none");
    $("#RejBatchQuantity").css("border-color", "#ced4da");

    $("#SpanReworkBatchQty").css("display", "none");
    $("#ReworkBatchQuantity").css("border-color", "#ced4da");

    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");

    ResetBatchDetailValGRN();
    debugger;
   
  
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNo").text();
    if (Sno == "") {
        Sno = clickedrow.find("#SNo").val();
    }
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ExpiryFlag = "";
    var ReceiveQty = 0;
    var RejectQty = 0;
    var ReworkQty = 0;
    /*if (RowIdType == "number") {*/
    ItemName = clickedrow.find("#IDItemName" + Sno + " option:selected").text()
    if (ItemName == null || ItemName == "") {
        ItemName = clickedrow.find("#IDItemName" + Sno).val()
    }
    ItemID = clickedrow.find("#hiddenfiledItemID" + Sno).val()
    ItemUOM = clickedrow.find("#idUOM" + Sno).val();
    ExpiryFlag = clickedrow.find("#hfexpiralble" + Sno).val();
    ReceiveQty = clickedrow.find("#ReceivedQuantity" + Sno).val();
    RejectQty = clickedrow.find("#RejectedQuantity" + Sno).val();
    ReworkQty = clickedrow.find("#ReworkQuantity" + Sno).val();

    var ReceQty = parseFloat(ReceiveQty).toFixed(QtyDecDigit);
    var RejQty = parseFloat(RejectQty).toFixed(QtyDecDigit);
    var RewQty = parseFloat(ReworkQty).toFixed(QtyDecDigit);

    var saleable = 'Yes'
    if ($("#SaleableFlag").is(":checked")) {
        saleable = 'Yes'
    }
    else {
        saleable = 'No'
    }

    if (ReceQty == "NaN" || ReceQty == "" || ReceQty == "0" || RejQty == "NaN" || RejQty == "" || RejQty == "0" || RewQty == "NaN" || RewQty == "" || RewQty == "0") {
        $("#BtnBatchDetail").attr("data-target", "");
        return false;
    }
    else {
        $("#BtnBatchDetail").attr("data-target", "#BatchNumber");
    }

    $("#BatchItemName").val(ItemName);
    $("#batchUOM").val(ItemUOM);
    $("#BatchReceivedQuantity").val(ReceQty);
    $("#BatchRejQuantity").val(RejQty);
    $("#BatchReworkQuantity").val(RewQty);
    $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    $("#hfItemSNo").val(Sno);
    $("#hfItemID").val(ItemID);
    $("#hfexpiryflag").val(ExpiryFlag);
    if (ExpiryFlag != "Y") {
        $("#spanexpiryrequire").css("display", "none");
    }
    else {
        $("#spanexpiryrequire").css("display", "");
    }

    var rowIdx = 0;
    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            $("#BatchDetailTbl >tbody >tr").remove();
            var EnableBatchdetail = $("#EnableBatchdetail").val();
            var GreyScale = "";
            if (EnableBatchdetail == "Disable") {
                GreyScale = "style='filter: grayscale(100%)'";
            }
            for (i = 0; i < FBatchDetails.length; i++) {
                var SUserID = FBatchDetails[i].UserID;
                var SRowID = FBatchDetails[i].RowSNo;
                var SItemID = FBatchDetails[i].ItemID;
                debugger;
                if (Sno != null && Sno != "") {
                    if (SRowID == Sno && SItemID == ItemID) {
                        var date = "";
                        if (FBatchDetails[i].BatchExDate != "" && FBatchDetails[i].BatchExDate != null) {
                            if (FBatchDetails[i].BatchExDate == "1900-01-01") {
                                date = "";
                            }
                            else {
                                date = moment(FBatchDetails[i].BatchExDate).format('DD-MM-YYYY');
                            }
                        }
                        var sales = FBatchDetails[i].saleable;
                        var MfgName = FBatchDetails[i].MfgName;
                        var Mrp = FBatchDetails[i].Mrp;
                        var MfgDate = Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].MfgDate);
                        
                        if (sales == "Y" || sales == "Yes") {
                            sales = "Yes"
                        }
                        else {
                            sales = "No"
                        }
                        debugger;
                        $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(FBatchDetails[i].RejBatchQty).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(FBatchDetails[i].ReworkBatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="td_MfgName" >${MfgName}</td>
<td id="td_MRP" class="num_right">${Mrp}</td>
<td id="td_MfgDate" >${MfgDate}</td>
<td id="td_hdn_MfgDate" hidden="hidden">${FBatchDetails[i].MfgDate}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
<td id="saleable" >${sales}</td>
</tr>`);
                    }
                }
                else {
                    debugger
                    if (SItemID == ItemID) {
                        var date = "";
                        if (FBatchDetails[i].BatchExDate != "" && FBatchDetails[i].BatchExDate != null) {
                            date = moment(FBatchDetails[i].BatchExDate).format('DD-MM-YYYY');
                        }
                        var mfgdate = Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].MfgDate);

                        $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(FBatchDetails[i].RejBatchQty).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(FBatchDetails[i].ReworkBatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="td_MfgName" >${MfgName}</td>
<td id="td_MRP" class="num_right">${Mrp}</td>
<td id="td_MfgDate" >${mfgdate}</td>
<td id="td_hdn_MfgDate" hidden="hidden" >${FBatchDetails[i].MfgDate}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
<td id="saleable" >${FBatchDetails[i].saleable}</td>
</tr>`);
                    }
                }

            }
            if (EnableBatchdetail == "Enable") {
                OnClickDeleteIconGRN();
            }

            CalculateBatchQtyTblGRN();

            var EDStatus = sessionStorage.getItem("GRNEnableDisable");
            if (EDStatus == "Disabled") {
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#BatchDeleteIcon").css("display", "none");
                });
            }
        }
        else {
            AutoGenerateBatchNoDatetimeVal();/*Added By NItesh 26032025 1505 For AutoGenerateBatchNo*/
        }
    }
    else {
        AutoGenerateBatchNoDatetimeVal();/*Added By NItesh 26032025 1505 For AutoGenerateBatchNo*/
    }
 
   
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#GRN_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SNo").text(SerialNo);
    });
};
function OnClickAddNewBatchDetailGRN() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var AcceptQty = CheckNullNumber($('#BatchQuantity').val());
    var RejQty = CheckNullNumber($('#RejBatchQuantity').val());
    var ReworkQty = CheckNullNumber($('#ReworkBatchQuantity').val());
    let MfgName = $('#txtMfgName').val();
    let mrp = $('#txtMRP').val();
    let MfgDate = $('#txtMfgDate').val();
    var saleable = 'Yes'
    if ($("#SaleableFlag").is(":checked")) {
        saleable = 'Yes'
    }
    else {
        saleable = 'No'
    }
    //var QtyDecDigit = $("#QtyDigit").text();

    var TotalQty = parseFloat(AcceptQty) + parseFloat(RejQty) + parseFloat(ReworkQty);
    if (TotalQty == "0") {

        ValidInfo = "Y";

    }
    else {
        $("#SpanBatchQty").css("display", "none");
        $("#BatchQuantity").css("border-color", "#ced4da");

        $("#SpanRejBatchQty").css("display", "none");
        $("#RejBatchQuantity").css("border-color", "#ced4da");

        $("#SpanReworkBatchQty").css("display", "none");
        $("#ReworkBatchQuantity").css("border-color", "#ced4da");

        var BQty = parseFloat(CheckNullNumber($('#BatchQuantity').val())).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat(CheckNullNumber($('#BatchReceivedQuantity').val())).toFixed(QtyDecDigit);
        var RejBQty = parseFloat(CheckNullNumber($('#RejBatchQuantity').val())).toFixed(QtyDecDigit);
        var RejQty = parseFloat(CheckNullNumber($('#BatchRejQuantity').val())).toFixed(QtyDecDigit);
        var ReworkBQty = parseFloat(CheckNullNumber($('#ReworkBatchQuantity').val())).toFixed(QtyDecDigit);
        var ReworkQty = parseFloat(CheckNullNumber($('#BatchReworkQuantity').val())).toFixed(QtyDecDigit);
        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        var TotalRejQty = parseFloat(0).toFixed(QtyDecDigit);
        var TotalReworkQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            var RejQty = parseFloat(currentRow.find("#RejBatchQty").text()).toFixed(QtyDecDigit);
            if (RejQty == null || RejQty == "") {
                RejQty = 0;
            }
            var ReworkQty = parseFloat(currentRow.find("#ReworkBatchQty").text()).toFixed(QtyDecDigit);
            if (ReworkQty == null || ReworkQty == "") {
                ReworkQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
            TotalRejQty = (parseFloat(TotalRejQty) + parseFloat(RejQty)).toFixed(QtyDecDigit);
            TotalReworkQty = (parseFloat(TotalReworkQty) + parseFloat(ReworkQty)).toFixed(QtyDecDigit);

        });

        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        TotalRejQty = (parseFloat(TotalRejQty) + parseFloat(RejBQty)).toFixed(QtyDecDigit);
        TotalReworkQty = (parseFloat(TotalReworkQty) + parseFloat(ReworkBQty)).toFixed(QtyDecDigit);
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat(CheckNullNumber($('#BatchQuantity').val())).toFixed(QtyDecDigit));
        }
        if (parseFloat(RejQty) < parseFloat(TotalRejQty)) {
            $("#RejBatchQuantity").css("border-color", "Red");
            $('#SpanRejBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanRejBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#RejBatchQuantity').val(parseFloat(CheckNullNumber($('#RejBatchQuantity').val())).toFixed(QtyDecDigit));
        }
        if (parseFloat(ReworkQty) < parseFloat(TotalReworkQty)) {
            $("#ReworkBatchQuantity").css("border-color", "Red");
            $('#SpanReworkBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanReworkBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#ReworkBatchQuantity').val(parseFloat(CheckNullNumber($('#ReworkBatchQuantity').val())).toFixed(QtyDecDigit));
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
    debugger;
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
    else {
        $("#SpanRejBatchQty").css("display", "none");
        $("#RejBatchQuantity").css("border-color", "#ced4da");

        $("#SpanReworkBatchQty").css("display", "none");
        $("#ReworkBatchQuantity").css("border-color", "#ced4da");
    }
    debugger;
    var accept_qty = $("#BatchQuantity").val()
    if (accept_qty == "") {
        accept_qty = 0;
    }
    var reject_qty = $("#RejBatchQuantity").val()
    if (reject_qty == "") {
        reject_qty = 0;
    }
    var rework_qty = $("#ReworkBatchQuantity").val()
    if (rework_qty == "") {
        rework_qty = 0;
    }
    var date = $("#BatchExpiryDate").val();
    if (date != null && date != "") {
        date = moment(date).format('DD-MM-YYYY');
    }
    else {
        date = "";
    }
    
    if (MfgDate != null && MfgDate != "") {
        MfgDate = moment(MfgDate).format('DD-MM-YYYY');
    }
    else {
        MfgDate = "";
    }
    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(CheckNullNumber(accept_qty)).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(CheckNullNumber(reject_qty)).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(CheckNullNumber(rework_qty)).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
<td id="td_MfgName" >${MfgName}</td>
<td id="td_MRP" class="num_right">${parseFloat(CheckNullNumber(mrp)).toFixed(RateDecDigit)}</td>
<td id="td_MfgDate" >${MfgDate}</td>
<td id="td_hdn_MfgDate" hidden >${$("#txtMfgDate").val()}</td>
<td id="BatchExDate" hidden="hidden">${$("#BatchExpiryDate").val()}</td>
<td>${date}</td>
<td id="saleable" >${saleable}</td>
</tr>`);

    ResetBatchDetailValGRN("Mfg_DoNotReset");
    CalculateBatchQtyTblGRN();
    OnClickDeleteIconGRN();
    $("#BatchQuantity").focus();
}
function OnClickSaveAndCloseGRN() {
    //var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ItemID = $('#hfItemID').val();
    var RowSNo = $("#hfItemSNo").val();
    var UserID = $("#UserID").text();

    var ReceiBQty = parseFloat($("#BatchReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQtyTotal").text()).toFixed(QtyDecDigit);

    var RejBQty = parseFloat($("#BatchRejQuantity").val()).toFixed(QtyDecDigit);
    var TotalRejBQty = parseFloat($("#RejBatchQtyTotal").text()).toFixed(QtyDecDigit);

    var ReworkBQty = parseFloat($("#BatchReworkQuantity").val()).toFixed(QtyDecDigit);
    var TotalReworkBQty = parseFloat($("#ReworkBatchQtyTotal").text()).toFixed(QtyDecDigit);

    var TotalReceQty = parseFloat(ReceiBQty) + parseFloat(RejBQty) + parseFloat(ReworkBQty);
    var SumofAllTotal = parseFloat(TotalBQty) + parseFloat(TotalRejBQty) + parseFloat(TotalReworkBQty);

    if (parseFloat(TotalReceQty) == parseFloat(SumofAllTotal)) {

        $("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
        //$("#hiddenfiledItemID" + RowSNo).closest("tr").find("#BtnBatchDetail").css("border-color", "");
        ValidateEyeColor($("#hiddenfiledItemID" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
        let NewArr = [];
        var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
        if (FBatchDetails != null) {
            if (FBatchDetails.length > 0) {
                for (i = 0; i < FBatchDetails.length; i++) {
                    var SUserID = FBatchDetails[i].UserID;
                    var SRowID = FBatchDetails[i].RowSNo;
                    var SItemID = FBatchDetails[i].ItemID;
                    if (SItemID == ItemID) {

                    }
                    else {
                        NewArr.push(FBatchDetails[i]);
                    }
                }
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    let currentRow = $(this);
                    let BatchQty = currentRow.find("#BatchQty").text();
                    let RejBatchQty = currentRow.find("#RejBatchQty").text();
                    let ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    let BatchNo = currentRow.find("#BatchNo").text();
                    let BatchExDate = currentRow.find("#BatchExDate").text();
                    let saleable = currentRow.find("#saleable").text();
                    let MfgName = currentRow.find("#td_MfgName").text();
                    let Mrp = currentRow.find("#td_MRP").text();
                    let MfgDate = currentRow.find("#td_hdn_MfgDate").text();

                    NewArr.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate, saleable: saleable
                        , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                    })
                });
                sessionStorage.removeItem("BatchDetailSession");
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(NewArr));
            }
            else {
                var BatchDetailList = [];
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    let currentRow = $(this);
                    let BatchQty = currentRow.find("#BatchQty").text();
                    let RejBatchQty = currentRow.find("#RejBatchQty").text();
                    let ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    let BatchNo = currentRow.find("#BatchNo").text();
                    let BatchExDate = currentRow.find("#BatchExDate").text();
                    let saleable = currentRow.find("#saleable").text();
                    let MfgName = currentRow.find("#td_MfgName").text();
                    let Mrp = currentRow.find("#td_MRP").text();
                    let MfgDate = currentRow.find("#td_hdn_MfgDate").text();

                    BatchDetailList.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate, saleable: saleable
                        , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate                    })
                });
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
            }
        }
        else {
            var BatchDetailList = [];
            $("#BatchDetailTbl >tbody >tr").each(function () {
                let currentRow = $(this);
                let BatchQty = currentRow.find("#BatchQty").text();
                let RejBatchQty = currentRow.find("#RejBatchQty").text();
                let ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                let BatchNo = currentRow.find("#BatchNo").text();
                let BatchExDate = currentRow.find("#BatchExDate").text();
                let saleable = currentRow.find("#saleable").text();
                let MfgName = currentRow.find("#td_MfgName").text();
                let Mrp = currentRow.find("#td_MRP").text();
                let MfgDate = currentRow.find("#td_hdn_MfgDate").text();

                BatchDetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate, saleable: saleable
                    , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                })
            });
            sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
        }

        $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var Sno = clickedrow.find("#SNo").text();
            var GRN_ItemID = clickedrow.find("#hiddenfiledItemID" + Sno).val();

            if (GRN_ItemID == ItemID) {
                //clickedrow.find("#BtnBatchDetail" + Sno).css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "BtnBatchDetail" + Sno, "N");
            }
        });
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function OnChangeBatchQtyGRN() {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();
    $("#BatchQuantity").css("border-color", "#ced4da");
    if ($('#BatchQuantity').val() != "0" && $('#BatchQuantity').val() != "" && $('#BatchQuantity').val() != "0") {
        $("#SpanBatchQty").css("display", "none");
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
        //Math.fround(ItmRate) > Math.fround(DisAmt)
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            //$('#BatchQuantity').val("");
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#BatchQuantity').val("0");
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
    }
}
function OnChangeRejBatchQtyGRN() {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();
    $("#RejBatchQuantity").css("border-color", "#ced4da");
    if ($('#RejBatchQuantity').val() != "0" && $('#RejBatchQuantity').val() != "" && $('#RejBatchQuantity').val() != "0") {
        $("#SpanRejBatchQty").css("display", "none");
        var BQty = parseFloat($('#RejBatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchRejQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#RejBatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });

        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        //Math.fround(ItmRate) > Math.fround(DisAmt)
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            //$('#BatchQuantity').val("");
            $("#RejBatchQuantity").css("border-color", "Red");
            $('#SpanRejBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanRejBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#RejBatchQuantity').val(parseFloat($('#RejBatchQuantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#RejBatchQuantity').val("0");
        $("#RejBatchQuantity").css("border-color", "Red");
        $('#SpanRejBatchQty').text($("#valueReq").text());
        $("#SpanRejBatchQty").css("display", "block");
    }
}
function ForwardBtnClick() {
    debugger;
    //var OMRStatus = "";
    //OMRStatus = $('#StatusCode').val().trim();
    //if (OMRStatus === "D" || OMRStatus === "F") {
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


    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DnDt = $("#grn_date").val();
    $.ajax({
        type: "POST",
        /*   url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DnDt
        },
        success: function (data) {
            /*if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var OMRStatus = "";
                OMRStatus = $('#StatusCode').val().trim();
                if (OMRStatus === "D" || OMRStatus === "F") {
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
function OnChangeReworkBatchQtyGRN() {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();
    $("#ReworkBatchQuantity").css("border-color", "#ced4da");
    if ($('#ReworkBatchQuantity').val() != "0" && $('#ReworkBatchQuantity').val() != "" && $('#ReworkBatchQuantity').val() != "0") {
        $("#SpanReworkBatchQty").css("display", "none");
        var BQty = parseFloat($('#ReworkBatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReworkQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#ReworkBatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });

        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        //Math.fround(ItmRate) > Math.fround(DisAmt)
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            //$('#BatchQuantity').val("");
            $("#ReworkBatchQuantity").css("border-color", "Red");
            $('#SpanReworkBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanReworkBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#ReworkBatchQuantity').val(parseFloat($('#ReworkBatchQuantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#ReworkBatchQuantity').val("0");
        $("#ReworkBatchQuantity").css("border-color", "Red");
        $('#SpanReworkBatchQty').text($("#valueReq").text());
        $("#SpanReworkBatchQty").css("display", "block");
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
function OnChangeMfgName(e) {
    debugger;
    var data = e.target.value;

    data = data.replaceAll("\\", "{\\}");
    data = data.replaceAll("\t", "");
    data = data.replaceAll("{\\}", "\\");
    e.target.value = data;
}
function OnChangeBatchExpiryDateGRN() {
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
function ResetBatchDetailValGRN(flag) {
    $('#BatchQuantity').val("");
    $('#RejBatchQuantity').val("");
    $('#ReworkBatchQuantity').val("");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
    if (flag != "Mfg_DoNotReset") {
        $('#txtMfgName').val("");
        $('#txtMRP').val("");
        $('#txtMfgDate').val("");
    }
}
function AutoGenerateBatchNoDatetimeVal() {
    /*Added By NItesh 26032025 1505 For AutoGenerateBatchNo*/
    var BatchNumber = $("#txtBatchNumber").val();
    if (BatchNumber == "" || BatchNumber == "0" || BatchNumber == null) {
        let now = new Date();
        let day = String(now.getDate()).padStart(2, '0'); // 2-digit day
        let month = String(now.getMonth() + 1).padStart(2, '0'); // Month (0-based)
        let year = now.getFullYear();
        let hours = String(now.getHours()).padStart(2, '0');
        let minutes = String(now.getMinutes()).padStart(2, '0');
        let seconds = String(now.getSeconds()).padStart(2, '0');
        let mileseconds = String(now.getMilliseconds()).padStart(2, '0');
        let formattedDateTime = `${day}${month}${year}${hours}${minutes}${seconds}${mileseconds}`;
        var count_formattedDateTime = formattedDateTime.length;
        if (count_formattedDateTime <= 25) {
            $("#txtBatchNumber").val(formattedDateTime);
        }
        else {
            let formattedDateTime1 = `${day}${month}${year}${hours}${minutes}${seconds}`;
            $("#txtBatchNumber").val(formattedDateTime1);
        }
       
    }
}
function CalculateBatchQtyTblGRN() {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();
    var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalRejQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalReworkQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#BatchDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);

        if (tblQty == null || tblQty == "") {
            tblQty = 0;
        }
        var RejQty = parseFloat(currentRow.find("#RejBatchQty").text()).toFixed(QtyDecDigit);
        if (RejQty == null || RejQty == "") {
            RejQty = 0;
        }
        var ReworkQty = parseFloat(currentRow.find("#ReworkBatchQty").text()).toFixed(QtyDecDigit);
        if (ReworkQty == null || ReworkQty == "") {
            ReworkQty = 0;
        }
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        TotalRejQty = (parseFloat(TotalRejQty) + parseFloat(RejQty)).toFixed(QtyDecDigit);
        TotalReworkQty = (parseFloat(TotalReworkQty) + parseFloat(ReworkQty)).toFixed(QtyDecDigit);
    });

    $('#BatchQtyTotal').text(parseFloat(TotalReceiQty).toFixed(QtyDecDigit));
    $('#RejBatchQtyTotal').text(parseFloat(TotalRejQty).toFixed(QtyDecDigit));
    $('#ReworkBatchQtyTotal').text(parseFloat(TotalReworkQty).toFixed(QtyDecDigit));



    var Batch_ReceivedQuantity = $("#BatchReceivedQuantity").val();
    var Batch_ReworkQuantity = $("#BatchReworkQuantity").val();
    var Batch_RejQuantity = $("#BatchRejQuantity").val();

    var BatchQtyTotal = $("#BatchQtyTotal").text();
    var RejBatchQtyTotal = $("#RejBatchQtyTotal").text();
    var ReworkBatchQtyTotal = $("#ReworkBatchQtyTotal").text();
    var totalBatchqty = parseFloat(Batch_ReceivedQuantity) + parseFloat(Batch_RejQuantity) + parseFloat(Batch_ReworkQuantity);
    var total_Batchqty = parseFloat(BatchQtyTotal) + parseFloat(RejBatchQtyTotal) + parseFloat(ReworkBatchQtyTotal);
    if (totalBatchqty != total_Batchqty) {
        AutoGenerateBatchNoDatetimeVal(); /*Added By NItesh 26032025 1505 For AutoGenerateBatchNo*/
    }
}
function OnClickBatchResetBtnGRN() {
  //var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValGRN();
    
    $('#BatchDetailTbl tbody tr').remove();
    $('#BatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));
    $('#RejBatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));
    $('#ReworkBatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));

    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#SpanRejBatchQty").css("display", "none");
    $("#RejBatchQuantity").css("border-color", "#ced4da");
    $("#SpanReworkBatchQty").css("display", "none");
    $("#ReworkBatchQuantity").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
    AutoGenerateBatchNoDatetimeVal(); /*Added By NItesh 26032025 1505 For AutoGenerateBatchNo*/
}
function OnClickDeleteIconGRN() {
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
        CalculateBatchQtyTblGRN();

    });
 
}
function OnClickDiscardAndExitGRN() {
    OnClickBatchResetBtnGRN();
}
//--------------------End--------------------------//

//---------------Serial Deatils-----------------------//
function OnClickSerialDetailBtnGRN(e) {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");

    var Sno = clickedrow.find("#SNo").text();
    if (Sno == "") {
        Sno = clickedrow.find("#SNo").val();
    }
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    //var ExpiryFlag = "";
    var ReceiveQty = 0;

    ItemName = clickedrow.find("#IDItemName" + Sno + " option:selected").text()
    if (ItemName == null || ItemName == "") {
        ItemName = clickedrow.find("#IDItemName" + Sno).val()
    }
    ItemID = clickedrow.find("#hiddenfiledItemID" + Sno).val()
    ItemUOM = clickedrow.find("#idUOM" + Sno).val();
    ReceiveQty = clickedrow.find("#ReceivedQuantity" + Sno).val();
    RejQty = clickedrow.find("#RejectedQuantity" + Sno).val();
    ReworkQty = clickedrow.find("#ReworkQuantity" + Sno).val();

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
    $("#SerialRejQuantity").val(RejQty);
    $("#SerialReworkQuantity").val(ReworkQty);
    $("#hfSItemSNo").val(Sno);
    $("#hfSItemID").val(ItemID);
    var rowIdx = 0;
    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails != null) {
        if (FSerialDetails.length > 0) {
            $("#SerialDetailTbl >tbody >tr").remove();
            var EnableBatchdetail = $("#EnableBatchdetail").val();
            var GreyScale = "";
            if (EnableBatchdetail == "Disable") {
                GreyScale = "style='filter: grayscale(100%)'";
            }
            for (i = 0; i < FSerialDetails.length; i++) {
                var SUserID = FSerialDetails[i].UserID;
                var SRowID = FSerialDetails[i].RowSNo;
                var SItemID = FSerialDetails[i].ItemID;
                var MfgName = FSerialDetails[i].MfgName;
                var Mrp = FSerialDetails[i].Mrp;
                var MfgDate = FSerialDetails[i].MfgDate;
                let mfg_date = (MfgDate == "" || MfgDate == "1900-01-01" || MfgDate == null) ? "" : moment(MfgDate).format("DD-MM-YYYY");
                if (SNo != null && SNo != "") {
                    if (SRowID == Sno && SItemID == ItemID) {
                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${rowIdx}</td>
<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
<td id="QtyType" >${FSerialDetails[i].QtyType}</td>
<td id="sr_MfgName" >${MfgName}</td>
<td id="sr_MRP" class="num_right">${Mrp}</td>
<td id="sr_MfgDate" >${mfg_date}</td>
<td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);
                    }
                    else {
                        if (SItemID == ItemID) {
                            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${rowIdx}</td>
<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
<td id="QtyType" >${FSerialDetails[i].QtyType}</td>
<td id="sr_MfgName" >${MfgName}</td>
<td id="sr_MRP" class="num_right">${Mrp}</td>
<td id="sr_MfgDate" >${mfg_date}</td>
<td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);
                        }
                    }
                }


            }
            if (EnableBatchdetail == "Enable") {
                OnClickSerialDeleteIconGRN();
            }
            var EDStatus = sessionStorage.getItem("GRNEnableDisable");
            if (EDStatus == "Disabled") {
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
function OnClickAddNewSerialDetailGRN() {
  //var QtyDecDigit = $("#QtyDigit").text();
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var RejSQty = parseFloat($("#SerialRejQuantity").val()).toFixed(QtyDecDigit);
    var ReworkSQty = parseFloat($("#SerialReworkQuantity").val()).toFixed(QtyDecDigit);
    let MfgName = $('#Sr_MfgName').val();
    let Mrp = parseFloat(CheckNullNumber($('#Sr_MfgMRP').val())).toFixed(RateDecDigit);
    let MfgDate = $('#Sr_MfgDate').val();
    //var TotalRecQty = parseFloat(ReceiSQty) + parseFloat(RejSQty) + parseFloat(ReworkSQty);


    var AcceptLength = parseInt(0);
    var RejLength = parseInt(0);
    var ReworkLength = parseInt(0);
    $("#SerialDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var QtyText = currentRow.find("#QtyType").text();
        if (QtyText == "Reworkable") {
            ReworkLength = parseInt(ReworkLength) + 1;
        }
        if (QtyText == "Accepted") {
            AcceptLength = parseInt(AcceptLength) + 1;
        }
        if (QtyText == "Rejected") {
            RejLength = parseInt(RejLength) + 1;
        }
    });

    //var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);

    //if (parseInt(AcceptLength) == parseInt(ReceiSQty)) {
    //    ValidInfo = "Y";
    //}
    //if (parseInt(RejLength) == parseInt(RejSQty)) {
    //    ValidInfo = "Y";
    //}
    //if (parseInt(ReworkLength) == parseInt(ReworkSQty)) {
    //    ValidInfo = "Y";
    //}
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
    let mfg_date = MfgDate == "" ? "" : moment(MfgDate).format("DD-MM-YYYY");
    if (ValidInfo == "Y") {
        return false;
    }
    else {
        var QtyType = "";
        if ($("#QuantityType").val() == "AC") {
            QtyType = "Accepted"
        }
        if ($("#QuantityType").val() == "RJ") {
            QtyType = "Rejected"
        }
        if ($("#QuantityType").val() == "RW") {
            QtyType = "Reworkable"
        }
      
        if (QtyType === "Accepted") {
            if (parseInt(AcceptLength) != parseInt(ReceiSQty)) {
                var TblLen = $('#SerialDetailTbl tbody tr').length;
                $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
<td id="sr_MfgName" >${MfgName}</td>
<td id="sr_MRP" class="num_right">${Mrp}</td>
<td id="sr_MfgDate" >${mfg_date}</td>
<td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);

                $('#SerialNo').val("");
                OnClickSerialDeleteIconGRN();
            }
        }

    }
    if (QtyType === "Rejected") {
        if (parseInt(RejLength) != parseInt(RejSQty)) {
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
<td id="sr_MfgName" >${MfgName}</td>
<td id="sr_MRP" class="num_right">${Mrp}</td>
<td id="sr_MfgDate" >${mfg_date}</td>
<td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);

            $('#SerialNo').val("");
            OnClickSerialDeleteIconGRN();
        }
    }
    if (QtyType === "Reworkable") {
        if (parseInt(ReworkLength) != parseInt(ReworkSQty)) {
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
<td id="sr_MfgName" >${MfgName}</td>
<td id="sr_MRP" class="num_right">${Mrp}</td>
<td id="sr_MfgDate" >${mfg_date}</td>
<td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
</tr>`);

            $('#SerialNo').val("");
            OnClickSerialDeleteIconGRN();
        }
    }
    $("#SerialNo").focus()
}
function OnKeyPressSerialNoGRN() {
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDeleteIconGRN() {
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
        ResetSerialNoAfterDeleteGRN();
    });
}
function ResetSerialNoAfterDeleteGRN() {
    var rowIdx = 0;
    $("#SerialDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#SerialID").text(++rowIdx);
    });
}
function OnClickSerialResetBtnGRN() {
    $('#SerialNo').val("");
    $('#Sr_MfgName').val("");
    $('#Sr_MfgMRP').val("");
    $('#Sr_MfgDate').val("");
    $('#SerialDetailTbl tbody tr').remove();
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDiscardAndExitGRN() {
    OnClickSerialResetBtnGRN();
}
function OnClickSerialSaveAndCloseGRN() {
    debugger;
  //var QtyDecDigit = $("#QtyDigit").text();

    var ItemID = $('#hfSItemID').val();
    var RowSNo = $("#hfSItemSNo").val();
    var UserID = $("#UserID").text();

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var RejSQty = parseFloat($("#SerialRejQuantity").val()).toFixed(QtyDecDigit);
    var ReworkSQty = parseFloat($("#SerialReworkQuantity").val()).toFixed(QtyDecDigit);
    var TotalRecQty = parseFloat(ReceiSQty) + parseFloat(RejSQty) + parseFloat(ReworkSQty);
    var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);
    if (parseFloat(TotalRecQty) == parseFloat(TotalSQty)) {

        $("#SerialSaveAndExitBtn").attr("data-dismiss", "modal");
        /*commented by Hina on 03-02-2024 to chng validate Eye Color*/
        /*$("#hiddenfiledItemID" + RowSNo).closest("tr").find("#BtnSerialDetail").css("border-color", "");*/
        ValidateEyeColor($("#hiddenfiledItemID" + RowSNo).closest("tr"), "BtnSerialDetail", "N");
        let NewArr = [];
        var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
        if (FSerialDetails != null) {
            if (FSerialDetails.length > 0) {
                for (i = 0; i < FSerialDetails.length; i++) {
                    var SUserID = FSerialDetails[i].UserID;
                    var SRowID = FSerialDetails[i].RowSNo;
                    var SItemID = FSerialDetails[i].ItemID;
                    if (SRowID == RowSNo && SItemID == ItemID) {
                    }
                    else {
                        NewArr.push(FSerialDetails[i]);
                    }
                }
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var SerialNo = currentRow.find("#SerialNo").text();
                    var QtyType = currentRow.find("#QtyType").text();
                    var MfgName = currentRow.find("#sr_MfgName").text();
                    var Mrp = currentRow.find("#sr_MRP").text();
                    var MfgDate = currentRow.find("#sr_hdn_MfgDate").text();
                    NewArr.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType
                        , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                    })
                });
                sessionStorage.removeItem("SerialDetailSession");
                sessionStorage.setItem("SerialDetailSession", JSON.stringify(NewArr));
            }
            else {
                var SerialDetailList = [];
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var SerialNo = currentRow.find("#SerialNo").text();
                    var QtyType = currentRow.find("#QtyType").text();
                    var MfgName = currentRow.find("#sr_MfgName").text();
                    var Mrp = currentRow.find("#sr_MRP").text();
                    var MfgDate = currentRow.find("#sr_hdn_MfgDate").text();
                    SerialDetailList.push({
                        UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType
                        , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                    })
                });
                sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
            }
        }
        else {
            var SerialDetailList = [];
            $("#SerialDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var SerialNo = currentRow.find("#SerialNo").text();
                var QtyType = currentRow.find("#QtyType").text();
                var MfgName = currentRow.find("#sr_MfgName").text();
                var Mrp = currentRow.find("#sr_MRP").text();
                var MfgDate = currentRow.find("#sr_hdn_MfgDate").text();
                SerialDetailList.push({
                    UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType
                    , MfgName: MfgName, Mrp: Mrp, MfgDate: MfgDate
                })
            });
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
        }

        OnClickSerialResetBtnGRN();
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function QtyFloatValueonly(el, evt) {
    let QtyDigit = hdn_ord_type == "I" ? "#ExpImpQtyDigit" : "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDigit) == false) {
        return false;
    }
    /*   $("VehicleLoadInTonnage").val(parseFloat().toFixed(RateDecDigit))*/
    return true;
}
function QtyRateValueonly(el, evt) {
    let RateDigit = hdn_ord_type == "I" ? "#ExpImpRateDigit" : "#RateDigit";
    if (Cmn_FloatValueonly(el, evt, RateDigit) == false) {
        return false;
    }
    /*   $("VehicleLoadInTonnage").val(parseFloat().toFixed(RateDecDigit))*/
    return true;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var GRNNO = "";
    var GRNdate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    GRNNO = $("#GRNNumber").val();
    GRNdate = $("#grn_date").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (GRNNO + ',' + GRNdate + ',' + "Update" + ',' + WF_status1);

    //var Narr = $('#JVRaisedAgainstMR').text(); 
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            docid = currentRow.find("#doc_id").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "GoodsRecieptNote_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(GRNNO, GRNdate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/GRNDetail/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && GRNNO != "" && GRNdate != "" && level != "") {
            await Cmn_InsertDocument_ForwardedDetail1(GRNNO, GRNdate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/GRNDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/GRNDetail/Approve_GoodNoteReceipt?grn_no=" + GRNNO + "&grn_dt=" + GRNdate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && GRNNO != "" && GRNdate != "") {
            await Cmn_InsertDocument_ForwardedDetail(GRNNO, GRNdate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/GRNDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && GRNNO != "" && GRNdate != "") {
            await Cmn_InsertDocument_ForwardedDetail(GRNNO, GRNdate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/GRNDetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(grnNo, grnDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/GRNDetail/SavePdfDocToSendOnEmailAlert",
//        data: { grnNo: grnNo, grnDate: grnDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OnChangeCancelFlag() {
    debugger;
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertGRNDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
}
//--------------------End----------------------------//
function OnClickSupplierInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNo").text();

    ItmCode = clickedrow.find("#hiddenfiledItemID" + SNo).val();
    var ItmCode = "";
    var ItmName = "";
    //$("#ItemCode").val("");
    //$("#ItemDescription").val("");
    //$("#PackingDetail").val("");
    //$("#Weight").val("");
    //$("#detailremarks").val("");


    ItmCode = clickedrow.find("#hiddenfiledItemID" + SNo).val();
    var Supp_id = $('#SupplierName').val();

    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);


}

/***--------------------------------Sub Item Section-----------------------------------------***/
function SubItemDetailsPopUp(flag, e) {
    debugger;

    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNo").text();

    var ProductNm = clickdRow.find("#IDItemName" + hfsno).val();
    var ProductId = clickdRow.find("#hiddenfiledItemID" + hfsno).val();
    var UOM = clickdRow.find("#idUOM" + hfsno).val();
    var Doc_no = $("#GRNNumber").val();
    var Doc_dt = $("#grn_date").val();
    var IsDisabled = "Y";//$("#DisableSubItem").val();

    var Sub_Quantity = 0;
    if (flag == "AccQuantity") {
        Sub_Quantity = clickdRow.find("#ReceivedQuantity" + hfsno).val();
    } else if (flag == "RejQuantity") {
        Sub_Quantity = clickdRow.find("#RejectedQuantity" + hfsno).val();
    } else if (flag == "RewQuantity") {
        Sub_Quantity = clickdRow.find("#ReworkQuantity" + hfsno).val();
    }
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {

        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.sub_item_name = row.find('#subItemName').val();
        List.Accqty = row.find('#subItemAccQty').val();
        List.Rejqty = row.find('#subItemRejQty').val();
        List.Rewqty = row.find('#subItemRewQty').val();
        List.Shortqty = row.find('#subItemShortQty').val();
        List.Sampleqty = row.find('#subItemSampleQty').val();
        NewArr.push(List);
    });

    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/GRNDetail/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: "RJO_QCAccptQty",/*For Reuse Code from Reworkable Job Order*/
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
    });

}
function CheckValidations_forSubItems() {
    debugger
    //return Cmn_CheckValidations_forSubItems("GRN_ItmDetailsTbl", "SNo", "IDItemName", "ord_qty_spec", "SubItemOrderSpecQty", "Y");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

/**Added By Nitesh 30-11-2023 for check Gst Perameter**/
function checkprameter() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticPacking/checkorderqtymorethenpackingqty",
        data: {},
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            var arr = [];
            arr = JSON.parse(data);
            if (arr.Table.length > 0) {
                $("#hdn_checkpreameter").val(arr.Table[0].param_stat);
            }
        }
    })
}
/***--------------------------------Costing Detail Section-----------------------------------------***/
function OnClickSaveAndExit(OnAddGRN) {
    debugger;
    //var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);

    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;

    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    debugger;
    let NewArr = [];
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    // debugger;
    if ($("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text(); /* commented by Suraj on 01-02-2022 due to not in use*/
            //var DocNo = currentRow.find("#DocNo").text();
            //var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            }
            else {
                if (TaxItemID == TaxItmCode /*&& DocNo == GRNNo && DocDate == GRNDate*/) {
                    $(this).remove();
                }
                else {
                    var TaxName = currentRow.find("#TaxName").text();
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxAmount = currentRow.find("#TaxAmount").text();
                    var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                    var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                    var TaxRecov = currentRow.find("#TaxRecov").text();
                    NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov })
                }
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
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
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
                </tr>`);

            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: tax_recov })
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
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $("#" + HdnTaxCalculateTable + " tbody").append(`<tr id="R${++rowIdx}">
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
                </tr>`);

            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: tax_recov })
        });
    }
    if (TaxOn != "OC" && TaxOn != "") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    }
    else {
        $("#GRNItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#TxtItemId").val() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                let GrnQty = currentRow.find("#TxtGrnQty").val();
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    tax_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                    tax_non_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                }
                else {
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                }

                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                var itmocamt = currentRow.find("#Txt_other_charge").val();
                if (itmocamt != null && itmocamt != "") {
                    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValueInBase").val()))).toFixed(ValDecDigit);
                //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#TxtNetValueInBase").val(parseFloat(NetOrderValueBase).toFixed(ValDecDigit));
                let FinalNetOrderValueSpec = NetOrderValueBase / ConvRate
                currentRow.find("#TxtNetValue").val(parseFloat(FinalNetOrderValueSpec).toFixed(ValDecDigit));
                let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#Txt_item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });

        CalculateAmount();

    }

}

function BindTaxAmountDeatils(TaxAmtDetail) {
    //debugger;

    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
}
function CalculateAmount() {
    //debugger;
    //var DecDigit = $("#ValDigit").text();
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
    //var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    //var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var TaxNonRecov = parseFloat(0).toFixed(ValDecDigit);
    var TaxRecov = parseFloat(0).toFixed(ValDecDigit);
    var NetLandedValue = parseFloat(0).toFixed(ValDecDigit);


    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValue").val())) > 0) {

            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtGrnValue").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValueInBase").val())) > 0) {

            GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(currentRow.find("#TxtGrnValueInBase").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_non_rec_amt").val())) > 0) {

            TaxNonRecov = (parseFloat(TaxNonRecov) + parseFloat(currentRow.find("#Txt_item_tax_non_rec_amt").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_rec_amt").val())) > 0) {

            TaxRecov = (parseFloat(TaxRecov) + parseFloat(currentRow.find("#Txt_item_tax_rec_amt").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtNetValueInBase").val())) > 0) {

            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValueInBase").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtNetValue").val())) > 0) {

            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtLandedCost").val())) > 0) {

            NetLandedValue = (parseFloat(NetLandedValue) + parseFloat(currentRow.find("#TxtLandedCost").val())).toFixed(ValDecDigit);
        }
    });
    $("#grnGrossValue").val(GrossValue);
    $("#grnGrossValueInBase").val(GrossValueInBase);
    $("#grnTaxAmtNonRecoverable").val(TaxNonRecov);
    $("#grnTaxAmtRecoverable").val(TaxRecov);
    //$("#grnOtherCharges").val();
    $("#grnNetMRValue").val(NetOrderValueBase);
    $("#grnNetMRValueSpec").val(NetOrderValueSpec);
    $("#grnNetLandedValue").val(NetLandedValue);
};
function OnClickTaxCalculationBtn(e) {
    debugger;
    var POItemListName = "#GRNItmDetailsTbl";
    var SNohiddenfiled = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, POItemListName);
    debugger;
    if (GstApplicable == "Y") {
        if ($("#ForDisableOCDlt").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").css("display", "Block");
            }
            else {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").css("display", "none");
            }
        }
    }
}
function OnClickReplicateOnAllItems() {
    //var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;

    var ArrNonRecov = [];
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
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
        var tax_recov = currentRow.find("#tax_recov").text();
        TaxCalculationList.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: tax_recov })
        TaxCalculationListFinalList.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: tax_recov })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "GRNItmDetailsTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                var currentRow = $(this);
                var ItemCode;
                var AssessVal;
                let checkCurrForOC = "Y";/*To Check currency in case of Other Charge */
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                    OcCurrId = currentRow.find("#HdnOCCurrId").text();
                    BsCurrId = $("#hdbs_curr").val();
                    if (BsCurrId == OcCurrId) {

                    } else {
                        checkCurrForOC = "N";
                    }
                } else {
                    ItemCode = currentRow.find("#TxtItemId").val();
                    AssessVal = currentRow.find("#TxtGrnValueInBase").val();

                }
                if (checkCurrForOC == "Y") {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        TaxPercentage = TaxCalculationList[i].TaxPercentage;
                        var TaxLevel = TaxCalculationList[i].TaxLevel;
                        var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                        var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                        var TaxRecov = TaxCalculationList[i].TaxRecov;
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
                        NewArray.push({ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov })
                    }
                    if (NewArray != null) {
                        if (NewArray.length > 0) {
                            for (k = 0; k < NewArray.length; k++) {
                                var TaxName = NewArray[k].TaxName;
                                var TaxNameID = NewArray[k].TaxNameID;
                                var TaxItmCode = NewArray[k].TaxItmCode;
                                var TaxPercentage = NewArray[k].TaxPercentage;
                                var TaxLevel = NewArray[k].TaxLevel;
                                var TaxApplyOn = NewArray[k].TaxApplyOn;
                                var TaxAmount = NewArray[k].TaxAmount;
                                var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                                var TaxRecov = NewArray[k].TaxRecov;
                                if (CitmTaxItmCode != TaxItmCode) {
                                    TaxCalculationListFinalList.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov })
                                }
                                //if (CitmTaxItmCode == TaxItmCode) {
                                //    if (TaxOn != "OC") {
                                //        TaxCalculationListFinalList.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov})
                                //    }
                                //}

                            }
                        }
                    }
                }

            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();

    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        let ItmID = TaxCalculationListFinalList[i].TaxItmCode;
        let taxAmt = TaxCalculationListFinalList[i].TaxAmount;
        let tax_recov = TaxCalculationListFinalList[i].TaxRecov;
        tax_recov_amt = 0;
        tax_non_recov_amt = 0;
        if (ArrNonRecov.findIndex(v => v.ItemId == ItmID) > -1) {
            let getIndex = ArrNonRecov.findIndex(v => v.ItemId == ItmID);
            if (tax_recov == "Y") {
                ArrNonRecov[getIndex].tax_recov_amt = parseFloat(ArrNonRecov[getIndex].tax_recov_amt) + parseFloat(taxAmt);
            } else {
                ArrNonRecov[getIndex].tax_non_recov_amt = parseFloat(ArrNonRecov[getIndex].tax_non_recov_amt) + parseFloat(taxAmt);
            }
        } else {
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(taxAmt);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(taxAmt);
            }
            ArrNonRecov.push({ ItemId: ItmID, tax_recov_amt: tax_recov_amt, tax_non_recov_amt: tax_non_recov_amt });
        }

        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            
            <td id="DocNo"></td>
            <td id="DocDate"></td>
            <td id="TaxItmCode">${TaxCalculationListFinalList[i].TaxItmCode}</td>
            <td id="TaxName">${TaxCalculationListFinalList[i].TaxName}</td>
            <td id="TaxNameID">${TaxCalculationListFinalList[i].TaxNameID}</td>
            <td id="TaxPercentage">${TaxCalculationListFinalList[i].TaxPercentage}</td>
            <td id="TaxLevel">${TaxCalculationListFinalList[i].TaxLevel}</td>
            <td id="TaxApplyOn">${TaxCalculationListFinalList[i].TaxApplyOn}</td>
            <td id="TaxAmount">${TaxCalculationListFinalList[i].TaxAmount}</td>
            <td id="TotalTaxAmount">${TaxCalculationListFinalList[i].TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxCalculationListFinalList[i].TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxCalculationListFinalList[i].TaxRecov}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OCValue = currentRow.find("#OCValue").text();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
                        if (OCValue == TaxItmCode) {
                            currentRow.find("#OCTaxAmt").text(TotalTaxAmtF);
                            var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(ValDecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(ValDecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(ValDecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    }
    else {
        tax_recov_amt = 0;
        tax_non_recov_amt = 0;
        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);

            var ItemID = currentRow.find("#TxtItemId").val();
            var GrnQty = currentRow.find("#TxtGrnQty").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    let NewArrayTaxCal = TaxCalculationListFinalList.filter(v => v.TaxItmCode == ItemID);
                    for (i = 0; i < NewArrayTaxCal.length; i++) {
                        var TotalTaxAmtF = NewArrayTaxCal[i].TotalTaxAmount;
                        var AItemID = NewArrayTaxCal[i].TaxItmCode;

                        if (ItemID == AItemID) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                            if (ArrNonRecov.findIndex(v => v.ItemId == AItemID) > -1) {
                                let getIndex = ArrNonRecov.findIndex(v => v.ItemId == AItemID);
                                tax_recov_amt = ArrNonRecov[getIndex].tax_recov_amt;
                                tax_non_recov_amt = ArrNonRecov[getIndex].tax_non_recov_amt;
                            }
                            currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(ValDecDigit));
                            currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(ValDecDigit));
                            currentRow.find("#Txt_item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                            if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(ValDecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#TxtGrnValueInBase").val())).toFixed(ValDecDigit);
                            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
                            FinalNetOrderValueSpec = (NetOrderValueBase / ConvRate).toFixed(ValDecDigit);
                            currentRow.find("#TxtNetValue").val(FinalNetOrderValueSpec);
                            let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                            let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                            currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                            currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtGrnValueInBase").val()).toFixed(ValDecDigit);
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(ValDecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                    currentRow.find("#TxtNetValueInBase").val(FGrossAmt);
                    FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(ValDecDigit);
                    currentRow.find("#TxtNetValue").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtGrnValueInBase").val()).toFixed(ValDecDigit);
                currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(ValDecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValueInBase").val(FGrossAmt);
                FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValue").val(FinalGrossAmt);
            }
        });
        CalculateAmount();

    }

}

function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    //var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    let TaxNonRecovAmt = 0;
    let TaxRecovAmt = 0;
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {

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
                    //debugger;
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
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
        $("#GRNItmDetailsTbl > tbody > tr #TxtItemId[value=" + ItmCode + "]").closest('tr').each(function () {
            debugger;
            var currentRow = $(this);

            var ItemNo;

            ItemNo = currentRow.find("#TxtItemId").val();

            if (ItemNo == ItmCode) {
                var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr');
                if (FTaxDetailsItemWise.length > 0) {
                    FTaxDetailsItemWise.each(function () {
                        debugger;
                        var CRow = $(this);
                        var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                        var ItemTaxAmt = currentRow.find("#Txt_item_tax_amt").val()
                        if (currentRow.find("#TaxExempted").is(":checked")) {
                            TotalTaxAmtF = 0;
                            TaxNonRecovAmt = 0;
                            TaxRecovAmt = 0;
                        }
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                        var GstApplicable = $("#Hdn_GstApplicable").text();
                        if (GstApplicable == "Y") {
                            if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit);
                                /*---------Added by Suraj Maurya on 25-10-2024---------*/
                                TaxNonRecovAmt = 0;
                                TaxRecovAmt = 0;
                                /*---------Added by Suraj Maurya on 25-10-2024 End---------*/
                            }
                        }
                        var TaxItmCode = CRow.find("#TaxItmCode").text();
                        if (TaxItmCode == ItmCode) {
                            currentRow.find("#Txt_item_tax_amt").val(parseFloat(CheckNullNumber(TotalTaxAmtF)).toFixed(ValDecDigit));
                            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                            if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                                OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#Txt_other_charge").val())).toFixed(ValDecDigit);
                            }
                            AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValueInBase").val()))).toFixed(ValDecDigit);
                            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
                            FinalNetOrderValueSpec = NetOrderValueBase / ConvRate;
                            currentRow.find("#TxtNetValue").val(parseFloat(FinalNetOrderValueSpec).toFixed(ValDecDigit));
                            currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                            currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));

                            let GrnQty = currentRow.find("#TxtGrnQty").val();
                            let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                            let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                            currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                            currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
                            //}
                        }
                    });
                }
                else {
                    debugger;
                    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    var GrossAmtOR = parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValueInBase").val())).toFixed(ValDecDigit);
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                        OC_Amt_OR = parseFloat(CheckNullNumber(currentRow.find("#Txt_other_charge").val())).toFixed(ValDecDigit);
                    }
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                    currentRow.find("#TxtNetValueInBase").val(FGrossAmtOR);
                    let FinalFGrossAmtOR = FGrossAmtOR / ConvRate;
                    currentRow.find("#TxtNetValue").val(parseFloat(FinalFGrossAmtOR).toFixed(ValDecDigit));
                    let GrnQty = currentRow.find("#TxtGrnQty").val();
                    let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                    let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                    currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                    currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));

                }
            }
        });

    }
}
function AfterDeleteResetPO_ItemTaxDetail() {
    //debugger;
    var GRNItmDetailsTbl = "#GRNItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var POItemListName = "#POItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(GRNItmDetailsTbl, SNohiddenfiled, POItemListName);
}
function OnClickSaveAndExit_OC_Btn() {
    debugger;
    var NetOrderValueSpe = "#grnNetMRValueSpec";
    var NetOrderValueInBase = "#grnNetMRValue";
    var PO_otherChargeId = '#Tbl_OtherChargeList';

    CMNOnClickSaveAndExit_OC_Btn(PO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    //var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#grnGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    //var ng = TotalOCAmt < 0 ? "-" : "";
    //TotalOCAmt = Math.abs(TotalOCAmt);
    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);

        var GrossValue = currentRow.find("#TxtGrnValue").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) != 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWise) != 0) {
                currentRow.find("#Txt_other_charge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
            }
            else {
                currentRow.find("#Txt_other_charge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#Txt_other_charge").val(parseFloat(0).toFixed(ValDecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var OCValue = CheckNullNumber(currentRow.find("#Txt_other_charge").val());
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) != 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        Sno = 1;
        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#Txt_other_charge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno == "1") {
                currentRow.find("#Txt_other_charge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
                Sno++;
            }

        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        let Sno = 1;
        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#Txt_other_charge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno == "1") {
                currentRow.find("#Txt_other_charge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
                Sno++;
            }

        });
    }
    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtGrnValueInBase").val();
        var TaxNonRecov = currentRow.find("#Txt_item_tax_non_rec_amt").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#Txt_other_charge").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txt_item_tax_amt").val() != null && currentRow.find("#Txt_item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_amt").val())).toFixed(ValDecDigit);
        }
        //var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#TxtNetValueInBase").val((parseFloat(CheckNullNumber(POItm_NetOrderValueBase))).toFixed(ValDecDigit));
        FinalPOItm_NetOrderValueSpec = POItm_NetOrderValueBase / ConvRate;
        currentRow.find("#TxtNetValue").val((parseFloat(CheckNullNumber(FinalPOItm_NetOrderValueSpec))).toFixed(ValDecDigit));

        let GrnQty = currentRow.find("#TxtGrnQty").val();
        let LandedCostValue = parseFloat(CheckNullNumber(POItm_GrossValue)) + parseFloat(CheckNullNumber(POItm_OCAmt)) + parseFloat(CheckNullNumber(TaxNonRecov));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
    });
    CalculateAmount();
};
//function Calculate_OC_AmountItemWise(TotalOCAmt) {
//    //debugger;
//    var DecDigit = $("#ValDigit").text();
//    var TotalGAmt = $("#grnGrossValue").val();
//    var ConvRate = $("#conv_rate").val();
//    //var checkNegative = "N";
//    //if (parseFloat(CheckNullNumber(TotalOCAmt)) < 0) {
//    //    checkNegative = "N";
//    //}
//    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);

//        var GrossValue = currentRow.find("#TxtGrnValue").val();
//        if (GrossValue == "" || GrossValue == null) {
//            GrossValue = "0";
//        }
//        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
//            //debugger;
//            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
//            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
//            if (parseFloat(OCAmtItemWise) > 0) {
//                currentRow.find("#Txt_other_charge").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
//            }
//            else {
//                currentRow.find("#Txt_other_charge").val(parseFloat(0).toFixed(DecDigit));
//            }
//        }
//        if (parseFloat(TotalOCAmt) == 0) {
//            currentRow.find("#Txt_other_charge").val(parseFloat(0).toFixed(DecDigit));
//        }
//    });
//    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
//    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);
//        var OCValue = currentRow.find("#Txt_other_charge").val();
//        if (OCValue != null && OCValue != "") {
//            if (parseFloat(OCValue) > 0) {
//                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
//            }
//            else {
//                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
//            }
//        }
//    });
//    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
//        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
//        Sno = 1;
//        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
//            //debugger;
//            var currentRow = $(this);
//            //var Sno = currentRow.find("#SNohiddenfiled").val();
//            var OCValue = currentRow.find("#Txt_other_charge").val();
//            if (OCValue == "" || OCValue == null) {
//                OCValue = "0";
//            }
//            if (Sno == "1") {
//                currentRow.find("#Txt_other_charge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
//                Sno++;
//            }

//        });
//    }
//    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
//        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
//        let Sno = 1;
//        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
//            //debugger;
//            var currentRow = $(this);
//            //var Sno = currentRow.find("#SNohiddenfiled").val();
//            var OCValue = currentRow.find("#Txt_other_charge").val();
//            if (OCValue == "" || OCValue == null) {
//                OCValue = "0";
//            }
//            if (Sno == "1") {
//                currentRow.find("#Txt_other_charge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
//                Sno++;
//            }

//        });
//    }
//    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);
//        var POItm_GrossValue = currentRow.find("#TxtGrnValueInBase").val();
//        var TaxNonRecov = currentRow.find("#Txt_item_tax_non_rec_amt").val();
//        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
//        var POItm_OCAmt = currentRow.find("#Txt_other_charge").val();
//        if (POItm_GrossValue == null || POItm_GrossValue == "") {
//            POItm_GrossValue = "0";
//        }
//        if (POItm_OCAmt == null || POItm_OCAmt == "") {
//            POItm_OCAmt = "0";
//        }
//        if (currentRow.find("#Txt_item_tax_amt").val() != null && currentRow.find("#Txt_item_tax_amt").val() != "") {
//            POItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_amt").val())).toFixed(DecDigit);
//        }
//        //var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
//        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
//        currentRow.find("#TxtNetValueInBase").val((parseFloat(CheckNullNumber(POItm_NetOrderValueBase))).toFixed(DecDigit));
//        FinalPOItm_NetOrderValueSpec = POItm_NetOrderValueBase / ConvRate;
//        currentRow.find("#TxtNetValue").val((parseFloat(CheckNullNumber(FinalPOItm_NetOrderValueSpec))).toFixed(DecDigit));

//        let GrnQty = currentRow.find("#TxtGrnQty").val();
//        let LandedCostValue = parseFloat(CheckNullNumber(POItm_GrossValue)) + parseFloat(CheckNullNumber(POItm_OCAmt)) + parseFloat(CheckNullNumber(TaxNonRecov));
//        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
//        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(DecDigit));
//        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
//    });
//    CalculateAmount();
//};

function BindOtherChargeDeatils(val) {
    //var DecDigit = $("#ValDigit").text();

    //var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            //if (DocumentMenuId == "105101145") {
            td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            //}

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(ValDecDigit);
            // if (DocumentMenuId == "105101145") {
            TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
            TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
            //}
        });

    }
    $("#grnOtherCharges").val(TotalAMountWT);
    //if (DocumentMenuId == "105101145") {
    //$("#_OtherChargeTotalTax").text(TotalTaxAMount);
    //$("#_OtherChargeTotalAmt").text(TotalAMountWT);

    //}

}

function SetOtherChargeVal() {

}
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var ItmCode = currentrow.find("#TxtItemId").val();
    var AssAmount = currentrow.find("#TxtGrnValueInBase").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    //$("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "GRNItmDetailsTbl", "TxtItemId", "", "TxtGrnValueInBase")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnChangeItemPrice(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var ItmCode = currentrow.find("#TxtItemId").val();
    //let ValDigit = $("#ValDecDigit").text();
    //let RateDigit = $("#RateDigit").text();
    let ExchangeRate = $("#ExchangeRate").val();
    var TxtGrnQty = currentrow.find("#TxtGrnQty").val();
    var TxtRate = currentrow.find("#TxtRate").val();
    var Txt_item_tax_amt = currentrow.find("#Txt_item_tax_amt").val();
    var Txt_other_charge = currentrow.find("#Txt_other_charge").val();
    var TxtGRNValue = parseFloat(CheckNullNumber(TxtGrnQty)) * parseFloat(CheckNullNumber(TxtRate));
    var TxtNetValueInBase = parseFloat(TxtGRNValue) + parseFloat(CheckNullNumber(Txt_item_tax_amt)) + parseFloat(CheckNullNumber(Txt_other_charge));

    var AssAmount = currentrow.find("#TxtGrnValueInBase").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);

    //currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(RateDecDigit));
    //currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(5));
    currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(cmn_GrnRateDigit));
    if (parseFloat(CheckNullNumber(TxtRate)) <= 0) {
        currentrow.find("#TxtRate").css("border-color", "red");
        currentrow.find("#TxtRate_Error").css("display", "block");
        currentrow.find("#TxtRate_Error").text($("#valueReq").text());

    } else {
        currentrow.find("#TxtRate").css("border-color", "#ced4da");
        currentrow.find("#TxtRate_Error").css("display", "none");
        currentrow.find("#TxtRate_Error").text("");
    }
    currentrow.find("#TxtGrnValue").val(parseFloat(TxtGRNValue).toFixed(ValDecDigit));
    currentrow.find("#TxtGrnValueInBase").val(parseFloat(parseFloat(TxtGRNValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit));
    currentrow.find("#TxtNetValueInBase").val(parseFloat(TxtNetValueInBase).toFixed(ValDecDigit));
    currentrow.find("#TxtNetValue").val(parseFloat(TxtNetValueInBase / ExchangeRate).toFixed(ValDecDigit));

    if (hdn_ord_type == "I") {//Added by Suraj Maurya on 25-10-2024
        CalculateAmount();
        Calculate_OC_AmountItemWise($("#grnOtherCharges").val());
    }
    

    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        if (GstApplicable == "Y") {
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
        }
    }
    if (GstApplicable == "Y") {
        if (currentrow.find("#ManualGST").is(":checked")) {
            //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
            //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
        }
    }
    else {
        
        //$("#Tax_ItemID").val(ItmCode);
        //var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
       // OnChangeTaxTmpltDdl(ItmTmplt_id);
    }
    CalculateAmount();
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');

    var ItmCode = currentrow.find("#TxtItemId").val();
    var AssAmount = currentrow.find("#TxtGrnValueInBase").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        let GrnQty = currentrow.find("#TxtGrnQty").val();
        let OC_Amt = currentrow.find("#Txt_other_charge").val();
        let LandedCostValue = parseFloat(CheckNullNumber(AssAmount)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(0));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
        currentrow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
        DeleteItemTaxDetail(ItmCode);
        currentrow.find('#TaxExempted').prop("checked", false);
        currentrow.find('#BtnTxtCalculation').attr('disabled', false);
        CalculateAmount();
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        //var gst_number = $("#ship_add_gstNo").val()
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "GRNItmDetailsTbl", "TxtItemId", "", "TxtGrnValueInBase")
        OnChangeItemPrice(e)
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
function DeleteItemTaxDetail(ItemCode) {
    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItemCode + ")").closest("tr").each(function () {
        $(this).remove();
    });

}
function SaveCostingDetail() {
    debugger;
    let ErrorFlg = "N";
    if (parseFloat(CheckNullNumber($("#ExchangeRate").val())) === 0 || $("#ExchangeRate").val() == "") {
        $('#SpanExRateErrorMsg').text($("#ValueShouldBeGreaterThan0").text());
        $("#SpanExRateErrorMsg").css("display", "block");
        $("#ExchangeRate").css("border-color", "Red");
        ErrorFlg = "Y";
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#ExchangeRate").css("border-color", "#ced4da");
    }
    if (ValidateGrnAmountBeforeSubmit() == false) {
        return false;
    }
    if (ErrorFlg == "Y") {
        return false;
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
            $("#ddldeliverynoteno").attr("disabled", false);
            $("#SupplierName").attr("disabled", false);
            return true;
        }
       
    } else {
        $("#GRNItmDetailsTbl >tbody >tr").each(function () {
            var currentrow = $(this);
            var TxtRate = currentrow.find("#TxtRate").val();
            if (parseFloat(CheckNullNumber(TxtRate)) <= 0) {
                currentrow.find("#TxtRate").css("border-color", "red");
                currentrow.find("#TxtRate_Error").css("display", "block");
                currentrow.find("#TxtRate_Error").text($("#valueReq").text());
                ErrorFlg = "Y";
            } else {
                currentrow.find("#TxtRate").css("border-color", "#ced4da");
                currentrow.find("#TxtRate_Error").css("display", "none");
                currentrow.find("#TxtRate_Error").text("");
            }
            var hsn_no = currentrow.find("#hsn_no").val();
            if (hsn_no == null || hsn_no == "0" || hsn_no=="") {
                currentrow.find("#span_hsn_no").text($("#valueReq").text());
                currentrow.find("#span_hsn_no").css("display", "block");
                currentrow.find('[aria-labelledby="select2-hsn_no-container"]').css("border", "1px solid red");
                ErrorFlg = "Y";
            } else {
                currentrow.find("#span_hsn_no").css("display", "none");
                currentrow.find('[aria-labelledby="select2-hsn_no-container"]').css("border", "1px solid #ced4da");
            }
        })
        if (ErrorFlg == "Y") {
            return false;
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("GRNItmDetailsTbl", "Txt_item_tax_amt", "TxtItemId", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
                return false;
            }
        }
        let FinalItemDetail = CostingDetailsItemList();
        let FinalTaxDetail = InsertTaxDetails();
        let FinalOCTaxDetail = InsertOCTaxDetails();
        let FinalOCDetail = InsertOtherChargeDetails();

        $("#CostingDetailItmDt").val(JSON.stringify(FinalItemDetail));
        $("#CostingDetailItmTaxDt").val(JSON.stringify(FinalTaxDetail));
        $("#CostingDetailItmOCTaxDt").val(JSON.stringify(FinalOCTaxDetail));
        $("#CostingDetailOcDt").val(JSON.stringify(FinalOCDetail));

        $("#ddldeliverynoteno").attr("disabled", false);
        $("#SupplierName").attr("disabled", false);
        return true;
    }

}
function onchangehsn_no(e) {
    var currentrow = $(e.target).closest('tr');
    var hsn_no = currentrow.find("#hsn_no").val();
    if (hsn_no == null || hsn_no == "0" || hsn_no == "") {
        currentrow.find("#span_hsn_no").text($("#valueReq").text());
        currentrow.find("#span_hsn_no").css("display", "block");
        currentrow.find('[aria-labelledby="select2-hsn_no-container"]').css("border", "1px solid red");
        ErrorFlg = "Y";
    } else {
        currentrow.find("#span_hsn_no").css("display", "none");
        currentrow.find('[aria-labelledby="select2-hsn_no-container"]').css("border", "1px solid #ced4da");
    }
}
function CostingDetailsItemList() {
    var GrnItemList = [];
    $("#GRNItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        let ItemID = "";
        let UOMID = "";
        let GrnQty = "";
        let ItmRate = "";
        let hsn_code = "";
        let GrossVal = "";
        let GrossValBase = "";
        let TaxAmt = "";
        let OCAmt = "";
        let NetValSpec = "";
        let NetValBase = "";
        let TaxExempted = "";
        let ManualGST = "";
        let TaxRecovAmt = "";
        let TaxNonRecovAmt = "";
        let LandedCostValue = "";
        let LandedCostPerPc = "";
        let currentRow = $(this);

        ItemID = currentRow.find("#TxtItemId").val();
        //hsn_code = currentRow.find("#ItemHsnCode").val();
        hsn_code = currentRow.find("#hsn_no").val();
        UOMID = currentRow.find("#TxtUomId").val();
        GrnQty = currentRow.find("#TxtGrnQty").val();
        ItmRate = currentRow.find("#TxtRate").val();
        GrossVal = currentRow.find("#TxtGrnValue").val();
        GrossValBase = currentRow.find("#TxtGrnValueInBase").val();
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_amt").val())) > 0) {
            TaxAmt = currentRow.find("#Txt_item_tax_amt").val();
        }
        else {
            TaxAmt = "0";
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_other_charge").val())) != 0) {
            OCAmt = currentRow.find("#Txt_other_charge").val();
        }
        else {
            OCAmt = "0";
        }
        TaxRecovAmt = currentRow.find("#Txt_item_tax_rec_amt").val();
        TaxNonRecovAmt = currentRow.find("#Txt_item_tax_non_rec_amt").val();
        NetValSpec = currentRow.find("#TxtNetValue").val();
        NetValBase = currentRow.find("#TxtNetValueInBase").val();
        LandedCostValue = currentRow.find("#TxtLandedCost").val();
        LandedCostPerPc = currentRow.find("#TxtLandedCostPerPc").val();
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y"
            }
            else {
                ManualGST = "N"
            }
        }
        GrnItemList.push({
            ItemID: ItemID, hsn_code: hsn_code, UOMID: UOMID, GrnQty: GrnQty, ItmRate: ItmRate, GrossVal: GrossVal
            , GrossValBase: GrossValBase
            , TaxAmt: TaxAmt, TaxRecovAmt: TaxRecovAmt, TaxNonRecovAmt: TaxNonRecovAmt, OCAmt: OCAmt
            , NetValSpec: NetValSpec, NetValBase: NetValBase
            , TaxExempted: TaxExempted, ManualGST: ManualGST, LandedCostPerPc: LandedCostPerPc, LandedCostValue: LandedCostValue
        });
    });
    return GrnItemList;
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxList = [];
    $("#GRNItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var ItmCode = currentRow.find("#TxtItemId").val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                        var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
                        var TaxRecov = Crow.find("#TaxRecov").text().trim();
                        TaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount, TaxRecov: TaxRecov });
                    });
                }
            }
        }
    });
    return TaxList;
};
function InsertOCTaxDetails() {
    debugger;
    var TaxList = [];
    $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").closest("tr").each(function () {
        var Crow = $(this);
        var ItemID = Crow.find("#TaxItmCode").text().trim();
        var TaxID = Crow.find("#TaxNameID").text().trim();
        var TaxName = Crow.find("#TaxName").text().trim();
        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
        var TaxLevel = Crow.find("#TaxLevel").text().trim();
        var TaxValue = Crow.find("#TaxAmount").text().trim();
        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
        var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
        var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
        TaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });
    });

    return TaxList;
};
function InsertOtherChargeDetails() {
    debugger;
    let OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_SuppName = "";
            var OC_SuppId = "";
            var OC_SuppType = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Curr_Id = currentRow.find("#HdnOCCurrId").text();
            OC_SuppName = currentRow.find("#td_OCSuppName").text();
            OC_SuppId = currentRow.find("#td_OCSuppID").text();
            OC_SuppType = currentRow.find("#td_OCSuppType").text();//Added by Suraj on 11-04-2024
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();

            OC_TaxAmt = currentRow.find("#OCTaxAmt").text();
            OC_TotlAmt = currentRow.find("#OCTotalTaxAmt").text();
            OC_BillNo = currentRow.find("#OCBillNo").text();
            OC_BillDate = currentRow.find("#OCBillDt").text();
            OCList.push({
                OC_ID: OC_ID, OCValue: OCValue, OCName: OCName, OC_Curr: OC_Curr, OC_Curr_Id: OC_Curr_Id, OC_Conv: OC_Conv
                , OC_AmtBs: OC_AmtBs, OC_TaxAmt: OC_TaxAmt, OC_TotlAmt: OC_TotlAmt, OC_SuppName: OC_SuppName
                , OC_SuppId: OC_SuppId, OC_SuppType: OC_SuppType, OC_BillNo: OC_BillNo, OC_BillDate: OC_BillDate
            });
        });
    }
    return OCList;
};
function OnClickOCTaxCalculationBtn(e) {
    debugger;
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";

    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
/***--------------------------------Costing Detail Section End-----------------------------------------***/

/****======------------Reason For Reject And Rework section Start-------------======****/
function onClickReasonRemarks(e, flag) {
    let Row = $(e.target).closest('tr');
    let Sno = Row.find("#HdnSrNo").val();
    let ItemName = Row.find("#IDItemName" + Sno).val();
    let ItemId = Row.find("#hiddenfiledItemID" + Sno).val();
    let UOM = Row.find("#idUOM" + Sno).val();
    let ReasonRemarks = "";

    $("#RR_Remarks").attr("readonly", true);
    $("#RR_btnClose").attr("hidden", false);
    $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").text();
        let rejQty = Row.find("#RejectedQuantity" + Sno).val();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);
        } else {
            Row.find("#ReasonForReject").text("");
            ReasonRemarks = "";
        }
    }
    else if (flag == "rework") {
        ReasonRemarks = Row.find("#ReasonForRework").text();

        let rwkQty = Row.find("#ReworkQuantity" + Sno).val();
        if (parseFloat(CheckNullNumber(rwkQty)) > 0) {
            $("#RR_Quantity").val(rwkQty);
        } else {
            Row.find("#ReasonForRework").text("");
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

function OnChangeExchangeRate() {
    debugger
    let ExchangeRate = $("#ExchangeRate").val();
    if (parseFloat(ExchangeRate) == 0) {
        $('#SpanExRateErrorMsg').text($("#ValueShouldBeGreaterThan0").text());
        $("#SpanExRateErrorMsg").css("display", "block");
        $("#ExchangeRate").css("border-color", "Red");
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#ExchangeRate").css("border-color", "#ced4da");
    }
    $("#conv_rate").val(parseFloat(CheckNullNumber(ExchangeRate)).toFixed(ExchDecDigit));
    $("#ExchangeRate").val(parseFloat(CheckNullNumber(ExchangeRate)).toFixed(ExchDecDigit));
    $("#GRNItmDetailsTbl tbody tr").each(function () {
        let row = $(this);

        row.find("#TxtRate").change();

        //var AssAmount = row.find("#TxtGrnValueInBase").val();
        //var ItmCode = row.find("#TxtItemId").val();

        //$("#HdnTaxOn").val("Item");
        //$("#TaxCalcItemCode").val(ItmCode);
        //$("#Tax_AssessableValue").val(AssAmount);

        //var ItmTmplt_id = currentrow1.find("#ItemtaxTmplt").val();
        //OnChangeTaxTmpltDdl(ItmTmplt_id);
    });

    let CurrId = $("#CurrId").val();
    $("#Tbl_OC_Deatils tbody tr").each(function () {
        let row = $(this);
        let td_currId = row.find("#HdnOCCurrId").text();
        if (CurrId == td_currId) {
            row.find("#OCConv").text(parseFloat(CheckNullNumber(ExchangeRate)).toFixed(ExchDecDigit));
            let OCConv = row.find("#OCConv").text();
            let OCAmount = row.find("#OCAmount").text();
            let OCValueInBase = parseFloat(CheckNullNumber(OCConv)) * parseFloat(CheckNullNumber(OCAmount));
            row.find("#OcAmtBs").text(OCValueInBase.toFixed(ValDecDigit));
            let OCTaxAmt = row.find("#OCTaxAmt").text();
            let totalValue = parseFloat(CheckNullNumber(OCValueInBase)) + parseFloat(CheckNullNumber(OCTaxAmt));
            row.find("#OCTotalTaxAmt").text(totalValue.toFixed(ValDecDigit));
        }
    });
    //$("#GRNItmDetailsTbl tbody tr").each(function () {
    //    let row1 = $(this);

    //    var AssAmount = row1.find("#TxtGrnValueInBase").val();
    //    var ItmCode = row1.find("#TxtItemId").val();

    //    $("#HdnTaxOn").val("Item");
    //    $("#TaxCalcItemCode").val(ItmCode);
    //    $("#Tax_AssessableValue").val(AssAmount);

    //    var ItmTmplt_id = currentrow1.find("#ItemtaxTmplt").val();
    //    OnChangeTaxTmpltDdl(ItmTmplt_id);
    //});
    OnClickSaveAndExit_OC_Btn();
    //   let CurrId = $("#CurrId").val();
    //    $("#ht_Tbl_OC_Deatils tbody tr").each(function () {
    //        let row = $(this);
    //        let td_currId = row.find("#HdnOC_CurrId").text();
    //        if (CurrId == td_currId) {
    //            row.find("#OC_Conv").text(parseFloat(CheckNullNumber(ExchangeRate)).toFixed(RateDecDigit));
    //            let OCConv = row.find("#OC_Conv").text();

    //            let OCValue = row.find("#OCAmtSp").text();
    //            let OCValueInBase = parseFloat(CheckNullNumber(OCConv)) * parseFloat(CheckNullNumber(OCValue));
    //            row.find("#OCAmtBs").text(OCValueInBase.toFixed(ValDecDigit));
    //            let OcAmtBs = row.find("#OCAmtBs").text();
    //            let OCTaxAmt = row.find("#OCTaxAmt").text();
    //            let total = parseFloat(CheckNullNumber(OcAmtBs)) + parseFloat(CheckNullNumber(OCTaxAmt));
    //            row.find("#OCTotalTaxAmt").text(total.toFixed(ValDecDigit));

    //        }


    //    });
}
function CheckOnlyRateValue(el, evt) {
    let RateDigit = hdn_ord_type == "I" ? "#ExpImpRateDigit" : "#RateDigit";
    if (Cmn_FloatValueonly(el, evt, RateDigit) == false) {
        return false;
    }
}
function CheckOnlyExchRateValue(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
}
function CheckOnlyFloatValue(el, evt) {
    let ValDigit = hdn_ord_type == "I" ? "#ExpImpValDigit" : "#ValDigit";
    if (Cmn_FloatValueonly(el, evt, ValDigit) == false) {
        return false;
    }
}

function OnChangeGrnValue(e) {
    
    var currentrow = $(e.target).closest('tr');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var ItmCode = currentrow.find("#TxtItemId").val();
    let ExchangeRate = $("#ExchangeRate").val();
    var TxtGrnQty = currentrow.find("#TxtGrnQty").val();
    //var TxtRate = currentrow.find("#TxtRate").val();
    var TxtGRNValue = CheckNullNumber(currentrow.find("#TxtGrnValue").val());
    if (parseFloat(CheckNullNumber(TxtGRNValue)) <= 0) {
        currentrow.find("#TxtGrnValue").css("border-color", "red");
        currentrow.find("#TxtGrnValue_Error").css("display", "block");
        currentrow.find("#TxtGrnValue_Error").text($("#valueReq").text());

    } else {
        currentrow.find("#TxtGrnValue").css("border-color", "#ced4da");
        currentrow.find("#TxtGrnValue_Error").css("display", "none");
        currentrow.find("#TxtGrnValue_Error").text("");
    }
    currentrow.find("#TxtGrnValue").val(parseFloat(TxtGRNValue).toFixed(ValDecDigit));
    currentrow.find("#TxtGrnValueInBase").val(parseFloat(parseFloat(TxtGRNValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit));
    var TxtRate = parseFloat(CheckNullNumber(TxtGRNValue)) / parseFloat(CheckNullNumber(TxtGrnQty));
    //currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(RateDecDigit));
    currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(2));

    if (parseFloat(CheckNullNumber(TxtRate)) <= 0) {
        currentrow.find("#TxtRate").css("border-color", "red");
        currentrow.find("#TxtRate_Error").css("display", "block");
        currentrow.find("#TxtRate_Error").text($("#valueReq").text());

    } else {
        currentrow.find("#TxtRate").css("border-color", "#ced4da");
        currentrow.find("#TxtRate_Error").css("display", "none");
        currentrow.find("#TxtRate_Error").text("");
    }

    //var Txt_item_tax_amt = currentrow.find("#Txt_item_tax_amt").val();
    //var Txt_other_charge = currentrow.find("#Txt_other_charge").val();
    //var TxtNetValueInBase = parseFloat(parseFloat(parseFloat(TxtGRNValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit)) + parseFloat(CheckNullNumber(Txt_item_tax_amt)) + parseFloat(CheckNullNumber(Txt_other_charge));
    //currentrow.find("#TxtNetValueInBase").val(parseFloat(TxtNetValueInBase).toFixed(ValDecDigit));
    //currentrow.find("#TxtNetValue").val(parseFloat(TxtNetValueInBase / ExchangeRate).toFixed(ValDecDigit));

    if (hdn_ord_type != "I") {
        var AssAmount = currentrow.find("#TxtGrnValueInBase").val();
        $("#HdnTaxOn").val("Item");
        $("#TaxCalcItemCode").val(ItmCode);
        $("#Tax_AssessableValue").val(AssAmount);

        if (currentrow.find("#TaxExempted").is(":checked")) {
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
            currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            if (GstApplicable == "Y") {
                CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
            }
        }
        if (GstApplicable == "Y") {
            if (currentrow.find("#ManualGST").is(":checked")) {
                //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
                CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
                //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            }
            else {
                CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValueInBase").val());
            }
        }
        else {

        }
    } else {
        CalculateAmount();
        Calculate_OC_AmountItemWise($("#grnOtherCharges").val());
        //let LandedCostValue = parseFloat(parseFloat(parseFloat(TxtGRNValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit)) + parseFloat(CheckNullNumber(Txt_other_charge));
        //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(TxtGrnQty));
        //currentrow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        //currentrow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
    }
    
    
}

/*code wrote by Suraj on 25-10-2024 */
function ValidateGrnAmountBeforeSubmit() {

    var grValue = $('#grnGrossValueInBase').val();
    var taxAmt = parseFloat(CheckNullNumber($('#grnTaxAmtRecoverable').val())) + parseFloat(CheckNullNumber($('#grnTaxAmtNonRecoverable').val()));//Non-Recoverable tax Added by Suraj Maurya on 22-11-2024
    var oc_Amount = $('#grnOtherCharges').val();
    if (hdn_ord_type == "I") {
        var netAmt = parseFloat(CheckNullNumber($('#grnNetMRValue').val())).toFixed(2);
        var calculatedAmt = parseFloat(parseFloat(CheckNullNumber(grValue)) + parseFloat(CheckNullNumber(taxAmt)) + parseFloat(CheckNullNumber(oc_Amount))).toFixed(2);

    } else {
        var netAmt = parseFloat(CheckNullNumber($('#grnNetMRValue').val())).toFixed(ValDecDigit);
        var calculatedAmt = parseFloat(parseFloat(CheckNullNumber(grValue)) + parseFloat(CheckNullNumber(taxAmt)) + parseFloat(CheckNullNumber(oc_Amount))).toFixed(ValDecDigit);

    }
    if (netAmt != calculatedAmt) {
        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    return true;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "GRN_ItmDetailsTbl", [{ "FieldId": "IDItemName", "FieldType": "input", "SrNo": "HdnSrNo"  }]);
}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}

function onchangeCancelledRemarks() {
    debugger;
  
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
async function OnChangeBillNo() {
    const Bill_No = $("#BillNumber").val();
    const Bill_Date = $("#BillDate").val();
    const DocumentMenuId = $("#DocumentMenuId").val();
    const supp_id = $("#SupplierName").val();
    const hdn_Bill_No = $("#hdn_Bill_No").val();
    const hdn_Bill_Dt = $("#hdsrc_doc_date1").val();

    const showError = (id, msgId) => {
        $(id).css("border-color", "Red");
        $(msgId).text($("#valueduplicate").text()).css("display", "block");
    };
    const clearError = (id, msgId) => {
        $(id).css("border-color", "#ced4da");
        $(msgId).css("display", "none");
    };
    if (!Bill_No) {
        $("#BillNumber").css("border-color", "Red");
        $('#billNoErrorMsg').text($("#valueReq").text()).css("display", "block");
        return;
    } else {
        clearError("#BillNumber", "#billNoErrorMsg");
    }
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

            if (result === "Duplicate") {
                if (hdn_Bill_No !== Bill_No) {
                    showError("#BillNumber", "#billNoErrorMsg");
                    showError("#BillDate", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else if (hdn_Bill_Dt !== Bill_Date) {
                    showError("#BillNumber", "#billNoErrorMsg");
                    showError("#BillDate", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else {
                    clearError("#BillNumber", "#billNoErrorMsg");
                    clearError("#BillDate", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("N");
                }
            } else {
                clearError("#BillNumber", "#billNoErrorMsg");
                clearError("#BillDate", "#BillDateErrorMsg");
                $("#duplicateBillNo").val("N");
            }
        }
    });
}
function OnChangeBillDt() {
    if ($("#BillDate").val() == "") {
        $('#BillDateErrorMsg').text($("#valueReq").text());
        $("#BillDateErrorMsg").css("display", "block");
        $("#BillDate").css("border-color", "Red");
    }
    else {
        $("#BillDateErrorMsg").css("display", "none");
        $("#BillDate").css("border-color", "#ced4da");
    }
    OnChangeBillNo();
}
function onclickReplicateWithAllItems() {
    $("#Replicat_wh_id_Error").css("display", "none");
    $("#Replicat_wh_id").css("border-color", "#ced4da");
    $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    BindWarehouseList("Replicate");
}
function OnChangeReplicatWarehouse() {
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickSaveAndExitReplicateBtn() {
    debugger;
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        $("#SaveExitReplicateBtn").attr("data-dismiss", "");
        return false;
    }
    $("#GRN_ItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var SNo = currentRow.find("#SNo").text();       
        currentRow.find("#wh_id" + SNo).val(wh_id).prop('selected', true);
        //row.find("#wh_id" + rowNo).val(wh_id).trigger("change");
    });
   
    $("#SaveExitReplicateBtn").attr("data-dismiss", "modal");
}