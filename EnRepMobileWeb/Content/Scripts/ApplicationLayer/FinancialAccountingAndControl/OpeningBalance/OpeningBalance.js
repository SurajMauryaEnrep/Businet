$(document).ready(function () {
    debugger;
    $("#coaid").select2();
    $("#sales_person").select2();
    $("#FinancialYear").val($("#hdFromdate").val());
    /*$("#OpBillDt").prop("disabled", true);*/
    $("#curr").prop("disabled", true);
    $("#OpeningListTble #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var FinYear = clickedrow.children("#fin_year").text();
            $("#Hdn_fin_year").val(FinYear);
            if (FinYear != null && FinYear != "") {
                window.location.href = "/ApplicationLayer/OpeningBalance/DBClickOpeningBalDt/?FinYear=" + FinYear;
            }
        }
        catch (err) {
            debugger
        }
    });

    $("#OpBaldetailTBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            sessionStorage.setItem("EditClick","N");
             //var currentRow = $(this).closest('tr')
            var clickedrow = $(e.target).closest("tr");
            var Acc_id = clickedrow.find("#hd_acc_id").val();
            var fin_year = clickedrow.find("#HDfin_year").text();               
            debugger;
            //BillWiseOpeningDetailHeader(Acc_id, fin_year);
           
            $.ajax({
                success: function () {
                    window.location.href = "/ApplicationLayer/OpeningBalance/EditOpeningBalDt/?Acc_id=" + Acc_id + "&Fin_year=" + fin_year;
                }
            });           
        }
        catch (err) {
        }
    });
    var EditS = sessionStorage.getItem("EditClick");
    if (EditS == "Y") {
        var acc_type = $("#acc_type").val();
        if (acc_type == 1 || acc_type == 2) {
            $("#OpBillDt").prop("disabled", false);
            $("#OpBillDt").css("filter", "")
        }
        else {
            $("#OpBillDt").prop("disabled", true);
            $("#OpBillDt").css("filter", "grayscale(100%)")
        }
    }

    var todaysDate = new Date(); // Gets today's date
    var year = todaysDate.getFullYear();                        // YYYY
    var month = ("0" + (todaysDate.getMonth() + 1)).slice(-2);  // MM
    var day = ("0" + todaysDate.getDate()).slice(-2);           // DD
    var maxDate = (year + "-" + month + "-" + day); // Results in "YYYY-MM-DD" for today's date 
    $('#BillVoucherDate').attr('max', maxDate);
    $(document).ready(function () {
        debugger;
        /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
        //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id=""
        //onclick = "OpeningBalanceCSV()"  tabindex = "0" aria - controls="datatable-buttons5" href = "#" > <span>CSV</span></button > ')
            $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" ' +'onclick="OpeningBalanceCSV()" tabindex="0" aria-controls="datatable-buttons5">' +'<span>CSV</span></button>');
    });
})
function OpeningBalanceCSV() {
    debugger;
   // var finYear = $("#Hdn_fin_year").val();
    $("#HdnCsvPrint").val("CsvPrint")
    //$("#Hdn_fin_year").val(finYear);
    var searchValue = $("#datatable-buttons_filter input[type=search]").val();
    $("#searchValue").val(searchValue);
    $('form').submit();
   //if (finYear != null && finYear != "") {
   // window.location.href = "/ApplicationLayer/OpeningBalance/OpeningBalExporttoExcelDt?finYear=" + finYear;
   // }
}
var DeleteText = $("#Span_Delete_Title").text();
var EditText = $("#Edit").text();
function RemoveSession() {
    sessionStorage.removeItem("OpeningDetailSession");
    sessionStorage.removeItem("EditClick");
}
function OnChangeAccType() {
    debugger;
    $("#AccountGroup").val("");
    $("#curr").val("0");
    $("#ConvRate").val("");
    $("#Opbalsp").val("");
    $("#Opbalbs").val("");
    var acc_type = $("#acc_type").val();
    //if (acc_type == "1") {
        //$("#op_bal_type").val("dr").trigger('change')
    //}
    //if (acc_type == "2") {
        //$("#op_bal_type").val("cr").trigger('change')
    //}
    var TransType = $("#TransType").val();
    if (acc_type == 1 || acc_type == 2) {
        $("#OpBillDt").prop("disabled", false);
        $("#OpBillDt").css("filter", "")
    }
    else {
        $("#OpBillDt").prop("disabled", true);
        $("#OpBillDt").css("filter", "grayscale(100%)")
    }
    if (acc_type == "" || acc_type == "0") {
        document.getElementById("vmAccounttype").innerHTML = $("#valueReq").text();
        $("#acc_type").css("border-color", "red");
        $("#vmAccounttype").css("display", "block");
    }
    else {
        $("#vmAccounttype").css("display", "none");
        $("#acc_type").css("border-color", "#ced4da");
    }
     $.ajax({
        type: "POST",
         url: "/ApplicationLayer/OpeningBalance/Getcoa1",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
         data: {
             acc_type: acc_type,
             TransType: TransType
         },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.length; i++) {
                    s += '<option value="' + arr[i].acc_id + '">' + arr[i].acc_name + '</option>';
                }
                $("#coaid").html(s);
              
            }
        },
        error: function (Data) {
        }
    });
}
function OnChangeCoa() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();
    var coa_id = $("#coaid").val();
     //-------Modifyed By Shubham Maurya on 06-03-2024 for balance Type Start--------//
    var coa_id1 = coa_id.split("",2);
    var coa_id2 = coa_id1[0] + '' + coa_id1[1];
    if (coa_id2 == "10") {
        $("#op_bal_type").val("dr").trigger('change')
    }
    if (coa_id2 == "20") {
        $("#op_bal_type").val("cr").trigger('change')
    }
     //-------Modifyed By Shubham Maurya on 06-03-2024 for balance Type End--------//
    if (coa_id == "" || coa_id == "0") {
        $('#vmcoa').text($("#valueReq").text());
        $("#vmcoa").css("display", "block");
        $("[aria-labelledby='select2-coaid-container']").css("border-color", "red");
    }
    else {
        $("#vmcoa").css("display", "none");
        $("[aria-labelledby='select2-coaid-container']").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");
    }
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/OpeningBalance/GetAccGroup",
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
                            $("#AccountGroup").val(arr.Table[0].AccGroupChildNood);                            
                            $("#hd_acc_grp_id").val(arr.Table[0].acc_grp_id);   
                        }
                        else {
                            $("#AccountGroup").val("");
                    }    
                    if (arr.Table1.length > 0) {
                        $("#curr").val(arr.Table1[0].curr_id);
                        $("#ConvRate").val(parseFloat(arr.Table1[0].conv_rate).toFixed(ExchDecDigit));
                    }
                    //else {
                    //    $("#curr").val(arr.Table2[0].curr_id);
                    //    $("#ConvRate").val(parseFloat(arr.Table2[0].conv_rate).toFixed(RateDecDigit));
                    //    $("#ConvRate").attr("readonly", true);
                    //}
                    if (arr.Table1.length > 0) {
                        if (arr.Table1[0].curr_id == arr.Table1[0].bs_curr_id) {
                            $("#ConvRate").attr("readonly", true);
                        }
                        else {
                            $("#ConvRate").attr("readonly", false);
                        }
                    }
                    else {
                        $("#ConvRate").attr("readonly", false);
                    }
                    OnChangeOpbal();            
                }              
            },
        });
}
function OnKeyupOpenBal() {

    
    var OpenBal =$("#Opbalsp").val();
    if (OpenBal != "") {
        $("#Opbalsp").css("border-color", "#ced4da");
        $("#OpbalspError").text("");
        $("#OpbalspError").css("display", "none");
    }
}
function OnChangeCurr() {
    debugger;
    var curr_id = $("#curr").val();  
    if (curr_id == "" || curr_id == "0") {
        document.getElementById("vmCurr").innerHTML = $("#valueReq").text();
        $("#vmCurr").css("display", "block");
        $("#curr").css("border-color", "red");
    }
    else {
        $("#vmCurr").css("display", "none");
        $("#curr").css("border-color", "#ced4da");
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
    }

    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/OpeningBalance/GetCurrConvRate",
            data: {
                curr_id: curr_id,
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
                        $("#ConvRate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                    }
                    else {
                        $("#ConvRate").val(parseFloat(0).toFixed(ExchDecDigit));
                    }

                    var OpBal = $("#Opbalsp").val();
                    var ConvRate = $("#ConvRate").val();

                    if ((OpBal !== 0 || OpBal !== null || OpBal !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
                        debugger;
                        var FAmt = OpBal * ConvRate;
                        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
                        $("#Opbalbs").val(FinVal);

                    }
                }
            },
        });
}
function OnChangeOpbal(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();  
    var Opbalsp1 = $("#Opbalsp").val();
    var Opbalsp = cmn_ReplaceCommas(Opbalsp1);
    if (Opbalsp == "" || Opbalsp == null || parseFloat(Opbalsp) == parseFloat("0")) {
        $("#OpbalspError").text($("#valueReq").text());
        $("#OpbalspError").css("display", "block");
        $("#Opbalsp").css("border-color", "red");
        $("#Opbalsp").val("");
    }
    else {
        var opbl = parseFloat(Opbalsp).toFixed(ValDecDigit)
        var opbl1 = cmn_addCommas(opbl);
        $("#Opbalsp").val(opbl1);
        $("#OpbalspError").css("display", "none");
        $("#Opbalsp").css("border-color", "#ced4da");
    }

    var OpBal = $("#Opbalsp").val();
    var ConvRate = $("#ConvRate").val();

    //if ((OpBal !== 0 || OpBal !== null || OpBal !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
    if ((OpBal !== 0 && OpBal !== null && OpBal !== "") && (ConvRate !== 0 && ConvRate !== null && ConvRate !== "")) {
        debugger;
        var opbl2 = cmn_ReplaceCommas(OpBal);
        var ConvRate1 = cmn_ReplaceCommas(ConvRate);
        var FAmt = parseFloat(opbl2) * parseFloat(ConvRate1);
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = cmn_addCommas(FinVal)
        $("#Opbalbs").val(FinVal1);

    }
}
function OnChangeConvRate(e) {
    debugger;
    
    var ValDecDigit = $("#ValDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();
    var ConvRate1 = $("#ConvRate").val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);
    if (ConvRate == "" || ConvRate == null || parseFloat(ConvRate) == parseFloat("0")) {
        $("#ConvRateError").text($("#valueReq").text());
        $("#ConvRateError").css("display", "block");
        $("#ConvRate").css("border-color", "red");
        $("#ConvRate").val("");
    }
    else {
        var Crate = parseFloat(ConvRate).toFixed(ExchDecDigit);
        var Crate1 = cmn_addCommas(Crate);
        $("#ConvRate").val(Crate1);
        $("#ConvRateError").css("display", "none");
        $("#ConvRate").css("border-color", "#ced4da");
    }
    var ConvRate = $("#ConvRate").val();
    var OpBal = $("#Opbalsp").val();

    if ((OpBal !== 0 || OpBal !== null || OpBal !== "") && (ConvRate !== 0 || ConvRate !== null || ConvRate !== "")) {
        debugger;
        var ConvRate2 = cmn_ReplaceCommas(ConvRate);
        var OpBal2 = cmn_ReplaceCommas(OpBal);
        var FAmt = parseFloat(OpBal2) * parseFloat(ConvRate2);
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        var FinVal1 = cmn_addCommas(FinVal);
        $("#Opbalbs").val(FinVal1);

    }
}
function RateFloatValueonly(el, evt) {    
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    return true;
}
function RateFloatValueonly_exch(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    return true;
}
function EditBtnClick() {
    sessionStorage.removeItem("EditClick");
    sessionStorage.setItem("EditClick", "Y");
    var acc_type = $("#acc_type").val();
    if (acc_type == 1 || acc_type == 2) {
        $("#OpBillDt").prop("disabled", false);
        $("#OpBillDt").css("filter", "")
    }
    else {
        $("#OpBillDt").prop("disabled", true);
        $("#OpBillDt").css("filter", "grayscale(100%)")
    }
   
}

//---------------Bill Detail Pop Up Start-----------------------//
function OnClickOpBalQtyIconBtn(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    $('#divAdd').css('display', 'block');
    $('#divUpdate').css('display', 'none');
    var coa_id = $("#coaid").val();
    var coa_name = $("#coaid option:selected").text();
    var op_bal_type = $("#op_bal_type option:selected").text();
    var ConvRate = $("#ConvRate").val();
    var Opbalsp = $("#Opbalsp").val();
    var Opbalbs = $("#Opbalbs").val();
    var curr_id = $("#curr option:selected").text();
    var accType = $('#acc_type').val();


    ResetVoucherDetailVal();
    EnableVoucherDetail();

    if (accType == "1") {
        $("#DviSlsPerson").css("display", "");
    }
    else {
        $("#DviSlsPerson").css("display", "none");
    }

    $("#IconAccountName").val(coa_name);
    $("#IconOpBalsp").val(Opbalsp);
    $("#IconOpBalbs").val(Opbalbs);
    $("#IconCurr").val(curr_id);
    $("#IconBalanceType").val(op_bal_type);
    $("#IconConvRate").val(ConvRate);
    //-------Modifyed By Shubham Maurya on 06-03-2024 for balance Type Start--------//
    var coa_id1 = coa_id.split("", 2);
    var coa_id2 = coa_id1[0] + '' + coa_id1[1];
    if (coa_id2 == "10") {
        $("#billVoucType").val("dr").trigger('change')
    }
    if (coa_id2 == "20") {
        $("#billVoucType").val("cr").trigger('change')
    }
    //-------Modifyed By Shubham Maurya on 06-03-2024 for balance Type End--------//
    var rowIdx = 0;
    //var FOpeningBalDetails = JSON.parse(sessionStorage.getItem("OpeningDetailSession"));
    var FOpeningBalDetails = [];
    if (("#HdnOpBilldetailTable >tbody >tr").length > 0) {
        //$("#OpBilldetailTable >tbody >tr").remove();
        $("#HdnOpBilldetailTable >tbody >tr").each(function () {
            var currentRow = $(this);
            debugger;
           
            var VoucherNo = currentRow.find("#VoucherNo").text();
            var VoucherDt = currentRow.find("#VoucherDt").text();           
            debugger;
            var VoucherAmt = cmn_ReplaceCommas(currentRow.find("#VoucherAmt").text());
            var VoucherType = currentRow.find("#VoucherType").text();
            var sales_person = currentRow.find("#hdn_txt_sales_person").text();
            var sales_person_id = currentRow.find("#hdn_txt_sales_person_id").text();

            FOpeningBalDetails.push({ VoucherNo: VoucherNo, VoucherDt: VoucherDt, VoucherAmt: VoucherAmt, VoucherType: VoucherType, sales_person_id: sales_person_id, sales_person: sales_person })
        });
    }
    if (FOpeningBalDetails.length == 0) {
        $("#OpBilldetailTable >tbody >tr").remove();
    }
    if (FOpeningBalDetails != null) {
        if (FOpeningBalDetails.length > 0) {
            var disableDeletebtn = $("#disDELBtn").val();
            if (disableDeletebtn == "Disable") {
                $("#OpBilldetailTable >tbody >tr").remove();
                //$('#BatchQtyTotal').text(parseFloat(0).toFixed(ValDecDigit));
                for (i = 0; i < FOpeningBalDetails.length; i++) {
                    var VoucherAmt1 = parseFloat(FOpeningBalDetails[i].VoucherAmt).toFixed(ValDecDigit);
                    $('#OpBilldetailTable tbody').append(`<tr id="R${++rowIdx}">
<td class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" id="" data-toggle="" title=""></i></td>
<td id="" class="red center"><i class="fa fa-trash deleteIcon" style="filter: grayscale(100%)" id="" aria-hidden="true" title=""></i></td>
<td id="OpBlId" >${rowIdx}</td>
<td id="VoucherNo" >${FOpeningBalDetails[i].VoucherNo}</td>
<td id="VoucherDt" >${FOpeningBalDetails[i].VoucherDt}</td>
<td id="VoucherAmt" class="num_right">${cmn_addCommas(VoucherAmt1)}</td>
<td id="VoucherType" >${FOpeningBalDetails[i].VoucherType}</td>
<td id="txt_sales_person">${FOpeningBalDetails[i].sales_person}</td>
<td id="txt_sales_person_id" hidden="hidden">${FOpeningBalDetails[i].sales_person_id}</td>
</tr>`);

                }
            }
            else {
                $("#OpBilldetailTable >tbody >tr").remove();
                //$('#BatchQtyTotal').text(parseFloat(0).toFixed(ValDecDigit));
                for (i = 0; i < FOpeningBalDetails.length; i++) {
                    var VoucherAmt1 = parseFloat(FOpeningBalDetails[i].VoucherAmt).toFixed(ValDecDigit);
                    $('#OpBilldetailTable tbody').append(`<tr id="R${++rowIdx}">
<td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></td>
<td id="" class="red center"> <i class="fa fa-trash deleteIcon" id="OpBalDeleteIcon" aria-hidden="true" title="${DeleteText}"></i></td>
<td id="OpBlId" >${rowIdx}</td>
<td id="VoucherNo" >${FOpeningBalDetails[i].VoucherNo}</td>
<td id="VoucherDt" >${FOpeningBalDetails[i].VoucherDt}</td>
<td id="VoucherAmt" class="num_right">${cmn_addCommas(VoucherAmt1)}</td>
<td id="VoucherType" >${FOpeningBalDetails[i].VoucherType}</td>
<td id="txt_sales_person">${FOpeningBalDetails[i].sales_person}</td>
<td id="txt_sales_person_id" hidden="hidden">${FOpeningBalDetails[i].sales_person_id}</td>
</tr>`);
                }
            }
        }
            ResetVoucherDetailVal();
            CalculateBillAmtTotal();
            OnClickOPDeleteIcon();
            OnClickEditIcon();
        }
    }


function AddVoucherDetail() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    //var ValDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();

    if ($('#BillVoucherNumber').val() == "") {
        ValidInfo = "Y";
        $("#BillVoucherNumber").css("border-color", "Red");
        $('#spanBillVouNo').text($("#valueReq").text());
        $("#spanBillVouNo").css("display", "block");
    }
    else {
        var BillVoucNo = $('#BillVoucherNumber').val().toUpperCase();
        $("#OpBilldetailTable >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblBillVoucNo = currentRow.find("#VoucherNo").text().toUpperCase();
            if (tblBillVoucNo == BillVoucNo) {
                $('#spanBillVouNo').text($("#valueduplicate").text());
                $("#spanBillVouNo").css("display", "block");
                $("#BillVoucherNumber").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
    }
    var Amt = cmn_ReplaceCommas($('#Amount').val());
    if (Amt == "0" || Amt == "") {
        ValidInfo = "Y";
        $("#Amount").css("border-color", "Red");
        $('#spanBillVouAmt').text($("#valueReq").text());
        $("#spanBillVouAmt").css("display", "block");
    }
    else {
        $("#spanBillVouAmt").css("display", "none");
        $("#Amount").css("border-color", "#ced4da");
    }
    var BillVouchDt = $("#BillVoucherDate").val();
    if (BillVouchDt == "" || BillVouchDt == null || BillVouchDt == "NaN") {
        $("#spanBillVouDt").text($("#valueReq").text());
        $("#spanBillVouDt").css("display", "block");
        $("#BillVoucherDate").css("border-color", "red");        
        ValidInfo = "Y";
    }
    else {
        $("#spanBillVouDt").css("display", "none");
        $("#BillVoucherDate").css("border-color", "#ced4da");
    }
    var balVouType = $('#billVoucType').val();
    if (balVouType == "dr") {
        var salesperson = $("#sales_person").val();
        if (salesperson == "" || salesperson == null || salesperson == "NaN" || salesperson == "0") {
            $("#spanSalePerson").text($("#valueReq").text());
            $("#spanSalePerson").css("display", "block");
            $("[aria-labelledby='select2-sales_person-container']").css("border-color", "red");
            ValidInfo = "Y";
        }
        else {
            $("#spanSalePerson").css("display", "none");
            $("[aria-labelledby='select2-sales_person-container']").css("border-color", "#ced4da");
        }
    }
    if (ValidInfo == "Y") {
        return false;
    }
    var sales_per = $("#sales_person option:selected").text();
    if (sales_per == "0" || sales_per == "---Select---") {
        sales_per = "";
    }
    var VoucherDt = BillVouchDt.split("-").reverse().join("-");
    var Amount = parseFloat(Amt).toFixed(ValDecDigit);
    var TblLen = $('#OpBilldetailTable tbody tr').length;
    $('#OpBilldetailTable tbody').append(`<tr id="R${++rowIdx}">
<td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></td>
<td class="red center"> <i class="fa fa-trash deleteIcon" id="OpBalDeleteIcon" aria-hidden="true" title="${DeleteText}"></i></td>
<td id="OpBlId">${TblLen + 1}</td>
<td id="VoucherNo" >${$("#BillVoucherNumber").val()}</td>
<td id="VoucherDt" >${VoucherDt}</td>
<td id="VoucherAmt" class="num_right">${cmn_addCommas(Amount)}</td>
<td id="VoucherType" >${$("#billVoucType option:selected").text()}</td>
<td id="txt_sales_person">${sales_per}</td>
<td id="txt_sales_person_id" hidden="hidden">${$("#sales_person").val()}</td>
</tr>`);
    /*<td id="VoucherDt" >${$("#BillVoucherDate").val()}</td>*/
//    $('#HdnOpBilldetailTable tbody').append(`<tr id="R${++rowIdx}">
//        <td id="VoucherNo">${$("#BillVoucherNumber").val()}</td>
//        <td id="VoucherDt">${$("#BillVoucherDate").val()}</td>
//        <td id="VoucherAmt">${parseFloat($("#Amount").val()).toFixed(ValDecDigit)}</td>
//        <td id="VoucherType">${$("#billVoucType option:selected").text()}</td>
//</tr>`);
    ResetVoucherDetailVal();
    CalculateBillAmtTotal();
    OnClickOPDeleteIcon();
    OnClickEditIcon();
}
function OnClickVoucherUpdateBtn() {
    debugger;
    var BillVoc = $("#BillVoucherNumber").val();  
    var ErrorFlag = "N";
    var BillVouchDt = $("#BillVoucherDate").val();
        if ($('#BillVoucherNumber').val() == "") {
            ErrorFlag = "Y";
            $("#BillVoucherNumber").css("border-color", "Red");
            $('#spanBillVouNo').text($("#valueReq").text());
            $("#spanBillVouNo").css("display", "block");
    }
    var Amount = cmn_ReplaceCommas($('#Amount').val());
    if (Amount == "0" || Amount == "") {
        ErrorFlag = "Y";
        $("#Amount").css("border-color", "Red");
        $('#spanBillVouAmt').text($("#valueReq").text());
        $("#spanBillVouAmt").css("display", "block");
    }
    if (BillVouchDt == "" || BillVouchDt == null || BillVouchDt == "NaN") {
        $("#spanBillVouDt").text($("#valueReq").text());
        $("#spanBillVouDt").css("display", "block");
        $("#BillVoucherDate").css("border-color", "red");
        ErrorFlag = "Y";
    }        
        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            $("#BillVoucherNumber").css("border-color", "#ced4da");
            $("#Amount").css("border-color", "#ced4da");
            $("#BillVoucherDate").css("border-color", "#ced4da");
            $("#spanBillVouDt").css("display", "none");
            $("#spanBillVouAmt").css("display", "none");
            $("#spanBillVouNo").css("display", "none");
            $("#BillVoucherNumber").prop("disabled", false);

            $("#OpBilldetailTable > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var RID = currentRow.find("#VoucherNo").text();
                var Vocdate = $("#BillVoucherDate").val();
                var Amt = cmn_ReplaceCommas($("#Amount").val());
                var Voctype = $("#billVoucType  option:selected").text();
                var BillVocNo = $("#BillVoucherNumber").val();
                var sales_per = $("#sales_person option:selected").text();
                if (sales_per == "0" || sales_per == "---Select---") {
                    sales_per = "";
                }

                if (BillVocNo == RID) {
                    var VoucherDt = Vocdate.split("-").reverse().join("-");
                    currentRow.find("#VoucherNo").text(BillVocNo);
                    currentRow.find("#VoucherDt").text(VoucherDt);
                    currentRow.find("#VoucherAmt").text(cmn_addCommas(Amt));
                    currentRow.find("#VoucherType").text(Voctype);
                    currentRow.find("#txt_sales_person").text(sales_per);
                    currentRow.find("#txt_sales_person_id").text($("#sales_person").val());
                }

            });
            debugger;
            ResetVoucherDetailVal();
            CalculateBillAmtTotal();
            $('#divAdd').css('display', 'block');
            $('#divUpdate').css('display', 'none');
        }
}
function ResetVoucherDetailVal() {
    $('#BillVoucherNumber').val("");
    $('#BillVoucherDate').val("");
    $('#Amount').val("");
    $('#sales_person').val("0").trigger('change');

    $("#spanBillVouAmt").css("display", "none");
    $("#Amount").css("border-color", "#ced4da");

    $("#spanBillVouDt").css("display", "none");
    $("#BillVoucherDate").css("border-color", "#ced4da");


    $("#BillVoucherNumber").css("border-color", "#ced4da");
    $("#spanBillVouNo").css("display", "none");

    $("#spanSalePerson").css("display", "none");
    $("[aria-labelledby='select2-sales_person-container']").css("border-color", "#ced4da");
}
function DisableVoucherDetail() {
    $('#BillVoucherNumber').prop("disabled",true);
    $('#BillVoucherDate').prop("disabled", true);
    $('#Amount').prop("disabled", true);
    $('#billVoucType').prop("disabled", true);
    $('#divAdd').css('display', 'none');
    $("#OPBalSaveAndExitBtn").attr("disabled", true);
    $("#DiscardAndExit").attr("disabled", true);

    debugger;
    //$("#Btndetail").css("display", "none");
    
}
function EnableVoucherDetail() {
    $('#BillVoucherNumber').prop("disabled", false);
    $('#BillVoucherDate').prop("disabled", false);
    $('#Amount').prop("disabled", false);
    $('#billVoucType').prop("disabled", false);
    $('#divAdd').css('display', 'block');
    $("#OPBalSaveAndExitBtn").attr("disabled", false);
    $("#DiscardAndExit").attr("disabled", false);
}
function CalculateBillAmtTotal() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmtDebit = parseFloat(0).toFixed(ValDecDigit);
    var TotalAmtCredit = parseFloat(0).toFixed(ValDecDigit);
    $("#OpBilldetailTable >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var AmtType = currentRow.find("#VoucherType").text();
        if (AmtType == "Debit") {
            var TblDramt = parseFloat(cmn_ReplaceCommas(currentRow.find("#VoucherAmt").text())).toFixed(ValDecDigit);
            if (TblDramt == null || TblDramt == "") {
                TblDramt = 0;
            }
        }
        if (AmtType == "Credit") {
            var TblCramt = parseFloat(cmn_ReplaceCommas(currentRow.find("#VoucherAmt").text())).toFixed(ValDecDigit);
            if (TblCramt == null || TblCramt == "") {
                TblCramt = 0;
            }
        }
        if (AmtType == "Debit") {
            TotalAmtDebit = (parseFloat(TotalAmtDebit) + parseFloat(TblDramt)).toFixed(ValDecDigit);
        }
        if (AmtType == "Credit") {
            TotalAmtCredit = (parseFloat(TotalAmtCredit) + parseFloat(TblCramt)).toFixed(ValDecDigit);
        }
    });
    debugger;   
    var TotalAmtDebit1 = parseFloat(TotalAmtDebit).toFixed(ValDecDigit);
    var TotalAmtCredit1 = parseFloat(TotalAmtCredit).toFixed(ValDecDigit);
    $('#TotalDebit').text(cmn_addCommas(TotalAmtDebit1));
    //$('#TotalCredit').text(parseFloat(TotalAmtCredit).toFixed(ValDecDigit));
    $('#TotalCredit').text(cmn_addCommas(TotalAmtCredit1));
    if (parseFloat(TotalAmtDebit) > parseFloat(TotalAmtCredit)) {
        var Balance = (parseFloat(TotalAmtDebit) - parseFloat(TotalAmtCredit));
        var Balance1 = parseFloat(Balance).toFixed(ValDecDigit)
        $('#TotalBalance').text(cmn_addCommas(Balance1));
        $('#TotalBalanceType').text("Debit");
    }
    else {
        var Balance = (parseFloat(TotalAmtCredit) - parseFloat(TotalAmtDebit));
        var Balance1 = parseFloat(Balance).toFixed(ValDecDigit);
        $('#TotalBalance').text(cmn_addCommas(Balance1));
        $('#TotalBalanceType').text("Credit");
    }
}
function OnChangeBillAmt(e) {
    debugger;
    //var ValDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();

    var Amount = cmn_ReplaceCommas($("#Amount").val());
    if (Amount == "" || Amount == null || parseFloat(Amount) == parseFloat("0")) {
        $("#spanBillVouAmt").text($("#valueReq").text());
        $("#spanBillVouAmt").css("display", "block");
        $("#Amount").css("border-color", "red");
        $("#Amount").val("");
    }
    else {
        var Amount1 = parseFloat(Amount).toFixed(ValDecDigit)
        $("#Amount").val(cmn_addCommas(Amount1));
        $("#spanBillVouAmt").css("display", "none");
        $("#Amount").css("border-color", "#ced4da");
    }

}
function OnChangeBillVoucherNo() {
    debugger;
   
    var BillVouchNo = $("#BillVoucherNumber").val();
    if (BillVouchNo == "" || BillVouchNo == null || BillVouchNo == "NaN") {
        $("#spanBillVouNo").text($("#valueReq").text());
        $("#spanBillVouNo").css("display", "block");
        $("#BillVoucherNumber").css("border-color", "red");
        $("#BillVoucherNumber").val("");
    }
    else {        
        $("#spanBillVouNo").css("display", "none");
        $("#BillVoucherNumber").css("border-color", "#ced4da");
    }

}
function OnChangeBillVoucherDt() {
    debugger;
    var BillVouchDt = $("#BillVoucherDate").val();
    if (BillVouchDt == "" || BillVouchDt == null || BillVouchDt == "NaN") {
        $("#spanBillVouDt").text($("#valueReq").text());
        $("#spanBillVouDt").css("display", "block");
        $("#BillVoucherDate").css("border-color", "red");
        $("#BillVoucherDate").val("");
    }
    else {
        $("#spanBillVouDt").css("display", "none");
        $("#BillVoucherDate").css("border-color", "#ced4da");
    }

}
function OnClickSaveAndCloseBtn() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    debugger;
    var IconOpBalsp = parseFloat(cmn_ReplaceCommas($("#IconOpBalsp").val())).toFixed(ValDecDigit);
    var TotalBalanceAmt = parseFloat(cmn_ReplaceCommas($("#TotalBalance").text())).toFixed(ValDecDigit);
    var IconBalanceType = $("#IconBalanceType").val();
    var TotalBalanceType = $("#TotalBalanceType").text();
    var rowIdx = 0;
    $("#OPBalSaveAndExitBtn").attr("data-dismiss", "");
    if (IconBalanceType == TotalBalanceType) {
        if (parseFloat(IconOpBalsp) == parseFloat(TotalBalanceAmt)) {

            let NewArr = [];
            debugger;
            $('#HdnOpBilldetailTable tbody >tr').remove();
            $("#OpBilldetailTable >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);               
                    var OpBlId = currentRow.find("#OpBlId").text();
                    var VoucherNo = currentRow.find("#VoucherNo").text();
                    var VoucherDt = currentRow.find("#VoucherDt").text();
                    var VoucherAmt = cmn_ReplaceCommas(currentRow.find("#VoucherAmt").text());
                    var VoucherType = currentRow.find("#VoucherType").text();
                    var sales_per_nm = currentRow.find("#txt_sales_person").text();
                    var sales_per_id= currentRow.find("#txt_sales_person_id").text();
                debugger;
                    $('#HdnOpBilldetailTable tbody').append(`<tr id="R${++rowIdx}">
            <td id="OpBlId">${OpBlId}</td>
            <td id="VoucherNo">${VoucherNo}</td>
            <td id="VoucherDt">${VoucherDt}</td>
            <td id="VoucherAmt">${VoucherAmt}</td>
            <td id="VoucherType">${VoucherType}</td>
            <td id="hdn_txt_sales_person">${sales_per_nm}</td>
            <td id="hdn_txt_sales_person_id" hidden="hidden">${sales_per_id}</td>
                </tr>`);
            });
            
            $("#OPBalSaveAndExitBtn").attr("data-dismiss", "modal");
        }
        else {
            //$("#OPBalSaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#OpeningBalMismatch").text(), "warning");
      
        }
    }
    else {
        //$("#OPBalSaveAndExitBtn").attr("data-dismiss", "");
        swal("", $("#OpeningBalMismatch").text(), "warning");
       
    }   
}
function OnClickOPDeleteIcon() {
    debugger;
    $('#OpBilldetailTable tbody').on('click', '#OpBalDeleteIcon', function () {
        debugger;
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
        debugger;
        //var SNo = $(this).closest('tr')[0].cells[2].children[0].value;
        //var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        CalculateBillAmtTotal();
        ResetVoucherDetailVal();
        $('#divAdd').css('display', 'block');
        $('#divUpdate').css('display', 'none');
        $("#BillVoucherNumber").prop("disabled", false);
    });
}
function OnClickEditIcon() {
   
    $("#OpBilldetailTable >tbody >tr").on('click', "#editBtnIcon", function (e) {

            debugger;
            var currentRow = $(this).closest('tr')
            //var row_index = currentRow.index();
            //  $("#comp_id").val(row_index);
            
        var VoucNo = currentRow.find("#VoucherNo").text()
        var VoucDt = currentRow.find("#VoucherDt").text()
        var VoucAmt = currentRow.find("#VoucherAmt").text()
        var VoucType = currentRow.find("#VoucherType").text()
        var txt_sales_person_id = currentRow.find("#txt_sales_person_id").text()

        var VoucherDt = VoucDt.split("-").reverse().join("-");

        $("#BillVoucherNumber").val(VoucNo);  
        $("#BillVoucherNumber").prop("disabled", true);
        $("#BillVoucherDate").val(VoucherDt);
        $("#Amount").val(VoucAmt);
        $('#sales_person').val(txt_sales_person_id).trigger('change');
        if (VoucType == "Debit") {
            $("#billVoucType").val("dr");
        }
        else {
            $("#billVoucType").val("cr");
        }           
        $('#divAdd').css('display', 'none');
        $('#divUpdate').css('display', 'block');

        $("#spanBillVouAmt").css("display", "none");
        $("#Amount").css("border-color", "#ced4da");

        $("#spanBillVouDt").css("display", "none");
        $("#BillVoucherDate").css("border-color", "#ced4da");

     
        $("#BillVoucherNumber").css("border-color", "#ced4da");
        $("#spanBillVouNo").css("display", "none");       
       
        });
    
}
function OnClickOpBalDiscardAndExitBtn() {
    OnClickVoucherDtResetBtn();
}
function OnClickVoucherDtResetBtn() {
    debugger;
    var ValDecDigit = $("#QtyDigit").text();
    ResetVoucherDetailVal();
    $('#OpBilldetailTable tbody tr').remove();
    $('#TotalDebit').text(parseFloat(0).toFixed(ValDecDigit));
    $('#TotalCredit').text(parseFloat(0).toFixed(ValDecDigit));
    $('#TotalBalance').text(parseFloat(0).toFixed(ValDecDigit));

    $("#spanBillVouNo").css("display", "none");
    $("#BillVoucherNumber").css("border-color", "#ced4da");
    $("#spanBillVouAmt").css("display", "none");
    $("#Amount").css("border-color", "#ced4da");
   
}
//---------------Bill Detail Pop Up End-----------------------//

function InsertOPBalanceDetail() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckFormValidation() == false) {
        return false;
    }
    var acc_type = $("#acc_type").val();
    if (acc_type == 1 || acc_type == 2) {
        if (CheckBillDetailValidations() == false) {
            return false;
        }
    }
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        var FinalBillWiseOPDetail = [];
        FinalBillWiseOPDetail = InsertBillWiseOPDetails();
        debugger;
        var BillWiseOPDt = JSON.stringify(FinalBillWiseOPDetail);
        $('#hdBillWiseOPDetailList').val(BillWiseOPDt);

        var Opbalsp = $("#Opbalsp").val();
        var ConvRate = $("#ConvRate").val();
        var Opbalbs = $("#Opbalbs").val();

        var Opbalsp1 = cmn_ReplaceCommas(Opbalsp);
        var ConvRate1 = cmn_ReplaceCommas(ConvRate);
        var Opbalbs1 = cmn_ReplaceCommas(Opbalbs);

        $("#Opbalsp").val(Opbalsp1);
        $("#ConvRate").val(ConvRate1);
        $("#Opbalbs").val(Opbalbs1);

        return true;
    }
    else {
        return false;
    }
};
function CheckFormValidation() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ValidationFlag = true;
    var acc_type = $('#acc_type').val();
    var coa_id = $('#coaid').val();
    var Opbalsp = $('#Opbalsp').val();
    var curr = $('#curr').val();
    var ConvRate1 = $('#ConvRate').val();
    var ConvRate = cmn_ReplaceCommas(ConvRate1);
    var coaid = $('#coaid').val();
    
    if (acc_type == "" || acc_type == "0") {
        document.getElementById("vmAccounttype").innerHTML = $("#valueReq").text();
        $("#acc_type").css("border-color", "red");
        ValidationFlag = false;
    }
    if (coa_id == "" || coa_id == "0") {
        //document.getElementById("vmcoa").innerHTML = $("#valueReq").text();
        $('#vmcoa').text($("#valueReq").text());
        $("#coaid").css("border-color", "red");
        $("[aria-labelledby='select2-coaid-container']").css("border-color", "red");
        $("#vmcoa").css("display", "block");
        ValidationFlag = false;
    }
    if (Opbalsp == "" || Opbalsp == "0") {
        document.getElementById("OpbalspError").innerHTML = $("#valueReq").text();
        $("#Opbalsp").css("border-color", "red");
        ValidationFlag = false;
    }
    if (curr == "" || curr == "0") {
        document.getElementById("vmCurr").innerHTML = $("#valueReq").text();
        $("#curr").css("border-color", "red");
        ValidationFlag = false;
    }
    if (ConvRate == "" || parseFloat(ConvRate) == parseFloat("0").toFixed(RateDecDigit)) {
        $("#ConvRateError").css("display", "block");
        document.getElementById("ConvRateError").innerHTML = $("#valueReq").text();
        $("#ConvRate").css("border-color", "red");
        ValidationFlag = false;
    }
    
    if (ValidationFlag == true) {
                return true;
            }
            else {
                return false;
            }
}
function CheckBillDetailValidations() {
    debugger;
    var ErrorFlag = "N";
    var ValDecDigit = $("#ValDigit").text();
    debugger;
    var OpBalsp = parseFloat(cmn_ReplaceCommas($("#Opbalsp").val())).toFixed(ValDecDigit);
    var TotalBalanceAmt = parseFloat(cmn_ReplaceCommas($("#TotalBalance").text())).toFixed(ValDecDigit);
    var OPBalanceType = $("#op_bal_type option:selected").text();
    var TotalBalanceType = $("#TotalBalanceType").text();

    if (OPBalanceType == TotalBalanceType) {
        if (parseFloat(OpBalsp) == parseFloat(TotalBalanceAmt)) {
        }
        else {
            swal("", $("#OpeningBalMismatch").text(), "warning");
            ErrorFlag = "Y";
            return false;
        }
    }
    else {
        swal("", $("#OpeningBalMismatch").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function InsertBillWiseOPDetails() {
    debugger;
    //var FBillWiseOPDetails = JSON.parse(sessionStorage.getItem("OpeningDetailSession"));
    $("#curr").prop("disabled", false);
    var BillWiseList = [];

    //<td id="VoucherNo" >${arr.Table[i].VoucherNo}</td>
//<td id="VoucherDt" >${arr.Table[i].VoucherDt}</td>
//<td id="VoucherAmt" class="num_right">${parseFloat(arr.Table[i].VoucherAmt).toFixed(ValDecDigit)}</td>
//<td id="VoucherType" >${arr.Table[i].VoucherType}</td>

     $("#OpBilldetailTable >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var VoucherNo = currentRow.find("#VoucherNo").text();
         var BillVouchDt = currentRow.find("#VoucherDt").text();
         var VoucherDt = BillVouchDt.split("-").reverse().join("-");
        //if (VoucherDt = moment(VoucherDt, "YYYY.MM.DD").format("DD-MM-YYYY")) {
        //    var VoucherDt = moment(VoucherDt, "DD.MM.YYYY").format("YYYY-MM-DD");
        //}
        //else {

         //}       
         var VoucherAmt = cmn_ReplaceCommas(currentRow.find("#VoucherAmt").text());
        var VoucherType = currentRow.find("#VoucherType").text();
         var sales_person_id = currentRow.find("#txt_sales_person_id").text();

         BillWiseList.push({ VoucherNo: VoucherNo, VoucherDt: VoucherDt, VoucherAmt: VoucherAmt, VoucherType: VoucherType, sales_person_id: sales_person_id });
    });   

    //$("#HdnOpBilldetailTable >tbody >tr").each(function (i, row) {
    //    debugger;

    //    //var VoucherNo = "";
    //    //var VoucherDt = "";
    //    //var VoucherAmt = "";
    //    //var VoucherType = "";

    //    var currentRow = $(this);
    //    var VoucherNo = currentRow.find("#VoucherNo").text();
    //    var VoucherDt = currentRow.find("#VoucherDt").text();
    //    //if (VoucherDt = moment(VoucherDt, "YYYY.MM.DD").format("DD-MM-YYYY")) {
    //    //    var VoucherDt = moment(VoucherDt, "DD.MM.YYYY").format("YYYY-MM-DD");
    //    //}
    //    //else {
           
    //    //}       
    //    var VoucherAmt = currentRow.find("#VoucherAmt").text();
    //    var VoucherType = currentRow.find("#VoucherType").text();

    //    BillWiseList.push({ VoucherNo: VoucherNo, VoucherDt: VoucherDt, VoucherAmt: VoucherAmt, VoucherType: VoucherType });
    //});   
    return BillWiseList;


};

function OnClickBillwiseDetailBtn(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var acc_id = clickedrow.find("#hd_acc_id").val();
    var AccName = clickedrow.find("#acc_name").text();
    var OpBalSp = 0;
    var OpBalBs = 0;
    var BalType = clickedrow.find("#hd_bal_type").val();
    var Curr = clickedrow.find("#curr_name").text();
    var Conv_rate = clickedrow.find("#conv_rate").text();
    var FinYear = clickedrow.find("#HDfin_year").text();
    if (BalType == "Dr" || BalType == "dr") {
        OpBalSp = clickedrow.find("#hdopbalsp_dr").val();
        OpBalBs = clickedrow.find("#op_bal_bs_dr").text();
    }
    if (BalType == "Cr" || BalType == "cr") {
        OpBalSp = clickedrow.find("#hdopbalsp_cr").val();
        OpBalBs = clickedrow.find("#op_bal_bs_cr").text();
    }

    $("#IconAccountName").val(AccName.trim());
    $("#IconOpBalsp").val(OpBalSp.trim());
    $("#IconOpBalbs").val(OpBalBs.trim());
    $("#IconCurr").val(Curr.trim());
    $("#IconBalanceType").val(BalType.trim());
    $("#IconConvRate").val(Conv_rate.trim());
    BillWiseOpeningDetail(acc_id, FinYear);
    $("#hdn_for_EditDeletebtn").val("N");
    

}
function BillWiseOpeningDetail(acc_id, FinYear) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
   
    if (acc_id != "" && acc_id != null && FinYear != "" && FinYear != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/OpeningBalance/GetOpeningBalBillDetail",
                data: {
                    AccId: acc_id, FinYear: FinYear
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#OpBilldetailTable tbody tr').remove();
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                ++rowIdx;
                                var amt_in_sp = parseFloat(cmn_ReplaceCommas(arr.Table[i].amt_in_sp)).toFixed(ValDecDigit);
                                $('#OpBilldetailTable tbody').append(` <tr id="${rowIdx}">
<td class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" style="filter: grayscale(100%)" id="" data-toggle="" title=""></i></td>
<td id="" class="red center"><i class="fa fa-trash deleteIcon" id="" style="filter: grayscale(100%)" aria-hidden="true" title=""></i></td>
<td>${rowIdx}</td>
<td id="VoucherNo" >${arr.Table[i].bill_no}</td>
<td id="VoucherDt" >${arr.Table[i].bill_dt}</td>
<td id="VoucherAmt" class="num_right">${cmn_addCommas(amt_in_sp)}</td>
<td id="VoucherType">${arr.Table[i].amt_type}</td>
            <td id="hdn_txt_sales_person">${arr.Table[i].sls_per_nm}</td>
            <td id="hdn_txt_sales_person_id" hidden="hidden">${arr.Table[i].sls_per_id}</td>
                </tr>`);
                            }
                            ResetVoucherDetailVal();
                            CalculateBillAmtTotal();
                            OnClickOPDeleteIcon();
                            OnClickEditIcon();
                            DisableVoucherDetail();
                        }

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }

}
//function BillWiseOpeningDetailHeader(acc_id, FinYear) {
//    debugger;
//    //var clickedrow = $(e.target).closest("tr");
//    var QtyDecDigit = $("#QtyDigit").text();///Quantity
//    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
//    var ValDecDigit = $("#ValDigit").text();///Amount

//    if (acc_id != "" && acc_id != null && FinYear != "" && FinYear != null) {
//        debugger;
//        $.ajax(
//            {
//                type: "Post",
//                url: "/ApplicationLayer/OpeningBalance/GetOpeningBalBillDetail1",
//                data: {
//                    AccId: acc_id, FinYear: FinYear
//                },
//                success: function (data) {
//                    debugger;
//                    if (data !== null && data !== "") {
//                        var arr = [];
//                        arr = JSON.parse(data);
//                        $('#OpBilldetailTable tbody tr').remove();
//                        if (arr.Table.length > 0) {
//                            //sessionStorage.removeItem("OpeningDetailSession");
//                            //sessionStorage.setItem("OpeningDetailSession", JSON.stringify(arr.Table));
//                            var rowIdx = 0;
//                            for (var i = 0; i < arr.Table.length; i++) {
//                                debugger;

//                                ++rowIdx;
//                                $('#OpBilldetailTable tbody').append(` <tr id="${rowIdx}">
//<td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></td>
//<td class="red center"> <i class="fa fa-trash deleteIcon" id="OpBalDeleteIcon" aria-hidden="true" title="${DeleteText}"></i></td>
//<td>${rowIdx}</td>
//<td id="VoucherNo" >${arr.Table[i].VoucherNo}</td>
//<td id="VoucherDt" >${arr.Table[i].VoucherDt}</td>
//<td id="VoucherAmt" class="num_right">${parseFloat(arr.Table[i].VoucherAmt).toFixed(ValDecDigit)}</td>
//<td id="VoucherType" >${arr.Table[i].VoucherType}</td>
//                </tr>`);
//                            }
//                            ResetVoucherDetailVal();
//                            CalculateBillAmtTotal();
//                            OnClickOPDeleteIcon();
//                            OnClickEditIcon();
//                            DisableVoucherDetail();
                            
//                        }

//                    }
//                },
//                error: function (XMLHttpRequest, textStatus, errorThrown) {
//                    debugger;
//                    //   alert("some error");
//                }
//            });
//    } else {

//    }

//}

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
function FetchOPBalData() {
    debugger
    var isfileexist = $('#OPBalfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    debugger
    var uploadStatus = $('#opbal_Status').val();
    if ($('#DttblOPBal tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    debugger
    $(".loader1").show();
    $('#BtnSearch').prop('disabled', false);
    var formData = new FormData();
    var file = $("#OPBalfile").get(0).files[0];
    formData.append("file", file);
    $('#btnOPBalImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + encodeURIComponent(uploadStatus));
    xhr.send(formData);
    xhr.onreadystatechange = function () {

        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportOPBalDetail').html(xhr.response);
            cmn_apply_datatable("#DttblOPBal")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnOPBalImportData').prop('disabled', true); // Keep the button disabled
                $('#btnOPBalImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnOPBalImportData').prop('disabled', false); // Enable the button
                $('#btnOPBalImportData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickOMRErrorDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var accountname = clickedrow.find("#acc_name").text();
    var accounttype = clickedrow.find("#acc_type").text();
    var formData = new FormData();
    var file = $("#OPBalfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?accounttype=' + encodeURIComponent(accounttype) + '&accountname=' + encodeURIComponent(accountname));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function BindExcelBillDetail(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var accountname = clickedrow.find("#acc_name").text();
    var accounttype = clickedrow.find("#acc_type").text();
    var amount = clickedrow.find("#op_bal").text().trim();
    var amounttype = clickedrow.find("#bal_type").text();
    var formData = new FormData();
    var file = $("#OPBalfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindExcelBill?accountname=' + encodeURIComponent(accountname) + '&accounttype=' + encodeURIComponent(accounttype));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#OpeningBalanceDetail").css("display", "block");
            $('#OpeningBalanceDetail').html(xhr.response);
            $("#AccountName").val(accountname);
            $("#AccountType").val(accounttype);
            $("#idamount").val(amount);
            $("#AmountType").val(amounttype);
        }
    }
}
function ImportDataFromExcel() {
    debugger
    $(".loader1").show();
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var file = $("#OPBalfile").get(0).files[0];
        formData.append("file", file);
        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportOpBalFromExcel');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            $(".loader1").hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText.toLowerCase().includes("Success")) {
                    swal("", xhr.responseText, "success");
                    $('.loader1').hide();
                }
                else {
                    var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                    if (responseMessage == "Data saved successfully") {
                        swal("", xhr.responseText, "success");
                        $('#PartialImportOPBalData').modal('hide');
                        $('.loader1').hide();
                        window.location.reload();
                    }
                    else {
                        swal("", xhr.responseText, "warning");
                        $('.loader1').hide();
                    }
                }
            }
        }
    }
}
function Closemodal() {
    $('#DttblOPBal').DataTable().destroy();
    $('#DttblOPBal tbody').empty()
    $('#opbalstatus_id').empty();
    $('#OPBalfile').val('');
    $('#btnOPBalImportData').prop('disabled', true);
    $('#btnOPBalImportData').css('background-color', '#D3D3D3')
}
function Close_modal() {
    debugger;
    $("#OpeningBalanceDetail").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
function onchangebillVoucType() {
    debugger;
    var balVouType = $('#billVoucType').val();
    var accType = $('#acc_type').val();
    if (balVouType == "dr") {
        if (accType == "1") {
            $("#DviSlsPerson").css("display", "");
        }
    }
    else {
        $("#DviSlsPerson").css("display", "none");
    }
}
function onchangeSalePerson() {
    var salesperson = $("#sales_person").val();
    if (salesperson == "" || salesperson == null || salesperson == "NaN" || salesperson == "0") {
        $("#spanSalePerson").text($("#valueReq").text());
        $("#spanSalePerson").css("display", "block");
        $("[aria-labelledby='select2-sales_person-container']").css("border-color", "red");
        ValidInfo = "Y";
    }
    else {
        $("#spanSalePerson").css("display", "none");
        $("[aria-labelledby='select2-sales_person-container']").css("border-color", "#ced4da");
    }
}

