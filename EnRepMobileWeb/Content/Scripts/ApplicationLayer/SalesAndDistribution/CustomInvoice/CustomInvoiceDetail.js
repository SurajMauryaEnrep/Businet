/************************************************
Javascript Name:Custom Invoice Detail
Created By:Hina Sharma
Created Date: 07-10-2022
Description: This Javascript use for the Domestic Sales Invoice Detail many function

Modified By:
Modified Date:
Description:

*************************************************/
var RateDecDigit = $("#ExpImpRateDigit").text();
var ValDecDigit = $("#ExpImpValDigit").text();
var QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity

$(document).ready(function () {

    
    debugger;
    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    var CustomInvDate = $("#CustomInvDate").val();
    if (CustomInvDate == "" || CustomInvDate == "0" || CustomInvDate == null) {
        $("#CustomInvDate").val(today);
    }
    BindCustomerList();
    BindDDlSIList();

    //BindPlOfReceiptByPreCarrier();
    var Cust_id = $('#CustomerName')[0].value;
    //BindShipmentList(Cust_id);  
    $("#SpanCustNameErrorMsg").css("display", "none");
    $("#CustomerName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
    var InvoiceNumber = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvoiceNumber);
    $("#hdnVouMsg").val($("#JVRaisedAgainstCustomInvoice").text());
    $("#hdnVouMsgOnC").val($("#JVRaisedAgainstCustomInvoice").text());
    CalculateAmount();
    if ($("#nontaxable").is(":checked")) {
        $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
            var currentRow = $(this);
            currentRow.find("#TaxExempted").attr("disabled", true);
            currentRow.find("#ManualGST").attr("disabled", true);
            currentRow.find("#BtnTxtCalculation").prop("disabled", true);
            currentRow.find('#ManualGST').prop("checked", false);
        });
    }
    BindPortOfLoading();
});
function OnchangePlOfReceiptByPreCarrier() {
    $("#Span_pi_rcpt_carr").css("display", "none");
    $("[aria-labelledby='select2-PlOfReceiptByPreCarrier-container']").css("border-color", "#ced4da");
}
function BindPlOfReceiptByPreCarrier() {
    debugger;
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/Shipment/GetPlOfReceiptByPreCarrierList",
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.length > 0) {
                $("#PlOfReceiptByPreCarrier option").remove();
                $("#PlOfReceiptByPreCarrier optgroup").remove();
                $("#PlOfReceiptByPreCarrier").attr("onchange", "OnchangePlOfReceiptByPreCarrier()");
                $('#PlOfReceiptByPreCarrier').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#span_PortOfLoading").text()}' data-date='${$("#span_State").text()}'></optgroup>`);
                for (var i = 0; i < arr.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr[i].state_name}" value="${arr[i].port_id}">${arr[i].port_desc}</option>`);
                }
            }
            var firstEmptySelect = true;
            $('#PlOfReceiptByPreCarrier').select2({
                templateResult: function (data) {
                    var DocDate = $(data.element).data('date');
                    var classAttr = $(data.element).attr('class');
                    var hasClass = typeof classAttr != 'undefined';
                    classAttr = hasClass ? ' ' + classAttr : '';
                    var $result = $(
                        '<div class="row">' +
                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                        '</div>'
                    );
                    return $result;
                    firstEmptySelect = false;
                }
            });
            var Hdnloading_port = $("#Hdnpi_rcpt_carr").val();
            if (Hdnloading_port != "") {
                $('#PlOfReceiptByPreCarrier').val(Hdnloading_port).trigger('change')
            }
        },
    })
}
function BindPortOfLoading() {
    debugger;
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/Shipment/GetPortOfLoadingList",
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.length > 0) {
                $("#PortOfLoading option").remove();
                $("#PortOfLoading optgroup").remove();
                $("#PortOfLoading").attr("onchange", "OnchangePortOfLoading()");
                $('#PortOfLoading').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#span_PortOfLoading").text()}' data-date='${$("#span_State").text()}'></optgroup>`);
                for (var i = 0; i < arr.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr[i].state_name}" value="${arr[i].port_id}">${arr[i].port_desc}</option>`);
                }
            }
            var firstEmptySelect = true;
            $('#PortOfLoading').select2({
                templateResult: function (data) {
                    var DocDate = $(data.element).data('date');
                    var classAttr = $(data.element).attr('class');
                    var hasClass = typeof classAttr != 'undefined';
                    classAttr = hasClass ? ' ' + classAttr : '';
                    var $result = $(
                        '<div class="row pol">' +
                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                        '</div>'
                    );
                    return $result;
                    firstEmptySelect = false;
                }
            });
            var Hdnloading_port = $("#Hdnloading_port").val();
            if (Hdnloading_port != "") {
                $('#PortOfLoading').val(Hdnloading_port).trigger('change')
            }
            BindPlOfReceiptByPreCarrier();
        },
    })
}
function OnchangePortOfLoading() {
    debugger;
    var PortOfLoading = $("#PortOfLoading option:selected").text();
    $("#Span_loading_port").css("display", "none");
    $("[aria-labelledby='select2-PortOfLoading-container']").css("border-color", "#ced4da");
}
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertSIDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function CheckedNontaxable() {
    debugger;
    try {
        showLoader();
        var totalItems = $("#SInvItmDetailsTbl > tbody > tr ").length;
        if (totalItems == 0) {
            hideLoader();
        }
        var ConvRate = $("#conv_rate").val();
        if ($("#nontaxable").is(":checked")) {
            let i = 1;
            
            $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
                var currentRow = $(this);
                currentRow.find("#TaxExempted").prop("checked", true);
                //if (totalItems == i) {
                //    OnClickTaxExemptedCheckBox("", currentRow).then(() => {
                //        GetAllGLID().then(() => {
                //            hideLoader();
                //        });
                //    });
                //} else {
                //    OnClickTaxExemptedCheckBox("", currentRow, "NoApply");
                //}

                currentRow.find("#TaxExempted").attr("disabled", true);
                currentRow.find("#ManualGST").attr("disabled", true);
                currentRow.find("#BtnTxtCalculation").prop("disabled", true);
                currentRow.find('#ManualGST').prop("checked", false);

                //--------------------------------------
                var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                var GrossAmtOR = parseFloat(currentRow.find("#TxtAssessableValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                currentRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff((parseFloat(GrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#InvoiceAmount").val(parseFloat(GrossAmtOR).toFixed(ValDecDigit));
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
            $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
                var currentRow = $(this);
                currentRow.find("#TaxExempted").prop("checked", false);
                //if (totalItems == i) {
                //    OnClickTaxExemptedCheckBox("", currentRow).then(() => {
                //        GetAllGLID().then(() => {
                //            hideLoader();
                //        });
                //    });
                //} else {
                //    OnClickTaxExemptedCheckBox("", currentRow, "NoApply");
                //}
                currentRow.find("#TaxExempted").attr("disabled", false);
                currentRow.find("#ManualGST").attr("disabled", false);
                currentRow.find("#BtnTxtCalculation").prop("disabled", false);
                //var GstApplicable = $("#Hdn_GstApplicable").text();
                //if (GstApplicable == "Y") {
                //    var gst_number = $("#Ship_Gst_number").val()
                //    Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValueinSpec", "", "")
                //}
                //else {
                //    CalculateTaxExemptedAmt(e)
                //    GetAllGLID();
                //};
                i++;
            });
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                var gst_number = $("#Ship_Gst_number").val()
                Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", "", "", null).then(() => {
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
    
    //triggerTaxApply();
}
function triggerTaxApply() {
    if ($("#nontaxable").is(":checked")) {
        $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
            var currentRow = $(this);
            //currentRow.find("#TaxExempted").prop("checked", true);
            currentRow.find("#TaxExempted").trigger('click');
        });
    }
}
/*-------------List Page Functionality----------------- */

function BindCustomerList() {
    $("#CustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPage: params.page,
                    CustType: "E"
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    //SI_ErrorPage();
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
function BtnSearch() {
    debugger;
    FilterSI_List();
    ResetWF_Level()
}
function FilterSI_List() {
    debugger;
    try {
        var CustId = $("#CustomerName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CustomInvoice/SearchSI_Detail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodySIList').html(data);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("SI Error : " + err.message);

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
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}

/*-------------List Page Functionality End----------------- */

function OnChangeAssRate(e) {
    debugger;
    var CRow = $(e.target).closest("tr");
    let conv_rate = $("#conv_rate").val();
    let AssRate = CheckNullNumber(CRow.find("#TxtAssRate").val());
    let RecQty = CRow.find("#TxtReceivedQuantity").val();
    let AssValSp = parseFloat(getvalwithoutroundoff(cmn_getmultival(parseFloat(AssRate), parseFloat(CheckNullNumber(RecQty))), ValDecDigit)).toFixed(ValDecDigit);
    let AssValBase = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(CheckNullNumber(AssValSp)), parseFloat(CheckNullNumber(conv_rate)))), ValDecDigit)).toFixed(ValDecDigit);

    CRow.find("#TxtAssRate").val(parseFloat(getvalwithoutroundoff(AssRate, RateDecDigit)).toFixed(RateDecDigit));
    if (parseFloat(CheckNullNumber(AssRate)) > 0) {
        CRow.find("#TxtAssRate").css("border-color", "#ced4da");
        CRow.find("#SpanTxtAssRateErrorMsg").css("display", "none");
        CRow.find("#SpanTxtAssRateErrorMsg").text("");
        if (parseFloat(CheckNullNumber(AssValSp)) > 0) {
            CRow.find("#TxtAssessableValueinSpec").css("border-color", "#ced4da");
            CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").css("display", "none");
            CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").text("");
        }

        CRow.find("#TxtAssessableValue").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        //CRow.find("#TxtItemGrossValueInBase").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#TxtAssessableValueinSpec").val(parseFloat(getvalwithoutroundoff(AssValSp, ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#InvoiceAmount").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff(AssValSp, ValDecDigit)).toFixed(ValDecDigit));
        //let TxtAssRate = CRow.find("#TxtAssRate").val();
        CalculateAmount();
        if (CRow.find("#ManualGST").is(":checked")) {
            var item_tax_amt = CRow.find("#Txtitem_tax_amt").val()
            if (parseFloat(CheckNullNumber(item_tax_amt)) > 0) {
                ResetManualGST(CRow);
            }
        } else {
            OnClickTaxExemptedCheckBox(e);
        }
    } else {
        CRow.find("#TxtAssRate").css("border-color","red");
        CRow.find("#SpanTxtAssRateErrorMsg").css("display","block");
        CRow.find("#SpanTxtAssRateErrorMsg").text($("#valueReq").text());
        return false;
    }
   
}
function ResetManualGST(clickedrow) {
    Sno = "";
    var ItemID = clickedrow.find("#hfItemID").val();
    var AssValue = clickedrow.find("#TxtAssessableValue").val();
    $("#HdnTaxOn").val("Item");
    $("#Tax_ItemID").val(ItemID);
    $("#Tax_AssessableValue").val(AssValue);
    $("#HiddenRowSNo").val(Sno);
    $("#TaxCalcItemCode").val(ItemID);
    var rowIdx = 0;
    $("#TaxCalculatorTbl > tbody > tr").remove();
    $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
        var Crow = $(this);
        var TaxTLevel = Crow.find("#TaxLevel").text();
        var TaxItemID = Crow.find("#TaxItmCode").text();
        var QTnumber = Crow.find("#DocNo").text();
        var QTdate = Crow.find("#DocDate").text();
        var Acc_ID = Crow.find("#Acc_ID").text();
        var Acc_name = Crow.find("#Acc_name").text();
        if (ItemID == TaxItemID) {
            var TaxName = Crow.find("#TaxName").text();
            var TaxNameID = Crow.find("#TaxNameID").text();
            var TaxPercentage = Crow.find("#TaxPercentage").text();
            var TaxLevel = Crow.find("#TaxLevel").text();
            var TaxApplyOn = Crow.find("#TaxApplyOn").text();

            var TaxAmount = (CheckNullNumber(AssValue) * TaxPercentage.split("%")[0]) / 100;//Crow.find("#TaxAmount").text();
            var TaxApplyOnID = Crow.find("#TaxApplyOnID").text();
            var TotalTaxAmount = parseFloat(Crow.find("#TotalTaxAmount").text()) + (TaxAmount - Crow.find("#TaxAmount").text());
            var Acc_ID = Crow.find("#Acc_ID").text();
            var Acc_name = Crow.find("#Acc_name").text();
            var TaxRecov = "";
            TaxTableDataBind(++rowIdx,
                TaxName, TaxNameID, TaxPercentage, TaxLevel, TaxApplyOn, TaxAmount, TaxApplyOnID, TotalTaxAmount, Acc_ID, Acc_name, TaxRecov
            )
        }
        
    });
    OnClickSaveAndExit('MGST');
}
function OnchangeCurrencyConvRateCalculate() {
    $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
        var CRow = $(this);
        let conv_rate = $("#conv_rate").val();
        let itemId = CRow.find("#hfItemID").val();
        let AssRate = CRow.find("#TxtAssRate").val();
        let ItmGrVal = CRow.find("#TxtItemGrossValue").val();
        let RecQty = CRow.find("#TxtReceivedQuantity").val();
        let AssValSp = getvalwithoutroundoff(cmn_getmultival(AssRate, RecQty), ValDecDigit);
        let AssValBase = getvalwithoutroundoff(cmn_getmultival(AssValSp, conv_rate), ValDecDigit);


        CRow.find("#TxtItemGrossValueInBase").val(parseFloat(getvalwithoutroundoff(cmn_getmultival(ItmGrVal, conv_rate), ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#TxtAssRate").val(parseFloat(AssRate).toFixed(RateDecDigit));
        CRow.find("#TxtAssessableValue").val(parseFloat(AssValBase).toFixed(ValDecDigit));
        CRow.find("#TxtAssessableValueinSpec").val(parseFloat(AssValSp).toFixed(ValDecDigit));


        if (CRow.find("#ManualGST").is(":checked") == true) {

            $("#TaxCalculatorTbl tbody tr").remove();
            $("#Tax_AssessableValue").val(parseFloat(AssValBase).toFixed(ValDecDigit));

            let total = 0;
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + itemId + "')").closest("tr").each(function () {
                var CurrRow = $(this);
                let taxname = CurrRow.find("#TaxName").text();
                let taxid = CurrRow.find("#TaxNameID").text();
                let taxrate = CurrRow.find("#TaxPercentage").text();
                let taxlevel = CurrRow.find("#TaxLevel").text();
                let taxapplyonname = CurrRow.find("#TaxApplyOn").text();
                let taxval = CurrRow.find("#TaxAmount").text();
                let TotalTaxAmount = CurrRow.find("#TotalTaxAmount").text();
                let taxapplyon = CurrRow.find("#TaxApplyOnID").text();

                //let perct = taxrate.split('.')[0];
                let perct = taxrate.split('%')[0];
                taxval = getvalwithoutroundoff(cmn_getmultival(parseFloat(AssValBase).toFixed(ValDecDigit), perct), ValDecDigit) / 100;
                total = cmn_getsumval(total, taxval, ValDecDigit);
                TaxTableDataBind(1, taxname, taxid, taxrate, taxlevel, taxapplyonname, taxval, taxapplyon, total, "", "")
            });
            $("#TaxCalcItemCode").val(itemId)
            $('#TotalTaxAmount').text(total);
            $("#TaxCalcGRNNo").val("");
            $("#TaxCalcGRNDate").val("");
            //OnClickSaveAndExit('MGST');
            OnClickTaxSaveAndExit();
        }
    });

    $("#SInvItmDetailsTbl > tbody > tr:eq(0) #TxtAssRate ").change();
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
function BindDDlSIList() {
    $("#CustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPage: params.page,
                    CustType: "D"
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    SI_ErrorPage();
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
function InsertSIDetail() {
    debugger;
    
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckSI_Validations() == false) {
        return false;
    }
    if (CheckSI_ItemValidations() == false) {
        return false;
    }
    /* Code Wrote by sanjay prasad on 2024-05-20 (base amount(ass , tax, total) validation)*/
    if (ValidateAmountBeforeSubmit() == false) {
        return false;
    }
    /*------------------------end---------------------------------*/
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (Cmn_taxVallidation("SInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
            return false;
        }
    }
    var CheckTaxAmount = parseFloat(CheckNullNumber($("#TxtTaxAmount").val()));
    if (CheckTaxAmount > 0) {
        if (CheckSI_VoucherValidations() == false) {
            return false;
        }
    }

    debugger;
    var Flag = "N";
    var POL = $("#PortOfLoading option:selected").val();
    var PI = $("#PlOfReceiptByPreCarrier option:selected").val();

    if (POL == "0" || POL == "" || POL == "---Select---") {
        $("#Span_loading_port").text($("#valueReq").text());
        $("#Span_loading_port").css("display", "block");
        $("[aria-labelledby='select2-PortOfLoading-container']").css("border-color", "red");
    }
    if (PI == "0" || PI == "") {
        $("#Span_pi_rcpt_carr").text($("#valueReq").text());
        $("#Span_pi_rcpt_carr").css("display", "block");
        $("[aria-labelledby='select2-PlOfReceiptByPreCarrier-container']").css("border-color", "red");
        Flag = "Y";
    }
    if (Flag == "Y") {
        $("#collapseSix").addClass("show");
        return false;
    }
    debugger;
    var TransType = "";
    if (INSDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    debugger;
    $("#hdnVouMsg").val($("#JVRaisedAgainstCustomInvoice").text());

    var FinalSI_ItemDetail = [];
    var FinalSI_VouDetail = [];

    FinalSI_ItemDetail = GetSI_ItemDetails();
    FinalSI_VouDetail = GetSI_VoucherDetails();
    FinalSI_OCDetail = GetCstmInv_OcDetails();
    debugger;
    var SIItemDt = JSON.stringify(FinalSI_ItemDetail);
    var SIVouGlDt = JSON.stringify(FinalSI_VouDetail);
    var OCDt = JSON.stringify(FinalSI_OCDetail);

    $('#hdItemDetailList').val(SIItemDt);
    $('#hdVouGlDetailList').val(SIVouGlDt);
    $('#hdOCDetailList').val(OCDt);
    GetSI_TaxDetails();
    //GetSI_OCTaxDetails();
    // GetSI_OtherChargeDetails();

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/

    var Custname = $('#CustomerName option:selected').text();
    $("#hdCInvcust_name").val(Custname);
    //var Custname = $('#CustomerName option:selected').text();
    //$("#hdCInvcust_name").val(Custname);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;

};
function CheckSI_Validations() {
    debugger;
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
        $("#CustomerName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
    }
    if ($("#conv_rate").val() === "0" || $("#conv_rate").val() === "") {
        $('#SpanExRateErrorMsg').text($("#valueReq").text());
        $("#conv_rate").css("border-color", "Red");
        $("SpanExRateErrorMsg").css("border-color", "red");
        $("#SpanExRateErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
        $("SpanExRateErrorMsg").css("border-color", "#ced4da");
    }
    if ($("#ddlSalesPerson").val() === "0" || $("#ddlSalesPerson").val() === "" || $("#ddlSalesPerson").val() === null) {
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $("#ddlSalesPerson").css("border-color", "red");
        /*$('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "Red");*/
        $("#SpanSalesPersonErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckSI_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#SInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            var row = $(this);
            var ass_price = row.find("#TxtAssRate").val();
            if (parseFloat(CheckNullNumber(ass_price)) > 0) {
                row.find("#TxtAssRate").css("border-color", "#ced4da");
                row.find("#SpanTxtAssRateErrorMsg").css("display", "none");
                row.find("#SpanTxtAssRateErrorMsg").text("");
            } else {
                row.find("#TxtAssRate").css("border-color", "red");
                row.find("#SpanTxtAssRateErrorMsg").css("display", "block");
                row.find("#SpanTxtAssRateErrorMsg").text($("#valueReq").text());
                ErrorFlag = "Y";
            }
            var ass_value = row.find("#TxtAssessableValueinSpec").val();
            if (parseFloat(CheckNullNumber(ass_value)) > 0) {
                row.find("#TxtAssessableValueinSpec").css("border-color", "#ced4da");
                row.find("#SpanTxtAssessableValueinSpecErrorMsg").css("display", "none");
                row.find("#SpanTxtAssessableValueinSpecErrorMsg").text("");
            } else {
                row.find("#TxtAssessableValueinSpec").css("border-color", "red");
                row.find("#SpanTxtAssessableValueinSpecErrorMsg").css("display", "block");
                row.find("#SpanTxtAssessableValueinSpecErrorMsg").text($("#valueReq").text());
                ErrorFlag = "Y";
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
function CheckSI_VoucherValidations() {
    debugger;
    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 19-07-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 19-07-2024 to add new common gl validations*/
    //var ErrorFlag = "N";
    //var DrTotal = $("#DrTotal").text();
    //var CrTotal = $("#CrTotal").text();
    //debugger;
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}
    //debugger;
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDecDigit) && CrTotal == parseFloat(0).toFixed(ValDecDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //$("#VoucherDetail tbody tr").each(function () {
    //    var currentrow = $(this);
    //    var acc_id = currentrow.find("#hfItemID").val();
    //    if (acc_id == "" && acc_id == Number) {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //    }
    //});
    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
}

function GetSI_ItemDetails() {
    var SI_ItemsDetail = [];
    $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var item_id = "";
        var uom_id = "";
        var item_name = "";
        var uom_name = "";
        var sub_item = "";
        var ship_qty = "";
        var item_rate = "";
        var item_gr_val_sp = "";
        var item_gr_val_bs = "";
        var item_tax_amt = "";
        var item_inv_amt_bs = "";
        var TaxExempted = "";
        var ManualGST = "";
        var hsn_code = "";


        var currentRow = $(this);
        item_id = currentRow.find("#hfItemID").val();
        uom_id = currentRow.find("#hfUOMID").val();
        item_name = currentRow.find('#TxtItemName').val();
        uom_name = currentRow.find('#TxtUOM').val();
        //sub_item = currentRow.find('#sub_item').val();
        sub_item = "";
        ship_qty = currentRow.find("#TxtReceivedQuantity").val();
        item_rate = currentRow.find("#TxtRate").val();
        item_gr_val_sp = currentRow.find("#TxtItemGrossValue").val();
        item_gr_val_bs = currentRow.find("#TxtItemGrossValueInBase").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        item_inv_amt_bs = currentRow.find("#InvoiceAmount").val();
        item_oc_amt_bs = currentRow.find("#TxtOtherCharge").val();
        item_ass_rate = currentRow.find("#TxtAssRate").val();
        item_ass_val_bs = currentRow.find("#TxtAssessableValue").val();
        item_ass_val_spec = currentRow.find("#TxtAssessableValueinSpec").val();
        item_inv_val_spec = currentRow.find("#InvoiceAmountInSpec").val();
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        /***Modifyed By Shubham Maurya on 27-10-2023 for manual Gst and Tax Exempted***/
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
        hsn_code = currentRow.find("#ItemHsnCode").val();
        var hsn_codes = $("#ItemHsnCode").val();
        //item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        //if (item_oc_amt == "" && item_oc_amt == null) {
        //    item_oc_amt = "0";
        //}

        SI_ItemsDetail.push({
            item_id: item_id, item_name: item_name, uom_id: uom_id, uom_name: uom_name, sub_item: sub_item, ship_qty: ship_qty
            , item_rate: item_rate, item_gr_val_sp: item_gr_val_sp, item_gr_val_bs: item_gr_val_bs, item_tax_amt: item_tax_amt
            , item_inv_amt_bs: item_inv_amt_bs, item_oc_amt_bs: item_oc_amt_bs, TaxExempted: TaxExempted, hsn_code: hsn_code
            , ManualGST: ManualGST, item_ass_rate: item_ass_rate, item_ass_val_bs: item_ass_val_bs, item_ass_val_spec: item_ass_val_spec
            , item_inv_val_spec: item_inv_val_spec
        });
    });
    return SI_ItemsDetail;
};

function GetCstmInv_OcDetails() {
    var Oc_Detail = [];
    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
        var oc_id = "";
        var oc_val = "";

        var currentRow = $(this);
        oc_id = currentRow.find("#OC_ID").text();
        oc_val = currentRow.find("#OCAmtBs").text();

        Oc_Detail.push({ oc_id: oc_id, oc_val: oc_val });
    });
    return Oc_Detail;
};

function GetSI_TaxDetails() {
    debugger;
    //var TaxDetails = [];
    var ItemTaxList = new Array();
    var taxrowcount = $('#Hdn_TaxCalculatorTbl tbody tr').length;
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var ItmCode = currentRow.find("#hfItemID").val();
            if (taxrowcount != null) {
                if (taxrowcount > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var TaxList = {};

                        TaxList.item_id = currentRow.find("#TaxItmCode").text().trim();//currentRow.find("#itemid").text();          
                        TaxList.tax_id = currentRow.find("#TaxNameID").text().trim();//currentRow.find("#taxid").text();
                        TaxList.tax_name = currentRow.find("#TaxName").text();

                        TaxList.tax_rate = currentRow.find("#TaxPercentage").text().trim().replace('%', '');//currentRow.find("#taxrate").text().replace('%', '');
                        TaxList.tax_level = currentRow.find("#TaxLevel").text().trim();//currentRow.find("#taxlevel").text();           
                        TaxList.tax_val = currentRow.find("#TaxAmount").text().trim();//currentRow.find("#taxval").text();            T;
                        TaxList.tax_apply_on = currentRow.find("#TaxApplyOnID").text().trim();// currentRow.find("#taxapplyon").text();
                        TaxList.tax_apply_name = currentRow.find("#TaxApplyOn").text();
                        TaxList.item_tax_amt = currentRow.find("#TotalTaxAmount").text();

                        ItemTaxList.push(TaxList);
                        //TaxDetails.push(TaxList)
                    });
                }
            }
        }
    });
    //if (taxrowcount > 0) {
    //    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
    //        debugger;
    //        var currentRow = $(this);
    //        var TaxList = {};

    //        TaxList.item_id = currentRow.find("#TaxItmCode").text().trim();//currentRow.find("#itemid").text();          
    //        TaxList.tax_id = currentRow.find("#TaxNameID").text().trim();//currentRow.find("#taxid").text();
    //        TaxList.tax_name = currentRow.find("#TaxName").text();

    //        TaxList.tax_rate = currentRow.find("#TaxPercentage").text().trim().replace('%', '');//currentRow.find("#taxrate").text().replace('%', '');
    //        TaxList.tax_level = currentRow.find("#TaxLevel").text().trim();//currentRow.find("#taxlevel").text();           
    //        TaxList.tax_val = currentRow.find("#TaxAmount").text().trim();//currentRow.find("#taxval").text();            T;
    //        TaxList.tax_apply_on = currentRow.find("#TaxApplyOnID").text().trim();// currentRow.find("#taxapplyon").text();
    //        TaxList.tax_apply_name = currentRow.find("#TaxApplyOn").text();
    //        TaxList.item_tax_amt = currentRow.find("#TotalTaxAmount").text();

    //        ItemTaxList.push(TaxList);
    //        TaxDetails.push(TaxList)
    //    });

    //}
    var str1 = JSON.stringify(ItemTaxList);
    $("#hdTaxDetailList").val(str1);
};

function GetSI_VoucherDetails() {
    debugger;
    var SI_VouList = [];
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "E";

    var TransType = "Sal";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var Gltype = "";
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#CInv_txthfAccID").text().trim();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            Gltype = currentRow.find("#type").val();
            SI_VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: _SIType, Value: CustVal, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, Gltype: Gltype });

        });
    }
    return SI_VouList;
};

function OnChangeCustomer(CustID) {
    debugger;
    var Cust_id = CustID.value;
    $("#hdn_Cust_id").val(Cust_id);
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
        $("#txtCurrency").val("");
        $("#conv_rate").val("");

    }
    else {
        var Custname = $('#CustomerName option:selected').text();
        $("#hdCInvcust_name").val(Custname);

        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
    //var Create_ID = $("#Create_ID").val();
    //if (Create_ID == "") {

    //}
    BindCurrAndAddress(Cust_id);
    BindShipmentList(Cust_id);

}
function BindCurrAndAddress(Cust_id) {

    try {

        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LSODetail/GetCustAddrDetail",
                data: {
                    Cust_id: Cust_id
                },
                success: function (data) {

                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            // var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#TxtBillingAddr").val(arr.Table[0].BillingAddress);
                            $("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            //$("#hdcurr").val(arr.Table[0].curr_id);
                            //$("#txtCurrency").val(arr.Table[0].curr_name);
                            //$("#ddlCurrency").html(s);
                            debugger;
                            //$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(RateDecDigit));
                            //$("#hdnconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(RateDecDigit));

                            /*$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#").text()));
                            if (Cust_type == "D") {
                                /*$("#conv_rate").prop("disabled", true);*/
                            $("#ConvRate").attr("readonly", true);
                        }
                        else {
                            $("#TxtBillingAddr").val("");
                            $("#TxtShippingAddr").val("");
                            $("#bill_add_id").val("");
                            $("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                            /*$("#conv_rate").prop("disabled", false);*/
                            //$("#ConvRate").attr("readonly", false);
                        }
                    }
                    else {
                        //var s= $("#hdcurr").append('<option value="0">---Select---</option>')
                        /*var s = '<option value="0">---Select---</option>';*/
                        //$("#ddlCurrency").append('<option value="0">---Select---</option>')
                        //$("#ddlCurrency").html(s);
                        //$("#txtCurrency").val("");
                        //$("#conv_rate").val("");
                    }
                }
            });

    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function BindShipmentList(Cust_id) {

    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CustomInvoice/GetShipmentLists",
            data: { Cust_id: Cust_id },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    SI_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $("#ddlShipmentNo option").remove();
                        $("#ddlShipmentNo optgroup").remove();
                        $('#ddlShipmentNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].ship_dt}" value="${arr.Table[i].ship_no}">${arr.Table[i].ship_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlShipmentNo').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });
                        debugger;
                        $(".plus_icon1").css("display", "block");
                        $("#SpanShipmentErrorMsg").css("display", "none");
                        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");
                        $("#Shipment_Date").val("");
                    }
                }
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
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
function OnChangeShipmentNo(Shipment_No) {
    debugger;
    var ExchDecDigit = $("#ExchDigit").text();
    var ShipmentNo = Shipment_No.value;
    $("#hd_ship_no_id").val(ShipmentNo);
    var ShipmentDate = $('#ddlShipmentNo').select2("data")[0].element.attributes[0].value;
    var ShipmentDP = ShipmentDate.split("-");
    var FShipmentDate = (ShipmentDP[2] + "-" + ShipmentDP[1] + "-" + ShipmentDP[0]);
    if (ShipmentNo == "---Select---") {
        $("#Shipment_Date").val("");
    }
    else {
        $("#Shipment_Date").val(FShipmentDate);
        $("#SpanShipmentErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");

        try {
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/CustomInvoice/Getcurr_details",
                    data: {
                        ship_no: ShipmentNo,
                        ship_date: FShipmentDate
                    },
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {

                                $("#hdcurr").val(arr[0].curr_id);
                                $("#txtCurrency").val(arr[0].curr_name);
                                $("#conv_rate").val(parseFloat(arr[0].conv_rate).toFixed(ExchDecDigit));
                                $("#hdnconv_rate").val(parseFloat(arr[0].conv_rate).toFixed(ExchDecDigit));
                                $("#conv_rate").attr("readonly", true);
                            }
                            else {
                                $("#hdcurr").val("");
                                $("#txtCurrency").val("");
                                $("#conv_rate").val("");
                                $("#hdnconv_rate").val("");
                                $("#conv_rate").attr("readonly", true);
                            }
                        }
                        else {
                            $("#hdcurr").val("");
                            $("#txtCurrency").val("");
                            $("#conv_rate").val("");
                            $("#hdnconv_rate").val("");
                            $("#conv_rate").attr("readonly", true);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        debugger;
                    }
                });

        } catch (err) {
            console.log("UserValidate Error : " + err.message);
        }
    }
}
function OnClickAddButton() {
    //debugger;
    var ConvRate;
    //ConvRate = $("#conv_rate").val();
    var Inv_type = "E";
    var ExchDecDigit = $("#ExchDigit").text();
    var ShipmentNo = $('#ddlShipmentNo').val();
    var ShipmentDate = $('#Shipment_Date').val();
    if (ShipmentNo == "---Select---" || ShipmentNo == "0") {
        $('#SpanShipmentErrorMsg').text($("#valueReq").text());
        $("#SpanShipmentErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "red");
        $("#ddlShipmentNo").css("border-color", "red");
    }
    else {
        $("#SpanShipmentErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");
        $("#CustomerName").attr("disabled", "disabled");
        $("#ddlShipmentNo").css("border-color", "#ced4da");
        $("#conv_rate").attr("disabled", "disabled");
        if (Inv_type == "E") {
            $("#ddlShipmentNo").attr("disabled", true);
            $("#Shipment_AddBtn").css("display", "none");
        }
        let startTime = moment();
        try {
            showLoader();
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/CustomInvoice/GetShipmentDetails",
                data: { ShipmentNo: ShipmentNo, ShipmentDate: ShipmentDate },
                success: function (data) {
                    debugger;
                    try {
                        let completeTime = moment();
                        duration = moment.duration(completeTime.diff(startTime));
                        console.log("Responce : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                        if (data == 'ErrorPage') {
                            SI_ErrorPage();
                            return false;
                        }

                        if (data !== null && data !== "") {
                            var arr = [];
                            var DocMenuId = $("#DocumentMenuId").val();
                            arr = JSON.parse(data);

                            $("#custom_inv_no").val(arr.Table[0].custom_inv_no);
                            $("#CustomInvDate").val(arr.Table[0].custom_inv_dt);
                            if (arr.Table2.length > 0) {
                                $("#conv_rate").val(parseFloat(arr.Table2[0].conv_rate).toFixed(ExchDecDigit));
                                $("#hdnconv_rate").val(parseFloat(arr.Table2[0].conv_rate).toFixed(ExchDecDigit));
                            }
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            var ManualGst = "";
                            var TaxExempted = "";
                            if (GstApplicable == "Y") {
                                ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                            }
                            TaxExempted = ' <td><div class="custom-control custom-switch sample_issue"><input type="checkbox"  class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                            ConvRate = $("#hdnconv_rate").val();
                            if (arr.Table.length > 0) {
                                var rowIdx = 0;
                                let totalGrVal = arr.Table[0].net_val;
                                for (var k = 0; k < arr.Table.length; k++) {
                                    //debugger;
                                    var BaseVal;
                                    BaseVal = getvalwithoutroundoff(cmn_getmultival(parseFloat(ConvRate).toFixed(ValDecDigit), parseFloat(getvalwithoutroundoff(arr.Table[k].item_gross_val, ValDecDigit)).toFixed(ValDecDigit)), ValDecDigit)
                                    //let ItemAssVal = parseFloat(BaseVal) + parseFloat(getvalwithoutroundoff(arr.Table[k].item_oc_amt_bs,ValDecDigit));
                                    let ItemAssVal = parseFloat(getvalwithoutroundoff(arr.Table[k].item_ass_val_bs, ValDecDigit));
                                    //totalGrVal = parseFloat(totalGrVal) + parseFloat(arr.Table[k].item_gross_val) + parseFloat(arr.Table[k].item_oc_amt_bs);
                                    //let BaseInvoiceVal = ItemAssVal;

                                    /*Commented by Suraj on 23-07-2024. Reason: now this calculation is coming from database*/
                                    //let ItemAssValSpec = (parseFloat(ItemAssVal) / parseFloat(ConvRate));
                                    //let assRate = parseFloat(arr.Table[k].item_oc_amt_bs) == 0 ? getvalwithoutroundoff(arr.Table[k].item_rate, ValDecDigit):parseFloat(getvalwithoutroundoff((parseFloat(ItemAssValSpec) / parseFloat(arr.Table[k].ship_qty)), RateDecDigit)).toFixed(RateDecDigit);

                                    //let NewAssVal = parseFloat(getvalwithoutroundoff((cmn_getmultival(getvalwithoutroundoff(cmn_getmultival(assRate, arr.Table[k].ship_qty), ValDecDigit), ConvRate)), ValDecDigit)).toFixed(ValDecDigit);
                                    //let finAssVal = ItemAssVal - (ItemAssVal - NewAssVal);
                                    let disabled = "";
                                    let paramOcIncludeToRate = $("#spnRateIncludingOC").text();
                                    let ocHidden = "hidden";
                                    if (paramOcIncludeToRate == "N") {
                                        disabled = "disabled";
                                        ocHidden = "";
                                    }

                                    var S_NO = $('#SInvItmDetailsTbl tbody tr').length + 1;
                                    $('#SInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${S_NO}</td>
                                    <td style="display: none;"><input type="hidden" id="SNohiddenfiled" value="${S_NO}"/></td>
                                    <td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-10 no-padding"><input id="TxtItemName" class="form-control time" type="text" name="" disabled><input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" /></div>
                                    <div class=" col-sm-1 i_Icon">
                                    <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                                    <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div></td>
                                    <td><input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value="${arr.Table[k].uom_alias}" disabled><input  type="hidden" id="hfUOMID" value="${arr.Table[k].uom_id}" /><input type="hidden" id="ItemHsnCode" value="${arr.Table[k].hsn_code}" /></td>
                                    <td><input id="TxtReceivedQuantity" class="form-control num_right" type="text" name="ReceivedQuantity" value="${parseFloat(arr.Table[k].ship_qty).toFixed(QtyDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td><input id="TxtRate" class="form-control num_right" type="text" name="Rate" placeholder="0000:00" value="${parseFloat(arr.Table[k].item_rate).toFixed(RateDecDigit)}" disabled></td>
                                    <td>
                                        <div class="lpo_form col-sm-12 no-padding">
                                            <input id="TxtAssRate" ${disabled} class="form-control num_right" type="text" name="AssRate" onchange="OnChangeAssRate(event)" onkeypress="return RateFloatValueonly(this,event)" placeholder="0000:00" value="${parseFloat(arr.Table[k].ass_rate).toFixed(RateDecDigit)}" >
                                        <span id="SpanTxtAssRateErrorMsg" class="error-message is-visible"></span>
                                        </div>
                                    </td>
                                    <td><input id="TxtItemGrossValue" class="form-control num_right" type="text" name="NetValue" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_gross_val, ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td><input id="TxtItemGrossValueInBase" class="form-control num_right" type="text" name="NetValueInBase" value="${parseFloat(arr.Table[k].item_gross_val_bs).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td ${ocHidden}>
                                        <input id="TxtOtherCharge" class="form-control num_right" autocomplete="off" type="text" name="OtherCharge" value="${parseFloat(arr.Table[k].item_oc_amt_bs).toFixed(ValDecDigit)}" placeholder="0000:00" onblur="this.placeholder='0000:00'" disabled>
                                        <input hidden id="Txt" class="form-control num_right" autocomplete="off" type="text" name="OtherCharge" value="${parseFloat(arr.Table[k].item_oc_amt_bs).toFixed(ValDecDigit)}" placeholder="0000:00" onblur="this.placeholder='0000:00'" disabled>
                                    </td>
                                    <td >
                                        <div class="lpo_form col-sm-12 no-padding">
                                          <input id="TxtAssessableValueinSpec" class="form-control num_right" onchange="OnChangeAssValueSpec(event)" onkeypress="return AmtFloatValueonly(this,event)" autocomplete="off" type="text" name="AssessableValueinSpec" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_ass_val/*(finAssVal / ConvRate)*/, ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" onblur="this.placeholder='0000:00'" >
                                        <span id="SpanTxtAssessableValueinSpecErrorMsg" class="error-message is-visible"></span>
                                        </div>
                                    </td>
                                    <td>
                                         <input id="TxtAssessableValue" class="form-control num_right" autocomplete="off" type="text" name="AssessableValue" value="${parseFloat(arr.Table[k].item_ass_val_bs).toFixed(ValDecDigit)}" placeholder="0000:00" onblur="this.placeholder='0000:00'" disabled>                                         
                                    </td>
                                    `+ TaxExempted + `
                                    `+ ManualGst + `
                                    <td><div class=" col-sm-9 no-padding"><input id="Txtitem_tax_amt" class="form-control num_right" type="text" name="item_tax_amt" placeholder="0000:00" disabled></div><div class=" col-sm-3 no-padding"><button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button></div></td>

                                    <td><input id="InvoiceAmount" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmount" value="${parseFloat(arr.Table[k].item_ass_val_bs).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td><input id="InvoiceAmountInSpec" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmount" value="${parseFloat(getvalwithoutroundoff((arr.Table[k].item_ass_val/*parseFloat(finAssVal) / parseFloat(ConvRate)*/), ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                </tr>`);
                                    var clickedrow = $("#SInvItmDetailsTbl >tbody >tr #SNohiddenfiled[value=" + S_NO + "]").closest("tr");
                                    clickedrow.find("#TxtItemName").val(arr.Table[k].item_name);
                                    var Itm_ID = arr.Table[k].item_id;
                                    //var GstApplicable = $("#Hdn_GstApplicable").text();

                                    if (GstApplicable == "Y") {
                                        //Cmn_ApplyGSTToAtable("SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", arr.Table1);
                                    }
                                    else {
                                        $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                        if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                            $("#HdnTaxOn").val("Item");
                                            $("#TaxCalcItemCode").val(Itm_ID);
                                            $("#HiddenRowSNo").val(k);
                                            $("#Tax_AssessableValue").val(ItemAssVal);
                                            $("#TaxCalcGRNNo").val(arr.Table[k].ship_no);
                                            $("#TaxCalcGRNDate").val(arr.Table[k].ship_date);
                                            var TaxArr = arr.Table1;
                                            let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                            selected = JSON.stringify(selected);
                                            TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                            selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                            selected = JSON.stringify(selected);
                                            TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                            if (TaxArr.length > 0) {
                                                AddTaxByHSNCalculation(TaxArr);
                                                OnClickTaxSaveAndExit("Y");
                                                var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                                Reset_ReOpen_LevelVal(lastLevel);
                                            }

                                        }
                                    }
                                }
                                CalculateAmount();
                                //CIAssVal = $("#TxtAssValueInBase").val();
                                CIAssVal = $("#TxtAssValueInSpec").val();
                                //CIAssVal = getvalwithoutroundoff(parseFloat(CIAssVal) / parseFloat(ConvRate),ValDecDigit);
                                //$('#SInvItmDetailsTbl tbody tr').each(function () {
                                //    var row = $(this);
                                //    var TxtAssessableValue = parseFloat(row.find("#TxtAssessableValue").val());
                                //    var CIAssVal = parseFloat(CIAssVal) + TxtAssessableValue;
                                //})
                                completeTime = moment();
                                duration = moment.duration(completeTime.diff(startTime));
                                console.log("Added : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
                                /*---------------------------- Reset Assessable Value ----------------------------*/
                                var Qty = arr.Table[0].ship_qty;
                                var Rate = arr.Table[0].ass_rate;
                                var itemWithMinQty = arr.Table[0].item_id;
                                arr.Table.map(v => {//Get
                                    if (Qty > v.ship_qty && Rate < v.ass_rate) {
                                        Qty = v.ship_qty;
                                        Rate = v.ass_rate;
                                        itemWithMinQty = v.item_id;
                                    }
                                })
                                if (Math.abs(parseFloat(CheckNullNumber(totalGrVal)) - parseFloat(CheckNullNumber(CIAssVal))) >= 0.00) {
                                    let differenc = parseFloat((parseFloat(CheckNullNumber(CIAssVal)) - parseFloat(CheckNullNumber(totalGrVal)))).toFixed(ValDecDigit);
                                    //commented by Suraj on 23-07-2024. Reason : after this calculation data gets incurrect 
                                    ResestAssRateToMatchInvoiceValue(differenc, /*row*/itemWithMinQty, arr);
                                    //let row = 0
                                    //while (Math.abs(differenc) > 0.00 && row < $('#SInvItmDetailsTbl tbody tr').length) {
                                    //    console.log(itemWithMinQty);
                                    //    ResestAssRateToMatchInvoiceValue(differenc, row, arr);
                                    //    CalculateAmount();
                                    //    CIAssVal = $("#TxtAssValueInBase").val();
                                    //    CIAssVal = getvalwithoutroundoff(parseFloat(CIAssVal) / parseFloat(ConvRate), ValDecDigit);
                                    //    differenc = parseFloat((parseFloat(CheckNullNumber(CIAssVal)) - parseFloat(CheckNullNumber(totalGrVal)))).toFixed(ValDecDigit);
                                    //    row = $('#SInvItmDetailsTbl tbody tr').length;
                                    //    row++;
                                    //}
                                    //console.log("Row Assessable Value Reset : " + row);
                                }
                                /*---------------------------- Reset Assessable Value End----------------------------*/
                                completeTime = moment();
                                duration = moment.duration(completeTime.diff(startTime));
                                console.log("Assable Price Reset : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                                if (GstApplicable == "Y") {//Applying Tax
                                    Cmn_ApplyGSTToAtable("SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", arr.Table1, '', '', '', "AddToHidden");
                                }

                                completeTime = moment();
                                duration = moment.duration(completeTime.diff(startTime));
                                console.log("Tax Reset : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');

                                if (arr.Table5.length > 0) {
                                    var OCVal = 0;
                                    for (var k = 0; k < arr.Table5.length; k++) {
                                        $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
                                  <td id="OC_name">${arr.Table5[k].oc_name}</td>
                                  <td id="OC_Curr">${arr.Table5[k].curr_name}</td>
                                  <td id="OC_Conv">${arr.Table5[k].conv_rate}</td>
                                  <td hidden="hidden" id="OC_ID">${arr.Table5[k].oc_id}</td>
                                  <td class="num_right" id="OCAmtSp">${arr.Table5[k].oc_val}</td>
                                  <td class="num_right" id="OCAmtBs">${arr.Table5[k].OCValBs}</td>
                                </tr>`);
                                        OCVal = parseFloat(OCVal) + parseFloat(arr.Table5[k].OCValBs);

                                        $('#Tbl_OtherChargeList tbody').append(`<tr>
                                <td>${arr.Table5[k].oc_name}</td>
                                <td id="OCAmtSp1" align="right">${arr.Table5[k].OCValBs}</td>
                                <td hidden="hidden" id="OCID">${arr.Table5[k].oc_id}</td>
                                </tr>`);
                                    }
                                    $("#TxtOtherCharges").val(parseFloat(OCVal).toFixed(ValDecDigit));
                                    $("#_OtherChargeTotal").text(parseFloat(OCVal).toFixed(ValDecDigit));
                                }
                                CalculateAmount();
                                CalculateVoucherTotalAmount();
                                //debugger;
                                //GetAllGLID();                         
                            }


                            if (arr.Table4.length > 0) {
                                if (arr.Table4[0].trade_term != null) {
                                    $("#trade_term").val(arr.Table4[0].trade_term);
                                }
                                else {
                                    $("#trade_term").val('CFR');
                                }
                                $("#PreCarriageBy").val(arr.Table4[0].pre_carr_by);
                                if (arr.Table4[0].pi_rcpt_carr != null) {
                                    $("#PlOfReceiptByPreCarrier").val(arr.Table4[0].pi_rcpt_carr).trigger('change');
                                }
                                else {
                                    $("#PlOfReceiptByPreCarrier").val(0).trigger('change');
                                }
                                $("#VesselFlightNumber").val(arr.Table4[0].ves_fli_no);
                                if (arr.Table4[0].loading_port != null) {
                                    $("#PortOfLoading").val(arr.Table4[0].loading_port).trigger('change');
                                }
                                else {
                                    $("#PortOfLoading").val(0).trigger('change');
                                }
                                $("#PortOfDischarge").val(arr.Table4[0].discharge_port);
                                $("#FinalDestination").val(arr.Table4[0].fin_disti);
                                $("#ContainerNumber").val(arr.Table4[0].container_no);
                                $("#OtherRefrences").val(arr.Table4[0].other_ref);
                                $("#TermOfDeliveryAndPayment").val(arr.Table4[0].term_del_pay);
                                $("#DescriptionOfGoods").val(arr.Table4[0].des_good);
                                $("#ProformaDetail").val(arr.Table4[0].prof_detail);
                                $("#Declaration").val(arr.Table4[0].declar);
                                $("#CountryOfOriginOfGoods").val(arr.Table4[0].cntry_origin);
                                $("#CountryOfFinalDestination").val(arr.Table4[0].cntry_fin_dest);
                                $("#ExportersReference").val(arr.Table4[0].ext_ref);
                                $("#BuyerIfOtherThenConsignee").val(arr.Table4[0].buyer_consig);
                                $("#InvoiceHeading").val(arr.Table4[0].inv_head);
                                $("#BuyersOrderNumberAndDate").val(arr.Table4[0].buyer_ord_no_dt);
                                $("#ExporterAddress").val(arr.Table4[0].exp_addr);
                            }
                            /***Modifyed By Shubham Maurya on 27-10-2023 for manual Gst and Tax Exempted***/
                            $("#SInvItmDetailsTbl >tbody >tr").each(function () {
                                debugger;
                                var currentRow = $(this);
                                var hsn_code = currentRow.find("#ItemHsnCode").val();
                                var ItemName = currentRow.find("#TxtItemName").val();
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (GstApplicable == "Y") {
                                    if (hsn_code == "" || hsn_code == null || hsn_code == "0") {
                                        swal("", $("#HSNNotDefinedFor").text() + ' ' + ItemName, "warning");
                                        return false;
                                    }
                                }
                            });
                            /***Modifyed By Shubham Maurya on 08-12-2023 for Nontaxable***/
                            debugger;
                            if ($("#nontaxable").is(":checked")) {
                                CheckedNontaxable();
                            }
                            else {
                                GetAllGLID();
                            }
                            completeTime = moment();
                            duration = moment.duration(completeTime.diff(startTime));
                            console.log("Done : " + duration.minutes() + ' minutes, ' + duration.seconds() + ' seconds');
                            hideLoader();
                            /***-----------------------------------------------------------------------------***/
                        }
                    }
                    catch (ex) {
                        console.log(ex);
                        hideLoader();
                    }
                    
                },
            });
        } catch (err) {
            console.log("SaleInvoice Error : " + err.message);
            hideLoader();
        }
    }
}
function ResestAssRateToMatchInvoiceValue(differenc, adjustRowNo, arr) {
    var ConvRate = $("#conv_rate").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $('#SInvItmDetailsTbl tbody tr #hfItemID[value=' + adjustRowNo + ']').closest('tr').each(function () {
    //$('#SInvItmDetailsTbl tbody tr:eq(' + adjustRowNo+')').each(function () {
        var row = $(this);
        var TxtAssessableValue = parseFloat(row.find("#TxtAssessableValue").val());
        var shipQty = parseFloat(row.find("#TxtReceivedQuantity").val());
        var NewAssVal = parseFloat((parseFloat(TxtAssessableValue) - (differenc * ConvRate))).toFixed(ValDecDigit);
        //row.find("#TxtAssessableValue").val(NewAssVal);
        var NewRate = parseFloat(getvalwithoutroundoff(((parseFloat(NewAssVal) / parseFloat(ConvRate)) / parseFloat(shipQty)), RateDecDigit)).toFixed(RateDecDigit);
        var AssAfterNewRate = getvalwithoutroundoff(cmn_getmultival(NewRate, shipQty), ValDecDigit);
        row.find("#TxtAssRate").val(NewRate);
        //row.find("#TxtAssessableValue").val(parseFloat(getvalwithoutroundoff(NewAssVal)).toFixed(ValDecDigit));
        //row.find("#TxtAssessableValueinSpec").val(parseFloat((parseFloat(NewAssVal) / parseFloat(ConvRate))).toFixed(ValDecDigit));
        row.find("#TxtAssessableValue").val(parseFloat(getvalwithoutroundoff(cmn_getmultival(AssAfterNewRate, ConvRate), ValDecDigit)).toFixed(ValDecDigit));
        row.find("#TxtAssessableValueinSpec").val(parseFloat(AssAfterNewRate).toFixed(ValDecDigit));
        //if (GstApplicable == "Y") {
        //    Cmn_ApplyGSTToAtable("SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", arr.Table1);
        //}
        //else {
        //    //$("#hd_tax_id").val(arr.Table[k].tmplt_id);
        //    //if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
        //    //    $("#HdnTaxOn").val("Item");
        //    //    $("#TaxCalcItemCode").val(Itm_ID);
        //    //    $("#HiddenRowSNo").val(k);
        //    //    $("#Tax_AssessableValue").val(ItemAssVal);
        //    //    $("#TaxCalcGRNNo").val(arr.Table[k].ship_no);
        //    //    $("#TaxCalcGRNDate").val(arr.Table[k].ship_date);
        //    //    var TaxArr = arr.Table1;
        //    //    let selected = []; selected.push({ item_id: arr.Table[k].item_id });
        //    //    selected = JSON.stringify(selected);
        //    //    TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
        //    //    selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
        //    //    selected = JSON.stringify(selected);
        //    //    TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
        //    //    if (TaxArr.length > 0) {
        //    //        AddTaxByHSNCalculation(TaxArr);
        //    //        OnClickTaxSaveAndExit("Y");
        //    //        var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
        //    //        Reset_ReOpen_LevelVal(lastLevel);
        //    //    }

        //    //}
        //}
    })
}
async function OnClickTaxExemptedCheckBox(e, row,flag) {
    debugger;

    var currentrow = row == null ? $(e.target).closest('tr') : row;

    var GstApplicable = $("#Hdn_GstApplicable").text();
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        CalculateTaxExemptedAmt(e, currentrow)
        if (flag == null) {
            GetAllGLID();
        }
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            await Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", "", flag, currentrow);
            CalculateAmount();
            if (flag == null) {
                
                GetAllGLID();
            }
        }
        else {
            CalculateTaxExemptedAmt(e, currentrow)
            GetAllGLID();
        };
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var item = currentrow.find("#hfItemID").val();
    $("#Tax_ItemID").val(item);
    if (currentrow.find("#ManualGST").is(":checked")) {
        currentrow.find('#TaxExempted').prop("checked", false);
        $("#TaxCalculatorTbl tbody tr").remove();
        $("#taxTemplate").text("Template");
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        $("#TotalTaxAmount").text(parseFloat(0).toFixed(ValDecDigit))
        CalculateTaxExemptedAmt(e)
        GetAllGLID();
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));

    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        $("#TaxCalculatorTbl tbody tr").remove();
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtAssessableValue", "", "", currentrow).then(() => {
            GetAllGLID();
        });
        CalculateTaxExemptedAmt(e)
        $("#taxTemplate").text("Template")
    }
}
function CalculateTaxExemptedAmt(e, row) {
    debugger;
    var clickedrow = row == null ? $(e.target).closest("tr") : row;
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    var ItmRate = clickedrow.find("#TxtRate").val();
    //var ItmAssRate = clickedrow.find("#TxtAssRate").val();
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
        var FAmt = cmn_getmultival(OrderQty , ItmRate);
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        CalculateAmount();
    }
    let AssAmount = parseFloat(CheckNullNumber(clickedrow.find("#TxtAssessableValue").val()));
    if ($("#nontaxable").is(":checked")) {
        //CalculateTaxAmount_ItemWise(ItemName, AssAmount);
        //clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed());
    }
    else if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(ItemName, AssAmount);
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        //CalculateTaxAmount_ItemWise(ItemName, AssAmount);
        if (GstApplicable == "Y") {
            if (clickedrow.find("#ManualGST").is(":checked")) {
                clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
                CalculateTaxAmount_ItemWise(ItemName, AssAmount);
                clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            }
            else {
                CalculateTaxAmount_ItemWise(ItemName, AssAmount);
            }
        } else {
            CalculateTaxAmount_ItemWise(ItemName, AssAmount);
        }
    }
}
function CalculateTaxAmount_ItemWise_forTaxExampt() {
    debugger;
    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    if (FTaxDetailsItemWise > 0) {
        var NewArray = [];
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = 0;//currentRow.find("#TaxAmount").text();
            var TotalTaxAmount = 0;// currentRow.find("#TotalTaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

            NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

        });
        BindTaxAmountDeatils(NewArray);
    }
}
function CalculateTaxAmount_ItemWise_forAllItems() {//For Calculate Tax 
    debugger;
    var ConvRate = $("#conv_rate").val();
    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    if (FTaxDetailsItemWise > 0) {
        var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var NewArray = [];
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            
            //---------------------------------------------------------
            var ItemRow = $("#SInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
            var AssAmount = ItemRow.find("#TxtAssessableValue").val();
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

        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
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
                        let AssessableValue = (parseFloat(currentRow.find("#TxtAssessableValue").val())).toFixed(ValDecDigit);
                        let NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let FinalNetOrderValueBase = /*ConvRate **/ NetOrderValueBase
                        var oc_amt = currentRow.find("#TxtOtherCharge").val();
                        if (oc_amt == ".") {
                            oc_amt = 0;
                        }
                        if (oc_amt != "" && oc_amt != ".") {
                            oc_amt = parseFloat(oc_amt);
                        }
                        currentRow.find("#InvoiceAmount").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                        currentRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff((parseFloat(FinalNetOrderValueBase) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                    }
                });
            }
            else {
                var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                var GrossAmtOR = parseFloat(currentRow.find("#TxtAssessableValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                currentRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff((parseFloat(FGrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#InvoiceAmount").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
            }
        });
        CalculateAmount();
        BindTaxAmountDeatils(NewArray);
    }
}
function Cmn_CalculateTaxAmount(TaxApplyOn, TaxLevel, AssessVal, TaxPec, TaxArr, TaxItmCode) {
    var TaxAmount = 0;
    var NewArray = TaxArr != null ? TaxArr.filter(v => v.TaxItmCode == TaxItmCode) : [];//Getting Tax List For One Item
    if (TaxApplyOn == "Immediate Level") {
        if (TaxLevel == "1") {
            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(AssessVal, TaxPec) / 100), ValDecDigit)).toFixed(ValDecDigit);
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
            TaxAmount = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinalAssAmtIL, TaxPec), ValDecDigit) / 100).toFixed(ValDecDigit);
        }
    }
    if (TaxApplyOn == "Cummulative") {
        var Level = TaxLevel;
        if (TaxLevel == "1") {
            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(AssessVal, TaxPec) / 100), ValDecDigit)).toFixed(ValDecDigit);
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
            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmt), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
        }
    }
    return TaxAmount;
}
function CalculateTaxAmount_ItemWise(ItmCode, AssAmount) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        debugger;
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if (TaxItemID == ItmCode) {
                    var ItemRow = $("#SInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItemID + "']").closest('tr');
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
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(AssessVal, TaxPec) / 100), ValDecDigit)).toFixed(ValDecDigit);
                            TotalTaxAmt = parseFloat(cmn_getsumval(parseFloat(TotalTaxAmt), parseFloat(TaxAmount), ValDecDigit)).toFixed(ValDecDigit);
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
                            TaxAmount = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinalAssAmtIL, TaxPec), ValDecDigit) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(AssessVal, TaxPec) / 100), ValDecDigit)).toFixed(ValDecDigit);
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
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmt), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    debugger;
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

                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            //$("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().parent().each(function () {
            $("#SInvItmDetailsTbl >tbody >tr #hfItemID[value=" + ItmCode + "]").closest("tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var ItemNo = currentRow.find("#hfItemID").val();
                if (ItemNo == ItmCode) {
                    //var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + GrnNo + "')").closest("tr");
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
                                //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed();
                                //}
                                //AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed();
                                let AssessableValue = (parseFloat(currentRow.find("#TxtAssessableValue").val())).toFixed(ValDecDigit);
                                let NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                let NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                //currentRow.find("#NetOrderValueSpe").val(NetOrderValueSpec);
                                //currentRow.find("#InvoiceAmountInSpec").val(NetOrderValueSpec);
                                //currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                                let FinalNetOrderValueBase = /*ConvRate **/ NetOrderValueBase
                                var oc_amt = currentRow.find("#TxtOtherCharge").val();
                                if (oc_amt == ".") {
                                    oc_amt = 0;
                                }
                                if (oc_amt != "" && oc_amt != ".") {
                                    oc_amt = parseFloat(oc_amt);
                                }
                                //FinalNetOrderValueBase = FinalNetOrderValueBase + oc_amt
                                //NetOrderValueSpec = parseFloat(NetOrderValueSpec) + oc_amt;
                                currentRow.find("#InvoiceAmount").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                                currentRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff((parseFloat(FinalNetOrderValueBase) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                                //currentRow.find("#TxtItemGrossValueInBase").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                            }
                        });
                    }
                    else {
                        //debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        //var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed();
                        var GrossAmtOR = parseFloat(currentRow.find("#TxtAssessableValue").val()).toFixed(ValDecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        //    OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed();
                        //}
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        currentRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff((parseFloat(FGrossAmtOR) / parseFloat(ConvRate)),ValDecDigit)).toFixed(ValDecDigit));
                        FinalFGrossAmtOR = /*ConvRate **/ FGrossAmtOR
                        //var oc_amt = currentRow.find("#TxtOtherCharge").val();
                        //if (oc_amt == ".") {
                        //    oc_amt = 0;
                        //}
                        //if (oc_amt != "" && oc_amt != ".") {
                        //    oc_amt = parseFloat(oc_amt);
                        //}
                        //FinalFGrossAmtOR = FinalFGrossAmtOR + oc_amt;
                        //FGrossAmtOR = FGrossAmtOR + oc_amt;
                        currentRow.find("#InvoiceAmount").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
                        //currentRow.find("#TxtItemGrossValueInBase").val(parseFloat(FinalFGrossAmtOR).toFixed());
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function RateFloatValueonlyConvRate(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}
function RateFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExpImpRateDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function QtyFloatValueonly(el, evt) {
    if (CmnAmtFloatVal(el, evt, "#ExpImpQtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function AmtFloatValueonly(el, evt) {
    if (CmnAmtFloatVal(el, evt, "#ExpImpValDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function FloatValuePerOnly(el, evt) {

    $("#SpanTaxPercent").css("display", "none");
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    else {
        return true;
    }


}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode)

}
function OnClickBuyerInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var ItmCode = "";

    ItmCode = clickedrow.find("#hfItemID").val();
    var Cust_id = $('#CustomerName').val();

    Cmn_BuyerInfoBtnClick(ItmCode, Cust_id);


}
function OnchangeConvRate(e) {
    //debugger;
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
    OnchangeCurrencyConvRateCalculate();
}

/*-----------------------END JS-------------------------------------- */
/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var SIStatus = "";
    //SIStatus = $('#hfSIStatus').val().trim();
    //if (SIStatus === "D" || SIStatus === "F") {

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

    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var CinvDate = $("#Inv_date").val();
        $.ajax({
            type: "POST",
            /*   url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: CinvDate
            },
            success: function (data) {
                /*if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var SIStatus = "";
                    SIStatus = $('#hfSIStatus').val().trim();
                    if (SIStatus === "D" || SIStatus === "F") {

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
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SINo = "";
    var SIDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var InvType = "";
    var mailerror = "";
    var Narration = $("#JVRaisedAgainstCustomInvoice").text();

    docid = $("#DocumentMenuId").val();
    SINo = $("#InvoiceNumber").val();
    SIDate = $("#Inv_date").val();
    $("#hdDoc_No").val(SINo);
    WF_status1 = $("#WF_status1").val();
    Remarks = $("#fw_remarks").val();
    var TrancType = (SINo + ',' + SIDate + ',' + docid + ',' + WF_status1)
    var ListFilterData1 = $("#ListFilterData1").val();
    if ($("#OrderTypeD").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "E";
    }
    //SaleVouMsg = $("#SaleVoucherPassAgainstInv").text()
    //SourceType = $("#ddlRequisitionTypeList").val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "ExportInvoice_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(SINo, SIDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/CustomInvoice/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SINo != "" && SIDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CustomInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ SINo: SINo, SIDate: SIDate, InvType: InvType, Narration: Narration, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/CustomInvoice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/CustomInvoice/SIListApprove?SI_No=" + SINo + "&SI_Date=" + SIDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&Narration=" + Narration;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SINo != "" && SIDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CustomInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SINo != "" && SIDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/CustomInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(invNo, invDt, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/CustomInvoice/SavePdfDocToSendOnEmailAlert",
//        data: { invNo: invNo, invDt: invDt, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
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

/*-----------End Workflow--------------*/

function GetCurrentDatetime(ActionType) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetCurrentDT",/*Controller=LSODetail and Fuction=GetCurrentDT*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',/*Registration pass value like model*/
        success: function (response) {
            debugger;
            if (response === 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (ActionType === "Save") {
                $("#SICreatedBy").text(response.CurrentUser);
                $("#SICreatedDate").text(response.CurrentDT);
            }
            if (ActionType === "Edit") {
                $("#SIAmdedBy").text(response.CurrentUser);
                $("#SIAmdedDate").text(response.CurrentDT);
            }
            if (ActionType === "Approved") {
                $("#SIApproveBy").text(response.CurrentUser);
                $("#SIApproveDate").text(response.CurrentDT);
            }
        }
    });
}
function DSIList() {
    try {
        location.href = "/ApplicationLayer/CustomInvoice/CustomInvoiceList";
    } catch (err) {

    }
}


//------------------Tax Amount Calculation------------------//

function OnClickTaxCalBtn(e) {
    debugger;
    var SOItemListName = "#TxtItemName";
    var SNohiddenfiled = "CI";
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
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
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
}
function OnClickSaveAndExit() {
    OnClickTaxSaveAndExit();
}
function OnClickTaxSaveAndExit(OnAddGRN) {
    var ConvRate = $("#conv_rate").val();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var ShipmentNo = $("#TaxCalcGRNNo").val();
    var ShipmentDate = $("#TaxCalcGRNDate").val();


    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var GrossOrderValueBase = parseFloat(0).toFixed(ValDecDigit);

    //debugger;
    let NewArr = new Array();
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    //if (TaxOn == "OC") {
    //    HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    //}
    //var taxrowcount = $('#hdn_TaxCalculatorTbl tbody tr').length;
    var taxrowcount = $('#' + HdnTaxCalculateTable + ' tbody tr').length;
    //var SelectedTaxItem = $("#hdTaxDetailList").val();
    //if (SelectedTaxItem != null || SelectedTaxItem !="") {
    if (taxrowcount > 0) {
        //debugger;
        //var SelectedItem = $("#TaxCalcItemCode").val();
        $("#" + HdnTaxCalculateTable + " TBODY TR").each(function () {
            // debugger;
            var row = $(this);
            var rowitem = row.find("#TaxItmCode").text();
            var shipno = row.find("#DocNo").text();
            var shipdate = row.find("#DocDate").text();
            //if (TaxOn == "OC" && rowitem == TaxItmCode) {
            //    $(this).remove();
            //} else {
            if (rowitem == TaxItmCode && shipno == ShipmentNo && shipdate == ShipmentDate) {
                //debugger;
                $(this).remove();
            }
            //}

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
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                <tr>
                                    <td id="DocNo">${ShipmentNo}</td>
                                    <td id="DocDate">${ShipmentDate}</td>
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
            //                $('#' + HdnTaxCalculateTable+' tbody').append(`<tr id="R${rowIdx}">
            //<td id="shipno" width="18%">${ShipmentNo}</td>
            //<td id="shipdate">${ShipmentDate}</td>
            //<td width="18%" id="itemid">${TaxItmCode}</td>
            //<td id="taxname" width="18%">${TaxName}</td>
            //<td  hidden="hidden" id="taxid">${TaxNameID}</td>
            //<td class="num_right" width="18%" id="taxrate">${TaxPercentage}</td>
            //<td width="19%" class='center' id="taxlevel">${TaxLevel}</td>
            //<td width="27%" id="taxapplyonname">${TaxApplyOn}</td>
            //<td align="right" width="18%" id="taxval">${parseFloat(TaxAmount).toFixed(ValDecDigit)}</td>
            //<td hidden="hidden" id="taxapplyon">${TaxApplyOnID}</td>
            //</tr>`);
        });

        var rowcount = $('#' + HdnTaxCalculateTable + ' tbody tr').length;
        if (rowcount > 0) {
            $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {

                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text().replace('', '%');
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                //var TotalTaxAmount = currentRow.find("#itemtaxamt").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

                NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TaxApplyOnID: TaxApplyOnID, });
            });
        }
        //debugger;
        if (TaxOn != "OC") {
            var ForGL = OnAddGRN == "Y" ? "N" : "";
            BindTaxAmountDeatils(NewArr, ForGL);
        }
    }
    else {

        var TaxCalculationList = new Array();

        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var TaxCL = {}
            var currentRow = $(this);
            TaxCL.TaxName = currentRow.find("#taxname").text();
            TaxCL.TaxNameID = currentRow.find("#taxid").text();
            TaxCL.TaxPercentage = currentRow.find("#taxrate").text();
            TaxCL.TaxLevel = currentRow.find("#taxlevel").text();
            TaxCL.TaxApplyOn = currentRow.find("#taxapplyonname").text();
            TaxCL.TaxAmount = currentRow.find("#taxval").text();
            TaxCL.TaxApplyOnID = currentRow.find("#taxapplyon").text();
            TaxCL.TaxItmCode = TaxItmCode;
            TaxCalculationList.push(TaxCL)
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                <tr>
                                    <td id="DocNo">${ShipmentNo}</td>
                                    <td id="DocDate">${ShipmentDate}</td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxCL.TaxName}</td>
                                    <td id="TaxNameID">${TaxCL.TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxCL.TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxCL.TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxCL.TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxCL.TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxCL.TaxApplyOnID}</td>

                                </tr>`);
            //var TaxName = currentRow.find("#taxname").text();
            //var TaxNameID = currentRow.find("#taxid").text();
            //var TaxPercentage = currentRow.find("#taxrate").text();
            //var TaxLevel = currentRow.find("#taxlevel").text();
            //var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            //var TaxAmount = currentRow.find("#taxval").text();
            //var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            //                $('#' + HdnTaxCalculateTable+' tbody').append(`<tr id="R${rowIdx}">
            //<td id="shipno" width="18%">${ShipmentNo}</td>
            //<td id="shipdate">${ShipmentDate}</td>
            //<td width="18%" id="itemid">${TaxItmCode}</td>
            //<td id="taxname" width="18%">${TaxCL.TaxName}</td>
            //<td  hidden="hidden" id="taxid">${TaxCL.TaxNameID}</td>
            //<td class="num_right" width="18%" id="taxrate">${TaxCL.TaxPercentage}</td>
            //<td width="19%" class='center' id="taxlevel">${TaxCL.TaxLevel}</td>
            //<td width="27%" id="taxapplyonname">${TaxCL.TaxApplyOn}</td>
            //<td align="right" width="18%" id="taxval">${parseFloat(TaxCL.TaxAmount).toFixed(ValDecDigit)}</td>
            //<td hidden="hidden" id="taxapplyon">${TaxCL.TaxApplyOnID}</td>
            //</tr>`);
        });
        if (TaxOn != "OC") {
            //var ForGL = OnAddGRN == "Y" ? "N" : "";
            BindTaxAmountDeatils(TaxCalculationList);
        }
    }

    //if (TaxOn == "OC") {
    //    $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
    //        // debugger;
    //        var currentRow = $(this);
    //        if (currentRow.find("#OCValue").text() == TaxItmCode) {
    //            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed();
    //            currentRow.find("#OCTaxAmt").text(TaxAmt);
    //            var OCAmt = currentRow.find("#OcAmtBs").text().trim();
    //            var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed()
    //            currentRow.find("#OCTotalTaxAmt").text(Total);
    //        }
    //    });
    //    Calculate_OCAmount();
    //} else {
    $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        //var ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
        //var ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();
        var ItmCode = currentRow.find("#hfItemID").val();
        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
        if (TaxAmt == "" || TaxAmt == "NaN") {
            TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        }
        OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
        if (ItmCode == TaxItmCode) {
            currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
            //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
            //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
            //}
            //AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
            AssessableValue = (parseFloat(currentRow.find("#TxtAssessableValueinSpec").val())).toFixed(ValDecDigit);
            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
            NetOrderValueBase = parseFloat(getvalwithoutroundoff(cmn_getmultival(AssessableValue, ConvRate), ValDecDigit)).toFixed(ValDecDigit);
            GrossOrderValueBase = parseFloat(AssessableValue).toFixed(ValDecDigit);
            //currentRow.find("#TxtItemGrossValue").val(AssessableValue);
            //debugger;
            FinalNetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
            NetOrderValueSpec = parseFloat(getvalwithoutroundoff((parseFloat(FinalNetOrderValueBase) / ConvRate), ValDecDigit)).toFixed(ValDecDigit);
            currentRow.find("#InvoiceAmount").val(FinalNetOrderValueBase);
            currentRow.find("#InvoiceAmountInSpec").val(NetOrderValueSpec);
        }
        var TaxAmt3 = parseFloat(0).toFixed(ValDecDigit)
        var ItemTaxAmt3 = currentRow.find("#Txtitem_tax_amt").val();
        if (ItemTaxAmt3 != TaxAmt3) {
            currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
        }
    });
    CalculateAmount();
    if (OnAddGRN != "Y") {
        GetAllGLID();
    }
    //}

}
function CalculateAmount(flag) {
    //debugger;
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    var AssValue = parseFloat(0).toFixed(ValDecDigit);
    var AssValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var TotalInvAmt = parseFloat(0).toFixed(ValDecDigit);
    var TotalInvAmtSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);

    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);

        if (parseFloat(CheckNullNumber(currentRow.find("#TxtAssessableValue").val())) == 0) {
            AssValue = (parseFloat(getvalwithoutroundoff(AssValue,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            AssValue = (parseFloat(getvalwithoutroundoff(AssValue, ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtAssessableValue").val(), ValDecDigit))).toFixed(ValDecDigit);
        }
        if (parseFloat(CheckNullNumber(currentRow.find("#TxtAssessableValueinSpec").val())) == 0) {
            AssValueSpec = (parseFloat(getvalwithoutroundoff(AssValueSpec,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            AssValueSpec = (parseFloat(getvalwithoutroundoff(AssValueSpec, ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtAssessableValueinSpec").val(), ValDecDigit))).toFixed(ValDecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(getvalwithoutroundoff(TaxValue,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TaxValue = (parseFloat(getvalwithoutroundoff(TaxValue, ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#Txtitem_tax_amt").val(), ValDecDigit))).toFixed(ValDecDigit);
        }

        if (currentRow.find("#InvoiceAmount").val() == "" || currentRow.find("#InvoiceAmount").val() == "NaN") {
            TotalInvAmt = (parseFloat(getvalwithoutroundoff(TotalInvAmt,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TotalInvAmt = (parseFloat(getvalwithoutroundoff(TotalInvAmt,ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#InvoiceAmount").val(),ValDecDigit))).toFixed(ValDecDigit);
        }
        if (currentRow.find("#InvoiceAmountInSpec").val() == "" || currentRow.find("#InvoiceAmountInSpec").val() == "NaN") {
            TotalInvAmtSpec = (parseFloat(getvalwithoutroundoff(TotalInvAmtSpec,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TotalInvAmtSpec = (parseFloat(getvalwithoutroundoff(TotalInvAmtSpec,ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#InvoiceAmountInSpec").val(),ValDecDigit))).toFixed(ValDecDigit);
        }
        if (currentRow.find("#TxtItemGrossValueInBase").val() == "" || currentRow.find("#TxtItemGrossValueInBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(getvalwithoutroundoff(NetOrderValueBase,ValDecDigit)) + parseFloat(0)).toFixed(ValDecDigit);

        }
        else {
            NetOrderValueBase = (parseFloat(getvalwithoutroundoff(NetOrderValueBase,ValDecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValueInBase").val(),ValDecDigit))).toFixed(ValDecDigit);

        }

    });
    $("#TxtGrossValue").val(NetOrderValueBase);
    $("#TxtAssValueInBase").val(AssValue);
    $("#TxtAssValueInSpec").val(AssValueSpec);
    $("#TxtTaxAmount").val(TaxValue);
    $("#TotalInvAmtBs").val(TotalInvAmt);
    $("#TotalInvAmtSpec").val(TotalInvAmtSpec);
    //$('#NetOrderValueSpe').change(GetAllGLID());

    //if (flag != "FromRO") {
    //    OnChangeRoundOffSpec("FromCalculate");
    //}
}
function ResetShipment_DDL_Detail() {
    $("#Shipment_Date").val("");
    var DocNo = $('#ddlShipmentNo').val();
    $("#ddlShipmentNo>optgroup>option[value='" + DocNo + "']").select2().hide();

    $('#ddlShipmentNo').val("---Select---").prop('selected', true);
    $('#ddlShipmentNo').trigger('change');
    $('#ddlShipmentNo').select2('close');
}

function OnClickReplicateOnAllItems() {
    var ConvRate = $("#conv_rate").val();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var ShipmentNo = $("#TaxCalcGRNNo").val();
    var ShipmentDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var UserID = $("#UserID").text();
    debugger;
    var rowIdx = 0;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    //if (TaxOn == "OC") {
    //    HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    //}
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ UserID: UserID, TaxShipmentNo: ShipmentNo, TaxShipmentDate: ShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: ShipmentNo, TaxShipmentDate: ShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "SInvItmDetailsTbl";
            //if (TaxOn == "OC") {
            //    TableOnwhichTaxApply = "Tbl_OC_Deatils";
            //}
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var ShipmentNoTbl;
                var ShipmentDateTbl;
                var ItemCode;
                var AssessVal;
                //ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
                //ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();

                //if (TaxOn == "OC") {
                //    ItemCode = currentRow.find("#OCValue").text();
                //    AssessVal = currentRow.find("#OCAmount").text();
                //} else {
                ItemCode = currentRow.find("#hfItemID").val();
                /*  AssessVal = currentRow.find("#TxtItemGrossValue").val();*/
                AssessVal = currentRow.find("#TxtItemGrossValue").val();
                //}


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
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
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
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
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmtIL), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
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
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmt), parseFloat(TaxPec)) / 100), ValDecDigit)).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    NewArray.push({ UserID: UserID, TaxShipmentNo: ShipmentNoTbl, TaxShipmentDate: ShipmentDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            var TaxShipmentNo = NewArray[k].TaxShipmentNo;
                            var TaxShipmentDate = NewArray[k].TaxShipmentDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo == TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate != TaxShipmentDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate != TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                        }
                    }
                }
            });
        }
        //debugger;
        $("#" + HdnTaxCalculateTable + " >tbody >tr").remove()
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                //$("#hdn_TaxCalculatorTbl >tbody >tr").remove();
                //debugger;
                for (n = 0; n < TaxCalculationListFinalList.length; n++) {
                    var TaxShipmentNo = TaxCalculationListFinalList[n].TaxShipmentNo;
                    var TaxShipmentDate = TaxCalculationListFinalList[n].TaxShipmentDate;
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationListFinalList[n].TaxName;
                    var TaxNameID = TaxCalculationListFinalList[n].TaxNameID;
                    var TaxItmCode = TaxCalculationListFinalList[n].TaxItmCode;
                    TaxPercentage = TaxCalculationListFinalList[n].TaxPercentage;
                    var TaxLevel = TaxCalculationListFinalList[n].TaxLevel;
                    var TaxApplyOn = TaxCalculationListFinalList[n].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationListFinalList[n].TaxApplyOnID;
                    var TaxAmount = TaxCalculationListFinalList[n].TaxAmount;
                    var Total_TaxAmount = TaxCalculationListFinalList[n].TotalTaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    $('#' + HdnTaxCalculateTable + ' tbody').append(`
                                <tr>
                                    <td id="DocNo">${TaxShipmentNo}</td>
                                    <td id="DocDate">${TaxShipmentDate}</td>
                                    <td id="TaxItmCode">${TaxItmCode}</td>
                                    <td id="TaxName">${TaxName}</td>
                                    <td id="TaxNameID">${TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxAmount}</td>
                                    <td id="TotalTaxAmount">${Total_TaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>                                   
                                </tr>`);

                }
            }
        }

    }


    //sessionStorage.setItem("SI_TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    //debugger;
    //if (TaxOn == "OC") {
    //    $("#Tbl_OC_Deatils >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        //debugger;
    //        var OCValue = currentRow.find("#OCValue").text();
    //        if (TaxCalculationListFinalList != null) {
    //            if (TaxCalculationListFinalList.length > 0) {
    //                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
    //                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
    //                    var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
    //                    if (OCValue == TaxItmCode) {
    //                        currentRow.find("#OCTaxAmt").text(TotalTaxAmtF);
    //                        var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
    //                        var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(ValDecDigit)
    //                        currentRow.find("#OCTotalTaxAmt").text(NetAmt);
    //                    }
    //                }
    //            }
    //            else {
    //                currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(ValDecDigit));
    //                var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
    //                var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(ValDecDigit)
    //                currentRow.find("#OCTotalTaxAmt").text(NetAmt);
    //            }
    //        }
    //    });
    //    Calculate_OCAmount();
    //} else {

    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        //var ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
        //var ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();
        var ItemID = currentRow.find("#hfItemID").val();
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                    var AShipmentNo = TaxCalculationListFinalList[i].TaxShipmentNo;
                    var AShipmentDate = TaxCalculationListFinalList[i].TaxShipmentDate;
                    var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                    if (ItemID == AItemID) {
                        TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TotalTaxAmtF);
                        var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                        if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                        }
                        //AssessVal = currentRow.find("#TxtItemGrossValue").val();
                        AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
                        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        NetOrderValueBase = parseFloat(getvalwithoutroundoff(cmn_getmultival(AssessableValue, ConvRate), ValDecDigit)).toFixed(ValDecDigit);
                        //currentRow.find("#TxtItemGrossValue").val(AssessableValue);
                        /*FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(ValDecDigit);*/
                        FinalNetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        debugger;
                        currentRow.find("#InvoiceAmount").val(FinalNetOrderValueBase);
                        //currentRow.find("#TxtItemGrossValueInBase").val(FinalNetOrderValueBase);
                    }
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
                //currentRow.find("#TxtItemGrossValue").val(FGrossAmt);
                FinalFGrossAmt = parseFloat(getvalwithoutroundoff(cmn_getmultival(FGrossAmt, ConvRate), ValDecDigit)).toFixed(ValDecDigit);
                currentRow.find("#FinalFGrossAmt").val(FGrossAmt);
            }
        }
        else {
            var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
            currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
            }
            var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(ValDecDigit);
            //currentRow.find("#TxtItemGrossValue").val(FGrossAmt);
            FinalFGrossAmt = parseFloat(getvalwithoutroundoff(cmn_getmultival(FGrossAmt, ConvRate), ValDecDigit)).toFixed(ValDecDigit);
            debugger;
            currentRow.find("#InvoiceAmount").val(FinalFGrossAmt);
            //currentRow.find("#TxtItemGrossValueInBase").val(FinalFGrossAmt);
        }
    });
    CalculateAmount();
    GetAllGLID();
    // }
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    //debugger;
    var SI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList"; /* "#SI_ItemTaxAmountList"; //Changed to common */
    var SI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal"; /*"#SI_ItemTaxAmountTotal";  //Changed to common*/

    //CMNBindTaxAmountDeatils(TaxAmtDetail, SI_ItemTaxAmountList, SI_ItemTaxAmountTotal)
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, SI_ItemTaxAmountList, SI_ItemTaxAmountTotal)
    if (bindval == "") {
        GetAllGLID();
    }


}
//---------------End Tax Amount Calculation------------------//
//------------------Tax For OC Calculation------------------//
//function OnClickOCTaxCalculationBtn(e) {
//    var OC_ID = "#OCValue";
//    var SNohiddenfiled = "OC";
//    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
//}
//------------------End------------------//
//------------------Other Charge Amount Calculation------------------//
//function OnClickSaveAndExit_OC_Btn() {
//    //debugger;
//    //BindOtherChargeGLDeatils(); 
//    var NetOrderValueSpe = "#NetOrderValueSpe";
//    var NetOrderValueInBase = "#NetOrderValueInBase";
//    var TxtOtherCharges = '#TxtOtherCharges';
//    CMNOnClickSaveAndExit_OC_Btn(TxtOtherCharges, NetOrderValueSpe, NetOrderValueInBase)
//}
//function Calculate_OC_AmountItemWise(TotalOCAmt) {
//    //debugger;
//    var ValDecDigit = $("#").text();
//    var ConvRate = $("#conv_rate").val();

//    var TotalGAmt = $("#TxtGrossValue").val();

//    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);
//        //var Sno = currentRow.find("#SNohiddenfiled").val();

//        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
//        var NetOrderValueSpec = currentRow.find("#TxtItemGrossValue").val();
//        var NetOrderValueBase = currentRow.find("#TxtItemGrossValueInBase").val();

//        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
//            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
//            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
//            if (parseFloat(OCAmtItemWise) > 0) {
//                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
//            }
//            else {
//                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(ValDecDigit));
//            }
//        }
//    });
//    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
//    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);
//        var OCValue = currentRow.find("#TxtOtherCharge").val();
//        if (OCValue != null && OCValue != "") {
//            if (parseFloat(OCValue) > 0) {
//                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
//            }
//            else {
//                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
//            }
//        }
//    });
//    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
//        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
//        var Sno = 0;
//        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//            //debugger;
//            var currentRow = $(this);
//            Sno = Sno + 1;
//            //var Sno = currentRow.find("#SNohiddenfiled").val();
//            var OCValue = currentRow.find("#TxtOtherCharge").val();
//            if (Sno == "1") {
//                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
//            }
//        });
//    }
//    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
//        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
//        var Sno = 0;
//        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//            //debugger;
//            Sno = Sno + 1;
//            var currentRow = $(this);
//            //var Sno = currentRow.find("#SNohiddenfiled").val();
//            var OCValue = currentRow.find("#TxtOtherCharge").val();
//            if (Sno == "1") {
//                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
//            }
//        });
//    }
//    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);

//        var POItm_GrossValue = currentRow.find("#TxtItemGrossValue").val();
//        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
//        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
//        if (POItm_OCAmt == null || POItm_OCAmt == "") {
//            POItm_OCAmt = "0";
//        }
//        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
//            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(ValDecDigit);
//        }
//        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
//        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
//        currentRow.find("#TxtItemGrossValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(ValDecDigit));
//        FinalItm_NetOrderValueBase = (POItm_NetOrderValueBase * ConvRate).toFixed(ValDecDigit);
//        currentRow.find("#TxtItemGrossValueInBase").val((parseFloat(FinalItm_NetOrderValueBase)).toFixed(ValDecDigit));
//    });
//    CalculateAmount();
//};
//function SetOtherChargeVal() {
//    var ValDecDigit = $("#").text();
//    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        //var Sno = currentRow.find("#SNohiddenfiled").val();
//        currentRow.find("#TxtOtherCharge").val((parseFloat(0)).toFixed(ValDecDigit));
//    });
//}
//function BindOtherChargeDeatils(val) {
//    //debugger;
//    var ValDecDigit = $("#").text();
//    //if ($("#SOItmDetailsTbl >tbody >tr").length == 0) {
//    //    $("#Tbl_OC_Deatils >tbody >tr").remove();
//    //    $("#Total_OC_Amount").text(parseFloat(0).toFixed(ValDecDigit));
//    //    $("#_OtherChargeTotal").val(parseFloat(0).toFixed(ValDecDigit));
//    //}
//    var hdbs_curr = $("#hdbs_curr").val();
//    var hdcurr = $("#hdcurr").val();
//    var OCTaxable = "N";
//    if (hdbs_curr == hdcurr) {
//        OCTaxable = "Y";
//    }

//    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
//    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
//    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
//    $("#Tbl_OtherChargeList >tbody >tr").remove();
//    $("#SI_OtherChargeTotal").text(parseFloat(0).toFixed(ValDecDigit));
//    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

//        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
//            var currentRow = $(this);
//            //debugger;  
//            var td = "";
//            if (OCTaxable=="Y") {
//                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
//<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
//            }

//            $('#Tbl_OtherChargeList tbody').append(`<tr>
//<td>${currentRow.find("#OCName").text()}</td>
//<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
//<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
//`+ td + `
//</tr>`);
//            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(ValDecDigit);
//            if (OCTaxable=="Y") {
//                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
//                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
//            }

//        });

//    }
//    if (val == "") {
//        GetAllGLID();
//    }
//    //$("#SI_OtherChargeTotal").text(TotalAMount);
//    $("#_OtherChargeTotal").text(TotalAMount);
//    if (OCTaxable=="Y") {
//        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
//        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
//    }

//}
//------------------Other Charge Amount Calculation END------------------//

async function GetAllGLID() {
    debugger;
    var cust_id = $("#CustomerName").val();
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "E";
    var TransType = 'Sal';
    var GLDetail = [];

    $("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var tax_id = currentRow.find("#taxID").text();
        var tax_amt = currentRow.find("#TotalTaxAmount").text();
        if (parseFloat(tax_amt) > 0) {
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "Tax", doctype: _SIType, Value: tax_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax"
            });
        }

    });
    if (GLDetail.length > 0) {

        debugger;
        await $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CustomInvoice/GetGLDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            data: JSON.stringify({ GLDetail: GLDetail }),
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    SI_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    var Voudet = 'Y';
                    $('#VoucherDetail tbody tr').remove();
                    if (arr.Table1.length > 0) {
                        var errors = [];
                        var step = [];
                        for (var i = 0; i < arr.Table1.length; i++) {
                            debugger;
                            if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
                                errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
                                step.push(parseInt(i));
                                Voudet = 'N';
                            }
                        }
                        var arrayOfErrorsToDisplay = [];
                        $.each(errors, function (i, error) {
                            arrayOfErrorsToDisplay.push({ text: error });
                        });
                        Swal.mixin({
                            confirmButtonText: 'Ok',
                            type: "warning",
                        }).queue(arrayOfErrorsToDisplay)
                            .then((result) => {
                                if (result.value) {

                                }
                            });
                    }
                    if (Voudet == 'Y') {
                        if (arr.Table.length > 0) {
                            $('#VoucherDetail tbody tr').remove();
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                debugger;
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${rowIdx}</td>
                                    <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
                                   
                            </tr>`);
                                }
                            }
                        }
                    }
                    CalculateVoucherTotalAmount();
                }
            }
        });
    }
    else {
        $('#VoucherDetail tbody tr').remove();
        CalculateVoucherTotalAmount();
        //swal("", $("#OneOrMoreGLAccountMissing").text(), "warning");
    }
}
async function CalculateVoucherTotalAmount(flag) {
    debugger;
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        debugger;
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
    });
    $("#DrTotal").text(DrTotAmt);
    $("#CrTotal").text(CrTotAmt);
    if (flag != "CalcOnly") {
        if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
            await AddTaxRecivable(DrTotAmt, CrTotAmt);
        }
    }

}
async function AddTaxRecivable(DrTotAmt, CrTotAmt) {
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CustomInvoice/GetTaxRecivable",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    $('#VoucherDetail tbody tr #type[value="Tr"]').closest("Tr").remove();
                    if (DrTotAmt < CrTotAmt) {
                        var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                        var rowIdx = $('#VoucherDetail tbody tr').length;
                        for (var j = 0; j < arr.Table.length; j++) {
                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${rowIdx}</td>
                                    <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
                                                   <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
                                   
                            </tr>`);
                            }
                        }
                    }
                    else {
                        var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);

                        var rowIdx = $('#VoucherDetail tbody tr').length;
                        for (var j = 0; j < arr.Table.length; j++) {
                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${rowIdx}</td>
                                    <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
                                     <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                    <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
                                                  
                                   
                            </tr>`);
                            }
                        }
                    }
                    CalculateVoucherTotalAmount("CalcOnly");

                }

            }

        }
    });
}

//------------------End Other Charge Amount Calculation------------------//

function OnClickbillingAddressIconBtn(e) {
    // debugger;

    $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());
    var Cust_id = $('#CustomerName').val();
    var bill_add_id = $('#bill_add_id').val()
    bill_add_id = bill_add_id != null ? bill_add_id.trim() : "";
    $('#hd_add_type').val("B");
    var CustPros_type = "C";

    var status = $("#hfSIStatus").val();
    status = status != null ? status.trim() : "";
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, SODTransType);
}
function OnClickShippingAddressIconBtn(e) {
    // debugger;
    $('#AddressInfo_LblName').text($('#AddressInfo_ShipAdd').text());
    var Cust_id = $('#CustomerName').val();
    var ship_add_id = $('#ship_add_id').val();
    ship_add_id = ship_add_id != null ? ship_add_id.trim() : "";
    $('#hd_add_type').val("S");
    var CustPros_type = "C";
    var status = $("#hfSIStatus").val();
    status = status != null ? status.trim() : "";
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }

    var SO_no = $("#so_no").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, ship_add_id, status, SODTransType, SO_no);
}

/*----------------- export items list to excel ------------------*/
function ExportItemsToExcel() {
    var invNo = $('#hdnINVNo').val();
    var invDate = $('#Inv_date').val();
    if (invNo != null && invNo != "" && invNo != undefined) {
        window.location.href = "/ApplicationLayer/CustomInvoice/ExportItemsToExcel?invNo=" + invNo + "&invDate=" + invDate;
    }
}
/*----------------- export items list to excel END------------------*/

function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
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

/*code wrote by sanjay on 20-05-2024 */
function ValidateAmountBeforeSubmit() {
    var assAmt = parseFloat($('#TxtAssValueInBase').val()).toFixed(ValDecDigit);
    /*Added by Suraj Maurya on 31-07-2024 in Case Round Off*/
    //var conv_rate = $("#conv_rate").val();
    //var assAmt = parseFloat(CheckNullNumber($('#TxtAssValueInSpec').val()) * CheckNullNumber(conv_rate)).toFixed(ValDecDigit);
    /*Added by Suraj Maurya 31-07-2024 in Case Round Off End*/
    var taxAmt = parseFloat($('#TxtTaxAmount').val()).toFixed(ValDecDigit);
    var totalAmt = parseFloat($('#TotalInvAmtBs').val()).toFixed(ValDecDigit);
    //Code modified by Suraj on 19-07-2024 to validate only two digit decimal place values.
    if ($("#nontaxable").is(":checked")) {
        if (parseFloat(getvalwithoutroundoff(assAmt, 2)).toFixed(2) != parseFloat(getvalwithoutroundoff(totalAmt, 2)).toFixed(2)) {
            swal("", $("#AssAmtDidntMatchWithTtlAmt").text(), "warning");
            return false;
        }
        else {
            /*  $('form').submit();*/
            return true;
        }
    }
    else {
        if (!$("#nontaxable").is(":checked")) {
            var ttlAmtCheck = parseFloat(assAmt) + parseFloat(taxAmt);
            if (parseFloat(getvalwithoutroundoff(ttlAmtCheck, 2)).toFixed(2) != parseFloat(getvalwithoutroundoff(totalAmt, 2)).toFixed(2)) {
                swal("", $("#AssAmtPlsTaxAmtDidntMatchWithTtlAmt").text(), "warning");
                return false;
            }
            else {
                /* $('form').submit();*/
                return true;
            }
        }
    }
}
///*code wrote by Suraj on 30-07-2024 */
//function OnChangeRoundOffSpec(flag) {
//    if (flag != "FromCalculate") {
//        CalculateAmount("FromRO");
//    }
//    var ValDigit = $("#ExpImpValDigit").text();
//    var conv_rate = $("#conv_rate").val();
//    var AssValueInSpec = $("#TxtAssValueInSpec").val();
//    var RoundOffValueInSpec = $("#RoundoffValueInSpec").val();
//    var NewAssValueInSpec = parseFloat(CheckNullNumber(AssValueInSpec)) + parseFloat(CheckNullNumber(RoundOffValueInSpec));
//    $("#TxtAssValueInSpec").val(parseFloat(NewAssValueInSpec).toFixed(ValDigit));
//    var TaxAmount = $("#TxtTaxAmount").val();
//    var NewAssValueInBs = parseFloat(NewAssValueInSpec).toFixed(ValDigit) * parseFloat(CheckNullNumber(conv_rate));
//    var TotalInvAmtBs = parseFloat(CheckNullNumber(TaxAmount)) + parseFloat(CheckNullNumber(NewAssValueInBs.toFixed(ValDigit)));
//    $("#TotalInvAmtBs").val(parseFloat(TotalInvAmtBs).toFixed(ValDigit));
//}

/*code wrote by Suraj on 31-07-2024 */
function OnChangeAssValueSpec(e) {
    
    var CRow = $(e.target).closest("tr");
    let conv_rate = $("#conv_rate").val();
   
    let RecQty = CRow.find("#TxtReceivedQuantity").val();
    let AssValSp = parseFloat(CheckNullNumber(CRow.find("#TxtAssessableValueinSpec").val())).toFixed(ValDecDigit);
    let AssRate = parseFloat(parseFloat(AssValSp) / parseFloat(RecQty)).toFixed(ValDecDigit);
    let AssValBase = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(CheckNullNumber(AssValSp)), parseFloat(CheckNullNumber(conv_rate)))), ValDecDigit)).toFixed(ValDecDigit);

    CRow.find("#TxtAssRate").val(parseFloat(getvalwithoutroundoff(AssRate, RateDecDigit)).toFixed(RateDecDigit));
    CRow.find("#TxtAssessableValueinSpec").val(parseFloat(getvalwithoutroundoff(AssValSp, ValDecDigit)).toFixed(ValDecDigit));
    if (parseFloat(CheckNullNumber(AssRate)) > 0) {
        CRow.find("#TxtAssessableValueinSpec").css("border-color", "#ced4da");
        CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").css("display", "none");
        CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").text("");

        CRow.find("#TxtAssessableValue").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        //CRow.find("#TxtItemGrossValueInBase").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        //CRow.find("#TxtAssessableValueinSpec").val(parseFloat(getvalwithoutroundoff(AssValSp, ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#InvoiceAmount").val(parseFloat(getvalwithoutroundoff(AssValBase, ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#InvoiceAmountInSpec").val(parseFloat(getvalwithoutroundoff(AssValSp, ValDecDigit)).toFixed(ValDecDigit));
        //let TxtAssRate = CRow.find("#TxtAssRate").val();
        CalculateAmount();
        if (CRow.find("#ManualGST").is(":checked")) {
            var item_tax_amt = CRow.find("#Txtitem_tax_amt").val()
            if (parseFloat(CheckNullNumber(item_tax_amt)) > 0) {
                ResetManualGST(CRow);
            }
        } else {
            OnClickTaxExemptedCheckBox(e);
        }
    } else {
        CRow.find("#TxtAssessableValueinSpec").css("border-color", "red");
        CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").css("display", "block");
        CRow.find("#SpanTxtAssessableValueinSpecErrorMsg").text($("#valueReq").text());
        return false;
    }
}
function Fn_AfterTaxChangeUpdateNetValueItemWise(currentRow, total_tax_val, ConvRate) {
    /* Created By : Suraj Maurya, On : 30-11-2024, Purpose : To Update Row Data after tax change */
    if (!currentRow.find("#TaxExempted").is(":checked")) {
        currentRow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(total_tax_val)).toFixed(cmn_ValDecDigit));
        let ItemAssVal = parseFloat(CheckNullNumber(currentRow.find("#TxtAssessableValue").val()));
        let ItemNetVal = ItemAssVal + parseFloat(CheckNullNumber(total_tax_val));
        currentRow.find("#InvoiceAmount").val(ItemNetVal.toFixed(cmn_ValDecDigit));
        let InvAmtInSp = ItemNetVal / IsNull(ConvRate, '1');
        currentRow.find("#InvoiceAmountInSpec").val(InvAmtInSp.toFixed(cmn_ValDecDigit));
    }

}

function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SInvItmDetailsTbl", [{ "FieldId": "TxtItemName", "FieldType": "input"}]);
}


