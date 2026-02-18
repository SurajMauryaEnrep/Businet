$(document).ready(function () {

    $("#DdlTranspt_Name").select2();
    $("#dispatch_State").select2();
    $("#dispatch_District").select2();
    $("#dispatch_City").select2();
    $("#ddlSalesPerson").select2();
    BindCustomerList();
    BindScrpItmList(1);
    $('#ScrapSIItemTbl tbody').on('click', '.deleteIcon',async function () {
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
        /*CalculateAmount("pm_flag");*//*commented and modify by hina on 13-01-2025*/
        CalculateAmount();
        let cust_id = $("#CustomerName").val();
        let GrossValue = $("#TxtGrossValue").val();
        await AutoTdsApply(cust_id, GrossValue);
        //var TOCAmount = parseFloat($("#TxtOtherCharges").val());//Commented by Suraj Maurya on 06-01-2025
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
    SrVer_No = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(SrVer_No);
    BindDDLAccountList();
    //GetAllGLID();
    //  $(document).ajaxStart(function () {
    //            showLoader();
    //            // Hide loader when all AJAX requests are complete
    //            //hideLoader();
    //        });
    //$(document).ajaxStop(function () {
    //    // Hide loader when all AJAX requests are complete
    //    hideLoader();
    //});
    CancelledRemarks("#Cancelled", "Disabled");

    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#ScrapSIItemTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#ItemName' + $(this).find('#SNohiddenfiled').val().trim()).val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
    if ($("#rpt_id").text() != "0") {
        $("#ddlSalesPerson").attr('disabled', true);
    }
   

    Cmn_ChangeValueReplicate("NetOrderValueInBase", "#PaymentscheduleInvoiceAmount");
    Cmn_RecalPaymScheTotals();
});
function onclickRefreshbtn() {
    debugger;
    $("#HdDeleteCommand").val('');
}
function DeleteItemBatchOrderQtyDetails(Itemid) {
    
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        
        var row = $(this);
        var rowitem = row.find("#scrap_lineBatchItemId").val();
        //var rowitem = $("#HDItemNameBatchWise").val();
        if (rowitem == Itemid) {
            
            $(this).remove();
        }
    });
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

function ShowSavedAttatchMentFiles(_LPO_No, _LPO_Date) {

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetPOAttatchDetailEdit",
        data: { PO_No: _LPO_No, PO_Date: _LPO_Date },
        success: function (data) {
            
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            } else {
                $("#PartialImageBind").html(data);
            }

        }
    });

}
function ApproveBtnClick() {
    InsertVerApproveDetails("", "", "");
}
function DeleteBtnClick() {
    //RemoveSession();
    Delete_PoDetails();
    ResetWF_Level();

}
function ForwardBtnClick() {

    //var OrderStatus = "";
    //OrderStatus = $('#hfStatus').val();
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

    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var DSinvDate = $("#Sinv_dt").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: DSinvDate
            },
            success: function (data) {
                /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
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
                else {/* to chk Financial year exist or not*/
                    /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
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
    var mailerror = "";
    var PV_VoucherNarr = $("#hdn_PV_Narration").val();
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
    try {
        if (fwchkval != "Approve") {
            var pdfAlertEmailFilePath = "TaxInvoice_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval);
            if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
                pdfAlertEmailFilePath = "";
            }
        }
    }
    catch { }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            
            //var Action_name = "DPODetail,DPO";
            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + DocNo + "&docdate=" + DocDate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
            //Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks);
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ScrapSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
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
        window.location.href = "/ApplicationLayer/ScrapSaleInvoice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&PV_VoucherNarr=" + PV_VoucherNarr + "&BP_VoucherNarr=" + BP_VoucherNarr + "&DN_VoucherNarr=" + DN_VoucherNarr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
             //Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ScrapSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
             //Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/ScrapSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
async function GetPdfFilePathToSendonEmailAlert(shpNo, shpDt, fileName, docid, docstatus) {
    debugger;
    var filepath = "";
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
        ItemAliasName: $("#ItemAliasName").val(),
        ShowWithoutSybbol: $("#ShowWithoutSybbol").val(),
        showDeclare1: $("#showDeclare1").val(),
        showDeclare2: $("#showDeclare2").val(),
        showInvHeading: $("#showInvHeading").val(),
        PrintShipFromAddress: $("#PrintShipFromAddress").val(),
        PrintCorpAddr: $("#PrintCorpAddr").val(),
        PrintRemarks: $("#PrintRemarks").val(),
    })
    try {
        filepath =  await $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/SavePdfDocToSendOnEmailAlert",
            data: { poNo: shpNo, poDate: shpDt, fileName: fileName, PrintFormat: JSON.stringify(PrintFormat), GstApplicable: GstApplicable, docid: docid, docstatus: docstatus },
            success: function (data) {
                hideLoader();
            }
        });
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
    return filepath;
}
function BindCustomerList() {
     
    var Branch = sessionStorage.getItem("BranchID");
    var DocId = $("#DocumentMenuId").val();
    $("#CustomerName").select2({
        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term,
                    BrchID: Branch,
                    DocumentMenuId: DocId
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {

                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
function OnChangeCustomer(CustID) {
    //var ExchDecDigit = $("#ExchDigit").text();
    var DecDigit = $("#RateDigit").text();
    var Cust_id = CustID.value;
    /*add by Hina on 20-07-2024 for third party OC */
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
        $(".plus_icon1").css("display","block")
       }
    var DocumentMenuId = $("#DocumentMenuId").val();

    try {

        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ScrapSaleInvoice/GetCustAddrDetail",
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
                            //$("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);
                            $("#ddlCurrency").val(arr.Table[0].curr_name);
                            $("#Hdn_ddlCurrency").val(arr.Table[0].curr_id);
                            /*------------Start Add By Hina on 20-07-2024 for third party OC----------*/
                            $("#cust_acc_id").val(arr.Table[0].cust_acc_id);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#txtCurrency").val(arr.Table[0].curr_name);
                            //$("#conv_rate").val(parseFloat(getvalwithoutroundoff(arr.Table[0].conv_rate, DecDigit)).toFixed(DecDigit));
                            //$("#hdnconv_rate").val(parseFloat(getvalwithoutroundoff(arr.Table[0].conv_rate, DecDigit)).toFixed(DecDigit));
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(DecDigit));
                            $("#hdnconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(DecDigit));
                            /*---------End Add By Hina on 20-07-2024 for third party OC---------*/



                            //$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));
                        }
                        else {
                            $("#Address").val("");
                            //$("#TxtShippingAddr").val("");
                            $("#bill_add_id").val("");
                            $("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                            
                        }
                        if (arr.Table1.length > 0) {
                            $("#Declaration_1").val(arr.Table1[0].declar_1);
                            $("#Declaration_2").val(arr.Table1[0].declar_2);
                            $("#Invoice_Heading").val(arr.Table1[0].inv_heading);
                            $("#Corporate_Address").val(arr.Table1[0].corp_off_addr);
                            $("#PvtMark").val(arr.Table1[0].pvt_mark);
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
            });


    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
    
}


function CheckValidations_forSubItems() {
    
    return Cmn_CheckValidations_forSubItems("ScrapSIItemTbl", "SNohiddenfiled", "ItemName", "TxtInvoiceQuantity", "SubItemOrdQty", "Y");
}
function ResetWorningBorderColor() {
    
    return Cmn_CheckValidations_forSubItems("ScrapSIItemTbl", "SNohiddenfiled", "ItemName", "TxtInvoiceQuantity", "SubItemOrdQty", "N");
}
function onchangeCancelledRemarks() {
    debugger;
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
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
    debugger;
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
    var netValue = parseFloat(CheckNullNumber($("#TxtGrossValue").val()))
    if (netValue <= 0) {
        swal("", $("#ValueShouldBeGreaterThan0").text(), "warning");
        return false;
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
    if ($("#Cancelled").is(':checked')) {
        
            if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
                $('#SpanCancelledRemarks').text($("#valueReq").text());
                $("#SpanCancelledRemarks").css("display", "block");
                $("#Cancelledremarks").css("border-color", "Red");
                return false;
            }
            else {
                $('#SpanCancelledRemarks').text("");
                $("#SpanCancelledRemarks").css("display", "none");
                $("#Cancelledremarks").css("border-color", "#ced4da");
            }

        
    }
    else {
        var ValidDispatchDetail = ValidationDispatchDetail();
        if (ValidDispatchDetail == false) {
            return false;
        }
    }
    if (CheckTransportDetailValidation() == false) {/*Add By Hina on 24-07-2024 for transporter detail for EInvoce*/
        return false;
    }
    if (Cmn_ValidationPaymentSchedule() == false) {
        return false;
    }
    //SerialFlag = CheckItemSerialValidation();
    //if (SerialFlag == false) {
    //    return false;
    //}
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
    var TcsDetail = [];
    TcsDetail = Cmn_Insert_Tcs_Details();
    var str_TcsDetail = JSON.stringify(TcsDetail);
    $('#hdn_tcs_details').val(str_TcsDetail);
    /* Insert TCS Details End */

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

    var Final_OC_TdsDetails = [];/*Add by Hina on 22-07-2024 for tds on 3rd Party OC*/
    Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
    var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
    $('#hdn_oc_tds_details').val(Oc_Tds_Details);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);
    BindItemBatchDetails();
    BindItemSerialDetails();
    Cmn_BindPaymentSchedule();
    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfStatus") == false) {
        return false;
    }
    var Suppname = $('#CustomerName option:selected').text();
    $("#Hdn_CustName").val(Suppname);
   // $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    $("#ddlSalesPerson").attr('disabled', false);
    return true;
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
        var FOC = "";
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
        if (currentRow.find("#FOC").is(":checked")) {
            FOC = "Y"
        }
        else {
            FOC = "N"
        }
        var PackSize = currentRow.find("#PackSize").val();
        PI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, Hsn_no: Hsn_no, inv_qty: inv_qty, item_rate: item_rate,
            item_gr_val: item_gr_val, TaxExempted: TaxExempted, ManualGST: ManualGST, item_tax_amt: item_tax_amt, item_oc_amt: item_oc_amt,
            item_net_val_bs: item_net_val_bs, UOMID: UOMID, wh_id: wh_id, avl_qty: avl_qty,
            item_acc_id: item_acc_id, itemRemarks: itemRemarks, DiscInPer: DiscInPer, DiscVal: DiscVal, sr_no: sr_no, FOC: FOC, PackSize: PackSize
        });
    });
    return PI_ItemsDetail;
}
function InsertTaxDetails() {
    /*commented and modify by Hina sharma on 13-01-2025 for tax exempted*/
    //var TaxDetails = [];
    //$("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        
       
    //    var item_id = "";
    //    var tax_id = "";
    //    var TaxName = "";
    //    var tax_rate = "";
    //    var tax_level = "";
    //    var tax_val = "";
    //    var tax_apply_on = "";
    //    var tax_apply_onName = "";
    //    var totaltax_amt = "";
    //    var currentRow = $(this);
      
    //    item_id = currentRow.find("#TaxItmCode").text();
    //    tax_id = currentRow.find("#TaxNameID").text();
    //    TaxName = currentRow.find("#TaxName").text().trim();
    //    tax_rate = currentRow.find("#TaxPercentage").text();
    //    tax_level = currentRow.find("#TaxLevel").text();
    //    tax_val = currentRow.find("#TaxAmount").text();
    //    tax_apply_on = currentRow.find("#TaxApplyOnID").text();
    //    tax_apply_onName = currentRow.find("#TaxApplyOn").text();
    //    totaltax_amt = currentRow.find("#TotalTaxAmount").text();
       
    //    TaxDetails.push({ item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    //});
    //return TaxDetails;
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
function InsertTdsDetails() {
    
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        
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

        TDS_Details.push({ Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val, Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName, Tds_totalAmnt: Tds_totalAmnt });
    });
    return TDS_Details;
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
            let tds_amt = currentRow.find("#OC_TDSAmt").text(); // Added by Hina on 05-07-2024 to chnge for tds on oc
            //var bill_date1 = bill_date[2] + "-" + bill_date[1] + "-" + bill_date[0];
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            PI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName
                , OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, supp_id: supp_id, curr_id: curr_id
                , supp_type: supp_type, bill_no: bill_no, bill_date: bill_date, tds_amt: tds_amt
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    //if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) { Commented by Hina on 22-07-2024 for 3rd party OC
    //    $("#Tbl_OC_Deatils >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        var oc_id = "";
    //        var oc_val = "";
    //        var OCTaxAmt = "";
    //        var OCTotalTaxAmt = "";
    //        var OCName = "";
    //        var OC_Curr = "";
    //        var OC_Conv = "";
    //        var OC_AmtBs = "";
    //        oc_id = currentRow.find("#OCValue").text();
    //        oc_val = currentRow.find("#OCAmount").text();
    //        OCTaxAmt = currentRow.find("#OCTaxAmt").text();
    //        OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
    //        OCName = currentRow.find("#OCName").text();
    //        OC_Curr = currentRow.find("#OCCurr").text();
    //        OC_Conv = currentRow.find("#OCConv").text();
    //        OC_AmtBs = currentRow.find("#OcAmtBs").text();
    //        PI_OCList.push({ oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs });
    //    });
    //}
    return PI_OCList;
};
//function GetSSI_VoucherDetails() { commented by Hina on 22-07-2024 for  3rd party OC
//    
//    var PI_VouList = [];
//    var PI_VouROList = [];
//    var SuppVal = 0;
//    var Compid = $("#CompID").text();
//    var InvType = "D";
   
//    var TransType = "Sal";
//    if ($("#VoucherDetail >tbody >tr").length > 0) {
//        $("#VoucherDetail >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var acc_id = "";
//            var acc_name = "";
//            var dr_amt = "";
//            var cr_amt = "";
//            var Gltype = "";
//            let GlSrNo = currentRow.find("#SrNoGL").text();
//            acc_id = currentRow.find("#hfAccID").val();
//            acc_name = currentRow.find("#txthfAccID").val();
//            dr_amt = currentRow.find("#dramt").text();
//            cr_amt = currentRow.find("#cramt").text();
//            Gltype = currentRow.find("#type").val();
            
//            PI_VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: InvType, Value: SuppVal, DrAmt: dr_amt, CrAmt: cr_amt, Gltype: Gltype, TransType: TransType, GlSrNo: GlSrNo });
//        });

//    }
//    return PI_VouList;
//};
function GetSSI_VoucherDetails() {
    /*Add Code by Hina on 22-07-2024 for change as 3rd party OC and tds on 3rd party OC*/
    
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
//function Cmn_Insert_OC_Tds_Details() { /*Add Code by Hina on 22-07-2024 for 3rd party OC and tds on 3rd party OC*/
//commented by Suraj maurya on 11-12-2024 
    
//    var TDS_Details = [];
//    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        
//        var Tds_id = "";
//        var Tds_rate = "";
//        var Tds_level = "";
//        var Tds_val = "";
//        var Tds_apply_on = "";
//        var Tds_name = "";
//        var Tds_applyOnName = "";
//        var Tds_totalAmnt = "";
//        var Tds_supp_id = "";
//        var Tds_oc_id = "";


//        var currentRow = $(this);

//        Tds_id = currentRow.find("#td_TDS_NameID").text();
//        Tds_rate = currentRow.find("#td_TDS_Percentage").text();
//        Tds_level = currentRow.find("#td_TDS_Level").text();
//        Tds_val = currentRow.find("#td_TDS_Amount").text();
//        Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
//        Tds_name = currentRow.find("#td_TDS_Name").text();
//        Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
//        Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
//        Tds_supp_id = currentRow.find("#td_TDS_Supp_Id").text();
//        Tds_oc_id = currentRow.find("#td_TDS_OC_Id").text();

//        TDS_Details.push({
//            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
//            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
//            , Tds_totalAmnt: Tds_totalAmnt, Tds_supp_id: Tds_supp_id, Tds_oc_id, Tds_oc_id: Tds_oc_id
//        });
//    });
//    return TDS_Details;
//}
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
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
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
        //$("#ddlSalesPerson").attr("Disabled", true);
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
    if (parseFloat(CheckNullNumber(NoOfPacks))==0) {
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
    var flag = "N";
    if ($("#ScrapSIItemTbl >tbody >tr").length > 0) {
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var ItemType = currentRow.find("#ItemType").val();
            if (ItemType == "Stockable") {
                flag = "Y";
            }

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
                if (ItemType != "Service") {
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
                }
               
            }
            if (currentRow.find("#FOC").is(":checked")) {

            }
            else {
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
            }
                
            var ddlId = "#wh_id" + Sno;
            var whERRID = "#wh_Error";
            if (ItemType == "Stockable") {
                if (currentRow.find(ddlId).val() == "0") {

                    currentRow.find(whERRID).text($("#valueReq").text());
                    currentRow.find(whERRID).css("display", "block");
                    currentRow.find(ddlId).css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    var WHName = $("#wh_id option:selected").text();
                    $("#hdwh_name").val(WHName)
                    currentRow.find(whERRID).css("display", "none");
                    currentRow.find(ddlId).css("border-color", "#ced4da");
                }
            }

            if (currentRow.find(ddlId).val() != "0") {

            }
            else {

            }

        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    if (flag == "N") {
        swal("", $("#AtleastOneStockableItemIsRequiredInOrder").text(), "warning");
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
//function CheckSSI_VoucherValidations() {/*Commented by Hina on 22-07-2024 to modify due to 3rd party OC and tds on 3rd party OC*/
//    
//    var ErrorFlag = "N";
//    var ValDigit = $("#ValDigit").text();
//    var DrTotal = $("#DrTotal").text();
//    var CrTotal = $("#CrTotal").text();
//    $("#VoucherDetail >tbody >tr").each(function (i, row) {
//        
//        var currentRow = $(this);
//        //var AccID = currentRow.find("#hfAccID").val();
//        var rowid = currentRow.find("#SNohf").val();
//        var AccountID = '#Acc_name_' + rowid;
//        var AccID = currentRow.find('#Acc_name_' + rowid).val();
//        if (AccID != '0' && AccID != "") {
//            ErrorFlag = "N";
//        }
//        else {
//            swal("", $("#GLPostingNotFound").text(), "warning");
//            ErrorFlag = "Y";
//            return false;
//        }

//    });
//    
//    if (DrTotal == '' || DrTotal == 'NaN') {
//        DrTotal = 0;
//    }
//    if (CrTotal == '' || CrTotal == 'NaN') {
//        CrTotal = 0;
//    }

//    
//    if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
//    }
//    else {
//        swal("", $("#DebtCredtAmntMismatch").text(), "warning");
//        ErrorFlag = "Y";
//    }
//    if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
//        swal("", $("#GLPostingNotFound").text(), "warning");
//        ErrorFlag = "Y";
//    }
//    if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
//        swal("", $("#GLPostingNotFound").text(), "warning");
//        ErrorFlag = "Y";
//    }


//    if (ErrorFlag == "Y") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function CheckSSI_VoucherValidations() {
    

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 06-08-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 06-08-2024 to add new common gl validations*/

    //var ErrorFlag = "N";
    //var ValDigit = $("#ValDigit").text();
    ////var DrTotal = $("#DrTotal").text();
    ////var CrTotal = $("#CrTotal").text();
    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {
    //    
    //    var currentRow = $(this);
    //    //var AccID = currentRow.find("#hfAccID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#Acc_name_' + rowid;
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //    }
    //    else {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //        return false;
    //    }

    //});
    //
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}

    //
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
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
    //window.location.reload();
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
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#TxtInvoiceQuantityError").css("display", "none");
    clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");


    return true;
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    } 
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#TxtRateError").css("display", "none");
    clickedrow.find("#TxtRate").css("border-color", "#ced4da");


    return true;
}

//function CheckedCancelled() {
//    debugger;
//    if ($("#Cancelled").is(":checked")) {
//        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
//        $("#btn_save").attr('disabled', false);
//        $("#HdDeleteCommand").val('');
//        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
//    }
//    else {
//        $("#btn_save").attr('onclick', '');
//        $("#btn_save").attr('disabled', true);
//        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    }
//}
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
    var SPI_no = $("#InvoiceNumber").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, SSIDTransType, '');
    //Cmn_SuppAddrInfoBtnClick(Cust_id, bill_add_id, status, PODTransType);
}
function OnClickShippingAddressIconBtn(e) {
    // debugger;
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
//----------------Transport detail-------------------------------//
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
    //$("#TxtTranspt_Name").css("border-color", "#ced4da");
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
//------------------------OC-------------------------//
function OnClickSaveAndExit_OC_Btn() {
    
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueInBase", "#NetOrderValueInBase")
    click_chkplusminus();/*add By Hina on 20-07-2024 for round off toggle*/
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
        //var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
       /* var NetOrderValueInBase = currentRow.find("#TxtNetValue").val();*/
        var NetOrderValueBase = currentRow.find("#NetValueinBase").val();
        //if (currentRow.find("#FOC").is(":checked")) {

        //}
        //else {
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
        //}
       
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
            //var Sno = currentRow.find("#SNohiddenfiled").val();
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
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#ScrapSIItemTbl >tbody >tr").each(function () {
        
        var currentRow = $(this);

        var POItm_GrossValue = CheckNullNumber(currentRow.find("#TxtItemGrossValue").val());
        //var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        var POItm_OCAmt = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
        //if (POItm_GrossValue == null || POItm_GrossValue == "") {
        //    POItm_GrossValue = "0.00";
        //}
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0.00";
        }
        //if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
        //    POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(DecDigit);
        //}
        var POItm_NetOrderValueInBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        /*currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueInBase)).toFixed(DecDigit));*/
        FinalPOItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(DecDigit);
        currentRow.find("#NetValueinBase").val((parseFloat(CheckNullNumber(FinalPOItm_NetOrderValueBase))).toFixed(DecDigit));
    });
    /*CalculateAmount("pm_flag");*//*commented and modify by hina on 13-01-2025 as per service purchase invoice*/
    CalculateAmount();
};
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
    /* $("#NetOrderValueInBase").val(NetOrderValueInBase);*/
    $("#NetOrderValueInBase").val(parseFloat(NetOrderValueBase).toFixed(DecDigit));
    $("#TxtGrossValue").val(GrossValue).trigger('change');
   
    /*code start Add by Hina sharma on 10-01-2025*/
    if (flag == "CallByGetAllGL") {
        ApplyRoundOff("CallByGetAllGL");/*add by Hina on 10-01-2025 */
    }/*code End Add by Hina sharma on 10-01-2025*/
            //if (flag != "pm_flag") {/*commented by Hina on 10-01-2025 */
            //    if ($("#chk_roundoff").is(":checked")) {
            //        //GetAllGLID('RO');
            //        await addRoundOffToNetValue();
            //    } else {
            //        GetAllGLID();
            //    }
            //}
    
    
}
function SetOtherChargeVal() {
    
    $("#ScrapSIItemTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed($("#ValDigit").text()));
    })
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
    //$("#PI_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        /*Comment by Hina on 20-07-2024 for third party OC*/
//        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
//            var currentRow = $(this);
//            //  
//            var td = "";
//            if (DocumentMenuId == "105103148") {
//                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
//<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
//            }

//            $('#Tbl_OtherChargeList tbody').append(`<tr>
//<td >${currentRow.find("#OCName").text()}</td>
//<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
//<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
//`+ td + `
//</tr>`);
//            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
//            if (DocumentMenuId == "105103148") {
//                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
//                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
//            }

//        });
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //  
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
    if (DocumentMenuId == "105103148" && OCTaxable == "Y") {
       
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
    
    if (val == "") {
        GetAllGLID();
    }

}

//--------------------------------------------//

//-------------GL Detail---------------------------------//
function BindDDLAccountList() {
    //
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105103148");
}
//function BindData() {/*Commented and changes by Hina on 22-07-2024 for 3rd party OC and tds on 3rd party OC */
//    
//    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
//    if (AccountListData != null) {
//        if (AccountListData.length > 0) {
//            $("#VoucherDetail >tbody >tr").each(function () {
//                var currentRow = $(this);
//                var rowid = currentRow.find("#SNohf").val();
//                rowid = parseFloat(rowid) + 1;
//                if (rowid > $("#VoucherDetail >tbody >tr").length) {
//                    return false;
//                }
//                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
//                for (var i = 0; i < AccountListData.length; i++) {
//                    $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
//                }
//                var firstEmptySelect = true;
//                $("#Acc_name_" + rowid).select2({
//                    templateResult: function (data) {
//                        var selected = $("#Acc_name_" + rowid).val();
//                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
//                            var classAttr = $(data.element).attr('class');
//                            var hasClass = typeof classAttr != 'undefined';
//                            classAttr = hasClass ? ' ' + classAttr : '';
//                            var $result = $(
//                                '<div class="row">' +
//                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                                '</div>'
//                            );
//                            return $result;
//                        }
//                        firstEmptySelect = false;
//                    }
//                });
//            });
//        }
//    }
//    $("#VoucherDetail >tbody >tr").each(function (i, row) {
//        var currentRow = $(this);
//        var AccID = currentRow.find("#hfAccID").val();
//        var rowid = currentRow.find("#SNohf").val();
//        if (AccID != '0' && AccID != "") {
//            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
//        }
//    });
//}
function BindData() {/*changes by Hina on 22-07-2024 for 3rd party OC and tds on 3rd party OC */
    
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                //rowid = parseFloat(rowid) + 1;
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                //$("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                //for (var i = 0; i < AccountListData.length; i++) {
                //    $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                //}
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="VouTextddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#VouTextddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
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
function OnChangeAccountName(RowID, e) {
    
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

//async function GetAllGLID(flag) {/*Commented by hina on 24-07-2024  for all multiple supplier 3rd Party OC and tds on OC*/
//    
//    if (flag != "RO") {
//        if ($("#chk_roundoff").is(":checked")) {
//            await addRoundOffToNetValue();
//        }
//    }
//    var GstType = $("#Hd_GstType").val();
//    var GstCat = $("#Hd_GstCat").val();
//    var NetInvValue = $("#NetOrderValueInBase").val();
//    var NetTaxValue = $("#TxtTaxAmount").val();
//    var ValueWithoutTax = (parseFloat(NetInvValue) - parseFloat(NetTaxValue));
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    var cust_id = $("#CustomerName").val();
//    var CustVal = 0;
//    var Compid = $("#CompID").text();
//    var InvType = "D";

//    var TransType = 'Sal';
//    var GLDetail = [];
//    var TxaExantedItemList = [];
//    GLDetail.push({ comp_id: Compid, id: cust_id, type: "Cust", doctype: InvType, Value: CustVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Cust" });
//    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
//        
//        var currentRow = $(this);
//        var item_id = currentRow.find("#hfItemID").val();
//        if (item_id != "" && item_id != null) {
//            var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
//            GLDetail.push({ comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm" });

//        }

//        });
//    
//    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
//        
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#TaxNameID").text();
//        var tax_amt = currentRow.find("#TaxAmount").text();
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
//        
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#taxID").text();
//        var tax_amt = currentRow.find("#TotalTaxAmount").text();
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Tbl_OtherChargeList >tbody >tr").each(function (i, row) {
//        
//        var currentRow = $(this);
//        var oc_id = currentRow.find("#OCID").text();
//        var oc_amt = currentRow.find("#OCAmtSp1").text();
//        GLDetail.push({ comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC" });
//    });
//    
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/ScrapSaleInvoice/GetGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({ GLDetail: GLDetail }),
//        success: function (data) {
//            
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                var Voudet = 'Y';
//                $('#VoucherDetail tbody tr').remove();
//                if (arr.Table1.length > 0) {
//                    var errors = [];
//                    var step = [];
//                    for (var i = 0; i < arr.Table1.length; i++) {
//                        //
//                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
//                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
//                            step.push(parseInt(i));
//                            Voudet = 'N';
//                        }
//                    }
//                    var arrayOfErrorsToDisplay = [];
//                    $.each(errors, function (i, error) {
//                        arrayOfErrorsToDisplay.push({ text: error });
//                    });
//                    Swal.mixin({
//                        confirmButtonText: 'Ok',
//                        type: "warning",
//                    }).queue(arrayOfErrorsToDisplay)
//                        .then((result) => {
//                            if (result.value) {

//                            }
//                        });
//                }
//                if (Voudet == 'Y') {
//                    if (arr.Table.length > 0) {
//                        $('#VoucherDetail tbody tr').remove();
//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            ++rowIdx;
//                            //var Acc_Name_previos = arr.Table1.acc_id;
//                            var Acc_Id = arr.Table[j].acc_id;
//                            //var onchange;
//                            //if (Acc_Id != "") {
//                            //    onchange = 'onchange="OnChangeAccountName(this,event)"';
//                            //}
//                            //else {
//                            //    onchange = '';
//                            //}

//                            var acc_Id = Acc_Id.substring(0, 1);
//                            var Disable;
//                            if (acc_Id == "3" || acc_Id == "4") {
//                                Disable = "";
//                            }
//                            else {
//                                Disable = "disabled";
//                            }
//                            // 
//                            var FieldType = "";
//                            if (arr.Table[j].type == 'Itm') {
//                                FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;">
//                                <select class="form-control" id="Acc_name_${rowIdx}" onchange="OnChangeAccountName(${rowIdx},event)">
//                                    <option value="${arr.Table[j].acc_id}">${arr.Table[j].acc_name}</option>
//                                 </select>
//                                <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" /></div> `;
//                            }
//                            else {
//                                FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" />`;
//                            }

//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                
                                
//                                    $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding" id="SrNoGL">${rowIdx}</td>
//                                    <td>`+ FieldType + `</td>
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                    <td class="center">
//                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                     </td>
//                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
//                                     <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                    <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
//                                    <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>
                                   
//                            </tr>`);
                               
//                            }
//                            Cmn_BindAccountList1("#Acc_name_", rowIdx, "#VoucherDetail", "#SNohf", "BindData", "105103148");
//                            
//                            if ($("#ForDisableOCDlt").val() == "Disable") {
//                                
//                                $("#Acc_name_" + rowIdx).attr("disabled", true);
//                            }
//                        }
//                        $("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat($("#NetOrderValueInBase").val()).toFixed(ValDecDigit));
//                    }

//                }

//                //if (Voudet == 'Y') {
//                //    if (arr.Table.length > 0) {
//                //        $('#VoucherDetail tbody tr').remove();
//                //        var rowIdx = $('#VoucherDetail tbody tr').length;
//                //        for (var j = 0; j < arr.Table.length; j++) {
//                //            
//                //            var Acc_Id = arr.Table[j].acc_id;
//                //            acc_Id = Acc_Id.substring(0, 1);
//                //            var Disable;
//                //            if (acc_Id == "3" || acc_Id == "4") {
//                //                Disable = "";
//                //            }
//                //            else {
//                //                Disable = "disabled";
//                //            }

//                //            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                //                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                //                    <td class="sr_padding">${rowIdx}</td>
//                //                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
//                //                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                //                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                //                    <td style="display:none"><input type="hidden" id="SNohf" value="${rowIdx}"/></td>
//                //                    <td class="center">
//                //                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                //                    </td>
//                //                    <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                //                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                //                   <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
//                //                    <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>
//                //            </tr>`);
//                //            }
//                //        }
//                //        $("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat($("#NetOrderValueInBase").val()).toFixed(ValDecDigit));
//                //    }

//                //}
//                CalculateVoucherTotalAmount();
//            }

//        }
//    });
//}
/*---------Start Gl Change By Hina on 24-07-2024 for multiple supplier---------*/
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
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt,''),acc_id:""
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
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: IsNull(oc_supp_bill_no,'')
            , bill_date: IsNull(oc_supp_bill_dt,''), acc_id: ""
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
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: IsNull(oc_supp_bill_no,'')
                , bill_date: IsNull(oc_supp_bill_dt,''), acc_id: ""
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
            bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date,''), acc_id: ""
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
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt,''), acc_id: ""
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
                , bill_no: IsNull(ArrOcGl[0].bill_no, ''), bill_date: IsNull(ArrOcGl[0].bill_date,'')
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
            });
        }



    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item, idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: IsNull(item.bill_no, ''), bill_date: IsNull(item.bill_date,''), acc_id: ""
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
    //var glData = JSON.stringify(GLDetail);
    //await $.ajax({
    //    type: "POST",
    //    //url: "/Common/Common/GetGLDetails1",
    //    url: "/Common/Common/GetGLDetails_StringData",
    //    //contentType: "application/json; charset=utf-8",
    //    //dataType: "json",
    //    //data: JSON.stringify({ GLDetail: GLDetail }),
    //    data: { GLDetail: glData },
    //    success: function (data) {
            
    //        if (data == 'ErrorPage') {
    //            return false;
    //        }
    //        if (data !== null && data !== "") {
    //            var arr = [];
    //            arr = JSON.parse(data);
    //            var Voudet = 'Y';
    //            $('#VoucherDetail tbody tr').remove();
    //            if (arr.Table1.length > 0) {
    //                var errors = [];
    //                var step = [];
    //                for (var i = 0; i < arr.Table1.length; i++) {
    //                    if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
    //                        errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
    //                        step.push(parseInt(i));
    //                        Voudet = 'N';
    //                    }
    //                }
    //                var arrayOfErrorsToDisplay = [];
    //                $.each(errors, function (i, error) {
    //                    arrayOfErrorsToDisplay.push({ text: error });
    //                });
    //                Swal.mixin({
    //                    confirmButtonText: 'Ok',
    //                    type: "warning",
    //                }).queue(arrayOfErrorsToDisplay)
    //                    .then((result) => {
    //                        if (result.value) {
    //                        }
    //                    });
    //            }
    //            if (Voudet == 'Y') {
    //                if (arr.Table.length > 0) {
    //                    $('#VoucherDetail tbody tr').remove();
    //                    var arrSupp = arr.Table.filter(v => (v.type == "Cust" || v.type == "Bank" || v.type == "Supp" || v.type == "TSupp"));/*chnges for tds on OC for TSupp  by hina sharma on 09-07-2024*/
    //                    var mainSuppGl = arrSupp.filter(v => v.acc_id == cust_acc_id);
    //                    var TdsSuppGl = arrSupp.filter(v => v.acc_id == cust_acc_id && v.type == "TSupp");/*chnges for tds on OC for TSupp  by hina sharma on 09-07-2024*/
    //                    var NewArrSupp = arrSupp.filter(v => v.acc_id != cust_acc_id).sort((a, b) => a.acc_id - b.acc_id/*chnges for tds on Oc for sort acc_id by hina sharma on 09-07-2024*/);
    //                    if (TdsSuppGl.length > 0) {/*chnges for tds on OC by hina sharma on 09-07-2024*/
    //                        NewArrSupp.unshift(TdsSuppGl[0]);
    //                    }
    //                    NewArrSupp.unshift(mainSuppGl[0]);
    //                    arrSupp = NewArrSupp;
    //                    for (var j = 0; j < arrSupp.length; j++) {
    //                        let cust_id = arrSupp[j].id;
    //                        let supp_type = arrSupp[j].type;/*chnges for tds on OC by hina sharma on 09-07-2024*/
    //                        let supp_bill_no = arrSupp[j].bill_no;
    //                        let supp_bill_dt = arrSupp[j].bill_dt;

    //                        //let arrDetail = arr.Table.filter(v => (v.id == cust_id && (v.type == "Cust" || v.type == "Bank" || v.type == "Supp")));
    //                        //let arrDetailDr = arr.Table.filter(v => (v.parent == cust_id && v.type != "OcTax"));
    //                        let arrDetailDr;
    //                        if (supp_type == "TSupp") {  /*chnges for tds on OC by hina sharma on 09-07-2024*/
    //                            arrDetail = arr.Table.filter(v => (v.id == cust_id && (v.type == "TSupp")));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == cust_id && v.type == "Tds"));
    //                        } else {
    //                            arrDetail = arr.Table.filter(v => (v.id == cust_id && (v.type == "Cust" || v.type == "Bank" || v.type == "Supp") && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                            arrDetailDr = arr.Table.filter(v => (v.parent == cust_id && v.type != "OcTax" && v.type != "Tds" && v.bill_no == supp_bill_no && v.bill_dt == supp_bill_dt));
    //                        }

    //                        let RcmValue = 0;
    //                        let RcmValueInBase = 0;
    //                        if (supp_type != "TSupp") { /* chnges for tds on OC by hina sharma on 10-07-2024 */
    //                            arr.Table.filter(v => (v.parent == cust_id && v.type == "RCM"))
    //                                .map((res) => {
    //                                    RcmValue = RcmValue + res.Value;
    //                                    RcmValueInBase = RcmValueInBase + res.ValueInBase;
    //                                });
    //                        }
    //                        arrDetail[0].Value = arrDetail[0].Value - RcmValue;
    //                        arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

    //                        let rowSpan = 1;//arrDetailDr.length + 1;
    //                        let GlRowNo = 1;
    //                        // First Row Generated here for all GL Voucher 
    //                        let vouType = "";
    //                        if (arrDetail[0].type == "TSupp")  /*chnges for tds on OC by hina sharma on 10-07-2024*/
    //                            vouType = "DN"
    //                        if (arrDetail[0].type == "Cust")
    //                            vouType = "SV"
    //                        if (arrDetail[0].type == "Supp")
    //                            vouType = "PV"
    //                        if (arrDetail[0].type == "Bank")
    //                            vouType = "BP"

    //                        if (vouType == "DN") {  /*chnges for tds on OC by hina sharma on 10-07-2024*/
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        }
    //                        else if (vouType == "SV") {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        }
    //                        else {
    //                            GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
    //                                , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
    //                                , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
    //                        }
    //                        let ArrTax = [];
    //                        let ArrGlDetailsDr = [];
    //                        for (var k = 0; k < arrDetailDr.length; k++) {
    //                            let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                            if (getAccIndex > -1) {
    //                                ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                            } else {
    //                                if (arrDetailDr[k].type == "Tax") {
    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
    //                                    } else {
    //                                        ArrTax.push(arrDetailDr[k]);
    //                                    }
    //                                }
    //                                else if (arrDetailDr[k].type == "OcTax") {
    //                                }
    //                                else {
    //                                    ArrGlDetailsDr.push(arrDetailDr[k]);
    //                                }
    //                            }
    //                            if (arrDetailDr[k].type == "OC") {

    //                                let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].oc_id && v.type == "OcTax"));
    //                                // Grouping the similer tax account
    //                                for (var l = 0; l < arrDetailOCDr.length; l++) {

    //                                    let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
    //                                    if (getAccIndex > -1) {
    //                                        ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
    //                                        ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
    //                                    } else {
    //                                        ArrTax.push(arrDetailOCDr[l]);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        var ArrGLDetailRCM = []
    //                        for (var i = 0; i < ArrGlDetailsDr.length; i++) {

    //                            if (ArrGlDetailsDr[i].type == "RCM") {
    //                                ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

    //                            } else {
    //                                GlRowNo++;
    //                                rowSpan++;
    //                                if (ArrGlDetailsDr[i].type == "Tds") {/*chnges for tds on OC by hina sharma on 10-07-2024*/
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                }
    //                                else if (vouType == "SV") {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                }
    //                                else {
    //                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                        , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
    //                                        vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                        , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                    )
    //                                }
    //                            }
    //                        }
    //                        for (var i = 0; i < ArrTax.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            // Row Generated here for Tax on Other Charge
    //                            if (vouType == "SV") {
    //                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
    //                                    , 0, 0, ArrTax[i].Value, ArrTax[i].ValueInBase, vouType,
    //                                    ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
    //                                )
    //                            }
    //                            else {
    //                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
    //                                    , ArrTax[i].Value, ArrTax[i].ValueInBase, 0, 0, vouType,
    //                                    ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
    //                                )
    //                            }
    //                        }
    //                        for (var i = 0; i < ArrGLDetailRCM.length; i++) {
    //                            GlRowNo++;
    //                            rowSpan++;
    //                            if (vouType == "SV") {
    //                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
    //                                    , ArrGLDetailRCM[i].acc_id, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase, 0, 0
    //                                    , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
    //                                    , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
    //                                )
    //                            }
    //                            else {
    //                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
    //                                    , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
    //                                    , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
    //                                    , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
    //                                )
    //                            }
    //                        }
    //                        $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
    //                        $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
    //                    }
    //                }
    //            }
    //            BindDDLAccountList();
                
    //        }
    //    }
    //});
    //await CalculateVoucherTotalAmount();
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
function Get_Gl_Narration(VouType, bill_no, bill_date, type) {
    let Narration = "";
    switch (VouType) {
        case "DN":
            Narration = $("#hdn_DN_Narration").val();
            if (type == "TCust" || type == "Tcs")
                Narration = $("#hdn_DN_Narration_Tcs").val();
            else
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

//async function CalculateVoucherTotalAmount() {/*commented and modify by Hina sharma on 13-01-2025 take reference of service purchase invoice*/
    
//     ValDecDigit = $("#ValDigit").text();
    


//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        var currentRow = $(this);
//        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//            DrTotAmt = parseFloat((parseFloat(DrTotAmt) + parseFloat(0))).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmt = parseFloat((parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text()))).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
//            DrTotAmtInBase = parseFloat((parseFloat(DrTotAmtInBase) + parseFloat(0))).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmtInBase = parseFloat((parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text()))).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//            CrTotAmt = parseFloat((parseFloat(CrTotAmt) + parseFloat(0))).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmt = parseFloat((parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text()))).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
//            CrTotAmtInBase = parseFloat((parseFloat(CrTotAmtInBase) + parseFloat(0))).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmtInBase = parseFloat((parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text()))).toFixed(ValDecDigit);
//        }
//    });
//    $("#DrTotalInBase").text(DrTotAmt);
//    $("#DrTotalInBase").text(DrTotAmtInBase);
//    $("#CrTotalInBase").text(CrTotAmt);
//    $("#CrTotalInBase").text(CrTotAmtInBase);

//    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        
//        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            
//            await AddRoundOffGL();
//        }
//    }
//}
async function CalculateVoucherTotalAmount() {
    debugger;
    console.log("CalculateVoucherTotalAmount")
    var ValDecDigit = $("#ValDigit").text();
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    //$("#VoucherDetail >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
    //    }
    //    if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
    //    }
    //    else {
    //        CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
    //    }
    //});
    var VouDrCrList = [];
    $("#VoucherDetail >tbody >tr").each(function (index) {
        //
        var currentRow = $(this);
        var vou_sr_no = currentRow.find("#td_vou_sr_no").text();
        var dr_amt = currentRow.find("#dramt").text();
        var cr_amt = currentRow.find("#cramt").text()
        var dr_amt_bs = currentRow.find("#dramtInBase").text()
        var cr_amt_bs = currentRow.find("#cramtInBase").text()

        var getVouIndex = VouDrCrList.findIndex(m => m.vou_sr_no == vou_sr_no);
        if (getVouIndex > -1) {
            VouDrCrList[getVouIndex].dr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_sp) + parseFloat(dr_amt)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].dr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_base) + parseFloat(dr_amt_bs)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].cr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_sp) + parseFloat(cr_amt)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].cr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_base) + parseFloat(cr_amt_bs)).toFixed(cmn_ValDecDigit);
            VouDrCrList[getVouIndex].vou_entries = VouDrCrList[getVouIndex].vou_entries + 1;
        } else {
            VouDrCrList.push({
                vou_sr_no: vou_sr_no
                , dr_in_sp: dr_amt, dr_in_base: dr_amt_bs
                , cr_in_sp: cr_amt, cr_in_base: cr_amt_bs, vou_entries: 1, all_previous_entries: index
            });
        }
    });

    var flag_check_diff = "N";
    VouDrCrList.map((item, index) => {
        DrTotAmt = parseFloat(DrTotAmt) + parseFloat(item.dr_in_sp);
        DrTotAmtInBase = parseFloat(DrTotAmtInBase) + parseFloat(item.dr_in_base);
        CrTotAmt = parseFloat(CrTotAmt) + parseFloat(item.cr_in_sp);
        CrTotAmtInBase = parseFloat(CrTotAmtInBase) + parseFloat(item.cr_in_base);

        if (parseFloat(item.dr_in_base) != parseFloat(item.cr_in_base)) {

            if (Math.abs(parseFloat(item.dr_in_base) - parseFloat(item.cr_in_base)) < 1) {
                flag_check_diff = "Y";
            }

        }
    });

    $("#DrTotal").text(parseFloat(DrTotAmt).toFixed(ValDecDigit));
    $("#DrTotalInBase").text(parseFloat(DrTotAmtInBase).toFixed(ValDecDigit));
    $("#CrTotal").text(parseFloat(CrTotAmt).toFixed(ValDecDigit));
    $("#CrTotalInBase").text(parseFloat(CrTotAmtInBase).toFixed(ValDecDigit));

    //if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {

    //    if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
    //        await AddRoundOffGL();
    //    }

    //}

    if (flag_check_diff == "Y") {
        await AddRoundOffGL();
    }

}
async function AddRoundOffGL() {/*Commented and modify by Hina sharma on 13-01-2025 take reference of Service purchase invoice*/

    var ValDecDigit = $("#ValDigit").text();///Amount
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
                    var VouDrCrList = [];
                    $("#VoucherDetail >tbody >tr").each(function (index) {
                        //
                        var currentRow = $(this);
                        var vou_sr_no = currentRow.find("#td_vou_sr_no").text();
                        var dr_amt = currentRow.find("#dramt").text();
                        var cr_amt = currentRow.find("#cramt").text()
                        var dr_amt_bs = currentRow.find("#dramtInBase").text()
                        var cr_amt_bs = currentRow.find("#cramtInBase").text()

                        var getVouIndex = VouDrCrList.findIndex(m => m.vou_sr_no == vou_sr_no);
                        if (getVouIndex > -1) {
                            VouDrCrList[getVouIndex].dr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_sp) + parseFloat(dr_amt)).toFixed(cmn_ValDecDigit);
                            VouDrCrList[getVouIndex].dr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].dr_in_base) + parseFloat(dr_amt_bs)).toFixed(cmn_ValDecDigit);
                            VouDrCrList[getVouIndex].cr_in_sp = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_sp) + parseFloat(cr_amt)).toFixed(cmn_ValDecDigit);
                            VouDrCrList[getVouIndex].cr_in_base = parseFloat(parseFloat(VouDrCrList[getVouIndex].cr_in_base) + parseFloat(cr_amt_bs)).toFixed(cmn_ValDecDigit);
                            VouDrCrList[getVouIndex].vou_entries = VouDrCrList[getVouIndex].vou_entries + 1;
                        } else {
                            VouDrCrList.push({
                                vou_sr_no: vou_sr_no
                                , dr_in_sp: dr_amt, dr_in_base: dr_amt_bs
                                , cr_in_sp: cr_amt, cr_in_base: cr_amt_bs, vou_entries: 1, all_previous_entries: index
                            });
                        }
                    });

                    console.log("AddRoundOffGL");
                    debugger
                    var CountEntries = 0;
                    VouDrCrList.map((item, index) => {

                        if (Math.abs(parseFloat(item.cr_in_base) - parseFloat(item.dr_in_base)) < 1) {
                            if (parseFloat(item.dr_in_base) != parseFloat(item.cr_in_base)) {
                                if (parseFloat(item.dr_in_base) < parseFloat(item.cr_in_base)) {
                                    var Diff = parseFloat(item.cr_in_sp) - parseFloat(item.dr_in_sp);
                                    var DiffInBase = parseFloat(item.cr_in_base) - parseFloat(item.dr_in_base);

                                    var rowIdx = $('#VoucherDetail tbody tr').length;
                                    for (var j = 0; j < arr.Table.length; j++) {
                                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                            var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                            GlSrNo = SrRows[index].textContent;
                                            let spanRowCount = 1;
                                            $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                                var row = $(this);
                                                if (row.text() == item.vou_sr_no) {
                                                    spanRowCount++;
                                                }
                                            });

                                            GlTableRenderHtml(item.vou_sr_no, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                                , Diff, DiffInBase, 0, 0, "SV", $("#Hdn_ddlCurrency").val()
                                                , $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())

                                            var vouRow = $('#VoucherDetail > tbody > #tr_GlRow' + item.vou_sr_no);
                                            vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                            var table = document.getElementById("VoucherDetail");
                                            rows = table.tBodies[0].rows;
                                            let ib = item.all_previous_entries + item.vou_entries + CountEntries;
                                            CountEntries = CountEntries + 1;
                                            let LastRow = $('#VoucherDetail tbody tr').length;
                                            rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                        }
                                    }
                                }
                                else {
                                    var Diff = parseFloat(item.dr_in_sp) - parseFloat(item.cr_in_sp);
                                    var DiffInBase = parseFloat(item.dr_in_base) - parseFloat(item.cr_in_base);
                                    var rowIdx = $('#VoucherDetail tbody tr').length;
                                    for (var j = 0; j < arr.Table.length; j++) {
                                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                            var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                            GlSrNo = SrRows[index].textContent;
                                            let spanRowCount = 1;
                                            $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                                var row = $(this);
                                                if (row.text() == item.vou_sr_no) {
                                                    spanRowCount++;
                                                }
                                            });
                                            GlTableRenderHtml(item.vou_sr_no, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                                , 0, 0, Diff, DiffInBase, "SV"
                                                , $("#Hdn_ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val()
                                                , $("#Bill_Date").val())
                                            var vouRow = $('#VoucherDetail tbody #tr_GlRow' + item.vou_sr_no);
                                            vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                            vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                            var table = document.getElementById("VoucherDetail");
                                            rows = table.tBodies[0].rows;
                                            let ib = item.all_previous_entries + item.vou_entries + CountEntries;
                                            CountEntries = CountEntries + 1;
                                            let LastRow = $('#VoucherDetail tbody tr').length;
                                            rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                        }
                                    }
                                }
                            }
                        }

                    });
                    

                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
//async function AddRoundOffGL() {/*commented and modify by Hina sharma on 13-01-2025*/
    
   
//        ValDecDigit = $("#ValDigit").text();///Amount
   

//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
            
//            if (data == 'ErrorPage') {
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
                
//                if (arr.Table.length > 0) {
//                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
//                    $("#VoucherDetail >tbody >tr").each(function () {
//                        var currentRow = $(this);
//                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//                            DrTotAmt = parseFloat((parseFloat(DrTotAmt) + parseFloat(0))).toFixed(ValDecDigit);
//                        }
//                        else {
//                            DrTotAmt = parseFloat((parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text()))).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//                            CrTotAmt = parseFloat((parseFloat(CrTotAmt) + parseFloat(0))).toFixed(ValDecDigit);
//                        }
//                        else {
//                            CrTotAmt = parseFloat((parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text()))).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
//                            DrTotAmtInBase = parseFloat((parseFloat(DrTotAmtInBase) + parseFloat(0))).toFixed(ValDecDigit);
//                        }
//                        else {
//                            DrTotAmtInBase = parseFloat((parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text()))).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
//                            CrTotAmtInBase = parseFloat((parseFloat(CrTotAmtInBase) + parseFloat(0))).toFixed(ValDecDigit);
//                        }
//                        else {
//                            CrTotAmtInBase = parseFloat((parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text()))).toFixed(ValDecDigit);
//                        }
//                    });
                    
//                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
//                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
//                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
//                                    //var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
//                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
//                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
//                                    let spanRowCount = 1;

//                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
//                                        var row = $(this);
//                                        if (row.text() == 1) {
//                                            spanRowCount++;
//                                        }
//                                    });
//                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
//                                    /* GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id*/

//                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
//                                        , Diff, DiffInBase, 0, 0, "SV"
//                                        , $("#hdcurr").val(), '1', '', '')
//                                    /*, $("#ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())*/

//                                    //var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
//                                    //vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);

//                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
//                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

//                                    var table = document.getElementById("VoucherDetail");
//                                    rows = table.tBodies[0].rows;
//                                    let ib = spanRowCount - 1;
//                                    let LastRow = $('#VoucherDetail tbody tr').length;
//                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
//                                }
//                            }
//                        }
//                        else {
//                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
//                            var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
//                                    //var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
//                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
//                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
//                                    let spanRowCount = 1;

//                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
//                                        var row = $(this);
//                                        if (row.text() == 1) {
//                                            spanRowCount++;
//                                        }
//                                    });
//                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
//                                    /* GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id*/
//                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
//                                        , 0, 0, Diff, DiffInBase, "SV"
//                                        , $("#hdcurr").val(), '1', '', '')
//                                    /*, $("#ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())*/

//                                    //var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
//                                    //vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
//                                    //vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);

//                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
//                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
//                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

//                                    var table = document.getElementById("VoucherDetail");
//                                    rows = table.tBodies[0].rows;
//                                    let ib = spanRowCount - 1;
//                                    let LastRow = $('#VoucherDetail tbody tr').length;
//                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
//                                }
//                            }
//                        }
//                    }
                    
                    
//                }
//            }
//        }
//    });
//    await CalculateVoucherTotalAmount();
//}
/*---------End Gl Change By Hina on 24-07-2024 for multiple supplier---------*/
//async function CalculateVoucherTotalAmount() {
//    
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        //
//        var currentRow = $(this);
//        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//        }
//    });
//    
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    
//    //if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//    //    
//    //    AddRoundOffGL();
//    //}
    
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//        
//        if (Math.abs(parseFloat(DrTotAmt) - parseFloat(CrTotAmt)) < 1) {
//            
//            await AddRoundOffGL();
//        }
//    }
//}
//async function AddRoundOffGL() {
//    
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
//            
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                
//                if (arr.Table.length > 0) {
//                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    $("#VoucherDetail >tbody >tr").each(function () {
//                        //
//                        var currentRow = $(this);
//                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//                        }
//                    });
//                    
//                    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//                        if (parseFloat(DrTotAmt) < parseFloat(CrTotAmt)) {
//                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                   
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding" id="SrNoGL">${rowIdx}</td>
//                                    <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                                    <td class="center">
//                                        <button type="button" disabled style="filter: grayscale" id="BtnCostCenterDetail"  class="calculator " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>
                                    
//                            </tr>`);

                                    
//                                }
//                            }
//                        }
//                        else {
//                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    
//                                   $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding" id="SrNoGL">${rowIdx}</td>
//                                    <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                                    <td class="center">
//                                        <button type="button" disabled style="filter: grayscale" id="BtnCostCenterDetail"  class="calculator " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>
                                   
//                            </tr>`);

                                    
//                                } 
//                            }
//                        }
//                    }
//                    
//                    CalculateVoucherTotalAmount();
//                }
               
//            }
//        }
//    });
//}

//--------------------------------------//

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
function OnClickSaveAndExit() {
    
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
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
    //var FTaxDetails = JSON.parse(sessionStorage.getItem("PI_TaxCalcDetails"));
    /* if (FTaxDetails != null) {*/
    /* if (("#TaxCalculatorTbl_New >tbody >tr").length > 0){*/
     
    if (("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
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
            } else {
                if (/*TaxUserID == UserID && */TaxItemID == TaxItmCode && DocNo == GRNNo && DocDate == GRNDate) {
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
                    //Added ItemRow by hina on 10-01-2025 for manual gst
                    var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                    var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                    if (TaxExempted == false) {//Added if (TaxExempted == false) by hina on 10-01-2025 for manual gst
                        NewArr.push({ /*UserID: UserID, */DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
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
            //
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
            //Added ItemRow by hina on 10-01-2025 for manual gst
            var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({ /*UserID: UserID, */DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
            }
            });
    
        //sessionStorage.removeItem("PI_TaxCalcDetails");
        //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(NewArr));
        // BindTaxAmountDeatils(NewArr, "");
    }
    else {
        //  var TaxCalculationList = [];
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
            //Added ItemRow by hina on 10-01-2025 for manual gst
            var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr');
            var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
            if (TaxExempted == false) {
                NewArr.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
            }
            });
        //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(TaxCalculationList));
        //BindTaxAmountDeatils(TaxCalculationList, "");
    }
    if (TaxOn != "OC") {
        //BindTaxAmountDeatils(NewArr, "");
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
                //var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit)
                //currentRow.find("#OCTotalTaxAmt").text(Total);
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
            //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (ItmCode == TaxItmCode /*&& GRNNoTbl == GRNNo && GRNDateTbl == GRNDate*/) {
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
               /* currentRow.find("#TxtNetValue").val(NetOrderValueInBase);*/
                FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
            }
        });
        CalculateAmount();
    }
    if ($("#taxTemplate").text() == "GST Slab") {/*add by hina on 10-01-2025 for maual gst*/
        GetAllGLID();
    }
    //GetAllGLID();
}

function OnClickReplicateOnAllItems() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var GRNNo = $("#TaxCalcGRNNo").val();
    var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueInBase = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "ScrapSIItemTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = currentRow.find("#TxtGrnNo").val();
                GRNDateTbl = currentRow.find("#hfGRNDate").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                } else {
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#TxtItemGrossValue").val();
                }


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                for (i = 0; i < TaxCalculationList.length; i++) {
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationList[i].TaxName;
                    var TaxNameID = TaxCalculationList[i].TaxNameID;
                    var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                    TaxPercentage = TaxCalculationList[i].TaxPercentage;
                    var TaxLevel = TaxCalculationList[i].TaxLevel;
                    var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
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
                    NewArray.push({ /*UserID: UserID,*/ DocNo: GRNNoTbl, DocDate: GRNDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            var DocNo = NewArray[k].DocNo;
                            var DocDate = NewArray[k].DocDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            if (CitmTaxItmCode != TaxItmCode && GRNNo == DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate == DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && GRNNo != DocNo && GRNDate != DocDate) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
    //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            
            <td id="DocNo">${TaxCalculationListFinalList[i].DocNo}</td>
            <td id="DocDate">${TaxCalculationListFinalList[i].DocDate}</td>
            <td id="TaxItmCode">${TaxCalculationListFinalList[i].TaxItmCode}</td>
            <td id="TaxName">${TaxCalculationListFinalList[i].TaxName}</td>
            <td id="TaxNameID">${TaxCalculationListFinalList[i].TaxNameID}</td>
            <td id="TaxPercentage">${TaxCalculationListFinalList[i].TaxPercentage}</td>
            <td id="TaxLevel">${TaxCalculationListFinalList[i].TaxLevel}</td>
            <td id="TaxApplyOn">${TaxCalculationListFinalList[i].TaxApplyOn}</td>
            <td id="TaxAmount">${TaxCalculationListFinalList[i].TaxAmount}</td>
            <td id="TotalTaxAmount">${TaxCalculationListFinalList[i].TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxCalculationListFinalList[i].TaxApplyOnID}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    //
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //
            var OCValue = currentRow.find("#OCValue").text();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
                        if (OCValue == TaxItmCode) {
                            currentRow.find("#OCTaxAmt").text(TotalTaxAmtF);
                            var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(DecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(DecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    } else {
        $("#ScrapSIItemTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            // 
            var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID && GRNNoTbl == AGRNNo && GRNDateTbl == AGRNDate) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#Txtitem_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                            NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                           /* currentRow.find("#TxtNetValue").val(NetOrderValueInBase);*/
                            FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                            currentRow.find("#NetValueinBase").val(FinalNetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                    currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                    /*currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                    FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                    currentRow.find("#NetValueinBase").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
               /* currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                currentRow.find("#NetValueinBase").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
        GetAllGLID();
    }

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
    CancelledRemarks("#Cancelled", "Enable");
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    

    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/

    /* CMNBindTaxAmountDeatils(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);*//*commented by Hina on 10-01-2025 for using New common function */
   
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);

    if (bindval == "") {
        GetAllGLID();
    }
}
function AfterDeleteResetSI_ItemTaxDetail() {
    //
    var ScrapSIItemTbl = "#ScrapSIItemTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ItemName = "#ItemName";
    CMNAfterDeleteReset_ItemTaxDetailModel(ScrapSIItemTbl, SNohiddenfiled, ItemName);
}

function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//---------------------------------------------//

function AddNewRow() {
    var rowIdx = 0;
    //
    /*var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#ScrapSIItemTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#ScrapSIItemTbl >tbody >tr").each(function (i, row) {
        //
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
    
    var docid = $("#DocumentMenuId").val();

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
    /*Code start Add by Hina Sharma on 10-01-2025 to add for TaxExempted */
    var ManualGst = "";
    var TaxExempted = "";
    var FOC = "";
    var Disable = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if ($("#nontaxable").is(":checked")) {
        Disable = "disabled";
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" disabled id="ManualGST"><label class="custom-control-label" for="" style="padding:3px 0px;"> </label></div></td>'
        }
        FOC = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" checked class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted" disabled><label class="custom-control-label" for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    else {
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" for="" style="padding:3px 0px;"> </label></div></td>'
        }
        FOC = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" for="" style="padding: 3px 0px;"> </label></div></td>'
    }

    /*Code End Add by Hina Sharma on 10-01-2025 to add for TaxExempted */

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
                                                                            <input id="ItemType" class="form-control" autocomplete="off" type="text" name="ItemType" value="" placeholder="${$("#ItemType").text()}" disabled>
                                                                        </div>
                                                                    </td>
<td><input id="HsnNo${RowNo}" class="form-control date" autocomplete="off" type="text" name="Hsn"  placeholder="${$("#Hsn").text()}" disabled></td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" onmouseover="OnMouseOver(this)" value="">
                                                                            </td>
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
`+FOC+`
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
function OnClickIconBtn(e) {
    
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#ItemName" + Sno).val();
    ItemInfoBtnClick(ItmCode)

}
function FloatValuePerOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    //$("#SpanTaxPercent").css("display", "none");
    return true;
}
function BindScrpItmList(ID) {
    //
    //BindItemList("#ItemName", ID, "#ScrapSIItemTbl", "#SNohiddenfiled", "", "ScrapSI");
    BindPItemList("#ItemName", ID, "#ScrapSIItemTbl", "#SNohiddenfiled", "", "ScrapSI");
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
function OnChangeServiceItemName(RowID, e) {
    BindServcItemListOnChng(e);
}
function OnChangeDiscountInPerc(e) {
    var clickedrow = $(e.target).closest("tr");
    CalculateDisPercent(clickedrow);
}
async function CalculateDisPercent(clickedrow) {
    debugger;
    //var DocumentMenuId = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
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
        //return false;
    }
    if (parseFloat(CheckNullNumber(DisPer)) >= 100) {
        clickedrow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
        clickedrow.find("#item_disc_percError").css("display", "block");
        clickedrow.find("#DiscountInPerc").css("border-color", "red");
        //clickedrow.find("#item_disc_amt").prop("readonly", true);
        //clickedrow.find("#item_disc_amt").val("");
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
        //clickedrow.find("#item_ass_val").val(FinGVal);
            //clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalFinGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#NetValueinBase").val(FinalFinGVal);
        clickedrow.find("#DiscountValue").val(FinDisVal);
        CalculateAmount();

        clickedrow.find("#DiscountInPerc").val(parseFloat(DisPer).toFixed(2));
        //clickedrow.find("#item_disc_amt").prop("readonly", true);
        //clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#TxtItemGrossValue").val(FinVal);
            //clickedrow.find("#item_ass_val").val(FinVal);
            //clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
            clickedrow.find("#NetValueinBase").val(FinalFinVal);
            clickedrow.find("#DiscountValue").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateAmount();
        }
        //clickedrow.find("#item_disc_amt").prop("readonly", false);

    }
    OnChangeGrossAmt();
    if ($("#nontaxable").is(":checked")) {

    } else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    GetAllGLID();
    let cust_id = $("#CustomerName").val();
    let GrossValue = $("#TxtGrossValue").val();
    await AutoTdsApply(cust_id, GrossValue);
}
async function BindServcItemListOnChng(e) {
    
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    /*var Itm_ID;*/
    var NewItm_ID;/*Rename by Hina on 06-08-2024 for differentiate old and new item id*/
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


async function ClearRowDetails(e, ItemID) {
    
    var docid = $("#DocumentMenuId").val();
    var DecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#HsnNo").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#TxtInvoiceQuantity").val("");
    clickedrow.find("#TxtRate" ).val("");
    clickedrow.find("#TxtItemGrossValue").val("");
    clickedrow.find("#Txtitem_tax_amt").val("");
    clickedrow.find("#TxtOtherCharge").val("");
    clickedrow.find("#NetValueinBase").val("");
    clickedrow.find("#AvailableStock").val("");
    clickedrow.find("#wh_id" + SNo).val(0).trigger('change.select2'); 
    var ddlId = "#wh_id" + SNo;
    var whERRID = "#wh_Error";          
    clickedrow.find(whERRID).css("display", "none");
    clickedrow.find(ddlId).css("border-color", "#ced4da");

/*    CalculateAmount("pm_flag");*//*commented and modify by hina on 13-01-2025*/
    
    CalculateAmount();
    let cust_id = $("#CustomerName").val();
    let GrossValue = $("#TxtGrossValue").val();
    await AutoTdsApply(cust_id, GrossValue);
    //var TOC_Amount = $("#TxtOtherCharges").val();
    //if (TOC_Amount == null || TOC_Amount =="") {
    //    TOCAmount = parseFloat(CheckNullNumber(TOC_Amount)).toFixed(DecDigit);
    //    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
    //        TOCAmount = 0;
    //    }
    //}
    //var TOCAmount = parseFloat($("#TxtOtherCharges").val());//Commented by Suraj Maurya on 06-01-2025
    var TOCAmount = parseFloat($("#TxtDocSuppOtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetSI_ItemTaxDetail();
   
}
function DisableHeaderField() {
    //
    
    $("#CustomerName").attr('disabled', true);
    //$("#ddlCurrency").attr('disabled', true);
    //$("#conv_rate").prop("readonly", true);
}
async function OnChangePOItemQty(RowID, e) {
    //
    await CalculationBaseQty(e);
    GetAllGLID();
}
async function OnChangePOItemRate(RowID, e) {
    debugger;
    await CalculationBaseRate(e);
    GetAllGLID();
}

async function CalculationBaseQty(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    var docid = $("#DocumentMenuId").val();
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
        /*CalculateAmount("pm_flag");*//*commented and modify by hina on 13-01-2025*/
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
    let cust_id = $("#CustomerName").val();
    let GrossValue = $("#TxtGrossValue").val();
    await AutoTdsApply(cust_id, GrossValue).then(() => {
        //GetAllGLID();
    }).catch(err => console.log(err));
    
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
    //if (currentrow.find("#FOC").is(":checked")) {
    //    //FOCDisabledAndEnable(currentrow, "Y");
    //}
    //else {
    if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {
        if (currentrow.find("#FOC").is(":checked")) {
            //FOCDisabledAndEnable(currentrow, "Y");
        }
        else {
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
            
        }
        else {
            clickedrow.find("#TxtRateError").css("display", "none");
            clickedrow.find("#TxtRate").css("border-color", "#ced4da");
        }
   // }
    

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#TxtRate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinVal);       
       
        FinalVal = FinVal * ConvRate
        clickedrow.find("#NetValueinBase").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        /* CalculateAmount("pm_flag");*//*commented and modify by hina sharma on 13 - 01 - 2025*/
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00" && DisPer !== "0" && DisPer !== "0.000" && DisPer !== "0.0000") {
        CalculateDisPercent(clickedrow);
    }
    clickedrow.find("#TxtRate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    /*----Code start Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
    if ($("#nontaxable").is(":checked")) {

    }
    else if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());
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
    
    /*----Code End Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
    //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#TxtItemGrossValue").val());/*Commented by Hina sharma on 10-01-2025 to add for TaxExempted */
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
    let cust_id = $("#CustomerName").val();
    let GrossValue = $("#TxtGrossValue").val();
    await AutoTdsApply(cust_id, GrossValue);
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
    var UserID = $("#UserID").text();
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
                /*----Code start Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
                var ItemRow = $("#ScrapSIItemTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
                var TaxExempted = ItemRow.find("#TaxExempted").is(":checked");
                /*----Code End Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
                if (TaxItemID == ItmCode) {
                    /*----Code start Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
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
                    /*----Code End Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
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
                    //
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
                if (TaxExempted == false) {/*----Code Add if (TaxExempted == false) by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
                    NewArray.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID
                        , TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount
                        , TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID
                    });
                }

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if ( TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
           
            $("#ScrapSIItemTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
                
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#ItemName" + Sno).val();

                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            /*----Code Start Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
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
                            /*----Code End Add by Hina Sharma on 10-01-2025 to add for TaxExempted -----*/
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (/*RowSNoF == RowSNo && */TaxItmCode == ItmCode) {                             
                                currentRow.find("#Txtitem_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                //if (currentRow.find("#TxtOtherCharges").val() != null && currentRow.find("#TxtOtherCharges").val() != "") {
                                //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharges").val()).toFixed(DecDigit);
                                //}
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
                        //if (currentRow.find("#TxtOtherCharges").val() != null && currentRow.find("#TxtOtherCharges").val() != "") {
                        //    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharges").val()).toFixed(DecDigit);
                        //}
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit); 
                        //FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        /*For This page, there will be not apply conv_rate */
                        FinalFGrossAmtOR = FGrossAmtOR
                        currentRow.find("#NetValueinBase").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
           /* CalculateAmount("pm_flag");*//*commented and modify by hina sharma on 13 - 01 - 2025*/
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
/*----Start code by Hina Sharma on 10-01-2025 for tax exempted and manual gst-----*/
function OnClickTaxExemptedCheckBox(e) { /*Code start Add by Hina Sharma on 10-01-2025 to add for TaxExempted */
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
            if (currentrow.find("#FOC").is(":checked")) {

            }
            else {
                currentrow.find("#BtnTxtCalculation").prop("disabled", false);
            }
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
    //var DiscPer = $("#DiscountInPerc").val();
    //if (parseFloat(CheckNullNumber(DiscPer))) {
    //    OnchangeDiscInPerc();
    //}
}
function OnClickManualGSTCheckBox(e) {

    var currentrow = $(e.target).closest('tr');
    //var RowSNo = currentrow.find("#SNohiddenfiled").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        let zero = parseFloat(0).toFixed($("#ValDigit").text());
        currentrow.find("#Txtitem_tax_amt").val(zero);
        var itemId = currentrow.find("#hfItemID").val();
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
    //var DiscPer = $("#DiscountInPerc").val();
    //if (parseFloat(CheckNullNumber(DiscPer))) {
    //    OnchangeDiscInPerc();
    //}
}
//function OnchangeDiscInPerc() {
//    var DecDigit = $("#ValDigit").text();
//    var Perc = $("#DiscountInPerc").val();
//    //if (parseFloat(CheckNullNumber(Perc)) > parseFloat(0)) {
//    //    DisableDiscountAmt();
//    //    $("#OrderDiscountInAmount").attr("disabled", true);
//    //}
//    //else {
//    //    EnableDiscountAmt();
//    //    $("#OrderDiscountInAmount").attr("disabled", false);
//    //}
//    if (Perc == "") {
//        Perc = "0";
//    }
//    var TOtalGrossVal = parseFloat(0).toFixed(DecDigit);
//    $("#ScrapSIItemTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        var ord_qty_spec = currentRow.find("#TxtInvoiceQuantity").val();
//        var item_rate = CheckNullNumber(currentRow.find("#TxtRate").val());
//        var GrossVal = parseFloat(ord_qty_spec) * parseFloat(item_rate)
//        var DiscVal = ((parseFloat(GrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
//        if (parseFloat(Perc) > 0) {
//            currentRow.find("#DiscountInPerc").val(parseFloat(Perc).toFixed(2));
//        } else {
//            currentRow.find("#DiscountInPerc").val("");
//        }
//        currentRow.find("#DiscountValue").val(parseFloat(DiscVal).toFixed(DecDigit));
//        currentRow.find("#TxtItemGrossValue").val(parseFloat(GrossVal).toFixed(DecDigit));
//        //currentRow.find("#item_ass_val").val(parseFloat(GrossVal).toFixed(DecDigit));

//        TOtalGrossVal = parseFloat(TOtalGrossVal) + parseFloat(GrossVal);
//    });

//    var PercDiscVal = ((parseFloat(TOtalGrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
//    //var TxtGrossValue = TOtalGrossVal;
//    //var TxtGrossValue1 = (parseFloat(TxtGrossValue) - parseFloat(PercDiscVal)).toFixed(DecDigit);
//    if (PercDiscVal == 0) {
//        $("#DiscountInPerc").val("");
//    }
//    else {
//        $("#DiscountInPerc").val(Perc);
//    }
//    $("#ScrapSIItemTbl >tbody >tr").each(function () {
//        debugger;
//        var currentRow = $(this);
//        CalculateOrderDiscount(currentRow);
//    });
//    CalculateAmount();
//}
//function CalculateOrderDiscount(clickedrow) {
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var ExpImpValDigit = $("#ExpImpValDigit").text();

//    var DocumentMenuId = $("#DocumentMenuId").val();
//    var conv_rate = $("#conv_rate").val();
//    var item_gr_val = clickedrow.find("#item_gr_val").val();
//    var item_disc_val = clickedrow.find("#item_disc_val").val();
//    var Sno = clickedrow.find("#SNohiddenfiled").val();
//    var ItemName = clickedrow.find("#SOItemListName" + Sno).val();

//    var FinDisVal = parseFloat(item_gr_val) - parseFloat(item_disc_val);

//    var ord_qty_spec = clickedrow.find("#ord_qty_spec").val();
//    var item_rate = clickedrow.find("#item_rate").val();

//    var Price = parseFloat(ord_qty_spec) * parseFloat(item_rate);
//    if (DocumentMenuId == "105103145110") {
//        clickedrow.find("#item_gr_val").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
//        clickedrow.find("#item_ass_val").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
//        clickedrow.find("#item_net_val_spec").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
//        FinalFinVal = (FinDisVal * conv_rate).toFixed(ExpImpValDigit);
//    }
//    else {
//        clickedrow.find("#item_gr_val").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
//        clickedrow.find("#item_ass_val").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
//        clickedrow.find("#item_net_val_spec").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
//        FinalFinVal = (FinDisVal * conv_rate).toFixed(ValDecDigit);
//    }

//    clickedrow.find("#item_net_val_bs").val(FinalFinVal);

//    OnChangeGrossAmt();
//    var OrderType = $('#src_type').val();
//    if (OrderType == "D") {
//        clickedrow.find("#QuotationNumber").val("Direct")
//        QuotationDate = "Direct";
//    }
//    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());
//}
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
/*----End code by Hina Sharma on 10-01-2025 for tax exempted and manual gst-----*/

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    
    //var clickedrow = $(e.target).closest("tr");/*Commented and changes by Hina on 30-07-2024 to chng cost center for multi supplier for 3rd party OC*/
    //var CstDbAmt = clickedrow.find("#dramt").text();
    //var CstCrtAmt = clickedrow.find("#cramt").text();
    //var GLAcc_Name = clickedrow.find("#txthfAccID").val();
    //var GLAcc_id = clickedrow.find("#hfAccID").val();
    //var disableflag = ($("#txtdisable").val());
    //var NewArr = new Array();
    //$("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
    //    var row = $(this);
    //    var List = {};
    //    List.GlAccount = row.find("#hdntbl_GlAccountId").text();
    //    List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
    //    List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
    //    List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
    //    List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
    //    var amount = row.find("#hdntbl_CstAmt").text();
    //    List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
    //    NewArr.push(List);
    //})
    //var Amt;
    //if (CheckNullNumber(CstDbAmt) != "0") {
    //    Amt = CstDbAmt;
    //}
    //else {
    //    Amt = CstCrtAmt;
    //}
    //$.ajax({
    //    type: "POST",
    //    url: "/ApplicationLayer/ServiceSaleInvoice/GetCstCntrtype",
    //    data: {
    //        Flag: flag,
    //        Disableflag: disableflag,
    //        CC_rowdata: JSON.stringify(NewArr),
    //    },
    //    success: function (data) {
    //        
    //        $("#CostCenterDetailPopup").html(data);
    //        $("#CC_GLAccount").val(GLAcc_Name);
    //        $("#hdnGLAccount_Id").val(GLAcc_id);
    //        $("#GLAmount").val(Amt);
    //        $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
    //        $("#hdnTable_Id").text("VoucherDetail");
    //    },
    //})

    
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


function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    
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

function OnChangeWarehouse(el, evt) {
    
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SNohiddenfiled").val();
    var ddlId = "#wh_id" + Index;
    var whERRID = "#wh_Error";
    if (clickedrow.find(ddlId).val() == "0") {
        
        clickedrow.find(whERRID).text($("#valueReq").text());
        clickedrow.find(whERRID).css("display", "block");
        clickedrow.find(ddlId).css("border-color", "red");
    }
    else {
        var WHName = $("#wh_id option:selected").text();
        $("#hdwh_name").val(WHName)
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
    }

    var ItemId = clickedrow.find("#hfItemID").val();
    var WarehouseId = clickedrow.find(ddlId).val();
    var QtyDecDigit = $("#QtyDigit").text();
    //var CompId = $("#HdCompId").val();
    //var BranchId = $("#HdBranchId").val();
    
    if (WarehouseId != "0" && WarehouseId != null) {
        //$("#SaveItemBatchTbl TBODY TR").each(function () {
        //    var row = $(this);
        //    var rowitem = row.find("#PD_BatchItemId").val();
        //    if (rowitem == ItemId) {
        //        
        //        $(this).remove();
        //    }
        //});


        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
                //CompId: CompId,
                //BranchId: BranchId
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

function ItemStockWareHouseWise(el, evt) {
    try {
        
        var clickedrow = $(evt.target).closest("tr");
        var sno = clickedrow.find("#SNohiddenfiled").val()
        var ItemId = clickedrow.find("#hfItemID").val();
        var ItemName = clickedrow.find("#ItemName").val().trim();
       // var ItemName = clickedrow.find("#ItemName" + sno).val();
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
function BindWarehouseList(id) {
    //
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/GetWarehouseList1",
            data: {},
            dataType: "json",
            success: function (data) {
                
                if (data == 'ErrorPage') {
                    //GRN_ErrorPage();
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
                    }
                }
            },
        });
   
}
function getsubitm(e, ItemID)
{
    try {
        
        /**Added By Nitesh**/
        var clickedrow = $(e.target).closest("tr");
        var rowNo = clickedrow.find("#SNohiddenfiled").val();
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
                   // if (arr.Table[0] > 0) {

                   
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
function GetSubItemAvlStock(e) {
    
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    // var ProductNm = Crow.find("#ItemName" + rowNo).text();
    var ProductNm = Crow.find("#ItemName").val().trim();
    var ProductId = Crow.find("#hfItemID").val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStock").val();
    var hdwh_Id = Crow.find("#wh_id" + rowNo).val();
    var hd_Status = $("#hfStatus").val();
    if (hd_Status == "A") {
        $("#SubItemAvlStk").attr("data-target", "#SubItem");
        SubItemDetailsPopUp("AvlScrap", e,"Avl")
    }
    else {
        $("#SubItemAvlStk").attr("data-target", "#SubItemStock");
        Cmn_SubItemWareHouseAvlStock((ProductNm).trim(), ProductId, UOM, hdwh_Id, AvlStk, "wh");

    }
}



function SubItemDetailsPopUp(flag, e,flag1) {
    
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
   // var abc = clickdRow.find("#ItemName" + hfsno).text();


    var ProductNm = clickdRow.find("#ItemName").val().trim();
    var ProductId = clickdRow.find("#hfItemID").val();
    var wh_id = clickdRow.find("#wh_id" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var UOMID = clickdRow.find("#UOMID").val();

    var doc_no = $("#InvoiceNumber").val();
    var doc_dt = $("#Sinv_dt").val();

    //var UOMID = clickdRow.find("#UOMID").val();
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
    if (flag1 == "Avl") {
        Sub_Quantity = clickdRow.find("#AvailableStock").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ScrapSaleInvoice/GetSubItemDetails",
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


function ItemStockBatchWise(el, evt) {
    try {
        debugger;
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

        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#_mdlCommand").val();
        var TransType = $("#TransType").val();
        if (parseFloat(inv_qty) == "0" || parseFloat(inv_qty) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#TxtInvoiceQuantityError").text($("#FillQuantity").text());
            clickedrow.find("#TxtInvoiceQuantityError").css("display", "block");
            clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/ScrapSaleInvoice/getItemStockBatchWise",
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
                //var ItemId = clickedrow.find("#hdItemId").val();;
                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();            
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ScrapSaleInvoice/getItemStockBatchWise",
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
                            var Index = clickedrow.find("#SNohiddenfiled").val();
                            var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                            var ddlId = "#wh_id" + Index;
                            //var ItemId = clickedrow.find("#hdItemId").val();;
                            var WarehouseId = clickedrow.find(ddlId).val();
                            var CompId = $("#HdCompId").val();
                            var BranchId = $("#HdBranchId").val();

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(TxtInvoiceQuantity);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);                         
                            $("#BatchwiseTotalIssuedQuantity").val("");

                            //Added by Hina on 30-07-2024
                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
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
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var ddlId = "#wh_id" + Index;
                var WarehouseId = clickedrow.find(ddlId).val();
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/ScrapSaleInvoice/getItemStockBatchWiseAfterStockUpadte",
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
                //if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {
                if (CheckNullNumber(BatchInvoiceQty) > 0) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    //var ItemExpiryDate = row.find("#ExpiryDate").val();
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
                //clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
            }
        });
    }
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
              //  batchList.ExpiryDate = row.find('#scrap_lineBatchExpiryDate').val();
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
        var _mdlCommand = $("#_mdlCommand").val();
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
            if (Scrap_Status == "" || Scrap_Status == null  || Scrap_Status == "D") {
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
                        url: "/ApplicationLayer/ScrapSaleInvoice/getItemstockSerialWiseAfterStockUpadte",
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
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
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
                //clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
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
                    if (ItemId == bchitemid && UOMId == bchuomid ) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(inv_qty).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#TxtInvoiceQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "btnbatchdeatil", "N");
                }
                else {
                   // clickedrow.find("#TxtInvoiceQuantity").css("border-color", "red");
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
    

    var TotalTDS_Amount = $('#TotalTDS_Amount').text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //var TDS_OcId = $("#TDS_oc_id").text();
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    //var TDS_SuppId = $("#TDS_supp_id").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var GstCat = $("#Hd_GstCat").val();
    //var ToTdsAmt = 0;
    //ToTdsAmt = parseFloat(CheckNullNumber(CC_Clicked_Row.find("#OCAmount").text()));

    //Added by Suraj Maurya on 06-12-2024
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValue").val()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber($("#TDS_AssessableValueWithTax").val()));
    }
        //Added by Suraj Maurya on 06-12-2024 End
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
            /*  if (TDS_Amount > 0.00) {*/
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
            /* }*/

        });

    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
    //GetAllGLID();
}
function SetTds_Amt_To_OC() {
    var Errorflag = "";
    var DecDigit = $("#ValDigit").text();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var tdsval = currentRow.find("#TDS_val").text();
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);

    });
    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}
//function OnClickTDS_SaveAndExit() {
    

//    if ($("#hdn_tds_on").val() == "OC") {
//        OnClickTP_TDS_SaveAndExit();
//        $("#hdn_tds_on").val("");
//    }

//}
/***------------------------------TDS On Third Party End------------------------------***/

/***-------------------Roundoff add by Hina on 09-07-2024------------------------------------***/
function checkMultiSupplier() {
    //let countSupp = 0;
    //$("#VoucherDetail tbody tr").each(function () {
    //    let row = $(this);
    //    if (row.find("#type").val() == "Supp") {
    //        countSupp++;
    //    }
    //});
    //if (countSupp > 1) {
    //    $("#chk_roundoff").attr("disabled", true);
    //    return false;
    //} else {
    //    return true;
    //}
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

            //$("#ServicePIItemTbl > tbody > tr").each(function () {
            //    
            //    var currentrow = $(this);
            //    CalculateTaxExemptedAmt(currentrow, "N")
            //});
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
//function click_chkplusminus() {/*commented and modify by Hina sharma on 13-01-2025 take reference of service purchase invoice*/
//    CalculateAmount();/*Added by Suraj on 29-03-2024*/
//    var ValDecDigit = $("#ValDigit").text();
//    if ($("#chk_roundoff").is(":checked")) {
//        try {
//            $.ajax(
//                {
//                    type: "POST",
//                    url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
//                    data: {},
//                    success: function (data) {
//                        
//                        if (data == 'ErrorPage') {
//                            PO_ErrorPage();
//                            return false;
//                        }
//                        if (data !== null && data !== "") {
//                            var arr = [];
//                            arr = JSON.parse(data);
//                            if (arr.length > 0) {
//                                if (parseInt(arr[0]["r_acc"]) > 0) {


//                                    var grossval = $("#TxtGrossValue").val();
//                                    var taxval = $("#TxtTaxAmount").val();
//                                    //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
//                                    var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

//                                    var finalnetval = (parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)).toFixed(ValDecDigit);

//                                    var netval = finalnetval;//$("#NetOrderValueInBase").val();
//                                    var fnetval = 0;

//                                    if (parseFloat(netval) > 0) {
//                                        var finnetval = netval.split('.');
//                                        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
//                                            if ($("#p_round").is(":checked")) {
//                                                var decval = '0.' + finnetval[1];
//                                                var faddval = 1 - parseFloat(decval);
//                                                fnetval = parseFloat(netval) + parseFloat(faddval);
//                                                $("#pm_flagval").val($("#p_round").val());

//                                                //let valInBase = $("#TxtGrossValue").val();
//                                                //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
//                                            }
//                                            if ($("#m_round").is(":checked")) {
//                                                //var finnetval = netval.split('.');
//                                                var decval = '0.' + finnetval[1];
//                                                fnetval = parseFloat(netval) - parseFloat(decval);
//                                                $("#pm_flagval").val($("#m_round").val());

//                                                //let valInBase = $("#TxtGrossValue").val();
//                                                //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
//                                            }
//                                            var roundoff_netval = Math.round(fnetval);
//                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
//                                            $("#NetOrderValueInBase").val(f_netval);
//                                            //$("#NetOrderValueSpe").val(f_netval);
//                                            GetAllGLID();
//                                        }
//                                    }
//                                }
//                                else {
//                                    swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                                    $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                                    return false;
//                                }
//                            }
//                            else {
//                                swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                                $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                                return false;
//                            }
//                        }
//                        else {
//                            swal("", $("#RoundoffaccnotdefRoundingoffamtcannotbeperformed").text(), "warning");
//                            $("#chk_roundoff").parent().find(".switchery").trigger("click");
//                            return false;
//                        }
//                    },
//                });
//        } catch (err) {
//            console.log("Purchase invoice round off Error : " + err.message);
//        }

//    }
//    else {
//        $("#div_pchkbox").hide();
//        $("#div_mchkbox").hide();
//        $("#pm_flagval").val("");
//        $("#p_round").prop('checked', false);
//        $("#m_round").prop('checked', false);

//        //$("#ServicePIItemTbl > tbody > tr").each(function () {
//        //    
//        //    var currentrow = $(this);
//        //    CalculateTaxExemptedAmt(currentrow, "N")
//        //});
//        CalculateAmount();
//        GetAllGLID();
//    }
//}
//async function click_chkplusminus() {
//    CalculateAmount("pm_flag");
//    var ValDecDigit = $("#ValDigit").text();
//    if ($("#chk_roundoff").is(":checked")) {
//        await addRoundOffToNetValue();
//    }
//    else {
//        $("#div_pchkbox").hide();
//        $("#div_mchkbox").hide();
//        $("#pm_flagval").val("");
//        $("#p_round").prop('checked', false);
//        $("#m_round").prop('checked', false);

//        //$("#ServicePIItemTbl > tbody > tr").each(function () {
//        //    
//        //    var currentrow = $(this);
//        //    CalculateTaxExemptedAmt(currentrow, "N")
//        //});
//        CalculateAmount();
//        GetAllGLID();
//    }
//}
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
/***----------------------end-------------------------------------***/

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
        $("#CorpAddrDiv").css("display", "none");
        $("#PrintRemarksDiv").css("display", "none");
    }
    else if ($("#OrderTypeImport1").is(":checked")) {
        $('#PrintFormat').val('F3');
        $("#CorpAddrDiv").css("display", "");
        $("#PrintRemarksDiv").css("display", "");
    }
    else {
        $('#PrintFormat').val('F1');
        $("#CorpAddrDiv").css("display", "none");
        $("#PrintRemarksDiv").css("display", "none");
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
function OnCheckedPrintCorpAddr() {
    debugger;
    if ($('#chkPrintCorpAddr').prop('checked')) {
        $('#PrintCorpAddr').val('Y');
    }
    else {
        $('#PrintCorpAddr').val('N');
    }
}
function OnCheckedPrintRemarks() {
    debugger;
    if ($('#chkPrintRemarks').prop('checked')) {
        $('#PrintRemarks').val('Y');
    }
    else {
        $('#PrintRemarks').val('N');
    }
}
/***----------------------end-------------------------------------***/

function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl > tbody >tr").each(function () {
        let row = $(this);
        row.find("#BatchInvoiceQty").val("");
    });
    $("#BatchwiseTotalIssuedQuantity").text("");
}
//function Fn_AfterTaxChangeUpdateNetValueItemWise(currentRow, total_tax_val) {
//    /* Created By : Suraj Maurya, On : 30-11-2024, Purpose : To Update Row Data after tax change */
//    currentRow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(total_tax_val)).toFixed(cmn_ValDecDigit));
//    let ItemGrVal = parseFloat(CheckNullNumber(currentRow.find("#TxtItemGrossValue").val()));
//    let ItemOcVal = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()));
//    let ItemNetVal = ItemGrVal + ItemOcVal + parseFloat(CheckNullNumber(total_tax_val));
//    currentRow.find("#NetValueinBase").val(ItemNetVal.toFixed(cmn_ValDecDigit));
//}

/*----------------------------TDS Section-----------------------------*/
function OnClickTCSCalculationBtn() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    const GrVal = $("#TxtGrossValue").val();
    const ToTcsAmt = parseFloat(CheckNullNumber(GrVal)).toFixed(ValDecDigit);
    // Added by Suraj Maurya on 07-12-2024
    const NetVal = $("#NetOrderValueInBase").val();
    const ToTcsAmt_IT = parseFloat(CheckNullNumber(NetVal)).toFixed(ValDecDigit);
    // Added by Suraj Maurya on 07-12-2024 End
    $("#hdn_tds_on").val("D");
    $("#TDS_AssessableValue").val(ToTcsAmt);
    CMN_OnClickTDSCalculationBtn(ToTcsAmt, null, null, ToTcsAmt_IT, "TCS");

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
//AutoTdsApply : Added by Suraj on 09-07-2024 to Add Auto TDS Apply functionality
async function AutoTdsApply(Entity_Id, GrossAmt) {
    var ValDecDigit = $("#ValDigit").text();
    showLoader();
    try {
        await $.ajax({
            type: "POST",
            url: "/Common/Common/Cmn_GetTdsDetails",
            data: { SuppId: Entity_Id, GrossVal: GrossAmt, tax_type: "TCS" },
            success: function (data) {
                debugger;
                var arr = JSON.parse(data);
                let tds_amt = 0;
                if (arr.Table1 != null) {
                    AddTcsDetails(arr.Table1);
                    CalculateAmount();
                }
                //GetAllGLID("");
                hideLoader();
            }
        })
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }

}

function AddTcsDetails(ArrTcsDtl) {
    var ValDecDigit = $("#ValDigit").text();
    let checkResult = ArrTcsDtl.length > 0 ? ArrTcsDtl[0].result : "";
    let tds_amt = 0;
    if (checkResult == "Invalid Slab") {
        swal("", $("#InvailidTdsSlabFound").text(), "warning")
    } else {
        var checkTdsAcc = "Y";
        $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
        for (var i = 0; i < ArrTcsDtl.length; i++) {
            //let td_tds_amt = Math.round(ArrTcsDtl[i].tds_amt);//commented by suraj maurya on 02-04-2025
            let td_tds_amt = Cmn_RoundValue(ArrTcsDtl[i].tds_amt,"P");
            if (ArrTcsDtl[i].tds_id == "") {
                checkTdsAcc = "N";
            }
            if (checkTdsAcc == "Y") {
                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${i + 1}">
                                <td id="td_TDS_Name">${ArrTcsDtl[i].tds_name}</td>
                                <td id="td_TDS_NameID">${ArrTcsDtl[i].tds_id}</td>
                                <td id="td_TDS_Percentage">${ArrTcsDtl[i].tds_perc}</td>
                                <td id="td_TDS_Level">${ArrTcsDtl[i].tds_level}</td>
                                <td id="td_TDS_ApplyOn">${ArrTcsDtl[i].tds_apply_on}</td>
                                <td id="td_TDS_Amount">${td_tds_amt}</td>
                                <td id="td_TDS_ApplyOnID">${ArrTcsDtl[i].tds_apply_on_id}</td>
                                <td id="td_TDS_BaseAmt">${ArrTcsDtl[i].tds_bs_amt}</td>
                                <td id="td_TDS_AccId">${ArrTcsDtl[i].tds_acc_id}</td>
                                    </tr>`);
                tds_amt += parseFloat(td_tds_amt);
            }
            //let totalNetVal = 0;
            //$('#ScrapSIItemTbl tbody tr').each(function () {
            //    var row = $(this);
            //    let netVal = row.find("#NetValueinBase").val();
            //    totalNetVal = parseFloat(totalNetVal) + parseFloat(CheckNullNumber(netVal));
            //});
            //let finNetVal = parseFloat(totalNetVal) + parseFloat(CheckNullNumber(tds_amt.toFixed(ValDecDigit)));
            
            //$("#NetOrderValueInBase").val(finNetVal);
            //ApplyRoundOff("CallByGetAllGL");
        }
        
        if (checkTdsAcc == "N") {
            $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
            $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
            swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
        }
    }
    $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
}

function FilterItemDetail(e) {//added by Prakash Kumar on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ScrapSIItemTbl", [{ "FieldId": "ItemName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
/*----------------------------TDS Section End-----------------------------*/
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
                if (currentRow.find("#FOC").is(":checked")) {

                }
                else {
                    currentRow.find("#TaxExempted").prop("checked", false);
                    currentRow.find("#TaxExempted").attr("disabled", false);
                    currentRow.find("#ManualGST").attr("disabled", false);
                    currentRow.find("#BtnTxtCalculation").prop("disabled", false);
                }
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
    /*start Add by Hina on 19-05-2025 to chk Financial year exist or not*/
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var DSinvDate = $("#Sinv_dt").val();
    var ErrorFlag = "N";
    $.ajax({
        type: "POST",
        /*  url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: DSinvDate
        },
        async: false,
        success: function (data) {
            /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var status = $("#hfStatus").val();
                if (status == "A") {
                    swal({
                        title: $("#UpdateTransportDetail").text() + "?",
                        //text: $("#deltext").text() + "!",
                        type: "warning",
                        showCancelButton: true,
                        //confirmButtonText: "No",
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
                            //return false;
                        }
                    });
                    ErrorFlag = "Y";
                    return false;
                }
            }
            else {/* to chk Financial year exist or not*/
                /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                ErrorFlag = "Y";
                //return false;
            }
        }
    });

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
    /*End to chk Financial year exist or not*/


    //var status = $("#hfStatus").val();
    //if (status == "A") {
    //    swal({
    //        title: $("#UpdateTransportDetail").text() + "?",
    //        //text: $("#deltext").text() + "!",
    //        type: "warning",
    //        showCancelButton: true,
    //        //confirmButtonText: "No",
    //        confirmButtonClass: "btn-success",
    //        cancelButtonClass: "btn-danger",
    //        confirmButtonText: "Yes",
    //        cancelButtonText: "No",
    //        closeOnConfirm: false
    //    }, function (isConfirm) {
    //        if (isConfirm) {
    //            $("#HdEditCommand").val("UpdateTransPortDetail");
    //            $('form').submit();

    //            return true;
    //        } else {
    //            $("#HdDeleteCommand").val("Edit");
    //            $('form').submit();
    //            return true;
    //            //return false;
    //        }
    //    });
    //    return false;
    //}
}
function FOCDisabledAndEnable(currentrow, Flag) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var QtyDigit = $("#QtyDigit").text();
    if (Flag == "Y") {
        currentrow.find("#TxtRate").val(parseFloat(0).toFixed(QtyDigit))
        currentrow.find("#TxtRate").attr("disabled", true)
        currentrow.find("#DiscountInPerc").val(parseFloat(0).toFixed(QtyDigit));
        currentrow.find("#DiscountInPerc").attr("disabled", true);
        currentrow.find("#DiscountValue").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#TxtItemGrossValue").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#NetValueinBase").val(parseFloat(0).toFixed(ValDigit));
        //currentrow.find("#txt_ScrpItemRemarks").attr("disabled", true);

        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#TaxExempted").prop("disabled", true);
        currentrow.find("#ManualGST").prop("disabled", true);
    }
    else {
        currentrow.find("#TxtRate").attr("disabled", false)
        currentrow.find("#DiscountInPerc").attr("disabled", false);
        //currentrow.find("#txt_ScrpItemRemarks").attr("disabled", false);
        if ($("#nontaxable").is(":checked")) {

        }
        else {
            currentrow.find("#BtnTxtCalculation").prop("disabled", false);
            currentrow.find("#TaxExempted").prop("disabled", false);
            currentrow.find("#ManualGST").prop("disabled", false);
        }
    }
}
async function OnClickFOCCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    if (currentrow.find("#FOC").is(":checked")) {
        FOCDisabledAndEnable(currentrow, "Y");
    }
    else {
        FOCDisabledAndEnable(currentrow, "N");
    }
    OnClickTaxExemptedCheckBox(e);
    CalculateAmount();
    OnChangeGrossAmt();
    let cust_id = $("#CustomerName").val();
    let GrossValue = $("#TxtGrossValue").val();
    await AutoTdsApply(cust_id, GrossValue);
    //OnClickSaveAndExit_OC_Btn();
}

function ValidationDispatchDetail() {
    debugger;
    var ErrorLog = "N";
    var Address = $("#Dispatch_address1").val();
    var Country = $("#dispatchCountry").val();
    var State = $("#dispatch_State").val();
    var District = $("#dispatch_District").val();
    var City = $("#dispatch_City").val();
    var Pin = $("#dispatch_Pin").val();
    if (Address == "" || Address == null || Address == 0) {
        $("#Dispatch_address1").css('border-color', 'red');
        $("#vmAddress1").text($("#valueReq").text());
        $("#vmAddress1").css("display", "inherit");
        ErrorLog = "Y";
    }
    else {
        $("#Dispatch_address1").css('border-color', "#ced4da");
        $("#vmAddress1").text("");
        $("#vmAddress1").css('display', "none");
    }

    if (Country == "" || Country == null || Country == 0) {
        document.getElementById("vmdispatchCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-dispatchCountry-container']").css('border-color', 'red');
        $("#vmdispatchCountryname").attr("style", "display: inherit;");
        ErrorLog = "Y";
    }
    else {
        document.getElementById("vmdispatchCountryname").innerHTML = "";
        $("[aria-labelledby='select2-dispatchCountry-container']").css('border-color', '#ced4da');
        $("#vmdispatchCountryname").attr("style", "display: none;");
    }
    if (State == "" || State == null || State == 0) {
        document.getElementById("vmdispatchStatename").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-dispatch_State-container']").css('border-color', 'red');
        $("#vmdispatchStatename").attr("style", "display: inherit;");
        ErrorLog = "Y";
    }
    else {
        document.getElementById("vmdispatchStatename").innerHTML = "";
        $("[aria-labelledby='select2-dispatch_State-container']").css('border-color', '#ced4da');
        $("#vmdispatchStatename").attr("style", "display: none;");
    }
    if (District == "" || District == null || District == 0) {
        document.getElementById("vmdispatchDistname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-dispatch_District-container']").css('border-color', 'red');
        $("#vmdispatchDistname").attr("style", "display: inherit;");
        ErrorLog = "Y";
    }
    else {
        document.getElementById("vmdispatchDistname").innerHTML = "";
        $("[aria-labelledby='select2-dispatch_District-container']").css('border-color', '#ced4da');
        $("#vmdispatchDistname").attr("style", "display: none;");
    }
    if (City == "" || City == null || City == 0) {
        document.getElementById("vmdispatchCityname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-dispatch_City-container']").css('border-color', 'red');
        $("#vmdispatchCityname").attr("style", "display: inherit;");
        ErrorLog = "Y";
    }
    else {
        document.getElementById("vmdispatchCityname").innerHTML = "";
        $("[aria-labelledby='select2-dispatch_City-container']").css('border-color', '#ced4da');
        $("#vmdispatchCityname").attr("style", "display: none;");
    }
    if (Pin == "" || Pin == null || Pin == 0) {
        document.getElementById("vmdispatchPin").innerHTML = $("#valueReq").text();
        $("#dispatch_Pin").css('border-color', 'red');
        $("#vmdispatchPin").attr("style", "display: inherit;");
        ErrorLog = "Y";
    }
    else {
        document.getElementById("vmdispatchPin").innerHTML = "";
        $("#dispatch_Pin").css('border-color', '#ced4da');
        $("#vmdispatchPin").attr("style", "display: none;");
    }
    if (ErrorLog == "Y") {
        return false
    }
    else {
        return true
    }
}
function onchangeCustAddr() {
    debugger;
    var customer_address = $("#Dispatch_address1").val();
    document.getElementById("vmAddress1").innerHTML = "";
    $("#Dispatch_address1").css('border-color', '#ced4da');
    $("#vmAddress1").attr("style", "display: none;");


}
function onchangeCustPin() {
    var Custtyp;


    var customer_Pin = $("#dispatch_Pin").val();
    if (customer_Pin == "") {
        document.getElementById("vmdispatchPin").innerHTML = $("#valueReq").text();
        $("#dispatch_Pin").css('border-color', 'red');
        $("#vmdispatchPin").attr("style", "display: block;");

    }
    else {

        document.getElementById("vmdispatchPin").innerHTML = "";
        $("#dispatch_Pin").css('border-color', '#ced4da');
        $("#vmdispatchPin").attr("style", "display: none;");
    }


}
function OnChangeState() {
    debugger;
    $('#dispatch_City').empty();
    $('#dispatch_City').append(`<option value="0">---Select---</option>`);
    $('#HdndispatchCity').val("");
    $('#dispatch_Pin').val("");
    GetDistrictByState();
    $('#vmdispatchStatename').text("");
    $("#vmdispatchStatename").css("display", "none");
    $("[aria-labelledby='select2-dispatch_State-container']").css('border-color', "#ced4da");
    $("#dispatch_State").css('border-color', "#ced4da");

}
function GetDistrictByState() {
    debugger;

    var ddlStateID = $("#dispatch_State").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ScrapSaleInvoice/GetDistrictOnState",

        dataType: "json",
        //async: true,
        data: { ddlStateID: ddlStateID, },
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            debugger;
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.length > 0) {
                    $('#dispatch_District').empty();

                    for (var i = 0; i < arr.length; i++) {
                        $('#dispatch_District').append(`<option value="${arr[i].district_id}">${arr[i].district_name}</option>`);
                    }
                    //$("#Cust_District").select2();

                    //var hdnDistrct = $("#HdnCustDistrict").val()
                    // $("#dispatch_District").val("0").trigger("change");
                    //$("#HdnCustDistrict").val("0");


                }
            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeDistrict() {
    debugger;
    GetCityByDistrict();
    $('#dispatch_Pin').val("");
    $('#vmdispatchDistname').text("");
    $("#vmdispatchDistname").css("display", "none");
    $("[aria-labelledby='select2-dispatch_District-container']").css('border-color', "#ced4da");

}
function GetCityByDistrict() {
    debugger;

    var ddlDistrictID = $("#dispatch_District").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ScrapSaleInvoice/GetCityOnDistrict",

        dataType: "json",
        //async: true,
        data: { ddlDistrictID: ddlDistrictID },
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }

            debugger;
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.length > 0) {
                    $('#dispatch_City').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#dispatch_City').append(`<option value="${arr[i].city_id}">${arr[i].city_name}</option>`);
                    }
                    //$("#Cust_City").select2();
                    //var HdnCustCity = $("#HdnCustCity").val();
                    //$("#Cust_City").val(HdnCustCity).trigger("change");
                    //$("#HdnCustCity").val('0');

                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeCity() {
    debugger;
    $('#dispatch_Pin').val("");
    $('#vmdispatchCityname').text("");
    $("#vmdispatchCityname").css("display", "None");
    $("[aria-labelledby='select2-dispatch_City-container']").css('border-color', "#ced4da");
}
$("#dispatch_Pin").on("keypress", function (event) {
    return OnkeyPressConNumber(event);
});
function OnkeyPressConNumber(event) {
    debugger;
    var charCode = event.which || event.keyCode;
    var char = String.fromCharCode(charCode);

    // Allow only numeric values (0-9)
    if (!/\d/.test(char)) {
        event.preventDefault(); // Block non-numeric input
    }
};
//Added by Nidhi on 27-08-2025
function SendEmail() {
    var docid = $("#DocumentMenuId").val();
    var cust_id = $("#CustomerName").val();
    Cmn_SendEmail(docid, cust_id, 'Cust');
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    var filepath = $('#hdfilepathpdf').val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var statusAM = '';
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/ScrapSaleInvoice/SendEmailAlert", filepath, "", GstApplicable)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    var filepath = $('#hdfilepathpdf').val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'DSI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ScrapSaleInvoice/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray), GstApplicable: GstApplicable },
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
    }
}
function EmailAlertLogDetails() {
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
function PrintFormate() {
    debugger;
    var status = $('#hfStatus').val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Sinv_dt").val();
    var docid = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'DSI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ScrapSaleInvoice/SavePdfDocToSendOnEmailAlert_Ext",
            data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray), GstApplicable: GstApplicable },
            /*dataType: "json",*/
            success: function (data) {
                var filepath = data;
                $('#hdfilepathpdf').val(filepath)
                $("#btn_mail_print").css("display", "none");
                $("#btn_print").css("display", "");
            }
        });
    }
}

function GetPrintFormatArray() {
    return [{
        PrintFormat: $('#PrintFormat').val(),
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        CustomerAliasName: $("#CustomerAliasName").val(),
        NumberOfCopy: "1",
        ShowWithoutSybbol: $("#ShowWithoutSybbol").val(),
        showDeclare1: $("#showDeclare1").val(),
        showDeclare2: $("#showDeclare2").val(),
        showInvHeading: $("#showInvHeading").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        ShowTotalQty: $("#ShowTotalQty").val(),
        PrintShipFromAddress: $("#PrintShipFromAddress").val(),
    }];
}
//End on 26-08-2025
