/************************************************
Javascript Name:Domestic Sales Invoice List Detail
Created By:Prem
Created Date: 26-05-2021
Description: This Javascript use for the Domestic Sales Invoice Detail many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    //RemoveSession();
    $("#sales_person").select2();
    $("#tbodySIList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var Docid = $("#DocumentMenuId").val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var SIId = clickedrow.children("#InvoiceNo").text();
            var SIDate = clickedrow.children("#InvDate").text();
            var InvType = clickedrow.children("#InvoiceType").text();
   
            if (SIId != null && SIId != "") {
                window.location.href = "/ApplicationLayer/LocalSaleInvoice/EditSI/?SIId=" + SIId + "&SIDate=" + SIDate + "&InvType=" + InvType + "&ListFilterData=" + ListFilterData + "&Docid=" + Docid + "&WF_status=" + WF_status;
            }
        }
        catch (err) {
            debugger
        }
    });

    $("#datatable-buttons >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        var SI_No = clickedrow.children("#InvoiceNo").text();
        var SI_Date = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SI_No);

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SI_No, SI_Date, Doc_id, Doc_Status);
    });
    BindCustomerList();
});
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
function BindCustomerList() {
    debugger;
    var DocID = $("#DocumentMenuId").val();
    $("#CustomerName").select2({
        ajax: {           
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPage: params.page,
                    DocumentMenuId:DocID
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    //SI_ErrorPage();
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
//function RemoveSession() {
//    sessionStorage.removeItem("EditSI_No");
//    sessionStorage.removeItem("EditSI_Date");
//}
//function RemoveSessionNew() {
//    sessionStorage.removeItem("SI_TaxCalcDetails");
//    sessionStorage.removeItem("SI_TransType");
//}
function BtnSearch() {
    debugger;
    FilterSI_List();
    ResetWF_Level()
}
function FilterSI_List() {
    debugger;
    try {
        var CustId = $("#CustomerName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var DocID = $("#DocumentMenuId").val();
        var sales_person = $("#sales_person").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalSaleInvoice/SearchSI_Detail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                DocID: DocID,
                sales_person: sales_person
            },
            success: function (data) {
                debugger;
                $('#tbodySIList').html(data);
                $('#ListFilterData').val(CustId + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + sales_person);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("SI Error : " + err.message);

    }
}
//function SI_Detail() {
//    debugger;
//    try {
//        location.href = "/ApplicationLayer/LocalSaleInvoice/LocalSaleInvoiceDetail";
//    } catch (err) {
//        console.log("SI Error : " + err.message);
//    }
//}
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