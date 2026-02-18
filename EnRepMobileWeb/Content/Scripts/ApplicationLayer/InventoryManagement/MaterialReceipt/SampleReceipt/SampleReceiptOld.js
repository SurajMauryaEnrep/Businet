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
var CurrentDate = moment().format('YYYY-MM-DD');
    //$("#SR_Date").val(CurrentDate);
    if ($("#SR_Date").val() == "0001-01-01") {
        $("#SR_Date").val(CurrentDate);
    }
    $("#HistoryTo_date").val(CurrentDate);

    RemoveSessionNew();

    $('#Btn_Forward').off('click');
    $('#Btn_Print').off('click');
    $('#Btn_Workflow').off('click');
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Workflow").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Print").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $('#Btn_Forward').off('click');
    $('#Btn_Print').off('click');
    $('#Btn_Mail').off('click');
    $('#Btn_Message').off('click');
    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Forward").prop('disabled', true);
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Print").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

  BindSampleRcptItmList(1);
    
    //BindWarehouseList(1)
    $('#SR_ItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
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
        // var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        var ItemCode = $(this).closest('tr').find("#hf_ItemID").val();
        if (ItemCode != "" && ItemCode != null && ItemCode != "0") {
            ShowItemListItm(ItemCode);
            AfterDeleteResetBatchAndSerialDetails(ItemCode);

        }
        SerialNoAfterDelete();
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var SRNo = clickedrow.children("#SRNo").text();
        var SR_Date = clickedrow.children("#SR_DT").text();
        var Doc_id = $("#DocumentMenuId").val();

        $("#hdDoc_No").val(SRNo);

        GetWorkFlowDetails(SRNo, SR_Date, Doc_id);

    });
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var _SR_No = clickedrow.children("#SRNo").text();
        var _SR_Date = clickedrow.children("#SR_DT").text();
        if (_SR_No != null && _SR_No != "") {
            sessionStorage.removeItem("EditSRNo");
            sessionStorage.setItem("EditSRNo", _SR_No);
            sessionStorage.removeItem("EditSRDate");
            sessionStorage.setItem("EditSRDate", _SR_Date);
            SR_Detail();
        }

    });

    ListRowHighLight();
    GetAndViewSR_Deatil();
    jQuery(document).trigger('jquery.loaded');
    SRNo = $("#SR_Number").val();
    $("#hdDoc_No").val(SRNo);
    sessionStorage.removeItem("EditSRNo");
    sessionStorage.removeItem("EditSRDate");
});

function EnableHeaderDetail() {
    $("#EntityType").attr('disabled', false);
    $("#EntityName").attr('disabled', false);
    $("#remarks").attr('disabled', false);
}
function DisableHeaderDetail() {

    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);
    $("#remarks").attr('disabled', true);

}

function CheckSR_ItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();

        if (currentRow.find("#ItemName_" + Sno).val() == "" || currentRow.find("#ItemName_" + Sno).val() == "0") {
            currentRow.find("#ItemNameError").text($("#valueReq").text());
            currentRow.find("#ItemNameError").css("display", "block");
            currentRow.find("[aria-labelledby='select2-ItemName_" + Sno + "-container']").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemNameError").css("display", "none");
            currentRow.find("#ItemName_" + Sno).css("border-color", "#ced4da");
        }

        if (currentRow.find("#TxtItemCost").val() == "" || parseFloat(currentRow.find("#TxtItemCost").val()) == parseFloat("0")) {
            currentRow.find("#ItmCostError").text($("#valueReq").text());
            currentRow.find("#ItmCostError").css("display", "block");
            currentRow.find("#TxtItemCost").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItmCostError").css("display", "none");
            currentRow.find("#TxtItemCost").css("border-color", "#ced4da");
        }
        if (currentRow.find("#ReceivedQuantity").val() == "") {
            currentRow.find("#ReceivedQtyError").text($("#valueReq").text());
            currentRow.find("#ReceivedQtyError").css("display", "block");
            currentRow.find("#ReceivedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
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
        }
        else {
            currentRow.find("#wh_Error").css("display", "none");
            currentRow.find("#wh_id_" + Sno).css("border-color", "#ced4da");
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
        //currentRow.find("#wh_id_" + SNo).attr("disabled", true);
        //currentRow.find("#BtnBatchDetail").attr("disabled", true);
        //currentRow.find("#BtnSerialDetail").attr("disabled", true);
        currentRow.find("#remarks").attr("disabled", true);
    });
}
function EnableTbleDetail() {
    $("#DivAddNewItemBtn").css("display", "block");

    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var SNo = currentRow.find("#hfSNo").val();
        currentRow.find("#ItemName_" + SNo).attr("disabled", false);
        currentRow.find("#UOM").attr("disabled", true);
        currentRow.find("#TxtItemCost").attr("disabled", false);
        currentRow.find("#ReceivedQuantity").attr("disabled", false);
        //currentRow.find("#wh_id_" + SNo).attr("disabled", false);
        //currentRow.find("#BtnBatchDetail").attr("disabled", true);
        //currentRow.find("#BtnSerialDetail").attr("disabled", true);
        currentRow.find("#remarks").attr("disabled", false);
    });
}

function OnChangeEntityType() {
    debugger;
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

    $("#EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlist").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType
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
function BindSR_SuppCustList_List(EntityType) {
    debugger;

    $("#ddl_EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlist").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType
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
        DisableTbleDetail();
    }
    else {
        $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
        $("#SpanEntityNameErrorMsg").css("display", "none");
        EnableTbleDetail();


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
}
function OnChangeReceivedQty(e) {
    var QtyDecDigit = $("#QtyDigit").text();
    debugger;

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
    }
    else {
        clickedrow.find("#ReceivedQuantity").val(parseFloat(ReceivedQty).toFixed(QtyDecDigit));
        clickedrow.find("#ReceivedQtyError").css("display", "none");
        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");

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

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#SR_Number").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;

}

//---------------Header Deatils-----------------------//
function GetAndViewSR_Deatil() {
    debugger;
    var _SR_No = sessionStorage.getItem("EditSRNo");
    var _SR_Date = sessionStorage.getItem("EditSRDate");
    var UserID = $("#UserID").text();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    var Doc_id = $("#DocumentMenuId").val();
    GetWorkFlowDetails(_SR_No, _SR_Date, Doc_id);

    if (_SR_No != null && _SR_No != "" && _SR_Date != null && _SR_Date != "") {
        debugger;
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleReceipt/GetSampleReceiptDetail_Edit",
            data: { SR_No: _SR_No, SR_Date: _SR_Date },
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                   ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    /*------------------Attatchmnet----------------*/
                    ShowSavedAttatchMentFiles(_SR_No, _SR_Date);
                    $("#FilesUpload").css("display", "none");
                /*------------------Attatchmnet End----------------*/
                    debugger;
                    arr = JSON.parse(data);
                    var OrderStatus = "";
                    var Createrid = parseInt(arr.Table[0].createid);
                    $("#Create_ID").val(Createrid);
                    var approval_id = parseInt(arr.Table[0].app_id);
                    $("#SRCreatedBy").text(arr.Table[0].CreateName);
                    $("#SRCreatedDate").text(arr.Table[0].CreateDate);
                    $("#SRApproveBy").text(arr.Table[0].ApproveName);
                    $("#SRApproveDate").text(arr.Table[0].ApproveDate);
                    $("#SRAmdedBy").text(arr.Table[0].ModifyName);
                    $("#SRAmdedDate").text(arr.Table[0].ModifyDate);
                    $("#SRStatus").text(arr.Table[0].status_name);
                    /*$("#hfStatus").val(arr.Table[0].sr_status.trim());*/

                    OrderStatus = $.trim(arr.Table[0].sr_status);
                    $("#hfStatus").val(OrderStatus);

                    //if (arr.Table[0].sr_status.trim() === "C") {
                    //    $("#Cancelled").prop("checked", true);
                    //}
                    //else {
                    //    $("#Cancelled").prop("checked", false);
                    //}
                    if (arr.Table[0].sr_status === "Y") {
                        $("#Cancelled").prop("checked", true);
                    }
                    else {
                        $("#Cancelled").prop("checked", false);
                    }
                    $("#hdDoc_No").val(_SR_No);

                    $("#SR_Number").val(arr.Table[0].sr_no);
                    $("#SR_Date").val(arr.Table[0].srDate);
                    $('#EntityType').val(arr.Table[0].entity_type).prop('selected', true);
                    $('#EntityName').empty().append('<option value=' + arr.Table[0].entity_id + ' selected="selected">' + arr.Table[0].EntityName + '</option>');
                    $("#remarks").val(arr.Table[0].remarks);


                    debugger;
                    if (arr.Table1.length > 0) {
                        sessionStorage.removeItem("SRItemDetailSession");
                        sessionStorage.setItem("SRItemDetailSession", JSON.stringify(arr.Table1));

                        $('#SR_ItmDetailsTbl tbody tr').remove();
                        for (var i = 0; i < arr.Table1.length; i++) {
                            if (i >= 0) {
                                AddNewRowForEditSRItem();
                            }
                        }
                        if ($("#SR_ItmDetailsTbl >tbody >tr").length == arr.Table1.length) {

                            sessionStorage.removeItem("SRItemDetailSession");
                            sessionStorage.setItem("SRItemDetailSession", JSON.stringify(arr.Table1));

                            //var SRItemListData = JSON.parse(sessionStorage.getItem("itemList"));
                            //if (SRItemListData != null && SRItemListData != "") {
                            //    if (SRItemListData.length > 0) {
                            //        $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                            //            //debugger;
                            //            var currentRow = $(this);
                            //            var SNo = currentRow.find("#hfSNo").val();
                            //            //if (SNo != "1") {

                            //                $('#ItemName_' + SNo).append(`<optgroup class='def-cursor' id="Textddl${SNo}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                            //                for (var i = 0; i < SRItemListData.length; i++) {
                            //                    $('#Textddl' + SNo).append(`<option data-uom="${SRItemListData[i].uom_name}" value="${SRItemListData[i].Item_id}">${SRItemListData[i].Item_name}</option>`);
                            //                }
                            //                var firstEmptySelect = true;
                            //                $('#ItemName_' + SNo).select2({
                            //                    templateResult: function (data) {
                            //                        var UOM = $(data.element).data('uom');
                            //                        var classAttr = $(data.element).attr('class');
                            //                        var hasClass = typeof classAttr != 'undefined';
                            //                        classAttr = hasClass ? ' ' + classAttr : '';
                            //                        var $result = $(
                            //                            '<div class="row">' +
                            //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                            //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                            //                            '</div>'
                            //                        );
                            //                        return $result;
                            //                        firstEmptySelect = false;
                            //                    }
                            //                });
                            //            //}
                            //        });
                            //    }
                            //}

                            var SR_WHListData = JSON.parse(sessionStorage.getItem("SR_WHList"));
                            if (SR_WHListData != null && SR_WHListData != "") {
                                if (SR_WHListData.length > 0) {
                                    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                                        //debugger;
                                        var currentRow = $(this);
                                        var SNo = currentRow.find("#hfSNo").val();

                                        var s = '<option value="0">---Select---</option>';
                                        for (var i = 0; i < SR_WHListData.length; i++) {
                                            s += '<option value="' + SR_WHListData[i].wh_id + '">' + SR_WHListData[i].wh_name + '</option>';
                                        }
                                        $("#wh_id_" + SNo).html(s);
                                    });
                                }
                            }

                            $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                                debugger;
                                var currentRow = $(this);
                                var SNo = currentRow.find("#hfSNo").val();
                                var DataSno;
                                DataSno = parseInt(SNo) - 1;

                                currentRow.find("#hf_ItemID").val(arr.Table1[DataSno].item_id);
                                currentRow.find("#ItemName_" + SNo).append('<option value="' + arr.Table1[DataSno].item_id+'">' + arr.Table1[DataSno].item_name+'</option>');
                                /*currentRow.find("#ItemName_" + SNo).val(arr.Table1[DataSno].item_id).trigger('change');*/
                                currentRow.find("#UOM").val(arr.Table1[DataSno].uom_alias);
                                currentRow.find("#UOMID").val(arr.Table1[DataSno].uom_id);

                                currentRow.find("#hfbatchable").val(arr.Table1[DataSno].i_batch);
                                currentRow.find("#hfserialable").val(arr.Table1[DataSno].i_serial);
                                currentRow.find("#hfexpiralble").val(arr.Table1[DataSno].i_exp);
                                currentRow.find("#TxtItemCost").val(parseFloat(arr.Table1[DataSno].item_rate).toFixed(RateDecDigit));
                                currentRow.find("#ReceivedQuantity").val(parseFloat(arr.Table1[DataSno].rec_qty).toFixed(QtyDecDigit));
                                currentRow.find("#wh_id_" + SNo).val(arr.Table1[DataSno].wh_id).prop('selected', true);
                                currentRow.find("#LotNumber").val(arr.Table1[DataSno].lot_id);
                                currentRow.find("#remarks").val(arr.Table1[DataSno].it_remarks);
                                BindSampleRcptItmList(DataSno+1);
                            });
                        }
                    }
                    if (arr.Table2.length > 0) {
                        debugger;
                        var BatchDetailList = [];
                        for (var j = 0; j < arr.Table2.length; j++) {
                            var RowSNo;
                            $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                                debugger;
                                var currentRow = $(this);
                                var SNo = currentRow.find("#hfSNo").val();
                                var ItmCode = "";
                                ItmCode = currentRow.find('#hf_ItemID').val();
                                if (ItmCode == arr.Table2[j].item_id) {
                                    RowSNo = SNo;
                                }
                            });

                            var BItmCode = arr.Table2[j].item_id;
                            var BBatchQty = arr.Table2[j].batch_qty;
                            var BBatchNo = arr.Table2[j].batch_no;
                            var BExDate = arr.Table2[j].exp_dt;
                            if (BExDate == null) {
                                BExDate = "";
                            }
                            BatchDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: BItmCode, BatchQty: BBatchQty, BatchNo: BBatchNo, BatchExDate: BExDate })
                        }
                        debugger;
                        sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
                    }
                    if (arr.Table3.length > 0) {
                        debugger;
                        var SerialDetailList = [];

                        for (var k = 0; k < arr.Table3.length; k++) {
                            var RowSNo;
                            $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                                debugger;
                                var currentRow = $(this);
                                var SNo = currentRow.find("#hfSNo").val();
                                var ItmCode = "";
                                ItmCode = currentRow.find('#hf_ItemID').val();
                                if (ItmCode == arr.Table3[k].item_id) {
                                    RowSNo = SNo;
                                }
                            });

                            var SItmCode = arr.Table3[k].item_id;
                            var SSerialNo = arr.Table3[k].serial_no;

                            SerialDetailList.push({ UserID: UserID, RowSNo: RowSNo, ItemID: SItmCode, SerialNo: SSerialNo })
                        }
                        debugger;
                        sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
                    }
                    if (arr.Table5.length > 0) {
                        debugger;
                        for (var yw = 0; yw < arr.Table5.length; yw++) {
                            var Level = parseInt(arr.Table5[yw].level);
                            var nextLevel = parseInt(Level) + 1;
                            $("#a_" + Level).removeClass("disabled");
                            $("#a_" + Level).addClass("done");
                            $("#a_" + nextLevel).removeClass("disabled");
                            $("#a_" + nextLevel).addClass("selected");
                        }
                    }

                    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var SNo = currentRow.find("#hfSNo").val();

                        $("#ItemName_" + SNo).attr('onchange', 'BindSR_ItemList(event)');

                      //  HideItemListItm();
                        HideSelectedItem("#ItemName_", "", "#SR_ItmDetailsTbl", "#hfSNo");
                    });
                    var Userid = parseInt($("#UserID").text());
                    var StatusD = $.trim(arr.Table[0].sr_status);
                    debugger;
                    //if (StatusD == "A") {
                    //    //$("#Btn_Edit").attr('onclick', '');
                    //    //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Delete").attr('onclick', '');
                    //    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Approve").attr('onclick', '');
                    //    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                    //    $("#btn_save").attr('onclick', '');
                    //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //}
                    //else if (StatusD == "D") {
                    //    $("#btn_save").attr('onclick', '');
                    //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //}
                    //else {
                    //    $("#Btn_Edit").attr('onclick', '');
                    //    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Delete").attr('onclick', '');
                    //    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Approve").attr('onclick', '');
                    //    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#btn_save").attr('onclick', '');
                    //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //}
                    if (OrderStatus === 'D') {
                        var WFLen = $("#wizard > ul > li").length;
                        if (WFLen > 0) {
 if (arr.Table.length > 0) {/*(removing Table4)*/
                            debugger;
                            var sent_to = arr.Table[0].sent_to;
                            /* if (Userid === sent_to) {*/
                            if (Userid === Createrid) {
                                $("#Btn_Forward").prop('disabled', false);
                                $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                                $("#Btn_Forward").attr('onclick', 'ForwardBtnClick()');
                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                            }
                            else {
                                $("#Btn_Forward").attr("data-target", "");
                                //$("#Btn_Forward").attr('onclick', '');
                                $("#Btn_Forward").prop('disabled', true);
                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            }
                        }
                        if (Createrid != Userid) {
                            debugger;
                            $("#Btn_Delete").attr('onclick', '');
                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                            $("#Btn_Approve").attr('onclick', '');
                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                            $("#Btn_Edit").attr('onclick', '');
                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        }
                        else {
                            if (arr.Table5.length > 0) {
                                debugger;
                                if (arr.Table5[0].nextlevel === 0) {
                                    if (Userid === Createrid) {
                                        $("#hd_nextlevel").val(arr.Table5[0].nextlevel);
                                        $("#hd_currlevel").val(arr.Table5[0].userlevel);

                                        $("#Btn_Approve").attr("data-toggle", "modal");
                                        $("#Btn_Approve").attr("data-target", "#Forward_Pop");
                                        $("#Btn_Approve").attr('onclick', 'ForwardBtnClick()');
                                        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                                        $("#span_forward").css("display", "none");
                                        $("#span_approve").removeAttr("style");

                                        $("#radio_forward").val("Approve");

                                        $("#Btn_Forward").attr("data-target", "");
                                        //$("#Btn_Forward").attr('onclick', '');
                                        $("#Btn_Forward").prop('disabled', true);
                                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                    }
                                }
                                else {
                                    $("#Btn_Forward").prop('disabled', false);
                                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                                    $("#Btn_Forward").attr('onclick', 'ForwardBtnClick()');
                                    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                }
                            }
                        }
                        } else {
                            $("#Btn_Approve").attr('onclick', 'ApproveBtnClick()');
                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                        }

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        //$("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").prop('disabled', true);
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                    if (OrderStatus != "D") {
                        debugger;
                        $("#Btn_Delete").attr('onclick', '');
                        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        $("#Btn_Approve").attr('onclick', '');
                        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                    if (OrderStatus == "F" || OrderStatus == "QP") {
                        $("#Btn_Delete").attr('onclick', '');
                        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        $("#Btn_Approve").attr('onclick', '');
                        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        $("#Btn_Edit").attr('onclick', '');
                        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        if (arr.Table4.length > 0) {
                            //var Userid =parseInt($("#UserID").text());
                            var sent_to = arr.Table4[0].sent_to;
                            if (Userid === sent_to) {
                                $("#hd_currlevel").val(arr.Table4[0].userlevel);
                                $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                                $("#Btn_Forward").attr('onclick', 'ForwardBtnClick()');
                                $("#Btn_Forward").prop('disabled', false);
                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                            }
                            else {
                                $("#Btn_Forward").attr("data-target", "");
                                //$("#Btn_Forward").attr('onclick', '');
                                $("#Btn_Forward").prop('disabled', true);
                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            }
                        }
                        if (arr.Table5.length > 0) {
                            debugger;
                            if (arr.Table5[0].nextlevel === 0) {
                                //var Userid = parseInt($("#UserID").text());
                                var sent_to = arr.Table5[0].userid;
                                debugger;
                                if (Userid === sent_to) {
                                    debugger;
                                    $("#hd_nextlevel").val(arr.Table5[0].nextlevel);
                                    $("#hd_currlevel").val(arr.Table5[0].userlevel);
                                    $("#hd_createrlevel").val(arr.Table5[0].createrlevel);
                                    $("#Btn_Approve").attr("data-toggle", "modal");
                                    $("#Btn_Approve").attr("data-target", "#Forward_Pop");
                                    $("#Btn_Approve").attr('onclick', 'ForwardBtnClick()');
                                    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                                    $("#span_forward").css("display", "none");
                                    $("#span_approve").removeAttr("style");

                                    $("#radio_forward").val("Approve");

                                    $("#Btn_Forward").attr("data-target", "");
                                    //$("#Btn_Forward").attr('onclick', '');
                                    $("#Btn_Forward").prop('disabled', true);
                                    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                    var createlvl = arr.Table5[0].createrlevel;
                                    debugger
                                    var userlvl = arr.Table5[0].userlevel;
                                    if (AvoidDot(createlvl) == false) {
                                        createlvl = 0;
                                    }
                                    if (AvoidDot(userlvl) == false) {
                                        userlvl = 0;
                                    }
                                    if (parseFloat(createlvl) < (parseFloat(userlvl) - parseFloat(createlvl))) {
                                        $("#radio_revert").prop("disabled", false);
                                    }
                                    else {
                                        $("#radio_revert").prop("disabled", true);
                                    }
                                    if (createlvl == "0" && userlvl == "1") {
                                        $("#radio_revert").prop("disabled", true);
                                    }
                                    if (arr.Table5[0].nextlevel === "0") {
                                        $("#span_forward").css("display", "none");
                                        $("#span_approve").removeAttr("style");
                                        $("#radio_forward").val("Approve");
                                    }
                                }
                            }


                        }
                    }
                    debugger;
                    if (OrderStatus == "C" || OrderStatus == "R") {
                        $("#Btn_Edit").attr('onclick', '');
                        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                    if (OrderStatus == "C") {
                        $("#Cancelled").prop("checked", true);
                    }
                    if (OrderStatus == "A" || OrderStatus == "R") {
                        if (Userid === approval_id || Userid === Createrid) {
                            $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                        }
                        else {
                            $("#Btn_Edit").attr('onclick', '');
                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                        }
                    }
                    NewStatus = 1;
                    $("#btn_save").attr('onclick', '');
                    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                }
                DisableAfterSaveDetail();
            }
        });
    }

    else {
        //$("#btn_add_new_item").attr('onclick', '');
        //$("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        //$("#Btn_Edit").attr('onclick', '');
        //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        //$("#Btn_Delete").attr('onclick', '');
        //$("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        //$("#Btn_Approve").attr('onclick', '');
        //$("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        //$("#btn_back").attr('onclick', '');
        //$("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        //$("#item_name").focus();
        $("#btn_add_new_item").attr('onclick', '');
        $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#Btn_Edit").attr('onclick', '');
        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#Btn_Delete").attr('onclick', '');
        $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#Btn_Approve").attr('onclick', '');
        $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#btn_back").attr('onclick', '');
        $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        $("#item_name").focus();
    }
}
/*------------------Attatchmnet----------------*/
function ShowSavedAttatchMentFiles(SR_No, SR_Date) {

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SampleReceipt/GetPIAttatchDetailEdit",
        data: { SR_No: SR_No, SR_Date: SR_Date },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            } else {
                $("#PartialImageBind").html(data);
            }

        }
    });

}

/*------------------Attatchmnet End----------------*/
function ForwardBtnClick() {
    debugger;
    var OrderStatus = "";
    OrderStatus = $('#hfStatus').val();
    if (OrderStatus === "D" || OrderStatus === "F") {

        if ($("#hd_nextlevel").val() === "0") {
            $("#Btn_Forward").attr("data-target", "");
        }
        else {
            $("#Btn_Forward").prop('disabled', false);
            //$("#Btn_Forward").attr("data-target", "#Forward_Pop");
            $("#Btn_Approve").attr("data-target", "");
        }
        var DocID = $("#DocumentMenuId").val();
        $("#OKBtn_FW").attr("data-dismiss", "modal");

        Cmn_GetForwarderList(DocID);
    }
    else {
        $("#Btn_Forward").attr("data-target", "");
        //$("#Btn_Forward").attr('onclick', '');
        $("#Btn_Forward").prop('disabled', true);
        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function AddNewRowForEditSRItem() {
    debugger;
    var rowIdx = 0;
    var RowNo = 0;
    var rowCount = $('#SR_ItmDetailsTbl >tbody >tr').length + 1;
    var levels = [];

    $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        //var currentRow = $(this);
        //RowNo = parseInt(currentRow.find("#hfSNo").val()) + 1;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#hfSNo").val());
        levels.push(RowNo);

    });
    //if (RowNo == "0" || RowNo == "NaN") {
    //    RowNo = 1;
    //}
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    $('#SR_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">

<td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
<td class="sr_padding"><span id="SpanRowId">${RowNo}</span><input  type="hidden" id="hfSNo" value="${RowNo}" /></td>
<td><div class="lpo_form col-sm-11" style="padding:0px;"><select class="form-control" id="ItemName_${RowNo}" ></select><input  type="hidden" id="hf_ItemID" />
<span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
<td><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00"  > <span id="ItmCostError" class="error-message is-visible"></span></td>
<td><input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></td>
<td><textarea id="remarks" class="form-control remarksmessage" name="remarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}"    ></textarea></td>
</tr>`);
    /* $('#SR_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
 
 <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${RowNo}</span><input  type="hidden" id="hfSNo" value="${RowNo}" /></td>
 <td><div class="lpo_form col-sm-11" style="padding:0px;"><select class="form-control" id="ItemName_${RowNo}" ></select><input  type="hidden" id="hf_ItemID" />
 <span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}">  </button></div></td>
 <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
 <td><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00"  > <span id="ItmCostError" class="error-message is-visible"></span></td>
 <td><input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></td>
 <td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id_${RowNo}" onchange="OnChangeWarehouse(event);"><option value="0">---Select---</option></select><span id="wh_Error" class="error-message is-visible"></span></div></td>
 <td><input id="LotNumber" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="LotNumber"  onblur="this.placeholder='LotNumber'" disabled></td>
  <td class="center"><button type="button" class="calculator" id="BtnBatchDetail" onclick="OnClickBatchDetailBtn(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
  <td class="center"><button type="button" class="calculator" id="BtnSerialDetail" onclick="OnClickSerialDetailBtn(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
 <td><textarea id="remarks" class="form-control remarksmessage" name="remarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}"    ></textarea></td>
 </tr>`);*/
    /*BindSampleRcptItmList(RowNo);*/
    //BindWarehouseList(RowNo);
};

function BackBtnClick() {
    RemoveSession();
    SampleReceoptList();
}
function AddNewBtnClick() {
    RemoveSession();
    ResetSR_Detail();
    DisableTbleDetail();
    EnableHeaderDetail();
    EnableBatchDetail();
    EnableSerialDetail();
    ResetWF_Level();
    /*------------------Attatchmnet----------------*/
    $("#FilesUpload").css("display", "block");
    /*------------------Attatchmnet End----------------*/
    //$("#btn_close").attr('onclick', 'RefreshBtnClick()');
    //$("#btn_save").attr('onclick', 'OnClickSaveBtn()');
    //$("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    //$("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });


    /*$("#Cancelled").attr('disabled', true);*/
    $("#Cancelled").prop("checked", false);

    $("#btn_add_new_item").attr('onclick', '');
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Approve").attr('onclick', '');
    $("#btn_back").attr('onclick', '');


    $("#Btn_Forward").attr("data-target", "");
    //$("#Btn_Forward").attr('onclick', '');
    $("#Btn_Forward").prop('disabled', true);
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
}
//$("#btn_close").attr('onclick', 'RefreshBtnClick()');
//$("#btn_save").attr('onclick', 'OnClickSaveBtn()');
//$("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
//$("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

//$("#btn_add_new_item").attr('onclick', '');
//$("#Btn_Edit").attr('onclick', '');
//$("#Btn_Delete").attr('onclick', '');
//$("#Btn_Approve").attr('onclick', '');
//$("#btn_back").attr('onclick', '');
//$("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//$("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//$("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
//$("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

function RefreshBtnClick() {
    RemoveSession();
    ResetSR_Detail();
    DisableHeaderDetail();
    DisableHeaderDetail_Refresh();
    DisableTbleDetail();
    ResetWF_Level();
    /*------------------Attatchmnet----------------*/
    $("#FilesUpload").css("display", "none");
    $(".fileinput-remove").click();
    /*------------------Attatchmnet End----------------*/

    $("#Cancelled").prop("checked", false);
    $("#btn_back").attr('onclick', "BackBtnClick()");
    $("#btn_add_new_item").attr('onclick', "AddNewBtnClick()");
    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

    $("#btn_save").attr('onclick', '');
    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_close").attr('onclick', '');
    $("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Forward").attr("data-target", "");
    $("#Btn_Forward").attr('onclick', '');
    $("#Btn_Forward").prop('disabled', true);
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_back").attr('onclick', "BackBtnClick()");
    $("#btn_add_new_item").attr('onclick', "AddNewBtnClick()");
    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

    //$("#btn_save").attr('onclick', '');
    //$("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#btn_close").attr('onclick', '');
    //$("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Edit").attr('onclick', '');
    //$("#Btn_Delete").attr('onclick', '');
    //$("#Btn_Approve").attr('onclick', '');
    //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
}
function EditBtnClick() {
    debugger

    var OrderStatus = "";
    OrderStatus = $('#hfStatus').val().trim();
    if (OrderStatus == "A" || OrderStatus == "R") {
        $("#Cancelled").attr('disabled', false);

        $("#btn_save").attr('onclick', "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    else {
        ///*----------Attatchment --------------*/
        //if ($("#hdn_attatchment_list tbody tr").length < 5) {
        //    $("#file-1").attr('disabled', false);
        //    $("#FilesUpload").css("display", "block");
        //}
        //Cmn_EnableDeleteAttatchFiles();
        ///*----------Attatchment End--------------*/
        EnableForEdit();
        EnableBatchDetail();
        EnableSerialDetail();

        $("#btn_save").attr('onclick', "OnClickSaveBtn()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }

    sessionStorage.removeItem("SREnableDisable");
    sessionStorage.removeItem("SRTransType");
    sessionStorage.setItem("SRTransType", "Update");
    //$("#btn_add_new_item").attr('onclick', '');
    //$("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Edit").attr('onclick', '');
    //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Delete").attr('onclick', '');
    //$("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#Btn_Approve").attr('onclick', '');
    //$("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //$("#btn_back").attr('onclick', '');
    //$("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    if (OrderStatus === "A") {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }

    $("#Btn_Forward").attr("data-target", "");
    //$("#Btn_Forward").attr('onclick', '');
    $("#Btn_Forward").prop('disabled', true);
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_add_new_item").attr('onclick', '');
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_back").attr('onclick', '');
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

}
function DeleteBtnClick() {
    Delete_SRDetails();
    SerialNoAfterDelete();
    ResetWF_Level();
}
function Delete_SRDetails() {
    debugger;
    var SR_No = $("#SR_Number").val();
    var SR_Date = $("#SR_Date").val();
    if (SR_No != null && SR_No != "" && SR_Date != null && SR_Date != "") {
        try {
            swal({
                title: $("#deltital").text() + "?",
                text: $("#deltext").text() + "!",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: $("#yesdeleteit").text() + "!",
                closeOnConfirm: false
            },
                function () {
                    $.ajax({
                        type: "POST",
                        url: "/ApplicationLayer/SampleReceipt/DeleteSRDetails",/*Controller=SampleReceipt and Fuction=DeleteSRDetails*/
                        dataType: "json",
                        data: { SR_No: SR_No, SR_Date: SR_Date },
                        success: function (data) {
                            debugger;
                            if (data == 'ErrorPage') {
                                ErrorPage();
                                return false;
                            }
                            //    /*------------------Attatchmnet----------------*/
                            //    $("#FilesUpload").css("display", "none");
                            //    $(".fileinput-remove").click();
                            ///*------------------Attatchmnet End----------------*/
                            swal("", $("#deletemsg").text(), "success");

                            RemoveSession();
                            ResetSR_Detail();
                            DisableHeaderDetail();
                            DisableTbleDetail();
                            $("#btn_back").attr('onclick', "BackBtnClick()");
                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                            $("#btn_add_new_item").attr('onclick', "AddNewBtnClick()");
                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                            $("#Btn_Forward").attr("data-target", "");
                            //$("#Btn_Forward").attr('onclick', '');
                            $("#Btn_Forward").prop('disabled', true);
                            $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            $("#btn_save").attr('onclick', '');
                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            $("#btn_close").attr('onclick', '');
                            $("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            $("#Btn_Edit").attr('onclick', '');
                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            $("#Btn_Delete").attr('onclick', '');
                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            $("#Btn_Approve").attr('onclick', '');
                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                            swal("", $("#deletemsg").text(), "success");
                            //$("#btn_back").attr('onclick', "BackBtnClick()");
                            //$("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                            //$("#btn_add_new_item").attr('onclick', "AddNewBtnClick()");
                            //$("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                            //$("#btn_save").attr('onclick', '');
                            //$("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            //$("#btn_close").attr('onclick', '');
                            //$("#btn_close").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            //$("#Btn_Edit").attr('onclick', '');
                            //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            //$("#Btn_Delete").attr('onclick', '');
                            //$("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                            //$("#Btn_Approve").attr('onclick', '');
                            //$("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                        },
                        error: function (Data) {

                        }
                    });
                });
        } catch (err) {
            console.log("Sample Receipt Error : " + err.message);
        }
    }
}
function SampleReceoptList() {
    try {
        location.href = "/ApplicationLayer/SampleReceipt/SampleReceiptList";
    } catch (err) {
    }
}
function ApproveBtnClick() {
    Approve_SampleReceipt("", "", "");
}

function AddNewRow() {
    debugger;
    if (CheckValidations() == false) {
        return false;
    }

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
<td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
<td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td>
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
<input type="hidden" id="hfbatchable" />
<input type="hidden" id="hfserialable" />
<input type="hidden" id="hfexpiralble" />
</td>
<td>
<div class="lpo_form">
<input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="ItmCostError" class="error-message is-visible"></span>
</div>
</td>
<td>
<div class="lpo_form">
<input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00" onblur="this.placeholder='0000.00'" >
<span id="ReceivedQtyError" class="error-message is-visible"></span>
</div>
</td>
<td>
<textarea id="remarks" class="form-control remarksmessage" @maxlength = "100", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea>
</td>
<td style="display: none;"><input type="hidden" id="hfSNo" value="${RowNo}" /></td>
</tr>`);
    /*    $('#SR_ItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
    <td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
    <td class="sr_padding"><span id="SpanRowId">${rowCount}</span><input  type="hidden" id="hfSNo" value="${RowNo}" /></td>
    <td><div class="lpo_form col-sm-11" style="padding:0px;"><select class="form-control" id="ItemName_${RowNo}" onchange="BindSR_ItemList(event);" ></select><input  type="hidden" id="hf_ItemID" />
    <span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div></td>
    <td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
    <td><div class="lpo_form"><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00"  > <span id="ItmCostError" class="error-message is-visible"></span></div></td>
    <td><div class="lpo_form"><input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></div></td>
    <td width="7%"><div class="lpo_form"><select class="form-control" id="wh_id_${RowNo}" onchange="OnChangeWarehouse(event);"><option value="0">---Select---</option></select><span id="wh_Error" class="error-message is-visible"></span></div></td>
    <td><input id="LotNumber" class="form-control" autocomplete="off" type="text" name="LotNumber" placeholder="LotNumber"  onblur="this.placeholder='LotNumber'" disabled></td>
     <td class="center"><button type="button" class="calculator" id="BtnBatchDetail" onclick="OnClickBatchDetailBtn(event)" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
     <td class="center"><button type="button" class="calculator" id="BtnSerialDetail" onclick="OnClickSerialDetailBtn(event)" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false" disabled><i class="fa fa-eye" aria-hidden="true" data-toggle="" title=""></i></button></td>
    <td><textarea id="remarks" class="form-control remarksmessage" @maxlength = "100", name="remarks"  placeholder="${$("#span_remarks").text()}"    ></textarea></td>
    </tr>`);*/
    BindSampleRcptItmList(RowNo);
    //BindWarehouseList(RowNo);
    //HideItemListItm();
    HideSelectedItem("#ItemName_", "", "#SR_ItmDetailsTbl", "#hfSNo");
};
function BindSampleRcptItmList(ID) {
    debugger;
    BindItemList("#ItemName_", ID, "#SR_ItmDetailsTbl", "#hfSNo","","SR");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/SampleReceipt/GetSR_ItemList",
    //        data: function (params) {
    //            var queryParameters = {
    //                SR_Item: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                Error_Page();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {

    //                    sessionStorage.removeItem("ItemList");
    //                    sessionStorage.setItem("ItemList", JSON.stringify(arr.Table));

    //                    $('#ItemName_' + ID).append(`<optgroup class='def-cursor' id="Textddl${ID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl' + ID).append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ItemName_' + ID).select2({
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
    //                    HideItemListItm();
    //                }
    //            }
    //        },
    //    });
}
function BindWarehouseList(id) {
    //debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/GRNDetail/GetWarehouseList",
            data: {},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {

                        sessionStorage.removeItem("SR_WHList");
                        sessionStorage.setItem("SR_WHList", JSON.stringify(arr.Table));

                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        $("#wh_id_" + id).html(s);
                        debugger;
                        var FItmDetails = JSON.parse(sessionStorage.getItem("SRItemDetailSession"));
                        if (FItmDetails != null) {
                            if (FItmDetails.length > 0) {
                                for (i = 0; i < FItmDetails.length; i++) {
                                    var Wh_ID = FItmDetails[i].wh_id;
                                    var Item_ID = FItmDetails[i].item_id;

                                    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
                                        var currentRow = $(this);
                                        var SNo = currentRow.find("#hfSNo").val();

                                        var ItmID = currentRow.find("#hf_ItemID").val();
                                        if (ItmID == Item_ID) {
                                            currentRow.find("#wh_id_" + SNo).val(Wh_ID).prop('selected', true);
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            },
        });
}
function Approve_SampleReceipt(status, clevel, remarks) {
    debugger;
    var SR_No = $("#SR_Number").val();
    var SR_Date = $("#SR_Date").val();

    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {

        if (SR_No != null && SR_No != "" && SR_Date != null && SR_Date != "") {
            debugger;
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/SampleReceipt/Approve_SampleReceiptDetails",/*Controller=SampleReceipt and Fuction=Approve_SampleReceiptDetails*/
                dataType: "json",
                data: { SR_No: SR_No, SR_Date: SR_Date, A_Status: status, A_Level: clevel, A_Remarks: remarks },/*Registration pass value like model*/
                success: function (Data) {
                    debugger;
                    if (Data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    var FData = Data.split(',');
                    var StatusC = FData[1].trim();
                    var SR_Number = FData[0].trim();

                    $("#SR_Number").val(SR_Number);
                    $("#hfStatus").val(StatusC);
                    $("#SRStatus").text(FData[2]);
                    $("#hdDoc_No").val(SR_Number);

                    sessionStorage.removeItem("SR_No");
                    sessionStorage.setItem("SR_No", SRStatus);

                    GetCurrentDatetime("Approved");
                    swal("", $("#approvemsg").text(), "success");

                    $("#SR_Number").val(SR_Number);

                    if (clevel != "" && clevel != null) {
                        var Level = parseInt(clevel);
                        if (status === "Approve") {
                            $("#a_" + Level).removeClass("disabled");
                            $("#a_" + Level).addClass("done");
                        }
                    }
                    if (StatusC == "A") {
                        $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                        $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    }
                    $("#Btn_Delete").attr('onclick', '');
                    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Btn_Approve").attr('onclick', '');
                    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#Btn_Forward").attr("data-target", "");
                    //$("#Btn_Forward").attr('onclick', '');
                    $("#Btn_Forward").prop('disabled', true);
                    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#Btn_Print").attr('onclick', '');
                    $("#Btn_Print").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#Btn_Mail").attr('onclick', '');
                    $("#Btn_Mail").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#Btn_Message").attr('onclick', '');
                    $("#Btn_Message").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    //if (Data !== null && Data !== "") /*For Chekin Json Data is return or not */ {
                    //    var arr = [];
                    //    debugger;
                    //    arr = JSON.parse(Data);

                    //    if (arr.Table.length > 0) {
                    //        for (var i = 0; i < arr.Table.length; i++) {
                    //            var ItemID = "";
                    //            var LotNo = "";
                    //            ItemID = arr.Table[i].item_id;
                    //            LotNo = arr.Table[i].lot_id;

                    //            $('#SRStatus').text(arr.Table[0].status_name);
                    //            $('#hfStatus').val(arr.Table[0].status_code.trim());

                    //            $("#SR_ItmDetailsTbl >tbody >tr").each(function (i, row) {
                    //                debugger;
                    //                var item_id = "";
                    //                var currentRow = $(this);

                    //                item_id = currentRow.find("#hf_ItemID").val();

                    //                if (item_id == ItemID) {
                    //                    currentRow.find("#LotNumber").val(LotNo);
                    //                }
                    //            });
                    //        }
                    //    }


                    //    GetCurrentDatetime("Approved");

                    //    swal("", $("#approvemsg").text(), "success");

                    //    $("#btn_save").attr('onclick', '');
                    //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    //$("#Btn_Edit").attr('onclick', '');
                    //    //$("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Delete").attr('onclick', '');
                    //    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //    $("#Btn_Approve").attr('onclick', '');
                    //    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                    //    $("#Btn_Print").attr('onclick', '');
                    //    $("#Btn_Print").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    //}
                },
                error: function (Data) {
                }
            });
        }
    }
    else {
        alert("Check network");
    }
};

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#hfSNo").val();
    var ItmCode = "";
    var ItmName = "";
    ItmCode = clickedrow.find("#hf_ItemID").val();
    ItemInfoBtnClick(ItmCode);
    //ItmName = clickedrow.find("#ItemName_" + SNo + " option:selected").text();

    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DPO/GetPOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                       ErrorPage();
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
function QtyFloatValueonly(el, evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //$("#SpanBatchQty").css("display", "none");
    //$("#BatchQuantity").css("border-color", "#ced4da");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function QtyFloatValueonlyRecQty(el, evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //$("#ReceivedQtyError").css("display", "none");
    //$("#ReceivedQuantity").css("border-color", "#ced4da");
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
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
    //$("#ItmCostError").css("display", "none");
    //$("#TxtItemCost").css("border-color", "#ced4da");
    //var QtyDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}
function EnableForEdit() {
    $("#DivAddNewItemBtn").css("display", "block");
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();

        var Batch = currentRow.find("#hfbatchable").val();
        var Serial = currentRow.find("#hfserialable").val();

        currentRow.find("#delBtnIcon").css("display", "block");
        currentRow.find("#ItemName_" + Sno).attr("disabled", false);
        currentRow.find("#TxtItemCost").prop("readonly", false);
        currentRow.find("#ReceivedQuantity").prop("readonly", false);
        currentRow.find("#wh_id_" + Sno).attr("disabled", false);
        currentRow.find("#remarks").prop("readonly", false);

        if (Batch == "Y") {
            currentRow.find("#BtnBatchDetail").attr("disabled", false);
        }
        else {
            currentRow.find("#BtnBatchDetail").attr("disabled", true);
        }
        if (Serial == "Y") {
            currentRow.find("#BtnSerialDetail").attr("disabled", false);
        }
        else {
            currentRow.find("#BtnSerialDetail").attr("disabled", true);
        }
    });

    $("#file-1").attr('disabled', false);
}
function ResetSR_Detail() {
    GenrateBlankRow();
    ClearHeaderDetails();
    ClearBatch_SerialDetails();
    ///*------Attatchment------------*/
    //$("#PartialImageBind").html("");
    //$("#file-1").prop("disabled", false);
    ///*------Attatchment------------*/
    //EnableForEdit();
}
function ClearHeaderDetails() {
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

function ClearBatch_SerialDetails() {
    $('#BatchQuantity').val("");
    $('#txtBatchNumber').val("");
    $('#BatchExpiryDate').val("");
    $('#BatchQtyTotal').text("0");
    $("#BatchDetailTbl >tbody >tr").remove();
    $('#SerialNo').val("");
    $("#SerialDetailTbl >tbody >tr").remove();
}
function GenrateBlankRow() {
    $('#SR_ItmDetailsTbl tbody tr').remove();
    $('#SR_ItmDetailsTbl tbody').append(` <tr>
<td class=" red center"> <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon" title="@Resource.Delete"></i></td>
<td class="sr_padding"><span id="SpanRowId">1</span><input  type="hidden" id="hfSNo" value="1" /></td>
<td><div class=" col-sm-11" style="padding:0px;"><select class="form-control" id="ItemName_1" onchange="BindSR_ItemList(event);" ></select><input  type="hidden" id="hf_ItemID" />
<span id="ItemNameError" class="error-message is-visible"></span></div><div class="col-sm-1 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator item_pop" data-toggle="modal" onclick="OnClickIconBtn(event);" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button></div></td>
<td><input id="UOM" class="form-control" autocomplete="off" type="text" name="idUOM" placeholder="${$("#ItemUOM").text()}" readonly="readonly"><input id="UOMID" type="hidden" /><input type="hidden" id="hfbatchable" /><input type="hidden" id="hfserialable" /><input type="hidden" id="hfexpiralble" /></td>
<td><input id="TxtItemCost" class="form-control num_right" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" onchange="OnChangeItemCost(event);" type="text" name="TxtItemCost" placeholder="0000.00"  > <span id="ItmCostError" class="error-message is-visible"></span></td>
<td><input id="ReceivedQuantity" class="form-control num_right" name="ReceivedQuantity" autocomplete="off" onkeypress="return QtyFloatValueonlyRecQty(this,event);" type="text" onchange="OnChangeReceivedQty(event);" placeholder="0000.00"  ><span id="ReceivedQtyError" class="error-message is-visible"></span></td>
<td><textarea id="remarks" class="form-control remarksmessage" name="remarks" @maxlength = "100",  placeholder="${$("#span_remarks").text()}"    ></textarea></td>
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
    BindWarehouseList(1);
}
function OnClickSaveBtn() {
    debugger;
    Insert_SR_Details();
}

function DisableHeaderDetail_Refresh() {

    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);
    $("#remarks").attr('disabled', true);

    $("#EntityType").val(0).prop('selected', true);;
    $("#EntityName").val(0).trigger('change');

    $("#EntityType").css("border-color", "#ced4da");
    $("#SpanEntityTypeErrorMsg").css("display", "none");

    $("#SpanEntityNameErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");

}
function DisableBatchDetail() {

    $("#BatchQuantity").attr('readonly', true);
    $("#txtBatchNumber").attr('readonly', true);
    $("#BatchExpiryDate").attr('readonly', true);

    $("#DivAddNewBatch").css("display", "none");

    $("#BatchDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#BatchDeleteIcon").css("display", "none");
    });
    //$("#BatchDeleteIcon").css("display", "none");

    $("#BatchResetBtn").css("display", "none");
    $("#BatchSaveAndExitBtn").css("display", "none");
    $("#BatchDiscardAndExitBtn").css("display", "none");
}
function DisableSerialDetail() {
    $("#SerialNo").attr('readonly', true);
    $("#DivAddNewSerial").css("display", "none");

    $("#SerialDetailTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#SerialDeleteIcon").css("display", "none");
    });

    $("#SerialResetBtn").css("display", "none");
    $("#SerialSaveAndExitBtn").css("display", "none");
    $("#SerialDiscardAndExitBtn").css("display", "none");
}
function DisableItemDetail() {
    $("#DivAddNewItemBtn").css("display", "none");
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();
        var Batch = currentRow.find("#hfbatchable").val();
        var Serial = currentRow.find("#hfserialable").val();

        currentRow.find("#delBtnIcon").css("display", "none");
        currentRow.find("#ItemName_" + Sno).attr("disabled", true);
        currentRow.find("#TxtItemCost").prop("readonly", true);
        currentRow.find("#ReceivedQuantity").prop("readonly", true);
        currentRow.find("#wh_id_" + Sno).attr("disabled", true);
        currentRow.find("#remarks").prop("readonly", true);

        if (Batch == "Y") {
            currentRow.find("#BtnBatchDetail").attr("disabled", false);
        }
        else {
            currentRow.find("#BtnBatchDetail").attr("disabled", true);
        }
        if (Serial == "Y") {
            currentRow.find("#BtnSerialDetail").attr("disabled", false);
        }
        else {
            currentRow.find("#BtnSerialDetail").attr("disabled", true);
        }
    });
}
function EnableBatchDetail() {

    $("#BatchQuantity").attr('readonly', false);
    $("#txtBatchNumber").attr('readonly', false);
    $("#BatchExpiryDate").attr('readonly', false);

    $("#DivAddNewBatch").css("display", "block");

    $("#BatchDetailTbl .deleteIcon").css("display", "block");

    $("#BatchResetBtn").css("display", "block");
    $("#BatchSaveAndExitBtn").css("display", "block");
    $("#BatchDiscardAndExitBtn").css("display", "block");
    OnClickDeleteIcon();
}
function EnableSerialDetail() {

    $("#SerialNo").attr('readonly', false);

    $("#DivAddNewSerial").css("display", "block");
    $("#SerialDeleteIcon").css("display", "block");

    $("#SerialResetBtn").css("display", "block");
    $("#SerialSaveAndExitBtn").css("display", "block");
    $("#SerialDiscardAndExitBtn").css("display", "block");
    OnClickSerialDeleteIcon();
}

function DisableAfterSaveDetail() {
    DisableHeaderDetail();
    DisableItemDetail();
    DisableBatchDetail();
    DisableSerialDetail();

    $("#file-1").attr('disabled', true);

    sessionStorage.removeItem("SREnableDisable");
    sessionStorage.setItem("SREnableDisable", "Disabled");
}





function Insert_SR_Details() {
    debugger;
    var DecDigit = $("#ValDigit").text();
    if (CheckValidations() == false) {
        return false;
    }

    var flag = CheckSR_ItemValidations();
    if (flag == false) {
        return false;
    }
    //var Batchflag = CheckItemBatchValidation();
    //if (Batchflag == false) {
    //    return false;
    //}
    //var SerialFlag = CheckItemSerialValidation();
    //if (SerialFlag == false) {
    //    return false;
    //}
    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
    if ($('#hfStatus').val() === "" || $('#hfStatus').val() === null) {
        OrdStatus = "D";
        SRStatus = "D";
    }
    else {
        OrdStatus = $('#hfStatus').val();
        SRStatus = $('#hfStatus').val();
    }

    if (flag == true /*&& Batchflag == true && SerialFlag == true*/) {
        var FinalHeaderDetail = [];
        var FinalItemDetail = [];
        //var FinalItemBatchDetail = [];
        //var FinalItemSerialDetail = [];

        if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);

            /*----- Attatchment End--------*/
            FinalHeaderDetail = GetSR_HeaderDetails();
            FinalItemDetail = GetSR_ItemDetails();
            //FinalItemBatchDetail = GetSR_ItemBatchDetails();
            //FinalItemSerialDetail = GetSR_ItemSerialDetails();
            debugger;

            if (FinalHeaderDetail != null) {
                if (FinalHeaderDetail.length > 0) {
                    for (i = 0; i < FinalHeaderDetail.length; i++) {
                        var Cancel = FinalHeaderDetail[i].Cancelled;
                        var SRNo = FinalHeaderDetail[i].sr_no;
                        var SRDate = FinalHeaderDetail[i].sr_dt;

                        if (Cancel == "Y") {
                            $.ajax({
                                type: "POST",
                                url: "/ApplicationLayer/SampleReceipt/CheckedSampleReceipt_ItemStock",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                data: JSON.stringify({ SR_No: SRNo, SR_Date: SRDate }),
                                success: function (Data) {
                                    debugger;
                                    if (Data == 'ErrorPage') {
                                        ErrorPage();
                                        return false;
                                    }
                                    else {

                                        var FData = Data.trim();
                                        if (FData == "Y") {
                                            swal("", $("#OrilotqtyhasbeenchangedDoccantbecancelled").text(), "warning");
                                            $("#Cancelled").prop("checked", false);
                                            $("#Cancelled").attr('disabled', true);

                                            $("#btn_save").attr('onclick', '');
                                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#btn_back").attr('onclick', 'BackBtnClick()');
                                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#Btn_Edit").attr('onclick', '');
                                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#btn_close").attr('onclick', 'RefreshBtnClick()');

                                            return false;
                                        }
                                        if (FData == "N") {
                                            $.ajax({
                                                type: "POST",
                                                url: "/ApplicationLayer/SampleReceipt/Insert_SampleReceiptDetails",/*Controller=SampleReceipt and Fuction=Insert_SampleReceiptDetails*/
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                async: true,
                                                data: JSON.stringify({ SR_HeaderDetail: FinalHeaderDetail, SR_ItemDetail: FinalItemDetail, AttachDeatil: ItemAttchmentDt/*, SR_ItemBatchDetail: FinalItemBatchDetail, SR_ItemSerialDetail: FinalItemSerialDetail*/ }),
                                                success: function (Data) {
                                                    debugger;
                                                    if (Data == 'ErrorPage') {
                                                        ErrorPage();
                                                        return false;
                                                    }
                                                    else {

                                                        var FData = Data.split(',');
                                                        var StatusC = FData[1].trim();
                                                        var SRNumber = FData[0].trim();
                                                        /*------------------Attatchmnet----------------*/
                                                        ShowSavedAttatchMentFiles(FData[0], $("#SR_Date").val());
                                                        $("#FilesUpload").css("display", "none");
                                                        $(".fileinput-remove").click();
                                                        /*------------------Attatchmnet End----------------*/

                                                        /*  var StatusC = FData[].trim();*/

                                                        $("#SR_Number").val(/*FData[0]*/SRNumber);
                                                        $("#hfStatus").val(StatusC);
                                                        $("#SRStatus").text(FData[2]);
                                                        $("#hdDoc_No").val(SRNumber);

                                                        if (StatusC != 'A') {
                                                            $("#SR_Number").val(SRNumber);
                                                        }

                                                        var SR_Status = StatusC;
                                                        var SRTransType = sessionStorage.getItem("SRTransType");
                                                        if (SR_Status === 'D' && SRTransType != "Update") {
                                                            GetCurrentDatetime("Save");
                                                        }
                                                        sessionStorage.removeItem("SRTransType");

                                                        if (SR_Status === "C") {
                                                            swal("", $("#cancelmsg").text(), "success");

                                                            GetCurrentDatetime("Cancelled");

                                                            $("#Cancelled").attr('disabled', true);

                                                            $("#btn_save").attr('onclick', '');
                                                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                            $("#btn_back").attr('onclick', 'BackBtnClick()');
                                                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#Btn_Edit").attr('onclick', '');
                                                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                            $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                                            $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#Btn_Delete").attr('onclick', '');
                                                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                            $("#Btn_Approve").attr('onclick', '');
                                                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                                                        }
                                                        else {
                                                            swal("", $("#savemsg").text(), "success");
                                                            $("#Cancelled").attr('disabled', true);
                                                            $("#btn_save").attr('onclick', '');
                                                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                            $("#btn_back").attr('onclick', 'BackBtnClick()');
                                                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                                                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                                            $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#Btn_Delete").attr('onclick', 'DeleteBtnClick()');
                                                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                            $("#Btn_Approve").attr('onclick', 'ApproveBtnClick()');
                                                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                        }
                                                        DisableAfterSaveDetail();
                                                    }
                                                },
                                                error: function (Data) {
                                                }
                                            });
                                        }
                                    }
                                },
                                error: function (Data) {
                                }
                            });
                        }
                        else {
                            $.ajax({
                                type: "POST",
                                url: "/ApplicationLayer/SampleReceipt/Insert_SampleReceiptDetails",/*Controller=SampleReceipt and Fuction=Insert_SampleReceiptDetails*/
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                data: JSON.stringify({ SR_HeaderDetail: FinalHeaderDetail, SR_ItemDetail: FinalItemDetail, AttachDeatil: ItemAttchmentDt/*, SR_ItemBatchDetail: FinalItemBatchDetail, SR_ItemSerialDetail: FinalItemSerialDetail */ }),
                                success: function (Data) {
                                    debugger;
                                    if (Data == 'ErrorPage') {
                                        ErrorPage();
                                        return false;
                                    }
                                    else {

                                        var FData = Data.split(',');
                                        var StatusC = FData[1].trim();
                                        var SRNumber = FData[0].trim();
                                        /*------------------Attatchmnet----------------*/
                                        ShowSavedAttatchMentFiles(FData[0], $("#SR_Date").val());
                                        $("#FilesUpload").css("display", "none");
                                        $(".fileinput-remove").click();
                                        /*------------------Attatchmnet End----------------*/

                                        $("#SR_Number").val(/*FData[0]*/SRNumber);
                                        $("#hfStatus").val(StatusC);
                                        $("#SRStatus").text(FData[2]);
                                        $("#hdDoc_No").val(SRNumber);

                                        if (StatusC != 'A') {
                                            $("#SR_Number").val(SRNumber);
                                        }

                                        var SR_Status = StatusC;
                                        var SRTransType = sessionStorage.getItem("SRTransType");
                                        if (SR_Status === 'D' && SRTransType != "Update") {
                                            GetCurrentDatetime("Save");
                                        }
                                        sessionStorage.removeItem("SRTransType");
                                        SerialNoAfterDelete();

                                        debugger;
                                        swal("", $("#savemsg").text(), "success");
                                        var WFLen = $("#wizard > ul > li").length;
                                        if (WFLen > 0) {
                                            if (SR_Status === 'D') {
                                                $("#Btn_Forward").prop('disabled', false);
                                                $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                                                $("#Btn_Forward").attr('onclick', 'ForwardBtnClick()');
                                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            }
                                            else {
                                                $("#Btn_Forward").attr("data-target", "");
                                                //$("#Btn_Forward").attr('onclick', '');
                                                $("#Btn_Forward").prop('disabled', true);
                                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            }
                                            if (parseInt(FData[3]) === 0) {
                                                //if (Userid === Createrid) {
                                                $("#hd_nextlevel").val(parseInt(FData[3]));
                                                $("#hd_currlevel").val(parseInt(FData[4]));

                                                $("#Btn_Approve").attr("data-toggle", "modal");
                                                $("#Btn_Approve").attr("data-target", "#Forward_Pop");
                                                $("#Btn_Approve").attr('onclick', 'ForwardBtnClick()');
                                                $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                                                $("#span_forward").css("display", "none");
                                                $("#span_approve").removeAttr("style");
                                                $("#radio_reject").attr("disabled", true);
                                                $("#radio_revert").attr("disabled", true);

                                                $("#radio_forward").val("Approve");

                                                $("#Btn_Forward").attr("data-target", "");
                                                //$("#Btn_Forward").attr('onclick', '');
                                                $("#Btn_Forward").prop('disabled', true);
                                                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                // }
                                            }
                                        } else {
                                            $("#Btn_Approve").attr('onclick', 'ApproveBtnClick()');
                                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        }




                                        $("#radio_reject").attr("disabled", true);
                                        $("#radio_revert").attr("disabled", true);

                                        if (StatusC === 'C' || StatusC === 'FC') {
                                            if (ForceClosed == "Y" || Cancelled == "Y") {
                                                if (Cancelled == "Y") {
                                                    swal("", $("#cancelmsg").text(), "success");
                                                }

                                                $("#Btn_Edit").attr('onclick', '');
                                                $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                $("#Btn_Print").attr('onclick', '');
                                                $("#Btn_Print").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                $("#Btn_Mail").attr('onclick', '');
                                                $("#Btn_Mail").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                                $("#Btn_Message").attr('onclick', '');
                                                $("#Btn_Message").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            }
                                            else {
                                                debugger;
                                                $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                                                $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                $("#Btn_Print").attr('onclick', '');
                                                $("#Btn_Print").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                $("#Btn_Mail").attr('onclick', '');
                                                $("#Btn_Mail").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                                $("#Btn_Message").attr('onclick', '');
                                                $("#Btn_Message").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            }
                                            DisableAfterSaveDetail();
                                            $("#btn_save").attr('onclick', '');
                                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#btn_back").attr('onclick', 'BackBtnClick()');
                                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#Btn_Delete").attr('onclick', '');
                                            $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#Btn_Approve").attr('onclick', '');
                                            $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                            $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        }
                                        else {
                                            debugger;
                                            $("#btn_save").attr('onclick', '');
                                            $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                            $("#btn_back").attr('onclick', 'BackBtnClick()');
                                            $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                            $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                                            $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                            $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                            if (StatusC === "D") {
                                                $("#Btn_Delete").attr('onclick', 'DeleteBtnClick()');
                                                $("#Btn_Delete").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });

                                            }
                                        }
                                        //if (SR_Status === "C") {
                                        //    swal("", $("#cancelmsg").text(), "success");

                                        //    GetCurrentDatetime("Cancelled");

                                        //    $("#Cancelled").attr('disabled', true);

                                        //    $("#btn_save").attr('onclick', '');
                                        //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                        //    $("#btn_back").attr('onclick', 'BackBtnClick()');
                                        //    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                        //    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#Btn_Edit").attr('onclick', '');
                                        //    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                        //    $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                        //    $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#Btn_Delete").attr('onclick', '');
                                        //    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                        //    $("#Btn_Approve").attr('onclick', '');
                                        //    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

                                        //}
                                        //else {
                                        //    swal("", $("#savemsg").text(), "success");
                                        //    $("#Cancelled").attr('disabled', true);
                                        //    $("#btn_save").attr('onclick', '');
                                        //    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                                        //    $("#btn_back").attr('onclick', 'BackBtnClick()');
                                        //    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#btn_add_new_item").attr('onclick', 'AddNewBtnClick()');
                                        //    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#Btn_Edit").attr('onclick', 'EditBtnClick()');
                                        //    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#btn_close").attr('onclick', 'RefreshBtnClick()');
                                        //    $("#btn_close").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#Btn_Delete").attr('onclick', 'DeleteBtnClick()');
                                        //    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //    $("#Btn_Approve").attr('onclick', 'ApproveBtnClick()');
                                        //    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                                        //}
                                        DisableAfterSaveDetail();
                                    }
                                },
                                error: function (Data) {
                                }
                            });
                        }
                    }
                }
            }
        }
        else {
            //alert("Check network");
        }
    }
};
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
function GetSR_HeaderDetails() {

    var SRTransType = sessionStorage.getItem("SRTransType");
    var SR_HDeatil = [];

    debugger;
    var TransType = "";
    var MenuID = "";
    var Cancelled = "";
    var comp_id = "";
    var br_id = "";
    var sr_no = "";
    var sr_dt = "";
    var entitytype = "";
    var entityid = "";
    var user_id = "";
    var sr_status = "";
    var mac_id = "";
    var sr_val = 0;
    var remarks = "";

    if (SRTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }

    comp_id = 0;
    br_id = 0;
    sr_no = $("#SR_Number").val();
    sr_dt = $("#SR_Date").val();
    entitytype = $("#EntityType").val();
    entityid = $("#EntityName").val();
    if ($("#Cancelled").is(":checked")) {
        Cancelled = "Y";
    }
    else {
        Cancelled = "N";
    }
    user_id = 0;
    sr_status = "D";
    mac_id = "";
    //sr_val = $("#grnGrossValue").val();
    remarks = $("#remarks").val();

    SR_HDeatil.push({ TransType: TransType, MenuID: MenuID, Cancelled: Cancelled, comp_id: comp_id, br_id: br_id, sr_no: sr_no, sr_dt: sr_dt, entity_name: entitytype, entity_id: entityid, user_id: user_id, sr_status: sr_status, mac_id: mac_id, sr_val: sr_val, remarks: remarks });

    return SR_HDeatil;
};
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
        var it_remarks = "";

        var currentRow = $(this);

        var SNo = currentRow.find("#hfSNo").val();
        item_id = currentRow.find("#hf_ItemID").val();
        uom_id = currentRow.find("#UOMID").val();
        rec_qty = currentRow.find("#ReceivedQuantity").val();
        //wh_id = currentRow.find("#wh_id_" + SNo).val();
        item_rate = currentRow.find("#TxtItemCost").val();
        it_remarks = currentRow.find("#remarks").val();

        SRItemsDetail.push({ item_id: item_id, uom_id: uom_id, rec_qty: rec_qty, lot_id: lot_id, wh_id: wh_id, item_rate: item_rate, it_remarks: it_remarks });
    });
    return SRItemsDetail;
};
function GetSR_ItemBatchDetails() {

    var SRItemsBatchDetail = [];

    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            for (i = 0; i < FBatchDetails.length; i++) {
                var ItemID = FBatchDetails[i].ItemID;
                var BatchNo = FBatchDetails[i].BatchNo;
                var BatchQty = FBatchDetails[i].BatchQty;
                var BatchExDate = FBatchDetails[i].BatchExDate;

                SRItemsBatchDetail.push({ item_id: ItemID, batch_no: BatchNo, batch_qty: BatchQty, exp_dt: BatchExDate });
            }
        }
    }

    return SRItemsBatchDetail;
};
function GetSR_ItemSerialDetails() {
    var SRItemsSerialDetail = [];

    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails != null) {
        if (FSerialDetails.length > 0) {
            for (i = 0; i < FSerialDetails.length; i++) {
                var ItemID = FSerialDetails[i].ItemID;
                var SerialNo = FSerialDetails[i].SerialNo;

                SRItemsSerialDetail.push({ item_id: ItemID, serial_no: SerialNo });
            }
        }
    }

    return SRItemsSerialDetail;
};
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr('onclick', "OnClickSaveBtn()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function SR_Detail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/SampleReceipt/SampleReceiptDetail";
    } catch (err) {
        console.log("SR Error : " + err.message);
    }
}

function CheckHeaderValidations() {
    debugger;
    var ErrorFlag = "N";

    var EntityType = $('#EntityType').val();
    var EntityName = $('#EntityName').val();

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
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function CheckSR1_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#hfSNo").val();

        if (currentRow.find("#ItemName_" + Sno).val() == "" || currentRow.find("#ItemName_" + Sno).val() == "0") {
            currentRow.find("#ItemNameError").text($("#valueReq").text());
            currentRow.find("#ItemNameError").css("display", "block");
            currentRow.find("[aria-labelledby='select2-ItemName_" + Sno + "-container']").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItemNameError").css("display", "none");
            currentRow.find("#ItemName_" + Sno).css("border-color", "#ced4da");
        }

        if (currentRow.find("#TxtItemCost").val() == "" || parseFloat(currentRow.find("#TxtItemCost").val()) == parseFloat("0")) {
            currentRow.find("#ItmCostError").text($("#valueReq").text());
            currentRow.find("#ItmCostError").css("display", "block");
            currentRow.find("#TxtItemCost").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ItmCostError").css("display", "none");
            currentRow.find("#TxtItemCost").css("border-color", "#ced4da");
        }
        if (currentRow.find("#ReceivedQuantity").val() == "" || parseFloat(currentRow.find("#ReceivedQuantity").val()) == parseFloat("0")) {
            currentRow.find("#ReceivedQtyError").text($("#valueReq").text());
            currentRow.find("#ReceivedQtyError").css("display", "block");
            currentRow.find("#ReceivedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ReceivedQtyError").css("display", "none");
            currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
        }

        if (currentRow.find("#wh_id_" + Sno).val() == "" || currentRow.find("#wh_id_" + Sno).val() == "0") {
            currentRow.find("#wh_Error").text($("#valueReq").text());
            currentRow.find("#wh_Error").css("display", "block");
            currentRow.find("#wh_id_" + Sno).css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#wh_Error").css("display", "none");
            currentRow.find("#wh_id_" + Sno).css("border-color", "#ced4da");
        }
    })
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
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ReceivedQuantity = clickedrow.find("#ReceivedQuantity").val();
        var ItemId = clickedrow.find("#hf_ItemID").val();
        var Batchable = clickedrow.find("#hfbatchable").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
            if (FBatchDetails != null) {
                if (FBatchDetails.length > 0) {
                    for (i = 0; i < FBatchDetails.length; i++) {
                        var BItemID = FBatchDetails[i].ItemID;
                        var BBatchQty = FBatchDetails[i].BatchQty;
                        if (ItemId == BItemID) {
                            TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(BBatchQty);
                        }
                    }
                    if (parseFloat(ReceivedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                        clickedrow.find("#BtnBatchDetail").css("border", "1px solid");
                        clickedrow.find("#BtnBatchDetail").css("border-color", "#007bff");
                    }
                    else {
                        clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                        clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                        BatchableFlag = "Y";
                        EmptyFlag = "N";
                    }
                }
                else {
                    clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                    clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                    BatchableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {
                clickedrow.find("#BtnBatchDetail").css("border", "2px solid");
                clickedrow.find("#BtnBatchDetail").css("border-color", "red");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }

        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithreceivedqty").text(), "warning");
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
function CheckItemSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#SR_ItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var ReceivedQuantity = clickedrow.find("#ReceivedQuantity").val();
        var ItemId = clickedrow.find("#hf_ItemID").val();
        //var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hfserialable").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");

            var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
            if (FSerialDetails != null) {
                if (FSerialDetails.length > 0) {
                    for (i = 0; i < FSerialDetails.length; i++) {
                        var SItemID = FSerialDetails[i].ItemID;
                        if (ItemId == SItemID) {
                            TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(1);
                        }
                    }
                    if (parseFloat(ReceivedQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                        clickedrow.find("#BtnSerialDetail").css("border", "1px solid");
                        clickedrow.find("#BtnSerialDetail").css("border-color", "#007bff");
                    }
                    else {
                        clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                        clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                        SerialableFlag = "Y";
                        EmptyFlag = "N";
                    }
                }
                else {

                    clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                    clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                    SerialableFlag = "Y";
                    EmptyFlag = "Y";
                }
            }
            else {

                clickedrow.find("#BtnSerialDetail").css("border", "2px solid");
                clickedrow.find("#BtnSerialDetail").css("border-color", "red");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithreceivedqty").text(), "warning");
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

function OnChnageWarehouse(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#hfSNo").val();

    if (currentRow.find("#wh_id_" + Sno).val() == "0" || currentRow.find("#wh_id_" + Sno).val() == "" || currentRow.find("#wh_id_" + Sno).val() == null) {
        currentRow.find("#wh_Error").text($("#valueReq").text());
        currentRow.find("#wh_Error").css("display", "block");
        currentRow.find("#wh_id_" + Sno).css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#wh_Error").css("display", "none");
        currentRow.find("#wh_id_" + Sno).css("border", "1px solid #aaa");
    }
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


function OnChangeWarehouse(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#hfSNo").val();
    var Warehouse = clickedrow.find("#wh_id_" + Sno).val();
    if (Warehouse == "" || Warehouse == null || Warehouse == "0") {
        clickedrow.find("#wh_Error").text($("#valueReq").text());
        clickedrow.find("#wh_Error").css("display", "block");
        clickedrow.find("#wh_id_" + Sno).css("border-color", "red");
    }
    else {
        clickedrow.find("#wh_Error").css("display", "none");
        clickedrow.find("#wh_id_" + Sno).css("border-color", "#ced4da");
    }
}

function BindSR_ItemList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();///Amount
    var clickedrow = $(e.target).closest("tr");
    var Itm_ID;
    var SNo = clickedrow.find("#hfSNo").val();

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
    AfterDeleteResetBatchAndSerialDetails(Itm_ID);
    //HideItemListItm();
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
                    if (arr.Table.length > 0) {
                        clickedrow.find("#UOM").val(arr.Table[0].uom_alias);
                        clickedrow.find("#UOMID").val(arr.Table[0].uom_id);
                        //clickedrow.find("#hfbatchable").val(arr.Table[0].i_batch);
                        //clickedrow.find("#hfserialable").val(arr.Table[0].i_serial);
                        //clickedrow.find("#hfexpiralble").val(arr.Table[0].i_exp);

                        //if (arr.Table[0].i_batch == "Y") {
                        //    clickedrow.find("#BtnBatchDetail").attr("disabled", false);
                        //}
                        //else {
                        //    clickedrow.find("#BtnBatchDetail").attr("disabled", true);
                        //}
                        //if (arr.Table[0].i_serial == "Y") {
                        //    clickedrow.find("#BtnSerialDetail").attr("disabled", false);
                        //}
                        //else {
                        //    clickedrow.find("#BtnSerialDetail").attr("disabled", true);
                        //}
                    }
                    else {
                        clickedrow.find("#UOM").val("");
                        //clickedrow.find("#BtnSerialDetail").attr("disabled", true);
                        //clickedrow.find("#BtnBatchDetail").attr("disabled", true);
                    }
                }
            },
        });
    } catch (err) {
    }
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
    //ClearBatch_SerialDetails();
}
function DisableHeaderField() {
    debugger;
    $("#EntityType").attr('disabled', true);
    $("#EntityName").attr('disabled', true);
}
function OtherFunctions(StatusC, StatusName) {
    $("#hfStatus").val(StatusC);
    $("#SRStatus").text(StatusName);
    $("#Btn_Delete").attr('onclick', '');
    $("#Btn_Delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $("#Btn_Approve").attr('onclick', '');
    $("#Btn_Approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $("#Btn_Edit").attr('onclick', '');
    $("#Btn_Edit").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    $("#Btn_Forward").attr("data-target", "");
    //$("#Btn_Forward").attr('onclick', '');
    $("#Btn_Forward").prop('disabled', true);
    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
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
function AfterDeleteResetBatchAndSerialDetails(ItemCode) {
    let NewArr = [];
    $("#BatchDetailTbl >tbody >tr").remove();
    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails != null) {
        if (FBatchDetails.length > 0) {
            for (i = 0; i < FBatchDetails.length; i++) {
                var SItemID = FBatchDetails[i].ItemID;
                if (SItemID == ItemCode) {
                }
                else {
                    NewArr.push(FBatchDetails[i]);
                }
            }
            sessionStorage.removeItem("BatchDetailSession");
            sessionStorage.setItem("BatchDetailSession", JSON.stringify(NewArr));
        }
    }
    $("#SerialDetailTbl >tbody >tr").remove();
    let SNewArr = [];
    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails != null) {
        if (FSerialDetails.length > 0) {
            for (i = 0; i < FSerialDetails.length; i++) {
                var SItemID = FSerialDetails[i].ItemID;
                if (SItemID == ItemCode) {
                }
                else {
                    SNewArr.push(FSerialDetails[i]);
                }
            }
            sessionStorage.removeItem("SerialDetailSession");
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SNewArr));
        }
    }
}
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var SRNo = "";
    var SR_Date = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";

    Remarks = $("#fw_remarks").val();
    SRNo = $("#SR_Number").val();
    SR_Date = $("#SR_Date").val();
    docid = $("#DocumentMenuId").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && SRNo != "" && SR_Date != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(SRNo, SR_Date, docid, level, forwardedto, fwchkval, Remarks);
        }
    }
    if (fwchkval === "Approve") {
        Approve_SampleReceipt("Approve", $("#hd_currlevel").val(), Remarks);
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && SRNo != "" && SR_Date != "") {
            Cmn_InsertDocument_ForwardedDetail(SRNo, SR_Date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);

        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && SRNo != "" && SR_Date != "") {
            Cmn_InsertDocument_ForwardedDetail(SRNo, SR_Date, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
        }
    }
}
//-----------------End------------------------------//

//---------------Batch Deatils-----------------------//
function OnClickBatchDetailBtn(e) {
    debugger;

    Cmn_OnClickBatchDetailBtn(e, "#hfSNo", "#ItemName_", "#hf_ItemID", "#UOM", "#hfexpiralble", "#ReceivedQuantity", "SREnableDisable", "");
}

function OnClickSaveAndClose() {

    Cmn_OnClickSaveAndClose("#SR_ItmDetailsTbl", "#hf_ItemID");
}
//--------------------End--------------------------//

//---------------Serial Deatils-----------------------//
function OnClickSerialDetailBtn(e) {
    debugger;
    Cmn_OnClickSerialDetailBtn(e, "#hfSNo", "#ItemName_", "#hf_ItemID", "#UOM", "#ReceivedQuantity", "SREnableDisable", "");
}

//--------------------End----------------------------//