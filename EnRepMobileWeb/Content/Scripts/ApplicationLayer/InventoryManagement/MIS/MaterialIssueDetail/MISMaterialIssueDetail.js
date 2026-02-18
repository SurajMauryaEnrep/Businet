$(document).ready(function () {
    $('#ddlItems').select2();
    //$('#ddlReqArea').select2();
    //$('#ddlTransferType').select2();
    //$('#ddlDstnBranch').select2();
    //$('#ddlDstnWarehouse').select2();
    //$('#ddlIssueTo').select2();
    BindRequirementAreaList();
    BindToBranchList();
    BindToWhList();
    BindIssueToList();
    BindIssueToList();
    BindItemsList();
    BindReport();
    Cmn_initializeMultiselect(['#ddlTransferType']);// Added By Nidhi on 16-12-2025
});
function ReplaceId() {
    //try {
    //    $("#datatable-buttons")[0].id = "dttbl";
    //}
    //catch { }
    try {
        $("#datatable-buttons1")[0].id = "dttbl1";
    }
    catch { }
    try {
        $("#datatable-buttons2")[0].id = "dttbl2";
    }
    catch { }
    //try {
    //    $("#datatable-buttons4")[0].id = "dttbl4";
    //}
    //catch { }
}
function OnChangeIssueType() {
    /*debugger*/
    var issueType = $('#IssueType').val();
    if (issueType == "E") {
        $('#divItems').css('display', 'block');
        $('#divReqAreas').css('display', 'block');
        $('#divTransferType').css('display', 'none');
        $('#divDestinationBranch').css('display', 'none');
        $('#divDestinationWarehouse').css('display', 'none');
        $('#divIssueTo').css('display', 'block');
    }
    else if (issueType == "M") {
        $('#divItems').css('display', 'block');
        $('#divReqAreas').css('display', 'none');
        $('#divTransferType').css('display', 'block');
        $('#divDestinationBranch').css('display', 'block');
        $('#divDestinationWarehouse').css('display', 'block');
        $('#divIssueTo').css('display', 'none');
    }
    else if (issueType == "S") {
        $('#divItems').css('display', 'block');
        $('#divReqAreas').css('display', 'block');
        $('#divTransferType').css('display', 'none');
        $('#divDestinationBranch').css('display', 'none');
        $('#divDestinationWarehouse').css('display', 'none');
        $('#divIssueTo').css('display', 'block');
    }
    else {
        $('#divItems').css('display', 'block');
        $('#divReqAreas').css('display', 'block');
        $('#divTransferType').css('display', 'none');
        $('#divDestinationBranch').css('display', 'none');
        $('#divDestinationWarehouse').css('display', 'none');
        $('#divIssueTo').css('display', 'none');
    }

};
function BindRequirementAreaList() {
    var issueType = $('#IssueType').val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetReqAreaList",
        data: {
            issueType: issueType
        },
        success: function (data) {
            /*debugger;*/
            $("#ddlReqArea").empty();
            var arr = [];
            arr = JSON.parse(data);
            /*var values = '<option value="0">---ALL---</option>';*/
            var values = '';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    values += '<option value="' + arr[i].setup_id + '">' + arr[i].setup_val + '</option>';
                }
            }
            $("#ddlReqArea").html(values);
            Cmn_initializeMultiselect(['#ddlReqArea']);// Added By Nidhi on 16-12-2025
            $('#ddlReqArea').multiselect('rebuild');
            /*alert(data);*/
        }
    })
}
function BindToBranchList() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetToBranchList",
        data: {},
        success: function (data) {
            /*debugger;*/
            $("#ddlDstnBranch").empty();
            var arr = [];
            arr = JSON.parse(data);
           // var values = '<option value="0">---ALL---</option>';
            var values = '';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    values += '<option value="' + arr[i].Comp_Id + '">' + arr[i].comp_nm + '</option>';
                }
            }
            $("#ddlDstnBranch").html(values);
            Cmn_initializeMultiselect(['#ddlDstnBranch']);// Added By Nidhi on 16-12-2025
            $('#ddlDstnBranch').multiselect('rebuild');
            /*alert(data);*/
        }
    })
}
function BindToWhList() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetToWarehouseList",
        data: {},
        success: function (data) {
            /*debugger;*/
            $("#ddlDstnWarehouse").empty();
            var arr = [];
            arr = JSON.parse(data);
           // var values = '<option value="0">---ALL---</option>';
            var values = '';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    values += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
                }
            }
            $("#ddlDstnWarehouse").html(values);
            Cmn_initializeMultiselect(['#ddlDstnWarehouse']);// Added By Nidhi on 16-12-2025
            $('#ddlDstnWarehouse').multiselect('rebuild');
            /*alert(data);*/
        }
    })
}
function BindIssueToList() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetIssueToList",
        data: {},
        success: function (data) {
            debugger;
            $("#ddlIssueTo").empty();
            var arr = [];
            arr = JSON.parse(data);
       /*var values = '<option value="0">---ALL---</option>';*/
            var values = '';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    values += '<option value="' + arr[i].id + '">' + arr[i].val + '</option>';
                }
            }
            $("#ddlIssueTo").html(values);
            Cmn_initializeMultiselect(['#ddlIssueTo']);// Added By Nidhi on 16-12-2025
            $('#ddlIssueTo').multiselect('rebuild');
            /*alert(data);*/
        }
    })
}
function BindItemsList() {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetItemsList",
        data: {},
        success: function (data) {
            debugger;
            $("#ddlItems").empty();
            var arr = [];
            arr = JSON.parse(data);
            var values = '<option value="0">---ALL---</option>';
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    values += '<option value="' + arr[i].Item_id + '">' + arr[i].Item_name + '</option>';
                }
            }
            $("#ddlItems").html(values);
            /*alert(data);*/
        }
    })
}
function Bind_S_I_E_IssueReport() {
    var issueType = $('#IssueType').val();
    var itemId = $('#ddlItems').val();
    /*var reqArea = $('#ddlReqArea').val();*/
    var reqArea = cmn_getddldataasstring('#ddlReqArea');
    var fromDate = $('#txtfromdate').val();
    var toDate = $('#txttodate').val();
    //var transferType = $('#ddlTransferType').val();
    //var dstnBranch = $('#ddlDstnBranch').val();
    //var dstnWarehouse = $('#ddlDstnWarehouse').val();
   // var issueTo = $('#ddlIssueTo').val();
    var issueTo = cmn_getddldataasstring('#ddlIssueTo');
    ReplaceId();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/Get_S_I_E_issueReport",
        data: {
            issueType: issueType,
            itemId: itemId,
            reqArea: reqArea,
            fromDate: fromDate,
            toDate: toDate,
            issueTo: issueTo
        },
        success: function (data) {
            /*debugger;*/
            $('#MIS_InternalIssue').html(data);
            try {
                $("#dttbl1")[0].id = "datatable-buttons1";
                $("#dttbl2")[0].id = "datatable-buttons2";
                /*$("#dttbl4")[0].id = "datatable-buttons4";*/
            }
            catch { }
        }

    });
}
function BindMaterialTransferIssueReport() {
    var issueType = $('#IssueType').val();
    var itemId = $('#ddlItems').val();
    var reqArea = $('#ddlReqArea').val();
    var fromDate = $('#txtfromdate').val();
    var toDate = $('#txttodate').val();
    //var transferType = $('#ddlTransferType').val();
    //var dstnBranch = $('#ddlDstnBranch').val();
    //var dstnWarehouse = $('#ddlDstnWarehouse').val();
    var transferType = cmn_getddldataasstring('#ddlTransferType');
    var dstnBranch = cmn_getddldataasstring('#ddlDstnBranch');
    var dstnWarehouse = cmn_getddldataasstring('#ddlDstnWarehouse');
    var issueTo = $('#ddlIssueTo').val();
    ReplaceId();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetMaterialTransferIssueReport",
        data: {
            itemId: itemId,
            fromDate: fromDate,
            toDate: toDate,
            transferType: transferType,
            destinationBranch: dstnBranch,
            destinationWarehouse: dstnWarehouse,
            issueTo: issueTo
        },
        success: function (data) {
            /*debugger;*/
            $('#MIS_MaterialTransferIssue').html(data);
        }

    });
}
function BindReport() {
    var issueType = $('#IssueType').val();
    if (issueType == "M") {
        BindMaterialTransferIssueReport();
    }
    else {
        Bind_S_I_E_IssueReport();
    }
}
function BindItemInfoBtnClick(e) {
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdnItemId").text();
    ItemInfoBtnClick(itemId);
}
function SubItemDetailsPopUp(act, e) {
    var qtydigit = $("#QtyDigit").text();
    var clickdRow = $(e.target).closest('tr');

    //var wh_id = clickdRow.find("#RtWhSubItm_WhId").val();
    //var WhName = clickdRow.find("#RtWhSubItm_WhName").val();
    //var WhType = clickdRow.find("#RtWhSubItm_WhType").val();

    //var ProductNm = $("#IconItemName").val();
    //var ProductId = $("#RQhdItemId").val();
    //var UOM = $("#IconUOM").val();
    //var GRNNo = $("#IconGRNNumber").val();
    //var GRNDate = $("#IconGRNDate").val();
    //var GRNDt = GRNDate.split("-").reverse().join("-");

    //var Doc_no = $("#ReturnNumber").val();
    //var Doc_dt = $("#txtReturnDate").val();
    //var src_doc_no = $("#hd_doc_no").val();
    //var QtyDecDigit = $("#QtyDigit").text();
    //var Sub_Quantity = 0;
    //var NewArr = new Array();
    var ProductNm = clickdRow.find("#ItemName").text();
    var Quantity = 0;
    var uom = clickdRow.find("#uomName").text();
    if (act == "ExterPendingQty"  ) {
        Quantity = clickdRow.find("#pend_qtyQty").text();
    }
    else if (act == "ExterRec_qty"){
        Quantity = clickdRow.find("#rec_qtyQty").text();
    }
    else {
        Quantity = clickdRow.find("#issueQty").text();
    }
   
    var itemId = clickdRow.find("#hdnItemId").text();
    //var UOM = clickdRow.find("#UOM").val();
    var issueNo = clickdRow.find("#tdIssueNo").text();
    var issueDate = clickdRow.find("#tdIssuedate").text();
    //var GRNDt = GRNDate.split("-").reverse().join("-");
    //Sub_Quantity = clickdRow.find("#GRNQuantity").val();
    //var IsDisabled = "Y";

    //var hd_Status = $("#hdPRTStatus").val();
    //hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssueDetail/GetSubItemDetails",
        data: {
            act: act,
            issueNo: issueNo,
            issueDate: issueDate,
            itemId: itemId
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $('#Sub_ProductlName').val(ProductNm);
            $('#Sub_serialUOM').val(uom);
            $("#Sub_Quantity").val(Quantity);
        }
    });

}
