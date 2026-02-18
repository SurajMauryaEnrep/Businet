/************************************************
Javascript Name:Bank List
Created By:Mukesh Sharma
Created Date: 30-05-2022
Description: This Javascript use for the BankPayment List Page

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    BindBankAccListOnList();
    $("#Tbl_BankPaymentDetails >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var BP_No = clickedrow.children("#VouNumber").text().trim();
        var BP_Date = clickedrow.children("#hdVouDate").text().trim();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(BP_No);

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(BP_No, BP_Date, Doc_id, Doc_Status);
    });
    var Doc_id = $("#DocumentMenuId").val();
    if (Doc_id == "105104115120") {
        $("#BPList #Tbl_BankPaymentDetails tbody").bind("dblclick", function (e) {

            try {
                debugger;
                var WF_Status = $("#WF_Status").val();
                var ListFilterData = $("#ListFilterData").val();
                var clickedrow = $(e.target).closest("tr");
                var VouNo = clickedrow.children("#VouNumber").text().trim();
                var Voudt = clickedrow.children("#hdVouDate").text().trim();
                if (VouNo != null && VouNo != "") {
                    window.location.href = "/ApplicationLayer/BankPayment/EditVou/?VouNo=" + VouNo + "&Voudt=" + Voudt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
                }

            }
            catch (err) {
                debugger
            }
        });

    }

    if (Doc_id == "105104115110") {
        $("#BPList #Tbl_BankPaymentDetails tbody").bind("dblclick", function (e) {

            try {
                debugger;
                var WF_Status = $("#WF_Status").val();
                var ListFilterData = $("#ListFilterData").val();
                var clickedrow = $(e.target).closest("tr");
                var VouNo = clickedrow.children("#VouNumber").text().trim();
                var Voudt = clickedrow.children("#hdVouDate").text().trim();
                if (VouNo != null && VouNo != "") {
                    window.location.href = "/ApplicationLayer/BankReceipt/EditVou/?VouNo=" + VouNo + "&Voudt=" + Voudt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
                }

            }
            catch (err) {
                debugger
            }
        });
    }
    //$(document).ready(function () {
    //    debugger;
    //   /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
    //    $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="BankPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
});
function BankPaymentCSV() {
    var docid = $("#DocumentMenuId").val();
    if (docid == '105104115120') {
        var SrcType = $("#Src_Type").val();
        var BankName = $("#ddlBankAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (BankName == "" || BankName == null) {
            BankName = "0";
        }
        var Currency = $("#ddlcurrlist").val();
        var InsType = $("#ddlInstype").val();
        var RecoStatus = $("#ddlRecostatus").val();
        window.location.href = "/ApplicationLayer/BankPayment/BankPaymentExporttoExcelDt?SrcType=" + SrcType + "&BankName=" + BankName + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&Status=" + Status + "&Currency=" + Currency + "&InsType=" + InsType + "&RecoStatus=" + RecoStatus;
    }
    else {
        var SrcType = $("#Src_Type").val();
        var BankName = $("#ddlBankAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (BankName == "" || BankName == null) {
            BankName = "0";
        }
        var Currency = $("#ddlcurrlist").val();
        var InsType = $("#ddlInstype").val();
        var RecoStatus = $("#ddlRecostatus").val();
        window.location.href = "/ApplicationLayer/BankReceipt/BankReceiptExporttoExcelDt?SrcType=" + SrcType + "&BankName=" + BankName + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&Status=" + Status + "&Currency=" + Currency + "&InsType=" + InsType + "&RecoStatus=" + RecoStatus;
    }
}
function BindBankAccListOnList() {
    debugger;
    $("#ddlBankAccName").select2({
        ajax: {
            url: $("#hdBankList").val(),
            data: function (params) {
                var queryParameters = {
                    bank_name: params.term // search term like "a" then "an"
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
    if (docid == '105104115120') {
        FilterBPList();
    }
    else {
        FilterBRList();
    }
    ResetWF_Level();
}

function FilterBPList() {
    debugger;
    try {
        var SrcType = $("#Src_Type").val();
        var BankName = $("#ddlBankAccName").val();       
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (BankName == "" || BankName == null) {
            BankName = "0";
        }
        var Currency = $("#ddlcurrlist").val();
        var InsType = $("#ddlInstype").val();
        var RecoStatus = $("#ddlRecostatus").val();
        
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/BankPayment/SearchBankPaymentDetail",
            data: {
                Src_Type: SrcType,
                bank_id: BankName,
                Fromdate: Fromdate,
                Todate: Todate,                
                Status: Status,
                Currency: Currency,
                InsType: InsType,
                RecoStatus : RecoStatus
            },
            success: function (data) {
                debugger;
                $('#BPList').html(data);
                /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                /*$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="BankPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')*/
                $('#ListFilterData').val(SrcType + ',' + BankName + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + Currency + ',' + InsType + ',' + RecoStatus);
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
function FilterBRList() {
    debugger;
    try {
        var SrcType = $("#Src_Type").val();
        var BankName = $("#ddlBankAccName").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();
        if (BankName == "" || BankName == null) {
            BankName = "0";
        }
        var Currency = $("#ddlcurrlist").val();
        var InsType = $("#ddlInstype").val();
        var RecoStatus = $("#ddlRecostatus").val();

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/BankReceipt/SearchBankPaymentDetail",
            data: {
                Src_Type: SrcType,
                bank_id: BankName,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Currency: Currency,
                InsType: InsType,
                RecoStatus: RecoStatus
            },
            success: function (data) {
                debugger;
                $('#BPList').html(data);
                /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                /*$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="BankPaymentCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')*/
                $('#ListFilterData').val(SrcType + ',' + BankName + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + Currency + ',' + InsType + ',' + RecoStatus);
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
            $("#txtTodate").val(today);
            debugger;
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
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var BP_No = clickedrow.children("#VouNumber").text();
    var BP_Date = clickedrow.children("#hdVouDate").text();
    var Doc_id = $("#DocumentMenuId").val();
    BP_No = BP_No.toString().trim();
    $("#hdDoc_No").val();
    GetWorkFlowDetails(BP_No, BP_Date, Doc_id);
    var a = 1;
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

    var vou_no = clickedrow.find("#Bp_Vouno").text();
    var voudt = clickedrow.find("#Bp_Date").text();
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
function InsertSearchDetail() {
    debugger;
    var dt = $("#Tbl_BankPaymentDetails").DataTable();
    var order = dt.order();  // [[colIndex, 'asc/desc']]
    var colIndex = order[0][0];
    var direction = order[0][1];
    var search = dt.search();
    var columnName = dt.settings().init().columns[colIndex].data; console.log(columnName); console.log(direction)

    $("#searchValue").val(search);
    $("#HdnsortColumn").val(columnName);
    $("#HdnsortColumnDir").val(direction);
    return true;
}