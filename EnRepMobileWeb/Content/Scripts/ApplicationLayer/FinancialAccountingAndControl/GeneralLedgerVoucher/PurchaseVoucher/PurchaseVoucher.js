$(document).ready(function () {
    debugger;
    BindSuppAccList();
    BindDDLAccountList();
    PVNo = $("#PVNo").val();
    $("#hdDoc_No").val(PVNo);
    $('#PVdetailTbl').on('click', '.deleteIcon', function () {
        var txtDisable = $("#txtdisable").val();
        if (txtDisable != "Y") {
            var hfAccID = $(this).closest('tr').find('#hfAccID').val();
            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
            }
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
        }
    });
    GetViewDetails();
    CancelledRemarks("#Cancelled", "Disabled");
});
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
function deleteCCdetail(hfAccID) {
    debugger;
    if ($("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").length > 0) {
        $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").closest('tr').remove();
    }

}
function GetViewDetails() {
    debugger;
    if ($("#hdBillWiseAdjDetailList").val() != null && $("#hdBillWiseAdjDetailList").val() != "") {
        debugger
        var arr2 = $("#hdBillWiseAdjDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdBillWiseAdjDetailList").val("");
        sessionStorage.removeItem("BillDetailSession");
        sessionStorage.setItem("BillDetailSession", JSON.stringify(arr));
    }
}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#PVdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function BindDDLAccountList() {
    debugger;

    Cmn_BindAccountList("#GLAccount_", "1", "#PVdetailTbl", "#SNohf", "BindData", "105104115140");

}
function BindData() {
    debugger;

    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#PVdetailTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                debugger;
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#PVdetailTbl >tbody >tr").length) {
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
                        if (check(data, selected, "#PVdetailTbl", "#SNohf", "#GLAccount_") == true) {
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
    $("#PVdetailTbl >tbody >tr").each(function (i, row) {
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
function BindSuppAccList() {
    debugger;
    $("#ddlSuppAccName").select2({
        ajax: {
            url: $("#hdSuppList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term // search term like "a" then "an"
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
function OnChangeSuppCoa() {
    debugger;
    var RateDecDigit = $("#RateDigit").text()
    var ExchDecDigit = $("#ExchDigit").text()
    var coa_id = $("#ddlSuppAccName").val();
    if (coa_id == "" || coa_id == "0" || coa_id == null) {
        $("[aria-labelledby='select2-ddlSuppAccName-container']").css("border-color", "red");
        $("#vmSuppAccName").text($("#valueReq").text());
        $("#vmSuppAccName").css("display", "block");
        $("#ddlSuppAccName").css("border-color", "red");

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

        validationofRemoveAcountSecOnChngHeadSec();
    }
    else {
        $("#vmSuppAccName").css("display", "none");
        //$("#ddlSuppAccName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlSuppAccName-container']").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");
    }

    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/PurchaseVoucher/GetAccCurr",
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
                        //$("#ConvRate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                        $("#ConvRate").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));
                        $("#hdSuppAccId").val(arr.Table3[0].acc_id);

                    }
                    else if (arr.Table1.length > 0) {
                        $("#curr").val(arr.Table1[0].curr_id);
                        $("#hdcurr").val(arr.Table1[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));

                        $("#ConvRate").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#hdSuppAccId").val(arr.Table3[0].acc_id);
                        $("#ConvRate").attr("readonly", true);
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
function OnChangeConvRate(e) {
    debugger;

    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text()

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
       // var ConvRate2 = parseFloat(ConvRate).toFixed(ExchDecDigit);
        var ConvRate2 = toDecimal(cmn_ReplaceCommas(CrdtAmnt), ExchDecDigit);
        var ConvRate3 = cmn_addCommas(ConvRate2);
        $("#ConvRate").val(ConvRate3);
        //$("#ConvRate").val(parseFloat(ConvRate).toFixed(RateDecDigit));
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
    }
    $("#PVdetailTbl tbody tr").each(function () {
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
            //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#CreditAmountInBase").val(FinVal);
        }
        if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            var FAmt = debitAmt * ConvRate;
           // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(cmn_ReplaceCommas(FAmt), ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#DebitAmountInBase").val(FinVal);
        }
    })
    /*Add by Hina on 30-10-2023 to show debit credit  total amount should be base amount instead of specific amount*/
    CalculateTotalDrCrValue()
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    //$("#ItemCostError").css("display", "none");
    //$("#ItemCost").css("border-color", "#ced4da");   

    return true;
}
function OnchangeSrcType() {
    debugger;
    var SrcType = $("#Src_Type").val();
    if (SrcType == "D") {
        $("#srcno").css("display", "none");
        $("#srcdt").css("display", "none");
    }
    if (SrcType == "P") {
        $("#srcno").css("display", "block");
        $("#srcdt").css("display", "block");
    }

}
function AddSuppDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount 
    if ($("#duplicateBillNo").val() == "Y") {
        $("#BillNo").css("border-color", "Red");
        $('#BillNoError').text($("#valueduplicate").text());
        $("#BillNoError").css("display", "block");
        return false;
    }
    else {
        $("#BillNoError").css("display", "none");
        $("#BillNo").css("border-color", "#ced4da");
    }
    if (HeaderValidation() == false) {
        return false;
    }
    var SuppAccID = $("#ddlSuppAccName").val();
    var ConvRate = $("#ConvRate").val();
    var VouDate = $("#PVDate").val();/*add by Hina Sharma on 27-08-2025*/
    var CC_Acc_id = SuppAccID.substring(0, 1);
    if (CC_Acc_id == "3" || CC_Acc_id == "4") {
        $("#BtnCostCenterDetail").attr("disabled", false);
        $("#BtnCostCenterDetail").removeClass("subItmImg");
    }
    else {
        $("#BtnCostCenterDetail").attr("disabled", true);
        $("#BtnCostCenterDetail").addClass("subItmImg");
    }
    if (SuppAccID != "0" && SuppAccID != "" && SuppAccID != null && ConvRate != "" && ConvRate != null) {
        $("#Plusbankdt").css('display', 'none');
        $("#BtnAddGL").css('display', 'block');
        debugger;
        $("#ddlSuppAccName").prop("disabled", true);
        $("#PVHeaderNarration").attr("disabled", true);

        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/PurchaseVoucher/GetSuppAccIDDetail",
                data: {
                    SuppAccID: SuppAccID,
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
                                $("#PVdetailTbl >tbody >tr").each(function () {
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
                                        if (arr.Table[i].dr_amt_sp == "0") {
                                            currentRow.find("#DebitAmountInSpecific").val("");
                                        }
                                        else {
                                          //  currentRow.find("#DebitAmountInSpecific").val(parseFloat(arr.Table[i].dr_amt_sp).toFixed(ValDecDigit));
                                            currentRow.find("#DebitAmountInSpecific").val(toDecimal(arr.Table[i].dr_amt_sp, ValDecDigit));
                                        }
                                        if (arr.Table[i].dr_amt_bs == "0") {
                                            currentRow.find("#DebitAmountInBase").val("");
                                        }
                                        else {
                                           // currentRow.find("#DebitAmountInBase").val(parseFloat(arr.Table[i].dr_amt_bs).toFixed(ValDecDigit));
                                            currentRow.find("#DebitAmountInBase").val(toDecimal(arr.Table[i].dr_amt_bs, ValDecDigit));
                                        }
                                        if (arr.Table[i].cr_amt_sp == "0") {
                                            currentRow.find("#CreditAmountInSpecific").val("");
                                        }
                                        else {
                                            //currentRow.find("#CreditAmountInSpecific").val(parseFloat(arr.Table[i].cr_amt_sp).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInSpecific").val(toDecimal(arr.Table[i].cr_amt_sp, ValDecDigit));
                                        }
                                        if (arr.Table[i].cr_amt_bs == "0") {
                                            currentRow.find("#CreditAmountInBase").val("");
                                        }
                                        else {
                                           // currentRow.find("#CreditAmountInBase").val(parseFloat(arr.Table[i].cr_amt_bs).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInBase").val(toDecimal(arr.Table[i].cr_amt_bs, ValDecDigit));
                                        }
                                        currentRow.find("#Narration").val($("#PVHeaderNarration").val());
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
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var rowCount = $('#PVdetailTbl >tbody >tr').length + 1
    var RowNo = 0;
    $("#PVdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#PVdetailTbl tbody').append(`<tr id="${++rowIdx}">
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
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','PVDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"> </button></div>

</td>

<td><textarea id="GLGroup" class="form-control remarksmessage" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="GLGroup" placeholder="${$("#AccGroup").text()}" readonly></textarea>
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
<textarea id="Narration" class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}">${$("#PVHeaderNarration").val()}</textarea>
<span id="Narr_Error" class="error-message is-visible"></span>
 </div>
</td>  
 <td class="center">
      <button type="button" disabled id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
 </td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
    //<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form" style="padding:0px;">
    //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
    //    <input type="hidden" id="hfAccID" style="display: none;" />
    //    <input type="hidden" id="hdAccType" style="display: none;" />
    //    <span id="AccountNameError" class="error-message is-visible"></span></div>
    //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','PVDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
    //</td>
    BindAcountList(RowNo);
}
function BindAcountList(ID) {
    debugger;

    Cmn_BindAccountList("#GLAccount_", ID, "#PVdetailTbl", "#SNohf", "", "105104115140");

}
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
    if (Acc_ID != "") {
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
    CalculateTotalDrCrValue();
    Cmn_BindGroup(clickedrow, Acc_ID, "")
    /*-----------Code Start Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
    var VouDate = $("#PVDate").val();
    CMN_BindAccountBalance(clickedrow, Acc_ID, VouDate);
    /*-----------Code End Add by Hina Sharma on 27-08-2025 to get account Balance Account wise-------- */
}

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
    var debitAmt1 = currentrow.find("#DebitAmountInSpecific").val();
    var debitAmt = cmn_ReplaceCommas(debitAmt1);
    if (AvoidDot(debitAmt) == false) {
        debitAmt = "";
        currentrow.find("#DebitAmountInSpecific").val("")
    }
    else {
        //var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit)
        var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDecDigit);
        var debitAmt3 = cmn_addCommas(debitAmt2);
        currentrow.find("#DebitAmountInSpecific").val(debitAmt3);
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

    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);

    if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        var FAmt = debitAmt * ConvRate;
       // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
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
    /* if (DocId == '105104115120') {*/
    if (AvoidDot(creditAmt) == false) {
        creditAmt = "";
        currentrow.find("#CreditAmountInSpecific").val("")
    }
    else {
      //  var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit)
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
    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);

    if ((creditAmt !== 0 || creditAmt !== null || creditAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        var FAmt = creditAmt * ConvRate;
      // var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
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
    $("#PVdetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        /*Commented by HIna on 28-10-2023 to changes for Base Amount not for specific*/
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
    //var debitAmount2 = parseFloat(debitAmount).toFixed(ValDigit);
    var debitAmount2 = toDecimal(cmn_ReplaceCommas(debitAmount), ValDigit);
    var debitAmount1 = cmn_addCommas(debitAmount2);
    $("#dbtTtlAmnt").text(debitAmount1);
    //$("#dbtTtlAmnt").text(parseFloat(debitAmount).toFixed(ValDigit));
   // var creditAmount2 = parseFloat(creditAmount).toFixed(ValDigit);
    var creditAmount2 = toDecimal(cmn_ReplaceCommas(creditAmount), ValDigit);
    var creditAmount1 = cmn_addCommas(creditAmount2);
    $("#crdtTtlAmnt").text(creditAmount1);
    //$("#crdtTtlAmnt").text(parseFloat(creditAmount).toFixed(ValDigit));

    $("#AccountBalance").val(parseFloat(0).toFixed(ValDigit));
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
 //   var FinalDiff2 = parseFloat(FinalDiff).toFixed(ValDigit);
    var FinalDiff2 = toDecimal(cmn_ReplaceCommas(FinalDiff), ValDigit);
    var FinalDiff1 = cmn_addCommas(FinalDiff2);
    $("#DiffrncAmnt").text(FinalDiff1);
    //$("#DiffrncAmnt").text(parseFloat(FinalDiff1).toFixed(ValDigit));
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function InsertPVDetail() {
    debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        if ($("#duplicateBillNo").val() == "Y") {
            $("#BillNo").css("border-color", "Red");
            $('#BillNoError').text($("#valueduplicate").text());
            $("#BillNoError").css("display", "block");
            return false;
        }
        else {
            $("#BillNoError").css("display", "none");
            $("#BillNo").css("border-color", "#ced4da");
        }
        var SrcType = $("#Src_Type").val();
        $("#hdSrc_Type").val(SrcType);
        if (HeaderValidation() == false) {
            return false;
        }
        if (AccountValidation() == false) {
            return false;
            swal("", $("#NoGLAccountSelected").text(), "warning");
        }
        if (saveDrCrEqualAmnt() == false) {
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
        FinalAccountDetail = InsertPVAccountDetails();
        var AccountDt = JSON.stringify(FinalAccountDetail);
        $('#hdAccountDetailList').val(AccountDt);
        CalculateTotalDrCrValue();

        if (Cmn_CC_DtlSaveButtonClick("PVdetailTbl", "DebitAmountInSpecific", "CreditAmountInSpecific","hdPVStatus") == false) {
            return false;
        }

        var FinalCostCntrDetails = [];
        FinalCostCntrDetails = Cmn_InsertCCDetails();
        var CCDetails = JSON.stringify(FinalCostCntrDetails);
        $('#hdn_CC_DetailList').val(CCDetails);

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
        /*----- Attatchment End--------*/
        /*----- Attatchment start--------*/
        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        var creditAmount1 = $("#hdnVouAmnt").val();
        $("#hdnVouAmnt").val(cmn_ReplaceCommas(creditAmount1));
        RemoveSession();
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    catch (ex) {
        console.log(ex);
        $("#hdnsavebtn").val("");
        return false
    }
};
function saveDrCrEqualAmnt() {
    debugger;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    var dbTotlAmnt = $("#dbtTtlAmnt").text();
    var crTotAmnt = $("#crdtTtlAmnt").text();
    debugger;
    if (dbTotlAmnt == crTotAmnt && dbTotlAmnt != "0" && crTotAmnt != "0") {
        var difference = 0;
       // $("#DiffrncAmnt").text(parseFloat(difference).toFixed(ValDigit));
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
function HeaderValidation() {
    debugger;
    var ErrorFlag = "N";
    var coa_id = $("#ddlSuppAccName").val();
    if (coa_id == "" || coa_id == "0" || coa_id == null) {
        $("#vmSuppAccName").css("display", "block");
        $("[aria-labelledby='select2-ddlSuppAccName-container']").css("border-color", "red");
        $("#vmSuppAccName").text($("#valueReq").text());

        ErrorFlag = "Y";
    }
    else {
        $("#vmSuppAccName").css("display", "none");
        $("#ddlSuppAccName").css("border-color", "#ced4da");
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
    //Added by Suraj on 24-04-2024 for validate bill no and bill date
    if (CheckVallidation("BillNo", "BillNoError","0")==false) {
        ErrorFlag = "Y";
    }
    if (CheckVallidation("BillDate", "BillDateError") == false) {
        ErrorFlag = "Y";
    }
   
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertPVAccountDetails() {
    debugger;
    var AccountDetailList = new Array();
    $("#PVdetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var SpanRowId = currentRow.find("#SpanRowId").text();
        var AccountList = {};
        /*commented by Hina on 02-07-2024 to seprate acc_id and SpanRowId as seq_no */
        /* AccountList.acc_id = (currentRow.find("#GLAccount_" + rowid).val()) + '_' + SpanRowId;*/
        AccountList.acc_id = currentRow.find("#GLAccount_" + rowid).val();
        AccountList.supp_acc_id = $("#hdSuppAccId").val();
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
function AccountValidation() {
    debugger
    var RateDecDigit = $("#RateDigit").text()
    var ValDecDigit = $("#ValDigit").text();///Amount 
    var DocId = $("#DocumentMenuId").val();
    var ErrorFlag = "N";
    if ($("#PVdetailTbl tbody tr").length > 0) {

        $("#PVdetailTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();
            if (Rowno == 1) {
                var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                var Narrat = currentrow.find("#Narration").val();
                if (AccName == "0" || AccName == "") {
                    swal("", $("#NoGLAccountSelected").text(), "warning");
                    ErrorFlag = "Y";
                }
                if (ErrorFlag == "Y") {
                    return false;
                }
                if (CrdtAmnt == "" || CrdtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
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

            if (Rowno > 1) {
                var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                var Narrat = currentrow.find("#Narration").val();
                if (AccName == "0") {
                    
                    currentrow.find("#AccountNameError").text($("#valueReq").text());
                    currentrow.find("#AccountNameError").css("display", "block");
                    currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "red");
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

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function NarratValidation() {
    debugger
    var DocId = $("#DocumentMenuId").val();
    if ($("#PVdetailTbl tbody tr").length > 0) {
        $("#PVdetailTbl tbody tr").each(function () {
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
async function OnchangeBillNo() {
    debugger;
    const Bill_No = $("#BillNo").val();
    const Bill_Date = $("#BillDate").val();
    const DocumentMenuId = $("#DocumentMenuId").val();
    const supp_id = $("#ddlSuppAccName").val();
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
    CheckVallidation("BillNo", "BillNoError", "0");//to check null, blank or 0
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ServicePurchaseInvoice/CheckDuplicateBillNo",
        data: { supp_id: supp_id, Bill_No, doc_id: DocumentMenuId, bill_dt: Bill_Date },
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
                    showError("#BillNo", "#BillNoError");
                    showError("#BillDate", "#BillDateError");
                    $("#duplicateBillNo").val("Y");
                } else if (hdn_Bill_Dt !== Bill_Date) {
                    showError("#BillNo", "#BillNoError");
                    showError("#BillDate", "#BillDateError");
                    $("#duplicateBillNo").val("Y");
                } else {
                    clearError("#BillNo", "#BillNoError");
                    clearError("#BillDate", "#BillDateError");
                    $("#duplicateBillNo").val("N");
                }
            } else {
                clearError("#BillNo", "#BillNoError");
                clearError("#BillDate", "#BillDateError");
                $("#duplicateBillNo").val("N");
            }
        }
    });
}
//function OnchangeBillNo() {
//    CheckVallidation("BillNo", "BillNoError","0");//to check null, blank or 0
//}
function BillDateValidation() {
    debugger;
    CheckVallidation("BillDate", "BillDateError");//to check null, blank or 0
    Cmn_VoucherDateValidation("#BillDate");
    OnchangeBillNo();
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
function ForwardBtnClick() {
    debugger;
    //var PVStatus = "";
    //PVStatus = $('#hdPVStatus').val().trim();
    //if (PVStatus === "D" || PVStatus === "F") {

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
        var Voudt = $("#PVDate").val();
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
                    var PVStatus = "";
                    PVStatus = $('#hdPVStatus').val().trim();
                    if (PVStatus === "D" || PVStatus === "F") {

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

    docid = $("#DocumentMenuId").val();
    VouNo = $("#PVNo").val();
    VouDate = $("#PVDate").val();
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
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    var pdfAlertEmailFilePath = 'PV_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
            debugger;
           await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }

    debugger;

    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/PurchaseVoucher/PurchaseVoucherApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/PurchaseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseVoucher/SavePdfDocToSendOnEmailAlert",
        data: { poNo: poNo, poDate: poDate, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#PVNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertPVDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function VoucherDateValidation() {
    debugger;

    Cmn_VoucherDateValidation("#PVDate");

}
function RemoveSession() {
    sessionStorage.removeItem("BillDetailSession");
    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.removeItem("EditBtnClick");
}
function EnableEditkBtnVou() {
    debugger;
    sessionStorage.setItem("EditBtnClick", "Y");
    return true;
}

function validationofRemoveAcountSecOnChngHeadSec() {

    if ($("#PVdetailTbl tbody tr").length > 0) {
        $("#PVdetailTbl tbody tr").each(function () {
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
        //var amount1 = toDecimal(cmn_ReplaceCommas(amount), ValDigit)
        List.CC_Amount = cmn_addCommas(toDecimal(cmn_ReplaceCommas(amount), ValDigit));
        TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amount);//add by sm 09-12-2024
        NewArr.push(List);
    });
    var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 09-12-2024
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
            $("#hdnTable_Id").text("PVdetailTbl");
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