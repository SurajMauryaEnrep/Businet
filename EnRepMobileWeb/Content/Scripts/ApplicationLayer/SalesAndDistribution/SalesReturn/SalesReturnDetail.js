$(document).ready(function () {
    debugger;
    $("#DdlTranspt_Name").select2();
    var src_type = $('#src_type').val();
    if (src_type == "R") {
        $("#ReqGRNumber").css("display", "none");
        $("#ReqGRDate").css("display", "none");
        $("#ReqNumberOfPacks").css("display", "none");
        $("#ReqTransporterName").css("display", "none");
    }
    else {
        $("#ReqGRNumber").css("display", "");
        $("#ReqGRDate").css("display", "");
        $("#ReqNumberOfPacks").css("display", "");
        $("#ReqTransporterName").css("display", "");
    }
    if (src_type == "C") {
        $("#CustmGlVoucherDt").css("display", "");
        $("#GlVoucherDt").css("display", "none");
        $("#DivRoundOff").css("display", "none");
        $("#DivRoundOffHeading").css("display", "none");
       // $('#chk_roundoff').prop("checked", false);
       // $("#chk_roundoff").prop("disabled", true);
    }
    else {
        $("#CustmGlVoucherDt").css("display", "none");
        $("#GlVoucherDt").css("display", "");
        $("#DivRoundOff").css("display", "");
        $("#DivRoundOffHeading").css("display", "");
        //$('#chk_roundoff').prop("checked", false);
        //$("#chk_roundoff").prop("disabled", false);
    }
    if (src_type == "A") {
        $('#AdSReturnItmDetailsTbl tbody tr').each(function () {
            var row = $(this);
            var Sno = row.find("#SNohiddenfiled").val();
            row.find("#wh_id" + Sno).select2();
        });
    }
    else {
        $('#SReturnItmDetailsTbl tbody tr').each(function () {
            var row = $(this);
            var Sno = row.find("#RowId").val();
            row.find("#wh_id" + Sno).select2();
        });
    }
    BindCustomerList();
    OnChangeInvoiceValue();
    //OnChangeReturnValue();
    debugger;
    //if (src_type == "A") {
        $('#AdSReturnItmDetailsTbl').on('click', '#deleteAdhocItem', function () {
            debugger;
            var child = $(this).closest('tr').nextAll();
            child.each(function () {
                var id = $(this).attr('id');
                var idx = $(this).children('.row-index').children('p');
                var dig = parseInt(id.substring(1));
                idx.html(`Row ${dig - 1}`);
                $(this).attr('id', `R${dig - 1}`);
            });

            var ItemCode = $(this).closest('tr').find("#hdItemId").val();
            //var ShipNo = $(this).closest('tr').find("#ShipNumber").val();
            //var InvoiceValue = $(this).closest('tr').find("#InvoiceValue").val();
            var ReturnValue = $(this).closest('tr').find("#ReturnValue").val();
            $(this).closest('tr').remove();
            var prt_no = $("#ReturnNumber").val();
            if (prt_no == null || prt_no == "") {
                if ($('#AdSReturnItmDetailsTbl tbody tr').length <= 1) {
                    debugger;
                    $("#ddlCustomerName").prop("disabled", false);
                    //$("#ddlDocumentNumber").prop("disabled", false);
                    //$("#hdSelectedSourceDocument").val(null);
                    //$("#BtnAttribute").css('display', 'block');
                    //$(".plus_icon1").css('display', 'block');
                }
            }
            updateItemSerialNumber(src_type);
            DeleteItemBatchSerialOrderQtyDetails(ItemCode);
            //DeleteItemBatchSerialDetail(ItemCode, ShipNo);
            Cmn_DeleteSubItemQtyDetail(ItemCode);
            DeleteVoucherDetail(ItemCode, "", "", ReturnValue)
        });
    //}
    //else {
        $('#SReturnItmDetailsTbl').on('click', '.deleteIcon', function () {
            debugger;
            var child = $(this).closest('tr').nextAll();
            child.each(function () {
                var id = $(this).attr('id');
                var idx = $(this).children('.row-index').children('p');
                var dig = parseInt(id.substring(1));
                idx.html(`Row ${dig - 1}`);
                $(this).attr('id', `R${dig - 1}`);
            });

            var ItemCode = $(this).closest('tr').find("#hdItemId").val();
            var ShipNo = $(this).closest('tr').find("#ShipNumber").val();
            var InvoiceValue = $(this).closest('tr').find("#InvoiceValue").val();
            var ReturnValue = $(this).closest('tr').find("#ReturnValue").val();
            $(this).closest('tr').remove();
            var prt_no = $("#ReturnNumber").val();
            if (prt_no == null || prt_no == "") {
                if ($('#SReturnItmDetailsTbl tr').length <= 1) {
                    debugger;
                    $("#ddlCustomerName").prop("disabled", false);
                    $("#ddlDocumentNumber").prop("disabled", false);
                    $("#hdSelectedSourceDocument").val(null);
                    //$("#BtnAttribute").css('display', 'block');
                    $(".plus_icon1").css('display', 'block');
                }
            }

            updateItemSerialNumber(src_type);
            DeleteItemBatchSerialDetail(ItemCode, ShipNo);
            Cmn_DeleteSubItemQtyDetail(ItemCode);
            DeleteVoucherDetail(ItemCode, ShipNo, InvoiceValue, ReturnValue)
        });
   // }
    sessionStorage.removeItem("ItemRetQtyDetails");
    GetViewDetails();
    SRTNo = $("#ReturnNumber").val();
    SRTDate = $("#txtReturnDate").val();
    $("#hdDoc_No").val(SRTNo);
    var CnNarr = $('#CreditNoteRaisedAgainstSalRet').text();
    CnNarr += IsNull(SRTNo, "") == "" ? " {INV_NO}" : " " + SRTNo;
    CnNarr += IsNull(SRTDate, "") == "" ? " {INV_DATE}" : " " + moment(SRTDate).format("DD-MM-YYYY");
    $('#CnNarr').val(CnNarr);
    BindDDLAccountList();

    if (src_type == "A") {
        $("#SReturnItmDetailsTbl").css("display", "none")
        $("#AdSReturnItmDetailsTbl").css("display", "")
        $("#TaxDetailAccordian").css("display", "")
        $("#InvBillNum").css("display", "")
        $("#InvBillDt").css("display", "")

        $("#InvNoDiv").css("display", "none");
        $("#InvDateDiv").css("display", "none");
        $("#InvWiseAddItem").css("display", "none");
        $("#th_inv_value").css("display", "none");

        var cust_id = $("#ddlCustomerName").val()
        var PageDisable = $("#PageDisable").val()
        if (cust_id != "0" && cust_id != "" && cust_id != null) {
            if (PageDisable == "Y") {
                $("#addbtn").css("display", "none");
            }
            else {
                $("#addbtn").css("display", "");
            }
            
        }
        else {
            $("#addbtn").css("display", "none");
        }
        BindScrpItmList("1");
        //BindWarehouseList("1");
    }
    else {
        if (src_type == "R") {
            $("#SReturnItmDetailsTbl #thUOM").css("display", "none")
            $("#SReturnItmDetailsTbl #ThWarehouse").css("display", "none")
        }
        else {
            $("#SReturnItmDetailsTbl #thUOM").css("display", "")
            $("#SReturnItmDetailsTbl #ThWarehouse").css("display", "")
        }
        $("#TaxDetailAccordian").css("display", "none")
        $("#InvBillNum").css("display", "none")
        $("#InvBillDt").css("display", "none")
        $("#InvNoDiv").css("display", "");
        $("#InvDateDiv").css("display", "");
        var SRTStatus = $("#hdSRTStatus").val()
        if (SRTStatus == "C") {
            $("#InvWiseAddItem").css("display", "none");
        }
        else {
            $("#InvWiseAddItem").css("display", "");
        }
       
        $("#th_inv_value").css("display", "");
    }
    CancelledRemarks("#Cancelled", "Disabled");
});
function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#ItemID").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#SerialItemID").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}
function BindDDLAccountList() {
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105103142");
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
                $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="TextddlV${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#TextddlV' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
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
var ValDecDigit = $("#ValDigit").text();
function GetViewDetails() {
    //
    debugger;
    if ($("#hdItemBatchSerialDetailList").val() != null && $("#hdItemBatchSerialDetailList").val() != "") {
        debugger
        var arr2 = $("#hdItemBatchSerialDetailList").val();
        var arr = JSON.parse(arr2);
        $("#hdItemBatchSerialDetailList").val("");

        sessionStorage.removeItem("ItemRetQtyDetails");
        sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(arr));
    }

}
function DeleteItemBatchSerialDetail(ItemCode, ShipNo) {
    debugger;
    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            debugger;
            for (i = 0; i < FReturnItemDetails.length; i++) {
                var ItemID = FReturnItemDetails[i].ItmCode;
                var ShipNumber = FReturnItemDetails[i].IconShipNumber;

                if (ItemID == ItemCode && ShipNumber == ShipNo) {


                }
                else {
                    NewArr.push(FReturnItemDetails[i])
                }

            }
            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));

        }
    }
}
function updateItemSerialNumber(src_type) {
    debugger;
    var SerialNo = 0;
    if (src_type == "A") {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            SerialNo = SerialNo + 1;
            currentRow.find("#SpanRowId").text(SerialNo);
        });
    }
    else {
        $("#SReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            SerialNo = SerialNo + 1;
            currentRow.find("#SpanRowId").text(SerialNo);
        });
    }
};
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
function CheckFormValidation() {
    debugger;
    var src_type = $('#src_type').val();
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var Narr = $('#SalesVoucherRaisedAgainstSalRet').text();
    $('#SvNarr').val(Narr);
    debugger;
    if (src_type == "A") {
        var rowcount = $('#AdSReturnItmDetailsTbl tr').length;
    }
    else {
        var rowcount = $('#SReturnItmDetailsTbl tr').length;
    }
    var ValidationFlag = true;
    var CustomerName = $('#ddlCustomerName').val();
    var DocumentNumber = $('#ddlDocumentNumber').val();
    if (CustomerName == "" || CustomerName == "0") {
        document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
        // $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }
    else {
        document.getElementById("vmcust_id").innerHTML = "";
        $("#ddlCustomerName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "ced4da");
    }
    if (src_type != "A") {
        if (DocumentNumber == "" || DocumentNumber == "0") {
            document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
            $("#ddlDocumentNumber").css("border-color", "red");
            ValidationFlag = false;
        }
        else {
            document.getElementById("vmsrc_doc_no").innerHTML = "";
            $("#ddlDocumentNumber").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "ced4da");
        }
    }
    else {
        var AdHocBill_no = $("#InvBillNumber").val();
        var AdHocBill_dt = $("#InvBillDate").val();
        if (AdHocBill_no == "" || AdHocBill_no == "0" || AdHocBill_no == null) {
            document.getElementById("vmInvBillNumber").innerHTML = $("#valueReq").text();
            $('#InvBillNumber').css("border-color", "red");
            ValidationFlag = false;
        }
        if (AdHocBill_dt == "" || AdHocBill_dt == "0" || AdHocBill_dt == null) {
            document.getElementById("vmInvBillDate").innerHTML = $("#valueReq").text();
            $('#InvBillDate').css("border-color", "red");
            ValidationFlag = false;
        }
    }
    var RT_Status = $('#hdSRTStatus').val();
    if (RT_Status == "RT") {
        if (CheckCancelledStatus() == false) {
            return false;
        }
    }
    if ($("#Cancelled").is(":checked")) {
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
    if (ValidationFlag == true) {
        if (rowcount > 1) {
            var flag = CheckItemLevelValidations();
            if (flag == false) {
                return false;
            }
            if (src_type == "A") {
                if (AdHocCheckItemBatchValidation() == false) {
                    swal("", $("#PleaseenterBatchDetails").text(), "warning");
                    return false;
                }
                if (CheckItemSerialValidation() == false) {
                    swal("", $("#PleaseenterSerialDetails").text(), "warning");
                    return false;
                }
                var GstApplicable = $("#Hdn_GstApplicable").text();
                if (GstApplicable == "Y") {
                    if (Cmn_taxVallidation("AdSReturnItmDetailsTbl", "item_tax_amt", "hdItemId", "Hdn_TaxCalculatorTbl", "ItemName") == false) {
                        return false;
                    }
                }
            }
            else {
                if (src_type != "R") {
                    var Batchflag = CheckItemBatchValidation();
                    if (Batchflag == false) {
                        return false;
                    }
                }
            }
            var VoucherValidationFlag = Check_VoucherValidations();
            if (VoucherValidationFlag == false) {
                return false;
            }
            if (CheckValidations_forSubItems() == false) {
                return false;
            }
            if (src_type != "R") {
                if (CheckTransportDetailValidation() == false) {/*Add By Hina on 29-05-2025 for transporter detail for EInvoce*/
                    return false;
                }
            }            
            if (flag == true) {
                var SalesReturnItemDetailList = new Array();
                if (src_type == "A") {
                    $("#AdSReturnItmDetailsTbl TBODY TR").each(function () {
                        debugger;
                        var row = $(this);
                        var ItemList = {};
                        var SpanRowId = row.find("#SNohiddenfiled").val();
                        ItemList.SourceDocumentNo = "";
                        ItemList.SourceDocumentDate = "";
                        ItemList.ItemId = row.find('#hdItemId').val();
                        ItemList.ItemName = row.find('#ItemName' + SpanRowId).val();
                        if (row.find('#hdUOMId').val() == "" || row.find('#hdUOMId').val() == null) {
                            ItemList.UOMId = 0;
                        }
                        else {
                            ItemList.UOMId = row.find('#hdUOMId').val();
                        }
                        ItemList.uom_name = row.find('#UOM').val();
                        ItemList.sub_item = row.find('#sub_item').val();
                        ItemList.ShipNo = "";
                        ItemList.ShipDate = "";
                        ItemList.ShipQuantity = "0";
                        ItemList.ReturnQuantity = row.find("#ReturnQuantity").val();
                        ItemList.AvailableQuantity = row.find("#AvailableQuantity").val();
                        ItemList.InvoiceValue = "";
                        ItemList.ReturnValue = row.find("#ReturnValue").val();
                        //ItemList.ReasonForReturn = row.find("#ReasonForReturn").val();
                        ItemList.ItemRemarks = row.find('#remarks').val();
                        ItemList.pending_qty = "0";
                        ItemList.item_acc_id = IsNull(row.find("#hdn_item_gl_acc").val(), '');                        
                        ItemList.wh_id = row.find("#wh_id" + SpanRowId).val();
                        ItemList.Price = row.find("#Price").val();
                        ItemList.item_tax_amt = row.find("#item_tax_amt").val();
                        SalesReturnItemDetailList.push(ItemList);
                        debugger;
                    });
                }
                else {
                    $("#SReturnItmDetailsTbl TBODY TR").each(function () {
                        debugger;
                        var row = $(this);
                        var ItemList = {};

                        ItemList.SourceDocumentNo = $("#vmsrc_doc_no option:selected").text();
                        ItemList.SourceDocumentDate = $("#txtsrcdocdate").val();
                        ItemList.ItemId = row.find('#hdItemId').val();
                        ItemList.ItemName = row.find('#ItemName').val();
                        if (src_type == "R") {
                            ItemList.UOMId = "0";
                            ItemList.uom_name ="0";
                        }
                        else {
                            ItemList.UOMId = row.find('#hdUOMId').val();
                            ItemList.uom_name = row.find('#UOM').val();
                        }
                        ItemList.sub_item = row.find('#sub_item').val();
                        ItemList.ShipNo = row.find("#ShipNumber").val();
                        var ShipDate = row.find("#ShipDate").val().trim();
                        var date = ShipDate.split("-");
                        var FDate = date[2] + '-' + date[1] + '-' + date[0];
                        ItemList.ShipDate = FDate;
                        ItemList.ShipQuantity = row.find("#ShipQuantity").val();
                        ItemList.ReturnQuantity = row.find("#ReturnQuantity").val();
                        ItemList.InvoiceValue = row.find("#InvoiceValue").val();
                        ItemList.ReturnValue = row.find("#ReturnValue").val();
                        //ItemList.ReasonForReturn = row.find("#ReasonForReturn").val();
                        ItemList.ItemRemarks = row.find('#remarks').val();
                        ItemList.pending_qty = row.find("#AvailableQuantity").val();
                        ItemList.item_acc_id = IsNull(row.find("#hdn_item_gl_acc").val(), '');
                        var SpanRowId = row.find("#RowId").val();
                        if (src_type == "R") {
                            ItemList.wh_id = "0";

                        }
                        else {
                            ItemList.wh_id = row.find("#wh_id" + SpanRowId).val();

                        }
                        ItemList.Price = IsNull(row.find('#Item_cost').val(), 0);
                        ItemList.item_tax_amt = 0;
                        SalesReturnItemDetailList.push(ItemList);
                        debugger;
                    });
                }
                var str = JSON.stringify(SalesReturnItemDetailList);
                $('#hdSalesReturnItemDetailList').val(str);
                /*-----------Sub-item-------------*/

                var SubItemsListArr = SRT_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/

                var TaxDetail = [];
                if (src_type == "A") {
                    TaxDetail = InsertTaxDetails();
                    var str_TaxDetail = JSON.stringify(TaxDetail);
                    $('#hdItemTaxDetail').val(str_TaxDetail);
                }

                
               

                BindGLVoucherDetail();

                FinalSI_VouDetail = GetSI_VoucherDetails();
                var SIVouGlDt = JSON.stringify(FinalSI_VouDetail);
                $('#hdVouGlDetailList').val(SIVouGlDt);

                var FinalCostCntrDetails = [];
                FinalCostCntrDetails = Cmn_InsertCCDetails();
                var CCDetails = JSON.stringify(FinalCostCntrDetails);
                $('#hdn_CC_DetailList').val(CCDetails);

                if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hdSRTStatus") == false) {
                    return false;
                }
                if (src_type == "A") {
                    SaveBatchItemDeatil();
                    SaveItemSerialItemDeatil();
                }
                else {
                    BindItemBatchSerialDetail();
                }

                var FinalPOOCDetail = [];
                var FinalPOOCTaxDetail = [];

                FinalPOOCDetail = InsertPOOtherChargeDetails();
                FinalPOOCTaxDetail = InsertPO_OCTaxDetails();

                $("#hdOCDetailList").val(JSON.stringify(FinalPOOCDetail));
                $("#hdOCTaxDetailList").val(JSON.stringify(FinalPOOCTaxDetail));

                var Custname = $('#ddlCustomerName option:selected').text();
                $("#hdSRTcust_name").val(Custname);
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                $('#src_type').attr("disabled", false);
                return true;
            }
            else {
                return false;
            }
        }
        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function InsertPOOtherChargeDetails() {
    debugger;
    var PO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OC_TaxAmt = IsNull(currentRow.find("#OCTaxAmt").text(), 0);
            OC_TotlAmt = IsNull(currentRow.find("#OCTotalTaxAmt").text(), 0);
            PO_OCList.push({ OC_ID: OC_ID, OCValue: OCValue, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, OC_TaxAmt: OC_TaxAmt, OC_TotlAmt: OC_TotlAmt });
        });
    }
    return PO_OCList;
};
function InsertPO_OCTaxDetails() {/*Add by Hina Sharma on 09-07-2025*/
    debugger;


    var FTaxDetails = $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").length;
    var PO_OCTaxList = [];

    if (FTaxDetails != null) {
        if (FTaxDetails > 0) {

            $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").each(function () {
                var Crow = $(this);

                debugger;
                var ItemID = Crow.find("#TaxItmCode").text().trim();
                var TaxID = Crow.find("#TaxNameID").text().trim();
                var TaxName = Crow.find("#TaxName").text().trim();
                var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                var TaxLevel = Crow.find("#TaxLevel").text().trim();
                var TaxValue = Crow.find("#TaxAmount").text().trim();
                var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                PO_OCTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });


            });
        }
    }
    return PO_OCTaxList;
};

function onchangeAdHocBill_no() {
    document.getElementById("vmInvBillNumber").innerHTML = "";
    $("#InvBillNumber").css("border-color", "#ced4da");
}
function onchangeAdHocBill_dt() {
    document.getElementById("vmInvBillDate").innerHTML = "";
    $("#InvBillDate").css("border-color", "#ced4da");
}
function SaveBatchItemDeatil() {
    debugger;
    var Batcharr = new Array;
    $('#SaveItemBatchTbl tbody tr').each(function () {
        var Batch_arr = {};
        var CurrentRow = $(this);
        var itemid = CurrentRow.find("#ItemID").val();
        Batch_arr.itemid = CurrentRow.find("#ItemID").val();
        Batch_arr.BatchNo = CurrentRow.find("#BatchNo").val();
        Batch_arr.BatchExDate = CurrentRow.find("#BatchExDate").val();
        Batch_arr.BatchQty = CurrentRow.find("#BatchQty").val();
        Batch_arr.mfg_name = CurrentRow.find("#MfgName").val() || '';
        Batch_arr.mfg_mrp = CurrentRow.find("#MfgMrp").val() || '';
        Batch_arr.mfg_date = CurrentRow.find("#MfgDate").val() || '';
        var GLRow = $("#AdSReturnItmDetailsTbl > tbody >tr #hdItemId[value='" + itemid+"']").closest('tr')
        Batch_arr.batchable = GLRow.find("#hfbatchable").val();
        Batch_arr.Seriable = GLRow.find("#hfserialable").val();
        Batcharr.push(Batch_arr);
    })
    $("#hdn_BatchItemDeatilData").val(JSON.stringify(Batcharr));
}
function SaveItemSerialItemDeatil() {
    var Serialarr = new Array;
    $('#SaveItemSerialTbl tbody tr').each(function () {
        var Serial_arr = {};
        var CurrentRow = $(this);
        var itemid = CurrentRow.find("#SerialItemID").val();
        Serial_arr.itemid = CurrentRow.find("#SerialItemID").val();
        Serial_arr.SerialNo = CurrentRow.find("#Serial_SerialNo").val();
        Serial_arr.mfg_name = CurrentRow.find("#Serial_MfgName").val() || '';
        Serial_arr.mfg_mrp = CurrentRow.find("#Serial_MfgMrp").val() || '';
        Serial_arr.mfg_date = CurrentRow.find("#Serial_MfgDate").val() || '';
        var GLRow = $("#AdSReturnItmDetailsTbl > tbody >tr #hdItemId[value='" + itemid + "']").closest('tr')
        Serial_arr.batchable = GLRow.find("#hfbatchable").val();
        Serial_arr.Seriable = GLRow.find("#hfserialable").val();
        Serialarr.push(Serial_arr);
    })
    $("#hdn_SerialItemDeatilData").val(JSON.stringify(Serialarr));
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ItmCode = currentRow.find("#hdItemId").val();
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
    });
    return TaxDetails;
}
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var docno = $('#ddlDocumentNumber option:selected').text();
    $("#hd_doc_no").val(docno);
    if ($('#ddlDocumentNumber').val() != "0" && $('#ddlDocumentNumber').val() != "") {
        var text = $('#ddlDocumentNumber').val();

        document.getElementById("vmsrc_doc_no").innerHTML = "";
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
        $("#ddlDocumentNumber").css("border-color", "#ced4da");

        $(".plus_icon1").css('display', 'none');
        debugger;
        $("#ddlCustomerName").prop("disabled", true);
        $("#ddlDocumentNumber").prop("disabled", true);
        var hdSelectedSourceDocument = null;
        var SourDocumentNo = $('#ddlDocumentNumber option:selected').text();
        hdSelectedSourceDocument = SourDocumentNo;
        $("#hdSelectedSourceDocument").val(hdSelectedSourceDocument);
        var src_type = $('#src_type').val();
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/SalesReturn/GetSIItemDetail",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument,
                    src_type: src_type
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                var returnedqty;
                                if (parseFloat(arr.Table[i].returnedqty) == 0) {
                                    returnedqty = "";
                                }
                                else {
                                    returnedqty = parseFloat(arr.Table[i].returnedqty).toFixed(QtyDecDigit);
                                }
                                var hidefield = "";
                                if (src_type == "R") {
                                    hidefield = "hidden='hidden'"
                                }
                                $('#SReturnItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">                                                       
                                                        <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                        <td class="sr_padding"><span id="SpanRowId">${i + 1}</span></td>                                                        
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class=" col-sm-11" style="padding:0px;">
                                                                <input id="ItemName" class="form-control" autocomplete="off" type="text" name="ItemName" placeholder=""  disabled value="${arr.Table[i].item_name}">
                                                                <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" />
                                                                <input type="hidden" id="hdn_item_gl_acc" value="${arr.Table[i].item_acc_id}"/>
                                                                <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                <input hidden type="text" id="RowId" value="${i + 1}" />
                                                            </div>
                                                           <div class="col-sm-1 i_Icon">
                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td ${hidefield}>
                                                       <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled value="${arr.Table[i].uom_name}">
                                                            <input type="hidden" id="hdUOMId" value="${arr.Table[i].uom_id}" style="display: none;" />
                                                        </td>
                                                        <td>
                                                            <input id="ShipNumber" value="${arr.Table[i].sh_no}"  class="form-control" autocomplete="" type="text" name="ShipNumber" placeholder="0000.00"  disabled>
                                                         </td>
                                                        <td>
                                                        <input id="ShipDate" value="${arr.Table[i].sh_date}" class="form-control" autocomplete="" type="text" name="ShipDate" placeholder="0000.00"  disabled>
                                                        <input hidden id="HdnShipDate" value='${arr.Table[i].sh_dt}' />
                                                    </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="ShipQuantity" value="${parseFloat(arr.Table[i].ship_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="ShipQuantity" placeholder="0000.00"  disabled>
                                                             </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemShipedQty">
                                                            <button type="button" id="SubItemShipedQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Shipped',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input hidden="hidden" id="Item_cost" value="${arr.Table[i].item_cost}" />
                                                            <input hidden="hidden" id="Item_Tax" value="${arr.Table[i].item_tax_amt}" />
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                            <input id="AvailableQuantity" value="${parseFloat(arr.Table[i].pending_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="AvailableQuantity" placeholder="0000.00"  disabled>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvailablQty">
                                                            <button type="button" id="SubItemAvailablQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ReturnAvlQty',event)"  data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                  <td ${hidefield}><div class=" col-sm-12 no-padding"><div class="lpo_form">
                                  <select class="form-control" id="wh_id${rowIdx}" onchange="OnChangeWarehouse(this,event)">
                                  </select><span id="sp_Warehouse" class="error-message is-visible"></span></div></div>
                                  </td>
                                                        <td> 
                                                                  <div class="col-sm-8 lpo_form no-padding">
                                                                    <input id="ReturnQuantity" autocomplete="off" value="${returnedqty}" onkeypress="return OnKeyPressRetQty(this,event);" onchange="OnChangeRetQty(this,event)" class="form-control num_right" autocomplete="" type="text" name="ReturnQuantity" placeholder="0000.00"  >
                                                                    <span id="ReturnQuantity_Error" class="error-message is-visible"></span>
                                                                   </div>
                                                                  <div class="col-sm-2 i_Icon" ${hidefield}>
                                                                          <button type="button" id="ReturnQuantityDetail" class="calculator" onclick="OnClickReturnedQtyIconBtn(event);" data-toggle="modal" data-target="#ReturnQuantityDetail" disabled><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Title_ReturnQuantityDetail").text()}"> </button>
                                                                   </div>
<div class="col-sm-2 i_Icon pl-0" id="div_SubItemReturnQty">
                        <button type="button" id="SubItemReturnQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('SRTReturnQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
                    </div>
                                                                
                                                            </td>   
                                                                    <td >
                                                                            <div class="col-sm-10" style="padding:0px;" id=''>
                                                                                <input id="InvoiceValue" class="form-control num_right" autocomplete="off" type="text" name="InvoiceValue" onchange="OnChangeInvoiceValue();"  placeholder="0000.00"  onblur="this.placeholder = '0000.00'" disabled>
                                                                            </div>
                                                                           <div class="col-sm-2 i_Icon">
                                                                                <button type="button" id="InvoiceValueCalculation" class="calculator" onclick="OnClickInvoiceValueIconBtn(event,${null});" data-toggle="modal" data-target="#InvoiceValueCalculation" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InvoiceValueCalculation").text()}"> </button>
                                                                            </div>
                                                                        </td>
                                                        <td>
                                                                <input id="ReturnValue" disabled class="form-control num_right" autocomplete="" type="text" onchange="OnChangeReturnValue();" name="ReturnValue" placeholder="0000.00"  >
                                                        </td>                                                                                                              
                                                        <td>
                                                                <textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks" @maxlength = "100", placeholder="${$("#span_remarks").text()}"    ></textarea>
                                                           
                                                        </td>
                                </tr>`);
                                BindWarehouseList(rowIdx, arr.Table[i].wh_id);
                            }
                           
                        }
                    }
                    if (arr.Table1.length > 0) {
                        var rowIdx = 0;
                        $('#hd_GL_DeatilTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table1.length; i++) {
                            $('#hd_GL_DeatilTbl tbody').append(`<tr>
<td id="compid">${arr.Table1[i].comp_id}</td>
<td id="brid">${arr.Table1[i].br_id}</td>
<td id="invno">${arr.Table1[i].inv_no}</td>
<td id="invdt">${arr.Table1[i].inv_dt}</td>
<td id="shipno">${arr.Table1[i].sh_no}</td>
<td id="shipdt">${arr.Table1[i].sh_date}</td>
<td id="itemid">${arr.Table1[i].item_id}</td>
<td id="shipqty">${arr.Table1[i].ship_qty}</td>
<td id="item_rate">${arr.Table1[i].item_rate}</td>
<td id="id">${arr.Table1[i].id}</td>
<td id="amt">${arr.Table1[i].amt}</td>
<td id="totval">${arr.Table1[i].TotalValue}</td>
<td id="type">${arr.Table1[i].Type}</td>
</tr>`);
                        }
                    }
                    $("#CurrId").val(arr.Table3[0].curr_id);
                    $("#conv_rate").val(arr.Table3[0].conv_rate);
                    $("#exch_rate").val(arr.Table3[0].conv_rate);
                    $("#exch_rate").val(arr.Table3[0].conv_rate);
                    $("#txtCurrency").val(arr.Table3[0].curr_name);
                    if (arr.Table4.length > 0) {
                       
                        $("#txtCustomer_Reference").val(arr.Table4[0].cust_ref_no);
                        $("#txtPayment_term").val(arr.Table4[0].pay_term);
                        $("#txtDelivery_term").val(arr.Table4[0].deli_term);
                        $("#txtInvoice_Heading").val(arr.Table4[0].inv_heading);
                        $("#txtRemarks").val(arr.Table4[0].remarks);
                        $("#txtPlaceofsupp").val(arr.Table4[0].placeofsupply);
                        
                    }
                    debugger;
                    //if (arr.Table2.length > 0) {
                    //    var rowIdx = 0;
                    //    for (var y = 0; y < arr.Table2.length; y++) {
                    //        var ItmId = arr.Table2[y].item_id;
                    //        var SubItmId = arr.Table2[y].sub_item_id;
                    //        var ShippedQty = arr.Table2[y].ship_qty;

                    //        $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                    //                <td><input type="text" id="ItemId" value='${ItmId}'></td>
                    //                <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                    //                <td><input type="text" id="subItemQty" value='${ShippedQty}'></td>
                    //                </tr>`);
                    //    }

                    //}
                    
                    //GetAllGLID();

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

        //var Str = $("#valueReq").text();
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
        $("#ddlDocumentNumber").css("border-color", "red");
    }
}
function BindWarehouseList(id, wh_id) {
    //debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/SalesReturn/GetWarehouseList1",
            data: {},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //GRN_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        //var PreWhVal = $("#wh_id" + id).val();
                        var PreWhVal = wh_id;
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
function OnddlDocumentNumberChange() {

    debugger;
    var DocumentNumber = $('#ddlDocumentNumber').val().trim();
    //if (DocumentNumber != "0") {
    //    document.getElementById("vmDocument_id").innerHTML = "";
    //    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
    //    $("#txtsrcdocdate").val($('#ddlDocumentNumber').val());
    //}
    //else {
    //    $("#txtsrcdocdate").val(null);
    //    $("#hdCustomerID").val(CustomerID);
    //    document.getElementById("vmDocument_id").innerHTML = $("#valueReq").text();
    //    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    //}

}
function OnTextChangeBillNumber() {

    debugger;
    var BillNUmber = $('#BillNumber').val().trim();

    if (BillNUmber != "0") {
        document.getElementById("vmbill_no").innerHTML = "";
        $("#BillNumber").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_no").innerHTML = $("#valueReq").text();
        $("#BillNumber").css("border-color", "red");
    }

}
function OnTextChangeBillDate() {

    debugger;
    var BillDate = $('#txtbilldate').val().trim();


    if (BillDate != "0") {
        document.getElementById("vmbill_date").innerHTML = "";
        $("#txtbilldate").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
        $("#txtbilldate").css("border-color", "red");
    }

}
function BindCustomerList() {
    debugger;
    //var src_type = $('#src_type').val();
    $("#ddlCustomerName").select2({
        ajax: {
            url: $("#hdCustomerList").val(),
            data: function (params) {
                var queryParameters = {
                    CustomerName: params.term,
                    src_type: $('#src_type').val(),
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
                    ErrorPage();
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

function OnChangeCust() {
    debugger;
    var CustID = $('#ddlCustomerName option:selected').val();
    $("#hdcust_id").val($("#ddlCustomerName").val());
    var src_type = $('#src_type').val();
    if (CustID != "0" && CustID != "") {
        document.getElementById("vmcust_id").innerHTML = "";
        $(".select2-container--default  .select2-selection--single").css("border-color", "#ced4da");
        $("#ddlCustomerName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "ced4da");
        
        $("#Hdn_src_type").val(src_type);
        $("#src_type").attr("disabled", true);
        if (src_type == "A") {
            $("#addbtn").css("display", "");
        }
    }
    else {
        var Custname = $('#ddlCustomerName option:selected').text();
        $("#hdSRTcust_name").val(Custname);

        document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");

        $("#src_type").attr("disabled", false);
        if (src_type == "A") {
            $("#addbtn").css("display", "none");
        }
    }
    document.getElementById("vmsrc_doc_no").innerHTML = "";
    //    $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    debugger;
    $("#txtsrcdocdate").val("");
    BindDocumentNumberList();

    //$("[aria-labelledby='select2-ddlCustomerName-container']").css("border-color", "red");
}
function OnChangeSourType() {
    debugger;
    var src_type = $('#src_type').val();
    if (src_type == "R") {
        $("#ReqGRNumber").css("display", "none");
        $("#ReqGRDate").css("display", "none");
        $("#ReqNumberOfPacks").css("display", "none");
        $("#ReqTransporterName").css("display", "none");
    }
    else {
        $("#ReqGRNumber").css("display", "");
        $("#ReqGRDate").css("display", "");
        $("#ReqNumberOfPacks").css("display", "");
        $("#ReqTransporterName").css("display", "");
    }
    if (src_type == "A") {
        $("#SReturnItmDetailsTbl").css("display","none")
        $("#AdSReturnItmDetailsTbl").css("display", "")
        $("#TaxDetailAccordian").css("display", "")
        $("#InvBillNum").css("display", "")
        $("#InvBillDt").css("display", "")

        $("#InvNoDiv").css("display", "none");
        $("#InvDateDiv").css("display", "none");
        $("#InvWiseAddItem").css("display", "none");
        $("#th_inv_value").css("display", "none");
        $("#CurrDetail").css("display", "none");
        $("#CustmGlVoucherDt").css("display", "none");
        $("#GlVoucherDt").css("display", "");
        $("#DivRoundOff").css("display", "");
        $("#DivRoundOffHeading").css("display", "");
    }
    else {
        if (src_type == "R") {
            $("#SReturnItmDetailsTbl #thUOM").css("display", "none")
            $("#SReturnItmDetailsTbl #ThWarehouse").css("display", "none")
        }
        else {
            $("#SReturnItmDetailsTbl #thUOM").css("display", "")
            $("#SReturnItmDetailsTbl #ThWarehouse").css("display", "")
        }
        $("#TaxDetailAccordian").css("display", "none")
        $("#InvBillNum").css("display", "none")
        $("#InvBillDt").css("display", "none")
        $("#InvNoDiv").css("display", "");
        $("#InvDateDiv").css("display", "");
        $("#InvWiseAddItem").css("display", "");
        $("#th_inv_value").css("display", "");
        if (src_type == "C") {
            $("#CurrDetail").css("display", "");
            $("#CustmGlVoucherDt").css("display", "");
            $("#GlVoucherDt").css("display", "none");
            $("#DivRoundOff").css("display", "none");
            $("#DivRoundOffHeading").css("display", "none");
            $("#th_oc").css("display", "none");
            //$("#chk_roundoff").prop("checked", false);
            //$("#chk_roundoff").prop("disabled", true); 
        }
        else {
            $("#CurrDetail").css("display", "none");
            $("#GlVoucherDt").css("display", "");
            $("#DivRoundOff").css("display", "");
            $("#DivRoundOffHeading").css("display", "");
            $("#CustmGlVoucherDt").css("display", "none");
            $("#th_oc").css("display", "");
        }
        BindDocumentNumberList();
    }
}
function AddAdHocItemNewRow() {
    var rowIdx = 0;
    var rowCount = $('#AdSReturnItmDetailsTbl >tbody >tr').length + 1;
    $('#AdSReturnItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                                                                    <td class="red center">
                                                                        <i class="fa fa-trash" id="deleteAdhocItem" aria-hidden="true" title="Delete"></i>
                                                                    </td>
                                                                    <td class="sr_padding"><span id="SpanRowId">${rowCount}</span>
                                                                        <input class="" type="hidden" id="SNohiddenfiled" value="${rowCount}" /></td>
                                                                    <td>
                                                                        <div class="col-sm-11 lpo_form no-padding" id="multiWrapper">
                                                                           <select class="form-control" id="ItemName${rowCount}" name="ItemName${rowCount}" onchange ="OnChangeSOItemName(event)"></select>
                                                                           <span id="ItemNameError" class="error-message is-visible"></span>
                                                                        </div>
                                                                        <div class="col-sm-1 i_Icon">
                                                                            <button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon " onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Item Information"></button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled="disabled" value="">
                                                                        <input type="hidden" id="hdUOMId" value="" />
                                                                        <input type="hidden" id="HsnNo" value="" style="display: none;">
                                                                        <input type="hidden" id="hfbatchable" value="" />
                                                                        <input type="hidden" id="hfserialable" value="" />
                                                                        <input type="hidden" id="hfexpiralble" value="" />
                                                                        <input class="" type="hidden" id="hdn_item_gl_acc" />
                                                                        <input type="hidden" id="sub_item" value="" />
                                                                        <input class="" type="hidden" id="hdItemId" />
                                                                        <input class="" type="hidden" id="ItemType" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-12 no-padding">
                                                                            <div class="lpo_form">
                                                                                  <select class="form-control" id="wh_id${rowCount}" onchange="OnChangeWarehouse(this,event)"></select>
                                                                                  <span id="wh_Error${rowCount}" class="error-message is-visible"></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-10 lpo_form no-padding lpo_form">
                                                                            <input id="ReturnQuantity" value="" class="form-control num_right" onpaste = "return CopyPasteAvoidFloat(event)" onchange ="OnChangePRItemQty(event)" autocomplete="off" type="text" name="ReturnQuantity" placeholder="0000.00">
                                                                            <span id="ReturnQuantity_Error" class="error-message is-visible"></span>
                                                                        </div>                                                                     
                                                                        <div class="col-sm-2 i_Icon pl-0" id="div_SubItemReturnQty">
                                                                            <button type="button" id="SubItemReturnQty" disabled="disabled" class="calculator subItmImg" onclick="return SubItemDetailsPopUp('AdSRTReturnQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_subitemdetail").text()}"=""> </button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="LotNumber" value="" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="${$("#span_LotNumber").text()}"  disabled="">
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" class="calculator subItmImg" id="BtnBatchDetail" onclick="return ItemStockBatchWise(this,event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                                    </td>
                                                                    <td class="center">
                                                                        <button type="button" class="calculator subItmImg" id="BtnSerialDetail" onclick="ItemStockSerialWise(this,event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled=""><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                                    </td>
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="Price" value="" onchange="OnChangeDPIItemRate(event)" onkeypress="return AmountFloatQty(this,event);" class="form-control num_right" autocomplete="off" type="text" name="Price" placeholder="0000.00">
                                                                            <span id="Price_Error" class="error-message is-visible"></span>
                                                                            <input id="item_ass_val" type="hidden" class="form-control date num_right" autocomplete="off" name="item_ass_val" placeholder="0000.00" onblur="this.placeholder='0000.00'">
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="col-sm-10 no-padding">
                                                                            <input id="item_tax_amt" disabled="" class="form-control num_right" value="" autocomplete="off" type="text" name="item_tax_amt" placeholder="0000.00">
                                                                        </div>
                                                                        <div class="col-sm-2 no-padding">
                                                                            <button type="button" class="calculator" id="BtnTxtCalculation" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}" data-original-title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <input id="ReturnValue" value="" class="form-control num_right" autocomplete="off" type="text" name="ReturnValue" placeholder="0000.00" disabled="">
                                                                    </td>
                                                                    <td>
                                                                        <textarea id="remarks" class="form-control remarksmessage" maxlength="100" name="remarks" value="" placeholder="${$("#span_remarks").text()}" onmouseover="OnMouseOver(this)" title="${$("#span_remarks").text()}"></textarea>
                                                                    </td>
                                                                </tr>`)
    BindScrpItmList(rowCount);
    BindWarehouseList(rowCount);
}
function OnChangePRItemQty(e) {
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow);
}
function AmountFloatQty(el, evt) {
    let QtyDecDigit = "#RateDigit";
    if (Cmn_FloatValueonly(el, evt, QtyDecDigit) == false) {
        return false;
    }
    return true;
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
                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        var src_type = $('#src_type').val();
                        if (src_type == "A") {
                            var PreWhVal = $("#AdSReturnItmDetailsTbl #wh_id" + id).val();
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).html(s);
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).val(IsNull(PreWhVal, '0'));
                            $("#AdSReturnItmDetailsTbl #wh_id" + id).select2();
                        }
                        else {
                            var PreWhVal = $("#SReturnItmDetailsTbl #wh_id" + id).val();
                            $("#SReturnItmDetailsTbl #wh_id" + id).html(s);
                            $("#SReturnItmDetailsTbl #wh_id" + id).val(IsNull(PreWhVal, '0'));
                            $("#SReturnItmDetailsTbl #wh_id" + id).select2();
                        }
                    }
                }
            },
        });
}
function BindScrpItmList(ID) {
    BindItemList("#ItemName", ID, "#AdSReturnItmDetailsTbl", "#SNohiddenfiled", "", "slsSrt");
}
function OnChangeSOItemName(e) {
    var clickedrow = $(e.target).closest("tr");
    var NewItm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    NewItm_ID = clickedrow.find("#ItemName" + SNo).val();
    var OldItemID = clickedrow.find("#hdItemId").val();
    clickedrow.find("#hdItemId").val(NewItm_ID);
    var ItemID = clickedrow.find("#hdItemId").val();
    if (NewItm_ID == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    Cmn_DeleteSubItemQtyDetail(OldItemID);
    DeleteItemBatchOrderQtyDetails(OldItemID);
    ClearRowDetails(e, ItemID);
    try {
        $("#HdnTaxOn").val("Tax");
        Cmn_BindUOM(clickedrow, NewItm_ID, "", "Y", "sale");

    } catch (err) {
        console.log(err.message)
    }
}
function DeleteItemBatchOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#ItemID").val();
        if (rowitem == Itemid) {
            $(this).remove();
        }
    });
}
function OnChangeDPIItemRate(e) {
    if ($("#chk_roundoff").is(":checked")) {
        $("#chk_roundoff").parent().find(".switchery").trigger("click");

    }
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow, "", "RateChange");
}
function CalculationBaseRate(clickedrow, flag, rate) {
    debugger;
    if (rate != "RateChange") {
        if ($("#chk_roundoff").is(":checked")) {
            $("#chk_roundoff").parent().find(".switchery").trigger("click");

        }
    }
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    OrderQty = clickedrow.find("#ReturnQuantity").val();
    ItemName = clickedrow.find("#ItemName" + Sno).val();
    ItmRate = clickedrow.find("#Price").val();

    if (ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate 
            clickedrow.find("#ItemNameError" + Sno).text($("#valueReq").text());
            clickedrow.find("#ItemNameError" + Sno).css("display", "block");
            clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid red");
            clickedrow.find("#Price").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ItemNameError" + Sno).css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate
            clickedrow.find("#ReturnQuantity_Error").text($("#valueReq").text());
            clickedrow.find("#ReturnQuantity_Error").css("display", "block");
            clickedrow.find("#ReturnQuantity").css("border-color", "red");
            clickedrow.find("#ReturnQuantity").val("");
            clickedrow.find("#ReturnQuantity").focus();
            clickedrow.find("#Price").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ReturnQuantity_Error").css("display", "none");
        clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");
        clickedrow.find("#ReturnQuantity").val(parseFloat(OrderQty).toFixed($("#QtyDigit").text()));
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
        //clickedrow.find("#Price_Error").text($("#valueReq").text());
        //clickedrow.find("#Price_Error").css("display", "block");
        //clickedrow.find("#Price").css("border-color", "red");
        clickedrow.find("#Price").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#Price").focus();
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#Price_Error").css("display", "none");
        clickedrow.find("#Price").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#Price").val("");
        ItmRate = 0;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#ReturnValue").val(FinVal);
        CalculateAmount();
    }
    if (ItmRate == 0) {
        clickedrow.find("#Price").val("");
    }
    else {
        clickedrow.find("#Price").val(parseFloat(ItmRate).toFixed($("#RateDigit").text()));
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
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
   if (OrderQty != 0 && OrderQty != null && OrderQty != "") {
        GetAllGLID();
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    let TaxNonRecovAmt = 0;
    let TaxRecovAmt = 0;
    var src_type = $('#src_type').val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
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
                var TaxRecov = currentRow.find("#TaxRecov").text();
                if (TaxItemID == ItmCode) {
                    if (TaxRecov == "Y") {
                        TaxRecovAmt = parseFloat(CheckNullNumber(TaxRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                    } else {
                        TaxNonRecovAmt = parseFloat(CheckNullNumber(TaxNonRecovAmt)) + parseFloat(CheckNullNumber(TaxAmount));
                    }
                }
                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID, TaxRecov: TaxRecov });
            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr').each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#ItemName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest('tr');
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                                TaxNonRecovAmt = 0;
                                TaxRecovAmt = 0;
                            }
                            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                            //var GstApplicable = $("#Hdn_GstApplicable").text();
                            //if (GstApplicable == "Y") {
                            //    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                            //        TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit);
                            //        TaxNonRecovAmt = 0;
                            //        TaxRecovAmt = 0;
                            //    }
                            //}
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                //if (currentRow.find("#TaxExempted").is(":checked")) {
                                //    if (parseFloat(ItemTaxAmt) == 0) {
                                //        CRow.remove();
                                //    }
                                //}
                                //else if (GstApplicable == "Y") {
                                //    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                //        if (parseFloat(ItemTaxAmt) == 0) {
                                //            CRow.remove();
                                //        }
                                //    }
                                //}
                                currentRow.find("#item_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                                //var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {//05-02-2025
                                //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                                //}
                                //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                                //}
                                AssessableValue = (parseFloat(currentRow.find("#item_ass_val").val())).toFixed(ValDecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF));
                                //currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(TaxRecovAmt).toFixed(ValDecDigit));
                                //currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(TaxNonRecovAmt).toFixed(ValDecDigit));
                                currentRow.find("#ReturnValue").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));

                                //let DPIQty = currentRow.find("#ord_qty_spec").val();
                                //debugger
                                //debugger
                                //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(OC_Amt)) + parseFloat(CheckNullNumber(TaxNonRecovAmt));
                                //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                                //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                                //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ValDecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        //var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                        //    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                        //}
                        //var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        currentRow.find("#ReturnValue").val(GrossAmtOR);
                        //let DPIQty = currentRow.find("#ord_qty_spec").val();
                        //debugger;
                        //debugger;
                        //let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                        //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                        //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                        //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                    }
                }
            });
            CalculateAmount();
            if (src_type == "A") {
                var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
                var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
                $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
            }
            BindTaxAmountDeatils(NewArray);
        }
        else {
            $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value=" + ItmCode + "]").closest('tr').each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;
                ItemNo = currentRow.find("#ItemName" + Sno).val();
                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                    var GrossAmtOR = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ValDecDigit);
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    //var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                    //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {//05-02-2025
                    //    OC_Amt_OR = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(ValDecDigit);
                    //}
                    //var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                    currentRow.find("#ReturnValue").val(GrossAmtOR);
                    //let DPIQty = currentRow.find("#ord_qty_spec").val();
                    //debugger;
                    //debugger;
                    //let LandedCostValue = parseFloat(CheckNullNumber(GrossAmtOR)) + parseFloat(CheckNullNumber(OC_Amt_OR)) + parseFloat(CheckNullNumber(TaxAmt));
                    //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(DPIQty));
                    //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                    //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
                }
            });
        }
    }
}
function CalculateAmount() {
    var ReturnValue = parseFloat(0).toFixed(ValDecDigit);
    var src_type = $('#src_type').val();
    if (src_type == "A") {
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            if (currentRow.find("#ReturnValue").val() == "" || currentRow.find("#ReturnValue").val() == "NaN") {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(currentRow.find("#ReturnValue").val())).toFixed(ValDecDigit);
            }
        });
        $("#TotalReturnValue").val(ReturnValue);
    }
    else {
        $("#SReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            if (currentRow.find("#ReturnValue").val() == "" || currentRow.find("#ReturnValue").val() == "NaN") {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(0)).toFixed(ValDecDigit);
            }
            else {
                ReturnValue = (parseFloat(ReturnValue) + parseFloat(currentRow.find("#ReturnValue").val())).toFixed(ValDecDigit);
            }
        });
        $("#TotalReturnValue").val(ReturnValue);
    }
}
function ClearRowDetails(e, ItemID) {
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#HsnNo").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#ReturnQuantity").val("");
    clickedrow.find("#Price").val("");
    clickedrow.find("#ReturnValue").val("");
    clickedrow.find("#remarks").val("");
    clickedrow.find("#wh_id" + SNo).val(0).trigger('change.select2');
    clickedrow.find("#wh_Error" + SNo).css("display", "none");
    clickedrow.find("[aria-labelledby='select2-wh_id" + SNo + "-container']").css("border-color", "#ced4da");
}
function AfterDeleteResetSI_ItemTaxDetail() {
    var ScrapSIItemTbl = "#AdSReturnItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var ItemName = "#ItemName";
    CMNAfterDeleteReset_ItemTaxDetailModel(ScrapSIItemTbl, SNohiddenfiled, ItemName);
}
function BindDocumentNumberList() {
    debugger;
    var CustID = $('#ddlCustomerName').val();
    var src_type = $('#src_type').val();
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/SalesReturn/GetSalesReturnSourceDocumentNoList",
        data: {
            DocumentNo: "",
            CustomerID: CustID,
            Src_Type: src_type
        },
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.Table.length > 0) {
                $("#ddlDocumentNumber option").remove();
                $("#ddlDocumentNumber optgroup").remove();
                $('#ddlDocumentNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                for (var i = 0; i < arr.Table.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr.Table[i].inv_dt}" value="${arr.Table[i].inv_dt}">${arr.Table[i].inv_no}</option>`);
                }
                var firstEmptySelect = true;
                $('#ddlDocumentNumber').select2({
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
                $('#ddlDocumentNumber').val("0").trigger('change.select2');
            }
            if (arr.Table1.length > 0) {
                $("#cust_acc_id").val(arr.Table1[0].cust_acc_id);
            }
            //if (src_type == "A") {
                if (arr.Table2.length > 0) {
                    $("#CurrId").val(arr.Table2[0].curr_id);
                    $("#conv_rate").val(arr.Table2[0].conv_rate);
                    $("#ship_add_gstNo").val(arr.Table2[0].cust_gst_no);
                    $("#Ship_StateCode").val(arr.Table2[0].bill_state_code);
                }
            //}
        },
    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    ItemInfoBtnClick(ItmCode);
}
function OnClickReturnedQtyIconBtn(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();

    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var SubItem = clickedrow.find("#sub_item").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMID = clickedrow.find("#hdUOMId").val();
    var ShipNumber = clickedrow.find("#ShipNumber").val();
    var ShipDate = clickedrow.find("#ShipDate").val();
    var HdnShipDate = clickedrow.find("#HdnShipDate").val();
    var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
    var SpanRowId = clickedrow.find("#RowId").val();
    var wh_id = clickedrow.find("#wh_id" + SpanRowId).val();
    var wh_Name = clickedrow.find("#wh_id" + SpanRowId + " option:selected").text();


    $("#IconItemName").val(ItemName);
    $("#Iconsub_item").val(SubItem);
    $("#IconUOM").val(UOM);
    $("#IconGRNNumber").val(ShipNumber);
    $("#IconGRNDate").val(ShipDate);
    $("#HdnIconGRNDate").val(HdnShipDate);
    $("#IconReturnQuantity").val(ReturnQuantity);
    $("#RQhdItemId").val(ItmCode);
    $("#RQhdUomId").val(UOMID);
    $("#ReturnWarehouse").val(wh_Name);
    $("#ReturnWarehouse_Id").val(wh_id);

    AddItemReturnedQtyDetail(ItmCode, ShipNumber, wh_id);
}
function AddItemReturnedQtyDetail(ItmID, ShipNumber, warehouse_id) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ItemID = ItmID;
    var DocumentNumber = $('#ReturnNumber').val();
    //var UomID = ItemuomID;
    var RT_Status = $('#hdSRTStatus').val();
    var src_type = $('#src_type').val();
    if (ItemID != "" && ItemID != null && ShipNumber != "" && ShipNumber != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/SalesReturn/GetShipmentItemDetail",
                data: {
                    ItemID: ItemID,
                    ShipNumber: ShipNumber,
                    SrcDocNumber: DocumentNumber,
                    RT_Status: RT_Status,
                    src_type: src_type
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);

                        if (ItmID != null && ItmID != "") {
                            arr.Table = arr.Table.filter(v => (v.item_id == ItmID));
                        }
                        $('#ReturnedQtyDetailsTbl tbody tr').remove();
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var TotalRetQty = "";
                            for (var i = 0; i < arr.Table.length; i++) {

                                var item_id = arr.Table[i].item_id;
                                var whid = arr.Table[i].wh_id;
                                var lot_id = arr.Table[i].lot_id;
                                var batch_no = arr.Table[i].batch_no;
                                var serial_no = arr.Table[i].serial_no;
                                var ShipNumber = arr.Table[i].ship_no;
                                debugger;
                                if (batch_no === null || batch_no === "null") {
                                    batch_no = "";
                                }
                                if (serial_no === null || serial_no === "null") {
                                    serial_no = "";
                                }
                                var returnqty = "";

                                var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
                                if (FReturnItemDetails != null && FReturnItemDetails != "") {
                                    if (FReturnItemDetails.length > 0) {
                                        for (j = 0; j < FReturnItemDetails.length; j++) {
                                            debugger;
                                            var ItemID = FReturnItemDetails[j].ItmCode;
                                            var ItmUomId = FReturnItemDetails[j].ItmUomId;
                                            var wh_id = FReturnItemDetails[j].wh_id;
                                            var Lot = FReturnItemDetails[j].Lot;
                                            var Batch = FReturnItemDetails[j].Batch;
                                            var Serial = FReturnItemDetails[j].Serial;
                                            var RetQty = FReturnItemDetails[j].RetQty;
                                            var IconShipNumber = FReturnItemDetails[j].IconShipNumber;
                                            var TotalQty = FReturnItemDetails[j].TotalQty;
                                            var Batchable = FReturnItemDetails[j].Batchable;
                                            var Serialble = FReturnItemDetails[j].Serialable;
                                            if (Batch === null || Batch === "null") {
                                                Batch = "";
                                            }
                                            if (Serial === null || Serial === "null") {
                                                Serial = "";
                                            }
                                            if (IconShipNumber == ShipNumber, ItemID == item_id /*&& wh_id == whid*/ && Lot == lot_id && Batch == batch_no && Serial == serial_no) {
                                                returnqty = RetQty;
                                                TotalRetQty = TotalQty;

                                            }
                                        }
                                    }
                                }
                                ++rowIdx;
                                debugger;
                                if (AvoidDot(returnqty) == false) {
                                    returnqty = "";
                                }
                                else {
                                    returnqty = parseFloat(returnqty).toFixed(QtyDecDigit);
                                }
                                if (RT_Status == "A" || RT_Status == "C") {
                                    if (returnqty > 0) {
                                        $('#ReturnedQtyDetailsTbl tbody').append(` <tr id="${rowIdx}">
                    <td style="display:none;">
                     <input id="Warehouse${rowIdx}" value="${arr.Table[i].wh_name}"  class="form-control" autocomplete="off" type="text" name="Warehouse${rowIdx}"  placeholder="Warehouse"  onblur="this.placeholder='Warehouse'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowIdx}" />                    
                     <input type="hidden" id="Item_id" value="${arr.Table[i].item_id}" />
                     <input type="hidden" id="hduom_id" value="${arr.Table[i].uom_id}" />
                     <input type="hidden" id="wh_id${rowIdx}" value="${warehouse_id}" />
                     <input type="hidden" id="Batchable" value="${arr.Table[i].i_batch}" />
                     <input type="hidden" id="Serialable" value="${arr.Table[i].i_serial}" />
                                </td>
                                 <td>
                                <input id="Lot${rowIdx}" value="${arr.Table[i].lot_id}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${rowIdx}" value="${arr.Table[i].batch_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${rowIdx}"  value="${arr.Table[i].serial_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id="MfgName${rowIdx}"  value="${IsNull(arr.Table[i].mfg_name, '')}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id="MfgMrp${rowIdx}"  value="${IsNull(arr.Table[i].mfg_mrp, '')}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id=""  value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                <input id="MfgDate${rowIdx}"  value="${IsNull(arr.Table[i].mfg_date, '')}" hidden type="text">
                                  </td>
                                <td>
                                <input id="AvailableQuantity${rowIdx}" value="${parseFloat(arr.Table[i].batch_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="1"  onblur="this.placeholder = '1'" disabled>
                                </td>
                                 <td><div class="lpo_form">
                                <input id="IconReturnQtyinput${rowIdx}" value="${returnqty}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressRetQty(this,event);" onchange="OnChangePopupReturnQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanRetQty${rowIdx}" class="error-message is-visible"></span></div>
                                 </td>
                </tr>`);
                                    }                                   
                                }
                                else {
                                    $('#ReturnedQtyDetailsTbl tbody').append(` <tr id="${rowIdx}">
                    <td style="display:none;">
                     <input id="Warehouse${rowIdx}" value="${arr.Table[i].wh_name}"  class="form-control" autocomplete="off" type="text" name="Warehouse${rowIdx}"  placeholder="Warehouse"  onblur="this.placeholder='Warehouse'" disabled>
                     <input type="hidden" id="SNohiddenfiled" value="${rowIdx}" />                    
                     <input type="hidden" id="Item_id" value="${arr.Table[i].item_id}" />
                     <input type="hidden" id="hduom_id" value="${arr.Table[i].uom_id}" />
                     <input type="hidden" id="wh_id${rowIdx}" value="${warehouse_id}" />
                     <input type="hidden" id="Batchable" value="${arr.Table[i].i_batch}" />
                     <input type="hidden" id="Serialable" value="${arr.Table[i].i_serial}" />
                                </td>
                                 <td>
                                <input id="Lot${rowIdx}" value="${arr.Table[i].lot_id}" class="form-control" autocomplete="off" type="text"  placeholder="${$("#span_LotNumber").text()}"  onblur="this.placeholder = '${$("#span_LotNumber").text()}'" disabled>
                                </td>

                                 <td><input id="Batch1${rowIdx}" value="${arr.Table[i].batch_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_BatchDetail").text()}"  onblur="this.placeholder = '${$("#span_BatchDetail").text()}'" disabled>
                                  </td>
                                   <td>
                                <input id="Serial1${rowIdx}"  value="${arr.Table[i].serial_no}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id="MfgName${rowIdx}"  value="${IsNull(arr.Table[i].mfg_name,'')}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id="MfgMrp${rowIdx}"  value="${IsNull(arr.Table[i].mfg_mrp,'')}" class="form-control" autocomplete="off" type="text" name="" placeholder="${$("#span_SerialDetail").text()}"  onblur="this.placeholder = '${$("#span_SerialDetail").text()}'" disabled>
                                  </td>
                                    <td>
                                <input id=""  value="${Cmn_FormatDate_ddmmyyyy(arr.Table[i].mfg_date)}" class="form-control" autocomplete="off" type="text" name=""  disabled>
                                <input id="MfgDate${rowIdx}"  value="${IsNull(arr.Table[i].mfg_date,'')}" hidden type="text">
                                  </td>
                                <td>
                                <input id="AvailableQuantity${rowIdx}" value="${parseFloat(arr.Table[i].batch_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="" placeholder="1"  onblur="this.placeholder = '1'" disabled>
                                </td>
                                 <td><div class="lpo_form">
                                <input id="IconReturnQtyinput${rowIdx}" value="${returnqty}" class="form-control num_right" autocomplete="off" maxlength="10" onkeypress="return OnKeyPressRetQty(this,event);" onchange="OnChangePopupReturnQty(this,event)"  type="text" name="" placeholder="0000.00"  onblur="this.placeholder = '0000.00'">
                                    <span id="SpanRetQty${rowIdx}" class="error-message is-visible"></span></div>
                                 </td>
                </tr>`);
                                }                              
                                if (AvoidDot(TotalRetQty) == false) {
                                    TotalRetQty = 0;
                                }
                                //TotalRetQty = parseFloat(TotalRetQty) + parseFloat(returnqty);
                                DisableDetail(RT_Status);
                            }
                            if (TotalRetQty == "") {
                                $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                            }
                            else {
                                $("#TotalReturnQty").text(parseFloat(TotalRetQty).toFixed(QtyDecDigit));
                            }
                        }
                        else {
                            $("#ReturnedQtyDetailsTbl >tbody >tr").remove();
                            $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                        }
                        //if (arr.Table[0].sub_item == "N") {
                        //    $("#SubItemReturnQty").attr("disabled", true);
                        //    $("#SubItemReturnQty").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        //}
                        //else {
                        //    $("#SubItemReturnQty").attr("disabled", false);
                        //    $("#SubItemReturnQty").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                        //}
                    }
                    else {
                        $("#ReturnedQtyDetailsTbl >tbody >tr").remove();
                        $('#TotalReturnQty').text(parseFloat(0).toFixed(ValDecDigit));
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }
}
function OnChangeWarehouse(el, evt) {
    debugger;
    var src_type = $('#src_type').val();
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#RowId").val();
    var hdItemId = clickedrow.find("#hdItemId").val();
    if (src_type == "A") {
        var Index = clickedrow.find("#SNohiddenfiled").val();
    }
    var Warehouse = clickedrow.find("#wh_id" + Index).val();
   
    //var Warehouse = clickedrow.find("#Warehouse" + Sno).val();
    //var ReturnQty = clickedrow.find("#IconReturnQtyinput" + Sno).val();
    if (Warehouse == "0") {
        clickedrow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "red");
        if (src_type == "A") {
            clickedrow.find("#wh_Error" + Index).text($("#valueReq").text());
            clickedrow.find("#wh_Error" + Index).css("display", "block");
        }
        else {
            clickedrow.find("#sp_Warehouse").text($("#valueReq").text());
            clickedrow.find("#sp_Warehouse").css("display", "block");
        }
    }
    else {
        clickedrow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "#ced4da");
        if (src_type == "A") {
            clickedrow.find("#wh_Error" + Index).text("");
            clickedrow.find("#wh_Error" + Index).css("display", "none");
        }
        else {
            clickedrow.find("#sp_Warehouse").text("");
            clickedrow.find("#sp_Warehouse").css("display", "none");
        }
    }
    var row = $("#ReturnedQtyDetailsTbl >tbody >tr #Item_id[value='" + hdItemId + "']").closest('tr');
    var Sno = row.find("#SNohiddenfiled").val();
    row.find("#wh_id" + Sno).val(Warehouse);

    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            for (j = 0; j < FReturnItemDetails.length; j++) {
                debugger;
                var ItmCode = FReturnItemDetails[j].ItmCode;
                var ItmUomId = FReturnItemDetails[j].ItmUomId;
                var wh_id = Warehouse;
                var Lot = FReturnItemDetails[j].Lot;
                var Batch = FReturnItemDetails[j].Batch;
                var Serial = FReturnItemDetails[j].Serial;
                var RetQty = FReturnItemDetails[j].RetQty;
                var IconShipNumber = FReturnItemDetails[j].IconShipNumber;
                var IconShipDate = FReturnItemDetails[j].IconShipDate;
                var TotalQty = FReturnItemDetails[j].TotalQty;
                var Batchable = FReturnItemDetails[j].Batchable;
                var Serialable = FReturnItemDetails[j].Serialable;
                if (Batch === null || Batch === "null") {
                    Batch = "";
                }
                if (Serial === null || Serial === "null") {
                    Serial = "";
                }
                if (ItmCode == hdItemId) {
                    NewArr.push({ TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty })
                }
                else {
                    NewArr.push({ TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate, ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: FReturnItemDetails[j].wh_id, Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty })
                }
               
            }
            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));
        }
    }
    //$('#SReturnItmDetailsTbl tbody tr').each(function () {
        //var Crow = $(this);
    var hdItemId = clickedrow.find("#hdItemId").val();
    var sub_item = clickedrow.find("#sub_item").val();
            if (sub_item == "Y") {
                var row = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value='" + hdItemId + "']").closest('tr');
                row.each(function () {
                    var row = $(this).closest("tr");
                    row.find("#subItemWhId").val(Warehouse);

                });
            }        
    //});




    //var ItemId = $("#RQhdItemId").val();
    //$('#SReturnItmDetailsTbl tbody tr').each(function () {
    //    var Crow = $(this);
    //    var hdItemId = Crow.find("#hdItemId").val();
    //    var sub_item = Crow.find("#sub_item").val();
    //    if (hdItemId == ItemId) {
    //        if (sub_item == "Y") {
    //            $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value='" + ItemId + "']").closest('tr').each(function () {
    //                var row = $(this).closest("tr");

    //                row.find("#subItemWhId").val(Warehouse);

    //            });
    //        }
    //    }
    //});
}
function DisableDetail(RT_Status) {
    debugger;
    var PageDisable = $("#PageDisable").val();
    // var sessionval = sessionStorage.getItem("EditBtnClickSR");
    //if (RT_Status == "") {
    //    sessionval = "Y";
    //}

    if (RT_Status == "D" || RT_Status == "") {
        $('#ReturnedQtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            if (PageDisable == "N") {
                currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", false);
            }
            else {
                currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", true);
            }
        });

        if (PageDisable == "N") {

            $("#btnRqSaveDiscardOnly").css("display", "");
            $("#btnRqCloseOnly").css("display", "none");

            $("#btnIvSaveDiscardOnly").css("display", "");
            $("#btnIvCloseOnly").css("display", "none");

            $("#SaveAndExitBtn").attr("disabled", false);
            $("#DiscardAndExit").attr("disabled", false);
            //$("#InvoicePopupReturnValue").attr("disabled", false);
            $("#InvoiceSaveAndExitBtn").attr("disabled", false);
            $("#InvoiceDiscardAndExit").attr("disabled", false);
        }
        else {

            $("#btnRqSaveDiscardOnly").css("display", "none");
            $("#btnRqCloseOnly").css("display", "");

            $("#btnIvSaveDiscardOnly").css("display", "none");
            $("#btnIvCloseOnly").css("display", "");

            $("#SaveAndExitBtn").attr("disabled", true);
            $("#DiscardAndExit").attr("disabled", true);
            $("#InvoicePopupReturnValue").attr("disabled", true);
            $("#InvoiceSaveAndExitBtn").attr("disabled", true);
            $("#InvoiceDiscardAndExit").attr("disabled", true);
        }
    }
    else {
        $('#ReturnedQtyDetailsTbl tbody tr').each(function () {
            var currentrow = $(this);
            var SNo = currentrow.find("#SNohiddenfiled").val()
            currentrow.find("#IconReturnQtyinput" + SNo).attr("disabled", true);
        });

        $("#btnRqSaveDiscardOnly").css("display", "none");
        $("#btnRqCloseOnly").css("display", "");

        $("#btnIvSaveDiscardOnly").css("display", "none");
        $("#btnIvCloseOnly").css("display", "");

        $("#SaveAndExitBtn").attr("disabled", true);
        $("#DiscardAndExit").attr("disabled", true);
        $("#InvoicePopupReturnValue").attr("disabled", true);
        $("#InvoiceSaveAndExitBtn").attr("disabled", true);
        $("#InvoiceDiscardAndExit").attr("disabled", true);
    }
}
function OnChangePopupReturnQty(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var RetQty = clickedrow.find("#IconReturnQtyinput" + Sno).val();
    var Available = parseFloat(clickedrow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
    if ((RetQty == "") || (RetQty == null) || (RetQty == 0) || (RetQty == "NaN")) {
        RetQty = 0;
    }
    if (parseFloat(RetQty) <= parseFloat(Available)) {

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);
        clickedrow.find("#SpanRetQty" + Sno).css("display", "none");
        clickedrow.find("#IconReturnQtyinput" + Sno).css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#SpanRetQty" + Sno).text($("#ExceedingQty").text());
        clickedrow.find("#SpanRetQty" + Sno).css("display", "block");
        clickedrow.find("#IconReturnQtyinput" + Sno).css("border-color", "red");
        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IconReturnQtyinput" + Sno).val(test);
    }
    CalTotalReturnQty(el, evt);
}
function CalTotalReturnQty(el, evt) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var TotalQty = parseFloat(0).toFixed(QtyDecDigit);

    $("#ReturnedQtyDetailsTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        var Sno = CRow.find("#SNohiddenfiled").val();
        var RtnQty = CRow.find("#IconReturnQtyinput" + Sno).val();
        if (RtnQty != "" && RtnQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(RtnQty));
        }
    });

    $("#TotalReturnQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));

};
function OnClickInvoiceValueIconBtn(evt, vChange) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var RT_Status = $('#hdSRTStatus').val();
    var clickedrow = $(evt.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItemName = clickedrow.find("#ItemName").val();
    var UOM = clickedrow.find("#UOM").val();
    var ShipNumber = clickedrow.find("#ShipNumber").val();
    var ShipDate = clickedrow.find("#ShipDate").val();
    var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
    var InvoiceNo = $('#ddlDocumentNumber option:selected').text();

    $("#SpanReturnValuePopup").css("display", "none");
    $("#InvoicePopupReturnValue").css("border-color", "#ced4da");

    $("#hd_doc_no").val(InvoiceNo);
    $("#InvoiceIconItemName").val(ItemName);
    $("#InvoiceItem_id").val(ItmCode)
    $("#InvoiceIconUOM").val(UOM);
    $("#InvoiceIconGRNNumber").val(ShipNumber);
    $("#InvoiceIconGRNDate").val(ShipDate);
    $("#InvoiceIconReturnQuantity").val(ReturnQuantity);

    GetInvoiceValueCalculationDetail(evt, ItmCode, InvoiceNo, ShipNumber, ReturnQuantity, vChange);
    DisableDetail(RT_Status);
}
function GetInvoiceValueCalculationDetail(evt, ItmCode, InvoiceNo, ShipNumber, ReturnQuantity, vChange) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount   
    var clickedrow = $(evt.target).closest("tr");
    var src_type = $('#src_type').val();
    var ItemID = ItmCode;
    if (ItemID != "" && ItemID != null && ShipNumber != "" && ShipNumber != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/SalesReturn/GetSalesInvoiceItemDetail",
                data: {
                    ItemID: ItemID,
                    InvoiceNo: InvoiceNo,
                    ShipNumber: ShipNumber,
                    src_type: src_type
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                ++rowIdx;
                                debugger;
                                var landedOC;
                                var landedtax;
                                var shipqty = arr.Table[i].ship_qty;
                                var iteshipate = arr.Table[i].item_rate;
                                //var item_rate_bs = arr.Table[i].item_rate_bs;

                                if (iteshipate == null) {
                                    $("#ItemCost").val(parseFloat(0).toFixed(ValDecDigit));
                                    iteshipate = 0;
                                }
                                else {
                                    $("#ItemCost").val(parseFloat(iteshipate).toFixed(RateDecDigit));
                                }
                                if (arr.Table[i].item_tax_amt == null) {
                                    //$("#TaxAmount").val(parseFloat(0).toFixed(ValDecDigit));
                                    $("#ItemWiseTaxAmount").val(parseFloat(0).toFixed(ValDecDigit));
                                    landedtax = 0;
                                }
                                else {
                                    var totaltaxamt = parseFloat(arr.Table[i].item_tax_amt).toFixed(ValDecDigit);

                                    landedtax = (totaltaxamt / shipqty)
                                    //$("#TaxAmount").val(parseFloat(landedtax).toFixed(ValDecDigit));
                                    $("#ItemWiseTaxAmount").val(parseFloat(landedtax).toFixed(ValDecDigit));
                                }

                                if (arr.Table[i].item_oc_amt == null) {
                                    $("#OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                                    landedOC = 0;
                                }
                                else {
                                    var totalOc = parseFloat(arr.Table[i].item_oc_amt).toFixed(ValDecDigit);
                                    landedOC = (totalOc / shipqty)
                                    $("#OtherCharges").val(parseFloat(landedOC).toFixed(ValDecDigit));
                                }
                                debugger;
                                var landedrate = iteshipate + landedtax + landedOC;
                                var invoiceAmt = (ReturnQuantity * landedrate);
                                $("#NetRate").val(parseFloat(landedrate).toFixed(ValDecDigit));
                                $("#InvoicePopupInvoiceAmount").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));

                                var ReturnQuantity1 = $("#InvoiceIconReturnQuantity").val();
                                var taxValue = parseFloat(landedtax) * parseFloat(ReturnQuantity1);
                                $("#TaxValue").val(parseFloat(CheckNullNumber(taxValue)).toFixed(ValDecDigit));

                                if (clickedrow.find("#ReturnValue").val() != null && clickedrow.find("#ReturnValue").val() != "") {
                                    $("#InvoicePopupReturnValue").val(parseFloat(clickedrow.find("#ReturnValue").val()).toFixed(ValDecDigit));
                                }
                                else {
                                    $("#InvoicePopupReturnValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                    clickedrow.find("#ReturnValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                    clickedrow.find("#InvoiceValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                }
                                if (vChange === "change") {
                                    clickedrow.find("#ReturnValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                    clickedrow.find("#InvoiceValue").val(parseFloat(invoiceAmt).toFixed(ValDecDigit));
                                }

                                OnChangeInvoiceValue();
                                OnChangeReturnValue();
                                var ItemCode = clickedrow.find("#hdItemId").val();
                                var ShipNo = clickedrow.find("#ShipNumber").val();
                                if ($("#hdSRTStatus").val() != "A" && $("#hdSRTStatus").val() != "C") {
                                    UpdateGLValue(ItemCode, ShipNo, ReturnQuantity)
                                }

                            }

                        }

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }
}


function OnclickTaxAmtDetailIcon(e) {/*Add by hina sharma on 10-12-2024*/
    debugger;
    var CurrentRow = $(e.target).closest("tr");
    var InvoiceNo = $("#hd_doc_no").val();
    var ItmCode = $("#InvoiceItem_id").val();
    var ShipNumber = $("#InvoiceIconGRNNumber").val();
    var ReturnQuantity = $("#InvoiceIconReturnQuantity").val();
    //var TaxAmt = $("#TaxAmount").val();
    var TaxAmt = $("#ItemWiseTaxAmount").val();
    var src_type = $('#src_type').val();
    if (TaxAmt != null && TaxAmt != "") {
        try {
            $.ajax({
                async: true,
                type: "POST",
                url: "/ApplicationLayer/SalesReturn/GetTaxAmountDetail",
                data: {
                    ItmCode: ItmCode,
                    InvoiceNo: InvoiceNo,
                    ShipNumber: ShipNumber,
                    ReturnQuantity: ReturnQuantity,
                    src_type: src_type
                },
                success: function (data) {
                    debugger;
                    $('#TaxAmountDetailsPopup').html(data);
                    //cmn_apply_datatable("#tbl_PaidAmtDetails");
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }
            });

        }

        catch (err) {
            debugger;
            console.log("Trial Balance Error : " + err.message);
        }
    }
    
}

function CheckItemLevelValidations() {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity 
    var src_type = $('#src_type').val();
    if (src_type == "A") {
        //var length = $("#AdSReturnItmDetailsTbl >tbody >tr").length;
        //if (length == "0") {
        //    swal("", $("#noitemselectedmsg").text(), "warning");
        //    ErrorFlag = "Y";
        //}
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var ItemType = currentRow.find("#ItemType").val();
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
            if (ItemType != "Service") {
                if (currentRow.find("#wh_id" + Sno).val() == "0" || currentRow.find("#wh_id" + Sno).val() == "" || currentRow.find("#wh_id" + Sno).val() == null) {
                    currentRow.find("[aria-labelledby='select2-wh_id" + Sno + "-container']").css("border-color", "red");
                    currentRow.find("#wh_Error" + Sno).text($("#valueReq").text());
                    currentRow.find("#wh_Error" + Sno).css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("[aria-labelledby='select2-wh_id" + Sno + "-container']").css("border-color", "#ced4da");
                    currentRow.find("#wh_Error" + Sno).text("");
                    currentRow.find("#wh_Error" + Sno).css("display", "none");
                }
            }          
            if (currentRow.find("#ReturnQuantity").val() == "" || parseFloat(currentRow.find("#ReturnQuantity").val()) == parseFloat("0")) {
                currentRow.find("#ReturnQuantity_Error").text($("#valueReq").text());
                currentRow.find("#ReturnQuantity_Error").css("display", "block");
                currentRow.find("#ReturnQuantity").css("border-color", "red");
                //$("#ReturnQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReturnQuantity_Error").css("display", "none");
                currentRow.find("#ReturnQuantity").css("border-color", "#ced4da");
            }
            if (currentRow.find("#Price").val() == "" || parseFloat(currentRow.find("#Price").val()) == parseFloat("0")) {
                currentRow.find("#Price_Error").text($("#valueReq").text());
                currentRow.find("#Price_Error").css("display", "block");
                currentRow.find("#Price").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#Price_Error").css("display", "none");
                currentRow.find("#Price").css("border-color", "#ced4da");
            }
        });
    }
    else {
        $("#SReturnItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Index = currentRow.find("#RowId").val();
            if (currentRow.find("#ReturnQuantity").val() == "" || parseFloat(currentRow.find("#ReturnQuantity").val()) == parseFloat("0")) {
                currentRow.find("#ReturnQuantity_Error").text($("#valueReq").text());
                currentRow.find("#ReturnQuantity_Error").css("display", "block");
                currentRow.find("#ReturnQuantity").css("border-color", "red");
                //$("#ReturnQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReturnQuantity_Error").css("display", "none");
                currentRow.find("#ReturnQuantity").css("border-color", "#ced4da");
            }
            if (src_type != "R") {
                if (currentRow.find("#wh_id" + Index).val() == "0" || currentRow.find("#wh_id" + Index).val() == "" || currentRow.find("#wh_id" + Index).val() == null) {
                    currentRow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "red");
                    currentRow.find("#sp_Warehouse").text($("#valueReq").text());
                    currentRow.find("#sp_Warehouse").css("display", "block");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("[aria-labelledby='select2-wh_id" + Index + "-container']").css("border-color", "#ced4da");
                    currentRow.find("#sp_Warehouse").text("");
                    currentRow.find("#sp_Warehouse").css("display", "none");
                }
            }
            var ReturnQuantity = CheckNullNumber(currentRow.find("#ReturnQuantity").val());
            var ReturnQuantity = parseFloat(ReturnQuantity).toFixed(parseFloat(QtyDecDigit));
            var ShipQuantity = parseFloat(currentRow.find("#ShipQuantity").val()).toFixed(parseFloat(QtyDecDigit));
            if (parseFloat(ReturnQuantity) <= parseFloat(ShipQuantity)) {

            }
            else {
                currentRow.find("#ReturnQuantity_Error").text($("#ExceedingQty").text());
                currentRow.find("#ReturnQuantity_Error").css("display", "block");
                currentRow.find("#ReturnQuantity").css("border-color", "red");

                var test = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
                currentRow.find("#ReturnQuantity").val(test);
                currentRow.find("#ReturnQuantityDetail").attr("disabled", true);
                ErrorFlag = "Y";
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
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    //var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#SReturnItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ReturnQuantity = clickedrow.find("#ReturnQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var ShipNumberCheck = clickedrow.find("#ShipNumber").val();
        var TotalItemBatchQty = parseFloat("0");


        let NewArr = [];
        var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
        if (FReturnItemDetails != null && FReturnItemDetails != "") {
            if (FReturnItemDetails.length > 0) {
                for (i = 0; i < FReturnItemDetails.length; i++) {

                    var ItemID = FReturnItemDetails[i].ItmCode;
                    var ShipNumber = FReturnItemDetails[i].IconShipNumber;

                    if (ItemID == ItemId && ShipNumber == ShipNumberCheck) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(FReturnItemDetails[i].RetQty);
                    }
                }
            }
        }

        // $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
        //         debugger;
        //     var currentRow = $(this);
        //     var Sno = currentRow.find("#SNohiddenfiled").val();   
        //     var LotRetQty = currentRow.find('#IconReturnQtyinput' + Sno).val();
        //     var Lotitemid = currentRow.find('#Item_id').val();
        //     var Lotuomid = currentRow.find('#hduom_id').val();
        //     if (ItemId == Lotitemid && UOMId == Lotuomid) {
        //         TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(LotRetQty);
        //     }
        //});
        if (parseFloat(ReturnQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
            //clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");
            clickedrow.find("#ReturnQuantityDetail").css("border", "");
        }
        else {
            //clickedrow.find("#ReturnQuantity").css("border-color", "red");
            clickedrow.find("#ReturnQuantityDetail").css("border", "1px solid red");
            //BatchableFlag = "Y";
            EmptyFlag = "Y";
        }
        // }
    });
    if (EmptyFlag == "Y") {
        swal("", $("#BatchSerialqtydoesnotmatchwithreturnqty").text(), "warning");
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
function Check_VoucherValidations() {
    debugger;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    var DrTotal = $("#DrTotalInBase").text();
    var CrTotal = $("#CrTotalInBase").text();
    //var DrTotal = $("#DrTotal").text();
    //var CrTotal = $("#CrTotal").text();
    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        var AccID = currentRow.find('#Acc_name_' + rowid).val();
        if (AccID != '0' && AccID != "") {
            ErrorFlag = "N";
        }
        else {
            swal("", $("#GLPostingNotFound").text(), "warning");
            ErrorFlag = "Y";
            return false;
        }

    });
    if (DrTotal == '' || DrTotal == 'NaN') {
        DrTotal = 0;
    }
    if (CrTotal == '' || CrTotal == 'NaN') {
        CrTotal = 0;
    }
    debugger;
    if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    }
    else {
        swal("", $("#DebtCredtAmntMismatch").text(), "warning");
        ErrorFlag = "Y";
    }
    if (DrTotal == "0" && CrTotal == "0") {
        swal("", $("#GLPostingNotFound").text(), "warning");
        ErrorFlag = "Y";
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ddlDocumentNumberSelect() {
    debugger;
    var SourceDocumentDate = $('#ddlDocumentNumber').val().trim();

    var date = SourceDocumentDate.split("-");

    var FDate = date[2] + '-' + date[1] + '-' + date[0];

    $("#txtsrcdocdate").val(FDate);

    var DocumentNumber = $('#ddlDocumentNumber').val();
    if (DocumentNumber != "0" && DocumentNumber != "") {
        document.getElementById("vmsrc_doc_no").innerHTML = "";
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "#ced4da");
        $("#ddlDocumentNumber").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmsrc_doc_no").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
        $("#ddlDocumentNumber").css("border-color", "red");
    }

}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}

function OnKeyPressRetQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#ReturnQuantity_Error").css("display", "none");
    clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");


    return true;
}
function OnChangeRetQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity   
    var clickedrow = $(evt.target).closest("tr");
    var Rqtydt = clickedrow.find("#ReturnQuantity").val();
    if ($("#chk_roundoff").is(":checked")) {
        $("#chk_roundoff").parent().find(".switchery").trigger("click");
    }
    if (AvoidDot(Rqtydt) == false) {
        Rqtydt = 0;
    }
    var ReturnQuantity = parseFloat(Rqtydt).toFixed(parseFloat(QtyDecDigit));
    var ShipQuantity = parseFloat(clickedrow.find("#ShipQuantity").val()).toFixed(parseFloat(QtyDecDigit));

    if (parseFloat(ReturnQuantity) <= parseFloat(ShipQuantity)) {

        if ((ReturnQuantity == "") || (ReturnQuantity == null) || (ReturnQuantity == 0)) {
            var test = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#ReturnQuantity").val(test);
            clickedrow.find("#ReturnQuantityDetail").attr("disabled", true);

            clickedrow.find("#ReturnQuantity_Error").text($("#valueReq").text());
            clickedrow.find("#ReturnQuantity_Error").css("display", "block");
            clickedrow.find("#ReturnQuantity").css("border-color", "red");
            OnClickInvoiceValueIconBtn(evt, "change");
        }
        else {
            var test = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#ReturnQuantity").val(test);
            clickedrow.find("#ReturnQuantityDetail").attr("disabled", false);
            clickedrow.find("#ReturnQuantity_Error").css("display", "none");
            clickedrow.find("#ReturnQuantity").css("border-color", "#ced4da");
            OnClickInvoiceValueIconBtn(evt, "change");

            //var ItemCode = clickedrow.find("#hdItemId").val();
            //var ShipNo = clickedrow.find("#ShipNumber").val();
            //UpdateGLValue(ItemCode, ShipNo, ReturnQuantity)
        }
    }
    else {


        clickedrow.find("#ReturnQuantity_Error").text($("#ExceedingQty").text());
        clickedrow.find("#ReturnQuantity_Error").css("display", "block");
        clickedrow.find("#ReturnQuantity").css("border-color", "red");

        var test = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#ReturnQuantity").val(test);
        clickedrow.find("#ReturnQuantityDetail").attr("disabled", true);

    }
    clickedrow.find("#ReturnQuantityDetail").css("border", "");
}
function OnChangeRetValue(el, evt) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(evt.target).closest("tr");
    var ReturnValue = parseFloat($("#InvoicePopupReturnValue").val()).toFixed(parseFloat(ValDecDigit));

    if ((ReturnValue == "NaN") || (ReturnValue == null) || (ReturnValue == 0)) {
        var test = parseFloat(parseFloat("0")).toFixed(parseFloat(ValDecDigit));
        $("#InvoicePopupReturnValue").val(test);
        $("#SpanReturnValuePopup").text($("#valueReq").text());
        $("#SpanReturnValuePopup").css("display", "block");
        $("#InvoicePopupReturnValue").css("border-color", "red");
    }
    else {
        var test = parseFloat(parseFloat(ReturnValue)).toFixed(parseFloat(ValDecDigit));
        $("#InvoicePopupReturnValue").val(test);
        $("#SpanReturnValuePopup").css("display", "none");
        $("#InvoicePopupReturnValue").css("border-color", "#ced4da");
    }
}

function functionConfirm(event) {
    debugger;
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        debugger;
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
function OnClickSupplierInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    ItmCode = clickedrow.find("#hdItemId").val();
    var Supp_id = $('#ddlCustomerName').val();
    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id)

}

function OnClickSaveAndExitRetQty_Btn() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity     
    var IconShipNumber = $("#IconGRNNumber").val();

    var IShipDate = $("#IconGRNDate").val().trim();
    var date = IShipDate.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    var IconShipDate = FDate;
    var IconReturnQuantity = $("#IconReturnQuantity").val();
    var ItmCode = $("#RQhdItemId").val();
    var ItmUomId = $("#RQhdUomId").val();
    var Batchable = $("#Batchable").val();
    var Serialable = $("#Serialable").val();
    var TotalQty = $("#TotalReturnQty").text();

    var error = "N";
    $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        debugger;
        var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
        if (RetQty == "" || RetQty == null || RetQty == 0 || RetQty == "NaN") {
            RetQty = 0;
        }
        var Available = parseFloat(currentRow.find("#AvailableQuantity" + Sno).val()).toFixed(parseFloat(QtyDecDigit));
        if (parseFloat(RetQty) <= parseFloat(Available)) {
            var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#IconReturnQtyinput" + Sno).val(test);
        }
        else {
            currentRow.find("#SpanRetQty" + Sno).text($("#ExceedingQty").text());
            currentRow.find("#SpanRetQty" + Sno).css("display", "block");
            currentRow.find("#IconReturnQtyinput" + Sno).css("border-color", "red");
            var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
            currentRow.find("#IconReturnQtyinput" + Sno).val(test);
            error = "Y";
            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
        }
        debugger;

    });
    if (error == "Y") {
        $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        if (parseFloat(IconReturnQuantity) == parseFloat(TotalQty)) {
            let clickedrow = $("#SReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + ItmCode + "']").closest("tr");
            clickedrow.find("#ReturnQuantityDetail").css("border", "");
            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "modal");
            //if (ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", ItmCode, IconShipNumber) == false) {
            //    error = "Y";
            //    $("#SaveAndExitBtn").attr("data-dismiss", "");
            //    swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
            //    //return false
            //}
            //else {
            //    $("#SaveAndExitBtn").attr("data-dismiss", "modal");
            //    //return true;
            //}
        }
        else {
            error = "Y";
            $("#ReturnQuantityDetail #SaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#ReturnQtyDoesNotMatchSelectedQty").text(), "warning");
            return false
        }
    }
    let NewArr = [];
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            for (i = 0; i < FReturnItemDetails.length; i++) {
                var ItemID = FReturnItemDetails[i].ItmCode;
                var ShipNumber = FReturnItemDetails[i].IconShipNumber;
                if (ItemID == ItmCode && ShipNumber == IconShipNumber) {
                }
                else {
                    NewArr.push(FReturnItemDetails[i]);
                }
            }
            $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();

                debugger;
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
                var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
                var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
                var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
                var TotalQty = $("#TotalReturnQty").text();

                NewArr.push({
                    TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate, ItmCode: ItmCode
                    , ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id, Lot: Lot
                    , Batch: Batch, Serial: Serial, RetQty: RetQty
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });

            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(NewArr));

        }
        else {
            var ReturnItemList = [];
            $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                debugger;
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
                var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
                var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
                var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
                var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
                var TotalQty = $("#TotalReturnQty").text();
                ReturnItemList.push({
                    TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate
                    , ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id
                    , Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });

            sessionStorage.removeItem("ItemRetQtyDetails");
            sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(ReturnItemList));
        }
    }
    else {
        var ReturnItemList = [];
        $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            debugger;
            var wh_id = currentRow.find("#wh_id" + Sno).val();
            var Lot = currentRow.find("#Lot" + Sno).val();
            var Batch = currentRow.find("#Batch1" + Sno).val();
            var Serial = currentRow.find("#Serial1" + Sno).val();
            var AvaQty = currentRow.find("#AvailableQuantity" + Sno).val();
            var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();
            var mfg_name = currentRow.find("#MfgName" + Sno).val() || '';
            var mfg_mrp = currentRow.find("#MfgMrp" + Sno).val() || '';
            var mfg_date = currentRow.find("#MfgDate" + Sno).val() || '';
            var TotalQty = $("#TotalReturnQty").text();
            ReturnItemList.push({
                TotalQty: TotalQty, IconShipNumber: IconShipNumber, IconShipDate: IconShipDate
                , ItmCode: ItmCode, ItmUomId: ItmUomId, Batchable: Batchable, Serialable: Serialable, wh_id: wh_id
                , Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
            })
        });
        sessionStorage.removeItem("ItemRetQtyDetails");
        sessionStorage.setItem("ItemRetQtyDetails", JSON.stringify(ReturnItemList));
    }

}
function OnClickInvoiceSaveAndExitBtn() {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var ValDecDigit = $("#ValDigit").text();
    var ItmName = $("#IconItemName").val();
    var ItemUOM = $("#IconUOM").val();
    var InvoiceIconShipNumber = $("#InvoiceIconGRNNumber").val();
    var InvoiceIconShipDate = $("#InvoiceIconGRNDate").val();
    var InvoiceIconReturnQuantity = $("#InvoiceIconReturnQuantity").val();
    var InvoicePopupReturnValue = $("#InvoicePopupReturnValue").val();
    var InvoiceItem_id = $("#InvoiceItem_id").val();
    debugger;
    if (InvoicePopupReturnValue == "" || InvoicePopupReturnValue == 0) {

        $("#SpanReturnValuePopup").text($("#valueReq").text());
        $("#SpanReturnValuePopup").css("display", "block");
        $("#InvoicePopupReturnValue").css("border-color", "red");
        $("#InvoiceSaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        $("#SpanReturnValuePopup").css("display", "none");
        $("#InvoicePopupReturnValue").css("border-color", "#ced4da");

        $("#InvoiceSaveAndExitBtn").attr("data-dismiss", "modal");
    }
    $("#SReturnItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemID = currentRow.find("#hdItemId").val()
        var ShipNo = currentRow.find("#ShipNumber").val()

        if (ItemID == InvoiceItem_id && ShipNo == InvoiceIconShipNumber) {
            currentRow.find("#ReturnValue").val(parseFloat(InvoicePopupReturnValue).toFixed(ValDecDigit));

        }
    })
    OnChangeReturnValue();
}
function BindItemBatchSerialDetail() {
    var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
    if (FReturnItemDetails != null && FReturnItemDetails != "") {
        if (FReturnItemDetails.length > 0) {
            var str = JSON.stringify(FReturnItemDetails);
            $('#hdItemBatchSerialDetailList').val(str);
        }

    }
}
function BindGLVoucherDetail() {
    debugger;
    var VouList = [];
    var Val = 0;
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var DocType = "D";
    var TransType = "SalRet";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
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
            VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: DocType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
            //var currentRow = $(this);
            //var acc_id = "";
            //var acc_name = "";
            //var dr_amt = "";
            //var cr_amt = "";
            //var gl_type = "";
            //acc_id = currentRow.find("#hfAccID").val();
            //acc_name = currentRow.find("#txthfAccID").text();
            //dr_amt = currentRow.find("#dramt").text();
            //cr_amt = currentRow.find("#cramt").text();
            //gl_type = currentRow.find("#type").val();
            //VouList.push({ comp_id: Compid, accid: acc_id, acc_name: acc_name, type: "I", doctype: DocType, Value: Val, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, gl_type: gl_type });

        });
    }
    var str = JSON.stringify(VouList);
    $('#hdSalesReturnVouDetailList').val(str);
}
function GetSI_VoucherDetails() {
    debugger;
    var SI_VouList = [];
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "E";

    var TransType = "Sal";
    if ($("#VoucherDetail1 >tbody >tr").length > 0) {
        $("#VoucherDetail1 >tbody >tr").each(function () {
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
function OnChangeInvoiceValue() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var ValDecDigit = $("#ValDigit").text();
    var TotalValue = parseFloat(0);
    $("#SReturnItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var InvoiceValue = "";
        InvoiceValue = row.find("#InvoiceValue").val();
        if (InvoiceValue != null && InvoiceValue != "") {
            if (parseFloat(InvoiceValue) > parseFloat(0)) {
                TotalValue = parseFloat(TotalValue) + parseFloat(InvoiceValue);
            }
        }
        $("#TotalInvoiceValue").val(parseFloat(TotalValue).toFixed(ValDecDigit));
    });
}
function OnChangeReturnValue() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var ValDecDigit = $("#ValDigit").text();
    var TotalValue = parseFloat(0);
    $("#SReturnItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var ReturnValue = "";
        ReturnValue = row.find("#ReturnValue").val();
        if (ReturnValue != null && ReturnValue != "") {
            if (parseFloat(ReturnValue) > parseFloat(0)) {
                TotalValue = parseFloat(TotalValue) + parseFloat(ReturnValue);
            }
        }
        $("#TotalReturnValue").val(parseFloat(TotalValue).toFixed(ValDecDigit));
    });
    var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
    var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
    $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {

        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        $("#btn_save").attr("disabled", false);
        $("#InvWiseAddItem").css("display", "none");
        CancelledRemarks("#Cancelled", "Enable");
        return true;
    }
    else {
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_save").attr("disabled", true);
        CancelledRemarks("#Cancelled", "Enable");
        return false;
    }
   
}
function CheckCancelledStatus() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        return true;
    }
    else {
        return false;
    }
}
function EnableEditkBtntst() {
    debugger;
    sessionStorage.setItem("EditBtnClickSR", "Y");
    return true;
}
function ForwardBtnClick() {
    debugger;
    //var SRTStatus = "";
    //SRTStatus = $('#hdSRTStatus').val().trim();
    //if (SRTStatus === "D" || SRTStatus === "F") {

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
        var SrtDate = $("#txtReturnDate").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: SrtDate
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var SRTStatus = "";
                    SRTStatus = $('#hdSRTStatus').val().trim();
                    if (SRTStatus === "D" || SRTStatus === "F") {

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
                    /*  swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
    var SRTNo = "";
    var SRTDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var Src_Type = "";
    var mailerror = "";

    Src_Type =$('#src_type').val();
    Remarks = $("#fw_remarks").val();
    SRTNo = $("#ReturnNumber").val();
    $("#hdDoc_No").val(SRTNo);
    SRTDate = $("#txtReturnDate").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var TrancType = (SRTNo + ',' + SRTDate + ',' + docid + ',' + WF_status1)
    var CnNarr = $('#CreditNoteRaisedAgainstSalRet').text();
     var JVNurr = $('#hdn_JVNurr').text();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
     if (fwchkval != "Approve") {
         var pdfAlertEmailFilePath = "SalesReturn_CreditNote_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
         var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(SRTNo, SRTDate, Src_Type, pdfAlertEmailFilePath, docid, fwchkval);
         if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
             pdfAlertEmailFilePath = "";
         }
     }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SRTNo != "" && SRTDate != "" && level != "") {
            Cmn_InsertDocument_ForwardedDetail1(SRTNo, SRTDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SalesReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/SalesReturn/SalesReturnApprove?SRTNo=" + SRTNo + "&SRTDate=" + SRTDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&CnNarr=" + CnNarr + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid + "&Src_Type=" + Src_Type + "&JVNurr=" + JVNurr;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SRTNo != "" && SRTDate != "") {
            Cmn_InsertDocument_ForwardedDetail1(SRTNo, SRTDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SalesReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && SRTNo != "" && SRTDate != "") {
            Cmn_InsertDocument_ForwardedDetail1(SRTNo, SRTDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/SalesReturn/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
async function GetPdfFilePathToSendonEmailAlert(SRTNo, SRTDate, SrcType, fileName, docid, docstatus) {
    debugger;
    var filepath = "";
    filepath = await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/SavePdfDocToSendOnEmailAlert",
        data: { SRTNo: SRTNo, SRTDate: SRTDate, SrcType: SrcType, fileName: fileName, docid: docid, docstatus: docstatus },
        success: function (data) {
        }
    });
    return filepath;
}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ReturnNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
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
    var disableflag = ($("#DisableSubItem").val());
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
            debugger;
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
function GetAllGLID() {
    var src_type = $('#src_type').val();
    if (src_type == "C") {
        GetAllGLID1();
    }
    else {
        GetAllGL_WithMultiSupplier();
    }
}
//------------------Custom Invoice GL Detail Start-----------------//
async function GetAllGLID1() {
    debugger;
    var cust_id = $("#CustomerName").val();
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "E";
    var TransType = 'Sal';
    var GLDetail = [];
    var conv_rate = $("#conv_rate").val();
    //$("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var tax_id = currentRow.find("#taxID").text();
    //    var tax_amt = currentRow.find("#TotalTaxAmount").text();
    //    if (parseFloat(tax_amt) > 0) {
    //        GLDetail.push({
    //            comp_id: Compid, id: tax_id, type: "Tax", doctype: _SIType, Value: tax_amt
    //            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax"
    //        });
    //    }
    //});

    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var id = currentRow.find("#id").text();
        var Amt = currentRow.find("#totval").text();
        var type = currentRow.find("#type").text();    
        if (type == "Tax") {
            GLDetail.push({
                comp_id: Compid, id: id, type: type, doctype: _SIType, Value: Amt, ValueInBase: Amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type
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
                            $('#VoucherDetail1 tbody tr').remove();
                            var rowIdx = $('#VoucherDetail1 tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                debugger;
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail1 tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//<td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
                                   
//                            </tr>`);

                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    $('#VoucherDetail1 tbody').append(`<tr id="R${++rowIdx}">
                                    <td class="sr_padding">${rowIdx}</td>
                                    <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    <td id="dramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
                                   
                            </tr>`);
                                }
                            }
                        }
                    }
                    CalculateVoucherTotalAmount1();
                }
            }
        });
    }
    else {
        $('#VoucherDetail1 tbody tr').remove();
        CalculateVoucherTotalAmount1();
    }
}
async function CalculateVoucherTotalAmount1(flag) {
    debugger;
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail1 >tbody >tr").each(function () {
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
                    $('#VoucherDetail1 tbody tr #type[value="Tr"]').closest("Tr").remove();
                    if (DrTotAmt < CrTotAmt) {
                        var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                        var rowIdx = $('#VoucherDetail1 tbody tr').length;
                        for (var j = 0; j < arr.Table.length; j++) {
                            //if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                            //    $('#VoucherDetail1 tbody').append(`<tr id="R${++rowIdx}">
                            //        <td class="sr_padding">${rowIdx}</td>
                            //        <td id="CInv_txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                            //        <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
                            //        <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
                            //                       <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                            //         <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>                                 
                            //</tr>`);
                            //}

                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                $('#VoucherDetail1 tbody').append(`<tr id="R${++rowIdx}">
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
                    else {
                        var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                        var rowIdx = $('#VoucherDetail1 tbody tr').length;
                        for (var j = 0; j < arr.Table.length; j++) {
                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                $('#VoucherDetail1 tbody').append(`<tr id="R${++rowIdx}">
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
                    CalculateVoucherTotalAmount1("CalcOnly");
                }
            }
        }
    });
}
//------------------Custom Invoice GL Detail End-----------------//
async function GetAllGL_WithMultiSupplier() {
    debugger;
    //var cust_acc_id = $("#cust_acc_id").val();
    var Compid = $("#CompID").text();
    var cust_acc_id = $("#cust_acc_id").val();
    var DocType = "D";
    var TransType = 'SalRet';
    var conv_rate = $("#conv_rate").val();
    var curr_id = $("#CurrId").val();
    //var cust_id = $("#hdcust_id").val();
    var cust_id = $("#ddlCustomerName").val();
    var NetInvValue = $("#TotalReturnValue").val();
    //var NetInvValue = $("#TotalInvoiceValue").val();
    var CustVal = 0;
    var CustValInBase = 0;
    var src_type = $('#src_type').val();
    if (src_type == "A") {
        var NetInvValue = $("#TotalReturnValue").val();
    }
    CustValInBase = parseFloat(NetInvValue).toFixed(ValDecDigit);
    CustVal = (parseFloat(CustValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)
    var GLDetail = [];
    if (src_type == "A") {     
        GLDetail.push({
            comp_id: Compid, id: cust_id, type: "Cust", doctype: DocType, Value: CustVal, ValueInBase: CustValInBase
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Cust", parent: 0, Entity_id: cust_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: '', bill_date: '', acc_id: ""
        });
        $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var item_id = currentRow.find("#hdItemId").val();
            var ItmGrossVal = currentRow.find("#item_ass_val").val();
            var ItemAccId = currentRow.find("#hdn_item_gl_acc").val()
            var ItmGrossValInBase = currentRow.find("#item_ass_val").val();
            GLDetail.push({
                comp_id: Compid, id: item_id, type: "Itm", doctype: DocType, Value: ItmGrossVal
                , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: cust_acc_id
                , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: '', bill_date: '', acc_id: ItemAccId
            });
        });
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            debugger
            var currentRow = $(this);
            var tax_id = currentRow.find("#TaxNameID").text();
            var tax_acc_id = currentRow.find("#TaxAccId").text();
            var tax_amt = currentRow.find("#TaxAmount").text();
            if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
                GLDetail.push({
                    comp_id: Compid, id: tax_id, type: "Tax", doctype: DocType, Value: tax_amt
                    , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: cust_acc_id
                    , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: '', bill_date: '', acc_id: ""
                });
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
            var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(conv_rate)).toFixed(ValDecDigit); //with tax
            //var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

            var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? cust_acc_id : oc_supp_acc_id;
            //var oc_parent = supp_acc_id; //: oc_supp_acc_id;

            GLDetail.push({
                comp_id: Compid, id: oc_id, type: "OC", doctype: "D", Value: oc_amt
                , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
                , Entity_id: oc_acc_id, curr_id: curr_id//oc_curr_id
                , conv_rate: conv_rate//oc_conv_rate
                , bill_no: ''
                , bill_date: ''
            });

            if (oc_supp_id != "" && oc_supp_id != "0") {
                let gl_type = "Supp";
                if (oc_supp_type == "2")
                    gl_type = "Supp";
                if (oc_supp_type == "7")
                    gl_type = "Bank";

                GLDetail.push({
                    comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: "D", Value: oc_amt_wt
                    , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                    , Entity_id: oc_supp_acc_id, curr_id: curr_id//oc_curr_id
                    , conv_rate: conv_rate //oc_conv_rate
                    , bill_no: ''
                    , bill_date: '', acc_id: ""
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
                comp_id: Compid, id: tax_id, type: "OcTax", doctype: "D", Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: "OcTax", parent: ArrOcGl[0].id
                , Entity_id: tax_acc_id, curr_id: curr_id//ArrOcGl[0].curr_id
                , conv_rate: conv_rate//ArrOcGl[0].conv_rate
                ,bill_no: '', bill_date: '', acc_id: ""
            });
        });
    }
    else {
        $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var id = currentRow.find("#id").text();
            var Amt = currentRow.find("#totval").text();
            var type = currentRow.find("#type").text();
            var acc_id = "";
            if (type == "Itm") {
                acc_id = $("#SReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + id + "']").closest('tr').find("#hdn_item_gl_acc").val();
            }
            if (type == "Cust") {
                GLDetail.push({ comp_id: Compid, id: cust_id, type: type, doctype: DocType, Value: CustVal, ValueInBase: CustValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type, parent: 0, Entity_id: cust_id, curr_id: curr_id, conv_rate: conv_rate, acc_id: acc_id });
            }
            else {
                if (src_type == "C") {
                    var amt_bs = (parseFloat(Amt) / parseFloat(conv_rate)).toFixed(ValDecDigit)
                    GLDetail.push({
                        comp_id: Compid, id: id, type: type, doctype: DocType, Value: amt_bs, ValueInBase: Amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type, parent: cust_acc_id, Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, acc_id: acc_id
                    });
                }
                else {
                    GLDetail.push({
                        comp_id: Compid, id: id, type: type, doctype: DocType, Value: Amt, ValueInBase: Amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type, parent: cust_acc_id, Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, acc_id: acc_id
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
            var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(conv_rate)).toFixed(ValDecDigit); //with tax
            //var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

            var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? cust_acc_id : oc_supp_acc_id;
            //var oc_parent = supp_acc_id; //: oc_supp_acc_id;

            GLDetail.push({
                comp_id: Compid, id: oc_id, type: "OC", doctype: "D", Value: oc_amt
                , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
                , Entity_id: oc_acc_id, curr_id: curr_id //oc_curr_id
                , conv_rate: conv_rate//oc_conv_rate
                , bill_no: oc_supp_bill_no == "" ? '' : oc_supp_bill_no
                , bill_date: oc_supp_bill_dt == "" ? '' : oc_supp_bill_dt, acc_id: ""
            });

            if (oc_supp_id != "" && oc_supp_id != "0") {
                let gl_type = "Supp";
                if (oc_supp_type == "2")
                    gl_type = "Supp";
                if (oc_supp_type == "7")
                    gl_type = "Bank";

                GLDetail.push({
                    comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: "D", Value: oc_amt_wt
                    , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                    , Entity_id: oc_supp_acc_id, curr_id: curr_id //oc_curr_id
                    , conv_rate: conv_rate//oc_conv_rate
                    , bill_no: oc_supp_bill_no
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
                comp_id: Compid, id: tax_id, type: "OcTax", doctype: "D", Value: tax_amt
                , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: "OcTax", parent: ArrOcGl[0].id
                , Entity_id: tax_acc_id, curr_id: curr_id//ArrOcGl[0].curr_id
                , conv_rate: conv_rate//ArrOcGl[0].conv_rate
                ,bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
            });
        });
    }
   
    await $.ajax({
        type: "POST",
        url: "/Common/Common/GetGLDetails1",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ GLDetail: GLDetail }),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
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
                        var arrSupp = arr.Table.filter(v => (v.type == "Cust" || v.type == "Bank" || v.type == "Supp"));
                        var mainSuppGl = arrSupp.filter(v => v.acc_id == cust_acc_id);
                        var NewArrSupp = arrSupp.filter(v => v.acc_id != cust_acc_id);
                        NewArrSupp.unshift(mainSuppGl[0]);
                        arrSupp = NewArrSupp;
                        for (var j = 0; j < arrSupp.length; j++) {
                            let cust_id = arrSupp[j].id;
                            let arrDetail = arr.Table.filter(v => (v.id == cust_id && (v.type == "Cust" || v.type == "Bank" || v.type == "Supp")));
                            let arrDetailDr = arr.Table.filter(v => (v.parent == cust_id && v.type != "OcTax"));
                            let RcmValue = 0;
                            let RcmValueInBase = 0;
                            arr.Table.filter(v => (v.parent == cust_id && v.type == "RCM"))
                                .map((res) => {
                                    RcmValue = RcmValue + res.Value;
                                    RcmValueInBase = RcmValueInBase + res.ValueInBase;
                                });
                            arrDetail[0].Value = arrDetail[0].Value - RcmValue;
                            arrDetail[0].ValueInBase = arrDetail[0].ValueInBase - RcmValueInBase;

                            let rowSpan = 1;//arrDetailDr.length + 1;
                            let GlRowNo = 1;
                            let vouType = "";
                            if (arrDetail[0].type == "Cust")
                                vouType = "CN"
                            if (arrDetail[0].type == "Supp")
                                vouType = "PV"
                            if (arrDetail[0].type == "Bank")
                                vouType = "BP"
                            if (vouType == "SV") {
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
                                    , arrDetail[0].Value, arrDetail[0].ValueInBase, 0, 0, vouType, arrDetail[0].curr_id
                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
                            }
                            else {
                                GlTableRenderHtml(j + 1, GlRowNo, rowSpan, arrDetail[0].type, arrDetail[0].acc_name, arrDetail[0].acc_id
                                    , 0, 0, arrDetail[0].Value, arrDetail[0].ValueInBase, vouType, arrDetail[0].curr_id
                                    , arrDetail[0].conv_rate, arrDetail[0].bill_no, arrDetail[0].bill_dt);
                            }
                            let ArrTax = [];
                            let ArrGlDetailsDr = [];
                            for (var k = 0; k < arrDetailDr.length; k++) {
                                let getAccIndex = ArrGlDetailsDr.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
                                if (getAccIndex > -1) {
                                    ArrGlDetailsDr[getAccIndex].Value = parseFloat(ArrGlDetailsDr[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
                                    ArrGlDetailsDr[getAccIndex].ValueInBase = parseFloat(ArrGlDetailsDr[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
                                } else {
                                    if (arrDetailDr[k].type == "Tax") {
                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailDr[k].acc_id);
                                        if (getAccIndex > -1) {
                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailDr[k].Value)
                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailDr[k].ValueInBase)
                                        } else {
                                            ArrTax.push(arrDetailDr[k]);
                                        }
                                    }
                                    else if (arrDetailDr[k].type == "OcTax") {
                                    }
                                    else {
                                        ArrGlDetailsDr.push(arrDetailDr[k]);
                                    }
                                }
                                if (arrDetailDr[k].type == "OC") {

                                    //let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].id && v.type == "OcTax"));
                                    let arrDetailOCDr = arr.Table.filter(v => (v.parent == arrDetailDr[k].oc_id && v.type == "OcTax"));
                                    // Grouping the similer tax account
                                    for (var l = 0; l < arrDetailOCDr.length; l++) {

                                        let getAccIndex = ArrTax.findIndex(v => v.acc_id == arrDetailOCDr[l].acc_id);
                                        if (getAccIndex > -1) {
                                            ArrTax[getAccIndex].Value = parseFloat(ArrTax[getAccIndex].Value) + parseFloat(arrDetailOCDr[l].Value)
                                            ArrTax[getAccIndex].ValueInBase = parseFloat(ArrTax[getAccIndex].ValueInBase) + parseFloat(arrDetailOCDr[l].ValueInBase)
                                        } else {
                                            ArrTax.push(arrDetailOCDr[l]);
                                        }
                                    }
                                }
                            }
                            var ArrGLDetailRCM = []
                            for (var i = 0; i < ArrGlDetailsDr.length; i++) {

                                if (ArrGlDetailsDr[i].type == "RCM") {
                                    ArrGLDetailRCM.push(ArrGlDetailsDr[i]);

                                } else {
                                    GlRowNo++;
                                    rowSpan++;
                                    if (vouType == "SV") {
                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
                                            , ArrGlDetailsDr[i].acc_id, 0, 0, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase,
                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
                                        )
                                    }
                                    else {
                                        GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGlDetailsDr[i].type, ArrGlDetailsDr[i].acc_name
                                            , ArrGlDetailsDr[i].acc_id, ArrGlDetailsDr[i].Value, ArrGlDetailsDr[i].ValueInBase, 0, 0,
                                            vouType, ArrGlDetailsDr[0].curr_id, ArrGlDetailsDr[0].conv_rate
                                            , ArrGlDetailsDr[0].bill_no, ArrGlDetailsDr[0].bill_dt
                                        )
                                    }

                                }
                            }
                            for (var i = 0; i < ArrTax.length; i++) {
                                GlRowNo++;
                                rowSpan++;
                                // Row Generated here for Tax on Other Charge
                                if (vouType == "SV") {
                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
                                        , 0, 0, ArrTax[i].Value, ArrTax[i].ValueInBase, vouType,
                                        ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
                                    )
                                }
                                else {
                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrTax[i].type, ArrTax[i].acc_name, ArrTax[i].acc_id
                                        , ArrTax[i].Value, ArrTax[i].ValueInBase, 0, 0, vouType,
                                        ArrTax[0].curr_id, ArrTax[0].conv_rate, ArrTax[0].bill_no, ArrTax[0].bill_dt
                                    )
                                }
                            }
                            for (var i = 0; i < ArrGLDetailRCM.length; i++) {
                                GlRowNo++;
                                rowSpan++;
                                if (vouType == "SV") {
                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
                                        , ArrGLDetailRCM[i].acc_id, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase, 0, 0
                                        , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
                                        , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
                                    )
                                }
                                else {
                                    GlTableRenderHtml(j + 1, GlRowNo, rowSpan, ArrGLDetailRCM[i].type, ArrGLDetailRCM[i].acc_name
                                        , ArrGLDetailRCM[i].acc_id, 0, 0, ArrGLDetailRCM[i].Value, ArrGLDetailRCM[i].ValueInBase
                                        , vouType, ArrGLDetailRCM[0].curr_id, ArrGLDetailRCM[0].conv_rate
                                        , ArrGLDetailRCM[0].bill_no, ArrGLDetailRCM[0].bill_dt
                                    )
                                }
                            }
                            $("#tr_GlRow" + (j + 1) + " #td_SrNo").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouType").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouNo").attr("rowspan", rowSpan);
                            $("#tr_GlRow" + (j + 1) + " #td_VouDate").attr("rowspan", rowSpan);
                        }
                    }
                }
                BindDDLAccountList();
                CalculateVoucherTotalAmount();
            }
        }
    });
}
function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {
    var ValDecDigit = $("#ValDigit").text();
    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
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
    /*--------------------Added by Suraj Maurya on 08-08-2025----------------------*/
    
    let gl_narr = "";

    try {
        gl_narr = Get_Gl_Narration(VouType, bill_bo, bill_date, type);/*Getting GL Narration from Main Document*/
    }
    catch (ex) {
        console.log(ex);
    }


    /*--------------------Added by Suraj Maurya on 08-08-2025 End----------------------*/
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
                                    <td class="truncate-text" id="gl_narr" contenteditable="true" style="width:350px;padding-right:30px;"><span>${gl_narr}</span></td>
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
    if (type == "Supp" || type == "Bank" || type == "Cust") {
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
function Get_Gl_Narration(VouType, bill_no, bill_date,type) {
    let Narration = "";
    switch (VouType) {
        case "CN":
            Narration = $("#CnNarr").val();
            break;
        default:
            Narration = $("#CnNarr").val();
            break;
    }
    return (Narration).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}

async function CalculateVoucherTotalAmount() {

    var ValDecDigit = $("#ValDigit").text();
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
    $("#DrTotalInBase").text(DrTotAmt);
    $("#DrTotalInBase").text(DrTotAmtInBase);
    $("#CrTotalInBase").text(CrTotAmt);
    $("#CrTotalInBase").text(CrTotAmtInBase);

    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        debugger;
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }

    }
}
async function AddRoundOffGL() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();///Amount
    var conv_rate = $("#conv_rate").val();
    var curr_id = $("#CurrId").val();
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
                    debugger;
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
                                    GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , Diff, DiffInBase, 0, 0, "CN"
                                        , curr_id, conv_rate, "", "")

                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);
                                }
                            }
                        }
                        else {
                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                            var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    var GlSrNo = $('#VoucherDetail tbody tr').length + 1;
                                    GlTableRenderHtml(1, GlSrNo, GlSrNo, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , 0, 0, Diff, DiffInBase, "CN"
                                        , curr_id, conv_rate, "", "")
                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouType").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouNo").attr("rowspan", GlSrNo);
                                    vouRow.find("#td_VouDate").attr("rowspan", GlSrNo);
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
//function GetAllGLID() {
//    debugger;
//    var Compid = $("#CompID").text();
//    var DocType = "D";
//    var TransType = 'SalRet';
//    var GLDetail = [];
//    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {
//        var currentRow = $(this);
//        var id = currentRow.find("#id").text();
//        var Amt = currentRow.find("#totval").text();
//        var type = currentRow.find("#type").text();

//        GLDetail.push({ comp_id: Compid, id: id, type: type, doctype: DocType, Value: Amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: type });
//    });
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetGLDetails",
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
//                if (arr.Table1.length > 0) {
//                    var errors = [];
//                    var step = [];

//                    for (var i = 0; i < arr.Table1.length; i++) {
//                        debugger;
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
//                            ++rowIdx
//                            var Accid = arr.Table[j].acc_id;
//                            var FieldType = "";
//                                if (arr.Table[j].type == 'Itm') {
//                                    FieldType = `<div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Acc_name_${rowIdx}" onchange ="OnChangeAccountName(${rowIdx},event)">
//                                        </select>
//                                    <input  type="hidden" id="hfAccID" value="${Accid}" /></div>`;
//                                    $("#hdnAccID").val(Accid);
//                                }
//                                else {
//                                    FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${Accid}" />`;
//                                }
                          
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                  <td>`+ FieldType + `</td>
//                                    <td id="dramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;">
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="${arr.Table[j].type}"/>
//                                    </td>
//                                    <td class="center">
//                                        <button type="button" id="CostCenterDetail" disabled onclick="" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>                                   
//                            </tr>`);
//                            }
//                        }
//                        var TotInvVal = $("#TotalInvoiceValue").val();
//                        if (TotInvVal == 0 || TotInvVal == null || TotInvVal == '' || TotInvVal == "NaN") {
//                            TotInvVal = 0;
//                            $("#VoucherDetail >tbody >tr:first").find("#cramt").text(parseFloat(TotInvVal).toFixed(ValDecDigit));
//                        }
//                        else {
//                            $("#VoucherDetail >tbody >tr:first").find("#cramt").text(parseFloat(TotInvVal).toFixed(ValDecDigit));
//                        }
//                    }
//                    CalculateVoucherTotalAmount();
//                    BindDDLAccountList();
//                }
//            }
//        }
//    });
//}
//function CalculateVoucherTotalAmount() {
//    debugger;
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
//        AddRoundOffGL(DrTotAmt, CrTotAmt);
//    }
//}
//function AddRoundOffGL(DrTotAmt, CrTotAmt) {
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
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
//                if (arr.Table.length > 0) {
//                    // $('#VoucherDetail tbody tr').remove();
//                    if (DrTotAmt < CrTotAmt) {
//                        var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;">
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="RO"/>
//                                    </td>
//                                    <td class="center">
//                                        <button type="button" id="CostCenterDetail" disabled onclick="" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>
                                   
//                            </tr>`);
//                            }
//                        }
//                    }
//                    else {
//                        var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);

//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;">
//                                        <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                        <input type="hidden" id="type" value="RO"/>
//                                    </td>
//                                    <td class="center">
//                                        <button type="button" id="CostCenterDetail" disabled onclick="" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                    </td>
                                   
//                            </tr>`);
//                            }
//                        }
//                    }

//                    CalculateVoucherTotalAmount();

//                }

//            }

//        }
//    });
//}
function DeleteVoucherDetail(ItemCode, ShipNo, InvoiceValue, ReturnValue) {
    debugger;
    if (InvoiceValue == '' || InvoiceValue == null) {
        InvoiceValue = 0;
    }
    if (ReturnValue == '' || ReturnValue == null) {
        ReturnValue = 0;
    }

    var TotalInvval = $("#TotalInvoiceValue").val();
    if (TotalInvval == '' || TotalInvval == null) {
        TotalInvval = 0;
    }
    var FTotalInvval = parseFloat(TotalInvval) - parseFloat(InvoiceValue)

    var TotalRetval = $("#TotalReturnValue").val();
    if (TotalRetval == '' || TotalRetval == null) {
        TotalRetval = 0;
    }

    var FTotalRetval = parseFloat(TotalRetval) - parseFloat(ReturnValue)
    $("#TotalReturnValue").val(parseFloat(FTotalRetval).toFixed(ValDecDigit));
    $("#TotalInvoiceValue").val(parseFloat(FTotalInvval).toFixed(ValDecDigit));
    $("#hd_GL_DeatilTbl >tbody >tr").each(function () {

        var currentRow = $(this);
        var ItemId = currentRow.find("#itemid").text();
        var ShNo = currentRow.find("#shipno").text();
        if (ItemCode == ItemId && ShipNo == ShNo) {
            currentRow.remove();
        }
    });
    GetAllGLID();


}
function UpdateGLValue(ItemCode, ShipNo, ReturnQuantity) {
    debugger;
    $("#hd_GL_DeatilTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemId = currentRow.find("#itemid").text();
        var ShNo = currentRow.find("#shipno").text();

        if (ItemCode == ItemId && ShipNo == ShNo) {

            var Amt = currentRow.find("#amt").text();
            var Value = parseFloat(Amt) * parseFloat(ReturnQuantity)
            currentRow.find("#totval").text(Value);

        }
    });
    GetAllGLID();
}
function OnChangeAccountName(RowID, e) {
   
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
    }
    if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }

    if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
        //Added by Suraj on 12-08-2024 to stop reset of GL Account if user changes the GL Acc.
        $("#SReturnItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
            var row = $(this);
            row.find("#hdn_item_gl_acc").val(Acc_ID);
        });
    }

    clickedrow.find("#hfAccID").val(Acc_ID);
    clickedrow.find("#hdnAccID").val(Acc_ID);
    $("#hdnAccID").val(Acc_ID);
}
/**---------------WareHouse Sub Item Return Quantity PopUP Work Start----------------------**/
function OnclickSubItemRtnQtyDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var Arr = [];
    var ProductNm = $("#IconItemName").val();
    var ProductId = $("#RQhdItemId").val();
    var Subitem = $("#Iconsub_item").val();
    var UOM = $("#IconUOM").val();
    var ShipNo = $("#IconGRNNumber").val();
    var ShipDt = $("#IconGRNDate").val();
    var HdnShipDt = $("#HdnIconGRNDate").val();

    var subitm_RtrnQty = $("#IconReturnQuantity").val();
    /*var doc_Date = $("#Prod_Ord_number option:selected")[0].dataset.date*/

    $("#RtWh_Sub_ProductlName").val(ProductNm);
    $("#RtWh_Sub_ProductlId").val(ProductId);
    $("#RtWh_Subitem").val(Subitem);
    $("#RtWh_Sub_serialUOM").val(UOM);
    $("#RtWh_Sub_GRNNo").val(ShipNo);
    $("#RtWh_Sub_GRNDate").val(ShipDt);
    $("#HdnRtWh_Sub_GRNDate").val(HdnShipDt);
    $("#RtWh_Sub_RetrnQuantity").val(subitm_RtrnQty);
    var subitmDisable = "";
    var RtWh_Subitem = $("#RtWh_Subitem").val();
    if (RtWh_Subitem != "Y") {
        subitmDisable = "disabled";
    }
    $("#ReturnedQtyDetailsTbl tbody tr").each(function () {
        var Row = $(this);
        var RowNo = Row.find("#SNohiddenfiled").val();
        var List = {};

        var wh_id = Row.find("#wh_id" + RowNo).val();
        var wh_type = Row.find("#wh_type" + RowNo).val();
        var wh_name = Row.find("#Warehouse" + RowNo).val();
        var avl_qty = Row.find("#AvailableQuantity" + RowNo).val();
        var return_qty = Row.find("#IconReturnQtyinput" + RowNo).val();
        if (return_qty == "") {
            return_qty = parseFloat(0);
        }
        var CheckWh = Arr.filter(v => v.wh_id == wh_id);
        if (CheckWh.length > 0) {
            let Index = Arr.findIndex(p => p.wh_id == wh_id);
            Arr[Index].avl_qty = parseFloat(Arr[Index].avl_qty) + parseFloat(avl_qty);
            Arr[Index].return_qty = parseFloat(Arr[Index].return_qty) + parseFloat(return_qty);
        } else {
            Arr.push({ wh_id: wh_id, wh_name: wh_name, wh_type: wh_type, avl_qty: avl_qty, return_qty: return_qty });
        }

    });
    $("#RtWh_Sub_ItemDetailTbl tbody >tr").remove();
    var PRTWhReturnQty = 0;
    var Disable = $("#DisableSubItem").val();

    Arr.map(function (Item, Index) {
        debugger
        var disabled = "";
        if (Disable == "Y") {
            disabled = "disabled";
            //$("#RtWh_SubItemSaveAndExitBtn").attr("disabled", true);
            //$("#RtWh_SubItemDiscardAndExitBtn").attr("disabled", true);
            $("#RtWh_SubItemSaveAndExitBtn").css("display", "none");
            $("#RtWh_SubItemDiscardAndExitBtn").css("display", "none");
            $("#Close").css("display", "block");


        }
        else {
            disabled = "";
            //$("#RtWh_SubItemSaveAndExitBtn").attr("disabled", false);
            //$("#RtWh_SubItemDiscardAndExitBtn").attr("disabled", false);
            $("#RtWh_SubItemSaveAndExitBtn").css("display", "block");
            $("#RtWh_SubItemDiscardAndExitBtn").css("display", "block");
            $("#Close").css("display", "none");
        }
        $("#RtWh_Sub_ItemDetailTbl tbody").append(`<tr>
                            <td><input class="form-control" id="RtWhSubItm_SrNo" disabled value="${Index + 1}" /></td>
                            <td><input class="form-control" id="RtWhSubItm_WhName" disabled value="${Item.wh_name}" /></td>
                            <td style="display: none;">
                            <input  type="hidden" id="RtWhSubItm_WhId" value="${Item.wh_id}"/></td>
                            <input  type="hidden" id="RtWhSubItm_WhType" value="${Item.wh_type}"/></td>
                            <td><input class="form-control num_right" id="RtWhSubItm_AvlQty" disabled value="${parseFloat(Item.avl_qty).toFixed(QtyDecDigit)}" autocomplete="off" type="text" placeholder="0000.00" /></td>
                            <td>
                              <div class="col-sm-10 lpo_form no-padding">
                                <input class="form-control num_right" id="RtWhSubItm_ReturnQty" ${disabled} value="${parseFloat(Item.return_qty).toFixed(QtyDecDigit)}" onchange="OnChangePopupRTWhReturnQty(this,event)" onkeypress="return OnKeyPressRetQty(this,event);" autocomplete="off" type="text" placeholder="0000.00" />
                                <span id="SpanRtWhSubItm_ReturnQty" class="error-message is-visible"></span></div>
                              </div>
                              <div class="col-sm-2 i_Icon pl-0" id="div_SubItemReturnQty" >
                              <button type="button" id="SubItemReturnQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('SRTReturnQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                              </div>
                            </td>
                        </tr>`);



        PRTWhReturnQty = parseFloat(PRTWhReturnQty) + parseFloat(Item.return_qty);

    })
    $("#RtWh_TotalQty").text(parseFloat(PRTWhReturnQty).toFixed(QtyDecDigit));
}
function OnChangePopupRTWhReturnQty(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var Sno = clickedrow.find("#RtWhSubItm_SrNo").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity  
    var RetQty = clickedrow.find("#RtWhSubItm_ReturnQty").val();
    var Available = parseFloat(clickedrow.find("#RtWhSubItm_AvlQty").val()).toFixed(parseFloat(QtyDecDigit));
    if ((RetQty == "") || (RetQty == null) || (RetQty == 0) || (RetQty == "NaN")) {
        RetQty = 0;
    }
    if (parseFloat(RetQty) <= parseFloat(Available)) {

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);

        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").css("display", "none");
        clickedrow.find("#RtWhSubItm_ReturnQty").css("border-color", "#ced4da");

    }
    else {
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").text($("#ExceedingQty").text());
        clickedrow.find("#SpanRtWhSubItm_ReturnQty").css("display", "block");
        clickedrow.find("#RtWhSubItm_ReturnQty").css("border-color", "red");
        var test = parseFloat(parseFloat(RetQty)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#RtWhSubItm_ReturnQty").val(test);

    }

    RtWhSubItmCalTotalReturnQty(el, evt);
}
function RtWhSubItmCalTotalReturnQty(el, evt) {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var TotalQty = parseFloat(0).toFixed(QtyDecDigit);

    $("#RtWh_Sub_ItemDetailTbl >tbody >tr").each(function (i, row) {
        var CRow = $(this);
        //var Sno = CRow.find("#SNohiddenfiled").val();
        var RtnQty = CRow.find("#RtWhSubItm_ReturnQty").val();
        if (RtnQty != "" && RtnQty != null) {
            TotalQty = (parseFloat(TotalQty) + parseFloat(RtnQty));
        }
    });

    $("#RtWh_TotalQty").text(parseFloat(TotalQty).toFixed(QtyDecDigit));

};
function PRtWh_SaveAndExitSubItem() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity     
    var IconShipNumber = $("#RtWh_Sub_GRNNo").val();

    var IShipDate = $("#HdnRtWh_Sub_GRNDate").val().trim();
    //var date = IShipDate.split("-");
    //var FDate = date[2] + '-' + date[1] + '-' + date[0];
    var IconShipDate = IShipDate;
    var IconReturnQuantity = $("#RtWh_Sub_RetrnQuantity").val();
    var ItmCode = $("#RtWh_Sub_ProductlId").val();
    var ItmUomId = $("#RtWh_Sub_serialUOM").val();

    var TotalQty = $("#RtWh_TotalQty").text();

    var error = "N";
    $("#RtWh_Sub_ItemDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#RtWhSubItm_SrNo").val();
        debugger;
        var RetQty = currentRow.find("#RtWhSubItm_ReturnQty").val();
        var RTWhId = currentRow.find("#RtWhSubItm_WhId").val();
        RetQty = parseFloat(CheckNullNumber(RetQty));

        var Available = parseFloat(currentRow.find("#RtWhSubItm_AvlQty").val()).toFixed(parseFloat(QtyDecDigit));
        if (parseFloat(RetQty) <= parseFloat(Available)) {

            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value='" + ItmCode + "']").closest('tr')
                .find("#subItemSrcDocNo[value='" + IconShipNumber + "']").closest('tr')
                .find("#subItemSrcDocDate[value='" + IconShipDate + "']").closest('tr');
                //.find("#subItemWhId[value='" + RTWhId + "']").closest('tr');

            var SubTotalQty = 0;

            rows.each(function () {
                var clickedrow1 = $(this);
                let subItemQty = clickedrow1.find('#subItemQty').val();
                SubTotalQty = parseFloat(SubTotalQty) + parseFloat(CheckNullNumber(subItemQty));
            });
            var SubItemButton = "SubItemReturnQty";
            var sub_item = $("#RtWh_Subitem").val();
            if (sub_item == "Y") {
                if (parseFloat(RetQty) != parseFloat(SubTotalQty)) {
                    error = "Y";
                    currentRow.find("#" + SubItemButton).css("border", "1px solid red");
                } else {
                    currentRow.find("#" + SubItemButton).css("border", "");
                }
            }
            currentRow.find("#SpanRtWhSubItm_ReturnQty").text("");
            currentRow.find("#SpanRtWhSubItm_ReturnQty").css("display", "none");
            currentRow.find("#RtWhSubItm_ReturnQty").css("border-color", "#ced4da");
            $("#SubItemReturnQuantity_Detail").css('border', '1px solid #ced4da');
        }
        else {
            currentRow.find("#SpanRtWhSubItm_ReturnQty").text($("#ExceedingQty").text());
            currentRow.find("#SpanRtWhSubItm_ReturnQty").css("display", "block");
            currentRow.find("#RtWhSubItm_ReturnQty").css("border-color", "red");
            error = "Y";
            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
        }
        debugger;

    });
    if (error == "Y") {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
        return false
    }
    else {
        if (parseFloat(IconReturnQuantity) == parseFloat(TotalQty)) {
            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "modal");
            if (ItemtbltoRetrnQtyTblToRtwhtosubitmValidation("Y", ItmCode, IconShipNumber) == false) {
                error = "Y";
                ResetWorningBorderColor();
                $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
                return false
            }
            else {
                return true;
            }
        }
        else {
            error = "Y";
            $("#RtWh_SubItemSaveAndExitBtn").attr("data-dismiss", "");
            swal("", $("#ReturnQtyDoesNotMatchSelectedQty").text(), "warning");
            return false
        }
    }
}

/**--------------- Sub Item Return Quantity PopUP Work End----------------------**/

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemReturnQty");
    //Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemShipedQty");
    //Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvailablQty");
}
//function SubItemDetailsPopUp(flag, e) {
//    debugger;

//    var clickdRow = $(e.target).closest('tr');
//    var hfsno = clickdRow.find("#SNohiddenfiled").val();
//    var ProductNm = clickdRow.find("#ItemName").val();
//    var ProductId = clickdRow.find("#hdItemId").val();
//    var UOM = clickdRow.find("#UOM").val();
//    var ShipNo = clickdRow.find("#ShipNumber").val();
//    //var SrcDo_dt = clickdRow.find("#ShipDate").val();
//    var Doc_no = $("#ReturnNumber").val();
//    var Doc_dt = $("#txtReturnDate").val();
//    var src_doc_no = $("#hd_doc_no").val();
//    var QtyDecDigit = $("#QtyDigit").text();
//    var Sub_Quantity = 0;
//    var NewArr = new Array();
//    if (flag == "Quantity") {
//        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
//            var row = $(this);
//            var List = {};
//            List.item_id = row.find("#ItemId").val();
//            List.sub_item_id = row.find('#subItemId').val();
//            List.qty = row.find('#subItemQty').val();
//            NewArr.push(List);
//        });
//        Sub_Quantity = clickdRow.find("#ReturnQuantity").val();
//    }
//    else if (flag == "Shipped") {
//        Sub_Quantity = clickdRow.find("#ShipQuantity").val();
//    }
//    else if (flag == "ReturnAvlQty") {
//        Sub_Quantity = clickdRow.find("#AvailableQuantity").val();
//    }
//    var IsDisabled = $("#DisableSubItem").val();
//    var hd_Status = $("#hdSRTStatus").val();
//    hd_Status = IsNull(hd_Status, "").trim();

//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetSubItemDetails",
//        data: {
//            Item_id: ProductId,
//            SubItemListwithPageData: JSON.stringify(NewArr),
//            IsDisabled: IsDisabled,
//            Flag: flag,
//            Status: hd_Status,
//            ShipNo: ShipNo,
//            Doc_no: Doc_no,
//            Doc_dt: Doc_dt,
//            src_doc_no: src_doc_no
//        },
//        success: function (data) {
//            debugger;
//            $("#SubItemPopUp").html(data);
//            $("#Sub_ProductlName").val(ProductNm);
//            $("#Sub_ProductlId").val(ProductId);
//            $("#Sub_serialUOM").val(UOM);
//            $("#Sub_Quantity").val(Sub_Quantity);
//        }
//    });

//}
function SubItemDetailsPopUp(flag, e) {
    debugger;

    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var src_type = $('#src_type').val();
    //var wh_id = clickdRow.find("#RtWhSubItm_WhId").val();
    //var WhName = clickdRow.find("#RtWhSubItm_WhName").val();
    //var wh_id = $("#ReturnWarehouse_Id").val();
    //var WhName = $("#ReturnWarehouse").val();
    if (src_type == "A") {
        //var Srno = clickdRow.find("#RowId").val();
        var wh_id = clickdRow.find("#wh_id" + hfsno).val();
        var WhName = clickdRow.find("#wh_id" + hfsno + " option:selected").text();
    }
    else {
        var Srno = clickdRow.find("#RowId").val();
        var wh_id = clickdRow.find("#wh_id" + Srno).val();
        var WhName = clickdRow.find("#wh_id" + Srno + " option:selected").text();
    }
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = clickdRow.find("#UOM").val();
    var WhType = clickdRow.find("#RtWhSubItm_WhType").val();

    //var ProductNm = $("#IconItemName").val();
    //var ProductId = $("#RQhdItemId").val();
    //var UOM = $("#IconUOM").val();
    var ShipNo = clickdRow.find("#ShipNumber").val();
    var ShipDt = clickdRow.find("#HdnShipDate").val();
    //var ShipDt = ShipDate.split("-").reverse().join("-");

    var PinvNo = $("#ddlDocumentNumber option:selected").text();

    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    //var src_doc_no = $("#hd_doc_no").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "SRTReturnQty") {

        //ProductNm = $("#RtWh_Sub_ProductlName").val();
        //ProductId = $("#RtWh_Sub_ProductlId").val();
        //UOM = $("#RtWh_Sub_serialUOM").val();
        //ShipNo = $("#RtWh_Sub_GRNNo").val();
        //ShipDt = $("#HdnRtWh_Sub_GRNDate").val();

        //ProductNm = $("#IconItemName").val();
        //ProductId = $("#RQhdItemId").val();
        //UOM = $("#IconUOM").val();
        //ShipNo = $("#IconGRNNumber").val();
        //ShipDt = $("#IconGRNDate").val();

        ProductNm = clickdRow.find("#ItemName").val();
        ProductId = clickdRow.find("#hdItemId").val();
        UOM = clickdRow.find("#UOM").val();
        ShipNo = clickdRow.find("#ShipNumber").val();
        ShipDt = clickdRow.find("#HdnShipDate").val();

        //wh_id = $("#ReturnWarehouse_Id").val();

        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + ProductId + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + ShipNo + "']").closest('tr');
            //.find("#subItemWhId[value='" + wh_id + "']").closest('tr');
        /*$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {*/
        rows.each(function () {
            var row = $(this);
            var List = {};
            debugger;
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlStk').val();
            List.ShipNo = row.find('#subItemSrcDocNo').val();
            List.ShipDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        });
        //Sub_Quantity = clickdRow.find("#RtWhSubItm_ReturnQty").val();
        Sub_Quantity = clickdRow.find("#ReturnQuantity").val();
        var IsDisabled = $("#DisableSubItem").val();
    }
    else if (flag == "AdSRTReturnQty") {
        ProductNm = clickdRow.find("#ItemName" + hfsno +" option:selected").text();
        Sub_Quantity = clickdRow.find("#ReturnQuantity").val();
        var IsDisabled = $("#DisableSubItem").val();
        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + ProductId + "]").closest('tr')
        rows.each(function () {
            var row = $(this);
            var List = {};
            debugger;
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.avl_stock = row.find('#subItemAvlStk').val();
            List.ShipNo = row.find('#subItemSrcDocNo').val();
            List.ShipDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        });
    }
    else if (flag == "ReturnAvlQty") {
        var ProductNm = clickdRow.find("#ItemName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();

        var ShipNo = clickdRow.find("#ShipNumber").val();
        var ShipDt = clickdRow.find("#HdnShipDate").val();
        Sub_Quantity = clickdRow.find("#AvailableQuantity").val();
        var IsDisabled = "Y";
    }
    else {
        var ProductNm = clickdRow.find("#ItemName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();
        //var PinvNo = $("#ddlDocumentNumber option:selected").text();
        var ShipNo = clickdRow.find("#ShipNumber").val();
        var ShipDt = clickdRow.find("#HdnShipDate").val();
        Sub_Quantity = clickdRow.find("#ShipQuantity").val();
        var IsDisabled = "Y";
    }
    var ShipDt = clickdRow.find("#HdnShipDate").val();
    //var date = ShipDate.split("-");
    //var ShipDt = date[2] + '-' + date[1] + '-' + date[0];

    var hd_Status = $("#hdSRTStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            ShNo: ShipNo,
            ShDt: ShipDt,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            wh_id: wh_id,
            WhType: WhType,
            SinvNo: PinvNo,
            src_type: src_type

        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            if (flag != "AdSRTReturnQty") {
                $("#subSrcDocNo").css('display', 'block');
                $("#subSrcDocdate").css('display', 'block');
            }   
            if (flag == "SRTReturnQty") {
                $("#subWHName").css('display', 'block');
            }
            $("#Sub_SrcDocNo").val(ShipNo);
            $("#Sub_SrcDocDate").val(ShipDt);
            $("#hdSub_SrcDocDate").val(ShipDt);
            $("#Sub_Quantity").val(Sub_Quantity);
            $("#hdSub_WHId").val(wh_id);
            $("#sub_WHName").val(WhName);
        }
    });

}

function CheckValidations_forSubItems() {
    debugger
    var src_type = $('#src_type').val();
    if (src_type == "A") {
        return Cmn_CheckValidations_forSubItems("AdSReturnItmDetailsTbl", "", "hdItemId", "ReturnQuantity", "SubItemReturnQty", "Y");
    }
    else {
        return Cmn_CheckValidations_forSubItems("SReturnItmDetailsTbl", "", "hdItemId", "ReturnQuantity", "SubItemReturnQty", "Y");
    }
}
function ResetWorningBorderColor() {
    debugger;
    return PRCheckValidations_forSubItems("RtWh_Sub_ItemDetailTbl", "", "RtWh_Sub_ProductlId", "RtWhSubItm_ReturnQty", "SubItemReturnQty", "N");
}
function PRCheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var SrcDocNo = $("#Sub_SrcDocNo").val();
    var SrcDocDt = $("#Sub_SrcDocDate").val();
    //var SrcDocWhId = $("#hdSub_WHId").val();
    var Sub_ProductlId = $('#Sub_ProductlId').val();
    var src_type = $('#src_type').val();
    let clickedrow=""
    if (src_type == "A") {
        clickedrow = $("#AdSReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + Sub_ProductlId + "']").closest("tr");
    }
    else {
        clickedrow = $("#SReturnItmDetailsTbl > tbody > tr #hdItemId[value='" + Sub_ProductlId + "']").closest("tr");
    }
    clickedrow.find("#SubItemReturnQty").css("border", "");

    var flag = "N";
    $("#" + Main_table + " tbody tr ").each(function () {
        var PPItemRow = $(this);
        var item_id;
        item_id = $("#" + Item_field_id).val();

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var SrcDocWhId = PPItemRow.find("#RtWhSubItm_WhId").val();
        var sub_item = $("#RtWh_Subitem").val();
        var Sub_Quantity = 0;

        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr');
            //.find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr');

        /*$("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {*/
        rows.each(function () {

            var Crow = $(this).closest("tr");
            //var subItemId = Crow.find("#subItemId").val();
            var subItemQty = Crow.find("#subItemQty").val();
            var subItemSrcDocNo = Crow.find("#subItemSrcDocNo").val();
            var subItemWhId = Crow.find("#subItemWhId").val();

            if (subItemSrcDocNo == SrcDocNo && subItemWhId == SrcDocWhId) {
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            }
            else {
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            }
            //Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        });

        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
        }
    });
    $("#SubItemReturnQty").css("border", "");
    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}

function fn_PRTCustomReSetSubItemData(itemId) {
    debugger
    var Quantity = $("#Sub_Quantity").val();
    var SrcDocNo = $("#Sub_SrcDocNo").val();
    var SrcDocDt = $("#Sub_SrcDocDate").val();
    var SrcDocWhId = $("#hdSub_WHId").val();
    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        var subItemAvlQty = Crow.find("#subItemAvlStk").val();
        //var subItemReqQty = Crow.find("#subItemReqQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();

        /* var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;*/
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr').length;
            //.find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr').length;

        if (len > 0) {
            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr');
                //.find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr');

            /*$("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {*/
            rows.each(function () {
                var InnerCrow = $(this).closest("tr");
                //if (InnerCrow.find("#subItemWhId").val() == SrcDocWhId) {
                    //var ItemId = InnerCrow.find("#ItemId").val();
                    InnerCrow.find("#subItemQty").val(subItemQty);
                    InnerCrow.find("#subItemAvlStk").val(subItemAvlQty);
                    InnerCrow.find("#subItemSrcDocNo").val(SrcDocNo);
                    InnerCrow.find("#subItemSrcDocDate").val(SrcDocDt);
                    InnerCrow.find("#subItemWhId").val(SrcDocWhId);
                //}
               

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlStk" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${SrcDocNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${SrcDocDt}'></td>
                            <td><input type="text" id="subItemWhId" value='${SrcDocWhId}'></td>
                            
                        </tr>`);

        }

    });

}
function fn_PRTCustomReSetSubItemData1(itemId) {
    debugger
    var Quantity = $("#Sub_Quantity").val();
    var SrcDocNo = $("#Sub_SrcDocNo").val();
    var SrcDocDt = $("#Sub_SrcDocDate").val();
    var SrcDocWhId = $("#hdSub_WHId").val();
    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        var subItemAvlQty = Crow.find("#subItemAvlStk").val();
        //var subItemReqQty = Crow.find("#subItemReqQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();
        //var subItemPendQty = Crow.find("#subItemPendQty").val();

        /* var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;*/
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr')
            .find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr').length;

        if (len > 0) {
            var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #subItemId[value=" + subItemId + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr')
                .find("#subItemWhId[value='" + SrcDocWhId + "']").closest('tr');

            /*$("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {*/
            rows.each(function () {
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemQty").val(subItemQty);
                InnerCrow.find("#subItemAvlStk").val(subItemAvlQty);
                InnerCrow.find("#subItemSrcDocNo").val(SrcDocNo);
                InnerCrow.find("#subItemSrcDocDate").val(SrcDocDt);
                InnerCrow.find("#subItemWhId").val(SrcDocWhId);

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemAvlStk" value='${subItemAvlQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${SrcDocNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${SrcDocDt}'></td>
                            <td><input type="text" id="subItemWhId" value='${SrcDocWhId}'></td>
                            
                        </tr>`);

        }

    });

}
function SRT_SubItemList() {
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        debugger;
        var Qty = row.find('#subItemQty').val();
        if (parseFloat(CheckNullNumber(Qty)) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            if (row.find("#subItemAvlStk").val() == "undefined") {
                List.avl_stock = "0";
            }
            else {
                List.avl_stock = IsNull(row.find('#subItemAvlStk').val(), 0);
            }
            List.ShipNo = row.find('#subItemSrcDocNo').val();
            List.ShipDate = row.find('#subItemSrcDocDate').val();
            List.WhId = row.find('#subItemWhId').val();
            NewArr.push(List);
        }
    });
    return NewArr;
}
function DeleteSubItemQtyDetail(item_id, ShipNo, ShipDt) {
    debugger;

    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + ShipNo + "']").closest('tr').length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                    .find("#subItemSrcDocNo[value='" + ShipNo + "']").closest('tr').remove();
            }
        }
    }

}
/***--------------------------------Sub Item Section End-----------------------------------------***/
/*------------------------All PopupValidation For SubItem ----------------*/
function ItemtbltoRetrnQtyTblToRtwhtosubitmValidation(ShowMessage, ItmCode, IconShipNumber) {
    debugger;
    var flag = "N";
    var EffectedRows;
    if (ItmCode != null && ItmCode != "") {
        EffectedRows = $("#SReturnItmDetailsTbl tbody tr #hdItemId[value='" + ItmCode + "']").closest("tr")
            .find("#ShipNumber[value='" + IconShipNumber + "']").closest("tr");
    } else {
        EffectedRows = $("#SReturnItmDetailsTbl tbody tr");
    }

    EffectedRows.each(function () {
        var Row = $(this);

        var RowNo = Row.find("#SpanRowId").text();
        var Arr = [];
        var ArrFiltData = [];
        var Item_Id = Row.find("#hdItemId").val();
        var ShipNo = Row.find("#ShipNumber").val();
        var ShipDt = Row.find("#HdnShipDate").val();
        //var ShipDt = ShipDate.split("-").reverse().join("-");
        var FReturnItemDetails = JSON.parse(sessionStorage.getItem("ItemRetQtyDetails"));
        if (ItmCode != null && ItmCode != "") {
            var NewArr = [];
            $("#ReturnedQtyDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();

                debugger;
                var wh_id = currentRow.find("#wh_id" + Sno).val();
                var Lot = currentRow.find("#Lot" + Sno).val();
                var Batch = currentRow.find("#Batch1" + Sno).val();
                var Serial = currentRow.find("#Serial1" + Sno).val();
                var RetQty = currentRow.find("#IconReturnQtyinput" + Sno).val();

                NewArr.push({
                    IconShipNumber: ShipNo, IconShipDate: ShipDt, ItmCode: Item_Id, wh_id: wh_id
                    , Lot: Lot, Batch: Batch, Serial: Serial, RetQty: RetQty
                })
            });
            FReturnItemDetails = NewArr;
        }

        if (FReturnItemDetails != null && FReturnItemDetails != "") {
            if (FReturnItemDetails.length > 0) {
                debugger;
                var ArrFiltData = FReturnItemDetails;
                var ArrFilt_Data = ArrFiltData.filter(v => v.ItmCode == Item_Id && v.IconShipNumber == ShipNo && v.IconShipDate == ShipDt && v.RetQty > 0)
                if (ArrFilt_Data.length > 0) {
                    var Array = [];
                    for (j = 0; j < ArrFilt_Data.length; j++) {
                        debugger;
                        var WhId = ArrFilt_Data[j].wh_id;
                        var return_qty = ArrFilt_Data[j].RetQty;

                        var CheckWh = Array.filter(v => v.wh_id == WhId);
                        if (CheckWh.length > 0) {
                            let Index = Array.findIndex(p => p.wh_id == WhId);
                            //Arr[Index].avl_qty = parseFloat(Arr[Index].avl_qty) + parseFloat(avl_qty);
                            Array[Index].return_qty = parseFloat(Array[Index].return_qty) + parseFloat(return_qty);
                        } else {
                            Array.push({ Item_Id: Item_Id, ShipNo: ShipNo, ShipDt: ShipDt, wh_id: WhId, return_qty: return_qty });
                        }
                    }
                    for (k = 0; k < Array.length; k++) {
                        let itmId = Array[k].Item_Id;
                        let ShipNo = Array[k].ShipNo;
                        let Shipdt = Array[k].ShipDt;
                        let whid = Array[k].wh_id;
                        let rtrnQty = Array[k].return_qty;
                        //var sub_item = $("#RtWh_Subitem").val();
                        //var sub_item = $("#Iconsub_item").val(); 
                        var sub_item = Row.find("#sub_item").val();
                        var Sub_Quantity = 0;
                        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + itmId + "]").closest('tr')
                            .find("#subItemSrcDocNo[value='" + ShipNo + "']").closest('tr')
                            .find("#subItemSrcDocDate[value='" + Shipdt + "']").closest('tr');
                            //.find("#subItemWhId[value='" + whid + "']").closest('tr');


                        rows.each(function () {
                            var Crow = $(this).closest("tr");
                            //var subItemId = Crow.find("#subItemId").val();
                            var subItemQty = Crow.find("#subItemQty").val();
                            Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
                        });
                        var SubItemButton = "SubItemReturnQty";
                        if (ItmCode != null && ItmCode != "") {
                            if (sub_item == "Y") {
                                if (rtrnQty != Sub_Quantity) {
                                    flag = "Y";
                                    $("#" + SubItemButton).css("border", "1px solid red");

                                } else {
                                    $("#" + SubItemButton).css("border", "");
                                }
                            }
                        }
                        else {
                            SubItemButton = "ReturnQuantityDetail";
                            if (sub_item == "Y") {
                                if (rtrnQty != Sub_Quantity) {
                                    flag = "Y";
                                    Row.find("#" + SubItemButton).css("border", "1px solid red");

                                } else {
                                    Row.find("#" + SubItemButton).css("border", "");
                                }
                            }
                        }

                    }
                }
            }
        }

        else {
            flag == "Y"
        }
    });
    if (flag == "Y") {
        if (ShowMessage == "Y") {
            //if (ItmCode != null && ItmCode != "") { //Called when Sending One Item to validate from return Qty Dtl Popup
            //    $("#SaveAndExitBtn").attr("data-dismiss", "modal");
            //}
            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        //if (ItmCode != null && ItmCode != "") { //Called when Sending One Item to validate from return Qty Dtl Popup
        //    $("#SaveAndExitBtn").attr("data-dismiss", "");
        //}
        return true;
    }
}


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
function FilterItemDetail(e) {//added by Prakash Kumar on 17-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SReturnItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}
/***-----------------Add for Batch Detail strat----------------------***/
function ItemStockBatchWise(el, evt) {
    debugger;
    var QtyDigit = $("#QtyDigit").text()
    var currentrow = $(evt.target).closest('tr');
    var SNohf = currentrow.find("#SNohiddenfiled").val();
    var item_id = currentrow.find("#hdItemId").val();
    var ExpireAble_item = currentrow.find("#hfexpiralble").val();
    if (ExpireAble_item == "Y") {
        $("#spanexpiryrequire").show();
    }
    else {
        $("#spanexpiryrequire").hide();
    }
    var uom_name = currentrow.find("#UOM").val();
    var ReceivedQuantity = currentrow.find("#ReturnQuantity").val();
    var Item_name = currentrow.find("#ItemName" + SNohf + " option:selected").text();
    var hdn_BatchCommand = $("#hdn_BatchCommand").val();
    var GreyScale = "";

    ResetBatchDetailValExternal();

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
            $("#BtnBatchDetail").attr("data-target", "#BatchNumber");
        }
        var rowIdx = 0;
        var Count = $("#SaveItemBatchTbl tbody tr").length;
        if (Count != null && Count != 0) {
            if (Count > 0) {
                $("#BatchDetailTbl >tbody >tr").remove();
                $("#SaveItemBatchTbl tbody tr").each(function () {
                    var SaveBatchRow = $(this);
                    var SUserID = SaveBatchRow.find("#BatchUserID").val();
                    var SRowID = SaveBatchRow.find("#RowSNo").val();
                    var SItemID = SaveBatchRow.find("#ItemID").val();
                    var BatchExDate = SaveBatchRow.find("#BatchExDate").val();
                    var BatchQty = SaveBatchRow.find("#BatchQty").val();
                    var BatchNo = SaveBatchRow.find("#BatchNo").val();
                    var mfg_name = SaveBatchRow.find("#MfgName").val();
                    var mfg_mrp = SaveBatchRow.find("#MfgMrp").val();
                    var mfg_date = SaveBatchRow.find("#MfgDate").val();
                    if (SNohf != null && SNohf != "") {
                        if (SItemID == item_id) {
                            var date = "";
                            if (BatchExDate != "" && BatchExDate != null) {
                                if (BatchExDate == "1900-01-01") {
                                    date = "";
                                }
                                else {
                                    date = moment(BatchExDate).format('DD-MM-YYYY');
                                }
                            }
                            RenderHtmlToBatchDetail(++rowIdx, GreyScale, BatchNo, BatchExDate, BatchQty, mfg_name, mfg_mrp, mfg_date)
                            //$('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                            // <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            //<td id="BatchNo" >${BatchNo}</td>
                            //<td id="BatchExDate" hidden="hidden">${BatchExDate}</td>
                            //<td>${date}</td>
                            //<td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(QtyDigit)}</td>
                            //</tr>`);
                        }
                    }
                    else {
                        debugger
                        if (SItemID == item_id) {
                            var date = "";
                            if (BatchExDate != "" && BatchExDate != null) {
                                date = moment(BatchExDate).format('DD-MM-YYYY');
                            }
                            RenderHtmlToBatchDetail(++rowIdx, GreyScale, BatchNo, BatchExDate, BatchQty, mfg_name, mfg_mrp, mfg_date)
                            //$('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                            // <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            //<td id="BatchNo" >${BatchNo}</td>
                            //<td id="BatchExDate" hidden="hidden">${BatchExDate}</td>
                            //<td>${date}</td>
                            //<td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(QtyDigit)}</td>
                            //</tr>`);
                        }
                    }
                })
                if (hdn_BatchCommand == "N") {
                    OnClickDeleteIconExternalReceipt();
                }
                CalculateBatchQtyTblExternalReceipt();
                if (hdn_BatchCommand == "Y") {
                    $("#BatchDetailTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        currentRow.find("#BatchDeleteIcon").css("display", "none");
                    });
                }
            }
        }
    }
}

function RenderHtmlToBatchDetail(rowIdx, GreyScale, BatchNo, ExpDate, BatchQty, mfg_name, mfg_mrp, mfg_date) {
    $('#BatchDetailTbl tbody').append(`<tr id="R${rowIdx}">
                             <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            <td id="BatchNo" >${BatchNo}</td>
                            <td id="mfg_name" >${mfg_name}</td>
                              <td id="mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
                              <td id="mfg_date" >${Cmn_FormatDate_ddmmyyyy(mfg_date)}</td>
                              <td id="mfg_date_hdn" hidden >${mfg_date}</td>
                            <td id="BatchExDate" hidden="hidden">${ExpDate}</td>
                            <td>${Cmn_FormatDate_ddmmyyyy(ExpDate)}</td>
                            <td id="BatchQty" class="num_right">${parseFloat(BatchQty).toFixed(cmn_QtyDecDigit)}</td>
                            </tr>`);
}

function RenderHtmlToSerialDetail(rowIdx, GreyScale, SerialNo, mfg_name, mfg_mrp, mfg_date) {
    $('#SerialDetailTbl tbody').append(`<tr id="R${rowIdx}">
                          <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                          <td id="SerialID" >${rowIdx}</td>
                          <td id="SerialNo" >${SerialNo}</td>
                          <td id="mfg_name" >${mfg_name}</td>
                          <td id="mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
                          <td id="mfg_date" >${Cmn_FormatDate_ddmmyyyy(mfg_date)}</td>
                          <td id="mfg_date_hdn" hidden >${mfg_date}</td>
                          </tr>`);
}

function OnChangeBatchExpiryDate_External() {
    if ($('#BatchExpiryDate').val() != "") {
        $("#SpanBatchExDate").css("display", "none");
        $("#BatchExpiryDate").css("border-color", "#ced4da");
    }
    else {
        if ($("#hfexpiryflag").val() == 'Y') {
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#valueReq").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
}
function OnKeyPressBatchNoExter_rec(e) {
    debugger
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
}
function OnClickAddNewBatchDetailExternalReceipt() {
    debugger;
    var BatchNumber = $("#txtBatchNumber").val();
    var BatchExpiryDate = $("#BatchExpiryDate").val();
    var BatchReceivedQuantity = $("#BatchReceivedQuantity").val();
    var mfg_name = $("#BtMfgName").val();
    var mfg_mrp = $("#BtMfgMrp").val();
    var mfg_date = $("#BtMfgDate").val();
    var rowIdx = 0;
    var ValidInfo = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    if ($('#txtBatchNumber').val() == "") {
        ValidInfo = "Y";
        $("#txtBatchNumber").css("border-color", "Red");
        $('#SpanBatchNo').text($("#valueReq").text());
        $("#SpanBatchNo").css("display", "block");
    }
    else {
        var BatchNo = $('#txtBatchNumber').val().toUpperCase();
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblBtachNo = currentRow.find("#BatchNo").text().toUpperCase();
            if (tblBtachNo == BatchNo) {
                $('#SpanBatchNo').text($("#valueduplicate").text());
                $("#SpanBatchNo").css("display", "block");
                $("#txtBatchNumber").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
    }
    if ($('#BatchExpiryDate').val() == "") {
        if ($("#hfexpiryflag").val() == 'Y') {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#valueReq").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
    var ExDate = $('#BatchExpiryDate').val().split('-');
    if ($('#BatchExpiryDate').val() != "") {
        if (ExDate[0].length > 4) {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatchExDate").css("display", "block");
        }
        var currentdate = moment().format('YYYY-MM-DD');
        if ($('#BatchExpiryDate').val() < currentdate) {
            ValidInfo = "Y";
            $("#BatchExpiryDate").css("border-color", "Red");
            $('#SpanBatchExDate').text($("#JC_InvalidDate").text());
            $("#SpanBatchExDate").css("display", "block");
        }
    }
    var BatchReceived_qty = $("#BatchQuantity").val();
    if (BatchReceived_qty == "" || BatchReceived_qty == null || parseFloat(BatchReceived_qty) == parseFloat(0)) {
        ValidInfo = "Y";
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
    }
    else {
        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
        }

    }
    if (ValidInfo == "Y") {
        return false;
    }

    var date = $("#BatchExpiryDate").val();
    if (date != null && date != "") {
        date = moment(date).format('DD-MM-YYYY');
    }
    else {
        date = "";
    }
    RenderHtmlToBatchDetail(++rowIdx, "", $("#txtBatchNumber").val(), $("#BatchExpiryDate").val(), BatchReceived_qty, mfg_name, mfg_mrp, mfg_date)
//    $('#BatchDetailTbl tbody').append(`<tr id="R${++rowIdx}">
// <td class=" red center"> <i class="fa fa-trash deleteIcon" id="BatchDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
//<td id="BatchNo" >${$("#txtBatchNumber").val()}</td>
//<td id="BatchExDate" hidden="hidden">${$("#BatchExpiryDate").val()}</td>
//<td>${date}</td>
//<td id="BatchQty" class="num_right">${parseFloat(CheckNullNumber(BatchReceived_qty)).toFixed(QtyDecDigit)}</td>
//</tr>`);
    ResetBatchDetailValExternal();
    CalculateBatchQtyTblExternalReceipt();
    OnClickDeleteIconExternalReceipt();
}
function CalculateBatchQtyTblExternalReceipt() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#BatchDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);

        if (tblQty == null || tblQty == "") {
            tblQty = 0;
        }
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
    });

    $('#BatchQtyTotal').text(parseFloat(TotalReceiQty).toFixed(QtyDecDigit));
}
function OnClickDeleteIconExternalReceipt() {
    $('#BatchDetailTbl tbody').on('click', '.deleteIcon', function () {
        //debugger;
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
        debugger;
        //var SNo = $(this).closest('tr')[0].cells[2].children[0].value;
        //var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        CalculateBatchQtyTblExternalReceipt();

    });
}
function ResetBatchDetailValExternal() {
    $('#BatchQuantity').val("");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
}
function OnChangeBatchNumber(e) {
    var data = Cmn_RemoveSpacesAndTabs(e.target.value);
    $("#txtBatchNumber").val(data)
}
function OnKeyDownBatchNoExter_rec(e) {
    debugger
    if (Cmn_Block_Keys(e, "Space") == false) {
        return false;
    }
}
function OnChangeBatchQtyExternal() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var BatchReceivedQuantity = $("#BatchQuantity").val();
    if (BatchReceivedQuantity == "" || BatchReceivedQuantity == "0" || BatchReceivedQuantity == null || parseFloat(BatchReceivedQuantity) == parseFloat(0)) {
        $("#BatchQuantity").css("border-color", "Red");
        $('#SpanBatchQty').text($("#valueReq").text());
        $("#SpanBatchQty").css("display", "block");
        return false;
    }
    else {
        $("#SpanBatchQty").css("display", "none");
        $("#BatchQuantity").css("border-color", "#ced4da");

        var BQty = parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit);
        var ReceiQty = parseFloat($('#BatchReceivedQuantity').val()).toFixed(QtyDecDigit);

        var TotalReceiQty = parseFloat(0).toFixed(QtyDecDigit);
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblQty = parseFloat(currentRow.find("#BatchQty").text()).toFixed(QtyDecDigit);
            if (tblQty == null || tblQty == "") {
                tblQty = 0;
            }
            TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(tblQty)).toFixed(QtyDecDigit);
        });
        TotalReceiQty = (parseFloat(TotalReceiQty) + parseFloat(BQty)).toFixed(QtyDecDigit);
        if (parseFloat(ReceiQty) < parseFloat(TotalReceiQty)) {
            $("#BatchQuantity").css("border-color", "Red");
            $('#SpanBatchQty').text($("#BatchQuantityExceeds").text());
            $("#SpanBatchQty").css("display", "block");
            return false;
        }
        else {
            $('#BatchQuantity').val(parseFloat($('#BatchQuantity').val()).toFixed(QtyDecDigit));
        }
    }

}
function OnClickSaveAndCloseGRN() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var RowSNo = $("#hfItemSNo").val();
    var ItemID = $('#BatchItemID').val();
    var ReceiBQty = parseFloat($("#BatchReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalBQty = parseFloat($("#BatchQtyTotal").text()).toFixed(QtyDecDigit);
    if (parseFloat(ReceiBQty) == parseFloat(TotalBQty)) {
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "modal");
        ValidateEyeColor($("#ItemName" + RowSNo).closest("tr"), "BtnBatchDetail", "N");
        debugger;
        var SelectedItem = $("#BatchItemID").val();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#ItemID").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#BatchDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var BatchQty = currentRow.find("#BatchQty").text();
            var BatchNo = currentRow.find("#BatchNo").text();
            var BatchExDate = currentRow.find("#BatchExDate").text();
            var MfgName = currentRow.find("#mfg_name").text();
            var MfgMrp = currentRow.find("#mfg_mrp").text();
            var MfgDate = currentRow.find("#mfg_date_hdn").text();
            $('#SaveItemBatchTbl tbody').append(
                `<tr>                
                    <td><input type="text" id="ItemID" value="${ItemID}" /></td>
                    <td><input type="text" id="BatchNo" value="${BatchNo}" /></td>
                    <td><input type="text" id="BatchExDate" value="${BatchExDate}" /></td>
                    <td><input type="text" id="BatchQty" value="${BatchQty}" /></td>
                    <td><input type="text" id="MfgName" value="${MfgName}" /></td>
                    <td><input type="text" id="MfgMrp" value="${MfgMrp}" /></td>
                    <td><input type="text" id="MfgDate" value="${MfgDate}" /></td>
                </tr>`
            );
        });
    }
    else {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#BatchSaveAndExitBtn").attr("data-dismiss", "");
    }
}
function RemoveSessionNew() {
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
}
function OnClickBatchResetBtnGRN() {
    var QtyDecDigit = $("#QtyDigit").text();
    ResetBatchDetailValExternal();
    $('#BatchDetailTbl tbody tr').remove();
    $('#BatchQtyTotal').text(parseFloat(0).toFixed(QtyDecDigit));

    $("#SpanBatchQty").css("display", "none");
    $("#BatchQuantity").css("border-color", "#ced4da");
    $("#SpanBatchNo").css("display", "none");
    $("#txtBatchNumber").css("border-color", "#ced4da");
    $("#SpanBatchExDate").css("display", "none");
    $("#BatchExpiryDate").css("border-color", "#ced4da");
}
function OnClickDiscardAndExitGRN() {
    OnClickBatchResetBtnGRN();
}
/***------------------Add for Batch Detail End-------------------***/
/***-----------------Add for Serial Detail strat-------------------***/
function ItemStockSerialWise(evt, e) {
    $("#SerialNo").val("");
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
    var QtyDecDigit = $("#QtyDigit").text();
    var currentrow = $(e.target).closest("tr");
    var ItemName = "";
    var ItemID = "";
    var ItemUOM = "";
    var ReceiveQty = 0;
    var GreyScale = "";
    var hdn_BatchCommand = $("#hdn_BatchCommand").val();
    if (hdn_BatchCommand == "N") {
        $("#SerialResetBtn").css("display", "block")
        $("#SerialSaveAndExitBtn").css("display", "block")
        $("#SerialDiscardAndExitBtn").css("display", "block")
        $("#Closebtn").css("display", "none")
        $("#DivAddNewSerial").css("display", "block")
        $("#SerialNo").attr("disabled", false);

    }
    else {
        GreyScale = "style='filter: grayscale(100%)'";
        $("#DivAddNewSerial").css("display", "none");
        $("#SerialNo").attr("disabled", true);
        $("#SerialResetBtn").css("display", "none")
        $("#SerialSaveAndExitBtn").css("display", "none")
        $("#SerialDiscardAndExitBtn").css("display", "none")
        $("#Closebtn").css("display", "block")
    }
    var SNohf = currentrow.find("#SNohiddenfiled").val();
    ItemID = currentrow.find("#hdItemId").val();
    ItemUOM = currentrow.find("#UOM").val();
    ReceiveQty = currentrow.find("#ReturnQuantity").val();
    ItemName = currentrow.find("#ItemName" + SNohf + " option:selected").text();
    var ReceQty = parseFloat(ReceiveQty).toFixed(QtyDecDigit);
    if (ReceQty == "NaN" || ReceQty == "" || ReceQty == "0") {
        $("#BtnSerialDetail").attr("data-target", "");
        return false;
    }
    else {
        $("#BtnSerialDetail").attr("data-target", "#SerialDetail");
    }
    $("#SerialItemName").val(ItemName);
    $("#serialUOM").val(ItemUOM);
    $("#SerialReceivedQuantity").val(ReceQty);
    $("#hfSItemSNo").val(SNohf);
    $("#hfItemSNo").val(SNohf);
    $("#hfSItemID").val(ItemID);
    var rowIdx = 0;
    var Count = $("#SaveItemSerialTbl >tbody >tr").length;
    if (Count != null) {
        if (Count > 0) {
            $("#SerialDetailTbl >tbody >tr").remove();
            $("#SaveItemSerialTbl >tbody >tr").each(function () {
                var SerialCurrentRow = $(this);
                var SItemID = SerialCurrentRow.find("#SerialItemID").val();
                var SerialNo = SerialCurrentRow.find("#Serial_SerialNo").val();
                var mfg_name = SerialCurrentRow.find("#Serial_MfgName").val();
                var mfg_mrp = SerialCurrentRow.find("#Serial_MfgMrp").val();
                var mfg_date = SerialCurrentRow.find("#Serial_MfgDate").val();
                if (SNohf != null && SNohf != "") {
                    if (SItemID == ItemID) {
                        RenderHtmlToSerialDetail(++rowIdx, GreyScale, SerialNo, mfg_name, mfg_mrp, mfg_date);
                        //$('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                        //  <td class=" red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                        //  <td id="SerialID" >${rowIdx}</td>
                        //  <td id="SerialNo" >${SerialNo}</td>
                        //  </tr>`);
                    }
                    else {
                        if (SItemID == ItemID) {
                            RenderHtmlToSerialDetail(++rowIdx, GreyScale, SerialNo, mfg_name, mfg_mrp, mfg_date);
                            //$('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
                            //  <td class="red center"> <i class="fa fa-trash deleteIcon" ${GreyScale} id="SerialDeleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                            //  <td id="SerialID" >${rowIdx}</td>
                            //  <td id="SerialNo" >${SerialNo}</td>
                            //  </tr>`);
                        }
                    }
                }
            })
            if (hdn_BatchCommand == "N") {
                OnClickSerialDeleteIcon_ExternalReceipt();
            }
            if (hdn_BatchCommand == "Y") {
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    currentRow.find("#SerialDeleteIcon").css("display", "none");
                });
            }
        }
        else {
            $("#SerialDetailTbl >tbody >tr").remove();
        }
    }
    else {
        $("#SerialDetailTbl >tbody >tr").remove();
    }
}
function OnClickAddNewSerialDetail_ExternalReceipt() {
    var rowIdx = 0;
    debugger;
    var ValidInfo = "N";
    var QtyDecDigit = $("#QtyDigit").text();
    var mfg_name = $("#SrMfgName").val();
    var mfg_mrp = $("#SrMfgMrp").val();
    var mfg_date = $("#SrMfgDate").val();
    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var AcceptLength = parseInt(0);
    $("#SerialDetailTbl >tbody >tr").each(function () {
        AcceptLength = parseInt(AcceptLength) + 1;
    });
    //AcceptLength = $("#SerialDetailTbl >tbody >tr").length();
    if ($('#SerialNo').val() == "") {
        ValidInfo = "Y";
        $("#SerialNo").css("border-color", "Red");
        $('#SpanSerialNo').text($("#valueReq").text());
        $("#SpanSerialNo").css("display", "block");
    }
    else {
        $("#SpanSerialNo").css("display", "none");
        $("#SerialNo").css("border-color", "#ced4da");

        var SerialNo = $('#SerialNo').val().toUpperCase();
        $("#SerialDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblSerialNo = currentRow.find("#SerialNo").text().toUpperCase();
            if (tblSerialNo == SerialNo) {
                $('#SpanSerialNo').text($("#valueduplicate").text());
                $("#SpanSerialNo").css("display", "block");
                $("#SerialNo").css("border-color", "Red");
                ValidInfo = "Y";
                return false;
            }
        });
    }
    if (ValidInfo == "Y") {
        return false;
    }
    else {
        if (parseInt(AcceptLength) != parseInt(ReceiSQty)) {
            var TblLen = $('#SerialDetailTbl tbody tr').length;
            RenderHtmlToSerialDetail((TblLen + 1), "", $("#SerialNo").val(), mfg_name, mfg_mrp, mfg_date);
            //$('#SerialDetailTbl tbody').append(`<tr id="R${++rowIdx}">
            //   <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="SerialDeleteIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
            //   <td id="SerialID" >${TblLen + 1}</td>
            //   <td id="SerialNo" >${$("#SerialNo").val()}</td>
            //   </tr>`);
            $('#SerialNo').val("").focus();
            OnClickSerialDeleteIcon_ExternalReceipt();
        }
    }
}
function OnClickSerialDeleteIcon_ExternalReceipt() {
    $('#SerialDetailTbl tbody').on('click', '.deleteIcon', function () {
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
        $(this).closest('tr').remove();
        debugger;
        ResetSerialNoAfterDelete_ER();
    });
}
function ResetSerialNoAfterDelete_ER() {
    debugger;
    var rowIdx = 0;
    $("#SerialDetailTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        currentRow.find("#SerialID").text(++rowIdx);
    });
}
function OnClickSerialSaveAndClose_ExternalReceipt() {
    var QtyDecDigit = $("#QtyDigit").text();
    var ItemID = $('#hfSItemID').val();
    var RowSNo = $("#hfItemSNo").val();
    var ReceiSQty = parseFloat($("#SerialReceivedQuantity").val()).toFixed(QtyDecDigit);
    var TotalRecQty = parseFloat(ReceiSQty);
    var TotalSQty = parseFloat($("#SerialDetailTbl >tbody >tr").length).toFixed(QtyDecDigit);
    if (parseFloat(TotalRecQty) == parseFloat(TotalSQty)) {
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "modal");
        ValidateEyeColor($("#ItemName" + RowSNo).closest("tr"), "BtnSerialDetail", "N");
        var SelectedItem = $("#hfSItemID").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#SerialItemID").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#SerialDetailTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var SerialNo = currentRow.find("#SerialNo").text();
            var MfgName = currentRow.find("#mfg_name").text();
            var MfgMrp = currentRow.find("#mfg_mrp").text();
            var MfgDate = currentRow.find("#mfg_date_hdn").text();
            $('#SaveItemSerialTbl tbody').append(`<tr>                  
                   <td><input type="text" id="SerialItemID" value="${ItemID}" /></td>
                   <td><input type="text" id="Serial_SerialNo" value="${SerialNo}" /></td>
                   <td><input type="text" id="Serial_MfgName" value="${MfgName}" /></td>
                   <td><input type="text" id="Serial_MfgMrp" value="${MfgMrp}" /></td>
                   <td><input type="text" id="Serial_MfgDate" value="${MfgDate}" /></td>
                   </tr>`
            );
        });
    }
    else {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
        $("#SerialSaveAndExitBtn").attr("data-dismiss", "");
    }
}
function OnClickSerialResetBtn_ExternalReceipt() {
    $('#SerialNo').val("");
    $('#SerialDetailTbl tbody tr').remove();
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
function OnClickSerialDiscardAndExit_ExternalReceipt() {
    OnClickSerialResetBtnGRN();
}
function OnKeyPressSerialNo_ExternalReceipt() {
    $("#SpanSerialNo").css("display", "none");
    $("#SerialNo").css("border-color", "#ced4da");
}
/***-------------Add for Serial Detail End-------------***/
function OnClickSaveAndExit(MGST) {
    debugger;
    var TotalTaxAmount = CheckNullNumber($('#TotalTaxAmount').text());
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);

    var tax_recov_amt = 0;
    var tax_non_recov_amt = 0;
    //var TaxOn = $("#HdnTaxOn").val();
    //var TaxOn = "Item";
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
    debugger;
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
            debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                Cmn_click_Oc_chkroundoff(null, currentRow);
            }
        });
        Calculate_OCAmount();
    }
    else {
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var ItmCode = currentRow.find("#hdItemId").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
            if (ItmCode == TaxItmCode) {
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                //if (currentRow.find("#TaxExempted").is(":checked")) {
                //    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                //    tax_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                //    tax_non_recov_amt = parseFloat(0).toFixed(ValDecDigit);
                //}
                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);

                currentRow.find("#item_tax_amt").val(TaxAmt);
                //var itmocamt = currentRow.find("#item_oc_amt").val();//05-02-2025
                //var itmocamt = currentRow.find("#TxtOtherCharge").val();
                //if (itmocamt != null && itmocamt != "") {
                //    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(ValDecDigit);
                //}
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_ass_val").val()))).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt)).toFixed(ValDecDigit);
                //currentRow.find("#Txt_item_tax_rec_amt").val(parseFloat(tax_recov_amt).toFixed(ValDecDigit));
                //currentRow.find("#Txt_item_tax_non_rec_amt").val(parseFloat(tax_non_recov_amt).toFixed(ValDecDigit));
                currentRow.find("#ReturnValue").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
                //debugger;
                //debugger;
                //let LandedCostValue = parseFloat(CheckNullNumber(AssessableValue)) + parseFloat(CheckNullNumber(tax_non_recov_amt));
                //let LandedCostValuePerPc = LandedCostValue / parseFloat(CheckNullNumber(InvQty));
                //currentRow.find("#TxtLandedCost").val(parseFloat(LandedCostValue).toFixed(ValDecDigit));
                //currentRow.find("#TxtLandedCostPerPc").val(parseFloat(CheckNullNumber(LandedCostValuePerPc)).toFixed(LddCostDecDigit));
            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
    //if (MGST == "MGST") {
        //let SuppId = $("#SupplierName").val();
        //let GrossAmt = CheckNullNumber($("#TxtGrossValue").val());
        //if (GrossAmt != 0) {
        //    AutoTdsApply(SuppId, GrossAmt).then(() => {
        //        GetAllGLID();
        //    });
        //}
        //else {
            //GetAllGLID();
        //}
    //}

    CalculateAmount();
    if (TaxOn != "OC") {
        GetAllGLID();
    }
    $('#BtnTxtCalculation').css('border', '"#ced4da"');
}
function OnClickTaxCalculationBtn(e) {
    debugger;
    var SOItemListName = "#ItemName";
    var SNohiddenfiled = "#SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentrow = $(e.target).closest('tr');
        var Itm_ID = currentrow.find("#hdItemId").val();
        var item_gross_val = currentrow.find("#item_ass_val").val();
        //var mr_no = currentrow.find("#QuotationNumber").val();
        //var mr_date = currentrow.find("#QuotationDate").val();
        var RowSNo = currentrow.find("#SNohiddenfiled").val();
        $("#HdnTaxOn").val("Item");
        $("#TaxCalcItemCode").val(Itm_ID);
        $("#Tax_AssessableValue").val(item_gross_val);
        //$("#TaxCalcGRNNo").val(mr_no);
        //$("#TaxCalcGRNDate").val(mr_date);
        $("#HiddenRowSNo").val(RowSNo)

        //if (currentrow.find("#ManualGST").is(":checked")) {
        //    $("#taxTemplate").text("GST Slab");
        //}
        //else {
        //    $("#taxTemplate").text("Template")
        //}
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    //if (GstApplicable == "Y") {
    //    if ($("#Disable").val() == "Y") {
    //        $("#Tax_Template").attr("disabled", true);
    //        $("#SaveAndExitBtn").prop("disabled", true);
    //    }
    //    else {
    //        if (currentrow.find("#ManualGST").is(":checked")) {
    //            $("#Tax_Template").attr("disabled", false);
    //            $("#SaveAndExitBtn").prop("disabled", false);
    //        }
    //        else {
    //            $("#Tax_Template").attr("disabled", true);
    //            $("#SaveAndExitBtn").prop("disabled", true);
    //        }
    //    }
    //}
}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/
    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}
function OnClickReplicateOnAllItems() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var GRNNo = "";
    var GRNDate = "";
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
        //TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "AdSReturnItmDetailsTbl";
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //
                var currentRow = $(this);
                var GRNNoTbl;
                var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                GRNNoTbl = "";
                GRNDateTbl = "";
                ItemCode = currentRow.find("#hdItemId").val();
                AssessVal = currentRow.find("#item_ass_val").val();


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
                            if (CitmTaxItmCode != TaxItmCode) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode ) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            //if (CitmTaxItmCode != TaxItmCode) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                            //if (CitmTaxItmCode == TaxItmCode) {
                            //    if (TaxOn != "OC") {
                            //        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //    }
                            //}
                            //if (CitmTaxItmCode != TaxItmCode) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ DocNo: DocNo, DocDate: DocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
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

    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            // 
            //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
        var ItemID = currentRow.find("#hdItemId").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            //var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                            //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                            //}
                            AssessableValue = (parseFloat(currentRow.find("#item_ass_val").val())).toFixed(DecDigit);
                            NetOrderValueInBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit);
                            /* currentRow.find("#TxtNetValue").val(NetOrderValueInBase);*/
                            FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                            currentRow.find("#ReturnValue").val(FinalNetOrderValueBase);
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(DecDigit);
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    //var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                    //}
                    var FGrossAmt = (parseFloat(GrossAmt)).toFixed(DecDigit);
                    /*currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                    FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                    currentRow.find("#ReturnValue").val(FinalGrossAmt);
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#item_ass_val").val()).toFixed(DecDigit);
                currentRow.find("#item_tax_amt").val(TaxAmt);
                //var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                //if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                //    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                //}
                var FGrossAmt = (parseFloat(GrossAmt)).toFixed(DecDigit);
                /* currentRow.find("#TxtNetValue").val(FGrossAmt);*/
                FinalGrossAmt = (FGrossAmt * ConvRate).toFixed(DecDigit);
                currentRow.find("#ReturnValue").val(FinalGrossAmt);
            }
        });
        CalculateAmount();
        GetAllGLID();
    

}
function AdHocCheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Batchable = currentRow.find("#hfbatchable").val();
        var ItemID = currentRow.find("#hdItemId").val();
        var ReceivedQuantity = currentRow.find("#ReturnQuantity").val();
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
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    $("#AdSReturnItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Serialable = currentRow.find("#hfserialable").val();
        var ItemID = currentRow.find("#hdItemId").val();
        var ReceivedQuantity = currentRow.find("#ReturnQuantity").val();
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
            var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
            var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
            $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
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
                                        var Oc = parseFloat(CheckNullNumber($("#PO_OtherCharges").val()));
                                        var Tr = parseFloat(CheckNullNumber($("#TotalReturnValue").val()));
                                        $("#TotalReturnValue").val(parseFloat(Oc + Tr).toFixed(ValDecDigit));
                                        ApplyRoundOff();
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
                                    var TotalReturnValue = $("#TotalReturnValue").val();
                                    //var taxval = $("#TxtTaxAmount").val();
                                    //var ocval = CheckNullNumber($("#TxtDocSuppOtherCharges").val());
                                    var finalnetval = (TotalReturnValue).toFixed(ValDecDigit);
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
                                            }
                                            if ($("#m_round").is(":checked")) {
                                                var decval = '0.' + finnetval[1];
                                                fnetval = parseFloat(netval) - parseFloat(decval);
                                                $("#pm_flagval").val($("#m_round").val());
                                            }
                                            var roundoff_netval = Math.round(fnetval);
                                            var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                                            $("#TotalReturnValue").val(f_netval);
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
function ApplyRoundOff(flag) {
    var finalnetval = parseFloat(CheckNullNumber($("#TotalReturnValue").val())).toFixed($("#ValDigit").text());
    var netval = finalnetval;
    var fnetval = 0;
    if (parseFloat(netval) > 0) {
        var finnetval = netval.split('.');
        if (parseFloat(CheckNullNumber(finnetval[1])) > 0) {
            if ($("#p_round").is(":checked")) {
                var decval = '0.' + finnetval[1];
                var faddval = 1 - parseFloat(decval);
                fnetval = parseFloat(netval) + parseFloat(faddval);
                $("#pm_flagval").val($("#p_round").val());
            }
            if ($("#m_round").is(":checked")) {
                //var finnetval = netval.split('.');
                var decval = '0.' + finnetval[1];
                fnetval = parseFloat(netval) - parseFloat(decval);
                $("#pm_flagval").val($("#m_round").val());
            }
            if ($("#p_round").is(":checked") || $("#m_round").is(":checked")) {
                var roundoff_netval = Math.round(fnetval);
                var f_netval = parseFloat(roundoff_netval).toFixed(ValDecDigit);
                $("#TotalReturnValue").val(f_netval);
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


/*------------------Trnasport Detail add by Hina on 29-05-2025---------------------*/
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
/***-------------------For PrintOption add by Hina on 29-05-2025------------------------------------***/

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
function OnCheckedChangePrintInvHead() {
    if ($('#chkprintInvHead').prop('checked')) {
        $('#showInvHeading').val('Y');
    }
    else {
        $('#showInvHeading').val('N');
    }
}
function OnCheckedChangePrintRemarks() {/*Add by Hina on 25-09-2024*/
    if ($('#chkprintremarks').prop('checked')) {
        $('#PrintRemarks').val('Y');
    }
    else {
        $('#PrintRemarks').val('N');
    }
}
/***----------------------end-------------------------------------***/
// Added by Nidhi on 21-08-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var cust_id = $("#ddlCustomerName").val()
    Cmn_SendEmail(docid, cust_id, 'Cust');
}
function SendEmailAlert() {
    debugger;
    var Src_Type = $('#src_type').val();
    var mail_id = $("#Email").val().trim();
    var status = $('#hdSRTStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    var filepath = $('#hdfilepathpdf').val();
    var statusAM = '';
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/SalesReturn/SendEmailAlert", filepath, Src_Type)
}
function ViewEmailAlert() {
    debugger;
    var Src_Type = $('#src_type').val();
    var mail_id = $("#Email").val().trim();
    var status = $('#hdSRTStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            PrintRemarks: $("#PrintRemarks").val(),
            ShowTotalQty: $("#ShowTotalQty").val(),
            ShowWithoutSybbol: $("#ShowWithoutSybbol").val(),
            showInvHeading: $("#showInvHeading").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SR__' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/SalesReturn/SavePdfDocToSendOnEmailAlert_Ext",
                data: { SRTNo: Doc_no, SRTDate: Doc_dt, Src_Type: Src_Type, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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
   
    var status = $('#hdShipmentStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
function PrintFormate() {
    debugger;
    var Src_Type = $('#src_type').val();
    var status = $('#hdSRTStatus').val();
    var Doc_no = $("#ReturnNumber").val();
    var Doc_dt = $("#txtReturnDate").val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            PrintRemarks: $("#PrintRemarks").val(),
            ShowTotalQty: $("#ShowTotalQty").val(),
            ShowWithoutSybbol: $("#ShowWithoutSybbol").val(),
            showInvHeading: $("#showInvHeading").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SR__' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SalesReturn/SavePdfDocToSendOnEmailAlert_Ext",
            data: { SRTNo: Doc_no, SRTDate: Doc_dt, Src_Type:Src_Type,fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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
//END 21-08-2025
function OnClickSaveAndExit_OC_Btn() {
    debugger;
    CalculateAmount();
    var NetOrderValueSpe = "#TotalReturnValue";
    var NetOrderValueInBase = "#TotalReturnValue";
    /*var PO_otherChargeId = '#Tbl_OtherChargeList';*//*commented and modify by Hina on 09-07-2025*/
    var PO_otherChargeId = '#PO_OtherCharges';
    CMNOnClickSaveAndExit_OC_Btn(PO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
    GetAllGLID();
}
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
