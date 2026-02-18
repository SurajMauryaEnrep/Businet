/************************************************
Javascript Name:Service Purchase Invoice List
Created By:Mukesh
Created Date: 23-03-2023
Description: This Javascript use for the Service Purchase Invoice many function
Modified By:
Modified Date:
Description:
*************************************************/
$(document).ready(function () {
    debugger;
    $("#sales_person").select2();
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $('#ListFilterData').val();
        var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        if (SSINo != "" && SSINo != null) {
            location.href = "/ApplicationLayer/ScrapSaleInvoice/DoubleClickOnList/?DocNo=" + SSINo + "&DocDate=" + SSIDt + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
        }

    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SSINo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SSINo, SSIDt, Doc_id, Doc_Status);

    });
    BindCustomerList();
    //$(document).ajaxStart(function () {
    //    showLoader();
    //    // Hide loader when all AJAX requests are complete
    //    //hideLoader();
    //});
    //$(document).ajaxStop(function () {
    //    // Hide loader when all AJAX requests are complete
    //    hideLoader();
    //});
});
function BtnSearch() {
    debugger;
    FilterVerificationList();
    ResetWF_Level();
}
function FilterVerificationList() {
    debugger;
    try {
        var CustId = $("#ddlCustomerName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var sales_person = $("#sales_person").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/SearchScrapSaleInvoiceList",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                sales_person: sales_person
            },
            success: function (data) {
                debugger;
                $('#tbodySSIList').html(data);
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

function BindCustomerList() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term,
                    DocumentMenuId: DocumentMenuId // search term like "a" then "an"
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
            //alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    else {
        //alert('please select from and to date');
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


