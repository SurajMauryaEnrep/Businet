$(document).ready(function () {
    debugger;
    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    $("#ddlCustomerName1").select2();
    //$("#txtTodate").val(today);
    //BindDnCustomerList();
    $("#ShipmentbBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var ShipMent_type = $("#ShipMent_type").val();
            var Docid = $("#DocumentMenuId").val();
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var ShipmentNumber = clickedrow.children("#ship_no").text();
            var ShipmentDT = clickedrow.children("#ship_date").text();
            window.location.href = "/ApplicationLayer/Shipment/EditShipment/?ShipmentNumber=" + ShipmentNumber + "&ShipmentDate=" + ShipmentDT + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status + "&ShipMent_type=" + ShipMent_type + "&Docid=" + Docid;
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
       /* debugger;*/
        var clickedrow = $(e.target).closest("tr");
        var ShipmentNumber = clickedrow.children("#ship_no").text();
        var ShipmentDT = clickedrow.children("#ship_date").text();
        var Doc_id = $("#DocumentMenuId").val();
    /*    debugger;*/
        $("#hdDoc_No").val(ShipmentNumber);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");

        GetWorkFlowDetails(ShipmentNumber, ShipmentDT, Doc_id, Doc_Status);

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
   
    //var ship_type = "D";
    var ship_type;
    if ($("#rbDomestic").prop('checked')) {
        ship_type = "D";
    }
    if ($("#rbExport").prop('checked')) {
        ship_type = "E";
    }
    var Docid = $("#DocumentMenuId").val()
    debugger;
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    filterCustomerName: params.term,
                    ShipMent_type: ship_type,// search term like "a" then "an"
                    DocumentMenuId: Docid
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
    var ToDate = $("#txtTodate1").val();
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
    else {
        
    }
}
function FilterShipmentListData() {
    debugger;
    try {
        var Docid = $("#DocumentMenuId").val();
        var CustID = $("#ddlCustomerName1 option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate1").val();
        var Status = $("#ddlStatus option:selected").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/Shipment/ShipmentListSearch",
            data: {
                CustID: CustID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Docid: Docid
            },
            success: function (data) {
                debugger;
                $('#ShipmentbBody').html(data);
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
}

