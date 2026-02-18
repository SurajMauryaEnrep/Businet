/************************************************
Javascript Name:Domestic Purchase Invoice List Detail
Created By:Prem
Created Date: 18-05-2021
Description: This Javascript use for the Domestic Purchase Invoice Detail many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    RemoveSession();
    $('#Btn_AddNewPI').on("click", function () {
        debugger;
        RemoveSessionNew();
    });
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var FilterData = $("#FilterData").val();      
        var DocumentMenuId = $("#DocumentMenuId").val();
        var WF_Status = $("#WF_Status").val();
        var clickedrow = $(e.target).closest("tr");
        var _PI_No = clickedrow.children("#InvoiceNo").text();
        var _PI_Date = clickedrow.children("#InvDate").text();
        if (_PI_No != null && _PI_Date != "") {
            window.location.href = "/ApplicationLayer/LocalPurchaseInvoice/EditPurchaseInvoice/?Inv_no=" + _PI_No + "&Inv_dt=" + _PI_Date + "&FilterData=" + FilterData + "&DocumentMenuId=" + DocumentMenuId + "&WF_Status=" + WF_Status;
        }
    });

    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var PI_No = clickedrow.children("#InvoiceNo").text();
        var PI_Date = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(PI_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(PI_No, PI_Date, Doc_id, Doc_Status);
    });
    BindSupplierList();
});

function BindSupplierList() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    PI_SuppName: params.term, // search term like "a" then "an"
                    DocumentMenuId: DocumentMenuId
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    //PO_ErrorPage();
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
function RemoveSession() {
    sessionStorage.removeItem("EditPI_No");
    sessionStorage.removeItem("EditPI_Date");
}
function RemoveSessionNew() {
    sessionStorage.removeItem("PI_TaxCalcDetails");
    sessionStorage.removeItem("PI_TransType");
}
function BtnSearch() {
    debugger;
    FilterPI_List();
    ResetWF_Level();
}

function FilterPI_List() {
    debugger;
    try {
        var SuppId = $("#SupplierName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
       var Docid = $("#DocumentMenuId").val();      
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalPurchaseInvoice/SearchPI_Detail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Docid: Docid
            },
            success: function (data) {
                debugger;
                $('#tbodyPIList').html(data);
                $("#FilterData").val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status)
                hideLoader()
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
                hideLoader();
            }
        });
    } catch (err) {
        debugger;
        console.log("PI Error : " + err.message);

    }
}

function GRNDetail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail";
    } catch (err) {
        console.log("PI Error : " + err.message);
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
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
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