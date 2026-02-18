$(document).ready(function () {
    debugger;

    $('#PriceListTbl tbody').on('click', '.deleteIcon', function () {
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
        var ItemCode = $(this).closest('tr')[0].cells[2].children[0].children[0].value;
        //  ShowItemListItm(ItemCode);
        SerialNoAfterDelete();
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });

    $('#PriceGroupTbl tbody').on('click', '.deleteIcon', function () {
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
        var PriceGroupName = $(this).closest('tr')[0].cells[4].childNodes[0].value;//$(this).closest('tr')[0].cells[4].innerHTML;
        //var Sno = $(this).find("#SNohiddenfiled").val();
        //var PriceGroupId = $(this).find("#PriceGroupId" + Sno).val();
        $("#cust_pr_grp option[value=" + PriceGroupName + "]").show();
        $('#cust_pr_grp').val("0").prop('selected', true);
        //var SNo = $(this).closest('tr')[0].cells[2].children[0].value;
        SerialNoAfterDeleteonPriceGroup();
    });
   
//    BindDLLSOItemList();
  

    $('select[id="Itemname_1"]').bind('change', function (e) {
        debugger;
        BindSOItemList(e);
    });
    HidePriceGroup();
    var Doc_no = $("#List_no").val();
    if (Doc_no == "0") {
        $("#hdDoc_No").val("");
    }
    else {
        $("#hdDoc_No").val(Doc_no);
    }

});


/*---------------------WorkFlow------------------------*/
function CmnGetWorkFlowDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var list_No = clickedrow.children("#List_no").text();
    var List_Date = clickedrow.children("#Create_DT").text();
    var Doc_Menu_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Doc_Menu_id);
    GetWorkFlowDetails(list_No, List_Date, Doc_id);
    //var a = 1;
}
function ForwardBtnClick() {
    debugger;
    try {
        var Doc_Status = "";
        Doc_Status = $('#hdQCstatuscode').val().trim();
        if (Doc_Status === "D" || Doc_Status === "F") {

            if ($("#hd_nextlevel").val() === "0") {
                $("#Btn_Forward").attr("data-target", "");
            }
            else {
                $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                $("#Btn_Approve").attr("data-target", "");
            }
            var Doc_Menu_ID = $("#DocumentMenuId").val();
            $("#OKBtn_FW").attr("data-dismiss", "modal");

            Cmn_GetForwarderList(Doc_Menu_ID);

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
}
function BindData() {
    debugger;

    var PLItemListData = JSON.parse(sessionStorage.getItem("itemList"));
    if (PLItemListData != null) {
        if (PLItemListData.length > 0) {
            $("#PriceListTbl >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#PriceListTbl >tbody >tr").length) {
                    return false;
                }
                $("#Itemname_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#SOItemName").text()}' data-uom='${$("#SOItemUOM").text()}'></optgroup>`);
                for (var i = 0; i < PLItemListData.length; i++) {
                    $('#Textddl' + rowid).append(`<option data-uom="${PLItemListData[i].uom_name}" value="${PLItemListData[i].Item_id}">${PLItemListData[i].Item_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#Itemname_" + rowid).select2({
                    templateResult: function (data) {
                        var selected = $("#Itemname_" + rowid).val();
                        if (check(data, selected, "#PriceListTbl", "#SNohf", "#Itemname_") == true) {
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
    $("#PriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var ItmID = currentRow.find("#hfItemID").val();
        var rowid = currentRow.find("#SNohf").val();
        var ItemID = '#Itemname_' + rowid;
        if (ItmID != '0' && ItmID != "") {
            currentRow.find("#Itemname_" + rowid).val(ItmID).trigger('change.select2');
        }

    });



    //$("#PriceListTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var ItmID = currentRow.find("#hfItemID").val();
    //    var rowid = currentRow.find("#SNohf").val();

    //    var ItemCode;
    //    ItemCode = ItmID;

    //    if (ItemCode != '0' && ItemCode != "") {

    //            $("#PriceListTbl >tbody >tr").each(function (i, row) {
    //                debugger;
    //                var currentRowD = $(this);
    //                var ItemCodeD;
    //                ItemCodeD = currentRowD.find("#hfItemID").val();
    //                var rowidD = currentRowD.find("#SNohf").val();
    //                if (ItemCodeD != '0' && ItemCodeD != "") {
    //                    if (currentRow.find("#Itemname_" + rowidD).val() != ItemCode) {
    //                        $("#Itemname_" + rowid + " option[value=" + ItemCodeD + "]").select2().hide();
    //                    }
    //                }
    //            })
    //        }
    //});
}
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var List_no = "";
    var Create_date = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_status1 = $("#WF_status1").val()
    Remarks = $("#fw_remarks").val();
    List_no = $("#List_no").val();
    Create_date = $("#Create_DT").val();
    docid = $("#DocumentMenuId").val();
    var DashBord = (docid + ',' + List_no + ',' + WF_status1)
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
    if (fwchkval === "Forward") {
        if (fwchkval != "" && List_no != "" && Create_date != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(List_no, Create_date, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/CustomerPriceList/ToRefreshByJS?DashBord=" + DashBord;
        }
    }
    if (fwchkval === "Approve") {
        var list = [{ list_no: List_no, create_dt: Create_date, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/CustomerPriceList/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&WF_status1=" + WF_status1 + "&docid=" + docid;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && List_no != "" && Create_date != "") {
            Cmn_InsertDocument_ForwardedDetail(List_no, Create_date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/CustomerPriceList/ToRefreshByJS?DashBord=" + DashBord;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && List_no != "" && Create_date != "") {
            Cmn_InsertDocument_ForwardedDetail(List_no, Create_date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/CustomerPriceList/ToRefreshByJS?DashBord=" + DashBord;
        }
    }
}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#List_no").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
    //var Doc_Menu_ID = $("#DocumentMenuId").val();
    //var List_No = $("#List_No").val();
    //debugger;
    //if (List_No != "" && List_No != null && Doc_ID != "" && Doc_ID != null)
    //    Cmn_GetForwarderHistoryList(List_No, Doc_Menu_ID);
}
function OtherFunctions(StatusC, StatusName) {
    debugger;
}
/*---------------------WorkFlow------------------------*/
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#PriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
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
function SerialNoAfterDeleteonPriceGroup() {
    var SerialNo = 0;
    $("#PriceGroupTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#_rowIdx").text(SerialNo);

    });
}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#PriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function CheckFormValidation() {
    debugger
    var rowcount1 = $('#PriceListTbl tbody tr').length;
    var rowcount = $('#PriceGroupTbl tbody tr').length;
    var ValidationFlag = true;
    var PriceListName = $('#PriceListName').val();

    var Flag = 'N';

    if ($('#PriceListName').val() == '') {
        $("#PriceListName").attr("style", "border-color: #ff0000;");
        $("#span_prlist_name").text($("#valueReq").text());
        $("#span_prlist_name").css("display", "block");
        Flag = 'Y';
    }
    if ($('#txtFromdate').val() == '') {
        $("#txtFromdate").attr("style", "border-color: #ff0000;");
        $("#span_fromdate").text($("#valueReq").text());
        $("#span_fromdate").css("display", "block");
        Flag = 'Y';
    }
    if ($('#txtTodate').val() == '') {
        $("#txtTodate").attr("style", "border-color: #ff0000;");
        $("#span_todate").text($("#valueReq").text());
        $("#span_todate").css("display", "block");
        Flag = 'Y';
    }
    if (Flag == 'N') {
        var FromDate = $("#txtFromdate").val();
        var ToDate = $("#txtTodate").val();
        if ((FromDate != null && FromDate != "") && (ToDate != null && ToDate != "")) {
            if (FromDate > ToDate) {
                swal("", $("#fromtodatemsg").text(), "warning");
                Flag = 'Y';
            }
        }
    }

    if (Flag == 'Y') {
        ValidationFlag = false;
        return false;
    }
    else {
        ValidationFlag = true;
    }
    if (CheckItemValidations() == false) {
        return false;
    }

    if (rowcount1 > 0) {
        /*   return true;*/
    }
    else {

        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    if (rowcount > 0) {
        /* return true;*/
    }
    else {
        swal("", $("#nopricegroupselectedmsg").text(), "warning");
        return false;
    }
    if (ValidationFlag == true) {
        return true;
    }
    else {
        return false;
    }

}
//function CheckItemValidations() {
//    debugger
//    showLoader();
//    var RateDecDigit = $("#RateDigit").text();
//    var QtyDecDigit = $("#QtyDigit").text();
//    var ErrorFlag = "y";
//    $("#PriceListTbl >tbody >tr").each(function () {
//        var currentRow = $(this);
//        var Sno = currentRow.find("#SNohf").val();

//        if (currentRow.find("#Itemname_" + Sno).val() == "0") {
//            currentRow.find("#ItemNameError").text($("#valueReq").text());
//            currentRow.find("#ItemNameError").css("display", "block");
//            currentRow.find("[aria-labelledby='select2-Itemname_" + Sno + "-container']").css("border", "1px solid red");

//            ErrorFlag = "Y";
//        }
//        else {
//            currentRow.find("#ItemNameError").css("display", "none");
//            currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
//        }

//        if (currentRow.find("#SalePrice").val() == "") {
//            currentRow.find("#SalePriceError").text($("#valueReq").text());
//            currentRow.find("#SalePriceError").css("display", "block");
//            currentRow.find("#SalePrice").css("border-color", "red");
//            ErrorFlag = "Y";
//        }
//        else {
//            currentRow.find("#SalePriceError").css("display", "none");
//            currentRow.find("#SalePrice").css("border-color", "#ced4da");
//        }
//        if (currentRow.find("#SalePrice").val() != "") {
//            if (parseFloat(currentRow.find("#SalePrice").val()).toFixed(RateDecDigit) == 0) {
//                currentRow.find("#SalePriceError").text($("#valueReq").text());
//                currentRow.find("#SalePriceError").css("display", "block");
//                currentRow.find("#SalePrice").css("border-color", "red");
//                ErrorFlag = "Y";
//            }
//            else {
//                currentRow.find("#SalePriceError").css("display", "none");
//                currentRow.find("#SalePrice").css("border-color", "#ced4da");
//            }
//        }
//    });
//    if (ErrorFlag == "Y") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function CheckItemValidations() {
    debugger;
    showLoader();

    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "y";

    var rows = $("#PriceListTbl >tbody >tr");
    var index = 0;
    var Size = 200; 

    function PriceListTblData() {
        var end = Math.min(index + Size, rows.length);
        for (; index < end; index++) {

            var currentRow = $(rows[index]);
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
            if (currentRow.find("#SalePrice").val() == "") {
                currentRow.find("#SalePriceError").text($("#valueReq").text());
                currentRow.find("#SalePriceError").css("display", "block");
                currentRow.find("#SalePrice").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#SalePriceError").css("display", "none");
                currentRow.find("#SalePrice").css("border-color", "#ced4da");
            }
            if (currentRow.find("#SalePrice").val() != "") {
                if (parseFloat(currentRow.find("#SalePrice").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#SalePriceError").text($("#valueReq").text());
                    currentRow.find("#SalePriceError").css("display", "block");
                    currentRow.find("#SalePrice").css("border-color", "red");
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#SalePriceError").css("display", "none");
                    currentRow.find("#SalePrice").css("border-color", "#ced4da");
                }
            }
        }
        if (index < rows.length) {
            setTimeout(PriceListTblData, 0);
        }
    }
    PriceListTblData();
    return ErrorFlag !== "Y";
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
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var ItmCode = "";
    // var ItmName = "";

    ItmCode = clickedrow.find("#Itemname_" + Sno).val();
    //ItmName = clickedrow.find("#Itemname_" + Sno + " option:selected").text()
    ItemInfoBtnClick(ItmCode);

}
function EditBtnClick() {
    debugger;
    sessionStorage.removeItem("Edittable");
    sessionStorage.setItem("Edittable", "Edit");
}
var missingItems = [];
function InsertPriceListDetail() {

    if ($("#hdnsavebtn").val() === "AllreadyclickSaveBtn") {
        return true;
    }
    showLoader();
    setTimeout(function () {
        var result = InsertPriceListDetail1();

        if (result === true) {
            $("#btn_save").off("click._async").trigger("click._async");
        } else {
            hideLoader();
        }
    }, 0);
    return false;
}
function InsertPriceListDetail1() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 09-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
    }
    if (missingItems.length > 0) {
        swal("", "Item not found ", "warning");
        return false;
    }
    var DecDigit = $("#ValDigit").text();
    var INSDTransType = sessionStorage.getItem("INSTransType");

    if (CheckFormValidation() == false) {
        return false;
    }
    if (CheckItemValidations() == false) {
        return false;
    }
    // if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
    var TransType = "";
    if (INSDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var FinalItemDetail = [];
    var FinalPriceGroupDetail = [];
    var edit = sessionStorage.getItem("Edittable");
    if (edit == 'Edit') {
        SetSessionData();
    }
    FinalItemDetail = InsertPriceListItemDetails();
    FinalPriceGroupDetail = InsertPriceGrouptDetails();

    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);

    var PriceGroupDt = JSON.stringify(FinalPriceGroupDetail);
    $('#hdPriceGroupDetailList').val(PriceGroupDt);

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
    // }
    //else {
    //    //alert("Check network");
    //    return false;
    //}
};
function InsertPriceListItemDetails() {

    var ItemDetailList = new Array();
    $("#PriceListTbl TBODY TR").each(function () {
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var ItemList = {};
        ItemList.ItemID = currentRow.find("#Itemname_" + rowid).val();
        ItemList.ItemName = currentRow.find("#Itemname_" + rowid + " option:selected").text();
        ItemList.UOMID = currentRow.find("#UOMID").val();
        ItemList.uom_name = currentRow.find("#UOM").val();
        ItemList.SalePrice = currentRow.find("#SalePrice").val();
        ItemList.sale_price_inc_tax = currentRow.find("#sale_price_inc_tax").val();
        ItemList.DiscPerc = currentRow.find("#DiscountPerc").val();
        ItemList.DiscMRP = currentRow.find("#MRPDiscount").val();
        ItemList.EffectPrice = currentRow.find("#EffectivePrice").val();
        ItemList.ItemRemarks = currentRow.find('#remarks').val();

        ItemDetailList.push(ItemList);
    });
    return ItemDetailList;
};
function RemoveSessionData() {
    sessionStorage.removeItem("PriceGroupDetails");
    sessionStorage.removeItem("Edittable");
}
function InsertPriceGrouptDetails() {
    var PriceGroupList = [];
    var FGroupDetails = JSON.parse(sessionStorage.getItem("PriceGroupDetails"));
    if (FGroupDetails != null) {
        if (FGroupDetails.length > 0) {
            for (i = 0; i < FGroupDetails.length; i++) {
                var PriceGroupId = FGroupDetails[i].PriceGroupId;
                var PriceGroupName = FGroupDetails[i].PriceGroupName;

                PriceGroupList.push({ PriceGroupId: PriceGroupId, PriceGroupName: PriceGroupName });
            }
        }
    }
    else {
        $("#PriceGroupTbl TBODY TR").each(function () {
            //var Sno = 0;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var GroupList = {};
            GroupList.PriceGroupId = currentRow.find("#PriceGroupId" + Sno).val();
            GroupList.PriceGroupName = currentRow.find("#setup_val").text();
            PriceGroupList.push(GroupList);
        });
    }

    sessionStorage.removeItem("PriceGroupDetails");
    sessionStorage.setItem("PriceGroupDetails", JSON.stringify(PriceGroupList));

    return PriceGroupList;
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
function AddNewRow() {
    debugger;
    $('#Partial_cpl_upload').modal('hide');
    $('[data-target="#Partial_cpl_upload"]').removeAttr('data-toggle').removeAttr('data-target').addClass('disabled');
    $("#uploadBtn").attr("disabled", true);
    $("#uploadBtn").css("filter", "grayscale(100%)");

    $("#downloadTemplateBtn").removeAttr("href");
    $("#downloadTemplateBtn").attr("disabled", true);
    $("#downloadTemplateBtn").css("filter", "grayscale(100%)");
    var rowIdx = 0;
    debugger;
    var rowCount = $('#PriceListTbl >tbody >tr').length + 1;
    var RowNo = 0;

    $("#PriceListTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#PriceListTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td><div class="col-sm-11 lpo_form" style="padding:0px;"><select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" 
onchange ="OnChangeSOItemName(${RowNo},event)"></select>
<input  type="hidden" id="hfItemID" value="" /><span id="ItemNameError" class="error-message is-visible"></span></div>
<div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
<img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button></div>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}" disabled><input id="UOMID" type="hidden" /></td>
<td><div class="lpo_form"><input id="sale_price_inc_tax" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="sale_price_inc_tax"  placeholder="0000.00"  ><span id="sale_price_inc_taxError" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="SalePrice" class="form-control num_right" autocomplete="off"  onchange ="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice"  placeholder="0000.00"  ><span id="SalePriceError" class="error-message is-visible"></span></div></td>
<td><div class="lpo_form"><input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" placeholder="0000.00"  ></div></td>
<td><input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);"  placeholder="0000.00"  ></td>
 <td><input id="EffectivePrice" onkeypress= RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" placeholder="0000.00"  disabled></td>
<td><textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "100" placeholder="${$("#span_remarks").text()}"></textarea></td>
 <td> <div class="col-md-3 col-sm-12"></div> <div class="col-md-6 col-sm-12"><a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false"><div class="plus_icon3"> <i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i> </div></a></div> <div class="col-md-3 col-sm-12"></div></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
</tr>`);
    BindSOItmList(RowNo);
};
function AddPriceGroup() {
    var rowIdx = 0;
    debugger;
    var rowCount = $('#PriceGroupTbl >tbody >tr').length;
    var RowNo = 0;
    $("#PriceGroupTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    rowCount = parseInt(rowCount) + 1;
    var PriceGrpID = $("#cust_pr_grp").val();
    if (PriceGrpID != "0") {
        $('#PriceGroupTbl tbody').append(`<tr id="R${++rowIdx}">
            <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i></td>
            <td  id="_rowIdx" class="sr_padding">${rowCount}</td>
 <td style="display: none;"><input type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
            <td id="setup_val">${$("#cust_pr_grp option:selected").text()}</td>
            <td style="display: none;"><input  type="hidden" id="PriceGroupId${RowNo}" value="${$("#cust_pr_grp").val()}" /></td>

        </tr>`);
        $("#cust_pr_grp option[value=" + $("#cust_pr_grp").val() + "]").hide();
        $('#cust_pr_grp').val("0").prop('selected', true);

    }
    if ($('#cust_pr_grp').val() == '') {
        $("#cust_pr_grp").attr("style", "border-color: #ff0000;");
        $("#vmcust_pr_grp").text($("#valueReq").text());
        $("#vmcust_pr_grp").css("display", "block");
        Flag = 'Y';
    }
    else {
        $("#vmcust_pr_grp").css("display", "none");
        $("#cust_pr_grp").css("border-color", "#ced4da");
        $("vmcust_pr_grp").css("border-color", "#ced4da");
    }
};
function HidePriceGroup() {
    var PriceGroupList = [];
    $("#PriceGroupTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        PriceGroupId = currentRow.find("#PriceGroupId").val();

        if (currentRow.find("#cust_pr_grp").val() != PriceGroupId) {
            $("#cust_pr_grp option[value=" + PriceGroupId + "]").select2().hide();
        }
        //$("#cust_pr_grp option[value=" + $("#cust_pr_grp").val() + "]").hide();
        //$('#cust_pr_grp').val("0").prop('selected', true);
    });
}
function BindDLLSOItemList() {
    debugger;

    BindItemList("#Itemname_", "1", "#PriceListTbl", "#SNohf", "ForOneRow", "CPL");

    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/LSODetail/GetSOItemList",
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
    //                }
    //            }
    //        },
    //    });
}
function BindSOItmList(ID) {
    debugger;
    BindItemList("#Itemname_", ID, "#PriceListTbl", "#SNohf", "ForOneRow", "CPL");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/LSODetail/GetSOItemList",
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
    //                    HideSOItemListItm(ID);
    //                }
    //            }
    //        },
    //    });
}
function BindSOItemList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#SNohf").val();

    Itm_ID = clickedrow.find("#Itemname_" + SNo).val();

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
    //HideSOItemListItm(SNo);
    //HideSelectedItem("#Itemname_", SNo, "#PriceListTbl", "#SNohf");
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "sale")

}
function OnChangeSOItemName(RowID, e) {
    debugger;
    BindSOItemList(e);
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null && FromDate != "") && (ToDate != null && ToDate != "")) {
        if (FromDate > ToDate) {

            //var now = new Date();
            //var month = (now.getMonth() + 1);
            //var day = now.getDate();
            //if (month < 10)
            //    month = "0" + month;
            //if (day < 10)
            //    day = "0" + day;
            //var today = now.getFullYear() + '-' + month + '-' + day;
            //$("#txtTodate").val(today);

            //var fromDate = new Date($("#hdFromdate").val());

            //var month = (fromDate.getMonth() + 1);
            //var day = fromDate.getDate();
            //if (month < 10)
            //    month = "0" + month;
            //if (day < 10)
            //    day = "0" + day;
            //var today = fromDate.getFullYear() + '-' + day + '-' + month;
            //$("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    OnchangeFromDate();
    OnchangeToDate();

}
function OnclickSalePrice(e) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohf").val();
    var ItemName;
    var SalePrice;
    var DisAmt;
    var DisPer;
    var OrderQty = 1;

    ItemName = clickedrow.find("#Itemname_" + Sno).val();
    SalePrice = clickedrow.find("#SalePrice").val();
    //DisAmt = clickedrow.find("#Discountunit").val();
    DisAmt = clickedrow.find("#MRPDiscount").val();
    DisPer = clickedrow.find("#DiscountPerc").val();


    if (ItemName == "0") {

        clickedrow.find("#ItemNameError").text($("#valueReq").text());
        clickedrow.find("#ItemNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
        clickedrow.find("#SalePrice").val("");

        return false;
    }
    else {

        clickedrow.find("#ItemNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");


    }
    if (SalePrice == "" || SalePrice == "0") {

        clickedrow.find("#SalePriceError").text($("#valueReq").text());
        clickedrow.find("#SalePriceError").css("display", "block");
        clickedrow.find("#SalePrice").css("border-color", "red");
        clickedrow.find("#SalePrice").val("");

        return false;
    }
    else {

        clickedrow.find("#SalePriceError").css("display", "none");
        clickedrow.find("#SalePrice").css("border-color", "#ced4da");

    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (SalePrice !== 0 || SalePrice !== null || SalePrice !== "")) {
        var FAmt = OrderQty * SalePrice;
        var FinVal = parseFloat(FAmt).toFixed(RateDecDigit);

        clickedrow.find("#EffectivePrice").val(FinVal);

    }
    //if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "" && DisPer !== 0 && DisPer !== "") {
    //    CalculateDisPercent(e);
    //}
    //if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "" && DisAmt !== 0 && DisAmt !== "") {
    //    CalculateDisAmt(e);
    //}
    CalculateEffectiveAmt(e);

    clickedrow.find("#SalePrice").val(parseFloat(SalePrice).toFixed(RateDecDigit));

}
function OnchangeMRPDisc(e) {

    CalculateEffectiveAmt(e);
}
function CalculateEffectiveAmt(e) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SalePrice;
    var MRPDisc;
    var DisPer;

    SalePrice = clickedrow.find("#SalePrice").val();
    MRPDisc = clickedrow.find("#MRPDiscount").val();
    MRPDisc = CheckNullNumber(MRPDisc);
    DisPer = clickedrow.find("#DiscountPerc").val();
    DisPer = CheckNullNumber(DisPer);
    if (DisPer != 0) {
        clickedrow.find("#DiscountPerc").val(parseFloat(DisPer).toFixed(2));
    }
    else {
        clickedrow.find("#DiscountPerc").val("");
    }
    if (MRPDisc != 0) {
        clickedrow.find("#MRPDiscount").val(parseFloat(MRPDisc).toFixed(2));
    }
    else {
        clickedrow.find("#MRPDiscount").val("");
    }



    if (SalePrice != 0 && SalePrice != "") {
        debugger;
        if (CheckNullNumber(MRPDisc) > 0) {
            var FAmt = (SalePrice * MRPDisc) / 100;
            var GAmt = SalePrice - FAmt;
            var FinGVal = parseFloat(GAmt).toFixed(RateDecDigit);
            //var FinDisVal = parseFloat(DisVal).toFixed(RateDecDigit);
            if (CheckNullNumber(DisPer) > 0) {
                FinGVal = FinGVal - (FinGVal * DisPer) / 100;
            }
            clickedrow.find("#EffectivePrice").val(parseFloat(FinGVal).toFixed(RateDecDigit));
        }
        else {
            if (CheckNullNumber(DisPer) > 0) {
                var GAmt = SalePrice - (SalePrice * DisPer) / 100;
                clickedrow.find("#EffectivePrice").val(parseFloat(GAmt).toFixed(RateDecDigit));
            }

        }
        if (CheckNullNumber(MRPDisc) == 0 && CheckNullNumber(DisPer) == 0) {
            clickedrow.find("#EffectivePrice").val(SalePrice).toFixed(RateDecDigit);
        }

    }
    else {
        clickedrow.find("#EffectivePrice").val(0).toFixed(RateDecDigit);
    }

}

function CalculateDisPercent(e) {
    debugger;
    //var RateDecDigit = $("#RateDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    //var Sno = clickedrow.find("#SNohf").val();
    //var ItemName;
    //var OrderQty = 1;
    //var SalePrice;
    //var DisPer;

    //ItemName = clickedrow.find("#Itemname_" + Sno).val();
    //SalePrice = clickedrow.find("#SalePrice").val();
    //DisPer = clickedrow.find("#DiscountPerc").val();


    //if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "" && DisPer !== 0 && DisPer !== "") {
    //    var FAmt = (SalePrice * DisPer) / 100;
    //    var GAmt = OrderQty * (SalePrice - FAmt);
    //    var DisVal = OrderQty * FAmt;
    //    var FinGVal = parseFloat(GAmt).toFixed(RateDecDigit);
    //    var FinDisVal = parseFloat(DisVal).toFixed(RateDecDigit);

    //    clickedrow.find("#EffectivePrice").val(FinGVal);
    //    clickedrow.find("#DiscountPerc").val(parseFloat(DisPer).toFixed(RateDecDigit));

    //    clickedrow.find("#Discountunit").prop("readonly", true);
    //    clickedrow.find("#Discountunit").val("");
    //}
    //else {
    //    if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "") {
    //        var FAmt = OrderQty * SalePrice;
    //        var FinVal = parseFloat(FAmt).toFixed(RateDecDigit);

    //        clickedrow.find("#EffectivePrice").val(FinVal);
    //        clickedrow.find("#Discountunit").val(parseFloat(0).toFixed(RateDecDigit));

    //    }
    //    clickedrow.find("#Discountunit").prop("readonly", false);
    //}
    CalculateEffectiveAmt(e);
}
//function CalculateDisAmt(e) {
//    debugger;
//    var RateDecDigit = $("#RateDigit").text();
//    var clickedrow = $(e.target).closest("tr");
//    var Sno = clickedrow.find("#SNohf").val();
//    var ItemName;
//    var OrderQty = 1;
//    var SalePrice;
//    var DisAmt;

//    ItemName = clickedrow.find("#Itemname_" + Sno).val();
//    SalePrice = clickedrow.find("#SalePrice").val();
//    //DisAmt = clickedrow.find("#Discountunit").val();
//    DisAmt = clickedrow.find("#MRPDiscount").val();

//    if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "" && DisAmt !== 0 && DisAmt !== "") {
//        if (Math.fround(SalePrice) > Math.fround(DisAmt)) {
//            var FRate = (SalePrice - DisAmt);
//            var GAmt = OrderQty * FRate;
//            var DisVal = OrderQty * DisAmt;
//            var FinGVal = parseFloat(GAmt).toFixed(RateDecDigit);

//            clickedrow.find("#EffectivePrice").val(FinGVal);;
//            clickedrow.find("#DiscountPerc").prop("readonly", true);
//            clickedrow.find("#DiscountPerc").val("");

//        }
//        else {

//            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
//            clickedrow.find("#item_disc_amtError").css("display", "block");
//            clickedrow.find("#item_disc_amt").css("border-color", "red");
//            clickedrow.find("#item_disc_amt").val('');
//            clickedrow.find("#item_disc_val").val('');


//        }

//        //clickedrow.find("#Discountunit").val(parseFloat(DisAmt).toFixed(RateDecDigit));
//        clickedrow.find("#MRPDiscount").val(parseFloat(DisAmt).toFixed(RateDecDigit));

//    }
//    else {
//        if (OrderQty !== 0 && OrderQty !== "" && SalePrice !== 0 && SalePrice !== "") {
//            var FAmt = OrderQty * SalePrice;
//            var FinVal = parseFloat(FAmt).toFixed(RateDecDigit);
//            clickedrow.find("#EffectivePrice").val(FinVal);
//        }
//        clickedrow.find("#DiscountPerc").prop("readonly", false);
//    }
//}
function PercentValueonly(el, evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    }
}
function RateFloatValueonly(el, evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    if (Cmn_FloatValueonly(el, evt, "#RateDigit") == false) {
        return false;
    }
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    if (RowNo == "1") {
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        clickedrow.find("#item_rateError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
        clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
        clickedrow.find("#item_rateError" + RowNo).css("display", "none");
        clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");
    }

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function OnchangePriceListName() {
    debugger
    var PriceListName = $('#PriceListName').val();
    if (PriceListName == "") {
        $("#PriceListName").attr("style", "border-color: #ff0000;");
        $("#span_prlist_name").text($("#valueReq").text());
        $("#span_prlist_name").css("display", "block");
    }
    else {
        $("#span_prlist_name").css("display", "none");
        $("#PriceListName").attr("style", "border-color: #ced4da;");
        $("#span_prlist_name").text("");
        $("#vmlist_name").text("");
    }
}
function OnchangeFromDate() {
    debugger;
    if ($('#txtFromdate').val() == '') {
        $("#txtFromdate").attr("style", "border-color: #ff0000;");
        $("#span_fromdate").text($("#valueReq").text());
        $("#span_fromdate").css("display", "block");
    }
    else {
        $("#span_fromdate").css("display", "none");
        $("#txtFromdate").attr("style", "border-color: #ced4da;");
        $("#span_fromdate").text("");
        $('#hdFromdate').val($("#txtFromdate").val());
    }
}
function OnchangeToDate() {
    debugger;
    if ($('#txtTodate').val() == '') {
        $("#txtTodate").attr("style", "border-color: #ff0000;");
        $("#span_todate").text($("#valueReq").text());
        $("#span_todate").css("display", "block");
    }
    else {
        $("#span_todate").css("display", "none");
        $("#txtTodate").attr("style", "border-color: #ced4da;");
        $("#span_todate").text("");
    }
}
function SetSessionData() {
    var PriceGroupList = [];
    $("#PriceGroupTbl TBODY TR").each(function () {
        debugger;
        //var Sno = 0;
        var currentRow = $(this);
        var GroupList = {};
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (Sno == "1") {

            GroupList.PriceGroupId = currentRow.find("#PriceGroupId").val();
            GroupList.PriceGroupName = currentRow.find("#setup_val").text();
        }
        else {
            GroupList.PriceGroupId = currentRow.find("#PriceGroupId" + Sno).val();
            GroupList.PriceGroupName = currentRow.find("#setup_val" + Sno).text();
        }

        PriceGroupList.push(GroupList);
    });
    sessionStorage.removeItem("PriceGroupDetails");
    sessionStorage.setItem("PriceGroupDetails", JSON.stringify(PriceGroupList));

}
function approveonclick() { /**Added this Condition by Nitesh 09-01-2024 for Disable Approve btn after one Click**/
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

function PrcHistryDtlOnClk(e) {
    debugger;
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohf").val();
    var ProductNm = clickdRow.find("#Itemname_" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#hfItemID").val();
    var UOM = clickdRow.find("#UOM").val();
    var SalePrice = clickdRow.find("#SalePrice").val();
    var MRPDiscount = clickdRow.find("#MRPDiscount").val();
    var DiscountPerc = clickdRow.find("#DiscountPerc").val();
    var EffectivePrice = clickdRow.find("#EffectivePrice").val();
    var Doc_no = $("#List_no").val();
    var Doc_dt = $("#txtFromdate").val();


    var hd_Status = $("#hdQCstatuscode").val();
    /*hd_Status = IsNull(hd_Status, "").trim();*/

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CustomerPriceList/GetPriceHistoryDetails",
        data: {
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            Item_id: ProductId,
            /*hd_Status: hd_Status*/
        },
        success: function (data) {
            debugger;
            $("#History_popup").html(data);
            $("#CPLPopupItemName").val(ProductNm);
            $("#CPLPopupItemId").val(ProductId);
            $("#CPLPopupUOM").val(UOM);

        }
    });

}
function FilterItemDetail(e) {//added by Prakash Kumar on 15-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "PriceListTbl", [{ "FieldId": "Itemname_", "FieldType": "select", "SrNo": "SNohf" }]);
}
// function UploadCPLData() {
//    debugger;
//    showLoader();
//    const selectedFile = document.getElementById('cplfile').files[0];
    
//    if (!selectedFile) {
//        alert('Please select an Excel file first.');
//        hideLoader();
//        return;
//    }
//    const reader = new FileReader();
//    reader.onload = function (e) {
//        debugger;
//        const data = new Uint8Array(e.target.result);
//        const workbook = XLSX.read(data, { type: 'array' });
//        // Sheet 1 (Main Price List)
//        const sheet1Name = workbook.SheetNames[0];
//        const worksheet1 = workbook.Sheets[sheet1Name];
//        const jsonPriceList = XLSX.utils.sheet_to_json(worksheet1, { header: 1 }); // raw matrix

//        // Sheet 2 (Item Details)
//        const sheet2Name = workbook.SheetNames[1];
//        const worksheet2 = workbook.Sheets[sheet2Name];
//        const jsonItemDetail = XLSX.utils.sheet_to_json(worksheet2, { header: 1 });

//        if (jsonPriceList.length <= 1) {
//            swal("", 'Excel file is empty. Please fill data in excel file and try again', "warning");
//            hideLoader();
//            return;
//        }

//        let rowIdx = $('#PriceListTbl >tbody >tr').length;
//        let RowNo = 0;

//        $("#PriceListTbl >tbody >tr").each(function () {
//            const currentRow = $(this);
//            RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
//        });

//        if (!RowNo) RowNo = 1;
//        let seenItems = new Set();
//        for (let i = 1; i < jsonPriceList.length; i++) { // skip header row
//            const row = jsonPriceList[i];

//            //const itemName = row[0] || '';
//            const itemName = row[0] ? row[0].toString().trim() : '';
//            if (!itemName) continue;
//            // Duplicate check
//            if (seenItems.has(itemName)) {
//                // alert(`Duplicate item found in Excel: "${itemName}". Please remove duplicates and try again.`);
//                swal("", 'Duplicate item found in Excel', "warning");
//                $('#PriceListTbl tbody').html('');
//                hideLoader();
//                return;
//            }
//            seenItems.add(itemName); // Add to seen list

//            const salePrice = row[1] ? parseFloat(row[1]).toFixed(3) : '';

//            //const mrpDiscount = row[3] || '';
//            // const discountPerc = row[4] || '';
//            const mrpDiscount = row[2] ? parseFloat(row[2]).toFixed(2) : '';
//            const discountPerc = row[3] ? parseFloat(row[3]).toFixed(2) : '';
//            const remarks = row[4] || '';
//           // var ItemDetail = getItemDetailsByName(itemName, jsonItemDetail);
//            var ItemDetail = getItemDetailsByName(itemName, jsonItemDetail) || ["", "", ""];
//            let rowClass = "";
//            if (!ItemDetail || ItemDetail.length === 0 || !ItemDetail[0]) {
//               // swal("", 'Item not found in Item Detail sheet can not be uploaded' + itemName, "warning");
//                missingItems.push(itemName);
//                rowClass = "highlight_tr";
//            }
//            var FinVal = salePrice * mrpDiscount / 100;
//            FinVal2 = (salePrice - FinVal).toFixed(2);
//            if (discountPerc > 0) {
//                FinVal2 = (FinVal2 - (FinVal2 * discountPerc) / 100).toFixed(3);
//            }
//              $('#PriceListTbl tbody').append(`
//                <tr id="R${++rowIdx}" class="${rowClass}">
//                    <td class=" red center">
//                        <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i>
//                    </td>
//                    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
//                    <td>
//                        <div class="col-sm-11 lpo_form" style="padding:0px;" onclick="OnClickDropdown(${RowNo})">
//                            <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" value="${itemName}" onchange="OnChangeSOItemName(${RowNo},event)">
//                                <option  value="${ItemDetail[0]}">${itemName}</option>
//                            </select>
//                            <input type="hidden" id="hfItemID" value="${ItemDetail[0]}" />
//                            <span id="ItemNameError" class="error-message is-visible"></span>
//                        </div>
//                        <div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;">
//                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
//                                <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">
//                            </button>
//                        </div>
//                    </td>
//                    <td>
//                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${ItemDetail[1]}" placeholder="${$("#ItemUOM").text()}" disabled>
//                        <input id="UOMID" type="hidden" value="${ItemDetail[2]}" />
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="SalePrice" class="form-control num_right" autocomplete="off" onchange="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice" value="${salePrice}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" value="${mrpDiscount}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" value="${discountPerc}" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);" placeholder="0000.00">
//                    </td>
//                    <td>
//                        <input id="EffectivePrice" onkeypress="return RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" value="${FinVal2}" placeholder="0000.00" disabled>
//                    </td>
//                    <td>
//                        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}">${remarks}</textarea>
//                    </td>
//                    <td>
//                        <div class="col-md-3 col-sm-12"></div>
//                        <div class="col-md-6 col-sm-12">
//                            <a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false">
//                                <div class="plus_icon3"><i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i></div>
//                            </a>
//                        </div>
//                        <div class="col-md-3 col-sm-12"></div>
//                    </td>
//                    <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
//                </tr>
//            `);
//           // BindSOItmList(RowNo);
//            RowNo++;
//        }
//        hideLoader();
//        if (missingItems.length > 0) {
//           // swal("", "Invalid item name in upload sheet\n\n" + missingItems.join("\n"), "warning");
//            swal("", "Item not found ", "warning");
//        }
//    };
//     reader.readAsArrayBuffer(selectedFile);
//    $('#Partial_cpl_upload').modal('hide');
//    $('[data-target="#Partial_cpl_upload"]').removeAttr('data-toggle').removeAttr('data-target').addClass('disabled');
//    $("#uploadBtn").attr("disabled", true);
//    $("#uploadBtn").css("filter", "grayscale(100%)");

//    $("#downloadTemplateBtn").removeAttr("href");
//    $("#downloadTemplateBtn").attr("disabled", true);
//    $("#downloadTemplateBtn").css("filter", "grayscale(100%)");
//}
function UploadCPLData() {
    debugger;
    showLoader();

    try { // 🔹 ADDED: global safety

        const selectedFile = document.getElementById('cplfile').files[0];

        if (!selectedFile) {
            alert('Please select an Excel file first.');
            hideLoader();
            return;
        }

        const reader = new FileReader();

        reader.onload = async function (e) { // 🔹 ADDED: async
            debugger;

            try { // 🔹 ADDED: safety inside reader

                const data = new Uint8Array(e.target.result);
                const workbook = XLSX.read(data, { type: 'array' });

                // Sheet 1
                const sheet1Name = workbook.SheetNames[0];
                const worksheet1 = workbook.Sheets[sheet1Name];
                const jsonPriceList = XLSX.utils.sheet_to_json(worksheet1, { header: 1 });

                // Sheet 2
                const sheet2Name = workbook.SheetNames[1];
                const worksheet2 = workbook.Sheets[sheet2Name];
                const jsonItemDetail = XLSX.utils.sheet_to_json(worksheet2, { header: 1 });

                if (jsonPriceList.length <= 1) {
                    swal("", 'Excel file is empty. Please fill data in excel file and try again', "warning");
                    hideLoader();
                    return;
                }

                let rowIdx = $('#PriceListTbl >tbody >tr').length;
                let RowNo = 0;

                $("#PriceListTbl >tbody >tr").each(function () {
                    const currentRow = $(this);
                    RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
                });

                if (!RowNo) RowNo = 1;

                let seenItems = new Set();

                let htmlBuffer = "";               // 🔹 ADDED: DOM buffer
                let itemDetailCache = {};          // 🔹 ADDED: cache
                window.missingItems = [];          // 🔹 ADDED: safety

                for (let i = 1; i < jsonPriceList.length; i++) {
                    const row = jsonPriceList[i];

                    const itemName = row[0] ? row[0].toString().trim() : '';
                    if (!itemName) continue;

                    if (seenItems.has(itemName)) {
                        swal("", 'Duplicate item found in Excel', "warning");
                        $('#PriceListTbl tbody').html('');
                        hideLoader();
                        return;
                    }
                    seenItems.add(itemName);

                    const sale_price_inc_tax = row[1] ? parseFloat(row[1]).toFixed(3) : '';
                    const salePrice = row[2] ? parseFloat(row[2]).toFixed(3) : '';
                    const mrpDiscount = row[3] ? parseFloat(row[3]).toFixed(2) : '';
                    const discountPerc = row[4] ? parseFloat(row[4]).toFixed(2) : '';
                    const remarks = row[5] || '';

                    // 🔹 OPTIMIZED but NOT REMOVED
                    if (!itemDetailCache[itemName]) {
                        itemDetailCache[itemName] =
                            getItemDetailsByName(itemName, jsonItemDetail) || ["", "", ""];
                    }
                    var ItemDetail = itemDetailCache[itemName];

                    let rowClass = "";
                    if (!ItemDetail || !ItemDetail[0]) {
                        missingItems.push(itemName);
                        rowClass = "highlight_tr";
                    }

                    var FinVal = salePrice * mrpDiscount / 100;
                    FinVal2 = (salePrice - FinVal).toFixed(2);
                    if (discountPerc > 0) {
                        FinVal2 = (FinVal2 - (FinVal2 * discountPerc) / 100).toFixed(3);
                    }

                    // 🔹 SAME HTML – just buffered
                    htmlBuffer += ` <tr id="R${++rowIdx}" class="${rowClass}">
                    <td class=" red center">
                        <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i>
                    </td>
                    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
                    <td>
                        <div class="col-sm-11 lpo_form" style="padding:0px;" onclick="OnClickDropdown(${RowNo})">
                            <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" value="${itemName}" onchange="OnChangeSOItemName(${RowNo},event)">
                                <option  value="${ItemDetail[0]}">${itemName}</option>
                            </select>
                            <input type="hidden" id="hfItemID" value="${ItemDetail[0]}" />
                            <span id="ItemNameError" class="error-message is-visible"></span>
                        </div>
                        <div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;">
                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                                <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">
                            </button>
                        </div>
                    </td>
                    <td>
                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${ItemDetail[1]}" placeholder="${$("#ItemUOM").text()}" disabled>
                        <input id="UOMID" type="hidden" value="${ItemDetail[2]}" />
                    </td>
                    <td>
                         <div class="lpo_form">
                            <input id="sale_price_inc_tax" class="form-control num_right" autocomplete="off" value="${sale_price_inc_tax}" onkeypress="return RateFloatValueonly(this,event);" type="text" name="sale_price_inc_tax"  placeholder="0000.00"  ><span id="sale_price_inc_taxError" class="error-message is-visible"></span>
                         </div>
                    </td>
                    <td>
                        <div class="lpo_form">
                            <input id="SalePrice" class="form-control num_right" autocomplete="off" onchange="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice" value="${salePrice}" placeholder="0000.00">
                        </div>
                    </td>
                    <td>
                        <div class="lpo_form">
                            <input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" value="${mrpDiscount}" placeholder="0000.00">
                        </div>
                    </td>
                    <td>
                        <input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" value="${discountPerc}" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);" placeholder="0000.00">
                    </td>
                    <td>
                        <input id="EffectivePrice" onkeypress="return RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" value="${FinVal2}" placeholder="0000.00" disabled>
                    </td>
                    <td>
                        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}">${remarks}</textarea>
                    </td>
                    <td>
                        <div class="col-md-3 col-sm-12"></div>
                        <div class="col-md-6 col-sm-12">
                            <a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false">
                                <div class="plus_icon3"><i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i></div>
                            </a>
                        </div>
                        <div class="col-md-3 col-sm-12"></div>
                    </td>
                    <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
                </tr>`;

//                    htmlBuffer += `
//<tr id="R${++rowIdx}" class="${rowClass}">
//    <td class=" red center">
//        <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}"></i>
//    </td>
//    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
//    <td>
//        <div class="col-sm-11 lpo_form" style="padding:0px;" onclick="OnClickDropdown(${RowNo})">
//            <select class="form-control" id="Itemname_${RowNo}" onchange="OnChangeSOItemName(${RowNo},event)">
//                <option value="${ItemDetail[0]}">${itemName}</option>
//            </select>
//            <input type="hidden" id="hfItemID" value="${ItemDetail[0]}" />
//            <span id="ItemNameError" class="error-message is-visible"></span>
//        </div>
//    </td>
//    <td>
//        <input id="UOM" class="form-control" value="${ItemDetail[1]}" disabled>
//        <input id="UOMID" type="hidden" value="${ItemDetail[2]}" />
//    </td>
//    <td><input id="SalePrice" class="form-control num_right" value="${salePrice}"></td>
//    <td><input id="MRPDiscount" class="form-control num_right" value="${mrpDiscount}"></td>
//    <td><input id="DiscountPerc" class="form-control num_right" value="${discountPerc}"></td>
//    <td><input id="EffectivePrice" class="form-control num_right" value="${FinVal2}" disabled></td>
//    <td><textarea id="remarks" class="form-control">${remarks}</textarea></td>
//    <td style="display:none"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
//</tr>`;

                    RowNo++;

                    // 🔹 ADDED: allow UI repaint
                    if (i % 50 === 0) {
                        await new Promise(r => setTimeout(r, 0));
                    }
                }

                $('#PriceListTbl tbody').append(htmlBuffer);

                if (missingItems.length > 0) {
                    swal("", "Item not found", "warning");
                }

            } catch (ex) {
                console.error(ex);
                swal("", "Error while processing Excel file", "error");
            } finally {
                hideLoader(); // 🔹 GUARANTEED
            }
        };

        reader.readAsArrayBuffer(selectedFile);

        $('#Partial_cpl_upload').modal('hide');
        $("#uploadBtn").attr("disabled", true).css("filter", "grayscale(100%)");

    } catch (err) {
        console.error(err);
        hideLoader();
        swal("", "Unexpected error", "error");
    }
}


function OnClickDropdown(RowNo) {
    showLoader();
    BindSOItmList(RowNo);
    hideLoader();
    setTimeout(() => {
        $(`#Itemname_${RowNo}`).select2('open');
    }, 100);
}
//if (!selectedFile) {
//    alert('Please select an Excel file first.');
//    hideLoader();
//    return;
//}

// Add "Wait or Exit" prompt

//function UploadCPLData() {
//    debugger;
//    showLoader();
//    const selectedFile = document.getElementById('cplfile').files[0];

//    if (!selectedFile) {
//        alert('Please select an Excel file first.');
//        hideLoader();
//        return;
//    }

//    const reader = new FileReader(); // ✅ This was missing

//    reader.onload = function (e) {
//        const data = new Uint8Array(e.target.result);
//        const workbook = XLSX.read(data, { type: 'array' });

//        const jsonPriceList = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 });
//        const jsonItemDetail = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[1]], { header: 1 });

//        if (jsonPriceList.length <= 1) {
//            swal("", 'Excel file is empty. Please fill data in excel file and try again', "warning");
//            hideLoader();
//            return;
//        }

//        let rowIdx = $('#PriceListTbl >tbody >tr').length;
//        let RowNo = 0;

//        $("#PriceListTbl >tbody >tr").each(function () {
//            const snoVal = $(this).find('input[id^="SNohf"]').val();
//            RowNo = snoVal ? parseInt(snoVal) + 1 : 1;
//        });

//        if (!RowNo) RowNo = 1;
//        let seenItems = new Set();
//        let rowsHtml = '';

//        for (let i = 1; i < jsonPriceList.length; i++) {
//            const row = jsonPriceList[i];
//            const itemName = row[0] ? row[0].toString().trim() : '';
//            if (!itemName) continue;

//            if (seenItems.has(itemName)) {
//                swal("", 'Duplicate item found in Excel', "warning");
//                $('#PriceListTbl tbody').html('');
//                hideLoader();
//                return;
//            }
//            seenItems.add(itemName);

//            const salePrice = row[1] ? parseFloat(row[1]).toFixed(3) : '';
//            const mrpDiscount = row[2] ? parseFloat(row[2]).toFixed(2) : '';
//            const discountPerc = row[3] ? parseFloat(row[3]).toFixed(2) : '';
//            const remarks = row[4] || '';
//            const ItemDetail = getItemDetailsByName(itemName, jsonItemDetail);

//            let FinVal = (salePrice * mrpDiscount / 100);
//            let FinVal2 = (salePrice - FinVal).toFixed(2);
//            if (discountPerc > 0) {
//                FinVal2 = (FinVal2 - (FinVal2 * discountPerc) / 100).toFixed(3);
//            }

//            rowsHtml += `<tr id="R${++rowIdx}">
//                    <td class=" red center">
//                        <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i>
//                    </td>
//                    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
//                    <td>
//                        <div class="col-sm-11 lpo_form" style="padding:0px;">
//                            <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" value="${itemName}" onchange="OnChangeSOItemName(${RowNo},event)">
//                                <option  value="${ItemDetail[0]}">${itemName}</option>
//                            </select>
//                            <input type="hidden" id="hfItemID" value="${ItemDetail[0]}" />
//                            <span id="ItemNameError" class="error-message is-visible"></span>
//                        </div>
//                        <div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;">
//                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
//                                <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">
//                            </button>
//                        </div>
//                    </td>
//                    <td>
//                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${ItemDetail[1]}" placeholder="${$("#ItemUOM").text()}" disabled>
//                        <input id="UOMID" type="hidden" value="${ItemDetail[2]}" />
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="SalePrice" class="form-control num_right" autocomplete="off" onchange="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice" value="${salePrice}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" value="${mrpDiscount}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" value="${discountPerc}" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);" placeholder="0000.00">
//                    </td>
//                    <td>
//                        <input id="EffectivePrice" onkeypress="return RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" value="${FinVal2}" placeholder="0000.00" disabled>
//                    </td>
//                    <td>
//                        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}">${remarks}</textarea>
//                    </td>
//                    <td>
//                        <div class="col-md-3 col-sm-12"></div>
//                        <div class="col-md-6 col-sm-12">
//                            <a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false">
//                                <div class="plus_icon3"><i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i></div>
//                            </a>
//                        </div>
//                        <div class="col-md-3 col-sm-12"></div>
//                    </td>
//                    <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
//                </tr>`;
//            RowNo++;
//        }

//        $('#PriceListTbl tbody').append(rowsHtml);

//        for (let i = 1; i < RowNo; i++) {
//            BindSOItmList(i);
//        }

//        hideLoader();
//        $('#Partial_cpl_upload').modal('hide');
//        $('[data-target="#Partial_cpl_upload"]').removeAttr('data-toggle').removeAttr('data-target').addClass('disabled');
//        $("#uploadBtn").attr("disabled", true).css("filter", "grayscale(100%)");
//        $("#downloadTemplateBtn").removeAttr("href").attr("disabled", true).css("filter", "grayscale(100%)");
//    };

//    reader.readAsArrayBuffer(selectedFile);
//}


 //function UploadCPLData() {
//        debugger;
//        showLoader();
//        const selectedFile = document.getElementById('cplfile').files[0];

//        if (!selectedFile) {
//            alert('Please select an Excel file first.');
//            return;
//        }
//        const reader = new FileReader();
//        reader.onload = function (e) {
//            debugger;
//            const data = new Uint8Array(e.target.result);
//            const workbook = XLSX.read(data, { type: 'array' });

//            const sheet1Name = workbook.SheetNames[0];
//            const worksheet1 = workbook.Sheets[sheet1Name];
//            const jsonPriceList = XLSX.utils.sheet_to_json(worksheet1, { header: 1 });

//            const sheet2Name = workbook.SheetNames[1];
//            const worksheet2 = workbook.Sheets[sheet2Name];
//            const jsonItemDetail = XLSX.utils.sheet_to_json(worksheet2, { header: 1 });

//            if (jsonPriceList.length <= 1) {
//                swal("", 'Excel file is empty. Please fill data in excel file and try again', "warning");
//                hideLoader();
//                return;
//            }

//            let rowIdx = $('#PriceListTbl >tbody >tr').length;
//            let RowNo = 0;

//            $("#PriceListTbl >tbody >tr").each(function () {
//                const currentRow = $(this);
//                RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
//            });

//            if (!RowNo) RowNo = 1;
//            let seenItems = new Set();
//            let i = 1;

//            function processNextRow() {
//                if (i >= jsonPriceList.length) {
//                    hideLoader();
//                    $('#Partial_cpl_upload').modal('hide');
//                    $('[data-target="#Partial_cpl_upload"]').removeAttr('data-toggle').removeAttr('data-target').addClass('disabled');

//                    $("#uploadBtn").attr("disabled", true).css("filter", "grayscale(100%)");
//                    $("#downloadTemplateBtn").removeAttr("href").attr("disabled", true).css("filter", "grayscale(100%)");
//                    return;
//                }
//                const row = jsonPriceList[i++];
//                const itemName = row[0] ? row[0].toString().trim() : '';
//                if (!itemName) return setTimeout(processNextRow, 0);

//                if (seenItems.has(itemName)) {
//                    swal("", 'Duplicate item found in Excel', "warning");
//                    $('#PriceListTbl tbody').html('');
//                    $('#Partial_cpl_upload').modal('hide');
//                    hideLoader();
//                    return;
//                }
//                seenItems.add(itemName);
//                const salePrice = row[1] ? parseFloat(row[1]).toFixed(3) : '';
//                const mrpDiscount = row[2] ? parseFloat(row[2]).toFixed(2) : '';
//                const discountPerc = row[3] ? parseFloat(row[3]).toFixed(2) : '';
//                const remarks = row[4] || '';
//                var ItemDetail = getItemDetailsByName(itemName, jsonItemDetail) || ['', '', ''];
//                var FinVal = salePrice * mrpDiscount / 100;
//                var FinVal2 = (salePrice - FinVal).toFixed(2);
//                if (discountPerc > 0) {
//                    FinVal2 = (FinVal2 - (FinVal2 * discountPerc) / 100).toFixed(3);
//                }
//                $('#PriceListTbl tbody').append(`
//                <tr id="R${++rowIdx}">
//                    <td class=" red center">
//                        <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i>
//                    </td>
//                    <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
//                    <td>
//                        <div class="col-sm-11 lpo_form" style="padding:0px;">
//                            <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" value="${itemName}" onchange="OnChangeSOItemName(${RowNo},event)">
//                                <option  value="${ItemDetail[0]}">${itemName}</option>
//                            </select>
//                            <input type="hidden" id="hfItemID" value="${ItemDetail[0]}" />
//                            <span id="ItemNameError" class="error-message is-visible"></span>
//                        </div>
//                        <div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;">
//                            <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
//                                <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">
//                            </button>
//                        </div>
//                    </td>
//                    <td>
//                        <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${ItemDetail[1]}" placeholder="${$("#ItemUOM").text()}" disabled>
//                        <input id="UOMID" type="hidden" value="${ItemDetail[2]}" />
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="SalePrice" class="form-control num_right" autocomplete="off" onchange="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice" value="${salePrice}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <div class="lpo_form">
//                            <input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" value="${mrpDiscount}" placeholder="0000.00">
//                        </div>
//                    </td>
//                    <td>
//                        <input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" value="${discountPerc}" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);" placeholder="0000.00">
//                    </td>
//                    <td>
//                        <input id="EffectivePrice" onkeypress="return RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" value="${FinVal2}" placeholder="0000.00" disabled>
//                    </td>
//                    <td>
//                        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}">${remarks}</textarea>
//                    </td>
//                    <td>
//                        <div class="col-md-3 col-sm-12"></div>
//                        <div class="col-md-6 col-sm-12">
//                            <a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false">
//                                <div class="plus_icon3"><i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i></div>
//                            </a>
//                        </div>
//                        <div class="col-md-3 col-sm-12"></div>
//                    </td>
//                    <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
//                </tr>
//            ` );
//                BindSOItmList(RowNo);
//                RowNo++;
//                setTimeout(processNextRow, 0);
//            }
//            processNextRow();
//            //hideLoader();
//        };
//        reader.readAsArrayBuffer(selectedFile);
//    }

function getItemDetailsByName(itemName, data) {
    debugger;
    const [headers, ...rows] = data;

    const nameIndex = headers.indexOf("Item Name");
    const idIndex = headers.indexOf("item id");
    const uomAliasIndex = headers.indexOf("uom_alias");
    const uomIdIndex = headers.indexOf("uom_id");

    if (nameIndex === -1 || idIndex === -1 || uomAliasIndex === -1 || uomIdIndex === -1) {
        throw new Error("One or more required headers are missing in Excel data");
    }

   // const normalizedItemName = normalize(itemName);
    const normalizedItemName = itemName;

    //const match = rows.find(row => {
    //    const rowItemName = normalize(row[nameIndex] || "").trim().toLowerCase();
    //    return rowItemName === normalizedItemName;
    //});
    const match = rows.find(row =>
        (row[nameIndex]) === normalizedItemName
    );
    return match
        ? [match[idIndex], match[uomAliasIndex], match[uomIdIndex]]
        : null;
}
//function normalize(str) {
//    return (str || "")
//        .toLowerCase()
//        .replace(/\s+/g, " ")     
//        .replace(/[^a-z0-9 ]/gi, "")  
//        .trim();
//}
function OnchangePriceListItemDetail() {
    debugger;
    $('#PriceListTbl tbody').html('');
    $('#Partial_cpl_upload').modal('hide');
    $('[data-target="#Partial_cpl_upload"]').removeAttr('data-toggle').removeAttr('data-target').addClass('disabled');
    $("#uploadBtn").attr("disabled", true);
    $("#uploadBtn").css("filter", "grayscale(100%)");

    $("#downloadTemplateBtn").removeAttr("href");
    $("#downloadTemplateBtn").attr("disabled", true);
    $("#downloadTemplateBtn").css("filter", "grayscale(100%)");

    var plist_id = $('#replicatecpl').val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CustomerPriceList/GetPriceListItemDetail",
        data: { plist_id },
        success: function (data) {
            for (let i = 0; i < data.length; i++) {
                var item = data[i]; // Get the current item

                // Extract values from the item object
                var item_id = item.item_id;
                var item_name = item.item_name;
                var uom_name = item.uom_name;
                var uom_id = item.uom_id;
                var sale_price = item.sale_price;
                var disc_mrp = item.disc_mrp;
                var disc_perc = item.disc_perc;
                var effect_price = item.effect_price;
                var it_remarks = item.it_remarks;
                var sale_price_inc_tax = item.sale_price_inc_tax;
                var RowNo = 1;
                $("#PriceListTbl >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var snoVal = parseInt(currentRow.find("#SNohf").val());
                    if (!isNaN(snoVal) && snoVal >= RowNo) {
                        RowNo = snoVal + 1;
                    }
                });

                $('#PriceListTbl tbody').append(`
                    <tr id="R${RowNo}">
                        <td class=" red center">
                            <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${$("#Span_Delete_Title").text()}"></i>
                        </td>
                        <td class="sr_padding"><span id="SpanRowId">${RowNo}</span></td>
                        <td>
                            <div class="col-sm-11 lpo_form" style="padding:0px;">
                                <select class="form-control" id="Itemname_${RowNo}" name="Itemname_${RowNo}" value="${item_id}" onchange="OnChangeSOItemName(${RowNo},event)">
                                    <option value="${item_id}">${item_name}</option>
                                </select>
                                <input type="hidden" id="hfItemID" value="${item_id}" />
                                <span id="ItemNameError" class="error-message is-visible"></span>
                            </div>
                            <div class="col-sm-1 i_Icon" style="padding:0px; text-align:right;">
                                <button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false">
                                    <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}">
                                </button>
                            </div>
                        </td>
                        <td>
                            <input id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" value="${uom_name}" placeholder="${$("#ItemUOM").text()}" disabled>
                            <input id="UOMID" type="hidden" value="${uom_id}" />
                        </td>
 <td>
                            <div class="lpo_form">
                                <input id="sale_price_inc_tax" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="sale_price_inc_tax" value="${sale_price_inc_tax}" placeholder="0000.00">
                            </div>
                        </td>
                        <td>
                            <div class="lpo_form">
                                <input id="SalePrice" class="form-control num_right" autocomplete="off" onchange="OnclickSalePrice(event)" onkeypress="return RateFloatValueonly(this,event);" type="text" name="SalePrice" value="${sale_price}" placeholder="0000.00">
                            </div>
                        </td>
                        <td>
                            <div class="lpo_form">
                                <input id="MRPDiscount" class="form-control date num_right" autocomplete="off" onchange="OnchangeMRPDisc(event)" onkeypress="return PercentValueonly(this,event);" type="text" name="MRPDiscount" value="${disc_mrp}" placeholder="0000.00">
                            </div>
                        </td>
                        <td>
                            <input id="DiscountPerc" class="form-control num_right" autocomplete="off" type="text" name="DiscountPerc" value="${disc_perc}" onchange="CalculateDisPercent(event)" onkeypress="return PercentValueonly(this,event);" placeholder="0000.00">
                        </td>
                        <td>
                            <input id="EffectivePrice" onkeypress="return RateFloatValueonly(this,event);" class="form-control num_right" autocomplete="off" type="text" name="EffectivePrice" required="required" value="${effect_price}" placeholder="0000.00" disabled>
                        </td>
                        <td>
                            <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="100" placeholder="${$("#span_remarks").text()}">${it_remarks}</textarea>
                        </td>
                        <td>
                            <div class="col-md-3 col-sm-12"></div>
                            <div class="col-md-6 col-sm-12">
                                <a href="#" style="color:#ffffff;" data-toggle="modal" data-target="#History" onclick="return PrcHistryDtlOnClk(event)" data-backdrop="static" data-keyboard="false">
                                    <div class="plus_icon3"><i class="fa fa-history" aria-hidden="true" data-toggle="tooltip" title="${$("#span_History").text()}"></i></div>
                                </a>
                            </div>
                            <div class="col-md-3 col-sm-12"></div>
                        </td>
                        <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
                    </tr>
                `);

                BindSOItmList(RowNo);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error: ", error);
        }
    });
}




