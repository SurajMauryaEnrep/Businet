$(document).ready(function () {
    $('#ddlGlAccList').select2();
    $('#ddlFYList').select2();
    $('#ddlQuarterList').select2();
    $('#ddlMonthsList').select2();
});
function OnChangeFY() {
    debugger;
    var finPeriod = $('#ddlFYList').val();
    $('#txtPeriod').val(finPeriod);
    var finYear = $('#ddlFYList option:selected').text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetQtrList",
        data: {
            finYear: finYear
        },
        success: function (data) {
            debugger;
            $("#ddlQuarterList").empty();
            var arr = [];
            arr = JSON.parse(data);
            var s = '<option value="0">---Select---</option>';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    s += '<option value="' + arr[i].mnth_qtr_name + '">' + arr[i].mnth_qtr_name + '</option>';
                }
            }
            $("#ddlQuarterList").html(s);
            /*alert(data);*/
        }
    })
}
function OnChangeQtr() {
    debugger;
    var finYear = $('#ddlFYList option:selected').text();
    var qtrName = $('#ddlQuarterList').val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetMnthsList",
        data: {
            finYear: finYear,
            qtrName: qtrName
        },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);
            var s = '<option value="0">---Select---</option>';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    s += '<option value="' + arr[i].mnth_qtr_name + '">' + arr[i].mnth_qtr_name + '</option>';
                }
            }
            $("#ddlMonthsList").html(s);
            /*alert(data);*/
        }

    })
}
function ReplaceId() {
    try {
        $("#datatable-buttons")[0].id = "dttbl";
    }
    catch { }
    try {
        $("#datatable-buttons1")[0].id = "dttbl1";
    }
    catch { }
    try {
        $("#datatable-buttons2")[0].id = "dttbl2";
    }
    catch { }
    try {
        $("#datatable-buttons4")[0].id = "dttbl4";
    }
    catch { }
}
function GetBudgerAnalysisReport() {
    debugger;
    if ($('#ddlFYList').val() == "0") {
        /*Code start by Hina sharma on 19-11-2024*/
        $("#valid_FinYear").text($("#valueReq").text());
        $("#valid_FinYear").css("display", "block");
        $("[aria-labelledby='select2-ddlFYList-container']").css("border-color", "red");
        //hideLoader();/*Add by Hina sharma on 19-11-2024 */
        /*Code End by Hina sharma on 19-11-2024*/
        return false;
    }
    else {
        $("#valid_FinYear").css("display", "none");
        $("[aria-labelledby='select2-ddlFYList-container']").css("border-color", "#ced4da");
        //showLoader();/*Add by Hina sharma on 19-11-2024 */
    }
    var action = $('#DdlShowAs').val();
    var glAccId = $('#ddlGlAccList').val();
    var finYear = $('#ddlFYList option:selected').text();
    var qtrName = $('#ddlQuarterList').val();
    var mnthName = $('#ddlMonthsList').val();
    ReplaceId();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetBudgerAnalysisReport",
        data: {
            action: action,
            glAccId: glAccId,
            finYear: finYear,
            quarter: qtrName,
            month: mnthName
        },
        success: function (data) {
            /*debugger;*/
            if (action == 'Y') {
                $("#BudgetAnalysisYearly").html(data);
            }
            else if (action == 'Q') {
                $("#BudgetAnalysisQuarterly").html(data);
            }
            else {
                $('#BudgetAnalysisMonthly').html(data);
            }
            //$("a.btn.btn-default.buttons-csv buttons-html5 btn-sm").remove();
            //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
            //hideLoader();/*Add by Hina sharma on 19-11-2024 */
            try {
                $("#dttbl")[0].id = "datatable-buttons";
                $("#dttbl1")[0].id = "datatable-buttons1";
                $("#dttbl2")[0].id = "datatable-buttons2";
                $("#dttbl4")[0].id = "datatable-buttons4";
            }
            catch {}
            /*alert(data);*/
        }

    })
}
function AccountRecevablCSV() {
    debugger;

    var arr = [];
    var action = $('#DdlShowAs').val();
    var glAccId = $('#ddlGlAccList').val();
    var finYear = $('#ddlFYList option:selected').text();
    var qtrName = $('#ddlQuarterList').val();
    var mnthName = $('#ddlMonthsList').val();

    var list = {};
    list.action = action
    list.glAccId = glAccId
    list.finYear = finYear
    list.qtrName = qtrName
    list.mnthName = mnthName

    arr.push(list);

    var array = JSON.stringify(arr);
    $("#hdnBudgetAnalysisData").val(array);

    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function OnChangeGlAcc() {

}
function GetMonthlyAllocation(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var accId = currentrow.find("#tdAccId").text();
    var qtr = currentrow.find("#tdQtr").text();
    var mnth = currentrow.find("#tdMnth").text();
    var valDigit = $('#ValDigit').text();
    var accName = currentrow.find("#tdAccName").text();
    var bgtAmt = currentrow.find("#tdBgtAmt").text();
    var actAmt = currentrow.find("#tdActAmt").text();
    var rev_no = currentrow.find("#tdRevNo").text();/*Add by Hina sharma on 08-10-2024 to remove duplicate data*/
    var finYear = $('#ddlFYList option:selected').text();
    
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetMonthlyAllocationBudget",
        data: {
            glAccId: accId,
            finYear: finYear,
            qtr: qtr,
            mnth: mnth,
            rev_no: rev_no
        },
        success: function (data) {
            /*debugger;*/
            $("#popupMonthlyAllocation").html(data);
            $('#GLAccount1').val(accName);
            var bgtAmt1 = cmn_ReplaceCommas(bgtAmt);
            var bgtAmt2 = parseFloat(bgtAmt1).toFixed(valDigit);
            $('#MonBudgetAmount1').val(cmn_addCommas(bgtAmt2));
            var actAmt1 = cmn_ReplaceCommas(actAmt);
            var actAmt2 = parseFloat(actAmt1).toFixed(valDigit);
            $('#ActualAmt1').val(cmn_addCommas(actAmt2));
            /*alert(data);*/
        }

    })
}
function GetQuarterlyAllocation(e) {
    var currentrow = $(e.target).closest("tr");
    var accId = currentrow.find("#tdAccId").text();
    var qtr = currentrow.find("#tdQtr").text();
    var mnth = currentrow.find("#tdMnth").text();

    var accName = currentrow.find("#tdAccName").text();
    var bgtAmt = currentrow.find("#tdBgtAmt").text();
    var actAmt = currentrow.find("#tdActAmt").text();
    var rev_no = currentrow.find("#tdRevNo").text();/*Add by Hina sharma on 08-10-2024 to remove duplicate data*/
    var finYear = $('#ddlFYList option:selected').text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetQuarterlyAllocationBudget",
        data: {
            glAccId: accId,
            finYear: finYear,
            qtr: qtr,
            mnth: mnth,
            rev_no: rev_no
        },
        success: function (data) {
            /*debugger;*/
            $("#PopupQuarterlyAllocation").html(data);
            $('#GLAccount2').val(accName);
            $('#MonBudgetAmount2').val(bgtAmt);
            $('#ActualAmount2').val(actAmt);
            /*alert(data);*/
        }

    })
}
function GetCostCenterAllocation(e) {
    debugger
    var currentrow = $(e.target).closest("tr");
    var accId = currentrow.find("#tdAccId").text();
    var accName = currentrow.find("#tdAccName").text();
    var bgtAmt = currentrow.find("#tdBgtAmt").text();
    var rev_no = currentrow.find("#tdRevNo").text();/*Add by Hina sharma on 08-10-2024 to remove duplicate data*/
    var finYear = $('#ddlFYList option:selected').text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetCostCenterAllocationBudget",
        data: {
            glAccId: accId,
            finYear: finYear,
            rev_no: rev_no
        },
        success: function (data) {
            /*debugger;*/
            $("#popupCostCenterAllocation").html(data);
            $('#CC_GLAccount1').val(accName);
            $('#CC_GLAmount1').val(bgtAmt);
            /*alert(data);*/
        }

    })
}
function validateFY() {
    debugger;
    var finYear = $('#ddlFYList').val();
    if (finYear == "0") {
        $("#valid_FinYear").text($("#valueReq").text());
        $("#valid_FinYear").css("display", "block");
        $("[aria-labelledby='select2-ddlFYList-container']").css("border-color", "red");
        //$('#ddlFYList').css('border', 'red');
        //$('#reqFY').css('display', 'block');
        //$('#reqFY').html($("#valueReq").text());
        return false;
    }
    else {
        //$('#ddlFYList').css('border', '#444');
        //$('#reqFY').css('display', 'none');
        $("#valid_FinYear").css("display", "none");
        $("[aria-labelledby='select2-ddlFYList-container']").css("border-color", "#ced4da");
        return true;
    }
}
function OnChangeShowHide() {
    var showAs = $('#DdlShowAs').val();
    if (showAs == "Y") {
        $('divQuarter').css('display', 'none');
        $('divMonth').css('display', 'none');
    }
    else if (showAs == "Q") {
        $('divQuarter').css('display', 'block');
        $('divMonth').css('display', 'none');
    }
    else {
        $('divQuarter').css('display', 'block');
        $('divMonth').css('display', 'block');
    }
}
function GetLedgerDetails(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var accId = currentrow.find("#tdAccId").text();
    var accName = currentrow.find("#tdAccName").text();
    if (accName.length < 1) {
        accName = $('#GLAccount1').val();
    }
    var fromDate = currentrow.find("#tdFromDate").text();
    var toDate = currentrow.find("#tdToDate").text();
    var actAmt = currentrow.find('#tdActAmt').text();
    $('#Amt1').val("");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetFYLedgerReport",
        data: {
            accId: accId,
            fromDate: fromDate,
            toDate: toDate
        },
        success: function (data) {
            /*debugger;*/
            $("#popupExpenseDetail").html(data);
            $('#AccName1').val(accName);
            var actAmt1 = cmn_ReplaceCommas(actAmt);
            var actAmt2 = parseFloat(actAmt1).toFixed(2)
            $('#Amt1').val(cmn_addCommas(actAmt2));
        }
    });
}
function GetLedgerDetails1(e) {/*Add by Hina sharma on 10-10-2024*/
    debugger;
    var currentrow = $(e.target).closest("tr");
    var accId = currentrow.find("#tdAccId").text();
    var CCtypeId = $("#CCtype_NameID").text();
    var CCtype = $("#CCtype_Name").text();
    var CCNameId = currentrow.find("#CC_NameId").text();
    var CCName = currentrow.find("#CC_Name").text();
   /* var accName = currentrow.find("#CC_GLAccount1").val();*/
    //if (accName.length < 1) {
    //    accName = $('#GLAccount1').val();
    //}
    var accName = $("#CC_GLAccount1").val();
    var fromDate = currentrow.find("#tdFromDate").text();
    var toDate = currentrow.find("#tdToDate").text();
    var actAmt = currentrow.find('#CC_Actual_Amount').text();
    $('#Amt1').val("");
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BudgetAnalysis/GetFYLedgerReportCostCenter",
        data: {
            accId: accId,
            fromDate: fromDate,
            toDate: toDate,
            CCtypeId: CCtypeId,
            CCNameId: CCNameId
        },
        success: function (data) {
            /*debugger;*/
            $("#popupExpenseDetail").html(data);
            $('#AccName1').val(accName);
            var actAmt1 = cmn_ReplaceCommas(actAmt);
            var actAmt2 = parseFloat(actAmt1).toFixed(2)
            $('#Amt1').val(cmn_addCommas(actAmt2));
            $('#txt_CCType').val(CCtype);
            $('#txt_CCName').val(CCName);
        }
    });
}
