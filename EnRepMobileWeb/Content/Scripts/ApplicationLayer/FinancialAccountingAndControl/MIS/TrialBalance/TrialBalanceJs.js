$(document).ready(function () {
    $('#branch').multiselect();
    debugger;
    $("#acc_name").select2({
        ajax: {
            url: $("#GLListName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    $("#acc_group").select2({
        ajax: {
            url: $("#GLAccGrp").val(),
            data: function (params) {
                var queryParameters = {
                    ddlGroup: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },
        },
    });
    $("#div_GeneratePDF_TB").css("display", "");

    OnClickBtnSearch();
}); 
var QtyDecDigit = $("#QtyDigit").text();///Quantity
var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
var ValDecDigit = $("#ValDigit").text();///Amount
function OnClickBtnSearch() {
    debugger;

    var curr_filt = "BS";
    if ($("#rdo_Specific").is(":checked")) {
        curr_filt = $("#rdo_Specific").val();
    }
    /*------Code Start add by Hina Sharma on 19-08-2025------------ */
    var Curr = (curr_filt == "BS") ? "Base" : "Specific";
    $("#hdn_currency").val(Curr)
    /*--------Code End add by Hina Sharma on 19-08-2025----------- */
   
    if ($("#IncludeZeroStock").is(":checked")) {
        FilterBalanceDetailList("Y", curr_filt);
    }
    else {
        FilterBalanceDetailList("N", curr_filt);
    }
}
function FilterBalanceDetailList(IncludeZeroBal,curr_filter) {
    debugger;
    try {
        //var BalanceBy = $("#bal_type").val();
        var BalanceBy = "BW";
        var AccId = $("#acc_name option:selected").val();
        var AccGroupId = $("#acc_group option:selected").val();
        var Acctype = $("#acc_type").val();
        var Rpttype = $("#rpt_type").val();
        var Br_ID="";
        var BrachID = $("#branch").val();
        if (BrachID != null && BrachID != "") {
            if (BrachID.length > 0) {
                for (var i = 0; i < BrachID.length; i++) {
                    if (Br_ID == "") {
                        Br_ID = BrachID[i];
                    }
                    else {
                        Br_ID = Br_ID + "," + BrachID[i];
                    }
                }
            }
        }
        else {
            Br_ID = "";
        }
        var Uptodate = $("#txtuptodate").val();
     
            $.ajax({
                async: true,
                type: "POST",
                url: "/ApplicationLayer/TrialBalance/SearchTrialBalDetail",
                data: {
                    IncludeZeroBlaFlag: IncludeZeroBal,
                    BalanceBy: BalanceBy,
                    AccId: AccId,
                    AccGroupId: AccGroupId,
                    Acctype: Acctype,
                    RptType: Rpttype,
                    BrachID: Br_ID,
                    Uptodt: Uptodate,
                    filter_curr: curr_filter,
                },
                success: function (data) {
                    debugger;
                    
                    $('#divtrialbaldetailstable').html(data);

                    if (Rpttype == "SU") {
                        $("#div_trialbalsummary").css("display", "block");
                        $("#div_trialbaldetails").css("display", "none");
                    }
                    else {
                        $("#div_trialbalsummary").css("display", "none");
                        $("#div_trialbaldetails").css("display", "block");
                        $("#hdnPDFPrint").val("Print");
                        $("a.btn.btn-default.buttons-print.btn-sm").remove();
                        $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                        $("#pdfData").remove();
                        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" id="pdfData" onclick="trialbalancePDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
                        //$("#CsvData").remove();
                        //$("a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
                        //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="trialbalanceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                    }
                    hideLoader();/*Add by Hina sharma on 19-11-2024 */
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                    hideLoader();
                }
            });
       
    } catch (err) {
        debugger;
        hideLoader();
        console.log("Trial Balance Error : " + err.message);
    }
}

//function OnChangeBalanceByddl() {
//    debugger;
//    var BalanceBy = $("#bal_type").val();
//    var curr_filt = "BS";
//    if ($("#rdo_Specific").is(":checked")) {
//        curr_filt = $("#rdo_Specific").val();
//    }
//    if (BalanceBy == "HW") {
//        $("#branch option[value=0]").show();
//        $("#div_brch").css("display", "none");
//    }
//    else {
//        $("#branch option[value=0]").hide();
//        $("#div_brch").css("display", "block");
//    }
//    FilterBalanceDetailList("N", curr_filt);
//    $("#hdnPDFPrint").val("Print");
//    $("#pdfData").remove();
//    $("a.btn.btn-default.buttons-print.btn-sm").remove();
//    $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
//    $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" id="pdfData" onclick="trialbalancePDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')

//    //$("#CsvData").remove();
//    //$("a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
//    //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="trialbalanceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
//}

function OnClickDebitCreditAndClosingIcon(evt,flag) {
    debugger;
    var CurrentRow = $(evt.target).closest("tr");
    //var BalType = $("#bal_type").val();
    var BalType = "BW";
    var AccName = CurrentRow.find("#sAccName").text();
    var AccId = CurrentRow.find("#hdnaccid").text();
    var BrId = CurrentRow.find("#hdnbrid").val();
    var Uptodate = $("#txtuptodate").val();
    var debitamt_bs = CurrentRow.find("#totdebitbs").text();
    var debitamt_sp = CurrentRow.find("#totdebitsp").text();
    var creditamt_bs = CurrentRow.find("#totcreditbs").text();
    var creditamt_sp = CurrentRow.find("#totcreditsp").text();
    var closingamt_bs = CurrentRow.find("#closingbalbs").text();
    var closingamt_sp = CurrentRow.find("#closingbalsp").text();
    var closingamt_type = CurrentRow.find("#closingbaltype").text();

    var curr_filt = "BS";
    if ($("#rdo_Specific").is(":checked")) {
        curr_filt = $("#rdo_Specific").val();
    }

    var BsAmt, SpAmp, Type;

    if (flag == "TD") {
        BsAmt = debitamt_bs;
        SpAmp = debitamt_sp;
        Type = "";
    }
    if (flag == "TC") {
        BsAmt = creditamt_bs;
        SpAmp = creditamt_sp;
        Type = "";
    }
    if (flag == "CB") {
        BsAmt = closingamt_bs;
        SpAmp = closingamt_sp;
        Type = closingamt_type;
    }

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TrialBalance/GetTrialBalHistoryDetail",
        data: {
            AccId: AccId,
            BrachID: BrId,
            Uptodt: Uptodate,
            Flag: flag,
            Accname: AccName,
            bs_amt: BsAmt,
            sp_amt: SpAmp,
            type: Type,
            baltype: BalType,
            currtype: curr_filt
        },
        success: function (data) {
            debugger;
            $('#div_debitcreditAndClosingdetails').html(data);
            cmn_apply_datatable("#TblTrialBalDebitDetails");
            //$("#div_dr_cr_popup .btn.btn-default.buttons-csv.buttons-html5").remove();
            //$("#div_dr_cr_popup .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="trialbalanceIbuttonCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
        },
    });
}
function trialbalanceIbuttonCSV() {
    debugger;
    var arr = [];
    $("#Hdn_TblTrialBalDebitDetails >tbody >tr").each(function () {
        var currentRow = $(this);
        var list = {};
        list.vou_no = currentRow.find("#hdn_vouno").val();
        list.vou_dt = currentRow.find("#spn_voudt").text();
        list.cc_vou_amt_bs = currentRow.find("#Amt_bs").text();
        list.cc_vou_amt_sp = currentRow.find("#Amt_sp").text();
        list.amt_type = currentRow.find("#vou_amt_type").text();
        list.curr_logo = currentRow.find("#curr_logo").text();
        list.conv_rate = currentRow.find("#conv_rate").text();
        list.narr = currentRow.find("#narr").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#InsightButtonData").val(array);

    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#InsightButton").val("InsightButtonCsvPrint");
    //var searchValue = $("#TblTransactionDetails_filter input[type=search]").val();
    var searchValue = $("#TblTrialBalDebitDetails_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function OnChangeReportTypeddl() {
    debugger;
    var BalanceBy = $("#rpt_type").val();

    if (BalanceBy == "SU") {
        $("#div_trialbalsummary").css("display", "block");
        $("#div_trialbaldetails").css("display", "none");
        $("#rdo_currfilter").attr('style', 'display:none');
        $("#div_GeneratePDF_TB").css("display", "");/*add by Hina on 20-06-2025*/
    }
    else {
        $("#div_GeneratePDF_TB").css("display", "none");/*add by Hina on 20-06-2025*/
        $("#rdo_currfilter").removeAttr("style");
        $("#div_trialbalsummary").css("display", "none");
        $("#div_trialbaldetails").css("display", "block");
        $("#hdnPDFPrint").val("Print");
        $("#pdfData").remove();
        $("a.btn.btn-default.buttons-print.btn-sm").remove();
        $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" id="pdfData" onclick="trialbalancePDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')

        //$("#CsvData").remove();
        //$("a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
        //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="trialbalanceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    }
}
function trialbalanceCSV() {
    debugger;
    var IncludeZeroStock = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        IncludeZeroStock = "Y";
    }
    var curr_filt = "BS";
    if ($("#rdo_Specific").is(":checked")) {
        curr_filt = $("#rdo_Specific").val();
    }
    //var BalanceBy = $("#bal_type").val();
    var BalanceBy = "BW";
    var AccId = $("#acc_name option:selected").val();
    var AccGroupId = $("#acc_group option:selected").val();
    var Acctype = $("#acc_type").val();
    var Rpttype = $("#rpt_type").val();
    var Br_ID = "";
    var BrachID = $("#branch").val();
    if (BrachID != null && BrachID != "") {
        if (BrachID.length > 0) {
            for (var i = 0; i < BrachID.length; i++) {
                if (Br_ID == "") {
                    Br_ID = BrachID[i];
                }
                else {
                    Br_ID = Br_ID + "," + BrachID[i];
                }
            }
        }
    }
    else {
        Br_ID = "";
    }
    var Uptodate = $("#txtuptodate").val();
    //var filters = IncludeZeroStock + "," + BalanceBy + "," + AccId + "," + AccGroupId + "," + Acctype + "," + Rpttype + "," + Br_ID + "," + Uptodate + "," + curr_filt;
    var filters = IncludeZeroStock + "," + BalanceBy + "," + AccId + "," + AccGroupId + "," + Acctype + "," + Rpttype + "," + Uptodate + "," + curr_filt;
    
    $("#Allfilters").val(filters);
    $("#BRIDListPrintData").val(Br_ID);
    $("#hdnPDFPrint").val(null);
    $("#InsightButton").val(null);
    $("#hdnCSVPrint").val("CsvPrint");
    //var searchValue = $("#TblTransactionDetails_filter input[type=search]").val();commented by shubham Maurya on 28-07-2025
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
    //var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    //window.location.href = "/ApplicationLayer/TrialBalance/ExportTrialBalanceData?searchValue=" + searchValue + "&filters=" + filters;

}
function trialbalancePDF() {
    debugger;
    var IncludeZeroStock = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        IncludeZeroStock="Y";
    }
    var curr_filt = "BS";
    if ($("#rdo_Specific").is(":checked")) {
        curr_filt = $("#rdo_Specific").val();
    }
    //var BalanceBy = $("#bal_type").val();
    var BalanceBy = "BW";
    var AccId = $("#acc_name option:selected").val();
    var AccGroupId = $("#acc_group option:selected").val();
    var Acctype = $("#acc_type").val();
    var Rpttype = $("#rpt_type").val();
    var Br_ID = "";
    var BrachID = $("#branch").val();
    if (BrachID != null && BrachID != "") {
        if (BrachID.length > 0) {
            for (var i = 0; i < BrachID.length; i++) {
                if (Br_ID == "") {
                    Br_ID = BrachID[i];
                }
                else {
                    Br_ID = Br_ID + "," + BrachID[i];
                }
            }
        }
    }
    else {
        Br_ID = "";
    }
    /*------Code Start add by Hina Sharma on 19-08-2025------------ */
    var AccGroupName = $("#acc_group option:selected").text();
    var AccTypeName = $("#acc_type option:selected").text();
    var Curr = "";
    var Curr = (curr_filt == "BS") ? "Base" : "Specific";
    $("#hdn_AcGrpName").val(AccGroupName)
    $("#hdn_AcTypName").val(AccTypeName)
    $("#hdn_currency").val(Curr)
    /*--------Code End add by Hina Sharma on 19-08-2025----------- */
    
    var Uptodate = $("#txtuptodate").val();

    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/TrialBalance/PrintTrialBalDetail",
        data: {
            IncludeZeroBlaFlag: IncludeZeroStock,
            BalanceBy: BalanceBy,
            AccId: AccId,
            AccGroupId: AccGroupId,
            Acctype: Acctype,
            RptType: Rpttype,
            BrachID: Br_ID,
            Uptodt: Uptodate,
            filter_curr: curr_filt,
        },
        success: function (data) {
            if (data !== null && data !== "") {
                var arr = [];
                var arr1 = [];
                var arrtotal=[]
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    for (var i = 0; i < arr.Table.length; i++) {
                        var list = {};
                        list.AccName = arr.Table[i].acc_name;
                        list.Acc_Id = arr.Table[i].acc_id;
                        list.Acc_typ = arr.Table[i].acc_typename;
                        list.Acc_grp = arr.Table[i].acc_group;
                        list.Branch = arr.Table[i].branch;
                        list.curr_name = arr.Table[i].curr_name;
                        //list.op_bal_bs = arr.Table[i].br_openingbs;/*commenetd and modify by Hina Sharma on 19-08-2025*/
                        //list.op_bal_type = arr.Table[i].openingbal_type;
                        //list.totdebitbs = arr.Table[i].br_totaldebitbs;
                        //list.totcreditbs = arr.Table[i].br_totalcreditbs;
                        //list.closingbalbs = arr.Table[i].br_closingbs;
                        //list.closingbaltype = arr.Table[i].br_closingbal_type;
                        if (BalanceBy == "BW") {
                            list.op_bal_bs_dr = arr.Table[i].br_dr_op_bs;
                            list.op_bal_bs_cr = arr.Table[i].br_cr_op_bs;
                            list.totdebitbs = arr.Table[i].br_totaldebitbs;
                            list.totcreditbs = arr.Table[i].br_totalcreditbs;
                            list.closingbalbs_dr = arr.Table[i].br_dr_cl_bs;
                            list.closingbalbs_cr = arr.Table[i].br_cr_cl_bs;
                           
                        }
                        else {
                            list.op_bal_bs_dr = arr.Table[i].ho_dr_op_bs;
                            list.op_bal_bs_cr = arr.Table[i].ho_cr_op_bs;
                            list.totdebitbs = arr.Table[i].ho_totaldebitbs;
                            list.totcreditbs = arr.Table[i].ho_totalcreditbs;
                            list.closingbalbs_dr = arr.Table[i].ho_dr_cl_bs;
                            list.closingbalbs_cr = arr.Table[i].ho_cr_cl_bs;
                        }
                        arr1.push(list);
                    }                    
                }
                if (arr.Table7.length > 0) {
                    for (var i = 0; i < arr.Table7.length; i++) {
                        var listtotal = {};
                        
                        listtotal.TotalOpnBalDebit = arr.Table7[i].TotalOpnBalDebit;
                        listtotal.TotalOpnBalCredit = arr.Table7[i].TotalOpnBalCredit;
                        listtotal.TotalDebit = arr.Table7[i].TotalDebit;
                        listtotal.TotalCredit = arr.Table7[i].TotalCredit;
                        listtotal.TotalClsBalDebit = arr.Table7[i].TotalClsBalDebit;
                        listtotal.TotalClsBalCredit = arr.Table7[i].TotalClsBalCredit;
                       
                        arrtotal.push(listtotal);
                    }
                }
                debugger;
                /*start Add by Hina on 22-05-2025 for multiple branch name */
                var arrBrList = [];
                var arrayBrList = [];
                arrBrList.push({
                    Br_ID: Br_ID
                });
                var arrayBrList = JSON.stringify(arrBrList);
                $("#BRIDListPrintData").val(arrayBrList);
                /*End Add by Hina on 22-05-2025 for multiple branch name */
                var array = JSON.stringify(arr1);
                $("#AccountPayblePrintData").val(array);

                var arraytotal = JSON.stringify(arrtotal);
                $("#TrialBalTotalPrintData").val(arraytotal);

                $("#InsightButton").val(null);
                $("#hdnCSVPrint").val(null);
                $("#hdnPDFPrint").val("Print");
    //window.location.href = "/ApplicationLayer/TrialBalance/GenratePdfFile?IncludeZeroBlaFlag=" + IncludeZeroStock + "&BalanceBy=" + BalanceBy + "&AccId=" + AccId+ "&AccGroupId=" + AccGroupId+ "&Acctype=" + Acctype + "&RptType=" + Rpttype + "&BrachID=" + Br_ID + "&Uptodt=" + Uptodate;
                $('form').submit();
            }
        },
        error: function OnError(xhr, errorType, exception) {
            debugger;
            hideLoader();
        }
    });

    //var arr = [];
    //$("#tbtrailbal tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};
    //    list.AccName = CurrentRow.find("#sAccName").text();
    //    list.Acc_Id = CurrentRow.find("#hdnaccid").text();
    //    list.Acc_typ = CurrentRow.find("#Acc_typ").text();
    //    list.Acc_grp = CurrentRow.find("#Acc_grp").text();
    //    list.Branch = CurrentRow.find("#Branch").text();
    //    list.op_bal_bs = CurrentRow.find("#op_bal_bs").text();
    //    list.op_bal_type = CurrentRow.find("#op_bal_type").text();
    //    list.totdebitbs = CurrentRow.find("#totdebitbs").text();
    //    list.totcreditbs = CurrentRow.find("#totcreditbs").text();
    //    list.closingbalbs = CurrentRow.find("#closingbalbs").text();
    //    list.closingbaltype = CurrentRow.find("#closingbaltype").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#AccountPayblePrintData").val(array);

   
}
function OnClickAcc_Name(evt) {
    debugger;
    var CurrentRow = $(evt.target).closest("tr");
    var Accid = CurrentRow.find("#accid").val();
    
}

//--------On Click Icon Button Voucher Details ------//

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#hdn_vouno").val();
    var voudt = clickedrow.find("#spn_voudt").text();
    var vou_dt = clickedrow.find("#hdn_voudt").val();
    //var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }

    Cmn_GetVouDetails(vou_no, voudt, vou_dt, cflag, '');
}

function OnclickCCIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var vou_no = $("#GLVoucherNo").val();
    var vou_dt = $("#mwhdn_voudt").val();
    var acc_id = clickedrow.find("#hdn_accid").val();
    var acc_name = clickedrow.find("#spn_glname").text();
    var dr_amt = clickedrow.find("#spn_dramt").text();
    var cr_amt = clickedrow.find("#spn_cramt").text();
    var amt = 0;
    if (dr_amt > 0) {
        amt = dr_amt;
    }
    if (cr_amt > 0) {
        amt = cr_amt;
    }
    Cmn_GetcostcenterDetails(vou_no, vou_dt, acc_id, acc_name, amt);
}

//--------Voucher Details End------//
function GenerateBalanceSheetPDF_TB() {
    debugger;
    var IncludeZeroStock = "N";
    if ($("#IncludeZeroStock").is(":checked")) {
        IncludeZeroStock = "Y";
    }
    var curr_filt = "BS";
    if ($("#rdo_Specific").is(":checked")) {
        curr_filt = $("#rdo_Specific").val();
    }
    //var BalanceBy = $("#bal_type").val();
    var BalanceBy = "BW";
    var AccId = $("#acc_name option:selected").val();
    var AccGroupId = $("#acc_group option:selected").val();
    var Acctype = $("#acc_type").val();
    var Rpttype = $("#rpt_type").val();
    var Br_ID = "";
    var BrachID = $("#branch").val();
    if (BrachID != null && BrachID != "") {
        if (BrachID.length > 0) {
            for (var i = 0; i < BrachID.length; i++) {
                if (Br_ID == "") {
                    Br_ID = BrachID[i];
                }
                else {
                    Br_ID = Br_ID + "," + BrachID[i];
                }
            }
        }
    }
    else {
        Br_ID = "";
    }

    var Uptodate = $("#txtuptodate").val();
    window.location.href = "/ApplicationLayer/TrialBalance/GenratePdfFilebySummary?IncludeZeroStock=" + IncludeZeroStock + "&curr_filt=" + curr_filt + "&BalanceBy=" + BalanceBy + "&AccId=" + AccId + "&AccGroupId=" + AccGroupId + "&Acctype=" + Acctype + "&Rpttype=" + Rpttype + "&Uptodate=" + Uptodate + "&Br_ID=" + Br_ID
    
}
function get_glvoucherdata(evt) {
    debugger;
    showLoader();
    var clickedrow = $(evt.target).closest("tr");
    var docid = $("#DocumentMenuId").val();
    var acc_id = clickedrow.find("#hdn_acc_id").val();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtuptodate").val();
    var rpt_type = $("#rpt_type").val();
    var curr ="0"
    if (rpt_type != "SU") {
        curr = clickedrow.find("#hdncurr_id").val();
        if ($('#rdo_Base').is(':checked')) {
            curr = "0";
        }
    }

    if (acc_id != "" && acc_id != null) {
        try {
            $.ajax({
                type: "POST",
                url: "/Common/Common/Cmn_GetGeneralLedger_Detail",
                data: {
                    from_dt: from_dt, to_dt: to_dt, acc_id: acc_id, doc_id: docid, curr_id: curr
                },
                success: function (data) {
                    debugger;
                    $('#div_gldetails_popup').html(data);

                    hideLoader();

                },
                error: function OnError(xhr, errorType, exception) {
                    hideLoader();
                }
            });
        } catch (err) {
            debugger;
            hideLoader();
            console.log("Profit and loss Statement Voucher Detail Error : " + err.message);
        }
    }
}

function OnclickVouDetailsIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#hdn_vouno").val();
    var voudt = clickedrow.find("#spn_voudt").text();
    var vou_dt = clickedrow.find("#hdn_voudt").val();
    var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }
    Cmn_GetVouDetails(vou_no, voudt, vou_dt, cflag, narr);
}