const QtyDecDigit = $("#QtyDigit").text();///Quantity
const RateDecDigit = $("#RateDigit").text();///Rate And Percentage
const ValDecDigit = $("#ValDigit").text();///Amount
var LddCostDecDigit = 5;///For Landed Cost 
$(document).ready(function () {
    debugger;
    $("#SupplierName").select2();
    $("#DPIListSupplier").select2();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#InvoiceNumber").val() == "" || $("#InvoiceNumber").val() == null) {
        $("#Inv_date").val(CurrentDate);
    }
    if ($("#hfPIStatus").val() == "D" || $("#hfPIStatus").val() == "") {
        BindPOItmList(1)
        //BindWarehouseList(1);
    }
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        //var DocumentMenuId = $("#DocumentMenuId").val();
        //var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        if (InvoiceNo != "" && InvoiceNo != null) {
            location.href = "/ApplicationLayer/DirectPurchaseInvoice/AddDirectPurchaseInvoiceDetail/?DocNo=" + InvoiceNo + "&DocDate=" + InvDate + "&ListFilterData=" + ListFilterData;//+ "&WF_status=" + WF_status;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(InvoiceNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(InvoiceNo, InvDate, Doc_id, Doc_Status);
    });
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    $('#POInvItmDetailsTbl tbody').on('click', '.delBtnIconDPI', function () {
        debugger;
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
        $(this).closest('tr').remove();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#hfItemID").val();
        CalculateAmount();
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        DeleteItemBatchSerialOrderQtyDetails(ItemCode);
        var TotalOCAmt = parseFloat($("#TxtOtherCharges").val());
        if (TotalOCAmt == "" || TotalOCAmt == null || TotalOCAmt == "NaN") {
            TotalOCAmt = 0;
        }
        Calculate_OC_AmountItemWise(TotalOCAmt)
        AfterDeleteResetPO_ItemTaxDetail();
        BindOtherChargeDeatils();
        SerialNoAfterDelete();
        var ToTdsAmt_IT = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        var ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
        ResetTDS_CalOnchangeDocDetail(ToTdsAmt, "#TxtTDS_Amount", ToTdsAmt_IT);
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                CalculateVoucherTotalAmount();

            });
        }
        else {
            GetAllGLID();
        }
    });
    var InvoiceNo = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvoiceNo);

    CancelledRemarks("#Cancelled", "Disabled");

    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#POInvItmDetailsTbl > tbody > tr').each(function () {

            var cellText = $(this).find('#hfItemID').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
    }
});
function SetOtherChargeVal() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
    })
}
function BtnSearch() {
    //debugger;
    FilterDPI_List();
    ResetWF_Level();
}
function FilterDPI_List() {
    try {
        var SuppId = $("#DPIListSupplier option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DirectPurchaseInvoice/DPIListSearch",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                $('#DPIList').html(data);
                $("#ListFilterData").val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status)
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        console.log("PI Error : " + err.message);
    }
}
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
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
async function AutoTdsApply(SuppId, GrossAmt) {
    var CustomTds = $('#Hdn_TDS_CalculatorTbl > tbody > tr #td_TDS_AssValApplyOn:contains("CA")').closest('tr');
    if (CustomTds.length == 0) {/* Added by Suraj Maurya on 14-05-2025 */
        await $.ajax({
            type: "POST",
            url: "/Common/Common/Cmn_GetTdsDetails",
            data: { SuppId: SuppId, GrossVal: GrossAmt },
            success: function (data) {
                debugger;
                var arr = JSON.parse(data);
                let tds_amt = 0;
                if (arr.Table1 != null) {
                    let checkResult = arr.Table1.length > 0 ? arr.Table1[0].result : "";
                    $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
                    if (checkResult == "Invalid Slab") {
                        swal("", $("#InvailidTdsSlabFound").text(), "warning")
                    } else {
                        var checkTdsAcc = "Y";
                        $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table1.length; i++) {
                            //let td_tds_amt = Math.round(arr.Table1[i].tds_amt);/* Commented by Suraj Maurya on 03-04-2025 */
                            let td_tds_amt = Cmn_RoundValue(arr.Table1[i].tds_amt, "P");
                            if (arr.Table1[i].tds_id == "") {
                                checkTdsAcc = "N";
                            }
                            if (checkTdsAcc == "Y") {
                                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${i + 1}">
                                <td id="td_TDS_Name">${arr.Table1[i].tds_name}</td>
                                <td id="td_TDS_NameID">${arr.Table1[i].tds_id}</td>
                                <td id="td_TDS_Percentage">${arr.Table1[i].tds_perc}</td>
                                <td id="td_TDS_Level">${arr.Table1[i].tds_level}</td>
                                <td id="td_TDS_ApplyOn">${arr.Table1[i].tds_apply_on}</td>
                                <td id="td_TDS_Amount">${td_tds_amt}</td>
                                <td id="td_TDS_ApplyOnID">${arr.Table1[i].tds_apply_on_id}</td>
                                <td id="td_TDS_BaseAmt">${arr.Table1[i].tds_bs_amt}</td>
                                <td id="td_TDS_AccId">${arr.Table1[i].tds_acc_id}</td>
                                    </tr>`);
                                tds_amt += parseFloat(td_tds_amt);
                            }
                            $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
                        }
                        if (checkTdsAcc == "N") {
                            $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
                            $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
                            swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
                        }
                    }
                }
                GetAllGLID();
            }
        });
    }
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertDPIDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohiddenfiled", "BindData", "105101154");
}
function BindData() {
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + rowid).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
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
        if (AccID != '0' && AccID != "") {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
function OnChangeAccountName(RowID, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var Acc_Name = clickedrow.find("#Acc_name_" + SNo + " option:selected").text();
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
        $("#POInvItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
            var row = $(this);
            row.find("#hdn_item_gl_acc").val(Acc_ID);
        });
    }
    if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }

    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
    $("#hdnAccID").val(Acc_ID);
}
function InsertDPIDetails() {
    debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        var INSDTransType = sessionStorage.getItem("INSTransType");
        if ($("#duplicateBillNo").val() == "Y") {
            $("#Bill_No").css("border-color", "Red");
            $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
            $("#SpanBillNoErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanBillNoErrorMsg").css("display", "none");
            $("#Bill_No").css("border-color", "#ced4da");
        }
        if (CheckDPI_Validations() == false) {
            return false;
        }
        if (CheckDPI_ItemValidations("N") == false) {
            return false;
        }
        let NetOrderValueInBase = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        if (NetOrderValueInBase > 0) {

        }
        else {
            return false;
        }
        if (CheckDPI_VoucherValidations() == false) {
            return false;
        }
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
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("POInvItmDetailsTbl", "item_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "POItemListName") == false) {
                return false;
            }
        }
        let GrossAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
        let TxtTaxAmount = parseFloat(CheckNullNumber($("#TxtTaxAmount").val()));
        let TxtDocSuppOtherCharges = parseFloat(CheckNullNumber($("#TxtDocSuppOtherCharges").val()));
        var tt = (GrossAmt + TxtTaxAmount + TxtDocSuppOtherCharges);
        var diffranceAmt = Math.abs(NetOrderValueInBase - tt)
        //if (NetOrderValueInBase != (GrossAmt + TxtTaxAmount + TxtDocSuppOtherCharges)) {
        if (diffranceAmt > 1) {
            swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
            return false;
        }
        if (NetOrderValueInBase > 0) {

        }
        else {
            return false;
        }
        //let TxtTDS_Amount = parseFloat(CheckNullNumber($("#TxtTDS_Amount").val()));
        //let TxtOtherCharges = parseFloat(CheckNullNumber($("#TxtOtherCharges").val()));
        //let totGlCrTotalInBase = parseFloat(CheckNullNumber($("#CrTotalInBase").text()));
        //if (totGlCrTotalInBase != (GrossAmt + TxtTaxAmount + TxtOtherCharges + TxtTDS_Amount)) {
        //    swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        //    return false;
        //}

        $("#SupplierName").attr("disabled", false);

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
            Cancelled = "Y";
        }
        else {
            Cancelled = "N";
        }
        var Narration = $("#DebitNoteRaisedAgainstInv").text()
        $('#hdNarration').val(Narration);

        var FinalItemDetail = [];
        FinalItemDetail = InsertDPIItemDetails();
        var str = JSON.stringify(FinalItemDetail);
        $('#hdItemDetailList').val(str);

        var TaxDetail = [];
        TaxDetail = InsertTaxDetails();
        var str_TaxDetail = JSON.stringify(TaxDetail);
        $('#hdItemTaxDetail').val(str_TaxDetail);

        var OC_TaxDetail = [];
        OC_TaxDetail = OC_InsertTaxDetails();
        var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
        $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

        var OCDetail = [];
        OCDetail = GetPI_OtherChargeDetails();
        var str_OCDetail = JSON.stringify(OCDetail);
        $('#hdItemOCDetail').val(str_OCDetail);

        var vou_Detail = [];
        vou_Detail = GetDPI_VoucherDetails();
        var str_vou_Detail = JSON.stringify(vou_Detail);
        $('#hdItemvouDetail').val(str_vou_Detail);

        var SubItemsListArr = Cmn_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#hdnSubItemDetails').val(str2);

        var FinalCostCntrDetails = [];
        FinalCostCntrDetails = Cmn_InsertCCDetails();
        var CCDetails = JSON.stringify(FinalCostCntrDetails);
        $('#hdn_CC_DetailList').val(CCDetails);

        var TdsDetail = [];
        TdsDetail = InsertTdsDetails();
        var str_TdsDetail = JSON.stringify(TdsDetail);
        $('#hdn_tds_details').val(str_TdsDetail);

        var Final_OC_TdsDetails = [];
        Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
        var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);

        SaveBatchItemDeatil();
        SaveItemSerialItemDeatil();

        $('#hdn_oc_tds_details').val(Oc_Tds_Details);
        try {
            if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramtInBase1", "cramtInBase1","hfPIStatus") == false) {
                return false;
            }
        }
        catch (ex) {
            console.log(ex);
            return false;
        }
        
        $("#RCMApplicable").prop("disabled", false);
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_PInvSuppName").val(Suppname);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    
}
function CheckDPI_VoucherValidations() {
    debugger;
    if (Cmn_CheckGlVoucherValidations() == false) {
        return false;
    } else {
        return true;
    }
}
function Check_subitemValidation() {
    return Cmn_CheckValidations_forSubItems("POInvItmDetailsTbl", "", "hfItemID", "ord_qty_spec", "SubItemDPIQty", "Y");
}
function InsertDPIItemDetails() {
    var PI_ItemsDetail = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var TaxExempted = "";
        var ManualGST = "";
        var ClaimITC = "";

        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var srno = currentRow.find("#srno").text();
        var item_id = currentRow.find("#hfItemID").val();
        var item_name = currentRow.find("#POItemListName" + Sno+" option:selected").text();
        var subitem = currentRow.find("#sub_item").val();
        currentRow.find("#ItemType").attr('disabled', false);
        var ItemType = currentRow.find("#ItemType").val();
        var uom_id = IsNull(currentRow.find("#UOM").val(), 0);
        var uom_name = IsNull(currentRow.find("#UOM").text(),0);
        var inv_qty = currentRow.find("#ord_qty_spec").val();
        var item_rate = currentRow.find("#item_rate").val();
        var item_disc_perc = currentRow.find("#item_disc_perc").val();
        var item_disc_amt = currentRow.find("#item_disc_amt").val();
        var item_disc_val = currentRow.find("#item_disc_val").val();
        var item_gr_val = currentRow.find("#item_gr_val").val();
        var item_gr_val_bs = currentRow.find("#item_gr_val").val();
        var tax_amt = currentRow.find("#item_tax_amt").val();
        var tax_amt_nrecov = currentRow.find("#Txt_item_tax_non_rec_amt").val();
        var tax_amt_recov = currentRow.find("#Txt_item_tax_rec_amt").val();
        var hsn_code = currentRow.find("#ItemHsnCode").val();
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        if (tax_amt == "" && tax_amt == null) {
            tax_amt = "0";
        }
        if (tax_amt_nrecov == "" && tax_amt_nrecov == null) {
            tax_amt_nrecov = "0";
        }
        if (tax_amt_recov == "" && tax_amt_recov == null) {
            tax_amt_recov = "0";
        }
        var item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        var item_net_val_spec = currentRow.find("#item_net_val_spec").val();
        var item_net_val_bs = currentRow.find("#item_net_val_spec").val();
        var gl_vou_no = null;
        var gl_vou_dt = null;
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
            if (currentRow.find("#ClaimITC").is(":checked")) {
                ClaimITC = "Y"
            }
            else {
                ClaimITC = "N"
            }
        }
        var wh_id = currentRow.find("#wh_id" + Sno).val();
        var LotNumber = currentRow.find("#LotNumber").val();
        var remarks = currentRow.find("#remarks").val();
        var LandedCostValue = currentRow.find("#TxtLandedCost").val();
        var LandedCostPerPc = currentRow.find("#TxtLandedCostPerPc").val();

        var PackSize = currentRow.find("#PackSize").val();
        var mrp = currentRow.find("#mrp").val();
        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, subitem: subitem, ItemType: ItemType
            , uom_id: uom_id, uom_name: uom_name, inv_qty: inv_qty, item_rate: item_rate, item_disc_perc: item_disc_perc, item_disc_amt: item_disc_amt, item_disc_val: item_disc_val, item_gr_val: item_gr_val
            , item_gr_val_bs: item_gr_val_bs, tax_amt: tax_amt, tax_amt_nrecov: tax_amt_nrecov, tax_amt_recov: tax_amt_recov, item_oc_amt: item_oc_amt
            , item_net_val_spec: item_net_val_spec, item_net_val_bs: item_net_val_bs, gl_vou_no: gl_vou_no
            , gl_vou_dt: gl_vou_dt, TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, ClaimITC: ClaimITC
            , item_acc_id: item_acc_id, wh_id: wh_id, LotNumber: LotNumber, remarks: remarks,
            LandedCostValue: LandedCostValue, LandedCostPerPc: LandedCostPerPc, srno: srno, mrp: mrp, PackSize: PackSize
        });
    });
    return PI_ItemsDetail;
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {
        }
        else {
            var ItmCode = currentRow.find("#hfItemID").val();
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
        }
    });
    return TaxDetails;
}
function OC_InsertTaxDetails() {
    debugger;
    var TaxDetails = [];
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
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
        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    });
    return TaxDetails;
}
function GetPI_OtherChargeDetails() {
    debugger;
    var PI_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = currentRow.find("#OCValue").text();
            var oc_val = currentRow.find("#OCAmount").text();
            var OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            var OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            var OCName = currentRow.find("#OCName").text();
            var OC_Curr = currentRow.find("#OCCurr").text();
            var OC_Conv = currentRow.find("#OCConv").text();
            var OC_AmtBs = currentRow.find("#OcAmtBs").text();
            var curr_id = currentRow.find("#HdnOCCurrId").text();
            var supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text(); 
            let bill_no = currentRow.find("#OCBillNo").text();
            let bill_date = currentRow.find("#OCBillDt").text();
            let tds_amt = currentRow.find("#OC_TDSAmt").text();
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            PI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName
                , OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, supp_id: supp_id, curr_id: curr_id
                , supp_type: supp_type, bill_no: bill_no, bill_date: bill_date, tds_amt: tds_amt
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    return PI_OCList;
};
function InsertTdsDetails() {
    debugger;
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var Tds_id = currentRow.find("#td_TDS_NameID").text();
        var Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        var Tds_level = currentRow.find("#td_TDS_Level").text();
        var Tds_val = currentRow.find("#td_TDS_Amount").text();
        var Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        var Tds_name = currentRow.find("#td_TDS_Name").text();
        var Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        var Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        var Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();
        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
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
        Batch_arr.MfgName = IsNull(CurrentRow.find("#bt_mfg_name").text(),"");
        Batch_arr.MfgMrp = IsNull(CurrentRow.find("#bt_mfg_mrp").val(), "") == "" ? "0" : CurrentRow.find("#bt_mfg_mrp").val();
        Batch_arr.MfgDate = IsNull(CurrentRow.find("#bt_mfg_date").val(),"");
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
        Serial_arr.MfgName = IsNull(CurrentRow.find("#sr_mfg_name").text(),"");
        Serial_arr.MfgMrp = IsNull(CurrentRow.find("#sr_mfg_mrp").val(), "") == "" ? "0" : CurrentRow.find("#sr_mfg_mrp").val();
        Serial_arr.MfgDate = IsNull(CurrentRow.find("#sr_mfg_date").val(),"");
        Serialarr.push(Serial_arr);
    })
    $("#hdn_SerialItemDeatilData").val(JSON.stringify(Serialarr));
}
function GetDPI_VoucherDetails() {
    debugger;
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            var acc_id = currentRow.find("#hfAccID").val();
            var acc_name = currentRow.find("#txthfAccID").val();
            var dr_amt = currentRow.find("#dramt").text();
            var cr_amt = currentRow.find("#cramt").text();
            var dr_amt_bs = currentRow.find("#dramtInBase").text();
            var cr_amt_bs = currentRow.find("#cramtInBase").text();
            var Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            PI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
        });
    }
    return PI_VouList;
};
function CheckDPI_Validations() {
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#SupplierName").css("border-color", "#ced4da");
    }
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    } else {
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
function CheckDPI_ItemValidations(VoucherValidate) {
    var ErrorFlag = "N";
    var count = 0;
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        let isFocused = false;
        var len = $("#POInvItmDetailsTbl > tbody > tr").length;
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            if (VoucherValidate == "Y") {
                if (parseFloat(len) == parseFloat(count)) {
                    var currentRow = $(this);
                    var Sno = currentRow.find("#SNohiddenfiled").val();
                    var POItemListName = currentRow.find("#POItemListName" + Sno).val();
                    if (POItemListName == "" || POItemListName == null || POItemListName == "0") {
                        currentRow.find("#SpanPOItemListName_Error" + Sno).text($("#valueReq").text());
                        currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "block");
                        currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "none");
                        currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
                    }
                    if (parseFloat(CheckNullNumber(currentRow.find("#ord_qty_spec").val())) == 0) {
                        currentRow.find("#ord_qty_spec_Error").text($("#valueReq").text());
                        currentRow.find("#ord_qty_spec_Error").css("display", "block");
                        currentRow.find("#ord_qty_spec").css("border-color", "red").focus();
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#ord_qty_spec_Error").css("display", "none");
                        currentRow.find("#ord_qty_spec").css("border-color", "#ced4da");
                    }
                    if (parseFloat(CheckNullNumber(currentRow.find("#item_rate").val())) == 0) {
                        currentRow.find("#item_rate_Error").text($("#valueReq").text());
                        currentRow.find("#item_rate_Error").css("display", "block");
                        currentRow.find("#item_rate").css("border-color", "red").focus();
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#item_rate_Error").css("display", "none");
                        currentRow.find("#item_rate").css("border-color", "#ced4da");
                    }
                   
                }               
                count = count + 1;
            }
            else {
                var currentRow = $(this);
                var ItemType = currentRow.find("#ItemType").val()
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var POItemListName = currentRow.find("#POItemListName" + Sno).val();
                if (POItemListName == "" || POItemListName == null || POItemListName == "0") {
                    currentRow.find("#SpanPOItemListName_Error" + Sno).text($("#valueReq").text());
                    currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "block");
                    currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#SpanPOItemListName_Error" + Sno).css("display", "none");
                    currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
                }
                if (parseFloat(CheckNullNumber(currentRow.find("#ord_qty_spec").val())) == 0) {
                    currentRow.find("#ord_qty_spec_Error").text($("#valueReq").text());
                    currentRow.find("#ord_qty_spec_Error").css("display", "block");
                    currentRow.find("#ord_qty_spec").css("border-color", "red").focus();
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#ord_qty_spec_Error").css("display", "none");
                    currentRow.find("#ord_qty_spec").css("border-color", "#ced4da");
                }
                if (parseFloat(CheckNullNumber(currentRow.find("#item_rate").val())) == 0) {
                    currentRow.find("#item_rate_Error").text($("#valueReq").text());
                    currentRow.find("#item_rate_Error").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red").focus();
                    ErrorFlag = "Y";
                    return false;
                }
                else {
                    currentRow.find("#item_rate_Error").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
                }
                if (ItemType == "Stockable") {
                    if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
                        currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
                        currentRow.find("#wh_Error" + Sno).css("display", "block");
                        currentRow.find("#wh_id" + Sno).css("border-color", "red").focus();
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#wh_Error" + Sno).css("display", "none");
                        currentRow.find("#wh_id" + Sno).css("border-color", "#ced4da");
                    }
                }                
                if (parseFloat(currentRow.find("#item_disc_amt").val()) > 0) {
                    if (parseFloat(currentRow.find("#item_disc_amt").val()) >= parseFloat(currentRow.find("#item_rate").val())) {
                        currentRow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
                        currentRow.find("#item_disc_amtError").css("display", "block");
                        currentRow.find("#item_disc_amt").css("border-color", "red");
                        if (!isFocused) {
                            currentRow.find("#item_disc_amt").focus();
                            isFocused = true;
                        }
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#item_disc_amtError").css("display", "none");
                        currentRow.find("#item_disc_amt").css("border-color", "#ced4da");
                    }
                }
            }
        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
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
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Batchable = currentRow.find("#hfbatchable").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ord_qty_spec").val();
        if (Batchable == "Y") {
            var Count = $("#SaveItemBatchTbl tbody tr").length;
            var totalbatchQty = "0";
            if (Count != null) {
                if (Count > 0) {
                    $("#SaveItemBatchTbl tbody tr").each(function () {
                        var Curr = $(this);
                        var batchitemit = Curr.find("#ItemID").val();
                        if (batchitemit == ItemID) {
                            var BatchQty = Curr.find("#BatchQty").val();
                            totalbatchQty = parseFloat(totalbatchQty) + parseFloat(BatchQty)
                        }
                    })
                    if (totalbatchQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "BtnBatchDetail", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                }
            }
            else {

                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
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
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Serialable = currentRow.find("#hfserialable").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ord_qty_spec").val();
        if (Serialable == "Y") {
            var TotalSQty = parseFloat($("#SaveItemSerialTbl tbody tr #SerialItemID[value='" + ItemID + "']").closest('tr').length).toFixed(QtyDecDigit);
            if (TotalSQty != null) {
                if (parseFloat(TotalSQty) > parseFloat(0)) {
                    if (TotalSQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
                    ErrorFlag = "Y";
                }
            }
            else {
                ErrorFlag = "Y";
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
async function OnChangeBillNo() {
    const Bill_No = $("#Bill_No").val();
    const Bill_Date = $("#Bill_Date").val();
    const DocumentMenuId = $("#DocumentMenuId").val();
    const supp_id = $("#SupplierName").val();
    const hdn_Bill_No = $("#hdn_Bill_No").val();
    const hdn_Bill_Dt = $("#hdn_Bill_Dt").val();

    const showError = (id, msgId) => {
        $(id).css("border-color", "Red");
        $(msgId).text($("#valueduplicate").text()).css("display", "block");
    };

    const clearError = (id, msgId) => {
        $(id).css("border-color", "#ced4da");
        $(msgId).css("display", "none");
    };
    if (!Bill_No) {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text()).css("display", "block");
        return;
    } else {
        clearError("#Bill_No", "#SpanBillNoErrorMsg");
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
                    showError("#Bill_No", "#SpanBillNoErrorMsg");
                    showError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else if (hdn_Bill_Dt !== Bill_Date) {
                    showError("#Bill_No", "#SpanBillNoErrorMsg");
                    showError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else {
                    clearError("#Bill_No", "#SpanBillNoErrorMsg");
                    clearError("#Bill_Date", "#SpanBillDateErrorMsg");
                    $("#duplicateBillNo").val("N");
                }
            } else {
                clearError("#Bill_No", "#SpanBillNoErrorMsg");
                clearError("#Bill_Date", "#SpanBillDateErrorMsg");
                $("#duplicateBillNo").val("N");
            }
        }
    });

    RefreshBillNoBillDateInGLDetail();
}

//async function OnChangeBillNo() {
//    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
//        $("#Bill_No").css("border-color", "Red");
//        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
//        $("#SpanBillNoErrorMsg").css("display", "block");
//    }
//    else {
//        $("#SpanBillNoErrorMsg").css("display", "none");
//        $("#Bill_No").css("border-color", "#ced4da");      
//    }
//    var bill_dt = $("#Bill_Date").val();
//    var DocumentMenuId = $("#DocumentMenuId").val();
//    var Bill_No = $("#Bill_No").val();
//    var supp_id = $("#SupplierName").val();
//    var hdn_Bill_No = $("#hdn_Bill_No").val();
//    var hdn_Bill_Dt = $("#hdn_Bill_Dt").val();
//    await $.ajax(
//        {
//            type: "POST",
//            url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
//            data: { supp_id: supp_id, Bill_No: Bill_No, doc_id: DocumentMenuId, bill_dt: bill_dt },
//            dataType: "json",
//            success: function (data) {
//                debugger
//                if (data == 'ErrorPage') {
//                    LSO_ErrorPage();
//                    return false;
//                }
//                if (data !== null && data !== "") {
//                    var arr = [];
//                    arr = JSON.parse(data);
//                    var flag = "N";
//                    if (arr.Table[0].Result == "Duplicate") {
//                        if (hdn_Bill_No != Bill_No) {
//                            $("#Bill_No").css("border-color", "Red");
//                            $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
//                            $("#SpanBillNoErrorMsg").css("display", "block");
//                            $("#duplicateBillNo").val("Y");

//                            $('#SpanBillDateErrorMsg').text($("#valueduplicate").text());
//                            $("#SpanBillDateErrorMsg").css("display", "block");
//                            $("#Bill_Date").css("border-color", "Red");

//                            flag = "Y";
//                        }
//                        else {
//                            $("#SpanBillNoErrorMsg").css("display", "none");
//                            $("#Bill_No").css("border-color", "#ced4da");
//                            $("#duplicateBillNo").val("N");
//                        }
//                        if (flag != "Y") {
//                            if (hdn_Bill_Dt != bill_dt) {
//                                $('#SpanBillDateErrorMsg').text($("#valueduplicate").text());
//                                $("#SpanBillDateErrorMsg").css("display", "block");
//                                $("#Bill_Date").css("border-color", "Red");

//                                $("#Bill_No").css("border-color", "Red");
//                                $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
//                                $("#SpanBillNoErrorMsg").css("display", "block");
//                                $("#duplicateBillNo").val("Y");
//                            }
//                            else {
//                                $("#SpanBillDateErrorMsg").css("display", "none");
//                                $("#Bill_Date").css("border-color", "#ced4da");
//                            }
//                        }
                       
//                    }
//                    else {
//                        $("#SpanBillNoErrorMsg").css("display", "none");
//                        $("#Bill_No").css("border-color", "#ced4da");
//                        $("#duplicateBillNo").val("N");

//                        $("#SpanBillDateErrorMsg").css("display", "none");
//                        $("#Bill_Date").css("border-color", "#ced4da");
//                    }
//                }
//            },
//        });
//    RefreshBillNoBillDateInGLDetail();
//}
function OnChangeBillDate() {
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
        //RefreshBillNoBillDateInGLDetail();
        OnChangeBillNo();
    }
}
function RefreshBillNoBillDateInGLDetail() {
    /* Modified by Suraj Maurya on 11-08-2025 to update bill_no and bill_date in GL Narration */
    /*This Function is Created by Suraj on 22-10-2024 to reset bill number and bill date to GL Details*/
    let supp_acc_id = $("#supp_acc_id").val();
    let bill_no = $("#Bill_No").val();
    let bill_dt = $("#Bill_Date").val();
    let change_vou_sr_no = "";
    //$('#VoucherDetail > tbody > tr #hfAccID[value="' + supp_acc_id + '"]').closest("tr").each(function () {
    $('#VoucherDetail > tbody > tr').closest("tr").each(function () {
        let row = $(this);
        let hfAccID = row.find("#hfAccID").val();
        let vouType = row.find("#glVouType").val();
        let vou_sr_no = row.find("#td_vou_sr_no").text();
        if (hfAccID == supp_acc_id) {
            change_vou_sr_no = vou_sr_no;
            row.find("#gl_bill_no").val(bill_no);
            row.find("#gl_bill_date").val(bill_dt);
            let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
            row.find("#gl_narr span").text(narr);
        } else {
            if (vou_sr_no == change_vou_sr_no) {
                let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
                row.find("#gl_narr span").text(narr);
            }
        }
    });
}
function AddNewItem() {
    var GstApplicable = $("#Hdn_GstApplicable").text();
    // <input id="TxtItemName" class="form-control time" autocomplete="off" type="text" name="" value="" onblur="this.placeholder='Item Name'" disabled="">
    // <input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" value="" placeholder="${$("#ItemUOM").text()}" onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled="">
    var S_NO = $('#POInvItmDetailsTbl tbody tr').length + 1;
    var RowNo = 0;
    if (RowNo == "0") {
        RowNo = 1;
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    var manualGST = "";
    if (GstApplicable == "Y") {
        manualGST =`<td class="qt_to">
                                                                                <div class="custom-control custom-switch sample_issue">
                                                                                    <input type="checkbox" class="custom-control-input  margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST">
                                                                                    <label class="custom-control-label" disabled="" style="padding: 3px 0px;"> </label>
                                                                                </div>
                                                                            </td>
                                                                            <td class="qt_to">
                                                                                <div class="custom-control custom-switch sample_issue">
                                                                                    <input type="checkbox" checked="" class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                                                                                    <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                                                                                </div>
                                                                            </td>`
    }
    $('#POInvItmDetailsTbl tbody').append(`<tr id="R${S_NO}">
                                                                            <td class=" red center"> <i class="fa fa-trash delBtnIconDPI" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                                            <td class="sr_padding" id="srno">${S_NO}</td>
                                                                            <td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
                                                                            <td class="ItmNameBreak itmStick tditemfrz">
<div class="lpo_form">
                                                                                <div class=" col-sm-10 no-padding">
                                                                                    <select class="form-control" id="POItemListName${RowNo}" name="POItemListName${RowNo}" onchange="OnChangePOItemName(${RowNo},event)" ></select>
                                                                                    <input type="hidden" id="hfItemID" value="">
                                                                                    <input type="hidden" id="hdn_item_gl_acc" value="">
                                                                                    <input hidden="" type="text" id="sub_item" value="">
                                                                                </div>
                                                                                <div class="col-sm-2">
                                                                                    <div class="col-sm-5 i_Icon pl-0">
                                                                                        <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
                                                                                    </div>
                                                                                    <div class="col-sm-7"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                                                </div>
                                                                                <span id="SpanPOItemListName_Error${S_NO}" class="error-message is-visible"></span>
</div>
                                                                            </td>
                                                                            <td>                                                                               
                                                                                <select id="UOM" class="form-control date" onchange="onChangeUom(event)">
                                                                                     <option value="0">---Select---</option>
                                                                                </select>
                                                                                <input type="hidden" id="hfUOMID" value="">
                                                                                <input id="ItemHsnCode" value="" type="hidden">
                                                                                <input id="ItemtaxTmplt" value="" type="hidden">
                                                                                <input type="hidden" id="hdnItemType" value="">
                                                                                <input type="hidden" id="ForDisable" value="">
                                                                            </td>
                                                                            <td>
                                                                                <input id="ItemType" class="form-control" onchange="" onkeypress="" autocomplete="off" type="text" name="ItemType" placeholder="${$("#ItemType").text()}" value="" disabled="">
                                                                            </td>
 <td>
                                                                        <input id="mrp" class="form-control num_right" autocomplete="off" type="text" name="mrp" placeholder="${$("#span_MRP").text()}" value="" disabled>
                                                                    </td>
                                                                    <td>
                                                                        <input id="PackSize" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="" onmouseover="OnMouseOver(this)" disabled>
                                                                    </td>
                                                                            <td class="lpo_form">
                                                                                <div class="col-sm-10 num_right no-padding">
                                                                            <input id="ord_qty_spec" onchange="OnChangeDPIItemQuantity(event)" onkeypress="return AmountFloatQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="ReceivedQuantity" value="" placeholder="0000.00">
                                                                        </div>
                                                                        <div class="col-sm-2 i_Icon" id="SubItemDPIQtyDiv">
                                                                            <button type="button" id="SubItemDPIQty" disabled="" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QuantitySpec',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                        </div>
                                                                                <span id="ord_qty_spec_Error" class="error-message is-visible"></span>
                                                                            </td>
                                                                            <td>
                                                                                <div class="lpo_form">
                                                                                    <input id="item_rate" class="form-control num_right" onchange="OnChangeDPIItemRate(event)" onkeypress="return AmountFloatQty(this,event);" autocomplete="off" type="text" name="Rate" placeholder="0000.00" value="">
                                                                                    <span id="item_rate_Error" class="error-message is-visible"></span>
                                                                                </div>
                                                                            </td>
                                                                            <td>                                                                       
                                                                            <input id="item_disc_perc" class="form-control num_right" onchange="OnChangePOItemDiscountPerc(event)" value="" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc" placeholder="0000.00" onblur="this.placeholder='0000.00'">                                                                       
                                                                    </td>
                                                                    <td>
                                                                        <div class="lpo_form">                                                                          
                                                                                <input id="item_disc_amt" class="form-control date num_right" onchange="OnChangePOItemDiscountAmt(event)" value="" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt" placeholder="0000.00" onblur="this.placeholder='0000.00'">                                                                         
                                                                            <span id="item_disc_amtError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="item_disc_val" class="form-control date num_right" value="" autocomplete="off" type="text" name="item_disc_val" placeholder="0000.00" disabled>
                                                                    </td>
                                                                            <td>
                                                                                <input id="item_gr_val" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="" placeholder="0000.00" disabled="">
                                                                            </td>
                                                                            <td hidden><div class="lpo_form"><input id="item_ass_val" class="form-control date num_right" autocomplete="off" type="text" name="item_ass_val" disabled placeholder="0000.00" onblur="this.placeholder='0000.00'"><span id="item_ass_valError" class="error-message is-visible"></span></div></td>
                                                                            <td>
                                                                                <div class="custom-control custom-switch sample_issue">
                                                                                    <input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted">
                                                                                    <label class="custom-control-label" disabled="" style="padding: 3px 0px;"> </label>
                                                                                </div>
                                                                            </td>
                                                                            `+ manualGST+`
                                                                            <td>
                                                                                <div class=" col-sm-10 num_right no-padding">
                                                                                    <input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" value="" name="item_tax_amt" placeholder="0000.00" disabled="">
                                                                                </div><div class=" col-sm-2 num_right no-padding">
                                                                                    <button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#TaxInfo").text()}"></i></button>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <input id="Txt_item_tax_non_rec_amt" class="form-control num_right" autocomplete="off" type="text" value="" name="TaxAmountNonRecoverable" placeholder="0000.00" disabled="">
                                                                            </td>
                                                                            <td class="no-padding">
                                                                                <input autocomplete="off" class="form-control num_right text_bold" id="Txt_item_tax_rec_amt" name="TaxAmountRecoverable" placeholder="0000.00" readonly="readonly" type="text" value="">
                                                                            </td>
                                                                            <td>
                                                                                <input id="TxtOtherCharge" class="form-control num_right" autocomplete="off" type="text" value="" name="OtherCharge" placeholder="0000.00" disabled="">
                                                                            </td>
                                                                            <td>
                                                                                <input id="item_net_val_spec" class="form-control num_right" autocomplete="off" type="text" value="" name="NetValue" placeholder="0000.00" disabled="">
                                                                            </td>
                                                                    <td>
                                                                        <input id="TxtLandedCostPerPc" class="form-control num_right" autocomplete="off" type="text" value="" name="NetLandedValue" placeholder="0000.00" disabled>
                                                                    </td>
                                                                    <td>
                                                                        <input id="TxtLandedCost" class="form-control num_right" autocomplete="off" type="text" value="" name="NetLandedValue" placeholder="0000.00" disabled>
                                                                    </td>
                                                                            <td>
                                                                                <div class="lpo_form">
                                                                                    <select class="form-control" id="wh_id${RowNo}" name="wh_id${RowNo}" onchange="OnChnageWarehouse(event)"></select>
                                                                                    <input type="hidden" id="Hdn_GRN_WhName${RowNo}" value="" style="display: none;">
                                                                                    <span id="wh_Error${S_NO}" class="error-message is-visible"></span>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <input id="LotNumber" value="" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}" onblur="this.placeholder='${$("#Span_Delete_Title").text()}'" disabled="">
                                                                            </td>
                                                                            <td class="center">
                                                                                <button type="button" value="" class="calculator subItmImg" onclick="return ItemStockBatchWise(this,event)" id="BtnBatchDetail" data-toggle="modal" data-target="#ExternalReceiptBatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                                            </td>
                                                                            <td class="center">
                                                                                <button type="button" value="" class="calculator subItmImg" onclick="ItemStockSerialWise(this,event)" id="BtnSerialDetail" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                                            </td>
                                                                            <td style="display: none;"><input type="hidden" id="hfbatchable" value="" /></td>
                                                                            <td style="display: none;"><input type="hidden" id="hfserialable" value="" /></td>
                                                                            <td style="display: none;"><input type="hidden" id="hfexpiralble" value="" /></td>
                                                                            <td>
                                                                                <textarea id="remarks" value="" class="form-control remarksmessage" name="remarks" maxlength="100" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}" title=""></textarea>
                                                                            </td>
                                                                        </tr>`)
    BindPOItmList(RowNo)
    BindWarehouseList(RowNo);
}
function AmtFloatValueonly(el, evt) {
    debugger;
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#item_rate").val();
    item_rate = CheckNullNumber(item_rate);

    var selectedval = Cmn_SelectedTextInTextField(evt);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#item_disc_amtError").css("display", "none");
    clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
    return true;
}
function OnClickOCTaxCalculationBtn(e) {
    debugger;
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
function OnChnageWarehouse(e) {
    try {
        debugger;
        var currentRow = $(e.target).closest("tr");
        var whname = currentRow.find("#wh_id option:selected").text();
        var Sno = currentRow.find("#SNohiddenfiled").val();
        $("#Hdn_GRN_WhName" + Sno).val(whname);
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
function OnChangeDPIItemQuantity(e) {
    CalculationBaseQty(e);
}
function OnChangeDPIItemRate(e) {
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow);
}
function CalculationBaseQty(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;

    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#SpanPOItemListName_Error"+Sno).text($("#valueReq").text());
        clickedrow.find("#SpanPOItemListName_Error"+Sno).css("display", "block");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#ord_qty_spec").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SpanPOItemListName_Error"+Sno).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (OrderQty != "" && OrderQty != ".") {
        OrderQty = parseFloat(OrderQty);
    }
    if (OrderQty == ".") {
        OrderQty = 0;
    }
    if (OrderQty == "" || OrderQty == 0 || ItemName == "0") {
        clickedrow.find("#ord_qty_spec_Error").text($("#valueReq").text());
        clickedrow.find("#ord_qty_spec_Error").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#ord_qty_spec").focus();
        //clickedrow.find("#item_oc_amt").val("");//05-02-2025
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_spec_Error").text("");
        clickedrow.find("#ord_qty_spec_Error").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
    }

    var OrderQty = clickedrow.find("#ord_qty_spec").val();
    var ItmRate = clickedrow.find("#item_rate").val();

    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#ord_qty_spec").val("");
        OrderQty = 0;
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        //FinalFinVal = ConvRate * FinVal
        //clickedrow.find("#item_net_val_bs").val(parseFloat(FinalFinVal).toFixed(ValDecDigit));
        CalculateAmount();
    }
    clickedrow.find("#ord_qty_base").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
    clickedrow.find("#ord_qty_spec").val(parseFloat(OrderQty).toFixed(QtyDecDigit));

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        CalculateDisPercent(clickedrow,"N");
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        CalculateDisAmt(clickedrow,"N");
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        //if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        //}
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    let SuppId = $("#SupplierName").val();
    let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
    if (GrossAmt != 0) {
        AutoTdsApply(SuppId, GrossAmt).then(() => {
            GetAllGLID();
        });
    }    
    else if (ItmRate != 0 && ItmRate != null && ItmRate != "") {
        GetAllGLID();
    }
}
function CalculationBaseRate(clickedrow, flag) {
    debugger;
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate 
            clickedrow.find("#SpanPOItemListName_Error"+Sno).text($("#valueReq").text());
            clickedrow.find("#SpanPOItemListName_Error"+Sno).css("display", "block");
            clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
            clickedrow.find("#item_rate").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SpanPOItemListName_Error"+Sno).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate
            clickedrow.find("#ord_qty_spec_Error").text($("#valueReq").text());
            clickedrow.find("#ord_qty_spec_Error").css("display", "block");
            clickedrow.find("#ord_qty_spec").css("border-color", "red");
            clickedrow.find("#ord_qty_spec").val("");
            clickedrow.find("#ord_qty_spec").focus();
            clickedrow.find("#item_rate").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_spec_Error").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
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
        clickedrow.find("#item_rate_Error").text($("#valueReq").text());
        clickedrow.find("#item_rate_Error").css("display", "block");
        clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#item_rate").focus();
        }
        //clickedrow.find("#item_oc_amt").val("");//05-02-2025
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rate_Error").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00") {
        CalculateDisPercent(clickedrow,"N");
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0.00") {
        CalculateDisAmt(clickedrow,"N");
    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
        }
    }
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        //if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        //}
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    let SuppId = $("#SupplierName").val();
    let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
    if (GrossAmt != 0) {
        AutoTdsApply(SuppId, GrossAmt).then(() => {
            GetAllGLID();
        });
    }
    else if(OrderQty != 0 && OrderQty != null && OrderQty != ""){
        GetAllGLID();
    }
}
function CalculateDisPercent(clickedrow, ApplyGL) {
    debugger;
    //var ConvRate = $("#conv_rate").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisPer;

    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (AvoidDot(DisPer) == false) {
        clickedrow.find("#item_disc_perc").val("");
        DisPer = 0;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        var FAmt = (ItmRate * DisPer) / 100;
        var GAmt = OrderQty * (ItmRate - FAmt);
        var DisVal = OrderQty * FAmt;
        var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
        var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinGVal);
        clickedrow.find("#item_ass_val").val(FinGVal);
        clickedrow.find("#item_net_val_spec").val(FinGVal);
        //FinalGVal = ConvRate * FinGVal
        //clickedrow.find("#item_net_val_bs").val(parseFloat(FinalGVal).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_val").val(FinDisVal);
        CalculateAmount();
        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(cmn_PerDecDigit));
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            //FinalVal = ConvRate * FinVal
            //clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateAmount();
        }
        clickedrow.find("#item_disc_amt").prop("readonly", false);
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
   
    if (OrderQty != 0 && OrderQty != "" && ItmRate != 0 && ItmRate != "") {
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else if (ApplyGL == "Y") {
            GetAllGLID();
        }
    }
}
function CalculateDisAmt(clickedrow,ApplyGL) {
    debugger;
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisAmt;
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    if (AvoidDot(DisAmt) == false) {
        clickedrow.find("#item_disc_amt").val("");
        DisAmt = 0;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        if (Math.fround(ItmRate) > Math.fround(DisAmt)) {
            var FRate = (ItmRate - DisAmt);
            var GAmt = OrderQty * FRate;
            var DisVal = OrderQty * DisAmt;
            var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
            var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
            clickedrow.find("#item_disc_amtError").text("");
            clickedrow.find("#item_disc_amtError").css("display", "none");
            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
            clickedrow.find("#item_disc_val").val(FinDisVal);
            clickedrow.find("#item_gr_val").val(FinGVal);
            clickedrow.find("#item_ass_val").val(FinGVal);
            clickedrow.find("#item_net_val_spec").val(FinGVal);
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_perc").val("");
            CalculateAmount();
        }
        else {
            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
            clickedrow.find("#item_disc_amtError").css("display", "block");
            clickedrow.find("#item_disc_amt").css("border-color", "red");
            clickedrow.find("#item_disc_amt").val('');
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
        }
        clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ValDecDigit));
    }
    else {
        clickedrow.find("#item_disc_amtError").text("");
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            CalculateAmount();
        }
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_perc").prop("readonly", false);
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    if (OrderQty != 0 && OrderQty != "" && ItmRate != 0 && ItmRate != "") {
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else if (ApplyGL == "Y") {
            GetAllGLID();
        }
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
                        if (arr.Table.length > 0) {
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table.length; i++) {
                                s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                            }
                            $("#wh_id" + id).html(s);
                        }
                    }
                },
            });
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }
}
function BindPOItmList(ID) {
    debugger;
    var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
    var ItemCode = "";
    ItemCode = $(this).closest('tr').find("#POItemListName" + SNo).val();
    Cmn_DeleteSubItemQtyDetail(ItemCode);
    BindPItemList("#POItemListName", ID, "#POInvItmDetailsTbl", "#SNohiddenfiled", "", "AllItemsForDPI");
}
function BindPItemList(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
        $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableItemDDLPO(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName)

        });
    }
    else {
        DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName)
    }
    if (OtherFx == "BindData") {
        BindData();
    }
}
function DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    $(ItmDDLName + RowID).select2({

        ajax: {
            url: "/Common/Common/GetItemList3",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    PageName: PageName,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize

                ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                var ItemListArrey = [];
                if (sessionStorage.getItem("selecteditemlist").length != null) {
                    ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                }
                let selected = [];
                selected.push({ id: $(ItmDDLName + RowID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemType").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---_0" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name.split("_")[0], UOM: val.ID.split("_")[1], type: val.Name.split("_")[1] };
                    }),
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
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function OnChangePOItemName(SrNo,e) {
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    Cmn_DeleteSubItemQtyDetail(ItemID);
    Itm_ID = clickedrow.find("#POItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);
    if (Itm_ID == "0") {
        clickedrow.find("#SpanPOItemListName_Error" + SNo).text($("#valueReq").text());
        clickedrow.find("#SpanPOItemListName_Error" + SNo).css("display", "block");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#SpanPOItemListName_Error" + SNo).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField();
    try {
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");
        debugger
    } catch (err) {
    }
    getMrpBoxDetailPrice(e);//add by Shubham Maurya on 18-12-2025
}
function getMrpBoxDetailPrice(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var supp_id = $('#SupplierName').val();
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/DPO/GetSPLDetail",
            data: {
                ItemID: ItemID,
                supp_id: supp_id
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "" && data != "ErrorPage") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        clickedrow.find("#PackSize").val(arr.Table[0].pack_size);
                        clickedrow.find("#mrp").val(parseFloat(arr.Table[0].mrp).toFixed(RateDecDigit));
                        clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].price).toFixed(RateDecDigit));
                        clickedrow.find("#item_disc_perc").val(parseFloat(arr.Table[0].item_disc_perc).toFixed(cmn_PerDecDigit));
                    }
                }
            },
        });
}
function ClearRowDetails(e, ItemID) {
    var clickedrow = $(e.target).closest("tr");
    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#UOM").val("");
    clickedrow.find("#ItemType").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#ord_qty_spec").val("");
    clickedrow.find("#item_rate").val("");
    clickedrow.find("#item_gr_val").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#Txt_item_tax_non_rec_amt").val("");
    clickedrow.find("#Txt_item_tax_rec_amt").val("");
    clickedrow.find("#TxtOtherCharge").val("");
    clickedrow.find("#item_net_val_spec").val("");
    clickedrow.find("#LotNumber").val("");
    clickedrow.find("#remarks").val("");
    CalculateAmount();
    var TOCAmount = parseFloat($("#TxtOtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetPO_ItemTaxDetail();
}
function AfterDeleteResetPO_ItemTaxDetail() {
    var POInvItmDetailsTbl = "#POInvItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var POItemListName = "#POItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(POInvItmDetailsTbl, SNohiddenfiled, POItemListName);
}
function DisableHeaderField() {
    $("#SupplierName").attr('disabled', true);
}
function CalculateAmount() {
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    //var NetLandedValue = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_ass_val").val() == "" || currentRow.find("#item_ass_val").val() == "NaN") {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_ass_val").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_net_val_spec").val() == "" || currentRow.find("#item_net_val_spec").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val_spec").val())).toFixed(ValDecDigit);
        }
        //if (parseFloat(CheckNullNumber(currentRow.find("#TxtLandedCost").val())) > 0) {

        //    NetLandedValue = (parseFloat(NetLandedValue) + parseFloat(currentRow.find("#TxtLandedCost").val())).toFixed(ValDecDigit);
        //}
    });
    var oc_amount = $("#TxtDocSuppOtherCharges").val();//05-02-2025
    //var oc_amount = $("#TxtOtherCharges").val();
    NetOrderValueSpec = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amount));
    $("#TxtGrossValue").val(GrossValue);
    //$("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
    $("#NetOrderValueInBase").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
    //$("#ToTNetLandedValue").val(NetLandedValue);
    if ($("#chk_roundoff").is(":checked")) {
        var roundoff_netval = Math.round(NetOrderValueSpec);
        var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
        $("#NetOrderValueInBase").val(f_netval);
    }
    else {
        $("#NetOrderValueInBase").val(NetOrderValueSpec.toFixed(ValDecDigit));
    }
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    var TotalGAmt = $("#TxtGrossValue").val();
    //var ConvRate = $("#conv_rate").val();
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var GrossValue = currentRow.find("#item_gr_val").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCValue = currentRow.find("#TxtOtherCharge").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#item_gr_val").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(ValDecDigit);
        }
        var TaxNonRecov = currentRow.find("#Txt_item_tax_non_rec_amt").val();
        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#item_net_val_spec").val((parseFloat(CheckNullNumber(POItm_NetOrderValueSpec))).toFixed(ValDecDigit));
        let DPIQty = currentRow.find("#ord_qty_spec").val();
        debugger
        debugger
        let LandedCostValue = parseFloat(CheckNullNumber(POItm_GrossValue)) + parseFloat(CheckNullNumber(POItm_OCAmt)) + parseFloat(CheckNullNumber(TaxNonRecov));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
    });
    CalculateAmount();
};
function OnChangeSupplier(SuppID) {
    debugger;
    var Supp_id = SuppID.value;
    if (Supp_id == "" || Supp_id == null || Supp_id == undefined) {
        Supp_id = SuppID;
    }
    if (Supp_id == "") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#AddItemsInDPI").css("display", "none")
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_PInvSuppName").val(Suppname);
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#AddItemsInDPI").css("display","")
    }
    GetSuppAddress(Supp_id);
    OnChangeBillNo();
}
function GetSuppAddress(Supp_id) {
    let ExchDecDigit = $("#ExchDigit").text();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DPO/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Rqrdaddr").css("display", "");
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#supp_acc_id").val(arr.Table[0].supp_acc_id);
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            $("#curr_id").val(arr.Table[0].curr_id);
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                            if (arr.Table[0].gst_cat == "UR") {
                                $('#RCMApplicable').prop("checked", true);
                                $("#RCMApplicable").prop("disabled", true);
                            }
                            else {
                                $('#RCMApplicable').prop("checked", false);
                                $("#RCMApplicable").prop("disabled", false);
                            }
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            if ($("#Address").val() === "") {
                                $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
                                $("#Address").css("border-color", "Red");
                                $("#SpanSuppAddrErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppAddrErrorMsg").css("display", "none");
                                $("#Address").css("border-color", "#ced4da");
                            }
                        }
                        else {
                            $("#Address").val(null);
                            $("#bill_add_id").val(null);
                            $("#ship_add_gstNo").val(null);
                            $("#Ship_StateCode").val(null);
                            var s = '<option value="' + "0" + '">' + "---Select---" + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
                            $("#conv_rate").prop("readonly", false);
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfPIStatus").val().trim();
    var PODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        PODTransType = "Update";
    }
    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, PODTransType);
}
function CheckRCMApplicable() {
    debugger;
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    let SuppId = $("#SupplierName").val();
    let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
    if (GrossAmt != 0) {
        AutoTdsApply(SuppId, GrossAmt).then(() => {
            GetAllGLID();
        });
    }
    else {
        GetAllGLID();
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickSupplierInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    var Supp_id = $('#SupplierName').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
async function onChangeUom(e) {
    debugger;
    let Crow = $(e.target).closest('tr');
    let Sno = Crow.find("#SNohiddenfiled").val();
    let ItemId = Crow.find("#POItemListName" + Sno).val();
    let UomId = Crow.find("#UOM").val();

    await Cmn_StockUomWise(ItemId, UomId).then((res) => {
        Crow.find("#UOMID").val(UomId);
    }).catch(err => console.log(err.message));
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}
function FloatValuePerOnly(el, evt) {
    $("#SpanTaxPercent").css("display", "none");
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }
}
function OnChangeGrossAmt() {
    //var TotalOCAmt = $('#Total_OC_Amount').text();
    //var TotalOCAmt = $('#TxtDocSuppOtherCharges').val();
    //var Total_PO_OCAmt = $('#TxtOtherCharges').val();
    //if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
    //    if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {
    //        Calculate_OC_AmountItemWise(TotalOCAmt);
    //    }
    //}
    var Total_PO_OCAmt = $('#TxtOtherCharges').val();
    if (parseFloat(Total_PO_OCAmt) > 0) {        
        Calculate_OC_AmountItemWise(Total_PO_OCAmt);
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
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
            $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#POItemListName" + Sno).val();
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
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit);
                                    TaxNonRecovAmt = 0;
                                    TaxRecovAmt = 0;
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                if (currentRow.find("#TaxExempted").is(":checked")) {
                                    if (parseFloat(ItemTaxAmt) == 0) {
                                        CRow.remove();
                                    }
                                }
                                else if (GstApplicable == "Y") {
                                    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                        if (parseFloat(ItemTaxAmt) == 0) {
                                            CRow.remove();
                                        }
                                    }
                                }
                                currentRow.find("#item_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {//05-02-2025
                                //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                                //}
                                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                                currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);

                                let DPIQty = currentRow.find("#ord_qty_spec").val();
                                debugger
                                debugger
                                let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                                let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                                currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                                currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        //    OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                        //}
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        let DPIQty = currentRow.find("#ord_qty_spec").val();
                        debugger;
                        debugger;
                        let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
        else {
            $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#POItemListName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                        OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                    }
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                    currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                    let DPIQty = currentRow.find("#ord_qty_spec").val();
                    debugger;
                    debugger;
                    let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                    let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                    currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                    currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                }
            });
        }
    }
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
}
function OnChangePOItemDiscountPerc(e) {
    let trgtrow = $(e.target).closest("tr");
    CalculateDisPercent(trgtrow,"Y");
}
function OnChangePOItemDiscountAmt(e) {
    let trgtrow = $(e.target).closest("tr");
    CalculateDisAmt(trgtrow,"Y");
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
    var TaxOn = $("#HdnTaxOn").val();
    var tbllenght = $("#TaxCalculatorTbl tbody tr").length
    if (TaxOn == "Item") {
        if (tbllenght == 0) {
            $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
            $("#SpanTax_Template").text($("#valueReq").text());
            $("#SpanTax_Template").css("display", "block");
            $("#SaveAndExitBtn").attr("data-dismiss", "");
            return false;
        }
        else {
            $("#SaveAndExitBtn").attr("data-dismiss", "modal");
        }
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
        //BindTaxAmountDeatils(NewArr);
    }
    else {
        //var TaxCalculationList = [];
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
        //BindTaxAmountDeatils(TaxCalculationList);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit);
                Cmn_click_Oc_chkroundoff(null, currentRow);
                //currentRow.find("#OCTotalTaxAmt").text(Total);
                var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
            }
        });
        //$("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {/* Commented by Suraj Maurya on 04-04-2025 */
        //    debugger;
        //    var currentRow = $(this);
        //    if (currentRow.find("#OCValue").text() == TaxItmCode) {
        //        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
        //        currentRow.find("#OCTaxAmt").text(TaxAmt);               
        //        Cmn_click_Oc_chkroundoff(null, currentRow);
        //    }
        //});
        Calculate_OCAmount();
    }
    else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode) {
                let InvQty = currentRow.find("#ord_qty_spec").val();
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    tax_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                    tax_non_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                }
                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);

                currentRow.find("#item_tax_amt").val(TaxAmt);
                //var itmocamt = currentRow.find("#item_oc_amt").val();//05-02-2025
                var itmocamt = currentRow.find("#TxtOtherCharge").val();
                if (itmocamt != null && itmocamt != "") {
                    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#item_net_val_spec").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
                debugger;
                debugger;
                let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(InvQty));
                currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
    if (MGST == "MGST" && TaxOn != "OC") {
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else {
            GetAllGLID();
        }
    }
}
function OnClickReplicateOnAllItems() {
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;

    var ArrNonRecov = [];
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
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
            var TableOnwhichTaxApply = "POInvItmDetailsTbl";
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
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#item_gr_val").val();

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
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);

            var ItemID = currentRow.find("#hfItemID").val();
            var GrnQty = currentRow.find("#ord_qty_spec").val();
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
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueBase);
                            //FinalNetOrderValueSpec = (NetOrderValueBase / ConvRate).toFixed(ValDecDigit);
                            //currentRow.find("#TxtNetValue").val(FinalNetOrderValueSpec);
                            debugger;
                            debugger;
                            let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                            let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                            currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                            currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                    currentRow.find("#item_net_val_spec").val(FGrossAmt);
                    //FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(ValDecDigit);
                    //currentRow.find("#TxtNetValue").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                currentRow.find("#item_net_val_spec").val(FGrossAmt);
                //FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(ValDecDigit);
                //currentRow.find("#TxtNetValue").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
    }
}
function OnClickTaxCalBtn(e) {
    debugger;
    var SOItemListName = "#POItemListName";
    var SNohiddenfiled = "SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var currentRow = $(e.target).closest("tr");
    if (GstApplicable == "Y") {
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    var Itm_ID = currentRow.find("#hfItemID").val();
    var item_gross_val = currentRow.find("#item_gr_val").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    if (GstApplicable == "Y") {
        if (currentRow.find("#ForDisable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").prop("disabled", true).css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").prop("disabled", false).css("display", "block");
            }
        }
    }
}
/***-----------------Add for Batch Detail strat----------------------***/
function ItemStockBatchWise(el, evt) {
    debugger;
    var QtyDigit = $("#QtyDigit").text()
    var currentrow = $(evt.target).closest('tr');
    var SNohf = currentrow.find("#SNohiddenfiled").val();
    var item_id = currentrow.find("#hfItemID").val();
    var ExpireAble_item = currentrow.find("#hfexpiralble").val();
    if (ExpireAble_item == "Y") {
        $("#spanexpiryrequire").show();
    }
    else {
        $("#spanexpiryrequire").hide();
    }
    var uom_name = currentrow.find("#UOM" + " option:selected").text();
    var ReceivedQuantity = currentrow.find("#ord_qty_spec").val();
    var Item_name = currentrow.find("#POItemListName" + SNohf + " option:selected").text();
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
            $("#BtnBatchDetail").attr("data-target", "#ExternalReceiptBatchNumber");
        }
        var rowIdx = 0;
        var Count = $("#SaveItemBatchTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                $("#BatchDetailTbl >tbody >tr").remove();
                $("#SaveItemBatchTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    let mfg_date = "";
                    var SUserID = SaveBatchRow.find("#BatchUserID").val();
                    var SRowID = SaveBatchRow.find("#RowSNo").val();
                    var SItemID = SaveBatchRow.find("#ItemID").val();
                    var BatchExDate = SaveBatchRow.find("#BatchExDate").val();
                    var BatchQty = SaveBatchRow.find("#BatchQty").val();
                    var BatchNo = SaveBatchRow.find("#BatchNo").val(); 
                    let MfgName = SaveBatchRow.find("#bt_mfg_name").text();
                    let MfgMrp = parseFloat(CheckNullNumber(SaveBatchRow.find("#bt_mfg_mrp").val())).toFixed(RateDecDigit);
                    let MfgDate = SaveBatchRow.find("#bt_mfg_date").val();
                    if (MfgDate != "" && MfgDate != null) {
                        if (MfgDate == "1900-01-01") {
                            mfg_date = "";
                        }
                        else {
                            mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                        }
                    }

                    if (SNohf != null && SNohf != "") {
                        if (SItemID == item_id) {
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
                            <td id="bt_MfgDate" >${mfg_date}</td>
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
                            <td id="bt_MfgDate" >${mfg_date}</td>
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
function OnKeyPressBatchNoExter_rec(e) {
    debugger
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
}
function OnClickAddNewBatchDetailExternalReceipt() {
    debugger;
    var BatchNumber = $("#txtBatchNumber").val();
    var BatchExpiryDate = $("#BatchExpiryDate").val();
    var BatchReceivedQuantity = $("#BatchReceivedQuantity").val();
    let MfgName = $("#BtMfgName").val();
    let MfgMrp = parseFloat(CheckNullNumber($("#BtMfgMrp").val())).toFixed(RateDecDigit);
    let MfgDate = $("#BtMfgDate").val();
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
    if (BatchReceived_qty == "" || BatchReceived_qty == null || parseFloat(BatchReceived_qty) == parseFloat(0)) {
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
    if (MfgDate != null && MfgDate != "") {
        MfgDate = moment(MfgDate).format('DD-MM-YYYY');
    }
    else {
        MfgDate = "";
    }
    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
<td id="bt_MfgName" >${MfgName}</td>
<td id="bt_MfgMrp" class="num_right">${MfgMrp}</td>
<td id="bt_MfgDate" >${MfgDate}</td>
<td id="bt_hdn_MfgDate" hidden="hidden">${$("#BtMfgDate").val()}</td>
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
function OnChangeBatchNumber(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    $("#txtBatchNumber").val(data)
}
function OnKeyDownBatchNoExter_rec(e) {
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeBatchQtyExternal() {
    debugger;
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
function OnClickSaveAndCloseGRN() {
    debugger;
    var RowSNo = $("#hfItemSNo").val();
    var ItemID = $('#BatchItemID').val();
    var ReceiBQty = parseFloat($("#BatchReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQtyTotal").text()).toFixed(QtyDecDigit);
    if (parseFloat(ReceiBQty) == parseFloat(TotalBQty)) {
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        ValidateEyeColor($("#POItemListName" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
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
/***------------------Add for Batch Detail End-------------------***/
/***-----------------Add for Serial Detail strat-------------------***/
function ItemStockSerialWise(evt, e) {
    var Disabled = $("#DisableSubItem").val();
    if (Disabled == "Y") {
        $("#DivDownloadImportExcelSerial").hide();
    }

    $("#SerialNo").val("");
    $("#SrMfgName").val("");
    $("#SrMfgMrp").val("");
    $("#SrMfgDate").val("");
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
    var QtyDecDigit = $("#QtyDigit").text();
    var currentrow = $(e.target).closest("tr");
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
    var SNohf = currentrow.find("#SNohiddenfiled").val();
    ItemID = currentrow.find("#hfItemID").val();
    ItemUOM = currentrow.find("#UOM").text();
    ReceiveQty = currentrow.find("#ord_qty_spec").val();
    ItemName = currentrow.find("#POItemListName" + SNohf + " option:selected").text();
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
    $("#hfItemSNo").val(SNohf);
    $("#hfSItemID").val(ItemID);
    var rowIdx = 0;
    var Count = $("#SaveItemSerialTbl >tbody >tr").length;
    if (Count != null) {
        if (Count > 0) {
            $("#SerialDetailTbl >tbody >tr").remove();
            $("#SaveItemSerialTbl >tbody >tr").each(function () {
                let SerialCurrentRow = $(this);
                let SItemID = SerialCurrentRow.find("#SerialItemID").val();
                let SerialNo = SerialCurrentRow.find("#Serial_SerialNo").val();
                let MfgName = SerialCurrentRow.find("#sr_mfg_name").text();
                let MfgMrp = parseFloat(CheckNullNumber(SerialCurrentRow.find("#sr_mfg_mrp").val())).toFixed(RateDecDigit);
                let MfgDate = SerialCurrentRow.find("#sr_mfg_date").val();
                let mfg_date = "";
                if (MfgDate != "" && MfgDate != null) {
                    if (MfgDate == "1900-01-01") {
                        mfg_date = "";
                    }
                    else {
                        mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                    }
                }
                if (SNohf != null && SNohf != "") {
                    if (SItemID == ItemID) {
                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                            <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            <td id="SerialID" >${rowIdx}</td>
                            <td id="SerialNo" >${SerialNo}</td>
                            <td id="sr_MfgName" >${MfgName}</td>
                            <td id="sr_MfgMrp" class="num_right">${MfgMrp}</td>
                            <td id="sr_MfgDate" >${mfg_date}</td>
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
                            <td id="sr_MfgDate" >${mfg_date}</td>
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
function OnClickAddNewSerialDetail_ExternalReceipt() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var AcceptLength = parseInt(0);
    $("#SerialDetailTbl >tbody >tr").each(function () {
        AcceptLength = parseInt(AcceptLength) + 1;
    });
    //AcceptLength = $("#SerialDetailTbl >tbody >tr").length();
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
            let mfg_date = "";
            if (MfgDate != null && MfgDate != "") {
                mfg_date = moment(MfgDate).format('DD-MM-YYYY');
            }
            else {
                mfg_date = "";
            }
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
               <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
               <td id="SerialID" >${TblLen + 1}</td>
               <td id="SerialNo" >${$("#SerialNo").val()}</td>
                <td id="sr_MfgName" >${MfgName}</td>
                <td id="sr_MfgMrp" class="num_right">${MfgMrp}</td>
                <td id="sr_MfgDate" >${mfg_date}</td>
                <td id="sr_hdn_MfgDate" hidden="hidden">${MfgDate}</td>
               </tr>`);
            $('#SerialNo').val("");
            //$('#SrMfgName').val("");
            //$('#SrMfgMrp').val("");
            //$('#SrMfgDate').val("");
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
    var ItemID = $('#hfSItemID').val();
    var RowSNo = $("#hfItemSNo").val();
    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalRecQty = parseFloat(ReceiSQty);
    var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);
    if (parseFloat(TotalRecQty) == parseFloat(TotalSQty)) {
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "modal");
        ValidateEyeColor($("#POItemListName" + RowSNo).closest("tr"), "BtnSerialDetail", "N");
        var SelectedItem = $("#hfSItemID").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#SerialItemID").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#SerialDetailTbl >tbody >tr").each(function () {
            let currentRow = $(this);
            let SerialNo = currentRow.find("#SerialNo").text();
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
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }
}
function OnClickSerialResetBtn_ExternalReceipt() {
    $('#SerialNo').val("");
    $('#SrMfgName').val("");
    $('#SrMfgMrp').val("");
    $('#SrMfgDate').val("");
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
/***-------------Add for Serial Detail End-------------***/
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
/***------------Add for Other Detail start------------***/
function OnchangePriceBasis() {
    if ($("#PriceBasis").val() == "") {
        $('#SpanPriceBasisErrorMsg').text($("#valueReq").text());
        $("#SpanPriceBasisErrorMsg").css("display", "block");
        $("#PriceBasis").css("border-color", "Red");
    }
    else {
        $("#SpanPriceBasisErrorMsg").css("display", "none");
        $("#PriceBasis").css("border-color", "#ced4da");
    }
}
function OnchangeFreightType() {
    if ($("#FreightType").val() == "") {
        $('#SpanFreightTypeErrorMsg').text($("#valueReq").text());
        $("#SpanFreightTypeErrorMsg").css("display", "block");
        $("#FreightType").css("border-color", "Red");
    }
    else {
        $("#SpanFreightTypeErrorMsg").css("display", "none");
        $("#FreightType").css("border-color", "#ced4da");
    }
}
function OnchangeModeOfTransport() {
    if ($("#ModeOfTransport").val() == "") {
        $('#SpanModeOfTransportErrorMsg').text($("#valueReq").text());
        $("#SpanModeOfTransportErrorMsg").css("display", "block");
        $("#ModeOfTransport").css("border-color", "Red");
    }
    else {
        $("#SpanModeOfTransportErrorMsg").css("display", "none");
        $("#ModeOfTransport").css("border-color", "#ced4da");
    }
}
function OnchangeDestination() {
    if ($("#Destination").val() == "") {
        $('#SpanDestinationErrorMsg').text($("#valueReq").text());
        $("#SpanDestinationErrorMsg").css("display", "block");
        $("#Destination").css("border-color", "Red");
    }
    else {
        $("#SpanDestinationErrorMsg").css("display", "none");
        $("#Destination").css("border-color", "#ced4da");
    }
}
/***-----------Add for Other Detail End----------------***/
function OnClickSaveAndExit_OC_Btn() {
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueInBase", "#NetOrderValueInBase")
}
function BindOtherChargeDeatils(val) {
    debugger;
    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#Total_OC_Amount").text(parseFloat(0).toFixed(ValDecDigit));
    $("#TxtOtherCharges").val(parseFloat(0).toFixed(ValDecDigit));

    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            //if (DocumentMenuId == "105101152") {
            td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            //}
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td >${currentRow.find("#td_OCSuppName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(ValDecDigit);
            TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
            TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
        });
        if (val == "") {
            let SuppId = $("#SupplierName").val();
            let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
            if (GrossAmt != 0) {
                AutoTdsApply(SuppId, GrossAmt).then(() => {
                    GetAllGLID();
                });
            }
            else {
                GetAllGLID();
            }
        }
    }
    else {
        GetAllGLID();
    }
    $("#TxtOtherCharges").val(TotalAMountWT);
    $("#_OtherChargeTotalTax").text(TotalTaxAMount);
    $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    $("#_OtherChargeTotal").text(TotalAMount);
}
/***---------------------------GL Voucher Entry Start-----------------------------***/
function GetAllGLID() {
    GetAllGL_WithMultiSupplier();
}
async function GetAllGL_WithMultiSupplier() {
    debugger;   
    if ($("#POInvItmDetailsTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#tbladdhdn tbody tr").remove();
        //$("#tbladd tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    if (CheckDPI_ItemValidations("Y") == false) {
        return false;
    }
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    var NetInvValue = $("#NetOrderValueInBase").val();
    var conv_rate = $("#conv_rate").val();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    var curr_id = $("#curr_id").val();
    var bill_no = $("#Bill_No").val();
    var bill_dt = $("#Bill_Date").val();
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
    });
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        var ItmGrossVal = currentRow.find("#item_gr_val").val();
        var ItmGrossValInBase = currentRow.find("#item_gr_val").val();
        var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
        var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExantedItemList.push({ item_id: item_id});
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExantedItemList.push({ item_id: item_id });
            }
        }
        GLDetail.push({
            comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
            , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: item_acc_id
        });
    });
    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
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

        GLDetail.push({
            comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt
            , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate
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
                comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: InvType, Value: oc_amt_wt
                , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: oc_supp_bill_no
                , bill_date: oc_supp_bill_dt, acc_id: ""
            });
        }
        else {

        }
    });
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var oc_id = currentRow.find("#TaxItmCode").text();
        var TaxPerc = currentRow.find("#TaxPercentage").text();
        var TaxPerc_id = TaxPerc.replace("%", "");
        var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
        GLDetail.push({
            comp_id: Compid, id: tax_id, type: "OcTax", doctype: InvType, Value: tax_amt
            , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: ArrOcGl[0].id
            , Entity_id: tax_acc_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate,
            bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
        });

        if (GstCat == "UR") { /* Added by Suraj Maurya on 04-08-2025 to Add in RCM all tax on OC for Self Supplier */
            tax_id = "R" + tax_id;
            $("#ht_Tbl_OC_Deatils >tbody >tr").find("#OC_ID:contains(" + oc_id + ")").closest('tr').each(function () {
                if ($(this).find("#td_OCSuppID").text() == "0" && $(this).find("#OC_ID").text() == oc_id) {
                    GLDetail.push({
                        comp_id: Compid, id: tax_id, type: "RCM", doctype: InvType, Value: tax_amt
                        , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                    });
                }
            });
        }

    });
    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();

        if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
            var GstApplicable = $("#Hdn_GstApplicable").text();
            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");//add by shubham maurya on 27-03-2025
            if (TaxExempted == false) {//add by shubham maurya on 27-03-2025
                var ClaimItc = true;
                if (GstApplicable == "Y") {
                    ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
                }
                if (TaxRecov == "N" || !ClaimItc) {
                    if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
                        var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
                        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
                        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
                    }
                } else {
                    GLDetail.push({
                        comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                    });
                }
            }
        }
    });
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();

            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
            if (ClaimITC) {
                if (TaxRecov == "N") {

                }
                else {
                    if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
                        if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
                            objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
                            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                        } else {
                            GLDetail.push({
                                comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal
                                , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                                , Entity_id: TaxAccID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""

                            });
                        }
                    }
                }
            }
        });
        /* Added by Suraj Maurya on 04-08-2025 to Add in RCM all tax on OC for Self Supplier */
        //$("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        //    var currentRow = $(this);
        //    var tax_id = currentRow.find("#TaxNameID").text();
        //    tax_id = "R" + tax_id;
        //    var tax_acc_id = currentRow.find("#TaxAccId").text();
        //    var tax_amt = currentRow.find("#TaxAmount").text();
        //    var oc_id = currentRow.find("#TaxItmCode").text();
        //    var TaxPerc = currentRow.find("#TaxPercentage").text();
        //    var TaxPerc_id = TaxPerc.replace("%", "");

        //    $("#ht_Tbl_OC_Deatils >tbody >tr").find("#OC_ID:contains(" + oc_id + ")").closest('tr').each(function () {
        //        if ($(this).find("#td_OCSuppID").text() == "0" && $(this).find("#OC_ID").text() == oc_id) {
        //            GLDetail.push({
        //                comp_id: Compid, id: tax_id, type: "RCM", doctype: InvType, Value: tax_amt
        //                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
        //                , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        //            });
        //        }
        //    });
        //});
        /* Added by Suraj Maurya on 04-08-2025 to Add in RCM all tax on OC for Self Supplier */
    }
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
        GLDetail.push({
            comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
            , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: supp_acc_id
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    });
    if (Cal_Tds_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "TSupp", doctype: InvType
            , Value: Cal_Tds_Amt, ValueInBase: Cal_Tds_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }
    var Oc_Tds = [];
    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        var tds_supp_id = currentRow.find("#td_TDS_Supp_Id").text();
        var ArrOcGl = GLDetail.filter(v => (v.id == tds_supp_id && v.type == "Supp"));
        var tdsIndex = Oc_Tds.findIndex(v => v.supp_id == tds_supp_id);
        if (tdsIndex > -1) {
            Oc_Tds[tdsIndex].tds_amt = parseFloat(Oc_Tds[tdsIndex].tds_amt) + parseFloat(tds_amt);
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate, bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        } else {
            Oc_Tds.push({
                supp_id: tds_supp_id, supp_acc_id: ArrOcGl[0].id, tds_amt: tds_amt
                , bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate, bill_no: ArrOcGl[0].bill_no
                , bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        }
    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item, idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: item.bill_no, bill_date: item.bill_date, acc_id: ""
            });
        });
    }
    await Cmn_GLTableBind(supp_acc_id, GLDetail, "Purchase")    
}
function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType, curr_id, conv_rate, bill_bo, bill_date) {
    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }
    if (parseFloat(DrValue) < 0) {
        CrValue = Math.abs(DrValue);
        DrValue = 0;
    }
    if (parseFloat(DrValueInBase) < 0) {
        CrValueInBase = Math.abs(DrValueInBase);
        DrValueInBase = 0;
    }
    if (parseFloat(CrValue) < 0) {
        DrValue = Math.abs(CrValue);
        CrValue = 0;
    }
    if (parseFloat(CrValueInBase) < 0) {
        DrValueInBase = Math.abs(CrValueInBase);
        CrValueInBase = 0;
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
async function AddRoundOffGL() {
    debugger;
    //var curr_id = $("#ddlCurrency").val();
    var curr_id = $("#curr_id").val();
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                debugger;
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    $("#VoucherDetail >tbody >tr").each(function () {
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
                    debugger;
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
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
                                        , Diff, DiffInBase, 0, 0, "PV", curr_id
                                        , $("#conv_rate").val()
                                        , $("#Bill_No").val(), $("#Bill_Date").val())

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
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
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
                                        , 0, 0, Diff, DiffInBase, "PV"
                                        , curr_id, $("#conv_rate").val(), $("#Bill_No").val()
                                        , $("#Bill_Date").val())
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
                    debugger;
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
async function CalculateVoucherTotalAmount() {
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
        debugger;
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }
    }

}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Nurration").val() : VouType == "BP" ? $("#hdn_BP_Nurration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
/***---------------------------GL Voucher Entry End-----------------------------***/
/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#POItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_gr_val").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
        CalculateTaxExemptedAmt(e, "N");
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else {
            GetAllGLID();
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#ship_add_gstNo").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val", "","TaxCalcItemCode")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    var currentrow = $(e.target).closest('tr');
    var AssAmount = currentrow.find("#item_gr_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find('#TaxExempted').prop("checked", false);
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        CalculateTaxExemptedAmt(e, "N")
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else {
            GetAllGLID();
        }
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#Txt_item_tax_rec_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(0).toFixed(ValDecDigit));
        let DPIQty = currentrow.find("#ord_qty_spec").val();
        let OC_Amt = currentrow.find("#TxtOtherCharge").val();
        debugger;
        debugger;
        let LandedCostValue = parseFloat(CheckNullNumber(AssAmount)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(0));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
        currentrow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentrow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val", "", "TaxCalcItemCode")
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
function CalculateTaxExemptedAmt(e, flag) {
    var clickedrow = $(e.target).closest("tr");
    if (flag == "Row") {
        clickedrow = e;//setting row to the clickedrow sent by the Previous method
    }
    var ItemName = clickedrow.find("#hfItemID").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var oc_amt = parseFloat(CheckNullNumber(clickedrow.find("#TxtOtherCharge").val()));
    var FinVal = parseFloat(CheckNullNumber(clickedrow.find("#item_gr_val").val())).toFixed(ValDecDigit);
    if (parseFloat(CheckNullNumber(FinVal)) > 0) {
        var FinalNetValue = parseFloat(FinVal) + oc_amt;
        clickedrow.find("#item_net_val_spec").val(FinalNetValue.toFixed(ValDecDigit));
        //clickedrow.find("#item_net_val_bs").val(FinalNetValue.toFixed(ValDecDigit));
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        if (GstApplicable == "Y") {
            if (clickedrow.find("#ManualGST").is(":checked")) {
                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            }
            else {
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
            }
        } else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        }
    }
}
/***--------------------------Sub-Item Start----------------------------------------***/
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#POItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#POItemListName" + hfsno).val();
    var UOMId = clickdRow.find("#UOM option:selected").val();
    var UOM = clickdRow.find("#UOM option:selected").text();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var IsDisabled = $("#DisableSubItem").val();

    var Sub_Quantity = 0;
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        NewArr.push(List);
    });
    if (flag == "QuantitySpec") {
        Sub_Quantity = clickdRow.find("#ord_qty_spec").val();

    }
    var src_type = "D";
    var hd_Status = $("#hfPIStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DirectPurchaseInvoice/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            src_type: src_type,
            UOMId: UOMId
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
    return Cmn_CheckValidations_forSubItems("POInvItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemDPIQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("POInvItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemDPIQty", "N");
}
/*----------------------------TDS Section-----------------------------*/
function OnClickTDSCalculationBtn() {
    debugger;
    const GrVal = $("#TxtGrossValue").val();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal)).toFixed(ValDecDigit);
    const NetVal = $("#NetOrderValueInBase").val();
    const ToTdsAmt_IT = parseFloat(CheckNullNumber(NetVal)).toFixed(ValDecDigit);
    $("#hdn_tds_on").val("D");
    $("#TDS_AssessableValue").val(ToTdsAmt);
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, null, null, ToTdsAmt_IT, "TDS");/* "TDS" passed by Suraj Maurya on 14-05-2025 */

}
function OnClickTDS_SaveAndExit() {
    debugger
    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {
        let TdsAssVal_applyOn = "ET";
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
        }
        else if ($("#TdsAssVal_Custom").is(":checked")) {/* Added by Suraj Maurya on 14-05-2025 */
            TdsAssVal_applyOn = "CA"
        }
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        var rowIdx = 0;
        if ($("#Hdn_TDS_CalculatorTbl >tbody >tr").length > 0) {
            $("#Hdn_TDS_CalculatorTbl >tbody >tr").remove();
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
            });
        }
        else {
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $("#Hdn_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
            });
        }
        var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
        });
        $("#TxtTDS_Amount").val(TotalAMount);
        if (($("#TDS_CalculatorTbl > tbody > tr").length > 0) && (TdsAssVal_applyOn == "CA") && $("#TxtTDS_Amount").val() == 0) {
            debugger;
            if ($("#TDS_AssessableValueCustom").val() === "0" || $("#TDS_AssessableValueCustom").val() === "0.00" || $("#TDS_AssessableValueCustom").val() == "") {
                $('#SpanTDS_AssessableValueCustomErrorMsg').text($("#valueReq").text());
                $("#TDS_AssessableValueCustom").css("border-color", "Red");
                $("#SpanTDS_AssessableValueCustomErrorMsg").css("display", "block");
                return;
            }
            else {
                $("#SpanTDS_AssessableValueCustomErrorMsg").css("display", "none");
                $("TDS_AssessableValueCustom").css("border-color", "#ced4da");
            }

        }
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        GetAllGLID();
    }
}

/*----------------------------TDS Section End-----------------------------*/
/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e) {
    const row = $(e.target).closest("tr");
    debugger;
    CC_Clicked_Row = row;
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const TotalVal = CC_Clicked_Row.find("#OCTotalTaxAmt").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    $("#TDS_AssessableValue").val(parseFloat(CheckNullNumber(ToTdsAmt)).toFixed(ValDecDigit));

    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, TotalVal, "OcTds");/* "OcTds" passed by Suraj Maurya on 15-05-2025 */

}
function OnClickTP_TDS_SaveAndExit() {
    debugger
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
    }
    if ($("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").length > 0) {
        $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function () {
            let row = $(this);
            if (row.find("#td_TDS_OC_Id").text() == TDS_OcId) {
                $(this).remove();
            }
        });
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $('#Hdn_OC_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });
    }
    else {
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $("#Hdn_OC_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });

    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
}
function SetTds_Amt_To_OC() {
    var TotalAMount = parseFloat(0);
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
    });
    CC_Clicked_Row.find("#OC_TDSAmt").text(parseFloat(CheckNullNumber(TotalAMount)).toFixed(ValDecDigit));
    CC_Clicked_Row = null;
}
/***------------------------------TDS On Third Party End------------------------------***/
/***-------------------Roundoff------------------------------------***/
function checkMultiSupplier() {
    return true;
}
function click_chkroundoff() {
    debugger;
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

            $("#POInvItmDetailsTbl > tbody > tr").each(function () {
                debugger;
                var currentrow = $(this);
                //CalculateTaxExemptedAmt(currentrow, "N")
                CalculateTaxExemptedAmt(currentrow, "Row")
            });
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
function click_chkplusminus(AutoAplyTaxPI) {
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    if ($("#chk_roundoff").is(":checked")) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                    data: {},
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            PO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {
                                if (parseInt(arr[0]["r_acc"]) > 0) {


                                    var grossval = $("#TxtGrossValue").val();
                                    var taxval = $("#TxtTaxAmount").val();
                                    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

                                    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

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
                                                var decval = '0.' + finnetval[1];
                                                fnetval = parseFloat(netval) - parseFloat(decval);
                                                $("#pm_flagval").val($("#m_round").val());
                                            }
                                            var roundoff_netval = Math.round(fnetval);
                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                            $("#NetOrderValueInBase").val(f_netval);
                                            GetAllGLID();
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
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
        if (AutoAplyTaxPI != "AutoAplyTaxPI") {
            $("#POInvItmDetailsTbl > tbody > tr").each(function () {
                debugger;
                var currentrow = $(this);
                //CalculateTaxExemptedAmt(currentrow, "N")
                CalculateTaxExemptedAmt(currentrow, "Row")
            });
        }
        GetAllGLID();
    }
}
/***----------------------end-------------------------------------***/
//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
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
    var disableflag = ($("#txtdisable").val());
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
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDecDigit);
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
    var DocMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            DocumentMenuId: DocMenuId
        },
        success: function (data) {
            debugger;
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
/***-------------------For Workflow Start----------------***/
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
function ForwardBtnClick() {
    debugger;
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DrPinvDate = $("#Inv_date").val();
    $.ajax({
        type: "POST",
        /*  url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DrPinvDate
        },
        success: function (data) {
            /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var INVStatus = "";
                INVStatus = $('#hfPIStatus').val().trim();
                if (INVStatus === "D" || INVStatus === "F") {

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
                /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");

            }
        }
    });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var INV_NO = "";
    var INVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var VoucherNarr = $("#hdn_Nurration").val()
    var BP_VoucherNarr = $("#hdn_BP_Nurration").val()
    var DN_VoucherNarr = $("#hdn_DN_Nurration").val()
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#InvoiceNumber").val();
    INVDate = $("#Inv_date").val();
    WF_Status1 = $("#WF_Status1").val();
    //var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    var TrancType = (INV_NO + ',' + INVDate + ',' + "Update" + ',' + WF_Status1 + ',' + docid)
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();
    var ListFilterData1 = $("#ListFilterData1").val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = INV_NO.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/DirectPurchaseInvoice/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DirectPurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
           //window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/DirectPurchaseInvoice/ApproveDPIDetails?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&VoucherNarr=" + VoucherNarr + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + WF_Status1 + "&Bp_Nurr=" + BP_VoucherNarr + "&Dn_Nurration=" + DN_VoucherNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DirectPurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DirectPurchaseInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
            //showLoader();
            //window.location.reload();
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        var Doc_Status = $("#hfPIStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "POItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
/***-------------------For Workflow End----------------***/
//function GetPdfFilePathToSendonEmailAlert(spoNo, spoDate, fileName) {
//    debugger;
//    var Inv_no = spoNo;
//    var InvDate = spoDate;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/DirectPurchaseInvoice/SavePdfDocToSendOnEmailAlert",
//        data: { poNo: Inv_no, poDate: InvDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
//Added by Nidhi on 02-09-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var supp_id = $("#SupplierName").val();
    Cmn_SendEmail(docid, supp_id, 'Supp');
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/DirectPurchaseInvoice/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var pdfAlertEmailFilePath = 'DPI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DirectPurchaseInvoice/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath },
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
    }
}
function EmailAlertLogDetails() {
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//End on  02-09-2025
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
function OnChangeMfgName(e) {
    debugger;
    var data = e.target.value;

    data = data.replaceAll("\\", "{\\}");
    data = data.replaceAll("\t", "");
    data = data.replaceAll("{\\}", "\\");
    e.target.value = data;
}
