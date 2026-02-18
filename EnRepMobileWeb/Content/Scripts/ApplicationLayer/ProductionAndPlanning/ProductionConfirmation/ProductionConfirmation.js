$(document).ready(function () {
    debugger;
    $("#ProductName").select2();
    BindProductNameDDL();
    var CnfNo= $("#ddlOrderNumber").val();/*add by Hina on 20-09-2024 to bind dropdown */
    if (CnfNo != "" && CnfNo != null) {
        var Shflid = $("#hdn_shopfloor_id").val();
        BindWorkStation(Shflid)
    }
    var Doc_No = $("#Txt_ConfirmationNo").val();
    $("#hdDoc_No").val(Doc_No);
    $("#tbodyproductioncnfList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var CnfNumber = clickedrow.children("#cnf_no").text();
            var CnfDate = clickedrow.children("#cnf_dt").text();
            if (CnfNumber != null && CnfNumber != "" && CnfDate != null && CnfDate != "") {
                window.location.href = "/ApplicationLayer/ProductionConfirmation/ProductionConfirmation_Edit/?Cnf_Number=" + CnfNumber + "&Cnf_Date=" + CnfDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var CnfNumber = clickedrow.children("#cnf_no").text();
        var CnfDate = clickedrow.children("#cnf_dt").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(CnfNumber, CnfDate, Doc_id, Doc_Status);
        $("#hdDoc_No").val(CnfNumber);
    });

    $('#ConsumptionItmDetailsTbl').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            //var dig = parseInt(id.substring(1));
            var dig = parseInt(id);
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        debugger;
        var Itemid = $(this).closest('tr').find("#hdn_citem").val();
        Cmn_DeleteSubItemQtyDetail(Itemid);
        DeleteItemBatchSerialOrderQtyDetails(Itemid);
        $(this).closest('tr').remove();
       
        updateItemSerialNumber()
    });
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#ConsumptionItmDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hdn_citem').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});

function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#Sno").text(SerialNo);

    });
};
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    debugger;
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#Batch_ItemId").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#Serial_ItemId").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}
var QtyDecDigit = $("#QtyDigit").text();

//----------------------WorkFlow------------------------------

function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#hdn_Status').val().trim();
    //if (OrderStatus === "D" || OrderStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var PrdCnfDt = $("#Txt_ConfirmationDT").val();
    $.ajax({
        type: "POST",
        /*  url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: PrdCnfDt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var OrderStatus = "";
                OrderStatus = $('#hdn_Status').val().trim();
                if (OrderStatus === "D" || OrderStatus === "F") {

                    if ($("#hd_nextlevel").val() === "0") {
                        $("#Btn_Forward").attr("data-target", "");
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                        $("#Btn_Approve").attr("data-target", "");
                    }
                    var Doc_ID = $("#DocumentMenuId").val();
                    $("#OKBtn_FW").attr("data-dismiss", "modal");

                    Cmn_GetForwarderList(Doc_ID);

                }
                else {
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Forward").attr('onclick', '');
                    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                }
            }
            else {/* to chk Financial year exist or not*/
                /*   swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Btn_Approve").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");

            }
        }
    });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#Txt_ConfirmationNo").val();
    DocDate = $("#Txt_ConfirmationDT").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (DocNo + ',' + DocDate + ',' + WF_Status1);
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ProductionConfirmation_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/ProductionConfirmation/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionConfirmation/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ cnf_no: DocNo, cnf_dt: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ProductionConfirmation/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionConfirmation/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ProductionConfirmation/ToRefreshByJS?ListFilterData1 =" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(confirmationNo, confirmationDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/ProductionConfirmation/SavePdfDocToSendOnEmailAlert",
//        data: { confirmationNo: confirmationNo, confirmationDate: confirmationDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#Txt_ConfirmationNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)

        var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}

//----------------------WorkFlow End------------------------------

function onclickcancilflag() {

    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").prop('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").prop('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
};
function BindProductNameDDL() {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID == null || Itm_ID == "" || Itm_ID == "0") {

        BindItemList("#ddl_ProductName", "", "", "", "", "PRDORD");
        //$.ajax(
        //    {
        //        type: "POST",
        //        url: "/ApplicationLayer/ProductionOrder/GetProductNameInDDL",
        //        data: function (params) {
        //            var queryParameters = {
        //                SO_ItemName: params.term
        //            };
        //            return queryParameters;
        //        },
        //        dataType: "json",
        //        success: function (data) {
        //            debugger;
        //            if (data == 'ErrorPage') {
        //                ErrorPage();
        //                return false;
        //            }
        //            if (data !== null && data !== "") {
        //                $('#ddl_ProductName').empty();
        //                var arr = [];
        //                arr = JSON.parse(data);
        //                if (arr.Table.length > 0) {
        //                    //sessionStorage.removeItem("PLitemList");
        //                    //sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
        //                    $('#ddl_ProductName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        //                    for (var i = 0; i < arr.Table.length; i++) {
        //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
        //                    }
        //                    var firstEmptySelect = true;
        //                    $('#ddl_ProductName').select2({
        //                        templateResult: function (data) {
        //                            var UOM = $(data.element).data('uom');
        //                            var classAttr = $(data.element).attr('class');
        //                            var hasClass = typeof classAttr != 'undefined';
        //                            classAttr = hasClass ? ' ' + classAttr : '';
        //                            var $result = $(
        //                                '<div class="row">' +
        //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
        //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
        //                                '</div>'
        //                            );
        //                            return $result;
        //                            firstEmptySelect = false;
        //                        }
        //                    });
        //                    debugger;
        //                    var productid = $("#hdn_product_id").val();
        //                    var orderNo = $("#hd_order_no").val();
        //                    var orderdate = $("#hd_order_dt").val();
        //                    if (productid != null && productid != "") {
        //                        $('#ddl_ProductName').val(productid).trigger('change');
        //                    }
        //                    if (orderNo != null && orderNo != "") {
        //                        //$('#ddlOrderNumber').val(orderNo).trigger('change');
        //                        $('#SupplierName').empty().append('<option value=' + orderNo + ' selected="selected">' + orderNo + '</option>');
        //                        $('#Txt_OrderDate').val(orderdate);
        //                    }
        //                }
        //            }
        //        },
        //    });
    }
}
function OnChangeProductName() {
    debugger;
    var Item_id = $("#ddl_ProductName").val();
    if (Item_id == "0") {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
    }
    else {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    if (Item_id != "" && Item_id != "0" && Item_id != null) {
        $("#hdn_product_id").val(Item_id);
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ProductionConfirmation/GetOrderLists",
                data: { Product_id: Item_id },
                success: function (data) {
                    debugger;
                    ClearTxtData();
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#ddlOrderNumber option").remove();
                            $("#ddlOrderNumber optgroup").remove();
                            $('#ddlOrderNumber').append(`<optgroup class='def-cursor' id="Textddlorder" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'
                                data-item='${$("#Outputproduct").text()}'                                  ></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#Textddlorder').append(`<option data-date="${arr.Table[i].jc_dt}" value="${arr.Table[i].jc_dt}"  data-item="${arr.Table[i].item_name}"
                        >${arr.Table[i].jc_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#ddlOrderNumber').select2({
                                templateResult: function (data) {
                                    var DocDate = $(data.element).data('date');
                                    var item = $(data.element).data('item');
                                    var classAttr = $(data.element).attr('class');
                                    var hasClass = typeof classAttr != 'undefined';
                                    classAttr = hasClass ? ' ' + classAttr : '';
                                    var $result = $(
                                        '<div class="row">' +
                                        '<div class="col-md-5 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                        '<div class="col-md-3 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                        ' <div class="col-md-4 col-xs-12' + classAttr + '">' + item + '</div>' +
                                        '</div>'
                                    );
                                    return $result;
                                    firstEmptySelect = false;
                                }
                            });

                            $("#Txt_OrderDate").val("");
                        }
                        if (arr.Table1.length > 0) {
                            $("#Txt_UOM").val(arr.Table1[0].uom_alias);
                            $("#hdn_uom_id").val(arr.Table1[0].uom_id);
                        }
                        else {
                            $("#Txt_UOM").val("");
                            $("#hdn_uom_id").val("");
                        }
                    }
                },
            });
        } catch (err) {
            console.log("Error : " + err.message);
        }
    }
}
function OnChangeOrderNo() {
    debugger;
    //var QtyDigit = $("#QtyDigit").text();
    var Order_No = $("#ddlOrderNumber option:selected").text();
    var Order_Dt = $("#ddlOrderNumber").val();
    var OrderNo = $("#ddlOrderNumber").val();
    if (OrderNo != "0") {
        var date = Order_Dt.split("-");
        var FDate = date[2] + '-' + date[1] + '-' + date[0];
    }

    $("#Txt_OrderDate").val(FDate);
    $("#hd_order_no").val(Order_No);

    if (OrderNo == "0") {
        $('#vm_order_no').text($("#valueReq").text());
        $("#vm_order_no").css("display", "block");
        $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "red");
        $("#Txt_OrderDate").val("");
    }
    else {
        $("#vm_order_no").css("display", "none");
        $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    }
    if (Order_No != "" && Order_No != "0" && Order_No != null && Order_Dt != "" && Order_Dt != "0" && Order_Dt != null) {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ProductionConfirmation/GetOrderDetail",
                data: { OrderNo: Order_No, OrderDate: FDate, Flag: "HD" },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {

                            $("#Txt_OrderQty").val(arr.Table[0].OrderQty);
                            $("#PendingQuantity").val(arr.Table[0].pending_qty);
                            $("#Txt_Operation").val(arr.Table[0].op_name);
                            $("#hdn_operation_id").val(arr.Table[0].op_id);
                            $("#Txt_Shopfloor").val(arr.Table[0].shfl_name);
                            $("#hdn_shopfloor_id").val(arr.Table[0].shfl_id);
                            //$("#Txt_Workstation").val(arr.Table[0].ws_name);/*Start commented and add by Hina on 20-09-2024 for bind dropdown of it*/
                            //$("#hdn_workstation_id").val(arr.Table[0].ws_id);
                            /*$('#ddl_WorkstationName option:selected').val(0);*/
                            $('#hdn_WorkstationName').val(arr.Table[0].ws_id);
                            $('#hdn_WorkstationText').val(arr.Table[0].ws_name);
                            var ddl_ShopfloorName = $("#hdn_shopfloor_id").val();
                             BindWorkStation(ddl_ShopfloorName)
                            $("#span_vm_ddl_WorkstationName").css("display", "none");
                            $("#ddl_WorkstationName").css("border-color", "#ced4da")
                            /*------End code by Hina on 20-09-2024-------*/

                            $("#Txt_Supervisor").val(arr.Table[0].supervisor_name);
                            //$("#Txt_BatchNo").val(arr.Table[0].batch_no);
                            // $('#ddlshiftID').empty().append('<option value=' + arr.Table[0].shift_id + ' selected="selected">' + arr.Table[0].ShiftName + '</option>');

                            $("#Txt_Shift").val(arr.Table[0].ShiftName);
                            $("#hdn_shift_id").val(arr.Table[0].shift_id);
                            $("#ddlshiftID").val(arr.Table[0].shift_id);

                            $("#OutputItemID").val(arr.Table[0].op_out_item_id);
                            $("#OutputItemName").val(arr.Table[0].Output_Item_Name);

                            $("#OutputUOMID").val(arr.Table[0].op_out_uom_id);
                            $("#OutputUOMName").val(arr.Table[0].uom_alias);
                            $("#OutputUOM").val(arr.Table[0].uom_alias);

                            HideShowPageWise1(arr.Table[0].sub_item, 'NoRow');
                        }
                    }
                },
            });
        } catch (err) {
            console.log("Error : " + err.message);
        }
    }
}
function BindWorkStation(ddl_ShopfloorName) {/*Add by Hina on 20-09-2024 to add for dropdown*/
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionOrder/BindWorkStationList",
            data: {
                shfl_id: ddl_ShopfloorName,
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    var WorkStationId = $("#hdn_WorkstationName").val();
                    var WorkStationName = $("#hdn_WorkstationText").val();
                    var Cnfno = $("#ddlOrderNumber").val();
                    if (Cnfno != "" && Cnfno != null) {
                        $('#ddl_WorkstationName').empty();
                    }
                    
                    arr = JSON.parse(data);

                    if (arr.Table.length > 0) {
                        if (Cnfno != "" && Cnfno != null) {
                            //$('#ddl_WorkstationName').append(`<option value=0>---Select---</option>`);
                            var s = '<option value="0">---Select---</option>';
                        }
                       
                        for (var i = 0; i < arr.Table.length; i++) {
                            //$('#ddl_WorkstationName').append(`<option value="${arr.Table[i].ws_id}">${arr.Table[i].ws_name}</option>`);
                            s += '<option value="' + arr.Table[i].ws_id + '">' + arr.Table[i].ws_name + '</option>';
                            
                        }
                        $('#ddl_WorkstationName').html(s);
                    }
                    else {
                        var s = '<option value="0">---Select---</option>';
                        $('#ddl_WorkstationName').html(s);
                        //$('#ddl_WorkstationName').append(`<option value=0>---Select---</option>`);
                    }
                    $('#ddl_WorkstationName').val(WorkStationId);
                    $("#ddl_WorkstationText").val(WorkStationName);
                }
            },
        });
}
function ddl_WorkstationName_onchange(e) {/*Add by Hina on 20-09-2024 to add for dropdown*/
    debugger;
    var ddl_WorkstationName = $("#ddl_WorkstationName").val();
    $("#hdn_WorkstationName").val(ddl_WorkstationName);
    var WorkstationName = $("#ddl_WorkstationName option:selected").text();
    $("#hdn_WorkstationText").val(WorkstationName);
    if (ddl_WorkstationName != "0") {
        $("#span_vm_ddl_WorkstationName").css("display", "none");
        $("#ddl_WorkstationName").css("border-color", "#ced4da")
    }
    else {
        $('#span_vm_ddl_WorkstationName').text($("#valueReq").text());
        $("#span_vm_ddl_WorkstationName").css("display", "block");
        $("#ddl_WorkstationName").css("border-color", "red")
    }
};
function ddl_ddl_shift_onchange() {
    debugger;
    var shiftid = $("#ddlshiftID").val();
    $("#hdn_shift_id").val(shiftid);

}
function ClearTxtData() {
    $("#Txt_OrderQty").val("");
    $("#PendingQuantity").val("");
    $("#Txt_Operation").val("");
    $("#hdn_operation_id").val("");
    $("#Txt_Shopfloor").val("");
    $("#hdn_shopfloor_id").val("");
    $("#Txt_Workstation").val("");
    $("#hdn_workstation_id").val("");
    $("#Txt_Supervisor").val("");
    $("#Txt_Shift").val("");
    $("#hdn_shift_id").val("");
}
function JobStartDateAndTime() {
    debugger;
    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    var txt_JobEndDateAndTime = $('#txt_JobEndDateAndTime').val().trim();
    $("#hdn_JobStartDateAndTime").val(txt_JobStartDateAndTime);
    if (txt_JobStartDateAndTime != "") {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = "";
        $("#txt_JobStartDateAndTime").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobStartDateAndTime").css("border-color", "red");
    }

    if (txt_JobStartDateAndTime != "" && txt_JobEndDateAndTime != "") {
        if (ChechStartEnddate(txt_JobStartDateAndTime, txt_JobEndDateAndTime) == "") {
            return false;
        }
        else {
            $("#Txt_Hours").val(ChechStartEnddate(txt_JobStartDateAndTime, txt_JobEndDateAndTime))
        }
    }
}
function JobEndDateAndTime() {
    debugger;
    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    var txt_JobEndDateAndTime = $('#txt_JobEndDateAndTime').val().trim();
    $("#hdn_JobEndDateAndTime").val(txt_JobEndDateAndTime);

    if (txt_JobStartDateAndTime != "" && txt_JobEndDateAndTime != "") {
        if (ChechStartEnddate(txt_JobStartDateAndTime, txt_JobEndDateAndTime) == "") {
            return false;
        }
        else {
            $("#Txt_Hours").val(ChechStartEnddate(txt_JobStartDateAndTime, txt_JobEndDateAndTime))
        }
    }
    if (txt_JobEndDateAndTime != "") {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = "";
        $("#txt_JobEndDateAndTime").css("border-color", "#ced4da");

        //$("#Txt_Hours").val(time_difference.hours+":"+time_difference.minutes);
    }
    else {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");
        $("#Txt_Hours").val("");
    }
}
function ChechStartEnddate(txt_JobStartDateAndTime, txt_JobEndDateAndTime) {
    var Dates = moment(txt_JobStartDateAndTime.replace('T', ' ')).toDate();
    var Datee = moment(txt_JobEndDateAndTime.replace('T', ' ')).toDate();

    var start = moment(txt_JobStartDateAndTime.replace('T', ' ')).toDate();
    var end = moment(txt_JobEndDateAndTime.replace('T', ' ')).toDate();

    var diff = end.getTime() - start.getTime();
    var time_difference = new Object();

    time_difference.hours = Math.floor(diff / 1000 / 60 / 60);
    diff -= time_difference.hours * 1000 * 60 * 60;
    if (time_difference.hours < 10) time_difference.hours = "0" + time_difference.hours;

    time_difference.minutes = Math.floor(diff / 1000 / 60);
    diff -= time_difference.minutes * 1000 * 60;
    if (time_difference.minutes < 10) time_difference.minutes = "0" + time_difference.minutes;

    //debugger;
    //var Timediff = time_difference;

    if (Datee.getTime() < Dates.getTime()) {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#JC_InvalidDate").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");
        $("#Txt_Hours").val("");
        return "";
    }
    else {
        return time_difference.hours + ":" + time_difference.minutes;
    }
}

function ClickAddIcon_AddItemDetails() {
    debugger;
    if (CheckValidationOnfields() == true) {
        var span_subitemdetail = $("#span_SubItemDetail").text();
        var flag = "ID";
        var Order_No = $("#ddlOrderNumber option:selected").text();
        var Order_Dt = $("#ddlOrderNumber").val();
        var OrderNo = $("#ddlOrderNumber").val();
        var Shfl_ID = $("#hdn_shopfloor_id").val();
        if (OrderNo != "0") {
            var date = Order_Dt.split("-");
            var FDate = date[2] + '-' + date[1] + '-' + date[0];
        }

        $("#ddl_ProductName").attr("disabled", true);
        $("#ddlOrderNumber").attr("disabled", true);
        $("#divAddNew").css("display", "none");

        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ProductionConfirmation/GetOrderDetail",
                data: { OrderNo: Order_No, OrderDate: FDate, Flag: flag, Shflid: Shfl_ID },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);

                        if (arr.Table1.length > 0) {
                            $('#ConsumptionItmDetailsTbl tbody tr').remove();
                            for (var i = 0; i < arr.Table1.length; i++) {
                                var subitmDisable = "";
                                if (arr.Table1[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                $('#ConsumptionItmDetailsTbl tbody').append(`<tr> 
                                    <td id="Sno">${i + 1}</td>
                                     <td class=" red center"><i class="deleteIcon fa fa-trash"  aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                    <td><div class="col-sm-11 lpo_form" style="padding:0px;">
                                    <input id="CItemName" class="form-control" autocomplete="off" type="text" name="ItemName" disabled>
                                    <input type="hidden" id="hdn_citem" value="${arr.Table1[i].item_id}" style="display: none;">
                                    <input type="hidden" id="hdnConItemType" value="${arr.Table1[i].Item_type}" style="display: none;">
                                    </div><div class="col-sm-1 i_Icon">
                                    <button type="button" id="CItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${arr.Table1[i].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button>
                                    </div></td>
                                    <td><input id="CUOM" class="form-control" autocomplete="off" value='${arr.Table1[i].uom_alias}' type="text" name="UOM" disabled><input type="hidden" id="hdn_cuom" value="${arr.Table1[i].uom_id}" style="display: none;"></td>
                                    <td><input id="CAvailableStockInShopfloor" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(arr.Table1[i].avl_stock_shfl).toFixed(QtyDecDigit)}" name="AvailableStockInShopfloor" disabled></td>
                                    <td><div class="col-sm-10 lpo_form" style="padding:0px;"><div class="lpo_form">
                                    <input type="hidden" id="hdnitem_reqqty" value="${arr.Table1[i].foronereqqty}" style="display: none;">
                                    <input id="CConsumedQuantity" class="form-control num_right" maxlength="18" autocomplete="off" value="${parseFloat(arr.Table1[i].req_qty).toFixed(QtyDecDigit)}" onkeypress="return AllowFloatQtyonly(this,event)" onchange="OnChangeConsumedQty(event);" placeholder="0000.00" type="text" name="ConsumedQuantity">
                                    <span id="consumedqty_Error" class="error-message is-visible"></span></div></div>
                                    <div class="col-sm-2 i_Icon" id="div_SubItemConsumeQuantity" style="padding:0px; ">
                                    <input hidden type="text" id="sub_item" value="${arr.Table1[i].sub_item}" />
                                    <button type="button" id="SubItemConsumeQuantity" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PC_ConsumeQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}">
                                    </button>
                                    </div>
                                    <input type="hidden" id="hdn_Subitmflag" value="" style="display: none;">
                                    </td>
                                    <td class="center">
                                    <button type="button" id="btncbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                    <input type="hidden" id="hdi_cbatch" value="${arr.Table1[i].i_batch}" style="display: none;"></td>
                                    <td class="center">
                                    <button type="button" id="btncserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                    <input type="hidden" id="hdi_cserial" value="${arr.Table1[i].i_serial}" style="display: none;"></td>
                                    <td><textarea id="Cremarks" class="form-control remarksmessage" name="remarks" placeholder="${$("#span_remarks").text()}" style="height:26px;"></textarea>
                                    </td>
                                </tr>`);
                                $('#ConsumptionItmDetailsTbl tbody tr #hdn_citem[value="' + arr.Table1[i].item_id + '"]').closest('tr').find("#CItemName").val(arr.Table1[i].item_name);

                            }
                            HideAndShow_BatchSerialButton();
                        }
                        if (arr.Table.length > 0) {
                            $('#OutputItmDetailsTbl tbody tr').remove();
                            
                            let Expirable = "N";
                            for (var j = 0; j < arr.Table.length; j++) {
                                var Greyfilter = "";
                                var dsbl = "";
                                if (arr.Table[j].sub_item == "Y") {
                                    //Greyfilter = "filter:grayscale(0%)";
                                } else {
                                    //Greyfilter = "filter:grayscale(100%)";
                                    dsbl = "disabled";
                                }
                                if (arr.Table[j].i_exp == "Y") {
                                    Expirable = "Y";
                                }//Change pending_qty to req_qty by shubham Maurya on 09-06-2025
                                var item_qty = "";
                                if (arr.Table[j].Item_type == "OF" || arr.Table[j].Item_type == "OW") {
                                    item_qty = arr.Table[j].pending_qty;
                                }
                                else {
                                    item_qty = arr.Table[j].req_qty;
                                }
                                $('#OutputItmDetailsTbl tbody').append(`<tr>
                                    <td>${j + 1}</td>
                                    <td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form" style="padding:0px;">
                                    <input id="OP_ProductName" class="form-control" autocomplete="off" type="text" name="ProductName" disabled>
                                    <input type="hidden" id="hdn_opitem" value="${arr.Table[j].item_id}" style="display: none;">
                                    <input type="hidden" id="hdn_itemType" value="${arr.Table[j].Item_type}" style="display: none;">
                                    </div><div class="col-sm-1 i_Icon">
                                    <button type="button" id="OP_ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,'${arr.Table[j].item_id}');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button></div></td>
                                    <td><input id="OP_UOM" class="form-control" autocomplete="off" value="${arr.Table[j].uom_alias}" type="text" name="UOM" disabled><input type="hidden" id="hdn_opuom" value="${arr.Table[j].uom_id}" style="display: none;"></td>
                                    <td><div class="col-sm-10 lpo_form">
                                    <input id="OP_ProducedQuantity" class="form-control num_right" maxlength="18" autocomplete="off" value="${item_qty}" onchange="OnChange_ProducedQty(event);" onkeypress = "if ( isNaN( String.fromCharCode(event.keyCode) )) return false;" type="text" name="ConsumedQuantity" placeholder="0000.00"  onblur="this.placeholder ='0000.00'">
                                    <input id="HdnOP_ProducedQuantity" hidden value="${item_qty}" type="text">
                                    <span id="producedqty_Error" class="error-message is-visible"></span>
                                    </div>
                                    <input id="sub_item" hidden value='${arr.Table[j].sub_item}' />
                                    <div class="col-sm-2 i_Icon" id="div_SubItemProdQty" style="padding:0px; ">
                                    <button type="button" id="SubItemProdQty" ${dsbl} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                    </div>
                                    </td>
                                    <td><input id="OP_lot" class="form-control" autocomplete="off" type="text" name="AvailableStockInShopfloor" placeholder="0000.00" disabled></td>
                                    <td><input id="OP_BatchNo"  maxlength="25" class="form-control" value="${arr.Table[j].batch_no}" autocomplete="off" type="text" name="BatchNo" placeholder="0000.00"  onblur="this.placeholder ='0000.00'"></td>
                                    <td><div class="lpo_form">
                                    <input id="OP_ExpiryDate" ${arr.Table[j].i_exp == "Y" ? "" : "disabled"} class="form-control" value="" onchange="onchageExpDate(event,this)" autocomplete="off" type="date" name="ExpiryDate" placeholder="">
                                    <span id="expirydate_Error" class="error-message is-visible"></span></div>
                                    <input type="hidden" id="hdn_itemex" value="${arr.Table[j].i_exp}" style="display: none;"></td>
                                    <td>
                                                                        <div class="col-sm-8 lpo_form no-padding">
                                                                            <div class="lpo_form">
                                                                                <input id="QCQuantity" class="form-control num_right" maxlength="18" value="" autocomplete="off" onkeypress="" onchange="" placeholder="0000.00" type="text" name="QCQuantity" disabled>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2 i_Icon no-padding" id="">
                                                                            <button type="button" id="" disabled="" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                                                        </div>
                                                                        <div class="col-sm-2 i_Icon no-padding">
                                                                            <button type="button" id="" class="calculator" onclick="insiteQcDetal(event)" data-toggle="modal" data-target="#PC_QC_Detail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${span_QCDetail}"> </button>
                                                                        </div>
                                                                    </td>
                                    <td>
                                    <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                    <input id="OP_QCAcceptQty" class="form-control num_right" autocomplete="off" type="text" name="QCAcceptQty" placeholder="0000.00"  onblur="this.placeholder ='0000.00'" disabled>
                                    </div>
                                    <div class=" col-sm-2 i_Icon" id="div_SubItemAccQty" >
                                         <button type="button" id="SubItemAccQty" ${dsbl} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCAccQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                     </div>
                                    </td>
                                    <td>
                                    <div class=" col-sm-10 lpo_form" style="padding:0px;">
                                    <input id="OP_QCRejectQty" class="form-control num_right" autocomplete="off" type="text" name="QCRejectQty" placeholder="0000.00"  onblur="this.placeholder ='0000.00'" disabled>
                                    </div>
                                    <div class=" col-sm-2 i_Icon" id="div_SubItemRejQty" >
                                         <button type="button" id="SubItemRejQty" ${dsbl} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRejQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                     </div>
                                    </td>
                                    <td>
                                    <div class=" col-sm-10 lpo_form no-padding" >
                                    <input id="OP_QCReworkQty" class="form-control num_right" autocomplete="off" type="text" name="QCReworkQty" placeholder="0000.00"  onblur="this.placeholder ='0000.00'" disabled>
                                    </div>
                                    <div class=" col-sm-2 i_Icon" id="div_SubItemRewQty" >
                                         <button type="button" id="SubItemRewQty" ${dsbl} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('QCRewQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_subitemdetail}"> </button>
                                     </div>
                                    </td>
                                    <td><textarea id="remarksCnfOutputitm" class="form-control remarksmessage" name="remarks" maxlength="100" autocomplete="off" placeholder="${$("#span_remarks").text()}" title="${$("#span_remarks").text()}">${arr.Table[j].remarks}</textarea></td>
                                 </tr>`);
                                $('#OutputItmDetailsTbl tbody tr #hdn_opitem[value="' + arr.Table[j].item_id + '"]').closest('tr').find("#OP_ProductName").val(arr.Table[j].item_name);
                            }
                            if (Expirable == "Y") {
                                $("#ExpRequiredStar").css("display", "");
                            } else {
                                $("#ExpRequiredStar").css("display", "none");
                            }
                        }
                        if (arr.Table2.length > 0) {
                            for (var j = 0; j < arr.Table2.length; j++) {

                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${arr.Table2[j].item_id}'></td>
                            <td><input type="text" id="subItemId" value='${arr.Table2[j].sub_item_id}'></td>
                            <td><input type="text" id="subItemQty" value=''></td>
                            <td><input type="text" id="subItemQty_forOne" value='${arr.Table2[j].foronereqqty}'></td>
                        </tr>`);

                            }
                        }
                    }
                },
            });
        } catch (err) {
            console.log("Error : " + err.message);
        }
    }
}
function insiteQcDetal(e) {
    debugger;
    var row = $(e.target).closest("tr");
    var ItemName = row.find("#OP_ProductName").val();
    var ItemId = row.find("#hdn_opitem").val();
    var UOMName = row.find("#OP_UOM").val();
    var UOMId = row.find("#hdn_opuom").val();
    var QCQuantity = row.find("#QCQuantity").val();
    var OP_ProducedQuantity = row.find("#OP_ProducedQuantity").val();
    var cnf_no = $("#Txt_ConfirmationNo").val();
    var cnf_dt = $("#Txt_ConfirmationDT").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionConfirmation/GetQcDetailItemWise",
            data: {
                cnf_no: cnf_no,
                cnf_dt: cnf_dt,
                ItemId: ItemId,
                UOMId: UOMId
            },
            success: function (data) {
                debugger;
                $("#QcDetail").html(data);
                $("#CnfItemName").val(ItemName);
                $("#CnfUOM").val(UOMName);
                $("#CnfQcQuantity").val(QCQuantity);
                $("#CnfProducedQuantity").val(OP_ProducedQuantity);
            }
        }
    )
}
function CheckValidationOnfields() {
    debugger;
    var valid = "Y";
    var Item_id = $("#ddl_ProductName").val();
    var OrderNo = $("#ddlOrderNumber").val();
    var item_name = $("#ddl_ProductName option:selected").text();
    $('#hdnproduct_name').val(item_name);
    if (Item_id == "0") {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        valid = "N";
    }
    else {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");

    }
    if (OrderNo == "0") {
        $('#vm_order_no').text($("#valueReq").text());
        $("#vm_order_no").css("display", "block");
        if (Item_id == "0") {
            $("#ddlOrderNumber").css("border-color", "red");
        }
        else {
            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "red");
        }

        $("#Txt_OrderDate").val("");
        valid = "N";
    }
    else {
        $("#vm_order_no").css("display", "none");
        if (Item_id == "0") {
            $("#ddlOrderNumber").css("border-color", "#ced4da");
        }
        else {
            $("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
        }
        //$("[aria-labelledby='select2-ddlOrderNumber-container']").css("border-color", "#ced4da");
    }
    debugger;
    var ddl_WorkstationName = $("#ddl_WorkstationName").val();/*Add by Hina on 20-09-2024 for dropdown*/
    if (ddl_WorkstationName != "0" && ddl_WorkstationName != null) {
        $("#span_vm_ddl_WorkstationName").css("display", "none");
    }
    else {
        $('#span_vm_ddl_WorkstationName').text($("#valueReq").text());
        $("#span_vm_ddl_WorkstationName").css("display", "block");
        $("#ddl_WorkstationName").css("border-color", "red");
        valid = "N";
    }
    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    if (txt_JobStartDateAndTime != "") {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = "";
        $("#txt_JobStartDateAndTime").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("span_vm_JobStartDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobStartDateAndTime").css("border-color", "red");
        valid = "N";
    }

    var txt_JobStartDateAndTime = $('#txt_JobStartDateAndTime').val().trim();
    var txt_JobEndDateAndTime = $('#txt_JobEndDateAndTime').val().trim();

    var Dates = moment(txt_JobStartDateAndTime.replace('T', ' ')).toDate();
    var Datee = moment(txt_JobEndDateAndTime.replace('T', ' ')).toDate();
    var todaydate = new Date();
    if (Datee.getTime() <= Dates.getTime() || (todaydate.getTime() < Datee.getTime() /*Added this condititon NItesh For Not Disable Save Button*/)) {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#JC_InvalidDate").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");
        $("#Txt_Hours").val("");
        valid = "N";
        return false
    }
    if (txt_JobEndDateAndTime != "") {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = "";
        $("#txt_JobEndDateAndTime").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("span_vm_JobEndDateAndTime").innerHTML = $("#valueReq").text();
        $("#txt_JobEndDateAndTime").css("border-color", "red");
        valid = "N";
    }

    if (valid === "Y") {
        return true;
    }
    else {
        return false
    }
}
function OnClickIconBtn(e, item_id) {
    debugger;
    var ItmCode = "";
    ItmCode = item_id;
    if (ItmCode != "" && ItmCode != null) {
        ItemInfoBtnClick(ItmCode);
    }
}
function ProductNameItemInfo() {
    debugger;
    var ItmCode = "";
    ItmCode = $("#OutputItemID").val();
    if (ItmCode != "" && ItmCode != null) {
        ItemInfoBtnClick(ItmCode);
    }
}
function HideAndShow_BatchSerialButton() {
    $("#ConsumptionItmDetailsTbl tbody tr").each(function () {
        debugger;
        var row = $(this);
        var b_flag = row.find("#hdi_cbatch").val();
        var s_flag = row.find("#hdi_cserial").val();
        if (b_flag === "Y") {
            row.find("#btncbatchdeatil").prop("disabled", false);
            row.find("#btncbatchdeatil").removeClass("subItmImg");
        }
        else {
            row.find("#btncbatchdeatil").prop("disabled", true);
            row.find("#btncbatchdeatil").addClass("subItmImg");
        }
        if (s_flag === "Y") {
            row.find("#btncserialdeatil").prop("disabled", false);
            row.find("#btncserialdeatil").removeClass("subItmImg");
        }
        else {
            row.find("#btncserialdeatil").prop("disabled", true);
            row.find("#btncserialdeatil").addClass("subItmImg");

        }
    });
}
function AllowFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnChangeConsumedQty(evt) {
    var currentRow = $(evt.target).closest("tr");
    //var consumedqty = currentRow.find("#CConsumedQuantity").val();
    var CnfQty = currentRow.find("#CConsumedQuantity").val();
    var AvlQty = currentRow.find("#CAvailableStockInShopfloor").val();
    var ItemType = currentRow.find("#hdnConItemType").val();
    var ErrorFlag = "N";
    debugger;

    if (CnfQty == "" || CnfQty == "0" || CnfQty == parseFloat(0)) {/*Add by Hina on 02-03-2024 to show blank instaed of 0 in inserted fields*/
        currentRow.find("#CConsumedQuantity").val("");
        currentRow.find("#consumedqty_Error").text($("#valueReq").text());
        currentRow.find("#consumedqty_Error").css("display", "block");
        currentRow.find("#CConsumedQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    //else condition removed by Suraj on 17-07-2024 to get back the functionality to Allow 0 qty for atleast one input-RM material.
    if (ItemType != "IR") {
        if (parseFloat(CnfQty) == parseFloat("0")) {
            currentRow.find("#CConsumedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
            currentRow.find("#consumedqty_Error").text($("#valueReq").text());
            currentRow.find("#consumedqty_Error").css("display", "block");
            currentRow.find("#CConsumedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            if (parseFloat(AvlQty) >= parseFloat(CnfQty)) {
                currentRow.find("#CConsumedQuantity").val(parseFloat(CnfQty).toFixed(QtyDecDigit));
                currentRow.find("#consumedqty_Error").css("display", "none");
                currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
            }
            else {
                currentRow.find("#CConsumedQuantity").val(parseFloat(CnfQty).toFixed(QtyDecDigit));
                currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
                currentRow.find("#consumedqty_Error").css("display", "block");
                currentRow.find("#CConsumedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
        }
    }
    //if (parseFloat(CnfQty) == parseFloat("0")) {
    // currentRow.find("#consumedqty_Error").text($("#valueReq").text());
    // currentRow.find("#consumedqty_Error").css("display", "block");
    // currentRow.find("#CConsumedQuantity").css("border-color", "red");
    // ErrorFlag = "Y";
    //}
    else {
        if (AvlQty == "") {
            AvlQty = 0;
        }

        else {
            if (parseFloat(AvlQty) >= parseFloat(CnfQty)) {
                currentRow.find("#CConsumedQuantity").val(parseFloat(CnfQty).toFixed(QtyDecDigit));
                currentRow.find("#consumedqty_Error").css("display", "none");
                currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
            }
            else {
                currentRow.find("#CConsumedQuantity").val(parseFloat(CnfQty).toFixed(QtyDecDigit));
                currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
                currentRow.find("#consumedqty_Error").css("display", "block");
                currentRow.find("#CConsumedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            if (ErrorFlag != "Y") {
                if (parseFloat(AvlQty) == parseFloat("0")) {
                    currentRow.find("#CConsumedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
                    //currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
                    //currentRow.find("#consumedqty_Error").css("display", "block");
                    //currentRow.find("#CConsumedQuantity").css("border-color", "red");
                    //ErrorFlag = "Y";
                }
            }
            if (parseFloat(CnfQty) == parseFloat("0")) {
                currentRow.find("#btncbatchdeatil").css("border", "#007bff");
            }
        }
    }
}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();

        var ItemName = clickedrow.find("#CItemName").val();
        var UOMName = clickedrow.find("#CUOM").val();

        var ItemId = clickedrow.find("#hdn_citem").val();
        var UOMId = clickedrow.find("#hdn_cuom").val();
        if (AvoidDot(ConsumedQuantity) == false) {
            ConsumedQuantity = "";
        }
        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#consumedqty_Error").text($("#FillQuantity").text());
            clickedrow.find("#consumedqty_Error").css("display", "block");
            clickedrow.find("#CConsumedQuantity").css("border-color", "red");
        }
        else {
            var Status = $("#hdn_Status").val();
            var Transtype = $("#hdn_TransType").val();
            var cmd = $("#batch_Command").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemBatchDetail();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                var Shfl_Id = $("#hdn_shopfloor_id").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ProductionConfirmation/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            ShflId: Shfl_Id,
                            DocStatus: Status,
                            SelectedItemdetail: SelectedItemdetail,
                            Transtype: Transtype,
                            cmd: cmd,
                            uom_id: UOMId
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(ConsumedQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");

                            //Added by Suraj on 20-02-2024
                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", ConsumedQuantity, "AvailableQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }
            else {
                var PC_No = $("#Txt_ConfirmationNo").val();
                var PC_Date = $("#Txt_ConfirmationDT").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ProductionConfirmation/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            PC_No: PC_No,
                            PC_Date: PC_Date,
                            DocStatus: Status,
                            ItemID: ItemId,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(ConsumedQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");

                        },
                    });
            }
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var ItemName = clickedrow.find("#CItemName").val();
        var UOMName = clickedrow.find("#CUOM").val();
        var ItemId = clickedrow.find("#hdn_citem").val();
        var UOMID = clickedrow.find("#hdn_cuom").val();

        if (parseFloat(ConsumedQuantity) == "0" || parseFloat(ConsumedQuantity) == "") {
            clickedrow.find("#consumedqty_Error").text($("#FillQuantity").text());
            clickedrow.find("#consumedqty_Error").css("display", "block");
            clickedrow.find("#CConsumedQuantity").css("border-color", "red");
        }
        else {

            var Status = $("#hdn_Status").val();
            if (Status == "" || Status == null || Status == 'D') {
                Comn_BindItemSerialDetail();

                var SelectedItemSerial = $("#HDSelectedSerialwise").val();
                var Shfl_Id = $("#hdn_shopfloor_id").val();
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ProductionConfirmation/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            ShflId: Shfl_Id,
                            DocStatus: Status,
                            SelectedItemSerial: SelectedItemSerial,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);

                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(ConsumedQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                        },
                    });
            }
            else {
                var PC_No = $("#Txt_ConfirmationNo").val();
                var PC_Date = $("#Txt_ConfirmationDT").val();
                var Transtype = $("#hdn_TransType").val();
                var cmd = $("#batch_Command").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ProductionConfirmation/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            PCNo: PC_No,
                            PCDate: PC_Date,
                            DocStatus: Status,
                            ItemID: ItemId,
                            Transtype: Transtype,
                            cmd: cmd
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(ConsumedQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#Serial_ItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#Serial_Qty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }
        }
    } catch (err) {
        console.log("Production Confirmation Error : " + err.message);
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    var QtyDigit = $("#QtyDigit").text();
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDigit));
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "0";
        }
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }

        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });

        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    if (ItemMI_Qty == "") {
        ItemMI_Qty = "0";
    }
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "btncbatchdeatil", "Y");
            IsuueFlag = false;
        }
    });

    if (IsuueFlag) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        }
        else {
            debugger;
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#Batch_ItemId").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });

            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="Batch_LotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="Batch_ItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="Batch_UOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="Batch_BatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="Batch_Qty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="Batch_ExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="Batch_AvlBatchQty" value="${AvailableQty}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdn_citem").val();
            if (ItemId == SelectedItem) {
                //clickedrow.find("#btncbatchdeatil").css("border-color", "#007bff");
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btncbatchdeatil").css("border", "#007bff");
                ValidateEyeColor(clickedrow, "btncbatchdeatil", "N");
            }
        });
    }
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#Serial_ItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="Serial_ItemId" value="${ItemId}" /></td>
            <td><input type="text" id="Serial_UOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="Serial_LOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="Serial_Qty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="Serial_SerialNo" value="${ItemSerialNO}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdn_citem").val();
            if (ItemId == SelectedItem) {
                //clickedrow.find("#btncserialdeatil").css("border-color", "#007bff");
                ValidateEyeColor(clickedrow, "btncserialdeatil", "N");
            }
        });
    }
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 11-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var inputrowcount = $('#ConsumptionItmDetailsTbl tbody tr').length;
    var outputrowcount = $('#OutputItmDetailsTbl tbody tr').length;

    if (CheckValidationOnfields() == true) {
        if (inputrowcount > 0 && outputrowcount > 0) {
            var flagConsumedQty = CheckItemValidations_ConsumedQty();
            if (flagConsumedQty == false) {
                return false;
            }
            var flagProducedQty = CheckItemValidations_ProducedQty();
            if (flagProducedQty == false) {
                return false;
            }
            var flagExDate = CheckItemValidations_ExpireDate();
            if (flagExDate == false) {
                return false;
            }
            var flagsubItemVald = CheckValidations_forSubItems();
            if (flagsubItemVald == false) {
                return false;
            }
            var Batchflag = CheckItemBatch_Validation();
            if (Batchflag == false) {
                return false;
            }
            var SerialFlag = CheckItemSerial_Validation();
            if (SerialFlag == false) {
                return false;
            }



            if (flagConsumedQty == true && flagProducedQty == true && flagExDate == true && Batchflag == true && SerialFlag == true) {
                var PCInputItemDetailList = new Array();
                var PCOutputItemDetailList = new Array();

                $("#ConsumptionItmDetailsTbl TBODY TR").each(function () {
                    var row = $(this);
                    var InputItemList = {};
                    InputItemList.ItemId = row.find("#hdn_citem").val();
                    InputItemList.UOMId = row.find('#hdn_cuom').val();
                    InputItemList.ConsumedQuantity = row.find('#CConsumedQuantity').val();
                    InputItemList.remarks = row.find('#Cremarks').val();
                    InputItemList.item_name = row.find('#CItemName').val();
                    InputItemList.uom_alias = row.find('#CUOM').val();
                    InputItemList.avl_stock_shfl = row.find('#CAvailableStockInShopfloor').val();
                    InputItemList.foronereqqty = row.find('#hdnitem_reqqty').val();
                    InputItemList.hdi_cbatch = row.find('#hdi_cbatch').val();
                    InputItemList.hdi_cserial = row.find('#hdi_cserial').val();
                    PCInputItemDetailList.push(InputItemList);
                    debugger;
                });

                $("#OutputItmDetailsTbl TBODY TR").each(function () {
                    var row = $(this);
                    var OutputItemList = {};


                    OutputItemList.ProductId = row.find("#hdn_opitem").val();
                    OutputItemList.item_name = row.find("#OP_ProductName").val();
                    OutputItemList.UOMId = row.find('#hdn_opuom').val();
                    OutputItemList.ProducedQuantity = row.find('#OP_ProducedQuantity').val();
                    OutputItemList.lot = row.find('#OP_lot').val();
                    OutputItemList.BatchNo = row.find('#OP_BatchNo').val();
                    OutputItemList.ExDate = row.find('#OP_ExpiryDate').val();
                    OutputItemList.Item_type = row.find('#hdn_itemType').val();
                    OutputItemList.ItemInfoBtnClick = row.find('#OP_ItmInfoBtnIcon').val();
                    OutputItemList.sub_item = row.find('#sub_item').val();
                    OutputItemList.i_exp = row.find('#hdn_itemex').val();
                    OutputItemList.OP_QCAcceptQty = row.find('#OP_QCAcceptQty').val();
                    OutputItemList.OP_QCRejectQty = row.find('#OP_QCRejectQty').val();
                    OutputItemList.OP_QCReworkQty = row.find('#OP_QCReworkQty').val();
                    OutputItemList.uom_alias = row.find('#OP_UOM').val();
                    OutputItemList.remarks = row.find('#remarksCnfOutputitm').val();
                    PCOutputItemDetailList.push(OutputItemList);
                    debugger;
                });

                var InputItmStr = JSON.stringify(PCInputItemDetailList);
                var OutputItmStr = JSON.stringify(PCOutputItemDetailList);
                $('#hd_ConsumptionItemDetail').val(InputItmStr);
                $('#hd_OutputItemDetail').val(OutputItmStr);

                Comn_BindItemBatchDetail();
                Comn_BindItemSerialDetail();

                /*-----------Sub-item-------------*/
                debugger;
                var SubItemsListArr = Cmn_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/

                /*----- Attatchment start--------*/
                $(".fileinput-upload").click();/*To Upload Img in folder*/
                FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
                /*----- Attatchment End--------*/

                /*----- Attatchment start--------*/
                var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
                $('#hdn_Attatchment_details').val(ItemAttchmentDt);
                /*----- Attatchment End--------*/
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                return true;
            }
            else {
                return false;
            }
        }
        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function CheckItemValidations_ConsumedQty() {
    debugger;
    var ErrorFlag = "N";
    var qty = "N";
    var qty1 = "N";
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var CnfQty = currentRow.find("#CConsumedQuantity").val();
        var AvlQty = currentRow.find("#CAvailableStockInShopfloor").val();
        var ItemType = currentRow.find("#hdnConItemType").val();
        if (CnfQty == "") {
            currentRow.find("#consumedqty_Error").text($("#valueReq").text());
            currentRow.find("#consumedqty_Error").css("display", "block");
            currentRow.find("#CConsumedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        if (ItemType != "IR") {
            if (parseFloat(CnfQty) == parseFloat("0")) {
                currentRow.find("#consumedqty_Error").text($("#valueReq").text());
                currentRow.find("#consumedqty_Error").css("display", "block");
                currentRow.find("#CConsumedQuantity").css("border-color", "red");
                //if (qty == "N") {
                ErrorFlag = "Y";
                //}
            }
            else {
                if (parseFloat(AvlQty) >= parseFloat(CnfQty)) {
                    currentRow.find("#consumedqty_Error").css("display", "none");
                    currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
                    //ErrorFlag = "N";
                    qty = "Y";
                }
                else {
                    currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
                    currentRow.find("#consumedqty_Error").css("display", "block");
                    currentRow.find("#CConsumedQuantity").css("border-color", "red");
                    ErrorFlag = "Y";
                }
            }
        }
        else {
            if (AvlQty == "") {
                AvlQty = 0;
            }
            //if (parseFloat(AvlQty) == parseFloat("0")) {
            //    currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
            //    currentRow.find("#consumedqty_Error").css("display", "block");
            //    currentRow.find("#CConsumedQuantity").css("border-color", "red");
            //    ErrorFlag = "Y";
            //}
            else {
                if (parseFloat(AvlQty) >= parseFloat(CnfQty)) {
                    currentRow.find("#consumedqty_Error").css("display", "none");
                    currentRow.find("#CConsumedQuantity").css("border-color", "#ced4da");
                    //ErrorFlag = "N";
                    qty = "Y";
                }
                else {
                    currentRow.find("#consumedqty_Error").text($("#Stocknotavailable").text());
                    currentRow.find("#consumedqty_Error").css("display", "block");
                    currentRow.find("#CConsumedQuantity").css("border-color", "red");
                    ErrorFlag = "Y";
                }
            }
        }
        if (parseFloat(CnfQty) == parseFloat("0")) {
            //ErrorFlag = "Y"
            if (qty1 == "N") {
                qty = "N";

            }
        }
        else {
            qty1 = "Y";
            qty = "Y";
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else if (qty == "N") {
        swal("", $("#ConsumptionQuantityOfAtLeastOneMaterialIsRequiredDocumentCannotBeSaved").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidations_ProducedQty() {
    debugger;
    var ErrorFlag = "N";
    $("#OutputItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#hdn_itemType").val() != "OS") {//Added by Suraj on 17-07-2024 to Allow Output scrap with 0 qty.
            if (currentRow.find("#OP_ProducedQuantity").val() == "" || parseFloat(currentRow.find("#OP_ProducedQuantity").val()) == "0") {
                currentRow.find("#producedqty_Error").text($("#valueReq").text());
                currentRow.find("#producedqty_Error").css("display", "block");
                currentRow.find("#OP_ProducedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#producedqty_Error").css("display", "none");
                currentRow.find("#OP_ProducedQuantity").css("border-color", "#ced4da");
            }
        }
        
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemValidations_ExpireDate() {
    debugger;
    var ErrorFlag = "N";
    $("#OutputItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var flag = currentRow.find("#hdn_itemex").val();
        if (flag === "Y") {
            if (currentRow.find("#OP_ExpiryDate").val() == "" || parseFloat(currentRow.find("#OP_ExpiryDate").val()) == null) {
                currentRow.find("#expirydate_Error").text($("#valueReq").text());
                currentRow.find("#expirydate_Error").css("display", "block");
                currentRow.find("#OP_ExpiryDate").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#expirydate_Error").css("display", "none");
                currentRow.find("#OP_ExpiryDate").css("border-color", "#ced4da");
            }
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatch_Validation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var ItemId = clickedrow.find("#hdn_citem").val();
        var UOMId = clickedrow.find("#hdn_cuom").val();
        var Batchable = clickedrow.find("#hdi_cbatch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                if (parseFloat(ConsumedQuantity) == parseFloat("0")) {

                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    /*clickedrow.find("#btncbatchdeatil").css("border", "1px solid #dc3545");*/
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "Y");
                }
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#Batch_Qty').val();
                    var bchitemid = currentRow.find('#Batch_ItemId').val();
                    var bchuomid = currentRow.find('#Batch_UOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btncbatchdeatil").css("border", "#007bff");
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "N");
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btncbatchdeatil").css("border", "1px solid #dc3545");
                    ValidateEyeColor(clickedrow, "btncbatchdeatil", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithconsumedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemSerial_Validation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    $("#ConsumptionItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#CConsumedQuantity").val();
        var ItemId = clickedrow.find("#hdn_citem").val();
        var UOMId = clickedrow.find("#hdn_cuom").val();
        var Serialable = clickedrow.find("#hdi_cserial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btncserialdeatil").css("border-color", "red");
                ValidateEyeColor(clickedrow, "btncserialdeatil", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#Serial_Qty').val();
                    var srialitemid = currentRow.find('#Serial_ItemId').val();
                    var srialuomid = currentRow.find('#Serial_UOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btncserialdeatil").css("border-color", "#007bff");
                    ValidateEyeColor(clickedrow, "btncserialdeatil", "N");
                }
                else {
                    /*commented byHina on 05-02-2024 to validate by Eye color*/
                    //clickedrow.find("#btncserialdeatil").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "btncserialdeatil", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serializedqtydoesnotmatchwithconsumedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChange_ProducedQty(evt) {
    var currentRow = $(evt.target).closest("tr");
    var prodqty = currentRow.find("#OP_ProducedQuantity").val();
    var itemType = currentRow.find("#hdn_itemType").val();
    /*Code add by Hina on 02-03-2024 to show balnk instead of 0 in insert fields*/

    // ' && itemType != "OS" ' added by Suraj on 17-07-2024 to allow '0' for output scrap items
    if ((prodqty == "0" || prodqty == parseFloat(0)) && itemType != "OS") {
        currentRow.find("#OP_ProducedQuantity").val("");
    }
    /*Code End*/
    if ((prodqty == "" || parseFloat(prodqty) == "0") && itemType != "OS") {
        currentRow.find("#producedqty_Error").text($("#valueReq").text());
        currentRow.find("#producedqty_Error").css("display", "block");
        currentRow.find("#OP_ProducedQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#producedqty_Error").css("display", "none");
        currentRow.find("#OP_ProducedQuantity").css("border-color", "#ced4da");
        OnChangeProdQty(evt);
    }
}
function functionConfirm(event) {
    debugger;
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
}
function FilterProductionCnfListData() {
    debugger;
    try {
        var ProductID = $("#ProductName option:selected").val();
        var OPID = $("#ddl_OperationName option:selected").val();
        var ShflID = $("#ddl_ShopfloorName option:selected").val();
        var WSID = $("#ddl_WorkstationName option:selected").val();
        var ShftID = $("#ddlshiftID option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductionConfirmation/ProductionConfirmation_Search",
            data: {
                ProductID: ProductID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                OPID: OPID,
                ShflID: ShflID,
                WSID: WSID,
                ShftID: ShftID
            },
            success: function (data) {
                debugger;
                $('#tbodyproductioncnfList').html(data);
                $('#ListFilterData').val(ProductID + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + OPID + ',' + ShflID + ',' + WSID + ',' + ShftID);
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

function OnChangeProdQty(evt) {
    debugger;
    var currentRow = $(evt.target).closest("tr");
    var prodqty = currentRow.find("#OP_ProducedQuantity").val();
    var Hdnprodqty = currentRow.find("#HdnOP_ProducedQuantity").val();
    var opitem = currentRow.find("#hdn_opitem").val();
    var ItemType = currentRow.find("#hdn_itemType").val();


    if (ItemType == 'OW' || ItemType == 'OF') {
        if ((prodqty == "") || (prodqty == null)) {
            prodqty = "0";
        }
        var ChangePerc = ((parseFloat(prodqty) - parseFloat(Hdnprodqty)) / parseFloat(Hdnprodqty)) * 100;
        $("#OutputItmDetailsTbl >tbody > tr").each(function () {
            var Crow = $(this);
            var prodqty = Crow.find("#HdnOP_ProducedQuantity").val();
            var opitem = Crow.find("#hdn_opitem").val();
            var ItemType = Crow.find("#hdn_itemType").val();
            if (ItemType != 'OW' && ItemType != 'OF') {
                var NewProdScrapQty = parseFloat(prodqty) + parseFloat(prodqty) * parseFloat(ChangePerc) / 100;
                Crow.find("#HdnOP_ProducedQuantity").val(NewProdScrapQty);
                NewProdScrapQty = parseFloat(NewProdScrapQty).toFixed(0) <= 0 ? 1 : parseFloat(NewProdScrapQty).toFixed(0);
                Crow.find("#OP_ProducedQuantity").val(parseFloat(NewProdScrapQty).toFixed(0));
            } else {
                var ForHdnprodqty = Crow.find("#OP_ProducedQuantity").val();
                Crow.find("#HdnOP_ProducedQuantity").val(parseFloat(ForHdnprodqty).toFixed(0));
            }

        });
        currentRow.find("#OP_ProducedQuantity").val(prodqty);
        if ((prodqty != "") || (prodqty != null) || (parseFloat(prodqty) != "0")) {
            $("#ConsumptionItmDetailsTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this);
                var imd_materialid = currentRow.find("#hdn_citem").val();
                var Req_qty = currentRow.find("#hdnitem_reqqty").val();
                totalReqqty = parseFloat(prodqty) * parseFloat(Req_qty)
                currentRow.find("#CConsumedQuantity").val(parseFloat(totalReqqty).toFixed(QtyDecDigit));
                //Added by Suraj Maurya on 24-12-2024 for change sub-item consume Qty according on change of output item qty
                $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + imd_materialid + "]").closest('tr').each(function (i, row) {
                    debugger;
                    var currentRow = $(this);
                    var item_id = currentRow.find("#ItemId").val();
                    if (item_id == imd_materialid) {
                        var Req_qty = currentRow.find("#subItemQty_forOne").val();
                        var totalReqqty = parseFloat(prodqty) * parseFloat(Req_qty)
                        currentRow.find("#subItemQty").val(parseFloat(totalReqqty).toFixed(QtyDecDigit));
                    }
                });
            });
           
        }
    }
    //else {
    //    currentRow.find("#OP_ProducedQuantity").val(parseFloat(prodqty).toFixed(QtyDecDigit));
    //}

}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemProdQty",);
    //Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPlanQty",);
}
function HideShowPageWise1(sub_item, clickedrow) {
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrderQty",);
    //Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPlanQty",);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfsno").val();
    var hdnshopfloorid = $("#hdn_shopfloor_id").val();
    var Doc_no = $("#Txt_ConfirmationNo").val();
    var Doc_dt = $("#Txt_ConfirmationDT").val();
    var Prod_no = "";
    var Prod_Dt = "";
    if (flag == "PC_ConsumeQuantity") {
        subflag = "PC_ConsumeQuantity";
        var hdnSubItmFlg = $("#hdn_Subitmflag").val(subflag);
        var ProductNm = clickdRow.find("#CItemName").val();
        var ProductId = clickdRow.find("#hdn_citem").val();
        var UOM = clickdRow.find("#CUOM").val();
        var UOMId = clickdRow.find("#hdn_cuom").val();
        Prod_no = $("#ddlOrderNumber option:selected").text();
        var DocDt = $("#ddlOrderNumber").val();
        var Doc_dt1 = DocDt.split("-");
        Prod_Dt = Doc_dt1[2] + '-' + Doc_dt1[1] + '-' + Doc_dt1[0]
    }
    else if (flag == "OrderedQty_Header") {

        var ProductNm = $("#OutputItemName").val();
        var ProductId = $("#OutputItemID").val();
        var UOM = $("#OutputUOMName").val();
        var UOMId = $("#OutputUOMID").val();
       
    }  
    else {
        var ProductNm = clickdRow.find("#OP_ProductName").val();
        var ProductId = clickdRow.find("#hdn_opitem").val();
        var UOM = clickdRow.find("#OP_UOM").val();
        var UOMId = clickdRow.find("#hdn_opuom").val();
        if (flag == "Quantity") {
            Prod_no = $("#ddlOrderNumber option:selected").text();
            var DocDt = $("#ddlOrderNumber").val();
            var Doc_dt1 = DocDt.split("-");
            Prod_Dt = Doc_dt1[2] + '-' + Doc_dt1[1] + '-' + Doc_dt1[0]
        }
    }
    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity" || flag == "PC_ConsumeQuantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            if (flag == "PC_ConsumeQuantity") {
                List.avl_stock_shfl = row.find('#subItemAvlQty').val();
                let SubItmQty = row.find('#subItemQty').val();
                if (SubItmQty == "") {//Added by Suraj Maurya on 23-12-2024 for getting auto suggest pending sub item Qty for consumption 
                    let prodqty = 0//$('#OutputItmDetailsTbl > tbody > tr #hdn_itemType[value="OF"||value="OW"]').closest('tr').find("#OP_ProducedQuantity").val();
                    $('#OutputItmDetailsTbl > tbody > tr').each(function () {
                        var rowQty = $(this);
                        var hdn_itemType = rowQty.find("#hdn_itemType").val();
                        if (hdn_itemType == "OF" || hdn_itemType == "OW") {
                            prodqty = CheckNullNumber(rowQty.find("#OP_ProducedQuantity").val());
                        }
                    })
                    let Req_qty = row.find("#subItemQty_forOne").val();
                    let totalReqqty = parseFloat(prodqty) * parseFloat(Req_qty)
                    row.find("#subItemQty").val(parseFloat(totalReqqty).toFixed(QtyDecDigit));
                }
                List.qty = row.find('#subItemQty').val();
            } else {
                List.qty = row.find('#subItemQty').val();
            }
            
            NewArr.push(List);
        });
        if (flag == "PC_ConsumeQuantity") {
            Sub_Quantity = clickdRow.find("#CConsumedQuantity").val();
        }
        else {
            Sub_Quantity = clickdRow.find("#OP_ProducedQuantity").val();
        }

    }
    else if (flag == "OrderedQty_Header")
    {
        ProductNm = $("#OutputItemName").val();
        ProductId = $("#OutputItemID").val();
        UOM = $("#OutputUOMName").val();
        UOMId = $("#OutputUOMID").val();
        Sub_Quantity = $("#Txt_OrderQty").val();
        Doc_no = $("#ddlOrderNumber option:selected").text();
        Doc_dt = $("#Txt_OrderDate").val();
        flag = "OrderedQty";
    }
    else if (flag == "OrderedQty") {

        ProductNm = $("#ddl_ProductName option:selected").text();
        ProductId = $("#ddl_ProductName option:selected").val();
        UOM = $("#Txt_UOM").val();
        UOMId = $("#hdn_uom_id").val();
        Sub_Quantity = $("#Txt_OrderQty").val(); ddlOrderNumber
        Doc_no = $("#ddlOrderNumber option:selected").text();
        Doc_dt = $("#Txt_OrderDate").val();
    }
    else if (flag == "QCQty") {
        QCQuantity = clickdRow.find("#QCQuantity").val();
        Sub_Quantity = clickdRow.find("#OP_ProducedQuantity").val();
    }
    else {
        Sub_Quantity = clickdRow.find("#OP_ProducedQuantity").val();

    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdn_Status").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ProductionConfirmation/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            Uom_id: UOMId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Shfl_id: hdnshopfloorid,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            Prod_no: Prod_no,
            Prod_Dt: Prod_Dt
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            if (flag == "QCQty") {
                $("#CnfSubProd_Quantity").val(Sub_Quantity);
                $("#Sub_Quantity").val(QCQuantity);
            }
            else {
                $("#Sub_Quantity").val(Sub_Quantity);
            }
            $("#Sub_ItemDetailTbl >tbody >tr").each(function () {
                var currantR = $(this);
                var qty = currantR.find("#subItemQty").val();
                if (flag == "PC_ConsumeQuantity") {
                    if (qty == null || qty == "" || parseFloat(qty) == 0) {
                        currantR.find("#subItemQty").attr("disabled", true);
                    }
                }             
            });
        }
    })
    if (flag == 'enable') {

    }
    else if (flag == 'readonly') {

    }
}
function CheckValidations_forSubItems() {
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    //return Cmn_CheckValidations_forSubItems("OutputItmDetailsTbl", "", "hdn_opitem", "OP_ProducedQuantity", "SubItemProdQty", "Y");

    debugger;
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    var ErrFlg = "";
    if (Cmn_CheckValidations_forSubItems("OutputItmDetailsTbl", "", "hdn_opitem", "OP_ProducedQuantity", "SubItemProdQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (Cmn_CheckValidations_forSubItems("ConsumptionItmDetailsTbl", "", "hdn_citem", "CConsumedQuantity", "SubItemConsumeQuantity", "Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }


}
function ResetWorningBorderColor() {
    var Tbllen = $("#ConsumptionItmDetailsTbl >tbody>tr").length;

    if (Tbllen > 0) {
        return Cmn_CheckValidations_forSubItems("ConsumptionItmDetailsTbl", "", "hdn_citem", "CConsumedQuantity", "SubItemConsumeQuantity", "N");

    }
    return Cmn_CheckValidations_forSubItems("ProductItemDetailsTbl", "hfsno", "ProductName", "PlannedQuantity", "SubItemPlanQty", "N");


}
/***--------------------------------Sub Item Section End-----------------------------------------***/
function onchageExpDate(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    currentRow.find("#expirydate_Error").css("display", "none");
    currentRow.find("#OP_ExpiryDate").css("border-color", "#ced4da");
}

function approveonclick() { /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "OutputItmDetailsTbl", [{ "FieldId": "OP_ProductName", "FieldType": "input" }]);
}

function FilterItemDetail1(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ConsumptionItmDetailsTbl", [{ "FieldId": "CItemName", "FieldType": "input" }]);
}


