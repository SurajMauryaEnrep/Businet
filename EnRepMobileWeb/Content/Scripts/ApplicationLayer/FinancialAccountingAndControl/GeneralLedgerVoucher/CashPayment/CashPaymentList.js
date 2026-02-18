/************************************************
Javascript Name:Voucher(Cash) List
Created By:Hina Sharma
Created Date: 16-06-2022
Description: This Javascript use for the (Cash)Voucher Payment List Page

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    BindCashAccListOnList();
    var Doc_id = $("#DocumentMenuId").val();
    if (Doc_id == "105104115115") {
        $("#CPList #datatable-buttons tbody").bind("dblclick", function (e) {
            debugger;
            try {
                var WF_Status = $("#WF_Status").val();
                var ListFilterData = $("#ListFilterData").val();
                var clickedrow = $(e.target).closest("tr");
                var VouNo = clickedrow.children("#VouNumber").text().trim();
                var Voudt = clickedrow.children("#hdVouDate").text().trim();
                if (VouNo != null && VouNo != "") {
                    window.location.href = "/ApplicationLayer/CashPayment/EditVou/?VouNo=" + VouNo + "&Voudt=" + Voudt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
                }
            }
            catch (err) {
                debugger
            }
        });
    }
    if (Doc_id == "105104115105") {
        $("#CPList #datatable-buttons tbody").bind("dblclick", function (e) {
            debugger;
            try {
                var ListFilterData = $("#ListFilterData").val();
                var WF_Status = $("#WF_Status").val();
                var clickedrow = $(e.target).closest("tr");
                var VouNo = clickedrow.children("#VouNumber").text().trim();
                var Voudt = clickedrow.children("#hdVouDate").text().trim();
                if (VouNo != null && VouNo != "") {
                    window.location.href = "/ApplicationLayer/CashReceipt/EditVou/?VouNo=" + VouNo + "&Voudt=" + Voudt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
                }

            }
            catch (err) {
                debugger
            }
        });
    }
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var Vou_No = clickedrow.children("#VouNumber").text().trim();
        var Vou_Date = clickedrow.children("#hdVouDate").text().trim();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(Vou_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(Vou_No, Vou_Date, Doc_id, Doc_Status);
    });
    //$(document).ready(function () {
    //    debugger;
    //   /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
    //    $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="CashPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
});
function CashPaymentCSV() {
    debugger;
    docid = $("#DocumentMenuId").val();
    if (docid == '105104115115') {
        var SrcType = $("#Src_Type").val();
        var CashName = $("#ddlCashAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (CashName == "" || CashName == null) {
            CashName = "0";
        }
        window.location.href = "/ApplicationLayer/CashPayment/CashPaymentExporttoExcelDt?SrcType=" + SrcType + "&CashName=" + CashName + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&Status=" + Status;
    }
    else {
        var SrcType = $("#Src_Type").val();
        var CashName = $("#ddlCashAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (CashName == "" || CashName == null) {
            CashName = "0";
        }
        window.location.href = "/ApplicationLayer/CashReceipt/CashReceiptExporttoExcelDt?SrcType=" + SrcType + "&CashName=" + CashName + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&Status=" + Status;
    }
}
function BindCashAccListOnList() {
    debugger;
    $("#ddlCashAccName").select2({
        ajax: {
            url: $("#hdCashList").val(),
            data: function (params) {
                var queryParameters = {
                    cash_name: params.term // search term like "a" then "an"
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
function BtnSearch() {
    debugger;
    docid = $("#DocumentMenuId").val();
    if (docid == '105104115115') {
        FilterCPList();
    }
    else {
        FilterCRList();
    }
    ResetWF_Level();
}

function FilterCPList() {
    debugger;
    try {
        var SrcType = $("#Src_Type").val();
        var CashName = $("#ddlCashAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (CashName == "" || CashName == null) {
            CashName = "0";
        }

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CashPayment/SearchVoucherDetail",
            data: {
                Src_Type: SrcType,
                cash_id: CashName,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#CPList').html(data);
                /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="CashPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                $('#ListFilterData').val(SrcType + ',' + CashName + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MTO Error : " + err.message);

    }
}
function FilterCRList() {
    debugger;
    try {
        var SrcType = $("#Src_Type").val();
        var CashName = $("#ddlCashAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (CashName == "" || CashName == null) {
            CashName = "0";
        }

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CashReceipt/SearchVoucherDetail",
            data: {
                Src_Type: SrcType,
                cash_id: CashName,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#CPList').html(data);
               /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="CashPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                $('#ListFilterData').val(SrcType + ',' + CashName + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MTO Error : " + err.message);

    }
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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
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
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = clickedrow.children("#VouNumber").text();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
}

//--------On Click Icon Button Voucher Details ------//

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#Cp_Vouno").text();
    var voudt = clickedrow.find("#Cp_Date").text();
    var vou_dt = clickedrow.find("#hdVouDate").text();
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
    //var dr_amt = clickedrow.find("#spn_dramt").text();/*Comment by Hina on 03-08-2024 to remove comma from amount*/
    //var cr_amt = clickedrow.find("#spn_cramt").text();
    var debit_amt = clickedrow.find("#spn_dramt").text();
    var credit_amt = clickedrow.find("#spn_cramt").text();
    var dr_amt = cmn_ReplaceCommas(debit_amt); /*Add start by Hina on 03-08-2024 to remove comma from amount*/
    var cr_amt = cmn_ReplaceCommas(credit_amt); /*Add start by Hina on 03-08-2024 to remove comma from amount*/
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