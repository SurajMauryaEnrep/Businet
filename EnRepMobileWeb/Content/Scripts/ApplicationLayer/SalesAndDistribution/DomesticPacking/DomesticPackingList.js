$(document).ready(function () {

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    //$("#txtTodate").val(today);
    $("#ddlCustomerName1").select2();
    //BindDnCustomerList();
    debugger;
    $("#PackingListTbBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $("#WF_Status").val();
            var Docid = $("#DocumentMenuId").val();
            var clickedrow = $(e.target).closest("tr");
            var PackigListNumber = clickedrow.children("#PackingListNO").text();
            var PackigListDate = clickedrow.children("#packing_date").text();
            if (PackigListNumber != null && PackigListNumber != "") {
                window.location.href = "/ApplicationLayer/DomesticPacking/EditDomesticPacking/?PackigListNumber=" + PackigListNumber + "&PackigListDate=" + PackigListDate  + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status + "&Docid=" + Docid;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var PackigListNumber = clickedrow.children("#PackingListNO").text();
        var PKLDate = clickedrow.children("#packing_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        $("#hdDoc_No").val(PackigListNumber);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(PackigListNumber, PKLDate, Doc_id, Doc_Status);

    });

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
function BindDnCustomerList() {
    var Pack_type='D';
    
    debugger;
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    filterCustomerName: params.term,
                    pack_type: Pack_type// search term like "a" then "an"
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
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
            //  var today = fromDate.getFullYear() + '-' + month + '-' + day;

            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            // alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");



        }
    }
    else {
        //  alert('please select from and to date');
    }
}
function FilterPackingListData() {
    debugger;
    try {
        var CustID = $("#ddlCustomerName1 option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var Docid = $("#DocumentMenuId").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DomesticPacking/PackingListSearch",
            data: {
                CustID: CustID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Docid: Docid
            },
            success: function (data) {
                debugger;
                $('#PackingListTbBody').html(data);
                $('#ListFilterData').val(CustID + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
    ResetWF_Level();
}