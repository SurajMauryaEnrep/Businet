$(document).ready(function () {
    debugger;
    $("#ddlBankList").select2();
    var fromDate = $("#BkReFromdate").val();
    var ToDate = $("#BkReTodate").val();
    $("#HdnfromDate").val(fromDate);
    $("#HdnToDate").val(ToDate);
});

function BankRevalidation() {
    debugger;
    var ddlBankList = $("#ddlBankList").val();
    var Flag = 'N';
    if (ddlBankList == '0' || ddlBankList == '' || ddlBankList == null) {
        
        $("#ddlBankList").css("border-color", "red");
        $('[aria-labelledby="select2-ddlBankList-container"]').css("border-color", "red");
        $("#Spn_BankList").text($("#valueReq").text());
        $("#Spn_BankList").css("display","block");
        Flag = 'Y';
    }
    if (Flag == 'Y') {
        return false; }
    else {
        return true; }
}
function onchangeGetAccountBal() {
    debugger;
    var ToDate = $("#BkReTodate").val();
    var ddlbankAcc_Id = $("#ddlBankList option:selected").val();
    /*FromToDateValidation('N');*/
    if (ddlbankAcc_Id != 0 || ddlbankAcc_Id != '0') {
        $.ajax({
            url: "/ApplicationLayer/BankReconcillation/GetAccBalDetail",
            type: "POST",
            data: {
                Date: ToDate,
                acc_id: ddlbankAcc_Id
            },
            success: function (data) {
                debugger;
                if (data != null) {
                    var arr = [];
                    arr = JSON.parse(data);
                    var ClosingBal = arr.Table[0].ClosBL;
                    $("#Account_Bal").val(ClosingBal);
                }
            }
        })
    }
}
function onchangeBankName() {
    debugger;
    var ddlbankAcc_Id = $("#ddlBankList option:selected").val();
    var FromDate = $("#BkReFromdate").val();
    var ToDate = $("#BkReTodate").val();
    $("#hdn_AccId").val(ddlbankAcc_Id);
    
    $("#Spn_BankList").css("display", "none");
    $('[aria-labelledby= "select2-ddlBankList-container"]').css("border-color", "#ced4da");
    if (ddlbankAcc_Id != 0 || ddlbankAcc_Id != "") {
        $.ajax({
            url: "/ApplicationLayer/BankReconcillation/GetCurrName",
            type: "POST",
            data: {
                BankId: ddlbankAcc_Id,
                FromDate: FromDate,
                ToDate: ToDate
            },
            success: function (data) {
                debugger;
                if (data != null || data != "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    $("#txtCurrName").val(arr.Table[0].curr_name);
                    $("#Account_Bal").val(arr.Table1[0].ClearBal);
                }
            },
        })
    }
};
function GetFromToDateBankRe() {
    debugger;
    var Flag = '';
    var FromDate = $("#BkReFromdate").val();
    var ToDate = $("#BkReTodate").val();
    var Stdate = getDate(FromDate);
    var StartYear = Stdate.getFullYear();
    
    //if (FromToDateValidation('Y') == false) {
    //    return false;
    //}
   
    if (!isNaN(StartYear)) {
        $.ajax({
            url: "/ApplicationLayer/BankReconcillation/OnchangeGetFyDate",
            type: "Post",
            data: {
                Year: StartYear,
                ToDate: ToDate,
                Flag: Flag,
                FromDate: FromDate
            },
            success: function (data) {
                debugger;
                $("#PartialBankrecnclToDate").html(data);
            }
        })
    }
}
function getDate(input) {
    var arr = [];
    var seperator = ['/', '-'];
    var year, month, date;
    var yearIndex = 2;
    var result = undefined;
    seperator.forEach(function (s) {
        if (input.indexOf(s) > -1)
            arr = input.split(s);
    });
    if (arr.length > 1) {
        if (arr[0] > 1000) {
            year = arr[0]
            yearIndex = 0;
        } else if (arr[2] > 1000) {
            year = arr[2];
            yearIndex = 2;
        }
        if (yearIndex === 0) {
            month = arr[1]
            date = arr[2]
        } else {
            if (arr[0] > 12) {
                date = arr[0]
                month = arr[1]
            } else if (arr[1] > 12) {
                date = arr[1]
                month = arr[0]
            }
            else {
                date = arr[0]
                month = arr[1]
            }
        }
        result = new Date(year, month - 1, date);
    }
    return result;
}
function OnclickBnkReSearchButton() {
    debugger;
    if (BankRevalidation() == false) {
        return false;
    }
    //var ValDigit = $("#ValDigit").text();
    var Acc_Id = $("#hdn_AccId").val();
    var FromDate = $("#BkReFromdate").val();
    var ToDate = $("#BkReTodate").val();
    var TransType = $("#BankRe_ddlTransType option:selected").val();
    var Status = $("#BankRe_ddlstatus option:selected").val();
    $.ajax({
        url: "/ApplicationLayer/BankReconcillation/SearchBankRecandetails",
        type: "Post",
        data: {
            acc_id: Acc_Id,
            FromDate: FromDate,
            ToDate: ToDate,
            TransType: TransType,
            Status: Status
        },
        success: function (data) {
            debugger;
            $("#SearchBankRecandetails").html(data);
            var Bankname = $("#Par_BankName").text();
            if (Bankname != "") {
                $("#BtnSearch").css("display", "none");
                $("#SaveTitle").attr("disabled", false).removeAttr("style");
                //$("#datatable-buttons5 > tbody > tr").remove();
                $("#ddlBankList").attr("disabled", true);
                $("#BkReFromdate").attr("disabled", true);
                $("#BkReTodate").attr("disabled", true);
                $("#BankRe_ddlTransType").attr("disabled", true);
                $("#BankRe_ddlstatus").attr("disabled", true);
            }
            //$("#datatable-buttons5_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
            //$("#datatable-buttons5_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" onclick="bankRecancellationCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')//commanted by shubhamMaurya on 19-09-2025
        }
    })
}
function bankRecancellationCSV() {
    debugger;
    $("#HdnCsvPrint").val("CsvPrint");
    var TransType = $("#BankRe_ddlTransType option:selected").val();
    var Status = $("#BankRe_ddlstatus option:selected").val();
    var ToDate = $("#BkReTodate").val();
    var Acc_Id = $("#hdn_AccId").val();
    $("#HdnStatus").val(Status);
    $("#hdnTransactionType").val(TransType);
    $("#hdnToDate").val(ToDate);
    $("#hdnAcc_id").val(Acc_Id);

    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function RestrictKeyboardentry(Date_Id) {
    debugger;
    $("#" + Date_Id).keypress(function (e) {
            e.preventDefault();
    })
    return false;
}
function disableInputBoxes() {
    debugger;
    /*if ($("#datatable-buttons5 > tbody > tr").length > 0) {*/
        $("#datatable-buttons5 > tbody > tr").remove();
        $("#ddlBankList").attr("disabled", true);
        $("#BkReFromdate").attr("disabled", true);
        $("#BkReTodate").attr("disabled", true);
        $("#BankRe_ddlTransType").attr("disabled", true);
        $("#BankRe_ddlstatus").attr("disabled", true);
    //}
}
function onclickSaveBtnBankRe() {
    if (BankRevalidation() == false) {
        return false;
    }
    var FinalBankRecoVouDetail = [];
    FinalBankRecoVouDetail = InsertBankReVouDetails();
    var BanRecVouDetail = JSON.stringify(FinalBankRecoVouDetail);
    if (CheckAdjustmentAgainstVoucher(BanRecVouDetail) == true) {
        return false;
    }
    $('#hdn_saveBanReVouList').val(BanRecVouDetail);
    $('#hfcommand').val('save');
    return true;
}
function InsertBankReVouDetails() {
    debugger;
    var BankReVouList = new Array();
    var ddlBankListId = $("#ddlBankList option:selected").val();
    
    if (ddlBankListId != null  && ddlBankListId != '' && ddlBankListId != "0")
        $("#datatable-buttons5 > tbody > tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            var Row = CurrentRow.find("#rowId").val();
            var VouList = {};
            VouList.BankName = CurrentRow.find("#Par_BankName").text();
            VouList.Acc_id = $("#hdn_AccId").val();
            VouList.VoucherNum = CurrentRow.find("#Par_VouNo").text();
            VouList.VoucherDate = CurrentRow.find("#hdnVouDt").val();
            VouList.Vouchertype = CurrentRow.find("#Par_VouType").text();
            VouList.InstrumentNum = CurrentRow.find("#Par_InsNum").text();
            VouList.InstrumentDate = CurrentRow.find("#hdnInsDate").val();
            //***Modifyed By Shubham Maurya on 29-01-2024***//
            //VouList.AmountBase = CurrentRow.find("#Par_Amtbs").text();
            var Par_Amtbs = CurrentRow.find("#Par_Amtbs").text();
            VouList.AmountBase = cmn_ReplaceCommas(Par_Amtbs);
            //VouList.AmoountSpec = CurrentRow.find("#Par_Amtsp").text();
            var Par_Amtsp = CurrentRow.find("#Par_Amtsp").text();
            VouList.AmoountSpec = cmn_ReplaceCommas(Par_Amtsp);
            VouList.Status = CurrentRow.find("#hdn_status_" + Row + "").val();
            if (VouList.Status == "Un-Cleared") {/*Add by Hina sharma on 15-07-2025*/
                VouList.Status = "U";
            }
            VouList.ClearDate = CurrentRow.find("#hdn_ClDt_" + Row +"").val();
            VouList.RsonFrReturn = CurrentRow.find("#Par_RsonFrRet_" + Row +"").text();
            BankReVouList.push(VouList);
           
        })
    return BankReVouList;
}
function OnclickCalculatorIcon(e, row) {
    debugger;
    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    /*----------whole function Modify by Hina on 11-07-2025------------*/
    var ClickedRow = $(e.target).closest("tr");
    var Rowno = ClickedRow.find("#rowId").val();
    var PrStatus = ClickedRow.find("#hdn_status_" + Rowno).val();

    /*  if (PrStatus == "Un-Cleared") {*/
    var VouNo = ClickedRow.find("#Par_VouNo").text();
    var VouDate = ClickedRow.find("#Par_Voudt").text();
    var VouDate1 = VouDate.split("-");
    var VouDate2 = VouDate1[2] + '-' + VouDate1[1] + '-' + VouDate1[0];
    var BnkName = $("#Par_BankName").text();
    var InsNum = ClickedRow.find("#Par_InsNum").text();
    var InsDt = ClickedRow.find("#Par_Insdt").text();
    var InstruDt = ClickedRow.find("#hdnInsdt").val();
    var InstruDate = InstruDt;//formatDateYYYYMMDD(InstruDt);

    /* Added by Suraj Maurya on 02-09-2025 */
    $("#HdnParstVouNo").val(VouNo);
    $("#HdnParstVouDate").val(VouDate2);
    /* Added by Suraj Maurya on 02-09-2025 */

    $("#span_RsnReturn").hide();
    $("#Span_ClDate").show();
    /*$("#HdnParstInsDt").val(InsDt);*/
    $("#ParSt_ClDate").attr("min", VouDate2)
    $("#ParSt_ClDate").attr("max", today)

    $("#ParSt_BankName").val(BnkName);
    $("#HdnParstVouDt").val(VouDate);
    $("#ParSt_InstrumentNumber").val(InsNum);
    $("#ParSt_Insdate").val(InsDt);
    //$("#ParSt_ClDate").val(null);
    $("#ParSt_RsnfrRet").val(null);
    $("#HdnTr_Id").val(row);

    $("#ParSt_ClDate").prop("disabled", false);
    $("#ParSt_RsnfrRet").prop("disabled", true);
    $("#ParSt_ClDate").css("border-color", "#ced4da");
    $("#Spn_parstupd").css("display", "none");
    $("#ParSt_RsnfrRet").css("border-color", "#ced4da");
    $("#Spn_parRsnFrRet").css("display", "none");
    $("#ParSt_ddlstatus").val('C');
    /*  }*/
    
      if (PrStatus == "Un-Cleared") {
          $("#ParSt_ClDate").val(null);
    }
    if (PrStatus == "R" || PrStatus == "Returned") {
        /*-------------code start Add by Hina Sharma on 15-07-2025-------------- */
        $("#ParSt_ClDate").attr("min", '')
        $("#ParSt_ClDate").attr("max", '')
        /*-------------code end Add by Hina Sharma on 15-07-2025-------------- */
        $("#ParSt_ClDate").val(null);
        $("#ParSt_ClDate").prop("disabled", true).val("yyyy-MM-dd");
        $("#ParSt_RsnfrRet").prop("disabled", false);
        var Reason = $("#Par_RsonFrRet_" + Rowno).text();/*---Add by Hina Sharma on 15-07-2025----- */
        $("#ParSt_RsnfrRet").val(Reason);/*---Add by Hina Sharma on 15-07-2025----- */
        $("#Span_ClDate").hide();
        $("#span_RsnReturn").show();
        $("#ParSt_ddlstatus").val('R');

    }
    if (PrStatus == "Cleared" || PrStatus == "C") {
        var Cldate = $("#Par_Cldt_" + Rowno).text();
        if (Cldate != isNaN && Cldate != "" && Cldate != null) {
            var Cldate1 = Cldate.split('-')
            var Cldate2 = Cldate1[2] + '-' + Cldate1[1] + '-' + Cldate1[0];
            $("#ParSt_ClDate").val(Cldate2);
            
        }
    }
   
    
    
}
function OnchangeddlStatus() {
    debugger;
    if ($("#ParSt_ddlstatus").val() == 'C') {
    /*-------------code start Add by Hina Sharma on 15-07-2025-------------- */
        var now = new Date();
        var month = (now.getMonth() + 1);
        var day = now.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = now.getFullYear() + '-' + month + '-' + day;

        var VouDate = $("#HdnParstVouDt").val()
        var VouDate1 = VouDate.split("-");
        var VouDate2 = VouDate1[2] + '-' + VouDate1[1] + '-' + VouDate1[0];
         $("#ParSt_ClDate").attr("min", VouDate2)
        $("#ParSt_ClDate").attr("max", today)
        /*-------------code end Add by Hina Sharma on 15-07-2025-------------- */

        $("#ParSt_ClDate").prop("disabled", false);
        $("#ParSt_RsnfrRet").prop("disabled", true).val(null);
        $("#Span_ClDate").show();
        $("#span_RsnReturn").hide();
    }
    if ($("#ParSt_ddlstatus").val() == 'R') {
        /*-------------code start Add by Hina Sharma on 15-07-2025-------------- */
        $("#ParSt_ClDate").attr("min", '')
        $("#ParSt_ClDate").attr("max", '')
        /*-------------code end Add by Hina Sharma on 15-07-2025-------------- */
        $("#ParSt_ClDate").prop("disabled", true).val("yyyy-MM-dd");
        $("#ParSt_RsnfrRet").prop("disabled", false);
        $("#Span_ClDate").hide();
        $("#span_RsnReturn").show();
    }
    $("#ParSt_ClDate").css("border-color", "#ced4da");
    $("#ParSt_RsnfrRet").css("border-color", "#ced4da");
    $("#Spn_parstupd").css("display", "none");
    $("#Spn_parRsnFrRet").css("display", "none");
}
function CheckAdjustmentAgainstVoucher(BanRecVouDetail) {
    let IsUsed = false;
    let DocNo = $("#HdnParstVouNo").val();
    let DocDate = $("#HdnParstVouDate").val();
    $.ajax({
        url: "/ApplicationLayer/BankReconcillation/CheckAdvancePayment",
        type: "POST",
        data: {
            DocNo: DocNo,
            DocDate: DocDate,
            doc_List: BanRecVouDetail == null ? "" : BanRecVouDetail
        },
        async: false,
        success: function (data) {
            debugger;
            if (data != null && data != "") {
                if (data == "Used") {
                    IsUsed = true;
                    swal("", $("#AdjustmentMadeVouchercannotbemodified").text(), "warning");
                }
                else {
                    try {
                        var arr = JSON.parse(data);
                        if (arr.length > 0) {
                            IsUsed = true;
                            
                            let strMsg = $("#AdjustmentMadeVouchercannotbemodified").text();
                            let Tr_Id = "";
                            arr.map((item, index) => {
                                if (index == 0) {
                                    strMsg += " (" + item.Vou_no;
                                    strMsg += (index == (arr.length - 1)) ? ")" : "";
                                }
                                else if (index < (arr.length - 1)) {
                                    strMsg += ", " + item.Vou_no;
                                } else {
                                    strMsg += ", " + item.Vou_no + ")";
                                }
                                
                                var row = $("#datatable-buttons5 > tbody > tr #Par_VouNo:contains(" + item.Vou_no + ")").closest('tr');

                                row.each(function () {
                                    var cRow = $(this);
                                    let vou_no = cRow.find("#Par_VouNo").text();
                                    if (vou_no == item.Vou_no) {
                                        Tr_Id = cRow.find("#rowId").val();
                                    }
                                });

                                $("#Par_Cldt_" + Tr_Id).text(null);
                                $("#hdn_ClDt_" + Tr_Id).val(null);
                                $("#Par_Status_" + Tr_Id).text("Un-Cleared");
                                $("#hdn_status_" + Tr_Id).val("Un-Cleared");
                                $("#Par_RsonFrRet_" + Tr_Id).text("");
                                //$("#hdn_status").val(Status);
                                //$("#hdn_RsonFrReturn").val(RsForReturn);
                            });
                            swal("", strMsg, "warning");
                        }

                    }
                    catch (ex) {
                        IsUsed = true;
                        console.log(ex);
                    }
                }
            }
        }
    });
    return IsUsed;
}
function onClickSaveAndExit(e) {
    debugger;
    /* Added by Suraj Maurya on 03-09-2025 to check the check status */
    let Checkstatus = $("#ParSt_ddlstatus").val();
    if (Checkstatus == "R") {
        let checkPayment = CheckAdjustmentAgainstVoucher("");
        if (checkPayment == true) {
            $("#PartSt_SaveAndExit").attr("data-dismiss", "");
            return false;
        }
    }
    /* Added by Suraj Maurya on 03-09-2025 to check the check status */
    if (onChangeValidateDate() == false) {
        $("#PartSt_SaveAndExit").attr("data-dismiss", "");
        return false;
    }
    var Status = $("#ParSt_ddlstatus option:selected").text();
    var Status1 = $("#ParSt_ddlstatus option:selected").val();
    var Tr_Id = $("#HdnTr_Id").val();
    var RsForReturn = $("#ParSt_RsnfrRet").val();
    var Cldate = $("#ParSt_ClDate").val();
    var ClearDate = formatDateYYYYMMDD(Cldate);
    if (Cldate != isNaN && Cldate != "" && Cldate != null) {
        debugger;
        if ($("#ParSt_ddlstatus option:selected").val() == 'C') {
            var date = new Date(Cldate);
            var newDate = date.toString('dd-MM-yyyy');
            $("#Par_Cldt_" + Tr_Id).text(newDate);
            $("#hdn_ClDt_" + Tr_Id).val(ClearDate);
            $("#ParSt_ClDate").css("border-color", "#ced4da");
            $("#Spn_parstupd").css("display", "none");
            $("#Par_Status_" + Tr_Id).text(Status);
            $("#hdn_status_" + Tr_Id).val(Status1);
            $("#hdn_status").val(Status);
            $("#Par_RsonFrRet_" + Tr_Id).text(null);
            $("#hdn_RsonFrReturn").val(null);
        }
    }
    else {
        $("#Par_Cldt_" + Tr_Id).text(null);
        $("#hdn_ClDt_" + Tr_Id).val(null);
        $("#Par_Status_" + Tr_Id).text(Status);
        $("#hdn_status_" + Tr_Id).val(Status1);
        $("#Par_RsonFrRet_" + Tr_Id).text(RsForReturn);
        $("#hdn_status").val(Status);
        $("#hdn_RsonFrReturn").val(RsForReturn);
    }
    $("#PartSt_SaveAndExit").attr("data-dismiss", "modal");
}
//function onChangeValidateDate() {
//    debugger;
//    var Flag = 'N';
//    var Cldate = $("#ParSt_ClDate").val()
//    var Voudate = $("#HdnParstVouDt").val()
//    if (Cldate <= Voudate) {
//        if ($("#ParSt_ddlstatus").val() == 'C' && Cldate == "") {
//            Flag = 'Y';
//            $("#ParSt_ClDate").css("border-color", "red");
//            $("#Spn_parstupd").text($("#valueReq").text());
//            $("#Spn_parstupd").css("display", "block");

//        }
//        if (Cldate != "") {
//            $("#ParSt_ClDate").css("border-color", "#ced4da");
//            $("#Spn_parstupd").css("display", "none");
//        }
//        if ($("#ParSt_ddlstatus").val() == 'R' && $("#ParSt_RsnfrRet").val() == "") {
//            Flag = 'Y';
//            $("#ParSt_RsnfrRet").css("border-color", "red");
//            $("#Spn_parRsnFrRet").text($("#valueReq").text());
//            $("#Spn_parRsnFrRet").css("display", "block");
//        }
//    }
    
//    if (Flag == 'Y') {
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function onChangeValidateDate() {
    debugger;
    var Flag = 'N';
    /*----------whole function Modify by Hina on 11-07-2025------------*/
    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;

    var VouDate = $("#HdnParstVouDt").val()
    var VouDate1 = VouDate.split("-");
    var VouDate2 = VouDate1[2] + '-' + VouDate1[1] + '-' + VouDate1[0];
    
    var Status = $("#ParSt_ddlstatus").val();
    if (Status == "C" || Status == "Cleared") {
        //$("#ParSt_ClDate").attr("min", VouDate2)
        //$("#ParSt_ClDate").attr("max", today)

        var Cldate = $("#ParSt_ClDate").val();
        if (Cldate >= VouDate2 && Cldate <= today) {
            $("#ParSt_ClDate").css("border-color", "#ced4da");
            $("#Spn_parstupd").css("display", "none");
        }
        else
        {
            $("#ParSt_ClDate").css("border-color", "red");
            $("#Spn_parstupd").text($("#JC_InvalidDate").text());
            $("#Spn_parstupd").css("display", "block");
            Flag = 'Y';
            
        }
    }
    else {
        if ($("#ParSt_ddlstatus").val() == 'R' && $("#ParSt_RsnfrRet").val() == "") {
            Flag = 'Y';
            $("#ParSt_RsnfrRet").css("border-color", "red");
            $("#Spn_parRsnFrRet").text($("#valueReq").text());
            $("#Spn_parRsnFrRet").css("display", "block");
        }
        $("#ParSt_ClDate").attr("min", '')
        $("#ParSt_ClDate").attr("max", '')
        $("#ParSt_ClDate").val(null);
    }
    

    if (Flag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}
function onkeypressreasonFrRet() {
    debugger;
    $("#ParSt_RsnfrRet").css("border-color", "#ced4da");
    $("#Spn_parRsnFrRet").css("display", "none");
}
function FromToDateValidation() { /*Added by NItesh For Validation From date and Todate*/
    debugger;
    var FromDate = $("#BkReFromdate").val();
    var ToDate = $("#BkReTodate").val();
    //$("#hdFromdate").val(FromDate)
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
            $("#BkReTodate").val(today);
            debugger;
            var fromDate = new Date($("#HdnfromDate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#BkReFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
//function disableKeyPress() {
//    debugger;
//    $("#ParSt_ClDate").keypress(function (e) {
//        e.preventDefault();
//    })
//    return false;
//}

//function FromToDateValidation(Flag) {
//    debugger;
//    var DisFlag = 'N';
//    var HdnfromDate = $("#HdnfromDate").val();
//    var HdnToDate = $("#HdnToDate").val();
//    var FromDate = $("#BkReFromdate").val();
//    var ToMaxdate = $("#ToMaxdate").val();
//    var FrMindate = $("#fromfymindate").val();
//    var Todate = $("#BkReTodate").val();
//    if ((FromDate != null || FromDate == "") && (FrMindate != null || FrMindate == "")) {
//        if (Flag == 'Y') {
//            if (FromDate <= ToMaxdate && FromDate >= FrMindate) {
//                debugger;
//                $("#BkReFromdate").val(FromDate);
//                $("#HdnfromDate").val(FromDate);
//            }
//            else {
//                $("#BkReFromdate").val(HdnfromDate);
//                DisFlag = 'Y';
//            }
//        }
//        if (Flag == 'N') {
//            if (Todate >= FromDate && Todate <= ToMaxdate) {
//                $("#BkReTodate").val(Todate);
//            }
//            else {
//                $("#BkReTodate").val(HdnToDate);
//            }
//        }
//    }
//    if (DisFlag == 'Y') {
//        return false;
//    }
//    else {
//        return true;
//    }
//}

