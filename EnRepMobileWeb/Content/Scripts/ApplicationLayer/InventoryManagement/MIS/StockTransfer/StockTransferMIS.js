$(document).ready(function () {
    $('#ddlItems').select2();
   // $('#ddlSrcBranch').select2();
   // $('#ddlSrcWarehouse').select2();
   // $('#ddlDstnBranch').select2();
   // $('#ddlDstnWarehouse').select2();
    cmn_apply_datatable("#StockTransferMISList");
    Cmn_initializeMultiselect(['#ddlTrfType', '#ddlSrcBranch', '#ddlSrcWarehouse', '#ddlDstnBranch','#ddlDstnWarehouse']);// Added By Nidhi on 17-12-2025
});
function GetStockTransferMIS() {
    try {
        var itemId = $('#ddlItems').val();
       // var mtType = $('#ddlTrfType').val();
        var mtType = cmn_getddldataasstring('#ddlTrfType');
        //var srcBranch = $('#ddlSrcBranch').val();
        var srcBranch = cmn_getddldataasstring('#ddlSrcBranch');
        /*var dstnBranch = $('#ddlDstnBranch').val();*/
        var dstnBranch = cmn_getddldataasstring('#ddlDstnBranch');
        /* var srcWarehouse = $('#ddlSrcWarehouse').val();*/
        var srcWarehouse = cmn_getddldataasstring('#ddlSrcWarehouse');
        /*var dstnWarehouse = $('#ddlDstnWarehouse').val();*/
        var dstnWarehouse = cmn_getddldataasstring('#ddlDstnWarehouse');
        var fromDate = $('#txtFromdate').val();
        var toDate = $('#txtTodate').val();
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/StockTransfer/GetStockTransferMISReport",
            data: {
                itemId: itemId,
                mtType: mtType,
                srcBranch: srcBranch,
                dstnBranch: dstnBranch,
                srcWarehouse: srcWarehouse,
                dstnWarehouse: dstnWarehouse,
                fromDate: fromDate,
                toDate: toDate
            },
            success: function (data) {
                debugger;
                $('#divStkTrf').html(data);
                cmn_apply_datatable("#StockTransferMISList");
            }
        });
    } catch (err) {
        debugger;
        console.log("stock movement  Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";

    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode)

}
function OnChangeFromBranch() {

}
function BindSrcWarehouseByBrId() {
    debugger;
    var srcBranch = $('#ddlSrcBranch').val();
    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/StockTransfer/GetSrcWarehouseList",
        data: {
            brId: srcBranch
        },
        success: function (data) {
            var items = JSON.parse(data);
            var dropdown = $('#ddlSrcWarehouse');
            dropdown.empty();
            dropdown.append($('<option></option>').attr('value', "0").text("---ALL---"));
            $.each(items, function (i, item) {
                dropdown.append($('<option></option>').attr('value', item.wh_id).text(item.wh_name));
            });
        },
        error: function (xhr, status, error) {
            console.error('Error fetching warehouses:', status, error);
        }
    });
}
function BindDstnWarehouseByBrId() {
    debugger;
    var srcBranch = $('#ddlDstnBranch').val();
    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/StockTransfer/GetSrcWarehouseList",
        data: {
            brId: srcBranch
        },
        success: function (data) {
            var items = JSON.parse(data);
            var dropdown = $('#ddlDstnWarehouse');
            dropdown.empty();
            dropdown.append($('<option></option>').attr('value', "0").text("---ALL---"));
            $.each(items, function (i, item) {
                dropdown.append($('<option></option>').attr('value', item.wh_id).text(item.wh_name));
            });
        },
        error: function (xhr, status, error) {
            console.error('Error fetching warehouses:', status, error);
        }
    });
}
function BindUom() {
    debugger;
    var itemId = $('#ddlItems').val();
    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/StockTransfer/GetUomNoByItemId",
        data: {
            itemId: itemId
        },
        success: function (data) {
            var items = JSON.parse(data);
            $.each(items, function (i, item) {
                $('#txtuomid').val(item.uom_name);
            });
        },
        error: function (xhr, status, error) {
            console.error('Error fetching warehouses:', status, error);
        }
    });
}
function GetStkTrfPopupData(e, actflag) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hfItemID").val();
    var srcBranch = clickedrow.find("#hfFromBr").val();
    var dstnBranch = clickedrow.find("#hfToBr").val();
    var srcWarehouse = clickedrow.find("#hfFromWh").val();
    var dstnWarehouse = clickedrow.find("#hfToWh").val();
    var mtType = clickedrow.find("#hfTrfType").val();
    var fromDate = $('#txtFromdate').val();
    var toDate = $('#txtTodate').val();

    var itemName = clickedrow.find('#hfItemName').val();
    var uom = clickedrow.find('#hfUOM').val();
    var srcBr = clickedrow.find('#hfFromBrName').val();
    var dstnBr = clickedrow.find('#hfToBrName').val();
    var srcWh = clickedrow.find('#hfFromWhName').val();
    var dstnWh = clickedrow.find('#hfToWhName').val();
  /*  var transType = $('#hfTransferType').val();*/
    var trfQty = clickedrow.find('#hfTrfQty').val();
    var issQty = clickedrow.find('#hfIssueQty').val();
    var recQty = clickedrow.find('#hfRcptQty').val();

    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/StockTransfer/GetStockTransferPopupData",
        data: {
            actflag: actflag,
            itemId: itemId,
            mtType: mtType,
            srcBranch: srcBranch,
            dstnBranch: dstnBranch,
            srcWarehouse: srcWarehouse,
            dstnWarehouse: dstnWarehouse,
            fromDate: fromDate,
            toDate: toDate
        },
        success: function (data) {
            debugger;
            if (actflag == 'Req') {
                $('#divPartialMISRequisitionQuantityDetail').html(data);
                $('#txtItmName1').val(itemName);
                $('#batch_UOM1').val(uom);
               // $('#batch_UOM').val(itemName);
                $('#txtSrcBr1').val(srcBr);
                $('#txtSrcWh1').val(srcWh);
                $('#txtDstnBr1').val(dstnBr);
                $('#txtDstnWh1').val(dstnWh);
                $('#RequisitionQuantity1').val(trfQty);
             /*   cmn_apply_datatable("#datatable-buttons1");*/
            }
            if (actflag == 'Trf') {

                $('#divPartialMISTransferQuantityDetail').html(data);
                $('#txttItmName').val(itemName);
                $('#batcht_UOM').val(uom);
               // $('#batch_UOM').val(itemName);
                $('#txttSrcBr').val(srcBr);
                $('#txttSrcWh').val(srcWh);
                $('#txttDstnBr').val(dstnBr);
                $('#txttDstnWh').val(dstnWh);
                $('#tIssuedQuantity').val(issQty);
                /*cmn_apply_datatable("#datatable-buttons1");*/
            }
            if (actflag == 'Rcpt')
            {
                $('#divPartialMISReceiptQuantityDetail').html(data);
                $('#txtrItmName').val(itemName);
                $('#batchr_UOM').val(uom);
               // $('#batch_UOM').val(itemName);
                $('#txtrSrcBr').val(srcBr);
                $('#txtrSrcWh').val(srcWh);
                $('#txtrDstnBr').val(dstnBr);
                $('#txtrDstnWh').val(dstnWh);
                $('#rReceiptQuantity').val(recQty);
               /* cmn_apply_datatable("#datatable-buttons1");*/
            }
        }
    });
}