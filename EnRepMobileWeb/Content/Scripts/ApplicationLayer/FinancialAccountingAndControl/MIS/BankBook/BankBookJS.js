$(document).ready(function () {
    debugger;
    $("#acc_name").select2({
        ajax: {
            url: $("#AccList").val(),
            data: function (params) {
                var queryParameters = {
                    Acc_name: params.term // search term like "a" then "an"
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
});

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
   
    if (FromDate != null && FromDate != "" && ToDate != null && ToDate != "") {
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
function FilterBankBookDetails() {
    debugger;
    try {
        var acc_name = $("#acc_name option:selected").val();
        var curr = $("#curr").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();

        if (acc_name == "0" || acc_name == null || acc_name == "") {
            $("#vmacc_name").text($("#valueReq").text());
            $("#vmacc_name").css("display", "block");
            $("[aria-labelledby='select2-acc_name-container']").css("border-color", "red");
            hideLoader();/*Add by Hina sharma on 19-11-2024 */ /*Commented by Suraj Maurya on 20-06-2025*/
            return false;
        }
        else {
            $("#vmacc_name").css("display", "none");
            $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");
            //showLoader();/*Add by Hina sharma on 19-11-2024 */ /*Commented by Suraj Maurya on 20-06-2025*/
        }
        if (Fromdate != "" && Fromdate != null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/BankBook/SearchBankBookDetail",
                data: {
                    acc_id: acc_name,
                    curr_id: curr,
                    Fromdate: Fromdate,
                    Todate: Todate
                },
                success: function (data) {
                    debugger;
                    $('#divbankbooklist').html(data);
                    //$("a.btn.btn-default.buttons-print.btn-sm").remove();//Commented by Suraj Maurya on 20-06-2025
                    //$("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                    //$(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm" onclick="GeneralLedgerPrint()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>');

                    //$("a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
                    //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

                    //var opbal = $("#op_bal").val();
                    //var opbaltype = $("#op_bal_type").val().trim();
                    //var clbal = $("#cl_bal").val();
                    //var clbaltype = $("#cl_bal_type").val().trim();

                    //$("#txt_OpeningBalance").val(opbal);
                    //if (opbaltype != "0") {
                    //    $("#lbl_opbaltype").text(opbaltype);
                    //    $("#Hdnlbl_opbaltype").val(opbaltype);
                    //}
                    //else {
                    //    $("#lbl_opbaltype").text("");
                    //}
                    //$("#txt_ClosingBalance").val(clbal);
                    //if (clbaltype != "0") {
                    //    $("#lbl_clbaltype").text(clbaltype);
                    //    $("#Hdnlbl_clbaltype").val(clbaltype);
                    //}
                    //else {
                    //    $("#lbl_clbaltype").text("");
                    //}
                    //hideLoader();/*Add by Hina sharma on 19-11-2024 */ /*Commented by Suraj Maurya on 20-06-2025*/
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }
            });
        }
        else {
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}
function GeneralLedgerPrint() {

    //var arr = [];//Commented by Suraj Maurya on 20-06-2025 due to process changed

    //$("#HdnTableBankBook tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};

    //    list.Acc_Name = CurrentRow.find("#Acc_Name").text();
    //    list.Vou_No = CurrentRow.find("#Vou_No").text();
    //    list.Vou_Dt = CurrentRow.find("#Vou_Dt").text();
    //    list.Ins_Type = CurrentRow.find("#Ins_Type").text();
    //    list.Ins_No = CurrentRow.find("#Ins_No").text();
    //    list.Reconciled = CurrentRow.find("#Reconciled").text();
    //    list.Narr = CurrentRow.find("#Narr").text();
    //    list.Dr_Amt = CurrentRow.find("#Dr_Amt").text();
    //    list.Cr_Amt = CurrentRow.find("#Cr_Amt").text();
    //    list.Closing_Bal = CurrentRow.find("#Closing_Bal").text();
    //    list.Bal_Type = CurrentRow.find("#Bal_Type").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#BankBookPrintData").val(array);

    $("#hdnCSVPrint").val(null);
    $("#hdnPDFPrint").val("Print");

    $('form').submit();
    hideLoader();

}
function AccountRecevablCSV() {
    //var arr = [];//Commented by Suraj Maurya on 20-06-2025 due to process changed

    //$("#HdnTableBankBook tbody tr").each(function () {
    //    var CurrentRow = $(this);
    //    var list = {};

    //    list.Acc_Name = CurrentRow.find("#Acc_Name").text();
    //    list.Vou_No = CurrentRow.find("#Vou_No").text();
    //    list.Vou_Dt = CurrentRow.find("#Vou_Dt").text();
    //    list.VouDt = CurrentRow.find("#Voudt").text();
    //    list.Ins_Type = CurrentRow.find("#Ins_Type").text();
    //    list.Ins_No = CurrentRow.find("#Ins_No").text();
    //    list.Reconciled = CurrentRow.find("#Reconciled").text();
    //    list.Narr = CurrentRow.find("#Narr").text();
    //    list.Dr_Amt = CurrentRow.find("#Dr_Amt").text();
    //    list.Cr_Amt = CurrentRow.find("#Cr_Amt").text();
    //    list.Closing_Bal = CurrentRow.find("#Closing_Bal").text();
    //    list.Bal_Type = CurrentRow.find("#Bal_Type").text();
    //    arr.push(list);
    //});
    //var array = JSON.stringify(arr);
    //$("#BankBookPrintData").val(array);

    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val("CsvPrint");
    //var searchValue = $("#DataTable_BankBook_filter input[type=search]").val();
    //$("#searchValue").val(searchValue);

    var dt = $("#DataTable_BankBook").DataTable(); var order = dt.order();  // [[colIndex, 'asc/desc']]
    var colIndex = order[0][0];
    var direction = order[0][1];
    var search = dt.search();
    var columnName = dt.settings().init().columns[colIndex].data; console.log(columnName); console.log(direction)

    $("#searchValue").val(search);
    $("#HdnsortColumn").val(columnName);
    $("#HdnsortColumnDir").val(direction);

    $('form').submit();
    hideLoader();
    
}
function OnChnage_AccName() {
    var acc_name = $("#acc_name option:selected").val();
    if (acc_name == "0" || acc_name == null || acc_name == "") {
        $("#vmacc_name").text($("#valueReq").text());
        $("#vmacc_name").css("display", "block");
        $("[aria-labelledby='select2-acc_name-container']").css("border-color", "red");
    }
    else {
        $("#HdnAccName").val($("#acc_name option:selected").text());
        $("#HdnBankAccId").val($("#acc_name option:selected").val());
        $("#vmacc_name").css("display", "none");
        $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");
    }
}
//function validatefydate(FromDate, ToDate) {/*commented by Hina shrama on 13-08-2024 for current date of fin year */
//    debugger
//    var validate = "N";
//    var finfromdt = "";
//    var finTodt = "";
//    var fin_fromdt = "";
//    var fin_Todt = "";
//    $("#tbl_hdnfylist tbody tr").each(function () {
//        var curr_row = $(this);
//        var fystdt = curr_row.find("#fystdt").text();
//        var fyenddt = curr_row.find("#fyenddt").text();

//        var boolfromdt = "N";
//        var booltodt = "N";

//        if ((Date.parse(fystdt) <= Date.parse(FromDate)) && (Date.parse(fyenddt) >= Date.parse(FromDate))) {
//            boolfromdt = "Y";
//            finfromdt = fystdt;
//            finTodt = fyenddt;
//        }
//        if ((Date.parse(fystdt) <= Date.parse(ToDate)) && (Date.parse(fyenddt) >= Date.parse(ToDate))) {
//            booltodt = "Y";
//        }
//        if (boolfromdt == "Y" && booltodt == "Y") {
//            validate = "Y";
//            fin_fromdt = fystdt;
//            fin_Todt = fyenddt;
//            return;
//        }
//    });

//    if (validate != "Y") {
//        //$("#txtFromdate").val(finfromdt);
//        $("#txtTodate").val(finTodt);
//    }
//}
function validatefydate(FromDate, ToDate) { /*Add changes by Hina on 13-08-2024 for show current date of fin Year */
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
        //$("#txtFromdate").val(finfromdt);
        //$("#txtTodate").val(finTodt);
    }
    else {
        $("#txtTodate").val(ToDate);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#Vou_no").text().trim();
    var voudt = clickedrow.find("#Vou_dt").text().trim();
    var vou_dt = clickedrow.find("#Voudt").text().trim();
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