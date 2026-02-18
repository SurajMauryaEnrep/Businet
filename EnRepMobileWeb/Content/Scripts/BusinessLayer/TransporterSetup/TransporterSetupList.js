$(document).ready(function () {
    debugger;
    $("#DivTransDetails #datatable-buttons tbody").bind("dblclick", function(e) {
        try {
            debugger;
            var clickedrow = $(e.target).closest("tr");
            var transId = clickedrow.children("#transId").text();

            if (transId != null && transId != "") {
                window.location.href = "/BusinessLayer/TransporterSetup/EditTransSetup?transId=" + transId;
            }
        }
        catch (exc) {
            throw exc;
        }
    });
});

function BindTransList() {
    debugger;
    var transId = $('#ddlTransList').val();
    var transType = $('#ddlTransType').val();
    var transMode = $('#ddlTransMode').val();
    // $("#datatable-buttons")[0].id = "dttbl";
    $.ajax({
        type: "POST",
        url: "/TransporterSetup/GetTransporterDetails",
        data: {
            transId: transId,
            transType: transType,
            transMode:transMode
        },
        success: function (data) {
            debugger;
            $("#DivTransDetails").html(data);
           // $("#dttbl")[0].id = "datatable-buttons";
        }
    });
}