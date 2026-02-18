/************************************************
Javascript Name:MRS List
Created By:Mukesh Sharma
Created Date: 12-06-2021
Description: This Javascript use for the MRS List Page

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    var Doc_no = $("#MRSNumber").val();
    $("#hdDoc_No").val(Doc_no);
    $("#EntityName").select2();
    $("#ddlRequiredArea").select2();

    $("#MRSListTbl #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val()
            var WF_status = $('#WF_status').val()
            var clickedrow = $(e.target).closest("tr");
            var MRSId = clickedrow.children("#MRSNo").text();
            var MRSDt = clickedrow.children("#MRS_Dt").text();
            if (MRSId != null && MRSId != "") {
                window.location.href = "/ApplicationLayer/MaterialRequisitionSlip/EditMRS/?MRSId=" + MRSId + "&MRSDt=" + MRSDt + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }
            
        }
        catch (err) {
            debugger
            //alert(err.message);
        }
    });

    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var MRSId = clickedrow.children("#MRSNo").text();
        var MRS_Date = clickedrow.children("#MRS_Dt").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(MRSId);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(MRSId, MRS_Date, Doc_id, Doc_Status);

    });
    //ListRowHighLight();
   // BindDDlList();
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList == "S") {
        DynamicSerchableItemDDL("", "#itemNameList", "", "", "", "MRS_SI");

    }
    else {
        DynamicSerchableItemDDL("","#itemNameList", "", "", "", "MRS");
    }
  /*  DynamicSerchableItemDDL("", "#itemNameList", "", "", "", "AllQcItem")*/
});
//function BindDDlList() {
//    debugger;
//    $("#EntityName").select2({
//        ajax: {
//            url: $("#GetAutoCompleteEntity").val(),
//            data: function (params) {
//                var queryParameters = {
//                    ddlissueto: params.term // search term like "a" then "an"
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    GRN_ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}

function OnChangeIssueto() {
    debugger;
    var IssueTo = $("#EntityName option:selected").text();
    if (IssueTo != 'All') {
        var test = IssueTo.split('(')
        var test1 = test[0];
        var test2 = test[1];
        var Entitype = test2.replace(")", "");
        $("#hdEntitype").val(Entitype);
    }
    else {
        $("#vmEntity").css("display", "none");
        /*$(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");*/
        $("EntityName").css("border-color", "#ced4da");
        $("#vmEntity").text("");
    }
}

function BtnSearch() {
    debugger;
    FilterMRSList();
    ResetWF_Level();
}

function FilterMRSList() {
    debugger;
    try {
        var ReqArea = $("#ddlRequiredArea").val();
        var IssueTo1 = $("#EntityName").val();
        var IssueTo = IssueTo1.replace("S", "").replace("C", "");
        var EntityType = $("#hdEntitype").val();
        var MRS_Type = $("#ddlRequisitionTypeList option:selected").val();
        var SRC_Type = $("#ddlSourceTypeList option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var ItemID = $("#itemNameList option:selected").val();
        var ItemName = $("#itemNameList option:selected").text();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequisitionSlip/SearchMRSDetail",
            data: {
                req_area: ReqArea,
                issue_to: IssueTo,
                EntityType: EntityType,
                MRS_Type: MRS_Type,
                SRC_Type: SRC_Type,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                ItemID: ItemID
            },
            success: function (data) {
                debugger;
                $('#MRSListTbl').html(data);
                $('#ListFilterData').val(ReqArea + ',' + IssueTo + ',' + EntityType + ',' + MRS_Type
                    + ',' + SRC_Type + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + ItemID + ',' + ItemName);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MRS Error : " + err.message);

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
function OnChangeRequisitnTyp() {
    debugger;
    $('#EntityName').val("0").trigger('change');
    var MRS_Type = $("#ddlRequisitionTypeList option:selected").val();
    if (MRS_Type == "I") {
        $("#ddlSourceTypeList").prop('disabled', false);
        $("#EntityName").prop('disabled', true);
    }
    else {
        $('#ddlSourceTypeList').val(0);
        $("#ddlSourceTypeList").prop('disabled', true);
    
        $("#EntityName").prop('disabled', false);
        
    }
}

function OnClickMRSTrackingBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var MRS_No = clickedrow.children("#MRSNo").text();
    var MRS_Date = clickedrow.children("#MRS_Dt").text();
    var tblmrsdt = clickedrow.children("#tblmrsdt").text();
    var ddlreqAreaid = clickedrow.children("#ddlreqAreaid").text();
    if (MRS_No != null && MRS_No != "" && MRS_Date != null && MRS_Date != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialRequisitionSlip/GetMRSTrackingDetail",
            data: { MRS_No: MRS_No, MRS_Date: MRS_Date },
            //dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }

                $("#trackingMRS").html(data);

                cmn_apply_datatable("#MRS_trackingTBL");

                $("#MRSOrderNumber").val(MRS_No);
                $("#MRSOrderNumber").text(MRS_No);
                $("#MRSOrderDate").text(tblmrsdt);
                $("#MRSOrderDate").val(tblmrsdt);
                $("#ddltackingRequirmentArea").val(ddlreqAreaid);
            }
        });
    }

}