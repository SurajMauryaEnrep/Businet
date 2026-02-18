/************************************************
Javascript Name:Purchase Quotation
Created By:Suraj
Created Date: 17-11-2021
Description: This Javascript use for Purchase Quotation Page many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    //RemoveSession();
    //BindSupplierList();
    if ($("#Supplier").is(":checked")) {
       $("#div_RaiseOrder").css("display", "");
        
    }
    if ($("#Prospect").is(":checked")) {
       $("#div_RaiseOrder").css("display", "none");
    }
    BindPOItmList(1);
    $("#SupplierNameList").select2();
    debugger

    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        $("#PQItmDetailsTbl > tbody > tr").each(function () {
            debugger;
            var currentRow = $(this);
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#PQItemListName_" + RowSNo).val();
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_ItemID").val(ItmCode);
                $("#taxTemplate").text("GST Slab");
            }
        });
    }
    DeleteDeliverySch();
    DeleteTermsAndCondition();
    PQSelectAddress();

    $('#PQItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        debugger
        $(this).closest('tr').remove();
        debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#PQItemListName_" + SNo).val();
        /* var ItemCode = $(this).closest('tr')[0].cells[2].children[0].children[0].value;*/
        if ($("#ddlSourceType").val() != "D") {
            var len = $('#PQItmDetailsTbl tbody tr').length;
            if (len == "0") {
                $("#SourceDocumentNumber").attr("disabled", false);
                $("#AddQuotation").css("display", "block");

            }

        }
        if (status == "D" && command == "Edit") {
            $("#ddlPRNumberList").prop("disabled", true);
        }
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        //  ShowItemListItm(ItemCode);
        //removeTaxItemWise(ItemCode);
        CalculateGrossValue();
        var TOCAmount = parseFloat($("#PQ_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetPQ_ItemTaxDetail();
        DelDeliSchAfterDelItem(ItemCode);
        //ResetSrNoDelSch();
    });
    debugger
    if ($("#hfStatus").val() == "A") {
        $("#Cancelled").css("disabled", false);
    }
    else {
        $("#Cancelled").attr("disabled", true);
    }
    $("#Tbl_list_PQ #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.find("#QuotationNumber").text();
        var doc_date = clickedrow.find("#QuotationDate").text();
        if (doc_no != null && doc_no != "") {
            window.location.href = "/ApplicationLayer/PurchaseQuotation/PurchaseQuotationDetail/?doc_no=" + doc_no + "&doc_date=" + doc_date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.children("#QuotationNumber").text();
        var doc_date = clickedrow.children("#QuotationDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(doc_no, doc_date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(doc_no);
    });
    var ddlSourceType = $("#ddlSourceType").val();
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
        $("#Prosbtn").css("display", "none");
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
        if (ddlSourceType == "Q") {
            $("#Prosbtn").css("display", "none");
        } else {
            //BindSupplierList();
           // $("#Prosbtn").css("display", "block");
            CheckUserRolePageAccess(SuppPros_type, "RQ");
        }
    }
    //$("#PQItmDetailsTbl > tbody > tr").each(function () {
    //    var currentrow = $(this);
    //    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    //    var ItmCode = currentrow.find("#PQItemListName_" + RowSNo).val();
    //    var AssAmount = currentrow.find("#item_ass_val").val();
    //    if (currentrow.find("#TaxExempted").is(":checked")) {
    //        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
    //        $("#TxtTaxAmount").val(parseFloat(0).toFixed(ValDigit));
    //    }
    //    else {
    //        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
    //    }
    //});
});

function PQSelectAddress() {

    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        debugger;

        var clickedrow = $(e.target).closest("tr");

        if (clickedrow.find('#OrderTypeLocal').prop("disabled") == false) {
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

}

function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var PQDate = $("#txtPQDate").val();
        $.ajax({
            type: "POST",
            /* url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: PQDate
            },
            success: function (data) {
                /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var OrderStatus = "";
                    OrderStatus = $('#hfStatus').val().trim();
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
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
        /*End to chk Financial year exist or not*/
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
    var PQNo = "";
    var PQDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
     var forwardedto = "";
     var mailerror = "";

    Remarks = $("#fw_remarks").val();
    PQNo = $("#QuotationNumber").val();
    PQDate = $("#txtPQDate").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var TrancType = (PQNo + ',' + PQDate + ',' + "Update" + ',' + WF_status1)
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
     //Added by Nidhi on 08-07-2025
     if (fwchkval != "Approve") {
         var pdfAlertEmailFilePath = PQNo.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
         var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/PurchaseQuotation/SavePdfDocToSendOnEmailAlert");
         if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
             pdfAlertEmailFilePath = "";
         }
     }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PQNo != "" && PQDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/PurchaseQuotation/ApprovePQDetails?PQNo=" + PQNo + "&PQDate=" + PQDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
        // InsertPOApproveDetails("Approve", $("#hd_currlevel").val(), Remarks);
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseQuotation/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#QuotationNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
//----------------------------WorkFlow JS End-------------------------------------//
function GetViewDetails() {
    debugger;
    var UserID = $("#UserID").text();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var supp_id = $("#hdn_Supp_id").val();
    var deletetext = $("#Span_Delete_Title").text();

    $("#hdDoc_No").val($("#QuotationNumber").val());
    if (supp_id != null && supp_id != "") {
        var supp_name = $("#hdn_Supp_name").val();
        var s = '<option value=' + supp_id + '>' + supp_name + '</option>';
        $("#SupplierNameList").append(s);
        $("#SupplierNameList").val(supp_id);
        OnChangeSuppName(supp_id);
    }
    //if ($("#hdItemDetailList").val() != null && $("#hdItemDetailList").val()!="") {
    //    debugger
    //    var arr2 = $("#hdItemDetailList").val();       
    //    var arr = JSON.parse(arr2);
    //    $("#hdItemDetailList").val("");
    //    var a = 0;
    //    $("#PQItmDetailsTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);
    //         debugger
    //        if (a < arr.length) {

    //            var SNo = currentRow.find("#SNohiddenfiled").val();
    //            if (SNo != "1") {
    //                BindItemDdlPQ(SNo);
    //            }                
    //            currentRow.find("#PQItemListName_" + SNo).attr("onchange", "");
    //            currentRow.find("#PQItemListName_" + SNo).val(arr[a].item_id).trigger('change');
    //            currentRow.find("#hfItemID").val(arr[a].item_id);
    //            currentRow.find("#UOM").val(arr[a].uom_name);
    //            currentRow.find("#UOMID").val(arr[a].uom_id);
    //            currentRow.find("#QuotedQuantity").val(parseFloat(arr[a].qt_qty).toFixed(QtyDecDigit));
    //            currentRow.find("#QuotedPrice").val(parseFloat(arr[a].item_rate).toFixed(RateDecDigit));
    //            currentRow.find("#DiscountInPercentage").val(parseFloat(arr[a].item_disc_perc).toFixed(2));
    //            currentRow.find("#DiscountInAmount").val(parseFloat(arr[a].item_disc_amt).toFixed(ValDecDigit));
    //            currentRow.find("#DiscountedVal").val(parseFloat(arr[a].item_disc_val).toFixed(ValDecDigit));
    //            currentRow.find("#NetRate").val(parseFloat(arr[a].net_rate).toFixed(RateDecDigit));
    //            currentRow.find("#item_ass_val").val(parseFloat(arr[a].item_ass_val).toFixed(ValDecDigit));
    //            currentRow.find("#item_tax_amt").val(parseFloat(arr[a].item_tax_amt).toFixed(ValDecDigit));
    //            currentRow.find("#OtherCharges").val(parseFloat(arr[a].item_oc_amt).toFixed(ValDecDigit));
    //            currentRow.find("#NetValue").val(parseFloat(arr[a].item_net_val_spec).toFixed(ValDecDigit));
    //            currentRow.find("#remarks").val(arr[a].it_remarks);
    //            currentRow.find("#PQItemListName_" + SNo).attr("onchange", "OnChangePQItemName(event)");
    //        }
    //        a = a + 1;
    //    });
    //}   
    //if ($("#hdrfqItemDetailList").val() != null && $("#hdrfqItemDetailList").val() != "") {
    //    debugger
    //    var arr2 = $("#hdrfqItemDetailList").val();
    //    var arr = JSON.parse(arr2);
    //    $("#hdrfqItemDetailList").val("");
    //    var a = 0;
    //    $("#PQItmDetailsTbl >tbody >tr").each(function () {
    //        var currentRow = $(this);      
    //        if (a < arr.length) {

    //            var SNo = currentRow.find("#SNohiddenfiled").val();
    //           // BindItemDdlPQ(SNo);
    //            currentRow.find("#PQItemListName_" + SNo).val(arr[a].item_id).trigger('change');              
    //            currentRow.find("#UOMID").val(arr[a].uom_id);
    //            currentRow.find("#QuotedQuantity").val(parseFloat(arr[a].rfq_qty).toFixed(QtyDecDigit));               
    //            currentRow.find("#remarks").val(arr[a].it_remarks);
    //        }
    //        a = a + 1;
    //    });

    //} 
    //if ($("#hdTaxDetailList").val() != null && $("#hdTaxDetailList").val() != "") {
    //    var arr2 = $("#hdTaxDetailList").val();
    //    var arr = JSON.parse(arr2);
    //    $("#hdTaxDetailList").val("");

    //    if (arr.length > 0) {
    //        var rowIdx = 0;
    //        var TaxCalculationList = [];
    //        for (var j = 0; j < arr.length; j++) {
    //            var RowSNo;
    //            $("#PQItmDetailsTbl >tbody >tr").each(function () {
    //               // debugger;
    //                var currentRow = $(this);
    //                var SNo = currentRow.find("#SNohiddenfiled").val();
    //                var ItmCode = "";

    //                ItmCode = currentRow.find('#PQItemListName_' + SNo).val();

    //                if (ItmCode == arr[j].item_id) {
    //                    RowSNo = SNo;
    //                }
    //            });
    //            var TaxItmCode = arr[j].item_id;
    //            var TaxName = arr[j].tax_name;
    //            var TaxNameID = arr[j].tax_id;
    //            var TaxPercentage = arr[j].tax_rate + "%";
    //            var TaxLevel = arr[j].tax_level;
    //            var TaxApplyOn = arr[j].tax_apply_Name;
    //            var TaxAmount = arr[j].tax_val;
    //            var TotalTaxAmount = arr[j].item_tax_amt;
    //            var TaxApplyOnID = arr[j].tax_apply_on;

    //            TaxCalculationList.push({ UserID: UserID, RowSNo: RowSNo, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    //        }
    //        ////debugger;
    //        sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
    //        BindTaxAmountDeatils(TaxCalculationList);
    //    }
    //}
    //    if ($("#hdOCDetailList").val() != null && $("#hdOCDetailList").val() != "") {
    //        var arr2 = $("#hdOCDetailList").val();
    //        var arr = JSON.parse(arr2); 
    //        $("#hdOCDetailList").val("");
    //        if (arr.length > 0) {
    //            debugger
    //            var rowIdx = 0;
    //            for (var k = 0; k < arr.length; k++) {
    //                $('#Tbl_OC_Deatils tbody').append(`<tr id="R${++rowIdx}">
    //                            <td class=" red"><i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" title="${deletetext}"></i></td>
    //                            <td id="OCName">${arr[k].oc_name}</td>
    //                    <td id="OCCurr">${arr[k].curr_name}</td>
    //                    <td id="OCConv">${parseFloat(arr[k].conv_rate).toFixed(RateDecDigit)}</td>
    //                            <td hidden="hidden" id="OCValue">${arr[k].oc_id}</td>
    //                            <td class="num_right" id="OCAmount">${parseFloat(arr[k].oc_val).toFixed(ValDecDigit)}</td>
    //                            <td align="right" id="OcAmtBs">${parseFloat(arr[k].OCValBs).toFixed(ValDecDigit)}</td>
    //                            </tr>`);
    //                $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
    //                <td id="OC_name">${arr[k].oc_name}</td>
    //                <td id="OC_Curr">${arr[k].curr_name}</td>
    //                <td id="OC_Conv">${parseFloat(arr[k].conv_rate).toFixed(RateDecDigit)}</td>
    //                <td hidden="hidden" id="OC_ID">${arr[k].oc_id}</td>
    //                <td class="num_right" id="OCAmtBs">${parseFloat(arr[k].oc_val).toFixed(ValDecDigit)}</td>
    //<td align="right" id="OCAmtSp">${parseFloat(arr[k].OCValBs).toFixed(ValDecDigit)}</td>
    //                </tr>`);
    //            }
    //            $("#Total_OC_Amount").text(parseFloat($("#PQ_OtherCharges").val()).toFixed(ValDecDigit));
    //            debugger
    //            BindOtherChargeDeatils();
    //        }
    //    }
    //BindOtherChargeDeatils();
    //    if ($("#hdDelSchDetailList").val() != null && $("#hdDelSchDetailList").val() != "") {
    //        var arr2 = $("#hdDelSchDetailList").val();
    //        var arr = JSON.parse(arr2);
    //        $("#hdDelSchDetailList").val();
    //        if (arr.length > 0) {
    //            var rowIdx = 0;
    //            for (var x = 0; x < arr.length; x++) {
    //                $('#DeliverySchTble tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
    //<td class=" red"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="DeliveryDelIconBtn" title="@Resource.Delete"></i></td>
    //<td>${++rowIdx}</td>
    //                            <td class="a-center ">${arr[x].item_name}</td>
    //                            <td  hidden="hidden">${arr[x].item_id}</td>
    //                            <td >${moment(arr[x].sch_date).format('DD-MM-YYYY')}</td>
    //                            <td >${parseFloat(arr[x].delv_qty).toFixed(QtyDecDigit)}</td>

    //                            </tr>`);
    //            }
    //        }
    //    }
    //if ($("#hdTermsDetailList").val() != null && $("#hdTermsDetailList").val() != "") {
    //    var arr2 = $("#hdTermsDetailList").val();
    //    var arr = JSON.parse(arr2);
    //    $("#hdTermsDetailList").val();
    //    if (arr.length > 0) {
    //        var rowIdx = 0;
    //        for (var y = 0; y < arr.length; y++) {
    //            $('#TblTerms_Condition tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
    //                        <td>${arr[y].term_desc}</td>
    //                        <td class="red"><i id="TC_DelIcon" class="deleteIcon fa fa-trash" title="@Resource.Delete"></i></td>
    //                        </tr>`);
    //        }
    //    }
    //}

    var ddlSourceType = $("#ddlSourceType").val();
    if (ddlSourceType == "Q" || ddlSourceType == "R") {
        $("#ForRFQ").css("display", "Block");
        $("#AddNewRowIconID").css("display", "none");
        $("#AddQuotation").css("display", "block");
        $("#PQItmDetailsTbl tbody tr select ,#QuotedQuantity").attr("disabled", true);
    } else {
        $("#ForRFQ").css("display", "none");
        $("#AddNewRowIconID").css("display", "block");
    }
    BindDelivertSchItemList();
    EnableTaxAndOC();
    if ($("#PQItmDetailsTbl tbody tr").length == 1 && ($("#QuotedQuantity").val() == null || $("#QuotedQuantity").val() == "")) {
        if ($("#SupplierNameList").val() == "0") {
            $("#PQItmDetailsTbl tbody tr input").attr("disabled", true);
            $("#PQItmDetailsTbl tbody tr select").attr("disabled", true);
            $("#AddNewRowIconID").css("display", "none");
        }
    }
    var Date12 = moment().format('YYYY-MM-DD');

    $("#HistoryTo_date").val(Date12);

}

function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#PQItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};
function BindSupplierList() {
    debugger;
    var SuppPros_type = "S";
    var ddlSourceType = $("#ddlSourceType").val();
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
        $("#div_RaiseOrder").css("display", "");
        $("#Prosbtn").css("display", "none");
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
        $("#div_RaiseOrder").css("display", "none");
        if (ddlSourceType == "Q") {
            $("#Prosbtn").css("display", "none");
        } else {
            //$("#Prosbtn").css("display", "block");
            CheckUserRolePageAccess(SuppPros_type, "RQ");
        }

    }
    $('#SupplierNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
    var status = $("#hfStatus").val();
    if (status == "") {
        $("#Address").val("");
    }
    $("#SupplierNameList").select2({

        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    SuppPros_type: SuppPros_type
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {

                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };

            },

        },

    });

    if (ddlSourceType == "Q") {
        if ($("#QuotationNumber").val() == "") {
            EnableForRfq();
        }
    }

}
//function RefreshBtnClick() {
//    //RemoveSession();

//    $("#btn_back").attr('onclick', "BackBtnClick()");
//    $("#btn_add_new_item").attr('onclick', "AddNewPOBtnClick()");
//    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
//    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
//    $("#Btn_Forward").attr("data-target", "");
//    $("#Btn_Forward").attr('onclick', '');
//    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    $("#btn_save").attr('onclick', '');
//    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    $("#btn_close").attr('onclick', '');
//    $("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    $("#Btn_Edit").attr('onclick', '');
//    $("#Btn_Delete").attr('onclick', '');
//    $("#Btn_Approve").attr('onclick', '');
//    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//}
//function RemoveSession() {
//    sessionStorage.removeItem("TaxCalcDetails");

//}
function BindPOItmList(ID) {
    //debugger;
    BindItemList("#PQItemListName_", ID, "#PQItmDetailsTbl", "#SNohiddenfiled", "BindData", "PQ");

}
function BindData() {

    $("#PQItmDetailsTbl").each(function () {
        debugger
        var currentrow = $(this);
        var ID = currentrow.find("#SNohiddenfiled").val();
        if (ID != "1") {
            BindItemDdlPQ(ID);
        }

    })
    GetViewDetails();
}
function BindItemDdlPQ(rowid) {
    if ($("#PQItemListName_" + rowid).val() == "0" || $("#PQItemListName_" + rowid).val() == "" || $("#PQItemListName_" + rowid).val() == null) {
        $("#PQItemListName_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + rowid).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    //DynamicSerchableItemDDL("#PQItmDetailsTbl", "#PQItemListName_", rowid, "#SNohiddenfiled", "", "PO")
    DynamicSerchableItemDDL("#PQItmDetailsTbl", "#PQItemListName_", rowid, "#SNohiddenfiled", "", "PQ")
    //BindItemList("#PQItemListName_", rowid, "#PQItmDetailsTbl", "#SNohiddenfiled", "", "PO");
    //var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    //if (PLItemListData != null) {
    //    if (PLItemListData.length > 0) {
    //        $("#PQItemListName_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
    //        for (var i = 0; i < PLItemListData.length; i++) {               
    //                $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);            
    //        }
    //        var firstEmptySelect = true;
    //        $("#PQItemListName_" + rowid).select2({
    //            templateResult: function (data) {
    //                debugger
    //                var selected = $("#PQItemListName_" + rowid).val();
    //                if (check(data, selected, "#PQItmDetailsTbl", "#SNohiddenfiled","#PQItemListName_") == true) {
    //                    var UOM = $(data.element).data('uom');
    //                    var classAttr = $(data.element).attr('class');
    //                    var hasClass = typeof classAttr != 'undefined';
    //                    classAttr = hasClass ? ' ' + classAttr : '';
    //                    var $result = $(
    //                        '<div class="row">' +
    //                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                        '</div>'
    //                    );
    //                    return $result;
    //                }
    //                firstEmptySelect = false;
    //            }
    //        });
    //    }
    //}
    //else {
    //    BindItemList("#PQItemListName_", rowid, "#PQItmDetailsTbl", "#SNohiddenfiled", "", "PO");
    //}


}

function AddNewRow() {
    debugger;
    var row_id = 0;
    var SrNo = $("#PQItmDetailsTbl tbody tr").length;
    SrNo = parseInt(SrNo) + 1;

    var deletetext = $("#Span_Delete_Title").text();
    var UOM = $("#ItemUOM").text();//
    var span_remarks = $("#span_remarks").text();
    $("#PQItmDetailsTbl tbody tr").each(function () {
        debugger
        row_id = $(this).find("#SNohiddenfiled").val();
    })
    row_id = parseInt(row_id) + 1;


    CreateNewRow('', '', row_id, SrNo);
    BindItemDdlPQ(row_id);
}
function CreateNewRow(subitem, subitmDisable, row_id, SrNo) {
    debugger;

    var subitm_EnablDisbl = "";
    if (subitmDisable != "Y") {

        subitm_EnablDisbl = "disabled"
    }
    //else {
    //    subitm_EnablDisbl = "Enabled";
    //}
    var deletetext = $("#Span_Delete_Title").text();
    var Sourcetyp = $("#ddlSourceType option:selected").val();
    var subitem_val = "";

    if (Sourcetyp == "D") {
        subitem_val = `<div class="col-sm-2 i_Icon no-padding" id="div_SubItemQTQty">
                <input hidden type="text" id="sub_item" value="${subitem}" />
                 <button type="button" id="SubItemQTQty" class="calculator subItmImg" ${subitm_EnablDisbl} onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>

</div >`
    }
    else {
        //$("#PQItemListName_"+row_id).
        subitem_val = `<div class="col-sm-2 i_Icon no-padding" id="div_SubItemPRRFQQTQty">
<input hidden type="text" id="sub_item" value="${subitem}" />
                <button type="button" id="SubItemPRRFQQTQty" ${subitm_EnablDisbl} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('PRFQQtQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>

</div >`

    }
    var UOM = $("#ItemUOM").text();//
    var span_remarks = $("#span_remarks").text();
    var ManualGst = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
    }
    $("#PQItmDetailsTbl tbody").append(`
<tr>
                                                                <td class="red" > <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon${row_id}" title="${deletetext}"></i></td>
                                                                <td id="srno" class="sr_padding">${SrNo}</td>
                                                                <td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${row_id}" /></td>
                                                                <td class="itmStick">
                                                                    <div class="col-sm-10 lpo_form" style="padding:0px;" id='multiWrapper'>
                                                                        <select class="form-control" id="PQItemListName_${row_id}" name="PQItemListName_${row_id}" onchange="OnChangePQItemName(event)">
                                                                        </select>
                                                                        <input type="hidden" id="Hdn_Item_ID" value="">
                                                                        <span id="PQItemListNameError" class="error-message is-visible"></span>
                                                                    </div>
                                                                <div class="col-sm-2 i_Icon">
                                                                <div class="col-sm-4 i_Icon">
                                                                        <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"></button>
                                                                    </div>
                                                                    <div class="col-sm-4 i_Icon"><button type="button" id="SupplierInformation" class="calculator" onclick="" data-toggle="modal" data-target="#SupplierInformation"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button></div>
                                                               <div class="col-sm-4 i_Icon"><button type="button" id="PurchaseHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#PurchaseHistory"> <i class="fa fa-history" aria-hidden="true" data-toggle="" title="${$("#span_PurchaseHistory_Title").text()}"></i> </button></div> </td>
                                                              
                                                                </div><td>
                                                                      <select id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" onchange="onChangeUom(event)">
                                                                      <option value="0">---Select---</option>
                                                                      </select>  
                                                                    <input id="UOMID" type="hidden" />
                                                                    <input id="ItemHsnCode" type="hidden" />
                                                                    <input id="ItemtaxTmplt" type="hidden" />
                                                                </td>
                                                                     <td>
                                                                    <input id="IDItemtype" class="form-control" autocomplete="off" type="text" name="ItemType" value="" placeholder="${$("#ItemType").text()}" disabled>
                                                                           </td>
                                                                 <td>
                                                                    <div class="col-sm-10 lpo_form no-padding">
                                                                    <input id="QuotedQuantity" onchange="OnchangeQuotedQuantity(event)" onkeyup="OnKeyupQuotedQty(event)" onkeypress="return AmountFloatQty(this, event);" class="form-control num_right" autocomplete="off" type="text" name="QuotedQuantity" placeholder="0000.00"  >
                                                                    <span id="Qtd_qty_Error" class="error-message is-visible"></span>
                                                                    </div>
                                                                    `+ subitem_val + `
                                                                </td>

                                                                <td>
                                                                 <div class="lpo_form">
                                                                    <input id="QuotedPrice" onchange="OnchangeQuotedPrice(event)" onkeyup="OnKeyupQuotedPrice(event)" onkeypress="return AmountFloatRate(this, event);" class="form-control num_right" autocomplete="off" type="text" name="QuotedPrice"  placeholder="0000.00"  >
                                                                        <span id="Qtd_price_Error" class="error-message is-visible"></span>
                                                                            </div>                                                                
                                                                        </td>
                                                                <td>
                                                                    <input id="DiscountInPercentage" onchange="OnChangeQTItemDiscountPerc(event)" onkeypress="return AmountFloatPer(this, event)" class="form-control num_right" autocomplete="off" type="text" name="DiscountInPercentage"  placeholder="0000.00"  >
                                                                </td>
                                                                <td>
                                                                    <div class="lpo_form">
                                                                    <input id="DiscountInAmount" onchange="OnChangeQTItemDiscountAmt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text" name="DiscountInAmount"  placeholder="0000.00"  >
                                                                  <span id="DiscAmt_price_Error" class="error-message is-visible"></span>
                                                                    </div>
                                                                </td>
                                                                <td style="display:none">
                                                                    <input id="DiscountedVal" type="hidden">
                                                                </td>
                                                                <td>
                                                                    <input id="NetRate" class="form-control num_right" autocomplete="off" type="text" name="NetRate"  placeholder="0000.00"  disabled>
                                                                </td>
                                                                 <td>
                                                                    <div class="lpo_form">
                                                                        <input id="item_ass_val" class="form-control date num_right" autocomplete="off" type="text" onkeypress="return AmountFloatVal(this,event);" onchange="OnchangeAssessableValue(event)" name="item_ass_val" required="required" placeholder="0000.00"  disabled="disabled">
                                                                        <span id="item_ass_valError" class="error-message is-visible"></span>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted" style="left:-20px;" ><label class="custom-control-label" for="" style="padding: 3px 0px;" > </label></div>
                                                                </td>
                                                               `+ ManualGst + `
                                                                <td disabled="disabled" width="8%">
                                                                <div class="col-sm-10 lpo_form" style="padding:0px;">
                                                                    <input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt" required="required" placeholder="0000.00"  disabled="disabled">
                                                                </div>
                                                                <div class="col-sm-2 lpo_form" style="padding:0px;">
                                                                    <button type="button" class="calculator" id="BtnTxtCalculation" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false" value=""><i class="fa fa-calculator" aria-hidden="true" title="Tax Calculator" data-original-title="Tax Calculator" disabled="disabled"></i></button>
                                                                </div>
                                                                </td>
                                                                <td>
                                                                    <input id="OtherCharges" class="form-control num_right" autocomplete="off" type="text" name="OtherCharges"  placeholder="0000.00"  disabled>
                                                                </td>
                                                                <td>
                                                                    <input id="NetValue" class="form-control num_right" autocomplete="off" type="text" name="NetValue"  placeholder="0000.00"  disabled>
                                                                </td>
                                                                <td>
                                                                    <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "100",  placeholder="${$("#span_remarks").text()}"></textarea>
                                                                </td>
                                                            </tr>
`)
}
//-------------------------Vallidation----------------------------------------//
function HeaderValidations() {
    var ddlSourceType = $("#ddlSourceType").val();
    var SupplierNameList = $("#SupplierNameList").val();
    var Address = $("#Address").val();
    var ValidUptoDate = $("#ValidUptoDate").val();
    var ErrorFlag = "N";
    if (ddlSourceType == "0") {
        $("#ddlSourceType").css("border-color", "red");
        $("#vmSourceType").text($("#valueReq").text());
        $("#vmSourceType").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#ddlSourceType").css("border-color", "#ced4da");
        $("#vmSourceType").text("");
        $("#vmSourceType").css("display", "none");
    }
    if (SupplierNameList == "0") {
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "red");
        $("#vmSupplierNameList").text($("#valueReq").text());
        $("#vmSupplierNameList").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "#ced4da");
        $("#vmSupplierNameList").text("");
        $("#vmSupplierNameList").css("display", "none");
    }
    if (Address == "") {
        $("#Address").css("border-color", "red");
        $("#SpanSuppAddrErrorMsg").text($("#valueReq").text());
        $("#SpanSuppAddrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#Address").css("border-color", "#ced4da");
        $("#SpanSuppAddrErrorMsg").text("");
        $("#SpanSuppAddrErrorMsg").css("display", "none");
    }
    if (ValidUptoDate == "") {
        $("#ValidUptoDate").css("border-color", "red");
        $("#vmValidUptoDate").text($("#valueReq").text());
        $("#vmValidUptoDate").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#ValidUptoDate").css("border-color", "#ced4da");
        $("#vmValidUptoDate").text("");
        $("#vmValidUptoDate").css("display", "none");
    }
    if (ddlSourceType == "Q" && SupplierNameList != "0") {
        var SourceDocumentNumber = $("#SourceDocumentNumber").val()
        if (SourceDocumentNumber == "---Select---" || SourceDocumentNumber == "0" || SourceDocumentNumber == "") {
            $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "red");
            $("#SourceDocumentNumber").css("border-color", "red");
            $("#vmSourceDocumentNumber").text($("#valueReq").text());
            $("#vmSourceDocumentNumber").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "#ced4da");
            $("#SourceDocumentNumber").css("border-color", "#ced4da");
            $("#vmSourceDocumentNumber").text("");
            $("#vmSourceDocumentNumber").css("display", "none");
        }
    }



    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }

}
function CheckVallidation(main_ID, Message_ID) {

    debugger
    var ddlSourceType = $("#" + main_ID).val();

    var ErrorFlag = "N";
    if (ddlSourceType == "0" && ddlSourceType == "") {
        $("#" + main_ID).css("border-color", "red");
        $("#" + Message_ID).text($("#valueReq").text());
        $("#" + Message_ID).css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#" + main_ID).css("border-color", "#ced4da");
        $("#" + Message_ID).text("");
        $("#" + Message_ID).css("display", "none");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function checkitemvallidation(currentrow, QuotedQuantity, Qtd_qty_Error) {
    var check = currentrow.find("#" + QuotedQuantity).val();
    if (check == "" || check == null) {
        currentrow.find("#" + QuotedQuantity).css("border-color", "red");
        currentrow.find("#" + Qtd_qty_Error).text($("#valueReq").text());
        currentrow.find("#" + Qtd_qty_Error).css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        currentrow.find("#" + QuotedQuantity).css("border-color", "#ced4da");
        currentrow.find("#" + Qtd_qty_Error).text("");
        currentrow.find("#" + Qtd_qty_Error).css("display", "none");
    }
}
function OnKeyupQuotedQty(e) {
    var currentrow = $(e.target).closest('tr');
    checkitemvallidation(currentrow, "QuotedQuantity", "Qtd_qty_Error");
}
function OnKeyupQuotedPrice(e) {

    var currentrow = $(e.target).closest('tr');
    checkitemvallidation(currentrow, "QuotedPrice", "Qtd_price_Error");
}

function ItemsVallidation() {
    var ErrorFlag = "N";

    if ($("#PQItmDetailsTbl tbody tr").length > 0) {
        $("#PQItmDetailsTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohiddenfiled").val();
            var ItemName = currentrow.find("#PQItemListName_" + Rowno).val();
            var QuotedQuantity = currentrow.find("#QuotedQuantity").val();
            var QuotedPrice = currentrow.find("#QuotedPrice").val();
            var DiscountInAmount = currentrow.find("#DiscountInAmount").val();
            if (ItemName == "0") {
                currentrow.find("[aria-labelledby='select2-PQItemListName_" + Rowno + "-container']").css("border-color", "red");
                currentrow.find("#PQItemListNameError").text($("#valueReq").text());
                currentrow.find("#PQItemListNameError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("[aria-labelledby='select2-PQItemListName_" + Rowno + "-container']").css("border-color", "#ced4da");
                currentrow.find("#PQItemListNameError").text("");
                currentrow.find("#PQItemListNameError").css("display", "none");
            }
            if (QuotedQuantity == "" || QuotedQuantity == null || QuotedQuantity == "0.000") {
                currentrow.find("#QuotedQuantity").css("border-color", "red");
                currentrow.find("#Qtd_qty_Error").text($("#valueReq").text());
                currentrow.find("#Qtd_qty_Error").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("#QuotedQuantity").css("border-color", "#ced4da");
                currentrow.find("#Qtd_qty_Error").text("");
                currentrow.find("#Qtd_qty_Error").css("display", "none");
            }
            if (QuotedPrice == "" || QuotedPrice == null || QuotedPrice == "0.000") {
                currentrow.find("#QuotedPrice").css("border-color", "red");
                currentrow.find("#Qtd_price_Error").text($("#valueReq").text());
                currentrow.find("#Qtd_price_Error").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("#QuotedPrice").css("border-color", "#ced4da");
                currentrow.find("#Qtd_price_Error").text("");
                currentrow.find("#Qtd_price_Error").css("display", "none");
            }
            if (parseFloat(CheckNullNumber(DiscountInAmount)) >= parseFloat(CheckNullNumber(QuotedPrice))) {
                currentrow.find("#DiscountInAmount").css("border-color", "red");
                currentrow.find("#DiscAmt_price_Error").text($("#GDAmtTORate").text());
                currentrow.find("#DiscAmt_price_Error").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("#DiscountInAmount").css("border-color", "#ced4da");
                currentrow.find("#DiscAmt_price_Error").text("");
                currentrow.find("#DiscAmt_price_Error").css("display", "none");
            }

        });
    } else {
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
function OnChangeSourceType() {
    var SuppPros_type = "";
    CheckVallidation("ddlSourceType", "vmSourceType");
    var ddlSourceType = $("#ddlSourceType").val();
    debugger
    if (ddlSourceType == "Q") {
        if ($("#Prospect").is(":checked")) {
            $("#Prosbtn").css("display", "none");
        }
       // $("#div_RaiseOrder").css("display", "none");
        $("#ForRFQ").css("display", "Block");
        $("#AddNewRowIconID").css("display", "none");
        $("#AddQuotation").css("display", "none");
        if ($("#SourceDocumentNumber").val() != "0" && $("#SourceDocumentNumber").val() != "---Select---") {
            //$('#SupplierNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
            $("#Address").val("");
        }
        $('#SupplierNameList').val(0).trigger('change');
        $('#SourceDocumentNumber').empty().append('<option value="0" selected="selected">---Select---</option>');
        $("#SourceDocumentNumber").attr("disabled", false);
        $("#txtsrcdocdate").val("");

        $("#PQItmDetailsTbl tbody tr").remove();
        CreateNewRow('', '', 1, 1);
        BindItemDdlPQ(1);
        if ($("#QuotationNumber").val() == "") {
            EnableForRfq();
        }
    }
    else if (ddlSourceType == "R") {
        if ($("#Prospect").is(":checked")) {
            //$("#Prosbtn").css("display", "block");
            SuppPros_type = "P";
            CheckUserRolePageAccess(SuppPros_type, "RQ");
        }
        $("#div_RaiseOrder").css("display", "Block");
        $("#ForRFQ").css("display", "Block");
        $("#AddNewRowIconID").css("display", "none");

        if ($("#SourceDocumentNumber").val() != "0" && $("#SourceDocumentNumber").val() != "---Select---") {
            $('#SupplierNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
            $("#Address").val("");
        }
        $('#SupplierNameList').val(0).trigger('change');
        $("#AddQuotation").css("display", "none");
        $('#SourceDocumentNumber').empty().append('<option value="0" selected="selected">---Select---</option>');
        $("#SourceDocumentNumber").attr("disabled", false);
        $("#txtsrcdocdate").val("");

        $("#PQItmDetailsTbl tbody tr").remove();
        CreateNewRow('', '', 1, 1);
        BindItemDdlPQ(1);
        if ($("#QuotationNumber").val() == "") {
            EnableForPR();
            GetPrList();
        }
    }
    else {
        if ($("#Prospect").is(":checked")) {
          //  $("#Prosbtn").css("display", "block");
            SuppPros_type = "P";
            CheckUserRolePageAccess(SuppPros_type, "RQ");
        }
        $('#SupplierNameList').val(0).trigger('change');
        //$('#SupplierNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
        $("#ForRFQ").css("display", "none");
        $("#Address").val("");
        $("#PQItmDetailsTbl tbody tr").remove();
        CreateNewRow('', '', 1, 1);
        BindItemDdlPQ(1);
    }
}
function GetPrList() {
    var ddlSourceType = $("#ddlSourceType").val();
    if (ddlSourceType == "R") {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/PurchaseQuotation/GetPRList",
                data: {

                },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {

                            $("#SourceDocumentNumber option").remove();
                            $("#SourceDocumentNumber optgroup").remove();
                            $('#SourceDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#Textddl').append(`<option data-date="${arr.Table[i].pr_dt}" value="${arr.Table[i].pr_no}">${arr.Table[i].pr_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#SourceDocumentNumber').select2({
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

                            $("#txtsrcdocdate").val("");
                            //$("#AddQuotation").css("display", "block");
                        }
                    }
                },
            });
    }
}
function EnableForPR() {
    debugger
    var ddlSourceType = $("#ddlSourceType").val();

    // if ($("#SupplierNameList").val() != "0") {
    $("#SourceDocumentNumber").attr("disabled", true);
    $("#PQItmDetailsTbl tbody tr #PQItemListName_1,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount").attr("disabled", false);

    if (ddlSourceType == "R") {
        $("#AddNewRowIconID").css("display", "none");
        $("#PQItmDetailsTbl tbody tr #PQItemListName_1,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount").attr("disabled", true);
        // $("#PQItmDetailsTbl tbody tr select ,#QuotedQuantity").attr("disabled", true);

    }
    else {
        $("#AddNewRowIconID").css("display", "block");
    }
    //  }
    //else {
    //    if ($("#SourceDocumentNumber").val() != "0") {
    //        $('#SourceDocumentNumber').empty().append('<option value="0" selected="selected">---Select---</option>');
    //    }
    //    $("#PQItmDetailsTbl tbody tr").remove();
    //    CreateNewRow(1, 1);
    //    BindItemDdlPQ(1);
    //    $("#AddNewRowIconID").css("display", "none");
    //    $("#PQItmDetailsTbl tbody tr input").attr("disabled", true);
    //    $("#PQItmDetailsTbl tbody tr select").attr("disabled", true);
    //    //$("#AddNewRowIconID").css("display", "none");

    //    $("#SourceDocumentNumber").attr("disabled", true);

    //}
}

function EnableForRfq() {
    debugger
    var ddlSourceType = $("#ddlSourceType").val();

    if ($("#SupplierNameList").val() != "0") {
        $("#SourceDocumentNumber").attr("disabled", false);
        $("#PQItmDetailsTbl tbody tr #PQItemListName_1,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount").attr("disabled", false);

        if (ddlSourceType == "Q") {
            $("#AddNewRowIconID").css("display", "none");
            $("#PQItmDetailsTbl tbody tr #PQItemListName_1,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount").attr("disabled", true);
            // $("#PQItmDetailsTbl tbody tr select ,#QuotedQuantity").attr("disabled", true);

        }
        else {
            $("#AddNewRowIconID").css("display", "block");
        }
    }
    else {
        if ($("#SourceDocumentNumber").val() != "0") {
            $('#SourceDocumentNumber').empty().append('<option value="0" selected="selected">---Select---</option>');
        }
        $("#PQItmDetailsTbl tbody tr").remove();
        CreateNewRow('', '', 1, 1);
        BindItemDdlPQ(1);
        $("#AddNewRowIconID").css("display", "none");
        $("#PQItmDetailsTbl tbody tr input").attr("disabled", true);
        $("#PQItmDetailsTbl tbody tr select").attr("disabled", true);
        //$("#AddNewRowIconID").css("display", "none");

        $("#SourceDocumentNumber").attr("disabled", true);

    }
}
function OnChangeVallidUpto() {
    CheckVallidation("ValidUptoDate", "vmValidUptoDate");

}
function OnChangeSuppName(SuppID) {
    var SupplierNameList = $("#SupplierNameList").val();
    if (SupplierNameList == "0") {
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "red");
        $("#vmSupplierNameList").text($("#valueReq").text());
        $("#vmSupplierNameList").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "#ced4da");
        $("#vmSupplierNameList").text("");
        $("#vmSupplierNameList").css("display", "none");
    }

    var SuppPros_type = "S";
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
    }
    var status = $("#hfStatus").val();
    if (status == "") {
        try {

            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/PurchaseQuotation/GetSuppAddrDetail",
                    data: {
                        Supp_id: SuppID, SuppPros_type: SuppPros_type
                    },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            PO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#Address").val(arr.Table[0].BillingAddress);
                                $("#bill_add_id").val(arr.Table[0].bill_add_id);
                                $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                                $("#Ship_StateCode").val(arr.Table[0].state_code);
                                //var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';                                
                                $("#hdn_Payterm").val(arr.Table[0].paym_term);
                                $("#currencyID").val(arr.Table[0].curr_id);
                                $("#conv_rate").val(parseFloat(1).toFixed($("#RateDigit").text()));
                                //$("#conv_rate").val(1);
                                
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
                        }
                        var ddlSourceType = $("#ddlSourceType").val();
                        if (ddlSourceType == "Q") {
                            $.ajax(
                                {
                                    type: "POST",
                                    url: "/ApplicationLayer/PurchaseQuotation/GetSuppRFQList",
                                    data: {
                                        Supp_id: SuppID, SuppPros_type: SuppPros_type
                                    },
                                    success: function (data) {
                                        debugger;
                                        if (data == 'ErrorPage') {
                                            PO_ErrorPage();
                                            return false;
                                        }
                                        if (data !== null && data !== "") {
                                            var arr = [];
                                            arr = JSON.parse(data);
                                            if (arr.Table.length > 0) {

                                                $("#SourceDocumentNumber option").remove();
                                                $("#SourceDocumentNumber optgroup").remove();
                                                $('#SourceDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                                                for (var i = 0; i < arr.Table.length; i++) {
                                                    $('#Textddl').append(`<option data-date="${arr.Table[i].rfq_dt}" value="${arr.Table[i].rfq_no}">${arr.Table[i].rfq_no}</option>`);
                                                }
                                                var firstEmptySelect = true;
                                                $('#SourceDocumentNumber').select2({
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

                                                $("#txtsrcdocdate").val("");
                                                $("#AddQuotation").css("display", "block");
                                            }
                                        }
                                    },
                                });
                        }
                        if (ddlSourceType == "D") {
                            $("#AddNewRowIconID").css("display", "block");
                            $("#PQItmDetailsTbl tbody tr select ,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount,#TaxExempted").attr("disabled", false);
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                $("#PQItmDetailsTbl tbody tr select ,#ManualGST").attr("disabled", false);
                            }
                        }
                        //if (ddlSourceType == "R") {
                        //    $.ajax(
                        //        {
                        //            type: "POST",
                        //            url: "/ApplicationLayer/PurchaseQuotation/GetPRList",
                        //            data: {
                        //                Supp_id: SuppID, SuppPros_type: SuppPros_type
                        //            },
                        //            success: function (data) {
                        //                debugger;
                        //                if (data == 'ErrorPage') {
                        //                    PO_ErrorPage();
                        //                    return false;
                        //                }
                        //                if (data !== null && data !== "") {
                        //                    var arr = [];
                        //                    arr = JSON.parse(data);
                        //                    if (arr.Table.length > 0) {

                        //                        $("#SourceDocumentNumber option").remove();
                        //                        $("#SourceDocumentNumber optgroup").remove();
                        //                        $('#SourceDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        //                        for (var i = 0; i < arr.Table.length; i++) {
                        //                            $('#Textddl').append(`<option data-date="${arr.Table[i].rfq_dt}" value="${arr.Table[i].rfq_no}">${arr.Table[i].rfq_no}</option>`);
                        //                        }
                        //                        var firstEmptySelect = true;
                        //                        $('#SourceDocumentNumber').select2({
                        //                            templateResult: function (data) {
                        //                                var DocDate = $(data.element).data('date');
                        //                                var classAttr = $(data.element).attr('class');
                        //                                var hasClass = typeof classAttr != 'undefined';
                        //                                classAttr = hasClass ? ' ' + classAttr : '';
                        //                                var $result = $(
                        //                                    '<div class="row">' +
                        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                        //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                        //                                    '</div>'
                        //                                );
                        //                                return $result;
                        //                                firstEmptySelect = false;
                        //                            }
                        //                        });

                        //                        $("#txtsrcdocdate").val("");
                        //                        $("#AddQuotation").css("display", "block");
                        //                    }
                        //                }
                        //            },
                        //        });
                        //}


                    },
                });



        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }

    }
    if ($("#QuotationNumber").val() == "") {
        var ddlsrc = $("#ddlSourceType").val();
        if (ddlsrc == "Q") {
            EnableForRfq();
        }
        if (ddlsrc == "R") {
            EnableForPR();
            $("#SourceDocumentNumber").attr("disabled", false);
            $("#AddQuotation").css("display", "block");
        }
    }

}
function OnchangeSrcDocNumber() {
    var doc_no = $("#SourceDocumentNumber").val();
    debugger
    if (doc_no != 0) {
        $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "#ced4da");
        $("#vmSourceDocumentNumber").text("");
        $("#vmSourceDocumentNumber").css("display", "none");

        var doc_Date = $("#SourceDocumentNumber option:selected")[0].dataset.date
        var newdate = doc_Date.split("-").reverse().join("-");
        $("#txtsrcdocdate").val(newdate);
        $("#HdnSrcDocNo").val(doc_no);
    }
    else {
        $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "red");
        $("#vmSourceDocumentNumber").text($("#valueReq").text());
        $("#vmSourceDocumentNumber").css("display", "block");
    }
}
function RemoveFirstRowVallidation() {
    var currentrow = $("#PQItmDetailsTbl tbody tr:eq(0)");
    currentrow.find("[aria-labelledby='select2-PQItemListName_1-container']").css("border-color", "#ced4da");
    currentrow.find("#PQItemListNameError").text("");
    currentrow.find("#PQItemListNameError").css("display", "none");
    currentrow.find("#QuotedQuantity").css("border-color", "#ced4da");
    currentrow.find("#Qtd_qty_Error").text("");
    currentrow.find("#Qtd_qty_Error").css("display", "none");
    currentrow.find("#QuotedPrice").css("border-color", "#ced4da");
    currentrow.find("#Qtd_price_Error").text("");
    currentrow.find("#Qtd_price_Error").css("display", "none");
}
function AddAttribute() {
    var doc_no = $("#SourceDocumentNumber").val();
    var Doc_date = $("#txtsrcdocdate").val();
    if (doc_no == "0") {
        ErrorFlag = "Y";
    }
    else {

    }
    var ddlSourceType = $("#ddlSourceType").val();
    //var supp_id = ;
    var QtyDecDigit = $("#QtyDigit").text();
    if (doc_no != "" && doc_no != null && doc_no != "---Select---" && doc_no != "0") {
        try {
            $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "#ced4da");
            $("#vmSourceDocumentNumber").text("");
            $("#vmSourceDocumentNumber").css("display", "none");
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/PurchaseQuotation/AddRfqOrPRItemDetailForQtsn",
                    data: {
                        doc_no: doc_no, Doc_date: Doc_date, Flag: ddlSourceType
                    },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            PO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            RemoveFirstRowVallidation();
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                if ($("#PQItmDetailsTbl >tbody >tr").length > 0) {
                                    for (var i = 1; i < arr.Table.length; i++) {
                                        //var subitmDisable = "";
                                        //if (arr.Table[i].sub_item != "Y") {
                                        //    subitmDisable = "disabled";
                                        //}
                                        var subitem = arr.Table[i].sub_item;
                                        CreateNewRow(subitem, arr.Table[i].sub_item, (i + 1), (i + 1));
                                        BindItemDdlPQ((i + 1));
                                    }
                                }
                                else {
                                    for (var i = 0; i < arr.Table.length; i++) {

                                        CreateNewRow(subitem, arr.Table[i].sub_item, (i + 1), (i + 1));
                                        BindItemDdlPQ((i + 1));
                                    }
                                }
                                var a = 0;
                                $("#PQItmDetailsTbl >tbody >tr").each(function () {
                                    var currentRow = $(this);
                                    debugger
                                    if (a < arr.Table.length) {
                                        var SNo = currentRow.find("#SNohiddenfiled").val();
                                      
                                        currentRow.find("#PQItemListName_" + SNo).append(`<option value="${arr.Table[a].item_id}" selected>${arr.Table[a].item_name}</option>`);//.append(arr.Table[a].item_id).trigger('change');
                                        currentRow.find("#Hdn_Item_ID").val(arr.Table[a].item_id);
                                        currentRow.find("#sub_item").val(arr.Table[a].sub_item);
                                        var subitm = currentRow.find("#sub_item").val();
                                        if (subitm == "Y") {
                                            currentRow.find('#SubItemPRRFQQTQty').attr("disabled", false);
                                        }
                                        else {
                                            currentRow.find('#SubItemPRRFQQTQty').attr('disabled', true);
                                        }
                                        if (arr.Table[a].ItemType === "Service")
                                        {

                                            var Uomid = arr.Table[a].uom_id
                                            var Uom_Name = "---Select---";
                                            if (Uomid == "0" || Uomid == "NULL" || Uomid == "null" || Uomid == "NaN" || Uomid == "") {
                                                currentRow.find("#UOMID").val("0");
                                                currentRow.find("#UOM").html(`<option value="${0}">${Uom_Name}</option>`); /*Modified By NItesh 23-07-2024 For Add Uom Conversion*/
                                            }
                                            else {
                                                currentRow.find("#UOMID").val(arr.Table[a].uom_id);
                                                currentRow.find("#UOM").html(`<option value="${arr.Table[a].uom_id}">${arr.Table[a].uom_alias}</option>`); /*Modified By NItesh 23-07-2024 For Add Uom Conversion*/
                                            }
                                         //   currentRow.find("#UOM").attr("disabled", true);
                                        }
                                        else {
                                           // currentRow.find("#UOM").attr("disabled", false);
                                            currentRow.find("#UOMID").val(arr.Table[a].uom_id);
                                            currentRow.find("#UOM").html(`<option value="${arr.Table[a].uom_id}">${arr.Table[a].uom_alias}</option>`); /*Modified By NItesh 23-07-2024 For Add Uom Conversion*/
                                        }

                                       // currentRow.find("#UOM").val(arr.Table[a].uom_alias); 
                                    
                                        currentRow.find("#QuotedQuantity").val(parseFloat(arr.Table[a].Qty).toFixed(QtyDecDigit)).change();

                                        currentRow.find("#IDItemtype").val(arr.Table[a].ItemType);
                                        currentRow.find("#remarks").val(arr.Table[a].it_remarks);
                                        currentRow.find("#ItemHsnCode").val(arr.Table[a].hsn_code);
                                        var k = a;
                                        var Itm_ID = arr.Table[k].item_id;
                                        var GstApplicable = $("#Hdn_GstApplicable").text();
                                        var docid = $("#DocumentMenuId").val();
                                        if (docid == '105101120') {
                                            if (GstApplicable == "Y") {
                                                $("#HdnTaxOn").val("Item");
                                                Cmn_ApplyGSTToAtable("PQItmDetailsTbl", "Hdn_Item_ID", "SNohiddenfiled", "item_ass_val", arr.Table1);
                                            }
                                            else {
                                                $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                                if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                                    $("#HdnTaxOn").val("Item");
                                                    $("#TaxCalcItemCode").val(Itm_ID);
                                                    $("#HiddenRowSNo").val(SNo);
                                                    $("#Tax_AssessableValue").val(0);
                                                    $("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                                                    $("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                                                    var TaxArr = arr.Table1;
                                                    let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                                    selected = JSON.stringify(selected);
                                                    TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                                    selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                                    selected = JSON.stringify(selected);
                                                    TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                                    if (TaxArr.length > 0) {
                                                        AddTaxByHSNCalculation(TaxArr);
                                                        OnClickSaveAndExit();
                                                        var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                                        Reset_ReOpen_LevelVal(lastLevel);
                                                    }
                                                }

                                            }
                                        }
                                    }

                                    a = a + 1;
                                });
                                $("#PQItmDetailsTbl tbody tr #PQItemListName_1,#QuotedQuantity,#QuotedPrice,#DiscountInPercentage,#DiscountInAmount,#TaxExempted").attr("disabled", false);
                                $("#PQItmDetailsTbl tbody tr select ,#QuotedQuantity").attr("disabled", true);
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (GstApplicable == "Y") {
                                    $("#PQItmDetailsTbl tbody tr select ,#ManualGST").attr("disabled", false);
                                }
                            }
                            debugger
                            if (arr.Table2.length > 0) {
                                for (var y = 0; y < arr.Table2.length; y++) {

                                    var ItmId = arr.Table2[y].item_id;
                                    var SubItmId = arr.Table2[y].sub_item_id;
                                    var SubItmName = arr.Table2[y].sub_item_name;
                                    var Qty = arr.Table2[y].Qty;
                                    //if (ddlSourceType == "R") {
                                    //     var Qty = arr.Table1[y].pr_qty;
                                    // }
                                    // else {
                                    //     var Qty = arr.Table1[y].rfq_qty;
                                    // }


                                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${Qty}'></td>
                            
                            </tr>`);
                                }

                            }
                        }
                        $("#AddQuotation").css("display", "none");
                        $("#SourceDocumentNumber").attr("disabled", true);
                        $("#ddlSourceType").attr("disabled", true);
                        //if (ddlSourceType=="Q") {
                        $("#SupplierNameList").attr("disabled", true);
                        $("#Prospect").attr("disabled", true);
                        $("#Supplier").attr("disabled", true);
                        $("#Prosbtn").css("display", "none");
                        //}
                        $("#HdnSourceType").val($("#ddlSourceType").val());
                        $("#PQItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            var Sno = currentRow.find("#SNohiddenfiled").val();
                            //currentRow.find("#delBtnIcon").css("display", "none");
                            currentRow.find("#PQItemListName_" + Sno).prop("disabled", true);
                        });
                    },
                });

        } catch (err) {
            console.log(PFName + " Error : " + err.message);
        }
    }
    else {

        $("[aria-labelledby='select2-SourceDocumentNumber-container']").css("border-color", "red");
        $("#vmSourceDocumentNumber").text($("#valueReq").text());
        $("#vmSourceDocumentNumber").css("display", "block");
    }

}
function OnChangePQItemName(e) {
    debugger
    var currentrow = $(e.target).closest('tr');
    var row_id = currentrow.find("#SNohiddenfiled").val();
    var Item_id = currentrow.find("#PQItemListName_" + row_id).val();
    var ItemIdToRemove = currentrow.find("#Hdn_Item_ID").val();
    currentrow.find("#Hdn_Item_ID").val(Item_id);
    
    if (Item_id == "0") {
        currentrow.find("[aria-labelledby='select2-PQItemListName_" + row_id + "-container']").css("border-color", "red");
        currentrow.find("#PQItemListNameError").text($("#valueReq").text());
        currentrow.find("#PQItemListNameError").css("display", "block");
    }
    else {
        currentrow.find("[aria-labelledby='select2-PQItemListName_" + row_id + "-container']").css("border-color", "#ced4da");
        currentrow.find("#PQItemListNameError").text("");
        currentrow.find("#PQItemListNameError").css("display", "none");
        $("#ddlSourceType").attr("disabled", true);

        currentrow.find("#QuotedQuantity").val(""/*parseFloat(0).toFixed($("#QtyDigit").text())*/)
        currentrow.find("#QuotedPrice").val(""/*parseFloat(0).toFixed($("#RateDigit").text())*/)
        currentrow.find("#NetRate").val(""/*parseFloat(0).toFixed($("#RateDigit").text())*/)
        currentrow.find("#item_ass_val").val(""/*parseFloat(0).toFixed($("#RateDigit").text())*/)
        currentrow.find("#NetValue").val(""/*parseFloat(0).toFixed($("#RateDigit").text())*/)
        currentrow.find("#DiscountInPercentage").val(""/*parseFloat(0).toFixed($("#ValDigit").text())*/)
        //if ($("#ddlSourceType").val() == "Q") {
        $("#SupplierNameList").attr("disabled", true);
        $("#Prospect").attr("disabled", true);
        $("#Supplier").attr("disabled", true);
        //}

        $("#Prosbtn").css("display", "none");
        $("#HdnSourceType").val($("#ddlSourceType").val());
    }
    //bindTaxExempted(e);
    //  HideSelectedItem("#PQItemListName_", row_id, "#PQItmDetailsTbl", "#SNohiddenfiled");
   
    try {
       // Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");
        Cmn_BindUOM(currentrow, Item_id, "", "", "pur");
        debugger
        //GetUomNameItemWise(Itm_ID, "UOM", "UOMID", clickedrow);
    } catch (err) {
    }
    if (ItemIdToRemove !== Item_id) {
        DelDeliSchAfterDelItem(ItemIdToRemove);
    }

}
//function bindTaxExempted(e) {
//    debugger;
//    var currantRow = $(e.target).closest('tr')
//    var row_id = currentrow.find("#SNohiddenfiled").val();
//    var Item_id = currentrow.find("#PQItemListName_" + row_id).val();
//    $.ajax(
//        {
//            type: "POST",
//            url: "/ApplicationLayer/PurchaseQuotation/GetTaxExemptedItem",
//            data: {
//                Item_id: Item_id
//            },
//            success: function (data) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    LSO_ErrorPage();
//                    return false;
//                }
//                if (data != null && data !== "") {
//                    var arr = [];
//                    arr = JSON.push(data);
//                    if (arr.Table.length > 0) {
//                        clickedrow.find("#ItemAvlbStock").val(arr.Table[0].item_avl_stk_bs);
//                    }
//                    else {
//                        clickedrow.find("#ItemAvlbStock").val("");
//                    }
//                }
//            }
//        }
//    )
//}
/*------------------------------------------UOM Conversion End-------------------------------------------*/

async function onChangeUom(e) {
    debugger;
    let Crow = $(e.target).closest('tr');
    var QtyDecDigit = $("#QtyDigit").text();

    let Sno = Crow.find("#SNohiddenfiled").val();
    let ItemId = Crow.find("#PQItemListName_" + Sno).val();
    let UomId = Crow.find("#UOM").val();
    let ItemType = Crow.find("#hdnItemtype").val();
    Crow.find("#UOMID").val(UomId);
    //await Cmn_StockUomWise(ItemId, UomId).then((res) => {
    //    Crow.find("#AvailableStockInBase").val(res);
    //    

    //    //if (ItemType == "Consumable") {
    //    //    Crow.find("#AvailableStockInBase").val(parseFloat(0).toFixed(QtyDecDigit));
    //    //    Crow.find("#SimpleIssue").attr("disabled", true);
    //    //    Crow.find("#MRSNumber").attr("disabled", true);
    //    //}

    //}).catch(err => console.log(err.message));
}
function OnChangeQTItemDiscountPerc(e) {
    var currentrow = $(e.target).closest('tr');
    var discPer = currentrow.find("#DiscountInPercentage").val();
    if (AvoidDot(discPer) == false) {
        discPer = "";
        currentrow.find("#DiscountInPercentage").val("")
    }
    else {
        currentrow.find("#DiscountInPercentage").val(parseFloat(discPer).toFixed(2))
    }
    if (discPer != "") {
        currentrow.find("#DiscountInAmount").attr("disabled", true);
    }
    else {
        currentrow.find("#DiscountInAmount").attr("disabled", false);
    }
    calculationByRow(e);
}
function OnChangeQTItemDiscountAmt(e) {
    var RateDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var discAmt = currentrow.find("#DiscountInAmount").val();
    var QuotedPrice = currentrow.find("#QuotedPrice").val();
    if (AvoidDot(QuotedPrice) == false) {
        QuotedPrice = 0;
    }
    if (AvoidDot(discAmt) == false) {
        discAmt = "";
        currentrow.find("#DiscountInAmount").val("")
    }
    else {
        if (parseFloat(QuotedPrice) > parseFloat(discAmt)) {
            currentrow.find("#DiscountInAmount").val(parseFloat(discAmt).toFixed(RateDigit))
            currentrow.find("#DiscountInAmount").css("border-color", "#ced4da");
            currentrow.find("#DiscAmt_price_Error").text("");
            currentrow.find("#DiscAmt_price_Error").css("display", "none");
        }
        else {
            currentrow.find("#DiscountInAmount").css("border-color", "red");
            currentrow.find("#DiscAmt_price_Error").text($("#GDAmtTORate").text());
            currentrow.find("#DiscAmt_price_Error").css("display", "block");
        }

    }
    if (discAmt != "") {
        currentrow.find("#DiscountInPercentage").attr("disabled", true);
    }
    else {
        currentrow.find("#DiscountInPercentage").attr("disabled", false);
    }
    calculationByRow(e);
}
function AmountFloatRate(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
}
function AmountFloatVal(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#QuotedPrice").val();
    item_rate = CheckNullNumber(item_rate);

    var selectedval = Cmn_SelectedTextInTextField(evt);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }

    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;

        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    clickedrow.find("#item_disc_amtError").css("display", "none");
    clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
    return true;
}
function AmountFloatQty(el, evt) {
    debugger
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
}
function AmountFloatPer(el, evt) {
    debugger
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
}
function OnchangeQuotedQuantity(e) {
    var QtyDigit = $("#QtyDigit").text();

    var currentrow = $(e.target).closest('tr');
    var ItemCode = currentrow.find("#Hdn_Item_ID").val();
    var QuotedQuantity = currentrow.find("#QuotedQuantity").val();
    if (AvoidDot(QuotedQuantity) == false) {
        currentrow.find("#QuotedQuantity").val("");
    }
    else {
        currentrow.find("#QuotedQuantity").val(parseFloat(QuotedQuantity).toFixed(QtyDigit));
    }
    calculationByRow(e);
    checkitemvallidation(currentrow, "QuotedQuantity", "Qtd_qty_Error");

    //BindDelivertSchItemList(); /*Commented by Suraj On 13-12-2022 for Deliting delivery schedule item when Quoted Qty changes*/
    DelDeliSchAfterDelItem(ItemCode);
}

function OnchangeQuotedPrice(e) {
    var RateDigit = $("#RateDigit").text();
    var currentrow = $(e.target).closest('tr');
    var QuotedPrice = currentrow.find("#QuotedPrice").val();
    if (AvoidDot(QuotedPrice) == false) {
        currentrow.find("#QuotedPrice").val("");
    }
    else {
        currentrow.find("#QuotedPrice").val(parseFloat(QuotedPrice).toFixed(RateDigit));
    }
    calculationByRow(e);
    checkitemvallidation(currentrow, "QuotedPrice", "Qtd_price_Error");
    BindDelivertSchItemList();
}
function OnchangeAssessableValue(e) {
    var RateDigit = $("#RateDigit").text();

    var currentrow = $(e.target).closest('tr');
    var QuotedQuantity = currentrow.find("#item_ass_val").val();
    if (AvoidDot(QuotedQuantity) == false) {
        currentrow.find("#item_ass_val").val("");
    }
    else {
        currentrow.find("#item_ass_val").val(parseFloat(QuotedQuantity).toFixed(RateDigit));
    }
    calculationByRow(e);
}
//-------------------------Vallidation End----------------------------------------//
//------------------------Save Data Functions-------------------------------------//

function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition By NItesh 08-01-2024 for Disable Save Button**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    if (HeaderValidations() == false) {
        return false;
    }
    if (ItemsVallidation() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (Cmn_taxVallidation("PQItmDetailsTbl", "item_tax_amt", "PQItemListName_", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
            return false;
        }
    }
    if (Cmn_CheckDeliverySchdlValidations("PQItmDetailsTbl", "Hdn_Item_ID", "PQItemListName_", "QuotedQuantity", "SNohiddenfiled") == false) {
        return false;
    }
    $("#Prospect").attr("disabled", false);
    $("#Supplier").attr("disabled", false);
    $("#SupplierNameList").attr("disabled", false);
    var FinalItemDetail = [];
    var FinalItemTaxDetail = [];
    var FinalItemOCDetail = [];
    var FinalItemDelSchDetail = [];
    var FinalItemTermsDetail = [];
    debugger
    FinalItemDetail = InsertPQItemDetails();
    /*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    /*-----------Sub-item end-------------*/
    FinalItemTaxDetail = InsertPQItemTaxDetails();
    FinalItemOCDetail = InsertPQItemOCDetails();
    FinalItemDelSchDetail = InsertPQItemDelSchDetails();
    FinalItemTermsDetail = InsertPQItemTermsDetails();

    debugger;
    var ItemDt = JSON.stringify(FinalItemDetail);
    var ItemTaxDt = JSON.stringify(FinalItemTaxDetail);
    var ItemOCDt = JSON.stringify(FinalItemOCDetail);
    var ItemDelSchDt = JSON.stringify(FinalItemDelSchDetail);
    var ItemTermsDt = JSON.stringify(FinalItemTermsDetail);

    $('#hdItemDetailList').val(ItemDt);
    $('#hdTaxDetailList').val(ItemTaxDt);
    $('#hdOCDetailList').val(ItemOCDt);
    $('#hdDelSchDetailList').val(ItemDelSchDt);
    $('#hdTermsDetailList').val(ItemTermsDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;

}
function InsertPQItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#PQItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#PQItemListName_" + rowid).val();
        ItemList.ItemName = currentRow.find("#PQItemListName_" + rowid + " option:selected").text();
        ItemList.subitem = currentRow.find("#sub_item").val();
        var Item_type = currentRow.find("#IDItemtype").val();
        var UOM_ID  = currentRow.find("#UOMID").val();
        if (Item_type == "Service") {
            if (UOM_ID != null && UOM_ID != "" && UOM_ID != "0" && UOM_ID != "NaN") {
                ItemList.UOMID  = UOM_ID;
            }
            else {
                 ItemList.UOMID  = "0";
            }
        }
        else {
            ItemList.UOMID = UOM_ID;
        }
      
        ItemList.UOMName = currentRow.find("#UOM").val();
        ItemList.qt_qty = currentRow.find("#QuotedQuantity").val();
        ItemList.item_rate = currentRow.find("#QuotedPrice").val();
        ItemList.item_disc_perc = currentRow.find("#DiscountInPercentage").val();
        ItemList.item_disc_amt = currentRow.find("#DiscountInAmount").val();
        ItemList.item_disc_val = currentRow.find("#DiscountedVal").val();
        ItemList.net_rate = currentRow.find("#NetRate").val();
        ItemList.item_gr_val = currentRow.find("#item_ass_val").val();
        ItemList.item_ass_val = currentRow.find("#item_ass_val").val();
        ItemList.item_tax_amt = currentRow.find("#item_tax_amt").val();
        ItemList.item_oc_amt = currentRow.find("#OtherCharges").val();
        ItemList.item_net_val_spec = currentRow.find("#NetValue").val();
        ItemList.item_net_val_bs = currentRow.find("#NetValue").val();
        ItemList.ItemRemarks = currentRow.find('#remarks').val();
        ItemList.hsn_code = currentRow.find('#ItemHsnCode').val();
        if (currentRow.find("#TaxExempted").is(":checked")) {
            ItemList.TaxExempted = "Y"
        }
        else {
            ItemList.TaxExempted = "N"
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ItemList.ManualGST = "Y"
            }
            else {
                ItemList.ManualGST = "N"
            }
        }
        else {
            ItemList.ManualGST = "N"
        }
        //ItemList.TaxExempted = currentRow.find('#TaxExempted').val();

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};
function InsertPQItemTaxDetails() {
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var ItemTaxDetailList = new Array();
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#PQItemListName_" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var ItemList = {};
                        var Crow = $(this);
                        ItemList.ItemID = Crow.find("#TaxItmCode").text().trim();
                        ItemList.TaxID = Crow.find("#TaxNameID").text().trim();
                        ItemList.TaxName = Crow.find("#TaxName").text().trim();
                        ItemList.TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        ItemList.TaxLevel = Crow.find("#TaxLevel").text().trim();
                        ItemList.TaxValue = Crow.find("#TaxAmount").text().trim();
                        ItemList.TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        ItemList.taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                        ItemList.TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
                        ItemTaxDetailList.push(ItemList);
                        //POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn });
                    });
                }
            }
        }
    });

    //var ItemTaxDetailList = new Array();  Commented by Suraj for
    //var FTaxDetails = JSON.parse(sessionStorage.getItem("TaxCalcDetails"));  
    //if (FTaxDetails != null) {
    //    if (FTaxDetails.length > 0) {
    //        for (i = 0; i < FTaxDetails.length; i++) {
    //            var ItemList = {};
    //            ItemList.ItemID = FTaxDetails[i].TaxItmCode;
    //            ItemList.TaxID = FTaxDetails[i].TaxNameID;
    //            ItemList.TaxRate = FTaxDetails[i].TaxPercentage.replace('%', '');
    //            ItemList.TaxLevel = FTaxDetails[i].TaxLevel;
    //            ItemList.TaxValue = FTaxDetails[i].TaxAmount;
    //            ItemList.TaxApplyOn = FTaxDetails[i].TaxApplyOnID;    
    //            ItemTaxDetailList.push(ItemList);
    //        }
    //    }
    //}
    return ItemTaxDetailList;
}
function InsertPQItemOCDetails() {
    debugger;
    var ItemOCDetailList = new Array();

    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemList = {};
            var OC_ID = "";
            var OCValue = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OCValue = currentRow.find("#OCAmount").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OC_TaxAmt = currentRow.find("#OCTaxAmt").text();
            OC_TotlAmt = currentRow.find("#OCTotalTaxAmt").text();
            ItemList.OC_ID = OC_ID;
            ItemList.OCName = OCName;
            ItemList.OC_Curr = OC_Curr;
            ItemList.OC_Conv = OC_Conv;
            ItemList.OCValue = OCValue;
            ItemList.OC_AmtBs = OC_AmtBs;
            ItemList.OC_TaxAmt = OC_TaxAmt;
            ItemList.OC_TotlAmt = OC_TotlAmt;
            ItemOCDetailList.push(ItemList);
        });
    }
    return ItemOCDetailList;

}
function InsertPQItemDelSchDetails() {
    var ItemDelSchDetailList = new Array();

    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            debugger
            var currentRow = $(this);
            var ItemList = {};
            ItemList.item_id = currentRow.find("#Hd_ItemIdFrDS").text();
            ItemList.ItemName = currentRow.find("#Hd_ItemNameFrDS").text();
            ItemList.sch_date = currentRow.find("#sch_date").text()
            ItemList.delv_qty = currentRow.find("#delv_qty").text();
            ItemDelSchDetailList.push(ItemList);
        });
    }
    return ItemDelSchDetailList;

}
function InsertPQItemTermsDetails() {
    var ItemTermsDetailList = new Array();

    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemList = {};
            ItemList.term_desc = currentRow.find("#term_desc").text();
            ItemTermsDetailList.push(ItemList);
        });
    }
    return ItemTermsDetailList;

}



//------------------------Save Data Functions End-------------------------------------//
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
            $("#hdnAction").val("Delete");
            //location.href = "/ApplicationLayer/PurchaseQuotation/PQDetailsActionCommands";
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

function OnClickIconBtn(e) {
    var row_id = $(e.target).closest('tr').find("#SNohiddenfiled").val();
    var ItmCode = $(e.target).closest('tr').find("#PQItemListName_" + row_id).val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickHistoryIconBtn(e) {
    // debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#SupplierNameList").val();
    ItmCode = clickedrow.find("#PQItemListName_" + Sno).val();
    ItmName = clickedrow.find("#PQItemListName_" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM option:selected").text();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
function calculationByRow(e) {

    var ValDigit = $("#ValDigit").text();
    debugger;
    var RateDigit = $("#RateDigit").text();
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#PQItemListName_" + RowSNo).val();

    var QuotedQuantity = currentrow.find("#QuotedQuantity").val();
    var QuotedPrice = currentrow.find("#QuotedPrice").val();
    var DiscountInPercentage = currentrow.find("#DiscountInPercentage").val();
    var DiscountInAmount = currentrow.find("#DiscountInAmount").val();
    var item_tax_amt = currentrow.find("#item_tax_amt").val();
    var OtherCharges = currentrow.find("#OtherCharges").val();

    if (AvoidDot(QuotedQuantity) == false) {
        QuotedQuantity = 0;
    }
    if (AvoidDot(QuotedPrice) == false) {
        QuotedPrice = 0;
    }
    if (AvoidDot(DiscountInPercentage) == false) {
        DiscountInPercentage = 0;
    }
    if (AvoidDot(DiscountInAmount) == false) {
        DiscountInAmount = 0;// parseFloat()
    }
    if (AvoidDot(item_tax_amt) == false) {
        item_tax_amt = 0;
    }
    if (AvoidDot(OtherCharges) == false) {
        OtherCharges = 0;
    }
    var discAmount = 0;
    var netRate = 0;
    if (parseFloat(QuotedPrice) != 0) {
        if (parseFloat(DiscountInPercentage) != 0) {
            discAmount = (parseFloat(QuotedPrice) * parseFloat(DiscountInPercentage)) / 100;
            netRate = parseFloat(QuotedPrice) - parseFloat(discAmount);
            currentrow.find("#NetRate").val(parseFloat(netRate).toFixed(RateDigit));

        }
        else if (parseFloat(DiscountInAmount) != 0) {
            discAmount = DiscountInAmount;
            netRate = parseFloat(QuotedPrice) - parseFloat(DiscountInAmount);
            currentrow.find("#NetRate").val(parseFloat(netRate).toFixed(RateDigit));
        }
        else {
            netRate = QuotedPrice;
            currentrow.find("#NetRate").val(parseFloat(netRate).toFixed(RateDigit));
        }
    }
    var netValue = 0;
    if (parseFloat(QuotedQuantity) != 0) {
        if (parseFloat(netRate) != 0) {
            netValue = parseFloat(netRate) * parseFloat(QuotedQuantity);
            currentrow.find("#item_ass_val").val(parseFloat(netValue).toFixed(ValDigit));//

            if (parseFloat(item_tax_amt) != 0) {
                netValue = parseFloat(netValue) + parseFloat(item_tax_amt);
            }
            if (parseFloat(OtherCharges) != 0) {
                netValue = parseFloat(netValue) + parseFloat(OtherCharges);
            }
            currentrow.find("#NetValue").val(parseFloat(netValue).toFixed(ValDigit));

        }
        else {
            currentrow.find("#item_ass_val").val(parseFloat(netValue).toFixed(ValDigit));
            currentrow.find("#NetValue").val(parseFloat(netValue).toFixed(ValDigit));
        }
        var discountedval = parseFloat(QuotedQuantity) * parseFloat(discAmount);

        currentrow.find("#DiscountedVal").val(parseFloat(discountedval).toFixed(ValDigit));
    }
    else {
        currentrow.find("#item_ass_val").val(parseFloat(netValue).toFixed(ValDigit));
        currentrow.find("#NetValue").val(parseFloat(netValue).toFixed(ValDigit));

    }
    if (netValue > 0) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    CalculateGrossValue();
    //removeTaxItemWise("");
    var AssAmount = currentrow.find("#item_ass_val").val();
    var TaxBySlab = $("#taxTemplate").text();
    var SlabItem = $("#Tax_ItemID").val()
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (TaxBySlab == "GST Slab" && ItmCode == SlabItem) {
            if (currentrow.find("#TaxExempted").is(":checked")) {
                CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
                currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
            }
            else {
                CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
            }
        }
        else {
            if (currentrow.find("#TaxExempted").is(":checked") || currentrow.find("#ManualGST").is(":checked")) {
                CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
                currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
            }
            else {
                CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
            }
        }
    }
    else {
        if (currentrow.find("#TaxExempted").is(":checked")) {
            CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
            currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        }
        else {
            CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        }
    }
    if (AvoidDot($("#PQ_OtherCharges").val()) != false) {
        Calculate_OC_AmountItemWise($("#PQ_OtherCharges").val());
    }
    else {
        Calculate_OC_AmountItemWise(0);
    }
}
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#PQItemListName_" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    $("#Tax_AssessableValue").val(AssAmount);
    $("#TaxCalcItemCode").val(ItmCode);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        calculationByRow(e)
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        debugger;
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            //if ($("#taxTemplate").text() == "GST Slab") {
            var gst_number = $("#ship_add_gstNo").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "PQItmDetailsTbl", "Hdn_Item_ID", "SNohiddenfiled", "item_ass_val")
            //calculationByRow(e)
            //CalculateGrossValue();
            //}
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
            //calculationByRow(e)
            //CalculateGrossValue();
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#PQItemListName_" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount);
        currentrow.find("#item_tax_amt").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        calculationByRow(e)
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "PQItmDetailsTbl", "Hdn_Item_ID", "SNohiddenfiled", "item_ass_val")
        calculationByRow(e)
        CalculateGrossValue();
        $("#taxTemplate").text("Template");
    }
}
function CalculateGrossValue() {
    var GrossValue = 0;
    var NetAmount = 0;
    var TotalTaxAmt = 0;
    var TotalDiscountedVal = 0;
    var ValDigit = $("#ValDigit").text();
    $("#PQItmDetailsTbl tbody tr").each(function () {
        var currentrow = $(this);
        //debugger;
        var AssVal = currentrow.find("#item_ass_val").val();
        var NetValue = currentrow.find("#NetValue").val();
        var TaxAmt = currentrow.find("#item_tax_amt").val();//
        var DiscountedVal = currentrow.find("#DiscountedVal").val();
        if (AvoidDot(AssVal) == false) {
            AssVal = 0;
        }
        if (AvoidDot(NetValue) == false) {
            NetValue = 0;
        }

        if (AvoidDot(TaxAmt) == false) {
            TaxAmt = 0;
        }
        if (AvoidDot(DiscountedVal) == false) {
            DiscountedVal = 0;
        }
        TotalTaxAmt = parseFloat(TotalTaxAmt) + parseFloat(TaxAmt);
        GrossValue = parseFloat(GrossValue) + parseFloat(AssVal);
        NetAmount = parseFloat(NetAmount) + parseFloat(NetValue);
        TotalDiscountedVal = parseFloat(TotalDiscountedVal) + parseFloat(DiscountedVal);
    })
    $("#hdDiscAmt").val(parseFloat(TotalDiscountedVal).toFixed(ValDigit));
    $("#TxtGrossValue").val(parseFloat(GrossValue).toFixed(ValDigit));
    $("#NetAmount").val(parseFloat(NetAmount).toFixed(ValDigit));
    $("#TxtTaxAmount").val(parseFloat(TotalTaxAmt).toFixed(ValDigit));
}

//------------------------------ Other charge---------------------------------//

function OnClickSaveAndExit_OC_Btn() {
    //debugger;

    CMNOnClickSaveAndExit_OC_Btn('#PQ_OtherCharges', "#NetAmount", "#NetAmount");
    //--- for this function we need to create to functions Calculate_OC_AmountItemWise(TotalOCAmt) and BindOtherChargeDeatils()
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var TotalGrossVal = $("#TxtGrossValue").val();
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        var currentrow = $(this);
        var assVal = currentrow.find("#item_ass_val").val();
        if (parseFloat(TotalGrossVal) > 0 && parseFloat(assVal) > 0 && parseFloat(TotalOCAmt) > 0) {
            var OCPercentage = ((parseFloat(assVal) / parseFloat(TotalGrossVal)) * 100);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentrow.find("#OtherCharges").val(parseFloat(OCAmtItemWise).toFixed(ValDigit));
            }
            else {
                currentrow.find("#OtherCharges").val(""/*parseFloat(0).toFixed(ValDigit)*/);
            }
        }
        else {
            currentrow.find("#OtherCharges").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(RateDigit);
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var OCValue = currentRow.find("#OtherCharges").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    debugger
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        $("#PQItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#OtherCharges").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#OtherCharges").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#PQItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#OtherCharges").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                var Aftdiffamt = parseFloat(OCValue) + parseFloat(AmtDifference);
                currentRow.find("#OtherCharges").val(parseFloat((Aftdiffamt)).toFixed(ValDigit));
            }
        });
    }
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        var currentrow = $(this);
        var OC_amt = currentrow.find("#OtherCharges").val();
        var item_tax_amt = currentrow.find("#item_tax_amt").val();
        var item_ass_val = currentrow.find("#item_ass_val").val();
        var NetValue = 0;
        if (AvoidDot(OC_amt) == false) {
            OC_amt = 0;
        }
        if (AvoidDot(item_tax_amt) == false) {
            item_tax_amt = 0;
        }
        if (AvoidDot(item_ass_val) == false) {
            item_ass_val = 0;
        }
        NetValue = parseFloat(item_tax_amt) + parseFloat(OC_amt) + parseFloat(item_ass_val);
        currentrow.find("#NetValue").val(parseFloat(NetValue).toFixed(ValDigit));
    })
    CalculateGrossValue();
};
function SetOtherChargeVal() {

}
function BindOtherChargeDeatils() {
    debugger;
    var DecDigit = $("#ValDigit").text();//Tbl_OC_Deatils
    if ($("#PQItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(""/*parseFloat(0).toFixed(DecDigit)*/);
        $("#PQ_OtherCharges").val(""/*parseFloat(0).toFixed(DecDigit)*/);
    }

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#_OtherChargeTotal").text(""/*parseFloat(0).toFixed($("#ValDigit").text())*/);
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        var TotalAMount = parseFloat(0).toFixed(DecDigit);
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td id=othrChrg_Name>${currentRow.find("#OCName").text()}</td>
<td class="num_right">${currentRow.find("#OCAmount").text()}</td>
</tr>`);
            $("#Tbl_OC_Deatils #Total_OC_Amount").text(TotalAMount);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
        });
        $("#_OtherChargeTotal").text(TotalAMount);
    }
}

//-------------------------Tax -----------------------------------------//

function OnClickTaxCalculationBtn(e) {
    debugger;
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    /* Modifyed By Shubham Maurya on 07 - 10 - 2023 : 1423 for Purchase Qutation Tax */
    var currentRow = $(e.target).closest("tr");
    if (currentRow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("GST Slab")
    }
    else {
        $("#taxTemplate").text("Template")
    }
    var RowNo = currentRow.find("#SNohiddenfiled").val();
    var Itm_ID = currentRow.find("#PQItemListName_" + RowNo).val();
    var item_gross_val = currentRow.find("#item_ass_val").val();
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    /*--------------------------------end-----------------------------*/
    CMNOnClickTaxCalculationBtnModel(e, "#SNohiddenfiled", "#PQItemListName_");
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
            $("#Tax_Template").attr('disabled', true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            var clickedrow = $(e.target).closest("tr");
            if (clickedrow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr('disabled', false);
                $("#SaveAndExitBtn").css("display", "Block");
            }
            else {
                $("#taxTemplate").text("Template")
                $("#Tax_Template").attr('disabled', true);
                $("#SaveAndExitBtn").css("display", "none");
            }
        }
    }
}
function OnClickSaveAndExit() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var tbllenght = $("#TaxCalculatorTbl tbody tr").length
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
    debugger;
    let NewArr = [];
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    if (FTaxDetails > 0) {
        $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
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
            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
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
            /* Modifyed By Shubham Maurya on 07 - 10 - 2023 : 1423 for Purchase Qutation Tax add <td id="DocNo"></td><td id="DocDate"></td> */

            $("#Hdn_TaxCalculatorTbl > tbody").append(`
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
            //TotalTaxAmount = parseFloat(TotalTaxAmount)+parseFloat(TaxAmount);
            NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //sessionStorage.removeItem("TaxCalcDetails");
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(NewArr));
        BindTaxAmountDeatils(NewArr);
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
            /* Modifyed By Shubham Maurya on 07 - 10 - 2023 : 1423 for Purchase Qutation Tax add <td id="DocNo"></td><td id="DocDate"></td> */
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
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
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
        BindTaxAmountDeatils(TaxCalculationList);
    }
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var RowNo = currentRow.find("#SNohiddenfiled").val();
        var Item_ID = currentRow.find("#PQItemListName_" + RowNo).val();
        if (Item_ID == TaxItmCode) {
            if (AvoidDot(TotalTaxAmount) == false) {
                TotalTaxAmount = 0;
            }
            TaxAmt = (parseFloat(TotalTaxAmount));
            if (currentRow.find("#TaxExempted").is(":checked")) {
                TaxAmt = parseFloat(0).toFixed(DecDigit)
            }
            else {
                currentRow.find("#item_tax_amt").val(parseFloat(TaxAmt).toFixed(DecDigit));
            }
            var OC_Amt = currentRow.find("#OtherCharges").val();
            if (AvoidDot(OC_Amt) == false) {
                OC_Amt = 0;
            }
            AssessableValue = currentRow.find("#item_ass_val").val();
            if (AvoidDot(AssessableValue) == false) {
                AssessableValue = 0;
            }
            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
            currentRow.find("#NetValue").val(NetOrderValueSpec);
            var TaxAmt1 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        }
    });
    CalculateGrossValue();
}
function OnClickReplicateOnAllItems() {
    var errorflag = "N";
    var DecDigit = $("#ValDigit").text();
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
    debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#Hdn_TaxCalculatorTbl > tbody > tr").remove();
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
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
            $("#PQItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemCode;
                var AssessVal;
                //debugger;
                ItemCode = currentRow.find("#PQItemListName_" + Sno).val();
                AssessVal = currentRow.find("#item_ass_val").val();
                //if (CheckPOItemValidations() == false) {
                //    errorflag = "Y"
                //}
                var AssessValcheck = parseFloat(AssessVal);
                if (AssessVal != "" && AssessValcheck > 0) {
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
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }
                }

            });
        }
    }
    if (TaxCalculationListFinalList.length > 0) {
        // sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            /* Modifyed By Shubham Maurya on 07 - 10 - 2023 : 1423 for Purchase Qutation Tax add <td id="DocNo"></td><td id="DocDate"></td> */
            $("#Hdn_TaxCalculatorTbl > tbody").append(`
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

        BindTaxAmountDeatils(TaxCalculationListFinalList);
    } else {
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    $("#PQItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var hfItemID = currentRow.find("#PQItemListName_" + Sno).val();
        if (TaxCalculationListFinalList != null) {
            if (TaxCalculationListFinalList.length > 0) {
                for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                    var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                    //var RowSNoF = TaxCalculationListFinalList[i].RowSNo;
                    var txtTaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;

                    if (hfItemID == txtTaxItmCode) {

                        //TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                        var OC_Amt = parseFloat(0);
                        if (currentRow.find("#OtherCharges").val() != null && currentRow.find("#OtherCharges").val() != "") {
                            OC_Amt = parseFloat(currentRow.find("#OtherCharges").val());
                        }
                        AssessableValue = parseFloat(currentRow.find("#item_ass_val").val());
                        if (AvoidDot(AssessableValue) == false) {
                            AssessableValue = 0;
                        }
                        NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        //  NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                        currentRow.find("#NetValue").val(NetOrderValueSpec);
                        //currentRow.find("#item_net_val_bs").val(NetOrderValueBase);                        
                    }
                }
            }
            else {
                var GrossAmtOR = parseFloat(0).toFixed(DecDigit);
                if (AvoidDot(currentRow.find("#item_ass_val").val()) == false) {
                }
                else {
                    GrossAmtOR = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(DecDigit);
                }
                currentRow.find("#item_tax_amt").val(TaxAmt);
                var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#OtherCharges").val() != null && currentRow.find("#OtherCharges").val() != "") {
                    OC_Amt_OR = parseFloat(currentRow.find("#OtherCharges").val()).toFixed(DecDigit);
                }
                var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                currentRow.find("#item_ass_val").val(GrossAmtOR);
                currentRow.find("#NetValue").val(FGrossAmtOR);
                //currentRow.find("#item_net_val_bs").val(FGrossAmtOR);

            }
        }
    });
    CalculateGrossValue();
}
function CalculateAmount() {

}
function BindTaxAmountDeatils(TaxAmtDetail) {
    //debugger;

    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatils(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);

}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                var currentRow = $(this);

                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
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

                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);

                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#PQItmDetailsTbl > tbody > tr #Hdn_Item_ID[value=" + ItmCode + "]").closest("tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo = currentRow.find("#PQItemListName_" + Sno).val();
                if (ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
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
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                currentRow.find("#item_net_val_bs").val(NetOrderValueBase);
                            }
                        });

                    }
                    else {
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(DecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        currentRow.find("#item_net_val_bs").val(FGrossAmtOR);
                    }
                }
            });
            //CalculateAmount();
            CalculateGrossValue();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function AfterDeleteResetPQ_ItemTaxDetail() {
    //debugger;
    var PoItmDetailsTbl = "#PQItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var POItemListName = "#PQItemListName_";
    //CMNAfterDeleteReset_ItemTaxDetail(PoItmDetailsTbl, SNohiddenfiled, POItemListName); /*Commented by Suraj On 13-12-2022 for change to Common method*/
    CMNAfterDeleteReset_ItemTaxDetailModel(PoItmDetailsTbl, SNohiddenfiled, POItemListName);
}

//-------------------------- Tax End-------------------//

//----------------------------Delevery Schedule----------------------------------------//
function DelDeliSchAfterDelItem(ItemID) {

    Cmn_DelDeliSchAfterDelItem(ItemID, "PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity");
    //$("#DeliverySchTble >tbody >tr").each(function () {
    //    debugger;
    //    var currentRow = $(this);
    //    var DelSchItem = currentRow.find("td:eq(2)").text();        
    //    if (ItemID == DelSchItem) {
    //        currentRow.remove();
    //    }
    //});
    //$("#DeliverySchQty").val("");    
    //$("#DeliveryDate").val("");
    //BindDelivertSchItemList();
}
function DeleteDeliverySch() {
    Cmn_DeleteDeliverySch();
    //$('#DeliverySchTble tbody').on('click', '.deleteIcon', function () {
    //    var child = $(this).closest('tr').nextAll();
    //    child.each(function () {
    //        var id = $(this).attr('id');
    //        var idx = $(this).children('.row-index').children('p');
    //        var dig = parseInt(id.substring(1));
    //        idx.html(`Row ${dig - 1}`);
    //        $(this).attr('id', `R${dig - 1}`);
    //    });
    //    $(this).closest('tr').remove();        
    //    BindDelivertSchItemList();
    //    ResetSrNoDelSch();
    //    $("#DeliveryDate").val("");
    //    $("#DeliverySchQty").val("");
    //});
}
//function ResetSrNoDelSch() {
//    var SrNo = 0;
//    $('#DeliverySchTble tbody tr').each(function () {
//        var currentRow = $(this);
//        SrNo = SrNo + 1;
//        currentRow.find("td:eq(1)").text(SrNo);

//    });
//}
function OnChangeDelSchItm() {
    debugger;
    Cmn_OnChangeDelSchItm("PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity");
    //var QtyDecDigit = $("#QtyDigit").text();
    //if ($('#DeliverySchItemDDL').val() == "0") {
    //    $('#DeliverySchItemDDL').css("border-color", "red");
    //    $('#SpanDelSchItemName').text($("#valueReq").text());
    //    $("#SpanDelSchItemName").css("display", "block");
    //}
    //else {
    //    $("#SpanDelSchItemName").css("display", "none");
    //    $('#DeliverySchItemDDL').css("border-color", "#ced4da");
    //    var DelSchItem = $('#DeliverySchItemDDL').val();
    //    var ItemQty = parseFloat(0);
    //    var AllDelSchQty = parseFloat(0);
    //    $("#PQItmDetailsTbl >tbody >tr").each(function () {
    //        debugger;
    //        var currentRow = $(this);
    //        var Sno = currentRow.find("#SNohiddenfiled").val();
    //        var PQItemQty = parseFloat(0);
    //        var PQItemID = "";

    //        PQItemID = currentRow.find("#PQItemListName_" + Sno).val();
    //        PQItemQty = parseFloat(currentRow.find("#QuotedQuantity").val());

    //        if (PQItemID === DelSchItem) {
    //            ItemQty = PQItemQty;
    //        }
    //    });

    //    $("#DeliverySchTble >tbody >tr").each(function () {
    //        debugger;
    //        var currentRowDS = $(this);
    //        var DelSchItemtbl = currentRowDS.find("td:eq(2)").text();
    //        var DelSchQtyTbl = currentRowDS.find("td:eq(5)").text();
    //        if (DelSchItem == DelSchItemtbl) {
    //            AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQtyTbl));
    //        }
    //    });

    //    $("#DeliveryDate").val("");
    //    $('#DeliverySchQty').val((parseFloat(ItemQty) - parseFloat(AllDelSchQty)).toFixed(QtyDecDigit));
    //    OnChangeDeliveryQty();
    //}
}
function OnChangeDeliveryDate(DeliveryDate) {
    debugger;
    Cmn_OnChangeDeliveryDate(DeliveryDate);
    //var DeliDate = DeliveryDate.value;

    //var DelSchItemddl = $('#DeliverySchItemDDL').val();
    //$('#DeliveryDate').css("border-color", "#ced4da");
    //$("#SpanDeliSchDeliDate").css("display", "none");

    //$("#DeliverySchTble >tbody >tr").each(function () {
    //    debugger;
    //    var currentRow = $(this);
    //    var DelSchItem = currentRow.find("td:eq(2)").text();
    //    var DelSchDateTbl = currentRow.find("td:eq(6)").text();
    //    if (DelSchItemddl == DelSchItem && DeliDate == DelSchDateTbl) {
    //        $('#DeliveryDate').css("border-color", "red");
    //        $('#SpanDeliSchDeliDate').text($("#valueduplicate").text());
    //        $("#SpanDeliSchDeliDate").css("display", "block");
    //        $("#DeliveryDate").val("")
    //        return false;
    //    }
    //});

}

function BindDelivertSchItemList() {
    debugger;
    Cmn_BindDelivertSchItemList("PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity")
};
function OnClickReplicateDeliveryShdul() {
    Cmn_OnClickReplicateDeliveryShdul("PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity");
    if ($("#DeliverySchTble tbody tr ").length > 0) {

        $("#DeliverySchTble tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            currentrow.find("#SRQty").remove();
            currentrow.find("#PenQty").remove();
            currentrow.find("#OdDays").remove();

        });
    }
}
function OnClickAddDeliveryDetail() {
    var rowIdx = 0;
    debugger;
    Cmn_OnClickAddDeliveryDetail("PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity")

    if ($("#DeliverySchTble tbody tr ").length > 0) {

        $("#DeliverySchTble tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            currentrow.find("#SRQty").remove();
            currentrow.find("#PenQty").remove();
            currentrow.find("#OdDays").remove();

        });
    }
    //    var QtyDecDigit = $("#QtyDigit").text(); 
    //    var ValidInfo = "N";
    //    if ($('#DeliverySchItemDDL').val() == "0") {
    //        ValidInfo = "Y";
    //        $('#DeliverySchItemDDL').css("border-color", "red");
    //        $('#SpanDelSchItemName').text($("#valueReq").text());
    //        $("#SpanDelSchItemName").css("display", "block");
    //    }
    //    else {
    //        $('#DeliverySchItemDDL').css("border-color", "#ced4da");
    //        $("#SpanDelSchItemName").css("display", "none");
    //    }
    //    if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
    //        ValidInfo = "Y";
    //        $('#DeliverySchQty').css("border-color", "red");
    //        $('#SpanDelSchItemQty').text($("#valueReq").text());
    //        $("#SpanDelSchItemQty").css("display", "block");
    //    }
    //    else {
    //        $('#DeliverySchQty').css("border-color", "#ced4da");
    //        $("#SpanDelSchItemQty").css("display", "none");
    //    }
    //    if ($('#DeliveryDate').val() == "") {
    //        ValidInfo = "Y";
    //        $('#DeliveryDate').css("border-color", "red");
    //        $('#SpanDeliSchDeliDate').text($("#valueReq").text());
    //        $("#SpanDeliSchDeliDate").css("display", "block");
    //    }
    //    else {
    //        $('#DeliveryDate').css("border-color", "#ced4da");
    //        $("#SpanDeliSchDeliDate").css("display", "none");
    //    }
    //    if (ValidInfo == "Y") {
    //        return false;
    //    }
    //    else {
    //        if (CalculateDeliverySchQty() === false) {
    //            $('#DeliverySchQty').css("border-color", "red");
    //            $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
    //            $("#SpanDelSchItemQty").css("display", "block");
    //            //$('#DeliverySchQty').val("");
    //            return false;
    //        }
    //        else {
    //            $('#DeliverySchQty').css("border-color", "#ced4da");
    //            $("#SpanDelSchItemQty").css("display", "none");

    //            var deldate = $('#DeliveryDate').val();
    //            var deletetext = $("#Span_Delete_Title").text();
    //            var rowCount = $('#DeliverySchTble >tbody >tr').length + 1;
    //            $('#DeliverySchTble tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
    // <td class="red"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${deletetext}"></i></td>
    // <td>${rowCount}</td>
    //<td style="display:none;">${$('#DeliverySchItemDDL').val()}</td>
    //<td >${$('#DeliverySchItemDDL option:selected').text()}</td>
    //<td >${moment($('#DeliveryDate').val()).format('DD-MM-YYYY')}</td>
    //<td class="num_right">${parseFloat($('#DeliverySchQty').val()).toFixed(QtyDecDigit)}</td>
    //<td style="display:none;">${$('#DeliveryDate').val()}</td>

    //</tr>`);
    //            BindDelivertSchItemList();

    //        }


    //    }


    //    //$("#DeliverySchQty").val(parseFloat(0).toFixed(QtyDecDigit));
    //    $("#DeliveryDate").val("");
}
function OnChangeDeliveryQty() {
    debugger;
    Cmn_OnChangeDeliveryQty("PQItmDetailsTbl", "N", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var DelQty = $('#DeliverySchQty').val();
    //if (AvoidDot(DelQty) == false) {
    //    DelQty = "";
    //    $('#DeliverySchQty').val("");
    //}
    //if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
    //    ValidInfo = "Y";
    //    $('#DeliverySchQty').css("border-color", "red");
    //    $('#SpanDelSchItemQty').text($("#valueReq").text());
    //    $("#SpanDelSchItemQty").css("display", "block");
    //}
    //else {
    //    $('#DeliverySchQty').css("border-color", "#ced4da");
    //    $("#SpanDelSchItemQty").css("display", "none");
    //    $('#DeliverySchQty').val(parseFloat(DelQty).toFixed(QtyDecDigit));
    //    if (CalculateDeliverySchQty() === false) {
    //        $('#DeliverySchQty').css("border-color", "red");
    //        $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
    //        $("#SpanDelSchItemQty").css("display", "block");
    //        //$('#DeliverySchQty').val("");
    //        return false;
    //    }
    //    else {
    //        $('#DeliverySchQty').css("border-color", "#ced4da");
    //        $("#SpanDelSchItemQty").css("display", "none");
    //    }
    //}
}


function DeleteTermsAndCondition() {
    $('#TblTerms_Condition tbody').on('click', '.deleteIcon', function () {
        $(this).closest('tr').remove();

    });
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierNameList').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var PQDTransType = sessionStorage.getItem("PQTransType");
    var SuppPros_type = "S";
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
        bill_add_id = "";
    }
    Cmn_SuppAddrInfoBtnClick1(Supp_id, bill_add_id, status, PQDTransType, SuppPros_type);

}
function EnableTaxAndOC() {
    var status = "";
    var PQDTransType = "";
    if ($("#hfStatus").val() != null && $("#hfStatus").val() != "") {
        status = $("#hfStatus").val().trim();
        PQDTransType = sessionStorage.getItem("PQTransType");
    }


    if (status == "D" && PQDTransType == "Update") {
        $("#Tax_Template").attr('disabled', false);
        $("#Tax_Type").attr('disabled', false);
        $("#Tax_Percentage").prop("readonly", false);
        $("#Tax_Level").attr('disabled', false);
        $("#Tax_ApplyOn").attr('disabled', false);
        $("#TaxResetBtn").css("display", "block");
        $("#TaxSaveTemplate").css("display", "block");
        $("#ReplicateOnAllItemsBtn").css("display", "block");
        $("#SaveAndExitBtn").css("display", "block");
        $("#TaxExitAndDiscard").css("display", "block");
        $("#Icon_AddNewRecord").css("display", "block");

        $("#plus_icon1").css("display", "block");
        $("#OtherCharge_DDL").attr('disabled', false);
        $("#TxtOCAmt").prop("readonly", false);
        $("#SaveAndExitBtn_OC").css("display", "block");
        $("#DiscardAndExit_OC").css("display", "block");

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#OCDelIcon").css("display", "block");
        });

    }
    else if (status == null || status == "") {
        $("#Tax_Template").attr('disabled', false);
        $("#Tax_Type").attr('disabled', false);
        $("#Tax_Percentage").prop("readonly", false);
        $("#Tax_Level").attr('disabled', false);
        $("#Tax_ApplyOn").attr('disabled', false);
        $("#TaxResetBtn").css("display", "block");
        $("#TaxSaveTemplate").css("display", "block");
        $("#ReplicateOnAllItemsBtn").css("display", "block");
        $("#SaveAndExitBtn").css("display", "block");
        $("#TaxExitAndDiscard").css("display", "block");
        $("#Icon_AddNewRecord").css("display", "block");

        $("#plus_icon1").css("display", "block");
        $("#OtherCharge_DDL").attr('disabled', false);
        $("#TxtOCAmt").prop("readonly", false);
        $("#SaveAndExitBtn_OC").css("display", "block");
        $("#DiscardAndExit_OC").css("display", "block");

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#OCDelIcon").css("display", "block");
        });
    }
    else {
        $("#Tax_Template").attr('disabled', true);
        $("#Tax_Type").attr('disabled', true);
        $("#Tax_Percentage").prop("readonly", true);
        $("#Tax_Level").attr('disabled', true);
        $("#Tax_ApplyOn").attr('disabled', true);
        $("#TaxResetBtn").css("display", "none");
        $("#TaxSaveTemplate").css("display", "none");
        $("#ReplicateOnAllItemsBtn").css("display", "none");
        $("#SaveAndExitBtn").css("display", "none");
        $("#TaxExitAndDiscard").css("display", "none");
        $("#Icon_AddNewRecord").css("display", "none");

        $(".plus_icon1").css("display", "none");
        $("#OtherCharge_DDL").attr('disabled', true);
        $("#TxtOCAmt").prop("readonly", true);
        $("#SaveAndExitBtn_OC").css("display", "none");
        $("#DiscardAndExit_OC").css("display", "none");
        debugger
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {//Tbl_OC_Deatils
            var currentRow = $(this);
            debugger
            currentRow.find("#OCDelIcon").css("display", "none");//OCDelIcon OCDelIcon
        });
    }

}
//window.location.href = "/ApplicationLayer/PurchaseQuotation/PurchaseQuotationDetail/?doc_no=" + doc_no + "&doc_date=" + doc_date;
function AddNewProspect() {
    debugger;
    try {
        var ProspectFromPQ = "Y";
        location.href = "/BusinessLayer/ProspectSetup/AddProspectSetupDetail?ProspectFromQuot=S&ProspectFromPQ=" + ProspectFromPQ;

    } catch (err) {
        console.log(PFName + " Error : " + err.message);
    }
    //location.href = "/BusinessLayer/ProspectSetup/AddProspectSetupDetail?ProspectFromQuot=S&BtnName=" + New + "&command=print";
}
function OnClickSupplierInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";

    ItmCode = clickedrow.find("#PQItemListName_" + Sno).val();
    var Supp_id = $('#SupplierNameList').val();

    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
function CancelBtnClick() {
    debugger;
    if ($("#Cancel_PQ").is(":checked")) {
        $("#btn_save").prop("disabled", false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $("#btn_save").attr("onclick", 'return SaveBtnClick()');
        $("#tgl_RaiseOrderPQ").attr("disabled", true);
    } else {
        $("#btn_save").prop("disabled", true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_save").attr("onclick", '');
        $("#tgl_RaiseOrderPQ").attr("disabled", false);
    }
}
function PQOnClickOtherChargeBtn() {
    debugger
    OnClickOtherChargeBtn();
    EnableTaxAndOC();
}

//-----------------------------Purchase Quotation List -------------------------------------------//
function FilterPQListData() {
    FilterPQList();
    ResetWF_Level();
}

function FilterPQList() {
    debugger;
    try {
        var SupplierID = $("#SupplierNameList").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $("#SupplierID").val(SupplierID);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PurchaseQuotation/SearchPQList",
            data: {
                SupplierID: SupplierID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#Tbl_list_PQ').html(data);
                $('#ListFilterData').val(SupplierID + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("MRS Error : " + err.message);

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


function OnClickHistoryIconBtn(e) {/* For Getting purchase history*/
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#SupplierNameList").val();
    ItmCode = clickedrow.find("#PQItemListName_" + Sno).val();
    ItmName = clickedrow.find("#PQItemListName_" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM option:selected").text();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var SuppID = "";
    SuppID = $("#SupplierNameList").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemQTQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, " ", "SubItemPRRFQQTQty");

}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#PQItemListName_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#PQItemListName_" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var source_type = $("#ddlSourceType  option:selected").val();
    var Doc_no = $("#QuotationNumber").val();
    var Doc_dt = $("#txtPQDate").val();
    var Sub_Quantity = 0;

    var NewArr = new Array();
    if (flag == "Quantity" || flag == "PRFQQtQty") {

        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            if (source_type == "R" || source_type == "Q") {
                List.sub_item_name = row.find('#subItemName').val();
            }
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);

        });
        Sub_Quantity = clickdRow.find("#QuotedQuantity").val();
    }
    if (source_type == "Q" || source_type == "R") {
        var IsDisabled = "Y";
    }
    else {
        var IsDisabled = $("#DisableSubItem").val();
    }

    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseQuotation/GetSubItemDetails",
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
    return Cmn_CheckValidations_forSubItems("PQItmDetailsTbl", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity", "SubItemQTQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("PQItmDetailsTbl", "SNohiddenfiled", "PQItemListName_", "QuotedQuantity", "SubItemQTQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() {
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
function FilterItemDetail(e) {//added by Prakash Kumar on 21-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "PQItmDetailsTbl", [{ "FieldId": "PQItemListName_", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}


function Checked_RaiseOrderPQ() {/*add by Hina sharma on 20-06-2025*/
    debugger;
    if ($("#tgl_RaiseOrderPQ").is(":checked")) {
        $("#Cancel_PQ").attr("disabled", true);
        $("#hdn_RaiseOrderPQ").val("Y");
        //$("#btn_save").attr("disabled", false);
        //$("#btn_save").attr("onclick", "return InsertSQTDetail()");
        //$("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%)" });
        $("#btn_save").prop("disabled", false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $("#btn_save").attr("onclick", 'return SaveBtnClick()');

    } else {
        $("#Cancel_PQ").attr("disabled", false);
        $("#hdn_RaiseOrderPQ").val("N");
        //$("#btn_save").attr("disabled", true);
        //$("#btn_save").attr("onclick", "");
        //$("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%)" });
        $("#btn_save").prop("disabled", true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_save").attr("onclick", '');

    }

   
}