$(document).ready(function () {
    $("#CIListSupplier").select2();
    $("#SupplierName").select2();
    $("#CurrencyInBase").val("Y");
    //debugger;
    if ($("#SupplierName").val() != "0") {
        if ($("#ForDisableOCDlt").val() != "Disable") {
            $("#DivBtnAddItem").css("display", "block")
        }
        else {
            $("#DivBtnAddItem").css("display", "none")
        }
    }
    else {
        $("#DivBtnAddItem").css("display", "none")
    }  
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#InvoiceNumber").val() == "" || $("#InvoiceNumber").val() == null) {
        $("#Inv_date").val(CurrentDate);
    }
    InvNo = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvNo);
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        //debugger;
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#FilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var _CI_No = clickedrow.children("#InvoiceNo").text();
        var _CI_Date = clickedrow.children("#InvDate").text();
        if (_CI_No != null && _CI_Date != "") {
            window.location.href = "/ApplicationLayer/ConsumableInvoice/EditConsumableInvoice/?Inv_no=" + _CI_No + "&Inv_dt=" + _CI_Date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
        }

    });
    $("#POInvItmDetailsTbl tbody tr").each(function () {
        //debugger
        row_id = $(this).find("#SNohiddenfiled").val();
        BindItemDdlPQ(row_id);
    })
    $("#datatable-buttons >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        var PI_No = clickedrow.children("#InvoiceNo").text();
        var PI_Date = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(PI_No); //OnClickSaveAndExit_OC_Btn
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(PI_No, PI_Date, Doc_id, Doc_Status);
    });
    $('#POInvItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        //////debugger;
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
        ////debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#TxtItemName_" + SNo).val();
        CalculateAmount();
        //var TOCAmount = parseFloat($("#TxtOtherCharges").val());//Commented by Suraj Maurya on 06-01-2025
        var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmount = 0;
        }        
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetPO_ItemTaxDetail();
        BindOtherChargeDeatils();
        //GetAllGLID()
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
            AutoTdsApply(SuppId, GrossAmt).then(() => {
            });
        var RateDigit = $("#RateDigit").text();
        if ($("#POInvItmDetailsTbl >tbody >tr").length == 0) {
            var src_type = $('#src_type').val();
            if (src_type == "O") {
                $("#src_doc_number").attr('disabled', false);
                $("#ddlRequiredArea").attr('disabled', false);
                $("#plusbtn_div").css('display', 'block');
            }
            $("#Hdn_OC_TaxCalculatorTbl tbody tr").remove();
            $("#NetOrderValueInBase").val(parseFloat(0).toFixed(RateDigit));
            $("#TxtOtherCharges").val(parseFloat(0).toFixed(RateDigit));
            $("#Tbl_OtherChargeList tbody tr").remove();
            $("#Tbl_OtherChargeList tfoot tr").remove();
            $("#ht_Tbl_OC_Deatils >tbody >tr").remove();
            $("#Tbl_OC_Deatils >tbody >tr").remove();
            $("#Hdn_OCTemp_TaxCalculatorTbl >tbody >tr").remove();
        }
    });
    BindDDLAccountList();
    ResetWF_Level();
    OnChangeSourType();
    var pm_flagval = $("#pm_flagval").val();
    if (pm_flagval == "P") {
        $('#p_round').prop("checked", true);
    }
    if (pm_flagval == "M") {
        $('#m_round').prop("checked", true);
    }
    if ($("#chk_roundoff").is(":checked")) {
        $("#div_pchkbox").show();
        $("#div_mchkbox").show();
        $("#pm_flagval").val("");
        //$("#p_round").prop('checked', false);
        //$("#m_round").prop('checked', false);
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        //$("#p_round").prop('checked', false);
        //$("#m_round").prop('checked', false);
    }
    CancelledRemarks("#Cancelled", "Disabled");
});
function BindOtherChargeDeatils(val) {
    ////debugger;
    var DecDigit = $("#ValDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
    $("#TxtOtherCharges").val(parseFloat(0).toFixed(DecDigit));

    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            ////debugger;  
            var td = "";
            if (DocumentMenuId == "105101152") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td >${currentRow.find("#td_OCSuppName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
            if (DocumentMenuId == "105101152") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
            }

        });
        if (val == "") {
            //debugger;
            //GetAllGLID();
            let SuppId = $("#SupplierName").val();
            let GrossAmt = $("#TxtGrossValue").val();
            AutoTdsApply(SuppId, GrossAmt).then(() => {
            });
        }
    }
    //$("#SI_OtherChargeTotal").text(TotalAMount);
    $("#TxtOtherCharges").val(TotalAMountWT);
    if (DocumentMenuId == "105101152") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
        $("#_OtherChargeTotal").text(TotalAMount);
    }

}
function SaveBtnClick() {
    //debugger;
    return InsertCIDetails();
}
function InsertCIDetails() {
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        if ($("#duplicateBillNo").val() == "Y") {
            $("#Bill_No").css("border-color", "Red");
            $('#billNoErrorMsg').text($("#valueduplicate").text());
            $("#billNoErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#billNoErrorMsg").css("display", "none");
            $("#Bill_No").css("border-color", "#ced4da");
        }
        if (CheckCIValidations() == false) {
            return false;
        }
        if (CheckCI_ItemValidations() == false) {
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
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("POInvItmDetailsTbl", "item_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName_") == false) {
                return false;
            }
        }
        if (CheckPI_VoucherValidations() == false) {
            return false;
        }

        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfPIStatus") == false) {
            return false;
        }
        var FinalCI_ItemDetail = [];
        var FinalCI_TaxDetail = [];
        var FinalCI_OCDetail = [];
        var vou_Detail = [];
        var OC_TaxDetail = [];
        var FinalCostCntrDetails = [];

        FinalCI_ItemDetail = InsertCI_ItemDetails();
        FinalCI_TaxDetail = InsertCI_TaxDetails();
        FinalCI_OCDetail = InsertCI_OtherChargeDetails();
        vou_Detail = InsertCI_VoucherDetails();
        OC_TaxDetail = OC_InsertTaxDetails();
        FinalCostCntrDetails = Cmn_InsertCCDetails();

        $("#hdnItemdetails").val(JSON.stringify(FinalCI_ItemDetail));
        $("#hdnItemTaxdetails").val(JSON.stringify(FinalCI_TaxDetail));
        $("#hdnItemOCdetails").val(JSON.stringify(FinalCI_OCDetail));
        var str_vou_Detail = JSON.stringify(vou_Detail);
        $('#hdItemvouDetail').val(str_vou_Detail);

        var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
        $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

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


        $("#RCMApplicable").prop("disabled", false);
        $("#SupplierName").attr("disabled", false);
        $("#Bill_No").attr("disabled", false);
        $("#Bill_Date").attr("disabled", false);
        $("#SupplierName").attr("disabled", false);

        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    catch (ex) {
        ToAddJsErrorLogs(ex)
        return false;
    }
}
function InsertTdsDetails() {
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
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
        })
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
function InsertCI_ItemDetails() {
    //debugger;
    var POItemList = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var ItemID = "";
        var UOMID = "";
        var InvQty = "";
        var ItmRate = "";
        var GrossVal = "";
        var TaxAmt = "";
        var OCAmt = "";
        var NetValBase = "";
        var TaxExempted = "";
        var ManualGST = "";
        var ClaimITC = "";
        var hsn_code = "";
        var item_acc_id = "";
        var Item_remarks = "";
        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();
        var sr_no = currentRow.find("#srno").text();
        ItemID = currentRow.find("#TxtItemName_" + SNo).val();
        UOMID = currentRow.find("#UOMID").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        if (currentRow.find("#ord_qty_spec").val() == null || currentRow.find("#ord_qty_spec").val() == "") {
            InvQty = "0";
        }
        else {
            InvQty = currentRow.find("#ord_qty_spec").val();
        }
        ItmRate = currentRow.find("#item_rate").val();
        GrossVal = currentRow.find("#item_gr_val").val();
        if (currentRow.find("#item_tax_amt").val() === "" || currentRow.find("#item_tax_amt").val() === null) {
            TaxAmt = "0";
        }
        else {
            TaxAmt = currentRow.find("#item_tax_amt").val();
        }
        if (currentRow.find("#TxtOtherCharge").val() === "" || currentRow.find("#TxtOtherCharge").val() === null) {
            OCAmt = "0";
        }
        else {
            OCAmt = currentRow.find("#TxtOtherCharge").val();
        }
        NetValBase = currentRow.find("#item_net_val_bs").val();
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
        hsn_code = currentRow.find("#ItemHsnCode").val();
        Item_remarks = currentRow.find("#remarks").val();
        POItemList.push({
            ItemID: ItemID, UOMID: UOMID, InvQty: InvQty, ItmRate: ItmRate, GrossVal: GrossVal, TaxAmt: TaxAmt
            , OCAmt: OCAmt, NetValBase: NetValBase, TaxExempted: TaxExempted, ManualGST: ManualGST,
            ClaimITC: ClaimITC, hsn_code: hsn_code, item_acc_id: item_acc_id, sr_no: sr_no, Item_remarks: Item_remarks
        });
    });
    return POItemList;
};
function InsertCI_TaxDetails() {

    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var POTaxList = [];

    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#TxtItemName_" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        var tax_recov = Crow.find("#TaxRecov").text().trim();
                        var TaxAccId = currentRow.find("#TaxAccId").text().trim();
                        POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, tax_recov: tax_recov, TaxAccId: TaxAccId });
                    });
                }
            }
        }
    });
    return POTaxList;
    ////debugger;
    //if (FTaxDetails != null) {
    //    if (FTaxDetails > 0) {
    //        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
    //            //debugger;
    //            var Crow = $(this);
    //            var ItemID = Crow.find("#TaxItmCode").text().trim();
    //            var TaxID = Crow.find("#TaxNameID").text().trim();
    //            var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
    //            var TaxLevel = Crow.find("#TaxLevel").text().trim();
    //            var TaxValue = Crow.find("#TaxAmount").text().trim();
    //            var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
    //            POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn });
    //        });
    //    }
    //}
    //return POTaxList;
};
function InsertCI_OtherChargeDetails() {
    //debugger;
    var PI_OCList = []; 
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = "";
            var oc_val = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            oc_id = currentRow.find("#OCValue").text();
            oc_val = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            let curr_id = currentRow.find("#HdnOCCurrId").text();
            let supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text(); // Added by Suraj on 20-05-2024
            let bill_no = currentRow.find("#OCBillNo").text(); // Added by Suraj on 20-05-2024
            let bill_date = currentRow.find("#OCBillDt").text(); // Added by Suraj on 20-05-2024
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            let tds_amt = currentRow.find("#OC_TDSAmt").text();
            PI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt
                , OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs
                , supp_type: supp_type, supp_id: supp_id, curr_id: curr_id, bill_no: bill_no, bill_date: bill_date
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag, tds_amt: tds_amt
            });
        });
    }
    return PI_OCList;
};
function InsertCI_VoucherDetails() {
    //debugger;
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    //if ($("#OrderTypeL").is(":checked")) {
    //    InvType = "D";
    //}
    //if ($("#OrderTypeE").is(":checked")) {
    //    InvType = "E";
    //}
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var Gltype = "";
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#txthfAccID").val();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            Gltype = currentRow.find("#type").val();
            var dr_amt_bs = currentRow.find("#dramtInBase").text();
            var cr_amt_bs = currentRow.find("#cramtInBase").text();
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
function OC_InsertTaxDetails() {
    //debugger;
    var TaxDetails = [];
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var item_id = "";
        var tax_id = "";
        var tax_rate = "";
        var tax_level = "";
        var tax_val = "";
        var tax_apply_on = "";

        var currentRow = $(this);
        item_id = currentRow.find("#TaxItmCode").text();
        tax_id = currentRow.find("#TaxNameID").text();
        tax_rate = currentRow.find("#TaxPercentage").text();
        tax_level = currentRow.find("#TaxLevel").text();
        tax_val = currentRow.find("#TaxAmount").text();
        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        tax_acc_id = currentRow.find("#TaxAccId").text();

        TaxDetails.push({ ItemID: item_id, TaxID: tax_id, TaxRate: tax_rate, TaxLevel: tax_level, TaxValue: tax_val, TaxApplyOn: tax_apply_on, tax_acc_id: tax_acc_id });
    });
    return TaxDetails;
}
function CheckPI_VoucherValidations() {
    //debugger;

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 06-08-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 06-08-2024 to add new common gl validations*/

    //var ErrorFlag = "N";
    //var ValDigit = $("#ValDigit").text();
    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {
    //    //debugger;
    //    var currentRow = $(this);
    //    //var AccID = currentRow.find("#hfAccID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#Acc_name_' + rowid;
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //    }
    //    else {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //        return false;
    //    }

    //});
    ////debugger;
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}

    ////debugger;
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
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
function CheckCIValidations() {
    //debugger;
   var src_type= $("#src_type").val()
    $("#Hdn_src_type").val(src_type);



    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");

        if ($("#Address").val() === "" || $("#Address").val() === null) {
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
    if ($("#Bill_No").val() == "" || $("#Bill_No").val() == null) {
        $('#billNoErrorMsg').text($("#valueReq").text());
        $("#billNoErrorMsg").css("display", "block");
        $("#Bill_No").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#billNoErrorMsg").css("display", "none");
        $("#ValidUptoDate").css("border-color", "#ced4da");
    }
    
    if ($("#Bill_Date").val() == "" || $("#Bill_Date").val() == null) {
        $('#BillDateErrorMsg').text($("#valueReq").text());
        $("#BillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        //Code Added by Suraj Maurya on 22-10-2024
        var minDate = new Date('1900-01-01')._format('Y-m-d');
        var billDate = new Date($("#Bill_Date").val())._format('Y-m-d');

        if (billDate > minDate) {
            $("#BillDateErrorMsg").css("display", "none");
            $("#Bill_Date").css("border-color", "#ced4da");
       
        } else {
            $('#BillDateErrorMsg').text($("#JC_InvalidDate").text());
            $("#BillDateErrorMsg").css("display", "block");
            $("#Bill_Date").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        //Code Added by Suraj Maurya on 22-10-2024 End
    }
    if (src_type != "D")
    {
        var src_docno = $("#src_doc_number").val();
        if (src_docno == "" || src_docno == "0" || src_docno == "---Select---") {
            $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
            $("#src_doc_number").css("border-color", "red");
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
            $("#SpanSourceDocNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#src_doc_number").css("border-color", "#ced4da");
            $("#SpanSourceDocNoErrorMsg").css("display", "none");
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
            $("#Hdn_src_doc_number").val(src_docno);
        }
    }


    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}
function CheckCI_ItemValidations() {
    //debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#TxtItemName_" + Sno).val() == "0") {
                currentRow.find("#TxtItemNameError").text($("#valueReq").text());
                currentRow.find("#TxtItemNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-TxtItemName_" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtItemNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#ord_qty_spec").val() == "") {
                currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                currentRow.find("#ord_qty_specError").css("display", "block");
                currentRow.find("#ord_qty_spec").css("border-color", "red");
                if (ErrorFlag == "N") {
                    currentRow.find("#ord_qty_spec");//.focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qty_specError").css("display", "none");
                currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_rate").val() == "" || parseFloat(currentRow.find("#item_rate").val()) == 0) {
                currentRow.find("#item_rate_specError").text($("#valueReq").text());
                currentRow.find("#item_rate_specError").css("display", "block");
                currentRow.find("#item_rate").css("border-color", "red");
                if (ErrorFlag == "N") {
                    currentRow.find("#item_rate");//.focus();
                }
                
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rate_specError").css("display", "none");
                currentRow.find("#item_rate_specError").css("border-color", "#ced4da");
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
function OnClickSaveAndExit_OC_Btn() {
    //debugger;
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueSpe", "#NetOrderValueInBase")
    //debugger;
    //GetAllGLID();
}
function AfterDeleteResetPO_ItemTaxDetail() {
    //debugger;
    var PoItmDetailsTbl = "#POInvItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var POItemListName = "#TxtItemName_";
    CMNAfterDeleteReset_ItemTaxDetailModel(PoItmDetailsTbl, SNohiddenfiled, POItemListName);
}
function OnChangeSupplier(SuppID) {
    //debugger;
    var Supp_id = SuppID.value;
    var srctype = $("#src_type").val();
    if (Supp_id == "" || Supp_id == null || Supp_id == undefined || Supp_id == 0) {
        Supp_id = SuppID;
    }
    if (Supp_id == "") {
        $("#DivBtnAddItem").css("display", "none")
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
    }
    else {
        if (srctype == "D") {
            $("#DivBtnAddItem").css("display", "block")
        }
        if (srctype == "O") {
            BindSrcDocNumberOnBehalfSrcType();
            $("#DivBtnAddItem").css("display", "none")
        }
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#src_type").prop("disabled", true)

    }
   
   
    GetSuppAddress(Supp_id);
    OnChangeBillNo();
   
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
                    //debugger;
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
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            $("#supp_acc_id").val(arr.Table[0].supp_acc_id);
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            //$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                            $("#curr_id").val(arr.Table[0].curr_id);
                            //$("#conv_rate").prop("readonly", true);
                            if (arr.Table[0].gst_cat == "UR") {
                                $('#RCMApplicable').prop("checked", true);
                                $("#RCMApplicable").prop("disabled", true);
                            }
                            else {
                                $('#RCMApplicable').prop("checked", false);
                                $("#RCMApplicable").prop("disabled", false);
                            }
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            //CheckPOHraderValidations();
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
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function AddNewRow() {
    //debugger;
    var row_id = 0;
    var SrNo = $("#POInvItmDetailsTbl tbody tr").length;
    SrNo = parseInt(SrNo) + 1;
    var docid = $("#DocumentMenuId").val();
    var deletetext = $("#Span_Delete_Title").text();
    var UOM = $("#ItemUOM").text();//
    //var span_remarks = $("#span_remarks").text();
    $("#POInvItmDetailsTbl tbody tr").each(function () {
        //debugger
        row_id = $(this).find("#SNohiddenfiled").val();
    })
    row_id = parseInt(row_id) + 1;


    CreateNewRow(row_id, SrNo);
    BindItemDdlPQ(row_id);
}
function CreateNewRow(row_id, SrNo) {
    //debugger;
    var rowIdx = 0;
    var deletetext = $("#Span_Delete_Title").text();
    var UOM = $("#ItemUOM").text();//

    var srctype = $("#src_type").val();
    var DisableinvQty = "";
    if (srctype == "O") {
        DisableinvQty = "Disabled";
    }

    var ManualGst = "";
    var TaxExempted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
            ManualGst += `<td class="qt_to">
                              <div class="custom-control custom-switch sample_issue">
                                      <input type="checkbox" checked class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                                  <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                              </div>
                          </td>`;
        }
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'

    $("#POInvItmDetailsTbl tbody").append(`<tr id="R${++rowIdx}">
                                                            <td class="red center">
                                                                <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${row_id}" title="${$("#Span_Delete_Title").text()}"></i>
                                                            </td>
                                                            <td id="srno" class="sr_padding">${SrNo}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${row_id}" /></td>
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class="lpo_form col-sm-10 no-padding" >
                                                                   <select class="form-control" id="TxtItemName_${row_id}" name="TxtItemName${row_id}" onchange="OnChangeItemName(event)">
                                                                        </select><span id="TxtItemNameError" class="error-message is-visible"></span>
                                                                        <input type="hidden" id="hfItemID">
<input class="" type="hidden" id="hdn_item_gl_acc" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">  </button>
                                                                </div>
                                                                <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                            </td>
                                                            <td>
                                                                <input id="UOM" class="form-control" autocomplete="off" type="text" placeholder="${$("#ItemUOM").text()}" name="UOM" onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled="">
                                                                <input type="hidden" id="UOMID"><input id="ItemHsnCode" type="hidden" /> <input id="ItemtaxTmplt" type="hidden" />
                                                            </td>
                                                            <td>
                                                               
                                                                    <div class="lpo_form"><input id="ord_qty_spec" ${DisableinvQty} class="form-control num_right" autocomplete="off" type="text" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" name="ReceivedQuantity" placeholder="0000.00"><span id="ord_qty_specError" class="error-message is-visible"></span></div>
                                                             
                                                            </td>
                                                            <td>
                                                                <div class="lpo_form"><input id="item_rate" autocomplete="off" class="form-control num_right" onchange ="OnChangePOItemRate(1,event)" onkeypress="return AmountFloatRate(this,event);" type="text" name="Rate" placeholder="0000.00"><span id="item_rate_specError" class="error-message is-visible"></span></div>
                                                            </td>
                                                            <td>
                                                                <input id="item_gr_val" class="form-control num_right" autocomplete="off" type="text" name="GrossValue"  placeholder="0000.00" disabled="">
                                                            </td>
                                                           `+ TaxExempted + `
                                                           `+ ManualGst + `
                                                            <td>
                                                                <div class=" col-sm-10 num_right no-padding" >
                                                                    <input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text"  name="item_tax_amt" placeholder="0000.00" disabled="">
                                                                </div><div class=" col-sm-2 num_right no-padding" >
                                                                    <button type="button" class="calculator item_pop" id="BtnTxtCalculation" onclick="OnClickTaxCalBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#TaxInfo").text()}"></i></button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input id="TxtOtherCharge" class="form-control num_right" autocomplete="off" type="text"  name="OtherCharge" placeholder="0000.00" disabled="">
                                                            </td>
                                                            <td style="display:none;">
                                                                <input id="item_net_val_spec" class="form-control num_right" autocomplete="off" type="text"  name="NetValue" placeholder="0000.00" disabled="">
                                                            </td>
                                                            <td>
                                                                <input id="item_net_val_bs" class="form-control num_right" autocomplete="off" type="text"  name="NetValueInBase" placeholder="0000.00" disabled="">
                                                            </td>
                                                            <td>
                                                                             <textarea id="remarks"  value="" class="form-control remarksmessage" name="remarks" maxlength="200" onmouseover="OnMouseOver(this)" title="${$("#span_remarks").text()}" placeholder="${$("#span_remarks").text()}"></textarea>
                                                                         </td>
                                                        </tr>`)
}
function OnClickTaxCalBtn(e) {
    //debugger;

    var POItemListName = "#TxtItemName_"
    var SNohiddenfiled = "#SNohiddenfiled";
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
    $("#HdnTaxOn").val("Item");
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, POItemListName)
    //debugger;
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
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
function BindItemDdlPQ(rowid) {
    if ($("#TxtItemName_" + rowid).val() == "0" || $("#TxtItemName_" + rowid).val() == "" || $("#TxtItemName_" + rowid).val() == null) {
        $("#TxtItemName_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + rowid).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    DynamicSerchableItemDDL("#POInvItmDetailsTbl", "#TxtItemName_", rowid, "#SNohiddenfiled", "", "ConsumableInvoice")
}
function OnChangePOItemQty(RowID, e) {
    //debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    if (OrderQty != "" && OrderQty != ".") {
        OrderQty = parseFloat(OrderQty);
    }
    if (OrderQty == ".") {
        OrderQty = 0;
    }
    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#ord_qty_spec").val("");
        ItmRate = 0;
        // return false;
    }
    if (OrderQty == "" || OrderQty == 0 ) {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#ord_qty_spec");//.focus();
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_specError").text("");
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
        //GetAllGLID();
    }
    clickedrow.find("#ord_qty_spec").val(parseFloat(CheckNullNumber(OrderQty)).toFixed(QtyDecDigit));
    //debugger;
    CalculationBaseRate(e);
    //debugger
    OnClickSaveAndExit_OC_Btn();
    var item_gr_val = parseFloat(CheckNullNumber(clickedrow.find("#item_gr_val").val()));
    if (item_gr_val > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
    }
    //debugger;
    //GetAllGLID();//commented by shubham maurya on 21-06-2025
    let SuppId = $("#SupplierName").val();
    let GrossAmt = $("#TxtGrossValue").val();
    AutoTdsApply(SuppId, GrossAmt).then(() => {
    });
}
function OnChangeAssessAmt(e) {
    ////debugger;
    var docid = $("#DocumentMenuId").val();
    var errorFlag = "N";
    var DecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var AssessAmt = parseFloat(0).toFixed(DecDigit);
    ItmCode = clickedrow.find("#TxtItemName_" + Sno).val();
    AssessAmt = clickedrow.find("#item_gr_val").val();
    if (AvoidDot(AssessAmt) == false) {
        clickedrow.find("#item_ass_valError").text($("#valueReq").text());
        clickedrow.find("#item_ass_valError").css("display", "block");
        clickedrow.find("#item_gr_val").css("border-color", "red");
        errorFlag = "Y";
    }
    if (CheckItemRowValidation(e) == false) {
        errorFlag = "Y";
    }
    if (errorFlag == "Y") {
        clickedrow.find("#item_gr_val").val("");
        AssessAmt = 0;
        //return false;
    }
    if (parseFloat(AssessAmt) > 0) {
        if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    if (parseFloat(AssessAmt).toFixed(DecDigit) > 0 && ItmCode != "" && Sno != null && Sno != "") {
        ////debugger;        
        clickedrow.find("#item_gr_val").val(parseFloat(AssessAmt).toFixed(DecDigit));
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(DecDigit));
    }
}
function OnChangeItemName(e) {
    debugger;
    var rowIdx = 0;
    var ValDecDigit = $("#ValDigit").text();///Amount
    var currentrow = $(e.target).closest('tr');
    var row_id = currentrow.find("#SNohiddenfiled").val();
    var Item_id = currentrow.find("#TxtItemName_" + row_id).val();
    var ItemIdToRemove = currentrow.find("#hfItemID").val();
    $("#HdnTaxOn").val("Item");
    currentrow.find("#hfItemID").val(Item_id);
    /***Commented By Shubham Maurya on 13-10-2023 for AfterDeleteResetPO_ItemTaxDetail delete privius data ***/
    AfterDeleteResetPO_ItemTaxDetail();
    Cmn_BindUOM(currentrow, Item_id, "", "", "pur","ItemOnchange");
    if (Item_id == "0") {
        currentrow.find("[aria-labelledby='select2-TxtItemName_" + row_id + "-container']").css("border-color", "red");
        currentrow.find("#TxtItemNameError").text($("#valueReq").text());
        currentrow.find("#TxtItemNameError").css("display", "block");
    }
    else {
        currentrow.find("[aria-labelledby='select2-TxtItemName_" + row_id + "-container']").css("border-color", "#ced4da");
        currentrow.find("#TxtItemNameError").text("");
        currentrow.find("#TxtItemNameError").css("display", "none");
        $("#ddlSourceType").attr("disabled", true);
        //if ($("#ddlSourceType").val() == "Q") {
        $("#SupplierNameList").attr("disabled", true);
        $("#Prospect").attr("disabled", true);
        $("#Supplier").attr("disabled", true);
        $("#Prosbtn").css("display", "none");
        $("#HdnSourceType").val($("#ddlSourceType").val());
        //debugger;

        //debugger;
        currentrow.find("#ord_qty_spec").val("");
        currentrow.find("#item_rate").val("");
        currentrow.find("#item_tax_amt").val("");
        currentrow.find("#item_gr_val").val("");
        currentrow.find("#item_net_val_bs").val("");
        currentrow.find("#TxtOtherCharge").val("");
        currentrow.find("#TxtOtherCharges").val("");
        //$('#VoucherDetail tbody tr').remove();
        //$("#DrTotal").text(parseFloat(0).toFixed(ValDecDigit));
        //$("#CrTotal").text(parseFloat(0).toFixed(ValDecDigit));
    }
   /***Commented By Shubham Maurya on 13-10-2023 for GetAllGLID ***/
    //GetAllGLID();
    //  HideSelectedItem("#PQItemListName_", row_id, "#POInvItmDetailsTbl", "#SNohiddenfiled");
    if (ItemIdToRemove !== Item_id) {
        //DelDeliSchAfterDelItem(ItemIdToRemove);
    }
}
function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertCIDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
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
function BtnSearch() {
    //debugger;
    FilterCI_List();
    ResetWF_Level();
}
function FilterCI_List() {
    //debugger;
    try {
        var SuppId = $("#CIListSupplier option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ConsumableInvoice/SearchCI_Detail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                //debugger;
                $('#tbodyPIList').html(data);
                $("#FilterData").val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status)
            },
            error: function OnError(xhr, errorType, exception) {
                //debugger;
            }
        });
    } catch (err) {
        //debugger;
        console.log("PI Error : " + err.message);

    }
}
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
function PQOnClickOtherChargeBtn() {
    //debugger
    OnClickOtherChargeBtn();
    EnableTaxAndOC();
}
function EnableTaxAndOC() {
    var status = "";
    var PQDTransType = "";
    if ($("#hfStatus").val() != null && $("#hfStatus").val() != "") {
        status = $("#hfStatus").val().trim();
        PQDTransType = sessionStorage.getItem("PQTransType");
    }


    if (status == "D" && PQDTransType == "Update") {
        $("#Tax_Template").attr('disabled', false);
        $("#Tax_Type").attr('disabled', false);
        $("#Tax_Percentage").prop("readonly", false);
        $("#Tax_Level").attr('disabled', false);
        $("#Tax_ApplyOn").attr('disabled', false);
        $("#TaxResetBtn").css("display", "block");
        $("#TaxSaveTemplate").css("display", "block");
        $("#ReplicateOnAllItemsBtn").css("display", "block");
        $("#SaveAndExitBtn").css("display", "block");
        $("#TaxExitAndDiscard").css("display", "block");
        $("#Icon_AddNewRecord").css("display", "block");

        $("#plus_icon1").css("display", "block");
        $("#OtherCharge_DDL").attr('disabled', false);
        $("#TxtOCAmt").prop("readonly", false);
        $("#SaveAndExitBtn_OC").css("display", "block");
        $("#DiscardAndExit_OC").css("display", "block");

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#OCDelIcon").css("display", "block");
        });

    }
    else if (status == null || status == "") {
        $("#Tax_Template").attr('disabled', false);
        $("#Tax_Type").attr('disabled', false);
        $("#Tax_Percentage").prop("readonly", false);
        $("#Tax_Level").attr('disabled', false);
        $("#Tax_ApplyOn").attr('disabled', false);
        $("#TaxResetBtn").css("display", "block");
        $("#TaxSaveTemplate").css("display", "block");
        $("#ReplicateOnAllItemsBtn").css("display", "block");
        $("#SaveAndExitBtn").css("display", "block");
        $("#TaxExitAndDiscard").css("display", "block");
        $("#Icon_AddNewRecord").css("display", "block");

        $("#plus_icon1").css("display", "block");
        $("#OtherCharge_DDL").attr('disabled', false);
        $("#TxtOCAmt").prop("readonly", false);
        $("#SaveAndExitBtn_OC").css("display", "block");
        $("#DiscardAndExit_OC").css("display", "block");

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#OCDelIcon").css("display", "block");
        });
    }
    else {
        $("#Tax_Template").attr('disabled', true);
        $("#Tax_Type").attr('disabled', true);
        $("#Tax_Percentage").prop("readonly", true);
        $("#Tax_Level").attr('disabled', true);
        $("#Tax_ApplyOn").attr('disabled', true);
        $("#TaxResetBtn").css("display", "none");
        $("#TaxSaveTemplate").css("display", "none");
        $("#ReplicateOnAllItemsBtn").css("display", "none");
        $("#SaveAndExitBtn").css("display", "none");
        $("#TaxExitAndDiscard").css("display", "none");
        $("#Icon_AddNewRecord").css("display", "none");

        $(".plus_icon1").css("display", "none");
        $("#OtherCharge_DDL").attr('disabled', true);
        $("#TxtOCAmt").prop("readonly", true);
        $("#SaveAndExitBtn_OC").css("display", "none");
        $("#DiscardAndExit_OC").css("display", "none");
        //debugger
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {//Tbl_OC_Deatils
            var currentRow = $(this);
            //debugger
            currentRow.find("#OCDelIcon").css("display", "none");//OCDelIcon OCDelIcon
        });
    }
}
function OnChangePOItemRate(RowID, e) {
    
    CalculationBaseRate(e);
    //***Modfyed By Shubham Maurya on 05-01-2024***//
    //OnClickSaveAndExit_OC_Btn();
    var clickedrow = $(e.target).closest("tr");
    var item_gr_val = parseFloat(CheckNullNumber(clickedrow.find("#item_gr_val").val()));
    if (item_gr_val > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
    }
    //GetAllGLID();
    let SuppId = $("#SupplierName").val();
    let GrossAmt = $("#TxtGrossValue").val();
    AutoTdsApply(SuppId, GrossAmt).then(() => {
    });
    //$("#BtnTxtCalculation").prop("disabled", false);
}
function CalculationBaseRate(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var docid = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;
    OrderQty = CheckNullNumber(clickedrow.find("#ord_qty_spec").val());
    ItemName = clickedrow.find("#TxtItemName_" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#TxtItemNameError").text($("#valueReq").text());
        clickedrow.find("#TxtItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-TxtItemName_" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-TxtItemName_" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || parseFloat(OrderQty) == "0" || ItemName == "0") {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#ord_qty_spec");//.focus();
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
    }
    debugger;
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(CheckNullNumber(ItmRate));
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 ) {
        //clickedrow.find("#item_rate_specError").text($("#valueReq").text());
        //clickedrow.find("#item_rate_specError").css("display", "block");
        //clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#item_rate");//.focus();
        }
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rate_specError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }
    //if (errorFlag == "Y") {
    //    return false;
    //}
    debugger;
    if (AvoidDot(CheckNullNumber(ItmRate)) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = FinVal * ConvRate
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    //Commented by Suraj Maurya on 09-12-2024 due to not in use
    //if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
    //    CalculateDisPercent(e);
    //}
    //if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
    //    CalculateDisAmt(e);
    //}
    clickedrow.find("#item_rate").val(parseFloat(CheckNullNumber(ItmRate)).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (clickedrow.find("#ManualGST").is(":checked")) {
                //clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
                //clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            }
            else {
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
            }
        } else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
        }
       
    }
   
    //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
    var assVal = clickedrow.find("#item_gr_val").val();
    assVal = parseFloat(assVal);
    //debugger;
    if (assVal > 0) {
        if (docid != '105101152') {
            //debugger;
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_gr_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
}
function OnChangeGrossAmt() {
    debugger;
    //var TotalOCAmt = $('#Total_OC_AmountInBs').text();//Commented by Suraj Maurya on 03-01-2025
    var TotalOCAmt = $('#TxtDocSuppOtherCharges').val();
    //var TotalOCAmt = $('#Total_OC_Amount').text();
    //var Total_PO_OCAmt = $('#PO_OtherCharges').val();
    var Total_PO_OCAmt = $('#TxtOtherCharges').val();

    //if (parseFloat(Total_PO_OCAmt) > 0) {
       // if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {
             //Calculate_OC_AmountItemWise(Total_PO_OCAmt);
       // }
    //}

    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
        //if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {//Commented by Suraj Maurya on 03-01-2025
            Calculate_OC_AmountItemWise(TotalOCAmt);
        //}
    }
}
function OnClickSupplierInfoIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";

    ItmCode = clickedrow.find("#TxtItemName_" + Sno).val();
    var Supp_id = $('#SupplierName').val();

    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
async function CalculateAmount() {
    debugger;
    console.log("CalculateAmount")
    var DecDigit = $("#ValDigit").text();
    var conv_rate = $("#conv_rate").val();
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        ////debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_net_val_spec").val() == "" || currentRow.find("#item_net_val_spec").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val_spec").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_net_val_bs").val() == "" || currentRow.find("#item_net_val_bs").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#item_net_val_bs").val())).toFixed(DecDigit);
        }
    });
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
    var oc_amount = $("#TxtDocSuppOtherCharges").val();
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amount));
    NetOrderValueSpec = NetOrderValueBase / conv_rate
    $("#NetOrderValueSpe").val(NetOrderValueSpec.toFixed(DecDigit));
    $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(DecDigit));
};
function SetOtherChargeVal() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed($("#ValDigit").text()));
    })
}
//function CalculateDisPercent(e) {//Commented by Suraj Maurya on 09-12-2024 due to not in use
    
//    var ConvRate = $("#conv_rate").val();
//    var RateDecDigit = $("#RateDigit").text();
//    var ValDecDigit = $("#ValDigit").text();
//    var clickedrow = $(e.target).closest("tr");
//    var Sno = clickedrow.find("#SNohiddenfiled").val();
//    var ItemName;
//    var OrderQty;
//    var ItmRate;
//    var DisPer;

//    ItemName = clickedrow.find("#TxtItemName_" + Sno).val();
//    OrderQty = clickedrow.find("#ord_qty_spec").val();
//    ItmRate = clickedrow.find("#item_rate").val();
//    DisPer = clickedrow.find("#item_disc_perc").val();

//    if (AvoidDot(DisPer) == false) {
//        clickedrow.find("#item_disc_perc").val("");
//        DisPer = 0;
//        //return false;
//    }
//    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
//        var FAmt = (ItmRate * DisPer) / 100;
//        var GAmt = OrderQty * (ItmRate - FAmt);
//        var DisVal = OrderQty * FAmt;
//        var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
//        var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);

//        clickedrow.find("#item_gr_val").val(FinGVal);
//        clickedrow.find("#item_gr_val").val(FinGVal);
//        clickedrow.find("#item_net_val_spec").val(FinGVal);
//        FinalGVal = ConvRate * FinGVal
//        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalGVal).toFixed(ValDecDigit));
//        clickedrow.find("#item_disc_val").val(FinDisVal);
//        CalculateAmount();
//        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(RateDecDigit));
//        clickedrow.find("#item_disc_amt").prop("readonly", true);
//        clickedrow.find("#item_disc_amt").val("");
//    }
//    else {
//        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
//            var FAmt = OrderQty * ItmRate;
//            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
//            clickedrow.find("#item_gr_val").val(FinVal);
//            clickedrow.find("#item_gr_val").val(FinVal);
//            clickedrow.find("#item_net_val_spec").val(FinVal);
//            FinalVal = ConvRate * FinVal
//            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
//            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
//            CalculateAmount();
//        }
//        clickedrow.find("#item_disc_amt").prop("readonly", false);
//    }
//    OnChangeGrossAmt();
//    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
//    //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());

//}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {
                    /*-----------Added by Suraj on 14-06-2024---------*/

                    var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted) {
                        AssessVal = 0;
                    }
                    var GstApplicable = $("#Hdn_GstApplicable").text();
                    if (GstApplicable == "Y") {

                        var ManualGST = ItemRow.find("#ManualGST").is(":checked");
                        var item_tax_amt = ItemRow.find("#item_tax_amt").val();
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
                var TaxAccId = currentRow.find("#TaxAccId").text();

                NewArray.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").closest('tr').each(function () {
                //debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#TxtItemName_" + Sno).val();

                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr');
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            //debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
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
                                currentRow.find("#item_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(DecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                    OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

                                //}
                            }
                        });
                    }
                    else {
                        //debugger;
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            OC_Amt_OR = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        //currentRow.find("#item_gr_val").val(GrossAmtOR);
                        currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        currentRow.find("#item_net_val_bs").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
            CalculateAmount();
            //sessionStorage.removeItem("TaxCalcDetails");
            //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(FTaxDetailsItemWise));
            BindTaxAmountDeatils(NewArray);
        }
    }
}
//function CalculateDisAmt(e) {//Commented by Suraj Maurya on 09-12-2024 due to not in use
//    //debugger;
//    var ConvRate = $("#conv_rate").val();
//    var ValDecDigit = $("#ValDigit").text();
//    var clickedrow = $(e.target).closest("tr");
//    var Sno = clickedrow.find("#SNohiddenfiled").val();
//    var ItemName;
//    var OrderQty;
//    var ItmRate;
//    var DisAmt;
//    ItemName = clickedrow.find("#TxtItemName_" + Sno).val();
//    OrderQty = clickedrow.find("#ord_qty_spec").val();
//    ItmRate = clickedrow.find("#item_rate").val();
//    DisAmt = clickedrow.find("#item_disc_amt").val();
//    if (AvoidDot(DisAmt) == false) {
//        clickedrow.find("#item_disc_amt").val("");
//        DisAmt = 0;
//        //return false;
//    }
//    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
//        if (Math.fround(ItmRate) > Math.fround(DisAmt)) {
//            var FRate = (ItmRate - DisAmt);
//            var GAmt = OrderQty * FRate;
//            var DisVal = OrderQty * DisAmt;
//            var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
//            var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
//            clickedrow.find("#item_disc_amtError").text("");
//            clickedrow.find("#item_disc_amtError").css("display", "none");
//            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
//            clickedrow.find("#item_disc_val").val(FinDisVal);
//            clickedrow.find("#item_gr_val").val(FinGVal);
//            clickedrow.find("#item_gr_val").val(FinGVal);
//            clickedrow.find("#item_net_val_spec").val(FinGVal);
//            FinalGVal = ConvRate * FinGVal
//            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalGVal).toFixed(ValDecDigit));
//            clickedrow.find("#item_disc_perc").prop("readonly", true);
//            clickedrow.find("#item_disc_perc").val("");
//            CalculateAmount();
//        }
//        else {
//            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
//            clickedrow.find("#item_disc_amtError").css("display", "block");
//            clickedrow.find("#item_disc_amt").css("border-color", "red");
//            clickedrow.find("#item_disc_amt").val('');
//            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
//        }
//        clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ValDecDigit));
//    }
//    else {
//        clickedrow.find("#item_disc_amtError").text("");
//        clickedrow.find("#item_disc_amtError").css("display", "none");
//        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
//        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
//            var FAmt = OrderQty * ItmRate;
//            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
//            clickedrow.find("#item_gr_val").val(FinVal);
//            clickedrow.find("#item_gr_val").val(FinVal);
//            clickedrow.find("#item_net_val_spec").val(FinVal);
//            FinalVal = ConvRate * FinVal
//            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
//            CalculateAmount();
//        }
//        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
//        clickedrow.find("#item_disc_perc").prop("readonly", false);
//    }
//    OnChangeGrossAmt();
//    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_gr_val").val());
//}
function OnClickIconBtn(e) {
    //debugger;
    var row_id = $(e.target).closest('tr').find("#SNohiddenfiled").val();
    var ItmCode = $(e.target).closest('tr').find("#TxtItemName_" + row_id).val();
    ItemInfoBtnClick(ItmCode);
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
        $('#billNoErrorMsg').text($("#valueReq").text()).css("display", "block");
        return;
    } else {
        clearError("#Bill_No", "#billNoErrorMsg");
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
                    showError("#Bill_No", "#billNoErrorMsg");
                    showError("#Bill_Date", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else if (hdn_Bill_Dt !== Bill_Date) {
                    showError("#Bill_No", "#billNoErrorMsg");
                    showError("#Bill_Date", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("Y");
                } else {
                    clearError("#Bill_No", "#billNoErrorMsg");
                    clearError("#Bill_Date", "#BillDateErrorMsg");
                    $("#duplicateBillNo").val("N");
                }
            } else {
                clearError("#Bill_No", "#billNoErrorMsg");
                clearError("#Bill_Date", "#BillDateErrorMsg");
                $("#duplicateBillNo").val("N");
            }
        }
    });

    RefreshBillNoBillDateInGLDetail();
}
async function OnChangeBillNo1() {
    debugger;
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#billNoErrorMsg').text($("#valueReq").text());
        $("#billNoErrorMsg").css("display", "block");
    }
    else {
        $("#billNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
       
        //return true;
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    var Bill_No = $("#Bill_No").val();
    var supp_id = $("#SupplierName").val();
    var hdn_Bill_No = $("#hdn_Bill_No").val();
    await $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
            data: { supp_id: supp_id, Bill_No: Bill_No, doc_id: DocumentMenuId },
            dataType: "json",
            success: function (data) {
                debugger
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table[0].Result == "Duplicate") {
                        if (hdn_Bill_No != Bill_No) {
                            $("#Bill_No").css("border-color", "Red");
                            $('#billNoErrorMsg').text($("#valueduplicate").text());
                            $("#billNoErrorMsg").css("display", "block");
                            $("#duplicateBillNo").val("Y");
                        }
                        else {
                            $("#billNoErrorMsg").css("display", "none");
                            $("#Bill_No").css("border-color", "#ced4da");
                            $("#duplicateBillNo").val("N");
                        }
                    }
                    else {
                        $("#billNoErrorMsg").css("display", "none");
                        $("#Bill_No").css("border-color", "#ced4da");
                        $("#duplicateBillNo").val("N");
                    }
                }
            },
        });
    RefreshBillNoBillDateInGLDetail(); 
}
function OnChangeBillDate() {
    if ($("#Bill_Date").val() == "") {
        $('#BillDateErrorMsg').text($("#valueReq").text());
        $("#BillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        /*------Code Added by Suraj Maurya on 22-10-2024------*/
        var minDate = new Date('1900-01-01')._format('Y-m-d');
        var billDate = new Date($("#Bill_Date").val())._format('Y-m-d');

        if (billDate > minDate) {
            $("#BillDateErrorMsg").css("display", "none");
            $("#Bill_Date").css("border-color", "#ced4da");
            //GetAllGLID();
            RefreshBillNoBillDateInGLDetail();
        } else {
            $('#BillDateErrorMsg').text($("#JC_InvalidDate").text());
            $("#BillDateErrorMsg").css("display", "block");
            $("#Bill_Date").css("border-color", "Red");
        }
        /*------Code Added by Suraj Maurya on 22-10-2024 End------*/
    }
    OnChangeBillNo();
}
function OnClickbillingAddressIconBtn(e) {
    //debugger;
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
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    //debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    if (parseFloat(CheckNullNumber(TotalOCAmt)) < 0) {
        TotalOCAmt = 0;
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#item_gr_val").val();
        var NetOrderValueSpec = currentRow.find("#item_net_val_spec").val();
        var NetOrderValueBase = currentRow.find("#item_net_val_bs").val();

        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(CheckNullNumber(OCAmtItemWise)).toFixed(DecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
            }
        } else {//Added by Suraj Maurya on 14-02-2025
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
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
        var Sno = 0;
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            Sno = Sno + 1;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        var Sno = 0;
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            Sno = Sno + 1;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);

        var POItm_GrossValue = currentRow.find("#item_gr_val").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(DecDigit);
        }
        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#item_net_val_spec").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(DecDigit));
        FinalPOItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(DecDigit);
        if (FinalPOItm_NetOrderValueBase == "NaN") {
            currentRow.find("#item_net_val_bs").val((parseFloat(0)).toFixed(DecDigit));
        }
        else {
            currentRow.find("#item_net_val_bs").val((parseFloat(FinalPOItm_NetOrderValueBase)).toFixed(DecDigit));
        }
    });
    CalculateAmount();
};
function FromToDateValidation() {
    //debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            //alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    else {
        //alert('please select from and to date');
    }
}
function Ass_valFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_ass_valError" + RowNo).css("display", "none");
    clickedrow.find("#item_ass_val" + RowNo).css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");
    return true;
}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#ord_qty_specError" + RowNo).css("display", "none");
    clickedrow.find("#ord_qty_spec" + RowNo).css("border-color", "#ced4da");
    return true;
}
function AmountFloatRate(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#ord_qty_specError" + RowNo).css("display", "none");
    clickedrow.find("#ord_qty_spec" + RowNo).css("border-color", "#ced4da");
    return true;
}
function AmtFloatValueonly(el, evt) {
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
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");

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
function OnClickSaveAndExit(ApplyGL) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    let NewArr = [];
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val(); 
    if (TaxOn != "OC") {
        //var tbllenght = $("#TaxCalculatorTbl tbody tr").length
        //if (tbllenght == 0) {
        //    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
        //    $("#SpanTax_Template").text($("#valueReq").text());
        //    $("#SpanTax_Template").css("display", "block");
        //    $("#SaveAndExitBtn").attr("data-dismiss", "");
        //    return false;
        //}
        //else {
        //    $("#SaveAndExitBtn").attr("data-dismiss", "modal");
        //}
    }
    
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    if ($("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();
            var TaxAccId = currentRow.find("#TaxAccId").text();

            if (TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {
                NewArr.push({
                    TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage
                    , TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount
                    , TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId
                })

            }

            //if (TaxOn == "OC") {
            //    if (TaxItemID == TaxItmCode) {
            //        $(this).remove();
            //    }
            //} else {
            //    if (TaxItemID == TaxItmCode && DocNo == GRNNo && DocDate == GRNDate) {
            //        $(this).remove();
            //    }
            //    else {
            //        var TaxName = currentRow.find("#TaxName").text();
            //        var TaxNameID = currentRow.find("#TaxNameID").text();
            //        var TaxPercentage = currentRow.find("#TaxPercentage").text();
            //        var TaxLevel = currentRow.find("#TaxLevel").text();
            //        var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            //        var TaxAmount = currentRow.find("#TaxAmount").text();
            //        var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
            //        var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

            //        NewArr.push({DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })

            //    }
            //}

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
            var TaxRecov = currentRow.find("#tax_recov").text();
            var TaxAccId = currentRow.find("#AccID").text();

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
            <td id="TaxAccId">${TaxAccId}</td>
                </tr>`);

            NewArr.push({ /*UserID: UserID,DocNo: GRNNo, DocDate: GRNDate, */ TaxItmCode: TaxItmCode, TaxName: TaxName
                , TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel
                , TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID
                , TaxRecov: TaxRecov, TaxAccId: TaxAccId
            })
        });
    }
    else {
        //  var TaxCalculationList = [];
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            var TaxAccId = currentRow.find("#AccID").text();

            ////debugger
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
            <td id="TaxAccId">${TaxAccId}</td>
                </tr>`);

            NewArr.push({ /*UserID: UserID, DocNo: GRNNo, DocDate: GRNDate, */ TaxItmCode: TaxItmCode, TaxName: TaxName
                , TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel
                , TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID
                , TaxRecov: TaxRecov, TaxAccId: TaxAccId
            })
        });
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit)
                /* Added by Suraj Maurya on 09-12-2024 */
                Cmn_click_Oc_chkroundoff(null, currentRow);
                //var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                //currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
                /* Added by Suraj Maurya on 09-12-2024 */
                //currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    } else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
             //debugger;
            var currentRow = $(this);
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = parseFloat(0).toFixed(DecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (ItmCode == TaxItmCode) {
                currentRow.find("#item_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                var ValDecDigit = $("#ValDigit").text();
                if (currentRow.find("#item_net_val_bs").val() == "NaN") {
                    currentRow.find("#item_net_val_bs").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
        });
        CalculateAmount();
        //debugger;
        //if (OnAddGRN != "Y") {
        //    GetAllGLID();
        //}
    }
    if ($("#taxTemplate").text() == "GST Slab") {
        //GetAllGLID();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
        AutoTdsApply(SuppId, GrossAmt).then(() => {
        });
    }
    if (ApplyGL == "ApplyGL" || ApplyGL == "MGST") {
        //GetAllGLID();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
        AutoTdsApply(SuppId, GrossAmt).then(() => {
        });
    }
    //GetAllGLID();
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
   // if (bindval == "") {
      //  GetAllGLID();
   // }
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
        //debugger;
        if (isConfirm) {
            //debugger;
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            RemoveSessionNew();
            ResetWF_Level();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function ForwardBtnClick() {
    //debugger;
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
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var CInvDate = $("#Inv_date").val();
        $.ajax({
            type: "POST",
            /* url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: CInvDate
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
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
                    /*swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
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
    var INV_NO = "";
    var INVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var VoucherNarr = $("#hdn_Nurration").val();//$("#PurchaseVoucherRaisedAgainstInv").text()
    var Bp_Nurr = $("#hdn_BP_Nurration").val();
    var Dn_Nurr = $("#hdn_DN_Nurration").val();
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#InvoiceNumber").val();
    INVDate = $("#Inv_date").val();
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();
    var curr_id = $("#curr_id").val();
    var conv_rate = $("#conv_rate").val();
    var WF_status1 = $("#WF_status1").val();
    //SourceType = $("#ddlRequisitionTypeList").val();
    var TrancType = (INV_NO + ',' + INVDate + ',' + "Update" + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ConsumbleInvoice_"+ GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ConsumableInvoice/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {
            //debugger;
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ConsumableInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/ConsumableInvoice/Approve_ConsumableInvoice?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&VoucherNarr=" + VoucherNarr + "&FilterData=" + FilterData + "&curr_id=" + curr_id + "&conv_rate=" + conv_rate + "&WF_status1=" + WF_status1 + "&Bp_Nurr=" + Bp_Nurr + "&Dn_Nurr=" + Dn_Nurr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ConsumableInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        //debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ConsumableInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(INV_NO, INV_Date, fileName) {
//    debugger;
//    var Inv_no = INV_NO;
//    var InvDate = INV_Date;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/ConsumableInvoice/SavePdfDocToSendOnEmailAlert",
//        data: { Inv_no: Inv_no, InvDate: InvDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    //debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {

}
function OnClickReplicateOnAllItems() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var RowSNo = $("#HiddenRowSNo").val();
    var CitmRowSNo = RowSNo;
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
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
   // $("#Hdn_TaxCalculatorTbl > tbody > tr").remove();
    if (TaxOn == "Item") {
        $("#Hdn_TaxCalculatorTbl > tbody > tr").remove();
    }
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        var TaxRecov = currentRow.find("#tax_recov").text();
        var TaxAccId = currentRow.find("#AccID").text();
        TaxCalculationList.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
        TaxCalculationListFinalList.push({ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "POInvItmDetailsTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = currentRow.find("#TxtGrnNo").val();
                GRNDateTbl = currentRow.find("#hfGRNDate").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                } else {
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#item_gr_val").val();
                }


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                for (i = 0; i < TaxCalculationList.length; i++) {
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationList[i].TaxName;
                    var TaxNameID = TaxCalculationList[i].TaxNameID;
                    var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                    TaxPercentage = TaxCalculationList[i].TaxPercentage;
                    var TaxLevel = TaxCalculationList[i].TaxLevel;
                    var RowSNo = TaxCalculationList[i].RowSNo;
                    var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                    var TaxRecov = TaxCalculationList[i].TaxRecov;
                    var TaxAccId = TaxCalculationList[i].TaxAccId;
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
                    NewArray.push({ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            var DocNo = NewArray[k].DocNo;
                            var DocDate = NewArray[k].DocDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var RowSNo = NewArray[k].RowSNo;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            var TaxRecov = NewArray[k].TaxRecov;
                            var TaxAccId = NewArray[k].TaxAccId;
                            if (CitmTaxItmCode != TaxItmCode && CitmRowSNo != RowSNo/* && GRNNo == DocNo && GRNDate == DocDate*/) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId })
                            }
                         //   if (CitmTaxItmCode == TaxItmCode && CitmRowSNo != RowSNo/* && GRNNo != DocNo && GRNDate == DocDate*/) {
                         //       if (TaxOn != "OC") {
                         //           TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov, TaxAccId: TaxAccId })
                         //       }
                            //}
                        //    if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                        //        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov, TaxAccId: TaxAccId })
                        //    }
                        //    if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                        //        if (TaxOn != "OC") {
                        //            TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov, TaxAccId: TaxAccId })
                        //        }
                        //    }
                        //    if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                        //        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID,TaxRecov:TaxRecov, TaxAccId: TaxAccId })
                        //    }
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
    //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            
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
            <td id="TaxRecov">${TaxCalculationListFinalList[i].TaxRecov}</td>
            <td id="TaxAccId">${TaxCalculationListFinalList[i].TaxAccId}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    //debugger;
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
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
    } else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        //var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        //var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID ) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                            currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                    currentRow.find("#item_net_val_spec").val(FGrossAmt);
                    FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                    currentRow.find("#item_net_val_bs").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                currentRow.find("#item_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val())).toFixed(DecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                currentRow.find("#item_net_val_spec").val(FGrossAmt);
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                currentRow.find("#item_net_val_bs").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
        //GetAllGLID();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
        AutoTdsApply(SuppId, GrossAmt).then(() => {
        });
    }

}
//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var GLAcc_Name = clickedrow.find("#txthfAccID").val();
    var GLAcc_id = clickedrow.find("#hfAccID").val();
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
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ConsumableInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            //debugger;
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
function BindSrcDocNumberOnBehalfSrcType() {
    //debugger;
    SuppID = $("#SupplierName").val();
    if (SuppID != null && SuppID != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/ConsumableInvoice/GetSourceDocList',
            data: { SuppID: SuppID },
            success: function (data) {
                //debugger;
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        $("#src_doc_number option").remove();
                        $("#src_doc_number optgroup").remove();
                        $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#src_doc_number').select2({
                            templateResult: function (data) {
                                //debugger;
                                var list = [];
                                $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
                                    //debugger;
                                    var currentRow = $(this);
                                    var Suppid = "";
                                    var rowin = currentRow.find('#SpanRowId').text();
                                    Suppid = currentRow.find('#src_doc_' + rowin).val();
                                    list.push(Suppid);
                                });

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

                        $("#src_doc_date").val("");

                    }
                }
            }

        })
    }
}
function OnChangeSourType() {
    //debugger;
    var srctype = $("#src_type").val();
    if (srctype == "D") {
        $("#srcdocNO_div").css("display" ,"none");
        $("#srcdocdt_div").css("display" ,"none");
        $("#plusbtn_div").css("display", "none");
        $("#HeaderclearDiv").css("display", "none");
       // $("#SupplierName").val("0").trigger('change');
    }
    if (srctype == "O") {
        $("#srcdocNO_div").css("display", "block");
        $("#srcdocdt_div").css("display", "block");
        $("#HeaderclearDiv").css("display", "block");
        //$("#SupplierName").val("0").trigger('change');      
    }

}
function OnchangeSrcDocNumber() {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var Src_Type = $("#src_type").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var src_docno = $("#src_doc_number").val();
    if (src_docno == "" || src_docno == "0" || src_docno == "---Select---") {
        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
        $("#src_doc_number").css("border-color", "red");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#src_doc_number").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
    }
    //debugger
    if (doc_no != 0) {
        var doc_Date = $("#src_doc_number option:selected")[0].dataset.date
        var newdate = doc_Date.split("-").reverse().join("-");

        $("#src_doc_date").val(newdate);
        $("#src_doc_number").val(doc_no);

        $("#plusbtn_div").css("display", "block");
        $("#src_type").attr("disabled", true);
        $("#SupplierName").attr("disabled", true);
        var suppid = $("#SupplierName").val();
        var srdocNo = $("#src_doc_number").val();
        var srcdoc_dt = $("#src_doc_date").val();
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/ConsumableInvoice/GetBillNoBillDate',
            data: { suppid: suppid, srdocNo: srdocNo, srcdoc_dt: srcdoc_dt },
            success: function (data) {
                //debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                debugger;
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0)
                    {
                        var BillNo = arr.Table[0].bill_no;
                        var BillDate = arr.Table[0].bill_date;

                        $("#Bill_No").val(BillNo);
                        $("#Bill_Date").val(moment(BillDate).format("YYYY-MM-DD"));
                        $("#Bill_No").css("border-color", "#ced4da");
                        $("#billNoErrorMsg").css("display", "none");
                    }
                }
            }
        })
    }
   
}
function AddAttribute() {
    //debugger;
    var ErrorFlag = "Y";
    var src_docno = $("#src_doc_number").val();
    if (src_docno == "" || src_docno == "0" || src_docno == "---Select---") {
        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
        $("#src_doc_number").css("border-color", "red");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#src_doc_number").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
    }
    if (ErrorFlag == "N") {
        return false;
    }
    else {
        //debugger;
        var ConvRate;
        ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        $("#conv_rate").prop("readonly", true);
        var ValDecDigit = $("#ValDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
        var RateDecDigit = $("#RateDigit").text();
        var suppid = $("#SupplierName").val();
        var srdocNo = $("#src_doc_number").val();
        var srcdoc_dt = $("#src_doc_date").val();
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/ConsumableInvoice/getdatapoitemconsinvoice',
            data: { suppid: suppid, srdocNo: srdocNo, srcdoc_dt: srcdoc_dt },
            success: function (data) {
                //debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            AddNewRow();
                            $("#plusbtn_div").css("display", "none")
                        }

                        var DataSno = 0;
                        var Dmenu = $("#DocumentMenuId").val();
                        $("#POInvItmDetailsTbl >tbody >tr").each(function () {

                            var currentRow = $(this);
                            debugger;
                            var TotalGrossVal = ((parseFloat(arr.Table[DataSno].item_gr_val)) + (parseFloat(arr.Table[DataSno].item_tax_amt)) + (parseFloat(arr.Table[DataSno].item_oc_amt)));
                            var BaseVal;
                            BaseVal = (parseFloat(ConvRate).toFixed(ValDecDigit) * parseFloat(TotalGrossVal).toFixed(ValDecDigit));
                            var SNo = currentRow.find("#SNohiddenfiled").val();
                            var itm_id = currentRow.find("#TxtItemName_" + SNo).val();
                            if (currentRow.find("#TxtItemName_" + SNo).val() == null || currentRow.find("#TxtItemName_" + SNo).val() == "0") {                 
                                currentRow.find("#TxtItemName_" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table1[DataSno].item_id).trigger('change.select2');
                                var clickedrow = currentRow.find("#TxtItemName_" + SNo).closest("tr");
                                currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);
                                currentRow.find("#UOM").val(arr.Table[DataSno].uom_name);
                                currentRow.find("#UOMID").val(arr.Table[DataSno].uom_id);
                                currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table[DataSno].ord_qty_spec).toFixed(QtyDecDigit));
                                currentRow.find("#ItemHsnCode").val(arr.Table[DataSno].hsn_code);
                                currentRow.find("#ItemtaxTmplt").val(arr.Table[DataSno].tmplt_id);
                                
                                //var itemRate = parseFloat(arr.Table[DataSno].item_gr_val) / parseFloat(arr.Table[DataSno].ord_qty_spec)

                                //currentRow.find("#item_rate").val(parseFloat(itemRate).toFixed(RateDecDigit));
                                currentRow.find("#item_rate").val(parseFloat(CheckNullNumber(arr.Table[DataSno].item_rate)).toFixed(RateDecDigit));
                                currentRow.find("#item_gr_val").val(parseFloat(arr.Table[DataSno].item_gr_val).toFixed(ValDecDigit));
                                currentRow.find("#item_tax_amt").val(parseFloat(arr.Table[DataSno].item_tax_amt).toFixed(ValDecDigit));
                               currentRow.find("#TxtOtherCharge").val(parseFloat(arr.Table[DataSno].item_oc_amt).toFixed(ValDecDigit));
                                currentRow.find("#item_net_val_spec").val(parseFloat(arr.Table[DataSno].item_net_val_spec).toFixed(ValDecDigit));
                                currentRow.find("#item_net_val_bs").val(parseFloat(BaseVal).toFixed(ValDecDigit));
                                currentRow.find("#TaxExempted").val(arr.Table[DataSno].tax_expted);
                                var TaxExempted = currentRow.find("#TaxExempted").val();
                                if (TaxExempted == "Y") {
                                    currentRow.find("#TaxExempted").prop("disabled", false);
                                    currentRow.find('#TaxExempted').prop("checked", true);
                                }
                                currentRow.find("#TxtItemName_" + SNo).attr("disabled", true);
                                $("#src_doc_number").attr("disabled", true);
                                //debugger;
                                var Itm_ID = arr.Table[DataSno].item_id;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var docid = $("#DocumentMenuId").val();
                                //for (var k = 0; k < arr.Table.length; k++) {
                                    if (docid == '105101152') {
                                        if (GstApplicable == "Y") {
                                            $("#HdnTaxOn").val("Item");
                                            Cmn_ApplyGSTToAtable("POInvItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val", arr.Table1, "Y");
                                        }
                                        else {
                                            var br_state_code = arr.Table[2].br_state_code;
                                            var state_code = $("#Ship_StateCode").val();
                                            if (br_state_code == state_code) {
                                                $("#Hd_GstType").val("Both");
                                            }
                                            else {
                                                $("#Hd_GstType").val("IGST");
                                            }

                                            $("#hd_tax_id").val(arr.Table[DataSno].tmplt_id);
                                            //if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                            if (arr.Table[DataSno].tmplt_id != 0 && arr.Table1.length > 0) {
                                                $("#HdnTaxOn").val("Item");
                                                $("#TaxCalcItemCode").val(Itm_ID);
                                                $("#Tax_AssessableValue").val(arr.Table[DataSno].item_gr_val);
                                                $("#TaxCalcGRNNo").val(srdocNo);
                                                $("#TaxCalcGRNDate").val(srcdoc_dt);
                                                var TaxArr = arr.Table1;
                                                let selected = []; selected.push({ item_id: arr.Table[DataSno].item_id });
                                                selected = JSON.stringify(selected);
                                                TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                                selected = []; selected.push({ tmplt_id: arr.Table[DataSno].tmplt_id });
                                                selected = JSON.stringify(selected);
                                                TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                                if (TaxArr.length > 0) {
                                                    AddTaxByHSNCalculation(TaxArr);
                                                    OnClickSaveAndExit("Y");
                                                    var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                                    Reset_ReOpen_LevelVal(lastLevel);
                                                }
                                            }

                                        }
                                    //}
                                }
                                

                            }
                            DataSno = DataSno + 1;
                        })
                      
                    }

                    var TxtOC = $("#TxtOtherCharges").val();
                    //var _OCTotalAmt = $("#_OtherChargeTotalAmt").text();//Commented by Suraj Maurya on 06-01-2025
                    var _OCTotalAmt = $("#TxtDocSuppOtherCharges").text();
                    if (TxtOC == _OCTotalAmt) {
                        Calculate_OC_AmountItemWise(_OCTotalAmt)
                    }
                    CalculateAmount();
                    CalculateVoucherTotalAmount();
                    //GetAllGLID();
                    let SuppId = $("#SupplierName").val();
                    let GrossAmt = $("#TxtGrossValue").val();
                    AutoTdsApply(SuppId, GrossAmt).then(() => {
                    });
                }
            }
        })
       
    }
   
}
function ResetGRN_DDL_Detail() {
    //debugger;
    $("#GRN_Date").val("");
    var DocNo = $('#src_doc_number').val();
    $("#src_doc_number>optgroup>option[value='" + DocNo + "']").select2().hide();

    $('#src_doc_number').val("---Select---").prop('selected', true);
    $('#src_doc_number').trigger('change'); // Notify any JS components that the value changed
    $('#src_doc_number').select2('close');

    $("#SpanSourceDocNoErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
    $("#src_doc_number").css("border-color", "red");
}
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#TxtItemName_" + RowSNo).val();
    var AssAmount = currentrow.find("#item_gr_val").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        //currentrow.find("#item_rate").trigger('change');
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
        CalculateTaxExemptedAmt(e, "N");
        GetAllGLID();
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
            //GetAllGLID();
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    //debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#TxtItemName_" + RowSNo).val();
    var AssAmount = currentrow.find("#item_gr_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        //currentrow.find("#item_rate").trigger('change'); // Commented by Suraj Maurya on 09-12-2024 to reduce unnecessary Calls
        //currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find('#TaxExempted').prop("checked", false);
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        CalculateTaxExemptedAmt(e, "N")
        GetAllGLID();
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "POInvItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_gr_val")
        //CalculationBaseRate(e) // Commented by Suraj Maurya on 09-12-2024 to reduce unnecessary Calls
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
function CalculateTaxExemptedAmt(e, flag) {
    //debugger;
    var ConvRate = $("#conv_rate").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    if (flag == "Row") {
        clickedrow = e;//setting row to the clickedrow sent by the Previous method
    }
    //var OrderQty = clickedrow.find("#ord_qty_spec").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    var TxtGrnNo = clickedrow.find("#TxtGrnNo").val();
    //var ItmRate = clickedrow.find("#item_rate").val();
    var oc_amt = parseFloat(CheckNullNumber(clickedrow.find("#TxtOtherCharge").val()));
    //if (ItmRate != "" && ItmRate != ".") {
    //    ItmRate = parseFloat(ItmRate);
    //}
    //if (ItmRate == ".") {
    //    ItmRate = 0;
    //}
    //if (oc_amt == ".") {
    //    oc_amt = 0;
    //}
    //if (oc_amt != "" && oc_amt != ".") {
    //    oc_amt = parseFloat(oc_amt);
    //}
    var FinVal = parseFloat(CheckNullNumber(clickedrow.find("#item_gr_val").val())).toFixed(ValDecDigit);
    if (parseFloat(CheckNullNumber(FinVal))>0) {
        //var FinalVal = parseFloat(FinVal) * ConvRate

        var FinalNetValue = parseFloat(FinVal) + oc_amt;
        clickedrow.find("#item_net_val_spec").val(FinalNetValue.toFixed(ValDecDigit));
        clickedrow.find("#item_net_val_bs").val(FinalNetValue.toFixed(ValDecDigit));
        //CalculateAmount();
    }

    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
    }
    else {
        if (GstApplicable == "Y") {
            if (clickedrow.find("#ManualGST").is(":checked")) {
                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
                CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
            }
            else {
                CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
            }
        } else {
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
        }
    }
}

//function CalculateTaxExemptedAmt(e, flag) {
//    //debugger;
//    var ConvRate = $("#conv_rate").val();
//    var RateDecDigit = $("#RateDigit").text();
//    var ValDecDigit = $("#ValDigit").text();
//    var clickedrow = $(e.target).closest("tr");

//    var OrderQty = clickedrow.find("#ord_qty_spec").val();
//    var ItemName = clickedrow.find("#hfItemID").val();
//    var TxtGrnNo = clickedrow.find("#TxtGrnNo").val();
//    var ItmRate = clickedrow.find("#item_rate").val();
//    var oc_amt = clickedrow.find("#TxtOtherCharge").val();
//    if (ItmRate != "" && ItmRate != ".") {
//        ItmRate = parseFloat(ItmRate);
//    }
//    if (ItmRate == ".") {
//        ItmRate = 0;
//    }
//    if (oc_amt == ".") {
//        oc_amt = 0;
//    }
//    if (oc_amt != "" && oc_amt != ".") {
//        oc_amt = parseFloat(oc_amt);
//    }

//    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
//        var FAmt = OrderQty * ItmRate;
//        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
//        clickedrow.find("#item_gr_val").val(FinVal);
//        clickedrow.find("#NetOrderValueSpe").val(FinVal);
//        clickedrow.find("#item_net_val_spec").val(FinVal);
//        FinalVal = FinVal * ConvRate

//        FinalVal = FinalVal + oc_amt
//        var FinalNetValue = parseFloat(FinVal) + oc_amt;
//        clickedrow.find("#item_net_val_spec").val(FinalNetValue);
//        clickedrow.find("#NetOrderValueInBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
//        CalculateAmount();
//    }
//    clickedrow.find("#item_rate").val(parseFloat(CheckNullNumber(ItmRate)).toFixed(RateDecDigit));
//    var GstApplicable = $("#Hdn_GstApplicable").text();
//    if (clickedrow.find("#TaxExempted").is(":checked")) {
//        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
//    }
//    else {
//        //CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());//Commented by Suraj Maurya on 09-12-2024 to reduce code recall
//        if (GstApplicable == "Y") {
//            if (clickedrow.find("#ManualGST").is(":checked")) {
//                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
//                CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//                clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
//            }
//            else {
//                CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//            }
//        } else {
//            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//        }
//    }
//    //if (GstApplicable == "Y") { //Commented by Suraj Maurya on 09-12-2024 to reduce code recall
//    //    if (clickedrow.find("#ManualGST").is(":checked")) {
//    //        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
//    //        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//    //        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
//    //    }
//    //    else {
//    //        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#item_gr_val").val());
//    //    }
//    //}
//}
function approveonclick() {
    //debugger;
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
/***-------------------Roundoff------------------------------------***/
function click_chkroundoff() {
    debugger;
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
            //debugger;
            var currentrow = $(this);
            CalculateTaxExemptedAmt(currentrow, "Row")
        });
        //GetAllGLID();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
        AutoTdsApply(SuppId, GrossAmt).then(() => {
        });
    }
}
var ValDecDigit = $("#ValDigit").text();///Amount
async function click_chkplusminus() {
    debugger;
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    //var ValDecDigit = $("#ValDigit").text();
    debugger;
    if ($("#chk_roundoff").is(":checked")) {
        await addRoundOffToNetValue();
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);

        //$("#ServicePIItemTbl > tbody > tr").each(function () {
        //    //debugger;
        //    var currentrow = $(this);
        //    CalculateTaxExemptedAmt(currentrow, "N")
        //});
        CalculateAmount();
        //GetAllGLID();
        let SuppId = $("#SupplierName").val();
        let GrossAmt = $("#TxtGrossValue").val();
        AutoTdsApply(SuppId, GrossAmt).then(() => {
        });
    }
}
async function addRoundOffToNetValue(flag) {
    var ValDecDigit = $("#ValDigit").text();
    try {
        await $.ajax(
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

                                if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {//if condition Added by suraj on 09-08-2024

                                var grossval = $("#TxtGrossValue").val();
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

                                            //let valInBase = $("#TxtGrossValue").val();
                                            //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                                        }
                                        if ($("#m_round").is(":checked")) {
                                            //var finnetval = netval.split('.');
                                            var decval = '0.' + finnetval[1];
                                            fnetval = parseFloat(netval) - parseFloat(decval);
                                            $("#pm_flagval").val($("#m_round").val());

                                            //let valInBase = $("#TxtGrossValue").val();
                                            //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                                        }
                                        var roundoff_netval = Math.round(fnetval);
                                        var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                        $("#NetOrderValueInBase").val(f_netval);
                                        $("#NetOrderValueSpe").val(f_netval);
                                        if (flag == "CallByGetAllGL") {
                                            //do not call  GetAllGLID("RO");
                                        } else {
                                            GetAllGLID("RO");
                                        }
                                    }
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
//function click_chkplusminus() {
//    try {
//        $.ajax(
//            {
//                type: "POST",
//                url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
//                data: {},
//                success: function (data) {
//                    //debugger;
//                    if (data == 'ErrorPage') {
//                        PO_ErrorPage();
//                        return false;
//                    }
//                    if (data !== null && data !== "") {
//                        var arr = [];
//                        arr = JSON.parse(data);
//                        if (arr.length > 0) {
//                            if (parseInt(arr[0]["r_acc"]) > 0) {
//                                var grossval = $("#TxtGrossValue").val();
//                                var taxval = $("#TxtTaxAmount").val();
//                                //var ocval = $("#TxtOtherCharges").val();
//                                var ocval = $("#DocSuppOtherCharges").val();
//                                if (ocval == "") {
//                                    ocval = 0;
//                                }
//                                var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

//                                var netval = finalnetval;//$("#NetOrderValueInBase").val();
//                                var fnetval = 0;
//                                if (parseFloat(netval) > 0) {

//                                    if ($("#p_round").is(":checked")) {
//                                        var finnetval = netval.split('.');
//                                        var decval = '0.' + finnetval[1];
//                                        var faddval = 1 - parseFloat(decval);
//                                        fnetval = parseFloat(netval) + parseFloat(faddval);
//                                        $("#pm_flagval").val($("#p_round").val());
//                                    }
//                                    if ($("#m_round").is(":checked")) {
//                                        var finnetval = netval.split('.');
//                                        var decval = '0.' + finnetval[1];
//                                        fnetval = parseFloat(netval) - parseFloat(decval);
//                                        $("#pm_flagval").val($("#m_round").val());
//                                    }

//                                    var roundoff_netval = Math.round(fnetval);
//                                    var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
//                                    $("#NetOrderValueInBase").val(f_netval);
//                                    $("#NetOrderValueSpe").val(f_netval);
//                                    GetAllGLID();
//                                }
//                            }
//                            else {
//                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                                return false;
//                            }
//                        }
//                        else {
//                            swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                            $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                            return false;
//                        }
//                    }
//                    else {
//                        swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                        $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                        return false;
//                    }
//                },
//            });
//    } catch (err) {
//        console.log("Purchase invoice round off Error : " + err.message);
//    }
//}

/***----------------------end-------------------------------------***/
/***---------------------------GL Voucher Entry-----------------------------***/
function BindDDLAccountList() {
    //debugger;
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105101145");
}
function BindData() {
    //debugger;
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
                var firstEmptySelect = true;
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + (rowid-1)).val();
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
                rowid = parseFloat(rowid) + 1;
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
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var Acc_Name = clickedrow.find("#Acc_name_" + SNo + " option:selected").text();
    if (Acc_ID != null) {
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
        clickedrow.find("#BtnCostCenterDetail").css("border-color", "#ced4da");
    }
    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
}

function GetAllGLID(flag) {
    //Commented Codes Removed At 09-12-2024 by Suraj Maurya.
    if (flag == "Calc") {//to calculate total before getting GL Details
        CalculateAmount();
    }
    GetAllGL_WithMultiSupplier(flag);
}
function CheckCI_ItemValidations1() {
    //debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#TxtItemName_" + Sno).val() == "0") {              
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtItemNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#ord_qty_spec").val() == "") {              
                if (ErrorFlag == "N") {
                    currentRow.find("#ord_qty_spec");//focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qty_specError").css("display", "none");
                currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
            }
            if (parseFloat(currentRow.find("#item_rate").val()) == "" || parseFloat(CheckNullNumber(currentRow.find("#item_rate").val())) == 0) {
                if (ErrorFlag == "N") {
                    currentRow.find("#item_rate");//.focus();
                }

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rate_specError").css("display", "none");
                currentRow.find("#item_rate_specError").css("border-color", "#ced4da");
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
async function GetAllGL_WithMultiSupplier(flag) {
    debugger;
    console.log("GetAllGL_WithMultiSupplier")
    if ($("#POInvItmDetailsTbl > tbody > tr").length == 0) {
        $("#VoucherDetail > tbody > tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    if (CheckCI_ItemValidations1() == false) {
        return false;
    }
    if (flag != "RO") {
        if ($("#chk_roundoff").is(":checked")) {
            await addRoundOffToNetValue("CallByGetAllGL");
        }
    }
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    var DocStatus = $('#hfPIStatus').val().trim();
    /////////////////12-07-2023 Comment by Shubham Maurya //////////
    //var NetInvValue = $("#NetOrderValueSpe").val();
    var NetInvValue = $("#NetOrderValueInBase").val();
    //var NetInvValue = $("#TxtGrossValue").val();
    
    var NetTaxValue = $("#TxtTaxAmount").val();
    var conv_rate = $("#conv_rate").val();
    var ValueWithoutTax = parseFloat(NetInvValue);
    var ValDecDigit = $("#ValDigit").text();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    //SuppValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    //if ($("#OrderTypeL").is(":checked")) {
    //    InvType = "D";
    //}
    //if ($("#OrderTypeE").is(":checked")) {
    //    InvType = "I";
    //}
    var curr_id = $("#curr_id").val();
    var bill_no = $("#Bill_No").val();
    var bill_dt = $("#Bill_Date").val();
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id:""
    });
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {

        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        var ItmGrossVal = currentRow.find("#item_gr_val").val();
        var ItmGrossValInBase = currentRow.find("#item_gr_val").val();
        var ItemAccId = currentRow.find("#hdn_item_gl_acc").val()
        //var TxtGrnNo = currentRow.find("#TxtGrnNo").val();
        var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
        var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
        var TxaExanted = "N";
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExanted = "Y";
            TxaExantedItemList.push({ item_id: item_id });
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id });
            }
        }
        GLDetail.push({
            comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
            , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ItemAccId
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
            bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date,''), acc_id: ""
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
            //var ClaimItc = true;
            //if (GstApplicable == "Y") {
            //    ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
            //}
            //if (TaxRecov == "N" || !ClaimItc) {
            //    if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
            //        var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
            //        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
            //        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
            //    }
            //} else {
            //    GLDetail.push({
            //        comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
            //        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
            //        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            //    });
            //}
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
    
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            //debugger;
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
    await Cmn_GLTableBind(supp_acc_id, GLDetail,"Purchase");
    //await $.ajax({
    //    type: "POST",
    //    url: "/Common/Common/GetGLDetails1",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify({ GLDetail: GLDetail }),
    //    success: function (data) {
    //        //debugger;
         
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
    //                    var arrSupp = arr.Table.filter(v => (v.type == "TSupp" || v.type == "Supp" || v.type == "Bank"));
    //                    var mainSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "Supp");
    //                    var TdsSuppGl = arrSupp.filter(v => v.acc_id == supp_acc_id && v.type == "TSupp");
    //                    var NewArrSupp = arrSupp.filter(v => v.acc_id != supp_acc_id);
    //                    if (TdsSuppGl.length > 0) {
    //                        NewArrSupp.unshift(TdsSuppGl[0]);
    //                    }
    //                    NewArrSupp.unshift(mainSuppGl[0]);
    //                    arrSupp = NewArrSupp;
    //                    for (var j = 0; j < arrSupp.length; j++) {
    //                        let supp_id = arrSupp[j].id;
    //                        let supp_type = arrSupp[j].type;
    //                        let supp_bill_no = arrSupp[j].bill_no;
    //                        let supp_bill_dt = arrSupp[j].bill_dt;
    //                        let arrDetail;// = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank")));
    //                        //let arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && (v.type == "OC" || v.type == "Itm" || v.type == "Tax" || v.type == "RCM")));
    //                        let arrDetailDr;
    //                        if (supp_type == "TSupp") {
    //                            arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "TSupp")));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type == "Tds"));
    //                        } else {
    //                            arrDetail = arr.Table.filter(v => (v.id == supp_id && (v.type == "Supp" || v.type == "Bank") && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == supp_id && v.type != "OcTax" && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                        }


    //                        let RcmValue = 0;
    //                        let RcmValueInBase = 0;
    //                        if (supp_type != "TSupp") {
    //                            arr.Table.filter(v => (v.parent == supp_id && v.type == "RCM"))
    //                                .map((res) => {
    //                                    RcmValue = RcmValue + res.Value;
    //                                    RcmValueInBase = RcmValueInBase + res.ValueInBase;
    //                                });
    //                        }

    //                        arrDetail[0].Value = arrDetail[0].Value - RcmValue;
    //                        arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

    //                        let rowSpan = 1;//arrDetailDr.length + 1;
    //                        let GlRowNo = 1;
    //                        // First Row Generated here for all GL Voucher 
    //                        let vouType = "";
    //                        if (arrDetail[0].type == "TSupp")
    //                            vouType = "DN"
    //                        if (arrDetail[0].type == "Supp")
    //                            vouType = "PV"
    //                        if (arrDetail[0].type == "Bank")
    //                            vouType = "BP"
    //                        if (vouType == "DN") {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        } else {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        }

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
    //                                if (ArrGlDetailsDr[i].type == "Tds") {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                } else {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                }

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
function RefreshBillNoBillDateInGLDetail() {
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
        }
        else {
            if (vou_sr_no == change_vou_sr_no) {
                let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
                row.find("#gl_narr span").text(narr);
            }
        }
    });
}
async function CalculateVoucherTotalAmount() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
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
    debugger;
    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        //debugger;
        await AddRoundOffGL();
    }

}
async function AddRoundOffGL() {
    //debugger;
    var ValDecDigit = $("#ValDigit").text();///Amount
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            //debugger;
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                //debugger;
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    $("#VoucherDetail >tbody >tr").each(function () {
                        ////debugger;
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
                    //debugger;
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
                                        , Diff, DiffInBase, 0, 0, "PV", $("#curr_id").val(), $("#conv_rate").val()
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
                                        , $("#curr_id").val(), $("#conv_rate").val(), $("#Bill_No").val()
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
                    //debugger;
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
//async function AddRoundOffGL() {
//    //debugger;
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
//            //debugger;
//            if (data == 'ErrorPage') {
//                //SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                //debugger;
//                if (arr.Table.length > 0) {
//                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    $("#VoucherDetail >tbody >tr").each(function () {
//                        ////debugger;
//                        var currentRow = $(this);
//                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//                        }
//                    });
//                    //debugger;
//                    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//                        if (parseFloat(DrTotAmt) < parseFloat(CrTotAmt)) {
//                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                            </tr>`);
//                                }
//                            }
//                        }
//                        else {
//                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                            </tr>`);
//                                }
//                            }
//                        }
//                    }
//                    //debugger;
//                    CalculateVoucherTotalAmount();
//                }
//            }
//        }
//    });
//}
//async function CalculateVoucherTotalAmount() {
//    //debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        ////debugger;
//        var currentRow = $(this);
//        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//        }
//    });
//    //debugger;
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    //debugger;
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//        //debugger;
//        await AddRoundOffGL();
//    }
//}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Nurration").val() : VouType == "BP" ? $("#hdn_BP_Nurration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
/***---------------------------GL Voucher Entry End-----------------------------***/

/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/


/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e) {
    const row = $(e.target).closest("tr");
    CC_Clicked_Row = row;
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId);

}
function OnClickTP_TDS_SaveAndExit() {
    //debugger

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
    var ToTdsAmt = 0;

    ToTdsAmt = parseFloat(CheckNullNumber(CC_Clicked_Row.find("#OCAmount").text()));
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
                </tr>`);
        });

    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
    //GetAllGLID();
}
function SetTds_Amt_To_OC() {
    var TotalAMount = parseFloat(0);
    var DecDigit = $("#ValDigit").text();
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
    });

    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "TxtItemName_", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
/***------------------------------TDS On Third Party End------------------------------***/
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
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        GetAllGLID();
    }

}

/*----------------------------TDS Section End-----------------------------*/
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
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/ConsumableInvoice/SendEmailAlert", filepath)
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
        var pdfAlertEmailFilePath = 'CI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ConsumableInvoice/SavePdfDocToSendOnEmailAlert_Ext",
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

