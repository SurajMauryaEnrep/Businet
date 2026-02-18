$(document).ready(function () {

    var Doc_no = $("#RFQNumber").val();
    $("#hdDoc_No").val(Doc_no);

    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#RFQNumber").val() == "" || $("#RFQNumber").val() == null) {
        $("#txtRQFDate").val(CurrentDate);
    }
    $("#RFQListTbl #datatable-buttons tbody").bind("dblclick", function (e) {
         
        debugger;
        try {
            var PRData = $("#PRData").val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var RFQId = clickedrow.children("#rfq_no").text();
            var RFQDate = clickedrow.children("#Rfq_date").text();
            if (RFQId != null && RFQId != "") {
                window.location.href = "/ApplicationLayer/RequestForQuotation/EditRFQ/?RFQId=" + RFQId + "&RFQDate=" + RFQDate + "&PRData=" + PRData + "&WF_status=" + WF_status;
            }

        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var RFQId = clickedrow.children("#rfq_no").text();
        var RFQDate = clickedrow.children("#Rfq_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        $("#hdDoc_No").val(RFQId);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(RFQId, RFQDate, Doc_id, Doc_Status);

    });
    BindSupplierList();
    BindDLLItemList();

    $('#RFQItmDetailsTbl tbody').on('click', '.deleteIcon', function () {

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);

        });
        //$("#divPlusIcon").show();
        $(this).closest('tr').remove();
        debugger;

        var ItemCode = $(this).closest('tr')[0].cells[2].children[0].children[0].value;

        Cmn_DeleteSubItemQtyDetail(ItemCode);
        //ShowItemListItm(ItemCode);
        DelDeliSchAfterDelItem(ItemCode);
        SerialNoAfterDelete();
        BindDelivertSchItemList();
        OndeleteItemDeleteDelData(ItemCode);
        var status = $("#RFQStatusCode").val()
        var command = $("#hdn_command").val()
        /*$("#ddlPRNumberList").prop("disabled", false);*/
        if ($("#ddlSourceType").val() != "D") {
            var len = $('#RFQItmDetailsTbl tbody tr').length;
            if (len == "0") {
                $("#ddlPRNumberList").prop("disabled", false);
                $("#divPlusIcon").css("display", "block");

                $("#BtnAddItem1").show();
            }

        }
        if (status == "D" && command == "Edit") {
            $("#ddlPRNumberList").prop("disabled", true);
        }
    });
    $('#SupplierDetailsTbl tbody').on('click', '.deleteIcon', function () {
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

        SerialNumAfterDelSupp();
    });
    $('#DeliverySchTble tbody').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var ItemID = $(this).closest('tr')[0].cells[2].innerHTML;
        $("#DeliverySchItemDDL option[value=" + ItemID + "]").show();
        $('#DeliverySchItemDDL').val("0").prop('selected', true);
        SerialNumAfterDelDelShe();
        BindDelivertSchItemList();
    });
    $('#TblTerms_Condition tbody').on('click', '.deleteIcon', function () {

        $(this).closest('tr').remove();
        //  SerialNumAfterDelTermCond();
    });
    $(document).on("change", "#RFQItmDetailsTbl tbody tr td", function () {
        BindDelivertSchItemList();
    })


})
function validUpTo() {
    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#ValidUptoDate").val() < CurrentDate) {
        return false;
    }
    else {
        return true;
    }
}
function OndeleteItemDeleteDelData(ItemCode) {
    debugger;
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            debugger;
            var currentRowDel = $(this);
            var DSchItemID = currentRowDel.find("#Hd_ItemIdFrDS").text();
            var DSQty = currentRowDel.find("#delv_qty").text();

            if (ItemCode === DSchItemID) {
                $(this).closest('tr').remove();
            }
        });
        SerialNumAfterDelDelShe();
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
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function AddNewProspect() {

    try {
        var ProspectFromQuot = "S";
        location.href = "/BusinessLayer/ProspectSetup/AddProspectSetupDetail?ProspectFromRFQ=Y&ProspectFromQuot=" + ProspectFromQuot;

    } catch (err) {
        console.log(PFName + " Error : " + err.message);
    }

}
function BtnSearchFilter() {
    debugger;
    FilterRFQList();
    ResetWF_Level();
}
function FilterRFQList() {
    debugger;
    try {

        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/RequestForQuotation/SearchRFQDetail",
            data: {

                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#RFQListTbl').html(data);
                $('#PRData').val(Fromdate + ',' + Todate + ',' + Status);
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
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckRFQFormValidation() == false) {
        return false;
    }
    if (CheckRFQItemValidations() == false) {
        return false;
    }
    if (CheckValidations_forSubItems() == false) {
        return false;
    }
    if (CheckSuppValidations() == false) {
        return false;
    }
    if (Cmn_CheckDeliverySchdlValidations("RFQItmDetailsTbl", "hfItemID", "Itemname_", "RequisitionQuantity", "SNohf") == false) {
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
    var FinalItemDetail = [];
    var FinalSupplierDetails = [];
    var FinalDeleverySheduleDetails = [];
    var FinalTermAndConditionDetails = [];
    FinalItemDetail = InsertRFQItemDetails();
    /*-----------Sub-item-------------*/

    var SubItemsListArr = Cmn_SubItemList();
    var str2 = JSON.stringify(SubItemsListArr);
    $('#SubItemDetailsDt').val(str2);

    /*-----------Sub-item end-------------*/
    FinalSupplierDetails = InsertSupplierDetails();
    FinalDeleverySheduleDetails = InsertDeleverySheduleDetails();
    FinalTermAndConditionDetails = InsertTermAndConditionDetails();

    debugger;
    var ItemDt = JSON.stringify(FinalItemDetail);
    var SuppDt = JSON.stringify(FinalSupplierDetails);
    var DelShedDt = JSON.stringify(FinalDeleverySheduleDetails);
    var TermAndConDt = JSON.stringify(FinalTermAndConditionDetails);
    $("#ddlSourceType").prop("disabled", false);
    $('#hdItemDetailList').val(ItemDt);
    $("#_hdnSuppDetailList").val(SuppDt);
    $("#_hdnDelShedDetailList").val(DelShedDt);
    $("#hdnTermAndConDetailsList").val(TermAndConDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;

}
function InsertSupplierDetails() {
    debugger;
    var SuppDetailList = new Array();
    $("#SupplierDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemList = {};
        //ItemList.SuppID1 = currentRow.find("td:input").val();
        //ItemList.SuppID = currentRow.find("td:eq(2) input").val();
        //ItemList.SuppType = currentRow.find("td:eq(3) input").val();
        //ItemList.SuppEmail = currentRow.find("td:eq(6) input").val();
        //ItemList.SuppContact = currentRow.find("td:eq(7) input").val();
        ItemList.SuppID = currentRow.find("#SuppID").val();
        ItemList.supp_name = currentRow.find("#SuppName").val();

        ItemList.Address = currentRow.find("#SuppAddress").val();
        ItemList.SuppType = currentRow.find("#SuppType").val();
        ItemList.SuppEmail = currentRow.find("#SuppEmail").val();
        ItemList.SuppContact = currentRow.find("#SuppContact").val();
        SuppDetailList.push(ItemList);
        debugger;
    });

    return SuppDetailList;
}
function InsertDeleverySheduleDetails() {
    debugger;
    var DeleveryShedList = new Array();
    $("#DeliverySchTble TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemList = {};
        ItemList.DelItemID = currentRow.find("#Hd_ItemIdFrDS").text();
        ItemList.ItemName = currentRow.find("#Hd_ItemNameFrDS").text();
        ItemList.DelDate = currentRow.find("#sch_date").text();
        ItemList.DelQty = currentRow.find("#delv_qty").text();
        DeleveryShedList.push(ItemList);
        debugger;
    });

    return DeleveryShedList;
}
function InsertTermAndConditionDetails() {
    debugger;
    var TermAndConList = new Array();
    $("#TblTerms_Condition TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemList = {};
        ItemList.TermAndCondition = currentRow.find("#term_desc").text();
        TermAndConList.push(ItemList);
        debugger;
    });

    return TermAndConList;
}
function CheckSuppValidations() {
    debugger;

    var ValidationFlag = true;
    var Flag = 'N';

    if ($("#SupplierDetailsTbl TBODY TR td").length > 0) {
        $("#SupplierDetailsTbl TBODY TR").each(function () {
            debugger;
            var currentRow = $(this);
            var Email = currentRow.find("#SuppEmail").val();
            if (Email == "" || Email == null) {
                currentRow.find("#SuppEmail_Error").text($("#valueReq").text());
                currentRow.find("#SuppEmail_Error").css("display", "block");
                currentRow.find("#SuppEmail").css("border-color", "red");
                Flag = 'Y';
            }
            else {
                currentRow.find("#SuppEmail_Error").css("display", "none");
                currentRow.find("#SuppEmail").css("border-color", "#ced4da");
            }
            var Contact = currentRow.find("#SuppContact").val();
            if (Contact == "" || Contact == null) {
                currentRow.find("#SuppContact_Error").text($("#valueReq").text());
                currentRow.find("#SuppContact_Error").css("display", "block");
                currentRow.find("#SuppContact").css("border-color", "red");
                Flag = 'Y';
            }
            else {
                currentRow.find("#SuppContact_Error").css("display", "none");
                currentRow.find("#SuppContact").css("border-color", "#ced4da");
            }

        })

    }
    else {

        swal("", $("#AtleastOneSupisReq").text(), "warning");
        Flag = 'Y';
    }

    if (Flag == 'Y') {
        ValidationFlag = false;
    }
    else {
        ValidationFlag = true;
    }

    if (ValidationFlag == true) {
        return true;
    }
    else {
        return false;
    }


}
function OnChangeCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
//function OnClickAddDeliveryDetail() {
//    var rowIdx = 0;
//    debugger;
//    var deletetext = $("#Span_Delete_Title").text();
//    var QtyDecDigit = $("#QtyDigit").text();
//    var ValidInfo = "N";
//    if ($('#DeliverySchItemDDL').val() == "0") {
//        ValidInfo = "Y";
//        $('#SpanDelSchItemName').text($("#valueReq").text());
//        $("#SpanDelSchItemName").css("display", "block");
//    }
//    else {
//        $("#SpanDelSchItemName").css("display", "none");
//    }
//    if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
//        ValidInfo = "Y";
//        $('#SpanDelSchItemQty').text($("#valueReq").text());
//        $("#SpanDelSchItemQty").css("display", "block");
//    }
//    else {
//        $("#SpanDelSchItemQty").css("display", "none");
//    }
//    if ($('#DeliveryDate').val() == "") {
//        ValidInfo = "Y";
//        $('#SpanDeliSchDeliDate').text($("#valueReq").text());
//        $("#SpanDeliSchDeliDate").css("display", "block");
//    }
//    else {
//        $("#SpanDeliSchDeliDate").css("display", "none");
//    }
//    if (ValidInfo == "Y") {
//        return false;
//    }
//    var deldate = $('#DeliveryDate').val();
//    var rowCount = $('#DeliverySchTble >tbody >tr').length + 1
//    $('#DeliverySchTble tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
// <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${deletetext}"></i></td>
// <td>${rowCount}</td>
//<td style="display:none;">${$('#DeliverySchItemDDL').val()}</td>
//<td >${$('#DeliverySchItemDDL option:selected').text()}</td>
//<td >${$('#DeliveryDate').val()}</td>
//<td >${parseFloat($('#DeliverySchQty').val()).toFixed(QtyDecDigit)}</td>

//</tr>`);
//    BindDelivertSchItemList();

//    $("#DeliverySchQty").val(parseFloat(0).toFixed(QtyDecDigit));
//    $("#DeliveryDate").val("");
//}



function OnClickAddTermConditionDetailRFQ() {
    var rowCount = $('#TblTerms_Condition >tbody >tr').length + 1
    if ($("#TxtTerms_Condition").val() != null && $("#TxtTerms_Condition").val() != "") {
        $("#TblTerms_Condition tbody").append(`
      <tr>
     <td class="red center"> <i class="deleteIcon fa fa-trash" id="TC_delBtnIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
     <td class="sr_padding">${rowCount}</td>
     <td>${$("#TxtTerms_Condition").val()}</td>
     </tr>
`)
    }


    $("#TxtTerms_Condition").val("");
}

function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    var clickedrow = $(evt.target).closest("tr");

    clickedrow.find("#SpanDelSchItemQty").css("display", "none");
    clickedrow.find("#DeliverySchQty").css("border-color", "#ced4da");

    return true;
}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    } else {
        return true;
    }

}

function SuppEmailChange(e) {
    //var currentRow = $(e.target).closest("tr");
    debugger;
    /*var validFlag = "N";*/
    var Email = $("#SuppEmail").val();

    var mailformat = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    //mailformat.test(Email);
    if (Email != "") {
        if (mailformat.test(Email)) {
            //document.getElementById("SuppEmail_Error").innerHTML = "";
            $("#SuppEmail_Error").css("display", "none");
            $("#SuppEmail").css("border-color", "#ced4da");
            return true;
        }
        else {
            //document.getElementById("SuppEmail_Error").innerHTML = $("#InvalidEmail").text();
            $('#SuppEmail_Error').text($("#InvalidEmail").text());
            $("#SuppEmail_Error").css("display", "block");
            $("#SuppEmail").css("border-color", "red");
            return false;
        }
    }
    else {
        $("#SuppEmail_Error").css("display", "none");
        //document.getElementById("vmEmail1").innerHTML = "";
        $("#SuppEmail").css("border-color", "#ced4da");
    }
    //currentRow.find("#SuppEmail_Error").css("display", "none");
    //currentRow.find("#SuppEmail").css("border-color", "#ced4da");
}
function SuppContactChange(e) {
    var currentRow = $(e.target).closest("tr");
    currentRow.find("#SuppContact_Error").css("display", "none");
    currentRow.find("#SuppContact").css("border-color", "#ced4da");
}
function onChangeddlSourceType() {
    debugger
    if ($("#ddlSourceType").val() != "0") {
        document.getElementById("vmSourceType").innerHTML = null;
        $("#ddlSourceType").css("border-color", "#ced4da");
    }
    if ($("#ddlSourceType").val() == "P") {
        debugger;
        $("#divddlprno").css('display', 'block');
        $("#divPRdate").css('display', 'block');
        $("#divPlusIcon").css('display', 'block');
        $("#listitemdelbtn").css('display', 'none');
        $("#ddlPRNumberList").prop("disabled", false);
        BindPRNumberList();
        $('#RFQItmDetailsTbl tbody tr').each(function () {
            $(this).closest('tr').remove();
        })

    }
    else if ($("#ddlSourceType").val() == "D") {
        $("#divddlprno").css('display', 'none');
        $("#divPRdate").css('display', 'none');
        $("#divPlusIcon").css('display', 'none');
        $("#listitemdelbtn").css('display', 'block');
        $('#RFQItmDetailsTbl tbody tr').each(function () {
            $(this).closest('tr').remove();
        })
    }
    //else {
    //    $("#divddlprno").css('display', 'none');
    //    $("#divPRdate").css('display', 'none');
    //    $("#divPlusIcon").css('display', '');
    //    $("#listitemdelbtn").css('display', 'none');
    //    $('#RFQItmDetailsTbl tbody tr').each(function () {
    //        $(this).closest('tr').remove();
    //    })
    //}
}

function OnChangeddlPrNumberList() {
    debugger;

    var prdate = $("#ddlPRNumberList").val().trim();
    var date = prdate.split("-");
    if (prdate != 0) {
        document.getElementById("vmddlPRNumberList").innerHTML = "";
        $("#vmddlPRNumberList").css("display", "none");
        $("[aria-labelledby='select2-ddlPRNumberList-container']").css("border-color", "#ced4da");

        var FDate = date[2] + '-' + date[1] + '-' + date[0];
        var prnumber = $("#Textddl option:selected").text();
        //prdate = prdate.Todate();
        $("#RfqPRNo").val(prnumber);
        $("#txtPRDate").val(FDate);
    }
    else {
        document.getElementById("vmddlPRNumberList").innerHTML = $("#valueReq").text();
        $("#vmddlPRNumberList").css("display", "block");
        $("[aria-labelledby='select2-ddlPRNumberList-container']").css("border-color", "red");
    }


}
function OnChangeValidUpToDate() {
    if ($("#ValidUptoDate").val() != "") {
        document.getElementById("vmValidUptoDate").innerHTML = null;
        $("#ValidUptoDate").css("border-color", "#ced4da");
    }

}
function InsertRFQItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#RFQItmDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var Itemname = currentRow.find("#Itemname_" + rowid).text();
        var Item_Name1 = Itemname.split('-')
        var Item_Name = Item_Name1[6];
        var subitem = "";
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Itemname_" + rowid).val();
        ItemList.ItemName = currentRow.find("#Itemname_" + rowid + " option:selected").text();
        ItemList.subitem = currentRow.find("#sub_item").val();
        //ItemList.UOMID = currentRow.find("#UOMID").val();
        var UOM_ID = currentRow.find("#UOMID").val();
        var Item_type= currentRow.find("#ItemType").val();
        if (Item_type == "Service") {
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
        ItemList.UOM = currentRow.find("#UOM").val();
        ItemList.RequQty = currentRow.find("#RequisitionQuantity").val();
        ItemList.ItemRemarks = currentRow.find('#ItemRemarks').val();

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};
function AddPRItemsRow() {
    debugger;

    var QtyDecDigit = $("#QtyDigit").text();
    var docno = $('#ddlPRNumberList option:selected').text();
    $("#RfqPRNo").val(docno);

    if ($('#ddlPRNumberList').val() != "0" && $('#ddlPRNumberList').val() != "") {
        var text = $('#ddlPRNumberList').val();

        //$(".Plus_IconToDisable").css('display', 'none');divPlusIcon
        $("#divPlusIcon").css('display', 'none');
        debugger;

        $("#ddlPRNumberList").prop("disabled", true);
        $("#ddlSourceType").prop("disabled", true);
        var hdSelectedSourceDocument = null;

        var SourDocumentNo = $('#ddlPRNumberList option:selected').text();
        hdSelectedSourceDocument = SourDocumentNo;
        $("#RfqPRNo").val(hdSelectedSourceDocument);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/RequestForQuotation/getDetailBySourceDocumentNo",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var RowNo = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                debugger;
                                var rowCount = $('#RFQItmDetailsTbl >tbody >tr').length + 1
                                if (rowCount > 1) {
                                    $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
                                        debugger;
                                        var currentRow = $(this);
                                        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
                                    });
                                }
                                
                                if (RowNo == "0") {
                                    RowNo = 1;
                                }
                                $('#RFQItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="spanrowid">${rowCount}</span></td>
<td><div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" disabled ><option value="${arr.Table[i].item_id}">${arr.Table[i].item_name}</option></select>
<input  type="hidden" id="hfItemID" value="${arr.Table[i].item_id}" />
<span id="ItemNameError" class="error-message is-visible"></span></div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
</button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].uom_name}" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" value="${arr.Table[i].uom_id}" type="hidden" /></td>
<td><input id="ItemType" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].ItemType}" name="ItemType" placeholder="${$("#ItemType").text()}" disabled></td>
<td>
<div class="col-sm-9 lpo_form no-padding">

<input id="RequisitionQuantity" onchange ="OnclickReqQty(event)" onkeyup="OnKeyupReqQty(event)"  disabled value="${parseFloat(arr.Table[i].pr_qty).toFixed(QtyDecDigit)} " onkeypress="return RqtyFloatValueonly(this, event)" class="form-control date num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  onblur="this.placeholder = '0000.00'" ><span id="RequisitionQuantity_Error" class="error - message is - visible"></span>
</div>
<div class="col-sm-3 i_Icon no-padding" id="div_SubItemRFQPRReqQty">
<button type="button" id="SubItemRFQPRReqQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('RFQPRReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
<input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
</div>
</td>
<td><input id="ItemRemarks" class="form-control remarksmessage"  type="textarea" value="${arr.Table[i].it_remarks}" name="ItemRemarks" maxlength = "100" autocomplete="off"  placeholder="${$("#span_remarks").text()}" ></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
                                                    </tr>`);
                                BindDelivertSchItemList();
                            }
                        }
                        debugger
                        if (arr.Table1.length > 0) {
                            var rowIdx = 0;
                            var deletetext = $("#Span_Delete_Title").text();
                            //$("#hdn_Sub_ItemDetailTbl >tbody tr").remove();
                            /* $("#hdn_Sub_ItemDetailTbl >tbody >tr").remove();*/
                            /*$("#hdn_Sub_ItemDetailTbl >tbody >tr td #ItemId[value=" + ItmId + "]").closest('tr').remove();*/
                            for (var y = 0; y < arr.Table1.length; y++) {

                                var ItmId = arr.Table1[y].item_id;
                                var SubItmId = arr.Table1[y].sub_item_id;
                                var SubItmName = arr.Table1[y].sub_item_name;
                                var PRQty = arr.Table1[y].pr_qty;
                                var SrcDocNo = arr.Table1[y].pr_no;
                                var SrcQtDt = arr.Table1[y].pr_date;
                                /* if (SrcDocNo == SourDocumentNo) {*/

                                $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            <td><input type="text" id="subItemQty" value='${PRQty}'></td>
                            
                            </tr>`);
                                /* }*/
                                //<td><input type="text" id="subItemSrcDocNo" value='${SrcQtNo}'></td>
                                //    <td><input type="text" id="subItemSrcDocDate" value='${SrcQtDt}'></td>

                            }

                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;

                }
            });
    } else {


        document.getElementById("vmddlPRNumberList").innerHTML = $("#valueReq").text();
        $("#vmddlPRNumberList").css("display", "block");
        $("[aria-labelledby='select2-ddlPRNumberList-container']").css("border-color", "red");
    }
}
function CheckRFQFormValidation() {

    debugger;

    var ValidationFlag = true;
    var Flag = 'N';

    var ddlSourceType = $('#ddlSourceType').val();
    if (ddlSourceType != "0") {
        document.getElementById("vmSourceType").innerHTML = null;
        $("#ddlSourceType").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmSourceType").innerHTML = $("#valueReq").text();
        $("#ddlSourceType").css("border-color", "red");

        Flag = 'Y';
    }


    var ddlSourceType = $('#ValidUptoDate').val();
    if (ddlSourceType != "") {
        document.getElementById("vmValidUptoDate").innerHTML = null;
        $("#ValidUptoDate").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmValidUptoDate").innerHTML = $("#valueReq").text();
        $("#ValidUptoDate").css("border-color", "red");

        Flag = 'Y';
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
        return true;
    }
    else {
        return false;
    }

}
function CheckRFQItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#RFQItmDetailsTbl TBODY TR td").length > 0) {
        $("#RFQItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohf").val();

            if (currentRow.find("#Itemname_" + Sno).val() == "0") {
                currentRow.find("#ItemNameError").text($("#valueReq").text());
                currentRow.find("#ItemNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border", "1px solid red");

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
function OnKeyupReqQty(e) {

    var currentrow = $(e.target).closest('tr');
    var ReqQty = currentrow.find("#RequisitionQuantity").val();
    if (ReqQty != "") {
        currentrow.find("#RequisitionQuantity").css("border-color", "#ced4da");
        currentrow.find("#RequisitionQuantity_Error").text("");
        currentrow.find("#RequisitionQuantity_Error").css("display", "none");
    }
}

function BindItmList(ID) {
    debugger;

    BindItemList("#Itemname_", ID, "#RFQItmDetailsTbl", "#SNohf", "", "RFQ");
    //HideSelectedItem("#Itemname_", ID, "#RFQItmDetailsTbl", "#SNohf");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/PurchaseRequisition/getListOfItems",
    //        data: function (params) {
    //            var queryParameters = {
    //                SO_ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    $('#Itemname_' + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl' + ID).append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#Itemname_' + ID).select2({
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
    //                    debugger;
    //                    HideItemListItm(ID);
    //                }
    //            }
    //        },
    //    });
}

function BindSupplierList() {
    debugger;
    var SuppPros_type;
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
        $("#Div_ProspectSetup").css("display", "none");
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
       
      //  $("#Prosbtn").css("display", "block");

        CheckUserRolePageAccess(SuppPros_type, "RFQ");
    }

    $('#SupplierNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
    debugger;
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
                debugger;

                $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
                    debugger;
                    var currentRow = $(this);
                    var Suppid = "";
                    Suppid = currentRow.find('#SuppID').val();
                    var SuppType = currentRow.find("#SuppType").val();
                    if (SuppType == SuppPros_type) {
                        for (var k = 0; k < data.length; k++) {
                            if (data[k].ID == Suppid) {
                                data[k].ID = null;
                                data[k].Name = null;

                            }
                        }
                    }


                });

                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

    });
}
function OnclickReqQty(e) {
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

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#Itemname_" + Sno).val();
    // ItmName = clickedrow.find("#Itemname_" + Sno + " option:selected").text()

    ItemInfoBtnClick(ItmCode);

    //if (ItmCode != "" && ItmCode != "0" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            $("#SpanItemDescription").text(ItmName);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        //currentRow.find("#SpanRowId").text(SerialNo);
        currentRow.find("#spanrowid").text(SerialNo);

    });


};
function SerialNumAfterDelSupp() {
    var SerialNo1 = 0;
    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo1 = SerialNo1 + 1;
        currentRow.find("#spanrowid").text(SerialNo1);
    });
}
function SerialNumAfterDelDelShe() {
    var SerialNo1 = 0;
    $("#DeliverySchTble >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo1 = SerialNo1 + 1;
        currentRow.find("#cmn_srno").text(SerialNo1);
    });
}
//function SerialNumAfterDelTermCond() {
//    var SerialNo1 = 0;
//    $("#TblTerms_Condition >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        SerialNo1 = SerialNo1 + 1;
//        currentRow.find("td:eq(1)").text(SerialNo1);

//    });
//}
//function ShowItemListItm(ItemCode) {
//    debugger;
//    if (ItemCode != "0") {
//        $("#RFQItmDetailsTbl >tbody >tr").each(function () {
//            debugger;
//            var currentRow = $(this);
//            var Sno = currentRow.find("#SNohf").val();
//            $("#Itemname_" + Sno + " option[value=" + ItemCode + "]").removeClass("select2-hidden-accessible");
//        });
//    }
//}
function AddNewRow() {
    var rowIdx = 0;
    debugger;

    var rowCount = $('#RFQItmDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#RFQItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="spanrowid">${rowCount}</span></td>
<td><div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" onchange ="OnChangeItemName(${RowNo},event)"></select>
<input  type="hidden" id="hfItemID" value="" />
<span id="ItemNameError" class="error-message is-visible"></span></div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">
</button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
 <td>
     <input id="ItemType" class="form-control" autocomplete="off" type="text" value="" name="ItemType" placeholder="${$("#ItemType").text()}" disabled>

 </td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="RequisitionQuantity" onchange ="OnclickReqQty(event)" onkeyup="OnKeyupReqQty(event)"  onkeypress="return RqtyFloatValueonly(this, event)" class="form-control date num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  onblur="this.placeholder='0000.00'" ><span id="RequisitionQuantity_Error" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemRFQReqQty">
<input hidden type="text" id="sub_item" value="" />
<button type="button" id="SubItemRFQReqQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Quantity',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>

<td><input id="ItemRemarks" class="form-control remarksmessage" type="textarea" name="ItemRemarks" @maxlength = "100" autocomplete="off"  placeholder="${$("#span_remarks").text()}" ></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);

    // BindItmList(RowNo);
    ItemDdlDataRFQ(RowNo);
}

function AddNewSuppRow() {
    var rowIdx = 0;
    debugger;
    var supp_id = $("#SupplierNameList").val();

    var rowCount = $('#SupplierDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#SupplierDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find(".SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    if (supp_id == "0" || supp_id == "" || supp_id == null) {
        $("#vmSupplierNameList").text($("#valueReq").text());
        $("#vmSupplierNameList").css("display", "block");
        $('[aria-labelledby="select2-SupplierNameList-container"]').css("border-color", "red");
    }
    else {

        $("#vmSupplierNameList").text("");
        $("#vmSupplierNameList").css("display", "none");
        $("#SupplierNameList").css("border-color", "#ced4da");
        //$('aria-labelledby="select2-SupplierNameList-container"').css("border-color", "#ced4da");

        //        $('#SuppDTbody').append(`<tr id="R${++rowIdx}">
        //<td class=" red"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="@Resource.Delete"></i></td>
        // <td class="sr_padding"><span id="spanrowid">${rowCount}</span></td>
        //<td style="display: none;"><input id="SuppID" type="hidden" /></td>
        //<td style="display: none;"><input id="SuppType" type="hidden" /></td>
        //<td><div class="lpo_form"><input id="SuppName" disabled class="form-control" autocomplete="off" type="text" placeholder="${$("#SupplierName").text()}"><span id="RequisitionQuantity_Error" class="error-message is-visible"></span></div></td>
        //<td><textarea class="form-control remarksmessage" cols="20" readonly id="SuppAddress" maxlength="100" name="Address" placeholder="${$("#Address").text()}"></textarea></td>
        //<td><div class="lpo_form"><input id="SuppEmail" class="form-control" autocomplete="off" type="text"  placeholder="${$("#EmailID").text()}" onchange="SuppEmailChange(event)" onkeypress="SuppEmailChange(event)" onblur="this.placeholder='Email ID'" ><span id="SuppEmail_Error" class="error-message is-visible"></span></div></td>
        //<td><div class="lpo_form"><input id="SuppContact" class="form-control" autocomplete="off" type="text"  placeholder="Contact Number" onchange="SuppContactChange(event)" onkeypress="SuppContactChange(event)" onblur="this.placeholder='Contact Number'" ><span id="SuppContact_Error" class="error-message is-visible"></span></div></td>
        //<td style="display: none;"><input type="hidden" class="SNohf" value="${RowNo}" /></td>
        //</tr>`);

        BindSuppList(RowNo, supp_id);
        $("#SupplierNameList").attr("onchange", "");
        $("#SupplierNameList").val("0").trigger("change");
        $("#SupplierNameList").attr("onchange", "OnChangeSupplierNameList()");
    }



}

function OnChangeSupplierNameList() {
    var supp_id = $("#SupplierNameList").val();
    debugger
    if (supp_id == "0" || supp_id == "" || supp_id == null) {
        $("#vmSupplierNameList").text($("#valueReq").text());
        $("#vmSupplierNameList").css("display", "block");
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "red");
    } else {
        $("#vmSupplierNameList").text("");
        $("#vmSupplierNameList").css("display", "none");
        //$("#SupplierNameList").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierNameList-container']").css("border-color", "#ced4da");
    }
}

function BindDLLItemList() {
    debugger;
    BindItemList("#Itemname_", "1", "#RFQItmDetailsTbl", "#SNohf", "BindData", "RFQ")

    // BindData();
    /* BindDelivertSchItemList();*/
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/PurchaseRequisition/getListOfItems",
    //        data: function (params) {
    //            var queryParameters = {
    //                SO_ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    sessionStorage.removeItem("PLitemList");
    //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
    //                    $('#Itemname_1').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#Itemname_1').select2({
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

    //                    BindData();
    //                    BindDelivertSchItemList();
    //                }
    //            }
    //        },
    //    });

}
function BindData() {
    debugger;
    //        $("#RFQItmDetailsTbl >tbody >tr").each(function () {

    //            var currentRow = $(this);
    //            var rowid = currentRow.find("#SNohf").val();
    //            rowid = parseFloat(rowid) + 1;
    //            if (rowid > $("#RFQItmDetailsTbl >tbody >tr").length) {
    //                return false;
    //            }
    //            ItemDdlDataRFQ(rowid);

    //        });   
    //$("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this); 
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var ItemID = '#Itemname_' + rowid;
    //    if (ItmID != '0' && ItmID != "") {
    //        currentRow.find("#Itemname_" + rowid).val(ItmID).trigger('change.select2');
    //    }

    //});

    //$("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();

    //    var ItemCode;
    //    ItemCode = ItmID;

    //    if (ItemCode != '0' && ItemCode != "") { 

    //        $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
    //            debugger;
    //            var currentRowD = $(this);
    //            var ItemCodeD;
    //            ItemCodeD = currentRowD.find("#hfItemID").val();
    //            var rowidD = currentRowD.find("#SNohf").val();
    //            if (ItemCodeD != '0' && ItemCodeD != "") {
    //                if (currentRow.find("#Itemname_" + rowidD).val() != ItemCode) {
    //                    $("#Itemname_" + rowid + " option[value=" + ItemCodeD + "]").select2().hide();
    //                }
    //            }
    //        })
    //    }
    //});

    BindDelivertSchItemList();
}
function ItemDdlDataRFQ(rowid) {
    debugger
    BindItemList("#Itemname_", rowid, "#RFQItmDetailsTbl", "#SNohf", "BindData", "RFQ")
    //var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    //if (PLItemListData != null) {
    //    if (PLItemListData.length > 0) {
    //        $("#Itemname_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
    //        for (var i = 0; i < PLItemListData.length; i++) {
    //            $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
    //        }
    //        var firstEmptySelect = true;
    //        $("#Itemname_" + rowid).select2({
    //            templateResult: function (data) {
    //                var selected = $("#Itemname_" + rowid).val();
    //                if (check(data, selected, "#RFQItmDetailsTbl", "#SNohf", "#Itemname_") == true) {
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
}
//function HideItemListItm(ID) {
//    $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
//        debugger;
//        var currentRow = $(this);
//        var ItmID = currentRow.find("#hfItemID").val();
//        var rowid = currentRow.find("#SNohf").val();

//        var ItemCode;
//        ItemCode = ItmID;

//        if (ItemCode != '0' && ItemCode != "") {
//            $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
//                debugger;
//                var currentRowD = $(this);
//                var ItemCodeD;
//                ItemCodeD = currentRowD.find("#hfItemID").val();
//                var rowidD = currentRowD.find("#SNohf").val();
//                if (ItemCodeD != '0' && ItemCodeD != "") {
//                    if (ItemCodeD != ItemCode) {
//                        $("#Itemname_" + rowid + " option[value=" + ItemCodeD + "]").select2().hide();
//                    }
//                }
//            })
//        }
//        else {
//            $("#RFQItmDetailsTbl >tbody >tr").each(function (i, row) {
//                debugger;
//                var currentRowD = $(this);
//                var ItemCodeD;
//                ItemCodeD = currentRowD.find("#hfItemID").val();
//                if (ItemCodeD != '0' && ItemCodeD != "") {
//                    if (ItemCodeD != ItemCode) {
//                        $("#Itemname_" + rowid + " option[value=" + ItemCodeD + "]").select2().hide();
//                    }
//                }
//            })
//        }
//    });
//}
function OnChangeItemName(RowID, e) {
    debugger;
    BindRFQItemList(e);
}

function BindSuppList(RowNo, supp_id) {
    debugger;
    var SuppPros_type;
    if ($("#Supplier").is(":checked")) {
        SuppPros_type = "S";
        $("#Prosbtn").css("display", "none");
    }
    if ($("#Prospect").is(":checked")) {
        SuppPros_type = "P";
        $("#Prosbtn").css("display", "block");
    }
    var rowCount = $('#SupplierDetailsTbl >tbody >tr').length + 1;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/RequestForQuotation/GetSuppDetail",
            data: {
                Supp_id: supp_id,
                SuppPros_type: SuppPros_type

            },
            dataType: "json",
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
                        var Suppame = arr.Table[0].supp_name
                        var suppEmail = arr.Table[0].cont_email
                        $('#SuppDTbody').append(`<tr id="R1">
<td class=" red"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="spanrowid">${rowCount}</span></td>
<td style="display: none;"><input id="SuppID" type="hidden" value="${arr.Table[0].supp_id}" /></td>
<td style="display: none;"><input id="SuppType" type="hidden" value="${arr.Table[0].supp_type}" /></td>
<td><div class="lpo_form"><input id="SuppName" disabled class="form-control" autocomplete="off" value="${Suppame}" type="text" placeholder="${$("#SupplierName").text()}"><span id="RequisitionQuantity_Error" class="error-message is-visible"></span></div></td>
<td><textarea class="form-control remarksmessage" cols="20" readonly id="SuppAddress" maxlength="100" name="Address" placeholder="${$("#Address").text()}">${IsNull(arr.Table[0].BillingAddress, "")}</textarea></td>
<td><div class="lpo_form"><input id="SuppEmail" class="form-control" autocomplete="off" value="${IsNull(suppEmail, "")}" type="text"  placeholder="${$("#EmailID").text()}" onchange="SuppEmailChange(event)" onkeypress="SuppEmailChange(event)" onblur="this.placeholder='${$("#EmailID").text()}'" ><span id="SuppEmail_Error" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="SuppContact" class="form-control" autocomplete="off" value="${IsNull(arr.Table[0].cont_num1, "")}" type="text"  placeholder="${$("#span_ContactNumber").text()}" onchange="SuppContactChange(event)" onkeypress="SuppContactChange(event)" onblur="this.placeholder='${$("#span_ContactNumber").text()}'" ><span id="SuppContact_Error" class="error-message is-visible"></span></div></td>
<td style="display: none;"><input type="hidden" class="SNohf" value="${RowNo}" /></td>
</tr>`);
                        //$('#SuppID' + RowNo).val(arr.Table[0].supp_id);
                        //$('#SuppType' + RowNo).val(arr.Table[0].supp_type);
                        //$('#SuppName' + RowNo).val(Suppame);
                        //$('#SuppAddress' + RowNo).val(arr.Table[0].BillingAddress);                       
                        //$('#SuppEmail' + RowNo).val(suppEmail);
                        //$('#SuppContact' + RowNo).val(arr.Table[0].cont_num1);

                    }

                }
            },
        });
}
function IsNull(instr, outstr) {
    if (instr == null || instr == "null" || instr == "") {
        return outstr;
    }
    else {
        return instr;
    }
}
function BindPRNumberList() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/RequestForQuotation/getListPRNumber",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
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

                        $('#ddlPRNumberList').empty();
                        $('#ddlPRNumberList').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-uom='${$("#DocDate").text()}'></optgroup>`);
                        $('#Textddl').append(`<option data-uom="" value="0">---Select---</option>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-uom="${arr.Table[i].pr_dt}" value="${arr.Table[i].pr_dt}">${arr.Table[i].pr_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlPRNumberList').select2({
                            templateResult: function (data) {
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
                                firstEmptySelect = false;
                            }
                        });

                    }
                }
            },
        });

}
function BindRFQItemList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();
    var ItemIdToRemove = clickedrow.find("#hfItemID").val();
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
    //HideItemListItm(SNo);
    // HideSelectedItem("#Itemname_", SNo, "#RFQItmDetailsTbl", "#SNohf");

    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "pur");
    if (ItemIdToRemove !== Itm_ID) {
        DelDeliSchAfterDelItem(ItemIdToRemove);
        Cmn_DeleteSubItemQtyDetail(ItemIdToRemove);
    }


}
function RqtyFloatValueonly(el, evt) {
    debugger;
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    return true;
}
//function EditBtnClick() {
//    debugger
//    var Status = "";   
//    Status = $('#RFQStatusCode').val();
//    if (Status === "A") {
//        debugger
//        var RFQNo = $("#RFQNumber").val();
//        var RFQDate = $("#txtRQFDate").val();
//        $.ajax({
//            type: "POST",
//            url: "/ApplicationLayer/RequestForQuotation/CheckPQAgainstRFQ",
//            dataType: "json",
//            data: { DocNo: RFQNo, DocDate: RFQDate },
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
//                        FlagUse = "Y";
//                        swal("", $("#QuotationHasBnPrcsdCanNotBeMdfd").text(), "warning");
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

//---------------------------------------------Delivery Schedule---------------------------------------//
function DelDeliSchAfterDelItem(ItemID) {
    // debugger;
    Cmn_DelDeliSchAfterDelItem(ItemID, "RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity");

}
function OnChangeDeliveryQty() {
    debugger;
    Cmn_OnChangeDeliveryQty("RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var DelQty = $('#DeliverySchQty').val();
    //if ($('#DeliverySchQty').val() == "0" || $('#DeliverySchQty').val() == "") {
    //    ValidInfo = "Y";
    //    $('#SpanDelSchItemQty').text($("#valueReq").text());
    //    $("#SpanDelSchItemQty").css("display", "block");
    //}
    //else {
    //    $("#SpanDelSchItemQty").css("display", "none");
    //    $('#DeliverySchQty').val(parseFloat(DelQty).toFixed(QtyDecDigit));
    //    if (CalculateDeliverySchQty() === false) {
    //        $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
    //        $("#SpanDelSchItemQty").css("display", "block");
    //        $('#DeliverySchQty').css("border-color", "red");
    //        //$('#DeliverySchQty').val("");
    //        return false;
    //    }
    //    else {
    //        $("#SpanDelSchItemQty").css("display", "none");
    //        $('#DeliverySchQty').css("border-color", "#ced4da");
    //    }
    //}
}
//function CalculateDeliverySchQty() {
//    debugger;
//    var QtyDecDigit = $("#QtyDigit").text();
//    var DelSchItem = $('#DeliverySchItemDDL').val();
//    var RFQItemQty = parseFloat(0).toFixed(QtyDecDigit);
//    var DelSchQty = parseFloat($('#DeliverySchQty').val()).toFixed(QtyDecDigit);
//    var AllDelSchQty = parseFloat(0).toFixed(QtyDecDigit);
//    $("#RFQItmDetailsTbl >tbody >tr").each(function () {
//        debugger;
//        var currentRow = $(this);
//        var Sno = currentRow.find("#SNohf").val();
//        var POItemID = "";
//        var ItemQty = parseFloat(0).toFixed(QtyDecDigit);

//        POItemID = currentRow.find("#Itemname_" + Sno).val();
//        ItemQty = parseFloat(currentRow.find("#RequisitionQuantity").val()).toFixed(QtyDecDigit);

//        if (POItemID === DelSchItem) {
//            RFQItemQty = ItemQty;
//        }
//    });
//    $("#DeliverySchTble >tbody >tr").each(function () {
//        debugger;
//        var currentRow = $(this);
//        var DelSchItemtbl = currentRow.find("td:eq(2)").text();
//        var DelSchQtyTbl = currentRow.find("td:eq(5)").text();
//        if (DelSchItem == DelSchItemtbl) {
//            AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQtyTbl)).toFixed(QtyDecDigit);
//        }
//    });
//    AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQty)).toFixed(QtyDecDigit);
//    if (parseFloat(RFQItemQty) >= parseFloat(AllDelSchQty)) {
//        return true;
//    }
//    else {

//        return false;
//    }
//}

function BindDelivertSchItemList() {
    debugger;
    Cmn_BindDelivertSchItemList("RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity")
};
function OnClickReplicateDeliveryShdul() {
    Cmn_OnClickReplicateDeliveryShdul("RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity");
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
    Cmn_OnClickAddDeliveryDetail("RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity");
    debugger;

    if ($("#DeliverySchTble tbody tr ").length > 0) {

        $("#DeliverySchTble tbody tr").each(function () {
            debugger
            var currentrow = $(this);

            //var a=  currentrow.find("td:eq(7)").text();
            //var b=  currentrow.find("td:eq(8)").text();
            //var c=  currentrow.find("td:eq(9)").text();

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
    //    var DelQty = $('#DeliverySchQty').val();
    //    if (AvoidDot(DelQty) == false) {
    //        DelQty = 0;
    //    }
    //    if (DelQty == "0" || DelQty == "") {
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
    //            $('#SpanDelSchItemQty').text($("#SchReqQtyEqualTo").text());
    //            $("#SpanDelSchItemQty").css("display", "block");
    //            $('#DeliverySchQty').css("border-color", "red");
    //            //$('#DeliverySchQty').val("");
    //            return false;
    //        }
    //        else {
    //            $("#SpanDelSchItemQty").css("display", "none");
    //            $('#DeliverySchQty').css("border-color", "#ced4da");
    //        }
    //        var deldate = $('#DeliveryDate').val();
    //        var deletetext = $("#Span_Delete_Title").text();
    //        var rowCount = $('#DeliverySchTble >tbody >tr').length + 1;
    //        $('#DeliverySchTble tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
    // <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon" title="${deletetext}"></i></td>
    // <td class="sr_padding">${rowCount}</td>
    //<td style="display:none;">${$('#DeliverySchItemDDL').val()}</td>
    //<td >${$('#DeliverySchItemDDL option:selected').text()}</td>
    //<td >${moment($('#DeliveryDate').val()).format('DD-MM-YYYY')}</td>
    //<td class="num_right">${parseFloat($('#DeliverySchQty').val()).toFixed(QtyDecDigit)}</td>
    //<td style="display:none;">${$('#DeliveryDate').val()}</td>
    //<td>&nbsp;</td>

    //</tr>`);
    //        BindDelivertSchItemList();

    //        //$("#DeliverySchQty").val(parseFloat(0).toFixed(QtyDecDigit));
    //        $("#DeliveryDate").val("");

    //    }

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
    //    var ItemID = $(this).closest('tr')[0].cells[1].innerHTML;
    //    $("#DeliverySchItemDDL option[value=" + ItemID + "]").show();
    //    $('#DeliverySchItemDDL').val("0").prop('selected', true);
    //});
}
function OnChangeDelSchItm() {
    debugger;
    Cmn_OnChangeDelSchItm("RFQItmDetailsTbl", "N", "SNohf", "Itemname_", "RequisitionQuantity");
    //var QtyDecDigit = $("#QtyDigit").text();
    //if ($('#DeliverySchItemDDL').val() == "0") {
    //    $("#DeliverySchItemDDL").css("border-color", "red");
    //    $('#SpanDelSchItemName').text($("#valueReq").text());
    //    $("#SpanDelSchItemName").css("display", "block");
    //}
    //else {
    //    $("#DeliverySchItemDDL").css("border-color", "#ced4da");
    //    $("#SpanDelSchItemName").css("display", "none");

    //    var DelSchItem = $('#DeliverySchItemDDL').val();
    //    var ItemQty = parseFloat(0).toFixed(QtyDecDigit);
    //    var AllDelSchQty = parseFloat(0).toFixed(QtyDecDigit);
    //    $("#RFQItmDetailsTbl >tbody >tr").each(function () {
    //        debugger;
    //        var currentRow = $(this);
    //        var Sno = currentRow.find("#SNohf").val();
    //        var POItemQty = parseFloat(0).toFixed(QtyDecDigit);
    //        var RFQItemID = "";

    //        RFQItemID = currentRow.find("#Itemname_" + Sno).val();
    //        POItemQty = parseFloat(currentRow.find("#RequisitionQuantity").val()).toFixed(QtyDecDigit);

    //        if (RFQItemID === DelSchItem) {
    //            ItemQty = POItemQty;
    //        }
    //    });

    //    $("#DeliverySchTble >tbody >tr").each(function () {
    //        debugger;
    //        var currentRowDS = $(this);
    //        var DelSchItemtbl = currentRowDS.find("td:eq(2)").text();
    //        var DelSchQtyTbl = currentRowDS.find("td:eq(5)").text();
    //        if (DelSchItem == DelSchItemtbl) {
    //            AllDelSchQty = (parseFloat(AllDelSchQty) + parseFloat(DelSchQtyTbl)).toFixed(QtyDecDigit);
    //        }
    //    });
    //    $("#DeliveryDate").val("");
    //    $('#DeliverySchQty').val((parseFloat(ItemQty) - parseFloat(AllDelSchQty)).toFixed(QtyDecDigit));
    //}
}
function OnChangeDeliveryDate(DeliveryDate) {
    debugger;
    Cmn_OnChangeDeliveryDate(DeliveryDate);
    //var DeliDate = DeliveryDate.value;

    //var DelSchItemddl = $('#DeliverySchItemDDL').val();

    //$("#SpanDeliSchDeliDate").css("display", "none");
    //$("#DeliveryDate").css("border-color", "#ced4da");
    //$("#DeliverySchTble >tbody >tr").each(function () {
    //    debugger;
    //    var currentRow = $(this);
    //    var DelSchItem = currentRow.find("td:eq(2)").text();
    //    var DelSchDateTbl = currentRow.find("td:eq(6)").text();
    //    if (DelSchItemddl == DelSchItem && DeliDate == DelSchDateTbl) {
    //        $("#DeliveryDate").css("border-color", "red");
    //        $('#SpanDeliSchDeliDate').text($("#valueduplicate").text());
    //        $("#SpanDeliSchDeliDate").css("display", "block");
    //        $("#DeliveryDate").val("");
    //        return false;
    //    }
    //});

}

function ForwardBtnClick() {
    debugger;
    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var RFQDate = $("#txtRQFDate").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: RFQDate
            },
            success: function (data) {
                /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var RFQStatus = "";
                    RFQStatus = $('#RFQStatusCode').val().trim();
                    if (RFQStatus === "D" || RFQStatus === "F") {

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
                    /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
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
    var RFQNo = "";
    var RFQDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var SourceType = "";
    var SourcDocNo = "";
    var SourcDocDt = "";

    docid = $("#DocumentMenuId").val();
    RFQNo = $("#RFQNumber").val();
    RFQDate = $("#txtRQFDate").val();
    $("#hdDoc_No").val(RFQNo);
    Remarks = $("#fw_remarks").val();
    SourceType = $("#ddlSourceType").val();
    /*SourcDocNo = $("#ddlPRNumberList").val();*/
    SourcDocNo = $("#RfqPRNo").val();
    SourcDocDt = $("#txtPRDate").val();
    var ListFilterData = $("#PRData1").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (RFQNo + ',' + "Update" + ',' + WF_status1)
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && RFQNo != "" && RFQDate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(RFQNo, RFQDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/RequestForQuotation/ToRefreshByJS?ListFilterData=" + ListFilterData + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/RequestForQuotation/RFQApprove?RFQ_no=" + RFQNo + "&RFQ_date=" + RFQDate + "&src_doc_no=" + SourcDocNo + "&src_doc_dt=" + SourcDocDt + "&SrcType=" + SourceType + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData=" + ListFilterData + "&WF_status1=" + WF_status1;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && RFQNo != "" && RFQDate != "") {
             Cmn_InsertDocument_ForwardedDetail(RFQNo, RFQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/RequestForQuotation/ToRefreshByJS?ListFilterData=" + ListFilterData + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && RFQNo != "" && RFQDate != "") {
             Cmn_InsertDocument_ForwardedDetail(RFQNo, RFQDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/RequestForQuotation/ToRefreshByJS?ListFilterData=" + ListFilterData + "&TrancType=" + TrancType;
        }
    }

}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#RFQNumber").val();
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
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemRFQReqQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemRFQPRReqQty");

}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#Itemname_" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var source_type = $("#ddlSourceType  option:selected").val();
    var Doc_no = $("#RFQNumber").val();
    var Doc_dt = $("#txtRQFDate").val();
    var Sub_Quantity = 0;

    var NewArr = new Array();
    if (flag == "Quantity" || flag == "RFQPRReqQty") {

        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            if (source_type == "P") {
                List.sub_item_name = row.find('#subItemName').val();
            }
            var subQty = row.find('#subItemQty').val();
            if (subQty == "0") {
                subQty = "";
            }
            List.qty = subQty;
            NewArr.push(List);

        });

        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val();
    }
    if (source_type == "P") {
        var IsDisabled = "Y";
    }
    else {
        var IsDisabled = $("#DisableSubItem").val();
    }
    var hd_Status = $("#RFQStatusCode").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/RequestForQuotation/GetSubItemDetails",
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
    return Cmn_CheckValidations_forSubItems("RFQItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemRFQReqQty", "Y");
}
function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("RFQItmDetailsTbl", "SNohf", "Itemname_", "RequisitionQuantity", "SubItemRFQReqQty", "N");
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

function OnClickOrderTrackingBtn(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RFQ_no = currentrow.find("#rfq_no").text();
    var RFQ_dt = moment(currentrow.find("#Rfq_date").text()).format('YYYY-MM-DD');
    var DocumentMenuId = $("#DocumentMenuId").val();

    if (RFQ_no && RFQ_dt) {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/RequestForQuotation/GetRFQTrackingDetail",
            data: { RFQ_no: RFQ_no, RFQ_dt: RFQ_dt, DocumentMenuId: DocumentMenuId },
            success: function (data) {
                debugger;

                if (data == 'ErrorPage') return false;

                // Inject the modal content
                $("#RFQTracking").html(data);

                // Set hidden inputs
                $("#RFQNumber").val(RFQ_no);
                $("#RFQDate").val(RFQ_dt);

                // Initialize DataTable **after modal content is added**
                initializeRFQTrackingTable(RFQ_no, RFQ_dt);
            }
        });
    }
}

function DownloadExcel() {
    $('#DownloadExcel').click(function () {
        debugger;
        var RFQNumber = $("#RFQNumber").val();
        var RFQDate = moment($("#RFQDate").val()).format('YYYY-MM-DD');
        var searchValue = $("#RFQTrackingtbl_filter input[type=search]").val();

        // Get DataTable instance
        var table = $('#RFQTrackingtbl').DataTable();
        var settings = table.settings()[0];

        // Get pagination info
        var start = settings._iDisplayStart;
        var length = settings._iDisplayLength;

        // Get sorting info
        var order = settings.aaSorting[0]; // [columnIndex, direction]
        var sortColumnIndex = order ? order[0] : 0;
        var sortDirection = order ? order[1] : 'asc';
        var sortColumnName = settings.aoColumns[sortColumnIndex].data; // column name used in DataTable

        // Build query string
        var queryString = "?searchValue=" + encodeURIComponent(searchValue)
            + "&RFQNumber=" + encodeURIComponent(RFQNumber)
            + "&RFQDate=" + encodeURIComponent(RFQDate)
            + "&start=" + start
            + "&length=" + length
            + "&sortColumnName=" + encodeURIComponent(sortColumnName)
            + "&sortDirection=" + encodeURIComponent(sortDirection);

        // Trigger file download
        window.location.href = "/ApplicationLayer/RequestForQuotation/ExcelDownload" + queryString;
    });
}

