/************************************************
Javascript Name:Domestic Sales Invoice Detail
Created By:Hina Sharma
Created Date: 07-10-2022
Description: This Javascript use for the Domestic Sales Invoice Detail many function

Modified By:
Modified Date:
Description:

*************************************************/
var RateDecDigit = $("#RateDigit").text();
var ValDecDigit = $("#ValDigit").text();
var ValDigit = $("#ValDigit").text();
var DecDigit = $("#ValDigit").text();
var QtyDecDigit = $("#QtyDigit").text();
var ExchDecDigit = $("#ExchDigit").text();
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
    $("#dispatch_State").select2();
    $("#dispatch_District").select2();
    $("#dispatch_City").select2();
    $("#ddlBankName").select2();
    BindCustomerList();
    BindDDlSIList();
    BindSalesPersonList();
    if ($("#nontaxable").is(":checked")) {
        $("#SInvItmDetailsTbl > tbody > tr ").each(function () {
            var currentRow = $(this);
            currentRow.find("#TaxExempted").attr("disabled", true);
            currentRow.find("#ManualGST").attr("disabled", true);
            currentRow.find("#BtnTxtCalculation").prop("disabled", true);
            currentRow.find('#ManualGST').prop("checked", false);
        });
    }
    $("#SpanCustNameErrorMsg").css("display", "none");
    $("#Cust_NameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "#ced4da");
    var InvoiceNumber = $("#InvoiceNumber").val();
    $("#hdDoc_No").val(InvoiceNumber);
    BindPortOfLoading();
    BindDDLAccountList();
    getRoundofValue();

    if ($("#ForDisableOCDlt").val() == "Disable") {

    }
    else {
        var Perc = parseFloat(CheckNullNumber($("#OrderDiscountInPercentage").val()));
        if (Perc > 0) {
            $("#OrderDiscountInAmount").attr("disabled", true);
            $("#OrderDiscountInPercentage").attr("disabled", false);
        }
        var Amt = parseFloat(CheckNullNumber($("#OrderDiscountInAmount").val()));
        if (Amt > 0) {
            $("#OrderDiscountInPercentage").attr("disabled", true);
            $("#OrderDiscountInAmount").attr("disabled", false);
        }
        if (Perc == 0 && Amt == 0) {
            $("#OrderDiscountInPercentage").attr("disabled", false);
            $("#OrderDiscountInAmount").attr("disabled", false);
        }
    }
    CancelledRemarks("#Cancelled", "Disabled");
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId != "105103145125") {
        if ($("#rpt_id").text() != "0") {
            $("#ddlSalesPerson").attr('disabled', true);
        }
    }
    Cmn_ChangeValueReplicate("NetOrderValueInBase", "#PaymentscheduleInvoiceAmount");
    Cmn_RecalPaymScheTotals();
});
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
        url: "/ApplicationLayer/LocalSaleInvoice/GetDistrictOnState",
       
        dataType: "json",
        //async: true,
        data: { ddlStateID: ddlStateID, },
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }        
            debugger;
            if (data !== null && data !== "")
            {
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
        url: "/ApplicationLayer/LocalSaleInvoice/GetCityOnDistrict",
       
        dataType: "json",
        //async: true,
        data: { ddlDistrictID: ddlDistrictID },
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
          
            debugger;
            if (data !== null && data !== "")  {
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
function getRoundofValue() {
    if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
        var grossval = $("#TxtGrossValue").val();
        var taxval = $("#TxtTaxAmount").val();
        //var ocval = CheckNullNumber($("#TxtOtherCharges").val());
        var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());

        var finalnetval = parseFloat(getvalwithoutroundoff((parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval)), ValDecDigit)).toFixed(ValDecDigit);

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
                    /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                    var FRoundOffAmt = parseFloat(faddval);
                    $("#hdn_FRoundOffAmt").val(FRoundOffAmt);

                    //let valInBase = $("#TxtGrossValue").val();
                    //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                }
                if ($("#m_round").is(":checked")) {
                    //var finnetval = netval.split('.');
                    var decval = '0.' + finnetval[1];
                    fnetval = parseFloat(netval) - parseFloat(decval);
                    $("#pm_flagval").val($("#m_round").val());
                    /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                    var FRoundOffAmt = parseFloat(decval);
                    $("#hdn_FRoundOffAmt").val(FRoundOffAmt);

                    //let valInBase = $("#TxtGrossValue").val();
                    //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                }
                //var roundoff_netval = Math.round(fnetval);
                //var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                //$("#NetOrderValueInBase").val(f_netval);
                //$("#NetOrderValueSpe").val(f_netval);
                //GetAllGLID("RO");
            }
            else {
                /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                $("#hdn_FRoundOffAmt").val("");
            }
        }
    }
}
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
            hideLoader();
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
            hideLoader();
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
    CancelledRemarks("#Cancelled", "Enable");
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
/*-------------List Page Functionality----------------- */

function BindCustomerList() {
    debugger;
    var DocID = $("#DocumentMenuId").val();
    $("#Cust_NameList").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPage: params.page,
                    DocumentMenuId: DocID,
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
        var CustId = $("#Cust_NameList option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalSaleInvoice/SearchSI_Detail",
            data: {
                CustId: CustId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodySIList').html(data);
                hideLoader();
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
                hideLoader();
            }
        });
    } catch (err) {
        debugger;
        console.log("SI Error : " + err.message);
        hideLoader();

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
function BindSalesPersonList() {
    // debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $("#salesperson").select2({
        ajax: {
            url: $("#salespersonList").val(),
            data: function (params) {
                var queryParameters = {
                    SalePerson: params.term,
                    BrchID: Branch
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
function BindDDlSIList() {
    var DocID = $("#DocumentMenuId").val();
    $("#Cust_NameList").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPage: params.page,
                    DocumentMenuId: DocID
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
    try {
        var Dmenu = $("#DocumentMenuId").val();

        var btn = $("#hdnsavebtn").val();
        if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
            $("#btn_save").attr("disabled", true);
            $("#btn_save").css("filter", "grayscale(100%)");
            return false;
        }
        var INSDTransType = sessionStorage.getItem("INSTransType");
        $("#BankAddress").css("display", "block");
        var as111 = $("#BankAddress").val();
        var accnum = $("#AccountNumber").val();
        var shft_cd = $("#SWIFT_Code").val();
        var ifsccode = $("#IFSC_Code").val();
        $("#hdnbankadd").val(as111);
        $("#hdnacc_no").val(accnum);
        $("#hdnshiftid").val(shft_cd);
        $("#hdnifccode").val(ifsccode);
        $("#AccountNumber").css("display", "block");
        $("#SWIFT_Code").css("display", "block");
        $("#IFSC_Code").css("display", "block");
        $("#ddlSalesPerson").attr("Disabled", false);

        if (CheckSI_Validations() == false) {
            return false;
        }
        if (CheckSI_ItemValidations() == false) {

            return false;
        }
        if ($("#Cancelled").is(':checked')) {

        } else {
            if (Dmenu == "105103140") {
                var ValidDispatchDetail = ValidationDispatchDetail();
                if (ValidDispatchDetail == false) {
                    return false;
                }
            }    
        }
       
        if (Dmenu == "105103145125") {
            if (ValidateCommercialInvoiceAmountBeforeSubmit() == false) {
                return false;
            }
        }
        var status = $("#hfSIStatus").val();
        if (Dmenu == "105103140" && status != "C") {
            if (ValidateSaleInvAmountBeforeSubmit() == false) {
                return false;
            }
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y" && Dmenu == "105103140") {
            if (Cmn_taxVallidation("SInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
                return false;
            }
        }
      
        if (CheckSI_VoucherValidations() == false) {
            return false;
        }
        if (Cmn_ValidationPaymentSchedule() == false) {
            /*Added by NItesh 22012026_1236*/
            return false;
        }
        //To Validate Cost Center
        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfSIStatus") == false) {//modified by Suraj Maurya on 04-11-2024
            return false;
        }
        var TransType = "";
        if (INSDTransType === 'Update') {
            TransType = 'Update';
        }
        else {
            TransType = 'Save';
        }
        debugger;
        $("#hdnVouMsg").val($("#CreditNotePassAgainstInv").text());

        var FinalSI_ItemDetail = [];
        var FinalSI_VouDetail = [];

        FinalSI_ItemDetail = GetSI_ItemDetails();
        FinalSI_VouDetail = GetSI_VoucherDetails();
        debugger;
        var SIItemDt = JSON.stringify(FinalSI_ItemDetail);
        var SIVouGlDt = JSON.stringify(FinalSI_VouDetail);

        $('#hdItemDetailList').val(SIItemDt);
        $('#hdVouGlDetailList').val(SIVouGlDt);
        GetSI_TaxDetails();
        GetSI_OCTaxDetails();

        var OCDetail = [];
        OCDetail = GetSI_OtherChargeDetails();
        var strOC = JSON.stringify(OCDetail);
        $("#hdOCDetailList").val(strOC);
        /*Add by Hina on 10-07-2024 for tds on OC */
        var Final_OC_TdsDetails = [];
        Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
        var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
        $('#hdn_oc_tds_details').val(Oc_Tds_Details);

        /*Added by Suraj Maurya on 24-01-2025 for TCS*/
        var Final_OC_TcsDetails = [];
        Final_OC_TcsDetails = Cmn_Insert_Tcs_Details();
        var Oc_Tcs_Details = JSON.stringify(Final_OC_TcsDetails);
        $('#hdn_tcs_details').val(Oc_Tcs_Details);
        /*-----------Sub-item-------------*/

        var SubItemsListArr = Cmn_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);

        Cmn_BindPaymentSchedule();

        var FinalCostCntrDetails = [];
        FinalCostCntrDetails = Cmn_InsertCCDetails();
        var CCDetails = JSON.stringify(FinalCostCntrDetails);
        $('#hdn_CC_DetailList').val(CCDetails);

        /*-----------Sub-item end-------------*/
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
        /*----- Attatchment End--------*/
        /*----- Attatchment start--------*/
        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        var Custname = $('#Cust_NameList option:selected').text();
        $("#hd_SICustName").val(Custname);
        var ShipNum = $('#ddlShipmentNo option:selected').text();
        $("#hdn_ShipNum").val(ShipNum);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        //$("#CustomInvDate").attr("Disabled", false);
        return true;
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }


};
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
   // if (customer_address != "") {
        document.getElementById("vmAddress1").innerHTML = "";
        $("#Dispatch_address1").css('border-color', '#ced4da');
        $("#vmAddress1").attr("style", "display: none;");
        //document.getElementById("vmCustCityname").innerHTML = "";
        //$("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
        //$("#vmCustCityname").attr("style", "display: none;");
      
    //}
    //else {
    //    document.getElementById("vmAddress1").innerHTML = $("#valueReq").text();
    //    $("#Dispatch_address1").css('border-color', 'red');
    //    $("#vmAddress1").attr("style", "display: block;");

    //}
    
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
function CheckSI_Validations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#Cust_NameList").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#Cust_NameList").css("border-color", "Red");
        $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("#Cust_NameList").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "#ced4da");
    }
    if (parseFloat(CheckNullNumber($("#conv_rate").val())) == 0) {
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
    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

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
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        RateDecDigit = $("#ExpImpRateDigit").text();///Rate And Percentage
        ValDecDigit = $("#ExpImpValDigit").text();///Amount
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
        RateDecDigit = $("#RateDigit").text();///Rate And Percentage
        ValDecDigit = $("#ValDigit").text();///Amount
    }

    var ErrorFlag = "N";
    if ($("#SInvItmDetailsTbl >tbody >tr").length > 0) {
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            var row = $(this);
            var TxtInvoiceRate = row.find("#TxtInvoiceRate").val();
            var ShQty = row.find("#TxtReceivedQuantity").val();
            var ShFocQty = row.find("#TxtFocQuantity").val();
            var InvRate = row.find("#TxtInvoiceRate").val();
            var Disc = row.find("#item_disc_val").val();
            var FOC = row.find("#HdFocItem").val();
            if (parseFloat(CheckNullNumber(ShQty)) == 0 && parseFloat(CheckNullNumber(ShFocQty)) > 0) {
                FOC = "Y";
            }
            if (FOC != "Y") {
                if (parseFloat(CheckNullNumber(TxtInvoiceRate)) > 0) {
                    row.find("#TxtInvoiceRate").css("border-color", "#ced4da");
                    row.find("#itemInvoiceAmtError").css("display", "none");
                    row.find("#itemInvoiceAmtError").text("");
                    
                    let GrVal = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(CheckNullNumber(ShQty)), parseFloat(CheckNullNumber(InvRate))) - parseFloat(CheckNullNumber(Disc))), ValDecDigit)) /*+ parseFloat(CheckNullNumber(OCVal))*/;
                    if (GrVal > 0) {
                        //row.find("#TxtItemGrossValue").val((GrVal).toFixed(ValDecDigit));
                        //row.find("#TxtNetValue").val((GrVal).toFixed(ValDecDigit));
                        //row.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(cmn_getmultival(GrVal, parseFloat(CheckNullNumber(conv_rate))), ValDecDigit)).toFixed(ValDecDigit));
                        row.find("#itemInvoiceAmtError").css("display", "none");
                        row.find("#TxtInvoiceRate").css("border-color", "#ced4da");
                        row.find("#itemInvoiceAmtError").text("");

                    } else {
                        row.find("#itemInvoiceAmtError").css("display", "block");
                        row.find("#TxtInvoiceRate").css("border-color", "red");
                        row.find("#itemInvoiceAmtError").text($("#span_InvalidPrice").text());
                        ErrorFlag = "Y";
                    }

                } else {
                    row.find("#TxtInvoiceRate").css("border-color", "red");
                    row.find("#itemInvoiceAmtError").css("display", "block");
                    row.find("#itemInvoiceAmtError").text($("#valueReq").text());
                    ErrorFlag = "Y";
                    CheckZeroError = "Y";
                }
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
    //var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145125") {
    //    ValDigit = $("#ExpImpValDigit").text();
    //}
    //else {
    //    ValDigit = $("#ValDigit").text();
    //}

    //var DrTotal = $("#DrTotalInBase").text();
    //var CrTotal = $("#CrTotalInBase").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {

    //    var currentRow = $(this);
    //    var rowid = currentRow.find("#SNohf").val();
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
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
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
//function CheckSI_VoucherValidations() {
//    debugger;
//    var ErrorFlag = "N";
//    var ValDigit = $("#ValDigit").text();
//    var DrTotal = $("#DrTotal").text();
//    var CrTotal = $("#CrTotal").text();
//    debugger;
//    if (DrTotal == '' || DrTotal == 'NaN') {
//        DrTotal = 0;
//    }
//    if (CrTotal == '' || CrTotal == 'NaN') {
//        CrTotal = 0;
//    }
//    debugger;
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

//    if (ErrorFlag == "Y") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function GetSI_ItemDetails() {
    var SI_ItemsDetail = [];
    $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var ship_no = "";
        var ship_date = "";
        var item_id = "";
        var item_name = "";
        var uom_id = "";
        var uom_name = "";
        var sub_item = "";
        var ship_qty = "";
        var ship_foc_qty = "";
        var mrp = "";
        var mrp_disc = "";
        var item_rate = "";
        var item_inv_rate = "";
        var item_disc_perc = "";
        var item_disc_amt = "";
        var item_gr_val = "";
        var item_tax_amt = "";
        var item_oc_amt = "";
        var item_net_val_spec = "";
        var item_net_val_bs = "";
        var gl_vou_no = null;
        var gl_vou_dt = null;
        var TaxExempted = "";
        var ManualGST = "";
        var hsn_code = "";
        var item_disc_val = "";
        var item_acc_id = "";
        var it_remarks = "";
        var foc = "";
        var currentRow = $(this);
        ship_no = currentRow.find("#TxtShipmentNo").val();
        ship_date = currentRow.find("#hfShipmentDate").val();
        item_id = currentRow.find("#hfItemID").val();
        item_name = currentRow.find("#TxtItemName").val();
        uom_id = currentRow.find("#hfUOMID").val();
        uom_name = currentRow.find("#TxtUOM").val();
        sub_item = "";
        //if (sub_item == "undefined") {
        //    sub_item = "";
        //}
        ship_qty = currentRow.find("#TxtReceivedQuantity").val();
        ship_foc_qty = currentRow.find("#TxtFocQuantity").val() || "0";
        mrp = currentRow.find("#MRP").val();
        mrp_disc = currentRow.find("#MRPDiscount").val();

        item_rate = currentRow.find("#TxtRate").val();
        item_inv_rate = currentRow.find("#TxtInvoiceRate").val();

        item_disc_perc = currentRow.find("#item_disc_perc").val();
        item_disc_amt = currentRow.find("#item_disc_amt").val();
        item_gr_val = currentRow.find("#TxtItemGrossValue").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        hsn_code = currentRow.find("#ItemHsnCode").val();
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        item_net_val_spec = currentRow.find("#TxtNetValue").val();
        item_net_val_bs = currentRow.find("#TxtNetValueInBase").val();
        gl_vou_no = null;
        gl_vou_dt = null;
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
            foc = "Y"
        }
        else {
            foc = "N"
        }
        item_disc_val = currentRow.find("#item_disc_val").val();
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        it_remarks = currentRow.find("#txt_SIItemRemarks").val();
        var PackSize = currentRow.find("#PackSize").val();
        SI_ItemsDetail.push({
            ship_no: ship_no, ship_date: ship_date, item_id: item_id, item_name: item_name, uom_id: uom_id
            , uom_name: uom_name, sub_item: sub_item, ship_qty: ship_qty, mrp: mrp, mrp_disc: mrp_disc, item_rate: item_rate, item_inv_rate: item_inv_rate
            , item_disc_perc: item_disc_perc, item_disc_amt: item_disc_amt, item_gr_val: item_gr_val, item_tax_amt: item_tax_amt
            , item_oc_amt: item_oc_amt, item_net_val_spec: item_net_val_spec, item_net_val_bs: item_net_val_bs, gl_vou_no: gl_vou_no
            , gl_vou_dt: gl_vou_dt, TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, item_disc_val: item_disc_val
            , item_acc_id: item_acc_id, it_remarks: it_remarks, foc: foc, PackSize: PackSize, ship_foc_qty: ship_foc_qty
        });
    });
    return SI_ItemsDetail;
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
                        TaxList.ship_no = currentRow.find("#DocNo").text().trim();//currentRow.find("#shipno").text();
                        TaxList.ship_date = currentRow.find("#DocDate").text().trim(); //currentRow.find("#shipdate").text();
                        TaxList.item_id = currentRow.find("#TaxItmCode").text().trim();//currentRow.find("#itemid").text();
                        TaxList.tax_name = currentRow.find("#TaxName").text();
                        TaxList.tax_id = currentRow.find("#TaxNameID").text().trim();//currentRow.find("#taxid").text();
                        TaxList.tax_rate = currentRow.find("#TaxPercentage").text().trim().replace('%', '');//currentRow.find("#taxrate").text().replace('%', '');
                        TaxList.tax_level = currentRow.find("#TaxLevel").text().trim();//currentRow.find("#taxlevel").text();
                        TaxList.tax_apply_name = currentRow.find("#TaxApplyOn").text();
                        TaxList.tax_val = currentRow.find("#TaxAmount").text().trim();//currentRow.find("#taxval").text();
                        TaxList.item_tax_amt = currentRow.find("#TotalTaxAmount").text();
                        TaxList.tax_apply_on = currentRow.find("#TaxApplyOnID").text().trim();// currentRow.find("#taxapplyon").text();

                        ItemTaxList.push(TaxList);
                    });
                }
            }
        }
    });
    var str1 = JSON.stringify(ItemTaxList);
    $("#hdTaxDetailList").val(str1);
};
function GetSI_OCTaxDetails() {
    debugger;
    //var TaxDetails = [];
    var ItemTaxList = new Array();
    var taxrowcount = $('#Hdn_OC_TaxCalculatorTbl tbody tr').length;
    if (taxrowcount > 0) {
        $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var TaxList = {};


            TaxList.ship_no = "";//currentRow.find("#DocNo").text().trim();//currentRow.find("#shipno").text();
            TaxList.ship_date = "";//currentRow.find("#DocDate").text().trim(); //currentRow.find("#shipdate").text();
            TaxList.item_id = currentRow.find("#TaxItmCode").text().trim();//currentRow.find("#itemid").text();
            TaxList.tax_name = currentRow.find("#TaxName").text();
            TaxList.tax_id = currentRow.find("#TaxNameID").text().trim();//currentRow.find("#taxid").text();
            TaxList.tax_rate = currentRow.find("#TaxPercentage").text().trim().replace('%', '');//currentRow.find("#taxrate").text().replace('%', '');
            TaxList.tax_level = currentRow.find("#TaxLevel").text().trim();//currentRow.find("#taxlevel").text();
            TaxList.tax_apply_name = currentRow.find("#TaxApplyOn").text();
            TaxList.tax_val = currentRow.find("#TaxAmount").text().trim();//currentRow.find("#taxval").text();
            TaxList.item_tax_amt = currentRow.find("#TotalTaxAmount").text();
            TaxList.tax_apply_on = currentRow.find("#TaxApplyOnID").text().trim();// currentRow.find("#taxapplyon").text();

            ItemTaxList.push(TaxList);
            //TaxDetails.push(TaxList)
        });

    }
    var str1 = JSON.stringify(ItemTaxList);
    $("#hdOCTaxDetailList").val(str1);
};
function GetSI_OtherChargeDetails() {
    debugger;
    //var SI_OCList = new Array();
    //if ($("#ht_Tbl_OC_Deatils >tbody >tr").length > 0) {
    //    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function () {
    //if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
    //    $("#Tbl_OC_Deatils >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //        var OCList = {};
    //        OCList.OC_ID = currentRow.find("#OCValue").text();
    //        OCList.OCAmtSp = currentRow.find("#OCAmount").text();
    //        OCList.tax_amt = currentRow.find("#OCTaxAmt").text();
    //        OCList.total_amt = currentRow.find("#OCTotalTaxAmt").text();
    //        OCList.OCName = currentRow.find("#OCName").text();
    //        OCList.OC_Curr = currentRow.find("#OCCurr").text();
    //        OCList.OC_Conv = currentRow.find("#OCConv").text();
    //        OCList.OC_AmtBs = currentRow.find("#OcAmtBs").text();
    //        SI_OCList.push(OCList);
    //    });
    //}
    var SI_OCList = [];
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
            let supp_type = currentRow.find("#td_OCSuppType").text(); // Added by Suraj on 12-04-2024
            let bill_no = currentRow.find("#OCBillNo").text(); // Added by Suraj on 12-04-2024
            let bill_date = currentRow.find("#OCBillDt").text()//.split("-"); // Added by Suraj on 12-04-2024
            var doc_id = $("#DocumentMenuId").val();

            if (doc_id == "105103140") {
                var tds_amt = currentRow.find("#OC_TDSAmt").text(); // Added by Hina on 10-07-2024 to chnge for tds on oc
            }
            else {
                var tds_amt = 0; // Added by Hina on 10-07-2024 to chnge for tds on oc
            }
            /* Added by Suraj Maurya on 11-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 11-12-2024 for third party round off */
            //var bill_date1 = bill_date[2] + "-" + bill_date[1] + "-" + bill_date[0];
            SI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName
                , OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, supp_id: supp_id, curr_id: curr_id
                , supp_type: supp_type, bill_no: bill_no, bill_date: bill_date, tds_amt: tds_amt
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    return SI_OCList
};
//function Cmn_Insert_OC_Tds_Details() {
//    debugger;
//    var TDS_Details = [];
//    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
//        debugger;
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
function GetSI_VoucherDetails() {
    debugger;
    var SI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "";
    if ($("#OrderTypeD").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "E";
    }
    var TransType = "Sal";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        debugger
        $("#VoucherDetail >tbody >tr").each(function () {
            debugger;
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
            SI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
        });
    }
    return SI_VouList;
};
//function GetSI_VoucherDetails() {
//    debugger;
//    var SI_VouList = [];
//    var CustVal = 0;
//    var Compid = $("#CompID").text();
//    var _SIType = "";
//    if ($("#OrderTypeD").is(":checked")) {
//        _SIType = "D";
//    }
//    if ($("#OrderTypeE").is(":checked")) {
//        _SIType = "E";
//    }
//    var TransType = "Sal";
//    if ($("#VoucherDetail >tbody >tr").length > 0) {
//        $("#VoucherDetail >tbody >tr").each(function () {
//            var currentRow = $(this);
//            var acc_id = "";
//            var acc_name = "";
//            var dr_amt = "";
//            var cr_amt = "";
//            var Gltype = "";
//            acc_id = currentRow.find("#hfAccID").val();
//            acc_name = currentRow.find("#txthfAccID").val();
//            dr_amt = currentRow.find("#dramt").text();
//            cr_amt = currentRow.find("#cramt").text();
//            Gltype = currentRow.find("#type").val();
//            SI_VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: _SIType, Value: CustVal, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, Gltype: Gltype });

//        });
//    }
//    return SI_VouList;
//};

function OnChangeCustomer(CustID)
{
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
        $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "red");
        $("#txtCurrency").val("");
        $("#conv_rate").val("");

    }
    else
    {
        var Custname = $('#Cust_NameList option:selected').text();
        $("#hd_SICustName").val(Custname);

        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "#ced4da");
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }

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
                            $("#TxtBillingAddr").val(arr.Table[0].BillingAddress);
                            $("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#cust_acc_id").val(arr.Table[0].cust_acc_id);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#txtCurrency").val(arr.Table[0].curr_name);
                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);

                            $("#conv_rate").val(parseFloat(getvalwithoutroundoff(arr.Table[0].conv_rate, ExchDecDigit)).toFixed(ExchDecDigit));
                            $("#hdnconv_rate").val(parseFloat(getvalwithoutroundoff(arr.Table[0].conv_rate, ExchDecDigit)).toFixed(ExchDecDigit));
                            var hdbs_curr = $("#hdbs_curr").val();
                            var hdcurr = $("#hdcurr").val();
                            if (hdbs_curr == hdcurr) {
                                $("#conv_rate").attr("readonly", true);
                            } else {
                                $("#conv_rate").attr("readonly", false);
                            }

                        }
                        else {
                            $("#TxtBillingAddr").val("");
                            $("#TxtShippingAddr").val("");
                            $("#bill_add_id").val("");
                            $("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                            $("#txtCurrency").val("");
                            $("#conv_rate").val("");
                            $("#ConvRate").attr("readonly", false);
                        }
                    }
                    else {
                        $("#txtCurrency").val("");
                        $("#conv_rate").val("");
                    }
                    hideLoader();
                }
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
        hideLoader();
    }
}
function BindShipmentList(Cust_id) {

    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalSaleInvoice/GetShipmentLists",
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
                        $(".Shipment_AddBtn").css("display", "block");

                        $("#SpanShipmentErrorMsg").css("display", "none");
                        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");
                        $("#Shipment_Date").val("");
                    }
                }
                hideLoader();
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
        hideLoader();
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
function OnChangeShipmentNo(Shipment_No) {
    debugger;
    //var flagerror = "";
    //var salpersn = $("#ddlSalesPerson option:selected").val();
    //if (salpersn == "0" || salpersn == "" || salpersn == null) {
    //    $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
    //    $("#SpanSalesPersonErrorMsg").css("display", "block");
    //    //$("[aria-labelledby='select2-ddlSalesPerson-container']").css("border-color", "red");
    //    $("#ddlSalesPerson").css("border-color", "red");
    //    flagerror = "Y";
    //}
    //else {
    var ShipmentNo = Shipment_No.value;
    var ShipmentDate = $('#ddlShipmentNo').select2("data")[0].element.attributes[0].value;
    var ShipmentDP = ShipmentDate.split("-");
    var FShipmentDate = (ShipmentDP[2] + "-" + ShipmentDP[1] + "-" + ShipmentDP[0]);
    if (ShipmentNo == "---Select---") {
        $("#Shipment_Date").val("");
    }
    else {
        var ShipNum = $('#ddlShipmentNo option:selected').text();
        $("#hdn_ShipNum").val(ShipNum);

        $("#Shipment_Date").val(FShipmentDate);
        $("#SpanShipmentErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");

        var doc_id = $("#DocumentMenuId").val();
        var flag = "";
        if (doc_id == "105103140") {
            flag = "domestic";
        }
        else {
            flag = "export";
        }
        ////var flag=
        // if (doc_id == "105103145125") {
        //     try {
        //         $.ajax(
        //             {
        //                 type: "Post",
        //                 url: "/ApplicationLayer/LocalSaleInvoice/Getcurr_details",
        //                 data: {
        //                     ship_no: ShipNum,
        //                     ship_date: FShipmentDate,
        //                     flag: flag
        //                 },
        //                 success: function (data) {
        //                     debugger;
        //                     if (data !== null && data !== "") {
        //                         var arr = [];
        //                         arr = JSON.parse(data);
        //                         if (arr.length > 0) {

        //                             $("#hdcurr").val(arr[0].curr_id);
        //                             $("#txtCurrency").val(arr[0].curr_name);
        //                             $("#conv_rate").val(parseFloat(arr[0].conv_rate).toFixed(ExchDecDigit));
        //                             $("#hdnconv_rate").val(parseFloat(arr[0].conv_rate).toFixed(ExchDecDigit));

        //                             if (arr[0].bcurrflag == "Y") {
        //                                 $("#conv_rate").attr("readonly", true);
        //                             }
        //                             else {
        //                                 $("#conv_rate").attr("readonly", false);
        //                             }

        //                         }
        //                         else {
        //                             $("#hdcurr").val("");
        //                             $("#txtCurrency").val("");
        //                             $("#conv_rate").val("");
        //                             $("#hdnconv_rate").val("");
        //                             $("#conv_rate").attr("readonly", true);
        //                         }
        //                     }
        //                     else {
        //                         $("#hdcurr").val("");
        //                         $("#txtCurrency").val("");
        //                         $("#conv_rate").val("");
        //                         $("#hdnconv_rate").val("");
        //                         $("#conv_rate").attr("readonly", true);
        //                     }
        //                 },
        //                 error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                     debugger;
        //                 }
        //             });

        //     } catch (err) {
        //         console.log("UserValidate Error : " + err.message);
        //     }
        // }
        // else
        // {
        try {
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/LocalSaleInvoice/Getcurr_details",
                    data: {
                        ship_no: ShipNum,
                        ship_date: FShipmentDate,
                        flag: flag
                    },
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.length > 0) {
                                //if (arr.Table[1].length < 0) {
                                var Empl_Id = arr[0].EMP_ID;
                                var cust_ref_no = arr[0].cust_ref_no;
                                if (Empl_Id != null && Empl_Id != "0") {
                                    var salpersn = $("#ddlSalesPerson option:selected").val();
                                    if (salpersn == "0" || salpersn == "" || salpersn == null) {
                                        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
                                        $("#SpanSalesPersonErrorMsg").css("display", "block");
                                        //$("[aria-labelledby='select2-ddlSalesPerson-container']").css("border-color", "red");
                                        $("#ddlSalesPerson").css("border-color", "red");
                                        flagerror = "Y";
                                    }
                                }
                                if (cust_ref_no != null && cust_ref_no != "0" && cust_ref_no != "") {
                                    var Custome_Reference = $("#ddlCustome_Reference").text();
                                    if (Custome_Reference == "0" || Custome_Reference == "" || Custome_Reference == null) {
                                        $("#ddlCustome_Reference").text(cust_ref_no)
                                    }
                                }

                                //if (Empl_Id != null && Empl_Id != "0") {
                                //    /*$("#ddlSalesPerson").val(Empl_Id).trigger('change');*/
                                //    $("#ddlSalesPerson").attr("Disabled", "Disabled");
                                //}
                                //else {
                                //    $("#ddlSalesPerson").val("0").trigger('change');
                                //    $("#ddlSalesPerson").attr("Disabled", false);
                                //}
                                //$("#CustomInvDate").val(arr[0].custom_inv_dt);
                                // }
                            }
                        }
                        hideLoader();
                    }
                })
        }
        catch (err) {
            console.log("UserValidate Error : " + err.message);
            hideLoader();
        }


        //}
    }

    //}
    //if (flagerror == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}


}
function OnClickAddButton() { //Modied by Suraj on 07-10-2024 for issue in performance
    //debugger;
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    var Inv_type;
    if ($("#OrderTypeD").is(":checked")) {
        Inv_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Inv_type = "E";
    }
    var ShipmentNo = $('#ddlShipmentNo').val();
    var ShipmentDate = $('#Shipment_Date').val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
        RateDecDigit = $("#ExpImpRateDigit").text();///Rate And Percentage
        ValDecDigit = $("#ExpImpValDigit").text();///Amount
    }
    else {
        QtyDecDigit = $("#QtyDigit").text();///Quantity
        RateDecDigit = $("#RateDigit").text();///Rate And Percentage
        ValDecDigit = $("#ValDigit").text();///Amount
    }

    var SaleParson = $("#ddlSalesPerson").val();
    var Cust_id = $("#Cust_NameList").val();
    if (Cust_id == "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#SpanCustNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "red");
        $("#txtCurrency").val("");
        $("#conv_rate").val("");

    }
    else {
        if (SaleParson == "0" || SaleParson == "" || SaleParson == null) {
            $("#ddlSalesPerson").css("border-color", "red");
            $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
            $('[aria-labelledby="select2-ddlSalesPerson-container"]').css("border-color", "Red");
            $("#SpanSalesPersonErrorMsg").css("display", "block");
        }
        else {


            if (ShipmentNo == "---Select---" || ShipmentNo == "0") {
                $('#SpanShipmentErrorMsg').text($("#valueReq").text());
                $("#SpanShipmentErrorMsg").css("display", "block");
                $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "red");
                $("#ddlShipmentNo").css("border-color", "red");
            }
            else {
                if (parseFloat(CheckNullNumber($("#conv_rate").val())) == 0) {
                    $('#SpanExRateErrorMsg').text($("#valueReq").text());
                    $("#conv_rate").css("border-color", "Red");
                    $("SpanExRateErrorMsg").css("border-color", "red");
                    $("#SpanExRateErrorMsg").css("display", "block");
                    return false;
                }
                else {
                    $("#SpanExRateErrorMsg").css("display", "none");
                    $("#conv_rate").css("border-color", "#ced4da");
                    $("SpanExRateErrorMsg").css("border-color", "#ced4da");
                }
                $("#SpanShipmentErrorMsg").css("display", "none");
                $("[aria-labelledby='select2-ddlShipmentNo-container']").css("border-color", "#ced4da");
                $("#Cust_NameList").attr("disabled", "disabled");
                // $("#ddlSalesPerson").attr("disabled", "disabled");
                $("#ddlShipmentNo").css("border-color", "#ced4da");
                //$("#conv_rate").attr("disabled", "disabled");
                $("#Shipment_AddBtn").css("display", "none");
                if (Inv_type == "E") {
                    $("#ddlShipmentNo").attr("disabled", "disabled");
                    $("#Shipment_AddBtn").css("display", "none");
                }
                showLoader();
                try {
                    $.ajax({
                        type: "POST",
                        url: "/ApplicationLayer/LocalSaleInvoice/GetShipmentDetails",
                        data: { ShipmentNo: ShipmentNo, ShipmentDate: ShipmentDate, Inv_type: Inv_type },
                        success: function (data) {
                            //debugger;
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
                                let PgdisableFlag = $("#DisableSubItem").val();
                                var Pgdisabled = "";
                                var invratedisabled = "";
                                if (PgdisableFlag == "Y") {
                                    Pgdisabled = "disabled";
                                }
                                //if (DocMenuId == "105103145125") {
                                var inv_rate_flag = arr.Table[0].flag_invrate;
                                if (inv_rate_flag == "Y") {
                                    if (PgdisableFlag == "Y") {
                                        invratedisabled = "disabled";
                                    }
                                    else {
                                        if (DocMenuId == "105103140") {
                                            invratedisabled = "disabled";
                                        }
                                        else {
                                            invratedisabled = "";
                                        }
                                    }

                                }
                                else {
                                    invratedisabled = "disabled";
                                }
                                //}
                                //else {
                                //invratedisabled = "disabled";
                                //}
                                var Disable = "";
                                if (arr.Table.length > 0) {
                                    var rowIdx = 0;
                                    var TatalTaxAmt_NonGst = 0;
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        debugger;
                                        var subitmDisable = "";
                                        if (arr.Table[k].sub_item != "Y") {
                                            subitmDisable = "disabled";
                                        }
                                        var FocFlag = "";
                                        var FOC = "";
                                        var FOCQty = "";
                                        if (arr.Table[k].foc == "Y") {
                                            FocFlag = "disabled";
                                        }
                                        if (arr.Table[k].state_code == $("#Ship_StateCode").val()) {
                                            $("#Hd_GstType").val("Both")
                                        } else {
                                            $("#Hd_GstType").val("IGST")
                                        }
                                        debugger
                                        var pack_size = "";
                                        if (arr.Table[k].pack_size != null) {
                                            pack_size = arr.Table[k].pack_size;
                                        }
                                        var BaseVal;
                                        BaseVal = parseFloat(getvalwithoutroundoff((cmn_getmultival(getvalwithoutroundoff(ConvRate, ValDecDigit), getvalwithoutroundoff(arr.Table[k].item_gross_val, ValDecDigit))), ValDecDigit)).toFixed(ValDecDigit);
                                        var S_NO = $('#SInvItmDetailsTbl tbody tr').length + 1;
                                        var mrptd = "";
                                        if (DocMenuId != "105103145125") {
                                            mrptd = `<td><input id="MRP" class="form-control num_right" autocomplete="" type="text" name="MRP" placeholder="0000:00" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].mrp, QtyDecDigit)).toFixed(QtyDecDigit)}" disabled></td>
                                    <td><input id="MRPDiscount" class="form-control num_right" autocomplete="" type="text" name="MRPDiscount" placeholder="0000:00" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].mrp_disc, QtyDecDigit)).toFixed(QtyDecDigit)}" disabled></td>`
                                        }
                                        var GstApplicable = $("#Hdn_GstApplicable").text();
                                        var ManualGst = "";
                                        var TaxExempted = "";
                                       
                                        if (DocMenuId != "105103145125") {
                                            if ($("#nontaxable").is(":checked")) {
                                                Disable = "disabled";
                                               
                                                FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" checked disabled class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                                FOCQty = `<td>
                                                    <div class="col-sm-10 lpo_form no-padding">
                                                    <input id="TxtFocQuantity" class="form-control num_right" autocomplete="" type="text" name="TxtFocQuantity" value="${parseFloat(CheckNullNumber(arr.Table[k].ship_foc_qty)).toFixed(QtyDecDigit)}" placeholder="0000:00" disabled>
                                                    </div>
                                                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShippedFocQty">
                                                    <button type="button" id="SubItemShippedFocQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ShippedFoc',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                    </div>
                                                    </td>`
                                                    if (GstApplicable == "Y") {
                                                        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                                    }
                                                    TaxExempted = ' <td><div class="custom-control custom-switch sample_issue"><input type="checkbox" checked disabled class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                              
                                            }
                                            else {
                                                FOCQty = `<td>
                                                    <div class="col-sm-10 lpo_form no-padding">
                                                    <input id="TxtFocQuantity" class="form-control num_right" autocomplete="" type="text" name="TxtFocQuantity" value="${parseFloat(CheckNullNumber(arr.Table[k].ship_foc_qty)).toFixed(QtyDecDigit)}" placeholder="0000:00" disabled>
                                                    </div>
                                                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShippedFocQty">
                                                    <button type="button" id="SubItemShippedFocQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ShippedFoc',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                    </div>
                                                    </td>`
                                                if (arr.Table[k].foc == "Y") {
                                                    FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" checked disabled class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                                  
                                                    if (GstApplicable == "Y") {
                                                        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                                    }
                                                    TaxExempted = ' <td><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                                }
                                                else {
                                                    FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                                    if (GstApplicable == "Y") {
                                                        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" ' + (parseFloat(CheckNullNumber(arr.Table[k].ship_qty))>0 ? "" : "disabled")+' class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                                    }
                                                    TaxExempted = ' <td><div class="custom-control custom-switch sample_issue"><input type="checkbox"  class="custom-control-input margin-switch" ' + (parseFloat(CheckNullNumber(arr.Table[k].ship_qty)) > 0 ? "" : "disabled")+' onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                                }
                                                //if (GstApplicable == "Y") {
                                                //    ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                                //}
                                                //TaxExempted = ' <td><div class="custom-control custom-switch sample_issue"><input type="checkbox"  class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
                                            }
                                        }
                                        var DisableForDomestic = DocMenuId == "105103140" ? "style='display:none;'" : "";
                                        // else {
                                        //    mrptd = `<td><input id="MRP" hidden class="form-control num_right" autocomplete="" type="text" name="MRP" placeholder="0000:00" value="${parseFloat(0).toFixed(QtyDecDigit)}" disabled></td>
                                        //    <td><input id="MRPDiscount" hidden class="form-control num_right" autocomplete="" type="text" name="MRPDiscount" placeholder="0000:00" value="${parseFloat(0).toFixed(QtyDecDigit)}" disabled></td>`
                                        //}
                                        var uom = arr.Table[k].uom_alias;
                                        if (uom == null) {
                                            uom = "";
                                        }
                                        var TxtRate=""
                                        if (DocMenuId == "105103145125") {
                                            TxtRate = `<td><input id="TxtRate" class="form-control num_right" type="text" name="Rate" placeholder="0000:00" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_rate, RateDecDigit)).toFixed(RateDecDigit)}" disabled></td>`
                                        }
                                        else {
                                            TxtRate = `<td style="display:none"><input id="TxtRate" class="form-control num_right" type="text" name="Rate" placeholder="0000:00" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_rate, RateDecDigit)).toFixed(RateDecDigit)}" disabled></td>`
                                        }
                                        $('#SInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${S_NO}</td>
                                    <td style="display: none;"><input id="TxtShipmentNo" class="form-control" type="text" value="${arr.Table[k].ship_no}" name="ShipmentNo" disabled></td>
                                    <td style="display: none;"><input id="TxtShipmentDate" class="form-control" type="text" value="${arr.Table[k].ship_dt}" name="ShipmentNo" disabled><input  type="hidden" id="hfShipmentDate" value="${arr.Table[k].ship_date}" /></td>
                                    <td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-10 no-padding"><input id="TxtItemName" class="form-control time" type="text" name="" disabled>
                                        <input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                        <input  type="hidden" id="hdn_item_gl_acc" value="${IsNull(arr.Table[k].item_acc_id, '')}" />
                                        <input type="hidden" id="tblsr_no" value="${S_NO}" style="display: none;" />
<input type="hidden" id="HdFocItem" value="${arr.Table[k].foc}" style="display: none;" />
                                        </div>
                                    <div class=" col-sm-1 i_Icon">
                                    <button type="button" class="calculator item_pop" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div>
                                    <div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div></td>
                                    <td><input id="TxtUOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" value="${uom}" disabled><input  type="hidden" id="hfUOMID" value="${arr.Table[k].uom_id}" /><input id="ItemHsnCode" value="${arr.Table[k].hsn_code}" type="hidden" /></td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="${pack_size}" onmouseover="OnMouseOver(this)" disabled>
                                                                            </td>
<td>
                                    <div class="col-sm-10 lpo_form no-padding">
                                    <input id="TxtReceivedQuantity" class="form-control num_right" type="text" name="ReceivedQuantity" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].ship_qty, QtyDecDigit)).toFixed(QtyDecDigit)}" placeholder="0000:00" disabled>
                                    </div>
                                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShippedQty" >
                                    <button type="button" id="SubItemShippedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Shipped',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                    </div>
                                    </td>
                                   `+ FOCQty + `
                                   `+ mrptd + `
                                    `+ TxtRate+`
                                    <td><div class=" col-sm-12 lpo_form no-padding"><input id="TxtInvoiceRate" ${FocFlag} ${invratedisabled} onchange="OnchangeInvoiceRate(event)" ${ parseFloat(CheckNullNumber(arr.Table[k].ship_qty)) > 0 ? "" : "disabled"} onkeypress="return RateFloatValueonly(this,event)" autocomplete="off" class="form-control num_right" type="text" name="Rate" placeholder="0000:00" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_rate, RateDecDigit)).toFixed(RateDecDigit)}" ><span id="itemInvoiceAmtError" class="error-message is-visible"></span></div></td>
                                    <td hidden><input id="item_disc_perc" class="form-control date num_right" readonly="" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_disc_perc, 2)).toFixed(2)}" onchange="OnChangeSOItemDiscountPerc(event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc" placeholder="0000.00"></td>
                                    <td hidden><div class="lpo_form"><input id="item_disc_amt" class="form-control date num_right" disabled="" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_disc_amt, ValDecDigit)).toFixed(ValDecDigit)}" onchange="OnChangeSOItemDiscountAmt(event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt" placeholder="0000.00"><span id="item_disc_amtError" class="error-message is-visible"></span></div></td>
                                    <td><div class="lpo_form"><input id="item_disc_val" class="form-control num_right" autocomplete="off" type="text" name="item_disc_val" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].disc_amt, ValDecDigit)).toFixed(ValDecDigit)}"  placeholder="0000.00"   disabled><span id="item_disc_valError" class="error-message is-visible"></span></div></td>
                                    <td><input id="TxtItemGrossValue" class="form-control num_right" type="text" name="GrossValue" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_gross_val, ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    `+ FOC +`
                                    `+ TaxExempted + `
                                    `+ ManualGst + `
                                    <td><div class=" col-sm-9 no-padding"><input id="Txtitem_tax_amt" class="form-control num_right" type="text" name="item_tax_amt" placeholder="0000:00" disabled></div><div class=" col-sm-3 no-padding"><button type="button" class="calculator item_pop" id="BtnTxtCalculation" ${Disable} data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button></div></td>
                                    <td><input id="TxtOtherCharge" class="form-control num_right" type="text" name="OtherCharge" placeholder="0000:00" disabled></td>
                                    <td ${DisableForDomestic}><input id="TxtNetValue" class="form-control num_right" type="text" name="NetValue" value="${parseFloat(getvalwithoutroundoff(arr.Table[k].item_gross_val, ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td><input id="TxtNetValueInBase" class="form-control num_right" type="text" name="NetValueInBase" value="${parseFloat(getvalwithoutroundoff(BaseVal, ValDecDigit)).toFixed(ValDecDigit)}" placeholder="0000:00" disabled></td>
                                    <td><textarea id="txt_SIItemRemarks"  class="form-control remarksmessage" maxlength="1500" onmouseover="OnMouseOver(this)" autocomplete="off" type="text" name="ItemRemarks"  placeholder="Remarks">${$("#span_remarks").val()}${arr.Table[k].it_remarks}</textarea></td>

</tr>`);
                                        var clickedrow = $("#SInvItmDetailsTbl >tbody >tr #tblsr_no[value=" + S_NO + "]").closest("tr");
                                        clickedrow.find("#TxtItemName").val(arr.Table[k].item_name);
                                        var Itm_ID = arr.Table[k].item_id;
                                        if (DocMenuId == "105103140") {
                                            var GstApplicable = $("#Hdn_GstApplicable").text();

                                            if (GstApplicable == "Y") {
                                                //Cmn_ApplyGSTToAtable("SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", arr.Table1);
                                            } else {
                                                $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                                if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                                    $("#HdnTaxOn").val("Item");
                                                    $("#TaxCalcItemCode").val(Itm_ID);
                                                    $("#HiddenRowSNo").val(S_NO);
                                                    $("#Tax_AssessableValue").val(arr.Table[k].item_gross_val);
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
                                                        var ItemTaxAmt = AddTaxByHSNCalculation(TaxArr);
                                                        TatalTaxAmt_NonGst = parseFloat(CheckNullNumber(TatalTaxAmt_NonGst)) + parseFloat(CheckNullNumber(ItemTaxAmt));
                                                        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(ItemTaxAmt)).toFixed(ValDecDigit));
                                                        clickedrow.find("#TxtNetValue").val(parseFloat(parseFloat(CheckNullNumber(arr.Table[k].item_gross_val)) + parseFloat(CheckNullNumber(ItemTaxAmt))).toFixed(ValDecDigit));
                                                        clickedrow.find("#TxtNetValueInBase").val(parseFloat(parseFloat(CheckNullNumber(arr.Table[k].item_gross_val)) + parseFloat(CheckNullNumber(ItemTaxAmt))).toFixed(ValDecDigit));
                                                        //OnClickTaxSaveAndExit("Y");
                                                        //var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                                        //Reset_ReOpen_LevelVal(lastLevel);
                                                    }

                                                }
                                            }

                                        }
                                    }


                                    debugger;
                                    if (arr.Table3.length > 0) {
                                        var rowIdx = 0;
                                        $("#hdn_Sub_ItemDetailTbl >tbody>tr").remove();
                                        for (var y = 0; y < arr.Table3.length; y++) {
                                            var ItmId = arr.Table3[y].item_id;
                                            var SubItmId = arr.Table3[y].sub_item_id;
                                            var ShippedQty = arr.Table3[y].shipped_qty;
                                            var ShippedFocQty = arr.Table3[y].shipped_Foc_qty||0;

                                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemQty" value='${ShippedQty}'></td>
                                    <td><input type="text" id="subItemFocQty" value='${ShippedFocQty}'></td>
                                    </tr>`);
                                        }

                                    }
                                    if (arr.Table4.length > 0) {
                                        $("#trade_term").val(arr.Table4[0].trade_term);
                                        $("#PreCarriageBy").val(arr.Table4[0].pre_carr_by);
                                        $("#PlOfReceiptByPreCarrier").val(arr.Table4[0].pi_rcpt_carr).trigger('change');
                                        $("#VesselFlightNumber").val(arr.Table4[0].ves_fli_no);
                                        $("#PortOfLoading").val(arr.Table4[0].loading_port).trigger('change');
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
                                        $("#BuyersOrderNumberAndDate").val(arr.Table4[0].buyer_ord_no_dt);
                                        $("#ExporterAddress").val(arr.Table4[0].exp_addr);
                                        $("#ConsigneeName").val(arr.Table4[0].consig_name);
                                        $("#ConsigneeAddress").val(arr.Table4[0].consig_addr);
                                    }
                                    if (Inv_type == "D") {
                                        if (arr.Table8.length > 0) {
                                            $("#Declaration_1").val(arr.Table8[0].declar_1);
                                            $("#Declaration_2").val(arr.Table8[0].declar_2);
                                            $("#Invoice_Heading").val(arr.Table8[0].inv_heading);
                                            $("#Corporate_Address").val(arr.Table8[0].corp_off_addr);
                                            $("#Invoice_remarks").val(arr.Table8[0].remarks);
                                            $("#PvtMark").val(arr.Table8[0].pvt_mark);
                                        }
                                    }
                                    if (Inv_type == "D") {
                                        ResetShipment_DDL_Detail();
                                    }
                                    if (Inv_type == "E") {
                                        /* $(".Shipment_AddBtn").css("display", "none");*/
                                    }
                                    if (DocMenuId == "105103140") {
                                        var GstApplicable = $("#Hdn_GstApplicable").text();
                                        if (GstApplicable == "Y") {
                                            var gst_number = $("#Ship_Gst_number").val()
                                            //Cmn_OnSaveAddressApplyGST(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "")
                                            if ($("#nontaxable").is(":checked")) {
                                                CalculateAmount();
                                                let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
                                                if (GrossAmt != 0) {
                                                    if (arr.Table7 != null) {
                                                        AddTcsDetails(arr.Table7);
                                                    }
                                                    CalculateAmount();
                                                    GetAllGLID();
                                                }
                                                else {
                                                    GetAllGLID();
                                                }
                                            }
                                            else {
                                                Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "").then(() => {
                                                    CalculateAmount();
                                                    //let SuppId = $("#Cust_NameList").val();
                                                    let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
                                                    if (GrossAmt != 0) {
                                                        if (arr.Table7 != null) {
                                                            AddTcsDetails(arr.Table7);
                                                        }
                                                        CalculateAmount();
                                                        GetAllGLID();
                                                        //AutoTdsApply(SuppId, GrossAmt).then(() => {
                                                        //    CalculateAmount();
                                                        //    GetAllGLID();
                                                        //});
                                                    }
                                                    else {
                                                        GetAllGLID();
                                                    }
                                                    //GetAllGLID();
                                                }).catch((ex) => console.log(ex));
                                            }
                                            
                                        } else {
                                            if ($("#nontaxable").is(":checked")) {
                                                let SuppId = $("#Cust_NameList").val();
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
                                            else {
                                                CalculateAmount();
                                                ResetTaxDetails_toTaxAndOcSection("Hdn_TaxCalculatorTbl", "N");
                                                $("#TxtTaxAmount").val(parseFloat(CheckNullNumber(TatalTaxAmt_NonGst)).toFixed(ValDecDigit));
                                                let SuppId = $("#Cust_NameList").val();
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
                                            
                                            //GetAllGLID();
                                        }
                                    } else {
                                        CalculateAmount();
                                        GetAllGLID();
                                    }
                                    //CalculateAmount();
                                    //CalculateVoucherTotalAmount();
                                    debugger;


                                }
                            }
                            hideLoader();
                        },
                    });
                } catch (err) {
                    console.log("SaleInvoice Error : " + err.message);
                    hideLoader();
                }
            }
        }
    }
}
function RateFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        if (CmnAmtFloatVal(el, evt, "#ExpImpRateDigit") == false) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        if (CmnAmtFloatVal(el, evt, "#RateDigit") == false) {
            return false;
        }
        else {
            return true;
        }
    }

}
function RateFloatValueonlyConvRate(el, evt) {
    //debugger;
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}
function QtyFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        if (CmnAmtFloatVal(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        if (CmnAmtFloatVal(el, evt, "#QtyDigit") == false) {
            return false;
        }
        else {
            return true;
        }
    }

}
function AmtFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        if (CmnAmtFloatVal(el, evt, "#ExpImpValDigit") == false) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        if (CmnAmtFloatVal(el, evt, "#ValDigit") == false) {
            return false;
        }
        else {
            return true;
        }
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
function OnClickTaxExemptedCheckBox(e, InvoiceRate) {
    //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }
    if (InvoiceRate == "InvoiceRate") {
        var currentrow = e;
    }
    else {
        var currentrow = $(e.target).closest('tr');
    }
    var currentrow = $(e.target).closest('tr');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        CalculateTaxExemptedAmt(e)
        GetAllGLID();
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "")
        }
        else {
            CalculateTaxExemptedAmt(e)
            GetAllGLID();
        }
        //CalculateTaxExemptedAmt(e)
        //GetAllGLID();
    }
}
function OnClickManualGSTCheckBox(e) {
    //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }

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
        Cmn_OnSaveAddressApplyGST(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "")
        CalculateTaxExemptedAmt(e)
        //GetAllGLID();
        $("#taxTemplate").text("Template")
    }
}
function CalculateTaxExemptedAmt(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        RateDecDigit = $("#ExpImpRateDigit").text();
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        RateDecDigit = $("#RateDigit").text();
        ValDecDigit = $("#ValDigit").text();
    }
    var clickedrow = $(e.target).closest("tr");
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    //var ItmRate = clickedrow.find("#TxtRate").val();
    //var ItmRate = clickedrow.find("#TxtInvoiceRate").val();
    //var oc_amt = clickedrow.find("#TxtOtherCharge").val();
    //var DisPer = clickedrow.find("#item_disc_perc").val();
    //var DisAmt = clickedrow.find("#item_disc_amt").val();
    //DisPer = DisPer = CheckNullNumber(DisPer);
    //DisAmt = DisAmt = CheckNullNumber(DisAmt);
    //if (ItmRate != "" && ItmRate != ".") {
    //    ItmRate = parseFloat(ItmRate);
    //}
    //if (ItmRate == ".") {
    //    ItmRate = 0;
    //}
    //if (oc_amt == ".") {
    //    oc_amt = 0;
    //}
    //if (oc_amt != "" && oc_amt != ".") {
    //    oc_amt = parseFloat(oc_amt);
    //}

    //if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
    //    var FAmt = cmn_getmultival(OrderQty, ItmRate);
    //    var FinVal = parseFloat(getvalwithoutroundoff(FAmt, ValDecDigit)).toFixed(ValDecDigit);
    //    debugger;
    //    clickedrow.find("#TxtItemGrossValue").val(FinVal);
    //    clickedrow.find("#NetOrderValueSpe").val(FinVal);
    //    FinalVal = cmn_getmultival(FinVal, ConvRate)

    //    FinalVal = FinalVal + oc_amt
    //    clickedrow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff(FinalVal, ValDecDigit)).toFixed(ValDecDigit));
    //    clickedrow.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(FinalVal, ValDecDigit)).toFixed(ValDecDigit));
    //    CalculateAmount();
    //}
    //if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
    //    //CalculateDisPercent(clickedrow); //commented by Suraj on 19-10-2024
    //}
    //if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
    //    //CalculateDisAmt(clickedrow); //commented by Suraj on 19-10-2024
    //}
    //clickedrow.find("#item_rate").val(parseFloat(getvalwithoutroundoff(ItmRate, RateDecDigit)).toFixed(RateDecDigit));
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
            CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
            clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
        }
    }
}
function CalculateDisPercent(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }

    //var clickedrow = $(e.target).closest("tr");
    var conv_rate = $("#conv_rate").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisPer;
    ItemName = clickedrow.find("#hfItemID").val();
    OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    //ItmRate = clickedrow.find("#TxtRate").val();
    ItmRate = clickedrow.find("#TxtInvoiceRate").val();
    DisPer = clickedrow.find("#item_disc_perc").val();
    if (AvoidDot(DisPer) == false) {
        clickedrow.find("#item_disc_perc").val("");
        DisPer = 0;
        //return false;
    }
    if (parseFloat(CheckNullNumber(DisPer)) >= 100) {
        clickedrow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
        clickedrow.find("#item_disc_percError").css("display", "block");
        clickedrow.find("#item_disc_perc").css("border-color", "red");
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
        return false;
    } else {
        clickedrow.find("#item_disc_percError").text("");
        clickedrow.find("#item_disc_percError").css("display", "none");
        clickedrow.find("#item_disc_perc").css("border-color", "#ced4da");
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        var FAmt = cmn_getmultival(ItmRate, DisPer) / 100;
        var GAmt = cmn_getmultival(OrderQty, (ItmRate - FAmt));
        var DisVal = cmn_getmultival(OrderQty, FAmt);
        var FinGVal = parseFloat(getvalwithoutroundoff(GAmt, ValDecDigit)).toFixed(ValDecDigit);
        var FinDisVal = parseFloat(getvalwithoutroundoff(DisVal, ValDecDigit)).toFixed(ValDecDigit);
        clickedrow.find("#TxtItemGrossValue").val(FinGVal);
        //clickedrow.find("#item_ass_val").val(FinGVal);
        //clickedrow.find("#item_net_val_spec").val(FinGVal);
        FinalFinGVal = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinGVal, conv_rate), ValDecDigit)).toFixed(ValDecDigit);
        clickedrow.find("#NetOrderValueSpe").val(FinalFinGVal);
        clickedrow.find("#item_disc_val").val(FinDisVal);
        CalculateAmount();

        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(2));
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = cmn_getmultival(OrderQty, ItmRate);
            var FinVal = parseFloat(getvalwithoutroundoff(FAmt, ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#TxtItemGrossValue").val(FinVal);
            //clickedrow.find("#item_ass_val").val(FinVal);
            //clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinVal, conv_rate), ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#NetOrderValueSpe").val(FinalFinVal);
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));

            CalculateAmount();
        }
        clickedrow.find("#item_disc_amt").prop("readonly", false);

    }
    CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
    //OnChangeGrossAmt();
    //var OrderType = $('#src_type').val();
    //if (OrderType == "D") {
    //    clickedrow.find("#QuotationNumber").val("Direct")
    //    QuotationDate = "Direct";
    //}
    //CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());

}
function CalculateDisAmt(clickedrow) {
    debugger;

    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }
    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisAmt;
    ItemName = clickedrow.find("#hfItemID").val();
    OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    //ItmRate = clickedrow.find("#TxtRate").val();
    ItmRate = clickedrow.find("#TxtInvoiceRate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    if (AvoidDot(DisAmt) == false) {
        clickedrow.find("#item_disc_amt").val("");
        DisAmt = 0;
        //return false;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        if (parseFloat(CheckNullNumber(ItmRate)) > parseFloat(CheckNullNumber(DisAmt))) {
            var FRate = (ItmRate - DisAmt);
            var GAmt = cmn_getmultival(OrderQty, FRate);
            var DisVal = cmn_getmultival(OrderQty, DisAmt);
            var FinGVal = parseFloat(getvalwithoutroundoff(GAmt, ValDecDigit)).toFixed(ValDecDigit);
            var FinDisVal = parseFloat(getvalwithoutroundoff(DisVal, ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#item_disc_val").val(FinDisVal);
            clickedrow.find("#TxtItemGrossValue").val(FinGVal);
            //clickedrow.find("#item_ass_val").val(FinGVal);
            //clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalFinGVal = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinGVal, conv_rate), ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#NetOrderValueSpe").val(FinalFinGVal);
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_perc").val("");
            CalculateAmount();
            clickedrow.find("#item_disc_amtError").text("");
            clickedrow.find("#item_disc_amtError").css("display", "none");
            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        }
        else {
            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
            clickedrow.find("#item_disc_amtError").css("display", "block");
            clickedrow.find("#item_disc_amt").css("border-color", "red");
            clickedrow.find("#item_disc_amt").val('');
            //clickedrow.find("#item_disc_val").val('');
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_perc").val("");
        }
        clickedrow.find("#item_disc_amt").val(parseFloat(getvalwithoutroundoff(DisAmt, ValDecDigit)).toFixed(ValDecDigit));
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = cmn_getmultival(OrderQty, ItmRate);
            var FinVal = parseFloat(getvalwithoutroundoff(FAmt, ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#TxtItemGrossValue").val(FinVal);
            //clickedrow.find("#item_ass_val").val(FinVal);
            //clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = parseFloat(getvalwithoutroundoff(cmn_getmultival(FinVal, conv_rate), ValDecDigit)).toFixed(ValDecDigit);
            clickedrow.find("#NetOrderValueSpe").val(FinalFinVal);
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateAmount();
        }
        clickedrow.find("#item_disc_perc").prop("readonly", false);

    }
    CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
}
function CalculateTaxAmount_ItemWise(ItmCode, AssAmount,Flag) {
    //debugger;
    var DecDigit = "";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }

    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(getvalwithoutroundoff(AssAmount, DecDigit)).toFixed(DecDigit);
                if (TaxItemID == ItmCode) {

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
                            TaxAmount = parseFloat(getvalwithoutroundoff(((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec))) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = parseFloat(getvalwithoutroundoff((parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)), DecDigit)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = parseFloat(getvalwithoutroundoff((parseFloat(TaxAMountColIL)), DecDigit)).toFixed(DecDigit);
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmtIL), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = parseFloat(getvalwithoutroundoff((parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)), DecDigit)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = parseFloat(getvalwithoutroundoff((parseFloat(AssessVal) + parseFloat(TaxAMountCol)), DecDigit)).toFixed(DecDigit);
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmt), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                    }
                    //debugger;
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
                var TaxAccId = currentRow.find("#TaxAccId").text();

                //var DocNo = currentRow.find("#DocNo").text();

                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId });

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
                            //debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val();
                            var TaxAmt = parseFloat(0).toFixed(DecDigit);
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                if (parseFloat(getvalwithoutroundoff(TotalTaxAmtF, DecDigit)) > 0) {//Added by Suraj Maurya on 04-11-2024
                                    currentRow.find("#BtnTxtCalculation").css("border", "none");
                                }
                                currentRow.find("#Txtitem_tax_amt").val(parseFloat(getvalwithoutroundoff(TotalTaxAmtF, DecDigit)).toFixed(DecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(getvalwithoutroundoff(currentRow.find("#item_oc_amt").val(), DecDigit)).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit))).toFixed(DecDigit);
                                NetOrderValueSpec = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                                NetOrderValueBase = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                                currentRow.find("#NetOrderValueSpe").val(NetOrderValueSpec);
                                //currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = cmn_getmultival(ConvRate, NetOrderValueBase)
                                var oc_amt = currentRow.find("#TxtOtherCharge").val();
                                if (oc_amt == ".") {
                                    oc_amt = 0;
                                }
                                if (oc_amt != "" && oc_amt != ".") {
                                    oc_amt = parseFloat(oc_amt);
                                }
                                FinalNetOrderValueBase = FinalNetOrderValueBase + oc_amt
                                NetOrderValueSpec = parseFloat(NetOrderValueSpec) + oc_amt;
                                currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff(NetOrderValueSpec, DecDigit)).toFixed(DecDigit));
                                currentRow.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(FinalNetOrderValueBase, DecDigit)).toFixed(DecDigit));
                            }
                        });
                    }
                    else {
                        //debugger;
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit)).toFixed(DecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(getvalwithoutroundoff(currentRow.find("#item_oc_amt").val(), DecDigit)).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = parseFloat(getvalwithoutroundoff((parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)), DecDigit)).toFixed(DecDigit);
                        //currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        FinalFGrossAmtOR = cmn_getmultival(ConvRate, FGrossAmtOR)
                        var oc_amt = currentRow.find("#TxtOtherCharge").val();
                        if (oc_amt == ".") {
                            oc_amt = 0;
                        }
                        if (oc_amt != "" && oc_amt != ".") {
                            oc_amt = parseFloat(oc_amt);
                        }
                        FinalFGrossAmtOR = FinalFGrossAmtOR + oc_amt;
                        FGrossAmtOR = FGrossAmtOR + oc_amt;
                        currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff(FGrossAmtOR), DecDigit).toFixed(DecDigit));
                        currentRow.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(FinalFGrossAmtOR), DecDigit).toFixed(DecDigit));
                    }
                }
            });
            if (Flag == "NoCalc") {//Added by Suraj Maurya on 12-02-2025
                //In Case of change in Discount in perc or value. 
            }
            else {
                CalculateAmount();
            }
            BindTaxAmountDeatils(NewArray);
        }
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
    var Cust_id = $('#Cust_NameList').val();

    Cmn_BuyerInfoBtnClick(ItmCode, Cust_id);


}
function OnchangeConvRate(e) {
    var ExchDecDigit = $("#ExchDigit").text();
    var ConvRate;
    ConvRate = $("#conv_rate").val();

    if (AvoidDot(ConvRate) == false) {

        $("#conv_rate").val("");
        $("#hdnconv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
    } else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
        $("SpanExRateErrorMsg").css("border-color", "#ced4da");

        $("#conv_rate").val(parseFloat(getvalwithoutroundoff(ConvRate, ExchDecDigit)).toFixed(ExchDecDigit));
        $("#hdnconv_rate").val(parseFloat(getvalwithoutroundoff(ConvRate, ExchDecDigit)).toFixed(ExchDecDigit));
        //$('#SInvItmDetailsTbl tbody tr').remove();
        //OnClickAddButton();

        ReCalculateWithNewExchangeRate();
        CalculateAmount();
        //GetAllGLID();
    }
}
function ReCalculateWithNewExchangeRate() {
    var ConvRate = $("#conv_rate").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();///Amount
    }
    else {
        ValDecDigit = $("#ValDigit").text();///Amount
    }


    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var NetOrderValueSpec = currentRow.find("#TxtNetValue").val();
        var NetOrderValueBase = cmn_getmultival(parseFloat(CheckNullNumber(NetOrderValueSpec)), parseFloat(CheckNullNumber(ConvRate)));

        currentRow.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(NetOrderValueBase, ValDecDigit)).toFixed(ValDecDigit));

    });
    //$("#ht_Tbl_OC_Deatils >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    var OCAmtSp = currentRow.find("#OCAmtSp").text();
    //    var OCAmtBs = parseFloat(CheckNullNumber(OCAmtSp)) * parseFloat(CheckNullNumber(ConvRate));

    //    currentRow.find("#OC_Conv").text(ConvRate);
    //    currentRow.find("#OCAmtBs").text(OCAmtBs.toFixed(ValDecDigit));
    //    currentRow.find("#OCTotalTaxAmt").text(OCAmtBs.toFixed(ValDecDigit));

    //});
    $("#Tbl_OC_Deatils >tbody >tr").each(function () {
        var currentRow = $(this);
        var OCAmtSp = currentRow.find("#OCAmount").text();
        var OCAmtBs = cmn_getmultival(parseFloat(CheckNullNumber(OCAmtSp)), parseFloat(CheckNullNumber(ConvRate)));

        currentRow.find("#OCConv").text(ConvRate);
        //currentRow.find("#OcAmtBs").text(OCAmtBs.toFixed(ValDecDigit));
        //currentRow.find("#OCTotalTaxAmt").text(OCAmtBs.toFixed(ValDecDigit));
        currentRow.find("#OcAmtBs").text(parseFloat(getvalwithoutroundoff(OCAmtBs, ValDecDigit)).toFixed(ValDecDigit));
        currentRow.find("#OCTotalTaxAmt").text(parseFloat(getvalwithoutroundoff(OCAmtBs, ValDecDigit)).toFixed(ValDecDigit));
    });
    OnClickSaveAndExit_OC_Btn();

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

    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var SinvDate = $("#Inv_date").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: SinvDate
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
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
                    /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
                hideLoader();
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
    var SaleVouMsg = "";
    var mailerror = "";

    var PV_VoucherNarr = $("#hdn_PV_Nurration").val()
    var BP_VoucherNarr = $("#hdn_BP_Nurration").val()

    docid = $("#DocumentMenuId").val();
    SINo = $("#InvoiceNumber").val();
    SIDate = $("#Inv_date").val();
    $("#hdDoc_No").val(SINo);
    var WF_status1 = $("#WF_status1").val();
    var CustType = $("#InvType").val();
    var TrancType = (SINo + ',' + SIDate + ',' + docid + ',' + WF_status1 + ',' + CustType)
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    if ($("#OrderTypeD").is(":checked")) {
        InvType = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        InvType = "E";
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    SaleVouMsg = $("#SaleVoucherPassAgainstInv").text();
    var DN_VoucherNarr = $("#hdn_DN_Narration").val();/*Add by Hina on 10-07-2024 to add for tds third party OC*/
    var DN_VoucherNarr_Tcs = $("#hdn_DN_Narration_Tcs").val();/*Add by Suraj Maurya on 04-02-2025 to add nurration for Debit note against tcs*/
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
    
    try {
        if (fwchkval != "Approve") {
            var pdfAlertEmailFilePath = "TaxInvoice_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(SINo, SIDate, pdfAlertEmailFilePath, docid, fwchkval);
            if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
                pdfAlertEmailFilePath = "";
            }
        }
    }
    catch { }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SINo != "" && SIDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{
            SINo: SINo, SIDate: SIDate, InvType: InvType
            , SaleVouMsg: SaleVouMsg.replace("#",''), A_Status: "Approve"
            , A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks, DN_Nurr_tcs: DN_VoucherNarr_Tcs, GstApplicable: GstApplicable
        }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/LocalSaleInvoice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid + "&InvType=" + InvType + "&PV_VoucherNarr=" + PV_VoucherNarr + "&BP_VoucherNarr=" + BP_VoucherNarr + "&DN_VoucherNarr=" + DN_VoucherNarr;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/LocalSaleInvoice/SIListApprove?SI_No=" + SINo + "&SI_Date=" + SIDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SINo != "" && SIDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SINo != "" && SIDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SINo, SIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LocalSaleInvoice/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}

function GetPdfFilePathToSendonEmailAlert(shpNo, shpDt, fileName, docid, docstatus) {
    debugger;
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var docId = "105103140";
    if ($("#OrderTypeE").is(":checked")) {
        docId = "105103145125";
    }
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
        PrintCorpAddr: $("#PrintCorpAddr").val(),
        PrintRemarks: $("#PrintRemarks").val(),
    })
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalSaleInvoice/SavePdfDocToSendOnEmailAlert",
            data: { docId: docId, shpNo: shpNo, shpDt: shpDt, fileName: fileName, PrintFormat: JSON.stringify(PrintFormat), GstApplicable: GstApplicable, docid: docid, docstatus: docstatus },
            /*dataType: "json",*/
            success: function (data) {
                hideLoader();
            }
        });
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
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
    try {
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
                hideLoader();
            }
        });

    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
function DSIList() {
    try {
        location.href = "/ApplicationLayer/LocalSaleInvoice/LocalSaleInvoiceList";
    } catch (err) {

    }
}


//------------------Tax Amount Calculation------------------//

function OnClickTaxCalBtn(e) {
    var SOItemListName = "#TxtItemName";
    var SNohiddenfiled = "SI";
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
    debugger;
    var ConvRate = $("#conv_rate").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }

    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var ShipmentNo = $("#TaxCalcGRNNo").val();
    var ShipmentDate = $("#TaxCalcGRNDate").val();


    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    //debugger;
    let NewArr = new Array();
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    var taxrowcount = $('#' + HdnTaxCalculateTable + ' tbody tr').length;
    if (taxrowcount > 0) {
        $("#" + HdnTaxCalculateTable + " TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#TaxItmCode").text();
            var shipno = row.find("#DocNo").text();
            var shipdate = row.find("#DocDate").text();
            if (TaxOn == "OC" && rowitem == TaxItmCode) {
                $(this).remove();
            } else {
                if (rowitem == TaxItmCode && shipno == ShipmentNo && shipdate == ShipmentDate) {
                    $(this).remove();
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
            var TaxAccID = currentRow.find("#AccID").text();
            var Arr = {
                ShipmentNo: ShipmentNo, ShipmentDate: ShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID
                , TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount
                , TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID
            }
            RenderToHdnTaxTables(HdnTaxCalculateTable, Arr);
        });

        //var rowcount = $('#' + HdnTaxCalculateTable + ' tbody tr').length;
        //if (rowcount > 0) {
        //    $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
        //        //debugger;
        //        var currentRow = $(this);

        //        var TaxItemID = currentRow.find("#TaxItmCode").text();
        //        var TaxName = currentRow.find("#TaxName").text();
        //        var TaxNameID = currentRow.find("#TaxNameID").text();
        //        var TaxPercentage = currentRow.find("#TaxPercentage").text().replace('', '%');
        //        var TaxLevel = currentRow.find("#TaxLevel").text();
        //        var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
        //        var TaxAmount = currentRow.find("#TaxAmount").text();
        //        //var TotalTaxAmount = currentRow.find("#itemtaxamt").text();
        //        var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
        //        var TaxAccId = currentRow.find("#TaxAccId").text();
        //        NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccId: TaxAccId });
        //    });
        //}
        ////debugger;
        if (TaxOn != "OC") {
            //var ForGL = OnAddGRN == "Y" ? "N" : "";
            var ForGL = OnAddGRN == "Y" ? "N" : null;
            ResetTaxDetails_toTaxAndOcSection(HdnTaxCalculateTable, ForGL)
            //BindTaxAmountDeatils(NewArr, ForGL);
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
            TaxCL.TaxAccId = currentRow.find("#AccID").text();
            TaxCL.TaxItmCode = TaxItmCode;
            TaxCalculationList.push(TaxCL);
            var Arr = TaxCL;
            Arr.ShipmentNo = ShipmentNo;
            Arr.ShipmentDate = ShipmentDate;
            Arr.TaxItmCode = TaxItmCode;
            Arr.TotalTaxAmount = TotalTaxAmount;
            Arr.TaxAccID = TaxCL.TaxAccId;

            RenderToHdnTaxTables(HdnTaxCalculateTable, Arr);
            //$("#" + HdnTaxCalculateTable + " > tbody").append(`
            //                    <tr>
            //                        <td id="DocNo">${ShipmentNo}</td>
            //                        <td id="DocDate">${ShipmentDate}</td>
            //                        <td id="TaxItmCode">${TaxItmCode}</td>
            //                        <td id="TaxName">${TaxCL.TaxName}</td>
            //                        <td id="TaxNameID">${TaxCL.TaxNameID}</td>
            //                        <td id="TaxPercentage">${TaxCL.TaxPercentage}</td>
            //                        <td id="TaxLevel">${TaxCL.TaxLevel}</td>
            //                        <td id="TaxApplyOn">${TaxCL.TaxApplyOn}</td>
            //                        <td id="TaxAmount">${TaxCL.TaxAmount}</td>
            //                        <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            //                        <td id="TaxApplyOnID">${TaxCL.TaxApplyOnID}</td>
            //                        <td id="TaxAccId">${TaxCL.TaxAccId}</td>
            //                    </tr>`);
        });
        if (TaxOn != "OC") {
            //var ForGL = OnAddGRN == "Y" ? "N" : "";
            BindTaxAmountDeatils(TaxCalculationList);
        }
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // //debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(getvalwithoutroundoff(TotalTaxAmount, DecDigit))).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                //var Total = parseFloat(getvalwithoutroundoff((parseFloat(OCAmt) + parseFloat(TaxAmt)), DecDigit)).toFixed(DecDigit)
                //currentRow.find("#OCTotalTaxAmt").text(Total);
                Cmn_click_Oc_chkroundoff(null, currentRow);//Added by Suraj Maurya on 11-12-2024
                var Total = CheckNullNumber(currentRow.find("#OCTotalTaxAmt").text()).trim();//Include Tax OC Amount
                var oc_tds_amt = Cmn_OcTDS_CalOnchangeDocDetail(OCAmt, TaxItmCode, Total);
                currentRow.find("#OC_TDSAmt").text(oc_tds_amt);
            }
        });
        Calculate_OCAmount();
    } else {
        $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            ////debugger;
            var currentRow = $(this);
            var ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
            var ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(getvalwithoutroundoff(TotalTaxAmount, DecDigit))).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = 0;
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            if (ItmCode == TaxItmCode && ShipmentNoTbl == ShipmentNo && ShipmentDateTbl == ShipmentDate) {
                currentRow.find("#Txtitem_tax_amt").val(parseFloat(getvalwithoutroundoff(TaxAmt, DecDigit)).toFixed(DecDigit));
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(getvalwithoutroundoff(currentRow.find("#TxtOtherCharge").val(), DecDigit)).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit))).toFixed(DecDigit);
                NetOrderValueSpec = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                NetOrderValueBase = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                FinalNetOrderValueBase = parseFloat(getvalwithoutroundoff((cmn_getmultival(NetOrderValueBase, ConvRate)), DecDigit)).toFixed(DecDigit);
                currentRow.find("#TxtNetValueInBase").val(FinalNetOrderValueBase);
            }
            var TaxAmt3 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt3 = currentRow.find("#Txtitem_tax_amt").val();
            if (ItemTaxAmt3 != TaxAmt3) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
    var ForGL = OnAddGRN == "Y" ? "N" : "";
    if (ForGL == "") {
        GetAllGLID();
    }
    //GetAllGLID();
}
function RenderToHdnTaxTables(TableName, Arr) {
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var ShipmentNo = $("#TaxCalcGRNNo").val();
    var ShipmentDate = $("#TaxCalcGRNDate").val();

    $("#" + TableName + " > tbody").append(`
                                <tr>
                                    <td id="DocNo">${IsNull(Arr.ShipmentNo, ShipmentNo)}</td>
                                    <td id="DocDate">${IsNull(Arr.ShipmentDate, ShipmentDate)}</td>
                                    <td id="TaxItmCode">${IsNull(Arr.TaxItmCode, TaxItmCode)}</td>
                                    <td id="TaxName">${Arr.TaxName}</td>
                                    <td id="TaxNameID">${Arr.TaxNameID}</td>
                                    <td id="TaxPercentage">${Arr.TaxPercentage}</td>
                                    <td id="TaxLevel">${Arr.TaxLevel}</td>
                                    <td id="TaxApplyOn">${Arr.TaxApplyOn}</td>
                                    <td id="TaxAmount">${Arr.TaxAmount}</td>
                                    <td id="TotalTaxAmount">${Arr.TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${Arr.TaxApplyOnID}</td>
                                    <td id="TaxAccId">${Arr.TaxAccID}</td>
                                </tr>`)
}
function ResetTaxDetails_toTaxAndOcSection(HdnTaxCalculateTable, ForGL) {
    var rowcount = $('#' + HdnTaxCalculateTable + ' tbody tr').length;
    let NewArr = new Array();
    if (rowcount > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            //debugger;
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
            var TaxAccId = currentRow.find("#TaxAccId").text();
            NewArr.push({
                TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage
                , TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TaxApplyOnID: TaxApplyOnID
                , TaxAccId: TaxAccId
            });
        });
    }
    BindTaxAmountDeatils(NewArr, ForGL);
}
function CalculateAmount() {
    debugger;
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var tcs_amt = $("#TxtTDS_Amount").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
        tcs_amt = 0;
    }
    else {
        DecDigit = $("#ValDigit").text();
    }

    var GrossValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(getvalwithoutroundoff(GrossValue, DecDigit)) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(getvalwithoutroundoff(GrossValue, DecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit))).toFixed(DecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(getvalwithoutroundoff(TaxValue, DecDigit)) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(getvalwithoutroundoff(TaxValue, DecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#Txtitem_tax_amt").val(), DecDigit))).toFixed(DecDigit);
        }

        if (currentRow.find("#TxtNetValue").val() == "" || currentRow.find("#TxtNetValue").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(getvalwithoutroundoff(NetOrderValueSpec, DecDigit)) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(getvalwithoutroundoff(NetOrderValueSpec, DecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtNetValue").val(), DecDigit))).toFixed(DecDigit);
        }
        if (currentRow.find("#TxtNetValueInBase").val() == "" || currentRow.find("#TxtNetValueInBase").val() == "NaN") {
            NetOrderValueBase = (parseFloat(getvalwithoutroundoff(NetOrderValueBase, DecDigit)) + parseFloat(0)).toFixed(DecDigit);
            /* FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);*/
        }
        else {
            NetOrderValueBase = (parseFloat(getvalwithoutroundoff(NetOrderValueBase, DecDigit)) + parseFloat(getvalwithoutroundoff(currentRow.find("#TxtNetValueInBase").val(), DecDigit))).toFixed(DecDigit);
            /* FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);*/
        }

    });
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtTaxAmount").val(TaxValue);
    var oc_amountBs = $("#TxtDocSuppOtherCharges").val();
    var Dmenu = $("#DocumentMenuId").val();
    
    var oc_amountSp = parseFloat(oc_amountBs) / parseFloat(ConvRate);
    NetOrderValueSpec = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amountSp))
        + parseFloat(CheckNullNumber(tcs_amt));
    NetOrderValueBase = NetOrderValueSpec * ConvRate;
    
    //NetOrderValueBase = NetOrderValueBase + parseFloat(CheckNullNumber(tcs_amt));

    $("#NetOrderValueSpe").val(parseFloat(getvalwithoutroundoff(NetOrderValueSpec, DecDigit)).toFixed(DecDigit));

    $("#NetOrderValueInBase").val(parseFloat(getvalwithoutroundoff(NetOrderValueBase, DecDigit)).toFixed(DecDigit));
    //$('#NetOrderValueSpe').change(GetAllGLID());

}
//function CalculateAmount() {
//    //debugger;
//    var ConvRate;
//    ConvRate = $("#conv_rate").val();
//    var DecDigit = $("#ValDigit").text();
//    var GrossValue = parseFloat(0).toFixed(DecDigit);
//    var TaxValue = parseFloat(0).toFixed(DecDigit);
//    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
//    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

//    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);

//        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
//            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
//        }
//        else {
//            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
//        }
//        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
//            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
//        }
//        else {
//            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
//        }

//        if (currentRow.find("#TxtNetValue").val() == "" || currentRow.find("#TxtNetValue").val() == "NaN") {
//            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(DecDigit);
//        }
//        else {
//            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#TxtNetValue").val())).toFixed(DecDigit);
//        }
//        if (currentRow.find("#TxtNetValueInBase").val() == "" || currentRow.find("#TxtNetValueInBase").val() == "NaN") {
//            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(DecDigit);
//            /* FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);*/
//        }
//        else {
//            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#TxtNetValueInBase").val())).toFixed(DecDigit);
//            /* FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);*/
//        }

//    });
//    $("#TxtGrossValue").val(GrossValue);
//    $("#TxtTaxAmount").val(TaxValue);
//    var oc_amountBs = $("#TxtDocSuppOtherCharges").val();
//    var oc_amountSp = parseFloat(oc_amountBs) / parseFloat(ConvRate);
//    NetOrderValueSpec = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amountSp));
//    NetOrderValueBase = parseFloat(NetOrderValueSpec) * parseFloat(ConvRate);
//    $("#NetOrderValueSpe").val(parseFloat(NetOrderValueSpec).toFixed(DecDigit));
//    $("#NetOrderValueInBase").val(parseFloat(NetOrderValueBase).toFixed(DecDigit));

//    //$('#NetOrderValueSpe').change(GetAllGLID());
//}
function ResetShipment_DDL_Detail() {
    //$("#Shipment_Date").val("");
    //var DocNo = $('#ddlShipmentNo').val();
    //$("#ddlShipmentNo>optgroup>option[value='" + DocNo + "']").select2().hide();

    //$('#ddlShipmentNo').val("---Select---").prop('selected', true);
    //$('#ddlShipmentNo').trigger('change');
    //$('#ddlShipmentNo').select2('close');
    $('#ddlShipmentNo').attr("disabled", true);
    $(".Shipment_AddBtn").css("display", "none");
}

function OnClickReplicateOnAllItems() {
    var ConvRate = $("#conv_rate").val();

    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var ShipmentNo = $("#TaxCalcGRNNo").val();
    var ShipmentDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    debugger;
    var rowIdx = 0;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
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
        TaxCalculationList.push({ UserID: UserID, TaxShipmentNo: ShipmentNo, TaxShipmentDate: ShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
        TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: ShipmentNo, TaxShipmentDate: ShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "SInvItmDetailsTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var ShipmentNoTbl;
                var ShipmentDateTbl;
                var ItemCode;
                var AssessVal;
                ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
                ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();

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
                    var TaxAccID = TaxCalculationList[i].TaxAccID;
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = parseFloat(getvalwithoutroundoff((parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)), DecDigit)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(getvalwithoutroundoff(TaxAMountColIL, DecDigit))).toFixed(DecDigit);
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmtIL), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(AssessVal), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = parseFloat(getvalwithoutroundoff((parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)), DecDigit)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = parseFloat(getvalwithoutroundoff((parseFloat(AssessVal) + parseFloat(TaxAMountCol)), DecDigit)).toFixed(DecDigit);
                            TaxAmount = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(FinalAssAmt), parseFloat(TaxPec)) / 100), DecDigit)).toFixed(DecDigit);
                            TotalTaxAmt = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)), DecDigit)).toFixed(DecDigit);
                        }
                    }
                    NewArray.push({ UserID: UserID, TaxShipmentNo: ShipmentNoTbl, TaxShipmentDate: ShipmentDateTbl, TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
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
                            var TaxAccID = NewArray[k].TaxAccID;
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo == TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate == TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
                            }
                            if (CitmTaxItmCode == TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate != TaxShipmentDate) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
                                }
                            }
                            if (CitmTaxItmCode != TaxItmCode && ShipmentNo != TaxShipmentNo && ShipmentDate != TaxShipmentDate) {
                                TaxCalculationListFinalList.push({ UserID: UserID, TaxShipmentNo: TaxShipmentNo, TaxShipmentDate: TaxShipmentDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID, TaxAccID: TaxAccID })
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
                    var TaxAccID = TaxCalculationListFinalList[n].TaxAccID;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');
                    /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
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
                                    <td id="TaxAccId">${TaxAccID}</td>
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
                            var NetAmt = parseFloat(getvalwithoutroundoff((parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)), DecDigit)).toFixed(DecDigit)
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

        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var ShipmentNoTbl = currentRow.find("#TxtShipmentNo").val();
            var ShipmentDateTbl = currentRow.find("#hfShipmentDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var AShipmentNo = TaxCalculationListFinalList[i].TaxShipmentNo;
                        var AShipmentDate = TaxCalculationListFinalList[i].TaxShipmentDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID && ShipmentNoTbl == AShipmentNo && ShipmentDateTbl == AShipmentDate) {
                            TaxAmt = parseFloat(getvalwithoutroundoff(TotalTaxAmount, DecDigit)).toFixed(DecDigit);
                            currentRow.find("#Txtitem_tax_amt").val(parseFloat(getvalwithoutroundoff(TotalTaxAmtF, DecDigit)).toFixed(DecDigit));
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                            }
                            AssessableValue = (parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit))).toFixed(DecDigit);
                            NetOrderValueSpec = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                            NetOrderValueBase = parseFloat(getvalwithoutroundoff((parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)), DecDigit)).toFixed(DecDigit);
                            currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = parseFloat(getvalwithoutroundoff(cmn_getmultival(NetOrderValueBase, ConvRate), DecDigit)).toFixed(DecDigit);
                            currentRow.find("#TxtNetValueInBase").val(FinalNetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(getvalwithoutroundoff(currentRow.find("#TxtItemGrossValue").val(), DecDigit)).toFixed(DecDigit);
                    currentRow.find("#Txtitem_tax_amt").val(parseFloat(getvalwithoutroundoff(TaxAmt, DecDigit)).toFixed(DecDigit));
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(getvalwithoutroundoff(currentRow.find("#TxtOtherCharge").val(), DecDigit)).toFixed(DecDigit);
                    }
                    var FGrossAmt = parseFloat(getvalwithoutroundoff((parseFloat(OC_Amt) + parseFloat(GrossAmt)), DecDigit)).toFixed(DecDigit);
                    currentRow.find("#TxtNetValue").val(FGrossAmt);
                    FinalFGrossAmt = parseFloat(getvalwithoutroundoff(cmn_getmultival(FGrossAmt, ConvRate), DecDigit)).toFixed(DecDigit);
                    currentRow.find("#FinalFGrossAmt").val(FGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                currentRow.find("#Txtitem_tax_amt").val(parseFloat(TaxAmt).toFixed(DecDigit));
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                var FGrossAmt = parseFloat(getvalwithoutroundoff((parseFloat(OC_Amt) + parseFloat(GrossAmt)), DecDigit)).toFixed(DecDigit);
                currentRow.find("#TxtNetValue").val(FGrossAmt);
                FinalFGrossAmt = parseFloat(getvalwithoutroundoff(cmn_getmultival(FGrossAmt, ConvRate), DecDigit)).toFixed(DecDigit);
                currentRow.find("#TxtNetValueInBase").val(FinalFGrossAmt);
            }
        });
        CalculateAmount();
        GetAllGLID();
    }
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
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//
//------------------Other Charge Amount Calculation------------------//
function OnClickSaveAndExit_OC_Btn() {
    //debugger;
    //BindOtherChargeGLDeatils(); 
    CMNOnClickSaveAndExit_OC_Btn("#TxtOtherCharges", "#NetOrderValueSpe", "#NetOrderValueInBase")
    click_chkplusminus();
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }
    var ConvRate = $("#conv_rate").val();
    var TotalGAmt = $("#TxtGrossValue").val();

    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
        var NetOrderValueSpec = currentRow.find("#TxtNetValue").val();
        var NetOrderValueBase = currentRow.find("#TxtNetValueInBase").val();

        //var fdec = parseInt(DecDigit) + 1;
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //var OCPercentage = parseFloat(getvalwithoutroundoff(cmn_getmultival((parseFloat(GrossValue) / parseFloat(TotalGAmt)) , 100), DecDigit)).toFixed(DecDigit);
            //var OCAmtItemWise = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(OCPercentage), parseFloat(TotalOCAmt)) / 100), DecDigit)).toFixed(DecDigit);
            var OCPercentage = parseFloat((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = parseFloat((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
            }
        } else {//Added by Suraj Maurya on 04-01-2025
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
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
        var AmtDifference = parseFloat(TotalOCAmount).toFixed(DecDigit) - parseFloat(TotalOCAmt);
        var Sno = 0;
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            Sno = Sno + 1;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val(parseFloat((parseFloat(OCValue) - parseFloat(AmtDifference))).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt).toFixed(DecDigit) - parseFloat(TotalOCAmount);
        var Sno = 0;
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            Sno = Sno + 1;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val(parseFloat((parseFloat(OCValue) + parseFloat(AmtDifference))).toFixed(DecDigit));
            }
        });
    }
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);

        var POItm_GrossValue = currentRow.find("#TxtItemGrossValue").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(DecDigit);
        }
        var POItm_NetOrderValueSpec = parseFloat((parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt)));
        var POItm_NetOrderValueBase = parseFloat((parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt)));
        currentRow.find("#TxtNetValue").val((parseFloat(POItm_NetOrderValueSpec)).toFixed(DecDigit));
        FinalItm_NetOrderValueBase = parseFloat(CheckNullNumber(POItm_NetOrderValueBase) * CheckNullNumber(ConvRate)).toFixed(DecDigit);
        currentRow.find("#TxtNetValueInBase").val((parseFloat(FinalItm_NetOrderValueBase)).toFixed(DecDigit));
    });
    CalculateAmount();
};
function SetOtherChargeVal() {

    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }


    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#TxtOtherCharge").val((parseFloat(0)).toFixed(DecDigit));
    });
}
function BindOtherChargeDeatils(val) {
    //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }


    //if ($("#SOItmDetailsTbl >tbody >tr").length == 0) {
    //    $("#Tbl_OC_Deatils >tbody >tr").remove();
    //    $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
    //    $("#_OtherChargeTotal").val(parseFloat(0).toFixed(DecDigit));
    //}
    var hdbs_curr = $("#hdbs_curr").val();
    var hdcurr = $("#hdcurr").val();
    var OCTaxable = "N";
    if (hdbs_curr == hdcurr) {
        OCTaxable = "Y";
    }
    var Dmenu = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#SI_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;  
            var td = "";
            var tdSupp = "";
            if (OCTaxable == "Y") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }
            if (Dmenu != "105103145125") {
                tdSupp = `<td>${currentRow.find("#td_OCSuppName").text()}</td>`
            }
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
`+ tdSupp + `
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = parseFloat(getvalwithoutroundoff((parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())), DecDigit)).toFixed(DecDigit);
            if (OCTaxable == "Y") {
                TotalTaxAMount = parseFloat(getvalwithoutroundoff((parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())), DecDigit)).toFixed(DecDigit);
                TotalAMountWT = parseFloat(getvalwithoutroundoff((parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())), DecDigit)).toFixed(DecDigit);
            }

        });

    }
    if (val == "") {
        GetAllGLID();
    }
    //$("#SI_OtherChargeTotal").text(TotalAMount);
    $("#_OtherChargeTotal").text(TotalAMount);
    if (OCTaxable == "Y") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }

}
function GetAllGLID(flag) {
    if ($("#SInvItmDetailsTbl >tbody >tr").length > 0) {
        GetAllGL_WithMultiSupplier(flag);
    }

}
async function GetAllGL_WithMultiSupplier(flag) { //Modied by Suraj on 07-10-2024 for issue in performance
    try {
        showLoader();

        if (flag != "RO") {
            if ($("#chk_roundoff").is(":checked")) {
                await addRoundOffToNetValue("CallByGetAllGL");
            }
        }
        var GstType = $("#Hd_GstType").val();
        var GstCat = $("#Hd_GstCat").val();
        var conv_rate = $("#conv_rate").val();
        var Dmenu = $("#DocumentMenuId").val();
        var TCSAmt = $("#TxtTDS_Amount").val();
        if (Dmenu == "105103145125") {
            ValDecDigit = $("#ExpImpValDigit").text();
            //var NetInvVal = $("#TxtGrossValue").val();
            var NetInvVal = $("#NetOrderValueInBase").val();
            var NetInvValue = parseFloat(NetInvVal);// * parseFloat(conv_rate);
        }
        else {
            ValDecDigit = $("#ValDigit").text();
            /*var NetInvValue = $("#TxtGrossValue").val();*/
            var NetInvValue = parseFloat(CheckNullNumber($("#NetOrderValueInBase").val())) - parseFloat(CheckNullNumber(TCSAmt));
        }
        var NetTaxValue = $("#TxtTaxAmount").val();


        var cust_id = $("#Cust_NameList").val();
        var cust_acc_id = $("#cust_acc_id").val();
        var CustVal = 0;
        var CustValInBase = 0;
        /*CustValInBase = (parseFloat(NetInvValue) + parseFloat(NetTaxValue)).toFixed(ValDecDigit);*/
        CustValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
        CustVal = CheckNullNumber($("#NetOrderValueSpe").val());// parseFloat(getvalwithoutroundoff((parseFloat(CustValInBase) / parseFloat(conv_rate)), ValDecDigit)).toFixed(ValDecDigit)
        if (Dmenu == "105103140") {
            CustVal = parseFloat(CustVal) - parseFloat(CheckNullNumber(TCSAmt));
        }
        var Compid = $("#CompID").text();
        var InvType = "";
        if ($("#OrderTypeD").is(":checked")) {
            InvType = "D";
        }
        if ($("#OrderTypeE").is(":checked")) {
            InvType = "E";
        }
        var curr_id = $("#hdcurr").val();
        var bill_no = IsNull($("#Bill_No").val(), '');
        var bill_dt = IsNull($("#Bill_Date").val(), '');
        var TransType = 'Sal';
        var GLDetail = [];
        var TxaExantedItemList = [];
        GLDetail.push({
            comp_id: Compid, id: cust_id, type: "Cust", doctype: InvType, Value: CustVal, ValueInBase: CustValInBase
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Cust", parent: 0, Entity_id: cust_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
        });
        $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {

            var currentRow = $(this);
            var item_id = currentRow.find("#hfItemID").val();
            var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
            var ItmDiscVal = currentRow.find("#item_disc_val").val();
            if (Dmenu == "105103145125") {
                var ItmGrossValInBs = currentRow.find("#TxtItemGrossValue").val();
                var ItmGrossValInBase = getvalwithoutroundoff(cmn_getmultival(parseFloat(ItmGrossValInBs), parseFloat(conv_rate)), ValDecDigit);
            }
            else {

                ItmGrossVal = parseFloat(CheckNullNumber(ItmGrossVal)) + parseFloat(CheckNullNumber(ItmDiscVal));
                var ItmGrossValInBase = ItmGrossVal;//currentRow.find("#TxtItemGrossValue").val();
            }
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
            var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
            var TxaExanted = "N";
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id });
            }
            if (ItemTaxAmt == TaxAmt) {
                if (currentRow.find("#ManualGST").is(":checked")) {
                    TxaExanted = "Y";
                    TxaExantedItemList.push({ item_id: item_id });
                }
            }
            GLDetail.push({
                comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
                , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: cust_acc_id
                , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: item_acc_id
            });
            if (Dmenu != "105103145125") {
                if (parseFloat(CheckNullNumber(ItmDiscVal)) > 0) {
                    GLDetail.push({
                        comp_id: Compid, id: item_id, type: "Disc", doctype: InvType, Value: ItmDiscVal
                        , ValueInBase: ItmDiscVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Disc", parent: cust_acc_id
                        , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: item_acc_id
                    });
                }
            }

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
            var oc_amt_wt = parseFloat(getvalwithoutroundoff((parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)), ValDecDigit)).toFixed(ValDecDigit); //with tax

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
        $("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var tax_id = currentRow.find("#taxID").text();
            var tax_acc_id = currentRow.find("#taxAccID").text();
            var tax_amt = currentRow.find("#TotalTaxAmount").text();
            var TaxPerc = currentRow.find("#TaxPerc").text();
            GLDetail.push({
                comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: cust_acc_id
                , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, ''), bill_date: IsNull(bill_dt, ''), acc_id: ""
            });
        });

        /*--------------------For Third Party TDS on Other charge code Start by Hina Sharma on 04-07-2024-----------------*/
        var Cal_Tds_Amt = 0;
        $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
            // debugger;
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
            // debugger;
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
                    , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: IsNull(bill_no, '')
                    , bill_date: IsNull(bill_dt, ''), acc_id: ""
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
            $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this);
                var TaxID = currentRow.find("#TaxNameID").text();
                var TaxAccID = currentRow.find("#TaxAccId").text();
                var TaxItmCode = currentRow.find("#TaxItmCode").text();
                var DocNo = currentRow.find("#DocNo").text();
                TaxID = "R" + TaxID;
                var TaxPerc = currentRow.find("#TaxPercentage").text();
                var TaxPerc_id = TaxPerc.replace("%", "");
                var TaxVal = currentRow.find("#TaxAmount").text();

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
        hideLoader();
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
    
    //var glData = JSON.stringify(GLDetail);

    //await $.ajax({
    //    type: "POST",
    //    url: "/Common/Common/GetGLDetails_StringData",
    //    //contentType: "application/json; charset=utf-8",
    //    //dataType: "json",
    //    data: { GLDetail: glData },
    //    success: function (data) {
    //        debugger;
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
    //                        if (supp_type != "TSupp") { /*chnges for tds on OC by hina sharma on 10-07-2024*/
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
    //                                    if (ArrGlDetailsDr[i].type == "Disc") {/*chnges for Discount by Suraj Maurya 25-10-2024*/
    //                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                            , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,0, 0,
    //                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                        )
    //                                    } else {
    //                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
    //                                            , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
    //                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
    //                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
    //                                        )
    //                                    }

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
    //            CalculateVoucherTotalAmount();
    //        }
    //    }
    //});
}
function Get_Gl_Narration(VouType, bill_no, bill_date,type) {
    let Narration = "";
    switch (VouType) {
        case "DN":
            if (type == "TCust" || type == "Tcs")
                Narration = $("#hdn_DN_Narration_Tcs").val();
            else
                Narration = $("#hdn_DN_Narration").val();
            break;
        case "BP":
            Narration = $("#hdn_BP_Nurration").val();
            break;
        case "PV":
            Narration = $("#hdn_PV_Nurration").val();
            break;
        case "CN":
            Narration = $("#hdn_CN_Nurration").val();
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

function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {
    var Dmenu = $("#DocumentMenuId").val();
    if (Dmenu == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }
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
async function CalculateVoucherTotalAmount() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        ValDecDigit = $("#ValDigit").text();
    }


    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
            DrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmt) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
        }
        else {
            //DrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())), ValDecDigit)).toFixed(ValDecDigit);
            DrTotAmt = parseFloat(getvalwithoutroundoff((cmn_getsumval(DrTotAmt, currentRow.find("#dramt").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
        }
        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
            DrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmtInBase) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
        }
        else {
            //DrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())), ValDecDigit)).toFixed(ValDecDigit);
            DrTotAmtInBase = parseFloat(getvalwithoutroundoff((cmn_getsumval(DrTotAmtInBase, currentRow.find("#dramtInBase").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
            CrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmt) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
        }
        else {
            //CrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())), ValDecDigit)).toFixed(ValDecDigit);
            CrTotAmt = parseFloat(getvalwithoutroundoff((cmn_getsumval(CrTotAmt, currentRow.find("#cramt").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
            CrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmtInBase) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
        }
        else {
            //CrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())), ValDecDigit)).toFixed(ValDecDigit);
            CrTotAmtInBase = parseFloat(getvalwithoutroundoff((cmn_getsumval(CrTotAmtInBase, currentRow.find("#cramtInBase").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
        }
    });
    $("#DrTotalInBase").text(DrTotAmt);
    $("#DrTotalInBase").text(DrTotAmtInBase);
    $("#CrTotalInBase").text(CrTotAmt);
    $("#CrTotalInBase").text(CrTotAmtInBase);

    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        debugger;
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            debugger;
            await AddRoundOffGL();
        }
    }
}
async function AddRoundOffGL() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpValDigit").text();///Amount
    }
    else {
        ValDecDigit = $("#ValDigit").text();///Amount
    }

    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                debugger;
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    //$("#VoucherDetail >tbody >tr #hfAccID[value='" + arr.Table[j].acc_id + "']").SrcShflst('tr').remove();
                    $("#VoucherDetail >tbody >tr").each(function () {
                        var currentRow = $(this);
                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
                            DrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmt) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        else {
                            //DrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())), ValDecDigit)).toFixed(ValDecDigit);
                            DrTotAmt = parseFloat(getvalwithoutroundoff((cmn_getsumval(DrTotAmt, currentRow.find("#dramt").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
                            CrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmt) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        else {
                            //CrTotAmt = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())), ValDecDigit)).toFixed(ValDecDigit);
                            CrTotAmt = parseFloat(getvalwithoutroundoff((cmn_getsumval(CrTotAmt, currentRow.find("#cramt").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
                            DrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmtInBase) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        else {
                            //DrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())), ValDecDigit)).toFixed(ValDecDigit);
                            DrTotAmtInBase = parseFloat(getvalwithoutroundoff((cmn_getsumval(DrTotAmtInBase, currentRow.find("#dramtInBase").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
                            CrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmtInBase) + parseFloat(0)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                        else {
                            //CrTotAmtInBase = parseFloat(getvalwithoutroundoff((parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())), ValDecDigit)).toFixed(ValDecDigit);
                            CrTotAmtInBase = parseFloat(getvalwithoutroundoff((cmn_getsumval(CrTotAmtInBase, currentRow.find("#cramtInBase").text(), ValDecDigit)), ValDecDigit)).toFixed(ValDecDigit);
                        }
                    });
                    debugger;
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
                                    //var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;

                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });
                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
                                    /* GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id*/

                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , Diff, DiffInBase, 0, 0, "SV"
                                        , $("#hdcurr").val(), $("#conv_rate").val(), '', '')
                                    /*, $("#ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())*/

                                    //var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    //vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);

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
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
                                    //var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;

                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });
                                    /*Changes by Hina sharma on 10-07-2024 to calculate round off in correct manner */
                                    /* GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id*/
                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , 0, 0, Diff, DiffInBase, "SV"
                                        , $("#hdcurr").val(), $("#conv_rate").val(), '', '')
                                    /*, $("#ddlCurrency").val(), $("#conv_rate").val(), $("#Bill_No").val(), $("#Bill_Date").val())*/

                                    //var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    //vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
                                    //vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);

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
                    debugger;
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
//function CalculateVoucherTotalAmount() {
//    //debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        debugger;
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
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//        AddRoundOffGL();
//    }
//}
//function AddRoundOffGL() {
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/LocalSaleInvoice/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
//            debugger;
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                debugger;
//                if (arr.Table.length > 0) {
//                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    $("#VoucherDetail >tbody >tr").each(function () {
//                        //debugger;
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
//                    debugger;
//                    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//                        if (parseFloat(DrTotAmt) < parseFloat(CrTotAmt)) {
//                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" /></td>
//                                     <input type="hidden" id="type" value="RO"/>
//                                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                            </tr>`);
//                                }
//                            }
//                        }
//                        else {
//                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" /></td>
//                                     <input type="hidden" id="type" value="RO"/>
//                                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                            </tr>`);
//                                }
//                            }
//                        }
//                    }
//                    debugger;
//                    CalculateVoucherTotalAmount();
//                }
//            }
//        }
//    });
//}
//function GetAllGLID() {
//    debugger;
//    var cust_id = $("#Cust_NameList").val();
//    var CustVal = 0;
//    var Compid = $("#CompID").text();
//    var _SIType = "";
//    if ($("#OrderTypeD").is(":checked")) {
//        _SIType = "D";
//    }
//    if ($("#OrderTypeE").is(":checked")) {
//        _SIType = "E";
//    }
//    var TransType = 'Sal';
//    var GLDetail = [];
//    var TxaExantedItemList = [];
//    GLDetail.push({ comp_id: Compid, id: cust_id, type: "Cust", doctype: _SIType, Value: CustVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Cust" });
//    $("#SInvItmDetailsTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var item_id = currentRow.find("#hfItemID").val();
//        var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
//        if (currentRow.find("#TaxExempted").is(":checked")) {
//            TxaExanted = "Y";
//            TxaExantedItemList.push({ item_id: item_id });
//        }
//        GLDetail.push({ comp_id: Compid, id: item_id, type: "Itm", doctype: _SIType, Value: ItmGrossVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm" });
//    });
//    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#TaxNameID").text();
//        var tax_amt = currentRow.find("#TaxAmount").text();
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: _SIType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#taxID").text();
//        var tax_amt = currentRow.find("#TotalTaxAmount").text();
//        //var TaxItmCode = currentRow.find("#TaxItmCode").text();
//        //if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
//        //if (GLDetail.findIndex(obj => obj.id == tax_id) > -1) {
//        //objIndex = GLDetail.findIndex((obj => obj.id == tax_id));
//        //GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
//        //}
//        //else {
//        //GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: _SIType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//        //}
//        //}
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: _SIType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Tbl_OtherChargeList >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var oc_id = currentRow.find("#OCID").text();
//        var oc_amt = currentRow.find("#OCAmtSp1").text();
//        GLDetail.push({ comp_id: Compid, id: oc_id, type: "OC", doctype: _SIType, Value: oc_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC" });
//    });
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/LocalSaleInvoice/GetGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({ GLDetail: GLDetail }),
//        success: function (data) {
//            debugger;
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
//                        //debugger;
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
//                            //debugger;
//                            var Acc_Id = arr.Table[j].acc_id;
//                            acc_Id = Acc_Id.substring(0, 1);
//                            var Disable;
//                            if (acc_Id == "3" || acc_Id == "4") {
//                                Disable = "";
//                            }
//                            else {
//                                Disable = "disabled";
//                            }
//                            var FieldType = "";
//                            var hdnAccID = $("#hdnAccID").val();
//                            if (hdnAccID != null && hdnAccID != "") {
//                                if (arr.Table[j].type == 'Itm') {
//                                    FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Acc_name_${rowIdx}" onchange ="OnChangeAccountName(${rowIdx},event)">
//                                        </select>
//                                    <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" /></div>`;
//                                    //<input  type="hidden" id="hfAccID"  value="${hdnAccID}" /></div> `;
//                                    $("#hdnAccID").val(arr.Table[j].acc_id);
//                                    //$("#hdnAccID").val(hdnAccID);
//                                }
//                                else {
//                                    FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" />`;
//                                }
//                            }
//                            else {
//                                if (arr.Table[j].type == 'Itm') {
//                                    FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Acc_name_${rowIdx}" onchange ="OnChangeAccountName(${rowIdx},event)">
//                                        </select>
//                                    <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" /></div> `;
//                                    $("#hdnAccID").val(arr.Table[j].acc_id);
//                                }
//                                else {
//                                    FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" />`;
//                                }
//                            }
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                     <td>`+ FieldType + `</td>                                  
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td style="display:none"><input type="hidden" id="SNohf" value="${rowIdx}"/></td>
//                                    <td class="center">
//                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>
//                                    <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>

//                            </tr>`);
//                            }
//                            /*<td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>*/
//                        }
//                        $("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat($("#NetOrderValueSpe").val()).toFixed(ValDecDigit));
//                    }
//                }

//                CalculateVoucherTotalAmount();
//                BindDDLAccountList();
//            }

//        }
//    });
//}
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105103140");
}
function BindData() {
    debugger;
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                $("#Acc_name_" + rowid).empty();
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="TextAccDdl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#TextAccDdl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Acc_name_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Acc_name_" + (rowid)).val();
                        if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                            /* var UOM = $(data.element).data('uom');*/
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
                                '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });
            });
            $("#VoucherDetail >tbody >tr").each(function (i, row) {
                try {
                    var currentRow = $(this);
                    var AccID = currentRow.find("#hfAccID").val();
                    var rowid = currentRow.find("#SNohf").val();
                    if (AccID != '0' && AccID != "") {
                        currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
                    }
                }
                catch {
                    hideLoader();
                }

            });
        }
    }

}
function OnChangeAccountName(RowID, e) {
    debugger;
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
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
        //Added by Suraj on 09-08-2024 to stop reset of GL Account if user changes the GL Acc.
        $("#SInvItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
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
    clickedrow.find("#hdnAccID").val(Acc_ID);
    $("#hdnAccID").val(Acc_ID);
}


function OnClickbillingAddressIconBtn(e) {
    // debugger;

    $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());
    var Cust_id = $('#Cust_NameList').val();
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
    var SO_no = $("#so_no").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, SODTransType);
}
function OnClickShippingAddressIconBtn(e) {
    // debugger;
    $('#AddressInfo_LblName').text($('#AddressInfo_ShipAdd').text());
    var Cust_id = $('#Cust_NameList').val();
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
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrderQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var inv_type = '';
    var DocumentMenuId = $("#DocumentMenuId").val();
    var invtyp = $("#rbDomestic").val();
    var invtype = $("#rbExport").val();
    if (invtyp || invtype) {
        if (invtyp == 'D') {
            inv_type = 'D'
        }
        else {
            inv_type = 'E'
        }
    }
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#TxtItemName").val();
    var ProductId = clickdRow.find("#hfItemID").val();
    var UOM = clickdRow.find("#TxtUOM").val();
    var SrcDoc_no = $("#TxtShipmentNo").val();
    var SrcDoc_dt = $("#TxtShipmentDate").val().split("-").reverse().join("-");
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();

    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = flag == "ShippedFoc" ? (row.find('#subItemFocQty').val() || "0") : row.find('#subItemQty').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#QuotQty").val();
    }
    else if (flag == "Shipped") {
        Sub_Quantity = clickdRow.find("#TxtReceivedQuantity").val();
    }
    else if (flag == "ShippedFoc") {
        Sub_Quantity = clickdRow.find("#TxtFocQuantity").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfSIStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            SrcDoc_no: SrcDoc_no,
            SrcDoc_dt: SrcDoc_dt,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            DocumentMenuId: DocumentMenuId
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
    return Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        ValDecDigit = $("#ExpImpRateDigit").text();
    }
    else {
        ValDecDigit = $("#RateDigit").text();
    }
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramt").text();
    var CstCrtAmt = clickedrow.find("#cramt").text();
    //if (DocumentMenuId == "105103145125") {
    //    CstDbAmt = clickedrow.find("#cramtInBase").text();
    //    CstCrtAmt = clickedrow.find("#cramtInBase").text();
    //}
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
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            DocumentMenuId: DocumentMenuId
        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}

//-------------------Cost Center Section End-------------------//

function onchangeBankName() { // add this function Nitesh 21-10-2023 1426 for headoffice onchnge
    debugger;

    var bankName = $("#ddlBankName").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/getbankdetail",
        dataType: "json",
        data: { bankName: bankName, },

        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);
                var length = arr.Table.length; /*this is use for json data braking in array type and put in a Array*/
                if (length > 0) {
                    $("#AccountNumber").val(arr.Table[0].acc_no);
                    $("#BankAddress").val(arr.Table[0].bank_addr);
                    $("#SWIFT_Code").val(arr.Table[0].swift_code);
                    $("#IFSC_Code").val(arr.Table[0].ifsc_code);
                }
            }
        },
        error: function (Data) {
        }
    });
}

/*--------------------------Print Type Start-------------------------*/
function OnClickPrintBtn() {
    debugger;
    let PrintOptions = $("#span_Hdn_ExportCommercialPrintCustomOptions").text();
    let DocMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetSelectPrintTypePopup",
        data: { PrintOptions: PrintOptions, DocMenuId: DocMenuId },
        success: function (data) {
            debugger;
            $("#PopupSelectPrintType").html(data);
            SelectPrintType();
        }
    })
    return false;
}
function OnClickPrint() {
    let Selected = "N";
    let PrintType = "";
    //$("#Table_SelectPrintType tbody tr").each(function () {
    //    var Row = $(this);
    //    if (Row.find("#Rdo_PackType").prop("checked")) {
    //        Selected = "Y";
    //        PrintType = Row.find("#Rdo_PackType").val();
    //    }
    //})
    //if (Selected == "Y") {
    let ErFlg = "N";
    if ($("#ChkPrtOpt_catlog_number").is(":checked")) {
        ErFlg = "Y";
        $("#PrtOpt_catlog_number").val("Y");
    } else {
        $("#PrtOpt_catlog_number").val("N");
    }

    if ($("#ChkPrtOpt_item_code").is(":checked")) {
        ErFlg = "Y";
        $("#PrtOpt_item_code").val("Y");
    } else {
        $("#PrtOpt_item_code").val("N");
    }

    if ($("#ChkPrtOpt_item_desc").is(":checked")) {
        ErFlg = "Y";
        $("#PrtOpt_item_desc").val("Y");
    } else {
        $("#PrtOpt_item_desc").val("N");
    }
    //if (PrintType == "Commercial") {
    if (ErFlg == "N") {
        swal("", $("#span_PleaseselectAtleastOnePrintOption").text(), "warning");
        return false;
    }
    //}
    $("#PrintButtonDiv").html('<button hidden type="submit" id="HdnButtonToPrint" name="Command" hidden value="Print" >Print</button>')
    $("#OKBtn_PrintSelect").attr("onclick", "");
    $("#OKBtn_PrintSelect").attr("data-dismiss", "modal");
    //$("form").submit();
    $("#HdnButtonToPrint").click();
    //} else {
    //    swal("", $("#span_PleaseSelectPrintType").text(), "warning");
    //}


}

function SelectPrintType() {
    $("#Table_SelectPrintType tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");

        if (clickedrow.find('#Rdo_PackType').prop("disabled") == false) {
            $("#Table_SelectPrintType >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
            clickedrow.find('#Rdo_PackType').prop("checked", true);
            if (clickedrow.find('#Rdo_PackType').val() == "Commercial") {
                $("#Div_CustomPrintOptions").css("display", "block");
            }
            else {
                $("#Div_CustomPrintOptions").css("display", "none");
            }
        }
    });
}
/*--------------------------Print Type End-------------------------*/

function OnchangeInvoiceRate(e) {
    var CRow = $(e.target).closest("tr");
    let ShQty = CRow.find("#TxtReceivedQuantity").val();
    let InvRate = CRow.find("#TxtInvoiceRate").val();
    let InvDiscPer = parseFloat(CheckNullNumber($("#OrderDiscountInPercentage").val()));
    let InvDiscAmt = parseFloat(CheckNullNumber($("#OrderDiscountInAmount").val()));
    if (InvDiscPer > 0) {
        let discVal = ((parseFloat(CheckNullNumber(ShQty)) * parseFloat(CheckNullNumber(InvRate))) * InvDiscPer) / 100;
        CRow.find("#item_disc_val").val(parseFloat(CheckNullNumber(discVal)).toFixed(ValDecDigit));
    }
    if (parseFloat(InvDiscAmt) > parseFloat(0)) {
        Calculate_DiscAmountItemWise(InvDiscAmt);
    }
    let Disc = CRow.find("#item_disc_val").val();
    let OCVal = CRow.find("#TxtOtherCharge").val();
    let conv_rate = $("#conv_rate").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        RateDecDigit = $("#ExpImpRateDigit").text();
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        RateDecDigit = $("#RateDigit").text();
        ValDecDigit = $("#ValDigit").text();
    }
    CRow.find("#TxtInvoiceRate").val(parseFloat(CheckNullNumber(InvRate)).toFixed(RateDecDigit));
    if (parseFloat(CheckNullNumber(InvRate)) > 0) {
        CRow.find("#itemInvoiceAmtError").css("display", "none");
        CRow.find("#TxtInvoiceRate").css("border-color", "#ced4da");
        CRow.find("#itemInvoiceAmtError").text("");
    } else {
        CRow.find("#itemInvoiceAmtError").css("display", "block");
        CRow.find("#TxtInvoiceRate").css("border-color", "red");
        CRow.find("#itemInvoiceAmtError").text($("#valueReq").text());
        return false;
    }


    let GrVal = parseFloat(getvalwithoutroundoff((cmn_getmultival(parseFloat(CheckNullNumber(ShQty)), parseFloat(CheckNullNumber(InvRate))) - parseFloat(CheckNullNumber(Disc))), ValDecDigit)) /*+ parseFloat(CheckNullNumber(OCVal))*/;
    if (GrVal > 0) {

        CRow.find("#TxtItemGrossValue").val((GrVal).toFixed(ValDecDigit));
        CRow.find("#TxtNetValue").val((GrVal).toFixed(ValDecDigit));
        CRow.find("#TxtNetValueInBase").val(parseFloat(getvalwithoutroundoff(cmn_getmultival(GrVal, parseFloat(CheckNullNumber(conv_rate))), ValDecDigit)).toFixed(ValDecDigit));
        CRow.find("#itemInvoiceAmtError").css("display", "none");
        CRow.find("#TxtInvoiceRate").css("border-color", "#ced4da");
        CRow.find("#itemInvoiceAmtError").text("");

    } else {
        CRow.find("#itemInvoiceAmtError").css("display", "block");
        CRow.find("#TxtInvoiceRate").css("border-color", "red");
        CRow.find("#itemInvoiceAmtError").text($("#span_InvalidPrice").text());
    }

    //let TotalOCAmount = $("#TxtOtherCharges").val();//Commented by Suraj Maurya on 04-01-2025
    let TotalOCAmount = $("#TxtDocSuppOtherCharges").val();
    if (DocumentMenuId == "105103145125") {//Added by Suraj Maurya on 19-03-2025
        TotalOCAmount = $("#TxtOtherCharges").val();
    }
    
    CalculateAmount();
    Calculate_OC_AmountItemWise(TotalOCAmount);
    if (DocumentMenuId != "105103145125") {
        if (DocumentMenuId == "105103140") {
            let Cust_Id = $("#Cust_NameList").val();
            let GrossValue = $("#TxtGrossValue").val();
            AutoTdsApply(Cust_Id, GrossValue).then(() => {
                OnClickTaxExemptedCheckBox(CRow, "InvoiceRate");
                //CalculateAmount();
                //GetAllGLID();
            });
        } else {
            OnClickTaxExemptedCheckBox(CRow, "InvoiceRate");
        }
    }
    else {
        GetAllGLID();
    }

    
    
    //$("#SInvItmDetailsTbl > tbody > tr").each(function () {
    //    var row = $(this);
    //    var TxtOtherCharge = row.find("#TxtOtherCharge").val();
    //    if (parseFloat(CheckNullNumber(TxtOtherCharge)) > 0) {
    //        Calculate_OC_AmountItemWise(TotalOCAmount);
    //    }
    //});
    //BindOtherChargeDeatils("");
    //GetAllGLID();
}

/*----------------- export items list to excel ------------------*/
function ExportItemsToExcel() {
    debugger;
    var docId = $('#DocumentMenuId').val();
    var invNo = $('#hdnINVNo').val();
    var invDate = $('#Inv_date').val();
    if (invNo != null && invNo != "" && invNo != undefined) {
        window.location.href = "/ApplicationLayer/LocalSaleInvoice/ExportItemsToExcel?invNo=" + invNo + "&invDate=" + invDate + "&docId=" + docId;
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

//function OnCheckedChangeProdDesc() {
//    $('#chkshowcustspecname').
//}

function OnCheckedChangeProdDesc() {
    if ($('#chkproddesc').prop('checked')) {
        $('#chkshowcustspecproddesc').prop('checked', false);
        $('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $("#ShowProdDesc").val("Y");
        $('#ShowCustSpecProdDesc').val('N');
        $('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        $('#chkproddesc').prop('checked', false);
        $('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $('#ShowCustSpecProdDesc').val('Y');
        $('#ShowProdDesc').val('N');
        $('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeItemAliasName() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkitmaliasname').prop('checked')) {
        $('#chkproddesc').prop('checked', false);
        $('#chkshowcustspecproddesc').prop('checked', false);
        $('#ItemAliasName').val('Y');
        $("#ShowProdDesc").val('N');
        $('#ShowCustSpecProdDesc').val('N');
    }
    else {
        $('#ItemAliasName').val('N');
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
    debugger;
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
    debugger;
    if ($('#chkcustaliasname').prop('checked')) {
        $('#CustomerAliasName').val('Y');
    }
    else {
        $('#CustomerAliasName').val('N');
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
function OnChngNoofCopy() {
    debugger;
    var Noofcopy = $('#txt_NoOfCopies').val();
    if (Noofcopy != "0" || Noofcopy != "" || Noofcopy != null) {
        if (Noofcopy > 0 && Noofcopy <= 4) {
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
function PrintInvoice() {
    debugger;
    var Docid = $("#DocumentMenuId").val();
    if (Docid == "105103140") {
        var ValidInfo = "N";
        var Noofcopy = $('#txt_NoOfCopies').val();
        if (Noofcopy == "0" || Noofcopy == "" || Noofcopy == null) {
            $("#txt_NoOfCopies").css("border-color", "Red");
            $('#SpanNoOfCopies').text($("#valueReq").text());
            $("#SpanNoOfCopies").css("display", "block");
            ValidInfo = "Y";
        }
        else {
            $('#NumberOfCopy').val(Noofcopy);
            $("#txt_NoOfCopies").css("border-color", "#ced4da");
            //$('#SpanNoOfCopies').text($("#valueReq").text());
            $("#SpanNoOfCopies").css("display", "none");
        }
        if (ValidInfo == "Y") {
            //$("#btn_print").attr("data-target", "");
            $("#btn_print").attr("data-toggle", "");
            return false;
        }
        else {
            //$("#btn_print").attr("data-target", "modal");
            $("#btn_print").attr("data-toggle", "modal");
            //$('form').submit();

            return true;
        }
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

/*code wrote by sanjay on 20-05-2024 */
function ValidateCommercialInvoiceAmountBeforeSubmit() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        RateDecDigit = $("#ExpImpRateDigit").text();
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        RateDecDigit = $("#RateDigit").text();
        ValDecDigit = $("#ValDigit").text();
    }
    /*var chkNonTax = $('#nontaxable').prop('checked');*/
    var netSpecAmt = parseFloat($('#NetOrderValueSpe').val()).toFixed(ValDecDigit);
    var convRate = parseFloat($('#conv_rate').val()).toFixed(ValDecDigit);
    var netBaseAmt = parseFloat(getvalwithoutroundoff($('#NetOrderValueInBase').val(), 0)).toFixed(ValDecDigit);

    //var amtAfterConvRate = parseFloat(getvalwithoutroundoff(cmn_getmultival(parseFloat(netSpecAmt) , parseFloat(convRate)), 0 /*ValDecDigit //Commented by Suraj on 19-07-2024*/)).toFixed(ValDecDigit);//commented by Suraj maurya 11-10-2024
    var amtAfterConvRate = parseFloat(getvalwithoutroundoff(getvalwithoutroundoff((parseFloat(netSpecAmt) * parseFloat(convRate)), ValDecDigit), 0 /*ValDecDigit //Commented by Suraj on 19-07-2024*/)).toFixed(ValDecDigit);

    if (amtAfterConvRate != netBaseAmt) {
        swal("", $("#NetAmtSpecDidntMatchWithNetAmtBase").text(), "warning");
        return false;
    }
    //else {
    //    /* $('form').submit();*/
    //    return true;
    //}
    var grValue = parseFloat($('#TxtGrossValue').val()).toFixed(ValDecDigit);
    var taxAmt = parseFloat($('#TxtTaxAmount').val()).toFixed(ValDecDigit);
    var oc_Amount = $('#TxtOtherCharges').val();

    var netAmt = parseFloat(getvalwithoutroundoff($('#NetOrderValueSpe').val(), 2)).toFixed(2);
    var calculatedAmt = parseFloat(getvalwithoutroundoff(parseFloat(parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(oc_Amount))), 2)).toFixed(2);
    if (netAmt != calculatedAmt) {
        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    return true;
}
function ValidateSaleInvAmountBeforeSubmit() {
    debugger;
    /* TcsAmt Added by Suraj Maurya on 03-02-2025 */
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        RateDecDigit = $("#ExpImpRateDigit").text();
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        RateDecDigit = $("#RateDigit").text();
        ValDecDigit = $("#ValDigit").text();
    }
    var grValue = parseFloat($('#TxtGrossValue').val()).toFixed(ValDecDigit);
    var taxAmt = parseFloat($('#TxtTaxAmount').val()).toFixed(ValDecDigit);
    var oc_Amount = $('#TxtOtherCharges').val();
    var TcsAmt = CheckNullNumber($('#TxtTDS_Amount').val());
    if (oc_Amount == "") {
        var FinalocAmt = parseFloat(0).toFixed(ValDecDigit);
    }
    else {
        var FinalocAmt = parseFloat($('#TxtOtherCharges').val()).toFixed(ValDecDigit);
    }
    var excludeSuppchrg = $('#TxtDocSuppOtherCharges').val();
    if (excludeSuppchrg == "") {
        var ocAmt = parseFloat(0).toFixed(ValDecDigit);
    }
    else {
        var ocAmt = parseFloat($('#TxtDocSuppOtherCharges').val()).toFixed(ValDecDigit);
    }

    var FRoundOffAmt = $("#hdn_FRoundOffAmt").val();
    var ThirdPartyOC = FinalocAmt - ocAmt
    var netAmt = parseFloat($('#NetOrderValueInBase').val()).toFixed(ValDecDigit);

    //var calculatedAmt = parseFloat(parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt))).toFixed(ValDecDigit);
    //if ($("#p_round").is(":checked")) {
    //    var calculatedAmt = parseFloat((parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt))) + (parseFloat(ThirdPartyOC) + parseFloat(FRoundOffAmt))).toFixed(ValDecDigit);

    //      }
    //if ($("#m_round").is(":checked")) {
    //    var calculatedAmt = parseFloat((parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt))) - (parseFloat(ThirdPartyOC) + parseFloat(FRoundOffAmt))).toFixed(ValDecDigit);
    //}
    if ($("#chk_roundoff").is(":checked")) {
        if ($("#p_round").is(":checked")) {
            var calculatedAmt = parseFloat(getvalwithoutroundoff(parseFloat((parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt)) + parseFloat(CheckNullNumber(TcsAmt))) + (parseFloat(CheckNullNumber(FRoundOffAmt)))), ValDecDigit)).toFixed(ValDecDigit);

        }
        if ($("#m_round").is(":checked")) {
            var calculatedAmt = parseFloat(getvalwithoutroundoff(parseFloat((parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt)) + parseFloat(CheckNullNumber(TcsAmt))) - (parseFloat(CheckNullNumber(FRoundOffAmt)))), ValDecDigit)).toFixed(ValDecDigit);
        }
    }
    else {
        var calculatedAmt = parseFloat(getvalwithoutroundoff(parseFloat(parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt)) + parseFloat(CheckNullNumber(TcsAmt))), ValDecDigit)).toFixed(ValDecDigit);
    }
    if (Math.abs(netAmt - calculatedAmt)>1) {
        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
        return false;
    }
    else {
        //$("#hdn_FRoundOffAmt").val("");
        /*  $('form').submit();*/
        return true;
    }
}
//function ValidateSaleInvAmountBeforeSubmit() {
//    var grValue = parseFloat($('#TxtGrossValue').val()).toFixed(ValDecDigit);
//    var taxAmt = parseFloat($('#TxtTaxAmount').val()).toFixed(ValDecDigit);
//    //var ocAmt = parseFloat($('#TxtOtherCharges').val()).toFixed(ValDecDigit);
//    var ocAmt = parseFloat($('#TxtDocSuppOtherCharges').val()).toFixed(ValDecDigit);
//    var netAmt = parseFloat($('#NetOrderValueInBase').val()).toFixed(ValDecDigit);
//    var calculatedAmt = parseFloat(parseFloat(grValue) + parseFloat(taxAmt) + parseFloat(CheckNullNumber(ocAmt))).toFixed(ValDecDigit);
//    if (netAmt != calculatedAmt) {
//        swal("", $("#GrPlsTaxPlsOcAmtDidntMatchWithNetAmt").text(), "warning");
//        return false;
//    }
//    else {
//        /*  $('form').submit();*/
//        return true;
//    }
//}

/***-------------------Roundoff------------------------------------***/
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
    debugger;
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
            //    debugger;
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
async function click_chkplusminus() {
    debugger;
    CalculateAmount();/*Added by Hina on 11-07-2024*/
    //var ValDecDigit = $("#ValDigit").text();
    if ($("#chk_roundoff").is(":checked")) {
        await addRoundOffToNetValue();
    }
    else {
        $("#div_pchkbox").hide();
        $("#div_mchkbox").hide();
        $("#pm_flagval").val("");
        $("#p_round").prop('checked', false);
        $("#m_round").prop('checked', false);

        //$("#ServicePIItemTbl > tbody > tr").each(function () {
        //    debugger;
        //    var currentrow = $(this);
        //    CalculateTaxExemptedAmt(currentrow, "N")
        //});
        CalculateAmount();
        GetAllGLID();
    }
}
async function addRoundOffToNetValue(flag) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        RateDecDigit = $("#ExpImpRateDigit").text();
        ValDecDigit = $("#ExpImpValDigit").text();
    }
    else {
        RateDecDigit = $("#RateDigit").text();
        ValDecDigit = $("#ValDigit").text();
    }
    try {
        await $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LocalPurchaseInvoice/CheckRoundOffAccount",
                data: {},
                success: function (data) {
                    debugger;
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
                                    var tcsAmt = CheckNullNumber($("#TxtTDS_Amount").val());

                                    var finalnetval = parseFloat(getvalwithoutroundoff((parseFloat(grossval) + parseFloat(taxval) + parseFloat(ocval) + parseFloat(tcsAmt)), ValDecDigit)).toFixed(ValDecDigit);

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
                                                /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                                                var FRoundOffAmt = parseFloat(faddval);
                                                $("#hdn_FRoundOffAmt").val(FRoundOffAmt);

                                                //let valInBase = $("#TxtGrossValue").val();
                                                //$("#TxtGrossValue").val((parseFloat(valInBase) + parseFloat(faddval)).toFixed(ValDecDigit))
                                            }
                                            if ($("#m_round").is(":checked")) {
                                                //var finnetval = netval.split('.');
                                                var decval = '0.' + finnetval[1];
                                                fnetval = parseFloat(netval) - parseFloat(decval);
                                                $("#pm_flagval").val($("#m_round").val());
                                                /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                                                var FRoundOffAmt = parseFloat(decval);
                                                $("#hdn_FRoundOffAmt").val(FRoundOffAmt);

                                                //let valInBase = $("#TxtGrossValue").val();
                                                //$("#TxtGrossValue").val((parseFloat(valInBase) - parseFloat(decval)).toFixed(ValDecDigit))
                                            }
                                            var roundoff_netval = Math.round(fnetval);
                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                            $("#NetOrderValueInBase").val(f_netval);
                                            $("#NetOrderValueSpe").val(f_netval);
                                            if (flag == "CallByGetAllGL") {
                                                //do not call  GetAllGLID("RO");
                                            } else {
                                                GetAllGLID("RO");
                                            }
                                        }
                                        else {
                                            /*Add by Hina on 16-07-2024 for check ValidateSaleInvAmountBeforeSubmit */
                                            $("#hdn_FRoundOffAmt").val("");
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
/***----------------------end-------------------------------------***/

/***-------------------Orderwise Discount------------------------------------***/
/*Added by Suraj Maurya on 16-10-2024*/
function OnchangeDiscInPerc() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var DecDigit = $("#ValDigit").text();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }

    var Perc = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(Perc)) > parseFloat(0)) {
        DisableDiscountAmt();
        $("#OrderDiscountInAmount").attr("disabled", true);
    }
    else {
        EnableDiscountAmt();
        $("#OrderDiscountInAmount").attr("disabled", false);
    }
    if (Perc == "") {
        Perc = "0";
    }
    var TOtalGrossVal = parseFloat(0).toFixed(DecDigit);
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var sip_qty = currentRow.find("#TxtReceivedQuantity").val();
        var inv_rate = CheckNullNumber(currentRow.find("#TxtInvoiceRate").val());
        var GrossVal = parseFloat(sip_qty) * parseFloat(inv_rate)
        var DiscVal = ((parseFloat(GrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
        if (parseFloat(Perc) > 0) {
            currentRow.find("#item_disc_perc").val(parseFloat(Perc).toFixed(2));
        } else {
            currentRow.find("#item_disc_perc").val("");
        }
        currentRow.find("#item_disc_amt").val("");
        currentRow.find("#item_disc_val").val(parseFloat(DiscVal).toFixed(DecDigit));
        //var GrVal = parseFloat(parseFloat(GrossVal).toFixed(DecDigit) - parseFloat(DiscVal).toFixed(DecDigit)).toFixed(DecDigit);
        currentRow.find("#TxtItemGrossValue").val(parseFloat(GrossVal).toFixed(DecDigit));

        TOtalGrossVal = parseFloat(TOtalGrossVal) + parseFloat(GrossVal);
        CalculateOrderDiscount(currentRow,"NoCalc");
    });

    var PercDiscVal = ((parseFloat(TOtalGrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
    if (parseFloat(PercDiscVal) == 0) {
        $("#OrderDiscountInPercentage").val("");
    }
    else {
        $("#OrderDiscountInPercentage").val(parseFloat(Perc).toFixed(2));
    }
    //$("#SInvItmDetailsTbl >tbody >tr").each(function () {//commented by SUraj maurya on 12-02-2025
    //    debugger;
    //    var currentRow = $(this);
    //    CalculateOrderDiscount(currentRow);
    //});
    CalculateAmount();
    let custId = $("#Cust_NameList").val();
    let grValForTcs = CheckNullNumber($("#TxtGrossValue").val())
    AutoTdsApply(custId, grValForTcs).then(() => {//Added by Suraj maurya on 11-02-2025
        GetAllGLID();
    });
    
}
function OnchangeDiscInAmt() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var DecDigit = $("#ValDigit").text();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    var Amt = parseFloat(CheckNullNumber($("#OrderDiscountInAmount").val()));
    if (Amt > 0) {
        DisableDiscountAmt();
        $("#OrderDiscountInPercentage").attr("disabled", true);
    }
    else {
        EnableDiscountAmt();
        $("#OrderDiscountInPercentage").attr("disabled", false);
    }
    if (Amt == "") {
        Amt = "0";
    }
    //var TOtalGrossVal = parseFloat(0).toFixed(DecDigit);
    //$("#SInvItmDetailsTbl >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    var ord_qty_spec = CheckNullNumber(currentRow.find("#TxtReceivedQuantity").val());
    //    var item_rate = CheckNullNumber(currentRow.find("#TxtInvoiceRate").val());
    //    var GrossVal = parseFloat(ord_qty_spec) * parseFloat(item_rate);
    //    currentRow.find("#TxtItemGrossValue").val(parseFloat(GrossVal).toFixed(DecDigit));
    //    currentRow.find("#item_disc_perc").val("");
    //    currentRow.find("#item_disc_amt").val("");
    //    TOtalGrossVal = parseFloat(TOtalGrossVal) + parseFloat(GrossVal);
    //});
    //var TxtGrossValue = TOtalGrossVal;
    //var TxtGrossValue1 = (parseFloat(TxtGrossValue) - parseFloat(Amt)).toFixed(DecDigit);
    if (Amt == 0) {
        $("#OrderDiscountInAmount").val("");
    }
    else {
        $("#OrderDiscountInAmount").val(parseFloat(Amt).toFixed(DecDigit));
    }
    //$("#TxtGrossValue").val(parseFloat(TxtGrossValue1).toFixed(DecDigit));
    if (Amt != "" && Amt != null) {
        if (parseFloat(Amt) > parseFloat(0)) {
            Calculate_DiscAmountItemWise(Amt);
        }
        else {
            SetDiscInAmt();
            Calculate_DiscAmountItemWise(Amt);
        }
    }
    //GetAllGLID();
    let custId = $("#Cust_NameList").val();
    let grValForTcs = CheckNullNumber($("#TxtGrossValue").val())
    AutoTdsApply(custId, grValForTcs).then(() => {//Added by Suraj maurya on 11-02-2025
        GetAllGLID();
    });
}
function SetDiscInAmt() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#item_disc_val").val((parseFloat(0)).toFixed(DecDigit));
    });
}
function Calculate_DiscAmountItemWise(Amt) {
    var DecDigit = $("#ValDigit").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    var TotalGAmt = parseFloat(0).toFixed(DecDigit);
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        //var currentRow = $(this);
        var currentRow = $(this);
        var ord_qty_spec = CheckNullNumber(currentRow.find("#TxtReceivedQuantity").val());
        var item_rate = CheckNullNumber(currentRow.find("#TxtInvoiceRate").val());
        var GrossVal = parseFloat(ord_qty_spec) * parseFloat(item_rate);

        currentRow.find("#TxtItemGrossValue").val(parseFloat(GrossVal).toFixed(DecDigit));
        currentRow.find("#item_disc_perc").val("");
        currentRow.find("#item_disc_amt").val("");

        var GValue = currentRow.find("#TxtItemGrossValue").val();
        if (GValue != null && GValue != "") {
            if (parseFloat(GValue) > 0) {
                TotalGAmt = parseFloat(TotalGAmt) + parseFloat(GValue);
            }
            else {
                TotalGAmt = parseFloat(TotalGAmt) + parseFloat(0);
            }
        }
    });
    $("#SInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        var GrossValue;
        var ItmGrVal = currentRow.find("#TxtItemGrossValue").val();
        if (CheckNullNumber(ItmGrVal) == 0) {
            GrossValue = parseFloat(0).toFixed();
        }
        else {
            GrossValue = currentRow.find("#TxtItemGrossValue").val();
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(Amt) > 0) {
            debugger;
            var AmtPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100)/*.toFixed(DecDigit)*/;
            var AmtItemWise = ((parseFloat(AmtPercentage) * parseFloat(Amt)) / 100).toFixed(DecDigit);
            if (parseFloat(AmtItemWise) > 0) {
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/
                /* Case :  gross_value = 2000,total_gross_value = 16200, total_diccount=16195, fix_decimal = 2. Then discount_value is 2000.08*/
                if (parseFloat(CheckNullNumber(AmtItemWise)) > parseFloat(CheckNullNumber(GrossValue))) {
                    currentRow.find("#item_disc_val").val(parseFloat(GrossValue).toFixed(DecDigit));
                } else {
                    currentRow.find("#item_disc_val").val(parseFloat(AmtItemWise).toFixed(DecDigit));
                }
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/
                //currentRow.find("#item_disc_val").val(parseFloat(AmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#item_disc_val").val(parseFloat(0).toFixed(DecDigit));
            }
        }
        else {
            currentRow.find("#item_disc_val").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalDiscAmount = parseFloat(0).toFixed(DecDigit);
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var DiscValue = currentRow.find("#item_disc_val").val();
        if (DiscValue != null && DiscValue != "") {
            if (parseFloat(DiscValue) > 0) {
                TotalDiscAmount = parseFloat(TotalDiscAmount) + parseFloat(DiscValue);
            }
            else {
                TotalDiscAmount = parseFloat(TotalDiscAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalDiscAmount) > parseFloat(Amt)) {
        var AmtDifference = parseFloat(TotalDiscAmount) - parseFloat(Amt);
        let adjusted = false;
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#tblsr_no").val();
            var DiscValue = currentRow.find("#item_disc_val").val();
            var GrossValue = currentRow.find("#TxtItemGrossValue").val();
            if (!adjusted) {
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/
                var AmtItemWise = parseFloat(DiscValue) - parseFloat(AmtDifference);
                if (parseFloat(CheckNullNumber(AmtItemWise)) > parseFloat(CheckNullNumber(GrossValue))) {
                    currentRow.find("#item_disc_val").val(parseFloat(GrossValue).toFixed(DecDigit));
                } else {
                    currentRow.find("#item_disc_val").val(parseFloat(AmtItemWise).toFixed(DecDigit));
                    adjusted = true;
                }
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/
                //currentRow.find("#item_disc_val").val((parseFloat(DiscValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalDiscAmount) < parseFloat(Amt)) {
        var AmtDifference = parseFloat(Amt) - parseFloat(TotalDiscAmount);
        let adjusted = false;
        $("#SInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#tblsr_no").val();
            var DiscValue = currentRow.find("#item_disc_val").val();
            var GrossValue = currentRow.find("#TxtItemGrossValue").val();
            if (!adjusted) {
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/
                var AmtItemWise = parseFloat(DiscValue) + parseFloat(AmtDifference);
                if (parseFloat(CheckNullNumber(AmtItemWise)) > parseFloat(CheckNullNumber(GrossValue))) {
                    currentRow.find("#item_disc_val").val(parseFloat(GrossValue).toFixed(DecDigit));
                } else {
                    currentRow.find("#item_disc_val").val(parseFloat(AmtItemWise).toFixed(DecDigit));
                    adjusted = true;
                }
                /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount case------*/

                //currentRow.find("#item_disc_val").val((parseFloat(DiscValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#SInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        /*------Code Added by Suraj Maurya on 15-10-2024 to add discount value per piece-----*/
        var DiscValue = currentRow.find("#item_disc_val").val();
        var Qty = currentRow.find("#TxtReceivedQuantity").val();
        var Rate = currentRow.find("#TxtInvoiceRate").val();
        var DiscAmtPerPec = parseFloat(CheckNullNumber(DiscValue)) / parseFloat(CheckNullNumber(Qty));
        if (parseFloat(CheckNullNumber(DiscAmtPerPec)) > 0) {
            currentRow.find("#item_disc_amt").val(parseFloat(CheckNullNumber(DiscAmtPerPec)).toFixed(DecDigit));
        } else {
            currentRow.find("#item_disc_amt").val("");
        }
        //Formula : (Rate * Quantity) > Discount Value
        if (parseFloat((parseFloat(CheckNullNumber(Rate)) * parseFloat(CheckNullNumber(Qty))).toFixed(DecDigit)) > parseFloat(parseFloat(CheckNullNumber(DiscValue)).toFixed(DecDigit))) {
            currentRow.find("#item_disc_amt").css("border-color", '#ced4da');
            currentRow.find("#item_disc_amtError").text("");
            currentRow.find("#item_disc_amtError").css("display", "none");

            currentRow.find("#itemInvoiceAmtError").css("display", "none");
            currentRow.find("#TxtInvoiceRate").css("border-color", "#ced4da");
            currentRow.find("#itemInvoiceAmtError").text("");
        }
        /*------Code Added by Suraj Maurya on 15-10-2024 to add discount value per piece End-----*/
        CalculateOrderDiscount(currentRow,"NoCalc");
    });
    CalculateAmount();
}

function DisableDiscountAmt() {
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#item_disc_perc").val("");
        currentRow.find("#item_disc_amt").val("");

        currentRow.find("#item_disc_perc").attr("readonly", true);
        currentRow.find("#item_disc_amt").attr("readonly", true);
    });
}
function EnableDiscountAmt() {
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#item_disc_perc").val("");
        currentRow.find("#item_disc_amt").val("");

        currentRow.find("#item_disc_perc").attr("readonly", false);
        currentRow.find("#item_disc_amt").attr("readonly", false);
    });
}
function CalculateOrderDiscount(clickedrow,Flag) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var conv_rate = $("#conv_rate").val();
    var item_gr_val = clickedrow.find("#TxtItemGrossValue").val();
    var item_disc_val = clickedrow.find("#item_disc_val").val();
    var ItemName = clickedrow.find("#hfItemID").val();

    var FinDisVal = parseFloat(item_gr_val) - parseFloat(item_disc_val);

    if (DocumentMenuId == "105103145125") {
        clickedrow.find("#TxtItemGrossValue").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
        FinalFinVal = (FinDisVal * conv_rate).toFixed(ExpImpValDigit);
    }
    else {
        clickedrow.find("#TxtItemGrossValue").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
        FinalFinVal = (FinDisVal * conv_rate).toFixed(ValDecDigit);
    }

    clickedrow.find("#TxtNetValueInBase").val(FinalFinVal);
    if ($("#nontaxable").is(":checked")) {

    }
    else {
        CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val(), Flag);
    }
}
function DiscFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpRateDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
            return false;
        }
    }
    /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount --------*/
    var key = evt.key;
    var number = el.value.split('.');
    //var GrVal = $("#TxtGrossValue").val();
    var GrVal = 0;
    $("#SInvItmDetailsTbl > tbody > tr").each(function () {
        var row = $(this);
        var rate = row.find("#TxtInvoiceRate").val();
        var Qty = row.find("#TxtReceivedQuantity").val();
        GrVal = parseFloat(GrVal) + parseFloat(parseFloat(CheckNullNumber(rate)) * parseFloat(CheckNullNumber(Qty)));
    });
    GrVal = CheckNullNumber(GrVal);

    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        if (parseFloat(valPer) >= parseFloat(GrVal)) {
            if (Cmn_CheckSelectedTextInTextField(evt) == true) {
                return true;
            };
            return false;
        }

    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        if (parseFloat(valPer) >= parseFloat(GrVal)) {
            if (Cmn_CheckSelectedTextInTextField(evt) == true) {
                return true;
            };
            return false;
        }

    }
    /*-------Code Added by Suraj Maurya on 15-10-2024 to avoid over discount End-------*/
    return true;
}

/***-------------------Orderwise Discount End------------------------------------***/
function Fn_AfterTaxChangeUpdateNetValueItemWise(currentRow, total_tax_val) {
    /* Created By : Suraj Maurya, On : 30-11-2024, Purpose : To Update Row Data after tax change */
    if (!currentRow.find("#TaxExempted").is(":checked")) {
        currentRow.find("#Txtitem_tax_amt").val(parseFloat(CheckNullNumber(total_tax_val)).toFixed(cmn_ValDecDigit));
        let ItemGrVal = parseFloat(CheckNullNumber(currentRow.find("#TxtItemGrossValue").val()));
        let ItemOcVal = parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()));
        let ItemNetVal = ItemGrVal + ItemOcVal + parseFloat(CheckNullNumber(total_tax_val));
        currentRow.find("#TxtNetValueInBase").val(ItemNetVal.toFixed(cmn_ValDecDigit));
        currentRow.find("#TxtNetValue").val(ItemNetVal.toFixed(cmn_ValDecDigit));
    }
}

/***------------------------------TDS On Third Party OC add by Hina on 09-07-2024------------------------------***/
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
    debugger

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

    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145125") {
        DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        DecDigit = $("#ValDigit").text();
    }
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var tdsval = currentRow.find("#TDS_val").text();
        TotalAMount = parseFloat(getvalwithoutroundoff((parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())), DecDigit)).toFixed(DecDigit);

    });

    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}
//function OnClickTDS_SaveAndExit() {
//    debugger

//    if ($("#hdn_tds_on").val() == "OC") {
//        OnClickTP_TDS_SaveAndExit();
//        $("#hdn_tds_on").val("");
//    }

//}
/***------------------------------TDS On Third Party End------------------------------***/
/*----------------------------TDS Section-----------------------------*/
function OnClickTCSCalculationBtn() {
    debugger;
    //var ValDigit = $("#ValDigit").text();
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
    let checkResult = ArrTcsDtl.length > 0 ? ArrTcsDtl[0].result : "";
    let tds_amt = 0;
    if (checkResult == "Invalid Slab") {
        swal("", $("#InvailidTdsSlabFound").text(), "warning")
    } else {
        var checkTdsAcc = "Y";
        $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
        for (var i = 0; i < ArrTcsDtl.length; i++) {
            //let td_tds_amt = Math.round(ArrTcsDtl[i].tds_amt);//Commented by Suraj Maurya on 24-03-2025
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
            
        }

        if (checkTdsAcc == "N") {
            $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
            $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
            swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
        }
    }
    $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));
}
function FilterItemDetail(e) {//added by Prakash Kumar on 17-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SInvItmDetailsTbl", [{ "FieldId": "TxtItemName", "FieldType": "input" }]);
}
/*----------------------------TDS Section End-----------------------------*/
/***------------------------------Non-Taxable Start------------------------------***/
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
                currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(FGrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#TxtNetValueInBase").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
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
                if (currentRow.find("#FOC").is(":checked")) {
                    currentRow.find("#TaxExempted").prop("checked", false);
                    currentRow.find("#TaxExempted").attr("disabled", true);
                    currentRow.find("#ManualGST").attr("disabled", true);
                }
                else {
                    currentRow.find("#TaxExempted").prop("checked", false);
                    currentRow.find("#TaxExempted").attr("disabled", false);
                    currentRow.find("#ManualGST").attr("disabled", false);
                }
                currentRow.find("#BtnTxtCalculation").prop("disabled", false);
                i++;
            });
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                var gst_number = $("#Ship_Gst_number").val()
                Cmn_OnSaveAddressApplyGST_Async(gst_number, "SInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "", null).then(() => {
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
                        let AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(ValDecDigit);
                        let NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                        let FinalNetOrderValueBase = /*ConvRate **/ NetOrderValueBase
                        var oc_amt = CheckNullNumber(currentRow.find("#TxtOtherCharge").val());
                        if (oc_amt == ".") {
                            oc_amt = 0;
                        }
                        if (oc_amt != "" && oc_amt != ".") {
                            oc_amt = parseFloat(oc_amt);
                        }
                        currentRow.find("#TxtNetValueInBase").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                        currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(FinalNetOrderValueBase) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                    }
                });
            }
            else {
                var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(ValDecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(CheckNullNumber(currentRow.find("#TxtOtherCharge").val()))).toFixed(ValDecDigit);
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                currentRow.find("#TxtNetValue").val(parseFloat(getvalwithoutroundoff((parseFloat(FGrossAmtOR) / parseFloat(ConvRate)), ValDecDigit)).toFixed(ValDecDigit));
                currentRow.find("#TxtNetValueInBase").val(parseFloat(FGrossAmtOR).toFixed(ValDecDigit));
            }
        });
        CalculateAmount();
        BindTaxAmountDeatils(NewArray);
    }
}
/***------------------------------Non-Taxable End------------------------------***/
//Added by Nidhi on 26-08-2025
function SendEmail() {
var docid = $("#DocumentMenuId").val();
var cust_id = $("#Cust_NameList option:selected").val();
Cmn_SendEmail(docid, cust_id, 'Cust');
}
function SendEmailAlert() {
    var mail_id = $("#Email").val().trim();
    var status = $('#hfSIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var filepath = $('#hdfilepathpdf').val();
    var statusAM = '';
    var GstApplicable = $("#Hdn_GstApplicable").text();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/LocalSaleInvoice/SendEmailAlert", filepath, "", GstApplicable)
}
function ViewEmailAlert() {
    var mail_id = $("#Email").val().trim();
    var status = $('#hfSIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var filepath = $('#hdfilepathpdf').val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'SI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/LocalSaleInvoice/SavePdfDocToSendOnEmailAlert_Ext",
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
    var status = $('#hfSIStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
function PrintFormate() {
    var status = $('#hfSIStatus').val();
    var Doc_no = $("#InvoiceNumber").val();
    var Doc_dt = $("#Inv_date").val();
    var docid = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray();
        var pdfAlertEmailFilePath = 'SI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LocalSaleInvoice/SavePdfDocToSendOnEmailAlert_Ext",
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
        ItemAliasName: $("#ItemAliasName").val(),
        PrintShipFromAddress: $("#PrintShipFromAddress").val(),
    }];
}
//End on 26-08-2025
