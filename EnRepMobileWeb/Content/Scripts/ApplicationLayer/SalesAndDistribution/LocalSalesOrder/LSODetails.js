/************************************************
Javascript Name:Local Sale Order Detail
Created By:Prem
Created Date: 23-02-2021
Description: This Javascript use for the Local Sale Order Detail many function

Modified By:
Modified Date:
Description: 
*************************************************/

$(document).ready(function () {
    debugger;
    BindDelivertSchItemList();
    var PageName = sessionStorage.getItem("MenuName"); 
    $('#LSODetailsPageName').text(PageName);
    $('#salesperson').select2();
    $('select[id="SOItemListName1"]').bind('change', function (e) {
         debugger;
        BindSOItemList(e);
    });
    ReplicateWith();
    BindSOCustomerList();
    BindDLLSOItemList(); 
    BindCountryList();
   
    $('#SOItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        // debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#SOItemListName" + SNo).val();
        var doc_no = $(this).closest('tr').find("#QuotationNumber").val();
        ShowDoc_number(doc_no, SNo);
        var TOCAmount = parseFloat($("#SO_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        var src_typ = $("#src_type").val();

        if (src_typ == "Q") {/*add by Hina on 12-06-2025 for single quotation*/
            //var SONum = $("#so_no").val();
            var Cust_id = $("#CustomerName option:selected").val();
           debugger;
                
                var lencount = $("#SOItmDetailsTbl >tbody >tr").length;
                if (lencount == 0) {
                    BindQuotationNumberList(Cust_id);
                    $('#AddQt').css("display", "");
                    $('#qt_number').attr("disabled", false);
                    $("#Hdn_OC_TaxCalculatorTbl > tbody>tr").remove();
                    $("#Hdn_OCTemp_TaxCalculatorTbl > tbody >tr").remove();
                    $("#ht_Tbl_OC_Deatils > tbody>tr").remove();
                    $("#Tbl_OC_Deatils > tbody >tr").remove();
                    $("#Tbl_OtherChargeList > tbody >tr").remove();
                }
           
        }
        Calculate_OC_AmountItemWise(TOCAmount);
        SerialNoAfterDelete();
        AfterDeleteResetSO_ItemTaxDetail();
        CalculateAmount();
        DelDeliSchAfterDelItem(ItemCode);
        BindOtherChargeDeatils();
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        if ($('#SOItmDetailsTbl >tbody >tr').length == "0") {
            $("#ddlReplicateWith").attr("disabled", false);
            $("#Tbl_OC_Deatils tbody tr").remove();
            $('#ht_Tbl_OC_Deatils tbody tr').remove();
            $('#TblTerms_Condition tbody tr').remove();/*add by Hina sharma on 05-06-2025*/
        }
        var DiscPer = $("#OrderDiscountInPercentage").val();
        if (parseFloat(CheckNullNumber(DiscPer)) > 0) {
            OnchangeDiscInPerc();
        }
        var DiscAmt = $("#OrderDiscountInAmount").val();
        if (parseFloat(CheckNullNumber(DiscAmt)) > 0) {
            OnchangeDiscInAmt();
        }
    }); 
    DeleteTermCondition();
    Cmn_DeleteDeliverySch();
    $("#ItemDetailsTbl tbody").bind("click", function (e) {
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
    var src_typ = $("#src_type").val();
    
    if (src_typ == "Q") {
        var SONum = $("#so_no").val();
        var Cust_id = $("#CustomerName option:selected").val();
        if (SONum == null || SONum == "") {/*add by Hina on 12-06-2025 for single quotation*/
            debugger;
            BindQuotationNumberList(Cust_id);
        }
        
        
    }
    
    $("#hdDoc_No").val($("#so_no").val());
    $("#ddlReplicateWith").attr("disabled", true);
    if ($("#Disable").val() == "Y") {
        var Perc = parseFloat($("#OrderDiscountInPercentage").val());
        if (Perc != null && Perc != "" && Perc != "0" && Perc != "0.00") {
           // DisableDiscountAmt();
            $("#OrderDiscountInAmount").attr("disabled", true);
        }
        var Amt = parseFloat($("#OrderDiscountInAmount").val());
        if (Amt != null && Amt != "" && Amt != "0" && Amt != "0.00") {
            //DisableDiscountAmt();
            $("#OrderDiscountInPercentage").attr("disabled", true);
        }
    }
    else {
        var Perc = parseFloat($("#OrderDiscountInPercentage").val());
        if (Perc != null && Perc != "" && Perc != "0" && Perc != "0.00") {
            //DisableDiscountAmt();
            $("#OrderDiscountInAmount").attr("disabled", true);
            $("#OrderDiscountInPercentage").attr("disabled", false);
        }
        var Amt = parseFloat($("#OrderDiscountInAmount").val());
        if (Amt != null && Amt != "" && Amt != "0" && Amt != "0.00") {
            //DisableDiscountAmt();
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
    if (DocumentMenuId != "105103145110") {
        if ($("#rpt_id").text() != "0") {
            $("#salesperson").attr('disabled', true);
        }
    }
    $("#ddlRequiredArea").select2();
    var hdn_AutoPR = $("#hdn_AutoPR").val();
    if (hdn_AutoPR=="Y") {
       
        $("#Div_RequirementArea").css("display", "block");
    }
    else {
       
        $("#Div_RequirementArea").css("display", "none");
    }
});
function ReplicateWithOrderQuantity() {
    debugger;
    $('#ChkReplicateWithPen_Qty').prop("checked", false);
}
function ReplicateWithPendingQuantity() {
    debugger;
    $('#ChkReplicateWithOrd_Qty').prop("checked", false);
}
function OnChanngReplicateWith() {
    debugger;
    //let ddlReplicateWith = $("#ddlReplicateWith").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/ReplicateWithSelectPrintTypePopup",
        data: { /*ddlReplicateWith: ddlReplicateWith*/ },
        success: function (data) {
            debugger;
            $("#PopupSelectReplicateWithType").html(data);
            $("#HdnReplicateBtn").click();
            //SelectPrintType();
        }
    })
    return false;
}
function OnClickReplicateWith() {
    debugger;
    //let ddlReplicateWith = $("#ddlReplicateWith").val();
    ddlReplicateWith_onchange();
}
function ReplicateWith() {
    debugger;
    var item = $("#ddlReplicateWith").val();
    var Cust_type;
    if ($("#OrderTypeD").is(":checked")) {
        Cust_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Cust_type = "E";
    }
    $("#ddlReplicateWith").append("<option value='0'>---Select---</option>");
    $("#ddlReplicateWith").select2({
        ajax: {
            url: "/ApplicationLayer/LSODetail/BindReplicateWithlist",
            data: function (params) {
                var queryParameters = {
                    item: params.term,
                    SO_OrderType: Cust_type,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                    <div class="row">
                    <div class="col-md-4 col-xs-12 def-cursor">${$("#DocNo").text()}</div>
                    <div class="col-md-4 col-xs-12 def-cursor" id="soDocDate">${$("#DocDate").text()}</div>
                    <div class="col-md-4 col-xs-12 def-cursor">${$("#span_CustomerName").text()}</div>
                    </div>
                    </strong></li></ul>`)
                }
                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                debugger;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.ID.split(",")[0], document: val.ID.split(",")[1], UOM: val.Name, };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-4 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-4 col-xs-12' + classAttr + '">' + data.document + '</div>' +
                '<div class="col-md-4 col-xs-12' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
var QtyDecDigit = $("#QtyDigit").text();
var RateDecDigit = $("#RateDigit").text();
var DecDigit = $("#ValDigit").text();
function ddlReplicateWith_onchange() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var ExpImpQtyDigit = $("#QtyDigit").text();
    }
    $("#tbl_AlternateItem > tbody > tr").remove();
    $('#tbladd >tbody >tr').remove();
    var replicate = $("#ddlReplicateWith").val();
    var so_no = replicate.split(",")[0];
    var so_dt1 = replicate.split(",")[1]

    var date = so_dt1.split("-");
    var so_dt = date[2] + '-' + date[1] + '-' + date[0];
    var ReplicateType;
    if ($("#ChkReplicateWithOrd_Qty").is(":checked")) {
        ReplicateType="OrdQtyType"
    }
    if ($("#ChkReplicateWithPen_Qty").is(":checked")) {
        ReplicateType = "PenQtyType"
    }
    $('#SOItmDetailsTbl >tbody >tr').remove();
    $("#ddlReplicateWith").attr("disabled", true);
    $("#CustomerName").attr("disabled", true);
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/LSODetail/GetReplicateWithSoNumber",
            data: {
                so_no: so_no,
                so_dt: so_dt,
                ReplicateType: ReplicateType,
                cust_id: $("#CustomerName").val(),
                order_dt: $("#so_date").val(),
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "" && data != "ErrorPage") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                var item_id = arr.Table[i].item_id;
                                var item_name = arr.Table[i].item_name;
                                var uom_id = arr.Table[i].uom_id;
                                var uom_name = arr.Table[i].uom_alias;
                                var src_doc_number = arr.Table[i].src_doc_number;
                                var src_doc_date = arr.Table[i].src_doc_date;
                                var QtyDecDigit = ExpImpQtyDigit;
                                var ord_qty_spec = arr.Table[i].ord_qty_spec;
                                var ord_qty_base = arr.Table[i].ord_qty_base;
                                var mrp = arr.Table[i].mrp;
                                var mrp_disc = arr.Table[i].mrp_disc;
                                var item_rate = arr.Table[i].item_rate;
                                var item_disc_perc = arr.Table[i].item_disc_perc;
                                var item_disc_amt = arr.Table[i].item_disc_amt;
                                var item_disc_val = arr.Table[i].item_disc_val;
                                var item_gr_val = arr.Table[i].item_gr_val;
                                var item_ass_val = arr.Table[i].item_ass_val;
                                var hsn_code = arr.Table[i].hsn_code;
                                var tax_expted = arr.Table[i].tax_expted;
                                var manual_gst = arr.Table[i].manual_gst;
                                var force_close = arr.Table[i].force_close;
                                var it_remarks = arr.Table[i].it_remarks;
                                var tmplt_id = arr.Table[i].tmplt_id;
                                var sub_item = arr.Table[i].sub_item;
                                var foc_qty = arr.Table[i].foc_qty || "0";
                                onchangereplicate_item(item_id, QtyDecDigit, item_name, uom_id, uom_name, src_doc_number, src_doc_date, ord_qty_spec, ord_qty_base, mrp, mrp_disc, item_rate, item_disc_perc, item_disc_amt, item_disc_val, item_gr_val, item_ass_val, hsn_code, tax_expted, manual_gst, force_close, it_remarks, tmplt_id, sub_item, foc_qty);
                            }
                        }
                        $("#SOItmDetailsTbl tbody tr").each(function () {
                            var currentRow = $(this);
                            GetSOItemUOM(currentRow,"ReplicateOrder");                         
                        });
                    }
                    if (arr.Table1.length > 0) {
                        for (var y = 0; y < arr.Table1.length; y++) {

                            var ItmId = arr.Table1[y].item_id;
                            var SubItmId = arr.Table1[y].sub_item_id;
                            var QtQty = arr.Table1[y].Qty;
                            var FocQty = arr.Table1[y].foc_qty;
                            var SrcQtNo = arr.Table1[y].src_doc_number;
                            var SrcQtDt = arr.Table1[y].src_doc_date;
                           
                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${QtQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${SrcQtNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${SrcQtDt}'></td>
                            <td><input type="text" id="subItemFocQty" value='${FocQty}'></td>
                            </tr>`);                           
                        }
                    }
                }
                else {
                    $("#txt_Quantity").val("");
                }
            }
        })
}
function onchangereplicate_item(item_id, QtyDecDigit, item_name, uom_id, uom_name, src_doc_number, src_doc_date, ord_qty_spec, ord_qty_base, mrp, mrp_disc, item_rate, item_disc_perc, item_disc_amt, item_disc_val, item_gr_val, item_ass_val, hsn_code, tax_expted, manual_gst, force_close, it_remarks, tmplt_id, sub_item, foc_qty) {
    debugger;
    var rowIdx = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var ExpImpRateDigit = $("#ExpImpRateDigit").text();
        var ExpImpValDigit = $("#ExpImpValDigit").text();
    }
    else {
        var ExpImpRateDigit = $("#RateDigit").text();
        var ExpImpValDigit = $("#ValDigit").text();
    }
    var rowCount = $('#SOItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var levels = [];
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
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
    var ManualGst = "";
    var TaxExpted = "";
    var MRP = "";
    var FOC = "";
    var FOCQty = "";
    var MRPDisc = "";
    var price = "";
    var Disable=""
    var displayNone = ""
    var displayfalse = "none"
    var PPDisable = "";
    var PPolicy = $("#SpanCustPricePolicy").text();
    if (PPolicy == "M") {
        PPDisable = "none";
    }
    else {
        PPDisable = "block"
    }

    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y" && DocumentMenuId == "105103125") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
    }
    var SaleHistory = "";
    if ($("#SaleHistoryFeatureId").val() == "true") {
        SaleHistory = '<button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"><i class="fa fa-history" aria-hidden="true" title="${$("#Span_SaleHistory_Title").text()}"></i></button>'
    }
    if (DocumentMenuId == "105103125") {
        FOCQty = `<td>
                    <div class="col-sm-9 lpo_form no-padding">
                        <input id="foc_qty" onpaste="return CopyPasteData(event)" onchange="OnChangeFocQty(event)" class="form-control date num_right" value="${foc_qty}" autocomplete="off" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec" placeholder="0000.00"><span id="ord_qty_specError" class="error-message is-visible"></span>
                    </div>
                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemFocQty">
                        <button type="button" id="SubItemFocQty" ${Disable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('FocQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                    </div>
                </td>`
        FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        TaxExpted = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    if (DocumentMenuId == "105103125") {
        MRP = '<td><div class="lpo_form"><input id="MRP" value="${parseFloat(mrp).toFixed(QtyDecDigit)}" onpaste="return CopyPasteData(event)" class="form-control num_right" autocomplete="off" type="text" name="MRP" onchange="OnchangeMRP(event)" onkeypress="return RateFloatValueonly(this,event);" placeholder="0000.00"><span id="item_mrpError" class="error-message is-visible"></span></div></td >'
        price = `<td>
 <div class="col-md-10 no-padding">
<input id="item_rate" class="form-control date num_right" value="${parseFloat(item_rate).toFixed(ExpImpRateDigit)}" onchange="OnChangeSOItemRate(event)" autocomplete="off" disabled onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate" placeholder="0000.00">
 </div>
<div class="col-md-2 no-padding i_Icon" style="display:${PPDisable}">
 <button type="button" id="PriceListDetail" class="calculator available_credit_limit" onclick="GetPriceListDetails(event);" data-toggle="modal" data-target="#PriceListDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PriceListDetail").text()}"> </button>
</div>
</td>`

    }
    else {
        price = `<td><input id="item_rate" class="form-control date num_right" value="${parseFloat(item_rate).toFixed(ExpImpRateDigit)}" onchange="OnChangeSOItemRate(event)" autocomplete="off" disabled onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate" placeholder="0000.00"></td>`
    }
    if (DocumentMenuId == "105103125") {
        MRPDisc = '<td><div class="lpo_form"><input id="MRPDiscount" value="${parseFloat(mrp_disc).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="MRPDiscount" onchange="OnchangeMRPDisc(event)" onkeypress="return FloatValuePerOnly(this,event);" placeholder="0000.00"><span id="MRPDiscountError" class="error-message is-visible"></span></div></td >'
    }
    if (sub_item == "N") {
        Disable = "disabled style='filter: grayscale(100 %)'"
    }
    if (DocumentMenuId != "105103125") {
        displayNone = "none"
    }
    if (DocumentMenuId == "105103145110") {
        displayfalse = ""
    }
    $('#SOItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SRNO_" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input type="hidden" id="hfItemID" value="${item_id}" /></td>
<td style="display: none" id="QTNOrow"><input id="QuotationNumber" value="${src_doc_number}" class="form-control" autocomplete="off" type="text" name="QuotationNumber" placeholder="ABV2342DG" onblur="this.placeholder='ABV2342DG'" disabled></td>
<td style="display: none" id="QTDTrow"><input id="QuotationDate" value="${src_doc_date}" class="form-control" autocomplete="off" type="date" name="QuotationDate" placeholder="16-10-2021" onblur="this.placeholder='16-10-2021'" disabled></td>
<td class="itmStick">
<div class="col-md-10 col-sm-12 lpo_form no-padding"><select class="form-control" id="SOItemListName${RowNo}" name="SOItemListName${RowNo}" onchange="OnChangeSOItemName(${RowNo},event)"><option value="${item_id}">${item_name}</option></select><span id="SOItemListNameError" class="error-message is-visible"></span></div >
<div class="col-md-2 col-sm-12 no-padding">
<div class="col-md-4 col-sm-12 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator available_credit_limit" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_CustomerInformation_Title").text()}"> </button></div>
<div class="col-md-4 col-sm-12 no-padding"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" data-toggle="" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div>
<div class="col-md-4 col-sm-12 no-padding">
    `+ SaleHistory+`
</div>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" value="${uom_name}" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" value="${uom_id}" type="hidden" />
<input id="ItemHsnCode" value="${hsn_code}" type="hidden" />
<input id="ItemtaxTmplt" value="${tmplt_id}" type="hidden" />
<input id="ItemType" value="" type="hidden" />
<input id="Price_list_no" value="" type="hidden" />
</td>
<td>
<div class="col-sm-8 lpo_form no-padding">
<input id="AvailableStockInBase" class="form-control date num_right" value="" autocomplete="off" type="text" name="AvailableStockInBase" placeholder="0000.00" disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
<button type="button" id="SubItemAvlQty" ${Disable} class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
<div class="col-sm-2 i_Icon"><button type="button" id="StkDtlBtnIcon" class="calculator available_credit_limit" onclick="OnClickStkDtlIconBtn(event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_StockDetail").text()}" > </button>
</div>
<input id="ReservedStockInBase" type="hidden" />
<input id="RejectedStockInBase" type="hidden" />
<input id="ReworkableStockInBase" type="hidden" />
<input id="TotalStock" type="hidden" />
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="ord_qty_spec" onpaste="return CopyPasteData(event)" class="form-control date num_right" value="${ord_qty_spec}" autocomplete="off" onchange="OnChangeSOItemQty(event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec" placeholder="0000.00"><span id="ord_qty_specError" class="error-message is-visible"></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemOrderQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemOrderQty" ${Disable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
`+ FOCQty + `
<td style="display:${displayNone}"><div class="lpo_form"><input id="MRP" value="${parseFloat(mrp).toFixed(ExpImpValDigit)}" onpaste="return CopyPasteData(event)" class="form-control num_right" autocomplete="off" type="text" name="MRP" onchange="OnchangeMRP(event)" onkeypress="return RateFloatValueonly(this,event);" placeholder="0000.00"><span id="item_mrpError" class="error-message is-visible"></span></div></td >
<td style="display:${displayNone}"><div class="lpo_form"><input id="MRPDiscount" value="${parseFloat(mrp_disc).toFixed(ExpImpValDigit)}" class="form-control num_right" autocomplete="off" type="text" name="MRPDiscount" onchange="OnchangeMRPDisc(event)" onkeypress="return FloatValuePerOnly(this,event);" placeholder="0000.00"><span id="MRPDiscountError" class="error-message is-visible"></span></div></td >
`+ price +`
<td>
<div class="lpo_form">
<input id="item_disc_perc" class="form-control date num_right" onpaste="return CopyPasteData(event)" value="${parseFloat(item_disc_perc).toFixed(ExpImpValDigit)}" onchange="OnChangeSOItemDiscountPerc(event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc" placeholder="0000.00">
<span id="item_disc_percError" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">                                                                                 
<input id="item_disc_amt" class="form-control date num_right" onpaste="return CopyPasteData(event)" value="${parseFloat(item_disc_amt).toFixed(ExpImpValDigit)}" onchange="OnChangeSOItemDiscountAmt(event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt" placeholder="0000.00">
<span id="item_disc_amtError" class="error-message is-visible"></span>
</div>
</td>
<td><input id="item_disc_val" class="form-control date num_right" autocomplete="off" value="${parseFloat(item_disc_val).toFixed(ExpImpValDigit)}" type="text" name="item_disc_val" placeholder="0000.00" disabled></td>
<td><input id="item_gr_val" class="form-control date num_right" autocomplete="off" value="${parseFloat(item_gr_val).toFixed(ExpImpValDigit)}" type="text" name="item_gr_val" placeholder="0000.00" disabled></td>

<td style="display:${displayfalse}"><div class="lpo_form"><input id="item_ass_val" onpaste="return CopyPasteData(event)" class="form-control date num_right" disabled value="${item_ass_val}" autocomplete="off" type="text" onkeypress="return Ass_valFloatValueonly(this,event);" onchange="OnChangeAssessAmt(event)" name="item_ass_val" placeholder="0000.00"><span id="item_ass_valError" class="error-message is-visible"></span></div></td>
`+ FOC +`
`+ TaxExpted + `
`+ ManualGst + `
<td>
<div class="col-sm-10 lpo_form no-padding"> <input id="item_tax_amt" class="form-control num_right" value="" autocomplete="off" type="text" name="item_tax_amt" placeholder="0000.00" disabled> </div>                                      
<div class="col-sm-2 lpo_form no-padding"><button type="button" class="calculator" id="BtnTxtCalculation" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#TaxInfo").text()}"></i></button></div>
</td>
<td><input id="item_oc_amt" class="form-control date num_right" autocomplete="off" value="" type="text" name="item_oc_amt" placeholder="0000.00" disabled></td>
<td style="display:${displayfalse}"><input id="item_net_val_spec" class="form-control date num_right" autocomplete="off" value="" type="text" name="item_net_val_spec" placeholder="0000.00" disabled></td>
<td><input id="item_net_val_bs" class="form-control date num_right" autocomplete="off" value="" type="text" name="item_net_val_bs" placeholder="0000.00" disabled></td>
<td>
<div class="custom-control custom-switch sample_issue">                                          
<input type="checkbox" Unchecked class="custom-control-input margin-switch" id="ItmFClosed" disabled>                                                 
<label class="custom-control-label" for="ItmFClosed" style="padding: 3px 0px;"> </label></div></td>
<td><textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="1500" onmouseover="OnMouseOver(this)" autocomplete="off" placeholder="${$("#span_remarks").text()}">${it_remarks}</textarea></td>
</tr>`);
    BindSOItmList(RowNo);
}
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    var rowCount = $('#SOItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var levels = [];
    //var TaxInfo = $("#TaxInfo").text();
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
        // debugger;
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


    var PPDisable = "";
    var PPolicy = $("#SpanCustPricePolicy").text();
    if (PPolicy == "M") {
        PPDisable = "none";
    }
    else {
        PPDisable = "block"
    }

    var FOCQty = "";
    var FOC = "";
    var ManualGst = "";
    var TaxExpted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y" && DocumentMenuId == "105103125") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
    }
    var SaleHistory = "";
    if ($("#SaleHistoryFeatureId").val() == "true") {
        SaleHistory = '<button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"><i class="fa fa-history" aria-hidden="true" title="${$("#Span_SaleHistory_Title").text()}"></i></button>'
    }
    if (DocumentMenuId == "105103125") {
        FOCQty = `<td>
                    <div class="col-sm-9 lpo_form no-padding">
                        <input id="foc_qty" onpaste="return CopyPasteData(event)" onchange="OnChangeFocQty(event)" class="form-control date num_right" value="" autocomplete="off" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec" placeholder="0000.00"><span id="ord_qty_specError" class="error-message is-visible"></span>
                    </div>
                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemFocQty">
                        <button type="button" id="SubItemFocQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('FocQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                    </div>
                </td>`
        FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        TaxExpted = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    var td = "";
    var displaynoneinDomestic = "";
    if (DocumentMenuId == "105103125") {
        td = `<td><div class="lpo_form"><input id="MRP" value="" onpaste="return CopyPasteData(event)" class="form-control num_right" autocomplete="off" type="text" name="MRP" onchange="OnchangeMRP(event)" onkeypress="return RateFloatValueonly(this,event);" placeholder="0000.00" > <span id="item_mrpError" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="MRPDiscount" onpaste="return CopyPasteData(event)" value="" class="form-control num_right" autocomplete="off" type="text" name="MRPDiscount" onchange="OnchangeMRPDisc(event)" onkeypress="return FloatValuePerOnly(this,event);" placeholder="0000.00" ><span id="MRPDiscountError" class="error-message is-visible"></span></div></td>
<td>
 <div class="col-md-10 no-padding">
<input id="item_rate" class="form-control date num_right" onchange ="OnChangeSOItemRate(event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00" disabled >
</div>
<div class="col-md-2 no-padding i_Icon" style="display:${PPDisable}">
                                                                                <button type="button" id="PriceListDetail" class="calculator available_credit_limit" onclick="GetPriceListDetails(event);" data-toggle="modal" data-target="#PriceListDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PriceListDetail").text()}"> </button>
                                                                            </div>
</td>`
        displaynoneinDomestic = "style='display: none;'";
    } else {
        td = `<td><input id="item_rate" class="form-control date num_right" onchange ="OnChangeSOItemRate(event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00" ></td>`
    }
    $('#SOItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SRNO_" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td> 
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td style="display: none;" id="QTNOrow"><input id="QuotationNumber" class="form-control" autocomplete="off" type="text" name="QuotationNumber" placeholder="${$("#span_QuotationNumber").text()}"  onblur="this.placeholder='${$("#span_QuotationNumber").text()}'" disabled></td>
<td style="display: none;" id="QTDTrow"><input id="QuotationDate" class="form-control" autocomplete="off" type="date" name="QuotationDate" placeholder="${$("#span_QuotationDate").text()}"  onblur="this.placeholder='${$("#span_QuotationDate").text()}'" disabled></td>

<td class="itmStick"><div class="col-md-10 col-sm-12 lpo_form no-padding"><select class="form-control" id="SOItemListName${RowNo}" name="SOItemListName${RowNo}" onchange ="OnChangeSOItemName(${RowNo},event)"></select><span id="SOItemListNameError" class="error-message is-visible"></span></div> <div class="col-md-2 col-sm-12 no-padding"><div class="col-md-4 col-sm-12 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div><div class="col-md-4 col-sm-12 no-padding"><button type="button" id="CustomerInformation" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div><div class="col-md-4 col-sm-12 no-padding">
`+ SaleHistory + `
</div> </div>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" />
<input id="ItemHsnCode"  type="hidden" />
<input id="ItemtaxTmplt"  type="hidden" />
<input id="ItemType" type="hidden" /></td>
<input id="Price_list_no" type="hidden" /></td>
<td>
<div class="col-sm-8 lpo_form no-padding">
<input id="AvailableStockInBase" class="form-control date num_right" autocomplete="off" type="text" name="AvailableStockInBase" placeholder="0000.00"   disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
<button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg"  data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
<div class="col-sm-2 i_Icon"><button type="button" id="StkDtlBtnIcon" class="calculator available_credit_limit" onclick="OnClickStkDtlIconBtn(event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_StockDetail").text()}" > </button>
</div>
<input id="ReservedStockInBase" type="hidden" />
<input id="RejectedStockInBase" type="hidden" />
<input id="ReworkableStockInBase" type="hidden" />
<input id="TotalStock" type="hidden" />
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="ord_qty_spec" onpaste="return CopyPasteData(event)" class="form-control date num_right" autocomplete="off" onchange ="OnChangeSOItemQty(event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00"  ><span id="ord_qty_specError" class="error-message is-visible"></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemOrderQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemOrderQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}""> </button>
</div>
</td>
`+ FOCQty+`
`+ td + `
<td><div class="lpo_form"><input id="item_disc_perc" onpaste="return CopyPasteData(event)" class="form-control num_right" onchange ="OnChangeSOItemDiscountPerc(event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc"  placeholder="0000.00"  ><span id="item_disc_percError" class="error-message is-visible"></span></div></td>
<td> <div class="lpo_form"><input id="item_disc_amt" onpaste="return CopyPasteData(event)" class="form-control date num_right" onchange ="OnChangeSOItemDiscountAmt(event)" autocomplete="off"  onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt"  placeholder="0000.00"  ><span id="item_disc_amtError" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_val" class="form-control date num_right" autocomplete="off" type="text" name="item_disc_val"  placeholder="0000.00"   disabled></td>
<td><input id="item_gr_val" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"   disabled></td>
<td ${displaynoneinDomestic}><div class="lpo_form"><input id="item_ass_val" disabled onpaste="return CopyPasteData(event)" class="form-control date num_right" autocomplete="off" type="text" onkeypress="return Ass_valFloatValueonly(this,event);"  onchange="OnChangeAssessAmt(event)" name="item_ass_val"  placeholder="0000.00"  ><span id="item_ass_valError" class="error-message is-visible"></span></div></td>
`+ FOC + `
`+ TaxExpted + `
`+ ManualGst + `
<td><input id="item_tax_amt" class="form-control col-md-10 col-sm-12 num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"   disabled><div class="col-sm-2 lpo_form no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation" disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" title="${$("#TaxInfo").text()}" aria-hidden="true"></i></button></div></td>
<td><input id="item_oc_amt" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"   disabled></td>
<td ${displaynoneinDomestic}><input id="item_net_val_spec" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec"  placeholder="0000.00"   disabled></td>
<td><input id="item_net_val_bs" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"   disabled></td>
<td><div class="custom-control custom-switch sample_issue" ><input type="checkbox" class="custom-control-input margin-switch" onchange="CheckedItemWiseFClose()" id="ItmFClosed" disabled><label class="custom-control-label" for="ItmFClosed" style="padding: 3px 0px;"> </label></div></td>
<td><textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "1500",onmouseover="OnMouseOver(this)" autocomplete="off"  placeholder="${$("#span_remarks").text()}"></textarea></td>
</tr>`);
    BindSOItmList(RowNo);
};
function OnchangeDiscInPerc() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var DecDigit = $("#ValDigit").text();
    if (DocumentMenuId == "105103145110") {
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
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ord_qty_spec = currentRow.find("#ord_qty_spec").val();
        var item_rate = CheckNullNumber(currentRow.find("#item_rate").val());
        var GrossVal = parseFloat(ord_qty_spec) * parseFloat(item_rate)
        var DiscVal = ((parseFloat(GrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
        if (parseFloat(Perc) > 0) {
            currentRow.find("#item_disc_perc").val(parseFloat(Perc).toFixed(2));
        } else {
            currentRow.find("#item_disc_perc").val("");
        }
        currentRow.find("#item_disc_val").val(parseFloat(DiscVal).toFixed(DecDigit));
        currentRow.find("#item_gr_val").val(parseFloat(GrossVal).toFixed(DecDigit));
        currentRow.find("#item_ass_val").val(parseFloat(GrossVal).toFixed(DecDigit));
        
        TOtalGrossVal = parseFloat(TOtalGrossVal) + parseFloat(GrossVal);
    });

    var PercDiscVal = ((parseFloat(TOtalGrossVal) * parseFloat(Perc)) / 100).toFixed(DecDigit);
    //var TxtGrossValue = TOtalGrossVal;
    //var TxtGrossValue1 = (parseFloat(TxtGrossValue) - parseFloat(PercDiscVal)).toFixed(DecDigit);
    if (PercDiscVal == 0) {
        $("#OrderDiscountInPercentage").val("");
    }
    else {
        $("#OrderDiscountInPercentage").val(Perc);
    }
    //$("#TxtGrossValue").val(parseFloat(TxtGrossValue1).toFixed(DecDigit));
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        CalculateOrderDiscount(currentRow);
    });
    CalculateAmount();
    //if (PercDiscVal != "" && PercDiscVal != null) {
    //    if (parseFloat(PercDiscVal) > parseFloat(0)) {
    //        Calculate_DiscAmountItemWise(PercDiscVal);
    //    }
    //    else {
    //        SetDiscInAmt();
    //        Calculate_DiscAmountItemWise(PercDiscVal);
    //    }
    //}
}
function OnchangeDiscInAmt() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var Amt = parseFloat(CheckNullNumber($("#OrderDiscountInAmount").val()));
    if (Amt != null && Amt != "" && Amt != "0") {
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
    var TOtalGrossVal = parseFloat(0).toFixed(DecDigit);
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var ord_qty_spec = CheckNullNumber(currentRow.find("#ord_qty_spec").val());
        var item_rate = CheckNullNumber(currentRow.find("#item_rate").val());
        var GrossVal = parseFloat(ord_qty_spec) * parseFloat(item_rate);
        currentRow.find("#item_gr_val").val(parseFloat(GrossVal).toFixed(DecDigit));
        currentRow.find("#item_ass_val").val(parseFloat(GrossVal).toFixed(DecDigit));
        currentRow.find("#item_disc_val").val("");
        TOtalGrossVal = parseFloat(TOtalGrossVal) + parseFloat(GrossVal);
    });
    var TxtGrossValue = TOtalGrossVal;
    var TxtGrossValue1 = (parseFloat(TxtGrossValue) - parseFloat(Amt)).toFixed(DecDigit);
    if (Amt == 0) {
        $("#OrderDiscountInAmount").val("");
    }
    else {
        $("#OrderDiscountInAmount").val(parseFloat(Amt).toFixed(DecDigit));
    }
    $("#TxtGrossValue").val(parseFloat(TxtGrossValue1).toFixed(DecDigit));
    if (Amt != "" && Amt != null) {
        if (parseFloat(Amt) > parseFloat(0)) {
            Calculate_DiscAmountItemWise(Amt);
        }
        else {
            SetDiscInAmt();
            Calculate_DiscAmountItemWise(Amt);
        }
    }
}
function Calculate_DiscAmountItemWise(Amt) {
    //var DecDigit = $("#ValDigit").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var TotalGAmt = parseFloat(0).toFixed(DecDigit);
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
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
    $("#SOItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var GrossValue;
        var ItmGrVal = currentRow.find("#item_gr_val").val();
        if (CheckNullNumber(ItmGrVal) == 0) {
            GrossValue = parseFloat(0).toFixed();
        }
        else {
            GrossValue = currentRow.find("#item_gr_val").val();
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
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
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
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var DiscValue = currentRow.find("#item_disc_val").val();
            var GrossValue = currentRow.find("#item_gr_val").val();
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
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var DiscValue = currentRow.find("#item_disc_val").val();
            var GrossValue = currentRow.find("#item_gr_val").val();
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
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        /*------Code Added by Suraj Maurya on 15-10-2024 to add discount value per piece-----*/
        var DiscValue = currentRow.find("#item_disc_val").val();
        var Qty = currentRow.find("#ord_qty_spec").val();
        var Rate = currentRow.find("#item_rate").val();
        var DiscAmtPerPec = parseFloat(CheckNullNumber(DiscValue)) / parseFloat(CheckNullNumber(Qty));
        if (parseFloat(CheckNullNumber(DiscAmtPerPec)) > 0) {
            currentRow.find("#item_disc_amt").val(parseFloat(CheckNullNumber(DiscAmtPerPec)).toFixed(DecDigit));
        } else {
            currentRow.find("#item_disc_amt").val("");
        }

        if (parseFloat(CheckNullNumber(Rate)) > parseFloat(parseFloat(CheckNullNumber(DiscAmtPerPec)).toFixed(DecDigit))) {
            currentRow.find("#item_disc_amt").css("border-color", '#ced4da');
            currentRow.find("#item_disc_amtError").text("");
            currentRow.find("#item_disc_amtError").css("display","none");
        }
        /*------Code Added by Suraj Maurya on 15-10-2024 to add discount value per piece End-----*/
        CalculateOrderDiscount(currentRow);
    });
    CalculateAmount();
}
function CalculateOrderDiscount(clickedrow) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var conv_rate = $("#conv_rate").val();
    var item_gr_val = clickedrow.find("#item_gr_val").val();
    var item_disc_val = clickedrow.find("#item_disc_val").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName = clickedrow.find("#SOItemListName" + Sno).val();

    var FinDisVal = parseFloat(item_gr_val) - parseFloat(item_disc_val);

    var ord_qty_spec = clickedrow.find("#ord_qty_spec").val();
    var item_rate = clickedrow.find("#item_rate").val();

    var Price = parseFloat(ord_qty_spec) * parseFloat(item_rate);
    if (DocumentMenuId == "105103145110") {
        clickedrow.find("#item_gr_val").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
        clickedrow.find("#item_ass_val").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
        clickedrow.find("#item_net_val_spec").val(parseFloat(FinDisVal).toFixed(ExpImpValDigit));
        FinalFinVal = (FinDisVal * conv_rate).toFixed(ExpImpValDigit);
    }
    else {
        clickedrow.find("#item_gr_val").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
        clickedrow.find("#item_ass_val").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
        clickedrow.find("#item_net_val_spec").val(parseFloat(FinDisVal).toFixed(ValDecDigit));
        FinalFinVal = (FinDisVal * conv_rate).toFixed(ValDecDigit);
    }

    clickedrow.find("#item_net_val_bs").val(FinalFinVal);

    OnChangeGrossAmt();
    var OrderType = $('#src_type').val();
    if (OrderType == "D") {
        clickedrow.find("#QuotationNumber").val("Direct")
        QuotationDate = "Direct";
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());
}
function SetDiscInAmt() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#item_disc_val").val((parseFloat(0)).toFixed(DecDigit));
    });
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
function GetTrackingDetails() { 
    $("#tbl_trackingDetail").DataTable().destroy();
    $("#tbl_trackingDetail").DataTable();
}
function ForwardBtnClick() {
    debugger;
    /*Add by Hina on 16-06-2025 to add for auto generate So by Sales Quotation*/
    try {
        if (Cmn_CheckDeliverySchdlValidations("SOItmDetailsTbl", "hfItemID", "SOItemListName", "ord_qty_spec", "SNohiddenfiled") == false) {
            $("#Btn_Forward").attr("data-target", "");
            $("#Btn_Forward").attr('onclick', '');
            $("#OKBtn_FW").attr("data-dismiss", "modal");
            return false;
        }
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
            $("#OKBtn_FW").attr("data-dismiss", "modal");
            var Doc_id = $("#DocumentMenuId").val();
            //debugger;
            Cmn_GetForwarderList(Doc_id);
        }
        else {
            $("#Btn_Forward").attr("data-target", "");
            $("#Btn_Forward").attr('onclick', '');
            $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        }
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;

    /*commented by Hina sharma on 30-05-2025 as per discuss with vishal sir */

    ///*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
    //var compId = $("#CompID").text();
    //var brId = $("#BrId").text();
    //var SODate = $("#so_date").val();
    //$.ajax({
    //    type: "POST",
    //    /*   url: "/Common/Common/CheckFinancialYear",*/
    //    url: "/Common/Common/CheckFinancialYearAndPreviousYear",
    //    data: {
    //        compId: compId,
    //        brId: brId,
    //        DocDate: SODate
    //    },
    //    success: function (data) {
    //        /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
    //        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
    //        if (data == "TransAllow") {
    //            var OrderStatus = "";
    //            OrderStatus = $('#hfStatus').val();
    //            if (OrderStatus === "D" || OrderStatus === "F") {
    //                if ($("#hd_nextlevel").val() === "0") {
    //                    $("#Btn_Forward").attr("data-target", "");
    //                }
    //                else {
    //                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //                    $("#Btn_Approve").attr("data-target", "");
    //                }
    //                $("#OKBtn_FW").attr("data-dismiss", "modal");
    //                var Doc_id = $("#DocumentMenuId").val();
    //                //debugger;
    //                Cmn_GetForwarderList(Doc_id);
    //            }
    //            else {
    //                $("#Btn_Forward").attr("data-target", "");
    //                $("#Btn_Forward").attr('onclick', '');
    //                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //            }
    //        }
    //        else {/* to chk Financial year exist or not*/
    //            /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
    //            /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
    //            swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
    //            $("#Btn_Forward").attr("data-target", "");
    //            $("#Btn_Approve").attr("data-target", "");
    //            $("#Forward_Pop").attr("data-target", "");

    //        }
    //    }
    //});
    ///*End to chk Financial year exist or not*/
    //return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    /*Add by Hina on 16-06-2025 to add for auto generate So by Sales Quotation*/
    if (Cmn_CheckDeliverySchdlValidations("SOItmDetailsTbl", "hfItemID", "SOItemListName", "ord_qty_spec", "SNohiddenfiled") == false) {
        return false;
    }
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SONo = "";
    var SODate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_status1 = "";
    var CustType = "";
    var mailerror = "";
    Remarks = $("#fw_remarks").val();
    SONo = $("#so_no").val();
    SODate = $("#so_date").val();
    docid = $("#DocumentMenuId").val();
    WF_status1 = $("#WF_status1").val();
    CustType = $("#CustType").val();
    var TrancType = (SONo + ',' + SODate + ',' + docid + ',' + WF_status1 + ',' + CustType)
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "SalesOrder_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(SONo, SODate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/LSODetail/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && SONo != "" && SODate != "" && level != "" && docid!="") {
            //debugger;
            await Cmn_InsertDocument_ForwardedDetail1(SONo, SODate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LSODetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        location.href = "/ApplicationLayer/LSODetail/InsertLSOApproveDetails/?SONo=" + SONo + "&SODate=" + SODate + "&A_Status=Approve&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&docid=" + docid + "&WF_status1=" + WF_status1 + "&CustType=" + CustType;
        //InsertSOApproveDetails("Approve", $("#hd_currlevel").val(), Remarks);
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SONo != "" && SODate != "" && docid != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SONo, SODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LSODetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && SONo != "" && SODate != "" && docid != "") {
            await Cmn_InsertDocument_ForwardedDetail1(SONo, SODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/LSODetail/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    return false;
}
//async function GetPdfFilePathToSendonEmailAlert(soNo, soDate, fileName) {
//    debugger;
//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/LSODetail/SavePdfDocToSendOnEmailAlert",
//        data: { soNo: soNo, soDate: soDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#so_no").val();
    //debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null) {
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    }

    return false;
}
function OtherFunctions(StatusC, StatusName) {
    window.location.reload();
}
//------------------------------WorkFlow End -----------------------------------//

function AddNewRowForEditSOItem() {
    var rowIdx = 0;
    var origin = window.location.origin + "/Content/Images/Calculator.png";
    var rowCount = $('#SOItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    rowIdx = rowCount
    var DocumentMenuId = $("#DocumentMenuId").val();
    var levels = [];
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
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
    var FOC = "";
    var FOCQty = "";
    var ManualGst = "";
    var TaxExpted = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y" && DocumentMenuId == "105103125") {
        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
    }
    var srcType = $('#src_type').val();
    if (DocumentMenuId == "105103125") {
        FOCQty = `<td>
                    <div class="col-sm-9 lpo_form no-padding">
                        <input id="foc_qty" onpaste="return CopyPasteData(event)" ${srcType == "D" ? "" :"disabled"} onchange="OnChangeFocQty(event)" class="form-control date num_right" value="" autocomplete="off" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec" placeholder="0000.00"><span id="ord_qty_specError" class="error-message is-visible"></span>
                    </div>
                    <div class="col-sm-3 i_Icon no-padding" id="div_SubItemFocQty">
                        <button type="button" id="SubItemFocQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('FocQuantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                    </div>
                </td>`
        if (srcType == "D") {
            FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        }
        else {
            FOC = '<td class="qt_to" hidden><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickFOCCheckBox(event)" id="FOC"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        }
        TaxExpted = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
    }
    var SaleHistory = "";
    if ($("#SaleHistoryFeatureId").val() == "true") {
        SaleHistory ='<button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-history" aria-hidden="true" title="${$("#Span_SaleHistory_Title").text()}"></i></button>'
    }
    //<i class="fa fa-history" aria-hidden="true" title="${$(" #Span_SaleHistory_Title").text()}" ></i >
    var td = "";
    var price = "";
    /* var displaynoneinDomestic = "";*//*commented by hina on 12-06-2025 for single quotation*/
    var Cust_type;
    if ($("#OrderTypeD").is(":checked")) {
        Cust_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Cust_type = "E";
    }
    if (Cust_type == "D") {
        var displaynoneinDomestic = 'style="display: none;"';
    }
    if (Cust_type == "D") {
        var displaynoneinDomestic = "";
    }
    var PPDisable = "";
    var PPolicy = $("#SpanCustPricePolicy").text();
    if (PPolicy == "M") {
        PPDisable = "none";
    }
    else {
        PPDisable = "block"
    }
    if (DocumentMenuId == "105103125") {
        td = `<td><div class="lpo_form"><input id="MRP" value="" class="form-control num_right" autocomplete="off" type="text" name="MRP" onchange="" disabled onkeypress="" placeholder="0000.00" ><span id="item_mrpError" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="MRPDiscount" value="" class="form-control num_right" autocomplete="off" type="text" name="MRPDiscount" onchange="" disabled onkeypress="return FloatValuePerOnly(this,event);" placeholder="0000.00" ><span id="MRPDiscountError" class="error-message is-visible"></span></div></td>
`
        price =`<td>
<div class="col-md-10 no-padding">
<input id="item_rate" class="form-control num_right" num_right" onchange ="OnChangeSOItemRate(event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  disabled="" >
</div>
<div class="col-md-2  no-padding i_Icon" style="display:${PPDisable}">
                                                                                <button type="button" id="PriceListDetail" class="calculator available_credit_limit" onclick="GetPriceListDetails(event);" data-toggle="modal" data-target="#PriceListDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_PriceListDetail").text()}"> </button>
                                                                            </div>
</td>`
        displaynoneinDomestic = 'style="display: none;"';
    }
    else {
        price =`<td><input id="item_rate" class="form-control num_right" num_right" onchange ="OnChangeSOItemRate(event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  disabled="" > </td>`
    }

    $('#SOItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td id="SRNO_" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td style="display: none;" id="QTNOrow"><input id="QuotationNumber" class="form-control" autocomplete="off" type="text" name="QuotationNumber" placeholder="${$("#span_QuotationNumber").text()}"  onblur="this.placeholder='${$("#span_QuotationNumber").text()}'" disabled></td>
<td style="display: none;" id="QTDTrow"><input id="QuotationDate" class="form-control" autocomplete="off" type="date" name="QuotationDate" placeholder="${$("#span_QuotationDate").text()}"  onblur="this.placeholder='${$("#span_QuotationDate").text()}'" disabled></td>

<td class="itmStick"><div class="col-sm-10 lpo_form no-padding"><select class="form-control" id="SOItemListName${RowNo}" name="SOItemListName${RowNo}" ></select><span id="SOItemListNameError" class="error-message is-visible"></span></div><div class="col-md-2 col-sm-12 no-padding"><div class="col-md-4 col-sm-12 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div><div class="col-md-4 col-sm-12 no-padding"><button type="button" id="CustomerInformation" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div> <div class="col-md-4 col-sm-12 no-padding">
`+ SaleHistory + `
</div></div> </td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" />
<input id="ItemHsnCode"  type="hidden" />
<input id="ItemtaxTmplt"  type="hidden" />
<input id="ItemType" type="hidden" />
<input id="Price_list_no" type="hidden" />
</td>
<td>
<div class="col-sm-8 lpo_form no-padding">
<input id="AvailableStockInBase" class="form-control num_right" autocomplete="off" type="text" name="AvailableStockInBase"  placeholder="0000.00"   disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
<button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
<div class="col-sm-2 i_Icon"><button type="button" id="StkDtlBtnIcon" class="calculator available_credit_limit" onclick="OnClickStkDtlIconBtn(event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_StockDetail").text()}" > </button>
</div>
<input id="ReservedStockInBase" type="hidden" />
<input id="RejectedStockInBase" type="hidden" />
<input id="ReworkableStockInBase" type="hidden" />
<input id="TotalStock" type="hidden" />
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="ord_qty_spec" class="form-control date num_right" autocomplete="off" onchange ="OnChangeSOItemQty(event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00"  ><span id="ord_qty_specError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemOrderQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
`+ FOCQty + `
`+ td + `
`+ price +`
<td><div class="lpo_form"><input id="item_disc_perc" class="form-control num_right" onchange ="OnChangeSOItemDiscountPerc(event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc"  placeholder="0000.00"  ><span id="item_disc_percError" class="error-message is-visible"></span></div></td>
<td> <div class="lpo_form"><input id="item_disc_amt" class="form-control num_right" onchange ="OnChangeSOItemDiscountAmt(event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt"  placeholder="0000.00"  ><span id="item_disc_amtError" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_val" class="form-control num_right" autocomplete="off" type="text" name="item_disc_val"  placeholder="0000.00"   disabled></td>
<td><input id="item_gr_val" class="form-control num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"   disabled></td>
<td ${displaynoneinDomestic}><div class="lpo_form"><input id="item_ass_val" disabled class="form-control date num_right" autocomplete="off" type="text" onkeypress="return Ass_valFloatValueonly(this,event);"  onchange="OnChangeAssessAmt(event)" name="item_ass_val"  placeholder="0000.00"  ><span id="item_ass_valError" class="error-message is-visible"></span></div></td>
`+ FOC + `
`+ TaxExpted + `
`+ ManualGst + `
<td> <div class="col-sm-10 lpo_form no-padding"><input id="item_tax_amt" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"   disabled> </div> <div class="col-sm-2 lpo_form no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation" disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true"></i></button></div></td>
<td><input id="item_oc_amt" class="form-control num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"   disabled></td>
<td ${displaynoneinDomestic}><input id="item_net_val_spec" class="form-control num_right" autocomplete="off" type="text" name="item_net_val_spec"  placeholder="0000.00"   disabled></td>
<td><input id="item_net_val_bs" class="form-control num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"   disabled></td>
<td><div class="custom-control custom-switch sample_issue" ><input type="checkbox" class="custom-control-input margin-switch" onchange="CheckedItemWiseFClose()" id="ItmFClosed" disabled><label class="custom-control-label" for="ItmFClosed" style="padding: 3px 0px;"> </label></div></td>
<td><textarea id="remarks"  class="form-control remarksmessage" name="remarks" maxlength = "1500", onmouseover="OnMouseOver(this)" autocomplete="off" placeholder="${$("#span_remarks").text()}"></textarea></td>
</tr>`);
    SerialNoAfterDelete();

};

function SaveBtnClick() {
    // debugger;
    var abc = InsertSODetails();
    if (abc == false) {
        return false;
    }
    else {
        return true;
    }
}

function BindSOCustomerList() {
    // debugger;
    var Branch = sessionStorage.getItem("BranchID");
    var DocId = $("#DocumentMenuId").val();
    $("#CustomerName").select2({
        ajax: {
            url: $("#CustNameList").val(), 
            data: function (params) {
                var queryParameters = {
                    SO_CustName: params.term,
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
function OnClickPrintBtn() {
    //debugger;
    $("#hd_SO_no").val($("#so_no").val());
    $("#hd_SO_dt").val($("#so_date").val());
    return true;
}
//function BindSalesPersonList() {
//    // debugger;
//    //var salesperson = $("#Hdn_salesperson").val();
//    //$("#salesperson").append(salesperson);
//    var Branch = sessionStorage.getItem("BranchID");
//    $("#salesperson").select2({
//        ajax: {
//            url: $("#salespersonList").val(),
//            data: function (params) {
//                var queryParameters = {
//                    SO_SalePerson: params.term, 
//                    BrchID: Branch
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    LSO_ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function BindCountryList() {
    //debugger;   
    $("#cntry_destination").select2({
        ajax: {
            url: $("#cntry_destinationList").val(),
            data: function (params) {
                var queryParameters = {
                    SO_Country: params.term,                  
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
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
function LSOList() {
    try {
        location.href = "/ApplicationLayer/LSOList/LSOList";
    } catch (err) {
        console.log(PFName + " Error : " + err.message);
    }
}

//------------------Tax Amount Calculation------------------//

function OnClickTaxCalculationBtn(e) {
    debugger;
    var SOItemListName = "#SOItemListName";
    var SNohiddenfiled = "#SNohiddenfiled";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentrow = $(e.target).closest('tr');
        var Itm_ID = currentrow.find("#hfItemID").val();
        var item_gross_val = currentrow.find("#item_gr_val").val();
        var mr_no = currentrow.find("#QuotationNumber").val();
        var mr_date = currentrow.find("#QuotationDate").val();
        var RowSNo = currentrow.find("#SNohiddenfiled").val();
        $("#HdnTaxOn").val("Item");
        $("#TaxCalcItemCode").val(Itm_ID);
        $("#Tax_AssessableValue").val(item_gross_val);
        $("#TaxCalcGRNNo").val(mr_no);
        $("#TaxCalcGRNDate").val(mr_date);
        $("#HiddenRowSNo").val(RowSNo)

        if (currentrow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab");
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, SOItemListName)
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Y") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").prop("disabled", true);
        }
        else {
            if (currentrow.find("#ManualGST").is(":checked")) {
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
function OnClickSaveAndExit(MGST) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var SrcDocNo = $("#TaxCalcGRNNo").val();
    if (SrcDocNo == "") {
        if ($("#src_type").val() == "D") {
            SrcDocNo = "Direct";
        }
    }
    //if (SrcDocNo == "Direct") {
    // SrcDocNo = "";
    //}
    var SrcDocDate = $("#TaxCalcGRNDate").val();

    //if ($("#src_type").val() == "Q") {
    //    var TaxItmCode = $("#TaxCalcItemCode").val();
    //    var SrcDocNo = $("#TaxCalcGRNNo").val();
    //    var SrcDocDate = $("#TaxCalcGRNDate").val();
    //}
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    var conv_rate = $("#conv_rate").val();

    //debugger;
    let NewArr = [];
    var TaxOn = $("#HdnTaxOn").val();
    //var TaxOn = "";
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }


    var FTaxDetails = $("#" + HdnTaxCalculateTable + " > tbody > tr").length;
    if (FTaxDetails > 0) {

        $("#" + HdnTaxCalculateTable + " > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text(); // Commented by Suraj on 30-11-2022 15:25
            //var TaxRowID = currentRow.find("#RowSNo").text();
            //if ($("#src_type").val() == "Q") {
            //    var SrcDocNo = currentRow.find("#DocNo").text();
            //    var SrcDocDate = currentRow.find("#DocDate").text();
            //    var TaxItmCode = currentRow.find("#TaxItmCode").text();
            //}
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            if (SrcDocNo == "Direct") {
                var QTNo = "Direct";
            }
            else {
                if (currentRow.find("#DocNo").text() == "") {
                    var QTNo = "";
                    SrcDocNo = "";
                }
                else {
                    if (SrcDocNo == "") {
                        var QTNo = "";
                    }
                    else {
                        var QTNo = currentRow.find("#DocNo").text();
                    }
                }
            }
            if (SrcDocDate == "") {
                var QTDate = "";
            }
            else {
                if (currentRow.find("#DocDate").text() == "") {
                    SrcDocDate = "";
                    var QTDate = "";
                }
                else {
                    var QTDate = currentRow.find("#DocDate").text();
                }
            }

            if (TaxOn == "OC") { QTNo = ""; QTDate = ""; }
            if (TaxOn == "OC") {
                if ( TaxItemID == TaxItmCode ) {
                    currentRow.remove();
                }
                else {
                    NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/DocNo: QTNo, DocDate: QTDate, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
                }
            }
            else {
                if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/  TaxItemID == TaxItmCode && SrcDocNo == QTNo && SrcDocDate == QTDate) {
                    //if (TaxItemID == TaxItmCode) {
                    currentRow.remove();
                }
                else {

                    NewArr.push({ /*UserID: UserID, RowSNo: RowSNo,*/DocNo: QTNo, DocDate: QTDate, TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
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
            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                <tr>
                                    <td id="DocNo">${SrcDocNo}</td>
                                    <td id="DocDate">${SrcDocDate}</td>
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
            NewArr.push({/* UserID: UserID, RowSNo: RowSNo,*/DocNo: SrcDocNo, DocDate: SrcDocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
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
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                   <tr>
                                    <td id="DocNo">${SrcDocNo}</td>
                                    <td id="DocDate">${SrcDocDate}</td>
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
            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo, */DocNo: SrcDocNo, DocDate: SrcDocDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationList);
        }

    }

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
    } else {
        $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
             
            var currentRow = $(this);
            if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                }
                if (MGST == "MGST") {

                }
                else {
                    var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                    var TaxAmt1 = "";
                    var TaxAmt2 = parseFloat(0).toFixed(DecDigit)
                    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt1) {
                        TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                    }
                    if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt2) {
                        TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                    }
                }
                if (currentRow.find("#ItemHsnCode").val() == 0) {
                    currentRow.find("#item_tax_amt").val(parseFloat(0).toFixed(DecDigit));
                }
                else {
                    currentRow.find("#item_tax_amt").val(parseFloat(TaxAmt).toFixed(DecDigit));
                }
                currentRow.find("#item_tax_amt").val(TaxAmt);
                OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                    OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(DecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
            }
            var TaxAmt3 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt3 = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt3 != TaxAmt3) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        var src_type = $("#src_type").val();
        var CRow = null;
        if (src_type == "Q") {
            CRow = $("#SOItmDetailsTbl >tbody >tr #hfItemID[value=" + TaxItmCode + "]").closest("tr #QuotationNumber[value=" + SrcDocNo + "]").closest("tr");
        } else {
            CRow = $("#SOItmDetailsTbl >tbody >tr #hfItemID[value=" + TaxItmCode + "]").closest("tr");
        }
        var DocumentMenuId = $("#DocumentMenuId").val();
        if ($("#ApplyTax_I").is(":checked")) {
            if (DocumentMenuId != "105103145110") {
                ResetItemRate(CRow);
            }
        }
        CalculateAmount();
    }
    $('#BtnTxtCalculation').css('border', '"#ced4da"');
}
function ManualGstValue() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        if (currentRow.find("#ManualGST").is(":checked")) {
            currentRow.find("#item_tax_amt").val(parseFloat(0).toFixed(DecDigit));
        }
    });
}
function OnClickReplicateOnAllItems() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
    var TaxOn = $("#HdnTaxOn").val();
     //debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var QTNo = $("#QuotationNumber").val();
        var QTDate = $("#QuotationDate").val();
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ QTNo: QTNo, QTDate: QTDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });

    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var SnoForOc = 0;
            var TaxApplicableTable = "SOItmDetailsTbl";
            if (TaxOn == "OC") {
                TaxApplicableTable = "Tbl_OC_Deatils";
            }

            $("#" + TaxApplicableTable + " >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var QTNo = currentRow.find("#QuotationNumber").val();
                var QTDate = currentRow.find("#QuotationDate").val();
                var ItemCode;
                var AssessVal;
                //debugger;
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OcAmtBs").text();
                    SnoForOc++;
                    Sno = SnoForOc;
                } else {
                    ItemCode = currentRow.find("#SOItemListName" + Sno).val();
                    AssessVal = currentRow.find("#item_ass_val").val();
                }
                  
                if (AssessVal != "") {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        //var RowSNo = TaxCalculationList[i].RowSNo;
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
                                    var TaxICode = NewArray[j].TaxItmCode;
                                    var A_QTNo = NewArray[j].QTNo;
                                    if (TaxLevelTbl == Level && ItemCode == TaxICode && QTNo == A_QTNo) {
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
                                    var TaxICode = NewArray[j].TaxItmCode;
                                    var A_QTNo = NewArray[j].QTNo;
                                    if (Level < TaxLevel && TaxLevel != Level && ItemCode == TaxICode && QTNo == A_QTNo) {
                                        TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                    }
                                }
                                var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                            }
                        }
                        NewArray.push({ /*UserID: UserID, RowSNo: Sno,*/ TaxItmCode: ItemCode, QTNo: QTNo,  TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
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
                                if (CitmTaxItmCode != TaxItmCode /*&& CitmRowSNo != RowSNo*/) {
                                //if (CitmRowSNo != RowSNo) {
                               
                                    if (TaxOn == "OC") {
                                        QTNo = "";
                                        QTDate = "";
                                    }
                                    
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ QTNo: QTNo, QTDate: QTDate, TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }

                }
            });
        }
    }
    
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    $("#" + HdnTaxCalculateTable+" > tbody >tr").remove();
    if (TaxCalculationListFinalList.length > 0) {
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 change position DocNo and Date***/
            $("#" + HdnTaxCalculateTable +" > tbody").append(`
                                 <tr>
                                   <td id="DocNo">${TaxCalculationListFinalList[j].QTNo}</td>
                                    <td id="DocDate">${TaxCalculationListFinalList[j].QTDate}</td>
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
    } else {
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationListFinalList);
        }
    } 
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
    } else {
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var hfItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    var fill = false;
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
                       

                        if (hfItemID == TaxItmCode && fill == false) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = parseFloat(CheckNullNumber(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);//(parseFloat(0)).toFixed(DecDigit);
                             
                            //if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            //    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                            //}
                            //if (currentRow.find("#item_gr_val").val() != null && currentRow.find("#item_gr_val").val() != "") {
                                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(DecDigit);
                            //}
                            //else {
                            //    AssessableValue = (parseFloat(0)).toFixed(DecDigit);
                            //}
                            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                            currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                            fill = true;
                        }

                    }
                }
                else {

                    var GrossAmtOR = parseFloat(0).toFixed(DecDigit);
                    if (currentRow.find("#item_gr_val").val() != "") {
                        GrossAmtOR = parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val())).toFixed(DecDigit);
                    }
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        OC_Amt_OR = parseFloat(CheckNullNumber(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);
                    }
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                    //currentRow.find("#item_ass_val").val(GrossAmtOR);
                    currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                    FinalFGrossAmtOR = (FGrossAmtOR * conv_rate).toFixed(DecDigit);
                    currentRow.find("#item_net_val_bs").val(FinalFGrossAmtOR);
                }
            }
        });
        CalculateAmount();
        if ($("#ApplyTax_I").is(":checked")) {
            OnClickApplyTax();
        }
        
    }
   
    
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount,QTNo) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var UserID = $("#UserID").text();
    var conv_rate = $("#conv_rate").val();
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    //if (TaxOn == "OC") {
    //    HdnTaxCalculateTable = "Hdn_OC_TaxCalculatorTbl";
    //}
    var FTaxDetailsItemWise = $("#" + HdnTaxCalculateTable+" > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
    
    if (FTaxDetailsItemWise > 0) {
        var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
        var NewArray = [];

        $("#" + HdnTaxCalculateTable +" > tbody > tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var HQTNo = currentRow.find("#DocNo").text();
            if (HQTNo == "") {
                HQTNo = "Direct";
            }
            var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == ItmCode && QTNo == HQTNo) {

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
                            var TaxICode = NewArray[j].TaxItmCode;
                            var AQTNo = NewArray[j].QTNo;
                            if (TaxLevelTbl == Level && TaxItemID == TaxICode && HQTNo == AQTNo) {
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
                            var TaxICode = NewArray[j].TaxItmCode;
                            var AQTNo = NewArray[j].QTNo;
                            if (Level < TaxLevel && TaxLevel != Level && TaxItemID == TaxICode && HQTNo == AQTNo) {
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

            NewArray.push({ /*UserID: UserID, RowSNo: RowSNo, */TaxItmCode: TaxItemID, DocNo: HQTNo, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

        });
        $("#" + HdnTaxCalculateTable +" > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text();
            //var TaxRowID = currentRow.find("#RowSNo").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var HQTNo = currentRow.find("#DocNo").text();
            if (/*TaxUserID == UserID && TaxRowID == RowSNo &&*/ TaxItemID == ItmCode && QTNo == HQTNo) {
                currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
            }
        });
        $("#SOItmDetailsTbl >tbody >tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {//.find("#QuotationNumber[value=" + QTNo + "]").closest("tr")
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var ItemNo;
            ItemNo = currentRow.find("#SOItemListName" + Sno).val();
            var QuotationNumber = currentRow.find("#QuotationNumber").val();

            if (ItemNo == ItmCode && QuotationNumber == QTNo) {
                if (QuotationNumber != "Direct" && QuotationNumber != "" && QuotationNumber != null) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + QuotationNumber + "')").closest("tr");
                    if (FTaxDetailsItemWise.length > 0) {
                        var fill = false;
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = (parseFloat(0)).toFixed(DecDigit);
                            }
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode && fill == false) {
                                //if (Sno == RowSNoF) {

                                //TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                                currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                                currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                                fill == true;
                                //}
                            }
                        });

                    }
                }
                else {
                    var FTaxDetailsItemWise = $("#" + HdnTaxCalculateTable +" > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    //var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + QuotationNumber + "')").closest("tr");
                    if (FTaxDetailsItemWise.length > 0) {
                        var fill = false;
                        FTaxDetailsItemWise.each(function () {
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = (parseFloat(0)).toFixed(DecDigit);
                            }
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            var TaxAmt = parseFloat(0).toFixed(DecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            // var RowSNoF = CRow.find("#RowSNo").text();
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode && fill == false) {
                                //if (Sno == RowSNoF) {

                                //TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                                currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = (NetOrderValueBase * conv_rate).toFixed(DecDigit);
                                currentRow.find("#item_net_val_bs").val(FinalNetOrderValueBase);
                                fill == true;
                                //}
                            }
                        });

                    }
                }
            }
        });
        CalculateAmount();
        BindTaxAmountDeatils(NewArray);
    }
        
    
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    debugger;
    var SO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var SO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal"; 

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, SO_ItemTaxAmountList, SO_ItemTaxAmountTotal)

}
function AfterDeleteResetSO_ItemTaxDetail() {
    // debugger;
    var SOItmDetailsTbl = "#SOItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var SOItemListName = "#SOItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(SOItmDetailsTbl, SNohiddenfiled, SOItemListName)
   
}
function OnClickSaveAndExit_OC_Btn() {
     debugger;
    var NetOrderValueSpe = "#NetOrderValueSpe";
    var NetOrderValueInBase = "#NetOrderValueInBase";
    var So_otherChargeId = '#SO_OtherCharges';
    CMNOnClickSaveAndExit_OC_Btn(So_otherChargeId, NetOrderValueSpe, NetOrderValueInBase)
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    var TotalGAmt = parseFloat(0).toFixed(DecDigit);
    var conv_rate = $("#conv_rate").val();
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;
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

    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var GrossValue; 
        var ItmGrVal = currentRow.find("#item_gr_val").val();
        if (CheckNullNumber(ItmGrVal) == 0) {
                GrossValue = parseFloat(0).toFixed();
            }
            else {
                GrossValue = currentRow.find("#item_gr_val").val();
            }

        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            // debugger;
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
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;
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
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this); 
            /*var Sno = currentRow.find("#SNohiddenfiled").val();*/
            var Sno = currentRow.find("#SRNO_").text();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (Sno == "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
             debugger;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var Sno = currentRow.find("#SRNO_").text();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (Sno == "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var SOItm_GrossValue = CheckNullNumber(currentRow.find("#item_gr_val").val());
            var SOItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var SOItm_OCAmt = CheckNullNumber(currentRow.find("#item_oc_amt").val());
            if (SOItm_GrossValue == null || SOItm_GrossValue == "") {
                SOItm_GrossValue = "0";
            }
            if (SOItm_TaxAmt == null || SOItm_TaxAmt == "") {
                SOItm_TaxAmt = "0";
            }
            //if (SOItm_OCAmt == null || SOItm_OCAmt == "") {
            //    SOItm_OCAmt = "0";
            //}
            //if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
                SOItm_TaxAmt = parseFloat(CheckNullNumber(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);
            //}
            var SOItm_NetOrderValueSpec = (parseFloat(SOItm_GrossValue) + parseFloat(SOItm_TaxAmt) + parseFloat(SOItm_OCAmt));
            var SOItm_NetOrderValueBase = (parseFloat(SOItm_GrossValue) + parseFloat(SOItm_TaxAmt) + parseFloat(SOItm_OCAmt));
        currentRow.find("#item_net_val_spec").val((parseFloat(SOItm_NetOrderValueSpec)).toFixed(DecDigit));
        FinalSOItm_NetOrderValueBase = (SOItm_NetOrderValueBase * conv_rate).toFixed(DecDigit);
        currentRow.find("#item_net_val_bs").val((parseFloat(FinalSOItm_NetOrderValueBase)).toFixed(DecDigit));
    });
    CalculateAmount();
};
function BindOtherChargeDeatils() { 
     debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    if ($("#SOItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(parseFloat(0).toFixed(DecDigit));
        $("#_OtherChargeTotal").val(parseFloat(0).toFixed(DecDigit));
    }

    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#SO_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
      
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;  
            var td = "";
            if (DocumentMenuId == "105103125") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            }

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td>${currentRow.find("#OCName").text()}</td>
<td align="right">${currentRow.find("#OCAmount").text()}</td>
`+td+`
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
            if (DocumentMenuId == "105103125") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
            }
           
        });
        
    }
    $("#_OtherChargeTotal").text(TotalAMount);
    if (DocumentMenuId == "105103125") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
    
}
function SetOtherChargeVal() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
        var DecDigit = $("#ExpImpValDigit").text();
    }
    else {
        var DecDigit = $("#ValDigit").text();
    }
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
            currentRow.find("#item_oc_amt").val((parseFloat(0)).toFixed(DecDigit));
    });
}

//------------------End------------------//

//------------------Tax For OC Calculation------------------//
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//

//------------------Delivert Section------------------//
function BindDelivertSchItemList() {
    // debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var s = '<option value="0">---Select---</option>';
    $("#SOItmDetailsTbl >tbody >tr").each(function () {         
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var SOItemName = "";
        if (DocumentMenuId == "105103145110") {
            var SOItemQty = parseFloat(0).toFixed(ExpImpQtyDigit);
        }
        else {
            var SOItemQty = parseFloat(0).toFixed(QtyDecDigit);
        }
       
        var SOItemID = "";
            SOItemName = currentRow.find("#SOItemListName" + Sno + " option:selected").text();
        SOItemID = currentRow.find("#SOItemListName" + Sno).val();
        if (DocumentMenuId == "105103145110") {
            SOItemQty = parseFloat(CheckNullNumber(currentRow.find("#ord_qty_spec").val())).toFixed(ExpImpQtyDigit);
        }
        else {
            SOItemQty = parseFloat(CheckNullNumber(currentRow.find("#ord_qty_spec").val())).toFixed(QtyDecDigit);
        }
        //SOItemQty = parseFloat(SOItemQty) + parseFloat(CheckNullNumber(currentRow.find("#foc_qty").val()));//Added by Suraj Maurya on 13-01-2026

            if (SOItemID != "0" && SOItemID != "" && SOItemQty > 0) {
                var delschItmId
                $(s + "option").each(function () {

                    delschItmId = $(this).val();
                    if (delschItmId == SOItemID) {
                        return false;
                    }
                })
                if (delschItmId != SOItemID) {
                    //var ItemType = currentRow.find("#ItemType").val();
                    //if (ItemType != "Y") { /*Commented by Nitesh 13112025 1629*/
                        s += '<option value="' + SOItemID + '" text="' + SOItemName + '">' + SOItemName + '</option>';
                    //}
                }
        }
       
    });

    $("#DeliverySchItemDDL").html(s);
    $("#DeliverySchQty").val("");

    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var DSItemID = "";
        var SOItemQty = parseFloat(0).toFixed(ExpImpQtyDigit);
        var DSItemQty = parseFloat(0).toFixed(ExpImpQtyDigit);
        var SOItemID = "";
        SOItemID = currentRow.find("#SOItemListName" + Sno).val();
        SOItemQty = parseFloat(CheckNullNumber(currentRow.find("#ord_qty_spec").val()));//.toFixed(QtyDecDigit);

        //SOItemQty = (SOItemQty + parseFloat(CheckNullNumber(currentRow.find("#foc_qty").val()))).toFixed(QtyDecDigit);//Added by Suraj Maurya on 13-01-2026

        if ($("#DeliverySchTble >tbody >tr").length > 0) {
            $("#DeliverySchTble >tbody >tr").each(function () {
                
                var currentRowDel = $(this);
                var DSchItemID = currentRowDel.find("#Hd_ItemIdFrDS").text();
                var DSQty = currentRowDel.find("#delv_qty").text();

                if (SOItemID === DSchItemID) {
                    DSItemID = DSchItemID;
                    if (DocumentMenuId == "105103145110") {
                        DSItemQty = (parseFloat(DSItemQty) + parseFloat(DSQty)).toFixed(ExpImpQtyDigit);
                    }
                    else {
                        DSItemQty = (parseFloat(DSItemQty) + parseFloat(DSQty)).toFixed(QtyDecDigit);
                    }                    
                }
            });
        }
        if (DocumentMenuId == "105103145110") {
            if (DSItemID === SOItemID && parseFloat(DSItemQty).toFixed(ExpImpQtyDigit) === parseFloat(SOItemQty).toFixed(ExpImpQtyDigit)) {
                $("#DeliverySchItemDDL option[value=" + DSItemID + "]").hide();
                $('#DeliverySchItemDDL').val("0").prop('selected', true);
            }
        }
        else {
            if (DSItemID === SOItemID && parseFloat(DSItemQty).toFixed(QtyDecDigit) === parseFloat(SOItemQty).toFixed(QtyDecDigit)) {
                $("#DeliverySchItemDDL option[value=" + DSItemID + "]").hide();
                $('#DeliverySchItemDDL').val("0").prop('selected', true);
            }
        }
        
    });
};
function OnClickAddDeliveryDetail() {
    var rowIdx = 0;
     //debugger;
    Cmn_OnClickAddDeliveryDetail("SOItmDetailsTbl", "N", "SNohiddenfiled", "SOItemListName", "ord_qty_spec");
}

function DeleteDeliverySch() {
    Cmn_DeleteDeliverySch();
}

function OnChangeDelSchItm() {
    // debugger;
    Cmn_OnChangeDelSchItm("SOItmDetailsTbl", "N", "SNohiddenfiled", "SOItemListName", "ord_qty_spec");
}
function OnChangeDeliveryDate(DeliveryDate) {
    //debugger;
    Cmn_OnChangeDeliveryDate(DeliveryDate);
}
function OnChangeDeliveryQty() {
    // debugger;
    Cmn_OnChangeDeliveryQty("SOItmDetailsTbl", "N", "SNohiddenfiled", "SOItemListName", "ord_qty_spec");  
}
function OnClickReplicateDeliveryShdul() {
    Cmn_OnClickReplicateDeliveryShdul("SOItmDetailsTbl", "N", "SNohiddenfiled", "SOItemListName", "ord_qty_spec");
}
function DelDeliSchAfterDelItem(ItemID) {
    // debugger;
    Cmn_DelDeliSchAfterDelItem(ItemID, "SOItmDetailsTbl", "N", "SNohiddenfiled", "SOItemListName", "ord_qty_spec");
}
//------------------End------------------//



//------------------SO Items Section------------------//


function BindDLLSOItemList() { 
    // debugger;
    //BindSOItmList(1);
    var ItmDDLName = "#SOItemListName";
    var TableID = "#SOItmDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103125") {
        BindItemList(ItmDDLName, "1", TableID, SnoHiddenField, "", "SO");
    }
    else {
        BindItemList(ItmDDLName, "1", TableID, SnoHiddenField, "", "ESO");
    }    
}
function BindSOItmList(ID) {
     debugger;
    var ItmDDLName = "#SOItemListName";
    var TableID = "#SOItmDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    $(ItmDDLName + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    $('#Textddl' + ID).append(`<option data-uom="0" value="0">---Select---</option>`);
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103125") {
        DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "SO")
    }
    else {
        DynamicSerchableItemDDL(TableID, ItmDDLName, ID, SnoHiddenField, "", "ESO")
    }


    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/Common/Common/GetItemList",
    //        data: function (params) {
    //            var queryParameters = {
    //                SO_ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            // debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    $('#SOItemListName' + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl' + ID).append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#SOItemListName' + ID).select2({
    //                        templateResult: function (data) {
    //                            var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    // debugger;
    //                    HideSOItemListItm(ID);
    //                }
    //            }
    //        },
    //    });
}
function GetSOItemList() {
    // debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/LSODetail/GetSOItemList",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                // debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        sessionStorage.removeItem("itemList");
                        sessionStorage.setItem("itemList", JSON.stringify(arr.Table));
                    }
                }
            },
        });
}

function Ass_valFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
    clickedrow.find("#item_ass_valError").css("display", "none");
    clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    return true;
}
function DiscFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
    $("#SOItmDetailsTbl > tbody > tr").each(function () {
        var row = $(this);
        var rate = row.find("#item_rate").val();
        var Qty = row.find("#ord_qty_spec").val();
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
function RateFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        clickedrow.find("#item_mrpError").css("display", "none");
        clickedrow.find("#MRP").css("border-color", "#ced4da"); 
    return true;
}
function RateFloatValueonlyConvRate(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_disc_amtError").css("display", "none");
    clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
    clickedrow.find("#item_mrpError").css("display", "none");
    clickedrow.find("#MRP").css("border-color", "#ced4da");
    return true;
}
function AmountFloatQty(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
        clickedrow.find("#foc_qty").css("border-color", "#ced4da");
 
    return true;
}
function AmtFloatValueonly(el, evt) {
    if (DocumentMenuId == "105103145110") {
        if (Cmn_FloatValueonly(el, evt, "#ExpImpValDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
            return false;
        }
    }
    
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#item_rate").val();
    item_rate = CheckNullNumber(item_rate);
   
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
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
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            if (Cmn_CheckSelectedTextInTextField(evt) == true) {
                return true;
            };
            return false;
        }
        
    }
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
   
    return true;
}
function FloatValuePercDiscOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    return true;
}
function FloatValuePerOnly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
    //debugger;
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    $("#SpanTaxPercent").css("display", "none");
    //var valPer = parseFloat($("#item_disc_perc1").val());
    //if (valPer >= 100) {

    //    return false;
    //}
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //// debugger;
    //var key = evt.key;
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > 1) {
    //    return false;
    //}
    return true;
}
function BindSOItemList(e) {
     debugger; 
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    Cmn_DeleteSubItemQtyDetail(ItemID);
 
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    Itm_ID = clickedrow.find("#SOItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);
    clickedrow.find("#SOItemListNameError").css("display", "none");
    clickedrow.find("[aria-labelledby='select2-SOItemListName" + SNo + "-container']").css("border", "1px solid #ced4da");
    ClearRowDetails(e, ItemID);
    clickedrow.find("#item_disc_perc").attr("readonly", false);
    clickedrow.find("#item_disc_amt").attr("readonly", false);

    //HideSOItemListItm(SNo);
    //HideSelectedItem("#SOItemListName" + Sno, SNo, "#SOItmDetailsTbl", "#SNohiddenfiled");
    DisableHeaderField();
    GetSOItemUOM(clickedrow);
   
    BindDelivertSchItemList();
    getremarksonchangeitem(clickedrow, Itm_ID);
}

function getremarksonchangeitem(clickedrow, Itm_ID) {/*Added By Nitesh 15-12-2023 for onchange item name get item*/
   // try {
   //var Itm_ID = clickedrow.find("#SOItemListName" + SNo).val();
    var Cust_id = $("#CustomerName option:selected").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSODetail/getremarks",
            data: {
                Itm_ID: Itm_ID,
               // PPolicy: PPolicy,
                //PGroup: PGroup,
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
                    if (arr.Table.length > 0) {
                        var remark = arr.Table[0].remark
                        clickedrow.find("#remarks").val(remark);
                    } else {
                        clickedrow.find("#remarks").val("");
                    }
                }
            },
        });

   // }
    
            
            
}

function GetSOItemUOM(clickedrow, Replicate) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    var Itm_ID = clickedrow.find("#SOItemListName" + SNo).val();
    var Cust_id = $("#CustomerName option:selected").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDecDigit = $("#RateDigit").text();
    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();
    var PPolicy = $("#SpanCustPricePolicy").text();
    var PGroup = $("#SpanCustPriceGroup").text();
    var Docid = $("#DocumentMenuId").val();
    if (PPolicy == "M") {
        clickedrow.find("#PriceListDetailDiv").css("display", "none");
    }
    else {
        clickedrow.find("#PriceListDetailDiv").css("display", "block");
    }
    try {
        $.ajax({
            type: "POST",
            // url: "/ApplicationLayer/LSODetail/GetSOItemUOM",
            url: "/Common/Common/GetItemUOM",
            data: {
                Itm_ID: Itm_ID,
                ItemUomType: "sale"
            },
            success: function (data) {
                //debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    var DocumentMenuId = $("#DocumentMenuId").val();
                    if (arr.Table.length > 0) {
                        clickedrow.find("#UOM").val(arr.Table[0].uom_alias);
                        clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
                        if (DocumentMenuId == '105103125') {
                            if (arr.Table[0].HSN_code == 0) {
                                clickedrow.find("#Hsn").val("");
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (GstApplicable == "Y") {
                                    swal("", $("#HSNNotDefinedFor").text() + ' ' + arr.Table[0].item_name, "warning");
                                }
                            }
                            else {
                                clickedrow.find("#ItemHsnCode").val(arr.Table[0].HSN_code);
                            }
                            clickedrow.find("#ItemtaxTmplt").val(arr.Table[0].tmplt_id);

                            //clickedrow.find("#ItemType").val(arr.Table[0].SrvcType);
                            clickedrow.find("#ItemType").val(arr.Table[0].ItemType);
                        }
                        else {
                            clickedrow.find("#ItemHsnCode").val(arr.Table[0].HSN_code);
                            clickedrow.find("#ItemType").val(arr.Table[0].SrvcType);
                        }
                        $("#hd_tax_id").val(arr.Table[0].tmplt_id);
                        try {
                            HideShowPageWise(arr.Table[0].sub_item, clickedrow);
                        }
                        catch (ex) {
                            //console.log(ex);
                        }
                    }
                    else {
                        clickedrow.find("#UOM").val("");
                    }
                }
                    if (DocumentMenuId != "105103145110") {
                        var GstApplicable = $("#Hdn_GstApplicable").text();
                        if (GstApplicable != "Y") {

                            
                            if (arr.Table[0].tmplt_id != 0) {
                                $("#HdnTaxOn").val("Tax");
                                $("#Tax_Template").val(0);
                                OnChangeTaxTmpltDdl(arr.Table[0].tmplt_id);
                                $("#TaxCalcItemCode").val(Itm_ID);
                                $("#HiddenRowSNo").val(SNo);
                                $("#Tax_AssessableValue").val(0);
                            }
                           
                        } else {
                            $("#HdnTaxOn").val("Tax")
                            Cmn_ApplyGST(arr, Itm_ID, SNo);
                            OnClickSaveAndExit();
                        }
                }
                Cmn_AvailableStk(clickedrow, Itm_ID, SNo)
                if (PPolicy == 'P') {
                    try {
                        $.ajax({
                            type: "POST",
                            url: "/ApplicationLayer/LSODetail/GetPriceListRate",
                            data: {
                                Itm_ID: Itm_ID,
                                PPolicy: PPolicy,
                                PGroup: PGroup,
                                Cust_id: Cust_id,
                                OrdDate: $("#so_date").val(),
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
                                    if (Replicate == "ReplicateOrder") {
                                        if (arr.Table1.length > 0) {//Added by Suraj Maurya on 12-01-2026 
                                            clickedrow.find("#foc_qty").attr("disabled", true);
                                        } else {
                                            clickedrow.find("#foc_qty").attr("disabled", false);
                                        }
                                    } else {
                                        if (arr.Table1.length > 0) {//Added by Suraj Maurya on 12-01-2026 
                                            clickedrow.find("#foc_qty").val("").attr("disabled", true);
                                        } else {
                                            clickedrow.find("#foc_qty").val("").attr("disabled", false);
                                        }
                                    }
                                    
                                    var ItmRate = clickedrow.find("#item_rate").val();
                                    if (AvoidDot(ItmRate) == false) {
                                        if (arr.Table.length > 0) {
                                            var sale_price = arr.Table[0].sale_price;
                                            if (DocumentMenuId == "105103145110") {
                                                clickedrow.find("#MRP").val(parseFloat(CheckNullNumber(sale_price)).toFixed(ExpImpRateDigit));
                                            }
                                            else {
                                                clickedrow.find("#MRP").val(parseFloat(CheckNullNumber(sale_price)).toFixed(RateDecDigit));
                                            }
                                            if (DocumentMenuId == "105103125") {
                                                clickedrow.find("#Price_list_no").val(arr.Table[0].list_no);
                                            }
                                            clickedrow.find("#MRP").attr("disabled", true);
                                        }
                                        else {
                                            if (DocumentMenuId == "105103145110") {
                                                clickedrow.find("#MRP").val(parseFloat("0").toFixed(ExpImpQtyDigit));
                                            }
                                            else {
                                                clickedrow.find("#MRP").val(parseFloat("0").toFixed(QtyDecDigit));
                                            }

                                            clickedrow.find("#MRP").attr("disabled", true);
                                        }
                                        if (arr.Table.length > 0) {
                                            if (DocumentMenuId == "105103145110") {
                                                clickedrow.find("#MRPDiscount").val(parseFloat(CheckNullNumber(arr.Table[0].disc_mrp)).toFixed(ExpImpRateDigit));
                                            }
                                            else {
                                                clickedrow.find("#MRPDiscount").val(parseFloat(CheckNullNumber(arr.Table[0].disc_mrp)).toFixed(RateDecDigit));
                                            }
                                            clickedrow.find("#MRPDiscount").attr("disabled", true);
                                        }
                                        else {
                                            if (DocumentMenuId == "105103145110") {
                                                clickedrow.find("#MRPDiscount").val(parseFloat("0").toFixed(ExpImpQtyDigit));
                                            }
                                            else {
                                                clickedrow.find("#MRPDiscount").val(parseFloat("0").toFixed(QtyDecDigit));
                                            }

                                            clickedrow.find("#MRPDiscount").attr("disabled", true);
                                        }
                                        if (arr.Table.length > 0) {
                                            clickedrow.find("#item_disc_perc").val(parseFloat(CheckNullNumber(arr.Table[0].disc_perc)).toFixed(2));
                                            //clickedrow.find("#item_disc_perc").attr("disabled", true);
                                        }
                                        else {
                                            clickedrow.find("#item_disc_perc").val(parseFloat("0").toFixed(2));
                                            //clickedrow.find("#item_disc_perc").attr("disabled", true);
                                        }
                                        //if (arr.Table.length > 0) {
                                        //    clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].effect_price).toFixed(RateDecDigit));
                                        clickedrow.find("#item_rate").attr("disabled", true);
                                        //}
                                        //else {
                                        //    clickedrow.find("#item_rate").val(parseFloat("0").toFixed(QtyDecDigit));
                                        //    clickedrow.find("#item_rate").attr("disabled", true);
                                        //}
                                        //debugger;
                                        var itemrate = clickedrow.find("#item_rate").val();
                                        if (itemrate != "" && itemrate != "0.000") {
                                            clickedrow.find("#item_mrpError").css("display", "none");
                                            clickedrow.find("#MRP").css("border", "1px solid #ced4da");
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
                                    else {//Added by Suraj Maurya on 10-02-2026
                                        clickedrow.find("#MRP").attr("disabled", true);
                                        clickedrow.find("#MRPDiscount").attr("disabled", true);
                                    }
                                    //if (arr.Table.length > 0) {
                                    //        clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].effect_price).toFixed(RateDecDigit));
                                    //        clickedrow.find("#item_rate").attr("disabled", true);
                                    //}
                                    //else {
                                    //        clickedrow.find("#item_rate").val(parseFloat("0").toFixed(QtyDecDigit));
                                    //}
                                    //// debugger;
                                    var itemrate = clickedrow.find("#MRP").val();
                                    if (CheckNullNumber(itemrate) > 0) {
                                        clickedrow.find("#item_mrpError").css("display", "none");
                                        clickedrow.find("#MRP").css("border", "1px solid #ced4da");
                                    }
                                    // debugger;
                                    // BindDelivertSchItemList();
                                    //var remark = arr.Table[0].remark;
                                    //clickedrow.find("#remarks").val(remark)
                                    //Modifyed By Shubham Maurya on 14-02-204 for only domestic time run this code//
                                    if (DocumentMenuId != "105103145110") {
                                        ResetItemRate(clickedrow);
                                    }                                  
                                }
                            },
                        });
                    }
                    catch (err) {
                    }
                }
                if (Replicate == "ReplicateOrder") {
                    CalculationBaseQty(clickedrow);
                }
            },
           
        });
    } catch (err) {
    }

}
function OnClickApplyTax() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    $("#SOItmDetailsTbl > tbody > tr").each(function () {
        var clickedrow = $(this);
        if (DocumentMenuId != "105103145110") {
            ResetItemRate(clickedrow);
        }       
    });
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer)) > 0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt)) > 0) {
        OnchangeDiscInAmt();
    }
}

function ResetItemRate(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();
    if ($("#ApplyTax_E").is(":checked")) {
        var RateDigit = $("#RateDigit").text();
        
        //var clickedrow = $(e.target).closest("tr");
        var MRP = clickedrow.find("#MRP").val();
        var MRP_disc = clickedrow.find("#MRPDiscount").val();
        var OrdQty = clickedrow.find("#ord_qty_spec").val();
        MRP = CheckNullNumber(MRP);
        MRP_disc = CheckNullNumber(MRP_disc);
        OrdQty = CheckNullNumber(OrdQty);

        var Rate = parseFloat(MRP) - parseFloat(MRP) * parseFloat(MRP_disc) / 100;
        if (Rate != 0) {
            if (DocumentMenuId == "105103145110") {
                clickedrow.find("#MRP").val(parseFloat(MRP).toFixed(ExpImpRateDigit));
            }
            else {
                clickedrow.find("#MRP").val(parseFloat(MRP).toFixed(RateDigit));
            }
        }
        else {
            clickedrow.find("#MRP").val("");
        }
        if (MRP_disc != 0) {
            clickedrow.find("#MRPDiscount").val(parseFloat(MRP_disc).toFixed(2));
        }
        else {
            clickedrow.find("#MRPDiscount").val("");
        }
        if (OrdQty != "" && OrdQty != "0.000") {
            if (DocumentMenuId == "105103145110") {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(ExpImpRateDigit));//.change();
            }
            else {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(RateDigit));//.change();
            }
           
            //clickedrow.find("#item_disc_perc").val();//.change();
            CalculationBaseRate(clickedrow);

        } else {
            if (DocumentMenuId == "105103145110") {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(ExpImpRateDigit));
            }
            else {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(RateDigit));
            }
            
        }
    } else {
        var RateDigit = $("#RateDigit").text();
        //var clickedrow = $(e.target).closest("tr");
        var hfItemID = clickedrow.find("#hfItemID").val();
        var MRP = clickedrow.find("#MRP").val();
        var MRP_disc = clickedrow.find("#MRPDiscount").val();
        var OrdQty = clickedrow.find("#ord_qty_spec").val();
        var QuotationNumber = clickedrow.find("#QuotationNumber").val();
        MRP = CheckNullNumber(MRP);
        MRP_disc = CheckNullNumber(MRP_disc);
        OrdQty = CheckNullNumber(OrdQty);
        var Rate = parseFloat(MRP) - parseFloat(MRP) * parseFloat(MRP_disc) / 100;
        if (DocumentMenuId == "105103145110") {
            clickedrow.find("#MRP").val(parseFloat(MRP).toFixed(ExpImpRateDigit));
        }
        else {
            clickedrow.find("#MRP").val(parseFloat(MRP).toFixed(RateDigit));
        }        
        clickedrow.find("#MRPDiscount").val(parseFloat(MRP_disc).toFixed(2));
        //var totalTaxAmt = 0;
        var TotalTaxAmt = 0;
        var NewArray = [];
        $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + hfItemID + ")").parent().each(function () {//.find("#DocNo:contains(" + QuotationNumber+")").parent()
            var currentRow = $(this);

            var QTNo = currentRow.find("#DocNo").text();
            if (QTNo == null || QTNo == "") {
                QTNo = "Direct";
            }
            if (QuotationNumber == QTNo) {
                if (DocumentMenuId == "105103145110") {
                    var AssessVal = parseFloat(Rate).toFixed(ExpImpRateDigit);
                }
                else {
                    var AssessVal = parseFloat(Rate).toFixed(RateDigit);
                }
                

                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxPercentage = "";
                TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxAmount;
                var TaxPec;
                TaxPec = TaxPercentage.replace('%', '');

                if (TaxApplyOn == "Immediate Level") {
                    if (TaxLevel == "1") {
                        if (DocumentMenuId == "105103145110") {
                            TaxAmount = (parseFloat(AssessVal) - (parseFloat(AssessVal) / (100 + parseFloat(TaxPec))) * 100).toFixed(ExpImpRateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ExpImpRateDigit);
                        }
                        else {
                            TaxAmount = (parseFloat(AssessVal) - (parseFloat(AssessVal) / (100 + parseFloat(TaxPec))) * 100).toFixed(RateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(RateDigit);
                        }
                       
                    }
                    else {
                        if (DocumentMenuId == "105103145110") {
                            var TaxAMountColIL = parseFloat(0).toFixed(ExpImpRateDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(RateDigit);
                        }
                        
                        var TaxLevelTbl = parseInt(TaxLevel) - 1;

                        for (j = 0; j < NewArray.length; j++) {
                            var Level = NewArray[j].TaxLevel;
                            var TaxAmtLW = NewArray[j].TaxAmount;
                            if (TaxLevelTbl == Level) {
                                if (DocumentMenuId == "105103145110") {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ExpImpRateDigit);
                                }
                                else {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(RateDigit);
                                }
                                
                            }
                        }
                        if (DocumentMenuId == "105103145110") {
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ExpImpRateDigit);
                            TaxAmount = (parseFloat(FinalAssAmtIL) - (parseFloat(FinalAssAmtIL) / (100 + parseFloat(TaxPec))) * 100).toFixed(ExpImpRateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ExpImpRateDigit);
                        }
                        else {
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(RateDigit);
                            TaxAmount = (parseFloat(FinalAssAmtIL) - (parseFloat(FinalAssAmtIL) / (100 + parseFloat(TaxPec))) * 100).toFixed(RateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(RateDigit);
                        }
                       
                    }
                }
                if (TaxApplyOn == "Cummulative") {
                    var Level = TaxLevel;
                    if (TaxLevel == "1") {
                        if (DocumentMenuId == "105103145110") {
                            TaxAmount = (parseFloat(AssessVal) - (parseFloat(AssessVal) / (100 + parseFloat(TaxPec))) * 100).toFixed(ExpImpRateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ExpImpRateDigit);
                        }
                        else {
                            TaxAmount = (parseFloat(AssessVal) - (parseFloat(AssessVal) / (100 + parseFloat(TaxPec))) * 100).toFixed(RateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(RateDigit);
                        }

                    }
                    else {
                        if (DocumentMenuId == "105103145110") {
                            var TaxAMountCol = parseFloat(0).toFixed(ExpImpRateDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(RateDigit);
                        }
                        

                        for (j = 0; j < NewArray.length; j++) {
                            var Level = NewArray[j].TaxLevel;
                            var TaxAmtLW = NewArray[j].TaxAmount;
                            if (Level < TaxLevel && TaxLevel != Level) {
                                if (DocumentMenuId == "105103145110") {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ExpImpRateDigit);
                                }
                                else {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(RateDigit);
                                }
                                
                            }
                        }
                        if (DocumentMenuId == "105103145110") {
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(ExpImpRateDigit);
                            TaxAmount = (parseFloat(FinalAssAmt) - (parseFloat(FinalAssAmt) / (100 + parseFloat(TaxPec))) * 100).toFixed(ExpImpRateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ExpImpRateDigit);
                        }
                        else {
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(RateDigit);
                            TaxAmount = (parseFloat(FinalAssAmt) - (parseFloat(FinalAssAmt) / (100 + parseFloat(TaxPec))) * 100).toFixed(RateDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(RateDigit);
                        }
                       
                    }
                }
                NewArray.push({ TaxLevel: TaxLevel, TaxAmount: TaxAmount });
            }                          
        });    
        if (clickedrow.find("#TaxExempted").is(":checked") || (CheckNullNumber(clickedrow.find("#item_tax_amt").val()) <= 0 && clickedrow.find("#ManualGST").is(":checked") )) {
            if (DocumentMenuId == "105103145110") {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(ExpImpRateDigit));//.change();
            }
            else {
                clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(RateDigit));//.change();
            }            
            CalculationBaseRate(clickedrow);
        }
        else {
            Rate = Rate - TotalTaxAmt;
            if (OrdQty != "" && OrdQty != "0.000") {
                if (DocumentMenuId == "105103145110") {
                    clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(ExpImpRateDigit));//.change();
                }
                else {
                    clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(RateDigit));//.change();
                }                
                CalculationBaseRate(clickedrow);

            } else {
                if (DocumentMenuId == "105103145110") {
                    clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(ExpImpRateDigit));
                }
                else {
                    clickedrow.find("#item_rate").val(parseFloat(Rate).toFixed(RateDigit));
                }                
            }
        }    
    }
}
function DisableHeaderField() {
    // debugger;
    $("#src_type").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);
    $("#CustomerName").attr('disabled', true);
    $("#ddlCurrency").attr('disabled', true);
    //$("#conv_rate").prop("readonly", true);
    //$("#salesperson").attr('disabled', true);
    $("#ref_doc_no").prop("readonly", true);
    $("#exp_file_no").prop("readonly", true);
    $('#qt_number').attr("disabled", true);
    //$("#OrderTypeD").attr("disabled", true);
    $("#cntry_destination").attr("disabled", true);
    $("#PortOfDestination").attr("disabled", true);
    $("#trade_term").attr("disabled", true);
    $('#AddQt').css("display", "none");
}
function DisableItemDetail() {
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#SOItemListName" + Sno).attr("disabled", true);
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
        currentRow.find("#ord_qty_spec").attr("disabled", true);
        currentRow.find("#foc_qty").attr("disabled", true);//Added by Suraj Maurya on 12-01-2026
        currentRow.find("#item_rate").attr("disabled", true);
        currentRow.find("#item_disc_perc").attr("disabled", true);
        currentRow.find("#item_disc_amt").attr("disabled", true);
        currentRow.find("#item_ass_val").attr("disabled", true);
        currentRow.find("#remarks").attr("disabled", true);
        currentRow.find("#BtnTxtCalculation").prop("disabled", false);

    });
  
    $("#SOItmDetailsTbl .plus_icon1").css("display", "none");
}

function CheckSOHraderValidations() {
    // debugger;
    var ErrorFlag = "N";
    if ($("#CustomerName").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#CustomerName").css("border-color", "Red");
        $('[aria-labelledby="select2-CustomerName-container"]').css("border-color", "Red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("#CustomerName").css("border-color", "#ced4da");
    }
    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "" || $("#ddlCurrency").val() == "0") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanCustCurrErrorMsg').text($("#valueReq").text());
        $("#SpanCustCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
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
    if ($("#qt_number").val() === "0" || $("#qt_number").val() == "" || $("#qt_number").val() == "---Select---") {
        $('#SpanQTNoErrorMsg').text($("#valueReq").text());
        $("#SpanQTNoErrorMsg").css("display", "block");
        $("#qt_number").css("border-color", "Red");
        $('[aria-labelledby="select2-qt_number-container"]').css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanQTNoErrorMsg").css("display", "none");
        $("#qt_number").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-qt_number-container"]').css("border-color", "#ced4da");
    }
    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CalculateDisPercent(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();

    //var clickedrow = $(e.target).closest("tr");
    var conv_rate = $("#conv_rate").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisPer;
    ItemName = clickedrow.find("#SOItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
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
        var FAmt = (ItmRate * DisPer) / 100;
        var GAmt = OrderQty * (ItmRate - FAmt);
        var DisVal = OrderQty * FAmt;
        if (DocumentMenuId == "105103145110") {
            var FinGVal = parseFloat(GAmt).toFixed(ExpImpValDigit);
            var FinDisVal = parseFloat(DisVal).toFixed(ExpImpValDigit);
            clickedrow.find("#item_gr_val").val(FinGVal);
            clickedrow.find("#item_ass_val").val(FinGVal);
            clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalFinGVal = (FinGVal * conv_rate).toFixed(ExpImpValDigit);
        }
        else {
            var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
            var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinGVal);
            clickedrow.find("#item_ass_val").val(FinGVal);
            clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalFinGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
        }       
        clickedrow.find("#item_net_val_bs").val(FinalFinGVal);
        clickedrow.find("#item_disc_val").val(FinDisVal);
        CalculateAmount();

        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(2));
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            if (DocumentMenuId == "105103145110") {
                var FinVal = parseFloat(FAmt).toFixed(ExpImpValDigit);
                clickedrow.find("#item_gr_val").val(FinVal);
                clickedrow.find("#item_ass_val").val(FinVal);
                clickedrow.find("#item_net_val_spec").val(FinVal);
                FinalFinVal = (FinVal * conv_rate).toFixed(ExpImpValDigit);
                clickedrow.find("#item_net_val_bs").val(FinalFinVal);
                clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ExpImpValDigit));
            }
            else {
                var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
                clickedrow.find("#item_gr_val").val(FinVal);
                clickedrow.find("#item_ass_val").val(FinVal);
                clickedrow.find("#item_net_val_spec").val(FinVal);
                FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
                clickedrow.find("#item_net_val_bs").val(FinalFinVal);
                clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            }
           

            CalculateAmount();
        }
        clickedrow.find("#item_disc_amt").prop("readonly", false);

    }
    OnChangeGrossAmt();
    var OrderType = $('#src_type').val();
    if (OrderType == "D") {
        clickedrow.find("#QuotationNumber").val("Direct")
        QuotationDate = "Direct";
    }

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());

}
function CalculateDisAmt(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();

    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisAmt;
    ItemName = clickedrow.find("#SOItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    if (AvoidDot(DisAmt) == false) {
        clickedrow.find("#item_disc_amt").val("");
        DisAmt = 0;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        if (parseFloat(CheckNullNumber(ItmRate)) > parseFloat(CheckNullNumber(DisAmt))) {
            var FRate = (ItmRate - DisAmt);
            var GAmt = OrderQty * FRate;
            var DisVal = OrderQty * DisAmt;
            if (DocumentMenuId == "105103145110") {
                var FinGVal = parseFloat(GAmt).toFixed(ExpImpValDigit);
                var FinDisVal = parseFloat(DisVal).toFixed(ExpImpValDigit);
                clickedrow.find("#item_disc_val").val(FinDisVal);
                clickedrow.find("#item_gr_val").val(FinGVal);
                clickedrow.find("#item_ass_val").val(FinGVal);
                clickedrow.find("#item_net_val_spec").val(FinGVal);
                FinalFinGVal = (FinGVal * conv_rate).toFixed(ExpImpValDigit);
            }
            else {
                var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
                var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
                clickedrow.find("#item_disc_val").val(FinDisVal);
                clickedrow.find("#item_gr_val").val(FinGVal);
                clickedrow.find("#item_ass_val").val(FinGVal);
                clickedrow.find("#item_net_val_spec").val(FinGVal);
                FinalFinGVal = (FinGVal * conv_rate).toFixed(ValDecDigit);
            }
            clickedrow.find("#item_net_val_bs").val(FinalFinGVal);
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
        if (DocumentMenuId == "105103145110") {
            clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ExpImpValDigit));
        }
        else {
            clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ValDecDigit));
        }
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            if (DocumentMenuId == "105103145110") {
                var FinVal = parseFloat(FAmt).toFixed(ExpImpValDigit);
                clickedrow.find("#item_gr_val").val(FinVal);
                clickedrow.find("#item_ass_val").val(FinVal);
                clickedrow.find("#item_net_val_spec").val(FinVal);
                FinalFinVal = (FinVal * conv_rate).toFixed(ExpImpValDigit);
                clickedrow.find("#item_net_val_bs").val(FinalFinVal);
                clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ExpImpValDigit));
            }
            else {
                var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
                clickedrow.find("#item_gr_val").val(FinVal);
                clickedrow.find("#item_ass_val").val(FinVal);
                clickedrow.find("#item_net_val_spec").val(FinVal);
                FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
                clickedrow.find("#item_net_val_bs").val(FinalFinVal);
                clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            }
            CalculateAmount();
        }
        clickedrow.find("#item_disc_perc").prop("readonly", false);

    }
    OnChangeGrossAmt();
    var OrderType = $('#src_type').val();
    if (OrderType == "D") {
        clickedrow.find("#QuotationNumber").val("Direct")
        QuotationDate = "Direct";
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());

}
function CalculationBaseQty(clickedrow) {
    debugger;

    //GetSOItemUOM(clickedrow); shubham 28-12-2023
    var DocumentMenuId = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();
    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var errorFlag = "N";
    var OrderQty;
    var FocQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;
        OrderQty = clickedrow.find("#ord_qty_spec").val();
        FocQty = clickedrow.find("#foc_qty").val();
        ItemName = clickedrow.find("#SOItemListName" + Sno).val();
        ItmRate = clickedrow.find("#item_rate").val();
        DisAmt = clickedrow.find("#item_disc_amt").val();
        DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
            clickedrow.find("#SOItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SOItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid red");
            clickedrow.find("#ord_qty_spec").val("");
        errorFlag = "Y";
    }
    else {        
            clickedrow.find("#SOItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }   
    if (errorFlag == "Y") {
        return false;
    }
    //if (OrderQty != "" && OrderQty != ".") {
    //    OrderQty = parseFloat(OrderQty);
    //}
    //if (OrderQty == ".") {
    //    OrderQty = 0;
    //}
    OrderQty = CheckNullNumber(OrderQty);
    FocQty = CheckNullNumber(FocQty);

    //if (OrderQty == "" || OrderQty == 0 || ItemName == "0") {
    if ((parseFloat(OrderQty) + parseFloat(FocQty)) == 0) {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#foc_qty").css("border-color", "red");
        
        errorFlag = "Y";
    } else {
        clickedrow.find("#ord_qty_specError").text("");
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
        clickedrow.find("#foc_qty").css("border-color", "#ced4da");
    }

    if (parseFloat(OrderQty) == 0) {
        clickedrow.find("#item_mrpError").text("");
        clickedrow.find("#item_mrpError").css("display", "none");
        clickedrow.find("#MRP").css("border-color", "#ced4da");
    }
    
   
        var OrderQty = clickedrow.find("#ord_qty_spec").val();
        var ItmRate = clickedrow.find("#item_rate").val();
    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#ord_qty_spec").val("");
        OrderQty = 0;
        //return false;
    }
    //ResetItemRate(e);
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        if (DocumentMenuId == "105103145110") {
            var FinVal = parseFloat(FAmt).toFixed(ExpImpValDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ExpImpValDigit);
        }
        else {
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        }       
        clickedrow.find("#item_net_val_bs").val(FinalFinVal);
        CalculateAmount();
    }
    if (DocumentMenuId == "105103145110") {
        clickedrow.find("#ord_qty_spec").val(parseFloat(OrderQty).toFixed(ExpImpQtyDigit));
    }
    else {
        clickedrow.find("#ord_qty_spec").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
    }    
    debugger;
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00" && DisPer !== "0" && DisPer !== "0.000" && DisPer !== "0.0000") {
        CalculateDisPercent(clickedrow);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0.00" && DisAmt !== "0" && DisAmt !== "0.000" && DisAmt !== "0.0000") {
        CalculateDisAmt(clickedrow);
    }
    OnChangeGrossAmt();
    var OrderType = $('#src_type').val();
    if (OrderType == "D") {
        clickedrow.find("#QuotationNumber").val("Direct")
        QuotationDate = "Direct";
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());
    // debugger;
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
    if ($("#DeliverySchTble >tbody >tr #Hd_ItemIdFrDS:contains(" + ItemName + ")").length < 0) {
        $("#DeliverySchTble >tbody >tr #Hd_ItemIdFrDS:contains(" + ItemName + ")").parent().remove();
        Cmn_ResetSerialNoInDelShcdl();
    }
    if (DocumentMenuId != "105103145110") {
        ResetItemRate(clickedrow);
    }
    BindDelivertSchItemList();
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer))>0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt))>0) {
        OnchangeDiscInAmt();
    }
}
function CalculationBaseRate(clickedrow) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();

    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();
    var ValDecDigit = $("#ValDigit").text();
   // var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SNohiddenfiled").val();
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var conv_rate = $("#conv_rate").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer; 
        OrderQty = clickedrow.find("#ord_qty_spec").val();
        ItemName = clickedrow.find("#SOItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
        DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();
    DisPer = CheckNullNumber(DisPer);
    DisAmt = CheckNullNumber(DisAmt);
    if (ItemName == "0") {    
            clickedrow.find("#SOItemListNameError").text($("#valueReq").text());
            clickedrow.find("#SOItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno+ "-container']").css("border", "1px solid red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {     
            clickedrow.find("#SOItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid #aaa");

    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {       
            clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
            clickedrow.find("#ord_qty_specError").css("display", "block");
            clickedrow.find("#ord_qty_spec").css("border-color", "red");
            clickedrow.find("#ord_qty_spec").val("");
            clickedrow.find("#item_rate").val("");
        errorFlag = "Y";
    }
    else {        
            clickedrow.find("#ord_qty_specError").css("display", "none");
            clickedrow.find("#ord_qty_specError").css("border-color", "#ced4da");
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
    //if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {      
    //        clickedrow.find("#item_mrpError").text($("#valueReq").text());
    //        clickedrow.find("#item_mrpError").css("display", "block");
    //    clickedrow.find("#MRP").css("border-color", "red");
    //    clickedrow.find("#MRP").val("");
    //    errorFlag = "Y";
    //}
    //else {       
    //        clickedrow.find("#item_mrpError").css("display", "none");
    //    clickedrow.find("#MRP").css("border-color", "#ced4da");     
    //}
   
    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
        //return false;
    }
    //ResetItemRate(e);
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        if (DocumentMenuId == "105103145110") {
            var FinVal = parseFloat(FAmt).toFixed(ExpImpValDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ExpImpValDigit);
        }
        else {
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalFinVal = (FinVal * conv_rate).toFixed(ValDecDigit);
        } 
        clickedrow.find("#item_net_val_bs").val(FinalFinVal);       
        CalculateAmount();
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0") {
        CalculateDisPercent(clickedrow);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0") {
        CalculateDisAmt(clickedrow);
    }
    if (DocumentMenuId == "105103145110") {
        clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(ExpImpRateDigit));
    }
    else {
        clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    }    
    OnChangeGrossAmt();
    var OrderType = $('#src_type').val();
    if (OrderType == "D") {
        clickedrow.find("#QuotationNumber").val("Direct")
        QuotationDate = "Direct";
    }
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val(), clickedrow.find("#QuotationNumber").val());
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
function CalculateAmount() {
     //debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
    var intVal = 1;

    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        // debugger;      
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
            if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
                TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
            }
            else {
                TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(DecDigit);
        }
        //var OrderType = $('#src_type').val();
        //if (OrderType == "Q") {
        //    if (currentRow.find("#item_oc_amt").val() == "" || currentRow.find("#item_oc_amt").val() == "0" || currentRow.find("#item_oc_amt").val() == "NaN") {
        //        OCValue = (parseFloat(OCValue) + parseFloat(0)).toFixed(DecDigit);
        //    }
        //    else {
        //        OCValue = (parseFloat(OCValue) + parseFloat(currentRow.find("#item_oc_amt").val())).toFixed(DecDigit);
        //    }
        //}
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
    var OrderType = $('#src_type').val();
    //if (OrderType == "Q") {
    //     $("#SO_OtherCharges").val(OCValue);
    //}
    $("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
};
function OnChangeGrossAmt() {
     debugger;
    var TotalOCAmt = $('#_OtherChargeTotalAmt').text();

    if ($("#DocumentMenuId").val() == "105103125") {
        TotalOCAmt = $('#_OtherChargeTotalAmt').text();
    }
    else {
        TotalOCAmt = $('#_OtherChargeTotal').text();
    }
    var Total_PO_OCAmt = $('#SO_OtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
       /* if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {*/
            Calculate_OC_AmountItemWise(TotalOCAmt);
       /* }*/
    }
}
function OnChangeCustomer(CustID) {
    debugger;
    var Cust_type;
    if ($("#OrderTypeD").is(":checked")) {
        Cust_type = "D";
    }
    if ($("#OrderTypeE").is(":checked")) {
        Cust_type = "E";
    }
    var Cust_id = CustID.value;
    if (Cust_id == "0") {
        $("#ddlReplicateWith").attr("disabled", true);
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("#SpanCustNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "red");
        DisableItemDetail();

        $("#TxtBillingAddr").val("");
        $("#TxtShippingAddr").val("");
        $("#ddlCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
    }
    else {
        $("#ddlReplicateWith").attr("disabled", false);
        $("#OrderDiscountInPercentage").attr("disabled", false);
        $("#OrderDiscountInAmount").attr("disabled", false);
        var CustName = $("#CustomerName option:selected").text();
        $("#Hdn_SoCustName").val(CustName)
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
        var OrderType = $('#src_type').val();
        if (OrderType == "D") {
            $("#SOItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();              
                    currentRow.find("#SOItemListName" + Sno).attr("disabled", false);
                    currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
                    currentRow.find("#BtnTxtCalculation").attr("disabled", true);
                currentRow.find("#ord_qty_spec").attr("disabled", false);
                currentRow.find("#foc_qty").attr("disabled", false);//Added by Suraj Maurya on 12-01-2026
                if (Cust_type == "D") {
                    currentRow.find("#MRP").attr("disabled", false);
                    currentRow.find("#MRPDiscount").attr("disabled", false);
                } else {
                    currentRow.find("#item_rate").attr("disabled", false);
                }
                    currentRow.find("#item_disc_perc").attr("disabled", false);
                    currentRow.find("#item_disc_amt").attr("disabled", false);
                    //currentRow.find("#item_ass_val").attr("disabled", false);
                currentRow.find("#remarks").attr("disabled", false);
                currentRow.find("#TaxExempted").attr("disabled", false);
                var GstApplicable = $("#Hdn_GstApplicable").text();
                if (GstApplicable == "Y") {
                    currentRow.find("#ManualGST").attr("disabled", false);
                }
                    currentRow.find("#SOItemListNameError").css("display", "none");
                currentRow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border-color", "#ced4da");
                    currentRow.find("#ord_qty_specError").css("display", "none");
                    currentRow.find("#ord_qty_spec").css("border-color", "#ced4da");
                    currentRow.find("#item_mrpError").css("display", "none");
                currentRow.find("#MRP").css("border-color", "#ced4da");
            })
        }
        if (OrderType == "Q") {           
            $("#SOItmDetailsTbl .plus_icon1").css("display", "none");
        }
        if (OrderType == "D") {           
            $("#SOItmDetailsTbl .plus_icon1").css("display", "block");
        }
       
    }
    
        
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
                                $("#bill_add_id").val(arr.Table[0].bill_add_id);
                                $("#ship_add_id").val(arr.Table[0].ship_add_id);
                                $("#ship_add_gstNo").val(arr.Table[0].cust_gst_no);
                                $("#Ship_StateCode").val(arr.Table[0].bill_state_code);
                                $("#cust_alias").val(arr.Table[0].cust_alias);
                                //var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                                //$("#ddlCurrency").html(s);

                                
                                

                                $("#AvailableCreditLimit").val(parseFloat(arr.Table[0].cre_limit).toFixed($("#ValDigit").text()));
                                 
                                $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                                $("#OCconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));

                                if (arr.Table[0].basecurrflag == 'Y') {
                                    $("#ddlCurrency").val(arr.Table[0].curr_id)
                                    $("#ddlCurrency").attr("disabled", true);
                                    $("#conv_rate").prop("readonly", true);
                                }
                                else {
                                    $("#ddlCurrency").val(arr.Table[0].curr_id)
                                    //$("#ddlCurrency").attr("disabled", false);
                                    $("#conv_rate").prop("readonly", false);
                                }

                                //if (Cust_type == "D") {
                                //    $("#conv_rate").prop("readonly", true);
                                //}
                                //else {
                                //    $("#conv_rate").prop("readonly", false);
                                //}
                                $("#SpanCustPricePolicy").text(arr.Table[0].cust_pr_pol);
                                $("#SpanCustPriceGroup").text(arr.Table[0].cust_pr_grp);
                                if (arr.Table[0].cust_pr_pol == "M") {
                                    $("#SOItmDetailsTbl >tbody >tr").each(function () {
                                        var currentRow = $(this);
                                        currentRow.find("#PriceListDetailDiv").css("display", "none");
                                    });
                                }
                                else {
                                    $("#SOItmDetailsTbl >tbody >tr").each(function () {
                                        var currentRow = $(this);
                                        currentRow.find("#PriceListDetailDiv").css("display", "block");
                                    });
                                }
                            }
                            else {
                                $("#TxtBillingAddr").val("");
                                $("#TxtShippingAddr").val("");
                                $("#bill_add_id").val("");
                                $("#ship_add_id").val("");
                                $("#ship_add_gstNo").val("");
                                var s = '<option value="0">---Select---</option>';
                                $("#ddlCurrency").html(s);
                                $("#conv_rate").val("");
                                $("#SpanCustPricePolicy").val("");
                                $("#SpanCustPriceGroup").val("");
                            }
                            var conv_rate = $("#conv_rate").val();
                            if (parseFloat(CheckNullNumber(conv_rate)) <= 0) {
                                $('#SpanExRateErrorMsg').text($("#valueReq").text());
                                $("#SpanExRateErrorMsg").css("display", "block");
                                $("#conv_rate").css("border-color", "red");
                            } else {
                                $('#SpanExRateErrorMsg').text("");
                                $("#SpanExRateErrorMsg").css("display", "none");
                                $("#conv_rate").css("border-color", "#ced4da");
                            }

                            var ddlCurrency = $("#ddlCurrency").val();
                            if (ddlCurrency == "0" || ddlCurrency == "" || ddlCurrency == null) {
                                $('#SpanCustCurrErrorMsg').text($("#valueReq").text());
                                $("#SpanCustCurrErrorMsg").css("display", "block");
                                $("#ddlCurrency").css("border-color", "red");
                            } else {
                                $('#SpanCustCurrErrorMsg').text("");
                                $("#SpanCustCurrErrorMsg").css("display", "none");
                                $("#ddlCurrency").css("border-color", "#ced4da");
                            }
                        }
                    },
                });
        
       
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
    try {
        //debugger
        var src_typ = $("#src_type").val();
        if (src_typ == "Q") {
            BindQuotationNumberList(Cust_id);
        }
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}

function OnChangeCurrency() {
    try {
        var Currid = $("#ddlCurrency").val();
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LSODetail/GetConrateDetail",
                data: {
                    Curr_id: Currid
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

                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            $("#OCconv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));

                            if (arr.Table[0].bcurrflag == "Y") {
                                $("#conv_rate").prop("readonly", true);
                            } else {
                                $("#conv_rate").prop("readonly", false);
                            }
                            
                            
                        }
                        else {
                            $("#conv_rate").val("");
                           
                        }
                        var conv_rate = $("#conv_rate").val();
                        if (parseFloat(CheckNullNumber(conv_rate)) <= 0) {
                            $('#SpanExRateErrorMsg').text($("#valueReq").text());
                            $("#SpanExRateErrorMsg").css("display", "block");
                            $("#conv_rate").css("border-color", "red");
                        } else {
                            $('#SpanExRateErrorMsg').text("");
                            $("#SpanExRateErrorMsg").css("display", "none");
                            $("#conv_rate").css("border-color", "#ced4da");
                        }
                    }
                },
            });


    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function FOCDisabledAndEnable(currentrow, Flag) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ValDigit = $("#ValDigit").text();
    var QtyDigit = $("#QtyDigit").text();
    if (Flag == "Y") {
        //currentrow.find("#ord_qty_spec").val(parseFloat(0).toFixed(QtyDigit));
        //currentrow.find("#ord_qty_spec").attr("disabled", true);

        currentrow.find("#MRP").val(parseFloat(0).toFixed(QtyDigit))
        currentrow.find("#MRP").attr("disabled", true)

        currentrow.find("#MRPDiscount").val(parseFloat(0).toFixed(QtyDigit));
        currentrow.find("#MRPDiscount").attr("disabled", true);

        currentrow.find("#item_disc_perc").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_disc_perc").attr("disabled", true);

        currentrow.find("#item_disc_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_disc_amt").attr("disabled", true);

        currentrow.find("#item_rate").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find("#item_rate").attr("disabled", true);

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

        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#TaxExempted").prop("disabled", true);
        currentrow.find("#ManualGST").prop("disabled", true);

        var RowSNo = currentrow.find("#SNohiddenfiled").val();
        var ItmCode = currentrow.find("#SOItemListName" + RowSNo).val();
        //debugger;
        //debugger;
        //$("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + ItmCode + "]").closest('tr').each(function () {
        //    var Crow = $(this).closest("tr");
        //    Crow.find("#subItemQty").val("0");
        //});
    }
    else {
        //currentrow.find("#ord_qty_spec").attr("disabled", false);
        currentrow.find("#MRP").attr("disabled", false)
        currentrow.find("#MRPDiscount").attr("disabled", false);
        currentrow.find("#item_disc_perc").attr("disabled", false);
        currentrow.find("#item_disc_amt").attr("disabled", false);
        //currentrow.find("#item_rate").attr("disabled", false);
        //currentrow.find("#item_disc_val").attr("disabled", false);
        //currentrow.find("#item_gr_val").attr("disabled", false);
        //currentrow.find("#item_tax_amt").attr("disabled", false);
        //currentrow.find("#item_oc_amt").attr("disabled", false);
        //currentrow.find("#item_net_val_bs").attr("disabled", false)
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
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ValDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var Itm_ID = currentrow.find("#hfItemID").val();
    var item_gross_val = currentrow.find("#item_gr_val").val();
    var mr_no = currentrow.find("#QuotationNumber").val();
    var mr_date = currentrow.find("#QuotationDate").val();
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var ItmCode = currentrow.find("#SOItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        var OrderType = $('#src_type').val();
        if (OrderType == "D") {
            currentrow.find("#QuotationNumber").val("Direct")
            QuotationDate = "Direct";
        }
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount, currentrow.find("#QuotationNumber").val());
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        var clickedrow = $(e.target).closest("tr");
        CalculationBaseQty(clickedrow);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
        if (DocumentMenuId != "105103145110") {
            ResetItemRate(clickedrow)
        }
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        if (GstApplicable == "Y") {
            var gst_number = $("#ship_add_gstNo").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "SOItmDetailsTbl", "hfItemID", "QuotationNumber", "item_ass_val", "TaxCalcGRNNo", "TaxCalcItemCode", e)
        }
        else {
            $("#Tax_ItemID").val(Itm_ID);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer))) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt))>0) {
        OnchangeDiscInAmt();
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ValDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var Itm_ID = currentrow.find("#hfItemID").val();
    var item_gross_val = currentrow.find("#item_gr_val").val();
    var mr_no = currentrow.find("#QuotationNumber").val();
    var mr_date = currentrow.find("#QuotationDate").val();
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(Itm_ID);
    $("#Tax_AssessableValue").val(item_gross_val);
    $("#TaxCalcGRNNo").val(mr_no);
    $("#TaxCalcGRNDate").val(mr_date);
    $("#HiddenRowSNo").val(RowSNo)
    var ItmCode = currentrow.find("#SOItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        var OrderType = $('#src_type').val();
        if (OrderType == "D") {
            currentrow.find("#QuotationNumber").val("Direct")
            currentrow.find("#DocNo").text("Direct")
            QuotationDate = "Direct";
        }
        //else {/*add by Hina 11-06-2025 for sales quotation*/
        //    var qtno = currentrow.find("#QuotationNumber").val();
        //    var qtdate = currentrow.find("#QuotationDate").val();
        //    currentrow.find("#DocNo").text(qtno)
        //    currentrow.find("#DocDate").text(qtdate)
        //    QuotationDate = qtdate;
        //}
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount, currentrow.find("#QuotationNumber").val());
        var clickedrow = $(e.target).closest("tr");
        CalculationBaseQty(clickedrow);
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        currentrow.find('#TaxExempted').prop("checked", false);
        if (DocumentMenuId != "105103145110") {
            ResetItemRate(clickedrow);

        }
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        var OrderType = $('#src_type').val();
        if (OrderType == "D") {
            currentrow.find("#QuotationNumber").val("Direct")
            currentrow.find("#DocNo").text("Direct")
            QuotationDate = "Direct";
        }
        //else {/*add by Hina 11-06-2025 for sales quotation*/
        //    var qtno = currentrow.find("#QuotationNumber").val();
        //    var qtdate = currentrow.find("#QuotationDate").val();
        //    currentrow.find("#DocNo").text(qtno)
        //    currentrow.find("#DocDate").text(qtdate)
        //    QuotationDate = qtdate;
        //}
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "SOItmDetailsTbl", "hfItemID", "QuotationNumber", "item_ass_val", "TaxCalcGRNNo", "TaxCalcItemCode",e)
        var clickedrow = $(e.target).closest("tr");
        CalculationBaseQty(clickedrow);
        $("#taxTemplate").text("Template");
        if (DocumentMenuId != "105103145110") {
            ResetItemRate(clickedrow);
        }
    }
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer))>0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt))>0) {
        OnchangeDiscInAmt();
    }
}
function BindQuotationNumberList(Cust_id) {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetQuotationNumberList",
        data: { Cust_id: Cust_id },
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
                    $("#qt_number option").remove();
                    $("#qt_number optgroup").remove();
                    $('#qt_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#Textddl').append(`<option data-date="${arr.Table[i].qt_dt}" value="${arr.Table[i].qt_no}">${arr.Table[i].qt_no}</option>`);
                    }
                    $("#SpanQTNoErrorMsg").css("display", "none");
                    var firstEmptySelect = true;
                    $('#qt_number').select2({
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

                    $("#qt_date").val("");
                }
            }
        },
    });
}

function OnClickIconBtn(e) {
    // debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";   
        ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text();
    ItemInfoBtnClick(ItmCode);    
}
function OnClickHistoryIconBtn(e) {
    // debugger;

    $("#tbl_trackingDetail").DataTable().destroy();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var CustID = "";
    CustID = $("#CustomerName").val();
    ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM").val();
    SalesOrderHistoryBtnClick(ItmCode, CustID, ItmName, UOMName);
}
function OnclickHistorySearchBtn() {
    //debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var CustID = "";
    CustID = $("#CustomerName").val();
    SalesOrderHistoryBtnClick(ItmCode, CustID, ItmName, UOMName);
}
function OnClickStkDtlIconBtn(e) {
     debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM").val();
    Cmn_GetItemStockWhLotBatchSerialWise(ItmCode, ItmName, UOMName);
    /*
    //var OrdTyp = $("#OrderTypeD").val();
    //if (OrdTyp)
    var AvalStk = clickedrow.find("#AvailableStockInBase").val();
    var ResrvStk = clickedrow.find("#ReservedStockInBase").val();
    var RejectStk = clickedrow.find("#RejectedStockInBase").val();
    var RewrkStk = clickedrow.find("#ReworkableStockInBase").val();
    var TotalStk = clickedrow.find("#TotalStock").val();
    //var AvalStk = clickedrow.find("#AvailableStockInBase" + Sno).val();
    //var ResrvStk = clickedrow.find("#ReservedStockInBase" + Sno).val();
    //var RejectStk = clickedrow.find("#RejectedStockInBase" + Sno).val();
    //var RewrkStk = clickedrow.find("#ReworkableStockInBase" + Sno).val();
    //var TotalStk = clickedrow.find("#TotalStock" + Sno).val();
    
    $("#prt_Item_Name").val(ItmName);
    $("#prt_UOM").val(UOMName);
    $("#Prt_AvailableStock").val(AvalStk);
    $("#Prt_ReservedStock").val(ResrvStk);
    $("#Prt_RejectedStock").val(RejectStk);
    $("#Prt_ReworkableStock").val(RewrkStk);
    $("#Prt_TotalStock").val(TotalStk);
   */
}


function OnChangeSOItemQty(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculationBaseQty(clickedrow);
    ApplyFoc(clickedrow);
    
}
function OnChangeSOItemRate(e) {
    // debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculationBaseRate(clickedrow);
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer))>0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt))>0) {
        OnchangeDiscInAmt();
    }
}
function OnChangeSOItemDiscountPerc(e) {
     debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculateDisPercent(clickedrow);
    ApplyFoc(clickedrow);
}
function OnChangeSOItemDiscountAmt(e) {
    // debugger;
    var clickedrow = $(e.target).closest("tr");
    CalculateDisAmt(clickedrow);
}
function OnChangeAssessAmt(e) {
     debugger;
    var errorFlag = "N";
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145110") {
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
    ItmCode = clickedrow.find("#SOItemListName" + Sno).val(); 
    if (AvoidDot(AssessAmt) == false) {
        clickedrow.find("#item_ass_valError").text($("#valueReq").text());
        clickedrow.find("#item_ass_valError").css("display", "block");
        clickedrow.find("#item_ass_val").css("border-color", "red");
        errorFlag = "Y";
    }
    if (CheckItemRowValidation(e) == false) {
        errorFlag = "Y";
    }
    if (errorFlag == "Y") {
        clickedrow.find("#item_ass_val").val("");
        AssessAmt = 0;
        //return false;
    }
    if (parseFloat(AssessAmt) > 0) {
        $("#BtnTxtCalculation").prop("disabled", false);
    }
    else {
        $("#BtnTxtCalculation").prop("disabled", true);
    }
    if (parseFloat(AssessAmt).toFixed(DecDigit) > 0 && ItmCode != "" && Sno != null && Sno != "") {
        debugger;
        clickedrow.find("#item_ass_val").val(parseFloat(AssessAmt).toFixed(DecDigit));
        var OrderType = $('#src_type').val();
        if (OrderType == "D") {
            clickedrow.find("#QuotationNumber").val("Direct")
            QuotationDate = "Direct";
        }
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(DecDigit), clickedrow.find("#QuotationNumber").val());
    } 
}
function OnChangeSOItemName(RowID, e) {
    debugger;
    BindSOItemList(e);
}
function OnchangeMRP(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr"); 
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;

    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItemName = clickedrow.find("#SOItemListName" + Sno).val();
    ItmRate = clickedrow.find("#MRP").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#SOItemListNameError").text($("#valueReq").text());
        clickedrow.find("#SOItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#MRP").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#SOItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid #aaa");

    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#MRP").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_specError").css("border-color", "#ced4da");
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
        clickedrow.find("#item_mrpError").text($("#valueReq").text());
        clickedrow.find("#item_mrpError").css("display", "block");
        clickedrow.find("#MRP").css("border-color", "red");
        clickedrow.find("#MRP").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_mrpError").css("display", "none");
        clickedrow.find("#MRP").css("border-color", "#ced4da");
    }
    if (DocumentMenuId != "105103145110") {
        ResetItemRate(clickedrow);
    }
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer)) > 0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt)) > 0) {
        OnchangeDiscInAmt();
    }
}
function OnchangeMRPDisc(e) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    var MRPDiscount = clickedrow.find("#MRPDiscount").val();
    if (parseFloat(CheckNullNumber(MRPDiscount)) >= 100) {
        clickedrow.find("#MRPDiscountError").text($("#DiscountCanNotBeGreaterThan99").text());
        clickedrow.find("#MRPDiscountError").css("display", "block");
        clickedrow.find("#MRPDiscount").css("border-color", "red");
        return false;
    } else {
        clickedrow.find("#MRPDiscountError").text("");
        clickedrow.find("#MRPDiscountError").css("display", "none");
        clickedrow.find("#MRPDiscount").css("border-color", "#ced4da");
    }

    if (DocumentMenuId != "105103145110") {
        ResetItemRate(clickedrow);
    }
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer)) > 0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt)) > 0) {
        OnchangeDiscInAmt();
    }
}
function ClearRowDetails(e,ItemID) {
     //debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();  
        clickedrow.find("#UOM").val("");
        clickedrow.find("#ord_qty_spec").val("");
        //clickedrow.find("#ord_qty_base").val("");
    clickedrow.find("#MRP").val("");
    clickedrow.find("#MRPDiscount").val("");
        clickedrow.find("#item_rate").val("");
        clickedrow.find("#item_disc_perc").val("");
        clickedrow.find("#item_disc_amt").val("");
        clickedrow.find("#item_disc_val").val("");
        clickedrow.find("#item_gr_val").val("");
        clickedrow.find("#item_ass_val").val("");
        clickedrow.find("#item_tax_amt").val("");
        clickedrow.find("#item_oc_amt").val("");
        clickedrow.find("#item_net_val_spec").val("");
        clickedrow.find("#item_net_val_bs").val("");
        clickedrow.find("#remarks").val("");
    clickedrow.find("#ItmFClosed").attr("checked", false);
    clickedrow.find("#ItemHsnCode").val("");

    if (ItemID != "" && ItemID != null) {
      //  ShowItemListItm(ItemID);
    }
    CalculateAmount();
    var TOCAmount = parseFloat($("#SO_OtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetSO_ItemTaxDetail();
    if (ItemID != "" && ItemID != null) {
        DelDeliSchAfterDelItem(ItemID);
    }
}
//------------------End------------------//

function CheckedFClose() {
    // debugger;
    if ($("#FClosed").is(":checked")) {
        $("#Cancelled").attr("disabled", true);
        $("#Cancelled").prop("checked", false);
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {
                //currentRow.find("#ItmFClosed").prop("checked", false);
            } else {
                currentRow.find("#ItmFClosed").prop("checked", true);
            }
            $("#btn_save").attr("Disabled", false);
            $("#btn_save").attr('onclick', "SaveBtnClick()");
           
    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        });
    }
    else {
        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();  
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {
               // currentRow.find("#ItmFClosed").prop("checked", false);
            } else {
                currentRow.find("#ItmFClosed").prop("checked", false);
            }
            //currentRow.find("#ItmFClosed").prop("checked", false);
            //$("#btn_save").attr('onclick', "");
            //$("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        });
        CheckedItemWiseFClose();
    }
  
}
function CheckedCancelled() {
    // debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#ChkFClose").attr("disabled", true);
        $("#ChkFClose").prop("checked", false);
    }
    else {
    }

    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr('onclick', "return SaveBtnClick()");
        $("#btn_save").attr("disabled", false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").attr("disabled", true);
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
function InsertSODetails() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var SRCTYP = $("#src_type").val();
    if (CheckSOValidations() == false) {
        return false;
    }
    if (CheckSOItemValidations() == false) {
        return false;
    }
    
    
    var DocMenuId = $("#DocumentMenuId").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y" && DocMenuId =="105103125") {
        if (Cmn_taxVallidation("SOItmDetailsTbl", "item_tax_amt", "SOItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
            return false;
        }
    }
    if (Cmn_CheckDeliverySchdlValidations("SOItmDetailsTbl", "hfItemID", "SOItemListName", "ord_qty_spec", "SNohiddenfiled") == false) {
        return false;
    }
    if (SRCTYP == "D") {  /*add by Hina sharma on 04-06-2025*/
        if (CheckValidations_forSubItems() == false) {
            return false;
        }
    }
    
    if ($("#SOItmDetailsTbl >tbody >tr").length == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145110") {
            var DecDigit = $("#ExpImpValDigit").text();
        }
        else {
            var DecDigit = $("#ValDigit").text();
        }
        var DiscountAmt = parseFloat(0).toFixed(DecDigit);

        $("#SOItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var IDV1 = currentRow.find("#item_disc_val").val();
            var ItemType = currentRow.find("#ItemType").val();
            if (IDV1 != null && IDV1 != "" && IDV1 != "NaN" && IDV1 != "0") {
                DiscountAmt = (parseFloat(DiscountAmt) + parseFloat(currentRow.find("#item_disc_val").val())).toFixed(DecDigit);
            }
        });
        if (DiscountAmt == null || DiscountAmt == "" || DiscountAmt == "NaN") {
            DiscountAmt = "0";
        }
        //if (DocMenuId == "105103125") {
        //    var flag = "N";
        //    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        //        var currentRow = $(this);
        //        var ItemType = currentRow.find("#ItemType").val();
        //        if (ItemType == "N") {
        //            flag = "Y";
        //            return false;
        //        }
        //    });
        //    if (flag == "N") {
        //        swal("", $("#AtleastOneStockableItemIsRequiredInOrder").text(), "warning");
        //        return false;
        //    }
        //}
        var src_type = $("#src_type").val();

        var FinalSOItemDetail = [];
        var FinalSOSubItemDetail = [];
        var FinalSODeliveryDetail = [];
        var FinalSOTaxDetail = [];
        var FinalSOOCTaxDetail = [];
        var FinalSOOCDetail = [];
        var FinalSOTermDetail = [];

        FinalSOItemDetail = InsertSOItemDetails();
        //FinalSOSubItemDetail = InsertSOSubItemDetails();
        FinalSOTaxDetail = InsertSOTaxDetails();
        FinalSOOCTaxDetail = InsertSO_OCTaxDetails();
        FinalSOOCDetail = InsertSOOtherChargeDetails();
        FinalSODeliveryDetail = InsertSOItem_DeliverySchDetails();
        FinalSOTermDetail = InsertSOTermConditionDetails();
        //var formData;
        $("#hdItemDetailList").val(JSON.stringify(FinalSOItemDetail));
        /*$("#SubItemDetailsDt").val(JSON.stringify(FinalSOSubItemDetail));*/
        $("#hdTaxDetailList").val(JSON.stringify(FinalSOTaxDetail));
        $("#hdOCTaxDetailList").val(JSON.stringify(FinalSOOCTaxDetail));
        $("#hdOCDetailList").val(JSON.stringify(FinalSOOCDetail));
        $("#hdDelSchDetailList").val(JSON.stringify(FinalSODeliveryDetail));
        $("#hdTermsDetailList").val(JSON.stringify(FinalSOTermDetail));

        ///*-----------Sub-item-------------*/

        var SubItemsListArr = Cmn_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);

        ///*-----------Sub-item end-------------*/
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $("#hdn_Attatchment_details").val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
        $("#Hdn_CustomerName").val($("#CustomerName").val());
        $("#Hdn_src_type").val($("#src_type").val());
        $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
        $("#Hdn_conv_rate").val($("#conv_rate").val());
        $("#Hdn_salesperson").val($("#salesperson").val());
        $("#TxtDiscountAmount").val(DiscountAmt);


        $("#Cancelled").attr('disabled', false);
    
        $("#ref_doc_no").prop("readonly", false);
        $("#exp_file_no").prop("readonly", false);
        $('#qt_number').attr("disabled", false);
        $("#ddlCurrency").attr("disabled", true);
        $("#cntry_destination").attr("disabled", false);
        $("#PortOfDestination").attr("disabled", false);
        $("#trade_term").attr("disabled", false);
        if ($("#ApplyTax_I").is(":checked")) {
            $("#Hdn_ApplyTax").val($("#ApplyTax_I").val());
        }
        if ($("#ApplyTax_E").is(":checked")) {
            $("#Hdn_ApplyTax").val($("#ApplyTax_E").val());
        }
    
        if (src_type != "D") {
            $("#Hdn_src_doc_number").val($("#src_doc_number").val());
        }
        debugger;
        var CustName = $("#CustomerName option:selected").text();
        $("#Hdn_SoCustName").val(CustName);

        var SalesPrsn = $("#salesperson option:selected").text();
        $("#Hdn_salespersnName").val(SalesPrsn)

        var SrcTyp = $("#salesperson option:selected").text();
        $("#Hdn_salespersnName").val(SrcTyp)

        var CurrName = $("#ddlCurrency option:selected").text();
        $("#Hdn_ddlCurrencyName").val(CurrName)

        var SrcDocNo = $("#qt_number option:selected").val();/*add by Hina sharma on 12-06-2025 for single quotation*/
        $("#Hdn_SO_SourceDocNo").val(SrcDocNo);

        var TxtGrssValue = $("#TxtGrossValue").val();
        var TxtAssbleValue = $("#TxtAssessableValue").val();
        if (parseFloat(CheckNullNumber(TxtGrssValue)) > 0 && parseFloat(CheckNullNumber(TxtAssbleValue)) > 0) {
            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;
        }
        else {
            swal("", $("#DocumentCanNotBeSavedGrossOrAssessableValueIsLessOrEqualToZero").text(), "warning");
            return false;
        }
    }
    else {
        //alert("Check network");
    }
};
function GetCurrentDatetime(ActionType) {
    // debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetCurrentDT",/*Controller=LSODetail and Fuction=GetCurrentDT*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',/*Registration pass value like model*/
        success: function (response) {
            // debugger;
            if (response === 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            if (ActionType === "Save") {
                $("#LSOCreatedBy").text(response.CurrentUser);
                $("#LSOCreatedDate").text(response.CurrentDT);
            }
            if (ActionType === "Edit") {
                $("#LSOAmdedBy").text(response.CurrentUser);
                $("#LSOAmdedDate").text(response.CurrentDT);
            }
            if (ActionType === "Approved") {
                $("#LSOApproveBy").text(response.CurrentUser);
                $("#LSOApproveDate").text(response.CurrentDT);
            }
        }
    });
}
//function DisableAfterSave() {
//   ////DisableHeaderField();
//    $("#SODetailRemarks").prop("readonly", true);

//    $("#SOItmDetailsTbl .plus_icon1").css("display", "none");
//    $("#SOItmDetailsTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        var Sno = currentRow.find("#SNohiddenfiled").val();        
//            currentRow.find("#delBtnIcon").css("display", "none");
//            currentRow.find("#SOItemListName" + Sno).attr("disabled", true);
//            currentRow.find("#ord_qty_spec").attr("disabled", true);
//            currentRow.find("#item_rate").attr("disabled", true);
//            currentRow.find("#item_disc_perc").attr("disabled", true);
//            currentRow.find("#item_disc_amt").attr("disabled", true);
//            currentRow.find("#item_ass_val").attr("disabled", true);
//            currentRow.find("#remarks").attr("disabled", true);
//            currentRow.find("#SimpleIssue").attr("disabled", true);
//            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
//            currentRow.find("#BtnTxtCalculation").attr("disabled", false);
//    });

//    $("#Tax_Template").attr('disabled', true);
//    $("#Tax_Type").attr('disabled', true);
//    $("#Tax_Percentage").prop("readonly", true);
//    $("#Tax_Level").attr('disabled', true);
//    $("#Tax_ApplyOn").attr('disabled', true);
//    $("#TaxResetBtn").css("display", "none");
//    $("#TaxSaveTemplate").css("display", "none");
//    $("#ReplicateOnAllItemsBtn").css("display", "none");
//    $("#SaveAndExitBtn").css("display", "none");
//    $("#TaxExitAndDiscard").css("display", "none");

//    $("#OtherCharge_DDL").attr('disabled', true);
//    $("#TxtOCAmt").prop("readonly", true);
//    $("#SaveAndExitBtn_OC").css("display", "none");
//    $("#DiscardAndExit_OC").css("display", "none");

//    $("#ForDisableOCDlt").val("Disable");

//    $("#DeliverySchItemDDL").attr('disabled', true);
//    $("#DeliveryDate").prop("readonly", true);
//    $("#DeliverySchQty").prop("readonly", true);
//    $(".plus_icon").css("display", "none");
//    $("#DeliverySchTble >tbody >tr").each(function () {
//        var currentRow = $(this);
//        currentRow.find("#DeliveryDelIconBtn").css("display", "none");
//    });

//    $("#TxtTerms_Condition").prop("readonly", true);
//    $(".plus_icon1").css("display", "none");

//    $("#TblTerms_Condition >tbody >tr").each(function () {
//        var currentRow = $(this);
//        currentRow.find("#TC_DelIcon").css("display", "none");
//    });
//    $("#TemplateTermsddl").attr('disabled', true);

//    //$("#file-1").attr('disabled', true);
//    /*------------------Attachment----------------*/
//    $("#file-1").attr('disabled', true);
///*------------------Attachment End----------------*/
//}
function DisableForSaveAfterApprove() {
    $("#FClosed").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);

    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();     
            currentRow.find("#ItmFClosed").attr("disabled", true);
    });
}
function InsertSOItemDetails() {
     debugger;
    
    var SONo = $("#so_no").val();
    var SODate = $("#so_date").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    var Branch = sessionStorage.getItem("BranchID");
    var SOItemList = [];
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
         debugger;
        var ItemID = "";
        var ItemName = "";
        var hsn_code = "";
        var UOMID = "";
        var UOMName = "";
        var subitem = "";
        var AvlStock = "";
        var OrderQty = "";
        var OrderBQty = "";
        var GRNQty = "";
        var InvQty = "0";
        var mrp = "";
        var mrp_disc = "";
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
        var FClosed = "";
        var Remarks = "";
        var OrderType = "";
        var ManualGST = "";
        var TaxExempted = "";
        var FOC = "";
        var FOCQty = "";
        var sr_no = 0;
        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();        
        var src_type = $("#src_type").val();
        debugger;
        ItemID = currentRow.find("#SOItemListName" + SNo).val();
        ItemName = currentRow.find("#SOItemListName" + SNo + " option:selected").text();
        var UOMID = currentRow.find("#UOMID").val();
        var UOMName = currentRow.find("#UOM").val(); 
        var subitem = currentRow.find("#sub_item").val();
        var hsn_code = currentRow.find("#ItemHsnCode").val();
        if (src_type == "D") {
            QuotationNumber = "Direct";
            QuotationDate = "Direct";
        }
        if (src_type == "Q") {
            QuotationNumber = currentRow.find("#QuotationNumber").val();
            QuotationDate = currentRow.find("#QuotationDate").val();
        }
      
            if (currentRow.find("#AvailableStockInBase").val() == null || currentRow.find("#AvailableStockInBase").val() == "") {
                AvlStock = "0";
            }
            else {
                AvlStock = currentRow.find("#AvailableStockInBase").val();
            }
            OrderQty = currentRow.find("#ord_qty_spec").val();
            //OrderBQty = currentRow.find("#ord_qty_base").val();
            if (currentRow.find("#grn_qty").val() == null || currentRow.find("#grn_qty").val() == "") {
                GRNQty = "0";
            }
            else {
                GRNQty = currentRow.find("#grn_qty").val();
            }
            //if (currentRow.find("#inv_qty").val() == null || currentRow.find("#inv_qty").val() == "") {
            //    InvQty = "0";
            //}
            //else {
            //    InvQty = currentRow.find("#inv_qty").val();
        //}
        if (DocumentMenuId == "105103125") {
            mrp = currentRow.find("#MRP").val();
            mrp_disc = currentRow.find("#MRPDiscount").val();
        }
        else{
            mrp = 0;
            mrp_disc = 0;
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
            if (currentRow.find("#ItmFClosed").is(":checked")) {
                FClosed = "Y";
            }
            else {
                FClosed = "N";
            }
        Remarks = currentRow.find("#remarks").val();     
        if ($("#OrderTypeD").is(":checked")) {
            OrderType = "D";
        }
        if ($("#OrderTypeE").is(":checked")) {
            OrderType = "E";
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
        sr_no = currentRow.find("#SRNO_").text();
        FOCQty = CheckNullNumber(currentRow.find("#foc_qty").val());
        if (currentRow.find("#FOC").is(":checked")) {
            FOC = "Y"
        }
        else {
            FOC = "N"
        }
        var price_list_no = currentRow.find("#Price_list_no").val();
        SOItemList.push({ SONo: SONo, SODate: SODate, Branch: Branch, QuotationNumber: QuotationNumber, QuotationDate: QuotationDate, ItemID: ItemID, ItemName: ItemName, UOMID: UOMID, UOMName: UOMName, subitem: subitem, AvlStock: AvlStock, OrderQty: OrderQty, OrderBQty: OrderBQty, GRNQty: GRNQty, InvQty: InvQty, mrp: mrp, mrp_disc: mrp_disc, ItmRate: ItmRate, ItmDisPer: ItmDisPer, ItmDisAmt: ItmDisAmt, DisVal: DisVal, GrossVal: GrossVal, AssVal: AssVal, TaxAmt: TaxAmt, OCAmt: OCAmt, NetValSpec: NetValSpec, NetValBase: NetValBase, FClosed: FClosed, Remarks: Remarks, OrderType: OrderType, TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, sr_no: sr_no, FOC: FOC, price_list_no: price_list_no, FOCQty: FOCQty});
    });
    return SOItemList;
   
};
function InsertSOTaxDetails() {
    debugger;
    var SONo = sessionStorage.getItem("LSO_No");
    var SODate = $("#so_date").val();
    var src_type = $("#src_type").val();

    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var SOTaxList = [];
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#SOItemListName" + RowSNo).val();
            var QuotationNumber = currentRow.find("#QuotationNumber").val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    if (src_type == "D") {
                        $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                            var Crow = $(this);
                            var OrderType = "";
                            if (src_type == "D") {
                                var QuotationNumber = "Direct";
                                var QuotationDate = "Direct";
                            }
                            else {
                                var QuotationNumber = Crow.find("#DocNo").text().trim();
                                var QuotationDate = Crow.find("#DocDate").text().trim();
                            }
                            if ($("#OrderTypeD").is(":checked")) {
                                OrderType = "L";
                            }
                            if ($("#OrderTypeE").is(":checked")) {
                                OrderType = "E";
                            }
                            var ItemID = Crow.find("#TaxItmCode").text().trim();
                            var TaxID = Crow.find("#TaxNameID").text().trim();
                            var TaxName = Crow.find("#TaxName").text().trim();
                            var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                            var TaxLevel = Crow.find("#TaxLevel").text().trim();
                            var TaxValue = Crow.find("#TaxAmount").text().trim();
                            var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                            var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                            var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                            SOTaxList.push({ Branch: "", SONo: "", SODate: "", QuotationNumber: QuotationNumber, QuotationDate: QuotationDate, TaxItmCode: ItemID, TaxNameID: TaxID, TaxName: TaxName, TaxPercentage: TaxRate, TaxAmount: TaxValue, TaxLevel: TaxLevel, TaxApplyOnID: TaxApplyOn, OrderType: OrderType, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount  });
                        });
                    }
                    else {
                        $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").find("#DocNo:contains('" + QuotationNumber + "')").closest("tr").each(function () {
                            //$("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                            var Crow = $(this);
                            var OrderType = "";
                            if (src_type == "D") {
                                var QuotationNumber = "Direct";
                                var QuotationDate = "Direct";
                            }
                            else {
                                var QuotationNumber = Crow.find("#DocNo").text().trim();
                                var QuotationDate = Crow.find("#DocDate").text().trim();
                            }
                            if ($("#OrderTypeD").is(":checked")) {
                                OrderType = "L";
                            }
                            if ($("#OrderTypeE").is(":checked")) {
                                OrderType = "E";
                            }
                            var ItemID = Crow.find("#TaxItmCode").text().trim();
                            var TaxID = Crow.find("#TaxNameID").text().trim();
                            var TaxName = Crow.find("#TaxName").text().trim();
                            var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                            var TaxLevel = Crow.find("#TaxLevel").text().trim();
                            var TaxValue = Crow.find("#TaxAmount").text().trim();
                            var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                            var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                            var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                            SOTaxList.push({ Branch: "", SONo: "", SODate: "", QuotationNumber: QuotationNumber, QuotationDate: QuotationDate, TaxItmCode: ItemID, TaxName: TaxName, TaxNameID: TaxID, TaxPercentage: TaxRate, TaxAmount: TaxValue, TaxLevel: TaxLevel, TaxApplyOnID: TaxApplyOn, OrderType: OrderType, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });

                        });
                    }                
                }
            }
        }
    });
    return SOTaxList;
};

function InsertSO_OCTaxDetails() {
    debugger;
    var SONo = sessionStorage.getItem("LSO_No");
    var SODate = $("#so_date").val();
    var src_type = $("#src_type").val();

    var FTaxDetails = $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").length;
    var SO_OCTaxList = [];

    if (FTaxDetails != null) {
        if (FTaxDetails > 0) {

            $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").each(function () {
                var Crow = $(this);
                var OrderType = "";
                if (src_type == "D") {
                    var QuotationNumber = "Direct";
                    var QuotationDate = "Direct";
                }
                else {
                    var QuotationNumber = Crow.find("#DocNo").text().trim();
                    var QuotationDate = Crow.find("#DocDate").text().trim();
                }
                if ($("#OrderTypeD").is(":checked")) {
                    OrderType = "L";
                }
                if ($("#OrderTypeE").is(":checked")) {
                    OrderType = "E";
                }
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

                SO_OCTaxList.push({ Branch: "", SONo: "", SODate: "", QuotationNumber: QuotationNumber, QuotationDate: QuotationDate, TaxItmCode: ItemID, TaxName: TaxName, TaxNameID: TaxID, TaxPercentage: TaxRate, TaxAmount: TaxValue, TaxLevel: TaxLevel, TaxApplyOnID: TaxApplyOn, OrderType: OrderType, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount  });

            });
        }
    }
    return SO_OCTaxList;
};

function InsertSOOtherChargeDetails() {
     debugger;
    var SONo = sessionStorage.getItem("LSO_No");
    var SODate = $("#so_date").val();
    var Branch = sessionStorage.getItem("BranchID");
    var SO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OCID = "";
            var OCValue = "";
            var OrderType = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            debugger;
            OCID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OrderType = "L";
            SO_OCList.push({ Branch: Branch, SONo: SONo, SODate: SODate, OCID: OCID, OCValue: OCValue, OCTaxAmt: OCTaxAmt, OCTotalTaxAmt: OCTotalTaxAmt, OrderType: OrderType, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs });
        });
    }
    return SO_OCList;
};
function InsertSOItem_DeliverySchDetails() {
    // debugger;
    var SONo = sessionStorage.getItem("LSO_No");
    var SODate = $("#so_date").val();
    var Branch = sessionStorage.getItem("BranchID");
    var SODelieryList = [];
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            var currentRow = $(this);
            var DeliverySchWhouse = "";
            var ItemID = "";
            var ItemName = "";
            var DeliverySchDate = "";
            var DeliverySchQty = "";
            var DeliverySchQty = "";
            var OrderType = "";
            DeliverySchWhouse = "0";
            ItemID = currentRow.find("#Hd_ItemIdFrDS").text();
            ItemName = currentRow.find("#Hd_ItemNameFrDS").text();
            DeliverySchDate = currentRow.find("#sch_date").text();
            DeliverySchQty = currentRow.find("#delv_qty").text();
            DeliverySchWhouse = DeliverySchWhouse;
            OrderType = "L";
            SODelieryList.push({ Branch: Branch, SONo: SONo, SODate: SODate, ItemID: ItemID, ItemName: ItemName, DeliverySchDate: DeliverySchDate, DeliverySchQty: DeliverySchQty, DeliverySchWhouse: DeliverySchWhouse, OrderType: OrderType });
        });
    }
    return SODelieryList;
};
function InsertSOTermConditionDetails() {
     //debugger;
    var SONo = sessionStorage.getItem("LSO_No");
    var SODate = $("#so_date").val();
    var Branch = sessionStorage.getItem("BranchID");
    var SOTermsList = [];
    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var TermsDescription = "";
            var OrderType = "";
            TermsDescription = currentRow.find("#term_desc").text();
            OrderType = "L";
            SOTermsList.push({ Branch: Branch, SONo: SONo, SODate: SODate, TermsDescription: TermsDescription, OrderType: OrderType });
        });
    }
    return SOTermsList;
};
function InsertSOSubItemDetails() {
    //debugger;
    var SOSubItemList = [];
    $("#hdn_Sub_ItemDetailTbl >tbody >tr").each(function (i, row) {
        debugger;
        var item_id = "";
        var sub_item_id = "";
        var qty = "";
        var currentRow = $(this);
        item_id = currentRow.find("#ItemId").val();
        sub_item_id = currentRow.find('#subItemId').val();
        qty = currentRow.find('#subItemQty').val();
        //var SNo = currentRow.find("#SNohiddenfiled").val();
        var src_type = $("#src_type").val();
       
        if (src_type == "D") {
            QuotationNumber = "Direct";
            QuotationDate = "Direct";
        }
        if (src_type == "Q") {
            QuotationNumber = currentRow.find("#QuotationNumber").val();
            QuotationDate = currentRow.find("#QuotationDate").val();
        }

        SOSubItemList.push({ item_id: item_id, sub_item_id: sub_item_id, qty: qty, QuotationNumber: QuotationNumber, QuotationDate: QuotationDate });
    });
    return SOSubItemList;

};
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
function SerialNoAfterDelete() {
  
    var SerialNo = 0;
    $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SRNO_").text(SerialNo);

    });
};

function CheckSOValidations() {
    // debugger;
    var ErrorFlag = "N";
    if ($("#CustomerName").val() === "0") {
        $('#SpanCustNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "Red");
        $("#SpanCustNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-CustomerName-container']").css("border-color", "#ced4da");
    }
    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "" || $("#ddlCurrency").val() == "0") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanCustCurrErrorMsg').text($("#valueReq").text());
        $("#SpanCustCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanCustCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
    }
    if ($("#salesperson").val() == null || $("#salesperson").val() == "" || $("#salesperson").val() == "0") {
        $("[aria-labelledby='select2-salesperson-container']").css("border-color", "Red");
        $("#salesperson").css("border-color", "red");
        $('#SpanSalesPersonErrorMsg').text($("#valueReq").text());
        $("#SpanSalesPersonErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-salesperson-container']").css("border-color", "#ced4da");
        $("#SpanSalesPersonErrorMsg").css("display", "none");
        $("#salesperson").css("border-color", "#ced4da");
    }
    if (parseFloat(CheckNullNumber($("#conv_rate").val())) <= 0) {
        $('#SpanExRateErrorMsg').text($("#valueReq").text());
        $("#SpanExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
    var hdn_AutoPR = $("#hdn_AutoPR").val();
    if (hdn_AutoPR == "Y")
    {
        var RequiredArea = $("#hdRequiredArea").val();
        if (RequiredArea == "0" || RequiredArea == "" || RequiredArea == null) {
            $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "Red");
            $("#ddlRequiredArea").css("border-color", "red");
            $('#vmRequiredArea').text($("#valueReq").text());
            $("#vmRequiredArea").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
            $("#vmRequiredArea").css("display", "none");
            $("#ddlRequiredArea").css("border-color", "#ced4da");
        }
      
    }
    else {
        $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
        $("#vmRequiredArea").css("display", "none");
        $("#ddlRequiredArea").css("border-color", "#ced4da");
        $("#hdRequiredArea").val("0");
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
function onchangesalesperson() {
    $("[aria-labelledby='select2-salesperson-container']").css("border-color", "#ced4da");
    $("#SpanSalesPersonErrorMsg").css("display", "none");
    $("#salesperson").css("border-color", "#ced4da");
}
function CheckSOItemValidations() {
     debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();

    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();

    var DocumentMenuId = $("#DocumentMenuId").val();
    var ErrorFlag = "N";
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
         debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#SOItemListName" + Sno).val() == "0") {
                currentRow.find("#SOItemListNameError").text($("#valueReq").text());
                currentRow.find("#SOItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SOItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
        }
        let ordQty = currentRow.find("#ord_qty_spec").val();
        let focQty = currentRow.find("#foc_qty").val();

        if ((parseFloat(CheckNullNumber(ordQty)) + parseFloat(CheckNullNumber(focQty))) == 0) {
            currentRow.find("#ord_qty_specError").text($("#valueReq").text());
            currentRow.find("#ord_qty_specError").css("display", "block");
            currentRow.find("#ord_qty_spec").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ord_qty_specError").css("display", "none");
            currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
        }
        if (currentRow.find("#FOC").is(":checked") || ( (parseFloat(CheckNullNumber(ordQty)) == 0 && parseFloat(CheckNullNumber(focQty)) ) > 0 )) {

        }
        else {
            
            if (currentRow.find("#ord_qty_spec").val() != "") {
                if (DocumentMenuId == "105103145110") {
                    if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(ExpImpQtyDigit) == 0) {
                        currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                        currentRow.find("#ord_qty_specError").css("display", "block");
                        currentRow.find("#ord_qty_spec").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#ord_qty_specError").css("display", "none");
                        currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
                    }
                }
                else {
                    if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(QtyDecDigit) == 0) {
                        currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                        currentRow.find("#ord_qty_specError").css("display", "block");
                        currentRow.find("#ord_qty_spec").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#ord_qty_specError").css("display", "none");
                        currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
                    }
                }

            }
            if (DocumentMenuId != "105103145110") {
                if (currentRow.find("#MRP").val() == "") {
                    currentRow.find("#item_mrpError").text($("#valueReq").text());
                    currentRow.find("#item_mrpError").css("display", "block");
                    currentRow.find("#MRP").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_mrpError").css("display", "none");
                    currentRow.find("#MRP").css("border-color", "#ced4da");
                }
                if (currentRow.find("#MRP").val() != "") {
                    if (DocumentMenuId == "105103145110") {
                        if (parseFloat(currentRow.find("#MRP").val()).toFixed(ExpImpRateDigit) == 0) {
                            currentRow.find("#item_mrpError").text($("#valueReq").text());
                            currentRow.find("#item_mrpError").css("display", "block");
                            currentRow.find("#MRP").css("border-color", "red");
                            ErrorFlag = "Y";
                        }
                        else {
                            currentRow.find("#item_mrpError").css("display", "none");
                            currentRow.find("#MRP").css("border-color", "#ced4da");
                        }
                    }
                    else {
                        if (parseFloat(currentRow.find("#MRP").val()).toFixed(RateDecDigit) == 0) {
                            currentRow.find("#item_mrpError").text($("#valueReq").text());
                            currentRow.find("#item_mrpError").css("display", "block");
                            currentRow.find("#MRP").css("border-color", "red");
                            ErrorFlag = "Y";
                        }
                        else {
                            currentRow.find("#item_mrpError").css("display", "none");
                            currentRow.find("#MRP").css("border-color", "#ced4da");
                        }
                    }

                }
            }
            var rate = currentRow.find("#item_rate").val();
            var discAmt = currentRow.find("#item_disc_amt").val();
            var discper = currentRow.find("#item_disc_perc").val();
            var mrpdiscper = currentRow.find("#MRPDiscount").val();
            if (parseFloat(CheckNullNumber(discAmt)) >= parseFloat(CheckNullNumber(rate))) {
                currentRow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
                currentRow.find("#item_disc_amtError").css("display", "block");
                currentRow.find("#item_disc_amt").css("border-color", "red");
                ErrorFlag = "Y";
            } else {
                currentRow.find("#item_disc_amtError").text("");
                currentRow.find("#item_disc_amtError").css("display", "none");
                currentRow.find("#item_disc_amt").css("border-color", "#ced4da");
            }

            if (parseFloat(CheckNullNumber(discper)) >= 100) {
                currentRow.find("#item_disc_percError").text($("#DiscountCanNotBeGreaterThan99").text());
                currentRow.find("#item_disc_percError").css("display", "block");
                currentRow.find("#item_disc_perc").css("border-color", "red");
                ErrorFlag = "Y";
            } else {
                currentRow.find("#item_disc_percError").text("");
                currentRow.find("#item_disc_percError").css("display", "none");
                currentRow.find("#item_disc_perc").css("border-color", "#ced4da");
            }
            if (parseFloat(CheckNullNumber(mrpdiscper)) >= 100) {
                currentRow.find("#MRPDiscountError").text($("#DiscountCanNotBeGreaterThan99").text());
                currentRow.find("#MRPDiscountError").css("display", "block");
                currentRow.find("#MRPDiscount").css("border-color", "red");
                ErrorFlag = "Y";
            } else {
                currentRow.find("#MRPDiscountError").text("");
                currentRow.find("#MRPDiscountError").css("display", "none");
                currentRow.find("#MRPDiscount").css("border-color", "#ced4da");
            }

            if (currentRow.find("#item_ass_val").val() == "") {
                currentRow.find("#item_ass_valError").text($("#valueReq").text());
                currentRow.find("#item_ass_valError").css("display", "block");
                currentRow.find("#item_ass_val").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_ass_valError").css("display", "none");
                currentRow.find("#item_ass_val").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_ass_val").val() != "") {
                if (DocumentMenuId == "105103145110") {
                    if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ExpImpRateDigit) == 0) {
                        currentRow.find("#item_ass_valError").text($("#valueReq").text());
                        currentRow.find("#item_ass_valError").css("display", "block");
                        currentRow.find("#item_ass_val").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#item_ass_valError").css("display", "none");
                        currentRow.find("#item_ass_val").css("border-color", "#ced4da");
                    }
                }
                else {
                    if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
                        currentRow.find("#item_ass_valError").text($("#valueReq").text());
                        currentRow.find("#item_ass_valError").css("display", "block");
                        currentRow.find("#item_ass_val").css("border-color", "red");
                        ErrorFlag = "Y";
                    }
                    else {
                        currentRow.find("#item_ass_valError").css("display", "none");
                        currentRow.find("#item_ass_val").css("border-color", "#ced4da");
                    }
                }

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
function CheckItemRowValidation(e) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();
    var ErrorFlag = "N";
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();
    if (currentRow.find("#SOItemListName" + Sno).val() == "0") {
        currentRow.find("#SOItemListNameError").text($("#valueReq").text());
        currentRow.find("#SOItemListNameError").css("display", "block");
        currentRow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid red");

        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#SOItemListNameError").css("display", "none");
        currentRow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (currentRow.find("#ord_qty_spec").val() == "") {
        currentRow.find("#ord_qty_specError").text($("#valueReq").text());
        currentRow.find("#ord_qty_specError").css("display", "block");
        currentRow.find("#ord_qty_spec").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#ord_qty_specError").css("display", "none");
        currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
    }
    if (currentRow.find("#ord_qty_spec").val() != "") {
        if (DocumentMenuId == "105103145110") {
            if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(ExpImpQtyDigit) == 0) {
                currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                currentRow.find("#ord_qty_specError").css("display", "block");
                currentRow.find("#ord_qty_spec").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qty_specError").css("display", "none");
                currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
            }
        }
        else {
            if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(QtyDecDigit) == 0) {
                currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                currentRow.find("#ord_qty_specError").css("display", "block");
                currentRow.find("#ord_qty_spec").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qty_specError").css("display", "none");
                currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
            }
        }

    }
    if (currentrow.find("#FOC").is(":checked")) {

    }
    else {
        
        if (currentRow.find("#MRP").val() == "") {
            currentRow.find("#item_mrpError").text($("#valueReq").text());
            currentRow.find("#item_mrpError").css("display", "block");
            currentRow.find("#MRP").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_mrpError").css("display", "none");
            currentRow.find("#MRP").css("border-color", "#ced4da");
        }
        if (currentRow.find("#MRP").val() != "") {
            if (DocumentMenuId == "105103145110") {
                if (parseFloat(currentRow.find("#item_rate").val()).toFixed(ExpImpRateDigit) == 0) {
                    currentRow.find("#item_mrpError").text($("#valueReq").text());
                    currentRow.find("#item_mrpError").css("display", "block");
                    currentRow.find("#MRP").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_mrpError").css("display", "none");
                    currentRow.find("#MRP").css("border-color", "#ced4da");
                }
            }
            else {
                if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_mrpError").text($("#valueReq").text());
                    currentRow.find("#item_mrpError").css("display", "block");
                    currentRow.find("#MRP").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_mrpError").css("display", "none");
                    currentRow.find("#MRP").css("border-color", "#ced4da");
                }
            }
        }
        if (currentRow.find("#item_ass_val").val() == "") {
            currentRow.find("#item_ass_valError").text($("#valueReq").text());
            currentRow.find("#item_ass_valError").css("display", "block");
            currentRow.find("#item_ass_val").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_ass_valError").css("display", "none");
            currentRow.find("#item_ass_val").css("border-color", "#ced4da");
        }
        if (currentRow.find("#item_ass_val").val() != "") {
            if (DocumentMenuId == "105103145110") {
                if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(ExpImpRateDigit) == 0) {
                    currentRow.find("#item_ass_valError").text($("#valueReq").text());
                    currentRow.find("#item_ass_valError").css("display", "block");
                    currentRow.find("#item_ass_val").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_ass_valError").css("display", "none");
                    currentRow.find("#item_ass_val").css("border-color", "#ced4da");
                }
            }
            else {
                if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_ass_valError").text($("#valueReq").text());
                    currentRow.find("#item_ass_valError").css("display", "block");
                    currentRow.find("#item_ass_val").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_ass_valError").css("display", "none");
                    currentRow.find("#item_ass_val").css("border-color", "#ced4da");
                }
            }

        }
    }
    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckedItemWiseFClose() {
    // debugger;
    var FClose = "N";
    var FinalFClose = "Y";
    $("#SOItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();        
            if (currentRow.find("#ItmFClosed").is(":checked")) {
                FClose = "Y";              
            }
            else {
                FinalFClose = "N";              
            }
    });
    if (FClose == "Y") {
        $("#btn_save").attr("Disabled", false);
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("Disabled", true);
        $("#btn_save").attr('onclick', '');
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    if (FClose == "Y" && FinalFClose == "Y") {
        $("#FClosed").prop("checked", true);
    }
    else {
        $("#FClosed").prop("checked", false);
    }
}

//------------------End------------------//
//------------------Sales Order Through Quotation------------------//
function ShowDoc_number(doc_no, Sr_No) {
    debugger;
    var check = "";
    var Qtnum = null;
    if ($("#SOItmDetailsTbl tbody tr").length > 0) {
        $("#SOItmDetailsTbl tbody tr").each(function () {
            var currentrow = $(this);
            //debugger;
            var Sno = currentrow.find("#SNohiddenfiled").val();
            var doc_no_tbl = currentrow.find("#QuotationNumber").val();
            if (doc_no_tbl == doc_no) {
                check = "Y";
                 Qtnum = $("#qt_number option[value='" + doc_no + "']").val();
                $("#qt_number option[value='" + doc_no + "']").removeClass("select2-hidden-accessible");
                if (Qtnum == null) {
                    var doc_date_tbl = currentrow.find("#QuotationDate").val();
                    $('#Textddl').append(`<option data-date="${moment(doc_date_tbl).format('DD-MM-YYYY')}" value="${doc_no_tbl}">${doc_no_tbl}</option>`);

                }
                

            }

        });
        if (check != "Y") {
            $("#qt_number option[value='" + doc_no + "']").removeClass("select2-hidden-accessible");
        }
    }
    else {
        $("#qt_number option[value='" + doc_no + "']").removeClass("select2-hidden-accessible");
        Qtnum = $("#qt_number option[value='" + doc_no + "']").val();
        if (Qtnum == null) {
            /*var doc_date_tbl = $("#SOItmDetailsTbl tbody tr #QuotationDate" + Sr_No).val();*/
            var doc_date_tbl = $("#SOItmDetailsTbl tbody tr #QuotationDate").val();
            $('#Textddl').append(`<option data-date="${moment(doc_date_tbl).format('DD-MM-YYYY')}" value="${doc_no}">${doc_no}</option>`);
        }
    }
    $("#SpanQTNoErrorMsg").css("display", "none");
    var firstEmptySelect = true;
    $('#qt_number').select2({
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

}
function OnChangeOrderType() {
     //debugger;   

    var OrderType = $('#src_type').val();
    $("#Hdn_src_type").val(OrderType);
    if (OrderType == "Q") {
        $("#div_replicateWith").css('display', 'none');
        $("#Qtno").css('display', 'block');
        $("#Qtdt").css('display', 'block');
        
        $("#AddQt").css('display', 'block');
        $("#ApplyTax_E").prop("checked", true)
        $("#ApplyTax_I").attr("disabled", true);
        $("#ApplyTax_E").attr("disabled", true);
        $("#SOItmDetailsTbl .plus_icon1").css("display", "none");
        $("#ThFoc").css("display", "none");
        $("#CustomerName").attr('onchange', '');
        $("#CustomerName").val("0").trigger('change');
        $("#CustomerName").attr('onchange', 'OnChangeCustomer(this)');
        $("#TxtBillingAddr").val('');
        $("#TxtShippingAddr").val('');
        $("#conv_rate").val('');
        DisableItemDetail();
        //debugger;
        $("#SOItmDetailsTbl >tbody >tr").remove();
        $("#SOItmDetailsTbl >thead >tr .required").css('display', 'none');
    }  
    ToHideAndShowQuotation();
    if (OrderType == "D") {
        $("#div_replicateWith").css('display', 'block');
        $("#Qtno").css('display', 'none');
        $("#Qtdt").css('display', 'none');      
        $("#AddQt").css('display', 'none');  
        $("#ApplyTax_E").prop("checked", true)
        $("#ApplyTax_I").attr("disabled", false);
        $("#ApplyTax_E").attr("disabled", false);
        //$("#SOItmDetailsTbl .plus_icon1").css("display", "block");  
        $("#CustomerName").attr('onchange', '');
        $("#CustomerName").val("0").trigger('change');
        $("#CustomerName").attr('onchange', 'OnChangeCustomer(this)');
        $("#TxtBillingAddr").val('');
        $("#TxtShippingAddr").val('');
        $("#conv_rate").val('');
        $("#SOItmDetailsTbl >tbody >tr").remove();
        //$("#SOItmDetailsTbl >tbody >tr").each(function () {
        //    var currentRow = $(this);
        //    var Sno = currentRow.find("#SNohiddenfiled").val();         
        //        currentRow.find("#SOItemListName" + Sno).attr("disabled", false);
        //        currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
        //        currentRow.find("#BtnTxtCalculation").attr("disabled", true);
        //        currentRow.find("#ord_qty_spec").attr("disabled", false);
        //        currentRow.find("#item_rate").attr("disabled", false);
        //        currentRow.find("#item_disc_perc").attr("disabled", false);
        //        currentRow.find("#item_disc_amt").attr("disabled", false);
        //        currentRow.find("#item_ass_val").attr("disabled", false);
        //        currentRow.find("#remarks").attr("disabled", false);
        //        currentRow.find("#SOItemListNameError").css("display", "none");
        //    currentRow.find("[aria-labelledby='select2-SOItemListName" + Sno + "-container']").css("border-color", "#ced4da");
        //        currentRow.find("#ord_qty_specError").css("display", "none");
        //        currentRow.find("#ord_qty_spec").css("border-color", "#ced4da");
        //        currentRow.find("#item_mrpError").css("display", "none");
        //    currentRow.find("#MRP").css("border-color", "#ced4da");
        //})
        $("#TxtGrossValue").val("");
        $("#TxtAssessableValue").val("");
        $("#TxtTaxAmount").val("");
        $("#SO_OtherCharges").val("");
        $("#NetOrderValueSpe").val("");
        $("#NetOrderValueInBase").val("");
        $("#TaxCalcItemCode").val("");
        $("#HiddenRowSNo").val("");
        $("#Tax_AssessableValue").val("");
        $("#TaxCalcGRNNo").val("");
        $("#TaxCalcGRNDate").val("");
        $("#Hdn_TaxCalculatorTbl >tbody > tr").remove();
        $("#SOItmDetailsTbl >thead >tr .required").css('display', '');
    }
}
function ToHideAndShowQuotation() {
    var OrderType = $('#src_type').val();
    if (OrderType == "Q") {
        /*commented and modify by Hina on 12-06-2025 for single quotation*/
        //$("#hQTNOrow").removeAttr("style");

        //$("#hQTDTrow").removeAttr("style");
        //$("#QTNOrow").removeAttr("style");
        //$("#QTDTrow").removeAttr("style");
        $("#hQTNOrow").css("display", "none");
        $("#hQTDTrow").css("display", "none");
        $("#QTNOrow").css("display", "none");
        $("#QTDTrow").css("display", "none");
    }
    if (OrderType == "D") {
        $("#hQTNOrow").css("display", "none");
        $("#hQTDTrow").css("display", "none");
        $("#QTNOrow").css("display", "none");
        $("#QTDTrow").css("display", "none");
    }
}
function OnChangeQuotNo(QuotNo) {
     debugger;
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ExchDecDigit = $("#ExchDigit").text();///Rate And Percentage
    var QuotNo = QuotNo.value;
    if (QuotNo == "---Select---") {
        $("#qt_dt").val("");     
        $("#qt_date").val("");
    }
    else {
        $("#SpanQTNoErrorMsg").css("display", "none");
        $("#qt_number").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-qt_number-container"]').css("border-color", "#ced4da");
        var SrcDocNo = $("#qt_number option:selected").val();/*add by Hina sharma on 12-06-2025 for single quotation*/
        $("#Hdn_SO_SourceDocNo").val(SrcDocNo);
    }
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/LSODetail/GetQuotDetail",
                data: {
                    QuotNo: QuotNo
                },
                success: function (data) {
                    // debugger;
                    if (data == 'ErrorPage') {
                        GRN_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#qt_date").val(arr.Table[0].qt_dt);
                            //$("#conv_rate").val(arr.Table[0].conv_rate);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed(ExchDecDigit));
                         
                        }
                    }
                },
            });
    } catch (err) {
        console.log("OnChangeQuotNo Error : " + err.message);
    }

}
function AddItemDetail() {
    debugger; 
    if (CheckSOHraderValidations() == false) {
        return false;
    }
    var DocumentMenuId = $("#DocumentMenuId").val();
    var so_no = $("#so_no").val();
    var QTNo = $("#qt_number").val();
    var QTDate = $("#qt_date").val();   
    var UserID = $("#UserID").text();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var ExpImpQtyDigit = $("#ExpImpQtyDigit").text();
    var ExpImpRateDigit = $("#ExpImpRateDigit").text();
    var ExpImpValDigit = $("#ExpImpValDigit").text();

    if (QTNo != null && QTNo != "" && QTDate != null && QTDate != "") {
        // debugger;
        var SOItemListData = JSON.parse(sessionStorage.getItem("itemList"));
        if (SOItemListData != null && SOItemListData != "") {
            if (SOItemListData.length > 0) {

            }
            else {
                BindDLLSOItemList()
                //GetSOItemList();
            }
        }
        else {
            BindDLLSOItemList()
            //GetSOItemList();
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSODetail/GetQTDetail",
            data: {
                QTNo: QTNo, QTDate: QTDate, so_no: so_no, DocumentMenuId: DocumentMenuId
            },
            dataType: "json",
            success: function (data) {
                //debugger
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    DisableHeaderField();
                    var arr = [];
                    //debugger;
                    arr = JSON.parse(data);
                    debugger;
                    //var FTaxDetails1 = JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
                    $("#SOItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNohiddenfiled").val();
                        var QTnumber = currentRow.find("#QuotationNumber").val();
                        var QTdate = currentRow.find("#QuotationDate").val();
                        if (QTnumber == QTNo && QTdate == QTDate) {

                            //for (var i = 0; i < FTaxDetails1.length; i++) {
                            //    //debugger;
                            //    if (FTaxDetails1[i].QTNo == QTNo && FTaxDetails1[i].QTDate == QTDate) {
                            //        FTaxDetails1.pop(FTaxDetails1[i]);
                            //    };
                            //}
                            if ($("#Hdn_TaxCalculatorTbl > tbody > tr #DocNo:contains(" + QTNo + ")").length > 0) {
                                $("#Hdn_TaxCalculatorTbl > tbody > tr #DocNo:contains(" + QTNo + ")").parent().remove();
                            }
                            debugger;
                            if ($("#hdn_Sub_ItemDetailTbl >tbody > tr > td #subItemSrcDocNo[value='" + QTNo + "']").length > 0) {
                                $("#hdn_Sub_ItemDetailTbl >tbody > tr > td #subItemSrcDocNo[value='" + QTNo + "']").closest('tr').remove();
                            }

                            currentRow.remove();
                        }

                    })
                    //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(FTaxDetails1));
                    var tblLength = $("#SOItmDetailsTbl >tbody >tr").length;
                    if (tblLength > 0) {
                        var tblLnght = tblLength + 1;
                        $("#HiddenRowSNo").val(tblLnght)
                    }
                    var QTationNo;
                    $("#SOItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNohiddenfiled").val();
                        QTationNo = currentRow.find("#QuotationNumber").val();
                        currentRow.find("#remarks").prop("disabled", false);
                    });
                    if (arr.Table1.length > 0) {
                        var rowIdx = 0;
                        if (QTationNo != "") {
                            for (var i = 0; i < arr.Table1.length; i++) {
                                AddNewRowForEditSOItem();
                                ResetTaxCalculationDetails();
                                $("#SOItmDetailsTbl >tbody >tr").each(function () {
                                    //$(this).find("#QTNOrow").removeAttr("style");/*commented by Hina on 12-06-2025 for single quotation*/
                                    //$(this).find("#QTDTrow").removeAttr("style");
                                });
                            }
                        }
                        else {
                            for (var i = 0; i < arr.Table1.length; i++) {
                                //  //debugger
                                if (tblLength == 0) {
                                    AddNewRowForEditSOItem();
                                }
                                else {
                                    if (i > 0) {

                                        AddNewRowForEditSOItem();
                                    }
                                }
                                $("#SOItmDetailsTbl >tbody >tr").each(function () {
                                    //$(this).find("#QTNOrow").removeAttr("style");/*commented by Hina on 12-06-2025 for single quotation*/
                                    //$(this).find("#QTDTrow").removeAttr("style");
                                });
                            }
                        }
                        if (DocumentMenuId == "105103145110") {
                            var PackedQty = parseFloat(0).toFixed(ExpImpQtyDigit);
                        }
                        else {
                            var PackedQty = parseFloat(0).toFixed(QtyDecDigit);
                        }
                        var DataSno = 0;
                        var Dmenu = $("#DocumentMenuId").val();
                        $("#SOItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            debugger;
                            var SNo = currentRow.find("#SNohiddenfiled").val();
                            var blank_QTationNo = currentRow.find("#QuotationNumber").val();

                            if (blank_QTationNo == "") {
                                var subitemflag = arr.Table1[DataSno].sub_item
                                if (subitemflag === "Y") {
                                    currentRow.find("#SubItemOrderQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemOrderQty").attr("disabled", false);
                                    currentRow.find("#SubItemAvlQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemAvlQty").attr("disabled", false);
                                    currentRow.find("#SubItemFocQty").attr("disabled", false);
                                }
                                else {
                                    currentRow.find("#SubItemOrderQty").css("filter", "grayscale(100%)");
                                    currentRow.find("#SubItemOrderQty").attr("disabled", true);
                                    currentRow.find("#SubItemAvlQty").css("filter", "grayscale(0%)");
                                    currentRow.find("#SubItemAvlQty").attr("disabled", true);
                                    currentRow.find("#SubItemFocQty").attr("disabled", true);

                                }
                                currentRow.find("#SOItemListName" + SNo).append(`<option value="${arr.Table1[DataSno].item_id}" selected>${arr.Table1[DataSno].item_name}</option>`);//.val(arr.Table1[DataSno].item_id).trigger('change.select2');
                                var clickedrow = currentRow.find("#SOItemListName" + SNo).closest("tr");

                                currentRow.find("#hfItemID").val(arr.Table1[DataSno].item_id);
                                currentRow.find("#UOM").val(arr.Table1[DataSno].uom_name);
                                currentRow.find("#UOMID").val(arr.Table1[DataSno].uom_id);
                                currentRow.find("#sub_item").val(arr.Table1[DataSno].sub_item);
                                currentRow.find("#QuotationNumber").val(QTNo);
                                currentRow.find("#QuotationDate").val(QTDate);

                                if (DocumentMenuId == "105103145110") {
                                    currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table1[DataSno].Avlstock).toFixed(ExpImpQtyDigit));
                                    currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table1[DataSno].ord_qty_spec).toFixed(ExpImpQtyDigit));
                                    currentRow.find("#PackedQuantity").val(parseFloat(arr.Table1[DataSno].pack_qty).toFixed(ExpImpQtyDigit));
                                    PackedQty = parseFloat(PackedQty).toFixed(ExpImpQtyDigit) + parseFloat(arr.Table1[DataSno].pack_qty).toFixed(ExpImpQtyDigit);
                                    currentRow.find("#grn_qty").val(parseFloat(arr.Table1[DataSno].ship_qty).toFixed(ExpImpQtyDigit));
                                    currentRow.find("#inv_qty").val(parseFloat(arr.Table1[DataSno].inv_qty).toFixed(ExpImpQtyDigit));
                                    currentRow.find("#MRP").val(parseFloat(arr.Table1[DataSno].item_rate).toFixed(ExpImpRateDigit)).change();
                                    currentRow.find("#item_rate").val(parseFloat(arr.Table1[DataSno].item_rate).toFixed(ExpImpRateDigit));
                                    currentRow.find("#item_disc_perc").val(parseFloat(arr.Table1[DataSno].item_disc_perc).toFixed(ExpImpRateDigit));
                                    currentRow.find("#item_disc_amt").val(parseFloat(arr.Table1[DataSno].item_disc_amt).toFixed(ExpImpValDigit));
                                    currentRow.find("#item_disc_val").val(parseFloat(arr.Table1[DataSno].item_disc_val).toFixed(ExpImpValDigit));
                                    currentRow.find("#item_gr_val").val(parseFloat(arr.Table1[DataSno].item_gr_val).toFixed(ExpImpValDigit));
                                    currentRow.find("#item_ass_val").val(parseFloat(arr.Table1[DataSno].item_ass_val).toFixed(ExpImpValDigit));
                                    currentRow.find("#item_oc_amt").val(parseFloat(arr.Table1[DataSno].item_oc_amt).toFixed(ExpImpValDigit));

                                }
                                else {
                                    currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table1[DataSno].Avlstock).toFixed(QtyDecDigit));
                                    currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table1[DataSno].ord_qty_spec).toFixed(QtyDecDigit));
                                    //currentRow.find("#foc_qty").val(parseFloat((arr.Table1[DataSno].foc_qty || 0)).toFixed(QtyDecDigit));
                                    currentRow.find("#PackedQuantity").val(parseFloat(arr.Table1[DataSno].pack_qty).toFixed(QtyDecDigit));
                                    PackedQty = parseFloat(PackedQty).toFixed(QtyDecDigit) + parseFloat(arr.Table1[DataSno].pack_qty).toFixed(QtyDecDigit);
                                    currentRow.find("#grn_qty").val(parseFloat(arr.Table1[DataSno].ship_qty).toFixed(QtyDecDigit));
                                    currentRow.find("#inv_qty").val(parseFloat(arr.Table1[DataSno].inv_qty).toFixed(QtyDecDigit));
                                    currentRow.find("#MRP").val(parseFloat(arr.Table1[DataSno].item_rate).toFixed(RateDecDigit)).change();
                                    currentRow.find("#item_rate").val(parseFloat(arr.Table1[DataSno].item_rate).toFixed(RateDecDigit));
                                    currentRow.find("#item_disc_perc").val(parseFloat(arr.Table1[DataSno].item_disc_perc).toFixed(RateDecDigit));
                                    currentRow.find("#item_disc_amt").val(parseFloat(arr.Table1[DataSno].item_disc_amt).toFixed(ValDecDigit));
                                    currentRow.find("#item_disc_val").val(parseFloat(arr.Table1[DataSno].item_disc_val).toFixed(ValDecDigit));
                                    currentRow.find("#item_gr_val").val(parseFloat(arr.Table1[DataSno].item_gr_val).toFixed(ValDecDigit));
                                    currentRow.find("#item_ass_val").val(parseFloat(arr.Table1[DataSno].item_ass_val).toFixed(ValDecDigit));
                                    currentRow.find("#item_oc_amt").val(parseFloat(arr.Table1[DataSno].item_oc_amt).toFixed(ValDecDigit));

                                    if (arr.Table1[DataSno].foc == "Y") {
                                        currentRow.find("#FOC").prop("checked", true);
                                        currentRow.find("#BtnTxtCalculation").prop("disabled", true);
                                    }
                                }
                                currentRow.find("#ItemHsnCode").val(arr.Table1[DataSno].hsn_code);
                                currentRow.find("#ItemType").val(arr.Table1[DataSno].SrvcType);
                                debugger;
                                if (arr.Table1[DataSno].force_close == "Y") {
                                    currentRow.find("#ItmFClosed").prop("checked", true);
                                }
                                else {
                                    currentRow.find("#ItmFClosed").prop("checked", false);
                                }
                                if (arr.Table1[DataSno].tax_expted == "Y") {
                                    currentRow.find("#TaxExempted").prop("checked", true);
                                }
                                else {
                                    currentRow.find("#TaxExempted").prop("checked", false);
                                }
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (GstApplicable == "Y") {
                                    if (arr.Table1[DataSno].manual_gst == "Y") {/*commented by Hina sharma on 11-06-2025 for as show hsn wise tax only instead of manual gst 
                                     after discus with vishal sir*/
                                        //currentRow.find("#ManualGST").prop("checked", true);
                                        //arr.Table1[DataSno].manual_gst == "N"
                                        currentRow.find("#ManualGST").prop("checked", false);
                                    }
                                    else {
                                        currentRow.find("#ManualGST").prop("checked", false);
                                    }
                                }
                                currentRow.find("#remarks").val(arr.Table1[DataSno].it_remarks);
                                currentRow.find("#SOItemListName" + SNo).attr("disabled", true);
                                currentRow.find("#ord_qty_spec").attr("disabled", true);
                                currentRow.find("#foc_qty").attr("disabled", true);//Added by Suraj Maurya on 12-01-2026
                                currentRow.find("#item_rate").attr("disabled", true);
                                currentRow.find("#item_disc_perc").attr("disabled", true);
                                currentRow.find("#item_disc_amt").attr("disabled", true);
                                currentRow.find("#item_ass_val").attr("disabled", true);
                                currentRow.find("#TaxExempted").attr("disabled", false);
                                if (GstApplicable == "Y") {
                                    currentRow.find("#ManualGST").attr("disabled", false);
                                }
                                var ItemId = currentRow.find("#hfItemID").val();
                                var SQNo = currentRow.find("#QuotationNumber").val();
                                    currentRow.find("#BtnTxtCalculation").prop("disabled", false);

                                if (arr.Table1[DataSno].foc == "Y") {
                                    currentRow.find("#BtnTxtCalculation").prop("disabled", true);
                                    currentRow.find("#TaxExempted").attr("disabled", true);
                                    if (GstApplicable == "Y") {
                                        currentRow.find("#ManualGST").attr("disabled", true);
                                    }
                                }
                               
                                $("#DeliverySchTble >tbody >tr").each(function () {
                                    // debugger;
                                    var currentRowDel = $(this);
                                    var DSchItemID = currentRowDel.find("#Hd_ItemIdFrDS").text();


                                    if (arr.Table1[DataSno].item_id === DSchItemID) {
                                        currentRowDel.remove();
                                    }
                                });
                                var Itm_ID = arr.Table1[DataSno].item_id;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                if (Dmenu == "105103125") {
                                    if (GstApplicable == "Y" && (arr.Table1[DataSno].manual_gst == "N" || arr.Table1[DataSno].manual_gst == "Y")) {/*commented by Hina on 11-06-2025 after discus with vishal sir*/
                                        Cmn_ApplyGSTToAtable("SOItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", arr.Table5, "", "", "Add");
                                    }
                    //                else if (GstApplicable == "Y" && arr.Table1[DataSno].manual_gst == "Y") {
                    //                    if (arr.Table2.length > 0) {
                    //                        for (var l = 0; l < arr.Table2.length; l++) {//Tax On Item
                    //                            if (arr.Table2[l].manual_gst == "Y" && arr.Table2[l].qt_no == SQNo && arr.Table2[l].item_id == ItemId)
                    //                            {


                    //                                $('#Hdn_TaxCalculatorTbl tbody').append(`<tr id="R${SNo}">
                    //                <td id="DocNo">${arr.Table2[l].qt_no}</td>
                    //                <td id="DocDate">${arr.Table2[l].qt_dt}</td>
                    //                <td id="TaxItmCode">${arr.Table2[l].item_id}</td>
                    //                <td id="TaxName">${arr.Table2[l].tax_name}</td>
                    //                <td id="TaxNameID">${arr.Table2[l].tax_id}</td>
                    //                <td id="TaxPercentage">${arr.Table2[l].tax_rate}</td>
                    //                <td id="TaxLevel">${arr.Table2[l].tax_level}</td>
                    //                <td id="TaxApplyOn">${arr.Table2[l].tax_apply_Name}</td>
                    //                <td id="TaxAmount">${arr.Table2[l].tax_val}</td>
                    //                <td id="TotalTaxAmount">${arr.Table2[l].item_tax_amt}</td>
                    //                <td id="TaxApplyOnID">${arr.Table2[l].tax_apply_on}</td>
                    //                <td id="TaxRecov">${arr.Table2[l].recov}</td>
                    //                <td id="TaxAccId">${arr.Table2[l].tax_acc_id}</td>
                    //                    </tr>`);

                    //                                if (ItemId == arr.Table2[l].item_id && SQNo == arr.Table2[l].qt_no) {
                    //                                    currentRow.find("#item_tax_amt").val(parseFloat(arr.Table2[l].item_tax_amt).toFixed(ValDecDigit));
                    //                                }

                    //                                $('#TaxCalculatorTbl tbody').append(`<tr id="R${SNo}">
                                    

                    //                <td id="taxname">${arr.Table2[l].tax_name}</td>
                    //                <td id="taxid">${arr.Table2[l].tax_id}</td>
                    //                <td id="taxrate">${arr.Table2[l].tax_rate}</td>
                    //                <td id="taxlevel">${arr.Table2[l].tax_level}</td>
                    //                <td id="taxapplyonname">${arr.Table2[l].tax_apply_Name}</td>
                    //                <td id="taxval">${arr.Table2[l].tax_val}</td>
                    //                <td id="taxapplyon">${arr.Table2[l].tax_apply_on}</td>
                    //                <td id="AccID">${arr.Table2[l].tax_acc_id}</td>
                    //                <td id="Acc_name">${arr.Table2[l].tax_acc_name}</td>
                    //                <td id="tax_recov">${arr.Table2[l].recov}</td>
                                    
                    //                    </tr>`);
                    //                                var ArrItemList = [];
                    //                                $("#SOItmDetailsTbl >tbody >tr").each(function () {
                    //                                    debugger;
                    //                                    var currentRow = $(this);
                    //                                    let Itmtaxexmp = "";
                    //                                    SOItmId = currentRow.find("#hfItemID").val()
                    //                                    taxexmp = currentRow.find("#TaxExempted").is(":checked");
                    //                                    if (taxexmp == true) {
                    //                                        Itmtaxexmp = "Y";
                    //                                    }
                    //                                    else {
                    //                                        Itmtaxexmp = "N";
                    //                                        ArrItemList.push({ SOItmId: SOItmId, Itmtaxexmp: Itmtaxexmp });
                    //                                    }
                                                       
                    //                                 });
                    //                               var ArrTaxList = [];
                    //                                $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
                    //                                    debugger;
                    //                                    var taxID = "";
                    //                                    //var taxAccID = "";
                    //                                    var taxName = "";
                    //                                    var totalTaxAmt = "";
                    //                                    var Taxpercentage = "";
                                                        
                    //                                    var CRow = $(this);
                    //                                    var taxItmId = CRow.find("#TaxItmCode").text();
                    //                                    var taxID = CRow.find("#TaxNameID").text();
                    //                                    var taxName = CRow.find("#TaxName").text();
                    //                                    var totalTaxAmt = CRow.find("#TotalTaxAmount").text();
                    //                                    var Taxpercentage = CRow.find("#TaxPercentage").text();
                    //                                    var taxexmp = currentRow.find("#TaxExempted").val();

                    //                                    for (var l = 0; l < ArrItemList.length; l++) {

                    //                                        if (ArrItemList[l].SOItmId == taxItmId) {
                    //                                            debugger;
                    //                                            if (ArrTaxList.findIndex(v => (v.taxID) == (taxID)) > -1) {
                    //                                                let getIndex = ArrTaxList.findIndex(v => (v.taxID) == (taxID));
                    //                                                ArrTaxList[getIndex].totalTaxAmt = parseFloat(CheckNullNumber(ArrTaxList[getIndex].totalTaxAmt)) + parseFloat(CheckNullNumber(totalTaxAmt));
                    //                                            } else {
                    //                                                    ArrTaxList.push({ taxID: taxID, taxName: taxName, totalTaxAmt: totalTaxAmt, Taxpercentage: Taxpercentage })
                    //                                            }
                    //                                        }
                    //                                    }
                                                            
                    //                                });

                    //                                /*return ArrTaxList;*/
                                                    
                    //    //if (ArrTaxList.findIndex(v => (v.taxID) == (arr.Table2[l].tax_id)) > -1) {
                    //    //    let getIndex = ArrTaxList.findIndex(v => (v.taxID) == (arr.Table2[l].tax_id));
                    //    //    ArrTaxList[getIndex].totalTaxAmt = parseFloat(CheckNullNumber(ArrTaxList[getIndex].totalTaxAmt)) + parseFloat(CheckNullNumber(arr.Table2[l].tax_val));
                    //    //} else {

                    //    //    ArrTaxList.push({ taxID: arr.Table2[l].tax_id, taxAccID: arr.Table2[l].tax_acc_id, taxName: arr.Table2[l].tax_name, totalTaxAmt: arr.Table2[l].tax_val, Taxpercentage: arr.Table2[l].tax_rate, ManualGst: arr.Table2[l].manual_gst, TaxExpm: arr.Table2[l].manual_gst })
                    //    //}
                    //}
                    //                        }
                    //                        debugger;
                    //                        let TotalTAmt = 0;
                    //                        //$("#Tbl_ItemTaxAmountList tbody tr").remove();
                    //                        for (var l = 0; l < ArrTaxList.length; l++) {
                    //                             /*if (ArrTaxList[l].ManualGst == "Y" *//*&& ArrTaxList[l].taxID != arr.Table2[l].tax_id*//*) {*/
                    //                                $("#Tbl_ItemTaxAmountList tbody").append(`<tr>
                    //                                <td>${ArrTaxList[l].taxName}</td>
                    //                                <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                    //                                <td hidden="hidden" id="taxID">${ArrTaxList[l].taxID}</td>
                    //                                <td hidden="hidden" id="TaxPerc">${ArrTaxList[l].Taxpercentage}</td>
                    //                            </tr>`)
                    //                                TotalTAmt = parseFloat(TotalTAmt) + parseFloat(ArrTaxList[l].totalTaxAmt);
                    //                            //}
                    //                            //else {
                    //                            //    var tbllen = $("#Tbl_ItemTaxAmountList > tbody > tr >td #taxID:contains(" + ArrTaxList[l].taxID + ")").length;
                    //                            //    if (tbllen > 0) {
                    //                            //        $("#Tbl_ItemTaxAmountList > tbody > tr >td #taxID:contains(" + ArrTaxList[l].taxID + ")").each(function () {
                    //                            //            var totaltaxamt = $(this).closest('tr').find("#TotalTaxAmount").text();
                    //                            //            TotalTAmt = parseFloat(TotalTAmt) + parseFloat(CheckNullNumber(totaltaxamt));
                    //                            //        });
                    //                            //    }
                    //                            //}
                                                
                    //                        }
                    //                        var ItemTaxAmountTotal = $("#_ItemTaxAmountTotal").text();
                    //                        //TotalTaxAmountFinal = (parseFloat(TotalTaxAmountFinal) + parseFloat(NewList[k].TotalTaxAmount)).toFixed(cmn_ValDecDigit);

                    //                        TotalTaxAmountFinal = (parseFloat(ItemTaxAmountTotal) + parseFloat(TotalTAmt)).toFixed(cmn_ValDecDigit);
                    //                        //$("#_ItemTaxAmountTotal").text(parseFloat(TotalTAmt).toFixed(ValDecDigit))
                    //                        $("#_ItemTaxAmountTotal").text(TotalTaxAmountFinal)
                    //                        debugger;
                                           
                                            
                    //                        //$("#HdnTaxOn").val('Item');
                    //                        //OnClickSaveAndExit('MGST');
                    //                    }
                    //                }

                                    else {

                                        $("#hd_tax_id").val(arr.Table1[DataSno].tmplt_id);
                                        if (arr.Table1[DataSno].tmplt_id != 0 && arr.Table5.length > 0) {
                                            $("#HdnTaxOn").val("Item");
                                            $("#TaxCalcItemCode").val(Itm_ID);
                                            $("#HiddenRowSNo").val(SNo);
                                            $("#Tax_AssessableValue").val(arr.Table1[DataSno].item_ass_val);
                                            $("#TaxCalcGRNNo").val(QTNo);
                                            $("#TaxCalcGRNDate").val(QTDate);
                                            var TaxArr = arr.Table5;
                                            let selected = []; selected.push({ item_id: arr.Table1[DataSno].item_id });
                                            selected = JSON.stringify(selected);
                                            TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                            selected = []; selected.push({ tmplt_id: arr.Table1[DataSno].tmplt_id });
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
                                    DataSno = DataSno + 1;
                                }  
                            });
                        if (DocumentMenuId == "105103145110") {
                            $("#TotalPackedQty").val(parseFloat(PackedQty).toFixed(ExpImpQtyDigit));
                        }
                        else {
                            $("#TotalPackedQty").val(parseFloat(PackedQty).toFixed(QtyDecDigit));
                        }
                    }
                    debugger;

                    
                        var rowIdx = 0;
                        var deletetext = $("#Span_Delete_Title").text();
                        debugger;
                        var arrMultiQtno = DeleteAllDataWithMultipleQtNo()
                        var len_arrMultiQtno = arrMultiQtno.length;
                        let Termlen = $("#TblTerms_Condition > tbody>tr").length;
                    if (len_arrMultiQtno == 1 && Termlen == 0)
                    {
                        if (arr.Table4.length > 0)
                        {
                            for (var y = 0; y < arr.Table4.length; y++)
                            {
                                $('#TblTerms_Condition tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
                            <td class="red"><i id="TC_DelIcon" class="deleteIcon fa fa-trash" title="${deletetext}"></i></td>
                            <td id="term_desc">${arr.Table4[y].term_desc}</td>
                            </tr>`);
                            }
                        }

                    }
                        else {
                            if (len_arrMultiQtno > 1) {
                                $("#TblTerms_Condition > tbody>tr").remove();
                            }
                       
                        /*commented and Modify code by hina sharma on 05-06-2025 for multiple QtNo*/
                        //for (var y = 0; y < arr.Table4.length; y++) {
                        //    //var checkTandC = "";
                        //    //$('#TblTerms_Condition tbody tr').each(function () {
                        //    //    var currentRow = $(this);
                        //    //    var Terms_Condition = currentRow.find("#term_desc").text();
                        //    //    if (arr.Table4[y].term_desc == Terms_Condition) {
                        //    //        checkTandC = "Duplicate";
                        //    //    }

                        //    //});
                        //    //if (checkTandC != "Duplicate") {
                        //        $('#TblTerms_Condition tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
                        //    <td class="red"><i id="TC_DelIcon" class="deleteIcon fa fa-trash" title="${deletetext}"></i></td>
                        //    <td id="term_desc">${arr.Table4[y].term_desc}</td>
                        //    </tr>`);
                        //    /*}   */                        
                        //}
                    }
                    $("#SOItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNohiddenfiled").val();                      
                        $("#SOItemListName" + SNo).attr('onchange', 'OnChangeSOItemName(' + SNo + ',event)');
                    });
                    debugger;
                    if (arr.Table6.length > 0) {
                        var rowIdx = 0;
                        var deletetext = $("#Span_Delete_Title").text();
                        for (var y = 0; y < arr.Table6.length; y++) {

                            var ItmId = arr.Table6[y].item_id;
                            var SubItmId = arr.Table6[y].sub_item_id;
                            var QtQty = arr.Table6[y].quot_qty;
                            var SrcQtNo = arr.Table6[y].qt_no;
                            var SrcQtDt = arr.Table6[y].qt_date;
                            if (SrcQtNo == QTNo) {
                               
                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${QtQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${SrcQtNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${SrcQtDt}'></td>
                            <td><input type="text" id="subItemFocQty" value=''></td>
                            </tr>`);
                            }
                        }
                    }
                    debugger;
                    /*code start Add by Hina sharma on 02-06-2025 to show OC and Tax on OC*/
                  /*  if (arr.Table3.length > 0) {*/
                        GetOCWithOCTaxDetailBySQNumber(arr)
                  /*  }*/
                    
                    /*code start Add by Hina sharma on 02-06-2025 to show OC and Tax on OC*/

                $("#SOItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var SNo = currentRow.find("#SNohiddenfiled").val();
                        $("#SOItemListName" + SNo).attr('onchange', 'OnChangeSOItemName(' + SNo + ',event)');
                    });
                }
                $("#ApplyTax_E").prop("checked", true)
                $("#ApplyTax_I").attr("disabled", true);
                $("#ApplyTax_E").attr("disabled", true);

                debugger;
                $("#Tbl_OC_Deatils >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var OC_name = currentRow.find("#OCName").text();

                });
                
                OnClickSaveAndExit_OC_Btn();
                
                CalculateAmount();
                BindDelivertSchItemList();
                /*commented by Hina sharma on 11-06-2025 it will work for single quotation to avoid multiple quotation */
                //$("#qt_number").attr("disabled", false);
                //$("#qt_number option[value='" + QTNo + "']").select2().hide();
                //$("#qt_number").val("---Select---").trigger("change");  
            }
        });
    }  
}
function OnClickbillingAddressIconBtn(e) {

    $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());
    var Cust_id = $('#CustomerName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var CustPros_type = "C";
   
    var status = $("#hfStatus").val().trim();
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
    var Cust_id = $('#CustomerName').val();
    var ship_add_id = $('#ship_add_id').val().trim();
    $('#hd_add_type').val("S");
    var CustPros_type = "C";
    var status = $("#hfStatus").val().trim();
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }

    var SO_no = $("#so_no").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, ship_add_id, status, SODTransType, SO_no);
}

function OnClickBuyerInfoIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#SOItemListName" + Sno).val();
    ItmName = clickedrow.find("#SOItemListName" + Sno + " option:selected").text()
    var Cust_id = $('#CustomerName').val();
    Cmn_BuyerInfoBtnClick(ItmCode, Cust_id);
}
function OnchangeConvRate(e) {
    //debugger;
    var RateDecDigit = $("#RateDigit").text();
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
    if (parseFloat(CheckNullNumber($("#conv_rate").val())) <= 0) {
        $('#SpanExRateErrorMsg').text($("#valueReq").text());
        $("#SpanExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        //ErrorFlag = "Y";
    }
    else {
        $("#SpanExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }

    $("#SOItmDetailsTbl > tbody > tr").each(function () {
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
    
    var DiscPer = $("#OrderDiscountInPercentage").val();
    if (parseFloat(CheckNullNumber(DiscPer)) > 0) {
        OnchangeDiscInPerc();
    }
    var DiscAmt = $("#OrderDiscountInAmount").val();
    if (parseFloat(CheckNullNumber(DiscAmt)) > 0) {
        OnchangeDiscInAmt();
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrderQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemFocQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemResQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRejQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRwkQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var src_type = $("#src_type").val();
    //var FOCFlag = "N";
    var DocumentMenuId = $("#DocumentMenuId").val();
    var clickdRow = $(e.target).closest('tr');
    //if (clickdRow.find("#FOC").is(":checked")) {
    //    FOCFlag = "Y";
    //}

    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    if (src_type == "D") {
        QuotationNumber = "Direct";
        QuotationDate = "Direct";
        var IsDisabled = $("#DisableSubItem").val();
        $("#subSrcDocNo").css('display', 'none');
        $("#subSrcDocdate").css('display', 'none');
    }
    if (src_type == "Q") {
        QuotationNumber = clickdRow.find("#QuotationNumber").val();
        //QuotationDate = clickdRow.find("#QuotationDate").val();
        //QuotationDate = clickdRow.find("#QuotationDate").val().split("-").reverse().join("-");
        QuotationDate = clickdRow.find("#QuotationDate").val();
        
        var IsDisabled = $("#DisableSubItem").val();
        IsDisabled = 'Y';
        $("#subSrcDocNo").css('display', 'block');
        $("#subSrcDocdate").css('display', 'block');
    }
    //if (FOCFlag == "Y") {
    //    IsDisabled = "Y";
    //}
    var ProductNm = clickdRow.find("#SOItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#SOItemListName" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var QtNo = clickdRow.find("#QuotationNumber").val();
    var Doc_no = $("#so_no").val();
    var Doc_dt = $("#so_date").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity" || flag == "FocQuantity") {
        if (src_type == "D") {
            $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {

                var row = $(this);
                //var List = {};
                //var srcQtNo = row.find('#subItemSrcDocNo').val();
                //if (srcQtNo == QtNo) {
                //    var List = {};
                //    List.item_id = row.find("#ItemId").val();
                //    List.sub_item_id = row.find('#subItemId').val();
                //    List.qty = row.find('#subItemQty').val();
                //    NewArr.push(List);

                //}
                //else {
                    var List = {};
                    List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                //if (FOCFlag == "Y") {
                //    List.qty = "0";
                //}
                //else {
                List.qty = row.find('#subItemQty').val();
                List.foc_qty = row.find('#subItemFocQty').val()||"";
                //}
                    
                    NewArr.push(List);
                /*}*/


                /* NewArr.push(List);*/
            });
        }
        else {
            /*add by Hina sharma on 04-06-2025*/
            //$("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            $("#hdn_Sub_ItemDetailTbl >tbody #ItemId[value='" + ProductId + "']").closest('tr').find("#subItemSrcDocNo[value='" + QuotationNumber + "']").closest("tr").each(function () {
                var row = $(this);
                var List = {};
                List.item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                //if (FOCFlag == "Y") {
                //    List.qty = "0";
                //}
                //else {
                    List.qty = row.find('#subItemQty').val();
                    List.foc_qty = row.find('#subItemFocQty').val()||"";
                //}
               
                List.src_doc_number = row.find('#subItemSrcDocNo').val();
                NewArr.push(List);
            });
        }
        Sub_Quantity = flag == "FocQuantity" ? clickdRow.find("#foc_qty").val() : clickdRow.find("#ord_qty_spec").val();
        $("#hdSub_SrcDocNo").val(QuotationNumber);
       $("#hdSub_SrcDocDate").val(QuotationDate);
        
    }
    else if (flag == "SOAvlQty" || flag =="AvlStock") {
        Sub_Quantity = clickdRow.find("#AvailableStockInBase").val();
    }
    //var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            QtNo: QtNo,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            DocumentMenuId: DocumentMenuId,
            src_type: src_type
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            if (src_type == "Q") {
                $("#subSrcDocNo").css('display', 'block');
                $("#subSrcDocdate").css('display', 'block');
                $("#Sub_SrcDocNo").val(QuotationNumber);
                $("#Sub_SrcDocDate").val(QuotationDate);
                $("#hdSub_SrcDocDate").val(QuotationDate);
            }
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "Y") == false) {
        return false;
    }
    
    if (DocumentMenuId == "105103125") {
        if (Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "foc_qty", "SubItemFocQty", "Y") == false) {
            return false;
        }
    }
    
    return true;
}
function ResetWorningBorderColor() {
    debugger;
    let errorFlg = "N";
    if (Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "N") == false) {
        errorFlg = "Y";
    };
    if (Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "foc_qty", "SubItemFocQty", "N") == false) {
        errorFlg = "Y";
    };
    if (errorFlg == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function GetSubItemAvlStock(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#SOItemListName" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#SOItemListName" + rowNo + " option:selected").val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStockInBase").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
    debugger;
    /*Add by Hina on 16-06-2025 to add for auto generate So by Sales Quotation*/
    if (Cmn_CheckDeliverySchdlValidations("SOItmDetailsTbl", "hfItemID", "SOItemListName", "ord_qty_spec", "SNohiddenfiled") == false) {
        return false;
    }
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
/*----------------- export items list to excel ------------------*/
function ExportItemsToExcel() {
    debugger;
    var soNo = $('#so_no').val();
    var soDate = $('#so_date').val();
    if (soNo != null && soNo != "" && soNo != undefined) {
        window.location.href = "/ApplicationLayer/LSODetail/ExportItemsToExcel?soNo=" + soNo + "&soDate=" + soDate;
    }
}
/*----------------- export items list to excel END------------------*/

/*----------------For Print Popup-------------------------------*/
function OnCheckedChangeProdDesc() {
    debugger
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $("#ShowProdDesc").val("Y");
        //$('#ShowCustSpecProdDesc').val('N');
        //$('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    debugger
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $('#ShowCustSpecProdDesc').val('Y');
        //$('#ShowProdDesc').val('N');
        //$('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeItemAliasName() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkitmaliasname').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$('#chkshowcustspecproddesc').prop('checked', false);
        $('#ItemAliasName').val('Y');
        //$("#ShowProdDesc").val('N');
        //$('#ShowCustSpecProdDesc').val('N');
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
//function onCheckedChangeFormatBtn() {
//    if ($("#OrderTypeImport").is(":checked")) {
//        $('#PrintFormat').val('F2');
//    }
//    else {
//        $('#PrintFormat').val('F1');
//    }
//}
function OnCheckedChangeCustAliasName() {
    debugger;
    if ($('#chkcustaliasname').prop('checked')) {
        $('#CustomerAliasName').val('Y');
    }
    else {
        $('#CustomerAliasName').val('N');
    }
}
function OnCheckedChangePrintQuantityTotal() {
    debugger;
    if ($('#chkshowtotalqty').prop('checked')) {
        $('#hdn_ShowTotalQty').val('Y');
    }
    else {
        $('#hdn_ShowTotalQty').val('N');
    }
}
function OnCheckedChangePrintItemImage() {
    debugger;
    if ($('#chkshowItemImage').prop('checked')) {
        $('#HdnPrintItemImage').val('Y');
    }
    else {
        $('#HdnPrintItemImage').val('N');
    }
}
function OnCheckedChangeDelvrySchdul() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkshowDelivrySchdl').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$('#chkshowcustspecproddesc').prop('checked', false);
        $('#hdnShowDelvrySchdule').val('Y');
        //$("#ShowProdDesc").val('N');
        //$('#ShowCustSpecProdDesc').val('N');
    }
    else {
        $('#hdnShowDelvrySchdule').val('N');
    }
}
function OnCheckedChangeProductCode() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkshowCustProdctCode').prop('checked')) {
        $('#hdnShowCustProductCode').val('Y');
    }
    else {
        $('#hdnShowCustProductCode').val('N');
    }
}
function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SOItmDetailsTbl", [{ "FieldId": "SOItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}

function GetOCWithOCTaxDetailBySQNumber(arr)
{
    debugger;
    var arrMultiQtno = DeleteAllDataWithMultipleQtNo()
    var len_arrMultiQtno = arrMultiQtno.length;
    let Oclen = $("#ht_Tbl_OC_Deatils > tbody>tr").length;
    var CurrencyInBase = "N";
    if (arr.Table3.length > 0) {
        var hdbs_curr = arr.Table3[0].bs_curr_id;
        var hdcurr = $("#ddlCurrency").val();
        if (hdbs_curr == hdcurr) {
            CurrencyInBase = "Y";
        } else {
            CurrencyInBase = "N";
        }
    }
    $("#CurrencyInBase").val(CurrencyInBase);
    if (len_arrMultiQtno == 1 && Oclen==0) {
            if (arr.Table3.length > 0) {
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
                var matchDoc = ("105103125").split(",").includes(DMenuId);
                displaynoneinDomestic = matchDoc ? "style='display:none;'" : "";
                var OC_For = "";
                var OCAccId = "";


                for (var m = 0; m < arr.Table3.length; m++) {

                    var OCName = arr.Table3[m].oc_name;
                    var OCID = arr.Table3[m].oc_id;
                    var OCValue = arr.Table3[m].oc_val;
                    var OCCurrId = arr.Table3[m].curr_id;
                    var OCCurr = arr.Table3[m].curr_name;
                    var OCConv = arr.Table3[m].conv_rate;
                    var OcAmtBs = arr.Table3[m].OCValBs;
                    var OCTaxAmt = arr.Table3[m].tax_amt;
                    var OCTaxTotalAmt = arr.Table3[m].total_amt;
                    var OCQtNo = arr.Table3[m].qt_no;
                    /*  if (OCQtNo == QTNo) {*/
                    debugger;
                    if (DMenuId == "105103125" && CurrencyInBase == "Y") {
                        tdTax = `<td class="num_right">
                        <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(OCTaxAmt)).toFixed(cmn_ValDecDigit)}</div>
                        <div class="col-md-2 col-sm-12" style="padding:0px;"><button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${tax_info}"></i></button></div>
                    </td>
                    <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(OCTaxTotalAmt)).toFixed(cmn_ValDecDigit)}</td>
                    `;
                    }
                    /*  }*/


                    //        $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
                    //<td id="OC_name">${arr.Table3[m].oc_name}</td>
                    //<td id="OC_Curr">${arr.Table3[m].curr_name}</td>
                    //<td id="HdnOC_CurrId">${arr.Table3[m].curr_id}</td>
                    //<td id="td_OCSuppName">${IsNull(arr.Table3[m].supp_name, '')}</td>
                    //<td id="td_OCSuppID">${IsNull(arr.Table3[m].supp_id, '')}</td>
                    //<td id="td_OCSuppType">${IsNull(arr.Table3[m].supp_type, '')}</td>
                    //<td id="OC_Conv">${arr.Table3[m].conv_rate}</td>
                    //<td hidden="hidden" id="OC_ID">${arr.Table3[m].oc_id}</td>
                    //<td class="num_right" id="OCAmtSp">${arr.Table3[m].oc_val}</td>
                    //<td class="num_right" id="OCAmtBs">${arr.Table3[m].OCValBs}</td>
                    //<td class="num_right" id="OCTaxAmt">${arr.Table3[m].tax_amt}</td>
                    //<td class="num_right" id="OCTotalTaxAmt">${arr.Table3[m].total_amt}</td>

                    //<td id="OCAccId" hidden>${isnull(OCAccId, '')}</td>
                    //<td id="OCSuppAccId" hidden>${IsNull(arr.Table4[l].supp_acc_id, '')}</td>
                    //</tr>`);



                    $('#Tbl_OC_Deatils tbody').append(`<tr id="R${rowIdx}">
<td id="deletetext" class="red center">${`<i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" title="${deletetext}" disabled></i>`}</td>
<td id="OCName" >${OCName}</td>
<td id="OCCurr" class="center" ${disblForJO}>${OCCurr}</td>
<td id="HdnOCCurrId" hidden>${OCCurrId}</td>
<td id="td_OCSuppName" style="display:none">${OcSupp_Name}</td>
<td id="td_OCSuppID" hidden >${OcSupp_Id}</td>
<td id="td_OCSuppType" hidden >${oc_supp_type}</td>
<td id="OCConv" class="num_right" ${disblForJO}>${parseFloat(OCConv).toFixed(cmn_ExchDecDigit)}</td>
<td hidden="hidden" id="OCValue">${OCID}</td>
<td class="num_right" id="OCAmount" ${displaynoneinDomestic}>${parseFloat(OCValue).toFixed(cmn_ValDecDigit)}</td>
<td class="num_right" id="OcAmtBs" ${disblForJO}>${parseFloat(OcAmtBs).toFixed(cmn_ValDecDigit)}</td>
  ` + tdTax + `
<td id="OCAccId" hidden >${OCAccId}</td>
<td id="OCSuppAccId" hidden >${OCSuppAccId}</td>
<td id="OCFor" hidden>${OC_For}</td>
</tr>`);

                }

            }
            debugger;
            var TaxOn = $("#HdnTaxOn").val();
            //var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
            if (TaxOn == "OC") {
                HdnTaxCalculateTable = "Hdn_OC_TaxCalculatorTbl";
            }

            /*---------------------Tax On OC-----------------------*/
            if (arr.Table7.length > 0) {
                var rowIdx = 0;

                for (var n = 0; n < arr.Table7.length; n++) {

                    var OCId = arr.Table7[n].item_id;
                    var OCQtNo = arr.Table7[n].qt_no;
                    var OCQtDt = arr.Table7[n].qt_dt;
                    var OCTaxId = arr.Table7[n].tax_id;
                    var OCTaxName = arr.Table7[n].tax_name;
                    var OCPercentg = arr.Table7[n].tax_rate;
                    var OCTaxval = arr.Table7[n].tax_val;
                    var OCTaxLevel = arr.Table7[n].tax_level;
                    var OCTaxApplyOnId = arr.Table7[n].tax_apply_on;
                    var OCTaxAmt = arr.Table7[n].item_tax_amt;
                    var OCTaxApplyOnName = arr.Table7[n].tax_apply_Name;
                    var OCTaxAccId = arr.Table7[n].acc_id;
                   /* if (OCQtNo == QTNo) {*/


                        $("#Hdn_OC_TaxCalculatorTbl >tbody").append(`<tr>
                            <td id="TaxItmCode">${OCId}</td>
                            <td id="TaxName">${OCTaxName}</td>
                            <td id="TaxNameID">${OCTaxId}</td>
                            <td id="TaxPercentage">${OCPercentg}</td>
                            <td id="TaxLevel">${OCTaxLevel}</td>
                            <td id="TaxApplyOn">${OCTaxApplyOnName}</td>
                            <td id="TaxAmount">${OCTaxval}</td>
                            <td id="TotalTaxAmount">${OCTaxAmt}</td>
                            <td id="TaxApplyOnID">${OCTaxApplyOnId}</td>
                            <td id="DocNo">${OCQtNo}</td>
                            <td id="DocDate">${OCQtDt}</td>

                            </tr>`);
                   /* }*/
                    $("#Hdn_OCTemp_TaxCalculatorTbl tbody").html($("#Hdn_OC_TaxCalculatorTbl tbody").html());
                }
            }
        }
    else
    {
        if (len_arrMultiQtno > 1) {
                $("#Hdn_OC_TaxCalculatorTbl > tbody>tr").remove();
                $("#Hdn_OCTemp_TaxCalculatorTbl > tbody >tr").remove();
                $("#ht_Tbl_OC_Deatils > tbody>tr").remove();
                $("#Tbl_OC_Deatils > tbody >tr").remove();
                $("#Tbl_OtherChargeList > tbody >tr").remove();
            }
    }
   

}

function DeleteAllDataWithMultipleQtNo() {
    debugger;
    var tblLength = $("#SOItmDetailsTbl >tbody >tr").length;
    if (tblLength > 0) {
        var ItemSQNO = new Array();
        let a1 = [];
        $("#SOItmDetailsTbl >tbody >tr").each(function (i, row) {
            debugger;
            var arrayItm = {};
            var currentRow = $(this);
            var SNo = currentRow.find("#SNohiddenfiled").val();
            //arrayItm.ItemID = currentRow.find("#SOItemListName" + SNo).val();
            //arrayItm.ItemName = currentRow.find("#SOItemListName" + SNo + " option:selected").text();
            arrayItm.QTNO = currentRow.find("#QuotationNumber").val();
            ItemSQNO.push(arrayItm);
            //a1 = a.filter((e, i, self) => i === self.indexOf(e));
        });
        //const uniqueArray = ItemSQNO.filter((value, index, self) =>
        //    index === self.findIndex((t) => (
        //        t.QTNO === value.QTNO
        //    ))
        //)
        //list = ItemSQNO.indexOffilter(function (x, i, a) {
        //    return a.(
        //var uniqueArray = Array.from(new Set(Ix) === i;
        //});temSQNO));
        //const uniqueArray1 = ItemSQNO.filter((value, index, self) => self.indexOf(value) === index)
        var len = ItemSQNO.length;
        for (let i = 0; i < len; i++) {

            // Check if the element exist in the new array
            if (!a1.includes(ItemSQNO[i].QTNO)) {

                // If not then push the element to new array
                a1.push(ItemSQNO[i].QTNO);
            }
        }
        return a1
     }
}
//-------------------------External Email Alert ------------------------------------
//Added by Nidhi on 04-08-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Cust_id = $("#CustomerName option:selected").val();
    Cmn_SendEmail(docid, Cust_id, 'Cust');
}
//Added by Nidhi on 04-08-2025
function ViewEmailAlert() {
    debugger;
    var srcType = $('#src_type').val();
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#so_no").val();
    var Doc_dt = $("#so_date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            ItemAliasName: $("#ItemAliasName").val(),
            ShowTotalQty: $("#hdn_ShowTotalQty").val(),
            PrintItemImage: $("#HdnPrintItemImage").val(),
            ShowDeliverySchedule: $("#hdnShowDelvrySchdule").val(),
            ShowCustProductCode: $("#hdnShowCustProductCode").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/LSODetail/SavePdfDocToSendOnEmailAlert_Ext",
                data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, srcType: srcType, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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
//Added by Nidhi on 04-08-2025
function SendEmailAlert() {
    debugger;
    var srcType = $('#src_type').val();
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SONo = $("#so_no").val();
    var SODate = $("#so_date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, SONo, SODate, statusAM, "/ApplicationLayer/LSODetail/SendEmailAlert", filepath, srcType)
}
//Added by Nidhi on 04-08-2025
function EmailAlertLogDetails() {
    debugger;
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var SONo = $("#so_no").val();
    var SODate = $("#so_date").val();
    Cmn_EmailAlertLogDetails(status, docid, SONo, SODate)
}
function PrintFormate() {
    debugger;
    var srcType = $('#src_type').val();
    var status = $('#hfStatus').val();
    var Doc_no = $("#so_no").val();
    var Doc_dt = $("#so_date").val();
    var docid = $("#DocumentMenuId").val();
    if (status == 'A') {
        var PrintFormatArray = [];
        var printFormat = {
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            ItemAliasName: $("#ItemAliasName").val(),
            ShowTotalQty: $("#hdn_ShowTotalQty").val(),
            PrintItemImage: $("#HdnPrintItemImage").val(),
            ShowDeliverySchedule: $("#hdnShowDelvrySchdule").val(),
            ShowCustProductCode: $("#hdnShowCustProductCode").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/LSODetail/SavePdfDocToSendOnEmailAlert_Ext",
            data: { Doc_no: Doc_no, Doc_dt: Doc_dt, docid: docid, srcType: srcType, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
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

function onclickAutoPR() {
    if ($("#AutoPR").is(":checked")) {
        $("#hdn_AutoPR").val("Y");
        $("#Div_RequirementArea").css("display", "block");
    }
    else {
        $("#hdn_AutoPR").val("N");
        $("#ddlRequiredArea").val("0").change();
        $("#Div_RequirementArea").css("display", "none");
    }
}
function onChangeRequiredArea() {
    debugger;
    var RequisitionArea = $('#ddlRequiredArea').val();
    $("#hdRequiredArea").val(RequisitionArea);
    $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
    $("#vmRequiredArea").css("display", "none");
    $("#ddlRequiredArea").css("border-color", "#ced4da");
}
function GetPriceListDetails(e) {
    debugger;
    var cust_id = $('#CustomerName').val();
    var clickdRow = $(e.target).closest('tr');
    var Price_list_no = clickdRow.find("#Price_list_no").val();
    var SNo = clickdRow.find("#SNohiddenfiled").val();
    var item_id = clickdRow.find("#SOItemListName" + SNo).val()
    var ItmName = clickdRow.find("#SOItemListName" + SNo + " option:selected").text();
    var UOM = clickdRow.find("#UOM").val();
    var disable = $("#Disable").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetPriceListDetails",
        data: {
            cust_id: cust_id,
            item_id: item_id,
            disable: disable,
            OrdDate: $("#so_date").val(),
        },
        success: function (data) {
            debugger;
            $("#PriceListDetails").html(data);

            if (Price_list_no != "") {
                $("#tbl_PriceListDetail > tbody > tr #list_no:contains(" + Price_list_no + ")").closest('tr').each(function () {
                    let row = $(this);
                    row.find('#plselect').prop("checked", true);
                });
            }                      
            $("#P_ItemName").val(ItmName);
            $("#P_UOM").val(UOM);
        }
    });
}
function OnClickSaveExistBtn() {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    $("#tbl_PriceListDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#plselect").is(":checked")) {
            var ItemId = currentRow.find("#pl_item_id").text();
            var list_no = currentRow.find("#list_no").text();
            var Pl_mrp = currentRow.find("#Pl_mrp").text();
            var pl_disc_mrp = currentRow.find("#pl_disc_mrp").text();
            var pl_disc_perc = currentRow.find("#pl_disc_perc").text();
            var pl_effect_price = currentRow.find("#pl_effect_price").text();

            $("#SOItmDetailsTbl > tbody > tr #hfItemID[value=" + ItemId + "]").closest('tr').each(function () {
                let row = $(this);
                row.find("#MRP").val(Pl_mrp);
                row.find("#MRPDiscount").val(pl_disc_mrp);
                
                row.find("#item_disc_perc").val(pl_disc_perc);
                row.find("#Price_list_no").val(list_no);
                //row.find("#item_rate").val(pl_effect_price).trigger("change");
                if (DocumentMenuId != "105103145110") {
                    ResetItemRate(row);
                }
                ApplyFoc(row);
                $("#btnplOrderQtySaveAndExit").attr("data-dismiss", "modal");
            });
        }       
    });
}
function onclickplselect(e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var list_no = clickdRow.find("#list_no").text();
    if (clickdRow.find("#plselect").is(":checked")) {
        $("#tbl_PriceListDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var pl_list_no = currentRow.find("#list_no").text();
            if (pl_list_no != list_no) {
                currentRow.find('#plselect').prop("checked", false);
            }
        });
    }
}



function OnChangeFocQty(e) {
    let clickedrow = $(e.target).closest("tr");

    let OrdQty = CheckNullNumber(clickedrow.find("#ord_qty_spec").val());
    let FocQty = CheckNullNumber(clickedrow.find("#foc_qty").val());
    clickedrow.find("#foc_qty").val(parseFloat(FocQty).toFixed(QtyDecDigit));
    if ((parseFloat(OrdQty) + parseFloat(FocQty)) == 0) {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#foc_qty").css("border-color", "red");
    } else {
        clickedrow.find("#ord_qty_specError").text("");
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
        clickedrow.find("#foc_qty").css("border-color", "#ced4da");
    }
    if (parseFloat(OrdQty) == 0) {
        clickedrow.find("#item_mrpError").text("");
        clickedrow.find("#item_mrpError").css("display", "none");
        clickedrow.find("#MRP").css("border-color", "#ced4da");
    }
    BindDelivertSchItemList();
}
function ApplyFoc(row) { //Created by Suraj Maurya on 19-01-2026
    var DocumentMenuId = $("#DocumentMenuId").val();
    var CustId = $("#CustomerName").val();
    var OrdDate = $("#so_date").val();

    var srNo = row.find("#SNohiddenfiled").val();
    var ItemId = row.find("#SOItemListName" + srNo).val();
    var OrderQty = CheckNullNumber(row.find("#ord_qty_spec").val());
    var OrderValue = CheckNullNumber(row.find("#item_gr_val").val());
    if (DocumentMenuId == "105103125") {//Added by Suraj Maurya on 19-01-2026
        var FocDtl = Cmn_getSchemeFOCQty(ItemId, CustId, OrderQty, OrderValue, OrdDate);
        if (FocDtl.length > 0) {
            row.find("#foc_qty").val(FocDtl[0].foc_qty);
        }
    }
}