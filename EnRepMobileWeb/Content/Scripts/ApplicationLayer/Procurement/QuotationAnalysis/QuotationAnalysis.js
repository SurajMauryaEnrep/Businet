/************************************************
Javascript Name: Quotation Analysis
Created By: Surbhi
Created Date: 05/08/2025
Description: This Javascript use for Purchase Quotation Analysis Page many function
Modified By:
Modified Date:
Description:
*************************************************/

$(document).ready(function () {
    debugger;
    $("#RFQ_Number").select2();
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#QA_No").val() == "" || $("#QA_No").val() == null) {
        $("#QA_Date").val(CurrentDate);
        //  $("#rfqdt").val(CurrentDate);
    }
    $("#Tbl_list_PQA #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        var WF_status = $("#WF_status").val();
        var ListFilterData = $("#ListFilterData").val();
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.find("#SSINo").text();
        var doc_date = clickedrow.find("#SSIDt").text();
        if (doc_no != null && doc_no != "") {
            window.location.href = "/ApplicationLayer/QuotationAnalysis/AddQuotationAnalysisDetail/?DocNo=" + doc_no + "&DocDate=" + doc_date + "&ListFilterData=" + ListFilterData;//+ "&WF_status=" + WF_status;
        }
    });
    $("#Tbl_list_PQA #datatable-buttons tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var doc_no = clickedrow.children("#SSINo").text();
        var doc_date = clickedrow.children("#SSIDt").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(doc_no, doc_date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(doc_no);
    });
    //if ($("#hfStatus").val() == "D" || $("#hfStatus").val() == "") {
    if ($("#hfStatus").val() == "") {
        BindRFQList()
    }
    var InvoiceNo = $("#QA_No").val();
    $("#hdDoc_No").val(InvoiceNo);
    CancelledRemarks("#Cancelled", "Disabled");
});

function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var PQDate = $("#QA_Date").val();
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
    PQNo = $("#QA_No").val();
    PQDate = $("#QA_Date").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var TrancType = (PQNo + ',' + PQDate + ',' + "Update" + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    //Added by Nidhi on 08-07-2025
    //var pdfAlertEmailFilePath = 'PQA_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath);
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "QuotationAnalysis_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/QuotationAnalysis/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PQNo != "" && PQDate != "" && level != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/QuotationAnalysis/ApprovPQADetails?Inv_No=" + PQNo + "&Inv_Date=" + PQDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&FilterData=" + ListFilterData1 + "&WF_Status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && PQNo != "" && PQDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PQNo, PQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            showLoader();
            window.location.reload();
        }
    }
}
//Added by Nidhi on 08-07-2025
//function GetPdfFilePathToSendonEmailAlert(PQNo, PQDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/QuotationAnalysis/SavePdfDocToSendOnEmailAlert",
//        data: { PQNo: PQNo, PQDate: PQDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#PPItemName").val();
    var ProductId = clickdRow.find("#Hdn_Item_ID").val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = clickdRow.find("#qt_no").val();
    var Doc_dt = clickdRow.find("#qt_dt").val();
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

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/QuotationAnalysis/GetSubItemDetails",
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

function POSubItemDetailsPopUp(flag, e) {
    debugger;
    var src_type = "R";
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#PPItemName").val();
    var ProductId = clickdRow.find("#Hdn_Item_ID").val();
    var UOMId = clickdRow.find("#Hdn_UOM_ID").val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#QA_OrderNo").val();
    var Doc_dt = $("#QA_OrderDate").val();
    var IsDisabled = $("#DisableSubItem").val();
    
    var Sub_Quantity = 0;
    Sub_Quantity = clickdRow.find("#ord_qty_spec").val();
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        NewArr.push(List);
    });
    if (flag == "POPROrdQty" || flag == "QuantityBase") {
       
        var SRCDoc_no = $("#src_doc_number").val();
        var SRCDoc_no = $("#Hdn_src_doc_number").val();
        var SRCDoc_date = $("#src_doc_date").val();
        if (flag == "QuantityBase") {
            IsDisabled = "Y";
        }
    }
    var hd_Status = "D";// $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: "POPROrdQty",
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            SRCDoc_no: SRCDoc_no,
            SRCDoc_date: SRCDoc_date,
            src_type: src_type,
            UOMId: UOMId
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#QA_No").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
//----------------------------WorkFlow JS End-------------------------------------//

function BindRFQList() {
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QuotationAnalysis/GetRFQList",
            dataType: "json",
            data: {},/*Registration pass value like model*/
            success: function (data) {
                if (data == 'ErrorPage') {
                    // _ErrorPage();
                    return false;
                }
                debugger;
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                    $("#RFQ_Number").empty();
                    var uniqueOptGroupId = "Textddl_" + new Date().getTime();
                    $('#RFQ_Number').append(`<optgroup class='def-cursor' id="${uniqueOptGroupId}" label='${$("#DocNo").text()}' data-doc_date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#' + uniqueOptGroupId).append(`<option data-doc_date="${arr.Table[i].rfq_dt}" value="${arr.Table[i].rfq_no}">${arr.Table[i].rfq_no}</option>`);
                    }
                    var firstEmptySelect = true;
                    $('#RFQ_Number').select2({
                        templateResult: function (arr) {
                            var DocDate = $(arr.element).data('doc_date');
                            var classAttr = $(arr.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + arr.text + '</div>' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                '</div>'
                            );
                            return $result;
                            firstEmptySelect = false;
                        }
                    });
                }
            },
            error: function (Data) {

            }
        });
    }
    catch (err) {
        ToAddJsErrorLogs(err);
    }
};
function OnChangeRFQNumber(RFQNo) {
    debugger;
    var RFQ_No = RFQNo.value;
    if (RFQ_No == "" || RFQ_No == null || RFQ_No == undefined) {
        RFQ_No = RFQNo;
    }
    if (RFQ_No == "") {
        $("#rfqdt").val("");
        $('#SpanRFQ_NoErrorMsg').text($("#valueReq").text());
        $("#SpanRFQ_NoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-RFQ_Number-container']").css("border-color", "red");
    }
    else {
        var Suppname = $('#RFQ_Number option:selected').text();
        var doc_Date = $('#RFQ_Number option:selected').data('doc_date');
        $("#Hdn_RFQ_No").val(Suppname);
        //  $("#rfqdt").val(doc_Date);
        var $dateInput = $("#rfqdt");
        var rawDate = $("#RFQ_Number option:selected").data("doc_date"); // e.g., "06-08-2025"
        if (!rawDate || rawDate === "0") {
            // No date available – clear the input
            $dateInput.val("");
        } else {
            if (rawDate && rawDate.includes("-")) {
                var parts = rawDate.split("-");
                // If it's dd-MM-yyyy → convert to yyyy-MM-dd
                if (parts[2].length === 4) {
                    var formatted = `${parts[2]}-${parts[1]}-${parts[0]}`;
                    $("#rfqdt").val(formatted); // Use in <input type="date">
                } else {
                    // Assume it's already in yyyy-MM-dd
                    $("#rfqdt").val(rawDate);
                }
            }
        }
        $("#SpanRFQ_NoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-RFQ_Number-container']").css("border-color", "#ced4da");
    }
    OnClickAddButton(RFQNo);
}

function OnClickAddButton(RFQNo) {
    debugger;
    var RFQ_No = RFQNo.value;
    if (RFQ_No == "" || RFQ_No == null || RFQ_No == undefined) {
        RFQ_No = RFQNo;
    }
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QuotationAnalysis/GetRFQDetail",
            data: { InvNo: RFQ_No },
            success: function (html) {
                $("#Tbl_list_ItemDetails").html(html);
            },
        });
    } catch (err) {
        console.log("QuotationAnalysisError : " + err.message);
    }
}

//-------------------------Vallidation----------------------------------------//
function HeaderValidations() {
    debugger;
    var ddlSourceType = $("#RFQ_Number").val();
    var ErrorFlag = "N";
    if (ddlSourceType == "---Select---") {
        $("[aria-labelledby='select2-RFQ_Number-container']").css("border-color", "red");
        $("#SpanRFQ_NoErrorMsg").text($("#valueReq").text());
        $("#SpanRFQ_NoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#select2-RFQ_Number-container").css("border-color", "#ced4da");
        $("#SpanRFQ_NoErrorMsg").text("");
        $("#SpanRFQ_NoErrorMsg").css("display", "none");
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

function checkAtLeastOneRadioPerItemGroup() {
    const radioButtons = document.querySelectorAll('#POInvItmDetailsTbl input[type="radio"]');
    const itemGroups = {};

    // Group radio buttons by name attribute (one group per item)
    radioButtons.forEach(radio => {
        const groupName = radio.name;
        if (!itemGroups[groupName]) {
            itemGroups[groupName] = [];
        }
        itemGroups[groupName].push(radio);
    });

    // Validate each group
    for (let groupName in itemGroups) {
        const group = itemGroups[groupName];
        const isSelected = group.some(r => r.checked);

        if (!isSelected) {
            const row = group[0].closest('tr');
            let itemName = 'Unknown Item';

            try {
                const itemNameInput = row.querySelector('input[name="PPItemName"]');
                if (itemNameInput) itemName = itemNameInput.value;
            } catch (e) {
                // fallback
            }

            //alert(`Select atleast one supplier for Item: ${itemName}`);
            swal("", $("#SelectatleastonesupplierforItem").text() + ` ` + `${itemName}`, "warning");
            group[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
            group[0].focus();
            return false;
        }
    }

    return true;
}
function ItemsVallidation() {
    debugger;
    var ErrorFlag = "N";
    if (!checkAtLeastOneRadioPerItemGroup()) {
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }

}
function OnChangeVallidUpto() {
    CheckVallidation("ValidUptoDate", "vmValidUptoDate");

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
    var FinalItemDetail = [];
    FinalItemDetail = InsertPQItemDetails();
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
}
function InsertPQItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#POInvItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohiddenfiled").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Hdn_Item_ID").val();
        ItemList.ItemName = currentRow.find("#PPItemName").text();
        ItemList.Uomid = currentRow.find("#UOMID").val();
        ItemList.UOMName = currentRow.find("#UOM").text();
        ItemList.ItemType = "1";//currentRow.find("#IDItemtype").val();
        ItemList.qt_qty = currentRow.find("#hdnQuotedQuantity").val();
        ItemList.ItemRemarks = currentRow.find("#hdnremarks").val();
        ItemList.SuppID = currentRow.find("#supp_id").val();
        ItemList.SuppPros_type = currentRow.find("#SupplierTypeId").val();
        ItemList.suppRating = currentRow.find("#SupplierRating").val();
        ItemList.qt_no = currentRow.find("#qt_no").val();
        ItemList.qt_dt = currentRow.find("#qt_dt").val();
        ItemList.item_rate = currentRow.find("#Price").val();
        ItemList.item_disc_perc = currentRow.find("#DiscountInPercentage").val();
        ItemList.net_rate = currentRow.find("#PriceAfterDiscount").val();
        ItemList.item_net_val_bs = currentRow.find("#NetValue").val();
        ItemList.PaymentTermsInDays = currentRow.find('#PaymentTermsInDays').val();
        ItemList.DeliveryDate = currentRow.find('#DeliveryDate').val();
        //var value1 = $('#OrderFor_' + rowid).val();
        console.log($('input[name="OrderFor_' + rowid + '"]').length); // should be 2
        console.log($('input[name="OrderFor_' + rowid + '"]:checked').length); // should be 1 if one is selected
        console.log($('input[name="OrderFor_' + rowid + '"]:checked').val());
        let value2 = $('input[name="OrderFor_' + rowid + '"]:checked').val();
        var value = "N";
        var checkedRadio = currentRow.find("input[type='radio']:checked");
        if (checkedRadio.length > 0) {
            var value = checkedRadio.val();
            console.log("Selected value in this row:", value);
        } else {
            var value = "N";
        }
        ItemList.OrderFor = value;
        ItemDetailList.push(ItemList);
        debugger;
    });
    return ItemDetailList;
};

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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnClickIconBtn(e) {
    debugger;
    var row_id = $(e.target).closest('tr').find("#SNohiddenfiled").val();
    var ItmCode = $(e.target).closest('tr').find("#Hdn_Item_ID").val();//$(e.target).closest('tr').find("#PQItemListName_" + row_id).val();
    ItemInfoBtnClick(ItmCode);
}

function OnPOItemDetails(e) {
    debugger;
    var row_id = $(e.target).closest('tr').find("#hdnpo_no").val();
    var PONO = $(e.target).closest('tr').find("#hdnpo_no").val();
    var PODt = $(e.target).closest('tr').find("#hdnpo_dt").val();
    POItemInfoBtnClick(PONO, PODt);
}

function POItemInfoBtnClick(PO_NO, PO_Dt) {
    if (PO_NO == "" || PO_NO == null || PO_NO == undefined) {
        return;
    }
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QuotationAnalysis/GetPOItemDetail",
            data: { PO_No: PO_NO, PO_Date: PO_Dt },
            success: function (html) {
                $("#QAOrderValueDetails").html(html);
            },
        });
    } catch (err) {
        console.log("QuotationAnalysisError : " + err.message);
    }
}

function POItemOCInfoBtnClick(PO_NO, PO_Dt) {
    debugger;
    if (PO_NO == "" || PO_NO == null || PO_NO == undefined) {
        PO_NO = $("#Hdn_PONO").val();
        PO_Dt = $("#Hdn_PO_DT").val();

    }
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QuotationAnalysis/GetPOItemDetailJson",
            data: { PO_No: PO_NO, PO_Date: PO_Dt },
            success: function (data) {
                BindDataInCaseOfQuotation(data);
            },
        });
    } catch (err) {
        console.log("QuotationAnalysisError : " + err.message);
    }
}

function BindDataInCaseOfQuotation(arr) {
    debugger;
    GetOCWithOCTaxDetailByQTNumber(arr)

}
function GetOCWithOCTaxDetailByQTNumber(arr1) {
    try {
        debugger;
        let arr = JSON.parse(arr1);

        //OC Details Start
        if (arr.Table2.length > 0) {
            let Oclen = $("#ht_Tbl_OC_Deatils > tbody>tr").length;
            var CurrencyInBase = "N";
            var hdbs_curr = arr.Table2[0].bs_curr_id;
            var hdcurr = $("#ddlCurrency").val();
            if (hdbs_curr == hdcurr) {
                CurrencyInBase = "Y";
            } else {
                CurrencyInBase = "N";
            }
            var rowIdx = 0;
            var deletetext = $("#Span_Delete_Title").text();
            var tax_info = $("#Span_TaxCalculator_Title").text();
            var tdTax = "";
            var disableRoundOff = "disabled";
            var DMenuId = $("#DocumentMenuId").val();
            var disblForJO = "";
            var OcSupp_Name = "";
            var OcSupp_Id = "";
            var oc_supp_type = "";
            var OCSuppAccId = "";
            var displaynoneinDomestic = "";
            var OCAmountsp = "";
            displaynoneinDomestic = "style='display:none;'";
            var OC_For = "";
            var OCAccId = "";
            $('#Tbl_OC_Deatils tbody').empty();
            for (var m = 0; m < arr.Table2.length; m++) {
                var OCName = arr.Table2[m].oc_name;
                var OCID = arr.Table2[m].oc_id;
                var OCValue = arr.Table2[m].oc_val;
                var OCHSNVal = arr.Table2[m].HSN_code;
                var OCCurrId = arr.Table2[m].curr_id;
                var OCCurr = arr.Table2[m].curr_name;
                var OCConv = arr.Table2[m].conv_rate;
                var OcAmtBs = arr.Table2[m].OCValBs;
                var OCTaxAmt = parseFloat(arr.Table2[m].tax_amt).toFixed(cmn_ValDecDigit)
                var OCTaxTotalAmt = arr.Table2[m].total_amt;

                debugger;
                tdTax = `<td class="num_right">
                    <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(OCTaxAmt)).toFixed(cmn_ValDecDigit)}</div>
                    <div class="col-md-2 col-sm-12" style="padding:0px;">
                        <button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${tax_info}"></i></button></div>
                </td>
                <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(OCTaxTotalAmt)).toFixed(cmn_ValDecDigit)}</td>`;


                $('#Tbl_OC_Deatils tbody').append(`<tr id="R${rowIdx}">
                <td id="deletetext" class="red center">${`<i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" title="${deletetext}" disabled></i>`}</td>
                <td id="OCName" >${OCName}</td>
                <td id="OCName" >${OCHSNVal}</td>
                <td id="OCCurr" class="center" ${disblForJO}>${OCCurr}</td>
                <td id="HdnOCCurrId" hidden>${OCCurrId}</td>
                <td id="td_OCSuppName" style="display:none">${OcSupp_Name}</td>
                <td id="td_OCSuppID" hidden >${OcSupp_Id}</td>
                <td id="td_OCSuppType" hidden >${oc_supp_type}</td>
                <td id="OCConv" class="num_right" ${disblForJO}>${parseFloat(OCConv).toFixed(cmn_ExchDecDigit)}</td>
                <td hidden="hidden" id="OCValue">${OCID}</td>
                <td class="num_right" id="OCAmount" ${displaynoneinDomestic}>${parseFloat(OCValue).toFixed(cmn_ValDecDigit)}</td>
                <td class="num_right" id="OcAmtBs" ${disblForJO}>${parseFloat(OcAmtBs).toFixed(cmn_ValDecDigit)}</td>
                `+ tdTax + `
                <td id="OCAccId" hidden >${OCAccId}</td>
                <td id="OCSuppAccId" hidden >${OCSuppAccId}</td>
                <td id="OCFor" hidden>${OC_For}</td>
            </tr>`);
            }

            ResetOCDetail();
            $("#Tbl_OC_Deatils >tbody >tr").each(function () {
                var currentRow = $(this);
                currentRow.find("#OCDelIcon").css("filter", "grayscale(100%)");
                currentRow.find("#OCDelIcon").removeClass("deleteIcon");
                currentRow.find("#oc_chk_roundoff").attr("disabled", true);
                currentRow.find("#oc_p_round").attr("disabled", true);
                currentRow.find("#oc_m_round").attr("disabled", true);
            });
            //$("#OC_buttons button").remove();
            $("#SaveAndExitBtn_OC").remove(); $("#DiscardAndExit_OC").remove();
            $("#AddOCDetailBtn").parent().css("display", "none");
            $("#OtherCharge_DDL").attr("disabled", true);
            $("#TxtOCAmt").attr("disabled", true);
            $("#OCCurrency").attr("disabled", true);
            $("#OcThirdPartySupp_DDL").attr("disabled", true);
            try {
                OtherChargePageFuncton();
            }
            catch (ex) {

            }
            Calculate_OCAmount();
            OnClickSaveAndExit_OC_Btn();
        }
        else {
            $("#BtnOC_Amount").attr("data-target", "");
            return;
        }
        //OC Details End
        //OC Tax Details Start
        if (arr.Table3.length > 0) {
            $('#Hdn_OC_TaxCalculatorTbl tbody tr').remove();
            $('#Tbl_OtherChargeList tbody tr').remove();
            if (arr.Table3.length > 0) {
                for (var l = 0; l < arr.Table3.length; l++) {
                    $('#Hdn_OC_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table3[l].item_id}</td>
                                    <td id="TaxName">${arr.Table3[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table3[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table3[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table3[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table3[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table3[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table3[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table3[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table3[l].tax_acc_id}</td>
                                    <td id="OCFor">${arr.Table3[l].OCFor}</td>
                                 </tr>`);

                    $('#Hdn_OCTemp_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table3[l].item_id}</td>
                                    <td id="TaxName">${arr.Table3[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table3[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table3[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table3[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table3[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table3[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table3[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table3[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table3[l].tax_acc_id}</td>
                                    <td id="OCFor">${arr.Table3[l].ocfor}</td>
                                   </tr>`);
                }
            }
        }
        //OC Tax Details End
    }
    catch (ex) {

    }

}
function OnClickOCTaxCalculationBtn(e) {
    debugger;
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
function OnClickSaveAndExit_OC_Btn() {
    // CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValueInBase", "#NetOrderValueInBase")
}
function OnClickHistoryIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#supp_id").val();
    ItmCode = clickedrow.find("#Hdn_Item_ID").val();
    ItmName = clickedrow.find("#hdnItemName").val();
    UOMName = clickedrow.find("#hdnUOMName").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
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

//-----------------------------Purchase Quotation List -------------------------------------------//
function FilterPQAListData() {
    FilterPQList();
    ResetWF_Level();
}
function FilterPQList() {
    debugger;
    try {
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/QuotationAnalysis/PQAListSearch",
            data: {
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#Tbl_list_PQA').html(data);
                $('#ListFilterData').val(Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("PQA Error : " + err.message);
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

function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var SuppID = "";
    SuppID = $("#SupplierNameList").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
}
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
    Cmn_FilterTableData(e, "POInvItmDetailsTbl", [{ "FieldId": "PPItemName", "FieldType": "input", "SrNo": "" }]);
}
function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#ForceClosed").attr("disabled", true);
        $("#ForceClosed").prop("checked", false);
        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function onchangeCancelledRemarks() {
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}