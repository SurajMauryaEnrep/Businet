/*toDecimal is added by Nitesh Tewatia on 15-02-2024*/
$(document).ready(function () {
    debugger;
    //BindBankAccList();
    BindDDLAccountList();
    BPNo = $("#VouNo").val().trim();
    $("#hdDoc_No").val(BPNo);
    $('#ContradetailTbl').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
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
    });
    CancelledRemarks("#Cancelled", "Disabled");
});

var ValDecDigit = $("#ValDigit").text();
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#ContradetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function BindDDLAccountList() {
    debugger;

    Cmn_BindAccountList("#GLAccount_", "1", "#ContradetailTbl", "#SNohf", "BindData", "105104115125");

}
function BindData() {
    debugger;

    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#ContradetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#ContradetailTbl >tbody >tr").length) {
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
                        if (check(data, selected, "#ContradetailTbl", "#SNohf", "#GLAccount_") == true) {
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
    $("#ContradetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        var AccountID = '#GLAccount_' + rowid;
        if (AccID != '0' && AccID != "") {
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "");
            currentRow.find("#GLAccount_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "OnChangeAccountName(this,event)");
        }

    });

}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    //$("#ItemCostError").css("display", "none");
    //$("#ItemCost").css("border-color", "#ced4da");   

    return true;
}
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var rowCount = $('#ContradetailTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#ContradetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#ContradetailTbl tbody').append(`<tr id="${++rowIdx}">
 <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>   
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form" style="padding:0px;">
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input type="hidden" id="hfAccID" style="display: none;" />
<input type="hidden" id="hdAccType" style="display: none;" />
<input type="hidden" id="hd_od_allow" style="display: none;" />
<input type="hidden" id="hd_od_limit" style="display: none;" />
<span id="AccountNameError" class="error-message is-visible"></span></div>
</td>

<td>
<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','VouDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"> </button></div>
</td>

<td><textarea class="form-control remarksmessage" cols="20" id="GLGroup" maxlength="100" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="GLGroup" value="" placeholder="${$("#AccGroup").text()}" rows="2"  readonly></textarea>
 <input id="hdGLGroupID" type="hidden" /></td>
 <td><input id="Currancy" class="form-control center" autocomplete="off" type="text" name="" placeholder="${$("#span_Currency").text()}" disabled>
<input type="hidden" id="hdCurrId" style="display: none;" />
<input type="hidden" id="hdForeignCurr" style="display: none;" />
<input type="hidden" id="hdClosBl" style="display: none;" />
</td>
 <td><input id="ExRate" onchange="OnChangeExchangeRate(event)" onkeypress = "return RateFloatValueonly(this,event);" class="form-control center" autocomplete="off" type="text" name="" disabled></td>
<td hidden><input id="Balance" class="form-control num_right" onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text" name="Balance" placeholder="0000.00" disabled></td>
<td>
<div class="lpo_form">
<input id="DebitAmountInBase" class="form-control num_right" onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Dbt_Amnt_Error" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInSpecific"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00" disabled>
<span id="Dbt_Amnt_Error_sp" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInBase" class="form-control num_right" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)"  onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00">
<span id="Crdt_Amnt_Error" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInSpecific"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)"  onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00" disabled>
<span id="Crdt_Amnt_Error_sp" class="error-message is-visible"></span>
</div>
</td>

<td>
<div class="lpo_form">
    <textarea id="Narration" class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}"></textarea>
       <span id="Narr_Error" class="error-message is-visible"></span>
 </div>

<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /><input type="hidden" id="hdn_clag" value="" /></td>
</tr>`);
    /*COMMENTED and modify above by Hina on 27-08-2025 to add account balance of particular account*/
    //<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form no-padding">
    //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
    //    <input type="hidden" id="hfAccID" style="display: none;" />
    //    <input type="hidden" id="hdAccType" style="display: none;" />
    //    <input type="hidden" id="hd_od_allow" style="display: none;" />
    //    <input type="hidden" id="hd_od_limit" style="display: none;" />
    //    <span id="AccountNameError" class="error-message is-visible"></span></div>
    //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','VouDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
    //</td>

    BindAcountList(RowNo);
}
function BindAcountList(ID) {
    debugger;

    Cmn_BindAccountList("#GLAccount_", ID, "#ContradetailTbl", "#SNohf", "", "105104115125");

}
function OnChangeAccountName(RowID, e) {
    debugger;
    $("#VouDate").attr("readonly", "readonly");

    BindAccountList(e);
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    //$("#ItemCostError").css("display", "none");
    //$("#ItemCost").css("border-color", "#ced4da");   

    return true;
}
function OnChangeExchangeRate(e) {
    var clickedRow = $(e.target).closest('tr');
    var ExchDecDigit = $("#ExchDigit").text();
    var ExRate = clickedRow.find("#ExRate").val();
    //clickedRow.find("#ExRate").val(parseFloat(ExRate).toFixed(ExchDecDigit));
    clickedRow.find("#ExRate").val(toDecimal(cmn_ReplaceCommas(ExRate), ExchDecDigit));
    CalculateBaseAmountOnChangeExRate(clickedRow);
}

function BindAccountList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Acc_ID = clickedrow.find("#GLAccount_" + SNo).val();

    clickedrow.find("#hfAccID").val(Acc_ID);


    var Status = $("#hdContraStatus").val();

    if (Status == "") {
        clickedrow.find("#CreditAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
        clickedrow.find("#CreditAmountInBase").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
        clickedrow.find("#DebitAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
        clickedrow.find("#DebitAmountInBase").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
    }
    if (Acc_ID == "0") {
        clickedrow.find("#AccountNameError").text($("#valueReq").text());
        clickedrow.find("#AccountNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#AccountNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");

        clickedrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
        clickedrow.find("#Crdt_Amnt_Error").text("");
        clickedrow.find("#Crdt_Amnt_Error").css("display", "none");
        clickedrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
        clickedrow.find("#Dbt_Amnt_Error").text("");
        clickedrow.find("#Dbt_Amnt_Error").css("display", "none");

        clickedrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
        clickedrow.find("#Crdt_Amnt_Error_sp").text("");
        clickedrow.find("#Crdt_Amnt_Error_sp").css("display", "none");
        clickedrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
        clickedrow.find("#Dbt_Amnt_Error_sp").text("");
        clickedrow.find("#Dbt_Amnt_Error_sp").css("display", "none");

        Cmn_BindGroup(clickedrow, Acc_ID, "")
        /*-----------Code Start Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
        var VouDate = $("#VouDate").val();
        CMN_BindAccountBalance(clickedrow, Acc_ID, VouDate);
    /*-----------Code End Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
        BindAccountBalance(clickedrow, Acc_ID);
    }
}
function BindAccountBalance(clickedrow, Acc_ID) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text()
    var VouDate = $("#VouDate").val();
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/Contra/GetAccCLbal",
            data: {
                acc_id: Acc_ID,
                Date: VouDate,
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
                    if (arr.Table2.length > 0) {
                        if (arr.Table3[0].acc_type != '7') {
                            clickedrow.find("#Balance").val(arr.Table2[0].ClosBL);
                            clickedrow.find("#hdClosBl").val(arr.Table2[0].ClosBL);
                        }
                        else {
                            clickedrow.find("#Balance").val(arr.Table2[0].Avl_bal);
                            clickedrow.find("#hdClosBl").val(arr.Table2[0].Avl_bal);
                            if (arr.Table2[0].OdLimit != '') {
                                clickedrow.find("#hd_od_limit").val(arr.Table2[0].OdLimit);
                                clickedrow.find("#hd_od_allow").val(arr.Table3[0].od_allow);
                            }
                        }
                    }
                    else {
                        clickedrow.find("#Balance").val(parseFloat(0).toFixed(RateDecDigit));
                        clickedrow.find("#hdClosBl").val(parseFloat(0).toFixed(RateDecDigit));
                    }
                    if (arr.Table.length > 0) {
                        var cflag = arr.Table[0].cflag.trim();

                        clickedrow.find("#hdn_clag").val(cflag);

                        if (cflag == "B") {
                            clickedrow.find("#DebitAmountInSpecific").attr("readonly", "readonly");
                            clickedrow.find("#CreditAmountInSpecific").attr("readonly", "readonly");
                            clickedrow.find("#DebitAmountInBase").removeAttr("readonly");
                            clickedrow.find("#CreditAmountInBase").removeAttr("readonly");

                            clickedrow.find("#DebitAmountInBase").removeAttr("disabled");
                            clickedrow.find("#CreditAmountInBase").removeAttr("disabled");
                        }
                        if (cflag == "S") {
                            clickedrow.find("#DebitAmountInSpecific").removeAttr("disabled");
                            clickedrow.find("#CreditAmountInSpecific").removeAttr("disabled");

                            clickedrow.find("#DebitAmountInSpecific").removeAttr("readonly");
                            clickedrow.find("#CreditAmountInSpecific").removeAttr("readonly");

                            clickedrow.find("#DebitAmountInBase").attr("readonly", "readonly");
                            clickedrow.find("#CreditAmountInBase").attr("readonly", "readonly");

                        }
                    }
                }
            },
        });

    Cmn_VoucherDateValidation("#VouDate");
}
function VoucherDateValidation() {
    debugger;
    Cmn_VoucherDateValidation("#VouDate");

}
function AmountFloatVal(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
}
function OnChangeDebitAmount(e) {
    debugger;

    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var cflag = currentrow.find("#hdn_clag").val().trim();

    if (cflag == "B") {

        /*Commented by HIna on 27-10-2023 to changes for Base Amount not for specific*/
        /*var debitAmt = currentrow.find("#DebitAmountInSpecific").val();*/
        var debitAmt12 = currentrow.find("#DebitAmountInBase").val();
        var debitAmt = cmn_ReplaceCommas(debitAmt12);

        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            /*currentrow.find("#DebitAmountInSpecific").val("")*/
            currentrow.find("#DebitAmountInBase").val("");
        }
        else {
            //currentrow.find("#DebitAmountInSpecific").val(parseFloat(debitAmt).toFixed(ValDecDigit))
            //currentrow.find("#CreditAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit))

            //var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit);/**Commented By Nitesh 15-02-2024 for remove Tofixed where value pass ignore Zero**/
            var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDecDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            currentrow.find("#DebitAmountInBase").val(debitAmt1);
            currentrow.find("#CreditAmountInBase").val(parseFloat(0).toFixed(ValDecDigit));
        }

        if (debitAmt != "") {
            currentrow.find("#CreditAmountInBase").attr("disabled", true);
            currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("");
            currentrow.find("#Crdt_Amnt_Error").css("display", "none");
            currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("");
            currentrow.find("#Dbt_Amnt_Error").css("display", "none");

        }
        else {
            currentrow.find("#CreditAmountInBase").attr("disabled", false);
            currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("");
            currentrow.find("#Crdt_Amnt_Error").css("display", "none");
        }

        var ConvRate1 = currentrow.find("#ExRate").val();
        var ConvRate = cmn_ReplaceCommas(ConvRate1);
        if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            /*Commented by HIna on 27-10-2023 to changes for Base Amount not for specific*/
            /* var FAmt = debitAmt * ConvRate;*/
            var FAmt = debitAmt / ConvRate;
            // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit)
            var FinVal = cmn_addCommas(FinVal1);
            /*currentrow.find("#DebitAmountInBase").val(FinVal);*/
            currentrow.find("#DebitAmountInSpecific").val(FinVal);
        }
    }
    if (cflag == "S") {
        var debitAmt12 = currentrow.find("#DebitAmountInSpecific").val();
        var debitAmt = cmn_ReplaceCommas(debitAmt12);
        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            currentrow.find("#DebitAmountInSpecific").val("")
        }
        else {
            // var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit);
            var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDecDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            currentrow.find("#DebitAmountInSpecific").val(debitAmt1);
            //currentrow.find("#DebitAmountInSpecific").val(parseFloat(debitAmt).toFixed(ValDecDigit))
            currentrow.find("#CreditAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit))
        }

        if (debitAmt != "") {
            currentrow.find("#CreditAmountInSpecific").attr("disabled", true);
            currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error_sp").text("");
            currentrow.find("#Crdt_Amnt_Error_sp").css("display", "none");
            currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error_sp").text("");
            currentrow.find("#Dbt_Amnt_Error_sp").css("display", "none");
        }
        else {
            currentrow.find("#CreditAmountInSpecific").attr("disabled", false);
            currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error_sp").text("");
            currentrow.find("#Crdt_Amnt_Error_sp").css("display", "none");
        }

        var ConvRate1 = currentrow.find("#ExRate").val();
        var ConvRate = cmn_ReplaceCommas(ConvRate1);
        if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            var FAmt = debitAmt * ConvRate;
            // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#DebitAmountInBase").val(FinVal);
        }
    }

    CalculateTotalDrCrValue();
}

function OnChangeCreditAmount(e) {
    debugger;

    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var Status = $("#hdContraStatus").val();
    var currentrow = $(e.target).closest('tr');
    let hdnAccType = currentrow.find("#hdAccType").val().trim();
    var cflag = currentrow.find("#hdn_clag").val().trim();

    if (cflag == "B") {

        /*Commented by HIna on 27-10-2023 to changes for Base Amount not for specific*/
        /*var creditAmt = currentrow.find("#CreditAmountInSpecific").val();*/
        var creditAmt12 = currentrow.find("#CreditAmountInBase").val();
        var creditAmt = cmn_ReplaceCommas(creditAmt12);
        /*Start Add by HIna on 27-10-2023 to credit Base Amount for only cash account with curr_id=1(INR) 
         * and credit specific Amount with CashA/c USD with curr_id!=1*/
        var AccType = currentrow.find("#hdAccType").val();
        var HdnCrBaseAmount1 = currentrow.find("#HdnCrBaseAmnt").val();
        var HdnCrBaseAmount = cmn_ReplaceCommas(HdnCrBaseAmount1);
        var HdnCrSpecificAmount1 = currentrow.find("#HdnCrSpecficAmnt").val();
        var HdnCrSpecificAmount = cmn_ReplaceCommas(HdnCrSpecificAmount1);
        var CurrId = currentrow.find("#hdCurrId").val();
        /*End */
        var con_rate1 = currentrow.find("#ExRate").val();
        var con_rate = cmn_ReplaceCommas(con_rate1);
        var Balance1 = currentrow.find("#Balance").val();
        var Balance = cmn_ReplaceCommas(Balance1);
        var OdAllow = currentrow.find("#hd_od_allow").val();
        var OdLimit = cmn_ReplaceCommas(currentrow.find("#hd_od_limit").val());
        const MyArray = Balance.split(" ");
        var Bal1 = MyArray[0];
        var fin_Bal = cmn_ReplaceCommas(Bal1);
        var Bal = (parseFloat(fin_Bal) * parseFloat(con_rate));
        var BalType = MyArray[1];
        var FinBal = 0;
        if (OdAllow == "Y") {

            if (BalType == "Dr") {
                FinBal = parseFloat(Bal) + parseFloat(OdLimit);
            }
            else {
                FinBal = parseFloat(OdLimit) - parseFloat(Bal);
            }
        }
        else {
            if (BalType == "Dr") {
                FinBal = parseFloat(Bal);
                if (Status == "D" && AccType == "8" && CurrId == "1") {
                    FinBal = parseFloat(FinBal) + parseFloat(HdnCrBaseAmount)
                }
                if (Status == "D" && AccType == "8" && CurrId != "1") {
                    FinBal = parseFloat(FinBal) + parseFloat(HdnCrSpecificAmount)
                }
            }
            else {
                FinBal = - parseFloat(Bal);
            }
        }
        debugger;

        /*Condition Added by Suraj on 04-04-2024 'if (hdnAccType == "7" || hdnAccType == "8")' */
        if (hdnAccType == "7" || hdnAccType == "8") {
            if (parseFloat(creditAmt) > parseFloat(FinBal)) {
                /*Commented by HIna on 27-10-2023 to changes for Base Amount not for specific*/
                /*currentrow.find("#CreditAmountInSpecific").css("border-color", "red");*/
                //var sp_bal1 = parseFloat(creditAmt / con_rate).toFixed(ValDecDigit);
                var sp_bal1 = toDecimal(cmn_ReplaceCommas(creditAmt / con_rate), ValDecDigit);
                var sp_bal = cmn_addCommas(sp_bal1);
                currentrow.find("#CreditAmountInSpecific").val(sp_bal);

                currentrow.find("#CreditAmountInBase").css("border-color", "red");
                currentrow.find("#Crdt_Amnt_Error").text($("#ExceedingBalance").text());
                currentrow.find("#Crdt_Amnt_Error").css("display", "block");
            }
            else {
                if (AvoidDot(creditAmt) == false) {
                    creditAmt = "";
                    /*currentrow.find("#CreditAmountInSpecific").val("")*/
                    currentrow.find("#CreditAmountInBase").val("")
                }
                else {
                    //currentrow.find("#CreditAmountInSpecific").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                    //currentrow.find("#DebitAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit))
                    //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
                    var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDecDigit);
                    var creditAmt1 = cmn_addCommas(creditAmt2);
                    currentrow.find("#CreditAmountInBase").val(creditAmt1);
                    //currentrow.find("#CreditAmountInBase").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                    currentrow.find("#DebitAmountInBase").val(parseFloat(0).toFixed(ValDecDigit));
                }

                if (creditAmt != "") {
                    currentrow.find("#DebitAmountInBase").attr("disabled", true);
                    currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
                    currentrow.find("#Dbt_Amnt_Error").text("");
                    currentrow.find("#Dbt_Amnt_Error").css("display", "none");
                    currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
                    currentrow.find("#Crdt_Amnt_Error").text("");
                    currentrow.find("#Crdt_Amnt_Error").css("display", "none");

                }
                else {
                    currentrow.find("#DebitAmountInBase").attr("disabled", false);
                    currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
                    currentrow.find("#Dbt_Amnt_Error").text("");
                    currentrow.find("#Dbt_Amnt_Error").css("display", "none");
                }

                var ConvRate1 = currentrow.find("#ExRate").val();
                var ConvRate = cmn_ReplaceCommas(ConvRate1);

                if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
                    debugger;
                    /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
                    /*var FAmt = creditAmt * ConvRate;*/
                    var FAmt = creditAmt / ConvRate;
                    // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
                    var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
                    var FinVal = cmn_addCommas(FinVal1);
                    /* currentrow.find("#CreditAmountInBase").val(FinVal);*/
                    currentrow.find("#CreditAmountInSpecific").val(FinVal);
                }
            }

        } else {
            if (AvoidDot(creditAmt) == false) {
                creditAmt = "";
                /*currentrow.find("#CreditAmountInSpecific").val("")*/
                currentrow.find("#CreditAmountInBase").val("")
            }
            else {
                //currentrow.find("#CreditAmountInSpecific").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                //currentrow.find("#DebitAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit))
                //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
                var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDecDigit);
                var creditAmt1 = cmn_addCommas(creditAmt2);
                currentrow.find("#CreditAmountInBase").val(creditAmt1);
                //currentrow.find("#CreditAmountInBase").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                currentrow.find("#DebitAmountInBase").val(parseFloat(0).toFixed(ValDecDigit));
            }

            if (creditAmt != "") {
                currentrow.find("#DebitAmountInBase").attr("disabled", true);
                currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");
                currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error").text("");
                currentrow.find("#Crdt_Amnt_Error").css("display", "none");

            }
            else {
                currentrow.find("#DebitAmountInBase").attr("disabled", false);
                currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");
            }

            var ConvRate1 = currentrow.find("#ExRate").val();
            var ConvRate = cmn_ReplaceCommas(ConvRate1);

            if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
                debugger;
                /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
                /*var FAmt = creditAmt * ConvRate;*/
                var FAmt = creditAmt / ConvRate;
                // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
                var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
                var FinVal = cmn_addCommas(FinVal1);
                /* currentrow.find("#CreditAmountInBase").val(FinVal);*/
                currentrow.find("#CreditAmountInSpecific").val(FinVal);
            }
        }

    }
    if (cflag == "S") {

        var creditAmt12 = currentrow.find("#CreditAmountInSpecific").val();
        var creditAmt = cmn_ReplaceCommas(creditAmt12);
        /*Start Add by HIna on 27-10-2023 to credit Base Amount for only cash account with curr_id=1(INR) 
         * and credit specific Amount with CashA/c USD with curr_id!=1*/
        var AccType = currentrow.find("#hdAccType").val();
        var HdnCrBaseAmount1 = currentrow.find("#HdnCrBaseAmnt").val();
        var HdnCrBaseAmount = cmn_ReplaceCommas(HdnCrBaseAmount1);
        var HdnCrSpecificAmount1 = currentrow.find("#HdnCrSpecficAmnt").val();
        var HdnCrSpecificAmount = cmn_ReplaceCommas(HdnCrSpecificAmount1);
        var CurrId = currentrow.find("#hdCurrId").val();
        /*End */
        var con_rate1 = currentrow.find("#ExRate").val();
        var con_rate = cmn_ReplaceCommas(con_rate1);
        var Balance1 = currentrow.find("#Balance").val();
        var Balance = cmn_ReplaceCommas(Balance1);
        var OdAllow = currentrow.find("#hd_od_allow").val();
        var OdLimit = cmn_ReplaceCommas(currentrow.find("#hd_od_limit").val());
        const MyArray = Balance.split(" ");
        var Bal1 = MyArray[0];
        var Bal = cmn_ReplaceCommas(Bal1);
        //var Bal = (parseFloat(fin_Bal) * parseFloat(con_rate));
        var BalType = MyArray[1];
        var FinBal = 0;
        if (OdAllow == "Y") {
            if (BalType == "Dr") {
                FinBal = parseFloat(Bal) + parseFloat(OdLimit);
            }
            else {
                FinBal = parseFloat(OdLimit) - parseFloat(Bal);
            }
        }
        else {
            if (BalType == "Dr") {
                FinBal = parseFloat(Bal);
                if (Status == "D" && AccType == "8" && CurrId == "1") {
                    FinBal = parseFloat(FinBal) + parseFloat(HdnCrBaseAmount)
                }
                if (Status == "D" && AccType == "8" && CurrId != "1") {
                    FinBal = parseFloat(FinBal) + parseFloat(HdnCrSpecificAmount)
                }
            }
            else {
                FinBal = - parseFloat(Bal);
            }
        }
        debugger;

        if (hdnAccType == "7" || hdnAccType == "8") {
            if (parseFloat(creditAmt) > parseFloat(FinBal)) {
                //  var sp_bal1 = parseFloat(creditAmt * con_rate).toFixed(ValDecDigit);
                var sp_bal1 = toDecimal(cmn_ReplaceCommas(creditAmt * con_rate), ValDecDigit);
                var sp_bal = cmn_addCommas(sp_bal1);
                currentrow.find("#CreditAmountInBase").val(sp_bal);

                currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                currentrow.find("#Crdt_Amnt_Error_sp").text($("#ExceedingBalance").text());
                currentrow.find("#Crdt_Amnt_Error_sp").css("display", "block");
            }
            else {
                if (AvoidDot(creditAmt) == false) {
                    creditAmt = "";
                    currentrow.find("#CreditAmountInSpecific").val("");
                }
                else {
                    //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
                    var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDecDigit);
                    var creditAmt1 = cmn_addCommas(creditAmt2);
                    currentrow.find("#CreditAmountInSpecific").val(creditAmt1);
                    //currentrow.find("#CreditAmountInSpecific").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                    currentrow.find("#DebitAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit));
                }

                if (creditAmt != "") {
                    currentrow.find("#DebitAmountInSpecific").attr("disabled", true);
                    currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                    currentrow.find("#Dbt_Amnt_Error_sp").text("");
                    currentrow.find("#Dbt_Amnt_Error_sp").css("display", "none");

                    currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                    currentrow.find("#Crdt_Amnt_Error_sp").text("");
                    currentrow.find("#Crdt_Amnt_Error_sp").css("display", "none");

                }
                else {
                    currentrow.find("#DebitAmountInSpecific").attr("disabled", false);
                    currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                    currentrow.find("#Dbt_Amnt_Error_sp").text("");
                    currentrow.find("#Dbt_Amnt_Error_sp").css("display", "none");
                }

                var ConvRate1 = currentrow.find("#ExRate").val();
                var ConvRate = cmn_ReplaceCommas(ConvRate1);
                if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
                    debugger;
                    var FAmt = creditAmt * ConvRate;
                    //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
                    var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
                    var FinVal = cmn_addCommas(FinVal1);
                    currentrow.find("#CreditAmountInBase").val(FinVal);
                }
            }
        } else {
            if (AvoidDot(creditAmt) == false) {
                creditAmt = "";
                currentrow.find("#CreditAmountInSpecific").val("");
            }
            else {
                //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
                var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDecDigit);
                var creditAmt1 = cmn_addCommas(creditAmt2);
                currentrow.find("#CreditAmountInSpecific").val(creditAmt1);
                //currentrow.find("#CreditAmountInSpecific").val(parseFloat(creditAmt).toFixed(ValDecDigit))
                currentrow.find("#DebitAmountInSpecific").val(parseFloat(0).toFixed(ValDecDigit));
            }

            if (creditAmt != "") {
                currentrow.find("#DebitAmountInSpecific").attr("disabled", true);
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error_sp").text("");
                currentrow.find("#Dbt_Amnt_Error_sp").css("display", "none");

                currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Crdt_Amnt_Error_sp").text("");
                currentrow.find("#Crdt_Amnt_Error_sp").css("display", "none");

            }
            else {
                currentrow.find("#DebitAmountInSpecific").attr("disabled", false);
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error_sp").text("");
                currentrow.find("#Dbt_Amnt_Error_sp").css("display", "none");
            }

            var ConvRate1 = currentrow.find("#ExRate").val();
            var ConvRate = cmn_ReplaceCommas(ConvRate1);
            if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
                debugger;
                var FAmt = creditAmt * ConvRate;
                //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
                var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
                var FinVal = cmn_addCommas(FinVal1);
                currentrow.find("#CreditAmountInBase").val(FinVal);
            }
        }

    }
    CalculateTotalDrCrValue();

}
function CalculateBaseAmountOnChangeExRate(currentrow) {
    /*Commented by HIna on 27-10-2023 to changes for Base Amount not for specific*/
    //var creditAmt = currentrow.find("#CreditAmountInSpecific").val();
    //var debitAmt = currentrow.find("#DebitAmountInSpecific").val();
    var debitAmt12 = currentrow.find("#DebitAmountInBase").val();
    var debitAmt = cmn_ReplaceCommas(debitAmt12);
    var creditAmt12 = currentrow.find("#CreditAmountInBase").val();
    var creditAmt = cmn_ReplaceCommas(creditAmt12);
    var ConvRate12 = currentrow.find("#ExRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate12);
    var ValDecDigit = $("#ValDigit").text();

    if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
        debugger;
        /* var FAmt = creditAmt * ConvRate;*/
        var FAmt = creditAmt / ConvRate;
        //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        /*currentrow.find("#CreditAmountInBase").val(FinVal);*/
        currentrow.find("#CreditAmountInSpecific").val(FinVal);
    }

    if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        /* var FAmt = debitAmt * ConvRate;*/
        var FAmt = debitAmt / ConvRate;
        // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        /*currentrow.find("#DebitAmountInBase").val(FinVal);*/
        currentrow.find("#DebitAmountInSpecific").val(FinVal);

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
function CalculateTotalDrCrValue() {
    var debitAmount = 0;
    var creditAmount = 0;
    //var GrossValue = 0;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    $("#ContradetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        var DbtAmnt12 = currentrow.find("#DebitAmountInBase").val();
        var DbtAmnt = cmn_ReplaceCommas(DbtAmnt12);
        var CrdtAmnt12 = currentrow.find("#CreditAmountInBase").val();
        var CrdtAmnt = cmn_ReplaceCommas(CrdtAmnt12);
        //var DbtCrtTAmnt = 0;
        if (AvoidDot(DbtAmnt) == false) {
            DbtAmnt = 0;
        }
        if (AvoidDot(CrdtAmnt) == false) {
            CrdtAmnt = 0;
        }
        debitAmount = parseFloat(parseFloat(debitAmount).toFixed(ValDigit)) + parseFloat(parseFloat(DbtAmnt).toFixed(ValDigit));
        creditAmount = parseFloat(parseFloat(creditAmount).toFixed(ValDigit)) + parseFloat(parseFloat(CrdtAmnt).toFixed(ValDigit));
    })
    //  var debitAmount2 = parseFloat(debitAmount).toFixed(ValDigit);
    var debitAmount2 = toDecimal(cmn_ReplaceCommas(debitAmount), ValDigit);
    var debitAmount1 = cmn_addCommas(debitAmount2);
    $("#dbtTtlAmnt").text(debitAmount1);
    //$("#dbtTtlAmnt").text(parseFloat(debitAmount).toFixed(ValDigit));
    // var creditAmount2 = parseFloat(creditAmount).toFixed(ValDigit);
    var creditAmount2 = toDecimal(cmn_ReplaceCommas(creditAmount), ValDigit);
    var creditAmount1 = cmn_addCommas(creditAmount2);
    $("#crdtTtlAmnt").text(creditAmount1);
    //$("#crdtTtlAmnt").text(parseFloat(creditAmount1).toFixed(ValDigit));
    //modifye by shubham maurya on 06-12-2022 16:25 comment this line
    //$("#AccountBalance").val(parseFloat(0).toFixed(ValDigit));
    $("#hdnVouAmnt").val(creditAmount1);
    //$("#hdnVouAmnt").val(parseFloat(creditAmount).toFixed(ValDigit));
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
    //  var FinalDiff2 = parseFloat(FinalDiff).toFixed(ValDigit);
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
function InsertContraDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    var SrcType = $("#Src_Type").val();
    $("#hdSrc_Type").val(SrcType);
    var hdContraStatus = $("#hdContraStatus").val();
    if (hdContraStatus == "A") {

    } else {
        if (HeaderValidation() == false) {
            return false;
        }
        if (AccountValidation() == false) {
            return false;
        }
        if (saveDrCrEqualAmnt() == false) {
            return false;
        }
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
    FinalAccountDetail = InsertContraAccDetail();
    var AccountDt = JSON.stringify(FinalAccountDetail);
    $('#hdAccountDetailList').val(AccountDt);
    CalculateTotalDrCrValue();


    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/

    RemoveSession();
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
};
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
        //  $("#DiffrncAmnt").text(parseFloat(difference).toFixed(ValDigit));
        $("#DiffrncAmnt").text(toDecimal(cmn_ReplaceCommas(difference), ValDigit));
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
function HeaderValidation() {
    debugger;
    var ErrorFlag = "N";

    //var ConvRate = $("#ConvRate").val();
    //if (ConvRate == "" || ConvRate == "0") {
    //    document.getElementById("ConvRateError").innerHTML = $("#valueReq").text();
    //    $("#ConvRateError").css("display", "block");
    //    $("#ConvRate").css("border-color", "red");
    //    $("#ManConv").css("display", "");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#ConvRateError").css("display", "none");
    //    $("#ConvRate").css("border-color", "#ced4da");
    //    $("#ManConv").css("display", "none");
    //}
    //var Currency = $("#curr").val();
    //if (Currency == "" || Currency == "0") {
    //    document.getElementById("vmCurr").innerHTML = $("#valueReq").text();
    //    $("#vmCurr").css("display", "block");
    //    $("#curr").css("border-color", "red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#vmCurr").css("display", "none");
    //    $("#curr").css("border-color", "#ced4da");
    //}

    

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertContraAccDetail() {
    debugger;
    var AccountDetailList = new Array();
    $("#ContradetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var SpanRowId = currentRow.find("#SpanRowId").text();
        var AccountList = {};
        /*Commented by Hina  on 29-06-2024 to seprate acc_id and spanrowid as seq_no*/
        /*AccountList.acc_id = (currentRow.find("#GLAccount_" + rowid).val()) + '_' + SpanRowId;*/
        AccountList.acc_id = currentRow.find("#GLAccount_" + rowid).val();
        AccountList.acc_name = currentRow.find("#GLAccount_" + rowid + " option:selected").text();
        AccountList.acc_group_name = currentRow.find("#GLGroup").val();
        AccountList.acc_type = currentRow.find("#hdAccType").val();
        AccountList.curr_id = currentRow.find("#hdCurrId").val();
        AccountList.curr_name = currentRow.find("#Currancy").val();
        AccountList.foreign_currency = currentRow.find("#hdForeignCurr").val();
        AccountList.ClosBL = cmn_ReplaceCommas(currentRow.find("#Balance").val());
        AccountList.conv_rate = cmn_ReplaceCommas(currentRow.find("#ExRate").val());
        AccountList.dr_amt_bs = cmn_ReplaceCommas(currentRow.find("#DebitAmountInBase").val());
        AccountList.dr_amt_sp = cmn_ReplaceCommas(currentRow.find("#DebitAmountInSpecific").val());
        AccountList.cr_amt_bs = cmn_ReplaceCommas(currentRow.find("#CreditAmountInBase").val());
        AccountList.cr_amt_sp = cmn_ReplaceCommas(currentRow.find("#CreditAmountInSpecific").val());
        AccountList.narr = currentRow.find("#Narration").val();
        AccountList.od_allow = cmn_ReplaceCommas(currentRow.find("#hd_od_allow").val());
        AccountList.od_limit = cmn_ReplaceCommas(currentRow.find("#hd_od_limit").val());
        AccountList.seq_no = SpanRowId;
        AccountDetailList.push(AccountList);

    });

    return AccountDetailList;
};
function AccountValidation() {
    debugger
    var RateDecDigit = $("#RateDigit").text()
    var ValDecDigit = $("#ValDigit").text();///Amount 
    var DocId = $("#DocumentMenuId").val();
    var Status = $("#hdContraStatus").val();

    var ErrorFlag = "N";
    if ($("#ContradetailTbl tbody tr").length > 0) {

        $("#ContradetailTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();
            var cflag = currentrow.find("#hdn_clag").val();

            var AccName = currentrow.find("#GLAccount_" + Rowno).val();

            var DbtAmnt;
            var CrdtAmnt;
            if (cflag == "B") {
                DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInBase").val());
                CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInBase").val());
            }
            if (cflag == "S") {
                DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
            }

            var Narrat = currentrow.find("#Narration").val().trim();

            /*Start Add by HIna on 27-10-2023 to credit Base Amount for only cash account with curr_id=1(INR) 
            * and credit specific Amount with CashA/c USD with curr_id!=1*/
            var AccType = currentrow.find("#hdAccType").val();
            var HdnCrBaseAmount = cmn_ReplaceCommas(currentrow.find("#HdnCrBaseAmnt").val());
            var HdnCrSpecificAmount = cmn_ReplaceCommas(currentrow.find("#HdnCrSpecficAmnt").val());
            var CurrId = currentrow.find("#hdCurrId").val();
            /*End */

            var Balance = cmn_ReplaceCommas(currentrow.find("#Balance").val());
            var OdAllow = cmn_ReplaceCommas(currentrow.find("#hd_od_allow").val());
            var OdLimit = cmn_ReplaceCommas(currentrow.find("#hd_od_limit").val());
            const MyArray = Balance.split(" ");
            var Bal1 = MyArray[0];
            var Bal = cmn_ReplaceCommas(Bal1);
            var BalType = MyArray[1];
            var FinBal = 0;
            if (OdAllow == "Y") {

                if (BalType == "Dr") {
                    FinBal = parseFloat(Bal) + parseFloat(OdLimit);
                }
                else {
                    FinBal = parseFloat(OdLimit) - parseFloat(Bal);
                }
            }
            else {
                if (BalType == "Dr") {
                    FinBal = parseFloat(Bal);
                }
                else {
                    FinBal = - parseFloat(Bal);
                }
            }
            debugger;
            if (DbtAmnt == "" || parseFloat(DbtAmnt) == parseFloat(0)) {
                var canflag = $("#Cancelled").is(':checked');
                //if (Status == "D" || Status == "") {
                if (canflag == false) {
                    if (Status == "D" && AccType == "8" && CurrId == "1") {
                        FinBal = parseFloat(FinBal) + parseFloat(HdnCrBaseAmount)
                    }
                    if (Status == "D" && AccType == "8" && CurrId != "1") {
                        FinBal = parseFloat(FinBal) + parseFloat(HdnCrSpecificAmount)
                    }
                    let hdnAccType = currentrow.find("#hdAccType").val().trim();
                    if (hdnAccType == "7" || hdnAccType == "8") {
                        if (parseFloat(CrdtAmnt) > parseFloat(FinBal)) {
                            if (cflag == "B") {
                                currentrow.find("#CreditAmountInBase").css("border-color", "red");
                                currentrow.find("#Crdt_Amnt_Error").text($("#ExceedingBalance").text());
                                currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                            }
                            if (cflag == "S") {
                                currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                                currentrow.find("#Crdt_Amnt_Error_sp").text($("#ExceedingBalance").text());
                                currentrow.find("#Crdt_Amnt_Error_sp").css("display", "block");
                            }

                            ErrorFlag = "Y";
                        }
                    }


                }
            }
            /*Add by Hina on 29-02-2024 to chk proper validation */
            var TotalAmt = "";
            if (DbtAmnt != "" && parseFloat(DbtAmnt) != parseFloat(0) || CrdtAmnt != "" && parseFloat(CrdtAmnt) != parseFloat(0)) {/*Add by Hina on 29-02-2024 to chk proper validation */
                TotalAmt = parseFloat(DbtAmnt) + parseFloat(CrdtAmnt);
            }


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
            if (parseFloat(TotalAmt) == parseFloat(0) || TotalAmt == "" || TotalAmt == "NaN") {
                debugger;
                if (CrdtAmnt == "" || CrdtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
                    if (cflag == "B") {
                        currentrow.find("#CreditAmountInBase").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                    }
                    if (cflag == "S") {
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error_sp").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error_sp").css("display", "block");
                    }
                    ErrorFlag = "Y";
                }
                if (DbtAmnt == "" || parseFloat(DbtAmnt) == parseFloat(0)) {
                    if (cflag == "B") {
                        currentrow.find("#DebitAmountInBase").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                    }
                    if (cflag == "S") {
                        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error_sp").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error_sp").css("display", "block");
                    }
                    ErrorFlag = "Y";
                }
            }


            if (Narrat == "") {
                currentrow.find("#Narration").css("border-color", "red");
                currentrow.find("#Narr_Error").text($("#valueReq").text());
                currentrow.find("#Narr_Error").css("display", "block");
                ErrorFlag = "Y";
            }
        });


    }
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

function NarratValidation() {
    debugger
    var DocId = $("#DocumentMenuId").val();
    if ($("#ContradetailTbl tbody tr").length > 0) {

        $("#ContradetailTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            currentrow.find("#Narration").css("border-color", "#ced4da");
            currentrow.find("#Narr_Error").text("");
            currentrow.find("#Narr_Error").css("display", "none");


        });


    }
}

function ForwardBtnClick() {
    debugger;

    //if (AccountValidation() == false) {
    //    debugger
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Approve").attr("data-target", "");
    //    return false;
    //}
    //else {
    //    var BPStatus = "";
    //    BPStatus = $('#hdContraStatus').val().trim();
    //    if (BPStatus === "D" || BPStatus === "F") {

    //        if ($("#hd_nextlevel").val() === "0") {
    //            $("#Btn_Forward").attr("data-target", "");
    //        }
    //        else {
    //            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //            $("#Btn_Approve").attr("data-target", "");
    //        }
    //        var Doc_ID = $("#DocumentMenuId").val();
    //        $("#OKBtn_FW").attr("data-dismiss", "modal");

    //        Cmn_GetForwarderList(Doc_ID);

    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "");
    //        $("#Btn_Forward").attr('onclick', '');
    //        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //    }
    //}
    //return false;

    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var Voudt = $("#VouDate").val();
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
                    if (AccountValidation() == false) {
                        debugger
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Approve").attr("data-target", "");
                        return false;
                    }
                    else {
                        var BPStatus = "";
                        BPStatus = $('#hdContraStatus').val().trim();
                        if (BPStatus === "D" || BPStatus === "F") {

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
    VouNo = $("#VouNo").val();
    VouDate = $("#VouDate").val();
    $("#hdDoc_No").val(VouNo);
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (VouNo + ',' + VouDate + ',' + WF_Status1);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
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
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ContraVoucher_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/Contra/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }

    if (fwchkval === "Forward") {
        if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Contra/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/Contra/ContraApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Contra/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Contra/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(vNo, vDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/Contra/SavePdfDocToSendOnEmailAlert",
//        data: { vNo: vNo, vDate: vDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#VouNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function OnClickCancelFlag() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertContraDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}


function RemoveSession() {
    sessionStorage.removeItem("BillDetailSession");
    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.removeItem("EditBtnClick");
}
function EnableEditkBtnVou() {
    debugger;
    sessionStorage.setItem("EditBtnClick", "Y");
    //sessionStorage.removeItem("BillDetailSession");
    return true;
}
function DisableDetail(Status) {
    debugger;
    var sessionval = sessionStorage.getItem("EditBtnClick");
    if (Status == "") {
        sessionval = "Y";
    }
    if (Status == "D" || Status == "") {
        $('#BillDetailTbl tbody tr').each(function () {
            var currentrow = $(this);
            if (sessionval == "Y") {
                currentrow.find("#PayAmount").attr("disabled", false);
            }
            else {
                currentrow.find("#PayAmount").attr("disabled", true);
            }
        });

        //if (sessionval == "Y") {
        //    $("#SaveAndExitBtn").attr("disabled", false);
        //    $("#DiscardAndExit").attr("disabled", false); 
        //}
        //else {
        //    $("#SaveAndExitBtn").attr("disabled", true);
        //    $("#DiscardAndExit").attr("disabled", true);            
        //}
    }
    else {
        $('#BillDetailTbl tbody tr').each(function () {
            var currentrow = $(this);
            currentrow.find("#PayAmount").attr("disabled", true);
        });
        //$("#SaveAndExitBtn").attr("disabled", true);
        //$("#DiscardAndExit").attr("disabled", true);
    }
}


/*----This validation for if EntityName has select option then Account table validation should be remove----*/

function validationofRemoveAcountSecOnChngHeadSec() {
    var DocId = $("#DocumentMenuId").val();
    if ($("#ContradetailTbl tbody tr").length > 0) {

        $("#ContradetailTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();

            currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
            currentrow.find("#AccountNameError").text("");
            currentrow.find("#AccountNameError").css("display", "none");

            currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("");
            currentrow.find("#Crdt_Amnt_Error").css("display", "none");

            currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("");
            currentrow.find("#Dbt_Amnt_Error").css("display", "none");

            currentrow.find("#Narration").css("border-color", "#ced4da");
            currentrow.find("#Narr_Error").text("");
            currentrow.find("#Narr_Error").css("display", "none");
        });


    }
}
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
