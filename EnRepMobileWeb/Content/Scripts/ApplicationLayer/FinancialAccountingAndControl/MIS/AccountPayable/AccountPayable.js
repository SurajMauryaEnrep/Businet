
$(document).ready(function () {
    debugger
    //BindSupplierList();
   // $("#SuppCategory").select2();
    //$("#SuppPortfolio").select2();
    BindCustomerList()
    EnbaleDatatable();
    $(document).ready(function () {
        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountPayablePDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
        //$("#datatable-buttons5_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountPayableCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    });
    Cmn_initializeMultiselect(['#SuppCategory', '#SuppPortfolio',]);
    Cmn_initializeMultiselect_ddl_br(["#ddl_branch"]);
});
function onclickPayableType() {
    if ($("#PayableTypeA").is(":checked")) {
        $("#Basis").attr("disabled", false)
        SearchSuppAgingDetail();
    }
    if ($("#PayableTypeO").is(":checked")) {
        $("#Basis").attr("disabled", true)
        $("#Basis").val("S").trigger('change');
        SearchSuppAgingDetail();
    }
    if ($("#PayableTypeU").is(":checked")) {
        $("#Basis").attr("disabled", true)
        $("#Basis").val("S").trigger('change');
        SearchSuppAgingDetail();
    }
}
//function BindCustomerList() {
//    debugger;
//    var Branch = sessionStorage.getItem("BranchID");
//    $.ajax({
//        url: "/ApplicationLayer/AccountPayable/GetAutoCompleteSearchSuppList",
//        data: {},
//        success: function (data, params) {
//            debugger;
//            if (data == 'ErrorPage') {
//                LSO_ErrorPage();
//                return false;
//            }
//            /* var s = "<option value='0'>---All---</option>";*/
//            var s = "";
//            $.map(data, function (val, item) {
//                if (val.Name.trim() != "---Select---") {
//                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
//                }
//            })
//            $("#ddl_SupplierName").html(s);
//            Cmn_initializeMultiselect(['#ddl_SupplierName']);
//            //$('#ddl_SupplierName').multiselect('rebuild');

//            var Supp_id = $("#Hdnsupp_id").val();
//            if (Supp_id !== "" && Supp_id !== null && Supp_id !== "0") {
//                $("#ddl_SupplierName").val(Supp_id);
//            }
//            //$("#ddl_SupplierName").select2({});
//        }
//    });
//}
function BindCustomerList() {
    debugger;
    var selectedValues = $("#ddl_SupplierName").val(); // preserve previous selection
    $.ajax({
        url: "/ApplicationLayer/AccountPayable/GetAutoCompleteSearchSuppList",
        data: {},
        success: function (data) {
            debugger;

            if (data === 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = "";
            $.map(data, function (val) {
                if (val.Name.trim() !== "---Select---") {
                    s += '<option value="' + val.ID + '">' + val.Name + '</option>';
                }
            });
            $("#ddl_SupplierName").html(s);
            Cmn_initializeMultiselect(['#ddl_SupplierName']);
            if (selectedValues && selectedValues.length > 0) {
                $("#ddl_SupplierName").val(selectedValues);
                $("#ddl_SupplierName").multiselect('refresh');
            }
            // Hidden value support
            var Supp_id = $("#Hdnsupp_id").val();
            if (Supp_id && Supp_id !== "0") {
                $("#ddl_SupplierName").val(Supp_id.split(','));
                $("#ddl_SupplierName").multiselect('refresh');
            }
        }
    });
}
function numValueOnly(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 58)) {
        return false;
    }
    return true;
}
function InsertRangeDetail() {
    debugger;
    if (RangeValidation() == false) {
        return false;
    }
    if (RangeValidation1() == false) {
        return false;
    }

    return true;
};
function EnbaleDatatable() {
    debugger;
    if (PageLoadRangeValidation() == false) {
        debugger;
        $("#BtnSearch").prop("disabled", true);
        $("#DivSuppAgingtable").css("display", "none");

    }
    else {
        debugger;
        $("#BtnSearch").prop("disabled", false);
        $("#DivSuppAgingtable").css("display", "block");
    }

}
function RangeValidation() {
    debugger;
    var ErrorFlag = "N";
    var range1 = $("#range1").val();
    if (range1 == "" || range1 == "0" || range1 == null) {
        $("#vmrange1").css("display", "block");
        $("#range1").css("border-color", "red");
        $("#vmrange1").text($("#valueReq").text());

        ErrorFlag = "Y";
    }
    else {
        $("#vmrange1").css("display", "none");
        $("#range1").css("border-color", "#ced4da");
    }

    var range2 = $("#range2").val();
    if (range2 == "" || range2 == "0" || range2 == null) {
        $("#vmrange2").css("display", "block");
        $("#range2").css("border-color", "red");
        $("#vmrange2").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange2").css("display", "none");
        $("#range2").css("border-color", "#ced4da");
    }

    var range3 = $("#range3").val();
    if (range3 == "" || range3 == "0" || range3 == null) {
        $("#vmrange3").css("display", "block");
        $("#range3").css("border-color", "red");
        $("#vmrange3").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange3").css("display", "none");
        $("#range3").css("border-color", "#ced4da");
    }

    var range4 = $("#range4").val();
    if (range4 == "" || range4 == "0" || range4 == null) {
        $("#vmrange4").css("display", "block");
        $("#range4").css("border-color", "red");
        $("#vmrange4").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange4").css("display", "none");
        $("#range4").css("border-color", "#ced4da");
    }

    var range5 = $("#range5").val();
    if (range5 == "" || range5 == "0" || range5 == null) {
        $("#vmrange5").css("display", "block");
        $("#range5").css("border-color", "red");
        $("#vmrange5").text($("#valueReq").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange5").css("display", "none");
        $("#range5").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function RangeValidation1() {
    debugger;
    var ErrorFlag = "N";
    var range1 = $("#range1").val();
    var range2 = $("#range2").val();
    var range3 = $("#range3").val();
    var range4 = $("#range4").val();
    var range5 = $("#range5").val();

    if (parseFloat(range1) >= parseFloat(range2)) {
        $("#vmrange1").css("display", "block");
        $("#range1").css("border-color", "red");
        $("#vmrange1").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange1").css("display", "none");
        $("#range1").css("border-color", "#ced4da");
    }
    if (parseFloat(range2) >= parseFloat(range3)) {
        $("#vmrange2").css("display", "block");
        $("#range2").css("border-color", "red");
        $("#vmrange2").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange2").css("display", "none");
        $("#range2").css("border-color", "#ced4da");
    }
    if (parseFloat(range3) >= parseFloat(range4)) {
        $("#vmrange3").css("display", "block");
        $("#range3").css("border-color", "red");
        $("#vmrange3").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange3").css("display", "none");
        $("#range3").css("border-color", "#ced4da");
    }
    if (parseFloat(range4) >= parseFloat(range5)) {
        $("#vmrange4").css("display", "block");
        $("#range4").css("border-color", "red");
        $("#vmrange4").text($("#InvalidRange").text());
        ErrorFlag = "Y";
    }
    else {
        $("#vmrange4").css("display", "none");
        $("#range4").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function PageLoadRangeValidation() {
    debugger;
    var ErrorFlag = "N";
    var range1 = $("#range1").val();
    if (range1 == "" || range1 == "0" || range1 == null) {
        ErrorFlag = "Y";
    }
    var range2 = $("#range2").val();
    if (range2 == "" || range2 == "0" || range2 == null) {
        ErrorFlag = "Y";
    }
    var range3 = $("#range3").val();
    if (range3 == "" || range3 == "0" || range3 == null) {
        ErrorFlag = "Y";
    }
    var range4 = $("#range4").val();
    if (range4 == "" || range4 == "0" || range4 == null) {
        ErrorFlag = "Y";
    }
    var range5 = $("#range5").val();
    if (range5 == "" || range5 == "0" || range5 == null) {
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SearchSuppAgingDetail() {
    debugger;
    try {
       // var Supp_id = $("#ddl_SupplierName option:selected").val(); 
        var Supp_id = cmn_getddldataasstring("#ddl_SupplierName");
        $("#Hdnsupp_id").val(Supp_id);
       // var Cat_id = $("#SuppCategory").val();
        Cat_id = cmn_getddldataasstring("#SuppCategory");
        $("#Hdncatg_id").val(Cat_id);
        //var Prf_id = $("#SuppPortfolio").val();
        Prf_id = cmn_getddldataasstring("#SuppPortfolio");
        $("#Hdnport_id").val(Prf_id);

        var Basis = $("#Basis").val();
        var AsDate = $("#AsDate").val();
        var PayableType = "A";
        if ($("#PayableTypeA").is(":checked")) {
            PayableType = "A";
        }
        if ($("#PayableTypeO").is(":checked")) {
            PayableType = "O";
        }
        if ($("#PayableTypeU").is(":checked")) {
            PayableType = "U";
        }
        var ReportType = "S";
        if ($("#ProcurementMISOrderSummary").is(":checked")) {
            ReportType = "S";
            $("#HdnReportType").val("S");
        }
        if ($("#ProcurementMISOrderDetail").is(":checked")) {
            ReportType = "D";
            $("#HdnReportType").val("D");
        }
        var brid_list = cmn_getddldataasstring("#ddl_branch");
        if (brid_list == '0' || brid_list == "" || brid_list == null) {
            return false;
        }
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountPayable/SearchAccountPayableDetail",
            data: {
                Supp_id: Supp_id,
                Cat_id: Cat_id,
                Prf_id: Prf_id,
                Basis: Basis,
                AsDate: AsDate,
                PayableType: PayableType,
                ReportType: ReportType,
                brlist: brid_list
            },
            success: function (data) {
                debugger;

                $('#DivSuppAgingtable').html(data);
               /* hideLoader();*//*commented by Hina sharma on 19-11-2024 after discuss vishal sir*/
                //$("#hdnCmdPDFPrint").val("Print");
                $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountPayablePDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')

                //if (ReportType == "D") {
                //    $("#datatable-buttons6_wrapper a.btn btn-default.buttons-csv.buttons-html5.btn-sm").remove();
                //    $("#datatable-buttons6_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountPayableCSV()"  tabindex="0" aria-controls="datatable-buttons6" href="#"><span>CSV</span></button>')
                //}
                //else {
                //    $("#datatable-buttons5_wrapper a.btn btn-default.buttons-csv.buttons-html5.btn-sm").remove();
                //    $("#datatable-buttons5_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountPayableCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //}
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
            },
            error: function OnError(xhr, errorType, exception) {
                hideLoader();
                debugger;
            }
        });
    } catch (err) {
        debugger;
        hideLoader();
        console.log("Trial Balance Error : " + err.message);
    }
}
function AccountPayablePDF() {
    debugger;
    var arr = [];
    var rangeList = [];
    //var totalmultiWrapperRange1 = 0;
    //var totalmultiWrapperRange2 = 0;
    //var totalmultiWrapperRange3 = 0;
    //var totalmultiWrapperRange4 = 0;
    //var totalmultiWrapperRange5 = 0;
    //var totalmultiWrapperRange6 = 0;
    //var TotalAmt = 0;
    //var TotalAdvanceAmount = 0;
    //$("#datatable-buttons5 tbody tr").each(function () {
    //$("#APHdnTble tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};
    //    list.Supp_name = CurrentRow.find("#Supp_name").text();
    //    list.Supp_id = CurrentRow.find("#Supp_id").text();
    //    list.Curr = CurrentRow.find("#Curr").text();
    //    list.multiWrapperRange1 = CurrentRow.find("#multiWrapperRange1").text();
    //    list.multiWrapperRange2 = CurrentRow.find("#multiWrapperRange2").text();
    //    list.multiWrapperRange3 = CurrentRow.find("#multiWrapperRange3").text();
    //    list.multiWrapperRange4 = CurrentRow.find("#multiWrapperRange4").text();
    //    list.multiWrapperRange5 = CurrentRow.find("#multiWrapperRange5").text();
    //    list.multiWrapperRange6 = CurrentRow.find("#multiWrapperRange6").text();
    //    list.TotalAmt = CurrentRow.find("#TotalAmt").text();
    //    list.AdvanceAmount = CurrentRow.find("#APAdvanceAmount").text();

    //    totalmultiWrapperRange1 = parseFloat(totalmultiWrapperRange1) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange1").text()));
    //    totalmultiWrapperRange2 = parseFloat(totalmultiWrapperRange2) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange2").text()));
    //    totalmultiWrapperRange3 = parseFloat(totalmultiWrapperRange3) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange3").text()));
    //    totalmultiWrapperRange4 = parseFloat(totalmultiWrapperRange4) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange4").text()));
    //    totalmultiWrapperRange5 = parseFloat(totalmultiWrapperRange5) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange5").text()));
    //    totalmultiWrapperRange6 = parseFloat(totalmultiWrapperRange6) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#multiWrapperRange6").text()));
    //    TotalAmt = parseFloat(TotalAmt) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#TotalAmt").text()));
    //    TotalAdvanceAmount = parseFloat(TotalAdvanceAmount) + parseFloat(cmn_ReplaceCommas(CurrentRow.find("#APAdvanceAmount").text()));


    //    arr.push(list);
    //});
    //var Totallist = {};
    //Totallist.totalmultiWrapperRange1 = totalmultiWrapperRange1;
    //Totallist.totalmultiWrapperRange2 = totalmultiWrapperRange2;
    //Totallist.totalmultiWrapperRange3 = totalmultiWrapperRange3;
    //Totallist.totalmultiWrapperRange4 = totalmultiWrapperRange4;
    //Totallist.totalmultiWrapperRange5 = totalmultiWrapperRange5;
    //Totallist.totalmultiWrapperRange6 = totalmultiWrapperRange6;
    //Totallist.TotalAmt = TotalAmt;
    //Totallist.TotalAdvanceAmount = TotalAdvanceAmount;
    //Totalarr.push(Totallist);

    //var Totalarray = JSON.stringify(Totalarr);
    //$("#TotalAccountPayblePrintData").val(Totalarray);


    var array = JSON.stringify(arr);
    $("#AccountPayblePrintData").val(array);
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    if (ReportType == "D") {
        $("#datatable-buttons6 thead tr").each(function () {
            var CurrentRow = $(this);
            var list = {};
            list.Range1 = CurrentRow.find("#th_Range1").text();
            list.Range2 = CurrentRow.find("#th_Range2").text();
            list.Range3 = CurrentRow.find("#th_Range3").text();
            list.Range4 = CurrentRow.find("#th_Range4").text();
            list.Range5 = CurrentRow.find("#th_Range5").text();
            list.Range6 = CurrentRow.find("#th_Range6").text();
            rangeList.push(list);
        });
    }
    else {
        $("#datatable-buttons5 thead tr").each(function () {
            var CurrentRow = $(this);
            var list = {};
            list.Range1 = CurrentRow.find("#th_Range1").text();
            list.Range2 = CurrentRow.find("#th_Range2").text();
            list.Range3 = CurrentRow.find("#th_Range3").text();
            list.Range4 = CurrentRow.find("#th_Range4").text();
            list.Range5 = CurrentRow.find("#th_Range5").text();
            list.Range6 = CurrentRow.find("#th_Range6").text();
            rangeList.push(list);
        });
    }
    

    var arrayRangeList = JSON.stringify(rangeList);
    $("#hdnRangeList").val(arrayRangeList);

    var basis = $("#Basis option:selected").text();
    var Date = $("#AsDate").val();
    var asDate = Date.split("-");
    var asDate1 = asDate[2] + '-' + asDate[1] + '-' + asDate[0]
    $("#hdnBasis").val(basis);
    $("#hdnAsonDate").val(asDate1);
    $("#hdnCSVPrint").val(null);
    $("#hdnInsightCSVPrint").val(null);
    $("#hdnAdvAmtInsightCSVPrint").val(null);
    $("#hdnPaidAmtInsightCSVPrint").val(null);

    $("#HdnReportType").val(ReportType);
    $("#hdnCmdPDFPrint").val("Print");

    $('form').submit();
}
function AccountPayableCSV() {
    debugger
    var arr = [];
    var rangeList = [];
    if ($("#PayableTypeO").is(":checked")) {
        //$("#Basis").attr("disabled", true)
        $("#Basis").val("S");
        //SearchSuppAgingDetail();
    }
    if ($("#PayableTypeU").is(":checked")) {
        //$("#Basis").attr("disabled", true)
        $("#Basis").val("S");
        //SearchSuppAgingDetail();
    }
    //$("#datatable-buttons5 tbody tr").each(function () {
    //$("#APHdnTble tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};
    //    list.Supp_name = CurrentRow.find("#Supp_name").text();
    //    list.Supp_id = CurrentRow.find("#Supp_id").text();
    //    list.Curr = CurrentRow.find("#Curr").text();
    //    list.multiWrapperRange1 = CurrentRow.find("#multiWrapperRange1").text();
    //    list.multiWrapperRange2 = CurrentRow.find("#multiWrapperRange2").text();
    //    list.multiWrapperRange3 = CurrentRow.find("#multiWrapperRange3").text();
    //    list.multiWrapperRange4 = CurrentRow.find("#multiWrapperRange4").text();
    //    list.multiWrapperRange5 = CurrentRow.find("#multiWrapperRange5").text();
    //    list.multiWrapperRange6 = CurrentRow.find("#multiWrapperRange6").text();
    //    list.TotalAmt = CurrentRow.find("#TotalAmt").text();
    //    list.AdvanceAmount = CurrentRow.find("#APAdvanceAmount").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#AccountPayblePrintData").val(array);
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    if (ReportType == "D") {
        $("#datatable-buttons6 thead tr").each(function () {
            var CurrentRow = $(this);
            var list = {};
            list.Range1 = CurrentRow.find("#th_Range1").text();
            list.Range2 = CurrentRow.find("#th_Range2").text();
            list.Range3 = CurrentRow.find("#th_Range3").text();
            list.Range4 = CurrentRow.find("#th_Range4").text();
            list.Range5 = CurrentRow.find("#th_Range5").text();
            list.Range6 = CurrentRow.find("#th_Range6").text();
            rangeList.push(list);
        });
    }
    else {
        $("#datatable-buttons5 thead tr").each(function () {
            var CurrentRow = $(this);
            var list = {};
            list.Range1 = CurrentRow.find("#th_Range1").text();
            list.Range2 = CurrentRow.find("#th_Range2").text();
            list.Range3 = CurrentRow.find("#th_Range3").text();
            list.Range4 = CurrentRow.find("#th_Range4").text();
            list.Range5 = CurrentRow.find("#th_Range5").text();
            list.Range6 = CurrentRow.find("#th_Range6").text();
            rangeList.push(list);
        });
    }
   

    var arrayRangeList = JSON.stringify(rangeList);
    $("#hdnRangeList").val(arrayRangeList);

    $("#hdnCmdPDFPrint").val(null);
    $("#hdnInsightCSVPrint").val(null);
    $("#hdnAdvAmtInsightCSVPrint").val(null);
    $("#hdnPaidAmtInsightCSVPrint").val(null);
    $("#HdnReportType").val(ReportType);
    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function AccountRecevablInsightCSV() {
    debugger;
    var arr = [];
    //$("#tblapInvDetail tbody tr").each(function () {
        $("#hdntblapInvDetail tbody tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.Ap_Bill_No = CurrentRow.find("#Ap_Bill_No").text();
        list.Ap_Bill_Dt = CurrentRow.find("#Ap_Bill_Dt").text();
        list.Hdn_InvNo = CurrentRow.find("#Hdn_InvNo").text();
        list.Inv_Dt = CurrentRow.find("#Inv_Dt").text();
        list.AP_Invoice_Amt = CurrentRow.find("#AP_Invoice_Amt").text();
        list.paid_amt = CurrentRow.find("#multiWrapper").text();
        list.AP_Balance_Amt = CurrentRow.find("#AP_Balance_Amt").text();
        list.AP_due_Date = CurrentRow.find("#AP_due_Date").text();
        list.AP_Payment_Terms = CurrentRow.find("#AP_Payment_Terms").text();
        list.AP_due_days = CurrentRow.find("#AP_due_days").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#AccountPayblePrintData").val(array);

    $("#hdnCmdPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnPaidAmtInsightCSVPrint").val(null);
    $("#hdnAdvAmtInsightCSVPrint").val(null);
    $("#hdnInsightCSVPrint").val("InsightCsvPrint");
    //var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
   // $("#searchValue").val(searchValue);
    $('form').submit();
}
function OnclickInvoiceDetailIcon(e, type) {
    debugger;
    var Basis = $("#Basis").val();
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");
    var SuppId = CurrentRow.find("#Supp_id").text();

    var Supp_name = CurrentRow.find("#Supp_name").text();
    var Curr = CurrentRow.find("#Curr").text();
    var CurrId = CurrentRow.find("#Curr_id").text();/*Add by Hina Sharma on 18-12-2024 for filter*/

    var range = "";
    if (type === "r1") {
        range = CurrentRow.find("#hdnr1").val();
    }
    if (type === "r2") {
        range = CurrentRow.find("#hdnr2").val();
    }
    if (type === "r3") {
        range = CurrentRow.find("#hdnr3").val();
    }
    if (type === "r4") {
        range = CurrentRow.find("#hdnr4").val();
    }
    if (type === "r5") {
        range = CurrentRow.find("#hdnr5").val();
    }
    if (type === "r6") {
        range = CurrentRow.find("#hdnr6").val();
    }
    debugger;
    var lurange = range.split(',');
    if (lurange[1] == null) {
        lurange[1] = "10000";
    }
    var PayableType = "A";
    if ($("#PayableTypeA").is(":checked")) {
        PayableType = "A";
    }
    if ($("#PayableTypeO").is(":checked")) {
        PayableType = "O";
    }
    if ($("#PayableTypeU").is(":checked")) {
        PayableType = "U";
    }
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var inv_no = CurrentRow.find("#Inv_no").text();
    var inv_dt = CurrentRow.find("#Inv_dt").text();
    if (ReportType == "D") {
        var inv_date = inv_dt.split("-");
        var InvDate = inv_date[2] + '-' + inv_date[1] + '-' + inv_date[0]
    }
    else {
        var InvDate = "";
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountPayable/GetInvoiceListDetail",
            data: {
                Supp_id: SuppId,
                Basis: Basis,
                AsDate: AsDate,
                lrange: lurange[0],
                urange: lurange[1],
                CurrId: CurrId,
                PayableType: PayableType,
                ReportType: ReportType,
                inv_no: inv_no,
                inv_dt: InvDate,
                brlist: brid_list
            },
            success: function (data) {
                debugger;
                $('#PartialInvDetail').html(data);

                cmn_apply_datatable("#tblapInvDetail");

                $("#SupplierName").val(Supp_name);
                $("#HdnSupplierNameForPaidAmt").val(SuppId);
                $("#Currency").val(Curr);

                //$("a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#tblapInvDetail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountRecevablInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
function OnCliekAdvanceAmountIcon(e) {
    var CurrentRow = $(e.target).closest("tr");
    var accId = CurrentRow.find("#acc_id").text();
    /*var Supp_name = CurrentRow.find("#Supp_name").text();*/
    var CurrId = CurrentRow.find("#Curr_id").text();/*Add by Hina Sharma on 18-12-2024 for filter*/
    var AsDate = $("#AsDate").val();/*Add by Shubham Maurya on 27-02-2025 for filter*/
    var Basis = $("#Basis").val();/*Add by Shubham Maurya on 27-02-2025 for filter*/
    var PayableType = "A";
    if ($("#PayableTypeA").is(":checked")) {
        PayableType = "A";
    }
    if ($("#PayableTypeO").is(":checked")) {
        PayableType = "O";
    }
    if ($("#PayableTypeU").is(":checked")) {
        PayableType = "U";
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountPayable/SearchAdvanceAmountDetail",
            data: {
                accId: accId,
                CurrId: CurrId,
                AsDate: AsDate,
                Basis: Basis,
                PayableType: PayableType,
                brlist: brid_list
            },
            success: function (data) {
                debugger;
                $('#AdvancePaymentDetailsPopup').html(data);

                cmn_apply_datatable("#TblAdvPmt");

                //$("#TblAdvPmt_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#TblAdvPmt_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountPayblAdvAmtInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //$("#SupplierName").val(Supp_name);
                //$("#Currency").val(Curr);
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
function AccountPayblAdvAmtInsightCSV() {
    debugger;
    var arr = [];
    $("#Hdn_TblAdvPmt tbody tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.AR_VouNo = CurrentRow.find("#Adv_AP_VouNo").text();
        list.AR_VouDate = CurrentRow.find("#Adv_AP_VouDate").text();
        list.AR_VouType = CurrentRow.find("#Adv_AP_VouType").text();
        list.AR_paid_amt = CurrentRow.find("#Adv_AP_amt_sp").text();
        list.Adj_amt = CurrentRow.find("#Adv_AP_adj_amt").text();
        list.Pend_amt = CurrentRow.find("#Adv_AP_pend_amt").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#AccountPayblePrintData").val(array);

    $("#hdnCmdPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnInsightCSVPrint").val(null);
    $("#hdnAdvAmtInsightCSVPrint").val("AdvAmtInsightCsvPrint");
    $('form').submit();
}
function OnclickPaidAmtDetailIcon(e) {/*Add by hina sharma on 11-12-2024*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InVNo = CurrentRow.find("#Hdn_InvNo").text();
    var InvDate = CurrentRow.find("#InvDate").text();
    var InvDt = CurrentRow.find("#Inv_Dt").text();
    var ason_date = $("#AsDate").val();
    var supp_id= $("#HdnSupplierNameForPaidAmt").val();
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountPayable/SearchPaidAmountDetail",
            data: {
                InVNo: InVNo,
                InvDate: InvDate,
                asondate: ason_date,
                supp_id: supp_id
            },
            success: function (data) {
                debugger;
                $('#PaidAmountDetailsPopup').html(data);
                $("#Prtal_PAInvoiceNumber").val(InVNo);
                $("#Prtal_PAInvoiceDate").val(InvDt);

                cmn_apply_datatable("#tbl_PaidAmtDetails");

                //$("#tbl_PaidAmtDetails_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#tbl_PaidAmtDetails_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountPayblPaidAmtInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
function AccountPayblPaidAmtInsightCSV() {
    debugger;
    var arr = [];
    $("#tbl_PaidAmtDetails tbody tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.AR_VouNo = CurrentRow.find("#AR_VouNo").text();
        list.AR_VouDate = CurrentRow.find("#AR_VouDate").text();
        list.AR_VouType = CurrentRow.find("#AR_VouType").text();
        list.AR_paid_amt = CurrentRow.find("#AR_paid_amt").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#AccountPayblePrintData").val(array);

    $("#hdnCmdPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnInsightCSVPrint").val(null);
    $("#hdnPaidAmtInsightCSVPrint").val("PaidAmtInsightCsvPrint");
    //var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    // $("#searchValue").val(searchValue);
    $('form').submit();
}
/*-------------------For Popup Invoice detail PDF Data---------------------*/
function GetPopupInoviceDetailsPDF(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Invoiceno = clickedrow.find("#Inv_No").text().trim();
    var InvDate = clickedrow.find("#InvDate").text();
    var Invno = Invoiceno.split("/");
    var INo = Invno[3];
    var code = INo.split("0");
    var Doccode = code[0];
    if (Doccode == "PDI") {    
        var Invoiceno = clickedrow.find("#Hdn_InvNo").text().trim();
    }
    //$("#hdnDocCode_InvDtlPopupPDF").val(Doccode);
    //clickedrow.find("#hdnDocCode").text(Doccode);
    if (Doccode == "DPI" || Doccode == "CIN" || Doccode == "SPI" || Doccode == "SJI" || Doccode == "IPI" || Doccode == "PDI" || Doccode == "SCI" || Doccode == "DSI" || Doccode == "SSI" || Doccode == "ESI") {
        window.location.href = "/ApplicationLayer/AccountPayable/GenerateInvoiceDetails?invNo=" + Invoiceno + "&invDate=" + InvDate + "&dataType=" + Doccode;
    }

    //$('form').submit();
}
function GenerateAccPblPDF(e) {
    debugger;
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");
    var SuppId = CurrentRow.find("#Supp_id").text();
    var CurrId = CurrentRow.find("#Curr").text();
    var Basis = $("#Basis").val();
    var Curr_Id = CurrentRow.find("#Curr_id").text();/*add by Hina on 25-12-2024 for curr_id*/
    var Acc_Id = CurrentRow.find("#acc_id").text();/*add by Hina on 25-12-2024 for acc_id*/
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    window.location.href = "/ApplicationLayer/AccountPayable/GenratePdfFile1?accId=" + SuppId + "&currId=" + CurrId + "&asOndate=" + AsDate + "&Curr_Id=" + Curr_Id + "&Acc_Id=" + Acc_Id + "&Basis=" + Basis + "&ReportType=" + ReportType + "&brlist=" + brid_list;
}
function onchangeShowAs() {
    
    SearchSuppAgingDetail()
}