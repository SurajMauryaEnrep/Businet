$(document).ready(function () {
    debugger;
    var Doc_no = $("#DeliveryNoteNumber").val();
    $("#hdDoc_No").val(Doc_no);

    $("#DeliveryNoteListTbBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var WF_status = $("#WF_status").val();
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var DeliveryNoteNo = clickedrow.children("#DeliverNoteNumber").text();
            var DeliveryNoteDt = clickedrow.children("#Dn_date").text();
            if (DeliveryNoteNo != null && DeliveryNoteNo != "") {
                window.location.href = "/ApplicationLayer/DeliveryNoteList/EditDeliveryNote/?DeliveryNoteNo=" + DeliveryNoteNo + "&DeliveryNoteDt=" + DeliveryNoteDt + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }
            
        }
        catch (err) {
        }
    });

    //ListRowHighLight();

    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var DeliveryNoteNo = clickedrow.children("#DeliverNoteNumber").text();
        var DeliveryNoteDate = clickedrow.children("#Dn_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(DeliveryNoteNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(DeliveryNoteNo, DeliveryNoteDate, Doc_id, Doc_Status);

    });
    //var now = new Date();
    //var month = (now.getMonth() + 1);
    //var day = now.getDate();
    //if (month < 10)
    //    month = "0" + month;
    //if (day < 10)
    //    day = "0" + day;
    //var today = now.getFullYear() + '-' + month + '-' + day;
    //$("#txtTodate").val(today);

    BindDnSupplierList();

});


function BindDnSupplierList() {
    debugger;
    $("#ddlSupplierName").select2({
        ajax: {
            url: $("#hdSupplierList").val(),
            data: function (params) {
                var queryParameters = {
                    Spp_Name: params.term // search term like "a" then "an"
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

function FilterDeliveryNoteList() {
    debugger;
    try {
        var SuppId = $("#ddlSupplierName option:selected").val();
        var SourceType = $("#ddlSourceType").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DeliveryNoteList/DeliveryNoteSearch",
            data: {
                SuppId: SuppId,
                SourceType: SourceType,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#DeliveryNoteListTbBody').html(data);
                $('#ListFilterData').val(SuppId + ',' + SourceType + ',' + Fromdate+ ',' + Todate + ',' + Status);
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