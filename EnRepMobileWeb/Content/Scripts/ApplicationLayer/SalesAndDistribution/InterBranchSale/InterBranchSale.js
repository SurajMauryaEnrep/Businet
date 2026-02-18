/************************************************
Javascript Name: Inter Branch Sales
Created By:Shubham Maurya
Created Date: 29-05-2025
Description: This Javascript use for the Local Sale Order Detail many function

Modified By:
Modified Date:
Description:
*************************************************/
$(document).ready(function () {
    debugger;
    $("#CustomerName").select2();
    $("#ddlCustomerName").select2();
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $('#ListFilterData').val();
        var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        if (SSINo != "" && SSINo != null) {
            location.href = "/ApplicationLayer/InterBranchSale/AddIBSDetail/?DocNo=" + SSINo + "&DocDate=" + SSIDt + "&ListFilterData=" + ListFilterData;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        var SSINo = clickedrow.children("#SSINo").text();
        var SSIDt = clickedrow.children("#SSIDt").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SSINo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SSINo, SSIDt, Doc_id, Doc_Status);

    });
   
    $('#ScrapSIItemTbl tbody').on('click', '.deleteIcon', async function () {
        ////
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
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
        var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
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
        $("#ScrapSIItemTbl > tbody > tr ").each(function () {
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
    GetWorkFlowDetails(InvoiceNumber, InvoiceDate, "105103149", hdStatus);
    $("#hdDoc_No").val(InvoiceNumber);
});
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
function SerialNoAfterDelete() {

    var SerialNo = 0;
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
        //
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
        //currentRow.find("#SNohiddenfiled").text(SerialNo);
    });
};
function BtnSearch() {
    debugger;
    FilterVerificationList();
    ResetWF_Level();
}
function FilterVerificationList() {
    debugger;
    try {
        var CustId = $("#ddlCustomerName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var WFStatus = "";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/InterBranchSale/SearchIBSList",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodySSIList').html(data);
                $('#ListFilterData').val(CustId + ',' + Fromdate + ',' + Todate + ',' + Status + ','+WFStatus);
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
function OnClickTDS_SaveAndExit() {
    debugger
    var ValDecDigit = $("#ValDigit").text();
    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {
        let TdsAssVal_applyOn = "ET";
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
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
        CalculateAmount();
        GetAllGLID();
    }
}
function OnChangeCustomer(CustID) {
    var DecDigit = $("#RateDigit").text();
    var Cust_id = CustID.value;
    var hdbs_curr = $("#hdbs_curr").val();
    var hdcurr = $("#hdcurr").val();
    if (hdcurr != null && hdcurr != "") {
        if (hdbs_curr != hdcurr) {

            $("#Tbl_OtherChargeList thead tr th:eq(2),#Tbl_OtherChargeList thead tr th:eq(3)").remove();
            $("#_OtherChargeTotalTax").remove();
            $("#_OtherChargeTotalAmt").remove();
        }
    }
    if (Cust_id == "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#SpanCustNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "red");
        $(".plus_icon1").css("display", "none")
    }
    else {
        var CustName = $("#CustomerName option:selected").text();
        $("#Hdn_CustName").val(CustName)
        var CustId = $("#CustomerName").val();
        $("#Hdn_CustId").val(CustId)

        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
        $(".plus_icon1").css("display", "block")
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/InterBranchSale/GetCustAddrDetail",
                data: {
                    Cust_id: Cust_id,
                    DocumentMenuId: DocumentMenuId
                },
                success: function (data) {
                    debugger;
                    debugger;
                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            $("#ddlCurrency").val(arr.Table[0].curr_name);
                            $("#Hdn_ddlCurrency").val(arr.Table[0].curr_id);
                            $("#cust_acc_id").val(arr.Table[0].cust_acc_id);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#txtCurrency").val(arr.Table[0].curr_name);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(DecDigit));
                            $("#hdnconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(DecDigit));
                        }
                        else {
                            $("#Address").val("");
                            $("#bill_add_id").val("");
                            $("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                        }
                        if (arr.Table1.length > 0) {
                            $("#Declaration_1").val(arr.Table1[0].declar_1);
                            $("#Declaration_2").val(arr.Table1[0].declar_2);
                            $("#Invoice_Heading").val(arr.Table1[0].inv_heading);
                        }
                        if (arr.Table2.length > 0) {
                            $("#hdncust_trnsportid").val(arr.Table2[0].def_trns_id);
                            var transport = $("#hdncust_trnsportid").val();
                            if (transport != "0" && transport != "" && transport != null) {
                                $("#DdlTranspt_Name").val(transport).trigger('change');
                            }
                            else {
                                $("#DdlTranspt_Name").val("0").trigger('change');
                            }
                        }
                    }
                },
            })
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function AddNewRow() {
    var RowNo = 0;
    var levels = [];
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    AddNewRowForEditScrapItem();
    BindScrpItmList(RowNo);
};
function AddNewRowForEditScrapItem() {
    var rowIdx = 0;
    var rowCount = $('#ScrapSIItemTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
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
    var deletetext = $("#Span_Delete_Title").text();
    var ManualGst = "";
    var TaxExempted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if ($("#nontaxable").is(":checked")) {
        Disable = "disabled";
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" disabled id="ManualGST"><label class="custom-control-label" for="" style="padding:3px 0px;"> </label></div></td>'
        }
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" checked class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted" disabled><label class="custom-control-label" for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    else {
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" for="" style="padding:3px 0px;"> </label></div></td>'
        }
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    $('#ScrapSIItemTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
<td id="srno" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" />
<input class="" type="hidden" id="hfItemID" />
<input class="" type="hidden" id="hdn_item_gl_acc" />
</td>
<input class="" type="hidden" id="ItemName" /></td>
<td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-11 lpo_form no-padding" id='multiWrapper'>
<select class="form-control" id="ItemName${CountRows}" name="ItemName${CountRows}" onchange="OnChangeServiceItemName(${CountRows},event)" ></select><span id="ItemNameError" class="error-message is-visible"></span>
</div>
    <div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
</button></div>
</td>
<td><input id="UOM${RowNo}" class="form-control" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}"  value="" disabled>
<input type="hidden" id="UOMID" value="" />
</td>
<td>
    <div class="lpo_form">
        <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" value="" placeholder="${$("#span_PackSize").text()}">
    </div>
</td>
<td><input id="HsnNo${RowNo}" class="form-control date" autocomplete="off" type="text" name="Hsn"  placeholder="${$("#Hsn").text()}" disabled></td>
   <td><div class="col-sm-11 no-padding"><div class="lpo_form">
   <select class="form-control" id="wh_id${CountRows}" onchange="OnChangeWarehouse(this,event)"></select>
   <span id="wh_Error" class="error-message is-visible"></span>
</div></div>
   <div class="col-sm-1 i_Icon">
   <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
   </div></td>
    <td>
 <div class="col-sm-10 lpo_form no-padding">
     <input id="AvailableStock" value="0.000" class="form-control date num_right" autocomplete="" type="text" name="AvailableStock" required="required" placeholder="0000.00"  disabled="">
  </div>
    <div class="col-sm-2 i_Icon" id="div_SubItemAvlStk">
      <button type="button" id="SubItemAvlStk" Disabled class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" onkeypress="return AmountFloatQty(this,event);" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
    </div>
   </td>
<td><div class="col-sm-10 lpo_form no-padding"><input id="TxtInvoiceQuantity${RowNo}" onpaste = "return CopyPasteAvoidFloat(event)", class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="TxtInvoiceQuantity"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="TxtInvoiceQuantityError${RowNo}" class="error-message is-visible"></span></div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrdQty">
 <input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemOrdQty" Disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Scrap',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> 
</div>
</td>
   <td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" class="calculator " data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
   <input type="hidden" id="hdi_batch" value="" style="display: none;">
   </td>
<td class="center">
  <button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" disabled class="calculator subItmImg " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#BatchSpanDetail").text()}"></i></button>
  <input type="hidden" id="hdi_serial" value="N" style="display: none;">
</td>
<td><div class="lpo_form"><input id="TxtRate${RowNo}" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(1,event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="TxtRate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="TxtRateError${RowNo}" class="error-message is-visible"></span></div></td>
<td class="num_right"><input id="DiscountInPerc" class="form-control date num_right" onchange ="OnChangeDiscountInPerc(event)" onpaste="return CopyPasteData(event)" autocomplete="off" onkeypress="return FloatValuePerOnly(this,event);" type="text" name="DiscountInPerc"  placeholder="0000.00"  onblur="this.placeholder='0000.00'">
<span id="item_disc_percError" class="error-message is-visible"></span></td>
<td class="num_right"><input id="DiscountValue" disabled="disabed" class="form-control date num_right" maxlength="10" autocomplete="off" type="text" name="DiscountValue"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"></td>
<td><input id="TxtItemGrossValue${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="TxtItemGrossValue"  placeholder="0000.00"  disabled></td>
`+ TaxExempted + `
 `+ ManualGst + `
<td><div class="col-sm-10 no-padding"><input id="Txtitem_tax_amt${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div><div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation${RowNo}" disabled onclick="OnClickTaxCalBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${$("#TaxInfo").text()}" data-original-title="${$("#TaxInfo").text()}"></i></button></div></td>
<td><input id="TxtOtherCharge${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="TxtOtherCharge"  placeholder="0000.00"  disabled></td>
<td><input id="NetValueinBase${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="NetValueinBase"  placeholder="0000.00"  disabled></td>
<td><textarea id="txt_ScrpItemRemarks"  class="form-control remarksmessage" maxlength="1500" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="ItemRemarks"  placeholder="Remarks">${$("#span_remarks").val()}</textarea></td>
</tr>`);
    $("#ItemName" + CountRows).focus()
    BindWarehouseList(CountRows);
};
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
function OnChangeServiceItemName(RowID, e) {
    BindServcItemListOnChng(e);
}
async function BindServcItemListOnChng(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var NewItm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    NewItm_ID = clickedrow.find("#ItemName" + SNo).val();
    var itemname = clickedrow.find("#ItemName" + SNo + "option:selected").text();
    var OldItemID = clickedrow.find("#hfItemID").val(); /*Add by Hina on 06-08-2024 for remove sub item data and batch detail data from hdn tables*/
    clickedrow.find("#hfItemID").val(NewItm_ID);
    var ItemID = clickedrow.find("#hfItemID").val();
    if (NewItm_ID == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    //Cmn_DeleteSubItemQtyDetail(ItemID);
    //DeleteItemBatchOrderQtyDetails(ItemID);
    Cmn_DeleteSubItemQtyDetail(OldItemID);/*replace with OldItemID by Hina on 06-08-2024 */
    DeleteItemBatchOrderQtyDetails(OldItemID); /*replace with OldItemID by Hina on 06-08-2024 */
    await ClearRowDetails(e, ItemID);
    DisableHeaderField();
    GetAllGLID();
    try {
        $("#HdnTaxOn").val("Tax");
        Cmn_BindUOM(clickedrow, NewItm_ID, "", "Y", "sale");
    } catch (err) {
        console.log(err.message)
    }
    getsubitm(e, NewItm_ID);
}
function getsubitm(e, ItemID) {
    try {
        var clickedrow = $(e.target).closest("tr");
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/ScrapSaleInvoice/getsubitem",
                data: {
                    ItemID: ItemID
                },
                success: function (data) {
                    var arr = [];
                    if (data != null && data != "") {
                        arr = JSON.parse(data);
                        var Disable_Enable = arr.Table[0].sub_item;
                        var Disable_EnableBatch = arr.Table[0].i_batch;
                        var Disable_EnableSerial = arr.Table[0].i_serial;
                        if (Disable_Enable == "Y") {
                            clickedrow.find("#SubItemAvlStk").attr("Disabled", false);
                            clickedrow.find("#SubItemOrdQty").attr("Disabled", false);
                        }
                        if (Disable_Enable == "N") {
                            clickedrow.find("#SubItemAvlStk").attr("Disabled", true);
                            clickedrow.find("#SubItemOrdQty").attr("Disabled", true);
                        }
                        if (Disable_EnableBatch == "Y") {
                            clickedrow.find("#btnbatchdeatil").attr("Disabled", false);
                            clickedrow.find("#btnbatchdeatil").css("filter", "");
                        }
                        else {
                            clickedrow.find("#btnbatchdeatil").attr("Disabled", true);
                            clickedrow.find("#btnbatchdeatil").css("filter", "grayscale(100%)");
                        }
                        if (Disable_EnableSerial == "Y") {
                            clickedrow.find("#btnserialdeatil").attr("Disabled", false);
                            clickedrow.find("#btnserialdeatil").css("filter", "");
                        }
                        else {
                            clickedrow.find("#btnserialdeatil").attr("Disabled", true);
                            clickedrow.find("#btnserialdeatil").css("filter", "grayscale(100%)");
                        }
                        clickedrow.find("#sub_item").val(Disable_Enable);
                        clickedrow.find("#hdi_batch").val(Disable_EnableBatch);
                        clickedrow.find("#hdi_serial").val(Disable_EnableSerial);
                        clickedrow.find("#ItemName").val(arr.Table[0].item_name);
                    }
                },
            });
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
//-------------------INSERT DETAIL START-------------------//
function InsertSSIDetails() {

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");
    if (CheckSSI_Validations() == false) {
        return false;
    }
    if (CheckSSI_ItemValidations() == false) {
        //swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();/*Add By Hina on 31-07-2024 to if hsn code has no tax slab*/
    if (GstApplicable == "Y" /*&& Dmenu == "105103148"*/) {
        if ($("#nontaxable").is(":checked")) {

        }
        else {
            if (Cmn_taxVallidation("ScrapSIItemTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "ItemName") == false) {
                return false;
            }
        }
    }
    if (CheckSSI_VoucherValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    Batchflag = CheckItemBatchValidation();
    if (Batchflag == false) {
        return false;
    }
    var serialValid = CheckItemSerial_Validation();
    if (serialValid == false) {
        return false;
    }
    //var flag = "N";
    //if ($("#ScrapSIItemTbl >tbody >tr").length > 0) {
    //    $("#ScrapSIItemTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        var ItemType = currentRow.find("#ItemType").val();
    //        if (ItemType == "Stockable") {
    //            flag = "Y";
    //        }
    //    });
    //    if (flag == "N") {
    //        swal("", $("#AtleastOneStockableItemIsRequiredInOrder").text(), "warning");
    //        return false;
    //    }
    //}
    if (CheckTransportDetailValidation() == false) {/*Add By Hina on 24-07-2024 for transporter detail for EInvoce*/
        return false;
    }
    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
    var TransType = "";
    if (INSDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    $("#SupplierName").attr("disabled", false);
    var Narration = $("#CreditNotePassAgainstInv").text()
    $('#hdnVouMsg').val(Narration);

    var SubItemsListArr = Cmn_SubItemList(); /*Added By Nitesh 30-01-2024*/
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    var FinalItemDetail = [];
    FinalItemDetail = InsertSSIItemDetails();
    var str = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(str);

    var TaxDetail = [];
    TaxDetail = InsertTaxDetails();
    var str_TaxDetail = JSON.stringify(TaxDetail);
    $('#hdItemTaxDetail').val(str_TaxDetail);

    /* Insert TCS Details */
    //var TcsDetail = [];
    //TcsDetail = Cmn_Insert_Tcs_Details();
    //var str_TcsDetail = JSON.stringify(TcsDetail);
    //$('#hdn_tcs_details').val(str_TcsDetail);
    ///* Insert TCS Details End */

    var OC_TaxDetail = [];
    OC_TaxDetail = OC_InsertTaxDetails();
    var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
    $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

    var OCDetail = [];
    OCDetail = GetSSI_OtherChargeDetails();
    var str_OCDetail = JSON.stringify(OCDetail);
    $('#hdItemOCDetail').val(str_OCDetail);

    var vou_Detail = [];
    vou_Detail = GetSSI_VoucherDetails();
    var str_vou_Detail = JSON.stringify(vou_Detail);
    $('#hdItemvouDetail').val(str_vou_Detail);

    var Final_OC_TdsDetails = [];
    Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
    var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
    $('#hdn_oc_tds_details').val(Oc_Tds_Details);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);
    BindItemBatchDetails();
    BindItemSerialDetails();
    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfStatus") == false) {
        return false;
    }
    var CustName = $('#CustomerName option:selected').text();
    $("#Hdn_CustName").val(CustName);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

    $("#ddlSalesPerson").attr("disabled", false);
    return true;
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function ForwardBtnClick() {
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DSinvDate = $("#Sinv_dt").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: DSinvDate
            },
            success: function (data) {
                if (data == "TransAllow") {
                    var OrderStatus = "";
                    OrderStatus = $('#hfStatus').val();
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
    return false;
}
async function OnClickForwardOK_Btn() {

    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SaleVouMsg = "";
    var PV_VoucherNarr = $("#hdn_PV_Narration").val();/*Add by Hina on 22-07-2024 to add for third party OC,tds ON third party OC*/
    var BP_VoucherNarr = $("#hdn_BP_Narration").val();
    var DN_VoucherNarr = $("#hdn_DN_Narration").val();
    var DN_VoucherNarr_Tcs = $("#hdn_DN_Narration_Tcs").val();

    Remarks = $("#fw_remarks").val();
    DocNo = $("#InvoiceNumber").val();
    DocDate = $("#Sinv_dt").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $('#ListFilterData1').val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (DocNo + ',' + DocDate + ',' + "Update" + ',' + WF_status1)
    SaleVouMsg = $("#SaleVoucherPassAgainstInv").text()


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    var pdfAlertEmailFilePath = 'DPI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Approve") {

        var list = [{
            DocNo: DocNo, DocDate: DocDate, SaleVouMsg: SaleVouMsg
            , A_Status: "Approve", A_Level: $("#hd_currlevel").val()
            , A_Remarks: Remarks, DN_Nurr_tcs: DN_VoucherNarr_Tcs
            , GstApplicable: $("#Hdn_GstApplicable").text()/* GstApplicable : Added by Suraj Maurya on 26-05-2025 */
        }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/InterBranchSale/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&PV_VoucherNarr=" + PV_VoucherNarr + "&BP_VoucherNarr=" + BP_VoucherNarr + "&DN_VoucherNarr=" + DN_VoucherNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            showLoader();
            window.location.reload();
        }
    }
}
function GetPdfFilePathToSendonEmailAlert(spoNo, spoDate, fileName) {
    debugger;
    var Inv_no = spoNo;
    var InvDate = spoDate;
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var PrintFormat = [];
    PrintFormat.push({
        PrintFormat: $("#PrintFormat").val(),
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        CustAliasName: "N",
        NumberOfCopy: "1",
        ShowWithoutSybbol: $("#ShowWithoutSybbol").val(),
        showDeclare1: $("#showDeclare1").val(),
        showDeclare2: $("#showDeclare2").val(),
        showInvHeading: $("#showInvHeading").val(),
        PrintShipFromAddress: $("#PrintShipFromAddress").val(),
        ShowPackSize: $("#hdn_ShowPackSize").val(),
    })
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InterBranchSale/SavePdfDocToSendOnEmailAlert",
        data: { poNo: Inv_no, poDate: InvDate, fileName: fileName, PrintFormat: JSON.stringify(PrintFormat), GstApplicable: GstApplicable },
        success: function (data) {

        }
    });
}
function CheckItemSerial_Validation() {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ConsumedQuantity = clickedrow.find("#TxtInvoiceQuantity").val();
        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                /*commented byHina on 05-02-2024 to validate by Eye color*/
                //clickedrow.find("#btncserialdeatil").css("border-color", "red");
                ValidateEyeColor(clickedrow, "btnserialdeatil", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#Scrap_lineSerialIssueQty').val();
                    var srialitemid = currentRow.find('#Scrap_lineSerialItemId').val();
                    var srialuomid = currentRow.find('#Scrap_lineSerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(ConsumedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {

                    ValidateEyeColor(clickedrow, "btnserialdeatil", "N");
                }
                else {

                    ValidateEyeColor(clickedrow, "btnserialdeatil", "Y");
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
function InsertSSIItemDetails() {

    var srctyp = $("#hdSrc_type").val();
    var PI_ItemsDetail = [];
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {


        var item_id = "";
        var item_name = "";
        var Hsn_no = "";
        var inv_qty = "";
        var item_rate = "";
        var item_gr_val = "";
        var TaxExempted = "";/*Add TaxExempted,ManualGST by Hina sharma on 13-01-2025*/
        var ManualGST = "";
        var item_tax_amt = "";
        var item_oc_amt = "";
        var item_net_val_bs = "";
        var wh_id = "";
        var UOMID = "";
        var avl_qty = "";
        var item_acc_id = "";
        var itemRemarks = "";/*add by Hina sharma on 04-03-2025*/
        var DiscInPer = "";/*add by Shubham Maurya on 01-04-2025*/
        var DiscVal = "";/*add by Shubham Maurya on 01-04-2025*/
        var sr_no = "";
        var currentRow = $(this);
        SNO = currentRow.find("#SNohiddenfiled").val();
        sr_no = currentRow.find("#srno").text();
        item_id = currentRow.find("#hfItemID").val();
        item_name = currentRow.find("#ItemName" + SNO + "option:selected").text();
        UOMID = currentRow.find("#UOMID").val();
        Hsn_no = currentRow.find("#HsnNo").val();
        wh_id = currentRow.find("#wh_id" + SNO).val();
        avl_qty = currentRow.find("#AvailableStock").val();

        inv_qty = currentRow.find("#TxtInvoiceQuantity").val();
        item_rate = currentRow.find("#TxtRate").val();
        item_gr_val = currentRow.find("#TxtItemGrossValue").val();
        if (currentRow.find("#TaxExempted").is(":checked")) {/*Add TaxExempted,ManualGST by Hina sharma on 13-01-2025*/
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y"
            }
            else {
                ManualGST = "N"
            }

        }
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        item_net_val_bs = currentRow.find("#NetValueinBase").val();
        itemRemarks = currentRow.find("#txt_ScrpItemRemarks").val();/*add by Hina sharma on 04-03-2025*/
        DiscInPer = currentRow.find("#DiscountInPerc").val();/*add by Hina sharma on 04-03-2025*/
        DiscVal = currentRow.find("#DiscountValue").val();/*add by Hina sharma on 04-03-2025*/
        var pack_size = currentRow.find("#PackSize").val();/*add by Hina sharma on 04-03-2025*/

        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, Hsn_no: Hsn_no, inv_qty: inv_qty, item_rate: item_rate,
            item_gr_val: item_gr_val, TaxExempted: TaxExempted, ManualGST: ManualGST, item_tax_amt: item_tax_amt, item_oc_amt: item_oc_amt,
            item_net_val_bs: item_net_val_bs, UOMID: UOMID, wh_id: wh_id, avl_qty: avl_qty,
            item_acc_id: item_acc_id, itemRemarks: itemRemarks, DiscInPer: DiscInPer, DiscVal: DiscVal, sr_no: sr_no, pack_size: pack_size
        });
    });
    return PI_ItemsDetail;
}
function InsertTaxDetails() {
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#ItemName" + RowSNo).val();

            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var item_id = Crow.find("#TaxItmCode").text();
                        var tax_id = Crow.find("#TaxNameID").text();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var tax_rate = Crow.find("#TaxPercentage").text();
                        var tax_level = Crow.find("#TaxLevel").text();
                        var tax_val = Crow.find("#TaxAmount").text();
                        var tax_apply_on = Crow.find("#TaxApplyOnID").text();
                        var tax_recov = Crow.find("#TaxRecov").text();
                        var tax_acc_id = Crow.find("#TaxAccId").text();
                        var tax_apply_onName = Crow.find("#TaxApplyOn").text();
                        var totaltax_amt = Crow.find("#TotalTaxAmount").text();
                        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_recov: tax_recov, tax_acc_id: tax_acc_id, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
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


        var item_id = "";
        var tax_id = "";
        var TaxName = "";
        var tax_rate = "";
        var tax_level = "";
        var tax_val = "";
        var tax_apply_on = "";
        var tax_apply_onName = "";
        var totaltax_amt = "";
        var currentRow = $(this);

        item_id = currentRow.find("#TaxItmCode").text();
        tax_id = currentRow.find("#TaxNameID").text();
        TaxName = currentRow.find("#TaxName").text().trim();
        tax_rate = currentRow.find("#TaxPercentage").text();
        tax_level = currentRow.find("#TaxLevel").text();
        tax_val = currentRow.find("#TaxAmount").text();
        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
        totaltax_amt = currentRow.find("#TotalTaxAmount").text();
        TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    });
    return TaxDetails;
}
function GetSSI_OtherChargeDetails() {
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
            let supp_type = currentRow.find("#td_OCSuppType").text();
            let bill_no = currentRow.find("#OCBillNo").text();
            let bill_date = currentRow.find("#OCBillDt").text()//.split("-"); 
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
function GetSSI_VoucherDetails() {
    var PI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    var TransType = "Sal";
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
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            if (bill_date == null || bill_date == "null") {
                bill_date = "";
            }
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
function CheckSSI_Validations() {

    var ErrorFlag = "N";
    if ($("#CustomerName").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#CustomerName").css("border-color", "Red");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
        $("#CustomerName").css("border-color", "#ced4da");
    }
    if ($("#ddlSalesPerson").val() === "0" || $("#ddlSalesPerson").val() === "" || $("#ddlSalesPerson").val() === null) {
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $("#ddlSalesPerson").css("border-color", "red");
        $("#SpanSalesPersonErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    } else {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
}
function CheckTransportDetailValidation() {/*Add By Hina on 22-04-2024 for Transport Detail*/

    var ErrorFlag = "N";
    var gr_no = $("#GRNumber").val();
    var GRDate = $("#GRDate").val();
    var txttrpt_name = $("#DdlTranspt_Name").val();
    var txtveh_number = $("#TxtVeh_Number").val();
    var NoOfPacks = $("#NoOfPacks").val();

    if (gr_no == "" || gr_no == "0") {
        $("#Span_GRNumber").text($("#valueReq").text());
        $("#Span_GRNumber").css("display", "block");
        $("#GRNumber").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (GRDate == "" || GRDate == "0") {
        $("#Span_GRDate").text($("#valueReq").text());
        $("#Span_GRDate").css("display", "block");
        $("#GRDate").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (parseFloat(CheckNullNumber(NoOfPacks)) == 0) {
        $("#Span_No_Of_Packages").text($("#valueReq").text());
        $("#Span_No_Of_Packages").css("display", "block");
        $("#NoOfPacks").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (txttrpt_name == "" || txttrpt_name == "0") {
        $("#Span_TransporterName").text($("#valueReq").text());
        $("#Span_TransporterName").css("display", "block");
        //$("#TxtTranspt_Name").css("border-color", "red");
        $("[aria-labelledby='select2-DdlTranspt_Name-container']").css("border-color", "red");
        ErrorFlag = "Y";
    }
    //if (txtveh_number == "" || txtveh_number == "0") {
    //    $("#Span_VehicleNumber").text($("#valueReq").text());
    //    $("#Span_VehicleNumber").css("display", "block");
    //    $("#TxtVeh_Number").css("border-color", "red");
    //    ErrorFlag = "Y";
    //}

    if (ErrorFlag == "Y") {
        swal("", $("#TransporterDetailNotFound").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function CheckSSI_ItemValidations() {
    var RateDecDigit = $("#RateDigit").text();
    var ErrorFlag = "N";
    //var flag = "N";
    if ($("#ScrapSIItemTbl >tbody >tr").length > 0) {
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            //var ItemType = currentRow.find("#ItemType").val();
            //if (ItemType == "Stockable") {
            //    flag = "Y";
            //}
            if (currentRow.find("#ItemName" + Sno).val() == "0") {
                currentRow.find("#ItemNameError").text($("#valueReq").text());
                currentRow.find("#ItemNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ItemNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#TxtInvoiceQuantity").val() == "") {
                currentRow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
                currentRow.find("#TxtInvoiceQuantityError").css("display", "block");
                currentRow.find("#TxtInvoiceQuantity").css("border-color", "red");
                currentRow.find("#TxtInvoiceQuantity").focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtInvoiceQuantityError").css("display", "none");
                currentRow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
            }
            var inv_qty = currentRow.find("#TxtInvoiceQuantity").val()
            var avl_qty = currentRow.find("#AvailableStock").val()
            // clickedrow.find("#TxtInvoiceQuantity").val(parseFloat(inv_qty).toFixed(QtyDecDigit));
            if (currentRow.find("#TxtInvoiceQuantity").val() != "" && currentRow.find("#TxtInvoiceQuantity").val() != null) {
                //if (ItemType != "Service") {
                    if (parseFloat(inv_qty) > parseFloat(avl_qty)) {
                        currentRow.find("#TxtInvoiceQuantityError").text($("#ExceedingQty").text());
                        currentRow.find("#TxtInvoiceQuantityError").css("display", "block");
                        currentRow.find("#TxtInvoiceQuantity").css("border-color", "red");
                        currentRow.find("#TxtInvoiceQuantity").focus();
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#TxtInvoiceQuantityError").css("display", "none");
                        currentRow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
                    }
                //}

            }
            if (currentRow.find("#TxtRate").val() == "") {
                currentRow.find("#TxtRateError").text($("#valueReq").text());
                currentRow.find("#TxtRateError").css("display", "block");
                currentRow.find("#TxtRate").css("border-color", "red");
                if (currentRow.find("#TxtInvoiceQuantity").val() != "" && currentRow.find("#TxtInvoiceQuantity").val() != null) {
                    currentRow.find("#TxtRate").focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#TxtRateError").css("display", "none");
                currentRow.find("#TxtRate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#TxtRate").val() != "") {
                if (parseFloat(currentRow.find("#TxtRate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#TxtRateError").text($("#valueReq").text());
                    currentRow.find("#TxtRateError").css("display", "block");
                    currentRow.find("#TxtRate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#TxtRateError").css("display", "none");
                    currentRow.find("#TxtRate").css("border-color", "#ced4da");
                }
            }
            var ddlId = "#wh_id" + Sno;
            var whERRID = "#wh_Error";
            //if (ItemType == "Stockable" || ItemType == "") {
                if (currentRow.find(ddlId).val() == "0") {
                    currentRow.find(whERRID).text($("#valueReq").text());
                    currentRow.find("#wh_id" + Sno).css("border-color", "Red");
                    currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border-color", "red");
                    currentRow.find(whERRID).css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    var WHName = $("#wh_id option:selected").text();
                    $("#hdwh_name").val(WHName)
                    currentRow.find(whERRID).css("display", "none");
                    currentRow.find('[aria-labelledby="select2-' + "wh_id" + Sno + '-container"]').css("border-color", "#ced4da");
                    currentRow.find(ddlId).css("border-color", "#ced4da");
                }
            //}
            if (currentRow.find(ddlId).val() != "0") {
            }
            else {
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
function CheckSSI_VoucherValidations() {
    if (Cmn_CheckGlVoucherValidations() == false) {
        return false;
    } else {
        return true;
    }
}
function CheckItemBatchValidation() {

    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var clickedrow = $(this);
        var inv_qty = clickedrow.find("#TxtInvoiceQuantity").val();
        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Batchable = clickedrow.find("#hdi_batch").val();
        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#scrap_lineBatchINVQty').val();
                    var bchitemid = currentRow.find('#scrap_lineBatchItemId').val();
                    var bchuomid = currentRow.find('#scrap_lineBatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });
                if (parseFloat(inv_qty).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
                }
                else {
                    ValidateEyeColor(clickedrow, "btnbatchdeatil", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#BatchQtydoesnotmatchwithInvoiceQty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#BatchQtydoesnotmatchwithInvoiceQty").text(), "warning");
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
function CheckItemSerialValidation() {
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var clickedrow = $(this);
        var inv_qty = clickedrow.find("#TxtInvoiceQuantity").val();
        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var Serialable = clickedrow.find("#hdi_serial").val();
        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#mi_lineSerialIssueQty').val();
                    var srialitemid = currentRow.find('#mi_lineSerialItemId').val();
                    var srialuomid = currentRow.find('#mi_lineSerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });
                if (parseFloat(inv_qty).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
                }
                else {
                    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
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
function OtherFunctions(StatusC, StatusName) {
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
            return true;
        } else {
            return false;
        }
    });
    return false;
}
//-------------------INSERT DETAIL END-------------------//
function DeleteItemBatchOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#scrap_lineBatchItemId").val();
        if (rowitem == Itemid) {
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        debugger;
        var HdnItemId = row.find("#Scrap_lineSerialItemId").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    });
}
async function ClearRowDetails(e, ItemID) {
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#HsnNo").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#TxtInvoiceQuantity").val("");
    clickedrow.find("#TxtRate").val("");
    clickedrow.find("#TxtItemGrossValue").val("");
    clickedrow.find("#Txtitem_tax_amt").val("");
    clickedrow.find("#TxtOtherCharge").val("");
    clickedrow.find("#NetValueinBase").val("");
    clickedrow.find("#AvailableStock").val("");
    clickedrow.find("#wh_id" + SNo).val(0).trigger('change.select2');
    var ddlId = "#wh_id" + SNo;
    var whERRID = "#wh_Error";
    clickedrow.find(whERRID).css("display", "none");
    //clickedrow.find(ddlId).css("border-color", "#ced4da");
    clickedrow.find('[aria-labelledby="select2-' + "wh_id" + SNo + '-container"]').css("border-color", "#ced4da");
    clickedrow.find(ddlId).css("border-color", "#ced4da");
    CalculateAmount();
    var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetSI_ItemTaxDetail();

}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
            }
        } else {
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCValue = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
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
        var Sno = 0;
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            Sno = Sno + 1;
            var OCValue = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        var Sno = 0;
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            Sno = Sno + 1;
            var currentRow = $(this);
            var OCValue = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var POItm_GrossValue = CheckNullNumber(currentRow.find("#TxtItemGrossValue").val());
        var POItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        var POItm_OCAmt = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0.00";
        }
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        FinalPOItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(DecDigit);
        currentRow.find("#NetValueinBase").val((parseFloat(CheckNullNumber(FinalPOItm_NetOrderValueBase))).toFixed(DecDigit));
    });
    CalculateAmount();
};
function DisableHeaderField() {
    $("#CustomerName").attr('disabled', true);
}
function OnClickIconBtn(e) {
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#ItemName" + Sno).val();
    ItemInfoBtnClick(ItmCode)
}
function FloatValuePerOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    return true;
}
function BindScrpItmList(ID) {
    BindPItemList("#ItemName", ID, "#ScrapSIItemTbl", "#SNohiddenfiled", "", "IBS");
}
function BindPItemList(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
        $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableItemDDLPO(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName)

        });
    }
    else {
        DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName)
    }
    if (OtherFx == "BindData") {
        BindData();
    }
}
function DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    $(ItmDDLName + RowID).select2({

        ajax: {
            url: "/Common/Common/GetItemList2",
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
                    pageSize = 20;//50000; // or whatever pagesize

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
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name.split("_")[0], UOM: val.ID.split("_")[1] };
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
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function GetAllGLID(flag) {
    GetAllGL_WithMultiSupplier(flag);
}
async function GetAllGL_WithMultiSupplier(flag) {
    debugger;
    console.log("GetAllGL_WithMultiSupplier")
    if ($("#ScrapSIItemTbl > tbody > tr").length == 0) {
        $("#VoucherDetail tbody tr").remove();
        $("#DrTotalInBase").text("");
        $("#CrTotalInBase").text("");
        return false;
    }
    //if (CheckSPI_ItemValidationsForGL() == false) {
    //    return false;
    //}
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


    var cust_id = $("#CustomerName").val();
    var cust_acc_id = $("#cust_acc_id").val();
    var CustVal = 0;
    var CustValInBase = 0;
    /*CustValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);*/
    CustValInBase = (parseFloat(NetInvValue) - parseFloat(NetTcsValue)).toFixed(ValDecDigit);//Changed by Suraj Maurya on 07-02-2025 for substract tcs.
    CustVal = parseFloat((parseFloat(CustValInBase) / parseFloat(conv_rate))).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";

    var curr_id = $("#hdcurr").val();
    var bill_no = $("#Bill_No").val();
    var bill_dt = $("#Bill_Date").val();
    var TransType = 'Sal';
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: cust_id, type: "Cust", doctype: InvType, Value: CustVal, ValueInBase: CustValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Cust", parent: 0, Entity_id: cust_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
    });
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {

        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
        var ItemAccId = currentRow.find("#hdn_item_gl_acc").val()
        var ItmGrossValInBase = currentRow.find("#TxtItemGrossValue").val();

        //var ItemTaxAmt = currentRow.find("#item_tax_amt").val()/*Commented by Hina sharma on 10-01-2025*/
        //var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
        /*---Code start Add by Hina sharma on 10-01-2025 for TaxExempted*/
        var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
        var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
        var TxaExanted = "N";
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExanted = "Y";
            TxaExantedItemList.push({ item_id: item_id/*, doc_no: TxtGrnNo*/ });
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id/*, doc_no: TxtGrnNo*/ });
            }
        }
        /*---Code End Add by Hina sharma on 10-01-2025 for TaxExempted*/
        GLDetail.push({
            comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
            , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: cust_acc_id
            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ItemAccId
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
        var oc_amt_wt = parseFloat((parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate))).toFixed(ValDecDigit); //with tax

        var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? cust_acc_id : oc_supp_acc_id;

        GLDetail.push({
            comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt
            , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: IsNull(oc_supp_bill_no, '')
            , bill_date: IsNull(oc_supp_bill_dt, ''), acc_id: ""
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
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: IsNull(oc_supp_bill_no, '')
                , bill_date: IsNull(oc_supp_bill_dt, ''), acc_id: ""
            });
        } else {
            //if (GLDetail.findIndex((obj => obj.id == cust_id)) > -1) {
            //    objIndex = GLDetail.findIndex((obj => obj.id == cust_id));
            //    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(oc_amt_wt);
            //    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(oc_amt_bs_wt);
            //}
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
            bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date, ''), acc_id: ""
        });
    });
    //$("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {/*Commented and modify by hina sharma on 10-01-2025*/
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#taxID").text();
    //    var tax_acc_id = currentRow.find("#taxAccID").text();
    //    var tax_amt = currentRow.find("#TotalTaxAmount").text();
    //    var TaxPerc = currentRow.find("#TaxPerc").text();
    //    GLDetail.push({
    //        comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
    //        , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: cust_acc_id
    //        , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt,''), acc_id: ""
    //    });
    //});
    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        debugger
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();
        var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
        var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
        if (TaxExempted == false) {
            debugger
            if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
                var GstApplicable = $("#Hdn_GstApplicable").text();

                //if (TaxRecov == "N" /*|| !ClaimItc*/) {
                //    if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
                //        var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
                //        GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
                //        GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
                //    }
                //}
                //else {
                GLDetail.push({
                    comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                    , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: cust_acc_id
                    , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
                });
                //}
            }
        }

    });
    /*--------------------For Third Party TDS on Other charge code Start by Hina Sharma on 04-07-2024-----------------*/
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // 
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        //tds_amt = parseFloat(tds_amt).toFixed(0);
        Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
        GLDetail.push({
            comp_id: Compid, id: tds_acc_id, type: "Tcs", doctype: InvType, Value: tds_amt
            , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tcs", parent: cust_acc_id
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
        });
    });
    if (Cal_Tds_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: cust_id, type: "TCust", doctype: InvType
            , Value: Cal_Tds_Amt, ValueInBase: Cal_Tds_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TCust", parent: 0, Entity_id: cust_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
        });
    }

    var Oc_Tds = [];
    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // 
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
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
            });
        } else {
            Oc_Tds.push({
                supp_id: tds_supp_id, supp_acc_id: ArrOcGl[0].id, tds_amt: tds_amt
                , bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date, '')
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date, ''), acc_id: ""
            });
        }



    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item, idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: IsNull(item.bill_no, ''), bill_date: IsNull(item.bill_date, ''), acc_id: ""
            });
        });

    }
    /*--------------------For Third Party TDS on Other charge code End-----------------*/



    if (GstCat == "UR") {
        debugger;
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {

            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            var DocNo = currentRow.find("#DocNo").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            debugger
            /*Commented and modify by Hina sharma on 10-01-2025 for tax expted and manual gst*/
            if (TxaExantedItemList.findIndex((obj => (obj.item_id + obj.doc_no) == (TaxItmCode + DocNo))) == -1) {
                if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
                    objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
                    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                } else {
                    GLDetail.push({
                        comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal
                        , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: cust_acc_id
                        , Entity_id: TaxAccID, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""

                    });
                }
            }
        });
    }
    await Cmn_GLTableBind(cust_acc_id, GLDetail, "Sales");
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    let Narration = "";
    switch (VouType) {
        case "DN":
            Narration = $("#hdn_DN_Narration").val();
            break;
        case "BP":
            Narration = $("#hdn_BP_Narration").val();
            break;
        case "PV":
            Narration = $("#hdn_PV_Narration").val();
            break;
        case "CN":
            Narration = $("#hdn_CN_Narration").val();
            break;
        case "SV":
            Narration = $("#hdn_SaleVouMsg").val();
            break;
        default:
            Narration = $("#hdn_SaleVouMsg").val();
            break;
    }
    return (Narration).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}
function CheckedCancelled() {
    debugger;
    $("#HdDeleteCommand").val('');
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);

        $("#btn_save").attr("onclick", "return  InsertSSIDetails();");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
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
function ItemStockWareHouseWise(el, evt) {
    try {
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hfItemID").val();
        var ItemName = clickedrow.find("#ItemName").val().trim();
        var UOMName = clickedrow.find("#UOM").val();
        $.ajax(
            {
                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId
                },
                success: function (data) {
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
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
        subitemInvoiceqty(clickedrow)
    }
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#ItemName").val().trim();
    var ProductId = Crow.find("#hfItemID").val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStock").val();
    var hdwh_Id = Crow.find("#wh_id" + rowNo).val();
    Cmn_SubItemWareHouseAvlStock((ProductNm).trim(), ProductId, UOM, hdwh_Id, AvlStk, "wh");
}
function SubItemDetailsPopUp(flag, e) {
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#ItemName").val().trim();
    var ProductId = clickdRow.find("#hfItemID").val();
    var wh_id = clickdRow.find("#wh_id" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
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
        List.avlqty = row.find('#avlqty').val();
        List.qty = row.find('#subItemQty').val();
        NewArr.push(List);
    });
    if (flag == "Scrap") {
        Sub_Quantity = clickdRow.find("#TxtInvoiceQuantity").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InterBranchSale/GetSubItemDetails",
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
    })
}
function subitemInvoiceqty(clickdRow) {


    // var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#ItemName" + hfsno).text();
    var ProductId = clickdRow.find("#hfItemID").val();
    Cmn_DeleteSubItemQtyDetail(ProductId);
    var wh_id = clickdRow.find("#wh_id" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var UOMID = clickdRow.find("#UOMID").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ScrapSaleInvoice/GetSubitemdata",
        data: {
            Item_id: ProductId,
            wh_id: wh_id,
            UomId: UOMID
        },
        success: function (data) {

            if (data == 'ErrorPage') {
                //GRN_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.length > 0) {
                    for (var y = 0; y < arr.length; y++) {
                        var ItmId = arr[y].item_id;
                        var SubItmId = arr[y].sub_item_id;
                        var SubItmName = arr[y].sub_item_name;
                        var avl_stk = arr[y].avl_stck;
                        $("#Sub_ProductlName").val(arr[y].item_name);
                        $("#hdn_Sub_ItemDetailTbl tbody").append(
                            `<tr>
                                                <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                                <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                                <td><input type="text" id="avlqty" value='${avl_stk}'></td>
                                                <td><input type="text" id="subItemQty" value=''></td>                                               
                                            </tr>`);
                    }
                }
            }
        }
    })
}
function Fn_AfterTaxChangeUpdateNetValueItemWise(currentRow, total_tax_val) {/*Add by Hina Sharma on 10-01-2025*/
    debugger;
    if (!currentRow.find("#TaxExempted").is(":checked")) {
        currentRow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(total_tax_val)).toFixed(cmn_ValDecDigit));
        let ItemGrVal = parseFloat(CheckNullNumber(currentRow.find("#TxtItemGrossValue").val()));
        let ItemOcVal = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()));
        let ItemNetVal = ItemGrVal + ItemOcVal + parseFloat(CheckNullNumber(total_tax_val));
        currentRow.find("#NetValueinBase").val(ItemNetVal.toFixed(cmn_ValDecDigit));
    }
}
async function CalculateAmount(flag) {

    var DecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    var cust_id = $("#CustomerName").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#ScrapSIItemTbl >tbody >tr").each(function () {

        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        }

        //if (currentRow.find("#TxtNetValue").val() == "" || currentRow.find("#TxtNetValue").val() == null || currentRow.find("#TxtNetValue").val() == "NaN") {
        //    NetOrderValueInBase = (parseFloat(NetOrderValueInBase) + parseFloat(0)).toFixed(DecDigit);
        //}
        //else {
        //    NetOrderValueInBase = (parseFloat(NetOrderValueInBase) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(DecDigit);
        //}
        if (currentRow.find("#NetValueinBase").val() == "" || currentRow.find("#NetValueinBase").val() == null || currentRow.find("#NetValueinBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#NetValueinBase").val())).toFixed(DecDigit);
        }

    });
    /*$("#TxtGrossValue").val(GrossValue).trigger('change');*//*commented and modify by Hina sharma on 10-01-2025 same as SPI*/
    $("#TxtTaxAmount").val(TaxValue);
    //$("#NetOrderValueInBase").val(NetOrderValueBase);
    var oc_amount = $("#TxtDocSuppOtherCharges").val();/*changes by Hina on 22-07-2024 for 3rd party OC and tds on 3rd party OC */
    var tcs_amt = $("#TxtTDS_Amount").val();

    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amount)) + parseFloat(CheckNullNumber(tcs_amt));
    $("#NetOrderValueInBase").val(parseFloat(NetOrderValueBase).toFixed(DecDigit));
    $("#TxtGrossValue").val(GrossValue).trigger('change');

    if (flag == "CallByGetAllGL") {
        ApplyRoundOff("CallByGetAllGL");
    }
}
//----------------Transport detail Start-------------------------------//
function OnChangeGRNumber() {
    $("#Span_GRNumber").css("display", "none");
    $("#GRNumber").css("border-color", "#ced4da");
}
function OnChangeGRDate() {
    var GRDate = $("#GRDate").val();
    $("#hdnGrDt").val(moment(GRDate).format('YYYY-MM-DD'));
    $("#Span_GRDate").css("display", "none");
    $("#GRDate").css("border-color", "#ced4da");

}
function OnChangeTransporterName() {
    var trpt_name = $("#DdlTranspt_Name option:selected").text();
    $("#hdnTrnasName").val(trpt_name)
    $("#Span_TransporterName").css("display", "none");
    $("[aria-labelledby='select2-DdlTranspt_Name-container']").css("border-color", "#ced4da");
}
function OnChangeVehicleNumber() {
    $("#Span_VehicleNumber").css("display", "none");
    $("#TxtVeh_Number").css("border-color", "#ced4da");
}
function OnChangeNoOfPackages(el, e) {
    var QtyDecDigit = $("#QtyDigit").text();
    FreightAmount = $("#NoOfPacks").val();
    if (parseFloat(CheckNullNumber(FreightAmount)) > 0) {
        $("#NoOfPacks").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));
    }
    $("#Span_No_Of_Packages").css("display", "none");
    $("#NoOfPacks").css("border-color", "#ced4da");
}
//----------------Transport detail End-------------------------------//
function AfterDeleteResetSI_ItemTaxDetail() {
    var ScrapSIItemTbl = "#ScrapSIItemTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ItemName = "#ItemName";
    CMNAfterDeleteReset_ItemTaxDetailModel(ScrapSIItemTbl, SNohiddenfiled, ItemName);
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
function To_RoundOff(Amount, type) {

    var netval = Amount.toString();
    var fnetval = 0;

    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if (type == "P") {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
            }
            if (type == "M") {
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
            }
            if (type == "P" || type == "M") {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(cmn_ValDecDigit);
                return f_netval;
            }
        }
    }
    return Amount;
}
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105103148");
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
                        var selected = $("#Acc_name_" + (rowid - 1)).val();
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
                rowid = parseFloat(rowid) + 1;
            });
        }
    }
    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        if (AccID != '0' && AccID != "") {
            currentRow.find("#Acc_name_" + rowid).attr("onchange", "");
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#Acc_name_" + rowid).attr("onchange", "OnChangeAccountName(" + rowid + ", event)");
        }
    });
}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#TxtRateError").css("display", "none");
    clickedrow.find("#TxtRate").css("border-color", "#ced4da");
    return true;
}
async function OnChangePOItemQty(RowID, e) {
    await CalculationBaseQty(e);
    GetAllGLID();
}
async function OnChangePOItemRate(RowID, e) {
    await CalculationBaseRate(e);
    GetAllGLID();
}
async function CalculationBaseQty(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisPer;
    OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    ItemName = clickedrow.find("#ItemName" + Sno).val();
    ItmRate = clickedrow.find("#TxtRate").val();
    DisPer = clickedrow.find("#DiscountInPerc").val();
    if (ItemName == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#TxtInvoiceQuantity").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    var inv_qty = clickedrow.find("#TxtInvoiceQuantity").val();
    var avl_qty = clickedrow.find("#AvailableStock").val();
    clickedrow.find("#TxtInvoiceQuantity").val(parseFloat(inv_qty).toFixed(QtyDecDigit));
    if (parseFloat(inv_qty) > parseFloat(avl_qty)) {
        clickedrow.find("#TxtInvoiceQuantityError").text($("#ExceedingQty").text());
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
        clickedrow.find("#TxtInvoiceQuantity").focus();
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (OrderQty != "" && OrderQty != ".") {
        OrderQty = parseFloat(OrderQty);
    }
    if (OrderQty == ".") {
        OrderQty = 0;
    }
    if (OrderQty == "" || OrderQty == 0 || ItemName == "0") {
        clickedrow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
        clickedrow.find("#TxtInvoiceQuantity").val("");
        clickedrow.find("#TxtInvoiceQuantity").focus();
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtInvoiceQuantityError").text("");
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    }
    var OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    var ItmRate = clickedrow.find("#TxtRate").val();
    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#TxtInvoiceQuantity").val("");
        OrderQty = 0;
    }
    debugger;
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#TxtItemGrossValue").val(FinVal);
        FinalFinVal = ConvRate * FinVal
        clickedrow.find("#NetValueinBase").val(parseFloat(FinalFinVal).toFixed(ValDecDigit));
        CalculateAmount();
    }
    clickedrow.find("#TxtInvoiceQuantity").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
    debugger;
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00" && DisPer !== "0" && DisPer !== "0.000" && DisPer !== "0.0000") {
        CalculateDisPercent(clickedrow);
    }
    OnChangeGrossAmt();
    if ($("#nontaxable").is(":checked")) {

    }
    else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    var assVal = clickedrow.find("#TxtItemGrossValue").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if ($("#nontaxable").is(":checked")) {

        }
        else {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#TxtItemGrossValueError").css("display", "none");
        clickedrow.find("#TxtItemGrossValue").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    //let cust_id = $("#CustomerName").val();
    //let GrossValue = $("#TxtGrossValue").val();
    //await AutoTdsApply(cust_id, GrossValue).then(() => {
    //}).catch(err => console.log(err));
}
async function CalculationBaseRate(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var docid = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisPer;

    OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    ItemName = clickedrow.find("#ItemName" + Sno).val();
    ItmRate = clickedrow.find("#TxtRate").val();
    DisPer = clickedrow.find("#DiscountInPerc").val();

    if (ItemName == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#TxtRate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        clickedrow.find("#TxtInvoiceQuantityError").text($("#valueReq").text());
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
        clickedrow.find("#TxtInvoiceQuantity").val("");
        clickedrow.find("#TxtInvoiceQuantity").focus();
        clickedrow.find("#TxtRate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
        clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {
        clickedrow.find("#TxtRateError").text($("#valueReq").text());
        clickedrow.find("#TxtRateError").css("display", "block");
        clickedrow.find("#TxtRate").css("border-color", "red");
        clickedrow.find("#TxtRate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#TxtRate").focus();
        }
        clickedrow.find("#TxtOtherCharge").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#TxtRateError").css("display", "none");
        clickedrow.find("#TxtRate").css("border-color", "#ced4da");
    }
    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#TxtRate").val("");
        ItmRate = 0;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinVal);
        FinalVal = FinVal * ConvRate
        clickedrow.find("#NetValueinBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00" && DisPer !== "0" && DisPer !== "0.000" && DisPer !== "0.0000") {
        CalculateDisPercent(clickedrow);
    }
    clickedrow.find("#TxtRate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    if ($("#nontaxable").is(":checked")) {

    }
    else if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (clickedrow.find("#ManualGST").is(":checked")) {
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
            }
            else {
                CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
            }
        }
    }
    var assVal = clickedrow.find("#TxtItemGrossValue").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if ($("#nontaxable").is(":checked")) {

        }
        else {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#TxtItemGrossValueError").css("display", "none");
        clickedrow.find("#TxtItemGrossValue").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    //let cust_id = $("#CustomerName").val();
    //let GrossValue = $("#TxtGrossValue").val();
    //await AutoTdsApply(cust_id, GrossValue);
}
function OnChangeGrossAmt() {

    /* var TotalOCAmt = $('#Total_OC_Amount').text();*/
    //var TotalOCAmt = $('#_OtherChargeTotalAmt').text();//Commented by Suraj Maurya on 06-01-2025 
    var TotalOCAmt = $('#TxtDocSuppOtherCharges').val();
    var Total_PO_OCAmt = $('#TxtOtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
        //if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {

        Calculate_OC_AmountItemWise(TotalOCAmt);
        //}
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    var DecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                if (TaxItemID == ItmCode) {
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
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    currentRow.find("#TaxAmount ").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount ").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
                if (TaxExempted == false) {
                    NewArray.push({TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID
                        , TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount
                        , TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID
                    });
                }
            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#ScrapSIItemTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo
                ItemNo = currentRow.find("#ItemName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val();
                            if ($("#nontaxable").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            else if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                                NewArray = NewArray.filter(v => v.TaxItmCode != ItemNo);
                            }
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit);
                                    NewArray = NewArray.filter(v => v.TaxItmCode != ItemNo);
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                currentRow.find("#Txtitem_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#NetValueinBase").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));

                            }
                        });
                    }
                    else {
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        FinalFGrossAmtOR = FGrossAmtOR
                        currentRow.find("#NetValueinBase").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}
function OnClickbillingAddressIconBtn(e) {
    var Cust_id = $('#CustomerName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var CustPros_type = "C";
    var status = $("#hfStatus").val().trim();
    var SSIDTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SSIDTransType = "Update";
    }
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, SSIDTransType, '');
}
function OnClickShippingAddressIconBtn(e) {
    $('#AddressInfo_LblName').text($('#AddressInfo_ShipAdd').text());
    var Cust_id = $('#CustomerName').val();
    var ship_add_id = $('#ship_add_id').val().trim();
    $('#hd_add_type').val("S");
    var CustPros_type = "C";
    var status = $("#hfStatus").val().trim();
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }
    var SO_no = $("#InvoiceNumber").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, ship_add_id, status, SODTransType, SO_no);
}
function CheckValidations_forSubItems() {
    return Cmn_CheckValidations_forSubItems("ScrapSIItemTbl", "SNohiddenfiled", "ItemName", "TxtInvoiceQuantity", "SubItemOrdQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("ScrapSIItemTbl", "SNohiddenfiled", "ItemName", "TxtInvoiceQuantity", "SubItemOrdQty", "N");
}
function ItemStockBatchWise(el, evt) {
    try {

        var clickedrow = $(evt.target).closest("tr");
        var inv_qty = clickedrow.find("#TxtInvoiceQuantity").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var TxtInvoiceQuantity = clickedrow.find("#TxtInvoiceQuantity").val();

        var ItemId = clickedrow.find("#hfItemID").val();
        var UOMId = clickedrow.find("#UOMID").val();
        var HdnitmRJOFlag = clickedrow.find("#hdn_RwkJobOrderFlag").val();
        if (AvoidDot(TxtInvoiceQuantity) == false) {
            TxtInvoiceQuantity = "";
        }
        var MRSNo = $('#ddlMRS_No option:selected').text();
        debugger;
        debugger;
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#_ModelCommand").val();
        var TransType = $("#hdn_TransType").val();
        if (parseFloat(inv_qty) == "0" || parseFloat(inv_qty) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#TxtInvoiceQuantityError").text($("#FillQuantity").text());
            clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
            clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/InterBranchSale/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        doc_status: srcp_Status,
                        SelectedItemdetail: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType,

                    },
                    success: function (data) {

                        $('#ItemStockBatchWise').html(data);

                    },
                });
        }
        else {
            var srcp_Status = $("#hfStatus").val();
            if (srcp_Status == "" || srcp_Status == null || srcp_Status == "D") {
                BindItemBatchDetails();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                var ddlId = "#wh_id" + Index;
                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/InterBranchSale/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            doc_status: srcp_Status,
                            SelectedItemdetail: SelectedItemdetail,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            MRSNo: MRSNo,
                            HdnitmRJOFlag: HdnitmRJOFlag,
                            UomId: UOMId
                        },
                        success: function (data) {
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(TxtInvoiceQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");
                            try {
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", TxtInvoiceQuantity, "AvailableQuantity", "BatchInvoiceQty", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }
            else {
                var Doc_No = $("#InvoiceNumber").val();
                var Doc_dt = $("#Sinv_dt").val();
                var WarehouseId = clickedrow.find(ddlId).val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/InterBranchSale/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            Doc_No: Doc_No,
                            Doc_dt: Doc_dt,
                            ItemID: ItemId,
                            doc_status: srcp_Status,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            UomId: UOMId,
                            WarehouseId: WarehouseId,
                        },
                        success: function (data) {
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(TxtInvoiceQuantity);
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
function onclickbtnItemBatchSaveAndExit() {
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var BatchInvoiceQty = row.find("#BatchInvoiceQty").val();
        if (parseFloat(CheckNullNumber(BatchInvoiceQty)) > parseFloat(AvailableQuantity)) {
            row.find("#BatchInvoiceQty_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#BatchInvoiceQty_Error").css("display", "block");
            row.find("#BatchInvoiceQty").css("border-color", "red");
            ValidateEyeColor(row, "btnbatchdeatil", "Y");
            IsuueFlag = false;
        }
    });
    if (IsuueFlag) {
        if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
            swal("", $("#BatchQtydoesnotmatchwithInvoiceQty").text(), "warning");
        }
        else {
            var SelectedItem = $("#HDItemNameBatchWise").val();
            $("#SaveItemBatchTbl TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#scrap_lineBatchItemId").val();
                if (rowitem == SelectedItem) {
                    $(this).remove();
                }
            });
            $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                var row = $(this);
                var BatchInvoiceQty = row.find("#BatchInvoiceQty").val();
                if (CheckNullNumber(BatchInvoiceQty) > 0) {
                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    var MfgName = row.find("#BtMfgName").val();
                    var MfgMrp = row.find("#BtMfgMrp").val();
                    var MfgDate = row.find("#BtMfgDate").val();
                    var ScrapSIItemTblRow = $("#ScrapSIItemTbl > tbody > tr #hfItemID[value='" + ItemId + "']").closest('tr');
                    var sr_no = ScrapSIItemTblRow.find("#SNohiddenfiled").val();
                    var bt_wh_id = ScrapSIItemTblRow.find("#wh_id" + sr_no).val();;
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="scrap_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="scrap_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="scrap_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="scrap_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="scrap_lineBatchINVQty" value="${BatchInvoiceQty}" /></td>
                    <td><input type="text" id="scrap_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="hfExDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="scrap_lineBatchavl_batch_qty" value="${AvailableQty}" /></td>
                    <td><input type="text" id="scrap_lineBatchWh_id" value="${bt_wh_id}" /></td>
                    <td><input type="text" id="scrap_lineBatchMfgName" value="${MfgName}" /></td>
                    <td><input type="text" id="scrap_lineBatchMfgMrp" value="${MfgMrp}" /></td>
                    <td><input type="text" id="scrap_lineBatchMfgDate" value="${MfgDate}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#ScrapSIItemTbl >tbody >tr").each(function () {

            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hfItemID").val();
            if (ItemId == SelectedItem) {
                ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
            }
        });
    }
}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl > tbody >tr").each(function () {
        let row = $(this);
        row.find("#BatchInvoiceQty").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function BindItemBatchDetails() {
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
                batchList.mfg_name = row.find('#scrap_lineBatchMfgName').val()||'';
                batchList.mfg_mrp = row.find('#scrap_lineBatchMfgMrp').val()||'';
                batchList.mfg_date = row.find('#scrap_lineBatchMfgDate').val()||'';
                ItemBatchList.push(batchList);
            }
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }
}
function OnChangeSalesPerson() {
    var SaleParson = $("#ddlSalesPerson").val();
    if (SaleParson == "0" || SaleParson == "" || SaleParson == null) {
        $("#ddlSalesPerson").css("border-color", "red");
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "Red");
        $("#SpanSalesPersonErrorMsg").css("display", "block");
    }
    else {
        $("#ddlSalesPerson").css("border-color", "#ced4da");
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "#ced4da");
    }
}
function OnClickSaveAndExit() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
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
    if (("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            } else {
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
                    var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted == false) {
                        NewArr.push({DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
                    }
                }
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
                </tr>`);
            var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
            }
        });
    }
    else {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);

            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();

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
                </tr>`);
            var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
            }
        });
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        debugger;
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // 
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                Cmn_click_Oc_chkroundoff(null, currentRow);
                var Total = CheckNullNumber(currentRow.find("#OCTotalTaxAmt").text()).trim();//Include Tax OC Amount
                var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
            }
        });
        Calculate_OCAmount();
    } else {
        $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (ItmCode == TaxItmCode) {
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
            }
        });
        CalculateAmount();
    }
    if ($("#taxTemplate").text() == "GST Slab") {/*add by hina on 10-01-2025 for maual gst*/
        GetAllGLID();
    }
}
function OnClickSaveAndExit_OC_Btn() {
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueInBase", "#NetOrderValueInBase")
    click_chkplusminus();
}
function click_chkplusminus() {
    debugger;
    CalculateAmount();/*Added by Suraj on 29-03-2024*/
    var ValDecDigit = $("#ValDigit").text();
    if ($("#chk_roundoff").is(":checked")) {
        try {
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                $.ajax(
                    {
                        type: "POST",
                        url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                        data: {},
                        success: function (data) {

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
                                        ////var ocval = CheckNullNumber($("#TxtOtherCharges").val());
                                        //var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

                                        //var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

                                        //var netval = finalnetval;//$("#NetOrderValueInBase").val();
                                        //var fnetval = 0;

                                        //if (parseFloat(netval) > 0) {
                                        //    var finnetval = netval.split('.');
                                        //    if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
                                        //        if ($("#p_round").is(":checked")) {
                                        //            var decval = '0.' + finnetval[1];
                                        //            var faddval = 1 - parseFloat(decval);
                                        //            fnetval = parseFloat(netval) + parseFloat(faddval);
                                        //            $("#pm_flagval").val($("#p_round").val());

                                        //            //let valInBase = $("#TxtGrossValue").val();
                                        //            //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                                        //        }
                                        //        if ($("#m_round").is(":checked")) {
                                        //            //var finnetval = netval.split('.');
                                        //            var decval = '0.' + finnetval[1];
                                        //            fnetval = parseFloat(netval) - parseFloat(decval);
                                        //            $("#pm_flagval").val($("#m_round").val());

                                        //            //let valInBase = $("#TxtGrossValue").val();
                                        //            //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                                        //        }
                                        //        if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                                        //            var roundoff_netval = Math.round(fnetval);
                                        //            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                        //            $("#NetOrderValueInBase").val(f_netval);
                                        //            //$("#NetOrderValueSpe").val(f_netval);
                                        //            GetAllGLID("RO");
                                        //        }

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
            }

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

        //$("#ServicePIItemTbl > tbody > tr").each(function () {
        //    
        //    var currentrow = $(this);
        //    CalculateTaxExemptedAmt(currentrow, "N")
        //});
        CalculateAmount();
        GetAllGLID();
    }
}
function BindOtherChargeDeatils(val) {
    var DecDigit = $("#ValDigit").text();
    var hdbs_curr = $("#hdbs_curr").val();/*Add by Hina on 20-07-2024 for third party OC*/
    var hdcurr = $("#hdcurr").val();
    var OCTaxable = "N";
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
async function addRoundOffToNetValue(flag) {
    var ValDecDigit = $("#ValDigit").text();
    try {
        await $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                data: {},
                success: function (data) {

                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.length > 0) {
                            if (parseInt(arr[0]["r_acc"]) > 0) {
                                if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {

                                    var grossval = $("#TxtGrossValue").val();
                                    var taxval = $("#TxtTaxAmount").val();
                                    //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
                                    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

                                    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

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
/***------------------------------TDS On Third Party add by Hina on 09-07-2024------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e) {
    const row = $(e.target).closest("tr");
    CC_Clicked_Row = row;
    let GrVal = row.find("#OCAmount").text();
    let GrValWithTax = row.find("#OCTotalTaxAmt").text();
    let TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    let ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    let ToTdsAmt_IT = parseFloat(CheckNullNumber(GrValWithTax));
    $("#hdn_tds_on").val("OC");
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, ToTdsAmt_IT);

}
function OnClickTP_TDS_SaveAndExit() {
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
    var DecDigit = $("#ValDigit").text();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
    });
    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}
/***------------------------------TDS On Third Party End------------------------------***/
function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var TxtInvoiceQuantity = clickedrow.find("#TxtInvoiceQuantity").val();

        var QtyDecDigit = $("#QtyDigit").text();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hfItemID").val();;
        var UOMID = clickedrow.find("#UOMID").val();
        var HdnitmRJOFlag = clickedrow.find("#hdn_RwkJobOrderFlag").val();

        // var MRSNo = $('#ddlMRS_No option:selected').text();
        var TxtInvoiceQuantity = clickedrow.find("#TxtInvoiceQuantity").val();
        var DMenuId = $("#DocumentMenuId").val();
        //var _mdlCommand = $("#_mdlCommand").val();
        var _mdlCommand = $("#_ModelCommand").val();
        var TransType = $("#hdn_TransType").val();

        if (parseFloat(TxtInvoiceQuantity) == "0" || parseFloat(TxtInvoiceQuantity) == "") {
            clickedrow.find("#TxtInvoiceQuantityError").text($("#FillQuantity").text());
            clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
            clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/ScrapSaleInvoice/getItemstockSerialWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemSerial: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType
                    },
                    success: function (data) {

                        $('#ItemStockSerialWise').html(data);
                    },
                });

        }
        else {

            var Scrap_Status = $("#hfStatus").val();
            if (Scrap_Status == "" || Scrap_Status == null || Scrap_Status == "D") {
                BindItemSerialDetails();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var ddlId = "#wh_id" + Index;

                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var SelectedItemSerial = $("#HDSelectedSerialwise").val();



                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ScrapSaleInvoice/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WhID: WarehouseId,
                            Scrap_Status: Scrap_Status,
                            SelectedItemSerial: SelectedItemSerial,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            HdnitmRJOFlag: HdnitmRJOFlag
                        },
                        success: function (data) {

                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(TxtInvoiceQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            //$("#TotalIssuedSerial").text("");
                        },
                    });
            }
            else {


                var Doc_No = $("#InvoiceNumber").val();
                var Doc_dt = $("#Sinv_dt").val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/InterBranchSale/getItemstockSerialWiseAfterStockUpadte",
                        data: {

                            Docno: Doc_No,
                            Docdt: Doc_dt,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType
                        },
                        success: function (data) {

                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(TxtInvoiceQuantity);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                                $("#SaveItemSerialTbl TBODY TR").each(function () {
                                    var row = $(this)
                                    var HdnItemId = row.find("#Scrap_lineSerialItemId").val();
                                    if (ItemId === HdnItemId) {
                                        TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#Scrap_lineSerialIssueQty").val());
                                    }
                                });
                            }
                            $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }

        }

    } catch (err) {
        console.log("Material Issue Error : " + err.message);
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
    // localStorage.setItem('BatchResetFlag', 'True');
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function onclickbtnItemSerialSaveAndExit() {

    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {

        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#Scrap_lineSerialItemId").val();
            if (rowitem == SelectedItem) {

                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var Inv_qty = $("#QuantitySerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var MfgName = row.find("#SrMfgName").val();
            var MfgMrp = row.find("#SrMfgMrp").val();
            var MfgDate = row.find("#SrMfgDate").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="Scrap_lineSerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="Scrap_lineSerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="Scrap_lineSerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="Scrap_lineSerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="Scrap_lineSerialInvQty" value="${Inv_qty}" /></td>
            <td><input type="text" id="Scrap_lineBatchSerialNO" value="${ItemSerialNO}" /></td>
            <td><input type="text" id="Scrap_lineSerialMfg_Name" value="${MfgName}" /></td>
            <td><input type="text" id="Scrap_lineSerialMfg_Mrp" value="${MfgMrp}" /></td>
            <td><input type="text" id="Scrap_lineSerialMfg_Date" value="${MfgDate}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');
        $("#ScrapSIItemTbl >tbody >tr").each(function () {

            var clickedrow = $(this);
            //var ItemId = clickedrow.find("#hdItemId").val();
            var ItemId = clickedrow.find("#hfItemID").val();

            if (ItemId == SelectedItem) {
                ValidateEyeColor(clickedrow, "btnserialdeatil", "N");
                clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
            }
        });
    }
}
function BindItemSerialDetails() {
    var serialrowcount = $('#SaveItemSerialTbl tbody tr').length;
    if (serialrowcount > 0) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.ItemId = row.find("#Scrap_lineSerialItemId").val();
            SerialList.UOMId = row.find("#Scrap_lineSerialUOMId").val();
            SerialList.LOTId = row.find("#Scrap_lineSerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#Scrap_lineSerialIssueQty").val();
            SerialList.SerialNO = row.find("#Scrap_lineBatchSerialNO").val();
            SerialList.invqty = row.find("#Scrap_lineSerialInvQty").val();
            SerialList.mfg_name = row.find("#Scrap_lineSerialMfg_Name").val()||'';
            SerialList.mfg_mrp = row.find("#Scrap_lineSerialMfg_Mrp").val()||'';
            SerialList.mfg_date = row.find("#Scrap_lineSerialMfg_Date").val()||'';
            ItemSerialList.push(SerialList);

        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);

    }

}

function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
function OnChangeDiscountInPerc(e) {
    var clickedrow = $(e.target).closest("tr");
    CalculateDisPercent(clickedrow);
}
async function CalculateDisPercent(clickedrow) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var conv_rate = $("#conv_rate").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisPer;
    ItemName = clickedrow.find("#ItemName" + Sno).val();
    OrderQty = clickedrow.find("#TxtInvoiceQuantity").val();
    ItmRate = clickedrow.find("#TxtRate").val();
    DisPer = clickedrow.find("#DiscountInPerc").val();
    if (AvoidDot(DisPer) == false) {
        clickedrow.find("#DiscountInPerc").val("");
        DisPer = 0;
    }
    if (parseFloat(CheckNullNumber(DisPer)) >= 100) {
        clickedrow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
        clickedrow.find("#item_disc_percError").css("display", "block");
        clickedrow.find("#DiscountInPerc").css("border-color", "red");
        return false;
    } else {
        clickedrow.find("#item_disc_percError").text("");
        clickedrow.find("#item_disc_percError").css("display", "none");
        clickedrow.find("#DiscountInPerc").css("border-color", "#ced4da");
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        var FAmt = (ItmRate * DisPer) / 100;
        var GAmt = OrderQty * (ItmRate - FAmt);
        var DisVal = OrderQty * FAmt;

        var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
        var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinGVal);
        FinalFinGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#NetValueinBase").val(FinalFinGVal);
        clickedrow.find("#DiscountValue").val(FinDisVal);
        CalculateAmount();

        clickedrow.find("#DiscountInPerc").val(parseFloat(DisPer).toFixed(2));
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#TxtItemGrossValue").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
            clickedrow.find("#NetValueinBase").val(FinalFinVal);
            clickedrow.find("#DiscountValue").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateAmount();
        }
    }
    OnChangeGrossAmt();
    if ($("#nontaxable").is(":checked")) {

    } else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    GetAllGLID();
}
function OnClickTaxCalBtn(e) {
    /*Commented and modify by Hina Sharma on 10-01-2025 for manual Gst */
    // var SOItemName = "#ItemName";
    //// var SOItemName = "#ItemName";
    // var SNohiddenfiled = "SI";
    // CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemName) 
    var SOItemName = "#ItemName";
    var SNohiddenfiled = "SI";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemName)
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").css("display", "Block");
            }
            else {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").css("display", "none");
            }
        }
    }
}
function OnClickTaxExemptedCheckBox(e) { 
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#ItemName" + RowSNo).val();
    var AssAmount = currentrow.find("#TxtItemGrossValue").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        if ($("#nontaxable").is(":checked")) {

        }
        else {
            currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "ScrapSIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue")
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    var currentrow = $(e.target).closest('tr');
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        let zero = parseFloat(0).toFixed($("#ValDigit").text());
        currentrow.find("#Txtitem_tax_amt").val(zero);
        currentrow.find("#TxtRate").trigger('change');
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed($("#ValDigit").text()));
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "ScrapSIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue")
        CalculationBaseRate(e)
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramt").text();
    var CstCrtAmt = clickedrow.find("#cramt").text();
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
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDigit);
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ServiceSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            $("#CostCenterDetailPopup").html(data);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}
//-------------------Cost Center Section End-------------------//
function checkMultiSupplier() {
    return true;
}
function click_chkroundoff() {
    debugger
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
            CalculateAmount();
            GetAllGLID();
        }
    } else {
        //for multi supplier
        if ($("#chk_roundoff").is(":checked")) {
            swal("", $("#ManualRoundOffIsNotApplicableForGLHavingMultipleSuppliers").text(), "warning")
            $("#chk_roundoff").attr("checked", false);
        }
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
function OnChangeInvQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        var clickedrow = $(evt.target).closest("tr");
        var BatchInvoiceQty = clickedrow.find("#BatchInvoiceQty").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(BatchInvoiceQty) == false) {
            BatchInvoiceQty = "0";
        }
        if (BatchInvoiceQty != "" && BatchInvoiceQty != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(BatchInvoiceQty) > parseFloat(AvailableQuantity)) {
                clickedrow.find("#BatchInvoiceQty_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
                clickedrow.find("#BatchInvoiceQty_Error").css("display", "block");
                clickedrow.find("#BatchInvoiceQty").css("border-color", "red");
                var test = parseFloat(parseFloat(BatchInvoiceQty)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#BatchInvoiceQty").val(test);
            }
            else {
                clickedrow.find("#BatchInvoiceQty_Error").css("display", "none");
                clickedrow.find("#BatchInvoiceQty").css("border-color", "#ced4da");
                var test = parseFloat(BatchInvoiceQty).toFixed(QtyDecDigit);
                clickedrow.find("#BatchInvoiceQty").val(test);
            }
        }
        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#BatchInvoiceQty").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
        });
        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
/***------------------------------Non-Taxable Start------------------------------***/
function CheckedNontaxable() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    try {
        showLoader();
        var totalItems = $("#ScrapSIItemTbl > tbody > tr ").length;
        if (totalItems == 0) {
            hideLoader();
        }
        //var ConvRate = $("#conv_rate").val();
        if ($("#nontaxable").is(":checked")) {
            let i = 1;
            $("#ScrapSIItemTbl > tbody > tr ").each(function () {
                var currentRow = $(this);
                currentRow.find("#TaxExempted").prop("checked", true);
                currentRow.find("#TaxExempted").attr("disabled", true);
                currentRow.find("#ManualGST").attr("disabled", true);
                currentRow.find("#BtnTxtCalculation").prop("disabled", true);
                currentRow.find('#ManualGST').prop("checked", false);

                //--------------------------------------
                var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()))).toFixed(ValDecDigit);
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                //currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(GrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#NetValueinBase").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
                //--------------------------------------
                i++;
            });
            CalculateTaxAmount_ItemWise_forTaxExampt();
            CalculateAmount();
            GetAllGLID().then(() => {
                hideLoader();
            });
        }
        else {
            let i = 1;
            $("#ScrapSIItemTbl > tbody > tr ").each(function () {
                var currentRow = $(this);
                currentRow.find("#TaxExempted").prop("checked", false);
                currentRow.find("#TaxExempted").attr("disabled", false);
                currentRow.find("#ManualGST").attr("disabled", false);
                currentRow.find("#BtnTxtCalculation").prop("disabled", false);
                i++;
            });
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                var gst_number = $("#Ship_Gst_number").val();
                Cmn_OnSaveAddressApplyGST_Async(gst_number, "ScrapSIItemTbl", "hfItemID", "SNohiddenfiled", "TxtItemGrossValue", "", "", null).then(() => {
                    CalculateAmount();
                    GetAllGLID().then(() => {
                        hideLoader();
                    });
                })
            } else {
                CalculateTaxAmount_ItemWise_forAllItems();
                CalculateAmount();
                GetAllGLID().then(() => {
                    hideLoader();
                });
            }
        }
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
function CalculateTaxAmount_ItemWise_forTaxExampt() {
    debugger;
    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    if (FTaxDetailsItemWise > 0) {
        var NewArray = [];
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = 0;
            var TotalTaxAmount = 0;
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
        });
        BindTaxAmountDeatils(NewArray);
    }
}
function CalculateTaxAmount_ItemWise_forAllItems() {//For Calculate Tax 
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var ConvRate = $("#conv_rate").val();
    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    if (FTaxDetailsItemWise > 0) {
        var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var NewArray = [];
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();

            //---------------------------------------------------------
            var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
            var AssAmount = ItemRow.find("#TxtItemGrossValue").val();
            var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
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
            var TaxPec = TaxPercentage.replace('%', '');
            var TaxAmount = Cmn_CalculateTaxAmount(TaxApplyOn, TaxLevel, AssessVal, TaxPec, NewArray, ItmCode);
            TotalTaxAmt = parseFloat(TotalTaxAmt) + parseFloat(TaxAmount);
            currentRow.find("#TaxAmount").text(TaxAmount);
            //currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
            //---------------------------------------------------------
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
        });
        let acc = [];
        NewArray.map((item) => {//Tax Amount is Grouped by Item Code;
            let index = acc.findIndex(v => v.TaxItmCode == item.TaxItmCode);
            if (index == -1) {
                acc.push({ TaxItmCode: item.TaxItmCode, TotalTaxAmount: item.TaxAmount });
            } else {
                acc[index].TotalTaxAmount = parseFloat(acc[index].TotalTaxAmount) + item.TaxAmount;
            }
        });
        $("#Hdn_TaxCalculatorTbl > tbody > tr").closest('tr').each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var arr = acc.filter(v => v.TaxItmCode == TaxItemID)[0].TotalTaxAmount;
            if (arr.length > 0) {
                currentRow.find("#TotalTaxAmount").text(arr[0].TotalTaxAmount);
            }
        });
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemNo = currentRow.find("#hfItemID").val();
            var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
            if (FTaxDetailsItemWise.length > 0) {
                FTaxDetailsItemWise.each(function () {
                    debugger;
                    var CRow = $(this);
                    var TotalTaxAmtF = CheckNullNumber(CRow.find("#TotalTaxAmount").text());
                    if ($("#nontaxable").is(":checked")) {
                        TotalTaxAmtF = 0;
                    }
                    else if (currentRow.find("#TaxExempted").is(":checked")) {
                        TotalTaxAmtF = 0;
                    }
                    var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val();
                    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    var GstApplicable = $("#Hdn_GstApplicable").text();
                    if (GstApplicable == "Y") {
                        if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                            TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit)
                        }
                    }
                    var TaxItmCode = CRow.find("#TaxItmCode").text();
                    if (TaxItmCode == ItmCode) {
                        currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                        let AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
                        //let NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let FinalNetOrderValueBase = /*ConvRate **/ NetOrderValueBase
                        var oc_amt = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
                        if (oc_amt == ".") {
                            oc_amt = 0;
                        }
                        if (oc_amt != "" && oc_amt != ".") {
                            oc_amt = parseFloat(oc_amt);
                        }
                        currentRow.find("#NetValueinBase").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                        //currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(FinalNetOrderValueBase) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                    }
                });
            }
            else {
                var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()))).toFixed(ValDecDigit);
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                //currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(FGrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#NetValueinBase").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
            }
        });
        CalculateAmount();
        BindTaxAmountDeatils(NewArray);
    }
}
/***------------------------------Non-Taxable End------------------------------***/
/***-------------------For PrintOption add by Hina on 09-07-2024------------------------------------***/
function OnCheckedChangeProdDesc() {
    if ($('#chkproddesc').prop('checked')) {
        $('#chkshowcustspecproddesc').prop('checked', false);
        $("#ShowProdDesc").val("Y");
        $('#ShowCustSpecProdDesc').val('N');
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        $('#chkproddesc').prop('checked', false);
        $('#ShowCustSpecProdDesc').val('Y');
        $('#ShowProdDesc').val('N');
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#ShowProdTechDesc').val('Y');
    }
    else {
        $('#ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#ShowSubItem').val('Y');
    }
    else {
        $('#ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeImport").is(":checked")) {
        $('#PrintFormat').val('F2');
    }
    else {
        $('#PrintFormat').val('F1');
    }
}
function OnCheckedChangeCustAliasName() {

    if ($('#chkcustaliasname').prop('checked')) {
        $('#CustomerAliasName').val('Y');
    }
    else {
        $('#CustomerAliasName').val('N');
    }
}
function OnCheckedChangeTotalQty() {

    if ($('#chkTotalqty').prop('checked')) {
        $('#ShowTotalQty').val('Y');
    }
    else {
        $('#ShowTotalQty').val('N');
    }
}
function OnCheckedChangePrintwithoutSymbol() {
    if ($('#chkprintwithout').prop('checked')) {
        $('#ShowWithoutSybbol').val('Y');
    }
    else {
        $('#ShowWithoutSybbol').val('N');
    }
}
function OnCheckedChangePrintDecl1() {
    if ($('#chkprintDecl1').prop('checked')) {
        $('#showDeclare1').val('Y');
    }
    else {
        $('#showDeclare1').val('N');
    }
}
function OnCheckedChangePrintDecl2() {
    if ($('#chkprintDecl2').prop('checked')) {
        $('#showDeclare2').val('Y');
    }
    else {
        $('#showDeclare2').val('N');
    }
}
function OnCheckedChangePrintInvHead() {
    if ($('#chkprintInvHead').prop('checked')) {
        $('#showInvHeading').val('Y');
    }
    else {
        $('#showInvHeading').val('N');
    }
}
function OnCheckedPrintShipFromAddress() {
    debugger;
    if ($('#chkPrintShipFromAddress').prop('checked')) {
        $('#PrintShipFromAddress').val('Y');
    }
    else {
        $('#PrintShipFromAddress').val('N');
    }
}
function OnCheckedChangePrintPackSize() {
    debugger;
    if ($('#chkshowPackSize').prop('checked')) {
        $('#hdn_ShowPackSize').val('Y');
    }
    else {
        $('#hdn_ShowPackSize').val('N');
    }
}
/***----------------------end-------------------------------------***/
function OnChngNoofCopy() {
    debugger;
    var Noofcopy = $('#txt_NoOfCopies').val();
    if (Noofcopy != "0" || Noofcopy != "" || Noofcopy != null) {
        if (Noofcopy > 0 && Noofcopy <= 4) {
            $('#NumberOfCopy').val(Noofcopy);
            $("#SpanNoOfCopies").css("display", "none");
            $("#txt_NoOfCopies").css("border-color", "#ced4da");
        }
        else {
            $('#txt_NoOfCopies').val("");
            $("#txt_NoOfCopies").css("border-color", "Red");
            $('#SpanNoOfCopies').text($("#valueReq").text());
            $("#SpanNoOfCopies").css("display", "block");
        }

    }
    else {
        $("#txt_NoOfCopies").css("border-color", "Red");
        $('#SpanNoOfCopies').text($("#valueReq").text());
        $("#SpanNoOfCopies").css("display", "block");
    }
}

function UpdateTransportConfirm(event) {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DSinvDate = $("#Sinv_dt").val();
    var ErrorFlag = "N";
    $.ajax({
        type: "POST",
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DSinvDate
        },
        async: false,
        success: function (data) {
            if (data == "TransAllow") {
                var status = $("#hfStatus").val();
                if (status == "A") {
                    swal({
                        title: $("#UpdateTransportDetail").text() + "?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonClass: "btn-success",
                        cancelButtonClass: "btn-danger",
                        confirmButtonText: "Yes",
                        cancelButtonText: "No",
                        closeOnConfirm: false
                    }, function (isConfirm) {
                        if (isConfirm) {
                            $("#HdEditCommand").val("UpdateTransPortDetail");
                            $('form').submit();

                            return true;
                        } else {
                            $("#HdDeleteCommand").val("Edit");
                            $('form').submit();
                            return true;
                        }
                    });
                    ErrorFlag = "Y";
                    return false;
                }
            }
            else {
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
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


function OnChangeAccountName(RowID, e) {//Added by Suraj Maurya on 31-01-2026

    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    /*commented by hina on 22-07-2024 to change because acc name has blank with 3rd party OC */
    //var hdn_acc_id = clickedrow.find("#hfAccID").val();
    //if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
    //    var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
    //    if (Len > 0) {
    //        $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').remove();
    //    }
    //}
    //if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
    //    clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    //}
    //else {
    //    clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    //}

    //clickedrow.find("#hfAccID").val(Acc_ID);
    /*changes by Hina on 22-07-2024 for 3rd party OC and tds on 3rd party OC */
    clickedrow.find("#SpanAcc_name_" + SNo).css("display", "none");
    clickedrow.find("[aria-labelledby='select2-Acc_name_" + SNo + "-container']").css("border-color", "#ced4da");

    if (Acc_ID != null) {
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

            //Added by Suraj on 12-08-2024 to stop reset of GL Account if user changes the GL Acc.
            $("#ScrapSIItemTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
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
    }
    clickedrow.find("#hfAccID").val(Acc_ID);
}