$(document).ready(function () {
    $("#ddlGlacclist").select2();
    BindAccList();

    var finYear = $("#ddlFinyrList option:selected").text();
    var rev = $("#FinRevisionNumber").val();
    $("#hdDoc_No").val(finYear + '_' + rev);

    $(document).ready(function () {
        debugger;
       /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
        $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="FinanceBudgetCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    });
})
var ValDigit = $("#ValDigit").text();
function FinanceBudgetCSV() {
    debugger;
    var FB_Yr = $("#ddlFinyrList option:selected").text();
    var FB_Per = $("#ddlFinyrList").val();
    var Rev_No = $("#FinRevisionNumber").val();
    $("#HdnCsvPrint").val("CsvPrint");
    $('form').submit();
}
function OnchangeFinYear() {
    debugger;
    $("#Spn_FinYear").css("display", "none");
    $("#ddlFinyrList").css("border-color", "#ced4da");
    var FinYrList = $("#ddlFinyrList").val().replace(' to ', ',');
    var fyrange = $("#ddlFinyrList").val();

    var fy = FinYrList.split(",");
    var fy1 = fy[0].split("-");
    var fy2 = fy[1].split("-");

    var fin_sfy = (fy1[2] + "-" + fy1[1] + "-" + fy1[0]);
    var fin_efy = (fy2[2] + "-" + fy2[1] + "-" + fy2[0]);

    $("#hdn_sfy").val(fin_sfy);
    $("#hdn_efy").val(fin_efy);

    $("#txtPeriod").val(fyrange);
    $("#ddlGlacclist").prop('disabled', false);
    $("#FinBudgetAmount").prop('disabled', false);
    $("#txtRemarks").prop('disabled', false);
    $("#ddlFinyrList").prop('disabled', true);
}
function onkeypressBudgetAmount(el, evt) {
    $("#Spn_BudAmt").css("display", "none");
    $("#FinBudgetAmount").css("border-color", "#ced4da");
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}
function onkeyupRemarks() {
    debugger;
    $("#Spn_Remarks").css("display", "none");
    $("#txtRemarks").css("border-color", "#ced4da");
}
function onchangeBudgetAmount(){
    debugger;
    var BudAmount1 = $("#FinBudgetAmount").val();
    var BudAmount = cmn_ReplaceCommas(BudAmount1);
  //  var Amt = parseFloat(BudAmount).toFixed(ValDigit);
    var Amt = toDecimal(cmn_ReplaceCommas(BudAmount), ValDigit);
    if (CheckNullNumber(BudAmount) == "0") {
        $("#FinBudgetAmount").val("");
    }
    else {
        $("#FinBudgetAmount").val(cmn_addCommas(Amt));
    }
    $("#tblhdn tbody tr").remove();
    onclickAllocationBtn('', '', event);
    $("#BtnMonthlyAllocation").attr("disabled", false);
    $("#BtnQuaterlyAllocation").attr("disabled", false);
    $("#BtnCostCenterDetail").attr("disabled", false);
}

//-- Monthly Quaterly Allocation button---//
function onclickAllocationBtn(flag , flag1,e) {
    debugger;
    $("#PeriodFlag").val(flag);
    var budAmount;
    var finYear = $("#ddlFinyrList").val().replace(' to ',',');
    var Acc_Id;
    var Finyear;
    var NewArr = new Array();
    if (flag1 != "" && flag1 == "Ey") {
            var CurrentRow = $(e.target).closest('tr');
            Acc_Id = CurrentRow.find("#hdnGlAccountId").val();
            budAmount = CurrentRow.find("#FinBudAmount").text();
            Finyear = CurrentRow.find("#hdnFinyear").val();
    }
    else {
        Acc_Id = $("#ddlGlacclist option:selected").val();
        budAmount = $("#FinBudgetAmount").val();
        Finyear = $("#ddlFinyrList option:selected").val();
    }

    if ((flag == 'M' || flag == 'Q') && (flag1 != "Ey") && (Acc_Id != "" && Acc_Id != "" && budAmount != null && budAmount != "")) {
        $("#tblhdn tbody tr").each(function () {
            debugger;
            var CurrRow = $(this);
            var Lst = {};
            Lst.glacc = CurrRow.find("#hdn_GlId").text();
            Lst.type = CurrRow.find("#hdn_type").text();
            Lst.monname = CurrRow.find('#hdn_Month').text();
            Lst.period = CurrRow.find('#hdn_Period').text();
            var amount = CurrRow.find('#hdn_BudAmt').text();
            var amt = cmn_addCommas(amount);
            Lst.amount = amt;
            Lst.total = CurrRow.find('#hdn_TotalAmt').text();
            Lst.Sqno = CurrRow.find('#hdn_Sqno').text();
            Lst.qtr_sno = CurrRow.find('#hdn_qtr_sno').text();
            NewArr.push(Lst);
        })
    }
    if (flag1 != "" && flag1 == "Ey") {
        var GlRow = $("#tblhdnAlloc tbody  tr #hdn_GlId:contains(" + Acc_Id + ")").closest('tr');
        var TypeRow = GlRow.find("#hdn_type:contains('" + flag + "')").closest('tr');
        TypeRow.each(function () {
            var row = $(this);
            var List = {};
            List.glacc = row.find("#hdn_GlId").text();
            List.type = row.find("#hdn_type").text();
            List.monname = row.find('#hdn_Month').text();
            List.period = row.find('#hdn_Period').text();
            List.amount = row.find('#hdn_BudAmt').text();
            List.total = row.find('#hdn_TotalAmt').text();
            List.Sqno = row.find('#hdn_Sqno').text();
            List.qtr_sno = row.find('#hdn_qtrSno').text();
            NewArr.push(List);
        })
    }

    $('#tblMonthlyAlloc tbody tr').remove();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/FinanceBudget/GetMonQuatAllocation",
        data: {
            BudAmount: budAmount,
            FinYear: finYear,
            Flag : flag, 
            Acc_Id: Acc_Id,
            DisFlag: flag1,
            Bud_RowData: JSON.stringify(NewArr),
        },
        success: function (data) {
            debugger;
            $("#MonthlyAllocationPopUp").html(data);
            $("#MonFinancialYear").val($("#ddlFinyrList option:selected").text());
            $("#MonBudgetAmount").val(budAmount);
        },
    })
}

function AddNew() {
    debugger;

    if (validateonclickAddBtn() == false) {
        return false;
    }
    onclkaddnew();
   
    var ddlGlAcc = $("#ddlGlacclist option:selected").text();
    var ddlGlAccID = $("#ddlGlacclist option:selected").val();
    var FinYear = $("#ddlFinyrList option:selected").val();
    var GLGroup = $("#GLGroup").val();
    
    var BudAmtAmt = $("#FinBudgetAmount").val();
    var Remarks = $("#txtRemarks").val();
    var onclickMonthAlloc = "onclick=onclickAllocationBtn('M','Ey',event)";
    var onclickQuatAlloc = "onclick=onclickAllocationBtn('Q','Ey',event)";
    var onclickConstCenter = "onclick=Onclick_CCbtn('type',event,'Y')";
    var onclickEditBtn = "onclick=OnclickEditBtn(this,event)";
    var Tb = $('#datatable-buttons').DataTable();
   
    Tb.row.add([
        `<div class='red center s_f_d'><i class='deleteIcon fa fa-trash'  id='delBtnIcon' onclick='onclickDeleteicon(this,event)' ></i></div>`,
        `<div class='bom_width_td'><i class="fa fa-edit" ${onclickEditBtn} aria-hidden="true" id="" title=""></i></div>`,
            `<div><span id="SpanRowId"></span></div>`,
            `${ddlGlAcc} 
            <input type='hidden' id='hdnGlAccountId' value="${ddlGlAccID}" />
            <input type='hidden' id='hdnGlAccName' value="${ddlGlAcc}" />
            <input type='hidden' id='hdnBudAmt' value="${BudAmtAmt}" />
            <input type='hidden' id='hdnRemarks' value="${Remarks}" />
            <input type ='hidden' id='hdnFinyear' value="${FinYear}"/>`,
            `<td>${GLGroup}</td>`,
             `<span id='FinBudAmount'>${BudAmtAmt}</span>`,
             `<div class="center">
             <button type="button" id="MonthlyAllocation" class="calculator" data-toggle="modal" data-target="#MonthlyAllocation" data-backdrop="static" data-keyboard="false">
             <i class="fa fa-eye" aria-hidden="true" ${onclickMonthAlloc} data-toggle="" title=""></i>
             </div>
             </td>`,
             `<div class="center">
             <button type="button" id="QuarterlyAllocation" class="calculator" data-toggle="modal" data-target="#MonthlyAllocation" data-backdrop="static" data-keyboard="false">
             <i class="fa fa-eye" aria-hidden="true" ${onclickQuatAlloc} data-toggle="" title=""></i>
             </button>
             </div>`,
             `<div class="center">
                 <button type="button" id="CostCenterAllocation" class="calculator" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false">
                <i class="fa fa-eye" aria-hidden="true" ${onclickConstCenter} data-toggle="" title=""></i> </button>
             </div>`,
        `<span id='spn_bgtrmk'>${Remarks}</span>`,
    ]).draw();
    var sno = 0;
    $("#datatable-buttons tbody tr").each(function (i, row) {
        debugger;
        sno = parseInt(sno) + 1
        $(this).closest('tr').find("#SpanRowId").text(sno);
    });
    DisableHeader();
    BindAccList();
}

function onclkaddnew() {
    debugger;
    var AccId = $("#ddlGlacclist option:selected").val();
    $("#tblhdnAlloc > tbody > tr > #hdn_GlId:contains('" + AccId + "')").closest('tr').remove();
    $("#tblhdn tbody tr").each(function () {
        var CurrRow = $(this);
        var Acc_Id = CurrRow.find("#hdn_GlId").text();
        var Flag = CurrRow.find("#hdn_type").text();
        var MonthName = CurrRow.find("#hdn_Month").text();
        var Period = CurrRow.find("#hdn_Period").text();
        var Amount = CurrRow.find("#hdn_BudAmt").text();
        var TotalAmt = CurrRow.find("#hdn_TotalAmt").text();
        var Sqno = CurrRow.find("#hdn_Sqno").text();
        var qtr_Sno = CurrRow.find("#hdn_qtr_sno").text();
        $("#tblhdnAlloc > tbody").append(`<tr>
                            <td id="hdn_GlId">${Acc_Id}</td>
                            <td id="hdn_type">${Flag}</td>
                            <td id="hdn_Month">${MonthName}</td>
                            <td id="hdn_Period">${Period}</td>
                            <td id="hdn_BudAmt">${Amount}</td>
                            <td id="hdn_TotalAmt">${TotalAmt}</td>
                            <td id="hdn_Sqno">${Sqno}</td>
                            <td id="hdn_qtrSno">${qtr_Sno}</td>
                        </tr>`)
    })
    $("#tblhdn tbody tr").remove();
}

function OnclickEditBtn(el,e) {
    debugger;
    var clickedrow = $(e.target).closest('tr');
    var GlAccId = clickedrow.find("#hdnGlAccountId").val();
    var GlAccName = clickedrow.find("#hdnGlAccName").val();
    var BudAmt = clickedrow.find("#hdnBudAmt").val();
    var Remarks = clickedrow.find("#hdnRemarks").val();

    var rowJavascript = el.parentNode.parentNode.parentNode.rowIndex;
    $("#hdnUpdatetbl").val(rowJavascript);
    $("#ddlGlacclist option").remove();
    $("#ddlGlacclist").append(`<option value="${GlAccId}">${GlAccName}</option>`);
    
    $("#FinBudgetAmount").val(BudAmt);
    $("#txtRemarks").val(Remarks);
    $("#ddlGlacclist").attr("disabled", true);
    $("#FBDivUpdate").css('display', 'block');
    $("#FBDivadd").css('display', 'none');

    $('#tblhdn > tbody > tr').remove()

    $("#tblhdnAlloc > tbody > tr > #hdn_GlId:contains('" + GlAccId + "')").closest('tr').each(function () {
        debugger;
        var currentRow = $(this);
        var Acc_Id = currentRow.find("#hdn_GlId").text();
        var Flag = currentRow.find("#hdn_type").text();
        var MonthName = currentRow.find("#hdn_Month").text();
        var Period = currentRow.find("#hdn_Period").text();
        var Amount = currentRow.find("#hdn_BudAmt").text();
        var TotalAmt = currentRow.find("#hdn_TotalAmt").text();
        var sqno = currentRow.find("#hdn_Sqno").text();
        var qtr_sno = currentRow.find("#hdn_qtrSno").text();

        $('#tblhdn > tbody').append(`<tr>
                            <td id="hdn_GlId">${Acc_Id}</td>
                            <td id="hdn_type">${Flag}</td>
                            <td id="hdn_Month">${MonthName}</td>
                            <td id="hdn_Period">${Period}</td>
                            <td id="hdn_BudAmt">${Amount}</td>
                            <td id="hdn_TotalAmt">${TotalAmt}</td>
                            <td id="hdn_Sqno">${sqno}</td>
                            <td id="hdn_qtr_sno">${qtr_sno}</td>
                        </tr>`)
    })

    $("#BtnMonthlyAllocation").prop('disabled', false);
    $("#BtnQuaterlyAllocation").prop('disabled', false);
    $("#BtnCostCenterDetail").prop('disabled', false);

    
}

function onclickUpdateBtn(el) {
    debugger;
    var BudAmt = cmn_ReplaceCommas($("#FinBudgetAmount").val());
    var Remarks = $("#txtRemarks").val();
    var Acc_Id = $("#ddlGlacclist option:selected").val();

    if (CC_Validate(Acc_Id, BudAmt) == false) {
        return false;
    }

    $("#datatable-buttons tbody tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var tblaccid = currentRow.find('#hdnGlAccountId').val();
        if (tblaccid == Acc_Id) {
            //var BudAmt1 = parseFloat(BudAmt).toFixed(ValDigit);
            var BudAmt1 = toDecimal(cmn_ReplaceCommas(BudAmt), ValDigit);
            var BudAmt2 = cmn_addCommas(BudAmt1);
            currentRow.find("#FinBudAmount").text(BudAmt2);
            currentRow.find("#hdnBudAmt").val(BudAmt2);
            currentRow.find("#spn_bgtrmk").text(Remarks);
            currentRow.find("#hdnRemarks").val(Remarks);
        }
    }); 
    $("#ddlGlacclist").attr("disabled", false);
    $("#FinBudgetAmount").val('');
    $("#txtRemarks").val('');
    $("#FBDivUpdate").css('display', 'none');
    $("#FBDivadd").css('display', 'block');
    BindAccList();
    onclkaddnew();
}

function BindAccList() {
    debugger;
    //var Flag = "";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/FinanceBudget/GlAccountList",
        data: {
            /*flag: Flag,*/
        },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);
            if (arr.length > 0) {
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.length; i++) {
                    var len = $("#datatable-buttons tbody tr #hdnGlAccountId[value=" + arr[i].acc_id + "] ").length;
                    if (len > 0) {
                    }
                    else {
                       s += '<option value="' + arr[i].acc_id + '">' + arr[i].acc_name + '</option>';
                    }
                }
                $("#ddlGlacclist").html(s);
            }
        }

    })
}

function onchangeGlaccount() {
    debugger;
    $("#Spn_GlaccList").css("display", "none");
    $('[aria-labelledby="select2-ddlGlacclist-container"]').css("border-color", "#ced4da");
    $("#FinBudgetAmount").val('');
    var Acc_ID = $("#ddlGlacclist option:selected").val();
    Cmn_BindGroup(null, Acc_ID, "");
}

function onclickBtnSaveExit(Flag, Acc_Id, TblId) {
    debugger;
    if (ValidateAmount(Flag, Acc_Id, TblId) == false) {
        return false;
    }
    //var TotalAmt = parseFloat(CalculateAmt()).toFixed(ValDigit);
    var TotalAmt = toDecimal(CalculateAmt(), ValDigit);
    $("#tblhdn > tbody tr #hdn_type:contains(" + Flag + ")").closest('tr').remove();
   
    $('#tblMonthlyAlloc tbody tr').each(function () {
        debugger;
        var currentRow = $(this);
        var MonthName = currentRow.find('#Mon_month').text();
        var Period = currentRow.find('#Mon_period').text();
        var Amount = currentRow.find('#Mon_Amt').val();
        var Sqno = currentRow.find('#Sqno').text();
        var qtr_sno = currentRow.find('#hdnqtr_sno').val();
       
        $("#tblhdn > tbody").append(`<tr>
                            <td id="hdn_GlId">${Acc_Id}</td>
                            <td id="hdn_type">${Flag}</td>
                            <td id="hdn_Month">${MonthName}</td>
                            <td id="hdn_Period">${Period}</td>
                            <td id="hdn_BudAmt">${Amount}</td>
                            <td id="hdn_Sqno">${Sqno}</td>
                            <td id="hdn_qtr_sno">${qtr_sno}</td>
                        </tr>`)

        $("#tblhdn tbody tr #hdn_type:contains(Q)").closest('tr').each(function () {
            //debugger;
            var qcurrRow = $(this);
            var qtrflag = qcurrRow.find('#hdn_qtr_sno').text().trim();
            var Qtramt = 0;

            $("#tblhdn tbody tr #hdn_type:contains(M)").closest('tr').each(function () {
                //debugger;
                var currRow = $(this);
                var mAmount = currRow.find('#hdn_BudAmt').text();
                var mtrflag = currRow.find('#hdn_qtr_sno').text().trim();

                if (qtrflag == mtrflag && qtr_sno == mtrflag) {
                    //var mAmount1 = mAmount.replace(",", "");
                    var mAmount1 = cmn_ReplaceCommas(mAmount);
                    Qtramt = parseFloat(Qtramt) + parseFloat(mAmount1);
                }
            })
            if (qtr_sno == qtrflag) {
                //qcurrRow.find('#hdn_BudAmt').text(parseFloat(Qtramt).toFixed(ValDigit));
                qcurrRow.find('#hdn_BudAmt').text(toDecimal(Qtramt, ValDigit));
            }
        })
    })
}

function ValidateAmount() {
    debugger;
    ErrorFlag = 'Y';
    var BudAmount = cmn_ReplaceCommas($("#FinBudgetAmount").val());
    var Total = cmn_ReplaceCommas($('#Mon_TotalAmt').text());
    if ((parseFloat(BudAmount) != parseFloat(Total)) || (parseFloat(BudAmount) > parseFloat(Total))) {
        $('#Mon_SaveExitBtn').attr("data-dismiss", "");
        swal("", $("#TotalMonthlyAmtVsyearlyAmt").text(), "warning");
        ErrorFlag = 'N';
    }
    else {
        $('#Mon_SaveExitBtn').attr("data-dismiss", "modal");
    }
    if (ErrorFlag == 'Y') {
        return true;
    }
    else {
        return false;
    }
}

function DisableHeader() {
    debugger;
    $("#ddlFinyrList").attr("disabled", true);
    $("#txtRemarks").val("");
    $("#FinBudgetAmount").val("");

}

function validateonclickAddBtn() {
    debugger;
    Flag = 'Y';
    var GlAccList = $("#ddlGlacclist option:selected").val();
    var BudAmt = $("#FinBudgetAmount").val();
    var FinYear = $("#ddlFinyrList option:selected").val();
    if (CheckNullNumber(FinYear) == "0" || FinYear == null || FinYear == "") {
        $('#Spn_FinYear').text($("#valueReq").text());
        $("#Spn_FinYear").css("display", "block");
        $("#ddlFinyrList").css("border-color", "red");
        Flag = 'N';
    }
    else {
        $("#Spn_FinYear").css("display", "none");
        $("#ddlFinyrList").css("border-color", "#ced4da");
    }
    if (CheckNullNumber(GlAccList) == "0" || GlAccList == null || GlAccList == "") {
        $('#Spn_GlaccList').text($("#valueReq").text());
        $("#Spn_GlaccList").css("display", "block");
        $('[aria-labelledby="select2-ddlGlacclist-container"]').css("border-color", "red");
        Flag = 'N';
    }
    else {
        $("#Spn_GlaccList").css("display", "none");
        $('[aria-labelledby="select2-ddlGlacclist-container"]').css("border-color", "#ced4da");
    }
    if (CheckNullNumber(BudAmt) == "0" || BudAmt == null || BudAmt == "") {
        $('#Spn_BudAmt').text($("#valueReq").text());
        $("#Spn_BudAmt").css("display", "block");
        $("#FinBudgetAmount").css("border-color", "red");
        Flag = 'N';
    }
    else {
        $("#Spn_BudAmt").css("display", "none");
        $("#FinBudgetAmount").css("border-color", "#ced4da");
    }
    if (CC_Validate(GlAccList, cmn_ReplaceCommas(BudAmt)) == false) {
        return false;
    }
    if (Flag == 'Y') {
        return true;
    }
    else {
        return false;
    }
}

function updateSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });

};

function onchangePARBudgetAmt(e,tbl,period) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var Amt = CurrentRow.find('#Mon_Amt').val();
    //var Amt3 = Amt.replace(",", "");
    var Amt3 = cmn_ReplaceCommas(Amt);
    if (Amt3 != null && Amt3 != "") {
       // var Amt1 = parseFloat(Amt3).toFixed(ValDigit)
        var Amt1 = toDecimal(cmn_ReplaceCommas(Amt3), ValDigit);
        var Amt2 = cmn_addCommas(Amt1);
        CurrentRow.find('#Mon_Amt').val(Amt2);
    }
    else {
        CurrentRow.find('#Mon_Amt').val(parseFloat(0).toFixed(ValDigit));
    }
   
    var TotalAmt = CalculateAmt();
    var TotalAmt3 = Math.round(parseFloat(TotalAmt));
   // var TotalAmt1 = parseFloat(TotalAmt3).toFixed(ValDigit)
    var TotalAmt1 = toDecimal(cmn_ReplaceCommas(TotalAmt3), ValDigit);
    var TotalAmt2 = cmn_addCommas(TotalAmt1);
    $('#Mon_TotalAmt').html(TotalAmt2);
}
function CalculateAmt() {
    debugger;
    var TotalAmt = 0;
    $('#tblMonthlyAlloc tbody tr').each(function () {
        var CurrentRow = $(this);
        var Amt = CurrentRow.find('#Mon_Amt').val();
        //var Amt1 = Amt.replace(",", "");
        var Amt1 = cmn_ReplaceCommas(Amt);
        TotalAmt = parseFloat(Amt1) + parseFloat(TotalAmt);
    })
    return TotalAmt;
}

function ValidateOnkeypressAmt(e,tbl, period) {
    debugger;
    var currRow = $(e.target).closest('tr');
    var BudAmt = $("#FinBudgetAmount").val();
    //var BudAmt1 = BudAmt.replace(",", "");
    var BudAmt1 = cmn_ReplaceCommas(BudAmt);
    var Amt = parseFloat(CalculateAmt(tbl, period));
    if (parseFloat(Amt) > parseFloat(BudAmt1)) {
        return false;
    }
    else {
        return true;
    }
}
function onclickDeleteicon(el, e) {
    debugger;
    var clickedrow = $(e.target).closest('tr');
    var Acc_Id = clickedrow.find("#hdnGlAccountId").val();
    $("#tblhdnAlloc > tbody > tr > #hdn_GlId:contains('" + Acc_Id + "')").closest('tr').remove();
    var i = el.parentNode.parentNode.parentNode.rowIndex - 1;
    var table = $('#datatable-buttons').DataTable();
    table.row(i).remove().draw(false);

    $("#tbladdhdn > tbody > tr > #hdntbl_GlAccountId:contains('" + Acc_Id + "')").closest('tr').remove();//add by sm 04-12-2024(Ticket Number -'93658')
    //$("#tbladdhdn > tbody > tr ").remove();commented by sm on 04-12-2024  

    $("#ddlGlacclist").prop("disabled", false);
    $("#FinBudgetAmount").val("");
    $("#txtRemarks").val("");
    $("#FBDivUpdate").css('display', 'none');
    $("#FBDivadd").css('display', 'block');
    updateSerialNumber();
    BindAccList();

}

//---Insert Finance Budget Details---------//
function insertFinanceBudDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    if (onclickSaveBtn() == false) {
        return false;
    }
    var FinYr = $("#ddlFinyrList option:selected").text();
    var period = $("#txtPeriod").val();
    $("#hdn_finyr").val(FinYr);
    $("#hdn_finyears").val(FinYr);
    $("#hdn_period").val(period);

    var FinalMonQuatDetail = [];
    FinalMonQuatDetail = InsertQuatMonDetails();
    var QuatMon = JSON.stringify(FinalMonQuatDetail);
    $('#hdn_QuatMonList').val(QuatMon);

    var FinalAccountDetails = [];
    FinalAccountDetails = InsertAccountDetails();
    var AccDetails = JSON.stringify(FinalAccountDetails);
    $('#hdn_Gldetails').val(AccDetails);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
}
function InsertQuatMonDetails() {
    debugger;
    var QtrmnthDetail = new Array();
    $("#datatable-buttons > tbody > tr").each(function () {
        debugger;
        var CurrRow = $(this);
        var AccID = CurrRow.find("#hdnGlAccountId").val();
        $("#tblhdnAlloc > tbody > tr > #hdn_GlId:contains('" + AccID+"')").closest('tr').each(function () {
            
            var currentRow = $(this);
            var List = {};
            List.acc_id = currentRow.find("#hdn_GlId").text();
            List.bud_type = currentRow.find("#hdn_type").text();
            List.mnth_qtr_name = currentRow.find("#hdn_Month").text();
            List.bgt_amt = cmn_ReplaceCommas(currentRow.find("#hdn_BudAmt").text());
            List.sqno = currentRow.find("#hdn_Sqno").text();
            QtrmnthDetail.push(List);
        });
    })
    
    return QtrmnthDetail;
};
function InsertAccountDetails() {
    debugger;
    var AccDetails = new Array();
    $("#datatable-buttons TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var List = {};
        List.acc_id = currentRow.find("#hdnGlAccountId").val();
        List.bud_amount = cmn_ReplaceCommas(currentRow.find("#hdnBudAmt").val());
        List.remarks = currentRow.find("#hdnRemarks").val();
        AccDetails.push(List);
    });
    return AccDetails;
};

function onclickSaveBtn() {
    debugger;
    var Lenght = $("#datatable-buttons > tbody tr>td").length;
    var Prop = $('#ddlFinyrList').prop('disabled');
    if (Lenght == 1) {
        if (HeaderValidation() == false) {
           return false;
        }
        if (Prop == true) {
            if (DatatTableGlAccValidate() == false) {
                return false;
            }
            else {
                return true;
            }
        }
    }
   
}

function HeaderValidation() {
    debugger;
    var errorflag = 'Y';
    var Prop = $('#ddlFinyrList').prop('disabled');
    var ddlGlList = $("#ddlGlacclist option:selected").val();
    var Budamount = $("#FinBudgetAmount").val();
    if (Prop == false) {
        $('#Spn_FinYear').text($("#valueReq").text());
        $("#Spn_FinYear").css("display", "block");
        $("#ddlFinyrList").css("border-color", "red");
        errorflag = 'N'
    }
    if (Prop != false) {
        if (CheckNullNumber(ddlGlList) == 0) {
            $('#Spn_GlaccList').text($("#valueReq").text());
            $("#Spn_GlaccList").css("display", "block");
            $('[aria-labelledby="select2-ddlGlacclist-container"]').css("border-color", "red");
            errorflag = 'N'
        }
        else {
            $("#Spn_GlaccList").css("display", "none");
            $('[aria-labelledby="select2-ddlGlacclist-container"]').css("border-color", "#ced4da");
        }
        if (CheckNullNumber(Budamount) == "0" || Budamount == null || Budamount == "") {
            $('#Spn_BudAmt').text($("#valueReq").text());
            $("#Spn_BudAmt").css("display", "block");
            $("#FinBudgetAmount").css("border-color", "red");
            errorflag = 'N';
        }
        else {
            $("#Spn_BudAmt").css("display", "none");
            $("#FinBudgetAmount").css("border-color", "#ced4da");
        }
    }
    if (errorflag == 'N') {
        return false
    }
    else {
        return true;
    }
}

//---Header Validation----//
function DatatTableGlAccValidate() {
    debugger;
    Flag = 'Y'
    var Lenght = $("#datatable-buttons > tbody tr>td").length;
    var Prop = $('#ddlFinyrList').prop('disabled');
    var Period = $('#txtPeriod').val();
    if (Prop == true || Period == "" || Period == null || Lenght == 1) {
        swal("", $("#NoGLAccountSelected").text(), "warning");
        Flag = 'Y'
    }
    else {
       
    }
    
    if (Flag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}
//---Work FLow
function ForwardBtnClick() {
    debugger;
    try {
        var FinBudStatus = "";
        FinBudStatus = $('#hdn_Doc_Staus').val().trim();
        if (FinBudStatus === "D" || FinBudStatus === "F") {

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
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Years = $("#hdn_finyears").val();
    var Rev_No = $("#hdn_Revno").val();
    var FB_No = Years + '_' + Rev_No;
    debugger;
    if (FB_No != "" && FB_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(FB_No, Doc_ID);
    return false;
}
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
     var FB_No = "";
    var FB_Dt = "";
    var Period = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";

    Remarks = $("#fw_remarks").val();
    var Years = $("#hdn_finyears").val();
    var Rev_No = $("#hdn_Revno").val();
    FB_No = Years + '_' + Rev_No;
    FB_Dt = $("#hdn_CreateDt").val();
    Period = $("#txtPeriod").val();
    docid = $("#DocumentMenuId").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (FB_No + ',' + Period +',' + WF_Status1);
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval === "Forward") {
        if (fwchkval != "" && FB_No != "" && FB_Dt != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(FB_No, FB_Dt , docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/FinanceBudget/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/FinanceBudget/FinanceBudgetApprove?FY=" + Years + "&RevNo=" + Rev_No + "&Period=" + Period + "&FB_Date=" + FB_Dt + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && FB_No != "" && FB_Dt != "" ) {
             Cmn_InsertDocument_ForwardedDetail(FB_No, FB_Dt, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/FinanceBudget/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && FB_No != "" && FB_Dt != "") {
             Cmn_InsertDocument_ForwardedDetail(FB_No, FB_Dt,docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/FinanceBudget/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
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

//------Delete FB Detail-----------//
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
            $("#HdnCsvPrint").val(null);
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e,dflag) {
    debugger;
    
    var Amt;
    var TotalAmt1 = 0;//add by sm 04-12-2024
    var GLAcc_Name ;
    var GLAcc_id ;
    var Doc_ID = $("#DocumentMenuId").val();//add by sm 04-12-2024
    if (dflag != "Y") {
        Amt = $("#FinBudgetAmount").val();
        GLAcc_Name = $("#ddlGlacclist option:selected").text();
        GLAcc_id = $("#ddlGlacclist option:selected").val();
    }
    else {
        var clickedrow = $(e.target).closest("tr");
         Amt = clickedrow.find("#hdnBudAmt").val();
         GLAcc_Name = clickedrow.find("#hdnGlAccName").val();
         GLAcc_id = clickedrow.find("#hdnGlAccountId").val();
    }
    var disableflag = dflag;
    var NewArr = new Array();
    if (GLAcc_id != "0" && GLAcc_id != "" && GLAcc_id != null) {
        $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
            var row = $(this);
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            var amount = cmn_ReplaceCommas(row.find("#hdntbl_CstAmt").text());
           // var amt = parseFloat(amount).toFixed(ValDigit);
            var amt = toDecimal(cmn_ReplaceCommas(amount), ValDigit);
            TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amt);//add by sm 04-12-2024
            var amt1 = cmn_addCommas(amt);
            List.CC_Amount = amt1;
            NewArr.push(List);
        })
        var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 04-12-2024
    }
        
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/BankReceipt/GetCstCntrtype",
            data: {
                Flag: flag,
                Disableflag: disableflag,
                CC_rowdata: JSON.stringify(NewArr),
                TotalAmt: TotalAmt,//add by sm 04-12-2024
                Doc_ID: Doc_ID//add by sm 04-12-2024
            },
            success: function (data) {
                debugger;
                $("#CostCenterDetailPopup").html(data);
                $("#CC_GLAccount").val(GLAcc_Name);
                $("#hdnGLAccount_Id").val(GLAcc_id);
                $("#GLAmount").val(Amt);
                $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
                $("#hdnTable_Id").text("BPdetailTbl");
            },
        })

}
function CC_Validate(hfAccID, JvAmt) {//Added by Suraj Maurya on 10-12-2025 to validate Cost Center Parameter wise
    debugger;
    const CcMendate = $("#hdn_CostCenterDetailMandatory").text();
    if (CcMendate == "Y") {
        var CC_Error = false;
        var acc_id_start_no = hfAccID.toString().substring(0, 1);
        var checkCC = false;
        if (acc_id_start_no == "3" || acc_id_start_no == "4") {
            checkCC = true;
        }
        if (checkCC) {
            var arr = [];
            $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains('" + hfAccID + "')").closest('tr').each(function () {
                var currentRow = $(this);
                var gl_acc_id = currentRow.find("#hdntbl_GlAccountId").text();
                if (gl_acc_id == hfAccID) {
                    var ddl_Type_Id = currentRow.find("#hdntbl_CCtype_Id").text();
                    arr.push(ddl_Type_Id);
                }
            })
            var unique = arr.filter(function (itm, i, arr) {
                return i == arr.indexOf(itm);
            });
            var amt = 0;

            var GLRow = $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains('" + hfAccID + "')").closest('tr');
            for (let i = 0; i <= unique.length - 1; i++) {
                var Amt = 0;
                var GL_CCRow = GLRow.find("#hdntbl_CCtype_Id:contains('" + unique[i] + "')").closest('tr');
                GL_CCRow.each(function () {
                    var CurrRow = $(this);
                    var CC_Amt = cmn_ReplaceCommas(CurrRow.find("#hdntbl_CstAmt").text());
                    Amt = parseFloat(CC_Amt) + parseFloat(Amt);
                });
                var jvAmt = parseFloat(JvAmt).toFixed(cmn_ValDecDigit);
                amt = (parseFloat(amt) + parseFloat(Amt)).toFixed(cmn_ValDecDigit);
            }
            if (unique.length != 0) {
                if (parseFloat(jvAmt) != parseFloat(amt)) {
                    //ValidateEyeColor(CurrentRow, "BtnCostCenterDetail", "Y");
                    CC_Error = true;
                }
                else {
                    //ValidateEyeColor(CurrentRow, "BtnCostCenterDetail", "N");
                }
            }
            if (unique.length == 0) {
                //ValidateEyeColor(CurrentRow, "BtnCostCenterDetail", "Y");
                CC_Error = true;
            }
        }
        if (CC_Error) {
            swal("", $("#GlamtmismatchWithCCAmt").text(), "warning");
            $("#BtnCostCenterDetail").addClass("highlight_tr_red");
            return false;
        } else {
            return true;
        }
    }
    return true;
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