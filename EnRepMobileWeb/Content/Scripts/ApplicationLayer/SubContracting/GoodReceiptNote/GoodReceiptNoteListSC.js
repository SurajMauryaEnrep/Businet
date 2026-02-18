$(document).ready(function () {
    debugger;

    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var WF_Status = $("#WF_Status").val();
        var clickedrow = $(e.target).closest("tr");
        var GRN_No = clickedrow.children("#GRNNo").text();
        var GRN_Date = clickedrow.children("#GRNDate").text();
        if (GRN_No != "" && GRN_No != null) {
            location.href = "/ApplicationLayer/GoodReceiptNoteSC/DoubleClickFromList/?DocNo=" + GRN_No + "&DocDate=" + GRN_Date + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;


        }

    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        var GRN_No = clickedrow.children("#GRNNo").text();
        var GRN_Date = clickedrow.children("#GRNDate").text();
        var Supp_name = clickedrow.children("#SuppName").text();
        var Doc_id = $("#DocumentMenuId").val();

        $("#hdDoc_No").val(GRN_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");

        GetWorkFlowDetails(GRN_No, GRN_Date, Doc_id, Doc_Status);

    });

    BindSupplierGRNList();


});
function BindSupplierGRNList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#GRNListSupplier").select2({
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
function BtnSearch() {
    debugger;
    FilterGRNList();
    ResetWF_Level();
}
function FilterGRNList() {
    debugger;
    try {
        var SuppId = $("#GRNListSupplier option:selected").val();

        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GoodReceiptNoteSC/SearchGRNSCListDetail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyGRNSCList').html(data);
                $('#ListFilterData').val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status)
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
