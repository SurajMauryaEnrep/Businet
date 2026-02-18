
/************************************************
Javascript Name:Domestic Purchase Order List
Created By:Rahul
Created Date: 23-03-2021
Description: This Javascript use for the DPO many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    RemoveSession();
    $('#Btn_AddNewPO').on("click", function () {
        debugger;
        RemoveSessionNew();
    });
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $('#ListFilterData').val();
        var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var SPO_No = clickedrow.children("#OrderNo").text();
        var SPO_Date = clickedrow.children("#OrderDt").text();
        if (SPO_No != "" && SPO_No != null) {
            location.href = "/ApplicationLayer/ServicePurchaseOrder/DoubleClickOnList/?DocNo=" + SPO_No + "&DocDate=" + SPO_Date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        var SPO_No = clickedrow.children("#OrderNo").text();
        var SPO_Date = clickedrow.children("#OrderDt").text();
        var Supp_name = clickedrow.children("#SuppName").text();
        var Doc_id = $("#DocumentMenuId").val();

        $("#hdDoc_No").val(SPO_No);

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        //GetPurchaseOrderDetails(SPO_No, SPO_Date);
        GetWorkFlowDetails(SPO_No, SPO_Date, Doc_id, Doc_Status);
        $("#hdpo_no").val(SPO_No);
        $("#hdpo_dt").val(SPO_Date);
        $("#hdsupp_name").val(Supp_name);
    });
    BindSupplierSPOList();
});
function RemoveSession() {
    sessionStorage.removeItem("EditLPONo");
    sessionStorage.removeItem("EditLPODate");
}
function RemoveSessionNew() {
    sessionStorage.removeItem("TaxCalcDetails");
    sessionStorage.removeItem("LPO_No");
    sessionStorage.removeItem("POTransType");
    sessionStorage.removeItem("POitemList");
}
function BtnSearch() {
    debugger;
    FilterDPOList();
    ResetWF_Level();
}
function FilterDPOList() {
    debugger;
    try {
        var SuppId = $("#POListSupplier option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseOrder/SearchSPODetail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodySPOList').html(data);
                $('#ListFilterData').val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status);
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
function DPODetail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/DPO/DPODetail";
    } catch (err) {
        console.log("PO Error : " + err.message);
    }
}
function BindSupplierSPOList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#POListSupplier").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
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
                    //results:data.results,
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
function OnClickOrderTrackingBtn(e) {
    debugger;
    $("#Table_SPOTracking tbody tr").remove();
    var currentrow = $(e.target).closest('tr');
    var SPONo = currentrow.find("#OrderNo").text();
    var SPODate = moment(currentrow.find("#OrderDt").text()).format('YYYY-MM-DD');;
    var SuppName = currentrow.find("#SuppName").text();
    if (SPONo != null && SPONo != "" && SPODate != null && SPODate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServicePurchaseOrder/GetSPOTrackingDetail",
            data: { SPONo: SPONo, SPODate: SPODate, SuppName: SuppName },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    return false;
                }
                $("#trackingSpo").html(data);
                cmn_apply_datatable("#Table_SPOTracking");
                $("#ServicePurchaseOrderNumber").val(SPONo);
                $("#ServiceOrderDate").val(SPODate);
                $("#SupplierName").val(SuppName);
            }
        });
    }
}
