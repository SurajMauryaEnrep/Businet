/************************************************
Javascript Name:Local Sale Quotation Detail
Created By:Prem 
Created Date: 23-02-2021
Description: This Javascript use for the Local Sale Order Detail many function
Modified By:
Modified Date:
Description:
*************************************************/

$(document).ready(function () {
    $("#sales_person").select2();

    var PageName = sessionStorage.getItem("MenuName");
    $('#LSODetailsPageName').text(PageName);

    $('select[id="SQItemListName"]').bind('change', function (e) {
        //debugger;
        BindQTItemList(e);
    });
    $('input[name="QuotQty"]').bind('change', function (e) {
        //debugger;
        CalculationBaseQty(e);
    });
    $('input[name="item_rate"]').bind('change', function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        CalculationBaseRate(clickedrow);
    });
    $('input[name="item_disc_perc"]').bind('change', function (e) {
        //debugger;
        CalculateDisPercent(e, "", "");
    });
    $('input[name="item_disc_amt"]').bind('change', function (e) {
        //debugger;
        CalculateDisAmt(e, "", "");
    });

    BindCustomerList();
    //BindSalesPersonList();
    BindDLLQTItemList();

    var CustPros_type;
   
    CustPros_type = "C";
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
       $("#div_SQRaiseOrder").css("display", "");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
        $("#div_SQRaiseOrder").css("display", "none");
    }

    $('#QTItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();

        debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#hfItemID").val();
        $("#hdn_Sub_ItemDetailTbl TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var rowitem = row.find("#ItemId").val();
            if (rowitem == ItemCode) {
                $(this).remove();
            }
        });
        //ShowItemListItm(ItemCode);
        /*CalculateAmount();*//*COMMENT AND WRITE BELOW BY HINA ON 22-05-2025*/
        debugger;
        var TOCAmount = parseFloat($("#QT_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmount = 0;
        }
        //var len = $("#QTItmDetailsTbl >tbody >tr").length;
        //if (len == "0")
        //{
        //    $("#QT_OtherCharges").val()
        //}
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetQT_ItemTaxDetail();
        CalculateAmount();
        CalculateTotalLandedCost();
        debugger;
        BindOtherChargeDeatils();
        if ($('#QTItmDetailsTbl >tbody >tr').length == "0") {/*ADD BY HINA ON 22-05-2025*/
            debugger;
           
            $("#Tbl_OC_Deatils tbody tr").remove();
            $('#ht_Tbl_OC_Deatils tbody tr').remove();
        }

    });

    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        //debugger;

        var clickedrow = $(e.target).closest("tr");
        if ((clickedrow.find('#OrderTypeLocal').prop("disabled")) == false) {
            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

            clickedrow.find('#OrderTypeLocal').prop("checked", true);
            var Address_id = clickedrow.find("#addhd").text();
            var Address = clickedrow.find("#cust_add").text();
            var pin = clickedrow.find("#cust_pin").text();
            var city = clickedrow.find("#city_name").text();
            var dist = clickedrow.find("#district_name").text();
            var state = clickedrow.find("#state_name").text();
            var country = clickedrow.find("#country_name").text();

            $('#hd_TxtBillingAddr').val(Address + ',' + ' ' + city + '-' + ' ' + pin + ',' + ' ' + dist + ',' + ' ' + state + ',' + ' ' + country);
            $("#hd_bill_add_id").val(Address_id);
        }
    });
    jQuery(document).trigger('jquery.loaded');
    var QT_No = $("#qt_no").val();
    $("#hdDoc_No").val(QT_No);


    DeleteTermCondition();
    //hideLoader();
    CalculateTotalLandedCost();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103120") {
        if ($("#rpt_id").text() != "0") {
            $("#sales_person").attr('disabled', true);
        }
    }
});

function AddNewProspect() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    try {
        var ProspectFromQuot = "Y";
        window.location.href = "/BusinessLayer/ProspectSetup/AddProspectSetupDetail/?ProspectFromQuot=" + ProspectFromQuot + "&QuotationDocumentMenuId=" + DocumentMenuId;

    }
    catch (err) {
        console.log(PFName + " Error : " + err.message);
    }

}
function BindCustomerList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    var CustPros_type;
    var Cust_type;
    var status = $("#hfStatus").val().trim();
    CustPros_type = "C";
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
        $("#Div_ProspectSetup").css("display", "none");
        $("#div_SQRaiseOrder").css("display", "");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
        $("#div_SQRaiseOrder").css("display", "none");
       /* $("#Prosbtn").css("display", "block");*/
        if (status == "D" || status == "F" || status == "A" || status == "C") {
            $("#Div_ProspectSetup").css("display", "none");
        }
        else {
            CheckUserRolePageAccess(CustPros_type, "SQ");
        }
    }
    if ($("#OrderTypeD").is(":checked")) {
        Cust_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Cust_type = "E";
    }
    $("#hdn_CustPros_type").val(CustPros_type);

    //$('#Customer_Name').empty().append('<option value="0" selected="selected">---Select---</option>');
    //$("#TxtBillingAddr").val("");
    //$("#TxtShippingAddr").val("");
    //$("#txtCurrency").val("");
    //$("#conv_rate").val("");
    //$('#sales_person').empty().append('<option value="0" selected="selected">---Select---</option>');
    //$("#dremarks").val("");

    $("#Customer_Name").select2({

        ajax: {
            url: $("#hdnCustNameList").val(),
            data: function (params) {
                debugger;
                var queryParameters = {
                    SQ_CustName: params.term, // search term like "a" then "an"
                    //CustPage: params.page,
                    //BrchID: Branch,
                    CustPros_type: CustPros_type,
                    Cust_type: Cust_type
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
//function BindSalesPersonList() {
//    debugger;
//    var Branch = sessionStorage.getItem("BranchID");
//    try {
//        $("#sales_person").select2({
//            ajax: {
//                url: $("#salespersonList").val(),
//                data: function (params) {
//                    var queryParameters = {
//                        SQ_SalePerson: params.term, // search term like "a" then "an"
//                        BrchID: Branch
//                    };
//                    return queryParameters;
//                },
//                dataType: "json",
//                cache: true,
//                delay: 250,
//                contentType: "application/json; charset=utf-8",
//                processResults: function (data, params) {
//                    if (data == 'ErrorPage') {
//                        LSO_ErrorPage();
//                        return false;
//                    }
//                    params.page = params.page || 1;
//                    return {
//                        //results:data.results,
//                        results: $.map(data, function (val, item) {
//                            return { id: val.ID, text: val.Name };
//                        })
//                    };
//                }
//            },
//        });
//    }
//    catch {
//        hideLoader();
//    }
//}
function QTList() {
    try {
        location.href = "/ApplicationLayer/DomesticSalesQuotationList/DomesticSalesQuotationList";

    } catch (err) {
        console.log(PFName + " Error : " + err.message);
    }
}
function ClearRowDetails(e, ItemID) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    clickedrow.find("#UOM").val("");
    clickedrow.find("#QuotQty").val("");
    clickedrow.find("#OrderQuantity").val("");
    clickedrow.find("#item_rate").val("");
    clickedrow.find("#item_disc_perc").val("");
    clickedrow.find("#item_disc_perc").attr("readonly", false);
    clickedrow.find("#item_disc_amt").val("");
    clickedrow.find("#item_disc_amt").attr("readonly", false);
    clickedrow.find("#item_disc_val").val("");
    clickedrow.find("#item_gr_val").val("");
    clickedrow.find("#item_ass_val").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#item_oc_amt").val("");
    clickedrow.find("#item_net_val_spec").val("");
    clickedrow.find("#item_net_val_bs").val("");
    clickedrow.find("#remarksid").val("");

    if (ItemID != "" && ItemID != null) {
        // ShowItemListItm(ItemID);
    }
    CalculateAmount();
    CalculateTotalLandedCost();
    var TOCAmount = parseFloat($("#QT_OtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmount = 0;
    }
    debugger
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetQT_ItemTaxDetail();

}
function OnChangeCustProsType() {
    debugger;
    DisableItemDetail();

    $('#Customer_Name').empty().append('<option value="0" selected="selected">---Select---</option>');
    $('#SpanCustNameErrorMsg').text("");
    $("#TxtBillingAddr").val("");
    $("#TxtShippingAddr").val("");
    $("#txtCurrency").val("");
    $("#conv_rate").val("");
   // $('#sales_person').empty().append('<option value="0" selected="selected">---Select---</option>');
    $("#dremarks").val("");
}

//function ApplyGST(gst_number) {
//    debugger;
//    Cmn_OnSaveAddressApplyGST(gst_number, "QTItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val") 
//    //var ItmCodes = "";
//    //$("#QTItmDetailsTbl >tbody >tr").each(function () {
//    //    var clickedrow = $(this);
//    //    var Itm_ID = clickedrow.find("#hfItemID").val();
//    //    ItmCodes += ItmCodes == "" ? Itm_ID : "," + Itm_ID;
//    //    //Cmn_BindUOM(clickedrow, Itm_ID, "", "", "sale");
//    //});

//    //if (ItmCodes != "") {

//    //    try {
//    //        $.ajax(
//    //            {
//    //                type: "POST",
//    //                url: "/Common/Common/GetItemGstDetails",
//    //                data: { ItemIDs: ItmCodes, gst_number: gst_number },
//    //                success: function (data) {
//    //                    debugger;
//    //                    if (data == 'ErrorPage') {
//    //                        LSO_ErrorPage();
//    //                        return false;
//    //                    }
//    //                    var arr = [];
//    //                    arr = JSON.parse(data);

//    //                    if (arr.Table.length > 0) {
//    //                        $("#QTItmDetailsTbl >tbody >tr").each(function () {
//    //                            var clickedrow = $(this);
//    //                            var Itm_ID = clickedrow.find("#hfItemID").val();
//    //                            var SNo = clickedrow.find("#SNohiddenfiled").val();
//    //                            var assessVal = clickedrow.find("#item_ass_val").val();
//    //                            for (var i = 0; i < arr.Table.length; i++) {
//    //                                if (Itm_ID == arr.Table[i].item_id) {

//    //                                    $('#TaxCalculatorTbl tbody tr').remove();
//    //                                    $("#TaxCalcItemCode").val(arr.Table[i].item_id);
//    //                                    $("#HiddenRowSNo").val(SNo);

//    //                                    $("#Tax_AssessableValue").val(assessVal);
//    //                                    var ship_add_gstNo = $("#ship_add_gstNo").val();
//    //                                    var Br_Gst_number = arr.Table[i].br_gst_no;
//    //                                    if (ship_add_gstNo.substring(0, 2) == Br_Gst_number.substring(0, 2)) {
//    //                                        var ctax_val = assessVal * arr.Table[i].cgst_tax_per / 100;
//    //                                        var stax_val = assessVal * arr.Table[i].sgst_tax_per / 100;
//    //                                        TaxTableDataBind(1,
//    //                                            arr.Table[i].cgst_tax_name, arr.Table[i].cgst_tax_id, String(arr.Table[i].cgst_tax_per), 1,
//    //                                            "Immediate Level", ctax_val, "I", (parseFloat(ctax_val) + parseFloat(stax_val)), "", ""
//    //                                        )

//    //                                        TaxTableDataBind(2,
//    //                                            arr.Table[i].sgst_tax_name, arr.Table[i].sgst_tax_id, String(arr.Table[i].sgst_tax_per), 1,
//    //                                            "Immediate Level", stax_val, "I", (parseFloat(ctax_val) + parseFloat(stax_val)), "", ""
//    //                                        )

//    //                                    } else {
//    //                                        var itax_val = assessVal * arr.Table[i].igst_tax_per / 100;
//    //                                        TaxTableDataBind(1,
//    //                                            arr.Table[i].igst_tax_name, arr.Table[i].igst_tax_id, String(arr.Table[i].igst_tax_per), 1,
//    //                                            "Immediate Level", itax_val, "I", itax_val, "", ""
//    //                                        )
//    //                                    }
//    //                                    OnClickSaveAndExit();
//    //                                }
//    //                            }

//    //                        });

//    //                    }



//    //                },
//    //            });
//    //    } catch (err) {
//    //        console.log("GetMenuData Error : " + err.message);
//    //    }
//    //}

//}

//------------------Tax Amount Calculation------------------//
function OnClickTaxCalculationBtn(e) {
    debugger;
    var SQItemListName = "#SQItemListName";
    var SNohiddenfiled = "#SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SQItemListName)
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").prop("disabled", true);
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").prop("disabled", false);
            }
            else {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").prop("disabled", true);
            }
        }
    }
    //CMNOnClickTaxCalculationBtn(e, SNohiddenfiled, SQItemListName)

}
function OnClickSaveAndExit() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    //var SrcDocNo = $("#TaxCalcGRNNo").val();
    //var SrcDocDate = $("#TaxCalcGRNDate").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var conv_rate = $("#conv_rate").val();

    debugger;
    let NewArr = [];
    /*start Add by Hina on 22-05-2025 for tax on oc */
    var TaxOn = $("#HdnTaxOn").val();
    //var TaxOn = "";
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    /*End Add by Hina on 22-05-2025 for tax on oc */
    /*var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;*/
    /*commented and modify by Hina on 22-05-2025 for tax on oc */
    var FTaxDetails = $("#" + HdnTaxCalculateTable + " > tbody > tr").length;

    if (FTaxDetails > 0) {

        /* $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {*/
        /*commented and modify by Hina on 22-05-2025 for tax on oc */
        $("#" + HdnTaxCalculateTable + " > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

            if (/*TaxUserID == UserID &&*/ TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
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
            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
            /*  $("#Hdn_TaxCalculatorTbl > tbody").append(`*/
            /*commented and modify by Hina on 22-05-2025 for tax on oc */
            $("#" + HdnTaxCalculateTable + " > tbody").append(`

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
                                    </tr>`)
            NewArr.push({ /*UserID: UserID, RowSNo: RowSNo, */TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        /*BindTaxAmountDeatils(NewArr);*/
        /*commented and modify by Hina on 22-05-2025 for tax on oc */
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(NewArr);
        }
    }
    else {
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
            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
            /* $("#Hdn_TaxCalculatorTbl > tbody").append(`*/
            /*commented and modify by Hina on 22-05-2025 for tax on oc */
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
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
                                </tr>
`)
            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        /*BindTaxAmountDeatils(TaxCalculationList);*/
        /*commented and modify by Hina on 22-05-2025 for tax on oc */
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationList);
        }
    }
    /*Add Start by Hina on 22-05-2025 for tax on oc */
    if (TaxOn == "OC") {

        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    }
    else {
        /*End Start by Hina on 22-05-2025 for tax on oc */
        $("#QTItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                }
                var gr = CheckNullNumber(currentRow.find("#item_gr_val").val());
                if (gr == 0) {
                    TaxAmt = 0;
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                }
                else {
                    if (currentRow.find("#ItemHsnCode").val() == 0) {
                        currentRow.find("#item_tax_amt").val(parseFloat(0).toFixed(DecDigit));
                    }
                    else {
                        currentRow.find("#item_tax_amt").val(parseFloat(TaxAmt).toFixed(DecDigit));
                    }

                }
                //currentRow.find("#item_tax_amt").val(TaxAmt);
                OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                }
                debugger;
                var GrVal = currentRow.find("#item_gr_val").val();
                //AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                AssessableValue = (parseFloat(CheckNullNumber(GrVal))).toFixed(DecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
            }
            var TaxAmt1 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt1 = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt1 != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
}
//------------------Tax For OC Calculation add by Hina on 22-05-2025------------------//
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//
function OnClickReplicateOnAllItems() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmRowSNo = RowSNo;
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var conv_rate = $("#conv_rate").val();
    var TaxOn = $("#HdnTaxOn").val();/*Add by Hina on 22-05-2025 for tax on oc*/
    debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //var QTNo = $("#QuotationNumber").val();
        //var QTDate = $("#QuotationDate").val();
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });

    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            /*start Add by Hina on 22-05-2025 for tax on oc*/
            var SnoForOc = 0;
            var TaxApplicableTable = "QTItmDetailsTbl";
            if (TaxOn == "OC") {
                TaxApplicableTable = "Tbl_OC_Deatils";
            }
            /*end Add by Hina on 22-05-2025 for tax on oc*/
            /* $("#QTItmDetailsTbl >tbody >tr").each(function () {*/ /*commented and modify by Hina on 22-05-2025 for tax on oc*/
            $("#" + TaxApplicableTable + " >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemCode;
                var AssessVal;
                debugger;
                /*start Add by Hina on 22-05-2025 for tax on oc*/
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OcAmtBs").text();
                    SnoForOc++;
                    Sno = SnoForOc;
                } else {
                    ItemCode = currentRow.find("#SQItemListName" + Sno).val();
                    AssessVal = currentRow.find("#item_ass_val").val();
                }
                //ItemCode = currentRow.find("#SQItemListName" + Sno).val();/*commented and modify above by Hina on 22-05-2025 for tax on oc*/
                //AssessVal = currentRow.find("#item_ass_val").val();
                /*end Add by Hina on 22-05-2025 for tax on oc*/

                if (AssessVal != "") {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        var RowSNo = TaxCalculationList[i].RowSNo;
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
                                    var TaxItmCodeA = NewArray[j].TaxItmCode;
                                    if (TaxLevelTbl == Level && TaxItmCodeA == ItemCode) {
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
                                    var TaxItmCodeA = NewArray[j].TaxItmCode;
                                    if (TaxLevel != Level && TaxItmCodeA == ItemCode && Level < TaxLevel) {
                                        TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                    }
                                }
                                var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                        }
                        NewArray.push({ /*UserID: UserID, RowSNo: Sno,*/ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                    }
                    if (NewArray != null) {
                        if (NewArray.length > 0) {
                            for (k = 0; k < NewArray.length; k++) {
                                var TaxName = NewArray[k].TaxName;
                                var TaxNameID = NewArray[k].TaxNameID;
                                var TaxItmCode = NewArray[k].TaxItmCode;
                                var RowSNo = NewArray[k].RowSNo;
                                var TaxPercentage = NewArray[k].TaxPercentage;
                                var TaxLevel = NewArray[k].TaxLevel;
                                var TaxApplyOn = NewArray[k].TaxApplyOn;
                                var TaxAmount = NewArray[k].TaxAmount;
                                var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                                if (CitmTaxItmCode != TaxItmCode && CitmRowSNo != RowSNo) {
                                    //if (CitmRowSNo != RowSNo) {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }

                }
            });
        }

    }
    /*start Add by Hina on 22-05-2025 for tax on oc*/
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    $("#" + HdnTaxCalculateTable + " > tbody >tr").remove();
    /*end Add by Hina on 22-05-2025 for tax on oc*/
    /*$("#Hdn_TaxCalculatorTbl > tbody >tr").remove();*//*commented and modify above by Hina on 22-05-2025 for tax on oc*/


    if (TaxCalculationListFinalList.length > 0) {
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
            /* $("#Hdn_TaxCalculatorTbl > tbody").append(`*//*commented and modify below by Hina on 22-05-2025 for tax on oc*/
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                 <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxCalculationListFinalList[j].TaxItmCode}</td>
                                    <td id="TaxName">${TaxCalculationListFinalList[j].TaxName}</td>
                                    <td id="TaxNameID">${TaxCalculationListFinalList[j].TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxCalculationListFinalList[j].TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxCalculationListFinalList[j].TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxCalculationListFinalList[j].TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxCalculationListFinalList[j].TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TaxCalculationListFinalList[j].TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxCalculationListFinalList[j].TaxApplyOnID}</td>
                                   </tr>`)
        }
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationListFinalList);
        }
        /*BindTaxAmountDeatils(TaxCalculationListFinalList);*//*commented and modify above by Hina on 22-05-2025 for tax on oc*/
    } else {
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationListFinalList);
        }
        /*BindTaxAmountDeatils(TaxCalculationListFinalList);*//*commented and modify above by Hina on 22-05-2025 for tax on oc*/

    }
    /*Code start by Hina on 22-05-2025 for tax on oc*/
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
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
    }
    else {/*Code end by Hina on 22-05-2025 for tax on oc*/
        $("#QTItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var ItemId = currentRow.find("#SQItemListName" + Sno).val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    var fill = false;/*add by Hina on 22-05-2025 for tax on oc*/
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var txtTaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemId == txtTaxItmCode && fill == false) {/*add && fill == false by Hina on 22-05-2025 for tax on oc*/

                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            /*var OC_Amt = (parseFloat(0)).toFixed(DecDigit);*//*commented and modify below by Hina on 22-05-2025 for tax on oc*/
                            //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                            //}
                            //AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                            var OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);
                            AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(DecDigit);

                            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                            currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                            fill = true;/*add by Hina on 22-05-2025 for tax on oc*/
                        }
                    }
                }
                else {
                    /*code start add by Hina on 22-05-2025 for tax on oc*/
                    /*var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);*/
                   
                    var GrossAmtOR = parseFloat(0).toFixed(DecDigit);
                    if (currentRow.find("#item_gr_val").val() != "") {
                        GrossAmtOR = parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                    }
                    /*code end add by Hina on 22-05-2025 for tax on oc*/
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        OC_Amt_OR = parseFloat(CheckNullNumber(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);
                    }
                    //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {/*commented and modify above by Hina on 22-05-2025 for tax on oc*/
                    //    OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                    //}
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                    /*currentRow.find("#item_ass_val").val(GrossAmtOR);*//*commented by Hina on 22-05-2025 for tax on oc*/
                    currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                    FinalFGrossAmtOR = (FGrossAmtOR * conv_rate).toFixed(DecDigit);
                    currentRow.find("#item_net_val_bs").val(FinalFGrossAmtOR);

                }
            }
        });
        CalculateAmount();
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    var UserID = $("#UserID").text();
    var conv_rate = $("#conv_rate").val();
    /*start add by Hina on 22-05-2025 for tax on oc*/
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    /*end add by Hina on 22-05-2025 for tax on oc*/
    //var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    var FTaxDetailsItemWise = $("#" + HdnTaxCalculateTable + " > tbody > tr").length;/*commented and modify by Hina on 22-05-2025 for tax on oc*/

    if (FTaxDetailsItemWise > 0) {
        var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
        var NewArray = [];

        /* $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {*//*commented and modify by Hina on 22-05-2025 for tax on oc*/
        $("#" + HdnTaxCalculateTable + " > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();  /*Commented by Suraj On 14-12-2022*/
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            //debugger;
            var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);

            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {/*Commented by Suraj On 14-12-2022*/

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
                            var TaxItemIDA = NewArray[j].TaxItmCode;
                            if (TaxLevelTbl == Level && TaxItemIDA == ItmCode) {
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
                            var TaxItemIDA = NewArray[j].TaxItmCode;
                            if (TaxLevel != Level && TaxItemIDA == ItmCode && Level < TaxLevel) {
                                TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                            }
                        }
                        var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                        TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                        TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                    }
                }
                //debugger;
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

            NewArray.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

        });
        /*  $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {*//*commented and modify by Hina on 22-05-2025 for tax on oc*/
        $("#" + HdnTaxCalculateTable + " > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (/*TaxUserID == UserID &&*/ TaxItemID == ItmCode) {
                currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
            }
        });

        $("#QTItmDetailsTbl >tbody >tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var ItemNo;
            ItemNo = currentRow.find("#SQItemListName" + Sno).val();

            if (Sno == RowSNo && ItemNo == ItmCode) {

                /*var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();*//*commented and modify by Hina on 22-05-2025 for tax on oc*/
                var FTaxDetailsItemWise = $("#" + HdnTaxCalculateTable + " > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                if (FTaxDetailsItemWise.length > 0) {
                    FTaxDetailsItemWise.each(function () {
                        var CRow = $(this);
                        var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                        if (currentRow.find("#TaxExempted").is(":checked")) {
                            TotalTaxAmtF = (parseFloat(0)).toFixed(DecDigit);
                        }
                        var TaxItmCode = CRow.find("#TaxItmCode").text();
                        var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                        var TaxAmt = parseFloat(0).toFixed(DecDigit)
                        var GstApplicable = $("#Hdn_GstApplicable").text();
                        if (GstApplicable == "Y") {
                            if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                            }
                        }
                        if (TaxItmCode == ItmCode) {
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                            }
                            debugger;
                            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                            currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);


                        }
                    });


                }
            }
        });
        CalculateAmount();
        BindTaxAmountDeatils(NewArray);
    }


}
function BindTaxAmountDeatils(TaxAmtDetail) {
    debugger;
    var QT_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var QT_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";
    //CMNBindTaxAmountDeatils(TaxAmtDetail, QT_ItemTaxAmountList, QT_ItemTaxAmountTotal)
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, QT_ItemTaxAmountList, QT_ItemTaxAmountTotal)
}
function AfterDeleteResetQT_ItemTaxDetail() {
    debugger;
    var QTItmDetailsTbl = "#QTItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var SQItemListName = "#SQItemListName";
    //CMNAfterDeleteReset_ItemTaxDetail(QTItmDetailsTbl, SNohiddenfiled, SQItemListName)
    CMNAfterDeleteReset_ItemTaxDetailModel(QTItmDetailsTbl, SNohiddenfiled, SQItemListName)
}

function OnClickSaveAndExit_OC_Btn() {
    debugger;

    var NetOrderValueSpe = "#NetOrderValueSpe";
    var NetOrderValueInBase = "#NetOrderValueInBase";
    var QT_OtherCharges = '#QT_OtherCharges';
    CMNOnClickSaveAndExit_OC_Btn(QT_OtherCharges, NetOrderValueSpe, NetOrderValueInBase)
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var conv_rate = $("#conv_rate").val();

    var TotalGAmt = parseFloat(0).toFixed(DecDigit);//$("#TxtGrossValue").val();
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var GValue = currentRow.find("#item_gr_val").val();
        if (GValue != null && GValue != "") {
            if (parseFloat(GValue) > 0) {
                TotalGAmt = parseFloat(TotalGAmt) + parseFloat(GValue);
            }
            else {
                TotalGAmt = parseFloat(TotalGAmt) + parseFloat(0);
            }
        }
    });
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var GrossValue;

        //var GrossValue = currentRow.find("#item_gr_val").val();
        //if (GrossValue == "" || GrossValue == null) {
        //    GrossValue = "0";

        //}
        var ItmGrVal = currentRow.find("#item_gr_val").val();
        if (CheckNullNumber(ItmGrVal) == 0) {
            GrossValue = parseFloat(0).toFixed();
        }
        else {
            GrossValue = currentRow.find("#item_gr_val").val();
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#item_oc_amt").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
            }
        }
        else {
            currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(DecDigit));
        }

    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var OCValue = currentRow.find("#item_oc_amt").val();
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
        $("#QTItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#QTItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var SOItm_GrossValue = currentRow.find("#item_gr_val").val();
        var SOItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var SOItm_OCAmt = currentRow.find("#item_oc_amt").val();
        if (SOItm_OCAmt == null || SOItm_OCAmt == "") {
            SOItm_OCAmt = "0";
        }
        if (SOItm_GrossValue == "" || SOItm_GrossValue == null) {
            SOItm_GrossValue = "0";
            //SOItm_GrossValue = parseFloat(0).toFixed(DecDigit);
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            SOItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(DecDigit);
        }
        var SOItm_NetOrderValueSpec = (parseFloat(SOItm_GrossValue) + parseFloat(SOItm_TaxAmt) + parseFloat(SOItm_OCAmt));
        var SOItm_NetOrderValueBase = (parseFloat(SOItm_GrossValue) + parseFloat(SOItm_TaxAmt) + parseFloat(SOItm_OCAmt));
        currentRow.find("#item_net_val_spec").val((parseFloat(SOItm_NetOrderValueSpec)).toFixed(DecDigit));
        FinalNetOrderValueBase = (SOItm_NetOrderValueBase * conv_rate).toFixed(DecDigit);
        currentRow.find("#item_net_val_bs").val((parseFloat(FinalNetOrderValueBase)).toFixed(DecDigit));

    });
    CalculateAmount();
};
function SetOtherChargeVal() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#item_oc_amt").val((parseFloat(0)).toFixed(DecDigit));
    });
}
function BindOtherChargeDeatils() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    if ($("#QTItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
        //$("#QT_OtherCharges").val(parseFloat(0).toFixed(DecDigit));
        $("#_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));

    }

    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    /*start add by Hina on 22-05-2025 for tax on oc*/
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    /*End add by Hina on 22-05-2025 for tax on oc*/

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#QT_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        //var TotalAMount = parseFloat(0).toFixed(DecDigit);

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            /*start add by Hina on 22-05-2025 for tax on oc*/
            var td = "";
            if (DocumentMenuId == "105103120") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
                
            }
            /*End add by Hina on 22-05-2025 for tax on oc*/
            debugger;
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
<td align="right">${currentRow.find("#OCAmount").text()}</td>
`+ td +`
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
            /*start add by Hina on 22-05-2025 for tax on oc*/
            if (DocumentMenuId == "105103120") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
            }
            /*end add by Hina on 22-05-2025 for tax on oc*/
        });
    }
    $("#_OtherChargeTotal").text(TotalAMount);
    /*start add by Hina on 22-05-2025 for tax on oc*/
    if (DocumentMenuId == "105103120") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
    /*end add by Hina on 22-05-2025 for tax on oc*/
}

//------------------End------------------//
//------------------SO Items Section------------------//
function AddNewRow() {
    debugger;
    var rowIdx = 0;
    var ItemInfo = $("#ItmInfo").text();
    var TaxInfo = $("#TaxInfo").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var Span_BuyerInformation_Title = $("#Span_BuyerInformation_Title").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    //debugger;
    /* var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#QTItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#QTItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
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
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var TaxExempted = "";
    var ManualGst = "";
    var FOC = "";
    if (DocumentMenuId == "105103120") {
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
        }
        FOC = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        TaxExempted = ' <td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input  margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" for="" style="padding: 3px 0px;"> </label></div></td>'
    }

    var displaynoneinDomestic = DocumentMenuId == "105103120" ? "style='display: none;'" : "";
    $('#QTItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="SRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<select class="form-control" id="SQItemListName${RowNo}" name="SQItemListName" onchange ="OnChangeQTItemName(${RowNo},event)"></select>
<span id="SQItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${ItemInfo}"></button>
</div>
<div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="CustomerInformation" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button>
</div>
<div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-history" aria-hidden="true" data-toggle="" title="${$("#Span_SaleHistory_Title").text()}"></i> </button>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
<input id="UOMID" type="hidden" />
<input type="hidden" id="ItemHsnCode" />
<input id="ItemtaxTmplt"  type="hidden" />
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="QuotQty" class="form-control date num_right" autocomplete="off" onchange ="OnChangeQTItemQty(event)" onpaste="return CopyPasteData(event);" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="QuotQty_name"  placeholder="0000.00">
<span id="Qt_qty_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemQuotQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemQuotQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="OrderQuantity" class="form-control num_right" autocomplete="off" type="text" name="OrderQuantity_name" placeholder="0000.00"  disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemSOrderQty">
<button type="button" id="SubItemSOrderQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('SOrderQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="lpo_form">
<input id="item_rate" class="form-control date num_right"  onchange ="OnChangeQTItemRate(event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate_name"  placeholder="0000.00">
<span id="item_rateError" class="error-message is-visible"></span>
</div>
</td>
<td>
 <div class="col-sm-10 lpo_form no-padding">
   <input id="Landed_Cost" class="form-control date num_right" onchange="OnChangeLandedCost(event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return RateFloatValueonly(this,event);" type="text" name="Landed Cost" placeholder="0000.00">
<input hidden="hidden" id="LandedPrice_Remarks"/>

 </div>
 <div class="col-sm-2 i_Icon">
     <button type="button" id="ItmInfoBtnIconLandedCost" class="calculator"  onclick="OnClickIconBtnLandedCost(event);" data-toggle="modal" data-target="#ReasonRemarks" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_LandedPriceremarks").text()}"></button>
 </d </td>
<td>
<div class="lpo_form">
<input id="item_disc_perc" class="form-control date num_right" onchange ="OnChangeQTItemDiscountPerc(event)" onpaste="return CopyPasteData(event);" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc_name" placeholder="0000.00">
<span id="item_disc_percError" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="item_disc_amt" class="form-control date num_right" onchange ="OnChangeQTItemDiscountAmt(event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt_name"  placeholder="0000.00">
<span id="item_disc_amtError" class="error-message is-visible"></span>
</div>
</td>
<td>
<input id="item_disc_val" class="form-control date num_right" autocomplete="off" type="text" name="item_disc_val_name"  placeholder="0000.00"  disabled>
</td>
<td>
<input id="item_gr_val" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val_name"  placeholder="0000.00"  disabled>
</td>
<td ${displaynoneinDomestic}>
<div class="lpo_form">
<input id="item_ass_val" class="form-control date num_right" autocomplete="off" type="text" onpaste="return CopyPasteData(event);" onkeypress="return Ass_valFloatValueonly(this,event);" onchange="OnChangeAssessAmt(event)" name="item_ass_val_name"  placeholder="0000.00" disabled>
<span id="item_ass_valError" class="error-message is-visible"></span>
</div>
</td>
`+ FOC + `
`+ TaxExempted + `
`+ ManualGst + `
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt_name"  placeholder="0000.00"  disabled>
</div>
<div class="col-sm-2 lpo_form no-padding">
<button type="button" class="calculator" id="BtnTxtCalculation"  onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${TaxInfo}" data-original-title=""></i></button>
</div>
</td>
<td>
<input id="item_oc_amt" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt_name"  placeholder="0000.00"  disabled>
</td>
<td ${displaynoneinDomestic}>
<input id="item_net_val_spec" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec_name"  placeholder="0000.00"  disabled>
</td>
<td>
<input id="item_net_val_bs" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs_name"  placeholder="0000.00"  disabled>
</td>
<td>
<textarea id="remarksid" class="form-control remarksmessage" name="remarks_name" onmouseover="OnMouseOver(this)" maxlength = "250",  placeholder="${$("#span_remarks").text()}"></textarea>
</td>
</tr>`);
    BindQTItmList(RowNo);
};
function BindDLLQTItemList() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103120") {
        BindItemList("#SQItemListName", "1", "#QTItmDetailsTbl", "#SNohiddenfiled", "", "SQ");
    }
    else {
        BindItemList("#SQItemListName", "1", "#QTItmDetailsTbl", "#SNohiddenfiled", "", "ESQ");
    }
}
function BindQTItmList(ID) {
    debugger;
    var ItmDDLName = "#SQItemListName";
    var TableID = "#QTItmDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103120") {
        DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "SQ")
       // BindItemList("#SQItemListName", "1", "#QTItmDetailsTbl", "#SNohiddenfiled", "", "SQ");
    }
    else {
        DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "ESQ")
        //BindItemList("#SQItemListName", "1", "#QTItmDetailsTbl", "#SNohiddenfiled", "", "ESQ");
    }
    //DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "SO") 
    /*Commented by NItesh 21112025*/

    //BindItemList("#SQItemListName", ID, "#QTItmDetailsTbl", "#SNohiddenfiled", "", "SQ");

}
function FOCDisabledAndEnable(currentrow, Flag) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var QtyDigit = $("#QtyDigit").text();
    if (Flag == "Y") {
        currentrow.find("#item_rate").val(parseFloat(0).toFixed(QtyDigit))
        currentrow.find("#item_rate").attr("disabled", true)
        currentrow.find("#Landed_Cost").val(parseFloat(0).toFixed(QtyDigit));
        currentrow.find("#Landed_Cost").attr("disabled", true);
        currentrow.find("#item_disc_perc").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_disc_perc").attr("disabled", true);
        currentrow.find("#item_disc_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_disc_amt").attr("disabled", true);
        currentrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_disc_val").attr("disabled", true);
        currentrow.find("#item_gr_val").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_gr_val").attr("disabled", true);
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_tax_amt").attr("disabled", true);
        currentrow.find("#item_oc_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_oc_amt").attr("disabled", true);
        currentrow.find("#item_net_val_bs").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_net_val_bs").attr("disabled", true)
        currentrow.find("#remarks").val("");
        currentrow.find("#remarks").attr("disabled", true);

        currentrow.find("#item_rateError").css("display", "none");
        currentrow.find("#item_rate").css("border-color", "#ced4da");

        currentrow.find("#item_disc_amtError").css("display", "none");
        currentrow.find("#item_disc_amt").css("border-color", "#ced4da");

        //var Itm_ID;
        //var SNo = currentrow.find("#SNohiddenfiled").val();
        //Itm_ID = currentrow.find("#SQItemListName" + SNo).val();

        //$("#Hdn_TaxCalculatorTbl tbody tr #TaxItmCode:contains('" + Itm_ID + "')").closest('tr').each(function () {
        //    var currentRow = $(this);
        //    currentRow.find("#TaxAmount").text("0");
        //    currentRow.find("#TotalTaxAmount").text("0");
          
        //})

        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#TaxExempted").prop("disabled", true);
        currentrow.find("#ManualGST").prop("disabled", true);
    }
    else {
        currentrow.find("#item_rate").attr("disabled", false)
        currentrow.find("#Landed_Cost").attr("disabled", false);
        currentrow.find("#item_disc_perc").attr("disabled", false);
        currentrow.find("#item_disc_amt").attr("disabled", false);
        currentrow.find("#remarks").attr("disabled", false);

        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        currentrow.find("#TaxExempted").prop("disabled", false);
        currentrow.find("#ManualGST").prop("disabled", false);
    }
}
function OnClickFOCCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    if (currentrow.find("#FOC").is(":checked")) {
        FOCDisabledAndEnable(currentrow, "Y");
    }
    else {
        FOCDisabledAndEnable(currentrow, "N");
    }
    OnClickTaxExemptedCheckBox(e);
}
function Ass_valFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpValDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
            return false;
        }
    }

    var clickedrow = $(evt.target).closest("tr");

    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_ass_valError" + RowNo).css("display", "none");
    clickedrow.find("#item_ass_val" + RowNo).css("border-color", "#ced4da");

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function RateFloatValueonlyConvRate(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpRateDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
            return false;
        }
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");
    return true;
}
function QtyFloatValueonly(el, evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#Qt_qty_Error" + RowNo).css("display", "none");
    clickedrow.find("#QuotQty" + RowNo).css("border-color", "#ced4da");

    return true;
}
function AmtFloatValueonly(el, evt) {
    //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpValDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
            return false;
        }
    }

    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#item_rate").val();
    item_rate = CheckNullNumber(item_rate);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    else {
        var valPer = el.value + key;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            if (Cmn_CheckSelectedTextInTextField(evt) == true) {
                return true;
            };
            return false;
        }
    }


    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");

    return true;
}
function FloatValuePerOnly(el, evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    $("#SpanTaxPercent").css("display", "none");
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    ////debugger;
    //var key = evt.key;
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > 1) {
    //    return false;
    //}
    return true;
}
function BindQTItemList(e) {
    debugger;
    //var Cust_id=$("#hdn_Cust_Name").val();
    var Cust_id = $("#Customer_Name option:selected").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var RateDecDigit = $("#ExpImpRateDigit").text();
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var RateDecDigit = $("#RateDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
    }

    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var PPolicy = $("#SpanCustPricePolicy").text();
    var PGroup = $("#SpanCustPriceGroup").text();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    Itm_ID = clickedrow.find("#SQItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {

        clickedrow.find("#SQItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SQItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + SNo + "-container']").css("border", "1px solid red");

    }
    else {

        clickedrow.find("#SQItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + SNo + "-container']").css("border", "1px solid #aaa");

    }
    ClearRowDetails(e, ItemID);

    DisableHeaderField();
    debugger;
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "sale")
    
    debugger;
    if (PPolicy == 'P') {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DomesticSalesQuotation/GetPriceListRate",
                data: {
                    Itm_ID: Itm_ID,
                    PPolicy: PPolicy,
                    PGroup: PGroup,
                    Cust_id: Cust_id
                },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (clickedrow.find("#item_rate").val() == "") {
                            if (arr.Table.length > 0) {
                                clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].effect_price).toFixed(RateDecDigit));
                                clickedrow.find("#item_rate").attr("disabled", true);
                            }
                            else {
                                clickedrow.find("#item_rate").val(parseFloat("0").toFixed(QtyDecDigit));
                                clickedrow.find("#item_rate").attr("disabled", true);
                            }
                            debugger;
                            var itemrate = clickedrow.find("#item_rate").val();
                            if (itemrate != "" && itemrate != "0.000") {
                                clickedrow.find("#item_rateError").css("display", "none");
                                clickedrow.find("#item_rate").css("border", "1px solid #ced4da");
                            }
                            var assVal = clickedrow.find("#item_ass_val").val();
                            assVal = parseFloat(assVal);
                            if (assVal > 0) {
                                clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
                                clickedrow.find("#item_ass_valError").css("display", "none");
                                clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
                            }
                            else {
                                clickedrow.find("#BtnTxtCalculation").prop("disabled", true);

                            }
                        }
                        //if (arr.Table.length > 0) {
                        //        clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].effect_price).toFixed(RateDecDigit));
                        //        clickedrow.find("#item_rate").attr("disabled", true);
                        //}
                        //else {
                        //        clickedrow.find("#item_rate").val(parseFloat("0").toFixed(QtyDecDigit));
                        //}
                        //// debugger;
                        //var itemrate = clickedrow.find("#item_rate").val();
                        //if (itemrate != "" && itemrate !="0.000") {
                        //    clickedrow.find("#item_rateError").css("display", "none");
                        //    clickedrow.find("#item_rate").css("border", "1px solid #ced4da");
                        //}

                    }
                    debugger;
                    /*var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();*/
                    //var SNo = clickedrow.find("#SNohiddenfiled").val();
                    //var ItemCode = "";
                    //ItemCode = clickedrow.find("#SQItemListName" + SNo).val();
                    //$("#hdn_Sub_ItemDetailTbl TBODY TR").each(function () {
                    //    debugger;
                    //    var row = $(this);
                    //    var rowitem = row.find("#ItemId").val();
                    //    if (rowitem == ItemCode) {
                    //        $(this).remove();
                    //    }
                    //});
                },
            });
        } catch (err) {
        }
    }

}
function DisableHeaderField() {
    debugger;
    $("#CustomerTypeC").attr('disabled', true);
    $("#CustomerTypeP").attr('disabled', true);
    $("#Customer_Name").attr('disabled', true);
    //$("#sales_person").attr('disabled', true);
    //$("#dremarks").attr('disabled', true);
    $("#Div_ProspectSetup").css("display", "none");
}



function CheckSOHraderValidations() {
    //debugger;
    var ErrorFlag = "N";
    if ($("#Customer_Name").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#Customer_Name").css("border-color", "Red");
        //$("[aria-labelledby='select2-CustomerName-container']").css("border-color", "red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("#Customer_Name").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
    }
    if ($("#txtCurrency").val() == null || $("#txtCurrency").val() == "" || $("#txtCurrency").val() == "0") {
        $("#txtCurrency").css("border-color", "Red");
        $('#SpanCustCurrErrorMsg').text($("#valueReq").text());
        $("#SpanCustCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustCurrErrorMsg").css("display", "none");
        $("#txtCurrency").css("border-color", "#ced4da");
    }
    if ($("#conv_rate").val() === "0" || $("#conv_rate").val() == "") {
        $('#SpanExRateErrorMsg').text($("#valueReq").text());
        $("#SpanExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
}
function CalculateDisPercent(e, clickedrow, RateChange) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var ValDecDigit = $("#ValDigit").text();
    }

    if (RateChange != "RateChange") {
        var clickedrow = $(e.target).closest("tr");
    }
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var ItemName;
    var QuotQty;
    var ItmRate;
    var DisPer;

    ItemName = clickedrow.find("#SQItemListName" + Sno).val();
    QuotQty = clickedrow.find("#QuotQty").val();
    ItmRate = clickedrow.find("#item_rate").val();
    debugger;
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (AvoidDot(DisPer) == false) {
        clickedrow.find("#item_disc_perc").val("");
        DisPer = 0;
    }
    if (parseFloat(CheckNullNumber(DisPer)) >= 100) {
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
        clickedrow.find("#item_disc_percError").css("display", "block");
        clickedrow.find("#item_disc_perc").css("border-color", "red");
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
        return false;
    } else {
        if (parseFloat(CheckNullNumber(DisPer)) == "" || parseFloat(CheckNullNumber(DisPer)) < 100) {
            clickedrow.find("#item_disc_percError").text("");
            clickedrow.find("#item_disc_percError").css("display", "none");
            clickedrow.find("#item_disc_perc").css("border-color", "#ced4da");
        }
    }

    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {

        var FAmt = (ItmRate * DisPer) / 100;
        var GAmt = QuotQty * (ItmRate - FAmt);
        var DisVal = QuotQty * FAmt;
        var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
        var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinGVal);
        clickedrow.find("#item_ass_val").val(FinGVal);
        clickedrow.find("#item_net_val_spec").val(FinGVal);
        FinalGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#item_net_val_bs").val(FinalGVal);
        clickedrow.find("#item_disc_val").val(FinDisVal);
        CalculateAmount();

        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(2));
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = QuotQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
            clickedrow.find("#item_net_val_bs").val(FinalVal);
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));

            CalculateAmount();
        }

        clickedrow.find("#item_disc_amt").prop("readonly", false);
    }
    OnChangeGrossAmt();

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
}
function CalculateDisAmt(e, clickedrow, RateChange) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var ValDecDigit = $("#ValDigit").text();
    }

    if (RateChange != "RateChange") {
        var clickedrow = $(e.target).closest("tr");
    }
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var ItemName;
    var QuotQty;
    var ItmRate;
    var DisAmt;

    ItemName = clickedrow.find("#SQItemListName" + Sno).val();
    QuotQty = clickedrow.find("#QuotQty").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();

    if (AvoidDot(DisAmt) == false) {
        clickedrow.find("#item_disc_amt").val("");
        DisAmt = 0;
    }

    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        debugger;
        //if (Math.fround(ItmRate) > Math.fround(DisAmt))
        if (parseFloat(CheckNullNumber(ItmRate)) > parseFloat(CheckNullNumber(DisAmt))) {
            var FRate = (ItmRate - DisAmt);
            var GAmt = QuotQty * FRate;
            var DisVal = QuotQty * DisAmt;
            var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
            var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);

            clickedrow.find("#item_disc_val").val(FinDisVal);
            clickedrow.find("#item_gr_val").val(FinGVal);
            clickedrow.find("#item_ass_val").val(FinGVal);
            clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
            clickedrow.find("#item_net_val_bs").val(FinalGVal);
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_perc").val("");
            CalculateAmount();
            clickedrow.find("#item_disc_amtError").text("");
            clickedrow.find("#item_disc_amtError").css("display", "none");
            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        }

        else {
            debugger;
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
            clickedrow.find("#item_disc_amtError").css("display", "block");
            clickedrow.find("#item_disc_amt").css("border-color", "red");
            //clickedrow.find("#item_disc_amt").val('');
            clickedrow.find("#item_disc_val").val('');
        }


        clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ValDecDigit));

    }
    else {
        if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = QuotQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
            clickedrow.find("#item_net_val_bs").val(FinalVal);
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            clickedrow.find("#item_disc_amtError").text("");
            clickedrow.find("#item_disc_amtError").css("display", "none");
            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");

            CalculateAmount();
        }

        clickedrow.find("#item_disc_perc").prop("readonly", false);


    }
    OnChangeGrossAmt();

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());


}
function CalculationBaseQty(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDecDigit = $("#ExpImpValDigit").text();
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var ValDecDigit = $("#ValDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
    }

    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var QuotQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;

    QuotQty = clickedrow.find("#QuotQty").val();
    ItemName = clickedrow.find("#SQItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#SQItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SQItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#QuotQty").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SQItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (QuotQty != "" && QuotQty != ".") {
        QuotQty = parseFloat(QuotQty);
    }
    if (QuotQty == ".") {
        QuotQty = 0;
    }
    if (QuotQty == "" || QuotQty == 0) {
        clickedrow.find("#Qt_qty_Error").text($("#valueReq").text());
        clickedrow.find("#Qt_qty_Error").css("display", "block");
        clickedrow.find("#QuotQty").css("border-color", "red");
        clickedrow.find("#QuotQty").val("");
        errorFlag = "Y";
    }
    else {
        //clickedrow.find("#Qt_qty_Error").text("");
        clickedrow.find("#Qt_qty_Error").css("display", "none");
        clickedrow.find("#QuotQty").css("border-color", "#ced4da");

    }

    QuotQty = clickedrow.find("#QuotQty").val();
    if (AvoidDot(QuotQty) == false) {
        clickedrow.find("#QuotQty").val("");
        QuotQty = 0;
    }
    else {
        clickedrow.find("#QuotQty").val(parseFloat(QuotQty).toFixed(QtyDecDigit));
    }
    if ((QuotQty !== 0 || QuotQty !== null || QuotQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = QuotQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        debugger;
        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_ass_valError").text("");
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#item_net_val_bs").val(FinalVal);

        CalculateAmount();
    }
    debugger;
    if (QuotQty == "0" && QuotQty == "" || ItmRate == "0" && ItmRate == "") {

        clickedrow.find("#item_disc_perc").val(parseFloat(0).toFixed(2));
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
    }
    if (QuotQty == "0" && QuotQty == "" || ItmRate == "0" && ItmRate == "") {
        clickedrow.find("#item_disc_amt").val(parseFloat(0).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
    }
    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00") {
        CalculateDisPercent(e, "", "");
    }
    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0.00") {
        CalculateDisAmt(e, "", "");
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
}
function CalculationBaseRate(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDecDigit = $("#ExpImpValDigit").text();
        var RateDecDigit = $("#ExpImpRateDigit").text();
    }
    else {
        var ValDecDigit = $("#ValDigit").text();
        var RateDecDigit = $("#RateDigit").text();
    }

    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var errorFlag = "N";
    var QuotQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;

    QuotQty = clickedrow.find("#QuotQty").val();
    ItemName = clickedrow.find("#SQItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#SQItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SQItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#QuotQty").val("");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SQItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    QuotQty = clickedrow.find("#QuotQty").val();
    if (QuotQty == "" || QuotQty == "0") {
        clickedrow.find("#Qt_qty_Error").text($("#valueReq").text());
        clickedrow.find("#Qt_qty_Error").css("display", "block");
        clickedrow.find("#QuotQty").css("border-color", "red");
        clickedrow.find("#QuotQty").val("");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#Qt_qty_Error").css("display", "none");
        clickedrow.find("#Qt_qty_Error").css("border-color", "#ced4da");
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
    if (ItmRate == "" || ItmRate == "0") {
        clickedrow.find("#item_rateError").text($("#valueReq").text());
        clickedrow.find("#item_rateError").css("display", "block");
        clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }


    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
    }
    else {
        clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    }
    debugger;

    if ((QuotQty !== 0 || QuotQty !== null || QuotQty !== "") && (ItmRate !== "0" || ItmRate !== null || ItmRate !== "")) {
        var FAmt = QuotQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        //clickedrow.find("#item_oc_amt").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#item_net_val_bs").val(FinalVal);

        CalculateAmount();
    }
    debugger;
    if (QuotQty == "0" && QuotQty == "" || ItmRate == "0" && ItmRate == "") {

        clickedrow.find("#item_disc_perc").val(parseFloat(0).toFixed(2));
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
    }
    if (QuotQty == "0" && QuotQty == "" || ItmRate == "0" && ItmRate == "") {
        clickedrow.find("#item_disc_amt").val(parseFloat(0).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
    }
    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00") {
        debugger;
        CalculateDisPercent("e", clickedrow, "RateChange");
    }
    debugger;
    if (QuotQty !== 0 && QuotQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0.00") {
        debugger;
        CalculateDisAmt("e", clickedrow, "RateChange");
    }


    OnChangeGrossAmt();
    debugger;
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
   
    
    debugger;
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
        clickedrow.find("#item_ass_valError").css("display", "none");
    }
    else {

        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
}
function OnchangeConvRate(e) {
    debugger;
    var ExchDecDigit = $("#ExchDigit").text();
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    if (AvoidDot(ConvRate) == false) {
        $("#conv_rate").val("");
        $("#hdnconv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
    } else {
        $("#conv_rate").val(parseFloat(ConvRate).toFixed(ExchDecDigit));
        $("#hdnconv_rate").val(parseFloat(ConvRate).toFixed(ExchDecDigit));
    }
    $("#QTItmDetailsTbl > tbody > tr").each(function () {
        var clickedrow = $(this);
        CalculationBaseRate(clickedrow);
    });

    $("#ht_Tbl_OC_Deatils > tbody > tr").each(function () {
        var clickedrow = $(this);
        clickedrow.find("#OC_Conv").text(ConvRate);
        var spcAmt = clickedrow.find("#OCAmtSp").text();
        var bsAmt = ConvRate * spcAmt;
        clickedrow.find("#OCAmtBs").text(bsAmt);
    });

}
function CalculateAmount() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var OCValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var FinalNetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var intVal = 1;
    var conv_rate = $("#conv_rate").val();

    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;

        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_ass_val").val() == "" || currentRow.find("#item_ass_val").val() == "NaN") {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_ass_val").val())).toFixed(DecDigit);
        }
        debugger;
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);

        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);

        }
        
        if (currentRow.find("#item_net_val_spec").val() == "" || currentRow.find("#item_net_val_spec").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val_spec").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#item_net_val_bs").val() == "" || currentRow.find("#item_net_val_bs").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);

        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#item_net_val_bs").val())).toFixed(DecDigit);

        }
        intVal = intVal + 1;

    });
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
  
    $("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
    CalculateTotalLandedCost();
};
function OnChangeGrossAmt() {
    debugger;
    //var TotalOCAmt = $('#_OtherChargeTotal').text();
    var TotalOCAmt = $('#_OtherChargeTotalAmt').text();

    if ($("#DocumentMenuId").val() == "105103120") {
        TotalOCAmt = $('#_OtherChargeTotalAmt').text();
    }
    else {
        TotalOCAmt = $('#_OtherChargeTotal').text();
    }
    var Total_PO_OCAmt = $('#QT_OtherCharges').val();


    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
       /* if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {*//*commented by hina on 22-05-2025*/
            debugger;
            Calculate_OC_AmountItemWise(TotalOCAmt);

        /*}*/
    }
}
function OnChangeCustomer(CustID) {
    debugger;
    var Cust_id = CustID.value;
    var CustPros_type;
    var Cust_type;
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
    }
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
    }
    if ($("#OrderTypeD").is(":checked")) {
        Cust_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Cust_type = "E";
    }
    $("#hdn_Cust_Name").val(Cust_id);
    if (Cust_id == "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#SpanCustNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-Customer_Name-container']").css("border-color", "red");

        DisableItemDetail();

        $("#TxtBillingAddr").val("");
        $("#TxtShippingAddr").val("");
        $("#txtCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
    }
    else {
        $('#SpanCustNameErrorMsg').text("");
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-Customer_Name-container']").css("border-color", "#ced4da");

        $("#QTItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            currentRow.find("#SQItemListName" + Sno).attr("disabled", false);
            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
            currentRow.find("#BtnTxtCalculation").attr("disabled", true);
            currentRow.find("#QuotQty").attr("disabled", false);
            currentRow.find("#item_rate").attr("disabled", false);
            currentRow.find("#Landed_Cost").attr("disabled", false);
            currentRow.find("#ItmInfoBtnIconLandedCost").attr("disabled", false);
            currentRow.find("#item_disc_perc").attr("disabled", false);
            currentRow.find("#item_disc_amt").attr("disabled", false);
            currentRow.find("#item_ass_val").attr("disabled", true);
            currentRow.find("#remarksid").attr("disabled", false);
            currentRow.find("#TaxExempted").attr("disabled", false);
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                currentRow.find("#ManualGST").attr("disabled", false);
            }
            currentRow.find("#SQItemListNameError").css("display", "none");
            currentRow.find("#SQItemListName" + Sno).css("border-color", "#ced4da");
            currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
            currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            currentRow.find("#Qt_qty_Error").css("display", "none");
            currentRow.find("#QuotQty").css("border-color", "#ced4da");
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
            currentRow.find("#item_ass_valError").css("display", "none");
            currentRow.find("#item_ass_val").css("border-color", "#ced4da");

        })
        $("#QTItmDetailsTbl .plus_icon1").css("display", "block");
    }

    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DomesticSalesQuotation/GetCustAddrDetail",
                data: {
                    Cust_id: Cust_id,
                    CustPros_type: CustPros_type
                },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            debugger;
                            $("#TxtBillingAddr").val(arr.Table[0].BillingAddress);
                            $("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#txtCurrency").val(arr.Table[0].curr_name);
                            //var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            //$("#txtCurrency").html(s);
                            debugger;
                            var DocumentMenuId = $("#DocumentMenuId").val();
                            if (DocumentMenuId == "105103145105") {
                                $("#AvailableCreditLimit").val(parseFloat(arr.Table[0].cre_limit).toFixed($("#ExpImpValDigit").text()));
                            }
                            else {
                                $("#AvailableCreditLimit").val(parseFloat(arr.Table[0].cre_limit).toFixed($("#ValDigit").text()));
                            }
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            $("#hdnconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));

                            /*$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));*/
                            if (Cust_type == "D") {
                                /* $("#conv_rate").prop("disabled", true);*/
                                $("#conv_rate").attr("readonly", true);
                            }
                            else {
                                var Custtype = $("#hdn_CustPros_type").val();
                                if (Custtype == "P") {
                                    /*$("#conv_rate").attr("readonly", true);*/
                                    $("#conv_rate").attr("readonly", false);
                                }
                                else {
                                    if (arr.Table[0].bs_curr_id == arr.Table[0].curr_id) {
                                        $("#conv_rate").attr("readonly", true);
                                    } else {
                                        $("#conv_rate").attr("readonly", false);
                                    }
                                }

                            }
                            debugger;
                            $("#SpanCustPricePolicy").text(arr.Table[0].cust_pr_pol);
                            $("#SpanCustPriceGroup").text(arr.Table[0].cust_pr_grp);

                        }
                        else {
                            debugger;
                            $("#TxtBillingAddr").val("");
                            $("#TxtShippingAddr").val("");
                            $("#bill_add_id").val("");
                            $("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                            $("#txtCurrency").val("");
                            $("#conv_rate").val("");
                            $("#SpanCustPricePolicy").val("");
                            $("#SpanCustPriceGroup").val("");
                            //var s = '<option value="0">---Select---</option>';
                            //$("#txtCurrency").html(s);
                            //$("#conv_rate").val("");
                        }


                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function DisableItemDetail() {
    debugger;
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        currentRow.find("#SQItemListName" + Sno).attr("disabled", true);
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
        currentRow.find("#BtnTxtCalculation").attr("disabled", true);
        currentRow.find("#QuotQty").attr("disabled", true);
        currentRow.find("#item_rate").attr("disabled", true);
        currentRow.find("#item_disc_perc").attr("disabled", true);
        currentRow.find("#item_disc_amt").attr("disabled", true);
        currentRow.find("#item_ass_val").attr("disabled", true);
        currentRow.find("#remarksid").attr("disabled", true);
        currentRow.find("#SQItemListNameError").css("display", "none");
        currentRow.find("#SQItemListName" + Sno).css("border-color", "#ced4da");
        currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
        currentRow.find("#Qt_qty_Error").css("display", "none");
        currentRow.find("#QuotQty").css("border-color", "#ced4da");
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
        currentRow.find("#item_ass_valError").css("display", "none");
        currentRow.find("#item_ass_val").css("border-color", "#ced4da");


    })
    $("#QTItmDetailsTbl .plus_icon1").css("display", "none");
}
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDigit = $("#ExpImpValDigit").text();
    }
    else {
        var ValDigit = $("#ValDigit").text();
    }

    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var item_gross_val = currentrow.find("#item_gr_val").val();
    var ItmCode = currentrow.find("#SQItemListName" + RowSNo).val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    $("#HiddenRowSNo").val(RowSNo)
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(item_gross_val);
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculationBaseQty(e);
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            //if ($("#taxTemplate").text() == "GST Slab") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "QTItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", "TaxCalcGRNNo", "TaxCalcItemCode", e)
            //}
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
        //CalculationBaseQty(e);
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var ValDigit = $("#ExpImpValDigit").text();
    }
    else {
        var ValDigit = $("#ValDigit").text();
    }
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#SQItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        $("#taxTemplate").text("Template")
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculationBaseQty(e);
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "QTItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", "TaxCalcGRNNo", "TaxCalcItemCode", e)
        CalculationBaseQty(e);
        $("#taxTemplate").text("Template")
    }
}
function OnChangeSalesPerson() {
    var SaleParson = $("#sales_person").val();
    if (SaleParson == "0" || SaleParson == "" || SaleParson == null) {
        $("#sales_person").css("border-color", "red");
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $('[aria-labelledby="select2-sales_person-container"]').css("border-color", "Red");
        $("#SpanSalesPersonErrorMsg").css("display", "block");
    }
    else {
        $("#sales_person").css("border-color", "#ced4da");
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-sales_person-container"]').css("border-color", "#ced4da");
    }
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#SQItemListName" + Sno).val();
    ItmName = clickedrow.find("#SQItemListName" + Sno + " option:selected").text()
    ItemInfoBtnClick(ItmCode);
}
function OnClickHistoryIconBtn(e) {
    debugger;
    //$("#tbl_trackingDetail").DataTable().destroy();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var CustID = "";
    CustID = $("#Customer_Name").val();
    ItmCode = clickedrow.find("#SQItemListName" + Sno).val();
    ItmName = clickedrow.find("#SQItemListName" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM").val();
    SalesOrderHistoryBtnClick(ItmCode, CustID, ItmName, UOMName);

}
function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var CustID = "";
    CustID = $("#Customer_Name").val();
    SalesOrderHistoryBtnClick(ItmCode, CustID, ItmName, UOMName);
}

function OnChangeQTItemQty(e) {
    //debugger;
    CalculationBaseQty(e);
    CalculateTotalLandedCost();
}
function OnChangeQTItemRate(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculationBaseRate(clickedrow);
}
function OnChangeQTItemDiscountPerc(e) {
    debugger;
    CalculateDisPercent(e, "", "");
    CalculateTotalLandedCost();
}
function OnChangeQTItemDiscountAmt(e) {
    debugger;
    CalculateDisAmt(e, "", "");
    CalculateTotalLandedCost();
}
function OnChangeAssessAmt(e) {
    debugger;
    var errorFlag = "N";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }

    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var AssessAmt = parseFloat(0).toFixed(DecDigit);
    AssessAmt = clickedrow.find("#item_ass_val").val();
    ItmCode = clickedrow.find("#SQItemListName" + Sno).val();
    if (AvoidDot(AssessAmt) == false) {

        errorFlag = "Y";
    }
    if (CheckItemRowValidation(e) == false) {
        errorFlag = "Y";
    }
    if (errorFlag == "Y") {
        clickedrow.find("#item_ass_val").val("");
        AssessAmt = 0;
        //clickedrow.find("#item_ass_valError").text($("#valueReq").text());
        //clickedrow.find("#item_ass_valError").css("display", "block");
        //clickedrow.find("#item_ass_val").css("border-color", "red");
        //return false;
    }
    if (parseFloat(AssessAmt) > 0) {
        //$("#BtnTxtCalculation").prop("disabled", false);
        clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
    }
    else {
        //$("#BtnTxtCalculation").prop("disabled", true);
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    debugger;
    if (parseFloat(AssessAmt).toFixed(DecDigit) > 0 && ItmCode != "" && Sno != null && Sno != "") {
        clickedrow.find("#item_ass_val").val(parseFloat(AssessAmt).toFixed(DecDigit));
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(DecDigit));
    }
    else {
        clickedrow.find("#item_ass_val").val(parseFloat(AssessAmt).toFixed(DecDigit));
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(DecDigit));
    }
}
function OnChangeQTItemName(RowID, e) {
    debugger;
    //$("#hdn_Sub_ItemDetailTbl TBODY TR").remove();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    var ItemCode = "";
    /*ItemCode = clickedrow.find("#SQItemListName" + SNo).val();*/
    var ItemCode = clickedrow.find("#hfItemID").val();
    $("#hdn_Sub_ItemDetailTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var rowitem = row.find("#ItemId").val();
        if (rowitem == ItemCode) {
            $(this).remove();
        }
    });
    BindQTItemList(e);
}
//------------------End------------------//

function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#tgl_RaiseOrder").attr("disabled", true);
        $("#btn_save").attr("onclick", "return InsertSQTDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#tgl_RaiseOrder").attr("disabled", false);
    }
}
function CheckedFClose() {
    if ($("#ForceClosed").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#tgl_RaiseOrder").attr("disabled", true);
        $("#btn_save").attr("onclick", "return InsertSQTDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#tgl_RaiseOrder").attr("disabled", false);
    }
}
function InsertSQTDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var QTDTransType = sessionStorage.getItem("QTTransType");
    var docid = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#hdnSQGstApplicable").val(GstApplicable)
    if (CheckQTValidations() == false) {
        return false;
    }
    if (CheckQTItemValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    if (docid == "105103120") {
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("QTItmDetailsTbl", "item_tax_amt", "SQItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
                return false;
            }
        }
    }
    if ($("#QTItmDetailsTbl >tbody >tr").length == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    debugger;
    var TransType = "";
    if (QTDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    debugger;
    //$("#hdnVouMsg").val($("#CreditNotePassAgainstInv").text());

    var FinalQT_ItemDetail = [];
    var FinalSQTaxDetail = [];
    var FinalSQOCDetail = [];
    var FinalSQOC_TaxDetail = [];/*Add by Hina on 22-05-2025*/
    var FinalQTTermDetail = [];
    debugger;
    FinalQT_ItemDetail = InsertQTItemDetails();
    FinalSQTaxDetail = InsertQTTaxDetails();
    FinalSQOCDetail = InsertQTOCDetails();
    FinalSQOC_TaxDetail = InsertQT_OCTaxDetails();/*Add by Hina on 22-05-2025*/
    FinalQTTermDetail = InsertQTTermConditionDetails();
    debugger;
    $('#hdItemDetailList').val(JSON.stringify(FinalQT_ItemDetail));
    $("#hdTaxDetailList").val(JSON.stringify(FinalSQTaxDetail));
    $("#hdOCDetailList").val(JSON.stringify(FinalSQOCDetail));
    $("#hdOCTaxDetailList").val(JSON.stringify(FinalSQOC_TaxDetail));/*Add by Hina on 22-05-2025*/
    $('#hdTermsDetailList').val(JSON.stringify(FinalQTTermDetail));

    /*-----------Sub-item-------------*/

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
    $("#hdn_Cust_Name").val($("#Customer_Name").val());
    $("#hdn_Salpersn_id").val($("#sales_person").val());

    debugger;
    if ($("#CustomerTypeC").is(":checked")) {
        cust_type = "C";
        //$("#Prosbtn").css("display", "none");
    }
    else {
        cust_type = "P";

    }

    $("#hdn_CustPros_type").val(cust_type);


    var txtGrVal = $("#TxtGrossValue").val();
    var txtAssVal = $("#TxtAssessableValue").val();
    if (parseFloat(CheckNullNumber(txtGrVal)) > 0 && parseFloat(CheckNullNumber(txtAssVal)) > 0) {
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
    else {
        swal("", $("DocumentCanNotBeSavedGrossOrAssessableValueIsLessOrEqualToZero").text(), "warning");
        return false;
    }
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    $("#sales_person").attr('disabled', false);
    return true;

};
function InsertQTItemDetails() {
    debugger;
    //var QTNo = sessionStorage.getItem("DSQ_No");
    var QTNo = $("#qt_no").val();
    var QTDate = $("#qt_date").val();
    var Branch = sessionStorage.getItem("BranchID");
    var QTItemList = [];
    $("#QTItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var ItemID = "";
        var UOMID = "";
        var QuotQty = "";
        var OrderBQty = "";
        var ItmRate = "";
        var ItmDisPer = "";
        var ItmDisAmt = "";
        var DisVal = "";
        var GrossVal = "";
        var AssVal = "";
        var TaxAmt = "";
        var OCAmt = "";
        var NetValSpec = "";
        var NetValBase = "";
        var Remarks = "";
        var QtType = "";
        var cust_type = "";
        var ItemHsnCode = "";
        var ManualGST = "";
        var LandedCost = "";
        var Landed_Remarks = "";
        var FOC = "";
        var currentRow = $(this);
        var sr_no = "";
        var SNo = currentRow.find("#SNohiddenfiled").val();
        sr_no = currentRow.find("#SRNO").text();

        ItemID = currentRow.find("#SQItemListName" + SNo).val();
        UOMID = currentRow.find("#UOMID").val();

        QuotQty = currentRow.find("#QuotQty").val();

        if (currentRow.find("#OrderQuantity").val() == null || currentRow.find("#OrderQuantity").val() == "") {
            OrderBQty = "0";
        }
        else {
            OrderBQty = currentRow.find("#OrderQuantity").val();
        }
        ItmRate = currentRow.find("#item_rate").val();
        if (currentRow.find("#item_disc_perc").val() === "") {
            ItmDisPer = "0";
        }
        else {
            ItmDisPer = currentRow.find("#item_disc_perc").val();
        }
        if (currentRow.find("#item_disc_amt").val() === "") {
            ItmDisAmt = "0";
        }
        else {
            ItmDisAmt = currentRow.find("#item_disc_amt").val();
        }
        if (currentRow.find("#item_disc_val").val() === "" || currentRow.find("#item_disc_val").val() === null) {
            DisVal = "0";
        }
        else {
            DisVal = currentRow.find("#item_disc_val").val();
        }
        GrossVal = currentRow.find("#item_gr_val").val();
        AssVal = currentRow.find("#item_ass_val").val();
        if (currentRow.find("#item_tax_amt").val() === "" || currentRow.find("#item_tax_amt").val() === null) {
            TaxAmt = "0";
        }
        else {
            TaxAmt = currentRow.find("#item_tax_amt").val();
        }
        if (currentRow.find("#item_oc_amt").val() === "" || currentRow.find("#item_oc_amt").val() === null) {
            OCAmt = "0";
        }
        else {
            OCAmt = currentRow.find("#item_oc_amt").val();
        }
        NetValSpec = currentRow.find("#item_net_val_spec").val();
        NetValBase = currentRow.find("#item_net_val_bs").val();
        Remarks = currentRow.find("#remarksid").val();
        ItemHsnCode = currentRow.find("#ItemHsnCode").val();

        if ($("#OrderTypeD").is(":checked")) {
            QtType = "D";
        }
        else {
            QtType = "E";
        }
        //debugger;
        if ($("#CustomerTypeC").is(":checked")) {
            cust_type = "C";
        }
        else {
            cust_type = "P";
        }
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        if (currentRow.find("#ManualGST").is(":checked")) {
            ManualGST = "Y"
        }
        else {
            ManualGST = "N"
        }
        if (currentRow.find("#FOC").is(":checked")) {
            FOC = "Y"
        }
        else {
            FOC = "N"
        }
        LandedCost = currentRow.find("#Landed_Cost").val();
        Landed_Remarks = currentRow.find("#LandedPrice_Remarks").text();
        QTItemList.push({
            QTNo: QTNo, QTDate: QTDate, Branch: Branch, ItemID: ItemID, UOMID: UOMID,
            QuotQty: QuotQty, OrderBQty: OrderBQty, ItmRate: ItmRate, ItmDisPer: ItmDisPer,
            ItmDisAmt: ItmDisAmt, DisVal: DisVal, GrossVal: GrossVal, AssVal: AssVal,
            TaxAmt: TaxAmt, OCAmt: OCAmt, NetValSpec: NetValSpec, NetValBase: NetValBase,
            Remarks: Remarks, QtType: QtType, cust_type: cust_type, TaxExempted: TaxExempted,
            ItemHsnCode: ItemHsnCode, ManualGST: ManualGST, LandedCost: LandedCost, Landed_Remarks: Landed_Remarks, sr_no: sr_no, FOC: FOC
        });
    });
    return QTItemList;
};
function InsertQTTaxDetails() {
    debugger;

    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var SQTaxList = [];
    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#SQItemListName" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var QtType = "";
                        if ($("#OrderTypeD").is(":checked")) {
                            QtType = "D";
                        }
                        else {
                            QtType = "E";
                        }
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        SQTaxList.push({ Branch: "", QTNo: "", QTDate: "", ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxValue: TaxValue, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, QtType: QtType });

                    });
                }
            }
        }
    });
    return SQTaxList;
};
function InsertQTOCDetails() {
    debugger;
    //var QTNo = sessionStorage.getItem("DSQ_No");
    //var QTDate = $("#qt_date").val();
    var QtType = "";
    if ($("#OrderTypeD").is(":checked")) {
        QtType = "D";
    }
    else {
        QtType = "E";
    }
    //var Branch = sessionStorage.getItem("BranchID");
    var SQ_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var OCID = "";
            var OCValue = "";
            /*start add by Hina on 22-05-2025 for tax on oc*/
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            /*end add by Hina on 22-05-2025 for tax on oc*/
            OCID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            /*start add by Hina on 22-05-2025 for tax on oc*/
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            /*end add by Hina on 22-05-2025 for tax on oc*/
            SQ_OCList.push({ Branch: "", QTNo: "", QTDate: "", OCID: OCID, OCValue: OCValue, QtType: QtType, OCTaxAmt: OCTaxAmt, OCTotalTaxAmt: OCTotalTaxAmt,OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs });
        });
    }
    return SQ_OCList;
};
function InsertQTTermConditionDetails() {
    debugger;
    var QTNo = $("#qt_no").val();
    var QTDate = $("#qt_date").val();
    var QtType = "";
    if ($("#OrderTypeD").is(":checked")) {
        QtType = "D";
    }
    else {
        QtType = "E";
    }
    //var Branch = sessionStorage.getItem("BranchID");
    var SQTermsList = [];
    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var TermsDesc = "";

            TermsDesc = currentRow.find("#term_desc").text();

            SQTermsList.push({ QTNo: QTNo, QTDate: QTDate, TermsDesc: TermsDesc, QtType: QtType });
        });
    }
    return SQTermsList;
};
function InsertQT_OCTaxDetails() {/*add by Hina on 22-05-2025 for tax on oc*/
    debugger;
    var FTaxDetails = $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").length;
    var QT_OCTaxList = [];

    if (FTaxDetails != null) {
        if (FTaxDetails > 0) {

            $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").each(function () {
                var Crow = $(this);
                var QtType = "";
                
                if ($("#OrderTypeD").is(":checked")) {
                    QtType = "D";
                }
                if ($("#OrderTypeE").is(":checked")) {
                    QtType = "E";
                }
                debugger;
                var ItemID = Crow.find("#TaxItmCode").text().trim();
                var TaxID = Crow.find("#TaxNameID").text().trim();
               /* var TaxName = Crow.find("#TaxName").text().trim();*/
                var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                var TaxLevel = Crow.find("#TaxLevel").text().trim();
                var TaxValue = Crow.find("#TaxAmount").text().trim();
                var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                //var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                //var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                QT_OCTaxList.push({ Branch: "", QTNo: "", QTDate: "", ItemID: ItemID, /*TaxName: TaxName,*/ TaxID: TaxID, TaxRate: TaxRate, TaxValue: TaxValue, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, QtType: QtType/*, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount*/ });

            });
        }
    }
    return QT_OCTaxList;
};
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

function CheckQTValidations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#Customer_Name").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#Customer_Name").css("border-color", "Red");
        $("[aria-labelledby='select2-Customer_Name-container']").css("border-color", "red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("#Customer_Name").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-Customer_Name-container']").css("border-color", "#ced4da");
        //$("#Prosbtn").css("display", "none");
    }
    if ($("#sales_person").val() == null || $("#sales_person").val() == "" || $("#sales_person").val() == "0") {
        $("[aria-labelledby='select2-sales_person-container']").css("border-color", "Red");
        $("#sales_person").css("border-color", "red");
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $("#SpanSalesPersonErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-sales_person-container']").css("border-color", "#ced4da");
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $("#sales_person").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckQTItemValidations() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var RateDecDigit = $("#ExpImpRateDigit").text();
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var RateDecDigit = $("#RateDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
    }

    var ErrorFlag = "N";
    if ($("#QTItmDetailsTbl >tbody >tr").length > 0) {
        $("#QTItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#SQItemListName" + Sno).val() == "0") {
                currentRow.find("#SQItemListNameError").text($("#valueReq").text());
                currentRow.find("#SQItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SQItemListNameError").css("display", "none");
                currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#QuotQty").val() == "") {
                currentRow.find("#Qt_qty_Error").text($("#valueReq").text());
                currentRow.find("#Qt_qty_Error").css("display", "block");
                currentRow.find("#QuotQty").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#Qt_qty_Error").css("display", "none");
                currentRow.find("#Qt_qty_Error").css("border-color", "#ced4da");
            }
            if (currentRow.find("#QuotQty").val() != "") {
                if (parseFloat(currentRow.find("#QuotQty").val()).toFixed(QtyDecDigit) == 0) {
                    currentRow.find("#Qt_qty_Error").text($("#valueReq").text());
                    currentRow.find("#Qt_qty_Error").css("display", "block");
                    currentRow.find("#QuotQty").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#Qt_qty_Error").css("display", "none");
                    currentRow.find("#Qt_qty_Error").css("border-color", "#ced4da");
                }
            }
            if (currentRow.find("#FOC").is(":checked")) {

            }
            else {
                if (currentRow.find("#item_rate").val() == "") {
                    currentRow.find("#item_rateError").text($("#valueReq").text());
                    currentRow.find("#item_rateError").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_rateError").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
                }
                if (currentRow.find("#item_rate").val() != "") {
                    if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
                        currentRow.find("#item_rateError").text($("#valueReq").text());
                        currentRow.find("#item_rateError").css("display", "block");
                        currentRow.find("#item_rate").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#item_rateError").css("display", "none");
                        currentRow.find("#item_rate").css("border-color", "#ced4da");
                    }
                }
                debugger;
                if (currentRow.find("#item_disc_perc").val() != "") {
                    var itemDiscPer = currentRow.find("#item_disc_perc").val();
                    if (parseFloat(CheckNullNumber(itemDiscPer)) >= 100) {
                        currentRow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
                        currentRow.find("#item_disc_percError").css("display", "block");
                        currentRow.find("#item_disc_perc").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                }
                if (currentRow.find("#item_disc_amt").val() != "") {
                    var itemDiscAmnt = currentRow.find("#item_disc_amt").val();
                    var itemRate = currentRow.find("#item_rate").val();
                    if (parseFloat(CheckNullNumber(itemDiscAmnt)) >= parseFloat(CheckNullNumber(itemRate))) {
                        currentRow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
                        currentRow.find("#item_disc_amtError").css("display", "block");
                        currentRow.find("#item_disc_amt").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                }
            //if (currentRow.find("#item_ass_val").val() == "") {
            //    currentRow.find("#item_ass_valError").text($("#valueReq").text());
            //    currentRow.find("#item_ass_valError").css("display", "block");
            //    currentRow.find("#item_ass_val").css("border-color", "red");
            //    ErrorFlag = "Y";
            //}
            //else {
            //    currentRow.find("#item_ass_valError").css("display", "none");
            //    currentRow.find("#item_ass_val").css("border-color", "#ced4da");
            //}
            //debugger;
            //if (currentRow.find("#item_ass_val").val() != "") {
            //    if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
            //        currentRow.find("#item_ass_valError").text($("#valueReq").text());
            //        currentRow.find("#item_ass_valError").css("display", "block");
            //        currentRow.find("#item_ass_val").css("border-color", "red");
            //        ErrorFlag = "Y";
            //    }
            //    else {
            //        //currentRow.find("#item_ass_valError").text("");
            //        currentRow.find("#item_ass_valError").css("display", "none");
            //        currentRow.find("#item_ass_val").css("border-color", "#ced4da");
            //    }
            //}
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
function OnClickbillingAddressIconBtn(e) {
    debugger;

    $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());

    var Cust_id = $('#Customer_Name').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var CustPros_type;
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
    }
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
    }
    var Amend = $("#AmendDocument").val()
    var status = "";
    if (Amend == "Amend") {
        status = "D";
    }
    else {
        status = $("#hfStatus").val().trim();
    }

    var QTDTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        QTDTransType = "Update";
    }
    //var QTDTransType = sessionStorage.getItem("QTTransType");
    var QT_no = $("#qt_no").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, QTDTransType, QT_no);
    //if (Cust_id != "" && Cust_id != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DomesticSalesQuotation/GetCustAddressdetail",
    //                data: {
    //                    CustID: Cust_id,
    //                    CustPros_type: CustPros_type,
    //                },
    //                success: function (data) {
    //                    //debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }                       
    //                    //debugger;                        
    //                    $("#tbodyAddressBodyNew").html(data);
    //                    $("#hd_bill_id").val(bill_add_id);

    //                    $("#ItemDetailsTbl >tbody >tr").each(function () {
    //                       //debugger;
    //                        var currentRow = $(this);
    //                        var Address_id = currentRow.find("td:eq(2)").text().trim();
    //                        var status = $("#hfStatus").val().trim();
    //                        var SODTransType = sessionStorage.getItem("QTTransType");
    //                        var QT_no = $("#qt_no").val();
    //                        if (status == "D" || status == null || status == "") {
    //                            if (SODTransType == "Update") {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", false);
    //                                $("#addrSaveAndExit").css("display", "block");
    //                            }
    //                            else if ((SODTransType == "" || SODTransType == null) && (QT_no == null || QT_no == "")) {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", false);
    //                                $("#addrSaveAndExit").css("display", "block");
    //                            }
    //                            else {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", true);
    //                                $("#addrSaveAndExit").css("display", "none");
    //                            }
    //                        }
    //                        else {
    //                            currentRow.find("#OrderTypeLocal").attr("disabled", true);
    //                            $("#addrSaveAndExit").css("display", "none");
    //                        }
    //                        if (Address_id == bill_add_id) {
    //                            currentRow.find('#OrderTypeLocal').prop("checked", true);
    //                            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
    //                            $(currentRow).css("background-color", "rgba(38, 185, 154, .16)");
    //                        }
    //                    });
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function OnClickShippingAddressIconBtn(e) {
    debugger;
    $('#AddressInfo_LblName').text($('#AddressInfo_ShipAdd').text());
    var Cust_id = $('#Customer_Name').val();
    var ship_add_id = $('#ship_add_id').val().trim();
    $('#hd_add_type').val("S");
    var CustPros_type;
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
    }
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
    }
    var Amend = $("#AmendDocument").val()
    var status = "";
    if (Amend == "Amend") {
        status = "D";
    }
    else {
        status = $("#hfStatus").val().trim();
    }
    var QTDTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        QTDTransType = "Update";
    }
    /* var QTDTransType = sessionStorage.getItem("QTTransType");*/
    var QT_no = $("#qt_no").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, ship_add_id, status, QTDTransType);
    //if (Cust_id != "" && Cust_id != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DomesticSalesQuotation/GetCustAddressdetail",
    //                data: {
    //                    CustID: Cust_id,
    //                    CustPros_type: CustPros_type,                    },
    //                success: function (data) {
    //                    //debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }
    //                    //debugger;
    //                    $("#tbodyAddressBodyNew").html(data);
    //                    $("#hd_ship_id").val(ship_add_id);

    //                    $("#ItemDetailsTbl >tbody >tr").each(function () {
    //                        //debugger;
    //                        var currentRow = $(this);
    //                        var Address_id = currentRow.find("td:eq(2)").text().trim();
    //                        var status = $("#hfStatus").val().trim();
    //                        var SODTransType = sessionStorage.getItem("QTTransType");
    //                        var QT_no = $("#qt_no").val();
    //                        if (status == "D" || status == null || status == "") {
    //                            if (SODTransType == "Update") {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", false);
    //                                $("#addrSaveAndExit").css("display", "block");
    //                            }
    //                            else if ((SODTransType == "" || SODTransType == null) && (QT_no == null || QT_no == "")) {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", false);
    //                                $("#addrSaveAndExit").css("display", "block");
    //                            }
    //                            else {
    //                                currentRow.find("#OrderTypeLocal").attr("disabled", true);
    //                                $("#addrSaveAndExit").css("display", "none");
    //                            }
    //                        }
    //                        else {
    //                            currentRow.find("#OrderTypeLocal").attr("disabled", true);
    //                            $("#addrSaveAndExit").css("display", "none");
    //                        }
    //                        if (Address_id == ship_add_id) {
    //                            currentRow.find('#OrderTypeLocal').prop("checked", true);
    //                            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
    //                            $(currentRow).css("background-color", "rgba(38, 185, 154, .16)");
    //                        }
    //                    });
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function OnClickBuyerInfoIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    $("#ItemCode").val("");
    $("#ItemDescription").val("");
    $("#PackingDetail").val("");
    $("#dremarks").val("");


    ItmCode = clickedrow.find("#SQItemListName" + Sno).val();
    ItmName = clickedrow.find("#SQItemListName" + Sno + " option:selected").text()
    var Cust_id = $('#Customer_Name').val();
    Cmn_BuyerInfoBtnClick(ItmCode, Cust_id);
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DomesticSalesQuotation/GetItemCustomerInfo",
    //                data: { ItemID: ItmCode, CustID: Cust_id },
    //                success: function (data) {
    //                    //debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#ItemCode").val(arr.Table[0].Item_code);
    //                            $("#ItemDescription").val(arr.Table[0].item_des);
    //                            $("#PackingDetail").val(arr.Table[0].pack_dt);
    //                            $("#dremarks").val(arr.Table[0].remark);
    //                            $("#SpanItemDescription").text(ItmName);

    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}


function GetCurrentDatetime(ActionType) {
    //debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticSalesQuotation/GetCurrentDT",/*Controller=LSODetail and Fuction=GetCurrentDT*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',/*Registration pass value like model*/
        success: function (response) {
            //debugger;
            if (response === 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            if (ActionType === "Save") {
                $("#CreatedBy").text(response.CurrentUser);
                $("#CreatedDate").text(response.CurrentDT);
            }
            if (ActionType === "Edit") {
                $("#AmdedBy").text(response.CurrentUser);
                $("#AmdedDate").text(response.CurrentDT);
            }
            if (ActionType === "Approved") {
                $("#ApproveBy").text(response.CurrentUser);
                $("#ApproveDate").text(response.CurrentDT);
            }
        }
    });
}

function SerialNoAfterDelete() {
    debugger
    var SerialNo = 0;
    $("#QTItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SRNO").text(SerialNo);


    });
};
function CheckItemRowValidation(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145105") {
        var RateDecDigit = $("#ExpImpRateDigit").text();
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var RateDecDigit = $("#RateDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
    }

    var ErrorFlag = "N";
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();
    if (currentRow.find("#SQItemListName" + Sno).val() == "0") {
        currentRow.find("#SQItemListNameError").text($("#valueReq").text());
        currentRow.find("#SQItemListNameError").css("display", "block");
        currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid red");

        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#SQItemListNameError").css("display", "none");
        currentRow.find("[aria-labelledby='select2-SQItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
    }
    if (currentRow.find("#QuotQty").val() == "") {
        currentRow.find("#Qt_qty_Error").text($("#valueReq").text());
        currentRow.find("#Qt_qty_Error").css("display", "block");
        currentRow.find("#QuotQty").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#Qt_qty_Error").css("display", "none");
        currentRow.find("#Qt_qty_Error").css("border-color", "#ced4da");
    }
    if (currentRow.find("#QuotQty").val() != "") {
        if (parseFloat(currentRow.find("#QuotQty").val()).toFixed(QtyDecDigit) == 0) {
            currentRow.find("#Qt_qty_Error").text($("#valueReq").text());
            currentRow.find("#Qt_qty_Error").css("display", "block");
            currentRow.find("#QuotQty").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#Qt_qty_Error").css("display", "none");
            currentRow.find("#Qt_qty_Error").css("border-color", "#ced4da");
        }
    }
    if (currentRow.find("#item_rate").val() == "") {
        currentRow.find("#item_rateError").text($("#valueReq").text());
        currentRow.find("#item_rateError").css("display", "block");
        currentRow.find("#item_rate").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
    }
    if (currentRow.find("#item_rate").val() != "") {
        if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
            currentRow.find("#item_rateError").text($("#valueReq").text());
            currentRow.find("#item_rateError").css("display", "block");
            currentRow.find("#item_rate").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
        }
    }
    debugger;
    //if (currentRow.find("#item_ass_val").val() == "") {
    //    currentRow.find("#item_ass_valError").text($("#valueReq").text());
    //    currentRow.find("#item_ass_valError").css("display", "block");
    //    currentRow.find("#item_ass_val").css("border-color", "red");
    //    ErrorFlag = "Y";
    //}
    //else {
    //    currentRow.find("#item_ass_valError").text("");
    //    currentRow.find("#item_ass_valError").css("display", "none");
    //    currentRow.find("#item_ass_val").css("border-color", "#ced4da");
    //}
    //if (currentRow.find("#item_ass_val").val() != "") {
    //    if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
    //        currentRow.find("#item_ass_valError").text($("#valueReq").text());
    //        currentRow.find("#item_ass_valError").css("display", "block");
    //        currentRow.find("#item_ass_val").css("border-color", "red");
    //        ErrorFlag = "Y";
    //    }
    //    else {
    //        currentRow.find("#item_ass_valError").css("display", "none");
    //        currentRow.find("#item_ass_val").css("border-color", "#ced4da");
    //    }
    //}
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}


/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    /*add start by Hina sharma on 28-01-2025 for sales enquiry to validation */
    //var docid = $("#DocumentMenuId").val();
    //var GstApplicable = $("#Hdn_GstApplicable").text();
    //$("#hdnSQGstApplicable").val(GstApplicable)

    //var ErrorFlag = "";
    //if (docid == "105103120") {
    //    if (GstApplicable == "Y") {
    //        if (Cmn_taxVallidation("QTItmDetailsTbl", "item_tax_amt", "SQItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
    //            ErrorFlag = "N";
    //            $("#Btn_Forward").attr("data-target", "");
    //            $("#Btn_Approve").attr("data-target", "");
    //            return false;
    //        }
    //        else {
    //            ErrorFlag = "Y";
    //            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //            $("#Btn_Approve").attr("data-target", "#Forward_Pop");
    //        }
    //    }
    //    else {
    //        ErrorFlag = "Y";
    //    }
    //}
   /* if (ErrorFlag == "Y") {*/
        /*add End code by Hina sharma on 28-01-2025 for sales enquiry to validation */
    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var SQDate = $("#qt_date").val();
        $.ajax({
            type: "POST",
         /*   url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: SQDate
            },
            success: function (data) {
                debugger;
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var SQStatus = "";
                    /*$("#Btn_Approve").attr("data-target", "");*/
                    SQStatus = $('#hfStatus').val().trim();
                    if (SQStatus === "D" || SQStatus === "F") {

                        /*add start by Hina sharma on 28-01-2025 for sales enquiry to validation */
                        var docid = $("#DocumentMenuId").val();
                        var GstApplicable = $("#Hdn_GstApplicable").text();
                        $("#hdnSQGstApplicable").val(GstApplicable)

                        var ErrorFlag = "";
                        if (docid == "105103120") {
                            if (GstApplicable == "Y") {
                                if (Cmn_taxVallidation("QTItmDetailsTbl", "item_tax_amt", "SQItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
                                    ErrorFlag = "N";
                                    $("#Btn_Forward").attr("data-target", "");
                                    $("#Btn_Approve").attr("data-target", "");
                                    return false;

                                }
                                else {
                                    ErrorFlag = "Y";
                                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                                    $("#Btn_Approve").attr("data-target", "#Forward_Pop");
                                }
                            }
                            else {
                                ErrorFlag = "Y";
                            }
                        }
                        //else {
                        //    if (CheckItemValidationCrtBySlsEnqry() == false) {
                        //        ErrorFlag = "N";
                        //        $("#Btn_Forward").attr("data-target", "");
                        //        $("#Btn_Approve").attr("data-target", "");
                        //        return false;
                        //    }
                        //}

                        //if (ErrorFlag == "Y") {
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
                        /*}*/

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    /*swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");
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
  /*  }*/
    //var SQStatus = "";
    //SQStatus = $('#hfStatus').val().trim();
    //if (SQStatus === "D" || SQStatus === "F") {

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






}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SQNo = "";
    var SQDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    SQNo = $("#qt_no").val();
    SQDate = $("#qt_date").val();
    $("#hdDoc_No").val(SQNo);
    Remarks = $("#fw_remarks").val();
    var WF_status1 = $("#WF_status1").val();
    var CustType = $("#CustType").val();
    var TrancType = (SQNo + ',' + SQDate + ',' + docid + ',' + WF_status1 + ',' + CustType)
    var ListFilterData1 = $("#ListFilterData1").val();
    //if ($("#OrderTypeD").is(":checked")) {
    //    InvType = "D";
    //}
    //if ($("#OrderTypeE").is(":checked")) {
    //    InvType = "E";
    //}
    //SaleVouMsg = $("#SaleVoucherPassAgainstInv").text()


    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "SalesQuotation_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(SQNo, SQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SQNo != "" && SQDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(SQNo, SQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticSalesQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ SQNo: SQNo, SQDate: SQDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/DomesticSalesQuotation/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid + "&CustType=" + CustType;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/DomesticSalesQuotation/SIListApprove?SI_No=" + SQNo + "&SI_Date=" + SQDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SQNo != "" && SQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SQNo, SQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticSalesQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SQNo != "" && SQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SQNo, SQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DomesticSalesQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
async function GetPdfFilePathToSendonEmailAlert(SQNo, SQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath) {
    var PrintFormat = [];
    var filepath = "";
    PrintFormat.push({
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        PrintFormat: $("#PrintFormat").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        ItemAliasName: "N",
        CustAliasName: $("#CustomerAliasName").val(),
        PrintRemarks: $("#PrintRemarks").val()
    })
    filepath = await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticSalesQuotation/SavePdfDocToSendOnEmailAlert",
        data: { soNo: SQNo, soDate: SQDate, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormat), docid: docid, docstatus: fwchkval },
        success: function (data) {
        }
    });
    return filepath;
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#qt_no").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {

    var SQNo = "";
    var SQDate = "";
    var docid = "";


    docid = $("#DocumentMenuId").val();
    SQNo = $("#qt_no").val();
    SQDate = $("#qt_date").val();

    var WF_status1 = $("#WF_status1").val();
    var CustType = $("#CustType").val();
    var TrancType = (SQNo + ',' + SQDate + ',' + docid + ',' + WF_status1 + ',' + CustType)
    var ListFilterData1 = $("#ListFilterData1").val();
    window.location.href = "/ApplicationLayer/DomesticSalesQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
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

/*-----------End Workflow--------------*/

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemQuotQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemSOrderQty");


}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var DocumentMenuId = $("#DocumentMenuId").val();
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#SQItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#SQItemListName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#qt_no").val();
    var Doc_dt = $("#qt_date").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#QuotQty").val();
    }
    else if (flag == "SOrderQty") {
        Sub_Quantity = clickdRow.find("#OrderQuantity").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    var Amend = $("#AmendDocument").val()
    if (Amend == "Amend") {
        hd_Status = "";
    }
    else {
        hd_Status = IsNull(hd_Status, "").trim();
    }
    var AmendmentFlag = $("#AmendmentFlag").val();
    var ddlrev_no = $("#ddlrev_no").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DomesticSalesQuotation/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            DocumentMenuId: DocumentMenuId,
            AmendmentFlag: AmendmentFlag,
            ddlrev_no: ddlrev_no
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    return Cmn_CheckValidations_forSubItems("QTItmDetailsTbl", "SNohiddenfiled", "SQItemListName", "QuotQty", "SubItemQuotQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("QTItmDetailsTbl", "SNohiddenfiled", "SQItemListName", "QuotQty", "SubItemQuotQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
    debugger;
    /*add start by Hina sharma on 28-01-2025 for sales enquiry to validation */
    var docid = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#hdnSQGstApplicable").val(GstApplicable)
    /*add End code by Hina sharma on 28-01-2025 for sales enquiry to validation */
    var ErrorFlag = "";
    if (docid == "105103120") {
        if (GstApplicable == "Y") {
            if (Cmn_taxVallidation("QTItmDetailsTbl", "item_tax_amt", "SQItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
                ErrorFlag = "N";
                debugger;
                /* e.preventDefault();*/
                $("#Btn_Forward").attr("data-target", "");
                //$("#btn_approve").attr("type", "");
                return false;

            }
            else {
                ErrorFlag = "Y";
                $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                $("#btn_approve").attr("type", "#Forward_Pop");
            }
        }
        else {
            ErrorFlag = "Y";
        }
    }
    if (ErrorFlag == "Y") {

        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickApprove") {
            $("#btn_approve").attr("disabled", true);
            $("#btn_approve").css("filter", "grayscale(100%)");
        }
        else {
            $("#btn_approve").css("filter", "grayscale(100%)");
            $("#hdnsavebtn").val("AllreadyclickApprove");

        }
        return true;
    }

}
function OnCheckedChangeProdDesc() {
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        $("#ShowProdDesc").val("Y");
        //$('#ShowCustSpecProdDesc').val('N');
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        $('#ShowCustSpecProdDesc').val('Y');
        //$('#ShowProdDesc').val('N');
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
    debugger;
    if ($('#chkcustaliasname').prop('checked')) {
        $('#CustomerAliasName').val('Y');
    }
    else {
        $('#CustomerAliasName').val('N');
    }
}
function OnCheckedChangePrintRemarks() {/*Add by Hina on 27-09-2024*/
    if ($('#chkprintremarks').prop('checked')) {
        $('#PrintRemarks').val('Y');
    }
    else {
        $('#PrintRemarks').val('N');
    }
}

function OnCheckedChangeItemAliasName() {
    debugger;
    if ($('#chkitmaliasname').prop('checked')) {
        $('#Hdn_ShowItemAliasName').val('Y');
    }
    else {
        $('#Hdn_ShowItemAliasName').val('N');
    }
}
function OnCheckedChangeCatalogueNum() {
    debugger;
    if ($('#chkshowCatalogueNum').prop('checked')) {
        $('#Hdn_ShowCatalogueNo').val('Y');
    }
    else {
        $('#Hdn_ShowCatalogueNo').val('N');
    }
}
function OnCheckedChangePrintItemImage() {
    debugger;
    if ($('#chkshowItemImage').prop('checked')) {
        $('#Hdn_ShowPrintImage').val('Y');
    }
    else {
        $('#Hdn_ShowPrintImage').val('N');
    }
}
function OnCheckedChangeRefNum() {
    debugger;
    if ($('#chkshowRefNum').prop('checked')) {
        $('#Hdn_ShowReferenceNo').val('Y');
    }
    else {
        $('#Hdn_ShowReferenceNo').val('N');
    }
}
function OnCheckedChangeOEM() {
    if ($('#chkshowOEM').prop('checked')) {
        $('#Hdn_ShowOEMNo').val('Y');
    }
    else {
        $('#Hdn_ShowOEMNo').val('N');
    }
}
function OnCheckedChangeHSNCode() {
    if ($('#chkshowHSN').prop('checked')) {
        $('#Hdn_ShowHSNCode').val('Y');
    }
    else {
        $('#Hdn_ShowHSNCode').val('N');
    }
}
function OnCheckedChangeDiscount() {
    if ($('#chkshowDiscount').prop('checked')) {
        $('#Hdn_ShowDiscount').val('Y');
    }
    else {
        $('#Hdn_ShowDiscount').val('N');
    }
}
function OnCheckedChangeShipTo() {
    if ($('#chkshowShipTo').prop('checked')) {
        $('#Hdn_ShowShipTo').val('Y');
    }
    else {
        $('#Hdn_ShowShipTo').val('N');
    }
}
function OnCheckedChangeBillTo() {
    if ($('#chkshowBillTo').prop('checked')) {
        $('#Hdn_ShowBillTo').val('Y');
    }
    else {
        $('#Hdn_ShowBillTo').val('N');
    }
}
function OnCheckedChangeBankDtl() {
    if ($('#chkshowBankDtl').prop('checked')) {
        $('#Hdn_ShowBankDetail').val('Y');
    }
    else {
        $('#Hdn_ShowBankDetail').val('N');
    }
}
function OnCheckedChangeCompAddrs() {
    if ($('#chkshowCompAddrs').prop('checked')) {
        $('#Hdn_ShowCompAddrs').val('Y');
    }
    else {
        $('#Hdn_ShowCompAddrs').val('N');
    }
}

function OnClickIconBtnLandedCost(e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var DocumentMenuId = $("#DocumentMenuId").val();
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#SQItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#SQItemListName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    let IsDisabled = $("#DisableSubItem").val();
    if (IsDisabled == "Y") {
        var landed_remarks = clickdRow.find("#LandedPrice_Remarks").text();
        if (landed_remarks != "" && landed_remarks != null) {
            $("#RR_Remarks").val(landed_remarks);
        }
        else {
            $("#RR_Remarks").val("");
        }
        $("#RR_Remarks").attr("readonly", true);
        $("#RR_btnClose").attr("hidden", false);
        $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", true);

    } else {
        var landed_remarks = clickdRow.find("#LandedPrice_Remarks").val();
        if (landed_remarks != "" && landed_remarks != null) {
            $("#RR_Remarks").val(landed_remarks);
        }
        else {
            $("#RR_Remarks").val("");
        }
        $("#RR_Remarks").attr("readonly", false);
        $("#RR_btnClose").attr("hidden", true);
        $("#RR_btnSaveAndExit,#RR_btnDiscardAndExit").attr("hidden", false);
    }

    $("#Heading").text($("#span_LandedPriceremarks").text());
    $("#RR_ItemName").val(ProductNm);
    $("#RR_ItemId").val(ProductId);
    $("#RR_Uom").val(UOM);
}

/****======------------Reason For Reject And Rework section Start-------------======****/
function onClick_RR_SaveAndExit() {
    debugger;
    var errorFlag = "N";
    let ItemId = $("#RR_ItemId").val();
    let RR_Remarks = $("#RR_Remarks").val();
    let CheckValid = CheckVallidation("RR_Remarks", "vmRR_Remarks");
    if (CheckValid == true) {
        $("#QTItmDetailsTbl tbody tr #hfItemID[value = '" + ItemId + "']").closest('tr').each(function () {
            let row = $(this);

            row.find("#LandedPrice_Remarks").text(RR_Remarks);
            row.find("#LandedPrice_Remarks").val(RR_Remarks);
            errorFlag = "Y";
        })

    }
    else {
        errorFlag = "N";
    }
    if (errorFlag == "Y") {
        $("#RR_btnSaveAndExit").attr("data-dismiss", "modal");
        // return true;
    }
    else {
        $("#RR_btnSaveAndExit").attr("data-dismiss", "");
        return false;
    }





}
function checkValidationRR_remarks() {
    CheckVallidation("RR_Remarks", "vmRR_Remarks");
}
/****======------------Reason For Reject And Rework section End-------------======****/

function OnChangeLandedCost(e) {
    let Row = $(e.target).closest('tr');
    var RateDigit = $("#RateDigit").text();
    var Landed_Cost = Row.find("#Landed_Cost").val();
    if (Landed_Cost != "" && Landed_Cost != "0" && parseFloat(Landed_Cost).toFixed(RateDigit) != parseFloat(0).toFixed(RateDigit)) {
        var landedPrice = parseFloat(Landed_Cost).toFixed(RateDigit)
        Row.find("#Landed_Cost").val(landedPrice);
    }
    else {
        Row.find("#Landed_Cost").val("");
    }
    CalculateTotalLandedCost();
}
function CalculateTotalLandedCost() {
    /* Added by Nitesh 26052025 Add Total Landed Cost */
    //debugger;

    var DocumentMenuId = $("#DocumentMenuId").val();
    var DecDigit = (DocumentMenuId == "105103145105") ? $("#ExpImpValDigit").text() : $("#ValDigit").text();

    var GrandTotalLandedCost = 0;

    $("#QTItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);

        var qty = parseFloat(CheckNullNumber( currentRow.find("#QuotQty").val())) || 0;
        var landedCost = parseFloat(CheckNullNumber(currentRow.find("#Landed_Cost").val())) || 0;

        var rowLandedTotal = qty * landedCost;
        GrandTotalLandedCost += rowLandedTotal;
    });

    var GrossValue = parseFloat($("#TxtGrossValue").val()) || 0;

    var MarginAmount = 0;
    var MarginPercentage = 0;

    if (GrossValue > 0) {
        MarginAmount = GrossValue - GrandTotalLandedCost;
        MarginPercentage = (MarginAmount / GrossValue) * 100;
    }

    // Round off values
    GrandTotalLandedCost = GrandTotalLandedCost.toFixed(DecDigit);
    MarginAmount = MarginAmount.toFixed(DecDigit);
    MarginPercentage = MarginPercentage.toFixed(DecDigit); // % usually with 2 decimal places

    // Output results
    $("#TotalLandedCost").val(GrandTotalLandedCost);
    $("#MarginPercentage").val(`${MarginAmount} (${MarginPercentage} %)`);
}
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "QTItmDetailsTbl", [{ "FieldId": "SQItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
function Checked_RaiseOrder() {/*add by Hina sharma on 13-06-2025*/
    debugger;
    if ($("#tgl_RaiseOrder").is(":checked")) {
        $("#Cancelled").attr("disabled", true);
        $("#hdn_RaiseOrder").val("Y");
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertSQTDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%)" });

    } else {
        $("#Cancelled").attr("disabled", false);
        $("#hdn_RaiseOrder").val("N");
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%)" });
       
    }
}
//-------------------------External Email Alert ------------------------------------
//Added by Nidhi on 01-08-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Cust_id = $("#Customer_Name option:selected").val();
    Cmn_SendEmail(docid, Cust_id, 'Cust');
}
//Added by Nidhi on 01-08-2025
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SQNo = $("#qt_no").val();
    var SQDate = $("#qt_date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowItemAliasName: $("#Hdn_ShowItemAliasName").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            ShowReferenceNo: $("#Hdn_ShowReferenceNo").val(),
            ShowCatalogueNo: $("#Hdn_ShowCatalogueNo").val(),
            ShowOEMNo: $("#Hdn_ShowOEMNo").val(),
            CustAliasName: $("#CustomerAliasName").val(),
            ShowHsnNumber: $("#Hdn_ShowHSNCode").val(),
            ShowDiscount: $("#Hdn_ShowDiscount").val(),
            ShowShipTo: $("#Hdn_ShowShipTo").val(),
            ShowBillTo: $("#Hdn_ShowBillTo").val(),
            ShowBankDetail: $("#Hdn_ShowBankDetail").val(),
            ShowPrintImage: $("#Hdn_ShowPrintImage").val(),
            PrintRemarks: $("#PrintRemarks").val(),
            ShowCompAddress: $("#Hdn_ShowCompAddrs").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SQ_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DomesticSalesQuotation/SavePdfDocToSendOnEmailAlert_Ext",
                data: { SQNo: SQNo, SQDate: SQDate, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, SQNo, SQDate, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, status, docid, SQNo, SQDate, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, status, docid, SQNo, SQDate);
    }
}
//Added by Nidhi on 02-08-2025
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SQNo = $("#qt_no").val();
    var SQDate = $("#qt_date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, SQNo, SQDate, statusAM, "/ApplicationLayer/DomesticSalesQuotation/SendEmailAlert", filepath)
}
function EmailAlertLogDetails() {
    debugger;
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SQNo = $("#qt_no").val();
    var SQDate = $("#qt_date").val();
    Cmn_EmailAlertLogDetails(status, docid, SQNo, SQDate)
}
//Added by Nidhi on 14-08-2025
function PrintFormate() {
    debugger;
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SQNo = $("#qt_no").val();
    var SQDate = $("#qt_date").val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowItemAliasName: $("#Hdn_ShowItemAliasName").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            ShowReferenceNo: $("#Hdn_ShowReferenceNo").val(),
            ShowCatalogueNo: $("#Hdn_ShowCatalogueNo").val(),
            ShowOEMNo: $("#Hdn_ShowOEMNo").val(),
            CustAliasName: $("#CustomerAliasName").val(),
            ShowHsnNumber: $("#Hdn_ShowHSNCode").val(),
            ShowDiscount: $("#Hdn_ShowDiscount").val(),
            ShowShipTo: $("#Hdn_ShowShipTo").val(),
            ShowBillTo: $("#Hdn_ShowBillTo").val(),
            ShowBankDetail: $("#Hdn_ShowBankDetail").val(),
            ShowPrintImage: $("#Hdn_ShowPrintImage").val(),
            PrintRemarks: $("#PrintRemarks").val(),
            ShowCompAddress: $("#Hdn_ShowCompAddrs").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SQ_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DomesticSalesQuotation/SavePdfDocToSendOnEmailAlert_Ext",
            data: { SQNo: SQNo, SQDate: SQDate, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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
//-------------------------External Email Alert End ------------------------------------

