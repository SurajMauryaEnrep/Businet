/************************************************
Javascript Name: Inter Branch Purchase
Created By: Surbhi
Created Date: 11-06-2025
Description: This Javascript use for the Inter Branch Purchase Detail & List many function
Modified By:
Modified Date:
Description:
*************************************************/
var LddCostDecDigit = 5;///For Landed Cost 
const ValDecDigit = $("#ValDigit").text();
const ExchDecDigit = $("#ExchDigit").text();///Amount
$(document).ready(function () {
    $("#ddlIBP_BranchName").select2();
    $("#ddlSupplierName").select2();
    $("#SupplierName").select2();
    $("#ddlIBP_BillNo").select2();
    //$("#DocumentMenuId").val("105101153");

    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        var ListFilterData = $('#ListFilterData').val();
        var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        if (SSINo != "" && SSINo != null) {
            location.href = "/ApplicationLayer/InterBranchPurchase/AddInterBranchPurchaseDetail/?DocNo=" + SSINo + "&DocDate=" + SSIDt + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        var Doc_id = "105101153";// $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SSINo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        debugger;
        GetWorkFlowDetails(SSINo, SSIDt, Doc_id, Doc_Status);
    });

    $('#POInvItmDetailsTbl tbody').on('click', '.deleteIcon', async function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();

        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";

        ItemCode = $(this).closest('tr').find("#ItemName" + SNo).val();
        CalculateAmount();
        // var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
        var TOCAmount = parseFloat($("#TxtOtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        DeleteItemBatchOrderQtyDetails(ItemCode); /**Added BY Nitesh 05-02-2023**/
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetSI_ItemTaxDetail();
        BindOtherChargeDeatils();
        GetAllGLID();
    });
    if ($("#nontaxable").is(":checked")) {
        $("#POInvItmDetailsTbl > tbody > tr ").each(function () {
            var currentRow = $(this);
            currentRow.find("#TaxExempted").attr("disabled", true);
            currentRow.find("#ManualGST").attr("disabled", true);
            currentRow.find("#BtnTxtCalculation").prop("disabled", true);
            currentRow.find('#ManualGST').prop("checked", false);
        });
    }
    var InvoiceNumber = $("#InvoiceNumber").val();
    var InvoiceDate = $("#Sinv_dt").val();
    var hdStatus = $('#hfStatus').val();
    GetWorkFlowDetails(InvoiceNumber, InvoiceDate, "105101153", hdStatus);
    $("#hdDoc_No").val(InvoiceNumber);
    BindDDLAccountList();
});

function BtnSearch() {
    FilterSuppPI_List();
    ResetWF_Level();
}

function FilterSuppPI_List() {
    try {
        var SuppId = $("#SupplierName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var WFStatus = "";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/InterBranchPurchase/IBPListSearch",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                $('#SuppPIList').html(data);
                $("#ListFilterData").val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + WFStatus)
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        console.log("SuppPI Error : " + err.message);
    }
}

function OnIBPChangeBranch(BranchID) {
    var BranchID = BranchID.value;
    if (BranchID == "" || BranchID == null || BranchID == undefined) {
        BranchID = BranchID;
    }

    if (BranchID == "") {
        $('#SpanddlIBP_BranchNameErrorMsg').text($("#valueReq").text());
        $("#SpanddlIBP_BranchNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlBranchName-container']").css("border-color", "red");
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        var SuppId = $('#SupplierName option:selected').val();
        $("#Hdn_PInvSuppName").val(Suppname);
        $("#Hdn_interbrnchid").val(BranchID);
        $("#Hdn_PInvSuppId").val(SuppId);
        $("#SpanddlIBP_BranchNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlIBP_BranchName-container']").css("border-color", "#ced4da");
        $("#AddItemsInDPI").css("display", "")
    }

}

function OnChangeSupplier(SuppID) {
    var Supp_id = SuppID.value;
    debugger
    if (Supp_id == "0") {
        $('#SpanSupplierNameErrorMsg').text($("#valueReq").text());
        $("#SpanSupplierNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlSupplierName-container']").css("border-color", "red");
    }
    else {
        var SuppName = $("#ddlSupplierName option:selected").text();
        $("#Hdn_SuppName").val(SuppName)
        var SuppId = $("#Hdn_SuppName").val();
        $("#Hdn_SuppId").val(SuppId);
        $("#SpanSupplierNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlSupplierName-container']").css("border-color", "#ced4da");
    }
    // var DocumentMenuId = $("#DocumentMenuId").val();
    try {

        GetSuppAddress(Supp_id);
        GetInterBranchSaleInvoices();
        //try {
        //    $.ajax({
        //        type: "POST",
        //        url: "/ApplicationLayer/InterBranchSale/GetAutoCompleteSearchSuppList",
        //        success: function (data) {
        //            if (data === 'ErrorPage') {
        //                ErrorPage();
        //                return false;
        //            }
        //            try {
        //                GetInterBranchSaleInvoices();
        //            } catch (e) {
        //                console.error("Error parsing data:", e);
        //            }
        //        },
        //        error: function (xhr, status, error) {
        //            console.error("AJAX Error: ", error); // Log AJAX errors
        //        }
        //    });
        //} catch (err) {
        //    console.log("GetInter Branch Sales Error: " + err.message); // Handle any JavaScript errors
        //}
    } catch (err) {
        console.log("SupplierChange Error : " + err.message);
    }
}

function GetSuppAddress(Supp_id) {
    let ExchDecDigit = $("#ExchDigit").text();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DPO/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Rqrdaddr").css("display", "");
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#supp_acc_id").val(arr.Table[0].supp_acc_id);
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            $("#curr_id").val(arr.Table[0].curr_id);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            //$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                            $("#conv_rate_tb").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                            if (arr.Table[0].gst_cat == "UR") {
                                $('#RCMApplicable').prop("checked", true);
                                $("#RCMApplicable").prop("disabled", true);
                            }
                            else {
                                $('#RCMApplicable').prop("checked", false);
                                $("#RCMApplicable").prop("disabled", false);
                            }
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            if ($("#Address").val() === "") {
                                $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
                                $("#Address").css("border-color", "Red");
                                $("#SpanSuppAddrErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppAddrErrorMsg").css("display", "none");
                                $("#Address").css("border-color", "#ced4da");
                            }
                        }
                        else {
                            $("#Address").val(null);
                            $("#bill_add_id").val(null);
                            $("#ship_add_gstNo").val(null);
                            $("#Ship_StateCode").val(null);
                            var s = '<option value="' + "0" + '">' + "---Select---" + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
                            $("#conv_rate").prop("readonly", false);
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetSupplierData Error : " + err.message);
    }
}

function OnClickbillingAddressIconBtn(e) {
    //debugger
    var Supp_id = $('#ddlSupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var CustPros_type = "C";
    var status = "A";//$("#hfStatus").val().trim();
    var SSIDTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SSIDTransType = "Update";
    }
    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, SSIDTransType);
}

function GetInterBranchSaleInvoices() {
    try {
        var brid = $("#ddlIBP_BranchName").val();
        if (brid == "0") {
            $('#SpanddlIBP_BillNoErrorMsg').text($("#valueReq").text());
            $("#SpanddlIBP_BillNoErrorMsg").css("display", "block");
            $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "red");
        }
        else {
            $("#SpanddlIBP_BillNoErrorMsg").css("display", "none");
            $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "#ced4da");
        }

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/InterBranchPurchase/GetBillList",
            data: {
                BranchId: brid,
            },
            success: function (data) {
                if (data === 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                if (data && data !== "{}") {
                    if (typeof data === 'string') {
                        try {
                            data = JSON.parse(data);
                        } catch (e) {
                            console.error("Error parsing data:", e);
                        }
                    }

                    if (data.length > 0) {

                        $("#ddlIBP_BillNo").empty();
                        var uniqueOptGroupId = "Textddl_" + new Date().getTime();

                        $('#ddlIBP_BillNo').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#span_InvoiceNo").text()}' data-invdate='${$("#span_InvoiceDate").text()}' ></optgroup>`);
                        var dataArray = data; for (var i = 0; i < dataArray.length; i++) {
                            var item = dataArray[i];
                            var option = `<option data-invdate="${item.ID}" value="${item.Name}">${item.Name}</option>`;
                            $('#' + uniqueOptGroupId).append(option);
                        }
                        $('#ddlIBP_BillNo').select2({
                            templateResult: function (data) {
                                var PInvDate = $(data.element).data('invdate');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                            }
                        });
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error);
            }
        });
    } catch (err) {
        console.log("GetInterBranchSaleInvoices Error: " + err.message);
    }
}

function OnIBPChangeBill(BillNo) {
    //var Bill_No = BillNo.value;
    //if (Bill_No == "0") {
    //    $('#SpanBillDateErrorMsg').text($("#valueReq").text());
    //    $("#SpanBillDateErrorMsg").css("display", "block");
    //    $("#BillDate").css("border-color", "red");
    //}
    //else {
    //    $("#SpanBillDateErrorMsg").css("display", "none");
    //    $("#BillDate").css("border-color", "#ced4da");
    //}
    //// var DocumentMenuId = $("#DocumentMenuId").val();
    //try {
    //    var selectedDate = $("#ddlIBP_BillNo").find(':selected').data('invdate');//$(this).find(':selected').data('invdate');
    //    $('#BillDate').val(selectedDate || '');
    //    OnClickAddButton();

    //} catch (err) {
    //    console.log("SupplierChange Error : " + err.message);
    //}

    var Bill_No = BillNo.value;
    var SPODate = $('#ddlIBP_BillNo').select2("data")[0].element.attributes[0].value;
    var SPODP = SPODate.split("-");
    $("#HDddlBillNo").val(Bill_No);
    var FSPODate = (SPODP[2] + "-" + SPODP[1] + "-" + SPODP[0]);
    if (Bill_No == "---Select---") {
        $("#BillDate").val("");
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "red");
    }
    else {
        $("#BillDate").val(FSPODate);
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "#ced4da");
        OnClickAddButton();
    }
}

function OnClickAddButton() {
    debugger;
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    let ExchDecDigit = $("#ExchDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var RowNo = 0;
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }

    //$("#conv_rate").prop("readonly", true);
    var DocMenuId = $("#DocumentMenuId").val();
    var InvNo = $('#ddlIBP_BillNo').val();
    var InvDt = $('#BillDate').val();
    var InterbrId = $('#Hdn_interbrnchid').val();
    var supp_id = $('#ddlSupplierName').val();

    if (InvNo == "---Select---" || InvNo == "0") {
        $('#SpanddlIBP_BillNoErrorMsg').text($("#valueReq").text());
        $("#SpanddlIBP_BillNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "red");
        $("#SpanddlIBP_BillNoErrorMsg").css("border-color", "red");
    }
    else {
        $('#ddlIBP_BillNo').attr("disabled", true);
        $('#BillDate').attr("disabled", true);

        $("#SpanddlIBP_BillNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "#ced4da");
        //$("#ddlIBP_BillNo").css("border-color", "#ced4da");
        // $("#BillDate").attr("disabled", "disabled");
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/InterBranchPurchase/GetInterBranchSaleInvoiceDetail",
                data: { InvNo: InvNo, InvDt: InvDt, InterbrId: InterbrId, supp_id: supp_id },
                success: function (data) {
                    //debugger
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            RowNo = RowNo + 1;
                            if (RowNo == "0") {
                                RowNo = 1;
                            }
                            var CountRows = rowIdx;
                            RowNo = "";
                            var AutoGenDN_forVarQty = "N";
                            //if (arr.Table6.length > 0) {//Added by Suraj Maurya on 29-03-2025
                            //    if (arr.Table6[0].AutoGenDN_forVarQty == "Y") {
                            //        AutoGenDN_forVarQty = "Y";
                            //    }
                            //}
                            for (var k = 0; k < arr.Table.length; k++) {
                                var subitmDisable = "";
                                if (arr.Table[k].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                if (arr.Table[k].state_code == $("#Ship_StateCode").val()) {
                                    $("#Hd_GstType").val("Both")
                                } else {
                                    $("#Hd_GstType").val("IGST")
                                }
                                // $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                                var BaseVal;
                                // BaseVal = (parseFloat(arr.Table[0].conv_rate) * parseFloat(arr.Table[k].net_val_spec)).toFixed(ValDecDigit)
                                BaseVal = (parseFloat(arr.Table[k].conv_rate) * parseFloat(arr.Table[k].net_val_spec)).toFixed(ValDecDigit)
                                var S_NO = $('#POInvItmDetailsTbl tbody tr').length + 1;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var deletetext = $("#Span_Delete_Title").text();
                                var ManualGst = "";
                                var TaxExempted = "";
                                var disabled = "";

                                var ForDisableTaxCal = "";
                                var Icon = "";
                                var ItemType = arr.Table[k].ItemType;
                                if (ItemType == "Consumable") {
                                    disabled = "disabled"
                                    disabled_price = ""
                                    ForDisableTaxCal = "Enable"
                                }
                                else if (ItemType == "Service") {
                                    disabled = ""
                                    disabled_price = "";
                                    ForDisableTaxCal = "Enable"
                                }
                                else {
                                    disabled = "disabled"
                                    disabled_price = "disabled"
                                    ForDisableTaxCal = "Disabled"
                                }
                                ++CountRows;
                                // Set flags
                                let rowData = arr.Table[k];
                                let isBatch = rowData.i_batch === "Y";
                                let isSerial = rowData.i_serial === "Y";

                                // Create buttons with conditional disabling
                                let batchDisabled = isBatch ? "" : "disabled";
                                let serialDisabled = isSerial ? "" : "disabled";
                                var packSize = "";
                                if (arr.Table[k].pack_size != null) {
                                    packSize = arr.Table[k].pack_size;
                                }
                                $('#POInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">

                           <td class="sr_padding" id="srno">${S_NO}</td>
                        <td hidden>
                            <input id="TxtGrnNo" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].mr_no}" name="GrnNo" placeholder="${$("#span_GRNNumber").text()}"   disabled>
                        </td>
                        <td hidden>
                            <input id="TxtGrnDate" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].mr_dt}" name="GrnNo" placeholder="${$("#span_GRNDate").text()}"  onblur="this.placeholder='${$("#span_GRNDate").text()}'" disabled>
                            <input  type="hidden" id="hfGRNDate" value="${arr.Table[k].mr_date}" />
                        </td>
                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class="lpo_form">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="TxtItemName${CountRows}" class="form-control time" autocomplete="off" type="text" name="" value='${arr.Table[k].item_name}' placeholder='${$("#ItemName").text()}' onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                                                    <input type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                                                    <input class="" type="hidden" id="hdn_item_gl_acc" value="${arr.Table[k].item_acc_id}" />
                                                                    <input hidden type="text" id="sub_item" value='${arr.Table[k].sub_item}' />
                                                                    <input type="hidden" id="SNohiddenfiled" value="${CountRows}" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon pl-0">
                                                                    <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal"
                                                                    data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                                                                    <img src="/Content/Images/iIcon1.png" alt="" title="Item Information"></button>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value="${arr.Table[k].uom_name}"  onblur="this.placeholder='${$("#ItemUOM").text()}'" disabled>
                                                            <input type="hidden" id="hfUOMID" value="${arr.Table[k].uom_id}" />
                                                            <input id="ItemHsnCode" value="${arr.Table[k].hsn_no}" type="hidden" />
                                                            <input type="hidden" id="ItemtaxTmplt" value="${arr.Table[k].tmplt_id}" />
                                                            <input type="hidden" id="hdnItemType" value="${arr.Table[k].ItemType}" />
                                                            <input type="hidden" id="ForDisable" value="${ForDisableTaxCal}" />
                                                        </td>
                                                        
                                                                    <td>
                                                                        <input id="PackSize" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="${packSize}" onmouseover="OnMouseOver(this)" disabled>
                                                                    </td>
                                                        <td>
                                                           <div class="lpo_form">
                                                             <div class="col-sm-9 num_right no-padding">
                                                            <input id="Txtinv_qty" ${disabled_price} onchange="OnChangeConsumbleItemPrice(event)" onkeypress="return AmountFloatQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="Rate" placeholder="0000.00" value="${parseFloat(arr.Table[k].inv_qty).toFixed(RateDecDigit)}" >
                                                         </div>
                                                        <div class="col-sm-3 i_Icon no-padding" id="div_SubItemReceivedQty" >
                                                                <button type="button" id="SubItemReceivedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('IBP', event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </div>
                                                            <span id="Txtinv_qty_Error" class="error-message is-visible"></span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="item_rate" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="${parseFloat(arr.Table[k].item_rate).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td hidden>
                                                            <input id="TxtItemGrossValue" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="${parseFloat(arr.Table[k].item_gr_val).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtItemGrossValueInBase" class="form-control num_right" autocomplete="off" type="text" name="GrossValueInBase" value="${parseFloat(arr.Table[k].item_gr_val).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                    <div class="col-sm-9 num_right no-padding" >
                                                        <input id="Txtitem_tax_amt" class="form-control num_right" autocomplete="off" value='${parseFloat(arr.Table[k].item_tax_amt).toFixed(ValDecDigit)}' type="text" name="item_tax_amt" placeholder="0000.00"  disabled>
                                                </div><div class=" col-sm-3 num_right no-padding"><button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                        </td>
                                                        <td>
                                                            <input id="Txtitem_tax_amt_NonRecovAmt" class="form-control num_right" autocomplete="off" value='${parseFloat(arr.Table[k].item_NonRecovAmt).toFixed(ValDecDigit)}' type="text" name="item_tax_amt" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                           <input id="Txtitem_tax_amt_RecovAmt" class="form-control num_right" autocomplete="off" value='${parseFloat(arr.Table[k].item_RecovAmt).toFixed(ValDecDigit)}' type="text" name="item_tax_amt" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtOtherCharge" class="form-control num_right"  value='${parseFloat(arr.Table[k].item_oc_amt).toFixed(ValDecDigit)}' autocomplete="off" type="text" name="OtherCharge" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td hidden>
                                                            <input id="TxtNetValue" class="form-control num_right" value="${parseFloat(arr.Table[k].item_net_val_bs).toFixed(ValDecDigit)}" autocomplete="off" type="text" name="NetValue"  placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtNetValueInBase" class="form-control num_right" value="${parseFloat(arr.Table[k].item_net_val_bs).toFixed(ValDecDigit)}" autocomplete="off" type="text" name="NetValueInBase" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td class="num_right">
                                                               <input id="TxtLandedcostperpc" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(arr.Table[k].item_Landedcostperpc).toFixed(5)}" name="TxtLandedcostperpc" placeholder="0000.00" disabled>
                                                          
                                                        </td>
                                                     <td class="num_right">
                                                               <input id="TxtLandedcost" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(arr.Table[k].item_netLandedcost).toFixed(ValDecDigit)}" name="TxtLandedcost" placeholder="0000.00" disabled>
                                                        </td>
                                                   <td>
<div class="lpo_form">
                                                   <select class="form-control" id="wh_id${CountRows}" name="wh_id${CountRows}" onchange="OnChnageWarehouse(event)"></select>
                                                                                    <input type="hidden" id="Hdn_GRN_WhName${CountRows}" value="" style="display: none;">
                                                                                    <span id="wh_Error${S_NO}" class="error-message is-visible"></span>
</div>
                                                  </td>
                                                  <td class="num_right">
                                                               <input id="TxtLotno" class="form-control num_right" autocomplete="off" type="text" value="" name="TxtLotno" placeholder="0000.00" disabled>
                                                        </td>
                                                        <td class="center"><button type="button" id="BtnBatchDetail" onclick="ItemStockBatchWise(this,event)" class="calculator subItmImg" data-toggle="modal" data-target="#ExternalReceiptBatchNumber" data-backdrop="static" data-keyboard="false" ${batchDisabled}><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                                        <input type="hidden" id="hdi_batch" value="" style="display: none;">
                                                        </td>
                                                        <td class="center">
                                                        <button type="button" id="BtnSerialDetail" onclick="ItemStockSerialWise(this,event)" class="calculator subItmImg " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" ${serialDisabled}><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
                                                        <input type="hidden" id="hdi_serial" value="N" style="display: none;">
                                                        </td>
                                                        <td class="num_right">                                     
                                                        <textarea id="TxtRemarks" class="form-control remarksmessage" maxlength="400" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="ItemRemarks"  placeholder="Remarks">${$("#span_remarks").val()}</textarea>
                                                        </td>
                                            </tr>`);
                                BindWarehouseList(CountRows);
                            }

                            $('#Tbl_ItemTaxAmountList tbody tr').remove();
                            if (arr.Table1.length > 0) {
                                var ArrTaxList = [];
                                for (var l = 0; l < arr.Table1.length; l++) {
                                    if (!arr.Table1[l].tax_name) {
                                        arr.Table1[l].tax_name = "Unknown";
                                    }
                                    $('#Hdn_TaxCalculatorTbl tbody').append(`
                                    <tr id="${++rowIdx}">
                                        <td id="DocNo">${arr.Table1[l].mr_no}</td>
                                        <td id="DocDate">${arr.Table1[l].mr_date}</td>
                                        <td id="TaxItmCode">${arr.Table1[l].item_id}</td>
                                        <td id="TaxName">${arr.Table1[l].tax_name}</td>
                                        <td id="TaxNameID">${arr.Table1[l].tax_id}</td>
                                        <td id="TaxPercentage">${arr.Table1[l].tax_rate}</td>
                                        <td id="TaxLevel">${arr.Table1[l].tax_level}</td>
                                        <td id="TaxApplyOn">${arr.Table1[l].tax_apply_Name}</td>
                                        <td id="TaxAmount">${arr.Table1[l].tax_val}</td>
                                        <td id="TotalTaxAmount">${arr.Table1[l].item_tax_amt}</td>
                                        <td id="TaxApplyOnID">${arr.Table1[l].tax_apply_on}</td>
                                        <td id="TaxRecov">${arr.Table1[l].recov}</td>
                                        <td id="TaxAccId">${arr.Table1[l].tax_acc_id}</td>
                                    </tr>`);
                                    if (ArrTaxList.findIndex(v => (v.taxID) == (arr.Table1[l].tax_id)) > -1) {
                                        let getIndex = ArrTaxList.findIndex(v => (v.taxID) == (arr.Table1[l].tax_id));
                                        ArrTaxList[getIndex].totalTaxAmt = parseFloat(CheckNullNumber(ArrTaxList[getIndex].totalTaxAmt)) + parseFloat(CheckNullNumber(arr.Table1[l].tax_val));
                                    } else {
                                        ArrTaxList.push({ taxID: arr.Table1[l].tax_id, taxAccID: arr.Table1[l].tax_acc_id, taxName: arr.Table1[l].tax_name, totalTaxAmt: arr.Table1[l].tax_val, TaxRecov: arr.Table1[l].recov })
                                    }
                                }

                                let TotalTAmt = 0;
                                for (var l = 0; l < ArrTaxList.length; l++) {
                                    $("#Tbl_ItemTaxAmountList tbody").append(`
                            <tr>
                                <td>${ArrTaxList[l].taxName}</td>
                                <td id="taxRecov" class="center">
                                    ${(ArrTaxList[l].TaxRecov === 'Y' ?
                                            '<i class="fa fa-check text-success " aria-hidden="true"></i>' :
                                            '<i class="fa fa-times-circle text-danger" aria-hidden="true"></i>')}
                                </td>
                                <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                                <td hidden="hidden" id="taxID">${ArrTaxList[l].tax_id}</td>
                                <td hidden="hidden" id="taxAccID">${ArrTaxList[l].taxAccID}</td>
                            </tr>`);
                                    TotalTAmt += parseFloat(ArrTaxList[l].totalTaxAmt);
                                } $("#_ItemTaxAmountTotal").text(parseFloat(TotalTAmt).toFixed(ValDecDigit));
                            }

                            if (arr.Table3.length > 0) {
                                var rowIdx = 0;
                                for (var y = 0; y < arr.Table3.length; y++) {
                                    var ItmId = arr.Table3[y].item_id;
                                    var SubItmId = arr.Table3[y].sub_item_id;
                                    var SubItmName = arr.Table3[y].sub_item_name;
                                    var GrnQty = arr.Table3[y].Qty;
                                    var avl_qty = arr.Table3[y].avl_qty;

                                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemQty" value='${GrnQty}'></td>
                                    <td><input type="text" id="subItemAvlStk" value='${avl_qty}'></td>
                                    </tr>`);
                                }
                            }

                            if (arr.Table4.length > 0) {
                                var rowIdx = 0;
                                for (var y = 0; y < arr.Table4.length; y++) {
                                    var lot_id = arr.Table4[y].lot_id;
                                    var bt_sale = arr.Table4[y].bt_sale;
                                    var batch_no = arr.Table4[y].batch_no;
                                    var Issue_Qty = arr.Table4[y].Issue_Qty;
                                    var expiry_date = arr.Table4[y].expiry_date;
                                    var exp_dt = arr.Table4[y].exp_dt;
                                    var avl_batch_qty = arr.Table4[y].avl_batch_qty;

                                    var BatchInvoiceQty = 0;//row.find("#BatchInvoiceQty").val();
                                    var ItemUOMID = 0//$("#HDUOMBatchWise").val();
                                    var ItemId = arr.Table4[y].item_id;
                                    var ItemBatchNo = batch_no;
                                    var ItemExpiryDate = exp_dt;//row.find("#hfExDate").val();
                                    var AvailableQty = Issue_Qty;// row.find("#AvailableQuantity").val();
                                    var LotNo = lot_id;//row.find("#Lot").val();
                                    var POInvItmDetailsTblRow = $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value='" + ItemId + "']").closest('tr');
                                    var sr_no = POInvItmDetailsTblRow.find("#SNohiddenfiled").val();
                                    var bt_wh_id = POInvItmDetailsTblRow.find("#wh_id" + sr_no).val();;
                                    $('#SaveItemBatchTbl tbody').append(
                                        `<tr>
                                        <td><input type="text" id="ItemID" value="${ItemId}" /></td>
                                        <td><input type="text" id="BatchNo" value="${ItemBatchNo}" /></td>
                                        <td><input type="text" id="BatchExDate" value="${ItemExpiryDate}" /></td>
                                        <td><input type="text" id="BatchQty" value="${AvailableQty}" /></td>
                                        <td><input type="text" id="Lot" value="${LotNo}" /></td>
                                        <td id="bt_mfg_name">${arr.Table4[y].mfg_name}</td>
                                        <td><input type="text" id="bt_mfg_mrp" value="${arr.Table4[y].mfg_mrp}" /></td>
                                        <td><input type="text" id="bt_mfg_date" value="${arr.Table4[y].mfg_date}" /></td>
                                    </tr>`);
                                }
                            }
                            $("#BatchNumber").modal('hide');

                            if (arr.Table5.length > 0) {
                                for (var l = 0; l < arr.Table5.length; l++) {
                                    var stk_ItemId = arr.Table5[l].item_id;
                                    var stk_UOMId = arr.Table5[l].uom_id;
                                    var stk_LOTNo = arr.Table5[l].lot_id;
                                    var stk_IssueQty = arr.Table5[l].issue_qty;
                                    var stk_SerialNO = arr.Table5[l].serial_no;
                                    var stk_InvQty = arr.Table5[l].Inv_qty;
                                    $('#SaveItemSerialTbl tbody').append(
                                        `<tr>
                                        <td><input type="text" id="Scrap_lineSerialItemId" value="${stk_ItemId}" /></td>
                                        <td><input type="text" id="Scrap_lineSerialUOMId" value="${stk_UOMId}" /></td>
                                        <td><input type="text" id="Scrap_lineSerialLOTNo" value="${stk_LOTNo}" /></td>
                                        <td><input type="text" id="Scrap_lineSerialIssueQty" value="${stk_IssueQty}" /></td>
                                        <td><input type="text" id="Scrap_lineBatchSerialNO" value="${stk_SerialNO}" /></td>
                                        <td><input type="text" id="Scrap_lineSerialInvQty" value="${stk_InvQty}" /></td>
                                        <td id="sr_mfg_name">${arr.Table5[l].mfg_name}</td>
                                        <td><input type="text" id="sr_mfg_mrp" value="${arr.Table5[l].mfg_mrp}" /></td>
                                        <td><input type="text" id="sr_mfg_date" value="${arr.Table5[l].mfg_date}" /></td>
                                    </tr>`);
                                }
                            }
                            //Total Amounts
                            if (arr.Table6.length > 0) {
                                var rowIdx = 0;
                                for (var y = 0; y < arr.Table6.length; y++) {
                                    var TotalValue = arr.Table6[y].gr_val;
                                    var TotalNetAmount = arr.Table6[y].net_val_bs;
                                    var TotalTaxAmount = arr.Table6[y].tax_amt;
                                    var OCAmount = arr.Table6[y].oc_amt_with_tp;
                                    // var TotalNetAmount_f = TotalNetAmount - OCAmount;
                                    //var TotalNetAmount_f = (parseFloat(TotalNetAmount) - parseFloat(OCAmount)).toFixed(ValDecDigit);
                                    var TotalNetAmount_f = (parseFloat(TotalValue) + parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                                    $("#TxtGrossValue").val(TotalValue);//.toFixed(ValDecDigit));
                                    $("#TxtTaxAmount").val(TotalTaxAmount);//.toFixed(ValDecDigit));
                                    $("#TxtOtherCharges").val(parseFloat(0.00));//.toFixed(ValDecDigit));
                                    $("#NetOrderValueInBase").val(TotalNetAmount_f);//.toFixed(ValDecDigit));
                                    $("#NetOrderValueInBase_fnl").val((TotalNetAmount_f));
                                }
                            }
                            GetAllGLID();
                        } /*<option value="${arr.Table[k].wh_id}">"${arr.Table[k].wh_name}"</option>*/
                        else {
                            $('#ddlIBP_BillNo').attr("disabled", false);
                            $('#BillDate').attr("disabled", false);
                            swal("", $("#span_ItmNotFound").text(), "warning");
                        }
                    }
                },
            });

        } catch (err) {
            console.log("InterBranchPurchaseError : " + err.message);
        }
    }
}

function BindWarehouseList(id) {
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/GetWarehouseList1",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        var PreWhVal = $("#wh_id" + id).val();
                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        $("#wh_id" + id).html(s);
                        $("#wh_id" + id).val(IsNull(PreWhVal, '0'));
                        $("#wh_id" + id).select2();
                    }
                }
            },
        });
}

function ItemStockBatchWise(el, evt) {
    var QtyDigit = $("#QtyDigit").text()
    var RateDigit = $("#RateDigit").text()
    var currentrow = $(evt.target).closest('tr');
    var SNohf = currentrow.find("#SNohiddenfiled").val();
    var item_id = currentrow.find("#hfItemID").val();
    var ExpireAble_item = currentrow.find("#hfexpiralble").val();
    var TxtInvoiceQuantity = currentrow.find("#Txtinv_qty").val();
    if (ExpireAble_item == "Y") {
        $("#spanexpiryrequire").show();
    }
    else {
        $("#spanexpiryrequire").hide();
    }
    var uom_name = currentrow.find("#TxtUOM").val();
    var uom_id = currentrow.find("#hfUOMID").val();
    var ReceivedQuantity = currentrow.find("#Txtinv_qty").val();
    var Item_name = currentrow.find("#TxtItemName" + SNohf).val();
    var hdn_BatchCommand = $("#hdn_BatchCommand").val();
    var GreyScale = "";
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");

    if (hdn_BatchCommand == "N") {
        $("#BatchResetBtn").css("display", "block")
        $("#BatchSaveAndExitBtn").css("display", "block")
        $("#BatchDiscardAndExitBtn").css("display", "block")
        $("#BatchClosebtn").css("display", "none")
        $("#DivAddNewBatch").css("display", "block")
        $("#txtBatchNumber").attr("disabled", false);
        $("#BatchExpiryDate").attr("disabled", false);
        $("#BatchQuantity").attr("disabled", false);
    }
    else {
        $("#BatchResetBtn").css("display", "none")
        $("#BatchSaveAndExitBtn").css("display", "none")
        $("#BatchDiscardAndExitBtn").css("display", "none")
        $("#BatchClosebtn").css("display", "block")
        GreyScale = "style='filter: grayscale(100%)'";
        $("#DivAddNewBatch").css("display", "none");
        $("#txtBatchNumber").attr("disabled", true);
        $("#BatchExpiryDate").attr("disabled", true);
        $("#BatchQuantity").attr("disabled", true);
    }
    debugger;
    if (item_id != null && item_id != "" && item_id != "0") {
        $("#BatchItemName").val(Item_name);
        $("#BatchItemID").val(item_id);
        $("#hfItemSNo").val(SNohf);
        $("#batchUOM").val(uom_name);
        $("#hfexpiryflag").val(ExpireAble_item);
        if (ReceivedQuantity != "" && parseFloat(ReceivedQuantity) != 0 && ReceivedQuantity != null) {
            $("#BatchReceivedQuantity").val(ReceivedQuantity);
        }
        else {
            $("#BatchReceivedQuantity").val("");
        }
        if (ReceivedQuantity == "NaN" || ReceivedQuantity == "" || ReceivedQuantity == "0") {
            $("#BtnBatchDetail").attr("data-target", "");
            return false;
        }
        else {
            $("#BtnBatchDetail").attr("data-target", "#ExternalReceiptBatchNumber");
        }
        var rowIdx = 0;
        var Count = $("#SaveItemBatchTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                $("#BatchWiseItemStockTbl >tbody >tr").remove();
                $("#SaveItemBatchTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    var SUserID = SaveBatchRow.find("#BatchUserID").val();
                    var SRowID = SaveBatchRow.find("#RowSNo").val();
                    var SItemID = SaveBatchRow.find("#ItemID").val();
                    var BatchExDate = SaveBatchRow.find("#BatchExDate").val();
                    var BatchQty = SaveBatchRow.find("#BatchQty").val();
                    var BatchNo = SaveBatchRow.find("#BatchNo").val();
                    var Lot = SaveBatchRow.find("#Lot").val();
                    var MfgName = SaveBatchRow.find("#bt_mfg_name").text();
                    var MfgMrp = parseFloat(CheckNullNumber(SaveBatchRow.find("#bt_mfg_mrp").val())).toFixed(RateDigit);
                    var MfgDate = SaveBatchRow.find("#bt_mfg_date").val();
                    var mfg_date = "";
                    if (MfgDate != null && MfgDate != "") {

                        if (MfgDate == "1900-01-01") {
                            mfg_date = "";
                        } else {
                            mfg_date = moment(MfgDate).format('DD-MM-YYYY');
                        }
                    }
                    var BatchQtyTotal = 0;
                    if (SNohf != null && SNohf != "") {
                        if (SItemID == item_id) {
                            var date = "";
                            if (BatchExDate != "" && BatchExDate != null) {
                                if (BatchExDate == "1900-01-01") {
                                    date = "";
                                }
                                else {
                                    //date = moment(BatchExDate).format('DD-MM-YYYY');
                                    //var parsedDate = moment(BatchExDate.toString());
                                    //var parsedDate = moment(BatchExDate, "YYYY-MM-DD", true);
                                    //if (parsedDate.isValid()) {
                                    //    date = parsedDate.format('DD-MM-YYYY');
                                    //}
                                    //var parsedDate = moment(BatchExDate, "YYYY-MM-DDTHH:mm:ss", true);
                                    //if (parsedDate.isValid()) {
                                    //    date = parsedDate.format("DD-MM-YYYY");
                                    //}

                                    var parsedDate = moment(BatchExDate, ["YYYY-MM-DD", "YYYY-MM-DDTHH:mm:ss", "M/D/YYYY h:mm:ss A"], true);
                                    if (parsedDate.isValid()) {
                                        date = parsedDate.format("DD-MM-YYYY");
                                    } else {
                                        date = null;
                                    }
                                }
                            }
                            $("#ItemNameBatchWise").val(Item_name);
                            $("#UOMBatchWise").val(uom_name);
                            $("#HDItemNameBatchWise").val(item_id);
                            $("#HDUOMBatchWise").val(uom_id);
                            $("#QuantityBatchWise").val(parseFloat(BatchQty).toFixed(QtyDigit));
                            $('#BatchWiseItemStockTbl tbody').append(`<tr id="R${++rowIdx}">
                                <td id="Lot">${Lot}</td>
                                <td id="BatchNo" >${BatchNo}</td>
                                <td id="BtMfgName" >${MfgName}</td>
                                <td id="BtMfgMrp" class="num_right">${MfgMrp}</td>
                                <td id="BtMfgDate" class="num_right">${mfg_date}</td>
                                <td id="BatchExDate">${date}</td>
                                <td hidden="hidden">${date}</td>
                                <td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(QtyDigit)}</td>
                            </tr>`);
                            $("#div_BatchWiseItemStockTbl").removeClass("price_item_detail")
                        }
                    }
                });
                $("#BatchwiseTotalIssuedQuantity").text(TxtInvoiceQuantity);
            }
        }
    }
}

function BindItemBatchDetails(ItemId) {
    var batchrowcount = $('#SaveItemBatchTbl tr').length;
    if (batchrowcount > 1) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            if (parseFloat(CheckNullNumber(row.find('#scrap_lineBatchINVQty').val())) > 0) {
                var batchList = {};
                batchList.LotNo = row.find('#scrap_lineBatchLotNo').val();
                batchList.ItemId = row.find('#scrap_lineBatchItemId').val();
                batchList.UOMId = row.find('#scrap_lineBatchUOMId').val();
                batchList.BatchNo = row.find('#scrap_lineBatchBatchNo').val();
                batchList.inv_qty = row.find('#scrap_lineBatchINVQty').val();
                batchList.ExpiryDate = row.find('#hfExDate').val();
                batchList.avl_batch_qty = row.find('#scrap_lineBatchavl_batch_qty').val();
                batchList.wh_id = row.find('#scrap_lineBatchWh_id').val();
                ItemBatchList.push(batchList);
            }
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }
}

function OnClickIconBtn(e) {
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode)
}

function SubItemDetailsPopUp(flag, e) {
    //debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#TxtItemName" + hfsno).val();
    var ProductId = clickdRow.find("#hfItemID").val();
    var wh_id = clickdRow.find("#wh_id" + hfsno).val();
    var UOM = clickdRow.find("#TxtUOM").val();
    var UOMID = clickdRow.find("#UOMID").val();
    var doc_no = $("#InvoiceNumber").val();
    var doc_dt = $("#Sinv_dt").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.sub_item_name = row.find('#subItemName').val();
        List.avl_stk = "0";//row.find('#avlqty').val();
        List.qty = row.find('#subItemQty').val();
        NewArr.push(List);
    });
    //if (flag == "Scrap") {
    //    Sub_Quantity = clickdRow.find("#TxtInvoiceQuantity").val();
    //}
    Sub_Quantity = clickdRow.find("#Txtinv_qty").val();
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InterBranchPurchase/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            wh_id: wh_id,
            UomId: UOMID
        },
        success: function (data) {
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });
}

function OnChangeWarehouse(el, evt) {
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SNohiddenfiled").val();
    var ddlId = "#wh_id" + Index;
    var whERRID = "#wh_Error";
    if (clickedrow.find(ddlId).val() == "0") {
        clickedrow.find(whERRID).text($("#valueReq").text());
        clickedrow.find("#wh_id" + Index).css("border-color", "Red");
        clickedrow.find('[aria-labelledby="select2-' + "wh_id" + Index + '-container"]').css("border-color", "red");
        clickedrow.find(whERRID).css("display", "block");
    }
    else {
        var WHName = $("#wh_id option:selected").text();
        $("#hdwh_name").val(WHName)
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find('[aria-labelledby="select2-' + "wh_id" + Index + '-container"]').css("border-color", "#ced4da");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
    }
    var ItemId = clickedrow.find("#hfItemID").val();
    var WarehouseId = clickedrow.find(ddlId).val();
    var QtyDecDigit = $("#QtyDigit").text();
    if (WarehouseId != "0" && WarehouseId != null) {
        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
            },
            success: function (data) {
                //var QtyDecDigit = $("#QtyDigit").text();///Quantity

                var avaiableStock = JSON.parse(data);
                var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                clickedrow.find("#AvailableStock").val(parseavaiableStock);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
        //subitemInvoiceqty(clickedrow)
    }
}

function OnClickTaxCalBtn(e) {
    var currentRow = $(e.currentTarget).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();
    var SOItemName = "#TxtItemName" + Sno;
    var SNohiddenfiled = "SI";
    //var SNohiddenfiled = "SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemName);
}

function OnClickSaveAndExit_OC_Btn() {
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueSpe", "#NetOrderValueInBase");
}
function SetOtherChargeVal() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
    })
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    //debugger;
    var TotalGAmt = $("#TxtGrossValue").val();
    //var ConvRate = $("#conv_rate").val();
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCValue = currentRow.find("#TxtOtherCharge").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }
        var TaxNonRecov = currentRow.find("#Txtitem_tax_amt_NonRecovAmt").val();
        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#TxtNetValueInBase").val((parseFloat(CheckNullNumber(POItm_NetOrderValueSpec))).toFixed(ValDecDigit));
        let DPIQty = currentRow.find("#Txtinv_qty").val();

        let LandedCostValue = parseFloat(CheckNullNumber(POItm_GrossValue)) + parseFloat(CheckNullNumber(POItm_OCAmt)) + parseFloat(CheckNullNumber(TaxNonRecov));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
        currentRow.find("#TxtLandedcost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedcostperpc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
    });
    CalculateAmount();
};

function CalculateAmount(flag) {
    //debugger
    //var DecDigit = $("#ValDigit").text();
    const ValDecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var NetDnAmt = parseFloat(0).toFixed(ValDecDigit);

    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TxtItemGrossValueInBase").val() == "" || currentRow.find("#TxtItemGrossValueInBase").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(ValDecDigit);
        }

        if (currentRow.find("#TxtNetValueInBase").val() == "" || currentRow.find("#TxtNetValueInBase").val() == null || currentRow.find("#TxtNetValueInBase").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TxtNetValueInBase").val() == "" || currentRow.find("#TxtNetValueInBase").val() == null || currentRow.find("#TxtNetValueInBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValueInBase").val())).toFixed(ValDecDigit);
        }
        NetDnAmt = parseFloat(NetDnAmt) + parseFloat(CheckNullNumber(currentRow.find("#TxtLandedcost").val()));

    });
    $("#TxtGrossValue").val(GrossValueInBase);
    var oc_amount = $("#TxtDocSuppOtherCharges").val();
    //var oc_amount = $("#TxtOtherCharges").val();
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValueInBase) + parseFloat(CheckNullNumber(oc_amount));
    NetOrderValueSpec = NetOrderValueBase / parseFloat(CheckNullNumber(ConvRate));
    //$("#TxtGrossValueInBase").val(GrossValueInBase);
    $("#TxtTaxAmount").val(TaxValue);
    $("#NetOrderValueSpe").val(NetOrderValueSpec.toFixed(ValDecDigit));
    $("#TxtVarDn_Amount").val(NetDnAmt);/* For Variance Total DN Amt */
    if ($("#chk_roundoff").is(":checked")) {
        var roundoff_netval = Math.round(NetOrderValueBase);
        var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
        $("#NetOrderValueInBase").val(f_netval);
    }
    else {
        $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(ValDecDigit));
    }
    $("#NetOrderValueInBase_fnl").val(NetOrderValueBase.toFixed(ValDecDigit));
    if (flag == "CallByGetAllGL") {
        ApplyRoundOff("CallByGetAllGL");
    }
}

function BindOtherChargeDeatils(val) {
    var DecDigit = $("#ValDigit").text();
    var hdbs_curr = $("#hdbs_curr").val();/*Add by Hina on 20-07-2024 for third party OC*/
    var hdcurr = $("#hdcurr").val();
    var OCTaxable = "Y";
    if (hdbs_curr == hdcurr) {
        OCTaxable = "Y";
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            var tdSupp = "";
            if (OCTaxable == "Y") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }

            tdSupp = `<td>${currentRow.find("#td_OCSuppName").text()}</td>`

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
`+ tdSupp + `
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = parseFloat((parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text()))).toFixed(DecDigit);
            if (OCTaxable == "Y") {
                TotalTaxAMount = parseFloat((parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text()))).toFixed(DecDigit);
                TotalAMountWT = parseFloat((parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text()))).toFixed(DecDigit);
            }
        });
    }
    $("#_OtherChargeTotal").text(TotalAMount);
    if (OCTaxable == "Y") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
    if (val == "") {
        GetAllGLID();
    }
}


function GetAllGLID(flag) {
    GetAllGL_WithMultiSupplier(flag);
}
async function GetAllGL_WithMultiSupplier(flag) {
    console.log("GetAllGL_WithMultiSupplier")
    debugger;
    if ($("#POInvItmDetailsTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    //if (CheckSPI_ItemValidationsForGL() == false) {
    //    return false;
    //}
    //CalculateAmount("CallByGetAllGL");
    CalculateAmount("CallByGetAllGL");
    //console.log("GetAllGL_WithMultiSupplier")/*commented by Hina on 10-01-2025 */
    //if (flag != "RO") {
    //    if ($("#chk_roundoff").is(":checked")) {
    //        await addRoundOffToNetValue("CallByGetAllGL");
    //    }
    //}

    var Compid = $("#CompID").text();
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    /* var conv_rate = $("#conv_rate").val();*/
    var conv_rate = 1;
    var ValDecDigit = $("#ValDigit").text();
    var NetInvValue = $("#NetOrderValueInBase").val();
    var NetTaxValue = $("#TxtTaxAmount").val();
    var NetTcsValue = CheckNullNumber($("#TxtTDS_Amount").val());
    var ValueWithoutTax = (parseFloat(NetInvValue) - parseFloat(NetTaxValue));
    var supp_id = $("#ddlSupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    /*SuppValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);*/
    SuppValInBase = (parseFloat(NetInvValue) - parseFloat(NetTcsValue)).toFixed(ValDecDigit);//Changed by Suraj Maurya on 07-02-2025 for substract tcs.
    SuppVal = parseFloat((parseFloat(SuppValInBase) / parseFloat(conv_rate))).toFixed(ValDecDigit);
    var InvType = "D";
    var curr_id = $("#hdcurr").val();
    var bill_no = $("#ddlIBP_BillNo").val();
    var bill_dt = $("#BillDate").val();
    var dateParts = bill_dt.split('-');
    // Reformat it to yyyy-dd-mm
    //var formattedDate = dateParts[2] + '-' + dateParts[1] + '-' + dateParts[0];
    //bill_dt = formattedDate;
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
    });
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        var ItmGrossVal = currentRow.find("#TxtItemGrossValueInBase").val();
        var ItmGrossValInBase = currentRow.find("#TxtItemGrossValueInBase").val();
        var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
        var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
        //if (currentRow.find("#TaxExempted").is(":checked")) {
        //    TxaExantedItemList.push({ item_id: item_id });
        //}
        //if (ItemTaxAmt == TaxAmt) {
        //    if (currentRow.find("#ManualGST").is(":checked")) {
        //        TxaExantedItemList.push({ item_id: item_id });
        //    }
        //}
        GLDetail.push({
            comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
            , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: item_acc_id
        });
    });
    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var oc_id = currentRow.find("#OC_ID").text();
        var oc_acc_id = currentRow.find("#OCAccId").text();
        var oc_amt = currentRow.find("#OCAmtSp").text();
        var oc_amt_bs = currentRow.find("#OCAmtBs").text();
        var oc_supp_id = currentRow.find("#td_OCSuppID").text();
        var oc_supp_acc_id = currentRow.find("#OCSuppAccId").text();
        var oc_supp_type = currentRow.find("#td_OCSuppType").text();
        var oc_amt_bs_wt = currentRow.find("#OCTotalTaxAmt").text();
        var oc_conv_rate = currentRow.find("#OC_Conv").text();
        var oc_curr_id = currentRow.find("#HdnOC_CurrId").text();
        var oc_supp_bill_no = currentRow.find("#OCBillNo").text();
        var oc_supp_bill_dt = currentRow.find("#OCBillDt").text();
        var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax
        var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? supp_acc_id : oc_supp_acc_id;

        GLDetail.push({
            comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt
            , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate
            , bill_no: oc_supp_bill_no == "" ? bill_no : oc_supp_bill_no
            , bill_date: oc_supp_bill_dt == "" ? bill_dt : oc_supp_bill_dt, acc_id: ""
        });

        if (oc_supp_id != "" && oc_supp_id != "0") {
            let gl_type = "Supp";
            if (oc_supp_type == "2")
                gl_type = "Supp";
            if (oc_supp_type == "7")
                gl_type = "Bank";

            GLDetail.push({
                comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: InvType, Value: oc_amt_wt
                , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: oc_supp_bill_no
                , bill_date: oc_supp_bill_dt, acc_id: ""
            });
        }
        else {

        }
    });
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var oc_id = currentRow.find("#TaxItmCode").text();
        var TaxPerc = currentRow.find("#TaxPercentage").text();
        var TaxPerc_id = TaxPerc.replace("%", "");
        var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
        GLDetail.push({
            comp_id: Compid, id: tax_id, type: "OcTax", doctype: InvType, Value: tax_amt
            , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: ArrOcGl[0].id
            , Entity_id: tax_acc_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate,
            bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
        });
        if (GstCat == "UR") { /* Added by Suraj Maurya on 04-08-2025 to Add in RCM all tax on OC for Self Supplier */
            tax_id = "R" + tax_id;
            $("#ht_Tbl_OC_Deatils >tbody >tr").find("#OC_ID:contains(" + oc_id + ")").closest('tr').each(function () {
                if ($(this).find("#td_OCSuppID").text() == "0" && $(this).find("#OC_ID").text() == oc_id) {
                    GLDetail.push({
                        comp_id: Compid, id: tax_id, type: "RCM", doctype: InvType, Value: tax_amt
                        , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                    });
                }
            });
        }
    });
    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        debugger
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();

        if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
            var GstApplicable = $("#Hdn_GstApplicable").text();
            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");//add by shubham maurya on 27-03-2025
            //if (TaxExempted == false) {//add by shubham maurya on 27-03-2025
            //    var ClaimItc = true;
            //    if (GstApplicable == "Y") {
            //        ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
            //    }
            //    if (TaxRecov == "N" || !ClaimItc) {
            //        if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
            //            var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
            //            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
            //            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
            //        }
            //    } else {
            //        GLDetail.push({
            //            comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
            //            , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
            //            , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            //        });
            //    }
            //}
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        }
    });
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            //debugger
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();

            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
                if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
                    objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
                    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                } else {
                    GLDetail.push({
                        comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal
                        , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                        , Entity_id: TaxAccID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""

                    });
                }
            }


        });
    }
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
        GLDetail.push({
            comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
            , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: supp_acc_id
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    });
    if (Cal_Tds_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "TSupp", doctype: InvType
            , Value: Cal_Tds_Amt, ValueInBase: Cal_Tds_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }
    var Oc_Tds = [];
    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        var tds_supp_id = currentRow.find("#td_TDS_Supp_Id").text();
        var ArrOcGl = GLDetail.filter(v => (v.id == tds_supp_id && v.type == "Supp"));
        var tdsIndex = Oc_Tds.findIndex(v => v.supp_id == tds_supp_id);
        if (tdsIndex > -1) {
            Oc_Tds[tdsIndex].tds_amt = parseFloat(Oc_Tds[tdsIndex].tds_amt) + parseFloat(tds_amt);
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate, bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        } else {
            Oc_Tds.push({
                supp_id: tds_supp_id, supp_acc_id: ArrOcGl[0].id, tds_amt: tds_amt
                , bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate, bill_no: ArrOcGl[0].bill_no
                , bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        }
    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item, idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: item.bill_no, bill_date: item.bill_date, acc_id: ""
            });
        });
    }
    await Cmn_GLTableBind(supp_acc_id, GLDetail, "Purchase")
}
function BindDDLAccountList() {
    //     Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohiddenfiled", "BindData", "105101153", "48");
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohiddenfiled", "BindData", "105101153");
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Narration").val() : VouType == "BP" ? $("#hdn_BP_Narration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
function BindData() {
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + rowid).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });
            });
        }
    }
    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        if (AccID != '0' && AccID != "") {
            //   currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#Acc_name_" + rowid).attr("onchange", "");
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#Acc_name_" + rowid).attr("onchange", "OnChangeAccountName(" + rowid + ", event)");
        }
    });
}
function OnChangeAccountName(RowID, e) {
    //debugger
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val().toString();
    var Acc_Name = clickedrow.find("#Acc_name_" + SNo + " option:selected").text();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').each(function () {
                var row = $(this);
                var vouSrNo = row.find("#hdntbl_vou_sr_no").text();
                if (vouSrNo == vou_sr_no) {
                    row.remove();
                }
            });
        }
        $("#POInvItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
            var row = $(this);
            row.find("#hdn_item_gl_acc").val(Acc_ID);
        });
    }
    if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }

    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#txthfAccID").val(Acc_Name);
    $("#hdnAccID").val(Acc_ID);
}
/***-------------------Roundoff------------------------------------***/
function checkMultiSupplier() {
    return true;
}
function click_chkroundoff() {
    //debugger
    if ($("#chk_roundoff").is(":checked")) {
        $("#div_pchkbox").show();
        $("#div_mchkbox").show();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
        var finalamt = $("#NetOrderValueInBase_fnl").val();
        $("#NetOrderValueInBase").val(finalamt);
        //$("#POInvItmDetailsTbl > tbody > tr").each(function () {
        //    //debugger
        //    var currentrow = $(this);
        //    //CalculateTaxExemptedAmt(currentrow, "N")
        //    CalculateTaxExemptedAmt(currentrow, "Row")
        //});
        GetAllGLID();
    }
}

function click_chkplusminus(AutoAplyTaxPI) {
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    //debugger
    const ValDecDigit = $("#ValDigit").text();
    if ($("#chk_roundoff").is(":checked")) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/InterBranchPurchase/CheckRoundOffAccount",
                    data: {},
                    success: function (data) {
                        //debugger
                        if (data == 'ErrorPage') {
                            PO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {
                                if (parseInt(arr[0]["r_acc"]) > 0) {
                                    ApplyRoundOff();
                                    //var grossval = $("#TxtGrossValue").val();
                                    //var taxval = $("#TxtTaxAmount").val();
                                    //var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());
                                    //var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);
                                    //var netval = finalnetval;
                                    //var fnetval = 0;

                                    //if (parseFloat(netval) > 0) {
                                    //    var finnetval = netval.split('.');
                                    //    if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                                    //        if ($("#p_round").is(":checked")) {
                                    //            var decval = '0.' + finnetval[1];
                                    //            var faddval = 1 - parseFloat(decval);
                                    //            fnetval = parseFloat(netval) + parseFloat(faddval);
                                    //            $("#pm_flagval").val($("#p_round").val());
                                    //        }
                                    //        if ($("#m_round").is(":checked")) {
                                    //            var decval = '0.' + finnetval[1];
                                    //            fnetval = parseFloat(netval) - parseFloat(decval);
                                    //            $("#pm_flagval").val($("#m_round").val());
                                    //        }
                                    //        var roundoff_netval = Math.round(fnetval);
                                    //        var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                    //        $("#NetOrderValueInBase").val(f_netval);
                                    //        GetAllGLID();
                                    //    }
                                    //}
                                }
                                else {
                                    swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                    $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                    return false;
                                }
                            }
                            else {
                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
                                return false;
                            }
                        }
                        else {
                            swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
                            $("#chk_roundoff").parent().find(".switchery").trigger("click");
                            return false;
                        }
                    },
                });
        } catch (err) {
            console.log("Purchase invoice round off Error : " + err.message);
        }
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
        if (AutoAplyTaxPI != "AutoAplyTaxPI") {
            $("#POInvItmDetailsTbl > tbody > tr").each(function () {
                //debugger
                var currentrow = $(this);
                //CalculateTaxExemptedAmt(currentrow, "N")
                CalculateTaxExemptedAmt(currentrow, "Row")
            });
        }
        GetAllGLID();
    }
}
/***----------------------end-------------------------------------***/

/***----------------------Insertion end-------------------------------------***/
function InsertIBPDetails() {
    //debugger
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    //var INSDTransType = sessionStorage.getItem("INSTransType");
    //if ($("#duplicateBillNo").val() == "Y") {
    //    $("#Bill_No").css("border-color", "Red");
    //    $('#SpanBillNoErrorMsg').text($("#valueduplicate").text());
    //    $("#SpanBillNoErrorMsg").css("display", "block");
    //    return false;
    //}
    //else {
    //    $("#SpanBillNoErrorMsg").css("display", "none");
    //    $("#Bill_No").css("border-color", "#ced4da");
    //}
    if (CheckDPI_Validations() == false) {
        return false;
    }
    if (CheckDPI_ItemValidations("N") == false) {
        return false;
    }
    let NetOrderValueInBase = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
    if (NetOrderValueInBase > 0) {

    }
    else {
        return false;
    }
    if (CheckDPI_VoucherValidations() == false) {
        return false;
    }
    var subitemValidation = Check_subitemValidation();
    if (subitemValidation == false)
        return false;
    //if (CheckItemBatchValidation() == false) {
    //    swal("", $("#PleaseenterBatchDetails").text(), "warning");
    //    return false;
    //}
    //if (CheckItemSerialValidation() == false) {
    //    swal("", $("#PleaseenterSerialDetails").text(), "warning");
    //    return false;
    //}
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        //if (Cmn_taxVallidation("POInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "POItemListName") == false) {
        //    return false;
        //}
    }
    let GrossAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
    let TxtTaxAmount = parseFloat(CheckNullNumber($("#TxtTaxAmount").val()));
    let TxtDocSuppOtherCharges = parseFloat(CheckNullNumber($("#TxtDocSuppOtherCharges").val()));
    var tt = (GrossAmt + TxtTaxAmount + TxtDocSuppOtherCharges);
    var diffranceAmt = Math.abs(NetOrderValueInBase - tt)
    //if (NetOrderValueInBase != (GrossAmt + TxtTaxAmount + TxtDocSuppOtherCharges)) {
    if (diffranceAmt > 1) {
        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    if (NetOrderValueInBase > 0) {

    }
    else {
        return false;
    }
    //let TxtTDS_Amount = parseFloat(CheckNullNumber($("#TxtTDS_Amount").val()));
    //let TxtOtherCharges = parseFloat(CheckNullNumber($("#TxtOtherCharges").val()));
    //let totGlCrTotalInBase = parseFloat(CheckNullNumber($("#CrTotalInBase").text()));
    //if (totGlCrTotalInBase != (GrossAmt + TxtTaxAmount + TxtOtherCharges + TxtTDS_Amount)) {
    //    swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
    //    return false;
    //}

    $("#SupplierName").attr("disabled", false);

    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
    var Narration = $("#DebitNoteRaisedAgainstInv").text()
    $('#hdNarration').val(Narration);

    var FinalItemDetail = [];
    FinalItemDetail = InsertDPIItemDetails();
    var str = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(str);

    var TaxDetail = [];
    TaxDetail = InsertTaxDetails();
    var str_TaxDetail = JSON.stringify(TaxDetail);
    $('#hdItemTaxDetail').val(str_TaxDetail);

    var OC_TaxDetail = [];
    OC_TaxDetail = OC_InsertTaxDetails();
    var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
    $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

    var OCDetail = [];
    OCDetail = GetPI_OtherChargeDetails();
    var str_OCDetail = JSON.stringify(OCDetail);
    $('#hdItemOCDetail').val(str_OCDetail);

    var vou_Detail = [];
    vou_Detail = GetDPI_VoucherDetails();
    var str_vou_Detail = JSON.stringify(vou_Detail);
    $('#hdItemvouDetail').val(str_vou_Detail);

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#hdnSubItemDetails').val(str2);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    var TdsDetail = [];
    TdsDetail = InsertTdsDetails();
    var str_TdsDetail = JSON.stringify(TdsDetail);
    $('#hdn_tds_details').val(str_TdsDetail);

    var Final_OC_TdsDetails = [];
    Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
    var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
    SaveBatchItemDeatil();
    SaveItemSerialItemDeatil();
    $('#hdn_oc_tds_details').val(Oc_Tds_Details);

    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramtInBase1", "cramtInBase1", "hfStatus") == false) {
        return false;
    }
    $("#RCMApplicable").prop("disabled", false);
    var Suppname = $('#SupplierName option:selected').text();
    $("#Hdn_PInvSuppName").val(Suppname);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    $("#ddlIBP_BillNo").attr("disabled", false);
    $("#BillDate").attr("disabled", false);
    $("#conv_rate").attr("disabled", false);
    return true;
}
function CheckDPI_VoucherValidations() {
    //debugger
    if (Cmn_CheckGlVoucherValidations() == false) {
        return false;
    } else {
        return true;
    }
}
function Check_subitemValidation() {
    return Cmn_CheckValidations_forSubItems("POInvItmDetailsTbl", "", "hfItemID", "Txtinv_qty", "SubItemDPIQty", "Y");
}
function InsertDPIItemDetails() {
    var PI_ItemsDetail = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger
        var TaxExempted = "";
        var ManualGST = "";
        var ClaimITC = "";

        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var srno = currentRow.find("#srno").text();
        var item_id = currentRow.find("#hfItemID").val();
        var item_name = currentRow.find("#TxtItemName" + Sno).val();
        var subitem = currentRow.find("#sub_item").val();
        var ItemType = "";
        var uom_id = IsNull(currentRow.find("#hfUOMID").val(), 0);
        var uom_name = IsNull(currentRow.find("#TxtUOM").val(), 0);
        var inv_qty = currentRow.find("#Txtinv_qty").val();
        var item_rate = currentRow.find("#item_rate").val();
        //var item_disc_perc = currentRow.find("#item_disc_perc").val();
        //var item_disc_amt = currentRow.find("#item_disc_amt").val();
        //var item_disc_val = currentRow.find("#item_disc_val").val();
        var item_gr_val = currentRow.find("#TxtItemGrossValueInBase").val();
        var item_gr_val_bs = currentRow.find("#TxtItemGrossValueInBase").val();
        var tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        var tax_amt_nrecov = currentRow.find("#Txtitem_tax_amt_NonRecovAmt").val();
        var tax_amt_recov = currentRow.find("#Txtitem_tax_amt_RecovAmt").val();
        var hsn_code = currentRow.find("#ItemHsnCode").val();
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        if (tax_amt == "" && tax_amt == null) {
            tax_amt = "0";
        }
        if (tax_amt_nrecov == "" && tax_amt_nrecov == null) {
            tax_amt_nrecov = "0";
        }
        if (tax_amt_recov == "" && tax_amt_recov == null) {
            tax_amt_recov = "0";
        }
        var item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        var item_net_val_spec = currentRow.find("#TxtNetValueInBase").val();
        var item_net_val_bs = currentRow.find("#TxtNetValueInBase").val();
        var gl_vou_no = null;
        var gl_vou_dt = null;
        //if (currentRow.find("#TaxExempted").is(":checked")) {
        //    TaxExempted = "Y"
        //}
        //else {
        //    TaxExempted = "N"
        //}
        var GstApplicable = $("#Hdn_GstApplicable").text();
        //if (GstApplicable == "Y") {
        //    if (currentRow.find("#ManualGST").is(":checked")) {
        //        ManualGST = "Y"
        //    }
        //    else {
        //        ManualGST = "N"
        //    }
        //    if (currentRow.find("#ClaimITC").is(":checked")) {
        //        ClaimITC = "Y"
        //    }
        //    else {
        //        ClaimITC = "N"
        //    }
        //}
        var wh_id = currentRow.find("#wh_id" + Sno).val();
        var LotNumber = currentRow.find("#TxtLotno").val();
        var remarks = currentRow.find("#TxtRemarks").val();
        var LandedCostValue = currentRow.find("#TxtLandedcost").val();
        var LandedCostPerPc = currentRow.find("#TxtLandedcostperpc").val();

        var PackSize = currentRow.find("#PackSize").val();
        var mrp = "0";

        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, subitem: subitem, ItemType: ItemType
            , uom_id: uom_id, uom_name: uom_name, inv_qty: inv_qty, item_rate: item_rate, item_gr_val: item_gr_val
            , item_gr_val_bs: item_gr_val_bs, tax_amt: tax_amt, tax_amt_nrecov: tax_amt_nrecov, tax_amt_recov: tax_amt_recov, item_oc_amt: item_oc_amt
            , item_net_val_spec: item_net_val_spec, item_net_val_bs: item_net_val_bs, gl_vou_no: gl_vou_no
            , gl_vou_dt: gl_vou_dt, hsn_code: hsn_code, item_acc_id: item_acc_id, wh_id: wh_id, LotNumber: LotNumber, remarks: remarks,
            LandedCostValue: LandedCostValue, LandedCostPerPc: LandedCostPerPc, srno: srno, mrp: mrp, PackSize: PackSize
        });
    });
    return PI_ItemsDetail;
}
function InsertTaxDetails() {
    //debugger
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {
        }
        else {
            var ItmCode = currentRow.find("#hfItemID").val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var currentRow = $(this);
                        var item_id = currentRow.find("#TaxItmCode").text();
                        var tax_id = currentRow.find("#TaxNameID").text();
                        var TaxName = currentRow.find("#TaxName").text().trim();
                        var tax_rate = currentRow.find("#TaxPercentage").text();
                        var tax_level = currentRow.find("#TaxLevel").text();
                        var tax_val = currentRow.find("#TaxAmount").text();
                        var tax_apply_on = currentRow.find("#TaxApplyOnID").text();
                        var tax_apply_onName = currentRow.find("#TaxApplyOn").text();
                        var totaltax_amt = currentRow.find("#TotalTaxAmount").text();
                        var tax_recov = currentRow.find("#TaxRecov").text();
                        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt, totaltax_amt, tax_recov: tax_recov });
                    });
                }
            }
        }
    });
    return TaxDetails;
}
function OC_InsertTaxDetails() {
    //debugger
    var TaxDetails = [];
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var item_id = currentRow.find("#TaxItmCode").text();
        var tax_id = currentRow.find("#TaxNameID").text();
        var TaxName = currentRow.find("#TaxName").text().trim();
        var tax_rate = currentRow.find("#TaxPercentage").text();
        var tax_level = currentRow.find("#TaxLevel").text();
        var tax_val = currentRow.find("#TaxAmount").text();
        var tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        var tax_apply_onName = currentRow.find("#TaxApplyOn").text();
        var totaltax_amt = currentRow.find("#TotalTaxAmount").text();
        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    });
    return TaxDetails;
}
function GetPI_OtherChargeDetails() {
    //debugger
    var PI_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = currentRow.find("#OCValue").text();
            var oc_val = currentRow.find("#OCAmount").text();
            var OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            var OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            var OCName = currentRow.find("#OCName").text();
            var OC_Curr = currentRow.find("#OCCurr").text();
            var OC_Conv = currentRow.find("#OCConv").text();
            var OC_AmtBs = currentRow.find("#OcAmtBs").text();
            var curr_id = currentRow.find("#HdnOCCurrId").text();
            var supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text();
            let bill_no = currentRow.find("#OCBillNo").text();
            let bill_date = currentRow.find("#OCBillDt").text();
            let tds_amt = currentRow.find("#OC_TDSAmt").text();
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            PI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName
                , OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, supp_id: supp_id, curr_id: curr_id
                , supp_type: supp_type, bill_no: bill_no, bill_date: bill_date, tds_amt: tds_amt
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    return PI_OCList;
};
function InsertTdsDetails() {
    //debugger
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var Tds_id = currentRow.find("#td_TDS_NameID").text();
        var Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        var Tds_level = currentRow.find("#td_TDS_Level").text();
        var Tds_val = currentRow.find("#td_TDS_Amount").text();
        var Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        var Tds_name = currentRow.find("#td_TDS_Name").text();
        var Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        var Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        var Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();
        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
}
function SaveBatchItemDeatil() {
    var Batcharr = new Array;
    $('#SaveItemBatchTbl tbody tr').each(function () {
        var Batch_arr = {};
        var CurrentRow = $(this);
        Batch_arr.itemid = CurrentRow.find("#ItemID").val();
        Batch_arr.BatchNo = CurrentRow.find("#BatchNo").val();
        Batch_arr.BatchExDate = CurrentRow.find("#BatchExDate").val();
        Batch_arr.BatchQty = CurrentRow.find("#BatchQty").val();
        Batch_arr.MfgName = CurrentRow.find("#bt_mfg_name").text();
        Batch_arr.MfgMrp = CurrentRow.find("#bt_mfg_mrp").val();
        Batch_arr.MfgDate = CurrentRow.find("#bt_mfg_date").val();
        Batcharr.push(Batch_arr);
    })
    $("#hdn_BatchItemDeatilData").val(JSON.stringify(Batcharr));
}
function SaveItemSerialItemDeatil() {
    var Serialarr = new Array;
    $('#SaveItemSerialTbl tbody tr').each(function () {
        //debugger;
        var Serial_arr = {};
        var CurrentRow = $(this);
        Serial_arr.itemid = CurrentRow.find("#Scrap_lineSerialItemId").val();
        Serial_arr.SerialNo = CurrentRow.find("#Scrap_lineBatchSerialNO").val();
        Serial_arr.MfgName = CurrentRow.find("#sr_mfg_name").text();
        Serial_arr.MfgMrp = CurrentRow.find("#sr_mfg_mrp").val();
        Serial_arr.MfgDate = CurrentRow.find("#sr_mfg_date").val();
        Serialarr.push(Serial_arr);
    })
    $("#hdn_SerialItemDeatilData").val(JSON.stringify(Serialarr));
}
function GetDPI_VoucherDetails() {
    //debugger
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            var acc_id = currentRow.find("#hfAccID").val();
            var acc_name = currentRow.find("#txthfAccID").val();
            var dr_amt = currentRow.find("#dramt").text();
            var cr_amt = currentRow.find("#cramt").text();
            var dr_amt_bs = currentRow.find("#dramtInBase").text();
            var cr_amt_bs = currentRow.find("#cramtInBase").text();
            var Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            PI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
        });
    }
    return PI_VouList;
};
function CheckDPI_Validations() {
    var ErrorFlag = "N";
    if ($("#ddlIBP_BranchName").val() === "0") {
        $('#SpanddlIBP_BranchNameErrorMsg').text($("#valueReq").text());
        $("#ddlIBP_BranchName").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlIBP_BranchName-container']").css("border-color", "red");
        $("#SpanddlIBP_BranchNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlIBP_BranchNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlIBP_BranchName-container']").css("border-color", "#ced4da");
        $("#ddlIBP_BranchName").css("border-color", "#ced4da");
    }

    if ($("#ddlSupplierName").val() === "0") {
        $('#SpanSupplierNameErrorMsg').text($("#valueReq").text());
        $("#ddlSupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlSupplierName-container']").css("border-color", "red");
        $("#SpanSupplierNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSupplierNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlSupplierName-container']").css("border-color", "#ced4da");
        $("#ddlSupplierName").css("border-color", "#ced4da");
    }

    if (($("#ddlIBP_BillNo").val() === "0") || ($("#ddlIBP_BillNo").val() === "")) {
        $('#SpanddlIBP_BillNoErrorMsg').text($("#valueReq").text());
        $("#ddlIBP_BillNo").css("border-color", "Red");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "red");
        $("#SpanddlIBP_BillNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanddlIBP_BillNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "#ced4da");
        $("#ddlIBP_BillNo").css("border-color", "#ced4da");
    }

    //if ($("#ddlIBP_BillNo").val() === "0") {
    //    $('#Spanbill_noErrorMsg').text($("#valueReq").text());
    //    $("#ddlIBP_BillNo").css("border-color", "Red");
    //    $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "red");
    //    $("#Spanbill_noErrorMsg").css("display", "block");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    $("#Spanbill_noErrorMsg").css("display", "none");
    //    $("[aria-labelledby='select2-ddlIBP_BillNo-container']").css("border-color", "#ced4da");
    //    $("#ddlIBP_BillNo").css("border-color", "#ced4da");
    //}
    if ($("#BillDate").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#BillDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("#BillDate").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    } else {
        //debugger
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}
function CheckDPI_ItemValidations(VoucherValidate) {
    //debugger
    var ErrorFlag = "N";
    var count = 0;
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        let isFocused = false;
        var len = $("#POInvItmDetailsTbl > tbody > tr").length;
        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
                currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
                //currentRow.find("#wh_Error" + Sno).css("display", "block");
                //currentRow.find("#wh_id" + Sno).css("border-color", "red").focus();
                currentRow.find("#wh_id" + Sno).css("border-color", "Red");
                currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border-color", "red").focus();
                currentRow.find("#wh_Error" + Sno).css("display", "block");
                //currentRow.find("#wh_Error" + Sno);
                ErrorFlag = "Y";
            }
            else {
                var WHName = $("#wh_id option:selected").text();
                $("#hdwh_name").val(WHName)
                currentRow.find("#wh_Error" + Sno).css("display", "none");
                currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border", "1px solid #aaa");
                //currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border-color", "#ced4da");
                //currentRow.find("#wh_id" + Sno).css("border", "1px solid #aaa");
                //currentRow.find("#wh_id" + Sno).css("border-color", "#ced4da");
            }
        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemBatchValidation() {
    //debugger
    var ErrorFlag = "N";
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger
        var currentRow = $(this);
        var Batchable = currentRow.find("#hfbatchable").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ord_qty_spec").val();
        if (Batchable == "Y") {
            var Count = $("#SaveItemBatchTbl tbody tr").length;
            var totalbatchQty = "0";
            if (Count != null) {
                if (Count > 0) {
                    $("#SaveItemBatchTbl tbody tr").each(function () {
                        var Curr = $(this);
                        var batchitemit = Curr.find("#ItemID").val();
                        if (batchitemit == ItemID) {
                            var BatchQty = Curr.find("#BatchQty").val();
                            totalbatchQty = parseFloat(totalbatchQty) + parseFloat(BatchQty)
                        }
                    })
                    if (totalbatchQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "BtnBatchDetail", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                    ErrorFlag = "Y";
                }
            }
            else {

                ValidateEyeColor(currentRow, "BtnBatchDetail", "Y");
                ErrorFlag = "Y";
            }
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemSerialValidation() {
    //debugger
    var ErrorFlag = "N";
    $("#POInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger
        var currentRow = $(this);
        var Serialable = currentRow.find("#hfserialable").val();
        var ItemID = currentRow.find("#hfItemID").val();
        var ReceivedQuantity = currentRow.find("#ord_qty_spec").val();
        if (Serialable == "Y") {
            var TotalSQty = parseFloat($("#SaveItemSerialTbl tbody tr #SerialItemID[value='" + ItemID + "']").closest('tr').length).toFixed(QtyDecDigit);
            if (TotalSQty != null) {
                if (parseFloat(TotalSQty) > parseFloat(0)) {
                    if (TotalSQty == ReceivedQuantity) {
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "N");
                    }
                    else {
                        ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
                        ErrorFlag = "Y";
                    }
                }
                else {
                    ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
                    ErrorFlag = "Y";
                }
            }
            else {
                ErrorFlag = "Y";
                ValidateEyeColor(currentRow, "BtnSerialDetail", "Y");
            }
        }
    });
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
/***----------------------end-------------------------------------***/

/*----------------------------TDS Section-----------------------------*/
function OnClickTDSCalculationBtn() {
    //debugger
    const GrVal = $("#TxtGrossValue").val();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal)).toFixed(ValDecDigit);
    const NetVal = $("#NetOrderValueInBase").val();
    const ToTdsAmt_IT = parseFloat(CheckNullNumber(NetVal)).toFixed(ValDecDigit);
    $("#hdn_tds_on").val("D");
    $("#TDS_AssessableValue").val(ToTdsAmt);
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, null, null, ToTdsAmt_IT, "TDS");/* "TDS" passed by Suraj Maurya on 14-05-2025 */

}
function OnClickTDS_SaveAndExit() {
    //debugger
    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {
        let TdsAssVal_applyOn = "ET";
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
        }
        else if ($("#TdsAssVal_Custom").is(":checked")) {/* Added by Suraj Maurya on 14-05-2025 */
            TdsAssVal_applyOn = "CA"
        }
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        var rowIdx = 0;
        if ($("#Hdn_TDS_CalculatorTbl >tbody >tr").length > 0) {
            $("#Hdn_TDS_CalculatorTbl >tbody >tr").remove();
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
            });
        }
        else {
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $("#Hdn_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
            });
        }
        var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
        });
        $("#TxtTDS_Amount").val(TotalAMount);
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        GetAllGLID();
    }
}

/*----------------------------TDS Section End-----------------------------*/
/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e) {
    const ValDecDigit = $("#ValDigit").text();
    const row = $(e.target).closest("tr");
    //debugger
    CC_Clicked_Row = row;
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const TotalVal = CC_Clicked_Row.find("#OCTotalTaxAmt").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    $("#TDS_AssessableValue").val(parseFloat(CheckNullNumber(ToTdsAmt)).toFixed(ValDecDigit));

    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, TotalVal, "OcTds");/* "OcTds" passed by Suraj Maurya on 15-05-2025 */

}
function OnClickTP_TDS_SaveAndExit() {
    //debugger
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
    }
    if ($("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").length > 0) {
        $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function () {
            let row = $(this);
            if (row.find("#td_TDS_OC_Id").text() == TDS_OcId) {
                $(this).remove();
            }
        });
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $('#Hdn_OC_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });
    }
    else {
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $("#Hdn_OC_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });

    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
}
function SetTds_Amt_To_OC() {
    var TotalAMount = parseFloat(0);
    const ValDecDigit = $("#ValDigit").text();
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
    });
    CC_Clicked_Row.find("#OC_TDSAmt").text(parseFloat(CheckNullNumber(TotalAMount)).toFixed(ValDecDigit));
    CC_Clicked_Row = null;
}
/***------------------------------TDS On Third Party End------------------------------***/
function OnClickSaveAndExit(MGST) {
    //debugger
    const ValDecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);

    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var tbllenght = $("#TaxCalculatorTbl tbody tr").length
    if (TaxOn == "Item") {
        if (tbllenght == 0) {
            $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
            $("#SpanTax_Template").text($("#valueReq").text());
            $("#SpanTax_Template").css("display", "block");
            $("#SaveAndExitBtn").attr("data-dismiss", "");
            return false;
        }
        else {
            $("#SaveAndExitBtn").attr("data-dismiss", "modal");
        }
    }
    //debugger
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    let NewArr = [];
    var FTaxDetails = $("#" + HdnTaxCalculateTable + " >tbody >tr").length;
    if (FTaxDetails > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();
            var TaxAccId = currentRow.find("#TaxAccId").text();
            if (TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov, TaxAccId: TaxAccId });
            }
        });
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $('#' + HdnTaxCalculateTable + ' tbody').append(`
                                <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    <td id="TaxRecov">${tax_recov}</td>
                                    <td id="TaxAccId">${TaxAccID}</td>
                                </tr>`)
            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: tax_recov })
        });
        //BindTaxAmountDeatils(NewArr);
    }
    else {
        //var TaxCalculationList = [];
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var tax_recov = currentRow.find("#tax_recov").text();
            if (tax_recov == "Y") {
                tax_recov_amt = parseFloat(tax_recov_amt) + parseFloat(TaxAmount);
            } else {
                tax_non_recov_amt = parseFloat(tax_non_recov_amt) + parseFloat(TaxAmount);
            }
            $("#" + HdnTaxCalculateTable + " tbody").append(`
 <tr>
                                      <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                                    <td id="TaxRecov">${tax_recov}</td>
                                    <td id="TaxAccId">${TaxAccID}</td>
                                </tr>`)
            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: tax_recov })
        });
        //BindTaxAmountDeatils(TaxCalculationList);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit);
                Cmn_click_Oc_chkroundoff(null, currentRow);
                //currentRow.find("#OCTotalTaxAmt").text(Total);
                var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
            }
        });
        //$("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {/* Commented by Suraj Maurya on 04-04-2025 */
        //    //debugger
        //    var currentRow = $(this);
        //    if (currentRow.find("#OCValue").text() == TaxItmCode) {
        //        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
        //        currentRow.find("#OCTaxAmt").text(TaxAmt);               
        //        Cmn_click_Oc_chkroundoff(null, currentRow);
        //    }
        //});
        Calculate_OCAmount();
    }
    else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode) {
                let InvQty = currentRow.find("#ord_qty_spec").val();
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    tax_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                    tax_non_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                }
                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);

                currentRow.find("#item_tax_amt").val(TaxAmt);
                //var itmocamt = currentRow.find("#item_oc_amt").val();//05-02-2025
                var itmocamt = currentRow.find("#TxtOtherCharge").val();
                if (itmocamt != null && itmocamt != "") {
                    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#item_net_val_spec").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
                //debugger
                //debugger
                let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(InvQty));
                currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
    if (MGST == "MGST" && TaxOn != "OC") {
        let SuppId = $("#SupplierName").val();
        let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        if (GrossAmt != 0) {
            AutoTdsApply(SuppId, GrossAmt).then(() => {
                GetAllGLID();
            });
        }
        else {
            GetAllGLID();
        }
    }
}

function OnClickOCTaxCalculationBtn(e) {
    //debugger
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}

//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
    //debugger
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
    if (GLAcc_Name == null || GLAcc_Name == "") {
        GLAcc_Name = clickedrow.find("#txthfAccID").val();
        GLAcc_id = clickedrow.find("#hfAccID").val();
    }
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            List.vou_sr_no = row.find('#hdntbl_vou_sr_no').text();
            List.gl_sr_no = row.find('#hdntbl_gl_sr_no').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDecDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    var DocMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            DocumentMenuId: DocMenuId
        },
        success: function (data) {
            //debugger
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}
//-------------------Cost Center Section End-------------------//

function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {

    var Dmenu = $("#DocumentMenuId").val();

    ValDecDigit = $("#ValDigit").text();

    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }

    if (parseFloat(DrValue) < 0) {/*chnges for tds on OC by hina sharma on 10-07-2024*/
        CrValue = Math.abs(DrValue);
        DrValue = 0;
    }
    if (parseFloat(DrValueInBase) < 0) {
        CrValueInBase = Math.abs(DrValueInBase);
        DrValueInBase = 0;
    }
    if (parseFloat(CrValue) < 0) {
        DrValue = Math.abs(CrValue);
        CrValue = 0;
    }
    if (parseFloat(CrValueInBase) < 0) {
        DrValueInBase = Math.abs(CrValueInBase);
        CrValueInBase = 0;
    }


    let hfSrNo = $('#VoucherDetail tbody tr').length + 1;
    var FieldType = "";
    if (type == 'Itm') {
        FieldType = `<div class="col-sm-11 lpo_form no-padding">
                            <select class="form-control" id="Acc_name_${hfSrNo}" onchange ="OnChangeAccountName(${hfSrNo},event)">
                              </select>
                        </div>`;
        $("#hdnAccID").val(acc_id);
    }
    else {
        FieldType = `${acc_name}`;
    }

    let Table_tds = `<td id="td_GlSrNo">${GlSrNo}</td>
                                    <td id="td_vou_sr_no" hidden>${SrNo}</td>
                                    <td id="td_GlAccName" class="no-padding">
                                        `+ FieldType + `
                                    </td>
                                    <td id="tdhdn_GlAccId" hidden>${acc_id}</td>
                                    <td class="num_right" id="dramt">${parseFloat(DrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="dramtInBase">${parseFloat(DrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramt">${parseFloat(CrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramtInBase">${parseFloat(CrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="center" id="td_CC">
                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
                                    </td>
                                <td hidden>
                                    <input type="hidden" id="SNohf" value="${hfSrNo}" />
                                    <input type="hidden" id="type" value="${type}"/>
                                    <input  type="hidden" id="txthfAccID" value="${acc_name}"/>
                                    <input  type="hidden" id="hfAccID"  value="${acc_id}" />
                                    <input  type="hidden" id="glVouType"  value="${VouType}" />
                                    <input  type="hidden" id="gl_curr_id"  value="${curr_id}" />
                                    <input  type="hidden" id="gl_conv_rate"  value="${conv_rate}" />
                                    <input  type="hidden" id="gl_bill_no"  value="${bill_bo}" />
                                    <input  type="hidden" id="gl_bill_date"  value="${bill_date}" />
                                    <input  type="hidden" id="dramt1" value="${parseFloat(DrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="dramtInBase1" value="${parseFloat(DrValueInBase).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramt1" value="${parseFloat(CrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramtInBase1" value="${parseFloat(CrValueInBase).toFixed(ValDecDigit)}"/>
                                </td>`
    if (type == "Supp" || type == "Bank" || type == "Cust" || type == "TSupp") {/*chnges as add TSupp for tds on OC  by hina sharma on 10-07-2024*/
        $('#VoucherDetail tbody').append(`<tr id="tr_GlRow${SrNo}">
                                <td rowspan="${rowSpan}" id="td_SrNo">${SrNo}</td>
                                `+ Table_tds + `
                                <td rowspan="${rowSpan}" id="td_VouType">${VouType}</td>
                                <td rowspan="${rowSpan}" id="td_VouNo"></td>
                                <td rowspan="${rowSpan}" id="td_VouDate"></td>
                                
                            </tr>`)
    } else {
        $('#VoucherDetail tbody').append(`<tr>
                                     `+ Table_tds + `
                                </tr>`)
    }
}
//---------------------------------------Stock Details Start---------------------//
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
        var currentrow = $(evt.target).closest('tr');
        var SNohf = currentrow.find("#SNohiddenfiled").val();
        var ItemId = currentrow.find("#hfItemID").val();
        var TxtInvoiceQuantity = currentrow.find("#Txtinv_qty").val();
        var UOMName = currentrow.find("#TxtUOM").val();
        var UOMID = currentrow.find("#hfUOMID").val();
        var ReceivedQuantity = currentrow.find("#Txtinv_qty").val();
        var ItemName = currentrow.find("#TxtItemName" + SNohf).val();
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#_ModelCommand").val();
        var TransType = $("#hdn_TransType").val();
        var Scrap_Status = $("#hfStatus").val();
        var RateDigit = $("#RateDigit").val();
        // if (Scrap_Status == "" || Scrap_Status == null || Scrap_Status == "D") {
        $("#ItemNameSerialWise").val(ItemName);
        $("#UOMSerialWise").val(UOMName);
        $("#QuantitySerialWise").val(TxtInvoiceQuantity);
        $("#TotalIssuedSerial").text(TxtInvoiceQuantity);
        $("#HDItemIDSerialWise").val(ItemId);
        $("#HDUOMIDSerialWise").val(UOMID);
        var rowIdx = 0;
        var Count = $("#SaveItemSerialTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                $("#ItemSerialwiseTbl >tbody >tr").remove();
                $("#SaveItemSerialTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    var SItemID = SaveBatchRow.find("#Scrap_lineSerialItemId").val();
                    // var Lot = SaveBatchRow.find("#Scrap_lineSerialLOTNo").val();
                    var Lot = SaveBatchRow.find("#Lot").val();

                    var MfgName = SaveBatchRow.find("#sr_mfg_name").text();
                    var MfgMrp = parseFloat(CheckNullNumber(SaveBatchRow.find("#sr_mfg_mrp").val())).toFixed(RateDigit);
                    var MfgDate = SaveBatchRow.find("#sr_mfg_date").val();
                    var mfg_date = "";
                    if (MfgDate != null && MfgDate != "") {

                        if (MfgDate == "1900-01-01") {
                            mfg_date = "";
                        } else {
                            mfg_date = MfgDate;
                        }

                    }
                    if (Lot == undefined) {
                        Lot = "";
                    }
                    var SerialNo = SaveBatchRow.find("#Scrap_lineBatchSerialNO").val();
                    if (SNohf != null && SNohf != "") {
                        if (SItemID == ItemId) {
                            $('#ItemSerialwiseTbl tbody').append(`<tr>
                                    <td><input type="text" id="Lot" value="${Lot}" class="form-control" disabled/></td>
                                    <td><input type="text" id="SerialNumberDetail" value="${SerialNo}" class="form-control" disabled /></td>
                                    <td><input type="text" id="SrMfgName" value="${MfgName}" class="form-control" disabled/></td>
                                    <td><input type="text" id="SrMfgMrp" value="${MfgMrp}" class="form-control num_right" disabled/></td>
                                    <td><input type="date" id="SrMfgDate" value="${mfg_date}" class="form-control" disabled/></td>
                                    </tr>`
                            );
                        }
                    }
                });
            }
        }
        // }
    } catch (err) {
        console.log("Stock Detail Error : " + err.message);
    }
}
//---------------------------------------Stock Details End---------------------//

function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    //debugger;
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "TxtItemName", "FieldType": "input" }]);
}

function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertIBPDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

function ApplyRoundOff(flag) {/*Add by Hina sharma on 13-01-2025 take reference of service prchase invoice*/
    var ValDecDigit = $("#ValDigit").text();
    var grossval = $("#TxtGrossValue").val();
    var taxval = $("#TxtTaxAmount").val();
    //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());
    var tcs_amt = CheckNullNumber($("#TxtTDS_Amount").val());//TCS Amount

    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval) + parseFloat(tcs_amt)).toFixed(ValDecDigit);

    var netval = finalnetval;//$("#NetOrderValueInBase").val();
    var fnetval = 0;

    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if ($("#p_round").is(":checked")) {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
                $("#pm_flagval").val($("#p_round").val());

                //let valInBase = $("#TxtGrossValue").val();
                //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
            }
            if ($("#m_round").is(":checked")) {
                //var finnetval = netval.split('.');
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
                $("#pm_flagval").val($("#m_round").val());

                //let valInBase = $("#TxtGrossValue").val();
                //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
            }
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                $("#NetOrderValueInBase").val(f_netval);
                //$("#NetOrderValueSpe").val(f_netval);
                if (flag == "CallByGetAllGL") {
                    //do not call  GetAllGLID("RO");
                } else {
                    GetAllGLID("RO");
                }

            }

        }
    }

}

/***-------------------For Workflow Start----------------***/
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
function ForwardBtnClick() {
    debugger;
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DrPinvDate = $("#Sinv_dt").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: DrPinvDate
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var INVStatus = "";
                    INVStatus = $('#hfStatus').val().trim();
                    if (INVStatus === "D" || INVStatus === "F") {

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
                    /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");
                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var INV_NO = "";
    var INVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var VoucherNarr = $("#hdn_Nurration").val();
    var BP_VoucherNarr = $("#hdn_BP_Narration").val();
    var DN_VoucherNarr = $("#hdn_DN_Narration").val();

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#InvoiceNumber").val();
    INVDate = $("#Sinv_dt").val();
    WF_Status1 = $("#WF_Status1").val();
    var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    var pdfAlertEmailFilePath = 'IBP_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/InterBranchPurchase/ApproveIBPDetails?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&VoucherNarr=" + VoucherNarr + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + WF_Status1 + "&Bp_Nurr=" + BP_VoucherNarr + "&Dn_Nurration=" + DN_VoucherNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        var Doc_Status = $("#hfStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "POItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
function GetPdfFilePathToSendonEmailAlert(spoNo, spoDate, fileName) {
    debugger;
    var Inv_no = spoNo;
    var InvDate = spoDate;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InterBranchPurchase/SavePdfDocToSendOnEmailAlert",
        data: { poNo: Inv_no, poDate: InvDate, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
/***-------------------For Workflow End----------------***/

function functionConfirm(event) {
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
function OnChnageWarehouse(e) {
    try {
        debugger;
        var currentRow = $(e.target).closest("tr");
        var whname = currentRow.find("#wh_id option:selected").text();
        var Sno = currentRow.find("#SNohiddenfiled").val();
        $("#Hdn_GRN_WhName" + Sno).val(whname);
        if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
            currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
            currentRow.find("#wh_Error" + Sno).css("display", "block");
            currentRow.find("#wh_id" + Sno).css("border", "1px solid red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error" + Sno).css("display", "none");
            currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border", "1px solid #aaa");
        }
    }
    catch (ex) {
        ToAddJsErrorLogs(ex);
    }
}