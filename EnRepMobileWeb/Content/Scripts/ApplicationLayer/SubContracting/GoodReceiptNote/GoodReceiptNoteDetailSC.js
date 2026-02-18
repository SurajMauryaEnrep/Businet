/************************************************
Javascript Name:Good Receipt Note SC Detail
Created By:Hina Sharma
Created Date: 28-03-2023
Description: This Javascript use for the Good Receipt Note SC many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {

    RemoveSessionNew();
    debugger;
    BindDDlContractorList();
    GetAndViewDetailsOfBatchSerialOnDblClik();
    GetAndViewDetailsOfScrapItemBatchDtlOnDblClik();
    //var SuppName = $("#SupplierName").val();
    //$("#Hdn_SupplierName").val(SuppName);

    grn_no = $("#GRNNumber").val();
    $("#hdDoc_No").val(grn_no);
    forcostingDetail();
});
function forcostingDetail() {
    if ($("#hfStatus").val() == "PC") {
        $("#GRNIServictmDetailsTbl tbody tr").each(function () {
            let row = $(this);
            row.find("#TxtRate").change();
        });
        let CurrId = $("#CurrId").val();
        let conv_rate = $("#conv_rate").val();
        $("#Tbl_OC_Deatils tbody tr").each(function () {
            let row = $(this);
            let td_currId = row.find("#HdnOCCurrId").text();
            if (CurrId == td_currId) {
                row.find("#OCConv").text(parseFloat(CheckNullNumber(conv_rate)).toFixed(RateDecDigit));
                let OCConv = row.find("#OCConv").text();
                let OCAmount = row.find("#OCAmount").text();
                let OCValueInBase = parseFloat(CheckNullNumber(OCConv)) * parseFloat(CheckNullNumber(OCAmount));
                row.find("#OcAmtBs").text(OCValueInBase.toFixed(ValDecDigit));
                let OCTaxAmt = row.find("#OCTaxAmt").text();
                let totalValue = parseFloat(CheckNullNumber(OCValueInBase)) + parseFloat(CheckNullNumber(OCTaxAmt));
                row.find("#OCTotalTaxAmt").text(totalValue.toFixed(ValDecDigit));
            }
        });
        OnClickSaveAndExit_OC_Btn();
    }
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    var ValDigit = $("#ValDigit").text();
    var OcWithoutTax = $("#Total_OC_AmountInBs").text();
    $("#OCWithoutTax").val(parseFloat(OcWithoutTax).toFixed(ValDigit));
    $("#grnOtherCharges").val(TotalOCAmt);
    CalculateAmount();
};
function BindOtherChargeDeatils(val) {
    var DecDigit = $("#ValDigit").text();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
            TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
            TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
        });
    }
    $("#grnOtherCharges").val(TotalAMountWT);
}
function SetOtherChargeVal() {

}
function OnClickSaveAndExit_OC_Btn() {
    debugger;
    var NetOrderValueSpe = "#grnNetMRValue";
    var NetOrderValueInBase = "#grnNetMRValue";
    var JO_otherChargeId = '#Tbl_OtherChargeList';
    CMNOnClickSaveAndExit_OC_Btn(JO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
}
function OnChangeItemPrice(e) {
    var currentrow = $(e.target).closest('tr');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var ItmCode = currentrow.find("#TxtItemId").val();
    let ValDigit = $("#ValDigit").text();
    let RateDigit = $("#RateDigit").text();
    let conv_rate = $("#conv_rate").val();
    var TxtGrnQty = currentrow.find("#TxtGrnQty").val();
    var TxtRate = currentrow.find("#TxtRate").val();
    var Txt_item_tax_amt = currentrow.find("#Txt_item_tax_amt").val();
    var Txt_other_charge = currentrow.find("#Txt_other_charge").val();
    var TxtGRNValue = parseFloat(CheckNullNumber(TxtGrnQty)) * parseFloat(CheckNullNumber(TxtRate));
    var TxtNetValueInBase = parseFloat(TxtGRNValue) + parseFloat(CheckNullNumber(Txt_item_tax_amt)) + parseFloat(CheckNullNumber(Txt_other_charge));

    var AssAmount = currentrow.find("#TxtNetValue").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);

    currentrow.find("#TxtRate").val(parseFloat(CheckNullNumber(TxtRate)).toFixed(RateDigit));
    currentrow.find("#TxtGrnValue").val(parseFloat(TxtGRNValue).toFixed(ValDigit));
    currentrow.find("#TxtNetValue").val(parseFloat(TxtNetValueInBase / conv_rate).toFixed(ValDigit));
    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValue").val());
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
    }
    else {
        if (GstApplicable == "Y") {
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValue").val());
        }
    }
    if (GstApplicable == "Y") {
        if (currentrow.find("#ManualGST").is(":checked")) {
            CalculateTaxAmount_ItemWise(null, ItmCode, currentrow.find("#TxtGrnValue").val());
        }
    }
    CalculateAmount();
}

function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    let TaxNonRecovAmt = 0;
    let TaxRecovAmt = 0;
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
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
        $("#GRNIServictmDetailsTbl > tbody > tr #TxtItemId[value=" + ItmCode + "]").closest('tr').each(function () {
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
                        var TaxAmt = parseFloat(0).toFixed(DecDigit)
                        var GstApplicable = $("#Hdn_GstApplicable").text();
                        if (GstApplicable == "Y") {
                            if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                            }
                        }
                        var TaxItmCode = CRow.find("#TaxItmCode").text();
                        if (TaxItmCode == ItmCode) {
                            currentRow.find("#Txt_item_tax_amt").val(parseFloat(CheckNullNumber(TotalTaxAmtF)).toFixed(DecDigit));
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                                OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#Txt_other_charge").val())).toFixed(DecDigit);
                            }
                            AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValue").val()))).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            //currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
                            FinalNetOrderValueSpec = NetOrderValueBase / ConvRate;
                            currentRow.find("#TxtNetValue").val(parseFloat(FinalNetOrderValueSpec).toFixed(DecDigit));
                            currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(TaxRecovAmt).toFixed(DecDigit));
                            currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(TaxNonRecovAmt).toFixed(DecDigit));

                            //let GrnQty = currentRow.find("#TxtGrnQty").val();
                            //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                            //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                            //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(DecDigit));
                            //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
                        }
                    });
                }
                else {
                    debugger;
                    var TaxAmt = parseFloat(0).toFixed(DecDigit);
                    var GrossAmtOR = parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValue").val())).toFixed(DecDigit);
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                        OC_Amt_OR = parseFloat(CheckNullNumber(currentRow.find("#Txt_other_charge").val())).toFixed(DecDigit);
                    }
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                    currentRow.find("#TxtNetValue").val(FGrossAmtOR);
                    let FinalFGrossAmtOR = FGrossAmtOR / ConvRate;
                    currentRow.find("#TxtNetValue").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    //let GrnQty = currentRow.find("#TxtGrnQty").val();
                    //let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                    //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                    //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(DecDigit));
                    //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
                }
            }
        });
    }
}
function CalculateAmount() {
    var DecDigit = $("#ValDigit").text();
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var TaxNonRecov = parseFloat(0).toFixed(DecDigit);
    var TaxRecov = parseFloat(0).toFixed(DecDigit);

    $("#GRNIServictmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValue").val())) > 0) {

            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtGrnValue").val())).toFixed(DecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_non_rec_amt").val())) > 0) {

            TaxNonRecov = (parseFloat(TaxNonRecov) + parseFloat(currentRow.find("#Txt_item_tax_non_rec_amt").val())).toFixed(DecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_rec_amt").val())) > 0) {

            TaxRecov = (parseFloat(TaxRecov) + parseFloat(currentRow.find("#Txt_item_tax_rec_amt").val())).toFixed(DecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtNetValue").val())) > 0) {

            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(DecDigit);
        }
    });
    //var OtherCharge = $("#grnOtherCharges").val();
    var OtherCharge = $("#OCWithoutTax").val();
    NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(OtherCharge)).toFixed(DecDigit);

    $("#grnGrossSrvcValue").val(GrossValue);
    $("#grnTaxAmtNonRecoverable").val(TaxNonRecov);
    $("#grnTaxAmtRecoverable").val(TaxRecov);
    $("#grnNetMRValue").val(NetOrderValueBase);

    var ConsumptionValue = parseFloat(0).toFixed(DecDigit);
    $("#CostingConsumptionTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TotalCost = currentRow.find("#ConsumedTotalCost").val()
        ConsumptionValue = (parseFloat(ConsumptionValue) + parseFloat(TotalCost)).toFixed(DecDigit);
    });
    $("#grnConsumptionValue").val(ConsumptionValue);
    debugger;
    var NetLandedValue = parseFloat(0).toFixed(DecDigit);
    NetLandedValue = (parseFloat(GrossValue) + parseFloat(TaxNonRecov) + parseFloat(CheckNullNumber(OtherCharge)) + parseFloat(ConsumptionValue)).toFixed(DecDigit);
    $("#grnNetLandedValue").val(NetLandedValue);
};
function OnClickSaveAndExit(OnAddGRN) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;

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
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    }
    else {
        $("#GRNIServictmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#TxtItemId").val() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                let GrnQty = currentRow.find("#TxtGrnQty").val();
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(DecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = parseFloat(0).toFixed(DecDigit);
                    tax_recov_amt = parseFloat(0).toFixed(DecDigit);
                    tax_non_recov_amt = parseFloat(0).toFixed(DecDigit);
                }
                else {
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                }

                OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                var itmocamt = currentRow.find("#Txt_other_charge").val();
                if (itmocamt != null && itmocamt != "") {
                    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#TxtGrnValue").val()))).toFixed(DecDigit);
                //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(DecDigit));
                currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(DecDigit));
                currentRow.find("#TxtNetValue").val(parseFloat(NetOrderValueBase).toFixed(DecDigit));
                let FinalNetOrderValueSpec = NetOrderValueBase / ConvRate
                currentRow.find("#TxtNetValue").val(parseFloat(FinalNetOrderValueSpec).toFixed(DecDigit));
                //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(DecDigit));
                //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt = currentRow.find("#Txt_item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
}
function OnClickTaxCalculationBtn(e) {
    debugger;
    var POItemListName = "#GRNIServictmDetailsTbl";
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
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var ItmCode = currentrow.find("#TxtItemId").val();
    var AssAmount = currentrow.find("#TxtGrnValue").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
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
            Cmn_OnSaveAddressApplyGST(gst_number, "GRNIServictmDetailsTbl", "TxtItemId", "", "TxtGrnValue")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var ItmCode = currentrow.find("#TxtItemId").val();
    //var AssAmount = currentrow.find("#TxtGrnValue").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#Txt_item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        //let GrnQty = currentrow.find("#TxtGrnQty").val();
        //let OC_Amt = currentrow.find("#Txt_other_charge").val();
        //let LandedCostValue = parseFloat(CheckNullNumber(AssAmount)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(0));
        //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
        //currentrow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed($("#ValDigit").text()));
        //currentrow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
        DeleteItemTaxDetail(ItmCode);
        currentrow.find('#TaxExempted').prop("checked", false);
        currentrow.find('#BtnTxtCalculation').attr('disabled', false);
        CalculateAmount();
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "GRNIServictmDetailsTbl", "TxtItemId", "", "TxtGrnValue")
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
function QtyRateValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
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
function OnClickReplicateOnAllItems() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;

    var ArrNonRecov = [];
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
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
            var TableOnwhichTaxApply = "GRNIServictmDetailsTbl";
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
                        var TaxRecov = TaxCalculationList[i].TaxRecov;
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
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(DecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(DecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    }
    else {
        tax_recov_amt = 0;
        tax_non_recov_amt = 0;
        $("#GRNIServictmDetailsTbl >tbody >tr").each(function () {
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
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            if (ArrNonRecov.findIndex(v => v.ItemId == AItemID) > -1) {
                                let getIndex = ArrNonRecov.findIndex(v => v.ItemId == AItemID);
                                tax_recov_amt = ArrNonRecov[getIndex].tax_recov_amt;
                                tax_non_recov_amt = ArrNonRecov[getIndex].tax_non_recov_amt;
                            }
                            currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(DecDigit));
                            currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(DecDigit));
                            currentRow.find("#Txt_item_tax_amt").val(TotalTaxAmtF);
                            //var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            //if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                            //    OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(DecDigit);
                            //}
                            AssessableValue = (parseFloat(currentRow.find("#TxtGrnValueInBase").val())).toFixed(DecDigit);
                            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit);
                            currentRow.find("#TxtNetValue").val(NetOrderValueBase);
                            FinalNetOrderValueSpec = (NetOrderValueBase / ConvRate).toFixed(DecDigit);
                            currentRow.find("#TxtNetValue").val(FinalNetOrderValueSpec);
                            //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                            //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(GrnQty));
                            //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(DecDigit));
                            //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(LandedCostValuePerPc).toFixed(LddCostDecDigit));
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtGrnValueInBase").val()).toFixed(DecDigit);
                    currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(DecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                    currentRow.find("#TxtNetValue").val(FGrossAmt);
                    FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(DecDigit);
                    currentRow.find("#TxtNetValue").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtGrnValueInBase").val()).toFixed(DecDigit);
                currentRow.find("#Txt_item_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#Txt_other_charge").val() != null && currentRow.find("#Txt_other_charge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#Txt_other_charge").val()).toFixed(DecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                currentRow.find("#TxtNetValue").val(FGrossAmt);
                FinalGrossAmt = (FGrossAmt / ConvRate).toFixed(DecDigit);
                currentRow.find("#TxtNetValue").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
    }
}
function SaveCostingDetail() {
    debugger;
    if ($("#Cancelled").is(":checked")) {

        //$("#ddldeliverynoteno").attr("disabled", false);
        //$("#SupplierName").attr("disabled", false);
        return true;
    } else {
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("GRNIServictmDetailsTbl", "Txt_item_tax_amt", "TxtItemId", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
                return false;
            }
        }
        let FinalItemDetail = CostingDetailsItemList();
        let FinalConItemDetail = CostingDetailsConItemList();
        let FinalTaxDetail = InsertTaxDetails();
        let FinalOCTaxDetail = InsertOCTaxDetails();
        let FinalOCDetail = InsertOtherChargeDetails();

        $("#CostingDetailItmDt").val(JSON.stringify(FinalItemDetail));
        $("#CostingDetailConItmDt").val(JSON.stringify(FinalConItemDetail));
        $("#CostingDetailItmTaxDt").val(JSON.stringify(FinalTaxDetail));
        $("#CostingDetailItmOCTaxDt").val(JSON.stringify(FinalOCTaxDetail));
        $("#CostingDetailOcDt").val(JSON.stringify(FinalOCDetail));

        //$("#ddldeliverynoteno").attr("disabled", false);
        //$("#SupplierName").attr("disabled", false);
        return true;
    }
}
function CostingDetailsItemList() {
    var GrnItemList = [];
    $("#GRNIServictmDetailsTbl >tbody >tr").each(function (i, row) {
        let ItemID = "";
        let GrnQty = "";
        let ItmRate = "";
        let hsn_code = "";
        let GrossVal = "";
        let TaxAmt = "";
        let NetValSpec = "";
        let TaxExempted = "";
        let ManualGST = "";
        let currentRow = $(this);

        ItemID = currentRow.find("#TxtItemId").val();
        hsn_code = currentRow.find("#ItemHsnCode").val();
        GrnQty = currentRow.find("#TxtGrnQty").val();
        ItmRate = currentRow.find("#TxtRate").val();
        GrossVal = currentRow.find("#TxtGrnValue").val();
        if (parseFloat(CheckNullNumber(currentRow.find("#Txt_item_tax_amt").val())) > 0) {
            TaxAmt = currentRow.find("#Txt_item_tax_amt").val();
        }
        else {
            TaxAmt = "0";
        }
        NetValSpec = currentRow.find("#TxtNetValue").val();
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
            ItemID: ItemID, hsn_code: hsn_code, GrnQty: GrnQty, ItmRate: ItmRate, GrossVal: GrossVal
            , TaxAmt: TaxAmt, NetValSpec: NetValSpec, TaxExempted: TaxExempted, ManualGST: ManualGST
        });
    });
    return GrnItemList;
}
function CostingDetailsConItemList() {
    var GrnItemList = [];
    $("#CostingConsumptionTbl >tbody >tr").each(function (i, row) {
        let ItemID = "";
        let uom_id = "";
        let ConQty = "";
        let TotCost = "";
        let hsn_code = "";
        let currentRow = $(this);

        ItemID = currentRow.find("#hdItemId").val();
        uom_id = currentRow.find("#hdUOMId").val();
        hsn_code = currentRow.find("#conItemHsnCode").val();
        ConQty = currentRow.find("#IssuedQuantity").val();
        TotCost = currentRow.find("#ConsumedTotalCost").val();

        GrnItemList.push({
            ItemID: ItemID, uom_id: uom_id, hsn_code: hsn_code, ConQty: ConQty, TotCost: TotCost
        });
    });
    return GrnItemList;
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxList = [];
    $("#GRNIServictmDetailsTbl >tbody >tr").each(function () {
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
/***--------------------------------Costing Detail Section End-----------------------------------------***/
function BindDDlContractorList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
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
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_GRNSupplierName").val(SuppName)
        $("#Hdn_SupplierName").val(Supp_id)
        $("#SpanDNNoErrorMsg").css("display", "none");
        $("#DdlDeliveryNoteNum").css("border-color", "#ced4da");
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }

    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GoodReceiptNoteSC/GetDeliveryNoteList",
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
                        $("#DdlDeliveryNoteNum option").remove();
                        $("#DdlDeliveryNoteNum optgroup").remove();
                        $('#DdlDeliveryNoteNum').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].dn_dt}" value="${arr.Table[i].dn_no}">${arr.Table[i].dn_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#DdlDeliveryNoteNum').select2({
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

                        $("#TxtDeliveryNoteDate").val("");
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

function OnChangeDeliveryNoteNo(DeliveryNoteNo) {
    debugger;
    var DNNo = DeliveryNoteNo.value;
    if (DNNo == "---Select---") {
        $("#TxtDeliveryNoteDate").val("");
        $("#BillNumber").val("");
        $("#BillDate").val("");

        $('#SpanDNNoErrorMsg').text($("#valueReq").text());
        $("#SpanDNNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-DdlDeliveryNoteNum-container']").css("border-color", "Red");
    }
    else {
        //$(".plus_icon1").css('display', 'block');
        $("#HdnplusIconId").css('display', 'block');
        
        $("#SpanDNNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-DdlDeliveryNoteNum-container']").css("border-color", "#ced4da");
    }
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/GoodReceiptNoteSC/GetDeliveryNoteDetail",
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
                            $("#TxtDeliveryNoteDate").val(arr.Table[0].dn_dt);
                            $("#BillNumber").val(arr.Table[0].bill_no);
                            $("#BillDate").val(arr.Table[0].bill_date);
                            $("#GRNSCHdn_FinishProductid").val(arr.Table[0].fg_product_id);
                            $("#GRNSCFinishProduct").val(arr.Table[0].FItem);
                            $("#GRNSCHdn_FinishUomId").val(arr.Table[0].fg_uom_id);
                            $("#GRNSCFinishUom").val(arr.Table[0].FUom);
                        }
                        var JobOrdTyp = arr.Table1[0].JobOrdTyp;
                        if (JobOrdTyp == "D") {
                            var hdnJobOrdTyp = "Direct";
                            $("#GRNSC_hdnJobOrdTyp").val(hdnJobOrdTyp);
                          var jobtyp=  $("#GRNSC_hdnJobOrdTyp").val();
                            if (jobtyp == "Direct") {
                                $("#th_Matrialltyp").css("display", "none");
                            }
                        }
                        
                    }
                },
            });
    } catch (err) {
        console.log("OnChangeDeliveryNoteNo Error : " + err.message);
    }

}

function AddItemDetail() {
    debugger;
    if (CheckHeaderValidations() == false) {
        return false;
    }
    DisableHeaderDetail();
    var JobOrdTyp = $("#GRNSC_hdnJobOrdTyp").val();
    var DNNo = $("#DdlDeliveryNoteNum").val();
    var DNDate = $("#TxtDeliveryNoteDate").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GoodReceiptNoteSC/GetGRNSCDetails",
            data: { DNNo: DNNo, DNDate: DNDate },
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

                        $('#GRNSC_ItmDetailsTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table.length; i++) {
                            var subitmDisable = "";
                            if (arr.Table[i].sub_item != "Y") {
                                subitmDisable = "disabled";
                            }
                            $('#GRNSC_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class="sr_padding"><span id="SNo">${rowIdx}</span> </td>
<td style="display: none;">
<input  type="hidden" id="hiddenfiledItemID${rowIdx}" value='${arr.Table[i].item_id}' />
<input type="hidden" id="sub_item${rowIdx}" value="${arr.Table[i].sub_item}" />

</td>
 <td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-10 no-padding"><input id="IDItemName${rowIdx}" class="form-control" value='${arr.Table[i].item_name}' autocomplete="off" type="text" name="IDItemName" placeholder='${$("#ItemName").text()}'  onblur="this.placeholder='Item Name'" readonly="readonly">
</div><div class="col-sm-1 i_Icon"><button type="button" class="calculator" id="ItmInfoBtnIcon${rowIdx}" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button></div> <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div></td>
<td><input id="idUOM${rowIdx}" class="form-control" value='${arr.Table[i].uom_alias}' autocomplete="off" type="text" name="idUOM" placeholder='${$("#ItemUOM").text()}'  onblur="this.placeholder='UOM'" readonly="readonly"></td>
<td style="display: none;"><input  type="hidden" id="hfItemUOMID${rowIdx}" value="${arr.Table[i].uom_id}" /></td></td>
<td hidden="hidden">
<div class="lpo_form">
<input id="item_rate${rowIdx}" class="form-control date num_right" value="${parseFloat(arr.Table[i].price).toFixed(QtyDecDigit)}" onchange ="OnChangeItemRate(1,event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return AmountFloatRate(this,event);" type="text" name="Price"  placeholder="0000.00" readonly="readonly">
<span id="item_rateError${rowIdx}" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="ReceivedQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].recd_qty).toFixed(QtyDecDigit)}" name="ReceivedQuantity" placeholder="0000.00"  readonly="readonly">
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemAcceptQty" >
<button type="button" id="SubItemAcceptQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('AccQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="RejectedQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" name="RejectedQuantity" placeholder="0000.00"  readonly="readonly">
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemRejectQty" >
<button type="button" id="SubItemRejectQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RejQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="ReworkQuantity${rowIdx}" class="form-control num_right" autocomplete="" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}" name="ReworkQuantity" placeholder="0000.00"  readonly="readonly">
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemReworkQty" >
<button type="button" id="SubItemReworkQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('RewQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td style="display: none;"><input  type="hidden" id="hfRejWHID${rowIdx}" value="${arr.Table[i].rej_wh_id}" /></td></td>
<td style="display: none;"><input  type="hidden" id="hfRejQty${rowIdx}" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" /></td></td>
<td style="display: none;"><input  type="hidden" id="hfRewWHID${rowIdx}" value="${arr.Table[i].rework_wh_id}" /></td></td>
<td style="display: none;"><input  type="hidden" id="hfRewQty${rowIdx}" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}" /></td></td>
<td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id${rowIdx}" onchange ="OnChnageWarehouse(event)"><option value="0">---Select---</option></select>
<input type="hidden" id="Hdn_GRNSC_WhName"  style="display: none;" />
<span id="wh_Error${rowIdx}" class="error-message is-visible"></span></div></td>

<td><input id="LotNumber${rowIdx}" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}"   disabled></td>
<td class="center"><button type="button" id="BtnBatchDetail" class="calculator subItmImg" onclick="OnClickBatchDetailBtnGRNSC(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle=""  title="${$("#span_BatchDetail").text()}"></i></button></td>
<td class="center"><button type="button" id="BtnSerialDetail" class="calculator subItmImg"  onclick="OnClickSerialDetailBtnGRNSC(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button></td>
<td><textarea id="remarks${rowIdx}"  class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}"></textarea></td>
<td style="display: none;"><input type="hidden" id="hfbatchable${rowIdx}" value="${arr.Table[i].i_batch}" /></td>
<td style="display: none;"><input type="hidden" id="hfserialable${rowIdx}" value="${arr.Table[i].i_serial}" /></td>
<td style="display: none;"><input type="hidden" id="hfexpiralble${rowIdx}" value="${arr.Table[i].i_exp}" /></td>


</tr>`);
                                  BindWarehouseList(rowIdx);
                        }
                        //DisableHeaderDetail();
                        
                        EnableForEdit();
                    }
                    debugger;
                    if (arr.Table1.length > 0) {
                        var rowIdx = 0;
                        var Ibatch = "";
                        //$('#ConsumedItemDetailTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table1.length; i++) {
                            var subitmDisable = "";
                            if (arr.Table1[i].sub_item != "Y") {
                                subitmDisable = "disabled";
                            }
                            debugger;
                            if (arr.Table1[i].i_batch == 'Y') {
                                Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" onchange="OnChangeConsumeQty" class="calculator" id="BtnCnsmBatchDetail" data-toggle="modal" data-target="#ConsumeBatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i>
</button>
                                              <input type="hidden" id="hdi_batch" value="Y" style="display: none;" /></td>`;
                            }
                            else {
                                Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" onchange="OnChangeConsumeQty" disabled class="calculator subItmImg" id="BtnCnsmBatchDetail" data-toggle="modal" data-target="#ConsumeBatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i>
</button>
                                                <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                            }
                            debugger;
                            $('#ConsumedItemDetailTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                           <td id="SRNO" class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>
                                                            <td>
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value="${arr.Table1[i].item_name}" class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}" disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table1[i].item_id}" style="display: none;" />
                                                                    <input type="hidden" id="sub_item" value="${arr.Table1[i].sub_item}" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtnConsume(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value="${arr.Table1[i].UOMName}" id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table1[i].uom_id}" style="display: none;" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="IssuedQuantity"  value="${parseFloat(arr.Table1[i].issueQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequiredQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemGrnSCIssueQty" >
                                                                <button type="button" id="SubItemGrnSCIssueQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('GRNSCIssueQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                                                             </td>
                                                            
                                                             <td>
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="ConsumedQuantity"  value="${parseFloat(arr.Table1[i].Consumed_Qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="ConsumedQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemGrnSCConsumQty" >
                                                                <button type="button" id="SubItemGrnSCConsumQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('GRNSCConsumeQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                                                             </td>
                                                            
                                                            `+ Ibatch + `
                                                            <td>
                                                            <textarea id="Consume_Remarks" value=${arr.Table1[i].it_remarks}" class="form-control remarksmessage" name="remarks" maxlength="100" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"></textarea>
                                                            </td>
                                                           
                                        </tr>
                                `);

                        }
                    }
                    /*$('.panel-collapse collapse').sh*/
                    debugger;
                    if (arr.Table2.length > 0) {
                        var rowIdx = 0;
                        var Ibatchscrap = "";

                        //if (JobOrdTyp == "Direct") {
                        //    $("th_Matrialltyp").css("display", "none");
                        //}

                        //$('#ConsumedItemDetailTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table2.length; i++) {

                            var subitmDisable = "";
                            if (arr.Table2[i].sub_item != "Y") {
                                subitmDisable = "disabled";
                            }
                            debugger;
                            if (arr.Table2[i].i_batch == 'Y') {
                                Ibatchscrap = ` <td class="center"><button type="button" onclick="OnClickBatchDetailBtnScrap(event)" class="calculator" id="BtnBatchDetail" data-toggle="modal" data-target="#ScrapBatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                              <input type="hidden" id="hdi_batch_scrap" value="Y" style="display: none;" /></td>`;
                            }
                            else {
                                Ibatchscrap = ` <td class="center"><button type="button" onclick="OnClickBatchDetailBtnScrap(event)" onchange="OnChangeConsumeQty" disabled class="calculator subItmImg" id="BtnBatchDetail" data-toggle="modal" data-target="#ScrapBatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch_scrap" value="N" style="display: none;" /></td>`;
                            }
                            ++rowIdx
                            if (JobOrdTyp == "Direct") {
                                //$("th_Matrialltyp").css("display", "none");
                                MaterialTyp = `<td style="display:none">
                                                    <input value="${arr.Table2[i].ItemTypName}" id="ItemTypName${rowIdx}" class="form-control" autocomplete="off" type="text" name="ItemTypName" placeholder="${$("#ItemUOM").text()}" disabled>
                                                    <input type="hidden" id="hdItemtype${rowIdx}" value="${arr.Table2[i].ItemType.trim()}" style="display: none;" />
                                                </td>`
                            }
                            else {
                                MaterialTyp = `<td>
                                                    <input value="${arr.Table2[i].ItemTypName}" id="ItemTypName${rowIdx}" class="form-control" autocomplete="off" type="text" name="ItemTypName" placeholder="${$("#ItemUOM").text()}" disabled>
                                                    <input type="hidden" id="hdItemtype${rowIdx}" value="${arr.Table2[i].ItemType.trim()}" style="display: none;" />
                                                </td>`
                            }
                            debugger;
                            $('#ScrapItemDetailsTbl tbody').append(`
                                   <tr id="${rowIdx}">
                                                        
                                                        <td id="SRNO_scrap" class="sr_padding"><span id="SpanRowId_scrap"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled_scrap" value="${rowIdx}"</td>
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName_scrap${rowIdx}" value="${arr.Table2[i].item_name}" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder="${$("#ItemName").text()}" disabled>                                                                    
                                                                    <input type="hidden" id="hdItemId_scrap${rowIdx}" value="${arr.Table2[i].item_id}" style="display: none;" />
                                                                    <input type="hidden" id="sub_item${rowIdx}" value="${arr.Table2[i].sub_item}" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtnScrapItm(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value="${arr.Table2[i].UomName}" id="UOM_scrap${rowIdx}" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                                <input type="hidden" id="hdUOMId_scrap${rowIdx}" value="${arr.Table2[i].uom_id}" style="display: none;" />
                                                            </td>
                                                            <td>
                                                                <div class="lpo_form">
                                                                    <input id="item_rateScrap${rowIdx}" class="form-control date num_right"  onchange ="OnChangeItemRateScrap(1,event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return AmountFloatRate(this,event);" type="text" name="Price"  placeholder="0000.00">
                                                                    <span id="item_rateScrapError${rowIdx}" class="error-message is-visible"></span>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="ReceivedQuantity_scrap${rowIdx}" value="${parseFloat(arr.Table2[i].rec_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="ReturnQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <div class="col-sm-3 i_Icon no-padding" id="div_SubItemGrnSCRecvQty" >
                                                                <button type="button" id="SubItemGrnSCRecvQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('GRNSCRecvQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            `+ MaterialTyp +`
                                                            
                                                           <td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id_Scrap${rowIdx}" onchange ="OnChnageWarehouseScrap(event)"><option value="0">---Select---</option></select>
                                                            <span id="wh_Error_scrap${rowIdx}" class="error-message is-visible"></span></div></td>

                                                            <td><input id="LotNumber_Scrap${rowIdx}" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="LotNumber"  onblur="this.placeholder='LotNumber'" disabled></td>

                                                            `+ Ibatchscrap + `
                                                           
                                                            <td><textarea id="scrap_remarks${rowIdx}"  class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}"></textarea></td>
                                                            </tr>
                                `);
                            BindWarehouseListScrapByProduct(rowIdx);
                        }
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
                            var IssueQty = arr.Table3[y].Issue_Qty;
                            var ConsQty = arr.Table3[y].Consumed_Qty;
                            var RecevQty = arr.Table3[y].Qty;
                            var SubItemTyp = arr.Table3[y].SubItmTyp;

                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemAccQty" value='${AccQty}'></td>
                                    <td><input type="text" id="subItemRejQty" value='${RejQty}'></td>
                                    <td><input type="text" id="subItemRewQty" value='${RewQty}'></td>
                                    <td><input type="text" id="subItemIssueQty" value='${IssueQty}'></td>
                                    <td><input type="text" id="subItemConsQty" value='${ConsQty}'></td>
                                    <td><input type="text" id="subItemRecevQty" value='${RecevQty}'></td>
                                    <td><input type="text" id="subItemType" value='${SubItemTyp}'></td>
                                    </tr>`);
                        }

                    }
                    
                }
            },
        });
    } catch (err) {
    }
};
function OnChnageWarehouse(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNo").text();
    var whname = currentRow.find("#wh_id" + Sno + " option:selected").text();
    $("#Hdn_GRNSC_WhName").val(whname);
    /*currentRow.find("#wh_id" + Sno).val(whname);*/
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
function BindWarehouseList(id) {
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
                        $("#wh_id"+id).select2()
                        var FItmDetails = JSON.parse(sessionStorage.getItem("GRNItemDetailSession"));
                        if (FItmDetails != null) {
                            if (FItmDetails.length > 0) {
                                for (i = 0; i < FItmDetails.length; i++) {
                                    //var Wh_ID = FItmDetails[i].wh_id;
                                    var Wh_Name = FItmDetails[i].wh_name;
                                    var Item_ID = FItmDetails[i].item_id;

                                    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
                                        var currentRow = $(this);
                                        var SNo = currentRow.find("#SNo").text();

                                        var ItmID = $("#hiddenfiledItemID" + SNo).val();
                                        if (ItmID == Item_ID) {
                                            currentRow.find("#wh_id" + SNo).val(Wh_ID).prop('selected', true);
                                            //currentRow.find("#hdn_WHName" + SNo).val(Wh_Name);
                                        }
                                    });
                                    
                                }
                            }
                        }
                    }
                }
            },
        });
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNo").text();
    var ItmCode = "";
    var ItmName = "";
    ItmCode = clickedrow.find("#hiddenfiledItemID" + SNo).val();
    ItmName = clickedrow.find("#IDItemName" + SNo).val()
    ItemInfoBtnClick(ItmCode);
}
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
function DisableHeaderDetail() {
    $("#SupplierName").attr('disabled', true);
    $("#DdlDeliveryNoteNum").attr('disabled', true);
    /*$(".plus_icon1").css("display", "none");*/
    $("#HdnplusIconId").css("display", "none");
   
}
function EnableForEdit() {
    debugger;
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
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
//function OnChangeItemRate(RowID, e) {
//    debugger;
//    CalculationBaseRate(e);
//}
function OnChangeItemRate(RowID,e) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNo").text();
    var ItmRate = clickedrow.find("#item_rate" + Sno).val();
    
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    /* if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {*/
    if (ItmRate == "" || ItmRate == 0) {
        clickedrow.find("#item_rateError" + Sno).text($("#valueReq").text());
        clickedrow.find("#item_rateError" + Sno).css("display", "block");
        clickedrow.find("#item_rate" + Sno).css("border-color", "red");
        clickedrow.find("#item_rate" + Sno).val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateError" + Sno).css("display", "none");
        clickedrow.find("#item_rate" + Sno).css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate" + Sno).val("");
        ItmRate = 0;
        // return false;
    }
    

    clickedrow.find("#item_rate" + Sno).val(parseFloat(ItmRate).toFixed(RateDecDigit));
    

}
function AmountFloatRate(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
}



/*-----------GRN Final Product Batch Detail Start------------------*/
function OnClickBatchDetailBtnGRNSC(e) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();

    $("#SpanBatch_Qty").css("display", "none");
    $("#Batch_Quantity").css("border-color", "#ced4da");

    $("#SpanRejBatch_Qty").css("display", "none");
    $("#RejBatch_Quantity").css("border-color", "#ced4da");

    $("#SpanReworkBatch_Qty").css("display", "none");
    $("#ReworkBatch_Quantity").css("border-color", "#ced4da");

    $("#SpanBatch_No").css("display", "none");
    $("#txtBatch_Number").css("border-color", "#ced4da");

    $("#SpanBatch_ExDate").css("display", "none");
    $("#BatchExpiry_Date").css("border-color", "#ced4da");

    ResetBatchDetailValGRN();
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

    if (ReceQty == "NaN" || ReceQty == "" || ReceQty == "0" || RejQty == "NaN" || RejQty == "" || RejQty == "0" || RewQty == "NaN" || RewQty == "" || RewQty == "0") {
        $("#BtnBatchDetail").attr("data-target", "");
        return false;
    }
    else {
        $("#BtnBatchDetail").attr("data-target", "#BatchNumber");
    }

    $("#BatchItem_Name").val(ItemName);
    $("#batch_UOM").val(ItemUOM);
    $("#BatchReceived_Quantity").val(ReceQty);
    $("#BatchRej_Quantity").val(RejQty);
    $("#BatchRework_Quantity").val(RewQty);
    $("#BatchSaveAndExit_Btn").attr("data-dismiss", "");
    $("#hfItem_SNo").val(Sno);
    $("#hfItem_ID").val(ItemID);
    $("#hfexpiry_flag").val(ExpiryFlag);
    if (ExpiryFlag != "Y") {
        $("#spanexpiry_require").css("display", "none");
    }
    else {
        $("#spanexpiry_require").css("display", "");
    }

    var rowIdx = 0;
    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            $("#BatchDetail_Tbl >tbody >tr").remove();

            var EnableBatchdetail = $("#EnableBatch_detail").val();
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

                        debugger;
                        $('#BatchDetail_Tbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(FBatchDetails[i].RejBatchQty).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(FBatchDetails[i].ReworkBatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
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
                        $('#BatchDetail_Tbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(FBatchDetails[i].RejBatchQty).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(FBatchDetails[i].ReworkBatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
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
                $("#BatchDetail_Tbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#BatchDeleteIcon").css("display", "none");
                });
            }
        }
    }
    else {
        $("#Batch_Quantity").val();
        $("#RejBatch_Quantity").val();
        $("#ReworkBatch_Quantity").val();
    }
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SNo").text(SerialNo);
    });
};
function OnClickAddNewBatchDetailGRN() {
    debugger;
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var AcceptQty = $('#Batch_Quantity').val()
    var RejQty = $('#RejBatch_Quantity').val()
    var ReworkQty = $('#ReworkBatch_Quantity').val()
    var QtyDecDigit = $("#QtyDigit").text();
    if (AcceptQty == "") {
        AcceptQty = 0;
    }
    if (RejQty == "") {
        RejQty = 0;
    }
    if (ReworkQty == ""){
       ReworkQty = 0;
    }
    var TotalQty = parseFloat(AcceptQty) + parseFloat(RejQty) + parseFloat(ReworkQty);
    if (TotalQty == "0") {
        ValidInfo = "Y";
    }
    else {
        $("#SpanBatch_Qty").css("display", "none");
        $("#Batch_Quantity").css("border-color", "#ced4da");

        $("#SpanRejBatch_Qty").css("display", "none");
        $("#RejBatch_Quantity").css("border-color", "#ced4da");

        $("#SpanReworkBatch_Qty").css("display", "none");
        $("#ReworkBatch_Quantity").css("border-color", "#ced4da");

        if ($('#Batch_Quantity').val() == "") {
            var BQty = 0;
        }
        else {
            var BQty = parseFloat($('#Batch_Quantity').val()).toFixed(QtyDecDigit);
        }
        if ($('#RejBatch_Quantity').val() == "") {
            var RejBQty = 0;
        }
        else {
            var RejBQty = parseFloat($('#RejBatch_Quantity').val()).toFixed(QtyDecDigit);
        }
        if ($('#ReworkBatch_Quantity').val() == "") {
            var ReworkBQty = 0;
        }
        else {
            var ReworkBQty = parseFloat($('#ReworkBatch_Quantity').val()).toFixed(QtyDecDigit);
        }
        var ReceiQty = parseFloat($('#BatchReceived_Quantity').val()).toFixed(QtyDecDigit);
        var RejQty = parseFloat($('#BatchRej_Quantity').val()).toFixed(QtyDecDigit);
        var ReworkQty = parseFloat($('#BatchRework_Quantity').val()).toFixed(QtyDecDigit);
        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        var TotalRejQty = parseFloat(0).toFixed(QtyDecDigit);
        var TotalReworkQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetail_Tbl >tbody >tr").each(function () {
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
            $("#Batch_Quantity").css("border-color", "Red");
            $('#SpanBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatch_Qty").css("display", "block");
            return false;
        }
        else {
            if ($('#Batch_Quantity').val() == "") {
                $('#Batch_Quantity').val(parseFloat(0).toFixed(QtyDecDigit));
            }
            else {
                $('#Batch_Quantity').val(parseFloat($('#Batch_Quantity').val()).toFixed(QtyDecDigit));
            }
            //$('#Batch_Quantity').val(parseFloat($('#Batch_Quantity').val()).toFixed(QtyDecDigit));
        }
        if (parseFloat(RejQty) < parseFloat(TotalRejQty)) {
            $("#RejBatch_Quantity").css("border-color", "Red");
            $('#SpanRejBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanRejBatch_Qty").css("display", "block");
            return false;
        }
        else {
            if ($('#RejBatch_Quantity').val() == "") {
                $('#RejBatch_Quantity').val(parseFloat(0).toFixed(QtyDecDigit));
            }
            else {
                $('#RejBatch_Quantity').val(parseFloat($('#RejBatch_Quantity').val()).toFixed(QtyDecDigit));
            }
            //$('#RejBatch_Quantity').val(parseFloat($('#RejBatch_Quantity').val()).toFixed(QtyDecDigit));
        }
        if (parseFloat(ReworkQty) < parseFloat(TotalReworkQty)) {
            $("#ReworkBatch_Quantity").css("border-color", "Red");
            $('#SpanReworkBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanReworkBatch_Qty").css("display", "block");
            return false;
        }
        else {
            if ($('#ReworkBatch_Quantity').val() == "") {
                $('#ReworkBatch_Quantity').val(parseFloat(0).toFixed(QtyDecDigit));
            }
            else {
                $('#ReworkBatch_Quantity').val(parseFloat($('#ReworkBatch_Quantity').val()).toFixed(QtyDecDigit));
            }
            //$('#ReworkBatch_Quantity').val(parseFloat($('#ReworkBatch_Quantity').val()).toFixed(QtyDecDigit));
        }

    }
    if ($('#txtBatch_Number').val() == "") {
        ValidInfo = "Y";
        $("#txtBatch_Number").css("border-color", "Red");
        $('#SpanBatch_No').text($("#valueReq").text());
        $("#SpanBatch_No").css("display", "block");
    }
    else {
        var BatchNo = $('#txtBatch_Number').val().toUpperCase();
        $("#BatchDetail_Tbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblBtachNo = currentRow.find("#Batch_No").text().toUpperCase();
            if (tblBtachNo == BatchNo) {
                $('#SpanBatch_No').text($("#valueduplicate").text());
                $("#SpanBatch_No").css("display", "block");
                $("#txtBatch_Number").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
    }
    debugger;
    if ($('#BatchExpiry_Date').val() == "") {
        if ($("#hfexpiry_flag").val() == 'Y') {
            ValidInfo = "Y";
            $("#BatchExpiry_Date").css("border-color", "Red");
            $('#SpanBatch_ExDate').text($("#valueReq").text());
            $("#SpanBatch_ExDate").css("display", "block");
        }
    }
    var ExDate = $('#BatchExpiry_Date').val().split('-');
    if ($('#BatchExpiry_Date').val() != "") {
        if (ExDate[0].length > 4) {
            ValidInfo = "Y";
            $("#BatchExpiry_Date").css("border-color", "Red");
            $('#SpanBatch_ExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatch_ExDate").css("display", "block");
        }
        var currentdate = moment().format('YYYY-MM-DD');
        if ($('#BatchExpiry_Date').val() < currentdate) {
            ValidInfo = "Y";
            $("#BatchExpiry_Date").css("border-color", "Red");
            $('#SpanBatch_ExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatch_ExDate").css("display", "block");
        }
    }
    if (ValidInfo == "Y") {
        return false;
    }
    else {
        $("#SpanRejBatch_Qty").css("display", "none");
        $("#RejBatch_Quantity").css("border-color", "#ced4da");

        $("#SpanReworkBatch_Qty").css("display", "none");
        $("#ReworkBatch_Quantity").css("border-color", "#ced4da");
    }
    debugger;
    var accept_qty = $("#Batch_Quantity").val()
    if (accept_qty == "") {
        accept_qty = 0;
    }
    var reject_qty = $("#RejBatch_Quantity").val()
    if (reject_qty == "") {
        reject_qty = 0;
    }
    var rework_qty = $("#ReworkBatch_Quantity").val()
    if (rework_qty == "") {
        rework_qty = 0;
    }
    var date = $("#BatchExpiry_Date").val();
    if (date != null && date != "") {
        date = moment(date).format('DD-MM-YYYY');
    }
    else {
        date = "";
    }
    $('#BatchDetail_Tbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(accept_qty).toFixed(QtyDecDigit)}</td>
<td id="RejBatchQty" class="num_right">${parseFloat(reject_qty).toFixed(QtyDecDigit)}</td>
<td id="ReworkBatchQty" class="num_right">${parseFloat(rework_qty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${$("#txtBatch_Number").val()}</td>
<td id="BatchExDate" hidden="hidden">${$("#BatchExpiry_Date").val()}</td>
<td>${date}</td>
</tr>`);

    ResetBatchDetailValGRN();
    CalculateBatchQtyTblGRN();
    OnClickDeleteIconGRN();
}
function OnClickSaveAndCloseGRN() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ItemID = $('#hfItem_ID').val();
    var RowSNo = $("#hfItem_SNo").val();
    var UserID = $("#UserID").text();

    var ReceiBQty = parseFloat($("#BatchReceived_Quantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQty_Total").text()).toFixed(QtyDecDigit);

    var RejBQty = parseFloat($("#BatchRej_Quantity").val()).toFixed(QtyDecDigit);
    var TotalRejBQty = parseFloat($("#RejBatchQty_Total").text()).toFixed(QtyDecDigit);

    var ReworkBQty = parseFloat($("#BatchRework_Quantity").val()).toFixed(QtyDecDigit);
    var TotalReworkBQty = parseFloat($("#ReworkBatchQty_Total").text()).toFixed(QtyDecDigit);

    var TotalReceQty = parseFloat(ReceiBQty) + parseFloat(RejBQty) + parseFloat(ReworkBQty);
    var SumofAllTotal = parseFloat(TotalBQty) + parseFloat(TotalRejBQty) + parseFloat(TotalReworkBQty);

    if (parseFloat(TotalReceQty) == parseFloat(SumofAllTotal)) {

        $("#BatchSaveAndExit_Btn").attr("data-dismiss", "modal");
        /*$("#hiddenfiledItemID" + RowSNo).closest("tr").find("#BtnBatchDetail").css("border-color", "");*/
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
                $("#BatchDetail_Tbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var BatchQty = currentRow.find("#BatchQty").text();
                    var RejBatchQty = currentRow.find("#RejBatchQty").text();
                    var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    var BatchNo = currentRow.find("#BatchNo").text();
                    var BatchExDate = currentRow.find("#BatchExDate").text();
                    NewArr.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate })
                });
                sessionStorage.removeItem("BatchDetailSession");
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(NewArr));
            }
            else {
                var BatchDetailList = [];
                $("#BatchDetail_Tbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var BatchQty = currentRow.find("#BatchQty").text();
                    var RejBatchQty = currentRow.find("#RejBatchQty").text();
                    var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    var BatchNo = currentRow.find("#BatchNo").text();
                    var BatchExDate = currentRow.find("#BatchExDate").text();

                    BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate })
                });
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
            }
        }
        else {
            var BatchDetailList = [];
            $("#BatchDetail_Tbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var BatchQty = currentRow.find("#BatchQty").text();
                var RejBatchQty = currentRow.find("#RejBatchQty").text();
                var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                var BatchNo = currentRow.find("#BatchNo").text();
                var BatchExDate = currentRow.find("#BatchExDate").text();

                BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty, BatchNo: BatchNo, BatchExDate: BatchExDate })
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
                ValidateEyeColor(clickedrow, "BtnBatchDetail", "N");
                
            }
        });
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExit_Btn").attr("data-dismiss", "");
    }

}
function OnChangeBatchQtyGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    $("#Batch_Quantity").css("border-color", "#ced4da");
    if ($('#Batch_Quantity').val() != "0" && $('#Batch_Quantity').val() != "" && $('#Batch_Quantity').val() != "0") {
        $("#SpanBatch_Qty").css("display", "none");
        var BQty = parseFloat($('#Batch_Quantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceived_Quantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetail_Tbl >tbody >tr").each(function () {
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
            $("#Batch_Quantity").css("border-color", "Red");
            $('#SpanBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatch_Qty").css("display", "block");
            return false;
        }
        else {
            $('#Batch_Quantity').val(parseFloat($('#Batch_Quantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#Batch_Quantity').val("");
        $("#Batch_Quantity").css("border-color", "Red");
        $('#SpanBatch_Qty').text($("#valueReq").text());
        $("#SpanBatch_Qty").css("display", "block");
    }
}
function OnChangeRejBatchQtyGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    $("#RejBatch_Quantity").css("border-color", "#ced4da");
    if ($('#RejBatch_Quantity').val() != "0" && $('#RejBatch_Quantity').val() != "" && $('#RejBatch_Quantity').val() != "0") {
        $("#SpanRejBatch_Qty").css("display", "none");
        var BQty = parseFloat($('#RejBatch_Quantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchRej_Quantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetail_Tbl >tbody >tr").each(function () {
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
            $("#RejBatch_Quantity").css("border-color", "Red");
            $('#SpanRejBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanRejBatch_Qty").css("display", "block");
            return false;
        }
        else {
            $('#RejBatch_Quantity').val(parseFloat($('#RejBatch_Quantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#RejBatch_Quantity').val("");
        $("#RejBatch_Quantity").css("border-color", "Red");
        $('#SpanRejBatch_Qty').text($("#valueReq").text());
        $("#SpanRejBatch_Qty").css("display", "block");
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

    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var grnDt = $("#GRNDate").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: grnDt
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
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
function OnChangeReworkBatchQtyGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ReworkBatch_Quantity").css("border-color", "#ced4da");
    if ($('#ReworkBatch_Quantity').val() != "0" && $('#ReworkBatch_Quantity').val() != "" && $('#ReworkBatch_Quantity').val() != "0") {
        $("#SpanReworkBatch_Qty").css("display", "none");
        var BQty = parseFloat($('#ReworkBatch_Quantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchRework_Quantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);

        $("#BatchDetail_Tbl >tbody >tr").each(function () {
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
            $("#ReworkBatch_Quantity").css("border-color", "Red");
            $('#SpanReworkBatch_Qty').text($("#BatchQuantityExceeds").text());
            $("#SpanReworkBatch_Qty").css("display", "block");
            return false;
        }
        else {
            $('#ReworkBatch_Quantity').val(parseFloat($('#ReworkBatch_Quantity').val()).toFixed(QtyDecDigit));
        }
    }
    else {
        $('#ReworkBatch_Quantity').val("");
        $("#ReworkBatch_Quantity").css("border-color", "Red");
        $('#SpanReworkBatch_Qty').text($("#valueReq").text());
        $("#SpanReworkBatch_Qty").css("display", "block");
    }
}
function OnKeyPressBatchNoGRN() {
    $("#SpanBatch_No").css("display", "none");
    $("#txtBatch_Number").css("border-color", "#ced4da");
}
function OnChangeBatchExpiryDateGRN() {
    if ($('#BatchExpiry_Date').val() != "") {
        $("#SpanBatch_ExDate").css("display", "none");
        $("#BatchExpiry_Date").css("border-color", "#ced4da");
    }
    else {
        if ($("#hfexpiry_flag").val() == 'Y') {
            $("#BatchExpiry_Date").css("border-color", "Red");
            $('#SpanBatch_ExDate').text($("#valueReq").text());
            $("#SpanBatch_ExDate").css("display", "block");
        }
    }
}
function ResetBatchDetailValGRN() {
    debugger;
    $('#Batch_Quantity').val("");
    $('#RejBatch_Quantity').val("");
    $('#ReworkBatch_Quantity').val("");
    $('#txtBatch_Number').val("");
    $('#BatchExpiry_Date').val("");
}
function CalculateBatchQtyTblGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalRejQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalReworkQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#BatchDetail_Tbl >tbody >tr").each(function () {
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

    $('#BatchQty_Total').text(parseFloat(TotalReceiQty).toFixed(QtyDecDigit));
    $('#RejBatchQty_Total').text(parseFloat(TotalRejQty).toFixed(QtyDecDigit));
    $('#ReworkBatchQty_Total').text(parseFloat(TotalReworkQty).toFixed(QtyDecDigit));
}
function OnClickBatchResetBtnGRN() {
    var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValGRN();
    $('#BatchDetail_Tbl tbody tr').remove();
    $('#BatchQty_Total').text(parseFloat(0).toFixed(QtyDecDigit));
    $('#RejBatchQty_Total').text(parseFloat(0).toFixed(QtyDecDigit));
    $('#ReworkBatchQty_Total').text(parseFloat(0).toFixed(QtyDecDigit));

    $("#SpanBatch_Qty").css("display", "none");
    $("#Batch_Quantity").css("border-color", "#ced4da");
    $("#SpanRejBatch_Qty").css("display", "none");
    $("#RejBatch_Quantity").css("border-color", "#ced4da");
    $("#SpanReworkBatch_Qty").css("display", "none");
    $("#ReworkBatch_Quantity").css("border-color", "#ced4da");
    $("#SpanBatch_No").css("display", "none");
    $("#txtBatch_Number").css("border-color", "#ced4da");
    $("#SpanBatch_ExDate").css("display", "none");
    $("#BatchExpiry_Date").css("border-color", "#ced4da");
}
function OnClickDeleteIconGRN() {
    $('#BatchDetail_Tbl tbody').on('click', '.deleteIcon', function () {
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

//---------------Consumed Item Batch Detail Start------------------//
function OnClickIconBtnConsume(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#hdItemId").val();
    ItemInfoBtnClick(ItmCode)

}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var QtyDigit = $("#QtyDigit").text();
        var IssueQuantity = clickedrow.find("#ConsumedQuantity").val();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var MI_pedQty = clickedrow.find("#ConsumedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "";
        }
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#hdn_command").val();
        var TransType = $("#hdtranstype").val();
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            //clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            //clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            //clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/GoodReceiptNoteSC/getOrderResrvItemStockBatchWise",
                    data: {
                        ItemId: "",
                       // WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType
                    },
                    success: function (data) {
                        debugger;
                        $('#ConsumeItemStockBatchWise').html(data);

                    },
                });
        }
        else {
            debugger;
            var GRN_Status = $("#hfStatus").val();
            var DN_NO = $("#DdlDeliveryNoteNum").val();
            
            var GRN_NO = $("#GRNNumber").val();
            if (GRN_Status == "" || GRN_Status == null || GRN_Status == "D" && GRN_NO == "" || GRN_NO == null || GRN_Status == "D" && _mdlCommand == "Edit") {
                BindConsumeItemBatchDetail();
                debugger;
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var SelectedItemdetail = $("#HDConsumItmSelectedBatchwise").val();
                //var ddlId = "#wh_id" + Index;

                //var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#CompID").val();
                var BranchId = $("#BrId").val();
                var HdnMessage = $("#Hdn_Message").val();


                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/GoodReceiptNoteSC/getOrderResrvItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            //WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemdetail: SelectedItemdetail,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            GRN_Status: GRN_Status,
                            DN_NO: DN_NO,
                            HdnMessage:HdnMessage
                        },
                        success: function (data) {
                            debugger;
                            $('#ConsumeItemStockBatchWise').html(data);
                            var Index = clickedrow.find("#SNohiddenfiled").val();
                            var SelectedItemdetail = $("#HDConsumItmSelectedBatchwise").val();
                            //var ddlId = "#wh_id" + Index;
                            //var ItemId = clickedrow.find("#hdItemId").val();;
                            //var WarehouseId = clickedrow.find(ddlId).val();
                            var CompId = $("#HdCompId").val();
                            var BranchId = $("#HdBranchId").val();

                            $("#ConsumeItemNameBatchWise").val(ItemName);
                            $("#ConsumeUOMBatchWise").val(UOMName);
                            $("#ConsumeQuantityBatchWise").val(MI_pedQty);
                            $("#HDConsumeItemNameBatchWise").val(ItemId);
                            $("#HDConsumeUOMBatchWise").val(UOMId);                          
                            $("#ConsumeBatchwiseTotalIssuedQuantity").val("");

                            var ConsumeBatchwiseTotalIssuedQuantity = 0;
                            $("#ConsumeBatchWiseItemStockTbl tbody tr").each(function () {
                                debugger;
                                var currentrow = $(this);
                                var ReserveQuantity = IsNull(currentrow.find("#ReserveQuantity").val());
                                if (ReserveQuantity != undefined) {
                                    ConsumeBatchwiseTotalIssuedQuantity = parseFloat(ConsumeBatchwiseTotalIssuedQuantity) + parseFloat(ReserveQuantity);
                                    currentrow.find("#ConsumedQuantity").val(parseFloat(ReserveQuantity).toFixed(QtyDigit));
                                }
                                else {
                                    ConsumeBatchwiseTotalIssuedQuantity = parseFloat(ConsumeBatchwiseTotalIssuedQuantity) + parseFloat(0);
                                    currentrow.find("#ConsumedQuantity").val(parseFloat(0).toFixed(QtyDigit));
                                }
                            });
                            $("#ConsumeBatchwiseTotalIssuedQuantity").text(parseFloat(ConsumeBatchwiseTotalIssuedQuantity).toFixed(QtyDigit));
                            //try {
                            //    //For Auto fill Quantity on FIFO basis in the Batch Table.
                            //    //this will work only first time after save old value will come in the table
                            //    Cmn_AutoFillBatchQty("ConsumeBatchWiseItemStockTbl", MI_pedQty, "AvailableQuantity", "IssuedQuantity", "ConsumeBatchwiseTotalIssuedQuantity");
                            //} catch (err) {
                            //    console.log('Error : ' + err.message)
                            //}
                        },
                    });              
            }
            else {
                debugger;
                var grnd_No = $("#GRNNumber").val();
                var grnd_Date = $("#GRNDate").val();
                var DN_NO = $("#DdlDeliveryNoteNum").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/GoodReceiptNoteSC/getOrderResrvItemStockBatchWiseOnDblClk",
                        data: {
                            //IssueType: Mrs_IssueType,
                            GRNNo: grnd_No,
                            GRNDate: grnd_Date,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            DN_NO: DN_NO
                        },
                        success: function (data) {
                            debugger;
                            $('#ConsumeItemStockBatchWise').html(data);

                            $("#ConsumeItemNameBatchWise").val(ItemName);
                            $("#ConsumeUOMBatchWise").val(UOMName);
                            $("#ConsumeQuantityBatchWise").val(MI_pedQty);
                            $("#HDConsumeItemNameBatchWise").val(ItemId);
                            $("#HDConsumeUOMBatchWise").val(UOMId);
                            $("#ConsumeBatchwiseTotalIssuedQuantity").val("");

                            var ConsumeBatchwiseTotalIssuedQuantity = 0;
                            $("#ConsumeBatchWiseItemStockTbl tbody tr").each(function () {
                                debugger;
                                var currentrow = $(this);
                                var ReserveQuantity = IsNull(currentrow.find("#ReserveQuantity").val());
                                if (ReserveQuantity != undefined) {
                                    ConsumeBatchwiseTotalIssuedQuantity = parseFloat(ConsumeBatchwiseTotalIssuedQuantity) + parseFloat(ReserveQuantity);
                                    currentrow.find("#ConsumedQuantity").val(parseFloat(ReserveQuantity).toFixed(QtyDigit));
                                }
                                else {
                                    ConsumeBatchwiseTotalIssuedQuantity = parseFloat(ConsumeBatchwiseTotalIssuedQuantity) + parseFloat(0);
                                    currentrow.find("#ConsumedQuantity").val(parseFloat(0).toFixed(QtyDigit));
                                }
                            })
                            $("#ConsumeBatchwiseTotalIssuedQuantity").text(parseFloat(ConsumeBatchwiseTotalIssuedQuantity).toFixed(QtyDigit));
                        },

                    });               
            }            
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function BindConsumeItemBatchDetail() {
    debugger;
    var batchrowcount = $('#SaveConsumeItemBatchTbl tbody tr').length;
    if (batchrowcount > 0) {
        var ItemBatchList = new Array();
        $("#SaveConsumeItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            batchList.SrcDocNo = row.find('#grn_lineBatchSrcDocNo').val();
            batchList.SrcDocDate = row.find('#grn_lineBatchSrcDocDate').val();
            batchList.SrcDocDt = row.find('#grn_lineBatchSrcDocDt').val();
            batchList.Warehouse = row.find('#grn_lineBatchWarehouse').val();
            batchList.WarehouseID = row.find('#grn_lineBatchWarehouseID').val();
            batchList.LotNo = row.find('#grn_lineBatchLotNo').val();
            batchList.ItemId = row.find('#grn_lineBatchItemId').val();
            batchList.UOMId = row.find('#grn_lineBatchUOMId').val();
            batchList.BatchNo = row.find('#grn_lineBatchBatchNo').val();
            batchList.ExpiryDate = row.find('#grn_lineBatchExpiryDate').val();
            batchList.ReserveQty = row.find('#grn_lineBatchReserveQty').val();
            batchList.Consumed_Qty = row.find('#grn_lineBatchConsumeQty').val();
            
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDConsumItmSelectedBatchwise").val(str1);
    }
}
function OnClickItemBatchResetbtn() {
    $("#ConsumeBatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#ConsumedQuantity").val("");
    });
    $("#ConsumeBatchwiseTotalIssuedQuantity").text("");
}
function OnChangeConsumeQty(el, evt) {
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalConsumeQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
       //var CSRNO = clickedrow.find("#CSNohiddenfiled").val();
       // var ConsumeQuantity = clickedrow.find("#ConsumedQuantity" + CSRNO).val();
       // var ReserveQuantity = clickedrow.find("#ReserveQuantity" + CSRNO).val();
        
        var ConsumeQuantity = clickedrow.find("#ConsumedQuantity").val();
        var ReserveQuantity = clickedrow.find("#ReserveQuantity").val();
        if (AvoidDot(ConsumeQuantity) == false) {
            ConsumeQuantity = "0";
        }
        if (ConsumeQuantity != "" && ConsumeQuantity != null && ReserveQuantity != "" && ReserveQuantity != null) {
            if (parseFloat(ConsumeQuantity) > parseFloat(ReserveQuantity)) {
               
                clickedrow.find("#ConsumeQuantity_Error").text($("#ConsumeQtyIsExceedingTheReserveQty").text());
                //clickedrow.find("#ConsumeQuantity_Error").text($("#IssuedQtyisGreaterthanAvailableQty").text());
                clickedrow.find("#ConsumeQuantity_Error").css("display", "block");
                clickedrow.find("#ConsumedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(ConsumeQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#ConsumedQuantity").val(test);
            }
            else {
                clickedrow.find("#ConsumeQuantity_Error").css("display", "none");
                clickedrow.find("#ConsumedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(ConsumeQuantity).toFixed(QtyDecDigit);
                clickedrow.find("#ConsumedQuantity").val(test);
            }
        }

        $("#ConsumeBatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Consumeqty = row.find("#ConsumedQuantity").val();
            if (Consumeqty != "" && Consumeqty != null) {
                TotalConsumeQty = parseFloat(TotalConsumeQty) + parseFloat(Consumeqty);
            }
            debugger;
        });

        $("#ConsumeBatchwiseTotalIssuedQuantity").text(parseFloat(TotalConsumeQty).toFixed(QtyDecDigit));
        debugger;
        //var totalQty = $("#ConsumeBatchwiseTotalIssuedQuantity").text();
        //var ConsumQtyBtch = $("#ConsumeQuantityBatchWise").val();
        //if (totalQty == ConsumQtyBtch && ConsumeQuantity > ReserveQuantity) {
        //    clickedrow.find("#ConsumeQuantity_Error").text($("#ConsumeQtyIsExceedingTheReserveQty").text());
        //    //clickedrow.find("#ConsumeQuantity_Error").text($("#IssuedQtyisGreaterthanAvailableQty").text());
        //    clickedrow.find("#ConsumeQuantity_Error").css("display", "block");
        //    clickedrow.find("#ConsumedQuantity").css("border-color", "red");
        //    ErrorFlag = "Y";
        //}
        //else {
        //    clickedrow.find("#ConsumeQuantity_Error").css("display", "none");
        //    clickedrow.find("#ConsumedQuantity").css("border-color", "#ced4da");
        //    //var test = parseFloat(parseFloat(ConsumeQuantity)).toFixed(parseFloat(QtyDecDigit));
        //    //clickedrow.find("#ConsumedQuantity").val(test);
        //}
        //if (ErrorFlag == "Y") {
        //    return false;
        //}
        //else {
        //    return true;
        //}
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
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
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var ConsumeFlag = true;
    var ItemMI_Qty = $("#ConsumeQuantityBatchWise").val();
    var ItemTotalConsumedQuantity = $("#ConsumeBatchwiseTotalIssuedQuantity").text();
    $("#ConsumeBatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var ReservQuantity = row.find("#ReserveQuantity").val();
        var ConsumedQuantity = row.find("#ConsumedQuantity").val();
        if (parseFloat(ConsumedQuantity) > parseFloat(ReservQuantity)) {
            row.find("#ConsumeQuantity_Error").text($("#ConsumeQtyIsExceedingTheReserveQty").text());
            row.find("#ConsumeQuantity_Error").css("display", "block");
            /*row.find("#ConsumedQuantity").css("border-color", "red");*/
            ValidateEyeColor(row, "BtnCnsmBatchDetail", "Y");
            ConsumeFlag = false;
        }
    });

    if (ConsumeFlag) {
        
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalConsumedQuantity)) {
            swal("", $("#Batchqtydoesnotmatchwithconsumedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDConsumeItemNameBatchWise").val();
            $("#SaveConsumeItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#grn_lineBatchItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#ConsumeBatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemConsumeQuantity = row.find("#ConsumedQuantity").val();
                if (ItemConsumeQuantity != "" && ItemConsumeQuantity != null) {

                    var ItemUOMID = $("#HDConsumeUOMBatchWise").val();
                    var ItemId = $("#HDConsumeItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ReserveQty = row.find("#ReserveQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    var SrcDocNo = row.find("#SrcDocNo").val();
                    var SrcDocDate = row.find("#SrcDocDate").val();
                    var SrcDocDt = row.find("#SrcDocDt").val();
                    var Warehouse = row.find("#Warehouse").val();
                    var WarehouseID = row.find("#hdWhId").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    $('#SaveConsumeItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="grn_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="grn_lineBatchSrcDocNo" value="${SrcDocNo}" /></td>
                    <td><input type="text" id="grn_lineBatchSrcDocDate" value="${SrcDocDate}" /></td>
                    <td><input type="text" id="grn_lineBatchSrcDocDt" value="${SrcDocDt}" /></td>
                    <td><input type="text" id="grn_lineBatchWarehouse"  value="${Warehouse}" /></td>
                    <td><input type="text" id="grn_lineBatchWarehouseID"  value="${WarehouseID}" /></td>
                    <td><input type="text" id="grn_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="grn_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="grn_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="grn_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="grn_lineBatchReserveQty" value="${ReserveQty}" /></td>
                    <td><input type="text" id="grn_lineBatchConsumeQty" value="${ItemConsumeQuantity}" /></td>
                    
                </tr>`
                    );
                }
            });
            $("#ConsumeBatchNumber").modal('hide');
        }
        $("#ConsumedItemDetailTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                //clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "N");
            }
        });
    }
}
function onclickbtnItemBatchDiscardAndExit() {
    $("#ConsumeBatchNumber").modal('hide');
}
//---------------Consumed Item Batch Detail End------------------//
//--------------- GRN Final ProductSerial Deatils-----------------------//
function OnClickSerialDetailBtnGRN(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
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
                if (SNo != null && SNo != "") {
                    if (SRowID == Sno && SItemID == ItemID) {
                        $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} aria-hidden="true" id="SerialDeleteIcon" title="Delete"></i></td>
<td id="SerialID" >${rowIdx}</td>
<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
<td id="QtyType" >${FSerialDetails[i].QtyType}</td>
</tr>`);
                    }
                    else {
                        if (SItemID == ItemID) {
                            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} aria-hidden="true" id="SerialDeleteIcon" title="Delete"></i></td>
<td id="SerialID" >${rowIdx}</td>
<td id="SerialNo" >${FSerialDetails[i].SerialNo}</td>
<td id="QtyType" >${FSerialDetails[i].QtyType}</td>
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
    var QtyDecDigit = $("#QtyDigit").text();
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";

    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var RejSQty = parseFloat($("#SerialRejQuantity").val()).toFixed(QtyDecDigit);
    var ReworkSQty = parseFloat($("#SerialReworkQuantity").val()).toFixed(QtyDecDigit);
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
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="Delete"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
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
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="Delete"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
</tr>`);

            $('#SerialNo').val("");
            OnClickSerialDeleteIconGRN();
        }
    }
    if (QtyType === "Reworkable") {
        if (parseInt(ReworkLength) != parseInt(ReworkSQty)) {
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            $('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="Delete"></i></td>
<td id="SerialID" >${TblLen + 1}</td>
<td id="SerialNo" >${$("#SerialNo").val()}</td>
<td id="QtyType" >${QtyType}</td>
</tr>`);

            $('#SerialNo').val("");
            OnClickSerialDeleteIconGRN();
        }
    }

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
    $('#SerialDetailTbl tbody tr').remove();
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDiscardAndExitGRN() {
    OnClickSerialResetBtnGRN();
}
function OnClickSerialSaveAndCloseGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();

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

        $("#hiddenfiledItemID" + RowSNo).closest("tr").find("#BtnSerialDetail").css("border-color", "");
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
                    NewArr.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType })
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
                    SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType })
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
                SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, SerialNo: SerialNo, QtyType: QtyType })
            });
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
        }
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    /*   $("VehicleLoadInTonnage").val(parseFloat().toFixed(RateDecDigit))*/
    return true;
}

//--------------------End----------------------------//
/*---------------Insert Save Data Section start---------------*/
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var StatusCode = $("#hfStatus").val();
    if (StatusCode == "PC") {
        return SaveCostingDetail();
    }
    var GRNNum = $("#GRNNumber").val();
    if (GRNNum == null || GRNNum == "") {
        $("#hdtranstype").val("Save");
        $("#hdn_TransType").val($("#hdtranstype").val());
    }
    else {
        $("#hdtranstype").val("Update");
        $("#hdn_TransType").val($("#hdtranstype").val());
    }
    if (CheckHeaderValidations() == false) {
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
    if (CheckConsumeItemBatchValidation() == false) {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        return false;
    }
    if (CheckScrapItemValidations() == false) {
        return false;
    }
    if (CheckScrapItemBatchValidation() == false) {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        return false;
    }
    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
   
   //var TransType = "";
    //if (INSDTransType === 'Update') {
    //    TransType = 'Update';
    //}
    //else {
    //    TransType = 'Save';
    //}
    debugger;
    var FinalItemDetail = [];
    var FinalConsumeItemDetail = [];
    var FinalScrapItemDetail = [];

    FinalItemDetail = InsertGRNSCItemDetails();
    FinalConsumeItemDetail = InsertGRNSCConsumeItemDetails();
    FinalScrapItemDetail = InsertScrapItemDetails();
    debugger;
    var str = JSON.stringify(FinalItemDetail);
    $('#hdGRNSCItemDetailList').val(str);

    var str = JSON.stringify(FinalConsumeItemDetail);
    $('#hdGRNSCConsumeItemDetailList').val(str);

    var str = JSON.stringify(FinalScrapItemDetail);
    $('#hdScrapItemDetailList').val(str);

    $("#SupplierName").attr("disabled", false);
    $("#DdlDeliveryNoteNum").attr("disabled", false);
    debugger;
    var batchdetails = [];
    batchdetails = GetGRNSC_ItemBatchDetails();
    debugger;
    var str1 = JSON.stringify(batchdetails);
    $('#HdnBatchDetail').val(str1);
    debugger;
    var serialdetails = [];
    serialdetails = GetGRNSC_ItemSerialDetails();
    debugger;
    var str2 = JSON.stringify(serialdetails);
    $('#HdnSerialDetail').val(str2);

    var Scrapbatchdetails = [];
    Scrapbatchdetails = GetScrapItemBatchDetails();
    debugger;
    var str1 = JSON.stringify(Scrapbatchdetails);
    $('#HDSelectedScrapBatchwise').val(str1);
    debugger;

    /*--------------Data save for Consume Item Batch Detail Start--------------*/

    BindConsumeItemBatchDetail();
    /*--------------Data save for Consume Item Batch Detail End--------------*/
    debugger;
    ///*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

        ///*-----------Sub-item end-------------*/

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $("#hdn_Attatchment_details").val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    debugger;
    $("#Hdn_SupplierName").val($("#SupplierName").val());
    $("#Hdn_DNNum").val($("#DdlDeliveryNoteNum").val());
    var SuppName = $("#SupplierName option:selected").text();
    $("#Hdn_GRNSupplierName").val(SuppName)
    var whname = $("#Hdn_GRNSC_WhName").val();
    $("#Hdn_GRNSC_WhName").val(whname);

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
};
function CheckHeaderValidations() {
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

    if ($("#DdlDeliveryNoteNum").val() == null || $("#DdlDeliveryNoteNum").val() == "" || $("#DdlDeliveryNoteNum").val() == "0" || $("#DdlDeliveryNoteNum").val() == "---Select---") {
        $("[aria-labelledby='select2-DdlDeliveryNoteNum-container']").css("border-color", "Red");
        $("#DdlDeliveryNoteNum").css("border-color", "Red");
        $('#SpanDNNoErrorMsg').text($("#valueReq").text());
        $("#SpanDNNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanDNNoErrorMsg").css("display", "none");
        $("#DdlDeliveryNoteNum").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNo").text();

        if (currentRow.find("#IDItemName").val() == "" || currentRow.find("#IDItemName").val() == "0") {
            swal("", $("#noitemselectedmsg").text(), "warning");
            ErrorFlag = "Y";
            return false;
        }
        var ItmRate = currentRow.find("#item_rate" + Sno).val();
        debugger;
        if (ItmRate == "" || ItmRate == 0) {
            currentRow.find("#item_rateError" + Sno).text($("#valueReq").text());
            currentRow.find("#item_rateError" + Sno).css("display", "block");
            currentRow.find("#item_rate" + Sno).css("border-color", "red");
            currentRow.find("#item_rate" + Sno).val("");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_rateError" + Sno).css("display", "none");
            currentRow.find("#item_rate" + Sno).css("border-color", "#ced4da");
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
        
        return true;
    }
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
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
                //currentRow.find("#BtnBatchDetail").css("border-color", "red");
                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
                //return false;
            }
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {
                    //currentRow.find("#BtnBatchDetail").css("border-color", "red");
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                    //return false;
                }
            }
            else {
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
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
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
                currentRow.find("#BtnSerialDetail").css("border-color", "red");
                //return false;
            }
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {
                    ErrorFlag = "Y";
                    currentRow.find("#BtnSerialDetail").css("border-color", "red");
                    //return false;
                }
            }
            else {
                ErrorFlag = "Y";
                currentRow.find("#BtnSerialDetail").css("border-color", "red");
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

function InsertGRNSCItemDetails() {
    debugger;
    var GRNSCItemList = [];
    $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var  UomName= "";
        var AccptQty = "";
        var RejQty = "";
        var RewrkQty = "";
        var WhID = "";
        var RejWhID = "";
        var RewrkWhID = "";
        var item_rate = "";
        var item_gross_val = "";
        var item_tax_amt_recov = "";
        var item_tax_amt_nrecov = "";
        var item_oc_amt = "";
        var item_net_val = "";
        var item_landed_rate = "";
        var item_landed_val = "";
        var Remarks = "";
        var i_batch = "";
        var i_serial = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#SNo").text();
        ItemID = currentRow.find("#hiddenfiledItemID" + SNo).val();
        ItemName = currentRow.find("#IDItemName" + SNo).val();
        subitem = currentRow.find("#sub_item" + SNo).val();
        UOMID = currentRow.find("#hfItemUOMID" + SNo).val();
        UomName = currentRow.find("#idUOM" + SNo).val();
        AccptQty = currentRow.find("#ReceivedQuantity" + SNo).val();
        RejQty = currentRow.find("#RejectedQuantity" + SNo).val();
        RewrkQty = currentRow.find("#ReworkQuantity" + SNo).val();
        WhID = currentRow.find("#wh_id" + SNo).val();
        RejWhID = currentRow.find("#hfRejWHID" + SNo).val();
        RewrkWhID = currentRow.find("#hfRewWHID" + SNo).val();
        item_rate = currentRow.find("#item_rate" + SNo).val();
        item_gross_val = "0";
        item_tax_amt_recov = "0";
        item_tax_amt_nrecov = "0";
        item_oc_amt = "0";
        item_net_val = "0";
        item_landed_rate = "0";
        item_landed_val = "0";
        Remarks = currentRow.find("#remarks" + SNo).val();
        var status = $("#hfStatus").val();
        if (status == "") {
            i_batch = currentRow.find("#hfbatchable" + SNo).val();
        }
        else {
            i_batch = currentRow.find("#BtnBatchDetail").val();

        }
        if (status == "") {
            i_serial = currentRow.find("#hfserialable" + SNo).val();
        }
        else {
            i_serial = currentRow.find("#BtnSerialDetail").val();
        }

        GRNSCItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UomName: UomName, AccptQty: AccptQty, RejQty: RejQty, RewrkQty: RewrkQty, WhID: WhID, RejWhID: RejWhID, RewrkWhID: RewrkWhID, item_rate: item_rate, item_gross_val: item_gross_val, item_tax_amt_recov: item_tax_amt_recov, item_tax_amt_nrecov: item_tax_amt_nrecov, item_oc_amt: item_oc_amt, item_net_val: item_net_val, item_landed_rate: item_landed_rate, item_landed_val: item_landed_val, i_batch: i_batch, i_serial: i_serial, Remarks: Remarks });
    });

    return GRNSCItemList;
};
function GetGRNSC_ItemBatchDetails() {
    debugger;
    var GRNSCItemsBatchDetail = [];
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

                GRNSCItemsBatchDetail.push({ item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, reject_batch_qty: RejBatchQty, rework_batch_qty: ReworkBatchQty, exp_dt: BatchExDate });
            }
        }
    }

    return GRNSCItemsBatchDetail;
}
function GetGRNSC_ItemSerialDetails() {
    debugger;
    var GRNSCItemsSerialDetail = [];

    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails != null) {
        if (FSerialDetails.length > 0) {
            for (i = 0; i < FSerialDetails.length; i++) {
                var ItemID = FSerialDetails[i].ItemID;
                var SerialNo = FSerialDetails[i].SerialNo;
                var QtyType = FSerialDetails[i].QtyType;
                GRNSCItemsSerialDetail.push({ item_id: ItemID, serial_no: SerialNo, QtyType: QtyType });
            }
        }
    }

    return GRNSCItemsSerialDetail;
}
function GetAndViewDetailsOfBatchSerialOnDblClik() {
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
                $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
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
                BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: BItmCode, BatchQty: BBatchQty, RejBatchQty: BRejBatchQty, ReworkBatchQty: BReworkBatchQty, BatchNo: BBatchNo, BatchExDate: BExDate })

            }
            debugger;
            sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
        }
    }
    if ($("#HdnSerialDetail").val() != null && $("#HdnSerialDetail").val() != "") {
        debugger;
        var arr2 = $("#HdnSerialDetail").val();
        var arr = JSON.parse(arr2);
        $("#HdnSerialDetail").val("");
        debugger;
        if (arr.length > 0) {
            var SerialDetailList = [];

            for (var k = 0; k < arr.length; k++) {
                var RowSNo;
                $("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
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

                SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: SItmCode, SerialNo: SSerialNo, QtyType: QtyType })
            }
            debugger;
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
        }
    }
    debugger;
    //EnableForEdit();
}
//-------------------Data save for Consume Item Detail Start--------------------------------
function InsertGRNSCConsumeItemDetails() {
    debugger;
    var GRNSCConsumeItemList = [];
    $("#ConsumedItemDetailTbl >tbody >tr").each(function (i, row) {
        debugger;

        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UomName = "";
        var IssuedQty = "";
        var ConsumedQty = "";
        var i_batch = "";
        var Remarks = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#SNo").text();
        ItemID = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ItemName").val();
        subitem = currentRow.find("#sub_item").val();
        UOMID = currentRow.find("#hdUOMId").val();
        UomName = currentRow.find("#UOM").val();
        IssuedQty = currentRow.find("#IssuedQuantity").val();
        ConsumedQty = currentRow.find("#ConsumedQuantity").val();
        i_batch = currentRow.find("#hdi_batch").val();
        Remarks = currentRow.find("#Consume_Remarks").val();

        GRNSCConsumeItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UomName: UomName, IssuedQty: IssuedQty, ConsumedQty: ConsumedQty, i_batch: i_batch, Remarks: Remarks });
    });

    return GRNSCConsumeItemList;
};
function CheckConsumeItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ConsumedItemDetailTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#ConsumedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveConsumeItemBatchTbl >tbody >tr").length == 0) {
                
                /*clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "red");*/
                ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            if ($("#SaveConsumeItemBatchTbl >tbody >tr").length > 0)
            {
                $("#SaveConsumeItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchConsumeQty = currentRow.find('#grn_lineBatchConsumeQty').val();
                    var bchitemid = currentRow.find('#grn_lineBatchItemId').val();
                    //var bchuomid = currentRow.find('#grn_lineBatchUOMId').val();
                    if (ItemId == bchitemid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchConsumeQty);
                    }
                    if (bchConsumeQty == "" || bchConsumeQty == null) {
                        /*clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "red");*/
                        ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "Y");
                    }
                    else {
                        /*clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "#ced4da");*/
                        ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "N");
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "N");
                }
                else {
                    /*clickedrow.find("#BtnCnsmBatchDetail").css("border-color", "red");*/
                    ValidateEyeColor(clickedrow, "BtnCnsmBatchDetail", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
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
//-------------------Data save for Consume Item Detail End--------------------------------

//-------------------Data save for Scrap Item Detail Start--------------------------------
function CheckScrapItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#ScrapItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SpanRowId_scrap").text();

        if (currentRow.find("#ItemName_scrap").val() == "" || currentRow.find("#ItemName_scrap").val() == "0") {
            swal("", $("#noitemselectedmsg").text(), "warning");
            ErrorFlag = "Y";
            return false;
        }
        var ItmRate = currentRow.find("#item_rateScrap" + Sno).val();
        debugger;
        if (ItmRate == "" || ItmRate == 0) {
            currentRow.find("#item_rateScrapError" + Sno).text($("#valueReq").text());
            currentRow.find("#item_rateScrapError" + Sno).css("display", "block");
            currentRow.find("#item_rateScrap" + Sno).css("border-color", "red");
            currentRow.find("#item_rateScrap" + Sno).val("");
            errorFlag = "Y";
        }
        else {
            currentRow.find("#item_rateScrapError" + Sno).css("display", "none");
            currentRow.find("#item_rateScrap" + Sno).css("border-color", "#ced4da");
        }
        if (currentRow.find("#wh_id_Scrap" + Sno).val() == "0" || currentRow.find("#wh_id_Scrap" + Sno).val() == "" || currentRow.find("#wh_id_Scrap" + Sno).val() == null) {
            currentRow.find("#wh_Error_scrap" + Sno).text($("#valueReq").text());
            currentRow.find("#wh_Error_scrap" + Sno).css("display", "block");
            currentRow.find("#wh_id_Scrap" + Sno).css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error_scrap" + Sno).css("display", "none");
            currentRow.find("#wh_id_Scrap" + Sno).css("border", "1px solid #aaa");
        }
        if (currentRow.find("#ReceivedQuantity_scrap" + Sno).val() == "" || currentRow.find("#ReceivedQuantity_scrap" + Sno).val() == "0") {
            currentRow.find("#ReceivedQuantity_scrap" + Sno).css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ReceivedQuantity_scrap" + Sno).css("border-color", "#ced4da");
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {

        return true;
    }
}
function CheckScrapItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    $("#ScrapItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SpanRowId_scrap").text().trim();
        var Batchable = currentRow.find("#hdi_batch_scrap" + Sno).val();
        var Batchable1 = currentRow.find("#hdi_batch_scrap").val();
        var ItemID = currentRow.find("#hdItemId_scrap" + Sno).val();
        if (Batchable == "Y" || Batchable1=="Y") {
            var FBatchDetails = JSON.parse(sessionStorage.getItem("ScrapBatchDetailSession"));

            if (FBatchDetails != null) {
                var filteredValue = FBatchDetails.filter(function (item) {
                    return item.ItemID == ItemID;
                });
            }
            else {
                /*currentRow.find("#BtnBatchDetail").css("border-color", "red");*/
                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
                //return false;
            }
            if (filteredValue != null) {
                if (filteredValue.length > 0) {
                }
                else {
                    /*currentRow.find("#BtnBatchDetail").css("border-color", "red");*/
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                    //return false;
                }
            }
            else {
                /*currentRow.find("#BtnBatchDetail").css("border-color", "red");*/
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
function InsertScrapItemDetails() {
    debugger;
    var status = $("#hfStatus").val();
    var JobOrdTyp = $("#GRNSC_hdnJobOrdTyp").val();
    var ScrapItemList = [];
    $("#ScrapItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UomName = "";
        var ReceivQty = "";
        var MtrTyp = "";
        var MtrTypName = "";
        var WhID = "";
        var lot_no = "";
        var i_batch = ""; 
        var Remarks = "";
       
        var currentRow = $(this);
        var SNo = currentRow.find("#SRNO_scrap").text().trim();
        ItemID = currentRow.find("#hdItemId_scrap" + SNo).val().trim(); 
        ItemName = currentRow.find("#ItemName_scrap" + SNo).val().trim();
        subitem = currentRow.find("#sub_item" + SNo).val().trim();
        UOMID = currentRow.find("#hdUOMId_scrap" + SNo).val().trim();
        UomName = currentRow.find("#UOM_scrap" + SNo).val().trim();
        ItemRateScrap = currentRow.find("#item_rateScrap" + SNo).val().trim();
        ReceivQty = currentRow.find("#ReceivedQuantity_scrap" + SNo).val().trim();
        if (JobOrdTyp == "Direct") {
            MtrTyp = 0;
            MtrTypName = "";
            
        }
        else {
            MtrTyp = currentRow.find("#hdItemtype" + SNo).val();
            MtrTypName = currentRow.find("#ItemTypName" + SNo).val().trim();
        }
        //MtrTyp = currentRow.find("#hdItemtype" + SNo).val();
        //MtrTypName = currentRow.find("#ItemTypName" + SNo).val().trim();
        WhID = currentRow.find("#wh_id_Scrap" + SNo).val().trim(); 
        lot_no = currentRow.find("#LotNumber_Scrap" + SNo).val();
        if (status == "") {
            i_batch = currentRow.find("#hdi_batch_scrap").val();
        }
        else {
            i_batch = currentRow.find("#hdi_batch_scrap" + SNo).val();
        }
       
        Remarks = currentRow.find("#scrap_remarks" + SNo).val().trim();

        ScrapItemList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UomName: UomName, ItemRateScrap: ItemRateScrap, ReceivQty: ReceivQty, MtrTyp: MtrTyp, MtrTypName: MtrTypName, WhID: WhID, lot_no: lot_no, i_batch: i_batch, Remarks: Remarks });
    });

    return ScrapItemList;
};
function GetScrapItemBatchDetails() {
    debugger;
    var ScrapItemsBatchDetail = [];
    var FScrap_BatchDetails = JSON.parse(sessionStorage.getItem("ScrapBatchDetailSession"));
    if (FScrap_BatchDetails != null) {
        if (FScrap_BatchDetails.length > 0) {
            for (i = 0; i < FScrap_BatchDetails.length; i++) {
                var ItemID = FScrap_BatchDetails[i].ItemID;
                var BatchNo = FScrap_BatchDetails[i].BatchNo;
                var BatchQty = FScrap_BatchDetails[i].BatchQty;
                var BatchExDate = FScrap_BatchDetails[i].BatchExDate;

                ScrapItemsBatchDetail.push({ item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, exp_dt: BatchExDate });
            }
        }
    }

    return ScrapItemsBatchDetail;
}
function GetAndViewDetailsOfScrapItemBatchDtlOnDblClik() {
    debugger;
    if ($("#HDSelectedScrapBatchwise").val() != null && $("#HDSelectedScrapBatchwise").val() != "") {
        var arr2 = $("#HDSelectedScrapBatchwise").val();
        var arr = JSON.parse(arr2);
        $("#HDSelectedScrapBatchwise").val("");
        debugger;
        if (arr.length > 0) {
            var rowIdx = 0;
            var ScrapBatchDetailList = [];

            for (var j = 0; j < arr.length; j++) {
                var RowSNo;
                $("#ScrapItemDetailsTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var SNo = currentRow.find("#SRNO_scrap").text().trim();
                    var ItmCode = "";
                    ItmCode = currentRow.find('#hdItemId_scrap' + SNo).val();
                    if (ItmCode == arr[j].item_id) {
                        RowSNo = SNo;
                    }
                });

                var BItmCode = arr[j].item_id;
                var BBatchQty = arr[j].batch_qty;
                
                var BBatchNo = arr[j].batch_no;
                var BExDate = arr[j].exp_dt;
                if (BExDate == null) {
                    BExDate = "";
                }
                ScrapBatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: BItmCode, BatchQty: BBatchQty, BatchNo: BBatchNo, BatchExDate: BExDate })

            }
            debugger;
            sessionStorage.setItem("ScrapBatchDetailSession", JSON.stringify(ScrapBatchDetailList));
        }
    }
   
}
//-------------------Data save for Scrap Item Detail End--------------------------------

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
    sessionStorage.removeItem("ScrapBatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
}

/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var GRNStatus = "";
    //GRNStatus = $('#hfStatus').val().trim();
    //if (GRNStatus === "D" || GRNStatus === "F") {

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
    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    $.ajax({
        type: "POST",
        url: "/Common/Common/CheckFinancialYear",
        data: {
            compId: compId,
            brId: brId
        },
        success: function (data) {
            if (data == "Exist") { /*End to chk Financial year exist or not*/
                var GRNStatus = "";
                GRNStatus = $('#hfStatus').val().trim();
                if (GRNStatus === "D" || GRNStatus === "F") {

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
                swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");

            }
        }
    });
    /*End to chk Financial year exist or not*/
    return false;
}
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var GRNNo = "";
    var GRNDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";

    docid = $("#DocumentMenuId").val();
    GRNNo = $("#GRNNumber").val();
    GRNDate = $("#GRNDate").val();
    $("#hdDoc_No").val(GRNNo);
    WF_Status = $("#WF_Status1").val();
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && GRNNo != "" && GRNDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(GRNNo, GRNDate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS";
            var list = [{ GRNNo: GRNNo, GRNDate: GRNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/GoodReceiptNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ GRNNo: GRNNo, GRNDate: GRNDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/GoodReceiptNoteSC/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/DeliveryNoteSC/SIListApprove?SI_No=" + DNNo + "&SI_Date=" + DNDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && GRNNo != "" && GRNDate != "") {
            Cmn_InsertDocument_ForwardedDetail(GRNNo, GRNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ GRNNo: GRNNo, GRNDate: GRNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/GoodReceiptNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && GRNNo != "" && GRNDate != "") {
            Cmn_InsertDocument_ForwardedDetail(GRNNo, GRNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS";
            var list = [{ GRNNo: GRNNo, GRNDate: GRNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/GoodReceiptNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#GRNNumber").val();
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

/*-----------End Workflow--------------*/
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

//---------------Material Scrap Item Detail Start------------------//
function OnClickIconBtnScrapItm(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var ItmName = "";
    SNo = clickedrow.find("#SpanRowId_scrap").text().trim();
    ItmCode = clickedrow.find("#hdItemId_scrap" + SNo).val();
    ItemInfoBtnClick(ItmCode)

}
function BindWarehouseListScrapByProduct(id) {
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
                        $("#wh_id_Scrap" + id).html(s);
                        $("#wh_id_Scrap" + id).select2();
                        var FItmscrapDetails = JSON.parse(sessionStorage.getItem("GRNItemScrapByProductDetailSession"));
                        if (FItmscrapDetails != null) {
                            if (FItmscrapDetails.length > 0) {
                                for (i = 0; i < FItmscrapDetails.length; i++) {
                                    var Wh_ID = FItmscrapDetails[i].wh_id;
                                    var Item_ID = FItmscrapDetails[i].item_id;

                                    //$("#GRNSC_ItmDetailsTbl >tbody >tr").each(function () {
                                    //    var currentRow = $(this);
                                    //    var SNo = currentRow.find("#SNo").text();

                                    //    var ItmID = $("#hiddenfiledItemID" + SNo).val();
                                    //    if (ItmID == Item_ID) {
                                    //        currentRow.find("#wh_id" + SNo).val(Wh_ID).prop('selected', true);
                                    //    }
                                    //});
                                    $("#ScrapItemDetailsTbl >tbody >tr").each(function () {
                                        var currentRow = $(this);
                                        var SNo = currentRow.find("#SRNO_scrap").text();

                                        var ItmID = $("#hdItemId_scrap" + SNo).val();
                                        if (ItmID == Item_ID) {
                                            currentRow.find("#wh_id_Scrap" + SNo).val(Wh_ID).prop('selected', true);
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            },
        });
}
function OnChangeItemRateScrap(RowID, e) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SpanRowId_scrap").text().trim();
    var ItmRate = clickedrow.find("#item_rateScrap" + Sno).val();

    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    /* if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {*/
    if (ItmRate == "" || ItmRate == 0) {
        clickedrow.find("#item_rateScrapError" + Sno).text($("#valueReq").text());
        clickedrow.find("#item_rateScrapError" + Sno).css("display", "block");
        clickedrow.find("#item_rateScrap" + Sno).css("border-color", "red");
        clickedrow.find("#item_rateScrap" + Sno).val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateScrapError" + Sno).css("display", "none");
        clickedrow.find("#item_rateScrap" + Sno).css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rateScrap" + Sno).val("");
        ItmRate = 0;
        // return false;
    }


    clickedrow.find("#item_rateScrap" + Sno).val(parseFloat(ItmRate).toFixed(RateDecDigit));


}
function OnChnageWarehouseScrap(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SRNO_scrap").text();

    if (currentRow.find("#wh_id_Scrap" + Sno).val() == "0" || currentRow.find("#wh_id_Scrap" + Sno).val() == "" || currentRow.find("#wh_id_Scrap" + Sno).val() == null) {
        currentRow.find("#wh_Error_scrap" + Sno).text($("#valueReq").text());
        currentRow.find("#wh_Error_scrap" + Sno).css("display", "block");
        currentRow.find("#wh_id_Scrap" + Sno).css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#wh_Error_scrap" + Sno).css("display", "none");
        currentRow.find("#wh_id_Scrap" + Sno).css("border", "1px solid #aaa");
    }
}

function OnClickBatchDetailBtnScrap(e) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();

    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");

    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");

    ResetBatchDetailValScrap();

    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SpanRowId_scrap").text();
    if (Sno == "") {
        Sno = clickedrow.find("#SNohiddenfiled_scrap").val();
    }
    var UserID = $("#UserID").text();
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ExpiryFlag = "";
    var ReceiveQty = 0;
    
    ItemName = clickedrow.find("#ItemName_scrap" + Sno).val();
    ItemID = clickedrow.find("#hdItemId_scrap" + Sno).val();
    ItemUOM = clickedrow.find("#UOM_scrap" + Sno).val();
    ExpiryFlag = clickedrow.find("#hfexpiralble" + Sno).val();
    ReceiveQty = clickedrow.find("#ReceivedQuantity_scrap" + Sno).val();
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
    $("#BatchReceivedQuantity").val(ReceQty);
    
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
    var FBatchDetails = JSON.parse(sessionStorage.getItem("ScrapBatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            $("#BatchDetailTbl >tbody >tr").remove();
            var EnableBatchdetail = $("#EnableBatch_detail").val();
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

                        debugger;
                        $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
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
                        $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${FBatchDetails[i].BatchNo}</td>
<td id="BatchExDate" hidden="hidden">${FBatchDetails[i].BatchExDate}</td>
<td>${date}</td>
</tr>`);
                    }
                }

            }
            if (EnableBatchdetail == "Enable") {
                OnClickDeleteIconScrap();
            }

            CalculateBatchQtyTblScrap();

            var EDStatus = sessionStorage.getItem("ScrapEnableDisable");
            if (EDStatus == "Disabled") {
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#BatchDeleteIcon").css("display", "none");
                });
            }
        }
    }
}
function OnClickAddNewBatchDetail() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var AcceptQty = $('#BatchQuantity').val()
    var QtyDecDigit = $("#QtyDigit").text();

    var TotalQty = parseFloat(AcceptQty) ;
    if (TotalQty == "0") {

        ValidInfo = "Y";

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
   
    debugger;
    var accept_qty = $("#BatchQuantity").val()
    if (accept_qty == "") {
        accept_qty =0;
    }
    var date = $("#BatchExpiryDate").val();
    if (date != null && date != "") {
        date = moment(date).format('DD-MM-YYYY');
    }
    else {
        date = "";
    }
    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="Delete"></i></td>
<td id="BatchQty" class="num_right">${parseFloat(accept_qty).toFixed(QtyDecDigit)}</td>
<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
<td id="BatchExDate" hidden="hidden">${$("#BatchExpiryDate").val()}</td>
<td>${date}</td>
</tr>`);

    ResetBatchDetailValScrap();
    CalculateBatchQtyTblScrap();
    OnClickDeleteIconScrap();
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
        $("#hdItemId_scrap" + RowSNo).closest("tr").find("#BtnBatchDetail").css("border-color", "");
        ValidateEyeColor($("#hdItemId_scrap" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
        let NewArr = [];
        var FBatchDetails = JSON.parse(sessionStorage.getItem("ScrapBatchDetailSession"));
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
                    var currentRow = $(this);
                    var BatchQty = currentRow.find("#BatchQty").text();
                    //var RejBatchQty = currentRow.find("#RejBatchQty").text();
                    //var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    var BatchNo = currentRow.find("#BatchNo").text();
                    var BatchExDate = currentRow.find("#BatchExDate").text();
                    NewArr.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, /*RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty,*/ BatchNo: BatchNo, BatchExDate: BatchExDate })
                });
                sessionStorage.removeItem("ScrapBatchDetailSession");
                sessionStorage.setItem("ScrapBatchDetailSession", JSON.stringify(NewArr));
            }
            else {
                var BatchDetailList = [];
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var BatchQty = currentRow.find("#BatchQty").text();
                    //var RejBatchQty = currentRow.find("#RejBatchQty").text();
                    //var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                    var BatchNo = currentRow.find("#BatchNo").text();
                    var BatchExDate = currentRow.find("#BatchExDate").text();

                    BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, /*RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty,*/ BatchNo: BatchNo, BatchExDate: BatchExDate })
                });
                sessionStorage.setItem("ScrapBatchDetailSession", JSON.stringify(BatchDetailList));
            }
        }
        else {
            var BatchDetailList = [];
            $("#BatchDetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var BatchQty = currentRow.find("#BatchQty").text();
                //var RejBatchQty = currentRow.find("#RejBatchQty").text();
                //var ReworkBatchQty = currentRow.find("#ReworkBatchQty").text();
                var BatchNo = currentRow.find("#BatchNo").text();
                var BatchExDate = currentRow.find("#BatchExDate").text();

                BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: ItemID, BatchQty: BatchQty, /*RejBatchQty: RejBatchQty, ReworkBatchQty: ReworkBatchQty,*/ BatchNo: BatchNo, BatchExDate: BatchExDate })
            });
            sessionStorage.setItem("ScrapBatchDetailSession", JSON.stringify(BatchDetailList));
        }

        $("#ScrapItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var Sno = clickedrow.find("#SRNO_scrap").text().trim();
            var scrap_ItemID = clickedrow.find("#hdItemId_scrap" + Sno).val();

            if (scrap_ItemID == ItemID) {
                /*clickedrow.find("#BtnBatchDetail" + Sno).css("border-color", "#007bff");*/
                ValidateEyeColor(clickedrow, "#BtnBatchDetail" + Sno, "Y");
            }
        });
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    }

}
function OnChangeBatchQty() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
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
function OnKeyPressBatchNo() {
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
function ResetBatchDetailValScrap() {
    debugger;
    $('#BatchQuantity').val("");
    //$('#RejBatchQuantity').val("0");
    //$('#ReworkBatchQuantity').val("0");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
}
function CalculateBatchQtyTblScrap() {
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
function OnClickBatchResetBtnScrap() {
    var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValScrap();
    $('#BatchDetailTbl tbody tr').remove();
    $('#BatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));
    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
}
function OnClickDeleteIconScrap() {
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
        CalculateBatchQtyTblScrap();

    });
}
function OnClickDiscardAndExit() {
    OnClickBatchResetBtn();
}

/***--------------------------------Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    debugger;

    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNo").text();
    var scraphfsno = clickdRow.find("#SNohiddenfiled_scrap").val();
    var DNNo = $("#DdlDeliveryNoteNum option:selected").text();
    if (flag == "AccQuantity" || flag == "RejQuantity" || flag == "RewQuantity") {
       
        var ProductNm = clickdRow.find("#IDItemName" + hfsno).val();
        var ProductId = clickdRow.find("#hiddenfiledItemID" + hfsno).val();
        var UOM = clickdRow.find("#idUOM" + hfsno).val();
        var subitmTyp = "Itm";
    }
    if (flag == "GRNSCRecvQty") {
        
        var ProductNm = clickdRow.find("#ItemName_scrap" + scraphfsno).val();
        var ProductId = clickdRow.find("#hdItemId_scrap" + scraphfsno).val();
        var UOM = clickdRow.find("#UOM_scrap" + scraphfsno).val();
        var subitmTyp = "Scrap";
    }
    if (flag == "GRNSCIssueQty" || flag == "GRNSCConsumeQty") {

        var ProductNm = clickdRow.find("#ItemName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();
        var subitmTyp = "Consume";
    }
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
    else if (flag == "GRNSCRecvQty") {
        Sub_Quantity = clickdRow.find("#ReceivedQuantity_scrap" + scraphfsno).val();
    }
    else if (flag == "GRNSCIssueQty") {
        Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
    }
    else  {
        Sub_Quantity = clickdRow.find("#ConsumedQuantity").val();
    }

    var NewArr = new Array();
    //$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
    var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ProductId + "]").closest('tr').find("#subItemType[value='" + subitmTyp + "']").closest('tr');
    rows.each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.sub_item_name = row.find('#subItemName').val();
        List.Accqty = row.find('#subItemAccQty').val();
        List.Rejqty = row.find('#subItemRejQty').val();
        List.Rewqty = row.find('#subItemRewQty').val();
        List.Issue_Qty = row.find('#subItemIssueQty').val();
        List.Consumed_Qty = row.find('#subItemConsQty').val();
        List.Qty = row.find('#subItemRecevQty').val();
        List.SubItmTyp = row.find('#subItemType').val();
        NewArr.push(List);
    });

    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/GoodReceiptNoteSC/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            DNNo: DNNo,
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
function approveonclick() { /**Added this Condition by Nitesh 15-01-2024 for Disable Approve btn after one Click**/
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

function OnKeyDownBatchNoGRN(e) {   
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeGRNBatchNo(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    e.target.value = data;
}
