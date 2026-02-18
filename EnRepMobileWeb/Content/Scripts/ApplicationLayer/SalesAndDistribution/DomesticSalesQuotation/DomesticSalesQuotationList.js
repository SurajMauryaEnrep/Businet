/************************************************
Javascript Name:Local Sale Quotation List
Created By:Mukesh
Created Date: 29-06-2021
Description: This Javascript use for the Local Sale Quotation List many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    $("#sales_person1").select2();
   //$("#QuotationTrackingIcon").bind("onclick", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    var Q_No = clickedrow.children("td:eq(1)").text();
    //    var Q_Date = clickedrow.children("td:eq(2)").text();
    //    var CustName = clickedrow.children("td:eq(4)").text();
    //    //var date = Q_Date.split("-");
    //    //var FDate = date[2] + '-' + date[1] + '-' + date[0];

    //    OnClickQTTrackingBtn(Q_No, Q_Date, CustName);
    //});
    debugger;
    $("#QTListTbody #datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var DocumentMenuId = $("#DocumentMenuId").val();
            var WF_status = $("#WF_status").val();
            var CustType = $("#CustType").val();
            var clickedrow = $(e.target).closest("tr");
            var SQId = clickedrow.children("#hdnQuotationNo").text();
            var SQDate = clickedrow.children("#QuotationDate").text();
            var rev_no = clickedrow.children("#rev_no").text();
            var Amendment = clickedrow.children("#dllAmendment").text();
            if (SQId != null && SQId != "") {
                window.location.href = "/ApplicationLayer/DomesticSalesQuotation/EditSQ/?SQId=" + SQId + "&SQDate=" + SQDate +
                    "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId +
                    "&WF_status=" + WF_status + "&CustType=" + CustType + "&Amendment=" + Amendment
                    + "&rev_no=" + rev_no;

            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
      //  debugger;
        var clickedrow = $(e.target).closest("tr");
        var QT_No = clickedrow.children("#hdnQuotationNo").text();
        var Q_Date = clickedrow.children("#QuotationDate").text();
        var date = Q_Date.split("-");
        var FDate = date[2] + '-' + date[1] + '-' + date[0];
        var Doc_id = $("#DocumentMenuId").val();
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        $("#hdDoc_No").val(QT_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" :( Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(QT_No, FDate, Doc_id, Doc_Status);
        

    });
    BindCustomerList();
   var QT_No = $("#qt_no").val();
   $("#hdDoc_No").val(QT_No);
});

function BindCustomerList() {
    debugger;
   var custtype= $("#CustType").val();
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                var queryParameters = {
                    QT_CustName: params.term, // search term like "a" then "an"
                    CustType: custtype
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
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
    FilterQuotList();
    ResetWF_Level();
    
}

function DateFormate_yyyyMMdd(Date) {
    var arr = Date.split('-');
    var ReturnDate = Date;
    if (arr[2].length > 2) {
        ReturnDate = arr[2] + '-' + arr[1] + '-' + arr[0];
    }
    return ReturnDate;
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
function FilterQuotList() {
    debugger;
    try {
        var CustId = $("#ddlCustomerName option:selected").val();
        //var OrderType = $("#ddlOrderType").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var QtType = $("#CustType").val();
        var sales_person = $("#sales_person1 option:selected").val();//$("#sales_person").val();
        $("#ddlCurrency option:selected").val();
        $("#datatable-buttons1").attr("id", "tbl1");

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DomesticSalesQuotationList/SearchQuotDetail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                QtType: QtType,
                sales_person: sales_person
            },
            success: function (data) {
                debugger;
                $('#QTListTbody').html(data);
                $("#tbl1").attr("id", "datatable-buttons1");
                $('#ListFilterData').val(CustId + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + sales_person);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

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
function OnClickQTTrackingBtn(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var QTNo = clickedrow.children("#QuotationNo").text();
    var QTDate = clickedrow.children("#QuotationDate").text();
    var CustName = clickedrow.children("#cust_name").text();
    $("#datatable-buttons").attr("id", "tbl");
    if (QTNo != null && QTNo != "" && QTDate != null && QTDate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DomesticSalesQuotationList/GetQTTrackingDetail",
            data: { QT_No: QTNo, QT_Date: QTDate },
            //dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                $("#trackingQuotation").html(data);
                $("#tbl").attr("id", "datatable-buttons");
                $("#QuotationNumber").val(QTNo);
                $("#QuotationDate").val(QTDate);
                $("#CustomerName").val(CustName);
            }
        });
    }
}

