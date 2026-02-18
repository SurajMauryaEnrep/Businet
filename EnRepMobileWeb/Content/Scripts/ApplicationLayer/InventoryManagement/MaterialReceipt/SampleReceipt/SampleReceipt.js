/**Modified By Nitesh 10-10-2023 17:00 for Src_type (against Issue ) Sample Name And Uom Is position is Change is Come After src_type and Disable Sample Name**/
/************************************************
Javascript Name:Sample Receipt Detail
Created By:Prem
Created Date: 29-07-2021
Description: This Javascript use for the Sample Receipt many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "D") {
        $("#SR_ItmDetailsTbl").removeClass('width2400');
        $("#SR_ItmDetailsTbl").addClass('width1500');
    }
    else {
        $("#SR_ItmDetailsTbl").removeClass('width1500');
        $("#SR_ItmDetailsTbl").addClass('width2400');
    }

    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#SR_Date").val() == "" || $("#SR_Date").val() == null) {
        $("#SR_Date").val(CurrentDate);
    }
    //var CurrentDate = moment().format('YYYY-MM-DD');
    //if ($("#SR_Date").val() == "0001-01-01") {
    //     $("#SR_Date").val(CurrentDate);
    //     $("#SR_Date").val(CurrentDate);
    // }
    $("#SRListTbl #SampleTrackingTbl tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var WF_status = $("#WF_status").val();
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var SRId = clickedrow.children("#SRNo").text();
            var SRDate = clickedrow.children("#SR_DT").text();
            if (SRId != null && SRId != "") {
                window.location.href = "/ApplicationLayer/SampleReceipt/EditSR/?SRId=" + SRId + "&SRDate=" + SRDate + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#SampleTrackingTbl >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var SR_No = clickedrow.children("#SRNo").text();
        var SR_Date = clickedrow.children("#SR_DT").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(SR_No);

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(SR_No, SR_Date, Doc_id, Doc_Status);
    });
    $("#HistoryTo_date").val(CurrentDate);
    BindSampleRcptItmList(1);

    $('#SR_ItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        debugger;

        var sou_type = $("#ddlsource_type" + " option:selected").val();
        if (sou_type == "D") {
        }
        else {
            var click = $(this);
            var rowid = click.find("#SpanRowId").text();
            var srcdocno = click.find("#src_doc_" + rowid).val();
            var srcdocdate = click.find("#src_docdate_" + rowid).val();
            sampleitemlist();
            // bindsrcno(srcdocno, srcdocdate);
        }

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

        var ItemCode = $(this).closest('tr').find("#hf_ItemID").val();
        if (ItemCode != "" && ItemCode != null && ItemCode != "0") {
            ShowItemListItm(ItemCode);
        }
        SerialNoAfterDelete();
        Cmn_DeleteSubItemQtyDetail(ItemCode);
    });
    ListRowHighLight();
    SRNo = $("#SR_Number").val();
    $("#hdDoc_No").val(SRNo);
    //$("#collapseFive").accordion({ active: false, collapsible: true });
    //panel - collapse hide collapse show
    //$("#FilesUpload").css("display", "block");
    //$("#file-1").attr('disabled', true);
    $('#button').on('click', function () {
        $('#AttachID').show();
    });
    //$("#collapseFive").accordion({ active: true, collapsible: true });
    //$("#AttachID").css("display", "block");
    //$(".button").css("display", "block")
    if ($("#SR_ItmDetailsTbl >tbody >tr").length > 0) {
        var sou_type = $("#ddlsource_type" + " option:selected").val();
        if (sou_type == "D") {
            //$("#DivAddNewItemBtn").css("display", "block");
            $("#samplenameid").css("display", "none");
            $("#uomid").css("display", "none");
            $("#srcnumberdiv").css("display", "none");
            $("#srcdocdatediv").css("display", "none");
            //$("#plusbtn").css("display", "none");
            $("#src_doctbl").css("display", "none");
            $("#src_docdatetbl").css("display", "none");
            $("#iss_datetbl").css("display", "none");
            $("#iss_qntytbl").css("display", "none");
            $("#pend_qtytbl").css("display", "none");
            $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                currentRow.find("#src_doctble").css("display", "none");
                currentRow.find("#src_docdatetble").css("display", "none");
                currentRow.find("#iss_datetble").css("display", "none");
                currentRow.find("#iss_qntytble").css("display", "none");
                currentRow.find("#pend_qtytble").css("display", "none");
            })
        }
        else {
            // $("#plusbtn").css("display", "none");
            // $("#DivAddNewItemBtn").css("display", "none");
            $("#samplenameid").css("display", "block");
            $("#uomid").css("display", "block");
            $("#srcnumberdiv").css("display", "block");
            $("#srcdocdatediv").css("display", "block");
            //$("#plusbtnhd").css("display", "block");
            sampleitemlist();
            if ($("#ddlItemName").val() == "0") {
                var smple_item = $("#ddlItemName").val();
                $("#ddlItemName option[value='" + smple_item + "']").select2().hide();
            }

        }
    }
    else {
        OnChangesourcetype();
    }
    CancelledRemarks("#Cancelled", "Disabled");
});
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
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
}
/*-----------------STARTING---- JS Work by using MODEL instead of whole js work -------------------------- */
function AddNewRow() {
    debugger;
    //if (CheckValidations() == false) {
    //    return false;
    //}

    var rowIdx = 0;
    debugger;
    var rowCount = $('#SR_ItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#SR_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td class="ItmNameBreak itmStick tditemfrz">
<div class="col-sm-11 lpo_form" style="padding:0px;" id="multiWrapper">
<select class="form-control" id="ItemName_${RowNo}" onchange="BindSR_ItemList(event)" name="Itemname">
</select>
<input  type="hidden" id="hf_ItemID" />
<span id="ItemNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
</td>
<td>
<input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly">
<input id="UOMID" type="hidden" />
</td>
<td><input id="SampleType" value="" maxlength = "50" class="form-control" autocomplete="off" type="text"  onkeyup="OnKeyupsam_typ(event)" onchange="OnChangesm_type(event);" name="SampleType" placeholder="${$("#span_SampleType").text()}"></td>
<td><input id="OtherDetail" value=""  maxlength = "250" class="form-control" autocomplete="off" type="text" onkeyup="OnKeyupoth_dtl(event)" onchange="OnChangeoth_dtl(event);" name="OtherDetail"  placeholder="${$("#span_OtherDetail").text()}"></td>

<td>
<div class="lpo_form">
<input class="form-control" id="ReceivedDate" value="" onchange="onchangerecdate()"   type="date" name="ReceivedDate">
<span id="spanerrorREcdate" class="error-message is-visible"></span>
</div>
</td>

<td>
<div class="lpo_form">
<input id="TxtItemCost"  onpaste="return CopyPasteData(event);" class="form-control num_right" autocomplete="off" onkeyup="OnKeyupItemcost(event)" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="ItmCostError" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="ReceivedQuantity" class="form-control num_right" onpaste="return CopyPasteData(event);" name="ReceivedQuantity" @maxlength = "25" autocomplete="off" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="ReceivedQtyError" class="error-message is-visible"></span>
</div>

</td>
<td>
    <div class="lpo_form">
         <input id="BinLocation" class="form-control num_left" autocomplete="off" name=""  maxlength = "25" onkeyup="" onkeypress="" type="text" onchange="" placeholder="${$("#span_BinLocation").text()}">
    </div>
</td>
<td>
<textarea id="remarks" class="form-control remarksmessage" maxlength = "100", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
</td>
<td style="display: none;"><input type="hidden" id="hfSNo" value="${RowNo}" /></td>
</tr>`);
    BindSampleRcptItmList(RowNo);
    HideSelectedItem("#ItemName_", "", "#SR_ItmDetailsTbl", "#hfSNo");
};
function BindSampleRcptItmList(ID) {
    debugger;
    BindItemList("#ItemName_", ID, "#SR_ItmDetailsTbl", "#hfSNo", "", "SR");
}

function BindSR_ItemList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();///Amount
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#hfSNo").val();
    var hdItemId = clickedrow.find("#hf_ItemID").val();
    Cmn_DeleteSubItemQtyDetail(hdItemId);
    Itm_ID = clickedrow.find("#ItemName_" + SNo + " option:selected").val();

    clickedrow.find("#hf_ItemID").val(Itm_ID);


    if (Itm_ID == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }

    ClearRowDetails(e, Itm_ID);
    debugger;
    HideSelectedItem("#ItemName_", "", "#SR_ItmDetailsTbl", "#hfSNo");
    DisableHeaderField();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleReceipt/GetItemUOM",
            data: { Itm_ID: Itm_ID },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    debugger;
                    if (arr.Table.length > 0) {
                        clickedrow.find("#UOM").val(arr.Table[0].uom_alias);
                        clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
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
            },
        });
    } catch (err) {
    }
}
function InsertSRDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckSRFormValidation() == false) {
        return false;
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
    if (CheckSR_ItemValidations() == false) {
        return false;
    }
    debugger;
    /*Modified By Nitesh */
    //if (CheckValidations_forSubItems() == false) {
    //    return false;
    //}
    debugger;
    var TransType = "";
    if (INSDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    debugger;
    var FinalItemDetail = [];
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    $("#ddlsrctype").val(sou_type);
    FinalItemDetail = GetSR_ItemDetails();
    debugger;
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
    EnableHeaderDetail();

    /*-----------Sub-item-------------*/
    /*Commented By Nitesh 04-12-2023*/

    //debugger;
    //var SubItemsListArr1 = [];
    // SubItemsListArr1 = Cmn_SubItemList();
    //var str2 = JSON.stringify(SubItemsListArr1);
    //$('#SubItemDetailsDt').val(str2);

    /*-----------Sub-item end-------------*/
    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

    return true;

};
function EnableHeaderDetail() {
    $("#EntityType").attr('disabled', false);
    $("#EntityName").attr('disabled', false);
    //$("#remarks").attr('disabled', false);
}
function DisableHeaderDetail() {

    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);


}
function CheckSRFormValidation() {
    debugger;
    var rowcount = $('#SR_ItmDetailsTbl >tbody>tr').length;
    var ValidationFlag = true;
    var ErrorFlag = "N";

    var EntityType = $('#EntityType').val();
    var EntityName = $('#EntityName').val();
    // var sou_type = $('#ddlsource_type').val();

    if (EntityType == "" || EntityType == "0") {
        $('#SpanEntityTypeErrorMsg').text($("#valueReq").text());
        $("#EntityType").css("border-color", "red");
        $("#SpanEntityTypeErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#EntityType").css("border-color", "#ced4da");
        $("#SpanEntityTypeErrorMsg").css("display", "none");
    }
    if (EntityName == "" || EntityName == "0") {
        $('#SpanEntityNameErrorMsg').text($("#valueReq").text());
        $("#EntityName").css("border-color", "red");
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "red");
        $("#SpanEntityNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#EntityName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
        $("#SpanEntityNameErrorMsg").css("display", "none");
    }
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "A") {
        if (rowcount < 1) {
            var ddlItemName = $("#ddlItemName").val();

            var src_docno = $("#src_doc_number").val();
            if (src_docno == "" || src_docno == "0" || src_docno == "---Select---") {
                $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
                $("#src_doc_number").css("border-color", "red");
                $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
                $("#SpanSourceDocNoErrorMsg").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#src_doc_number").css("border-color", "#ced4da");
                $("#SpanSourceDocNoErrorMsg").css("display", "none");
            }

            if (ddlItemName == "" || ddlItemName == "0") {
                $('#vmST_Item').text($("#valueReq").text());
                $("#ddlItemName").css("border-color", "red");
                $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
                $("#vmST_Item").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                $("#ddlItemName").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");
                $("#vmST_Item").css("display", "none");
            }
        }

    }


    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
    if (ErrorFlag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }
    if (ValidationFlag == true) {


        if (CheckSR_ItemValidations() == false) {
            return false;
        }

        if (rowcount > 0) {

            return true;

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

function CheckSR_ItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    var focus = "N";
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();

        if (currentRow.find("#ItemName_" + Sno).val() == "" || currentRow.find("#ItemName_" + Sno).val() == "0") {
            currentRow.find("#ItemNameError").text($("#valueReq").text());
            currentRow.find("#ItemNameError").css("display", "block");
            currentRow.find("[aria-labelledby='select2-ItemName_" + Sno + "-container']").css("border-color", "red");
            currentRow.find("#ItemName_" + Sno).focus();
            ErrorFlag = "Y";
            focus = "Y";

        }
        else {
            currentRow.find("#ItemNameError").css("display", "none");
            currentRow.find("#ItemName_" + Sno).css("border-color", "#ced4da");
        }

        if (currentRow.find("#TxtItemCost").val() == "" || parseFloat(currentRow.find("#TxtItemCost").val()) == parseFloat("0")) {
            currentRow.find("#ItmCostError").text($("#valueReq").text());
            currentRow.find("#ItmCostError").css("display", "block");
            currentRow.find("#TxtItemCost").css("border-color", "red");


            if (focus == "Y") {
                return false
            }
            else {
                currentRow.find("#TxtItemCost").focus();
                focus = "Y";

            }
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItmCostError").css("display", "none");
            currentRow.find("#TxtItemCost").css("border-color", "#ced4da");
        }

        if ($("#ReceivedDate").val() == "" || $("#ReceivedDate").val() == "1990-01-01") {
            currentRow.find("#spanerrorREcdate").text($("#valueReq").text());
            currentRow.find("#spanerrorREcdate").css("display", "block");
            currentRow.find("#ReceivedDate").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#spanerrorREc_date").css("display", "none");
            currentRow.find("#ReceivedDate").css("border-color", "#ced4da");
        }

        if (currentRow.find("#ReceivedQuantity").val() == "") {
            currentRow.find("#ReceivedQtyError").text($("#valueReq").text());
            currentRow.find("#ReceivedQtyError").css("display", "block");
            currentRow.find("#ReceivedQuantity").css("border-color", "red");

            ErrorFlag = "Y";

            if (focus == "Y") {
                return false
            }
            else {
                currentRow.find("#ReceivedQuantity").focus();
                focus = "Y";

            }
        }
        else {
            currentRow.find("#ReceivedQtyError").css("display", "none");
            currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        }
        //if (currentRow.find("#ReceivedQuantity").val() == "" || parseFloat(currentRow.find("#ReceivedQuantity").val()).toFixed(QtyDecDigit) == 0) {
        //    currentRow.find("#ReceivedQtyError").text($("#valueReq").text());
        //    currentRow.find("#ReceivedQtyError").css("display", "block");
        //    currentRow.find("#ReceivedQuantity").css("border-color", "red");
        //    ErrorFlag = "Y";
        //}
        //else {
        //    currentRow.find("#ReceivedQtyError").css("display", "none");
        //    currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        //}
        if (currentRow.find("#ReceivedQuantity").val() != "") {
            if (parseFloat(currentRow.find("#ReceivedQuantity").val()).toFixed(QtyDecDigit) == 0) {
                currentRow.find("#ReceivedQtyError").text($("#valueReq").text());
                currentRow.find("#ReceivedQtyError").css("display", "block");
                currentRow.find("#ReceivedQuantity").css("border-color", "red");
                ErrorFlag = "Y";

                if (focus == "Y") {
                    return false
                }
                else {
                    currentRow.find("#ReceivedQuantity").focus();
                    focus = "Y";

                }
            }
            else {
                currentRow.find("#ReceivedQtyError").css("display", "none");
                currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            }
        }

        if (currentRow.find("#wh_id_" + Sno).val() == "" || currentRow.find("#wh_id_" + Sno).val() == "0") {
            currentRow.find("#wh_Error").text($("#valueReq").text());
            currentRow.find("#wh_Error").css("display", "block");
            currentRow.find("#wh_id_" + Sno).css("border-color", "red");
            ErrorFlag = "Y";

            if (focus == "Y") {
                return false
            }
            else {
                currentRow.find("#wh_id_").focus();
                focus = "Y";

            }
        }
        else {
            currentRow.find("#wh_Error").css("display", "none");
            currentRow.find("#wh_id_" + Sno).css("border-color", "#ced4da");
        }
        var sou_type = $("#ddlsource_type" + " option:selected").val();
        if (sou_type == "A") {
            // var status = $("#hfStatus").val();
            var PendingQuantity = $("#pend_qty").val();
            var ReceivedQuantity = $("#ReceivedQuantity").val();
            debugger;
            if ($("#Cancelled").is(":checked")) {

            }
            //else if (status == "D") {
            //    var Received_Quantity = $("#hdrec_qty").val();
            //    var totalpenQuantity = (parseInt(PendingQuantity) + parseInt(Received_Quantity)).toFixed(QtyDecDigit)
            //    if (ReceivedQuantity != "") {
            //        if (parseFloat(ReceivedQuantity) > (parseFloat(totalpenQuantity).toFixed(QtyDecDigit))) {
            //            currentRow.find("#ReceivedQtyError").text($("#ExceedingQty").text());
            //            currentRow.find("#ReceivedQtyError").css("display", "block");
            //            currentRow.find("#ReceivedQuantity").css("border-color", "red");
            //            ErrorFlag = "Y";
            //        }
            //        else {
            //            currentRow.find("#ReceivedQtyError").css("display", "none");
            //            currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            //        }
            //    }

            //}
            else {
                if (ReceivedQuantity != "") {
                    if (parseFloat(ReceivedQuantity) > parseFloat(PendingQuantity)) {
                        currentRow.find("#ReceivedQtyError").text($("#ExceedingQty").text());
                        currentRow.find("#ReceivedQtyError").css("display", "block");
                        currentRow.find("#ReceivedQuantity").css("border-color", "red");
                        ErrorFlag = "Y";

                        if (focus == "Y") {
                            return false
                        }
                        else {
                            currentRow.find("#ReceivedQuantity").focus();
                            focus = "Y";

                        }
                    }
                    else {
                        currentRow.find("#ReceivedQtyError").css("display", "none");
                        currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
                    }
                }


            }


        }
    })

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function DisableTbleDetail() {
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var SNo = currentRow.find("#hfSNo").val();
        currentRow.find("#ItemName_" + SNo).attr("disabled", true);
        currentRow.find("#UOM").attr("disabled", true);
        currentRow.find("#TxtItemCost").attr("disabled", true);
        currentRow.find("#ReceivedQuantity").attr("disabled", true);
        currentRow.find("#BinLocation").attr("disabled", true);
        currentRow.find("#remarks").attr("disabled", true);
        currentRow.find("#SampleType").attr("disabled", true);
        currentRow.find("#OtherDetail").attr("disabled", true);
        currentRow.find("#ReceivedDate").attr("disabled", true);
    });

    $('#SR_ItmDetailsTbl thead th').each(function () {
        var currentRow = $(this);
        currentRow.find("#DivAddNewItemBtn").css("display", "none")

    });

}
function EnableTbleDetail() {

    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type != "A") {
        $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var SNo = currentRow.find("#hfSNo").val();
            currentRow.find("#ItemName_" + SNo).attr("disabled", false);
            currentRow.find("#UOM").attr("disabled", true);
            currentRow.find("#TxtItemCost").attr("disabled", false);
            currentRow.find("#ReceivedQuantity").attr("disabled", false);
            currentRow.find("#BinLocation").attr("disabled", false);
            //currentRow.find("#wh_id_" + SNo).attr("disabled", false);
            //currentRow.find("#BtnBatchDetail").attr("disabled", true);
            //currentRow.find("#BtnSerialDetail").attr("disabled", true);
            currentRow.find("#remarks").attr("disabled", false);
            $("#addnewrow").css("display", "block");
            //currentRow.find("#DivAddNewItemBtn").css("display", "block");
            //currentRow.find("#DivAddNewItemBtn").prop("display", "block");

            $("#DivAddNewItemBtn").css("display", "block")

            currentRow.find("#SampleType").attr("disabled", false);
            currentRow.find("#OtherDetail").attr("disabled", false);
            currentRow.find("#ReceivedDate").attr("disabled", false);
        });
        $('#SR_ItmDetailsTbl thead th').each(function () {
            var currentRow = $(this);
            currentRow.find("#DivAddNewItemBtn").css("display", "block")

        });
    }
    else {
        $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var SNo = currentRow.find("#hfSNo").val();
            currentRow.find("#ItemName_" + SNo).attr("disabled", false);
            currentRow.find("#UOM").attr("disabled", true);
            currentRow.find("#TxtItemCost").attr("disabled", false);
            currentRow.find("#ReceivedQuantity").attr("disabled", false);
            currentRow.find("#BinLocation").attr("disabled", false);
            //currentRow.find("#wh_id_" + SNo).attr("disabled", false);
            //currentRow.find("#BtnBatchDetail").attr("disabled", true);
            //currentRow.find("#BtnSerialDetail").attr("disabled", true);
            currentRow.find("#remarks").attr("disabled", false);
            $("#addnewrow").css("display", "block");


            $("#DivAddNewItemBtn").css("display", "block")

            currentRow.find("#SampleType").attr("disabled", true);
            currentRow.find("#OtherDetail").attr("disabled", true);
            currentRow.find("#ReceivedDate").attr("disabled", true);
        });
    }
}

function GetSR_ItemDetails() {
    var SRItemsDetail = [];
    $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var item_id = "";
        var uom_id = "";
        var rec_qty = "";
        var lot_id = "";
        var wh_id = "";
        var item_rate = "";
        var bin_loaction = "";
        var it_remarks = "";
        var sr_type = "";
        var other_dtl = "";
        var receive_date = "";
        var src_doc = "";
        var src_docdate = "";
        var issu_qty = "";
        var iss_date = "";
        var currentRow = $(this);

        var SNo = currentRow.find("#hfSNo").val();
        var SRNo = currentRow.find("#SpanRowId").text();
        item_id = currentRow.find("#hf_ItemID").val();
        uom_id = currentRow.find("#UOMID").val();
        rec_qty = currentRow.find("#ReceivedQuantity").val();
        //wh_id = currentRow.find("#wh_id_" + SNo).val();
        item_rate = currentRow.find("#TxtItemCost").val();
        bin_loaction = currentRow.find("#BinLocation").val();
        it_remarks = currentRow.find("#remarks").val();
        sr_type = currentRow.find("#SampleType").val();
        other_dtl = currentRow.find("#OtherDetail").val();
        receive_date = currentRow.find("#ReceivedDate").val();
        var sou_type = $("#ddlsource_type" + " option:selected").val();
        if (sou_type == "A") {
            src_doc = currentRow.find("#src_doc_").val();
            src_docdate = currentRow.find("#src_docdate_").val();
            issu_qty = currentRow.find("#issu_qty").val();
            iss_date = currentRow.find("#iss_date").val();
            SRItemsDetail.push({
                item_id: item_id, uom_id: uom_id, rec_qty: rec_qty, lot_id: lot_id, wh_id: wh_id, item_rate: item_rate, it_remarks: it_remarks, bin_loaction: bin_loaction, sr_type: sr_type
                , other_dtl: other_dtl, receive_date: receive_date, src_doc: src_doc, src_docdate: src_docdate, issu_qty: issu_qty, iss_date: iss_date
            });
        }
        else {
            //   src_doc = currentRow.find("#src_doc_" + SRNo).val("");
            //src_docdate = currentRow.find("#src_docdate_" + SRNo).val("");
            //issu_qty = currentRow.find("#issu_qty").val("");
            //iss_date = currentRow.find("#iss_date").val("");
            SRItemsDetail.push({
                item_id: item_id, uom_id: uom_id, rec_qty: rec_qty, lot_id: lot_id, wh_id: wh_id, item_rate: item_rate, it_remarks: it_remarks, bin_loaction: bin_loaction, sr_type: sr_type
                , other_dtl: other_dtl, receive_date: receive_date//, src_doc: src_doc, src_docdate: src_docdate, issu_qty: issu_qty, iss_date: iss_date
            });
        }

    });
    return SRItemsDetail;
};

function OnChangeEntityType() {
    debugger;
    var ListFilterData = $("#ListFilterData").val();
    var EntityType = $("#EntityType").val();
    if (EntityType != "0") {
        $("#EntityType").css("border-color", "#ced4da");
        $("#SpanEntityTypeErrorMsg").css("display", "none");

        $("#EntityName").val(0).trigger('change');
        BindSR_SuppCustList(EntityType);
        $("#EntityName").empty();
        $("#EntityName").append('<option value="0">---Select---</option>')
    }
    else {
        $("#SpanEntityTypeErrorMsg").text($("#valueReq").text());
        $("#EntityType").css("border-color", "red");
        $("#SpanEntityTypeErrorMsg").css("display", "block");
        BindSR_SuppCustList(EntityType);
        $("#EntityName").val(0).trigger('change');
        if (EntityName == "" || EntityName == "0") {
            $("#EntityName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
            $("#SpanEntityNameErrorMsg").css("display", "none");

        }

    }
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "D") {
        $("#EntityName").attr("Disabled", false);
    }
    $("#SpanEntityNameErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
}
function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();
    //if (EntityType != "0") {
    $("#ddl_EntityName").val(0).trigger('change');
    BindSR_SuppCustList_List(EntityType);
    //}
    //else {
    //    $("#ddl_EntityName").val(0).trigger('change');
    //}
}
function BindSR_SuppCustList(EntityType) {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    $("#EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlist").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType,
                    source_type: sou_type
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
                    Error_Page();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return {
                            id: val.ID, text: val.Name

                        };
                    })
                };
            }
        },
    });
}
function BindSR_SuppCustList_List(EntityType) {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();

    $("#ddl_EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlist").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType,
                    source_type: sou_type
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
                    Error_Page();
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
function OnChangeEntityName() {
    debugger;
    var EntityName = $("#EntityName").val();
    if (EntityName == "0") {
        $("#SpanEntityNameErrorMsg").text($("#valueReq").text());
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "red");
        $("#SpanEntityNameErrorMsg").css("display", "block");
        $("#DivAddNewItemBtn").css("display", "none");
        DisableTbleDetail();
    }
    else {
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
        $("#SpanEntityNameErrorMsg").css("display", "none");
        EnableTbleDetail();
        var sou_type = $("#ddlsource_type" + " option:selected").val();
        if (sou_type == "A") {
            // $("#ddlItemName").attr("disabled", false);
            var EntityName = $("#EntityName" + " option:selected").text();
            // var enty = EntityName.split('(',);
            var enty = EntityName.substr(EntityName.length-3);         
            var entye = enty[1].split(')');

            $("#EntityType").val(entye[0].trim());
            BindSrcDocNumberOnBehalfSrcType();
            $("#src_doc_number").attr("Disabled", false);
            $("#ItemName_1").attr("Disabled", true);
            //if ($("#ddlItemName").val() == "0") {
            //    var smple_item = $("#ddlItemName").val();
            //    $("#ddlItemName option[value='" + smple_item + "']").select2().hide();
            //}
            /* sampleitemlist();*/
        }
        else {
            //$("#delBtnIcon").attr("Disabled", false)
            $("#SampleType").attr("disabled", false);
            $("#OtherDetail").attr("Disabled", false);
            $("#ReceivedDate").attr("Disabled", false);
            //$("#DivAddNewItemBtn").css("display", "block");
            // $("#DivAddNewItemBtn").css("display", "block");
            //var currentrow = $("#SR_ItmDetailsTbl").val();
            //$("#addnewrow").css("display", "block");

        }
    }


}
function OnChangeItemCost(e) {
    var RateDecDigit = $("#RateDigit").text();
    debugger;

    var clickedrow = $(e.target).closest("tr");

    var EntityName = clickedrow.find("#TxtItemCost").val();
    if (AvoidDot(EntityName) == false) {
        clickedrow.find("#TxtItemCost").val("");
        EntityName = 0;
    }
    if (EntityName == "" || EntityName == null || parseFloat(EntityName) == parseFloat("0")) {
        clickedrow.find("#ItmCostError").text($("#valueReq").text());
        clickedrow.find("#ItmCostError").css("display", "block");
        clickedrow.find("#TxtItemCost").css("border-color", "red");
    }
    else {
        clickedrow.find("#TxtItemCost").val(parseFloat(EntityName).toFixed(RateDecDigit));
        clickedrow.find("#ItmCostError").css("display", "none");
        clickedrow.find("#TxtItemCost").css("border-color", "#ced4da");
    }
    if (EntityName == 'A-Z' || EntityName == 'a-z') {
        clickedrow.find("#TxtItemCost").val(parseFloat("0"));

    }
}
function OnChangeReceivedQty(e) {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;
    var ErrorFlag = "N";
    var clickedrow = $(e.target).closest("tr");

    var ReceivedQty = clickedrow.find("#ReceivedQuantity").val();
    if (AvoidDot(ReceivedQty) == false) {
        clickedrow.find("#ReceivedQuantity").val("");
        ReceivedQty = 0;
    }
    if (ReceivedQty == "" || ReceivedQty == null || parseFloat(ReceivedQty) == parseFloat("0")) {
        clickedrow.find("#ReceivedQtyError").text($("#valueReq").text());
        clickedrow.find("#ReceivedQtyError").css("display", "block");
        clickedrow.find("#ReceivedQuantity").css("border-color", "red");
        clickedrow.find("#ReceivedQuantity").val("");
        ErrorFlag = "Y";
    }
    else {
        clickedrow.find("#ReceivedQuantity").val(parseFloat(ReceivedQty).toFixed(QtyDecDigit));
        clickedrow.find("#ReceivedQtyError").css("display", "none");
        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");

    }
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "A") {
        var status = $("#hfStatus").val();

        var PendingQuantity = $("#pend_qty").val();
        var ReceivedQuantity = $("#ReceivedQuantity").val();
        //if (status == "D") {
        //    var Received_Quantity = $("#hdrec_qty").val();
        //    var totalpenQuantity = (parseInt(PendingQuantity) + parseInt(Received_Quantity)).toFixed(QtyDecDigit);
        //    if (parseFloat(ReceivedQuantity) > (parseFloat(totalpenQuantity).toFixed(QtyDecDigit))) {
        //        clickedrow.find("#ReceivedQtyError").text($("#ExceedingQty").text());
        //        clickedrow.find("#ReceivedQtyError").css("display", "block");
        //        clickedrow.find("#ReceivedQuantity").css("border-color", "red");
        //        ErrorFlag = "Y";
        //    }
        //    else {
        //        clickedrow.find("#ReceivedQtyError").css("display", "none");
        //        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        //    }
        //}
        //else {
        if (parseFloat(ReceivedQuantity) > parseFloat(PendingQuantity)) {
            clickedrow.find("#ReceivedQtyError").text($("#ExceedingQty").text());
            clickedrow.find("#ReceivedQtyError").css("display", "block");
            clickedrow.find("#ReceivedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            clickedrow.find("#ReceivedQtyError").css("display", "none");
            clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        }
        //}

    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnKeyupRecQty(e) {

    var currentrow = $(e.target).closest('tr');
    var RecQty = currentrow.find("#ReceivedQuantity").val();
    if (RecQty != "") {
        currentrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        currentrow.find("#ReceivedQtyError").text("");
        currentrow.find("#ReceivedQtyError").css("display", "none");
    }
}
function OnKeyupItemcost(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var itmcst = currentrow.find("#TxtItemCost").val();
    if (itmcst != "") {
        currentrow.find("#TxtItemCost").css("border-color", "#ced4da");
        currentrow.find("#ItmCostError").text("");
        currentrow.find("#ItmCostError").css("display", "none");
    }
}
function BtnSearch() {
    debugger;
    FilterSampleReceiptList();
    ResetWF_Level();
}

function FilterSampleReceiptList() {
    debugger;
    try {
        var EntityType = $("#ddl_EntityType option:selected").val();
        var EntityName = $("#ddl_EntityName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleReceipt/SearchSampleReceiptDetail",
            data: {
                EntityType: EntityType,
                EntityName: EntityName,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#SRListTbl').html(data);
                $('#ListFilterData').val(EntityType + ',' + EntityName + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hf_ItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    return true;
}
function QtyFloatValueonlyRecQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function RateFloatValueonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    return true;
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
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
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertSRDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#Cancelled", "Enable");
}
/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var SRStatus = "";
    //SRStatus = $('#hfStatus').val().trim();
    //if (SRStatus === "D" || SRStatus === "F") {

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

    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
    try {
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var smRcpDt = $("#SR_Date").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: smRcpDt
        },
        success: function (data) {
            /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var SRStatus = "";
                SRStatus = $('#hfStatus').val().trim();
                if (SRStatus === "D" || SRStatus === "F") {

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
                /*    swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
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
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SRNo = "";
    var SRDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SourceType = "";

    docid = $("#DocumentMenuId").val();
    SRNo = $("#SR_Number").val();
    SRDate = $("#SR_Date").val();
    $("#hdDoc_No").val(SRNo);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (SRNo + ',' + SRDate + ',' + "Update" + ',' + WF_status1)
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

    if (fwchkval === "Forward") {
        if (fwchkval != "" && SRNo != "" && SRDate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(SRNo, SRDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SampleReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/SampleReceipt/SRListApprove?SR_No=" + SRNo + "&SR_Date=" + SRDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SRNo != "" && SRDate != "") {
             Cmn_InsertDocument_ForwardedDetail(SRNo, SRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SampleReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && SRNo != "" && SRDate != "") {
             Cmn_InsertDocument_ForwardedDetail(SRNo, SRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/SampleReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#SR_Number").val();
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

/*-----------------ENDING---- JS Work by using MODEL instead of whole js work -------------------------- */

function ResetSR_Detail() {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "D") {
        GenrateBlankRow();
    }
    else {
        GenrateBlankRowAganinstRow();
    }
    ClearHeaderDetails();
    ClearBatch_SerialDetails();
    ///*------Attatchment------------*/
    //$("#PartialImageBind").html("");
    //$("#file-1").prop("disabled", false);
    ///*------Attatchment------------*/
    //EnableForEdit();
}
function ClearHeaderDetails() {
    debugger;
    $("#SRCreatedBy").text("");
    $("#SRCreatedDate").text("");
    $("#SRApproveBy").text("");
    $("#SRApproveDate").text("");
    $("#SRAmdedBy").text("");
    $("#SRAmdedDate").text("");
    $("#SRStatus").text("");
    $("#hfStatus").val("");

    $("#SR_Number").val("");
    var CurrentDate = moment().format('YYYY-MM-DD');
    $("#SR_Date").val(CurrentDate);
    $("#EntityType").val(0).prop('selected', true);
    $("#Cancelled").prop("checked", false);
    $('#EntityName').empty().append('<option value="0" selected="selected">---Select---</option>');
    $("#remarks").val("");
}
function GenrateBlankRow() {
    $('#SR_ItmDetailsTbl tbody tr').remove();
    $('#SR_ItmDetailsTbl tbody').append(` <tr>
<td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="${$("#Span_Delete_Title").text()}"></i></td>
<td class="sr_padding"><span id="SpanRowId">1</span><input  type="hidden" id="hfSNo" value="1" /></td>
<td class="ItmNameBreak itmStick tditemfrz"><div class=" col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="ItemName_1" onchange="BindSR_ItemList(event);" disabled></select><input  type="hidden" id="hf_ItemID" />
<span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td><td><input id="SampleType" value="" disabled maxlength = "50"  class="form-control" autocomplete="off" type="text"  onkeyup="OnKeyupsam_typ(event)" onchange="OnChangesm_type(event);" name="SampleType" placeholder="${$("#span_SampleType").text()}"></td>
<td><input id="OtherDetail" value=""  maxlength = "250" class="form-control" autocomplete="off" type="text" onkeyup="OnKeyupoth_dtl(event)" onchange="OnChangeoth_dtl(event);" name="OtherDetail"  placeholder="${$("#span_OtherDetail").text()}" disabled></td>

<td>
    <div class="lpo_form">
<input class="form-control" id="ReceivedDate" value=""   onchange="onchangerecdate()" autocomplete="off" type="date" name="ReceivedDate" disabled>
<span id="spanerrorREcdate" class="error-message is-visible" disabled></span>
</div>
</td>
<td> <div class="lpo_form"><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00" disabled > <span id="ItmCostError" class="error-message is-visible" disabled></span></div></td>
<td>
<div class="lpo_form">
<input id="ReceivedQuantity" class="form-control num_right" onpaste="return CopyPasteData(event);" name="ReceivedQuantity" @maxlength = "25" autocomplete="off" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'" disabled>
<span id="ReceivedQtyError" class="error-message is-visible"></span>
</div>
</td>
<td>
    <div class="lpo_form">
         <input id="BinLocation" class="form-control num_left" autocomplete="off" name="" maxlength = "25" onkeyup="" onkeypress="" type="text" disabled onchange="" placeholder="${$("#span_BinLocation").text()}">
    </div>
</td><td><textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "100",  placeholder="${$("#span_remarks").text()}" disabled   ></textarea></td>
</tr>`);
    /*  $('#SR_ItmDetailsTbl tbody').append(` <tr>
  <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
  <td class="sr_padding"><span id="SpanRowId">1</span><input  type="hidden" id="hfSNo" value="1" /></td>
  <td><div class=" col-sm-11" style="padding:0px;"><select class="form-control" id="ItemName_1" onchange="BindSR_ItemList(event);" ></select><input  type="hidden" id="hf_ItemID" />
  <span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div></td>
  <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
  <td><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00"  > <span id="ItmCostError" class="error-message is-visible"></span></td>
  <td><input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></td>
  <td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id_1" onchange="OnChangeWarehouse(event);"><option value="0">---Select---</option></select><span id="wh_Error" class="error-message is-visible"></span></div></td>
  <td><input id="LotNumber" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="LotNumber"  onblur="this.placeholder='LotNumber'" disabled></td>
   <td class="center"><button type="button" class="calculator" id="BtnBatchDetail" onclick="OnClickBatchDetailBtn(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
   <td class="center"><button type="button" class="calculator" id="BtnSerialDetail" onclick="OnClickSerialDetailBtn(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
  <td><textarea id="remarks" class="form-control remarksmessage" name="remarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}"    ></textarea></td>
  </tr>`);*/
    BindSampleRcptItmList(1);
    // BindWarehouseList(1);
}
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
                ErrorPage();
                return false;
            }
            if (ActionType === "Save") {
                $("#SRCreatedBy").text(response.CurrentUser);
                $("#SRCreatedDate").text(response.CurrentDT);
            }
            if (ActionType === "Approved") {
                $("#SRApproveBy").text(response.CurrentUser);
                $("#SRApproveDate").text(response.CurrentDT);
            }
            if (ActionType === "Cancelled") {
                $("#SRAmdedBy").text(response.CurrentUser);
                $("#SRAmdedDate").text(response.CurrentDT);
            }
        }
    });
}
function RemoveSession() {
    sessionStorage.removeItem("SRTransType");
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
    sessionStorage.removeItem("EditSRNo");
    sessionStorage.removeItem("EditSRDate");
    sessionStorage.removeItem("SRItemDetailSession");
    sessionStorage.removeItem("SREnableDisable");
}
function RemoveSessionNew() {
    sessionStorage.removeItem("SRTransType");
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
    sessionStorage.removeItem("SRItemDetailSession");
    sessionStorage.removeItem("SREnableDisable");
}
function ClearRowDetails(e, ItemID) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#hfSNo").val();

    clickedrow.find("#UOM").val("");
    clickedrow.find("#UOMID").val("");
    clickedrow.find("#wh_id_" + Sno).val(0).prop('selected', true);
    clickedrow.find("#hfbatchable").val("");
    clickedrow.find("#hfserialable").val("");
    clickedrow.find("#TxtItemCost").val("");
    clickedrow.find("#ReceivedQuantity").val("");
    clickedrow.find("#LotNumber").val("");
    clickedrow.find("#remarks").val("");

    if (ItemID != "" && ItemID != null) {
        ShowItemListItm(ItemID);
    }

}
function DisableHeaderField() {
    debugger;
    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);
    $("#ddlsource_type").attr('disabled', true);
}

function ShowItemListItm(ItemCode) {
    debugger;
    if (ItemCode != "0") {
        $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#hfSNo").val();
            $("#ItemName_" + Sno + " option[value=" + ItemCode + "]").removeClass("select2-hidden-accessible");
        });
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemReceivedQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#hfSNo").val();
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    var ProductNm = "";
    if (sou_type == "D") {
        ProductNm = clickdRow.find("#ItemName_" + hfsno + " option:selected").text();
        flag = flag;
    }
    else {
        var status = $("#hfStatus").val();
        if (status == "") {
            ProductNm = clickdRow.find("#ItemName_1").val();
        }
        else {
            var ProductN = clickdRow.find("#ItemName_1").text();
            ProductNm = ProductN.trim("");
        }


        flag = "SRAgnst_Issue";
    }
    var ProductId = clickdRow.find("#hf_ItemID" + hfsno).val();
    var ProductId = clickdRow.find("#hf_ItemID").val();
    var UOM = clickdRow.find("#UOM").val();
    var Doc_no = $("#SR_Number").val();
    var Doc_dt = $("#SR_Date").val();
    var QtyDecDigit = $("#QtyDigit").text();
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
        Sub_Quantity = clickdRow.find("#ReceivedQuantity").val();
    }
    else if (flag == "SRAgnst_Issue") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            List.iss_qty = row.find("#subItemissuedQty").val();

            List.pend_qty = row.find('#subItemPendQty').val();

            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#ReceivedQuantity").val();
        var src_doc_no = clickdRow.find("#src_doc_").val();
        var src_docdate = clickdRow.find("#src_docdate_").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SampleReceipt/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt, src_doc_no: src_doc_no,
            src_docdate: src_docdate
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
            var Qty = clickdRow.find('#ReceivedQuantity').val();
            $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
                var row = $(this);
                var Rowqty = row.find('#subItemQty').val();
                if (Rowqty != null && Rowqty != "" && Rowqty != "0.000") {
                    $("#SubItemTotalQty").text(Qty);
                }
                else if (Rowqty == "") {
                    row.find('#subItemQty').val("0.000");

                }
                else {
                    row.find('#subItemQty').val(parseFloat(0).toFixed(QtyDecDigit));
                }
            });

        }
    });
}
function CheckValidations_forSubItems() {
    debugger
    return Cmn_CheckValidations_forSubItems("SR_ItmDetailsTbl", "", "hf_ItemID", "ReceivedQuantity", "SubItemReceivedQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("SR_ItmDetailsTbl", "", "hf_ItemID", "ReceivedQuantity", "SubItemReceivedQty", "N");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function OnChangesourcetype() {
    debugger;
    $("#EntityType").val('0');

    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "D") {
        var EntityName = $("#EntityName").val(0)
        $("#EntityName").val(0).trigger('change');
        if (EntityName == "" || EntityName == "0") {
            $("#EntityName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
            $("#SpanEntityNameErrorMsg").css("display", "none");

        }
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
        $("#SpanEntityNameErrorMsg").css("display", "none");
        //$("#DivAddNewItemBtn").css("display", "inherit");
        $("#EntityType").attr("Disabled", false)

        $("#samplenameid").css("display", "none");
        $("#uomid").css("display", "none");
        $("#srcnumberdiv").css("display", "none");
        $("#srcdocdatediv").css("display", "none");

        $("#plusbtnhd").css("display", "none");
        $("#itemcoststar").css("display", "block");
        //GenrateBlankRow();
        $("#src_doctbl").css("display", "none");
        $("#src_docdatetbl").css("display", "none");
        $("#iss_datetbl").css("display", "none");
        $("#iss_qntytbl").css("display", "none");
        $("#pend_qtytbl").css("display", "none");
        $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            currentRow.find("#src_doctble").css("display", "none");
            currentRow.find("#src_docdatetble").css("display", "none");
            currentRow.find("#iss_datetble").css("display", "none");
            currentRow.find("#iss_qntytble").css("display", "none");
            currentRow.find("#pend_qtytble").css("display", "none");
        })
        // OnChangeEntityName();
        appendthbody();
        GenrateBlankRow();
        $("#SR_ItmDetailsTbl").removeClass('width2400')
        $("#SR_ItmDetailsTbl").addClass('width1250')



    }
    else {
        if ($("#ddlItemName").val() == "0") {
            var smple_item = $("#ddlItemName").val();
            $("#ddlItemName option[value='" + smple_item + "']").select2().hide();
        }
        sampleitemlist();
        $("#samplenameid").css("display", "block");
        // $("#DivAddNewItemBtn").css("display", "none");

        $("#uomid").css("display", "block");
        $("#srcnumberdiv").css("display", "block");
        $("#srcdocdatediv").css("display", "block");
        $("#plusbtnhd").css("display", "block");
        // $("#ddlItemName").attr("Disabled", true);
        $("#src_doc_number").attr("Disabled", true)
        // $("#ddlItemName").attr("Disabled", true)
        $("#EntityType").attr("Disabled", true)
        $("#EntityName").attr("Disabled", true)
        $("#itemcoststar").css("display", "none");
        $("#SR_ItmDetailsTbl").removeClass('width1250')
        $("#SR_ItmDetailsTbl").addClass('width2400')
        appendthbody();
        GenrateBlankRowAganinstRow();
        // OnChangeEntityName();
        //ResetSR_Detail();
        $('#SR_ItmDetailsTbl thead th').each(function () {
            var currentRow = $(this);
            currentRow.find("#DivAddNewItemBtn").css("display", "none")

        });
    }
    $("#EntityName").val(0).trigger('change');
    if (EntityName == "" || EntityName == "0") {
        $("#EntityName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
        $("#SpanEntityNameErrorMsg").css("display", "none");

    }
    $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
    $("#SpanEntityNameErrorMsg").css("display", "none");

}

function appendthbody() {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();
    if (sou_type == "A") {
        $('#SR_ItmDetailsTbl thead th').remove();
        $('#SR_ItmDetailsTbl thead').append(` <tr>

                                                              <th width="1%"> &nbsp;</th>
                                                            <th width="2%">${$("#span_srno").text()}</th>
                                                            <th width="11%" class="itmStick">
                                                                <div class="col-md-2 col-sm-12"> </div>
                                                                <div class="col-md-8 col-sm-12">
                                                                    <span class="item_add">
                                                                     ${$("#span_SampleName").text()}
                                                                    </span>
                                                                   
                                                                      <a href="#">
                                                                               <div class="plus_icon1" style="display: none;" id="DivAddNewItemBtn"> <i class="fa fa-plus" id="BtnAddItem" onclick="AddNewRow()"  ${$("#span_AddNew").text()} title="${$("#span_AddNew").text()}"></i> </div>
                                                                            </a>
                                                                </div>
                                                                <div class="col-md-2 col-sm-12"> </div>

                                                            </th>
                                                            <th width="4%"> ${$("#ItemUOM").text()}</th>
                                                            <th id="src_doctbl" width="8%">${$("#span_SourceDocumentNumber").text()}</th>
                                            <th id="src_docdatetbl" >${$("#span_SourceDocumentDate").text()}</th>
                                            <th> ${$("#span_SampleType").text()}</th>
                                            <th>${$("#span_OtherDetail").text()}</th>
                                            <th id="iss_datetbl"> ${$("#span_issuedDate").text()} </th>
                                            <th width="5%" id="iss_qntytbl">${$("#span_issuedQuantity").text()}</th>
                                            <th width="5%" id="pend_qtytbl">${$("#span_pendingquantity").text()}</th>

                                            <th> ${$("#span_ReceviedDate").text()}<span class="required">*</span></th>
                                            <th width="8%">${$("#span_itemcost").text()}</th>
                                            <th width="8%">${$("#span_ReceivedQuantity").text()}<span class="required">*</span>  </th>
                                            <th width="8%">${$("#span_BinLocation").text()} </th>
                                            <th width="10%">${$("#span_remarks").text()}</th>                                      
                         </tr>`);
    }
    else {
        $('#SR_ItmDetailsTbl thead th').remove();
        $('#SR_ItmDetailsTbl thead').append(` <tr>

                                                              <th width="1%"> &nbsp;</th>
                                                            <th width="2%">${$("#span_srno").text()}</th>
                                                            <th width="18%" class="itmStick">
                                                                <div class="col-md-2 col-sm-12"> </div>
                                                                <div class="col-md-8 col-sm-12">
                                                                    <span class="item_add">
                                                                     ${$("#span_SampleName").text()}
                                                                    </span>
                                                                   
                                                                      <a href="#">
                                                                               <div class="plus_icon1" style="display: none;" id="DivAddNewItemBtn"> <i class="fa fa-plus" id="BtnAddItem" onclick="AddNewRow()"  ${$("#span_AddNew").text()} title="${$("#span_AddNew").text()}"></i> </div>
                                                                            </a>
                                                                </div>
                                                                <div class="col-md-2 col-sm-12"> </div>

                                                            </th>
                                                            <th width="4%"> ${$("#ItemUOM").text()}</th>                                                       
                                            <th  width="10%"> ${$("#span_SampleType").text()}</th>
                                            <th  width="10%">${$("#span_OtherDetail").text()}</th>
                                            <th  width="10%"> ${$("#span_ReceviedDate").text()}<span class="required">*</span></th>
                                            <th width="10%">${$("#span_itemcost").text()} <span id="itemcoststar" class="required">*</span></th>
                                            <th width="10%">${$("#span_ReceivedQuantity").text()}<span class="required">*</span>  </th>
                                            <th width="10%">${$("#span_BinLocation").text()} </th>
                                            <th width="20%">${$("#span_remarks").text()}</th>                                      
                         </tr>`);
    }
}
function GenrateBlankRowAganinstRow() {
    debugger;
    $('#SR_ItmDetailsTbl tbody tr').remove();
}

function sampleitemlist() {
    debugger;
    $("#src_doc_number").attr("Disabled", true);
    //var EntityName = $("#EntityName").val();
    //var entity_type = $("#EntityType").val();
    //var ItmName = $("#ddlItemName").val();
    var src_type = $("#ddlsource_type").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/SampleReceipt/GetST_ItemList",
            data: {
                //EntityName: EntityName,
                //entity_type: entity_type,
                src_type: src_type
            },
            // dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                debugger;
                if (data == null || data == "") {
                    $('#ddlItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                    $('#Textddl').append(`<option data-uom="0" value="0">---Select---</option>`);
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);

                    if (arr.Table.length > 0) {
                        $("#ddlItemName optgroup option").remove();
                        $("#ddlItemName optgroup").remove();
                        $('#Textddl').append(`<option data-uom="0" value="0">---Select---</option>`);
                        $('#ddlItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        debugger;
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlItemName').select2({
                            templateResult: function (data) {
                                var selected = $('#ddlItemName').val();
                                var ItmDDLName = $('#ddlItemName').val();
                                var SnoHiddenField = $('#SNohiddenfiled').val();
                                var TableID = $('#SR_ItmDetailsTbl').val();
                                if (check(data, selected, TableID, SnoHiddenField, ItmDDLName) == true) {
                                    var UOM = $(data.element).data('uom');
                                    var classAttr = $(data.element).attr('class');
                                    var hasClass = typeof classAttr != 'undefined';
                                    classAttr = hasClass ? ' ' + classAttr : '';
                                    var $result = $(
                                        '<div class="row">' +
                                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                                        '</div>'
                                    );
                                    return $result;
                                }
                                firstEmptySelect = false;
                            }
                        });
                        debugger;
                    }
                }
            },
        });
}
function OnChangeItemName(event) {
    debugger;
    //var EntityType = "";
    $("#ddlItemName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");
    $("#vmST_Item").css("display", "none");
    var ItemName = $("#ddlItemName").val();
    if (ItemName == "0") {
        $("#src_doc_number").attr("Disabled", true);
    }
    else {
        $("#EntityName").attr("Disabled", false);
        //$("#src_doc_number").attr("Disabled", false);
        $("#ddlsource_type").attr("Disabled", true);
        $("#EntityType").attr("Disabled", true);


    }
    if ($("#ddlItemName").val() == "0") {
        $("#vmST_Item").css("display", "none");
        $("#ddlItemName").css("border-color", "#ced4da");
    }
    BindSR_uom();
    // BindSrcDocNumberOnBehalfSrcType();
    debugger;
    $("#src_doc_number").css("border-color", "#ced4da");
    $("#SpanSourceDocNoErrorMsg").css("display", "none");
    BindSR_SuppCustList(ItemName);
}

function BindSR_uom() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();///Amount

    var Itm_ID;
    Itm_ID = $("#ddlItemName option:selected").val();

    $("#hf_ItemID").val(Itm_ID);


    //if (Itm_ID == "0") {
    //    $("#ItemNameError").text($("#valueReq").text());
    //    $("#ItemNameError").css("display", "block");
    //    $(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    //}
    //else {
    //    $("#ItemNameError").css("display", "none");
    //    $(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    //}

    //ClearRowDetails(e, Itm_ID);
    debugger;
    HideSelectedItem(Itm_ID, "#SR_ItmDetailsTbl", " ", "SNohiddenfiled");
    //DisableHeaderField();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleReceipt/GetItemUOM",
            data: { Itm_ID: Itm_ID },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    debugger;
                    if (arr.Table.length > 0) {
                        $('#uom_id').append(`<option data-uom="0" value="0">---Select---</option>`);
                        $("#uom_id").val(arr.Table[0].uom_alias);
                        $("#uomid").val(arr.Table[0].uom_id);
                    }
                    else {
                        //clickedrow.find("#uom_id").val("");
                        $("#uom_id").val("");

                    }
                }
            },
        });
    } catch (err) {
    }
}


function OnchangeSrcDocNumber() {
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    //$("#src_doc_number").css("border-color", "#ced4da");
    //$("#SpanSourceDocNoErrorMsg").css("display", "none");
    debugger
    $("#plusbtnhd").css("display", "block");
    if (doc_no != 0) {
        var doc_Date = $("#src_doc_number option:selected")[0].dataset.date
        var newdate = doc_Date.split("-").reverse().join("-");

        $("#src_doc_date").val(newdate);
        $("#src_doc_number").val(doc_no);


    }
    if (doc_no == "" || doc_no == "0" || doc_no == "---Select---") {
        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
        $("#src_doc_number").css("border-color", "red");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
        $("#src_doc_number").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
    }
}
function addbtnnewdata() {
    debugger;
    var sample_name = $("#ddlItemName").val();
    var src_docno = $("#src_doc_number").val();
    var ErrorFlag = "N";
    if (sample_name == "" || sample_name == "0" || sample_name == "---Select---") {
        $('#vmST_Item').text($("#valueReq").text());
        $("#ddlItemName").css("border-color", "red");
        $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
        $("#vmST_Item").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#ddlItemName").css("border-color", "#ced4da");
        $("#vmST_Item").css("display", "none");
    }
    if (src_docno == "" || src_docno == "0" || src_docno == "---Select---") {
        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
        $("#src_doc_number").css("border-color", "red");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red");
        $("#SpanSourceDocNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#src_doc_number").css("border-color", "#ced4da");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        AddItemDetail();
        return true;

    }

}
function BindSrcDocNumberOnBehalfSrcType() {
    debugger;
    var SuppID = $("#EntityName").val();
    var sr_number = $("#SR_Number").val();
    var entity_type = $('#EntityType option:selected').val();
    var Itm_ID = $("#ddlItemName option:selected").val();
    if (Itm_ID != null && Itm_ID != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/SampleReceipt/GetSourceDocList',
            data: { SuppID: SuppID, Itm_ID: Itm_ID, entity_type: entity_type, sr_number: sr_number },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                if (data !== null && data !== "" && data !== "{}") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        $("#src_doc_number option").remove();
                        $("#src_doc_number optgroup").remove();
                        $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl1" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl1').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#src_doc_number').select2({
                            templateResult: function (data) {
                                debugger;
                                var list = [];
                                $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
                                    debugger;
                                    var currentRow = $(this);
                                    var Suppid = "";
                                    var rowin = currentRow.find('#SpanRowId').text();
                                    Suppid = currentRow.find('#src_doc_' + rowin).val();
                                    list.push(Suppid);
                                });
                                if (list.includes(data.id) == false) {
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
                                }


                                firstEmptySelect = false;
                            }
                        });

                        $("#src_doc_date").val("");

                    }
                }
            }

        })
    }
}
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount

    //$("#HdMrsDate").val($('#ddlMRS_No').val());

    debugger;

    var sample_name = $('#ddlItemName option:selected').val();
    var srcdocno = $('#src_doc_number option:selected').val();
    var entity_type = $('#EntityType option:selected').val();
    var srcdocdt = $('#src_doc_date').val();
    var SR_Number = $('#SR_Number').val();
    var EntityName = $('#EntityName').val();
    $.ajax(
        {
            type: "Post",
            url: "/ApplicationLayer/SampleReceipt/getMaterialIssuedata",
            data: {
                sample_name: sample_name,
                srcdocno: srcdocno,
                srcdocdt: srcdocdt,
                entity_type: entity_type,
                EntityName: EntityName,
                SR_Number: SR_Number
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "") {
                    var arr = [];
                    var rowIdx = 0;
                    $("#plus_icon1").css('display', 'none');
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        debugger;
                        //if ($('#SR_ItmDetailsTbl >tbody>tr').length > 0) {
                        //    var len = $('#SR_ItmDetailsTbl >tbody>tr').length
                        //    rowIdx = len++
                        //}
                        for (var i = 0; i < arr.Table.length; i++) {
                            debugger;
                            var Disabled = "N"
                            var dsbl_subItem = "";
                            if (arr.Table[i].sub_item == "Y") {
                                //$("#SubItemReceivedQty").attr("Disabled", false)
                                Disabled = "Y"
                                dsbl_subItem = "";
                            }
                            else {
                                //$("#SubItemReceivedQty").attr("Disabled", true)
                                Disabled = "N"
                                dsbl_subItem = "disabled"
                            }
                            var today_date = moment().format("YYYY-MM-DD");
                            // var today_date = formatDateYYYYMMDD(new Date());
                            // var Rec_date = formatDateYYYYMMDD('');
                            var issued_Dt = (arr.Table[i].issued_date)
                            //<td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$(" #Span_Delete_Title").text()}"></i></td >
                       /*   currentRow.find("#pend_qty").val(parseFloat(arr.Table1[DataSno].ship_qty).toFixed(QtyDecDigit))*/;
                            //$('#SR_ItmDetailsTbl tbody tr').remove();
                            $('#SR_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td width="1%"> &nbsp;</td>
<td class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="hfSNo" value="${rowIdx}"</td>
<td class="ItmNameBreak itmStick tditemfrz">
 <div class=" col-sm-11" style="padding:0px;">
  <input  id="ItemName_${rowIdx}" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder='${$("#ItemName").text()}'  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                                                    <input type="hidden" id="hf_ItemID" value="${arr.Table[i].item_id}" style="display: none;" />

</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
</td>
<td>
<input id="UOM" value='${arr.Table[i].uom_name}'  class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly">
<input id="UOMID" value='${arr.Table[i].uom_id}'  type="hidden" />
</td>
<td ><input  placeholder="${$("#span_SourceDocumentNumber").text()}"  value='${arr.Table[i].srcdocno}' class="form-control" disabled="disabled" id="src_doc_" maxlength="20" name="MaterialIssueNo"  readonly="readonly" type="text"></td>
 <td ><input class="form-control" id="src_docdate_" name="" readonly="readonly" type="date" value='${arr.Table[i].src_docdate}' placeholder="${$("#span_SourceDocumentDate").text()}"></td>
<td><input id="SampleType"  value='${arr.Table[i].sr_type}' disabled="disabled" maxlength = "50" class="form-control" autocomplete="off" type="text"  onkeyup="OnKeyupsam_typ(event)" onchange="OnChangesm_type(event);" name="SampleType" placeholder="${$("#span_SampleType").text()}"></td>
<td><input id="OtherDetail" value='${arr.Table[i].other_dtl}' disabled="disabled"  maxlength = "250" class="form-control" autocomplete="off" type="text" onkeyup="OnKeyupoth_dtl(event)" onchange="OnChangeoth_dtl(event);" name="OtherDetail"  placeholder="${$("#span_OtherDetail").text()}"></td>

 <td id="iss_datetble"><input class="form-control" id="iss_date" name="" readonly="readonly" type="date" value='${arr.Table[i].issued_date}' placeholder="${$("#span_issuedDate").text()}"></td>
<td id="iss_qntytble"><div class="lpo_form"><input id="issu_qty" value='${arr.Table[i].issue_qty}' class="form-control num_right" autocomplete="off" onkeyup="" onkeypress="" onchange="" type="text" name="" placeholder="0000.00" disabled="">
</div></td> <td id="pend_qtytble"> <div class="lpo_form"><input id="pend_qty" class="form-control num_right" value='${arr.Table[i].pend_qty}' autocomplete="off" onkeyup="" onkeypress="" onchange="" type="text" name="" placeholder="0000.00" disabled="">
 </div></td>
<td>
<div class="lpo_form">
<input id="ReceivedDate"  class="form-control" value=""  autocomplete="off"   min="${issued_Dt}" max="${today_date}" onchange="onchangerecdate()"   type="date" name="ReceivedDate">
<span id="spanerrorREcdate" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="TxtItemCost" onpaste="return CopyPasteData(event);" value='${arr.Table[i].cost_pr}' disabled="disabled" class="form-control num_right" autocomplete="off" onkeyup="OnKeyupItemcost(event)" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="ReceivedQuantity"  onpaste="return CopyPasteData(event);" class="form-control num_right" name="ReceivedQuantity" @maxlength = "25" autocomplete="off" onkeyup="OnKeyupRecQty(event)" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="ReceivedQtyError" class="error-message is-visible"></span>
</div>

</td>
<td>
    <div class="lpo_form">
         <input id="BinLocation" value='${arr.Table[i].bin_loc}' class="form-control num_left" maxlength = "25" placeholder="${$("#span_BinLocation").text()}" autocomplete="off" name="" onkeyup="" onkeypress="" type="text" onchange="" >
    </div>
</td>
<td>
<textarea id="remarks" class="form-control remarksmessage" value=$("#remarks") maxlength = "100", name="remarks"  placeholder="${$("#span_remarks").text()}" ></textarea>
</td>
</tr>`);
                            debugger;
                            /* Commented By Nitesh 04-12-2023 for not Subitm in Sample Recipt After Recive Qty*/
                            /*   for (var i = 0; i < arr.Table1.length; i++) {
                                   $("#hdn_Sub_ItemDetailTbl tbody").append(
                                       `<tr>
                                                   <td><input type="text" id="ItemId" value='${arr.Table1[i].item_id}'></td>
                                                   <td><input type="text" id="subItemId" value='${arr.Table1[i].sub_item_id}'></td>
                                                   <td><input type="text" id="subItemissuedQty" value='${arr.Table1[i].iss_qty}'></td>
                                                   <td><input type="text" id="subItemPendQty" value='${arr.Table1[i].pend_qty}'></td>
                                                   <td><input type="text" id="subItemQty" value='${parseFloat(0).toFixed(QtyDecDigit)}'></td>
                                               </tr>`);
                               } */

                            headervaluenull();

                        }
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    /*Commented by Nitesh 04-12-2023 for no sub_item  in Sample Reciept After Recieve Qty*/
    /*<div class="col-sm-2 i_Icon no-padding" id="div_SubItemReceivedQty">
<input hidden type="text" id="sub_item" value="${Disabled}" />
<button type="button" id="SubItemReceivedQty"   ${dsbl_subItem} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>*/
}
function headervaluenull() {
    debugger;

    var src_no = $("#src_doc_number").val();
    var sample_name = $("#ddlItemName").val();
    if ($("#ddlItemName").val() == "0") {
        currentRow.find("#vmST_Item").css("display", "none");
        currentRow.find("#ddlItemName").css("border-color", "#ced4da");
    }

    $('#SR_ItmDetailsTbl >tbody >tr').each(function () {
        var currentRow = $(this);
        var date = "yyyy-MM-dd"
        currentRow.find("#ReceivedDate").val(date);

    });

    // SerialNoAfterDelete();
    // $("#src_doc_date").val("");
    // $("#uom_id").val("");
    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);
    $("#ddlsource_type").attr('disabled', true);
    $("#src_doc_number").attr('disabled', true);

    $("#ddlItemName").attr('disabled', true);
    //$("#select2-src_doc_number-container").val("0");
    // $("#src_doc_number").val("---Select---").trigger('change');
    //$("#src_doc_number").val("---Select---");
    //if ($("#ddlItemName").val() == "---Select---") {
    //    $("#SpanSourceDocNoErrorMsg").css("display", "none");
    //    $("#src_doc_number").css("border-color", "#ced4da");
    //}

    //$("#ddlItemName").val("0").trigger('change');
    $("#ddlsource_type").attr('disabled', true);
    $("#plusbtnhd").css('display', "none");


}
function onchangerecdate() {
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();
        if ($("#ReceivedDate").val() == "" || $("#ReceivedDate").val() == "1990-01-01") {
            currentRow.find("#spanerrorREcdate").text($("#valueReq").text());
            currentRow.find("#spanerrorREcdate").css("display", "block");
            currentRow.find("#ReceivedDate").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#spanerrorREcdate").css("display", "none");
            currentRow.find("#ReceivedDate").css("border-color", "#ced4da");
        }
    })
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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "SR_ItmDetailsTbl", [{ "FieldId": "ItemName_", "FieldType": "select", "SrNo": "hfSNo" }]);
}
function onchangeCancelledRemarks() {
    debugger;

    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}

function UploadExportExcel() {

    $('#export').click(function () {
        debugger;
        var EntityType = $("#ddl_EntityType option:selected").val();
        var EntityName = $("#ddl_EntityName option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
      

        var searchValue = $("#SampleTrackingTbl_filter input[type=search]").val();
        window.location.href = "/ApplicationLayer/SampleReceipt/ExtcelDownload?searchValue=" + searchValue + "&EntityType=" + EntityType
            + "&EntityName=" + EntityName + "&Fromdate=" + Fromdate + "&Todate=" + Todate; + "&Status=" + Status

    });
}