

$(document).ready(function () {
    debugger;

    /* for Hide Convrate list Display */
    $("#ConvRate").attr("autocomplete", "off");
    $("#hdnEntitytype").val(1);
    /*----------------------------*/
    CNNo = $("#CreditNoteNo").val().trim();
    $("#hdDoc_No").val(CNNo);

    BindEntityList();
    BindDDLAccountList();
    $("#Entity_Type").on("change", function () {
        debugger;
        BindEntityList();
        onchangeEntityType("EnableBillD");//"EnableBillD" Added by Suraj on 31-07-2024 
        /*----This validation for if EntityType will chnaged then Account table validation should be remove----*/
        validofRemoveAcountSecOnChngHeadSec();

        /*for remove values and required validation  of Enity name and convRate on change of Entity Type*/
        $("#vmEntityName").css("display", "none");
        $("[aria-labelledby='select2-ddlEntityName-container']").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#ConvRate").val("");
        $("#curr").val("");
        $("#curr").append('<option value="0">---Select---</option>')

        /*For remove Bind Earliear Value from EntityName DDL on chane of EntityType*/
        $("#ddlEntityName").empty();
        $("#ddlEntityName").append('<option value="0">---Select---</option>')
        //var EntityType = $("#Entity_Type").val();
        //if (EntityType == 1) {
        //    $("#CreditAmountInSpecific").attr("disabled", false);
        //    $("#DebitAmountInSpecific").attr("disabled", true);

        // }
        //else {
        //    $("#DebitAmountInSpecific").attr("disabled", false);
        //    $("#CreditAmountInSpecific").attr("disabled", true);
        //}
        $("#hdnEntitytype").val($("#Entity_Type").val());


    })
    $('#CNdetailTbl').on('click', '.deleteIcon', function () {
        var txtDisable = $("#txtdisable").val();
        if (txtDisable != "Y") {
            var hfAccID = $(this).closest('tr').find('#hfAccID').val();
            var child = $(this).closest('tr').nextAll();
            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
            }
            child.each(function () {
                var id = $(this).attr('id');
                var idx = $(this).children('.row-index').children('p');
                var dig = parseInt(id.substring(1));
                idx.html(`Row ${dig - 1}`);
                $(this).attr('id', `R${dig - 1}`);
            });
            $(this).closest('tr').remove();
            CalculateTotalDrCrValue();
            updateItemSerialNumber();
        }
    });
    GetViewDetails();
    onchangeEntityType();
    CancelledRemarks("#Cancelled", "Disabled");
});
function onchangeEntityType(flag) {
    if ($("#Entity_Type").val() == "1") {
        if (flag == "EnableBillD") {
            $("#BillNo").val("").attr("readonly", true);
            $("#BillDate").val("").attr("readonly", true);
        }
        $("#star_bill_no").css("display", "none");
        $("#star_bill_dt").css("display", "none");
        $("#div_bill_no").css("display", "none");
        $("#div_bill_dt").css("display", "none");
    } else {
        if (flag == "EnableBillD") {
            $("#BillNo").attr("readonly", false);
            $("#BillDate").attr("readonly", false);
        }
        $("#star_bill_no").css("display", "");
        $("#star_bill_dt").css("display", "");
        $("#div_bill_no").css("display", "");
        $("#div_bill_dt").css("display", "");
    }
}
function deleteCCdetail(hfAccID) {
    debugger;
    if ($("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").length > 0) {
        $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").closest('tr').remove();
    }

}
/*fOR Header Section*/
function BindEntityList() {
    debugger;
    /*for bill adjustment to show or hide in account table a/c to customer and supplier*/
    var Entitytype = $("#Entity_Type").val();
    var flag = " ----Select----";
    if (Entitytype == "2") {
        $("#CNdetailTbl>tbody>tr> #tdCR_BillAdjustment").css("display", "none")
        $("#CNdetailTbl>thead>tr> #thCR_BillAdjustment").css("display", "none")
    }
    else {
        $("#CNdetailTbl>tbody>tr>#tdCR_BillAdjustment").css("display", "")
        $("#CNdetailTbl>thead>tr>#thCR_BillAdjustment").css("display", "")
    }



    /*Dropdown Bind*/
    $("#ddlEntityName").select2({
        ajax: {
            url: $("#hdEntityList").val(),
            data: function (params) {
                var queryParameters = {
                    flag: flag,
                    EntityName: params.term, // search term like "a" then "an"
                    Entitytype: Entitytype
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
}

function GetViewDetails() {
    debugger;
    sessionStorage.removeItem("BillDetailSession");
    if ($("#hdBillWiseAdjDetailList").val() != null && $("#hdBillWiseAdjDetailList").val() != "") {
        debugger
        var arr2 = $("#hdBillWiseAdjDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdBillWiseAdjDetailList").val("");
        sessionStorage.removeItem("BillDetailSession");
        sessionStorage.setItem("BillDetailSession", JSON.stringify(arr));
    }
}
function OnChangeEntityCoa() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();

    var coa_id = $("#ddlEntityName").val();
    if (coa_id == "" || coa_id == "0" || coa_id == null) {
        $("[aria-labelledby='select2-ddlEntityName-container']").css("border-color", "red");
        $("#vmEntityName").text($("#valueReq").text());
        $("#vmEntityName").css("display", "block");
        $("#ddlEntityName").css("border-color", "red");
        /*For Convr rate*/
        $("#ConvRateError").text($("#valueReq").text());
        $("#ConvRateError").css("display", "block");
        $("#ConvRate").css("border-color", "red");
        $("#ConvRate").val("");
        $("#ManConv").css("display", "");
        /*For Currency*/
        $("#curr").val("");
        $("#curr").append('<option value="0">---Select---</option>')
        /*----This validation for if EntityName has select option then Account table validation should be remove----*/
        validofRemoveAcountSecOnChngHeadSec();
    }
    else {
        $("#vmEntityName").css("display", "none");
        $("[aria-labelledby='select2-ddlEntityName-container']").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");
    }

    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/CreditNote/GetAccCurr",
            data: {
                acc_id: coa_id,
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
                    if (arr.Table.length > 0) {
                        $("#curr").val(arr.Table[0].curr_id);
                        $("#hdcurr").val(arr.Table[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));/****/
                        //$("#hdconv").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                        $("#ConvRate").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));
                        $("#hdEntityAccId").val(arr.Table3[0].acc_id);
                    }
                    else if (arr.Table1.length > 0) {
                        $("#curr").val(arr.Table1[0].curr_id);
                        $("#hdcurr").val(arr.Table1[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));

                        $("#ConvRate").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#ConvRate").attr("readonly", true);
                        $("#hdEntityAccId").val(arr.Table3[0].acc_id);
                    }
                    else {
                        //$("#curr").val(arr.Table1[0].curr_id);
                        //$("#hdcurr").val(arr.Table1[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table1[0].conv_rate).toFixed(RateDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table1[0].conv_rate).toFixed(RateDecDigit));
                        $("#ConvRate").attr("readonly", true);
                    }
                    if (arr.Table.length > 0) {
                        if (arr.Table[0].curr_id == arr.Table1[0].bs_curr_id) {
                            $("#ConvRate").attr("readonly", true);
                            $("#ManConv").css("display", "none");
                        }
                        else {
                            $("#ManConv").css("display", "");
                            $("#ConvRate").attr("readonly", false);
                        }
                    }

                }
            },
        });
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    return true;
}
function OnChangeConvRate(e) {
    debugger;

    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();

    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);
    if (AvoidDot(ConvRate) == false) {
        ConvRate = 0;
    }
    if (ConvRate == "" || ConvRate == null || parseFloat(ConvRate) == parseFloat("0")) {
        $("#ConvRateError").text($("#valueReq").text());
        $("#ConvRateError").css("display", "block");
        $("#ConvRate").css("border-color", "red");
        $("#ConvRate").val("");
    }
    else {
        //  var ConvRate3 = parseFloat(ConvRate).toFixed(ExchDecDigit);
        var ConvRate3 = toDecimal(cmn_ReplaceCommas(ConvRate), ExchDecDigit);
        var ConvRate2 = cmn_addCommas(ConvRate3);
        $("#ConvRate").val(ConvRate2);
        //$("#ConvRate").val(parseFloat(ConvRate).toFixed(RateDecDigit));
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
    }

    $("#CNdetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        var debitAmt1 = currentrow.find("#DebitAmountInSpecific").val();
        var debitAmt = cmn_ReplaceCommas(debitAmt1);
        var creditAmt1 = currentrow.find("#CreditAmountInSpecific").val();
        var creditAmt = cmn_ReplaceCommas(creditAmt1);
        if (AvoidDot(debitAmt) == false) {
            debitAmt = 0;
        }
        if (AvoidDot(creditAmt) == false) {
            creditAmt = 0;
        }
        if ((creditAmt !== 0 || creditAmt !== null || creditAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            var FAmt = creditAmt * ConvRate;
            //  var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#CreditAmountInBase").val(FinVal);

        }
        if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            var FAmt = debitAmt * ConvRate;
            //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#DebitAmountInBase").val(FinVal);

        }
    })
    /*Add by Hina on 30-10-2023 to show debit credit  total amount should be base amount instead of specific amount*/
    CalculateTotalDrCrValue();
}

function AddEntityDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount 
    /*To stop untill when each validation complete of header detail */
    if (HeaderValidation() == false) {
        return false;
    }
    var VouDate = $("#CNDate").val();/*add by Hina Sharma on 27-08-2025*/
    var ConvRate = $("#ConvRate").val();
    /*ddlBind*/
    if ($('#ddlEntityName').val() != "0" && $('#ddlEntityName').val() != "" && ConvRate != "" && ConvRate != null) {
        $("#Plusbankdt").css('display', 'none');
        $("#BtnAddGL").css('display', 'block');
        debugger;
        $("#ddlEntityName").prop("disabled", true);
        $("#Entity_Type").prop("disabled", true);
        $("#DocumentNumber").attr("disabled", true);
        $("#CnHeaderNarration").attr("disabled", true);
        var EntityID = $("#ddlEntityName").val();
        var EntitytypeID = $("#Entity_Type").val();
        var CC_Acc_id = EntityID.substring(0, 1);
        if (CC_Acc_id == "3" || CC_Acc_id == "4") {
            $("#BtnCostCenterDetail").attr("disabled", false);
        }
        else {
            $("#BtnCostCenterDetail").attr("disabled", true);
        }


        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/CreditNote/GetEntityIDDetail",
                data: {
                    EntityID: EntityID,
                    VouDate: VouDate
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var a = 0;
                                $("#CNdetailTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    debugger
                                    if (a < arr.Table.length) {
                                        var SNo = currentRow.find("#SNohf").val();
                                        currentRow.find("#GLAccount_" + SNo).empty().append('<option value=' + arr.Table[a].acc_id + ' selected="selected">' + arr.Table[a].acc_name + '</option>');
                                        //currentRow.find("#GLAccount_" + SNo).val(arr.Table[a].acc_name);
                                        currentRow.find("#RowWiseAccBal").val(arr.Table1[a].Avl_bal);/*add AccBal by Hina On 27-08-2025 */

                                        currentRow.find("#GLGroup").val(arr.Table[a].acc_group_name);
                                        currentRow.find("#hfAccID").val(arr.Table[a].acc_id);
                                        currentRow.find("#hdAccType").val(arr.Table[a].acc_type);

                                        //if (EntitytypeID == 1) {
                                        //    currentRow.find("#DebitAmountInSpecific").attr("disabled", true);
                                        //    currentRow.find("#DebitAmountInBase").attr("disabled", true);
                                        //    currentRow.find("#CreditAmountInSpecific").attr("disabled", false);
                                        //    currentRow.find("#CreditAmountInBase").attr("disabled", true);
                                        //}
                                        //else {
                                        //    currentRow.find("#DebitAmountInSpecific").attr("disabled", false);
                                        //    currentRow.find("#DebitAmountInBase").attr("disabled", true);
                                        //    currentRow.find("#CreditAmountInSpecific").attr("disabled", true);
                                        //    currentRow.find("#CreditAmountInBase").attr("disabled", true);
                                        //}
                                        if (arr.Table[i].dr_amt_sp == "0") {
                                            currentRow.find("#DebitAmountInSpecific").val("");
                                        }
                                        else {
                                            //currentRow.find("#DebitAmountInSpecific").val(parseFloat(arr.Table[i].dr_amt_sp).toFixed(ValDecDigit));
                                            currentRow.find("#DebitAmountInSpecific").val(toDecimal(arr.Table[i].dr_amt_sp, ValDecDigit));
                                        }
                                        if (arr.Table[i].dr_amt_bs == "0") {
                                            currentRow.find("#DebitAmountInBase").val("");
                                        }
                                        else {
                                            //currentRow.find("#DebitAmountInBase").val(parseFloat(arr.Table[i].dr_amt_bs).toFixed(ValDecDigit));
                                            currentRow.find("#DebitAmountInBase").val(toDecimal(arr.Table[i].dr_amt_bs, ValDecDigit));
                                        }
                                        if (arr.Table[i].cr_amt_sp == "0") {
                                            currentRow.find("#CreditAmountInSpecific").val("");
                                        }
                                        else {
                                            //  currentRow.find("#CreditAmountInSpecific").val(parseFloat(arr.Table[i].cr_amt_sp).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInSpecific").val(toDecimal(arr.Table[i].cr_amt_sp, ValDecDigit));
                                        }
                                        if (arr.Table[i].cr_amt_bs == "0") {
                                            currentRow.find("#CreditAmountInBase").val("");
                                        }
                                        else {
                                            //currentRow.find("#CreditAmountInBase").val(parseFloat(arr.Table[i].cr_amt_bs).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInBase").val(toDecimal(arr.Table[i].cr_amt_bs, ValDecDigit));
                                        }
                                        currentRow.find("#Narration").val($("#CnHeaderNarration").val());
                                        if ($("#Entity_Type").val() == "1") {
                                            currentRow.find("#BillAdjustment").attr("disabled", false);
                                        } else {
                                            currentRow.find("#BillAdjustment").attr("disabled", true);
                                        }
                                        
                                    }
                                    a = a + 1;
                                });
                            }

                        }
                    }
                    NarratValidation();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    }
}
function HeaderValidation() {
    debugger;
    var ErrorFlag = "N";
    var voudate = $("#CNDate").val();/*Add by Hina on 13-11-2024 to validate voucher date on save */
    if (voudate == "") {
        $('#SpanVouDateErrorMsg').text($("#valueReq").text());
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#CNDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#CNDate").css("border-color", "#ced4da");
    }
    var entity_id = $("#ddlEntityName").val();
    if (entity_id == "" || entity_id == "0" || entity_id == null) {
        $("#vmEntityName").css("display", "block");
        $("[aria-labelledby='select2-ddlEntityName-container']").css("border-color", "red");
        $("#vmEntityName").text($("#valueReq").text());

        ErrorFlag = "Y";
    }
    else {
        $("#vmEntityName").css("display", "none");
        $("#ddlEntityName").css("border-color", "#ced4da");
    }
    var ConvRate = $("#ConvRate").val();
    if (ConvRate == "" || ConvRate == "0") {
        document.getElementById("ConvRateError").innerHTML = $("#valueReq").text();
        $("#ConvRateError").css("display", "block");
        $("#ConvRate").css("border-color", "red");
        $("#ManConv").css("display", "");
        ErrorFlag = "Y";
    }
    else {
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#ManConv").css("display", "none");
    }
    var Currency = $("#curr").val();
    if (Currency == "" || Currency == "0") {
        document.getElementById("vmCurr").innerHTML = $("#valueReq").text();
        $("#vmCurr").css("display", "block");
        $("#curr").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");

    }
    //Added by Suraj on 24-04-2024 for validate bill no and bill date
    let EntityType = $("#Entity_Type").val();
    if (EntityType == "2") {
        if (CheckVallidation("BillNo", "BillNoError", "0") == false) {
            ErrorFlag = "Y";
        }
        if (CheckVallidation("BillDate", "BillDateError") == false) {
            ErrorFlag = "Y";
        }
    }
   
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}
/*fOR Accoount Section*/
function AccountValidation() {
    debugger
    var RateDecDigit = $("#RateDigit").text()
    var ValDecDigit = $("#ValDigit").text();///Amount 
    var DocId = $("#DocumentMenuId").val();
    var EntityType = $("#Entity_Type").val();
    var ErrorFlag = "N";
    if ($("#CNdetailTbl tbody tr").length > 0) {
        if (DocId == '105104115135') {
            $("#CNdetailTbl tbody tr").each(function () {
                debugger
                var currentrow = $(this);
                var Rowno = currentrow.find("#SNohf").val();

                if (Rowno == 1) {
                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if (AccName == "0") {
                        swal("", $("#NoGLAccountSelected").text(), "warning");
                        ErrorFlag = "Y";
                    }
                    if (ErrorFlag == "Y") {
                        return false;
                    }

                    debugger;
                    //if (EntityType == "2") {
                    //    if (DbtAmnt == "" || DbtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
                    //        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                    //        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                    //        currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                    //        ErrorFlag = "Y";
                    //    }
                    //    else {
                    //        currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                    //        currentrow.find("#Dbt_Amnt_Error").text("");
                    //        currentrow.find("#Dbt_Amnt_Error").css("display", "none");
                    //    }

                    //}
                    //else {

                    if (CrdtAmnt == "" || CrdtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                        currentrow.find("#Crdt_Amnt_Error").text("");
                        currentrow.find("#Crdt_Amnt_Error").css("display", "none");
                    }
                    //}

                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }

                }
                if (Rowno > 1) {
                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if (AccName == "0") {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "red");
                        currentrow.find("#AccountNameError").text($("#valueReq").text());
                        currentrow.find("#AccountNameError").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
                        currentrow.find("#AccountNameError").text("");
                        currentrow.find("#AccountNameError").css("display", "none");
                    }
                    if ((DbtAmnt == "" || parseFloat(DbtAmnt) == parseFloat(0)) && (CrdtAmnt == "" || parseFloat(CrdtAmnt) == parseFloat(0))) {

                        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        ErrorFlag = "Y";
                    }
                }

                if (Narrat == "") {
                    currentrow.find("#Narration").css("border-color", "red");
                    currentrow.find("#Narr_Error").text($("#valueReq").text());
                    currentrow.find("#Narr_Error").css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    currentrow.find("#Narration").css("border-color", "#ced4da");
                    currentrow.find("#Narr_Error").text("");
                    currentrow.find("#Narr_Error").css("display", "none");
                }
            });
        }

    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnchangeNarr(e) {

    var currentrow = $(e.target).closest('tr');
    var Narrat = currentrow.find("#Narration").val();
    if (Narrat == "") {
        currentrow.find("#Narration").css("border-color", "red");
        currentrow.find("#Narr_Error").text($("#valueReq").text());
        currentrow.find("#Narr_Error").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        currentrow.find("#Narration").css("border-color", "#ced4da");
        currentrow.find("#Narr_Error").text("");
        currentrow.find("#Narr_Error").css("display", "none");
    }
}
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var rowCount = $('#CNdetailTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#CNdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    if (DocId == '105104115135') {
        $('#CNdetailTbl tbody').append(`<tr id="${++rowIdx}">
 <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>   
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form" style="padding:0px;">
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input type="hidden" id="hfAccID" style="display: none;" />
<input type="hidden" id="hdAccType" style="display: none;" />
<span id="AccountNameError" class="error-message is-visible"></span></div>
</td>

<td>

<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','CNDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"> </button></div>

</td>



<td><textarea class="form-control remarksmessage" cols="20" id="GLGroup" maxlength="100" autocomplete="off" type="text" name="GLGroup" value="" onmouseover="OnMouseOver(this)" placeholder="${$("#AccGroup").text()}" rows="2"  readonly></textarea>
 <input id="hdGLGroupID" type="hidden" /></td>
<input id="hdnAccGrpval3_4"  type="hidden" />
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInSpecific"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Dbt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInBase" class="form-control num_right" onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"  disabled>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInSpecific"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Crdt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInBase" class="form-control num_right" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00" disabled>
 </div>
</td>
<td>
<div class="lpo_form">
    <textarea id="Narration" class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}">${$("#CnHeaderNarration").val()}</textarea>
       <span id="Narr_Error" class="error-message is-visible"></span>
 </div>
</td>
  <td class="center" id="tdCR_BillAdjustment">
          <button type="button" disabled id="BillAdjustment" onclick="OnClickBillAdjIconBtn(event)" class="calculator subItmImg" data-toggle="modal" data-target="#BillAdjustment" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BillAdjustment").text()}"></i></button>
 </td>
<td class="center">
      <button type="button" disabled id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
 </td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
        /*commented and modify above by Hina on 27-08-2025 to verify condtion*/
        //<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form no-padding">
        //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
        //    <input type="hidden" id="hfAccID" style="display: none;" />
        //    <input type="hidden" id="hdAccType" style="display: none;" />
        //    <span id="AccountNameError" class="error-message is-visible"></span></div>
        //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','CNDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
        //</td>
    }
    else {
        $('#CNdetailTbl tbody').append(`<tr id="${++rowIdx}">
 <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>   
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form" style="padding:0px;">
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input type="hidden" id="hfAccID" style="display: none;" />
<input type="hidden" id="hdAccType" style="display: none;" />
<span id="AccountNameError" class="error-message is-visible"></span> </div>
</td>

<td>

<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','CNDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"> </button></div>

</td>

<td><textarea id="GLGroup" class="form-control remarksmessage" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="GLGroup" placeholder="${$("#AccGroup").text()}" readonly></textarea>
 <input id="hdGLGroupID" type="hidden" /></td>
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInSpecific"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Dbt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInBase" class="form-control num_right" onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"  disabled>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInSpecific"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Crdt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInBase" class="form-control num_right" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00" disabled>
 </div>
</td>
<td>
<div class="lpo_form">
    <textarea id="Narration"  class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}">${$("#CnHeaderNarration").val()}</textarea>
       <span id="Narr_Error" class="error-message is-visible"></span>
 </div>
</td>
  <td class="center" id="tdCR_BillAdjustment">
          <button type="button" disabled id="BillAdjustment" onclick="OnClickBillAdjIconBtn(event)" class="calculator subItmImg" data-toggle="modal" data-target="#BillAdjustment" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BillAdjustment").text()}"></i></button>
 </td>
<td class="center">
      <button type="button" id="CostCenterDetail" onclick="" class="calculator " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
 </td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

        /*commented and modify above by Hina on 27-08-2025 to verify condtion*/
        //<td><div class="col-sm-11 lpo_form no-padding">
        //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
        //    <input type="hidden" id="hfAccID" style="display: none;" />
        //    <input type="hidden" id="hdAccType" style="display: none;" />
        //    <span id="AccountNameError" class="error-message is-visible"></span></div>
        //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','CNDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
        //</td>
    }
    /*for bill adjustment to show or hide in account table a/c to customer and supplier*/
    var Entitytype = $("#Entity_Type").val();
    if (Entitytype == "2") {
        $("#CNdetailTbl>tbody>tr>#tdCR_BillAdjustment").css("display", "none")
        $("#CNdetailTbl>thead>tr>#thCR_BillAdjustment").css("display", "none")
    }
    else {
        $("#CNdetailTbl>tbody>tr>#tdCR_BillAdjustment").css("display", "")
        $("#CNdetailTbl>thead>tr>#thCR_BillAdjustment").css("display", "")
    }

    BindAcountList(RowNo);
}

function NarratValidation() {
    debugger
    var DocId = $("#DocumentMenuId").val();
    if ($("#CNdetailTbl tbody tr").length > 0) {
        if (DocId == '105104115135') {
            $("#CNdetailTbl tbody tr").each(function () {
                debugger
                var currentrow = $(this);
                var Rowno = currentrow.find("#SNohf").val();

                if (Rowno == 1) {
                    currentrow.find("#Narration").css("border-color", "#ced4da");
                    currentrow.find("#Narr_Error").text("");
                    currentrow.find("#Narr_Error").css("display", "none");
                }
            });
        }

    }
}
/*For Bind GL Account in second row*/
function BindAcountList(ID) {
    debugger;
    /*Give documnet id to bind others accout in second row*/
    /*go to commonjs and search this function to  give document id in procedure for bind GL account of add othr type a/c in second row*/
    Cmn_BindAccountList("#GLAccount_", ID, "#CNdetailTbl", "#SNohf", "", "105104115135");

}
//------Bind Group on change of Account Dropdown-------//
function OnChangeAccountName(RowID, e) {
    debugger;
    BindJornlVoucAccountList(e);
}
function BindJornlVoucAccountList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Acc_ID = clickedrow.find("#GLAccount_" + SNo).val();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    if (hdn_acc_id != "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').remove();
        }
    }

    clickedrow.find("#hfAccID").val(Acc_ID);
    if (Acc_ID != "0") {
        clickedrow.find("#DebitAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#CreditAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#DebitAmountInBase").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#CreditAmountInBase").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#DebitAmountInSpecific").attr("disabled", false);
        clickedrow.find("#CreditAmountInSpecific").attr("disabled", false);
    }

    if (Acc_ID == "0") {
        clickedrow.find("#AccountNameError").text($("#valueReq").text());
        clickedrow.find("#AccountNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#AccountNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }
    Cmn_BindGroup(clickedrow, Acc_ID, "");
    /*-----------Code Start Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
    var VouDate = $("#CNDate").val();
    CMN_BindAccountBalance(clickedrow, Acc_ID, VouDate);
    /*-----------Code End Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
    CalculateTotalDrCrValue();
}
/*for after Delete Trash Bin to set serial number automatic*/
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#CNdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};

function AmountFloatVal(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
}
function OnChangeDebitAmount(e) {
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var debitAmt12 = currentrow.find("#DebitAmountInSpecific").val();
    var debitAmt = cmn_ReplaceCommas(debitAmt12);
    if (DocId == '105104115135') {
        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            currentrow.find("#DebitAmountInSpecific").val("")
        }
        else {
            // var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit);
            var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDecDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            currentrow.find("#DebitAmountInSpecific").val(debitAmt1)
            //currentrow.find("#DebitAmountInSpecific").val(parseFloat(debitAmt).toFixed(ValDecDigit))
        }
        var Rowno = currentrow.find("#SNohf").val();
        if (Rowno == 1) {
            if (debitAmt != "") {
                currentrow.find("#CreditAmountInSpecific").attr("disabled", true);
                currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error").text("");
                currentrow.find("#Crdt_Amnt_Error").css("display", "none");
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");

            }
        }
        else {
            if (debitAmt != "") {
                currentrow.find("#CreditAmountInSpecific").attr("disabled", true);
                currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error").text("");
                currentrow.find("#Crdt_Amnt_Error").css("display", "none");
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");

            }
            else {
                currentrow.find("#CreditAmountInSpecific").attr("disabled", false);
                currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error").text("");
                currentrow.find("#Crdt_Amnt_Error").css("display", "none");
            }
        }
    }

    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);

    if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        var FAmt = debitAmt * ConvRate;
        //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        currentrow.find("#DebitAmountInBase").val(FinVal);

    }
    CalculateTotalDrCrValue();
}
function OnChangeCreditAmount(e) {
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var creditAmt1 = currentrow.find("#CreditAmountInSpecific").val();
    var creditAmt = cmn_ReplaceCommas(creditAmt1);
    if (AvoidDot(creditAmt) == false) {
        creditAmt = "";
        currentrow.find("#CreditAmountInSpecific").val("")
    }
    else {
        // var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
        var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDecDigit);
        var creditAmt3 = cmn_addCommas(creditAmt2);
        currentrow.find("#CreditAmountInSpecific").val(creditAmt3);
        //currentrow.find("#CreditAmountInSpecific").val(parseFloat(creditAmt).toFixed(ValDecDigit))
    }
    var Rowno = currentrow.find("#SNohf").val();
    if (Rowno == 1) {
        if (creditAmt != "") {
            currentrow.find("#DebitAmountInSpecific").attr("disabled", true);
            currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("");
            currentrow.find("#Dbt_Amnt_Error").css("display", "none");
            currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("");
            currentrow.find("#Crdt_Amnt_Error").css("display", "none");
        }
    }
    else {
        if (creditAmt != "") {
            currentrow.find("#DebitAmountInSpecific").attr("disabled", true);
            currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("");
            currentrow.find("#Dbt_Amnt_Error").css("display", "none");
            currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("");
            currentrow.find("#Crdt_Amnt_Error").css("display", "none");
        }
        else {
            currentrow.find("#DebitAmountInSpecific").attr("disabled", false);

        }
    }
    //}
    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);

    if ((creditAmt !== 0 || creditAmt !== null || creditAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        var FAmt = creditAmt * ConvRate;
        //  var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        currentrow.find("#CreditAmountInBase").val(FinVal);
    }
    CalculateTotalDrCrValue();
}
function OnKeyupDebtAmnt(e) {

    var currentrow = $(e.target).closest('tr');
    var debitAmt = currentrow.find("#DebitAmountInSpecific").val();
    if (debitAmt != "") {
        currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
        currentrow.find("#Dbt_Amnt_Error").text("");
        currentrow.find("#Dbt_Amnt_Error").css("display", "none");
    }
}
function OnKeyupCrdtAmnt(e) {

    var currentrow = $(e.target).closest('tr');
    var creditAmt = currentrow.find("#CreditAmountInSpecific").val();
    if (creditAmt != "") {
        currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
        currentrow.find("#Crdt_Amnt_Error").text("");
        currentrow.find("#Crdt_Amnt_Error").css("display", "none");
    }
}
/**** For Data SAve****/
function InsertCNDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    if (HeaderValidation() == false) {
        return false;
    }
    if (AccountValidation() == false) {
        return false;
    }
    if (saveDrCrEqualAmnt() == false) {
        return false;
    }
    if (CheckBillDetailValidation() == false) {
        return false
    }
    if (onclkDtlSaveBtnValidateBillAd() == false) {
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
    var FinalAccountDetail = [];
    FinalAccountDetail = InsertCNAccountDetails();
    var AccountDt = JSON.stringify(FinalAccountDetail);
    $('#hdAccountDetailList').val(AccountDt);
    CalculateTotalDrCrValue();

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    if (Cmn_CC_DtlSaveButtonClick("CNdetailTbl", "DebitAmountInSpecific", "CreditAmountInSpecific","hdCNStatus") == false) {
        return false;
    }

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/

    /*-------------Bill Adjustment -----------*/ //Added by Suraj on 07-05-2024
    var FinalBillWiseAdjDetail = [];
    FinalBillWiseAdjDetail = InsertBillWiseAdjDetails();
    var BillWiseAdjDt = JSON.stringify(FinalBillWiseAdjDetail);
    $('#hdBillWiseAdjDetailList').val(BillWiseAdjDt);
    /*-------------Bill Adjustment End-----------*/

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;

};
function InsertCNAccountDetails() {
    debugger;
    var AccountDetailList = new Array();
    $("#CNdetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var SpanRowId = currentRow.find("#SpanRowId").text();
        var AccountList = {};
        /*commented by Hina on 01-07-2024 to seprate acc_id and SpanRowId as seq_no */
        /*AccountList.acc_id = (currentRow.find("#GLAccount_" + rowid).val()) + '_' + SpanRowId;*/
        AccountList.acc_id = currentRow.find("#GLAccount_" + rowid).val();
        AccountList.acc_name = currentRow.find("#GLAccount_" + rowid + " option:selected").text();
        AccountList.acc_group_name = currentRow.find("#GLGroup").val();
        AccountList.acc_type = currentRow.find("#hdAccType").val();
        AccountList.curr_id = $("#curr").val();
        AccountList.conv_rate = cmn_ReplaceCommas($("#ConvRate").val());
        AccountList.dr_amt_bs = cmn_ReplaceCommas(currentRow.find("#DebitAmountInBase").val());
        AccountList.dr_amt_sp = cmn_ReplaceCommas(currentRow.find("#DebitAmountInSpecific").val());
        AccountList.cr_amt_bs = cmn_ReplaceCommas(currentRow.find("#CreditAmountInBase").val());
        AccountList.cr_amt_sp = cmn_ReplaceCommas(currentRow.find("#CreditAmountInSpecific").val());
        AccountList.narr = currentRow.find("#Narration").val();
        AccountList.seq_no = SpanRowId;
        AccountDetailList.push(AccountList);

    });

    return AccountDetailList;
};
function CalculateTotalDrCrValue() {
    var debitAmount = 0;
    var creditAmount = 0;
    //var GrossValue = 0;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    //Added by Suraj on 10-05-2024 
    Cmn_ControlDecimalDiffVal("CNdetailTbl", "DebitAmountInBase", "DebitAmountInSpecific", "CreditAmountInBase", "CreditAmountInSpecific");
    $("#CNdetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        /*Commented and modify by HIna on 30-10-2023 to changes for Base Amount instead of specific*/
        //var DbtAmnt = currentrow.find("#DebitAmountInSpecific").val();
        //var CrdtAmnt = currentrow.find("#CreditAmountInSpecific").val();
        var DbtAmnt1 = currentrow.find("#DebitAmountInBase").val();
        var DbtAmnt = cmn_ReplaceCommas(DbtAmnt1);
        var CrdtAmnt1 = currentrow.find("#CreditAmountInBase").val();
        var CrdtAmnt = cmn_ReplaceCommas(CrdtAmnt1);
        //var DbtCrtTAmnt = 0;
        if (AvoidDot(DbtAmnt) == false) {
            DbtAmnt = 0;
        }
        if (AvoidDot(CrdtAmnt) == false) {
            CrdtAmnt = 0;
        }
        debitAmount = parseFloat(parseFloat(debitAmount).toFixed(ValDigit)) + parseFloat(parseFloat(DbtAmnt).toFixed(ValDigit));
        creditAmount = parseFloat(parseFloat(creditAmount).toFixed(ValDigit)) + parseFloat(parseFloat(CrdtAmnt).toFixed(ValDigit));
    });
    // var debitAmount2 = parseFloat(debitAmount).toFixed(ValDigit);
    var debitAmount2 = toDecimal(cmn_ReplaceCommas(debitAmount), ValDigit);
    var debitAmount1 = cmn_addCommas(debitAmount2);
    $("#dbtTtlAmnt").text(debitAmount1);
    //$("#dbtTtlAmnt").text(parseFloat(debitAmount).toFixed(ValDigit));

    //  var creditAmount2 = parseFloat(creditAmount).toFixed(ValDigit);
    var creditAmount2 = toDecimal(cmn_ReplaceCommas(creditAmount), ValDigit);
    var creditAmount1 = cmn_addCommas(creditAmount2);
    $("#crdtTtlAmnt").text(creditAmount1);
    //$("#crdtTtlAmnt").text(parseFloat(creditAmount1).toFixed(ValDigit));
    var difference = toDecimal(debitAmount, ValDigit) - toDecimal(creditAmount, ValDigit);
    if (difference > 0) {
        FinalDiff = (difference * 1)
        $("#spanamttype").text("Dr")
    }
    else {
        FinalDiff = (difference * -1)
        $("#spanamttype").text("Cr")
    }
    if (difference == 0) {
        $("#spanamttype").text("")
    }
    // var FinalDiff2 = parseFloat(FinalDiff).toFixed(ValDigit);
    var FinalDiff2 = toDecimal(cmn_ReplaceCommas(FinalDiff), ValDigit);
    var FinalDiff1 = cmn_addCommas(FinalDiff2);
    $("#DiffrncAmnt").text(FinalDiff1);
    //$("#DiffrncAmnt").text(parseFloat(FinalDiff).toFixed(ValDigit));
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function saveDrCrEqualAmnt() {
    debugger;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    var dbTotlAmnt1 = $("#dbtTtlAmnt").text();
    var dbTotlAmnt = cmn_ReplaceCommas(dbTotlAmnt1);
    var crTotAmnt1 = $("#crdtTtlAmnt").text();
    var crTotAmnt = cmn_ReplaceCommas(crTotAmnt1);
    debugger;
    if (dbTotlAmnt == crTotAmnt && dbTotlAmnt != "0" && crTotAmnt != "0") {
        var difference = 0;
        //$("#DiffrncAmnt").text(parseFloat(difference).toFixed(ValDigit));
        $("#DiffrncAmnt").text(toDecimal(difference, ValDigit));
    }
    else {
        swal("", $("#DebtCredtAmntMismatch").text(), "warning");
        ErrorFlag = "Y";
    }
    var vouamnt = $("#dbtTtlAmnt").text();
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
//----Bind Gl Account as Other type account in second row on double click and also call on page load of js ------//
function BindDDLAccountList() {
    debugger;

    Cmn_BindAccountList("#GLAccount_", "1", "#CNdetailTbl", "#SNohf", "BindData", "105104115135");

}
function BindData() {
    debugger;

    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#CNdetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#CNdetailTbl >tbody >tr").length) {
                    return false;
                }
                $("#GLAccount_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#GLAccount_" + rowid).select2({

                    templateResult: function (data) {
                        var selected = $("#GLAccount_" + rowid).val();
                        if (check(data, selected, "#CNdetailTbl", "#SNohf", "#GLAccount_") == true) {
                            /* var UOM = $(data.element).data('uom');*/
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
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
    $("#CNdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        var AccountID = '#GLAccount_' + rowid;
        if (AccID != '0' && AccID != "") {
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "");
            currentRow.find("#GLAccount_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "OnChangeAccountName(this, event)");
        }

    });

}
//-------For Delete button confirmation-------//
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
//-------Cancel and WorkFlow work-------//
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertCNDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}

function ForwardBtnClick() {
    debugger;
    //var CNStatus = "";
    //CNStatus = $('#hdCNStatus').val().trim();
    //if (CNStatus === "D" || CNStatus === "F") {

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

    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var Voudt = $("#CNDate").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/Fin_CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId,
                Voudt: Voudt
            },
            success: function (data) {
                if (data == "FY Exist") { /*End to chk Financial year exist or not*/
                    var CNStatus = "";
                    CNStatus = $('#hdCNStatus').val().trim();
                    if (CNStatus === "D" || CNStatus === "F") {

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
                    if (data == "FY Not Exist") {
                        swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                        //$("#Btn_Forward").attr("data-target", "");
                        $("#Forward_Pop").attr("data-target", "");
                        //$("#Btn_Approve").attr("data-target", "");
                    }
                    else {
                        swal("", $("#BooksAreClosedEntryCanNotBeMadeInThisFinancialYear").text(), "warning");
                        /* $("#Btn_Forward").attr("data-target", "");*/
                        $("#Forward_Pop").attr("data-target", "");
                        //$("#Btn_Approve").attr("data-target", "");
                    }

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
    var VouNo = "";
    var VouDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    VouNo = $("#CreditNoteNo").val();
    VouDate = $("#CNDate").val();
    $("#hdDoc_No").val(VouNo);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (VouNo + ',' + VouDate + ',' + WF_Status1);
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    //var pdfAlertEmailFilePath = 'CN_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath);
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "CreditNote_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/CreditNote/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CreditNote/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/CreditNote/CreditNoteApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CreditNote/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CreditNote/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(vNo, vDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/CreditNote/SavePdfDocToSendOnEmailAlert",
//        data: { vNo: vNo, vDate: vDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#CreditNoteNo").val();
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
function validofRemoveAcountSecOnChngHeadSec() {
    /*------Start Validation of Header fields related to account table------*/
    /*----This validation for if EntityName has select option then Account table validation should be remove----*/
    /*Validation Remove from child section if header section has value required*/
    if ($("#CNdetailTbl tbody tr").length > 0) {
        $("#CNdetailTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();

            currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
            currentrow.find("#AccountNameError").text("");
            currentrow.find("#AccountNameError").css("display", "none");
            var EntityType = $("#Entity_Type").val();
            if (EntityType == "1" || EntityType == "2") {
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");
                currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error").text("");
                currentrow.find("#Crdt_Amnt_Error").css("display", "none");

            }
            currentrow.find("#Narration").css("border-color", "#ced4da");
            currentrow.find("#Narr_Error").text("");
            currentrow.find("#Narr_Error").css("display", "none");
        });
    }
    /*------End Validation of Header fields related to account table------*/
}

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
    var TotalAmt1 = 0;//add by sm 09-12-2024
    var Doc_ID = $("#DocumentMenuId").val();//add by sm 09-12-2024
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt1 = clickedrow.find("#DebitAmountInSpecific").val();
    var CstDbAmt = cmn_ReplaceCommas(CstDbAmt1);
    var CstCrtAmt1 = clickedrow.find("#CreditAmountInSpecific").val();
    var CstCrtAmt = cmn_ReplaceCommas(CstCrtAmt1);
    var GLAcc_Name = clickedrow.find('option:selected').text();
    var GLAcc_id = clickedrow.find('option:selected').val();
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        var List = {};
        List.GlAccount = row.find("#hdntbl_GlAccountId").text();
        List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
        List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
        List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
        List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
        var amount = cmn_ReplaceCommas(row.find("#hdntbl_CstAmt").text());
        //  var amount1 = parseFloat(amount).toFixed(ValDigit);
        var amount1 = toDecimal(cmn_ReplaceCommas(amount), ValDigit);
        TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amount);//add by sm 09-12-2024
        List.CC_Amount = cmn_addCommas(amount1);
        NewArr.push(List);
    })
    var TotalAmt = 0;
    /*Cond Added by Surbhi on 23/06/2025*/
    if (CstDbAmt > 0 && CstCrtAmt== 0) {
        TotalAmt = cmn_addCommas(toDecimal(CstDbAmt, ValDigit)); 
    }
    else if (CstCrtAmt > 0 && CstDbAmt== 0) {
        TotalAmt = cmn_addCommas(toDecimal(CstCrtAmt, ValDigit)); 
    }
    //var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 09-12-2024
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CreditNote/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            TotalAmt: TotalAmt,//add by sm 09-12-2024
            Doc_ID: Doc_ID//add by sm 09-12-2024   

        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(cmn_addCommas(Amt));
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("CNdetailTbl");
        },
    })
}

//-------------------Cost Center Section End-------------------//


function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
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

function BillDateValidation() {
    debugger;
    CheckVallidation("BillDate", "BillDateError");//to check null, blank or 0
    Cmn_VoucherDateValidation("#BillDate");

}
function OnchangeBillNo() {
    CheckVallidation("BillNo", "BillNoError", "0");//to check null, blank or 0
}
function VoucherDateValidation() {/*Add by Hina sharma on 13-11-2024 to validate voucher date on change*/
    debugger;
    var VouDate = $("#CNDate").val();

    if (VouDate == "") {/*code start Add by Hina on 12-11-2024 to validate voucher date */
        $('#SpanVouDateErrorMsg').text($("#valueReq").text());
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#CNDate").css("border-color", "Red");
        return false
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#CNDate").css("border-color", "#ced4da");/*code End Add by Hina on 12-11-2024 to validate voucher date */
        Cmn_VoucherDateValidation("#CNDate");
    }

}

//------------------Bill Adjustmet detail------------------------
function OnClickBillAdjIconBtn(e) {
    debugger;
    //BindDate();/*Commented and modify by Hina on 13-11-2024*/
    var clickedrow = $(e.target).closest("tr");
    var voudate = $("#CNDate").val();/*code start Add by Hina on 13-11-2024 to validate voudate */
    if (voudate == "") {
        $('#SpanVouDateErrorMsg').text($("#valueReq").text());
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#CNDate").css("border-color", "Red");
        clickedrow.find("#BillAdjustment").attr("data-target", "");
        return false;
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#CNDate").css("border-color", "#ced4da");
        clickedrow.find("#BillAdjustment").attr("data-target", "#BillAdjustment");
        Cmn_BindVouDate("", voudate);
    }
    var DocId = $("#DocumentMenuId").val();

    var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var Rowno = clickedrow.find("#SNohf").val();
    var AccID = clickedrow.find("#GLAccount_" + Rowno).val();
    var AccName = clickedrow.find("#GLAccount_" + Rowno + " option:selected").text().trim();
    var DbtAmnt = clickedrow.find("#DebitAmountInSpecific").val();
    var CrdtAmnt = clickedrow.find("#CreditAmountInSpecific").val();
    debugger;
    $("#IconHdEntityID").val(AccID);
    $("#IconEntityName").val(AccName);
    if (DocId == '105104115130') {
        $("#IconPayAmt").val(DbtAmnt);
    }
    else {
        $("#IconPayAmt").val(CrdtAmnt);
    }

    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.setItem("FromDateToDate", "N");

    var Status = $("#hdBPStatus").val();
    BillWiseDetail(AccID);
}
function BillWiseDetail(AccID) {
    debugger;
    
    var ValDecDigit = $("#ValDigit").text();///Amount
    docid = $("#DocumentMenuId").val();
    var DocumentNumber = $("#CreditNoteNo").val();
    var Status = $("#hdCNStatus").val();
    var Entity_Type = $("#Entity_Type").val();
    var flag = '';
    if (Entity_Type == "1") {
        flag = "R";
    }
    //var FromTodate = sessionStorage.getItem("FromDateToDate");/*commented and modify by Hina on 12-11-2024*/
    //if (FromTodate == "Y") {
    //    var fromdt = $("#txtFromdate").val();
    //    var todt = $("#txtTodate").val();
    //}
    //else {
    //    var fromdt = null;
    //    var todt = null;
    //}
    /*start and modify below commented by Hina on 09-06-2025 discus with vishal sir for show data as voucher date should be less then equal to bill dt for BP,CP*/

    //document.getElementById("txtFromdate").setAttribute('max', $("#txtTodate").val());/*start code Add by Hina on 13-11-2024 to change discuss by vishal sir*/
    //document.getElementById("txtTodate").setAttribute('max', $("#txtTodate").val());
    //var fromdt = $("#txtFromdate").val();
    //var todt = $("#txtTodate").val();/*End Code by Hina on 13-11-2024*/
    var VouDt = $("#DNDate").val();
    var todt = $("#txtTodate").val();
    var fromdt = "";
    var Curr = $("#curr").val();
    if (AccID != "" && AccID != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/BankPayment/GetBillDetail",
                data: {
                    AccId: AccID, fromdt: fromdt, todt: todt, flag: flag, DocumentNumber: DocumentNumber, Status: Status, Curr: Curr
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#BillDetailTbl tbody tr').remove();
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;

                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var inv_no = arr.Table[i].inv_no;
                                var inv_dt = arr.Table[i].inv_dt;
                                var acc_id = arr.Table[i].acc_id;

                                var Pay_Amount = 0;
                                var Rem_Bal = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt), ValDecDigit);
                                var Pend_Amount = 0;

                                var FBillDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
                                if (FBillDetails != null && FBillDetails != "") {
                                    if (FBillDetails.length > 0) {
                                        for (j = 0; j < FBillDetails.length; j++) {
                                            debugger;
                                            var InvNo = FBillDetails[j].InvoiceNo;
                                            var InvDt = FBillDetails[j].InvoiceDate;
                                            var SSAccID = FBillDetails[j].AccID;
                                            //var PayAmount = FBillDetails[j].PayAmount;
                                            //var RemBal = cmn_ReplaceCommas(FBillDetails[j].RemBal);
                                            //var PendAmount = FBillDetails[j].PendAmount;
                                            var PayAmount = parseFloat(cmn_ReplaceCommas(FBillDetails[j].PayAmount));
                                            var RemBal = parseFloat(cmn_ReplaceCommas(FBillDetails[j].RemBal));
                                            var PendAmount = parseFloat(cmn_ReplaceCommas(FBillDetails[j].PendAmount));
                                            if (SSAccID == acc_id && InvNo == inv_no && InvDt == inv_dt) {
                                                if ((PayAmount == "") || (PayAmount == null) || (PayAmount == 0) || (PayAmount == "NaN")) {
                                                    Pay_Amount = 0;
                                                }
                                                else {
                                                    Pay_Amount = PayAmount;
                                                }
                                                if ((RemBal == "") || (RemBal == null) || (RemBal == 0) || (RemBal == "NaN")) {
                                                    Rem_Bal = 0;
                                                }
                                                else {
                                                    Rem_Bal = RemBal;
                                                }
                                                if ((PendAmount == "") || (PendAmount == null) || (PendAmount == 0) || (PendAmount == "NaN")) {
                                                    Pend_Amount = 0;
                                                }
                                                else {
                                                    Pend_Amount = PendAmount;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (AvoidDot(Pay_Amount) == false) {
                                    Pay_Amount = 0;
                                }
                                if (AvoidDot(Pend_Amount) == false) {
                                    Pend_Amount = 0;
                                }
                                if (AvoidDot(Rem_Bal) == false) {
                                    Rem_Bal = 0;
                                }
                                var UnAdjustedAmount = parseFloat(cmn_ReplaceCommas(Pay_Amount)) + parseFloat(cmn_ReplaceCommas(Rem_Bal));
                                //var UnAdjustedAmount = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt), ValDecDigit);
                                //if ($("#hdCNStatus").val() == "A") {
                                //    UnAdjustedAmount = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt) - (- cmn_ReplaceCommas(Pay_Amount)), ValDecDigit);
                                //}
                                var td_billNoAndDate = '';
                                if (flag == "P") {
                                    td_billNoAndDate = `<td><input id="BillNo" class="form-control" autocomplete="off" type="text" name="InvoiceDate" disabled value="${arr.Table[i].bill_no}"></td>
                        <td><input id="BillDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].bill_dt}"></td>
                        `
                                }

                                ++rowIdx;
                                $('#BillDetailTbl tbody').append(` <tr id="${rowIdx}">
                        <td class="center"><input type="checkbox" class="tableflat" id="BillCheck" onclick="OnClickBillCheck(event)"></td>
                        <td class="center">${rowIdx}</td>
                            <td>
                        <div class="col-sm-10 no-padding">
                            <input id="InvoiceNumber" class="form-control" autocomplete="off" type="text" name="InvoiceNumber"  placeholder="" disabled value="${arr.Table[i].inv_no}">
                         <input id="HdAccId" hidden value="${AccID}">
</div>
                        <div class="col-sm-2 i_Icon">
                            <button type="button" class="calculator" onclick="" data-toggle="modal" data-target="#InvoiceDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InvoiceDetail").text()}"> </button>
                        </div>
                        </td>
                        <td><input id="InvoiceDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].inv_dt}"></td>
                        `+ td_billNoAndDate + `<td><input id="Currency" class="form-control center" autocomplete="off" type="text" disabled value="${arr.Table[i].curr}"></td>
                        <td><input id="InvoiceAmountInBase" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInBase" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_bs}"></td>
                        <td><input id="InvoiceAmountInSpecific" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInSpecific" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_sp}"></td>
                        <td><input id="UnAdjustedAmount"  class="form-control num_right" autocomplete="off" type="text" name="UnAdjustedAmount" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(UnAdjustedAmount), ValDecDigit))}"></td>
                        <td><div class="lpo_form"><input id="PayAmount"  class="form-control num_right" onchange="OnChangePayAmount(event)" onkeypress="return AmountFloatVal(this, event)" value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Pay_Amount), ValDecDigit))}" type="text" placeholder="0000.00">
<span id="PayAmount_Error" class="error-message is-visible"></span></div></td>                        
<td><input id="RemainingBalance" class="form-control num_right" autocomplete="off" type="text" name="RemainingBalance" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Rem_Bal), ValDecDigit))}"></td>
                
                </tr>`);

                                debugger;
                                //if (arr.Table[0].min_dt == "") {/*code start Add by Hina on 13-11-2024 to change discuss by vishal sir*/
                                //    Cmn_BindVouDate(todt, todt);
                                //}
                                //else {
                                /*start commented by Hina on 09-06-2025 discus with vishal sir for show data as voucher date should be less then equal to bill dt for BP,CP*/

                                //document.getElementById("txtFromdate").setAttribute('min', arr.Table[0].min_dt);
                                //document.getElementById("txtTodate").setAttribute('min', arr.Table[0].min_dt);
                                //$("#txtFromdate").val(arr.Table[0].min_dt);

                                /*}*//*code End Add by Hina on 13-11-2024 */
                            }
                            DisableDetail(Status);
                            CalculateTotalAmt();
                        }
                        else {
                            Cmn_BindVouDate(todt, todt);/*Add by Hina on 13-11-2024 to change discuss by vishal sir*/
                        }
                        //parseFloat(Rem_Bal).toFixed(ValDecDigit)
                    }
                    DisableData();
                    BillDetailCheckbox();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }
}
function DisableDetail(Status) {
    debugger;
    //var sessionval = sessionStorage.getItem("EditBtnClick");
    //if (Status == "") {
    //    sessionval = "Y";
    //}
    let txtdisable = $("#txtdisable").val();
    if (Status == "D" || Status == "") {
        $('#BillDetailTbl tbody tr').each(function () {
            var currentrow = $(this);
            if (txtdisable == "N") {
                currentrow.find("#PayAmount").attr("disabled", false);
                currentrow.find("#BillCheck").attr("disabled", false);
            }
            else {
                currentrow.find("#PayAmount").attr("disabled", true);
                currentrow.find("#BillCheck").attr("disabled", true);
            }
        });
    }
    else {
        $('#BillDetailTbl tbody tr').each(function () {
            var currentrow = $(this);
            currentrow.find("#PayAmount").attr("disabled", true);
            currentrow.find("#BillCheck").attr("disabled", true);
        });
    }
}
function RemoveSession() {
    sessionStorage.removeItem("BillDetailSession");
    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.removeItem("EditBtnClick");
}
function DisableData() {
    debugger;
    var Disable = $("#Disable").val();
    if (Disable == "Disable") {
        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#BillCheck").prop("disabled", true);
        });
    }
}
function OnClickAllBillCheck() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var Payamount = 0;
    var PendingAmt = 0;
    var Allcheck = "";
    if ($("#AllBillCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }

    if (Allcheck == 'Y') {
        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#UnAdjustedAmount").val());

            CurrentRow.find("#BillCheck").prop("checked", true);
            CurrentRow.find("#BillCheck").is(":checked")
            check = 'Y';
            if (check == 'Y') {
                //CurrentRow.find("#BillCheck").prop("checked", true);

                //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
                CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
                Payamount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
                var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount))
                //CurrentRow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemaingBal)).toFixed(ValDecDigit));
                CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));
            }
        });
    }
    else {
        //CurrentRow.find("#BillCheck").prop("checked", false);

        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#BillCheck").prop("checked", false);
            PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#UnAdjustedAmount").val());
            if (CurrentRow.find("#BillCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {

                CurrentRow.find("#BillCheck").prop("checked", false);
                CurrentRow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
                CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
            }
        });
    }

    CalculateTotalAmt();
}
function OnClickBillCheck(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var check = "";
    var CurrentRow = $(e.target).closest("tr");
    /*  $("#BillDetailTbl tbody tr").each(function () {*/
    debugger;
    var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#UnAdjustedAmount").val());
    //var Payamount = CurrentRow.find("#PayAmount").val();       
    if (CurrentRow.find("#BillCheck").is(":checked")) {
        check = 'Y';
    }
    else {
        check = 'N';
    }
    if (check == 'Y') {
        CurrentRow.find("#BillCheck").prop("checked", true);

        //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
        CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
        var Payamount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount));
        //CurrentRow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemaingBal)).toFixed(ValDecDigit));
        CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));
    }
    else {
        CurrentRow.find("#BillCheck").prop("checked", false);
        CurrentRow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
        //CurrentRow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
        CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
    }
    debugger;
    BillDetailCheckbox();
    CalculateTotalAmt();
}
function BillDetailCheckbox() {
    var check1 = "Y"
    $("#BillDetailTbl tbody tr").each(function () {
        debugger;
        var CurrentRow = $(this);
        if (CurrentRow.find("#BillCheck").is(":checked")) {
            check1 = 'Y';
        }
        else {
            check1 = 'N';
            return false;
        }
    });
    if (check1 == "Y") {
        $("#AllBillCheck").prop("checked", true);
    }
    else {
        $("#AllBillCheck").prop("checked", false);
    }
}
function OnChangePayAmount(e) {
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var PayAmount = cmn_ReplaceCommas(currentrow.find("#PayAmount").val());
    if (AvoidDot(PayAmount) == false) {
        PayAmount = "";
        currentrow.find("#PayAmount").val("")
    }
    else {
        //currentrow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PayAmount)).toFixed(ValDecDigit))
        currentrow.find("#PayAmount").val(cmn_addCommas(toDecimal(PayAmount, ValDecDigit)))
    }
    var PendingAmt = cmn_ReplaceCommas(currentrow.find("#UnAdjustedAmount").val());
    //var RemainingAmt = currentrow.find("#RemainingBalance").val();
    debugger;
    if (PayAmount != "") {
        if (parseFloat(PayAmount) != parseFloat(0)) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                currentrow.find("#PayAmount_Error").text($("#ExceedingAmount").text());
                currentrow.find("#PayAmount_Error").css("display", "block");
                currentrow.find("#PayAmount").css("border-color", "red");
            }
            else {
                var RemAmt = parseFloat(PendingAmt) - parseFloat(PayAmount);
                currentrow.find("#PayAmount_Error").css("display", "none");
                currentrow.find("#PayAmount").css("border-color", "#ced4da");
                //var test = parseFloat(cmn_ReplaceCommas(PayAmount)).toFixed(ValDecDigit);
                //var test = toDecimal(PayAmount, ValDecDigit);
                currentrow.find("#PayAmount").val(cmn_addCommas(toDecimal(PayAmount, ValDecDigit)));
                //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemAmt)).toFixed(ValDecDigit));
                currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(RemAmt, ValDecDigit)));
            }
        }
        else {
            //var test = parseFloat(0).toFixed(ValDecDigit);
            currentrow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
            //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
            currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
        }
    }
    else {
        //var test = parseFloat(0).toFixed(ValDecDigit);
        currentrow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
        //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
        currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
    }
    CalculateTotalAmt();
}
function CalculateTotalAmt() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        if (PayAmount != null && PayAmount != "") {
            TotalAmount = parseFloat(TotalAmount) + parseFloat(PayAmount);
        }
    });
    //$("#TotalPaid").text(parseFloat(cmn_ReplaceCommas(TotalAmount)).toFixed(ValDecDigit));
    $("#TotalPaid").text(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit)));
}
function OnClickSaveAndExitBillDt() {
    debugger
    var AccID = $('#IconHdEntityID').val();
    var ValDecDigit = $("#ValDigit").text();
    if (CheckBillDetailValidation() === true) {
        CalculateTotalAmt();
        let NewArr = [];
        var PayAmount = 0;
        debugger;
        //var IconPayAmt = parseFloat(cmn_ReplaceCommas($("#IconPayAmt").val())).toFixed(ValDecDigit);
        //var TotalPaidAmt = parseFloat(cmn_ReplaceCommas($("#TotalPaid").text())).toFixed(ValDecDigit);
        var IconPayAmt = toDecimal(cmn_ReplaceCommas($("#IconPayAmt").val()), ValDecDigit);
        var TotalPaidAmt = toDecimal(cmn_ReplaceCommas($("#TotalPaid").text()), ValDecDigit);
        if (parseFloat(cmn_ReplaceCommas(IconPayAmt)) >= parseFloat(cmn_ReplaceCommas(TotalPaidAmt))) {
            var FBillDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
            if (FBillDetails != null) {
                if (FBillDetails.length > 0) {

                    for (i = 0; i < FBillDetails.length; i++) {
                        var SAccID = FBillDetails[i].AccID;
                        if (SAccID == AccID) {
                        }
                        else {
                            NewArr.push(FBillDetails[i]);
                        }
                    }

                    $("#BillDetailTbl >tbody >tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var AccID = currentRow.find("#HdAccId").val();
                        var InvoiceNo = currentRow.find("#InvoiceNumber").val();
                        var InvoiceDate = currentRow.find("#InvoiceDate").val();
                        var BillNo = currentRow.find("#BillNo").val();//Added by Suraj on 03-05-2024
                        var BillDate = currentRow.find("#BillDate").val();//Added by Suraj on 03-05-2024
                        var Currency = currentRow.find("#Currency").val();
                        var InvAmtInBase = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInBase").val());
                        var InvAmtInSp = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInSpecific").val());
                        var PendAmount = cmn_ReplaceCommas(currentRow.find("#UnAdjustedAmount").val());
                        var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                        if (PayAmt == "") {
                            PayAmount = 0;
                        }
                        else {
                            PayAmount = PayAmt;
                        }
                        var RemBal = cmn_ReplaceCommas(currentRow.find("#RemainingBalance").val());
                        NewArr.push({
                            AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                            , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                            , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                            , BillNo: BillNo, BillDate: BillDate
                        })
                    });
                    sessionStorage.removeItem("BillDetailSession");
                    sessionStorage.setItem("BillDetailSession", JSON.stringify(NewArr));
                }
                else {
                    var BillDetailList = [];
                    $("#BillDetailTbl >tbody >tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var AccID = currentRow.find("#HdAccId").val();
                        var InvoiceNo = currentRow.find("#InvoiceNumber").val();
                        var InvoiceDate = currentRow.find("#InvoiceDate").val();
                        var BillNo = currentRow.find("#BillNo").val();//Added by Suraj on 03-05-2024
                        var BillDate = currentRow.find("#BillDate").val();//Added by Suraj on 03-05-2024
                        var Currency = currentRow.find("#Currency").val();
                        var InvAmtInBase = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInBase").val());
                        var InvAmtInSp = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInSpecific").val());
                        var PendAmount = cmn_ReplaceCommas(currentRow.find("#UnAdjustedAmount").val());
                        var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                        if (PayAmt == "") {
                            PayAmount = 0;
                        }
                        else {
                            PayAmount = PayAmt;
                        }
                        var RemBal = cmn_ReplaceCommas(currentRow.find("#RemainingBalance").val());
                        BillDetailList.push({
                            AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                            , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                            , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                            , BillNo: BillNo, BillDate: BillDate
                        })
                    });
                    sessionStorage.setItem("BillDetailSession", JSON.stringify(BillDetailList));
                }
            }
            else {
                var BillDetailList = [];
                $("#BillDetailTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var AccID = currentRow.find("#HdAccId").val();
                    var InvoiceNo = currentRow.find("#InvoiceNumber").val();
                    var InvoiceDate = currentRow.find("#InvoiceDate").val();
                    var BillNo = currentRow.find("#BillNo").val();//Added by Suraj on 03-05-2024
                    var BillDate = currentRow.find("#BillDate").val();//Added by Suraj on 03-05-2024
                    var Currency = currentRow.find("#Currency").val();
                    var InvAmtInBase = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInBase").val());
                    var InvAmtInSp = cmn_ReplaceCommas(currentRow.find("#InvoiceAmountInSpecific").val());
                    var PendAmount = cmn_ReplaceCommas(currentRow.find("#UnAdjustedAmount").val());
                    var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                    if (PayAmt == "") {
                        PayAmount = 0;
                    }
                    else {
                        PayAmount = PayAmt;
                    }
                    var RemBal = cmn_ReplaceCommas(currentRow.find("#RemainingBalance").val());
                    BillDetailList.push({
                        AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                        , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                        , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                        , BillNo: BillNo, BillDate: BillDate
                    })
                });
                sessionStorage.setItem("BillDetailSession", JSON.stringify(BillDetailList));
            }
            debugger;
            $("#BillSaveAndExitBtn").attr("data-dismiss", "modal");
            var row = $("#CNdetailTbl > tbody >tr #hfAccID[value=" + AccID + "]").closest('tr');
            ValidateEyeColor(row, "BillAdjustment", "N");
        }
        else {
            $("#BillSaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#AdjustedAmountMismatch").text(), "warning");
            return false;
        }


    }
    else {
        return false;
    }
}
function OnClickBillReset() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var PayAmt = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var Rembalance = cmn_ReplaceCommas(CurrentRow.find("#RemainingBalance").val());
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        var aalRemaining = parseFloat(PayAmount) + parseFloat(Rembalance);
        CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(aalRemaining, ValDecDigit)));
        CurrentRow.find("#PayAmount").val(PayAmt);
        CurrentRow.find("#BillCheck").prop("checked", false);
    });
    $('#TotalPaid').text(parseFloat(0).toFixed(ValDecDigit));
    $("#AllBillCheck").prop("checked", false);
}
function OnClickBillFifoAdj() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    //var AutopayAmt = parseFloat(cmn_ReplaceCommas($("#IconPayAmt").val())).toFixed(ValDecDigit);
    var AutopayAmt = cmn_ReplaceCommas($("#IconPayAmt").val());
    var PayAmt = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#UnAdjustedAmount").val());
        if (parseFloat(AutopayAmt) > parseFloat(PendingAmt)) {
            //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
            CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
            AutopayAmt = parseFloat(AutopayAmt) - parseFloat(PendingAmt);
        }
        else {
            //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(AutopayAmt)).toFixed(ValDecDigit));
            CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(AutopayAmt, ValDecDigit)));
            AutopayAmt = parseFloat(AutopayAmt) - parseFloat(AutopayAmt);
        }

        var Payamount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount))
        //CurrentRow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemaingBal)).toFixed(ValDecDigit));
        CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));

        CurrentRow.find("#BillCheck").prop("checked", false);
    });
    $("#AllBillCheck").prop("checked", false);
    CalculateTotalAmt();
}
function CheckBillDetailValidation() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var status = "N";
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#UnAdjustedAmount").val());

        if (PayAmount != "" && PayAmount != null) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                CurrentRow.find("#PayAmount_Error").text($("#ExceedingAmount").text());
                CurrentRow.find("#PayAmount_Error").css("display", "block");
                CurrentRow.find("#PayAmount").css("border-color", "red");
                status = "Y";
            }
            else {
                CurrentRow.find("#PayAmount_Error").css("display", "none");
                CurrentRow.find("#PayAmount").css("border-color", "#ced4da");
                //var test = parseFloat(cmn_ReplaceCommas(PayAmount)).toFixed(ValDecDigit);
                //var test = toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit);
                CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(PayAmount, ValDecDigit)));
            }
        }
        else {
            var TotalPaidAmt = cmn_ReplaceCommas($("#TotalPaid").text());
            if (parseFloat(cmn_ReplaceCommas(TotalPaidAmt)) > parseFloat(0)) {
            }
            else {
                swal("", $("#AdjustedAmountMismatch").text(), "warning");
                status = "Y";
            }
        }
    });
    if (status === "Y") {
        return false;
    }
    else {
        return true;
    }
}
function BtnBillSearch() {
    debugger;
    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.setItem("FromDateToDate", "Y");
    var AccID = $("#IconHdEntityID").val();
    BillWiseDetail(AccID);
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    //$("#hdFromdate").val(FromDate)
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {
            var date = new Date(), y = date.getFullYear(), m = date.getMonth();
            var firstDay = new Date(y, m, 1);

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            //var fromDate = new Date($("#hdFromdate").val());

            var month = (firstDay.getMonth() + 1);
            var day = firstDay.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = firstDay.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
function BindDate() {
    debugger;
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    var firstDay = new Date(y, m, 1);

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    $("#txtTodate").val(today);
    //$("#txtFromdate").val(firstDay);
    //var fromDate = new Date($("#hdFromdate").val());

    var month = (firstDay.getMonth() + 1);
    var day = firstDay.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = firstDay.getFullYear() + '-' + month + '-' + day;
    $("#txtFromdate").val(today);
}
function InsertBillWiseAdjDetails() {
    debugger;
    var FBillWiseAdjDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
    var BillWiseList = [];

    if (FBillWiseAdjDetails != null) {
        if (FBillWiseAdjDetails.length > 0) {
            for (i = 0; i < FBillWiseAdjDetails.length; i++) {
                var AccID = FBillWiseAdjDetails[i].AccID;
                var InvoiceNo = FBillWiseAdjDetails[i].InvoiceNo;
                var InvoiceDate = FBillWiseAdjDetails[i].InvoiceDate;
                var Currency = FBillWiseAdjDetails[i].Currency;
                var InvAmtInBase = cmn_ReplaceCommas(FBillWiseAdjDetails[i].InvAmtInBase);
                var InvAmtInSp = cmn_ReplaceCommas(FBillWiseAdjDetails[i].InvAmtInSp);
                var PendAmount = cmn_ReplaceCommas(FBillWiseAdjDetails[i].PendAmount);
                var PayAmount = cmn_ReplaceCommas(FBillWiseAdjDetails[i].PayAmount);
                var RemBal = cmn_ReplaceCommas(FBillWiseAdjDetails[i].RemBal);
                var BillNo = FBillWiseAdjDetails[i].BillNo;
                var BillDate = FBillWiseAdjDetails[i].BillDate;
                BillWiseList.push({
                    AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                    , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                    , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                    , BillNo: IsNull(BillNo, ""), BillDate: IsNull(BillDate, "")
                })
            }
        }
    }
    debugger;
    return BillWiseList;


};
function onclkDtlSaveBtnValidateBillAd() {
    debugger;
    Flag = "N";

    $("#CNdetailTbl > tbody > tr").each(function () {
        debugger;
        var CurrRow = $(this);
        var Acc_Type = CurrRow.find("#hdAccType").val();
        var JvAmt;
        var CreditAmountInSpecific = CurrRow.find('#CreditAmountInSpecific').val();
        if (CheckNullNumber(CreditAmountInSpecific) != "0") {
            JvAmt = CurrRow.find("#CreditAmountInSpecific").val();
        }
        else {
            JvAmt = CurrRow.find("#DebitAmountInSpecific").val();
        }
        var GlId_Billad = CurrRow.find('option:selected').val();
        var BillAdjustment = JSON.parse(sessionStorage.getItem("BillDetailSession"));
        var Amt = 0;
        if (Acc_Type != "8") {
            if (BillAdjustment != null) {
                debugger;
                if (BillAdjustment.length > 0) {
                    for (i = 0; i < BillAdjustment.length; i++) {
                        var Acc_id = BillAdjustment[i].AccID;
                        if (GlId_Billad == Acc_id) {
                            Amt = parseFloat(cmn_ReplaceCommas(Amt)) + parseFloat(cmn_ReplaceCommas(BillAdjustment[i].PayAmount))
                        }
                    }
                }
            }
            if (parseFloat(cmn_ReplaceCommas(JvAmt)) >= parseFloat(cmn_ReplaceCommas(Amt))) {
                CurrRow.find("#BillAdjustment").css("border-color", "#ced4da");
            }
            else {
                //CurrRow.find("#BillAdjustment").css("border-color", "red");
                ValidateEyeColor(CurrRow, "BillAdjustment", "Y");
                Flag = "Y";
            }
        }
    })


    if (Flag == "Y") {
        swal("", $("#AdjustmentAmtshouldbeLessorEqualtoApymentAmt").text(), "warning");
        return false;
    } else {
        return true;
    }
}
function DeletebillAdjustment(hfAccID) {
    debugger;
    var DeleteDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
    var DelteArrData = [];
    if (DeleteDetails != null) {
        if (DeleteDetails.length > 0) {
            for (i = 0; i < DeleteDetails.length; i++) {
                var Acc_id = DeleteDetails[i].AccID;
                if (hfAccID == Acc_id) {
                }
                else {
                    DelteArrData.push(DeleteDetails[i])
                }
            }
        }
    }
    sessionStorage.setItem("BillDetailSession", JSON.stringify(DelteArrData));
}
//------------------Bill Adjustmet End--------------------------
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