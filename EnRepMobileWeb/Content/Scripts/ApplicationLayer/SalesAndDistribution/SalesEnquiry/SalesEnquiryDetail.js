///*CREATED THIS PAGE  BY HINA SHARMA ON 16-01-2025*/
$(document).ready(function () {
    debugger;
    $("#sales_person").select2();
    if ($("#tgl_QuotationCreated").is(":checked")) {
        debugger;
        var condition = $("#tgl_QuotationCreated").is(":checked")
        $("#hdQuationCreated").val(condition);
    }
    $('select[id="SEItemListName"]').bind('change', function (e) {
        //debugger;
        BindSEItemList(e);
    });
    $('input[name="SE_Quantity"]').bind('change', function (e) {
        //debugger;
        CalculationBaseQty(e);
    });
    $('input[name="item_rate"]').bind('change', function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        CalculationBaseRate(clickedrow);
    });

    BindCustomerList();
   // BindSalesPersonList();
   $("#SpanCustNameErrorMsg").css("display", "none");
    $("#Cust_NameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-Cust_NameList-container']").css("border-color", "#ced4da");
    /*---------------HEADER SECTION END--------------- */
    BindDLLSEItemList();
    $('#SlsEnqryItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        debugger;
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
        SerialNoAfterDelete();
        if ($("#SlsEnqryItmDetailsTbl >tbody >tr").length == 0) {
            $("#SlsEnqryItmDetailsTbl .plus_icon1").css("display", "block");
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
    var Enq_No = $("#txt_EnquiryNo").val();
    $("#hdDoc_No").val(Enq_No);
    debugger;
    if (Enq_No != "" && Enq_No != null) {
        AddRemoveCommTyp()
    }
});
/*---------------HEADER SECTION START--------------- */
function BindCustomerList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    var CustPros_type;
    var Enquiry_type;
    var status = $("#hfStatus").val().trim();
    CustPros_type = "C";
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
        $("#Div_ProspectSetup").css("display", "none");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
       // $("#Prosbtn").css("display", "block");
        if (status == "D" || status == "SQR" || status == "A" || status == "C") {
            $("#Div_ProspectSetup").css("display", "none");
        }
        else {
            CheckUserRolePageAccess(CustPros_type, "SEQ");
        }
    }
    if ($("#EnquiryTypeD").is(":checked")) {
        Enquiry_type = "D";
        $("#tbl_hdr_Netvalspecfic").css("display", "none");
    }
    if ($("#EnquiryTypeE").is(":checked")) {
        Enquiry_type = "E";
        $("#tbl_hdr_Netvalspecfic").css("display", "");
    }
    $("#hdn_Enquiry_type").val(Enquiry_type);
    
    $("#hdn_CustPros_type").val(CustPros_type);

    $("#Customer_Name").select2({

        ajax: {
            url: $("#hdnCustNameList").val(),
            data: function (params) {
                debugger;
                var queryParameters = {
                    SE_CustName: params.term, // search term like "a" then "an"
                    //CustPage: params.page,
                    //BrchID: Branch,
                    CustPros_type: CustPros_type,
                    Enquiry_type: Enquiry_type
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
//                        SE_SalePerson: params.term, // search term like "a" then "an"
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
function OnChangeEnquiryType() {
    debugger;
    BindCustomerList();
    ClearHeaderDetails();
}
function AddNewProspect() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    try {
        var ProspectFromEnquiry = "Y";
        window.location.href = "/BusinessLayer/ProspectSetup/AddProspectSetupDetail/?ProspectFromQuot=" + ProspectFromEnquiry + "&QuotationDocumentMenuId=" + DocumentMenuId;

    }
    catch (err) {
        console.log(PFName + " Error : " + err.message);
    }

}
function OnChangeCustProsType() {
    debugger;
    DisableItemDetail();
    ClearHeaderDetails();
    
}
function onclickbtnSaveAndExitItemInfo() {
    debugger;
    var DiscussRemark = $("#Partial_txt_DiscussRemark").text().trim();
    if (DiscussRemark != "" && DiscussRemark != null) {
        $("txt_Discusremarks").text(DiscussRemark);
    }
    else {
        $("txt_Discusremarks").text("");
    }
}
function OnChangeCustomer(CustID) {
    debugger;
    var Cust_id = CustID.value;
    var CustPros_type;
    /*var Cust_type;*/
    var Enquiry_type
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
    }
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
    }
    if ($("#EnquiryTypeD").is(":checked")) {
        Enquiry_type = "D";
    }
    if ($("#EnquiryTypeE").is(":checked")) {
        Enquiry_type = "E";
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
        EnableItemDetails();
        $("#SlsEnqryItmDetailsTbl .plus_icon1").css("display", "block");
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
                            //$("#TxtShippingAddr").val(arr.Table[0].ShippingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            //$("#ship_add_id").val(arr.Table[0].ship_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                            $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                            $("#hdcurr").val(arr.Table[0].curr_id);
                            $("#txtCurrency").val(arr.Table[0].curr_name);
                            //var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            //$("#txtCurrency").html(s);
                            /*code Start by Hina on 16-01-2025 only for this page*/
                            $("#txt_ContactEmailID").val(arr.Table[0].cont_email);
                            $("#txt_ContactNumber").val(arr.Table[0].cont_num);
                            $("#txt_ContactPerson").val(arr.Table[0].cont_person);
                            /*code End by Hina on 16-01-2025 only for this page*/
                            debugger;
                            $("#AvailableCreditLimit").val(parseFloat(arr.Table[0].cre_limit).toFixed($("#ValDigit").text()));
                            
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            $("#hdnconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));

                            /*$("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));*/
                            if (Enquiry_type == "D") {
                                /* $("#conv_rate").prop("disabled", true);*/
                                $("#conv_rate").attr("readonly", true);
                            }
                            else {
                                var Custtype = $("#hdn_CustPros_type").val();
                                if (Custtype == "P") {
                                    $("#conv_rate").attr("readonly", true);
                                }
                                else {
                                    $("#conv_rate").attr("readonly", false);
                                }

                            }
                            debugger;
                            $("#divAddNewCommuniDtl").css("display", "block");
                            //$("#SpanCustPricePolicy").text(arr.Table[0].cust_pr_pol);
                            //$("#SpanCustPriceGroup").text(arr.Table[0].cust_pr_grp);

                        }
                        else {
                            debugger;
                            $("#TxtBillingAddr").val("");
                           // $("#TxtShippingAddr").val("");
                            $("#bill_add_id").val("");
                            //$("#ship_add_id").val("");
                            $("#ship_add_gstNo").val("");
                            $("#txtCurrency").val("");
                            $("#conv_rate").val("");
                            $("#txt_ContactPerson").val("");
                            $("#txt_ContactEmailID").val("");
                            $("#txt_ContactNumber").val("");
                            $("#txt_ContactWebsite").val("");
                            $("#divAddNewCommuniDtl").css("display", "block");
                            //$("#SpanCustPricePolicy").val("");
                            //$("#SpanCustPriceGroup").val("");
                            
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
    $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        currentRow.find("#SEItemListName" + Sno).attr("disabled", true);
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
        
        currentRow.find("#SE_Quantity").attr("disabled", true);
        currentRow.find("#item_rate").attr("disabled", true);
        
        currentRow.find("#item_remarks").attr("disabled", true);
        currentRow.find("#SEItemListNameError").css("display", "none");
        currentRow.find("#SEItemListName" + Sno).css("border-color", "#ced4da");
        currentRow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
        currentRow.find("#SE_qty_Error").css("display", "none");
        currentRow.find("#SE_Quantity").css("border-color", "#ced4da");
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
        //currentRow.find("#item_ass_valError").css("display", "none");
        //currentRow.find("#item_ass_val").css("border-color", "#ced4da"); item_net_val_bs


    })
    $("#SlsEnqryItmDetailsTbl .plus_icon1").css("display", "none"); 
}
function EnableItemDetails() {
    $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        currentRow.find("#SEItemListName" + Sno).attr("disabled", false);
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
        
        currentRow.find("#SE_Quantity").attr("disabled", false);
        currentRow.find("#item_rate").attr("disabled", false);
        
        currentRow.find("#item_remarks").attr("disabled", false);
        
        currentRow.find("#SEItemListNameError").css("display", "none");
        currentRow.find("#SEItemListName" + Sno).css("border-color", "#ced4da");
        currentRow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
        currentRow.find("#SE_qty_Error").css("display", "none");
        currentRow.find("#SE_Quantity").css("border-color", "#ced4da");
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
        

    })
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
    var status = "";
    var QTDTransType = "";
    status = $("#hfStatus").val().trim();
    //var Enquiry_no = $("#txt_EnquiryNo").val();
    //if ((Enquiry_no != "" || Enquiry_no != null) && ) {
    //    QTDTransType = "Update";
    //}
    //else {
    //    QTDTransType = "Save";
    //}
    var QTDTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        QTDTransType = "Update";
    }
    var Enquiry_no = $("#txt_EnquiryNo").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, QTDTransType, Enquiry_no);
    
}
function OnChangeEnquirySource() {
    debugger;
    var EnquirySource = $('#ddl_EnquirySource').val();
    if (EnquirySource != "0") {
        document.getElementById("vmEnquirySource").innerHTML = null;
        $("#ddl_EnquirySource").css("border-color", "#ced4da");
        var EnqrySrc = $("#ddl_EnquirySource").val()
        $("#hdnEnquirySource").val(EnqrySrc);
        $("#ddl_EnquirySource" + " option:selected").val(EnqrySrc);
    }
    else {
        document.getElementById("vmEnquirySource").innerHTML = $("#valueReq").text();
        $("#ddl_EnquirySource").css("border-color", "red");
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
    $("#SlsEnqryItmDetailsTbl > tbody > tr").each(function () {
        var clickedrow = $(this);
        CalculationBaseRate(clickedrow);
    });

    

}
function OnChangeSalesPerson() {
    debugger;
    var SaleParson = $("#sales_person").val();
    if (SaleParson == "0" || SaleParson == "" || SaleParson == null) {
        $("#sales_person").css("border-color", "red");
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $('[aria-labelledby="select2-sales_person-container"]').css("border-color", "Red");
        $("#SpanSalesPersonErrorMsg").css("display", "block");
        //$('#sales_person').val("0");
        //$('#sales_person').text("---Select---");
    }
    else {
        $("#sales_person").css("border-color", "#ced4da");
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $('[aria-labelledby="select2-sales_person-container"]').css("border-color", "#ced4da");
    }
}

function ClearHeaderDetails() {
    debugger;
    $('#ddl_EnquirySource').val("0");
    $('#Customer_Name').empty().append('<option value="0" selected="selected">---Select---</option>');
    $('#SpanCustNameErrorMsg').text("");
    $("#TxtBillingAddr").val("");
    $("#txt_ContactPerson").val("");
    $("#txt_ContactEmailID").val("");
    $("#txt_ContactNumber").val(""); 
    $("#txt_ContactWebsite").val("");
    //$("#TxtShippingAddr").val("");
    $("#txtCurrency").val("");
    $("#conv_rate").val("");
    $('#sales_person').empty().append('<option value="0" selected="selected">---Select---</option>');
    $("#txt_remarks").val("");
}
function OnkeyPressConNumber(event) {
    debugger;
    var charCode = event.which || event.keyCode;
    var char = String.fromCharCode(charCode);

    // Allow only numeric values (0-9)
    if (!/\d/.test(char)) {
        event.preventDefault(); // Block non-numeric input
    }
};
function OnchangeCustomerNUm() {
    debugger;
    var Hdn_GstApplicable = $("#Hdn_GstApplicable").text();
    var cont_num1 = $("#cont_num1").val().length
    if (cont_num1 != null && cont_num1 != "") {
        if (Hdn_GstApplicable == "Y") {
            if (cont_num1 < 10) {
                document.getElementById("vmcont_num1").innerHTML = ($("#span_InvalidFormat").text() + " (9999999999)");
                $("#cont_num1").css('border-color', 'red');
                $("#vmcont_num1").attr("style", "display: block;");
                $("#cont_num1").css("display", "inherit");
                return false;
            }
            else {
                $('#vmcont_num1').text("");
                $("#vmcont_num1").css("display", "None");
                $("#cont_num1").css('border-color', "#ced4da");
            }
        }
    }
    else {
        $('#vmcont_num1').text("");
        $("#vmcont_num1").css("display", "None");
        $("#cont_num1").css('border-color', "#ced4da");
    }
}

function lettersOnly(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
    if (charCode > 32 && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122) ) {
        return false;
    }
    //evt.keyCode == 32
    return true;
}
function ValidateEmail() {
    debugger;
    /*var validFlag = "N";*/
    var Email = $("#txt_ContactEmailID").val();

    var mailformat = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    //mailformat.test(Email);
    if (Email != "") {
        if (mailformat.test(Email)) {
            document.getElementById("vmEmail").innerHTML = "";
            $("#txt_ContactEmailID").css("border-color", "#ced4da");
            return true;
        }
        else {
            document.getElementById("vmEmail").innerHTML = $("#InvalidEmail").text();
            $("#txt_ContactEmailID").css("border-color", "red");
            return false;
        }
    }
    else {
        document.getElementById("vmEmail").innerHTML = "";
        $("#txt_ContactEmailID").css("border-color", "#ced4da");
    }

}
function CheckedQuotCret() {
    debugger;
    if ($("#tgl_QuotationCreated").is(":checked")) {
        debugger;
        var condition = $("#tgl_QuotationCreated").is(":checked")
        $("#hdQuationCreated").val(condition);
        $("#hdQuationCreated").val("Y");
        //$("#ddl_EnquirySource").attr('disabled', true);
        //DisableItemDetail();
        

    }
    else {
        $("#hdQuationCreated").val("N");
        //$("#ddl_EnquirySource").attr('disabled', false);
        //EnableItemDetails()
        $("#SlsEnqryItmDetailsTbl .plus_icon1").css("display", "block");
        
    }
}
/*---------------HEADER SECTION END--------------- */

/*---------------ITEM SECTION START--------------- */
function AddNewRow() {
    debugger;
    var rowIdx = 0;
    var ItemInfo = $("#ItmInfo").text();
    var TaxInfo = $("#TaxInfo").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    //var Span_BuyerInformation_Title = $("#Span_BuyerInformation_Title").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    //debugger;
    /* var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#SlsEnqryItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function (i, row) {
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
    var EnqTyp = $("#hdn_Enquiry_type").val();
    var displaynoneinDomestic = EnqTyp == "D" ? "style='display: none;'" : "";
    $('#SlsEnqryItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class="red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="SRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td>
<div class="col-sm-11 lpo_form no-padding">
<select class="form-control" id="SEItemListName${RowNo}" name="SlsenqryItemListName" onchange ="OnChangeSEItemName(${RowNo},event)"></select>
<span id="SEItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${ItemInfo}"></button>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
<input id="UOMID" type="hidden" />
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="SE_Quantity" class="form-control date num_right" autocomplete="off" onchange ="OnChangeSEItemQty(event)" onpaste="return CopyPasteData(event);" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="SEQty_name"  placeholder="0000.00">
<span id="SE_qty_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemEnqQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemEnqQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="lpo_form">
<input id="item_rate" class="form-control date num_right"  onchange ="OnChangeSEItemRate(event)" autocomplete="off" onpaste="return CopyPasteData(event);" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate_name"  placeholder="0000.00">
<span id="item_rateError" class="error-message is-visible"></span>
</div>
</td>
<td>
<input id="item_net_val_bs" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs_name"  placeholder="0000.00"  disabled>
</td>
<td ${displaynoneinDomestic}>
<input id="item_net_val_spec" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec_name"  placeholder="0000.00"  disabled>
</td>
<td>
<textarea id="item_remarks" class="form-control remarksmessage" name="remarks_name" onmouseover="OnMouseOver(this)" maxlength = "200",  placeholder="${$("#span_remarks").text()}"></textarea>
</td>
</tr>`);
    BindSEItmList(RowNo);
};
function BindSEItmList(ID) {
    debugger;
    var ItmDDLName = "#SEItemListName";
    var TableID = "#SlsEnqryItmDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "SO")
}
function BindDLLSEItemList() {
    debugger;
    BindItemList("#SEItemListName", "1", "#SlsEnqryItmDetailsTbl", "#SNohiddenfiled", "", "SQ");
    
}
function OnChangeSEItemName(RowID, e) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
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
    BindSEItemList(e);
}
function BindSEItemList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    Itm_ID = clickedrow.find("#SEItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {

        clickedrow.find("#SEItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SEItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + SNo + "-container']").css("border", "1px solid red");

    }
    else {

        clickedrow.find("#SEItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + SNo + "-container']").css("border", "1px solid #aaa");

    }
    ClearRowDetails(e, ItemID);
    DisableHeaderField();
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "sale")
    

}
function DisableHeaderField() {
    debugger;
    $("#EnquiryTypeD").attr('disabled', true);
    $("#EnquiryTypeE").attr('disabled', true);
    $("#CustomerTypeC").attr('disabled', true);
    $("#CustomerTypeP").attr('disabled', true);
    $("#Customer_Name").attr('disabled', true);
    //$("#sales_person").attr('disabled', true);
    //$("#dremarks").attr('disabled', true);
    $("#Div_ProspectSetup").css("display", "none");
}
function ClearRowDetails(e, ItemID) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    clickedrow.find("#UOM").val("");
    clickedrow.find("#SE_Quantity").val("");
    clickedrow.find("#item_rate").val("");
    clickedrow.find("#item_net_val_bs").val("");
    clickedrow.find("#item_remarks").val("");
}
function OnChangeSEItemQty(e) {
    //debugger;
    CalculationBaseQty(e);
}
function OnChangeSEItemRate(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculationBaseRate(clickedrow);
}
function CalculationBaseQty(e) {
    debugger;
    //var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145105") {
    //    var ValDecDigit = $("#ExpImpValDigit").text();
    //    var QtyDecDigit = $("#ExpImpQtyDigit").text();
    //}
    //else {
        var ValDecDigit = $("#ValDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
   /* }*/

    var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var EnqryQty;
    var ItemName;
    var ItmRate;
    

    EnqryQty = clickedrow.find("#SE_Quantity").val();
    ItemName = clickedrow.find("#SEItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    

    if (ItemName == "0") {
        clickedrow.find("#SEItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SEItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#SE_Quantity").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SEItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (EnqryQty != "" && EnqryQty != ".") {
        EnqryQty = parseFloat(EnqryQty);
    }
    if (EnqryQty == ".") {
        EnqryQty = 0;
    }
    if (EnqryQty == "" || EnqryQty == 0) {
        clickedrow.find("#SE_qty_Error").text($("#valueReq").text());
        clickedrow.find("#SE_qty_Error").css("display", "block");
        clickedrow.find("#SE_Quantity").css("border-color", "red");
        clickedrow.find("#SE_Quantity").val("");
        errorFlag = "Y";
    }
    else {
        //clickedrow.find("#Qt_qty_Error").text("");
        clickedrow.find("#SE_qty_Error").css("display", "none");
        clickedrow.find("#SE_Quantity").css("border-color", "#ced4da");

    }

    EnqryQty = clickedrow.find("#SE_Quantity").val();
    if (AvoidDot(EnqryQty) == false) {
        clickedrow.find("#SE_Quantity").val("");
        EnqryQty = 0;
    }
    else {
        clickedrow.find("#SE_Quantity").val(parseFloat(EnqryQty).toFixed(QtyDecDigit));
    }
    if ((EnqryQty !== 0 || EnqryQty !== null || EnqryQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = EnqryQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        debugger;
        //clickedrow.find("#item_net_val_bs").val(FinVal);
        
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#item_net_val_bs").val(FinalVal);
        
    }
    
    
    
}
function CalculationBaseRate(clickedrow) {
    debugger;
    //var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145105") {
    //    var ValDecDigit = $("#ExpImpValDigit").text();
    //    var RateDecDigit = $("#ExpImpRateDigit").text();
    //}
    //else {
        var ValDecDigit = $("#ValDigit").text();
        var RateDecDigit = $("#RateDigit").text();
   /* }*/

    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var errorFlag = "N";
    var EnqryQty;
    var ItemName;
    var ItmRate;
    

    EnqryQty = clickedrow.find("#SE_Quantity").val();
    ItemName = clickedrow.find("#SEItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    

    if (ItemName == "0") {
        clickedrow.find("#SEItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SEItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#SE_Quantity").val("");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SEItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    EnqryQty = clickedrow.find("#SE_Quantity").val();
    if (EnqryQty == "" || EnqryQty == "0") {
        clickedrow.find("#SE_qty_Error").text($("#valueReq").text());
        clickedrow.find("#SE_qty_Error").css("display", "block");
        clickedrow.find("#SE_Quantity").css("border-color", "red");
        clickedrow.find("#SE_Quantity").val("");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SE_qty_Error").css("display", "none");
        clickedrow.find("#SE_qty_Error").css("border-color", "#ced4da");
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

    if ((EnqryQty !== 0 || EnqryQty !== null || EnqryQty !== "") && (ItmRate !== "0" || ItmRate !== null || ItmRate !== "")) {
        var FAmt = EnqryQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        //clickedrow.find("#item_net_val_bs").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        clickedrow.find("#item_net_val_bs").val(FinalVal);
        
    }
    
    
    
}
function SerialNoAfterDelete() {
    debugger
    var SerialNo = 0;
    $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SRNO").text(SerialNo);


    });
};
function RateFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145105") {
    //    if (Cmn_FloatValueonly(el, evt, "#ExpImpRateDigit") == false) {
    //        return false;
    //    }
    //}
    //else {
        if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
            return false;
        /*}*/
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");
    return true;
}
function QtyFloatValueonly(el, evt) {
    
    //debugger;
    //var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145105") {
    //    if (Cmn_FloatValueonly(el, evt, "#ExpImpQtyDigit") == false) {
    //        return false;
    //    }
    //}
    //else {
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    /*}*/

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    
    clickedrow.find("#SE_qty_Error" + RowNo).css("display", "none");
    clickedrow.find("#SE_Quantity" + RowNo).css("border-color", "#ced4da");

    return true;
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#SEItemListName" + Sno).val();
    ItmName = clickedrow.find("#SEItemListName" + Sno + " option:selected").text()
    ItemInfoBtnClick(ItmCode);
}

function CheckSEHeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    var RequisitionTypeList = $('#ddl_EnquirySource').val();
    if (RequisitionTypeList != "0") {
        document.getElementById("vmEnquirySource").innerHTML = null;
        $("#ddl_EnquirySource").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmEnquirySource").innerHTML = $("#valueReq").text();
        $("#ddl_EnquirySource").css("border-color", "red");
        ErrorFlag = "Y";
    }
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
        //$("#Div_ProspectSetup").css("display", "none");
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
function CheckSEItemValidations() {
    debugger;
    //var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145105") {
    //    var RateDecDigit = $("#ExpImpRateDigit").text();
    //    var QtyDecDigit = $("#ExpImpQtyDigit").text();
    //}
    //else {
        var RateDecDigit = $("#RateDigit").text();
        var QtyDecDigit = $("#QtyDigit").text();
    /*}*/

    var ErrorFlag = "N";
    if ($("#SlsEnqryItmDetailsTbl >tbody >tr").length > 0) {
        $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#SEItemListName" + Sno).val() == "0") {
                currentRow.find("#SEItemListNameError").text($("#valueReq").text());
                currentRow.find("#SEItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SEItemListNameError").css("display", "none");
                currentRow.find("[aria-labelledby='select2-SEItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#SE_Quantity").val() == "") {
                currentRow.find("#SE_qty_Error").text($("#valueReq").text());
                currentRow.find("#SE_qty_Error").css("display", "block");
                currentRow.find("#SE_Quantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SE_qty_Error").css("display", "none");
                currentRow.find("#SE_qty_Error").css("border-color", "#ced4da");
            }
            if (currentRow.find("#SE_Quantity").val() != "") {
                if (parseFloat(currentRow.find("#SE_Quantity").val()).toFixed(QtyDecDigit) == 0) {
                    currentRow.find("#SE_qty_Error").text($("#valueReq").text());
                    currentRow.find("#SE_qty_Error").css("display", "block");
                    currentRow.find("#SE_Quantity").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#SE_qty_Error").css("display", "none");
                    currentRow.find("#SE_qty_Error").css("border-color", "#ced4da");
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
function InsertSalesEnquiryDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        //$("#btn_save").attr("disabled", true);
        //$("#btn_save").css("filter", "grayscale(100%)");
        //return false;
    }
    var EnquiryNo = $("#txt_EnquiryNo").val();
    var docid = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#hdnSQGstApplicable").val(GstApplicable)
    if (CheckSEHeaderValidations() == false) {
        return false;
    }
    if (CheckSEItemValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    
    if ($("#SlsEnqryItmDetailsTbl >tbody >tr").length == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    debugger;
    //var TransType = "";
    //if (QTDTransType === 'Update') {
    //    TransType = 'Update';
    //}
    //else {
    //    TransType = 'Save';
    //}
    if (EnquiryNo == null || EnquiryNo == "") {
        $("#hdn_TransType").val("Save");
        
    }
    else {
        $("#hdn_TransType").val("Update");
        
    }
    var FinalSE_ItemDetail = [];
    var FinalCommuniDetail = [];
    debugger;
    FinalSE_ItemDetail = InsertSEItemDetails();
    FinalCommuniDetail = InsertCommunicatnDetails();
    debugger;
    $('#hdItemDetailList').val(JSON.stringify(FinalSE_ItemDetail));
    
    $('#hdCommunicatnDetailList').val(JSON.stringify(FinalCommuniDetail));
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
        //$("#Div_ProspectSetup").css("display", "none");
    }
    else {
        cust_type = "P";

    }
    $("#hdn_CustPros_type").val(cust_type);
    if ($("#EnquiryTypeD").is(":checked")) {
        Enquiry_type = "D";
    }
    if ($("#EnquiryTypeE").is(":checked")) {
        Enquiry_type = "E";
    }
    $("#hdn_Enquiry_type").val(Enquiry_type);
    debugger;
    var EnqrySrc = $("#ddl_EnquirySource").val()
    $("#hdnEnquirySource").val(EnqrySrc);
    $("#ddl_EnquirySource" + " option:selected").val(EnqrySrc);
    //$("#hdnsavebtn").val("AllreadyclickSaveBtn");
    resetCommunicationHeaderdetail();
    
    debugger;
    if ($("#tgl_QuotationCreated").is(":checked")) {
        debugger;
        var condition = $("#tgl_QuotationCreated").is(":checked")
        $("#hdQuationCreated").val(condition);
        if (CheckSEHeaderValidations() == false) {
            return false;
        }
        else if (CheckSEItemValidations() == false) {
            return false;
        }
        else if (CheckValidations_forSubItems() == false) {
            return false;
        }
        else if ($("#SlsEnqryItmDetailsTbl >tbody >tr").length == 0) {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }

        else {
            return true;
        }
    }
    else {
        return true;
    }
    

};
function InsertSEItemDetails() {
    debugger;
    //var QTNo = sessionStorage.getItem("DSQ_No");
    var SENo = $("#txt_EnquiryNo").val();
    var SEDate = $("#txt_EnquiryDate").val();
    var Branch = sessionStorage.getItem("BranchID");
    Enquiry_type= $("#hdn_Enquiry_type").val();
    var SEItemList = [];
    $("#SlsEnqryItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var ItemID = "";
        var UOMID = "";
        var SEQty = "";
        var ItmRate = "";
        var NetValBase = "";
        var NetValSpec = "";
        var Remarks = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();
        var Sr_No = currentRow.find("#SRNO").text();
        ItemID = currentRow.find("#SEItemListName" + SNo).val() + '_' + Sr_No;
        UOMID = currentRow.find("#UOMID").val();
        SEQty = currentRow.find("#SE_Quantity").val();
        ItmRate = currentRow.find("#item_rate").val();
        NetValBase = currentRow.find("#item_net_val_bs").val();
       /* if (Enquiry_type == "E") {*/
            NetValSpec = currentRow.find("#item_net_val_spec").val();
        //}
        //else {
        //    NetValSpec = "";
        //}
        Remarks = currentRow.find("#item_remarks").val();
        
        SEItemList.push({ SENo: SENo, SEDate: SEDate, ItemID: ItemID, UOMID: UOMID, SEQty: SEQty, ItmRate: ItmRate, NetValBase: NetValBase, NetValSpec: NetValSpec,Remarks: Remarks});
    });
    return SEItemList;
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
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemEnqQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var DocumentMenuId = $("#DocumentMenuId").val();
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#SEItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#SEItemListName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#txt_EnquiryNo").val();
    var Doc_dt = $("#txt_EnquiryDate").val();
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
        Sub_Quantity = clickdRow.find("#SE_Quantity").val();
    }
    
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    
    

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesEnquiry/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
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
    return Cmn_CheckValidations_forSubItems("SlsEnqryItmDetailsTbl", "SNohiddenfiled", "SEItemListName", "SE_Quantity", "SubItemEnqQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("SlsEnqryItmDetailsTbl", "SNohiddenfiled", "SEItemListName", "SE_Quantity", "SubItemEnqQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

/*---------------ITEM SECTION END--------------- */
/*---------------COMMUNICATION SECTION START--------------- */
function OnChangeCommunicatnTyp() {
    debugger;
    var CommTyp = $('#ddl_CommunicatnTyp').val();
    if (CommTyp != "0") {
        document.getElementById("vmCommunicatnTyp").innerHTML = null;
        $("#ddl_CommunicatnTyp").css("border-color", "#ced4da");
        $("#hdnCommunicatnTyp").val(CommTyp);
    }
    else {
        document.getElementById("vmCommunicatnTyp").innerHTML = $("#valueReq").text();
        $("#ddl_CommunicatnTyp").css("border-color", "red");
    }
    var Enq_No = $("#txt_EnquiryNo").val();
    $("#hdDoc_No").val(Enq_No);
    debugger;
    if (Enq_No != "" && Enq_No != null) {
        AddRemoveCommTyp()
    }
}
function CheckCommunictnDtlValidation() {
    //debugger;
    var Errorflag = "N";
    var CommTyp = $('#ddl_CommunicatnTyp').val();
    if (CommTyp != "0") {
        document.getElementById("vmCommunicatnTyp").innerHTML = null;
        $("#ddl_CommunicatnTyp").css("border-color", "#ced4da");
        //var EnqrySrc = $("#ddl_CommunicatnTyp").val()
        $("#hdnCommunicatnTyp").val(CommTyp);
        //$("#ddl_CommunicatnTyp" + " option:selected").val(CommTyp);
    }
    else {
        document.getElementById("vmCommunicatnTyp").innerHTML = $("#valueReq").text();
        $("#ddl_CommunicatnTyp").css("border-color", "red");
        Errorflag = "Y";
    }
    var CallDate = $('#txt_CallDate').val();
    if ($('#txt_CallDate').val() == "")
    {
        $('#txt_CallDate').css("border-color", "red");
        $('#SpanCallDate').text($("#valueReq").text());
        $("#SpanCallDate").css("display", "block");
        Errorflag = "Y";
    }
    else
    {
        $('#txt_CallDate').css("border-color", "#ced4da");
        $("#SpanCallDate").css("display", "none");
    }
    var ContactedBy = $('#txt_ContactedBy').val();
    if (ContactedBy == "") {
        $('#txt_ContactedBy').css("border-color", "red");
        $('#SpanContactedBy').text($("#valueReq").text());
        $("#SpanContactedBy").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactedBy').css("border-color", "#ced4da");
        $("#SpanCallDate").css("display", "none");
    }
    var ContactedTo = $('#txt_ContactedTo').val();
    if (ContactedTo == "") {
        $('#txt_ContactedTo').css("border-color", "red");
        $('#SpanContactedTo').text($("#valueReq").text());
        $("#SpanContactedTo").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactedTo').css("border-color", "#ced4da");
        $("#SpanContactedTo").css("display", "none");
    }
    var ContactDetail = $('#txt_ContactDetail').val();
    if (ContactDetail == "") {
        $('#txt_ContactDetail').css("border-color", "red");
        $('#SpanContactDetail').text($("#valueReq").text());
        $("#SpanContactDetail").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactDetail').css("border-color", "#ced4da");
        $("#SpanContactDetail").css("display", "none");
    }
    var Discusremarks = $('#txt_Discusremarks').val();
    if (Discusremarks == "") {
        $('#txt_Discusremarks').css("border-color", "red");
        $('#SpanDiscussRemarks').text($("#valueReq").text());
        $("#SpanDiscussRemarks").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_Discusremarks').css("border-color", "#ced4da");
        $("#SpanDiscussRemarks").css("display", "none");
    }
    if (Errorflag == "Y") {
        return false
    }
    else {
        return true
    }

}
function OnChangeCallDate(CallDate) {
    debugger;
    var Call_Date = CallDate.value;
    var CallDt = $('#txt_CallDate').val();
    if (CallDt == "") {
        $('#txt_CallDate').css("border-color", "red");
        $('#SpanCallDate').text($("#valueReq").text());
        $("#SpanCallDate").css("display", "block");
    }
    else {
        $('#txt_CallDate').css("border-color", "#ced4da");
        $("#SpanCallDate").css("display", "none");
    }
}
function OnChangeContactedBy() {
    debugger;
    var ContactedBy = $('#txt_ContactedBy').val();
    if (ContactedBy == "") {
        $('#txt_ContactedBy').css("border-color", "red");
        $('#SpanContactedBy').text($("#valueReq").text());
        $("#SpanContactedBy").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactedBy').css("border-color", "#ced4da");
        $("#SpanContactedBy").css("display", "none");
    }
}
function OnChangeContactedTo() {
    debugger;
    var ContactedTo = $('#txt_ContactedTo').val();
    if (ContactedTo == "") {
        $('#txt_ContactedTo').css("border-color", "red");
        $('#SpanContactedTo').text($("#valueReq").text());
        $("#SpanContactedTo").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactedTo').css("border-color", "#ced4da");
        $("#SpanContactedTo").css("display", "none");
    }
}
function OnChangeContactDetail() {
    debugger;
    var ContactDetail = $('#txt_ContactDetail').val();
    if (ContactDetail == "") {
        $('#txt_ContactDetail').css("border-color", "red");
        $('#SpanContactDetail').text($("#valueReq").text());
        $("#SpanContactDetail").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_ContactDetail').css("border-color", "#ced4da");
        $("#SpanContactDetail").css("display", "none");
    }
}
function OnChangeDiscusremarks() {
    debugger;
    var Discusremarks = $('#txt_Discusremarks').val();
    if (Discusremarks == "") {
        $('#txt_Discusremarks').css("border-color", "red");
        $('#SpanDiscussRemarks').text($("#valueReq").text());
        $("#SpanDiscussRemarks").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#Partial_txt_DiscussRemark').text(Discusremarks);
        $('#txt_Discusremarks').css("border-color", "#ced4da");
        $("#SpanDiscussRemarks").css("display", "none");
    }
}
//function Partial_OnChangeDiscusremarks(event ) {
//    debugger;
//    //event.preventDefault();
//    var pos = $('#Partial_txt_DiscussRemark').val().trim();
//    $('#Partial_txt_DiscussRemark').val(pos)
//    //document.getElementById("Partial_txt_DiscussRemark").autofocus = true;
//    //$('#Partial_txt_DiscussRemark').autofocus = true;
//    $('#txt_Discusremarks').val(pos);
//}
function Partial_OnChangeDiscusremarks() {
    var pos = $("#Partial_txt_DiscussRemark").val();

    $("#Partial_txt_DiscussRemark").val(pos);
    $('#txt_Discusremarks').val(pos);
}

function PlusBtnAddCommunicatnDetail() {
    debugger;
    
    var RMflag = 'N';
    var CommTypVal = $("#ddl_CommunicatnTyp").val();
    var CommTypName = $("#ddl_CommunicatnTyp option:selected").text();
    var CallDate = $("#txt_CallDate").val();
    
    var ContactedBy = $("#txt_ContactedBy").val();
    var ContactedTo = $("#txt_ContactedTo").val();
    var ContactDetail = $("#txt_ContactDetail").val();
    var Discusremarks = $("#txt_Discusremarks").val(); 

    
    var ValidationFlag = CheckCommunictnDtlValidation();
    if (ValidationFlag == false) {
        return false;
    }

    if (ValidationFlag == true) {
        var RowId = 0;
        var rowCount = $('#tbl_CommunicationDetails >tbody >tr').length + 1;
        //if (rowCount > 0) {
        //    $("#ddl_ItemName").prop("disabled", true);
        //    $("#ddl_WarehouseName").prop("disabled", true);
        //}
        debugger;
        var RowNo = 0;
        var levels = [];
        $("#tbl_CommunicationDetails >tbody >tr").each(function (i, row) {
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
        $('#tbl_CommunicationDetails tbody').append(`

            <tr id="${++RowId}">
        
            <td class=" red center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event,'${CommTypVal}')" title="${$("#Span_Delete_Title").text()}"></i>
            </td>
            <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editRow(this, event,'${CommTypVal}')" title="${$("#Edit").text()}"></i>
            </td>
            <td id="SRNO" class="sr_padding">
            <span id="SpanRowId"  value="${rowCount}">${rowCount}</span>
            <input  type="hidden" id="SNohiddenfiled" value="${RowNo}"/>
            </td>
            
            <td>
                <input id="tbl_CommuTypeName" value="${CommTypName}" class="form-control" type="text" name="commType" placeholder="${$("#span_SampleType").text()}" readonly>
                <input type="hidden" id="tbl_hdCommuTypVal" value="${CommTypVal}" style="display: none;" />
            </td>
            <td>
            <input id="tbl_CallDate" value="${CallDate}" class="form-control" autocomplete="off" type="date" name="calldt" placeholder="${$("#span_QuotationDate").text()}"  onblur="this.placeholder='Call Date'" readonly>
             </td>
            <td>
                <input  id="tbl_ContactBy" value="${ContactedBy}" class="form-control" maxlength="100" autocomplete="off" type="text" name="contby" placeholder="${$("#span_ContactedBy").text()}" readonly>
            </td>
            <td>
             <input id="tbl_ContactTo" value='${ContactedTo}' class="form-control" maxlength="100" autocomplete="off" type="text" name="contto" placeholder="${$("#span_ContactedTo").text()}" readonly>
            </td>
            <td>
             <input id="tbl_ContactDtl" value='${ContactDetail}' class="form-control" maxlength="250" autocomplete="off" type="text" name="contDtl" placeholder="${$("#span_ContactDetail").text()}" readonly>
            </td>
            <td>
             <textarea class="form-control remarksmessage" cols="20" id="tbl_DiscusRemark" maxlength="1000" name="DiscussionRemarks" placeholder="${$("#span_DiscussionRemarks").text()}" readonly="readonly" rows="2" onmouseover="OnMouseOver(this)">${Discusremarks}</textarea>
            </td>

            </tr>

        `);
        //<input id="tbl_DiscusRemark" value='${Discusremarks}' class="form-control" autocomplete="off" name="DiscRmk" placeholder="${$(" #span_DiscussionRemarks").text()}" readonly >
        $('#Partial_txt_DiscussRemark').val("");
        if (RMflag == 'N') {
           resetCommunicationHeaderdetail()
            AddRemoveCommTyp()
        }
    }

}
function AddRemoveCommTyp() {
    debugger;
   
    $("#tbl_CommunicationDetails >tbody >tr").each(function (i, rows) {
        var currentRowChild = $(this);
        //Hide item name, if exist in table
        debugger;
        var tbl_commutypval = currentRowChild.find("#tbl_hdCommuTypVal").val();//recheck
        $("#ddl_CommunicatnTyp option[value=" + tbl_commutypval + "]").hide();
        //$("#ddl_CommunicatnTyp").val("0");
        
    });

}
function checkRM(data, selected, TableNameID, ItemDDlNameID) {
    debugger;
    var mtype = $("#ddlMaterialType").val();
    var ddl_HedrItemId = $("#ddl_ItemName option:selected").val();
    if (mtype != '0') {
        var Flag = "N";
        if (data.id != null) {
            if ($(TableNameID + " tbody tr").length > 0) {
                /*$("#ddlMaterialName option[value=" + ddl_HedrItemId + "]").select2().hide();*/
                var checkItemlen = $(TableNameID + " tbody tr " + ItemDDlNameID + "[value='" + data.id + "']").length;
                if (checkItemlen > 0) {
                    Flag = "Y";
                }
            }

        }
        if (Flag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }

}
function editRow(el, e, option) {
    debugger;
    $("#ddl_CommunicatnTyp").removeAttr('onchange');
    var rowJavascript = el.parentNode.parentNode;
    var clickedrow = $(e.target).closest("tr");
    $('#hdnUpdateInTableCommuniTbl').val(rowJavascript.rowIndex);

    var tbl_CommuTypeName = clickedrow.find("#tbl_CommuTypeName").val();
    var tbl_hdCommuTypVal = clickedrow.find("#tbl_hdCommuTypVal").val();
    
    $("#ddl_CommunicatnTyp").val(tbl_hdCommuTypVal);
    $("#ddl_CommunicatnTyp option:selected").text(tbl_CommuTypeName);
    $("#ddl_CommunicatnTyp").prop("disabled", true);

    var tbl_CallDate = clickedrow.find("#tbl_CallDate").val();
    $("#txt_CallDate").val(tbl_CallDate);
   

    var tbl_ContactBy = clickedrow.find("#tbl_ContactBy").val();
    $("#txt_ContactedBy").val(tbl_ContactBy);

    var tbl_ContactTo = clickedrow.find("#tbl_ContactTo").val();
    $("#txt_ContactedTo").val(tbl_ContactTo);

    var tbl_ContactDtl = clickedrow.find("#tbl_ContactDtl").val();
    $("#txt_ContactDetail").val(tbl_ContactDtl);

    var tbl_DiscusRemark = clickedrow.find("#tbl_DiscusRemark").val(); 
    $("#txt_Discusremarks").val(tbl_DiscusRemark);
    $('#Partial_txt_DiscussRemark').val(tbl_DiscusRemark)
    $("#CommunicationDtlAddBtn").hide();
    $("#divUpdateCommuniDtl").show();
    $("#tbl_CommunicationDetails >tbody >tr").each(function (i, rows) {
        var clickedrow = $(this);
        clickedrow.find("#delBtnIcon").css("filter", "grayscale(100%)");
        clickedrow.find("#delBtnIcon").removeClass("deleteIcon");
        clickedrow.find("#delBtnIcon").attr('onclick', '');

    });
    removeValueRequired_MsgValidate();

};
function removeValueRequired_MsgValidate() {
debugger;
 document.getElementById("vmCommunicatnTyp").innerHTML = null;
 $("#ddl_CommunicatnTyp").css("border-color", "#ced4da");
 $('#txt_CallDate').css("border-color", "#ced4da");
 $("#SpanCallDate").css("display", "none");
}
function OnClickCommunicatnDtlUpdateBtn() {
    debugger
    if (CheckCommunictnDtlValidation() == false) {
        return false;
    }
    else {
        debugger;
        $("#ddl_CommunicatnTyp").prop("disabled", false);
        var ddl_CommunicatnTyp = $("#ddl_CommunicatnTyp").val();
       
        var tableRow = $('#hdnUpdateInTableCommuniTbl').val();
        debugger;
        var CommTypVal = $("#ddl_CommunicatnTyp").val();
        var CommTypName = $("#ddl_CommunicatnTyp option:selected").text();
        var txt_CallDate = $("#txt_CallDate").val();
        var txt_ContactedBy = $("#txt_ContactedBy").val();
        var txt_ContactedTo = $("#txt_ContactedTo").val();
        var txt_ContactDetail = $("#txt_ContactDetail").val();
        var txt_Discusremarks = $("#txt_Discusremarks").val();
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_CommuTypeName").text(CommTypName);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_hdCommuTypVal").val(CommTypVal);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_CallDate").val(txt_CallDate);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_ContactBy").val(txt_ContactedBy);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_ContactTo").val(txt_ContactedTo);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_ContactDtl").val(txt_ContactDetail);
        $('#tbl_CommunicationDetails').find("tr:eq(" + tableRow + ")").find("#tbl_DiscusRemark").val(txt_Discusremarks);
        $('#Partial_txt_DiscussRemark').val("");
        $("#CommunicationDtlAddBtn").show();
        $("#divUpdateCommuniDtl").hide();
        var len = $("#tbl_CommunicationDetails >tbody >tr").length;
        $("#tbl_CommunicationDetails >tbody >tr").each(function (i, rows) {
            debugger;
            var clickedrow = $(this);
            clickedrow.find("#delBtnIcon").css("filter", "");
            clickedrow.find("#delBtnIcon").addClass("deleteIcon");
            clickedrow.find("#delBtnIcon").attr('onclick', 'deleteRow(this, event,"")');

        });
        resetCommunicationHeaderdetail();
        AddRemoveCommTyp();
    }
};
function resetCommunicationHeaderdetail() {
    debugger;
    $("#ddl_CommunicatnTyp").val("0");
    $("#txt_CallDate").val("");
    $("#txt_ContactedBy").val("");
    $("#txt_ContactedTo").val("");
    $("#txt_ContactDetail").val("");
    $("#txt_Discusremarks").val("");
};
function deleteRow(i, e, option) {
    debugger;
    var currentRowChild = $(e.target).closest('tr');
    var tbl_commutypnm = currentRowChild.find("#tbl_CommuTypeName").val();
    var tbl_commutypval = currentRowChild.find("#tbl_hdCommuTypVal").val();
    
    $("#ddl_CommunicatnTyp option[value=" + tbl_commutypval + "]").show();
    
    $(i).closest('tr').remove();
    SerialNoAfterDeleteCommTbl();
    resetCommunicationHeaderdetail();
    removeValueRequired_MsgValidate();
    debugger;
    $("#divAddNewCommuniDtl").css("display", "block");
    $("#CommunicationDtlAddBtn").show();
};
function SerialNoAfterDeleteCommTbl() {
    debugger;
    var SerialNo = 0;
    $("#tbl_CommunicationDetails >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);
    });
};
function InsertCommunicatnDetails() {
    debugger;
    //var QTNo = sessionStorage.getItem("DSQ_No");
    var SENo = $("#txt_EnquiryNo").val();
    var SEDate = $("#txt_EnquiryDate").val();
    var Branch = sessionStorage.getItem("BranchID");
    var SECommunictnDtlList = [];
    $("#tbl_CommunicationDetails >tbody >tr").each(function (i, row) {
        debugger;
        var CommuTyp = "";
        var CallDate = "";
        var ContactBy = "";
        var ContactTo = "";
        var ContactDtl = "";
        var DiscusRemark = "";

        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();
        CommuTyp = currentRow.find("#tbl_hdCommuTypVal").val();
        CallDate = currentRow.find("#tbl_CallDate").val();
        ContactBy = currentRow.find("#tbl_ContactBy").val();
        ContactTo = currentRow.find("#tbl_ContactTo").val();
        ContactDtl = currentRow.find("#tbl_ContactDtl").val();
        DiscusRemark = currentRow.find("#tbl_DiscusRemark").val();

        SECommunictnDtlList.push({ SENo: SENo, SEDate: SEDate, CommuTyp: CommuTyp, CallDate: CallDate, ContactBy: ContactBy, ContactTo: ContactTo, ContactDtl: ContactDtl, DiscusRemark: DiscusRemark });
    });
    return SECommunictnDtlList;
};

function FilterItemDetail(e) {//added by Prakash Kumar on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SlsEnqryItmDetailsTbl", [{ "FieldId": "SEItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
/*---------------COMMUNICATION SECTION END--------------- */