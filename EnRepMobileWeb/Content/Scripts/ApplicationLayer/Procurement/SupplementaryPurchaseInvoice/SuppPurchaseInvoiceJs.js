//const ExchDecDigit = $("#ExchDigit").text();///Amount
var LddCostDecDigit = 5;///For Landed Cost 
//const ord_type_imp = $("#OrderTypeE").is(":checked");///Amount
//const QtyDecDigit = $("#QtyDigit").text();///Quantity
//const RateDecDigit = $("#RateDigit").text();///Rate And Percentage
const ValDecDigit = $("#ValDigit").text();///Amount

$(document).ready(function () {
    $("#SupplierName").select2();
    $("#Purchaseinv_no").select2();
    $("#AddQt").show();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#InvoiceNumber").val() == "" || $("#InvoiceNumber").val() == null) {
        $("#SuppPIInv_date").val(CurrentDate);
    }
    if ($("#hfPIStatus").val() == "D" || $("#hfPIStatus").val() == "") {
        //BindPOItmList(1);
    }
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        var WF_Status = $("#WF_Status").val();
        if (InvoiceNo != "" && InvoiceNo != null) {
            location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/DblClick/?DocNo=" + InvoiceNo + "&DocDate=" + InvDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        // ////debugger;
        var clickedrow = $(e.target).closest("tr");
        var InvoiceNo = clickedrow.children("#InvoiceNo").text();
        var InvDate = clickedrow.children("#InvDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(InvoiceNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(InvoiceNo, InvDate, Doc_id, Doc_Status);
    });
    if ($("#RCMApplicable").is(":checked")) {
        $("#Hd_GstCat").val("UR");
    }
    else {
        $("#Hd_GstCat").val("RR");
    }
    $('#POInvItmDetailsTbl tbody').on('click', '.delBtnIconDPI', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#hfItemID").val();
        CalculateAmount(true);
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        DeleteItemBatchSerialOrderQtyDetails(ItemCode);
        var TotalOCAmt = CheckNullNumber($("#TxtDocSuppOtherCharges").val()) || 0.00;// parseFloat($("#TxtOtherCharges").val());
        if (TotalOCAmt == "" || TotalOCAmt == null || TotalOCAmt == "NaN") {
            TotalOCAmt = 0;
        }
        var TotalOCAmtTP = CheckNullNumber($("#TxtDocSuppOtherChargesTP").val()) || 0.00;// parseFloat($("#TxtOtherCharges").val());
        if (TotalOCAmtTP == "" || TotalOCAmtTP == null || TotalOCAmtTP == "NaN") {
            TotalOCAmtTP = 0;
        }
        //Calculate_OC_AmountItemWise(TotalOCAmt, "amt");
        //Calculate_OC_AmountItemWiseTP(TotalOCAmtTP, "amt");
        AfterDeleteResetPO_ItemTaxDetail();
        //BindOtherChargeDeatils();
        SerialNoAfterDelete();
        var ToTdsAmt_IT = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val()));
        var ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
        //ResetTDS_CalOnchangeDocDetail(ToTdsAmt, "#TxtTDS_Amount", ToTdsAmt_IT);
        //let SuppId = $("#SupplierName").val();
        //let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        //if (GrossAmt != 0) {
        //    AutoTdsApply(SuppId, GrossAmt).then(() => {
        //        CalculateVoucherTotalAmount();
        //    });
        //}
        //else {
        //    //   GetAllGLID();
        //}

    });
    var InvoiceNo = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvoiceNo);
    //var InvoiceNumber = $("#InvoiceNumber").val();
    var InvoiceDate = $("#SuppPIInv_date").val();
    var hdStatus = $('#hfStatus').val();
    GetWorkFlowDetails(InvoiceNo, InvoiceDate, "105101147", hdStatus);
    BindDDLAccountList();
    hideLoader();
});
function OnMouseOver(el) {
    $(el).prop("title", el.value);
    // $('.remarksmessage').prop('title', remarkstext);
}
function QtyRateValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, cmn_GrnRateDigit) == false) {
        return false;
    }
    /*   $("VehicleLoadInTonnage").val(parseFloat().toFixed(RateDecDigit))*/
    return true;
}
function OnChangeSupplier(SuppID, InvoiceType) {
    $("#AddQt").show();
    $("#ddlPurchaseinv_no").prop("disabled", false);
    // document.getElementById("Inv_date").value = "";
    const invDate = document.getElementById("Inv_date");
    if (invDate) {
        invDate.value = ""; // clears the value
    }
    document.getElementById("GoodReceiptNoteNo").value = "";
    document.getElementById("GRN_Date").value = "";
    document.getElementById("PIBill_No").value = "";
    document.getElementById("PIBill_Date").value = "";
    var table = document.getElementById("POInvItmDetailsTbl");
    var octable = document.getElementById("Tbl_OC_Deatils");
    var Taxtable = document.getElementById("Tbl_ItemTaxAmountList");
    var OCListtable = document.getElementById("Tbl_OtherChargeList");

    if (table.rows.length > 1) {
        table.deleteRow(1);
    }
    if (octable.rows.length > 1) {
        octable.deleteRow(1);
    }
    if (Taxtable.rows.length > 1) {
        $("#Tbl_ItemTaxAmountList tbody").empty();
    }
    if (OCListtable.rows.length > 1) {
        $("#Tbl_OtherChargeList tbody").empty();
    }

    document.getElementById("TxtGrossValue").value = "";
    document.getElementById("TxtGrossValueInBasePrev").value = "";
    document.getElementById("TxtGrossValueInBase").value = "";
    document.getElementById("TaxAmountRecoverable").value = "";
    document.getElementById("TaxAmountNonRecoverable").value = "";
    document.getElementById("TxtOtherCharges").value = "";
    document.getElementById("TxtDocSuppOtherCharges").value = "";
    document.getElementById("NetOrderValueSpe").value = "";
    document.getElementById("NetOrderValueInBase").value = "";
    document.getElementById("TxtTDS_Amount").value = "";
    document.getElementById("TxtGrossValuePrev").value = "";
    document.getElementById("TaxAmountRecoverablePrev").value = "";
    document.getElementById("TaxAmountNonRecoverablePrev").value = "";
    document.getElementById("TxtOtherChargesPrev").value = "";
    document.getElementById("TxtDocSuppOtherChargesPrev").value = "";
    document.getElementById("NetOrderValueSpePrev").value = ""; // Or clear it if needed
    document.getElementById("NetOrderValueInBasePrev").value = "";
    document.getElementById("TxtTDS_AmountPrev").value = "";
    document.getElementById("TxtDiff_TotalAmountInSpec").value = "";
    document.getElementById("TxtDiff_TotalAmountInBase").value = "";
    document.getElementById("TxtDiff_TotalTaxAmountRec").value = "";
    document.getElementById("TxtDiff_TotalOCAmount").value = "";
    document.getElementById("TxtDiff_Amount").value = "";
    document.getElementById("TxtDiff_TotalTaxAmountNonRec").value = "";
    document.getElementById("TxtDiff_TotalTDSAmount").value = "";

    var Supp_id = SuppID.value;
    var Invoice_Type = InvoiceType.value;
    if (Supp_id == "" || Supp_id == null || Supp_id == undefined) {
        Supp_id = SuppID;
    }
    if (Invoice_Type == "" || Invoice_Type == null || Invoice_Type == undefined) {
        Invoice_Type = InvoiceType;
    }
    if (Supp_id == "") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#AddItemsInDPI").css("display", "none")
    }
    else {
        var Suppname = $('#SupplierName option:selected').text();
        var SuppId = $('#SupplierName option:selected').val();
        $("#Hdn_PInvSuppName").val(Suppname);
        $("#Hdn_PInvSuppId").val(SuppId);
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#AddItemsInDPI").css("display", "")
    }
    GetSuppAddress(Supp_id);
    GetPurchaseInvoices(Supp_id, Invoice_Type);/**/
    $("#HdnSuppType").val(Invoice_Type);
}

function GetSuppAddress(Supp_id) {
    //let ExchDecDigit = $("#ExchDigit").text();
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
                        PO_ErrorPage();
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
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            //$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                            $("#conv_rate_tb").val(parseFloat(arr.Table[0].conv_rate).toFixed(cmn_ExchDecDigit));
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
                            $("#conv_rate").val(parseFloat(0).toFixed(cmn_ExchDecDigit));
                            $("#conv_rate").prop("readonly", false);
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetSupplierData Error : " + err.message);
    }
}
function GetPurchaseInvoices(Supp_id, InvoiceType) {
    if (InvoiceType == '' || InvoiceType == undefined) {
        InvoiceType = "D";
    }
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SupplementaryPurchaseInvoice/GetPurchaseInvoices",
            data: {
                Supp_id: Supp_id,
                InvoiceType: InvoiceType
            },
            success: function (data) {
                if (data === 'ErrorPage') {
                    PO_ErrorPage();
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
                    if (data && data.Table && data.Table.length > 0) {
                        $("#ddlPurchaseinv_no").empty();
                        var uniqueOptGroupId = "Textddl_" + new Date().getTime();
                        $('#ddlPurchaseinv_no').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#span_InvoiceNo").text()}' data-invdate='${$("#span_InvoiceDate").text()}' data-invbillno='${$("#span_BillNumber").text()}' data-invbilldt='${$("#span_BillDate").text()}'></optgroup>`);
                        // Append options to the dropdown
                        for (var i = 0; i < data.Table.length; i++) {
                            $('#' + uniqueOptGroupId).append(`<option data-invdate="${data.Table[i].Invdt}" data-invbillno="${data.Table[i].BillNo}" data-invbilldt="${data.Table[i].Billdt}" value="${data.Table[i].Invdt}">${data.Table[i].inv_no}</option>`);
                        }
                        $('#ddlPurchaseinv_no').select2({
                            templateResult: function (data) {
                                var PInvDate = $(data.element).data('invdate');
                                var PInvBillNo = $(data.element).data('invbillno');
                                var PInvBillDate = $(data.element).data('invbilldt');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-4 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvDate + '</div>' +
                                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + PInvBillNo + '</div>' +
                                    '<div class="col-md-2 col-xs-6' + classAttr + '">' + PInvBillDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                            }
                        });
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: ", error); // Log AJAX errors
            }
        });
    } catch (err) {
        console.log("GetPurchaseInvoices Error: " + err.message); // Handle any JavaScript errors
    }
}
function ddlPurchaseinv_noSelect(InvNo, InvoiceType) {
    var Invoice_Type = InvoiceType.value;
    var InvNo = $("#ddlPurchaseinv_no option:selected").text();
    var InvNoVal = $("#ddlPurchaseinv_no option:selected").val();

    if (InvNo == "" || InvNo == null || InvNo == undefined) {
        InvNo = InvNo;
    }
    if (InvNoVal == "" || InvNoVal == null || InvNoVal == undefined || InvNoVal == "0") {
        InvNoVal = "";
    }
    if (Invoice_Type == "" || Invoice_Type == null || Invoice_Type == undefined) {
        Invoice_Type = InvoiceType;
    }
    if (InvNo == "" || InvNoVal == "") {
        $('#SpanPInvoiceErrorMsg').text($("#valueReq").text());
        $("#SpanPInvoiceErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "red");
        $("#BtnAddQtDt").css("display", "none")
    }
    else {
        var PInvoice = $('#ddlPurchaseinv_no option:selected').text();
        $("#Hdn_PInvNo").val(PInvoice);
        $("#SpanPInvoiceErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "#ced4da");
        $("#BtnAddQtDt").css("display", "")
    }
    $("#Inv_date").val(null);
    $("#GoodReceiptNoteNo").val(null);
    //$("#GRNDate").text(null);
    $("#PIBill_No").val(null);
    $("#PIBill_Date").val(null);
    document.getElementById("GRN_Date").value = "";
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/SupplementaryPurchaseInvoice/GetPurchaseInvoicesDetails",
                data: {
                    InvNo: InvNo,
                    InvoiceType: "D",//InvoiceType,
                },
                success: function (data) {
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);

                        var rcmValue = 'N';
                        /*console.log(rcmValue);*/
                        if (arr.Table.length > 0) {
                            $("#Inv_date").val(arr.Table[0].Invdt);
                            $("#GoodReceiptNoteNo").val(arr.Table[0].GRNNo);
                            $("#GRN_Date").val(arr.Table[0].GRNdt);
                            $("#PIBill_No").val(arr.Table[0].BillNo);
                            $("#PIBill_Date").val(arr.Table[0].Billdt);
                            rcmValue = arr.Table[0].rcm_app;
                            //console.log(rcmValue);
                            if (rcmValue === 'Y') {
                                $("#RCMApplicable").prop("checked", true);
                            } else if (rcmValue === 'N') {
                                $("#RCMApplicable").prop("checked", false);
                            }
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                        }
                        else {
                            $("#Inv_date").val(null);
                            $("#GoodReceiptNoteNo").val(null);
                            $("#GRNDate").val(null);
                            $("#PIBill_No").val(null);
                            $("#PIBill_Date").val(null);
                            $("#RCMApplicable").prop("checked", false);
                            $("#conv_rate").val(null);
                        }
                        $("#RCMApplicable").prop("disabled", true);
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function AddItemDetail() {
    var InvNo = $("#ddlPurchaseinv_no option:selected").text();
    var InvNoval = $("#ddlPurchaseinv_no option:selected").val();
    var Invoice_Type = $("input[name='OrderType']:checked").val();
    $("#HdnSuppType").val(Invoice_Type);
    if (Invoice_Type == "" || Invoice_Type == null || Invoice_Type == undefined) {
        Invoice_Type = InvoiceType.value;
    }
    if (InvNoval == "0") {
        $('#SpanPInvoiceErrorMsg').text($("#valueReq").text());
        $("#SpanPInvoiceErrorMsg").css("display", "block");
        $("#ddlPurchaseinv_no").css("border-color", "red");
        //$("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "red");
        //$("#BtnAddQtDt").css("display", "none");
        return;
    } else {
        let PInvoice = $('#ddlPurchaseinv_no option:selected').text();
        $("#Hdn_PInvNo").val(PInvoice);
        $("#SpanPInvoiceErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "#ced4da");
        $("#BtnAddQtDt").css("display", "");
    }
    $("#AddQt").hide();
    $("#ddlPurchaseinv_no").prop("disabled", true);
    $("#SupplierName").prop("disabled", true);
    $("#Domestic").prop("disabled", true);
    $("#Import").prop("disabled", true);
    // Ajax call to get data
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SupplementaryPurchaseInvoice/GetPurchaseInvoicesItemDetails",
            data: { InvoiceType: Invoice_Type, InvNo: InvNo },
            success: function (data) {
                if (data == 'ErrorPage') {
                    PI_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    // ////debugger;
                    arr = JSON.parse(data);
                    var RowNo = 0;
                    var levels = [];
                    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
                        var currentRow = $(this);
                        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());
                        levels.push(RowNo);
                    });
                    if (levels.length > 0) {
                        RowNo = Math.max(...levels);
                    }
                    RowNo = RowNo + 1;
                    if (RowNo == "0") {
                        RowNo = 1;
                    }
                    var CountRows = RowNo;
                    RowNo = "";

                    if (arr.Table.length > 0) {
                        var rowIdx = 0;
                        $('#POInvItmDetailsTbl tbody tr').remove();
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

                            BaseVal = (parseFloat(arr.Table[0].conv_rate) * parseFloat(arr.Table[k].net_val_spec)).toFixed(ValDecDigit)
                            var S_NO = 0;
                            if (k == 0) {
                                S_NO = 1;
                            }
                            else {
                                S_NO = k + 1;//$('#POInvItmDetailsTbl tbody tr').length + 1;
                            }
                            /* S_NO = $('#POInvItmDetailsTbl tbody tr').length + 1;*/
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            var deletetext = $("#Span_Delete_Title").text();
                            var ManualGst = "";
                            var TaxExempted = "";
                            var itc_claim = "";
                            if (GstApplicable == "Y") {
                                ManualGst = `<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input ${disabled} type="checkbox" ${arr.Table[k].manual_gst == "Y" ? "checked" : ""} class="custom-control-input margin-switch" id="ManualGST" disabled><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label><input type="hidden" id="SNohiddenfiled" value="@rowIdx" /></div></td>`;
                                itc_claim = ` <td class="qt_to">
            <div class="custom-control custom-switch sample_issue">
                    <input type="checkbox" checked class="custom-control-input  margin-switch" id="ClaimITC" disabled>
                <label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label>
            </div>
        </td>`
                            }
                            TaxExempted = `<td><div class="custom-control custom-switch sample_issue"><input ${disabled} type="checkbox" ${arr.Table[k].tax_expted == "Y" ? "checked" : ""} class="custom-control-input margin-switch" id="TaxExempted" disabled><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>`

                            var disabled = "";
                            $('#POInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}"> 
                                                                        <td class="sr_padding" id="srno"  rowspan="2" valign="middle" style="vertical-align:middle">${S_NO}</td>
                                                                        <td hidden  rowspan="2" valign="middle" style="vertical-align:middle">
                                                                            <input id="TxtGrnNo" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].GRNNo}" name="GrnNo" placeholder="GRN No" disabled>
                                                                        </td>
                                                                        <td hidden  rowspan="2" valign="middle" style="vertical-align:middle">
                                                                            <input id="TxtGrnDate" class="form-control" autocomplete="off" type="text" value="${arr.Table[k].GRNdt}" name="GrnNo" placeholder="GRN Date" onblur="this.placeholder='GRN Date'" disabled>
                                                                            <input type="hidden" id="hfGRNDate" value="${arr.Table[k].GRNdt}" />
                                                                        </td>
                                                                        <td  rowspan="2" valign="middle" class='ItmNameBreak itmStick tditemfrz' style="vertical-align:middle">
                                                                            <div class=" col-sm-10 no-padding">
                                                                                <input id="TxtItemName" class="form-control time" autocomplete="off" type="text" name="" value="${arr.Table[k].item_name}" onblur="this.placeholder='Item Name'" disabled>
                                                                            </div>
                                                                            <div class="col-sm-2 no-padding">
                                                                                <div class="col-sm-5 i_Icon">
                                                                                    <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button>
                                                                                </div>
                                                                                <div class="col-sm-7"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                                            </div>
                                                                        </td>
                                                                        <td rowspan="2" valign="middle" style="vertical-align:middle">
                                                                            <input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${arr.Table[k].uom_alias}" onblur="this.placeholder='UOM'" disabled>
                                                                            <input type="hidden" id="hfUOMID" value="${arr.Table[k].uom_id}" />
                                                                            <input id="ItemHsnCode" value="${arr.Table[k].hsn_code}" type="hidden" />
                                                                            <input id="ItemtaxTmplt" value="${arr.Table[k].tmplt_id}" type="hidden" />
                                                                            <input type="hidden" id="hdnItemType" value="${arr.Table[k].ItemType}"/>
                                                                            <input type="hidden" id="ForDisable" value="Enable" />
                                                                            <input type="hidden" id="hdnSrno" value="${arr.Table[k].count + 1}"/>
                                                                            <input type="hidden" id="hdn_item_gl_acc" value="${arr.Table[k].item_acc_id}"/>
                                                                        </td>
                                                                        <td rowspan="2" valign="middle" style="vertical-align:middle">
                                                                            <div class="col-sm-8 num_right no-padding">
                                                                                <input id="TxtReceivedQuantity" class="form-control num_right" autocomplete="off" type="text" name="ReceivedQuantity" value= ${parseFloat(arr.Table[k].mr_qty).toFixed(ValDecDigit)} placeholder="0000.00" disabled>
                                                                            </div>
                                                                            <div class=" col-sm-4 no-padding">
                                                                                <div class="col-sm-7 i_Icon" id="div_SubItemReceivedQty">
                                                                                    <button type="button" id="SubItemReceivedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                                                </div>
                                                                            </div>
                                                                            <span id="TxtReceivedQuantity_Error" class="error-message is-visible"></span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="lpo_form">
                                                                                <input id="TxtRate${rowIdx}" class="form-control num_right" onchange="OnChangeConsumbleItemPrice(event)" onkeypress="return QtyRateValueonly(this,event);" type="text" name="Rate" placeholder="0000.00" value= ${parseFloat(arr.Table[k].item_rate).toFixed(cmn_GrnRateDigit)} maxlength="10" autocomplete="off">
                                                                                <span id="TxtRate_Error${rowIdx}" class="error-message is-visible"></span>
                                                                                <input type="hidden" id="hfItemID" value="${arr.Table[k].item_id + 'R'}" />
                                                                                <input type="hidden" id="hfItemID_SubItems" value="${arr.Table[k].item_id}" />
                                                                                <input hidden type="text" id="sub_item" value="${arr.Table[k].sub_item}"/>
                                                                                <input hidden type="text" id="item_disc_val" value="0"/>
                                                                                <input  type="hidden" id="hfSNo" value="${rowIdx}"/>
                                                                                <input type="hidden" id="hfDataFor" value="R" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <input id="TxtItemGrossValue" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value=${parseFloat(arr.Table[k].item_gr_val).toFixed(ValDecDigit)} placeholder="0000.00" disabled>
                                                                        </td>
                                                                        <td>
                                                                            <input id="TxtItemGrossValueInBase" class="form-control num_right" autocomplete="off" type="text" name="GrossValueInBase" value="${parseFloat(arr.Table[k].value_bs).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                         <td>
                                                                            <input id="TxtItemDiffValue" class="form-control num_right" autocomplete="off" type="text" name="TxtItemDiffValue" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                          `+ TaxExempted + `
                                                                          `+ ManualGst + `
                                                                          `+ itc_claim + `
                                                                        <td>
                                                                            <div class=" col-sm-10 num_right no-padding">
                                                                                <input id="Txtitem_tax_amt" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_tax_amt).toFixed(ValDecDigit)} name="item_tax_amt" placeholder="0000.00" disabled>
                                                                            </div>
                                                                           <div class=" col-sm-2 num_right no-padding">
                                                                                <button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event, 'R')" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                                            </div>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxDiffValue" class="form-control num_right" autocomplete="off" type="text" name="TxtItemTaxDiffValue" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>


<td>
                                                                            <input id="TxtItemTaxRec" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(arr.Table[k].tax_amt_recov).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                        <td>
                                                                            <input id="TxtItemTaxRecDiff" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxNonRec" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(arr.Table[k].tax_amt_nrecov).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxNonRecDiff" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                        <td>
                                                                            <input id="TxtOtherCharge" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_oc_amt).toFixed(ValDecDigit)} name="OtherCharge" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtOtherChargeDiffValue" class="form-control num_right" autocomplete="off" type="text" name="TxtOtherChargeDiffValue" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                         <input id="TxtOtherChargeTP" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_oc_amtTP).toFixed(ValDecDigit)} name="TxtOtherChargeTP" placeholder="0000.00" disabled>
                                                <input id="TxtOtherChargeTP_prv" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_oc_amtTP).toFixed(ValDecDigit)} name="TxtOtherChargeTP" placeholder="0000.00" hidden>
                                                                        </td>
<td>
                                                                         <input id="TxtOtherChargeTPDiff" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(0).toFixed(ValDecDigit)} name="TxtOtherChargeTPDiff" placeholder="0000.00" disabled>
                                                                        </td>
                                                                        <td hidden style="display:@DisableForDomestic;"></td>
                                                                        <td>
                                                                            <input id="TxtNetValueInBase" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_net_val_bs).toFixed(ValDecDigit)} name="NetValueInBase" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtNetDiffValueInBase" class="form-control num_right" autocomplete="off" type="text" value="0.00" name="TxtNetDiffValueInBase" placeholder="0000.00" disabled>
                                                                        </td>
                                                                        <td>
                                                                            <input id="TxtLandedCostPerPc" class="form-control num_right" autocomplete="off" type="text" name="TxtLandedCostPerPc" value="${parseFloat(arr.Table[k].landed_cost_per_pc).toFixed(LddCostDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                         <td>
                                                                            <input id="TxtLandedCostPerPcDiffValue" class="form-control num_right" autocomplete="off" type="text" name="TxtLandedCostPerPcDiffValue" value="${parseFloat(0).toFixed(LddCostDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                    </tr>
                                                             <tr id="R${++rowIdx}">
                                                            <td>
                                                                 <div class="lpo_form">
                                                                             
    <input id="TxtRatePrev${rowIdx}" class="form-control num_right" autocomplete="" type="text" name="RatePrev" placeholder="0000.00" value= ${parseFloat(arr.Table[k].item_rate).toFixed(cmn_GrnRateDigit)} disabled>
                                                                                <span id="TxtRatePrev_Error" class="error-message is-visible"></span>
                                                                                <input hidden type="text" id="item_disc_valPrev" value="0"/>
                                                                                <input  type="hidden" id="hfSNo" value="${rowIdx}"/>
   <input type="hidden" id="hfItemID" value="${arr.Table[k].item_id + 'P'}" /><input type="hidden" id="hfDataFor" value="P" />
                                                                            </div>
                                                            </td>
                                                            <td>
                                                               <input id="TxtItemGrossValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtItemGrossValuePrev" value=${parseFloat(arr.Table[k].item_gr_val).toFixed(ValDecDigit)} placeholder="0000.00" disabled>
                                                            </td>
                                                             <td>
                                                                 <input id="TxtItemGrossValueInBasePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtItemGrossValueInBasePrev" value="${parseFloat(arr.Table[k].value_bs).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                            </td>
                                                            <td>
                                                                            <input id="TxtItemDiffValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtItemDiffValuePrev" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                             `+ TaxExempted + `
                                                             `+ ManualGst + `
                                                             `+ itc_claim + `
                                                          <td class="qt_to">
                                                               <div class=" col-sm-10 num_right no-padding">
                                                                                <input id="Txtitem_tax_amtPrev" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_tax_amt).toFixed(ValDecDigit)} name="Txtitem_tax_amtPrev" placeholder="0000.00" disabled>
                                                                            </div>
                                             <div class=" col-sm-2 num_right no-padding">
                                                                                <button type="button" class="calculator item_pop" id="BtnTxtCalculationPrev" data-toggle="modal" onclick="OnClickTaxCalBtn(event,'P')" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                                            </div>
                                                            </td>
                                                            <td>
                                                                            <input id="TxtItemTaxDiffValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtItemTaxDiffValuePrev" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled></td>
<td>
                                                                       
<input id="TxtItemTaxRecPrev" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(arr.Table[k].tax_amt_recov).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxRecDiffPrev" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxNonRecPrev" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(arr.Table[k].tax_amt_nrecov).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
<td>
                                                                            <input id="TxtItemTaxNonRecDiffPrev" class="form-control num_right" autocomplete="off" type="text" name="" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                            <td>
                                                                   <input id="TxtOtherChargePrev" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_oc_amt).toFixed(ValDecDigit)} name="TxtOtherChargePrev" placeholder="0000.00" disabled>
                                                            </td>
                                                             <td>
                                                             <input id="TxtOtherChargeDiffValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtOtherChargeDiffValuePrev" value="${parseFloat(0).toFixed(ValDecDigit)}" placeholder="0000.00" disabled></td>
<td>                             
<input id="TxtOtherChargeTPPrev" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_oc_amtTP).toFixed(ValDecDigit)} name="TxtOtherChargePrev" placeholder="0000.00" disabled>
                                                                        </td>
<td>
<input id="TxtOtherChargeTPPrevDiff" class="form-control num_right" autocomplete="off" type="text" value="${parseFloat(0).toFixed(ValDecDigit)}" name="TxtOtherChargePrev" placeholder="0000.00" disabled>
                                                                        </td>
                                                            <td hidden>
                                                                <input id="TxtNetValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtNetValuePrev" placeholder="0000.00" disabled="">
                                                            </td>
                                                            <td>
                                                                <input id="TxtNetValueInBasePrev" class="form-control num_right" autocomplete="off" type="text" value=${parseFloat(arr.Table[k].item_net_val_bs).toFixed(ValDecDigit)} name="TxtNetValueInBasePrev" placeholder="0000.00" disabled>
                                                            </td>   
                                                            <td>
                                                                            <input id="TxtNetValueInBasePrevDiff" class="form-control num_right" autocomplete="off" type="text" value="0.00" name="NetValueInBase" placeholder="0000.00" disabled="">
                                                                        </td>
                                                             <td>
                                                                            <input id="TxtNetLandedPrev" class="form-control num_right" autocomplete="off" type="text" name="TxtNetLanded" value="${parseFloat(arr.Table[k].landed_cost_per_pc).toFixed(LddCostDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                                         <td>
                                                                            <input id="TxtNetLandedDiffValuePrev" class="form-control num_right" autocomplete="off" type="text" name="TxtNetLandedDiffValue" value="${parseFloat(0).toFixed(LddCostDecDigit)}" placeholder="0000.00" disabled>
                                                                        </td>
                                                        </tr>`)
                        }

                        // $('#Hdn_TaxCalculatorTbl tbody tr').remove();
                        $('#Tbl_ItemTaxAmountList tbody tr').remove();
                        if (arr.Table1.length > 0) {
                            var ArrTaxList = [];
                            for (var l = 0; l < arr.Table1.length; l++) {
                                if (!arr.Table1[l].tax_name) {
                                    arr.Table1[l].tax_name = "Unknown";
                                }
                                $('#Hdn_TaxCalculatorTbl tbody').append(`
            <tr id="R${++rowIdx}">
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
                <td id="TaxAmountRevised">0.00</td>
            </tr>`);
                                if (arr.Table1[l].item_id.endsWith('R') || arr.Table1[l].tax_name.endsWith('R')) {
                                    let getIndex = ArrTaxList.findIndex(v => v.taxName === arr.Table1[l].tax_name);
                                    if (getIndex > -1) {
                                        ArrTaxList[getIndex].totalTaxAmt += parseFloat(CheckNullNumber(arr.Table1[l].tax_val));
                                    } else {
                                        ArrTaxList.push({
                                            taxID: arr.Table1[l].tax_id || 'Unknown', // Fallback to 'Unknown' if tax_id is missing
                                            taxAccID: arr.Table1[l].tax_acc_id,
                                            taxName: arr.Table1[l].tax_name,
                                            totalTaxAmt: parseFloat(arr.Table1[l].tax_val),
                                            TaxRecov: arr.Table1[l].recov
                                        });
                                    }
                                }
                            }

                            let TotalTAmt = 0;
                            //                for (var l = 0; l < ArrTaxList.length; l++) {
                            //                    $("#Tbl_ItemTaxAmountList tbody").append(`
                            //<tr>
                            //    <td>${ArrTaxList[l].taxName}</td>
                            //    <td id="taxRecov" class="center">
                            //        ${(ArrTaxList[l].TaxRecov === 'Y' ?
                            //                            '<i class="fa fa-check text-success " aria-hidden="true"></i>' :
                            //                            '<i class="fa fa-times-circle text-danger" aria-hidden="true"></i>')}
                            //    </td>
                            //    <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                            //    <td hidden="hidden" id="taxID">${ArrTaxList[l].tax_id}</td>
                            //    <td hidden="hidden" id="taxAccID">${ArrTaxList[l].taxAccID}</td>
                            //</tr>`);
                            //                    TotalTAmt += parseFloat(ArrTaxList[l].totalTaxAmt);
                            //                } $("#_ItemTaxAmountTotal").text(parseFloat(TotalTAmt).toFixed(ValDecDigit));
                        }

                        //OC Details
                        $('#Hdn_OC_TaxCalculatorTbl tbody tr').remove();
                        $('#Tbl_OtherChargeList tbody tr').remove();
                        if (arr.Table2.length > 0) {
                            for (var l = 0; l < arr.Table2.length; l++) {
                                $('#Hdn_OC_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table2[l].item_id}</td>
                                    <td id="TaxName">${arr.Table2[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table2[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table2[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table2[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table2[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table2[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table2[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table2[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table2[l].tax_acc_id}</td>
                                    <td id="OCFor">${arr.Table2[l].OCFor}</td>
                                 </tr>`);

                                $('#Hdn_OCTemp_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table2[l].item_id}</td>
                                    <td id="TaxName">${arr.Table2[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table2[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table2[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table2[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table2[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table2[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table2[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table2[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table2[l].tax_acc_id}</td>
                                    <td id="OCFor">${arr.Table2[l].ocfor}</td>
                                   </tr>`);
                            }
                        }
                        $('#ht_Tbl_OC_Deatils tbody tr').remove();
                        $('#Tbl_OC_Deatils tbody tr').remove();
                        let TotalAMount = 0;
                        let TotalTaxAMount = 0;
                        let TotalAMountWT = 0;
                        let TotalSuppOCAmtWT = 0;
                        let TotalSuppOCAmtTP = 0;
                        const totalOtherCharges = $("#span_TotalOtherCharges").text();

                        // ////debugger;
                        if (arr.Table3.length > 0) {
                            for (var l = 0; l < arr.Table3.length; l++) {
                                let row = arr.Table3[l];

                                $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
                        <td id="OC_name">${arr.Table3[l].oc_name}</td>
                        <td id="OC_HSNCode">${arr.Table3[l].HSN_code}</td>
                        <td id="OC_Curr">${arr.Table3[l].curr_name}</td>
                        <td id="HdnOC_CurrId">${arr.Table3[l].curr_id}</td>
                        <td id="td_OCSuppName">${IsNull(arr.Table3[l].supp_name, '')}</td>
                        <td id="td_OCSuppID">${IsNull(arr.Table3[l].supp_id, '')}</td>
                        <td id="td_OCSuppType">${IsNull(arr.Table3[l].supp_type, '')}</td>
                        <td id="OC_Conv">${arr.Table3[l].conv_rate}</td>
                        <td hidden="hidden" id="OC_ID">${arr.Table3[l].oc_id}</td>
                        <td class="num_right" id="OCAmtSp">${arr.Table3[l].oc_val}</td>
                        <td class="num_right" id="OCAmtBs">${arr.Table3[l].OCValBs}</td>
                        <td class="num_right" id="OCTaxAmt">${arr.Table3[l].tax_amt}</td>
                        <td class="num_right" id="OCTotalTaxAmt">${arr.Table3[l].total_amt}</td>
                        <td id="OCBillNo">${arr.Table3[l].bill_no == null ? "" : arr.Table3[l].bill_no}</td>
                        <td id="OCBillDate">${arr.Table3[l].bill_dt == null ? "" : arr.Table3[l].bill_dt}</td>
                        <td id="OCBillDt" hidden>${arr.Table3[l].bill_date == null ? "" : arr.Table3[l].bill_date}</td>
                        <td id="OCAccId" hidden>${arr.Table3[l].oc_acc_id}</td>
                        <td id="OCSuppAccId" hidden>${IsNull(arr.Table3[l].supp_acc_id, '')}</td>
                        <td id="OCFor" hidden>${arr.Table3[l].OCFor}</td>
                        <td id="OC_TDSAmt" hidden>${arr.Table3[l].tds_amt}</td>
                        </tr>`);

                                $('#Tbl_OC_Deatils tbody').append(`<tr id="R${l}">

                        <td id="deletetext" class=" red center">
                        //if(${arr.Table3[l].OCFor}=="R"){<i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" "></i>}</td>
                        if(${arr.Table3[l].DetailsShowStatus}=="R"){<i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" "></i>}</td>
                        <td id="OCName" >${arr.Table3[l].oc_name}</td>
                        <td id="OC_HSNCode">${arr.Table3[l].HSN_code}</td>
                        <td id="OCCurr" >${arr.Table3[l].curr_name}</td>
                        <td id="HdnOCCurrId" hidden>${arr.Table3[l].curr_id}</td>
                        <td id="td_OCSuppName" >${IsNull(arr.Table3[l].supp_name, '')}</td>
                        <td id="td_OCSuppID" style="display:none">${IsNull(arr.Table3[l].supp_id, '')}</td>
                        <td id="td_OCSuppType" style="display:none">${IsNull(arr.Table3[l].supp_type, '')}</td>
                        <td id="OCConv" class="num_right">${parseFloat(arr.Table3[l].conv_rate).toFixed(cmn_ExchDecDigit)}</td>
                        <td hidden="hidden" id="OCValue">${arr.Table3[l].oc_id}</td>
                        <td class="num_right" id="OCAmount">${parseFloat(arr.Table3[l].oc_val).toFixed(ValDecDigit)}</td>
                        <td class="num_right" id="OcAmtBs" >${parseFloat(arr.Table3[l].OCValBs).toFixed(ValDecDigit)}</td>
                          <td class="num_right">
                        <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(arr.Table3[l].tax_amt)).toFixed(ValDecDigit)}</div>
                         <div class="col-md-2 col-sm-12 no-padding">
                            <button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)"
                            data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false">
                            <i class="fa fa-calculator" aria-hidden="true" title=""></i></button>
                            </div>
                    </td>
                    <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(arr.Table3[l].total_amt)).toFixed(ValDecDigit)}</td>
                    <td id="OCBillNo">${arr.Table3[l].bill_no == null ? "" : arr.Table3[l].bill_no}</td>
                    <td id="OCBillDate">${arr.Table3[l].bill_dt == null ? "" : arr.Table3[l].bill_dt}</td>
                    <td id="OCBillDt" hidden>${arr.Table3[l].bill_date == null ? "" : arr.Table3[l].bill_date}</td>
                    <td id="OCAccId" hidden>${arr.Table3[l].oc_acc_id}</td>
                    <td id="OCSuppAccId" hidden>${IsNull(arr.Table3[l].supp_acc_id, '')}</td>
             /*       <td id="OCForId" hidden>${arr.Table3[l].OCFor}</td>*/
                    </tr>`);
                                /*$('#Tbl_OC_Deatils tfoot').append(`<tr id="R${l}">*/
                                if ($('#Tbl_OC_Deatils tfoot #TotalFooterRow').length === 0) {
                                    $('#Tbl_OC_Deatils tfoot').append(`
                    <tr id="TotalFooterRow" class="total">
                        <td>&nbsp;</td>
                        <td class="num_right"><strong>${totalOtherCharges}</strong></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td class="num_right">
                            <strong><span id="Total_OC_Amount"></span></strong>
                        </td>
                        <td class="num_right">
                        <strong><span id="Total_OC_AmountInBs"> </span></strong>
                        </td>
                        <td class="num_right"><strong><span id="Total_Oc_Tax_Amt"> </span></strong></td>
                        <td class="num_right"><strong><span id="Total_Oc_Amt_with_tx"> </span></strong></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>`
                                    );
                                }
                                TotalAMount = parseFloat(TotalAMount) + parseFloat(arr.Table3[l].OCValBs);
                                TotalTaxAMount = parseFloat(TotalTaxAMount) + parseFloat(arr.Table3[l].tax_amt);
                                TotalAMountWT = parseFloat(TotalAMountWT) + parseFloat(arr.Table3[l].total_amt);
                                if (arr.Table3[l].bill_no == "") {
                                    TotalSuppOCAmtWT = parseFloat(TotalSuppOCAmtWT) + parseFloat(arr.Table3[l].total_amt);
                                }
                                else {
                                    TotalSuppOCAmtTP = parseFloat(TotalSuppOCAmtTP) + parseFloat(arr.Table3[l].total_amt);
                                }
                            }

                            $("#TxtOtherCharges").val(TotalAMountWT.toFixed(ValDecDigit));
                            $("#TxtOtherChargesPrev").val(TotalAMountWT.toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherCharges").val(TotalSuppOCAmtWT.toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesPrev").val(TotalSuppOCAmtWT.toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesTP").val(TotalSuppOCAmtTP.toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesPrevTP").val(TotalSuppOCAmtTP.toFixed(ValDecDigit));
                            $("#TxtDiff_Amount").val(0.00);
                            $("#hdn_TxtDiff_Amount").val(0.00);
                        }
                        else {
                            $("#_OtherChargeTotal").text(parseFloat(0).toFixed(ValDecDigit));
                            $("#_OtherChargeTotalTax").text(parseFloat(0).toFixed(ValDecDigit));
                            $("#_OtherChargeTotalAmt").text(parseFloat(0).toFixed(ValDecDigit));
                            $("#TxtOtherChargesPrev").val(parseFloat(0).toFixed(ValDecDigit))
                            $("#TxtOtherCharges").val(parseFloat(0).toFixed(ValDecDigit))
                            $("#TxtDocSuppOtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesPrev").val(parseFloat(0).toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesTP").val(parseFloat(0).toFixed(ValDecDigit));
                            $("#TxtDocSuppOtherChargesPrevTP").val(parseFloat(0).toFixed(ValDecDigit));
                            $("#TxtDiff_Amount").val(0.00);
                            $("#hdn_TxtDiff_Amount").val(0.00);
                        }
                        CalculateAmountPrev();
                        //Calculate_OC_AmountItemWise(TotalSuppOCAmtWT.toFixed(ValDecDigit))
                        var Flag = "N";
                        $("#POInvItmDetailsTbl >tbody >tr").each(function () {
                            var row = $(this).closest('tr');
                            var hdnItemType = row.find("#hdnItemType").val();
                            if (hdnItemType == "Consumable") {
                                Flag = "Y";
                                OnClickTaxExemptedCheckBox(row, "AutoAplyTax");
                            }
                        });

                        var TDSAmt = 0;
                        if (arr.Table5.length > 0) {
                            for (var l = 0; l < arr.Table5.length; l++) {
                                TDSAmt = TDSAmt + parseFloat(arr.Table5[l].tds_val);
                                $("#Hdn_TDS_CalculatorTbl >tbody").append(`<tr>
                                    <td id="td_TDS_Name">${arr.Table5[l].tds_name}</td>
                                    <td id="td_TDS_NameID">${arr.Table5[l].tds_id}</td>
                                    <td id="td_TDS_Percentage">${arr.Table5[l].tds_rate}</td>
                                    <td id="td_TDS_Level">${arr.Table5[l].tds_level}</td>
                                    <td id="td_TDS_ApplyOn">${arr.Table5[l].tds_apply_Name}</td>
                                    <td id="td_TDS_Amount">${arr.Table5[l].tds_val}</td>
                                    <td id="td_TDS_ApplyOnID">${arr.Table5[l].tds_apply_on}</td>
                                    <td id="td_TDS_BaseAmt">${arr.Table5[l].tds_base_amt}</td>
                                    <td id="td_TDS_AccId">${arr.Table5[l].tds_acc_id}</td>
                                    <td id="td_TDS_AssValApplyOn">${arr.Table5[l].tds_ass_apply_on}</td>
                                </tr>`);
                            }
                        }
                        else {
                            TDSAmt = 0;
                        }
                        if (arr.Table5.length > 0) {
                            for (var l = 0; l < arr.Table5.length; l++) {
                                $("#Hdn_TDS_CalculatorTblRevised >tbody").append(`<tr>
                                    <td id="td_TDS_Name">${arr.Table5[l].tds_name}</td>
                                    <td id="td_TDS_NameID">${arr.Table5[l].tds_id}</td>
                                    <td id="td_TDS_Percentage">${arr.Table5[l].tds_rate}</td>
                                    <td id="td_TDS_Level">${arr.Table5[l].tds_level}</td>
                                    <td id="td_TDS_ApplyOn">${arr.Table5[l].tds_apply_Name}</td>
                                    <td id="td_TDS_Amount">${arr.Table5[l].tds_val}</td>
                                    <td id="td_TDS_ApplyOnID">${arr.Table5[l].tds_apply_on}</td>
                                    <td id="td_TDS_BaseAmt">${arr.Table5[l].tds_base_amt}</td>
                                    <td id="td_TDS_AccId">${arr.Table5[l].tds_acc_id}</td>
                                    <td id="td_TDS_AssValApplyOn">${arr.Table5[l].tds_ass_apply_on}</td>
                                </tr>`);
                            }
                        }
                        else {
                            TDSAmt = 0;
                        }
                        $("#TxtTDS_Amount").val(TDSAmt.toFixed(2));
                        $("#TxtTDS_AmountPrev").val(TDSAmt.toFixed(2));

                        var OCTDSAmt = 0;
                        if (arr.Table6.length > 0) {
                            for (var l = 0; l < arr.Table6.length; l++) {
                                OCTDSAmt = parseFloat(arr.Table6[l].tds_val);
                                $("#Hdn_OC_TDS_CalculatorTbl >tbody").append(`<tr>
                            <td id="td_TDS_Name">${arr.Table6[l].tds_name}</td>
                            <td id="td_TDS_NameID">${arr.Table6[l].tds_id}</td>
                            <td id="td_TDS_Percentage">${arr.Table6[l].tds_rate}</td>
                            <td id="td_TDS_Level">${arr.Table6[l].tds_level}</td>
                            <td id="td_TDS_ApplyOn">${arr.Table6[l].tds_apply_Name}</td>
                            <td id="td_TDS_Amount">${arr.Table6[l].tds_val}</td>
                            <td id="td_TDS_ApplyOnID">${arr.Table6[l].tds_apply_on}</td>
                            <td id="td_TDS_BaseAmt">${arr.Table6[l].tds_base_amt}</td>
                            <td id="td_TDS_AccId">${arr.Table6[l].tds_acc_id}</td>
                            <td id="td_TDS_AssValApplyOn">${arr.Table6[l].tds_ass_apply_on}</td>
                             <td id="td_TDS_OC_Id">${arr.Table6[l].tds_oc_id}</td>
                            <td id="td_TDS_Supp_Id">${arr.Table6[l].tds_supp_id}</td>
                                    </tr>`);
                            }
                        }
                        else {
                            OCTDSAmt = 0;
                        }

                        $("#TxtOCTDS_Amount").val(OCTDSAmt.toFixed(2));
                        $("#TxtOCTDS_AmountPrev").val(OCTDSAmt.toFixed(2));
                    }
                    $('#hdn_Sub_ItemDetailTbl tbody tr').remove();
                    if (arr.Table4.length > 0) {
                        var rowIdx = 0;
                        for (var y = 0; y < arr.Table4.length; y++) {
                            var srcDocNo = arr.Table4[y].mr_no;
                            var srcDocDate = arr.Table4[y].mr_dt;
                            var ItmId = arr.Table4[y].item_id;
                            var SubItmId = arr.Table4[y].sub_item_id;
                            var SubItmName = arr.Table4[y].sub_item_name;
                            var GrnQty = arr.Table4[y].Qty;

                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                                    <td><input type="text" id="subItemQty" value='${GrnQty}'></td>
                                    <td><input type="text" id="subItemSrcDocNo" value='${srcDocNo}'></td>
                                    <td><input type="text" id="subItemSrcDocDate" value='${srcDocDate}'></td>
                                    </tr>`);
                        }
                    }

                    if (arr.Table7.length > 0) {
                        var rowIdx = 0;
                        for (var y = 0; y < arr.Table7.length; y++) {
                            var netval = arr.Table7[y].net_val;
                            var netvalbs = arr.Table7[y].net_val_bs;
                            var gr_val = arr.Table7[y].gr_val;
                            var tds_amt = arr.Table7[y].tds_amt;
                            var oc_amt = arr.Table7[y].oc_amt;
                            var oc_amt_with_tp = arr.Table7[y].oc_amt_with_tp;
                            var oc_amt_tp = arr.Table7[y].oc_amt_tp;
                            var RecovAmt = arr.Table7[y].RecovAmt;
                            var NRecovAmt = arr.Table7[y].NRecovAmt;

                            $("#TxtGrossValuePrev").val(gr_val);
                            $("#TxtGrossValueInBasePrev").val(gr_val);
                            $("#NetOrderValueSpePrev").val(netval);
                            //$("#NetOrderValueInBasePrev").val(netval);
                            $("#TxtGrossValueInBase").val(gr_val);
                            $("#TaxAmountRecoverablePrev").val(RecovAmt);
                            $("#TaxAmountNonRecoverablePrev").val(NRecovAmt);
                            $("#NetOrderValueSpe").val(netval);
                            $("#TxtOtherChargesPrev").val(oc_amt_with_tp);
                            $("#TxtDocSuppOtherChargesPrev").val(oc_amt);
                            $("#TxtDocSuppOtherChargesPrevTP").val(oc_amt_tp);
                            $("#TxtTDS_AmountPrev").val(tds_amt);
                            $("#TxtDocSuppOtherChargesTP").val(oc_amt_tp)


                            $("#TxtGrossValue").val(gr_val);
                            $("#TxtGrossValueInBase").val(gr_val);
                            $("#TaxAmountRecoverable").val(RecovAmt);
                            $("#TaxAmountNonRecoverable").val(NRecovAmt);
                            $("#NetOrderValueSpe").val(netval);
                            $("#TxtDocSuppOtherCharges").val(oc_amt);
                            //$("#NetOrderValueInBase").val(netval);
                            //$("#hdn_NetOrderValueInBase").val(netvalbs);
                            $("#TDS_Amount").val(tds_amt);
                            $("#TxtDocSuppOtherCharges_self").val(oc_amt);
                        }
                    }
                }
            }

        });
    } catch (err) {
        console.log("SupplementaryPurchaseInvoice Error: " + err.message);
    }
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105101145");
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
                var firstEmptySelect = true;
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
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }
    });
}
function RateFloatValueonly(el, evt) {
    let RateDigit = ord_type ? "#ExpImpRateDigit" : "#RateDigit";
    if (CmnAmtFloatVal(el, evt, RateDigit) == false) {
        return false;
    }
    else {
        return true;
    }
}
function OnchangeConvRate(e) {
    var ConvRate = $("#conv_rate").val();
    if (AvoidDot(ConvRate) == false) {
        $("#conv_rate").val("");
        $("#hdnconv_rate").val(parseFloat(0).toFixed(cmn_ExchDecDigit));
    } else {
        $("#conv_rate").val(parseFloat(ConvRate).toFixed(cmn_ExchDecDigit));
        $("#hdnconv_rate").val(parseFloat(ConvRate).toFixed(cmn_ExchDecDigit));
    }
}

function InsertSPIDetails() {
    //debugger;
    try {
        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") {
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }

        var Dmenu = $("#DocumentMenuId").val();
        ////var DecDigit = $("#ValDigit").text();
        // var INSDTransType = sessionStorage.getItem("INSTransType");
        if (ValidateNetAmountBeforeSubmit() == false) {
            return false;
        }
        //Commented for while
        //if (ValidateGLAmountBeforeSubmit() == false) {
        //    return false;
        //}

        var HeaderValidation = CheckPI_Validations();
        if (HeaderValidation == false) {
            return false;
        }
        var BillDateRangeValidations = Check_BillDateRangeValidations();
        if (BillDateRangeValidations == false) {
            return false;
        }
        var ItemValidation = Check_ItemValidations();
        if (ItemValidation == false) {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
        var ItemRateValidation = Check_ItemRateValidations();
        if (ItemRateValidation == false) {
            return false;
        }

        var ItemRateValidation = Check_ItemRateiwthPrevValidations();
        if (ItemRateValidation == false) {
            return false;
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("POInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
                return false;
            }
        }
        var VoucherValidations = CheckPI_VoucherValidations();
        if (VoucherValidations == false) {
            return false;
        }
        if ($("#Cancelled").is(":checked")) {
            Cancelled = "Y";
        }
        else {
            Cancelled = "N";
        }

        var TransType = "";
        //if (INSDTransType === 'Update') {
        //    TransType = 'Update';
        //}
        //else {
        //    TransType = 'Save';
        //}
        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramtInBase1", "cramtInBase1", "hfPIStatus") == false) {
            return false;
        }
        if (HeaderValidation == true && ItemValidation == true) {
            var docCurr = $("#ddlCurrency").val();
            $("#Hdn_ddlCurrency").val(docCurr);
            $("#SupplierName").attr("disabled", false);
            var Narration = $("#DebitNoteRaisedAgainstInv").text()
            $('#hdNarration').val(Narration);
            var FinalItemDetail = [];
            FinalItemDetail = InsertItemDetails();
            var str = JSON.stringify(FinalItemDetail);
            $("#hdItemDetailList").val(str);

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
            vou_Detail = GetPI_VoucherDetails();
            var str_vou_Detail = JSON.stringify(vou_Detail);
            $('#hdItemvouDetail').val(str_vou_Detail);

            ///*-----------Sub-item-------------*/

            var SubItemsListArr = Cmn_SubItemList();
            var str2 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str2);

            ///*-----------Sub-item end-------------*/

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
            var Final_OC_TdsDetails_Revised = [];

            Final_OC_TdsDetails.map(item => {
                var matchedItem = OCDetail.filter(detail => detail.supp_id == item.Tds_supp_id && detail.OcFor == "R");
                if (matchedItem.length > 0) {
                    Final_OC_TdsDetails_Revised.push({ ...item });
                }
            });
            var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails_Revised);
            $('#hdn_oc_tds_details').val(Oc_Tds_Details);


            $("#RCMApplicable").prop("disabled", false);

            $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
            var Suppname = $('#SupplierName option:selected').text();
            $("#Hdn_PInvSuppName").val(Suppname);
            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
            $('#hdn_Attatchment_details').val(ItemAttchmentDt);
            /*----- Attatchment End--------*/

            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;
        }
        else {
            return false;
        }
    } catch (err) {
        console.error('An error occurred:', err);
    }
}
function InsertItemDetails() {
    var _ItemsDetail = [];
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var mr_no = "";
        var mr_date = "";
        var item_id = "";
        var uom_id = "";
        var inv_qty = "";
        var item_rate = "";
        var gl_vou_no = null;
        var gl_vou_dt = null;
        var TaxExempted = "";
        var hsn_code = "";
        var ManualGST = "";
        var ClaimITC = "";
        var item_acc_id = "";
        var item_gr_val_bs = "";
        var item_gr_val_bs_diff = "";
        var item_gr_val_sp = "";
        var item_gr_val_sp_diff = "";
        var item_tax_amt = "";
        var item_tax_amt_diff = "";
        var item_tax_recov = "";
        var item_tax_recov_diff = "";
        var item_tax_nrecov = "";
        var item_tax_nrecov_diff = "";
        var item_oc_amt_self = "";
        var item_oc_amt_self_diff = "";
        var item_oc_amt_tp = "";
        var item_net_val_sp = "";
        var item_net_val_sp_diff = "";
        var item_net_val_bs = "";
        var item_net_val_bs_diff = "";
        var item_landed_cost = "";
        var item_landed_cost_diff = "";
        var item_oc_amt_tp_prv = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#hfSNo").val();
        mr_no = currentRow.find("#TxtGrnNo").val();
        mr_date = currentRow.find("#hfGRNDate").val();
        item_id = currentRow.find("#hfItemID").val();
        uom_id = currentRow.find("#hfUOMID").val();
        inv_qty = currentRow.find("#TxtReceivedQuantity").val();
        item_rate = currentRow.find("#TxtRate" + SNo).val();
        item_gr_val_bs = currentRow.find("#TxtItemGrossValueInBase").val();
        item_gr_val_bs_diff = currentRow.find("#TxtItemDiffValue").val();
        item_gr_val_sp = currentRow.find("#TxtItemGrossValue").val();
        item_gr_val_sp_diff = currentRow.find("#TxtItemDiffValue").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        item_tax_amt_diff = currentRow.find("#TxtItemTaxDiffValue").val();
        item_tax_recov = currentRow.find("#TxtItemTaxRec").val();
        item_tax_recov_diff = currentRow.find("#TxtItemTaxRecDiff").val();
        item_tax_nrecov = currentRow.find("#TxtItemTaxNonRec").val();
        item_tax_nrecov_diff = currentRow.find("#TxtItemTaxNonRecDiff").val();
        item_oc_amt_self = currentRow.find("#TxtOtherCharge").val();
        item_oc_amt_self_diff = currentRow.find("#TxtOtherChargeDiffValue").val();
        item_oc_amt_tp = currentRow.find("#TxtOtherChargeTP").val();
        item_oc_amt_tp_diff = currentRow.find("#TxtOtherChargeTPDiff").val();
        item_oc_amt_tp_prv = currentRow.find("#TxtOtherChargeTP_prv").val();
        item_net_val_sp = 0;
        item_net_val_sp_diff = 0;
        item_net_val_bs = currentRow.find("#TxtNetValueInBase").val();
        item_net_val_bs_diff = currentRow.find("#TxtNetDiffValueInBase").val();
        item_landed_cost = currentRow.find("#TxtLandedCostPerPc").val();
        item_landed_cost_diff = currentRow.find("#TxtLandedCostPerPcDiffValue").val();
        hsn_code = currentRow.find("#ItemHsnCode").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();

        // Ensure numeric fields have a default value if empty or null
        if (item_tax_amt == "" || item_tax_amt == null) {
            item_tax_amt = "0";
        }
        if (item_tax_amt_diff == "" || item_tax_amt_diff == null) {
            item_tax_amt_diff = "0";
        }
        if (item_tax_recov == "" || item_tax_recov == null) {
            item_tax_recov = "0";
        }
        if (item_tax_recov_diff == "" || item_tax_recov_diff == null) {
            item_tax_recov_diff = "0";
        }
        if (item_tax_nrecov == "" || item_tax_nrecov == null) {
            item_tax_nrecov = "0";
        }
        if (item_tax_nrecov_diff == "" || item_tax_nrecov_diff == null) {
            item_tax_nrecov_diff = "0";
        }
        if (item_oc_amt_self == "" || item_oc_amt_self == null) {
            item_oc_amt_self = "0";
        }
        if (item_oc_amt_self_diff == "" || item_oc_amt_self_diff == null) {
            item_oc_amt_self_diff = "0";
        }
        if (item_oc_amt_tp == "" || item_oc_amt_tp == null) {
            item_oc_amt_tp = "0";
        }
        if (item_net_val_sp_diff == "" || item_net_val_sp_diff == null) {
            item_net_val_sp_diff = "0";
        }
        if (item_net_val_bs_diff == "" || item_net_val_bs_diff == null) {
            item_net_val_bs_diff = "0";
        }
        if (item_landed_cost == "" || item_landed_cost == null) {
            item_landed_cost = "0";
        }
        if (item_landed_cost_diff == "" || item_landed_cost_diff == null) {
            item_landed_cost_diff = "0";
        }
        if (item_oc_amt_tp_prv == "" || item_oc_amt_tp_prv == null) {
            item_oc_amt_tp_prv = "0";
        }
        if (item_oc_amt_tp_diff == "" || item_oc_amt_tp_diff == null) {
            item_oc_amt_tp_diff = "0";
        }

        gl_vou_no = null;
        gl_vou_dt = null;
        //debugger;
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y";
        } else {
            TaxExempted = "N";
        }

        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y";
            } else {
                ManualGST = "N";
            }
            if (currentRow.find("#ClaimITC").is(":checked")) {
                ClaimITC = "Y";
            } else {
                ClaimITC = "N";
            }
        }

        _ItemsDetail.push({
            mr_no: mr_no,
            mr_date: mr_date,
            item_id: item_id,
            uom_id: uom_id,
            inv_qty: inv_qty,
            item_rate: item_rate,
            gl_vou_no: gl_vou_no,
            gl_vou_dt: gl_vou_dt,
            TaxExempted: TaxExempted,
            hsn_code: hsn_code,
            ManualGST: ManualGST,
            ClaimITC: ClaimITC,
            item_acc_id: item_acc_id,
            item_gr_val_bs: item_gr_val_bs,
            item_gr_val_bs_diff: item_gr_val_bs_diff,
            item_gr_val_sp: item_gr_val_sp,
            item_gr_val_sp_diff: item_gr_val_sp_diff,
            item_tax_amt: item_tax_amt,
            item_tax_amt_diff: item_tax_amt_diff,
            item_tax_recov: item_tax_recov,
            item_tax_recov_diff: item_tax_recov_diff,
            item_tax_nrecov: item_tax_nrecov,
            item_tax_nrecov_diff: item_tax_nrecov_diff,
            item_oc_amt_self: item_oc_amt_self,
            item_oc_amt_self_diff: item_oc_amt_self_diff,
            item_oc_amt_tp: item_oc_amt_tp,
            item_net_val_sp: item_net_val_sp,
            item_net_val_sp_diff: item_net_val_sp_diff,
            item_net_val_bs: item_net_val_bs,
            item_net_val_bs_diff: item_net_val_bs_diff,
            item_landed_cost: item_landed_cost,
            item_landed_cost_diff: item_landed_cost_diff,
            item_oc_amt_tp_prv: item_oc_amt_tp_prv,
            item_oc_amt_tp_diff: item_oc_amt_tp_diff
        });

    });
    return _ItemsDetail;
}
function InsertTaxDetails() {
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
        var ocfor = currentRow.find("#OCFor").text();
        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt, ocfor: ocfor });
    });
    return TaxDetails;
}
function GetPI_OtherChargeDetails() {
    var PI_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = "";
            var oc_val = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var curr_id = "";
            var supp_id = "";
            var OcFor = "";
            oc_id = currentRow.find("#OCValue").text();
            oc_val = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            curr_id = currentRow.find("#HdnOCCurrId").text();
            supp_id = currentRow.find("#td_OCSuppID").text();
            var OCFor = currentRow.find("#OCFor").text();
            var OCForText = (OCFor.includes("P") ? "P" : (OCFor.includes("R") ? "R" : null));
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
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag, OcFor: OCForText
            });
        });
    }
    return PI_OCList;
};
function InsertTdsDetails() {
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTblRevised >tbody >tr").each(function (i, row) {
        var Tds_id = "";
        var Tds_rate = "";
        var Tds_level = "";
        var Tds_val = "";
        var Tds_apply_on = "";
        var Tds_name = "";
        var Tds_applyOnName = "";
        var Tds_totalAmnt = "";
        var currentRow = $(this);
        Tds_id = currentRow.find("#td_TDS_NameID").text();
        Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        Tds_level = currentRow.find("#td_TDS_Level").text();
        Tds_val = currentRow.find("#td_TDS_Amount").text();
        Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        Tds_name = currentRow.find("#td_TDS_Name").text();
        Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();
        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
}

function CheckPI_Validations() {
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#SupplierName").css("border-color", "#ced4da");
    }
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {

        return true;
    }
}
function OnchangePriceBasis() {
    if ($("#PriceBasis").val() == "") {
        $('#SpanPriceBasisErrorMsg').text($("#valueReq").text());
        $("#SpanPriceBasisErrorMsg").css("display", "block");
        $("#PriceBasis").css("border-color", "Red");
    }
    else {
        $("#SpanPriceBasisErrorMsg").css("display", "none");
        $("#PriceBasis").css("border-color", "#ced4da");
    }
}
function OnchangeFreightType() {
    if ($("#FreightType").val() == "") {
        $('#SpanFreightTypeErrorMsg').text($("#valueReq").text());
        $("#SpanFreightTypeErrorMsg").css("display", "block");
        $("#FreightType").css("border-color", "Red");
    }
    else {
        $("#SpanFreightTypeErrorMsg").css("display", "none");
        $("#FreightType").css("border-color", "#ced4da");
    }
}
function OnchangeModeOfTransport() {
    if ($("#ModeOfTransport").val() == "") {
        $('#SpanModeOfTransportErrorMsg').text($("#valueReq").text());
        $("#SpanModeOfTransportErrorMsg").css("display", "block");
        $("#ModeOfTransport").css("border-color", "Red");
    }
    else {
        $("#SpanModeOfTransportErrorMsg").css("display", "none");
        $("#ModeOfTransport").css("border-color", "#ced4da");
    }
}
function OnchangeDestination() {
    if ($("#Destination").val() == "") {
        $('#SpanDestinationErrorMsg').text($("#valueReq").text());
        $("#SpanDestinationErrorMsg").css("display", "block");
        $("#Destination").css("border-color", "Red");
    }
    else {
        $("#SpanDestinationErrorMsg").css("display", "none");
        $("#Destination").css("border-color", "#ced4da");
    }
}
function GetPI_VoucherDetails() {
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "";
    if ($("#OrderTypeL").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "E";
    }
    var TransType = "Pur";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var dr_amt_bs = "";
            var cr_amt_bs = "";
            var Gltype = "";
            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#txthfAccID").val();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            dr_amt_bs = currentRow.find("#dramtInBase").text();
            cr_amt_bs = currentRow.find("#cramtInBase").text();
            Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            //var bill_no = currentRow.find("#gl_bill_no").val();
            //var bill_date = currentRow.find("#gl_bill_date").val();
            var bill_no = $("#Bill_No").val();
            var bill_date = $("#Bill_Date").val();
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

function Check_ItemValidations() {
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
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
function Check_BillDateRangeValidations() {
    var ErrorFlag = "N";
    //Bill Date 10 yrs range validation Start
    let minDateStr = new Date(new Date().setFullYear(new Date().getFullYear() - 10)).toISOString().slice(0, 10);
    let maxDateStr = new Date().toISOString().slice(0, 10);
    const minDate = new Date(minDateStr + "T00:00:00");
    const maxDate = new Date(maxDateStr + "T00:00:00");
    const input = document.getElementById('Bill_Date');
    const value = input.value;
    const enteredDate = new Date(value + "T00:00:00");
    if (enteredDate < minDate || enteredDate > maxDate) {
        $('#SpanBillDateErrorMsg').text($("#JC_InvalidDate").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    //Bill Date 10 yrs range validation End
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function Check_ItemRateValidations() {
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#POInvItmDetailsTbl > tbody > tr").each(function (index) {
            if (index % 2 === 0) {
                var currentRow = $(this);
                var SNo = currentRow.find("#hfSNo").val();

                var TxtRate = currentRow.find("#TxtRate" + SNo).val();
                if ((parseFloat(CheckNullNumber(TxtRate)) == 0)) {
                    currentRow.find("#TxtRate" + SNo).css("border-color", "red");
                    currentRow.find("#TxtRate_Error" + SNo).css("display", "block");
                    currentRow.find("#TxtRate_Error" + SNo).text($("#valueReq").text());
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#TxtRate" + SNo).css("border-color", "#ced4da");
                    currentRow.find("#TxtRate_Error" + SNo).css("display", "none");

                }
            }
        });
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function Check_ItemRateiwthPrevValidations() {
    var ErrorFlag = "N";
    if ($("#POInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#POInvItmDetailsTbl > tbody > tr").each(function (index) {
            var currentRow = $(this);
            var SNo = currentRow.find("#hfSNo").val();
            var TxtRate = currentRow.find("#TxtRate" + SNo).val();
            var SNo_next = currentRow.next().find("#hfSNo").val();
            var TxtRatePrev = currentRow.next().find("#TxtRatePrev" + SNo_next).val();

            if ((parseFloat(CheckNullNumber(TxtRate)) < TxtRatePrev)) {
                currentRow.find("#TxtRate" + SNo).css("border-color", "red");
                currentRow.find("#TxtRate_Error" + SNo).css("display", "block");
                currentRow.find("#TxtRate_Error" + SNo).text($("#span_InvalidPrice").text());
                ErrorFlag = "Y";
            } else {
                currentRow.find("#TxtRate" + SNo).css("border-color", "#ced4da");
                currentRow.find("#TxtRate_Error" + SNo).css("display", "none");
                currentRow.find("#TxtRate_Error" + SNo).text("");
            }
        });
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function CheckPI_VoucherValidations() {
    if (Cmn_CheckGlVoucherValidations() == false) {
        return false;
    } else {
        return true;
    }
}
function OnChangeConsumbleItemQuantity(e) {
    var currentrow = $(e.target).closest('tr');
    var TxtReceivedQuantity = currentrow.find("#TxtReceivedQuantity").val();
    currentrow.find("#TxtReceivedQuantity").val(parseFloat(CheckNullNumber(TxtReceivedQuantity)).toFixed(QtyDecDigit));
    if (parseFloat(CheckNullNumber(TxtReceivedQuantity)) <= 0) {
        currentrow.find("#TxtReceivedQuantity").css("border-color", "red");
        currentrow.find("#TxtReceivedQuantity_Error").css("display", "block");
        currentrow.find("#TxtReceivedQuantity_Error").text($("#valueReq").text());
        return false;

    } else {
        currentrow.find("#TxtReceivedQuantity").css("border-color", "#ced4da");
        currentrow.find("#TxtReceivedQuantity_Error").css("display", "none");
        currentrow.find("#TxtReceivedQuantity_Error").text("");
    }
    OnChangeConsumbleItemPrice(e);
}

function CalculateTaxExemptedAmt(e, flag) {
    var ConvRate = $("#conv_rate").val();
    var clickedrow = $(e.target).closest("tr");
    var OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    var TxtGrnNo = clickedrow.find("#TxtGrnNo").val();
    var ItmRate = clickedrow.find("#TxtRate").val();
    var oc_amt = clickedrow.find("#TxtOtherCharge").val();
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (oc_amt == ".") {
        oc_amt = 0;
    }
    if (oc_amt != "" && oc_amt != ".") {
        oc_amt = parseFloat(oc_amt);
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinVal);
        clickedrow.find("#NetOrderValueSpe").val(FinVal);
        clickedrow.find("#TxtNetValue").val(FinVal);
        FinalVal = FinVal * ConvRate

        FinalVal = FinalVal + oc_amt
        var FinalNetValue = parseFloat(FinVal) / ConvRate //+ oc_amt;
        clickedrow.find("#TxtNetValue").val(FinalNetValue);
        clickedrow.find("#TxtNetValueInBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount(true);
    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        }
    }
    CalculateAmount(true);
}

function OnClickIconBtn(e) {
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hfItemID_SubItems").val();
    if (ItmCode.endsWith("R")) {
        ItmCode = ItmCode.slice(0, -1);
    }
    ItemInfoBtnClick(ItmCode);
}
function ResetGRN_DDL_Detail() {
    $("#GRN_Date").val("");
    var DocNo = $('#ddlGoodReceiptNoteNo').val();
    $("#ddlGoodReceiptNoteNo>optgroup>option[value='" + DocNo + "']").select2().hide();
    $('#ddlGoodReceiptNoteNo').val("---Select---").prop('selected', true);
    $('#ddlGoodReceiptNoteNo').trigger('change'); // Notify any JS components that the value changed
    $('#ddlGoodReceiptNoteNo').select2('close');
    $("#SpanGRNNoErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
    $("#ddlGoodReceiptNoteNo").css("border-color", "red");
}

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
            RemoveSessionNew();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function RemoveSessionNew() {
    sessionStorage.removeItem("PI_TaxCalcDetails");
    sessionStorage.removeItem("PI_TransType");
}
//------------------Tax Amount Calculation------------------//
function OnClickTaxCalBtn(e, Param) {

    var SOItemListName = "#TxtItemName";
    var currentRow = $(e.target).closest("tr");
    if (Param == "P") {
        var SNohiddenfiled = "SUPPPI_P";
        var item_gross_val = currentRow.find("#TxtItemGrossValueInBasePrev").val();
    }
    else if (Param == "R") {
        var SNohiddenfiled = "SUPPPI_R";
        var item_gross_val = currentRow.find("#TxtItemGrossValueInBase").val();
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    var Itm_ID = currentRow.find("#hfItemID").val();
    var mr_no = currentRow.find("#TxtGrnNo").val();
    var mr_date = currentRow.find("#hfGRNDate").val();
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    if (GstApplicable == "Y") {
        if (currentRow.find("#ForDisable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").prop("disabled", true).css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").prop("disabled", true).css("display", "none");
                //$("#SaveAndExitBtn").prop("disabled", false).css("display", "block");
            }
        }
    }
}

function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertSPIDetails()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    var ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var ItemTaxAmountTotal = "#_ItemTaxAmountTotal";
    CMNBindTaxAmountDeatils(TaxAmtDetail, ItemTaxAmountList, ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}
function OnClickSaveAndExit_OC_Btn() {
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueSpe", "#NetOrderValueInBase");
}
function OnClickSaveAndExit(OnAddGRN, AutoAplyTaxPI) {
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    let NewArr = [];
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    if ($("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text(); /* commented by Suraj on 01-02-2022 due to not in use*/
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            }
            else {
                if (TaxItemID == TaxItmCode && DocNo == GRNNo && DocDate == GRNDate) {
                    $(this).remove();
                }
                else {
                    var TaxName = currentRow.find("#TaxName").text();
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxAmount = currentRow.find("#TaxAmount").text();
                    var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                    var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                    var TaxAccId = currentRow.find("#TaxAccId").text();
                    var TaxRecov = currentRow.find("#TaxRecov").text();
                    var OCFor = currentRow.find("#OCFor").text();

                    NewArr.push({ /*UserID: UserID, */DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov, OCFor: OCFor })
                }
            }
        });

        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            var Ocfor = currentRow.find("#OCFor").text();
            $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccID}</td>
            <td id="OCFor">R</td>
                </tr>`);

            NewArr.push({
                DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName
                , TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage
                , TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount
                , TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccID, TaxRecov: TaxRecov, Ocfor: Ocfor
            })
        });
    }
    else {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxAccID = currentRow.find("#AccID").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            var TaxRecov = currentRow.find("#tax_recov").text();
            var Ocfor = currentRow.find("#OCFor").text();

            $("#" + HdnTaxCalculateTable + " tbody").append(`<tr id="R${++rowIdx}">
            <td id="DocNo">${GRNNo}</td>
            <td id="DocDate">${GRNDate}</td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
            <td id="TaxRecov">${TaxRecov}</td>
            <td id="TaxAccId">${TaxAccID}</td>
            <td id="OCFor">R</td>
                </tr>`);

            NewArr.push({
                DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage,
                TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId,
                TaxRecov: TaxRecov, Ocfor: Ocfor
            })
        });
    }
    if (TaxOn != "OC" && TaxOn != "") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit)
                try {
                    if (currentRow.find("#oc_chk_roundoff").is(":checked")) {
                        if (currentRow.find("#oc_p_round").is(":checked")) {
                            Total = SPI_RoundValue(Total, "P");
                        }
                        if (currentRow.find("#oc_m_round").is(":checked")) {
                            Total = SPI_RoundValue(Total, "M");
                        }
                    }
                    currentRow.find("#OCTotalTaxAmt").text(Total);
                }
                catch { }
            }
        });
        Calculate_OCAmount();
    } else {
        $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode && GRNNoTbl == GRNNo && GRNDateTbl == GRNDate) {
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
                }
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);

                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(NetOrderValueBase) / ConvRate).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                //FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(ValDecDigit);
                //currentRow.find("#TxtNetValueInBase").val(NetOrderValueBase);
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt1 = currentRow.find("#Txtitem_tax_amt").val();
            if (ItemTaxAmt1 != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount(true);
    }
    //if ($("#taxTemplate").text() == "GST Slab") {
    if (AutoAplyTaxPI != "AutoAplyTaxPI") {
        GetAllGLID();
    }
    //}
}

function CheckAmountStatus() {
    var status = false;
    const rows = $("#POInvItmDetailsTbl > tbody > tr").closest('tr');
    let TotalOCAmount = 0;
    let totalRateValue = 0;
    let totalRateValuePrev = 0;
    rows.each(function () {
        const currentRow = $(this);
        const nextRow = currentRow.next();
        var SNo = currentRow.find("#hfSNo").val();
        var SNo_next = nextRow.find("#hfSNo").val();
        const rateValue = parseFloat(currentRow.find("#TxtRate" + SNo).val()) || 0;
        const rateValuePrev = parseFloat(nextRow.find("#TxtRatePrev" + SNo_next).val()) || 0;
        totalRateValue += rateValue;
        totalRateValuePrev += rateValuePrev;
    });
    if (totalRateValue == totalRateValuePrev) {
        status = false;
    }
    else {
        status = true;
    }
    return status;
}
function Calculate_OC_AmountItemWiseWithNoChange(TotalOCAmt) {
    var TotalGAmt = $("#TxtGrossValueInBase").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    // Cache the rows once to avoid repeated jQuery DOM queries
    const rows = $("#POInvItmDetailsTbl > tbody > tr").closest('tr');
    let TotalOCAmount = 0;

    // First pass: calculate and set OtherCharge values, and sum them up in one loop
    rows.each(function () {
        const currentRow = $(this);
        const nextRow = currentRow.next();
        const grossValue = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()) || 0;
        const grossValuePrev = parseFloat(nextRow.find("#TxtItemGrossValueInBasePrev").val()) || 0;
        const OCAmtItemWisePrev = parseFloat(nextRow.find("#TxtOtherChargePrev").val()) || 0;
        const OCAmtItemWiseAmt = parseFloat(currentRow.find("#TxtOtherCharge").val()) || 0;
        const OCAmtItemWiseAmtDiff = parseFloat(currentRow.find("#TxtOtherChargeDiffValue").val()) || 0;
        const DiffVal = parseFloat(OCAmtItemWiseAmt - OCAmtItemWiseAmtDiff).toFixed(ValDecDigit) || 0;
        let otherCharge = 0;
        //currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit));
        //otherCharge = parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit);
        //TotalOCAmount += parseFloat(otherCharge);

        if ((DiffVal) == OCAmtItemWisePrev) {
            currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit));
            currentRow.find("#TxtOtherChargeDiffValue").val(0);
            otherCharge = parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit);
        }
        else {
            if (grossValue > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
                const OCPercentage = ((grossValue / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
                //otherCharge = (((OCPercentage * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
                var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
                if (parseFloat(OCAmtItemWise) != 0) {
                    // otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
                    currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
                    otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
                }
                else {
                    otherCharge = parseFloat(0).toFixed(ValDecDigit);
                    currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
        }
        TotalOCAmount += parseFloat(otherCharge);
    });
    TotalOCAmount = parseFloat(TotalOCAmount);
    const totalOCAmtFloat = parseFloat(TotalOCAmt);
    if (TotalOCAmount !== totalOCAmtFloat) {
        const AmtDifference = totalOCAmtFloat - TotalOCAmount;
        const firstRow = rows.first();
        const firstRowOCValue = parseFloat(firstRow.find("#TxtOtherCharge").val()) || 0;
        const adjustedValue = firstRowOCValue + AmtDifference;
        firstRow.find("#TxtOtherCharge").val(adjustedValue.toFixed(ValDecDigit));
    }

    $("#POInvItmDetailsTbl >tbody >tr").closest('tr').each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        let Qty = currentRow.find("#TxtReceivedQuantity").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        var POItm_OCAmtTP = currentRow.find("#TxtOtherChargeTP").val();
        if (POItm_OCAmtTP == null || POItm_OCAmtTP == "") {
            POItm_OCAmtTP = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt) + parseFloat(POItm_OCAmtTP));
        var POItm_NetOrderValueSpec = (parseFloat(POItm_NetOrderValueBase) / ConvRate);
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
        currentRow.find("#TxtNetValueInBase").val((parseFloat(POItm_NetOrderValueBase)).toFixed(ValDecDigit));

        var Txtitem_Net_amtPrev = currentRow.next().find("#TxtNetValueInBasePrev").val();
        var DiffNetAmt = parseFloat(POItm_NetOrderValueBase) - parseFloat(CheckNullNumber(Txtitem_Net_amtPrev));
        currentRow.find("#TxtNetDiffValueInBase").val(parseFloat(DiffNetAmt).toFixed(ValDecDigit));

        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
            OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
        }
        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
        AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherChargeTP").val() != null && currentRow.find("#TxtOtherChargeTP").val() != "") {
            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherChargeTP").val()).toFixed(ValDecDigit);
        }

        var Tax_Amt_nonRec = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtItemTaxNonRec").val() != null && currentRow.find("#TxtItemTaxNonRec").val() != "") {
            Tax_Amt_nonRec = parseFloat(currentRow.find("#TxtItemTaxNonRec").val()).toFixed(ValDecDigit);
        }

        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(POItm_TaxAmt) + parseFloat(OC_Amt) + parseFloat(OC_Amt_OR)).toFixed(ValDecDigit);
        currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
        let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(Tax_Amt_nonRec));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(Qty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));

        var val = parseFloat(currentRow.next().find("#TxtNetLandedPrev").val());
        var LandedCostPrev = isNaN(val) ? (0).toFixed(LddCostDecDigit) : val.toFixed(LddCostDecDigit);
        var LandedCostDiff = parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit) - parseFloat(CheckNullNumber(LandedCostPrev)).toFixed(LddCostDecDigit);
        currentRow.find("#TxtLandedCostPerPcDiffValue").val(parseFloat(CheckNullNumber(LandedCostDiff)).toFixed(LddCostDecDigit));
    });
    CalculateVoucherTotalAmount();
    CalculateAmountCoreLogic("OC"); FinalizeCalculation();
    OnClickTDSCalculationBtn();
};
function Calculate_OC_AmountItemWiseTPWithNoChange(TotalOCAmt) {
    //debugger;
    var TotalGAmt = $("#TxtGrossValueInBase").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    const rows = $("#POInvItmDetailsTbl > tbody > tr").closest('tr');
    let TotalOCAmountTP = 0;
    const totalOCAmtFloatTP = parseFloat(TotalOCAmt);
    //debugger;
    // Calculate and set OtherChargeTP, sum total in one pass
    rows.each(function () {
        const currentRow = $(this);
        const nextRow = currentRow.next();
        const grossValue = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()) || 0;
        let otherChargeTP = 0;
        const grossValuePrev = parseFloat(nextRow.find("#TxtItemGrossValueInBasePrev").val()) || 0;
        const OCAmtItemWisePrev = parseFloat(nextRow.find("#TxtOtherChargeTPPrev").val()) || 0;
        const OCAmtItemWise = parseFloat(currentRow.find("#TxtOtherChargeTP").val()) || 0;
        //  currentRow.find("#TxtOtherChargeTP").val(parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit));
        // otherChargeTP = parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit);
        const OCAmtItemWiseAmtDiff = parseFloat(currentRow.find("#TxtOtherChargeTPDiff").val()) || 0;
        const DiffVal = parseFloat(OCAmtItemWise - OCAmtItemWiseAmtDiff).toFixed(ValDecDigit) || 0;
        /*if ((OCAmtItemWisePrev + OCAmtItemWise) == OCAmtItemWisePrev) {*/
        if ((DiffVal) == OCAmtItemWisePrev) {
            currentRow.find("#TxtOtherChargeTP").val(parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit));
            otherChargeTP = parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit);
        }
        else {
            if (grossValue > 0 && parseFloat(TotalGAmt) > 0 && totalOCAmtFloatTP > 0) {
                const OCPercentageTP = ((grossValue / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
                var OCAmtItemWiseTP = ((parseFloat(OCPercentageTP) * parseFloat(totalOCAmtFloatTP)) / 100).toFixed(ValDecDigit);
                if (parseFloat(OCAmtItemWiseTP) != 0) {
                    currentRow.find("#TxtOtherChargeTP").val(parseFloat(OCAmtItemWiseTP).toFixed(ValDecDigit));
                    otherChargeTP = parseFloat(OCAmtItemWiseTP).toFixed(ValDecDigit);
                }
                else {
                    otherChargeTP = parseFloat(0).toFixed(ValDecDigit);
                    currentRow.find("#TxtOtherChargeTP").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
            else {
                if (totalOCAmtFloatTP == 0) {
                    currentRow.find("#TxtOtherChargeTP").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
        }
        TotalOCAmountTP += parseFloat(otherChargeTP);
    });
    TotalOCAmountTP = parseFloat(TotalOCAmountTP.toFixed(ValDecDigit));
    if (TotalOCAmountTP !== totalOCAmtFloatTP) {
        const AmtDifference = totalOCAmtFloatTP - TotalOCAmountTP;
        const firstRow = rows.first();
        const firstRowVal = parseFloat(firstRow.find("#TxtOtherChargeTP").val()) || 0;
        firstRow.find("#TxtOtherChargeTP").val((firstRowVal + AmtDifference).toFixed(ValDecDigit));
    }

    $("#POInvItmDetailsTbl >tbody >tr").closest('tr').each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val() || 0.00;
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        var POItm_OCAmtTP = currentRow.find("#TxtOtherChargeTP").val();
        let Qty = currentRow.find("#TxtReceivedQuantity").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (POItm_OCAmtTP == null || POItm_OCAmtTP == "") {
            POItm_OCAmtTP = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }

        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt) + parseFloat(POItm_OCAmtTP));
        // console.log("POItm_NetOrderValueBase: " + POItm_NetOrderValueBase);
        var POItm_NetOrderValueSpec = (parseFloat(POItm_NetOrderValueBase) / ConvRate);
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
        currentRow.find("#TxtNetValueInBase").val(parseFloat(POItm_NetOrderValueBase).toFixed(ValDecDigit));

        var Txtitem_Net_amtPrev = currentRow.next().find("#TxtNetValueInBasePrev").val();
        var DiffNetAmt = parseFloat(POItm_NetOrderValueBase) - parseFloat(CheckNullNumber(Txtitem_Net_amtPrev));
        currentRow.find("#TxtNetDiffValueInBase").val(parseFloat(DiffNetAmt).toFixed(ValDecDigit));
        //currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));

        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
            OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
        }
        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
        AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);

        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherChargeTP").val() != null && currentRow.find("#TxtOtherChargeTP").val() != "") {
            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherChargeTP").val()).toFixed(ValDecDigit);
        }

        var Tax_Amt_nonRec = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtItemTaxNonRec").val() != null && currentRow.find("#TxtItemTaxNonRec").val() != "") {
            Tax_Amt_nonRec = parseFloat(currentRow.find("#TxtItemTaxNonRec").val()).toFixed(ValDecDigit);
        }

        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(POItm_TaxAmt) + parseFloat(OC_Amt) + parseFloat(OC_Amt_OR)).toFixed(ValDecDigit);
        currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);

        //let LandedCostValue = parseFloat(CheckNullNumber(NetOrderValueSpec));
        let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(Tax_Amt_nonRec));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(Qty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));

        var val = parseFloat(currentRow.next().find("#TxtNetLandedPrev").val());
        var LandedCostPrev = isNaN(val) ? (0).toFixed(LddCostDecDigit) : val.toFixed(LddCostDecDigit);
        var LandedCostDiff = parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit) - parseFloat(CheckNullNumber(LandedCostPrev)).toFixed(LddCostDecDigit);
        currentRow.find("#TxtLandedCostPerPcDiffValue").val(parseFloat(CheckNullNumber(LandedCostDiff)).toFixed(LddCostDecDigit));
    });
    CalculateVoucherTotalAmount();
    CalculateAmountCoreLogic("OC"); FinalizeCalculation();
    OnClickTDSCalculationBtn();
};
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    var TotalGAmt = $("#TxtGrossValueInBase").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    const rows = $("#POInvItmDetailsTbl > tbody > tr").closest('tr');
    let TotalOCAmount = 0;
    rows.each(function () {
      /*  debugger;*/
        const currentRow = $(this);
        const nextRow = currentRow.next();
        const grossValue = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()) || 0;
        const grossValuePrev = parseFloat(nextRow.find("#TxtItemGrossValueInBasePrev").val()) || 0;
        const OCAmtItemWisePrev = parseFloat(nextRow.find("#TxtOtherChargePrev").val()) || 0;
        let otherCharge = 0;

        //if (grossValue == grossValuePrev) {
        //    currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit));
        //    otherCharge = parseFloat(OCAmtItemWisePrev).toFixed(ValDecDigit);
        //}
        //else {
        //    if (grossValue > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
        //        const OCPercentage = ((grossValue / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
        //        //otherCharge = (((OCPercentage * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
        //        var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
        //        if (parseFloat(OCAmtItemWise) != 0) {
        //            // otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
        //            currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
        //            otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
        //        }
        //        else {
        //            otherCharge = parseFloat(0).toFixed(ValDecDigit);
        //            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
        //        }
        //    }
        //}
        if (grossValue > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            const OCPercentage = ((grossValue / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            //otherCharge = (((OCPercentage * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWise) != 0) {
                // otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
                otherCharge = parseFloat(OCAmtItemWise).toFixed(ValDecDigit);
            }
            else {
                otherCharge = parseFloat(0).toFixed(ValDecDigit);
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        else {
            if (parseFloat(TotalOCAmt) == 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        // Accumulate total OC amount
        TotalOCAmount += parseFloat(otherCharge);
    });

    // Round to fixed decimal (to avoid floating point issues later)
    //TotalOCAmount = parseFloat(TotalOCAmount.toFixed(ValDecDigit));
    TotalOCAmount = parseFloat(TotalOCAmount);

    const totalOCAmtFloat = parseFloat(TotalOCAmt);

    // Adjust the first row if there's any difference between sum and expected total
    if (TotalOCAmount !== totalOCAmtFloat) {
        const AmtDifference = totalOCAmtFloat - TotalOCAmount;
        const firstRow = rows.first();
        const firstRowOCValue = parseFloat(firstRow.find("#TxtOtherCharge").val()) || 0;
        const adjustedValue = firstRowOCValue + AmtDifference;
        firstRow.find("#TxtOtherCharge").val(adjustedValue.toFixed(ValDecDigit));
    }

    $("#POInvItmDetailsTbl >tbody >tr").closest('tr').each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        let Qty = currentRow.find("#TxtReceivedQuantity").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        var POItm_OCAmtTP = currentRow.find("#TxtOtherChargeTP").val();
        if (POItm_OCAmtTP == null || POItm_OCAmtTP == "") {
            POItm_OCAmtTP = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt) + parseFloat(POItm_OCAmtTP));
        var POItm_NetOrderValueSpec = (parseFloat(POItm_NetOrderValueBase) / ConvRate);
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
        currentRow.find("#TxtNetValueInBase").val((parseFloat(POItm_NetOrderValueBase)).toFixed(ValDecDigit));

        var Txtitem_Net_amtPrev = currentRow.next().find("#TxtNetValueInBasePrev").val();
        var DiffNetAmt = parseFloat(POItm_NetOrderValueBase) - parseFloat(CheckNullNumber(Txtitem_Net_amtPrev));
        currentRow.find("#TxtNetDiffValueInBase").val(parseFloat(DiffNetAmt).toFixed(ValDecDigit));

        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
            OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
        }
        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
        AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherChargeTP").val() != null && currentRow.find("#TxtOtherChargeTP").val() != "") {
            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherChargeTP").val()).toFixed(ValDecDigit);
        }

        var Tax_Amt_nonRec = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtItemTaxNonRec").val() != null && currentRow.find("#TxtItemTaxNonRec").val() != "") {
            Tax_Amt_nonRec = parseFloat(currentRow.find("#TxtItemTaxNonRec").val()).toFixed(ValDecDigit);
        }

        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(POItm_TaxAmt) + parseFloat(OC_Amt) + parseFloat(OC_Amt_OR)).toFixed(ValDecDigit);
        currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
        let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(Tax_Amt_nonRec));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(Qty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));

        var val = parseFloat(currentRow.next().find("#TxtNetLandedPrev").val());
        var LandedCostPrev = isNaN(val) ? (0).toFixed(LddCostDecDigit) : val.toFixed(LddCostDecDigit);
        var LandedCostDiff = parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit) - parseFloat(CheckNullNumber(LandedCostPrev)).toFixed(LddCostDecDigit);
        currentRow.find("#TxtLandedCostPerPcDiffValue").val(parseFloat(CheckNullNumber(LandedCostDiff)).toFixed(LddCostDecDigit));
    });
    CalculateVoucherTotalAmount();
    CalculateAmountCoreLogic("OC"); FinalizeCalculation();
    OnClickTDSCalculationBtn();
};
function Calculate_OC_AmountItemWiseTP(TotalOCAmt) {
    var TotalGAmt = $("#TxtGrossValueInBase").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    // $("#TxtDocSuppOtherChargesTP").val(TotalOCAmt);
    const rows = $("#POInvItmDetailsTbl > tbody > tr").closest('tr');
    let TotalOCAmountTP = 0;
    const totalOCAmtFloatTP = parseFloat(TotalOCAmt);

    // Calculate and set OtherChargeTP, sum total in one pass
    rows.each(function () {
        const currentRow = $(this);
        const nextRow = currentRow.next();
        const grossValue = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()) || 0;
        let otherChargeTP = 0;
        const grossValuePrev = parseFloat(nextRow.find("#TxtItemGrossValueInBasePrev").val()) || 0;
        const OCAmtItemWisePrev = parseFloat(nextRow.find("#TxtOtherChargeTPPrev").val()) || 0;

        if (grossValue > 0 && parseFloat(TotalGAmt) > 0 && totalOCAmtFloatTP > 0) {
            const OCPercentageTP = ((grossValue / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            var OCAmtItemWiseTP = ((parseFloat(OCPercentageTP) * parseFloat(totalOCAmtFloatTP)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWiseTP) != 0) {
                currentRow.find("#TxtOtherChargeTP").val(parseFloat(OCAmtItemWiseTP).toFixed(ValDecDigit));
                otherChargeTP = parseFloat(OCAmtItemWiseTP).toFixed(ValDecDigit);
            }
            else {
                otherChargeTP = parseFloat(0).toFixed(ValDecDigit);
                currentRow.find("#TxtOtherChargeTP").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        else {
            if (totalOCAmtFloatTP == 0) {
                currentRow.find("#TxtOtherChargeTP").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        //currentRow.find("#TxtOtherChargeTP").val(otherChargeTP.toFixed(ValDecDigit));
        TotalOCAmountTP += parseFloat(otherChargeTP);
    });

    // Fix floating point and round
    TotalOCAmountTP = parseFloat(TotalOCAmountTP.toFixed(ValDecDigit));

    // Adjust first row if there's difference
    if (TotalOCAmountTP !== totalOCAmtFloatTP) {
        console.log('1');
        const AmtDifference = totalOCAmtFloatTP - TotalOCAmountTP;
        const firstRow = rows.first();
        const firstRowVal = parseFloat(firstRow.find("#TxtOtherChargeTP").val()) || 0;
        firstRow.find("#TxtOtherChargeTP").val((firstRowVal + AmtDifference).toFixed(ValDecDigit));
    }

    $("#POInvItmDetailsTbl >tbody >tr").closest('tr').each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = currentRow.find("#TxtItemGrossValueInBase").val() || 0.00;
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        var POItm_OCAmtTP = currentRow.find("#TxtOtherChargeTP").val();
        let Qty = currentRow.find("#TxtReceivedQuantity").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (POItm_OCAmtTP == null || POItm_OCAmtTP == "") {
            POItm_OCAmtTP = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
        }

        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt) + parseFloat(POItm_OCAmtTP));
        // console.log("POItm_NetOrderValueBase: " + POItm_NetOrderValueBase);
        var POItm_NetOrderValueSpec = (parseFloat(POItm_NetOrderValueBase) / ConvRate);
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
        currentRow.find("#TxtNetValueInBase").val(parseFloat(POItm_NetOrderValueBase).toFixed(ValDecDigit));

        var Txtitem_Net_amtPrev = currentRow.next().find("#TxtNetValueInBasePrev").val();
        var DiffNetAmt = parseFloat(POItm_NetOrderValueBase) - parseFloat(CheckNullNumber(Txtitem_Net_amtPrev));
        currentRow.find("#TxtNetDiffValueInBase").val(parseFloat(DiffNetAmt).toFixed(ValDecDigit));
        //currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));

        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
            OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
        }
        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
        AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);

        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtOtherChargeTP").val() != null && currentRow.find("#TxtOtherChargeTP").val() != "") {
            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherChargeTP").val()).toFixed(ValDecDigit);
        }

        var Tax_Amt_nonRec = (parseFloat(0)).toFixed(ValDecDigit);
        if (currentRow.find("#TxtItemTaxNonRec").val() != null && currentRow.find("#TxtItemTaxNonRec").val() != "") {
            Tax_Amt_nonRec = parseFloat(currentRow.find("#TxtItemTaxNonRec").val()).toFixed(ValDecDigit);
        }

        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(POItm_TaxAmt) + parseFloat(OC_Amt) + parseFloat(OC_Amt_OR)).toFixed(ValDecDigit);
        currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);

        //let LandedCostValue = parseFloat(CheckNullNumber(NetOrderValueSpec));
        let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(Tax_Amt_nonRec));
        let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(Qty));
        currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
        currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));

        var val = parseFloat(currentRow.next().find("#TxtNetLandedPrev").val());
        var LandedCostPrev = isNaN(val) ? (0).toFixed(LddCostDecDigit) : val.toFixed(LddCostDecDigit);
        var LandedCostDiff = parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit) - parseFloat(CheckNullNumber(LandedCostPrev)).toFixed(LddCostDecDigit);
        currentRow.find("#TxtLandedCostPerPcDiffValue").val(parseFloat(CheckNullNumber(LandedCostDiff)).toFixed(LddCostDecDigit));
    });
    CalculateVoucherTotalAmount();
    CalculateAmountCoreLogic("OC"); FinalizeCalculation();
    OnClickTDSCalculationBtn();
};
function CalculateAmountCoreLogic(Status) {
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue_recov = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue_nonrecov = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    let oc_amount = parseFloat(0).toFixed(ValDecDigit);
    let oc_amount_tp = parseFloat(0).toFixed(ValDecDigit);
    let oc_amount_revised_self = parseFloat(0).toFixed(ValDecDigit);

    $("#POInvItmDetailsTbl > tbody > tr").each(function (index) {
        if (index % 2 === 0) {
            var currentRow = $(this);
            if (index + 1 < $("#POInvItmDetailsTbl > tbody > tr").length) {
                var nextRow = currentRow.next();
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
                    if (GrossValueInBase == "" || GrossValueInBase == null || GrossValueInBase == "NaN") {
                        GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
                    }
                    GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(currentRow.find("#TxtItemGrossValueInBase").val())).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TxtItemTaxNonRec").val() == "" || currentRow.find("#TxtItemTaxNonRec").val() == "0" || currentRow.find("#TxtItemTaxNonRec").val() == "NaN") {
                    TaxValue_nonrecov = (parseFloat(TaxValue_nonrecov) + parseFloat(0)).toFixed(ValDecDigit);
                }
                else {
                    TaxValue_nonrecov = (parseFloat(TaxValue_nonrecov) + parseFloat(currentRow.find("#TxtItemTaxNonRec").val())).toFixed(ValDecDigit);
                }

                if (currentRow.find("#TxtItemTaxRec").val() == "" || currentRow.find("#TxtItemTaxRec").val() == "0" || currentRow.find("#TxtItemTaxRec").val() == "NaN") {
                    TaxValue_recov = (parseFloat(TaxValue_recov) + parseFloat(0)).toFixed(ValDecDigit);
                }
                else {
                    TaxValue_recov = (parseFloat(TaxValue_recov) + parseFloat(currentRow.find("#TxtItemTaxRec").val())).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TxtNetValue").val() == "" || currentRow.find("#TxtNetValue").val() == null || currentRow.find("#TxtNetValue").val() == "NaN") {
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

                var Txtitem_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
                var Txtitem_tax_amtPrev = currentRow.next().find("#Txtitem_tax_amtPrev").val();
                var DiffTaxAmt = parseFloat(Txtitem_tax_amt) - parseFloat(CheckNullNumber(Txtitem_tax_amtPrev));
                currentRow.find("#TxtItemTaxDiffValue").val(parseFloat(DiffTaxAmt).toFixed(ValDecDigit));

                var Txtitem_tax_amt_rec = currentRow.find("#TxtItemTaxRec").val();
                var Txtitem_tax_amtPrev_rec = currentRow.next().find("#TxtItemTaxRecPrev").val();
                var DiffTaxAmt_rec = parseFloat(Txtitem_tax_amt_rec) - parseFloat(CheckNullNumber(Txtitem_tax_amtPrev_rec));
                currentRow.find("#TxtItemTaxRecDiff").val(parseFloat(DiffTaxAmt_rec).toFixed(ValDecDigit));

                var Txtitem_tax_amt_nonrec = currentRow.find("#TxtItemTaxNonRec").val();
                var Txtitem_tax_amtPrev_nonrec = currentRow.next().find("#TxtItemTaxNonRecPrev").val();
                var DiffTaxAmt_nonrec = parseFloat(Txtitem_tax_amt_nonrec) - parseFloat(CheckNullNumber(Txtitem_tax_amtPrev_nonrec));
                currentRow.find("#TxtItemTaxNonRecDiff").val(parseFloat(DiffTaxAmt_nonrec).toFixed(ValDecDigit));
                //debugger;
                var Txtitem_OC_amt = currentRow.find("#TxtOtherCharge").val();
                var Txtitem_OC_amt1 = currentRow.find("#TxtOtherChargeDiffValue").val();
                const ocAmtDiffPrev = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherChargeDiffValue").val()));
                // console.log('TxtOtherChargeDiffValue ' + Txtitem_OC_amt1);
                var Txtitem_OC_amtPrev = currentRow.next().find("#TxtOtherChargePrev").val();
                var DiffOCAmt = 0;
                let newDiff = Txtitem_OC_amt - Txtitem_OC_amtPrev;
                //if (Status === "OC" && newDiff !== ocAmtDiffPrev) {
                //    newDiff = ocAmtDiffPrev + newDiff;
                //}

                if (ocAmtDiffPrev.toFixed(ValDecDigit) !== newDiff.toFixed(ValDecDigit)) {
                    currentRow.find("#TxtOtherChargeDiffValue").val(parseFloat(newDiff).toFixed(ValDecDigit));
                }
                else {
                    currentRow.find("#TxtOtherChargeDiffValue").val(parseFloat(newDiff).toFixed(ValDecDigit));
                }
                //// Only update if value has changed or was empty before
                //if (ocAmtDiffPrev != 0) {
                //    if (ocAmtDiffPrev.toFixed(ValDecDigit) !== newDiff.toFixed(ValDecDigit)) {
                //        currentRow.find("#TxtOtherChargeDiffValue").val(parseFloat(newDiff).toFixed(ValDecDigit));
                //    }
                //    else {
                //        currentRow.find("#TxtOtherChargeDiffValue").val(parseFloat(newDiff).toFixed(ValDecDigit));
                //    }
                //}
                //if (Status === "OC") {
                //    DiffOCAmt = parseFloat(Txtitem_OC_amt) - parseFloat(CheckNullNumber(Txtitem_OC_amtPrev));
                //    console.log('DiffOCAmtDiff ' + DiffOCAmt);
                //    if (parseFloat(DiffOCAmt) != parseFloat(Txtitem_OC_amt1)) {
                //        DiffOCAmt = parseFloat(Txtitem_OC_amt1) + parseFloat(CheckNullNumber(DiffOCAmt));
                //        console.log('DiffOCAmtFinal ' + DiffOCAmt);
                //    }

                //}
                //else {
                //    DiffOCAmt = parseFloat(Txtitem_OC_amt) - parseFloat(CheckNullNumber(Txtitem_OC_amtPrev));
                //}

                //var DiffOCAmtFinal = parseFloat(Txtitem_OC_amt1) + parseFloat(CheckNullNumber(DiffOCAmt));

                // currentRow.find("#TxtOtherChargeDiffValue").val(parseFloat(DiffOCAmt).toFixed(ValDecDigit));
                var Txtitem_OC_amt_TP = currentRow.find("#TxtOtherChargeTP").val();
                var Txtitem_OC_amtPrev_TP = currentRow.next().find("#TxtOtherChargeTPPrev").val();
                var DiffOCAmt_TP = parseFloat(Txtitem_OC_amt_TP) - parseFloat(CheckNullNumber(Txtitem_OC_amtPrev_TP));
                currentRow.find("#TxtOtherChargeTPDiff").val(parseFloat(DiffOCAmt_TP).toFixed(ValDecDigit));

                if (currentRow.find("#TxtOtherChargeDiffValue").val() == "" || currentRow.find("#TxtOtherChargeDiffValue").val() == null || currentRow.find("#TxtOtherChargeDiffValue").val() == "NaN") {
                    oc_amount_revised_self = (parseFloat(oc_amount_revised_self) + parseFloat(0)).toFixed(ValDecDigit);
                }
                else {
                    oc_amount_revised_self = (parseFloat(oc_amount_revised_self) + parseFloat(currentRow.find("#TxtOtherChargeDiffValue").val())).toFixed(ValDecDigit);
                }

                if (currentRow.find("#TxtOtherChargeTP").val() == "" || currentRow.find("#TxtOtherChargeTP").val() == null || currentRow.find("#TxtOtherChargeTP").val() == "NaN") {
                    oc_amount_tp = (parseFloat(oc_amount_tp) + parseFloat(0)).toFixed(ValDecDigit);
                }
                else {
                    oc_amount_tp = (parseFloat(oc_amount_tp) + parseFloat(currentRow.find("#TxtOtherChargeTP").val())).toFixed(ValDecDigit);
                }
                currentRow = nextRow.next();
            }
        }
    });
    oc_amount = CheckNullNumber($("#TxtDocSuppOtherCharges").val()) || 0.00;
    //oc_amount = CheckNullNumber($("#TxtOtherCharges").val()) || 0.00;
    //CheckNullNumber($("#TxtDocSuppOtherCharges").val()) || 0.00;// parseFloat($("#TxtOtherCharges").val()) || 0.00;
    $("#TxtDocSuppOtherChargesRevised_Self").val(oc_amount_revised_self);
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtGrossValueInBase").val(GrossValueInBase);
    TaxValue_recov = parseFloat(TaxValue_recov) || 0.00;
    TaxValue_nonrecov = parseFloat(TaxValue_nonrecov) || 0.00;
    TaxValue = (parseFloat(TaxValue_recov) + parseFloat(TaxValue_nonrecov)) || 0.00;

    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValueInBase) + parseFloat(CheckNullNumber(oc_amount));
    NetOrderValueSpec = NetOrderValueBase / parseFloat(CheckNullNumber(ConvRate));

    $("#TaxAmountRecoverable").val((parseFloat(TaxValue_recov) || 0).toFixed(ValDecDigit));
    $("#TaxAmountRecoverable").val((parseFloat(TaxValue_recov) || 0).toFixed(ValDecDigit));
    $("#TaxAmountNonRecoverable").val((parseFloat(TaxValue_nonrecov) || 0).toFixed(ValDecDigit));
    $("#NetOrderValueSpe").val(NetOrderValueSpec.toFixed(ValDecDigit));
    $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(ValDecDigit));
    $("#hdn_NetOrderValueInBase").val(NetOrderValueBase.toFixed(ValDecDigit));
    CalculateTDS_RevisedAmt();
}
function FinalizeCalculation() {
    DifferenceSummary();
    if ($("#chk_roundoff").is(":checked")) {
        click_chkplusminus();
    }
    else {
        GetAllGLID();
    }
}
let isMainCalculateAmountRunning = false;
function CalculateAmount(status) {
    const isReentrantCall = isMainCalculateAmountRunning;
    if (!isReentrantCall) {
        isMainCalculateAmountRunning = true;
    }
    CalculateAmountCoreLogic();
    //if ($("#chk_roundoff").is(":checked")) {
    //    var roundoff_netval = Math.round(NetOrderValueBase);
    //    var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
    //    $("#NetOrderValueInBase").val(f_netval);
    //}
    //else {
    //    $("#NetOrderValueInBase").val(NetOrderValueBase.toFixed(ValDecDigit));
    //}

    //var oc_amount = CheckNullNumber($("#TxtDocSuppOtherCharges").val());
    //var oc_amount_tp = CheckNullNumber($("#TxtDocSuppOtherChargesTP").val());

    // Initialize totals
    let oc_amount = 0;
    let oc_amount_tp = 0;

    // Get all the rows from the table
    let rows = document.querySelectorAll("#Tbl_OC_Deatils tbody tr");
    rows.forEach(row => {
        let suppId = row.querySelector("#td_OCSuppID") ? row.querySelector("#td_OCSuppID").textContent.trim() : "0";
        let ocAmount = parseFloat(row.querySelector("#OCTotalTaxAmt") ? row.querySelector("#OCTotalTaxAmt").textContent.trim() : "0");
        if (suppId !== "0") {
            oc_amount_tp += ocAmount;
        } else {
            oc_amount += ocAmount;
        }
    });
    // console.log('oc_amount_tp ' + oc_amount_tp);

    if (status) {
        Calculate_OC_AmountItemWise(oc_amount, "CalculateAmount");
        Calculate_OC_AmountItemWiseTP(oc_amount_tp, "CalculateAmount");
    }
    else {
        Calculate_OC_AmountItemWiseWithNoChange(oc_amount);
        Calculate_OC_AmountItemWiseTPWithNoChange(oc_amount_tp);
    }
    FinalizeCalculation();
}

function CalculateAmountPrev() {
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var GrossValueInBase = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue_recov = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue_nonrecov = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    $("#POInvItmDetailsTbl > tbody > tr").each(function (index) {
        if (index % 2 !== 0) {
            var currentRow = $(this);
            var nextRow = currentRow.next();
            if (currentRow.find("#TxtItemGrossValuePrev").val() == "" || currentRow.find("#TxtItemGrossValuePrev").val() == null || currentRow.find("#TxtItemGrossValuePrev").val() == "NaN") {
                GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValuePrev").val())).toFixed(ValDecDigit);
            }
            if (currentRow.find("#TxtItemGrossValueInBasePrev").val() == "" || currentRow.find("#TxtItemGrossValueInBasePrev").val() == null || currentRow.find("#TxtItemGrossValuePrev").val() == "NaN") {
                GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                GrossValueInBase = (parseFloat(GrossValueInBase) + parseFloat(currentRow.find("#TxtItemGrossValueInBasePrev").val())).toFixed(ValDecDigit);
            }

            if (currentRow.find("#TxtNetValuePrev").val() == "" || currentRow.find("#TxtNetValuePrev").val() == null || currentRow.find("#TxtNetValuePrev").val() == "NaN") {
                NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#TxtNetValuePrev").val())).toFixed(ValDecDigit);
            }
            if (currentRow.find("#TxtNetValueInBasePrev").val() == "" || currentRow.find("#TxtNetValueInBasePrev").val() == null || currentRow.find("#TxtNetValueInBasePrev").val() == "NaN") {
                NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValueInBasePrev").val())).toFixed(ValDecDigit);
            }
            if (currentRow.find("#TxtItemTaxNonRecPrev").val() == "" || currentRow.find("#TxtItemTaxNonRecPrev").val() == "0" || currentRow.find("#TxtItemTaxNonRecPrev").val() == "NaN") {
                TaxValue_nonrecov = (parseFloat(TaxValue_nonrecov) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                TaxValue_nonrecov = (parseFloat(TaxValue_nonrecov) + parseFloat(currentRow.find("#TxtItemTaxNonRecPrev").val())).toFixed(ValDecDigit);
            }

            if (currentRow.find("#TxtItemTaxRecPrev").val() == "" || currentRow.find("#TxtItemTaxRecPrev").val() == "0" || currentRow.find("#TxtItemTaxRecPrev").val() == "NaN") {
                TaxValue_recov = (parseFloat(TaxValue_recov) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                TaxValue_recov = (parseFloat(TaxValue_recov) + parseFloat(currentRow.find("#TxtItemTaxRecPrev").val())).toFixed(ValDecDigit);
            }
            currentRow = nextRow.next();
        }
    });

    // let oc_amount = parseFloat($("#TxtDocSuppOtherChargesPrev").val()) || 0.00;
    let oc_amount = parseFloat($("#TxtDocSuppOtherChargesPrev").val()) || 0.00;
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtGrossValueInBase").val(GrossValue);
    $("#TxtGrossValuePrev").val(GrossValue);
    TaxValue_recov = parseFloat(TaxValue_recov).toFixed(ValDecDigit) || 0.00;
    TaxValue_nonrecov = parseFloat(TaxValue_nonrecov).toFixed(ValDecDigit) || 0.00;
    TaxValue = (parseFloat(TaxValue_recov) + parseFloat(TaxValue_nonrecov)).toFixed(ValDecDigit) || 0.00;
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValueInBase) + parseFloat(CheckNullNumber(oc_amount));
    NetOrderValueSpec = NetOrderValueBase / parseFloat(CheckNullNumber(ConvRate));
    NetOrderValueBase = parseFloat(NetOrderValueBase).toFixed(ValDecDigit) || 0.00;

    $("#TxtGrossValueInBasePrev").val(GrossValueInBase);
    $("#NetOrderValueSpePrev").val(NetOrderValueSpec);
    $("#TxtGrossValueInBase").val(GrossValueInBase);
    $("#TaxAmountRecoverable").val(TaxValue_recov);
    $("#TaxAmountNonRecoverable").val(TaxValue_nonrecov);
    $("#TaxAmountRecoverablePrev").val(TaxValue_recov);
    $("#TaxAmountNonRecoverablePrev").val(TaxValue_nonrecov);
    $("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBasePrev").val(NetOrderValueBase);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
    $("#hdn_NetOrderValueInBase").val(NetOrderValueBase);
    //$("#TxtDocSuppOtherCharges").val(oc_amount);
    //if ($("#chk_roundoff").is(":checked")) {
    //    var roundoff_netval = Math.round(NetOrderValueBase);
    //    var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
    //    $("#NetOrderValueInBasePrev").val(f_netval);
    //    $("#NetOrderValueInBase").val(f_netval);
    //    $("#hdn_NetOrderValueInBase").val(f_netval);
    //}
    //else {
    //    $("#NetOrderValueInBasePrev").val(NetOrderValueBase);
    //    $("#NetOrderValueInBase").val(NetOrderValueBase);
    //    $("#hdn_NetOrderValueInBase").val(NetOrderValueBase);
    //}
    DifferenceSummary();
}

function DifferenceSummary() {
    //debugger;
    //Total Value (In Specific)
    let GrossValuePrevAmt = parseFloat($("#TxtGrossValuePrev").val()) || 0.00;
    let currentGrossValue = parseFloat($("#TxtGrossValue").val()) || 0.00;
    let GrossValueDiff = (currentGrossValue - GrossValuePrevAmt).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_TotalAmountInSpec").val(GrossValueDiff);

    //Total Value (In Base)
    let GrossValueInBasePrev = parseFloat($("#TxtGrossValueInBasePrev").val()) || 0.00;
    let GrossValueInBase = parseFloat($("#TxtGrossValueInBase").val()) || 0.00;
    let GrossValueDiffInBase = (GrossValueInBase - GrossValueInBasePrev).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_TotalAmountInBase").val(GrossValueDiffInBase);

    //Total Tax Amount
    let TaxValuePrev_Recoverable = parseFloat($("#TaxAmountRecoverablePrev").val()) || 0.00;
    let TaxValue_Recoverable = parseFloat($("#TaxAmountRecoverable").val()) || 0.00;
    let TaxAmountDiff_Recoverable = (TaxValue_Recoverable - TaxValuePrev_Recoverable).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_TotalTaxAmountRec").val(TaxAmountDiff_Recoverable);

    //Total Tax Amount
    let TaxValuePrev = parseFloat($("#TaxAmountNonRecoverablePrev").val()) || 0.00;
    let TaxValue = parseFloat($("#TaxAmountNonRecoverable").val()) || 0.00;
    let TaxAmountDiff = (TaxValue - TaxValuePrev).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_TotalTaxAmountNonRec").val(TaxAmountDiff);

    //Other Charges ( with Third Party )
    let oc_amount = parseFloat($("#TxtOtherCharges").val()) || 0.00;
    let oc_amountPrev = parseFloat($("#TxtOtherChargesPrev").val()) || 0.00;
    var Diffoc_amount = (oc_amount - oc_amountPrev).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_TotalOCAmount").val(Diffoc_amount);

    //Total Net Amount (In Base)
    var NetOrderValueInBasePrevAmt = $("#NetOrderValueInBasePrev").val() || 0.00;
    var NetOrderValueBase = $("#NetOrderValueInBase").val() || 0.00;
    var DiffNetOrderValueInBas = (NetOrderValueBase - NetOrderValueInBasePrevAmt).toFixed(ValDecDigit) || "0.00";
    $("#TxtDiff_Amount").val(DiffNetOrderValueInBas);
    $("#hdn_TxtDiff_Amount").val(DiffNetOrderValueInBas);

    //TDS
    let TDSPrev = parseFloat($("#TxtTDS_AmountPrev").val()) || 0.00;
    let TDS = parseFloat($("#TxtTDS_Amount").val()) || 0.00;
    let TDSDiff = 0.00;
    if (TDS > 0) {
        TDSDiff = (TDS - TDSPrev).toFixed(ValDecDigit) || "0.00";
    } else {
        TDSDiff = (0).toFixed(ValDecDigit) || "0.00";
    }
    $("#TxtDiff_TotalTDSAmount").val(TDSDiff);
}

function SetOtherChargeVal() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
    })
}
function BindOtherChargeDeatils(val) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
    $("#_OtherChargeTotal").text(parseFloat(0).toFixed(ValDecDigit));
    $("#_OtherChargeTotalTax").text(parseFloat(0).toFixed(ValDecDigit));
    $("#_OtherChargeTotalAmt").text(parseFloat(0).toFixed(ValDecDigit));

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var td = "";
            var OCFor = currentRow.find("#OCFor").text();
            var OCForText = (OCFor.includes("P") ? "P" : (OCFor.includes("R") ? "R" : null));

            let ocAmt = currentRow.find("#OcAmtBs").text();
            if (OCForText == "R") {
                $('#Tbl_OtherChargeList tbody').append(`<tr>
                <td >${currentRow.find("#OCName").text()}</td>
                <td >${currentRow.find("#td_OCSuppName").text()}</td>
                <td id="OCAmtSp1" align="right">${ocAmt}</td>
                <td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
                <td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
                <td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>
            </tr>`);

                TotalAMount = (parseFloat(TotalAMount) + parseFloat(ocAmt)).toFixed(ValDecDigit);
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);

            }
            else {


            }
        });
        if (val == "") {
            GetAllGLID();
        }
    }
    $("#_OtherChargeTotal").text(TotalAMount);
    $("#_OtherChargeTotalTax").text(TotalTaxAMount);
    $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    GetAllGLID();
}
function OnClickOCTaxCalculationBtn(e, OCFor) {
    //debugger;

    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    if (OCFor == "P") {
        SNohiddenfiled = "OCP";
    }
    else if (OCFor == "R") {
        SNohiddenfiled = "OCR";
    }
    //var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//
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
    //debugger;
    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DPIDate = $("#SuppPIInv_date").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: DPIDate
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var INVStatus = "";
                    INVStatus = $('#hfPIStatus').val().trim();
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
                else {
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
    //debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var INV_NO = "";
    var INVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var mailerror = "";
    var VoucherNarr = $("#hdn_Nurration").val()
    var BP_VoucherNarr = $("#hdn_BP_Nurration").val()
    var DN_VoucherNarr = $("#hdn_DN_Nurration").val()
    var CN_VoucherNarr = $("#hdn_CN_Nurration").val()

    docid = $("#DocumentMenuId").val();
    INV_NO = $("#InvoiceNumber").val();
    INVDate = $("#SuppPIInv_date").val();
    WF_Status1 = $("#WF_Status1").val();
    var TrancType = (docid + ',' + INV_NO + ',' + INVDate + ',' + WF_Status1)
    $("#hdDoc_No").val(INV_NO);
    Remarks = $("#fw_remarks").val();
    var FilterData = $("#FilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    ///var pdfAlertEmailFilePath = 'SI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    // await GetPdfFilePathToSendonEmailAlert(INV_NO, INVDate, docid, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "" && level != "") {

            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, level, forwardedto, fwchkval, Remarks, "");
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/ApproveSuppPIDetails?Inv_No=" + INV_NO + "&Inv_Date=" + INVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&VoucherNarr=" + VoucherNarr + "&FilterData=" + FilterData + "&docid=" + docid + "&WF_Status1=" + WF_Status1 + "&Bp_Nurr=" + BP_VoucherNarr + "&Dn_Nurration=" + DN_VoucherNarr + "&Cn_Nurration=" + CN_VoucherNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, "")
            mailerror = $("#MailError").val();;
            window.location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        ////debugger
        if (fwchkval != "" && INV_NO != "" && INVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(INV_NO, INVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, "")
            mailerror = $("#MailError").val();;
            window.location.href = "/ApplicationLayer/SupplementaryPurchaseInvoice/ToRefreshByJS?FilterData=" + FilterData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//Added by Nidhi 12-06-2025
function GetPdfFilePathToSendonEmailAlert(INV_NO, INV_Date, docid, fileName) {
    //debugger;
    var Inv_no = INV_NO;
    var Inv_Date = INV_Date;
    var docid = docid;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SupplementaryPurchaseInvoice/SavePdfDocToSendOnEmailAlert",
        data: { docid: docid, Inv_no: INV_NO, Inv_Date: INV_Date, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    //debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        var Doc_Status = $("#hfPIStatus").val();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    Cmn_GetForwarderHistoryList(Doc_No, Doc_ID, Doc_Status);
    return false;
}
function OnClickbillingAddressIconBtn(e) {
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfPIStatus").val().trim();
    var PODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        PODTransType = "Update";
    }
    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, PODTransType);
}

//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
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
/***--------------------------------Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNo").text();
    var ProductNm = clickdRow.find("#TxtItemName").val();
    /*var ProductId = clickdRow.find("#hfItemID_SubItems").val();*/
    var ProductId = clickdRow.find("#hfItemID_SubItems").val();
    if (ProductId.endsWith("R")) {
        ProductId = ProductId.slice(0, -1);
    }
    var GrnNo = clickdRow.find("#TxtGrnNo").val();
    var GRNDate = clickdRow.find("#hfGRNDate").val();
    var UOM = clickdRow.find("#TxtUOM").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var IsDisabled = "Y";//$("#DisableSubItem").val();
    var Sub_Quantity = 0;
    if (flag == "Quantity") {
        Sub_Quantity = clickdRow.find("#TxtReceivedQuantity").val();
    }
    else {
        ProductNm = $("#GrnItemName").val();
        UOM = $("#UOM").val();
        Doc_no = $("#GRNNumber").val();
        Doc_dt = $("#GRN_Date_hdn").val();
        ProductId = $("#GrnItemId").val();
    }
    if (flag == "BillQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdBill_qty").text();
    }
    if (flag == "RecdQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRec_qty").text();
    }
    if (flag == "AccQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdAcc_qty").text();
    }
    if (flag == "RejQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRej_qty").text();
    }
    if (flag == "RewQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdRew_qty").text();
    }
    if (flag == "ShortQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdShort_qty").text();
    }
    if (flag == "SampleQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdSample_qty").text();
    }
    if (flag == "InvQuantity") {
        Sub_Quantity = clickdRow.find("#V_tdInv_qty").text();
    }
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").find("#subItemSrcDocNo[value='" + GrnNo + "']").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.sub_item_name = row.find('#subItemName').val();
        List.qty = row.find('#subItemQty').val();
        List.src_doc_no = row.find('#subItemSrcDocNo').val();
        List.src_doc_dt = row.find('#subItemSrcDocDate').val();
        NewArr.push(List);
    });
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SupplementaryPurchaseInvoice/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt
        },
        success: function (data) {
            // ////debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
            if (flag == "Quantity") {
                $("#SubItemPopUp").css("max-width", "600px");
            } else {
                $("#SubItemPopUp").css("max-width", "1100px");
            }
        }
    });
}

/***--------------------------------Sub Item Section End-----------------------------------------***/
function approveonclick() {

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
/****======------------Reason For Reject And Rework section Start-------------======****/

function onClickReasonRemarks(e, flag) {
    let Row = $(e.target).closest('tr');
    let ItemName = $("#GrnItemName").val();
    let ItemId = $("#GrnItemId").val();
    let UOM = $("#UOM").val();
    let ReasonRemarks = "";

    $("#RR_Remarks").attr("readonly", true);
    $("#RR_btnClose").attr("hidden", false);
    $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    if (flag == "reject") {
        ReasonRemarks = Row.find("#ReasonForReject").text();
        let rejQty = Row.find("#V_tdRej_qty").text();
        if (parseFloat(CheckNullNumber(rejQty)) > 0) {
            $("#RR_Quantity").val(rejQty);
        } else {
            Row.find("#ReasonForReject").text("");
            ReasonRemarks = "";
        }
    }
    else if (flag == "rework") {
        ReasonRemarks = Row.find("#ReasonForRework").text();

        let rwkQty = Row.find("#V_tdRew_qty").text();
        if (parseFloat(CheckNullNumber(rwkQty)) > 0) {
            $("#RR_Quantity").val(rwkQty);
        } else {
            Row.find("#ReasonForRework").text("");
            ReasonRemarks = "";
        }
    }
    $("#RR_ItemName").val(ItemName);
    $("#RR_ItemId").val(ItemId);
    $("#RR_Uom").val(UOM);
    $("#RR_Flag").val(flag);

    $("#RR_Remarks").val(ReasonRemarks);

}
/***-------------------Roundoff------------------------------------***/
function checkMultiSupplier() {
    return true;
}
function click_chkroundoff() {
    if (checkMultiSupplier() == true) {
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
            CalculateAmount(true);
            //$("#POInvItmDetailsTbl > tbody > tr").each(function () {
            //    var currentrow = $(this);
            //   CalculateTaxExemptedAmt(currentrow, "Row")
            //});
            // GetAllGLID();
        }
    } else {
        //for multi supplier
        if ($("#chk_roundoff").is(":checked")) {
            swal("", $("#ManualRoundOffIsNotApplicableForGLHavingMultipleSuppliers").text(), "warning")
            $("#chk_roundoff").attr("checked", false);
        }
    }

}
function click_chkplusminus(AutoAplyTaxPI) {

    if ($("#chk_roundoff").is(":checked")) {
        let Diffoc_amount = parseFloat(0);
        let DiffGrossValueInBase = parseFloat(0);
        let DiffTaxValue_recov = parseFloat(0);
        let DiffTaxValue_nonrecov = parseFloat(0);
        let DiffTaxValue = parseFloat(0);
        var DiffTotNetAmt = parseFloat(0);
        let Revisedoc_amount = parseFloat(0);
        let RevisedGrossValueInBase = parseFloat(0);
        let RevisedTaxValue_recov = parseFloat(0);
        let RevisedTaxValue_nonrecov = parseFloat(0);
        let RevisedTaxValue = parseFloat(0);
        let NetAmtPrev = parseFloat(0);
        //var RevisedTotNetAmt = parseFloat(NetAmtPrev) + parseFloat(RevisedTaxValue) + parseFloat(RevisedGrossValueInBase) + parseFloat(CheckNullNumber(Revisedoc_amount));
        var RevisedTotNetAmt = parseFloat(0);
        let RevisedTotNetAmtRO = 0.00;
        var DiffTotNetAmtRO = 0.00; let numericValue = 0;
        Diffoc_amount = CheckNullNumber($("#TxtDocSuppOtherChargesRevised_Self").val()) || 0.00;
        DiffGrossValueInBase = CheckNullNumber($("#TxtDiff_TotalAmountInBase").val()) || 0.00;
        DiffTaxValue_recov = CheckNullNumber($("#TxtDiff_TotalTaxAmountRec").val()) || 0.00;
        DiffTaxValue_nonrecov = CheckNullNumber($("#TxtDiff_TotalTaxAmountNonRec").val()) || 0.00;
        DiffTaxValue = (parseFloat(DiffTaxValue_recov) + parseFloat(DiffTaxValue_nonrecov)) || 0.00;
        DiffTotNetAmt = parseFloat(DiffTaxValue) + parseFloat(DiffGrossValueInBase) + parseFloat(CheckNullNumber(Diffoc_amount));
        if (DiffTotNetAmt != 0) {
            //Revisedoc_amount = CheckNullNumber($("#TxtDocSuppOtherCharges").val()) || 0.00;
            //RevisedGrossValueInBase = CheckNullNumber($("#TxtGrossValueInBase").val()) || 0.00;
            //RevisedTaxValue_recov = CheckNullNumber($("#TaxAmountRecoverable").val()) || 0.00;
            //RevisedTaxValue_nonrecov = CheckNullNumber($("#TaxAmountNonRecoverable").val()) || 0.00;
            //RevisedTaxValue = (parseFloat(RevisedTaxValue_recov) + parseFloat(RevisedTaxValue_nonrecov)) || 0.00;
            //NetAmtPrev = CheckNullNumber($("#NetOrderValueInBasePrev").val()) || 0.00;
            ////var RevisedTotNetAmt = parseFloat(NetAmtPrev) + parseFloat(RevisedTaxValue) + parseFloat(RevisedGrossValueInBase) + parseFloat(CheckNullNumber(Revisedoc_amount));
            //RevisedTotNetAmt = parseFloat(RevisedTaxValue) + parseFloat(RevisedGrossValueInBase) + parseFloat(CheckNullNumber(Revisedoc_amount));

            // var RevisedTotNetAmt = $("#hdn_NetOrderValueInBase").val();
            // var DiffTotNetAmt = $("#TxtDiff_Amount").val();
            //var DiffTotNetAmt = $("#hdn_TxtDiff_Amount").val();

            try {
                if ($("#p_round").is(":checked")) {
                    //RevisedTotNetAmtRO = SPI_RoundValue(RevisedTotNetAmt, "P");
                    DiffTotNetAmtRO = SPI_RoundValue(DiffTotNetAmt, "P");
                    $("#pm_flagval").val($("#p_round").val());
                }
                if ($("#m_round").is(":checked")) {
                    // RevisedTotNetAmtRO = SPI_RoundValue(RevisedTotNetAmt, "M");
                    DiffTotNetAmtRO = SPI_RoundValue(DiffTotNetAmt, "M");
                    $("#pm_flagval").val($("#m_round").val());
                }
            } catch (err) {
                console.log("Round off Error : " + err.message);
            }
            numericValue = parseFloat(RevisedTotNetAmtRO); let DiffnumericValue = parseFloat(DiffTotNetAmtRO);
            // $("#NetOrderValueInBase").val(numericValue.toFixed(ValDecDigit));
            $("#TxtDiff_Amount").val(DiffnumericValue.toFixed(ValDecDigit));
            //$("#hdn_TxtDiff_Amount").val(DiffnumericValue.toFixed(ValDecDigit));
        }
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);
        //if (AutoAplyTaxPI != "AutoAplyTaxPI") {
        //    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        //        var currentrow = $(this);
        //        CalculateTaxExemptedAmt(currentrow, "Row")
        //    });
        //}
    }
    GetAllGLID();
}
async function AutoTdsApply(SuppId, GrossAmt) {

    await $.ajax({
        type: "POST",
        url: "/Common/Common/Cmn_GetTdsDetails",
        data: { SuppId: SuppId, GrossVal: GrossAmt, tax_type: "TDS" },
        success: function (data) {
            //debugger;
            var arr = JSON.parse(data);
            let tds_amt = 0;
            if (arr.Table1 != null) {
                let checkResult = arr.Table1.length > 0 ? arr.Table1[0].result : "";
                // $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
                if (checkResult == "Invalid Slab") {
                    swal("", $("#InvailidTdsSlabFound").text(), "warning")
                } else {
                    var checkTdsAcc = "Y";
                    $('#Hdn_TDS_CalculatorTblRevised tbody tr').remove();
                    for (var i = 0; i < arr.Table1.length; i++) {
                        //let td_tds_amt = Math.round(arr.Table1[i].tds_amt);
                        let td_tds_amt = Cmn_RoundValue(arr.Table1[i].tds_amt, "P");
                        if (arr.Table1[i].tds_id == "") {
                            checkTdsAcc = "N";
                        }
                        if (checkTdsAcc == "Y") {
                            $('#Hdn_TDS_CalculatorTblRevised tbody').append(`<tr id="R${i + 1}">
                                <td id="td_TDS_Name">${arr.Table1[i].tds_name}</td>
                                <td id="td_TDS_NameID">${arr.Table1[i].tds_id}</td>
                                <td id="td_TDS_Percentage">${arr.Table1[i].tds_perc}</td>
                                <td id="td_TDS_Level">${arr.Table1[i].tds_level}</td>
                                <td id="td_TDS_ApplyOn">${arr.Table1[i].tds_apply_on}</td>
                                <td id="td_TDS_Amount">${td_tds_amt}</td>
                                <td id="td_TDS_ApplyOnID">${arr.Table1[i].tds_apply_on_id}</td>
                                <td id="td_TDS_BaseAmt">${arr.Table1[i].tds_bs_amt}</td>
                                <td id="td_TDS_AccId">${arr.Table1[i].tds_acc_id}</td>
                                <td id="td_TDS_RevisedAmt">0</td>
                                    </tr>`);
                            tds_amt += parseFloat(td_tds_amt);
                        }
                        //$("#TxtTDS_Amount").val((Math.ceil(tds_amt * Math.pow(10, ValDecDigit)) / Math.pow(10, ValDecDigit)).toFixed(ValDecDigit));

                        //   $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));

                    }

                    if (checkTdsAcc == "N") {
                        $('#Hdn_TDS_CalculatorTblRevised tbody tr').remove();
                        $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
                        swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
                    }
                }
            }

            GetAllGLID();

        }
    })
}
function SPI_RoundValue(value, round_type) {
    if (value == null || value == "") {
        value = 0;
    }
    var fnetval = value;
    if (round_type == null || round_type == "") {
        fnetval = parseFloat(value).toFixed(0);
    }
    else {
        var netval = parseFloat(value).toFixed(cmn_ValDecDigit);
        if (parseFloat(netval) > 0) {
            var finnetval = netval.split('.');
            if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                if (round_type == "P") {
                    var decval = '0.' + finnetval[1];
                    var faddval = 1 - parseFloat(decval);
                    fnetval = parseFloat(netval) + parseFloat(faddval);
                }
                if (round_type == "M") {
                    var decval = '0.' + finnetval[1];
                    fnetval = parseFloat(netval) - parseFloat(decval);
                }
            }
            else {
                fnetval = netval;
            }

        }
        else if (parseFloat(netval) < 0) {
            var finnetval = netval.split('.');
            if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                if (round_type == "P") {
                    var decval = '0.' + finnetval[1];
                    fnetval = parseFloat(netval) + parseFloat(decval);
                }
                if (round_type == "M") {
                    var decval = '0.' + finnetval[1];
                    var faddval = 1 - parseFloat(decval);
                    fnetval = parseFloat(netval) - parseFloat(faddval);
                }
            } else {
                fnetval = netval;
            }
        }
    }
    return fnetval
}
/***----------------------end-------------------------------------***/

/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/

function GetInvoiceTypeSupplier() {
    var SuppType = "";
    if ($("#Domestic").is(":checked")) {
        SuppType = "D";
    }
    if ($("#Import").is(":checked")) {
        SuppType = "I";
    }
    var data = {
        SuppType: SuppType,
    };
    // Send the request
    fetch("/SupplementaryPurchaseInvoice/GetSuppList_OrderType", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data) // Send the data as JSON
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
            bindSupplierDropdown(data);
            $("#Address").val("");
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
function bindSupplierDropdown(suppliers) {
    const supplierDropdown = document.getElementById('SupplierName');
    supplierDropdown.innerHTML = '';
    suppliers.forEach(supplier => {
        const option = document.createElement('option');
        option.value = supplier.ID;
        option.text = supplier.Name;
        supplierDropdown.appendChild(option);
    });
}
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
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SupplementaryPurchaseInvoice/SuppPIListSearch",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                $('#SuppPIList').html(data);
                $("#ListFilterData").val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status)
            },
            error: function OnError(xhr, errorType, exception) {
            }
        });
    } catch (err) {
        console.log("SuppPI Error : " + err.message);
    }
}
function DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    $(ItmDDLName + RowID).select2({
        ajax: {
            url: "/Common/Common/GetItemList3",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    PageName: PageName,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                var pageSize,
                    pageSize = 20;

                ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                var ItemListArrey = [];
                if (sessionStorage.getItem("selecteditemlist").length != null) {
                    ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                }
                let selected = [];
                selected.push({ id: $(ItmDDLName + RowID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemType").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---_0" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name.split("_")[0], UOM: val.ID.split("_")[1], type: val.Name.split("_")[1] };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}

function OnChangeConsumbleItemPrice(e) {
    //debugger;
    var currentrow = $(e.target).closest('tr');
    var SNo = currentrow.find("#hfSNo").val();
    var SNo_next = currentrow.next().find("#hfSNo").val();
    var ItmCode = currentrow.find("#hfItemID").val();
    let ExchangeRate = $("#conv_rate").val();
    //let ExchangeRate = $("#conv_rate_tb").val();
    var TxtGrnNo = currentrow.find("#TxtGrnNo").val();
    var TxtReceivedQuantity = currentrow.find("#TxtReceivedQuantity").val();
    var TxtRate = currentrow.find("#TxtRate" + SNo).val();

    var TxtInvValue = parseFloat(CheckNullNumber(TxtReceivedQuantity)) * parseFloat(CheckNullNumber(TxtRate));
    var PrevValue = currentrow.next().find("#TxtItemGrossValuePrev").val();
    var DiffAmt = parseFloat(TxtInvValue) - parseFloat(CheckNullNumber(PrevValue));
    var rowIndex = currentrow.index();// + 1;
    currentrow.find("#TxtRate" + SNo).val(parseFloat(CheckNullNumber(TxtRate)).toFixed(cmn_GrnRateDigit));
    rowIndex = currentrow.index() + 1;
    if ((parseFloat(CheckNullNumber(TxtRate)) <= 0) || (parseFloat(CheckNullNumber(TxtRate)) == 0)) {
        currentrow.find("#TxtRate" + SNo).css("border-color", "red");
        currentrow.find("#TxtRate_Error" + SNo).css("display", "block");
        currentrow.find("#TxtRate_Error" + SNo).text($("#valueReq").text());
        return false;
    } else {
        currentrow.find("#TxtRate" + SNo).css("border-color", "#ced4da");
        currentrow.find("#TxtRate_Error" + SNo).css("display", "none");
        currentrow.find("#TxtRate_Error" + SNo).text("");
    }

    var TxtRatePrev = currentrow.next().find("#TxtRatePrev" + SNo_next).val();
    if ((parseFloat(CheckNullNumber(TxtRate)) < TxtRatePrev)) {
        currentrow.find("#TxtRate" + SNo).css("border-color", "red");
        currentrow.find("#TxtRate_Error" + SNo).css("display", "block");
        currentrow.find("#TxtRate_Error" + SNo).text($("#span_InvalidPrice").text());
        return false;
    } else {
        currentrow.find("#TxtRate" + SNo).css("border-color", "#ced4da");
        currentrow.find("#TxtRate_Error" + SNo).css("display", "none");
        currentrow.find("#TxtRate_Error" + SNo).text("");
    }

    currentrow.find("#TxtItemGrossValue").val(parseFloat(TxtInvValue).toFixed(ValDecDigit));
    currentrow.find("#TxtItemGrossValueInBase").val(parseFloat(parseFloat(TxtInvValue) * parseFloat(ExchangeRate)).toFixed(ValDecDigit));
    currentrow.find("#TxtItemDiffValue").val(parseFloat(DiffAmt).toFixed(ValDecDigit));

    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (currentrow.find("#ManualGST").is(":checked")) {
            CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, currentrow.find("#TxtItemGrossValueInBase").val());
        }
    }
    //debugger;
    var CheckAmtStatus = CheckAmountStatus();
    //console.log('CheckAmtStatus ' + CheckAmtStatus);
    CalculateAmount(CheckAmtStatus);

    var TxtOtherCharge = currentrow.find("#TxtOtherCharge").val();
    var TxtOtherChargeTP = currentrow.find("#TxtOtherChargeTP").val();
    var Txtitem_tax_amt = currentrow.find("#Txtitem_tax_amt").val();
    var TxtNetValueInBase = parseFloat(TxtInvValue) + parseFloat(CheckNullNumber(Txtitem_tax_amt)) + parseFloat(CheckNullNumber(TxtOtherCharge)) + parseFloat(CheckNullNumber(TxtOtherChargeTP));
    var Txtitem_tax_amt = currentrow.find("#Txtitem_tax_amt").val();
    var Txtitem_tax_amtPrev = currentrow.next().find("#Txtitem_tax_amtPrev").val();
    var DiffTaxAmt = parseFloat(Txtitem_tax_amt) - parseFloat(CheckNullNumber(Txtitem_tax_amtPrev));
    //debugger;
    currentrow.find("#TxtItemTaxDiffValue").val(parseFloat(DiffTaxAmt).toFixed(ValDecDigit));
    // currentrow.find("#TxtItemTaxRecDiff").val(parseFloat(DiffTaxAmt).toFixed(ValDecDigit));

    var Txtitem_Net_amtPrev = currentrow.next().find("#TxtNetValueInBasePrev").val();
    var DiffNetAmt = parseFloat(TxtNetValueInBase) - parseFloat(CheckNullNumber(Txtitem_Net_amtPrev));
    currentrow.find("#TxtNetDiffValueInBase").val(parseFloat(DiffNetAmt).toFixed(ValDecDigit));
    currentrow.find("#TxtNetValueInBase").val(parseFloat(TxtNetValueInBase).toFixed(ValDecDigit));
}
function OnClickSupplierInfoIconBtn(e) {
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID_SubItems").val();
    var Supp_id = $('#SupplierName').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#QtyDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
}
function CalculateTaxAmount_ItemWise(TxtGrnNo, ItmCode, AssAmount) {
    //debugger;
    let TaxNonRecovAmt = 0;
    let TaxRecovAmt = 0;
    let TaxNonRecovAmtPrev = 0;
    let TaxRecovAmtPrev = 0;
    if (AssAmount != "" && AssAmount != null && AssAmount != "NaN") {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var TaxAmountRevised = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            var NewArrayDiff = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var GrnNo = currentRow.find("#DocNo").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if (TaxItemID == ItmCode && TxtGrnNo == GrnNo) {
                    var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted) {
                        AssessVal = 0;
                    }
                    var GstApplicable = $("#Hdn_GstApplicable").text();
                    if (GstApplicable == "Y") {
                        var ManualGST = ItemRow.find("#ManualGST").is(":checked");
                        var item_tax_amt = ItemRow.find("#Txtitem_tax_amt").val();
                        if (ManualGST && parseFloat(CheckNullNumber(item_tax_amt)) == 0) {
                            AssessVal = 0;
                        }
                    }
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxPercentage = "";
                    TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');
                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    currentRow.find("#TaxAmount").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                var DocNo = currentRow.find("#DocNo").text();
                var TaxAccId = currentRow.find("#TaxAccId").text();
                var TaxRecov = currentRow.find("#TaxRecov").text();
                var TaxRecovAmtPrev = currentRow.next().find("#TaxRecov").text();
                var TaxNonRecovAmtPrev = currentRow.next().find("#TaxNonRecov").text();
                //debugger;
                if (TaxItemID == ItmCode) {
                    if (TaxRecov == "Y") {
                        if ((TaxRecovAmt - TaxRecovAmtPrev) != 0) {
                            TaxRecovAmt = parseFloat(CheckNullNumber(TaxRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                        }
                    } else {
                        TaxNonRecovAmt = parseFloat(CheckNullNumber(TaxNonRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                    }
                }

                let cleanTaxItemID = TaxItemID.replace(/(R|P)$/g, '');
                if (TaxItemID.endsWith('R')) {
                    var previousRow = currentRow.nextAll('tr').filter(function () {
                        var prevTaxItemCode = $(this).find('td#TaxItmCode').text().trim();
                        var prevTaxNameID = $(this).find('td#TaxNameID').text().trim();
                        let prevcleanTaxItemID = prevTaxItemCode.replace(/(R|P)$/g, '');
                        return cleanTaxItemID === prevcleanTaxItemID && prevTaxNameID === TaxNameID;
                    }).first();

                    var previousTaxAmount = parseFloat(previousRow.find('td#TaxAmount').text().trim()) || 0;
                    previousTaxAmount = parseFloat(previousTaxAmount).toFixed(ValDecDigit);
                    var taxDifference = (TaxAmount - previousTaxAmount).toFixed(2);
                    currentRow.find("#TaxAmountRevised").text(taxDifference);
                    if (previousRow.length > 0) {
                        var previousTaxAmount = parseFloat(previousRow.find('td#TaxAmount').text().trim());
                        previousTaxAmount = parseFloat(previousTaxAmount).toFixed(ValDecDigit);
                        var taxDifference = (TaxAmount - previousTaxAmount).toFixed(2);
                        currentRow.find("#TaxAmountRevised").text(taxDifference);
                        TaxAmountRevised = taxDifference;
                    }
                }
                NewArray.push({ DocNo: DocNo, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov, TaxNonRecov: TaxNonRecovAmt, TaxAmountRevised: TaxAmountRevised });
                NewArrayDiff.push({ DocNo: DocNo, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: taxDifference, TotalTaxAmount: TaxAmountRevised, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId, TaxRecov: TaxRecov, TaxNonRecov: TaxNonRecovAmt });
            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().parent().each(function () {
                var currentRow = $(this);
                var ItemNo = currentRow.find("#hfItemID").val();
                var GrnNo = currentRow.find("#TxtGrnNo").val();
                let Qty = currentRow.find("#TxtReceivedQuantity").val();
                if (ItemNo == ItmCode && TxtGrnNo == GrnNo) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                                TaxNonRecovAmt = 0;
                                TaxRecovAmt = 0;
                            }
                            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit);
                                    TaxNonRecovAmt = 0;
                                    TaxRecovAmt = 0;
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                if (currentRow.find("#TaxExempted").is(":checked")) {
                                    if (parseFloat(ItemTaxAmt) == 0) {
                                        CRow.remove();
                                    }
                                }
                                else if (GstApplicable == "Y") {
                                    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                        if (parseFloat(ItemTaxAmt) == 0) {
                                            CRow.remove();
                                        }
                                    }
                                }

                                var Prev_nonRecv = currentRow.next().find("#TxtItemTaxNonRecPrev").val();
                                var Prev_Recv = currentRow.next().find("#TxtItemTaxRecPrev").val();

                                var DiffTaxAmt_rec = parseFloat(TaxRecovAmt) - parseFloat(CheckNullNumber(Prev_Recv));
                                var DiffTaxAmt_nonrec = parseFloat(TaxNonRecovAmt) - parseFloat(CheckNullNumber(Prev_nonRecv));

                                if (DiffTaxAmt_rec !== "" && !isNaN(DiffTaxAmt_rec) && DiffTaxAmt_rec !== 0) {
                                    currentRow.find("#TxtItemTaxRecDiff").val(parseFloat(DiffTaxAmt_rec).toFixed(ValDecDigit));
                                }
                                if (DiffTaxAmt_nonrec !== "" && !isNaN(DiffTaxAmt_nonrec))// && DiffTaxAmt_nonrec !== 0) 
                                {
                                    currentRow.find("#TxtItemTaxNonRecDiff").val(parseFloat(DiffTaxAmt_nonrec).toFixed(ValDecDigit));
                                }
                                //if (DiffTaxAmt_rec != 0) {
                                //    currentRow.find("#TxtItemTaxRec").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                                //}
                                currentRow.find("#TxtItemTaxRec").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                                //if (DiffTaxAmt_nonrec != 0) {
                                //    currentRow.find("#TxtItemTaxNonRec").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));
                                //}
                                currentRow.find("#TxtItemTaxNonRec").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));
                                currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));

                                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                                }
                                var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
                                var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                                if (currentRow.find("#TxtOtherChargeTP").val() != null && currentRow.find("#TxtOtherChargeTP").val() != "") {
                                    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherChargeTP").val()).toFixed(ValDecDigit);
                                }
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt) + parseFloat(OC_Amt_OR)).toFixed(ValDecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                                let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(Qty));
                                currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                                currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                                var val = parseFloat(currentRow.next().find("#TxtNetLandedPrev").val());
                                var LandedCostPrev = isNaN(val) ? (0).toFixed(LddCostDecDigit) : val.toFixed(LddCostDecDigit);
                                var LandedCostDiff = parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit) - parseFloat(CheckNullNumber(LandedCostPrev)).toFixed(LddCostDecDigit);
                                currentRow.find("#TxtLandedCostPerPcDiffValue").val(parseFloat(CheckNullNumber(LandedCostDiff)).toFixed(LddCostDecDigit));
                            }
                        });
                    }
                }
            });
            BindTaxAmountDeatils(NewArrayDiff);
        }
    }
}
/*----------------------------TDS Section-----------------------------*/
function CalculateTDS() {
    var tdsamt = $("#TxtTDS_AmountPrev").val();
    let SuppId = $("#SupplierName").val();
    var RevisedAmtDiff = parseFloat($("#TxtDiff_TotalAmountInSpec").val());
    var result = 0.00; var resultprev = 0.00; var resultTDSwise = 0.00; var totTDSAmt = 0.00;
    if (tdsamt > 0) {
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        var revisedTableBody = document.querySelector("#Hdn_TDS_CalculatorTblRevised tbody");
        if (!revisedTableBody) {
            console.error('Table body not found');
            return;
        }
        // AutoTdsApplyRevised(SuppId, RevisedAmtDiff);
        revisedTableBody.innerHTML = '';
        $("#Hdn_TDS_CalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#td_TDS_Name").text();
            var TDS_NameID = currentRow.find("#td_TDS_NameID").text();
            var TDS_Percentage = parseFloat(currentRow.find("#td_TDS_Percentage").text());
            var TDS_Level = currentRow.find("#td_TDS_Level").text();
            var TDS_ApplyOn = currentRow.find("#td_TDS_ApplyOn").text();
            // var TDS_Amount = parseFloat(currentRow.find("#td_TDS_Amount").text());
            var TDS_ApplyOnID = currentRow.find("#td_TDS_ApplyOnID").text();
            // var TDS_BaseAmt = parseFloat(currentRow.find("#td_TDS_BaseAmt").text());
            var TDS_AccId = currentRow.find("#td_TDS_AccId").text();
            var TDS_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();
            var RevisedTDS = 0;
            var RevisedTDSNew = 0;
            var PrevAmt = parseFloat($("#TxtGrossValueInBasePrev").val());
            var RevisedAmt = parseFloat($("#TxtGrossValueInBase").val());
            var RevisedTaxAmt = parseFloat($("#TaxAmountNonRecoverable").val());
            var PrevTaxAmt = parseFloat($("#TaxAmountNonRecoverablePrev").val());
            var RevisedOCAmt = parseFloat($("#TxtOtherCharges").val());
            var PrevOCAmt = parseFloat($("#TxtOtherChargesPrev").val());
            var PrevTDSAmt = parseFloat($("#TxtTDS_AmountPrev").val());
            if (TDS_AssValApplyOn == "ET") {
                if (!isNaN(PrevAmt) && !isNaN(RevisedAmt)) {
                    RevisedTDS = (RevisedAmt - PrevAmt).toFixed(2);

                    RevisedTDSNew = (RevisedAmt).toFixed(2);
                } else {
                    console.error("Invalid amounts: PrevAmt or RevisedAmt is not a valid number.");
                }
            }
            else {
                if (!isNaN(PrevAmt) && !isNaN(RevisedAmt) && !isNaN(PrevTaxAmt) && !isNaN(RevisedTaxAmt) && !isNaN(PrevOCAmt) && !isNaN(RevisedOCAmt)) {
                    RevisedTDS = ((RevisedAmt + RevisedTaxAmt + RevisedOCAmt) - (PrevAmt + PrevTaxAmt + PrevOCAmt)).toFixed(2);
                    PrevAmt = (PrevAmt + PrevTaxAmt + PrevOCAmt).toFixed(2)
                    RevisedTDSNew = ((RevisedAmt + RevisedTaxAmt + RevisedOCAmt)).toFixed(2);
                } else {
                    console.error("Invalid amounts: PrevAmt or RevisedAmt is not a valid number.");
                }
            }
            if (!isNaN(TDS_Percentage) && !isNaN(RevisedTDS)) {
                result = (TDS_Percentage / 100) * RevisedTDS;
                resultprev = (TDS_Percentage / 100) * PrevAmt;
                resultTDSwise = (TDS_Percentage / 100) * RevisedTDSNew;
                result = Math.ceil(result);
                DiffTDSWiseAmt = Math.ceil(resultTDSwise) - Math.ceil(resultprev);
                resultTDSwise = Math.ceil(resultTDSwise);
                result = parseFloat((Number(PrevTDSAmt) + Number(result)).toFixed(2));
                //var result = Math.ceil(((TDS_Percentage / 100) * RevisedTDS) * Math.pow(10, ValDecDigit)) / Math.pow(10, ValDecDigit);
                //currentRow.find("#td_TDS_Amount").text(resultTDSwise.toFixed(2));
                //currentRow.find("#TDS_val").text(result.toFixed(2));
                //currentRow.find("#TotalTDS_Amount").text(result.toFixed(2));
                //currentRow.find("#td_TDS_RevisedAmt").text(DiffTDSWiseAmt);
                totTDSAmt = resultTDSwise + totTDSAmt;
            } else {
                console.error("Invalid TDS_Percentage or RevisedTDS.");
            }

            var newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td id="td_TDS_Name">${TDS_Name}</td>
                <td id="td_TDS_NameID">${TDS_NameID}</td>
                <td id="td_TDS_Percentage">${TDS_Percentage.toFixed(2)}</td>
                <td id="td_TDS_Level">${TDS_Level}</td>
                <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
                <td id="td_TDS_Amount">${resultTDSwise.toFixed(2)}</td>
                <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
                <td id="td_TDS_BaseAmt">${RevisedAmt.toFixed(2)}</td>
                <td id="td_TDS_AccId">${TDS_AccId}</td>
                <td id="td_TDS_AssValApplyOn">${TDS_AssValApplyOn}</td>
                <td id="td_TDS_RevisedAmt">${DiffTDSWiseAmt || "0.00"}</td>
                <td>${(result || "0.00")}</td>`;
            revisedTableBody.appendChild(newRow);
        });
        //$("#TxtTDS_Amount").val(totTDSAmt.toFixed(2));
        $("#TxtTDS_Amount").val(resultTDSwise.toFixed(2));
    }
}
function CalculateTDS_RevisedAmt() {
    var tdsamt = $("#TxtTDS_AmountPrev").val();
    var result = 0.00; var resultprev = 0.00; var resultTDSwise = 0.00; var totTDSAmt = 0.00; var TDS_AssValApplyOn_Main = "";
    if (tdsamt > 0) {
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        var revisedTableBody = document.querySelector("#Hdn_TDS_CalculatorTblRevised tbody");
        if (!revisedTableBody) {
            console.error('Table body not found');
            return;
        }
        // AutoTdsApplyRevised(SuppId, RevisedAmtDiff);
        revisedTableBody.innerHTML = '';
        $("#Hdn_TDS_CalculatorTbl > tbody > tr").each(function () {
            resultTDSwise = 0.00;
            var currentRow = $(this);
            var TDS_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();
            TDS_AssValApplyOn_Main = currentRow.find("#td_TDS_AssValApplyOn").text();
            if (TDS_AssValApplyOn != "CA") {
                var TDS_Name = currentRow.find("#td_TDS_Name").text();
                var TDS_NameID = currentRow.find("#td_TDS_NameID").text();
                var TDS_Percentage = parseFloat(currentRow.find("#td_TDS_Percentage").text());
                var TDS_Level = currentRow.find("#td_TDS_Level").text();
                var TDS_ApplyOn = currentRow.find("#td_TDS_ApplyOn").text();
                var TDS_ApplyOnID = currentRow.find("#td_TDS_ApplyOnID").text();
                var TDS_AccId = currentRow.find("#td_TDS_AccId").text();

                var RevisedTDS = 0;
                var RevisedTDSNew = 0;
                var PrevAmt = parseFloat($("#TxtGrossValueInBasePrev").val());
                var RevisedAmt = parseFloat($("#TxtDiff_TotalAmountInSpec").val());
                var RevisedTaxAmt = parseFloat($("#TaxAmountNonRecoverable").val());
                var PrevTaxAmt = parseFloat($("#TaxAmountNonRecoverablePrev").val());
                var RevisedOCAmt = parseFloat($("#TxtOtherCharges").val());
                var PrevOCAmt = parseFloat($("#TxtOtherChargesPrev").val());
                var PrevTDSAmt = parseFloat($("#TxtTDS_AmountPrev").val());
                if (TDS_AssValApplyOn == "ET") {
                    if (!isNaN(PrevAmt) && !isNaN(RevisedAmt)) {
                        RevisedTDS = (parseFloat($("#TxtDiff_TotalAmountInSpec").val()) || 0.00).toFixed(2);
                        RevisedTDSNew = RevisedTDS;//(RevisedTDS).toFixed(2);
                    } else {
                        console.error("Invalid amounts: PrevAmt or RevisedAmt is not a valid number.");
                    }
                }
                else {
                    if (!isNaN(PrevAmt) && !isNaN(RevisedAmt) && !isNaN(PrevTaxAmt) && !isNaN(RevisedTaxAmt) && !isNaN(PrevOCAmt) && !isNaN(RevisedOCAmt)) {
                        RevisedTDS = (parseFloat($("#TxtDiff_Amount").val()) || 0.00).toFixed(2);
                        RevisedTDSNew = RevisedTDS;//(RevisedTDS).toFixed(2);
                    } else {
                        console.error("Invalid amounts: PrevAmt or RevisedAmt is not a valid number.");
                    }
                }
                if (!isNaN(TDS_Percentage) && !isNaN(RevisedTDS)) {
                    result = (TDS_Percentage / 100) * RevisedTDS;
                    resultprev = (TDS_Percentage / 100) * PrevAmt;
                    resultTDSwise = (TDS_Percentage / 100) * RevisedTDSNew;
                    result = Math.ceil(result);
                    DiffTDSWiseAmt = Math.ceil(resultTDSwise) - Math.ceil(resultprev);
                    resultTDSwise = Math.ceil(resultTDSwise);
                    result = parseFloat((Number(PrevTDSAmt) + Number(result)).toFixed(2));
                    totTDSAmt = resultTDSwise + totTDSAmt;
                    var TDS_Amount = resultTDSwise + PrevTDSAmt;
                } else {
                    console.error("Invalid TDS_Percentage or RevisedTDS.");
                }
                var newRow = document.createElement('tr');
                newRow.innerHTML = `
                <td id="td_TDS_Name">${TDS_Name}</td>
                <td id="td_TDS_NameID">${TDS_NameID}</td>
                <td id="td_TDS_Percentage">${TDS_Percentage.toFixed(2)}</td>
                <td id="td_TDS_Level">${TDS_Level}</td>
                <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
                <td id="td_TDS_Amount">${TDS_Amount.toFixed(2)}</td>
                <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
                <td id="td_TDS_BaseAmt">${resultTDSwise.toFixed(2)}</td>
                <td id="td_TDS_AccId">${TDS_AccId}</td>
                <td id="td_TDS_AssValApplyOn">${TDS_AssValApplyOn}</td>
                <td id="td_TDS_RevisedAmt">${resultTDSwise || "0.00"}</td>
                <td>${(result || "0.00")}</td>`;
                revisedTableBody.appendChild(newRow);
            }
        });
        var amt = parseFloat(($("#TxtTDS_AmountPrev").val()));
        if (TDS_AssValApplyOn_Main != "CA") {
            var b = parseFloat(resultTDSwise) + parseFloat(amt);
        }
        else {
            var b = parseFloat(0);
        }
        $("#TxtTDS_Amount").val((b).toFixed(2));
    }
}
function OnClickTDSCalculationBtn() {
    var SupplierNo = $("#SupplierName option:selected").text();
    var SupplierVal = $("#SupplierName option:selected").val();

    if (SupplierNo == "" || SupplierNo == null || SupplierNo == undefined) {
        SupplierNo = InvNo;
    }
    if (SupplierVal == "" || SupplierVal == null || SupplierVal == undefined || SupplierVal == "0") {
        SupplierVal = "";
    }
    if (SupplierNo == "" || SupplierVal == "") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#BtnTDS_Amount").attr("data-target", "");
        return false;
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }

    var InvNo = $("#ddlPurchaseinv_no option:selected").text();
    var InvNoval = $("#ddlPurchaseinv_no option:selected").val();

    if (InvNoval == "0") {
        $('#SpanPInvoiceErrorMsg').text($("#valueReq").text());
        $("#SpanPInvoiceErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "red");
        $("#BtnTDS_Amount").attr("data-target", "");
        return false;
    } else {
        $("#SpanPInvoiceErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "#ced4da");
    }

    const RevisedGrVal = $("#TxtGrossValueInBase").val();
    const ToTdsAmt = parseFloat(CheckNullNumber(RevisedGrVal)).toFixed(ValDecDigit);
    var OcValWithTax = $("#NetOrderValueInBase").val();
    const AssessableValue = parseFloat(CheckNullNumber(ToTdsAmt));
    $("#hdn_tds_on").val("D");
    $("#TDS_AssessableValue").val(AssessableValue);
    //const NetVal = $("#NetOrderValueInBase").val();
    TDSCalculationBtn(ToTdsAmt, null, null, OcValWithTax);
    //CalculateTDS_RevisedAmt();
    //CMN_OnClickTDSCalculationBtn(ToTdsAmt, null, null, NetVal, "TDS");
}
function OnClickTDSCalculationBtnPrev() {
    var SupplierNo = $("#SupplierName option:selected").text();
    var SupplierVal = $("#SupplierName option:selected").val();

    if (SupplierNo == "" || SupplierNo == null || SupplierNo == undefined) {
        SupplierNo = InvNo;
    }
    if (SupplierVal == "" || SupplierVal == null || SupplierVal == undefined || SupplierVal == "0") {
        SupplierVal = "";
    }
    if (SupplierNo == "" || SupplierVal == "") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#BtnTDS_AmountPrev").attr("data-target", "");
        return false;
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }
    var InvNo = $("#ddlPurchaseinv_no option:selected").text();
    var InvNoval = $("#ddlPurchaseinv_no option:selected").val();

    if (InvNoval == "0") {
        $('#SpanPInvoiceErrorMsg').text($("#valueReq").text());
        $("#SpanPInvoiceErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "red");
        $("#BtnTDS_AmountPrev").attr("data-target", "");
        return false;
    } else {
        $("#SpanPInvoiceErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlPurchaseinv_no-container']").css("border-color", "#ced4da");
    }
    const GrVal = $("#TxtGrossValuePrev").val();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal)).toFixed(ValDecDigit);
    const NetVal = $("#NetOrderValueInBasePrev").val();
    const ToTdsAmt_IT = parseFloat(CheckNullNumber(NetVal)).toFixed(ValDecDigit);
    $("#hdn_tds_on").val("D");
    $("#TDS_AssessableValuePrev").val(ToTdsAmt);

    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "P", null, ToTdsAmt_IT);
}

async function TDSCalculationBtn(NetAmt, Hdn_Tds_Table_Id, Oc_Id, OcValWithTax, tax_type) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var TDS_data = [];
    Hdn_Tds_Table_Id = IsNull(Hdn_Tds_Table_Id, "Hdn_TDS_CalculatorTblRevised");
    var tds_ass_aply_on = "ET";
    $("#" + Hdn_Tds_Table_Id + " tbody tr").each(function () {
        var row = $(this);
        var bindrow = "Y";
        if (Hdn_Tds_Table_Id == "Hdn_OC_TDS_CalculatorTbl") {
            if (row.find("#td_TDS_OC_Id").text() != Oc_Id) {
                bindrow = "N";
            }
        }
        // CalculateTDS();
        CalculateTDS_RevisedAmt();
        if (bindrow == "Y") {
            var list = {};
            list.tds_name = row.find("#td_TDS_Name").text();
            list.tds_id = row.find("#td_TDS_NameID").text();
            list.tds_rate = row.find("#td_TDS_Percentage").text();
            list.tds_level = row.find("#td_TDS_Level").text();
            list.tds_apply_on = row.find("#td_TDS_ApplyOn").text();
            list.tds_amt = parseFloat(row.find("#td_TDS_Amount").text()).toFixed(0);
            //list.tds_amt = parseFloat($("#TxtTDS_Amount").val()).toFixed(2);
            list.tds_apply_on_id = row.find("#td_TDS_ApplyOnID").text();
            list.tds_acc_id = row.find("#td_TDS_AccId").text();
            list.tds_base_amt = row.find("#td_TDS_BaseAmt").text();
            list.tds_ass_val_apply_on = row.find("#td_TDS_AssValApplyOn").text();
            TDS_data.push(list);
            if (row.find("#td_TDS_AssValApplyOn").text() == "IT") {
                tds_ass_aply_on = "IT";
            }
        }
    });
    var data = JSON.stringify(TDS_data);
    var txtdisable = "Y";//$("#txtdisable").val();
    await $.ajax({
        type: "POST",
        url: "/Common/Common/Cmn_GetTDSDetail",
        data: {
            NetAmt: parseFloat(CheckNullNumber(NetAmt)).toFixed(cmn_ValDecDigit), TDS_data: data, doc_id: DocumentMenuId, Disable: txtdisable, tax_type: tax_type
        },
        success: function (data) {

            $("#PopUp_TDSCalculate").html(data);
            $("#TDS_AssessableValueWithTax").val(parseFloat(CheckNullNumber(OcValWithTax)).toFixed(cmn_ValDecDigit));
            if (tds_ass_aply_on == "ET") {
                $("#TdsAssVal_ExcludeTax").attr("checked", true);
            }
            else if (tds_ass_aply_on == "IT") {
                $("#TdsAssVal_IncluedTax").attr("checked", true);
            }
            HideShowTDSLevel();
            var TDS_Level = $("#TDS_CalculatorTbl tbody tr:last-child #TDS_level").text();
            ResetTDS_LevelVal(TDS_Level);
            $('#TDS_Level').val("1").prop('selected', true);
            if (parseFloat(CheckNullNumber(NetAmt)) == 0) {

                let SuppId = $("#SupplierName").val();
                let GrossAmt = CheckNullNumber(NetAmt);
                //$.ajax({
                //    type: "POST",
                //    url: "/SupplementaryPurchaseInvoice/GetTdsDetails",
                //    data: { SuppId: SuppId, GrossVal: GrossAmt, tax_type: "TDS" },
                //    success: function (data) {
                var arr = JSON.parse(data);
                let total_tds_bs_amt = 0;
                let AutoTds = "N";
                if (arr.Table != null) {
                    if (arr.Table[0].tds_posting == "Y") {
                        if (arr.Table[0].tds_apply_on == "B") {
                            AutoTds = "Y";
                        }
                    }
                }
                if (AutoTds == "Y") {
                    if (arr.Table1 != null) {
                        for (var i = 0; i < arr.Table1.length; i++) {
                            let tds_bs_amt = arr.Table1[i].tds_bs_amt;
                            total_tds_bs_amt = parseFloat(total_tds_bs_amt) + parseFloat(tds_bs_amt);
                        }
                        $("#TDS_AssessableValue").val(parseFloat(total_tds_bs_amt).toFixed(cmn_ValDecDigit));
                    }
                } else {
                    $("#TDS_AssessableValue").val(parseFloat(NetAmt).toFixed(cmn_ValDecDigit));
                }
            }
            /*})*/
            //}
            else {
                $("#TDS_AssessableValue").val(parseFloat(CheckNullNumber(NetAmt)).toFixed(cmn_ValDecDigit));
            }
        }
    });
}

async function AutoTdsApplyRevised(SuppId, GrossAmt) {
    //debugger;
    await $.ajax({
        type: "POST",
        url: "/Common/Common/Cmn_GetTdsDetails",
        data: { SuppId: SuppId, GrossVal: GrossAmt, tax_type: "TDS" },
        success: function (data) {
            //debugger;
            var arr = JSON.parse(data);
            let tds_amt = 0;
            if (arr.Table1 != null) {
                let checkResult = arr.Table1.length > 0 ? arr.Table1[0].result : "";
                // $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
                if (checkResult == "Invalid Slab") {
                    swal("", $("#InvailidTdsSlabFound").text(), "warning")
                } else {
                    var checkTdsAcc = "Y";
                    $('#Hdn_TDS_CalculatorTblRevised tbody tr').remove();
                    for (var i = 0; i < arr.Table1.length; i++) {
                        //let td_tds_amt = Math.round(arr.Table1[i].tds_amt);
                        let td_tds_amt = Cmn_RoundValue(arr.Table1[i].tds_amt, "P");
                        if (arr.Table1[i].tds_id == "") {
                            checkTdsAcc = "N";
                        }
                        if (checkTdsAcc == "Y") {
                            $('#Hdn_TDS_CalculatorTblRevised tbody').append(`<tr id="R${i + 1}">
                                <td id="td_TDS_Name">${arr.Table1[i].tds_name}</td>
                                <td id="td_TDS_NameID">${arr.Table1[i].tds_id}</td>
                                <td id="td_TDS_Percentage">${arr.Table1[i].tds_perc}</td>
                                <td id="td_TDS_Level">${arr.Table1[i].tds_level}</td>
                                <td id="td_TDS_ApplyOn">${arr.Table1[i].tds_apply_on}</td>
                                <td id="td_TDS_Amount">${td_tds_amt}</td>
                                <td id="td_TDS_ApplyOnID">${arr.Table1[i].tds_apply_on_id}</td>
                                <td id="td_TDS_BaseAmt">${arr.Table1[i].tds_bs_amt}</td>
                                <td id="td_TDS_AccId">${arr.Table1[i].tds_acc_id}</td>
                                <td id="td_TDS_RevisedAmt">0</td>
                                    </tr>`);
                            tds_amt += parseFloat(td_tds_amt);
                        }
                        // $("#TxtTDS_Amount").val((Math.ceil(tds_amt * Math.pow(10, ValDecDigit)) / Math.pow(10, ValDecDigit)).toFixed(ValDecDigit));

                        //   $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));

                    }

                    if (checkTdsAcc == "N") {
                        $('#Hdn_TDS_CalculatorTblRevised tbody tr').remove();
                        $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
                        swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
                    }
                }
                GetAllGLID();
            }
            else {

            }


        }
    })

    DifferenceSummary();
}
/*----------------------------TDS Section End-----------------------------*/
/*---------------------------------------List Page---------------------------------*/
function FromToDateValidation() {
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
/*---------------------------------------List Page---------------------------------*/
/*** --------------------------- GL Voucher Entry Start-----------------------------***/
function GetAllGLID() {

    GetAllGL_WithMultiSupplier();
}
async function GetAllGL_WithMultiSupplier() {
    //debugger;
    //console.log('1');// click_chkplusminus();
    //$("#VoucherDetail tbody tr").remove();
    if ($("#POInvItmDetailsTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#tbladdhdn tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    if (Check_ItemValidations("Y") == false) {
        return false;
    }
    var GstType = $("#Hd_GstType").val();
    var TxtDiff_TotalAmountInSpec = $("#TxtDiff_TotalAmountInSpec").val();
    //var TxtDiff_Amount = $("#TxtDiff_Amount").val();
    //var TxtDiff_TotalOCAmount = $("#TxtDiff_TotalOCAmount").val();
    var GstCat = $("#Hd_GstCat").val();
    //var NetInvValue = $("#NetOrderValueInBase").val();
    var NetInvValue = $("#TxtDiff_Amount").val();
    var conv_rate = $("#conv_rate").val();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0;
    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    var curr_id = $("#curr_id").val();
    var bill_no = $("#Bill_No").val();
    var bill_dt = $("#Bill_Date").val();
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];

    //if ((Number(TxtDiff_TotalAmountInSpec) !== 0)|| (Number(TxtDiff_TotalOCAmount) !== 0))
    if ((Number(SuppVal) !== 0)) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }
    $("#POInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        let newStr = item_id.replace(/R$/, "");
        //var ItmGrossVal = parseFloat(currentRow.find("#TxtItemGrossValue").val()) || 0;  
        //var ItmGrossValInBase = parseFloat(currentRow.find("#TxtItemGrossValueInBase").val()) || 0; 
        var ItemTaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()) || 0;
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
        var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
        var ItemDiffAmt = parseFloat(currentRow.find("#TxtItemDiffValue").val()) || 0;
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExantedItemList.push({ item_id: item_id });
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExantedItemList.push({ item_id: item_id });
            }
        }
        if (Number(TxtDiff_TotalAmountInSpec) !== 0) {
            if (Number(ItemDiffAmt) !== 0) {
                GLDetail.push({
                    comp_id: Compid, id: newStr, type: "Itm", doctype: InvType, Value: ItemDiffAmt
                    , ValueInBase: ItemDiffAmt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
                    , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: item_acc_id
                });
            }
        }
    });
    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var OCFor = currentRow.find("#OCFor").text();
        var OCForText = (OCFor.includes("P") ? "P" : (OCFor.includes("R") ? "R" : null));
        if (OCForText == 'R') {
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
        if (ArrOcGl.length > 0) {
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "OcTax", doctype: InvType, Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: ArrOcGl[0].id
                , Entity_id: tax_acc_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate,
                bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        }
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
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        //var tax_amt = currentRow.find("#TaxAmount").text();
        var tax_amt = currentRow.find("#TaxAmountRevised").text();

        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();
        ////debugger;
        if (Number(TxtDiff_TotalAmountInSpec) !== 0) {

            //if (parseFloat(CheckNullNumber(tax_amt)) !== 0) {
            ////debugger;

            var GstApplicable = $("#Hdn_GstApplicable").text();
            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            var tax_amt_diff = ItemRow.find("#TxtItemTaxDiffValue").val() || 0;
            if (parseFloat(CheckNullNumber(tax_amt_diff)) !== 0) {

                if (TaxExempted == false) {
                    var ClaimItc = true;
                    if (GstApplicable == "Y") {
                        ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
                    }
                    if (TaxRecov == "N" || !ClaimItc) {
                        if (GLDetail.findIndex((obj => obj.id + "R" == TaxItmCode)) > -1) {
                            var objIndex = GLDetail.findIndex((obj => obj.id + "R" == TaxItmCode));
                            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
                            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);

                            console.log('10 ' + objIndex + ' ' + parseFloat(GLDetail[objIndex].Value) + ' ' + parseFloat(tax_amt));
                        }
                    } else {
                        //debugger;
                        GLDetail.push({
                            comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                            , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                            , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                        });
                    }
                }
            }
        }
    });
   // debugger;
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            //var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxVal = currentRow.find("#TaxAmountRevised").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();

            var ItemRow = $("#POInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            // var tax_amt_diff = ItemRow.find("#TxtItemTaxDiffValue").text();
            var tax_amt_diff = ItemRow.find("#TxtItemTaxDiffValue").val();

            if (parseFloat(CheckNullNumber(tax_amt_diff)) != 0) {
                //debugger;
                var ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
                if (ClaimITC) {
                    if (TaxRecov != "N") {
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
                    }
                }
            }
        });
    }
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTblRevised >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        //var tds_amt = $("#TxtDiff_TotalTDSAmount").val();//currentRow.find("#td_TDS_Amount").text();
        var tds_amt = currentRow.find("#td_TDS_RevisedAmt").text();

        if (parseFloat(CheckNullNumber(tds_amt)) != 0) {
            Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: supp_acc_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        }
    });
    if (Cal_Tds_Amt != 0) {
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
        //debugger;
        var ArrOcGl = GLDetail.filter(v => (v.id == tds_supp_id && v.type == "Supp"));
        var tdsIndex = Oc_Tds.findIndex(v => v.supp_id == tds_supp_id);
        if (ArrOcGl.length > 0) {
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
    //debugger;
    //if ((Number(TxtDiff_TotalAmountInSpec) !== 0) || (Number(TxtDiff_TotalOCAmount) !== 0)) {
    await Cmn_GLTableBind(supp_acc_id, GLDetail, "Purchase")
    // }
}

function OnChangeAccountName(RowID, e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
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

function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType, curr_id, conv_rate, bill_bo, bill_date) {
    ////debugger;
    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }
    if (parseFloat(DrValue) < 0) {
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
    if (type == "Supp" || type == "Bank") {
        $('#VoucherDetail tbody').append(`<tr id="tr_GlRow${SrNo}">
                                <td rowspan="${rowSpan}" id="td_SrNo">${SrNo}</td>
                                `+ Table_tds + `
                                <td rowspan="${rowSpan}" id="td_VouType">${VouType}</td>
                                <td rowspan="${rowSpan}" id="td_VouNo"></td>
                                <td rowspan="${rowSpan}" id="td_VouDate"></td>
                                
                            </tr>`)
    } else {
        $('#VoucherDetail tbody').append(`<tr>` + Table_tds + ` </tr>`)
    }
}
async function AddRoundOffGL() {
    var curr_id = $("#curr_id").val();
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    $("#VoucherDetail >tbody >tr").each(function () {
                        var currentRow = $(this);
                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
                        }
                    });
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });

                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , Diff, DiffInBase, 0, 0, "PV", curr_id
                                        , $("#conv_rate").val()
                                        , $("#Bill_No").val(), $("#Bill_Date").val())

                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }
                        else {
                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                            var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });
                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , 0, 0, Diff, DiffInBase, "PV"
                                        , curr_id, $("#conv_rate").val(), $("#Bill_No").val()
                                        , $("#Bill_Date").val())
                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }
                    }
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
async function CalculateVoucherTotalAmount() {
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
        }
    });
    $("#DrTotal").text(DrTotAmt);
    $("#DrTotalInBase").text(DrTotAmtInBase);
    $("#CrTotal").text(CrTotAmt);
    $("#CrTotalInBase").text(CrTotAmtInBase);
    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }
    }
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Nurration").val() : VouType == "BP" ? $("#hdn_BP_Nurration").val() : VouType == "CN" ? $("#hdn_CN_Nurration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}

/***---------------------------GL Voucher Entry End-----------------------------***/

/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e, OCFor) {
    //debugger;
    const row = $(e.target).closest("tr");
    CC_Clicked_Row = row;
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const TotalVal = CC_Clicked_Row.find("#OCTotalTaxAmt").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    $("#TDS_AssessableValue").val(parseFloat(CheckNullNumber(ToTdsAmt)).toFixed(ValDecDigit));
    //CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, TotalVal);
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, OCFor, TDS_OcId, TotalVal);
}
function OnClickTP_TDS_SaveAndExit() {
    //debugger;
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

            let TDS_temp = "per";

            $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function () {
                let row = $(this);
                let existingSuppID = row.find("#td_TDS_Supp_Id").text().trim();
                if (existingSuppID === TDS_SuppId) {
                    TDS_temp = "temp";
                }
            });
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
            <td id="td_TDS_temp">${TDS_temp}</td>
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
            var TDS_Temp = "permanent";

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
            <td id="td_TDS_temp">${TDS_Temp}</td>
                </tr>`);
        });
    }
    SetTds_Amt_To_OC();
    /*<td id="td_TDS_temp">${TDS_Temp}</td>*/
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
}
function SetTds_Amt_To_OC() {
    var TotalAMount = parseFloat(0);
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
    });
    CC_Clicked_Row.find("#OC_TDSAmt").text(parseFloat(CheckNullNumber(TotalAMount)).toFixed(ValDecDigit));
    CC_Clicked_Row = null;
}
function OnClickTDS_SaveAndExit() {
    //debugger;
    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {
        let TdsAssVal_applyOn = "ET";
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
        }
        // var TotalTDS_Amount = $('#TotalTDS_Amount').text();
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        var rowIdx = 0;
        var GstCat = $("#Hd_GstCat").val();
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
            <td id="td_TDS_RevisedAmt">0</td>
            <td id="td_TDS_temp">"temp"</td>
                </tr>`);

                //NewArr.push({ /*UserID: UserID, */DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
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

                $("#Hdn_TDS_CalculatorTblRevised tbody").append(`<tr id="R${++rowIdx}">
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
            <td id="td_TDS_RevisedAmt">0</td>
                </tr>`);

                //NewArr.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
            });

        }
        var TotalAMount = parseFloat(0).toFixed(ValDecDigit);

        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            console.log('temp');
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(ValDecDigit);
            currentRow.find("#TDS_temp").text("T");
        });

        //$("#TxtTDS_Amount").val(TotalAMount);
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        //$("#PopUp_TDSCalculate").html("");

        GetAllGLID();
    }
}
/***------------------------------TDS On Third Party End------------------------------***/

function OnchangeBillNumber() {
    if ($("#Bill_No").val() == "") {
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
        $("#Bill_No").css("border-color", "Red");
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
    RefreshBillNoBillDateInGLDetail();
}

function OnchangeBill_Date() {
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        $("#SpanBillDateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }
    RefreshBillNoBillDateInGLDetail();
}

function RefreshBillNoBillDateInGLDetail() {
    /* Modified by Suraj Maurya on 11-08-2025 to update bill_no and bill_date in GL Narration */
    /*This Function is Created by Suraj on 22-10-2024 to reset bill number and bill date to GL Details*/
    let supp_acc_id = $("#supp_acc_id").val();
    let bill_no = $("#Bill_No").val();
    let bill_dt = $("#Bill_Date").val();
    let change_vou_sr_no = "";
    //$('#VoucherDetail > tbody > tr #hfAccID[value="' + supp_acc_id + '"]').closest("tr").each(function () {
    $('#VoucherDetail > tbody > tr').closest("tr").each(function () {
        let row = $(this);
        let hfAccID = row.find("#hfAccID").val();
        let vouType = row.find("#glVouType").val();
        let vou_sr_no = row.find("#td_vou_sr_no").text();
        if (hfAccID == supp_acc_id) {
            change_vou_sr_no = vou_sr_no;
            row.find("#gl_bill_no").val(bill_no);
            row.find("#gl_bill_date").val(bill_dt);
            let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
            row.find("#gl_narr span").text(narr);
        } else {
            if (vou_sr_no == change_vou_sr_no) {
                let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
                row.find("#gl_narr span").text(narr);
            }
        }
    });
}

function ValidateNetAmountBeforeSubmit() {
    $("#POInvItmDetailsTbl > tbody > tr").each(function () {
        var currentrow = $(this);
        var SNo = currentrow.find("#hfSNo").val();
        var SNo_next = currentrow.next().find("#hfSNo").val();
        var TxtRate = currentrow.find("#TxtRate" + SNo).val();
        var TxtRatePrev = currentrow.next().find("#TxtRatePrev" + SNo_next).val();
        if ((parseFloat(CheckNullNumber(TxtRate)) < TxtRatePrev)) {
            currentrow.find("#TxtRate" + SNo).css("border-color", "red");
            currentrow.find("#TxtRate_Error" + SNo).css("display", "block");
            currentrow.find("#TxtRate_Error" + SNo).text($("#span_InvalidPrice").text());
            return false;
        } else {
            currentrow.find("#TxtRate" + SNo).css("border-color", "#ced4da");
            currentrow.find("#TxtRate_Error" + SNo).css("display", "none");
            currentrow.find("#TxtRate_Error" + SNo).text("");
        }
    });

    var grValue = $('#TxtDiff_TotalAmountInBase').val();
    var taxAmt = parseFloat(CheckNullNumber($('#TxtDiff_TotalTaxAmountRec').val())) + parseFloat(CheckNullNumber($('#TxtDiff_TotalTaxAmountNonRec').val()));
    var OcAmt_self = 0.00; var OcAmt_tp = 0.00;
    // $("#Tbl_OC_Deatils >tbody >tr").each(function () {
    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCFor = currentRow.find("#OCFor").text().trim();
        var OCForText = (OCFor.includes("P") ? "P" : (OCFor.includes("R") ? "R" : null));
        var OCBillNo = currentRow.find("#OCBillNo").text();
        if (OCForText === "R" && OCBillNo == "") {
            OcAmt_self = (parseFloat(OcAmt_self) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
        }
    });
    OcAmt_self = OcAmt_self || 0.00;
    var netAmt = parseFloat(CheckNullNumber($('#TxtDiff_Amount').val())).toFixed(ValDecDigit);
    var calculatedAmt = parseFloat(parseFloat(CheckNullNumber(grValue)) + parseFloat(CheckNullNumber(taxAmt)) + parseFloat(CheckNullNumber(OcAmt_self))).toFixed(ValDecDigit);
    if ($("#chk_roundoff").is(":checked")) {
        //var roundoff_netval = Math.round(calculatedAmt);
        //calculatedAmt = parseFloat(roundoff_netval).toFixed(ValDecDigit);
        if ($("#p_round").is(":checked")) {
            calculatedAmt = SPI_RoundValue(calculatedAmt, "P")
        }
        if ($("#m_round").is(":checked")) {
            calculatedAmt = SPI_RoundValue(calculatedAmt, "M")
        }
    }
    if (netAmt != calculatedAmt) {
        swal("", $("#NetPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    return true;
}

function ValidateGLAmountBeforeSubmit() {
    var grValue = $('#TxtDiff_TotalAmountInBase').val();
    var TdsValue = $('#TxtDiff_TotalTDSAmount').val();
    var taxAmt = parseFloat(CheckNullNumber($('#TxtDiff_TotalTaxAmountRec').val())) + parseFloat(CheckNullNumber($('#TxtDiff_TotalTaxAmountNonRec').val()));
    var OcAmt = parseFloat(0); var OcAmt_self = parseFloat(0); var OcAmt_tp = parseFloat(0); var OcTdsAmt = parseFloat(0);
    $("#Tbl_OC_Deatils >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCFor = currentRow.find("#OCFor").text().trim();
        var OCForText = (OCFor.includes("P") ? "P" : (OCFor.includes("R") ? "R" : null));
        var OCBillNo = currentRow.find("#OCBillNo").text();
        OcTdsAmt = parseFloat(OcTdsAmt) + parseFloat(currentRow.find("#OC_TDSAmt").text());
        if (OCForText === "R" && OCBillNo == "") {
            OcAmt_self = (parseFloat(OcAmt_self) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
        }
        else if (OCForText === "R" && OCBillNo != "") {
            OcAmt_tp = (parseFloat(OcAmt_tp) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
        }
        OcAmt_self = parseFloat(OcAmt_self) || 0.00;
        OcAmt_tp = parseFloat(OcAmt_tp) || 0.00;
        OcTdsAmt = parseFloat(OcTdsAmt) || 0.00;
        OcAmt = parseFloat(OcAmt_self) + parseFloat(OcAmt_tp) + parseFloat(OcTdsAmt);
    });
    OcAmt = parseFloat(OcAmt) || 0.00;

    var netAmt = 0;
    $("#VoucherDetail >tfoot >tr").each(function () {
        var currentRow = $(this);
        netAmt = currentRow.find("#CrTotalInBase").text().trim() || 0.00;
    });
    var calculatedAmt = parseFloat(parseFloat(CheckNullNumber(grValue)) + parseFloat(CheckNullNumber(taxAmt)) + parseFloat(CheckNullNumber(OcAmt)) + parseFloat(CheckNullNumber(TdsValue))).toFixed(ValDecDigit);

    if (netAmt != calculatedAmt) {
        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    return true;
}
function OtherFunctions(StatusC, StatusName) {
    //window.location.reload();
}
//Added by Nidhi on 02-09-2025
function SendEmail() {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var supp_id = $("#SupplierName").val();
    Cmn_SendEmail(docid, supp_id, 'Supp');
}
function SendEmailAlert() {
    //debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#SuppPIInv_date").val();
    var statusAM = $("#Amendment").val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/Common/Common/SendEmailAlert", "")
}
function ViewEmailAlert() {
    //debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#SuppPIInv_date").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
}
function EmailAlertLogDetails() {
    var status = $('#hfPIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#SuppPIInv_date").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//End on  02-09-2025