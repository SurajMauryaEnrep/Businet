
/*-----------------Modified by Suraj Maurya on 25-10-2024-------------------*/
const ExchDecDigit = $("#ExchDigit").text();///Amount
const ord_type_imp = $("#OrderTypeE").is(":checked");///Amount
const QtyDecDigit = ord_type_imp ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();///Quantity
//const RateDecDigit = ord_type_imp ? $("#ExpImpRateDigit").text() : $("#RateDigit").text();///Rate And Percentage
const RateDecDigit = ord_type_imp ? $("#ExpImpRateDigit").text() : $("#PurCostingRateDigit").text();///Rate And Percentage
const ValDecDigit = ord_type_imp ? $("#ExpImpValDigit").text() : $("#ValDigit").text();///Amount
/*-----------------Modified by Suraj Maurya on 25-10-2024 End-------------------*/
$(document).ready(function () {
    debugger;
    var Supp_id = $("#SupplierName").val();
    if ($("#InvoiceNumber").val() == "" || $("#InvoiceNumber").val() == null) {
        BindGoodReceiptNoteLists(Supp_id);
    }

    $("#SupplierName").select2();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#InvoiceNumber").val() == "" || $("#InvoiceNumber").val() == null) {
        $("#Inv_date").val(CurrentDate);
    }
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    $('#POInvItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        ////debugger;
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
        //debugger;
        //var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";

        ItemCode = $(this).closest('tr').find("#hfItemID").val();

        ////var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        // ShowItemListItm(ItemCode);
        CalculateAmount();
        //var TOCAmount = parseFloat($("#PO_OtherCharges").val());
        //if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        //TOCAmoun = 0;
        //}
        //Calculate_OC_AmountItemWise(TOCAmount);
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        SerialNoAfterDelete();
        var docid = $("#DocumentMenuId").val();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValueInBase").val();
        if (docid == "105101145") {
            var CustomTds = $('#Hdn_TDS_CalculatorTbl > tbody > tr #td_TDS_AssValApplyOn:contains("CA")').closest('tr');
            if (CustomTds.length == 0) {/* Added by Suraj Maurya on 14-05-2025 */
                AutoTdsApply(SuppId, GrossAmt).then(() => {
                    CalculateVoucherTotalAmount();
                    GetAllGLID();
                });
            }
        }
        //GetAllGLID();
        //AfterDeleteResetPO_ItemTaxDetail();
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });
    debugger;
    InvNo = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvNo);
    BindDDLAccountList();
    LoadVarData();
    CancelledRemarks("#Cancelled", "Disabled");
});
function LoadVarData() {
    const var_qty_details = $("#hdn_var_qty_details").val();
    if (var_qty_details != null && var_qty_details != "") {
        sessionStorage.setItem("VarianceDataList", var_qty_details);
    }
    else {
        sessionStorage.removeItem("VarianceDataList");
    }
    const var_qty_tax_details = $("#hdn_var_tax_details").val();
    if (var_qty_tax_details != null && var_qty_tax_details != "") {
        sessionStorage.setItem("VarianceTaxDataList", var_qty_tax_details);
    } else {
        sessionStorage.removeItem("VarianceTaxDataList");
    }
    CalcVarianceDnAmount();
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
//var ValDecDigit = $("#ValDigit").text();///Amount
//var RateDecDigit = $("#RateDigit").text();
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105101145");
}
function BindData() {
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
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
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
        if (AccID != '0' && AccID != "") {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
function RateFloatValueonly(el, evt) {
    let RateDigit = ord_type ? "#ExpImpRateDigit" : "#RateDigit";
    if (CmnAmtFloatVal(el, evt, RateDigit) == false) {
        return false;
    }
    else {
        return true;
    }
}
function OnchangeConvRate(e) {
    //var RateDecDigit = $("#RateDigit").text();
    var ConvRate;
    ConvRate = $("#conv_rate").val();

    if (AvoidDot(ConvRate) == false) {
        $("#conv_rate").val("");
        $("#hdnconv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
    } else {
        $("#conv_rate").val(parseFloat(ConvRate).toFixed(ExchDecDigit));
        $("#hdnconv_rate").val(parseFloat(ConvRate).toFixed(ExchDecDigit));
    }
}
function InsertPIDetails() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var Dmenu = $("#DocumentMenuId").val();
    ////var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");
    if (CheckPI_Validations() == false) {
        return false;
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
    if (CheckPI_ItemValidations() == false) {
        //swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y" && Dmenu == "105101145") {
        if (Cmn_taxVallidation("POInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
            return false;
        }
    }
    if (CheckPI_VoucherValidations() == false) {
        return false;
    }
    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
    debugger;
    var TransType = "";
    if (INSDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var docCurr = $("#ddlCurrency").val();
    $("#Hdn_ddlCurrency").val(docCurr);

    debugger;
    $("#SupplierName").attr("disabled", false);

    var Narration = $("#DebitNoteRaisedAgainstInv").text()
    $('#hdNarration').val(Narration);

    var FinalItemDetail = [];
    FinalItemDetail = InsertPIItemDetails();
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
    vou_Detail = GetPI_VoucherDetails();
    var str_vou_Detail = JSON.stringify(vou_Detail);
    $('#hdItemvouDetail').val(str_vou_Detail);

    ///*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    ///*-----------Sub-item end-------------*/

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
    $('#hdn_oc_tds_details').val(Oc_Tds_Details);

    var Final_var_Details = [];
    Final_var_Details = Insert_var_Details();//retruns stringified data;
    $('#hdn_var_qty_details').val(Final_var_Details);

    var Final_var_tax_Details = [];
    Final_var_tax_Details = Insert_var_tax_Details();//retruns stringified data;
    $('#hdn_var_tax_details').val(Final_var_tax_Details);

    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1", "hfPIStatus") == false) {//modified by Suraj Maurya on 05-06-2025 to change dramtInBase1 --> dramt1
        return false;
    }
    $("#RCMApplicable").prop("disabled", false);

    $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
    var Suppname = $('#SupplierName option:selected').text();
    $("#Hdn_PInvSuppName").val(Suppname);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
}
function InsertPIItemDetails() {

    var PI_ItemsDetail = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var mr_no = "";
        var mr_date = "";
        var item_id = "";
        var item_name = "";
        var subitem = "";
        var uom_id = "";
        var uom_name = "";
        var mr_qty = "";
        var item_rate = "";
        var item_gr_val = "";
        var item_gr_val_bs = "";
        var item_tax_amt = "";
        var item_oc_amt = "";
        var item_net_val_spec = "";
        var item_net_val_bs = "";
        var gl_vou_no = null;
        var gl_vou_dt = null;
        var TaxExempted = "";
        var hsn_code = "";
        var ManualGST = "";
        var ClaimITC = "";
        var item_acc_id = "";
        var item_var_dn_amt = "";
        var ItemType = "";
        var currentRow = $(this);
        var srno = currentRow.find("#srno").text();
        mr_no = currentRow.find("#TxtGrnNo").val();
        mr_date = currentRow.find("#hfGRNDate").val();
        item_id = currentRow.find("#hfItemID").val();
        item_name = currentRow.find("#TxtItemName").val();
        subitem = currentRow.find("#sub_item").val();

        var uomid = currentRow.find("#hfUOMID").val();
        ItemType = currentRow.find("#hdnItemType").val();
        if (ItemType == "Service") {
            if (uomid != "0" && uomid != "" && uomid != null && uomid != "NaN") {
                uom_id = uomid;
            }
            else {
                uom_id = "0";
            }

        }
        else {
            uom_id = uomid;
        }
        uom_name = currentRow.find("#TxtUOM").val();
        mr_qty = currentRow.find("#TxtReceivedQuantity").val();
        item_rate = currentRow.find("#TxtRate").val();
        item_gr_val = currentRow.find("#TxtItemGrossValue").val();
        item_gr_val_bs = currentRow.find("#TxtItemGrossValueInBase").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        hsn_code = currentRow.find("#ItemHsnCode").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        item_var_dn_amt = currentRow.find("#TxtDebitNoteValue").val();//Added by Suraj Maurya on 31-03-2025
        item_var_dn_amt = (item_var_dn_amt == null || item_var_dn_amt == "" || item_var_dn_amt == "NaN") ? "0" : item_var_dn_amt;
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        item_net_val_spec = currentRow.find("#TxtNetValueInBase").val();
        item_net_val_bs = currentRow.find("#TxtNetValueInBase").val();
        gl_vou_no = null;
        gl_vou_dt = null;
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
        var PackSize = currentRow.find("#PackSize").val();
        var mrp = currentRow.find("#mrp").val();
        PI_ItemsDetail.push({
            mr_no: mr_no, mr_date: mr_date, item_id: item_id, item_name: item_name, subitem: subitem
            , uom_id: uom_id, uom_name: uom_name, mr_qty: mr_qty, item_rate: item_rate, item_gr_val: item_gr_val
            , item_gr_val_bs: item_gr_val_bs, item_tax_amt: item_tax_amt, item_oc_amt: item_oc_amt
            , item_net_val_spec: item_net_val_spec, item_net_val_bs: item_net_val_bs, gl_vou_no: gl_vou_no
            , gl_vou_dt: gl_vou_dt, TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, ClaimITC: ClaimITC
            , item_acc_id: item_acc_id, item_var_dn_amt: item_var_dn_amt, srno: srno, mrp: mrp, PackSize: PackSize
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
            var TxtGrnNo = currentRow.find("#TxtGrnNo").val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").find("#DocNo:contains('" + TxtGrnNo + "')").closest("tr").each(function () {
                        var mr_no = "";
                        var mr_date = "";
                        var item_id = "";
                        var tax_id = "";
                        var TaxName = "";
                        var tax_rate = "";
                        var tax_level = "";
                        var tax_val = "";
                        var tax_apply_on = "";
                        var tax_apply_onName = "";
                        var totaltax_amt = "";
                        debugger;
                        var currentRow = $(this);
                        mr_no = currentRow.find("#DocNo").text();
                        mr_date = currentRow.find("#DocDate").text();
                        item_id = currentRow.find("#TaxItmCode").text();
                        tax_id = currentRow.find("#TaxNameID").text();
                        TaxName = currentRow.find("#TaxName").text().trim();
                        tax_rate = currentRow.find("#TaxPercentage").text();
                        tax_level = currentRow.find("#TaxLevel").text();
                        tax_val = currentRow.find("#TaxAmount").text();
                        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
                        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
                        totaltax_amt = currentRow.find("#TotalTaxAmount").text();
                        tax_recov = currentRow.find("#TaxRecov").text();

                        TaxDetails.push({ mr_no: mr_no, mr_date: mr_date, item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt, totaltax_amt, tax_recov: tax_recov });
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
        debugger;
        var mr_no = "";
        var mr_date = "";
        var item_id = "";
        var tax_id = "";
        var TaxName = "";
        var tax_rate = "";
        var tax_level = "";
        var tax_val = "";
        var tax_apply_on = "";
        var tax_apply_onName = "";
        var totaltax_amt = "";
        debugger;
        var currentRow = $(this);
        mr_no = "";//$("#ddlGoodReceiptNoteNo option:selected").text();
        mr_date = "";//$("#GRN_Date").val();
        item_id = currentRow.find("#TaxItmCode").text();
        tax_id = currentRow.find("#TaxNameID").text();
        TaxName = currentRow.find("#TaxName").text().trim();
        tax_rate = currentRow.find("#TaxPercentage").text();
        tax_level = currentRow.find("#TaxLevel").text();
        tax_val = currentRow.find("#TaxAmount").text();
        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
        totaltax_amt = currentRow.find("#TotalTaxAmount").text();
        TaxDetails.push({ mr_no: mr_no, mr_date: mr_date, item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    });
    return TaxDetails;
}
function GetPI_OtherChargeDetails() {
    debugger;
    var PI_OCList = [];

    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = "";
            var oc_val = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var curr_id = "";
            var supp_id = "";
            oc_id = currentRow.find("#OCValue").text();
            oc_val = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            curr_id = currentRow.find("#HdnOCCurrId").text();
            supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text(); // Added by Suraj on 12-04-2024
            let bill_no = currentRow.find("#OCBillNo").text(); // Added by Suraj on 12-04-2024
            let bill_date = currentRow.find("#OCBillDt").text(); // Added by Suraj on 12-04-2024
            let tds_amt = currentRow.find("#OC_TDSAmt").text(); // Added by Suraj on 03-07-2024
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
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
        debugger;
        var Tds_id = "";
        var Tds_rate = "";
        var Tds_level = "";
        var Tds_val = "";
        var Tds_apply_on = "";
        var Tds_name = "";
        var Tds_applyOnName = "";
        var Tds_totalAmnt = "";


        var currentRow = $(this);

        Tds_id = currentRow.find("#td_TDS_NameID").text();
        Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        Tds_level = currentRow.find("#td_TDS_Level").text();
        Tds_val = currentRow.find("#td_TDS_Amount").text();
        Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        Tds_name = currentRow.find("#td_TDS_Name").text();
        Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();

        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
}
function Insert_var_Details() {
    //created by Suraj Maurya on 31-03-2025
    let VarianceDataList = sessionStorage.getItem("VarianceDataList");
    let Dmenu = $("#DocumentMenuId").val();
    if (Dmenu == "105101145") {
        if (VarianceDataList != null && VarianceDataList != "") {
            return VarianceDataList;
        }
    }
    return '[]';//in case session is null or '' 
}
function Insert_var_tax_Details() {
    //created by Suraj Maurya on 31-03-2025
    let VarianceTaxDataList = sessionStorage.getItem("VarianceTaxDataList");
    var Dmenu = $("#DocumentMenuId").val();
    if (Dmenu == "105101145") {
        if (VarianceTaxDataList != null && VarianceTaxDataList != "") {
            return VarianceTaxDataList;
        }
    }
    return '[]';//in case session is null or '' 
}
function CheckPI_Validations() {
    debugger;
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
function GetPI_VoucherDetails() {
    debugger;
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "";
    if ($("#OrderTypeL").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "E";
    }
    var TransType = "Pur";
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
function OnChangeBillNo() {
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
}
function OnChangeBillDate() {
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }
}
function CheckPI_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            if (parseFloat(currentRow.find("#TxtReceivedQuantity").val()) == 0) {
                currentRow.find("#TxtReceivedQuantity_Error").text($("#valueReq").text());
                currentRow.find("#TxtReceivedQuantity_Error").css("display", "block");
                currentRow.find("#TxtReceivedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtReceivedQuantity_Error").css("display", "none");
                currentRow.find("#TxtReceivedQuantity").css("border-color", "#ced4da");
            }
            if (parseFloat(currentRow.find("#TxtRate").val()) == 0) {
                currentRow.find("#TxtRate_Error").text($("#valueReq").text());
                currentRow.find("#TxtRate_Error").css("display", "block");
                currentRow.find("#TxtRate").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtRate_Error").css("display", "none");
                currentRow.find("#TxtRate").css("border-color", "#ced4da");
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
function CheckPI_VoucherValidations() {
    debugger;

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 22-07-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 22-07-2024 to add new common gl validations*/

    //var ErrorFlag = "N";
    ////var ValDigit = $("#ValDigit").text();
    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {

    //    var currentRow = $(this);
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#Acc_name_' + rowid;
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    //var CurrId = currentRow.find('#gl_curr_id').val();
    //    //var ConvRate = currentRow.find('#gl_conv_rate').val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //        //if (CurrId == "0" || ConvRate == 0 || ConvRate == "" || ConvRate == null) {
    //        //    swal("", $("#GLPostingNotFound").text(), "warning");
    //        //    ErrorFlag = "Y";
    //        //}
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

    //debugger;
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDecDigit) && CrTotal == parseFloat(0).toFixed(ValDecDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDecDigit) && CrTotal == parseFloat(0).toFixed(ValDecDigit)) {
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
function OnChangeGoodReceiptNoteNo(GRN_No) {
    debugger;
    var GRNNo = GRN_No.value;
    var GRNDate = $('#ddlGoodReceiptNoteNo').select2("data")[0].element.attributes[0].value;
    var GRNDP = GRNDate.split("-");
    var FGRNDate = (GRNDP[2] + "-" + GRNDP[1] + "-" + GRNDP[0]);
    if (GRNNo == "---Select---") {
        $("#GRN_Date").val("");
        $('#SpanGRNNoErrorMsg').text($("#valueReq").text());
        $("#SpanGRNNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "red");
    }
    else {
        $("#GRN_Date").val(FGRNDate);
        $("#SpanGRNNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
        EInvoiceNo_EwbNumber()
    }
}

function EInvoiceNo_EwbNumber() {
    debugger;
    var grnno = $("#ddlGoodReceiptNoteNo").val();
    var GRN_Date = $("#GRN_Date").val();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/getEinvoiceno_ewbNo",
                data: {
                    grnno: grnno,
                    GRN_Date: GRN_Date,
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
                            //var ewbnumber = $("#ddlEWBNNumber").val();
                            //var einvoice = $("#ddlInvoive").val();
                            $("#ddlEWBNNumber").val(arr.Table[0].ewb_no);
                            $("#ddlInvoive").val(arr.Table[0].einv_no);
                            /*Commented by Suraj on 23-02-2024 for an issue by vikrant sir*/
                            //if (ewbnumber == null || ewbnumber == "")
                            //{
                            //    $("#ddlEWBNNumber").val(arr.Table[0].ewb_no);
                            //}
                            //if (einvoice == null || einvoice == "")
                            //{
                            //    $("#ddlInvoive").val(arr.Table[0].einv_no);
                            //}
                        }
                        else {
                            $("#ddlEWBNNumber").val("");
                            $("#ddlInvoive").val("");
                        }
                        if (arr.Table1.length > 0) {/**Added by Nitesh 11-04-2024 for Bill_no and Bill_dt **/
                            $("#Bill_No").val(arr.Table1[0].bill_no);
                            $("#hdnbillno").val(arr.Table1[0].bill_no);
                            $("#Bill_Date").val(arr.Table1[0].bill_date);
                            $("#hdnbilldt").val(arr.Table1[0].bill_date);
                        }
                        else {
                            $("#Bill_No").val("");
                            $("#Bill_Date").val("");
                        }
                        OnChangeBillNo();
                        OnChangeBillDate();
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}

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
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        $("#Hdn_PInvSuppName").val(Suppname);
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }
    GetSuppAddress(Supp_id);
    BindGoodReceiptNoteLists(Supp_id)

}
function GetSuppAddress(Supp_id) {
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
function CheckRCMApplicable() {
    debugger;
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    GetAllGLID();
}
function BindGoodReceiptNoteLists(Supp_id) {
    try {
        debugger;

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalPurchaseInvoice/GetGoodReceiptNoteLists",

            data: { Supp_id: Supp_id },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    PI_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        debugger;
                        $("#ddlGoodReceiptNoteNo option").remove();
                        $("#ddlGoodReceiptNoteNo optgroup").remove();
                        $('#ddlGoodReceiptNoteNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].mr_dt}" value="${arr.Table[i].mr_no}">${arr.Table[i].mr_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlGoodReceiptNoteNo').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-7 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-5 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });

                        $("#GRN_Date").val("");
                    }
                }
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnClickAddButton() {

    var ConvRate;
    ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    $("#conv_rate").prop("readonly", true);
    var DocMenuId = $("#DocumentMenuId").val();
    var GRNNo = $('#ddlGoodReceiptNoteNo').val();
    var GRNDate = $('#GRN_Date').val();


    //var QtyDecDigit = $("#QtyDigit").text();///Quantity
    //var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    //var ValDecDigit = $("#ValDigit").text();///Amount
    if (GRNNo == "---Select---" || GRNNo == "0") {
        $('#SpanGRNNoErrorMsg').text($("#valueReq").text());
        $("#SpanGRNNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "red");
        $("#ddlGoodReceiptNoteNo").css("border-color", "red");
    }
    else {
        $('#ddlGoodReceiptNoteNo').attr("disabled", true);
        $('#GRN_AddBtn').css("display", "none");
        $("#SpanGRNNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
        $("#ddlGoodReceiptNoteNo").css("border-color", "#ced4da");
        $("#SupplierName").attr("disabled", "disabled");
        var supp_id = $('#SupplierName').val();
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/GetGoodReceiptNoteDetails",
                data: { GRNNo: GRNNo, GRNDate: GRNDate, supp_id: supp_id },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        PI_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var AutoGenDN_forVarQty = "N";
                            if (arr.Table6.length > 0) {//Added by Suraj Maurya on 29-03-2025
                                if (arr.Table6[0].AutoGenDN_forVarQty == "Y") {
                                    AutoGenDN_forVarQty = "Y";
                                }
                            }
                            for (var k = 0; k < arr.Table.length; k++) {
                                var subitmDisable = "";
                                if (arr.Table[k].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                if (arr.Table[k].state_code == $("#Ship_StateCode").val()) {
                                    $("#Hd_GstType").val("Both")
                                } else {
                                    $("#Hd_GstType").val("IGST")
                                }
                                $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                                var BaseVal;
                                // BaseVal = (parseFloat(arr.Table[0].conv_rate) * parseFloat(arr.Table[k].net_val_spec)).toFixed(ValDecDigit)
                                BaseVal = (parseFloat(arr.Table[k].conv_rate) * parseFloat(arr.Table[k].net_val_spec)).toFixed(ValDecDigit)
                                var S_NO = $('#POInvItmDetailsTbl tbody tr').length + 1;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var deletetext = $("#Span_Delete_Title").text();
                                var ManualGst = "";
                                var TaxExempted = "";
                                var mrp = "";
                                var packSize = "";
                                var disabled = "";

                                var ForDisableTaxCal = "";
                                var Icon = "";
                                var ItemType = arr.Table[k].ItemType;

                                var packSize = "";
                                if (arr.Table[k].pack_size != null) {
                                    packSize = arr.Table[k].pack_size;
                                }

                                if (ItemType == "Consumable") {
                                    disabled = "disabled"
                                    disabled_price = ""
                                    ForDisableTaxCal = "Enable"
                                    Icon = `<td class=" red center"> <i class="deleteIcon fa fa-trash"  aria-hidden="true" disabled id="delBtnIcon" title="${deletetext}"></i></td>`
                                }
                                else if (ItemType == "Service") {
                                    disabled = ""
                                    disabled_price = "";
                                    ForDisableTaxCal = "Enable"
                                    Icon = `<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${deletetext}"></i></td>`
                                }
                                else {
                                    disabled = "disabled"
                                    disabled_price = "disabled"
                                    ForDisableTaxCal = "Disabled"
                                    Icon = `<td class=" red center"> <i class="fa fa-trash" aria-hidden="true" disabled style="filter: grayscale(100%)" id="delBtnIcon" title="${deletetext}"></i></td>`
                                }
                                if (DocMenuId == "105101145") {
                                    if (GstApplicable == "Y") {
                                        ManualGst = `<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input ${ForDisableTaxCal} type="checkbox" ${arr.Table[k].manual_gst == "Y" ? "checked" : ""} class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>`
                                        ManualGst += `<td class="qt_to">
                                                     <div class="custom-control custom-switch sample_issue">
                                                             <input type="checkbox" checked class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                                                         <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                                                     </div>
                                                 </td>`;
                                    }
                                    TaxExempted = `<td><div class="custom-control custom-switch sample_issue"><input ${ForDisableTaxCal} type="checkbox" ${arr.Table[k].tax_expted == "Y" ? "checked" : ""} class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>`
                                    mrp = ` <td>
                                                                        <input id="mrp" class="form-control num_right" autocomplete="off" type="text" name="mrp" placeholder="${$("#span_MRP").text()}" value="${parseFloat(CheckNullNumber(arr.Table[k].mrp)).toFixed(ValDecDigit)}" disabled>
                                                                    </td>`
                                    packSize = ` <td>
                                                                        <input id="PackSize" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="${packSize}" disabled>
                                                                    </td>`
                                }

                                var DisableForDomestic = DocMenuId == "105101145" ? "style='display:none;'" : "";
                                $('#POInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                            `+ Icon + `
                           <td class="sr_padding" id="srno">${S_NO}</td>
                                                        <td hidden>
                                                            <input id="TxtGrnNo" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].mr_no}" name="GrnNo" placeholder="${$("#span_GRNNumber").text()}"   disabled>
                                                        </td>
                                                        <td hidden>
                                                            <input id="TxtGrnDate" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].mr_dt}" name="GrnNo" placeholder="${$("#span_GRNDate").text()}"  onblur="this.placeholder='${$("#span_GRNDate").text()}'" disabled>
                                                            <input  type="hidden" id="hfGRNDate" value="${arr.Table[k].mr_date}" />
                                                        </td>
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class=" col-sm-10 no-padding">
                                                                <input id="TxtItemName" class="form-control time" autocomplete="off" type="text" name="" value='${arr.Table[k].item_name}' placeholder='${$("#ItemName").text()}'  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                                                <input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                                                <input class="" type="hidden" id="hdn_item_gl_acc" value="${DocMenuId == "105101145" ? (arr.Table[k].i_capg == "Y" ? arr.Table[k].asset_coa : arr.Table[k].loc_pur_coa) : arr.Table[k].imp_pur_coa}" />
                                                                <input hidden type="text" id="sub_item" value='${arr.Table[k].sub_item}' />
                                                            </div>
<div class="col-sm-2 no-padding"> <div class="col-sm-5 i_Icon">
                                                                <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button>
                                                            </div>
                                                            <div class="col-sm-7" style="padding:0px; text-align:right;"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                         </div>
                                                           </td>
                                                        <td>
                                                            <input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value="${arr.Table[k].uom_alias}"  onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled>
                                                            <input  type="hidden" id="hfUOMID" value="${arr.Table[k].uom_id}" />
                                                            <input id="ItemHsnCode1" value="${arr.Table[k].hsn_code}" type="hidden" />
                                                            <input  type="hidden" id="ItemtaxTmplt" value="${arr.Table[k].tmplt_id}" />
                                                            <input  type="hidden" id="hdnItemType" value="${arr.Table[k].ItemType}" />
                                                            <input  type="hidden" id="ForDisable" value="${ForDisableTaxCal}" />
                                                        </td>
<td>
 <input id="ItemHsnCode" class="form-control" value="${arr.Table[k].hsn_code}" disabled/>
</td>
<td>
<input id="ItemType" class="form-control" autocomplete="off" type="text" name="ItemType" placeholder="${$("#ItemType").text()}" value="${arr.Table[k].ItemType}"  onblur="this.placeholder='${$("#ItemType").text()}'" disabled>
</td>
`+ mrp +`
`+ packSize+`
                                                        <td class="lpo_form">
                                                        <div class="col-sm-8 num_right no-padding" >
                                                            <input id="TxtReceivedQuantity" onchange="OnChangeConsumbleItemQuantity(event)" onkeypress="return AmountFloatQty(this,event);" autocomplete="off" ${disabled} class="form-control num_right" autocomplete="" type="text" name="ReceivedQuantity" value="${parseFloat(arr.Table[k].rec_qty).toFixed(QtyDecDigit)}" placeholder="0000.00">
                                                        </div>
                                                        <div class=" col-sm-4 no-padding"> <div class=" col-sm-5 i_Icon">
                                                                <button type="button" class="calculator item_pop" data-toggle="modal" data-target="#VarianceDetail" onclick="OnClickVarienceDetailIButton(event)" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_VarianceDetail_Title").text()}"> </button>
                                                            </div>
                                                            <div class="col-sm-7 i_Icon no-padding" id="div_SubItemReceivedQty" >
                                                            <button type="button" id="SubItemReceivedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </div>
                                                      <span id="TxtReceivedQuantity_Error" class="error-message is-visible"></span>
                                                        </td>
                                                        <td>
                                                           <div class="lpo_form">
                                                            <input id="TxtRate" ${disabled_price} onchange="OnChangeConsumbleItemPrice(event)" onkeypress="return AmountFloatRate(this,event);" class="form-control num_right" autocomplete="off" type="text" name="Rate" placeholder="0000.00" value="${parseFloat(arr.Table[k].item_rate).toFixed(RateDecDigit)}" >
                                                            <span id="TxtRate_Error" class="error-message is-visible"></span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="TxtItemGrossValue" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="${parseFloat(arr.Table[k].item_gross_val).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtItemGrossValueInBase" class="form-control num_right" autocomplete="off" type="text" name="GrossValueInBase" value="${parseFloat(arr.Table[k].item_gross_val_bs).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        `+ TaxExempted + `
                                                        `+ ManualGst + `
                                                        <td>
                                                            <div class=" col-sm-10 num_right no-padding">
                                                            <input id="Txtitem_tax_amt" class="form-control num_right" autocomplete="off" value='${parseFloat(arr.Table[k].tax_amt).toFixed(ValDecDigit)}' type="text" name="item_tax_amt" placeholder="0000.00"  disabled>
                                                            </div><div class=" col-sm-2 num_right no-padding"><button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                            </div>
                                                        </td>
                                                        <td hidden>
                                                            <input id="TxtTaxRecovAmt" hidden value='${parseFloat(arr.Table[k].tax_amt_recov).toFixed(ValDecDigit)}' />
                                                            <input id="TxtTaxNonRecovAmt" hidden value='${parseFloat(arr.Table[k].tax_amt_nrecov).toFixed(ValDecDigit)}' />
                                                        </td>
                                                        <td>
                                                            <input id="TxtOtherCharge" class="form-control num_right"  value='${parseFloat(0/*arr.Table[k].oc_amt*/).toFixed(ValDecDigit)}' autocomplete="off" type="text" name="OtherCharge" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td hidden>
                                                            <input id="TxtNetValue" class="form-control num_right" value="${parseFloat(arr.Table[k].net_val_spec).toFixed(ValDecDigit)}" autocomplete="off" type="text" name="NetValue"  placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtNetValueInBase" class="form-control num_right" value="${parseFloat(arr.Table[k].net_val_bs).toFixed(ValDecDigit)}" autocomplete="off" type="text" name="NetValueInBase" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td class="num_right" ${AutoGenDN_forVarQty == "Y" ? "" : "hidden"}>
                                                            <div class="col-sm-10 lpo_form num_right no-padding">
                                                                <input id="TxtDebitNoteValue" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(0).toFixed(ValDecDigit)}" name="TxtDebitNoteValue" placeholder="0000.00" disabled>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="">
                                                                <button type="button" class="calculator" id="BtnVarianceQtyDtls" onclick="return onClickBtnVarianceQtyDtls(event)" data-toggle="modal" data-target="#VarianceQuantityDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_VarianceQualityDetail").text()}"></button>
                                                            </div>
                                                        </td>
                            </tr>`);

                                var Itm_ID = arr.Table[k].item_id;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var docid = $("#DocumentMenuId").val();
                                if (docid == '105101145') {
                                    //

                                    //if (GstApplicable == "Y") {
                                    //    $("#HdnTaxOn").val("Item");
                                    //    Cmn_ApplyGSTToAtable("POInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", arr.Table1,"Y","","Add");
                                    //}
                                    //else {
                                    //    $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                    //    if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                    //        $("#HdnTaxOn").val("Item");
                                    //        $("#TaxCalcItemCode").val(Itm_ID);
                                    //        $("#Tax_AssessableValue").val(arr.Table[k].item_gross_val);
                                    //        $("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                                    //        $("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                                    //        var TaxArr = arr.Table1;
                                    //        let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                    //        selected = JSON.stringify(selected);
                                    //        TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                    //        selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                    //        selected = JSON.stringify(selected);
                                    //        TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                    //        if (TaxArr.length > 0) {
                                    //            AddTaxByHSNCalculation(TaxArr);
                                    //            OnClickSaveAndExit("Y");
                                    //            var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                    //            Reset_ReOpen_LevelVal(lastLevel);
                                    //        }
                                    //    }

                                    //}
                                }


                            }
                            if (arr.Table3.length > 0) {
                                var ArrTaxList = [];
                                for (var l = 0; l < arr.Table3.length; l++) {//Tax On Item
                                    $('#Hdn_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo">${arr.Table3[l].mr_no}</td>
                                    <td id="DocDate">${arr.Table3[l].mr_dt}</td>
                                    <td id="TaxItmCode">${arr.Table3[l].item_id}</td>
                                    <td id="TaxName">${arr.Table3[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table3[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table3[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table3[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table3[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${parseFloat(arr.Table3[l].tax_val).toFixed(ValDecDigit)}</td>
                                    <td id="TotalTaxAmount">${parseFloat(arr.Table3[l].item_tax_amt).toFixed(ValDecDigit)}</td>
                                    <td id="TaxApplyOnID">${arr.Table3[l].tax_apply_on}</td>
                                    <td id="TaxRecov">${arr.Table3[l].recov}</td>
                                    <td id="TaxAccId">${arr.Table3[l].tax_acc_id}</td>
                                        </tr>`);

                                    if (ArrTaxList.findIndex(v => (v.taxID) == (arr.Table3[l].tax_id)) > -1) {
                                        let getIndex = ArrTaxList.findIndex(v => (v.taxID) == (arr.Table3[l].tax_id));
                                        ArrTaxList[getIndex].totalTaxAmt = parseFloat(CheckNullNumber(ArrTaxList[getIndex].totalTaxAmt)) + parseFloat(CheckNullNumber(arr.Table3[l].tax_val));
                                    } else {
                                        ArrTaxList.push({ taxID: arr.Table3[l].tax_id, taxAccID: arr.Table3[l].tax_acc_id, taxName: arr.Table3[l].tax_name, totalTaxAmt: arr.Table3[l].tax_val, TaxRecov: arr.Table3[l].recov })
                                    }
                                }
                                let TotalTAmt = 0;
                                for (var l = 0; l < ArrTaxList.length; l++) {
                                    $("#Tbl_ItemTaxAmountList tbody").append(`<tr>
                                                    <td>${ArrTaxList[l].taxName}</td>
                                                    <td id="taxRecov" class="center">${(ArrTaxList[l].TaxRecov == 'Y' ? '<i class="fa fa-check text-success " aria-hidden="true"></i>' : '<i class="fa fa-times-circle text-danger" aria-hidden="true"></i>')}</td>
                                                    <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                                                    <td hidden="hidden" id="taxID">${ArrTaxList[l].taxID}</td>
                                                    <td hidden="hidden" id="taxAccID">${ArrTaxList[l].taxAccID}</td>
                                                </tr>`)
                                    TotalTAmt = parseFloat(TotalTAmt) + parseFloat(parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit));

                                }
                                $("#_ItemTaxAmountTotal").text(parseFloat(TotalTAmt).toFixed(ValDecDigit))

                            }

                            if (arr.Table5.length > 0) {
                                for (var l = 0; l < arr.Table5.length; l++) {//Tax On OC
                                    $('#Hdn_OC_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table5[l].item_id}</td>
                                    <td id="TaxName">${arr.Table5[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table5[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table5[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table5[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table5[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${parseFloat(arr.Table5[l].tax_val).toFixed(ValDecDigit)}</td>
                                    <td id="TotalTaxAmount">${parseFloat(arr.Table5[l].item_tax_amt).toFixed(ValDecDigit)}</td>
                                    <td id="TaxApplyOnID">${arr.Table5[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table5[l].tax_acc_id}</td>
                                        </tr>`);

                                    $('#Hdn_OCTemp_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table5[l].item_id}</td>
                                    <td id="TaxName">${arr.Table5[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table5[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table5[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table5[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table5[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${parseFloat(arr.Table5[l].tax_val).toFixed(ValDecDigit)}</td>
                                    <td id="TotalTaxAmount">${parseFloat(arr.Table5[l].item_tax_amt).toFixed(ValDecDigit)}</td>
                                    <td id="TaxApplyOnID">${arr.Table5[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table5[l].tax_acc_id}</td>
                                        </tr>`);
                                }
                            }
                            debugger;
                            let TotalAMount = 0;
                            let TotalTaxAMount = 0;
                            let TotalAMountWT = 0;
                            let TotalSuppOCAmtWT = 0;
                            if (arr.Table4.length > 0) {
                                for (var l = 0; l < arr.Table4.length; l++) {//Tax On OC
                                    $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
                        <td id="OC_name">${arr.Table4[l].oc_name}</td>
                        <td id="OC_HSNCode">${arr.Table4[l].HSN_code}</td>
                        <td id="OC_Curr">${arr.Table4[l].curr_name}</td>
                        <td id="HdnOC_CurrId">${arr.Table4[l].curr_id}</td>
                        <td id="td_OCSuppName">${IsNull(arr.Table4[l].supp_name, '')}</td>
                        <td id="td_OCSuppID">${IsNull(arr.Table4[l].supp_id, '')}</td>
                        <td id="td_OCSuppType">${IsNull(arr.Table4[l].supp_type, '')}</td>
                        <td id="OC_Conv">${arr.Table4[l].conv_rate}</td>
                        <td hidden="hidden" id="OC_ID">${arr.Table4[l].oc_id}</td>
                        <td class="num_right" id="OCAmtSp">${arr.Table4[l].oc_val}</td>
                        <td class="num_right" id="OCAmtBs">${arr.Table4[l].OCValBs}</td>
                        <td class="num_right" id="OCTaxAmt">${arr.Table4[l].tax_amt}</td>
                        <td class="num_right" id="OCTotalTaxAmt">${arr.Table4[l].total_amt}</td>
                        <td id="OCBillNo">${arr.Table4[l].bill_no == null ? "" : arr.Table4[l].bill_no}</td>
                        <td id="OCBillDate">${arr.Table4[l].bill_dt == null ? "" : arr.Table4[l].bill_dt}</td>
                        <td id="OCBillDt" hidden>${arr.Table4[l].bill_date == null ? "" : arr.Table4[l].bill_date}</td>
                        <td id="OCAccId" hidden>${arr.Table4[l].oc_acc_id}</td>
                        <td id="OCSuppAccId" hidden>${IsNull(arr.Table4[l].supp_acc_id, '')}</td>
                        </tr>`);

                                    $('#Tbl_OC_Deatils tbody').append(`<tr id="R${l}">
                        <td id="deletetext" class=" red center"><i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" "></i></td>
                        <td id="OCName" >${arr.Table4[l].oc_name}</td>
                        <td id="OC_HSNCode">${arr.Table4[l].HSN_code}</td>
                        <td id="OCCurr" >${arr.Table4[l].curr_name}</td>
                        <td id="HdnOCCurrId" hidden>${arr.Table4[l].curr_id}</td>
                        <td id="td_OCSuppName" >${IsNull(arr.Table4[l].supp_name, '')}</td>
                        <td id="td_OCSuppID" style="display:none">${IsNull(arr.Table4[l].supp_id, '')}</td>
                        <td id="td_OCSuppType" style="display:none">${IsNull(arr.Table4[l].supp_type, '')}</td>
                        <td id="OCConv" class="num_right">${parseFloat(arr.Table4[l].conv_rate).toFixed(ExchDecDigit)}</td>
                        <td hidden="hidden" id="OCValue">${arr.Table4[l].oc_id}</td>
                        <td class="num_right" id="OCAmount">${parseFloat(arr.Table4[l].oc_val).toFixed(ValDecDigit)}</td>
                        <td class="num_right" id="OcAmtBs" >${parseFloat(arr.Table4[l].OCValBs).toFixed(ValDecDigit)}</td>
                          <td class="num_right">
                        <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(arr.Table4[l].tax_amt)).toFixed(ValDecDigit)}</div>
                         <div class="col-md-2 col-sm-12 no-padding"><button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title=""></i></button></div>
                    </td>
                    <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(arr.Table4[l].total_amt)).toFixed(ValDecDigit)}</td>
                    <td id="OCBillNo">${arr.Table4[l].bill_no == null ? "" : arr.Table4[l].bill_no}</td>
                    <td id="OCBillDate">${arr.Table4[l].bill_dt == null ? "" : arr.Table4[l].bill_dt}</td>
                    <td id="OCBillDt" hidden>${arr.Table4[l].bill_date == null ? "" : arr.Table4[l].bill_date}</td>
                    <td id="OCAccId" hidden>${arr.Table4[l].oc_acc_id}</td>
                    <td id="OCSuppAccId" hidden>${IsNull(arr.Table4[l].supp_acc_id, '')}</td>
                    </tr>`);
                                    $("#Tbl_OtherChargeList tbody").append(`<tr>
                                                    <td id="othrChrg_Name">${arr.Table4[l].oc_name}</td>
                                                    <td>${arr.Table4[l].supp_name}</td>
                                                    <td align="right" id="OCAmtSp1">${parseFloat(arr.Table4[l].OCValBs).toFixed(ValDecDigit)}</td>
                                                    <td hidden="hidden" id="OCID">${arr.Table4[l].oc_id}</td>
                                                    <td align="right">${parseFloat(CheckNullNumber(arr.Table4[l].tax_amt)).toFixed(ValDecDigit)}</td>
                                                    <td align="right">${parseFloat(CheckNullNumber(arr.Table4[l].total_amt)).toFixed(ValDecDigit)}</td>
                                                </tr>`)
                                    TotalAMount = parseFloat(TotalAMount) + parseFloat(arr.Table4[l].OCValBs);
                                    TotalTaxAMount = parseFloat(TotalTaxAMount) + parseFloat(arr.Table4[l].tax_amt);
                                    TotalAMountWT = parseFloat(TotalAMountWT) + parseFloat(arr.Table4[l].total_amt);
                                    if (arr.Table4[l].bill_no == "") {
                                        TotalSuppOCAmtWT = parseFloat(TotalSuppOCAmtWT) + parseFloat(arr.Table4[l].total_amt);
                                    }

                                }
                                $("#_OtherChargeTotal").text(TotalAMount.toFixed(ValDecDigit));
                                //if (DocumentMenuId == "105101145") {
                                $("#_OtherChargeTotalTax").text(TotalTaxAMount.toFixed(ValDecDigit));
                                $("#_OtherChargeTotalAmt").text(TotalAMountWT.toFixed(ValDecDigit));
                                $("#TxtOtherCharges").val(TotalAMountWT.toFixed(ValDecDigit))
                                $("#TxtDocSuppOtherCharges").val(TotalSuppOCAmtWT.toFixed(ValDecDigit));

                            }
                            CalculateAmount();
                            Calculate_OC_AmountItemWise(TotalSuppOCAmtWT.toFixed(ValDecDigit))
                            var Flag = "N";
                            $("#POInvItmDetailsTbl >tbody >tr").each(function () {
                                var row = $(this).closest('tr');
                                var hdnItemType = row.find("#hdnItemType").val();
                                if (hdnItemType == "Consumable" || hdnItemType == "Service" /*Added By NItesh Service 05052025 1149  */) {
                                    Flag = "Y";
                                    OnClickTaxExemptedCheckBox(row, "AutoAplyTax");
                                }
                            });
                            //var TxtOC = $("#TxtOtherCharges").val();
                            //var _OCTotalAmt = $("#_OtherChargeTotalAmt").text();
                            //if (TxtOC == _OCTotalAmt) {
                            //    Calculate_OC_AmountItemWise(_OCTotalAmt)
                            //}

                            //if (docid == '105101145') {
                            //    ResetGRN_DDL_Detail();
                            //}
                            debugger;
                            debugger;
                            let SuppId = $("#SupplierName").val();
                            let GrossAmt = $("#TxtGrossValueInBase").val();
                            if (docid == "105101145") {
                                AutoTdsApply(SuppId, GrossAmt).then(() => {
                                    //CalculateVoucherTotalAmount();
                                    //GetAllGLID(); 
                                });
                            } else {
                                CalculateVoucherTotalAmount();
                                //OnClickOtherChargeBtn();
                                GetAllGLID();
                            }
                        }
                        debugger;
                        if (arr.Table2.length > 0) {
                            var rowIdx = 0;
                            for (var y = 0; y < arr.Table2.length; y++) {
                                var srcDocNo = arr.Table2[y].mr_no;
                                var srcDocDate = arr.Table2[y].mr_dt;
                                var ItmId = arr.Table2[y].item_id;
                                var SubItmId = arr.Table2[y].sub_item_id;
                                var SubItmName = arr.Table2[y].sub_item_name;
                                var GrnQty = arr.Table2[y].Qty;

                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemQty" value='${GrnQty}'></td>
                                    <td><input type="text" id="subItemSrcDocNo" value='${srcDocNo}'></td>
                                    <td><input type="text" id="subItemSrcDocDate" value='${srcDocDate}'></td>
                                    </tr>`);
                            }

                        }

                        if (arr.Table6.length > 0) {//Added by Suraj Maurya on 29-03-2025
                            if (arr.Table6[0].AutoGenDN_forVarQty == "Y") {
                                let VarianceDataList = [];
                                let VarianceTaxDataList = [];
                                if (arr.Table7.length > 0) {

                                    arr.Table7.map((item, index) => {
                                        let sr_no = 0;
                                        sr_no++;
                                        VarianceDataList.push({
                                            sr_no: sr_no,
                                            dn_no: item.dn_no, dn_date: item.dn_date, mr_no: item.mr_no, mr_date: item.mr_date,
                                            item_id: item.item_id, var_type: "SRGE", var_type_desc: $("#span_ShortReceivedInGateEntry").text()
                                            , qty: item.short_recd, price: item.price
                                            , value: item.short_recd_val, tax: item.short_recd_val_tax
                                            , net_amt: parseFloat(parseFloat(item.short_recd_val) + parseFloat(item.short_recd_val_tax)).toFixed(ValDecDigit)
                                            , dn_amt: (parseFloat(item.short_recd_val) + parseFloat(item.short_recd_val_tax)).toFixed(ValDecDigit)
                                            , include: "Y"/*, item_acc_id: item.item_acc_id*/
                                        });
                                        sr_no++;
                                        VarianceDataList.push({
                                            sr_no: sr_no,
                                            dn_no: item.dn_no, dn_date: item.dn_date, mr_no: item.mr_no, mr_date: item.mr_date,
                                            item_id: item.item_id, var_type: "SCQC", var_type_desc: $("#span_ShortCountedInQC").text()
                                            , qty: item.short_qty, price: item.price, value: item.short_val, tax: item.short_val_tax
                                            , net_amt: (parseFloat(item.short_val) + parseFloat(item.short_val_tax)).toFixed(ValDecDigit)
                                            , dn_amt: (parseFloat(item.short_val) + parseFloat(item.short_val_tax)).toFixed(ValDecDigit)
                                            , include: "Y"/*, item_acc_id: item.item_acc_id*/
                                        });
                                        sr_no++;
                                        VarianceDataList.push({
                                            sr_no: sr_no,
                                            dn_no: item.dn_no, dn_date: item.dn_date, mr_no: item.mr_no, mr_date: item.mr_date,
                                            item_id: item.item_id, var_type: "SQTY", var_type_desc: $("#span_SampleReserveQuantity").text()
                                            , qty: item.sample_qty, price: item.price, value: item.sample_val, tax: item.sample_val_tax
                                            , net_amt: (parseFloat(item.sample_val) + parseFloat(item.sample_val_tax)).toFixed(ValDecDigit)
                                            , dn_amt: (parseFloat(item.sample_val) + parseFloat(item.sample_val_tax)).toFixed(ValDecDigit)
                                            , include: "Y"/*, item_acc_id: item.item_acc_id*/
                                        });
                                    });
                                    sessionStorage.setItem("VarianceDataList", JSON.stringify(VarianceDataList));
                                    if (arr.Table8.length > 0) {

                                        arr.Table8.map((item, index) => {
                                            VarianceTaxDataList.push({
                                                mr_no: item.mr_no, mr_date: item.mr_date,
                                                item_id: item.item_id, var_type: "SRGE", tax_id: item.tax_id, tax_name: item.tax_name
                                                , tax_rate: item.tax_rate, tax_val: item.short_recd_val_tax
                                                , tax_level: item.tax_level, tax_apply_on: item.tax_apply_on, tax_recov: item.tax_recov
                                                , tax_acc_id: item.tax_acc_id
                                            });
                                            VarianceTaxDataList.push({
                                                mr_no: item.mr_no, mr_date: item.mr_date,
                                                item_id: item.item_id, var_type: "SCQC", tax_id: item.tax_id, tax_name: item.tax_name
                                                , tax_rate: item.tax_rate, tax_val: item.short_val_tax
                                                , tax_level: item.tax_level, tax_apply_on: item.tax_apply_on, tax_recov: item.tax_recov
                                                , tax_acc_id: item.tax_acc_id
                                            });
                                            VarianceTaxDataList.push({
                                                mr_no: item.mr_no, mr_date: item.mr_date,
                                                item_id: item.item_id, var_type: "SQTY", tax_id: item.tax_id, tax_name: item.tax_name
                                                , tax_rate: item.tax_rate, tax_val: item.sample_val_tax
                                                , tax_level: item.tax_level, tax_apply_on: item.tax_apply_on, tax_recov: item.tax_recov
                                                , tax_acc_id: item.tax_acc_id
                                            });
                                        });
                                        sessionStorage.setItem("VarianceTaxDataList", JSON.stringify(VarianceTaxDataList));
                                    }
                                }

                                $("#POInvItmDetailsTbl tbody tr").each(function () {
                                    const row = $(this);
                                    let item_id = row.find("#hfItemID").val();
                                    let total_dn_amt = 0;
                                    VarianceDataList.map((item) => {
                                        if (item.item_id == item_id) {
                                            total_dn_amt = parseFloat(total_dn_amt) + parseFloat(item.dn_amt);
                                        }
                                    })
                                    row.find("#TxtDebitNoteValue").val(total_dn_amt.toFixed(ValDecDigit));
                                });
                            }

                        }

                    }
                },
            });
        } catch (err) {
            console.log("GoodReceiptInvoice Error : " + err.message);
        }
    }
}
//AutoTdsApply : Added by Suraj on 09-07-2024 to Add Auto TDS Apply functionality
async function AutoTdsApply(SuppId, GrossAmt) {
    var CustomTds = $('#Hdn_TDS_CalculatorTbl > tbody > tr #td_TDS_AssValApplyOn:contains("CA")').closest('tr');
    if (CustomTds.length == 0) {/* Added by Suraj Maurya on 14-05-2025 */
        await $.ajax({
            type: "POST",
            url: "/Common/Common/Cmn_GetTdsDetails",
            data: { SuppId: SuppId, GrossVal: GrossAmt, tax_type: "TDS" },
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
                            //let td_tds_amt = Math.round(arr.Table1[i].tds_amt);
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
                                <td id="td_TDS_AssValApplyOn">${arr.Table1[i].TdsAssVal_applyOn}</td>
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
                //CalculateVoucherTotalAmount();
                GetAllGLID();

            }
        })
    }
}
function AmountFloatQty(el, evt) {
    let QtyDigit = ord_type_imp ? "#ExpImpQtyDigit" : "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDigit) == false) {
        return false;
    }
    return true;
}
function AmountFloatRate(el, evt) {
    let RateDigit = ord_type_imp ? "#ExpImpRateDigit" : "#PurCostingRateDigit";
    if (Cmn_FloatValueonly(el, evt, RateDigit) == false) {
        return false;
    }
    return true;
}
function OnChangeConsumbleItemQuantity(e) {
    var currentrow = $(e.target).closest('tr');
    //let QtyDigit = $("#QtyDigit").text();
    var TxtReceivedQuantity = currentrow.find("#TxtReceivedQuantity").val();
    currentrow.find("#TxtReceivedQuantity").val(parseFloat(CheckNullNumber(TxtReceivedQuantity)).toFixed(QtyDecDigit));
    if (parseFloat(CheckNullNumber(TxtReceivedQuantity)) <= 0) {
        currentrow.find("#TxtReceivedQuantity").css("border-color", "red");
        currentrow.find("#TxtReceivedQuantity_Error").css("display", "block");
        currentrow.find("#TxtReceivedQuantity_Error").text($("#valueReq").text());
        return false;

    } else {
        currentrow.find("#TxtReceivedQuantity").css("border-color", "#ced4da");
        currentrow.find("#TxtReceivedQuantity_Error").css("display", "none");
        currentrow.find("#TxtReceivedQuantity_Error").text("");
    }
    //var docid = $("#DocumentMenuId").val();
    OnChangeConsumbleItemPrice(e);
    //let SuppId = $("#SupplierName").val();
    //let GrossAmt = $("#TxtGrossValueInBase").val();
    //if (docid == "105101145") {
    //    AutoTdsApply(SuppId, GrossAmt).then(() => {
    //        CalculateVoucherTotalAmount();
    //        GetAllGLID();
    //    });
    //}
}
function OnChangeConsumbleItemPrice(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var ItmCode = currentrow.find("#hfItemID").val();
    //let ValDigit = $("#ValDigit").text();
    //let RateDigit = $("#RateDigit").text();
    //let ExchangeRate = $("#ExchangeRate").val();
    let ExchangeRate = $("#conv_rate").val();

    var TxtGrnNo = currentrow.find("#TxtGrnNo").val();
    var TxtReceivedQuantity = currentrow.find("#TxtReceivedQuantity").val();
    var TxtRate = currentrow.find("#TxtRate").val();
    var Txtitem_tax_amt = currentrow.find("#Txtitem_tax_amt").val();
    var TxtOtherCharge = currentrow.find("#TxtOtherCharge").val();
    var TxtInvValue = parseFloat(CheckNullNumber(TxtReceivedQuantity)) * parseFloat(CheckNullNumber(TxtRate));
    var TxtNetValueInBase = parseFloat(TxtInvValue) + parseFloat(CheckNullNumber(Txtitem_tax_amt)) + parseFloat(CheckNullNumber(TxtOtherCharge));

    currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(RateDecDigit));
    if (parseFloat(CheckNullNumber(TxtRate)) <= 0) {
        currentrow.find("#TxtRate").css("border-color", "red");
        currentrow.find("#TxtRate_Error").css("display", "block");
        currentrow.find("#TxtRate_Error").text($("#valueReq").text());
        return false;

    } else {
        currentrow.find("#TxtRate").css("border-color", "#ced4da");
        currentrow.find("#TxtRate_Error").css("display", "none");
        currentrow.find("#TxtRate_Error").text("");
    }
    currentrow.find("#TxtItemGrossValue").val(parseFloat(TxtInvValue).toFixed(ValDecDigit));
    currentrow.find("#TxtItemGrossValueInBase").val(parseFloat(parseFloat(TxtInvValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit));
    currentrow.find("#TxtNetValueInBase").val(parseFloat(TxtNetValueInBase).toFixed(ValDecDigit));
    currentrow.find("#TxtNetValue").val(parseFloat(TxtNetValueInBase / ExchangeRate).toFixed(ValDecDigit));
    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (currentrow.find("#ManualGST").is(":checked")) {
            //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
            //currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        //else {
        //CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
        //}
    }
    CalculateAmount();
    var docid = $("#DocumentMenuId").val();
    let SuppId = $("#SupplierName").val();
    let GrossAmt = $("#TxtGrossValueInBase").val();
    if (docid == "105101145") {
        AutoTdsApply(SuppId, GrossAmt).then(() => {
            //CalculateVoucherTotalAmount();
            //GetAllGLID();
        });
    }
    //GetAllGLID();
}
function OtherChargePageFuncton() {
    //OnClickSaveAndExit_OC_Btn();
    //GetAllGLID();
}
function ApplyGSTToAtable(TaxCalcItemCode, TaxCalcGRNNo, TaxCalcGRNDate, item_ass_val, Taxarr, GL, AutoAplyTaxPI) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var ItemId = $("#Tax_ItemID").val();
    var TaxCalcItemCode1 = $("#" + TaxCalcItemCode).val();

    var arr = Taxarr;
    $("#" + TaxCalcGRNNo + " >tbody >tr").each(function () {
        var clickedrow = $(this);

        var Tax_AssessableValue = clickedrow.find("#" + item_ass_val).val();
        var Itm_ID = clickedrow.find("#" + TaxCalcItemCode).val();

        var arr = Taxarr;
        let selected = []; selected.push({ item_id: Itm_ID });
        selected = JSON.stringify(selected);
        arr = arr.filter(i => selected.includes(i.item_id));

        for (var i = 0; i < arr.length; i++) {
            var Grn_no = clickedrow.find("#TxtGrnNo").val();
            if (clickedrow.find("#ManualGST").is(":checked")) {

            }
            else if (Itm_ID == arr[i].item_id) {

                $("#TaxCalcItemCode").val(arr[i].item_id);
                var DocNo = clickedrow.find("#TxtGrnNo").val();
                var DocDate = clickedrow.find("#hfGRNDate").val();

                $("#TaxCalcGRNNo").val(DocNo);
                $("#TaxCalcGRNDate").val(DocDate);

                $('#TaxCalculatorTbl tbody tr').remove();
                $("#Tax_AssessableValue").val(Tax_AssessableValue);
                var Ship_StateCode = $("#Ship_StateCode").val();
                var Br_Gst_StateCode = arr[i].br_state_code;
                var itax_val = 0;
                if (parseInt(Ship_StateCode) == parseInt(Br_Gst_StateCode)) {
                    var ctax_val = Tax_AssessableValue * arr[i].cgst_tax_per / 100;
                    var stax_val = Tax_AssessableValue * arr[i].sgst_tax_per / 100;
                    $("#Hd_GstType").val("Both")/*Only for Domestic Purchase Invoice RCM Functionality*/
                    TaxTableDataBind(1, arr[i].cgst_tax_name, arr[i].cgst_tax_id, String(arr[i].cgst_tax_per), 1, "Immediate Level", ctax_val, "I", (parseFloat(ctax_val) + parseFloat(stax_val)), arr[i].cgst_tax_acc_id, "", arr[i].cgst_tax_recov)
                    TaxTableDataBind(2, arr[i].sgst_tax_name, arr[i].sgst_tax_id, String(arr[i].sgst_tax_per), 1, "Immediate Level", stax_val, "I", (parseFloat(ctax_val) + parseFloat(stax_val)), arr[i].sgst_tax_acc_id, "", arr[i].sgst_tax_recov)
                } else {
                    itax_val = Tax_AssessableValue * arr[i].igst_tax_per / 100;
                    $("#Hd_GstType").val("IGST")/*Only for Domestic Purchase Invoice RCM Functionality*/
                    TaxTableDataBind(1, arr[i].igst_tax_name, arr[i].igst_tax_id, String(arr[i].igst_tax_per), 1, "Immediate Level", itax_val, "I", itax_val, arr[i].igst_tax_acc_id, "", arr[i].igst_tax_recov)
                }
                debugger;
                if (GL == "ItemTax") {
                    $("#HdnTaxOn").val("Item");
                    $("#taxTemplate").text("Template")
                    OnClickSaveAndExit("Y", AutoAplyTaxPI);
                }
            }
        }
    });
    click_chkplusminus(AutoAplyTaxPI);
}

function OnClickTaxExemptedCheckBox(e, AutoAplyTax) {
    debugger;
    //var ValDecDigit = $("#ValDigit").text();
    if (AutoAplyTax == "AutoAplyTax") {
        var currentrow = e;
    }
    else {
        var currentrow = $(e.target).closest('tr');
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var Itm_ID = currentrow.find("#hfItemID").val();
    var item_gross_val = currentrow.find("#TxtItemGrossValue").val();
    var mr_no = currentrow.find("#TxtGrnNo").val();
    var mr_date = currentrow.find("#hfGRNDate").val();
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        CalculateTaxExemptedAmt(e, "N")
        if (AutoAplyTax != "AutoAplyTax") {
            GetAllGLID();
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#ship_add_gstNo").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "TxtGrnNo", "TxtItemGrossValue", "TaxCalcGRNNo", "TaxCalcItemCode", "", "", "", "AutoAplyTaxPI")
        }
        else {
            $("#Tax_ItemID").val(Itm_ID);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    //var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var Itm_ID = currentrow.find("#hfItemID").val();
    var item_gross_val = currentrow.find("#TxtItemGrossValue").val();
    var mr_no = currentrow.find("#TxtGrnNo").val();
    var mr_date = currentrow.find("#hfGRNDate").val();
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find('#TaxExempted').prop("checked", false);
        $("#TaxCalculatorTbl tbody tr").remove();
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        $("#TotalTaxAmount").text(parseFloat(0).toFixed(ValDecDigit))
        CalculateTaxExemptedAmt(e, "N")
        GetAllGLID();
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        $("#TaxCalculatorTbl tbody tr").remove();
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "TxtGrnNo", "TxtItemGrossValue", "TaxCalcGRNNo", "TaxCalcItemCode")
        CalculateTaxExemptedAmt(e, "N")
        $("#taxTemplate").text("Template");
    }
}
function CalculateTaxExemptedAmt(e, flag) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    //var RateDecDigit = $("#RateDigit").text();
    //var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");

    var OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    var TxtGrnNo = clickedrow.find("#TxtGrnNo").val();
    var ItmRate = clickedrow.find("#TxtRate").val();
    var oc_amt = clickedrow.find("#TxtOtherCharge").val();
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (oc_amt == ".") {
        oc_amt = 0;
    }
    if (oc_amt != "" && oc_amt != ".") {
        oc_amt = parseFloat(oc_amt);
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinVal);
        //clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#NetOrderValueSpe").val(FinVal);
        clickedrow.find("#TxtNetValue").val(FinVal);
        FinalVal = FinVal * ConvRate

        FinalVal = FinalVal + oc_amt
        var FinalNetValue = parseFloat(FinVal) / ConvRate //+ oc_amt;
        clickedrow.find("#TxtNetValue").val(FinalNetValue);
        clickedrow.find("#TxtNetValueInBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        }
    }
}
function CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, AssAmount) {
    debugger;
    //var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var GrnNo = currentRow.find("#DocNo").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if (TaxItemID == ItmCode && TxtGrnNo == GrnNo) {
                    /*-----------Added by Suraj on 14-06-2024---------*/

                    var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted) {
                        AssessVal = 0;
                    }
                    var GstApplicable = $("#Hdn_GstApplicable").text();
                    if (GstApplicable == "Y") {

                        var ManualGST = ItemRow.find("#ManualGST").is(":checked");
                        var item_tax_amt = ItemRow.find("#Txtitem_tax_amt").val();
                        if (ManualGST && parseFloat(CheckNullNumber(item_tax_amt)) == 0) {
                            AssessVal = 0;
                        }
                    }
                    /*-----------Added by Suraj on 14-06-2024 End---------*/

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
                    currentRow.find("#TaxAmount").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                var DocNo = currentRow.find("#DocNo").text();
                var TaxAccId = currentRow.find("#TaxAccId").text();
                var TaxRecov = currentRow.find("#TaxRecov").text();

                NewArray.push({ DocNo: DocNo, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + TxtGrnNo + "')").closest("tr").each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().parent().each(function () {
                debugger;
                var currentRow = $(this);
                var ItemNo;

                ItemNo = currentRow.find("#hfItemID").val();
                var GrnNo = currentRow.find("#TxtGrnNo").val();

                if (ItemNo == ItmCode && TxtGrnNo == GrnNo) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + GrnNo + "')").closest("tr");
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
                            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                if (parseFloat(ItemTaxAmt) == 0) {
                                    CRow.remove();
                                }
                                //CRow.remove();

                                currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                NetOrderValueSpec = (parseFloat(NetOrderValueBase) / ConvRate).toFixed(ValDecDigit);

                                currentRow.find("#NetOrderValueSpe").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#TxtNetValueInBase").val(parseFloat(NetOrderValueBase).toFixed(ValDecDigit));
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()).toFixed(ValDecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        //FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        //var oc_amt = currentRow.find("#TxtOtherCharge").val();
                        //if (oc_amt == ".") {
                        //    oc_amt = 0;
                        //}
                        //if (oc_amt != "" && oc_amt != ".") {
                        //    oc_amt = parseFloat(oc_amt);
                        //}
                        //FinalFGrossAmtOR = FinalFGrossAmtOR + oc_amt;
                        currentRow.find("#TxtNetValueInBase").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function ResetGRN_DDL_Detail() {
    debugger;
    $("#GRN_Date").val("");
    var DocNo = $('#ddlGoodReceiptNoteNo').val();
    $("#ddlGoodReceiptNoteNo>optgroup>option[value='" + DocNo + "']").select2().hide();

    $('#ddlGoodReceiptNoteNo').val("---Select---").prop('selected', true);
    $('#ddlGoodReceiptNoteNo').trigger('change'); // Notify any JS components that the value changed
    $('#ddlGoodReceiptNoteNo').select2('close');

    $("#SpanGRNNoErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
    $("#ddlGoodReceiptNoteNo").css("border-color", "red");
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
    $("#VoucherDetail > tbody > tr #hfAccID[value=" + hdn_acc_id + "]").closest('tr').each(function () {
        let row = $(this);
        let hf_acc_id = row.find("#hfAccID").val();
        let gltype = row.find("#type").val();
        if (gltype == "VItm" && hf_acc_id == hdn_acc_id) {
            row.find("#td_GlAccName").text(Acc_Name);
            row.find("#tdhdn_GlAccId").text(Acc_ID);
            row.find("#hfAccID").val(Acc_ID);
            row.find("#txthfAccID").val(Acc_Name);
        }

    });
    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
    $("#hdnAccID").val(Acc_ID);
}
function OnClickSupplierInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    var Supp_id = $('#SupplierName').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
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
    sessionStorage.removeItem("PI_TaxCalcDetails");
    sessionStorage.removeItem("PI_TransType");
}
function OnClickVarienceDetailIButton(e) {
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID").val();
    var ItmName = clickedrow.find("#TxtItemName").val();
    var GRNDate = clickedrow.find("#hfGRNDate").val();
    var GRN_dt = clickedrow.find("#TxtGrnDate").val();
    var GRNNo = clickedrow.find("#TxtGrnNo").val();
    var Uom = clickedrow.find("#TxtUOM").val();
    var sub_item = clickedrow.find("#sub_item").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var DnForVarncQty = $("#hdn_DnForVarianceQty").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalPurchaseInvoice/VarienceDetails",
        data: { GRNNo: GRNNo, GRNDate: GRNDate, ItmCode: ItmCode, DocumentMenuId, SubItem: sub_item, DnForVarncQty: DnForVarncQty },
        success: function (data) {
            debugger;
            $("#VarianceDetailPopUp").html(data);
            Cmn_rowHighLight();
            $("#GRNNumber").val(GRNNo);
            $("#GRNDate").val(GRN_dt);
            $("#GRN_Date_hdn").val(GRNDate);
            $("#GrnItemName").val(ItmName);
            $("#GrnItemId").val(ItmCode);
            $("#UOM").val(Uom);
        }
    })
}

//------------------Tax Amount Calculation------------------//
function OnClickTaxCalBtn(e) {
    debugger;
    //$("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    //$("#SpanTax_Template").text("");
    // $("#SpanTax_Template").css("display", "none");
    var SOItemListName = "#TxtItemName";
    var SNohiddenfiled = "PI";
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
    var item_gross_val = currentRow.find("#TxtItemGrossValueInBase").val();
    var mr_no = currentRow.find("#TxtGrnNo").val();
    var mr_date = currentRow.find("#hfGRNDate").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    if (GstApplicable == "Y") {
        //if ($("#ForDisable").val() == "Disable") {
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
function OnClickSaveAndExit(OnAddGRN, AutoAplyTaxPI) {
    //var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
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
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            }
            else {
                if (TaxItemID == TaxItmCode && DocNo == GRNNo && DocDate == GRNDate) {
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
                    var TaxAccId = currentRow.find("#TaxAccId").text();
                    var TaxRecov = currentRow.find("#TaxRecov").text();
                    NewArr.push({ /*UserID: UserID, */DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov })
                }
            }
        });

        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccID}</td>
                </tr>`);

            NewArr.push({
                DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName
                , TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage
                , TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount
                , TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: TaxRecov
            })
        });
    }
    else {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();

            $("#" + HdnTaxCalculateTable + " tbody").append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccID}</td>
                </tr>`);

            NewArr.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov })
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
    } else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode && GRNNoTbl == GRNNo && GRNDateTbl == GRNDate) {
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
                }
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(NetOrderValueBase) / ConvRate).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                //FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt1 = currentRow.find("#Txtitem_tax_amt").val();
            if (ItemTaxAmt1 != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
        debugger;
    }
    //if ($("#taxTemplate").text() == "GST Slab") {
    if (AutoAplyTaxPI != "AutoAplyTaxPI") {
        GetAllGLID();
    }
    //}
}
function OnClickReplicateOnAllItems() {
    //var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTabl = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTabl = "Hdn_OCTemp_TaxCalculatorTbl";
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
        var TaxAccId = currentRow.find("#AccID").text();
        TaxCalculationList.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
        TaxCalculationListFinalList.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "POInvItmDetailsTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = currentRow.find("#TxtGrnNo").val();
                GRNDateTbl = currentRow.find("#hfGRNDate").val();
                var ItemType = currentRow.find("#hdnItemType").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                } else {
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#TxtItemGrossValue").val();
                }

                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
                //if (ItemType == "Consumable") {
                for (i = 0; i < TaxCalculationList.length; i++) {

                    if (ItemType == "Consumable" || ItemType == "Service") {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        TaxPercentage = TaxCalculationList[i].TaxPercentage;
                        var TaxLevel = TaxCalculationList[i].TaxLevel;
                        var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                        var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                        var TaxAccId = TaxCalculationList[i].TaxAccId;
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
                        NewArray.push({ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })

                    }
                    else {
                        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
                            debugger;
                            var currentRow = $(this);
                            var TaxItmCode = currentRow.find("#TaxItmCode").text();
                            var TaxName = currentRow.find("#TaxName").text();
                            var TaxNameID = currentRow.find("#TaxNameID").text();
                            var TaxLevel = currentRow.find("#TaxLevel").text();
                            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                            var TotalTaxAmt = currentRow.find("#TotalTaxAmount").text();
                            var TaxAmount = currentRow.find("#TaxAmount").text();
                            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                            var TaxPercentage = currentRow.find("#TaxPercentage").text();
                            var TaxAccId = currentRow.find("#TaxAccId").text();

                            if (ItemCode == TaxItmCode) {
                                NewArray.push({ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                            }
                        });
                    }

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
                            var TaxAccId = NewArray[k].TaxAccId;
                            var TotalTaxAmt = NewArray[k].TotalTaxAmount;
                            if (CitmTaxItmCode != TaxItmCode && GRNNo == DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId })
                            }
                        }
                    }
                }
                //}

            });
        }
    }
    //$("#POInvItmDetailsTbl >tbody >tr").each(function() {
    //    var currentRow = $(this);
    //    var ItemType = currentRow.find("#hdnItemType").val();
    //    var ItmCode = currentRow.find("#hfItemID").val();
    //    if (ItemType == "Consumable") {
    //        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function () {
    //            var cRow = $(this);
    //            var ItmId = cRow.find("#TaxItmCode").text();
    //            if (ItmCode == ItmId) {
    //                cRow.remove();
    //            }
    //        });
    //    }
    //});
    $("#" + HdnTaxCalculateTabl + " >tbody >tr").remove();

    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        $('#' + HdnTaxCalculateTabl + ' tbody').append(`<tr id="R${++rowIdx}">
            
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
            <td id="TaxAccId">${TaxCalculationListFinalList[i].TaxAccId}</td>
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
    } else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID && GRNNoTbl == AGRNNo && GRNDateTbl == AGRNDate) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                            currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            NetOrderValueSpec = (parseFloat(NetOrderValueBase) / ConvRate).toFixed(ValDecDigit);
                            currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                            //FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(ValDecDigit);
                            currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()).toFixed(ValDecDigit);
                    currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                    FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(ValDecDigit);
                    currentRow.find("#TxtNetValue").val(FinalGrossAmt);

                    currentRow.find("#TxtNetValueInBase").val(FGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValue").val(FGrossAmt);
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValueInBase").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
        GetAllGLID();
    }

}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertPIDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    debugger;

    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/
    CMNBindTaxAmountDeatils(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}

function OnClickSaveAndExit_OC_Btn() {
    debugger;
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueSpe", "#NetOrderValueInBase")
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    //var DecDigit = $("#ValDigit").text();
    //var TotalGAmt = $("#TxtGrossValue").val(); //Commented by Suraj on 27-03-2024
    var TotalGAmt = 0;
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var docid = $("#DocumentMenuId").val();
    if (docid == "105101145") {
        var TotalOcGrossItemWise = 0;
        $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
            debugger;
            var currentRow = $(this);
            var hdnItemType = currentRow.find("#hdnItemType").val();
            if (hdnItemType == "Stockable") {
                TotalOcGrossItemWise = parseFloat(TotalOcGrossItemWise) + parseFloat(currentRow.find("#TxtItemGrossValueInBase").val());
            }
        });
        TotalGAmt = TotalOcGrossItemWise;
    }
    else {
        TotalGAmt = $("#TxtGrossValueInBase").val();
    }
    if (docid == "105101145") {
        $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
            debugger;
            var currentRow = $(this);
            var GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
            var hdnItemType = currentRow.find("#hdnItemType").val();
            if (hdnItemType == "Stockable") {
                if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
                    var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
                    var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
                    if (parseFloat(OCAmtItemWise) > 0) {
                        currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
                    }
                    else {
                        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
                    }
                }
                else {
                    currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
            });
    }
    else {
        $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
            debugger;
            var currentRow = $(this);
            var GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
            if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
                var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
                var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
                if (parseFloat(OCAmtItemWise) > 0) {
                    currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
                }
                else {
                    currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
                }
            } else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        });
    }
    
    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
        debugger;
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
    if (docid == "105101145") {
        if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
            var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
            var Sno = 0;
            $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var hdnItemType = currentRow.find("#hdnItemType").val();
                if (hdnItemType == "Stockable") {
                    var OCValue = currentRow.find("#TxtOtherCharge").val();
                    currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
                    return false;
                }     
            });
        }
        if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
            var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
            var Sno = 0;
            $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
                var currentRow = $(this);
                var OCValue = currentRow.find("#TxtOtherCharge").val();
                var hdnItemType = currentRow.find("#hdnItemType").val();
                if (hdnItemType == "Stockable") {
                    currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
                    return false;
                }
                
            });
        }
    }
    else {
        if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
            var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
            var Sno = 0;
            $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                Sno = Sno + 1;
                var OCValue = currentRow.find("#TxtOtherCharge").val();
                if (Sno == "1") {
                    currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
                }
            });
        }
        if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
            var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
            var Sno = 0;
            $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
                debugger;
                Sno = Sno + 1;
                var currentRow = $(this);
                var OCValue = currentRow.find("#TxtOtherCharge").val();
                if (Sno == "1") {
                    currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
                }
            });
        }
    }
   
    $("#POInvItmDetailsTbl >tbody >tr ").closest('tr').each(function () {
        debugger;
        var currentRow = $(this);

        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueSpec = (parseFloat(POItm_NetOrderValueBase) / ConvRate);
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
        //FinalPOItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(ValDecDigit);
        currentRow.find("#TxtNetValueInBase").val((parseFloat(POItm_NetOrderValueBase)).toFixed(ValDecDigit));
    });
    CalculateAmount();
};
function CalculateAmount() {
    //var DecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var NetDnAmt = parseFloat(0).toFixed(ValDecDigit);

    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TxtItemGrossValueInBase").val() == "" || currentRow.find("#TxtItemGrossValueInBase").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(ValDecDigit);
        }

        if (currentRow.find("#TxtNetValue").val() == "" || currentRow.find("#TxtNetValue").val() == null || currentRow.find("#TxtNetValue").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TxtNetValueInBase").val() == "" || currentRow.find("#TxtNetValueInBase").val() == null || currentRow.find("#TxtNetValueInBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValueInBase").val())).toFixed(ValDecDigit);
        }
        NetDnAmt = parseFloat(NetDnAmt) + parseFloat(CheckNullNumber(currentRow.find("#TxtDebitNoteValue").val()));

    });
    $("#TxtGrossValue").val(GrossValue);

    var oc_amount = $("#TxtDocSuppOtherCharges").val();
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValueInBase) + parseFloat(CheckNullNumber(oc_amount));
    NetOrderValueSpec = NetOrderValueBase / parseFloat(CheckNullNumber(ConvRate));

    $("#TxtGrossValueInBase").val(GrossValueInBase);
    $("#TxtTaxAmount").val(TaxValue);
    $("#NetOrderValueSpe").val(NetOrderValueSpec.toFixed(ValDecDigit));
    $("#TxtVarDn_Amount").val(NetDnAmt);/* For Variance Total DN Amt */
    if ($("#chk_roundoff").is(":checked")) {
        var roundoff_netval = Math.round(NetOrderValueBase);
        var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
        $("#NetOrderValueInBase").val(f_netval);
    }
    else {
        $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(ValDecDigit));
    }
    debugger;


}
function SetOtherChargeVal() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
    })
}
function BindOtherChargeDeatils(val) {
    //var DecDigit = $("#ValDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            if (DocumentMenuId == "105101145" || DocumentMenuId == "105101140125") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
                      <td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }
            //let ocAmt = currentRow.find("#OCAmount").text();
            let ocAmt = currentRow.find("#OcAmtBs").text();

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td >${currentRow.find("#td_OCSuppName").text()}</td>
<td id="OCAmtSp1" align="right">${ocAmt}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(ocAmt)).toFixed(ValDecDigit);
            if (DocumentMenuId == "105101145" || DocumentMenuId == "105101140125") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
            }
        });
        if (val == "") {
            GetAllGLID();
        }
    }
    $("#_OtherChargeTotal").text(TotalAMount);
    if (DocumentMenuId == "105101145" || DocumentMenuId == "105101140125") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
        GetAllGLID();
    }

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
//------------------End------------------//


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
    //var INVStatus = "";
    //INVStatus = $('#hfPIStatus').val().trim();
    //if (INVStatus === "D" || INVStatus === "F") {

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
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DPIDate = $("#Inv_date").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DPIDate
        },
        success: function (data) {
            /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
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
                /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Btn_Approve").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");

            }
        }
    });
    /*End to chk Financial year exist or not*/
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
    var mailerror = "";
    //var VoucherNarr = $("#PurchaseVoucherRaisedAgainstInv").text()
    var VoucherNarr = $("#hdn_Nurration").val()
    var BP_VoucherNarr = $("#hdn_BP_Nurration").val()
    var DN_VoucherNarr = $("#hdn_DN_Nurration").val()
    var DN_VarNarr = $("#hdn_DN_VarNurration").val()

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#InvoiceNumber").val();
    INVDate = $("#Inv_date").val();
    WF_Status1 = $("#WF_Status1").val();
    var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();

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
        var pdfAlertEmailFilePath = "PurchaseInvoice_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/LocalPurchaseInvoice/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/Approve_PurchaseInvoice?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&VoucherNarr=" + VoucherNarr + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + WF_Status1 + "&Bp_Nurr=" + BP_VoucherNarr + "&Dn_Nurration=" + DN_VoucherNarr + "&DN_VarNarr=" + DN_VarNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(INV_NO, INV_Date, docid, fileName) {
//    debugger;
//    var Inv_no = INV_NO;
//    var Inv_Date = INV_Date;
//    var docid = docid;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/LocalPurchaseInvoice/SavePdfDocToSendOnEmailAlert",
//        data: { docid: docid,Inv_no: INV_NO, Inv_Date: INV_Date,fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        //var Doc_Status = clickedrow.children("#Doc_Status").text();

        var Doc_Status = $("#hfPIStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
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

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramt").text(); //commented reverted by Suraj on 05-06-2025 //commented by Suraj on 30-03-2024
    var CstCrtAmt = clickedrow.find("#cramt").text();
    //var CstDbAmt = clickedrow.find("#dramtInBase").text();//commented by Suraj on 05-06-2025
    //var CstCrtAmt = clickedrow.find("#cramtInBase").text();
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

/***--------------------------------Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    debugger;

    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNo").text();

    var ProductNm = clickdRow.find("#TxtItemName").val();
    var ProductId = clickdRow.find("#hfItemID").val();
    var GrnNo = clickdRow.find("#TxtGrnNo").val();
    var GRNDate = clickdRow.find("#hfGRNDate").val();
    var UOM = clickdRow.find("#TxtUOM").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var IsDisabled = "Y";//$("#DisableSubItem").val();

    var Sub_Quantity = 0;
    if (flag == "Quantity") {
        Sub_Quantity = clickdRow.find("#TxtReceivedQuantity").val();
    }
    else {
        ProductNm = $("#GrnItemName").val();
        UOM = $("#UOM").val();
        Doc_no = $("#GRNNumber").val();
        Doc_dt = $("#GRN_Date_hdn").val();
        ProductId = $("#GrnItemId").val();
    }
    if (flag == "BillQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdBill_qty").text();
    }
    if (flag == "RecdQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRec_qty").text();
    }
    if (flag == "AccQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdAcc_qty").text();
    }
    if (flag == "RejQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRej_qty").text();
    }
    if (flag == "RewQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRew_qty").text();
    }
    if (flag == "ShortQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdShort_qty").text();
    }
    if (flag == "SampleQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdSample_qty").text();
    }
    if (flag == "InvQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdInv_qty").text();
    }
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").find("#subItemSrcDocNo[value='" + GrnNo + "']").closest("tr").each(function () {

        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.sub_item_name = row.find('#subItemName').val();
        List.qty = row.find('#subItemQty').val();
        List.src_doc_no = row.find('#subItemSrcDocNo').val();
        List.src_doc_dt = row.find('#subItemSrcDocDate').val();
        NewArr.push(List);
    });

    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalPurchaseInvoice/GetSubItemDetails",
        data: {
            Item_id: ProductId,
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
            if (flag == "Quantity") {
                $("#SubItemPopUp").css("max-width", "600px");
            } else {
                $("#SubItemPopUp").css("max-width", "1100px");
            }
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    //return Cmn_CheckValidations_forSubItems("GRN_ItmDetailsTbl", "SNo", "IDItemName", "ord_qty_spec", "SubItemOrderSpecQty", "Y");
}

/***--------------------------------Sub Item Section End-----------------------------------------***/
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

/****======------------Reason For Reject And Rework section Start-------------======****/

function onClickReasonRemarks(e, flag) {
    let Row = $(e.target).closest('tr');
    let ItemName = $("#GrnItemName").val();
    let ItemId = $("#GrnItemId").val();
    let UOM = $("#UOM").val();
    let ReasonRemarks = "";

    $("#RR_Remarks").attr("readonly", true);
    $("#RR_btnClose").attr("hidden", false);
    $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").text();
        let rejQty = Row.find("#V_tdRej_qty").text();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);
        } else {
            Row.find("#ReasonForReject").text("");
            ReasonRemarks = "";
        }
    }
    else if (flag == "rework") {
        ReasonRemarks = Row.find("#ReasonForRework").text();

        let rwkQty = Row.find("#V_tdRew_qty").text();
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

/***-------------------Roundoff------------------------------------***/
function checkMultiSupplier() {
    //let countSupp = 0;
    //$("#VoucherDetail tbody tr").each(function () {
    //    let row = $(this);
    //    if (row.find("#type").val() == "Supp") {
    //        countSupp++;
    //    }
    //});
    //if (countSupp > 1) {
    //    $("#chk_roundoff").attr("disabled", true);
    //    return false;
    //} else {
    //    return true;
    //}
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
                CalculateTaxExemptedAmt(currentrow, "N")
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


                                    var grossval = $("#TxtGrossValueInBase").val();
                                    var taxval = $("#TxtTaxAmount").val();

                                    //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
                                    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

                                    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

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

                                                //let valInBase = $("#TxtGrossValueInBase").val();
                                                //$("#TxtGrossValueInBase").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                                            }
                                            if ($("#m_round").is(":checked")) {
                                                //var finnetval = netval.split('.');
                                                var decval = '0.' + finnetval[1];
                                                fnetval = parseFloat(netval) - parseFloat(decval);
                                                $("#pm_flagval").val($("#m_round").val());

                                                //let valInBase = $("#TxtGrossValueInBase").val();
                                                //$("#TxtGrossValueInBase").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                                            }
                                            var roundoff_netval = Math.round(fnetval);
                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                            $("#NetOrderValueInBase").val(f_netval);
                                            if (!ord_type_imp) {
                                                $("#NetOrderValueSpe").val(f_netval);
                                            }

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
                CalculateTaxExemptedAmt(currentrow, "N")
            });
        }
        //var Len = $("#POInvItmDetailsTbl > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        GetAllGLID();
    }
}

/***----------------------end-------------------------------------***/

/***---------------------------GL Voucher Entry-----------------------------***/
function GetAllGLID() {
    GetAllGL_WithMultiSupplier();
}
/*GetAllGLIDForImport() function is use for both domestinc and import*/
async function GetAllGL_WithMultiSupplier() {
    debugger;
    if ($("#POInvItmDetailsTbl > tbody > tr").length == 0) {
        return false;
    }
    if (CheckPI_ItemValidations() == false) {
        return false;
    }
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    var DocStatus = $('#hfPIStatus').val().trim();
    /////////////////12-07-2023 Comment by Shubham Maurya //////////
    //var NetInvValue = $("#NetOrderValueSpe").val();
    var NetInvValue = $("#NetOrderValueInBase").val();
    //var NetInvValue = $("#TxtGrossValueInBase").val();
    //var NetTaxValue = $("#TxtTaxAmount").val();
    var conv_rate = $("#conv_rate").val();
    //var ValueWithoutTax = parseFloat(NetInvValue);
    //var ValDecDigit = $("#ValDigit").text();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    //SuppValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "";
    if ($("#OrderTypeL").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "I";
    }
    var curr_id = $("#ddlCurrency").val();
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
        var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
        var ItmGrossValInBase = currentRow.find("#TxtItemGrossValueInBase").val();
        var TxtGrnNo = currentRow.find("#TxtGrnNo").val();
        var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
        var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
        var TxaExanted = "N";
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExanted = "Y";
            TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
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
        } else {
            //if (GLDetail.findIndex((obj => obj.id == supp_id)) > -1) {
            //    objIndex = GLDetail.findIndex((obj => obj.id == supp_id));
            //    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(oc_amt_wt);
            //    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(oc_amt_bs_wt);
            //}
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
    //$("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#taxID").text();
    //    var tax_acc_id = currentRow.find("#taxAccID").text();
    //    var tax_amt = currentRow.find("#TotalTaxAmount").text();
    //    var TaxPerc = currentRow.find("#TaxPerc").text();
    //    GLDetail.push({
    //        comp_id: Compid, id: tax_acc_id, type: "Tax", doctype: InvType, Value: tax_amt
    //        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
    //        , Entity_id: tax_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt
    //    });
    //});
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

        //if (TaxRecov == "N") {
        //    if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
        //        var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
        //        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
        //        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
        //    }
        //} else {
        //    GLDetail.push({
        //        comp_id: Compid, id: tax_acc_id, type: "Tax", doctype: InvType, Value: tax_amt
        //        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
        //        , Entity_id: tax_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt
        //    });
        //}


    });
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            var DocNo = currentRow.find("#DocNo").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();

            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
            if (ClaimITC) {
                if (TaxRecov == "N") {
                    //if (GLDetail.findIndex((obj => obj.id == supp_acc_id)) > -1) {
                    //    var objIndex = GLDetail.findIndex((obj => obj.id == supp_acc_id));
                    //    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                    //    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                    //}
                }
                else {
                    if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
                        if (GLDetail.findIndex((obj => obj.id == TaxID && obj.type == "RCM")) > -1) {
                            objIndex = GLDetail.findIndex((obj => obj.id == TaxID && obj.type == "RCM"));
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

            //if (TxaExantedItemList.findIndex((obj => (obj.item_id + obj.doc_no) == (TaxItmCode + DocNo))) == -1) {
            //    if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
            //        objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
            //        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
            //        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
            //    } else {
            //        GLDetail.push({
            //            comp_id: Compid, id: TaxAccID, type: "RCM", doctype: InvType, Value: TaxVal
            //            , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
            //            , Entity_id: TaxID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt

            //        });
            //    }

            //}

        });
    }

    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        //tds_amt = parseFloat(tds_amt).toFixed(0);
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
        // debugger;
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

    /*--------------------------------------Debit note against variance quantity---------------------------------------*/
    var Cal_dn_Amt = 0;
    let varDetails = [];
    if (sessionStorage.getItem("VarianceDataList") != null && sessionStorage.getItem("VarianceDataList") != "") {
        varDetails = JSON.parse(sessionStorage.getItem("VarianceDataList"));
    }
    if (sessionStorage.getItem("VarianceDataList") != null && sessionStorage.getItem("VarianceDataList") != "") {
        //varDetails = JSON.parse(sessionStorage.getItem("VarianceDataList"));
        varDetails.map((item) => {
            if (item.include == "Y") {
                if (parseFloat(item.dn_amt) > 0) {
                    Cal_dn_Amt = parseFloat(Cal_dn_Amt) + parseFloat(item.dn_amt);
                    if (GLDetail.findIndex((obj => obj.id == item.item_id && obj.type == "VItm")) > -1) {

                        objIndex = GLDetail.findIndex((obj => obj.id == item.item_id && obj.type == "VItm"));
                        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(item.dn_amt);
                        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(item.dn_amt);
                    }
                    else {

                        let filteredGlDtl = GLDetail.filter((obj => obj.id == item.item_id && obj.type == "Itm"));
                        let itmAccId = "";
                        if (filteredGlDtl != null && filteredGlDtl.length > 0) {
                            itmAccId = filteredGlDtl[0].acc_id;
                        }
                        GLDetail.push({
                            comp_id: Compid, id: item.item_id, type: "VItm", doctype: InvType, Value: item.dn_amt
                            , ValueInBase: item.dn_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "VItm", parent: supp_acc_id
                            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: IsNull(itmAccId, "")
                        });
                    }
                }

            }


        })

    }

    if (Cal_dn_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "VSupp", doctype: InvType
            , Value: Cal_dn_Amt, ValueInBase: Cal_dn_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "VSupp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }

    if (sessionStorage.VarianceTaxDataList != null && sessionStorage.VarianceTaxDataList != "") {

        let arrVarTaxDtl = JSON.parse(sessionStorage.VarianceTaxDataList);

        arrVarTaxDtl.map((item) => {

            let varItemDtl = varDetails.filter(v => v.item_id == item.item_id && v.var_type == item.var_type);
            if (varItemDtl.length > 0) {
                if (varItemDtl[0].include == "Y") {
                    if (parseFloat(varItemDtl[0].dn_amt) > 0) {
                        let TaxRecov = item.tax_recov;
                        let ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + item.item_id + "']").closest('tr');
                        let ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
                        if (ClaimITC) {
                            if (TaxRecov == "N") {

                            }
                            else {
                                if (GLDetail.findIndex((obj => obj.id == item.item_id && obj.type == "VItm")) > -1) {

                                    objIndex = GLDetail.findIndex((obj => obj.id == item.item_id && obj.type == "VItm"));
                                    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) - parseFloat(item.tax_val);
                                    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) - parseFloat(item.tax_val);
                                }

                                if (TxaExantedItemList.findIndex(obj => obj.item_id == item.item_id) == -1) {
                                    if (GLDetail.findIndex((obj => obj.id == item.tax_id && obj.type == "VTax")) > -1) {

                                        objIndex = GLDetail.findIndex((obj => obj.id == item.tax_id && obj.type == "VTax"));
                                        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(item.tax_val);
                                        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(item.tax_val);
                                    } else {

                                        GLDetail.push({
                                            comp_id: Compid, id: item.tax_id, type: "VTax", doctype: InvType, Value: item.tax_val
                                            , ValueInBase: item.tax_val, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "VTax", parent: supp_acc_id
                                            , Entity_id: item.tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                                        });
                                    }
                                    if (GstCat == "UR") {
                                        if (GLDetail.findIndex((obj => obj.id == item.tax_id && obj.type == "VRCM")) > -1) {

                                            objIndex = GLDetail.findIndex((obj => obj.id == item.tax_id && obj.type == "VRCM"));
                                            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(item.tax_val);
                                            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(item.tax_val);

                                        } else {
                                            GLDetail.push({
                                                comp_id: Compid, id: item.tax_id, type: "VRCM", doctype: InvType, Value: item.tax_val
                                                , ValueInBase: item.tax_val, DrAmt: item.tax_rate.toString().replace("%", ""), CrAmt: 0, TransType: TransType, gl_type: GstType
                                                , parent: supp_acc_id
                                                , Entity_id: item.tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""

                                            });
                                        }
                                    }


                                }
                            }
                        }

                    }

                }
            }

        });
    }

    /*--------------------------------------Debit note against variance quantity End---------------------------------------*/

    await Cmn_GLTableBind(supp_acc_id, GLDetail, "Purchase")
    //await $.ajax({
    //    type: "POST",
    //    url: "/Common/Common/GetGLDetails",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify({ GLDetail: GLDetail }),
    //    success: function (data) {
    //        debugger;
    //        if (data == 'ErrorPage') {
    //            Comn_ErrorPage();
    //            return false;
    //        }

    //        if (data !== null && data !== "") {
    //            var arr = [];
    //            arr = JSON.parse(data);
    //            var Voudet = 'Y';
    //            $('#VoucherDetail tbody tr').remove();
    //            if (arr.Table1.length > 0) {
    //                var errors = [];
    //                var step = [];
    //                for (var i = 0; i < arr.Table1.length; i++) {

    //                    if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
    //                        errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
    //                        step.push(parseInt(i));
    //                        Voudet = 'N';
    //                    }
    //                }
    //                var arrayOfErrorsToDisplay = [];
    //                $.each(errors, function (i, error) {
    //                    arrayOfErrorsToDisplay.push({ text: error });
    //                });
    //                Swal.mixin({
    //                    confirmButtonText: 'Ok',
    //                    type: "warning",
    //                }).queue(arrayOfErrorsToDisplay)
    //                    .then((result) => {
    //                        if (result.value) {

    //                        }
    //                    });
    //            }
    //            if (Voudet == 'Y') {
    //                if (arr.Table.length > 0) {
    //                    $('#VoucherDetail tbody tr').remove();
    //                    //let supp_acc_id = $("#supp_acc_id").val(); 
    //                    var arrSupp = arr.Table.filter(v => (v.type == "Supp" || v.type == "Bank"));
    //                    var mainSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id);
    //                    var NewArrSupp = arrSupp.filter(v => v.acc_id != supp_acc_id);
    //                    NewArrSupp.unshift(mainSuppGl[0]);
    //                    arrSupp = NewArrSupp;
    //                    for (var j = 0; j < arrSupp.length; j++) {
    //                        let supp_id = arrSupp[j].id;
    //                        let arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank")));
    //                        //let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && (v.type == "OC" || v.type == "Itm" || v.type == "Tax" || v.type == "RCM")));
    //                        let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "OcTax"));

    //                        let RcmValue = 0;
    //                        let RcmValueInBase = 0;
    //                        arr.Table.filter(v => (v.parent == supp_id && v.type == "RCM"))
    //                            .map((res) => {
    //                                RcmValue = RcmValue + res.Value;
    //                                RcmValueInBase = RcmValueInBase + res.ValueInBase;
    //                            });


    //                        arrDetail[0].Value = arrDetail[0].Value - RcmValue;
    //                        arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

    //                        let rowSpan = 1;//arrDetailDr.length + 1;
    //                        let GlRowNo = 1;
    //                        // First Row Generated here for all GL Voucher 
    //                        let vouType = "";
    //                        if (arrDetail[0].type == "Supp")
    //                            vouType = "PV"
    //                        if (arrDetail[0].type == "Bank")
    //                            vouType = "BP"

    //                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                            , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
    //                            , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);

    //                        let ArrTax = [];
    //                        let ArrGlDetailsDr = [];
    //                        for (var k = 0; k < arrDetailDr.length; k++) {
    //                            //GlRowNo++;
    //                            // Row Generated here for Other charge and Tax on item
    //                            let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                            if (getAccIndex > -1) {
    //                                ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                            } else {
    //                                //rowSpan++;
    //                                if (arrDetailDr[k].type == "Tax") {
    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                                    } else {
    //                                        //rowSpan++;
    //                                        ArrTax.push(arrDetailDr[k]);
    //                                    }
    //                                    // ArrTax.push(arrDetailDr[k]);
    //                                }
    //                                else if (arrDetailDr[k].type == "OcTax") {
    //                                }
    //                                else {
    //                                    ArrGlDetailsDr.push(arrDetailDr[k]);
    //                                }
    //                            }



    //                            if (arrDetailDr[k].type == "OC") {

    //                                let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].oc_id && v.type == "OcTax"));
    //                                // Grouping the similer tax account
    //                                for (var l = 0; l < arrDetailOCDr.length; l++) {

    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
    //                                    } else {
    //                                        //rowSpan++;
    //                                        ArrTax.push(arrDetailOCDr[l]);
    //                                    }
    //                                }
    //                                //Updating rowspan as Tax on OC added

    //                            }
    //                        }
    //                        var ArrGLDetailRCM = []
    //                        for (var i = 0; i < ArrGlDetailsDr.length; i++) {

    //                            if (ArrGlDetailsDr[i].type == "RCM") {
    //                                ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

    //                            } else {
    //                                GlRowNo++;
    //                                rowSpan++;
    //                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                    , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
    //                                    vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                    , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                )
    //                            }

    //                        }
    //                        for (var i = 0; i < ArrTax.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            // Row Generated here for Tax on Other Charge
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
    //                                , ArrTax[i].Value, ArrTax[i].ValueInBase, 0, 0, vouType,
    //                                ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
    //                            )

    //                        }
    //                        for (var i = 0; i < ArrGLDetailRCM.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
    //                                , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
    //                                , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
    //                                , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
    //                            )
    //                        }
    //                        $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
    //                    }

    //                }
    //            }
    //            BindDDLAccountList();
    //            CalculateVoucherTotalAmount();
    //        }
    //    }
    //});

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
    //var hdnAccID = $("#hdnAccID").val();
    //if (hdnAccID != null && hdnAccID != "") {
    //    if (type == 'Itm') {
    //        FieldType = `<div class="col-sm-12 lpo_form no-padding">
    //                        <select class="form-control" id="Acc_name_${hfSrNo}" onchange ="OnChangeAccountName(${hfSrNo},event)">
    //                         </select>
    //                    </div>`;
    //        $("#hdnAccID").val(hdnAccID);
    //    }
    //    else {
    //        FieldType = `${acc_name}`;
    //    }
    //}
    //else {
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
    //}


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
    var curr_id = $("#ddlCurrency").val();
    //var ValDecDigit = $("#ValDigit").text();///Amount
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
                        //debugger;
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

    //var ValDecDigit = $("#ValDigit").text();
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
function Get_Gl_Narration(VouType, bill_bo, bill_date, type) {
    let narration;

    if (VouType === "DN") {
        if (["VSupp", "VItm", "VTax", "VRCM"].includes(type)) {
            narration = $("#hdn_DN_VarNurration").val();
        } else {
            narration = $("#hdn_DN_Nurration").val();
        }
    }
    else if (VouType === "BP") {
        narration = $("#hdn_BP_Nurration").val();
    }
    else {
        narration = $("#hdn_Nurration").val();
    }

    return narration
        .replace("_bill_no", bill_bo)
        .replace("_bill_dt", IsNull(bill_date, "") == "" ? "" : moment(bill_date).format("DD-MM-YYYY"))
        .replace("_nrr_supp_name", $("#SupplierName option:selected").text());
}

/***---------------------------GL Voucher Entry End-----------------------------***/

/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/

/*----------------------------TDS Section-----------------------------*/
function OnClickTDSCalculationBtn() {
    debugger;
    //var ValDigit = $("#ValDigit").text();
    const GrVal = $("#TxtGrossValue").val();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal)).toFixed(ValDecDigit);
    // Added by Suraj Maurya on 07-12-2024
    const NetVal = $("#NetOrderValueInBase").val();
    const ToTdsAmt_IT = parseFloat(CheckNullNumber(NetVal)).toFixed(ValDecDigit);
    // Added by Suraj Maurya on 07-12-2024 End
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
        //Added by Suraj Maurya on 06-12-2024
        //var ToTdsAmt = 0;
        let TdsAssVal_applyOn = "ET";
        //ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
            //ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
        }
        else if ($("#TdsAssVal_Custom").is(":checked")) {/* Added by Suraj Maurya on 14-05-2025 */
            TdsAssVal_applyOn = "CA"
        }
        //Added by Suraj Maurya on 06-12-2024 End
        //var DecDigit = $("#ValDigit").text();
        var TotalTDS_Amount = $('#TotalTDS_Amount').text();
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        //let NewArr = [];
        var rowIdx = 0;
        var GstCat = $("#Hd_GstCat").val();
        //var ToTdsAmt = 0;
        //if (GstCat == "UR") {
        //    ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val())) + parseFloat(CheckNullNumber($("#TxtOtherCharges").val()));

        //} else {
        //    ToTdsAmt = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        //}
        //ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
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

                //NewArr.push({ /*UserID: UserID, */DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
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

                //NewArr.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
            });

        }
        var TotalAMount = parseFloat(0).toFixed(ValDecDigit);

        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
        });

        $("#TxtTDS_Amount").val(TotalAMount);
        /*Added by Surbhi on 11/09/2025 for Custome TDS Clear*/
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
        //$("#PopUp_TDSCalculate").html("");
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
    //var ValDigit = $("#ValDigit").text();
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const TotalVal = CC_Clicked_Row.find("#OCTotalTaxAmt").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    $("#TDS_AssessableValue").val(parseFloat(CheckNullNumber(ToTdsAmt)).toFixed(ValDecDigit));

    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, TotalVal, "OcTds");/* "OcTds" passed by Suraj Maurya on 14-05-2025 */

}
function OnClickTP_TDS_SaveAndExit() {
    debugger

    var TotalTDS_Amount = $('#TotalTDS_Amount').text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //var TDS_OcId = $("#TDS_oc_id").text();
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    //var TDS_SuppId = $("#TDS_supp_id").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var GstCat = $("#Hd_GstCat").val();
    //Added by Suraj Maurya on 06-12-2024
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
    }
    //Added by Suraj Maurya on 06-12-2024 End
    //ToTdsAmt = parseFloat(CheckNullNumber(CC_Clicked_Row.find("#OCAmount").text()));
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
    //GetAllGLID();
}
function SetTds_Amt_To_OC() {
    //var ValDigit = $("#ValDigit").text();
    var TotalAMount = parseFloat(0);
    //var DecDigit = $("#ValDigit").text();
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
    });

    CC_Clicked_Row.find("#OC_TDSAmt").text(parseFloat(CheckNullNumber(TotalAMount)).toFixed(ValDecDigit));
    CC_Clicked_Row = null;
}

/***------------------------------TDS On Third Party End------------------------------***/

function onClickBtnVarianceQtyDtls(e) {
    /* Created by Suraj Maurya on 25-03-2025 */
    const row = $(e.target).closest("tr");
    let ItmCode = row.find("#hfItemID").val();
    let ItmName = row.find("#TxtItemName").val();
    let GRNDate = row.find("#hfGRNDate").val();
    let GRN_dt = row.find("#TxtGrnDate").val();
    let GRNNo = row.find("#TxtGrnNo").val();
    let Uom = row.find("#TxtUOM").val();
    let sub_item = row.find("#sub_item").val();
    let DocumentMenuId = $("#DocumentMenuId").val();
    let DnForVarncQty = $("#hdn_DnForVarianceQty").val();

    let VarianceDataList = sessionStorage.getItem("VarianceDataList");
    if (VarianceDataList != null && VarianceDataList != "") {
        VarianceDataList = JSON.parse(VarianceDataList).filter(v => (v.item_id == ItmCode));
    }

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalPurchaseInvoice/VarienceQuantityDetails",
        data: { GRNNo: GRNNo, GRNDate: GRNDate, ItmCode: ItmCode, DocumentMenuId, VarianceDataList: JSON.stringify(VarianceDataList), Disabled: $("#DisableSubItem").val() },
        success: function (data) {
            debugger;
            $("#PopUp_VarianceQuantityDetail").html(data);
            Cmn_rowHighLight();
            $("#VQD_ItemName").val(ItmName);
            $("#VQD_ItemId").val(ItmCode);
            $("#VQD_Uom").val(Uom);
            VQD_Calculate();
        }
    })
}
function VQD_Calculate() {
    /* Created by Suraj Maurya on 27-03-2025 */
    let total_value = 0, total_tax = 0, total_net_amt = 0, total_dn_amt = 0;
    $("#Table_VarianceQuantityDetail tbody tr").each(function () {
        const vqdRow = $(this);
        let value = vqdRow.find("#vqd_value").text();
        let tax = vqdRow.find("#vqd_item_tax").text();
        let net_amt = vqdRow.find("#vqd_net_amount").text();
        let dn_amt = vqdRow.find("#vqd_dn_amount").text();//debit note amount
        total_value = (parseFloat(total_value) + parseFloat(CheckNullNumber(value))).toFixed(ValDecDigit);
        total_tax = (parseFloat(total_tax) + parseFloat(CheckNullNumber(tax))).toFixed(ValDecDigit);
        total_net_amt = (parseFloat(total_net_amt) + parseFloat(CheckNullNumber(net_amt))).toFixed(ValDecDigit);
        if (vqdRow.find("#vqd_Include").is(":checked")) {
            total_dn_amt = (parseFloat(total_dn_amt) + parseFloat(CheckNullNumber(dn_amt))).toFixed(ValDecDigit);
        } else {
            vqdRow.find("#vqd_dn_amount").text(parseFloat(0).toFixed(ValDecDigit));
        }
    });
    $("#Table_VarianceQuantityDetail tfoot tr").each(function () {
        const vqdRow = $(this);
        vqdRow.find("#vqd_total_value").text(total_value);
        vqdRow.find("#vqd_total_tax_amount").text(total_tax);
        vqdRow.find("#vqd_total_net_amount").text(total_net_amt);
        vqdRow.find("#vqd_total_dn_amount").text(total_dn_amt);
    });
}
function OnClickIncludeVQD(e) {
    /* Created by Suraj Maurya on 27-03-2025 */
    const row = $(e.target).closest("tr");
    if (row.find("#vqd_Include").is(":checked")) {
        let net_amt = row.find("#vqd_net_amount").text();
        row.find("#vqd_dn_amount").text(net_amt);
    } else {
        row.find("#vqd_dn_amount").text(parseFloat(0).toFixed(ValDecDigit));
    }
    VQD_Calculate();
}

function ShowVarianceTaxDetails(e, varianceType) {
    /* Created by Suraj Maurya on 25-03-2025 */
    const row = $(e.target).closest("tr");
    let item_name = $("#VQD_ItemName").val();
    let ass_val = row.find("#vqd_value").text();

    let ItmCode = $("#VQD_ItemId").val();
    let totalTaxAmt = 0;
    debugger
    $("#Tax_ItemName").val(item_name);
    $("#Tax_AssessableValue").val(ass_val);
    $("#Tax_Template,#Tax_Type,#Tax_Percentage,#Tax_Level,#Tax_ApplyOn").attr("disabled", true);
    $("#Icon_AddNewRecord,#TaxResetBtn,#TaxSaveTemplate,#ReplicateOnAllItemsBtn,#SaveAndExitBtn,#TaxExitAndDiscard").css("display", "none");

    let VarianceTaxDataList = sessionStorage.getItem("VarianceTaxDataList");
    if (VarianceTaxDataList != null && VarianceTaxDataList != "") {
        VarianceTaxDataList = JSON.parse(VarianceTaxDataList).filter(v => (v.item_id == ItmCode && v.var_type == varianceType));
        VarianceTaxDataList.map((item) => {
            totalTaxAmt = parseFloat(totalTaxAmt) + parseFloat(item.tax_val);
        })
        VarianceTaxDataList.map((item, index) => {
            let taxApplyOn = item.tax_apply_on == "I" ? "Immediate Level" : "Cummulative";
            TaxTableDataBind((index + 1), item.tax_name, item.tax_id, item.tax_rate.toString(), item.tax_level, taxApplyOn,
                item.tax_val, item.tax_apply_on, totalTaxAmt, "", "", item.tax_recov
            )
        });
    }

}
function onClick_VQD_SaveAndExit() {
    /* Created by Suraj Maurya on 25-03-2025 */
    let VarianceDataList = sessionStorage.getItem("VarianceDataList");
    if (VarianceDataList != null && VarianceDataList != "") {
        VarianceDataList = JSON.parse(VarianceDataList);
    }
    let item_id = $("#VQD_ItemId").val();
    let total_dn_amt = 0;
    $("#Table_VarianceQuantityDetail tbody tr").each(function () {//Reset 
        const vqdRow = $(this);
        let var_type = vqdRow.find("#vqd_var_type").text();
        let dn_amt = vqdRow.find("#vqd_dn_amount").text();
        let include = vqdRow.find("#vqd_Include").is(":checked") ? "Y" : "N";
        VarianceDataList = VarianceDataList.map(v =>
            (v.item_id == item_id && v.var_type == var_type) ? { ...v, dn_amt: dn_amt, include: include } : { ...v }
        );
        total_dn_amt = parseFloat(total_dn_amt) + parseFloat(dn_amt);
    });

    sessionStorage.setItem("VarianceDataList", JSON.stringify(VarianceDataList));
    $("#VQD_btnSaveAndExit").attr("data-dismiss", 'modal');

    $("#POInvItmDetailsTbl tbody tr #hfItemID[value='" + item_id + "']").closest('tr').each(function () {
        const row = $(this);
        row.find("#TxtDebitNoteValue").val(total_dn_amt.toFixed(ValDecDigit));
    });
    CalcVarianceDnAmount();
    GetAllGLID();
}
function CalcVarianceDnAmount() {
    let dn_amt = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl tbody tr").each(function () {
        const row = $(this);
        let item_dn_amt = row.find("#TxtDebitNoteValue").val();
        dn_amt = parseFloat(parseFloat(dn_amt) + parseFloat(CheckNullNumber(item_dn_amt))).toFixed(ValDecDigit);
    });
    $("#TxtVarDn_Amount").val(dn_amt);

}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
//Added by Nidhi on 01-09-2025
function SendEmail() {
    var docid = $("#DocumentMenuId").val();
    var supp_id = $("#SupplierName").val();
    Cmn_SendEmail(docid, supp_id, 'Supp');
}
function SendEmailAlert() {
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/LocalPurchaseInvoice/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var pdfAlertEmailFilePath = 'PI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/SavePdfDocToSendOnEmailAlert_Ext",
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
//End on  01-09-2025
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
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "TxtItemName", "FieldType": "input", "SrNo": "" }]);
}