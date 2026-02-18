/*toDecimal is added by Suraj on 13-02-2024*/
$(document).ready(function () {
    debugger;
    hdnAccGrpval();
    BindBankAccList();
    BindDDLAccountList();
    var InstrumentType = $("#ins_type").val();
    if (InstrumentType == "O") {
        $("#RequiredInstNumber").css("display", "none");
        $("#RequiredInstDate").css("display", "none");
    }
    else {
        $("#RequiredInstNumber").css("display", "inherit");
        $("#RequiredInstDate").css("display", "inherit");
    }
    BPNo = $("#BankPaymentNo").val();
    $("#hdDoc_No").val(BPNo);
    $('#BPdetailTbl').on('click', '.deleteIcon', function () {
        var txtDisable = $("#txtdisable").val();
        if (txtDisable != "Y") {
            var hfAccID = $(this).closest('tr').find('#hfAccID').val();
            var hfIntBrId = $(this).closest('tr').find('#hfGlBranchID').val();
            var child = $(this).closest('tr').nextAll();
            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
                DeletebillAdjustment(hfAccID, hfIntBrId);
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
    sessionStorage.removeItem("BillDetailSession");
    GetViewDetails();

    CancelledRemarks("#Cancelled", "Disabled");
});
function deleteCCdetail(hfAccID) {
    debugger;
    if ($("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").length > 0) {
        $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").closest('tr').remove();
    }

}
function hdnAccGrpval() {
    debugger;
    $("#BPdetailTbl > tbody > tr").each(function () {
        debugger;
        var CurrRow = $(this);
        var hdngrpval_id = CurrRow.find("#hdnAccGrpval3_4").val();
        if (hdngrpval_id == "3" || hdngrpval_id == "4") {
            CurrRow.find("#BtnCostCenterDetail").attr("disabled", false);
            CurrRow.find("#BtnCostCenterDetail").removeClass("subItmImg");
        }
        else {
            CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
            CurrRow.find("#BtnCostCenterDetail").addClass("subItmImg");
        }
    })

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
    fetch_BranchList().then((arr) => {
        $("#BPdetailTbl tbody tr").each(function () {
            var row = $(this);
            var rowNo = row.find("#SNohf").val();
            var GlBrId = row.find("#GLbranch_" + rowNo).val();
            bindGlBranchList(rowNo, arr);
            //let BrId = $("#BrId").text();
            $("#GLbranch_" + rowNo).val(GlBrId);
        });
        
    });
}
var ValDecDigit = $("#ValDigit").text();
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#BPdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function BindDDLAccountList() {
    debugger;
    if ($("#hdBPStatus").val() != "A") {
        /*Cmn_BindAccountList("#GLAccount_", "1", "#BPdetailTbl", "#SNohf", "BindData", "105104115120");*/

        $("#BPdetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var rowid = currentRow.find("#SNohf").val();
            //rowid = parseFloat(rowid) + 1;
            //if (rowid > $("#BPdetailTbl >tbody >tr").length) {
            //    return false;
            //} 
            var AccID = currentRow.find("#hfAccID").val();
            var glBrId = currentRow.find("#GLbranch_" + rowid).val();
            Cmn_BindAccountListAsync("#GLAccount_", rowid, "#BPdetailTbl", "#SNohf", "", "105104115120", glBrId);
            //$("#GLAccount_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}' data-date='${$("#span_State").text()}' data-value='${$("#span_City").text()}'}'></optgroup>`);
            //for (var i = 0; i < AccountListData.length; i++) {
            //    if (AccountListData[i].stateName == null) {
            //        AccountListData[i].stateName = "";
            //    }
            //    if (AccountListData[i].cityNmae == null) {
            //        AccountListData[i].cityNmae = "";
            //    }
            //    $('#Textddl' + rowid).append(`<option data-date="${AccountListData[i].stateName}" data-value="${AccountListData[i].cityNmae}" value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
            //}
            //var firstEmptySelect = true;
            //$("#GLAccount_" + rowid).select2({

            //    templateResult: function (data) {
            //        var selected = $("#GLAccount_" + rowid).val();
            //        if (check(data, selected, "#BPdetailTbl", "#SNohf", "#GLAccount_") == true) {
            //            var DocDate = $(data.element).data('date');
            //            var value = $(data.element).data('value');
            //            var classAttr = $(data.element).attr('class');
            //            var hasClass = typeof classAttr != 'undefined';
            //            classAttr = hasClass ? ' ' + classAttr : '';
            //            var $result = $(
            //                '<div class="row bpbr">' +
            //                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
            //                '<div class="col-md-3 col-xs-3' + classAttr + '">' + DocDate + '</div>' +
            //                '<div class="col-md-3 col-xs-3' + classAttr + '">' + value + '</div>' +
            //                '</div>'
            //            );
            //            return $result;
            //        }
            //        firstEmptySelect = false;
            //    }
            //});

        });

    }
    
}
function BindData() {
    debugger;
    //var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    //if (AccountListData != null) {
    //    if (AccountListData.length > 0) {
    //        $("#BPdetailTbl >tbody >tr").each(function () {
    //            var currentRow = $(this);
    //            var rowid = currentRow.find("#SNohf").val();
    //            rowid = parseFloat(rowid) + 1;
    //            if (rowid > $("#BPdetailTbl >tbody >tr").length) {
    //                return false;
    //            }
    //            $("#GLAccount_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}' data-date='${$("#span_State").text()}' data-value='${$("#span_City").text()}'}'></optgroup>`);
    //            for (var i = 0; i < AccountListData.length; i++) {
    //                if (AccountListData[i].stateName == null) {
    //                    AccountListData[i].stateName = "";
    //                }
    //                if (AccountListData[i].cityNmae == null) {
    //                    AccountListData[i].cityNmae = "";
    //                }
    //                $('#Textddl' + rowid).append(`<option data-date="${AccountListData[i].stateName}" data-value="${AccountListData[i].cityNmae}" value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
    //            }
    //            var firstEmptySelect = true;
    //            $("#GLAccount_" + rowid).select2({

    //                templateResult: function (data) {
    //                    var selected = $("#GLAccount_" + rowid).val();
    //                    if (check(data, selected, "#BPdetailTbl", "#SNohf", "#GLAccount_") == true) {
    //                        var DocDate = $(data.element).data('date');
    //                        var value = $(data.element).data('value');
    //                        var classAttr = $(data.element).attr('class');
    //                        var hasClass = typeof classAttr != 'undefined';
    //                        classAttr = hasClass ? ' ' + classAttr : '';
    //                        var $result = $(
    //                            '<div class="row bpbr">' +
    //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                            '<div class="col-md-3 col-xs-3' + classAttr + '">' + DocDate + '</div>' +
    //                            '<div class="col-md-3 col-xs-3' + classAttr + '">' + value + '</div>' +
    //                            '</div>'
    //                        );
    //                        return $result;
    //                    }
    //                    firstEmptySelect = false;
    //                }
    //            });

    //        });
    //    }
    //}
    //$("#BPdetailTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var AccID = currentRow.find("#hfAccID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#GLAccount_' + rowid;
    //    if (AccID != '0' && AccID != "") {
    //        currentRow.find("#GLAccount_" + rowid).attr("onchange", "");
    //        currentRow.find("#GLAccount_" + rowid).val(AccID).trigger('change.select2');
    //        currentRow.find("#GLAccount_" + rowid).attr("onchange", "OnChangeAccountName(this, event)");
    //    }
    //});
}
//function BindData() {
//    debugger;
//    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
//    if (AccountListData != null) {
//        if (AccountListData.length > 0) {
//            $("#BPdetailTbl >tbody >tr").each(function () {
//                var currentRow = $(this);
//                var rowid = currentRow.find("#SNohf").val();
//                rowid = parseFloat(rowid) + 1;
//                if (rowid > $("#BPdetailTbl >tbody >tr").length) {
//                    return false;
//                }
//                $("#GLAccount_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
//                for (var i = 0; i < AccountListData.length; i++) {
//                    $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
//                }
//                var firstEmptySelect = true;
//                $("#GLAccount_" + rowid).select2({

//                    templateResult: function (data) {
//                        var selected = $("#GLAccount_" + rowid).val();
//                        if (check(data, selected, "#BPdetailTbl", "#SNohf", "#GLAccount_") == true) {
//                            /* var UOM = $(data.element).data('uom');*/
//                            var classAttr = $(data.element).attr('class');
//                            var hasClass = typeof classAttr != 'undefined';
//                            classAttr = hasClass ? ' ' + classAttr : '';
//                            var $result = $(
//                                '<div class="row">' +
//                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                                /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
//                                '</div>'
//                            );
//                            return $result;
//                        }
//                        firstEmptySelect = false;
//                    }
//                });

//            });
//        }
//    }
//    $("#BPdetailTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var AccID = currentRow.find("#hfAccID").val();
//        var rowid = currentRow.find("#SNohf").val();
//        var AccountID = '#GLAccount_' + rowid;
//        if (AccID != '0' && AccID != "") {
//            currentRow.find("#GLAccount_" + rowid).attr("onchange", "");
//            currentRow.find("#GLAccount_" + rowid).val(AccID).trigger('change.select2');
//            currentRow.find("#GLAccount_" + rowid).attr("onchange", "OnChangeAccountName(this, event)");
//        }
//    });
//}
function OnchangeInstrumentType() {
    debugger;
    var InstrumentType = $("#ins_type").val();
    if (InstrumentType == "O") {
        $("#RequiredInstNumber").css("display", "none");
        $("#RequiredInstDate").css("display", "none");
    }
    else {
        $("#RequiredInstNumber").css("display", "inherit");
        $("#RequiredInstDate").css("display", "inherit");
    }
    $("#vm_ins_no").css("display", "none");
    $("#ins_no").css("border-color", "#ced4da");

    $("#vm_ins_dt").css("display", "none");
    $("#ins_dt").css("border-color", "#ced4da");
}
function BindBankAccList() {
    
    $("#ddlBankAccName").select2({
        ajax: {
            url: $("#hdBankList").val(),
            data: function (params) {
                var queryParameters = {
                    BankName: params.term // search term like "a" then "an"
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
function OnChangeBankCoa() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();

    var coa_id = $("#ddlBankAccName").val();
    var YouDate = $("#BPDate").val();
    if (coa_id == "" || coa_id == "0" || coa_id == null) {
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "red");
       $("#vmBankAccName").text($("#valueReq").text());
       $("#vmBankAccName").css("display", "block");
        $("#ddlBankAccName").css("border-color", "red");

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
        $("#vmBankAccName").css("display", "none");
        //$("#ddlBankAccName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");
    }

    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/BankPayment/GetAccCurr",
            data: {
                acc_id: coa_id,
                Date: YouDate,
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
                        $("#AccountBalance").val(arr.Table2[0].Avl_bal);
                            $("#hd_od_limit").val(arr.Table2[0].OdLimit);
                            $("#hd_od_allow").val(arr.Table3[0].od_allow);
                        $("#hdnBankName").val(arr.Table3[0].acc_id);
                    }
                    else {
                        $("#AccountBalance").val(parseFloat(0).toFixed(RateDecDigit));
                    }
                    if (arr.Table.length > 0) {
                        $("#curr").val(arr.Table[0].curr_id);
                        $("#hdcurr").val(arr.Table[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                        $("#ConvRate").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table[0].conv_rate, ExchDecDigit));

                    }
                   else if (arr.Table1.length > 0) {
                        $("#curr").val(arr.Table1[0].curr_id);
                        $("#hdcurr").val(arr.Table1[0].curr_id);
                        //$("#ConvRate").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));
                        //$("#hdconv").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));
                        $("#ConvRate").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#hdconv").val(toDecimal(arr.Table1[0].conv_rate, ExchDecDigit));
                        $("#ConvRate").attr("readonly", true);
                    }
                    else {
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
    var ExchDecDigit = $("#ExchDigit").text();

    var ConvRate12 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate12);

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
        //var ConvRate2 = parseFloat(ConvRate).toFixed(ExchDecDigit);
        var ConvRate2 = toDecimal(ConvRate, ExchDecDigit);
        var ConvRate1 = cmn_addCommas(ConvRate2);
        $("#ConvRate").val(ConvRate1);
        //$("#ConvRate").val(parseFloat(ConvRate1).toFixed(RateDecDigit));
            $("#ConvRateError").css("display", "none");
            $("#ConvRate").css("border-color", "#ced4da");
        }
    $("#BPdetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
        var debitAmt1 = currentrow.find("#DebitAmountInSpecific").val();
        var debitAmt = cmn_ReplaceCommas(debitAmt1);
        var creditAmt1 = currentrow.find("#CreditAmountInSpecific").val();
        var creditAmt = cmn_ReplaceCommas(creditAmt1);
        //var debitAmt = currentrow.find("#DebitAmountInBase").val();
        //var creditAmt = currentrow.find("#CreditAmountInBase").val();
        if (AvoidDot(debitAmt) == false) {
            debitAmt = 0;
            //DbtAmnt = 0;
        }
        if (AvoidDot(creditAmt) == false) {
            creditAmt = 0;
            //CrdtAmnt = 0;
        }
        if ((creditAmt !== 0 || creditAmt !== null || creditAmt !== "" ) && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
            var FAmt = parseFloat(creditAmt) * parseFloat(ConvRate);
           /* var FAmt = creditAmt / ConvRate;*/
            //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(FAmt, ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#CreditAmountInBase").val(FinVal);
            /*currentrow.find("#CreditAmountInSpecific").val(FinVal);*/

        }
        if ((debitAmt !== 0 || debitAmt !== null || debitAmt !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
            debugger;
            /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
            var FAmt = parseFloat(debitAmt) * parseFloat(ConvRate);
            /*var FAmt = debitAmt / ConvRate;*/
            //var FinVal1 = parseFloat(FAmt).toFixed(ValDecDigit);
            var FinVal1 = toDecimal(FAmt, ValDecDigit);
            var FinVal = cmn_addCommas(FinVal1);
            currentrow.find("#DebitAmountInBase").val(FinVal);
            /*currentrow.find("#DebitAmountInSpecific").val(FinVal);*/

        }
    })
    /*Add by hina on 27-10-2023 for the base amount in Total debit and total credit*/
    CalculateTotalDrCrValue();
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
function AddBankDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount 
    if (HeaderValidation() == false) {
        return false;
    }
    var bankid = $('#ddlBankAccName').val();
    var ConvRate = $("#ConvRate").val();
     if (bankid != "0" && bankid != "" && bankid != null && ConvRate != "" && ConvRate != null) {
        
        $("#Plusbankdt").css('display', 'none');
        $("#BtnAddGL").css('display', 'block');
        debugger;
        $("#ddlBankAccName").prop("disabled", true);
        $("#Src_Type").prop("disabled", true);       
        $("#BpHeaderNarration").attr("disabled", true);
        var BankAccID = $("#ddlBankAccName").val();
         $("#InterBranch").attr("disabled", true);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/BankPayment/GetBankAccIDDetail",
                data: {
                    BankAccID: BankAccID,
                   
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
                                $("#BPdetailTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    debugger
                                    if (a < arr.Table.length) {
                                        var SNo = currentRow.find("#SNohf").val();
                                        currentRow.find("#GLAccount_" + SNo).empty().append('<option value=' + arr.Table[a].acc_id + ' selected="selected">' + arr.Table[a].acc_name + '</option>');
                                        //currentRow.find("#GLAccount_" + SNo).val(arr.Table[a].acc_name);
                                        var AccBal = $("#AccountBalance").val();/*add AccBal by Hina On 26-08-2025 */
                                        currentRow.find("#RowWiseAccBal").val(AccBal);/*add AccBal by Hina On 26-08-2025 */
                                        currentRow.find("#GLGroup").val(arr.Table[a].acc_group_name);
                                        currentRow.find("#hfAccID").val(arr.Table[a].acc_id);
                                        currentRow.find("#hdAccType").val(arr.Table[a].acc_type);
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
                                            //currentRow.find("#CreditAmountInSpecific").val(parseFloat(arr.Table[i].cr_amt_sp).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInSpecific").val(toDecimal(arr.Table[i].cr_amt_sp, ValDecDigit));
                                        }
                                        if (arr.Table[i].cr_amt_bs == "0") {
                                            currentRow.find("#CreditAmountInBase").val("");
                                        }
                                        else {
                                            //currentRow.find("#CreditAmountInBase").val(parseFloat(arr.Table[i].cr_amt_bs).toFixed(ValDecDigit));
                                            currentRow.find("#CreditAmountInBase").val(toDecimal(arr.Table[i].cr_amt_bs, ValDecDigit));
                                        }
                                        //currentRow.find("#DebitAmountInSpecific").val(parseFloat(cmn_ReplaceCommas(arr.Table[i].dr_amt_sp)).toFixed(ValDecDigit));
                                        //currentRow.find("#DebitAmountInBase").val(parseFloat(cmn_ReplaceCommas(arr.Table[i].dr_amt_bs)).toFixed(ValDecDigit));
                                        //currentRow.find("#CreditAmountInSpecific").val(parseFloat(cmn_ReplaceCommas(arr.Table[i].cr_amt_sp)).toFixed(ValDecDigit));
                                        //currentRow.find("#CreditAmountInBase").val(parseFloat(cmn_ReplaceCommas(arr.Table[i].cr_amt_bs)).toFixed(ValDecDigit));
                                        currentRow.find("#Narration").val($("#BpHeaderNarration").val());
                                        if ($("#InterBranch").is(":checked")) {//Added by Suraj maurya on 13-08-2024

                                            fetch_BranchList().then((arr) => {
                                                bindGlBranchList(SNo, arr);
                                                let BrId = $("#BrId").text();
                                                $("#GLbranch_" + SNo).val(BrId);
                                            });//Getting Branch List

                                            
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
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var rowCount = $('#BPdetailTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#BPdetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    var InterBranch = "N";
    var BrId = $("#TopNavBranchList option:selected").val();
    var BrName = $("#TopNavBranchList option:selected").text();
    if ($("#InterBranch").is(":checked")) {
        InterBranch = "Y";
        BrId = "0";
        BrName = "---Select---";
    }
    
    

    if (DocId == '105104115120') {
        $('#BPdetailTbl tbody').append(`<tr id="${++rowIdx}">
 <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td id="td_gl_br" ${InterBranch=="N"?"hidden":""}>
    <div class="col-sm-12 lpo_form no-padding" >
        <select class="form-control" id="GLbranch_${RowNo}" name="GLbranch_${RowNo}" onchange="OnChangeGlBranchName(this,event)">
            <option value="${BrId}">${BrName}</option>
        </select>
        <input type="hidden" id="hfGlBranchID" value="${BrId}" />
        <span id="GLbranchError" class="error-message is-visible"></span>
    </div>
</td>
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form" style="padding:0px;">
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input type="hidden" id="hfAccID" style="display: none;" />
<input type="hidden" id="hdAccType" style="display: none;" />
<span id="AccountNameError" class="error-message is-visible"></span></div>
</td>


<td>

<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','BPDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div><input id="hdn_RowWiseAccBal"  type="hidden" />

</td>

<td><textarea class="form-control remarksmessage" cols="20" id="GLGroup" maxlength="100" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="GLGroup" value="" placeholder="${$("#AccGroup").text()}" rows="2"  readonly></textarea>
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
<input id="DebitAmountInBase" class="form-control num_right" onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00" disabled>
</div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInSpecific"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)"  onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Crdt_Amnt_Error" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInBase" class="form-control num_right" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)"  onkeypress="return AmountFloatVal(this, event)" autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00" disabled>
</div>
</td>
<td>
<div class="lpo_form">
    <textarea id="Narration" class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}">${$("#BpHeaderNarration").val()}</textarea>
       <span id="Narr_Error" class="error-message is-visible"></span>
 </div>
</td>
  <td class="center">
          <button type="button" disabled id="BillAdjustment" onclick="OnClickBillAdjIconBtn(event);" class="calculator subItmImg" data-toggle="modal" data-target="#BillAdjustment" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BillAdjustment").text()}"></i></button>
 </td>
      <td class="center">
      <button type="button" disabled id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
 </td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

        //<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form no-padding">
        //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
        //    <input type="hidden" id="hfAccID" style="display: none;" />
        //    <input type="hidden" id="hdAccType" style="display: none;" />
        //    <span id="AccountNameError" class="error-message is-visible"></span></div>
        //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','BPDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
        //</td>
    }
    else {
        $('#BPdetailTbl tbody').append(`<tr id="${++rowIdx}">
 <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td id="td_gl_br" ${InterBranch == "N" ? "hidden" : ""}>
    <div class="col-sm-12 lpo_form no-padding" >
        <select class="form-control" id="GLbranch_${RowNo}" name="GLbranch_${RowNo}" onchange="OnChangeGlBranchName(this,event)">
            <option value="${BrId}">${BrName}</option>
        </select>
        <input type="hidden" id="hfGlBranchID" value="${BrId}" />
        <span id="GLbranchError" class="error-message is-visible"></span>
    </div>
</td>
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-12 lpo_form" style="padding:0px;">
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input type="hidden" id="hfAccID" style="display: none;" />
<input type="hidden" id="hdAccType" style="display: none;" />
<span id="AccountNameError" class="error-message is-visible"></span></div>
</td>

<td>

<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','BPDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
<input id="hdn_RowWiseAccBal"  type="hidden" />

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
<textarea id="Narration" class="form-control remarksmessage" maxlength="200" onkeyup="OnchangeNarr(event)" onchange="OnchangeNarr(event)" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="Narration" placeholder="${$("#span_Narration").text()}">${$("#BpHeaderNarration").val()}</textarea>
<span id="Narr_Error" class="error-message is-visible"></span>
 </div>
</td>
  <td class="center">
          <button type="button" disabled id="BillAdjustment" onclick="OnClickBillAdjIconBtn(event);" class="calculator subItmImg" data-toggle="modal" data-target="#BillAdjustment" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BillAdjustment").text()}"></i></button>
 </td>
      <td class="center">
      <button type="button" disabled id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
</td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

        //<td><div class="col-sm-11 lpo_form no-padding">
        //    <select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
        //    <input type="hidden" id="hfAccID" style="display: none;" />
        //    <input type="hidden" id="hdAccType" style="display: none;" />
        //    <span id="AccountNameError" class="error-message is-visible"></span></div>
        //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','BPDate',${RowNo},'')" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
        //</td>
    }
    BindAcountList(RowNo);
}
function BindAcountList(ID) {
    debugger;
    var br_Id = 0;
    if ($("#InterBranch").is(":checked")) {//Added by Suraj maurya on 16-08-2024
        fetch_BranchList().then((arr) => {
            bindGlBranchList(ID, arr);
        });//Getting Branch List
    } else {
        br_Id = $("#BrId").text();
    }
    //Cmn_BindAccountList("#GLAccount_", ID, "#BPdetailTbl", "#SNohf", "", "105104115120");
    $("#GLAccount_" + ID).html(`<option value="0">---Select---</option>`);
    Cmn_BindAccountListAsync("#GLAccount_", ID, "#BPdetailTbl", "#SNohf", "", "105104115120", br_Id);

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
    var hdn_int_br_id = clickedrow.find("#hfGlBranchID").val();
    if (hdn_acc_id != "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').each(function () {
                var row = $(this);
                if (row.find("#hdntbl_CstIntBrId").text() == hdn_int_br_id) {
                    row.remove();
                }
            });
        }
    }
    DeletebillAdjustment(hdn_acc_id, hdn_int_br_id);

    clickedrow.find("#hfAccID").val(Acc_ID);
    var Status = $("#hdBPStatus").val();

    if (Status == "" || Status == "D") {
        clickedrow.find("#BillAdjustment").prop("disabled", true);
        clickedrow.find("#DebitAmountInBase").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
        clickedrow.find("#DebitAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDecDigit)*/).prop("disabled", false);
        clickedrow.find("#CreditAmountInBase").val(""/*parseFloat(0).toFixed(ValDecDigit)*/);
        clickedrow.find("#CreditAmountInSpecific").val(""/*parseFloat(0).toFixed(ValDecDigit)*/).prop("disabled", false);
        /*Commented by HIna on 27-10-2023 to changes for all debit side enable in case of BP except 1 row
         * and all credit side enable in case of BR except 1 row*/
        //clickedrow.find("#DebitAmountInSpecific").attr("disabled", true);
        //clickedrow.find("#CreditAmountInSpecific").attr("disabled", true);
    }

   
    if (Acc_ID == "0") {
        clickedrow.find("#AccountNameError").text($("#valueReq").text());
        clickedrow.find("#AccountNameError").css("display", "block");
        //clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
        clickedrow.find('[aria-labelledby="select2-GLAccount_' + SNo+'-container"]').css("border", "1px solid red");
    }
    else {
        clickedrow.find("#AccountNameError").css("display", "none");
        //clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
        clickedrow.find('[aria-labelledby="select2-GLAccount_' + SNo +'-container"]').css("border", "1px solid #aaa");
    }
    CalculateTotalDrCrValue();
    Cmn_BindGroup(clickedrow, Acc_ID, "")
    /*-----------Code Start Add by Hina Sharma on 26-08-2025 to get account Balance Account wise-------- */
    var VouDate = $("#BPDate").val();
    CMN_BindAccountBalance(clickedrow, Acc_ID, VouDate);
    /*-----------Code End Add by Hina Sharma on 26-08-2025 to get account Balance Account wise-------- */

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
    
    /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
   /* var debitAmt = currentrow.find("#DebitAmountInBase").val();*/
    var debitAmt12 = currentrow.find("#DebitAmountInSpecific").val();
    var debitAmt = cmn_ReplaceCommas(debitAmt12);
    if (DocId == '105104115120') {
        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
            currentrow.find("#DebitAmountInSpecific").val("")
            /*currentrow.find("#DebitAmountInBase").val("")*/
        }
        else {
            //var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit);
            var debitAmt2 = toDecimal(debitAmt, ValDecDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            currentrow.find("#DebitAmountInSpecific").val(debitAmt1)
            //currentrow.find("#DebitAmountInSpecific").val(parseFloat(cmn_ReplaceCommas(debitAmt)).toFixed(ValDecDigit))
            /*currentrow.find("#DebitAmountInBase").val(parseFloat(debitAmt).toFixed(ValDecDigit))*/
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
        debugger;
        if (debitAmt != 0 && debitAmt != null && debitAmt != "") {
            var AccType = currentrow.find("#hdAccType").val();
            if (AccType==2) {
                currentrow.find("#BillAdjustment").prop("disabled", false);
                
            }
            else {
                currentrow.find("#BillAdjustment").prop("disabled", true);
                /*----Start code add by Hina on 15-10-2025------ */
                var hfAccID = currentrow.find('#hfAccID').val();
                var hfIntBrId = currentrow.find('#hfGlBranchID').val();
                if (hfAccID != "") {
                    DeletebillAdjustment(hfAccID, hfIntBrId);
                    ValidateEyeColor(currentrow, "BillAdjustment", "Empty");
                }
                
                /*----End code add by Hina on 15-10-2025------ */
            }
        }
        else {
            currentrow.find("#BillAdjustment").prop("disabled", true);
            /*----Start code add by Hina on 15-10-2025------ */
            var hfAccID = currentrow.find('#hfAccID').val();
            var hfIntBrId = currentrow.find('#hfGlBranchID').val();
            if (hfAccID != "") {
                DeletebillAdjustment(hfAccID, hfIntBrId);
                ValidateEyeColor(currentrow, "BillAdjustment", "Empty");
             }
                /*----End code add by Hina on 15-10-2025------ */
        }
    }
    if (DocId == '105104115110') {
        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
            currentrow.find("#DebitAmountInSpecific").val("")
        }
        else {
            //var debitAmt2 = parseFloat(debitAmt).toFixed(ValDecDigit);
            var debitAmt2 = toDecimal(debitAmt, ValDecDigit);
            var debitAmt1 = cmn_addCommas(debitAmt2);
            currentrow.find("#DebitAmountInSpecific").val(debitAmt1);
            //currentrow.find("#DebitAmountInSpecific").val(parseFloat(cmn_ReplaceCommas(debitAmt)).toFixed(ValDecDigit))
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
        /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
        var FAmt = debitAmt * ConvRate;
       /* var FAmt = debitAmt / ConvRate;*/
        var FinVal1 = toDecimal(FAmt, ValDecDigit);//parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        currentrow.find("#DebitAmountInBase").val(FinVal);
        /*currentrow.find("#DebitAmountInSpecific").val(FinVal);*/

    }
    CalculateTotalDrCrValue();
}
function OnChangeCreditAmount(e) {

    debugger;  
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
    var creditAmt12 = currentrow.find("#CreditAmountInSpecific").val();
    var creditAmt = cmn_ReplaceCommas(creditAmt12);
   /* var creditAmt = currentrow.find("#CreditAmountInBase").val();*/
    var OverDrlimit = $("#HdnOdLimit").val();
    var Balance = $("#AccountBalance").val();
    var OdAllow = $("#hd_od_allow").val();
    var OdLimit = $("#hd_od_limit").val();

    const MyArray = Balance.split(" ");
    var Bal = MyArray[0];
    var BalType = MyArray[1];
    var FinBal = 0;
    if (OdAllow == "Y") {

        if (BalType == "Dr") {
            FinBal = parseFloat(cmn_ReplaceCommas(Bal)) + parseFloat(cmn_ReplaceCommas(OdLimit));
        }
        else {
            FinBal = parseFloat(cmn_ReplaceCommas(OdLimit)) - parseFloat(cmn_ReplaceCommas(Bal));
        }
    }
    else {
        if (BalType == "Dr") {
            FinBal = parseFloat(cmn_ReplaceCommas(Bal));
        }
        else {
            FinBal = - parseFloat(cmn_ReplaceCommas(Bal));
        }
    }

   if (DocId == '105104115120') {
        if (AvoidDot(creditAmt) == false) {
            creditAmt = "";
            /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
            currentrow.find("#CreditAmountInSpecific").val("")
            /*currentrow.find("#CreditAmountInBase").val("")*/
        }
        else {
            /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
            //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
            var creditAmt2 = toDecimal(creditAmt,ValDecDigit);
            var creditAmt1 = cmn_addCommas(creditAmt2);
            currentrow.find("#CreditAmountInSpecific").val(creditAmt1)
            /*currentrow.find("#CreditAmountInBase").val(parseFloat(creditAmt).toFixed(ValDecDigit))*/
        }
        var Rowno = currentrow.find("#SNohf").val();
       if (Rowno == 1) {
           if (parseFloat(creditAmt) > (parseFloat(FinBal))) {
               currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
               currentrow.find("#Crdt_Amnt_Error").text($("#ExceedingBalance").text());
               currentrow.find("#Crdt_Amnt_Error").css("display", "block");
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
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");
            }
         }
   
    }
    if (DocId == '105104115110') {
        if (AvoidDot(creditAmt) == false) {
            creditAmt = "";
            currentrow.find("#CreditAmountInSpecific").val("")
        }
        else {
            //var creditAmt2 = parseFloat(creditAmt).toFixed(ValDecDigit);
            var creditAmt2 = toDecimal(creditAmt, ValDecDigit);
            var creditAmt1 = cmn_addCommas(creditAmt2);
            currentrow.find("#CreditAmountInSpecific").val(creditAmt1);
            //currentrow.find("#CreditAmountInSpecific").val(parseFloat(cmn_ReplaceCommas(creditAmt1)).toFixed(ValDecDigit))
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
                currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
                currentrow.find("#Dbt_Amnt_Error").text("");
                currentrow.find("#Dbt_Amnt_Error").css("display", "none");
            }
        }
        debugger;
        if (creditAmt != 0 && creditAmt != null && creditAmt != "") {
            var AccType = currentrow.find("#hdAccType").val();
            if (AccType == 1) {
                currentrow.find("#BillAdjustment").prop("disabled", false);
            }
            else {
                currentrow.find("#BillAdjustment").prop("disabled", true);
                /*----Start code add by Hina on 15-10-2025------ */
                var hfAccID = currentrow.find('#hfAccID').val();
                var hfIntBrId = currentrow.find('#hfGlBranchID').val();
                if (hfAccID != "") {
                    DeletebillAdjustment(hfAccID, hfIntBrId);
                    ValidateEyeColor(currentrow, "BillAdjustment", "Empty");
                }
                /*----End code add by Hina on 15-10-2025------ */
            }
        }
        else {
            currentrow.find("#BillAdjustment").prop("disabled", true);
            /*----Start code add by Hina on 15-10-2025------ */
            var hfAccID = currentrow.find('#hfAccID').val();
            var hfIntBrId = currentrow.find('#hfGlBranchID').val();
            if (hfAccID != "") {
                DeletebillAdjustment(hfAccID, hfIntBrId);
                ValidateEyeColor(currentrow, "BillAdjustment", "Empty");
            }
           
                /*----End code add by Hina on 15-10-2025------ */
        }
    }
   
    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);

    if ((creditAmt != 0 || creditAmt != null || creditAmt != "") && (ConvRate != 0 || ConvRate != null || ConvRate != "")) {
        debugger;
        /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
         var FAmt = creditAmt * ConvRate;
        /*var FAmt = creditAmt / ConvRate;*/
        var FinVal1 = toDecimal(FAmt, ValDecDigit);//parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal = cmn_addCommas(FinVal1);
        currentrow.find("#CreditAmountInBase").val(FinVal);
        /*currentrow.find("#CreditAmountInSpecific").val(FinVal);*/
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
    var GrossValue = 0;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    //Added by Suraj on 07-05-2024 
    Cmn_ControlDecimalDiffVal("BPdetailTbl", "DebitAmountInBase", "DebitAmountInSpecific", "CreditAmountInBase", "CreditAmountInSpecific");
    $("#BPdetailTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
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
    })
    var debitAmount2 = toDecimal(debitAmount, ValDigit);//parseFloat(debitAmount).toFixed(ValDigit);
    var debitAmount1 = cmn_addCommas(debitAmount2);
    $("#dbtTtlAmnt").text(debitAmount1);
    //$("#dbtTtlAmnt").text(parseFloat(debitAmount1).toFixed(ValDigit));
    var creditAmount2 = toDecimal(creditAmount, ValDigit);//parseFloat(creditAmount).toFixed(ValDigit);
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
        FinalDiff = (difference*-1)
        $("#spanamttype").text("Cr")
    }
    if (difference == 0) {
        $("#spanamttype").text("")
    }
    var FinalDiff2 = toDecimal(FinalDiff, ValDigit);//parseFloat(FinalDiff).toFixed(ValDigit);
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
function InsertBPDetail() {
    debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        var SrcType = $("#Src_Type").val();
        $("#hdSrc_Type").val(SrcType);
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
        if (CheckBillAdjAccountDetails() == false) {
            swal("", $("#AdjustedAmountMismatch").text(), "warning");
            return false;
        }
        var FinalAccountDetail = [];
        FinalAccountDetail = InsertBPAccountDetails();
        var AccountDt = JSON.stringify(FinalAccountDetail);
        $('#hdAccountDetailList').val(AccountDt);
        CalculateTotalDrCrValue();

        var FinalBillWiseAdjDetail = [];
        FinalBillWiseAdjDetail = InsertBillWiseAdjDetails();
        var BillWiseAdjDt = JSON.stringify(FinalBillWiseAdjDetail);
        $('#hdBillWiseAdjDetailList').val(BillWiseAdjDt);

        var FinalCostCntrDetails = [];
        FinalCostCntrDetails = Cmn_InsertCCDetails();
        var CCDetails = JSON.stringify(FinalCostCntrDetails);
        $('#hdn_CC_DetailList').val(CCDetails);

        if (onclkDtlSaveBtnValidateBillAd() == false) {
            return false;
        }

        if (Cmn_CC_DtlSaveButtonClick("BPdetailTbl", "DebitAmountInSpecific", "CreditAmountInSpecific","hdBPStatus") == false) {
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
        RemoveSession();
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    catch (ex) {
        console.log(ex);
        return false;
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
function OnchangeInSNo() {
    debugger;
    var ins_no = $("#ins_no").val();
    if (ins_no == "" || ins_no == "0") {
        document.getElementById("vm_ins_no").innerHTML = $("#valueReq").text();
        $("#vm_ins_no").css("display", "block");
        $("#ins_no").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#vm_ins_no").css("display", "none");
        $("#ins_no").css("border-color", "#ced4da");
    }

}
function OnchangeInSDt() {
    debugger;
    var ins_dt = $("#ins_dt").val();
    if (ins_dt == "" || ins_dt == "0") {
        document.getElementById("vm_ins_dt").innerHTML = $("#valueReq").text();
        $("#vm_ins_dt").css("display", "block");
        $("#ins_dt").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#vm_ins_dt").css("display", "none");
        $("#ins_dt").css("border-color", "#ced4da");
    }
    $("#vm_ins_dt").css("display", "none");
    $("#ins_dt").css("border-color", "#ced4da");
}
function OnchangeInSName() {
    debugger;
    var ins_name = $("#ins_name").val();
    if (ins_name == "" || ins_name == "0") {
        document.getElementById("vm_ins_name").innerHTML = $("#valueReq").text();
        $("#vm_ins_name").css("display", "block");
        $("#ins_name").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        $("#vm_ins_name").css("display", "none");
        $("#ins_name").css("border-color", "#ced4da");
    }


}
function HeaderValidation() {
    debugger;
    var ErrorFlag = "N";
    var VouDate = $("#BPDate").val();/*start Add by Hina on 11-11-2024 to validate date */
    if (VouDate == "") {
        $('#SpanVouDateErrorMsg').text($("#valueReq").text());
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#BPDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#BPDate").css("border-color", "#ced4da");
    }/*End Add by Hina on 11-11-2024 to validate date */

    var coa_id = $("#ddlBankAccName").val();
    if (coa_id == "" || coa_id == "0" || coa_id == null) {
        $("#vmBankAccName").css("display", "block");
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "red");
        $("#vmBankAccName").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmBankAccName").css("display", "none");
        $("#ddlBankAccName").css("border-color", "#ced4da");
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
    var InstrumentType = $("#ins_type").val();
    if (InstrumentType == "O") {

    }
    else {
        var ins_no = $("#ins_no").val();
        if (ins_no == "" || ins_no == "0") {
            document.getElementById("vm_ins_no").innerHTML = $("#valueReq").text();
            $("#vm_ins_no").css("display", "block");
            $("#ins_no").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            $("#vm_ins_no").css("display", "none");
            $("#ins_no").css("border-color", "#ced4da");
        }
        var ins_dt = $("#ins_dt").val();
        if (ins_dt == "" || ins_dt == "0") {
            document.getElementById("vm_ins_dt").innerHTML = $("#valueReq").text();
            $("#vm_ins_dt").css("display", "block");
            $("#ins_dt").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            $("#vm_ins_dt").css("display", "none");
            $("#ins_dt").css("border-color", "#ced4da");
        }
    }

    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertBPAccountDetails() {
    debugger;
    var AccountDetailList = new Array();
    $("#BPdetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var SpanRowId = currentRow.find("#SpanRowId").text();
        var AccountList = {};
        /*commented  by Hina on 29-06-2024 to Seprate acc_id and SpanRowId as seq_no under supervision of Prem Sir*/
        
        AccountList.acc_id = currentRow.find("#GLAccount_" + rowid).val();
        /* AccountList.acc_id = (currentRow.find("#GLAccount_" + rowid).val()) + '_' + SpanRowId;*/
        
        AccountList.acc_name = currentRow.find("#GLAccount_" + rowid + " option:selected").text();
        AccountList.acc_group_name = currentRow.find("#GLGroup").val();
        AccountList.acc_type = currentRow.find("#hdAccType").val();
        AccountList.curr_id = $("#curr").val();
        //AccountList.conv_rate = $("#ConvRate").val();
        var ConvRate = $("#ConvRate").val();
        AccountList.conv_rate = cmn_ReplaceCommas(ConvRate);
        //AccountList.dr_amt_bs = currentRow.find("#DebitAmountInBase").val();
        var DebitAmountInBase = currentRow.find("#DebitAmountInBase").val();
        AccountList.dr_amt_bs = cmn_ReplaceCommas(DebitAmountInBase);
        //AccountList.dr_amt_sp = currentRow.find("#DebitAmountInSpecific").val();
        var DebitAmountInSpecific = currentRow.find("#DebitAmountInSpecific").val();
        AccountList.dr_amt_sp = cmn_ReplaceCommas(DebitAmountInSpecific);
        //AccountList.cr_amt_bs = currentRow.find("#CreditAmountInBase").val();
        var CreditAmountInBase = currentRow.find("#CreditAmountInBase").val();
        AccountList.cr_amt_bs = cmn_ReplaceCommas(CreditAmountInBase);
        //AccountList.cr_amt_sp = currentRow.find("#CreditAmountInSpecific").val();
        var CreditAmountInSpecific = currentRow.find("#CreditAmountInSpecific").val();
        AccountList.cr_amt_sp = cmn_ReplaceCommas(CreditAmountInSpecific);
        AccountList.narr = currentRow.find("#Narration").val();
        AccountList.seq_no = SpanRowId;

        AccountList.GlBrID = currentRow.find("#GLbranch_" + rowid).val();
        AccountList.GlBrName = currentRow.find("#GLbranch_" + rowid + " option:selected").text();
        AccountDetailList.push(AccountList);
    });
    return AccountDetailList;
};
function AccountValidation() {
    debugger
    var RateDecDigit = $("#RateDigit").text()
    var ValDecDigit = $("#ValDigit").text();///Amount 
    var DocId = $("#DocumentMenuId").val();
    var status = $("#hdBPStatus").val();
    var Msg = $("#hdn_Message").val();
    var ErrorFlag = "N";
    if ($("#BPdetailTbl tbody tr").length > 0) {
        if (DocId == '105104115120') {
            
            $("#BPdetailTbl tbody tr").each(function () {
                debugger
                var currentrow = $(this);
                var Rowno = currentrow.find("#SNohf").val();
                if (Rowno == 1) {
                    var Balance = $("#AccountBalance").val();
                    var OdAllow = $("#hd_od_allow").val();
                    var OdLimit = cmn_ReplaceCommas($("#hd_od_limit").val());

                    const MyArray = Balance.split(" ");
                    var Bal = cmn_ReplaceCommas(MyArray[0]);
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


                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    //var DbtAmnt = currentrow.find("#DebitAmountInBase").val();
                    //var CrdtAmnt = currentrow.find("#CreditAmountInBase").val();
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if (AccName == "0") {
                        swal("", $("#NoGLAccountSelected").text(), "warning");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#GLAccount_" + Rowno).focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (ErrorFlag == "Y") {
                        return false;
                    }
                    debugger;
                    var canflag = $("#Cancelled").is(':checked');
                    //if ((status != "D" && status != "F" && status != "") && (Msg == "N")) {
                    if (canflag == false) {
                        if (parseFloat(CrdtAmnt) > (parseFloat(FinBal))) {
                            currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                            currentrow.find("#Crdt_Amnt_Error").text($("#ExceedingBalance").text());
                            currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                            if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                                currentrow.find("#CreditAmountInSpecific").focus();
                            }
                            ErrorFlag = "Y";
                        }
                    }
                    if (CrdtAmnt == "" || CrdtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#CreditAmountInSpecific").focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#Narration").focus();
                        }
                        ErrorFlag = "Y";
                    }
                }
                if (Rowno > 1)  {
                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    
                    /*Commented by HIna on 26-10-2023 to changes for Base Amount not for specific*/
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    //var DbtAmnt = currentrow.find("#DebitAmountInBase").val();
                    //var CrdtAmnt = currentrow.find("#CreditAmountInBase").val();
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if ($("#InterBranch").is(":checked")) {
                        var GLBrName = currentrow.find("#GLbranch_" + Rowno).val();
                        if (GLBrName == "0") {
                            currentrow.find("#GLbranch_" + Rowno).css("border-color", "red");
                            currentrow.find("#GLbranchError").text($("#valueReq").text());
                            currentrow.find("#GLbranchError").css("display", "block");
                            if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                                currentrow.find("#GLbranch_" + Rowno).focus();
                            }
                            ErrorFlag = "Y";
                        }
                        else {
                            currentrow.find("#GLbranch_" + Rowno).css("border-color", "#ced4da");
                            currentrow.find("#GLbranchError").text("");
                            currentrow.find("#GLbranchError").css("display", "none");
                        }
                    }
                    

                    if (AccName == "0") {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "red");
                        currentrow.find("#AccountNameError").text($("#valueReq").text());
                        currentrow.find("#AccountNameError").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#GLAccount_" + Rowno).focus();
                        }
                        ErrorFlag = "Y";
                    }
                    else {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
                        currentrow.find("#AccountNameError").text("");
                        currentrow.find("#AccountNameError").css("display", "none");
                    }
                    if ((DbtAmnt == "" || parseFloat(cmn_ReplaceCommas(DbtAmnt)) == parseFloat(0)) && (CrdtAmnt == "" || parseFloat(cmn_ReplaceCommas(CrdtAmnt)) == parseFloat(0))) {
                        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error").css("display", "block");

                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#DebitAmountInSpecific").focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#Narration").focus();
                        }
                        ErrorFlag = "Y";
                    }
                }

                if (Narrat == "") {
                    currentrow.find("#Narration").css("border-color", "red");
                    currentrow.find("#Narr_Error").text($("#valueReq").text());
                    currentrow.find("#Narr_Error").css("display", "block");
                    if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                        currentrow.find("#Narration").focus();
                    }
                    ErrorFlag = "Y";
                }
                else {
                    currentrow.find("#Narration").css("border-color", "#ced4da");
                    currentrow.find("#Narr_Error").text("");
                    currentrow.find("#Narr_Error").css("display", "none");
                }
            });
        }
        if (DocId == '105104115110') {
            $("#BPdetailTbl tbody tr").each(function () {
                debugger
                var currentrow = $(this);
                var Rowno = currentrow.find("#SNohf").val();
                if ($("#InterBranch").is(":checked")) {
                    var GLBrName = currentrow.find("#GLbranch_" + Rowno).val();
                    if (GLBrName == "0") {
                        currentrow.find("#GLbranch_" + Rowno).css("border-color", "red");
                        currentrow.find("#GLbranchError").text($("#valueReq").text());
                        currentrow.find("#GLbranchError").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#GLbranch_" + Rowno).focus();
                        }
                        ErrorFlag = "Y";
                    }
                    else {
                        currentrow.find("#GLbranch_" + Rowno).css("border-color", "#ced4da");
                        currentrow.find("#GLbranchError").text("");
                        currentrow.find("#GLbranchError").css("display", "none");
                    }
                }
                if (Rowno == 1) {
                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if (AccName == "0") {
                        swal("", $("#NoGLAccountSelected").text(), "warning");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#GLAccount_" + Rowno).focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (ErrorFlag == "Y") {
                        return false;
                    }
                    if (DbtAmnt == "" || DbtAmnt == parseFloat(0).toFixed(ValDecDigit)) {
                        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#DebitAmountInSpecific").focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#Narration").focus();
                        }
                        ErrorFlag = "Y";
                    }
                }

                if (Rowno > 1){
                    var AccName = currentrow.find("#GLAccount_" + Rowno).val();
                    var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInSpecific").val());
                    var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInSpecific").val());
                    var Narrat = currentrow.find("#Narration").val().trim();
                    if (AccName == "0") {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "red");
                        currentrow.find("#AccountNameError").text($("#valueReq").text());
                        currentrow.find("#AccountNameError").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#GLAccount_" + Rowno).focus();
                        }
                        ErrorFlag = "Y";
                    }
                    else {
                        currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
                        currentrow.find("#AccountNameError").text("");
                        currentrow.find("#AccountNameError").css("display", "none");
                    }
                    
                    if ((DbtAmnt == "" || parseFloat(cmn_ReplaceCommas(DbtAmnt)) == parseFloat(0)) && (CrdtAmnt == "" || parseFloat(cmn_ReplaceCommas(CrdtAmnt)) == parseFloat(0))) {
                        currentrow.find("#DebitAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                        currentrow.find("#CreditAmountInSpecific").css("border-color", "red");
                        currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                        currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#DebitAmountInSpecific").focus();
                        }
                        ErrorFlag = "Y";
                    }
                    if (Narrat == "") {
                        currentrow.find("#Narration").css("border-color", "red");
                        currentrow.find("#Narr_Error").text($("#valueReq").text());
                        currentrow.find("#Narr_Error").css("display", "block");
                        if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                            currentrow.find("#Narration").focus();
                        }
                        ErrorFlag = "Y";
                    }
                }
                if (Narrat == "") {
                    currentrow.find("#Narration").css("border-color", "red");
                    currentrow.find("#Narr_Error").text($("#valueReq").text());
                    currentrow.find("#Narr_Error").css("display", "block");
                    if (ErrorFlag == "N") {//Added by Suraj on 04-09-2024
                        currentrow.find("#Narration").focus();
                    }
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
    if ($("#BPdetailTbl tbody tr").length > 0) {
        if (DocId == '105104115120') {
            $("#BPdetailTbl tbody tr").each(function () {
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

        if (DocId == '105104115110') {
            $("#BPdetailTbl tbody tr").each(function () {
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
    //    BPStatus = $('#hdBPStatus').val().trim();
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

    /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var Voudt = $("#BPDate").val();
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
                        BPStatus = $('#hdBPStatus').val().trim();
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
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    VouNo = $("#BankPaymentNo").val();
    VouDate = $("#BPDate").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (VouNo + ',' + VouDate + ',' + WF_Status1);
    $("#hdDoc_No").val(VouNo);
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
        if (docid == '105104115110') {
            var pdfAlertEmailFilePath = "BankPayment_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/BankPayment/SavePdfDocToSendOnEmailAlert");
        }
        if (docid == '105104115110') {
            var pdfAlertEmailFilePath = "BankReceipt_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/BankReceipt/SavePdfDocToSendOnEmailAlert");
        }
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (docid == '105104115120') {
        if (fwchkval === "Forward") {
            if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
                debugger;
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankPayment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
    else {
        if (fwchkval === "Forward") {
            if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
                debugger;
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
    debugger;
    if (docid == '105104115120') {
        if (fwchkval === "Approve") {
            window.location.href = "/ApplicationLayer/BankPayment/BankPaymentApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&int_br_nurr=" + $('#HdnIntBrNurr_BP').val();

        }
    }
    else {
        if (fwchkval === "Approve") {
            window.location.href = "/ApplicationLayer/BankReceipt/BankReceiptApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&int_br_nurr=" + $('#HdnIntBrNurr_BR').val();

        }
    }
    if (docid == '105104115120') {
        if (fwchkval === "Reject") {
            if (fwchkval != "" && VouNo != "" && VouDate != "") {
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankPayment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
    else {
        if (fwchkval === "Reject") {
            if (fwchkval != "" && VouNo != "" && VouDate != "") {
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
    if (docid == '105104115120') {
        if (fwchkval === "Revert") {
            debugger
            if (fwchkval != "" && VouNo != "" && VouDate != "") {
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankPayment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
    else {
        if (fwchkval === "Revert") {
            debugger
            if (fwchkval != "" && VouNo != "" && VouDate != "") {
                await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
                mailerror = $("#MailError").val();
                window.location.href = "/ApplicationLayer/BankReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
            }
        }
    }
}

function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#BankPaymentNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function OnClickCancelFlag() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertBPDetail()");
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
    var RateDecDigit = $("#RateDigit").text()
    var ExchDecDigit = $("#ExchDigit").text();

    var coa_id = $("#ddlBankAccName").val();
    var YouDate = $("#BPDate").val();
    if (YouDate == "") {/*start Add by Hina on 11-11-2024 to validate date */
        $('#SpanVouDateErrorMsg').text($("#valueReq").text());
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#BPDate").css("border-color", "Red");
        return false;
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#BPDate").css("border-color", "#ced4da");
        /*End Add by Hina on 11-11-2024 to validate date */

        debugger;
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/BankPayment/GetAccCurr",
                data: {
                    acc_id: coa_id,
                    Date: YouDate,
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
                            $("#AccountBalance").val(arr.Table2[0].Avl_bal);
                        }
                        else {
                            $("#AccountBalance").val(parseFloat(0).toFixed(RateDecDigit));
                        }
                        if (arr.Table.length > 0) {
                            $("#curr").val(arr.Table[0].curr_id);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            //$("#ConvRate").val(parseFloat(cmn_ReplaceCommas(arr.Table[0].conv_rate)).toFixed(ExchDecDigit));
                            //$("#hdconv").val(parseFloat(cmn_ReplaceCommas(arr.Table[0].conv_rate)).toFixed(ExchDecDigit));
                            $("#ConvRate").val(toDecimal(cmn_ReplaceCommas(arr.Table[0].conv_rate), ExchDecDigit));
                            $("#hdconv").val(toDecimal(cmn_ReplaceCommas(arr.Table[0].conv_rate), ExchDecDigit));
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
        if ($("#PDC").is(":checked")) {/*Add By hina sharma on 09-08-2024 for pdc*/
            $('#ins_dt').val(YouDate);
            //$('#BPDate').on('onkeypress', function (e) {
            //    var keyCode = (e.keyCode ? e.keyCode : e.which);
            //    if (keyCode > 47 && keyCode < 58) {
            //        e.preventDefault();
            //    }
            //});
        }
        else {
            Cmn_VoucherDateValidation("#BPDate");
        }
    }
    //Cmn_VoucherDateValidation( "#BPDate");
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
//------------------Bill Adjustmet detail------------------------
function OnClickBillAdjIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var voudate = $("#BPDate").val();/*Add by Hina on 11-11-2024 to */
    if (voudate == "") {
        $('#SpanVouDateErrorMsg').text($("#valueReq").text()); 
        $("#SpanVouDateErrorMsg").css("display", "block");
        $("#BPDate").css("border-color", "Red");
        clickedrow.find("#BillAdjustment").attr("data-target", "");
        return false;
    }
    else {
        $("#SpanVouDateErrorMsg").css("display", "none");
        $("#BPDate").css("border-color", "#ced4da");
        clickedrow.find("#BillAdjustment").attr("data-target", "#BillAdjustment");
        Cmn_BindVouDate("",voudate);
    }
    
    var DocId = $("#DocumentMenuId").val();

    var ValDecDigit = $("#ValDigit").text();            
               
    var Rowno = clickedrow.find("#SNohf").val();                   
    var GlBrID = clickedrow.find("#GLbranch_" + Rowno).val();//Added by Suraj on 16-08-2024
    var GlBrName = clickedrow.find("#GLbranch_" + Rowno + " option:selected").text();//Added by Suraj on 31-08-2024
    var AccID = clickedrow.find("#GLAccount_" + Rowno).val();
    var AccName = clickedrow.find("#GLAccount_" + Rowno + " option:selected").text().trim();
    var DbtAmnt = clickedrow.find("#DebitAmountInSpecific").val();
    var CrdtAmnt = clickedrow.find("#CreditAmountInSpecific").val();
    var AccType = clickedrow.find("#hdAccType").val();/*Add by Hina on 28-10-2025*/
    debugger;  
    $("#IconHdEntityID").val(AccID);
    $("#IconEntityName").val(AccName); 
    $("#IconHdAcctype").val(AccType);/*Add by Hina on 28-10-2025*/
    if (DocId == '105104115120') {
        $("#IconPayAmt").val(DbtAmnt);
    }
    else {
        $("#IconPayAmt").val(CrdtAmnt);
    }

    sessionStorage.removeItem("FromDateToDate");
    sessionStorage.setItem("FromDateToDate", "N");

    var Status = $("#hdBPStatus").val();
    if ($("#InterBranch").is(":checked")) {//Added by Suraj on 16-08-2024
        $("#div_PopUpGlBrName").attr("hidden",false);
    } else {
        //GlBrID = "";
        $("#div_PopUpGlBrName").attr("hidden", true);
    }
    $("#IconHdGlBranchID").val(GlBrID);//Added by Suraj on 16-08-2024
    $("#IconGlBranchName").val(GlBrName);//Added by Suraj on 31-08-2024
    BillWiseDetail(AccID, GlBrID);
}
function BillWiseDetail(AccID, GlBrID) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    docid = $("#DocumentMenuId").val();
    var DocumentNumber = $("#BankPaymentNo").val();  
    var Status = $("#hdBPStatus").val();
    var flag = '';
    if (docid == '105104115120') {
        flag = "P";
    }
    else {
        flag = "R";
    }
    //var FromTodate = sessionStorage.getItem("FromDateToDate");/*commented by Hina on 11-11-2024 to change discuss by vishal sir*/
    //document.getElementById("txtFromdate").setAttribute('max', $("#txtTodate").val());/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
    //document.getElementById("txtTodate").setAttribute('max', $("#txtTodate").val());

    //if (FromTodate == "Y") {/*commented by Hina on 11-11-2024 to change discuss by vishal sir*/
    /*var fromdt = $("#txtFromdate").val();commented by hina on 09-06-2025 discus with vishal sir*/
    /*start commented  and modify below by Hina on 09-06-2025 discus with vishal sir for show data as voucher date should be less then equal to bill dt for BP,CP*/
    var VouDt = $("#BPDate").val();
    var todt = $("#txtTodate").val();
    var fromdt = "";
        /*var todt = $("#BPDate").val();*//*commented by Hina on 11-11-2024 to change discuss by vishal sir*/
    //}
    //else {
    //    //var fromdt = null;/*commented by Hina on 11-11-2024 to change discuss by vishal sir*/
    //    //var todt = null;
    //    var fromdt = $("#txtFromdate").val();/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
    //    var todt = $("#txtTodate").val();
    //}
    var Curr = $("#curr").val();
    if (AccID != "" && AccID != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/BankPayment/GetBillDetail",
                data: {
                    AccId: AccID, fromdt: fromdt, todt: todt, flag: flag, DocumentNumber: DocumentNumber
                    , Status: Status, Curr: Curr, GlBrID: GlBrID
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
                                //var Rem_Bal = parseFloat(cmn_ReplaceCommas(arr.Table[i].pend_amt)).toFixed(ValDecDigit);
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
                                //var UnAdjustedAmount = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt), ValDecDigit);
                                var UnAdjustedAmount = parseFloat(cmn_ReplaceCommas(Pay_Amount)) + parseFloat(cmn_ReplaceCommas(Rem_Bal));
                                //if ($("#hdBPStatus").val() == "A") {
                                //    UnAdjustedAmount = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt) - (- cmn_ReplaceCommas(Pay_Amount)), ValDecDigit);
                                //}
                                var td_billNoAndDate = '';
                                var printPDF = "";
                                var printFlag = arr.Table[i].PrintFlag;//Add By shubham maurya on 13-06-2025 for 
                                if (printFlag == "Y") {
                                    printPDF = `<div class="col-sm-2 i_Icon no-padding">
                                            <button type="button" id="" class="calculator" onclick="" data-toggle="modal" data-target="#" data-backdrop="static" data-keyboard="false">
                                                <img src="/Content/Images/pdf.png" alt="" onclick="GetPopupInoviceDetailsPDF(event)" title="${$("#span_InvoiceDetail").text()}">
                                            </button>
                                        </div>`
                                }
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
                       `+ printPDF +`
                        </td>
                        <td><input id="InvoiceDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].inv_dt}"></td>
                        `+ td_billNoAndDate + `<td><input id="Currency" class="form-control center" autocomplete="off" type="text" disabled value="${arr.Table[i].curr}"></td>
                        <td><input id="InvoiceAmountInBase" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInBase" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_bs}"></td>
                        <td><input id="InvoiceAmountInSpecific" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInSpecific" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_sp}"></td>
                        <td class="num_right">
                                        <div class="col-sm-10 lpo_form no-padding" id="multiWrapper">
<input id="UnAdjustedAmount"  class="form-control num_right" autocomplete="off" type="text" name="UnAdjustedAmount" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(UnAdjustedAmount), ValDecDigit))}">
</div>
                                        <div class="col-sm-2 i_Icon">
                                            <button type="button" id="btn_AdjustedAmountDetails" class="calculator" data-toggle="modal" onclick="OnclickAdjustedAmtDetailIcon(event)" data-target="#AdjustedAmountDetails" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AdjustAmountDetail").text()}">  </button>
                                        </div>
                                    </td>

<td><div class="lpo_form"><input id="PayAmount"  class="form-control num_right" onchange="OnChangePayAmount(event)" onkeypress="return AmountFloatVal(this, event)" value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Pay_Amount), ValDecDigit))}" type="text" placeholder="0000.00">
                        <span id="PayAmount_Error" class="error-message is-visible"></span></div></td>                        
                        <td><input id="RemainingBalance" class="form-control num_right" autocomplete="off" type="text" name="RemainingBalance" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Rem_Bal), ValDecDigit))}"></td>               
                </tr>`);
                                debugger;
                                //if (arr.Table[0].min_dt == "") {/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
                                //    Cmn_BindVouDate(todt, todt);
                                //}
                                //else {
                                /*start commented by Hina on 09-06-2025 discus with vishal sir for show data as voucher date should be less then equal to bill dt for BP,CP*/
                                    //document.getElementById("txtFromdate").setAttribute('min', arr.Table[0].min_dt);/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
                                    //document.getElementById("txtTodate").setAttribute('min', arr.Table[0].min_dt);/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
                                    //$("#txtFromdate").val(arr.Table[0].min_dt);/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
                                /*End commented by Hina on 09-06-2025 discus with vishal sir for show data as voucher date should be less then equal to bill dt for BP,CP*/

                                /*}*/
                            }


                            DisableDetail(Status);
                            //CalculateTotalAmt();
                        }
                        else {
                            Cmn_BindVouDate(todt, todt);/*Add by Hina on 11-11-2024 to change discuss by vishal sir*/
                        }
                        //parseFloat(Rem_Bal).toFixed(ValDecDigit)
                        CalculateTotalAmt();
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
function GetPopupInoviceDetailsPDF(e) {
    debugger; debugger;
    var clickedrow = $(e.target).closest("tr");
    var Invoiceno = clickedrow.find("#InvoiceNumber").val().trim();
    var InvDate = clickedrow.find("#InvoiceDate").val();
    var Invno = Invoiceno.split("/");
    var INo = Invno[3];
    var code = INo.split("0");
    var Doccode = code[0];
    if (Doccode == "DPI" || Doccode == "CIN" || Doccode == "SPI" || Doccode == "SJI" || Doccode == "IPI" || Doccode == "ESI"  || Doccode == "PDI" || Doccode == "SCI" || Doccode == "DSI" || Doccode == "SSI") {
        window.location.href = "/ApplicationLayer/AccountPayable/GenerateInvoiceDetails?invNo=" + Invoiceno + "&invDate=" + InvDate + "&dataType=" + Doccode;
    }
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
    var Payamount=0;
    var PendingAmt=0;
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
    var PayAmount = currentrow.find("#PayAmount").val();
    if (AvoidDot(PayAmount) == false) {
        PayAmount = "";
        currentrow.find("#PayAmount").val("")
    }
    else {
        //currentrow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PayAmount)).toFixed(ValDecDigit))
        currentrow.find("#PayAmount").val(toDecimal(cmn_ReplaceCommas(PayAmount),ValDecDigit))
    }
    var PendingAmt = cmn_ReplaceCommas(currentrow.find("#UnAdjustedAmount").val());
    var RemainingAmt = cmn_ReplaceCommas(currentrow.find("#RemainingBalance").val());
    debugger;
    if (PayAmount != "") {
        if (parseFloat(cmn_ReplaceCommas(PayAmount)) != parseFloat(0)) {
            if (parseFloat(cmn_ReplaceCommas(PayAmount)) > parseFloat(cmn_ReplaceCommas(PendingAmt))) {
                debugger;
                currentrow.find("#PayAmount_Error").text($("#ExceedingAmount").text());
                currentrow.find("#PayAmount_Error").css("display", "block");
                currentrow.find("#PayAmount").css("border-color", "red");
            }
            else {
                var RemAmt = parseFloat(cmn_ReplaceCommas(PendingAmt)) - parseFloat(cmn_ReplaceCommas(PayAmount));
                currentrow.find("#PayAmount_Error").css("display", "none");
                currentrow.find("#PayAmount").css("border-color", "#ced4da");
                //var test = parseFloat(cmn_ReplaceCommas(PayAmount)).toFixed(ValDecDigit);
                //var test = toDecimal(cmn_ReplaceCommas(PayAmount),ValDecDigit);
                currentrow.find("#PayAmount").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit)));
                //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemAmt)).toFixed(ValDecDigit));
                currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(RemAmt),ValDecDigit)));
            }
        }
        else {
            //var test = parseFloat(0).toFixed(ValDecDigit);
            currentrow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
            //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
            currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PendingAmt),ValDecDigit)));
        }
    }
    else {
        //var test = parseFloat(0).toFixed(ValDecDigit);
        currentrow.find("#PayAmount").val(parseFloat(0).toFixed(ValDecDigit));
        //currentrow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
        currentrow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PendingAmt),ValDecDigit)));
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
            TotalAmount = parseFloat(cmn_ReplaceCommas(TotalAmount)) + parseFloat(cmn_ReplaceCommas(PayAmount));
        }
    });
    //$("#TotalPaid").text(parseFloat(cmn_ReplaceCommas(TotalAmount)).toFixed(ValDecDigit));
    $("#TotalPaid").text(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit)));
}
function OnClickSaveAndExitBillDt() {
    debugger
    var AccID = $('#IconHdEntityID').val();
    var InterBranchId = $('#IconHdGlBranchID').val();
    var ValDecDigit = $("#ValDigit").text();    
    if (CheckBillDetailValidation() === true) {
        CalculateTotalAmt();
        let NewArr = [];
        var PayAmount=0;
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
                        var IntBranchId = FBillDetails[i].GlBrId;
                        if (SAccID == AccID && IntBranchId == InterBranchId) {
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
                        var InvAmtInBase = currentRow.find("#InvoiceAmountInBase").val();
                        var InvAmtInSp = currentRow.find("#InvoiceAmountInSpecific").val();
                        var PendAmount = currentRow.find("#UnAdjustedAmount").val();
                        var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                        if (PayAmt == "") {
                            PayAmount = 0;
                        }
                        else {
                            PayAmount = PayAmt;
                        }
                        var RemBal = currentRow.find("#RemainingBalance").val();
                        var GlBrId = $("#IconHdGlBranchID").val();
                        NewArr.push({
                            AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                            , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                            , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                            , BillNo: BillNo, BillDate: BillDate, GlBrId: GlBrId
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
                        var InvAmtInBase = currentRow.find("#InvoiceAmountInBase").val();
                        var InvAmtInSp = currentRow.find("#InvoiceAmountInSpecific").val();
                        var PendAmount = currentRow.find("#UnAdjustedAmount").val();
                        var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                        if (PayAmt == "") {
                            PayAmount = 0;
                        }
                        else {
                            PayAmount = PayAmt;
                        }
                        var RemBal = currentRow.find("#RemainingBalance").val();
                        var GlBrId = $("#IconHdGlBranchID").val();
                        BillDetailList.push({
                            AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                            , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                            , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                            , BillNo: BillNo, BillDate: BillDate, GlBrId: GlBrId
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
                    var InvAmtInBase = currentRow.find("#InvoiceAmountInBase").val();
                    var InvAmtInSp = currentRow.find("#InvoiceAmountInSpecific").val();
                    var PendAmount = currentRow.find("#UnAdjustedAmount").val();
                    var PayAmt = cmn_ReplaceCommas(currentRow.find("#PayAmount").val());
                    if (PayAmt == "") {
                        PayAmount = 0;
                    }
                    else {
                        PayAmount = PayAmt;
                    }
                    var RemBal = currentRow.find("#RemainingBalance").val();
                    var GlBrId = $("#IconHdGlBranchID").val();
                    BillDetailList.push({
                        AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                        , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                        , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                        , BillNo: BillNo, BillDate: BillDate, GlBrId: GlBrId
                    })
                });
                sessionStorage.setItem("BillDetailSession", JSON.stringify(BillDetailList));
            }
            debugger;
            $("#BillSaveAndExitBtn").attr("data-dismiss", "modal");
            var row = $("#BPdetailTbl > tbody >tr #hfAccID[value=" + AccID + "]").closest('tr')
                .find("#hfGlBranchID[value=" + InterBranchId + "]").closest('tr');
            ValidateEyeColor(row, "BillAdjustment", "N");
        }
        else {
            $("#BillSaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#AdjustedAmountMismatch").text(), "warning");
            return false;
        }
       
       
    }
    else {
        $("#BillSaveAndExitBtn").attr("data-dismiss", "");
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
        if (parseFloat(cmn_ReplaceCommas(AutopayAmt)) > parseFloat(cmn_ReplaceCommas(PendingAmt))) {
            //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(PendingAmt)).toFixed(ValDecDigit));
            CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PendingAmt),ValDecDigit)));
            AutopayAmt = parseFloat(cmn_ReplaceCommas(AutopayAmt)) - parseFloat(cmn_ReplaceCommas(PendingAmt));
        }
        else {
            //CurrentRow.find("#PayAmount").val(parseFloat(cmn_ReplaceCommas(AutopayAmt)).toFixed(ValDecDigit));
            CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(AutopayAmt),ValDecDigit)));
            AutopayAmt = parseFloat(cmn_ReplaceCommas(AutopayAmt)) - parseFloat(cmn_ReplaceCommas(AutopayAmt));
        }
        
        var Payamount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        var RemaingBal = (parseFloat(cmn_ReplaceCommas(PendingAmt)) - parseFloat(cmn_ReplaceCommas(Payamount)))
        //CurrentRow.find("#RemainingBalance").val(parseFloat(cmn_ReplaceCommas(RemaingBal)).toFixed(ValDecDigit));
        CurrentRow.find("#RemainingBalance").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(RemaingBal),ValDecDigit)));
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
                CurrentRow.find("#PayAmount").val(cmn_addCommas(toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit)));
            }
        }
        else {
            var TotalPaidAmt = cmn_ReplaceCommas($("#TotalPaid").text());
            if (parseFloat(TotalPaidAmt) > parseFloat(0)) {
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
    sessionStorage.setItem("FromDateToDate","Y");
    var AccID = $("#IconHdEntityID").val();
    var Br_Id = $("#IconHdGlBranchID").val();
    BillWiseDetail(AccID, Br_Id);
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
//function BindDate(fdt,todt) {
//    debugger;
//    //var voudate = $("#BPDate").val();/*Add by Hina on 11-11-2024 to */
//    //var date = new Date()
//    var date = new Date(todt), y = date.getFullYear(), m = date.getMonth();
//    var firstDay = new Date(y, m, 1);

//    //var now = new Date();
//    //var month = (now.getMonth() + 1);
//    //var day = now.getDate();
//    //if (month < 10)
//    //    month = "0" + month;
//    //if (day < 10)
//    //    day = "0" + day;
//    //var today = now.getFullYear() + '-' + month + '-' + day;

//    /*$("#txtTodate").val(today);*//*commented and modify by Hina on 11-11-2024 */
    
//    //$("#txtFromdate").val(firstDay);
//    //var fromDate = new Date($("#hdFromdate").val());
    
//    var month = (firstDay.getMonth() + 1);
//    var day = firstDay.getDate();
//    if (month < 10)
//        month = "0" + month;
//    if (day < 10)
//        day = "0" + day;
//    var today = firstDay.getFullYear() + '-' + month + '-' + day;

//    if (fdt != "")
//    { $("#txtFromdate").val(today); }
//else
//    {
//        $("#txtFromdate").val("");
//        $("#txtTodate").val(todt);
//    }
    
    
//}
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
                var InvAmtInBase = cmn_ReplaceCommas(FBillWiseAdjDetails[i].InvAmtInBase); //cmn_ReplaceCommas added by Suraj on 14-08-2024
                var InvAmtInSp = cmn_ReplaceCommas(FBillWiseAdjDetails[i].InvAmtInSp);//cmn_ReplaceCommas added by Suraj on 14-08-2024
                var PendAmount = cmn_ReplaceCommas(FBillWiseAdjDetails[i].PendAmount);//cmn_ReplaceCommas added by Suraj on 14-08-2024
                var PayAmount = cmn_ReplaceCommas(FBillWiseAdjDetails[i].PayAmount);//cmn_ReplaceCommas added by Suraj on 14-08-2024
                var RemBal = cmn_ReplaceCommas(FBillWiseAdjDetails[i].RemBal);//cmn_ReplaceCommas added by Suraj on 14-08-2024
                var BillNo = FBillWiseAdjDetails[i].BillNo;
                var BillDate = FBillWiseAdjDetails[i].BillDate;
                var GlBrId = IsNull(FBillWiseAdjDetails[i].GlBrId,'');
                BillWiseList.push({
                    AccID: AccID, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate
                    , Currency: Currency, InvAmtInBase: InvAmtInBase, InvAmtInSp: InvAmtInSp
                    , PendAmount: PendAmount, PayAmount: PayAmount, RemBal: RemBal
                    , BillNo: BillNo, BillDate: BillDate, GlBrID: GlBrId
                })
            }
        }
    }
    debugger;
    return BillWiseList;


};
function CheckBillAdjAccountDetails() {/*Created by Suraj on 05-09-2024 for Checking Bill Adjustment Detail Account wise*/
    debugger;
    var FBillWiseAdjDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
    var BillWiseList = [];
    var ErrorFlg = "N"
    if (FBillWiseAdjDetails != null) {

        if (FBillWiseAdjDetails.length > 0) {
            $("#BPdetailTbl TBODY TR").each(function () {
                debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                var intBrId = IsNull(currentRow.find("#GLbranch_" + rowid).val(),"0");
                var glAccId = currentRow.find("#GLAccount_" + rowid).val();
                var drAmt = currentRow.find("#DebitAmountInSpecific").val();
                var crAmt = currentRow.find("#CreditAmountInSpecific").val();
                var billData = [];
                if ($("#InterBranch").is(":checked")) {
                    billData = FBillWiseAdjDetails.filter(v => v.GlBrId == intBrId && v.AccID == glAccId);
                } else {
                    billData = FBillWiseAdjDetails.filter(v => v.AccID == glAccId);
                }
                
                var totalBillAmt = 0;
                if (billData.length > 0) {
                    billData.map((item) => {
                        totalBillAmt = parseFloat(totalBillAmt) + parseFloat(cmn_ReplaceCommas(item.PayAmount));
                    });
                    if (parseFloat(cmn_ReplaceCommas(CheckNullNumber(drAmt))) + parseFloat(cmn_ReplaceCommas(CheckNullNumber(crAmt))) >= totalBillAmt) {
                        ValidateEyeColor(currentRow, "BillAdjustment", "N");
                    } else {
                        ErrorFlg = "Y";
                        ValidateEyeColor(currentRow, "BillAdjustment", "Y");
                    }
                }
                
            });
            
        }
    }
    if (ErrorFlg == "Y") {
        return false;
    } else {
        return true;
    }


};

function onclkDtlSaveBtnValidateBillAd() {
    debugger;
    Flag = "N";

    $("#BPdetailTbl > tbody > tr").each(function () {
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
                CurrRow.find("#BillAdjustment").css("border-color", "red");
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
function DeletebillAdjustment(hfAccID,hfIntBrId) {
    debugger;
    var DeleteDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
    var DelteArrData = [];
    if (DeleteDetails != null) {
        if (DeleteDetails.length > 0) {
            for (i = 0; i < DeleteDetails.length; i++) {
                var Acc_id = DeleteDetails[i].AccID;
                var Gl_br_id = DeleteDetails[i].GlBrId;
                if (hfAccID == Acc_id && hfIntBrId == Gl_br_id) {
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

/*----This validation for if EntityName has select option then Account table validation should be remove----*/

function validationofRemoveAcountSecOnChngHeadSec() {
    var DocId = $("#DocumentMenuId").val();
    if ($("#BPdetailTbl tbody tr").length > 0) {
        if (DocId == '105104115120' || DocId == '105104115110') {
            $("#BPdetailTbl tbody tr").each(function () {
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
}

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
    var TotalAmt1 = 0;//add by sm 04-12-2024
    var Doc_ID = $("#DocumentMenuId").val();//add by sm 04-12-2024
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = cmn_ReplaceCommas(clickedrow.find("#DebitAmountInSpecific").val());
    var CstCrtAmt = cmn_ReplaceCommas(clickedrow.find("#CreditAmountInSpecific").val());
    var SNohf = clickedrow.find('#SNohf').val();
    var GLAcc_Name = clickedrow.find('#GLAccount_' +  SNohf +' option:selected').text();
    var GLAcc_id = clickedrow.find('#GLAccount_'+ SNohf +' option:selected').val();
    var GL_int_br_id = clickedrow.find('#GLbranch_'+ SNohf +' option:selected').val();
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        if (row.find('#hdntbl_CstIntBrId').text() == GL_int_br_id) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            var amount = cmn_ReplaceCommas(row.find("#hdntbl_CstAmt").text());
            //var amount1 = toDecimal(amount, ValDigit);
            List.CC_Amount = cmn_addCommas(toDecimal(amount, ValDigit));
            List.ddl_int_br_id = row.find('#hdntbl_CstIntBrId').text();
            TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amount);//add by sm 04-12-2024
            NewArr.push(List);
        }
    })
    var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 06-12-2024
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BankReceipt/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            TotalAmt: TotalAmt,//add by sm 06-12-2024
            Doc_ID: Doc_ID//add by sm 06-12-2024     

        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(cmn_addCommas(Amt));
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("BPdetailTbl");
            $("#CC_int_br_id").val(GL_int_br_id);
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

//------------------------Unclear_rcpt_paydetails----------------------------
function onclickIcon_rcptpaydetails(rtype) {

    var accid = $("#hdnacc_id").val();
    var Acc_Name = $("#GLName").val();
    var unclr_amt = "";
    if (rtype == "rcpt") {
        unclr_amt = $("#UnclearedReceipt").val();
    }
    if (rtype == "paym") {
        unclr_amt = $("#UnclearedBalance").val();
    }
     
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BankPayment/Get_rcptpaymhist",
        data: {
            Flag: rtype,
            Accid: accid
        },
        success: function (data) {
            debugger;
            $("#div_chequeDetails").html(data);

            $("#chq_GLName").val(Acc_Name);
            $("#chq_ApprovedBalance").val(unclr_amt);
            
        },
    })
}

//-----------------Work for PDC add by Hina on 08-08-2024 --------------------------------//
function OnClickPDCFlag() {
    debugger;
    if ($("#PDC").is(":checked")) {
        $('#Hdn_PDCFlag').val('Y');
        var mindt = $('#hdnVouMin_Dt').val();
        var maxdt = $('#hdnVouMaxDt').val();
        document.getElementById("BPDate").setAttribute('min', mindt);
        document.getElementById("BPDate").setAttribute('max', maxdt);
        $('#BPDate').val(mindt);
        //$("#BPDate").removeAttr("onchange")
        $('#ins_dt').val(mindt);
        $('#ins_dt').prop("readonly", true);

        $('#ins_type').val("C");//Added by Suraj on 30-08-2024
        $('#ins_type').attr("disabled", true);//Added by Suraj on 30-08-2024
        //var keyCode = (e.keyCode ? e.keyCode : e.which);
        //if (keyCode > 47 && keyCode < 58) {
        //    e.preventDefault();
        //}
        //document.getElementsByName("start")[0].setAttribute('max', date3m)
        /*$("#BPDate").datepicker({ minDate: mindt, maxDate: maxdt });*/
        //$("#BPDate").datepicker({ minDate: mindt, maxDate: maxdt, defaultDate: mindt });
    }
    else {
        $('#Hdn_PDCFlag').val('N');
        var CurrentDate = moment().format('YYYY-MM-DD');

        //var nowDate = new Date();
        //var date = nowDate.getFullYear() + '/' + (nowDate.getMonth() + 1) + '/' + nowDate.getDate();
        var mindt = '';
        var maxdt = CurrentDate;
        var dtae =
            document.getElementById("BPDate").setAttribute('min', mindt);
        document.getElementById("BPDate").setAttribute('max', maxdt);
        $("#BPDate").val(CurrentDate);
        $('#ins_dt').val('dd-mm-yyyy');
        $('#ins_dt').prop("readonly", false);

        $('#ins_type').attr("disabled", false);//Added by Suraj on 30-08-2024
    }

}
//function Check(e) {
//    var keyCode = (e.keyCode ? e.keyCode : e.which);
//    if (keyCode > 47 && keyCode < 58) {
//        e.preventDefault();
//    }
//}

//-----------------Work for Interbranch add by Hina on 08-08-2024 --------------------------------//
async function OnClickInterBrnchFlag() {/*add by Hina on 09-08-2024 */ //Modified by Suraj on 13-08-2024
    debugger
    let InterBranch = "N";
    if ($("#InterBranch").is(":checked")) {
        InterBranch = "Y";
    }
    $('#Hdn_IntBrFlag').val(InterBranch);
    if (InterBranch == "Y") {
        $("#BPdetailTbl thead tr #th_gl_br").attr("hidden", false);
        $("#BPdetailTbl tbody tr #td_gl_br").attr("hidden", false);
    
    } else {
        $("#BPdetailTbl thead tr #th_gl_br").attr("hidden", true);
        $("#BPdetailTbl tbody tr #td_gl_br").attr("hidden", true);
    }

}
async function bindGlBranchList(row_id,arr) {
    $("#GLbranch_" + row_id).empty();
    $("#GLbranch_" + row_id).append(`<option value="0">---Select---</option>`);
    for (var i = 0; i < arr.length; i++) {
        $("#GLbranch_" + row_id).append(`<option value="${arr[i].br_id}">${arr[i].br_name}</option>`);
    }
}
async function fetch_BranchList() {
    var arr = [];
    await $.ajax({
        type: "GET",
        url: "/ApplicationLayer/BankPayment/GetBranchList",
        data: {},
        success: function (data) {
            if (data == "Error") {
                console.log(data);
            } else {
                arr = JSON.parse(data);
            }
        }
    });
    return arr;
}

function OnChangeGlBranchName(el, evt) {
    var ClickRow = $(evt.target).closest('tr');
    var SNo = ClickRow.find("#SNohf").val();
    var glBrId = ClickRow.find("#GLbranch_" + SNo).val();
    
    debugger;
    
    if (glBrId == "0") {
        ClickRow.find("#GLbranch_" + SNo).css("border-color", "red");
        ClickRow.find("#GLbranchError").text($("#valueReq").text());
        ClickRow.find("#GLbranchError").css("display", "block");
        //ErrorFlag = "Y";
    }
    else {
        ClickRow.find("#GLbranch_" + SNo).css("border-color", "#ced4da");
        ClickRow.find("#GLbranchError").text("");
        ClickRow.find("#GLbranchError").css("display", "none");
    }
    $("#GLAccount_" + SNo).html(`<option value="0">---Select---</option>`);
    Cmn_BindAccountListAsync("#GLAccount_", SNo, "#BPdetailTbl", "#SNohf", "ResetAccount", "105104115120", glBrId);
    $("#GLAccount_" + SNo).trigger("change");
    ClickRow.find("#hfGlBranchID").val(glBrId);
    
    
    //Cmn_BindAccountListWithBranch("#GLAccount_", SNo, "#BPdetailTbl", "#SNohf", "", "105104115120", glBrId);
}
function ResetGlAccount(AccId) {
    $(AccId).val(0).trigger("change");
}
//Added by Nidhi on 06-09-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#BankPaymentNo").val();
    if (docid == "105104115120") {
        Cmn_SendEmail(docid, Doc_no, 'BP');
    }
    else {
        Cmn_SendEmail(docid, Doc_no, 'BR');
    }
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hdBPStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#BankPaymentNo").val();
    var Doc_dt = $("#BPDate").val();
    var statusAM = $("#Amendment").val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/Common/Common/SendEmailAlert", "")
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hdBPStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#BankPaymentNo").val();
    var Doc_dt = $("#BPDate").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
}
function EmailAlertLogDetails() {
    var status = $('#hdBPStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#BankPaymentNo").val();
    var Doc_dt = $("#BPDate").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//End on  06-09-2025

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

function OnclickAdjustedAmtDetailIcon(e) {/*Add by hina sharma on 28-10-2025*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InVNo = CurrentRow.find("#InvoiceNumber").val();
    var InvDate = CurrentRow.find("#InvoiceDate").val();
    var InvDt = CurrentRow.find("#InvoiceDate").val().split("-").reverse().join("-");
    var Curr = CurrentRow.find("#Currency").val();
    var InvAmtBs = CurrentRow.find("#InvoiceAmountInBase").val();
    var InvAmtSpc = CurrentRow.find("#InvoiceAmountInSpecific").val();
    var AccTyp = $("#IconHdAcctype").val();
    var Accid = $("#IconHdEntityID").val();
    var VouNo = $("#BankPaymentNo").val();
    
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/BankReceipt/ClickAdjustedAmountDetail",
            data: {
                InVNo: InVNo,
                InvDate: InvDate,
                Accid: Accid,
                VouNo: VouNo,
                AccTyp: AccTyp
            },
            success: function (data) {
                debugger;
                $('#AdjustedAmountDetailsPopup').html(data);
                $("#Prtal_cmnInvoiceNumber").val(InVNo);
                $("#Prtal_cmnInvoiceDate").val(InvDt);
                $("#Prtal_cmnCurr").val(Curr);
                $("#Prtal_cmnInvoiceAmtBs").val(InvAmtBs);
                $("#Prtal_cmnInvoiceAmtSpc").val(InvAmtSpc);
               

                cmn_apply_datatable("#tbl_AdjustedAmtDetails");

                //    $("#tbl_PaidAmtDetails_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //    $("#tbl_PaidAmtDetails_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountRecevablPaidAmtInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });

    } catch (err) {
        debugger;
        console.log("Trial Balance Error : " + err.message);
    }
}