
$(document).ready(function () {
    debugger
    //$("#CustomerCategory").select2();
    //$("#CustomerPortfolio").select2();
    //$("#ddl_RegionName").select2();
   /* $('#ddl_branch').multiselect();*/
    BindCustomerList();
    EnbaleDatatable();
    BindCityList();
    BindStateList();
    $(document).ready(function () {
        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountRecevablPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')
        //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    });

    Cmn_initializeMultiselect(
        ["#CustomerCategory", "#CustomerPortfolio", '#ddlSalesPerson', "#ddl_RegionName", "#ddl_customerZone", "#ddl_CustomerGroup"]
    );
    Cmn_initializeMultiselect_ddl_br(["#ddl_branch"]);
});

function onclickReceivableType() {
    if ($("#ReceivableTypeA").is(":checked")) {
        $("#Basis").attr("disabled", false)
        SearchAgingDetail();
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        $("#Basis").attr("disabled", true)
        $("#Basis").val("I").trigger('change');
        SearchAgingDetail();
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        $("#Basis").attr("disabled", true)
        $("#Basis").val("I").trigger('change');
        SearchAgingDetail();
    }
}
//function BindCustomerList() {
//    debugger;
//    var Branch = sessionStorage.getItem("BranchID");
//    $.ajax({
//        url: "/ApplicationLayer/SalesDetail/GetAutoCompleteSearchCustList",
//        data: {},
//        success: function (data, params) {
//            debugger;
//            if (data == 'ErrorPage') {
//                LSO_ErrorPage();
//                return false;
//            }
//            var s = '';
//            $.map(data, function (val, item) {
//                if (val.Name.trim() != "---All---") {
//                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
//                }
//            })
//            $("#ddl_CustomerName").html(s);
//            $("#ddl_CustomerName").multiselect({
//                includeSelectAllOption: true,
//                enableFiltering: true,
//                enableCaseInsensitiveFiltering: true,
//                buttonWidth: '100%',
//                buttonClass: 'btn btn-default btn-custom',
//                nonSelectedText: '---All---',
//                numberDisplayed: 1,
//            });
//            //$("#ddl_CustomerName").select2({});
//        }
//    });

//}
function BindCustomerList() {
    var Branch = sessionStorage.getItem("BranchID");
    $.ajax({
        url: "/ApplicationLayer/SalesDetail/GetAutoCompleteSearchCustList",
        data: {},
        success: function (data, params) {

            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = '';
            $.map(data, function (val) {
                if (val.Name.trim() != "---All---") {
                    s += '<option value="' + val.ID + '">' + val.Name + '</option>';
                }
            });
            $("#ddl_CustomerName").html(s);
           // $('#ddl_CustomerName').multiselect('destroy');
            Cmn_initializeMultiselect(["#ddl_CustomerName"]);
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
        $("#DivCustAgingtable").css("display", "none");

    }
    else {
        debugger;
        $("#BtnSearch").prop("disabled", false);
        $("#DivCustAgingtable").css("display", "block");
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

    if (parseFloat(range1) >= parseFloat(range2) ) {
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
function SearchAgingDetail() {
    debugger;
    try {
        // var Cust_id = $("#ddl_CustomerName option:selected").val();
        //var Cat_id = $("#CustomerCategory").val();
        //var Prf_id = $("#CustomerPortfolio").val();
        //var Reg_id = $("#ddl_RegionName").val();
        var Cust_id = cmn_getddldataasstring("#ddl_CustomerName"); //$("#ddl_CustomerName option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
        var Cat_id = cmn_getddldataasstring("#CustomerCategory"); //$("#CustomerCategory option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
        var Prf_id = cmn_getddldataasstring("#CustomerPortfolio"); //$("#CustomerPortfolio option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
        var Reg_id = cmn_getddldataasstring("#ddl_RegionName"); //$("#ddl_RegionName option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
        var brid_list = cmn_getddldataasstring("#ddl_branch");
        var sales_per = cmn_getddldataasstring("#ddlSalesPerson");
        var customerZone = cmn_getddldataasstring("#ddl_customerZone");
        var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");
        var state_id = $('#ddl_state').val();
        var city_id = $('#ddl_city').val();

        var Basis = $("#Basis").val();
        var AsDate = $("#AsDate").val();
        var ReceivableType = "A";
        if ($("#ReceivableTypeA").is(":checked")) {
            ReceivableType = "A";
        }
        if ($("#ReceivableTypeO").is(":checked")) {
            ReceivableType = "O";
        }
        if ($("#ReceivableTypeU").is(":checked")) {
            ReceivableType = "U";
        }
        var ReportType = "S";
        if ($("#ProcurementMISOrderSummary").is(":checked")) {
            ReportType = "S";
            $("#SlsPersIdDiv").css("display", "none");
            sales_per = "0";
        }
        if ($("#ProcurementMISOrderDetail").is(":checked")) {
            ReportType = "D";
            $("#SlsPersIdDiv").css("display", "block");
        }
        $("#HdnReportType").val(ReportType);

        //var br_list = $("#ddl_branch").val();

        //var brid_list = cmn_multibranchlist(br_list);
        var brid_list = cmn_getddldataasstring("#ddl_branch");
        if (brid_list == '0' || brid_list == "" || brid_list == null) {
            return false;
        }

        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/SearchAccountReceivableDetail",
            data: {
                Cust_id: Cust_id,
                Cat_id: Cat_id,
                Prf_id: Prf_id,
                Reg_id: Reg_id,
                Basis: Basis,
                AsDate: AsDate,
                ReceivableType: ReceivableType,
                ReportType: ReportType,
                brlist: brid_list,
                sales_per: sales_per,
                customerZone: customerZone,
                CustomerGroup: CustomerGroup,
                state_id: state_id,
                city_id: city_id
            },
            success: function (data) {
                debugger;
                $('#DivCustAgingtable').html(data);
                $("a.btn.btn-default.buttons-print.btn-sm").remove();
                $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm ap" onclick="AccountRecevablPDF()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>PDF</span></button>')

                //$("a.btn btn-default.buttons-csv buttons-html5 btn-sm").remove();
                //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

                $("#DivCustAgingtable").css("display", "block")
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
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
function AccountRecevablCSV() {
    debugger;
    var arr = [];
    //$("#HdnTableAccountReceivable tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};
    //    list.cust_name = CurrentRow.find("#cust_name").text();
    //    list.cust_id = CurrentRow.find("#cust_id").text();
    //    list.Curr = CurrentRow.find("#hdn_curr").text();
    //    list.r1 = CurrentRow.find("#hdn_r1").text();
    //    list.r2 = CurrentRow.find("#hdn_r2").text();
    //    list.r3 = CurrentRow.find("#hdn_r3").text();
    //    list.r4 = CurrentRow.find("#hdn_r4").text();
    //    list.r5 = CurrentRow.find("#hdn_r5").text();
    //    list.tamt = CurrentRow.find("#hdn_tamt").text();
    //    list.AdvanceAmount = CurrentRow.find("#hdn_AdvanceAmount").text();
    //    list.gtr5 = CurrentRow.find("#hdn_gtr5").text();
    //    list.range1 = CurrentRow.find("#hdn_range1").text();
    //    list.range2 = CurrentRow.find("#hdn_range2").text();
    //    list.range3 = CurrentRow.find("#hdn_range3").text();
    //    list.range4 = CurrentRow.find("#hdn_range4").text();
    //    list.range5 = CurrentRow.find("#hdn_range5").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#AccRecvablPDFData").val(array);

    var basis = $("#Basis").val()
    $("#HdnBasis").val(basis);


    $("#hdnPDFPrint").val(null);
    $("#hdnCSVInsight").val(null);
    $("#hdnAdvAmtCSVInsight").val(null);
    $("#hdnPaidAmtCSVInsight").val(null);
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    $("#HdnReportType").val(ReportType);
    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);
    return false;
    $('form').submit();
}
function AccountRecevablPDF() {
    debugger;
    var arr = [];

    //$("#HdnTableAccountReceivable tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};
    //    list.cust_name = CurrentRow.find("#cust_name").text();
    //    list.cust_id = CurrentRow.find("#cust_id").text();
    //    list.Curr = CurrentRow.find("#hdn_curr").text();
    //    list.r1 = CurrentRow.find("#hdn_r1").text();
    //    list.r2 = CurrentRow.find("#hdn_r2").text();
    //    list.r3 = CurrentRow.find("#hdn_r3").text();
    //    list.r4 = CurrentRow.find("#hdn_r4").text();
    //    list.r5 = CurrentRow.find("#hdn_r5").text();
    //    list.tamt = CurrentRow.find("#hdn_tamt").text();
    //    list.gtr5 = CurrentRow.find("#hdn_gtr5").text();
    //    list.AdvanceAmount = CurrentRow.find("#hdn_AdvanceAmount").text();
    //    list.range1 = CurrentRow.find("#hdn_range1").text();
    //    list.range2 = CurrentRow.find("#hdn_range2").text();
    //    list.range3 = CurrentRow.find("#hdn_range3").text();
    //    list.range4 = CurrentRow.find("#hdn_range4").text();
    //    list.range5 = CurrentRow.find("#hdn_range5").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#AccRecvablPDFData").val(array);

    var basis = $("#Basis").val()
    $("#HdnBasis").val(basis);

    $("#hdnCSVPrint").val(null);
    $("#hdnCSVInsight").val(null);
    $("#hdnAdvAmtCSVInsight").val(null);
    $("#hdnPaidAmtCSVInsight").val(null);
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var Cust_id = cmn_getddldataasstring("#ddl_CustomerName");//$("#ddl_CustomerName option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var Cat_id = cmn_getddldataasstring("#CustomerCategory");//$("#CustomerCategory option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var Prf_id = cmn_getddldataasstring("#CustomerPortfolio");//$("#CustomerPortfolio option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var Reg_id = cmn_getddldataasstring("#ddl_RegionName");//$("#ddl_RegionName option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var br_ids = cmn_getddldataasstring("#ddl_branch");//$("#ddl_branch option:selected").map(function () { return $(this).val(); }).get().join(",") || "";
    var customerZone = cmn_getddldataasstring("#ddl_customerZone");
    var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");


    $("#HdCustomerName").val(Cust_id);
    $("#Hdcategory").val(Cat_id);
    $("#HdportFolioLists").val(Prf_id);
    $("#HdRegionName").val(Reg_id);
    $("#HdnReportType").val(ReportType);
    $("#Hdn_brlist").val(br_ids);

    $("#HdcustomerZone").val(customerZone);
    $("#HdCustomerGroup").val(CustomerGroup);
    $("#hdnPDFPrint").val("Print");
    $('form').submit();

}
function OnclickInvoiceDetailIcon(e,type) {
    debugger;
    //var Basis = "I";
    var Basis = $("#Basis").val();
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");  
    var CustId = CurrentRow.find("#Cust_id").text();

    var Cust_name = CurrentRow.find("#Cust_name").text();
    var Curr = CurrentRow.find("#Curr").text();
    var CurrId = CurrentRow.find("#Curr_id").text();/*Add by Hina Sharma on 17-12-2024 for filter*/
    var sales_per = cmn_getddldataasstring("#ddlSalesPerson");

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
    if (lurange[0] == "0") {
        lurange[0] = "-10000";
    }
    var ReceivableType = "A";
    if ($("#ReceivableTypeA").is(":checked")) {
        ReceivableType = "A";
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        ReceivableType = "O";
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        ReceivableType = "U";
    }
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
        sales_per = "0";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var inv_no = CurrentRow.find("#Inv_no").text();
    var inv_dt = CurrentRow.find("#Inv_dt").text();
    if (ReportType == "S") {
        var InvDate = "";
    }
    else {
        var inv_date = inv_dt.split("-");
        var InvDate = inv_date[2] + '-' + inv_date[1] + '-' + inv_date[0];
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/GetInvoiceListDetail",
            data: {
                Cust_id: CustId,               
                Basis: Basis,
                AsDate: AsDate,
                lrange: lurange[0],
                urange: lurange[1],
                CurrId: CurrId,
                ReceivableType: ReceivableType,
                ReportType: ReportType,
                inv_no: inv_no,
                inv_dt: InvDate,
                brlist: brid_list,
                sls_per: sales_per
            },
            success: function (data) {
                debugger;
                $('#PartialInvDetail').html(data);
                $("#CustomerName").val(Cust_name);
                $("#HdnCustomerNameForPaidAmt").val(CustId);
                $("#Currency").val(Curr);
                cmn_apply_datatable("#tbl_InvoiceDetails");

                //$("#tbl_InvoiceDetails_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#tbl_InvoiceDetails_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountRecevablInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                
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
function AccountRecevablInsightCSV() {
    debugger;
    var arr = [];
    $("#tbl_InvoiceDetails >tbody >tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.inv_no = CurrentRow.find("#Hdn_InvNo").text();
        list.inv_dt = CurrentRow.find("#td_InvDt").text();
        list.ApInvoice_Amt = CurrentRow.find("#ApInvoice_Amt").text();
        list.Paid_amt = CurrentRow.find("#multiWrapper").text();
        list.ApBalance_Amt = CurrentRow.find("#ApBalance_Amt").text();
        list.Apdue_Date = CurrentRow.find("#Apdue_Date").text();
        list.APdue_days = CurrentRow.find("#APdue_days").text();       
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#hdnCSVInsightData").val(array);

    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnPaidAmtCSVInsight").val(null);
    $("#hdnAdvAmtCSVInsight").val(null);
    $("#hdnCSVInsight").val("CSVInsight");
    var searchValue = $("#tbl_InvoiceDetails_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function OnCliekAdvanceAmountIcon(e) {
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var accId = CurrentRow.find("#acc_id").text();
    /*var Supp_name = CurrentRow.find("#Supp_name").text();*/
    var CurrId = CurrentRow.find("#Curr_id").text();/*Add by Hina Sharma on 17-12-2024 for filter*/
    var CurrName = CurrentRow.find("#Curr").text();/*Add by Hina Sharma on 17-12-2024 for filter*/
    var AsDate = $("#AsDate").val();/*Add by SHubham Maurya on 27-02-2025 for filter*/
    var Basis = $("#Basis").val();/*Add by SHubham Maurya on 27-02-2025 for filter*/
    var ReceivableType = "A";/*Add by SHubham Maurya on 27-02-2025 for filter*/
    if ($("#ReceivableTypeA").is(":checked")) {
        ReceivableType = "A";
    }
    if ($("#ReceivableTypeO").is(":checked")) {
        ReceivableType = "O";
    }
    if ($("#ReceivableTypeU").is(":checked")) {
        ReceivableType = "U";
    }
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/SearchAdvanceAmountDetail",
            data: {
                accId: accId,
                CurrId: CurrId,
                AsDate: AsDate,
                Basis: Basis,
                ReceivableType: ReceivableType,
                brlist: brid_list,
                CurrName: CurrName
            },
            success: function (data) {
                debugger;
                $('#AdvancePaymentDetailsPopup').html(data);

                cmn_apply_datatable("#TblAdvPmt");

                //$("#TblAdvPmt_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                //$("#TblAdvPmt_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="AccountRecevablAdvAmtInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
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
function AccountRecevablAdvAmtInsightCSV() {
    debugger;
    var arr = [];
    $("#Hdn_TblAdvPmt >tbody >tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.VouNo = CurrentRow.find("#Adv_AP_VouNo").text();
        list.VouDt = CurrentRow.find("#Adv_AP_VouDate").text();
        list.VouType = CurrentRow.find("#Adv_AP_VouType").text();
        list.amt_sp = CurrentRow.find("#Adv_AP_amt_sp").text();
        list.adj_amt = CurrentRow.find("#Adv_AP_adj_amt").text();
        list.pend_amt = CurrentRow.find("#Adv_AP_pend_amt").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#AccRecvablPDFData").val(array);

    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnPaidAmtCSVInsight").val(null);
    $("#hdnCSVInsight").val(null);
    $("#hdnAdvAmtCSVInsight").val("AdvAmtCSVInsight");
    var searchValue = $("#TblAdvPmt_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function OnclickPaidAmtDetailIcon(e) {/*Add by hina sharma on 10-12-2024*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InVNo = CurrentRow.find("#Hdn_InvNo").text();
    var InvDate = CurrentRow.find("#InvDate").text();
    var InvDt = CurrentRow.find("#td_InvDt").text();
    var cust_id = $("#HdnCustomerNameForPaidAmt").val();
    /*var Curr = CurrentRow.find("#Curr").text();*/
    try {
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/SearchPaidAmountDetail",
            data: {
                InVNo: InVNo,
                InvDate: InvDate,
                cust_id: cust_id
            },
            success: function (data) {
                debugger;
                $('#PaidAmountDetailsPopup').html(data);
                $("#Prtal_PAInvoiceNumber").val(InVNo);
                $("#Prtal_PAInvoiceDate").val(InvDt);

                cmn_apply_datatable("#tbl_PaidAmtDetails");

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
function AccountRecevablPaidAmtInsightCSV() {
    debugger;
    var arr = [];
    $("#Hdn_tbl_PaidAmtDetails >tbody >tr").each(function () {
        var CurrentRow = $(this);
        var list = {};
        list.AR_VouNo = CurrentRow.find("#AR_VouNo").text();
        list.AR_VouDate = CurrentRow.find("#AR_VouDate").text();
        list.AR_VouType = CurrentRow.find("#AR_VouType").text();
        list.AR_paid_amt = CurrentRow.find("#AR_paid_amt").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#hdnPaidAmtCSVInsightData").val(array);

    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val(null);
    $("#hdnCSVInsight").val(null);
    $("#hdnAdvAmtCSVInsight").val(null);
    $("#hdnPaidAmtCSVInsight").val("PaidAmtCSVInsight");
    var searchValue = $("#tbl_PaidAmtDetails_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
/*-------------------For Popup Invoice detail PDF Data---------------------*/
function AccRec_GetPopupInoviceDetailsPDF(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Invoiceno = clickedrow.find("#Inv_No").text().trim();
    var InvDate = clickedrow.find("#InvDate").text();
    var Invno = Invoiceno.split("/");
    var INo = Invno[3];
    var code = INo.split("0");
    var Doccode = code[0];
    //$("#hdnDocCode_InvDtlPopupPDF").val(Doccode);
    //clickedrow.find("#hdnDocCode").text(Doccode);
    if (Doccode == "DSI" || Doccode == "CIN" || Doccode == "ESI" || Doccode == "SSI" || Doccode == "SCI" || Doccode == "SJI" || Doccode == "DSI" || Doccode == "SSI" || Doccode == "IPI" || Doccode == "PDI")
    window.location.href = "/ApplicationLayer/AccountReceivable/GenerateInvoiceDetails?invNo=" + Invoiceno + "&invDate=" + InvDate + "&dataType=" + Doccode;

    //$('form').submit();
}

function GenerateAccRcblPDF(e) {
    debugger;
    var AsDate = $("#AsDate").val();
    var CurrentRow = $(e.target).closest("tr");
    var custId = CurrentRow.find("#Cust_id").text(); 
    var CurrId = CurrentRow.find("#Curr").text();
    var Curr_Id = CurrentRow.find("#Curr_id").text();/*add by Hina on 24-12-2024 for curr_id*/
    var Acc_Id = CurrentRow.find("#acc_id").text();/*add by Hina on 24-12-2024 for acc_id*/
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    $("#HdnReportType").val(ReportType);
    var brid_list = cmn_getddldataasstring("#ddl_branch");
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    var sales_per = cmn_getddldataasstring("#ddlSalesPerson");
    var customerZone = cmn_getddldataasstring("#ddl_customerZone");
    var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");
    var state_id = $("#ddl_state").val();
    var ddl_city = $("#ddl_city").val();

    window.location.href = "/ApplicationLayer/AccountReceivable/GenratePdfFile1?accId=" + custId + "&currId=" + CurrId + "&asOndate=" + AsDate + "&Curr_Id=" + Curr_Id + "&Acc_Id=" + Acc_Id + "&ReportType=" + ReportType + "&brlist=" + brid_list + "&sales_per=" + sales_per + "&customerZone=" + customerZone + "&CustomerGroup=" + CustomerGroup + "&state_id=" + state_id + "&city_id=" + ddl_city;
}
function onchangeShowAs() {
    SearchAgingDetail()
}
//Added by Nidhi on 03-10-2025
function SendEmail(e) {
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var data = {
        custname: CurrentRow.find("#Cust_name").text(),
        curr: CurrentRow.find("#Curr").text(),
        //Totalamount: CurrentRow.find("#totalamt").text(),
        Totalamount: CurrentRow.find("#hdnTotalAmt").val(),
        Acc_Id: CurrentRow.find("#acc_id").text(),
        CustId: CurrentRow.find("#Cust_id").text(),
        CurrId: CurrentRow.find("#Curr_id").text()
    };
     localStorage.setItem("emailData", JSON.stringify(data));

    var custId = CurrentRow.find("#Cust_id").text();
    var docid = $("#DocumentMenuId").val();
    Cmn_SendEmail(docid, custId, 'Cust');
}
function ViewEmailAlert() {
    debugger;
    var storedData = localStorage.getItem("emailData");
    if (storedData) {
        var data = JSON.parse(storedData);
        var Custname = data.custname;
        var Curr = data.curr;
        var Totalamount = data.Totalamount;
        var Curr_id = data.CurrId;
        var Cust_id = data.CustId;
        var Acc_id = data.Acc_Id;
    }
    var AsDate = $("#AsDate").val();
    var mail_id = $("#Email").val().trim();
    var status = "A";
    var docid = $("#DocumentMenuId").val();
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
    var sales_per = cmn_getddldataasstring("#ddlSalesPerson");

    var customerZone = cmn_getddldataasstring("#ddl_customerZone");
    var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");
    var state_id = $("#ddl_state").val();
    var city_id = $("#ddl_city").val();

    var Custdetail = Custname + '_' + Curr + '_' + AsDate + '_' + Totalamount;
    var pdfAlertEmailFilePath = 'CustomerStatement_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/AccountReceivable/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Cust_id: Cust_id, Curr: Curr, Curr_id: Curr_id, Acc_id: Acc_id, AsDate: AsDate, ReportType: ReportType, fileName: pdfAlertEmailFilePath, br_list: brid_list, sales_per: sales_per, customerZone: customerZone, CustomerGroup: CustomerGroup, state_id: state_id, city_id: city_id },
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, Custdetail, "", filepath);
                }
            });
}
function SendEmailAlert() {
    debugger;
    var filepath = $('#hdfilepathpdf').val()
    var storedData = localStorage.getItem("emailData");
    if (storedData) {
        var data = JSON.parse(storedData);
        var Custname = data.custname;
        var Curr = data.curr;
        var Totalamount = data.Totalamount;
        var Curr_id = data.CurrId;
        var Cust_id = data.CustId;
        var Acc_id = data.Acc_Id;
    }

    var AsDate = $("#AsDate").val();
    var mail_id = $("#Email").val().trim();
    var status = "A";
    var docid = $("#DocumentMenuId").val();
    var ReportType = "S";
    if ($("#ProcurementMISOrderSummary").is(":checked")) {
        ReportType = "S";
    }
    if ($("#ProcurementMISOrderDetail").is(":checked")) {
        ReportType = "D";
    }
    var Doc_no = Custname + '_' + Curr + '_' + AsDate + '_' + Totalamount + '_' + Acc_id;
    var brid_list = cmn_getddldataasstring("#ddl_branch");

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }
    var sales_per = cmn_getddldataasstring("#ddlSalesPerson");
    var customerZone = cmn_getddldataasstring("#ddl_customerZone");
    var CustomerGroup = cmn_getddldataasstring("#ddl_CustomerGroup");
    var state_id = $("#ddl_state").val();
    var city_id = $("#ddl_city").val();

    if (filepath == "" || filepath == null) {
        var pdfAlertEmailFilePath = 'CustomerStatement_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AccountReceivable/SavePdfDocToSendOnEmailAlert_Ext",
            data: { Cust_id: Cust_id, Curr: Curr, Curr_id: Curr_id, Acc_id: Acc_id, AsDate: AsDate, ReportType: ReportType, fileName: pdfAlertEmailFilePath, br_list: brid_list, sales_per: sales_per, customerZone: customerZone, CustomerGroup: CustomerGroup, state_id: state_id, city_id: city_id },
            /*dataType: "json",*/
            success: function (data) {
                filepath = data;
                $('#hdfilepathpdf').val(filepath)
                Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, AsDate, "", "/Common/Common/SendEmailAlert", filepath)
            }
        });
    }
    else {
        Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, AsDate, "", "/Common/Common/SendEmailAlert", filepath)
    }
}
function EmailAlertLogDetails() {
    debugger
    var storedData = localStorage.getItem("emailData");
    if (storedData) {
        var data = JSON.parse(storedData);
        var Custname = data.custname;
        var Curr = data.curr;
        var Acc_id = data.Acc_Id;
    }
    var status = "A";
    var docid = $("#DocumentMenuId").val();
    // var Doc_no = Custname + '_' + Curr
    var Doc_no = Custname + '_' + Acc_id;
    var Doc_dt = $("#AsDate").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
function BindCityList() {
    debugger;
    var state_id = $("#ddl_state").val();
    $("#ddl_city").append("<option value='0'>---Select---</option>");
    $("#ddl_city").select2({
        ajax: {
            url: "/ApplicationLayer/AccountReceivable/BindCityListdata",
            data: function (params) {
                var queryParameters = {
                    SearchCity: params.term,
                    state_id: state_id,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                  <div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#span_City").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                  </strong></li></ul>`)
                }
                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (data[0] != null) {
                        if (data[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0,0", Name: " ---Select---" };
                            data.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, UOM: val.ID.split(",")[1], type: val.ID.split(",")[2] };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function BindStateList() {
    debugger;
    $("#ddl_state").append("<option value='0'>---Select---</option>");
    $("#ddl_state").select2({
        ajax: {
            url: "/ApplicationLayer/AccountReceivable/BindStateListData",
            data: function (params) {
                var queryParameters = {
                    SearchState: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                   <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                   <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                   </strong></li></ul>`)
                }
                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (data[0] != null) {
                        if (data[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0", Name: " ---Select---" };
                            data.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, UOM: val.ID.split(",")[1] };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function onchangeStateName() {
    BindCityList()
}