
$(document).ready(function () {
    $("#ddlRequiredArea").select2();
    debugger;
    var Doc_no = $("#PRNumber").val();
    $("#hdDoc_No").val(Doc_no);

    var RequisitionTypeList1 = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList1 == "S") {
        $("#Div_AddBtnReOrderLevel").css("display", "block")
        $("#Div_AddBtnItem").css("display", "none")
    }
    else {
        $("#Div_AddBtnReOrderLevel").css("display", "none")
        $("#Div_AddBtnItem").css("display", "block")
    }
    $("#PRListTbl #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var PRData = $("#PRData").val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var PRId = clickedrow.children("#pr_no").text();
            var PRDate = clickedrow.children("#prdateID").text();
            if (PRId != null && PRId != "") {
                window.location.href = "/ApplicationLayer/PurchaseRequisition/EditPR/?PRId=" + PRId + "&PRDate=" + PRDate + "&PRData=" + PRData + "&WF_status=" + WF_status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var PR_No = clickedrow.children("#pr_no").text();
        var PR_Date = clickedrow.children("#prdateID").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(PR_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(PR_No, PR_Date, Doc_id, Doc_Status);
    });
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#txtPRDate").val() == "0001-01-01") {
        $("#txtPRDate").val(CurrentDate);
    }
    $("#txtTodate").val(CurrentDate);
   
    BindDLLItemList();

    $('#PRItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
     
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
        // Removing the current row. 
        $(this).closest('tr').remove();
        debugger;        
        var ItemCode = $(this).closest('tr')[0].cells[2].children[0].children[0].value;
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        //ShowItemListItm(ItemCode);
        SerialNoAfterDelete();
       
    });
})
function OnClickOrderTrackingBtn(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var PRNo = currentrow.find("#pr_no").text();
    var PRDate = currentrow.find("#prdateID").text();
    var req_by = currentrow.find("#reqArea").text();
    if (PRNo != null && PRNo != "" && PRDate != null && PRDate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PurchaseRequisition/GetPRTrackingDetail",
            data: { PR_No: PRNo, PR_Date: PRDate },
            //dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
                $("#trackingpo").html(data);
                cmn_apply_datatable("#Table_POTracking");
                $("#RequisitionNumber").val(PRNo);
                $("#RequisitionDate").val(PRDate);
                $("#RequestedBy").val(req_by);
            }
        });
    }

}
//function onclickBackToList() {
//    debugger;
//    try {
//        var PRData = $("#PRData1").val();
//        if (PRData != null && PRData != "") {
//            window.location.href = "/ApplicationLayer/PurchaseRequisition/PurchaseRiquisitionSave/?PRData=" + PRData;
//        }
//    }
//    catch (err) {
//        debugger
//    }
//}
function OnClickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertPRDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

function BtnSearch() {
    debugger;
    FilterPRList();
    ResetWF_Level();
}
function FilterPRList() {
    debugger;
    try {
        var ReqArea = $("#ddlRequiredArea").val();

        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate1").val();
        var Status = $("#ddlStatus").val();
        //$("#txtTodate").val(Todate);
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PurchaseRequisition/SearchPRDetail",
            data: {
                req_area: ReqArea,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;                
                $('#PRListTbl').html(data); 
                $('#PRData').val(ReqArea + ',' + Fromdate + ',' + Todate + ',' + Status);
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
    var ToDate = $("#txtTodate1").val();
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
            $("#txtTodate1").val(today);

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
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#PRItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });

};
//function EditBtnClick() {
//    debugger
//    var Status = "";
//    Status = $('#StatusCode').val();
//    if (Status === "A") {
//        debugger
//        var PRNumber = $("#PRNumber").val();
//        var txtPRDate = $("#txtPRDate").val();
//        $.ajax({
//            type: "POST",
//            url: "/ApplicationLayer/PurchaseRequisition/CheckRFQAgainstPR",
//            dataType: "json",
//            data: { DocNo: PRNumber, DocDate: txtPRDate },
//            success: function (data) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    //LSO_ErrorPage();
//                    return false;
//                }

//                if (data !== null && data !== "") {
//                    var arr = [];
//                    arr = JSON.parse(data);
//                    if (arr.Table.length > 0) {

//                        swal("", $("#RFQHasBnPrcsdCanNotBeMdfd").text(), "warning");
//                        $("#hdnForEdit").attr("name", "");
//                        return false;
//                    }
//                    else {
//                        $("#hdnForEdit").attr("name", "Command");
//                        $('form').submit();
//                    }
//                }
//                else {
//                    $("#hdnForEdit").attr("name", "Command");
//                    $('form').submit();
//                }
//            },
//            error: function (Data) {

//            }
//        });
//    }
//    else {
//        $("#hdnForEdit").attr("name", "Command");
//        $('form').submit();
//    }
//}
function InsertPRDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckPRFormValidation() == false) {
        return false;
    }
    if (CheckPRItemValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    debugger;
    
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

    FinalItemDetail = InsertPRItemDetails();


    debugger;
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);

    /*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    $("#ddlRequisitionTypeList").attr("disabled", false);
    /*-----------Sub-item end-------------*/
    return true;
   
};
function OnChangeRequisitionQuantity(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();

    var ReqQty = "#RequisitionQuantity" + Sno;
    var ReqQtyError = "#RequisitionQuantity_Error" + Sno;
    try {
        debugger;
        var RequisitionQuantity = clickedrow.find(ReqQty).val();
        if (RequisitionQuantity == "" && RequisitionQuantity == null) {

            clickedrow.find(ReqQtyError).text($("#PackedQtyBalanceQty").text());
            clickedrow.find(ReqQtyError).css("display", "block");
            clickedrow.find(ReqQty).css("border-color", "red");

        }
        else {
            clickedrow.find(ReqQtyError).css("display", "none");
            clickedrow.find(ReqQtyError).css("border-color", "#ced4da");
            var test = parseFloat(parseFloat(RequisitionQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find(ReqQty).val(test);
        }


    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function onChangeRequiredArea() {
    debugger;
    var RequisitionArea = $('#ddlRequiredArea').val();
    if (RequisitionArea != "0") {
        $("#SpanRequiredArea").css("display", "none");
        $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "#ced4da");
        //document.getElementById("vmRequiredArea").innerHTML = null;
        //$("#ddlRequiredArea").css('border-color', '#ced4da');
    }

}
function onChangeddlRequisitionTypeList() {
    debugger;
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList != "0") {

        //if (RequisitionTypeList == "E") {
        //    $("#div_IssueTo").css('display', 'block');
        //}
        //if (RequisitionTypeList == "I") {
        //    $("#div_IssueTo").css('display', 'none');
        //}
        if (RequisitionTypeList == "S") {
            $("#Div_AddBtnReOrderLevel").css("display", "block")
            $("#Div_AddBtnItem").css("display", "none")
            $("#PRItmDetailsTbl TBODY TR").remove();
        }
        else {
            $("#Div_AddBtnReOrderLevel").css("display", "none")
            $("#Div_AddBtnItem").css("display", "block");
            AddNewRow();
        }
        document.getElementById("vmRequisitionTypeList").innerHTML = null;
        $("#ddlRequisitionTypeList").css("border-color", "#ced4da");

    }
    else {
        $("#Div_AddBtnReOrderLevel").css("display", "none")
        $("#Div_AddBtnItem").css("display", "block")
        //$("#div_IssueTo").css('display', 'none');
    }

}
function CheckPRFormValidation() {

    debugger;
    var rowcount = $('#PRItmDetailsTbl tr').length;
    var ValidationFlag = true;
    var Flag = 'N';

    var RequirementArea = $('#ddlRequiredArea').val();
    if (RequirementArea == "" || RequirementArea == "0") {
        $('#SpanRequiredArea').text($("#valueReq").text());
        $("#SpanRequiredArea").css("display", "block");
        $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "red");
        //document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        //$("#ddlRequiredArea").css("border-color", "red");
        Flag = 'Y';
    }
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList != "0") {
        document.getElementById("vmRequisitionTypeList").innerHTML = null;
        $("#ddlRequisitionTypeList").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmRequisitionTypeList").innerHTML = $("#valueReq").text();
        $("#ddlRequisitionTypeList").css("border-color", "red");
        $("#div_IssueTo").css('display', 'none');
        Flag = 'Y';
    }

    var MRSType = $("#ddlRequisitionTypeList").val();
    if (MRSType == "E") {
        var ddlEntityType = $("#EntityName").val();
        if (ddlEntityType == "" || ddlEntityType == "0") {
            $("#vmEntity").text($("#valueReq").text());
            $("#vmEntity").css("display", "block");            
            $("EntityName").css("border-color", "red");

            Flag = 'Y';
        }
        else {
            $("#vmEntity").css("display", "none");            
            $("EntityName").css("border-color", "#ced4da");
            $("#vmEntity").text("");
        }
    }

    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }

    if (ValidationFlag == true) {

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/

        if (CheckPRItemValidations() == false) {
            return false;
        }

        if (rowcount > 1) {

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
function InsertPRItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#PRItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Itemname_" + rowid).val();
        var UOM_ID  = currentRow.find("#UOMID").val();
        var Itemtype = currentRow.find("#IDItemtype").val();
        if (Itemtype == "Service") {
            if (UOM_ID != null && UOM_ID != "" && UOM_ID != "0" && UOM_ID != "NaN") {
                ItemList.UOMID = UOM_ID;
            }
            else {
                ItemList.UOMID = "0";
            }
          
        }
        else {
            ItemList.UOMID = UOM_ID;
        }

       
        ItemList.RequQty = currentRow.find("#RequisitionQuantity").val();
        if (currentRow.find("#ItemOrderedQuantity").val() == "") {
            ItemList.OrederQty = "0";
        }
        else {
            ItemList.OrederQty = currentRow.find("#ItemOrderedQuantity").val();
        }
       
        ItemList.ItemRemarks = currentRow.find('#ItemRemarks').val();
        ItemList.min_stk_lvl = currentRow.find('#IDmin_stk_lvl').val();

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};
function CheckPRItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    $("#PRItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohf").val();

        if (currentRow.find("#Itemname_" + Sno).val() == "0") {
            currentRow.find("#ItemNameError").text($("#valueReq").text());
            currentRow.find("#ItemNameError").css("display", "block");
            debugger;
           
            currentRow.find("[aria-labelledby='select2-Itemname_"+Sno+"-container']").css("border", "1px solid red");

            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemNameError").css("display", "none");
            currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
        }

        if (currentRow.find("#RequisitionQuantity").val() == "") {
            currentRow.find("#RequisitionQuantity_Error").text($("#valueReq").text());
            currentRow.find("#RequisitionQuantity_Error").css("display", "block");
            currentRow.find("#RequisitionQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#RequisitionQuantity_Error").css("display", "none");
            currentRow.find("#RequisitionQuantity").css("border-color", "#ced4da");
        }
        if (currentRow.find("#RequisitionQuantity").val() != "") {
            if (parseFloat(currentRow.find("#RequisitionQuantity").val()).toFixed(QtyDecDigit) == 0) {
                currentRow.find("#RequisitionQuantity_Error").text($("#valueReq").text());
                currentRow.find("#RequisitionQuantity_Error").css("display", "block");
                currentRow.find("#RequisitionQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#RequisitionQuantity_Error").css("display", "none");
                currentRow.find("#RequisitionQuantity").css("border-color", "#ced4da");
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
function BindDLLItemList() {
    debugger;

    BindItemList("#Itemname_", "1", "#PRItmDetailsTbl", "#SNohf", "BindData", "PR");

}
function BindData() {
    debugger;

    var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    if (PLItemListData != null) {
        if (PLItemListData.length > 0) {
            $("#PRItmDetailsTbl >tbody >tr").each(function () {
               
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#PRItmDetailsTbl >tbody >tr").length) {
                    return false;
                }
                $("#Itemname_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}' data-value='${$("#ItemType").text() }'></optgroup>`);
                for (var i = 0; i < PLItemListData.length; i++) {
                    $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" data-value="${PLItemListData[i].ItemType}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Itemname_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Itemname_" + rowid).val();
                        if (check(data, selected, "#PRItmDetailsTbl", "#SNohf", "#Itemname_") == true) {
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
            });
        }
    }
    /*Commented By Hina after checking by Prem sir on 10-10-2023 due to sub item hidden table deleted on when it's onchange in working*/
    //$("#PRItmDetailsTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var ItemID = '#Itemname_' + rowid;
    //    if (ItmID != '0' && ItmID != "") {
    //        currentRow.find("#Itemname_" + rowid).val(ItmID).trigger('change.select2');
    //    }

    //});

}
function AddNewRow() {
    var rowIdx = 0;
    debugger;
    
    var rowCount = $('#PRItmDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;
    
    $("#PRItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
      
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
       
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#PRItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td><div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"></select>
<input  type="hidden" id="hfItemID" value="" />
<span id="ItemNameError" class="error-message is-visible"></span></div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
</button></div></td>
<td>
 <select id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" onchange="onChangeUom(event)">
                                                                      <option value="0">---Select---</option>
                                                                      </select>
<input id="UOMID" type="hidden" />
 <input id="hdnItemtype" type="hidden" />
</td>
 <td>
   <input id="IDItemtype" value='' class="form-control date num_right" autocomplete="off" type="text" name="ItemType" placeholder="${$("#ItemType").text()}" onblur="this.placeholder=${$("#ItemType").text()}" readonly>
</td>
 <td>
        <input id="IDmin_stk_lvl"  class="form-control date num_right"  type="text" autocomplete="off" placeholder="0000.00" readonly>
    </td>
<td>
<div class="col-sm-9 lpo_form no-padding ">
<input id="AvailableStockInBase" class="form-control date num_right" autocomplete="off" type="text" name="AvailableStockInBase"  placeholder="0000.00"  readonly>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
    <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
 <div class="col-sm-1 i_Icon no-padding">
  <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Warehouse Wise Stock"> </button>
  </div>
</td>

<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="RequisitionQuantity" autocomplete="off" onchange ="OnclickReqQty(this,event)" onkeyup="OnKeyupReqQty(event)" onkeypress="return RqtyFloatValueonly(this, event)" class="form-control date num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  onblur="this.placeholder='0000.00'" ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemPRReqQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemPRReqQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td>
<div class="col-sm-9 lpo_form no-padding">
<input id="ItemOrderedQuantity" onchange ="OnclickReqQty(this,event)" class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(,event)" onkeypress="return QtyFloatValueonly(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00" disabled></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemPROrdQty">
<button type="button" id="SubItemPROrdQty" class="calculator subItmImg" disabled onclick="return SubItemDetailsPopUp('PROrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td><textarea id="ItemRemarks" class="form-control remarksmessage" maxlength="100" onmouseover="OnMouseOver(this.value)" autocomplete="off" type="text" name="ItemRemarks"  placeholder="Remarks">${$("#span_remarks").val()}</textarea></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

    BindItmList(RowNo);
}
function RqtyFloatValueonly(el, evt) {
   
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }

} 
function OnclickReqQty(el,e) {
    if (AvoidChar(el,"QtyDigit") == false) {
        return false;
    }
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var SalePrice;
    ReqQty = clickedrow.find("#RequisitionQuantity").val();

    if (ReqQty == "" || ReqQty == "0") {

        clickedrow.find("#RequisitionQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#RequisitionQuantity_Error").css("display", "block");
        clickedrow.find("#RequisitionQuantity").css("border-color", "red");
        clickedrow.find("#RequisitionQuantity").val("");

        return false;
    }
    else {

        clickedrow.find("#RequisitionQuantity_Error").css("display", "none");
        clickedrow.find("#RequisitionQuantity").css("border-color", "#ced4da");

    }

    clickedrow.find("#RequisitionQuantity").val(parseFloat(ReqQty).toFixed(QtyDecDigit));

}
function OnKeyupReqQty(e) {

    var currentrow = $(e.target).closest('tr');
    var ReqQty = currentrow.find("#RequisitionQuantity").val();
    if (ReqQty != "") {
        currentrow.find("#RequisitionQuantity").css("border-color", "#ced4da");
        currentrow.find("#RequisitionQuantity_Error").text("");
        currentrow.find("#RequisitionQuantity_Error").css("display", "none");
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#Itemname_" + Sno).val();    
    ItemInfoBtnClick(ItmCode)
  
}
function OnChangeItemName(RowID, e) {
    debugger;
    BindPRItemList(e);
}
function BindPRItemList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var pre_Item_id = clickedrow.find("#hfItemID").val();
   clickedrow.find("#RequisitionQuantity").val("");
    Cmn_DeleteSubItemQtyDetail(pre_Item_id);
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {
        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    }

    /*Cmn_BindUOM(clickedrow, Itm_ID,"","","pur")*/ /*commented and add Y for stockable item by Hina on 23-09-2024 */
    Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur")
  
}
function BindItmList(ID) {
    debugger;

    BindItemList("#Itemname_", ID, "#PRItmDetailsTbl", "#SNohf","","PR");

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

function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text(); txtPRDate
        var brId = $("#BrId").text();
        var PRDate = $("#txtPRDate").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: PRDate
            },
            success: function (data) {
                /* if (data == "Exist") { */
                if (data == "TransAllow") { /*End to chk Financial year exist or not*/
                    var PRStatus = "";
                    PRStatus = $('#StatusCode').val().trim();
                    if (PRStatus === "D" || PRStatus === "F") {

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
    var PRNo = "";
    var PRDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SourceType = "";
    var mailerror = "";

    docid = $("#DocumentMenuId").val();
    PRNo = $("#PRNumber").val();
    PRDate = $("#txtPRDate").val();
    $("#hdDoc_No").val(PRNo);
    Remarks = $("#fw_remarks").val();
    SourceType = $("#ddlRequisitionTypeList").val();
    var PRData = $("#PRData1").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (PRNo + ',' + PRDate + ',' + "Update" + ',' + WF_status1)
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
        var pdfAlertEmailFilePath = PRNo.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(PRNo, PRDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/PurchaseRequisition/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && PRNo != "" && PRDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/PurchaseRequisition/PRListApprove?PR_No=" + PRNo + "&PR_Date=" + PRDate + "&SrcType=" + SourceType + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&PRData=" + PRData + "&WF_status1=" + WF_status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PRNo != "" && PRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && PRNo != "" && PRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PRNo, PRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/PurchaseRequisition/ToRefreshByJS?PRData=" + PRData + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(PRNo, PRDate, fileName) {
//    debugger;
//    var PRNo = PRNo;
//    var PRDate = PRDate;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/PurchaseRequisition/SavePdfDocToSendOnEmailAlert",
//        data: { PR_No: PRNo, PR_Date: PRDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#PRNumber").val();
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
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemAvlQty");/*Add by Hina on 23-09-2024 to show aval qty*/
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemPRReqQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPROrdQty");
   
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#Itemname_" + hfsno).val();
    //var UOM = clickdRow.find("#UOM").val();
    var UOM = clickdRow.find("#UOM  option:selected").text();
    var sou_type = $("#ddlRequisitionTypeList" + " option:selected").val();
    var Doc_no = $("#PRNumber").val();
    var Doc_dt = $("#txtPRDate").val();
    var Sub_Quantity = 0;
    var Amendbtn = $("#Amend").val();
    var NewArr = new Array();
    if (flag == "Quantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            /*List.OrederQty = row.find('#subItem_PROrdQty').val();*/
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val();
    }
    else if (flag == "PROrdQty") {
        Sub_Quantity = clickdRow.find("#ItemOrderedQuantity").val();

    }
    else if (flag == "AvlStock") {
        Sub_Quantity = clickdRow.find("#AvailableStockInBase").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#StatusCode").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseRequisition/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            Amendbtn: Amendbtn
        },
        success: function (data) {
            debugger;
            if (flag == "AvlStock") {
                $("#SubItemStockPopUp").html(data);
                $("#Stk_Sub_ProductlName").val(ProductNm);
                $("#Stk_Sub_ProductlId").val(ProductId);
                $("#Stk_Sub_serialUOM").val(UOM);
                $("#Stk_Sub_Quantity").val(Sub_Quantity);
            }
            else {
                $("#SubItemPopUp").html(data);
                $("#Sub_ProductlName").val(ProductNm);
                $("#Sub_ProductlId").val(ProductId);
                $("#Sub_serialUOM").val(UOM);
                $("#Sub_Quantity").val(Sub_Quantity);
            }
       
        }
    });
}

function CheckValidations_forSubItems() {
    return Cmn_CheckValidations_forSubItems("PRItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemPRReqQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("PRItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemPRReqQty", "N");
}
function GetSubItemAvlStock(e) {/*Add by Hina sharma on 23-09-2024 for show aval stock*/
    debugger;
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohf").val();
    var ProductNm = Crow.find("#Itemname_" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#Itemname_" + rowNo + " option:selected").val();
    var UOMId = Crow.find("#UOM").val();
    var UOM = Crow.find("#UOM option:selected").text();
    var AvlStk = Crow.find("#AvailableStockInBase").val();
    var hd_Status = $("#StatusCode").val();
    var Amend_ment = $("#Amendment").val();
    hd_Status = IsNull(hd_Status, "").trim();
    if (hd_Status == "A" && Amend_ment != "AmendmentDetails" ) {
        SubItemDetailsPopUp("AvlStock", e)
    }
    else {
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br", UOMId);
    }
   

   
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
//async function onChangeUom(e) {
//    debugger;
//    let Crow = $(e.target).closest('tr');
//    var QtyDecDigit = $("#QtyDigit").text();

//    let Sno = Crow.find("#SNohiddenfiled").val();
//    let ItemId = Crow.find("#Itemname_" + Sno).val();
//    let UomId = Crow.find("#UOM").val();
//    let ItemType = Crow.find("#hdnItemtype").val();
//    Crow.find("#UOMID").val(UomId); 
//}
async function onChangeUom(e) {
    debugger;
    let Crow = $(e.target).closest('tr');
    var QtyDecDigit = $("#QtyDigit").text();

    let Sno = Crow.find("#SNohf").val();
    let ItemId = Crow.find("#Itemname_" + Sno).val();
    let UomId = Crow.find("#UOM").val();
    let ItemType = Crow.find("#hdnItemtype").val();

    await Cmn_StockUomWise(ItemId, UomId).then((res) => {
        Crow.find("#AvailableStockInBase").val(res);
        Crow.find("#UOMID").val(UomId);
}).catch(err => console.log(err.message));

    //if (ItemType == "Consumable") {
    //Crow.find("#UOM").val(0);

    //}
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        //BindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
       
        var SNohf = clickedrow.find("#SNohf").val();;
        var ItemName = clickedrow.find("#Itemname_" + SNohf + " option:selected").text();
        var ItemId = clickedrow.find("#Itemname_" + SNohf + " option:selected").val();
        var UOMName = clickedrow.find("#UOM option:selected").text();
        var UOMID = clickedrow.find("#UOMID").val();

        Cmn_GetItemStockWhLotBatchSerialWise(ItemId, ItemName, UOMName);
        //$("#WareHouseWiseItemName").val(ItemName);
        //$("#WareHouseWiseUOM").val(UOMName);
        //$.ajax(
        //    {

        //        type: "Post",
        //        url: "/Common/Common/getItemstockWareHouselWise",
        //        data: {
        //            ItemId: ItemId, UomId: UOMID
                    
        //        },
        //        success: function (data) {
        //            debugger;
        //            $('#ItemStockWareHouseWise').html(data);
        //            $("#WareHouseWiseItemName").val(ItemName);
        //            $("#WareHouseWiseUOM").val(UOMName);
        //        },
        //    });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}

function GetDataReorderLevel() {
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PurchaseRequisition/GetDataReorderLevel",
            data: JSON.stringify({}),  // agar params bhejne hain to yaha pass karo
            contentType: "application/json; charset=utf-8",
         //   dataType: "json",
            success: function (data) {
                debugger;
                if (data && data.length > 0) {
                    var rowIdx = $('#PRItmDetailsTbl >tbody >tr').length;
                    var rowCount = rowIdx + 1;
                    var RowNo = rowIdx;
                    debugger;
                    $.each(JSON.parse(data), function (i, item) {
                        RowNo++; // Row number increment
                        rowIdx++;
                        debugger;
                        // Determine if sub-item buttons should be enabled
                        const isSubItem = item["sub_item"] === 'Y'; // true if Y, else false
                        $('#PRItmDetailsTbl tbody').append(`
<tr id="R${rowIdx}">
    <td class="red">
        <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i>
    </td>
    <td class="sr_padding"><span id="SpanRowId">${rowCount++}</span></td>
    <td>
        <div class="col-sm-11 lpo_form" style="padding:0px;">
            <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange="OnChangeItemName(${RowNo},event)" disabled>
                <option value="${item["item_id"]}">${item["item_name"]}</option>
            </select>
            <input type="hidden" id="hfItemID" value="${item["item_id"]}" />
            <span id="ItemNameError" class="error-message is-visible"></span>
        </div>
        <div class="col-sm-1 i_Icon">
            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
            </button>
        </div>
    </td>
    <td>
        <select id="UOM" class="form-control" autocomplete="off" name="UOM" onchange="onChangeUom(event)" disabled>
            <option value="${item["base_uom_id"]}">${item["uom_alias"]}</option>
        </select>
        <input id="UOMID" type="hidden" value="${item["base_uom_id"]}" />
        <input id="hdnItemtype" type="hidden" value="${item["itemtype"]}" />
    </td>
    <td>
        <input id="IDItemtype" value="${item["itemtype"]}" class="form-control date num_right" readonly>
    </td>
 <td>
        <input id="IDmin_stk_lvl" value="${item["min_stk_lvl"]}" class="form-control date num_right" readonly>
    </td>
    <td>
        <div class="col-sm-9 lpo_form no-padding">
            <input id="AvailableStockInBase" class="form-control date num_right" type="text" value="${item["avl_stk1"]}" readonly>
        </div>
       <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
        <button type="button" id="SubItemAvlQty" ${isSubItem ? '' : 'disabled'} class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false">
            <img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}">
        </button>
    </div>
        <div class="col-sm-1 i_Icon">
            <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false">
                <img src="/Content/Images/iIcon1.png" alt="" title="Warehouse Wise Stock">
            </button>
        </div>
    </td>
    <td>
        <div class="col-sm-10 lpo_form no-padding">
            <input id="RequisitionQuantity" class="form-control date num_right" type="text" placeholder="0000.00" onblur="this.placeholder='0000.00'" onchange="OnclickReqQty(this,event)" onkeyup="OnKeyupReqQty(event)" onkeypress="return RqtyFloatValueonly(this,event)">
            <span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
        </div>
        <div class="col-sm-2 i_Icon no-padding" id="div_SubItemPRReqQty">
        <input hidden type="text" id="sub_item" value="${item["sub_item"] || ''}" />
        <button type="button" id="SubItemPRReqQty" ${isSubItem ? '' : 'disabled'} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false">
            <img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}">
        </button>
    </div>
    </td>
    <td>
        <div class="col-sm-9 lpo_form no-padding">
            <input id="ItemOrderedQuantity" class="form-control date num_right" type="text" value="${item["ord_qty"] || ''}" disabled onkeypress="return QtyFloatValueonly(this,event);" onchange="OnclickReqQty(this,event)">
        </div>
        <div class="col-sm-3 i_Icon no-padding" id="div_SubItemPROrdQty">
        <button type="button" id="SubItemPROrdQty" class="calculator subItmImg" ${isSubItem ? '' : 'disabled'} onclick="return SubItemDetailsPopUp('PROrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false">
            <img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}">
        </button>
    </div>
    </td>
    <td>
        <textarea id="ItemRemarks" class="form-control remarksmessage" maxlength="100" placeholder="Remarks">${item["remarks"] || ''}</textarea>
    </td>
    <td style="display:none;">
        <input type="hidden" id="SNohf" value="${RowNo}" />
    </td>
</tr>
                        `);
                    });
                    $("#Div_AddBtnReOrderLevel").css("display", "none");
                    $("#ddlRequisitionTypeList").attr("disabled", true);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error:", error);
            }
        });
    } catch (err) {
        console.log("GetDataReorderLevel Error : " + err.message);
    }
}
function FilterItemDetail(e) {//added by Prakash Kumar on 23-10-2025 to filter item details
    Cmn_FilterTableData(e, "PRItmDetailsTbl", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}

