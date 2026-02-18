
$(document).ready(function () {
    debugger;
    $("#PriceList #datatable-buttons tbody").bind("dblclick", function (e) {
        // debugger;
        try {
            var WF_status = $('#WF_status').val();
            //var Liststatus = $('#hdQCstatuscode').val();
            var clickedrow = $(e.target).closest("tr");
            var list_no = clickedrow.children("#List_No").text();
            window.location.href = "/ApplicationLayer/CustomerPriceList/EditPriceList/?list_no=" + list_no + "&WF_status=" + WF_status;
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.children("#List_No").text();
        var doc_date = clickedrow.children("#Create_DT").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(doc_no, doc_date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(doc_no);
    });

});
function FilterListPrice() {
    debugger;

    var Act_Status = $("#supp_act").val();


    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CustomerPriceList/PriceListSearch",
        data: {        
            Act_Status: Act_Status,
        },
        success: function (data) {
            debugger;
            $('#PriceList').html(data);
           // $('#ListFilterData').val(SuppAct);
        },


    });
}
