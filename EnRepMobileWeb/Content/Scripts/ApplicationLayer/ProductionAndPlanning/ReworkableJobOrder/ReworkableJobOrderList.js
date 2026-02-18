$(document).ready(function () {
    debugger;
    

    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var WF_Status = $("#WF_Status").val();
        var clickedrow = $(e.target).closest("tr");
        var RJO_No = clickedrow.children("#RJONo").text(); 
        var RJO_Date = clickedrow.children("#RJODt").text();
        debugger;
        var ItemIdList = clickedrow.children("#hdnListItem_ID").text();
        if (RJO_No != "" && RJO_No != null) {
            location.href = "/ApplicationLayer/ReworkableJobOrder/RJODoubleClickFromList/?DocNo=" + RJO_No + "&DocDate=" + RJO_Date + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status + "&ItemIdList=" + ItemIdList;

        }

    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        var RJO_No = clickedrow.children("#RJONo").text();
        var RJO_Date = clickedrow.children("#RJODt").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#RJODoc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        $("#hdDoc_No").val(RJO_No);
        GetWorkFlowDetails(RJO_No, RJO_Date, Doc_id, Doc_Status);
        
    });

    //$("#ddl_ItemNameList").select2();
    DynamicSerchableItemDDL("", "#ddl_ItemNameList", "", "", "", "RwkJobOrdr", "Y"); /* Added by Suraj Maurya on 28-07-2025 */


});



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
    $("#WF_Status").val(null);
    $("#WF_Status").val("");
    FilterRJOList();
    ResetWF_Level();
}
function FilterRJOList() {
    debugger;
    try {
        var ItemId = $("#ddl_ItemNameList option:selected").val();

        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $("#WF_Status").val(null);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ReworkableJobOrder/SearchRewrkJOListDetail",
            data: {
                ItemId: ItemId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyRJOList').html(data);
                $('#ListFilterData').val(ItemId + ',' + Fromdate + ',' + Todate + ',' + Status)
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
function OnChange_ProductNameList() {
    debugger;
    var productid = $("#ddl_ItemName").val();
    if (productid == "0" || productid == null) {
        $("#vmddl_ItemName").text($("#valueReq").text());
        $("#vmddl_ItemName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
    }
    else {
        $("#vmddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
    }
    
}