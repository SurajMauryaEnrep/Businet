/************************************************
Javascript Name:QC Inspection List
Created By:Mukesh Sharma
Created Date: 27-04-2021
Description: This Javascript use for the QC Inspection List Page

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    DynamicSerchableItemDDL("", "#itemNameList", "", "", "", "AllQcItem")

    $("#QCInspectionList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var DocumentMenuId = $("#DocumentMenuId").val();
            var WF_status = $("#WF_status").val();
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr"); 
            var QCId = clickedrow.find("#tbl_Qc_no").text();
            var QCDt = clickedrow.find("#tbl_Qc_dt").text();
            var QCType = clickedrow.find("#tbl_Qc_Type").text();
            if (QCId != null && QCId != "" || QCType != null && QCType != "") {
                window.location.href = "/ApplicationLayer/QualityInspection/EditQualityInspection/?QCId=" + QCId + "&QCDt=" + QCDt + "&QCType=" + QCType + "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId + "&WF_status=" + WF_status;
            }      
        }
        catch (err) {
            debugger      
        }
    });

    //ListRowHighLight();

});
function GetDocWiseWorkflowBar(e) {

    var clickedrow = $(e.target).closest("tr");
    var Src_Type = clickedrow.children("#tbl_Qc_Type").text();
    sessionStorage.setItem("ShowLoader", "N");//Added by Suraj Maurya on 29-08-2025
    $.ajax({
        url: "/ApplicationLayer/QualityInspection/SetDocMenuID",
        data: { Src_Type },
        success: function (data) {
            debugger
            var arr = data.split(",");
            if (arr[0] != "") {
                $("#DocumentMenuId").val(arr[0]);
                var WFBar = '<ul class="wizard_steps">';
                if (arr[1] > 0) {
                    for (var i = 1; i <= arr[1]; i++) {
                        WFBar += `
 <li>
                <a href="#step-1" class="disabled" isdone="1" onclick="ForwardBarHistoryClick()" data-toggle="modal" data-target="#WorkflowInformation" data-backdrop="static" data-keyboard="false" rel="1" id="a_${i}">
                    <span class="step_no">${i}</span>
                </a>
            </li>
`;
                    }
                }
                WFBar += '</ul>'
                WFBar += '<input type="hidden" id="hdDoc_No" value="" />';
                $("#wizard").html(WFBar);
                CmnGetWorkFlowDetails(e);
            }
        }
    })
}
function CmnGetWorkFlowDetails(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var QC_No = clickedrow.children("#tbl_Qc_no").text();
    var QC_Date = clickedrow.children("#tbl_Qc_dt").text();
    var Doc_Menu_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(QC_No);
    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(QC_No, QC_Date, Doc_Menu_id, Doc_Status);
    var a = 1;
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
        var ItemID = "0";
        var Item_Name = "---Select---";
        var QC_Type = $("#Source_type option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105102120" || DocumentMenuId == "105105135" ) {
             ItemID = $("#itemNameList option:selected").val();
             Item_Name = $("#itemNameList option:selected").text();
        }
       
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QualityInspection/SearchQCDetail",
            data: {
                QC_Type: QC_Type,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                DocumentMenuId: DocumentMenuId,
                ItemID: ItemID
            },
            success: function (data) {
                debugger;
                $('#QCInspectionList').html(data);
                $('#ListFilterData').val(QC_Type + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + ItemID + ',' + Item_Name);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Quality Inspection Error : " + err.message);

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