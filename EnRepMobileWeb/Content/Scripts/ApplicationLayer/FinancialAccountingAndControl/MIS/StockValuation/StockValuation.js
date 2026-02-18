$(document).ready(function () {
    debugger;
    $("#itemGrpList").select2(); 
    $("#ddlFinyrList").select2();
    $("#ddl_priceList").multiselect({
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        buttonWidth: '100%',
        buttonClass: 'btn btn-default btn-custom',
        maxHeight: 200,
        numberDisplayed:2
    });
    //showLoader();
    
});
function GetStkValuationMISDetails() {
    GetStkValuation_Details("0", "", "sv_search", "");/*Add sv_search instead of search due to not show 2 tables in p&L Page by Hina Sharma on 03-02-2025 */
}

//function OnchangeFinYear() {
//    debugger;
//    $("[aria-labelledby='select2-ddlFinyrList-container']").prop("style", "border-color: #ced4da;");
//    $("#Spn_FinYear").attr("style", "border-color: #ced4da;");
//    $("#Spn_FinYear").css("display", "none");
//    var Fin_Year = $("#ddlFinyrList").val();
//    if (Fin_Year != "0") {
//        var fy = $("#ddlFinyrList").val().split('to');
//        var fy1 = fy[0].split("-");
//        var fy2 = fy[1].split("-");
//        var fin_sfy = (fy1[2].trim() + "-" + fy1[1].trim() + "-" + fy1[0].trim());
//        var fin_efy = (fy2[2].trim() + "-" + fy2[1].trim() + "-" + fy2[0].trim());
//        var FinYear = (fin_sfy + ',' + fin_efy)
//        $("#hdn_finyr").val(FinYear);
//    }
//    $.ajax(
//        {
//            type: "POST",
//            url: "/ApplicationLayer/StockValuation/BindMonth",
//            data: {
               
//                fin_sfy: fin_sfy,
//            },
//            dataType: "json",
//            success: function (data) {
//                debugger;
//                if (data !== null && data !== "") {
//                    var arr = [];
//                    $('#ddl_Month').empty();
//                    arr = JSON.parse(data);
//                    if (arr.Table.length > 0) {
//                        $('#ddl_Month').append(`<option value=0>---Select---</option>`);
//                        for (var i = 0; i < arr.Table.length; i++) {
//                            $('#ddl_Month').append(`<option value="${arr.Table[i].id}">${arr.Table[i].name}</option>`);
//                        } 
//                    }
//                    else {
//                        $('#ddl_Month').append(`<option value=0>---Select---</option>`);
//                    }
//                }
//            },
//        });
//}
//function OnchangMnth() {
//    debugger;

//    var month = $("#ddl_Month").val();

//    var mst_end_dt = month.split(',');
//    var st_dt = mst_end_dt[0];
//    var end_dt = mst_end_dt[1];

//    $("#txtFromdate").val(st_dt);
   

//    $("#txtTodate").attr('min', st_dt);
//    $("#txtTodate").attr('max', end_dt);
//    $("#txtTodate").val(end_dt);

//    $("#spanMonthName").css("display", "none");
//    $("#ddl_Month").css("border-color", "#ced4da");
//}

function OnClickIcon_ItemDetail(evt) {
    debugger;
  
    var CurrentRow = $(evt.target).closest("tr");
    var rpt_type = CurrentRow.find("#hdn_rpttype").val();
    var grp_acc_name = CurrentRow.find("#span_name").text();
    var Id = "";
    var Sp_Uom_Id = "";
    if (rpt_type == "grp_wise") {
        Id = CurrentRow.find("#hdn_grpid").val();
        Sp_Uom_Id = CurrentRow.find("#sp_uom_id").text();
    }
    if (rpt_type == "acc_wise") {
        Id = CurrentRow.find("#hdn_accid").val();
    }

    GetStkValuation_Details(Id, rpt_type, "popup", grp_acc_name, Sp_Uom_Id);
}

function GetStkValuation_Details(id, rpt_type, filer_type, grp_acc_name, sp_uom_id) {
    debugger;
    showLoader();
    let Flag = "";
    var accid = "0";
    var ItmGrpID = "0";
    if (filer_type == "popup") {
        if (rpt_type == "grp_wise") {
            ItmGrpID = id;
        }
        if (rpt_type == "acc_wise") {
            accid = id;
        }
    }
    else {
         ItmGrpID = $("#itemGrpList option:selected").val();
    }
    var Fin_Year = $("#ddlFinyrList").val();
    if (Fin_Year != "0") {
        var FinYear = $("#hdn_finyr").val();
    }
    var Month = $("#ddl_Month").val();
    var fromdt = $("#txtFromdate").val();
    var todt = $("#txtTodate").val();
    var ReportType = $('#ddl_ReportType').val();
    var CostBase = $('#ddl_costbase').val();
    var Inc_Shfl = "N";
    var Inc_zero = "N";
    if ($('#inc_shfl_flag').is(":checked")) {
        Inc_Shfl = "Y";
    }
    if ($('#Include_Zero').is(":checked")) {
        Inc_zero = "Y";
    }
    //if (Fin_Year == '0' || Fin_Year == "" || Fin_Year == null) {
    //    $("[aria-labelledby='select2-ddlFinyrList-container']").prop("style", "border-color: #ff0000;");
    //    $("#Spn_FinYear").text($("#valueReq").text());
    //    $("#Spn_FinYear").css("display", "block");
    //    Flag = 'Y';
    //}
    //else {
    //    $("#Spn_FinYear").attr("style", "border-color: #ced4da;");
    //    $("#Spn_FinYear").css("display", "none");
    //}
    //if (Month == '0' || Month == "" || Month == null) {
    //    $("#ddl_Month").attr("style", "border-color: #ff0000;");
    //    $("#spanMonthName").text($("#valueReq").text());
    //    $("#spanMonthName").css("display", "block");
    //    Flag = 'Y';
    //}
    //else {
    //    $("#spanMonthName").attr("style", "border-color: #ced4da;");
    //    $("#spanMonthName").css("display", "none");
    //}
    if (Flag == "Y") {
        hideLoader();
        return false
    }
    else {

        let priceList = $("#ddl_priceList").val();

        try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/StockValuation/SearchStkValDetails",
            data: {
                ItmGrpID: ItmGrpID, ReportType: ReportType, costbase: CostBase
                , inc_shfl: Inc_Shfl, inc_zero: Inc_zero, Acc_id: accid, from_dt: fromdt
                , to_dt: todt, ftype: filer_type, sp_uom_id: IsNull(sp_uom_id, '',)
                , priceList: priceList != null && priceList !== "" ? priceList.filter(d => d != 'multiselect-all').join(',') : ""
            },
            success: function (data) {
                debugger;
                if (filer_type == "popup") {
                    $('#div_stkval_itemwise').html(data);
                    cmn_apply_datatable("#TblStkVal_itemdwtail");
                    //$("#TblStkVal_itemdwtail_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                    //$("#TblStkVal_itemdwtail_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="StockValuationInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                }
                else {
                    $("#MISStkValDetails").html(data);
                    //$("a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                    //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="StockValuationCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                }
                $("#acc_grp_name").val(grp_acc_name.replace(" ",""));
                $("#filer_type").val(filer_type);
                $("#hdn_grp_id").val(id);
                $("#hdn_rpt_type").val(rpt_type);
                //if (filer_type == "popup") {
                //    $("a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //    $(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="StockValuationInsightCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //}
                //else {
                //    $("a.btn.btn-default.buttons-csv.buttons-html5 btn-sm").remove();
                //    $(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="StockValuationCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                //}
                hideLoader();
            },
            error: function OnError(xhr, errorType, exception) {
                hideLoader();
            }
        });
        } catch (err) {
        debugger;
        hideLoader();
        console.log("Trial Balance Error : " + err.message);
    }   
    }
}
function StockValuationInsightCSV() {
    debugger;
    var accid = "0";
    var ItmGrpID = "0";
    var id = $("#hdn_grp_id").val();
    var filer_type = $("#filer_type").val();
    var rpt_type = $("#hdn_rpt_type").val();
    var filer_type = "popup";
    if (filer_type == "popup") {
        if (rpt_type == "grp_wise") {
            ItmGrpID = id;
        }
        if (rpt_type == "acc_wise") {
            accid = id;
        }
    }
    else {
        ItmGrpID = $("#itemGrpList option:selected").val();
    }
    var Fin_Year = $("#ddlFinyrList").val();
    if (Fin_Year != "0") {
        var FinYear = $("#hdn_finyr").val();
    }
    var Month = $("#ddl_Month").val();
    var fromdt = $("#txtFromdate").val();
    var todt = $("#txtTodate").val();
    var ReportType = $('#ddl_ReportType').val();
    var CostBase = $('#ddl_costbase').val();
    var Inc_Shfl = "N";
    var Inc_zero = "N";
    if ($('#inc_shfl_flag').is(":checked")) {
        Inc_Shfl = "Y";
    }
    if ($('#Include_Zero').is(":checked")) {
        Inc_zero = "Y";
    }
    var sp_uom_id = "";
    var arr = [];
    var list = {};
    list.ItmGrpID = ItmGrpID;
    list.FinYear = FinYear;
    list.Month = Month;
    list.ReportType = ReportType;
    list.costbase = CostBase;
    list.inc_shfl = Inc_Shfl;
    list.inc_zero = Inc_zero;
    list.Acc_id = accid;
    list.from_dt = fromdt;
    list.to_dt = todt;
    list.ftype = filer_type;
    list.sp_uom_id = sp_uom_id;
    arr.push(list);

    var array = JSON.stringify(arr);
    $("#StockValuationData").val(array);

    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function StockValuationCSV() {
    debugger;
    var accid = "0";
    var ItmGrpID = "0";
    //var id = "0";
    var id = $("#hdn_grp_id").val();
    var filer_type = $("#filer_type").val();
    var rpt_type = $("#hdn_rpt_type").val();
    var filer_type = "search";
    //if (filer_type == "popup") {
    //    if (rpt_type == "grp_wise") {
    //        ItmGrpID = id;
    //    }
    //    if (rpt_type == "acc_wise") {
    //        accid = id;
    //    }
    //}
    //else {
        ItmGrpID = $("#itemGrpList option:selected").val();
    //}
    var Fin_Year = $("#ddlFinyrList").val();
    if (Fin_Year != "0") {
        var FinYear = $("#hdn_finyr").val();
    }
    var Month = $("#ddl_Month").val();
    var fromdt = $("#txtFromdate").val();
    var todt = $("#txtTodate").val();
    var ReportType = $('#ddl_ReportType').val();
    var CostBase = $('#ddl_costbase').val();
    var Inc_Shfl = "N";
    var Inc_zero = "N";
    if ($('#inc_shfl_flag').is(":checked")) {
        Inc_Shfl = "Y";
    }
    if ($('#Include_Zero').is(":checked")) {
        Inc_zero = "Y";
    }
    var sp_uom_id = "";

    var arr = [];
    var list = {};
    list.ItmGrpID = ItmGrpID;
    list.FinYear = FinYear;
    list.Month = Month;
    list.ReportType = ReportType;
    list.costbase = CostBase;
    list.inc_shfl = Inc_Shfl;
    list.inc_zero = Inc_zero;
    list.Acc_id = accid;
    list.from_dt = fromdt;
    list.to_dt = todt;
    list.ftype = filer_type;
    list.sp_uom_id = sp_uom_id;

    arr.push(list);

    var array = JSON.stringify(arr);
    $("#StockValuationData").val(array);

    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function FromToDateValidation() {
    debugger;
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
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);

            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    if (ToDate != "") {
        validatefydate(FromDate, ToDate);
    }
}
function validatefydate(FromDate, ToDate) { /*Add changes by Hina on 14-08-2024 for show current date of fin Year */
    debugger
    var validate = "N";
    var finfromdt = "";
    var finTodt = "";
    var fin_fromdt = "";
    var fin_Todt = "";
    $("#tbl_hdnfylist tbody tr").each(function () {
        var curr_row = $(this);
        var fystdt = curr_row.find("#fystdt").text();
        var fyenddt = curr_row.find("#fyenddt").text();

        var currdate = new Date();
        var fin_date = new Date(fystdt);
        var year = fin_date.getFullYear();
        var cyear = currdate.getFullYear();

        var fstdate = fystdt.replaceAll("-", "");
        var fenddate = fyenddt.replaceAll("-", "");
        var frmdate = FromDate.replaceAll("-", "");

        var yyyy = currdate.getFullYear().toString();
        var mm = (currdate.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = currdate.getDate().toString();
        var fdate = yyyy + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + (dd[1] ? dd : "0" + dd[0]);
        var fin_currdt = fdate.replaceAll("/", "-");
        var fin_dt = fdate.replaceAll("/", "");

        var boolfromdt = "N";
        var booltodt = "N";

        if ((Date.parse(fystdt) <= Date.parse(FromDate)) && (Date.parse(fyenddt) >= Date.parse(FromDate))) {
            boolfromdt = "Y";
            finfromdt = fystdt;
            finTodt = fyenddt;
            document.getElementById("txtTodate").setAttribute('min', FromDate);
            if (year == cyear) {
                document.getElementById("txtTodate").setAttribute('max', fin_currdt);
                $("#txtTodate").val(fin_currdt);
            }
            else {
                document.getElementById("txtTodate").setAttribute('max', fyenddt);
                $("#txtTodate").val(fyenddt);
            }
        }
        if ((Date.parse(fystdt) <= Date.parse(ToDate)) && (Date.parse(fyenddt) >= Date.parse(ToDate))) {
            booltodt = "Y";
        }
        if (boolfromdt == "Y" && booltodt == "Y") {
            validate = "Y";
            fin_fromdt = fystdt;
            fin_Todt = fyenddt;

            if (fstdate <= frmdate && fenddate >= frmdate) {

                document.getElementById("txtTodate").setAttribute('min', "");
                document.getElementById("txtTodate").setAttribute('max', "");

                document.getElementById("txtTodate").setAttribute('min', FromDate);
                if (year == cyear) {
                    document.getElementById("txtTodate").setAttribute('max', fin_currdt);
                    $("#txtTodate").val(fin_currdt);
                }
                else {
                    document.getElementById("txtTodate").setAttribute('max', fyenddt);
                    $("#txtTodate").val(fyenddt);
                }
            }
            else {
                document.getElementById("txtTodate").setAttribute('min', "");
                document.getElementById("txtTodate").setAttribute('max', "");

                document.getElementById("txtTodate").setAttribute('min', fystdt);
                document.getElementById("txtTodate").setAttribute('max', fyenddt);
                $("#txtTodate").val(fyenddt);
            }
            return;
        }
    });

    if (validate != "Y") {
    }
    else {
        $("#txtTodate").val(ToDate);
    }
}
function getrcpt_issuedetail(evt,flag) {
    debugger
    showLoader();
    var curr_row = $(evt.target).closest("tr");
    var rpt_type = curr_row.find("#hdn_rpttype").val();
    var grp_acc_name = curr_row.find("#span_name").text();
    var Id = "";
    if (rpt_type == "itm_wise") {
        Id = curr_row.find("#hdn_itmid").val();
    }
    if (rpt_type == "grp_wise") {
        Id = curr_row.find("#hdn_grpid").val();
    }
    if (rpt_type == "acc_wise") {
        Id = curr_row.find("#hdn_accid").val();
    }
    var fromdt = $("#txtFromdate").val();
    var todt = $("#txtTodate").val();
    var ReportType = $('#ddl_ReportType').val();
    var CostBase = $('#ddl_costbase').val();
    var Inc_Shfl = "N";
    if ($('#inc_shfl_flag').is(":checked")) {
        Inc_Shfl = "Y";
    }

    let priceList = $("#ddl_priceList").val();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/StockValuation/get_recpissuedetail",
            data: {
                id: Id,
                fromdt: fromdt,
                todt: todt,
                flag: flag,
                rpttype: ReportType,
                shflflag: Inc_Shfl,
                cost_type: CostBase,
                priceList: priceList != null && priceList !== "" ? priceList.filter(d => d != 'multiselect-all').join(',') : ""
            },
            success: function (data) {
                debugger;

                $('#div_stkval_receissue').html(data);
                
                cmn_apply_datatable("#TblStkVal_receissuedetail");

                $("#txt_name").val(grp_acc_name);

                hideLoader();
            },
            error: function OnError(xhr, errorType, exception) {
                hideLoader();
            }
        });
    } catch (err) {
        debugger;
        hideLoader();
        console.log("Stock Valuation Error : " + err.message);
    }
}

function OnChangeCostBase() {

    let costbase = $("#ddl_costbase").val();
    if (costbase == "PL") {
        $("#div_PriceList").css("display", "");
    }
    else {
        $("#div_PriceList").css("display", "none");
        /*$("#ddl_priceList").val([0]).trigger('change');*/
        $('#ddl_priceList').val([]); // clear all
        $('#ddl_priceList').multiselect('refresh');

    }
}
//function bind_PriceList() {
//    debugger
//    showLoader();
//    var fromdt = $("#txtFromdate").val();
//    var todt = $("#txtTodate").val();

//    try {
//        $.ajax({
//            type: "POST",
//            url: "/ApplicationLayer/StockValuation/PriceListData",
//            data: {
//                SearchValue: "",
//                fromdt: fromdt,
//                todt: todt,
//            },
//            success: function (data) {
//                debugger;

//                hideLoader();
//            },
//            error: function OnError(xhr, errorType, exception) {
//                hideLoader();
//            }
//        });
//    } catch (err) {
//        debugger;
//        hideLoader();
//        console.log("Stock Valuation Error : " + err.message);
//    }
//}
